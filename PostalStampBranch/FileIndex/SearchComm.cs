using Microsoft.Data.SqlClient;
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
            dgvResults.AutoGenerateColumns = true;
        }
        private void SearchData(string searchTerm, DateTime fromDate, DateTime toDate)
        {

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    // Query: Date range filter aur Text search dono shamil hain
                    string query = @"SELECT 
                    c.Id, 
                    c.IssueNo, 
                    c.DateOfIssue, 
                    f.FileNo AS [FileNumber], 
                    f.FileSubject, 
                    p.StampPrice, 
                    p.SouvenirPrice,
                    c.Remarks
                    
                 FROM CommStamp c
                 INNER JOIN FileIndex f ON c.FileNo = f.Id 
                 LEFT JOIN Price p ON f.Id = p.FileNo
                 WHERE (c.DateOfIssue BETWEEN @fromDate AND @toDate) -- Date Filter
                 AND (
                        f.FileNo LIKE @search 
                        OR f.FileSubject LIKE @search 
                        OR c.IssueNo LIKE @search 
                        OR c.Remarks LIKE @search
                     )";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Parameters add karna (SQL Injection se bachne ke liye)
                    cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                    cmd.Parameters.AddWithValue("@fromDate", dtpFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpTo.Value.Date.AddDays(1).AddTicks(-1)); // Din ke aakhir tak ka data

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Data GridView mein load karna
                    dgvResults.DataSource = dt;

                    // UI ki setting (Columns fix karna)
                    if (dt.Rows.Count > 0 && dgvResults.Columns.Count > 0)
                    {
                        // Width settings
                        if (dgvResults.Columns.Contains("Id")) dgvResults.Columns["Id"].Visible = false; // ID chupa di
                        if (dgvResults.Columns.Contains("FileNumber")) dgvResults.Columns["FileNumber"].Width = 120;
                        if (dgvResults.Columns.Contains("IssueNo")) dgvResults.Columns["IssueNo"].Width = 150;
                        if (dgvResults.Columns.Contains("DateOfIssue"))
                        {
                            dgvResults.Columns["DateOfIssue"].Width = 120;
                            dgvResults.Columns["DateOfIssue"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                        }

                        // Subject ko zyada jagah dena
                        if (dgvResults.Columns.Contains("FileSubject"))
                        {
                            dgvResults.Columns["FileSubject"].Width = 350;
                            dgvResults.Columns["FileSubject"].HeaderText = "File Subject";
                        }

                        // Remarks ko bachi hui sari jagah dena
                        if (dgvResults.Columns.Contains("Remarks"))
                        {
                            dgvResults.Columns["Remarks"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

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

        private void searchBtn_Click(object sender, EventArgs e)
        {
            SearchData(txtSearch.Text, dtpFrom.Value, dtpTo.Value);
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
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                searchBtn.Enabled = true;
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
    }
    }

