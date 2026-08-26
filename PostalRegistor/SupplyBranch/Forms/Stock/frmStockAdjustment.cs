using SupplyBranch.DAL;
using SupplyBranch.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StampStoreApp
{
    public partial class frmStockAdjustment : Form
    {
        private StockDAL stockDAL = new StockDAL();

       

        public frmStockAdjustment()
        {
            InitializeComponent();
        }

        private void frmStockAdjustment_Load(object sender, EventArgs e)
        {
           UITheme.Apply(this);
            LoadCategories();
            LoadAdjustmentTypes();
            ClearFields();
            LoadAdjustmentHistory();
        }

        // 1. Dropdowns Populate Karein
        private void LoadCategories()
        {
            DataTable dt = stockDAL.GetCategories(); // Aapki DAL ka Category method
            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryID";
            cmbCategory.SelectedIndex = -1;
        }

       

        private void LoadAdjustmentTypes()
        {
            cmbAdjustType.Items.Clear();
            cmbAdjustType.Items.Add("ADD");
            cmbAdjustType.Items.Add("LESS");
            cmbAdjustType.SelectedIndex = 0;
        }

        // 2. Save Button Click Event
      

        // 3. Reset Fields
        private void ClearFields()
        {
            cmbCategory.SelectedIndex = -1;
            cmbDenomination.DataSource = null;
            cmbAdjustType.SelectedIndex = 0;

            txtBoxQty.Text = "0";
            txtPacketQty.Text = "0";
            txtSheetQty.Text = "0";
            txtStampQty.Text = "0";
            txtReason.Text = "";
        }

       

       

        // 4. History Grid Population
        private void LoadAdjustmentHistory()
        {
            DataTable dt = stockDAL.GetAdjustmentHistory();
            dgvHistory.DataSource = dt;
        }

         private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue != null && int.TryParse(cmbCategory.SelectedValue.ToString(), out int categoryID))
            {
                DataTable dt = stockDAL.GetDenominationsByCategory(categoryID); // Denomination method
                cmbDenomination.DataSource = dt;
                cmbDenomination.DisplayMember = "Denomination";
                cmbDenomination.ValueMember = "DenominationID";
                cmbDenomination.SelectedIndex = -1;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
        
            ClearFields();
        }

       

        private void btnSave_Click(object sender, EventArgs e)
        {
        
            // Validation 1: Dropdowns Check
            if (cmbCategory.SelectedValue == null || cmbDenomination.SelectedValue == null)
            {
                MessageBox.Show("Baraye meharbani Category aur Denomination select karein.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int categoryID = Convert.ToInt32(cmbCategory.SelectedValue);
            int denominationID = Convert.ToInt32(cmbDenomination.SelectedValue);
            string adjustType = cmbAdjustType.SelectedItem.ToString();

            // Validation 2: Quantities Read
            int.TryParse(txtBoxQty.Text.Trim(), out int boxQty);
            int.TryParse(txtPacketQty.Text.Trim(), out int packetQty);
            int.TryParse(txtSheetQty.Text.Trim(), out int sheetQty);
            int.TryParse(txtStampQty.Text.Trim(), out int stampQty);

            if (boxQty == 0 && packetQty == 0 && sheetQty == 0 && stampQty == 0)
            {
                MessageBox.Show("Kam se kam ek quantity (Box/Packet/Sheet/Stamp) darj karein.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation 3: Reason Mandatory Check
            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Adjustment ki wajah (Reason) likhna zaroori hai.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReason.Focus();
                return;
            }

            // Confirmation Box
            DialogResult dr = MessageBox.Show(
                $"Kya aap stock ko {adjustType} karna chahte hain?",
                "Confirm Adjustment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            // Save to Database
            try
            {
                bool success = stockDAL.AdjustStock(categoryID, denominationID, adjustType, boxQty, packetQty, sheetQty, stampQty, txtReason.Text.Trim());

                if (success)
                {
                    MessageBox.Show("Stock Adjustment kamyabi se save ho gayi hai.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadAdjustmentHistory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void txtBoxQty_KeyPress(object sender, KeyPressEventArgs e)
            {
        
                if (!char.IsControl(e.KeyChar) &&
                    !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

        private void txtPacketQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
                { e.Handled = true; }
        }

        private void txtSheetQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
                { e.Handled = true; }
        }

       

        private void txtStampQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
            { e.Handled = true; }
        }
    }
}