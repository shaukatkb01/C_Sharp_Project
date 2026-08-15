using SupplyBranch.Classes;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SupplyBranch.Forms.Help
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
        
            this.Close();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            
            lblVersion.Text = "Version: " + AppVersionInfo.CurrentVersion;

            if (AppVersionInfo.CurrentVersion == AppVersionInfo.AvailableVersion)
            {
                lblAvalibleVersion.Text = "You are using the latest version.";
                btnUpdate.Enabled = false;
            }
            else
            {
                lblAvalibleVersion.Text = "A new version is available: " + AppVersionInfo.AvailableVersion;
                btnUpdate.Enabled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(AppVersionInfo.DownloadUrl))
            {
                MessageBox.Show("Download URL missing. Please check UpdateInfo.txt on server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Temp folder mein zip file save karenge
            string tempZipPath = Path.Combine(Path.GetTempPath(), "SupplyBranch_Update.zip");

            // Step 4 ka Download Form open karein
            using (var downloadForm = new frmUpdateDownload(AppVersionInfo.DownloadUrl, tempZipPath))
            {
                if (downloadForm.ShowDialog() == DialogResult.OK)
                {
                    // Download mukammal hone par auto-installer batch script chalayein
                    ApplyUpdateAndRestart(tempZipPath);
                }
            }
        }

        private void ApplyUpdateAndRestart(string zipFilePath)
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string batPath = Path.Combine(Path.GetTempPath(), "update_runner.bat");

            // Temporary Batch Script jo extraction aur app restart ko handle karegi
            string scriptContent = $@"@echo off
timeout /t 2 /nobreak > nul
powershell -Command ""Expand-Archive -Path '{zipFilePath}' -DestinationPath '{appDir}' -Force""
del ""{zipFilePath}""
start """" ""{Path.Combine(appDir, "SupplyBranch.exe")}""
del ""%~f0""
";

            File.WriteAllText(batPath, scriptContent);

            // Batch script ko background mein execute karein
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = batPath,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(psi);

            // Application close taake files lock free ho sakein
            Application.Exit();
        }
    }
}
