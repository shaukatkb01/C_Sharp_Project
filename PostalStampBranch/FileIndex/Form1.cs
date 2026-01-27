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
           
            if (!IsFormValid()) return;

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string fullIssueNo = issueNo.Text.Trim();
                    int selectedFileId = (int)fileNoCmb.SelectedValue;

                    // --- Table 1: CommStamp ---
                    string query1 = @"INSERT INTO CommStamp (IssueNo, FileNo, DateOfIssue, Remarks) 
                  VALUES (@IssueNo, @FileNo, @DateOfIssue, @Remarks);
                  SELECT SCOPE_IDENTITY();"; // Yeh line nayi ID wapas bhejti hai

                    SqlCommand cmd1 = new SqlCommand(query1, con, trans);
                    cmd1.Parameters.AddWithValue("@IssueNo", fullIssueNo);
                    cmd1.Parameters.AddWithValue("@FileNo", selectedFileId);
                    cmd1.Parameters.AddWithValue("@DateOfIssue", dateOfIssuePicker.Value);
                    cmd1.Parameters.AddWithValue("@Remarks", remarkTxt.Text);
                    object result = cmd1.ExecuteScalar();
                    if (result != null)
                    {
                        int newIssueId = Convert.ToInt32(result);

                        // --- Table 2: Design ---
                        string queryDesign = @"INSERT INTO Design (IssueNo, StampDesignNo, SouvenirDesignNo, DesignRemark) 
                                 VALUES (@ino, @sd, @svd, @dremark)";

                        SqlCommand cmdDesign = new SqlCommand(queryDesign, con, trans);
                        cmdDesign.Parameters.AddWithValue("@ino", newIssueId);
                        cmdDesign.Parameters.AddWithValue("@sd", stampDesignNUM.Value);
                        cmdDesign.Parameters.AddWithValue("@svd", souvenirDesignNUM.Value);
                        cmdDesign.Parameters.AddWithValue("@dremark", remarkTxt.Text);
                        cmdDesign.ExecuteNonQuery();
                    }
                    // --- Table 3: StockPrice (Sirf Qeemat) ---
                    string queryPrice = @"INSERT INTO StockPrice (FileNo, StampPrice, SouvenirPrice, FDCPrice, LeafletPrice, PostmarkPrice, Remark) 
                                 VALUES (@FileID, @SP, @SvP, @FP, @LP, @PP, @Rem)";

                    SqlCommand cmdPrice = new SqlCommand(queryPrice, con, trans);
                    cmdPrice.Parameters.AddWithValue("@FileID", selectedFileId);
                    cmdPrice.Parameters.AddWithValue("@SP", stampPrice.Value);
                    cmdPrice.Parameters.AddWithValue("@SvP", souvenirPrice.Value);
                    cmdPrice.Parameters.AddWithValue("@FP", FDCPrice.Value);
                    cmdPrice.Parameters.AddWithValue("@LP", leafletPrice.Value);
                    cmdPrice.Parameters.AddWithValue("@PP", postmarkPrice.Value);
                    cmdPrice.Parameters.AddWithValue("@Rem", remarkTxt.Text);
                    cmdPrice.ExecuteNonQuery();

                    // --- Table 4: StockPhilQuantity (Sirf Ginti) ---
                    string queryQty = @"INSERT INTO StockPhilQuantity 
                                      (FileNo, StampQty, SouvenirQty, FDCQty, StampFCQty, LeafletQty, PostMarkQty) 
                               VALUES (@FileID, @SQ, @SvQ, @FQ, @SFCQ, @LQ, @PQ)";

                    SqlCommand cmdQty = new SqlCommand(queryQty, con, trans);
                    cmdQty.Parameters.AddWithValue("@FileID", selectedFileId);
                    cmdQty.Parameters.AddWithValue("@SQ", (int)stampQty.Value);
                    cmdQty.Parameters.AddWithValue("@SvQ", (int)souvenirQty.Value);
                    cmdQty.Parameters.AddWithValue("@FQ", (int)FDCQty.Value);
                    cmdQty.Parameters.AddWithValue("@SFCQ", (int)stampQtyFC.Value); // stamp free of cost Qty ke liye
                    cmdQty.Parameters.AddWithValue("@LQ", (int)leafletQty.Value);
                    cmdQty.Parameters.AddWithValue("@PQ", (int)postmarkQty.Value);
                    cmdQty.ExecuteNonQuery();

                    // --- Table 5: Update FileIndex Status ---
                    string queryStatus = "UPDATE FileIndex SET Status = 1 WHERE Id = @Id";
                    SqlCommand cmdStatus = new SqlCommand(queryStatus, con, trans);
                    cmdStatus.Parameters.AddWithValue("@Id", selectedFileId);
                    cmdStatus.ExecuteNonQuery();

                    // Sab sahi hai toh save kar do
                    trans.Commit();
                    MessageBox.Show("All data is saved in tables..");

                    LoadData(); // ComboBox refresh
                    ClearForm(); // Form saaf karein
                }

                catch (Exception ex)
                {
                    trans.Rollback(); // Galti hone par sab wapas purani halat mein
                    MessageBox.Show("Save Error (Data Rollbacked): " + ex.Message);
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
         
            // 1. Picker se saal nikaalna
            int selectedYear = dateOfIssuePicker.Value.Year;

            // 2. Query: DateOfIssue column se Year nikaal kar count karna
            // Hum YEAR(DateOfIssue) use karenge aur us saal ke records count karenge
            string query = "SELECT COUNT(*) + 1 FROM CommStamp WHERE YEAR(DateOfIssue) = @SelectedYear";

            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@SelectedYear", selectedYear);

                try
                {
                    con.Open();
                    int nextCount = Convert.ToInt32(cmd.ExecuteScalar());

                    // 3. Issue Number format: "1-2026"
                    issueNo.Text = $"{nextCount}-{selectedYear}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        

        
    }
}