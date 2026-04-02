using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using PostalStampSystem;

namespace FileIndex
{
    public partial class DispatchType : Form
    {
        private void Clear(object sender, EventArgs e)
        {
            txt_distype.Clear();
            txt_Remarks.Clear();
            btn_AddUpdate.Text = "Add";
            lbl_HiddenID.Text = ""; // Clear the hidden ID label
        }

        private void loadDistype(DataGridView gridView)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    con.Open(); // Connection open karna achi baat hai
                    string query = "SELECT [ID], [DispatchType], [Remarks] FROM dbo.DispatchType";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // GridView ko data dena
                    //gridView.AutoGenerateColumns = true;
                    //gridView.DataSource = null;
                    //gridView.Columns.Clear(); // Purane saare columns khatam karein
                    //gridView.DefaultCellStyle.ForeColor = Color.Black;
                    //gridView.DefaultCellStyle.BackColor = Color.White;
                    gridView.DataSource = dt;
                } // Connection yahan khud band ho jayega
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public DispatchType()
        {
            InitializeComponent();
        }

        private void btn_AddUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_distype.Text))
            {
                MessageBox.Show("Please enter a dispatch type.");
                return;
            }
            if (btn_AddUpdate.Text == "Add")
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    try
                    {
                        con.Open();
                        string query = "INSERT INTO DispatchType (DispatchType, Remarks) VALUES (@Type, @remark)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Type", txt_distype.Text);
                            cmd.Parameters.AddWithValue("@remark", txt_Remarks.Text);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Dispatch type added successfully!");
                        loadDistype(dataGridView1);
                        Clear(sender, e); // Clear the form after adding
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

            }
            else
            {
                // Update logic here

                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    try
                    {
                        con.Open();
                        string query = "UPDATE DispatchType SET DispatchType = @Type, Remarks = @remark WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Type", txt_distype.Text);
                            cmd.Parameters.AddWithValue("@remark", txt_Remarks.Text);
                            cmd.Parameters.AddWithValue("@Id", lbl_HiddenID.Text); // Assuming lbl_Id contains the ID of the record to update
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Dispatch type updated successfully!");
                        loadDistype(dataGridView1);
                        Clear(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void DispatchType_Load(object sender, EventArgs e)
        {
           


            loadDistype(dataGridView1);
            ThemeManager.ApplyTheme(this);
            this.WindowState = FormWindowState.Maximized;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_AddUpdate.Text = "Update";
            if (e.RowIndex > 0) // Ensure that the click is on a valid row
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                lbl_HiddenID.Text = row.Cells["ID"].Value.ToString(); // Assuming "Id" is the name of the ID column
                txt_distype.Text = row.Cells["DispatchType"].Value.ToString(); // Assuming "DispatchType" is the name of the DispatchType column
                txt_Remarks.Text = row.Cells["Remarks"].Value.ToString(); // Assuming "Remarks" is the name of the Remarks column
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (btn_AddUpdate.Text == "Add")
            {
                return; // Clear the form if we are in Update mode
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this dispatch type?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Db.ConString))
                    {
                        con.Open();
                        string query = "DELETE FROM dbo.DispatchType WHERE ID=@id";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@id", lbl_HiddenID.Text); // Assuming lbl_HiddenID contains the ID of the record to delete
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Dispatch type deleted successfully!");
                        loadDistype(dataGridView1);
                        Clear(sender, e); // Clear the form after deletion
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Deletion cancelled.");
            }
        }
    }
}