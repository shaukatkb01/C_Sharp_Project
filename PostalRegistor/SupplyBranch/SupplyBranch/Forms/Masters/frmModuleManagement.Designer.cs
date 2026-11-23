namespace SupplyBranch.Forms.Masters
{
    partial class frmModuleManagement
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.numSortOrder = new System.Windows.Forms.NumericUpDown();
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.txtModuleName = new System.Windows.Forms.TextBox();
            this.lblModuleName = new System.Windows.Forms.Label();
            this.txtModuleCode = new System.Windows.Forms.TextBox();
            this.lblModuleCode = new System.Windows.Forms.Label();
            this.btnToggleModule = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSaveModule = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvModules = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSortOrder)).BeginInit();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModules)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.chkIsActive);
            this.pnlTop.Controls.Add(this.numSortOrder);
            this.pnlTop.Controls.Add(this.lblSortOrder);
            this.pnlTop.Controls.Add(this.txtModuleName);
            this.pnlTop.Controls.Add(this.lblModuleName);
            this.pnlTop.Controls.Add(this.txtModuleCode);
            this.pnlTop.Controls.Add(this.lblModuleCode);
            this.pnlTop.Controls.Add(this.btnToggleModule);
            this.pnlTop.Controls.Add(this.btnClear);
            this.pnlTop.Controls.Add(this.btnSaveModule);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(834, 130);
            this.pnlTop.TabIndex = 0;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(595, 39);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(56, 17);
            this.chkIsActive.TabIndex = 6;
            this.chkIsActive.Text = "Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // numSortOrder
            // 
            this.numSortOrder.Location = new System.Drawing.Point(480, 36);
            this.numSortOrder.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numSortOrder.Name = "numSortOrder";
            this.numSortOrder.Size = new System.Drawing.Size(90, 20);
            this.numSortOrder.TabIndex = 5;
            this.numSortOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(480, 18);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(58, 13);
            this.lblSortOrder.TabIndex = 4;
            this.lblSortOrder.Text = "Sort Order:";
            // 
            // txtModuleName
            // 
            this.txtModuleName.Location = new System.Drawing.Point(210, 36);
            this.txtModuleName.MaxLength = 100;
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(250, 20);
            this.txtModuleName.TabIndex = 3;
            // 
            // lblModuleName
            // 
            this.lblModuleName.AutoSize = true;
            this.lblModuleName.Location = new System.Drawing.Point(210, 18);
            this.lblModuleName.Name = "lblModuleName";
            this.lblModuleName.Size = new System.Drawing.Size(76, 13);
            this.lblModuleName.TabIndex = 2;
            this.lblModuleName.Text = "Module Name:";
            // 
            // txtModuleCode
            // 
            this.txtModuleCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtModuleCode.Location = new System.Drawing.Point(20, 36);
            this.txtModuleCode.MaxLength = 50;
            this.txtModuleCode.Name = "txtModuleCode";
            this.txtModuleCode.Size = new System.Drawing.Size(170, 20);
            this.txtModuleCode.TabIndex = 1;
            // 
            // lblModuleCode
            // 
            this.lblModuleCode.AutoSize = true;
            this.lblModuleCode.Location = new System.Drawing.Point(20, 18);
            this.lblModuleCode.Name = "lblModuleCode";
            this.lblModuleCode.Size = new System.Drawing.Size(73, 13);
            this.lblModuleCode.TabIndex = 0;
            this.lblModuleCode.Text = "Module Code:";
            // 
            // btnToggleModule
            // 
            this.btnToggleModule.Location = new System.Drawing.Point(304, 78);
            this.btnToggleModule.Name = "btnToggleModule";
            this.btnToggleModule.Size = new System.Drawing.Size(150, 32);
            this.btnToggleModule.TabIndex = 9;
            this.btnToggleModule.Text = "Disable Module";
            this.btnToggleModule.UseVisualStyleBackColor = true;
            this.btnToggleModule.Click += new System.EventHandler(this.btnToggleModule_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(172, 78);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 32);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSaveModule
            // 
            this.btnSaveModule.Location = new System.Drawing.Point(20, 78);
            this.btnSaveModule.Name = "btnSaveModule";
            this.btnSaveModule.Size = new System.Drawing.Size(140, 32);
            this.btnSaveModule.TabIndex = 7;
            this.btnSaveModule.Text = "Save Module";
            this.btnSaveModule.UseVisualStyleBackColor = true;
            this.btnSaveModule.Click += new System.EventHandler(this.btnSaveModule_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dgvModules);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 130);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(8);
            this.pnlGrid.Size = new System.Drawing.Size(834, 380);
            this.pnlGrid.TabIndex = 1;
            // 
            // dgvModules
            // 
            this.dgvModules.AllowUserToAddRows = false;
            this.dgvModules.AllowUserToDeleteRows = false;
            this.dgvModules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvModules.Location = new System.Drawing.Point(8, 8);
            this.dgvModules.MultiSelect = false;
            this.dgvModules.Name = "dgvModules";
            this.dgvModules.ReadOnly = true;
            this.dgvModules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvModules.Size = new System.Drawing.Size(818, 364);
            this.dgvModules.TabIndex = 0;
            this.dgvModules.SelectionChanged += new System.EventHandler(this.dgvModules_SelectionChanged);
            // 
            // frmModuleManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(834, 510);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlTop);
            this.Name = "frmModuleManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Module Management";
            this.Load += new System.EventHandler(this.frmModuleManagement_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSortOrder)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvModules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.NumericUpDown numSortOrder;
        private System.Windows.Forms.Label lblSortOrder;
        private System.Windows.Forms.TextBox txtModuleName;
        private System.Windows.Forms.Label lblModuleName;
        private System.Windows.Forms.TextBox txtModuleCode;
        private System.Windows.Forms.Label lblModuleCode;
        private System.Windows.Forms.Button btnToggleModule;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSaveModule;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvModules;
    }
}