using SupplyBranch.DAL;
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

namespace SupplyBranch.Forms.Stock
{
    public partial class frmStockIn : Form
    {
        private StockDAL stockDAL = new StockDAL();
        private UnitConversionDAL unitConversionDAL = new UnitConversionDAL();

        private void LoadCategories()
        {
            cmbCategory.DataSource =
                unitConversionDAL.GetCategories();

            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryID";

            cmbCategory.SelectedIndex = -1;
        }

        private void LoadDenominations(int categoryID)
        {
            cmbDenomination.DataSource =
                unitConversionDAL.GetStockDenominations(categoryID);

            cmbDenomination.DisplayMember = "Denomination";
            cmbDenomination.ValueMember = "DenominationID";

            cmbDenomination.SelectedIndex = -1;
        }
        private void cmbCategory_SelectedIndexChanged(
      object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue == null)
                return;

            if (cmbCategory.SelectedValue is DataRowView)
                return;

            int categoryID =
                Convert.ToInt32(cmbCategory.SelectedValue);

            LoadDenominations(categoryID);
        }

        private void txtQuantity_KeyPress(
            object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ClearForm()
        {
            cmbCategory.SelectedIndex = -1;
            cmbDenomination.DataSource = null;

            txtBoxQty.Clear();
            txtPacketQty.Clear();
            txtSheetQty.Clear();
            txtStampQty.Clear();
            txtRemarks.Clear();

            cmbCategory.Focus();
        }

        private void LoadStockInGrid()
        {
            try
            {
                dgvStockIn.AutoGenerateColumns = false;

                dgvStockIn.DataSource =
                    stockDAL.GetStockInHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load stock history.\n\n" + ex.Message,
                    "Stock In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private bool ValidateStockIn()
        {
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select category.",
                    "Stock In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategory.Focus();
                return false;
            }

            if (cmbDenomination.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select denomination.",
                    "Stock In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbDenomination.Focus();
                return false;
            }

            int boxQty = GetQuantity(txtBoxQty);
            int packetQty = GetQuantity(txtPacketQty);
            int sheetQty = GetQuantity(txtSheetQty);
            int stampQty = GetQuantity(txtStampQty);

            if (boxQty == 0 &&
                packetQty == 0 &&
                sheetQty == 0 &&
                stampQty == 0)
            {
                MessageBox.Show(
                    "Please enter at least one stock quantity.",
                    "Stock In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtBoxQty.Focus();
                return false;
            }

            return true;
        }
        private int GetQuantity(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return 0;

            if (!int.TryParse(textBox.Text, out int quantity))
                return 0;

            if (quantity < 0)
                return 0;

            return quantity;
        }

        public frmStockIn()
        {
            InitializeComponent();
        }

        private void frmStockIn_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            LoadCategories();

            cmbDenomination.DataSource = null;
            LoadStockInGrid();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateStockIn())
                    return;

                int categoryID =
                    Convert.ToInt32(cmbCategory.SelectedValue);

                int denominationID =
                    Convert.ToInt32(cmbDenomination.SelectedValue);

                int boxQty = GetQuantity(txtBoxQty);
                int packetQty = GetQuantity(txtPacketQty);
                int sheetQty = GetQuantity(txtSheetQty);
                int stampQty = GetQuantity(txtStampQty);

                string remarks = txtRemarks.Text.Trim();

                if (stockDAL.StockIn(
                    categoryID,
                    denominationID,
                    boxQty,
                    packetQty,
                    sheetQty,
                    stampQty,
                    remarks))
                {
                    MessageBox.Show(
                        "Stock received successfully.",
                        "Stock In",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearForm();
                    LoadStockInGrid();
                }
                else
                {
                    MessageBox.Show(
                        "Stock could not be saved.",
                        "Stock In",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to save stock.\n\n" + ex.Message,
                    "Stock In",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
