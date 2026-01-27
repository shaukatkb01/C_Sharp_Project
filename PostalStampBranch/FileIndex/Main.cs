using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FileIndex
{
    public partial class Main : Form
    {



        public void MigratePriceAndQty()
        {

        }
        private void AutoBackup()
        {

            // Backup folder ka rasta
            string folderPath = @"C:\Users\ccsk_\OneDrive\PS\ProjectBackups";

            // 7 din se purani files dhoondo aur delete karo
            DirectoryInfo d = new DirectoryInfo(folderPath);
            foreach (FileInfo file in d.GetFiles("*.bak"))
            {
                if (file.LastWriteTime < DateTime.Now.AddDays(-7))
                {
                    file.Delete();
                }
            }
            try
            {
                // 1. Backup kahan save karna hai (Folder path)
                
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                // 2. File ka naam date ke sath (taake roz naya backup banay)
                string fileName = "Backup_" + DateTime.Now.ToString("yyyy-MM-dd_HHmm") + ".bak";
                string fullPath = System.IO.Path.Combine(folderPath, fileName);

                // 3. SQL Command
                string query = $"BACKUP DATABASE [PSDB] TO DISK = '{fullPath}'";

                using (SqlConnection con = new(Db.ConString))
                {
                    SqlCommand cmd = new(query, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Agar backup fail ho jaye (maslan space na ho ya permission na ho)
                MessageBox.Show("Backup Error: " + ex.Message);
            }
        }


        // Text update karne ke liye alag function (Performance behtar hogi)
        private void SetSidebarText(bool show)
        {
            string f = show ? "  Add New File" : "";
            string c = show ? "  Add Comm Details" : "";
            string i = show ? "  Issue Printing Work" : "";
            string p = show ? "  Philetalic Supply" : "";
            string inv = show ? "  Invoice Work" : "";
            // ... baqi buttons ...

            btn_File.Text = f;
            btn_Com.Text = c;
            btn_Issue.Text = i;
            btn_PhiletalicSupply.Text = p;
            btn_InvoiceWork.Text = inv;
        }
        private void BindFormToTab(Form childForm, TabPage targetTab, string tabHeading)
        {
            // 1. Child form ki settings taake wo tab mein fit ho jaye
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // 2. Tab ki Heading (Text) ko update karna
            targetTab.Text = tabHeading;

            // 3. Purane controls ko khatam karke naya form add karna
            targetTab.Controls.Clear();
            targetTab.Controls.Add(childForm);

            // 4. Form ko display karna
            childForm.Show();
            childForm.CreateControl();
        }
        public Main()
        {
            InitializeComponent();
        }


        private void OpenChildForm(Form child)
        {
            // 1. Isay Main Form ka bacha (Child) banayen
            child.MdiParent = this;

            // 2. Kholne se pehle Theme apply kar dein
            UIHelper.SetFormTheme(child);

            // 3. Form ko show karein
            child.Show();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

            // Form ko screen ke mutabiq set karna
            this.Size = new Size(workingArea.Width, workingArea.Height);
            this.Location = new Point(0, 0);

            // Sidebar aur TabControl ko force fit karna
            pnl_Sidebar.Dock = DockStyle.Left;
            tabControl1.Dock = DockStyle.Fill;
            this.WindowState = FormWindowState.Maximized;
            tabControl1.TabPages.Clear();
            if (GlobalData.UerRole != null)
            {
                txt_userName.Text = "Welcome Mr." + GlobalData.CurrentUser + "login as " + GlobalData.UerRole;


            }
            else
            {
                txt_userName.Visible = false;

            }


            TabPage todoTab = new("📋 My To-Do List");
            tabControl1.TabPages.Add(todoTab);

            // 2. FormHelper ko use karte hue Todo form ko is tab mein dalkar show karein
            BindFormToTab(new frmDashboard(), todoTab, "📋 My To-Do List");

            // 3. Todo tab ko default select karlein
            tabControl1.SelectedTab = todoTab;

        }



        private void addIssueDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Check karein kya Form1 pehle se khula hai?
            Form frm = Application.OpenForms["Form1"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                Form1 newFrm = new();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Check karein kya Form1 pehle se khula hai?
            Form frm = Application.OpenForms["AddFileNo"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                AddFileNo newFrm = new();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void registorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form frm = Application.OpenForms["Register"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                Register newFrm = new();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void editIssueDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Check karein kya Form1 pehle se khula hai?
            Form frm = Application.OpenForms["IssueCorrection"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                IssueCorrection newFrm = new();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void editFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["FileCorrection"];

            if (frm == null)
            {
                // Agar nahi khula to naya khol dein
                FileCorrection newFrm = new();
                newFrm.MdiParent = this; // Isay parent ke andar rakhein

                // Agar aap chahte hain ke form maximize khule (full screen):
                newFrm.WindowState = FormWindowState.Maximized;

                newFrm.Show();
            }
            else
            {
                // 2. Agar khula hai to usay samnay le aayen
                if (frm.WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal; // Agar minimize tha to wapis normal kar dein
                }
                frm.Activate();
            }
        }

        private void searchFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                searchFile childForm = new();

                // 1. Batayein ke iska Parent ye Form hai
                childForm.MdiParent = this;

                // 2. IS LINE KO ADD KAREIN: 
                // Is se form parent ke andar poori screen par khulay ga
                childForm.WindowState = FormWindowState.Maximized;

                childForm.Show();
            }
        }



        private void addDistributionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["DistributionList"];

            if (frm == null)

            {
                DistributionList newFRm = new();
                newFRm.MdiParent = this;
                newFRm.WindowState = FormWindowState.Maximized;
                newFRm.Show();
            }
            else
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    frm.WindowState = FormWindowState.Normal;
                }
                frm.Activate();
            }
        }








        private void timer1_Tick(object sender, EventArgs e)
        {
            bool isMouseOver = pnl_Sidebar.ClientRectangle.Contains(pnl_Sidebar.PointToClient(Control.MousePosition));

            // Poore Form ka layout rokein taake jhatke na lagein
            this.SuspendLayout();

            // Sidebar ko hamesha sab se upar rakhein (Overlay effect)
            pnl_Sidebar.BringToFront();

            if (isMouseOver)
            {
                if (pnl_Sidebar.Width < 220)
                {
                    pnl_Sidebar.Width += 40;
                }
                else if (string.IsNullOrEmpty(btn_File.Text))
                {
                    SetButtonsText(true);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(btn_File.Text))
                {
                    SetButtonsText(false);
                }

                if (pnl_Sidebar.Width > 50)
                {
                    pnl_Sidebar.Width -= 40;
                }
            }

            this.ResumeLayout(); // Layout dobara shuru karein
        }



        // Button text set karne ke liye Helper Function
        private void SetButtonsText(bool show)
        {
            btn_File.Text = show ? "   Add New File" : "";
            btn_Com.Text = show ? "   Add Comm Details" : "";
            btn_Issue.Text = show ? "   Issue Printing Work" : "";
            btn_PhiletalicSupply.Text = show ? "   Philetalic Supply" : "";
            btn_InvoiceWork.Text = show ? "   Invoice Work" : "";
            btn_Admin.Text = show ? "   User Management" : "";
            btn_Address.Text = show ? "   Address Book" : "";
        }





        private void btn_Issue_Click(object sender, EventArgs e)
        {
            // 1. Loading dikhao
            pnl_Loading.Visible = true;
            pnl_Loading.BringToFront();

            // Windows ko batayein ke pehle panel dikhao, phir agla kaam karo
            Application.DoEvents();

            // 2. TabControl saaf karein
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tab_1);
            tabControl1.TabPages.Add(tab_2);
            tabControl1.TabPages.Add(tab_3);

            // 3. Forms load karein (Yahan asli waqt lagta hai)
            BindFormToTab(new DistributionList(), tab_1, "[+] Add Supply distribution");
            BindFormToTab(new IssuePrinting(), tab_2, "[🖨] Issue Printing Work");
            BindFormToTab(new IssueMaillist(), tab_3, "[🔍] Issue MailList");

            pnl_Loading.Left = (this.ClientSize.Width - pnl_Loading.Width) / 2;
            pnl_Loading.Top = (this.ClientSize.Height - pnl_Loading.Height) / 2;
            // 4. Kaam khatam, ab loading chhupa dein
            pnl_Loading.Visible = false;
        }

        private void btn_PhiletalicSupply_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tab_1);
            tabControl1.TabPages.Add(tab_2);
            //tabControl1.TabPages.Add(tab_3);
            tabControl1.TabPages.Add(tab_4);
            tabControl1.TabPages.Add(tab_6);

            BindFormToTab(new PhilatelicSupply(), tab_1, "[+] Stamp Phil Supply");
            BindFormToTab(new PhilatelicSupplyDetail(), tab_2, "[🖨] Change Phil Supply");
            //BindFormToTab(new PendingInvoice(), tab_3, "[🔍] Akcnowledge Invoice");
            BindFormToTab(new AddStationeryItems(), tab_4, "[🖨] Add Stationery Items");
            BindFormToTab(new StationeryTransactionscs(), tab_6, "[🖨] Stationery Issue");
        }

        private void btn_InvoiceWork_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear(); // Pehle purane tabs saaf karein

            tabControl1.TabPages.Add(tab_1);
            tabControl1.TabPages.Add(tab_2);
            tabControl1.TabPages.Add(tab_3);
            tabControl1.TabPages.Add(tab_4);
            BindFormToTab(new InvoiceEntrycs(), tab_1, "[+] Generate Invoice");
            BindFormToTab(new InvoiceCorection(), tab_2, "[🖨] Invoice Corection");
            BindFormToTab(new InvoiceAcknowlodge(), tab_3, "[🔍] Akcnowledge Invoice");
            BindFormToTab(new InvoicePrint(), tab_4, "[🖨] Print Invoice");
        }

        private void btn_Admin_Click(object sender, EventArgs e)
        {

            tabControl1.TabPages.Clear(); // Pehle purane tabs saaf karein

            tabControl1.TabPages.Add(tab_1);
            tabControl1.TabPages.Add(tab_2);


            BindFormToTab(new UserRoles(), tab_1, "[+] Allow User");
            BindFormToTab(new Register(), tab_2, "[✎] Register User");


        }

        private void btn_Address_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear(); // Pehle purane tabs saaf karein

            tabControl1.TabPages.Add(tab_1);

            BindFormToTab(new Address(), tab_1, "[+] Add/Edit Address");

        }

        private void btn_Com_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tab_1);
            tabControl1.TabPages.Add(tab_2);
            tabControl1.TabPages.Add(tab_3);

            BindFormToTab(new Form1(), tab_1, "[+] Add New Comm Details");
            BindFormToTab(new IssueCorrection(), tab_2, "[✎] Edit / Update");
            BindFormToTab(new SearchComm(), tab_3, "[🔍] Search Records");

        }

        private void btn_File_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tab_1);
            tabControl1.TabPages.Add(tab_2);
            tabControl1.TabPages.Add(tab_3);

            // Pehle Tab 3 ko select karein (Micro-second ke liye)
            tabControl1.SelectedTab = tab_3;
            BindFormToTab(new searchFile(), tab_3, "[🔍] Search Records");

            // Phir baaki tabs bind karein
            BindFormToTab(new FileCorrection(), tab_2, "[✎] Edit / Update ");
            BindFormToTab(new AddFileNo(), tab_1, "[+] Add New File");

            // Aakhir mein wapas Tab 1 par aa jayein
            tabControl1.SelectedTab = tab_1;


        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            // User se confirm karein
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Application ko restart karne se Main Form band ho jayega 
                // aur Program.cs dobara shuru se chalay ga (yani pehle Login dikhayega)
                Application.Restart();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string accessPath = @"F:\PostalStampBranchData\backend\PostalStampsBranch_be.accdb";
            string accessConStr = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={accessPath};";
            string sqlConStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PSDB;Integrated Security=True;";

            this.Cursor = Cursors.WaitCursor;

            using (OleDbConnection accCon = new(accessConStr))
            using (SqlConnection sqlCon = new(sqlConStr))
            {
                try
                {
                    accCon.Open();
                    sqlCon.Open();

                    // Teeno tables ko बारी-बारी (one by one) migrate karein
                    // NOTE: Agar Access mein table ka naam mukhtalif hai to yahan sahi karein
                    //MigrateTable(accCon, sqlCon, "StockPrice");
                    //MigrateTable(accCon, sqlCon, "StockPhilQuantity");
                    MigrateTable(accCon, sqlCon, "Design");

                    MessageBox.Show("Migration Process Finished! Please check your SQL tables.", "Status");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Error: " + ex.Message);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void MigrateTable(OleDbConnection accCon, SqlConnection sqlCon, string tableName)
        {
            // Access se data read karna
            OleDbCommand accCmd = new($"SELECT * FROM [{tableName}]", accCon);

            try
            {
                using (OleDbDataReader reader = accCmd.ExecuteReader())
                {
                    int rowCount = 0;
                    while (reader.Read())
                    {
                        using (SqlTransaction trans = sqlCon.BeginTransaction())
                        {
                            try
                            {
                                string columns = "";
                                string values = "";
                                SqlCommand sqlCmd = new SqlCommand { Connection = sqlCon, Transaction = trans };

                                // Har column ko dynamic map karna
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string colName = reader.GetName(i);

                                    // Identity (AutoNumber) column ko skip karna
                                    if (colName.ToLower() == "id" || colName.ToLower() == "issueid") continue;

                                    columns += (columns == "" ? "" : ", ") + "[" + colName + "]";
                                    values += (values == "" ? "" : ", ") + "@" + colName;

                                    // Null handling
                                    object val = reader[i] == DBNull.Value ? DBNull.Value : reader[i];
                                    sqlCmd.Parameters.AddWithValue("@" + colName, val);
                                }

                                sqlCmd.CommandText = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values})";
                                sqlCmd.ExecuteNonQuery();
                                trans.Commit();
                                rowCount++;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                // Agar koi specific row fail ho rahi hai to yahan pata chalega
                                MessageBox.Show($"Error in Table [{tableName}] at row {rowCount + 1}: \n\n{ex.Message}");
                                return; // Ek error par ruk jayein taake solve kar saken
                            }
                        }
                    }
                    Debug.WriteLine($"Table {tableName} migrated: {rowCount} rows.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not find or read Access table [{tableName}]. \nError: {ex.Message}");
            }
        }

        private void btn_task_Click(object sender, EventArgs e)
        {

            TabPage todoTab = new("📋 My To-Do List");
            tabControl1.TabPages.Add(todoTab);

            // 2. FormHelper ko use karte hue Todo form ko is tab mein dalkar show karein
            BindFormToTab(new frmDashboard(), todoTab, "📋 My To-Do List");

            // 3. Todo tab ko default select karlein
            tabControl1.SelectedTab = todoTab;

        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Software band hone se pehle backup le lo
            AutoBackup();
        }
    }
}


