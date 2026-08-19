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
            this.btnOfficeWise = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbOffice = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFinancialYear = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOfficeWise
            // 
            this.btnOfficeWise.Location = new System.Drawing.Point(234, 331);
            this.btnOfficeWise.Name = "btnOfficeWise";
            this.btnOfficeWise.Size = new System.Drawing.Size(174, 32);
            this.btnOfficeWise.TabIndex = 0;
            this.btnOfficeWise.Text = "Print Report";
            this.btnOfficeWise.UseVisualStyleBackColor = true;
            this.btnOfficeWise.Click += new System.EventHandler(this.btnOfficeWise_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Office Wise Report";
            // 
            // cmbOffice
            // 
            this.cmbOffice.FormattingEnabled = true;
            this.cmbOffice.Location = new System.Drawing.Point(234, 121);
            this.cmbOffice.Name = "cmbOffice";
            this.cmbOffice.Size = new System.Drawing.Size(175, 21);
            this.cmbOffice.TabIndex = 2;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(234, 156);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(175, 21);
            this.cmbStatus.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Supply Status";
            // 
            // cmbReportType
            // 
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Items.AddRange(new object[] {
            "Office Wise Supply",
            "Category Wise Supply",
            "Supply Register",
            "Office Wise Indent",
            "Category Wise Indent",
            "Indent Register",
            "Financial Year Report",
            "Pending Supplies",
            "Dispatch Register",
            "Invoice Register",
            "Index Register",
            "Stock Register"});
            this.cmbReportType.Location = new System.Drawing.Point(234, 86);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(175, 21);
            this.cmbReportType.TabIndex = 7;
            this.cmbReportType.SelectedIndexChanged += new System.EventHandler(this.cmbReportType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Report Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Financial Year";
            // 
            // cmbFinancialYear
            // 
            this.cmbFinancialYear.FormattingEnabled = true;
            this.cmbFinancialYear.Location = new System.Drawing.Point(234, 191);
            this.cmbFinancialYear.Name = "cmbFinancialYear";
            this.cmbFinancialYear.Size = new System.Drawing.Size(175, 21);
            this.cmbFinancialYear.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(98, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Category";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(235, 226);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(175, 21);
            this.cmbCategory.TabIndex = 10;
            // 
            // dtFrom
            // 
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(234, 261);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(176, 20);
            this.dtFrom.TabIndex = 12;
            // 
            // dtTo
            // 
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(235, 295);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(175, 20);
            this.dtTo.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(98, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "From :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(97, 304);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "To :";
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbFinancialYear);
            this.Controls.Add(this.cmbReportType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.cmbOffice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOfficeWise);
            this.Name = "frmReport";
            this.Text = " INDENT/SUPPLY REPORTS AND INDENX";
            this.Load += new System.EventHandler(this.frmReport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOfficeWise;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbOffice;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbFinancialYear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}