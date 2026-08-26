using StampStoreApp;
using SupplyBranch.Classes;
using SupplyBranch.DataAccess;
using SupplyBranch.Forms.Administration;
using SupplyBranch.Forms.Help;
using SupplyBranch.Forms.Masters;
using SupplyBranch.Forms.Reports;
using SupplyBranch.Forms.Stock;
using SupplyBranch.Forms.Transactions;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
   

    public partial class frmMain : Form
    {
        private Form activeForm = null;

       

        private async Task<string> GetLatestVersionAsync()
        {
            string url = "https://raw.githubusercontent.com/shaukatkb01/C_Sharp_Project/SupplyBranch-Only/PostalRegistor/SupplyBranch/UpdateInfo.txt";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    string content = await client.GetStringAsync(url);

                    if (string.IsNullOrWhiteSpace(content))
                        return string.Empty;

                    // GitHub text file mein 2 lines hongi:
                    // Line 1: Version Number (e.g., 1.0.0.1)
                    // Line 2: ZIP Download Link
                    string[] lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length > 0)
                    {
                        string latestVersion = lines[0].Trim();
                        AppVersionInfo.AvailableVersion = latestVersion;

                        if (lines.Length > 1)
                        {
                            AppVersionInfo.DownloadUrl = lines[1].Trim();
                        }

                        return latestVersion;
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent handling: Internet na hone par startup delay/error popup na aaye
                Debug.WriteLine("UPDATE CHECK ERROR: " + ex.Message);
            }

            return string.Empty;
        }

        private void Logout()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;


            // ========================================
            // Take Database Backup
            // ========================================

            if (!DatabaseBackup.BackupDatabase())
            {
                MessageBox.Show(
                    "Logout cancelled because database backup could not be completed.",
                    "Logout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // ========================================
            // Clear Current User
            // ========================================

            CurrentUser.UserID = 0;
            CurrentUser.UserName = null;
            CurrentUser.FullName = null;


            // ========================================
            // Close Forms inside pnlMain
            // ========================================

            foreach (Control control in pnlMain.Controls)
            {
                if (control is Form childForm)
                {
                    childForm.Close();
                }
            }

            pnlMain.Controls.Clear();


            // ========================================
            // Show Login
            // ========================================

            frmLogin login = new frmLogin();

            login.Show();

            this.Hide();
        }

        private void OpenForm(Form childForm)
        {
            // ==========================================
            // 1. Check whether same form is already open
            // ==========================================
            Form existingForm = pnlMain.Controls
                .OfType<Form>()
                .FirstOrDefault(f => f.GetType() == childForm.GetType());

            if (existingForm != null)
            {
                existingForm.Close();
                existingForm.Dispose();
                pnlMain.Controls.Remove(existingForm);
            }

            // ==========================================
            // 2. Open new form
            // ==========================================
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.Margin = new Padding(0);

            pnlMain.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();

            // ==========================================
            // 3. Dynamic SubTitle in lblMain (Next Line)
            // ==========================================
            string mainTitle = "Supply Branch Management System "; // Line 1 Text
            string subTitleText = "";

            // Child form ke andar "lblSubTitle" ko dhoondein
            Control[] foundControls = childForm.Controls.Find("lblSubTitle", true);

            if (foundControls.Length > 0 && foundControls[0] is Label lblSub)
            {
                subTitleText = lblSub.Text;
                
            }
            else if (!string.IsNullOrWhiteSpace(childForm.Text))
            {
                // Agar lblSubTitle na mile toh form ki Text property istemal karein
                subTitleText = childForm.Text;
            }

            // Single Line / Double Line Set Karein
            if (!string.IsNullOrWhiteSpace(subTitleText))
            {
                lblMain.Text = $"{mainTitle}\n{subTitleText}";
            }
            else
            {
                lblMain.Text = mainTitle;
            }
        }
        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            // F5 = Refresh active form
            if (e.KeyCode == Keys.F5)
            {
                RefreshActiveForm();
                e.Handled = true;
            }

            // Esc = Close active child form
            else if (e.KeyCode == Keys.Escape)
            {
                CloseActiveForm();
                e.Handled = true;
            }
        }
        private void CloseActiveForm()
        {
            Form activeForm = pnlMain.Controls
                .OfType<Form>()
                .FirstOrDefault(f => f.Visible);

            if (activeForm != null)
            {
                activeForm.Close();
            }
        }
        private void RefreshActiveForm()
        {
            Form activeForm = pnlMain.Controls
                .OfType<Form>()
                .FirstOrDefault(f => f.Visible);

            if (activeForm is IRefreshable refreshable)
            {
                refreshable.RefreshData();
            }
        }
        public interface IRefreshable
        {
            void RefreshData();
        }
        public frmMain()
        {
            InitializeComponent();





            this.KeyPreview = true;
            this.KeyDown += frmMain_KeyDown;
        }

        private void btnPostal_Click(object sender, EventArgs e)
        {
            frmPostal_Stationery frmPostal = new frmPostal_Stationery();
            frmPostal.ShowDialog();
        }

        public async void frmMain_Load(object sender, EventArgs e)
        {
     
            UITheme.Apply(this);
            //DatabaseMigrator.ApplyMigrations();
            // Form ka default Title
            this.Text = "SupplyBranch Version " + AppVersion.CurrentVersion;

            // Header Label Formatting
            lblMain.BackColor = Color.FromArgb(232, 240, 248);
            lblMain.ForeColor = Color.FromArgb(31, 78, 121);
            lblMain.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblMain.TextAlign = ContentAlignment.MiddleCenter;
            lblMain.AutoSize = false;
            lblMain.Dock = DockStyle.Top;
            lblMain.Height = 100;

            // Status Strip Data
            tsUser.Text = "User: " + CurrentUser.UserName;
            tsDate.Text = "Date: " + DateTime.Now.ToString("dd-MMM-yyyy");
            tsTime.Text = "Time: " + DateTime.Now.ToString("hh:mm:ss tt");

            tsUser.Padding = new Padding(0, 0, 25, 0);
            tsDate.Padding = new Padding(0, 0, 25, 0);
            tsTime.Padding = new Padding(0, 0, 10, 0);

            // Dashboard Open Karein
            OpenForm(new frmDashBoard());

            // Admin Menus Control
            bool isAdmin = string.Equals(CurrentUser.UserName, "admin", StringComparison.OrdinalIgnoreCase);
            mnuRestoreDatabase.Visible = isAdmin;
            mnuRestoreSafetyBackup.Visible = isAdmin;
            mnuUserManagement.Visible = isAdmin;
        }



        private void officeZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmOfficeZone());
        }

        private void officeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmOffice());
        }

        private void stampCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStampCategory());
        }

        private void denominationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmDenomination());
        }

        

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseBackup.BackupDatabase();
            Application.Exit();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmUnitConversionMaster());
        }

        private void indentCorrectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmIndentSearch());
        }

        private void indentEntryerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmIndent());

        }

        private void searchIndentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmSupplySearch());    
        }

        private void draftSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmDraftSupply());
        }

        private void officeWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            OpenForm(new frmReport());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

      

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenForm(new frmChangePassword());
        }

        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseBackup.ChangeBackupLocation();
        }

        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmDashBoard());
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAbout about = new frmAbout())
            {
                about.ShowDialog(this);
            }
        }

        private void userGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmUserGuide guide = new frmUserGuide())
            {
                guide.ShowDialog(this);
            }
        }

        private void keyboardShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmKeyboardShortcuts shortcuts =
          new frmKeyboardShortcuts())
            {
                shortcuts.ShowDialog(this);
            }
        }

        private void mnuRestoreDatabase_Click(object sender, EventArgs e)
        {
            if (!DatabaseBackup.RestoreDatabase())
                return;

            Application.Restart();
        }

        private void mnuRestoreSafetyBackup_Click(object sender, EventArgs e)
        {
            if (!DatabaseBackup.RestoreSafetyBackup())
                return;

            Application.Restart();
        }

        private void mnuUserManagement_Click(object sender, EventArgs e)
        {
            if (!string.Equals(
            CurrentUser.UserName,
            "admin",
            StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Only Admin user can access User Management.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using (frmUserManagement form = new frmUserManagement())
            {
                //form.ShowDialog(this);
                OpenForm( new frmUserManagement());
            }
        }

        private async void frmMain_Shown(object sender, EventArgs e)
        {
            // Form open hone ke baad update check start hoga
            await CheckForUpdatesAsync();
        }
       

        private async Task CheckForUpdatesAsync()
        {
            this.Text = "SupplyBranch - Checking Update...";

            try
            {
                string currentVersion = AppVersion.CurrentVersion;
                AppVersionInfo.CurrentVersion = currentVersion;

                string latestVersion = await GetLatestVersionAsync();
                AppVersionInfo.AvailableVersion = latestVersion;

                if (!string.IsNullOrWhiteSpace(latestVersion))
                {
                    if (Version.TryParse(AppVersionInfo.CurrentVersion, out Version current) &&
                        Version.TryParse(latestVersion, out Version latest))
                    {
                        if (latest > current)
                        {
                            DialogResult result = MessageBox.Show(
                                $"A new version of SupplyBranch is available!\n\n" +
                                $"Current Version: {AppVersionInfo.CurrentVersion}\n" +
                                $"Latest Version: {latestVersion}\n\n" +
                                $"Would you like to download and install the update now?",
                                "SupplyBranch Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (result == DialogResult.Yes)
                            {
                                if (string.IsNullOrEmpty(AppVersionInfo.DownloadUrl))
                                {
                                    MessageBox.Show("Download URL missing. Please check UpdateInfo.txt on server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                string tempZipPath = Path.Combine(Path.GetTempPath(), "SupplyBranch_Update.zip");

                                using (var downloadForm = new frmUpdateDownload(AppVersionInfo.DownloadUrl, tempZipPath))
                                {
                                    if (downloadForm.ShowDialog() == DialogResult.OK)
                                    {
                                        // Batch script trigger hoga aur app close ho jayegi
                                        UpdateDAL.ApplyUpdateAndRestart(tempZipPath);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("VERSION CHECK ERROR: " + ex.ToString());
            }
            finally
            {
                // Exception aaye ya na aaye, Title reset ho jayega
                this.Text = "SupplyBranch Version " + AppVersionInfo.CurrentVersion;
            }
        }

        private void enterStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStockIn());
        }

        private void stockBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStockBalance());
        }

        private void stockAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStockAdjustment());
        }
    }
}
