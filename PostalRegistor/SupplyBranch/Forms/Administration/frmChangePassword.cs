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
using static System.Collections.Specialized.BitVector32;

namespace SupplyBranch.Forms.Administration
{
    public partial class frmChangePassword : Form
    {
        UserDAL userDAL = new UserDAL();
        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                MessageBox.Show(
                    "Please enter current password.",
                    "Change Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCurrentPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show(
                    "Please enter new password.",
                    "Change Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show(
                    "Please confirm new password.",
                    "Change Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtConfirmPassword.Focus();
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(
                    "New password and confirm password do not match.",
                    "Change Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtConfirmPassword.Focus();
                return;
            }

            if (txtCurrentPassword.Text == txtNewPassword.Text)
            {
                MessageBox.Show(
                    "New password must be different from current password.",
                    "Change Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                return;
            }

            try
            {
                bool result = userDAL.ChangePassword(
                    CurrentUser.UserID,
                    txtCurrentPassword.Text,
                    txtNewPassword.Text);

                if (result)
                {
                    MessageBox.Show(
                        "Password changed successfully.",
                        "Change Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    txtCurrentPassword.Clear();
                    txtNewPassword.Clear();
                    txtConfirmPassword.Clear();

                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Current password is incorrect.",
                        "Change Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtCurrentPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Change Password Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
