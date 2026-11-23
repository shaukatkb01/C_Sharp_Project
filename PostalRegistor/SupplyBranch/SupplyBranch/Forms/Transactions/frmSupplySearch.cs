using SupplyBranch.DataAccess;
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

namespace SupplyBranch.Forms.Transactions
{
    public partial class frmSupplySearch : Form
    {
        
        private readonly OfficeZoneDAL zoneDAL = new OfficeZoneDAL();
        private readonly OfficeDAL officeDAL = new OfficeDAL();
        private readonly SupplyDAL supplyDAL = new SupplyDAL();

       
        private void SearchIndent()
        {
            int officeID = 0;

            if (cmbOffice.SelectedValue != null &&
                !(cmbOffice.SelectedValue is DataRowView))
            {
                officeID = Convert.ToInt32(cmbOffice.SelectedValue);
            }

            int filterType = 2; // All

            if (rbPending.Checked)
                filterType = 0;
            else if (rbCompleted.Checked)
                filterType = 1;
            else if (rbAll.Checked)
                filterType = 2;

            dgvSupply.DataSource = supplyDAL.GetPendingIndent(
                officeID,
                dtFrom.Value.Date,
                dtTo.Value.Date,
                filterType,
                txtSearchIndentNo.Text.Trim());

            if (dgvSupply.Columns.Contains("IndentID"))
                dgvSupply.Columns["IndentID"].Visible = false;

            dgvSupply.ClearSelection();
        }


        private void LoadZone()
        {
            DataTable dt = zoneDAL.GetZones();

            DataRow row = dt.NewRow();

            row["ZoneID"] = 0;
            row["ZoneName"] = "All Zones";

            dt.Rows.InsertAt(row, 0);

            cmbZone.DataSource = dt;

            cmbZone.DisplayMember = "ZoneName";

            cmbZone.ValueMember = "ZoneID";
        }

        private void LoadOffice()
        {
            int zoneID = Convert.ToInt32(cmbZone.SelectedValue);

            DataTable dt = officeDAL.GetByZone(zoneID);

            DataRow row = dt.NewRow();

            row["OfficeID"] = 0;
            row["OfficeName"] = "All Offices";

            dt.Rows.InsertAt(row, 0);

            cmbOffice.DataSource = dt;

            cmbOffice.DisplayMember = "OfficeName";

            cmbOffice.ValueMember = "OfficeID";
        }


        public frmSupplySearch()
        {
            InitializeComponent();
        }

        private void frmSupplySearch_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            LoadZone();

            rbPending.Checked = true;

            dtFrom.Value = DateTime.Today;

            dtTo.Value = DateTime.Today;

            SearchIndent();
        }

        private void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbZone.SelectedValue == null)
                return;

            if (cmbZone.SelectedValue is DataRowView)
                return;

            LoadOffice();
            SearchIndent();
        }

        private void rbPending_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPending.Checked)
                SearchIndent();
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPending.Checked)
                SearchIndent();
        }

        private void rbCompleted_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPending.Checked)
                SearchIndent();
        }

        private void cmbOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedValue is DataRowView)
                return;

            
            SearchIndent();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchIndent();
        }

        private void dgvSupply_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int indentID = Convert.ToInt32(
                dgvSupply.Rows[e.RowIndex].Cells["IndentID"].Value);

            frmSupply frm = new frmSupply(indentID);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                SearchIndent();   // Grid Refresh
            }
        }

        private void rbCompleted_Click(object sender, EventArgs e)
        {
            SearchIndent();

        }

        private void rbAll_Click(object sender, EventArgs e)
        {
            SearchIndent();

        }

        private void rbPending_Click(object sender, EventArgs e)
        {
            SearchIndent();

        }

        private void btnNewSupply_Click(object sender, EventArgs e)
        {
            cmbZone.SelectedIndex = -0;
            cmbOffice.SelectedIndex = -1;
            dtFrom.Value = DateTime.Today;
            dtTo.Value = DateTime.Today;
            txtSearchIndentNo.Text = string.Empty;


        }

        private void txtSearchIndentNo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearchIndentNo.Text))
            {
                dtFrom.Value = new DateTime(1947, 8, 1);
            }
            else
            {
                dtFrom.Value = DateTime.Today;
            }
            SearchIndent();

        }
    }
}
