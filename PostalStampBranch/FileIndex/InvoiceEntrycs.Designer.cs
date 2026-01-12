namespace FileIndex
{
    partial class InvoiceEntrycs
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
            sqlCommandBuilder1 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            btn_save = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            date_Picker = new DateTimePicker();
            label6 = new Label();
            num_Stamp = new NumericUpDown();
            num_FDC = new NumericUpDown();
            num_Leaflet = new NumericUpDown();
            num_Postmark = new NumericUpDown();
            num_FDCC = new NumericUpDown();
            groupBox1 = new GroupBox();
            com_PhilName = new ComboBox();
            label16 = new Label();
            label11 = new Label();
            text_PMPrice = new TextBox();
            com_FileNo = new ComboBox();
            text_FDCPrice = new TextBox();
            text_LeafletPrice = new TextBox();
            label10 = new Label();
            label12 = new Label();
            label13 = new Label();
            groupBox2 = new GroupBox();
            label15 = new Label();
            Remark_txt = new TextBox();
            combo_DT = new ComboBox();
            label8 = new Label();
            label9 = new Label();
            text_TA = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label7 = new Label();
            label14 = new Label();
            lastInvoice_No = new Label();
            ((System.ComponentModel.ISupportInitialize)num_Stamp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_Postmark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btn_save
            // 
            btn_save.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_save.Location = new Point(324, 1010);
            btn_save.Name = "btn_save";
            btn_save.Size = new Size(157, 38);
            btn_save.TabIndex = 11;
            btn_save.Text = "Save";
            btn_save.UseVisualStyleBackColor = true;
            btn_save.Click += btn_save_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(54, 105);
            label1.Name = "label1";
            label1.Size = new Size(83, 21);
            label1.TabIndex = 22;
            label1.Text = "Stamp Qty";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(54, 155);
            label2.Name = "label2";
            label2.Size = new Size(64, 21);
            label2.TabIndex = 24;
            label2.Text = "FDCQty";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(322, 60);
            label3.Name = "label3";
            label3.Size = new Size(85, 21);
            label3.TabIndex = 26;
            label3.Text = "Leaflet Qty";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(322, 105);
            label4.Name = "label4";
            label4.Size = new Size(104, 21);
            label4.TabIndex = 28;
            label4.Text = "Postmark Qty";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(322, 155);
            label5.Name = "label5";
            label5.Size = new Size(139, 21);
            label5.TabIndex = 30;
            label5.Text = "FDC Cancelled Qty";
            // 
            // date_Picker
            // 
            date_Picker.CustomFormat = "27/12/2025";
            date_Picker.Format = DateTimePickerFormat.Short;
            date_Picker.Location = new Point(176, 52);
            date_Picker.Name = "date_Picker";
            date_Picker.Size = new Size(127, 29);
            date_Picker.TabIndex = 3;
            date_Picker.Value = new DateTime(2025, 12, 27, 0, 0, 0, 0);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(54, 60);
            label6.Name = "label6";
            label6.Size = new Size(42, 21);
            label6.TabIndex = 32;
            label6.Text = "Date";
            // 
            // num_Stamp
            // 
            num_Stamp.Location = new Point(176, 97);
            num_Stamp.Name = "num_Stamp";
            num_Stamp.Size = new Size(127, 29);
            num_Stamp.TabIndex = 4;
            // 
            // num_FDC
            // 
            num_FDC.Location = new Point(176, 147);
            num_FDC.Name = "num_FDC";
            num_FDC.Size = new Size(127, 29);
            num_FDC.TabIndex = 5;
            num_FDC.ValueChanged += num_FDC_ValueChanged;
            // 
            // num_Leaflet
            // 
            num_Leaflet.Location = new Point(486, 52);
            num_Leaflet.Name = "num_Leaflet";
            num_Leaflet.Size = new Size(127, 29);
            num_Leaflet.TabIndex = 7;
            num_Leaflet.ValueChanged += num_Leaflet_ValueChanged;
            // 
            // num_Postmark
            // 
            num_Postmark.Location = new Point(486, 97);
            num_Postmark.Name = "num_Postmark";
            num_Postmark.Size = new Size(127, 29);
            num_Postmark.TabIndex = 8;
            num_Postmark.ValueChanged += num_Postmark_ValueChanged;
            // 
            // num_FDCC
            // 
            num_FDCC.Location = new Point(486, 147);
            num_FDCC.Name = "num_FDCC";
            num_FDCC.Size = new Size(127, 29);
            num_FDCC.TabIndex = 9;
            num_FDCC.ValueChanged += num_FDCC_ValueChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(com_PhilName);
            groupBox1.Controls.Add(label16);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(text_PMPrice);
            groupBox1.Controls.Add(com_FileNo);
            groupBox1.Controls.Add(text_FDCPrice);
            groupBox1.Controls.Add(text_LeafletPrice);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(label13);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(74, 184);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(667, 385);
            groupBox1.TabIndex = 44;
            groupBox1.TabStop = false;
            groupBox1.Text = "Price";
            // 
            // com_PhilName
            // 
            com_PhilName.DropDownStyle = ComboBoxStyle.DropDownList;
            com_PhilName.FormattingEnabled = true;
            com_PhilName.Location = new Point(145, 109);
            com_PhilName.Name = "com_PhilName";
            com_PhilName.Size = new Size(468, 29);
            com_PhilName.TabIndex = 2;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = Color.White;
            label16.Location = new Point(7, 117);
            label16.Name = "label16";
            label16.Size = new Size(94, 21);
            label16.TabIndex = 51;
            label16.Text = "Address To:-";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = Color.White;
            label11.Location = new Point(-8, 259);
            label11.Name = "label11";
            label11.Size = new Size(77, 21);
            label11.TabIndex = 50;
            label11.Text = "FDC Price";
            // 
            // text_PMPrice
            // 
            text_PMPrice.Enabled = false;
            text_PMPrice.Location = new Point(130, 330);
            text_PMPrice.Name = "text_PMPrice";
            text_PMPrice.ReadOnly = true;
            text_PMPrice.Size = new Size(127, 29);
            text_PMPrice.TabIndex = 47;
            // 
            // com_FileNo
            // 
            com_FileNo.DropDownStyle = ComboBoxStyle.DropDownList;
            com_FileNo.FormattingEnabled = true;
            com_FileNo.Location = new Point(145, 59);
            com_FileNo.Name = "com_FileNo";
            com_FileNo.Size = new Size(176, 29);
            com_FileNo.TabIndex = 1;
            com_FileNo.SelectedIndexChanged += com_FileNo_SelectedIndexChanged;
            // 
            // text_FDCPrice
            // 
            text_FDCPrice.Enabled = false;
            text_FDCPrice.Location = new Point(130, 230);
            text_FDCPrice.Name = "text_FDCPrice";
            text_FDCPrice.ReadOnly = true;
            text_FDCPrice.Size = new Size(127, 29);
            text_FDCPrice.TabIndex = 49;
            // 
            // text_LeafletPrice
            // 
            text_LeafletPrice.Enabled = false;
            text_LeafletPrice.Location = new Point(130, 280);
            text_LeafletPrice.Name = "text_LeafletPrice";
            text_LeafletPrice.ReadOnly = true;
            text_LeafletPrice.Size = new Size(127, 29);
            text_LeafletPrice.TabIndex = 45;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.White;
            label10.Location = new Point(13, 67);
            label10.Name = "label10";
            label10.Size = new Size(64, 21);
            label10.TabIndex = 20;
            label10.Text = "FileNo:-";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.White;
            label12.Location = new Point(7, 347);
            label12.Name = "label12";
            label12.Size = new Size(113, 21);
            label12.TabIndex = 48;
            label12.Text = "Postmark Price";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.White;
            label13.Location = new Point(-8, 309);
            label13.Name = "label13";
            label13.Size = new Size(94, 21);
            label13.TabIndex = 46;
            label13.Text = "Leaflet Price";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(Remark_txt);
            groupBox2.Controls.Add(num_Stamp);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(num_FDCC);
            groupBox2.Controls.Add(combo_DT);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(num_Postmark);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(num_Leaflet);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(text_TA);
            groupBox2.Controls.Add(num_FDC);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(date_Picker);
            groupBox2.Controls.Add(label6);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(74, 575);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(667, 429);
            groupBox2.TabIndex = 45;
            groupBox2.TabStop = false;
            groupBox2.Text = "Philatelic Quantity";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.White;
            label15.Location = new Point(28, 264);
            label15.Name = "label15";
            label15.Size = new Size(71, 21);
            label15.TabIndex = 201;
            label15.Text = "Remarks";
            // 
            // Remark_txt
            // 
            Remark_txt.Location = new Point(28, 288);
            Remark_txt.Multiline = true;
            Remark_txt.Name = "Remark_txt";
            Remark_txt.Size = new Size(612, 124);
            Remark_txt.TabIndex = 200;
            Remark_txt.Tag = "Skip";
            // 
            // combo_DT
            // 
            combo_DT.FormattingEnabled = true;
            combo_DT.ItemHeight = 21;
            combo_DT.Location = new Point(176, 197);
            combo_DT.Name = "combo_DT";
            combo_DT.Size = new Size(127, 29);
            combo_DT.TabIndex = 6;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.White;
            label8.Location = new Point(54, 205);
            label8.Name = "label8";
            label8.Size = new Size(102, 21);
            label8.TabIndex = 36;
            label8.Text = "DispatchType";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.White;
            label9.Location = new Point(322, 200);
            label9.Name = "label9";
            label9.Size = new Size(102, 21);
            label9.TabIndex = 34;
            label9.Text = "Total Amount";
            // 
            // text_TA
            // 
            text_TA.Enabled = false;
            text_TA.Location = new Point(486, 192);
            text_TA.Name = "text_TA";
            text_TA.ReadOnly = true;
            text_TA.Size = new Size(127, 29);
            text_TA.TabIndex = 199;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(253, 126);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 3);
            flowLayoutPanel1.TabIndex = 90;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(0, 165, 255);
            label7.Location = new Point(204, 20);
            label7.Name = "label7";
            label7.Size = new Size(398, 87);
            label7.TabIndex = 91;
            label7.Text = "Invoice Entry";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(183, 147);
            label14.Name = "label14";
            label14.Size = new Size(148, 25);
            label14.TabIndex = 92;
            label14.Text = "Last Invoice No";
            // 
            // lastInvoice_No
            // 
            lastInvoice_No.AutoSize = true;
            lastInvoice_No.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lastInvoice_No.Location = new Point(369, 147);
            lastInvoice_No.Name = "lastInvoice_No";
            lastInvoice_No.Size = new Size(76, 25);
            lastInvoice_No.TabIndex = 93;
            lastInvoice_No.Text = "label15";
            // 
            // InvoiceEntrycs
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(815, 1060);
            Controls.Add(lastInvoice_No);
            Controls.Add(label14);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label7);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(btn_save);
            Name = "InvoiceEntrycs";
            Text = "InvoiceEntrycs";
            Load += InvoiceEntrycs_Load;
            ((System.ComponentModel.ISupportInitialize)num_Stamp).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_Postmark).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder1;
        private Button btn_save;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private DateTimePicker date_Picker;
        private Label label6;
        private NumericUpDown num_Stamp;
        private NumericUpDown num_FDC;
        private NumericUpDown num_Leaflet;
        private NumericUpDown num_Postmark;
        private NumericUpDown num_FDCC;
        private GroupBox groupBox1;
        private Label label11;
        private TextBox text_PMPrice;
        private TextBox text_FDCPrice;
        private TextBox text_LeafletPrice;
        private Label label12;
        private Label label13;
        private GroupBox groupBox2;
        private ComboBox combo_DT;
        private Label label10;
        private TextBox text_TA;
        private Label label9;
        private Label label8;
        private ComboBox com_FileNo;
        private ComboBox com_PhilName;
        private Label label16;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label7;
        private Label label14;
        private Label lastInvoice_No;
        private Label label15;
        private TextBox Remark_txt;
    }
}