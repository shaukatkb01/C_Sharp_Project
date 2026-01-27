using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIndex
{

    public partial class InvoiceEntrycs : Form
    {
        // for InvoiceNo
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

        int selectedFileId = 0;// for FileNo
        private void CalculateTotal()
        {
            try
            {
                // 1. Text se sirf Numbers nikalne ka safe tareeka
                // Hum "Rs. " ko mita kar khali jagah (trim) khatam kar rahe hain
                string fdcText = text_FDCPrice.Text.Replace("Rs.", "").Trim();
                string leafText = text_LeafletPrice.Text.Replace("Rs.", "").Trim();
                string pmText = text_PMPrice.Text.Replace("Rs.", "").Trim();

                // 2. Agar koi box khali hai to 0 le lo (decimal.TryParse behtar hai)
                decimal fp, lp, pmp;
                decimal.TryParse(fdcText, out fp);
                decimal.TryParse(leafText, out lp);
                decimal.TryParse(pmText, out pmp);

                // 3. NumericUpDown se quantities (Ye hamesha number hi deta hai)
                decimal fq = num_FDC.Value;
                decimal lq = num_Leaflet.Value;
                decimal pmq = num_Postmark.Value;

                // 4. Calculation
                decimal total = (fq * fp) + (lq * lp) + (pmq * pmp);

                // 5. Result show karein
                text_TA.Text = $"Rs. {total:N2}";
            }
            catch (Exception ex)
            {
                // Agar koi aur error ho to message box dikhaye
                // MessageBox.Show(ex.Message); 
                text_TA.Text = "Rs. 0.00";
            }
        }
        public InvoiceEntrycs()
        {
            InitializeComponent();
        }
        string lastInvoiceNo;
        private void InvoiceEntrycs_Load(object sender, EventArgs e)
        {
            UIHelper.SetFormTheme(this);
            UIHelper.ApplyTheme(this);

            // for last invoiceNo
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = @"SELECT TOP 1 InvoiceNo 
                     FROM InvoiceRegister 
                     ORDER BY Id DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    con.Open(); // Sirf aik baar open karein
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        lastInvoiceNo = "";
                        lastInvoiceNo = lastInvoice_No.Text = result.ToString();
                    }
                    else
                    {
                        // Agar table khali hai toh "N/A" ya "0" likhein
                        lastInvoice_No.Text = "No Record";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Asal Error ye hai: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    lastInvoice_No.Text = "Error";
                }
            }

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    con.Open();

                    // 1. Mukammal SQL Query (Table ka naam 'FileIndex' farz kiya hai)
                    // SQL mein '+' ki jagah ' + ' spaces ke sath use karein
                    string query = @"SELECT F.Id, (F.FileNo + ' - ' + F.FileSubject) AS Filedetail 
                 FROM FileIndex F
                 INNER JOIN CommStamp C ON F.Id = C.FileNo
                 WHERE F.FileType IN (1, 2, 3, 4) 
                 GROUP BY F.Id, F.FileNo, F.FileSubject -- Takay duplicate records na aayein
                 ORDER BY F.Id DESC";

                    // 2. Adapter ko query aur connection dena zaroori hai
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    string query2 = @"SELECT Id, PhilitelicBuearuName 
                                      FROM PhilitelicBuearu";
                    SqlDataAdapter adapter2 = new SqlDataAdapter(query2, con);
                    DataTable dt2 = new DataTable();
                    adapter2.Fill(dt2);

                    string query3 = @" SELECT ID,DispatchType
                                   FROM DispatchType";
                    SqlDataAdapter adapter3 = new SqlDataAdapter(query3, con);
                    DataTable dt3 = new DataTable();
                    adapter3.Fill(dt3);

                    //string query4 = @"SELECT ID, SupplyType
                    //                FROM SupplyType
                    //                WHERE ID IN (3,4)";
                    //SqlDataAdapter adapter4 = new SqlDataAdapter(query4, con);
                    //DataTable dt4 = new DataTable();
                    //adapter4.Fill(dt4);

                    // 3. ComboBoxs ko data dena
                    if (dt.Rows.Count > 0 && dt2.Rows.Count > 0 && dt3.Rows.Count > 0) // Check karein ke kya database se data aaya?
                    {
                        // for FileNo Combo
                        com_FileNo.DataSource = dt;
                        com_FileNo.DisplayMember = "Filedetail"; // Jo user ko dikhega
                        com_FileNo.ValueMember = "F.Id";          // Jo background mein save hoga
                        com_FileNo.SelectedIndex = -1; // Shuru mein khali dikhane ke liye -1 behtar hai
                        com_FileNo.DropDownWidth = 1500;

                        // for address combo
                        com_PhilName.DataSource = dt2;
                        com_PhilName.DisplayMember = "PhilitelicBuearuName";
                        com_PhilName.ValueMember = "Id";
                        com_PhilName.SelectedIndex = -1;
                        com_PhilName.DropDownWidth = 500;

                        // for dispatchType combo
                        combo_DT.DataSource = dt3;
                        combo_DT.DisplayMember = "DispatchType";
                        combo_DT.ValueMember = "ID";
                        combo_DT.SelectedIndex = -1;

                        //// for supply type combo
                        //com_ST.DataSource= dt4;
                        //com_ST.DisplayMember = "SupplyType";
                        //com_ST.ValueMember = "ID";
                        //com_ST.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

       



        private void num_FDC_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void num_Leaflet_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void num_Postmark_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void num_FDCC_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (ValidationHelper.IsFormValid(this) == false) return;

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string newInvoiceNo = GetNextInvoiceNoWithConnection(con, trans);

                    // 2. InvoiceRegister Query (DispatchType column add kiya)
                    string query = @"INSERT INTO InvoiceRegister 
                    (FileNo, InvoiceNo, SupplyType, PhiliticBureauName, IssueDate, Totalamount, Remarks) 
                    VALUES (@fno, @inv, @st, @pbn, @dt, @ta, @remarks, );
                    SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, con, trans);
                    cmd.Parameters.AddWithValue("@fno", selectedFileId);
                    cmd.Parameters.AddWithValue("@inv", newInvoiceNo);
                    cmd.Parameters.AddWithValue("@st", 4);
                    cmd.Parameters.AddWithValue("@pbn", com_PhilName.SelectedValue);
                    cmd.Parameters.AddWithValue("@dt", date_Picker.Value.Date);

                    decimal finalTotal = decimal.Parse(text_TA.Text.Replace("Rs. ", ""));
                    cmd.Parameters.AddWithValue("@ta", finalTotal);
                    cmd.Parameters.AddWithValue("@remarks", Remark_txt.Text);


                    int newInvoiceNoId = Convert.ToInt32(cmd.ExecuteScalar());

                    // 3. PhilatelicSupply Query
                    string query1 = @"INSERT INTO PhilatelicSupply
                    (Address, FileNo, SupplyType, Supply_Date, StampsQty, FDCQty, LeafletQty, FDCCQty, PostmarkQty, Remark, InvoiceNo)
                    VALUES (@ad, @fn, @st, @sd, @sq, @fq, @lq, @fcq, @pmq, @remark, @in)";

                    SqlCommand cmd1 = new SqlCommand(query1, con, trans);
                    cmd1.Parameters.AddWithValue("@ad", com_PhilName.SelectedValue);
                    cmd1.Parameters.AddWithValue("@fn", selectedFileId);
                    cmd1.Parameters.AddWithValue("@st", 4);
                    cmd1.Parameters.AddWithValue("@sd", date_Picker.Value);
                    cmd1.Parameters.AddWithValue("@sq", num_Stamp.Value);
                    cmd1.Parameters.AddWithValue("@fq", num_FDC.Value);
                    cmd1.Parameters.AddWithValue("@lq", num_Leaflet.Value);
                    cmd1.Parameters.AddWithValue("@fcq", num_FDCC.Value);
                    cmd1.Parameters.AddWithValue("@pmq", num_Postmark.Value);
                    cmd1.Parameters.AddWithValue("@remark", Remark_txt.Text);
                    cmd1.Parameters.AddWithValue("@in", newInvoiceNoId);
                    cmd1.ExecuteNonQuery();

                    // 4. PendingInvoice Query (Fixing @inv and @ac)
                    string query2 = @"INSERT INTO PendingInvoice 
                    (InvoiceRegisterId, AcknowledgeStatus, Remarks) 
                    VALUES (@invId, @ac, @rem)";

                    SqlCommand cmd2 = new SqlCommand(query2, con, trans);
                    cmd2.Parameters.AddWithValue("@invId", newInvoiceNoId); // Naam match kar diya
                    cmd2.Parameters.AddWithValue("@ac", 2); // '=' hata diya
                    cmd2.Parameters.AddWithValue("@rem", Remark_txt.Text);
                    cmd2.ExecuteNonQuery(); // Isay chalana bhool gaye thay aap

                    trans.Commit();

                    ClearForm.ClearAllControls(this);
                    MessageBox.Show("Invoice " + newInvoiceNo + " Saved Successfully!");
                    lastInvoice_No.Text = newInvoiceNo;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Save Error: " + ex.Message);
                }
            }
        }

        private void com_FileNo_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (com_FileNo.SelectedValue != null && com_FileNo.SelectedValue is int)
            {
                // stock remaining update karne ke liye
                int id = (int)com_FileNo.SelectedValue;


                StockManager.CalculateAndDisplayStock(id, text_Stamp_B, text_FDC_B, text_Leaflet_B, text_FDCC_B, text_PM_B);
            }
            // 1. Check karein ke user ne waqayi kuch select kiya hai
            if (com_FileNo.Focused && com_FileNo.SelectedValue != null)
            {
                selectedFileId = Convert.ToInt32(com_FileNo.SelectedValue);

                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    // Query mein hum wahi columns mangwa rahe hain jo humein chahiye
                    string query = @"SELECT FDCPrice, LeafletPrice, PostmarkPrice 
                             FROM Price 
                             WHERE FileNo = @FileID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@FileID", selectedFileId);

                    try
                    {
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read()) // Agar record mil jaye
                        {
                            // 2. TextBoxes mein data show karna
                            // .ToString() lazmi hai kyunke database se number aata hai aur box ko text chahiye
                            text_FDCPrice.Text = "Rs." + dr["FDCPrice"].ToString();
                            text_LeafletPrice.Text = "Rs." + dr["LeafletPrice"].ToString();
                            text_PMPrice.Text = "Rs." + dr["PostmarkPrice"].ToString();
                        }
                        else
                        {
                            // Agar us FileID ka data Price table mein na ho toh boxes khali kar dein
                            text_FDCPrice.Text = "0";
                            text_LeafletPrice.Text = "0";
                            text_PMPrice.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data Load Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
