using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class InvoiceCorection : Form
    {
        public InvoiceCorection()
        {
            InitializeComponent();
        }
        // golobal var for invoiceno
        string selectedInvoiceNo = "";
        int selectedInvoiceId;
        private void InvoiceCorection_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = @"SELECT Id, InvoiceNo
                                FROM InvoiceRegister
                                ORDER BY Id DESC";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);



                DataTable dt = new DataTable();

                adapter.Fill(dt);

                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        com_InvoiceNo.DataSource = dt;
                        com_InvoiceNo.DisplayMember = "InvoiceNo";
                        com_InvoiceNo.ValueMember = "Id";
                        com_InvoiceNo.SelectedIndex = -1;
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Invoice No Data Load Error" + ex.Message);
                }

                string query1 = @"SELECT Id, PhilitelicBuearuName
                                   FROM PhilitelicBuearu";
                SqlDataAdapter adapter1 = new SqlDataAdapter(query1, con);
                DataTable dt1 = new DataTable();
                adapter1.Fill(dt1);
                try
                {
                    if (dt1.Rows.Count > 0)
                    {
                        com_PhilName.DataSource = dt1;
                        com_PhilName.DisplayMember = "PhilitelicBuearuName";
                        com_PhilName.ValueMember = "Id";
                        com_PhilName.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Philatelic Nameo Data Load Error" + ex.Message);
                }
                // for fileNo
                string query2 = @"SELECT Id, FileNo
                                FROM FileIndex";
                SqlDataAdapter adapter2 = new SqlDataAdapter(query2, con);
                DataTable dt2 = new DataTable();
                adapter2.Fill(dt2);
                try
                {
                    if (dt2.Rows.Count > 0)
                    {
                        com_FileNo.DataSource = dt2;
                        com_FileNo.DisplayMember = "FileNo";
                        com_FileNo.ValueMember = "Id";
                        com_FileNo.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FileNO data load Error" + ex.Message);
                }

                // for DispatchType
                string query3 = @"SELECT ID, DispatchType
                                FROM DispatchType";
                SqlDataAdapter adapter3 = new SqlDataAdapter(query3, con);
                DataTable dt3 = new DataTable();
                adapter3.Fill(dt3);
                try
                {
                    if (dt3.Rows.Count > 0)
                    {
                        combo_DT.DataSource = dt3;
                        combo_DT.DisplayMember = "DispatchType";
                        combo_DT.ValueMember = "ID";
                        combo_DT.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DispatchType data load error" + ex.Message);
                }
            }
        }


        private void com_InvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Pehle check karein ke kya data load ho chuka hai?
            // Agar SelectedValue null hai ya abhi load ho raha hai, toh kuch mat karo.
            if (com_InvoiceNo.SelectedValue == null || com_InvoiceNo.SelectedIndex == -1)
            {
                return;
            }

            // 2. Ab check karein ke SelectedValue asli ID hai ya sirf aik "Row Object"
            // Shuru mein jab data bind hota hai, toh kabhi kabhi ye string/object hota hai
            if (com_InvoiceNo.SelectedValue is int || com_InvoiceNo.SelectedValue is long ||
                int.TryParse(com_InvoiceNo.SelectedValue.ToString(), out _))
            {
                // 3. Global variable ko value assign karein
                selectedInvoiceId = Convert.ToInt32(com_InvoiceNo.SelectedValue);


                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    try
                    {
                        con.Open(); // Connection sirf EK BAAR shuru mein kholna hai

                        // --- Pehla Hissa: InvoiceRegister ---


                        string query = @"SELECT 
                                IR.*,                                       -- InoiceRegiter1
                                PS.*,                                       -- PhilatelicSupply
                                P.FDCPrice, P.LeafletPrice, P.PostmarkPrice   -- Price
                                FROM InvoiceRegister IR

                                LEFT JOIN PhilatelicSupply PS ON  IR.Id = PS.InvoiceNo  
                                LEFT JOIN Price P ON IR.FileNo = P.FileNo                 -- Link 2
                                WHERE IR.Id = @id";


                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", selectedInvoiceId);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            com_FileNo.SelectedValue = reader["FileNo"] != DBNull.Value ? reader["FileNo"] : -1;

                            text_TA.Text = Convert.ToDecimal(reader["Totalamount"]).ToString("N2");
                            text_FDCPrice.Text = reader["FDCPrice"].ToString();
                            text_LeafletPrice.Text = reader["LeafletPrice"].ToString();
                            com_PhilName.SelectedValue = reader["PhiliticBureauName"] != DBNull.Value ? reader["PhiliticBureauName"] : -1;
                            text_PMPrice.Text = reader["PostMarkPrice"].ToString();
                            Remark_txt.Text = reader["Remarks"] != DBNull.Value ? Convert.ToString(reader["Remarks"]) : "";
                            date_Picker.Value = reader["Supply_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Supply_Date"]) : DateTime.Now;
                            num_FDC.Value = reader["FDCQty"] != DBNull.Value ? Convert.ToDecimal(reader["FDCQty"]) : 0;
                            num_FDCC.Value = reader["FDCCQty"] != DBNull.Value ? Convert.ToDecimal(reader["FDCCQty"]) : 0;
                            num_Leaflet.Value = reader["LeafletQty"] != DBNull.Value ? Convert.ToDecimal(reader["LeafletQty"]) : 0;
                            num_Stamp.Value = reader["StampsQty"] != DBNull.Value ? Convert.ToDecimal(reader["StampsQty"]) : 0;
                            num_Postmark.Value = reader["PostmarkQty"] != DBNull.Value ? Convert.ToDecimal(reader["PostmarkQty"]) : 0;
                            combo_DT.SelectedValue = reader["DispatchType"] != DBNull.Value ? reader["DispatchType"] : -1;

                            // Baqi controls yahan bharein...
                        }
                        else
                        {
                            MessageBox.Show("Invoice Register Record not found");
                        }
                        reader.Close(); // Pehla reader band, rasta saaf!

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                } // Using khatam hote hi connection khud band ho jayega

            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();

                try
                {
                    string query = @"UPDATE PhilatelicSupply 
                                    SET Address = @ad, Supply_Date= @sd, 
                                    StampsQty = @sq,FDCQty = @fq,LeafletQty = @lq,
                                    FDCCQty = @fcq,
                                    PostmarkQty = @pq, Remark = @remark
                                WHERE InvoiceNo=@in";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@in", com_InvoiceNo.SelectedValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ad", com_PhilName.SelectedValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sd", date_Picker.Value.Date);
                    cmd.Parameters.AddWithValue("@sq", num_Stamp.Value);
                    cmd.Parameters.AddWithValue("@fq", num_FDC.Value);
                    cmd.Parameters.AddWithValue("@lq", num_Leaflet.Value);
                    cmd.Parameters.AddWithValue("@fcq", num_FDCC.Value);
                    cmd.Parameters.AddWithValue("@pq", num_Postmark.Value);
                    cmd.Parameters.AddWithValue("@remark", string.IsNullOrEmpty(Remark_txt.Text) ? (object)DBNull.Value : Remark_txt.Text);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Philatelicsupply data update error:" + ex.Message);
                }

                try
                {
                    string query1 = @"UPDATE InvoiceRegister
                                    SET PhiliticBureauName=@pn, IssueDate=@id, 
                                    Totalamount=@ta, DispatchType=@dt,Remarks=@remarks
                                    WHERE Id=@in";
                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    cmd1.Parameters.AddWithValue("@in", com_InvoiceNo.SelectedValue);
                    cmd1.Parameters.AddWithValue("pn", com_PhilName.SelectedValue ?? DBNull.Value);
                    cmd1.Parameters.AddWithValue("id", date_Picker.Value.Date);
                    cmd1.Parameters.AddWithValue("@ta", Convert.ToDecimal(text_TA.Text));
                    cmd1.Parameters.AddWithValue("@dt", combo_DT.SelectedValue ?? DBNull.Value);
                    cmd1.Parameters.AddWithValue("remarks", string.IsNullOrEmpty(Remark_txt.Text) ? (object)DBNull.Value : Remark_txt.Text);
                    cmd1.ExecuteNonQuery();
                    ClearForm.ClearAllControls(this);
                    MessageBox.Show("Invoice Data sucessfully update");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("InvoiceRegister data update error" + ex.Message);
                }

            }
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
    }
}
