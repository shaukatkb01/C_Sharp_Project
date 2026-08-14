namespace SupplyBranch.Forms.Masters
{
    partial class frmUnitConversionMaster
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
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.cmbDenomination = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPiecesPerSheet = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvConversion = new System.Windows.Forms.DataGridView();
            this.ConversionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DenominationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Denomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PiecesPerSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConversion)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(136, 73);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(389, 21);
            this.cmbCategory.TabIndex = 0;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // cmbDenomination
            // 
            this.cmbDenomination.FormattingEnabled = true;
            this.cmbDenomination.Location = new System.Drawing.Point(136, 114);
            this.cmbDenomination.Name = "cmbDenomination";
            this.cmbDenomination.Size = new System.Drawing.Size(389, 21);
            this.cmbDenomination.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Category";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Denomination";
            // 
            // txtPiecesPerSheet
            // 
            this.txtPiecesPerSheet.Location = new System.Drawing.Point(136, 155);
            this.txtPiecesPerSheet.Name = "txtPiecesPerSheet";
            this.txtPiecesPerSheet.Size = new System.Drawing.Size(389, 20);
            this.txtPiecesPerSheet.TabIndex = 4;
            this.txtPiecesPerSheet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPiecesPerSheet_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "PiecesPerSheet";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(390, 236);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(79, 33);
            this.btnUpdate.TabIndex = 46;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(170, 236);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(79, 33);
            this.btnNew.TabIndex = 44;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(280, 236);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 33);
            this.btnSave.TabIndex = 43;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvConversion
            // 
            this.dgvConversion.AllowUserToAddRows = false;
            this.dgvConversion.AllowUserToDeleteRows = false;
            this.dgvConversion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConversion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConversion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConversion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConversionID,
            this.CategoryID,
            this.Category,
            this.DenominationID,
            this.Denomination,
            this.PiecesPerSheet,
            this.Remarks,
            this.Edit,
            this.Delete});
            this.dgvConversion.Location = new System.Drawing.Point(3, 312);
            this.dgvConversion.MultiSelect = false;
            this.dgvConversion.Name = "dgvConversion";
            this.dgvConversion.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConversion.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvConversion.RowHeadersVisible = false;
            this.dgvConversion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConversion.Size = new System.Drawing.Size(850, 307);
            this.dgvConversion.TabIndex = 42;
            this.dgvConversion.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConversion_CellClick);
            // 
            // ConversionID
            // 
            this.ConversionID.DataPropertyName = "ConversionID";
            this.ConversionID.HeaderText = "ConversionID";
            this.ConversionID.Name = "ConversionID";
            this.ConversionID.ReadOnly = true;
            this.ConversionID.Visible = false;
            // 
            // CategoryID
            // 
            this.CategoryID.DataPropertyName = "CategoryID";
            this.CategoryID.HeaderText = "CategoryID";
            this.CategoryID.Name = "CategoryID";
            this.CategoryID.ReadOnly = true;
            this.CategoryID.Visible = false;
            // 
            // Category
            // 
            this.Category.DataPropertyName = "Category";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // DenominationID
            // 
            this.DenominationID.DataPropertyName = "DenominationID";
            this.DenominationID.HeaderText = "DenominationID";
            this.DenominationID.Name = "DenominationID";
            this.DenominationID.ReadOnly = true;
            this.DenominationID.Visible = false;
            // 
            // Denomination
            // 
            this.Denomination.DataPropertyName = "Denomination";
            this.Denomination.HeaderText = "Denomination";
            this.Denomination.Name = "Denomination";
            this.Denomination.ReadOnly = true;
            // 
            // PiecesPerSheet
            // 
            this.PiecesPerSheet.DataPropertyName = "PiecesPerSheet";
            this.PiecesPerSheet.HeaderText = "PiecesPerSheet";
            this.PiecesPerSheet.Name = "PiecesPerSheet";
            this.PiecesPerSheet.ReadOnly = true;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            // 
            // Edit
            // 
            this.Edit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Edit.Text = "Edit";
            this.Edit.UseColumnTextForButtonValue = true;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Text = "Delete";
            this.Delete.UseColumnTextForButtonValue = true;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Location = new System.Drawing.Point(343, 21);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(155, 13);
            this.lblSubTitle.TabIndex = 49;
            this.lblSubTitle.Text = "UNIT CONVERSION MASTER";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 52;
            this.label6.Text = "Remarks:";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(136, 195);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(389, 20);
            this.txtRemarks.TabIndex = 51;
            // 
            // frmUnitConversionMaster
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(861, 614);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.lblSubTitle);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvConversion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPiecesPerSheet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDenomination);
            this.Controls.Add(this.cmbCategory);
            this.Name = "frmUnitConversionMaster";
            this.Text = "UnitConversionMaster";
            this.Load += new System.EventHandler(this.UnitConversionMaster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConversion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ComboBox cmbDenomination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPiecesPerSheet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvConversion;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConversionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn DenominationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Denomination;
        private System.Windows.Forms.DataGridViewTextBoxColumn PiecesPerSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
    }
}