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
using static SupplyBranch.Helpers.FormStyleHelper;



namespace SupplyBranch.Forms.Transactions
{
    public partial class frmDraftSupply : Form
    {
        private bool _isFormLoading = true;
        OfficeDAL officeDAL = new OfficeDAL();
        private SupplyDAL supplyDAL = new SupplyDAL();



        private void DeleteSupply()
        {
            int supplyID = Convert.ToInt32(
                dgvDraft.CurrentRow.Cells["SupplyID"].Value);

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete this draft supply?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            bool result = supplyDAL.DeleteDraftSupply(supplyID);

            if (result)
            {
                MessageBox.Show(
                    "Draft Supply deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                SearchDraft();
            }
            else
            {
                MessageBox.Show(
                    "Only Draft Supply can be deleted.",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }


        private void LoadOffice()
        {
            DataTable dt = officeDAL.GetOfficeList();

            DataRow row = dt.NewRow();

            row["OfficeID"] = 0;
            row["OfficeName"] = "-- All Offices --";

            dt.Rows.InsertAt(row, 0);

            cmbOffice.DataSource = dt;

            cmbOffice.DisplayMember = "OfficeName";

            cmbOffice.ValueMember = "OfficeID";
        }

        private void SearchDraft()
        {
            int officeID = 0;

            if (cmbOffice.SelectedValue != null &&
                !(cmbOffice.SelectedValue is DataRowView))
            {
                officeID = Convert.ToInt32(cmbOffice.SelectedValue);
            }

            int statusID = Convert.ToInt32(cmbDraftStatus.SelectedValue);

            dgvDraft.AutoGenerateColumns = false;

            dgvDraft.DataSource = supplyDAL.GetDraftSupply(
                officeID,
                statusID,
                dtFrom.Value.Date,
                dtTo.Value.Date,
                txtSupplyNo.Text.Trim());

            if (dgvDraft.Columns.Contains("SupplyID"))
                dgvDraft.Columns["SupplyID"].Visible = false;

            dgvDraft.ClearSelection();
        }

        private void LoadDraftStatus()
        {
            DataTable dt = supplyDAL.GetDraftStatus();
            cmbDraftStatus.DataSource = dt;
            cmbDraftStatus.DisplayMember = "StatusName";
            cmbDraftStatus.ValueMember = "StatusID";
        }
        public frmDraftSupply()
        {
            InitializeComponent();
        }

        private void dgvDraft_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            //WinFormsModernizer.Apply(this);

            _isFormLoading = false;
            LoadOffice();
            LoadDraftStatus();

            dtFrom.Value = DateTime.Today;

            dtTo.Value = DateTime.Today;

            SearchDraft();


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchDraft();
        }

        private void dgvDraft_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int supplyID = Convert.ToInt32(
                dgvDraft.Rows[e.RowIndex].Cells["SupplyID"].Value);


            frmSupply frm = new frmSupply(supplyID, true);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                SearchDraft();
            }
        }

        private void cmbOffice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbDraftStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isFormLoading)
                return;

            if (cmbDraftStatus.SelectedValue == null)
                return;

            if (cmbDraftStatus.SelectedValue is DataRowView)
                return;

            SearchDraft();
        }

        private void dgvDraft_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex < 0)
                return;

            if (dgvDraft.Columns[e.ColumnIndex].Name == "Delete")
            {
                DeleteSupply();
            }
        }
    }
}
