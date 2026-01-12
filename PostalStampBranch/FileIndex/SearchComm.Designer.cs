namespace FileIndex
{
    partial class SearchComm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label3 = new Label();
            exportBtn = new Button();
            dgvResults = new DataGridView();
            label1 = new Label();
            txtSearch = new TextBox();
            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();
            label2 = new Label();
            From = new Label();
            searchBtn = new Button();
            checkBox1 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(337, 82);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(250, 3);
            flowLayoutPanel1.TabIndex = 22;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 165, 255);
            label3.Location = new Point(263, 33);
            label3.Name = "label3";
            label3.Size = new Size(398, 46);
            label3.TabIndex = 23;
            label3.Text = "Search Commemorative Issue";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // exportBtn
            // 
            exportBtn.BackColor = Color.FromArgb(0, 165, 255);
            exportBtn.Cursor = Cursors.Hand;
            exportBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportBtn.ForeColor = Color.White;
            exportBtn.Location = new Point(688, 108);
            exportBtn.Margin = new Padding(1);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(169, 46);
            exportBtn.TabIndex = 20;
            exportBtn.Text = "Print";
            exportBtn.UseVisualStyleBackColor = false;
            exportBtn.Click += exportBtn_Click;
            // 
            // dgvResults
            // 
            dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvResults.BackgroundColor = SystemColors.Control;
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Location = new Point(1, 233);
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvResults.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvResults.Size = new Size(970, 373);
            dgvResults.TabIndex = 21;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(126, 129);
            label1.Name = "label1";
            label1.Size = new Size(71, 25);
            label1.TabIndex = 18;
            label1.Text = "Search";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(223, 125);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(394, 29);
            txtSearch.TabIndex = 19;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dtpFrom
            // 
            dtpFrom.Enabled = false;
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(284, 190);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(114, 29);
            dtpFrom.TabIndex = 24;
            dtpFrom.Value = new DateTime(1947, 1, 14, 9, 52, 0, 0);
            // 
            // dtpTo
            // 
            dtpTo.Enabled = false;
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(523, 191);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(114, 29);
            dtpTo.TabIndex = 25;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(484, 193);
            label2.Name = "label2";
            label2.Size = new Size(33, 25);
            label2.TabIndex = 26;
            label2.Text = "To";
            // 
            // From
            // 
            From.AutoSize = true;
            From.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            From.ForeColor = Color.White;
            From.Location = new Point(190, 194);
            From.Name = "From";
            From.Size = new Size(59, 25);
            From.TabIndex = 27;
            From.Text = "From";
            // 
            // searchBtn
            // 
            searchBtn.BackColor = Color.FromArgb(0, 165, 255);
            searchBtn.Cursor = Cursors.Hand;
            searchBtn.Enabled = false;
            searchBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            searchBtn.ForeColor = Color.White;
            searchBtn.Location = new Point(688, 183);
            searchBtn.Margin = new Padding(1);
            searchBtn.Name = "searchBtn";
            searchBtn.Size = new Size(169, 46);
            searchBtn.TabIndex = 28;
            searchBtn.Text = "Search";
            searchBtn.UseVisualStyleBackColor = false;
            searchBtn.Click += searchBtn_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(10, 198);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(163, 29);
            checkBox1.TabIndex = 29;
            checkBox1.Text = "Search by Date";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // SearchComm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(968, 720);
            Controls.Add(checkBox1);
            Controls.Add(searchBtn);
            Controls.Add(From);
            Controls.Add(label2);
            Controls.Add(dtpTo);
            Controls.Add(dtpFrom);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label3);
            Controls.Add(exportBtn);
            Controls.Add(dgvResults);
            Controls.Add(label1);
            Controls.Add(txtSearch);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SearchComm";
            Text = "SearchComm";
            WindowState = FormWindowState.Maximized;
            Load += SearchComm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Label label3;
        private Button exportBtn;
        private DataGridView dgvResults;
        private Label label1;
        private TextBox txtSearch;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Label label2;
        private Label From;
        private Button searchBtn;
        private CheckBox checkBox1;
    }
}