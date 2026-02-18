namespace FileIndex
{
    partial class InvoiceAcknowlodge
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
            label38 = new Label();
            cmb_InvoiceNo = new ComboBox();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            txt_PageNo = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txt_Remark = new TextBox();
            button1 = new Button();
            Picker_RDate = new DateTimePicker();
            btn_update = new Button();
            groupBox1 = new GroupBox();
            radio_Accepted = new RadioButton();
            radio_Pending = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(418, 70);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(463, 3);
            flowLayoutPanel1.TabIndex = 461;
            // 
            // label38
            // 
            label38.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label38.BackColor = Color.Transparent;
            label38.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label38.ForeColor = Color.FromArgb(0, 165, 255);
            label38.Location = new Point(325, 9);
            label38.Name = "label38";
            label38.Size = new Size(661, 46);
            label38.TabIndex = 462;
            label38.Text = "Invoice Acknowlodge";
            label38.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmb_InvoiceNo
            // 
            cmb_InvoiceNo.FormattingEnabled = true;
            cmb_InvoiceNo.Location = new Point(176, 100);
            cmb_InvoiceNo.Name = "cmb_InvoiceNo";
            cmb_InvoiceNo.Size = new Size(137, 29);
            cmb_InvoiceNo.TabIndex = 463;
            cmb_InvoiceNo.SelectedIndexChanged += cmb_InvoiceNo_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 103);
            label1.Name = "label1";
            label1.Size = new Size(129, 21);
            label1.TabIndex = 464;
            label1.Text = "Select Invoice No";
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(4, 270);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1045, 168);
            dataGridView1.TabIndex = 465;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // txt_PageNo
            // 
            txt_PageNo.Location = new Point(155, 189);
            txt_PageNo.Name = "txt_PageNo";
            txt_PageNo.Size = new Size(108, 29);
            txt_PageNo.TabIndex = 467;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(167, 144);
            label2.Name = "label2";
            label2.Size = new Size(68, 21);
            label2.TabIndex = 468;
            label2.Text = "Page No";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 144);
            label3.Name = "label3";
            label3.Size = new Size(111, 21);
            label3.TabIndex = 469;
            label3.Text = "Receiving date";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(298, 141);
            label4.Name = "label4";
            label4.Size = new Size(64, 21);
            label4.TabIndex = 471;
            label4.Text = "Remark";
            // 
            // txt_Remark
            // 
            txt_Remark.Location = new Point(286, 186);
            txt_Remark.Multiline = true;
            txt_Remark.Name = "txt_Remark";
            txt_Remark.Size = new Size(497, 60);
            txt_Remark.TabIndex = 470;
            // 
            // button1
            // 
            button1.Location = new Point(803, 201);
            button1.Name = "button1";
            button1.Size = new Size(228, 45);
            button1.TabIndex = 472;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Picker_RDate
            // 
            Picker_RDate.Format = DateTimePickerFormat.Short;
            Picker_RDate.Location = new Point(10, 188);
            Picker_RDate.Name = "Picker_RDate";
            Picker_RDate.Size = new Size(134, 29);
            Picker_RDate.TabIndex = 473;
            Picker_RDate.ValueChanged += Picker_RDate_ValueChanged;
            // 
            // btn_update
            // 
            btn_update.Location = new Point(803, 132);
            btn_update.Name = "btn_update";
            btn_update.Size = new Size(228, 45);
            btn_update.TabIndex = 474;
            btn_update.Text = "Update";
            btn_update.UseVisualStyleBackColor = true;
            btn_update.Click += btn_update_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radio_Accepted);
            groupBox1.Controls.Add(radio_Pending);
            groupBox1.Location = new Point(15, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(335, 69);
            groupBox1.TabIndex = 475;
            groupBox1.TabStop = false;
            groupBox1.Text = "Select Invoice Type";
            // 
            // radio_Accepted
            // 
            radio_Accepted.AutoSize = true;
            radio_Accepted.Location = new Point(185, 36);
            radio_Accepted.Name = "radio_Accepted";
            radio_Accepted.Size = new Size(91, 25);
            radio_Accepted.TabIndex = 1;
            radio_Accepted.TabStop = true;
            radio_Accepted.Text = "Accepted";
            radio_Accepted.UseVisualStyleBackColor = true;
            radio_Accepted.CheckedChanged += radio_Accepted_CheckedChanged;
            // 
            // radio_Pending
            // 
            radio_Pending.AutoSize = true;
            radio_Pending.Location = new Point(13, 36);
            radio_Pending.Name = "radio_Pending";
            radio_Pending.Size = new Size(84, 25);
            radio_Pending.TabIndex = 0;
            radio_Pending.TabStop = true;
            radio_Pending.Text = "Pending";
            radio_Pending.UseVisualStyleBackColor = true;
            radio_Pending.CheckedChanged += radio_Pending_CheckedChanged;
            // 
            // InvoiceAcknowlodge
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1063, 450);
            Controls.Add(groupBox1);
            Controls.Add(btn_update);
            Controls.Add(Picker_RDate);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(txt_Remark);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txt_PageNo);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Controls.Add(cmb_InvoiceNo);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label38);
            Name = "InvoiceAcknowlodge";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "InvoiceAcknowlodge";
            WindowState = FormWindowState.Maximized;
            Load += InvoiceAcknowlodge_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Label label38;
        private ComboBox cmb_InvoiceNo;
        private Label label1;
        private DataGridView dataGridView1;
        private TextBox txt_PageNo;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txt_Remark;
        private Button button1;
        private DateTimePicker Picker_RDate;
        private Button btn_update;
        private GroupBox groupBox1;
        private RadioButton radio_Accepted;
        private RadioButton radio_Pending;
    }
}