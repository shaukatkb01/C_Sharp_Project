using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace FileIndex
{

    public partial class DistributionList : Form
    {
        // for filedorp down 
        private void FillFileDropdown()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // Aapki purani query jo NOT IN use karti hai
                string query = @"SELECT 
                    F.Id, 
                    F.FileNo,
                    C.Id AS C_Id, C.IssueNo,
                    (F.FileNo + ' - ' + ISNULL(C.IssueNo, '') + ' - ' + ISNULL(F.FileSubject, '')) AS Filedetail
                 FROM FileIndex F
                 INNER JOIN CommStamp C ON F.Id = C.FileNo 
                 WHERE F.FileType IN (1,2,3,4)
                 AND F.Id NOT IN (SELECT FileNo FROM PhilatelicSupply WHERE FileNo IS NOT NULL) 
                 AND F.Id NOT IN (SELECT FileNo FROM InvoiceRegister WHERE FileNo IS NOT NULL)
                 AND F.Id IN (SELECT FileNo FROM Price)
                 ORDER BY C.Id DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                combFileNo.DataSource = dt;
                combFileNo.DisplayMember = "Filedetail";
                combFileNo.ValueMember = "Id";

                // Selection khali karna
                combFileNo.SelectedIndex = -1;
            }
        }
        // clear form funcation
        private void ClearFormControls()
        {
            // 1 se 24 tak loop chalayein (taake saari rows clear ho jayein)
            for (int i = 1; i <= 24; i++)
            {
                // TextBox ko dhoond kar khali karein
                var txt = this.Controls.Find($"text_{i}_TA", true).FirstOrDefault() as TextBox;
                if (txt != null) txt.Clear();

                // NumericUpDown ko dhoond kar 0 karein
                var nums = new string[] { "Stamp", "FDC", "Leaflet", "FDCCanccelled", "Postmark" };
                foreach (var name in nums)
                {
                    var n = this.Controls.Find($"num_{i}_{name}", true).FirstOrDefault() as NumericUpDown;
                    if (n != null) n.Value = 0;
                }

                // ComboBox ko dhoond kar unselect karein
                var drops = new string[] { "DisType" };
                foreach (var dName in drops)
                {
                    var cb = this.Controls.Find($"drop_{i}_{dName}", true).FirstOrDefault() as ComboBox;
                    if (cb != null) cb.SelectedIndex = -1; // -1 ka matlab "Nothing Selected"
                }
            }

            // Agar main FileNo wala combo bhi clear karna hai:
            combFileNo.SelectedIndex = -1;
        }
        // Invoice number function
        private string GetNextInvoiceNoWithConnection(SqlConnection con, SqlTransaction trans)
        {
            string currentYear = DateTime.Now.Year.ToString();
            string prefix = "Ps.";

            // Sab se latest number uthao (chahe wo abhi isi transaction mein save hua ho)
            string query = "SELECT TOP 1 InvoiceNo FROM InvoiceRegister WITH (UPDLOCK, HOLDLOCK) WHERE InvoiceNo LIKE '%/" + currentYear + "' ORDER BY InvoiceNo DESC";

            SqlCommand cmd = new SqlCommand(query, con, trans);
            object result = cmd.ExecuteScalar();

            int nextNum = 1;

            if (result != null && result != DBNull.Value)
            {
                string lastInvoice = result.ToString();
                int dotIndex = lastInvoice.IndexOf('.');
                int slashIndex = lastInvoice.IndexOf('/');

                string numberPart = lastInvoice.Substring(dotIndex + 1, slashIndex - dotIndex - 1);
                nextNum = int.Parse(numberPart) + 1;
            }

            return prefix + nextNum.ToString("D3") + "/" + currentYear;
        }

        //sfcQTy for Stamp of Coast, fQty FDC, lQty Leaflet,pmQty Postmark
        // Class level variables
        int sfcQty = 0, fQty = 0, lQty = 0, pmQty = 0;

        private void UpdateRemainingStock()
        {
            // 1. Temporary totals (Zero se shuru karein)
            decimal totalUsed_s = 0;
            decimal totalUsed_f = 0;
            decimal totalUsed_l = 0;
            decimal totalUsed_pm = 0;
            decimal totalUsed_fc = 0;

            // 2. Loop 1 se 41 tak chala kar data jama karein
            for (int i = 1; i <= 41; i++)
            {
                // Skip unwanted rows
                if (i == 21 || i == 22 || i == 23)
                {
                    continue;
                }

                // Controls ko dhoondna (Find controls by name)
                var nS = this.Controls.Find($"num_{i}_Stamp", true).FirstOrDefault() as NumericUpDown;
                var nF = this.Controls.Find($"num_{i}_FDC", true).FirstOrDefault() as NumericUpDown;
                var nL = this.Controls.Find($"num_{i}_Leaflet", true).FirstOrDefault() as NumericUpDown;
                var nP = this.Controls.Find($"num_{i}_Postmark", true).FirstOrDefault() as NumericUpDown;
                var nFC = this.Controls.Find($"num_{i}_FDCCanccelled", true).FirstOrDefault() as NumericUpDown;

                // 3. i == 26 par logic badalna (Plus/Minus adjustment)
                if (i == 20)
                {
                    // Agar i=26 hai toh totalUsed mein se minus karein 
                    // (Taake Main Stock - (-Value) = Main Stock + Value ban jaye)
                    if (nS != null) totalUsed_s -= nS.Value;
                    if (nF != null) totalUsed_f -= nF.Value;
                    if (nL != null) totalUsed_l -= nL.Value;
                    if (nP != null) totalUsed_pm -= nP.Value;
                    if (nFC != null) totalUsed_fc -= nFC.Value;
                }
                else
                {
                    // Baaki sab cases mein totalUsed mein plus karein (Yani stock kharch ho raha hai)
                    if (nS != null) totalUsed_s += nS.Value;
                    if (nF != null) totalUsed_f += nF.Value;
                    if (nL != null) totalUsed_l += nL.Value;
                    if (nP != null) totalUsed_pm += nP.Value;
                    if (nFC != null) totalUsed_fc += nFC.Value;
                }
            }

            // 4. Final Calculation (Main Stock minus Total Used/Adjusted)
            decimal remaining_s = sfcQty - totalUsed_s;
            decimal remaining_f = fQty - totalUsed_f;
            decimal remaining_l = lQty - totalUsed_l;
            decimal remaining_pm = pmQty - totalUsed_pm;

            // Note: num_20_FDC ko aapne as a base use kiya hai
            decimal remaining_fc = num_20_FDC.Value - totalUsed_fc;

            // 5. UI par Display karein
            text_Ba_St.Text = remaining_s.ToString();
            text_Ba_FDC.Text = remaining_f.ToString();
            text_Ba_Leaflet.Text = remaining_l.ToString();
            text_Ba_PM.Text = remaining_pm.ToString();
            text_Ba_Fc.Text = remaining_fc.ToString();

            // 6. Warning Colors (Red if stock is low or negative)
            text_Ba_St.ForeColor = (remaining_s < 200) ? Color.Red : Color.Black;
            text_Ba_FDC.ForeColor = (remaining_f < 200) ? Color.Red : Color.Black;
            text_Ba_Leaflet.ForeColor = (remaining_l < 200) ? Color.Red : Color.Black;
            text_Ba_PM.ForeColor = (remaining_pm < 0) ? Color.Red : Color.Black;
            text_Ba_Fc.ForeColor = (remaining_fc < 0) ? Color.Red : Color.Black;
        }
        public DistributionList()
        {
            InitializeComponent();
        }

        //FDC Leaflet and Postmark Total Function
        private void CalculateRowTotal(int rowNum)
        {
            try
            {
                // 1. Dynamic tareeqe se controls ko dhoondna
                var numFDC = this.Controls.Find($"num_{rowNum}_FDC", true).FirstOrDefault() as NumericUpDown;
                var numLeaflet = this.Controls.Find($"num_{rowNum}_Leaflet", true).FirstOrDefault() as NumericUpDown;
                var numPostmark = this.Controls.Find($"num_{rowNum}_Postmark", true).FirstOrDefault() as NumericUpDown;
                var txtTA = this.Controls.Find($"text_{rowNum}_TA", true).FirstOrDefault() as TextBox;

                // 2. Prices ko Convert karna (Agar prices fix hain toh direct likhein, warna textbox se lein)
                decimal fPrice = string.IsNullOrEmpty(textFDCPrice.Text) ? 0 : Convert.ToDecimal(textFDCPrice.Text);
                decimal lPrice = string.IsNullOrEmpty(textLeafletPrice.Text) ? 0 : Convert.ToDecimal(textLeafletPrice.Text);
                decimal pmPrice = string.IsNullOrEmpty(textPostmarkPrice.Text) ? 0 : Convert.ToDecimal(textPostmarkPrice.Text);

                // 3. Formula (Agar controls mil gaye hain)
                if (numFDC != null && numLeaflet != null && numPostmark != null && txtTA != null)
                {
                    decimal totalAmount = (numFDC.Value * fPrice) +
                                         (numLeaflet.Value * lPrice) +
                                         (numPostmark.Value * pmPrice);

                    // 4. Result ko Total Amount wale box mein dikhana
                    txtTA.Text = totalAmount.ToString("N2"); // "N2" se 0.00 format mein aayega

                    // Calculation ke baad stock update karne wala function call karein
                    UpdateRemainingStock();
                }
            }
            catch { /* Error handling */ }
        }








        private void DistributionList_Load(object sender, EventArgs e)
        {

            UIHelper.SetFormTheme(this);
            UIHelper.ApplyTheme(this);
            FillFileDropdown();


            drop_26_Phil.SelectedValue = 20;



            // Form ke har control ko check karein
            foreach (Control c in this.Controls)
            {
                // Pehle check karein ke kya ye NumericUpDown hai
                if (c is NumericUpDown)
                {
                    // Control 'c' ko NumericUpDown mein convert karke variable banayein
                    NumericUpDown numControl = (NumericUpDown)c;

                    // Ab Tag ya Naam check karein
                    if (numControl.Tag?.ToString() == "StockControl" || numControl.Name.StartsWith("num_"))
                    {
                        // Ab ValueChanged chalega kyunki 'numControl' ko pata hai wo NumericUpDown hai
                        numControl.ValueChanged += (s, ev) =>
                        {
                            // Row number nikalne ka logic
                            string[] parts = numControl.Name.Split('_');
                            if (parts.Length >= 2 && int.TryParse(parts[1], out int rowNo))
                            {
                                CalculateRowTotal(rowNo);
                            }

                            // Stock update karein
                            UpdateRemainingStock();
                        };
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();

                //// for File drop down
                //string query = @"SELECT 
                //    F.Id, 
                //    F.FileNo,
                //    C.Id AS C_Id, C.IssueNo,
                //    (F.FileNo + ' - ' + ISNULL(C.IssueNo, '') + ' - ' + ISNULL(F.FileSubject, '')) AS Filedetail
                // FROM FileIndex F
                // INNER JOIN CommStamp C ON F.Id = C.FileNo 
                // WHERE F.FileType IN (1,2,3,4)
                // AND F.Id NOT IN (SELECT FileNo FROM PhilatelicSupply WHERE FileNo IS NOT NULL) 
                // AND F.Id NOT IN (SELECT FileNo FROM InvoiceRegister WHERE FileNo IS NOT NULL)
                // AND F.Id IN (SELECT FileNo FROM Price)
                // ORDER BY C.Id DESC";// Yahan FileId column ka naam check kar lein

                //SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                //DataTable dt = new DataTable();
                //adapter.Fill(dt);

                //combFileNo.DataSource = dt;
                //combFileNo.DisplayMember = "Filedetail"; // 2. '=' lagana zaroori hai
                //combFileNo.ValueMember = "Id";            // 3. Background ID ke liye ye lazmi hai

                //combFileNo.SelectedIndex = -1;           // 4. Default khali rakhne ke liye -1 hota hai

                // 1. Pehle data mangwa lein (Sirf ek dafa)
                string query1 = "SELECT ID, Dispatchtype FROM DispatchType";
                SqlDataAdapter adapter1 = new SqlDataAdapter(query1, con);
                DataTable dt1 = new DataTable();
                adapter1.Fill(dt1);

                // 2. Loop ke zariye 20 dropdowns ko dhoond kar data dein
                for (int i = 1; i <= 41; i++)
                {
                    // Dropdown ka naam dhoondna (e.g., drop_1_DisType, drop_2_DisType...)
                    var combo = this.Controls.Find($"drop_{i}_DisType", true).FirstOrDefault() as ComboBox;

                    if (combo != null)
                    {
                        // Har dropdown ko usi DataTable ki copy dena
                        combo.DisplayMember = "Dispatchtype";
                        combo.ValueMember = "ID";
                        combo.DataSource = new BindingSource(dt1, null); // BindingSource use karna behtar hai loop mein
                        combo.SelectedIndex = -1; // Default khali rakhne ke liye


                    }
                }

                // 1. Data load karein (Sirf ek dafa)
                string query2 = @"SELECT Id, PhilitelicBuearuName FROM PhilitelicBuearu";
                SqlDataAdapter adapter2 = new SqlDataAdapter(query2, con);
                DataTable dt2 = new DataTable();
                adapter2.Fill(dt2);


                for (int i = 1; i <= 41; i++)
                {
                    drop_26_Phil.SelectedValue = 20;
                    drop_25_Phil.SelectedValue = 41;
                    var combo = this.Controls.Find($"drop_{i}_Phil", true).FirstOrDefault() as ComboBox;
                    if (combo != null)
                    {
                        combo.DisplayMember = "PhilitelicBuearuName";
                        combo.ValueMember = "Id";
                        combo.DataSource = new BindingSource(dt2, null);

                        // --- ALAG VALUE SET KARNE KA LOGIC ---

                        // Agar aap chahte hain ke Row 1 mein list ka 0 item ho, Row 2 mein 1 item...
                        if (dt2.Rows.Count >= i)
                        {
                            combo.SelectedIndex = i - 1; // i=1 hai toh index 0 (Pehla shehar)
                        }
                        else
                        {
                            combo.SelectedIndex = -1; // Agar shehar khatam ho jayein toh khali rakhein
                        }
                    }
                }

            }
        }

        private void combFileNo_DropDown(object sender, EventArgs e)
        {
            // User ko dhoondne mein asani ho, isliye lambi detail dikhayein
            //combFileNo.DisplayMember = "Fildetail";
        }
        private void combFileNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Safety check
            if (combFileNo.SelectedValue == null || combFileNo.SelectedValue is DataRowView)
                return;
            button1.Enabled = true;

            decimal fPrice = 0, lPrice = 0, pmPrice = 0;


            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = "SELECT * FROM Price WHERE FileNo = @No";

                SqlCommand cmd = new SqlCommand(query, con);


                // FIX 1: SelectedValue ko explicit string ya int mein convert karein (SQL matching ke liye)
                cmd.Parameters.AddWithValue("@No", Convert.ToString(combFileNo.SelectedValue));

                try
                {
                    // FIX 2: Connection open karna bhool gaye thay, wo lazmi hai
                    if (con.State == ConnectionState.Closed) con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        fPrice = reader["FDCPrice"] != DBNull.Value ? Convert.ToDecimal(reader["FDCPrice"]) : 0;
                        lPrice = reader["LeafletPrice"] != DBNull.Value ? Convert.ToDecimal(reader["LeafletPrice"]) : 0;
                        pmPrice = reader["PostmarkPrice"] != DBNull.Value ? Convert.ToDecimal(reader["PostmarkPrice"]) : 0;

                        sfcQty = reader["StampFC"] != DBNull.Value ? Convert.ToInt32(reader["StampFC"]) : 0;
                        fQty = reader["FDCQty"] != DBNull.Value ? Convert.ToInt32(reader["FDCQty"]) : 0;
                        lQty = reader["LeafletQty"] != DBNull.Value ? Convert.ToInt32(reader["LeafletQty"]) : 0;
                        pmQty = reader["PostmarkQty"] != DBNull.Value ? Convert.ToInt32(reader["PostMarkQty"]) : 0;


                        textFDCPrice.Text = fPrice.ToString("0.00");
                        textLeafletPrice.Text = lPrice.ToString("0.00");
                        textPostmarkPrice.Text = pmPrice.ToString("0.00");

                        num_21_Stamp.Value = sfcQty;
                        num_22_FDC.Value = fQty;
                        num_22_Leaflet.Value = lQty;
                        num_23_Postmark.Value = pmQty;


                        CalculateRowTotal(1);
                    }
                    else
                    {
                        // FIX 3: Agar data na mile toh boxes khali ho jayein (purana data na rahay)
                        textFDCPrice.Text = "0.00";
                        textLeafletPrice.Text = "0.00";
                        textPostmarkPrice.Text = "0.00";

                        num_21_Stamp.Value = 0;
                        num_22_FDC.Value = 0;
                        num_22_Leaflet.Value = 0;
                        num_23_Postmark.Value = 0;


                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // --- HISSA 1: Rows 1 to 19 (InvoiceRegister) ---
                    for (int i = 1; i <= 19; i++)
                    {
                        var dropPhil = this.Controls.Find($"drop_{i}_Phil", true).FirstOrDefault() as ComboBox;
                        var txtTA = this.Controls.Find($"text_{i}_TA", true).FirstOrDefault() as TextBox;
                        var dropDis = this.Controls.Find($"drop_{i}_DisType", true).FirstOrDefault() as ComboBox;

                        if (dropDis != null && dropDis.SelectedValue != null && !string.IsNullOrWhiteSpace(txtTA.Text))
                        {
                            string currentInvoiceNo = GetNextInvoiceNoWithConnection(con, trans);

                            string query = @"INSERT INTO InvoiceRegister 
                        (PhiliticBureauName, Totalamount, SupplyType, DispatchType, FileNo, InvoiceNo, IssueDate) 
                        VALUES (@phil, @amount, @ST, @dis, @file, @inv, @date)";

                            using (SqlCommand cmd = new SqlCommand(query, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@phil", dropPhil.SelectedValue);
                                cmd.Parameters.AddWithValue("@amount", decimal.Parse(txtTA.Text));
                                cmd.Parameters.AddWithValue("@dis", dropDis.SelectedValue);
                                cmd.Parameters.AddWithValue("@file", combFileNo.SelectedValue);
                                cmd.Parameters.AddWithValue("@inv", currentInvoiceNo);
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@ST", 2);
                                cmd.ExecuteNonQuery();
                            }
                            // 2. IssueMaleList mein entry (Sirf 1 to 19 ke liye)
                            string query2 = @"INSERT INTO IssueMaleList
                        (MailListFileId, Address, DispatchType)
                        VALUES (@mf, @ad, @dt)";
                            if (i != 2)
                            {

                                using (SqlCommand cmd2 = new SqlCommand(query2, con, trans))
                                {
                                    cmd2.Parameters.AddWithValue("@mf", combFileNo.SelectedValue);
                                    cmd2.Parameters.AddWithValue("@ad", dropPhil.SelectedValue);
                                    cmd2.Parameters.AddWithValue("@dt", dropDis?.SelectedValue ?? DBNull.Value);
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }


                    }

                    // --- HISSA 2: Rows 20 to 26 (PhilatelicSupply) ---
                    // Yahan i=20 se shuru karein taake koi row miss na ho
                    for (int i = 1; i <= 26; i++)
                    {

                        var dropPhil = this.Controls.Find($"drop_{i}_Phil", true).FirstOrDefault() as ComboBox;
                        var numStamp = this.Controls.Find($"num_{i}_Stamp", true).FirstOrDefault() as NumericUpDown;
                        var numFDC = this.Controls.Find($"num_{i}_FDC", true).FirstOrDefault() as NumericUpDown;
                        var numLeaflet = this.Controls.Find($"num_{i}_Leaflet", true).FirstOrDefault() as NumericUpDown;
                        var numPM = this.Controls.Find($"num_{i}_Postmark", true).FirstOrDefault() as NumericUpDown;
                        var numFDCC = this.Controls.Find($"num_{i}_FDCCanccelled", true).FirstOrDefault() as NumericUpDown;


                        // Check: Agar Bureau select hai aur koi quantity (Stamp) 0 se zyada hai
                        if (dropPhil != null && dropPhil.SelectedValue != null && numStamp != null)
                        {
                            string query2 = @"INSERT INTO PhilatelicSupply
                        (Address, FileNo, SupplyType, Supply_Date, StampsQty, FDCQty, LeafletQty, FDCCQty, PostmarkQty)
                        VALUES (@phil, @file, @rstype, @date, @sq, @fq, @lq, @fcq, @pmq)";

                            using (SqlCommand cmd = new SqlCommand(query2, con, trans))
                            {

                                cmd.Parameters.AddWithValue("@phil", dropPhil.SelectedValue);
                                cmd.Parameters.AddWithValue("@file", combFileNo.SelectedValue);
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@sq", numStamp.Value);
                                cmd.Parameters.AddWithValue("@fq", numFDC.Value);
                                cmd.Parameters.AddWithValue("@lq", numLeaflet.Value);
                                cmd.Parameters.AddWithValue("@fcq", numFDCC.Value);
                                cmd.Parameters.AddWithValue("@pmq", numPM.Value);

                                // --- AAPKA TYPE LOGIC ---
                                // 20 se 23 tak Type 1, baaki (24, 25, 26) ke liye Type 2
                                if (i >= 20 && i <= 23)
                                {
                                    cmd.Parameters.AddWithValue("@rstype", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@rstype", 2);
                                }

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    trans.Commit();
                    MessageBox.Show("All 26 records processed and saved successfully!");
                    FillFileDropdown();
                    ClearFormControls();
                    button1.Enabled = false;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Saving Error: " + ex.Message);
                }
            }
        }

        private void combFileNo_DropDownClosed(object sender, EventArgs e)
        {
            // Jab dropdown band ho, toh check karein kuch select hua hai?
            if (combFileNo.SelectedItem is DataRowView drv)
            {
                // Box ke andar ka text zabardasti sirf FileNo kar dein
                combFileNo.Text = drv["FileNo"].ToString();
            }
        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void drop_20_Phil_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

