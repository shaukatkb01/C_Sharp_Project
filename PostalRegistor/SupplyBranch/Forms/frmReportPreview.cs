using Microsoft.Reporting.WinForms;
using SupplyBranch.DataAccess;
using System;
using System.Data;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmReportPreview : Form
    {
        private readonly SupplyDAL supplyDAL = new SupplyDAL();

        //=========================================
        // Supply Performa
        //=========================================

        private int _supplyID;

        //=========================================
        // Register Reports
        //=========================================

        private bool _isRegisterReport = false;

        private string _reportFile = "";

        private DataTable _reportData = null;
        private DataTable _reportData1 = null;

        //=========================================
        // Constructor
        //=========================================

        public frmReportPreview()
        {
            InitializeComponent();
        }

        public frmReportPreview(int supplyID)
        {
            InitializeComponent();

            _supplyID = supplyID;
        }

        //=========================================
        // Generic Report Loader
        //=========================================

        public void LoadReport(string reportFile, DataTable dt, DataTable dt1)
        {
            _isRegisterReport = true;

            _reportFile = reportFile;

            _reportData = dt;
            _reportData1 = dt1;
        }

        //=========================================
        // Form Load
        //=========================================

        private void frmReportPreview_Load(object sender, EventArgs e)
        {

       
            //-----------------------------------------------------
            // Register Reports
            //-----------------------------------------------------

            if (_isRegisterReport)
            {
                reportViewer1.Reset();

                reportViewer1.LocalReport.ReportEmbeddedResource =
                    "SupplyBranch.Reports.RDLC." + _reportFile;

                reportViewer1.LocalReport.DataSources.Clear();


                //-------------------------------------------------
                // Indent Reports
                //-------------------------------------------------

                if (_reportFile == "rptIndentRegister.rdlc" ||
                    _reportFile == "rptOfficeWiseIndent.rdlc" ||
                    _reportFile == "rptCategoryWiseIndent.rdlc")
                {
                    reportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(
                            "dsIndent",
                            _reportData1));
                }


                //-------------------------------------------------
                // Office Wise Indent
                //-------------------------------------------------

                //else if (_reportFile == "rptOfficeWiseIndent.rdlc")
                //{
                //    reportViewer1.LocalReport.DataSources.Add(
                //        new ReportDataSource(
                //            "dsSupply",
                //            _reportData));
                //}


                //-------------------------------------------------
                // Other Supply Register Reports
                //-------------------------------------------------

                else
                {
                    reportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(
                            "dsSupply",
                            _reportData));
                }


                reportViewer1.RefreshReport();

                return;
            }


            //-----------------------------------------------------
            // Supply Performa
            //-----------------------------------------------------

            reportViewer1.Reset();

            reportViewer1.LocalReport.ReportEmbeddedResource =
                "SupplyBranch.Reports.rptSupplyPerforma.rdlc";


            DataTable dtHeader =
                supplyDAL.GetSupplyPerformaHeader(_supplyID);

            DataTable dtDetail =
                supplyDAL.GetSupplyPerformaDetail(_supplyID);


            reportViewer1.LocalReport.DataSources.Clear();


            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource(
                    "dsHeader",
                    dtHeader));


            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource(
                    "dsDetail",
                    dtDetail));


            reportViewer1.RefreshReport();
        }
    }
}