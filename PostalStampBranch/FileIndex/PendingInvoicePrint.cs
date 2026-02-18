using Microsoft.Data.SqlClient;

using Microsoft.VisualBasic;
using PostalStampSystem;
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
    public partial class PendingInvoicePrint : Form
    {


        int reminder_No = 0;
        
        int PMGId = 0;
        string PMG_Referenc = ""; // fro report copy to
        int recordcont = 0;
        string subFirst = "";
        string subSecond = "";


        ////' ---- SINGLE / FIRST REMINDER ----
        //string sub_1_Single_First = "";
        //string sub_2_Single_First = "";

        ////' ---- DOUBLE / FIRST REMINDER ----
        //string sub_1_Double_First = "";
        //string sub_2_Double_First = "";

        ////' ---- SINGLE / SECOND REMINDER ----
        //string sub_1_Single_Second = "";
        //string sub_2_Single_Second = "";

        ////' ---- DOUBLE / SECOND REMINDER ----
        //string sub_1_Double_Second = "";
        //string sub_2_Double_Second = "";

        // DateTime variable (VBA ke lastReminderDate ki jagah)
        DateTime lastReminderDate = DateTime.Now; // Aap isay database se fill karenge

       

        public string GetOrdinal(long num)
        {
            if ((num % 100) is >= 11 and <= 13) return $"{num}<sup>th</sup>";

            string suffix = (num % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };

            return $"{num}<sup>{suffix}</sup>";
        }
        public string subTopFirstReminder(int id)
        {
            // Agar ek record hai
            if (id == 1)
            {
                string sub_1_Single_First = @"  It is hereby pointed out that the invoice pertaining to philatelic material, including FDCs, leaflets, and postmark (canceller) for Commemorative Postage Stamp, is still outstanding at your end owing to the persistent non-receipt of acknowledgment. Despite lapse of considerable time, no acknowledgment has been received by this office. The details of the pending invoice are given below.";

                return sub_1_Single_First;
            }
            // Agar ek se zyada records hain (id > 1)
            else
            {
                string sub_1_Double_First = @"      It is hereby pointed out that the invoices pertaining to philatelic material, including FDCs, leaflets, and postmarks (canceller) for Commemorative Postage Stamps, are still outstanding at your end owing to the persistent non-receipt of acknowledgments. Despite lapse of considerable time, no acknowledgments have been received by this office. The details of the pending invoices are given below.";

                return sub_1_Double_First;
            }
        }
        // fucntion ager second remider ho
        public string subToSecondReminder(int id)
        {

            if (id == 1)
            {
                string sub_1_Single_Second =
                    $@"     Kindly refer to this office letter of even No. dated {lastReminderDate.ToString("dd-MM-yyyy")}, through which this office pointed out that the invoice pertaining to philatelic material, including FDCs, leaflets, and postmark (cancellers) for Commemorative Postage Stamp, is still outstanding at your end owing to non-receipt of acknowledgment. The details of the pending invoice are given below.";
                return sub_1_Single_Second;
            }
            else
            {
                string sub_2_Double_Second =
                    $@"     Kindly refer to this office letter of even No. dated {lastReminderDate.ToString("dd-MM-yyyy")}, through which this office pointed out that the invoices pertaining to philatelic material, including FDCs, leaflets, and postmark (cancellers) for Commemorative Postage Stamp, is still outstanding at your end owing to non-receipt of acknowledgment. The details of the pending invoice are given below.";
                return sub_2_Double_Second;
            }
        }
        // function bottom
        public string subButtom(int id)
        {
            if (id == 1)
            {


                string sub_2_Single_First =
                            @"      In view of the above, you are requested to furnish the acknowledgment of the aforementioned invoice without any further delay, positively, for completion of official record.";
                return sub_2_Single_First;
            }
            else
            {

                string sub_2_Double_First =
                    @"      In view of the above, you are requested to furnish the acknowledgments of the aforementioned invoices without any further delay, positively, for completion of official record.";
                return sub_2_Double_First;
            }
        }



        //       '---PMG---

        string PMG_left_txt = "The Postmaster General,";
        string PMG_right_txt = " for information and with request to issue direction to concerned for doing the needful attention.";


        string PMG_Rawalpindi = " Northern Punjab Circle, Rawalpindi,";
        string PMG_Multtan = " Southern Punjab Circle, Multan";
        string PMG_Lahore = " Central Punjab Circle, Lahore,";
        string PMG_Karachi = " Metropolitan Circle, Karachi,";
        string PMG_Hyderabad = " Northern Sindh Circle,";
        string PMG_Quetta = " Balochistan Circle, Quetta,";
        string PMG_Peshawar = " Khyber Pakhtunkhwa Circle,";
        string PMG_Islamabad = " Islamabad & GB Circle,";
        public string PMGName(string pmg)

        {
            return PMG_left_txt + pmg + PMG_right_txt;


        }





        public void dropLoad(ComboBox cmb, ComboBox cmb2, int wiedth,int wiedth2)
        {
            using (SqlConnection con = new(Db.ConString))
            {
                string query = @"SELECT Id, Address
                            FROM PhilitelicBuearu
                            WHERE Id<=19
                            ORDER BY Id ASC";
                SqlDataAdapter adapter = new(query, con);
                DataTable dt = new();
                adapter.Fill(dt);
                cmb.DataSource = dt;
                cmb.DisplayMember = "Address";
                cmb.ValueMember = "Id";
                cmb.SelectedIndex = -1;
                cmb.DropDownWidth = wiedth;


                string query2 = @"SELECT Id, SignatureAuthority
                            FROM SignatureAuthority
                            
                            ORDER BY Id ASC";
                SqlDataAdapter adapter2 = new(query2, con);
                DataTable dt2 = new();
                adapter2.Fill(dt2);
                cmb2.DataSource = dt2;
                cmb2.DisplayMember = "SignatureAuthority";
                cmb2.ValueMember = "Id";
                cmb2.SelectedIndex = -1;
                cmb2.DropDownWidth = wiedth2;

            }
        }
        public PendingInvoicePrint()
        {
            InitializeComponent();
        }

        private void PendingInvoicePrint_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            dropLoad(cmb_Address, cmb_Signature,600,600);
            num_reminderNo.Value = 1;
            if (dtpRemidner.Value.Date >= DateTime.Now)
            {
                lastReminderDate = dtpRemidner.Value.Date;
            }
        }

        private void num_reminderNo_ValueChanged(object sender, EventArgs e)
        {
                reminder_No = Convert.ToInt32(num_reminderNo.Value);
            if (num_reminderNo.Value < 1)
            {
                num_reminderNo.Value++;
            }
            if (num_reminderNo.Value > 1)
            {
                dtpRemidner.Enabled = true;
                

            }
            else
            {
                dtpRemidner.Enabled = false;
            }
            
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (cmb_Address.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Address first", "missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cmb_Signature.Focus();
                cmb_Address.DroppedDown= true;
                return;
            }
            else if (cmb_Signature.SelectedIndex == -1)
            {
                MessageBox.Show("Select Signature first");
                cmb_Signature.Focus();
                cmb_Signature.DroppedDown= true;
                return;
            }
            if (dtpRemidner.Enabled)
            {
                if (dtpRemidner.Value.Date >= DateTime.Today)
                { 
                    MessageBox.Show("Select reminder Date");
                dtpRemidner.Focus();
                    SendKeys.Send("{F4}");
                    return;
            }
            }

            using (SqlConnection con = new(Db.ConString))
                {
                    string query = @"SELECT
                        I.FileNo,
                        I.InvoiceNo, 
                        I.PhiliticBureauName, 
                        I.IssueDate, 
                        I.Totalamount, 
                        I.Remarks,
                        PI.InvoiceRegisterId,
                        P.Address,
                        F.FileNo AS FileDisplayNo,
                        F.FileSubject,
                        @PMGr AS PMGRefer,
                        @subTop as SubTop,
                        @subBot as SubBot,
                        @sig AS Signature,
                        @rem AS ReminderNo
                        FROM InvoiceRegister I
                        LEFT JOIN PendingInvoice PI ON I.Id=PI.InvoiceRegisterId
                        LEFT JOIN PhilitelicBuearu P ON P.Id=I.PhiliticBureauName
                        LEFT JOIN FileIndex F ON F.Id=I.FileNo
                        WHERE I.PhiliticBureauName=@ad 
                        AND PI.AcknowledgeStatus=2";

                    SqlDataAdapter da = new(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@ad", cmb_Address.SelectedValue);
                    da.SelectCommand.Parameters.AddWithValue("@PMGr", PMG_Referenc);
                    da.SelectCommand.Parameters.AddWithValue("@subTop", "");
                    da.SelectCommand.Parameters.AddWithValue("@subBot", "");
                    da.SelectCommand.Parameters.AddWithValue("@sig", cmb_Signature.Text);
                    da.SelectCommand.Parameters.AddWithValue("@rem", GetOrdinal(reminder_No));
                    DataTable dt = new();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        recordcont = (dt.Rows.Count > 1) ? 2 : 1;

                        string topParagraph = "";
                        string bottomParagraph = "";
                        if (reminder_No == 1)
                        {

                            topParagraph = subTopFirstReminder(recordcont);
                            bottomParagraph = subButtom(recordcont);
                        }
                        else
                        {
                            topParagraph = subToSecondReminder(recordcont);
                            bottomParagraph = subButtom(recordcont);

                        }

                        da.SelectCommand.Parameters["@subTop"].Value = topParagraph;
                        da.SelectCommand.Parameters["@subBot"].Value = bottomParagraph;

                        dt.Clear();

                        da.Fill(dt);

                        frmReportView reportForm = new frmReportView();
                        reportForm.LoadReport(dt, "dtPInvoiceReminder", "Report/rptPInvoiceReminder.rdlc");
                        reportForm.Show();

                    }

                    else
                    {
                        MessageBox.Show("There are no pending invoices for this address!", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
        }

        

    

        private void cmb_Address_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Address.SelectedValue != null && int.TryParse(cmb_Address.SelectedValue.ToString(), out int id))
            {
                PMGId = id;

                switch (PMGId)
                {
                    case 4:
                    case 14:
                    case 16:
                        PMG_Referenc = PMGName(PMG_Rawalpindi); break;
                   
                    case 11:
                    case 15:
                        PMG_Referenc = PMGName(PMG_Multtan); break;
                    
                    case 13:
                    case 19:
                    case 3:
                    case 8:
                    case 9:
                    case 10:
                        PMG_Referenc = PMGName(PMG_Lahore); break;
                    
                    case 2:
                        PMG_Referenc = PMGName(PMG_Karachi); break;
                    
                    case 12:
                    case 18:
                    case 7:
                        PMG_Referenc = PMGName(PMG_Hyderabad); break;
                    
                    case 6:
                        PMG_Referenc = PMGName(PMG_Quetta); break;
                    
                    case 5:
                    case 17:
                        PMG_Referenc = PMGName(PMG_Peshawar); break;
                    
                    case 1:
                        PMG_Referenc = PMGName(PMG_Islamabad); break;

                }

            }
        }

        
    }
}
