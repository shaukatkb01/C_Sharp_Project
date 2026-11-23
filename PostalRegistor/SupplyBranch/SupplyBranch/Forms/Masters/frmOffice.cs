using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms.Masters
{
    public partial class frmOffice : Form
    {
    private int CurrentOfficeID = 0;
        private void LoadData()
        {
            OfficeDAL dal = new OfficeDAL();

            dgvOffice.DataSource = dal.GetAll();

            dgvOffice.Columns["OfficeID"].Visible = false;

            dgvOffice.Columns["ZoneID"].Visible = false;

            dgvOffice.Columns["ZoneName"].HeaderText = "Zone";

            dgvOffice.Columns["OfficeName"].HeaderText = "Office Name";

            dgvOffice.Columns["OfficeFileNo"].HeaderText = "File No";

            dgvOffice.Columns["OfficeCode"].HeaderText = "Office Code";
        }

        private void LoadZones()
        {
            OfficeZoneDAL dal = new OfficeZoneDAL();

            cmbZone.DataSource = dal.GetZones();

            cmbZone.DisplayMember = "ZoneName";

            cmbZone.ValueMember = "ZoneID";

            cmbZone.SelectedIndex = -1;
        }

        public frmOffice()
        {
            InitializeComponent();
        }

        private void frmOffice_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);

            LoadZones();
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
          
            if (cmbZone.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Zone.");
                cmbZone.Focus();
                return;
            }

            if (txtOfficeName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Office Name.");
                txtOfficeName.Focus();
                return;
            }

            if (txtOfficeFileNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Office File No.");
                txtOfficeFileNo.Focus();
                return;
            }

            OfficeDAL dal = new OfficeDAL();

            if (dal.IsOfficeExists(txtOfficeName.Text.Trim(),
                Convert.ToInt32(cmbZone.SelectedValue),
                CurrentOfficeID))
            {
                MessageBox.Show("Office Name already exists in this Zone.");
                txtOfficeName.Focus();
                txtOfficeName.SelectAll();
                return;
            }

            if (dal.IsFileNoExists(txtOfficeFileNo.Text.Trim(), CurrentOfficeID))
            {
                MessageBox.Show("Office File No already exists.");
                txtOfficeFileNo.Focus();
                txtOfficeFileNo.SelectAll();
                return;
            }

            Office office = new Office();

            office.OfficeID = CurrentOfficeID;
            office.OfficeName = txtOfficeName.Text.Trim();
            office.OfficeFileNo = txtOfficeFileNo.Text.Trim();
            office.OfficeCode = txtOfficeCode.Text.Trim();
            office.ZoneID = Convert.ToInt32(cmbZone.SelectedValue);

            bool result;

            if (CurrentOfficeID == 0)
            {
                result = dal.Save(office);
            }
            else
            {
                result = dal.Update(office);
            }

            if (result)
            {
                MessageBox.Show("Record saved successfully.");

                LoadData();

                btnNew.PerformClick();
            }
            else
            {
                MessageBox.Show("Operation failed.");
            }
        }

        private void dgvOffice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex < 0)
                return;

            CurrentOfficeID = Convert.ToInt32(dgvOffice.Rows[e.RowIndex].Cells["OfficeID"].Value);

            cmbZone.SelectedValue = dgvOffice.Rows[e.RowIndex].Cells["ZoneID"].Value;

            txtOfficeName.Text = dgvOffice.Rows[e.RowIndex].Cells["OfficeName"].Value.ToString();

            txtOfficeFileNo.Text = dgvOffice.Rows[e.RowIndex].Cells["OfficeFileNo"].Value.ToString();

            txtOfficeCode.Text = dgvOffice.Rows[e.RowIndex].Cells["OfficeCode"].Value.ToString();

            btnSave.Text = "Update";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
          
            CurrentOfficeID = 0;

            cmbZone.SelectedIndex = -1;

            txtOfficeName.Clear();

            txtOfficeFileNo.Clear();

            txtOfficeCode.Clear();

            btnSave.Text = "Save";

            txtOfficeName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
            if (CurrentOfficeID == 0)
            {
                MessageBox.Show("Please select a record.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this Office?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            try
            {
                OfficeDAL dal = new OfficeDAL();

                if (dal.Delete(CurrentOfficeID))
                {
                    MessageBox.Show("Record deleted successfully.");

                    LoadData();

                    btnNew.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "This Office cannot be deleted because it is being used in another record.\n\n" + ex.Message,
                    "Delete Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
          
            OfficeDAL dal = new OfficeDAL();

            dgvOffice.DataSource = dal.Search(txtSearch.Text.Trim());
        }
    
    }
}
