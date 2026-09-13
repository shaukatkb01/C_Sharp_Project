using FastReport.Utils;
using Microsoft.Win32;
using SupplyBranch.DAL;
using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Reflection;


namespace SupplyBranch.Forms.Reports
{
    public partial class frmReport : Form
    {
        private bool IsEditMode = false;

        private readonly ReportDAL _dal = new ReportDAL();

        private ToolTip _toolTip;

        /// <summary>
        /// Apply modern visual adjustments to frmReport controls at runtime.
        /// Does not modify control names, handlers or behavior — only appearance, layout and accessibility hints.
        /// </summary>
        private void ApplyModernReportUI()
        {
            try
            {
                // 1. FlowLayoutPanel layouting ko PAUSE karein taake controls hilein nahi
                if (flowLayoutPanel1 != null)
                {
                    flowLayoutPanel1.SuspendLayout();

                    // Double Buffering enable karein flickering khatam karne ke liye
                    typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                   ?.SetValue(flowLayoutPanel1, true, null);
                }

                // Form ki drawing bhi pause karein
                this.SuspendLayout();

                // ToolTip initialization
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

                // Base fonts and colors
                var labelFont = new Font("Segoe UI", 9F, FontStyle.Regular);
                var labelBold = new Font("Segoe UI", 9F, FontStyle.Bold);
                var controlFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                var primary = Color.FromArgb(31, 78, 121);

                // Flow panel visual improvements
                if (flowLayoutPanel1 != null)
                {
                    flowLayoutPanel1.BackColor = Color.Transparent;
                    flowLayoutPanel1.WrapContents = false;
                    flowLayoutPanel1.AutoScroll = true;
                    flowLayoutPanel1.Padding = new Padding(10);
                    flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
                    flowLayoutPanel1.Width = Math.Max(360, flowLayoutPanel1.Width);
                }

                // Styling logic
                Action<Control> styleControl = (ctrl) =>
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
                        cb.Padding = new Padding(6);
                        cb.DropDownHeight = 200;
                        cb.IntegralHeight = false;
                        cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        cb.AutoCompleteSource = AutoCompleteSource.ListItems;

                        string targetName = (cb.Tag != null)
                            ? cb.Tag.ToString()
                            : cb.Name.Replace("cmb", "").Replace("Combo", "").Trim();
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
                        btn.Cursor = Cursors.Hand;
                        btn.Margin = new Padding(6, 12, 6, 6);
                        _toolTip.SetToolTip(btn, btn.Text);
                    }
                };

                // Standardize all controls in flowLayoutPanel1
                if (flowLayoutPanel1 != null)
                {
                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                    {
                        styleControl(ctrl);
                    }
                }

                // Specific adjustments
                if (label3 != null)
                {
                    label3.Font = labelBold;
                    label3.ForeColor = primary;
                    label3.Margin = new Padding(3, 2, 3, 6);
                }

                if (cmbReportType != null) { cmbReportType.Width = 320; _toolTip.SetToolTip(cmbReportType, "Select the report type to generate"); }
                if (cmbCategory != null) { cmbCategory.Width = 320; cmbCategory.DropDownHeight = 240; }
                if (cmbDenomination != null) cmbDenomination.Width = 320;
                if (cmbOffice != null) cmbOffice.Width = 320;
                if (cmbStatus != null) cmbStatus.Width = 320;
                if (cmbFinancialYear != null) cmbFinancialYear.Width = 320;
                if (dtFrom != null) dtFrom.Width = 200;
                if (dtTo != null) dtTo.Width = 200;

                if (cmbReportType != null && string.IsNullOrWhiteSpace(cmbReportType.AccessibilityObject.Name))
                    cmbReportType.AccessibleName = "Report Type";

                if (btnOfficeWise != null)
                {
                    btnOfficeWise.Anchor = AnchorStyles.Top;
                }
            }
            catch
            {
                // Non-blocking catch
            }
            finally
            {
                // 2. Layout rendering ko RESUME karein taake sab aik saath display ho
                if (flowLayoutPanel1 != null)
                {
                    flowLayoutPanel1.ResumeLayout(true);
                }
                this.ResumeLayout(true);
            }
        }

        private void UpdateCombo()
        {
            cmbStatus.Enabled = IsEditMode;

            //btnDelete.Enabled = !IsEditMode;

            //btnPrint.Enabled = !IsEditMode;

            //btnNew.Enabled = !IsEditMode;

            //btnEdit.Enabled = !IsEditMode;
        }
        // Constructor: enable double-buffering on heavy panels and keep existing InitializeComponent call
        public frmReport()
        {
            InitializeComponent();

            // reduce flicker / repaint overhead on complex layouts
            try
            {
                if (flowLayoutPanel1 != null)
                {
                    // set protected DoubleBuffered property via reflection
                    typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                        .SetValue(flowLayoutPanel1, true, null);
                }
            }
            catch
            {
                // non-fatal
            }
        }

        // Make Load async and move blocking data calls off the UI thread
        private async void frmReport_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            // Apply modern UI improvements (runtime only) — keep quick UI ops on UI thread
            ApplyModernReportUI();

            if (dtFrom != null)
            {
                dtFrom.Value = new DateTime(2026, 1, 1);
            }

            //----------------------------
            // Office (load off UI thread)
            //----------------------------
            try
            {
                OfficeDAL officeDAL = new OfficeDAL();

                // fetch on threadpool, then marshal result to UI thread
                var offices = await Task.Run(() => officeDAL.GetAllOffices());

                if (cmbOffice != null)
                {
                    cmbOffice.DataSource = offices;
                    cmbOffice.DisplayMember = "OfficeName";
                    cmbOffice.ValueMember = "OfficeID";
                    cmbOffice.SelectedIndex = 0;
                }
            }
            catch
            {
                // handle/log as needed
            }

            // Financial Year
            OfficeDAL officeDAL1 = new OfficeDAL();
            cmbFinancialYear.DataSource = officeDAL1.GetFinancialYear();

            cmbFinancialYear.DisplayMember = "FinancialYearName";

            cmbFinancialYear.ValueMember = "FinancialYear";

            cmbFinancialYear.SelectedIndex = 0;
            //----------------------------
            // Status
            //----------------------------

            SupplyDAL supplyDAL = new SupplyDAL();

            cmbStatus.DataSource = supplyDAL.GetDraftStatus();
            cmbStatus.DisplayMember = "StatusName";
            cmbStatus.ValueMember = "StatusID";
            cmbStatus.SelectedIndex = 0;

            //----------------------------
            // Category
            //----------------------------
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryID";
            cmbCategory.DataSource = _dal.GetCategories();
            //cmbCategory.SelectedIndex = 0;
            cmbCategory.Enabled = true;

        }

        public void LoadDenominationCombo(int categoryID)
        {
            DataTable dt = _dal.GetDenominations(categoryID);

            // Dynamic Top Row 'All' Insert
            DataRow dr = dt.NewRow();
            dr["DenominationID"] = 0;
            dr["DisplayDenomination"] = "-- All Denominations --";
            dt.Rows.InsertAt(dr, 0);

            // ComboBox Binding
            cmbDenomination.DataSource = dt;
            cmbDenomination.DisplayMember = "DisplayDenomination";
            cmbDenomination.ValueMember = "DenominationID";
        }

        private void LoadAllDenominationsOnly()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DenominationID", typeof(int));
            dt.Columns.Add("DisplayDenomination", typeof(string));

            // Sirf Single Row "All Denominations"
            dt.Rows.Add(0, "-- All Denominations --");

            cmbDenomination.DataSource = dt;
            cmbDenomination.DisplayMember = "DisplayDenomination";
            cmbDenomination.ValueMember = "DenominationID";
        }


        private void btnOfficeWise_Click(object sender, EventArgs e)
        {
            ReportFilter filter = new ReportFilter();

            //----------------------------
            // Common Filters
            //----------------------------

            if (cmbOffice.SelectedValue != null)
            {
                int officeID = Convert.ToInt32(cmbOffice.SelectedValue);

                if (officeID != 0)
                    filter.OfficeID = officeID;
            }

            if (cmbCategory.SelectedValue != null)
            {
                int categoryID = Convert.ToInt32(cmbCategory.SelectedValue);

                if (categoryID != 0)
                    filter.CategoryID = categoryID;
            }

            if (cmbStatus.SelectedValue != null)
            {
                int statusID = Convert.ToInt32(cmbStatus.SelectedValue);

                if (statusID != 0)
                    filter.StatusID = statusID;
            }

            filter.FromDate = dtFrom.Value.Date;
            filter.ToDate = dtTo.Value.Date;


            //----------------------------
            // Variables
            //----------------------------

            string reportFile = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            DataTable dt4 = null;


            //----------------------------
            // Report Selection
            //----------------------------

            switch (cmbReportType.Text)
            {
                //========================================
                // SUPPLY REPORTS
                //========================================

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


                //========================================
                // INDENT REPORTS
                //========================================

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


                //========================================
                // OTHER REPORTS
                //========================================

                case "Invoice Register":

                    reportFile = "rptInvoiceRegister.rdlc";

                    dt = _dal.GetSupplyRegister(filter);

                    break;


                case "Financial Year Report":

                    reportFile = "rptCurrentStock.rdlc";
                    StockDAL sd = new StockDAL();

                    dt = sd.GetCurrentStockPosition();

                    break;


                case "Index Register":

                    reportFile = "rptIndex.rdlc";

                    dt = _dal.GetSupplyRegister(filter);

                    break;


                case "Current Stock":
                    reportFile = "rptCurrentStock.rdlc";

                    int selectedCategory = 0;
                    int selectedDenomination = 0;

                    if (cmbCategory.SelectedValue != null && int.TryParse(cmbCategory.SelectedValue.ToString(), out int cId))
                        selectedCategory = cId;

                    if (cmbDenomination.SelectedValue != null && int.TryParse(cmbDenomination.SelectedValue.ToString(), out int dId))
                        selectedDenomination = dId;

                    dt3 = _dal.GetCurrentStockReport(selectedCategory, selectedDenomination);

                    
                    break;

                case "Stock Register":

                    reportFile = "rptStockRegister.rdlc";

                    int selectedCategory1 = 0;
                    int selectedDenomination1 = 0;
                    string transactionType = "";

                    // 1. Category ID Read
                    if (cmbCategory.SelectedValue != null && int.TryParse(cmbCategory.SelectedValue.ToString(), out int cId1))
                        selectedCategory1 = cId1;

                    // 2. Denomination ID Read
                    if (cmbDenomination.SelectedValue != null && int.TryParse(cmbDenomination.SelectedValue.ToString(), out int dId1))
                        selectedDenomination1 = dId1;

                    // 3. Transaction Type Read (FIXED SYNTAX)
                    if (cmbTransactionType.SelectedItem != null)
                    {
                        transactionType = cmbTransactionType.Text.Trim();
                    }

                    // Call DAL Method
                    dt3 = _dal.GetStockTransaction(selectedCategory1, selectedDenomination1, transactionType);

                    break;

                default:

                    MessageBox.Show(
                        "Please Select Report.",
                        "Report",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
            }


            //----------------------------
            // Check Record
            //----------------------------

            if ((dt == null || dt.Rows.Count == 0) &&
                (dt1 == null || dt1.Rows.Count == 0) &&
                (dt2 == null || dt2.Rows.Count == 0) &&
                (dt3 == null || dt3.Rows.Count == 0) &&
                (dt4 == null || dt4.Rows.Count == 0))
            {
                MessageBox.Show(
                    "No Record Found.",
                    "Report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }


            //----------------------------
            // Open Preview
            //----------------------------

            frmReportPreview frm = new frmReportPreview();

            frm.LoadReport(
                reportFile,
                dt,
                dt1,
                dt2,
                dt3,
                dt4);

            frm.ShowDialog();
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {

            // defult property of status combo box is enabled, but for some reports it should be disabled, so we are changing the property based on report type selection.
            cmbStatus.SelectedValue = 0;
            cmbStatus.Enabled = true;
            cmbOffice.SelectedIndex = 0;
            cmbOffice.Enabled = true;
            cmbStatus.SelectedValue = 0;
            cmbStatus.Enabled = true;
            cmbCategory.SelectedIndex = 0;
            cmbCategory.Enabled = true;
            lblDenomination.Visible = false;
            cmbDenomination.Visible = false;

            lblOffice.Visible = true;
            lblStatus.Visible = true;
            lblFincialYear.Visible = true;
            lblFrom.Visible = true;
            lblTo.Visible = true;
            // combo
            cmbOffice.Visible = true;
            cmbStatus.Visible = true;
            cmbFinancialYear.Visible = true;
            dtFrom.Visible = true;
            dtTo.Visible = true;

            switch (cmbReportType.Text)
            {

                case "Office Wise Supply":

                    cmbStatus.SelectedValue = 3;
                    cmbStatus.Enabled = false;


                    break;

                case "Category Wise Supply":

                    break;

                case "Supply Register":


                    break;


                case "Office Wise Indent":
                    cmbStatus.Enabled = false;

                    break;

                case "Category Wise Indent":

                    break;


                case "Indent Register":

                    break;

                case "Invoice Register":

                    break;

                case "Financial Year Report":


                case "Pending Supplies":


                    break;

                case "Current Stock":
                    lblDenomination.Visible = true;
                    cmbDenomination.Visible = true;

                    // lable
                    lblOffice.Visible = false;
                    lblStatus.Visible = false;
                    lblFincialYear.Visible = false;
                    lblFrom.Visible = false;
                    lblTo.Visible = false;
                    // combo
                    cmbOffice.Visible = false;
                    cmbStatus.Visible = false;
                    cmbFinancialYear.Visible = false;
                    dtFrom.Visible = false;
                    dtTo.Visible = false;

                    break;

                case "Index Register":
                    break;


                case "Performa":

                    break;

                default:
                    MessageBox.Show("Please Select Report.");
                    return;
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cmbCategory.SelectedValue != null && int.TryParse(cmbCategory.SelectedValue.ToString(), out int categoryID))
            {
                if (categoryID > 0)
                {
                    // Jab koi specific Category select ho, to us ke Denominations load karein
                    LoadDenominationCombo(categoryID);
                    
                }
                else
                {
                    // Jab "All Categories" (0) select ho, to Denomination mein bhi sirf "All" set kar dein
                    LoadAllDenominationsOnly();
                }
            }
        }
    }
}