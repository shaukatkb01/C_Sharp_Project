using SupplyBranch.DAL;
using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Reports
{
    public partial class frmReport : Form
    {
        private readonly ReportDAL _dal = new ReportDAL();
        private readonly OfficeDAL _officeDal = new OfficeDAL();
        private readonly SupplyDAL _supplyDal = new SupplyDAL();
        private readonly StockDAL _stockDal = new StockDAL();

        private ToolTip _toolTip;
        private bool _isLoadingLookups = false;

        public frmReport()
        {
            InitializeComponent();
            EnableDoubleBuffering(flowLayoutPanel1);
        }

        private static void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;

            try
            {
                PropertyInfo prop = typeof(Control).GetProperty(
                    "DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                prop?.SetValue(control, true, null);
            }
            catch
            {
                // Ignore if restricted by environment
            }
        }

        /// <summary>
        /// Apply modern visual adjustments to frmReport controls at runtime.
        /// </summary>
        private void ApplyModernReportUI()
        {
            if (flowLayoutPanel1 != null)
                flowLayoutPanel1.SuspendLayout();

            SuspendLayout();

            try
            {
                _toolTip = new ToolTip
                {
                    ShowAlways = true,
                    AutoPopDelay = 8000,
                    InitialDelay = 400,
                    ReshowDelay = 200,
                    IsBalloon = false,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(34, 34, 34)
                };

                Font labelFont = new Font("Segoe UI", 9F, FontStyle.Regular);
                Font labelBold = new Font("Segoe UI", 9F, FontStyle.Bold);
                Font controlFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                Color primary = Color.FromArgb(31, 78, 121);

                if (flowLayoutPanel1 != null)
                {
                    flowLayoutPanel1.BackColor = Color.Transparent;
                    flowLayoutPanel1.WrapContents = false;
                    flowLayoutPanel1.AutoScroll = true;
                    flowLayoutPanel1.Padding = new Padding(10);
                    flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
                    flowLayoutPanel1.Width = Math.Max(360, flowLayoutPanel1.Width);

                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                    {
                        if (ctrl is Label lbl)
                        {
                            lbl.Font = labelFont;
                            lbl.ForeColor = Color.FromArgb(45, 45, 45);
                            lbl.Margin = new Padding(3, 6, 3, 4);
                        }
                        else if (ctrl is ComboBox cb)
                        {
                            cb.Font = controlFont;
                            cb.FlatStyle = FlatStyle.Flat;
                            cb.BackColor = Color.White;
                            cb.ForeColor = Color.FromArgb(34, 34, 34);
                            cb.Margin = new Padding(3, 3, 3, 8);
                            cb.DropDownHeight = 220;
                            cb.IntegralHeight = false;
                            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                            cb.Width = 320;

                            string targetName = cb.Tag != null
                                ? cb.Tag.ToString()
                                : cb.Name.Replace("cmb", "").Trim();
                            _toolTip.SetToolTip(cb, "Select " + targetName);
                        }
                        else if (ctrl is DateTimePicker dt)
                        {
                            dt.Font = controlFont;
                            dt.Format = DateTimePickerFormat.Custom;
                            dt.CustomFormat = "dd-MMM-yyyy";
                            dt.Width = 200;
                            dt.Margin = new Padding(3, 3, 3, 8);
                            _toolTip.SetToolTip(dt, "Choose date");
                        }
                        else if (ctrl is Button btn)
                        {
                            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderSize = 0;
                            btn.BackColor = primary;
                            btn.ForeColor = Color.White;
                            btn.Padding = new Padding(12, 6, 12, 6);
                            btn.Height = 36;
                            btn.Width = 320;
                            btn.Cursor = Cursors.Hand;
                            btn.Margin = new Padding(3, 12, 3, 6);
                            _toolTip.SetToolTip(btn, btn.Text);
                        }
                    }
                }

                if (label3 != null)
                {
                    label3.Font = labelBold;
                    label3.ForeColor = primary;
                    label3.Margin = new Padding(3, 2, 3, 6);
                }

                if (cmbReportType != null)
                {
                    _toolTip.SetToolTip(cmbReportType, "Select the report type to generate");
                    if (string.IsNullOrWhiteSpace(cmbReportType.AccessibilityObject.Name))
                        cmbReportType.AccessibleName = "Report Type";
                }
            }
            finally
            {
                if (flowLayoutPanel1 != null)
                    flowLayoutPanel1.ResumeLayout(true);

                ResumeLayout(true);
            }
        }

        private async void frmReport_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            ApplyModernReportUI();

            // Transaction Type (IN / OUT) stays hidden unless "Stock Register" is chosen
            SetTransactionTypeVisible(false);
            SetDenominationVisible(false);

            if (dtFrom != null)
                dtFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);

            if (dtTo != null)
                dtTo.Value = DateTime.Today;

            btnOfficeWise.Enabled = false;
            _isLoadingLookups = true;

            try
            {
                var lookups = await Task.Run(() => new
                {
                    Offices = _officeDal.GetAllOffices(),
                    FinancialYears = _officeDal.GetFinancialYear(),
                    Statuses = _supplyDal.GetDraftStatus(),
                    Categories = _dal.GetCategories()
                });

                BindComboSafe(cmbOffice, lookups.Offices, "OfficeName", "OfficeID");
                BindComboSafe(cmbFinancialYear, lookups.FinancialYears, "FinancialYearName", "FinancialYear");
                BindComboSafe(cmbStatus, lookups.Statuses, "StatusName", "StatusID");
                BindComboSafe(cmbCategory, lookups.Categories, "Name", "CategoryID");

                LoadAllDenominationsOnly();
                InitializeTransactionTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Report filters could not be loaded. Please try again.\n\n" + ex.Message,
                    "Report Filters",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _isLoadingLookups = false;
                btnOfficeWise.Enabled = true;
            }
        }

        private static void BindComboSafe(ComboBox combo, DataTable data, string displayMember, string valueMember)
        {
            if (combo == null || data == null) return;

            combo.DisplayMember = displayMember;
            combo.ValueMember = valueMember;
            combo.DataSource = data;

            if (data.Rows.Count > 0)
                combo.SelectedIndex = 0;
        }

        private void InitializeTransactionTypes()
        {
            cmbTransactionType.Items.Clear();
            cmbTransactionType.Items.Add("-- All Transactions --");
            cmbTransactionType.Items.Add("IN");
            cmbTransactionType.Items.Add("OUT");
            cmbTransactionType.SelectedIndex = 0;
        }

        private void SetTransactionTypeVisible(bool visible)
        {
            lblTransactionType.Visible = visible;
            cmbTransactionType.Visible = visible;

            if (!visible && cmbTransactionType.Items.Count > 0)
                cmbTransactionType.SelectedIndex = 0;
        }

        private void SetDenominationVisible(bool visible)
        {
            lblDenomination.Visible = visible;
            cmbDenomination.Visible = visible;
        }

        private void SetStandardFilterVisibility(bool showOffice, bool showStatus, bool showFinancialYear, bool showDateRange)
        {
            lblOffice.Visible = showOffice;
            cmbOffice.Visible = showOffice;

            lblStatus.Visible = showStatus;
            cmbStatus.Visible = showStatus;

            lblFincialYear.Visible = showFinancialYear;
            cmbFinancialYear.Visible = showFinancialYear;

            lblFrom.Visible = showDateRange;
            dtFrom.Visible = showDateRange;
            lblTo.Visible = showDateRange;
            dtTo.Visible = showDateRange;
        }

        public void LoadDenominationCombo(int categoryID)
        {
            DataTable dt = _dal.GetDenominations(categoryID);

            DataRow dr = dt.NewRow();
            dr["DenominationID"] = 0;
            dr["DisplayDenomination"] = "-- All Denominations --";
            dt.Rows.InsertAt(dr, 0);

            cmbDenomination.DisplayMember = "DisplayDenomination";
            cmbDenomination.ValueMember = "DenominationID";
            cmbDenomination.DataSource = dt;
            cmbDenomination.SelectedIndex = 0;
        }

        private void LoadAllDenominationsOnly()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DenominationID", typeof(int));
            dt.Columns.Add("DisplayDenomination", typeof(string));
            dt.Rows.Add(0, "-- All Denominations --");

            cmbDenomination.DisplayMember = "DisplayDenomination";
            cmbDenomination.ValueMember = "DenominationID";
            cmbDenomination.DataSource = dt;
            cmbDenomination.SelectedIndex = 0;
        }

        private static int GetSelectedInt(ComboBox combo)
        {
            if (combo?.SelectedValue == null) return 0;

            int value;
            return int.TryParse(combo.SelectedValue.ToString(), out value) ? value : 0;
        }

        private void btnOfficeWise_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbReportType.Text))
            {
                MessageBox.Show("Please select a report type.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbReportType.Focus();
                return;
            }

            if (dtFrom.Visible && dtTo.Visible && dtFrom.Value.Date > dtTo.Value.Date)
            {
                MessageBox.Show("'From' date cannot be greater than 'To' date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtFrom.Focus();
                return;
            }

            ReportFilter filter = new ReportFilter();

            int officeID = GetSelectedInt(cmbOffice);
            if (officeID != 0)
                filter.OfficeID = officeID;

            int categoryID = GetSelectedInt(cmbCategory);
            if (categoryID != 0)
                filter.CategoryID = categoryID;

            int denominationID = GetSelectedInt(cmbDenomination);
            if (denominationID != 0)
                filter.DenominationID = denominationID;

            if (cmbFinancialYear.SelectedValue != null)
            {
                string financialYear = cmbFinancialYear.SelectedValue.ToString();
                if (!string.IsNullOrWhiteSpace(financialYear))
                    filter.FinancialYear = financialYear;
            }

            int statusID = GetSelectedInt(cmbStatus);
            if (statusID != 0)
                filter.StatusID = statusID;

            filter.FromDate = dtFrom.Value.Date;
            filter.ToDate = dtTo.Value.Date;

            string reportFile = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            DataTable dt4 = null;

            switch (cmbReportType.Text)
            {
                // ========================================
                // SUPPLY REPORTS
                // ========================================
                case "Office Wise Supply":
                    reportFile = "rptOfficeWiseSupply.rdlc";
                    filter.StatusID = 3;
                    dt = _dal.GetSupplyRegister(filter);
                    break;

                case "Category Wise Supply":
                    reportFile = "rptCategoryWiseSupply.rdlc";
                    dt = _dal.GetSupplyRegister(filter);
                    break;

                case "Supply Register":
                    reportFile = "rptSupplyRegister.rdlc";
                    filter.SupplyRegisterOnly = true;
                    dt = _dal.GetSupplyRegister(filter);
                    break;

                case "Dispatch Register":
                    reportFile = "rptSupplyRegister.rdlc";
                    filter.StatusID = 3;
                    dt = _dal.GetSupplyRegister(filter);
                    break;

                case "Pending Supplies":
                    reportFile = "rptIndentRegister.rdlc";
                    dt2 = _dal.GetIndentCurrentBalanceReport(filter);
                    break;

                // ========================================
                // INDENT REPORTS
                // ========================================
                case "Office Wise Indent":
                    reportFile = "rptOfficeWiseIndent.rdlc";
                    dt1 = _dal.GetIndentRegister(filter);
                    break;

                case "Category Wise Indent":
                    reportFile = "rptCategoryWiseIndent.rdlc";
                    dt1 = _dal.GetIndentRegister(filter);
                    break;

                case "Indent Register":
                    reportFile = "rptIndentRegister.rdlc";
                    dt2 = _dal.GetIndentCurrentBalanceReport(filter);
                    break;

                // ========================================
                // OTHER & STOCK REPORTS
                // ========================================
                case "Invoice Register":
                    reportFile = "rptInvoiceRegister.rdlc";
                    dt = _dal.GetSupplyRegister(filter);
                    break;

                case "Financial Year Report":
                    reportFile = "rptCurrentStock.rdlc";
                    dt = _stockDal.GetCurrentStockPosition();
                    break;

                case "Index Register":
                    reportFile = "rptIndex.rdlc";
                    dt = _dal.GetSupplyRegister(filter);
                    break;

                case "Current Stock":
                    reportFile = "rptCurrentStock.rdlc";
                    dt3 = _dal.GetCurrentStockReport(categoryID, denominationID);
                    break;

                case "Stock Register":
                    reportFile = "rptStockRegister.rdlc";
                    string transactionType = cmbTransactionType.SelectedIndex > 0
                        ? cmbTransactionType.Text.Trim()
                        : "";
                    dt3 = _dal.GetStockTransaction(categoryID, denominationID, transactionType);
                    break;

                default:
                    MessageBox.Show("Please Select Report.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
            }

            if ((dt == null || dt.Rows.Count == 0) &&
                (dt1 == null || dt1.Rows.Count == 0) &&
                (dt2 == null || dt2.Rows.Count == 0) &&
                (dt3 == null || dt3.Rows.Count == 0) &&
                (dt4 == null || dt4.Rows.Count == 0))
            {
                MessageBox.Show("No Record Found.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmReportPreview frm = new frmReportPreview();
            frm.LoadReport(reportFile, dt, dt1, dt2, dt3, dt4);
            frm.ShowDialog();
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedValue = 0;
            cmbStatus.Enabled = true;

            if (cmbOffice.Items.Count > 0) cmbOffice.SelectedIndex = 0;
            cmbOffice.Enabled = true;

            if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;
            cmbCategory.Enabled = true;

            // Default: hide Denomination and TransactionType (IN/OUT)
            SetDenominationVisible(false);
            SetTransactionTypeVisible(false);
            SetStandardFilterVisibility(showOffice: true, showStatus: true, showFinancialYear: true, showDateRange: true);

            switch (cmbReportType.Text)
            {
                case "Office Wise Supply":
                case "Dispatch Register":
                    cmbStatus.SelectedValue = 3;
                    cmbStatus.Enabled = false;
                    break;

                case "Category Wise Supply":
                case "Supply Register":
                case "Invoice Register":
                case "Index Register":
                case "Financial Year Report":
                    break;

                case "Office Wise Indent":
                    cmbStatus.Enabled = false;
                    break;

                case "Category Wise Indent":
                case "Pending Supplies":
                    SetDenominationVisible(true);
                    break;

                case "Indent Register":
                    SetDenominationVisible(true);
                    SetStandardFilterVisibility(showOffice: true, showStatus: false, showFinancialYear: false, showDateRange: true);
                    break;

                case "Current Stock":
                    SetDenominationVisible(true);
                    SetStandardFilterVisibility(showOffice: false, showStatus: false, showFinancialYear: false, showDateRange: false);
                    break;

                case "Stock Register":
                    // IN / OUT combo shown ONLY here
                    SetDenominationVisible(true);
                    SetTransactionTypeVisible(true);
                    SetStandardFilterVisibility(showOffice: false, showStatus: false, showFinancialYear: false, showDateRange: false);
                    break;

                default:
                    MessageBox.Show("Please Select Report.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingLookups) return;

            int categoryID = GetSelectedInt(cmbCategory);

            if (categoryID > 0)
                LoadDenominationCombo(categoryID);
            else
                LoadAllDenominationsOnly();
        }
    }
}