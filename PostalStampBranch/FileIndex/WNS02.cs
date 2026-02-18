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
    public partial class WNS02 : Form
    {
        public WNS02()
        {
            InitializeComponent();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            string title = "";
            if (ch_Mr.Checked)
            {
                title = "Mr.";
            }
            else if (ch_Mrs.Checked)
            {
                title = "Mrs.";
            }
            else if (ch_Ms.Checked)
            {
                title = "Ms.";
            }

            if (cmb_Signature.SelectedIndex == -1)

            {
                cmb_Signature.Focus();
                cmb_Signature.DroppedDown = true;
                return;
            }
            if (dt_From.Value.Date >= DateTime.Today)
            {
                MessageBox.Show("Select date first");
                return;
            }

            

            using (SqlConnection con = new(Db.ConString))
            {
                string query = @"SELECT
                            C.DateOfIssue,
                            F.FileSubject,
                            P.StampPrice,
                            @Signature AS Signature,
                            @Name AS Name,
                            @title AS Title
                            FROM CommStamp C 
                            INNER JOIN FileIndex F on C.FileNo=F.Id
                            LEFT JOIN StockPrice P ON F.Id= P.FileNo
                            WHERE CAST(C.DateOfIssue AS DATE) >= @fd 
                            AND CAST(C.DateOfIssue AS DATE) <= @td
                            AND F.FileType IN(1,2,3)
                            ORDER BY C.DateOfIssue ASC";


                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@fd", dt_From.Value.Date);
                da.SelectCommand.Parameters.AddWithValue("@td", dt_To.Value.Date);
                da.SelectCommand.Parameters.AddWithValue("@Signature", cmb_Signature.Text);
                da.SelectCommand.Parameters.AddWithValue("@Name", cmb_Signature.SelectedValue);
                da.SelectCommand.Parameters.AddWithValue("@title", title);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Report wala form open karein
                    frmReportView reportForm = new frmReportView();
                    // dt yahan apka datatable hai
                    reportForm.LoadReport(dt, "dtWNS02", "Report/rptWNS02.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("No any Commemorative Stamp Issue found in selected Dates.");
                }
            }


        }

        private void WNS02_Load(object sender, EventArgs e)
        {

            ThemeManager.ApplyTheme(this);
            ch_Mr.Checked = true;
            using (SqlConnection con = new(Db.ConString))
            {
                string query = @"SELECT SignatureAuthority, Name 
                        FROM SignatureAuthority";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmb_Signature.DisplayMember = "SignatureAuthority";
                cmb_Signature.ValueMember = "Name";
                cmb_Signature.DataSource = dt;
            }
        }

        private void ch_Ms1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
