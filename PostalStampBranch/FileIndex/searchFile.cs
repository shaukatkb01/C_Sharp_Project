using Microsoft.Data.SqlClient;
using PostalStampSystem;
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
    public partial class searchFile : Form
    {
        public void fileregisterload(int Status)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = @"SELECT F.Id, F.FileSubject, FT.FileType, F.Remark,
                                   S.Status
                                   
                                   FROM FileIndex F
                                   LEFT JOIN Status S ON S.Id= F.Status
                                      LEFT JOIN FileType FT ON FT.Id= F.FileType
                                   WHERE F.FileType=@ft";
                    SqlCommand cmd = new(query, con);
                    cmd.Parameters.AddWithValue("@ft", Status);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        frmReportView reportForm = new frmReportView();
                        // dt yahan apka datatable hai
                        reportForm.LoadReport(dt, "dtFileSearch", "Report/rptFileSearch.rdlc");
                        reportForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("No invoices found for this issue.");
                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file types: " + ex.Message);
                }
            }



        }
        public void FileStatus(ComboBox cmb)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = "SELECT Id, Status FROM Status";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cmb.DataSource = dt;
                    cmb.DisplayMember = "Status";
                    cmb.ValueMember = "Id";
                    cmb.SelectedIndex = -1; // Default selection ko khali rakhein
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading statuses: " + ex.Message);
                }
            }
        }

        public searchFile()
        {
            

            // Force Layout refresh
            this.Load += (s, e) => {
                this.PerformLayout();
                dgvResults.Refresh();
            };
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        private void searchFile_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            dgvResults.AutoGenerateColumns = true;
            FileStatus(cmb_Status);
            radio_Search.Checked = true;
            gridload(dgvResults, null);

        }
        // Status ko 'int?' kar diya taake ye null accept kar sakay
        private void gridload(DataGridView? grid, int? Status)
        {
            if (grid == null) return;

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    // SQL Trick: Agar @st NULL hai toh (f.Status = f.Status) hamesha TRUE hoga 
                    // aur saara data load ho jayega.
                    string query = @"SELECT 
                                f.Id, 
                                f.FileNo, 
                                f.FileSubject, 
                                t.FileType, 
                                f.DateOfCreation,
                                s.Status, 
                                f.Remark 
                             FROM FileIndex f
                             LEFT JOIN FileType t ON f.FileType = t.Id
                             LEFT JOIN Status s ON f.Status = s.Id
                             WHERE (@st IS NULL OR f.Status = @st)";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // DBNull check: Agar C# ka 'Status' null hai toh SQL ko 'DBNull.Value' bhejien
                    cmd.Parameters.AddWithValue("@st", (object)Status ?? DBNull.Value);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    grid.DataSource = dt;

                    if (dt.Rows.Count > 0 && grid.Columns.Count > 0)
                    {
                        // Columns ki Formatting
                        grid.Columns["Id"].Width = 50;
                        grid.Columns["FileNo"].Width = 100;
                        grid.Columns["FileType"].Width = 150;
                        grid.Columns["FileSubject"].Width = 400; // 1000 bohat zyada tha, screen se bahar chala jata hai

                        grid.Columns["FileNo"].HeaderText = "File Number";
                        grid.Columns["FileSubject"].HeaderText = "Subject / Details";
                        grid.Columns["DateOfCreation"].DefaultCellStyle.Format = "dd-MM-yyyy";
                        grid.Columns["Remark"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search Error: " + ex.Message);
                }
            }
        }

        private void SearchData(string? searchTerm)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = @"SELECT 
                    f.Id, 
                    f.FileNo, 
                    f.FileSubject, 
                    
                    t.FileType,   -- Table FileType se naam
                    f.DateOfCreation,
                    s.Status,      -- Table Status se naam
                    f.Remark 
                 FROM FileIndex f
                 LEFT JOIN FileType t ON f.FileType = t.Id
                 LEFT JOIN Status s ON f.Status = s.Id
                 WHERE f.FileNo LIKE @search 
                 OR f.FileSubject LIKE @search 
                 OR f.Remark LIKE @search 
                 OR t.FileType LIKE @search  -- Ab aap 'Urgent' likh kar bhi search kar saken gay
                 OR s.Status LIKE @search";

                    // Pehle Command banayein
                    SqlCommand cmd = new SqlCommand(query, con);

                    // Parameter yahan add karein
                    cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                    // Phir Adapter ko woh command dein
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Check karein ke DataTable mein data aaya bhi hai ya nahi
                    if (dt.Rows.Count > 0)
                    {
                        dgvResults.DataSource = dt;

                        // Check karein ke data aaya bhi hai ya nahi
                        if (dgvResults.Columns.Count > 0)
                        {
                            // 1. Column ki Width (Chorayi) set karein
                            // Yaad rahe: "Id" ya "FileNo" wahi naam hon jo database table mein hain
                            dgvResults.Columns["Id"].Width = 50;
                            dgvResults.Columns["FileNo"].Width = 100;
                            dgvResults.Columns["FileType"].Width = 200;

                            // Subject ko bada rakhein kyunke is mein text zyada hota hai
                            dgvResults.Columns["FileSubject"].Width = 1000;

                            // 2. Column ka Heading text tabdeel karein (Jo user ko nazar aaye ga)
                            dgvResults.Columns["FileNo"].HeaderText = "File Number";
                            dgvResults.Columns["FileSubject"].HeaderText = "Subject / Details";
                            dgvResults.Columns["FileType"].HeaderText = "Type";
                            dgvResults.Columns["DateOfCreation"].HeaderText = "Date of Creation";
                            // Date format set karne ke liye (DD-MM-YYYY format)
                            dgvResults.Columns["DateOfCreation"].DefaultCellStyle.Format = "dd-MM-yyyy";
                            // 3. Remarks ko 'Fill' kar dein taaki bachi hui sari jagah ye gher lay
                            dgvResults.Columns["Remark"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                    else
                    {
                        dgvResults.DataSource = null; // Agar kuch na mile toh khali kar do
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search Error: " + ex.Message);
                }
            }
        }



        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            // Agar textbox khali hai toh data na dikhao ya sara dikha do (aap ki marzi)
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgvResults.DataSource = null;
                return;
            }

            SearchData(txtSearch.Text);
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            if (radio_Search.Checked)
            {


                if (dgvResults.DataSource != null)
                {
                    // 1. Grid ka data DataTable mein lein (.Copy() zaroori hai)
                    DataTable dt = ((DataTable)dgvResults.DataSource).Copy();

                    // 2. Naya Column add karein (Sirf report mein dikhane ke liye)
                    dt.Columns.Add("SearchCriteria", typeof(string));
                    dt.Columns.Add("FromDate", typeof(string));
                    dt.Columns.Add("ToDate", typeof(string));

                    // 3. Pehli row mein value bhar dein (Report sirf pehli row se utha legi)
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows[0]["SearchCriteria"] = "Search Results for: " + txtSearch.Text;

                        //dt.Rows[0]["FromDate"] = dtpFrom.Value.ToString("dd-MMM-yyyy");
                        //dt.Rows[0]["ToDate"] = dtpTo.Value.ToString("dd-MMM-yyyy");
                    }

                    // Baqi code wahi hai
                    frmReportView reportForm = new frmReportView();
                    string rpath = Path.Combine(Application.StartupPath, "Report", "rptFileSearch.rdlc");

                    reportForm.LoadReport(dt, "dtFileSearch", rpath);
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Pehle search karein taake data grid mein aa jaye.");
                }
            }
            else
            {
                if (cmb_Status.SelectedIndex == -1)
                {
                    MessageBox.Show("Select File Status");
                    cmb_Status.DroppedDown = true;
                    cmb_Status.Focus();
                    return;

                }
                fileregisterload(Convert.ToInt32(cmb_Status.SelectedValue));

            }
        }

        private void cmb_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Pehle check karein ke kuch select hai ya nahi
            // SelectedValue check karna zyada behtar hai DataRowView se bachne ke liye
            if (cmb_Status.SelectedIndex != -1 && cmb_Status.SelectedValue != null)
            {
                // TryParse ya direct conversion tabhi karein jab value mojud ho
                if (int.TryParse(cmb_Status.SelectedValue.ToString(), out int statusId))
                {
                    gridload(dgvResults, statusId);
                }
            }
            else
            {
                // Agar selection khatam kar di ya -1 hai toh null pass karein (Saara data load hoga)
                gridload(dgvResults, null);
            }
        }

        private void cmb_Status_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                cmb_Status.SelectedIndex = -1;
                gridload(dgvResults, null);
            }
        }
    }
}

