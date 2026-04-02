using Microsoft.Data.SqlClient;
using PostalStampSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace FileIndex
{
    public partial class SearchComm : Form
    {
        public SearchComm()
        {
            InitializeComponent();
        }

        private void SearchComm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            dgvResults.AutoGenerateColumns = true;
        }
        private void SearchData(string searchTerm, DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    // 1. Query mein StampImage table ko JOIN kiya aur Path uthaya
                    string query = @"SELECT 
                                c.IssueId, c.IssueNo, c.DateOfIssue, 
                                f.FileNo AS [FileNumber], t.FileType AS [IssueType], 
                                f.FileSubject, p.StampPrice, p.SouvenirPrice, c.Remarks,
                                s.StampImagePath, SouvenirImagePath -- Path yahan se aayega
                             FROM CommStamp c 
                             INNER JOIN FileIndex f ON c.FileNo = f.Id 
                             LEFT JOIN StockPrice p ON c.FileNo = p.FileNo 
                             LEFT JOIN FileType t ON f.FileType = t.Id 
                             LEFT JOIN StampImage s ON c.IssueNo = s.IssueNo -- Image table join kiya
                             WHERE (c.DateOfIssue BETWEEN @fromDate AND @toDate) 
                             AND (f.FileNo LIKE @search OR f.FileSubject LIKE @search OR c.IssueNo LIKE @search)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1).AddTicks(-1));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // 2. DataTable mein aik naya Column add karein jo Picture hold karega
                    dt.Columns.Add("Picture", typeof(byte[]));

                    foreach (DataRow row in dt.Rows)
                    {
                        string path = row["StampImagePath"]?.ToString();
                        if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                        {
                            // Path se asli image read karke bytes mein convert ki
                            row["Picture"] = System.IO.File.ReadAllBytes(path);
                        }
                    }

                    dgvResults.DataSource = dt;

                    // 3. UI Settings (Image column ko set karna)
                    if (dt.Rows.Count > 0)
                    {
                        // Path wala column chhupa dein, kyunke humne "Picture" column dikhana hai
                        if (dgvResults.Columns.Contains("StampImagePath")) dgvResults.Columns["StampImagePath"].Visible = false;
                        if (dgvResults.Columns.Contains("IssueId")) dgvResults.Columns["IssueId"].Visible = false;

                        // Image Column ki setting
                        if (dgvResults.Columns.Contains("Picture"))
                        {
                            DataGridViewImageColumn imgCol = (DataGridViewImageColumn)dgvResults.Columns["Picture"];
                            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom; // Image ko box mein fit karein
                            imgCol.HeaderText = "Stamp";
                            imgCol.Width = 60;
                        }

                        dgvResults.RowTemplate.Height = 60; // Row ki height thori barha dein taake image saaf dikhe
                        dgvResults.Columns["FileSubject"].Width = 400;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search Error: " + ex.Message);
                }
            }
        }







        private void exportBtn_Click(object sender, EventArgs e)
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

                    dt.Rows[0]["FromDate"] = dtpFrom.Value.ToString("dd-MMM-yyyy");
                    dt.Rows[0]["ToDate"] = dtpTo.Value.ToString("dd-MMM-yyyy");
                }

                // Baqi code wahi hai
                frmReportView reportForm = new frmReportView();
                string rpath = Path.Combine(Application.StartupPath, "Report", "rptCommSearch.rdlc");

                reportForm.LoadReport(dt, "dtCommSearch", rpath);
                reportForm.Show();
            }
            else
            {
                MessageBox.Show("Pehle search karein taake data grid mein aa jaye.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                searchBtn.Enabled = true;
                txtSearch.Text = "";
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                dtpFrom.Focus();
                dtpFrom.Select();
            }
            else
            {
                dtpFrom.Value = new DateTime(1947, 8, 14);
                dtpTo.Value = DateTime.Today;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                searchBtn.Enabled = true;
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            SearchData(txtSearch.Text, dtpFrom.Value, dtpTo.Value);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgvResults.DataSource = null;
                return;
            }

            // Ab yahan 3 arguments bhejein: Text, Start Date, aur End Date
            SearchData(txtSearch.Text, dtpFrom.Value, dtpTo.Value);
        }

        private void dgvResults_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Sirf tab chale jab mouse "Picture" column par ho
            if (e.RowIndex >= 0 && dgvResults.Columns[e.ColumnIndex].Name == "Picture")
            {
                var cellValue = dgvResults.Rows[e.RowIndex].Cells["Picture"].Value;

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    // 1. Picture load karein
                    byte[] imgBytes = (byte[])cellValue;
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        picPopup.Image = Image.FromStream(ms);
                    }

                    // 2. Parent aur Z-Order set karein (Taake Grid ke upar nazar aaye)
                    if (picPopup.Parent != this)
                    {
                        picPopup.Parent = this;
                    }
                    picPopup.BringToFront();

                    // 3. Left Side Positioning Logic
                    // Mouse ki position Form ke hisaab se nikalna
                    Point mousePos = this.PointToClient(Cursor.Position);

                    // Mouse ke X se PictureBox ki Width minus karein taake wo Left par nazar aaye
                    int posX = mousePos.X - picPopup.Width - 20;
                    int posY = mousePos.Y -100;

                    // Boundary Check: Agar Left par jagah na ho (X negative ho jaye), toh Right par dikhao
                    if (posX < 0)
                    {
                        posX = mousePos.X + 20;
                    }

                    // Final Location set karein
                    picPopup.Location = new Point(posX, posY);
                    picPopup.Visible = true;
                }
            }
        }

        private void dgvResults_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            picPopup.Visible = false; // Popup ko chhupa dein
        }
    }
}

