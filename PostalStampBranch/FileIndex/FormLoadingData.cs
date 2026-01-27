using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndex
{
    internal class FormLoadingData
    {
        // Humne (ComboBox cmb) ko bracket mein dala hai taake 
        // jis form se call karein, wo apna ComboBox humein bhej de

        public static void FillIssueNumbers(ComboBox cmb)
        {
            using (var con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = @"SELECT 
                                    F.Id AS FileId,
                                    C.IssueId, 
                                    (C.IssueNo + ' - ' + F.FileSubject) AS IssueDetail
                                 FROM CommStamp C
                                 LEFT JOIN FileIndex F ON F.Id = C.FileNo
                                 ORDER BY C.IssueId DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Ab hum 'cmb' use kar rahe hain jo bahir se aya hai
                    cmb.DataSource = dt;
                    cmb.DisplayMember = "IssueDetail";
                    cmb.ValueMember = "FileId";
                    cmb.DropDownWidth = 1000;
                    cmb.SelectedIndex = -1; // Shuru mein kuch select na ho
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Issues: " + ex.Message);
                }
            }
        }

        public static void SignatureAuthority(ComboBox cmb)
        {
            using (var con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = @"SELECT 
                                    Id, SignatureAuthority
                                    FROM SignatureAuthority
                                    ORDER BY Id ASC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Ab hum 'cmb' use kar rahe hain jo bahir se aya hai
                    cmb.DataSource = dt;
                    cmb.DisplayMember = "SignatureAuthority";
                    cmb.ValueMember = "Id";
                    
                    cmb.SelectedIndex = -1; // Shuru mein kuch select na ho
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Issues: " + ex.Message);
                }
            }
        }



    }
}
