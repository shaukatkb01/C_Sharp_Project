using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class IssueCorrection : Form
    {
        int currentFileType = 0;

        private void RefreshForm()
        {
            issuNoCmb.SelectedIndex = -1;
            fileNoCmb.SelectedIndex = -1;
            dateOfIssuePicker.Value = DateTime.Now;
            ClearAllControls(this);
            //LoadAllIssueNumbers();
            issuNoCmb.Focus();
        }

        private void ClearAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // 1. TextBox saaf karein
                if (c is TextBox textBox)
                {
                    textBox.Clear(); // Text = "" se behtar hai .Clear() use karna
                }
                // 2. NumericUpDown ko uski minimum value par le jayein
                else if (c is NumericUpDown numeric)
                {
                    numeric.Value = numeric.Minimum; // Direct 0 likhne se behtar hai Minimum use karna
                }
                // 3. ComboBox ko reset karein
                else if (c is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1;
                    comboBox.Text = string.Empty;
                }
                // 4. CheckBox uncheck karein
                else if (c is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }

                // 5. Agar is control ke andar aur controls hain (Panel/GroupBox), toh wahan bhi jao
                if (c.HasChildren)
                {
                    ClearAllControls(c);
                }
            }
        }

        private void fileNoCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileNoCmb.SelectedIndex != -1 && fileNoCmb.SelectedItem is DataRowView drv)
            {
                textFileSubject.Text = drv["FileSubject"].ToString();
            }
            else { textFileSubject.Clear(); }
        }

        public IssueCorrection()
        {
            InitializeComponent();
        }

        private void IssueCorrection_Load(object sender, EventArgs e)
        {
            LoadAllFiles();
            LoadAllIssueNumbers();
        }

        private void LoadAllFiles()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "SELECT Id, FileNo, FileSubject FROM FileIndex";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    fileNoCmb.DisplayMember = "FileNo";
                    fileNoCmb.ValueMember = "Id";
                    fileNoCmb.DataSource = dt;
                    fileNoCmb.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Files Load Error: " + ex.Message); }
        }

        private void LoadAllIssueNumbers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "SELECT IssueId, IssueNo FROM CommStamp ORDER BY IssueId DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // 1. Pehle purani binding saaf karein
                    //issuNoCmb.DataSource = null;

                    // 2. Phir batayein ke kon se columns use karne hain
                    issuNoCmb.DataSource = dt;
                    issuNoCmb.DisplayMember = "IssueNo";
                    issuNoCmb.ValueMember = "IssueId";

                    // 3. Sab se aakhir mein naya data assign karein

                    // 4. Khali nazar aaye shuru mein
                    issuNoCmb.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Issues Load Error: " + ex.Message);
            }
        }

        private void issuNoCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIssue = Convert.ToInt32(issuNoCmb.SelectedValue);
            // Sirf tab chale jab waqayi koi number select ho (DataRowView se bachne ke liye)
            if (issuNoCmb.SelectedIndex == -1 ) return;

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    // Specific Columns mention karein taake ambiguity na ho
                    string query = @"SELECT C.FileNo, C.DateOfIssue, C.IssueNo, C.Remarks AS CommRemarks, 
                                     P.StampPrice, P.SouvenirPrice, P.FDCPrice, P.LeafletPrice, P.PostmarkPrice,
                                     Q.StampQty, Q.SouvenirQty, Q.FDCQty, Q.LeafletQty, Q.PostMarkQty,
                                     D.StampDesignNo, D.SouvenirDesignNo, F.FileType 
                                     FROM CommStamp C 
                                     LEFT JOIN StockPrice P ON C.FileNo = P.FileNo 
                                     LEFT JOIN StockPhilQuantity Q ON C.FileNo = Q.FileNo
                                     LEFT JOIN Design D ON C.IssueId = D.IssueNo
                                     LEFT JOIN FileIndex F ON C.FileNo = F.Id
                                     WHERE C.IssueId = @IssueNo";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@IssueNo", selectedIssue);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        currentFileType = dr["FileType"] != DBNull.Value ? Convert.ToInt32(dr["FileType"]) : 0;
                        EnableSouveControls(this, (currentFileType == 2 || currentFileType == 4));

                        fileNoCmb.SelectedValue = dr["FileNo"];
                        dateOfIssuePicker.Value = dr["DateOfIssue"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfIssue"]) : DateTime.Now;
                        issueNo.Text = dr["IssueNo"].ToString();
                        remarkTxt.Text = dr["CommRemarks"]?.ToString() ?? "";

                        // Design
                        stampDesignNUM.Value = dr["StampDesignNo"] != DBNull.Value ? Convert.ToDecimal(dr["StampDesignNo"]) : 0;
                        souvenirDesignNUM.Value = dr["SouvenirDesignNo"] != DBNull.Value ? Convert.ToDecimal(dr["SouvenirDesignNo"]) : 0;

                        // Prices & Quantities (Safe display)
                        stampPrice.Text = dr["StampPrice"]?.ToString() ?? "0";
                        souvenirPrice.Text = dr["SouvenirPrice"]?.ToString() ?? "0";
                        FDCPrice.Text = dr["FDCPrice"]?.ToString() ?? "0";
                        leafletPrice.Text = dr["LeafletPrice"]?.ToString() ?? "0";
                        postmarkPrice.Text = dr["PostmarkPrice"]?.ToString() ?? "0";

                        stampQty.Text = dr["StampQty"]?.ToString() ?? "0";
                        souvenirQty.Text = dr["SouvenirQty"]?.ToString() ?? "0";
                        FDCQty.Text = dr["FDCQty"]?.ToString() ?? "0";
                        leafletQty.Text = dr["LeafletQty"]?.ToString() ?? "0";
                        postmarkQty.Text = dr["PostMarkQty"]?.ToString() ?? "0";
                    }
                    dr.Close();
                }
                catch (Exception ex) { MessageBox.Show("Display Error: " + ex.Message); }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(issuNoCmb.SelectedValue is int selectedIssueId)) { MessageBox.Show("Please select an Issue."); return; }
            if (!(fileNoCmb.SelectedValue is int selectedFileId)) { MessageBox.Show("Please select a File."); return; }

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // 1. Update CommStamp (Using IssueId is safer than IssueNo)
                    string up1 = "UPDATE CommStamp SET DateOfIssue=@Dt, Remarks=@Rm WHERE IssueId=@IsID";
                    SqlCommand cmd1 = new SqlCommand(up1, con, trans);
                    cmd1.Parameters.AddWithValue("@Dt", dateOfIssuePicker.Value);
                    cmd1.Parameters.AddWithValue("@Rm", remarkTxt.Text);
                    cmd1.Parameters.AddWithValue("@IsID", selectedIssueId);
                    cmd1.ExecuteNonQuery();

                    // 2. Update Design
                    string upDesign = "UPDATE Design SET StampDesignNo=@SD, SouvenirDesignNo=@SvD WHERE IssueNo=@IsID";
                    SqlCommand cmdD = new SqlCommand(upDesign, con, trans);
                    cmdD.Parameters.AddWithValue("@SD", stampDesignNUM.Value);
                    cmdD.Parameters.AddWithValue("@SvD", souvenirDesignNUM.Value);
                    cmdD.Parameters.AddWithValue("@IsID", selectedIssueId);
                    cmdD.ExecuteNonQuery();

                    // 3. Update StockPrice (Using FileId)
                    string up2 = @"UPDATE StockPrice SET StampPrice=@SP, SouvenirPrice=@SvP, FDCPrice=@FP, 
                                   LeafletPrice=@LP, PostmarkPrice=@PP WHERE FileNo=@FID";
                    SqlCommand cmd2 = new SqlCommand(up2, con, trans);
                    cmd2.Parameters.AddWithValue("@SP", SafeDecimal(stampPrice.Text));
                    cmd2.Parameters.AddWithValue("@SvP", SafeDecimal(souvenirPrice.Text));
                    cmd2.Parameters.AddWithValue("@FP", SafeDecimal(FDCPrice.Text));
                    cmd2.Parameters.AddWithValue("@LP", SafeDecimal(leafletPrice.Text));
                    cmd2.Parameters.AddWithValue("@PP", SafeDecimal(postmarkPrice.Text));
                    cmd2.Parameters.AddWithValue("@FID", selectedFileId);
                    cmd2.ExecuteNonQuery();

                    // 4. Update StockPhilQuantity
                    string up3 = @"UPDATE StockPhilQuantity SET StampQty=@SQ, SouvenirQty=@SvQ, FDCQty=@FQ, 
                                   LeafletQty=@LQ, PostMarkQty=@PQ WHERE FileNo=@FID";
                    SqlCommand cmd3 = new SqlCommand(up3, con, trans);
                    cmd3.Parameters.AddWithValue("@SQ", SafeDecimal(stampQty.Text));
                    cmd3.Parameters.AddWithValue("@SvQ", SafeDecimal(souvenirQty.Text));
                    cmd3.Parameters.AddWithValue("@FQ", SafeDecimal(FDCQty.Text));
                    cmd3.Parameters.AddWithValue("@LQ", SafeDecimal(leafletQty.Text));
                    cmd3.Parameters.AddWithValue("@PQ", SafeDecimal(postmarkQty.Text));
                    cmd3.Parameters.AddWithValue("@FID", selectedFileId);
                    cmd3.ExecuteNonQuery();

                    trans.Commit();
                    MessageBox.Show("All Tables Updated Successfully!");
                   
                    RefreshForm();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Update Error: " + ex.Message);
                }
            }
        }

        // Helper function for safe decimal conversion
        private decimal SafeDecimal(string text)
        {
            return decimal.TryParse(text, out decimal res) ? res : 0;
        }

       

        private void EnableSouveControls(Control parent, bool enable)
        {
            if (parent.Tag?.ToString() == "souveControll") parent.Enabled = enable;
            foreach (Control child in parent.Controls) EnableSouveControls(child, enable);
        }

       

        
    }
}