namespace FileIndex
{
    partial class searchFile
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dgvResults = new DataGridView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            label3 = new Label();
            txtSearch = new TextBox();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            radio_Date = new RadioButton();
            radio_Search = new RadioButton();
            cmb_Status = new ComboBox();
            exportBtn = new Button();
            toolTip1 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvResults
            // 
            dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvResults.BackgroundColor = SystemColors.Control;
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvResults.DefaultCellStyle = dataGridViewCellStyle1;
            dgvResults.Location = new Point(4, 262);
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvResults.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvResults.Size = new Size(912, 274);
            dgvResults.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(330, 79);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(250, 3);
            flowLayoutPanel1.TabIndex = 16;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 209);
            label1.Name = "label1";
            label1.Size = new Size(71, 25);
            label1.TabIndex = 1;
            label1.Text = "Search";
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 165, 255);
            label3.Location = new Point(256, 30);
            label3.Name = "label3";
            label3.Size = new Size(398, 46);
            label3.TabIndex = 17;
            label3.Text = "Search in File Index";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(101, 205);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(287, 29);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged_1;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(cmb_Status);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(exportBtn);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtSearch);
            panel1.Location = new Point(5, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(911, 256);
            panel1.TabIndex = 36;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radio_Date);
            groupBox1.Controls.Add(radio_Search);
            groupBox1.Location = new Point(382, 99);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(292, 78);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Print Selection";
            // 
            // radio_Date
            // 
            radio_Date.AutoSize = true;
            radio_Date.Location = new Point(144, 33);
            radio_Date.Name = "radio_Date";
            radio_Date.Size = new Size(128, 25);
            radio_Date.TabIndex = 1;
            radio_Date.TabStop = true;
            radio_Date.Text = "Print by Stutas";
            radio_Date.UseVisualStyleBackColor = true;
            // 
            // radio_Search
            // 
            radio_Search.AutoSize = true;
            radio_Search.Location = new Point(19, 33);
            radio_Search.Name = "radio_Search";
            radio_Search.Size = new Size(112, 25);
            radio_Search.TabIndex = 0;
            radio_Search.TabStop = true;
            radio_Search.Text = "Print Search";
            radio_Search.UseVisualStyleBackColor = true;
            // 
            // cmb_Status
            // 
            cmb_Status.FormattingEnabled = true;
            cmb_Status.Location = new Point(101, 161);
            cmb_Status.Name = "cmb_Status";
            cmb_Status.Size = new Size(185, 29);
            cmb_Status.TabIndex = 18;
            toolTip1.SetToolTip(cmb_Status, "\"Select Status to Filter | Press 'Delete' to Clear Filter\"");
            cmb_Status.SelectedIndexChanged += cmb_Status_SelectedIndexChanged;
            cmb_Status.KeyDown += cmb_Status_KeyDown;
            // 
            // exportBtn
            // 
            exportBtn.BackColor = Color.FromArgb(0, 165, 255);
            exportBtn.Cursor = Cursors.Hand;
            exportBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportBtn.ForeColor = Color.White;
            exportBtn.Location = new Point(411, 194);
            exportBtn.Margin = new Padding(1);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(169, 46);
            exportBtn.TabIndex = 2;
            exportBtn.Text = "Print";
            exportBtn.UseVisualStyleBackColor = false;
            exportBtn.Click += exportBtn_Click;
            // 
            // searchFile
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(921, 551);
            Controls.Add(panel1);
            Controls.Add(dgvResults);
            FormBorderStyle = FormBorderStyle.None;
            Name = "searchFile";
            Text = "searchFile";
            WindowState = FormWindowState.Maximized;
            Load += searchFile_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dgvResults;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private Label label3;
        private TextBox txtSearch;
        private Panel panel1;
        private ComboBox cmb_Status;
        private GroupBox groupBox1;
        private RadioButton radio_Date;
        private RadioButton radio_Search;
        private Button exportBtn;
        private ToolTip toolTip1;
    }
}