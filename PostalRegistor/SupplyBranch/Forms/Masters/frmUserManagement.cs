using SupplyBranch.Forms.Administration;
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
    public partial class frmUserManagement : Form
    {
        private UserDAL userDAL = new UserDAL();

        private void LoadUsers()
        {
            try
            {
                dgvUsers.DataSource = userDAL.GetAllUsers();

                if (dgvUsers.Columns.Contains("UserID"))
                    dgvUsers.Columns["UserID"].Visible = false;

                if (dgvUsers.Columns.Contains("FullName"))
                    dgvUsers.Columns["FullName"].HeaderText = "Full Name";

                if (dgvUsers.Columns.Contains("UserName"))
                    dgvUsers.Columns["UserName"].HeaderText = "User Name";

                if (dgvUsers.Columns.Contains("UserEmail"))
                    dgvUsers.Columns["UserEmail"].HeaderText = "Email";

                if (dgvUsers.Columns.Contains("Athu_ID"))
                    dgvUsers.Columns["Athu_ID"].HeaderText = "Authority";

                dgvUsers.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load users.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public frmUserManagement()
        {
            InitializeComponent();
        }

        private void frmUserManagement_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            LoadUsers();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (frmAddUser form = new frmAddUser())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
         
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a user first.",
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int userID = Convert.ToInt32(
                dgvUsers.SelectedRows[0]
                    .Cells["UserID"]
                    .Value);

            using (frmEditUser form =
                   new frmEditUser(userID))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
       
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a user first.",
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int userID = Convert.ToInt32(
                dgvUsers.SelectedRows[0]
                    .Cells["UserID"]
                    .Value);

            using (frmAdminChangePassword form =
                   new frmAdminChangePassword(userID))
            {
                form.ShowDialog(this);
            }
        }
    }
}
