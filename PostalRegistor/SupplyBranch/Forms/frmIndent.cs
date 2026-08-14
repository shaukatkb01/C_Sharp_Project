using SupplyBranch.DAL;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace SupplyBranch.Forms
{

    public partial class frmIndent : Form
    {

        UnitConversionDAL conversionDAL = new UnitConversionDAL();
        private readonly IndentDAL dal = new IndentDAL();

            private DataTable dtItems;

        private int _indentID = 0;

        private int EditRowIndex = -1;
       
            private int PiecesPerSheet = 0;

        // private int SheetsPerSet = 0;

            private int IndentID = 0;

            private bool IsEditMode = false;

        

        private bool CanAddCategory(int newCategoryID)
        {
            bool hasService = false;
            bool hasOtherCategory = false;

            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow)
                    continue;

                object value = row.Cells["CategoryID"].Value;

                if (value == null || value == DBNull.Value)
                    continue;

                int categoryID = Convert.ToInt32(value);

                if (categoryID == 2)
                    hasService = true;
                else
                    hasOtherCategory = true;
            }

            // ==========================================
            // Grid is empty
            // ==========================================

            if (!hasService && !hasOtherCategory)
                return true;

            // ==========================================
            // Service already exists
            // Do not allow other category
            // ==========================================

            if (hasService && newCategoryID != 2)
            {
                MessageBox.Show(
                    "Service is already added.\n\n" +
                    "No other category can be added with Service.",
                    "Category Not Allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            // ==========================================
            // Other category already exists
            // Do not allow Service
            // ==========================================

            if (hasOtherCategory && newCategoryID == 2)
            {
                MessageBox.Show(
                    "Other categories are already added.\n\n" +
                    "Service cannot be added with another category.",
                    "Category Not Allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }
        private void CalculateTotalPieces()
        {
            int sheetQty = 0;
            int pieceQty = 0;

            int.TryParse(txtSheetQty.Text, out sheetQty);

            int.TryParse(txtPieceQty.Text, out pieceQty);

            int total = (sheetQty * PiecesPerSheet) + pieceQty;

            lblTotalPieces.Text = "Total Pieces = " + total;
        }
        private void SetupGrid()
        {
            dgvItems.AutoGenerateColumns = true;

            dgvItems.Columns["DetailID"].Visible = false;
            dgvItems.Columns["CategoryID"].Visible = false;
            dgvItems.Columns["DenominationID"].Visible = false;

            dgvItems.Columns["Category"].HeaderText = "Category";
            dgvItems.Columns["Denomination"].HeaderText = "Denomination";
            dgvItems.Columns["SheetQty"].HeaderText = "Sheets";
            dgvItems.Columns["PieceQty"].HeaderText = "Pieces";

            if (!dgvItems.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();

                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;

                dgvItems.Columns.Add(btnEdit);
            }

            if (!dgvItems.Columns.Contains("Delete"))
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();

                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;

                dgvItems.Columns.Add(btnDelete);
            }
        }
        private void CreateTempTable()
        {
            dtItems = new DataTable();

            dtItems.Columns.Add("DetailID", typeof(int));

            dtItems.Columns.Add("CategoryID", typeof(int));

            dtItems.Columns.Add("Category", typeof(string));

            dtItems.Columns.Add("DenominationID", typeof(int));

            //dtItems.Columns.Add("Denomination", typeof(decimal));
            dtItems.Columns.Add("Denomination", typeof(string));


            dtItems.Columns.Add("SheetQty", typeof(int));

            dtItems.Columns.Add("PieceQty", typeof(int));

            dtItems.Columns.Add("PiecesPerSheet", typeof(int));
            dtItems.Columns.Add("TotalPieces", typeof(int), "SheetQty * PiecesPerSheet + PieceQty");
            dtItems.Columns.Add("Remarks", typeof(string));
            dgvItems.DataSource = dtItems;

            SetupGrid();
        }

        private void ResetForm()
        {
            // Header
            txtIndentNo.Clear();

            dtpIndentDate.Value = DateTime.Today;

            cmbZone.SelectedIndex = -1;

            cmbOffice.DataSource = null;

            UnlockHeader();

            // Items
            CreateTempTable();

            ClearItem();

            txtSheetQty.Text = "0";

            txtPieceQty.Text = "0";

            txtIndentNo.Focus();
        }

        private bool ValidateHeader()
        {
            if (txtIndentNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter indent number.");
                txtIndentNo.Focus();
                return false;
            }

            if (cmbZone.SelectedIndex == -1)
            {
                MessageBox.Show("Please select zone.");
                cmbZone.Focus();
                return false;
            }

            if (cmbOffice.SelectedIndex == -1)
            {
                MessageBox.Show("Please select office.");
                cmbOffice.Focus();
                return false;
            }

            if (dtItems.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one item.");
                return false;
            }

            return true;
        }

        private void FillControls()
        {
            if (EditRowIndex < 0)
                return;

            cmbCategory.SelectedValue = dtItems.Rows[EditRowIndex]["CategoryID"];

            cmbDenomination.SelectedValue = dtItems.Rows[EditRowIndex]["DenominationID"];

            txtSheetQty.Text = dtItems.Rows[EditRowIndex]["SheetQty"].ToString();

            txtPieceQty.Text = dtItems.Rows[EditRowIndex]["PieceQty"].ToString();
            
            txtIndentRemarks.Text = dtItems.Rows[EditRowIndex]["Remarks"].ToString(); 

            btnAddItem.Text = "Update Item";

            cmbCategory.Focus();
        }

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

        private void LoadCategory()
        {
            cmbCategory.DataSource = dal.GetCategories();

            cmbCategory.DisplayMember = "Name";

            cmbCategory.ValueMember = "CategoryID";

            cmbCategory.SelectedIndex = -1;
        }

        private void LoadDenomination(int categoryID)
        {
            cmbDenomination.DataSource = dal.GetDenominations(categoryID);

            cmbDenomination.DisplayMember = "DisplayDenomination";

            cmbDenomination.ValueMember = "DenominationID";

            cmbDenomination.SelectedIndex = -1;
        }

        

        private bool ValidateItem()
        {
            if (txtIndentNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter indent number.");
                txtIndentNo.Focus();
                return false;
            }

            if (!dtpIndentDate.Checked)
            {
                MessageBox.Show("Please select indent date.");
                dtpIndentDate.Focus();
                return false;
            }

            if (cmbZone.SelectedIndex == -1)
            {
                MessageBox.Show("Please select zone.");
                cmbZone.Focus();
                return false;
            }

            if (cmbOffice.SelectedIndex == -1)
            {
                MessageBox.Show("Please select office.");
                cmbOffice.Focus();
                return false;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select category.");
                cmbCategory.Focus();
                return false;
            }

            if (cmbDenomination.SelectedIndex == -1)
            {
                MessageBox.Show("Please select denomination.");
                cmbDenomination.Focus();
                return false;
            }

            int sheetQty = 0;
            int pieceQty = 0;

            int.TryParse(txtSheetQty.Text.Trim(), out sheetQty);
            int.TryParse(txtPieceQty.Text.Trim(), out pieceQty);

            if (sheetQty == 0 && pieceQty == 0)
            {
                MessageBox.Show("Please enter Sheet Qty or Piece Qty.");

                txtSheetQty.Focus();
                return false;
            }

            return true;
        }

        private void ClearItem()
        {
            cmbCategory.SelectedIndex = -1;

            cmbDenomination.DataSource = null;
            txtIndentRemarks.Clear();
            txtSheetQty.Text = "0";
            txtPieceQty.Text = "0";

            btnAddItem.Text = "Add Item";

            EditRowIndex = -1;

            cmbCategory.Focus();
        }

        private void LockHeader()
        {
            txtIndentNo.ReadOnly = true;

            dtpIndentDate.Enabled = false;

            cmbZone.Enabled = false;

            cmbOffice.Enabled = false;
        }

        private void UnlockHeader()
        {
            txtIndentNo.ReadOnly = false;

            dtpIndentDate.Enabled = true;

            cmbZone.Enabled = true;

            cmbOffice.Enabled = true;
        }

        public frmIndent(int indentID)
        {
            InitializeComponent();
            _indentID = indentID;


            IsEditMode = true;
        }

       

        

        public frmIndent()
        {
            InitializeComponent();
        }

        private void LoadIndent(int indentID)
        {
            // =========================
            // Load Header
            // =========================

            DataRow header = dal.GetIndentHeader(indentID);

            if (header == null)
            {
                MessageBox.Show("Indent not found.");
                Close();
                return;
            }

            txtIndentNo.Text = header["IndentNo"].ToString();

            dtpIndentDate.Value =
                Convert.ToDateTime(header["IndentDate"]);

            txtIndentRemarks.Text =
                header["Remarks"] == DBNull.Value
                ? ""
                : header["Remarks"].ToString();

            cmbZone.SelectedValue = header["ZoneID"];

            LoadOffice(Convert.ToInt32(header["ZoneID"]));

            cmbOffice.SelectedValue = header["OfficeID"];


            // =========================
            // Load Details
            // =========================

            DataTable dt = dal.GetIndentDetails(indentID);

            dtItems.Rows.Clear();

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = dtItems.NewRow();

                newRow["DetailID"] = row["DetailID"];
                newRow["CategoryID"] = row["CategoryID"];
                newRow["Category"] = row["Category"];
                newRow["DenominationID"] = row["DenominationID"];
                newRow["Denomination"] = row["Denomination"];
                newRow["SheetQty"] = row["SheetQty"];
                newRow["PieceQty"] = row["PieceQty"];
                newRow["PiecesPerSheet"] = row["PiecesPerSheet"];
                newRow["TotalPieces"] = row["TotalPieces"];

                dtItems.Rows.Add(newRow);
            }


            // =========================
            // Refresh
            // =========================

            CalculateTotalPieces();

            dgvItems.Refresh();

            EditRowIndex = -1;

            btnSave.Enabled = true;
            btnSave.Text = "Update";
        }

        private void frmIndent_Load(object sender, EventArgs e)
        {
           
            UITheme.Apply(this);

            btnSave.Enabled = true;

            LoadZone();
            LoadCategory();

            CreateTempTable();

            // =========================
            // EDIT MODE
            // =========================

            if (_indentID > 0)
            {
                btnSave.Text = "Update";

                LoadIndent(_indentID);
            }
            else
            {
                // =========================
                // NEW MODE
                // =========================

                btnSave.Text = "Save";

                txtSheetQty.Text = "0";
                txtPieceQty.Text = "0";

                ResetForm();
            }
        }

        private void cmbZone_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue == null)
                return;


            if (int.TryParse(cmbCategory.SelectedValue.ToString(), out int categoryID))
            {
                LoadDenomination(categoryID);
            
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
          
            if (!ValidateItem())
                return;
            if (dgvItems.Rows.Count >= 1 && EditRowIndex == -1)
            {
                int categoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                if (!CanAddCategory(categoryID))
                    return;

            }
            int sheetQty = 0;
            int pieceQty = 0;

            int.TryParse(txtSheetQty.Text.Trim(), out sheetQty);
            int.TryParse(txtPieceQty.Text.Trim(), out pieceQty);

            // Duplicate Check
            foreach (DataRow row in dtItems.Rows)
            {
                if (EditRowIndex == -1)
                {
                    if (Convert.ToInt32(row["CategoryID"]) == Convert.ToInt32(cmbCategory.SelectedValue)
                        && Convert.ToInt32(row["DenominationID"]) == Convert.ToInt32(cmbDenomination.SelectedValue))
                    {
                        MessageBox.Show("This denomination is already added.");
                        return;
                    }
                }
            }

            if (EditRowIndex == -1)
            {
                DataRow dr = dtItems.NewRow();

                dr["DetailID"] = 0;
                dr["CategoryID"] = cmbCategory.SelectedValue;
                dr["Category"] = cmbCategory.Text;
                dr["DenominationID"] = cmbDenomination.SelectedValue;
                dr["Denomination"] = cmbDenomination.Text;

                dr["SheetQty"] = sheetQty;
                dr["PieceQty"] = pieceQty;
                dr["PiecesPerSheet"] = PiecesPerSheet;
                //dr["Remarks"] = txtIndentRemarks.Text.Trim();

                dtItems.Rows.Add(dr);
                
                LockHeader();
            }
            else
            {
                dtItems.Rows[EditRowIndex]["CategoryID"] = cmbCategory.SelectedValue;
                dtItems.Rows[EditRowIndex]["Category"] = cmbCategory.Text;
                dtItems.Rows[EditRowIndex]["DenominationID"] = cmbDenomination.SelectedValue;
                dtItems.Rows[EditRowIndex]["Denomination"] = cmbDenomination.Text;

                dtItems.Rows[EditRowIndex]["SheetQty"] = sheetQty;
                dtItems.Rows[EditRowIndex]["PieceQty"] = pieceQty;
                dtItems.Rows[EditRowIndex]["PiecesPerSheet"] = PiecesPerSheet;
                //dtItems.Rows[EditRowIndex]["Remarks"] = txtIndentRemarks.Text.Trim();
                txtIndentNo.ReadOnly = true;
            }

            ClearItem();
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // Edit
            if (dgvItems.Columns[e.ColumnIndex].Name == "Edit")
            {
                EditRowIndex = e.RowIndex;

                FillControls();
                txtIndentNo.ReadOnly = false;

            }

            // Delete
            else if (dgvItems.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this item?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dtItems.Rows.RemoveAt(e.RowIndex);

                    ClearItem();
                }
                if (dtItems.Rows.Count == 0)
                {
                    UnlockHeader();
                }
            }
        }

        private void txtSheetQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
        !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPieceQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
        !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            UnlockHeader();

            
        }

        private void btnClearItem_Click(object sender, EventArgs e)
        {
            ClearItem();
        }

       

        private void btnSave_Click(object sender, EventArgs e)
        {
       
            if (!ValidateHeader())
                return;

            try
            {
               
                IndentMasterModel master = new IndentMasterModel();

                master.IndentID = _indentID;  

                master.IndentNo = txtIndentNo.Text.Trim();
                master.IndentDate = dtpIndentDate.Value;
                master.OfficeID = Convert.ToInt32(cmbOffice.SelectedValue);
                master.Remarks = txtIndentRemarks.Text.Trim();
                //=========================================
                // EDIT / UPDATE MODE
                //=========================================

                if (_indentID > 0)
                {
                  
                    if (dal.Exists(txtIndentNo.Text.Trim(), _indentID))
                    {
                        MessageBox.Show(
                            "This Indent Number already exists.",
                            "Duplicate Indent",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        txtIndentNo.Focus();
                        return;
                    }

                    if (dal.UpdateIndent(master, dtItems))
                    {
                        MessageBox.Show(
                            "Indent updated successfully.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Indent could not be updated.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }

                //if (IndentID > 0)
                //{
                //    MessageBox.Show(
                //        "IndentID = " + IndentID +
                //        "\nIndentNo = " + txtIndentNo.Text.Trim());

                //    bool exists = dal.Exists(
                //        txtIndentNo.Text.Trim(),
                //        IndentID);

                //    MessageBox.Show("Exists Result = " + exists);

                //    if (exists)
                //    {
                //        MessageBox.Show("This Indent Number already exists.");
                //        return;
                //    }

                //    // Update یہاں آئے گا
                //}

                //=========================================
                // NEW / SAVE MODE
                //=========================================

                else
                {
                    if (dal.Exists(txtIndentNo.Text.Trim()))
                    {
                        MessageBox.Show(
                            "This Indent Number already exists.",
                            "Duplicate Indent",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        txtIndentNo.Focus();
                        return;
                    }

                    if (dal.SaveIndent(master, dtItems))
                    {
                        MessageBox.Show(
                            "Indent saved successfully.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Indent could not be saved.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cmbDenomination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDenomination.SelectedValue == null)
                return;

            if (cmbDenomination.SelectedValue is DataRowView)
                return;
            if (cmbDenomination.SelectedIndex == -1)
                return;

            

            DataRow dr = conversionDAL.GetConversion(
                Convert.ToInt32(cmbDenomination.SelectedValue));

            if (dr != null)
            {
                PiecesPerSheet = Convert.ToInt32(dr["PiecesPerSheet"]);

              
                lblStampPersheet.Text = "1 Sheet = " + PiecesPerSheet + " Pieces";

                
                CalculateTotalPieces();
            }
            else
            {
                PiecesPerSheet = 0;

                

                lblStampPersheet.Text = "";

                

                lblTotalPieces.Text = "";
            }
        }

        private void txtSheetQty_Leave(object sender, EventArgs e)
        {
            CalculateTotalPieces();
        }

        private void txtPieceQty_Leave(object sender, EventArgs e)
        {
            CalculateTotalPieces();
        }

        private void cmbZone_SelectedIndexChanged_1(object sender, EventArgs e)
        {
         
            if (cmbZone.SelectedValue == null)
                return;

            if (cmbZone.SelectedValue is DataRowView)
                return;

            LoadOffice(Convert.ToInt32(cmbZone.SelectedValue));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
            //if (!ValidateHeader())
            //    return;

            IndentMasterModel master = new IndentMasterModel();

            master.IndentID = IndentID;
            master.IndentNo = txtIndentNo.Text.Trim();
            master.IndentDate = dtpIndentDate.Value;
            master.OfficeID = Convert.ToInt32(cmbOffice.SelectedValue);
            master.Remarks = txtIndentRemarks.Text.Trim();

            if (dal.UpdateIndent(master, dtItems))
            {
                MessageBox.Show("Indent updated successfully.");

                Close();
            }
            else
            {
                MessageBox.Show("Indent could not be updated.");
            }
        }

        private void txtSheetQty_TextChanged(object sender, EventArgs e)
        {
            if (txtSheetQty.Text!="0" )
            {
                btnSave.Enabled = false;
            }
            else { btnSave.Enabled = true; }
        }

        private void txtPieceQty_TextChanged(object sender, EventArgs e)
        {
            if ( txtPieceQty.Text != "0")
            {
                btnSave.Enabled = false;
            }
            else { btnSave.Enabled = true; }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
