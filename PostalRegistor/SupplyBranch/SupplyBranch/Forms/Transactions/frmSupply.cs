using SupplyBranch.DAL;
using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Transactions
{
    public partial class frmSupply : Form
    {
        // =====================================================================
        // Constants
        // =====================================================================

        // Grid column names - single source of truth
        private const string ColSupplySheets = "SupplySheets";
        private const string ColSupplyPieces = "SupplyPieces";
        private const string ColPiecesPerSheet = "PiecesPerSheet";
        private const string ColRemainingPieces = "RemainingPieces";
        private const string ColSupplyTotalPieces = "SupplyTotalPieces";
        private const string ColPending = "OriginalPendingPieces";

        // Supply status ids
        private const int StatusDraft = 1;
        private const int StatusApproved = 2;
        private const int StatusIssued = 3;
        private const int StatusCancelled = 4;

        // Row status colors
        private static readonly Color ColorOverSupplyBack = Color.MistyRose;      // red    - total > pending
        private static readonly Color ColorOverSupplyFore = Color.DarkRed;
        private static readonly Color ColorFullySuppliedBack = Color.PaleGreen;   // green  - total == pending
        private static readonly Color ColorFullySuppliedFore = Color.DarkGreen;
        private static readonly Color ColorPartialSupplyBack = Color.LightYellow; // yellow - 0 < total < pending
        private static readonly Color ColorPartialSupplyFore = Color.DarkGoldenrod;

        private static readonly Color EditCellBack = Color.FromArgb(254, 249, 195); // editable supply cells
        private static readonly Color LockedCellBack = Color.FromArgb(238, 240, 243);

        // =====================================================================
        // Fields
        // =====================================================================

        public bool addDate = false;   // used by the performa report

        private readonly SupplyDAL supplyDAL = new SupplyDAL();
        private readonly StockDAL stockDAL = new StockDAL();
        private readonly ErrorProvider _errors = new ErrorProvider();
        private readonly Label lblGridHint = new Label();
        private readonly Dictionary<string, int> _sheetsPerCaseCache = new Dictionary<string, int>();

        // Stock entry grid (built in code, hosted in pnlStockEntry)
        private DataGridView dgvStockEntry;
        private DataGridViewTextBoxColumn StockCategoryID;
        private DataGridViewTextBoxColumn StockDenominationID;
        private DataGridViewTextBoxColumn StockCategory;
        private DataGridViewTextBoxColumn StockDenomination;
        private DataGridViewTextBoxColumn StockCaseQty;
        private DataGridViewTextBoxColumn StockPacketQty;
        private DataGridViewTextBoxColumn StockSheetQty;
        private DataGridViewTextBoxColumn StockStampQty;
        private DataTable _stockTable;

        private SupplyNumberInfo _supplyInfo;

        private int _indentID;
        private int _supplyID = 0;
        private int _currentStatusID = StatusDraft;
        private int _indentStatus = 0;
        private int _stockCardRow = -1;

        private bool _isExisting = false;          // opened an existing supply (draft / approved / issued)
        private bool _isLoadingDraft = false;
        private bool _isUpdatingSupplyRow = false;
        private bool _isDataChanged = false;
        private bool _lastPackingWasCase = false;

        private string _originalSignature = "";

        // =====================================================================
        // Constructors
        // =====================================================================

        /// <summary>Open an existing supply (isDraft = true).</summary>
        public frmSupply(int supplyID, bool isDraft)
        {
            InitializeComponent();
            InitCommon();

            _supplyID = supplyID;
            _isExisting = isDraft;
        }

        /// <summary>Create a new supply for an indent.</summary>
        public frmSupply(int indentID)
        {
            InitializeComponent();
            InitCommon();

            _indentID = indentID;
            _isExisting = false;
        }

        // =====================================================================
        // One-time setup shared by both constructors
        // =====================================================================

        private void InitCommon()
        {
            // The pending column must show the real pending quantity (it was bound to RemainingTotalPieces)
            OriginalPendingPieces.DataPropertyName = "OriginalPendingPieces";

            BuildStockEntryGrid();

            _errors.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // ----- layout fixes (can later be moved into the Designer) -----
            fpnlButton.WrapContents = false;          // all buttons on one line
            fpnlButton.Width = 1100;

            pnlStockEntry.Width = 420;                // wider stock entry grid
            pnlCurrentStock.Left = pnlStockEntry.Right + 8;

            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // ----- keyboard / tab order -----
            cmbPackingType.TabIndex = 11;             // Packing Type is above Dispatch Mode on screen
            cmbDispatchMode.TabIndex = 12;

            foreach (TextBox tb in new[] { txtSupplyNo, txtFinancialYear, txtSupplyStatus, txtIndentNo,
                                           txtOfficeName, txtIndentStatus, txtIndentDate })
            {
                tb.TabStop = false;
            }

            txtInvoiceNo.ReadOnly = true;             // assigned only through the Assign Invoice button
            txtInvoiceNo.TabStop = false;

            // ----- hint shown while the supply grid is hidden -----
            lblGridHint.AutoSize = true;
            lblGridHint.Text = "Select Packing Type and Dispatch Mode to start entering supply quantities.";
            lblGridHint.Location = new Point(tableLayoutPanel2.Left + 20, tableLayoutPanel2.Top + 20);
            Controls.Add(lblGridHint);
            lblGridHint.BringToFront();

            // ----- events attached once -----
            dtSupplyDate.ValueChanged += FormDataChanged;
            txtRemarks.TextChanged += FormDataChanged;
            dgvSupplyDetail.CurrentCellChanged += dgvSupplyDetail_CurrentCellChanged;
        }

        private void BuildStockEntryGrid()
        {
            dgvStockEntry = new DataGridView();

            StockCategoryID = new DataGridViewTextBoxColumn();
            StockDenominationID = new DataGridViewTextBoxColumn();
            StockCategory = new DataGridViewTextBoxColumn();
            StockDenomination = new DataGridViewTextBoxColumn();
            StockCaseQty = new DataGridViewTextBoxColumn();
            StockPacketQty = new DataGridViewTextBoxColumn();
            StockSheetQty = new DataGridViewTextBoxColumn();
            StockStampQty = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)dgvStockEntry).BeginInit();

            dgvStockEntry.AllowUserToAddRows = false;
            dgvStockEntry.AllowUserToDeleteRows = false;
            dgvStockEntry.AllowUserToResizeRows = false;
            dgvStockEntry.AutoGenerateColumns = false;
            dgvStockEntry.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStockEntry.BackgroundColor = Color.White;
            dgvStockEntry.BorderStyle = BorderStyle.FixedSingle;
            dgvStockEntry.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStockEntry.Dock = DockStyle.Fill;
            dgvStockEntry.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvStockEntry.MultiSelect = false;
            dgvStockEntry.Name = "dgvStockEntry";
            dgvStockEntry.RowHeadersVisible = false;
            dgvStockEntry.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            StockCategoryID.DataPropertyName = "CategoryID";
            StockCategoryID.Name = "StockCategoryID";
            StockCategoryID.Visible = false;

            StockDenominationID.DataPropertyName = "DenominationID";
            StockDenominationID.Name = "StockDenominationID";
            StockDenominationID.Visible = false;

            StockCategory.DataPropertyName = "Category";
            StockCategory.HeaderText = "Category";
            StockCategory.Name = "StockCategory";
            StockCategory.ReadOnly = true;

            StockDenomination.DataPropertyName = "Denomination";
            StockDenomination.HeaderText = "Denomination";
            StockDenomination.Name = "StockDenomination";
            StockDenomination.ReadOnly = true;

            StockCaseQty.DataPropertyName = "BoxQty";
            StockCaseQty.HeaderText = "Case Qty";
            StockCaseQty.Name = "StockCaseQty";

            StockPacketQty.DataPropertyName = "PacketQty";
            StockPacketQty.HeaderText = "Packet Qty";
            StockPacketQty.Name = "StockPacketQty";

            StockSheetQty.DataPropertyName = "SheetQty";
            StockSheetQty.HeaderText = "Sheet Qty";
            StockSheetQty.Name = "StockSheetQty";

            StockStampQty.DataPropertyName = "StampQty";
            StockStampQty.HeaderText = "Stamp Qty";
            StockStampQty.Name = "StockStampQty";

            dgvStockEntry.Columns.AddRange(new DataGridViewColumn[]
            {
                StockCategoryID, StockDenominationID, StockCategory, StockDenomination,
                StockCaseQty, StockPacketQty, StockSheetQty, StockStampQty
            });

            foreach (DataGridViewColumn c in dgvStockEntry.Columns)
                c.SortMode = DataGridViewColumnSortMode.NotSortable;

            pnlStockEntry.Controls.Add(dgvStockEntry);

            ((System.ComponentModel.ISupportInitialize)dgvStockEntry).EndInit();

            // One DataTable for the whole life of the form; rows are added / removed / edited in place
            _stockTable = CreateStockTable();
            dgvStockEntry.DataSource = _stockTable;

            dgvStockEntry.EditingControlShowing += dgvStockEntry_EditingControlShowing;
            dgvStockEntry.CurrentCellDirtyStateChanged += dgvStockEntry_CurrentCellDirtyStateChanged;
            dgvStockEntry.CellValueChanged += dgvStockEntry_CellValueChanged;
            dgvStockEntry.CellFormatting += dgvStockEntry_CellFormatting;
            dgvStockEntry.DataError += dgvStockEntry_DataError;
        }

        private static DataTable CreateStockTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CategoryID", typeof(int));
            dt.Columns.Add("DenominationID", typeof(int));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Denomination", typeof(string));
            dt.Columns.Add("BoxQty", typeof(int));
            dt.Columns.Add("PacketQty", typeof(int));
            dt.Columns.Add("SheetQty", typeof(int));
            dt.Columns.Add("StampQty", typeof(int));
            return dt;
        }

        // =====================================================================
        // Small helpers
        // =====================================================================

        private static int ParseInt(object value)
        {
            int result;
            int.TryParse(Convert.ToString(value), out result);
            return result;
        }

        private static int ToInt(object value)
        {
            return (value == null || value == DBNull.Value) ? 0 : Convert.ToInt32(value);
        }

        private static string Str(object value)
        {
            return (Convert.ToString(value) ?? "").Trim();
        }

        private static void Info(string message)
        {
            MessageBox.Show(message, "Supply", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void Warn(string message)
        {
            MessageBox.Show(message, "Supply", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Supply", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>Shows a validation message, puts an error icon next to the control and focuses it.</summary>
        private bool Fail(string message, Control control)
        {
            if (control != null)
            {
                _errors.SetError(control, message);
                control.Focus();
            }

            MessageBox.Show(message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void CloseWithResult(DialogResult result)
        {
            DialogResult = result;
            Close();
        }

        private static string FormatDenomination(object value)
        {
            decimal amount;
            if (value != null && value != DBNull.Value && decimal.TryParse(Convert.ToString(value), out amount))
                return "Rs." + Convert.ToInt32(amount) + "/-";

            return Convert.ToString(value);
        }

        private bool IsPacking(string name)
        {
            return string.Equals(cmbPackingType.Text.Trim(), name, StringComparison.OrdinalIgnoreCase);
        }

        private static string StatusText(int statusID, object fallback)
        {
            switch (statusID)
            {
                case StatusDraft: return "Draft";
                case StatusApproved: return "Approved";
                case StatusIssued: return "Issued";
                case StatusCancelled: return "Cancelled";
                default: return Convert.ToString(fallback);
            }
        }

        private string GetOtherPackingTypeValue()
        {
            return IsPacking("Other") ? txtOtherPacking.Text.Trim() : null;
        }

        private int GetPackingQtyValue()
        {
            if (IsPacking("Other")) return 0;

            decimal qty;
            if (decimal.TryParse(txtPackingQty.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out qty))
                return (int)Math.Round(qty);

            return 0;
        }

        private int GetSupplyTypeID()
        {
            return Convert.ToInt32(cmbSupplyType.SelectedValue);
        }

        /// <summary>Pending quantity (in pieces) this supply may use for one grid row.</summary>
        private static int GetAllowedQuantity(DataGridViewRow row)
        {
            int pending = ParseInt(row.Cells[ColPending].Value);
            return pending < 0 ? 0 : pending;
        }

        private void UpdateWindowTitle()
        {
            if (_isExisting)
                Text = "Supply " + txtSupplyNo.Text + " - " + StatusText(_currentStatusID, txtSupplyStatus.Text);
            else
                Text = "New Supply - Indent " + txtIndentNo.Text;
        }

        // =====================================================================
        // Load
        // =====================================================================

        private void frmSupply_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            ConfigureGridColumns();

            btnAssignInvoiceNo.Visible = false;
            btnApprove.Visible = false;
            btnIssue.Visible = false;
            btnRefresh.Visible = false;

            LoadSupplyTypes();

            if (_isExisting)
            {
                if (!LoadExistingSupply())
                {
                    Warn("This supply could not be found.");
                    DialogResult = DialogResult.Cancel;
                    BeginInvoke(new Action(Close));
                    return;
                }
            }
            else
            {
                if (!LoadNewSupply())
                {
                    DialogResult = DialogResult.Cancel;
                    BeginInvoke(new Action(Close));
                    return;
                }
            }

            UpdateGridLayout();
            ApplyButtonState();
            UpdateWindowTitle();

            if (dgvSupplyDetail.Rows.Count > 0)
                ShowStockForRow(0);
        }

        private void ConfigureGridColumns()
        {
            DataGridView g = dgvSupplyDetail;

            foreach (DataGridViewColumn c in g.Columns)
                c.SortMode = DataGridViewColumnSortMode.NotSortable;

            g.Columns["DetailID"].Visible = false;
            g.Columns["CategoryID"].Visible = false;
            g.Columns["DenominationID"].Visible = false;
            g.Columns["RemainingTotalPieces"].Visible = false;

            // Header text (all quantities are in pieces)
            g.Columns["PiecesPerSheet"].HeaderText = "Pieces / Sheet";
            g.Columns["IndentSheets"].HeaderText = "Indent Sheets";
            g.Columns["IndentLoosePieces"].HeaderText = "Indent Loose Pieces";
            g.Columns["IndentTotalPieces"].HeaderText = "Indent Total Pieces";
            g.Columns[ColPending].HeaderText = "Pending Pieces";
            g.Columns[ColRemainingPieces].HeaderText = "Balance Pieces";
            g.Columns[ColSupplySheets].HeaderText = "Supply Sheets";
            g.Columns[ColSupplyPieces].HeaderText = "Supply Pieces";
            g.Columns[ColSupplyTotalPieces].HeaderText = "Total Supply Pieces";
            g.Columns["LedgerFolio"].HeaderText = "Ledger Folio";

            // Numbers right aligned
            foreach (string name in new[] { "IndentSheets", "IndentLoosePieces", "IndentTotalPieces", ColPending,
                                            ColRemainingPieces, ColSupplySheets, ColSupplyPieces, ColSupplyTotalPieces })
            {
                g.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Cells the user types into are light yellow
            g.Columns[ColSupplySheets].DefaultCellStyle.BackColor = EditCellBack;
            g.Columns[ColSupplyPieces].DefaultCellStyle.BackColor = EditCellBack;

            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            g.ScrollBars = ScrollBars.Both;
            g.AllowUserToResizeColumns = true;
            g.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            SetWidth("Category", 150);
            SetWidth("Denomination", 100);
            SetWidth("PiecesPerSheet", 95);
            SetWidth("IndentSheets", 95);
            SetWidth("IndentLoosePieces", 120);
            SetWidth("IndentTotalPieces", 120);
            SetWidth(ColPending, 115);
            SetWidth(ColRemainingPieces, 115);
            SetWidth(ColSupplySheets, 105);
            SetWidth(ColSupplyPieces, 105);
            SetWidth(ColSupplyTotalPieces, 135);
            SetWidth("LedgerFolio", 110);
            SetWidth("CaseCode", 100);
            SetWidth("CaseNoFrom", 110);
            SetWidth("CaseNoTo", 110);
        }

        private void SetWidth(string columnName, int width)
        {
            if (dgvSupplyDetail.Columns.Contains(columnName))
                dgvSupplyDetail.Columns[columnName].Width = width;
        }

        private void LoadSupplyTypes()
        {
            cmbSupplyType.DataSource = supplyDAL.GetSupplyTypes();
            cmbSupplyType.DisplayMember = "SupplyTypeName";
            cmbSupplyType.ValueMember = "SupplyTypeID";
            cmbSupplyType.SelectedIndex = -1;
        }

        // ---------------------------------------------------------------------
        // New supply
        // ---------------------------------------------------------------------

        private bool LoadNewSupply()
        {
            LoadIndentHeader();

            if (_indentStatus == 7)
            {
                Warn("This indent is already closed. No further supply can be made.");
                return false;
            }

            LoadIndentItems();

            if (dgvSupplyDetail.Rows.Count == 0)
            {
                Warn("This indent has no pending items.");
                return false;
            }

            ResetSupplyEntry();
            SetSupplyTypeByCategory();

            chkAddDate.Visible = false;
            return true;
        }

        private void LoadIndentHeader()
        {
            DataTable dt = supplyDAL.GetIndentHeader(_indentID);
            if (dt.Rows.Count == 0)
                return;

            DataRow row = dt.Rows[0];

            txtIndentNo.Text = Convert.ToString(row["IndentNo"]);
            txtIndentDate.Text = Convert.ToDateTime(row["IndentDate"]).ToString("dd-MM-yyyy");
            txtOfficeName.Text = Convert.ToString(row["OfficeName"]);
            txtIndentStatus.Text = Convert.ToString(row["StatusName"]);
            _indentStatus = ToInt(row["IndentStatus"]);

            // A new supply always gets a fresh supply number
            if (!_isExisting)
            {
                SupplyNumberGenerator generator = new SupplyNumberGenerator();
                _supplyInfo = generator.GenerateSupplyNumber(_indentID);

                txtFinancialYear.Text = _supplyInfo.FinancialYear;
                txtSupplyNo.Text = _supplyInfo.SupplyNo;
                _supplyID = 0;
            }
        }

        private void LoadIndentItems()
        {
            dgvSupplyDetail.AutoGenerateColumns = false;
            dgvSupplyDetail.DataSource = supplyDAL.GetIndentItems(_indentID);
            _stockCardRow = -1;

            // The view already gives the pending quantity of EACH item
            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int pending = ParseInt(row.Cells["RemainingTotalPieces"].Value);
                row.Cells[ColPending].Value = pending;
                row.Cells[ColRemainingPieces].Value = pending;
            }
        }

        // ---------------------------------------------------------------------
        // Existing supply
        // ---------------------------------------------------------------------

        private bool LoadExistingSupply()
        {
            _isLoadingDraft = true;

            try
            {
                if (!LoadDraftHeader(_supplyID))
                    return false;

                LoadDraftItems();
                LoadSavedStockTransactions(_supplyID);

                // Supply type follows the categories only while the supply is still a draft
                if (_currentStatusID == StatusDraft)
                    SetSupplyTypeByCategory();
            }
            finally
            {
                _isLoadingDraft = false;
            }

            if (_currentStatusID == StatusIssued || _currentStatusID == StatusCancelled)
                LockForm();

            SaveOriginalState();
            return true;
        }

        private bool LoadDraftHeader(int supplyID)
        {
            DataTable dt = supplyDAL.GetDraftHeader(supplyID);
            if (dt.Rows.Count == 0)
                return false;

            DataRow row = dt.Rows[0];

            _indentID = Convert.ToInt32(row["IndentID"]);
            LoadIndentHeader();

            txtSupplyNo.Text = Convert.ToString(row["SupplyNo"]);
            txtInvoiceNo.Text = Convert.ToString(row["InvoiceNo"]);
            txtFinancialYear.Text = Convert.ToString(row["FinancialYear"]);
            dtSupplyDate.Value = Convert.ToDateTime(row["SupplyDate"]);

            cmbSupplyType.SelectedValue = Convert.ToInt32(row["SupplyType"]);
            cmbDispatchMode.Text = Convert.ToString(row["DispatchMode"]);
            cmbPackingType.Text = Convert.ToString(row["PackingType"]);
            txtOtherPacking.Text = Convert.ToString(row["OtherPackingType"]);
            txtPackingQty.Text = Convert.ToString(row["PackingQty"]);   // after the packing type (its handler resets it)
            txtRemarks.Text = Convert.ToString(row["Remarks"]);

            _currentStatusID = Convert.ToInt32(row["StatusID"]);
            txtSupplyStatus.Text = StatusText(_currentStatusID, row["SupplyStatus"]);
            UITheme.StyleStatusBox(txtSupplyStatus, _currentStatusID);

            return true;
        }

        private void LoadDraftItems()
        {
            dgvSupplyDetail.AutoGenerateColumns = false;

            DataTable dt = supplyDAL.GetSupplyDetailsForEdit(_supplyID);

            if (dt.Rows.Count > 0)
            {
                dgvSupplyDetail.DataSource = dt;
                _stockCardRow = -1;
                RefreshAllRowStatus();
            }
            else
            {
                // Draft without saved detail rows: start from the indent items
                LoadIndentItems();
                ResetSupplyEntry();
            }
        }

        private void LoadSavedStockTransactions(int supplyID)
        {
            _stockTable.Clear();

            if (supplyID <= 0)
                return;

            try
            {
                DataTable saved = stockDAL.GetStockTransactionsBySupplyID(supplyID);

                foreach (DataRow src in saved.Rows)
                {
                    DataRow r = _stockTable.NewRow();
                    r["CategoryID"] = ToInt(src["CategoryID"]);
                    r["DenominationID"] = ToInt(src["DenominationID"]);
                    r["Category"] = Convert.ToString(src["Category"]);
                    r["Denomination"] = Convert.ToString(src["Denomination"]);
                    r["BoxQty"] = ToInt(src["BoxQty"]);
                    r["PacketQty"] = ToInt(src["PacketQty"]);
                    r["SheetQty"] = ToInt(src["SheetQty"]);
                    r["StampQty"] = ToInt(src["StampQty"]);
                    _stockTable.Rows.Add(r);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load the stock entry.\n\n" + ex.Message,
                    "Supply", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSupplyTypeByCategory()
        {
            bool category2Found = false;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                if (ParseInt(row.Cells["CategoryID"].Value) == 2)
                {
                    category2Found = true;
                    break;
                }
            }

            int index = category2Found ? 4 : 3;

            if (cmbSupplyType.Items.Count > index)
                cmbSupplyType.SelectedIndex = index;
        }

        private void LockForm()
        {
            dgvSupplyDetail.ReadOnly = true;
            dgvStockEntry.ReadOnly = true;

            cmbSupplyType.Enabled = false;
            cmbDispatchMode.Enabled = false;
            cmbPackingType.Enabled = false;
            dtSupplyDate.Enabled = false;

            txtPackingQty.ReadOnly = true;
            txtOtherPacking.ReadOnly = true;
            txtRemarks.ReadOnly = true;
        }

        // =====================================================================
        // Button state
        // =====================================================================

        private void ApplyButtonState()
        {
            if (!_isExisting)
            {
                btnSaveDraft.Visible = true;
                btnSaveDraft.Enabled = true;
                btnSaveDraft.Text = "Save Draft";

                btnApprove.Visible = false;
                btnIssue.Visible = false;
                btnAssignInvoiceNo.Visible = false;
                btnSupplyPerforma.Visible = false;
                btnRefresh.Visible = true;
                return;
            }

            bool changed = _isDataChanged;
            bool editable = _currentStatusID == StatusDraft || _currentStatusID == StatusApproved;

            btnRefresh.Visible = false;
            btnSupplyPerforma.Visible = true;

            btnSaveDraft.Visible = editable && changed;
            btnSaveDraft.Enabled = true;
            btnSaveDraft.Text = "Save Changes";

            btnApprove.Visible = _currentStatusID == StatusDraft && !changed;
            btnApprove.Enabled = true;

            btnAssignInvoiceNo.Visible = _currentStatusID == StatusDraft;

            btnIssue.Visible = _currentStatusID == StatusApproved || _currentStatusID == StatusIssued;
            btnIssue.Enabled = _currentStatusID == StatusApproved && !changed;
            btnIssue.Text = (_currentStatusID == StatusApproved && changed) ? "Save Changes First" : "Issue Supply";
        }

        // =====================================================================
        // Change tracking (one signature string for everything the user can edit)
        // =====================================================================

        private string BuildSignature()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(dtSupplyDate.Value.ToString("yyyy-MM-dd")).Append('#')
              .Append(Convert.ToString(cmbSupplyType.SelectedValue)).Append('#')
              .Append(cmbDispatchMode.Text).Append('#')
              .Append(cmbPackingType.Text).Append('#')
              .Append(txtOtherPacking.Text.Trim()).Append('#')
              .Append(txtPackingQty.Text.Trim()).Append('#')
              .Append(txtRemarks.Text.Trim());

            sb.Append("|S:");
            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int sheets = ParseInt(row.Cells[ColSupplySheets].Value);
                int pieces = ParseInt(row.Cells[ColSupplyPieces].Value);
                if (sheets <= 0 && pieces <= 0) continue;

                sb.Append(Str(row.Cells["DetailID"].Value)).Append('=')
                  .Append(sheets).Append(',')
                  .Append(pieces).Append(',')
                  .Append(Str(row.Cells["LedgerFolio"].Value)).Append(',')
                  .Append(Str(row.Cells["CaseCode"].Value)).Append(',')
                  .Append(Str(row.Cells["CaseNoFrom"].Value)).Append(',')
                  .Append(Str(row.Cells["CaseNoTo"].Value)).Append(';');
            }

            sb.Append("|K:");
            DataView sorted = new DataView(_stockTable);
            sorted.Sort = "CategoryID, DenominationID";
            foreach (DataRowView r in sorted)
            {
                sb.Append(ToInt(r["CategoryID"])).Append('_').Append(ToInt(r["DenominationID"])).Append('=')
                  .Append(ToInt(r["BoxQty"])).Append(',')
                  .Append(ToInt(r["PacketQty"])).Append(',')
                  .Append(ToInt(r["SheetQty"])).Append(',')
                  .Append(ToInt(r["StampQty"])).Append(';');
            }

            return sb.ToString();
        }

        private void SaveOriginalState()
        {
            _originalSignature = BuildSignature();
            _isDataChanged = false;
            ApplyButtonState();
        }

        private void CheckDraftChanges()
        {
            if (!_isExisting || _isLoadingDraft)
                return;

            _isDataChanged = BuildSignature() != _originalSignature;
            ApplyButtonState();
        }

        private void FormDataChanged(object sender, EventArgs e)
        {
            if (_isLoadingDraft)
                return;

            _errors.Clear();
            CheckDraftChanges();
        }

        private bool HasUnsavedChanges()
        {
            if (_isExisting)
                return _isDataChanged;

            if (_supplyID != 0)
                return false;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;
                if (ParseInt(row.Cells[ColSupplySheets].Value) > 0 || ParseInt(row.Cells[ColSupplyPieces].Value) > 0)
                    return true;
            }

            return false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.Cancel || DialogResult == DialogResult.OK)
                return;

            if (HasUnsavedChanges())
            {
                DialogResult answer = MessageBox.Show(
                    "You have unsaved changes. Close without saving?",
                    "Supply", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (answer != DialogResult.Yes)
                    e.Cancel = true;
            }
        }

        // =====================================================================
        // Supply grid: layout, status colors, entry
        // =====================================================================

        /// <summary>Shows / hides the supply grid and the columns that depend on packing type.</summary>
        private void UpdateGridLayout()
        {
            bool ready = cmbPackingType.SelectedIndex > -1 && cmbDispatchMode.SelectedIndex > -1;

            dgvSupplyDetail.Visible = ready;
            lblGridHint.Visible = !ready;

            bool isCase = IsPacking("Case");
            SetColumnVisible("CaseCode", isCase);
            SetColumnVisible("CaseNoFrom", isCase);
            SetColumnVisible("CaseNoTo", isCase);

            bool hasLoose = false;
            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                if (ParseInt(row.Cells["IndentLoosePieces"].Value) > 0)
                {
                    hasLoose = true;
                    break;
                }
            }

            SetColumnVisible("IndentLoosePieces", hasLoose);
        }

        private void SetColumnVisible(string columnName, bool visible)
        {
            if (dgvSupplyDetail.Columns.Contains(columnName))
                dgvSupplyDetail.Columns[columnName].Visible = visible;
        }

        private void ApplyCaseColumnState(bool isCase)
        {
            string[] caseColumns = { "CaseNoFrom", "CaseNoTo", "CaseCode" };

            foreach (string name in caseColumns)
            {
                DataGridViewColumn col = dgvSupplyDetail.Columns[name];
                col.ReadOnly = !isCase;
                col.DefaultCellStyle.BackColor = isCase ? Color.White : LockedCellBack;
            }

            dgvSupplyDetail.Columns["LedgerFolio"].ReadOnly = false;

            if (!isCase && !_isLoadingDraft)
            {
                _isUpdatingSupplyRow = true;
                try
                {
                    foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
                    {
                        if (row.IsNewRow) continue;

                        row.Cells["CaseNoFrom"].Value = DBNull.Value;
                        row.Cells["CaseNoTo"].Value = DBNull.Value;
                        row.Cells["CaseCode"].Value = "";
                    }
                }
                finally
                {
                    _isUpdatingSupplyRow = false;
                }
            }
        }

        private static void ApplyStatusColor(DataGridViewRow row, int totalSupply, int allowed)
        {
            Color back, fore;

            if (totalSupply > allowed)
            {
                back = ColorOverSupplyBack;
                fore = ColorOverSupplyFore;
            }
            else if (totalSupply == 0)
            {
                back = Color.Empty;
                fore = Color.Empty;
            }
            else if (totalSupply == allowed)
            {
                back = ColorFullySuppliedBack;
                fore = ColorFullySuppliedFore;
            }
            else
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

        private void RefreshAllRowStatus()
        {
            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;
                ApplyStatusColor(row, ParseInt(row.Cells[ColSupplyTotalPieces].Value), GetAllowedQuantity(row));
            }
        }

        private void ResetSupplyEntry()
        {
            _isUpdatingSupplyRow = true;

            try
            {
                foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
                {
                    if (row.IsNewRow) continue;

                    row.Cells[ColSupplySheets].Value = 0;
                    row.Cells[ColSupplyPieces].Value = 0;
                    row.Cells[ColSupplyTotalPieces].Value = 0;

                    row.Cells["LedgerFolio"].Value = "";
                    row.Cells["CaseCode"].Value = "";
                    row.Cells["CaseNoFrom"].Value = "";
                    row.Cells["CaseNoTo"].Value = "";

                    int pending = GetAllowedQuantity(row);
                    row.Cells[ColRemainingPieces].Value = pending;
                    ApplyStatusColor(row, 0, pending);
                }

                _stockTable.Clear();
            }
            finally
            {
                _isUpdatingSupplyRow = false;
            }

            CalculateTotalCasePackingQty();
            _errors.Clear();
        }

        private int ComputeIndentStatusID()
        {
            // 6 = Partial, 7 = Closed. Closed only when nothing is left pending after this supply.
            int stillPending = 0;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int left = GetAllowedQuantity(row) - ParseInt(row.Cells[ColSupplyTotalPieces].Value);
                if (left > 0) stillPending += left;
            }

            return stillPending <= 0 ? 7 : 6;
        }

        // ---------------------------------------------------------------------
        // Supply grid events
        // ---------------------------------------------------------------------

        private void dgvSupplyDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isUpdatingSupplyRow)
                return;

            dgvSupplyDetail.Rows[e.RowIndex].ErrorText = string.Empty;

            string col = dgvSupplyDetail.Columns[e.ColumnIndex].Name;

            if (col != ColSupplySheets && col != ColSupplyPieces)
            {
                CheckDraftChanges();   // ledger folio / case fields
                return;
            }

            _isUpdatingSupplyRow = true;

            try
            {
                DataGridViewRow row = dgvSupplyDetail.Rows[e.RowIndex];

                int sheets = Math.Max(0, ParseInt(row.Cells[ColSupplySheets].Value));
                int pieces = Math.Max(0, ParseInt(row.Cells[ColSupplyPieces].Value));
                row.Cells[ColSupplySheets].Value = sheets;
                row.Cells[ColSupplyPieces].Value = pieces;

                int piecesPerSheet = ParseInt(row.Cells[ColPiecesPerSheet].Value);
                int totalSupply = (sheets * piecesPerSheet) + pieces;
                int allowed = GetAllowedQuantity(row);

                row.Cells[ColSupplyTotalPieces].Value = totalSupply;
                row.Cells[ColRemainingPieces].Value = Math.Max(allowed - totalSupply, 0);

                ApplyStatusColor(row, totalSupply, allowed);
                SyncStockEntryRow(row);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                _isUpdatingSupplyRow = false;
            }

            CalculateTotalCasePackingQty();
            CheckDraftChanges();
        }

        private void dgvSupplyDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string col = dgvSupplyDetail.Columns[e.ColumnIndex].Name;
            if (col != ColSupplySheets && col != ColSupplyPieces)
                return;

            string text = Convert.ToString(e.FormattedValue).Trim();
            if (text.Length == 0)
                return;

            int number;
            if (!int.TryParse(text, out number) || number < 0)
            {
                dgvSupplyDetail.Rows[e.RowIndex].ErrorText = "Enter a whole number (0 or more).";
                e.Cancel = true;   // press Esc to cancel the edit
            }
        }

        private void dgvSupplyDetail_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            string col = dgvSupplyDetail.Columns[e.ColumnIndex].Name;

            if (col == ColSupplySheets || col == ColSupplyPieces)
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(e.Value)))
                {
                    e.Value = 0;
                    e.ParsingApplied = true;
                }
            }
        }

        private void dgvSupplyDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string column = (e.ColumnIndex >= 0 && e.ColumnIndex < dgvSupplyDetail.Columns.Count)
                ? dgvSupplyDetail.Columns[e.ColumnIndex].HeaderText
                : "";

            MessageBox.Show("Invalid value in '" + column + "'. Please enter a valid whole number.",
                "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            e.ThrowException = false;
        }

        private void dgvSupplyDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if (tb == null) return;

            tb.KeyPress -= DigitsOnly_KeyPress;

            string col = dgvSupplyDetail.CurrentCell.OwningColumn.Name;

            if (col == ColSupplySheets || col == ColSupplyPieces)
            {
                tb.KeyPress += DigitsOnly_KeyPress;
                tb.BeginInvoke(new Action(tb.SelectAll));   // typing replaces the old value
            }
        }

        private void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void dgvSupplyDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvSupplyDetail.Columns[e.ColumnIndex].Name == "Denomination" &&
                e.Value != null && e.Value != DBNull.Value)
            {
                e.Value = FormatDenomination(e.Value);
                e.FormattingApplied = true;
            }
        }

        private void dgvSupplyDetail_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            // Programmatic updates are handled by the code that makes them
            if (e.RowIndex < 0 || _isLoadingDraft || _isUpdatingSupplyRow)
                return;

            if (dgvSupplyDetail.Columns[e.ColumnIndex].Name == ColSupplySheets)
                CalculateTotalCasePackingQty();
        }

        private void dgvSupplyDetail_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvSupplyDetail.CurrentCell != null)
                ShowStockForRow(dgvSupplyDetail.CurrentCell.RowIndex);
        }

        private void dgvSupplyDetail_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            ShowStockForRow(e.RowIndex);
        }

        // Wired in the Designer - nothing to do
        private void dgvSupplyDetail_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvSupplyDetail_KeyPress(object sender, KeyPressEventArgs e) { }

        // ---------------------------------------------------------------------
        // Enter moves to the next editable cell (right, then next row)
        // ---------------------------------------------------------------------

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                DataGridView grid = ActiveControl as DataGridView;

                if (grid != null && (grid == dgvSupplyDetail || grid == dgvStockEntry))
                {
                    MoveToNextEditableCell(grid);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static void MoveToNextEditableCell(DataGridView grid)
        {
            if (grid.CurrentCell == null) return;
            if (!grid.EndEdit()) return;

            int row = grid.CurrentCell.RowIndex;
            int col = grid.CurrentCell.ColumnIndex;
            int guard = grid.RowCount * grid.ColumnCount;

            for (int i = 0; i < guard; i++)
            {
                col++;
                if (col >= grid.ColumnCount)
                {
                    col = 0;
                    row++;
                }

                if (row >= grid.RowCount) return;

                DataGridViewCell cell = grid.Rows[row].Cells[col];

                if (cell.Visible && !cell.ReadOnly)
                {
                    grid.CurrentCell = cell;
                    grid.BeginEdit(true);
                    return;
                }
            }
        }

        // =====================================================================
        // Current stock cards
        // =====================================================================

        private void ShowStockForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvSupplyDetail.Rows.Count || rowIndex == _stockCardRow)
                return;

            _stockCardRow = rowIndex;

            try
            {
                DataGridViewRow row = dgvSupplyDetail.Rows[rowIndex];
                int categoryID = ParseInt(row.Cells["CategoryID"].Value);
                int denominationID = ParseInt(row.Cells["DenominationID"].Value);

                if (categoryID == 0 || denominationID == 0)
                {
                    ClearCurrentStockCards();
                    return;
                }

                lblStockItem.Text = Convert.ToString(row.Cells["Category"].Value) + "  |  " +
                                    FormatDenomination(row.Cells["Denomination"].Value);

                DataRow dr = stockDAL.GetStockBalance(categoryID, denominationID);

                if (dr == null)
                {
                    ClearCurrentStockCards();
                    return;
                }

                lblBoxBalance.Text = ToInt(dr["BoxQty"]).ToString("N0");
                lblPacketBalance.Text = ToInt(dr["PacketQty"]).ToString("N0");
                lblSheetBalance.Text = ToInt(dr["SheetQty"]).ToString("N0");
                lblStampBalance.Text = ToInt(dr["StampQty"]).ToString("N0");
            }
            catch (Exception)
            {
                ClearCurrentStockCards();
            }
        }

        private void ClearCurrentStockCards()
        {
            lblBoxBalance.Text = "0";
            lblPacketBalance.Text = "0";
            lblSheetBalance.Text = "0";
            lblStampBalance.Text = "0";
        }

        // =====================================================================
        // Stock entry grid
        // =====================================================================

        private DataRow FindStockRow(int categoryID, int denominationID)
        {
            foreach (DataRow r in _stockTable.Rows)
            {
                if (ToInt(r["CategoryID"]) == categoryID && ToInt(r["DenominationID"]) == denominationID)
                    return r;
            }

            return null;
        }

        /// <summary>
        /// Keeps the stock entry in step with ONE supply row only.
        /// Other rows (and anything typed manually for them) are left untouched.
        /// </summary>
        private void SyncStockEntryRow(DataGridViewRow row)
        {
            int categoryID = ParseInt(row.Cells["CategoryID"].Value);
            int denominationID = ParseInt(row.Cells["DenominationID"].Value);
            if (categoryID == 0 || denominationID == 0) return;

            int sheets = ParseInt(row.Cells[ColSupplySheets].Value);
            int pieces = ParseInt(row.Cells[ColSupplyPieces].Value);

            DataRow existing = FindStockRow(categoryID, denominationID);

            if (sheets <= 0 && pieces <= 0)
            {
                if (existing != null) _stockTable.Rows.Remove(existing);
                return;
            }

            if (existing == null)
            {
                existing = _stockTable.NewRow();
                existing["CategoryID"] = categoryID;
                existing["DenominationID"] = denominationID;
                existing["Category"] = Convert.ToString(row.Cells["Category"].Value);
                existing["Denomination"] = Convert.ToString(row.Cells["Denomination"].Value);
                _stockTable.Rows.Add(existing);
            }

            existing["BoxQty"] = 0;
            existing["PacketQty"] = 0;
            existing["SheetQty"] = sheets;
            existing["StampQty"] = pieces;
        }

        private void dgvStockEntry_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if (tb == null) return;

            tb.KeyPress -= DigitsOnly_KeyPress;

            string col = dgvStockEntry.Columns[dgvStockEntry.CurrentCell.ColumnIndex].Name;

            if (col == "StockCaseQty" || col == "StockPacketQty" || col == "StockSheetQty" || col == "StockStampQty")
            {
                tb.KeyPress += DigitsOnly_KeyPress;
                tb.BeginInvoke(new Action(tb.SelectAll));
            }
        }

        private void dgvStockEntry_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvStockEntry.IsCurrentCellDirty)
                dgvStockEntry.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvStockEntry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isLoadingDraft && e.RowIndex >= 0)
                CheckDraftChanges();
        }

        private void dgvStockEntry_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvStockEntry.Columns[e.ColumnIndex].Name == "StockDenomination" &&
                e.Value != null && e.Value != DBNull.Value)
            {
                e.Value = FormatDenomination(e.Value);
                e.FormattingApplied = true;
            }
        }

        private void dgvStockEntry_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private bool ValidateStockEntry()
        {
            bool hasCase = false, hasPacket = false, hasSheet = false, hasStamp = false;

            foreach (DataRow r in _stockTable.Rows)
            {
                if (ToInt(r["BoxQty"]) > 0) hasCase = true;
                if (ToInt(r["PacketQty"]) > 0) hasPacket = true;
                if (ToInt(r["SheetQty"]) > 0) hasSheet = true;
                if (ToInt(r["StampQty"]) > 0) hasStamp = true;
            }

            if (!(hasCase || hasPacket || hasSheet || hasStamp))
                return Fail("Please enter at least one quantity in the Stock Entry.", dgvStockEntry);

            // Case packing: any combination of units is allowed
            if (IsPacking("Case"))
                return true;

            if (IsPacking("Packet"))
            {
                if (!hasPacket)
                    return Fail("Packet packing is selected. Please enter Packet Qty in the Stock Entry.", dgvStockEntry);
                if (hasCase)
                    return Fail("Case quantity cannot be entered together with Packet packing.", dgvStockEntry);
                return true;
            }

            if (IsPacking("Sheets"))
            {
                if (!hasSheet)
                    return Fail("Sheets packing is selected. Please enter Sheet Qty in the Stock Entry.", dgvStockEntry);
                if (hasCase)
                    return Fail("Case quantity cannot be entered together with Sheets packing.", dgvStockEntry);
                return true;
            }

            if (IsPacking("Labels"))
            {
                if (!hasStamp)
                    return Fail("Labels packing is selected. Please enter Stamp Qty in the Stock Entry.", dgvStockEntry);
                if (hasCase)
                    return Fail("Case quantity cannot be entered together with Labels packing.", dgvStockEntry);
                return true;
            }

            // Bags / Parcel / Envelope / Other
            if (hasCase)
                return Fail("Case quantity can only be entered with Case packing.", dgvStockEntry);

            if (!hasPacket && !hasSheet && !hasStamp)
                return Fail("Please enter at least one Packet, Sheet or Stamp quantity in the Stock Entry.", dgvStockEntry);

            return true;
        }

        // =====================================================================
        // Header controls
        // =====================================================================

        private void cmbPackingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPackingType.SelectedIndex == -1)
                return;

            bool isCase = IsPacking("Case");
            bool isOther = IsPacking("Other");

            txtPackingQty.Visible = !isOther;
            txtOtherPacking.Visible = isOther;
            label14.Text = isOther ? "Other Type" : "Packing Qty";

            cmbDispatchMode.Enabled = !isCase;

            // Do not touch the saved values while a draft is being loaded
            if (!_isLoadingDraft)
            {
                if (isCase)
                    cmbDispatchMode.SelectedIndex = 2;                      // By RMS
                else if (cmbDispatchMode.SelectedIndex == -1 || _lastPackingWasCase)
                    cmbDispatchMode.SelectedIndex = 0;                      // By Post

                if (!isOther)
                    txtPackingQty.Text = "1";
            }

            _lastPackingWasCase = isCase;

            CalculateTotalCasePackingQty();
            ApplyCaseColumnState(isCase);
            UpdateGridLayout();
            CheckDraftChanges();
        }

        private void cmbDispatchMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDispatchMode.SelectedIndex == -1)
                return;

            UpdateGridLayout();
            CheckDraftChanges();
        }

        private void cmbSupplyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckDraftChanges();
        }

        private void txtPackingQty_TextChanged(object sender, EventArgs e)
        {
            _errors.SetError(txtPackingQty, "");
            CheckDraftChanges();
        }

        private void txtPackingQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
            CheckDraftChanges();
        }

        private void chkAddDate_CheckedChanged(object sender, EventArgs e)
        {
            addDate = chkAddDate.Checked;
        }

        private int GetSheetsPerCase(int categoryID, int denominationID)
        {
            string key = categoryID + "_" + denominationID;
            int value;

            if (!_sheetsPerCaseCache.TryGetValue(key, out value))
            {
                value = supplyDAL.GetSheetsPerCase(categoryID, denominationID);
                _sheetsPerCaseCache[key] = value;
            }

            return value;
        }

        private void CalculateTotalCasePackingQty()
        {
            if (!IsPacking("Case"))
            {
                txtPackingQty.ReadOnly = IsFormLocked();
                return;
            }

            txtPackingQty.ReadOnly = true;
            decimal totalCases = 0;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int categoryID = ParseInt(row.Cells["CategoryID"].Value);
                int denominationID = ParseInt(row.Cells["DenominationID"].Value);
                int sheets = ParseInt(row.Cells[ColSupplySheets].Value);

                if (categoryID > 0 && denominationID > 0 && sheets > 0)
                {
                    int sheetsPerCase = GetSheetsPerCase(categoryID, denominationID);

                    if (sheetsPerCase > 0)
                        totalCases += (decimal)sheets / sheetsPerCase;
                }
            }

            txtPackingQty.Text = totalCases.ToString("0.##");
        }

        private bool IsFormLocked()
        {
            return _isExisting && (_currentStatusID == StatusIssued || _currentStatusID == StatusCancelled);
        }

        // =====================================================================
        // Validation
        // =====================================================================

        private bool ValidateSupply(bool checkCase, bool askOverSupply)
        {
            _errors.Clear();

            if (cmbSupplyType.SelectedIndex < 0)
                return Fail("Please select Supply Type.", cmbSupplyType);

            if (string.IsNullOrWhiteSpace(cmbDispatchMode.Text))
                return Fail("Please select Dispatch Mode.", cmbDispatchMode);

            if (string.IsNullOrWhiteSpace(cmbPackingType.Text))
                return Fail("Please select Packing Type.", cmbPackingType);

            bool isOther = IsPacking("Other");
            bool isCase = IsPacking("Case");

            if (isOther && string.IsNullOrWhiteSpace(txtOtherPacking.Text))
                return Fail("Please specify the Other Packing Type.", txtOtherPacking);

            // ----- supply rows -----
            bool hasSupply = false;
            StringBuilder overItems = new StringBuilder();
            DataGridViewRow firstOverRow = null;

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int sheets = ParseInt(row.Cells[ColSupplySheets].Value);
                int pieces = ParseInt(row.Cells[ColSupplyPieces].Value);
                int piecesPerSheet = ParseInt(row.Cells[ColPiecesPerSheet].Value);
                int total = (sheets * piecesPerSheet) + pieces;

                if (total <= 0) continue;

                hasSupply = true;

                // More than the indent (pending) quantity is allowed, but only after confirmation (asked below)
                int allowed = GetAllowedQuantity(row);
                if (total > allowed)
                {
                    if (firstOverRow == null) firstOverRow = row;

                    overItems.AppendFormat("{0} {1}: supply {2:N0}, pending {3:N0}\n",
                        Convert.ToString(row.Cells["Category"].Value),
                        FormatDenomination(row.Cells["Denomination"].Value),
                        total,
                        allowed);
                }

                if (checkCase && isCase)
                {
                    foreach (string name in new[] { "CaseNoFrom", "CaseNoTo", "CaseCode" })
                    {
                        if (Str(row.Cells[name].Value).Length == 0)
                        {
                            dgvSupplyDetail.CurrentCell = row.Cells[name];
                            dgvSupplyDetail.BeginEdit(true);

                            return Fail("Please enter " + dgvSupplyDetail.Columns[name].HeaderText + ".", dgvSupplyDetail);
                        }
                    }
                }
            }

            if (!hasSupply)
                return Fail("Please enter Supply Quantity for at least one item.", dgvSupplyDetail);

            // ----- supply more than the indent quantity: warn and ask -----
            if (askOverSupply && overItems.Length > 0)
            {
                dgvSupplyDetail.CurrentCell = firstOverRow.Cells[ColSupplySheets];

                DialogResult overAnswer = MessageBox.Show(
                    "The supply quantity is MORE than the indent (pending) quantity for:\n\n" +
                    overItems +
                    "\nDo you want to continue anyway?",
                    "Quantity Exceeds Indent",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (overAnswer != DialogResult.Yes)
                {
                    dgvSupplyDetail.Focus();
                    return false;
                }
            }

            // ----- packing quantity -----
            if (!isOther)
            {
                decimal qty;

                if (!decimal.TryParse(txtPackingQty.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out qty) || qty < 0)
                    return Fail("Please enter a valid Packing Qty.", txtPackingQty);

                if (qty % 1 != 0)
                {
                    if (isCase)
                        return Fail("Case quantity " + qty.ToString("0.##") +
                                    " is not a whole number. Adjust the supply quantities so that complete cases are supplied.",
                                    dgvSupplyDetail);

                    return Fail("Packing Qty must be a whole number.", txtPackingQty);
                }
            }

            return true;
        }

        // =====================================================================
        // Building the data sent to the DAL
        // =====================================================================

        private List<SupplyLine> BuildSupplyLines()
        {
            List<SupplyLine> lines = new List<SupplyLine>();

            foreach (DataGridViewRow row in dgvSupplyDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int sheets = ParseInt(row.Cells[ColSupplySheets].Value);
                int pieces = ParseInt(row.Cells[ColSupplyPieces].Value);
                if (sheets <= 0 && pieces <= 0) continue;

                int piecesPerSheet = ParseInt(row.Cells[ColPiecesPerSheet].Value);

                lines.Add(new SupplyLine
                {
                    IndentDetailID = ParseInt(row.Cells["DetailID"].Value),
                    CategoryID = ParseInt(row.Cells["CategoryID"].Value),
                    DenominationID = ParseInt(row.Cells["DenominationID"].Value),
                    SupplySheets = sheets,
                    PiecesPerSheet = piecesPerSheet,
                    LoosePieces = pieces,
                    TotalPieces = (sheets * piecesPerSheet) + pieces,
                    LedgerFolio = Str(row.Cells["LedgerFolio"].Value),
                    CaseNoFrom = Str(row.Cells["CaseNoFrom"].Value),
                    CaseNoTo = Str(row.Cells["CaseNoTo"].Value),
                    CaseCode = Str(row.Cells["CaseCode"].Value)
                });
            }

            return lines;
        }

        private List<StockLine> BuildStockLines()
        {
            List<StockLine> lines = new List<StockLine>();

            foreach (DataRow r in _stockTable.Rows)
            {
                StockLine line = new StockLine
                {
                    CategoryID = ToInt(r["CategoryID"]),
                    DenominationID = ToInt(r["DenominationID"]),
                    BoxQty = ToInt(r["BoxQty"]),
                    PacketQty = ToInt(r["PacketQty"]),
                    SheetQty = ToInt(r["SheetQty"]),
                    StampQty = ToInt(r["StampQty"])
                };

                if (line.CategoryID == 0 || line.DenominationID == 0) continue;

                if (line.BoxQty == 0 && line.PacketQty == 0 && line.SheetQty == 0 && line.StampQty == 0)
                    continue;

                lines.Add(line);
            }

            return lines;
        }

        // =====================================================================
        // Buttons
        // =====================================================================

        private void btnSaveDraft_Click(object sender, EventArgs e)
        {
            if (!ValidateSupply(false, true) || !ValidateStockEntry())
                return;

            try
            {
                List<SupplyLine> lines = BuildSupplyLines();
                List<StockLine> stock = BuildStockLines();

                int indentStatusID = ComputeIndentStatusID();
                int packingQty = GetPackingQtyValue();
                int supplyTypeID = GetSupplyTypeID();

                if (_supplyID == 0)
                {
                    // ----- NEW supply (one transaction) -----
                    _supplyID = supplyDAL.CreateSupplyDraft(
                        _supplyInfo, _indentID, supplyTypeID,
                        cmbDispatchMode.Text, cmbPackingType.Text, GetOtherPackingTypeValue(), packingQty,
                        txtRemarks.Text, indentStatusID, dtSupplyDate.Value,
                        lines, stock);

                    Info("Draft saved successfully.");
                }
                else
                {
                    // ----- EXISTING supply (one transaction) -----
                    CheckDraftChanges();

                    if (!_isDataChanged)
                    {
                        Info("No changes found.");
                        return;
                    }

                    supplyDAL.UpdateSupplyDraft(
                        _supplyID, supplyTypeID, dtSupplyDate.Value,
                        cmbDispatchMode.Text, cmbPackingType.Text, GetOtherPackingTypeValue(), packingQty,
                        txtInvoiceNo.Text, txtRemarks.Text, indentStatusID,
                        lines, stock);

                    if (_currentStatusID == StatusApproved)
                        supplyDAL.UpdateIndentStatusAfterSupply(_supplyID, _indentID);

                    Info("Changes saved successfully.");
                }

                CloseWithResult(DialogResult.OK);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (!ValidateSupply(true, false))
                return;

            try
            {
                if (_supplyID == 0)
                {
                    Warn("Please save the draft first.");
                    return;
                }

                if (_isDataChanged)
                {
                    Warn("Please save your changes before approving.");
                    return;
                }

                // The saved quantities, together with other approved / issued supplies, must stay within the indent
                DataTable over = supplyDAL.GetOverSuppliedItems(_supplyID);
                if (over.Rows.Count > 0)
                {
                    DialogResult overAnswer = MessageBox.Show(
                        DescribeOverSupply(over),
                        "Quantity Exceeds Indent",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);

                    if (overAnswer != DialogResult.Yes)
                        return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Do you want to approve this draft?\n\nOnce approved, the Supply Number is finalized and the draft cannot be edited.",
                    "Approve Supply", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                int openedSupplyID = _supplyID;

                // ----- current supply number -----
                SupplyNumberInfo currentInfo = supplyDAL.GetSupplyNumberInfo(openedSupplyID);

                if (currentInfo == null)
                {
                    Warn("Current Supply Number information could not be found.");
                    CloseWithResult(DialogResult.Cancel);
                    return;
                }

                string financialYear = currentInfo.FinancialYear;

                var office = supplyDAL.GetOfficeInfo(_indentID);

                if (office.OfficeID <= 0)
                {
                    Warn("Office information could not be found.");
                    CloseWithResult(DialogResult.Cancel);
                    return;
                }

                // ----- sequence check: the draft must hold the first free numbers -----
                int requiredGlobalSequence = supplyDAL.GetNextAvailableGlobalSequence(openedSupplyID, financialYear);
                int requiredOfficeSequence = supplyDAL.GetNextAvailableOfficeSequence(openedSupplyID, office.OfficeID, financialYear);

                if (currentInfo.GlobalSequence != requiredGlobalSequence ||
                    currentInfo.OfficeSequence != requiredOfficeSequence)
                {
                    bool updated = supplyDAL.UpdateDraftSupplyNumber(
                        openedSupplyID, requiredGlobalSequence, requiredOfficeSequence,
                        office.OfficeCode, financialYear);

                    if (!updated)
                    {
                        MessageBox.Show("Unable to correct the Supply Number.", "Supply",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CloseWithResult(DialogResult.Cancel);
                        return;
                    }

                    Info("The Supply Number has been corrected according to the current sequence.\n\nPlease reopen this draft and approve it.");
                    CloseWithResult(DialogResult.Cancel);
                    return;
                }

                // ----- drafts must be approved in order -----
                if (!supplyDAL.CanApproveDraftSequentially(openedSupplyID))
                {
                    Warn("This supply cannot be approved yet because an earlier draft supply is still pending approval.\n\nPlease approve the draft supplies in sequence.");
                    CloseWithResult(DialogResult.Cancel);
                    return;
                }

                // ----- final number, invoice, approval -----
                SupplyNumberGenerator generator = new SupplyNumberGenerator();
                SupplyNumberInfo info = generator.GenerateApprovedSupplyNumber(_indentID);

                txtInvoiceNo.Text = supplyDAL.EnsureValidInvoice(openedSupplyID, false);

                supplyDAL.ApproveSupply(
                    openedSupplyID, info, GetSupplyTypeID(),
                    cmbDispatchMode.Text, cmbPackingType.Text, GetOtherPackingTypeValue(),
                    GetPackingQtyValue(), txtRemarks.Text);

                supplyDAL.UpdateIndentStatusAfterSupply(openedSupplyID, _indentID);

                txtSupplyNo.Text = info.SupplyNo;
                txtFinancialYear.Text = info.FinancialYear;
                _currentStatusID = StatusApproved;
                txtSupplyStatus.Text = "Approved";
                UITheme.StyleStatusBox(txtSupplyStatus, _currentStatusID);

                Info("Supply approved successfully.");
                CloseWithResult(DialogResult.OK);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private static string DescribeOverSupply(DataTable over)
        {
            StringBuilder sb = new StringBuilder(
                "Together with the other approved / issued supplies, the following items exceed the indent quantity:\n\n");

            foreach (DataRow r in over.Rows)
            {
                sb.AppendFormat("{0} {1}: indent {2:N0}, already supplied {3:N0}, this supply {4:N0}\n",
                    Convert.ToString(r["CategoryName"]),
                    FormatDenomination(r["Denomination"]),
                    ToInt(r["IndentTotal"]),
                    ToInt(r["AlreadySupplied"]),
                    ToInt(r["ThisSupply"]));
            }

            sb.Append("\nDo you want to approve this supply anyway?");

            return sb.ToString();
        }

        private void btnAssignInvoiceNo_Click(object sender, EventArgs e)
        {
            if (!supplyDAL.CanAssignInvoiceSequentially(_supplyID))
            {
                Warn("The invoice of the previous draft supply must be assigned first.");
                return;
            }

            try
            {
                txtInvoiceNo.Text = supplyDAL.EnsureValidInvoice(_supplyID, true);

                Info(supplyDAL.InvoiceRegenerated
                    ? "Invoice Number was invalid and has been reassigned."
                    : "Invoice Number is already valid.");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (_supplyID == 0)
            {
                Warn("Supply record not found.");
                return;
            }

            if (_currentStatusID != StatusApproved)
            {
                Warn("Only an Approved supply can be issued.");
                return;
            }

            if (_isDataChanged)
            {
                Warn("Please save your changes before issuing.");
                return;
            }

            if (!ValidateSupply(true, false))
                return;

            DialogResult confirm = MessageBox.Show(
                "Do you want to issue this supply?\n\nStock will be deducted and the supply cannot be edited afterwards.",
                "Issue Supply", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                // Status change + stock deduction happen in ONE transaction.
                // If the stock is not enough nothing is changed.
                supplyDAL.IssueSupplyWithStock(_supplyID);

                _currentStatusID = StatusIssued;
                txtSupplyStatus.Text = "Issued";
                UITheme.StyleStatusBox(txtSupplyStatus, _currentStatusID);

                Info("Supply issued successfully.");
                CloseWithResult(DialogResult.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Issue Supply", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!HasUnsavedChanges())
            {
                ResetSupplyEntry();
                return;
            }

            DialogResult answer = MessageBox.Show(
                "Clear all entered supply quantities?",
                "Supply", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
                ResetSupplyEntry();
        }

        private void btnSupplyPerforma_Click(object sender, EventArgs e)
        {
            if (_supplyID == 0)
            {
                Warn("Please save the draft first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
            {
                Warn("Please assign the Invoice Number first.");
                return;
            }

            frmReportPreview frm = new frmReportPreview(_supplyID, addDate);
            frm.ShowDialog();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}