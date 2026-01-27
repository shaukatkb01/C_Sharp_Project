using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    cmd.Parameters.AddWithValue("@remark",string.IsNullOrWhiteSpace(txt_Remarks.Text) ? (object)DBNull.Value : txt_Remarks.Text.Trim()); 
                    
                        cmd.ExecuteNonQuery();
                    
                    selectedTransId = 0;
                    MessageBox.Show("Update Seccesfully");
                    ClearFields();
                }
                catch (Exception ex) {
                    MessageBox.Show("error in update" + ex.Message);
                }
            }
        }
        public void StationeryGridLoad(int SelectedId)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = @"SELECT 
    S.TransId,
    I.ItemName,
    A.Address,
    S.TransDate,
    S.ItemId AS ItemID,       -- Taake row.Cells[""ItemID""] mil jaye
    S.Qty,
    S.AddressId AS AddressID, -- Taake row.Cells[""AddressID""] mil jaye
    S.SupplyTypeID AS STID,   -- Taake row.Cells[""STID""] mil jaye
    T.SupplyType,
    S.Remarks
 FROM StationeryTransactions S
 LEFT JOIN StationeryItems I ON S.ItemId = I.ItemID
 LEFT JOIN Addresses A ON S.AddressId = A.Id
 LEFT JOIN SupplyType T ON S.SupplyTypeID = T.ID"; // Filtered Query



                    // 1. Adapter banayein
                    SqlDataAdapter da = new SqlDataAdapter(query, con);

                    // 2. Parameter ko seedha Adapter ke Command mein add karein
                    // Humne function ke bracket wala 'SelectedId' use kiya hai
                    da.SelectCommand.Parameters.AddWithValue("@id", SelectedId);

                    DataTable dt = new DataTable();
                    da.Fill(dt); // Adapter khud hi connection open aur close kar dega

                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns["TransId"].Visible= false;
                    dataGridView1.Columns["ItemID"].Visible = false;
                    dataGridView1.Columns["AddressID"].Visible = false;// adress
                    dataGridView1.Columns["STID"].Visible = false;// supplytype
                    dataGridView1.Columns["ItemName"].Width = 200;
                    dataGridView1.Columns["Address"].Width = 800;
                    dataGridView1.Columns["Remarks"].Width = 150;

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
                    string query = "SELECT Id, (ISNULL(Name, 'No Name') + ' - ' + ISNULL(Address, 'No Address')) AS Addressdetail FROM Addresses;";
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

        public void AddStationery()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                if (num_out.Value == 0)
                {
                    MessageBox.Show("Please enter Quantity In or Out!");
                    return;
                }

                if (cmb_item.SelectedIndex == -1 && cmb_Address.SelectedIndex == -1 && cmb_ST.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select Item and Address first!");
                    return;

                }
                try
                {
                    con.Open();
                    string query = @"INSERT INTO StationeryTransactions 
                (ItemId,TransDate,AddressId,SupplyTypeId,Qty,Remarks)
                 VALUES(@itemId,@dt,@ad,@stid,@qty,@remark)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@itemId", cmb_item.SelectedValue);
                    cmd.Parameters.AddWithValue("@dt", dt_Supply.Value.Date);
                    cmd.Parameters.AddWithValue("@ad", cmb_Address.SelectedValue);
                    cmd.Parameters.AddWithValue("@stid", cmb_ST.SelectedValue);
                    cmd.Parameters.AddWithValue("@qty", num_out.Value);
                    cmd.Parameters.AddWithValue("@remark", string.IsNullOrWhiteSpace(txt_Remarks.Text)
                        ? (object)DBNull.Value
                        : txt_Remarks.Text.Trim());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Stationery supply Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();

                }


                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        public StationeryTransactionscs()
        {
            InitializeComponent();
        }

        private void StationeryTransactionscs_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);

            AddressComboLoad();
            StationItemComboLoad();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            AddStationery();
            StationeryGridLoad(id);
        }

        private void cmb_item_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Agar Grid par click ki wajah se ye event chala hai, toh kuch na karein
            if (isSelectionProcess) return;

            if (cmb_item.SelectedIndex != -1 && cmb_item.SelectedValue != null)
            {
                // SelectedValue ko int mein convert karke bhejein
                id = Convert.ToInt32(cmb_item.SelectedValue);
                StationeryGridLoad(id);
            }
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
            if (btn_Add.Enabled==false&&cmb_item.Enabled==false&&btn_Assign.Enabled==true)
            {
                btn_Assign.Enabled = true;
                btn_Add.Enabled = true;
                cmb_item.Enabled = true;
                cmb_item.SelectedIndex = -1;
            }


        }
    }
}
