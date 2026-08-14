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
    public partial class frmUnitConversionMaster : Form
    {
        UnitConversionDAL dal = new UnitConversionDAL();
        int EditID = 0;

        private void ClearForm()
        {
            EditID = 0;

            cmbCategory.SelectedIndex = -1;
            cmbDenomination.DataSource = null;

            txtPiecesPerSheet.Clear();
            txtRemarks.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;

            cmbCategory.Enabled = true;
            cmbDenomination.Enabled = true;

            cmbCategory.Focus();
        }
        private void LoadGrid()
        {
            dgvConversion.AutoGenerateColumns = false;

            dgvConversion.DataSource = dal.GetAll();
        }

        private void LoadCategory()
        {
            cmbCategory.DataSource = dal.GetCategories();

            cmbCategory.DisplayMember = "Name";

            cmbCategory.ValueMember = "CategoryID";

            cmbCategory.SelectedIndex = -1;
        }

        private void LoadDenomination(int categoryID)
        {
            cmbDenomination.DataSource = dal.GetDenominations(categoryID);

            cmbDenomination.DisplayMember = "Denomination";

            cmbDenomination.ValueMember = "DenominationID";

            cmbDenomination.SelectedIndex = -1;
        }


        public frmUnitConversionMaster()
        {
            InitializeComponent();
            
        }

        private void UnitConversionMaster_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            btnUpdate.Enabled = false;

            LoadCategory();
            LoadGrid();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        
            if (cmbCategory.SelectedValue == null)
                return;

            if (cmbCategory.SelectedValue is DataRowView)
                return;

            LoadDenomination(Convert.ToInt32(cmbCategory.SelectedValue));
        }

        private bool ValidateData()
        {
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select category.");

                cmbCategory.Focus();

                return false;
            }

            if (cmbDenomination.SelectedIndex == -1)
            {
                MessageBox.Show("Please select denomination.");

                cmbDenomination.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPiecesPerSheet.Text))
            {
                MessageBox.Show("Please enter Pieces Per Sheet.");

                txtPiecesPerSheet.Focus();

                return false;
            }

            if (!int.TryParse(txtPiecesPerSheet.Text, out int pieces))
            {
                MessageBox.Show("Pieces Per Sheet must be numeric.");

                txtPiecesPerSheet.Focus();

                return false;
            }

            if (pieces <= 0)
            {
                MessageBox.Show("Pieces Per Sheet must be greater than zero.");

                txtPiecesPerSheet.Focus();

                return false;
            }

            return true;
        }

        private void txtPiecesPerSheet_KeyPress(object sender, KeyPressEventArgs e)
        {
       
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
       
            if (!ValidateData())
                return;

            UnitConversionModel model = new UnitConversionModel();

            model.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);

            model.DenominationID = Convert.ToInt32(cmbDenomination.SelectedValue);

            model.PiecesPerSheet = Convert.ToInt32(txtPiecesPerSheet.Text);

            model.Remarks = txtRemarks.Text.Trim();

            if (dal.Save(model))
            {
                MessageBox.Show("Record saved successfully.");

                LoadGrid();
                ClearForm();

                // ClearForm();
                // LoadGrid();
            }
            else
            {
                MessageBox.Show("Record could not be saved.");
            }
        }

        private void dgvConversion_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            if (dgvConversion.Columns[e.ColumnIndex].Name == "Edit")
            {
                EditID = Convert.ToInt32(
                    dgvConversion.Rows[e.RowIndex].Cells["ConversionID"].Value);

                DataRow dr = dal.GetByID(EditID);

                if (dr != null)
                {
                    cmbCategory.SelectedValue = dr["CategoryID"];

                    cmbDenomination.SelectedValue = dr["DenominationID"];

                    txtPiecesPerSheet.Text = dr["PiecesPerSheet"].ToString();

                    txtRemarks.Text = dr["Remarks"].ToString();

                    btnSave.Enabled = false;

                    btnUpdate.Enabled = true;
                }
            }

            if (dgvConversion.Columns[e.ColumnIndex].Name == "Delete")
            {
                int id = Convert.ToInt32(
                    dgvConversion.Rows[e.RowIndex].Cells["ConversionID"].Value);

                DialogResult result = MessageBox.Show(
                    "Do you want to delete this record?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (dal.Delete(id))
                    {
                        MessageBox.Show("Record deleted successfully.");

                        LoadGrid();

                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Record could not be deleted.");
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (!ValidateData())
                return;

            UnitConversionModel model = new UnitConversionModel();

            model.ConversionID = EditID;
            model.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
            model.DenominationID = Convert.ToInt32(cmbDenomination.SelectedValue);
            model.PiecesPerSheet = Convert.ToInt32(txtPiecesPerSheet.Text);
            model.Remarks = txtRemarks.Text.Trim();
            
            if (dal.IsDuplicate(EditID,
                    model.CategoryID,
                    model.DenominationID))
            {
                MessageBox.Show(
                    "This denomination already exists in this category.",
                    "Duplicate Record",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
            if (dal.Update(model))
            {
                MessageBox.Show("Record updated successfully.");

                LoadGrid();

                ClearForm();
            }
            else
            {
                MessageBox.Show("Record could not be updated.");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
