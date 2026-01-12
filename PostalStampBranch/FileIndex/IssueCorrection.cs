using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class IssueCorrection : Form
    {
        string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PSDB;Integrated Security=True;";
        int currentFileType = 0;

        public IssueCorrection()
        {
            InitializeComponent();
        }

        private void IssueCorrection_Load(object sender, EventArgs e)
        {
            UIHelper.SetFormTheme(this);
            UIHelper.ApplyTheme(this);
            LoadAllFiles();
            LoadAllIssueNumbers();
        }

        private void LoadAllFiles()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    // Correction: Comma add kiya gaya hai
                    string query = "SELECT Id, FileNo, FileSubject, (FileNo + ' - ' + FileSubject) AS DisplayName FROM FileIndex";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    fileNoCmb.DataSource = dt;
                    fileNoCmb.DisplayMember = "FileNo"; // Dropdown band hone par sirf FileNo dikhega
                    fileNoCmb.ValueMember = "Id";
                    fileNoCmb.SelectedIndex = -1;

                    textFileSubject.Clear(); // Shuru mein khali rakhein
                }
            }
            catch (Exception ex) { MessageBox.Show("Files Load Error: " + ex.Message); }
        }

        private void LoadAllIssueNumbers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    string query = "SELECT Id, IssueNo FROM CommStamp ORDER BY Id DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    issuNoCmb.DataSource = dt;
                    issuNoCmb.DisplayMember = "IssueNo";
                    issuNoCmb.ValueMember = "IssueNo";
                    issuNoCmb.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Issues Load Error: " + ex.Message); }
        }

        private void issuNoCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (issuNoCmb.SelectedIndex == -1 || issuNoCmb.SelectedValue == null) return;

            string selectedIssue = issuNoCmb.SelectedValue.ToString();

            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    string query = @"SELECT C.*, P.*, F.FileType 
                                     FROM CommStamp C 
                                     LEFT JOIN Price P ON C.FileNo = P.FileNo 
                                     LEFT JOIN FileIndex F ON C.FileNo = F.Id
                                     WHERE C.IssueNo = @IssueNo";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@IssueNo", selectedIssue);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        currentFileType = dr["FileType"] != DBNull.Value ? Convert.ToInt32(dr["FileType"]) : 0;
                        EnableSouveControls(this, (currentFileType == 2 || currentFileType == 4));

                        // Mapping data to UI
                        fileNoCmb.SelectedValue = dr["FileNo"];
                        dateOfIssuePicker.Value = Convert.ToDateTime(dr["DateOfIssue"]);
                        issueNo.Text = dr["IssueNo"].ToString();

                        stampDesignNUM.Value = dr["StampDesignNo"] != DBNull.Value ? Convert.ToDecimal(dr["StampDesignNo"]) : 0;
                        souvenirDesignNUM.Value = dr["SouvenirDesignNo"] != DBNull.Value ? Convert.ToDecimal(dr["SouvenirDesignNo"]) : 0;

                        // Numeric values ko "0" handle karna
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

                        remarkTxt.Text = dr["Remarks"]?.ToString() ?? "";
                    }
                    dr.Close();
                }
                catch (Exception ex) { MessageBox.Show("Data Display Error: " + ex.Message); }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // --- SECURITY CHECKS ---
            if (issuNoCmb.SelectedValue == null) { MessageBox.Show("Please select an Issue."); return; }
            if (fileNoCmb.SelectedValue == null) { MessageBox.Show("File ID is missing. Select File again."); return; }

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // 1. Update CommStamp
                    string up1 = @"UPDATE CommStamp SET DateOfIssue=@Dt, StampDesignNo=@SD, 
                                   SouvenirDesignNo=@SvD, Remarks=@Rm WHERE IssueNo=@IsID";
                    SqlCommand cmd1 = new SqlCommand(up1, con, trans);
                    cmd1.Parameters.AddWithValue("@Dt", dateOfIssuePicker.Value);
                    cmd1.Parameters.AddWithValue("@SD", stampDesignNUM.Value);
                    cmd1.Parameters.AddWithValue("@SvD", souvenirDesignNUM.Value);
                    cmd1.Parameters.AddWithValue("@Rm", remarkTxt.Text);
                    cmd1.Parameters.AddWithValue("@IsID", issuNoCmb.SelectedValue);
                    cmd1.ExecuteNonQuery();

                    // 2. Update Price
                    string up2 = @"UPDATE Price SET StampPrice=@SP, SouvenirPrice=@SvP, FDCPrice=@FP, 
                                   LeafletPrice=@LP, PostmarkPrice=@PP, StampQty=@SQ, SouvenirQty=@SvQ, 
                                   FDCQty=@FQ, LeafletQty=@LQ, PostMarkQty=@PQ, Remark=@Rm 
                                   WHERE FileNo=@FID";
                    SqlCommand cmd2 = new SqlCommand(up2, con, trans);

                    // Har field ko check karna taake null na jaye (Agar 0 hai to 0 jaye)
                    cmd2.Parameters.AddWithValue("@SP", string.IsNullOrWhiteSpace(stampPrice.Text) ? "0" : stampPrice.Text);
                    cmd2.Parameters.AddWithValue("@SvP", string.IsNullOrWhiteSpace(souvenirPrice.Text) ? "0" : souvenirPrice.Text);
                    cmd2.Parameters.AddWithValue("@FP", string.IsNullOrWhiteSpace(FDCPrice.Text) ? "0" : FDCPrice.Text);
                    cmd2.Parameters.AddWithValue("@LP", string.IsNullOrWhiteSpace(leafletPrice.Text) ? "0" : leafletPrice.Text);
                    cmd2.Parameters.AddWithValue("@PP", string.IsNullOrWhiteSpace(postmarkPrice.Text) ? "0" : postmarkPrice.Text);
                    cmd2.Parameters.AddWithValue("@SQ", string.IsNullOrWhiteSpace(stampQty.Text) ? "0" : stampQty.Text);
                    cmd2.Parameters.AddWithValue("@SvQ", string.IsNullOrWhiteSpace(souvenirQty.Text) ? "0" : souvenirQty.Text);
                    cmd2.Parameters.AddWithValue("@FQ", string.IsNullOrWhiteSpace(FDCQty.Text) ? "0" : FDCQty.Text);
                    cmd2.Parameters.AddWithValue("@LQ", string.IsNullOrWhiteSpace(leafletQty.Text) ? "0" : leafletQty.Text);
                    cmd2.Parameters.AddWithValue("@PQ", string.IsNullOrWhiteSpace(postmarkQty.Text) ? "0" : postmarkQty.Text);
                    cmd2.Parameters.AddWithValue("@Rm", remarkTxt.Text);

                    // FIX: @FID hamesha yahan se provide hoga
                    cmd2.Parameters.AddWithValue("@FID", fileNoCmb.SelectedValue);

                    cmd2.ExecuteNonQuery();

                    trans.Commit();
                    MessageBox.Show("Record Updated and Saved!");
                    RefreshForm();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Update Error: " + ex.Message);
                }
            }
        }

        private void RefreshForm()
        {
            issuNoCmb.SelectedIndex = -1;
            fileNoCmb.SelectedIndex = -1;
            dateOfIssuePicker.Value = DateTime.Now;
            ClearAllControls(this);
            issuNoCmb.Focus();
        }

        private void ClearAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox) c.Text = "";
                else if (c is NumericUpDown n) n.Value = 0;
                if (c.HasChildren) ClearAllControls(c);
            }
        }

        private void EnableSouveControls(Control parent, bool enable)
        {
            if (parent.Tag?.ToString() == "souveControll") parent.Enabled = enable;
            foreach (Control child in parent.Controls) EnableSouveControls(child, enable);
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }

        private void fileNoCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Pehle check karein ke kya waqai kuch select hua hai?
            // -1 ka matlab hota hai ke kuch bhi select nahi hua.
            if (fileNoCmb.SelectedIndex != -1)
            {
                // Selected item ko DataRowView mein convert karein
                // Kyunke hamara data DataTable se aa raha hai
                DataRowView drv = (DataRowView)fileNoCmb.SelectedItem;

                // Ab database ke column "FileSubject" ki value nikal kar TextBox mein insert karein
                if (drv != null)
                {
                    textFileSubject.Text = drv["FileSubject"].ToString();
                }
            }
            else
            {
                // Agar selection khali hai, toh box ko bhi saaf kar dein
                textFileSubject.Clear();
            }
        }

       

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            ClearAllControls(this);
        }
    }
}