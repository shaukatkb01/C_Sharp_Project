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
    public partial class frmAdminChangePassword : Form
    {

        private int _userID;
        private UserDAL userDAL;

        public frmAdminChangePassword(int userID)
        {

            

            InitializeComponent();

            _userID = userID;
            userDAL = new UserDAL();
        }


        private void frmAdminChangePassword_Load(object sender, EventArgs e)
        {
            txtCurrentPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;


            UITheme.Apply(this);

            try
            {
                DataTable dt = userDAL.GetUserByID(_userID);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "User not found.",
                        "Change Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    DialogResult = DialogResult.Cancel;
                    Close();

                    return;
                }

                txtUserName.Text =
                    Convert.ToString(dt.Rows[0]["UserName"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load user.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                MessageBox.Show(
                    "Please enter current password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCurrentPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show(
                    "Please enter new password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(
                    "New password and confirm password do not match.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtConfirmPassword.Focus();
                return;
            }

            bool changed = userDAL.ChangePassword(
                _userID,
                txtCurrentPassword.Text,
                txtNewPassword.Text);

            if (!changed)
            {
                MessageBox.Show(
                    "Current password is incorrect.",
                    "Change Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCurrentPassword.Clear();
                txtCurrentPassword.Focus();

                return;
            }

            MessageBox.Show(
                "Password changed successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
