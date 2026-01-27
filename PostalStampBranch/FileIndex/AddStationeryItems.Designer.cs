namespace FileIndex
{
    partial class AddStationeryItems
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
            dataGridView1 = new DataGridView();
            panel2 = new Panel();
            txt_Remark = new TextBox();
            txt_Item = new TextBox();
            btn_Add = new Button();
            label1 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            btn_Assign = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(624, 349);
            dataGridView1.TabIndex = 164;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // panel2
            // 
            panel2.Controls.Add(txt_Remark);
            panel2.Controls.Add(txt_Item);
            panel2.Controls.Add(btn_Add);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Controls.Add(btn_Assign);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(14, 15);
            panel2.Name = "panel2";
            panel2.Size = new Size(624, 246);
            panel2.TabIndex = 163;
            // 
            // txt_Remark
            // 
            txt_Remark.Location = new Point(3, 117);
            txt_Remark.Multiline = true;
            txt_Remark.Name = "txt_Remark";
            txt_Remark.PlaceholderText = "Remark";
            txt_Remark.Size = new Size(593, 57);
            txt_Remark.TabIndex = 162;
            // 
            // txt_Item
            // 
            txt_Item.Location = new Point(10, 72);
            txt_Item.Name = "txt_Item";
            txt_Item.PlaceholderText = "Item";
            txt_Item.Size = new Size(178, 29);
            txt_Item.TabIndex = 160;
            // 
            // btn_Add
            // 
            btn_Add.Location = new Point(293, 199);
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new Size(124, 44);
            btn_Add.TabIndex = 159;
            btn_Add.Text = "Add";
            btn_Add.UseVisualStyleBackColor = true;
            btn_Add.Click += btn_Add_Click;
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
            label1.Text = "Add Edit Stationery";
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
            // panel1
            // 
            panel1.Controls.Add(dataGridView1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(14, 261);
            panel1.Name = "panel1";
            panel1.Size = new Size(624, 349);
            panel1.TabIndex = 165;
            // 
            // AddStationeryItems
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(652, 625);
            Controls.Add(panel1);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddStationeryItems";
            Padding = new Padding(14, 15, 14, 15);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "AddStationeryItems";
            Load += AddStationeryItems_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridView1;
        private Panel panel2;
        private TextBox txt_Remark;
        private TextBox txt_Item;
        private Button btn_Add;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button btn_Assign;
        private Panel panel1;
    }
}
