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
    public partial class searchFile : Form
    {

        public searchFile()
        {
            InitializeComponent();
        }
        private void searchFile_Load(object sender, EventArgs e)
        {
            dgvResults.AutoGenerateColumns = true;
           
        }

        private void SearchData(string searchTerm)
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
                            dgvResults.Columns["FileType"].Width= 200;

                            // Subject ko bada rakhein kyunke is mein text zyada hota hai
                            dgvResults.Columns["FileSubject"].Width = 1000;

                            // 2. Column ka Heading text tabdeel karein (Jo user ko nazar aaye ga)
                            dgvResults.Columns["FileNo"].HeaderText = "File Number";
                            dgvResults.Columns["FileSubject"].HeaderText = "Subject / Details";
                            dgvResults.Columns["FileType"].HeaderText = "Type";

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
    }
}

