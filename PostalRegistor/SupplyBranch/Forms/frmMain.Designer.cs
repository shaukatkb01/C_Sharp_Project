namespace SupplyBranch.Forms
{
    partial class frmMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mastersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officeZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stampCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.denominationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userRolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUserManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indentEntryerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indentCorrectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supplyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchIndentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.draftSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officeWiseReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stockReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRestoreDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.taskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMain = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKeyboardShortcuts = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRestoreSafetyBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mastersToolStripMenuItem,
            this.transactionsToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.administrationToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 29);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 25);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mastersToolStripMenuItem
            // 
            this.mastersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.officeZoneToolStripMenuItem,
            this.officeToolStripMenuItem,
            this.stampCategoryToolStripMenuItem,
            this.denominationToolStripMenuItem,
            this.usersToolStripMenuItem,
            this.statusToolStripMenuItem,
            this.rolesToolStripMenuItem,
            this.userRolesToolStripMenuItem,
            this.mnuUserManagement});
            this.mastersToolStripMenuItem.Name = "mastersToolStripMenuItem";
            this.mastersToolStripMenuItem.Size = new System.Drawing.Size(77, 25);
            this.mastersToolStripMenuItem.Text = "Masters";
            // 
            // officeZoneToolStripMenuItem
            // 
            this.officeZoneToolStripMenuItem.Name = "officeZoneToolStripMenuItem";
            this.officeZoneToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.officeZoneToolStripMenuItem.Text = "Office Zone";
            this.officeZoneToolStripMenuItem.Click += new System.EventHandler(this.officeZoneToolStripMenuItem_Click);
            // 
            // officeToolStripMenuItem
            // 
            this.officeToolStripMenuItem.Name = "officeToolStripMenuItem";
            this.officeToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.officeToolStripMenuItem.Text = "Office";
            this.officeToolStripMenuItem.Click += new System.EventHandler(this.officeToolStripMenuItem_Click);
            // 
            // stampCategoryToolStripMenuItem
            // 
            this.stampCategoryToolStripMenuItem.Name = "stampCategoryToolStripMenuItem";
            this.stampCategoryToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.stampCategoryToolStripMenuItem.Text = "Stamp Category";
            this.stampCategoryToolStripMenuItem.Click += new System.EventHandler(this.stampCategoryToolStripMenuItem_Click);
            // 
            // denominationToolStripMenuItem
            // 
            this.denominationToolStripMenuItem.Name = "denominationToolStripMenuItem";
            this.denominationToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.denominationToolStripMenuItem.Text = "Denomination";
            this.denominationToolStripMenuItem.Click += new System.EventHandler(this.denominationToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.usersToolStripMenuItem.Text = "UnitConversion";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.statusToolStripMenuItem.Text = "Status";
            // 
            // rolesToolStripMenuItem
            // 
            this.rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            this.rolesToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.rolesToolStripMenuItem.Text = "Roles";
            // 
            // userRolesToolStripMenuItem
            // 
            this.userRolesToolStripMenuItem.Name = "userRolesToolStripMenuItem";
            this.userRolesToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.userRolesToolStripMenuItem.Text = "User Roles";
            // 
            // mnuUserManagement
            // 
            this.mnuUserManagement.Name = "mnuUserManagement";
            this.mnuUserManagement.Size = new System.Drawing.Size(208, 26);
            this.mnuUserManagement.Text = "User Management";
            this.mnuUserManagement.Click += new System.EventHandler(this.mnuUserManagement_Click);
            // 
            // transactionsToolStripMenuItem
            // 
            this.transactionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indentToolStripMenuItem,
            this.supplyToolStripMenuItem});
            this.transactionsToolStripMenuItem.Name = "transactionsToolStripMenuItem";
            this.transactionsToolStripMenuItem.Size = new System.Drawing.Size(151, 25);
            this.transactionsToolStripMenuItem.Text = "Supply Operations";
            // 
            // indentToolStripMenuItem
            // 
            this.indentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indentEntryerToolStripMenuItem,
            this.indentCorrectionToolStripMenuItem});
            this.indentToolStripMenuItem.Name = "indentToolStripMenuItem";
            this.indentToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.indentToolStripMenuItem.Text = "Indent";
            // 
            // indentEntryerToolStripMenuItem
            // 
            this.indentEntryerToolStripMenuItem.Name = "indentEntryerToolStripMenuItem";
            this.indentEntryerToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.indentEntryerToolStripMenuItem.Text = "Indent Entryer";
            this.indentEntryerToolStripMenuItem.Click += new System.EventHandler(this.indentEntryerToolStripMenuItem_Click);
            // 
            // indentCorrectionToolStripMenuItem
            // 
            this.indentCorrectionToolStripMenuItem.Name = "indentCorrectionToolStripMenuItem";
            this.indentCorrectionToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.indentCorrectionToolStripMenuItem.Text = "Indent Correction";
            this.indentCorrectionToolStripMenuItem.Click += new System.EventHandler(this.indentCorrectionToolStripMenuItem_Click);
            // 
            // supplyToolStripMenuItem
            // 
            this.supplyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchIndentToolStripMenuItem,
            this.draftSearchToolStripMenuItem});
            this.supplyToolStripMenuItem.Name = "supplyToolStripMenuItem";
            this.supplyToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.supplyToolStripMenuItem.Text = "Supply";
            // 
            // searchIndentToolStripMenuItem
            // 
            this.searchIndentToolStripMenuItem.Name = "searchIndentToolStripMenuItem";
            this.searchIndentToolStripMenuItem.Size = new System.Drawing.Size(175, 26);
            this.searchIndentToolStripMenuItem.Text = "SupplySearch";
            this.searchIndentToolStripMenuItem.Click += new System.EventHandler(this.searchIndentToolStripMenuItem_Click);
            // 
            // draftSearchToolStripMenuItem
            // 
            this.draftSearchToolStripMenuItem.Name = "draftSearchToolStripMenuItem";
            this.draftSearchToolStripMenuItem.Size = new System.Drawing.Size(175, 26);
            this.draftSearchToolStripMenuItem.Text = "DraftSearch";
            this.draftSearchToolStripMenuItem.Click += new System.EventHandler(this.draftSearchToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.officeWiseReportToolStripMenuItem,
            this.stockReportToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(76, 25);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // officeWiseReportToolStripMenuItem
            // 
            this.officeWiseReportToolStripMenuItem.Name = "officeWiseReportToolStripMenuItem";
            this.officeWiseReportToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.officeWiseReportToolStripMenuItem.Text = "Indent and Supply Report";
            this.officeWiseReportToolStripMenuItem.Click += new System.EventHandler(this.officeWiseReportToolStripMenuItem_Click);
            // 
            // stockReportToolStripMenuItem
            // 
            this.stockReportToolStripMenuItem.Name = "stockReportToolStripMenuItem";
            this.stockReportToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.stockReportToolStripMenuItem.Text = "Stock Report";
            // 
            // administrationToolStripMenuItem
            // 
            this.administrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changePasswordToolStripMenuItem,
            this.backupDatabaseToolStripMenuItem,
            this.mnuRestoreDatabase,
            this.mnuRestoreSafetyBackup,
            this.taskToolStripMenuItem});
            this.administrationToolStripMenuItem.Name = "administrationToolStripMenuItem";
            this.administrationToolStripMenuItem.Size = new System.Drawing.Size(125, 25);
            this.administrationToolStripMenuItem.Text = "Administration";
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.changePasswordToolStripMenuItem.Text = "Change Password";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // backupDatabaseToolStripMenuItem
            // 
            this.backupDatabaseToolStripMenuItem.Name = "backupDatabaseToolStripMenuItem";
            this.backupDatabaseToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.backupDatabaseToolStripMenuItem.Text = "Backup Database";
            this.backupDatabaseToolStripMenuItem.Click += new System.EventHandler(this.backupDatabaseToolStripMenuItem_Click);
            // 
            // mnuRestoreDatabase
            // 
            this.mnuRestoreDatabase.Name = "mnuRestoreDatabase";
            this.mnuRestoreDatabase.Size = new System.Drawing.Size(234, 26);
            this.mnuRestoreDatabase.Text = "Restore Database";
            this.mnuRestoreDatabase.Click += new System.EventHandler(this.mnuRestoreDatabase_Click);
            // 
            // taskToolStripMenuItem
            // 
            this.taskToolStripMenuItem.Name = "taskToolStripMenuItem";
            this.taskToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.taskToolStripMenuItem.Text = "Task";
            this.taskToolStripMenuItem.Click += new System.EventHandler(this.taskToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userGuideToolStripMenuItem,
            this.mnuKeyboardShortcuts,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // lblMain
            // 
            this.lblMain.BackColor = System.Drawing.Color.White;
            this.lblMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMain.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(43)))), ((int)(((byte)(60)))));
            this.lblMain.Location = new System.Drawing.Point(0, 29);
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(800, 70);
            this.lblMain.TabIndex = 7;
            this.lblMain.Text = "Supply Branch Management System ";
            this.lblMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsUser,
            this.tsDate,
            this.tsTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 424);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 26);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsUser
            // 
            this.tsUser.Name = "tsUser";
            this.tsUser.Size = new System.Drawing.Size(55, 21);
            this.tsUser.Text = "User :-";
            // 
            // tsDate
            // 
            this.tsDate.Name = "tsDate";
            this.tsDate.Size = new System.Drawing.Size(55, 21);
            this.tsDate.Text = "Date :-";
            // 
            // tsTime
            // 
            this.tsTime.Name = "tsTime";
            this.tsTime.Size = new System.Drawing.Size(57, 21);
            this.tsTime.Text = "Time :-";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.AutoSize = true;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 99);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(800, 325);
            this.pnlMain.TabIndex = 10;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.aboutToolStripMenuItem.Text = "About Supply Branch";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // userGuideToolStripMenuItem
            // 
            this.userGuideToolStripMenuItem.Name = "userGuideToolStripMenuItem";
            this.userGuideToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.userGuideToolStripMenuItem.Text = "User Guide";
            this.userGuideToolStripMenuItem.Click += new System.EventHandler(this.userGuideToolStripMenuItem_Click);
            // 
            // mnuKeyboardShortcuts
            // 
            this.mnuKeyboardShortcuts.Name = "mnuKeyboardShortcuts";
            this.mnuKeyboardShortcuts.Size = new System.Drawing.Size(226, 26);
            this.mnuKeyboardShortcuts.Text = "Keyboard Shortcuts";
            this.mnuKeyboardShortcuts.Click += new System.EventHandler(this.keyboardShortcutsToolStripMenuItem_Click);
            // 
            // mnuRestoreSafetyBackup
            // 
            this.mnuRestoreSafetyBackup.Name = "mnuRestoreSafetyBackup";
            this.mnuRestoreSafetyBackup.Size = new System.Drawing.Size(234, 26);
            this.mnuRestoreSafetyBackup.Text = "Restore Safety Backup";
            this.mnuRestoreSafetyBackup.Click += new System.EventHandler(this.mnuRestoreSafetyBackup_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Supply Branch Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mastersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officeZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stampCategoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem denominationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officeWiseReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuRestoreDatabase;
        private System.Windows.Forms.Label lblMain;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsUser;
        private System.Windows.Forms.ToolStripStatusLabel tsDate;
        private System.Windows.Forms.ToolStripStatusLabel tsTime;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userRolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuUserManagement;
        private System.Windows.Forms.ToolStripMenuItem indentEntryerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indentCorrectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supplyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchIndentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem draftSearchToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolStripMenuItem taskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuKeyboardShortcuts;
        private System.Windows.Forms.ToolStripMenuItem mnuRestoreSafetyBackup;
    }
}