using SupplyBranch.DAL;
using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
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


namespace SupplyBranch.Forms.Transactions
{
    

public partial class frmSupply : Form
    {
        private DataGridView dgvStockEntry;
        private DataGridViewTextBoxColumn StockCategoryID;
        private DataGridViewTextBoxColumn StockDenominationID;
        private DataGridViewTextBoxColumn StockCategory;
        private DataGridViewTextBoxColumn StockDenomination;
        private DataGridViewTextBoxColumn StockCaseQty;
        private DataGridViewTextBoxColumn StockPacketQty;
        private DataGridViewTextBoxColumn StockSheetQty;
        private DataGridViewTextBoxColumn StockStampQty;

        private bool _quantityWarningShown = false;
        int indentStatus = 0;
        private Dictionary<string, string> _originalDraftValues =
    new Dictionary<string, string>();
        private bool _isDataChanged = false;

        //private bool _draftChanged = false;

        private bool _isLoadingDraft = false;

        private bool _isDraft = false;

        private SupplyNumberInfo _supplyInfo;

        private int _indentID;

        private int _supplyID = 0;

        private int _currentStatusID = 1;

        private bool _isEditDraft = false;

        private bool _isEditDraftWarningShown = false;

        private bool _draftChanged = false;

        private readonly SupplyDAL supplyDAL = new SupplyDAL();


        private StockDAL stockDAL = new StockDAL();

        private void LoadSavedStockTransactions(int supplyID)
        {
            // 1. Grid Control Null Check
            if (dgvStockEntry == null)
                return;
            // 2. SupplyID Check
            if (supplyID <= 0)
            {
                dgvStockEntry.DataSource = null;
                return;
            }

            try
            {
                StockDAL stockDAL = new StockDAL();
                DataTable dt = stockDAL.GetStockTransactionsBySupplyID(supplyID);

                // 3. Pehle DataSource null karein (Unbound to Bound error se bachne ke liye)
                dgvStockEntry.DataSource = null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    dgvStockEntry.AutoGenerateColumns = false;
                    dgvStockEntry.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Data load karte waqt error aya: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStockEntryGrid()
        {
            // 1. Pehle DataSource null karein
            dgvStockEntry.DataSource = null;

            // 2. Schema / Columns wala DataTable banayein
            DataTable dt = new DataTable();
            dt.Columns.Add("CategoryID", typeof(int));
            dt.Columns.Add("DenominationID", typeof(int));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("BoxQty", typeof(int));
            dt.Columns.Add("PacketQty", typeof(int));
            dt.Columns.Add("SheetQty", typeof(int));
            dt.Columns.Add("StampQty", typeof(int));

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int supplySheets = 0, supplyPieces = 0;
                if (row.Cells["SupplySheets"].Value != null) int.TryParse(row.Cells["SupplySheets"].Value.ToString(), out supplySheets);
                if (row.Cells["SupplyPieces"].Value != null) int.TryParse(row.Cells["SupplyPieces"].Value.ToString(), out supplyPieces);

                if (supplySheets <= 0 && supplyPieces <= 0) continue;

                int categoryID = 0, denominationID = 0;
                if (row.Cells["CategoryID"].Value != null) int.TryParse(row.Cells["CategoryID"].Value.ToString(), out categoryID);
                if (row.Cells["DenominationID"].Value != null) int.TryParse(row.Cells["DenominationID"].Value.ToString(), out denominationID);

                if (categoryID == 0 || denominationID == 0) continue;

                string category = row.Cells["Category"].Value?.ToString() ?? "";
                string denomination = row.Cells["Denomination"].Value?.ToString() ?? "";

                // Row add karein DataTable me
                dt.Rows.Add(categoryID, denominationID, category, denomination, 0, 0, 0, 0);
            }

            // 3. Grid me Bind karein
            dgvStockEntry.AutoGenerateColumns = false;
            dgvStockEntry.DataSource = dt;
        }

        // 1. Event ko Form Load ya Constructor me attach karein
        private void InitializeGridValidation()
        {
            this.dgvStockEntry.EditingControlShowing += dgvStockEntry_EditingControlShowing;
        }

        // 2. Editing Control Event: Jab user cell me type karna shuru karta hai
        private void dgvStockEntry_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Check karein ke cell TextBox type hai ya nahi
            if (e.Control is TextBox txt)
            {
                // Pehle purana KeyPress event remove karein taake duplicate attach na ho
                txt.KeyPress -= QuantityColumn_KeyPress;

                // Current column check karein
                string columnName = dgvStockEntry.Columns[dgvStockEntry.CurrentCell.ColumnIndex].Name;

                // Agar Column Quantity wala hai to KeyPress restrict karein
                if (columnName == "StockCaseQty" || columnName == "StockPacketQty" ||
                    columnName == "StockSheetQty" || columnName == "StockStampQty")
                {
                    txt.KeyPress += QuantityColumn_KeyPress;
                }
            }
        }

        // 3. KeyPress Event: Non-Numeric Keys ko Block Karne Ke Liye
        private void QuantityColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow sirf digits (0-9) aur Control keys (jaise Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Key input ko block kar do
            }
        }

        private void SaveStockTransactions()
        {
            if (_supplyID == 0)
            return;
            

            foreach (DataGridViewRow row in dgvStockEntry.Rows)
            {
                if (row.IsNewRow)
                    continue;

                // -----------------------------------------
                // Get CategoryID & DenominationID
                // -----------------------------------------
                int categoryID = 0;
                int denominationID = 0;

                int.TryParse(Convert.ToString(row.Cells["StockCategoryID"].Value), out categoryID);
                int.TryParse(Convert.ToString(row.Cells["StockDenominationID"].Value), out denominationID);


                if (categoryID == 0 || denominationID == 0)
                {
                    MessageBox.Show("Please select a valid category and denomination.",
                        "Stock Transaction",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    continue;
                }

                // -----------------------------------------
                // Get StockID from StockMaster
                // -----------------------------------------
                int stockID = stockDAL.GetStockID(categoryID, denominationID);

                if (stockID == 0)
                {
                    MessageBox.Show(
                        "No stock record was found for the selected category/item in StockMaster",
                        "Stock Transaction",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    continue;
                }

                // -----------------------------------------
                // Get quantities from Stock Entry Grid
                // -----------------------------------------
                int boxQty = 0, packetQty = 0, sheetQty = 0, stampQty = 0;

                int.TryParse(Convert.ToString(row.Cells["StockCaseQty"].Value), out boxQty);
                int.TryParse(Convert.ToString(row.Cells["StockPacketQty"].Value), out packetQty);
                int.TryParse(Convert.ToString(row.Cells["StockSheetQty"].Value), out sheetQty);
                int.TryParse(Convert.ToString(row.Cells["StockStampQty"].Value), out stampQty);

                // -----------------------------------------
                // Skip if all quantities are zero
                // -----------------------------------------
                if (boxQty == 0 && packetQty == 0 && sheetQty == 0 && stampQty == 0)
                {
                    continue;
                }
                MessageBox.Show($"Saving Stock Transaction for StockID: {stockID}, BoxQty: {boxQty}, PacketQty: {packetQty}, SheetQty: {sheetQty}, StampQty: {stampQty}");
                // -----------------------------------------
                // Insert ya Update Check
                // -----------------------------------------
                if (_isEditDraft)
                {
                    // Agar edit mode hai to UPDATE chalayein
                    stockDAL.UpdateStockTransactionOut(
                        stockID,
                        boxQty,
                        packetQty,
                        sheetQty,
                        stampQty,
                        _supplyID,
                        txtRemarks.Text.Trim());
                }
                else
                {
                    // Naye draft ke waqt INSERT chalayein
                    stockDAL.InsertStockTransactionOut(
                        stockID,
                        boxQty,
                        packetQty,
                        sheetQty,
                        stampQty,
                        _supplyID,
                        txtRemarks.Text.Trim());
                }
            }
        }

        private void SetSupplyTypeByCategory()
        {
            try
            {
                bool category2Found = false;

                foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (row.Cells["CategoryID"].Value != null &&
                        row.Cells["CategoryID"].Value != DBNull.Value)
                    {
                        int categoryID =
                            Convert.ToInt32(row.Cells["CategoryID"].Value);

                        if (categoryID == 2)
                        {
                            category2Found = true;
                            break;
                        }
                    }
                }
                //cmbSupplyType.Enabled = true;

                if (category2Found)
                    cmbSupplyType.SelectedIndex = 4;
                    
                else
                    cmbSupplyType.SelectedIndex = 3;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to set Supply Type.\n\n" + ex.Message,
                    "Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void MarkDataChanged()
        {
            // صرف Approved Record پر Apply ہوگا
            if (_currentStatusID != 2)
                return;

            _isDataChanged = true;

            btnIssue.Enabled = false;
            btnIssue.Text = "Save Changes First";
        }
        private void MarkDataSaved()
        {
            _isDataChanged = false;

            if (_currentStatusID == 2)
            {
                btnIssue.Enabled = true;
                btnIssue.Text = "Issue";
            }
        }

      
        private void SaveDraft()
        {
            int packingQty = 0;
            int indentStatusID = 6; // Default = Partial
            

            int.TryParse(txtPackingQty.Text, out packingQty);

            //--------------------------------------------------
            // Calculate Total Supply in current grid
            //--------------------------------------------------

            int currentSupplyTotal = 0;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int totalPieces = 0;

                int.TryParse(
                    Convert.ToString(row.Cells["SupplyTotalPieces"].Value),
                    out totalPieces);

                currentSupplyTotal += totalPieces;
            }

            //--------------------------------------------------
            // Calculate Remaining
            //--------------------------------------------------

            int indentTotal = 0;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int indentPieces = 0;

                int.TryParse(
                    Convert.ToString(row.Cells["IndentTotalPieces"].Value),
                    out indentPieces);

                indentTotal += indentPieces;
            }

            int remaining = indentTotal - currentSupplyTotal;

            if (remaining <= 0)
            {
                remaining = 0;
                indentStatusID = 7; // Closed
               
            }
            else
            {
                indentStatusID = 6; // Partial
               
            }

            //--------------------------------------------------
            // Save Supply Master
            //--------------------------------------------------

            _supplyID = supplyDAL.SaveDraftSupply(
                _supplyInfo,
                _indentID,
                Convert.ToInt32(cmbSupplyType.SelectedValue),
                cmbDispatchMode.Text,
                cmbPackingType.Text,
                packingQty,
                txtRemarks.Text,
                indentStatusID,
                dtSupplyDate.Value);

            //--------------------------------------------------
            // Save Supply Details
            //--------------------------------------------------

            SaveSupplyDetails();

            // Save Stock Transaction OUT
            SaveStockTransactions();
        }

        private void UpdateDraft()
        {
            // Header Update
            supplyDAL.UpdateSupplyMaster(
                _supplyID,
                Convert.ToInt32(cmbSupplyType.SelectedValue),
                dtSupplyDate.Value,
                cmbDispatchMode.Text,
                cmbPackingType.Text,        
                Convert.ToInt32(txtPackingQty.Text),
                txtInvoiceNo?.Text ?? string.Empty,
                txtRemarks?.Text ?? string.Empty);
            // Detail Update
            SaveSupplyDetails();
            SaveStockTransactions();

            MessageBox.Show(
                "Draft Updated Successfully.",
                "Supply",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private bool ValidateSupply(bool checkCase = false)
        {
            //=========================================
            // Supply Type
            //=========================================

            if (cmbSupplyType.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select Supply Type.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbSupplyType.Focus();
                return false;
            }


            //=========================================
            // Dispatch Mode
            //=========================================

            if (string.IsNullOrWhiteSpace(cmbDispatchMode.Text))
            {
                MessageBox.Show(
                    "Please select Dispatch Mode.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbDispatchMode.Focus();
                return false;
            }


            //=========================================
            // Packing Type
            //=========================================

            if (string.IsNullOrWhiteSpace(cmbPackingType.Text))
            {
                MessageBox.Show(
                    "Please select Packing Type.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbPackingType.Focus();
                return false;
            }


            //=========================================
            // At least one Supply Item
            //=========================================

            bool hasSupply = false;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int totalSupply = 0;

                int.TryParse(
                    Convert.ToString(
                        row.Cells["SupplyTotalPieces"].Value),
                    out totalSupply);

                if (totalSupply <= 0)
                    continue;

                hasSupply = true;


                //=====================================
                // CASE VALIDATION
                // صرف Approve کے وقت
                //=====================================

                if (checkCase &&
                    cmbPackingType.Text.Trim()
                        .Equals("Case", StringComparison.OrdinalIgnoreCase))
                {
                    string caseNoFrom =
                        Convert.ToString(
                            row.Cells["CaseNoFrom"].Value)?.Trim();

                    string caseNoTo =
                        Convert.ToString(
                            row.Cells["CaseNoTo"].Value)?.Trim();

                    string caseCode =
                        Convert.ToString(
                            row.Cells["CaseCode"].Value)?.Trim();


                    if (string.IsNullOrWhiteSpace(caseNoFrom))
                    {
                        MessageBox.Show(
                            "Please enter Case No. From.",
                            "Validation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        dgvSupplyDetail.CurrentCell =
                            row.Cells["CaseNoFrom"];

                        dgvSupplyDetail.BeginEdit(true);

                        return false;
                    }


                    if (string.IsNullOrWhiteSpace(caseNoTo))
                    {
                        MessageBox.Show(
                            "Please enter Case No. To.",
                            "Validation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        dgvSupplyDetail.CurrentCell =
                            row.Cells["CaseNoTo"];

                        dgvSupplyDetail.BeginEdit(true);

                        return false;
                    }


                    if (string.IsNullOrWhiteSpace(caseCode))
                    {
                        MessageBox.Show(
                            "Please enter Case Code.",
                            "Validation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        dgvSupplyDetail.CurrentCell =
                            row.Cells["CaseCode"];

                        dgvSupplyDetail.BeginEdit(true);

                        return false;
                    }
                }
            }


            //=========================================
            // Supply Quantity Required
            //=========================================

            if (!hasSupply)
            {
                MessageBox.Show(
                    "Please enter Supply Quantity for at least one item.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void LoadDraft()
        {
            DataTable dt = supplyDAL.GetSupplyHeader(_supplyID);

            if (dt.Rows.Count == 0)
                return;

            DataRow row = dt.Rows[0];

            txtSupplyNo.Text = row["SupplyNo"].ToString();

            txtFinancialYear.Text = row["FinancialYear"].ToString();

            txtIndentNo.Text = row["IndentNo"].ToString();

            txtIndentDate.Text =
                Convert.ToDateTime(row["IndentDate"])
                .ToString("dd-MM-yyyy");

            txtOfficeName.Text = row["OfficeName"].ToString();

            txtIndentStatus.Text = row["StatusName"].ToString();

            cmbSupplyType.SelectedValue =
                Convert.ToInt32(row["SupplyType"]);

            cmbDispatchMode.Text =
                row["DispatchMode"].ToString();

            cmbPackingType.Text =
                row["PackingType"].ToString();

            // Packing Qty اگر Form پر موجود ہے
            if (txtPackingQty != null)
                txtPackingQty.Text = row["PackingQty"].ToString();

            // Remarks اگر Form پر موجود ہے
            if (txtRemarks != null)
                txtRemarks.Text = row["Remarks"].ToString();

            LoadDraftItems();
        }


        private void LoadDraftHeader(int supplyID)
        {


            DataTable dt = supplyDAL.GetDraftHeader(supplyID);

            if (dt.Rows.Count == 0)
                return;

            DataRow row = dt.Rows[0];

            // Draft سے متعلق Indent معلوم کرو
            _indentID = Convert.ToInt32(row["IndentID"]);

            // پہلے Indent Header لوڈ کرو
            LoadIndentHeader();

            // پھر Supply Header
            txtSupplyNo.Text = row["SupplyNo"].ToString();
            
            txtInvoiceNo.Text = row["InvoiceNo"].ToString();

            txtFinancialYear.Text = row["FinancialYear"].ToString();
            dtSupplyDate.Value = Convert.ToDateTime(row["SupplyDate"]);

            cmbSupplyType.SelectedValue =
                Convert.ToInt32(row["SupplyType"]);

            cmbDispatchMode.Text =
                row["DispatchMode"].ToString();

            cmbPackingType.Text =
                row["PackingType"].ToString();
            txtSupplyStatus.Text = row["SupplyStatus"].ToString();
            txtPackingQty.Text =
                row["PackingQty"].ToString();

            txtRemarks.Text =
                row["Remarks"].ToString();
            _currentStatusID = Convert.ToInt32(row["StatusID"]);

           

            switch (_currentStatusID)
            {
                case 1:
                    txtSupplyStatus.Text = "Draft";
                    break;

                case 2:
                    txtSupplyStatus.Text = "Approved";
                    break;

                case 3:
                    txtSupplyStatus.Text = "Issued";
                    break;

                case 4:
                    txtSupplyStatus.Text = "Cancelled";
                    break;
            }
        }


        private void SaveSupplyDetails()
        {
            if (_supplyID == 0)
                return;

            // پہلے پرانی Detail Delete
            supplyDAL.DeleteSupplyDetails(_supplyID);

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int supplySheets = 0;
                int supplyPieces = 0;

                int.TryParse(Convert.ToString(row.Cells["SupplySheets"].Value), out supplySheets);
                int.TryParse(Convert.ToString(row.Cells["SupplyPieces"].Value), out supplyPieces);

                // اگر دونوں صفر ہیں تو Save نہ کرو
                if (supplySheets == 0 && supplyPieces == 0)
                    continue;

                supplyDAL.InsertSupplyDetail(_supplyID, row);
            }
        }
        private void SaveOriginalDraftValues()
        {
            _originalDraftValues.Clear();

            // =========================================
            // 1. Header Values
            // =========================================
            _originalDraftValues["SupplyDate"] = dtSupplyDate.Value.ToString("yyyy-MM-dd");
            _originalDraftValues["SupplyType"] = Convert.ToString(cmbSupplyType.SelectedValue);
            _originalDraftValues["DispatchMode"] = cmbDispatchMode.Text;
            _originalDraftValues["PackingType"] = cmbPackingType.Text;
            _originalDraftValues["PackingQty"] = txtPackingQty.Text;
            _originalDraftValues["Remarks"] = txtRemarks.Text;


            // =========================================
            // 2. Stock Grid Values (ADDED FIX)
            // =========================================


            foreach (DataGridViewRow row in dgvStockEntry.Rows)
            {
                if (row.IsNewRow)
                    continue;

                // 1. Quantities safe parsing
                int caseQty = 0, packetQty = 0, sheetQty = 0, stampQty = 0;
                int.TryParse(Convert.ToString(row.Cells["StockCaseQty"].Value), out caseQty);
                int.TryParse(Convert.ToString(row.Cells["StockPacketQty"].Value), out packetQty);
                int.TryParse(Convert.ToString(row.Cells["StockSheetQty"].Value), out sheetQty);
                int.TryParse(Convert.ToString(row.Cells["StockStampQty"].Value), out stampQty);

                string value = $"{caseQty}|{packetQty}|{sheetQty}|{stampQty}";

                // 2. IDs extraction
                string stockCategoryID = Convert.ToString(row.Cells["StockCategoryID"].Value);
                string stockDenominationID = Convert.ToString(row.Cells["StockDenominationID"].Value);

                // 3. Fallback Key creation (agar ID na mile to Row Index use karein)
                string key;
                if (!string.IsNullOrEmpty(stockCategoryID) && !string.IsNullOrEmpty(stockDenominationID))
                {
                    key = "StockGrid_" + stockCategoryID + "_" + stockDenominationID;
                }
                else
                {
                    key = "StockGrid_Row_" + row.Index;
                }

                // 4. Dictionary me save karein
                _originalDraftValues[key] = value;
            }


            // =========================================
            // 3. Supply Detail Grid Values
            // =========================================
            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string detailID = Convert.ToString(row.Cells["DetailID"].Value);

                if (string.IsNullOrEmpty(detailID))
                    continue;

                string value =
                    Convert.ToString(row.Cells["SupplySheets"].Value) + "|" +
                    Convert.ToString(row.Cells["SupplyPieces"].Value) + "|" +
                    Convert.ToString(row.Cells["LedgerFolio"].Value) + "|" +
                    Convert.ToString(row.Cells["CaseCode"].Value) + "|" +
                    Convert.ToString(row.Cells["CaseNoFrom"].Value) + "|" +
                    Convert.ToString(row.Cells["CaseNoTo"].Value);

                _originalDraftValues["Grid_" + detailID] = value;
            }


            // =========================================
            // 4. Initial State Reset
            // =========================================
            _draftChanged = false;

            btnApprove.Enabled = true;
            btnIssue.Enabled = true;
        }

        private void CheckDraftChanges()
        {
            if (!_isEditDraft)
            {
                // Agar ye message aata hai to iska matlab _isEditDraft false hai!
                return;
            }

            if (_isLoadingDraft)
            {
                return;
            }

            bool changed = false;


            // =========================================
            // Header
            // =========================================

            // Supply Date
            string currentSupplyDate =
                dtSupplyDate.Value.ToString("yyyy-MM-dd");

            if (currentSupplyDate !=
                GetOriginalValue("SupplyDate"))
            {
                changed = true;
            }


            // Supply Type
            if (Convert.ToString(cmbSupplyType.SelectedValue) !=
                GetOriginalValue("SupplyType"))
            {
                changed = true;
            }


            // Dispatch Mode
            if (cmbDispatchMode.Text !=
                GetOriginalValue("DispatchMode"))
            {
                changed = true;
            }


            // Packing Type
            if (cmbPackingType.Text !=
                GetOriginalValue("PackingType"))
            {
                changed = true;
            }


            // Packing Qty
            if (txtPackingQty.Text !=
                GetOriginalValue("PackingQty"))
            {
                changed = true;
            }


            // Remarks
            if (txtRemarks.Text !=
                GetOriginalValue("Remarks"))
            {
                changed = true;
            }


            // =========================================
            // Grid
            // =========================================

            // =========================================

            // =========================================
            // Stock Grid Comparison (Fixed)
            // =========================================
            // =========================================
            // Stock Grid Comparison (Debug-Ready)
            // =========================================
            int rowCount = dgvStockEntry.Rows.Count;

            foreach (DataGridViewRow row1 in dgvStockEntry.Rows)
            {
                if (row1.IsNewRow)
                    continue;

                // Direct string conversion without skipping early
                string stockCategoryID = Convert.ToString(row1.Cells["StockCategoryID"].Value);
                string stockDenominationID = Convert.ToString(row1.Cells["StockDenominationID"].Value);

                int caseQty = 0, packetQty = 0, sheetQty = 0, stampQty = 0;

                int.TryParse(Convert.ToString(row1.Cells["StockCaseQty"].Value), out caseQty);
                int.TryParse(Convert.ToString(row1.Cells["StockPacketQty"].Value), out packetQty);
                int.TryParse(Convert.ToString(row1.Cells["StockSheetQty"].Value), out sheetQty);
                int.TryParse(Convert.ToString(row1.Cells["StockStampQty"].Value), out stampQty);

                string currentValue = $"{caseQty}|{packetQty}|{sheetQty}|{stampQty}";

                string key = "StockGrid_" + stockCategoryID + "_" + stockDenominationID;
                string originalValue = GetOriginalValue(key);

                // TEST MESSAGEBOX (Aap ko screen par farq dikhayega)

                if (currentValue != originalValue)
                {
                    changed = true;
                    break;
                }
            }

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string detailID =
                    Convert.ToString(row.Cells["DetailID"].Value);

                if (string.IsNullOrEmpty(detailID))
                    continue;


                string currentValue =
                    Convert.ToString(row.Cells["SupplySheets"].Value) + "|" +
                    Convert.ToString(row.Cells["SupplyPieces"].Value) + "|" +
                    Convert.ToString(row.Cells["LedgerFolio"].Value) + "|" +
                    Convert.ToString(row.Cells["CaseCode"].Value) + "|" +
                    Convert.ToString(row.Cells["CaseNoFrom"].Value) + "|" +
                    Convert.ToString(row.Cells["CaseNoTo"].Value);


                string originalValue =
                    GetOriginalValue("Grid_" + detailID);


                if (currentValue != originalValue)
                {
                    changed = true;
                    break;
                }
            }


            // =========================================
            // Final State
            // =========================================

            _draftChanged = changed;
            _isDataChanged = changed;


            // Change موجود ہو تو
            // Approve / Issue بند
            btnApprove.Enabled = !changed;
            btnIssue.Enabled = !changed;


            // Save Changes صرف change کی صورت میں
            btnSaveDraft.Visible = changed;

            if (changed)
            {
                btnSaveDraft.Enabled = true;
                btnSaveDraft.Text = "Save Changes";
                btnIssue.Text = "Save Changes First";
            }
            else
            {
                btnSaveDraft.Enabled = false;
                btnSaveDraft.Text = "Update Draft";
                btnIssue.Text = "Issue";
            }
        }

        private string GetOriginalValue(string key)
        {
            if (_originalDraftValues.ContainsKey(key))
                return _originalDraftValues[key];

            return "";
        }
        private void LoadDraftItems()
        {
            dgvSupplyDetail.AutoGenerateColumns = false;

            DataTable dt = supplyDAL.GetSupplyDetails(_supplyID);

            if (dt.Rows.Count > 0)
            {
                // Draft Detail موجود ہیں
                dgvSupplyDetail.DataSource = dt;

            }
            else
            {
                // نئی Draft ہے، ابھی Detail Save نہیں ہوئی
                LoadIndentItems();
            }
        }

        private void LoadIndentItems()
        {
            dgvSupplyDetail.AutoGenerateColumns = false;

            DataTable dt = supplyDAL.GetIndentItems(_indentID);

            dgvSupplyDetail.DataSource = dt;
            //LoadStockEntryGrid();

            // پہلے سے Approved / Issued supplies کا total
            int totalSupplyPieces =
                supplyDAL.GetTotalSupplyPieces(_indentID);

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int indentTotalPieces =
                    ParseInt(row.Cells["IndentTotalPieces"].Value);

                int remainingTotalPieces =
                    ParseInt(row.Cells["RemainingTotalPieces"].Value);


                //-----------------------------------------
                // Partial Indent
                //-----------------------------------------

                if (indentStatus == 6)
                {
                    MessageBox.Show(
                        $"This Indent has already been partially supplied.\n\n" +
                        $"Total Supplied: {totalSupplyPieces:N0}\n" +
                        $"Remaining Quantity: {remainingTotalPieces:N0}",
                        "Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    indentTotalPieces =
       ParseInt(row.Cells["IndentTotalPieces"].Value);

                    int originalPendingPieces =
                        indentTotalPieces - totalSupplyPieces;

                    if (originalPendingPieces < 0)
                        originalPendingPieces = 0;

                    row.Cells["OriginalPendingPieces"].Value =
                        originalPendingPieces;

                    dgvSupplyDetail.Columns["RemainingTotalPieces"]
                        .HeaderText = "Total Supply";
                }


                //-----------------------------------------
                // Closed Indent
                //-----------------------------------------

                else if (indentStatus == 7)
                {
                    MessageBox.Show(
                        "This Indent is already closed. No further Supply can be made.",
                        "Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }


                //-----------------------------------------
                // First Supply / Open
                //-----------------------------------------

                else
                {
                    // پہلی Supply
                    row.Cells["OriginalPendingPieces"].Value =
                        indentTotalPieces;
                }
            }
        }


        public frmSupply(int supplyID, bool isDraft)
        {
            InitializeComponent();


            _supplyID = supplyID;
            _isDraft = isDraft;

            // 1. Safe Event Unbind & Re-bind
            if (this.dgvSupplyDetail != null)
            {
                this.dgvSupplyDetail.CellEndEdit -= this.dgvSupplyDetail_CellEndEdit;
                this.dgvSupplyDetail.CellEndEdit += this.dgvSupplyDetail_CellEndEdit;
            }

            // 2. DataGridView Check & Instance Creation
            if (this.dgvStockEntry == null)
            {
                this.dgvStockEntry = new System.Windows.Forms.DataGridView();
            }

            ((System.ComponentModel.ISupportInitialize)(this.dgvStockEntry)).BeginInit();

            // 3. Columns Null Checks & Instantations (Line 1057 Crash Fix)
            if (this.StockCategoryID == null) this.StockCategoryID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            if (this.StockDenominationID == null) this.StockDenominationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            if (this.StockCategory == null) this.StockCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            if (this.StockDenomination == null) this.StockDenomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            if (this.StockCaseQty == null) this.StockCaseQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            if (this.StockPacketQty == null) this.StockPacketQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            if (this.StockSheetQty == null) this.StockSheetQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            if (this.StockStampQty == null) this.StockStampQty = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // 4. Grid Properties Setup
            this.dgvStockEntry.AllowUserToAddRows = false;
            this.dgvStockEntry.AllowUserToDeleteRows = false;
            this.dgvStockEntry.AllowUserToResizeRows = false;
            this.dgvStockEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockEntry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStockEntry.BackgroundColor = System.Drawing.Color.White;
            this.dgvStockEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvStockEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockEntry.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvStockEntry.MultiSelect = false;
            this.dgvStockEntry.Name = "dgvStockEntry";
            this.dgvStockEntry.RowHeadersVisible = false;
            this.dgvStockEntry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // 5. Hidden ID Columns
            this.StockCategoryID.DataPropertyName = "CategoryID";
            this.StockCategoryID.Name = "StockCategoryID";
            this.StockCategoryID.Visible = false;

            this.StockDenominationID.DataPropertyName = "DenominationID";
            this.StockDenominationID.Name = "StockDenominationID";
            this.StockDenominationID.Visible = false;

            // 6. Visible Columns & SQL Binding Mapping
            this.StockCategory.DataPropertyName = "Category";
            this.StockCategory.HeaderText = "Category";
            this.StockCategory.Name = "StockCategory";
            this.StockCategory.ReadOnly = true;

            this.StockDenomination.DataPropertyName = "Denomination";
            this.StockDenomination.HeaderText = "Denomination";
            this.StockDenomination.Name = "StockDenomination";
            this.StockDenomination.ReadOnly = true;

            this.StockCaseQty.DataPropertyName = "BoxQty";
            this.StockCaseQty.HeaderText = "Case Qty";
            this.StockCaseQty.Name = "StockCaseQty";

            this.StockPacketQty.DataPropertyName = "PacketQty";
            this.StockPacketQty.HeaderText = "Packet Qty";
            this.StockPacketQty.Name = "StockPacketQty";

            this.StockSheetQty.DataPropertyName = "SheetQty";
            this.StockSheetQty.HeaderText = "Sheet Qty";
            this.StockSheetQty.Name = "StockSheetQty";

            this.StockStampQty.DataPropertyName = "StampQty";
            this.StockStampQty.HeaderText = "Stamp Qty";
            this.StockStampQty.Name = "StockStampQty";

            // 7. Add Columns to DataGridView
            this.dgvStockEntry.Columns.Clear();
            this.dgvStockEntry.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
        this.StockCategoryID,
        this.StockDenominationID,
        this.StockCategory,
        this.StockDenomination,
        this.StockCaseQty,
        this.StockPacketQty,
        this.StockSheetQty,
        this.StockStampQty
            });

            // 8. Attach Grid to Panel
            if (this.pnlStockEntry != null)
            {
                if (!this.pnlStockEntry.Controls.Contains(this.dgvStockEntry))
                {
                    this.pnlStockEntry.Controls.Add(this.dgvStockEntry);
                }
            }

            ((System.ComponentModel.ISupportInitialize)(this.dgvStockEntry)).EndInit();

            InitializeGridValidation();
            

            // Detach pehle karein, phir attach karein
            this.dgvStockEntry.CellValueChanged -= dgvStockEntry_CellValueChanged;
                this.dgvStockEntry.CellValueChanged += dgvStockEntry_CellValueChanged;

                this.dgvStockEntry.CurrentCellDirtyStateChanged -= dgvStockEntry_CurrentCellDirtyStateChanged;
                this.dgvStockEntry.CurrentCellDirtyStateChanged += dgvStockEntry_CurrentCellDirtyStateChanged;

        }
        private void LoadIndentHeader()
        {
            DataTable dt = supplyDAL.GetIndentHeader(_indentID);
            if (dt.Rows.Count == 0)
                return;

             DataRow row = dt.Rows[0];


            txtIndentNo.Text = row["IndentNo"].ToString();

            txtIndentDate.Text =
                Convert.ToDateTime(row["IndentDate"])
                .ToString("dd-MM-yyyy");

            txtOfficeName.Text = row["OfficeName"].ToString();

            txtIndentStatus.Text = row["StatusName"].ToString();
            indentStatus = Convert.ToInt32(row["IndentStatus"]);

           
            // ہمیشہ نئی Supply Number Generate ہوگی
            if (!_isDraft)
            {
                
                SupplyNumberGenerator generator =
                    new SupplyNumberGenerator();

                _supplyInfo = generator.GenerateSupplyNumber(_indentID);

                txtFinancialYear.Text = _supplyInfo.FinancialYear;
                txtSupplyNo.Text = _supplyInfo.SupplyNo;

                _supplyID = 0;
            }
        }

        

        private void LoadSupplyTypes()
        {
            cmbSupplyType.DataSource = supplyDAL.GetSupplyTypes();

            cmbSupplyType.DisplayMember = "SupplyTypeName";

            cmbSupplyType.ValueMember = "SupplyTypeID";

            cmbSupplyType.SelectedIndex = -1;
        }
        public frmSupply(int indentID)
        {


            InitializeComponent();

            _indentID = indentID;


            this.dgvSupplyDetail.CellClick +=
    new System.Windows.Forms.DataGridViewCellEventHandler(
        this.dgvSupplyDetail_CellClick);

            this.dgvSupplyDetail.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSupplyDetail_CellEndEdit);

            this.dgvStockEntry = new System.Windows.Forms.DataGridView();

            this.StockCategoryID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockDenominationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockDenomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockCaseQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockPacketQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockSheetQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockStampQty = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvStockEntry)).BeginInit();

            this.dgvSupplyDetail.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSupplyDetail_CellEndEdit);
            dgvSupplyDetail.CellEndEdit += dgvSupplyDetail_CellEndEdit;
            this.dgvStockEntry.AllowUserToAddRows = false;
            this.dgvStockEntry.AllowUserToDeleteRows = false;
            this.dgvStockEntry.AllowUserToResizeRows = false;
            // Control ko Parent container ke poore area me fill karne ke liye:
            this.dgvStockEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockEntry.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.dgvStockEntry.BackgroundColor =
                System.Drawing.Color.White;

            this.dgvStockEntry.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.dgvStockEntry.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dgvStockEntry.EditMode =
                System.Windows.Forms.DataGridViewEditMode.EditOnEnter;

            this.dgvStockEntry.MultiSelect = false;

            this.dgvStockEntry.Name = "dgvStockEntry";

            this.dgvStockEntry.RowHeadersVisible = false;

            this.dgvStockEntry.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.dgvStockEntry.Size =
                new System.Drawing.Size(1430, 150);

            this.dgvStockEntry.Location =
                new System.Drawing.Point(10, 35);

            this.dgvStockEntry.TabIndex = 41;

            // hidden ID

            this.StockCategoryID.DataPropertyName = "CategoryID";
            this.StockCategoryID.Name = "StockCategoryID";
            this.StockCategoryID.Visible = false;

            this.StockDenominationID.DataPropertyName = "DenominationID";
            this.StockDenominationID.Name = "StockDenominationID";
            this.StockDenominationID.Visible = false;
            //visible  columns

            this.StockCategory.DataPropertyName = "Category";
            this.StockCategory.HeaderText = "Category";
            this.StockCategory.Name = "StockCategory";
            this.StockCategory.ReadOnly = true;

            this.StockDenomination.DataPropertyName = "Denomination";
            this.StockDenomination.HeaderText = "Denomination";
            this.StockDenomination.Name = "StockDenomination";
            this.StockDenomination.ReadOnly = true;

            this.StockCaseQty.HeaderText = "Case Qty";
            this.StockCaseQty.Name = "StockCaseQty";

            this.StockPacketQty.HeaderText = "Packet Qty";
            this.StockPacketQty.Name = "StockPacketQty";

            this.StockSheetQty.HeaderText = "Sheet Qty";
            this.StockSheetQty.Name = "StockSheetQty";

            this.StockStampQty.HeaderText = "Stamp Qty";
            this.StockStampQty.Name = "StockStampQty";

            this.dgvStockEntry.Columns.AddRange(
    new System.Windows.Forms.DataGridViewColumn[]
    {
        this.StockCategoryID,
        this.StockDenominationID,
        this.StockCategory,
        this.StockDenomination,
        this.StockCaseQty,
        this.StockPacketQty,
        this.StockSheetQty,
        this.StockStampQty
    });

            this.pnlStockEntry.Controls.Add(this.dgvStockEntry);

            ((System.ComponentModel.ISupportInitialize)(this.dgvStockEntry)).EndInit();

            InitializeGridValidation();


        }
        private void ResetSupplyEntry()
        {
            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow)
                    continue;

                // Supply Values
                row.Cells["SupplySheets"].Value = 0;
                row.Cells["SupplyPieces"].Value = 0;
                row.Cells["SupplyTotalPieces"].Value = 0;

                // Other Fields
                row.Cells["LedgerFolio"].Value = "";
                row.Cells["CaseCode"].Value = "";
                row.Cells["CaseNoFrom"].Value = "";
                row.Cells["CaseNoTo"].Value = "";

                // Default Row Color
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.Black;

                // Remaining Qty Highlight
                row.Cells["RemainingPieces"].Style.BackColor = Color.MistyRose;
                row.Cells["RemainingPieces"].Style.ForeColor = Color.DarkRed;
                row.Cells["RemainingPieces"].Style.Font =
                    new Font(dgvSupplyDetail.Font, FontStyle.Bold);

                // User Input Cells
                row.Cells["SupplySheets"].Style.BackColor = Color.LightYellow;
                row.Cells["SupplyPieces"].Style.BackColor = Color.LightYellow;

                // Total Pieces
                row.Cells["SupplyTotalPieces"].Style.BackColor = Color.AliceBlue;
            }
        }

        private void FormDataChanged(object sender, EventArgs e)
        {
            if (_isLoadingDraft)
                return;

            CheckDraftChanges();
        }

        private void RegisterChangeEvents()
        {
            cmbSupplyType.SelectedIndexChanged += FormDataChanged;
            cmbDispatchMode.SelectedIndexChanged += FormDataChanged;
            cmbPackingType.SelectedIndexChanged += FormDataChanged;
            dtSupplyDate.ValueChanged += FormDataChanged;

            txtPackingQty.TextChanged += FormDataChanged;
            txtRemarks.TextChanged += FormDataChanged;

            dgvSupplyDetail.CellValueChanged += DgvSupplyDetail_CellValueChanged;


        }

        private bool ValidateStockEntry()
        {
            string packingType = cmbPackingType.Text.Trim();

            bool hasAnyStock = false;
            bool hasCase = false;
            bool hasPacket = false;
            bool hasSheet = false;
            bool hasStamp = false;

            // =========================================
            // Check Stock Entry Grid
            // =========================================

            foreach (DataGridViewRow row in dgvStockEntry.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int caseQty = 0;
                int packetQty = 0;
                int sheetQty = 0;
                int stampQty = 0;

                int.TryParse(
                    Convert.ToString(row.Cells["StockCaseQty"].Value),
                    out caseQty);

                int.TryParse(
                    Convert.ToString(row.Cells["StockPacketQty"].Value),
                    out packetQty);

                int.TryParse(
                    Convert.ToString(row.Cells["StockSheetQty"].Value),
                    out sheetQty);

                int.TryParse(
                    Convert.ToString(row.Cells["StockStampQty"].Value),
                    out stampQty);

                if (caseQty > 0)
                    hasCase = true;

                if (packetQty > 0)
                    hasPacket = true;

                if (sheetQty > 0)
                    hasSheet = true;

                if (stampQty > 0)
                    hasStamp = true;

                if (caseQty > 0 ||
                    packetQty > 0 ||
                    sheetQty > 0 ||
                    stampQty > 0)
                {
                    hasAnyStock = true;
                }
            }


            // =========================================
            // No Stock Entry
            // =========================================

            if (!hasAnyStock)
            {
                MessageBox.Show(
                    "Stock Entry mein kam az kam ek quantity enter karein.",
                    "Stock Entry",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }


            // =========================================
            // CASE
            // =========================================

            if (packingType == "Case")
            {
                if (!hasCase)
                {
                    MessageBox.Show(
                        "Case packing select hai. Stock Entry Grid mein Case Qty enter karna zaroori hai.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // Case ke sath koi aur stock allowed nahi
                if (hasPacket || hasSheet || hasStamp)
                {
                    MessageBox.Show(
                        "Case ke sath Packet, Sheet ya Stamp stock enter nahi kar sakte.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }


            // =========================================
            // PACKET
            // =========================================

            else if (packingType == "Packet")
            {
                if (!hasPacket)
                {
                    MessageBox.Show(
                        "Packet packing select hai. Stock Entry Grid mein Packet Qty enter karna zaroori hai.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // Case ke sath Packet allowed nahi
                if (hasCase)
                {
                    MessageBox.Show(
                        "Packet ke sath Case stock enter nahi kar sakte.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }


            // =========================================
            // SHEETS
            // =========================================

            else if (packingType == "Sheets")
            {
                if (!hasSheet)
                {
                    MessageBox.Show(
                        "Sheets packing select hai. Stock Entry Grid mein Sheet Qty enter karna zaroori hai.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // Case ke sath Sheet allowed nahi
                if (hasCase)
                {
                    MessageBox.Show(
                        "Sheets ke sath Case stock enter nahi kar sakte.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }


            // =========================================
            // LABELS
            // =========================================

            else if (packingType == "Labels")
            {
                if (!hasStamp)
                {
                    MessageBox.Show(
                        "Labels packing select hai. Stock Entry Grid mein Stamp Qty enter karna zaroori hai.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // Case ke sath Stamp allowed nahi
                if (hasCase)
                {
                    MessageBox.Show(
                        "Labels ke sath Case stock enter nahi kar sakte.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }


            // =========================================
            // OTHER PACKING TYPES
            // Bags / Parce / Envelope / Other
            // =========================================

            else
            {
                // Case sirf Case packing ke sath
                if (hasCase)
                {
                    MessageBox.Show(
                        "Case Qty sirf Case packing type ke sath enter ki ja sakti hai.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // Packet / Sheet / Stamp mein se kam az kam ek
                if (!hasPacket &&
                    !hasSheet &&
                    !hasStamp)
                {
                    MessageBox.Show(
                        "Stock Entry mein Packet, Sheet ya Stamp mein se kam az kam ek quantity enter karein.",
                        "Stock Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }

            return true;
        }
        private void DgvSupplyDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                MarkDataChanged();
        }

        private void frmSupply_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);



            //--------------------------------------------------
            // Common Setup
            //--------------------------------------------------

            dgvSupplyDetail.Columns["originalPendingPieces"].Visible = true;

            dgvSupplyDetail.Columns["originalPendingPieces"]
                .DefaultCellStyle.BackColor = Color.LightYellow;

            dgvSupplyDetail.Columns["originalPendingPieces"]
                .DefaultCellStyle.ForeColor = Color.Black;

         

            btnApprove.Visible = false;
            btnIssue.Visible = false;

            LoadSupplyTypes();


            //--------------------------------------------------
            // Existing Supply / Draft / Approved / Issued
            //--------------------------------------------------

            if (_isDraft)
            {
                _isEditDraft = true;

                _isLoadingDraft = true;

             this.Text = "Supply - Edit Draft";

                if (dgvSupplyDetail.Columns.Contains("RemainingPieces"))
                {

                    dgvSupplyDetail.Columns["RemainingPieces"].Visible = false;
                }
                if (dgvSupplyDetail.Columns.Contains("RemainingTotalPieces"))
                {
                    dgvSupplyDetail.Columns["RemainingTotalPieces"].HeaderText = "Remaining Sheets";
                }
               
                bool hasValue = false;

                foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Cells["IndentLoosePieces"].Value != null &&
                        row.Cells["IndentLoosePieces"].Value != DBNull.Value)
                    {
                        if (int.TryParse(row.Cells["IndentLoosePieces"].Value.ToString(), out int val))
                        {
                            if (val > 0)
                            {
                                hasValue = true;
                                break; // 0 se bari value miltay hi loop rok dein
                            }
                        }
                    }
                }

                // Target column ko show ya hide karein
                if (dgvSupplyDetail.Columns.Contains("IndentLoosePieces"))
                {
                    dgvSupplyDetail.Columns["IndentLoosePieces"].Visible = hasValue;
                    dgvSupplyDetail.Columns["SupplyPieces"].Visible = hasValue;
                    dgvSupplyDetail.Columns["IndentTotalPieces"].Visible = hasValue;

                }


                try
                {
                    LoadDraftHeader(_supplyID);
                    LoadDraftItems();


                    if (_supplyID > 0)
                    {
                        

                        LoadSavedStockTransactions(_supplyID);
                    }
                    

                    // Original values محفوظ کریں
                    SaveOriginalDraftValues();

                    SetSupplyTypeByCategory();
                }
                finally
                {
                    _isLoadingDraft = false;
                }

                RegisterChangeEvents();

              
                //--------------------------------------------------
                // Draft
                //--------------------------------------------------

                if (_currentStatusID == 1)
                {
                    this.Text = "Supply - Edit Draft";

                    btnSaveDraft.Text = "Update Draft";
                    btnSaveDraft.Enabled = true;

                    btnApprove.Visible = true;
                    btnApprove.Enabled = true;
                    btnSaveDraft.Visible = false;
                    btnIssue.Visible = false;
                }


                //--------------------------------------------------
                // Approved
                //--------------------------------------------------

                else if (_currentStatusID == 2)
                {
                    this.Text = "Supply - Approved";
                    btnSaveDraft.Enabled = true;

                    btnApprove.Visible = false;
                    btnSaveDraft.Visible = false;
                    btnSaveDraft.Text = "Update Draft";

                    btnIssue.Visible = true;
                    btnIssue.Enabled = true;
                }


                //--------------------------------------------------
                // Issued
                //--------------------------------------------------

                else if (_currentStatusID == 3)
                {
                    this.Text = "Supply - Issued";

                    btnSaveDraft.Enabled = false;

                    btnApprove.Visible = false;
                    btnSaveDraft.Visible = false;

                    btnIssue.Visible = true;
                    btnIssue.Enabled = false;

                    dgvSupplyDetail.ReadOnly = true;

                    cmbSupplyType.Enabled = false;
                    cmbDispatchMode.Enabled = false;
                    cmbPackingType.Enabled = false;

                    txtPackingQty.ReadOnly = true;
                    txtRemarks.ReadOnly = true;
                }
            }


            //--------------------------------------------------
            // New Supply
            //--------------------------------------------------

            else
            {
                this.Text = "Supply - New Entry";
                LoadIndentHeader();

                LoadIndentItems();

                ResetSupplyEntry();

                SetSupplyTypeByCategory();

                btnApprove.Visible = false;
                btnIssue.Visible = false;

                btnSaveDraft.Text = "Save Draft";
                btnSaveDraft.Visible = true;

            }

            dgvSupplyDetail.AutoSizeColumnsMode =
    DataGridViewAutoSizeColumnsMode.None;

            dgvSupplyDetail.ScrollBars =
                ScrollBars.Both;

            dgvSupplyDetail.AllowUserToResizeColumns =
                true;

            dgvSupplyDetail.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;

            
        }

        private void txtPackingQty_KeyPress(object sender, KeyPressEventArgs e)
        {
         
            // Allow only digits and Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

            #region Supply Grid: constants, status colors, and state

            // Column names - single source of truth, avoids typo bugs
        private const string ColSupplySheets = "SupplySheets";
        private const string ColSupplyPieces = "SupplyPieces";
        private const string ColPiecesPerSheet = "PiecesPerSheet";
        private const string ColRemainingPieces = "RemainingPieces";
        private const string ColSupplyTotalPieces = "SupplyTotalPieces";

        // Status colors
        private static readonly Color ColorOverSupplyBack = Color.MistyRose;      // Red    - Total > Remaining
        private static readonly Color ColorOverSupplyFore = Color.DarkRed;
        private static readonly Color ColorFullySuppliedBack = Color.PaleGreen;   // Green  - Total == Remaining
        private static readonly Color ColorFullySuppliedFore = Color.DarkGreen;
        private static readonly Color ColorPartialSupplyBack = Color.LightYellow; // Yellow - 0 < Total < Remaining
        private static readonly Color ColorPartialSupplyFore = Color.DarkGoldenrod;
        private static readonly Color ColorNoEntryBack = Color.Empty;             // Default - Total == 0
        private static readonly Color ColorNoEntryFore = Color.Empty;

        // Stops this handler from re-processing its own programmatic cell updates
        private bool _isUpdatingSupplyRow = false;

            #endregion

        /// <summary>Reads a cell as an int. Blank or non-numeric becomes 0.</summary>
        private static int ParseInt(object cellValue)
        {
            int.TryParse(Convert.ToString(cellValue), out int result);
            return result;
        }

        /// <summary>
        /// Reads a cell, treats blank/invalid as 0, clamps negative values to 0,
        /// writes the cleaned value back into the cell, and returns it.
        /// </summary>
        private static int NormalizeToNonNegativeInt(DataGridViewRow row, string columnName)
        {
            int value = ParseInt(row.Cells[columnName].Value);

            if (value < 0)
                value = 0;

            row.Cells[columnName].Value = value;
            return value;
        }

       
        private static void ApplyStatusColor(DataGridViewRow row, int totalSupply, int remainingPieces)
        {
            Color back, fore;

            if (totalSupply > remainingPieces)
            {
                back = ColorOverSupplyBack;
                fore = ColorOverSupplyFore;
            }
            else if (totalSupply == 0)
            {
                back = ColorNoEntryBack;
                fore = ColorNoEntryFore;
            }
            else if (totalSupply == remainingPieces)
            {
                back = ColorFullySuppliedBack;
                fore = ColorFullySuppliedFore;
            }
            else // 0 < totalSupply < remainingPieces
            {
                back = ColorPartialSupplyBack;
                fore = ColorPartialSupplyFore;
            }

            foreach (string colName in new[] { ColSupplyTotalPieces, ColRemainingPieces })
            {
                DataGridViewCellStyle style = row.Cells[colName].Style;
                style.BackColor = back;
                style.ForeColor = fore;
                style.SelectionBackColor = back;
                style.SelectionForeColor = fore;
            }
        }
        

        private void dgvSupplyDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            string col = dgvSupplyDetail.Columns[e.ColumnIndex].Name;

            if (col != "SupplySheets" && col != "SupplyPieces")
                return;

            string value = e.FormattedValue.ToString().Trim();

            // اگر Cell خالی ہے تو Allow کریں
            if (string.IsNullOrWhiteSpace(value))
                return;

            int number;

            if (!int.TryParse(value, out number))
            {
                MessageBox.Show(
    "Column : " + dgvSupplyDetail.Columns[e.ColumnIndex].Name +
    "\nValue = [" + e.FormattedValue.ToString() + "]");
                MessageBox.Show(
                    "Only numeric values are allowed.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                e.Cancel = true;
                return;
            }

            if (number < 0)
            {
                MessageBox.Show(
                    "Negative values are not allowed.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                e.Cancel = true;
            }
        }

        private void dgvSupplyDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
          
            MessageBox.Show(
                "Column : " + dgvSupplyDetail.Columns[e.ColumnIndex].Name +
                "\nValue : " + Convert.ToString(dgvSupplyDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue) +
                "\n\n" + e.Exception.Message);

            e.ThrowException = false;
        }

        private void dgvSupplyDetail_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            string col = dgvSupplyDetail.Columns[e.ColumnIndex].Name;

            if (col == "SupplySheets" || col == "SupplyPieces")
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(e.Value)))
                {
                    e.Value = 0;
                    e.ParsingApplied = true;
                }
            }
        }

       
        private void cmbPackingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDraftChanges();
            bool isCase = cmbPackingType.Text == "Case";

            dgvSupplyDetail.Columns["CaseNoFrom"].ReadOnly = !isCase;
            dgvSupplyDetail.Columns["CaseNoTo"].ReadOnly = !isCase;
            dgvSupplyDetail.Columns["CaseCode"].ReadOnly = !isCase;

            // Ledger ہمیشہ Editable رہے گا
            dgvSupplyDetail.Columns["LedgerFolio"].ReadOnly = false;

            dgvSupplyDetail.Columns["CaseNoFrom"].DefaultCellStyle.BackColor =
                isCase ? Color.White : Color.LightGray;

            dgvSupplyDetail.Columns["CaseNoTo"].DefaultCellStyle.BackColor =
                isCase ? Color.White : Color.LightGray;

            dgvSupplyDetail.Columns["CaseCode"].DefaultCellStyle.BackColor =
                isCase ? Color.White : Color.LightGray;


            if (!isCase)
            {
                foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
                {
                    row.Cells["CaseNoFrom"].Value = DBNull.Value;
                    row.Cells["CaseNoTo"].Value = DBNull.Value;
                    row.Cells["CaseCode"].Value = "";
                }
            }
        }

        

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (!ValidateSupply(true))
                return;

            try
            {
                // ==================================================
                // STEP 1
                // Draft موجود ہونا چاہیے
                // ==================================================

                if (_supplyID == 0)
                {
                    MessageBox.Show(
                        "Please save the Draft first.",
                        "Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }


                // ==================================================
                // STEP 2
                // Approval Confirmation
                // ==================================================

                DialogResult dr = MessageBox.Show(
                    "Do you want to approve this Draft?\n\n" +
                    "Once approved, the Supply Number will be finalized " +
                    "and the Draft cannot be edited.",
                    "Approve Supply",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                    return;


                // ==================================================
                // STEP 3
                // Save the currently opened SupplyID
                // ==================================================

                int openedSupplyID = _supplyID;


                // ==================================================
                // STEP 4
                // Get current Supply Number information
                // ==================================================

                SupplyNumberInfo currentInfo =
                    supplyDAL.GetSupplyNumberInfo(openedSupplyID);

                if (currentInfo == null)
                {
                    MessageBox.Show(
                        "Current Supply Number information could not be found.",
                        "Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();

                    return;
                }


                // ==================================================
                // STEP 5
                // Financial Year
                // Take it from the CURRENT Draft itself.
                // ==================================================

                string financialYear =
                    currentInfo.FinancialYear;


                // ==================================================
                // STEP 6
                // Get Office information
                // ==================================================

                var office =
                    supplyDAL.GetOfficeInfo(_indentID);

                // GetOfficeInfo() returns a value/object in
                // the current project, therefore we check OfficeID.
                if (office.OfficeID <= 0)
                {
                    MessageBox.Show(
                        "Office information could not be found.",
                        "Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();

                    return;
                }


                // ==================================================
                // STEP 7
                // Find FIRST AVAILABLE Global Sequence
                //
                // The current SupplyID is excluded inside DAL.
                // Draft + Approved + Issued are treated as occupied.
                // ==================================================

                int requiredGlobalSequence =
                    supplyDAL.GetNextAvailableGlobalSequence(
                        openedSupplyID,
                        financialYear);


                // ==================================================
                // STEP 8
                // Find FIRST AVAILABLE Office Sequence
                //
                // The current SupplyID is excluded inside DAL.
                // ==================================================

                int requiredOfficeSequence =
                    supplyDAL.GetNextAvailableOfficeSequence(
                        openedSupplyID,
                        office.OfficeID,
                        financialYear);


                // ==================================================
                // STEP 9
                // Compare current number with required number
                //
                // If current Draft number is wrong:
                //
                // 1. Correct ONLY this Draft
                // 2. Do NOT approve
                // 3. Inform the user
                // 4. Close the form
                //
                // User will reopen the Draft and approve it.
                // ==================================================

                if (currentInfo.GlobalSequence != requiredGlobalSequence ||
                    currentInfo.OfficeSequence != requiredOfficeSequence)
                {
                    bool updated =
                        supplyDAL.UpdateDraftSupplyNumber(
                            openedSupplyID,
                            requiredGlobalSequence,
                            requiredOfficeSequence,
                            office.OfficeCode,
                            financialYear);

                    if (!updated)
                    {
                        MessageBox.Show(
                            "Unable to correct the Supply Number.",
                            "Supply",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        this.DialogResult = DialogResult.Cancel;
                        this.Close();

                        return;
                    }

                    MessageBox.Show(
                        "The Supply Number has been corrected according to the current sequence.\n\n" +
                        "Please reopen this Draft and approve it.",
                        "Supply Number Corrected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();

                    return;
                }


                // ==================================================
                // STEP 10
                // Now check LOWEST DRAFT
                //
                // Number is already correct.
                // Now determine whether this Draft is next in line.
                // ==================================================

                if (!supplyDAL.CanApproveDraftSequentially(openedSupplyID))
                {
                    MessageBox.Show(
                        "This Supply cannot be approved yet because an earlier Draft Supply is still pending approval.\n\n" +
                        "Please approve the Draft Supplies in sequence.",
                        "Approval Sequence",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();

                    return;
                }


                // ==================================================
                // STEP 11
                // Save latest Draft Header changes
                // ==================================================

                supplyDAL.UpdateSupplyMaster(
                    openedSupplyID,
                    Convert.ToInt32(cmbSupplyType.SelectedValue),
                    dtSupplyDate.Value,
                    cmbDispatchMode.Text,
                    cmbPackingType.Text,
                    Convert.ToInt32(txtPackingQty.Text),
                    txtInvoiceNo?.Text ?? string.Empty,
                    txtRemarks.Text);


                // ==================================================
                // STEP 12
                // Save Grid Details
                // ==================================================

                SaveSupplyDetails();


                // ==================================================
                // STEP 13
                // Generate Final Approved Supply Number
                // ==================================================

                SupplyNumberGenerator generator =
                    new SupplyNumberGenerator();

                SupplyNumberInfo info =
                    generator.GenerateApprovedSupplyNumber(_indentID);


                // ==================================================
                // STEP 14
                // Ensure Invoice Number
                // ==================================================

                SupplyDAL dal =
                    new SupplyDAL();

                string invoice =
                    dal.EnsureValidInvoice(
                        openedSupplyID,
                        false);

                txtInvoiceNo.Text = invoice;


                // ==================================================
                // STEP 15
                // Approve Supply
                // ==================================================

                supplyDAL.ApproveSupply(
                    openedSupplyID,
                    info,
                    Convert.ToInt32(cmbSupplyType.SelectedValue),
                    cmbDispatchMode.Text,
                    cmbPackingType.Text,
                    Convert.ToInt32(txtPackingQty.Text),
                    txtRemarks.Text);


                // ==================================================
                // STEP 16
                // Update Indent Status
                // ==================================================

                supplyDAL.UpdateIndentStatusAfterSupply(
                    openedSupplyID,
                    _indentID);


                // ==================================================
                // STEP 17
                // Refresh Screen
                // ==================================================

                txtSupplyNo.Text =
                    info.SupplyNo;

                txtFinancialYear.Text =
                    info.FinancialYear;

                txtSupplyStatus.Text =
                    "Approved";


                // ==================================================
                // STEP 18
                // Success Message
                // ==================================================

                MessageBox.Show(
                    "Supply Approved Successfully.",
                    "Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);


                // ==================================================
                // STEP 19
                // Close Form
                // ==================================================

                this.DialogResult =
                    DialogResult.OK;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (!Validate(true))
                return;
            try
            {
                if (_supplyID == 0)
                {
                    MessageBox.Show(
                        "Supply record not found.",
                        "Issue Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // صرف Approved Supply ہی Issue ہوگی
                if (_currentStatusID != 2)
                {
                    SupplyDAL dal = new SupplyDAL();
                    string invoice = dal.EnsureValidInvoice(_supplyID, false);
                    txtInvoiceNo.Text = invoice;
                    MessageBox.Show(
                        "Only Approved Supply can be Issued.",
                        "Issue Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult dr = MessageBox.Show(
                    "Do you want to Issue this Supply?\n\nOnce Issued, it cannot be edited.",
                    "Issue Supply",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                    return;

                // Status = Issued
                supplyDAL.IssueSupply(_supplyID);

                _currentStatusID = 3;
                txtSupplyStatus.Text = "Issued";

                MessageBox.Show(
                    "Supply Issued Successfully.",
                    "Issue Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Form Lock
                btnSaveDraft.Enabled = false;
                btnApprove.Enabled = false;
                btnIssue.Enabled = false;

                cmbSupplyType.Enabled = false;
                cmbDispatchMode.Enabled = false;
                cmbPackingType.Enabled = false;

                txtPackingQty.ReadOnly = true;
                txtRemarks.ReadOnly = true;

                dgvSupplyDetail.ReadOnly = true;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            switch (_currentStatusID)
            {
                case 1: // Draft
                    btnSaveDraft.Visible = true;
                    btnApprove.Visible = true;
                    btnIssue.Visible = false;
                    btnCancel.Visible = true;

                    break;

                case 2: // Approved
                    btnSaveDraft.Visible = true;
                    btnApprove.Visible = false;
                    btnIssue.Visible = true;
                    btnCancel.Visible = false;
                    break;

                case 3: // Issued
                    btnSaveDraft.Visible = false;
                    btnApprove.Visible = false;
                    btnIssue.Visible = false;
                    btnCancel.Visible = false;
                    dgvSupplyDetail.ReadOnly = true;
                    break;

                case 4: // Cancelled
                    btnSaveDraft.Visible = false;
                    btnApprove.Visible = false;
                    btnIssue.Visible = false;
                    btnCancel.Visible = false;
                    dgvSupplyDetail.ReadOnly = true;
                    break;
            }
            try
            {
                if (_supplyID == 0)
                {
                    MessageBox.Show(
                        "No Supply selected.",
                        "Cancel Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // صرف Draft Cancel ہوگی
                if (_currentStatusID != 1)
                {
                    MessageBox.Show(
                        "Only Draft Supply can be Cancelled.",
                        "Cancel Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult dr = MessageBox.Show(
                    "Do you want to Cancel this Draft?\n\nThis Draft will no longer be available for processing.",
                    "Cancel Draft",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                    return;

                // Status = Cancelled
                supplyDAL.CancelSupply(_supplyID);

                _currentStatusID = 4;
                txtSupplyStatus.Text = "Cancelled";

                MessageBox.Show(
                    "Draft Cancelled Successfully.",
                    "Cancel Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rptSupplyPerforma_Click(object sender, EventArgs e)
        {
           
            if (_supplyID == 0)
            {
                MessageBox.Show(
                    "Please save Draft first.",
                    "Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
            if(string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
            {
                MessageBox.Show(
                    "Please assign Invoice Number first.",
                    "Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            frmReportPreview frm =
                new frmReportPreview(_supplyID);

            frm.ShowDialog();
        }

        private void btnAssignInvoiceNo_Click(object sender, EventArgs e)
        {
                SupplyDAL dal = new SupplyDAL();
            if (!dal.CanAssignInvoiceSequentially(_supplyID))
            {
                MessageBox.Show(
                    "Previous draft supply invoice must be assigned first.",
                    "Invoice Sequence",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {


                // پہلے Validation / Generation کرو
                string invoice = dal.EnsureValidInvoice(_supplyID, true);

                int openedSupplyID = _supplyID;

              
                txtInvoiceNo.Text = invoice;

                // اب Result Check کرو
                if (dal.InvoiceRegenerated)
                {
                    MessageBox.Show(
                        "Invoice Number was invalid and has been reassigned.",
                        "Invoice",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Invoice Number is already valid.",
                        "Invoice",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSaveDraft_Click(object sender, EventArgs e)
        {
           if (!ValidateSupply())
                return;

            if (!ValidateStockEntry())
                return;



            try
            {

                // =========================================
                // NEW SUPPLY
                // =========================================

                if (_supplyID == 0)
                {

                    SaveDraft();

                    MessageBox.Show(
                        "Draft Saved Successfully.",
                        "Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();

                    return;
                }


                // =========================================
                // EXISTING SUPPLY
                // =========================================

                // پہلے check کریں کہ واقعی کوئی change ہوا ہے یا نہیں
                CheckDraftChanges();

                if (!_isDataChanged)
                {
                    MessageBox.Show(
                        "No changes found.",
                        "Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }


                // =========================================
                // UPDATE
                // =========================================
                MessageBox.Show("Updating Draft",
                    "Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);


                UpdateDraft();

                

                _isDataChanged = false;

                // دوبارہ current values کا snapshot لے لیں
                SaveOriginalDraftValues();

                btnIssue.Enabled = true;
                btnIssue.Text = "Issue";


                MessageBox.Show(
                    "Changes Saved Successfully.",
                    "Supply",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvSupplyDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isUpdatingSupplyRow)
                return;

            string col = dgvSupplyDetail.Columns[e.ColumnIndex].Name;

            if (col != ColSupplySheets && col != ColSupplyPieces)
                return;

            _isUpdatingSupplyRow = true;

            try
            {
                DataGridViewRow row = dgvSupplyDetail.Rows[e.RowIndex];

                if (_isEditDraft)
                {
                    CheckDraftChanges();
                    _draftChanged = true;
                }

                //-----------------------------------------
                // Read Values
                //-----------------------------------------
                int supplySheets = NormalizeToNonNegativeInt(row, ColSupplySheets);
                int supplyPieces = NormalizeToNonNegativeInt(row, ColSupplyPieces);
                int piecesPerSheet = ParseInt(row.Cells[ColPiecesPerSheet].Value);
                int originalPending = ParseInt(row.Cells["OriginalPendingPieces"].Value);
                int indentTotalPieces = ParseInt(row.Cells["IndentTotalPieces"].Value);

                if (originalPending < 0) originalPending = 0;
                if (indentTotalPieces < 0) indentTotalPieces = 0;

                //-----------------------------------------
                // Total Supply Calculation
                //-----------------------------------------
                int totalSupply = (supplySheets * piecesPerSheet) + supplyPieces;
                row.Cells[ColSupplyTotalPieces].Value = totalSupply;

                //-----------------------------------------
                // Allowed Quantity Fix
                //-----------------------------------------
                int allowedQuantity;

                if (originalPending > 0)
                {
                    allowedQuantity = originalPending;
                }
                else
                {
                    allowedQuantity = indentTotalPieces;
                }

                //-----------------------------------------
                // Balance Calculation
                //-----------------------------------------
                int balance = allowedQuantity - totalSupply;

                if (balance < 0)
                    balance = 0;

                row.Cells[ColRemainingPieces].Value = balance;

                //-----------------------------------------
                // Validation
                //-----------------------------------------
                if (totalSupply > allowedQuantity)
                {
                    string quantityName = (_isDraft || _isEditDraft) ? "Indent Quantity" : "Pending Quantity";

                    MessageBox.Show(
                        $"Supply Quantity ({totalSupply:N0}) is greater than {quantityName} ({allowedQuantity:N0}).",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else if (totalSupply > 0 && totalSupply < allowedQuantity)
                {
                    int remaining = allowedQuantity - totalSupply;
                    string denomination = Convert.ToString(row.Cells["Denomination"].Value);

                    MessageBox.Show(
                        $"Rs.{denomination} remaining {remaining:N0}.",
                        "Partial Supply",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    _quantityWarningShown = false;
                }

                //-----------------------------------------
                // Row Color
                //-----------------------------------------
                ApplyStatusColor(row, totalSupply, allowedQuantity);

                // Sub Calculation khatam hone ke baad Stock Entry Reload karein
                LoadStockEntryGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isUpdatingSupplyRow = false;
            }
        }

        private void cmbSupplyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDraftChanges();
        }

        private void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
            CheckDraftChanges();
        }

        private void cmbDispatchMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDraftChanges();
        }

        private void txtPackingQty_TextChanged(object sender, EventArgs e)
        {
            CheckDraftChanges();
        }

        private void dgvSupplyDetail_CellClick(
    object sender,
    DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvSupplyDetail.Rows[e.RowIndex];

            if (row.Cells["CategoryID"].Value == null ||
                row.Cells["DenominationID"].Value == null)
                return;

            int categoryID =
                Convert.ToInt32(row.Cells["CategoryID"].Value);

            int denominationID =
                Convert.ToInt32(row.Cells["DenominationID"].Value);

            string category =
                Convert.ToString(row.Cells["Category"].Value);

            string denomination =
                Convert.ToString(row.Cells["Denomination"].Value);

            lblStockItem.Text =
                category + "  |  Rs." + denomination;

            // Next step:
            // StockTransaction se is CategoryID +
            // DenominationID ka available balance load hoga.
        }

        private void dgvSupplyDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvSupplyDetail.CellClick +=
    new System.Windows.Forms.DataGridViewCellEventHandler(
        this.dgvSupplyDetail_CellClick);

        }

        private void dgvSupplyDetail_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvSupplyDetail.Rows[e.RowIndex];

            if (row.Cells["CategoryID"].Value == null ||
                row.Cells["DenominationID"].Value == null)
            {
                ClearCurrentStockCards();
                return;
            }

            int categoryID = Convert.ToInt32(
                row.Cells["CategoryID"].Value);

            int denominationID = Convert.ToInt32(
                row.Cells["DenominationID"].Value);

            LoadCurrentStockCards(categoryID, denominationID);
        }

        private void LoadCurrentStockCards(
    int categoryID,
    int denominationID)
        {
            DataRow dr = stockDAL.GetStockBalance(
                categoryID,
                denominationID);

            if (dr == null)
            {
                ClearCurrentStockCards();
                return;
            }

            lblBoxBalance.Text =
                Convert.ToInt32(dr["BoxQty"]).ToString("N0");

            lblPacketBalance.Text =
                Convert.ToInt32(dr["PacketQty"]).ToString("N0");

            lblSheetBalance.Text =
                Convert.ToInt32(dr["SheetQty"]).ToString("N0");

            lblStampBalance.Text =
                Convert.ToInt32(dr["StampQty"]).ToString("N0");
        }

        private void ClearCurrentStockCards()
        {
            lblBoxBalance.Text = "0";
            lblPacketBalance.Text = "0";
            lblSheetBalance.Text = "0";
            lblStampBalance.Text = "0";
        }
        // 1. User jab cell me value change kare ya type kare:
        private void dgvStockEntry_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvStockEntry.IsCurrentCellDirty)
            {
                // Foran cell ki editing commit karein taake CellValueChanged event fire ho
                dgvStockEntry.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        // 2. Cell value commit hone par CheckDraftChanges call hoga:
        private void dgvStockEntry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure karein ke loading ke dauran event na chale
            if (!_isLoadingDraft && e.RowIndex >= 0)
            {
                CheckDraftChanges();
            }
        }

        private void StockQty_KeyPress(
    object sender,
    KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvSupplyDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSupplyDetail.Columns[e.ColumnIndex].Name == "Denomination" && e.Value != null && e.Value != DBNull.Value)
            {
                
                if (decimal.TryParse(e.Value.ToString(), out decimal amount))
                {
                    // Decimal ko Integer banakar format apply karein
                    e.Value = $"Rs.{Convert.ToInt32(amount)}/-";
                    e.FormattingApplied = true;
                }
            }
        }
    }
}

