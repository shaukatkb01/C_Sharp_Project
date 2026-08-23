namespace SupplyBranch.Forms
{
    partial class frmIndent
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblStampPersheet = new System.Windows.Forms.Label();
            this.lblTotalPieces = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPieceQty = new System.Windows.Forms.TextBox();
            this.txtSheetQty = new System.Windows.Forms.TextBox();
            this.btnClearItem = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtIndentRemarks = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.cmbDenomination = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpIndentDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIndentNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbZone = new System.Windows.Forms.ComboBox();
            this.cmbOffice = new System.Windows.Forms.ComboBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblStampPersheet);
            this.panel2.Controls.Add(this.lblTotalPieces);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.txtPieceQty);
            this.panel2.Controls.Add(this.txtSheetQty);
            this.panel2.Controls.Add(this.btnClearItem);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.txtIndentRemarks);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cmbCategory);
            this.panel2.Controls.Add(this.btnAddItem);
            this.panel2.Controls.Add(this.cmbDenomination);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dtpIndentDate);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtIndentNo);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cmbZone);
            this.panel2.Controls.Add(this.cmbOffice);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(852, 279);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // lblStampPersheet
            // 
            this.lblStampPersheet.AutoSize = true;
            this.lblStampPersheet.Location = new System.Drawing.Point(705, 171);
            this.lblStampPersheet.Name = "lblStampPersheet";
            this.lblStampPersheet.Size = new System.Drawing.Size(0, 13);
            this.lblStampPersheet.TabIndex = 57;
            // 
            // lblTotalPieces
            // 
            this.lblTotalPieces.AutoSize = true;
            this.lblTotalPieces.Location = new System.Drawing.Point(705, 198);
            this.lblTotalPieces.Name = "lblTotalPieces";
            this.lblTotalPieces.Size = new System.Drawing.Size(0, 13);
            this.lblTotalPieces.TabIndex = 53;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(513, 162);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 18);
            this.label10.TabIndex = 47;
            this.label10.Text = "Piece Qty";
            // 
            // txtPieceQty
            // 
            this.txtPieceQty.Location = new System.Drawing.Point(582, 163);
            this.txtPieceQty.Name = "txtPieceQty";
            this.txtPieceQty.Size = new System.Drawing.Size(67, 20);
            this.txtPieceQty.TabIndex = 7;
            this.txtPieceQty.TextChanged += new System.EventHandler(this.txtPieceQty_TextChanged);
            this.txtPieceQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPieceQty_KeyPress);
            this.txtPieceQty.Leave += new System.EventHandler(this.txtPieceQty_Leave);
            // 
            // txtSheetQty
            // 
            this.txtSheetQty.Location = new System.Drawing.Point(440, 160);
            this.txtSheetQty.Name = "txtSheetQty";
            this.txtSheetQty.Size = new System.Drawing.Size(67, 20);
            this.txtSheetQty.TabIndex = 6;
            this.txtSheetQty.TextChanged += new System.EventHandler(this.txtSheetQty_TextChanged);
            this.txtSheetQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSheetQty_KeyPress);
            this.txtSheetQty.Leave += new System.EventHandler(this.txtSheetQty_Leave);
            // 
            // btnClearItem
            // 
            this.btnClearItem.Location = new System.Drawing.Point(508, 233);
            this.btnClearItem.Name = "btnClearItem";
            this.btnClearItem.Size = new System.Drawing.Size(93, 32);
            this.btnClearItem.TabIndex = 10;
            this.btnClearItem.Text = "Clear";
            this.btnClearItem.UseVisualStyleBackColor = true;
            this.btnClearItem.Click += new System.EventHandler(this.btnClearItem_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 18);
            this.label9.TabIndex = 43;
            this.label9.Text = "Remark";
            // 
            // txtIndentRemarks
            // 
            this.txtIndentRemarks.Location = new System.Drawing.Point(114, 196);
            this.txtIndentRemarks.MaxLength = 500;
            this.txtIndentRemarks.Multiline = true;
            this.txtIndentRemarks.Name = "txtIndentRemarks";
            this.txtIndentRemarks.Size = new System.Drawing.Size(535, 25);
            this.txtIndentRemarks.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(252, 233);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 32);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(346, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 18);
            this.label8.TabIndex = 15;
            this.label8.Text = "Sheet Qty";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 18);
            this.label7.TabIndex = 14;
            this.label7.Text = "Denomination";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(346, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "Category";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownHeight = 1060;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.DropDownWidth = 400;
            this.cmbCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.IntegralHeight = false;
            this.cmbCategory.Location = new System.Drawing.Point(440, 112);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(209, 26);
            this.cmbCategory.TabIndex = 4;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(373, 233);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(93, 32);
            this.btnAddItem.TabIndex = 9;
            this.btnAddItem.Text = "Add";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // cmbDenomination
            // 
            this.cmbDenomination.DropDownHeight = 1060;
            this.cmbDenomination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDenomination.DropDownWidth = 400;
            this.cmbDenomination.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDenomination.FormattingEnabled = true;
            this.cmbDenomination.IntegralHeight = false;
            this.cmbDenomination.Location = new System.Drawing.Point(114, 154);
            this.cmbDenomination.Name = "cmbDenomination";
            this.cmbDenomination.Size = new System.Drawing.Size(209, 26);
            this.cmbDenomination.TabIndex = 5;
            this.cmbDenomination.SelectedIndexChanged += new System.EventHandler(this.cmbDenomination_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "Office";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Zone";
            // 
            // dtpIndentDate
            // 
            this.dtpIndentDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpIndentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpIndentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpIndentDate.Location = new System.Drawing.Point(440, 72);
            this.dtpIndentDate.Name = "dtpIndentDate";
            this.dtpIndentDate.Size = new System.Drawing.Size(209, 24);
            this.dtpIndentDate.TabIndex = 1;
            this.dtpIndentDate.Value = new System.DateTime(2026, 7, 15, 18, 46, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(346, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Indent Date";
            // 
            // txtIndentNo
            // 
            this.txtIndentNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIndentNo.Location = new System.Drawing.Point(115, 40);
            this.txtIndentNo.Name = "txtIndentNo";
            this.txtIndentNo.Size = new System.Drawing.Size(534, 24);
            this.txtIndentNo.TabIndex = 0;
            this.txtIndentNo.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Indent details";
            // 
            // cmbZone
            // 
            this.cmbZone.DropDownHeight = 1060;
            this.cmbZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZone.DropDownWidth = 400;
            this.cmbZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbZone.FormattingEnabled = true;
            this.cmbZone.IntegralHeight = false;
            this.cmbZone.ItemHeight = 18;
            this.cmbZone.Location = new System.Drawing.Point(115, 75);
            this.cmbZone.Name = "cmbZone";
            this.cmbZone.Size = new System.Drawing.Size(209, 26);
            this.cmbZone.TabIndex = 2;
            this.cmbZone.SelectedIndexChanged += new System.EventHandler(this.cmbZone_SelectedIndexChanged_1);
            // 
            // cmbOffice
            // 
            this.cmbOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOffice.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOffice.FormattingEnabled = true;
            this.cmbOffice.Location = new System.Drawing.Point(115, 112);
            this.cmbOffice.Name = "cmbOffice";
            this.cmbOffice.Size = new System.Drawing.Size(209, 26);
            this.cmbOffice.TabIndex = 3;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(0, 0);
            this.dgvItems.MultiSelect = false;
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItems.Size = new System.Drawing.Size(852, 269);
            this.dgvItems.TabIndex = 10;
            this.dgvItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvItems);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 279);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(852, 269);
            this.panel1.TabIndex = 11;
            // 
            // frmIndent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(852, 548);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmIndent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Indent Entry";
            this.Load += new System.EventHandler(this.frmIndent_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbOffice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIndentNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbZone;
        private System.Windows.Forms.DateTimePicker dtpIndentDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDenomination;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnClearItem;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIndentRemarks;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPieceQty;
        private System.Windows.Forms.TextBox txtSheetQty;
        private System.Windows.Forms.Label lblTotalPieces;
        private System.Windows.Forms.Label lblStampPersheet;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Panel panel1;
    }
}