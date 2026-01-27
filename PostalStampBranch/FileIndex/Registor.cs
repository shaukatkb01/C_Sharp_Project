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
    public partial class Register : Form
    {

        public Register()
        {
            InitializeComponent();
        }

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
        private void button1_Click(object sender, EventArgs e)
        {
            // 🔹 Step 0: Basic validation
            if (string.IsNullOrWhiteSpace(textFullName.Text) ||
                string.IsNullOrWhiteSpace(texUserName.Text) ||
                string.IsNullOrWhiteSpace(textEmail.Text) ||
                string.IsNullOrWhiteSpace(textPassword.Text))
            {
                MessageBox.Show(
                    "All fields are required. Please complete the form.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();

                // 🔹 Step 1: Check if username already exists
                string checkQuery =
                    "SELECT COUNT(*) FROM UsersInfo WHERE UserName = @UserName";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50)
                            .Value = texUserName.Text.Trim();

                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        MessageBox.Show(
                            "This username is already registered.\nPlease choose a different username.",
                            "Username Already Exists",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        texUserName.Focus();
                        return; // ⛔ stop registration
                    }
                }

                // 🔹 Step 2: Insert new user
                string insertQuery = @"
            INSERT INTO UsersInfo (FullName, UserName, Password, UserEmail)
            VALUES (@FullName, @UserName, @Password, @UserEmail)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                {
                    insertCmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 100)
                             .Value = textFullName.Text.Trim();

                    insertCmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50)
                             .Value = texUserName.Text.Trim();

                    insertCmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100)
                             .Value = textPassword.Text.Trim(); // (hash later)

                    insertCmd.Parameters.Add("@UserEmail", SqlDbType.NVarChar, 100)
                             .Value = textEmail.Text.Trim();

                    insertCmd.ExecuteNonQuery();
                }

                // 🔹 Step 3: Success message
                MessageBox.Show(
                    "Registration was successful.\nYou can now log in.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            // all textbox clear 
            ClearAllTextboxes(this);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Main regmain = new Main();
            if (regmain.Visible == true)
            {
                regmain.Hide();
            }
            Login regForm = new Login();

            regForm.Show();

            this.Hide();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);
        }

        private void checkRegisterAgree_CheckedChanged(object sender, EventArgs e)
        {
            if (checkRegisterAgree.Checked)
            {
                string agreement = "Terms of Service:\n\n" +
                                  "1. Data will be stored in SQL Server.\n" +
                                  "2. User is responsible for password security.\n" +
                                  "3. Unauthorized access is prohibited.\n\n" +
                                  "Do you accept these terms?";

                DialogResult result = MessageBox.Show(agreement, "User Agreement", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    btnRegister.Enabled = true;
                }
                else
                {
                    checkRegisterAgree.Checked = false;
                    btnRegister.Enabled = false;
                }
            }
            else
            {
                btnRegister.Enabled = false;
            }
        }

        private void texUserName_Validating(object sender, CancelEventArgs e)
        {

        }

        private void textEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!textEmail.Text.Contains("@"))
            {
                e.Cancel = true; // User ko textbox se bahar nahi jane dega
                MessageBox.Show("Email adress must have '@'!");
            }
        }
    }
}
