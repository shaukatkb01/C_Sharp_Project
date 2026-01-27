
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

namespace FileIndex
{
    public partial class InvoicePrint : Form
    {
        public InvoicePrint()
        {
            InitializeComponent();
        }

        private void InvoicePrint_Load(object sender, EventArgs e)
        {

            UIHelper.SetFormTheme(this);
            UIHelper.ApplyTheme(this);
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    string query = @"
                                    SELECT Id, InvoiceNo
                                    FROM InvoiceRegister
                                    ORDER BY ID DESC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    com_FileNo.DataSource = dt;
                    com_FileNo.DisplayMember = "InvoiceNo";
                    com_FileNo.ValueMember = "Id";
                    com_FileNo.SelectedIndex = -1;

                    string query2 = @"SELECT ID, AcknowledgeType
                                    FROM AcknowldeType";
                    SqlCommand cmd2 = new SqlCommand(query2, con);
                    SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    adapter2.Fill(dt2);
                    com_ST.DataSource = dt2;
                    com_ST.DisplayMember = "AcknowledgeType";
                    com_ST.ValueMember = "ID";
                    com_ST.SelectedIndex = -1;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading invoices: " + ex.Message);
                }
            }
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {

            if (com_FileNo.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Invoice Number.");
                return;
            }

            // ComboBox se ID utha li
            int issueId = Convert.ToInt32(com_FileNo.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein humne aakhir mein filter (WHERE) laga diya hai
                // Yaad rakhein: PhilitelicBuearu ki spelling database wali hi rakhein (e.g. Buearu)
                string query = @"SELECT 
                F.Id AS F_Id, 
                F.FileNo, 
                F.FileSubject,
                P.PhilitelicBuearuName AS BureauName,
                I.InvoiceNo, 
                I.IssueDate, 
                I.DispatchType,
                I.Totalamount, 
                I.Remarks,
                D.ID AS D_Id,
                D.DispatchType AS D_DispatchType,
                PR.FDCPrice, 
                PR.LeafletPrice, 
                PR.PostmarkPrice,
                PS.StampsQty, 
                PS.FDCQty, 
                PS.LeafletQty, 
                PS.FDCCQty, 
                PS.PostMarkQty

                FROM PhilatelicSupply PS
                
           
            INNER JOIN FileIndex F ON F.Id = PS.FileNo
            
           
            INNER JOIN Price PR ON PR.FileNo = F.Id
            LEFT JOIN PhilitelicBuearu P ON P.Id = PS.Address
            
            -- Invoice ko join karte waqt FileNo ke sath sath Bureau (Address) ka bhi khayal rakhen
            INNER JOIN InvoiceRegister I ON I.FileNo = F.Id AND I.PhiliticBureauName = P.Id
            LEFT JOIN DispatchType D ON D.ID = I.DispatchType
            WHERE PS.SupplyType IN (2, 4) -- Sirf issue receive or issue supply ke liye
            AND I.Id = @id
            ORDER BY I.InvoiceNo ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Report wala form open karein
                    frmReportView reportForm = new frmReportView();
                    // dt yahan apka datatable hai
                    reportForm.LoadReport(dt, "dtSingleInvoice", "Report/SingleInvoice.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Is Issue ke liye koi  Invoice nahi mila.");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                MessageBox.Show("Please insert dates of Invoices for duration", "Informaiton", MessageBoxButtons.OK);
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                searchBtn.Enabled = false;
                com_ST.Enabled = false;
                dtpFrom.Focus();
            }
            else
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                searchBtn.Enabled = true;
                com_ST.Enabled = true;
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {

            if ( com_ST.SelectedIndex == -1)
            {
                MessageBox.Show("Please slect type or Invoice", "Informaiton", MessageBoxButtons.OK);
                com_ST.Focus();
                com_ST.DroppedDown = true;
                return;
            }
            

            // ComboBox se ID utha li
            int issueId = Convert.ToInt32(com_ST.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein humne aakhir mein filter (WHERE) laga diya hai
                // Yaad rakhein: PhilitelicBuearu ki spelling database wali hi rakhein (e.g. Buearu)
                string query = @"SELECT 
                F.Id AS F_Id, 
                F.FileNo, 
                F.FileSubject,
                P.PhilitelicBuearuName AS BureauName,
                I.InvoiceNo, 
                I.IssueDate, 
                I.DispatchType,
                I.Acknowledgetyp AS I_Acknowledgetype,
                I.Totalamount, 
                I.Remarks,
                D.ID AS D_Id,
                D.DispatchType AS D_DispatchType,
                A.AcknowledgeType,
                @From AS FromDate,
                @To AS ToDate

                FROM InvoiceRegister I
                
           
            INNER JOIN FileIndex F ON F.Id = I.FileNo
            LEFT JOIN PhilitelicBuearu P ON P.Id = I.PhiliticBureauName
            LEFT JOIN DispatchType D ON D.ID = I.DispatchType
            LEFT JOIN AcknowldeType A ON A.ID = I.Acknowledgetyp    
            
            WHERE (@id = 3 AND I.Acknowledgetyp IN (1, 2)) 
                    OR (I.Acknowledgetyp = @id)
            ORDER BY I.InvoiceNo ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@From", dtpFrom.Value.Date);
                da.SelectCommand.Parameters.AddWithValue("@To", dtpTo.Value.Date);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Report wala form open karein
                    frmReportView reportForm = new frmReportView();
                    // dt yahan apka datatable hai
                    reportForm.LoadReport(dt, "dtInvoiceRegister", "Report/rptInvoiceRegister.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Is Issue ke liye koi  Invoice nahi mila.");
                }
            }


        }
    }
}
