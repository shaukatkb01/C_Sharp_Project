using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostalStampSystem;

namespace FileIndex
{
    public partial class SingleMaillist : Form
    {
        int rowCounter = 1;
        

        public void ComboLoad(ComboBox? cmb_Phil=null, ComboBox? cmb_DisType = null,  ComboBox? cmb_FileNo=null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    con.Open(); // Connection open karna zaroori hai
                    if (cmb_Phil != null)
                    {
                        // Query 1: Bureau
                        string query = "SELECT Id, Address FROM PhilitelicBuearu";
                        SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cmb_Phil.DataSource = dt;
                        cmb_Phil.DisplayMember = "Address";
                        cmb_Phil.ValueMember = "Id";
                        cmb_Phil.SelectedIndex = -1;
                    }
                    // Query 2: Dispatch Type
                    if (cmb_DisType != null)
                    {
                        string query2 = "SELECT ID, DispatchType FROM DispatchType";
                        SqlDataAdapter adapter2 = new SqlDataAdapter(query2, con);
                        DataTable dt2 = new DataTable();
                        adapter2.Fill(dt2);
                        cmb_DisType.DataSource = dt2;
                        cmb_DisType.DisplayMember = "DispatchType";
                        cmb_DisType.ValueMember = "ID";
                        cmb_DisType.SelectedIndex = -1;
                    }

                    if (cmb_FileNo != null)
                    {
                        string query3 = "SELECT Id, FileNo FROM FileIndex";
                        SqlDataAdapter adapter3 = new SqlDataAdapter(query3, con);
                        DataTable dt3 = new DataTable();
                        adapter3.Fill(dt3);
                        cmb_FileNo.DataSource = dt3;
                        cmb_FileNo.DisplayMember = "FileNo";
                        cmb_FileNo.ValueMember = "Id";
                        cmb_FileNo.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Load Error: " + ex.Message);
            }
        }


        public SingleMaillist()
        {
            InitializeComponent();
        }




        // Ye variable class ke bilkul upar hona chahiye
        //int rowCounter = 1;

        private void btn_ADDRow_Click(object sender, EventArgs e)
        {
            
            // Safety Check: Agar flowLayoutPanel designer mein nahi mila to error nahi ayega
            if (this.flowLayoutPanel1 == null)
            {
                MessageBox.Show("Error: FlowLayoutPanel1 control nahi mila!");
                return;
            }

            int numberOfLines = 1;
            // Debugging ke liye ye message box lagayein
            MessageBox.Show("Total Rows to process: " + (rowCounter - 1).ToString());
            for (int i = 0; i < numberOfLines; i++)
            {
                // 1. Label (Serial Number)
                Label lbl = new Label();
                lbl.Text = rowCounter.ToString() + ".";
                lbl.AutoSize = true;
                lbl.Margin = new Padding(0, 8, 0, 0);

                // 2. First Dropdown (Philatelic Bureau)
                ComboBox dropPhil = new ComboBox();
                dropPhil.Name = "drop_" + rowCounter + "_Phil";
                dropPhil.Width = 400;
                dropPhil.Tag = "Phil";
                dropPhil.DropDownStyle = ComboBoxStyle.DropDownList; // Taake user khud se na likh sakay

                // 3. NumericUpDown (K)
                NumericUpDown numK = new NumericUpDown();
                numK.Name = "num_" + rowCounter + "_K";
                numK.Width = 60;

                // 4. NumericUpDown (G)
                NumericUpDown numG = new NumericUpDown();
                numG.Name = "num_" + rowCounter + "_G";
                numG.Width = 60;

                // 5. Second Dropdown (Dispatch Type)
                ComboBox dropDis = new ComboBox();
                dropDis.Name = "drop_" + rowCounter + "_DisType";
                dropDis.Width = 150;
                dropDis.DropDownStyle = ComboBoxStyle.DropDownList;

                // --- DATABASE SE DATA LOAD KARNA ---
                // Hum aapka banaya hua function call kar rahe hain
                ComboLoad(dropPhil, dropDis, null);

                // --- CONTROLS KO PANEL MEIN ADD KARNA ---
                this.flowLayoutPanel1.Controls.Add(lbl);
                this.flowLayoutPanel1.Controls.Add(dropPhil);
                this.flowLayoutPanel1.Controls.Add(numK);
                this.flowLayoutPanel1.Controls.Add(numG);
                this.flowLayoutPanel1.Controls.Add(dropDis);

                // Nayi line ke liye break lagana
                this.flowLayoutPanel1.SetFlowBreak(dropDis, true);

                rowCounter++;

            }
        }

        private void SingleMaillist_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ComboLoad(null, drop_DisType, Cmb_IssueNo);
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (rowCounter < 1)
            {
                MessageBox.Show("Please add at least one row before saving!");
                btn_ADDRow.BackColor = Color.Red;
                btn_ADDRow.Focus();
                return;
            }
            // 1. Validations
            if (Cmb_IssueNo.SelectedIndex == -1 || drop_DisType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Issue No and Dispatch Type first!");
                return;
            }

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    con.Open();

                    // 2. Table Khali Karna (Truncate)
                    string truncateQuery = "TRUNCATE TABLE SinglMaleList";
                    using (SqlCommand cmdTrunc = new SqlCommand(truncateQuery, con))
                    {
                        cmdTrunc.ExecuteNonQuery();
                    }

                    // 3. Loop chala kar dynamic data INSERT karna
                    // Hum rowCounter tak loop chalayenge (jitni rows banni hain)
                    for (int i = 1; i < rowCounter; i++)
                    {
                        var comK = this.Controls.Find($"num_{i}_K", true).FirstOrDefault() as NumericUpDown;
                        var comG = this.Controls.Find($"num_{i}_G", true).FirstOrDefault() as NumericUpDown;
                        var dropPhil = this.Controls.Find($"drop_{i}_Phil", true).FirstOrDefault() as ComboBox;
                        var dropDT = this.Controls.Find($"drop_{i}_DisType", true).FirstOrDefault() as ComboBox;
                        
                       
                        // Check: Agar row mojud hai aur Bureau select hai
                        if (dropPhil != null && dropPhil.SelectedValue != null)
                        {
                            // Query mein Address column lazmi shamil karein
                            string insertQuery = @"INSERT INTO SinglMaleList 
                                         (MaleListFileId, Address, K, G, DispatchType) 
                                         VALUES (@fid, @ad, @k, @g, @DT)";

                            using (SqlCommand cmdIns = new SqlCommand(insertQuery, con))
                            {
                                cmdIns.Parameters.AddWithValue("@fid", Cmb_IssueNo.SelectedValue);
                                cmdIns.Parameters.AddWithValue("@ad", dropPhil.SelectedValue);
                                cmdIns.Parameters.AddWithValue("@k", (int)comK.Value);
                                cmdIns.Parameters.AddWithValue("@g", (int)comG.Value);
                                cmdIns.Parameters.AddWithValue("@DT", dropDT?.SelectedValue ?? DBNull.Value);

                                cmdIns.ExecuteNonQuery();
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during saving: " + ex.Message);
                    return; // Agar save nahi hua to report na chalayein
                }
            } // Connection yahan close ho jayega

            // 4. Report Load Karna
            LoadReportData();
        }

        // Report ke liye alag function (Safayi ke liye)
        private void LoadReportData()
        {
            int DistypId = Convert.ToInt32(drop_DisType.SelectedValue);

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // SELECT query mein M.Address ki jagah table name 'SinglMaleList' use karein
                string query = @"SELECT 
            M.Id AS M_Id, 
            M.MaleListFileId, 
            M.Address,
            M.K,
            M.G,
            M.DispatchType AS M_DispatchType,
            P.PhilitelicBuearuName AS BureauName,
            D.DispatchType,
            F.FileNo
        FROM SinglMaleList M 
        LEFT JOIN PhilitelicBuearu P ON M.Address = P.Id
        LEFT JOIN DispatchType D ON M.DispatchType = D.ID
        LEFT JOIN FileIndex F ON M.MaleListFileId = F.Id
        WHERE M.DispatchType = @dtId";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@dtId", DistypId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    frmReportView reportForm = new frmReportView();
                    reportForm.LoadReport(dt, "dtISinglMaleList", "Report/rptSingleMailList.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("No data found for report.");
                }
            }
        }
        // Data load karne ka function jo har row ke liye chalega

    }
}

