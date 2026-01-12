namespace FileIndex
{
    partial class FileCorrection
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
            saveCorrectionBtn = new Button();
            FileNoTxt = new TextBox();
            label6 = new Label();
            remarkTxt = new TextBox();
            label5 = new Label();
            subjectTxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            fileNoCmb = new ComboBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label3 = new Label();
            SuspendLayout();
            // 
            // saveCorrectionBtn
            // 
            saveCorrectionBtn.Anchor = AnchorStyles.Left;
            saveCorrectionBtn.BackColor = Color.FromArgb(0, 165, 255);
            saveCorrectionBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            saveCorrectionBtn.ForeColor = Color.White;
            saveCorrectionBtn.Location = new Point(250, 560);
            saveCorrectionBtn.Margin = new Padding(1);
            saveCorrectionBtn.Name = "saveCorrectionBtn";
            saveCorrectionBtn.Size = new Size(169, 46);
            saveCorrectionBtn.TabIndex = 5;
            saveCorrectionBtn.Text = "Save";
            saveCorrectionBtn.UseVisualStyleBackColor = false;
            saveCorrectionBtn.Click += saveCorrectionBtn_Click;
            // 
            // FileNoTxt
            // 
            FileNoTxt.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FileNoTxt.Location = new Point(186, 214);
            FileNoTxt.Name = "FileNoTxt";
            FileNoTxt.Size = new Size(362, 29);
            FileNoTxt.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(47, 214);
            label6.Name = "label6";
            label6.Size = new Size(132, 21);
            label6.TabIndex = 24;
            label6.Text = "New File Number";
            // 
            // remarkTxt
            // 
            remarkTxt.Location = new Point(186, 402);
            remarkTxt.Multiline = true;
            remarkTxt.Name = "remarkTxt";
            remarkTxt.Size = new Size(362, 119);
            remarkTxt.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(47, 402);
            label5.Name = "label5";
            label5.Size = new Size(71, 21);
            label5.TabIndex = 23;
            label5.Text = "Remarks";
            // 
            // subjectTxt
            // 
            subjectTxt.Location = new Point(186, 284);
            subjectTxt.Multiline = true;
            subjectTxt.Name = "subjectTxt";
            subjectTxt.Size = new Size(362, 83);
            subjectTxt.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(47, 284);
            label2.Name = "label2";
            label2.Size = new Size(89, 21);
            label2.TabIndex = 20;
            label2.Text = "File Subject";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(47, 163);
            label1.Name = "label1";
            label1.Size = new Size(115, 21);
            label1.TabIndex = 18;
            label1.Text = "Select File Type";
            // 
            // fileNoCmb
            // 
            fileNoCmb.FormattingEnabled = true;
            fileNoCmb.Location = new Point(189, 163);
            fileNoCmb.Name = "fileNoCmb";
            fileNoCmb.Size = new Size(359, 29);
            fileNoCmb.TabIndex = 1;
            fileNoCmb.SelectedIndexChanged += fileNoCmb_SelectedIndexChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(182, 94);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(250, 3);
            flowLayoutPanel1.TabIndex = 13;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 165, 255);
            label3.Location = new Point(108, 45);
            label3.Name = "label3";
            label3.Size = new Size(398, 46);
            label3.TabIndex = 15;
            label3.Text = "Correction in File Index";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FileCorrection
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(607, 685);
            Controls.Add(saveCorrectionBtn);
            Controls.Add(FileNoTxt);
            Controls.Add(label6);
            Controls.Add(remarkTxt);
            Controls.Add(label5);
            Controls.Add(subjectTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(fileNoCmb);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FileCorrection";
            Text = "FileCorrection";
            Load += FileCorrection_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button saveCorrectionBtn;
        private TextBox FileNoTxt;
        private Label label6;
        private TextBox remarkTxt;
        private Label label5;
        private TextBox subjectTxt;
        private Label label2;
        private Label label1;
        private ComboBox fileNoCmb;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label3;
    }
}