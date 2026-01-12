using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class Form1 : Form
    {
        // Global variables
        int currentFileType = 0;
        string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PSDB;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);

        }

        // 1. Data Load karne ka function
        void LoadData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    string query = "SELECT Id, (FileNo + ' - ' + FileSubject) AS FileDetail FROM FileIndex WHERE FileType IN (1,2,3,4) AND Status = 2 ORDER BY Id DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    fileNoCmb.DataSource = dt;
                    fileNoCmb.DisplayMember = "FileDetail";
                    fileNoCmb.ValueMember = "Id";
                    fileNoCmb.SelectedIndex = -1; // Shuru mein khali rahay
                    fileNoCmb.DropDownWidth = 1000;
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        // 2. Validation Function (FileType ke mutabiq check)
        private bool IsFormValid()
        {
            if (fileNoCmb.SelectedValue == null)
            {
                MessageBox.Show("Please select a File Number.");
                return false;
            }

            if (string.IsNullOrEmpty(issueNo.Text) || !issueNo.Text.Contains("-"))
            {
                MessageBox.Show("Please enter Issue Number correctly (e.g., 20-25).");
                return false;
            }

            // Stamp details har haal mein lazmi hain
            if (string.IsNullOrEmpty(stampPrice.Text) || stampDesignNUM.Value == 0)
            {
                MessageBox.Show("Stamp Price and Design Number are mandatory.");
                return false;
            }

            // Agar FileType 2 ya 4 ho to Souvenir lazmi check karein
            if (currentFileType == 2 || currentFileType == 4)
            {
                if (string.IsNullOrEmpty(souvenirPrice.Text) || souvenirDesignNUM.Value == 0)
                {
                    MessageBox.Show("Souvenir data is required for this File Type!");
                    return false;
                }
            }

            return true;
        }

        // 3. Form ko Khali (Clean) karne ka function
        private void ClearForm()
        {
            issueNo.Clear();
            remarkTxt.Clear();
            dateOfIssuePicker.Value = DateTime.Now;
            fileNoCmb.SelectedIndex = -1;
            currentFileType = 0;

            // Tamam Controls ko loop ke zariye clean karein
            RecursiveClear(this);
        }

        private void RecursiveClear(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox) c.Text = "";
                else if (c is NumericUpDown n) n.Value = 0; // NumericUpDown clean karne ka sahi tariqa

                if (c.HasChildren) RecursiveClear(c); // Panels/Groups ke andar bhi clean karein
            }
        }

        // 4. Save Button Code
        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsFormValid()) return; // Pehle check karein

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string fullIssueNo = issueNo.Text.Trim();
                    string[] parts = fullIssueNo.Split('-');
                    int issueCounter = Convert.ToInt32(parts[0]);
                    string issueYear = parts[1];

                    // Table 1: CommStamp
                    string query1 = @"INSERT INTO CommStamp (FileNo, DateOfIssue, IssueNo, StampDesignNo, SouvenirDesignNo, IssueYear, IssueCounter, Remarks) 
                                    VALUES (@FileNo, @DateOfIssue, @IssueNo, @StampDesignNo, @SouvenirDesignNo, @IssueYear, @IssueCounter, @Remarks)";

                    SqlCommand cmd1 = new SqlCommand(query1, con, trans);
                    cmd1.Parameters.AddWithValue("@FileNo", fileNoCmb.SelectedValue);
                    cmd1.Parameters.AddWithValue("@DateOfIssue", dateOfIssuePicker.Value);
                    cmd1.Parameters.AddWithValue("@IssueNo", fullIssueNo);
                    cmd1.Parameters.AddWithValue("@StampDesignNo", stampDesignNUM.Value);
                    cmd1.Parameters.AddWithValue("@SouvenirDesignNo", souvenirDesignNUM.Value);
                    cmd1.Parameters.AddWithValue("@IssueCounter", issueCounter);
                    cmd1.Parameters.AddWithValue("@IssueYear", issueYear);
                    cmd1.Parameters.AddWithValue("@Remarks", remarkTxt.Text);
                    cmd1.ExecuteNonQuery();

                    // Table 2: Price
                    string query2 = @"INSERT INTO Price (FileNo, StampPrice, SouvenirPrice, FDCPrice, LeafletPrice, PostmarkPrice, StampQty, SouvenirQty, FDCQty, LeafletQty, PostMarkQty, Remark) 
                                    VALUES (@FileID, @SP, @SvP, @FP, @LP, @PP, @SQ, @SvQ, @FQ, @LQ, @PQ, @Rem)";

                    SqlCommand cmd2 = new SqlCommand(query2, con, trans);
                    cmd2.Parameters.AddWithValue("@FileID", fileNoCmb.SelectedValue);
                    cmd2.Parameters.AddWithValue("@SP", stampPrice.Text);
                    cmd2.Parameters.AddWithValue("@SvP", souvenirPrice.Text);
                    cmd2.Parameters.AddWithValue("@FP", FDCPrice.Text);
                    cmd2.Parameters.AddWithValue("@LP", leafletPrice.Text);
                    cmd2.Parameters.AddWithValue("@PP", postmarkPrice.Text);
                    cmd2.Parameters.AddWithValue("@SQ", stampQty.Text);
                    cmd2.Parameters.AddWithValue("@SvQ", souvenirQty.Text);
                    cmd2.Parameters.AddWithValue("@FQ", FDCQty.Text);
                    cmd2.Parameters.AddWithValue("@LQ", leafletQty.Text);
                    cmd2.Parameters.AddWithValue("@PQ", postmarkQty.Text);
                    cmd2.Parameters.AddWithValue("@Rem", remarkTxt.Text);
                    cmd2.ExecuteNonQuery();

                    // Table 3: Update Status
                    string query3 = "UPDATE FileIndex SET Status = 1 WHERE Id = @Id";
                    SqlCommand cmd3 = new SqlCommand(query3, con, trans);
                    cmd3.Parameters.AddWithValue("@Id", fileNoCmb.SelectedValue);
                    cmd3.ExecuteNonQuery();

                    trans.Commit();
                    MessageBox.Show("Data Saved Successfully!");

                    LoadData(); // ComboBox refresh karein
                    ClearForm(); // Form clean karein
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Save Error: " + ex.Message);
                }
            }
        }

        private void fileNoCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileNoCmb.SelectedValue == null || fileNoCmb.SelectedIndex == -1) return;

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    string query = "SELECT FileType FROM FileIndex WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", fileNoCmb.SelectedValue);
                    con.Open();
                    currentFileType = Convert.ToInt32(cmd.ExecuteScalar());
                }

                bool shouldEnable = (currentFileType == 2 || currentFileType == 4);
                EnableSouveControls(this, shouldEnable);
            }
            catch { }
        }

        private void EnableSouveControls(Control parent, bool enable)
        {
            if (parent.Tag != null && parent.Tag.ToString() == "souveControll")
            {
                parent.Enabled = enable;
            }
            foreach (Control child in parent.Controls) EnableSouveControls(child, enable);
        }

        private void dateOfIssuePicker_ValueChanged(object sender, EventArgs e)
        {
            string issueYear = dateOfIssuePicker.Value.ToString("yyyy");
            string query = "SELECT ISNULL(MAX(IssueCounter), 0) + 1 FROM CommStamp WHERE IssueYear = @IssueYear";
            issueNo.Text = null;
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IssueYear", issueYear);
                con.Open();
                int nextCounter = Convert.ToInt32(cmd.ExecuteScalar());
                issueNo.Text = $"{nextCounter}-{issueYear}";
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}