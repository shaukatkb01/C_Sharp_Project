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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dgvResults = new DataGridView();
            panel1 = new Panel();
            picPopup = new PictureBox();
            checkBox1 = new CheckBox();
            searchBtn = new Button();
            From = new Label();
            label2 = new Label();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label3 = new Label();
            exportBtn = new Button();
            label1 = new Label();
            txtSearch = new TextBox();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPopup).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // dgvResults
            // 
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
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.Location = new Point(0, 0);
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
            dgvResults.Size = new Size(968, 504);
            dgvResults.TabIndex = 21;
            dgvResults.CellMouseEnter += dgvResults_CellMouseEnter;
            dgvResults.CellMouseLeave += dgvResults_CellMouseLeave;
            // 
            // panel1
            // 
            panel1.Controls.Add(picPopup);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(searchBtn);
            panel1.Controls.Add(From);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(dtpTo);
            panel1.Controls.Add(dtpFrom);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(exportBtn);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtSearch);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(968, 216);
            panel1.TabIndex = 22;
            // 
            // picPopup
            // 
            picPopup.Location = new Point(145, 247);
            picPopup.Name = "picPopup";
            picPopup.Size = new Size(690, 357);
            picPopup.SizeMode = PictureBoxSizeMode.Zoom;
            picPopup.TabIndex = 41;
            picPopup.TabStop = false;
            picPopup.Visible = false;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(44, 157);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(163, 29);
            checkBox1.TabIndex = 40;
            checkBox1.Text = "Search by Date";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // searchBtn
            // 
            searchBtn.BackColor = Color.FromArgb(0, 165, 255);
            searchBtn.Cursor = Cursors.Hand;
            searchBtn.Enabled = false;
            searchBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            searchBtn.ForeColor = Color.White;
            searchBtn.Location = new Point(722, 140);
            searchBtn.Margin = new Padding(1);
            searchBtn.Name = "searchBtn";
            searchBtn.Size = new Size(169, 46);
            searchBtn.TabIndex = 39;
            searchBtn.Text = "Search";
            searchBtn.UseVisualStyleBackColor = false;
            searchBtn.Click += searchBtn_Click;
            // 
            // From
            // 
            From.AutoSize = true;
            From.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            From.ForeColor = Color.White;
            From.Location = new Point(224, 161);
            From.Name = "From";
            From.Size = new Size(59, 25);
            From.TabIndex = 38;
            From.Text = "From";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(518, 161);
            label2.Name = "label2";
            label2.Size = new Size(33, 25);
            label2.TabIndex = 37;
            label2.Text = "To";
            // 
            // dtpTo
            // 
            dtpTo.Enabled = false;
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(557, 157);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(114, 29);
            dtpTo.TabIndex = 36;
            // 
            // dtpFrom
            // 
            dtpFrom.Enabled = false;
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(318, 157);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(114, 29);
            dtpFrom.TabIndex = 35;
            dtpFrom.Value = new DateTime(1947, 1, 14, 9, 52, 0, 0);
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(359, 51);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(250, 3);
            flowLayoutPanel1.TabIndex = 33;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 165, 255);
            label3.Location = new Point(285, 2);
            label3.Name = "label3";
            label3.Size = new Size(398, 46);
            label3.TabIndex = 34;
            label3.Text = "Search Commemorative Issue";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // exportBtn
            // 
            exportBtn.BackColor = Color.FromArgb(0, 165, 255);
            exportBtn.Cursor = Cursors.Hand;
            exportBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportBtn.ForeColor = Color.White;
            exportBtn.Location = new Point(722, 77);
            exportBtn.Margin = new Padding(1);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(169, 46);
            exportBtn.TabIndex = 32;
            exportBtn.Text = "Print";
            exportBtn.UseVisualStyleBackColor = false;
            exportBtn.Click += exportBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(184, 98);
            label1.Name = "label1";
            label1.Size = new Size(71, 25);
            label1.TabIndex = 30;
            label1.Text = "Search";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(287, 94);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(394, 29);
            txtSearch.TabIndex = 31;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(dgvResults);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 216);
            panel2.Name = "panel2";
            panel2.Size = new Size(968, 504);
            panel2.TabIndex = 23;
            // 
            // SearchComm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(968, 720);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SearchComm";
            Text = "SearchComm";
            WindowState = FormWindowState.Maximized;
            Load += SearchComm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPopup).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dgvResults;
        private Panel panel1;
        private CheckBox checkBox1;
        private Button searchBtn;
        private Label From;
        private Label label2;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label3;
        private Button exportBtn;
        private Label label1;
        private TextBox txtSearch;
        private Panel panel2;
        private PictureBox picPopup;
    }
}