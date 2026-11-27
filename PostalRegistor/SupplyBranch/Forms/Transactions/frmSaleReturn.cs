using SupplyBranch.Controls;
using SupplyBranch.DAL;
using SupplyBranch.DataAccess;
using SupplyBranch.Forms.Reports;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Transactions
{
    public partial class frmSaleReturn : Form
    {
        // =====================================================================
        // Constants
        // =====================================================================


        private const string ColIssuedSheets = "IssuedSheets";
        private const string ColIssuedPieces = "IssuedTotalPieces";
        private const string ColAlreadyReturned = "AlreadyReturnedPieces";
        private const string ColMaxReturnable = "MaxReturnablePieces";
        private const string ColPiecesPerSheet = "PiecesPerSheet";
        private const string ColReturnSheets = "ReturnSheets";
        private const string ColReturnPieces = "ReturnLoosePieces";
        private const string ColReturnTotal = "ReturnTotalPieces";
        private const string ColBalanceReturn = "RemainingReturnablePieces";
        private const string ReportFileName = "rptSaleRetun.rdlc";

        private const int StatusDraft = 1;
        private const int StatusPosted = 3;

        private static readonly Color ColorExceedBack = Color.MistyRose;
        private static readonly Color ColorExceedFore = Color.DarkRed;
        private static readonly Color ColorFullReturnBack = Color.PaleGreen;
        private static readonly Color ColorFullReturnFore = Color.DarkGreen;
        private static readonly Color ColorPartialReturnBack = Color.LightYellow;
        private static readonly Color ColorPartialReturnFore = Color.DarkGoldenrod;

        private static Color EditCellBack { get { return UITheme.EditableCellColor; } }

        // =====================================================================
        // Fields
        // =====================================================================
        private bool _isConfiguringGrid = false;
        private readonly SupplyDAL _supplyDal = new SupplyDAL();
        private readonly StockDAL _stockDal = new StockDAL();
        private readonly SaleReturnDAL _returnDal = new SaleReturnDAL();
        private readonly ErrorProvider _errors = new ErrorProvider();

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

        private DataTable _entryDetailTable;
        private DataTable _returnListTable;
        private bool _isListMode = false;

        private int _returnID = 0;
        private int _selectedSupplyID = 0;
        private int _currentStatusID = StatusDraft;
        private int _stockCardRow = -1;
        private bool _isLoadingData = false;
        private bool _isUpdatingRow = false;
        private bool _isSyncingGrids = false;

        // =====================================================================
        // Constructors
        // =====================================================================

        public frmSaleReturn()
        {
            InitializeComponent();
            InitCommon();
        }

        public frmSaleReturn(int returnID)
        {
            InitializeComponent();
            InitCommon();
            _returnID = returnID;
        }

        // =====================================================================
        // Initialization
        // =====================================================================

        private void InitCommon()
        {
            _errors.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            BuildStockEntryGrid();
            InitializeReturnListTable();

            foreach (TextBox tb in new[]
            {
                txtReturnNo, txtFinancialYear, txtReturnStatus,
                txtOfficeName, txtInvoiceNo, txtSupplyDate
            })
            {
                tb.ReadOnly = true;
                tb.TabStop = false;
            }

            dtReturnDate.ValueChanged += dtReturnDate_ValueChanged;
            AppTheme.ThemeChanged += HandleThemeChanged;
            Disposed += (s, e) => AppTheme.ThemeChanged -= HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            RefreshGridTheme();
            StyleModeButtons();
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
            dgvStockEntry.BackgroundColor = AppTheme.P.Background;
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

            foreach (DataGridViewColumn col in dgvStockEntry.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            pnlStockEntry.Controls.Add(dgvStockEntry);
            ((System.ComponentModel.ISupportInitialize)dgvStockEntry).EndInit();

            _stockTable = CreateStockTable();
            dgvStockEntry.DataSource = _stockTable;

            // Stock IN lines are derived from the return lines, so this grid is read-only.
            dgvStockEntry.ReadOnly = true;
            dgvStockEntry.TabStop = false;
            dgvStockEntry.DataError += (s, e) => e.ThrowException = false;
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

        private void InitializeReturnListTable()
        {
            _returnListTable = new DataTable();
            _returnListTable.Columns.Add("ReturnID", typeof(int));
            _returnListTable.Columns.Add("ReturnNo", typeof(string));
            _returnListTable.Columns.Add("ReturnDate", typeof(DateTime));
            _returnListTable.Columns.Add("SupplyNo", typeof(string));
            _returnListTable.Columns.Add("InvoiceNo", typeof(string));
            _returnListTable.Columns.Add("OfficeName", typeof(string));
            _returnListTable.Columns.Add("TotalSheets", typeof(int));
            _returnListTable.Columns.Add("TotalPieces", typeof(int));
            _returnListTable.Columns.Add("StatusName", typeof(string));
            _returnListTable.Columns.Add("Remarks", typeof(string));
        }

        // =====================================================================
        // Load & Mode Toggle (Entry vs List)
        // =====================================================================

        private void frmSaleReturn_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            RefreshGridTheme();
            ConfigureEntryGridColumns();

            _isLoadingData = true;
            try
            {
                dtReturnDate.Value = DateTime.Today;
                dtFilterFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                dtFilterTo.Value = DateTime.Today;

                cmbFilterStatus.Items.Clear();
                cmbFilterStatus.Items.AddRange(new object[] { "All", "Draft", "Posted" });
                cmbFilterStatus.SelectedIndex = 0;

                RefreshFinancialYearAndPreviewNo();
                SetStatus(StatusDraft, "Draft");

                LoadIssuedSuppliesCombo();

                if (_returnID > 0)
                    LoadExistingReturn(_returnID);
            }
            finally
            {
                _isLoadingData = false;
            }

            SwitchToMode(isListMode: false);
            ApplyButtonState();
        }

        private void dtReturnDate_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoadingData || _returnID > 0) return;
            RefreshFinancialYearAndPreviewNo();
        }

        private void RefreshFinancialYearAndPreviewNo()
        {
            string fy = SupplyDAL.GetFinancialYearsRange(dtReturnDate.Value).CurrentFY;
            txtFinancialYear.Text = fy;

            try
            {
                var next = _returnDal.GetNextReturnNumber(fy);
                txtReturnNo.Text = next.ReturnNo;
            }
            catch
            {
                txtReturnNo.Text = "SR0001/" + fy;
            }
        }

        private void btnModeEntry_Click(object sender, EventArgs e)
        {
            SwitchToMode(isListMode: false);
        }

        private void btnModeList_Click(object sender, EventArgs e)
        {
            SwitchToMode(isListMode: true);
            LoadReturnListData();
        }

        private void SwitchToMode(bool isListMode)
        {
            _isListMode = isListMode;

            // Entry-only controls
            labelReturnNo.Visible = !isListMode;
            txtReturnNo.Visible = !isListMode;
            labelReturnDate.Visible = !isListMode;
            dtReturnDate.Visible = !isListMode;
            labelFY.Visible = !isListMode;
            txtFinancialYear.Visible = !isListMode;
            labelStatus.Visible = !isListMode;
            txtReturnStatus.Visible = !isListMode;

            labelIssuedSupply.Visible = !isListMode;
            cmbIssuedSupply.Visible = !isListMode;
            labelOffice.Visible = !isListMode;
            txtOfficeName.Visible = !isListMode;
            labelInvoiceNo.Visible = !isListMode;
            txtInvoiceNo.Visible = !isListMode;
            labelSupplyDate.Visible = !isListMode;
            txtSupplyDate.Visible = !isListMode;

            labelRemarks.Visible = !isListMode;
            txtRemarks.Visible = !isListMode;

            pnlStockEntry.Visible = !isListMode;
            pnlCurrentStock.Visible = !isListMode;

            btnSaveDraft.Visible = !isListMode;
            btnPostReturn.Visible = !isListMode;
            btnRefresh.Visible = !isListMode;

            // List-only controls
            labelSearchReturnNo.Visible = isListMode;
            txtSearchReturnNo.Visible = isListMode;
            labelFilterStatus.Visible = isListMode;
            cmbFilterStatus.Visible = isListMode;
            labelFilterFrom.Visible = isListMode;
            dtFilterFrom.Visible = isListMode;
            labelFilterTo.Visible = isListMode;
            dtFilterTo.Visible = isListMode;

            btnSearchReturns.Visible = isListMode;
            btnOpenReturn.Visible = isListMode;
            btnPrintReturn.Visible = isListMode;

            if (isListMode)
            {
                ConfigureListGridColumns();
                dgvReturnDetail.ReadOnly = true;
                dgvReturnDetail.DataSource = _returnListTable;
            }
            else
            {
                ConfigureEntryGridColumns();
                dgvReturnDetail.ReadOnly = (_currentStatusID == StatusPosted);
                dgvReturnDetail.DataSource = _entryDetailTable;
                RefreshAllRowStatus();
                ApplyButtonState();
            }

            StyleModeButtons();
        }

        private void StyleModeButtons()
        {
            Color activeBack = Color.FromArgb(31, 78, 121);
            Color activeFore = Color.White;
            Color inactiveBack = AppTheme.P.SurfaceAlt;
            Color inactiveFore = AppTheme.P.TextPrimary;

            btnModeEntry.BackColor = !_isListMode ? activeBack : inactiveBack;
            btnModeEntry.ForeColor = !_isListMode ? activeFore : inactiveFore;

            btnModeList.BackColor = _isListMode ? activeBack : inactiveBack;
            btnModeList.ForeColor = _isListMode ? activeFore : inactiveFore;
        }

        private void ConfigureEntryGridColumns()
        {
            _isConfiguringGrid = true;
            try
            {
                dgvReturnDetail.AutoGenerateColumns = false;
                dgvReturnDetail.Columns.Clear();

                AddReadOnlyCol("SupplyDetailID", "SupplyDetailID", "SupplyDetailID", 80, false);
                AddReadOnlyCol("CategoryID", "CategoryID", "CategoryID", 80, false);
                AddReadOnlyCol("DenominationID", "DenominationID", "DenominationID", 80, false);
                AddReadOnlyCol("CategoryName", "Category", "CategoryName", 150, true);
                AddReadOnlyCol("Denomination", "Denomination", "Denomination", 105, true);
                AddReadOnlyCol(ColPiecesPerSheet, "Pieces / Sheet", ColPiecesPerSheet, 100, true, true);
                AddReadOnlyCol(ColIssuedSheets, "Issued Sheets", ColIssuedSheets, 105, true, true);
                AddReadOnlyCol(ColIssuedPieces, "Issued Pieces", ColIssuedPieces, 110, true, true);
                AddReadOnlyCol(ColAlreadyReturned, "Already Returned", ColAlreadyReturned, 120, true, true);
                AddReadOnlyCol(ColMaxReturnable, "Returnable Balance", ColMaxReturnable, 130, true, true);

                DataGridViewTextBoxColumn colRetSheets = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = ColReturnSheets,
                    HeaderText = "Return Sheets",
                    Name = ColReturnSheets,
                    Width = 105,
                    ReadOnly = false,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleRight,
                        BackColor = EditCellBack
                    }
                };

                DataGridViewTextBoxColumn colRetPieces = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = ColReturnPieces,
                    HeaderText = "Return Pieces",
                    Name = ColReturnPieces,
                    Width = 105,
                    ReadOnly = false,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleRight,
                        BackColor = EditCellBack
                    }
                };

                dgvReturnDetail.Columns.AddRange(new DataGridViewColumn[] { colRetSheets, colRetPieces });

                AddReadOnlyCol(ColReturnTotal, "Total Return Pieces", ColReturnTotal, 130, true, true);
                AddReadOnlyCol(ColBalanceReturn, "Left After Return", ColBalanceReturn, 125, true, true);

                DataGridViewTextBoxColumn colRemarks = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ReturnRemarks",
                    HeaderText = "Reason / Remarks",
                    Name = "ReturnRemarks",
                    Width = 180,
                    ReadOnly = false
                };
                dgvReturnDetail.Columns.Add(colRemarks);

                foreach (DataGridViewColumn col in dgvReturnDetail.Columns)
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;

                // Bound grid par pehli hidden column khud visible ho jati hai, is liye
                // hide karne ka kaam columns add hone ke baad karte hain.
                SetColumnVisible("SupplyDetailID", false);
                SetColumnVisible("CategoryID", false);
                SetColumnVisible("DenominationID", false);
            }
            finally
            {
                _isConfiguringGrid = false;
            }

            if (!_isListMode && dgvReturnDetail.CurrentCell != null)
            {
                _stockCardRow = -1;
                ShowStockForRow(dgvReturnDetail.CurrentCell.RowIndex);
            }
        }

        private void ConfigureListGridColumns()
        {
            dgvReturnDetail.AutoGenerateColumns = false;
            dgvReturnDetail.Columns.Clear();

            AddReadOnlyCol("ReturnID", "ReturnID", "ReturnID", 70, false);
            AddReadOnlyCol("ReturnNo", "Return No", "ReturnNo", 130, true);
            AddReadOnlyCol("ReturnDate", "Return Date", "ReturnDate", 105, true);
            AddReadOnlyCol("SupplyNo", "Supply No", "SupplyNo", 155, true);
            AddReadOnlyCol("InvoiceNo", "Invoice No.", "InvoiceNo", 165, true);
            AddReadOnlyCol("OfficeName", "Office", "OfficeName", 170, true);
            AddReadOnlyCol("TotalSheets", "Total Sheets", "TotalSheets", 105, true, true);
            AddReadOnlyCol("TotalPieces", "Total Pieces", "TotalPieces", 115, true, true);
            AddReadOnlyCol("StatusName", "Status", "StatusName", 95, true);
            AddReadOnlyCol("Remarks", "Remarks", "Remarks", 210, true);

            foreach (DataGridViewColumn col in dgvReturnDetail.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            SetColumnVisible("ReturnID", false);
        }

        private void SetColumnVisible(string columnName, bool visible)
        {
            if (dgvReturnDetail.Columns.Contains(columnName))
                dgvReturnDetail.Columns[columnName].Visible = visible;
        }

        private void AddReadOnlyCol(string name, string headerText, string dataProperty, int width, bool visible, bool rightAlign = false)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = dataProperty,
                Width = width,
                Visible = visible,
                ReadOnly = true
            };

            if (rightAlign)
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvReturnDetail.Columns.Add(col);
        }

        // =====================================================================
        // Entry Data Loading
        // =====================================================================

        private void LoadIssuedSuppliesCombo()
        {
            DataTable dt = _supplyDal.GetSuppliesByStatusIDs(new[] { 3 });

            cmbIssuedSupply.DisplayMember = "SupplyNo";
            cmbIssuedSupply.ValueMember = "SupplyID";
            cmbIssuedSupply.DataSource = dt;
            cmbIssuedSupply.SelectedIndex = -1;
        }

        private void cmbIssuedSupply_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingData || _isListMode || cmbIssuedSupply.SelectedValue == null)
                return;

            int supplyID;
            if (!int.TryParse(cmbIssuedSupply.SelectedValue.ToString(), out supplyID) || supplyID <= 0)
                return;

            LoadIssuedSupplyItems(supplyID);
        }

        private void LoadIssuedSupplyItems(int supplyID)
        {
            _selectedSupplyID = supplyID;
            _stockCardRow = -1;
            _stockTable.Clear();
            txtOfficeName.Text = string.Empty;
            txtInvoiceNo.Text = string.Empty;
            txtSupplyDate.Text = string.Empty;

            DataTable header = _supplyDal.GetSupplyHeader(supplyID);
            if (header != null && header.Rows.Count > 0)
            {
                DataRow h = header.Rows[0];
                txtOfficeName.Text = Convert.ToString(h["OfficeName"]);
                if (header.Columns.Contains("SupplyDate") && h["SupplyDate"] != DBNull.Value)
                    txtSupplyDate.Text = Convert.ToDateTime(h["SupplyDate"]).ToString("dd-MM-yyyy");
            }

            DataTable draftHeader = _supplyDal.GetDraftHeader(supplyID);
            if (draftHeader != null && draftHeader.Rows.Count > 0)
            {
                DataRow dh = draftHeader.Rows[0];
                if (draftHeader.Columns.Contains("InvoiceNo"))
                    txtInvoiceNo.Text = Convert.ToString(dh["InvoiceNo"]);
                if (string.IsNullOrWhiteSpace(txtSupplyDate.Text) &&
                    draftHeader.Columns.Contains("SupplyDate") && dh["SupplyDate"] != DBNull.Value)
                {
                    txtSupplyDate.Text = Convert.ToDateTime(dh["SupplyDate"]).ToString("dd-MM-yyyy");
                }
            }

            DataTable details = _supplyDal.GetSupplyDetails(supplyID);
            _entryDetailTable = BuildReturnableTable(details, _returnID);
            dgvReturnDetail.DataSource = _entryDetailTable;

            RefreshAllRowStatus();
            RebuildStockFromDetails();
            if (dgvReturnDetail.Rows.Count > 0)
                ShowStockForRow(0);
        }

        private DataTable BuildReturnableTable(DataTable supplyDetails, int excludeReturnID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SupplyDetailID", typeof(int));
            dt.Columns.Add("CategoryID", typeof(int));
            dt.Columns.Add("DenominationID", typeof(int));
            dt.Columns.Add("CategoryName", typeof(string));
            dt.Columns.Add("Denomination", typeof(object));
            dt.Columns.Add("PiecesPerSheet", typeof(int));
            dt.Columns.Add(ColIssuedSheets, typeof(int));
            dt.Columns.Add(ColIssuedPieces, typeof(int));
            dt.Columns.Add(ColAlreadyReturned, typeof(int));
            dt.Columns.Add(ColMaxReturnable, typeof(int));
            dt.Columns.Add(ColReturnSheets, typeof(int));
            dt.Columns.Add(ColReturnPieces, typeof(int));
            dt.Columns.Add(ColReturnTotal, typeof(int));
            dt.Columns.Add(ColBalanceReturn, typeof(int));
            dt.Columns.Add("ReturnRemarks", typeof(string));

            if (supplyDetails == null) return dt;

            foreach (DataRow src in supplyDetails.Rows)
            {
                // GetSupplyDetails also returns DetailID (= IndentDetailID) for the other
                // screens. SaleReturnDetail must store the real SupplyDetailID, otherwise
                // the returnable balance and the posting check cannot find the supply line.
                int detailID = supplyDetails.Columns.Contains("SupplyDetailID")
                    ? ToInt(src["SupplyDetailID"])
                    : ToInt(src["DetailID"]);
                int issuedSheets = supplyDetails.Columns.Contains("SupplySheets") ? ToInt(src["SupplySheets"]) : 0;
                int issuedTotal = ToInt(src["SupplyTotalPieces"]);
                int alreadyReturned = _returnDal.GetAlreadyReturnedPieces(detailID, excludeReturnID);
                int maxReturnable = Math.Max(0, issuedTotal - alreadyReturned);

                DataRow r = dt.NewRow();
                r["SupplyDetailID"] = detailID;
                r["CategoryID"] = ToInt(src["CategoryID"]);
                r["DenominationID"] = ToInt(src["DenominationID"]);
                r["CategoryName"] = Convert.ToString(src["CategoryName"]);
                r["Denomination"] = src["Denomination"];
                r["PiecesPerSheet"] = ToInt(src["PiecesPerSheet"]);
                r[ColIssuedSheets] = issuedSheets;
                r[ColIssuedPieces] = issuedTotal;
                r[ColAlreadyReturned] = alreadyReturned;
                r[ColMaxReturnable] = maxReturnable;
                r[ColReturnSheets] = 0;
                r[ColReturnPieces] = 0;
                r[ColReturnTotal] = 0;
                r[ColBalanceReturn] = maxReturnable;
                r["ReturnRemarks"] = string.Empty;
                dt.Rows.Add(r);
            }

            return dt;
        }

        private void LoadExistingReturn(int returnID)
        {
            DataTable dtHeader = _returnDal.GetSaleReturnHeader(returnID);
            if (dtHeader == null || dtHeader.Rows.Count == 0) return;

            DataRow h = dtHeader.Rows[0];
            _returnID = ToInt(h["ReturnID"]);
            _selectedSupplyID = ToInt(h["SupplyID"]);
            txtReturnNo.Text = Convert.ToString(h["ReturnNo"]);
            txtFinancialYear.Text = Convert.ToString(h["FinancialYear"]);
            dtReturnDate.Value = Convert.ToDateTime(h["ReturnDate"]);
            txtRemarks.Text = Convert.ToString(h["Remarks"]);
            txtOfficeName.Text = Convert.ToString(h["OfficeName"]);
            txtInvoiceNo.Text = Convert.ToString(h["InvoiceNo"]);
            if (h["SupplyDate"] != DBNull.Value)
                txtSupplyDate.Text = Convert.ToDateTime(h["SupplyDate"]).ToString("dd-MM-yyyy");

            cmbIssuedSupply.SelectedValue = _selectedSupplyID;

            DataTable supplyDetails = _supplyDal.GetSupplyDetails(_selectedSupplyID);
            _entryDetailTable = BuildReturnableTable(supplyDetails, _returnID);

            DataTable savedDetails = _returnDal.GetSaleReturnDetails(_returnID);
            foreach (DataRow saved in savedDetails.Rows)
            {
                int sdID = ToInt(saved["SupplyDetailID"]);
                foreach (DataRow row in _entryDetailTable.Rows)
                {
                    if (ToInt(row["SupplyDetailID"]) == sdID)
                    {
                        int retSheets = ToInt(saved["ReturnSheets"]);
                        int retPieces = ToInt(saved["ReturnLoosePieces"]);
                        int retTotal = ToInt(saved["ReturnTotalPieces"]);
                        int maxRet = ToInt(row[ColMaxReturnable]);

                        row[ColReturnSheets] = retSheets;
                        row[ColReturnPieces] = retPieces;
                        row[ColReturnTotal] = retTotal;
                        row[ColBalanceReturn] = Math.Max(0, maxRet - retTotal);
                        row["ReturnRemarks"] = Convert.ToString(saved["ReturnRemarks"]);
                        break;
                    }
                }
            }

            dgvReturnDetail.DataSource = _entryDetailTable;

            int statusID = ToInt(h["StatusID"]);

            if (statusID == StatusPosted)
            {
                // Posted return: stock me jo likha gaya tha wohi dikhao.
                _stockTable.Clear();
                DataTable savedStock = _returnDal.GetSaleReturnStock(_returnID);
                foreach (DataRow s in savedStock.Rows)
                {
                    DataRow sr = _stockTable.NewRow();
                    sr["CategoryID"] = ToInt(s["CategoryID"]);
                    sr["DenominationID"] = ToInt(s["DenominationID"]);
                    sr["Category"] = Convert.ToString(s["Category"]);
                    sr["Denomination"] = FormatDenomText(s["Denomination"]);
                    sr["BoxQty"] = ToInt(s["BoxQty"]);
                    sr["PacketQty"] = ToInt(s["PacketQty"]);
                    sr["SheetQty"] = ToInt(s["SheetQty"]);
                    sr["StampQty"] = ToInt(s["StampQty"]);
                    _stockTable.Rows.Add(sr);
                }
            }
            else
            {
                // Draft: stock IN lines hamesha return lines se derive hoti hain.
                RebuildStockFromDetails();
            }

            SetStatus(statusID, Convert.ToString(h["StatusName"]));
            if (statusID == StatusPosted)
                LockForm();

            RefreshAllRowStatus();
            if (dgvReturnDetail.Rows.Count > 0)
                ShowStockForRow(0);
        }

        // =====================================================================
        // Grid Calculations, Status Colors & Manual Sync Between Grids
        // =====================================================================

        private void dgvReturnDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_isListMode || e.RowIndex < 0 || _isUpdatingRow || _isSyncingGrids)
                return;

            string colName = dgvReturnDetail.Columns[e.ColumnIndex].Name;
            if (colName != ColReturnSheets && colName != ColReturnPieces)
                return;

            _isUpdatingRow = true;
            try
            {
                DataGridViewRow row = dgvReturnDetail.Rows[e.RowIndex];
                int sheets = Math.Max(0, ParseInt(row.Cells[ColReturnSheets].Value));
                int pieces = Math.Max(0, ParseInt(row.Cells[ColReturnPieces].Value));
                int perSheet = Math.Max(0, ParseInt(row.Cells[ColPiecesPerSheet].Value));
                int maxAllowed = Math.Max(0, ParseInt(row.Cells[ColMaxReturnable].Value));

                // Loose pieces ko sheets me carry karo, taake breakdown sahi rahe
                // (misal: 150 pieces + 100 per sheet => 1 sheet + 50 pieces).
                if (perSheet > 0 && pieces >= perSheet)
                {
                    sheets += pieces / perSheet;
                    pieces = pieces % perSheet;
                }

                int totalReturn = (sheets * perSheet) + pieces;
                row.Cells[ColReturnSheets].Value = sheets;
                row.Cells[ColReturnPieces].Value = pieces;
                row.Cells[ColReturnTotal].Value = totalReturn;
                row.Cells[ColBalanceReturn].Value = Math.Max(0, maxAllowed - totalReturn);

                ApplyRowStatusColor(row, totalReturn, maxAllowed);
                RebuildStockFromDetails();
            }
            finally
            {
                _isUpdatingRow = false;
            }
        }

        /// <summary>
        /// Stock IN lines hamesha return lines se derive hoti hain. Is liye stock grid
        /// read-only hai aur user kabhi return se alag stock quantity post nahi kar sakta.
        /// Total pieces ko Box -> Packet -> Sheet -> Stamp me toora jata hai
        /// (UnitConversionMaster ke rates ke mutabiq).
        /// </summary>
        private void RebuildStockFromDetails()
        {
            if (_isSyncingGrids || _isListMode || _stockTable == null || _entryDetailTable == null)
                return;

            _isSyncingGrids = true;
            try
            {
                _stockTable.Clear();

                Dictionary<string, StockDAL.UnitConversion> convCache =
                    new Dictionary<string, StockDAL.UnitConversion>();

                foreach (DataRow src in _entryDetailTable.Rows)
                {
                    int categoryID = ToInt(src["CategoryID"]);
                    int denominationID = ToInt(src["DenominationID"]);
                    if (categoryID == 0 || denominationID == 0) continue;

                    int totalPieces = ToInt(src[ColReturnTotal]);
                    if (totalPieces <= 0) continue;

                    string key = categoryID + "|" + denominationID;
                    StockDAL.UnitConversion conv;
                    if (!convCache.TryGetValue(key, out conv))
                    {
                        conv = _stockDal.GetUnitConversion(categoryID, denominationID);
                        convCache[key] = conv;
                    }

                    int piecesPerSheet = ToInt(src[ColPiecesPerSheet]);
                    if (piecesPerSheet <= 0 && conv != null)
                        piecesPerSheet = conv.PiecesPerSheet;

                    int box, packet, sheet, stamp;
                    SplitIntoStockUnits(
                        totalPieces,
                        piecesPerSheet,
                        conv == null ? 0 : conv.PacketsPerBox,
                        conv == null ? 0 : conv.SheetsPerPacket,
                        out box, out packet, out sheet, out stamp);

                    DataRow r = _stockTable.NewRow();
                    r["CategoryID"] = categoryID;
                    r["DenominationID"] = denominationID;
                    r["Category"] = Convert.ToString(src["CategoryName"]);
                    r["Denomination"] = FormatDenomText(src["Denomination"]);
                    r["BoxQty"] = box;
                    r["PacketQty"] = packet;
                    r["SheetQty"] = sheet;
                    r["StampQty"] = stamp;
                    _stockTable.Rows.Add(r);
                }
            }
            finally
            {
                _isSyncingGrids = false;
            }
        }

        /// <summary>
        /// Total pieces ko stock units me break karta hai (bara unit pehle).
        /// Misal: 13000 pieces, 1/sheet, 1000 sheets/packet, 6 packets/box
        /// => 2 box + 0 packet + 1000 sheet + 0 stamp.
        /// </summary>
        private static void SplitIntoStockUnits(
            int totalPieces,
            int piecesPerSheet,
            int packetsPerBox,
            int sheetsPerPacket,
            out int box,
            out int packet,
            out int sheet,
            out int stamp)
        {
            box = 0;
            packet = 0;
            sheet = 0;
            stamp = 0;

            if (totalPieces <= 0) return;

            int pps = Math.Max(0, piecesPerSheet);
            int spp = Math.Max(0, sheetsPerPacket);
            int ppb = Math.Max(0, packetsPerBox);

            if (pps <= 0)
            {
                // Conversion rate nahi hai to pieces ko hi sheets maan lo.
                sheet = totalPieces;
                return;
            }

            int piecesPerPacket = spp > 0 ? spp * pps : 0;
            int piecesPerBox = (ppb > 0 && piecesPerPacket > 0) ? ppb * piecesPerPacket : 0;

            int remaining = totalPieces;

            if (piecesPerBox > 0)
            {
                box = remaining / piecesPerBox;
                remaining %= piecesPerBox;
            }

            if (piecesPerPacket > 0)
            {
                packet = remaining / piecesPerPacket;
                remaining %= piecesPerPacket;
            }

            sheet = remaining / pps;
            stamp = remaining % pps;
        }

        private static void ApplyRowStatusColor(DataGridViewRow row, int returned, int maxAllowed)
        {
            Color back, fore;

            if (returned > maxAllowed)
            {
                back = ColorExceedBack;
                fore = ColorExceedFore;
            }
            else if (returned == 0)
            {
                back = Color.Empty;
                fore = Color.Empty;
            }
            else if (returned == maxAllowed)
            {
                back = ColorFullReturnBack;
                fore = ColorFullReturnFore;
            }
            else
            {
                back = ColorPartialReturnBack;
                fore = ColorPartialReturnFore;
            }

            foreach (string colName in new[] { ColReturnTotal, ColBalanceReturn })
            {
                if (!row.DataGridView.Columns.Contains(colName)) continue;

                DataGridViewCellStyle style = row.Cells[colName].Style;
                style.BackColor = back;
                style.ForeColor = fore;
                style.SelectionBackColor = back;
                style.SelectionForeColor = fore;
            }
        }

        private void RefreshAllRowStatus()
        {
            if (_isListMode) return;

            foreach (DataGridViewRow row in dgvReturnDetail.Rows)
            {
                if (row.IsNewRow) continue;
                int returned = ParseInt(row.Cells[ColReturnTotal].Value);
                int maxAllowed = ParseInt(row.Cells[ColMaxReturnable].Value);
                ApplyRowStatusColor(row, returned, maxAllowed);
            }
        }

        private void dgvReturnDetail_CurrentCellChanged(object sender, EventArgs e)
        {
            if (_isListMode || _isConfiguringGrid || _isLoadingData) return;

            if (dgvReturnDetail.CurrentCell != null)
                ShowStockForRow(dgvReturnDetail.CurrentCell.RowIndex);
        }

        private void dgvReturnDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isListMode && e.RowIndex >= 0)
                OpenSelectedReturnFromList();
        }

        private void dgvReturnDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvReturnDetail.Columns[e.ColumnIndex].Name == "Denomination" &&
                e.Value != null && e.Value != DBNull.Value)
            {
                decimal amount;
                if (decimal.TryParse(Convert.ToString(e.Value), out amount))
                {
                    e.Value = "Rs." + Convert.ToInt32(amount) + "/-";
                    e.FormattingApplied = true;
                }
            }
        }

        private static string FormatDenomText(object val)
        {
            if (val == null || val == DBNull.Value) return "";
            string s = Convert.ToString(val);
            if (s.StartsWith("Rs.", StringComparison.OrdinalIgnoreCase)) return s;

            decimal amount;
            if (decimal.TryParse(s, out amount))
                return "Rs." + Convert.ToInt32(amount) + "/-";

            return s;
        }

        private void dgvReturnDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (_isListMode) return;

            TextBox tb = e.Control as TextBox;
            if (tb == null || dgvReturnDetail.CurrentCell == null) return;

            tb.KeyPress -= DigitsOnly_KeyPress;
            string col = dgvReturnDetail.CurrentCell.OwningColumn.Name;
            if (col == ColReturnSheets || col == ColReturnPieces)
                tb.KeyPress += DigitsOnly_KeyPress;
        }

        private static void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void ShowStockForRow(int rowIndex)
        {
            if (_isConfiguringGrid || _isLoadingData || _isListMode)
                return;

            if (rowIndex < 0 || rowIndex >= dgvReturnDetail.Rows.Count || rowIndex == _stockCardRow)
                return;

            if (!dgvReturnDetail.Columns.Contains("CategoryID") ||
                !dgvReturnDetail.Columns.Contains("DenominationID") ||
                !dgvReturnDetail.Columns.Contains("CategoryName") ||
                !dgvReturnDetail.Columns.Contains("Denomination"))
                return;

            _stockCardRow = rowIndex;
            DataGridViewRow row = dgvReturnDetail.Rows[rowIndex];
            int categoryID = ParseInt(row.Cells["CategoryID"].Value);
            int denominationID = ParseInt(row.Cells["DenominationID"].Value);

            if (categoryID == 0 || denominationID == 0)
            {
                ClearStockCards();
                return;
            }

            lblStockItem.Text = Convert.ToString(row.Cells["CategoryName"].Value) + " | " +
                                FormatDenomText(row.Cells["Denomination"].Value);

            DataRow bal = _stockDal.GetStockBalance(categoryID, denominationID);
            if (bal == null)
            {
                ClearStockCards();
                return;
            }

            lblBoxBalance.Text = ToInt(bal["BoxQty"]).ToString("N0");
            lblPacketBalance.Text = ToInt(bal["PacketQty"]).ToString("N0");
            lblSheetBalance.Text = ToInt(bal["SheetQty"]).ToString("N0");
            lblStampBalance.Text = ToInt(bal["StampQty"]).ToString("N0");
        }


        private void ClearStockCards()
        {
            lblBoxBalance.Text = "0";
            lblPacketBalance.Text = "0";
            lblSheetBalance.Text = "0";
            lblStampBalance.Text = "0";
        }

        private void RefreshGridTheme()
        {
            if (IsDisposed || dgvStockEntry == null) return;

            ThemePalette palette = AppTheme.P;
            dgvStockEntry.BackgroundColor = palette.Background;
            GridStyler.Apply(dgvStockEntry);

            if (pnlCurrentStock != null)
            {
                pnlCurrentStock.BackColor = palette.Surface;
                pnlCurrentStock.ForeColor = palette.TextPrimary;
            }

            foreach (Panel card in new[] { pnlBox, pnlPacket, pnlSheet, pnlStamp })
            {
                if (card != null)
                {
                    card.BackColor = palette.SurfaceAlt;
                    card.ForeColor = palette.TextPrimary;
                }
            }
        }

        // =====================================================================
        // List Mode Operations & rptSaleRetun.rdlc Report
        // =====================================================================

        private void LoadReturnListData()
        {
            DataTable dt = _returnDal.GetSaleReturns(
                txtSearchReturnNo.Text,
                cmbFilterStatus.Text,
                dtFilterFrom.Value.Date,
                dtFilterTo.Value.Date);

            _returnListTable.Clear();
            if (dt != null)
            {
                foreach (DataRow src in dt.Rows)
                {
                    DataRow row = _returnListTable.NewRow();
                    row["ReturnID"] = ToInt(src["ReturnID"]);
                    row["ReturnNo"] = Convert.ToString(src["ReturnNo"]);
                    row["ReturnDate"] = Convert.ToDateTime(src["ReturnDate"]);
                    row["SupplyNo"] = Convert.ToString(src["SupplyNo"]);
                    row["InvoiceNo"] = Convert.ToString(src["InvoiceNo"]);
                    row["OfficeName"] = Convert.ToString(src["OfficeName"]);
                    row["TotalSheets"] = ToInt(src["TotalSheets"]);
                    row["TotalPieces"] = ToInt(src["TotalPieces"]);
                    row["StatusName"] = Convert.ToString(src["StatusName"]);
                    row["Remarks"] = Convert.ToString(src["Remarks"]);
                    _returnListTable.Rows.Add(row);
                }
            }
        }

        private void btnSearchReturns_Click(object sender, EventArgs e)
        {
            LoadReturnListData();
        }

        private void btnOpenReturn_Click(object sender, EventArgs e)
        {
            OpenSelectedReturnFromList();
        }

        private void OpenSelectedReturnFromList()
        {
            if (dgvReturnDetail.CurrentRow == null)
            {
                MessageBox.Show("Please select a sale return record.", "Sale Return",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedID = ParseInt(dgvReturnDetail.CurrentRow.Cells["ReturnID"].Value);
            if (selectedID <= 0) return;

            _isLoadingData = true;
            try
            {
                LoadExistingReturn(selectedID);
            }
            finally
            {
                _isLoadingData = false;
            }

            SwitchToMode(isListMode: false);
            ApplyButtonState();
        }

        private void btnPrintReturn_Click(object sender, EventArgs e)
        {
            int targetReturnID = _returnID;
            if (_isListMode && dgvReturnDetail.CurrentRow != null)
                targetReturnID = ParseInt(dgvReturnDetail.CurrentRow.Cells["ReturnID"].Value);

            DataTable dtHeader = targetReturnID > 0
                ? _returnDal.GetSaleReturnHeader(targetReturnID)
                : new DataTable("SaleReturnHeader");

            DataTable dtDetail = targetReturnID > 0
                ? _returnDal.GetSaleReturnDetails(targetReturnID)
                : (_entryDetailTable != null ? _entryDetailTable.Copy() : new DataTable("SaleReturnDetail"));

            DataTable dtStock = targetReturnID > 0
                ? _returnDal.GetSaleReturnStock(targetReturnID)
                : (_stockTable != null ? _stockTable.Copy() : new DataTable("SaleReturnStock"));

            frmReportPreview preview = new frmReportPreview();
            preview.LoadReport(ReportFileName, dtHeader, dtDetail, dtStock, null, null);
            preview.ShowDialog();
        }

        // =====================================================================
        // Validation & Entry Actions
        // =====================================================================

        private bool ValidateReturn()
        {
            _errors.Clear();

            if (_selectedSupplyID <= 0)
            {
                MessageBox.Show("Please select an Issued Supply first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbIssuedSupply.Focus();
                return false;
            }

            bool hasAnyReturn = false;
            StringBuilder overLimit = new StringBuilder();

            foreach (DataGridViewRow row in dgvReturnDetail.Rows)
            {
                if (row.IsNewRow) continue;

                int totalReturn = ParseInt(row.Cells[ColReturnTotal].Value);
                int maxAllowed = ParseInt(row.Cells[ColMaxReturnable].Value);

                if (totalReturn > 0)
                    hasAnyReturn = true;

                if (totalReturn > maxAllowed)
                {
                    overLimit.AppendFormat("{0}: return {1:N0} > allowed {2:N0}\n",
                        Convert.ToString(row.Cells["CategoryName"].Value),
                        totalReturn,
                        maxAllowed);
                }
            }

            if (!hasAnyReturn)
            {
                MessageBox.Show("Please enter return quantity for at least one item.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (overLimit.Length > 0)
            {
                MessageBox.Show("Return quantity cannot exceed returnable balance:\n\n" + overLimit,
                    "Return Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidateStockLines())
                return false;

            return true;
        }

        /// <summary>
        /// Har return line ke liye ek derived stock line honi chahiye. Stock grid ab
        /// read-only hai, is liye ye check sirf safety net hai.
        /// </summary>
        private bool ValidateStockLines()
        {
            bool hasAnyReturn = false;
            foreach (DataRow r in _entryDetailTable.Rows)
            {
                if (ToInt(r[ColReturnTotal]) > 0) { hasAnyReturn = true; break; }
            }

            if (!hasAnyReturn)
                return true;

            if (_stockTable == null || _stockTable.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Stock IN lines could not be prepared. Please press Refresh and enter the quantities again.",
                    "Sale Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (DataRow r in _entryDetailTable.Rows)
            {
                if (ToInt(r[ColReturnTotal]) <= 0) continue;

                int categoryID = ToInt(r["CategoryID"]);
                int denominationID = ToInt(r["DenominationID"]);

                bool found = false;
                foreach (DataRow s in _stockTable.Rows)
                {
                    if (ToInt(s["CategoryID"]) == categoryID &&
                        ToInt(s["DenominationID"]) == denominationID)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    MessageBox.Show(
                        "Stock IN line is missing for " + Convert.ToString(r["CategoryName"]) +
                        ". Please press Refresh and enter the quantities again.",
                        "Sale Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private List<SaleReturnLine> CollectReturnLines()
        {
            List<SaleReturnLine> list = new List<SaleReturnLine>();
            if (_entryDetailTable == null) return list;

            foreach (DataRow r in _entryDetailTable.Rows)
            {
                list.Add(new SaleReturnLine
                {
                    SupplyDetailID = ToInt(r["SupplyDetailID"]),
                    CategoryID = ToInt(r["CategoryID"]),
                    DenominationID = ToInt(r["DenominationID"]),
                    PiecesPerSheet = ToInt(r[ColPiecesPerSheet]),
                    ReturnSheets = ToInt(r[ColReturnSheets]),
                    ReturnPieces = ToInt(r[ColReturnPieces]),
                    TotalPieces = ToInt(r[ColReturnTotal]),
                    Remarks = Convert.ToString(r["ReturnRemarks"])
                });
            }
            return list;
        }

        private List<SaleReturnStockLine> CollectStockLines()
        {
            List<SaleReturnStockLine> list = new List<SaleReturnStockLine>();
            if (_stockTable == null) return list;

            foreach (DataRow r in _stockTable.Rows)
            {
                list.Add(new SaleReturnStockLine
                {
                    CategoryID = ToInt(r["CategoryID"]),
                    DenominationID = ToInt(r["DenominationID"]),
                    BoxQty = ToInt(r["BoxQty"]),
                    PacketQty = ToInt(r["PacketQty"]),
                    SheetQty = ToInt(r["SheetQty"]),
                    StampQty = ToInt(r["StampQty"])
                });
            }
            return list;
        }

        private void btnSaveDraft_Click(object sender, EventArgs e)
        {
            if (!ValidateReturn()) return;

            try
            {
                _returnID = _returnDal.SaveDraft(
                    _returnID,
                    txtFinancialYear.Text.Trim(),
                    dtReturnDate.Value.Date,
                    _selectedSupplyID,
                    txtRemarks.Text.Trim(),
                    CollectReturnLines(),
                    CollectStockLines());

                DataTable hdr = _returnDal.GetSaleReturnHeader(_returnID);
                if (hdr != null && hdr.Rows.Count > 0)
                    txtReturnNo.Text = Convert.ToString(hdr.Rows[0]["ReturnNo"]);

                SetStatus(StatusDraft, "Draft");
                ApplyButtonState();

                MessageBox.Show($"Sale return draft saved ({txtReturnNo.Text}).",
                    "Sale Return", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving draft:\n\n" + ex.Message,
                    "Sale Return", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPostReturn_Click(object sender, EventArgs e)
        {
            if (!ValidateReturn()) return;

            DialogResult confirm = MessageBox.Show(
                "Post this sale return? Quantities will be added (IN) to StockMaster.",
                "Post Sale Return", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _returnID = _returnDal.PostReturnWithStockIn(
                    _returnID,
                    txtFinancialYear.Text.Trim(),
                    dtReturnDate.Value.Date,
                    _selectedSupplyID,
                    txtRemarks.Text.Trim(),
                    CollectReturnLines(),
                    CollectStockLines());

                DataTable hdr = _returnDal.GetSaleReturnHeader(_returnID);
                if (hdr != null && hdr.Rows.Count > 0)
                    txtReturnNo.Text = Convert.ToString(hdr.Rows[0]["ReturnNo"]);

                SetStatus(StatusPosted, "Posted");
                LockForm();
                ApplyButtonState();

                if (dgvReturnDetail.CurrentCell != null)
                {
                    _stockCardRow = -1;
                    ShowStockForRow(dgvReturnDetail.CurrentCell.RowIndex);
                }

                MessageBox.Show($"Sale return {txtReturnNo.Text} posted and stock updated (IN).",
                    "Sale Return", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error posting return:\n\n" + ex.Message,
                    "Sale Return", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _isUpdatingRow = true;
            try
            {
                foreach (DataGridViewRow row in dgvReturnDetail.Rows)
                {
                    if (row.IsNewRow) continue;
                    int maxAllowed = ParseInt(row.Cells[ColMaxReturnable].Value);
                    row.Cells[ColReturnSheets].Value = 0;
                    row.Cells[ColReturnPieces].Value = 0;
                    row.Cells[ColReturnTotal].Value = 0;
                    row.Cells[ColBalanceReturn].Value = maxAllowed;
                    ApplyRowStatusColor(row, 0, maxAllowed);
                }
                RebuildStockFromDetails();
            }
            finally
            {
                _isUpdatingRow = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetStatus(int statusID, string labelText)
        {
            _currentStatusID = statusID;
            txtReturnStatus.Text = labelText;
            UITheme.StyleStatusBox(txtReturnStatus, statusID);
        }

        private void ApplyButtonState()
        {
            bool editable = !_isListMode && _currentStatusID != StatusPosted;
            btnSaveDraft.Enabled = editable;
            btnPostReturn.Enabled = editable;
            btnRefresh.Enabled = editable;
        }

        private void LockForm()
        {
            cmbIssuedSupply.Enabled = false;
            dtReturnDate.Enabled = false;
            txtRemarks.ReadOnly = true;
            dgvReturnDetail.ReadOnly = true;
            dgvStockEntry.ReadOnly = true;
        }

        private static int ParseInt(object val)
        {
            int n;
            int.TryParse(Convert.ToString(val), out n);
            return n;
        }

        private static int ToInt(object val)
        {
            return (val == null || val == DBNull.Value) ? 0 : Convert.ToInt32(val);
        }
    }
}