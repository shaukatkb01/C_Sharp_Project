using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Masters
{
    public partial class frmEditUser : Form
    {
        private int _userID;
        private UserDAL userDAL;

        
        public frmEditUser(int userID)
        {

            InitializeComponent();
            _userID = userID;
            userDAL = new UserDAL();

       
        
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

                // ==========================================
                // Check duplicate UserName
                // ==========================================

                if (userDAL.UserNameExistsForOtherUser(
                        txtUserName.Text,
                        _userID))
                {
                    MessageBox.Show(
                        "This User Name already belongs to another user.",
                        "Duplicate User Name",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtUserName.Focus();
                    txtUserName.SelectAll();

                    return;
                }

                // ==========================================
                // Update
                // ==========================================

                bool updated = userDAL.UpdateUser(
                    _userID,
                    txtFullName.Text,
                    txtUserName.Text,
                    txtEmail.Text,
                    txtAuthorityID.Text);

                if (!updated)
                    return;

                MessageBox.Show(
                    "User updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update user.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void frmEditUser_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
         

            try
            {
                DataTable dt = userDAL.GetUserByID(_userID);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "User not found.",
                        "User Management",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();

                    return;
                }

                DataRow row = dt.Rows[0];

                txtFullName.Text = row["FullName"].ToString();

                txtUserName.Text = row["UserName"].ToString();

                txtEmail.Text =
                    row["UserEmail"] == DBNull.Value
                        ? ""
                        : row["UserEmail"].ToString();

                txtAuthorityID.Text =
                    row["Athu_ID"] == DBNull.Value
                        ? ""
                        : row["Athu_ID"].ToString();
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
    }
}
