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
using SupplyBranch;

namespace SupplyBranch.Forms.Masters
{
    public partial class frmUserManagement : Form
    {
        private UserDAL userDAL = new UserDAL();
        private readonly ModuleDAL moduleDAL = new ModuleDAL();
        private bool isLoadingPermissions = false;

        private sealed class ModuleCheckItem
        {
            public int ModuleID { get; set; }
            public string ModuleCode { get; set; }
            public string ModuleName { get; set; }

            public override string ToString()
            {
                return ModuleName;
            }
        }

        private void LoadModulesChecklist()
        {
            clbModules.DataSource = null;
            clbModules.Items.Clear();

            DataTable dtModules = moduleDAL.GetActiveModules();

            foreach (DataRow row in dtModules.Rows)
            {
                clbModules.Items.Add(new ModuleCheckItem
                {
                    ModuleID = Convert.ToInt32(row["ModuleID"]),
                    ModuleCode = Convert.ToString(row["ModuleCode"]),
                    ModuleName = Convert.ToString(row["ModuleName"])
                });
            }
        }

        private int GetSelectedUserID()
        {
            if (dgvUsers.SelectedRows.Count == 0 && dgvUsers.CurrentRow == null)
                return 0;

            DataGridViewRow row = dgvUsers.SelectedRows.Count > 0
                ? dgvUsers.SelectedRows[0]
                : dgvUsers.CurrentRow;

            if (row == null)
                return 0;

            foreach (DataGridViewColumn col in dgvUsers.Columns)
            {
                if (string.Equals(col.Name, "UserID", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.Name, "UserId", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.Name, "Id", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.DataPropertyName, "UserID", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.DataPropertyName, "Id", StringComparison.OrdinalIgnoreCase))
                {
                    object val = row.Cells[col.Index].Value;
                    if (val != null && val != DBNull.Value && int.TryParse(val.ToString(), out int id) && id > 0)
                        return id;
                }
            }

            if (row.Cells.Count > 0 && row.Cells[0].Value != null && row.Cells[0].Value != DBNull.Value)
            {
                if (int.TryParse(row.Cells[0].Value.ToString(), out int firstColId) && firstColId > 0)
                    return firstColId;
            }

            return 0;
        }

        private void LoadUserPermissionsInChecklist()
        {
            try
            {
                isLoadingPermissions = true;
                btnSavePermissions.Enabled = true;

                int userID = GetSelectedUserID();

                for (int i = 0; i < clbModules.Items.Count; i++)
                {
                    clbModules.SetItemChecked(i, false);
                }

                if (userID <= 0)
                    return;

                HashSet<int> allowedModuleIDs = moduleDAL.GetUserModuleIDs(userID);

                for (int i = 0; i < clbModules.Items.Count; i++)
                {
                    if (clbModules.Items[i] is ModuleCheckItem item)
                    {
                        clbModules.SetItemChecked(i, allowedModuleIDs.Contains(item.ModuleID));
                    }
                }
            }
            finally
            {
                isLoadingPermissions = false;
            }
        }

        private void UpdateUserToggleButton()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                btnToggleUser.Enabled = false;
                btnToggleUser.Text = "Disable User";
                return;
            }

            DataGridViewRow row = dgvUsers.SelectedRows[0];

            bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value);

            btnToggleUser.Enabled = true;
            btnToggleUser.Text = isActive ? "Disable User" : "Enable User";
        }

        private void LoadUsers()
        {
            try
            {
                DataTable dt = userDAL.GetAllUsers();

                if (!dt.Columns.Contains("Status"))
                {
                    dt.Columns.Add("Status", typeof(string));
                }

                foreach (DataRow row in dt.Rows)
                {
                    bool isActive = Convert.ToBoolean(row["IsActive"]);
                    row["Status"] = isActive ? "Active" : "Disabled";
                }

                dgvUsers.DataSource = dt;

                if (dgvUsers.Columns.Contains("UserID"))
                    dgvUsers.Columns["UserID"].Visible = false;

                if (dgvUsers.Columns.Contains("IsActive"))
                    dgvUsers.Columns["IsActive"].Visible = false;

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

                if (dgvUsers.Rows.Count > 0)
                {
                    dgvUsers.Rows[0].Selected = true;
                }
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
            LoadModulesChecklist();
            LoadUsers();
            LoadUserPermissionsInChecklist();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (frmAddUser form = new frmAddUser())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsers();
                    LoadUserPermissionsInChecklist();
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

            using (frmEditUser form = new frmEditUser(userID))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsers();
                    LoadUserPermissionsInChecklist();
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

            using (frmAdminChangePassword form = new frmAdminChangePassword(userID))
            {
                form.ShowDialog(this);
            }
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            UpdateUserToggleButton();
            LoadUserPermissionsInChecklist();
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

            int userID = Convert.ToInt32(row.Cells["UserID"].Value);
            string userName = Convert.ToString(row.Cells["UserName"].Value);
            bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value);

            string action = isActive ? "disable" : "enable";

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
                LoadUserPermissionsInChecklist();
            }
        }

        private void btnSavePermissions_Click_1(object sender, EventArgs e)
        {
            if (isLoadingPermissions)
                return;

            int userID = GetSelectedUserID();
            if (userID <= 0)
            {
                MessageBox.Show(
                    "Please select a user first.",
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                List<int> selectedModuleIDs = new List<int>();

                foreach (object checkedItem in clbModules.CheckedItems)
                {
                    if (checkedItem is ModuleCheckItem item)
                    {
                        selectedModuleIDs.Add(item.ModuleID);
                    }
                }

                moduleDAL.SaveUserModulePermissions(userID, selectedModuleIDs);
                LoadUserPermissionsInChecklist();

                MessageBox.Show(
                    "Module permissions saved successfully.",
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to save module permissions.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}