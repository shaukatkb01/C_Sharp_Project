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
    public partial class PendingInvoice : Form
    {
        // 1. Function parameters ko theek kiya (int status aur ComboBox comp)
        public void pendinginvoice(int status, ComboBox comp)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    // 2. Query mein filter lagaya jo 'CommStamp' table mein moojood hon
                    string query = @"SELECT Id, InvoiceNo
                             FROM InvoiceRegister
                             WHERE Acknowledgetyp = @st
                            
                             ORDER BY Id DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    // 3. Status parameter ko @st se jora
                    cmd.Parameters.AddWithValue("@st", status);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // 4. 'comp' variable use kiya jo bahar se aaya hai
                    comp.DataSource = dt;
                    comp.DisplayMember = "InvoiceNo";
                    comp.ValueMember = "Id";
                    comp.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    // '&' ki jagah '+' lagayein
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        public PendingInvoice()
        {
            InitializeComponent();
        }

        private void PendingInvoice_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                //try
                //{
                //    string query = @"SELECT Id,InvoiceNo
                //                 FROM  InvoiceRegister
                //                WHERE Acknowledgetyp=2
                //                ORDER BY Id DESC";
                //    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                //    DataTable dt = new DataTable();
                //    adapter.Fill(dt);
                //    com_InvoiceNo.DataSource = dt;
                //    com_InvoiceNo.DisplayMember = "InvoiceNo";
                //    com_InvoiceNo.ValueMember = "id";
                //    com_InvoiceNo.SelectedIndex = -1;
                //}
                //catch (Exception ex) { MessageBox.Show(ex.Message); }

                pendinginvoice(2, com_InvoiceNo);

            }
        }

        private void com_InvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (com_InvoiceNo.SelectedValue == null|| !(com_InvoiceNo.SelectedValue is int)) return;

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = @"SELECT Remarks
                                FROM InvoiceRegister
                                WHERE Id=@in";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@in", com_InvoiceNo.SelectedValue);
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        text_remark.Text = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString():"";
                    }
                    else
                    {
                        text_remark.Text = "";
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message); 
            }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(text_PageNo.Text))
                {
                    
                    MessageBox.Show("Please insert PageNo first");
                    text_PageNo.Focus();
                    return;
                }
                if (datePicker_receiving.Value==null)
                {
                    MessageBox.Show("Please insert receiving date first");
                    datePicker_receiving.Focus();
                    return;
                }
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = @"UPDATE InvoiceRegister 
                                SET Acknowledgetyp=@at, 
                                    AcknowldgeDate=@ad, 
                                    PageNo=@pn,
                                    Remarks=@remark
                                    WHERE Id=@in";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@in", com_InvoiceNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@at", 1);
                    cmd.Parameters.AddWithValue("@ad", datePicker_receiving.Value.Date);
                    cmd.Parameters.AddWithValue("@pn", text_PageNo.Text);
                    cmd.Parameters.AddWithValue("@remark", string.IsNullOrEmpty(text_remark.Text) ? (object)DBNull.Value : text_remark.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();

                    ClearForm.ClearAllControls(this);
                    com_InvoiceNo.Focus();
                    MessageBox.Show("Pending Invoice Acknowldge successfully");
                pendinginvoice(2, com_InvoiceNo);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

