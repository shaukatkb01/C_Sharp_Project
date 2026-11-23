namespace SupplyBranch.Forms.Masters
{
    partial class frmOffice
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
            this.lblZone = new System.Windows.Forms.Label();
            this.lblOfficeName = new System.Windows.Forms.Label();
            this.lblOfficeFileNo = new System.Windows.Forms.Label();
            this.lblOfficeCode = new System.Windows.Forms.Label();
            this.cmbZone = new System.Windows.Forms.ComboBox();
            this.txtOfficeName = new System.Windows.Forms.TextBox();
            this.txtOfficeCode = new System.Windows.Forms.TextBox();
            this.txtOfficeFileNo = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvOffice = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOffice)).BeginInit();
            this.SuspendLayout();
            // 
            // lblZone
            // 
            this.lblZone.AutoSize = true;
            this.lblZone.Location = new System.Drawing.Point(44, 44);
            this.lblZone.Name = "lblZone";
            this.lblZone.Size = new System.Drawing.Size(32, 13);
            this.lblZone.TabIndex = 0;
            this.lblZone.Text = "Zone";
            // 
            // lblOfficeName
            // 
            this.lblOfficeName.AutoSize = true;
            this.lblOfficeName.Location = new System.Drawing.Point(41, 75);
            this.lblOfficeName.Name = "lblOfficeName";
            this.lblOfficeName.Size = new System.Drawing.Size(66, 13);
            this.lblOfficeName.TabIndex = 1;
            this.lblOfficeName.Text = "Office Name";
            // 
            // lblOfficeFileNo
            // 
            this.lblOfficeFileNo.AutoSize = true;
            this.lblOfficeFileNo.Location = new System.Drawing.Point(44, 111);
            this.lblOfficeFileNo.Name = "lblOfficeFileNo";
            this.lblOfficeFileNo.Size = new System.Drawing.Size(71, 13);
            this.lblOfficeFileNo.TabIndex = 2;
            this.lblOfficeFileNo.Text = "Office File No";
            // 
            // lblOfficeCode
            // 
            this.lblOfficeCode.AutoSize = true;
            this.lblOfficeCode.Location = new System.Drawing.Point(44, 147);
            this.lblOfficeCode.Name = "lblOfficeCode";
            this.lblOfficeCode.Size = new System.Drawing.Size(63, 13);
            this.lblOfficeCode.TabIndex = 3;
            this.lblOfficeCode.Text = "Office Code";
            // 
            // cmbZone
            // 
            this.cmbZone.FormattingEnabled = true;
            this.cmbZone.Location = new System.Drawing.Point(133, 36);
            this.cmbZone.Name = "cmbZone";
            this.cmbZone.Size = new System.Drawing.Size(285, 21);
            this.cmbZone.TabIndex = 0;
            // 
            // txtOfficeName
            // 
            this.txtOfficeName.Location = new System.Drawing.Point(133, 73);
            this.txtOfficeName.Name = "txtOfficeName";
            this.txtOfficeName.Size = new System.Drawing.Size(285, 20);
            this.txtOfficeName.TabIndex = 1;
            // 
            // txtOfficeCode
            // 
            this.txtOfficeCode.Location = new System.Drawing.Point(133, 145);
            this.txtOfficeCode.Name = "txtOfficeCode";
            this.txtOfficeCode.Size = new System.Drawing.Size(285, 20);
            this.txtOfficeCode.TabIndex = 3;
            // 
            // txtOfficeFileNo
            // 
            this.txtOfficeFileNo.Location = new System.Drawing.Point(133, 109);
            this.txtOfficeFileNo.Name = "txtOfficeFileNo";
            this.txtOfficeFileNo.Size = new System.Drawing.Size(285, 20);
            this.txtOfficeFileNo.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(133, 181);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(285, 20);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(137, 213);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(79, 40);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(238, 213);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 40);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(339, 213);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(79, 40);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dgvOffice
            // 
            this.dgvOffice.AllowUserToAddRows = false;
            this.dgvOffice.AllowUserToDeleteRows = false;
            this.dgvOffice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOffice.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOffice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOffice.Location = new System.Drawing.Point(0, 272);
            this.dgvOffice.Name = "dgvOffice";
            this.dgvOffice.ReadOnly = true;
            this.dgvOffice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOffice.Size = new System.Drawing.Size(788, 177);
            this.dgvOffice.TabIndex = 13;
            this.dgvOffice.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOffice_CellDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Search : ";
            // 
            // frmOffice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvOffice);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.txtOfficeFileNo);
            this.Controls.Add(this.txtOfficeCode);
            this.Controls.Add(this.txtOfficeName);
            this.Controls.Add(this.cmbZone);
            this.Controls.Add(this.lblOfficeCode);
            this.Controls.Add(this.lblOfficeFileNo);
            this.Controls.Add(this.lblOfficeName);
            this.Controls.Add(this.lblZone);
            this.Name = "frmOffice";
            this.Text = "Office Information";
            this.Load += new System.EventHandler(this.frmOffice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOffice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblZone;
        private System.Windows.Forms.Label lblOfficeName;
        private System.Windows.Forms.Label lblOfficeFileNo;
        private System.Windows.Forms.Label lblOfficeCode;
        private System.Windows.Forms.ComboBox cmbZone;
        private System.Windows.Forms.TextBox txtOfficeName;
        private System.Windows.Forms.TextBox txtOfficeCode;
        private System.Windows.Forms.TextBox txtOfficeFileNo;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dgvOffice;
        private System.Windows.Forms.Label label1;
    }
}