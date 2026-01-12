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

namespace FileIndex
{
    public partial class IssueMaillist : Form
    {

        private void FillBureauDropdowns()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = "SELECT Id, PhilitelicBuearuName FROM PhilitelicBuearu";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 1; i <= 31; i++)
                {
                    // Rows jinhein fill nahi karna
                    if (i == 2 || i == 21 || i == 22 || i == 23)
                    {
                        continue;
                    }

                    // Dynamic control dhoondna
                    var combo = this.Controls.Find($"drop_{i}_Phil", true).FirstOrDefault() as ComboBox;

                    if (combo != null)
                    {
                        // Independent data source
                        combo.DataSource = dt.Copy();
                        combo.DisplayMember = "PhilitelicBuearuName";
                        combo.ValueMember = "Id";

                        // --- SAFE SELECTION LOGIC ---
                        // Agar aap chahte hain ke Row 1 mein Record 1 ho, Row 5 mein Record 5:
                        int targetIndex = i - 1;

                        if (targetIndex >= 0 && targetIndex < dt.Rows.Count)
                        {
                            combo.SelectedIndex = targetIndex;
                        }
                        else
                        {
                            combo.SelectedIndex = 23; // Khali chor dein agar record nahi hai
                        }
                    }
                }
            }
        }
        private void FillDisTypeDropdowns()
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();

                // 1. Pehle TAMAM Dispatch Types uthain jo dropdown mein dikhani hain
                DataTable dtAllTypes = new DataTable();
                string queryAll = "SELECT ID, DispatchType FROM DispatchType"; // Tamam options
                SqlDataAdapter adapterAll = new SqlDataAdapter(queryAll, con);
                adapterAll.Fill(dtAllTypes);

                for (int i = 1; i <= 20; i++)
                {
                    var combo = this.Controls.Find($"drop_{i}_DisType", true).FirstOrDefault() as ComboBox;
                    var bureauCombo = this.Controls.Find($"drop_{i}_Phil", true).FirstOrDefault() as ComboBox;

                    if (combo != null && bureauCombo?.SelectedValue != null)
                    {
                        // 2. Dropdown ko TAMAM options dein (Copy use karein taake selection alag rahe)
                        combo.DataSource = dtAllTypes.Copy();
                        combo.DisplayMember = "DispatchType";
                        combo.ValueMember = "ID";

                        // 3. Ab check karein ke IssueMaleList mein is Address ke liye kya save hai
                        string querySelected = "SELECT DispatchType FROM IssueMaleList WHERE Address = @add";
                        using (SqlCommand cmd = new SqlCommand(querySelected, con))
                        {
                            cmd.Parameters.AddWithValue("@add", bureauCombo.SelectedValue);

                            // ExecuteScalar use karein kyunke humein sirf aik VALUE chahiye (ID)
                            object selectedID = cmd.ExecuteScalar();

                            if (selectedID != null && selectedID != DBNull.Value)
                            {
                                // Jo ID database mein mili, usay dropdown mein select kar dein
                                combo.SelectedValue = selectedID;
                            }
                            else
                            {
                                combo.SelectedIndex = -1; // Agar kuch nahi mila toh khali
                            }
                        }
                    }
                }
            }
        }

        public IssueMaillist()
        {
            InitializeComponent();
        }

        private void IssueMaillist_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);
            FormLoadingData.FillIssueNumbers(Cmb_IssueNo);

            FillBureauDropdowns(); // Pehle Bureaus bharein
            FillDisTypeDropdowns(); // Phir unke mutabiq Dispatch Types bharein

            

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                con.Open();

                // Loop 1 se 19 tak
                for (int i = 1; i <= 20; i++)
                {
                    // Controls dhoondna
                    var comK = this.Controls.Find($"num_{i}_K", true).FirstOrDefault() as NumericUpDown;
                    var comG = this.Controls.Find($"num_{i}_G", true).FirstOrDefault() as NumericUpDown;
                    var dropPhil = this.Controls.Find($"drop_{i}_Phil", true).FirstOrDefault() as ComboBox;
                    var dropDT = this.Controls.Find($"drop_{i}_DisType", true).FirstOrDefault() as ComboBox;

                    // Check karein ke controls mil gaye hain aur Philately Bureau select hai
                    if (comK != null && comG != null && dropPhil?.SelectedValue != null)
                    {
                        // SQL UPDATE Query (Brackets ke baghair)
                        string query = @"UPDATE IssueMaleList 
                                SET MaleListFileId=@fid,
                                    K = @k, 
                                    G = @g, 
                                    DispatchType = @DT 
                                WHERE Address = @ad 
                                "; // Spelling check: Address

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            // Parameters add karna
                            cmd.Parameters.AddWithValue("@k", (int)comK.Value); // Ya SelectedValue agar ID hai
                            cmd.Parameters.AddWithValue("@g", (int)comG.Value);
                            cmd.Parameters.AddWithValue("@DT", dropDT?.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@ad", dropPhil.SelectedValue);
                            cmd.Parameters.AddWithValue("fid", Cmb_IssueNo.SelectedValue);
                            // Query ko chalana (Iske baghair data save nahi hoga)
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("MaleList Data successfully updated!");
            }
        }
    }
}

       
    

