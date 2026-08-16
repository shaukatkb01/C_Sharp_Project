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
using SupplyBranch.DataAccess;
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
                    // Direct update apply karein aur app exit kar dein
                    UpdateDAL.ApplyUpdateAndRestart(tempZipPath);
                }
            }
        }
              
    }
}
