using SupplyBranch.DAL;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
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
    public partial class frmDenomination : Form
    {

        DenominationDAL dal = new DenominationDAL();

        private int CurrentDenominationID = 0;

        StampCategoryDAL categoryDAL = new StampCategoryDAL();

        

        private void FillControls()
        {
            if (dgvDenomination.CurrentRow == null)
                return;

            CurrentDenominationID = Convert.ToInt32(dgvDenomination.CurrentRow.Cells["DenominationID"].Value);

            cmbCategory.SelectedValue = dgvDenomination.CurrentRow.Cells["CategoryID"].Value;
             txtDenomination.Text = dgvDenomination.CurrentRow.Cells["Denomination"].Value.ToString();


            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            cmbCategory.Focus();
        }

        private void LoadData()
        {
            dgvDenomination.DataSource = dal.GetAll();

            dgvDenomination.Columns["DenominationID"].HeaderText = "ID";
            dgvDenomination.Columns["Category"].HeaderText = "Category";
            dgvDenomination.Columns["Denomination"].HeaderText = "Denomination";
            dgvDenomination.Columns["CategoryID"].Visible = false;
        }

        private void ResetForm()
        {
            CurrentDenominationID = 0;

            cmbCategory.SelectedIndex = -1;

            txtDenomination.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            cmbCategory.Focus();

            dgvDenomination.ClearSelection();
        }

        private void LoadCategories()
        {
            cmbCategory.DataSource = categoryDAL.GetCategoryList();

            cmbCategory.DisplayMember = "Name";

            cmbCategory.ValueMember = "CategoryID";

            cmbCategory.SelectedIndex = -1;
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvDenomination.DataSource = dal.Search(txtSearch.Text.Trim());
        }

        private bool ValidateData()
        {
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select category.");

                cmbCategory.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDenomination.Text))
            {
                MessageBox.Show("Please enter denomination.");

                txtDenomination.Focus();

                return false;
            }

            decimal denomination;

            if (!decimal.TryParse(txtDenomination.Text.Trim(), out denomination))
            {
                MessageBox.Show("Please enter a valid denomination.");

                txtDenomination.Focus();

                return false;
            }

            return true;
        }



        public frmDenomination()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
          
            
        }

        

        private void frmDenomination_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            LoadCategories();

            LoadData();

            ResetForm();
        }

        private void txtDenomination_KeyPress(object sender, KeyPressEventArgs e)
        {
          
            // Digits, Backspace aur sirf ek Decimal Point allow hoga

            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Sirf ek decimal point allow kare
            if (e.KeyChar == '.' && txtDenomination.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadData();
            }
            else
            {
                dgvDenomination.DataSource = dal.Search(txtSearch.Text.Trim());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            decimal denomination = Convert.ToDecimal(txtDenomination.Text.Trim());

            if (dal.Exists(Convert.ToInt32(cmbCategory.SelectedValue),
                           denomination,
                           CurrentDenominationID))
            {
                MessageBox.Show("This denomination already exists in the selected category.");

                txtDenomination.Focus();
                return;
            }

            DenominationModel model = new DenominationModel();

            model.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
            model.Denomination = denomination;

            if (dal.Insert(model))
            {
                MessageBox.Show("Denomination saved successfully.");

                LoadData();

                ResetForm();
            }
            else
            {
                MessageBox.Show("Record could not be saved.");
            }
        }

        private void dgvDenomination_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex < 0)
                return;

            FillControls();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
          
            if (!ValidateData())
                return;

            decimal denomination = Convert.ToDecimal(txtDenomination.Text.Trim());

            if (dal.Exists(Convert.ToInt32(cmbCategory.SelectedValue),
                           denomination,
                           CurrentDenominationID))
            {
                MessageBox.Show("This denomination already exists in the selected category.");

                txtDenomination.Focus();
                return;
            }

            DenominationModel model = new DenominationModel();

            model.DenominationID = CurrentDenominationID;
            model.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
            model.Denomination = denomination;

            if (dal.Update(model))
            {
                MessageBox.Show("Record updated successfully.");

                LoadData();

                ResetForm();
            }
            else
            {
                MessageBox.Show("Record could not be updated.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
            if (CurrentDenominationID == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this record?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            if (dal.Delete(CurrentDenominationID))
            {
                MessageBox.Show("Record deleted successfully.");

                LoadData();

                ResetForm();
            }
            else
            {
                MessageBox.Show("Record could not be deleted.");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
