using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostalStampSystem;
using Microsoft.Reporting.WinForms;

namespace FileIndex
{

    public partial class StationeryTransactionscs : Form
    {
        int id = 0;
        // Inhein Form class ke shuru mein likhen
        int selectedTransId = 0;
        bool isSelectionProcess = false;

        // for update
        public void updateStationery()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();

                try
                {
                    string query = @"UPDATE StationeryTransactions
                            SET ItemId = @itid, TransDate = @sd, AddressId = @aid,
                            SupplyTypeId = @sid, Qty=@q, Remarks = @remark
                           
                            WHERE TransId=@tid";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@tid", selectedTransId);
                    cmd.Parameters.AddWithValue("@itid", cmb_item.SelectedValue);
                    cmd.Parameters.AddWithValue("@sd", dt_Supply.Value.Date);
                    cmd.Parameters.AddWithValue("@aid", cmb_Address.SelectedValue);
                    cmd.Parameters.AddWithValue("@sid", cmb_ST.SelectedValue);
                    cmd.Parameters.AddWithValue("@q", Convert.ToInt32(num_out.Value));
                    cmd.Parameters.AddWithValue("@remark", string.IsNullOrWhiteSpace(txt_Remarks.Text) ? (object)DBNull.Value : txt_Remarks.Text.Trim());

                    cmd.ExecuteNonQuery();

                    selectedTransId = 0;
                    MessageBox.Show("Update Seccesfully");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error in update" + ex.Message);
                }
            }
        }
        public void StationeryGridLoad(int? SelectedId) // 'int?' ka matlab hai ke ye null accept kar sakta hai
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    // SQL Query mein tabdeeli: 
                    // Agar @id null hai (@id IS NULL) ya Column @id ke barabar hai (ItemId = @id)
                    string query = @"SELECT  
                S.TransId,
                ISNULL(I.ItemName, 'N/A') AS ItemName,
                ISNULL(A.Address, 'No Address') AS Address,
                S.TransDate,
                S.ItemId AS ItemID,       
                ISNULL(S.Qty, 0) AS Qty,
                S.AddressId AS AddressID, 
                S.SupplyTypeID AS STID,   
                ISNULL(T.SupplyType, 'Standard') AS SupplyType,
                ISNULL(S.Remarks, '') AS Remarks
            FROM StationeryTransactions S
            LEFT JOIN StationeryItems I ON S.ItemId = I.ItemID
            LEFT JOIN PhilitelicBuearu A ON S.AddressId = A.Id
            LEFT JOIN SupplyType T ON S.SupplyTypeID = T.ID
            WHERE (@id IS NULL OR @id = 0 OR S.ItemId = @id)"; // Teeno conditions handle ho gayi

                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    // DBNull.Value ka istemal karein agar SelectedId null ho
                    if (SelectedId == null)
                        da.SelectCommand.Parameters.AddWithValue("@id", DBNull.Value);
                    else
                        da.SelectCommand.Parameters.AddWithValue("@id", SelectedId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Column settings (Check ke saath taake error na aaye)
                    if (dt.Rows.Count > 0)
                    {
                        if (dataGridView1.Columns.Contains("TransId")) dataGridView1.Columns["TransId"].Visible = false;
                        if (dataGridView1.Columns.Contains("ItemID")) dataGridView1.Columns["ItemID"].Visible = false;
                        if (dataGridView1.Columns.Contains("AddressID")) dataGridView1.Columns["AddressID"].Visible = false;
                        if (dataGridView1.Columns.Contains("STID")) dataGridView1.Columns["STID"].Visible = false;

                        dataGridView1.Columns["TransDate"].HeaderText = "Supply Date";
                        dataGridView1.Columns["ItemName"].HeaderText = "Item Name";
                        dataGridView1.Columns["Address"].HeaderText = "Address";
                        dataGridView1.Columns["ItemName"].Width = 250;
                        dataGridView1.Columns["Address"].Width = 350;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        public void StationItemComboLoad()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = "SELECT ItemID, ItemName FROM StationeryItems";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmb_item.DisplayMember = "ItemName";
                    cmb_item.ValueMember = "ItemID";
                    cmb_item.DataSource = dt;
                    cmb_item.SelectedIndex = -1; // No selection by default
                    cmb_item.DropDownWidth = 300;

                    string query2 = @"SELECT ID, SupplyType
                                    FROM SupplyType
                                    WHERE ID IN (3,4)";

                    SqlDataAdapter da2 = new SqlDataAdapter(query2, con);
                    DataTable dt2 = new DataTable();

                    da2.Fill(dt2);
                    cmb_ST.DisplayMember = "SupplyType";
                    cmb_ST.ValueMember = "ID";
                    cmb_ST.DataSource = dt2;
                    cmb_ST.SelectedIndex = -1;
                    cmb_ST.DropDownWidth = 100;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        public void AddressComboLoad()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = "SELECT Id, (ISNULL(Name, 'No Name') + ' - ' + ISNULL(Address, 'No Address')) AS Addressdetail FROM PhilitelicBuearu;";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmb_Address.DisplayMember = "Addressdetail";
                    cmb_Address.ValueMember = "Id";
                    cmb_Address.DataSource = dt;
                    cmb_Address.SelectedIndex = -1; // No selection by default

                    cmb_Address.DropDownWidth = 300;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        public void ClearFields()
        {
            cmb_item.SelectedIndex = -1;
            cmb_Address.SelectedIndex = -1;
            cmb_ST.SelectedIndex = -1;
            num_out.Value = 0;
            txt_Remarks.Clear();
        }

        /// <summary>
        /// Stationery transaction add karta hai with proper validation and error handling
        /// </summary>
        public void AddStationery()
        {
            // ═══════════════════════════════════════════════════════════════
            // STEP 1: INPUT VALIDATION (User-friendly messages)
            // ═══════════════════════════════════════════════════════════════

            // Quantity validation
            if (num_out.Value == 0)
            {
                MessageBox.Show("⚠️ Please enter Quantity!\n\nQuantity cannot be zero.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                num_out.Focus();
                return;
            }

            // Item selection validation
            if (cmb_item.SelectedIndex == -1 || cmb_item.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Please select an Item!",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmb_item.Focus();
                cmb_item.DroppedDown = true;
                return;
            }

            // Address selection validation
            if (cmb_Address.SelectedIndex == -1 || cmb_Address.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Please select an Address!",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmb_Address.Focus();
                cmb_Address.DroppedDown = true;
                return;
            }

            // Supply Type selection validation
            if (cmb_ST.SelectedIndex == -1 || cmb_ST.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Please select a Supply Type!",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmb_ST.Focus();
                cmb_ST.DroppedDown = true;
                return;
            }

            // Remarks length validation (max 255 characters)
            if (!string.IsNullOrWhiteSpace(txt_Remarks.Text) && txt_Remarks.Text.Trim().Length > 255)
            {
                MessageBox.Show("⚠️ Remarks is too long!\n\nMaximum 255 characters allowed.\n" +
                    $"Current length: {txt_Remarks.Text.Trim().Length}",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txt_Remarks.Focus();
                return;
            }

            // ═══════════════════════════════════════════════════════════════
            // STEP 2: DATABASE OPERATION (Using block for safety)
            // ═══════════════════════════════════════════════════════════════

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    con.Open();

                    string query = @"INSERT INTO StationeryTransactions 
                (ItemId, TransDate, AddressId, SupplyTypeId, Qty, Remarks)
                VALUES(@itemId, @dt, @ad, @stid, @qty, @remark)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // ✅ ItemId - INT
                        cmd.Parameters.Add("@itemId", SqlDbType.Int).Value =
                            Convert.ToInt32(cmb_item.SelectedValue);

                        // ✅ TransDate - DATETIME (only date part)
                        cmd.Parameters.Add("@dt", SqlDbType.DateTime).Value =
                            dt_Supply.Value.Date;

                        // ✅ AddressId - INT
                        cmd.Parameters.Add("@ad", SqlDbType.Int).Value =
                            Convert.ToInt32(cmb_Address.SelectedValue);

                        // ✅ SupplyTypeId - TINYINT (0-255 range)
                        cmd.Parameters.Add("@stid", SqlDbType.TinyInt).Value =
                            Convert.ToByte(cmb_ST.SelectedValue);

                        // ✅ Qty - INT
                        cmd.Parameters.Add("@qty", SqlDbType.Int).Value =
                            Convert.ToInt32(num_out.Value);

                        // ✅ Remarks - NVARCHAR(255) - NULL allowed
                        cmd.Parameters.Add("@remark", SqlDbType.NVarChar, 255).Value =
                            string.IsNullOrWhiteSpace(txt_Remarks.Text)
                                ? (object)DBNull.Value
                                : txt_Remarks.Text.Trim();

                        // Execute query
                        cmd.ExecuteNonQuery();
                    }

                    // ═══════════════════════════════════════════════════════════════
                    // STEP 3: SUCCESS - Show message and clear fields
                    // ═══════════════════════════════════════════════════════════════

                    MessageBox.Show("✅ Stationery supply added successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearFields();
                }
                catch (SqlException sqlEx)
                {
                    // ═══════════════════════════════════════════════════════════════
                    // SQL-SPECIFIC ERROR HANDLING
                    // ═══════════════════════════════════════════════════════════════

                    string errorMsg = "";

                    switch (sqlEx.Number)
                    {
                        case 547: // Foreign Key Violation
                            errorMsg = "❌ Invalid Reference!\n\n" +
                                      "One of the selected values does not exist in the database:\n" +
                                      "• Check if the selected Item exists\n" +
                                      "• Check if the selected Address exists\n" +
                                      "• Check if the selected Supply Type exists\n\n" +
                                      "Please refresh the form and try again.";
                            break;

                        case 2627: // Primary Key Violation
                        case 2601: // Unique Constraint
                            errorMsg = "❌ Duplicate Entry!\n\n" +
                                      "This transaction already exists in the database.";
                            break;

                        case 8152: // String Truncation
                        case 2628: // String or binary data would be truncated
                            errorMsg = "❌ Data Too Long!\n\n" +
                                      "Remarks text exceeds the maximum length (255 characters).\n" +
                                      $"Current length: {txt_Remarks.Text?.Trim().Length ?? 0}";
                            break;

                        case 245: // Conversion Failed (varchar to int, etc.)
                            errorMsg = "❌ Data Type Mismatch!\n\n" +
                                      "The selected values contain invalid data types.\n" +
                                      "Please check ComboBox bindings.";
                            break;

                        case 220: // Arithmetic overflow
                            errorMsg = "❌ Value Out of Range!\n\n" +
                                      "Supply Type ID must be between 0 and 255.\n" +
                                      $"Selected value: {cmb_ST.SelectedValue}";
                            break;

                        case -1: // Connection timeout
                        case -2: // Connection timeout
                            errorMsg = "❌ Connection Timeout!\n\n" +
                                      "Unable to connect to database.\n" +
                                      "Please check your network connection.";
                            break;

                        default:
                            errorMsg = $"❌ Database Error!\n\n" +
                                      $"Error Code: {sqlEx.Number}\n" +
                                      $"Message: {sqlEx.Message}\n\n" +
                                      "Please contact system administrator.";
                            break;
                    }

                    MessageBox.Show(errorMsg,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (FormatException fex)
                {
                    // ═══════════════════════════════════════════════════════════════
                    // FORMAT ERROR (Invalid data type conversion)
                    // ═══════════════════════════════════════════════════════════════

                    MessageBox.Show($"❌ Invalid Data Format!\n\n" +
                        "ComboBox values must be numeric.\n\n" +
                        $"Details: {fex.Message}\n\n" +
                        "Please check ComboBox bindings:\n" +
                        "• ValueMember should be set to ID column\n" +
                        "• DisplayMember should be set to Name column",
                        "Format Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (OverflowException oex)
                {
                    // ═══════════════════════════════════════════════════════════════
                    // OVERFLOW ERROR (Value too large for TINYINT)
                    // ═══════════════════════════════════════════════════════════════

                    MessageBox.Show($"❌ Value Overflow!\n\n" +
                        "Supply Type ID is out of valid range (0-255).\n\n" +
                        $"Current value: {cmb_ST.SelectedValue}\n\n" +
                        "Please check your SupplyType table.",
                        "Overflow Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (InvalidOperationException ioEx)
                {
                    // ═══════════════════════════════════════════════════════════════
                    // CONNECTION ERROR
                    // ═══════════════════════════════════════════════════════════════

                    MessageBox.Show($"❌ Connection Error!\n\n" +
                        "Unable to establish database connection.\n\n" +
                        $"Details: {ioEx.Message}\n\n" +
                        "Please check:\n" +
                        "• Database server is running\n" +
                        "• Connection string is correct\n" +
                        "• Network connectivity",
                        "Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // ═══════════════════════════════════════════════════════════════
                    // GENERAL ERROR (Unexpected errors)
                    // ═══════════════════════════════════════════════════════════════

                    MessageBox.Show($"❌ Unexpected Error!\n\n" +
                        $"Type: {ex.GetType().Name}\n" +
                        $"Message: {ex.Message}\n\n" +
                        "Please contact system administrator.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            } // using block automatically closes connection
        }


        // ═══════════════════════════════════════════════════════════════════════════════
        // HELPER METHOD: Clear Fields After Successful Insert
        // ═══════════════════════════════════════════════════════════════════════════════

        private void ClearFields1()
        {
            cmb_item.SelectedIndex = -1;
            cmb_Address.SelectedIndex = -1;
            cmb_ST.SelectedIndex = -1;
            num_out.Value = 0;
            txt_Remarks.Clear();
            dt_Supply.Value = DateTime.Now;

            // Focus on first control for next entry
            cmb_item.Focus();
        }


        // ═══════════════════════════════════════════════════════════════════════════════
        // BONUS: COMBOBOX BINDING METHODS (Make sure these are properly set)
        // ═══════════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Load Items ComboBox
        /// </summary>
        private void LoadItems1()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "SELECT ItemId, ItemName FROM StationeryItems ORDER BY ItemName";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmb_item.DataSource = dt;
                    cmb_item.DisplayMember = "ItemName";  // Display
                    cmb_item.ValueMember = "ItemId";      // Value (INT)
                    cmb_item.SelectedIndex = -1;          // No default selection
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items:\n{ex.Message}",
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load Addresses ComboBox
        /// </summary>
        private void LoadAddresses()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "SELECT Id, Name FROM PhilitelicBuearu ORDER BY Name";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmb_Address.DataSource = dt;
                    cmb_Address.DisplayMember = "Name";
                    cmb_Address.ValueMember = "Id";       // INT
                    cmb_Address.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading addresses:\n{ex.Message}",
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load Supply Types ComboBox
        /// IMPORTANT: ValueMember is TINYINT (0-255 range)
        /// </summary>
        private void LoadSupplyTypes()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "SELECT ID, TypeName FROM SupplyType ORDER BY TypeName";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmb_ST.DataSource = dt;
                    cmb_ST.DisplayMember = "TypeName";
                    cmb_ST.ValueMember = "ID";            // TINYINT (0-255)
                    cmb_ST.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supply types:\n{ex.Message}",
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ═══════════════════════════════════════════════════════════════════════════════
        // FORM LOAD EVENT: Initialize everything
        // ═══════════════════════════════════════════════════════════════════════════════

        private void Form_Load(object sender, EventArgs e)
        {
            // Apply theme
            ThemeManager.ApplyTheme(this);

            // Load all ComboBoxes
            //LoadItems();
            LoadAddresses();
            LoadSupplyTypes();

            // Set default date to today
            dt_Supply.Value = DateTime.Now;

            // Focus on first field
            cmb_item.Focus();
        }


        public StationeryTransactionscs()
        {
            InitializeComponent();
        }

        private void StationeryTransactionscs_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);

            AddressComboLoad();
            StationItemComboLoad();
            StationeryGridLoad(null);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            AddStationery();
            StationeryGridLoad(id);
        }


        private void cmb_item_SelectedIndexChanged(object sender, EventArgs e)
{
           
            // ✅ 1. Pehle guard checks karo
            if (isSelectionProcess) return;
            if (cmb_item.SelectedIndex == -1 || cmb_item.SelectedValue == null)
            {
                txt_Balance.Text = "Loading...";
                txt_Balance.ForeColor = Color.White;
                return;
            }
            // ✅ 2. Sirf ek baar value lo
            int itemId = Convert.ToInt32(cmb_item.SelectedValue);
    id = itemId; // ✅ Aik jagah assign
    
    // ✅ 3. Grid load karo
    StationeryGridLoad(itemId);
    
    // ✅ 4. Balance calculate karo
    LoadItemBalance(itemId);
}

// ✅ Balance logic alag method mein
private void LoadItemBalance(int itemId)
{
            

                try
                {
                    // ✅ UI disable karo during load
                    //cmb_item.Enabled = false;

                    int balanceQty = CalculateBalance(itemId);

                    // ✅ Ek jagah set karo - clean logic
                    txt_Balance.Text = balanceQty > 0
                        ? balanceQty.ToString()
                        : "0";

                    // ✅ Visual warning agar balance low hai
                    txt_Balance.ForeColor = balanceQty <= 0
                        ? Color.Red
                        : balanceQty < 10
                            ? Color.Orange
                            : Color.Green;
                }
                catch (Exception ex)
                {
                    txt_Balance.Text = "Error";
                    MessageBox.Show($"Balance Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // ✅ Hamesha re-enable karo
                    cmb_item.Enabled = true;
                }
            }


// ✅ Pure calculation method (no UI code)
private int CalculateBalance(int itemId)
{
           
            int balanceQty = 0;
    
    using (SqlConnection con = new SqlConnection(Db.ConString))
    {
        // ✅ Better query - SUM directly in SQL (faster!)
        string query = @"
            SELECT 
                SUM(CASE WHEN SupplyTypeId = 3 THEN Qty ELSE 0 END) AS StockIn,
                SUM(CASE WHEN SupplyTypeId = 4 THEN Qty ELSE 0 END) AS StockOut
            FROM StationeryTransactions 
            WHERE ItemId = @itemId";
        
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = itemId;
        con.Open();
        
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                int stockIn  = reader["StockIn"]  == DBNull.Value ? 0 : Convert.ToInt32(reader["StockIn"]);
                int stockOut = reader["StockOut"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StockOut"]);
                balanceQty = stockIn - stockOut;
            }
        }
    }
    
    return balanceQty;
}
        private void cmb_item_MouseClick(object sender, MouseEventArgs e)
        {

            // Jab user window band karke wapas aaye, toh list dobara load karein!
            StationItemComboLoad();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check karein ke click header par na ho balkay row par ho
            if (e.RowIndex >= 0)
            {
                btn_Add.Enabled = false;
                cmb_item.Enabled = false;
                btn_Assign.Enabled = true;
                try
                {
                    // 1. Flag ON karein: Taake ComboBoxes ka event Grid ko refresh na kare
                    isSelectionProcess = true;

                    btn_Add.Enabled = false;
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // 2. ID ko class variable mein save karein (Update/Delete ke liye)
                    selectedTransId = Convert.ToInt32(row.Cells["TransId"].Value);

                    // 3. Date aur Quantity
                    dt_Supply.Value = Convert.ToDateTime(row.Cells["TransDate"].Value);
                    num_out.Value = Convert.ToDecimal(row.Cells["Qty"].Value);

                    // 4. ComboBoxes (SelectedValue ke zariye)
                    // Note: In columns ke naam wahi honay chahiye jo aapki SQL query mein hain
                    cmb_item.SelectedValue = row.Cells["ItemID"].Value;
                    cmb_ST.SelectedValue = row.Cells["STID"].Value;

                    if (row.Cells["AddressID"].Value != DBNull.Value)
                    {
                        cmb_Address.SelectedValue = row.Cells["AddressID"].Value;
                    }
                    else
                    {
                        cmb_Address.SelectedIndex = -1;
                    }

                    // 5. Remarks (Null handling)
                    txt_Remarks.Text = row.Cells["Remarks"].Value != DBNull.Value
                                       ? row.Cells["Remarks"].Value.ToString()
                                       : "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Selection Error: " + ex.Message);
                }
                finally
                {
                    // 6. Flag OFF karein: Taake normal filtering dobara shuru ho sakay
                    isSelectionProcess = false;
                }
            }
        }

        private void btn_Assign_Click(object sender, EventArgs e)
        {
            updateStationery();
            if (btn_Add.Enabled == false && cmb_item.Enabled == false && btn_Assign.Enabled == true)
            {
                btn_Assign.Enabled = true;
                btn_Add.Enabled = true;
                cmb_item.Enabled = true;
                cmb_item.SelectedIndex = -1;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Validation
            if (cmb_item.SelectedIndex == -1 || cmb_item.SelectedValue == null)
            {
                MessageBox.Show("Select File no first!");
                cmb_item.DroppedDown = true;
                cmb_item.Focus();
                return;
            }

            int id = (int)cmb_item.SelectedValue;



            // 3. Report Processing
            if (dataGridView1.DataSource != null)
            {
                DataTable dt = ((DataTable)dataGridView1.DataSource).Copy();

                // Parameters ki list - Check karein ke RDLC mein exact yahi naam hon
                List<ReportParameter> reportParams = new List<ReportParameter>
        {
            new ReportParameter("itemBal", txt_Balance.Text ?? "0"),

        };

                if (dt.Rows.Count > 0)
                {
                    frmReportView reportForm = new frmReportView();
                    // Path hamesha sahi check karein
                    string rpath = Path.Combine(Application.StartupPath, "Report", "rptStationery.rdlc");

                    // LoadReport ko Parameters list bhej rahe hain
                    reportForm.LoadReport(dt, "dtStationery", rpath, reportParams);
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Grid mein koi data mojud nahi hai.");
                }
            }
            else
            {
                MessageBox.Show("Pehle search karein taake data grid mein aa jaye.");
            }

        }
    }
}

