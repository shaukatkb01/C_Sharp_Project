using SupplyBranch.DataAccess;
using SupplyBranch.Forms;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Reports
{
    public partial class frmDemandPrint : Form
    {
        private DataGridView dgvSupply;

        private DateTimePicker dtFrom;
        private DateTimePicker dtTo;

        private Button btnDemand;
        private Button btnDispatch;
        private Button btnMemo;
        private Button btnInvoice;

        private readonly ReportDAL _reportDAL = new ReportDAL();

        public frmDemandPrint()
        {
            InitializeComponent();
            BuildDemandPrintForm();
        }

        private void BuildDemandPrintForm()
        {
            Text = "Supply Number Selection";
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScroll = true;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            Size = new Size(
                Math.Min(1100, Screen.PrimaryScreen.WorkingArea.Width),
                Math.Min(650, Screen.PrimaryScreen.WorkingArea.Height));
            MinimumSize = new Size(650, 400);
            BackColor = Color.White;

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(12),
                BackColor = Color.White
            };

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 55F));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 55F));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            // Date filter panel
            FlowLayoutPanel datePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 8, 0, 0)
            };

            Label lblFrom = new Label
            {
                Text = "From Date:",
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Margin = new Padding(3, 8, 5, 0)
            };

            dtFrom = CreateDatePicker();
            dtFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);

            Label lblTo = new Label
            {
                Text = "To Date:",
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Margin = new Padding(25, 8, 5, 0)
            };

            dtTo = CreateDatePicker();
            dtTo.Value = DateTime.Today;

            datePanel.Controls.Add(lblFrom);
            datePanel.Controls.Add(dtFrom);
            datePanel.Controls.Add(lblTo);
            datePanel.Controls.Add(dtTo);

            mainLayout.Controls.Add(datePanel, 0, 0);

            // Supply grid
            dgvSupply = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersHeight = 35,
                EnableHeadersVisualStyles = false,
                MultiSelect = true,
                ReadOnly = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvSupply.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(31, 78, 121);

            dgvSupply.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvSupply.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);

            dgvSupply.DefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Regular);

            dgvSupply.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(221, 235, 247);

            dgvSupply.DefaultCellStyle.SelectionForeColor = Color.Black;

            DataGridViewCheckBoxColumn checkColumn =
                new DataGridViewCheckBoxColumn
                {
                    Name = "Select",
                    HeaderText = "Select",
                    DataPropertyName = "Select",
                    FillWeight = 8,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn supplyNoColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "SupplyNo",
                    HeaderText = "Supply Number",
                    DataPropertyName = "SupplyNo",
                    FillWeight = 22,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn supplyDateColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "SupplyDate",
                    HeaderText = "Supply Date",
                    DataPropertyName = "SupplyDate",
                    FillWeight = 16,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn officeColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "OfficeName",
                    HeaderText = "Office",
                    DataPropertyName = "OfficeName",
                    FillWeight = 22,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn statusColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "StatusName",
                    HeaderText = "Status",
                    DataPropertyName = "StatusName",
                    FillWeight = 16,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn supplyTypeColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "SupplyType",
                    HeaderText = "Supply Type",
                    DataPropertyName = "SupplyType",
                    FillWeight = 15,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn financialYearColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "FinancialYear",
                    HeaderText = "Financial Year",
                    DataPropertyName = "FinancialYear",
                    FillWeight = 15,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn statusIDColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "StatusID",
                    HeaderText = "Status ID",
                    DataPropertyName = "StatusID",
                    FillWeight = 12,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            DataGridViewTextBoxColumn invoiceColumn =
                new DataGridViewTextBoxColumn
                {
                    Name = "InvoiceNo",
                    HeaderText = "Invoice No",
                    DataPropertyName = "InvoiceNo",
                    FillWeight = 18,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

            dgvSupply.Columns.Add(checkColumn);
            dgvSupply.Columns.Add(supplyNoColumn);
            dgvSupply.Columns.Add(supplyDateColumn);
            dgvSupply.Columns.Add(statusColumn);
            dgvSupply.Columns.Add(officeColumn);
            dgvSupply.Columns.Add(invoiceColumn);

            mainLayout.Controls.Add(dgvSupply, 0, 2);

            // Report buttons
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0)
            };

            btnInvoice = CreateReportButton("Invoice");
            btnMemo = CreateReportButton("Memo");
            btnDispatch = CreateReportButton("Dispatch");
            btnDemand = CreateReportButton("Demand");

            btnDemand.Click += btnDemand_Click;
            btnDispatch.Click += btnDispatch_Click;
            btnMemo.Click += btnMemo_Click;
            btnInvoice.Click += btnInvoice_Click;

            buttonPanel.Controls.Add(btnInvoice);
            buttonPanel.Controls.Add(btnMemo);
            buttonPanel.Controls.Add(btnDispatch);
            buttonPanel.Controls.Add(btnDemand);

            mainLayout.Controls.Add(buttonPanel, 0, 1);

            Controls.Add(mainLayout);

            dtFrom.ValueChanged += DateFilter_ValueChanged;
            dtTo.ValueChanged += DateFilter_ValueChanged;

            LoadSupplyData();
        }

        private DateTimePicker CreateDatePicker()
        {
            return new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MMM-yyyy",
                Width = 145,
                Height = 28,
                Font = new Font("Segoe UI", 9F),
                Margin = new Padding(0, 3, 0, 0)
            };
        }

        private Button CreateReportButton(string buttonText)
        {
            return new Button
            {
                Text = buttonText,
                Width = 125,
                Height = 35,
                Margin = new Padding(6, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(31, 78, 121),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
        }

        private void DateFilter_ValueChanged(object sender, EventArgs e)
        {
            if (dtFrom.Value.Date > dtTo.Value.Date)
            {
                MessageBox.Show(
                    "From Date, To Date se bari nahi ho sakti.",
                    "Date Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            LoadSupplyData();
        }

        private void LoadSupplyData()
        {
            try
            {
                DataTable dt = _reportDAL.GetSupplyMasterByDate(
                    dtFrom.Value.Date,
                    dtTo.Value.Date);

                dgvSupply.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "SupplyMaster se data load nahi ho saka.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDemand_Click(object sender, EventArgs e)
        {
            OpenSelectedReport("rptDemand.rdlc");
        }

        private void btnDispatch_Click(object sender, EventArgs e)
        {
            OpenSelectedReport("rptDispatch.rdlc");
        }

        private void btnMemo_Click(object sender, EventArgs e)
        {
            OpenSelectedReport("rptMemo.rdlc");
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            OpenSelectedReport("rptInvoice.rdlc");
        }

        private void OpenSelectedReport(string reportFile)
        {
            dgvSupply.EndEdit();

            DataTable selectedData = GetSelectedSupplyData();

            if (selectedData == null || selectedData.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Please select at least one Supply No. first.",
                    "Selection Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // Yahan se aik se zyada supplies select karne wali restriction hata di gayi hai.
            // Ab agar user aik se zyada rows select karega, toh yeh un sab ki IDs collect karega.

            List<string> supplyIdsList = new List<string>();
            foreach (DataRow row in selectedData.Rows)
            {
                supplyIdsList.Add(row["SupplyID"].ToString());
            }

            string commaSeparatedIDs = string.Join(",", supplyIdsList);

            frmReportPreview preview = new frmReportPreview();

            // Multiple supplies ka data report mein bhejne ke liye 
            preview.LoadMultipleSupplyDocumentReport(
                reportFile,
                commaSeparatedIDs);

            preview.ShowDialog(this);
        }

        private DataTable GetSelectedSupplyData()
        {
            DataTable sourceTable = dgvSupply.DataSource as DataTable;

            if (sourceTable == null)
                return null;

            DataTable selectedTable = sourceTable.Clone();

            foreach (DataGridViewRow gridRow in dgvSupply.Rows)
            {
                if (gridRow.IsNewRow)
                    continue;

                bool isChecked = false;

                object checkValue = gridRow.Cells["Select"].Value;

                if (checkValue != null &&
                    checkValue != DBNull.Value)
                {
                    isChecked = Convert.ToBoolean(checkValue);
                }

                if (!isChecked)
                    continue;

                DataRowView rowView =
                    gridRow.DataBoundItem as DataRowView;

                if (rowView != null)
                {
                    selectedTable.ImportRow(rowView.Row);
                }
            }

            return selectedTable;
        }

        private void frmDemandPrint_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
        }
    }
}
