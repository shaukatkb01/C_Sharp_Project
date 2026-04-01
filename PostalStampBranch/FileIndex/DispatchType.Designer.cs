namespace FileIndex
{
    partial class DispatchType
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
            button1 = new Button();
            lbl_HiddenID = new Label();
            txt_Remarks = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btn_AddUpdate = new Button();
            label7 = new Label();
            txt_distype = new TextBox();
            panel2 = new Panel();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(lbl_HiddenID);
            panel1.Controls.Add(txt_Remarks);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(btn_AddUpdate);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(txt_distype);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1194, 387);
            panel1.TabIndex = 109;
            // 
            // button1
            // 
            button1.Location = new Point(251, 327);
            button1.Name = "button1";
            button1.Size = new Size(206, 50);
            button1.TabIndex = 108;
            button1.Text = "Delete";
            button1.UseVisualStyleBackColor = true;
            // 
            // lbl_HiddenID
            // 
            lbl_HiddenID.AutoSize = true;
            lbl_HiddenID.Location = new Point(362, 316);
            lbl_HiddenID.Name = "lbl_HiddenID";
            lbl_HiddenID.Size = new Size(52, 21);
            lbl_HiddenID.TabIndex = 107;
            lbl_HiddenID.Text = "label1";
            lbl_HiddenID.Visible = false;
            // 
            // txt_Remarks
            // 
            txt_Remarks.Location = new Point(3, 157);
            txt_Remarks.Multiline = true;
            txt_Remarks.Name = "txt_Remarks";
            txt_Remarks.PlaceholderText = "Remark";
            txt_Remarks.Size = new Size(457, 77);
            txt_Remarks.TabIndex = 8;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(68, 98);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(319, 3);
            flowLayoutPanel1.TabIndex = 105;
            // 
            // btn_AddUpdate
            // 
            btn_AddUpdate.Location = new Point(27, 327);
            btn_AddUpdate.Name = "btn_AddUpdate";
            btn_AddUpdate.Size = new Size(206, 50);
            btn_AddUpdate.TabIndex = 5;
            btn_AddUpdate.Text = "Add";
            btn_AddUpdate.UseVisualStyleBackColor = true;
            btn_AddUpdate.Click += btn_AddUpdate_Click;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(0, 165, 255);
            label7.Location = new Point(16, 8);
            label7.Name = "label7";
            label7.Size = new Size(417, 87);
            label7.TabIndex = 106;
            label7.Text = "Add/Update Dispatch Type";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txt_distype
            // 
            txt_distype.Location = new Point(0, 122);
            txt_distype.Name = "txt_distype";
            txt_distype.PlaceholderText = "DispatchType";
            txt_distype.Size = new Size(457, 29);
            txt_distype.TabIndex = 6;
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1194, 662);
            panel2.TabIndex = 110;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1194, 662);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // DispatchType
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1194, 662);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "DispatchType";
            Text = "DispatchType";
            Load += DispatchType_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private Label lbl_HiddenID;
        private TextBox txt_Remarks;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btn_AddUpdate;
        private Label label7;
        private TextBox txt_distype;
        private Panel panel2;
        private DataGridView dataGridView1;
    }
}