using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SupplyBranch.Classes;
using SupplyBranch.Helpers;
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
    }
}
