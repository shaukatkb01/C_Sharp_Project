namespace FileIndex
{
    partial class AddressOption
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
            label5 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox1 = new GroupBox();
            radio3 = new RadioButton();
            radio2 = new RadioButton();
            radio1 = new RadioButton();
            panel2 = new Panel();
            btnOK = new Button();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(558, 130);
            panel1.TabIndex = 0;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(0, 165, 255);
            label5.Location = new Point(80, -4);
            label5.Name = "label5";
            label5.Size = new Size(399, 87);
            label5.TabIndex = 169;
            label5.Text = "Select Address Type";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(80, 102);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(399, 3);
            flowLayoutPanel1.TabIndex = 168;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.None;
            groupBox1.Controls.Add(radio3);
            groupBox1.Controls.Add(radio2);
            groupBox1.Controls.Add(radio1);
            groupBox1.Location = new Point(84, 61);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(390, 100);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "AddressType";
            // 
            // radio3
            // 
            radio3.AutoSize = true;
            radio3.Location = new Point(307, 50);
            radio3.Name = "radio3";
            radio3.Size = new Size(51, 25);
            radio3.TabIndex = 2;
            radio3.TabStop = true;
            radio3.Text = "Tag";
            radio3.UseVisualStyleBackColor = true;
            // 
            // radio2
            // 
            radio2.AutoSize = true;
            radio2.Location = new Point(190, 50);
            radio2.Name = "radio2";
            radio2.Size = new Size(117, 25);
            radio2.TabIndex = 1;
            radio2.TabStop = true;
            radio2.Text = "OnlyAddress";
            radio2.UseVisualStyleBackColor = true;
            // 
            // radio1
            // 
            radio1.AutoSize = true;
            radio1.Location = new Point(31, 50);
            radio1.Name = "radio1";
            radio1.Size = new Size(159, 25);
            radio1.TabIndex = 0;
            radio1.TabStop = true;
            radio1.Text = "NameWithAddress";
            radio1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnOK);
            panel2.Controls.Add(groupBox1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 130);
            panel2.Name = "panel2";
            panel2.Size = new Size(558, 223);
            panel2.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(215, 174);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(129, 32);
            btnOK.TabIndex = 1;
            btnOK.Text = "Print";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // AddressOption
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(558, 353);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "AddressOption";
            Text = "AddressOption";
            Load += AddressOption_Load;
            panel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label5;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox1;
        private RadioButton radio3;
        private RadioButton radio2;
        private RadioButton radio1;
        private Panel panel2;
        private Button btnOK;
    }
}