namespace FileIndex
{
    partial class Address
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            panel2 = new Panel();
            cmb_BPS = new ComboBox();
            txt_Address = new TextBox();
            txt_City = new TextBox();
            txt_Name = new TextBox();
            btn_Add = new Button();
            label1 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            btn_Assign = new Button();
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Controls.Add(cmb_BPS);
            panel2.Controls.Add(txt_Address);
            panel2.Controls.Add(txt_City);
            panel2.Controls.Add(txt_Name);
            panel2.Controls.Add(btn_Add);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Controls.Add(btn_Assign);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(14, 15);
            panel2.Name = "panel2";
            panel2.Size = new Size(624, 246);
            panel2.TabIndex = 161;
            // 
            // cmb_BPS
            // 
            cmb_BPS.FormattingEnabled = true;
            cmb_BPS.Location = new Point(432, 72);
            cmb_BPS.Name = "cmb_BPS";
            cmb_BPS.Size = new Size(121, 29);
            cmb_BPS.TabIndex = 163;
            // 
            // txt_Address
            // 
            txt_Address.Location = new Point(3, 117);
            txt_Address.Multiline = true;
            txt_Address.Name = "txt_Address";
            txt_Address.PlaceholderText = "Address";
            txt_Address.Size = new Size(593, 57);
            txt_Address.TabIndex = 162;
            // 
            // txt_City
            // 
            txt_City.Location = new Point(232, 72);
            txt_City.Name = "txt_City";
            txt_City.PlaceholderText = "City";
            txt_City.Size = new Size(185, 29);
            txt_City.TabIndex = 161;
            // 
            // txt_Name
            // 
            txt_Name.Location = new Point(10, 72);
            txt_Name.Name = "txt_Name";
            txt_Name.PlaceholderText = "Name";
            txt_Name.Size = new Size(178, 29);
            txt_Name.TabIndex = 160;
            // 
            // btn_Add
            // 
            btn_Add.Location = new Point(293, 199);
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new Size(124, 44);
            btn_Add.TabIndex = 159;
            btn_Add.Text = "Add";
            btn_Add.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(0, 165, 255);
            label1.Location = new Point(104, 0);
            label1.Name = "label1";
            label1.Size = new Size(398, 51);
            label1.TabIndex = 155;
            label1.Text = "Add Edit Adress";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel2.Location = new Point(163, 54);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(300, 3);
            flowLayoutPanel2.TabIndex = 154;
            // 
            // btn_Assign
            // 
            btn_Assign.Location = new Point(469, 199);
            btn_Assign.Name = "btn_Assign";
            btn_Assign.Size = new Size(124, 44);
            btn_Assign.TabIndex = 153;
            btn_Assign.Text = "Update";
            btn_Assign.UseVisualStyleBackColor = true;
            btn_Assign.Click += btn_Assign_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(624, 341);
            dataGridView1.TabIndex = 162;
            dataGridView1.CellClick += dataGridView1_CellClick;
          
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.Controls.Add(dataGridView1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(14, 261);
            panel1.Name = "panel1";
            panel1.Size = new Size(624, 341);
            panel1.TabIndex = 163;
            // 
            // Address
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(652, 617);
            Controls.Add(panel1);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Address";
            Padding = new Padding(14, 15, 14, 15);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Address";
            Load += Address_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Panel panel2;
        private Button btn_Add;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button btn_Assign;
        private DataGridView dataGridView1;
        private TextBox txt_Address;
        private TextBox txt_City;
        private TextBox txt_Name;
        private ComboBox cmb_BPS;
        private Panel panel1;
    }
}
