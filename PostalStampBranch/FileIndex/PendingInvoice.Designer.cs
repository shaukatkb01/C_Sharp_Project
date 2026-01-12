namespace FileIndex
{
    partial class PendingInvoice
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
            com_InvoiceNo = new ComboBox();
            label17 = new Label();
            datePicker_receiving = new DateTimePicker();
            label1 = new Label();
            label2 = new Label();
            text_PageNo = new TextBox();
            btn_save = new Button();
            text_remark = new TextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(250, 195);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 3);
            flowLayoutPanel1.TabIndex = 103;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(0, 165, 255);
            label7.Location = new Point(201, 89);
            label7.Name = "label7";
            label7.Size = new Size(398, 87);
            label7.TabIndex = 104;
            label7.Text = "Invoice Entry";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // com_InvoiceNo
            // 
            com_InvoiceNo.DropDownStyle = ComboBoxStyle.DropDownList;
            com_InvoiceNo.FormattingEnabled = true;
            com_InvoiceNo.Location = new Point(374, 221);
            com_InvoiceNo.Name = "com_InvoiceNo";
            com_InvoiceNo.Size = new Size(176, 29);
            com_InvoiceNo.TabIndex = 101;
            com_InvoiceNo.SelectedIndexChanged += com_InvoiceNo_SelectedIndexChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = Color.Black;
            label17.Location = new Point(230, 224);
            label17.Name = "label17";
            label17.Size = new Size(138, 21);
            label17.TabIndex = 102;
            label17.Text = "Select Invoice No:-";
            // 
            // datePicker_receiving
            // 
            datePicker_receiving.Format = DateTimePickerFormat.Short;
            datePicker_receiving.Location = new Point(238, 338);
            datePicker_receiving.Name = "datePicker_receiving";
            datePicker_receiving.Size = new Size(138, 29);
            datePicker_receiving.TabIndex = 105;
            datePicker_receiving.Value = new DateTime(2025, 12, 31, 0, 0, 0, 0);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Black;
            label1.Location = new Point(389, 342);
            label1.Name = "label1";
            label1.Size = new Size(141, 21);
            label1.TabIndex = 107;
            label1.Text = "Insert PageNo No:-";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Black;
            label2.Location = new Point(94, 342);
            label2.Name = "label2";
            label2.Size = new Size(122, 21);
            label2.TabIndex = 108;
            label2.Text = "Receiving Date:-";
            // 
            // text_PageNo
            // 
            text_PageNo.Location = new Point(536, 338);
            text_PageNo.Name = "text_PageNo";
            text_PageNo.Size = new Size(171, 29);
            text_PageNo.TabIndex = 109;
            // 
            // btn_save
            // 
            btn_save.Location = new Point(305, 512);
            btn_save.Name = "btn_save";
            btn_save.Size = new Size(188, 53);
            btn_save.TabIndex = 110;
            btn_save.Text = "Save";
            btn_save.UseVisualStyleBackColor = true;
            btn_save.Click += btn_save_Click;
            // 
            // text_remark
            // 
            text_remark.Location = new Point(94, 410);
            text_remark.Multiline = true;
            text_remark.Name = "text_remark";
            text_remark.Size = new Size(613, 87);
            text_remark.TabIndex = 111;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Black;
            label3.Location = new Point(94, 386);
            label3.Name = "label3";
            label3.Size = new Size(80, 21);
            label3.TabIndex = 112;
            label3.Text = "Remarks:-";
            // 
            // PendingInvoice
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 655);
            Controls.Add(label3);
            Controls.Add(text_remark);
            Controls.Add(btn_save);
            Controls.Add(text_PageNo);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(datePicker_receiving);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label7);
            Controls.Add(com_InvoiceNo);
            Controls.Add(label17);
            Name = "PendingInvoice";
            Text = "PendingInvoice";
            Load += PendingInvoice_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Label label7;
        private ComboBox com_InvoiceNo;
        private Label label17;
        private DateTimePicker datePicker_receiving;
        private Label label1;
        private Label label2;
        private TextBox text_PageNo;
        private Button btn_save;
        private TextBox text_remark;
        private Label label3;
    }
}