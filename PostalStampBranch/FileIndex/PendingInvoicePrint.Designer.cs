namespace FileIndex
{
    partial class PendingInvoicePrint
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
            panel1 = new Panel();
            cmb_Signature = new ComboBox();
            label3 = new Label();
            dtpRemidner = new DateTimePicker();
            label1 = new Label();
            num_reminderNo = new NumericUpDown();
            From = new Label();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            btn_Print = new Button();
            label2 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label7 = new Label();
            cmb_Address = new ComboBox();
            label17 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_reminderNo).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(cmb_Signature);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(dtpRemidner);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(num_reminderNo);
            panel1.Controls.Add(From);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(dtpTo);
            panel1.Controls.Add(dtpFrom);
            panel1.Controls.Add(btn_Print);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(cmb_Address);
            panel1.Controls.Add(label17);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(587, 744);
            panel1.TabIndex = 0;
            // 
            // cmb_Signature
            // 
            cmb_Signature.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Signature.FormattingEnabled = true;
            cmb_Signature.Location = new Point(191, 412);
            cmb_Signature.Name = "cmb_Signature";
            cmb_Signature.Size = new Size(252, 29);
            cmb_Signature.TabIndex = 141;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(12, 412);
            label3.Name = "label3";
            label3.Size = new Size(122, 21);
            label3.TabIndex = 142;
            label3.Text = "Select Signature";
            // 
            // dtpRemidner
            // 
            dtpRemidner.Enabled = false;
            dtpRemidner.Format = DateTimePickerFormat.Short;
            dtpRemidner.Location = new Point(191, 290);
            dtpRemidner.Name = "dtpRemidner";
            dtpRemidner.Size = new Size(252, 29);
            dtpRemidner.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 254);
            label1.Name = "label1";
            label1.Size = new Size(106, 21);
            label1.TabIndex = 140;
            label1.Text = "Reminder No.";
            // 
            // num_reminderNo
            // 
            num_reminderNo.Location = new Point(191, 254);
            num_reminderNo.Name = "num_reminderNo";
            num_reminderNo.Size = new Size(252, 29);
            num_reminderNo.TabIndex = 1;
            num_reminderNo.ValueChanged += num_reminderNo_ValueChanged;
            // 
            // From
            // 
            From.AutoSize = true;
            From.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            From.ForeColor = Color.White;
            From.Location = new Point(12, 327);
            From.Name = "From";
            From.Size = new Size(59, 25);
            From.TabIndex = 138;
            From.Text = "From";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(12, 371);
            label4.Name = "label4";
            label4.Size = new Size(33, 25);
            label4.TabIndex = 137;
            label4.Text = "To";
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(191, 377);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(252, 29);
            dtpTo.TabIndex = 4;
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(191, 329);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(252, 29);
            dtpFrom.TabIndex = 3;
            dtpFrom.Value = new DateTime(1947, 1, 14, 9, 52, 0, 0);
            // 
            // btn_Print
            // 
            btn_Print.Location = new Point(191, 462);
            btn_Print.Name = "btn_Print";
            btn_Print.Size = new Size(252, 53);
            btn_Print.TabIndex = 5;
            btn_Print.Text = "Print";
            btn_Print.UseVisualStyleBackColor = true;
            btn_Print.Click += btn_Print_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(12, 288);
            label2.Name = "label2";
            label2.Size = new Size(155, 21);
            label2.TabIndex = 130;
            label2.Text = "Last Reminder Date:-";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(143, 151);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 3);
            flowLayoutPanel1.TabIndex = 126;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(0, 165, 255);
            label7.Location = new Point(94, 45);
            label7.Name = "label7";
            label7.Size = new Size(398, 87);
            label7.TabIndex = 127;
            label7.Text = "Pending Invoice Print";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmb_Address
            // 
            cmb_Address.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Address.FormattingEnabled = true;
            cmb_Address.Location = new Point(191, 213);
            cmb_Address.Name = "cmb_Address";
            cmb_Address.Size = new Size(252, 29);
            cmb_Address.TabIndex = 0;
            cmb_Address.SelectedIndexChanged += cmb_Address_SelectedIndexChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = Color.White;
            label17.Location = new Point(12, 213);
            label17.Name = "label17";
            label17.Size = new Size(111, 21);
            label17.TabIndex = 125;
            label17.Text = "Select Address";
            // 
            // PendingInvoicePrint
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(587, 744);
            Controls.Add(panel1);
            Name = "PendingInvoicePrint";
            Text = "PendingInvoicePrint";
            Load += PendingInvoicePrint_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_reminderNo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btn_Print;
        private Label label2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label7;
        private ComboBox cmb_Address;
        private Label label17;
        private Label From;
        private Label label4;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private Label label1;
        private NumericUpDown num_reminderNo;
        private DateTimePicker dtpRemidner;
        private ComboBox cmb_Signature;
        private Label label3;
    }
}