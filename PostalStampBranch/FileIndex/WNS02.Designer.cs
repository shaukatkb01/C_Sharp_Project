namespace FileIndex
{
    partial class WNS02
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
            dt_From = new DateTimePicker();
            label1 = new Label();
            label2 = new Label();
            dt_To = new DateTimePicker();
            btn_Open = new Button();
            cmb_Signature = new ComboBox();
            label3 = new Label();
            groupBox1 = new GroupBox();
            ch_Mr = new RadioButton();
            ch_Mrs = new RadioButton();
            ch_Ms = new RadioButton();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dt_From
            // 
            dt_From.Format = DateTimePickerFormat.Short;
            dt_From.Location = new Point(94, 148);
            dt_From.Name = "dt_From";
            dt_From.Size = new Size(114, 29);
            dt_From.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 156);
            label1.Name = "label1";
            label1.Size = new Size(47, 21);
            label1.TabIndex = 1;
            label1.Text = "From";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(238, 156);
            label2.Name = "label2";
            label2.Size = new Size(25, 21);
            label2.TabIndex = 3;
            label2.Text = "To";
            // 
            // dt_To
            // 
            dt_To.Format = DateTimePickerFormat.Short;
            dt_To.Location = new Point(302, 148);
            dt_To.Name = "dt_To";
            dt_To.Size = new Size(114, 29);
            dt_To.TabIndex = 2;
            // 
            // btn_Open
            // 
            btn_Open.Location = new Point(193, 289);
            btn_Open.Name = "btn_Open";
            btn_Open.Size = new Size(243, 68);
            btn_Open.TabIndex = 4;
            btn_Open.Text = "Open Form";
            btn_Open.UseVisualStyleBackColor = true;
            btn_Open.Click += btn_Open_Click;
            // 
            // cmb_Signature
            // 
            cmb_Signature.FormattingEnabled = true;
            cmb_Signature.Location = new Point(189, 38);
            cmb_Signature.Name = "cmb_Signature";
            cmb_Signature.Size = new Size(227, 29);
            cmb_Signature.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(94, 46);
            label3.Name = "label3";
            label3.Size = new Size(77, 21);
            label3.TabIndex = 6;
            label3.Text = "Signature";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ch_Ms);
            groupBox1.Controls.Add(ch_Mrs);
            groupBox1.Controls.Add(ch_Mr);
            groupBox1.Location = new Point(518, 38);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(270, 127);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Title";
            // 
            // ch_Mr
            // 
            ch_Mr.AutoSize = true;
            ch_Mr.Location = new Point(43, 50);
            ch_Mr.Name = "ch_Mr";
            ch_Mr.Size = new Size(51, 25);
            ch_Mr.TabIndex = 3;
            ch_Mr.TabStop = true;
            ch_Mr.Text = "Mr.";
            ch_Mr.UseVisualStyleBackColor = true;
            // 
            // ch_Mrs
            // 
            ch_Mrs.AutoSize = true;
            ch_Mrs.Location = new Point(106, 50);
            ch_Mrs.Name = "ch_Mrs";
            ch_Mrs.Size = new Size(58, 25);
            ch_Mrs.TabIndex = 4;
            ch_Mrs.TabStop = true;
            ch_Mrs.Text = "Mrs.";
            ch_Mrs.UseVisualStyleBackColor = true;
            // 
            // ch_Ms
            // 
            ch_Ms.AutoSize = true;
            ch_Ms.Location = new Point(176, 50);
            ch_Ms.Name = "ch_Ms";
            ch_Ms.Size = new Size(52, 25);
            ch_Ms.TabIndex = 5;
            ch_Ms.TabStop = true;
            ch_Ms.Text = "Ms.";
            ch_Ms.UseVisualStyleBackColor = true;
            ch_Ms.CheckedChanged += ch_Ms1_CheckedChanged;
            // 
            // WNS02
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Controls.Add(label3);
            Controls.Add(cmb_Signature);
            Controls.Add(btn_Open);
            Controls.Add(label2);
            Controls.Add(dt_To);
            Controls.Add(label1);
            Controls.Add(dt_From);
            Name = "WNS02";
            Text = "WNS02";
            Load += WNS02_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dt_From;
        private Label label1;
        private Label label2;
        private DateTimePicker dt_To;
        private Button btn_Open;
        private ComboBox cmb_Signature;
        private Label label3;
        private GroupBox groupBox1;
        private RadioButton ch_Ms;
        private RadioButton ch_Mrs;
        private RadioButton ch_Mr;
    }
}