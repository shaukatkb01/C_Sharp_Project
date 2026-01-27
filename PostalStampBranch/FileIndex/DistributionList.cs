using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class DistributionList : Form
    {
        int newInvoiceId = 0;
        int sfcQty = 0, fQty = 0, lQty = 0, pmQty = 0; // Class level variables
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
        public DistributionList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        // --- HELPER FUNCTION: Har jagah control dhoondne ke liye ---
        private Control GetControl(string name)
        {
            // 'true' ka matlab hai panels ke andar bhi dhoondo
            return this.Controls.Find(name, true).FirstOrDefault();
        }

        private void DistributionList_Load(object sender, EventArgs e)
        {
            UIHelper.SetFormTheme(this);
            UIHelper.ApplyTheme(this);
            FillFileDropdown();
            if(GlobalData.UerRole!="Admin")
            {
                combFileNo.Enabled= false;
                button1.Enabled= false;
            }
            // 1. NumericUpDown Events (Panels ke andar dhoond kar attach karna)
            AttachEventsToAllNumericControls(this);

            // 2. Database se Dropdowns load karna
            LoadAllDropdowns();
        }

        private void AttachEventsToAllNumericControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is NumericUpDown numControl && (numControl.Tag?.ToString() == "StockControl" || numControl.Name.StartsWith("num_")))
                {
                    numControl.ValueChanged += (s, ev) =>
                    {
                        string[] parts = numControl.Name.Split('_');
                        if (parts.Length >= 2 && int.TryParse(parts[1], out int rowNo))
                        {
                            CalculateRowTotal(rowNo);
                        }
                        UpdateRemainingStock();
                    };
                }
                if (c.HasChildren) AttachEventsToAllNumericControls(c);
            }
        }

        private void LoadAllDropdowns()
        {
            using (SqlConnection con = new (Db.ConString))
            {
                try
                {
                    con.Open();
                    // Dispatch Types
                    DataTable dt1 = new ();
                    new SqlDataAdapter("SELECT ID, Dispatchtype FROM DispatchType", con).Fill(dt1);

                    // Bureaus
                    DataTable dt2 = new ();
                    new SqlDataAdapter("SELECT Id, PhilitelicBuearuName FROM PhilitelicBuearu", con).Fill(dt2);

                    for (int i = 1; i <= 41; i++)
                    {
                        var comboDis = GetControl($"drop_{i}_DisType") as ComboBox;
                        if (comboDis != null)
                        {
                            comboDis.DisplayMember = "Dispatchtype";
                            comboDis.ValueMember = "ID";
                            comboDis.DataSource = new BindingSource(dt1, null);
                            comboDis.SelectedIndex = -1;
                        }

                        var comboPhil = GetControl($"drop_{i}_Phil") as ComboBox;
                        if (comboPhil != null)
                        {
                            comboPhil.DisplayMember = "PhilitelicBuearuName";
                            comboPhil.ValueMember = "Id";
                            comboPhil.DataSource = new BindingSource(dt2, null);
                            if (dt2.Rows.Count >= i) comboPhil.SelectedIndex = i - 1;
                            else comboPhil.SelectedIndex = -1;
                        }
                    }

                    var d26 = GetControl("drop_26_Phil") as ComboBox;
                    if (d26 != null) d26.SelectedValue = 24;
                }
                catch (Exception ex) { MessageBox.Show("Drop Error: " + ex.Message); }
            }
        }

        private void FillFileDropdown()
        {
            using (SqlConnection con = new (Db.ConString))
            {
                string query = @"SELECT F.Id, F.FileNo, C.IssueNo, 
                               CONCAT(F.FileNo, ' - ', ISNULL(C.IssueNo, ''), ' - ', ISNULL(F.FileSubject, '')) AS Filedetail
                               FROM FileIndex F
                               INNER JOIN CommStamp C ON F.Id = C.FileNo 
                               WHERE F.FileType IN (1,2,3,4) AND F.Status = 1
                               AND NOT EXISTS (SELECT 1 FROM PhilatelicSupply P WHERE P.FileNo = F.Id) 
                               AND NOT EXISTS (SELECT 1 FROM InvoiceRegister I WHERE I.FileNo = F.Id)
                               AND EXISTS (SELECT 1 FROM StockPrice PR WHERE PR.FileNo = F.Id)
                               ORDER BY C.IssueId DESC";

                SqlDataAdapter da = new (query, con);
                DataTable dt = new ();
                da.Fill(dt);
                combFileNo.DataSource = dt;
                combFileNo.DisplayMember = "Filedetail";
                combFileNo.ValueMember = "Id";
                combFileNo.SelectedIndex = -1;
            }
        }

        private void UpdateRemainingStock()
        {
            decimal tS = 0, tF = 0, tL = 0, tPM = 0, tFC = 0;

            for (int i = 1; i <= 41; i++)
            {
                if (i == 21 || i == 22 || i == 23) continue;

                var nS = GetControl($"num_{i}_Stamp") as NumericUpDown;
                var nF = GetControl($"num_{i}_FDC") as NumericUpDown;
                var nL = GetControl($"num_{i}_Leaflet") as NumericUpDown;
                var nP = GetControl($"num_{i}_Postmark") as NumericUpDown;
                var nFC = GetControl($"num_{i}_FDCCanccelled") as NumericUpDown;

                decimal valS = nS?.Value ?? 0;
                decimal valF = nF?.Value ?? 0;
                decimal valL = nL?.Value ?? 0;
                decimal valP = nP?.Value ?? 0;
                decimal valFC = nFC?.Value ?? 0;

                if (i == 24) // Adjustment Row
                {
                    tS -= valS; tF -= valF; tL -= valL; tPM -= valP; tFC -= valFC;
                }
                else
                {
                    tS += valS; tF += valF; tL += valL; tPM += valP; tFC += valFC;
                }
            }

            text_Ba_St.Text = (sfcQty - tS).ToString();
            text_Ba_FDC.Text = (fQty - tF).ToString();
            text_Ba_Leaflet.Text = (lQty - tL).ToString();
            text_Ba_PM.Text = (pmQty - tPM).ToString();

            // num_20_FDC aapka base hai
            if (num_20_FDC != null) text_Ba_Fc.Text = (num_20_FDC.Value - tFC).ToString();
        }

        private void CalculateRowTotal(int rowNum)
        {
            var nF = GetControl($"num_{rowNum}_FDC") as NumericUpDown;
            var nL = GetControl($"num_{rowNum}_Leaflet") as NumericUpDown;
            var nP = GetControl($"num_{rowNum}_Postmark") as NumericUpDown;
            var tA = GetControl($"text_{rowNum}_TA") as TextBox;

            decimal fP = decimal.TryParse(textFDCPrice.Text, out decimal d1) ? d1 : 0;
            decimal lP = decimal.TryParse(textLeafletPrice.Text, out decimal d2) ? d2 : 0;
            decimal pP = decimal.TryParse(textPostmarkPrice.Text, out decimal d3) ? d3 : 0;

            if (nF != null && nL != null && nP != null && tA != null)
            {
                tA.Text = ((nF.Value * fP) + (nL.Value * lP) + (nP.Value * pP)).ToString("N2");
            }
        }

        private void ClearFormControls()
        {
            for (int i = 1; i <= 41; i++)
            {
                if (GetControl($"text_{i}_TA") is TextBox t) t.Clear();
                string[] types = { "Stamp", "FDC", "Leaflet", "FDCCanccelled", "Postmark" };
                foreach (var type in types)
                    if (GetControl($"num_{i}_{type}") is NumericUpDown n) n.Value = 0;

                if (GetControl($"drop_{i}_DisType") is ComboBox c) c.SelectedIndex = -1;
            }
            combFileNo.SelectedIndex = -1;
        }

        // --- BAAKI FUNCTIONS (Invoice No, SelectedIndexChanged etc) ---
        // (Aapke purane code wale yahan fit honge, GetControl use karte huye)
        private string GetNextInvoiceNoWithConnection(SqlConnection con, SqlTransaction trans)
        {
            string year = DateTime.Now.Year.ToString();
            string q = "SELECT TOP 1 InvoiceNo FROM InvoiceRegister WITH (UPDLOCK) WHERE InvoiceNo LIKE '%/" + year + "' ORDER BY InvoiceNo DESC";
            object res = new SqlCommand(q, con, trans).ExecuteScalar();
            int n = (res != null) ? int.Parse(res.ToString().Split('.')[1].Split('/')[0]) + 1 : 1;
            return "Ps." + n.ToString("D3") + "/" + year;
        }

        private void combFileNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combFileNo.SelectedValue == null || combFileNo.SelectedValue is DataRowView) return;
            button1.Enabled = true;

            using (SqlConnection con = new (Db.ConString))
            {
                try
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT * FROM StockPrice WHERE FileNo = @No", con);
                    cmd.Parameters.AddWithValue("@No", combFileNo.SelectedValue);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            textFDCPrice.Text = Convert.ToDecimal(r["FDCPrice"]).ToString("0.00");
                            textLeafletPrice.Text = Convert.ToDecimal(r["LeafletPrice"]).ToString("0.00");
                            textPostmarkPrice.Text = Convert.ToDecimal(r["PostmarkPrice"]).ToString("0.00");
                        }
                    }
                    var cmd2 = new SqlCommand("SELECT * FROM StockPhilQuantity WHERE FileNo=@No", con);
                    cmd2.Parameters.AddWithValue("@No", combFileNo.SelectedValue);
                    using (var r2 = cmd2.ExecuteReader())
                    {
                        if (r2.Read())
                        {
                            sfcQty = Convert.ToInt32(r2["StampFCQty"]);
                            fQty = Convert.ToInt32(r2["FDCQty"]);
                            lQty = Convert.ToInt32(r2["LeafletQty"]);
                            pmQty = Convert.ToInt32(r2["PostMarkQty"]);
                            num_21_Stamp.Value = sfcQty;
                            num_22_FDC.Value = fQty;
                            num_22_Leaflet.Value = lQty;
                            num_23_Postmark.Value = pmQty;


                        }
                    }
                    UpdateRemainingStock();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new (Db.ConString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();
                Dictionary<int, int> invoiceIdMap = new ();

                try
                {
                    string qMD = "TRUNCATE TABLE IssueMaleList";
                    SqlCommand qm = new(qMD, con, trans);
                    qm.ExecuteNonQuery();
                    // Logic for InvoiceRegister (1-19)
                    for (int i = 1; i <= 19; i++)
                    {
                        var dP = GetControl($"drop_{i}_Phil") as ComboBox;
                        var tA = GetControl($"text_{i}_TA") as TextBox;
                        var dD = GetControl($"drop_{i}_DisType") as ComboBox;

                        if (dP?.SelectedValue != null && !string.IsNullOrWhiteSpace(tA?.Text) && decimal.Parse(tA.Text) > 0)
                        {
                            string invNo = GetNextInvoiceNoWithConnection(con, trans);
                            string q = @"INSERT INTO InvoiceRegister (FileNo, InvoiceNo, SupplyType, PhiliticBureauName, IssueDate, Totalamount) 
                                         VALUES (@file, @inv, 2, @phil, @date, @amount); SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmd = new SqlCommand(q, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@file", combFileNo.SelectedValue);
                                cmd.Parameters.AddWithValue("@inv", invNo);
                                cmd.Parameters.AddWithValue("@phil", dP.SelectedValue);
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@amount", decimal.Parse(tA.Text));
                                int id = Convert.ToInt32(cmd.ExecuteScalar());
                                invoiceIdMap.Add(i, id);

                                // Pending Invoice
                                new SqlCommand($"INSERT INTO PendingInvoice (InvoiceRegisterId) VALUES({id})", con, trans).ExecuteNonQuery();

                               
                                // IssueMaleList
                                if (i != 2)
                                {
                                    string qM = "INSERT INTO IssueMaleList (MaleListFileId, Address, DispatchType) VALUES (@mf, @ad, @dt)";
                                    SqlCommand cM = new SqlCommand(qM, con, trans);
                                    cM.Parameters.AddWithValue("@mf", combFileNo.SelectedValue);
                                    cM.Parameters.AddWithValue("@ad", dP.SelectedValue);
                                    cM.Parameters.AddWithValue("@dt", dD?.SelectedValue ?? DBNull.Value);
                                    cM.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // PhilatelicSupply (1-26)
                    for (int i = 1; i <= 26; i++)
                    {
                        var dP = GetControl($"drop_{i}_Phil") as ComboBox;
                        if (dP?.SelectedValue == null) continue;

                        string qS = @"INSERT INTO PhilatelicSupply (Address, FileNo, InvoiceNo, SupplyType, Supply_Date, StampsQty, FDCQty, LeafletQty, FDCCQty, PostmarkQty)
                                      VALUES (@phil, @file, @inv, @rstype, @date, @sq, @fq, @lq, @fcq, @pmq)";

                        using (SqlCommand cmd = new SqlCommand(qS, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@phil", dP.SelectedValue);
                            cmd.Parameters.AddWithValue("@file", combFileNo.SelectedValue);
                            cmd.Parameters.AddWithValue("@inv", (i <= 19 && invoiceIdMap.ContainsKey(i)) ? (object)invoiceIdMap[i] : DBNull.Value);
                            cmd.Parameters.AddWithValue("@rstype", ((i >= 1 && i <= 20) || i == 25 || i == 26) ? 2 : 1);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@sq", (GetControl($"num_{i}_Stamp") as NumericUpDown)?.Value ?? 0);
                            cmd.Parameters.AddWithValue("@fq", (GetControl($"num_{i}_FDC") as NumericUpDown)?.Value ?? 0);
                            cmd.Parameters.AddWithValue("@lq", (GetControl($"num_{i}_Leaflet") as NumericUpDown)?.Value ?? 0);
                            cmd.Parameters.AddWithValue("@fcq", (GetControl($"num_{i}_FDCCanccelled") as NumericUpDown)?.Value ?? 0);
                            cmd.Parameters.AddWithValue("@pmq", (GetControl($"num_{i}_Postmark") as NumericUpDown)?.Value ?? 0);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();
                    MessageBox.Show("Saved Successfully!");
                    ClearFormControls();
                    FillFileDropdown();
                }
                catch (Exception ex) { trans.Rollback(); MessageBox.Show("Save Error: " + ex.Message); }
            }
        }
    }
}