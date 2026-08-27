namespace StampStoreApp
{
    partial class frmStockAdjustment
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbAdjustType = new System.Windows.Forms.ComboBox();
            this.cmbDenomination = new System.Windows.Forms.ComboBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.txtStampQty = new System.Windows.Forms.TextBox();
            this.txtPacketQty = new System.Windows.Forms.TextBox();
            this.txtSheetQty = new System.Windows.Forms.TextBox();
            this.txtBoxQty = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbAdjustType);
            this.groupBox1.Controls.Add(this.cmbDenomination);
            this.groupBox1.Controls.Add(this.cmbCategory);
            this.groupBox1.Location = new System.Drawing.Point(36, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 175);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Category Detail";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Adjust Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Denomination";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Category";
            // 
            // cmbAdjustType
            // 
            this.cmbAdjustType.FormattingEnabled = true;
            this.cmbAdjustType.Items.AddRange(new object[] {
            "ADD",
            "LESS"});
            this.cmbAdjustType.Location = new System.Drawing.Point(93, 101);
            this.cmbAdjustType.Name = "cmbAdjustType";
            this.cmbAdjustType.Size = new System.Drawing.Size(214, 21);
            this.cmbAdjustType.TabIndex = 2;
            // 
            // cmbDenomination
            // 
            this.cmbDenomination.FormattingEnabled = true;
            this.cmbDenomination.Location = new System.Drawing.Point(93, 66);
            this.cmbDenomination.Name = "cmbDenomination";
            this.cmbDenomination.Size = new System.Drawing.Size(214, 21);
            this.cmbDenomination.TabIndex = 1;
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(93, 31);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(214, 21);
            this.cmbCategory.TabIndex = 0;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtReason);
            this.groupBox2.Controls.Add(this.txtStampQty);
            this.groupBox2.Controls.Add(this.txtPacketQty);
            this.groupBox2.Controls.Add(this.txtSheetQty);
            this.groupBox2.Controls.Add(this.txtBoxQty);
            this.groupBox2.Location = new System.Drawing.Point(380, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(369, 175);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Addjust Detail";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(180, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Label";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(180, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Packet";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Reason";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sheet";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Box";
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(65, 112);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(261, 20);
            this.txtReason.TabIndex = 4;
            this.txtReason.Text = "Reason";
            // 
            // txtStampQty
            // 
            this.txtStampQty.Location = new System.Drawing.Point(230, 75);
            this.txtStampQty.Name = "txtStampQty";
            this.txtStampQty.Size = new System.Drawing.Size(96, 20);
            this.txtStampQty.TabIndex = 3;
            this.txtStampQty.Text = "StampQty";
            this.txtStampQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStampQty_KeyPress);
            // 
            // txtPacketQty
            // 
            this.txtPacketQty.Location = new System.Drawing.Point(230, 39);
            this.txtPacketQty.Name = "txtPacketQty";
            this.txtPacketQty.Size = new System.Drawing.Size(96, 20);
            this.txtPacketQty.TabIndex = 2;
            this.txtPacketQty.Text = "PacketQty";
            this.txtPacketQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPacketQty_KeyPress);
            // 
            // txtSheetQty
            // 
            this.txtSheetQty.Location = new System.Drawing.Point(65, 75);
            this.txtSheetQty.Name = "txtSheetQty";
            this.txtSheetQty.Size = new System.Drawing.Size(96, 20);
            this.txtSheetQty.TabIndex = 1;
            this.txtSheetQty.Text = "SheetQty";
            this.txtSheetQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSheetQty_KeyPress);
            // 
            // txtBoxQty
            // 
            this.txtBoxQty.Location = new System.Drawing.Point(65, 39);
            this.txtBoxQty.Name = "txtBoxQty";
            this.txtBoxQty.Size = new System.Drawing.Size(96, 20);
            this.txtBoxQty.TabIndex = 0;
            this.txtBoxQty.Text = "BoxQty";
            this.txtBoxQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxQty_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(283, 241);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 42);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save Adjustment";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(398, 241);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(91, 42);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear / Reset";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dgvHistory
            // 
            this.dgvHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(3, 289);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.Size = new System.Drawing.Size(781, 220);
            this.dgvHistory.TabIndex = 5;
            // 
            // frmStockAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(784, 510);
            this.Controls.Add(this.dgvHistory);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmStockAdjustment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stock Adjustment - Central Stamp Store";
            this.Load += new System.EventHandler(this.frmStockAdjustment_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbAdjustType;
        private System.Windows.Forms.ComboBox cmbDenomination;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.TextBox txtStampQty;
        private System.Windows.Forms.TextBox txtPacketQty;
        private System.Windows.Forms.TextBox txtSheetQty;
        private System.Windows.Forms.TextBox txtBoxQty;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}