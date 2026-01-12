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
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace FileIndex
{
    public partial class PhilatelicSupplyDetail : Form
    {
        public PhilatelicSupplyDetail()
        {
            InitializeComponent();
        }

        private void PhilatelicSupplyDetail_Load(object sender, EventArgs e)
        {
            UIHelper.SetFormTheme(this);
            UIHelper.ApplyTheme(this);

            using var con = new SqlConnection(Db.ConString);

            try
            {
                string query = @"SELECT 
                               FI.Id, 
                               FI.FileNo AS IndexFileNo, 
                               CS.FileNo AS StampFileNo
                               FROM FileIndex FI
                               INNER JOIN CommStamp CS ON CS.FileNo=FI.Id
                                ORDER BY Id DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                com_FileNo.DataSource = dt;
                com_FileNo.DisplayMember = "IndexFileNo";
                com_FileNo.ValueMember = "Id";
                com_FileNo.SelectedIndex = -1;
                string query1 = @"SELECT ID, PhilitelicBuearuName 
                                FROM PhilitelicBuearu 
                                ORDER BY PhilitelicBuearuName ASC";

                SqlDataAdapter adapter1 = new SqlDataAdapter(query1, con);
                DataTable dt1 = new DataTable();
                adapter1.Fill(dt1);
                com_Address.DataSource = dt1;
                com_Address.DisplayMember = "PhilitelicBuearuName";
                com_Address.ValueMember = "ID";
                com_Address.SelectedIndex = -1;

                string query2 = @"SELECT ID, SupplyType 
                                FROM SupplyType 
                                ORDER BY SupplyType ASC";
                SqlDataAdapter adapter2 = new SqlDataAdapter(query2, con);
                DataTable dt2 = new DataTable();
                adapter2.Fill(dt2);
                com_ST.DataSource = dt2;
                com_ST.DisplayMember = "SupplyType";
                com_ST.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void FormatGrid()
        {
            if (dataGridView1.Columns.Count > 0)
            {
                // 1. Hide Hidden IDs (Jo query mein AS ke sath hain wahi use karein)
                if (dataGridView1.Columns.Contains("Id")) dataGridView1.Columns["Id"].Visible = false;
                if (dataGridView1.Columns.Contains("Address")) dataGridView1.Columns["Address"].Visible = false;
                if (dataGridView1.Columns.Contains("ST_ID")) dataGridView1.Columns["ST_ID"].Visible = false;

                // 2. Headers (Naye Names use karein jo Query mein 'AS' ke baad hain)
                if (dataGridView1.Columns.Contains("Bureau Name"))
                    dataGridView1.Columns["Bureau Name"].HeaderText = "Bureau Address";

                if (dataGridView1.Columns.Contains("Type"))
                    dataGridView1.Columns["Type"].HeaderText = "Supply Type";

                // 3. Formatting
                if (dataGridView1.Columns.Contains("Supply_Date"))
                    dataGridView1.Columns["Supply_Date"].DefaultCellStyle.Format = "dd-MM-yyyy";

                // 4. Auto Size
                var cols = dataGridView1.Columns;
                if (cols.Contains("Bureau Address")) cols["Bureau Address"].Width = 1500;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void com_FileNo_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (!(com_FileNo.SelectedValue is int))
            {
                return;
            }

            int id = (int)com_FileNo.SelectedValue;

            StockManager.CalculateAndDisplayStock(id, text_Stamp_B, text_FDC_B, text_Leaflet_B, text_FDCC_B, text_PM_B);
            try
            {
                using var con = new SqlConnection(Db.ConString);

                string query = @"SELECT 
                    F.FileNo AS [File Number],
                    A.PhilitelicBuearuName AS [Bureau Name], -- Ye naam nazar aayega
                    S.SupplyType AS [Type],
                    P.Id,           -- Database row ID (Zaroori)
                    P.Address,      -- Bureau ki ID (Hidden rakhenge)
                    P.SupplyType AS [ST_ID], -- Supply type ki ID (Hidden)
                    P.Supply_Date,
                    P.StampsQty,
                    P.FDCQty,
                    P.LeafletQty,
                    P.FDCCQty,
                    P.PostmarkQty,
                    P.Remark
                 FROM PhilatelicSupply P
                 LEFT JOIN FileIndex F ON P.FileNo = F.Id
                 LEFT JOIN PhilitelicBuearu A ON P.Address = A.Id
                 LEFT JOIN SupplyType S ON P.SupplyType = S.ID
                 WHERE P.FileNo = @fn";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@fn", com_FileNo.SelectedValue);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                FormatGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        // Ye chota sa helper function crash se bachayega
        private decimal GetDecimal(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            return Convert.ToDecimal(value);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            num_FDC.Value = 0;
            num_Leaflet.Value = 0;
            num_FDCC.Value = 0;
            num_PM.Value = 0;
            num_Stamp.Value = 0;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // 1. Numeric Values (GetDecimal helper use kiya hai, perfect!)
                num_Stamp.Value = GetDecimal(row.Cells["StampsQty"].Value);
                num_FDC.Value = GetDecimal(row.Cells["FDCQty"].Value);
                num_Leaflet.Value = GetDecimal(row.Cells["LeafletQty"].Value);
                num_FDCC.Value = GetDecimal(row.Cells["FDCCQty"].Value);
                num_PM.Value = GetDecimal(row.Cells["PostmarkQty"].Value);

                // 2. ComboBoxes (Check karein ke column names query se match kar rahe hain)
                // Agar query mein 'P.Address' ko alias nahi diya to 'Address' hi rahega
                com_Address.SelectedValue = row.Cells["Address"].Value;

                // AGAR aapne query mein 'P.SupplyType AS [ST_ID]' likha hai, to yahan 'ST_ID' likhein
                if (dataGridView1.Columns.Contains("ST_ID"))
                {
                    com_ST.SelectedValue = row.Cells["ST_ID"].Value;
                }

                // 3. Remark aur ID
                text_remark.Text = row.Cells["Remark"].Value?.ToString() ?? "";
                lbl_SelectedID.Text = row.Cells["Id"].Value?.ToString() ?? "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using var con = new SqlConnection(Db.ConString);

            string query = @"UPDATE PhilatelicSupply
                             SET Address = @address,
                                 SupplyType = @stype,
                                 Supply_Date = @sdate,
                                 StampsQty = @stamps,
                                 FDCQty = @fdc,
                                 LeafletQty = @leaflet,
                                 FDCCQty = @fdcc,
                                 PostmarkQty = @pm,
                                 Remark = @remark
                             WHERE Id = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id",Convert.ToInt32( lbl_SelectedID.Text));
            cmd.Parameters.AddWithValue("@address", com_Address.SelectedValue);
            cmd.Parameters.AddWithValue("@stype", com_ST.SelectedValue);
            cmd.Parameters.AddWithValue("@Sdate", date_Supply.Value.Date);
            cmd.Parameters.AddWithValue("@stamps", num_Stamp.Value);
            cmd.Parameters.AddWithValue("@fdc", num_FDC.Value);
            cmd.Parameters.AddWithValue("@Leaflet", num_Leaflet.Value);
            cmd.Parameters.AddWithValue("@fdcc", num_FDCC.Value);
            cmd.Parameters.AddWithValue("@pm", num_PM.Value);
            cmd.Parameters.AddWithValue("@remark", string.IsNullOrWhiteSpace(text_remark.Text)?(Object)DBNull.Value:text_remark.Text);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                ClearForm.ClearAllControls(this);
                
                MessageBox.Show("Philatelic Supply update suecessfully","Successfull",MessageBoxButtons.OK);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

