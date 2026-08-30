namespace SupplyBranch.Forms.Transactions
{
    partial class frmSupplySearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmbOffice = new System.Windows.Forms.ComboBox();
            this.cmbZone = new System.Windows.Forms.ComboBox();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.txtSearchIndentNo = new System.Windows.Forms.TextBox();
            this.rbPending = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbCompleted = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnNewSupply = new System.Windows.Forms.Button();
            this.dgvSupply = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupply)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbOffice
            // 
            this.cmbOffice.FormattingEnabled = true;
            this.cmbOffice.Location = new System.Drawing.Point(57, 98);
            this.cmbOffice.Name = "cmbOffice";
            this.cmbOffice.Size = new System.Drawing.Size(340, 21);
            this.cmbOffice.TabIndex = 1;
            this.cmbOffice.SelectedIndexChanged += new System.EventHandler(this.cmbOffice_SelectedIndexChanged);
            // 
            // cmbZone
            // 
            this.cmbZone.FormattingEnabled = true;
            this.cmbZone.Location = new System.Drawing.Point(57, 55);
            this.cmbZone.Name = "cmbZone";
            this.cmbZone.Size = new System.Drawing.Size(340, 21);
            this.cmbZone.TabIndex = 0;
            this.cmbZone.SelectedIndexChanged += new System.EventHandler(this.cmbZone_SelectedIndexChanged);
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(57, 140);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(131, 20);
            this.dtFrom.TabIndex = 2;
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(266, 140);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(131, 20);
            this.dtTo.TabIndex = 3;
            // 
            // txtSearchIndentNo
            // 
            this.txtSearchIndentNo.Location = new System.Drawing.Point(57, 181);
            this.txtSearchIndentNo.Name = "txtSearchIndentNo";
            this.txtSearchIndentNo.Size = new System.Drawing.Size(340, 20);
            this.txtSearchIndentNo.TabIndex = 4;
            // 
            // rbPending
            // 
            this.rbPending.AutoSize = true;
            this.rbPending.Checked = true;
            this.rbPending.Location = new System.Drawing.Point(77, 22);
            this.rbPending.Name = "rbPending";
            this.rbPending.Size = new System.Drawing.Size(64, 17);
            this.rbPending.TabIndex = 6;
            this.rbPending.TabStop = true;
            this.rbPending.Text = "Pending";
            this.rbPending.UseVisualStyleBackColor = true;
            this.rbPending.CheckedChanged += new System.EventHandler(this.rbPending_CheckedChanged);
            this.rbPending.Click += new System.EventHandler(this.rbPending_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(193, 22);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 7;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            this.rbAll.Click += new System.EventHandler(this.rbAll_Click);
            // 
            // rbCompleted
            // 
            this.rbCompleted.AutoSize = true;
            this.rbCompleted.Location = new System.Drawing.Point(281, 22);
            this.rbCompleted.Name = "rbCompleted";
            this.rbCompleted.Size = new System.Drawing.Size(75, 17);
            this.rbCompleted.TabIndex = 8;
            this.rbCompleted.Text = "Completed";
            this.rbCompleted.UseVisualStyleBackColor = true;
            this.rbCompleted.CheckedChanged += new System.EventHandler(this.rbCompleted_CheckedChanged);
            this.rbCompleted.Click += new System.EventHandler(this.rbCompleted_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(57, 207);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(340, 34);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnNewSupply
            // 
            this.btnNewSupply.Location = new System.Drawing.Point(57, 247);
            this.btnNewSupply.Name = "btnNewSupply";
            this.btnNewSupply.Size = new System.Drawing.Size(340, 34);
            this.btnNewSupply.TabIndex = 6;
            this.btnNewSupply.Text = "New Supply";
            this.btnNewSupply.UseVisualStyleBackColor = true;
            this.btnNewSupply.Click += new System.EventHandler(this.btnNewSupply_Click);
            // 
            // dgvSupply
            // 
            this.dgvSupply.AllowUserToAddRows = false;
            this.dgvSupply.AllowUserToDeleteRows = false;
            this.dgvSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSupply.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSupply.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSupply.Location = new System.Drawing.Point(1, 287);
            this.dgvSupply.MultiSelect = false;
            this.dgvSupply.Name = "dgvSupply";
            this.dgvSupply.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSupply.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSupply.RowHeadersVisible = false;
            this.dgvSupply.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupply.Size = new System.Drawing.Size(862, 328);
            this.dgvSupply.TabIndex = 13;
            this.dgvSupply.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSupply_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Zone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Office";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(211, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Search";
            // 
            // frmSupplySearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(866, 620);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvSupply);
            this.Controls.Add(this.btnNewSupply);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.rbCompleted);
            this.Controls.Add(this.rbAll);
            this.Controls.Add(this.rbPending);
            this.Controls.Add(this.txtSearchIndentNo);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.cmbZone);
            this.Controls.Add(this.cmbOffice);
            this.Name = "frmSupplySearch";
            this.Text = "Supply Search";
            this.Load += new System.EventHandler(this.frmSupplySearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupply)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbOffice;
        private System.Windows.Forms.ComboBox cmbZone;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.TextBox txtSearchIndentNo;
        private System.Windows.Forms.RadioButton rbPending;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbCompleted;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnNewSupply;
        private System.Windows.Forms.DataGridView dgvSupply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}