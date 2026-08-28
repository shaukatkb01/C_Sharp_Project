using FastReport.Utils;
using Microsoft.Win32;
using SupplyBranch.DAL;
using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Data;
using System.Windows.Forms;


namespace SupplyBranch.Forms.Reports
{
    public partial class frmReport : Form
    {
        private bool IsEditMode = false;

        private readonly ReportDAL _dal = new ReportDAL();

       

        private void UpdateCombo()
        {
            cmbStatus.Enabled = IsEditMode;

            //btnDelete.Enabled = !IsEditMode;

            //btnPrint.Enabled = !IsEditMode;

            //btnNew.Enabled = !IsEditMode;

            //btnEdit.Enabled = !IsEditMode;
        }
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            if (dtFrom !=null)
            {
                dtFrom.Value = new DateTime(2026, 1, 1);
            }


            //----------------------------
            // Office
            //----------------------------

            OfficeDAL officeDAL = new OfficeDAL();

            cmbOffice.DataSource = officeDAL.GetAllOffices();
            cmbOffice.DisplayMember = "OfficeName";
            cmbOffice.ValueMember = "OfficeID";
            cmbOffice.SelectedIndex = 0;

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
                    reportFile = "rptStockRegister.rdlc";
                    dt = _dal.GetSupplyRegister(filter);
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
                (dt2 == null || dt2.Rows.Count == 0)) 
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
                dt2);

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
    }
}