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
    public partial class frmOfficeZone : Form
    {

        private int CurrentZoneID = 0;

        private void LoadData()
        {
            OfficeZoneDAL dal = new OfficeZoneDAL();

            dgvZone.DataSource = dal.GetAll();

            dgvZone.Columns["ZoneID"].HeaderText = "Zone ID";
            dgvZone.Columns["ZoneName"].HeaderText = "Zone Name";

            dgvZone.Columns["ZoneID"].Width = 80;
        }
        public frmOfficeZone()
        {

            InitializeComponent();
        }

        private void frmOfficeZone_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
         
            if (txtZoneName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Zone Name.");
                txtZoneName.Focus();
                return;
            }

            OfficeZoneDAL dal = new OfficeZoneDAL();

            if (dal.IsZoneExists(txtZoneName.Text.Trim(), CurrentZoneID))
            {
                MessageBox.Show("Zone Name already exists.");
                txtZoneName.Focus();
                txtZoneName.SelectAll();
                return;
            }

            OfficeZone zone = new OfficeZone();

            zone.ZoneID = CurrentZoneID;
            zone.ZoneName = txtZoneName.Text.Trim();

            bool result;

            if (CurrentZoneID == 0)
            {
                result = dal.Save(zone);
            }
            else
            {
                result = dal.Update(zone);
            }

            if (result)
            {
                MessageBox.Show("Record saved successfully.");

                LoadData();

                txtZoneName.Clear();

                txtZoneName.Focus();

                CurrentZoneID = 0;

                btnSave.Text = "Save";
            }
            else
            {
                MessageBox.Show("Operation failed.");
            }
        
        }


        

        private void dgvZone_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                CurrentZoneID = Convert.ToInt32(dgvZone.Rows[e.RowIndex].Cells["ZoneID"].Value);

                txtZoneName.Text = dgvZone.Rows[e.RowIndex].Cells["ZoneName"].Value.ToString();

                btnSave.Text = "Update";

                txtZoneName.Focus();
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
         
            CurrentZoneID = 0;

            txtZoneName.Clear();

            txtZoneName.Focus();

            btnSave.Text = "Save";
        
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
          
            if (CurrentZoneID == 0)
            {
                MessageBox.Show("Please select a record.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this Zone?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            try
            {
                OfficeZoneDAL dal = new OfficeZoneDAL();

                if (dal.Delete(CurrentZoneID))
                {
                    MessageBox.Show("Record deleted successfully.");

                    LoadData();

                    btnNew.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "This Zone cannot be deleted because it is being used by another record.\n\n" + ex.Message,
                    "Delete Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    
    }
}
