using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIndex
{
    partial class Address : Form
    {
        int selectedRecordId = 0;
        private void ClearFields()
        {
            txt_Name.Clear();           // Ya txt_Name.Text = "";
            txt_Address.Clear();
            txt_City.Clear();

            // ComboBox ko reset karne ke liye Index -1 karein
            cmb_BPS.SelectedIndex = -1;

            // Jo humne global ID save ki thi update ke liye, usay bhi 0 kar dein
            selectedRecordId = 0;

            // Cursor wapas pehle textbox par le jayein
            txt_Name.Focus();
        }
        public void updateAddress(int id, string name, string address, string city, int bps)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
                try
                {
                    string query = @"UPDATE Addresses
                                     SET Name = @name,
                                         Address = @address,
                                         City = @city,
                                         BPS = @bps
                                     WHERE ID = @id";
                    id = selectedRecordId;
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@bps", bps);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Address Updated Successfully!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Proble in Database!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        public void BPS(ComboBox cmb)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
                try
                {
                    string query = @"SELECT BPSID, BPS 
                                FROM BPS
                                ORDER BY BPSID DESC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);
                    cmb.DisplayMember = "BPS";
                    cmb.ValueMember = "BPSID";
                    cmb.DataSource = dt;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Proble in Database!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }
        public void editAddress(int id, string name, string address, string city, int bps)
        {

            //using (SqlConnection con = new SqlConnection(Db.ConString))
            //    try
            //    {
            //        string query = @"SELECT A.ID, A.Name, A.Address, A.City, 
            //                        B.BPS
            //                    FROM Addresses A
            //                    LEFT JOIN BPS B ON A.BPS = B.BPSID
            //                    WHERE A.ID = @id";
            //        SqlCommand cmd = new SqlCommand(query, con);
            //        cmd.Parameters.AddWithValue("@id", id);

            //            txt_Name.Text = name;
            //        txt_Address.Text = address;
            //        txt_City.Text = city;
            //        cmb_BPS.SelectedValue = bps;
            //        btn_delet.Enabled = false;
            //    }
        }
        public void AddressLoadgrid(DataGridView DGV)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
                try
                {
                    string query = @"SELECT A.ID, A.Name, A.Address, A.City, 
                                    B.BPS
                                FROM Addresses A
                                LEFT JOIN BPS B ON A.BPS = B.BPSID
                                ORDER BY A.ID DESC";
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
        public Address()
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





        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (txt_Name.Text != "" && txt_City.Text != "" && txt_Address.Text != "" && cmb_BPS.SelectedIndex != -1)
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                    try
                    {
                        string query = @"INSERT INTO Addresses(Name, Address,City,BPS)
                          VALUES(@name,@address,@city,@bps)";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", txt_Name.Text);
                        cmd.Parameters.AddWithValue("@address", txt_Address.Text);
                        cmd.Parameters.AddWithValue("@city", txt_City.Text);
                        cmd.Parameters.AddWithValue("@bps", cmb_BPS.SelectedValue);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Address Added Successfully!");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Proble in Database!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
            else
            {
                MessageBox.Show("Please Fill all the fields");
                txt_Name.Focus();
            }
        }

        private void Address_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);

            BPS(cmb_BPS);
            AddressLoadgrid(dataGridView1);
            dataGridView1.Columns["Address"].Width = 1000;
            dataGridView1.Columns["Name"].Width = 500;
            dataGridView1.Columns["City"].Width = 500;
        }

        

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int Id = 0;
            if (e.RowIndex >= 0)
            {
            btn_Add.Enabled = false;
                // Selected Row ko aik variable mein save kar lein
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Column ke naam wahi likhen jo aapki Grid mein nazar aa rahe hain
                // Ya phir index number use karein (0, 1, 2...)
                selectedRecordId = Convert.ToInt32(row.Cells["Id"].Value);
                txt_Name.Text = row.Cells["Name"].Value.ToString();
                txt_Address.Text = row.Cells["Address"].Value.ToString();
                txt_City.Text = row.Cells["City"].Value.ToString();

                // ComboBox mein BPS ID set karne ke liye
                if (row.Cells["BPS"].Value != DBNull.Value)
                {
                    cmb_BPS.SelectedValue = row.Cells["BPS"].Value;
                }
            }
        }

        private void btn_Assign_Click(object sender, EventArgs e)
        {
            updateAddress(selectedRecordId, txt_Name.Text, txt_Address.Text, txt_City.Text, Convert.ToInt32(cmb_BPS.SelectedValue));
            AddressLoadgrid(dataGridView1);
            ClearFields();
            if (btn_Add.Enabled==false)
            {
                btn_Add.Enabled = true;
            }

        }

        
    }
}
