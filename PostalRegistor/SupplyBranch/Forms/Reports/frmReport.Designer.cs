namespace SupplyBranch.Forms.Reports
{
    partial class frmReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbReportType = new SupplyBranch.Controls.ModernComboBox();
            this.lblOffice = new System.Windows.Forms.Label();
            this.cmbOffice = new SupplyBranch.Controls.ModernComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new SupplyBranch.Controls.ModernComboBox();
            this.lblFincialYear = new System.Windows.Forms.Label();
            this.cmbFinancialYear = new SupplyBranch.Controls.ModernComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCategory = new SupplyBranch.Controls.ModernComboBox();
            this.lblDenomination = new System.Windows.Forms.Label();
            this.cmbDenomination = new SupplyBranch.Controls.ModernComboBox();
            this.lblTransactionType = new System.Windows.Forms.Label();
            this.cmbTransactionType = new SupplyBranch.Controls.ModernComboBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnOfficeWise = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.cmbReportType);
            this.flowLayoutPanel1.Controls.Add(this.lblOffice);
            this.flowLayoutPanel1.Controls.Add(this.cmbOffice);
            this.flowLayoutPanel1.Controls.Add(this.lblStatus);
            this.flowLayoutPanel1.Controls.Add(this.cmbStatus);
            this.flowLayoutPanel1.Controls.Add(this.lblFincialYear);
            this.flowLayoutPanel1.Controls.Add(this.cmbFinancialYear);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.cmbCategory);
            this.flowLayoutPanel1.Controls.Add(this.lblDenomination);
            this.flowLayoutPanel1.Controls.Add(this.cmbDenomination);
            this.flowLayoutPanel1.Controls.Add(this.lblTransactionType);
            this.flowLayoutPanel1.Controls.Add(this.cmbTransactionType);
            this.flowLayoutPanel1.Controls.Add(this.lblFrom);
            this.flowLayoutPanel1.Controls.Add(this.dtFrom);
            this.flowLayoutPanel1.Controls.Add(this.lblTo);
            this.flowLayoutPanel1.Controls.Add(this.dtTo);
            this.flowLayoutPanel1.Controls.Add(this.btnOfficeWise);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(178, 31);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(360, 568);
            this.flowLayoutPanel1.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Report Type";
            // 
            // cmbReportType
            // 
            this.cmbReportType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Items.AddRange(new object[] {
            "Office Wise Supply",
            "Category Wise Supply",
            "Supply Register",
            "Indent Register",
            "Pending Supplies",
            "Dispatch Register",
            "Invoice Register",
            "Index Register",
            "Current Stock",
            "Stock Register"});
            this.cmbReportType.Location = new System.Drawing.Point(3, 16);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(320, 21);
            this.cmbReportType.TabIndex = 0;
            this.cmbReportType.SelectedIndexChanged += new System.EventHandler(this.cmbReportType_SelectedIndexChanged);
            // 
            // lblOffice
            // 
            this.lblOffice.AutoSize = true;
            this.lblOffice.Location = new System.Drawing.Point(3, 40);
            this.lblOffice.Name = "lblOffice";
            this.lblOffice.Size = new System.Drawing.Size(97, 13);
            this.lblOffice.TabIndex = 19;
            this.lblOffice.Text = "Office Wise Report";
            // 
            // cmbOffice
            // 
            this.cmbOffice.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbOffice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbOffice.FormattingEnabled = true;
            this.cmbOffice.Location = new System.Drawing.Point(3, 56);
            this.cmbOffice.Name = "cmbOffice";
            this.cmbOffice.Size = new System.Drawing.Size(320, 21);
            this.cmbOffice.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(3, 80);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(72, 13);
            this.lblStatus.TabIndex = 22;
            this.lblStatus.Text = "Supply Status";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(3, 96);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(320, 21);
            this.cmbStatus.TabIndex = 2;
            // 
            // lblFincialYear
            // 
            this.lblFincialYear.AutoSize = true;
            this.lblFincialYear.Location = new System.Drawing.Point(3, 120);
            this.lblFincialYear.Name = "lblFincialYear";
            this.lblFincialYear.Size = new System.Drawing.Size(74, 13);
            this.lblFincialYear.TabIndex = 26;
            this.lblFincialYear.Text = "Financial Year";
            // 
            // cmbFinancialYear
            // 
            this.cmbFinancialYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFinancialYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFinancialYear.FormattingEnabled = true;
            this.cmbFinancialYear.Location = new System.Drawing.Point(3, 136);
            this.cmbFinancialYear.Name = "cmbFinancialYear";
            this.cmbFinancialYear.Size = new System.Drawing.Size(320, 21);
            this.cmbFinancialYear.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Category";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(3, 176);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(320, 21);
            this.cmbCategory.TabIndex = 4;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // lblDenomination
            // 
            this.lblDenomination.AutoSize = true;
            this.lblDenomination.Location = new System.Drawing.Point(3, 200);
            this.lblDenomination.Name = "lblDenomination";
            this.lblDenomination.Size = new System.Drawing.Size(72, 13);
            this.lblDenomination.TabIndex = 34;
            this.lblDenomination.Text = "Denomination";
            this.lblDenomination.Visible = false;
            // 
            // cmbDenomination
            // 
            this.cmbDenomination.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDenomination.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDenomination.FormattingEnabled = true;
            this.cmbDenomination.Location = new System.Drawing.Point(3, 216);
            this.cmbDenomination.Name = "cmbDenomination";
            this.cmbDenomination.Size = new System.Drawing.Size(320, 21);
            this.cmbDenomination.TabIndex = 5;
            this.cmbDenomination.Visible = false;
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.AutoSize = true;
            this.lblTransactionType.Location = new System.Drawing.Point(3, 240);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(90, 13);
            this.lblTransactionType.TabIndex = 18;
            this.lblTransactionType.Text = "Transaction Type";
            this.lblTransactionType.Visible = false;
            // 
            // cmbTransactionType
            // 
            this.cmbTransactionType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTransactionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransactionType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTransactionType.FormattingEnabled = true;
            this.cmbTransactionType.Items.AddRange(new object[] {
            "-- All Transactions --",
            "IN",
            "OUT"});
            this.cmbTransactionType.Location = new System.Drawing.Point(3, 256);
            this.cmbTransactionType.Name = "cmbTransactionType";
            this.cmbTransactionType.Size = new System.Drawing.Size(320, 21);
            this.cmbTransactionType.TabIndex = 17;
            this.cmbTransactionType.Visible = false;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(3, 280);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(36, 13);
            this.lblFrom.TabIndex = 31;
            this.lblFrom.Text = "From :";
            // 
            // dtFrom
            // 
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(3, 296);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(203, 20);
            this.dtFrom.TabIndex = 6;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(3, 319);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(26, 13);
            this.lblTo.TabIndex = 32;
            this.lblTo.Text = "To :";
            // 
            // dtTo
            // 
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(3, 335);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(203, 20);
            this.dtTo.TabIndex = 7;
            // 
            // btnOfficeWise
            // 
            this.btnOfficeWise.Location = new System.Drawing.Point(3, 361);
            this.btnOfficeWise.Name = "btnOfficeWise";
            this.btnOfficeWise.Size = new System.Drawing.Size(320, 36);
            this.btnOfficeWise.TabIndex = 8;
            this.btnOfficeWise.Text = "Print Report";
            this.btnOfficeWise.UseVisualStyleBackColor = true;
            this.btnOfficeWise.Click += new System.EventHandler(this.btnOfficeWise_Click);
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 611);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "frmReport";
            this.Text = "INDENT / SUPPLY REPORTS AND INDEX";
            this.Load += new System.EventHandler(this.frmReport_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOffice;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblFincialYear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDenomination;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnOfficeWise;
        private System.Windows.Forms.Label lblTransactionType;
        private Controls.ModernComboBox cmbReportType;
        private Controls.ModernComboBox cmbOffice;
        private Controls.ModernComboBox cmbStatus;
        private Controls.ModernComboBox cmbFinancialYear;
        private Controls.ModernComboBox cmbCategory;
        private Controls.ModernComboBox cmbDenomination;
        private Controls.ModernComboBox cmbTransactionType;
    }
}