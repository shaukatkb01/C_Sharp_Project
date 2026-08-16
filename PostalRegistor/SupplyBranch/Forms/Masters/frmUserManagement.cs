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

        private void UpdateUserToggleButton()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                btnToggleUser.Enabled = false;
                btnToggleUser.Text = "Disable User";
                return;
            }

            DataGridViewRow row = dgvUsers.SelectedRows[0];

            bool isActive = Convert.ToBoolean(
                row.Cells["IsActive"].Value);

            btnToggleUser.Enabled = true;

            if (isActive)
            {
                btnToggleUser.Text = "Disable User";
            }
            else
            {
                btnToggleUser.Text = "Enable User";
            }
        }
        private void LoadUsers()
        {
            try
            {
                DataTable dt = userDAL.GetAllUsers();

                // Create Status column in DataTable
                if (!dt.Columns.Contains("Status"))
                {
                    dt.Columns.Add("Status", typeof(string));
                }

                // Fill Status before binding to DataGridView
                foreach (DataRow row in dt.Rows)
                {
                    bool isActive = Convert.ToBoolean(row["IsActive"]);

                    row["Status"] = isActive
                        ? "Active"
                        : "Disabled";
                }

                dgvUsers.DataSource = dt;

                // Hide UserID
                if (dgvUsers.Columns.Contains("UserID"))
                    dgvUsers.Columns["UserID"].Visible = false;

                // Hide IsActive
                if (dgvUsers.Columns.Contains("IsActive"))
                    dgvUsers.Columns["IsActive"].Visible = false;

                // Headers
                if (dgvUsers.Columns.Contains("FullName"))
                    dgvUsers.Columns["FullName"].HeaderText = "Full Name";

                if (dgvUsers.Columns.Contains("UserName"))
                    dgvUsers.Columns["UserName"].HeaderText = "User Name";

                if (dgvUsers.Columns.Contains("UserEmail"))
                    dgvUsers.Columns["UserEmail"].HeaderText = "Email";

                if (dgvUsers.Columns.Contains("Athu_ID"))
                    dgvUsers.Columns["Athu_ID"].HeaderText = "Authority";

                if (dgvUsers.Columns.Contains("Status"))
                {
                    dgvUsers.Columns["Status"].HeaderText = "Status";
                    dgvUsers.Columns["Status"].ReadOnly = true;
                }

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
            UpdateUserToggleButton();
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

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            UpdateUserToggleButton();
        }

        private void btnToggleUser_Click(object sender, EventArgs e)
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

            DataGridViewRow row = dgvUsers.SelectedRows[0];

            int userID = Convert.ToInt32(
                row.Cells["UserID"].Value);

            string userName =
                Convert.ToString(row.Cells["UserName"].Value);

            bool isActive =
                Convert.ToBoolean(row.Cells["IsActive"].Value);

            string action = isActive
                ? "disable"
                : "enable";

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to {action} user '{userName}'?",
                "User Management",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            bool newStatus = !isActive;

            if (userDAL.ToggleUserStatus(userID, newStatus))
            {
                MessageBox.Show(
                    newStatus
                        ? "User has been enabled successfully."
                        : "User has been disabled successfully.",
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadUsers();
            }
        }
    }
}
