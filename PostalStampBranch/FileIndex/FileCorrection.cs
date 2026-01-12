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
    public partial class FileCorrection : Form
    {
        public FileCorrection()
        {
            InitializeComponent();
        }

        private void FileCorrection_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {

                con.Open();
                string query = "SELECT Id, FileNo FROM FileIndex ORDER BY Id DESC";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                fileNoCmb.DataSource = dt;
                fileNoCmb.DisplayMember = "FileNo";
                fileNoCmb.ValueMember = "Id";
                fileNoCmb.SelectedIndex = -1;
            }
        }

        private void fileNoCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileNoCmb.SelectedValue == null || fileNoCmb.SelectedValue is System.Data.DataRowView)
            {
                return;
            }

            try
            {
                string selectedId = fileNoCmb.SelectedValue.ToString();

                using (SqlConnection connection = new SqlConnection(Db.ConString))
                {
                    connection.Open();

                    // 1. SELECT query mein 'FileType' ka izafa kiya
                    string query = "SELECT FileNo, FileSubject, Remark, FileType FROM FileIndex WHERE Id = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", selectedId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        FileNoTxt.Text = reader["FileNo"].ToString();
                        subjectTxt.Text = reader["FileSubject"].ToString();
                        remarkTxt.Text = reader["Remark"].ToString();

                        // 2. FileType ki value nikaalein
                        // Hum Convert.ToInt32 use kar rahe hain kyunki ye number hai
                        int fileType = Convert.ToInt32(reader["FileType"]);

                        // 3. Logic: Agar FileType 1, 2, 3, ya 4 ho
                        if (fileType == 1 || fileType == 2 || fileType == 3 || fileType == 4)
                        {
                            FileNoTxt.Enabled = false; // Control ko disable kar do
                        }
                        else
                        {
                            FileNoTxt.Enabled = true;  // Baqi types ke liye enable rakho
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void saveCorrectionBtn_Click(object sender, EventArgs e)
        {
            // 1. Pehle check karein ke ComboBox se koi File select hai bhi ya nahi
            if (fileNoCmb.SelectedValue == null || fileNoCmb.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a file first!");
                return;
            }

            // 2. Database connection kholna
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    con.Open();

                    // 3. SQL Update Query
                    // Iska matlab hai: "FileIndex table mein ye tabdeeliyaan karo WAHAAN jahan Id match kare"
                    string query = @"UPDATE FileIndex 
                             SET FileNo = @fno, 
                                 FileSubject = @subject, 
                                 Remark = @remark 
                             WHERE Id = @id";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // 4. TextBoxes se naya data utha kar parameters mein dalna
                    cmd.Parameters.AddWithValue("@fno", FileNoTxt.Text);
                    cmd.Parameters.AddWithValue("@subject", subjectTxt.Text);
                    cmd.Parameters.AddWithValue("@remark", remarkTxt.Text);
                    cmd.Parameters.AddWithValue("@id", fileNoCmb.SelectedValue); // Yeh wahi hidden ID hai

                    // 5. Query chalana
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("File Index data has been successfully updated!!");

                        // (Optrional) Fields ko khali kar dena
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("File Index data could not be updated!.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Ek chota sa function fields saaf karne ke liye
        private void ClearFields()
        {
            FileNoTxt.Clear();
            subjectTxt.Clear();
            remarkTxt.Clear();
            fileNoCmb.SelectedIndex = -1;
        }
    }
}
