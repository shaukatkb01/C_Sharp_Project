using Microsoft.Reporting.WinForms;
using SupplyBranch.DataAccess;
using SupplyBranch.Forms.Transactions;
using System;
using System.Data;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmReportPreview : Form
    {
        private frmSupply supply;

        private bool _addDate;
        private readonly SupplyDAL supplyDAL = new SupplyDAL();

        //=========================================
        // Supply Performa
        //=========================================

        private int _supplyID;

        //=========================================
        // Register Reports
        //=========================================

        private bool _isRegisterReport = false;
        private bool _isSupplyDocumentReport = false;

        private string _reportFile = "";
        private string _supplyIDs;
        private DataTable _reportData = null;
        private DataTable _reportData1 = null;
        private DataTable _reportData2 = null;
        private DataTable _reportData3 = null;
        private DataTable _reportData4 = null;

        //=========================================
        // Constructor
        //=========================================

        
        public frmReportPreview()
        {
            InitializeComponent();
        }

        public frmReportPreview(int supplyID, bool? addDate)
        {
            InitializeComponent();

            _supplyID = supplyID;
            _addDate = addDate ?? false;
        }

        //=========================================
        // Generic Report Loader
        //=========================================

        public void LoadReport(string reportFile, DataTable dt, DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4)
        {
            _isRegisterReport = true;

            _reportFile = reportFile;

            _reportData = dt;
            _reportData1 = dt1;
            _reportData2 = dt2;
            _reportData3 = dt3;
            _reportData4 = dt4;
        }

        public void LoadSupplyDocumentReport(
            string reportFile,
            int supplyID)
        {
            _isSupplyDocumentReport = true;
            _isRegisterReport = false;

            _reportFile = reportFile;
            _supplyID = supplyID;
        }

        //=========================================
        // Form Load
        //=========================================

        public void frmReportPreview_Load(object sender, EventArgs e)
        {
            if (_isSupplyDocumentReport)
            {
                reportViewer1.Reset();

                reportViewer1.LocalReport.ReportEmbeddedResource =
                    "SupplyBranch.Reports.RDLC." + _reportFile;

                // 1. Header ke liye header method (Agar aap multiple IDs ke liye header lana chahte hain)
                DataTable documentHeader =
                    supplyDAL.GetSupplyPerformaHeaderForMultiple(_supplyIDs, false); // _supplyIDs agar comma-separated string hai

                // 2. Detail ke liye detail method (Yahan pehle ghalti se header method call ho gaya tha)
                DataTable documentDetail =
                    supplyDAL.GetSupplyDemandDetailForMultiple(_supplyIDs);

                reportViewer1.LocalReport.DataSources.Clear();

                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource(
                        "SupplyHeader",
                        documentHeader));

                reportViewer1.LocalReport.DataSources.Add(
                    new ReportDataSource(
                        "SupplyDetail",
                        documentDetail));

                reportViewer1.RefreshReport();

                return;
            }

            if (_isRegisterReport)
            {
                //-----------------------------------------------------
                // Register Reports
                //-----------------------------------------------------

                reportViewer1.Reset();

                reportViewer1.LocalReport.ReportEmbeddedResource =
                    "SupplyBranch.Reports.RDLC." + _reportFile;

                reportViewer1.LocalReport.DataSources.Clear();


                //-------------------------------------------------
                // 1. Indent Reports (Office / Category Wise)
                //-------------------------------------------------
                if (_reportFile == "rptOfficeWiseIndent.rdlc" ||
                    _reportFile == "rptCategoryWiseIndent.rdlc")
                {
                    reportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(
                            "dsIndent",
                            _reportData1));
                }

                //-------------------------------------------------
                // 2. Indent Register (Current Balance Report)
                //-------------------------------------------------
                else if (_reportFile == "rptIndentRegister.rdlc")
                {
                    // Yahan RDLC ka Dataset name dein (Check karein RDLC mein "dsIndent" hai ya "dsIndentCurrentBalance")
                    reportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(
                            "dsSupply",  // <-- RDLC Designer mein jo Dataset ka naam rakha hua hai wo likhein
                            _reportData2));
                }

                //-----------------------------------------------
                // 3. Current Stock Position
                //-----------------------------------------------

                else if (_reportFile == "rptCurrentStock.rdlc" ||
                         _reportFile == "rptStockRegister.rdlc")
                {
                    reportViewer1.LocalReport.DataSources.Add(
                        new ReportDataSource(
                            "dsStock",
                            _reportData3));
                }
                //-------------------------------------------------
                // 3. Other Supply Register Reports
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
                supplyDAL.GetSupplyPerformaHeader(_supplyID, _addDate);

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


            reportViewer1.LocalReport.SetParameters(
    new ReportParameter("AddDate", _addDate.ToString())
);


            reportViewer1.RefreshReport();
        }

        public void LoadMultipleSupplyDocumentReport(string reportFile, string supplyIDs)
        {
            _isSupplyDocumentReport = true;
            _isRegisterReport = false;
            _reportFile = reportFile;
            _supplyIDs = supplyIDs;

            if (IsHandleCreated)
            {
                RenderSupplyDocumentReport();
            }
        }

        private void RenderSupplyDocumentReport()
        {
            reportViewer1.Reset();
            reportViewer1.LocalReport.ReportEmbeddedResource =
                "SupplyBranch.Reports.RDLC." + _reportFile;

            DataTable documentHeader =
                supplyDAL.GetSupplyPerformaHeaderForMultiple(_supplyIDs, false) ?? new DataTable();

            DataTable documentDetail =
                supplyDAL.GetSupplyDemandDetailForMultiple(_supplyIDs) ?? new DataTable();

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource("SupplyHeader", documentHeader));
            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource("SupplyDetail", documentDetail));

            reportViewer1.RefreshReport();
        }
    }
}