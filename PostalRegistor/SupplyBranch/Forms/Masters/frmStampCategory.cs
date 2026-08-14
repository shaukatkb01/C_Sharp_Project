using SupplyBranch.DAL;
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
    public partial class frmStampCategory : Form
    {
        StampCategoryDAL dal = new StampCategoryDAL();
        private int CurrentCategoryID = 0;

        private void SearchData()
        {
            dgvCategory.DataSource = dal.Search(txtSearch.Text.Trim());
        }
       

        private void FillControls()
        {
            if (dgvCategory.CurrentRow == null)
                return;

            CurrentCategoryID = Convert.ToInt32(dgvCategory.CurrentRow.Cells["CategoryID"].Value);

            txtCategoryName.Text = dgvCategory.CurrentRow.Cells["Name"].Value.ToString();
            txtDescription.Text = dgvCategory.CurrentRow.Cells["Description"].Value?.ToString();

            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            txtCategoryName.Focus();
        }

        private void ResetForm()
        {
            CurrentCategoryID = 0;

            txtCategoryName.Clear();
            txtDescription.Clear();

            txtCategoryName.Focus();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            dgvCategory.ClearSelection();
        }


       
        private void dgvCategory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FillControls();
            }
        }



        private void ClearForm()
        {
            CurrentCategoryID = 0;

            txtCategoryName.Clear();
            txtDescription.Clear();

            txtCategoryName.Focus();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            dgvCategory.ClearSelection();
        }
        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Please enter Category Name.");

                txtCategoryName.Focus();

                return false;
            }

            return true;
        }
        private void LoadData()
        {
            dgvCategory.DataSource = dal.GetAll();

            dgvCategory.Columns["CategoryID"].HeaderText = "ID";
            dgvCategory.Columns["Name"].HeaderText = "Category";
            dgvCategory.Columns["Description"].HeaderText = "Description";
        }

        public frmStampCategory()
        {
            InitializeComponent();
        }

        private void frmStampCategory_Load(object sender, EventArgs e)
        {
            UITheme.Apply(this);
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
         
            if (!ValidateData())
                return;

            if (dal.Exists(txtCategoryName.Text.Trim(), CurrentCategoryID))
            {
                MessageBox.Show("Category already exists.");

                txtCategoryName.Focus();

                return;
            }

            StampCategoryModel model = new StampCategoryModel();

            model.Name = txtCategoryName.Text.Trim();
            model.Description = txtDescription.Text.Trim();

            if (dal.Insert(model))
            {
                MessageBox.Show("Category saved successfully.");

                LoadData();

                ClearForm();
            }
            else
            {
                MessageBox.Show("Record could not be saved.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
            if (!ValidateData())
                return;

            if (dal.Exists(txtCategoryName.Text.Trim(), CurrentCategoryID))
            {
                MessageBox.Show("Category already exists.");
                return;
            }

            StampCategoryModel model = new StampCategoryModel();

            model.CategoryID = CurrentCategoryID;
            model.Name = txtCategoryName.Text.Trim();
            model.Description = txtDescription.Text.Trim();

            if (dal.Update(model))
            {
                MessageBox.Show("Category updated successfully.");

                LoadData();

                ResetForm();
            }
        }

        

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            FillControls();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CurrentCategoryID == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this record?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (dal.Delete(CurrentCategoryID))
                {
                    MessageBox.Show("Category deleted successfully.");

                    LoadData();

                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Record could not be deleted.");
                }
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            SearchData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
