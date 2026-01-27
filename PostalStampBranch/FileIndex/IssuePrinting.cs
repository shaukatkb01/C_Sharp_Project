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
using FileIndex.dtTable;

namespace FileIndex
{
    public partial class IssuePrinting : Form
    {
        int currentFileType = 0;

        int Dgref = 0;
        int silnt = 0;
        
        private void EnableSouveControls(Control parent, bool enable)
        {
            if (parent.Tag != null && parent.Tag.ToString() == "souveControll")
            {
                parent.Enabled = enable;
            }
            foreach (Control child in parent.Controls) EnableSouveControls(child, enable);
        }

        public IssuePrinting()
        {
            InitializeComponent();
        }

        private void IssuePrinting_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                silnt = txt_salient.Text.Length;
                Dgref = txt_DgRefer.Text.Length;
                UIHelper.ApplyTheme(this);
                UIHelper.SetFormTheme(this);
                FormLoadingData.FillIssueNumbers(cmb_Issue_No);
                FormLoadingData.SignatureAuthority(cmb_Signature);

                // salient features

                // Form load hotay hi default matter textbox mein show ho jayega
                // StringBuilder use karna zyada professional hai jab lambi strings hon
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Size of Stamp               : 56 mm x 35 mm.");
                sb.AppendLine("Number of Stamps in a sheet : 3 x 5 (15 Stamps)");
                sb.AppendLine("Denomination                : Rs.30/-");
                sb.AppendLine("Colors                      : CMYK");
                sb.AppendLine("Printing Technology         : Lithography (Offset)");
                sb.AppendLine("Format                      : Vertical");
                sb.AppendLine("Paper                       : 100 GSM W/M Gummed Paper");
                sb.AppendLine("Gum                         : PVA");
                sb.AppendLine("Quantity of Stamp           : 0.0.3 million (Thirty Thousand Only)");
                sb.AppendLine("Stamp Design                : Supplied by Customer");
                sb.AppendLine("Printers                    : Pakistan Security Printing Corporation, Karachi.");
                sb.AppendLine("Date of Issue               : 10/12/2025");

                txt_salient.Text = sb.ToString();
            }



        }


















        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                MessageBox.Show("You must edit the Supply Date.");
                picker_DilverDate.Enabled = true;
                picker_DilverDate.Focus();
            }
            else
            {
                picker_DilverDate.Enabled = false;

            }
        }

        private void cmb_Issue_No_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Issue_No.SelectedValue == null || cmb_Issue_No.SelectedIndex == -1) return;

            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "SELECT FileType FROM FileIndex WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", cmb_Issue_No.SelectedValue);
                    con.Open();
                    currentFileType = Convert.ToInt32(cmd.ExecuteScalar());
                }

                bool shouldEnable = (currentFileType == 2 || currentFileType == 4);
                EnableSouveControls(this, shouldEnable);
            }
            catch { }
        }

        private void btn_Distribution_Click(object sender, EventArgs e)
        {

            if (cmb_Issue_No.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Issue Number.");
                return;
            }

            // ComboBox se ID utha li
            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein humne aakhir mein filter (WHERE) laga diya hai
                // Yaad rakhein: PhilitelicBuearu ki spelling database wali hi rakhein (e.g. Buearu)
                string query = @"
                SELECT F.Id AS F_Id, F.FileNo, F.FileSubject,
                       P.PhilitelicBuearuName AS BureauName,
                       C.IssueNo,
                       PS.StampsQty, PS.FDCQty, PS.LeafletQty,PS.FDCCQty,PS.PostmarkQty,
                       S.Id AS S_Id, S.SupplyType,
                       SortTable.OrderNo
                FROM PhilatelicSupply PS
                LEFT JOIN PhilitelicBuearu P ON P.Id = PS.Address
                LEFT JOIN FileIndex F ON F.Id = PS.FileNo
                LEFT JOIN SupplyType S ON S.Id = PS.SupplyType
                LEFT JOIN CommStamp C ON C.FileNo = PS.FileNo
                LEFT JOIN ( 
                VALUES  
                    ('Philatelic Branch, Karachi', 1, 1),
                    ('Philatelic Branch, Karachi', 2, 2), 
                    ('M/S Pakistan Security Printing Corporation, Karachi', Null,  3),  
                    ('M/S Promt Printer, Karachi', NULL, 4), 
                    ('M/s Post Foundation Press, Karachi', NULL, 5),
                    ('Manager National Philatelic Bureau, Islamabad G.P.O.', NULL, 6), 
                    ('Manager Philatelic Bureau, Karachi G.P.O.', NULL, 7), 
                    ('Manager Philatelic Bureau, Lahore G.P.O.', NULL, 8), 
                    ('Manager Philatelic Bureau, Rawalpindi G.P.O. ', NULL, 9), 
                    ('Manager Philatelic Bureau, Peshawar G.P.O. ', NULL, 10), 
                    ('Manager Philatelic Bureau, Quetta G.P.O.', NULL, 11), 
                    ('Manager Philatelic Bureau, Khairpur G.P.O.', NULL, 12), 
                    ('Manager Philatelic Bureau, Faisalabad G.P.O.', NULL, 13), 
                    ('Manager Philatelic Bureau, Sialkot G.P.O.', NULL, 14), 
                    ('Manager Philatelic Bureau, Sargodha G.P.O.', NULL, 15), 
                    ('Manager Philatelic Bureau, Multan G.P.O.', NULL, 16), 
                    ('Manager Philatelic Bureau, Hyderabad G.P.O.', NULL, 17), 
                    ('Manager Philatelic Bureau, Gujranwala G.P.O.', NULL, 18), 
                    ('Manager Philatelic Bureau, Gujrat G.P.O.', NULL, 19), 
                    ('Manager Philatelic Bureau, Bahawalpur G.P.O.', NULL, 20), 
                    ('Manager Philatelic Bureau, Murree G.P.O.', NULL, 21), 
                    ('Manager Philatelic Bureau, Mardan G.P.O.', NULL, 22), 
                    ('Manager Philatelic Bureau,Sukkur G.P.O.', NULL, 23), 
                    ('Manager Post Mall Gulberg, Lahore.', NULL, 24), 
                    ('The Asstt. Director (Phil), Dte-General Pakistan Post Islamabad', NULL, 25) 
                )   

                 AS SortTable(BName, SID, OrderNo) 
                  ON (P.PhilitelicBuearuName = SortTable.BName AND (SortTable.SID IS NULL OR S.Id = SortTable.SID))
                WHERE PS.FileNo = @id AND S.Id IN(1,2) -- Sirf issue receive or issue supply ke liye
                ORDER BY SortTable.OrderNo ASC"; // Jo list mein nahi wo aakhir mein (99)

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Report wala form open karein
                    frmReportView reportForm = new frmReportView();

                    // Ye function humne abhi Report Form ke andar banana hai (Niche Step 2 dekhein)
                    reportForm.LoadReport(dt, "dtSupply", "Report/rptSupply.rdlc");

                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Is Issue ke liye koi data nahi mila.");
                }
            }

        }

        private void btn_Promtprinter_Click(object sender, EventArgs e)
        {
            
            if (cmb_Issue_No.SelectedIndex == -1 || cmb_Signature.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Issue Number.");
                return;
            }


            if (string.IsNullOrWhiteSpace(txt_StmOption.Text) && string.IsNullOrWhiteSpace(txt_SouvOption.Text)
                ||
                (string.IsNullOrWhiteSpace(txt_Joborder.Text) && string.IsNullOrWhiteSpace(txt_JoborderSouv.Text)))
            {
                MessageBox.Show("Please insert Stamp or Souvenir Option and Joborder number fist");
                return;
            }

            int NesDgref =txt_DgRefer.Text.Length;

            if (NesDgref<=Dgref)
            {
                // Agar length barhi nahi, toh error dikhao
                MessageBox.Show( "Reference  mein kuch naya likhna lazmi hai!");
                return;
            }

            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein sirf @sg (Signature) rakha hai, @sl nikal diya gaya hai
                string query = @"SELECT 
                F.Id AS F_Id, 
                F.FileNo, 
                F.FileSubject, 
                F.FileType,
                C.DateOfIssue, 
               
                @sg AS SignatureAuthority,
                @rf AS DGReference,
                @so AS StampOption,
                @jo AS JoborderNo,
                @su AS SouvenirOption,
                @sjo AS SouvenirJoborderNo,
                @dd AS DeliveryDate,
                P.StampPrice, 
                P.SouvenirPrice,
                Q.StampQty,
                Q.SouvenirQty,
                Q.FDCQty, 
                Q.LeafletQty,  
                Q.PostMarkQty -- Spelling check kar lain agar database mein 'Mark' hai
                
    FROM CommStamp C 
    INNER JOIN FileIndex F ON F.Id = C.FileNo 
    LEFT JOIN StockPrice P ON P.FileNo = C.FileNo  
    LEFT JOIN StockPhilQuantity Q ON Q.FileNo = C.FileNo
    WHERE C.FileNo = @id AND F.FileType IN(1,2,3,4)"; // @id hamesha main ID (FileNo) honi chahiye

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@sg", cmb_Signature.Text); // ComboBox se string value
                da.SelectCommand.Parameters.AddWithValue("@so", txt_StmOption.Text); // Form par moojood txt_PrintOption ka text
                da.SelectCommand.Parameters.AddWithValue("@jo", txt_Joborder.Text); // Form par moojood txt_JobOrderNo ka text
                da.SelectCommand.Parameters.AddWithValue("@su", txt_SouvOption.Text); // Form par moojood txt_SouvOption ka text
                da.SelectCommand.Parameters.AddWithValue("@sjo", txt_JoborderSouv.Text); // Form par moojood txt_SouvJoborder ka text

                // Parameter ko pehle define kar letay hain taake code saaf rahay
                string deliveryText = "";

                if (picker_DilverDate.Enabled)
                {
                    // Behtar format: "10-Jan-2026"
                    string formattedDate = picker_DilverDate.Value.ToString("dd-MM-yyyy");
                    deliveryText = "Please supply of above items should be by the First half of " + formattedDate;
                }
                else
                {
                    deliveryText = "Please supply of above items should be Immediately";
                }

                // Parameter add karein
                da.SelectCommand.Parameters.AddWithValue("@dd", deliveryText);
                // Form par moojood txt_DgRefer ka sara text parameter mein bhej diya
                da.SelectCommand.Parameters.AddWithValue("@rf", txt_DgRefer.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    frmReportView reportForm = new frmReportView();
                    // dtDGQty aapka dataset name hai jo RDLC mein hona chahiye
                    reportForm.LoadReport(dt, "dtPrintOrder", "Report/rptPromtPPO.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Record nahi mila! Check karein ID: " + issueId.ToString());
                }
            }
        }

        private void btn_Foundation_Click(object sender, EventArgs e)
        {
            if (cmb_Issue_No.SelectedIndex == -1 || cmb_Signature.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Issue Number.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_StmOption.Text) && string.IsNullOrWhiteSpace(txt_SouvOption.Text)
                ||
                (string.IsNullOrWhiteSpace(txt_Joborder.Text) && string.IsNullOrWhiteSpace(txt_JoborderSouv.Text)))
            {
                MessageBox.Show("Please insert Stamp or Souvenir Option and Joborder number fist");
                return;
            }

            int NesDgref = txt_DgRefer.Text.Length;

            if (NesDgref <= Dgref)
            {
                // Agar length barhi nahi, toh error dikhao
                MessageBox.Show("Reference  mein kuch naya likhna lazmi hai!");
                return;
            }

            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein sirf @sg (Signature) rakha hai, @sl nikal diya gaya hai
                string query = @"SELECT 
                F.Id AS F_Id, 
                F.FileNo, 
                F.FileSubject, 
                F.FileType,
                C.DateOfIssue, 
               
                @sg AS SignatureAuthority,
                @rf AS DGReference,
                @so AS StampOption,
                @jo AS JoborderNo,
                @su AS SouvenirOption,
                @sjo AS SouvenirJoborderNo,
                @dd AS DeliveryDate,
                P.StampPrice, 
                P.SouvenirPrice,
                Q.StampQty,
                Q.SouvenirQty,
                Q.FDCQty, 
                Q.LeafletQty,  
                Q.PostMarkQty -- Spelling check kar lain agar database mein 'Mark' hai
                
    FROM CommStamp C 
    INNER JOIN FileIndex F ON F.Id = C.FileNo 
    LEFT JOIN StockPrice P ON P.FileNo = C.FileNo  
    LEFT JOIN StockPhilQuantity Q ON Q.FileNo = C.FileNo
    WHERE C.FileNo = @id AND F.FileType IN(1,2,3,4)"; // @id hamesha main ID (FileNo) honi chahiye

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@sg", cmb_Signature.Text); // ComboBox se string value
                da.SelectCommand.Parameters.AddWithValue("@so", txt_StmOption.Text); // Form par moojood txt_PrintOption ka text
                da.SelectCommand.Parameters.AddWithValue("@jo", txt_Joborder.Text); // Form par moojood txt_JobOrderNo ka text
                da.SelectCommand.Parameters.AddWithValue("@su", txt_SouvOption.Text); // Form par moojood txt_SouvOption ka text
                da.SelectCommand.Parameters.AddWithValue("@sjo", txt_JoborderSouv.Text); // Form par moojood txt_SouvJoborder ka text
                                                                                         // Form par moojood txt_DgRefer ka sara text parameter mein bhej diya

                string deliveryText = "";

                if (picker_DilverDate.Enabled)
                {
                    // Behtar format: "10-Jan-2026"
                    string formattedDate = picker_DilverDate.Value.ToString("dd-MM-yyyy");
                    deliveryText = "by the first half of " + formattedDate;
                }
                else
                {
                    deliveryText = "Immediately";
                }

                // Parameter add karein
                da.SelectCommand.Parameters.AddWithValue("@dd", deliveryText);
                da.SelectCommand.Parameters.AddWithValue("@rf", txt_DgRefer.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    frmReportView reportForm = new frmReportView();
                    // dtDGQty aapka dataset name hai jo RDLC mein hona chahiye
                    reportForm.LoadReport(dt, "dtPrintOrder", "Report/rptFoundtionPO.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Record nahi mila! Check karein ID: " + issueId.ToString());
                }
            }
        }

        private void btn_Printorder_Click(object sender, EventArgs e)
        {

            if (cmb_Issue_No.SelectedIndex == -1 || cmb_Signature.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Issue Number.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_StmOption.Text) && string.IsNullOrWhiteSpace(txt_SouvOption.Text)
                ||
                (string.IsNullOrWhiteSpace(txt_Joborder.Text) && string.IsNullOrWhiteSpace(txt_JoborderSouv.Text)))
            {
                MessageBox.Show("Please insert Stamp or Souvenir Option and Joborder number fist");
                return;
            }

            int NesDgref = txt_DgRefer.Text.Length;

            if (NesDgref <= Dgref)
            {
                // Agar length barhi nahi, toh error dikhao
                MessageBox.Show("Reference  mein kuch naya likhna lazmi hai!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_StmOption.Text) ||
                string.IsNullOrWhiteSpace(txt_Joborder.Text) ||
                string.IsNullOrWhiteSpace(txt_DgRefer.Text))
            {
                MessageBox.Show("Please fill in all the required fields: Stamp Option, Job Order No, Souvenir Option, and DG Reference.");
                return;
            }

            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein sirf @sg (Signature) rakha hai, @sl nikal diya gaya hai
                string query = @"SELECT 
                F.Id AS F_Id, 
                F.FileNo, 
                F.FileSubject, 
                F.FileType,
                C.DateOfIssue, 
               
                @sg AS SignatureAuthority,
                @rf AS DGReference,
                @so AS StampOption,
                @jo AS JoborderNo,
                @su AS SouvenirOption,
                @sjo AS SouvenirJoborderNo,
                @dd AS DeliveryDate,
                P.StampPrice, 
                P.SouvenirPrice,
                Q.StampQty,
                Q.SouvenirQty,
                Q.FDCQty, 
                Q.LeafletQty,  
                Q.PostMarkQty -- Spelling check kar lain agar database mein 'Mark' hai
                
    FROM CommStamp C 
    INNER JOIN FileIndex F ON F.Id = C.FileNo 
    LEFT JOIN StockPrice P ON P.FileNo = C.FileNo  
    LEFT JOIN StockPhilQuantity Q ON Q.FileNo = C.FileNo
    WHERE C.FileNo = @id AND F.FileType IN(1,2,3,4)"; // @id hamesha main ID (FileNo) honi chahiye

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@sg", cmb_Signature.Text); // ComboBox se string value
                da.SelectCommand.Parameters.AddWithValue("@so", txt_StmOption.Text); // Form par moojood txt_PrintOption ka text
                da.SelectCommand.Parameters.AddWithValue("@jo", txt_Joborder.Text); // Form par moojood txt_JobOrderNo ka text
                da.SelectCommand.Parameters.AddWithValue("@su", txt_SouvOption.Text); // Form par moojood txt_SouvOption ka text
                da.SelectCommand.Parameters.AddWithValue("@sjo", txt_JoborderSouv.Text); // Form par moojood txt_SouvJoborder ka text
                                                                                         // Form par moojood txt_DgRefer ka sara text parameter mein bhej diya

                string deliveryText = "";

                if (picker_DilverDate.Enabled)
                {
                    // Behtar format: "10-Jan-2026"
                    string formattedDate = picker_DilverDate.Value.ToString("dd-MM-yyyy");
                    deliveryText = "Please supply of above items should be by the First half of " + formattedDate;
                }
                else
                {
                    deliveryText = "Please supply of above items should be Immediately";
                }

                // Parameter add karein
                da.SelectCommand.Parameters.AddWithValue("@dd", deliveryText);
                da.SelectCommand.Parameters.AddWithValue("@rf", txt_DgRefer.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    frmReportView reportForm = new frmReportView();
                    // dtDGQty aapka dataset name hai jo RDLC mein hona chahiye
                    reportForm.LoadReport(dt, "dtPrintOrder", "Report/rptNSPCPrintOrder.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Record nahi mila! Check karein ID: " + issueId.ToString());
                }
            }
        }

        private void btn_DGCircular_Click(object sender, EventArgs e)
        {
            if (cmb_Issue_No.SelectedIndex == -1 || cmb_Signature.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Issue Number and Signature first.");
                return;
            }
            int newSilnt = txt_salient.Text.Length;
            if (newSilnt <= silnt)
            {
                MessageBox.Show("Please insert in silent feture msut!");
                return;
            }

            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein aakhir mein '@sl AS SalientFeatures' add kiya hai
                string query = @"SELECT 
        F.Id AS F_Id, 
        F.FileNo, 
        F.FileSubject, 
        C.DateOfIssue, 
        
        @sg AS SignatureAuthority,
        
        P.StampPrice, 
        P.SouvenirPrice, 
        P.FDCPrice, 
        P.LeafletPrice 
        FROM CommStamp C
        LEFT JOIN FileIndex F ON F.Id = C.FileNo
        
        LEFT JOIN StockPrice P ON P.FileNo = C.FileNo
        WHERE C.FileNo = @id";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@sg", cmb_Signature.Text);

                // Form par moojood txt_salient ka sara text parameter mein bhej diya


                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    frmReportView reportForm = new frmReportView();
                    reportForm.LoadReport(dt, "dtDGCircular", "Report/rptDGCircular.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Record nahi mila! Check karein ID: " + issueId.ToString());
                }
            }
        }

        private void btn_DGQty_Click(object sender, EventArgs e)
        {
            if (cmb_Issue_No.SelectedIndex == -1 || cmb_Signature.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Issue Number.");
                return;
            }

            
            
            int newSilnt = txt_salient.Text.Length;
            if (newSilnt == silnt)
            {
                MessageBox.Show("Please insert in silent feture msut!");
                return;
            }


            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein sirf @sg (Signature) rakha hai, @sl nikal diya gaya hai
                string query = @"SELECT 
                                F.Id AS F_Id, 
                                F.FileNo, 
                                F.FileSubject, 
                                C.DateOfIssue, 
                                
                                @sg AS SignatureAuthority,
                                 @sl AS SalientFeatures,  -- Naya dynamic column
                                PB.PhilitelicBuearuName AS BureauName,
                                PS.StampsQty,
                                PS.FDCQty,
                                PS.LeafletQty,
                                PS.FDCCQty,
                                P.StampPrice, 
                                P.SouvenirPrice, 
                                P.FDCPrice, 
                                P.LeafletPrice 
FROM PhilatelicSupply PS  -- Ab humne main table isay banaya hai
INNER JOIN CommStamp C ON C.FileNo = PS.FileNo  -- Sirf matching records
INNER JOIN FileIndex F ON F.Id = C.FileNo        -- File ki maloomat lazmi ho
LEFT JOIN StockPrice P ON P.FileNo = C.FileNo        -- Price agar na bhi ho toh chalega
LEFT JOIN PhilitelicBuearu PB ON PB.Id = PS.Address
WHERE C.FileNo = @id AND PS.SupplyType=2
AND PS.Address NOT IN(24,25)";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@sg", cmb_Signature.Text);

                // Form par moojood txt_salient ka sara text parameter mein bhej diya
                da.SelectCommand.Parameters.AddWithValue("@sl", txt_salient.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    frmReportView reportForm = new frmReportView();
                    // dtDGQty aapka dataset name hai jo RDLC mein hona chahiye
                    reportForm.LoadReport(dt, "dtDGQty", "Report/rptDGQty.rdlc");
                    reportForm.Show();
                }
                else
                {
                    MessageBox.Show("Record nahi mila! Check karein ID: " + issueId.ToString());
                }
            }
        }

        private void btn_Tags_Click(object sender, EventArgs e)
        {
            if (cmb_Issue_No.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Issue Number.");
                return;
            }

            // ComboBox se ID utha li
            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein humne aakhir mein filter (WHERE) laga diya hai
                // Yaad rakhein: PhilitelicBuearu ki spelling database wali hi rakhein (e.g. Buearu)
                string query = @"SELECT 
                F.Id AS F_Id, 
                F.FileNo, 
                I.FileNo AS InvoiceFileNo,
                I.InvoiceNo,
                I.PhiliticBureauName,
                @sg AS SignatureAuthority, -- C# se pass kiya gaya parameter
                P.PhilitelicBuearuName AS BureauName,
                D.ID AS D_Id,
                D.DispatchType AS D_DispatchType,
                M.DispatchType AS M_DispatchType
                FROM InvoiceRegister I
                LEFT JOIN FileIndex F ON F.Id = I.FileNo
                LEFT JOIN PhilitelicBuearu P ON P.Id = I.PhiliticBureauName
                -- Pehle M (MaleList) ko join karna lazmi hai
                LEFT JOIN IssueMaleList M ON M.MaleListFileId = I.FileNo 
                -- Phir D (DispatchType) ko M ke sath jorein
                LEFT JOIN DispatchType D ON D.ID = M.DispatchType
                WHERE I.SupplyType IN (1, 2) 
                AND I.FileNo = @id 
                ORDER BY I.InvoiceNo ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@sg", cmb_Signature.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Report wala form open karein
                    frmReportView reportForm = new frmReportView();
                    // dt yahan apka datatable hai
                    reportForm.LoadReport(dt, "dtTag", "Report/rptTag.rdlc");
                    reportForm.Show();
                }
                else
                {
                    // Yeh batayega ke @id mein kya value ja rahi hai
                    MessageBox.Show("Record nahi mila! Check karein ID: " + issueId.ToString());
                }
            }
        }

        private void btn_Invoices_Click(object sender, EventArgs e)
        {
            if (cmb_Issue_No.SelectedIndex == -1 || cmb_Signature.SelectedIndex== -1)
            {
                MessageBox.Show("Please select an Issue Number and Signature Authority.");
                return;
            }

            // ComboBox se ID utha li
            int issueId = Convert.ToInt32(cmb_Issue_No.SelectedValue);

            using (var con = new SqlConnection(Db.ConString))
            {
                // Query mein humne aakhir mein filter (WHERE) laga diya hai
                // Yaad rakhein: PhilitelicBuearu ki spelling database wali hi rakhein (e.g. Buearu)
                string query = @"SELECT 
                F.Id AS F_Id, 
                F.FileNo, 
                F.FileSubject,
                @sg AS SignatureAuthority,
                P.PhilitelicBuearuName AS BureauName,
                I.InvoiceNo, 
                I.IssueDate, 
                M.DispatchType,
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
            -- Pehle FileIndex join karein taake baaki tables 'F.Id' use kar saken
            INNER JOIN FileIndex F ON F.Id = PS.FileNo
            
            -- Phir PriceList aur baaki tables
            LEFT JOIN StockPrice PR ON PR.FileNo = F.Id
            LEFT JOIN PhilitelicBuearu P ON P.Id = PS.Address
            
            -- Invoice ko join karte waqt FileNo ke sath sath Bureau (Address) ka bhi khayal rakhen
            LEFT JOIN InvoiceRegister I ON I.FileNo = F.Id AND I.PhiliticBureauName = P.Id
            INNER JOIN IssueMaleList M ON M.MaleListFileId=I.FileNo
            LEFT JOIN DispatchType D ON D.ID = M.DispatchType
            WHERE I.SupplyType IN (1, 2) -- Sirf issue receive or issue supply ke liye
            AND PS.FileNo = @id
            ORDER BY I.InvoiceNo ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                da.SelectCommand.Parameters.AddWithValue("@sg", cmb_Signature.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Report wala form open karein
                    frmReportView reportForm = new frmReportView();
                    // dt yahan apka datatable hai
                    reportForm.LoadReport(dt, "dtIssueInvoice", "Report/rptIssueInvoice.rdlc");
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

        
  

