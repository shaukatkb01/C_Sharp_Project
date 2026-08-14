using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Masters
{
    public partial class frmAddUser : Form
    {
        public frmAddUser()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
         
            try
            {
                // ==========================================
                // Validation
                // ==========================================

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show(
                        "Please enter Full Name.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtFullName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    MessageBox.Show(
                        "Please enter User Name.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtUserName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show(
                        "Please enter Password.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtPassword.Focus();
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show(
                        "Password and Confirm Password do not match.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtConfirmPassword.Focus();
                    return;
                }

                UserDAL userDAL = new UserDAL();

                if (userDAL.UserNameExists(txtUserName.Text))
                {
                    MessageBox.Show(
                        "This User Name already exists.\n\nPlease choose another User Name.",
                        "Duplicate User Name",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtUserName.Focus();
                    txtUserName.SelectAll();

                    return;
                }

                // ==========================================
                // Save User
                // ==========================================


                bool saved = userDAL.AddUser(
                    txtFullName.Text,
                    txtUserName.Text,
                    txtPassword.Text,
                    txtEmail.Text,
                    txtAuthorityID.Text);

                if (!saved)
                    return;

                MessageBox.Show(
                    "User added successfully.",
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to save user.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
        }
    }
}
