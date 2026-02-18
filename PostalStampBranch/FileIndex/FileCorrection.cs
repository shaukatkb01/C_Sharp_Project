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
    public partial class FileCorrection : Form
    {
        public FileCorrection()
        {
            InitializeComponent();
        }

        // combo load for filecoreection

        private void ComboLoad(ComboBox? cmb, ComboBox? cmb2)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();
                string query = "SELECT Id, FileType, FileNo FROM FileIndex ORDER BY Id DESC";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                // FileNo aur FileType dono ko ComboBox mein load karna
                if (cmb != null)
                {
                    cmb.DataSource = dt;
                    cmb.DisplayMember = "FileNo";
                    cmb.ValueMember = "Id";
                    cmb.SelectedIndex = -1;

                }

                string query2 = "SELECT Id, FileType FROM FileType ORDER BY Id ASC";
                SqlDataAdapter adapter2 = new SqlDataAdapter(query2, con);
                DataTable dt2 = new DataTable();
                adapter2.Fill(dt2);
                if (cmb2 != null)
                {

                    cmb2.DataSource = dt2;
                    cmb2.DisplayMember = "FileType";
                    cmb2.ValueMember = "Id";
                    cmb2.SelectedIndex = -1;
                }
            }
        }
        private void FileCorrection_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ComboLoad(fileNoCmb, fileTypeCmb);
        }

        private void fileNoCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Initial Checks (Null aur DataRowView se bachne ke liye)
            if (fileNoCmb.SelectedValue == null || fileNoCmb.SelectedValue is System.Data.DataRowView)
            {
                return;
            }

            try
            {
                string? selectedId = fileNoCmb.SelectedValue.ToString();

                using (SqlConnection connection = new SqlConnection(Db.ConString))
                {
                    connection.Open();
                    string query = "SELECT FileNo, FileSubject, Remark, FileType FROM FileIndex WHERE Id = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", selectedId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        FileNoTxt.Text = reader["FileNo"].ToString();
                        subjectTxt.Text = reader["FileSubject"].ToString();
                        remarkTxt.Text = reader["Remark"].ToString();

                        // --- SAHI TARIQA (SelectedValue use karein) ---
                        if (reader["FileType"] != DBNull.Value)
                        {
                            int fileType = Convert.ToInt32(reader["FileType"]);

                            // Is se ComboBox khud us item par jump kar jayega jiski ID match hogi
                            fileTypeCmb.SelectedValue = fileType;

                            // --- Logic: Enable/Disable ---
                            // Array use karna zyada saaf (clean) lagta hai
                            int[] restrictedTypes = { 1, 2, 3, 4 };
                            if (restrictedTypes.Contains(fileType))
                            {
                                FileNoTxt.ReadOnly = true;
                                fileTypeCmb.DropDownStyle = ComboBoxStyle.DropDownList; // User ko dropdown kholne se rokna
                            }
                            else
                            {
                                FileNoTxt.ReadOnly = false;
                                fileTypeCmb.DropDownStyle = ComboBoxStyle.Simple; // User ko dropdown kholne se rokna
                            }
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
                                  FileType = @filetype,
                                 Remark = @remark
                             WHERE Id = @id";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // 4. TextBoxes se naya data utha kar parameters mein dalna
                    cmd.Parameters.AddWithValue("@fno", FileNoTxt.Text);
                    cmd.Parameters.AddWithValue("@subject", subjectTxt.Text);
                    cmd.Parameters.AddWithValue("@filetype", fileTypeCmb.SelectedValue);
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
            fileTypeCmb.SelectedIndex = -1;
        }

        private void FileNoTxt_Click(object sender, EventArgs e)
        {
            if (FileNoTxt.ReadOnly == true)
            {
                
                MessageBox.Show("You can't change General FileType here!",
                "System Restriction",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning); // Peela (Yellow) triangle icon aayega
            }
        }

        private void fileTypeCmb_Click(object sender, EventArgs e)
        {
            if (fileTypeCmb.DropDownStyle==ComboBoxStyle.Simple)
            {
                MessageBox.Show("You can't change General FileType here!",
                "System Restriction",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning); // Peela (Yellow) triangle icon aayega
            }
        }
    }
}
