namespace SupplyBranch.Forms.Transactions
{
    partial class frmIndentSearch
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
            this.cmbZone = new System.Windows.Forms.ComboBox();
            this.cmbOffice = new System.Windows.Forms.ComboBox();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.cmbIndentNo = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvIndent = new System.Windows.Forms.DataGridView();
            this.IndentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IndentNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IndentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OfficeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSearchIndentNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTotalRecord = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndent)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbZone
            // 
            this.cmbZone.FormattingEnabled = true;
            this.cmbZone.Location = new System.Drawing.Point(83, 51);
            this.cmbZone.Name = "cmbZone";
            this.cmbZone.Size = new System.Drawing.Size(310, 21);
            this.cmbZone.TabIndex = 0;
            this.cmbZone.SelectedIndexChanged += new System.EventHandler(this.cmbZone_SelectedIndexChanged);
            // 
            // cmbOffice
            // 
            this.cmbOffice.FormattingEnabled = true;
            this.cmbOffice.Location = new System.Drawing.Point(83, 83);
            this.cmbOffice.Name = "cmbOffice";
            this.cmbOffice.Size = new System.Drawing.Size(310, 21);
            this.cmbOffice.TabIndex = 1;
            // 
            // dtFrom
            // 
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(83, 151);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(118, 20);
            this.dtFrom.TabIndex = 3;
            this.dtFrom.Value = new System.DateTime(2026, 8, 10, 22, 0, 0, 0);
            // 
            // dtTo
            // 
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(275, 151);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(118, 20);
            this.dtTo.TabIndex = 4;
            // 
            // cmbIndentNo
            // 
            this.cmbIndentNo.FormattingEnabled = true;
            this.cmbIndentNo.Location = new System.Drawing.Point(83, 115);
            this.cmbIndentNo.Name = "cmbIndentNo";
            this.cmbIndentNo.Size = new System.Drawing.Size(310, 21);
            this.cmbIndentNo.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(139, 218);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 35);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(234, 218);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 35);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dgvIndent
            // 
            this.dgvIndent.AllowUserToAddRows = false;
            this.dgvIndent.AllowUserToDeleteRows = false;
            this.dgvIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvIndent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIndent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIndent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndentID,
            this.IndentNo,
            this.IndentDate,
            this.OfficeName,
            this.TotalItems,
            this.Remarks,
            this.colEdit,
            this.colDelete});
            this.dgvIndent.Location = new System.Drawing.Point(2, 319);
            this.dgvIndent.MultiSelect = false;
            this.dgvIndent.Name = "dgvIndent";
            this.dgvIndent.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvIndent.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvIndent.RowHeadersVisible = false;
            this.dgvIndent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIndent.Size = new System.Drawing.Size(757, 211);
            this.dgvIndent.TabIndex = 11;
            this.dgvIndent.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIndent_CellClick);
            // 
            // IndentID
            // 
            this.IndentID.DataPropertyName = "IndentID";
            this.IndentID.HeaderText = "IndentID";
            this.IndentID.Name = "IndentID";
            this.IndentID.ReadOnly = true;
            this.IndentID.Visible = false;
            // 
            // IndentNo
            // 
            this.IndentNo.DataPropertyName = "IndentNo";
            this.IndentNo.HeaderText = "Indent No";
            this.IndentNo.Name = "IndentNo";
            this.IndentNo.ReadOnly = true;
            // 
            // IndentDate
            // 
            this.IndentDate.DataPropertyName = "IndentDate";
            this.IndentDate.HeaderText = "Indent Date";
            this.IndentDate.Name = "IndentDate";
            this.IndentDate.ReadOnly = true;
            // 
            // OfficeName
            // 
            this.OfficeName.DataPropertyName = "OfficeName";
            this.OfficeName.HeaderText = "Office";
            this.OfficeName.Name = "OfficeName";
            this.OfficeName.ReadOnly = true;
            // 
            // TotalItems
            // 
            this.TotalItems.DataPropertyName = "TotalItems";
            this.TotalItems.HeaderText = "Total Items";
            this.TotalItems.Name = "TotalItems";
            this.TotalItems.ReadOnly = true;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            this.Remarks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colEdit
            // 
            this.colEdit.DataPropertyName = "Edit";
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            // 
            // colDelete
            // 
            this.colDelete.DataPropertyName = "Delete";
            this.colDelete.HeaderText = "Delete";
            this.colDelete.Name = "colDelete";
            this.colDelete.ReadOnly = true;
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Zone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Office";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Indent No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "From";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(222, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "To";
            // 
            // txtSearchIndentNo
            // 
            this.txtSearchIndentNo.Location = new System.Drawing.Point(83, 189);
            this.txtSearchIndentNo.Name = "txtSearchIndentNo";
            this.txtSearchIndentNo.Size = new System.Drawing.Size(310, 20);
            this.txtSearchIndentNo.TabIndex = 5;
            this.txtSearchIndentNo.TextChanged += new System.EventHandler(this.txtSearchIndentNo_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Search";
            // 
            // lblTotalRecord
            // 
            this.lblTotalRecord.AutoSize = true;
            this.lblTotalRecord.Location = new System.Drawing.Point(398, 168);
            this.lblTotalRecord.Name = "lblTotalRecord";
            this.lblTotalRecord.Size = new System.Drawing.Size(0, 13);
            this.lblTotalRecord.TabIndex = 20;
            // 
            // frmIndentSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(756, 532);
            this.Controls.Add(this.lblTotalRecord);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSearchIndentNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvIndent);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.cmbIndentNo);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.cmbOffice);
            this.Controls.Add(this.cmbZone);
            this.Name = "frmIndentSearch";
            this.Text = "Indent Correction";
            this.Load += new System.EventHandler(this.frmIndentSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbZone;
        private System.Windows.Forms.ComboBox cmbOffice;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.ComboBox cmbIndentNo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvIndent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSearchIndentNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndentNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OfficeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.Label lblTotalRecord;
    }
}