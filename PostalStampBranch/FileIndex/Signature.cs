using Microsoft.Data.SqlClient;
using PostalStampSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace FileIndex
{



    public partial class Signature : Form
    {
        private void loadSignature(DataGridView gridView)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    con.Open(); // Connection open karna achi baat hai
                    string query = "SELECT * FROM SignatureAuthority";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // GridView ko data dena
                    gridView.DataSource = dt;
                } // Connection yahan khud band ho jayega
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public Signature()
        {
            InitializeComponent();
        }

        private void Signature_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            loadSignature(dataGridView1);
            this.WindowState = FormWindowState.Maximized;

        }

        private void Add_Update_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    con.Open();
                    string query = "";

                    if (btn_AddUpdate.Text == "Add")
                    {
                        query = "INSERT INTO SignatureAuthority (SignatureAuthority, Name, Remark) VALUES (@Designation, @Name, @Remark)";
                    }
                    else // Yani button ka text "Update" hai
                    {
                        // Name ki jagah ID use karna hamesha safe hota hai
                        query = "UPDATE SignatureAuthority SET SignatureAuthority=@Designation, Name=@Name, Remark=@Remark WHERE ID=@ID";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Designation", txt_Autority.Text);
                        cmd.Parameters.AddWithValue("@Name", txt_name.Text);
                        cmd.Parameters.AddWithValue("@Remark", txt_Remarks.Text);

                        if (btn_AddUpdate.Text != "Add")
                        {
                            // Yeh ID aapne CellClick ke waqt kisi hidden label ya variable mein save ki hogi
                            cmd.Parameters.AddWithValue("@ID", lbl_HiddenID.Text);
                        }

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(btn_AddUpdate.Text == "Add" ? "Added Successfully!" : "Updated Successfully!");

                    // Grid refresh karein aur fields khali karein
                    loadSignature(dataGridView1);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Ek chota sa helper function fields saaf karne ke liye
        private void ClearFields()
        {
            txt_Autority.Clear();
            txt_name.Clear();
            txt_Remarks.Clear();
            btn_AddUpdate.Text = "Add"; // Wapas "Add" mode mein le aayein
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_AddUpdate.Text = "Update";
            if (e.RowIndex > 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                lbl_HiddenID.Text = row.Cells["Id"].Value.ToString(); // ID ko hidden label mein store karna
                txt_name.Text = row.Cells["Name"].Value.ToString();
                txt_Autority.Text = row.Cells["SignatureAuthority"].Value.ToString();
                txt_Remarks.Text = row.Cells["Remark"].Value.ToString();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (btn_AddUpdate.Text == "Add")
            {
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    try
                    {
                        con.Open();
                        string query = "DELETE FROM SignatureAuthority WHERE Id=@ID";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", lbl_HiddenID.Text);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Deleted Successfully!");
                        // Grid refresh karein aur fields khali karein
                        loadSignature(dataGridView1);
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Deletion cancelled.");
            }
        }
    }
}