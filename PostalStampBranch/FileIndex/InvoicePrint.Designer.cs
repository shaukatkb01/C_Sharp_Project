namespace FileIndex
{
    partial class InvoicePrint
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
            searchBtn = new Button();
            From = new Label();
            label2 = new Label();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            exportBtn = new Button();
            com_ST = new ComboBox();
            label14 = new Label();
            com_FileNo = new ComboBox();
            label13 = new Label();
            cmb_DT = new ComboBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel2.Location = new Point(171, 126);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(300, 3);
            flowLayoutPanel2.TabIndex = 113;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(0, 165, 255);
            label1.Location = new Point(122, 20);
            label1.Name = "label1";
            label1.Size = new Size(398, 87);
            label1.TabIndex = 114;
            label1.Text = "Print Invoice Register";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // searchBtn
            // 
            searchBtn.BackColor = Color.FromArgb(0, 165, 255);
            searchBtn.Cursor = Cursors.Hand;
            searchBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            searchBtn.ForeColor = Color.White;
            searchBtn.Location = new Point(387, 348);
            searchBtn.Margin = new Padding(1);
            searchBtn.Name = "searchBtn";
            searchBtn.Size = new Size(169, 46);
            searchBtn.TabIndex = 120;
            searchBtn.Text = "Print Register";
            searchBtn.UseVisualStyleBackColor = false;
            searchBtn.Click += searchBtn_Click;
            // 
            // From
            // 
            From.AutoSize = true;
            From.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            From.ForeColor = Color.White;
            From.Location = new Point(365, 257);
            From.Name = "From";
            From.Size = new Size(59, 25);
            From.TabIndex = 119;
            From.Text = "From";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(500, 255);
            label2.Name = "label2";
            label2.Size = new Size(33, 25);
            label2.TabIndex = 118;
            label2.Text = "To";
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(475, 294);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(114, 29);
            dtpTo.TabIndex = 117;
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(343, 294);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(114, 29);
            dtpFrom.TabIndex = 116;
            dtpFrom.Value = new DateTime(1947, 1, 14, 9, 52, 0, 0);
            // 
            // exportBtn
            // 
            exportBtn.BackColor = Color.FromArgb(0, 165, 255);
            exportBtn.Cursor = Cursors.Hand;
            exportBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportBtn.ForeColor = Color.White;
            exportBtn.Location = new Point(23, 348);
            exportBtn.Margin = new Padding(1);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(169, 46);
            exportBtn.TabIndex = 115;
            exportBtn.Text = "Print Single";
            exportBtn.UseVisualStyleBackColor = false;
            exportBtn.Click += exportBtn_Click;
            // 
            // com_ST
            // 
            com_ST.DropDownStyle = ComboBoxStyle.DropDownList;
            com_ST.FormattingEnabled = true;
            com_ST.Location = new Point(343, 210);
            com_ST.Name = "com_ST";
            com_ST.Size = new Size(246, 29);
            com_ST.TabIndex = 139;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = Color.Black;
            label14.Location = new Point(343, 171);
            label14.Name = "label14";
            label14.Size = new Size(99, 21);
            label14.TabIndex = 141;
            label14.Text = "SupplyType:-";
            // 
            // com_FileNo
            // 
            com_FileNo.DropDownStyle = ComboBoxStyle.DropDownList;
            com_FileNo.FormattingEnabled = true;
            com_FileNo.Location = new Point(23, 195);
            com_FileNo.Name = "com_FileNo";
            com_FileNo.Size = new Size(169, 29);
            com_FileNo.TabIndex = 138;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Black;
            label13.Location = new Point(63, 171);
            label13.Name = "label13";
            label13.Size = new Size(88, 21);
            label13.TabIndex = 140;
            label13.Text = "Select File:-";
            // 
            // cmb_DT
            // 
            cmb_DT.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_DT.FormattingEnabled = true;
            cmb_DT.Location = new Point(23, 280);
            cmb_DT.Name = "cmb_DT";
            cmb_DT.Size = new Size(169, 29);
            cmb_DT.TabIndex = 142;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Black;
            label3.Location = new Point(49, 255);
            label3.Name = "label3";
            label3.Size = new Size(102, 21);
            label3.TabIndex = 143;
            label3.Text = "DispatchType";
            // 
            // InvoicePrint
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(642, 450);
            Controls.Add(cmb_DT);
            Controls.Add(label3);
            Controls.Add(com_ST);
            Controls.Add(label14);
            Controls.Add(com_FileNo);
            Controls.Add(label13);
            Controls.Add(searchBtn);
            Controls.Add(From);
            Controls.Add(label2);
            Controls.Add(dtpTo);
            Controls.Add(dtpFrom);
            Controls.Add(exportBtn);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(label1);
            Name = "InvoicePrint";
            Text = "InvoicePrint";
            Load += InvoicePrint_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1;
        private Button searchBtn;
        private Label From;
        private Label label2;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private Button exportBtn;
        private ComboBox com_ST;
        private Label label14;
        private ComboBox com_FileNo;
        private Label label13;
        private ComboBox cmb_DT;
        private Label label3;
    }
}