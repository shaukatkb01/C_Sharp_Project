namespace FileIndex
{
    partial class AddFileNo
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
            label3 = new Label();
            fileTypeCmb = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            subjectTxt = new TextBox();
            label4 = new Label();
            remarkTxt = new TextBox();
            label5 = new Label();
            newFileNoTxt = new TextBox();
            label6 = new Label();
            dateOfCreationPick = new DateTimePicker();
            addFileNoBtn = new Button();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(154, 69);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(250, 3);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 165, 255);
            label3.Location = new Point(80, 20);
            label3.Name = "label3";
            label3.Size = new Size(398, 46);
            label3.TabIndex = 2;
            label3.Text = "Add New File Number";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // fileTypeCmb
            // 
            fileTypeCmb.FormattingEnabled = true;
            fileTypeCmb.Location = new Point(174, 162);
            fileTypeCmb.Name = "fileTypeCmb";
            fileTypeCmb.Size = new Size(359, 29);
            fileTypeCmb.TabIndex = 1;
            fileTypeCmb.SelectedIndexChanged += fileTypeCmb_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(32, 162);
            label1.Name = "label1";
            label1.Size = new Size(115, 21);
            label1.TabIndex = 4;
            label1.Text = "Select File Type";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(32, 283);
            label2.Name = "label2";
            label2.Size = new Size(89, 21);
            label2.TabIndex = 5;
            label2.Text = "File Subject";
            // 
            // subjectTxt
            // 
            subjectTxt.Location = new Point(171, 283);
            subjectTxt.Multiline = true;
            subjectTxt.Name = "subjectTxt";
            subjectTxt.Size = new Size(362, 83);
            subjectTxt.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(32, 389);
            label4.Name = "label4";
            label4.Size = new Size(115, 21);
            label4.TabIndex = 7;
            label4.Text = "Select File Type";
            // 
            // remarkTxt
            // 
            remarkTxt.Location = new Point(171, 449);
            remarkTxt.Multiline = true;
            remarkTxt.Name = "remarkTxt";
            remarkTxt.Size = new Size(362, 119);
            remarkTxt.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(32, 449);
            label5.Name = "label5";
            label5.Size = new Size(71, 21);
            label5.TabIndex = 9;
            label5.Text = "Remarks";
            // 
            // newFileNoTxt
            // 
            newFileNoTxt.Location = new Point(171, 213);
            newFileNoTxt.Name = "newFileNoTxt";
            newFileNoTxt.Size = new Size(362, 29);
            newFileNoTxt.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(32, 213);
            label6.Name = "label6";
            label6.Size = new Size(132, 21);
            label6.TabIndex = 11;
            label6.Text = "New File Number";
            // 
            // dateOfCreationPick
            // 
            dateOfCreationPick.Location = new Point(171, 389);
            dateOfCreationPick.Name = "dateOfCreationPick";
            dateOfCreationPick.Size = new Size(362, 29);
            dateOfCreationPick.TabIndex = 4;
            // 
            // addFileNoBtn
            // 
            addFileNoBtn.BackColor = Color.FromArgb(0, 165, 255);
            addFileNoBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addFileNoBtn.ForeColor = Color.White;
            addFileNoBtn.Location = new Point(253, 606);
            addFileNoBtn.Name = "addFileNoBtn";
            addFileNoBtn.Size = new Size(169, 46);
            addFileNoBtn.TabIndex = 6;
            addFileNoBtn.Text = "Add";
            addFileNoBtn.UseVisualStyleBackColor = false;
            addFileNoBtn.Click += addFileNoBtn_Click;
            // 
            // AddFileNo
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(607, 685);
            Controls.Add(addFileNoBtn);
            Controls.Add(dateOfCreationPick);
            Controls.Add(newFileNoTxt);
            Controls.Add(label6);
            Controls.Add(remarkTxt);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(subjectTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(fileTypeCmb);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddFileNo";
            RightToLeft = RightToLeft.No;
            Text = "AddFileNo";
            Load += AddFileNo_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Label label3;
        private ComboBox fileTypeCmb;
        private Label label1;
        private Label label2;
        private TextBox subjectTxt;
        private Label label4;
        private TextBox remarkTxt;
        private Label label5;
        private TextBox newFileNoTxt;
        private Label label6;
        private DateTimePicker dateOfCreationPick;
        private Button addFileNoBtn;
    }
}