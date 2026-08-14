using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmPostal_Stationery : Form
    {
        public frmPostal_Stationery()
        {
            InitializeComponent();
        }

        private void btnIndent_Click(object sender, EventArgs e)
        {
            frmIndent frmIndent = new frmIndent();
            frmIndent.ShowDialog();
        }

        private void frmPostal_Stationery_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
        }
    }
}
