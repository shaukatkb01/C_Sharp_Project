namespace FileIndex
{
    partial class AddItems
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
            sqlCommandBuilder1 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            SignatureName = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label7 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // SignatureName
            // 
            SignatureName.Location = new Point(43, 135);
            SignatureName.Name = "SignatureName";
            SignatureName.Size = new Size(187, 50);
            SignatureName.TabIndex = 0;
            SignatureName.Text = "Signature Authority";
            SignatureName.UseVisualStyleBackColor = true;
            SignatureName.Click += SignatureName_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(264, 115);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 3);
            flowLayoutPanel1.TabIndex = 107;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(0, 165, 255);
            label7.Location = new Point(212, 25);
            label7.Name = "label7";
            label7.Size = new Size(398, 87);
            label7.TabIndex = 108;
            label7.Text = "Add/Update Items";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.Location = new Point(236, 135);
            button1.Name = "button1";
            button1.Size = new Size(187, 50);
            button1.TabIndex = 109;
            button1.Text = "DispatchType";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // AddItems
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label7);
            Controls.Add(SignatureName);
            Name = "AddItems";
            Text = "AddItems";
            Load += AddItems_Load;
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder1;
        private Button SignatureName;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label7;
        private Button button1;
    }
}