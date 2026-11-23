using SupplyBranch.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Masters
{
    public partial class frmModuleManagement : Form
    {
        private readonly ModuleDAL moduleDAL = new ModuleDAL();
        private int selectedModuleID = 0;
        private bool isLoadingGrid = false;

        public frmModuleManagement()
        {
            InitializeComponent();
        }

        private void frmModuleManagement_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            LoadModules();
            ClearInputs();
        }

        private void LoadModules()
        {
            try
            {
                isLoadingGrid = true;

                DataTable dt = moduleDAL.GetAllModules();

                if (!dt.Columns.Contains("StatusText"))
                {
                    dt.Columns.Add("StatusText", typeof(string));
                }

                foreach (DataRow row in dt.Rows)
                {
                    bool active = Convert.ToBoolean(row["IsActive"]);
                    row["StatusText"] = active ? "Active" : "Inactive";
                }

                dgvModules.DataSource = dt;

                if (dgvModules.Columns.Contains("ModuleID"))
                    dgvModules.Columns["ModuleID"].Visible = false;

                if (dgvModules.Columns.Contains("IsActive"))
                    dgvModules.Columns["IsActive"].Visible = false;

                if (dgvModules.Columns.Contains("ModuleCode"))
                {
                    dgvModules.Columns["ModuleCode"].HeaderText = "Module Code";
                    dgvModules.Columns["ModuleCode"].FillWeight = 22;
                }

                if (dgvModules.Columns.Contains("ModuleName"))
                {
                    dgvModules.Columns["ModuleName"].HeaderText = "Module Name";
                    dgvModules.Columns["ModuleName"].FillWeight = 43;
                }

                if (dgvModules.Columns.Contains("SortOrder"))
                {
                    dgvModules.Columns["SortOrder"].HeaderText = "Order";
                    dgvModules.Columns["SortOrder"].FillWeight = 15;
                }

                if (dgvModules.Columns.Contains("StatusText"))
                {
                    dgvModules.Columns["StatusText"].HeaderText = "Status";
                    dgvModules.Columns["StatusText"].FillWeight = 20;
                }

                dgvModules.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load modules.\n\n" + ex.Message,
                    "Module Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingGrid = false;
                UpdateToggleButtonState();
            }
        }

        private void ClearInputs()
        {
            selectedModuleID = 0;
            txtModuleCode.Clear();
            txtModuleName.Clear();
            numSortOrder.Value = 1;
            chkIsActive.Checked = true;
            btnSaveModule.Text = "Save Module";
            dgvModules.ClearSelection();
            UpdateToggleButtonState();
            txtModuleCode.Focus();
        }

        private void UpdateToggleButtonState()
        {
            if (dgvModules.SelectedRows.Count == 0 || selectedModuleID == 0)
            {
                btnToggleModule.Enabled = false;
                btnToggleModule.Text = "Disable Module";
                return;
            }

            DataGridViewRow row = dgvModules.SelectedRows[0];
            bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value);

            btnToggleModule.Enabled = true;
            btnToggleModule.Text = isActive ? "Disable Module" : "Enable Module";
        }

        private void dgvModules_SelectionChanged(object sender, EventArgs e)
        {
            if (isLoadingGrid)
                return;

            if (dgvModules.SelectedRows.Count == 0)
            {
                selectedModuleID = 0;
                btnSaveModule.Text = "Save Module";
                UpdateToggleButtonState();
                return;
            }

            DataGridViewRow row = dgvModules.SelectedRows[0];

            selectedModuleID = Convert.ToInt32(row.Cells["ModuleID"].Value);
            txtModuleCode.Text = Convert.ToString(row.Cells["ModuleCode"].Value);
            txtModuleName.Text = Convert.ToString(row.Cells["ModuleName"].Value);

            int sortOrder = Convert.ToInt32(row.Cells["SortOrder"].Value);
            if (sortOrder < numSortOrder.Minimum) sortOrder = (int)numSortOrder.Minimum;
            if (sortOrder > numSortOrder.Maximum) sortOrder = (int)numSortOrder.Maximum;
            numSortOrder.Value = sortOrder;

            chkIsActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
            btnSaveModule.Text = "Update Module";

            UpdateToggleButtonState();
        }

        private void btnSaveModule_Click(object sender, EventArgs e)
        {
            string code = txtModuleCode.Text.Trim().ToUpper();
            string name = txtModuleName.Text.Trim();
            int sortOrder = Convert.ToInt32(numSortOrder.Value);
            bool isActive = chkIsActive.Checked;

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show(
                    "Please enter a Module Code.",
                    "Module Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtModuleCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "Please enter a Module Name.",
                    "Module Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtModuleName.Focus();
                return;
            }

            try
            {
                if (moduleDAL.ModuleCodeExists(code, selectedModuleID))
                {
                    MessageBox.Show(
                        $"Module Code '{code}' already exists.",
                        "Module Management",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtModuleCode.Focus();
                    return;
                }

                bool success;
                if (selectedModuleID == 0)
                {
                    success = moduleDAL.InsertModule(code, name, sortOrder, isActive);
                }
                else
                {
                    success = moduleDAL.UpdateModule(selectedModuleID, code, name, sortOrder, isActive);
                }

                if (success)
                {
                    MessageBox.Show(
                        selectedModuleID == 0
                            ? "Module added successfully."
                            : "Module updated successfully.",
                        "Module Management",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadModules();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to save module.\n\n" + ex.Message,
                    "Module Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void btnToggleModule_Click(object sender, EventArgs e)
        {
            if (selectedModuleID == 0 || dgvModules.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a module first.",
                    "Module Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvModules.SelectedRows[0];
            string moduleName = Convert.ToString(row.Cells["ModuleName"].Value);
            bool currentActive = Convert.ToBoolean(row.Cells["IsActive"].Value);
            bool newActive = !currentActive;

            string actionText = newActive ? "enable" : "disable";
            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to {actionText} module '{moduleName}'?",
                "Module Management",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                if (moduleDAL.ToggleModuleStatus(selectedModuleID, newActive))
                {
                    MessageBox.Show(
                        newActive
                            ? "Module enabled successfully."
                            : "Module disabled successfully.",
                        "Module Management",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadModules();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update module status.\n\n" + ex.Message,
                    "Module Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}