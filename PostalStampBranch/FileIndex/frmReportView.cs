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
using PostalStampSystem;



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
        //public void LoadReport(DataTable dt, string dataSetName, string reportPath)
        //{
        //    reportViewer1.LocalReport.DataSources.Clear();

        //    // 1. "dataSetName" ab dynamic hai (e.g., "dtSupply" ya "dtIssueInvoice")
        //    ReportDataSource rds = new ReportDataSource(dataSetName, dt);

        //    // 2. Report ka file name bhi ab dynamic hai
        //    reportViewer1.LocalReport.ReportPath = reportPath;

        //    reportViewer1.LocalReport.DataSources.Add(rds);

        //    // Scrollbar aur display settings (pichle masle ko hal karne ke liye)
        //    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
        //    reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;

        //    reportViewer1.RefreshReport();
        //}

        public void LoadReport(DataTable dataSource, string dataSetName, string reportPath,
            List<ReportParameter>? parameters = null)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.LocalReport.ReportPath = reportPath;

                // Data source
                ReportDataSource rds = new ReportDataSource(dataSetName, dataSource);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                // ✅ Parameters set karo
                if (parameters != null && parameters.Count > 0)
                {
                    reportViewer1.LocalReport.SetParameters(parameters);
                }

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void frmReportView_Load(object sender, EventArgs e)
        {
            //ThemeManager.ApplyTheme(this);
        }
    }
}
