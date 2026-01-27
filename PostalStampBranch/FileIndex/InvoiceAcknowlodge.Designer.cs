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
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            cmb_InvoiceNo.Location = new Point(158, 44);
            cmb_InvoiceNo.Name = "cmb_InvoiceNo";
            cmb_InvoiceNo.Size = new Size(137, 29);
            cmb_InvoiceNo.TabIndex = 463;
            cmb_InvoiceNo.SelectedIndexChanged += cmb_InvoiceNo_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 47);
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
            dataGridView1.Location = new Point(4, 227);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1045, 211);
            dataGridView1.TabIndex = 465;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // txt_PageNo
            // 
            txt_PageNo.Location = new Point(158, 164);
            txt_PageNo.Name = "txt_PageNo";
            txt_PageNo.Size = new Size(108, 29);
            txt_PageNo.TabIndex = 467;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(170, 119);
            label2.Name = "label2";
            label2.Size = new Size(68, 21);
            label2.TabIndex = 468;
            label2.Text = "Page No";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 119);
            label3.Name = "label3";
            label3.Size = new Size(111, 21);
            label3.TabIndex = 469;
            label3.Text = "Receiving date";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(301, 116);
            label4.Name = "label4";
            label4.Size = new Size(64, 21);
            label4.TabIndex = 471;
            label4.Text = "Remark";
            // 
            // txt_Remark
            // 
            txt_Remark.Location = new Point(289, 161);
            txt_Remark.Multiline = true;
            txt_Remark.Name = "txt_Remark";
            txt_Remark.Size = new Size(497, 60);
            txt_Remark.TabIndex = 470;
            // 
            // button1
            // 
            button1.Location = new Point(806, 176);
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
            Picker_RDate.Location = new Point(13, 163);
            Picker_RDate.Name = "Picker_RDate";
            Picker_RDate.Size = new Size(134, 29);
            Picker_RDate.TabIndex = 473;
            Picker_RDate.ValueChanged += Picker_RDate_ValueChanged;
            // 
            // btn_update
            // 
            btn_update.Location = new Point(806, 107);
            btn_update.Name = "btn_update";
            btn_update.Size = new Size(228, 45);
            btn_update.TabIndex = 474;
            btn_update.Text = "Update";
            btn_update.UseVisualStyleBackColor = true;
            btn_update.Click += btn_update_Click;
            // 
            // InvoiceAcknowlodge
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1063, 450);
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
    }
}