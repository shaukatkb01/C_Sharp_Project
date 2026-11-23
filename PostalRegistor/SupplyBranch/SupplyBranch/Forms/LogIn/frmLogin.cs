using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmLogin : Form
    {
        // ---- Theme colors (used only by ApplyLoginStyle, never by InitializeComponent) ----
        private static readonly Color Primary = ColorTranslator.FromHtml("#2563EB");
        private static readonly Color PrimaryHover = ColorTranslator.FromHtml("#1D4ED8");
        private static readonly Color Dark = ColorTranslator.FromHtml("#0F172A");
        private static readonly Color TextMain = ColorTranslator.FromHtml("#1E293B");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#64748B");
        private static readonly Color BorderColor = ColorTranslator.FromHtml("#CBD5E1");
        private static readonly Color ErrorColor = ColorTranslator.FromHtml("#DC2626");

        // ---- Controls ----
        private Panel pnlLeft;
        private Panel pnlAccent;
        private Panel pnlRight;
        private Panel pnlUser;
        private Panel pnlPass;
        private Label lblBrand;
        private Label lblTag;
        private Label lblWelcome;
        private Label lblSub;
        private Label lblUser;
        private Label lblPass;
        private Label lblError;
        private TextBox txtUserName;
        private TextBox txtPassword;
        private CheckBox chkShow;
        private Button btnLogin;
        private Button btnExit;

        public frmLogin()
        {
            InitializeComponent();

            // Skip custom styling inside the Visual Studio designer
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                ApplyLoginStyle();
                LoadUsernameSuggestions();
            }
        }

        // =====================================================================
        // DESIGNER-FRIENDLY: plain object creation + literal properties only.
        // No lambdas, no helper methods, no custom Font/Color objects here.
        // =====================================================================
        private void InitializeComponent()
        {
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlAccent = new System.Windows.Forms.Panel();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblTag = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.pnlPass = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkShow = new System.Windows.Forms.CheckBox();
            this.lblError = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlPass.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlAccent);
            this.pnlLeft.Controls.Add(this.lblBrand);
            this.pnlLeft.Controls.Add(this.lblTag);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(300, 460);
            this.pnlLeft.TabIndex = 1;
            // 
            // pnlAccent
            // 
            this.pnlAccent.Location = new System.Drawing.Point(35, 136);
            this.pnlAccent.Name = "pnlAccent";
            this.pnlAccent.Size = new System.Drawing.Size(48, 4);
            this.pnlAccent.TabIndex = 0;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(32, 150);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(76, 13);
            this.lblBrand.TabIndex = 1;
            this.lblBrand.Text = "Supply Branch";
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(35, 206);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(154, 13);
            this.lblTag.TabIndex = 2;
            this.lblTag.Text = "Inventory & Supply Management";
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.lblWelcome);
            this.pnlRight.Controls.Add(this.lblSub);
            this.pnlRight.Controls.Add(this.lblUser);
            this.pnlRight.Controls.Add(this.pnlUser);
            this.pnlRight.Controls.Add(this.lblPass);
            this.pnlRight.Controls.Add(this.pnlPass);
            this.pnlRight.Controls.Add(this.chkShow);
            this.pnlRight.Controls.Add(this.lblError);
            this.pnlRight.Controls.Add(this.btnLogin);
            this.pnlRight.Controls.Add(this.btnExit);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(300, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(460, 460);
            this.pnlRight.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(50, 40);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(79, 13);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome back";
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Location = new System.Drawing.Point(53, 84);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(95, 13);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Sign in to continue";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(50, 132);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(60, 13);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "User Name";
            // 
            // pnlUser
            // 
            this.pnlUser.Controls.Add(this.txtUserName);
            this.pnlUser.Location = new System.Drawing.Point(50, 154);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(360, 40);
            this.pnlUser.TabIndex = 3;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Location = new System.Drawing.Point(10, 11);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(340, 13);
            this.txtUserName.TabIndex = 0;
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(50, 212);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(53, 13);
            this.lblPass.TabIndex = 4;
            this.lblPass.Text = "Password";
            // 
            // pnlPass
            // 
            this.pnlPass.Controls.Add(this.txtPassword);
            this.pnlPass.Location = new System.Drawing.Point(50, 234);
            this.pnlPass.Name = "pnlPass";
            this.pnlPass.Size = new System.Drawing.Size(360, 40);
            this.pnlPass.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Location = new System.Drawing.Point(10, 11);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(340, 13);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // chkShow
            // 
            this.chkShow.AutoSize = true;
            this.chkShow.Location = new System.Drawing.Point(50, 284);
            this.chkShow.Name = "chkShow";
            this.chkShow.Size = new System.Drawing.Size(101, 17);
            this.chkShow.TabIndex = 0;
            this.chkShow.TabStop = false;
            this.chkShow.Text = "Show password";
            this.chkShow.UseVisualStyleBackColor = true;
            // 
            // lblError
            // 
            this.lblError.Location = new System.Drawing.Point(50, 312);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(360, 20);
            this.lblError.TabIndex = 7;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(50, 342);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(360, 44);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Sign In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(50, 396);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(360, 34);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(760, 460);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Supply Branch - Login";
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlPass.ResumeLayout(false);
            this.pnlPass.PerformLayout();
            this.ResumeLayout(false);

        }

        // =====================================================================
        // Runtime styling (not parsed by the designer)
        // =====================================================================
        private void ApplyLoginStyle()
        {
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            // ----- Left brand panel -----
            pnlLeft.BackColor = Dark;
            pnlAccent.BackColor = Primary;

            lblBrand.Font = new Font("Segoe UI Semibold", 26F);
            lblBrand.ForeColor = Color.White;
            lblBrand.BackColor = Color.Transparent;

            lblTag.Font = new Font("Segoe UI", 11F);
            lblTag.ForeColor = ColorTranslator.FromHtml("#CBD5E1");
            lblTag.BackColor = Color.Transparent;

            // ----- Right panel -----
            pnlRight.BackColor = Color.White;

            lblWelcome.Font = new Font("Segoe UI Semibold", 22F);
            lblWelcome.ForeColor = TextMain;

            lblSub.Font = new Font("Segoe UI", 11F);
            lblSub.ForeColor = TextMuted;

            lblUser.Font = new Font("Segoe UI Semibold", 9.5F);
            lblUser.ForeColor = TextMain;
            lblPass.Font = new Font("Segoe UI Semibold", 9.5F);
            lblPass.ForeColor = TextMain;

            StyleInput(pnlUser, txtUserName);
            StyleInput(pnlPass, txtPassword);

            chkShow.Font = new Font("Segoe UI", 9F);
            chkShow.ForeColor = TextMuted;
            chkShow.Cursor = Cursors.Hand;
            chkShow.CheckedChanged += chkShow_CheckedChanged;

            lblError.Font = new Font("Segoe UI", 9F);
            lblError.ForeColor = ErrorColor;

            // Sign In button
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.BackColor = Primary;
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI Semibold", 11F);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseOverBackColor = PrimaryHover;
            btnLogin.FlatAppearance.MouseDownBackColor = PrimaryHover;

            // Exit button
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.UseVisualStyleBackColor = false;
            btnExit.BackColor = Color.White;
            btnExit.ForeColor = TextMuted;
            btnExit.Font = new Font("Segoe UI", 10F);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#F1F5F9");

            txtPassword.KeyUp += txtPassword_KeyUp;

            this.ActiveControl = txtUserName;
        }

        // =====================================================================
        // Saved Username Suggestions (AutoComplete)
        // =====================================================================
        private string GetUserHistoryFilePath()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SupplyBranch"
            );

            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "saved_usernames.txt");
        }

        private void LoadUsernameSuggestions()
        {
            try
            {
                string filePath = GetUserHistoryFilePath();
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

                if (File.Exists(filePath))
                {
                    string[] savedUsers = File.ReadAllLines(filePath)
                        .Select(u => u.Trim())
                        .Where(u => !string.IsNullOrWhiteSpace(u))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    collection.AddRange(savedUsers);
                }

                txtUserName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtUserName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtUserName.AutoCompleteCustomSource = collection;
            }
            catch
            {
                // Agar file read karne mein koi masla aaye toh login form na ruke
            }
        }

        private void SaveUsernameToHistory(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return;

            try
            {
                string filePath = GetUserHistoryFilePath();
                List<string> usernames = new List<string>();

                if (File.Exists(filePath))
                {
                    usernames = File.ReadAllLines(filePath)
                        .Select(u => u.Trim())
                        .Where(u => !string.IsNullOrWhiteSpace(u))
                        .ToList();
                }

                string cleanName = username.Trim();
                bool alreadyExists = usernames.Any(u =>
                    string.Equals(u, cleanName, StringComparison.OrdinalIgnoreCase));

                if (!alreadyExists)
                {
                    usernames.Add(cleanName);
                    File.WriteAllLines(filePath, usernames);
                }
            }
            catch
            {
                // Silent ignore taake login flow disturb na ho
            }
        }

        private void StyleInput(Panel pnl, TextBox tb)
        {
            pnl.BackColor = Color.White;
            tb.BackColor = Color.White;
            tb.ForeColor = TextMain;
            tb.Font = new Font("Segoe UI", 11F);
            tb.BorderStyle = BorderStyle.None;
            tb.Top = (pnl.Height - tb.Height) / 2;

            pnl.Paint += InputPanel_Paint;
            pnl.Click += (s, e) => tb.Focus();
            tb.Enter += Input_FocusChanged;
            tb.Leave += Input_FocusChanged;
        }

        private void InputPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = (Panel)sender;
            bool focused = false;
            foreach (Control c in pnl.Controls)
            {
                if (c.Focused) focused = true;
            }

            using (Pen pen = new Pen(focused ? Primary : BorderColor, focused ? 2 : 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            }
        }

        private void Input_FocusChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null && c.Parent != null) c.Parent.Invalidate();
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShow.Checked;
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock) && string.IsNullOrEmpty(lblError.Text))
            {
                lblError.ForeColor = TextMuted;
                lblError.Text = "Caps Lock is on";
            }
            else if (!Control.IsKeyLocked(Keys.CapsLock) && lblError.Text == "Caps Lock is on")
            {
                lblError.Text = "";
            }
        }

        private void ShowError(string message, Control focusTarget = null)
        {
            lblError.ForeColor = ErrorColor;
            lblError.Text = message;
            if (focusTarget != null) focusTarget.Focus();
        }

        private void SetBusy(bool busy)
        {
            btnLogin.Enabled = !busy;
            txtUserName.Enabled = !busy;
            txtPassword.Enabled = !busy;
            btnLogin.Text = busy ? "Signing in..." : "Sign In";
            this.Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
        }

        // =====================================================================
        // Events
        // =====================================================================
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (txtUserName.Text.Trim() == "")
            {
                ShowError("Please enter your user name.", txtUserName);
                return;
            }

            if (txtPassword.Text.Trim() == "")
            {
                ShowError("Please enter your password.", txtPassword);
                return;
            }

            User user = new User
            {
                UserName = txtUserName.Text.Trim(),
                Password = txtPassword.Text
            };

            SetBusy(true);
            bool success;

            try
            {
                // DB call on a background thread so the form doesn't freeze
                success = await Task.Run(() => new UserDAL().Login(user));
            }
            catch (Exception)
            {
                SetBusy(false);
                ShowError("Could not sign in. Please check your connection and try again.");
                return;
            }

            SetBusy(false);

            if (success)
            {
                CurrentUser.FullName = user.UserName;
                SaveUsernameToHistory(user.UserName);

                frmMain frm = new frmMain();
                frm.FormClosed += (s, a) => Application.Exit();
                frm.Show();
                this.Hide();
            }
            else
            {
                ShowError("Invalid user name or password.");
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}