namespace SupplyBranch.Forms.Stock
{
    partial class frmStockBalance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 =
                new System.Windows.Forms.DataGridViewCellStyle();

            this.dgvStockBalance = new System.Windows.Forms.DataGridView();

            this.StockID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Denomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BoxQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PacketQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SheetQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StampQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModifiedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.btnRefresh = new System.Windows.Forms.Button();

            this.lblTitle = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)
                (this.dgvStockBalance)).BeginInit();

            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font =
                new System.Drawing.Font(
                    "Microsoft Sans Serif",
                    12F,
                    System.Drawing.FontStyle.Bold,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));

            this.lblTitle.Location =
                new System.Drawing.Point(20, 20);

            this.lblTitle.Name = "lblTitle";

            this.lblTitle.Size =
                new System.Drawing.Size(130, 20);

            this.lblTitle.TabIndex = 0;

            this.lblTitle.Text = "Stock Balance";

            // 
            // btnRefresh
            // 
            this.btnRefresh.Location =
                new System.Drawing.Point(770, 15);

            this.btnRefresh.Name = "btnRefresh";

            this.btnRefresh.Size =
                new System.Drawing.Size(90, 30);

            this.btnRefresh.TabIndex = 1;

            this.btnRefresh.Text = "Refresh";

            this.btnRefresh.UseVisualStyleBackColor = true;

            this.btnRefresh.Click +=
                new System.EventHandler(
                    this.btnRefresh_Click);

            // 
            // dgvStockBalance
            // 
            this.dgvStockBalance.AllowUserToAddRows = false;

            this.dgvStockBalance.AllowUserToDeleteRows = false;

            this.dgvStockBalance.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                ((((System.Windows.Forms.AnchorStyles.Top |
                    System.Windows.Forms.AnchorStyles.Bottom) |
                    System.Windows.Forms.AnchorStyles.Left) |
                    System.Windows.Forms.AnchorStyles.Right)));

            this.dgvStockBalance.AutoGenerateColumns = false;

            this.dgvStockBalance.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.dgvStockBalance.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dgvStockBalance.Columns.AddRange(
                new System.Windows.Forms.DataGridViewColumn[]
                {
                    this.StockID,
                    this.Category,
                    this.Denomination,
                    this.BoxQty,
                    this.PacketQty,
                    this.SheetQty,
                    this.StampQty,
                    this.ModifiedDate
                });

            this.dgvStockBalance.Location =
                new System.Drawing.Point(10, 65);

            this.dgvStockBalance.MultiSelect = false;

            this.dgvStockBalance.Name =
                "dgvStockBalance";

            this.dgvStockBalance.ReadOnly = true;

            dataGridViewCellStyle1.Alignment =
                System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            dataGridViewCellStyle1.WrapMode =
                System.Windows.Forms.DataGridViewTriState.True;

            this.dgvStockBalance.RowHeadersDefaultCellStyle =
                dataGridViewCellStyle1;

            this.dgvStockBalance.RowHeadersVisible = false;

            this.dgvStockBalance.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.dgvStockBalance.Size =
                new System.Drawing.Size(920, 480);

            this.dgvStockBalance.TabIndex = 2;

            // 
            // StockID
            // 
            this.StockID.DataPropertyName =
                "StockID";

            this.StockID.HeaderText =
                "StockID";

            this.StockID.Name =
                "StockID";

            this.StockID.ReadOnly = true;

            this.StockID.Visible = false;

            // 
            // Category
            // 
            this.Category.DataPropertyName =
                "Category";

            this.Category.HeaderText =
                "Category";

            this.Category.Name =
                "Category";

            this.Category.ReadOnly = true;

            // 
            // Denomination
            // 
            this.Denomination.DataPropertyName =
                "Denomination";

            this.Denomination.HeaderText =
                "Denomination";

            this.Denomination.Name =
                "Denomination";

            this.Denomination.ReadOnly = true;

            // 
            // BoxQty
            // 
            this.BoxQty.DataPropertyName =
                "BoxQty";

            this.BoxQty.HeaderText =
                "Box";

            this.BoxQty.Name =
                "BoxQty";

            this.BoxQty.ReadOnly = true;

            // 
            // PacketQty
            // 
            this.PacketQty.DataPropertyName =
                "PacketQty";

            this.PacketQty.HeaderText =
                "Packet";

            this.PacketQty.Name =
                "PacketQty";

            this.PacketQty.ReadOnly = true;

            // 
            // SheetQty
            // 
            this.SheetQty.DataPropertyName =
                "SheetQty";

            this.SheetQty.HeaderText =
                "Sheet";

            this.SheetQty.Name =
                "SheetQty";

            this.SheetQty.ReadOnly = true;

            // 
            // StampQty
            // 
            this.StampQty.DataPropertyName =
                "StampQty";

            this.StampQty.HeaderText =
                "Stamp";

            this.StampQty.Name =
                "StampQty";

            this.StampQty.ReadOnly = true;

            // 
            // ModifiedDate
            // 
            this.ModifiedDate.DataPropertyName =
                "ModifiedDate";

            this.ModifiedDate.HeaderText =
                "Last Updated";

            this.ModifiedDate.Name =
                "ModifiedDate";

            this.ModifiedDate.ReadOnly = true;

            // 
            // frmStockBalance
            // 
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(96F, 96F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Dpi;

            this.ClientSize =
                new System.Drawing.Size(940, 560);

            this.Controls.Add(
                this.dgvStockBalance);

            this.Controls.Add(
                this.btnRefresh);

            this.Controls.Add(
                this.lblTitle);

            this.Name =
                "frmStockBalance";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text =
                "Stock Balance";

            this.Load +=
                new System.EventHandler(
                    this.frmStockBalance_Load);

            ((System.ComponentModel.ISupportInitialize)
                (this.dgvStockBalance)).EndInit();

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStockBalance;

        private System.Windows.Forms.DataGridViewTextBoxColumn StockID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Denomination;
        private System.Windows.Forms.DataGridViewTextBoxColumn BoxQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PacketQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn StampQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModifiedDate;

        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Label lblTitle;
    }
}