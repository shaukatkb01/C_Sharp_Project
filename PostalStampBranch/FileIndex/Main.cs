// ═══════════════════════════════════════════════════════════════════════════════
// IMPROVED & OPTIMIZED Main.cs
// All issues fixed, best practices implemented
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.Data.SqlClient;
using PostalStampSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Microsoft.Reporting.WinForms;



namespace FileIndex
{
    public partial class Main : Form
    {

        private void ExecuteBackup(string folderPath)
        {
            // Apne Database ka sahi naam yahan likhein
            string dbName = "PSDB";
            string fileName = $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string fullPath = Path.Combine(folderPath, fileName);

            // SQL Query for Backup
            string query = $@"BACKUP DATABASE [{dbName}] TO DISK = '{fullPath}'";

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // Backup hone ke baad purani files saaf karein
                    DeleteOldBackups(folderPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Backup Failed: " + ex.Message); // Ye aapko asli wajah batayega
                    LogError(ex, "ExecuteBackup");
                }
            }



        }

        // 3. Purani files (7 days old) delete karne ka function
        private void DeleteOldBackups(string folderPath)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(folderPath);
                FileInfo[] files = info.GetFiles("*.bak"); // Sirf backup files

                foreach (FileInfo file in files)
                {
                    // Agar file 7 din se purani hai
                    if (file.CreationTime < DateTime.Now.AddDays(-7))
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "DeleteOldBackups");
            }
        }

        private void CreateLoadingPanel()
        {
            pnl_Loading = new Panel
            {
                Size = new Size(300, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            Label lblLoading = new Label
            {
                Text = "⏳ Loading...\nPlease Wait",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(0, 25, 0, 0)
            };

            System.Windows.Forms.ProgressBar pbLoading = new System.Windows.Forms.ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Dock = DockStyle.Bottom,
                Height = 30,
                Margin = new Padding(20)
            };

            pnl_Loading.Controls.Add(lblLoading);
            pnl_Loading.Controls.Add(pbLoading);

            this.Controls.Add(pnl_Loading);
            pnl_Loading.BringToFront();
        }

        //✅ Show/Hide
        private void ShowLoading(bool show)
        {
            if (pnl_Loading == null) return;

            if (show)
            {
                pnl_Loading.Left = (this.ClientSize.Width - pnl_Loading.Width) / 2;
                pnl_Loading.Top = (this.ClientSize.Height - pnl_Loading.Height) / 2;
                pnl_Loading.BringToFront();
                pnl_Loading.Visible = true;
                pnl_Loading.Refresh();
                Application.DoEvents();
            }
            else
            {
                pnl_Loading.Visible = false;
            }
        }
        #region Constants

        // ✅ Magic numbers replaced with constants
        private const int SIDEBAR_MAX_WIDTH = 220;
        private const int SIDEBAR_MIN_WIDTH = 50;
        private const int SIDEBAR_ANIMATION_STEP = 40;
        private const int TIMER_THROTTLE_MS = 50;
        private const int BACKUP_RETENTION_DAYS = 7;

        #endregion

        #region Fields

        private DateTime lastTimerCheck = DateTime.MinValue;
        private readonly Dictionary<string, Form> openForms = new Dictionary<string, Form>();

        #endregion

        #region Constructor & Initialization

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            CreateLoadingPanel();
            try
            {
                // 1. Theme setup
                ThemeManager.LoadThemePreference();
                ThemeManager.ApplyTheme(this);
                SetupThemeComboBox();

                // 2. Window setup
                SetupWindow();

                // 3. User info display
                DisplayUserInfo();

                // 4. Load default dashboard
                LoadDefaultDashboard();
            }
            catch (Exception ex)
            {
                LogError(ex, "Main_Load");
                MessageBox.Show($"❌ Error loading form:\n{ex.Message}",
                    "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void SetupWindow()
        {
            // Use a null-safe working area acquisition to avoid CS8602 on Screen.PrimaryScreen
            Rectangle workingArea = GetPrimaryWorkingArea();

            // Set bounds and position using the resolved working area
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(workingArea.Left, workingArea.Top);
            this.Size = new Size(workingArea.Width, workingArea.Height);

            // Preserve original intent to maximize; setting Bounds above then maximizing is fine
            this.WindowState = FormWindowState.Maximized;

            // Guard against designer controls being null (prevents possible dereferences)
            if (pnl_Sidebar != null)
            {
                pnl_Sidebar.Dock = DockStyle.Left;
            }

            if (tabControl1 != null)
            {
                tabControl1.Dock = DockStyle.Fill;
            }
        }

        private Rectangle GetPrimaryWorkingArea()
        {
            // 1) Try the primary screen if available
            try
            {
                if (Screen.PrimaryScreen != null)
                {
                    return Screen.PrimaryScreen.WorkingArea;
                }
            }
            catch
            {
                // swallow - we'll try fallbacks
            }

            // 2) Try deriving a screen from this form if its handle exists
            try
            {
                if (this != null && this.IsHandleCreated)
                {
                    Screen s = Screen.FromControl(this);
                    if (s != null)
                        return s.WorkingArea;
                }
            }
            catch
            {
                // ignore and continue to next fallback
            }

            // 3) Use the first available screen
            try
            {
                Screen any = Screen.AllScreens.FirstOrDefault();
                if (any != null)
                    return any.WorkingArea;
            }
            catch
            {
                // ignore
            }

            // 4) Fallback to a sensible default so caller always receives a valid Rectangle
            return new Rectangle(0, 0, 1024, 768);
        }

        private void DisplayUserInfo()
        {
            if (!string.IsNullOrEmpty(GlobalData.CurrentUser))
            {
                txt_userName.Text = $"Welcome Mr. {GlobalData.CurrentUser} logged in as {GlobalData.UerRole ?? "User"}";
                txt_userName.Visible = true;
            }
            else
            {
                txt_userName.Visible = false;
            }
        }

        private void LoadDefaultDashboard()
        {
            tabControl1.TabPages.Clear();
            TabPage todoTab = new TabPage("📋 My To-Do List");
            tabControl1.TabPages.Add(todoTab);
            BindFormToTab(new frmDashboard(), todoTab, "📋 My To-Do List");
            tabControl1.SelectedTab = todoTab;
        }

        #endregion

        #region Theme Management

        private void SetupThemeComboBox()
        {
            cmbTheme.Items.Clear();
            cmbTheme.Items.Add("🌞 Light Mode");
            cmbTheme.Items.Add("🌙 Dark Mode");
            cmbTheme.SelectedIndex = ThemeManager.CurrentTheme == ThemeManager.ThemeMode.Light ? 0 : 1;
        }

        private void btnApplyTheme_Click(object sender, EventArgs e)
        {
            if (cmbTheme.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Please select a theme first!", "Theme Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ThemeManager.ThemeMode selectedTheme = cmbTheme.SelectedIndex == 0
                ? ThemeManager.ThemeMode.Light
                : ThemeManager.ThemeMode.Dark;

            ThemeManager.ChangeTheme(selectedTheme);

            string themeName = selectedTheme == ThemeManager.ThemeMode.Light ? "Light" : "Dark";
            MessageBox.Show($"✅ {themeName} theme applied successfully!\n\nAll open forms have been updated.",
                "Theme Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Sidebar Animation

        private void timer1_Tick(object sender, EventArgs e)
        {
            // ✅ Throttle timer execution
            if ((DateTime.Now - lastTimerCheck).TotalMilliseconds < TIMER_THROTTLE_MS)
                return;

            lastTimerCheck = DateTime.Now;

            bool isMouseOver = pnl_Sidebar.ClientRectangle.Contains(
                pnl_Sidebar.PointToClient(Control.MousePosition));

            this.SuspendLayout();
            pnl_Sidebar.BringToFront();

            if (isMouseOver)
            {
                ExpandSidebar();
            }
            else
            {
                CollapseSidebar();
            }

            this.ResumeLayout();
        }

        private void ExpandSidebar()
        {
            if (pnl_Sidebar.Width < SIDEBAR_MAX_WIDTH)
            {
                pnl_Sidebar.Width = Math.Min(pnl_Sidebar.Width + SIDEBAR_ANIMATION_STEP, SIDEBAR_MAX_WIDTH);
            }
            else if (string.IsNullOrEmpty(btn_File.Text))
            {
                SetButtonsText(true);
            }
        }

        private void CollapseSidebar()
        {
            if (!string.IsNullOrEmpty(btn_File.Text))
            {
                SetButtonsText(false);
            }

            if (pnl_Sidebar.Width > SIDEBAR_MIN_WIDTH)
            {
                pnl_Sidebar.Width = Math.Max(pnl_Sidebar.Width - SIDEBAR_ANIMATION_STEP, SIDEBAR_MIN_WIDTH);
            }
        }

        private void SetButtonsText(bool show)
        {
            btn_File.Text = show ? "   Add New File" : "";
            btn_Com.Text = show ? "   Add Comm Details" : "";
            btn_Issue.Text = show ? "   Issue Printing Work" : "";
            btn_PhiletalicSupply.Text = show ? "   Philatelic Supply" : "";
            btn_InvoiceWork.Text = show ? "   Invoice Work" : "";
            btn_Admin.Text = show ? "   User Management" : "";
            btn_Address.Text = show ? "   Address Book" : "";
        }

        #endregion

        #region Form Management (Reusable Helper)

        /// <summary>
        /// ✅ Reusable method to show or activate forms
        /// </summary>
        private void ShowOrActivateForm<T>(string formName) where T : Form, new()
        {
            try
            {
                Form frm = Application.OpenForms[formName];

                if (frm == null)
                {
                    T newForm = new T();
                    newForm.MdiParent = this;
                    newForm.WindowState = FormWindowState.Maximized;
                    newForm.Show();

                    // Track form
                    openForms[formName] = newForm;
                }
                else
                {
                    if (frm.WindowState == FormWindowState.Minimized)
                        frm.WindowState = FormWindowState.Normal;

                    frm.Activate();
                    frm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                LogError(ex, $"ShowOrActivateForm<{typeof(T).Name}>");
                MessageBox.Show($"❌ Error opening form:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindFormToTab(Form childForm, TabPage targetTab, string tabHeading)
        {
            try
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                targetTab.Text = tabHeading;
                targetTab.Controls.Clear();
                targetTab.Controls.Add(childForm);

                childForm.Show();
                childForm.CreateControl();
            }
            catch (Exception ex)
            {
                LogError(ex, "BindFormToTab");
                MessageBox.Show($"❌ Error binding form to tab:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Menu Strip Events (Using Helper Method)

        private void addIssueDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<Form1>("Form1");
        }

        private void addFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<AddFileNo>("AddFileNo");
        }

        private void registorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<Register>("Register");
        }

        private void editIssueDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<IssueCorrection>("IssueCorrection");
        }

        private void editFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<FileCorrection>("FileCorrection");
        }

        private void searchFileNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<searchFile>("searchFile");
        }

        private void addDistributionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<DistributionList>("DistributionList");
        }

        #endregion

        #region Sidebar Button Events

        private async void btn_Issue_Click(object sender, EventArgs e)
        {
            try
            {
                ShowLoading(true);
                await Task.Delay(100);

                // ✅ STEP 1: Pehle forms create karo (hidden)
                DistributionList frm1 = null;
                IssuePrinting frm2 = null;
                IssueMaillist frm3 = null;
                SingleMaillist frm4 = null;
                WNS02 frm6 = null;

                // ✅ STEP 2: Forms ko background mein fully load karo
                await Task.Run(() =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        // ✅ Poora form hide rakho during creation
                        this.SuspendLayout();
                        tabControl1.Visible = false; // ← Tab hide karo jab tak ready na ho

                        // Forms create karo
                        frm1 = new DistributionList();
                        frm2 = new IssuePrinting();
                        frm3 = new IssueMaillist();
                        frm4 = new SingleMaillist();
                        frm6 = new WNS02();

                        // Forms ko offscreen load karo
                        frm1.Location = new Point(-9999, -9999);
                        frm2.Location = new Point(-9999, -9999);
                        frm3.Location = new Point(-9999, -9999);
                        frm4.Location = new Point(-9999, -9999);
                        frm6.Location = new Point(-9999, -9999);

                        // Forms ko invisible rakh kar show karo (load trigger)
                        frm1.Opacity = 0; frm1.Show(); frm1.Hide();
                        frm2.Opacity = 0; frm2.Show(); frm2.Hide();
                        frm3.Opacity = 0; frm3.Show(); frm3.Hide();
                        frm4.Opacity = 0; frm4.Show(); frm4.Hide();
                        frm6.Opacity = 0; frm6.Show(); frm6.Hide();
                    });
                });

                // ✅ STEP 3: Ab sab ek saath bind karo
                await Task.Run(() =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Tabs clear aur add karo
                        tabControl1.TabPages.Clear();
                        tabControl1.TabPages.Add(tab_1);
                        tabControl1.TabPages.Add(tab_2);
                        tabControl1.TabPages.Add(tab_3);
                        tabControl1.TabPages.Add(tab_4);
                        tabControl1.TabPages.Add(tab_6);

                        // ✅ Ek saath bind karo
                        BindFormToTab(frm1, tab_1, "[+] Add Supply Distribution");
                        BindFormToTab(frm2, tab_2, "[🖨] Issue Printing Work");
                        BindFormToTab(frm3, tab_3, "[🔍] Issue MailList");
                        BindFormToTab(frm4, tab_4, "Single Mail List");
                        BindFormToTab(frm6, tab_6, "[📊] WNS02 Form");

                        // ✅ Pehla tab select karo
                        tabControl1.SelectedTab = tab_1;
                    });
                });
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_Issue_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // ✅ STEP 4: Sab ready hai, ab show karo
                this.Invoke((MethodInvoker)delegate
                {
                    tabControl1.Visible = true; // ← Ek saath sab dikho!
                    this.ResumeLayout();
                });

                ShowLoading(false);
            }
        }

        private void btn_PhiletalicSupply_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tab_1);
                tabControl1.TabPages.Add(tab_2);
                tabControl1.TabPages.Add(tab_4);
                tabControl1.TabPages.Add(tab_6);

                BindFormToTab(new PhilatelicSupply(), tab_1, "[+] Stamp Phil Supply");
                BindFormToTab(new PhilatelicSupplyDetail(), tab_2, "[🖨] Change Phil Supply");
                BindFormToTab(new AddStationeryItems(), tab_4, "[🖨] Add Stationery Items");
                BindFormToTab(new StationeryTransactionscs(), tab_6, "[🖨] Stationery Issue");
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_PhiletalicSupply_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_InvoiceWork_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tab_1);
                tabControl1.TabPages.Add(tab_2);
                tabControl1.TabPages.Add(tab_3);
                tabControl1.TabPages.Add(tab_4);
                tabControl1.TabPages.Add(tab_6);

                BindFormToTab(new InvoiceEntrycs(), tab_1, "[+] Generate Invoice");
                BindFormToTab(new InvoiceCorection(), tab_2, "[🖨] Invoice Correction");
                BindFormToTab(new InvoiceAcknowlodge(), tab_3, "[🔍] Acknowledge Invoice");
                BindFormToTab(new InvoicePrint(), tab_4, "[🖨] Print Invoice");
                BindFormToTab(new PendingInvoicePrint(), tab_6, "Pending Invoice");
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_InvoiceWork_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Admin_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tab_1);
                tabControl1.TabPages.Add(tab_2);

                BindFormToTab(new UserRoles(), tab_1, "[+] Allow User");
                BindFormToTab(new Register(), tab_2, "[✎] Register User");
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_Admin_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Address_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tab_1);

                BindFormToTab(new Address(), tab_1, "[+] Add/Edit Address");
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_Address_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Com_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tab_1);
                tabControl1.TabPages.Add(tab_2);
                tabControl1.TabPages.Add(tab_3);

                BindFormToTab(new Form1(), tab_1, "[+] Add New Comm Details");
                BindFormToTab(new IssueCorrection(), tab_2, "[✎] Edit / Update");
                BindFormToTab(new SearchComm(), tab_3, "[🔍] Search Records");
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_Com_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_File_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tab_1);
                tabControl1.TabPages.Add(tab_2);
                tabControl1.TabPages.Add(tab_3);

                // 1. Forms ke objects pehle bana lein
                var frmSearch = new searchFile();
                var frmEdit = new FileCorrection();
                var frmAdd = new AddFileNo();

                // 2. Inhein Tabs mein bind karein
                BindFormToTab(frmSearch, tab_3, "[🔍] Search Records");
                BindFormToTab(frmEdit, tab_2, "[✎] Edit / Update");
                BindFormToTab(frmAdd, tab_1, "[+] Add New File");

                // 3. --- JADU KI LINE ---
                // Ye line Windows ko majboor karti hai ke wo invisible tab ke controls ko foran "Zinda" kare
                IntPtr h1 = frmSearch.Handle;
                IntPtr h2 = frmEdit.Handle;
                IntPtr h3 = frmAdd.Handle;

                tabControl1.SelectedTab = tab_1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error: {ex.Message}");
            }
        }

        private void btn_task_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage todoTab = new TabPage("📋 My To-Do List");
                tabControl1.TabPages.Add(todoTab);
                BindFormToTab(new frmDashboard(), todoTab, "📋 My To-Do List");
                tabControl1.SelectedTab = todoTab;
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_task_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Loading Panel

        //private void ShowLoading(bool show)
        //{
        //    if (pnl_Loading == null) return;

        //    if (show)
        //    {
        //        // ✅ PEHLE position set karo
        //        pnl_Loading.Left = (this.ClientSize.Width - pnl_Loading.Width) / 2;
        //        pnl_Loading.Top = (this.ClientSize.Height - pnl_Loading.Height) / 2;

        //        // ✅ PHIR bring to front
        //        pnl_Loading.BringToFront();

        //        // ✅ AB visible karo
        //        pnl_Loading.Visible = true;

        //        // ✅ Force UI update (IMPORTANT!)
        //        pnl_Loading.Refresh();
        //        Application.DoEvents();
        //    }
        //    else
        //    {
        //        pnl_Loading.Visible = false;
        //    }
        //}

        #endregion

        #region Backup System

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select backup location";
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        // Path save karna Settings mein
                        Properties.Settings.Default.BackupFolderPath = fbd.SelectedPath;
                        Properties.Settings.Default.Save();

                        MessageBox.Show("✅ Backup location saved successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "btnBackupLocation_Click");
                MessageBox.Show($"❌ Error: {ex.Message}");
            }
        }

        public async Task ExecuteBackupAsync(string folderPath)
        {
            try
            {
                // ✅ Validate path
                if (!IsValidBackupPath(folderPath))
                {
                    throw new ArgumentException("Invalid backup path");
                }

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = $"ManualBackup_{DateTime.Now:yyyy-MM-dd_HHmm}.bak";
                string fullPath = Path.Combine(folderPath, fileName);

                // ✅ Async database backup
                await Task.Run(() =>
                {
                    string query = $"BACKUP DATABASE [PSDB] TO DISK = '{fullPath}' WITH FORMAT";

                    using (SqlConnection con = new SqlConnection(Db.ConString))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandTimeout = 300; // 5 minutes
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                });

                MessageBox.Show($"✅ Backup saved successfully!\n\nPath: {fullPath}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogError(ex, "ExecuteBackupAsync");
                MessageBox.Show($"❌ Backup failed:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task RunAutoBackupAsync(string folderPath)
        {
            try
            {
                if (!IsValidBackupPath(folderPath)) return;
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                // ✅ Delete old backups
                await Task.Run(() =>
                {
                    DirectoryInfo d = new DirectoryInfo(folderPath);
                    foreach (FileInfo file in d.GetFiles("AutoBackup_*.bak"))
                    {
                        if (file.LastWriteTime < DateTime.Now.AddDays(-BACKUP_RETENTION_DAYS))
                        {
                            file.Delete();
                        }
                    }
                });

                // ✅ Create new backup
                string fileName = $"AutoBackup_{DateTime.Now:yyyy-MM-dd_HHmm}.bak";
                string fullPath = Path.Combine(folderPath, fileName);
                string query = $"BACKUP DATABASE [PSDB] TO DISK = '{fullPath}' WITH FORMAT";

                await Task.Run(() =>
                {
                    using (SqlConnection con = new SqlConnection(Db.ConString))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandTimeout = 300;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                });
            }
            catch (Exception ex)
            {
                LogError(ex, "RunAutoBackupAsync");
            }
        }

        private bool IsValidBackupPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;
            if (path.Contains("..")) return false; // Prevent directory traversal
            if (!Path.IsPathRooted(path)) return false;

            try
            {
                Path.GetFullPath(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Data Migration

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Access Database|*.accdb;*.mdb";
                    ofd.Title = "Select Access Database";

                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;

                    string accessPath = ofd.FileName;
                    string accessConStr = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={accessPath};";

                    this.Cursor = Cursors.WaitCursor;

                    await Task.Run(() =>
                    {
                        using (OleDbConnection accCon = new OleDbConnection(accessConStr))
                        using (SqlConnection sqlCon = new SqlConnection(Db.ConString))
                        {
                            accCon.Open();
                            sqlCon.Open();

                            MigrateTable(accCon, sqlCon, "PhilitelicBuearu");
                        }
                    });

                    MessageBox.Show("✅ Migration completed successfully!\n\nHTML tags have been cleaned.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "button1_Click");
                MessageBox.Show($"❌ Migration error:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void MigrateTable(OleDbConnection accCon, SqlConnection sqlCon, string tableName)
        {
            OleDbCommand accCmd = new OleDbCommand($"SELECT * FROM [{tableName}]", accCon);

            try
            {
                using (OleDbDataReader reader = accCmd.ExecuteReader())
                {
                    int rowCount = 0;

                    // ✅ Single transaction for all rows
                    using (SqlTransaction trans = sqlCon.BeginTransaction())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                StringBuilder columns = new StringBuilder();
                                StringBuilder values = new StringBuilder();
                                SqlCommand sqlCmd = new SqlCommand { Connection = sqlCon, Transaction = trans };

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string colName = reader.GetName(i);

                                    // Skip identity columns
                                    if (colName.Equals("id", StringComparison.OrdinalIgnoreCase) ||
                                        colName.Equals("issueid", StringComparison.OrdinalIgnoreCase))
                                        continue;

                                    if (columns.Length > 0)
                                    {
                                        columns.Append(", ");
                                        values.Append(", ");
                                    }

                                    columns.Append($"[{colName}]");
                                    values.Append($"@{colName}");

                                    // ✅ HTML cleaning for string fields
                                    object val = reader[i] == DBNull.Value
                                        ? DBNull.Value
                                        : reader.GetFieldType(i) == typeof(string)
                                            ? StripHTML(reader[i].ToString())
                                            : reader[i];

                                    sqlCmd.Parameters.AddWithValue($"@{colName}", val);
                                }

                                sqlCmd.CommandText = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values})";
                                sqlCmd.ExecuteNonQuery();
                                rowCount++;
                            }

                            trans.Commit();
                            Debug.WriteLine($"Table {tableName} migrated: {rowCount} rows.");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception($"Error at row {rowCount + 1}: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not migrate table [{tableName}]: {ex.Message}", ex);
            }
        }

        private string StripHTML(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string result = input.Replace("<br>", Environment.NewLine)
                                .Replace("<div>", Environment.NewLine)
                                .Replace("</div>", "");

            result = Regex.Replace(result, "<.*?>", string.Empty);
            result = result.Replace("&nbsp;", " ").Replace("&amp;", "&");

            return result.Trim();
        }

        #endregion

        #region Logout & Exit

        private void btn_logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        #endregion

        #region Form Closing

        private async void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            string savedPath = Properties.Settings.Default.BackupFolderPath;

            if (!string.IsNullOrEmpty(savedPath) && Directory.Exists(savedPath))
            {
                // Program band hone se pehle backup lega
                ExecuteBackup(savedPath);
            }
        }

        #endregion

        #region Logging

        private void LogError(Exception ex, string source = "")
        {
            try
            {
                string logDir = Path.Combine(Application.StartupPath, "Logs");
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                string logFile = Path.Combine(logDir, $"error_{DateTime.Now:yyyy-MM-dd}.log");

                string logEntry = $@"
═══════════════════════════════════════════════════
Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
Source: {source}
User: {GlobalData.CurrentUser ?? "Unknown"}
Message: {ex.Message}
Stack Trace:
{ex.StackTrace}
═══════════════════════════════════════════════════
";

                File.AppendAllText(logFile, logEntry);
            }
            catch
            {
                // Silent fail - don't break app if logging fails
            }
        }

        #endregion



        private void btn_Items_Click(object sender, EventArgs e)
        {

            try
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tab_1);
                tabControl1.TabPages.Add(tab_2);
                tabControl1.TabPages.Add(tab_3);

                BindFormToTab(new AddItems(), tab_1, "[+] Add New Items");
                BindFormToTab(new IssueCorrection(), tab_2, "[✎] Edit / Update");
                BindFormToTab(new SearchComm(), tab_3, "[🔍] Search Records");
            }
            catch (Exception ex)
            {
                LogError(ex, "btn_Com_Click");
                MessageBox.Show($"❌ Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


          
        }
    }
}