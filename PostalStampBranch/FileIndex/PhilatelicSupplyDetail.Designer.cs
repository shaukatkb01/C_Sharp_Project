namespace FileIndex
{
    partial class PhilatelicSupplyDetail
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
            com_FileNo = new ComboBox();
            label13 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            groupBox1 = new GroupBox();
            text_Leaflet_B = new TextBox();
            label11 = new Label();
            text_Stamp_B = new TextBox();
            text_PM_B = new TextBox();
            text_FDCC_B = new TextBox();
            label6 = new Label();
            label5 = new Label();
            text_FDC_B = new TextBox();
            label3 = new Label();
            label4 = new Label();
            text_remark = new TextBox();
            label15 = new Label();
            date_Supply = new DateTimePicker();
            com_ST = new ComboBox();
            label14 = new Label();
            num_Leaflet = new NumericUpDown();
            label12 = new Label();
            num_FDC = new NumericUpDown();
            num_FDCC = new NumericUpDown();
            num_PM = new NumericUpDown();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            num_Stamp = new NumericUpDown();
            com_Address = new ComboBox();
            label2 = new Label();
            label16 = new Label();
            lbl_SelectedID = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_PM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_Stamp).BeginInit();
            SuspendLayout();
            // 
            // com_FileNo
            // 
            com_FileNo.DropDownStyle = ComboBoxStyle.DropDownList;
            com_FileNo.FormattingEnabled = true;
            com_FileNo.Location = new Point(104, 170);
            com_FileNo.Name = "com_FileNo";
            com_FileNo.Size = new Size(128, 29);
            com_FileNo.TabIndex = 1;
            com_FileNo.SelectedIndexChanged += com_FileNo_SelectedIndexChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Black;
            label13.Location = new Point(10, 173);
            label13.Name = "label13";
            label13.Size = new Size(88, 21);
            label13.TabIndex = 113;
            label13.Text = "Select File:-";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel2.Location = new Point(237, 109);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(300, 3);
            flowLayoutPanel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(0, 165, 255);
            label1.Location = new Point(184, 30);
            label1.Name = "label1";
            label1.Size = new Size(398, 87);
            label1.TabIndex = 116;
            label1.Text = "Correction in Philatelic Supply";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.Location = new Point(-320, 462);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1374, 169);
            dataGridView1.TabIndex = 0;
            dataGridView1.AllowUserToAddRowsChanged += com_FileNo_SelectedIndexChanged;
            dataGridView1.CellContentClick += dataGridView1_CellClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(text_Leaflet_B);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(text_Stamp_B);
            groupBox1.Controls.Add(text_PM_B);
            groupBox1.Controls.Add(text_FDCC_B);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(text_FDC_B);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new Point(711, 58);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(323, 312);
            groupBox1.TabIndex = 122;
            groupBox1.TabStop = false;
            groupBox1.Text = "Balance";
            // 
            // text_Leaflet_B
            // 
            text_Leaflet_B.Location = new Point(136, 163);
            text_Leaflet_B.Name = "text_Leaflet_B";
            text_Leaflet_B.ReadOnly = true;
            text_Leaflet_B.Size = new Size(145, 29);
            text_Leaflet_B.TabIndex = 121;
            text_Leaflet_B.TabStop = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = Color.Black;
            label11.Location = new Point(8, 171);
            label11.Name = "label11";
            label11.Size = new Size(62, 21);
            label11.TabIndex = 122;
            label11.Text = "Leaflet-";
            // 
            // text_Stamp_B
            // 
            text_Stamp_B.Location = new Point(134, 89);
            text_Stamp_B.Name = "text_Stamp_B";
            text_Stamp_B.ReadOnly = true;
            text_Stamp_B.Size = new Size(145, 29);
            text_Stamp_B.TabIndex = 113;
            text_Stamp_B.TabStop = false;
            // 
            // text_PM_B
            // 
            text_PM_B.Location = new Point(134, 237);
            text_PM_B.Name = "text_PM_B";
            text_PM_B.ReadOnly = true;
            text_PM_B.Size = new Size(145, 29);
            text_PM_B.TabIndex = 117;
            text_PM_B.TabStop = false;
            // 
            // text_FDCC_B
            // 
            text_FDCC_B.Location = new Point(134, 200);
            text_FDCC_B.Name = "text_FDCC_B";
            text_FDCC_B.ReadOnly = true;
            text_FDCC_B.Size = new Size(145, 29);
            text_FDCC_B.TabIndex = 119;
            text_FDCC_B.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Black;
            label6.Location = new Point(6, 208);
            label6.Name = "label6";
            label6.Size = new Size(125, 21);
            label6.TabIndex = 120;
            label6.Text = "FDC(Cancelled):-";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Black;
            label5.Location = new Point(6, 245);
            label5.Name = "label5";
            label5.Size = new Size(84, 21);
            label5.TabIndex = 118;
            label5.Text = "Postmark:-";
            // 
            // text_FDC_B
            // 
            text_FDC_B.Location = new Point(134, 126);
            text_FDC_B.Name = "text_FDC_B";
            text_FDC_B.ReadOnly = true;
            text_FDC_B.Size = new Size(145, 29);
            text_FDC_B.TabIndex = 115;
            text_FDC_B.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Black;
            label3.Location = new Point(6, 97);
            label3.Name = "label3";
            label3.Size = new Size(63, 21);
            label3.TabIndex = 114;
            label3.Text = "Stamp:-";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Black;
            label4.Location = new Point(6, 134);
            label4.Name = "label4";
            label4.Size = new Size(48, 21);
            label4.TabIndex = 116;
            label4.Text = "FDC:-";
            // 
            // text_remark
            // 
            text_remark.Location = new Point(104, 349);
            text_remark.Multiline = true;
            text_remark.Name = "text_remark";
            text_remark.PlaceholderText = "Insert Remark if any";
            text_remark.Size = new Size(579, 107);
            text_remark.TabIndex = 149;
            text_remark.Tag = "PhilControll";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.Black;
            label15.Location = new Point(501, 173);
            label15.Name = "label15";
            label15.Size = new Size(48, 21);
            label15.TabIndex = 157;
            label15.Text = "Date-";
            // 
            // date_Supply
            // 
            date_Supply.Format = DateTimePickerFormat.Short;
            date_Supply.Location = new Point(555, 170);
            date_Supply.Name = "date_Supply";
            date_Supply.Size = new Size(128, 29);
            date_Supply.TabIndex = 142;
            date_Supply.Tag = "PhilControll";
            date_Supply.Value = new DateTime(2026, 1, 1, 0, 0, 0, 0);
            // 
            // com_ST
            // 
            com_ST.DropDownStyle = ComboBoxStyle.DropDownList;
            com_ST.FormattingEnabled = true;
            com_ST.Location = new Point(351, 170);
            com_ST.Name = "com_ST";
            com_ST.Size = new Size(128, 29);
            com_ST.TabIndex = 141;
            com_ST.Tag = "PhilControll";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = Color.Black;
            label14.Location = new Point(246, 173);
            label14.Name = "label14";
            label14.Size = new Size(99, 21);
            label14.TabIndex = 156;
            label14.Text = "SupplyType:-";
            // 
            // num_Leaflet
            // 
            num_Leaflet.Location = new Point(363, 295);
            num_Leaflet.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_Leaflet.Name = "num_Leaflet";
            num_Leaflet.Size = new Size(94, 29);
            num_Leaflet.TabIndex = 146;
            num_Leaflet.Tag = "PhilControll";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.Black;
            label12.Location = new Point(363, 259);
            label12.Name = "label12";
            label12.Size = new Size(65, 21);
            label12.TabIndex = 155;
            label12.Text = "Leaflet:-";
            // 
            // num_FDC
            // 
            num_FDC.Location = new Point(253, 295);
            num_FDC.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_FDC.Name = "num_FDC";
            num_FDC.Size = new Size(94, 29);
            num_FDC.TabIndex = 145;
            num_FDC.Tag = "PhilControll";
            // 
            // num_FDCC
            // 
            num_FDCC.Location = new Point(473, 295);
            num_FDCC.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_FDCC.Name = "num_FDCC";
            num_FDCC.Size = new Size(94, 29);
            num_FDCC.TabIndex = 147;
            num_FDCC.Tag = "PhilControll";
            // 
            // num_PM
            // 
            num_PM.Location = new Point(583, 295);
            num_PM.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_PM.Name = "num_PM";
            num_PM.Size = new Size(94, 29);
            num_PM.TabIndex = 148;
            num_PM.Tag = "PhilControll";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.Black;
            label7.Location = new Point(473, 259);
            label7.Name = "label7";
            label7.Size = new Size(102, 21);
            label7.TabIndex = 154;
            label7.Text = "FDC(Caned):-";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.Black;
            label8.Location = new Point(583, 259);
            label8.Name = "label8";
            label8.Size = new Size(84, 21);
            label8.TabIndex = 153;
            label8.Text = "Postmark:-";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Black;
            label9.Location = new Point(143, 259);
            label9.Name = "label9";
            label9.Size = new Size(63, 21);
            label9.TabIndex = 151;
            label9.Text = "Stamp:-";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.Black;
            label10.Location = new Point(253, 259);
            label10.Name = "label10";
            label10.Size = new Size(48, 21);
            label10.TabIndex = 152;
            label10.Text = "FDC:-";
            // 
            // num_Stamp
            // 
            num_Stamp.Location = new Point(143, 295);
            num_Stamp.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_Stamp.Name = "num_Stamp";
            num_Stamp.Size = new Size(94, 29);
            num_Stamp.TabIndex = 144;
            num_Stamp.Tag = "PhilControll";
            // 
            // com_Address
            // 
            com_Address.DropDownStyle = ComboBoxStyle.DropDownList;
            com_Address.FormattingEnabled = true;
            com_Address.Location = new Point(104, 217);
            com_Address.Name = "com_Address";
            com_Address.Size = new Size(579, 29);
            com_Address.TabIndex = 143;
            com_Address.Tag = "PhilControll";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Black;
            label2.Location = new Point(10, 220);
            label2.Name = "label2";
            label2.Size = new Size(75, 21);
            label2.TabIndex = 150;
            label2.Text = "Address:-";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = Color.Black;
            label16.Location = new Point(104, 327);
            label16.Name = "label16";
            label16.Size = new Size(73, 21);
            label16.TabIndex = 158;
            label16.Text = "Remark:-";
            // 
            // lbl_SelectedID
            // 
            lbl_SelectedID.AutoSize = true;
            lbl_SelectedID.ForeColor = Color.Black;
            lbl_SelectedID.Location = new Point(22, 47);
            lbl_SelectedID.Name = "lbl_SelectedID";
            lbl_SelectedID.Size = new Size(0, 21);
            lbl_SelectedID.TabIndex = 159;
            lbl_SelectedID.Visible = false;
            // 
            // button1
            // 
            button1.Location = new Point(784, 376);
            button1.Name = "button1";
            button1.Size = new Size(209, 57);
            button1.TabIndex = 150;
            button1.Tag = "PhilControll";
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // PhilatelicSupplyDetail
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1149, 665);
            Controls.Add(button1);
            Controls.Add(lbl_SelectedID);
            Controls.Add(label16);
            Controls.Add(text_remark);
            Controls.Add(label15);
            Controls.Add(date_Supply);
            Controls.Add(com_ST);
            Controls.Add(label14);
            Controls.Add(num_Leaflet);
            Controls.Add(label12);
            Controls.Add(num_FDC);
            Controls.Add(num_FDCC);
            Controls.Add(num_PM);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label10);
            Controls.Add(num_Stamp);
            Controls.Add(com_Address);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Controls.Add(dataGridView1);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(label1);
            Controls.Add(com_FileNo);
            Controls.Add(label13);
            Name = "PhilatelicSupplyDetail";
            Tag = "149";
            Load += PhilatelicSupplyDetail_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_PM).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_Stamp).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox com_FileNo;
        private Label label13;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1;
        private DataGridView dataGridView1;
        private GroupBox groupBox1;
        private TextBox text_Leaflet_B;
        private Label label11;
        private TextBox text_Stamp_B;
        private TextBox text_PM_B;
        private TextBox text_FDCC_B;
        private Label label6;
        private Label label5;
        private TextBox text_FDC_B;
        private Label label3;
        private Label label4;
        private TextBox text_remark;
        private Label label15;
        private DateTimePicker date_Supply;
        private ComboBox com_ST;
        private Label label14;
        private NumericUpDown num_Leaflet;
        private Label label12;
        private NumericUpDown num_FDC;
        private NumericUpDown num_FDCC;
        private NumericUpDown num_PM;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private NumericUpDown num_Stamp;
        private ComboBox com_Address;
        private Label label2;
        private Label label16;
        private Label lbl_SelectedID;
        private Button button1;
    }
}