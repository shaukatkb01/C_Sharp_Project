namespace SupplyBranch.Forms.Stock
{
    partial class frmStockIn
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.cmbDenomination = new System.Windows.Forms.ComboBox();

            this.lblCategory = new System.Windows.Forms.Label();
            this.lblDenomination = new System.Windows.Forms.Label();

            this.txtBoxQty = new System.Windows.Forms.TextBox();
            this.txtPacketQty = new System.Windows.Forms.TextBox();
            this.txtSheetQty = new System.Windows.Forms.TextBox();
            this.txtStampQty = new System.Windows.Forms.TextBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();

            this.lblBoxQty = new System.Windows.Forms.Label();
            this.lblPacketQty = new System.Windows.Forms.Label();
            this.lblSheetQty = new System.Windows.Forms.Label();
            this.lblStampQty = new System.Windows.Forms.Label();
            this.lblRemarks = new System.Windows.Forms.Label();

            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();

            this.dgvStockIn = new System.Windows.Forms.DataGridView();

            this.TransactionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Denomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BoxQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PacketQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SheetQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StampQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvStockIn)).BeginInit();
            this.SuspendLayout();

            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(35, 35);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(49, 13);
            this.lblCategory.Text = "Category";

            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(145, 30);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(350, 21);
            this.cmbCategory.TabIndex = 0;
            this.cmbCategory.SelectedIndexChanged +=
                new System.EventHandler(this.cmbCategory_SelectedIndexChanged);

            // 
            // lblDenomination
            // 
            this.lblDenomination.AutoSize = true;
            this.lblDenomination.Location = new System.Drawing.Point(35, 75);
            this.lblDenomination.Name = "lblDenomination";
            this.lblDenomination.Size = new System.Drawing.Size(72, 13);
            this.lblDenomination.Text = "Denomination";

            // 
            // cmbDenomination
            // 
            this.cmbDenomination.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDenomination.FormattingEnabled = true;
            this.cmbDenomination.Location = new System.Drawing.Point(145, 70);
            this.cmbDenomination.Name = "cmbDenomination";
            this.cmbDenomination.Size = new System.Drawing.Size(350, 21);
            this.cmbDenomination.TabIndex = 1;

            // 
            // lblBoxQty
            // 
            this.lblBoxQty.AutoSize = true;
            this.lblBoxQty.Location = new System.Drawing.Point(35, 115);
            this.lblBoxQty.Name = "lblBoxQty";
            this.lblBoxQty.Size = new System.Drawing.Size(62, 13);
            this.lblBoxQty.Text = "Box Qty";

            // 
            // txtBoxQty
            // 
            this.txtBoxQty.Location = new System.Drawing.Point(145, 110);
            this.txtBoxQty.Name = "txtBoxQty";
            this.txtBoxQty.Size = new System.Drawing.Size(350, 20);
            this.txtBoxQty.TabIndex = 2;
            this.txtBoxQty.KeyPress +=
                new System.Windows.Forms.KeyPressEventHandler(
                    this.txtQuantity_KeyPress);

            // 
            // lblPacketQty
            // 
            this.lblPacketQty.AutoSize = true;
            this.lblPacketQty.Location = new System.Drawing.Point(35, 150);
            this.lblPacketQty.Name = "lblPacketQty";
            this.lblPacketQty.Size = new System.Drawing.Size(75, 13);
            this.lblPacketQty.Text = "Packet Qty";

            // 
            // txtPacketQty
            // 
            this.txtPacketQty.Location = new System.Drawing.Point(145, 145);
            this.txtPacketQty.Name = "txtPacketQty";
            this.txtPacketQty.Size = new System.Drawing.Size(350, 20);
            this.txtPacketQty.TabIndex = 3;
            this.txtPacketQty.KeyPress +=
                new System.Windows.Forms.KeyPressEventHandler(
                    this.txtQuantity_KeyPress);

            // 
            // lblSheetQty
            // 
            this.lblSheetQty.AutoSize = true;
            this.lblSheetQty.Location = new System.Drawing.Point(35, 185);
            this.lblSheetQty.Name = "lblSheetQty";
            this.lblSheetQty.Size = new System.Drawing.Size(65, 13);
            this.lblSheetQty.Text = "Sheet Qty";

            // 
            // txtSheetQty
            // 
            this.txtSheetQty.Location = new System.Drawing.Point(145, 180);
            this.txtSheetQty.Name = "txtSheetQty";
            this.txtSheetQty.Size = new System.Drawing.Size(350, 20);
            this.txtSheetQty.TabIndex = 4;
            this.txtSheetQty.KeyPress +=
                new System.Windows.Forms.KeyPressEventHandler(
                    this.txtQuantity_KeyPress);

            // 
            // lblStampQty
            // 
            this.lblStampQty.AutoSize = true;
            this.lblStampQty.Location = new System.Drawing.Point(35, 220);
            this.lblStampQty.Name = "lblStampQty";
            this.lblStampQty.Size = new System.Drawing.Size(67, 13);
            this.lblStampQty.Text = "Stamp Qty";

            // 
            // txtStampQty
            // 
            this.txtStampQty.Location = new System.Drawing.Point(145, 215);
            this.txtStampQty.Name = "txtStampQty";
            this.txtStampQty.Size = new System.Drawing.Size(350, 20);
            this.txtStampQty.TabIndex = 5;
            this.txtStampQty.KeyPress +=
                new System.Windows.Forms.KeyPressEventHandler(
                    this.txtQuantity_KeyPress);

            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(35, 260);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(52, 13);
            this.lblRemarks.Text = "Remarks";

            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(145, 255);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars =
                System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(350, 50);
            this.txtRemarks.TabIndex = 6;

            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(145, 325);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(90, 35);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click +=
                new System.EventHandler(this.btnNew_Click);

            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(255, 325);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click +=
                new System.EventHandler(this.btnSave_Click);

            // 
            // dgvStockIn
            // 
            this.dgvStockIn.AllowUserToAddRows = false;
            this.dgvStockIn.AllowUserToDeleteRows = false;
            this.dgvStockIn.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((
                    System.Windows.Forms.AnchorStyles.Top |
                    System.Windows.Forms.AnchorStyles.Bottom) |
                    System.Windows.Forms.AnchorStyles.Left) |
                    System.Windows.Forms.AnchorStyles.Right));

            this.dgvStockIn.AutoGenerateColumns = false;
            this.dgvStockIn.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.dgvStockIn.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dgvStockIn.Columns.AddRange(
                new System.Windows.Forms.DataGridViewColumn[]
                {
                    this.TransactionID,
                    this.Category,
                    this.Denomination,
                    this.BoxQty,
                    this.PacketQty,
                    this.SheetQty,
                    this.StampQty,
                    this.TransactionDate,
                    this.Remarks
                });

            this.dgvStockIn.Location =
                new System.Drawing.Point(10, 390);

            this.dgvStockIn.MultiSelect = false;
            this.dgvStockIn.Name = "dgvStockIn";
            this.dgvStockIn.ReadOnly = true;
            this.dgvStockIn.RowHeadersVisible = false;
            this.dgvStockIn.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.dgvStockIn.Size =
                new System.Drawing.Size(950, 260);

            this.dgvStockIn.TabIndex = 9;

            // 
            // TransactionID
            // 
            this.TransactionID.DataPropertyName = "TransactionID";
            this.TransactionID.HeaderText = "Transaction ID";
            this.TransactionID.Name = "TransactionID";
            this.TransactionID.Visible = false;

            // 
            // Category
            // 
            this.Category.DataPropertyName = "Category";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";

            // 
            // Denomination
            // 
            this.Denomination.DataPropertyName = "Denomination";
            this.Denomination.HeaderText = "Denomination";
            this.Denomination.Name = "Denomination";

            // 
            // BoxQty
            // 
            this.BoxQty.DataPropertyName = "BoxQty";
            this.BoxQty.HeaderText = "Box";
            this.BoxQty.Name = "BoxQty";

            // 
            // PacketQty
            // 
            this.PacketQty.DataPropertyName = "PacketQty";
            this.PacketQty.HeaderText = "Packet";
            this.PacketQty.Name = "PacketQty";

            // 
            // SheetQty
            // 
            this.SheetQty.DataPropertyName = "SheetQty";
            this.SheetQty.HeaderText = "Sheet";
            this.SheetQty.Name = "SheetQty";

            // 
            // StampQty
            // 
            this.StampQty.DataPropertyName = "StampQty";
            this.StampQty.HeaderText = "Stamp";
            this.StampQty.Name = "StampQty";

            // 
            // TransactionDate
            // 
            this.TransactionDate.DataPropertyName = "TransactionDate";
            this.TransactionDate.HeaderText = "Date";
            this.TransactionDate.Name = "TransactionDate";

            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";

            // 
            // frmStockIn
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(96F, 96F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Dpi;

            this.ClientSize =
                new System.Drawing.Size(970, 670);

            this.Controls.Add(this.dgvStockIn);

            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);

            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.lblRemarks);

            this.Controls.Add(this.txtStampQty);
            this.Controls.Add(this.lblStampQty);

            this.Controls.Add(this.txtSheetQty);
            this.Controls.Add(this.lblSheetQty);

            this.Controls.Add(this.txtPacketQty);
            this.Controls.Add(this.lblPacketQty);

            this.Controls.Add(this.txtBoxQty);
            this.Controls.Add(this.lblBoxQty);

            this.Controls.Add(this.cmbDenomination);
            this.Controls.Add(this.lblDenomination);

            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);

            this.Name = "frmStockIn";
            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text = "Stock In";

            this.Load +=
                new System.EventHandler(this.frmStockIn_Load);

            ((System.ComponentModel.ISupportInitialize)
                (this.dgvStockIn)).EndInit();

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ComboBox cmbDenomination;

        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblDenomination;

        private System.Windows.Forms.TextBox txtBoxQty;
        private System.Windows.Forms.TextBox txtPacketQty;
        private System.Windows.Forms.TextBox txtSheetQty;
        private System.Windows.Forms.TextBox txtStampQty;
        private System.Windows.Forms.TextBox txtRemarks;

        private System.Windows.Forms.Label lblBoxQty;
        private System.Windows.Forms.Label lblPacketQty;
        private System.Windows.Forms.Label lblSheetQty;
        private System.Windows.Forms.Label lblStampQty;
        private System.Windows.Forms.Label lblRemarks;

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;

        private System.Windows.Forms.DataGridView dgvStockIn;

        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Denomination;
        private System.Windows.Forms.DataGridViewTextBoxColumn BoxQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PacketQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn StampQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
    }
}