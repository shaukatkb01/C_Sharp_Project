using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIndex
{
    partial class AddStationeryItems : Form
    {
        int selectedRecordId = 0;
        public void ClearFields()
        {
            txt_Item.Clear();
            txt_Remark.Clear();
            dataGridView1.Refresh();
        }

        public void addStationeryItems()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    // Pehle check karein ke kya naam pehle se mojud hai?
                    string checkQuery = "SELECT COUNT(*) FROM StationeryItems WHERE ItemName = @itemname";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@itemname", txt_Item.Text.Trim());

                    con.Open();
                    int count = (int)checkCmd.ExecuteScalar(); // Ye count wapas layega (0 ya 1)

                    if (count > 0)
                    {
                        MessageBox.Show("Ye Item pehle se mojud hai!", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Function yahi ruk jayega, insert nahi karega
                    }

                    // Agar mojud nahi hai, toh insert karein
                    string query = @"INSERT INTO StationeryItems (ItemName, Remarks) VALUES (@itemname, @remarks)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@itemname", txt_Item.Text.Trim());
                    cmd.Parameters.AddWithValue("@remarks", txt_Remark.Text.Trim());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Stationery Item Added Successfully");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        public void updateStationeryItems(int selectedID)
        {
            if (string.IsNullOrWhiteSpace(txt_Item.Text))
            {
                MessageBox.Show("Item Name is required!");
                return;
            }
            using (SqlConnection con = new SqlConnection(Db.ConString))
                try
                {
                    string query = @"UPDATE StationeryItems
                                SET ItemName=@itemname,Remarks=@remarks
                                WHERE ItemId=@itemid";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@itemid", selectedID);
                    cmd.Parameters.AddWithValue("@itemname", txt_Item.Text.Trim());
                    cmd.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(txt_Remark.Text) ? (object)DBNull.Value : txt_Remark.Text.Trim()); 
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Stationery Item Updated Successfully", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Proble in Database!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


        }

        public void StationeryLoadgrid(DataGridView DGV)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
                try
                {
                    string query = @"SELECT ItemId, ItemName,Remarks
                                FROM StationeryItems
                                ORDER BY ItemId ASC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);
                    DGV.DataSource = dt;

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Proble in Database!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        public AddStationeryItems()
        {
            InitializeComponent();

        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void AddStationeryItems_Load(object sender, EventArgs e)
        {
            UIHelper.SetFormTheme(this);
            UIHelper.ApplyTheme(this);

            StationeryLoadgrid(dataGridView1);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            addStationeryItems();
            StationeryLoadgrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_Add.Enabled = false; 
            // Check karein ke row index valid hai (Header click handle karne ke liye)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // 1. ID ko global variable mein save karein (Update ke liye)
                // selectedRecordId wo variable hai jo aapne class ke top par banaya hoga
                selectedRecordId = Convert.ToInt32(row.Cells["ItemId"].Value);

                // 2. Data ko TextBoxes mein load karein
                txt_Item.Text = row.Cells["ItemName"].Value.ToString();

                // Remarks mein NULL handle karte hue load karein
                txt_Remark.Text = row.Cells["Remarks"].Value == DBNull.Value
                                  ? ""
                                  : row.Cells["Remarks"].Value.ToString();
            }
        }

        private void btn_Assign_Click(object sender, EventArgs e)
        {
            updateStationeryItems(selectedRecordId);
            StationeryLoadgrid(dataGridView1);
            btn_Add.Enabled = true;
            selectedRecordId = 0;
        }

       
    }
}
