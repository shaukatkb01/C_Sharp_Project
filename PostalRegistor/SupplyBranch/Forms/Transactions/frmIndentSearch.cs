using SupplyBranch.DAL;
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
    public partial class frmIndentSearch : Form
    {
        IndentDAL dal = new IndentDAL();

        private void LoadZone()
        {
            cmbZone.DataSource = dal.GetZones();

            cmbZone.DisplayMember = "ZoneName";

            cmbZone.ValueMember = "ZoneID";

            cmbZone.SelectedIndex = -1;

           
        }


        private void LoadOffice(int zoneID)
        {
            cmbOffice.DataSource = dal.GetOffices(zoneID);

            cmbOffice.DisplayMember = "OfficeName";

            cmbOffice.ValueMember = "OfficeID";

            cmbOffice.SelectedIndex = -1;
        }

       
        public frmIndentSearch()
        {
            InitializeComponent();
        }

        private void frmIndentSearch_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            cmbZone.DataSource = dal.GetZones();

            cmbZone.DisplayMember = "ZoneName";

            cmbZone.ValueMember = "ZoneID";

            cmbZone.SelectedIndex = -1;

            LoadZone();

            dgvIndent.AutoGenerateColumns = false;


        }

        private void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZone.SelectedValue == null)
                return;

            if (cmbZone.SelectedValue is DataRowView)
                return;

            LoadOffice(Convert.ToInt32(cmbZone.SelectedValue));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

       
            int? zoneID = null;
            int? officeID = null;

            if (cmbZone.SelectedValue != null &&
                !(cmbZone.SelectedValue is DataRowView))
            {
                zoneID = Convert.ToInt32(cmbZone.SelectedValue);
            }

            if (cmbOffice.SelectedValue != null &&
                !(cmbOffice.SelectedValue is DataRowView))
            {
                officeID = Convert.ToInt32(cmbOffice.SelectedValue);
            }

            DataTable dt = dal.SearchIndent(
                zoneID,
                officeID,
                dtFrom.Value.Date,
                dtTo.Value.Date,
                txtSearchIndentNo.Text.Trim());

            dgvIndent.DataSource = dt;

            lblTotalRecord.Text = $"Total Records: {dt.Rows.Count}";
        }

        private void dgvIndent_CellClick(object sender, DataGridViewCellEventArgs e)
        {


        
            if (e.RowIndex < 0)
                return;

            // Edit Button
            if (dgvIndent.Columns[e.ColumnIndex].Name == "colEdit")
            {
                int indentID = Convert.ToInt32(
                    dgvIndent.Rows[e.RowIndex].Cells["IndentID"].Value);

                frmIndent frm = new frmIndent(indentID);

                frm.ShowDialog();

                btnSearch.PerformClick();   // Grid Refresh
            }

            // Delete Button
            else if (dgvIndent.Columns[e.ColumnIndex].Name == "colDelete")
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this Indent?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int indentID = Convert.ToInt32(
                        dgvIndent.Rows[e.RowIndex].Cells["IndentID"].Value);

                    if (dal.DeleteIndent(indentID))
                    {
                        MessageBox.Show(
                            "Indent deleted successfully.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        btnSearch.PerformClick();   // Refresh Grid
                    }
                    else
                    {
                        MessageBox.Show(
                            "Indent could not be deleted.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void txtSearchIndentNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbZone.SelectedIndex = -1;
            cmbOffice.SelectedIndex = -1;
            txtSearchIndentNo.Text = "";
            dtFrom.Value = DateTime.Now.AddMonths(-200);
            dtTo.Value = DateTime.Now;
        }
    }
}
