namespace FileIndex
{
    partial class PhilatelicSupply
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
            flowLayoutPanel2 = new FlowLayoutPanel();
            label1 = new Label();
            com_Address = new ComboBox();
            label2 = new Label();
            text_Stamp_B = new TextBox();
            label3 = new Label();
            label4 = new Label();
            text_FDC_B = new TextBox();
            label5 = new Label();
            text_PM_B = new TextBox();
            label6 = new Label();
            text_FDCC_B = new TextBox();
            groupBox1 = new GroupBox();
            text_Leaflet_B = new TextBox();
            label11 = new Label();
            num_Stamp = new NumericUpDown();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            num_PM = new NumericUpDown();
            num_FDCC = new NumericUpDown();
            num_FDC = new NumericUpDown();
            num_Leaflet = new NumericUpDown();
            label12 = new Label();
            com_FileNo = new ComboBox();
            label13 = new Label();
            com_ST = new ComboBox();
            label14 = new Label();
            button1 = new Button();
            date_Supply = new DateTimePicker();
            label15 = new Label();
            text_remark = new TextBox();
            label16 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_Stamp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_PM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).BeginInit();
            SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel2.Location = new Point(239, 126);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(300, 3);
            flowLayoutPanel2.TabIndex = 111;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(0, 165, 255);
            label1.Location = new Point(190, 20);
            label1.Name = "label1";
            label1.Size = new Size(398, 87);
            label1.TabIndex = 112;
            label1.Text = "Philatelic Supply ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // com_Address
            // 
            com_Address.DropDownStyle = ComboBoxStyle.DropDownList;
            com_Address.FormattingEnabled = true;
            com_Address.Location = new Point(114, 202);
            com_Address.Name = "com_Address";
            com_Address.Size = new Size(527, 29);
            com_Address.TabIndex = 3;
            com_Address.SelectedIndexChanged += com_IssueNo_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Black;
            label2.Location = new Point(20, 210);
            label2.Name = "label2";
            label2.Size = new Size(75, 21);
            label2.TabIndex = 110;
            label2.Text = "Address:-";
            // 
            // text_Stamp_B
            // 
            text_Stamp_B.Location = new Point(134, 89);
            text_Stamp_B.Name = "text_Stamp_B";
            text_Stamp_B.ReadOnly = true;
            text_Stamp_B.Size = new Size(145, 29);
            text_Stamp_B.TabIndex = 113;
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
            // text_FDC_B
            // 
            text_FDC_B.Location = new Point(134, 126);
            text_FDC_B.Name = "text_FDC_B";
            text_FDC_B.ReadOnly = true;
            text_FDC_B.Size = new Size(145, 29);
            text_FDC_B.TabIndex = 115;
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
            // text_PM_B
            // 
            text_PM_B.Location = new Point(134, 237);
            text_PM_B.Name = "text_PM_B";
            text_PM_B.ReadOnly = true;
            text_PM_B.Size = new Size(145, 29);
            text_PM_B.TabIndex = 117;
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
            // text_FDCC_B
            // 
            text_FDCC_B.Location = new Point(134, 200);
            text_FDCC_B.Name = "text_FDCC_B";
            text_FDCC_B.ReadOnly = true;
            text_FDCC_B.Size = new Size(145, 29);
            text_FDCC_B.TabIndex = 119;
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
            groupBox1.Location = new Point(655, 49);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(289, 349);
            groupBox1.TabIndex = 121;
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
            // num_Stamp
            // 
            num_Stamp.Location = new Point(49, 356);
            num_Stamp.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_Stamp.Name = "num_Stamp";
            num_Stamp.Size = new Size(94, 29);
            num_Stamp.TabIndex = 4;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.Black;
            label7.Location = new Point(379, 320);
            label7.Name = "label7";
            label7.Size = new Size(102, 21);
            label7.TabIndex = 128;
            label7.Text = "FDC(Caned):-";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.Black;
            label8.Location = new Point(489, 320);
            label8.Name = "label8";
            label8.Size = new Size(84, 21);
            label8.TabIndex = 127;
            label8.Text = "Postmark:-";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Black;
            label9.Location = new Point(49, 320);
            label9.Name = "label9";
            label9.Size = new Size(63, 21);
            label9.TabIndex = 125;
            label9.Text = "Stamp:-";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = Color.Black;
            label10.Location = new Point(159, 320);
            label10.Name = "label10";
            label10.Size = new Size(48, 21);
            label10.TabIndex = 126;
            label10.Text = "FDC:-";
            // 
            // num_PM
            // 
            num_PM.Location = new Point(489, 356);
            num_PM.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_PM.Name = "num_PM";
            num_PM.Size = new Size(94, 29);
            num_PM.TabIndex = 8;
            // 
            // num_FDCC
            // 
            num_FDCC.Location = new Point(379, 356);
            num_FDCC.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_FDCC.Name = "num_FDCC";
            num_FDCC.Size = new Size(94, 29);
            num_FDCC.TabIndex = 7;
            // 
            // num_FDC
            // 
            num_FDC.Location = new Point(159, 356);
            num_FDC.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_FDC.Name = "num_FDC";
            num_FDC.Size = new Size(94, 29);
            num_FDC.TabIndex = 5;
            // 
            // num_Leaflet
            // 
            num_Leaflet.Location = new Point(269, 356);
            num_Leaflet.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            num_Leaflet.Name = "num_Leaflet";
            num_Leaflet.Size = new Size(94, 29);
            num_Leaflet.TabIndex = 6;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.Black;
            label12.Location = new Point(269, 320);
            label12.Name = "label12";
            label12.Size = new Size(65, 21);
            label12.TabIndex = 132;
            label12.Text = "Leaflet:-";
            // 
            // com_FileNo
            // 
            com_FileNo.DropDownStyle = ComboBoxStyle.DropDownList;
            com_FileNo.FormattingEnabled = true;
            com_FileNo.Location = new Point(114, 143);
            com_FileNo.Name = "com_FileNo";
            com_FileNo.Size = new Size(126, 29);
            com_FileNo.TabIndex = 0;
            com_FileNo.SelectedIndexChanged += com_FileNo_SelectedIndexChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Black;
            label13.Location = new Point(20, 146);
            label13.Name = "label13";
            label13.Size = new Size(88, 21);
            label13.TabIndex = 100;
            label13.Text = "Select File:-";
            // 
            // com_ST
            // 
            com_ST.DropDownStyle = ComboBoxStyle.DropDownList;
            com_ST.FormattingEnabled = true;
            com_ST.Location = new Point(345, 143);
            com_ST.Name = "com_ST";
            com_ST.Size = new Size(108, 29);
            com_ST.TabIndex = 1;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = Color.Black;
            label14.Location = new Point(240, 146);
            label14.Name = "label14";
            label14.Size = new Size(99, 21);
            label14.TabIndex = 137;
            label14.Text = "SupplyType:-";
            // 
            // button1
            // 
            button1.Location = new Point(395, 609);
            button1.Name = "button1";
            button1.Size = new Size(209, 57);
            button1.TabIndex = 10;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // date_Supply
            // 
            date_Supply.Format = DateTimePickerFormat.Short;
            date_Supply.Location = new Point(513, 143);
            date_Supply.Name = "date_Supply";
            date_Supply.Size = new Size(128, 29);
            date_Supply.TabIndex = 2;
            date_Supply.Value = new DateTime(2026, 1, 1, 0, 0, 0, 0);
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.Black;
            label15.Location = new Point(459, 146);
            label15.Name = "label15";
            label15.Size = new Size(48, 21);
            label15.TabIndex = 140;
            label15.Text = "Date-";
            // 
            // text_remark
            // 
            text_remark.Location = new Point(43, 436);
            text_remark.Multiline = true;
            text_remark.Name = "text_remark";
            text_remark.Size = new Size(540, 120);
            text_remark.TabIndex = 9;
            text_remark.Tag = "Skip";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = Color.Black;
            label16.Location = new Point(43, 412);
            label16.Name = "label16";
            label16.Size = new Size(73, 21);
            label16.TabIndex = 142;
            label16.Text = "Remark:-";
            // 
            // PhilatelicSupply
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(999, 664);
            Controls.Add(label16);
            Controls.Add(text_remark);
            Controls.Add(label15);
            Controls.Add(date_Supply);
            Controls.Add(button1);
            Controls.Add(com_ST);
            Controls.Add(label14);
            Controls.Add(com_FileNo);
            Controls.Add(label13);
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
            Controls.Add(groupBox1);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(label1);
            Controls.Add(com_Address);
            Controls.Add(label2);
            Name = "PhilatelicSupply";
            Text = "PhilatelicSupply";
            Load += PhilatelicSupply_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_Stamp).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_PM).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDCC).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_FDC).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_Leaflet).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1;
        private ComboBox com_Address;
        private Label label2;
        private TextBox text_Stamp_B;
        private Label label3;
        private Label label4;
        private TextBox text_FDC_B;
        private Label label5;
        private TextBox text_PM_B;
        private Label label6;
        private TextBox text_FDCC_B;
        private GroupBox groupBox1;
        private NumericUpDown num_Stamp;
        private TextBox text_Leaflet_B;
        private Label label11;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private NumericUpDown num_PM;
        private NumericUpDown num_FDCC;
        private NumericUpDown num_FDC;
        private NumericUpDown num_Leaflet;
        private Label label12;
        private ComboBox com_FileNo;
        private Label label13;
        private ComboBox com_ST;
        private Label label14;
        private Button button1;
        private DateTimePicker date_Supply;
        private Label label15;
        private TextBox text_remark;
        private Label label16;
    }
}