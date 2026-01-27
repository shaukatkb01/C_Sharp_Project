namespace FileIndex
{
    partial class InvoiceCorection
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            label7 = new Label();
            sqlCommandBuilder1 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            panel1 = new Panel();
            panel2 = new Panel();
            groupBox1 = new GroupBox();
            com_InvoiceNo = new ComboBox();
            label17 = new Label();
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
            btn_save = new Button();
            groupBox2 = new GroupBox();
            label15 = new Label();
            Remark_txt = new TextBox();
            num_Stamp = new NumericUpDown();
            label1 = new Label();
            num_FDCC = new NumericUpDown();
            combo_DT = new ComboBox();
            label2 = new Label();
            label8 = new Label();
            num_Postmark = new NumericUpDown();
            label3 = new Label();
            num_Leaflet = new NumericUpDown();
            label9 = new Label();
            label4 = new Label();
            text_TA = new TextBox();
            num_FDC = new NumericUpDown();
            label5 = new Label();
            date_Picker = new DateTimePicker();
            label6 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_Stamp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_Postmark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).BeginInit();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(200, 58);
            flowLayoutPanel1.Margin = new Padding(2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(233, 2);
            flowLayoutPanel1.TabIndex = 97;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(0, 165, 255);
            label7.Location = new Point(154, -2);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(310, 62);
            label7.TabIndex = 98;
            label7.Text = "Invoice Corection";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(label7);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(650, 71);
            panel1.TabIndex = 97;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(groupBox1);
            panel2.Controls.Add(btn_save);
            panel2.Controls.Add(groupBox2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 71);
            panel2.Name = "panel2";
            panel2.Size = new Size(650, 678);
            panel2.TabIndex = 98;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(com_InvoiceNo);
            groupBox1.Controls.Add(label17);
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
            groupBox1.Location = new Point(37, 20);
            groupBox1.Margin = new Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2);
            groupBox1.Size = new Size(519, 275);
            groupBox1.TabIndex = 100;
            groupBox1.TabStop = false;
            groupBox1.Text = "Price";
            // 
            // com_InvoiceNo
            // 
            com_InvoiceNo.DropDownStyle = ComboBoxStyle.DropDownList;
            com_InvoiceNo.FormattingEnabled = true;
            com_InvoiceNo.Location = new Point(120, 44);
            com_InvoiceNo.Margin = new Padding(2);
            com_InvoiceNo.Name = "com_InvoiceNo";
            com_InvoiceNo.Size = new Size(138, 23);
            com_InvoiceNo.TabIndex = 52;
            com_InvoiceNo.SelectedIndexChanged += com_InvoiceNo_SelectedIndexChanged_1;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = Color.Black;
            label17.Location = new Point(8, 48);
            label17.Margin = new Padding(2, 0, 2, 0);
            label17.Name = "label17";
            label17.Size = new Size(106, 15);
            label17.TabIndex = 53;
            label17.Text = "Select Invoice No:-";
            // 
            // com_PhilName
            // 
            com_PhilName.DropDownStyle = ComboBoxStyle.DropDownList;
            com_PhilName.FormattingEnabled = true;
            com_PhilName.Location = new Point(120, 111);
            com_PhilName.Margin = new Padding(2);
            com_PhilName.Name = "com_PhilName";
            com_PhilName.Size = new Size(365, 23);
            com_PhilName.TabIndex = 2;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = Color.Black;
            label16.Location = new Point(3, 115);
            label16.Margin = new Padding(2, 0, 2, 0);
            label16.Name = "label16";
            label16.Size = new Size(72, 15);
            label16.TabIndex = 51;
            label16.Text = "Address To:-";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = Color.Black;
            label11.Location = new Point(3, 155);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(58, 15);
            label11.TabIndex = 50;
            label11.Text = "FDC Price";
            // 
            // text_PMPrice
            // 
            text_PMPrice.Enabled = false;
            text_PMPrice.Location = new Point(120, 207);
            text_PMPrice.Margin = new Padding(2);
            text_PMPrice.Name = "text_PMPrice";
            text_PMPrice.ReadOnly = true;
            text_PMPrice.Size = new Size(100, 23);
            text_PMPrice.TabIndex = 47;
            // 
            // com_FileNo
            // 
            com_FileNo.DropDownStyle = ComboBoxStyle.DropDownList;
            com_FileNo.Enabled = false;
            com_FileNo.FormattingEnabled = true;
            com_FileNo.Location = new Point(120, 77);
            com_FileNo.Margin = new Padding(2);
            com_FileNo.Name = "com_FileNo";
            com_FileNo.Size = new Size(138, 23);
            com_FileNo.TabIndex = 1;
            // 
            // text_FDCPrice
            // 
            text_FDCPrice.Enabled = false;
            text_FDCPrice.Location = new Point(120, 136);
            text_FDCPrice.Margin = new Padding(2);
            text_FDCPrice.Name = "text_FDCPrice";
            text_FDCPrice.ReadOnly = true;
            text_FDCPrice.Size = new Size(100, 23);
            text_FDCPrice.TabIndex = 49;
            // 
            // text_LeafletPrice
            // 
            text_LeafletPrice.Enabled = false;
            text_LeafletPrice.Location = new Point(120, 171);
            text_LeafletPrice.Margin = new Padding(2);
            text_LeafletPrice.Name = "text_LeafletPrice";
            text_LeafletPrice.ReadOnly = true;
            text_LeafletPrice.Size = new Size(100, 23);
            text_LeafletPrice.TabIndex = 45;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.Black;
            label10.Location = new Point(8, 82);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(49, 15);
            label10.TabIndex = 20;
            label10.Text = "FileNo:-";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.Black;
            label12.Location = new Point(8, 218);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new Size(86, 15);
            label12.TabIndex = 48;
            label12.Text = "Postmark Price";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Black;
            label13.Location = new Point(3, 191);
            label13.Margin = new Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.Size = new Size(71, 15);
            label13.TabIndex = 46;
            label13.Text = "Leaflet Price";
            // 
            // btn_save
            // 
            btn_save.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_save.Location = new Point(230, 634);
            btn_save.Margin = new Padding(2);
            btn_save.Name = "btn_save";
            btn_save.Size = new Size(122, 27);
            btn_save.TabIndex = 99;
            btn_save.Text = "Save";
            btn_save.UseVisualStyleBackColor = true;
            btn_save.Click += btn_save_Click_1;
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
            groupBox2.Location = new Point(37, 312);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(519, 306);
            groupBox2.TabIndex = 98;
            groupBox2.TabStop = false;
            groupBox2.Text = "Philatelic Quantity";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.Black;
            label15.Location = new Point(21, 188);
            label15.Margin = new Padding(2, 0, 2, 0);
            label15.Name = "label15";
            label15.Size = new Size(52, 15);
            label15.TabIndex = 201;
            label15.Text = "Remarks";
            // 
            // Remark_txt
            // 
            Remark_txt.Location = new Point(22, 206);
            Remark_txt.Margin = new Padding(2);
            Remark_txt.Multiline = true;
            Remark_txt.Name = "Remark_txt";
            Remark_txt.Size = new Size(477, 90);
            Remark_txt.TabIndex = 200;
            Remark_txt.Tag = "Skip";
            // 
            // num_Stamp
            // 
            num_Stamp.Location = new Point(135, 52);
            num_Stamp.Margin = new Padding(2);
            num_Stamp.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_Stamp.Name = "num_Stamp";
            num_Stamp.Size = new Size(99, 23);
            num_Stamp.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Black;
            label1.Location = new Point(39, 57);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(63, 15);
            label1.TabIndex = 22;
            label1.Text = "Stamp Qty";
            // 
            // num_FDCC
            // 
            num_FDCC.Location = new Point(376, 88);
            num_FDCC.Margin = new Padding(2);
            num_FDCC.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_FDCC.Name = "num_FDCC";
            num_FDCC.Size = new Size(99, 23);
            num_FDCC.TabIndex = 9;
            // 
            // combo_DT
            // 
            combo_DT.FormattingEnabled = true;
            combo_DT.ItemHeight = 15;
            combo_DT.Location = new Point(135, 124);
            combo_DT.Margin = new Padding(2);
            combo_DT.Name = "combo_DT";
            combo_DT.Size = new Size(100, 23);
            combo_DT.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Black;
            label2.Location = new Point(39, 93);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(48, 15);
            label2.TabIndex = 24;
            label2.Text = "FDCQty";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.Black;
            label8.Location = new Point(39, 128);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(77, 15);
            label8.TabIndex = 36;
            label8.Text = "DispatchType";
            // 
            // num_Postmark
            // 
            num_Postmark.Location = new Point(376, 52);
            num_Postmark.Margin = new Padding(2);
            num_Postmark.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_Postmark.Name = "num_Postmark";
            num_Postmark.Size = new Size(99, 23);
            num_Postmark.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Black;
            label3.Location = new Point(248, 25);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 26;
            label3.Text = "Leaflet Qty";
            // 
            // num_Leaflet
            // 
            num_Leaflet.Location = new Point(376, 20);
            num_Leaflet.Margin = new Padding(2);
            num_Leaflet.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_Leaflet.Name = "num_Leaflet";
            num_Leaflet.Size = new Size(99, 23);
            num_Leaflet.TabIndex = 7;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Black;
            label9.Location = new Point(248, 125);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(79, 15);
            label9.TabIndex = 34;
            label9.Text = "Total Amount";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Black;
            label4.Location = new Point(248, 57);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(79, 15);
            label4.TabIndex = 28;
            label4.Text = "Postmark Qty";
            // 
            // text_TA
            // 
            text_TA.Enabled = false;
            text_TA.Location = new Point(376, 120);
            text_TA.Margin = new Padding(2);
            text_TA.Name = "text_TA";
            text_TA.ReadOnly = true;
            text_TA.Size = new Size(100, 23);
            text_TA.TabIndex = 199;
            text_TA.TextAlign = HorizontalAlignment.Right;
            // 
            // num_FDC
            // 
            num_FDC.Location = new Point(135, 88);
            num_FDC.Margin = new Padding(2);
            num_FDC.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_FDC.Name = "num_FDC";
            num_FDC.Size = new Size(99, 23);
            num_FDC.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Black;
            label5.Location = new Point(248, 93);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(106, 15);
            label5.TabIndex = 30;
            label5.Text = "FDC Cancelled Qty";
            // 
            // date_Picker
            // 
            date_Picker.CustomFormat = "27/12/2025";
            date_Picker.Format = DateTimePickerFormat.Short;
            date_Picker.Location = new Point(135, 20);
            date_Picker.Margin = new Padding(2);
            date_Picker.Name = "date_Picker";
            date_Picker.Size = new Size(100, 23);
            date_Picker.TabIndex = 3;
            date_Picker.Value = new DateTime(2025, 12, 27, 0, 0, 0, 0);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Black;
            label6.Location = new Point(39, 25);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(31, 15);
            label6.TabIndex = 32;
            label6.Text = "Date";
            // 
            // InvoiceCorection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 749);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new Padding(2);
            Name = "InvoiceCorection";
            Text = "InvoiceCorection";
            Load += InvoiceCorection_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_Stamp).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_Postmark).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label7;
        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder1;
        private Panel panel1;
        private Panel panel2;
        private GroupBox groupBox1;
        private ComboBox com_InvoiceNo;
        private Label label17;
        private ComboBox com_PhilName;
        private Label label16;
        private Label label11;
        private TextBox text_PMPrice;
        private ComboBox com_FileNo;
        private TextBox text_FDCPrice;
        private TextBox text_LeafletPrice;
        private Label label10;
        private Label label12;
        private Label label13;
        private Button btn_save;
        private GroupBox groupBox2;
        private Label label15;
        private TextBox Remark_txt;
        private NumericUpDown num_Stamp;
        private Label label1;
        private NumericUpDown num_FDCC;
        private ComboBox combo_DT;
        private Label label2;
        private Label label8;
        private NumericUpDown num_Postmark;
        private Label label3;
        private NumericUpDown num_Leaflet;
        private Label label9;
        private Label label4;
        private TextBox text_TA;
        private NumericUpDown num_FDC;
        private Label label5;
        private DateTimePicker date_Picker;
        private Label label6;
    }
}