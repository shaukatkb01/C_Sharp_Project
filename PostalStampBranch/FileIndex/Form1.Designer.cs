namespace FileIndex
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            colorDialog1 = new ColorDialog();
            button1 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label5 = new Label();
            IssueMaxNo = new Label();
            nextIssueNolbl = new Label();
            stampDesignNUM = new NumericUpDown();
            fileNoCmb = new ComboBox();
            label4 = new Label();
            issueNo = new TextBox();
            label1 = new Label();
            postmarkQty = new NumericUpDown();
            label2 = new Label();
            label13 = new Label();
            leafletQty = new NumericUpDown();
            label14 = new Label();
            dateOfIssuePicker = new DateTimePicker();
            FDCQty = new NumericUpDown();
            label15 = new Label();
            label3 = new Label();
            label7 = new Label();
            remarkTxt = new TextBox();
            souvenirQty = new NumericUpDown();
            stampQty = new NumericUpDown();
            label16 = new Label();
            label18 = new Label();
            postmarkPrice = new NumericUpDown();
            label17 = new Label();
            leafletPrice = new NumericUpDown();
            label12 = new Label();
            FDCPrice = new NumericUpDown();
            label11 = new Label();
            souvenirPrice = new NumericUpDown();
            stampPrice = new NumericUpDown();
            label10 = new Label();
            label9 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            souvenirDesignNUM = new NumericUpDown();
            label6 = new Label();
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            stampQtyFC = new NumericUpDown();
            label8 = new Label();
            ((System.ComponentModel.ISupportInitialize)stampDesignNUM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)postmarkQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)leafletQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FDCQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)souvenirQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)stampQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)postmarkPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)leafletPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FDCPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)souvenirPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)stampPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)souvenirDesignNUM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)stampQtyFC).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 165, 255);
            button1.ForeColor = Color.White;
            button1.Location = new Point(338, 701);
            button1.Name = "button1";
            button1.Size = new Size(190, 52);
            button1.TabIndex = 18;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(283, 138);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 3);
            flowLayoutPanel1.TabIndex = 88;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(0, 165, 255);
            label5.Location = new Point(234, 32);
            label5.Name = "label5";
            label5.Size = new Size(398, 87);
            label5.TabIndex = 89;
            label5.Text = "Add Commemorative Stamps Issue Details";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // IssueMaxNo
            // 
            IssueMaxNo.AutoSize = true;
            IssueMaxNo.Location = new Point(552, 98);
            IssueMaxNo.Name = "IssueMaxNo";
            IssueMaxNo.Size = new Size(0, 21);
            IssueMaxNo.TabIndex = 87;
            // 
            // nextIssueNolbl
            // 
            nextIssueNolbl.AutoSize = true;
            nextIssueNolbl.Font = new Font("Segoe UI", 20F);
            nextIssueNolbl.Location = new Point(545, 132);
            nextIssueNolbl.Name = "nextIssueNolbl";
            nextIssueNolbl.Size = new Size(0, 37);
            nextIssueNolbl.TabIndex = 86;
            // 
            // stampDesignNUM
            // 
            stampDesignNUM.Location = new Point(495, 203);
            stampDesignNUM.Name = "stampDesignNUM";
            stampDesignNUM.Size = new Size(171, 29);
            stampDesignNUM.TabIndex = 3;
            stampDesignNUM.Tag = "stampcontroll";
            // 
            // fileNoCmb
            // 
            fileNoCmb.DropDownStyle = ComboBoxStyle.DropDownList;
            fileNoCmb.FormattingEnabled = true;
            fileNoCmb.Location = new Point(24, 203);
            fileNoCmb.Name = "fileNoCmb";
            fileNoCmb.Size = new Size(141, 29);
            fileNoCmb.TabIndex = 0;
            fileNoCmb.Tag = "stampcontroll";
            fileNoCmb.SelectedIndexChanged += fileNoCmb_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(495, 169);
            label4.Name = "label4";
            label4.Size = new Size(171, 31);
            label4.TabIndex = 60;
            label4.Text = "Stamp Design No:-";
            // 
            // issueNo
            // 
            issueNo.Enabled = false;
            issueNo.Location = new Point(318, 203);
            issueNo.Name = "issueNo";
            issueNo.Size = new Size(171, 29);
            issueNo.TabIndex = 2;
            issueNo.Tag = "stampcontroll";
            issueNo.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(353, 169);
            label1.Name = "label1";
            label1.Size = new Size(100, 21);
            label1.TabIndex = 56;
            label1.Text = "issueNo";
            // 
            // postmarkQty
            // 
            postmarkQty.BackColor = SystemColors.Window;
            postmarkQty.Location = new Point(660, 474);
            postmarkQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            postmarkQty.Name = "postmarkQty";
            postmarkQty.Size = new Size(171, 29);
            postmarkQty.TabIndex = 16;
            postmarkQty.Tag = "stampcontroll";
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(59, 169);
            label2.Name = "label2";
            label2.Size = new Size(70, 29);
            label2.TabIndex = 58;
            label2.Text = "File No:-";
            // 
            // label13
            // 
            label13.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label13.ForeColor = Color.White;
            label13.Location = new Point(442, 467);
            label13.Name = "label13";
            label13.Size = new Size(169, 29);
            label13.TabIndex = 84;
            label13.Text = "Postmark Qty:-";
            // 
            // leafletQty
            // 
            leafletQty.Location = new Point(660, 438);
            leafletQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            leafletQty.Name = "leafletQty";
            leafletQty.Size = new Size(171, 29);
            leafletQty.TabIndex = 14;
            leafletQty.Tag = "stampcontroll";
            // 
            // label14
            // 
            label14.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label14.ForeColor = Color.White;
            label14.Location = new Point(442, 431);
            label14.Name = "label14";
            label14.Size = new Size(169, 29);
            label14.TabIndex = 82;
            label14.Text = "Leaflet Qty:-";
            // 
            // dateOfIssuePicker
            // 
            dateOfIssuePicker.CalendarForeColor = Color.MistyRose;
            dateOfIssuePicker.CustomFormat = "";
            dateOfIssuePicker.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateOfIssuePicker.Format = DateTimePickerFormat.Short;
            dateOfIssuePicker.Location = new Point(171, 203);
            dateOfIssuePicker.Name = "dateOfIssuePicker";
            dateOfIssuePicker.RightToLeft = RightToLeft.Yes;
            dateOfIssuePicker.Size = new Size(141, 29);
            dateOfIssuePicker.TabIndex = 1;
            dateOfIssuePicker.Tag = "stampcontroll";
            dateOfIssuePicker.Value = new DateTime(2026, 1, 12, 18, 48, 0, 0);
            dateOfIssuePicker.ValueChanged += dateOfIssuePicker_ValueChanged;
            // 
            // FDCQty
            // 
            FDCQty.Location = new Point(660, 402);
            FDCQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            FDCQty.Name = "FDCQty";
            FDCQty.Size = new Size(171, 29);
            FDCQty.TabIndex = 13;
            FDCQty.Tag = "stampcontroll";
            // 
            // label15
            // 
            label15.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label15.ForeColor = Color.White;
            label15.Location = new Point(443, 397);
            label15.Name = "label15";
            label15.Size = new Size(169, 29);
            label15.TabIndex = 80;
            label15.Text = "FDC Qty:-";
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(171, 169);
            label3.Name = "label3";
            label3.Size = new Size(141, 19);
            label3.TabIndex = 59;
            label3.Text = "Date of Issue:- ";
            // 
            // label7
            // 
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label7.ForeColor = Color.White;
            label7.Location = new Point(39, 535);
            label7.Name = "label7";
            label7.Size = new Size(80, 21);
            label7.TabIndex = 61;
            label7.Text = "Remarks:-";
            // 
            // remarkTxt
            // 
            remarkTxt.BackColor = SystemColors.Window;
            remarkTxt.ForeColor = SystemColors.WindowText;
            remarkTxt.Location = new Point(41, 559);
            remarkTxt.Multiline = true;
            remarkTxt.Name = "remarkTxt";
            remarkTxt.PlaceholderText = "Insert Remark if any";
            remarkTxt.Size = new Size(790, 129);
            remarkTxt.TabIndex = 17;
            // 
            // souvenirQty
            // 
            souvenirQty.Enabled = false;
            souvenirQty.Location = new Point(660, 364);
            souvenirQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            souvenirQty.Name = "souvenirQty";
            souvenirQty.Size = new Size(171, 29);
            souvenirQty.TabIndex = 12;
            souvenirQty.Tag = "souveControll";
            // 
            // stampQty
            // 
            stampQty.Location = new Point(660, 327);
            stampQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            stampQty.Name = "stampQty";
            stampQty.Size = new Size(171, 29);
            stampQty.TabIndex = 11;
            stampQty.Tag = "stampcontroll";
            // 
            // label16
            // 
            label16.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label16.ForeColor = Color.White;
            label16.Location = new Point(443, 359);
            label16.Name = "label16";
            label16.Size = new Size(169, 29);
            label16.TabIndex = 76;
            label16.Text = "Souvenir Qty:-";
            // 
            // label18
            // 
            label18.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label18.ForeColor = Color.White;
            label18.Location = new Point(443, 329);
            label18.Name = "label18";
            label18.Size = new Size(169, 29);
            label18.TabIndex = 77;
            label18.Text = "Stamp Qty:-";
            // 
            // postmarkPrice
            // 
            postmarkPrice.Location = new Point(251, 441);
            postmarkPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            postmarkPrice.Name = "postmarkPrice";
            postmarkPrice.Size = new Size(171, 29);
            postmarkPrice.TabIndex = 9;
            postmarkPrice.Tag = "stampcontroll";
            postmarkPrice.Value = new decimal(new int[] { 1850, 0, 0, 0 });
            // 
            // label17
            // 
            label17.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label17.ForeColor = Color.White;
            label17.Location = new Point(6, 434);
            label17.Name = "label17";
            label17.Size = new Size(173, 29);
            label17.TabIndex = 74;
            label17.Text = "Postmark Price:-";
            // 
            // leafletPrice
            // 
            leafletPrice.Location = new Point(251, 405);
            leafletPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            leafletPrice.Name = "leafletPrice";
            leafletPrice.Size = new Size(171, 29);
            leafletPrice.TabIndex = 8;
            leafletPrice.Tag = "stampcontroll";
            // 
            // label12
            // 
            label12.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label12.ForeColor = Color.White;
            label12.Location = new Point(6, 398);
            label12.Name = "label12";
            label12.Size = new Size(172, 29);
            label12.TabIndex = 72;
            label12.Text = "Leaflet Price:-";
            // 
            // FDCPrice
            // 
            FDCPrice.Location = new Point(251, 370);
            FDCPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            FDCPrice.Name = "FDCPrice";
            FDCPrice.Size = new Size(171, 29);
            FDCPrice.TabIndex = 7;
            FDCPrice.Tag = "stampcontroll";
            // 
            // label11
            // 
            label11.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label11.ForeColor = Color.White;
            label11.Location = new Point(7, 365);
            label11.Name = "label11";
            label11.Size = new Size(172, 29);
            label11.TabIndex = 70;
            label11.Text = "FDC Price:-";
            // 
            // souvenirPrice
            // 
            souvenirPrice.Enabled = false;
            souvenirPrice.Location = new Point(251, 332);
            souvenirPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            souvenirPrice.Name = "souvenirPrice";
            souvenirPrice.Size = new Size(171, 29);
            souvenirPrice.TabIndex = 6;
            souvenirPrice.Tag = "souveControll";
            // 
            // stampPrice
            // 
            stampPrice.Location = new Point(251, 295);
            stampPrice.Margin = new Padding(3, 3, 3, 30);
            stampPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            stampPrice.Name = "stampPrice";
            stampPrice.Size = new Size(171, 29);
            stampPrice.TabIndex = 5;
            stampPrice.Tag = "stampcontroll";
            // 
            // label10
            // 
            label10.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label10.ForeColor = Color.White;
            label10.Location = new Point(7, 327);
            label10.Name = "label10";
            label10.Size = new Size(172, 29);
            label10.TabIndex = 67;
            label10.Text = "Souvenir Price:-";
            // 
            // label9
            // 
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label9.ForeColor = Color.White;
            label9.Location = new Point(7, 297);
            label9.Name = "label9";
            label9.Size = new Size(172, 29);
            label9.TabIndex = 66;
            label9.Text = "Stamp Price:-";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            // 
            // souvenirDesignNUM
            // 
            souvenirDesignNUM.Enabled = false;
            souvenirDesignNUM.Location = new Point(672, 203);
            souvenirDesignNUM.Name = "souvenirDesignNUM";
            souvenirDesignNUM.Size = new Size(171, 29);
            souvenirDesignNUM.TabIndex = 4;
            souvenirDesignNUM.Tag = "souveControll";
            // 
            // label6
            // 
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label6.ForeColor = Color.White;
            label6.Location = new Point(661, 169);
            label6.Name = "label6";
            label6.Size = new Size(192, 31);
            label6.TabIndex = 90;
            label6.Text = "Souvenir Design No:-";
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // stampQtyFC
            // 
            stampQtyFC.Location = new Point(660, 290);
            stampQtyFC.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            stampQtyFC.Name = "stampQtyFC";
            stampQtyFC.Size = new Size(171, 29);
            stampQtyFC.TabIndex = 10;
            stampQtyFC.Tag = "stampcontroll";
            // 
            // label8
            // 
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label8.ForeColor = Color.White;
            label8.Location = new Point(442, 292);
            label8.Name = "label8";
            label8.Size = new Size(212, 29);
            label8.TabIndex = 92;
            label8.Text = "Stamp FreeofCost Qty:-";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(866, 791);
            Controls.Add(stampQtyFC);
            Controls.Add(label8);
            Controls.Add(souvenirDesignNUM);
            Controls.Add(label6);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label5);
            Controls.Add(IssueMaxNo);
            Controls.Add(nextIssueNolbl);
            Controls.Add(stampDesignNUM);
            Controls.Add(fileNoCmb);
            Controls.Add(label4);
            Controls.Add(issueNo);
            Controls.Add(label1);
            Controls.Add(postmarkQty);
            Controls.Add(label2);
            Controls.Add(label13);
            Controls.Add(leafletQty);
            Controls.Add(label14);
            Controls.Add(dateOfIssuePicker);
            Controls.Add(FDCQty);
            Controls.Add(label15);
            Controls.Add(label3);
            Controls.Add(label7);
            Controls.Add(remarkTxt);
            Controls.Add(souvenirQty);
            Controls.Add(stampQty);
            Controls.Add(label16);
            Controls.Add(label18);
            Controls.Add(postmarkPrice);
            Controls.Add(label17);
            Controls.Add(leafletPrice);
            Controls.Add(label12);
            Controls.Add(FDCPrice);
            Controls.Add(label11);
            Controls.Add(souvenirPrice);
            Controls.Add(stampPrice);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            Text = "Add Commemorative Issue";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)stampDesignNUM).EndInit();
            ((System.ComponentModel.ISupportInitialize)postmarkQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)leafletQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)FDCQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)souvenirQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)stampQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)postmarkPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)leafletPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)FDCPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)souvenirPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)stampPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)souvenirDesignNUM).EndInit();
            ((System.ComponentModel.ISupportInitialize)stampQtyFC).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox4;
        private ColorDialog colorDialog1;
        private Button button1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label5;
        private Label IssueMaxNo;
        private Label nextIssueNolbl;
        private NumericUpDown stampDesignNUM;
        private ComboBox fileNoCmb;
        private Label label4;
        private TextBox issueNo;
        private Label label1;
        private NumericUpDown postmarkQty;
        private Label label2;
        private Label label13;
        private NumericUpDown leafletQty;
        private Label label14;
        private DateTimePicker dateOfIssuePicker;
        private NumericUpDown FDCQty;
        private Label label15;
        private Label label3;
        private Label label7;
        private TextBox remarkTxt;
        private NumericUpDown souvenirQty;
        private NumericUpDown stampQty;
        private Label label16;
        private Label label18;
        private NumericUpDown postmarkPrice;
        private Label label17;
        private NumericUpDown leafletPrice;
        private Label label12;
        private NumericUpDown FDCPrice;
        private Label label11;
        private NumericUpDown souvenirPrice;
        private NumericUpDown stampPrice;
        private Label label10;
        private Label label9;
        private System.Windows.Forms.Timer timer1;
        private NumericUpDown souvenirDesignNUM;
        private Label label6;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private NumericUpDown stampQtyFC;
        private Label label8;
    }
}
