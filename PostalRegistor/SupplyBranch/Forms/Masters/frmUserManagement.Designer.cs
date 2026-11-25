namespace SupplyBranch.Forms.Masters
{
    partial class frmUserManagement
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
            this.btnToggleUser = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.pnlModules = new System.Windows.Forms.Panel();
            this.clbModules = new SupplyBranch.Controls.ModernCheckedListBox();
            this.lblModulesTitle = new System.Windows.Forms.Label();
            this.btnSavePermissions = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.pnlModules.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnToggleUser);
            this.panel2.Controls.Add(this.btnChangePassword);
            this.panel2.Controls.Add(this.btnEditUser);
            this.panel2.Controls.Add(this.btnAddUser);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(980, 100);
            this.panel2.TabIndex = 1;
            // 
            // btnToggleUser
            // 
            this.btnToggleUser.Location = new System.Drawing.Point(537, 54);
            this.btnToggleUser.Name = "btnToggleUser";
            this.btnToggleUser.Size = new System.Drawing.Size(156, 31);
            this.btnToggleUser.TabIndex = 3;
            this.btnToggleUser.Text = "Disable User";
            this.btnToggleUser.UseVisualStyleBackColor = true;
            this.btnToggleUser.Click += new System.EventHandler(this.btnToggleUser_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.Location = new System.Drawing.Point(365, 54);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(156, 31);
            this.btnChangePassword.TabIndex = 2;
            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // btnEditUser
            // 
            this.btnEditUser.Location = new System.Drawing.Point(193, 54);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(156, 31);
            this.btnEditUser.TabIndex = 1;
            this.btnEditUser.Text = "Edit User";
            this.btnEditUser.UseVisualStyleBackColor = true;
            this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(21, 54);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(156, 31);
            this.btnAddUser.TabIndex = 0;
            this.btnAddUser.Text = "New User";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvUsers);
            this.panel3.Controls.Add(this.pnlModules);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 100);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(6);
            this.panel3.Size = new System.Drawing.Size(980, 410);
            this.panel3.TabIndex = 2;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(6, 6);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(680, 398);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.SelectionChanged += new System.EventHandler(this.dgvUsers_SelectionChanged);
            // 
            // pnlModules
            // 
            this.pnlModules.Controls.Add(this.clbModules);
            this.pnlModules.Controls.Add(this.lblModulesTitle);
            this.pnlModules.Controls.Add(this.btnSavePermissions);
            this.pnlModules.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlModules.Location = new System.Drawing.Point(686, 6);
            this.pnlModules.Name = "pnlModules";
            this.pnlModules.Padding = new System.Windows.Forms.Padding(8, 4, 0, 0);
            this.pnlModules.Size = new System.Drawing.Size(288, 398);
            this.pnlModules.TabIndex = 1;
            // 
            // clbModules
            // 
            this.clbModules.CheckOnClick = true;
            this.clbModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbModules.Location = new System.Drawing.Point(8, 25);
            this.clbModules.Name = "clbModules";
            this.clbModules.Size = new System.Drawing.Size(280, 331);
            this.clbModules.TabIndex = 1;
            // 
            // lblModulesTitle
            // 
            this.lblModulesTitle.AutoSize = true;
            this.lblModulesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblModulesTitle.Location = new System.Drawing.Point(8, 4);
            this.lblModulesTitle.Name = "lblModulesTitle";
            this.lblModulesTitle.Padding = new System.Windows.Forms.Padding(0, 2, 0, 6);
            this.lblModulesTitle.Size = new System.Drawing.Size(103, 21);
            this.lblModulesTitle.TabIndex = 0;
            this.lblModulesTitle.Text = "Module Permissions:";
            // 
            // btnSavePermissions
            // 
            this.btnSavePermissions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSavePermissions.Location = new System.Drawing.Point(8, 356);
            this.btnSavePermissions.Name = "btnSavePermissions";
            this.btnSavePermissions.Size = new System.Drawing.Size(280, 42);
            this.btnSavePermissions.TabIndex = 2;
            this.btnSavePermissions.Text = "Save Permissions";
            this.btnSavePermissions.UseVisualStyleBackColor = true;
            this.btnSavePermissions.Click += new System.EventHandler(this.btnSavePermissions_Click_1);
            // 
            // frmUserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(980, 510);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "frmUserManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.frmUserManagement_Load);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.pnlModules.ResumeLayout(false);
            this.pnlModules.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnToggleUser;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Panel pnlModules;
        private System.Windows.Forms.Label lblModulesTitle;
        private System.Windows.Forms.CheckedListBox clbModules;
        private System.Windows.Forms.Button btnSavePermissions;
    }
}