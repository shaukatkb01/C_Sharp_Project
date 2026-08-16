namespace SupplyBranch.Forms.Transactions
{
    partial class frmDraftSupply
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSupplyNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbOffice = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvDraft = new System.Windows.Forms.DataGridView();
            this.SupplyID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplyNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplyDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OfficeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cmbDraftStatus = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDraft)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Supply No";
            // 
            // txtSupplyNo
            // 
            this.txtSupplyNo.Location = new System.Drawing.Point(125, 85);
            this.txtSupplyNo.Name = "txtSupplyNo";
            this.txtSupplyNo.Size = new System.Drawing.Size(291, 20);
            this.txtSupplyNo.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Office";
            // 
            // cmbOffice
            // 
            this.cmbOffice.FormattingEnabled = true;
            this.cmbOffice.Location = new System.Drawing.Point(125, 116);
            this.cmbOffice.Name = "cmbOffice";
            this.cmbOffice.Size = new System.Drawing.Size(291, 21);
            this.cmbOffice.TabIndex = 3;
            this.cmbOffice.SelectedIndexChanged += new System.EventHandler(this.cmbOffice_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(246, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "To";
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(125, 148);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(104, 20);
            this.dtFrom.TabIndex = 6;
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(312, 148);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(104, 20);
            this.dtTo.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(125, 189);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(291, 34);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvDraft
            // 
            this.dgvDraft.AllowUserToAddRows = false;
            this.dgvDraft.AllowUserToDeleteRows = false;
            this.dgvDraft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDraft.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDraft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDraft.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SupplyID,
            this.SupplyNo,
            this.SupplyDate,
            this.OfficeName,
            this.StatusName,
            this.Delete});
            this.dgvDraft.Location = new System.Drawing.Point(3, 240);
            this.dgvDraft.MultiSelect = false;
            this.dgvDraft.Name = "dgvDraft";
            this.dgvDraft.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDraft.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDraft.RowHeadersVisible = false;
            this.dgvDraft.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDraft.Size = new System.Drawing.Size(806, 325);
            this.dgvDraft.TabIndex = 14;
            this.dgvDraft.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDraft_CellClick);
            this.dgvDraft.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDraft_CellDoubleClick);
            // 
            // SupplyID
            // 
            this.SupplyID.DataPropertyName = "SupplyID";
            this.SupplyID.HeaderText = "SupplyID";
            this.SupplyID.Name = "SupplyID";
            this.SupplyID.ReadOnly = true;
            this.SupplyID.Visible = false;
            // 
            // SupplyNo
            // 
            this.SupplyNo.DataPropertyName = "SupplyNo";
            this.SupplyNo.HeaderText = "SupplyNo";
            this.SupplyNo.Name = "SupplyNo";
            this.SupplyNo.ReadOnly = true;
            // 
            // SupplyDate
            // 
            this.SupplyDate.DataPropertyName = "SupplyDate";
            this.SupplyDate.HeaderText = "SupplyDate";
            this.SupplyDate.Name = "SupplyDate";
            this.SupplyDate.ReadOnly = true;
            // 
            // OfficeName
            // 
            this.OfficeName.DataPropertyName = "OfficeName";
            this.OfficeName.HeaderText = "OfficeName";
            this.OfficeName.Name = "OfficeName";
            this.OfficeName.ReadOnly = true;
            // 
            // StatusName
            // 
            this.StatusName.DataPropertyName = "StatusName";
            this.StatusName.HeaderText = "StatusName";
            this.StatusName.Name = "StatusName";
            this.StatusName.ReadOnly = true;
            // 
            // Delete
            // 
            this.Delete.DataPropertyName = "Delete";
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Text = "Delete";
            this.Delete.ToolTipText = "Delete draft";
            this.Delete.UseColumnTextForButtonValue = true;
            // 
            // cmbDraftStatus
            // 
            this.cmbDraftStatus.FormattingEnabled = true;
            this.cmbDraftStatus.Location = new System.Drawing.Point(125, 53);
            this.cmbDraftStatus.Name = "cmbDraftStatus";
            this.cmbDraftStatus.Size = new System.Drawing.Size(291, 21);
            this.cmbDraftStatus.TabIndex = 15;
            this.cmbDraftStatus.SelectedIndexChanged += new System.EventHandler(this.cmbDraftStatus_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Draft Status";
            // 
            // frmDraftSupply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 562);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbDraftStatus);
            this.Controls.Add(this.dgvDraft);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbOffice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSupplyNo);
            this.Controls.Add(this.label1);
            this.Name = "frmDraftSupply";
            this.Text = "DRAFT/APPROVED ADN ISSUED SUPPLY";
            this.Load += new System.EventHandler(this.dgvDraft_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDraft)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSupplyNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbOffice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvDraft;
        private System.Windows.Forms.ComboBox cmbDraftStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplyID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplyNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplyDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OfficeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusName;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
    }
}