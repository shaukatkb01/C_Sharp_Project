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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockBalance));
            this.dgvStockBalance = new System.Windows.Forms.DataGridView();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDenom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPacket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.btnPrint = new System.Windows.Forms.Button();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBalance)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvStockBalance
            // 
            this.dgvStockBalance.AllowUserToAddRows = false;
            this.dgvStockBalance.AllowUserToDeleteRows = false;
            this.dgvStockBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStockBalance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStockBalance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockBalance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategory,
            this.colDenom,
            this.colBox,
            this.colPacket,
            this.colSheet,
            this.colStamp});
            this.dgvStockBalance.Location = new System.Drawing.Point(10, 65);
            this.dgvStockBalance.MultiSelect = false;
            this.dgvStockBalance.Name = "dgvStockBalance";
            this.dgvStockBalance.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockBalance.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStockBalance.RowHeadersVisible = false;
            this.dgvStockBalance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockBalance.Size = new System.Drawing.Size(920, 480);
            this.dgvStockBalance.TabIndex = 2;
            // 
            // colCategory
            // 
            this.colCategory.DataPropertyName = "Category";
            this.colCategory.HeaderText = "Stamp Category";
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colDenom
            // 
            this.colDenom.DataPropertyName = "Denomination";
            this.colDenom.HeaderText = "Denomination";
            this.colDenom.Name = "colDenom";
            this.colDenom.ReadOnly = true;
            // 
            // colBox
            // 
            this.colBox.DataPropertyName = "BoxQty";
            this.colBox.HeaderText = "BoxQty";
            this.colBox.Name = "colBox";
            this.colBox.ReadOnly = true;
            // 
            // colPacket
            // 
            this.colPacket.DataPropertyName = "PacketQty";
            this.colPacket.HeaderText = "PacketQty";
            this.colPacket.Name = "colPacket";
            this.colPacket.ReadOnly = true;
            // 
            // colSheet
            // 
            this.colSheet.DataPropertyName = "SheetQty";
            this.colSheet.HeaderText = "SheetQty";
            this.colSheet.Name = "colSheet";
            this.colSheet.ReadOnly = true;
            // 
            // colStamp
            // 
            this.colStamp.DataPropertyName = "StampQty";
            this.colStamp.HeaderText = "StampQty";
            this.colStamp.Name = "colStamp";
            this.colStamp.ReadOnly = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(770, 22);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(101, 37);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage_1);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(651, 22);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(101, 37);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // frmStockBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(940, 560);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dgvStockBalance);
            this.Controls.Add(this.btnRefresh);
            this.Name = "frmStockBalance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Balance";
            this.Load += new System.EventHandler(this.frmStockBalance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBalance)).EndInit();
            this.ResumeLayout(false);

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
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDenom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacket;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStamp;
    }
}