namespace FileIndex
{
    partial class SingleMaillist
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
            btn_ADDRow = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label16 = new Label();
            drop_DisType = new ComboBox();
            label6 = new Label();
            Cmb_IssueNo = new ComboBox();
            btn_Print = new Button();
            SuspendLayout();
            // 
            // btn_ADDRow
            // 
            btn_ADDRow.Location = new Point(279, 12);
            btn_ADDRow.Name = "btn_ADDRow";
            btn_ADDRow.Size = new Size(184, 43);
            btn_ADDRow.TabIndex = 726;
            btn_ADDRow.Text = "Add";
            btn_ADDRow.UseVisualStyleBackColor = true;
            btn_ADDRow.Click += btn_ADDRow_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(1, 110);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(972, 469);
            flowLayoutPanel1.TabIndex = 727;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label16.ForeColor = Color.Black;
            label16.Location = new Point(1, 61);
            label16.Name = "label16";
            label16.Size = new Size(81, 15);
            label16.TabIndex = 735;
            label16.Text = "DispatchType";
            label16.TextAlign = ContentAlignment.MiddleRight;
            // 
            // drop_DisType
            // 
            drop_DisType.DropDownStyle = ComboBoxStyle.DropDownList;
            drop_DisType.FormattingEnabled = true;
            drop_DisType.Location = new Point(115, 47);
            drop_DisType.Name = "drop_DisType";
            drop_DisType.Size = new Size(129, 29);
            drop_DisType.TabIndex = 734;
            drop_DisType.Tag = "InvoiceControl";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(1, 26);
            label6.Name = "label6";
            label6.Size = new Size(45, 15);
            label6.TabIndex = 733;
            label6.Text = "File No";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Cmb_IssueNo
            // 
            Cmb_IssueNo.DropDownStyle = ComboBoxStyle.DropDownList;
            Cmb_IssueNo.FormattingEnabled = true;
            Cmb_IssueNo.Location = new Point(115, 12);
            Cmb_IssueNo.Name = "Cmb_IssueNo";
            Cmb_IssueNo.Size = new Size(129, 29);
            Cmb_IssueNo.TabIndex = 732;
            Cmb_IssueNo.Tag = "InvoiceControl";
            // 
            // btn_Print
            // 
            btn_Print.Location = new Point(279, 61);
            btn_Print.Name = "btn_Print";
            btn_Print.Size = new Size(184, 43);
            btn_Print.TabIndex = 736;
            btn_Print.Text = "Print";
            btn_Print.UseVisualStyleBackColor = true;
            btn_Print.Click += btn_Print_Click;
            // 
            // SingleMaillist
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 634);
            Controls.Add(btn_Print);
            Controls.Add(label16);
            Controls.Add(drop_DisType);
            Controls.Add(label6);
            Controls.Add(Cmb_IssueNo);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(btn_ADDRow);
            Name = "SingleMaillist";
            Text = "SingleMaillist";
            Load += SingleMaillist_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label16;
        private ComboBox drop_DisType;
        
        private Label label10;
        private Button btn_ADDRow;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label6;
        private ComboBox Cmb_IssueNo;
        
        private Button btn_Print;
    }
}