using Microsoft.Data.SqlClient;
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
    public partial class PhilatelicSupply : Form
    {
        public PhilatelicSupply()
        {
            InitializeComponent();
        }

        private void PhilatelicSupply_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = @" SELECT Id,PhilitelicBuearuName
                              From PhilitelicBuearu
                              ORDER BY Id DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    com_Address.DataSource = dt;
                    com_Address.DisplayMember = "PhilitelicBuearuName";
                    com_Address.ValueMember = "Id";
                    com_Address.SelectedIndex = -1;

                    string query2 = @"SELECT ID, SupplyType
                                      FROM SupplyType
                                      WHERE ID IN (3,4)";
                    SqlDataAdapter adapter1 = new SqlDataAdapter(query2, con);
                    DataTable dt2 = new DataTable();
                    adapter1.Fill(dt2);
                    com_ST.DataSource = dt2;
                    com_ST.DisplayMember = "SupplyType";
                    com_ST.ValueMember = "ID";
                    com_ST.SelectedIndex = -1;


                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                try
                {
                    string query1 = @"SELECT fi.Id, fi.FileNo 
                     FROM FileIndex fi
                     INNER JOIN CommStamp cs ON fi.Id = cs.FileNo
                     ORDER BY Id DESC";
                    //WHERE cs.Id = @stampId";
                    SqlDataAdapter adapter1 = new SqlDataAdapter(query1, con);
                    DataTable dt1 = new DataTable();
                    adapter1.Fill(dt1);
                    com_FileNo.DataSource = dt1;
                    com_FileNo.DisplayMember = "FileNo";
                    com_FileNo.ValueMember = "Id";
                    com_FileNo.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void com_IssueNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = @"SELECT FileNo, StampsQty, FDCQty, 
                                LeafletQty, FDCCQty, PostmarkQty, Remark
                                FROM PhilatelicSupply
                                WHERE FileNo=@fid
                                AND FileType IN(1,3)";

                SqlCommand cmd = new SqlCommand(query, con);

            }
        }

        private void com_FileNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (com_FileNo.SelectedValue != null && com_FileNo.SelectedValue is int)
            {
                int id = (int)com_FileNo.SelectedValue;

                // Alag file wala function call ho raha hai
                // Hum TextBoxes ke naam bhi bhej rahe hain taake wo unhein update kar sake
                StockManager.CalculateAndDisplayStock(id, text_Stamp_B, text_FDC_B, text_Leaflet_B, text_FDCC_B, text_PM_B);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsFormValid(this))
            { return; }

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                
                try
                {
                    string query = @" INSERT INTO PhilatelicSupply
                            (Address, FileNo, SupplyType, Supply_Date, StampsQty, FDCQty, LeafletQty, FDCCQty, PostmarkQty, Remark)
                            VALUES(@ad, @fn,@st,@sd,@sq,@fq,@lq,@fcq,@pm,@remark)";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ad", com_Address.SelectedValue);
                    cmd.Parameters.AddWithValue("@fn", com_FileNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@st", com_ST.SelectedValue);
                    cmd.Parameters.AddWithValue("@sd", date_Supply.Value.Date);
                    cmd.Parameters.AddWithValue("@sq", num_Stamp.Value);
                    cmd.Parameters.AddWithValue("@fq", num_FDC.Value);
                    cmd.Parameters.AddWithValue("@lq", num_Leaflet.Value);
                    cmd.Parameters.AddWithValue("@pm", num_PM.Value);
                    cmd.Parameters.AddWithValue("@fcq",num_FDCC.Value);
                    cmd.Parameters.AddWithValue("@remark", string.IsNullOrWhiteSpace(text_remark.Text) ? (object)DBNull.Value : text_remark.Text);
                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result>0)
                    {
                     MessageBox.Show("Record saved successfully");
                        ClearForm.ClearAllControls(this);
                    }
                    
                   
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
