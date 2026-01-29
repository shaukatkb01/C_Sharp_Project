using Microsoft.Data.SqlClient;
using System.Data;

namespace FileIndex
{
    public partial class InvoiceAcknowlodge : Form
    {
        bool isSelectionProcess = false;
        bool isDateChanged = false;
        // Humne "DataGridView" ko parameter bana diya taake ye dynamic ho jaye
        private void LoadDataToGrid(DataGridView targetGrid, int acknowledgeType)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // Query ko bhi dynamic banaya taake aap 1, 2 ya koi bhi type load kar sakein
                string query = @"SELECT 
                    I.Id, 
                    F.FileNo, 
                    I.IssueDate,
                    I.InvoiceNo, 
                    PB.PhilitelicBuearuName,    -- PB use kiya
                    FORMAT(I.Totalamount, 'N2') AS Totalamount, 
                    A.AcknowledgeType, 
                    PI.ReceivingDate,          -- PI use kiya
                    PI.PageNoInFile,           -- PI use kiya
                    PI.Remarks
                FROM InvoiceRegister I
                LEFT JOIN FileIndex F ON I.FileNo = F.Id
                LEFT JOIN PhilitelicBuearu PB ON I.PhiliticBureauName = PB.Id
                LEFT JOIN PendingInvoice PI ON I.Id = PI.InvoiceRegisterId
                LEFT JOIN AcknowldeType A ON PI.AcknowledgeStatus = A.ID
                WHERE A.ID=@type
                ORDER BY I.Id DESC";

                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@type", acknowledgeType);

                    SqlDataAdapter adapter = new(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Jo grid humne pass ki thi, usay data de rahay hain
                    targetGrid.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading grid: " + ex.Message);
                }
            }
        }

        private void InvoiceToComboBox(ComboBox targitcombo, int status)
        {
            using (SqlConnection con = new(Db.ConString))
            {
                string query = @"SELECT 
                 I.Id, 
                 I.InvoiceNo,
                 PI.InvoiceRegisterID,
                 PI.AcknowledgeStatus
                 FROM InvoiceRegister I
                 INNER JOIN PendingInvoice PI ON PI.InvoiceRegisterID = I.Id
                 WHERE PI.AcknowledgeStatus = @status
                 ORDER BY I.Id DESC"; // Alias (I.) lazmi lagayein
                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@status", status);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        targitcombo.DataSource = dt;
                        targitcombo.DisplayMember = "InvoiceNo";
                        targitcombo.ValueMember = "Id";
                        targitcombo.SelectedIndex = -1;
                    }
                    else
                    {
                        targitcombo.DataSource = null; // Purana data saaf karein
                    } // No selection by default
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading combo box: " + ex.Message);
                }
            }

        }
        public int GetInvoiceType() // Function ka naam sahi tareeqe se likhein
        {
            int a = 0; // Variable ko pehle define karein

            if (radio_Accepted.Checked)
            {
                // Agar Accepted wala radio button select hai
                a = 1;
            }
            else if (radio_Pending.Checked)
            {
                // Agar Pending wala select hai
                a = 2;
            }
            else
            {
                // Agar koi bhi select nahi (Safety ke liye)
                a = 0;
            }

            return a; // Yeh line value ko wapas bhejti hai
        }

        public InvoiceAcknowlodge()
        {
            InitializeComponent();
        }
        private void InvoiceAcknowlodge_Load(object sender, EventArgs e)
        {

            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);
            btn_update.Enabled = false;
            LoadDataToGrid(dataGridView1, GetInvoiceType()); // Load data for Acknowledge Type 1
            InvoiceToComboBox(cmb_InvoiceNo, 2); // Load invoices with Acknowledge Type 1

        }



        private void cmb_InvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. DataRowView error aur null se bachne ke liye check
            if (cmb_InvoiceNo.SelectedValue == null || cmb_InvoiceNo.SelectedValue is DataRowView)
            {
                return;
            }

            int selectedInvoiceId = Convert.ToInt32(cmb_InvoiceNo.SelectedValue);

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // Table se columns uthane ke liye query
                string query = "SELECT ReceivingDate, PageNoInFile, Remarks FROM PendingInvoice WHERE InvoiceRegisterId = @id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", selectedInvoiceId);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // Agar record mil jaye
                    {
                        // --- 1. ReceivingDate Handle Karein ---
                        if (reader["ReceivingDate"] != DBNull.Value)
                        {
                            Picker_RDate.Value = Convert.ToDateTime(reader["ReceivingDate"]);
                        }
                        else
                        {
                            Picker_RDate.Value = DateTime.Now; // Null ho to aaj ki date
                        }

                        // --- 2. PageNo Handle Karein ---
                        txt_PageNo.Text = reader["PageNoInFile"] != DBNull.Value ? reader["PageNoInFile"].ToString() : "";

                        // --- 3. Remarks Handle Karein ---
                        txt_Remark.Text = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : "";
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading details: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmb_InvoiceNo.SelectedIndex == -1)
            {

                MessageBox.Show("Select Invoice No first  to update status", "InvoiceNo Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cmb_InvoiceNo.Focus();
                cmb_InvoiceNo.DroppedDown = true;
                return;
            }
            // for update acknowldge date and page no
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // Pehle hi check kar len ke kya date cheri gayi hai
                if (!isDateChanged)
                {
                    MessageBox.Show("Please select a receiving date first.", "Date Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Code yahan ruk jayega
                }

                // Confirmation Message
                DialogResult result = MessageBox.Show("Are you sure you want to acknowledge this Invoice?", "Confirm Acknowledge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = @"UPDATE PendingInvoice
                         SET ReceivingDate = @ackdate,
                             PageNoInFile = @pageno,
                             AcknowledgeStatus = @atype,
                             Remarks = @remark
                         WHERE InvoiceRegisterId = @id";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Parameters
                    cmd.Parameters.AddWithValue("@id", cmb_InvoiceNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@ackdate", Picker_RDate.Value.Date);
                    cmd.Parameters.AddWithValue("@pageno", string.IsNullOrWhiteSpace(txt_PageNo.Text) ? (object)DBNull.Value : txt_PageNo.Text);
                    cmd.Parameters.AddWithValue("@atype", 1); // 1 = Accepted/Acknowledged
                    cmd.Parameters.AddWithValue("@remark", string.IsNullOrWhiteSpace(txt_Remark.Text) ? (object)DBNull.Value : txt_Remark.Text);

                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Invoice Acknowledged successfully!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh UI
                            LoadDataToGrid(dataGridView1, 1);
                            InvoiceToComboBox(cmb_InvoiceNo, 1);

                            // Reset flag for next entry
                            isDateChanged = false;
                        }
                        else
                        {
                            MessageBox.Show("No record found to update.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating invoice: " + ex.Message);
                    }
                }
            }

        }

        private void Picker_RDate_ValueChanged(object sender, EventArgs e)
        {
            isDateChanged = true;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            // 1. Check karein ke koi row select hui bhi hai ya nahi
            if (cmb_InvoiceNo.SelectedValue == null)
            {
                MessageBox.Show("Pehle list se koi record select karein!");
                return;
            }
            if (radio_Pending.Checked == true && string.IsNullOrWhiteSpace(txt_PageNo.Text))
            {
                MessageBox.Show("must insert Page number"); return;
            }

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // SQL Query ko theek kiya gaya hai (Brackets hata diye hain aur commas lagaye hain)
                string query = @"UPDATE PendingInvoice 
                         SET ReceivingDate = @rd, 
                             PageNoInFile = @pg, 
                             AcknowledgeStatus=@as,
                             Remarks = @rmk 
                         WHERE InvoiceRegisterId = @id"; // Id ki jagah aksar InvoiceRegisterId use hota hai aapke case mein

                SqlCommand cmd = new SqlCommand(query, con);

                // Parameters add karein
                cmd.Parameters.AddWithValue("@rd", Picker_RDate.Value.Date);
                cmd.Parameters.AddWithValue("@pg", string.IsNullOrWhiteSpace(txt_PageNo.Text) ? (object)DBNull.Value : txt_PageNo.Text);
                cmd.Parameters.AddWithValue("@rmk", string.IsNullOrWhiteSpace(txt_Remark.Text) ? (object)DBNull.Value : txt_Remark.Text);
                cmd.Parameters.AddWithValue("@id", cmb_InvoiceNo.SelectedValue);

                if (radio_Accepted.Checked||string.IsNullOrWhiteSpace(txt_PageNo.Text))
                {
                cmd.Parameters.AddWithValue("@as" ,2);
                    InvoiceToComboBox(cmb_InvoiceNo, 2);
                }else if (radio_Accepted.Checked||!string.IsNullOrWhiteSpace(txt_PageNo.Text))
                {
                    cmd.Parameters.AddWithValue("@as", 1);
                    InvoiceToComboBox(cmb_InvoiceNo, 1);
                }
                else if (radio_Pending.Checked)
                {
                    cmd.Parameters.AddWithValue("@as", 1);
                    InvoiceToComboBox(cmb_InvoiceNo, 1);
                }
                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Record Update ho gaya!");
                        groupBox1.Enabled = true;

                            // Refresh Grid taake tabdeeli nazar aaye
                            LoadDataToGrid(dataGridView1, 1);
                        txt_PageNo.Text = "";
                        txt_Remark.Text = "";
                            // Form ko wapas normal halat mein layein
                            button1.Enabled = true;
                            cmb_InvoiceNo.Enabled = true;
                            isDateChanged = false; // Flag reset
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Update Error: " + ex.Message);
                    }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (radio_Accepted.Checked)

            {
                InvoiceToComboBox(cmb_InvoiceNo, 1);
                groupBox1.Enabled = false;
            }
            if (e.RowIndex >= 0)
            {
                try
                {
                    isSelectionProcess = true; // Flag ON

                    button1.Enabled = false;
                    cmb_InvoiceNo.Enabled = false;
                    btn_update.Enabled = true;

                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // 1. ID handle karein
                    int selectedTransId = Convert.ToInt32(row.Cells["Id"].Value);
                    cmb_InvoiceNo.SelectedValue = selectedTransId;

                    // 2. Date handle karein (Check for NULL)
                    if (row.Cells["ReceivingDate"].Value != DBNull.Value && row.Cells["ReceivingDate"].Value != null)
                    {
                        Picker_RDate.Value = Convert.ToDateTime(row.Cells["ReceivingDate"].Value);
                    }
                    else
                    {
                        Picker_RDate.Value = DateTime.Now; // Ya koi default date
                    }

                    // 3. TextBox Handle karein (Sahi tareeqa)
                    txt_PageNo.Text = row.Cells["PageNoInFile"].Value?.ToString() ?? "";

                    txt_Remark.Text = row.Cells["Remarks"].Value != DBNull.Value
                                       ? row.Cells["Remarks"].Value.ToString()
                                       : "";

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Selection Error: " + ex.Message);
                }
                finally
                {
                    isSelectionProcess = false; // Flag OFF
                }
            }
        }

        private void radio_Pending_CheckedChanged(object sender, EventArgs e)
        {
            LoadDataToGrid(dataGridView1, GetInvoiceType());
            InvoiceToComboBox(cmb_InvoiceNo, 2);
        }

        private void radio_Accepted_CheckedChanged(object sender, EventArgs e)
        {
            LoadDataToGrid(dataGridView1, GetInvoiceType());
            InvoiceToComboBox(cmb_InvoiceNo, 1);
        }
    }
}


