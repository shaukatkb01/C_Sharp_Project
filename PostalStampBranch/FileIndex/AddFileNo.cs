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
    public partial class AddFileNo : Form
    {
        // all txtcontroll clear function 
        private void ClearAllTextboxes(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                // Agar cheez TextBox hai
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Clear();
                }

                // Agar TextBox kisi Panel ya GroupBox ke andar hai
                if (ctrl.HasChildren)
                {
                    ClearAllTextboxes(ctrl);
                }
            }
        }
        // file number genrater function
        private string GenerateNewFileNumber()
        {
            string nextFileNo = "";
            string currentYear = DateTime.Now.Year.ToString();
            string prefix = "Ps.3-";

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // SQL Query mein humne IN (1,2,3,4) laga diya hai
                // Taake sirf in types ka latest number mil sakay
                string query = @"SELECT TOP 1 FileNo FROM FileIndex 
                         WHERE FileType IN (1, 2, 3, 4) 
                         AND FileNo LIKE 'Ps.3-%' 
                         ORDER BY Id DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string? lastFileNo = result?.ToString(); // Maslan "Ps.3-58/2025"

                    // Saal aur Number ko alag karna
                    string[]? parts = lastFileNo?.Split('/');
                    string? lastYear = parts?[1];

                    // Prefix hata kar sirf number (58) nikalna
                    string? lastCountStr = parts?[0].Replace(prefix, "");
                    int lastCount = int.Parse(lastCountStr);

                    if (lastYear == currentYear)
                    {
                        // Agar saal wahi hai toh +1 kar do
                        int newCount = lastCount + 1;
                        nextFileNo = prefix + newCount.ToString() + "/" + currentYear;
                    }
                    else
                    {
                        // Naya saal shuru ho gaya toh dobara 01
                        nextFileNo = prefix + "01/" + currentYear;
                    }
                }
                else
                {
                    // Agar pehle is type ki koi file maujood nahi
                    nextFileNo = prefix + "01/" + currentYear;
                }
            }
            return nextFileNo;
        }
        public AddFileNo()
        {
            InitializeComponent();
        }

        private void AddFileNo_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            // using conntion open
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // connection open
                con.Open();

                string query = "SELECT Id, FileType FROM FileType";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                fileTypeCmb.DataSource = dt;
                fileTypeCmb.DisplayMember = "FileType";
                fileTypeCmb.ValueMember = "Id";
                fileTypeCmb.SelectedIndex = -1;

            }

        }

        private void fileTypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Pehle check karein ke kuch select hua bhi hai ya nahi
            if (fileTypeCmb.SelectedValue != null && fileTypeCmb.SelectedIndex != -1)
            {

                // 2. Selected Value (ID) ko pakrein
                // Note: Hum try-parse use karte hain taake error na aaye
                int selectedId;
                bool isValid = int.TryParse(fileTypeCmb.SelectedValue.ToString(), out selectedId);
                if (selectedId >= 1 && selectedId <= 4)
                {
                    newFileNoTxt.Text = GenerateNewFileNumber();
                    newFileNoTxt.Enabled = false;
                }
                else 
                { 
                    newFileNoTxt.Clear(); 
                    newFileNoTxt.Enabled=true;
                }
                if (isValid)
                {
                    // 3. Logic: ID ke mutabiq text set karein
                    switch (selectedId)
                    {
                        case 1:
                            subjectTxt.Text = "Issuance the Commemorative Postage Stamp";
                            break;
                        case 2:
                            subjectTxt.Text = "Issuance the Commemorative Postage stamp with Souvenir Sheet";
                            break;
                        case 3:
                            subjectTxt.Text = "Issuance the Special Postage Stamp";
                            break;
                        case 4:
                            subjectTxt.Text = "Issuance the Commemorative Souvenir Sheet";
                            break;
                        case 5:
                        case 6:
                            subjectTxt.Text = "General File";
                            break;
                        default:
                            subjectTxt.Text = ""; // Agar koi aur ID ho toh khali rakhein
                            break;
                    }
                }
            }

        }

        private void addFileNoBtn_Click(object sender, EventArgs e)
        {
          
            // 1. Pehle Check karein ke zaroori cheezein khali to nahi
            if (fileTypeCmb.SelectedIndex == -1 || string.IsNullOrWhiteSpace(newFileNoTxt.Text))
            {
                MessageBox.Show("Please select File Type and ensure File Number is generated.");
                return;
            }

            // 2. Status ka faisla (1,2,3,4 ke liye Status 2, warna 3)
            int selectedTypeId = Convert.ToInt32(fileTypeCmb.SelectedValue);
            int statusValue = (selectedTypeId >= 1 && selectedTypeId <= 4) ? 2 : 3;

            // 3. Database Connection
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // SQL Query
                string query = @"INSERT INTO FileIndex (FileType, FileNo, FileSubject, dateOfCreation, Remark, Status) 
                         VALUES (@type, @no, @subject, @date, @remark, @status)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Values ko parameters ke sath jorein (Mapping)
                    cmd.Parameters.AddWithValue("@type", selectedTypeId);
                    cmd.Parameters.AddWithValue("@no", newFileNoTxt.Text.Trim());
                    cmd.Parameters.AddWithValue("@subject", subjectTxt.Text.Trim());
                    cmd.Parameters.AddWithValue("@date", dateOfCreationPick.Value); // DateTimePicker ki value
                    cmd.Parameters.AddWithValue("@remark", remarkTxt.Text.Trim());
                    cmd.Parameters.AddWithValue("@status", statusValue);

                    try
                    {
                        con.Open();
                        int rows = cmd.ExecuteNonQuery(); // Command chalane ke liye

                        if (rows > 0)
                        {
                            MessageBox.Show("File Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Yaad hai humne ClearFields ka function banaya tha? Use yahan call karein
                            ClearAllTextboxes(this);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        
    }
    }
}
