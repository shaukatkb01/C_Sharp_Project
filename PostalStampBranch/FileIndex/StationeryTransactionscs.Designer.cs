namespace FileIndex
{
    partial class StationeryTransactionscs
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
            panel2 = new Panel();
            cmb_ST = new ComboBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            num_out = new NumericUpDown();
            cmb_Address = new ComboBox();
            dt_Supply = new DateTimePicker();
            label2 = new Label();
            cmb_item = new ComboBox();
            txt_Remarks = new TextBox();
            btn_Add = new Button();
            label1 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            btn_Assign = new Button();
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_out).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Controls.Add(cmb_ST);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(num_out);
            panel2.Controls.Add(cmb_Address);
            panel2.Controls.Add(dt_Supply);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(cmb_item);
            panel2.Controls.Add(txt_Remarks);
            panel2.Controls.Add(btn_Add);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Controls.Add(btn_Assign);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 286);
            panel2.TabIndex = 164;
            // 
            // cmb_ST
            // 
            cmb_ST.FormattingEnabled = true;
            cmb_ST.Location = new Point(167, 187);
            cmb_ST.Name = "cmb_ST";
            cmb_ST.Size = new Size(121, 29);
            cmb_ST.TabIndex = 174;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(329, 163);
            label6.Name = "label6";
            label6.Size = new Size(68, 21);
            label6.TabIndex = 173;
            label6.Text = "Qty. Out";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(167, 163);
            label5.Name = "label5";
            label5.Size = new Size(86, 21);
            label5.TabIndex = 172;
            label5.Text = "SuppyType";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 99);
            label4.Name = "label4";
            label4.Size = new Size(66, 21);
            label4.TabIndex = 171;
            label4.Text = "Address";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 164);
            label3.Name = "label3";
            label3.Size = new Size(42, 21);
            label3.TabIndex = 170;
            label3.Text = "Date";
            // 
            // num_out
            // 
            num_out.Location = new Point(329, 188);
            num_out.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            num_out.Name = "num_out";
            num_out.Size = new Size(104, 29);
            num_out.TabIndex = 169;
            // 
            // cmb_Address
            // 
            cmb_Address.FormattingEnabled = true;
            cmb_Address.Location = new Point(12, 123);
            cmb_Address.Name = "cmb_Address";
            cmb_Address.Size = new Size(595, 29);
            cmb_Address.TabIndex = 167;
            // 
            // dt_Supply
            // 
            dt_Supply.Format = DateTimePickerFormat.Short;
            dt_Supply.Location = new Point(11, 188);
            dt_Supply.Name = "dt_Supply";
            dt_Supply.Size = new Size(127, 29);
            dt_Supply.TabIndex = 166;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 36);
            label2.Name = "label2";
            label2.Size = new Size(86, 21);
            label2.TabIndex = 165;
            label2.Text = "Select Item";
            // 
            // cmb_item
            // 
            cmb_item.FormattingEnabled = true;
            cmb_item.Location = new Point(12, 67);
            cmb_item.Name = "cmb_item";
            cmb_item.Size = new Size(595, 29);
            cmb_item.TabIndex = 163;
            cmb_item.SelectedIndexChanged += cmb_item_SelectedIndexChanged;
            cmb_item.MouseClick += cmb_item_MouseClick;
            // 
            // txt_Remarks
            // 
            txt_Remarks.Location = new Point(6, 223);
            txt_Remarks.Multiline = true;
            txt_Remarks.Name = "txt_Remarks";
            txt_Remarks.PlaceholderText = "Remark";
            txt_Remarks.Size = new Size(601, 57);
            txt_Remarks.TabIndex = 162;
            // 
            // btn_Add
            // 
            btn_Add.Location = new Point(635, 67);
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
            label1.Location = new Point(201, 0);
            label1.Name = "label1";
            label1.Size = new Size(398, 51);
            label1.TabIndex = 155;
            label1.Text = "Add Edit Stationery";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel2.Location = new Point(250, 54);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(300, 3);
            flowLayoutPanel2.TabIndex = 154;
            // 
            // btn_Assign
            // 
            btn_Assign.Enabled = false;
            btn_Assign.Location = new Point(635, 117);
            btn_Assign.Name = "btn_Assign";
            btn_Assign.Size = new Size(124, 44);
            btn_Assign.TabIndex = 153;
            btn_Assign.Text = "Update";
            btn_Assign.UseVisualStyleBackColor = true;
            btn_Assign.Click += btn_Assign_Click;
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 287);
            dataGridView1.TabIndex = 165;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // panel1
            // 
            panel1.Controls.Add(dataGridView1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 286);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 287);
            panel1.TabIndex = 166;
            // 
            // StationeryTransactionscs
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 573);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "StationeryTransactionscs";
            Text = "StationeryTransactionscs";
            Load += StationeryTransactionscs_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_out).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel2;
        private TextBox txt_Remarks;
        private TextBox txt_Item;
        private Button btn_Add;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button btn_Assign;
        private ComboBox cmb_item;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private DataGridView dataGridView1;
        private Panel panel1;
        private DateTimePicker dt_Supply;
        private Label label2;
        private TextBox textBox1;
        private ComboBox cmb_Address;
        private DomainUpDown domainUpDown1;
        private NumericUpDown num_out;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private ComboBox cmb_ST;
    }
}
