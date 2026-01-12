using System;
using System.Data;
using System.Windows.Forms;
using System.Xaml;
using System.Configuration;
using System.Drawing;
using Microsoft.Data.SqlClient;


namespace FileIndex
{

    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent(); // ❗ لازمی
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
        private void loginBtn_MouseEnter(object sender, EventArgs e)
        {
            loginBtn.BackColor = Color.DarkBlue;
        }
        private void loginBtn_MouseLeave(object sender, EventArgs e)
        {
            loginBtn.BackColor = Color.FromArgb(0, 165, 255);
        }
        private void loginBtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {

                con.Open();
                //1.Inputs Check karein
                if (string.IsNullOrWhiteSpace(userTxt.Text) ||
     string.IsNullOrWhiteSpace(passwordTxt.Text))
                {
                    MessageBox.Show(
                        "Please enter both username and password.",
                        "Missing Info",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                try
                {




                    // Query 1: Kya ye user database mein maujood hai?
                    string query = @"SELECT COUNT(1) FROM UserInfo 
                             WHERE UserName = @userName AND Password = @Password";

                    using (SqlCommand cmd1 = new SqlCommand(query, con))
                    {
                        cmd1.Parameters.AddWithValue("@userName", userTxt.Text.Trim());
                        cmd1.Parameters.AddWithValue("@Password", passwordTxt.Text.Trim());

                        int result = Convert.ToInt32(cmd1.ExecuteScalar());

                        if (result == 1)
                        {
                            // Query 2: Sirf ISI USER ka Auth_ID check karein (query update ki hai)
                            string query2 = @"SELECT COUNT(1) FROM UserInfo 
                                     WHERE UserName = @userName AND Athu_ID IS NOT NULL";

                            using (SqlCommand cmd2 = new SqlCommand(query2, con))
                            {
                                cmd2.Parameters.AddWithValue("@userName", userTxt.Text.Trim());
                                int result2 = Convert.ToInt32(cmd2.ExecuteScalar());

                                if (result2 >= 1)
                                {
                                    MessageBox.Show("Login successful. Welcome!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    Main mainform = new Main();
                                    mainform.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Access Denied. Your account is not authorized (Auth_ID missing).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Error: App.config mein connection string nahi mili. Check karein ke wahan name='DBCon' sahi likha hai.", "Config Error");
                }
                catch (Exception ex)
                {
                    // Ye line aapko asal masla batayegi (e.g. Table ka naam ghalat hona)
                    MessageBox.Show("Database Error: " + ex.Message, "Error");
                }
            }
        }

        private void showPasswordCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (showPasswordCheck.Checked)
            {
                // Password dikhao (Dots khatam kar do)
                passwordTxt.UseSystemPasswordChar = false;
            }
            else
            {
                // Password chupa do (Dots wapis lao)
                passwordTxt.UseSystemPasswordChar = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register regForm = new Register();

            // Hide the login window so the app stays running.
            this.Hide();

            // When the registration form closes, show the login form again.
            regForm.FormClosed += (s, args) => this.Show();

            regForm.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
