using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;



namespace FileIndex
{
    public partial class frmReportView : Form
    {
        // ReportViewer ka object banayein
        private ReportViewer reportViewer1;

        public frmReportView()
        {
            InitializeComponent();

            // ReportViewer ko initialize karein
            reportViewer1 = new ReportViewer();
            reportViewer1.Dock = DockStyle.Fill;
            this.Controls.Add(reportViewer1);
        }

        // Is function mein do extra cheezein bhejni hain: dataSetName aur reportPath
        public void LoadReport(DataTable dt, string dataSetName, string reportPath)
        {
            reportViewer1.LocalReport.DataSources.Clear();

            // 1. "dataSetName" ab dynamic hai (e.g., "dtSupply" ya "dtIssueInvoice")
            ReportDataSource rds = new ReportDataSource(dataSetName, dt);

            // 2. Report ka file name bhi ab dynamic hai
            reportViewer1.LocalReport.ReportPath = reportPath;

            reportViewer1.LocalReport.DataSources.Add(rds);

            // Scrollbar aur display settings (pichle masle ko hal karne ke liye)
            reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;

            reportViewer1.RefreshReport();
        }
    }
}
