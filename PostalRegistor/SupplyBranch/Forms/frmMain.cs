using SupplyBranch.Forms.Administration;
using SupplyBranch.Forms.Help;
using SupplyBranch.Forms.Masters;
using SupplyBranch.Forms.Reports;
using SupplyBranch.Forms.Transactions;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Http.Headers;

namespace SupplyBranch.Forms
{
   

    public partial class frmMain : Form
    {
        private Form activeForm = null;

        private async Task<string> GetLatestVersionAsync()
        {
            string url = "https://1drv.ms/t/c/b8728c5330ecc526/IQDGcM0odTHjSpAVVpod6cttARqm969loQnefBK-QitHGgE?e=BqnQN8";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(15);

                    HttpResponseMessage response = await client.GetAsync(
                        url,
                        HttpCompletionOption.ResponseHeadersRead);

                    response.EnsureSuccessStatusCode();

                    byte[] data = await response.Content.ReadAsByteArrayAsync();

                    string content = Encoding.UTF8.GetString(data);
                    MessageBox.Show(
    "RETURNING CONTENT:\n[" + content + "]",
    "Debug");
                    return content.Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Update Check Error:\n\n" +
                    ex.GetType().Name + "\n\n" +
                    ex.Message +
                    "\n\nInner Error:\n" +
                    (ex.InnerException?.Message ?? "None"),
                    "SupplyBranch Update",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return "";
            }
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
            // Check whether same form is already open
            // ==========================================

            Form existingForm = pnlMain.Controls
                .OfType<Form>()
                .FirstOrDefault(f =>
                    f.GetType() == childForm.GetType());

            if (existingForm != null)
            {
                // Close existing form
                existingForm.Close();
                existingForm.Dispose();

                // Remove from panel
                pnlMain.Controls.Remove(existingForm);
            }

            // ==========================================
            // Open new form
            // ==========================================

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;

            childForm.Dock = DockStyle.Fill;
            childForm.Margin = new Padding(0);

            pnlMain.Controls.Add(childForm);

            childForm.BringToFront();
            childForm.Show();
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
            MessageBox.Show("1 - frmMain_Load START");

            UITheme.Apply(this);

            this.Text = "SupplyBranch - Checking Update...";

            try
            {
                string latestVersion = await GetLatestVersionAsync();

                MessageBox.Show("ABC-123");

                //MessageBox.Show(
                //    "Latest Version = [" + latestVersion + "]",
                //    "SupplyBranch Update Test");

                try
                {
                    string testVersion = latestVersion;

                    Debug.WriteLine("LATEST VERSION = [" + testVersion + "]");

                    this.Text = "Version: " + testVersion;

                    MessageBox.Show("TEST MESSAGE");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("VERSION TEST ERROR: " + ex.ToString());

                    MessageBox.Show(
                        "VERSION TEST ERROR:\n\n" + ex.ToString(),
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                this.Text = "SupplyBranch";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Update Error");
            }

            MessageBox.Show("5 - Update check section finished");

            lblMain.BackColor = Color.FromArgb(232, 240, 248);
            lblMain.ForeColor = Color.FromArgb(31, 78, 121);
            lblMain.Font = new Font("Segoe UI", 22F, FontStyle.Bold);

            lblMain.TextAlign = ContentAlignment.MiddleCenter;
            lblMain.AutoSize = false;
            lblMain.Dock = DockStyle.Top;
            lblMain.Height = 70;

            tsUser.Text = "User: " + CurrentUser.UserName;
            tsDate.Text = "Date: " + DateTime.Now.ToString("dd-MMM-yyyy");
            tsTime.Text = "Time: " + DateTime.Now.ToString("hh:mm:ss tt");

            
            tsUser.Padding = new Padding(0, 0, 25, 0);
            tsDate.Padding = new Padding(0, 0, 25, 0);
            tsTime.Padding = new Padding(0, 0, 10, 0);
            OpenForm(new frmDashBoard());

            // Show or hide the "Restore Database" menu item based on the current user's username
            mnuRestoreDatabase.Visible =
            string.Equals(
           CurrentUser.UserName,
           "admin",
           StringComparison.OrdinalIgnoreCase);

            mnuRestoreSafetyBackup.Visible =
    string.Equals(
        CurrentUser.UserName,
        "admin",
        StringComparison.OrdinalIgnoreCase);

            mnuUserManagement.Visible =
    string.Equals(
        CurrentUser.UserName,
        "admin",
        StringComparison.OrdinalIgnoreCase);
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
    }
}
