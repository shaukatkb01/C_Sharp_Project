using SupplyBranch.Controls;
using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmLogin : Form
    {
        // ---- Controls ----
        private Panel pnlLeft;
        private ThemedPanel pnlRight;
        private CardPanel card;
        private Panel pnlAccent;

        private Label lblBrand;
        private Label lblTag;
        private Label lblFeature1Icon;
        private Label lblFeature1;
        private Label lblFeature2Icon;
        private Label lblFeature2;
        private Label lblFeature3Icon;
        private Label lblFeature3;
        private Panel pnlBrandFooter;
        private Label lblCopyright;
        private Label lblBrandVersion;

        private ThemedLabel lblWelcome;
        private ThemedLabel lblSub;
        private ThemedLabel lblUser;
        private ThemedLabel lblPass;
        private ThemedLabel lblError;
        private ThemedLabel lblHelp;
        private ThemedLabel lblVersion;

        private InputBox txtUserName;
        private InputBox txtPassword;
        private ModernButton btnLogin;
        private ModernButton btnExit;
        private ModernButton btnTheme;
        private IconBadge badgeLogo;

        public frmLogin()
        {
            InitializeComponent();

            // Skip custom styling inside the Visual Studio designer
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                ApplyLoginStyle();
                LoadUsernameSuggestions();
                AppTheme.ThemeChanged += HandleThemeChanged;

                this.ActiveControl = txtUserName;
                txtUserName.FocusInput();
            }
        }

        // =====================================================================
        // Layout
        // =====================================================================
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ---------------- Form ----------------
            this.ClientSize = new Size(960, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Supply Branch - Login";
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.DoubleBuffered = true;

            // ---------------- Left brand panel ----------------
            this.pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 420,
                Name = "pnlLeft"
            };
            this.pnlLeft.Paint += PaintBrandPanel;

            this.badgeLogo = new IconBadge
            {
                Glyph = ThemeIcons.Package,
                Accent = ColorTranslator.FromHtml("#2563EB"),
                Location = new Point(48, 56),
                Size = new Size(56, 56),
                Radius = 16
            };

            this.lblBrand = new Label
            {
                AutoSize = true,
                Location = new Point(46, 132),
                Text = "Supply Branch",
                UseMnemonic = false,
                BackColor = Color.Transparent,
                ForeColor = Color.White
            };

            this.lblTag = new Label
            {
                AutoSize = true,
                Location = new Point(48, 176),
                Text = "Inventory & Supply Management",
                UseMnemonic = false,
                BackColor = Color.Transparent,
                ForeColor = ColorTranslator.FromHtml("#B9C6DA")
            };

            this.pnlAccent = new Panel
            {
                Location = new Point(48, 214),
                Size = new Size(64, 4),
                BackColor = ColorTranslator.FromHtml("#2563EB")
            };

            this.lblFeature1Icon = CreateFeatureIcon(ThemeIcons.Document, 272);
            this.lblFeature1 = CreateFeatureText("Indent & supply tracking", 272);
            this.lblFeature2Icon = CreateFeatureIcon(ThemeIcons.Package, 320);
            this.lblFeature2 = CreateFeatureText("Real-time stock control", 320);
            this.lblFeature3Icon = CreateFeatureIcon(ThemeIcons.Chart, 368);
            this.lblFeature3 = CreateFeatureText("Powerful reporting", 368);

            this.pnlBrandFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 74,
                BackColor = Color.Transparent
            };

            this.lblCopyright = new Label
            {
                AutoSize = true,
                Location = new Point(48, 18),
                Text = "© 2026 Supply Branch",
                BackColor = Color.Transparent,
                ForeColor = ColorTranslator.FromHtml("#7F8FA8")
            };

            this.lblBrandVersion = new Label
            {
                AutoSize = true,
                Location = new Point(48, 40),
                Text = "Version " + AppVersion.CurrentVersion,
                BackColor = Color.Transparent,
                ForeColor = ColorTranslator.FromHtml("#7F8FA8")
            };

            this.pnlBrandFooter.Controls.Add(this.lblCopyright);
            this.pnlBrandFooter.Controls.Add(this.lblBrandVersion);

            this.pnlLeft.Controls.Add(this.badgeLogo);
            this.pnlLeft.Controls.Add(this.lblBrand);
            this.pnlLeft.Controls.Add(this.lblTag);
            this.pnlLeft.Controls.Add(this.pnlAccent);
            this.pnlLeft.Controls.Add(this.lblFeature1Icon);
            this.pnlLeft.Controls.Add(this.lblFeature1);
            this.pnlLeft.Controls.Add(this.lblFeature2Icon);
            this.pnlLeft.Controls.Add(this.lblFeature2);
            this.pnlLeft.Controls.Add(this.lblFeature3Icon);
            this.pnlLeft.Controls.Add(this.lblFeature3);
            this.pnlLeft.Controls.Add(this.pnlBrandFooter);

            // ---------------- Right panel ----------------
            this.pnlRight = new ThemedPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Background,
                Name = "pnlRight"
            };
            this.pnlRight.Resize += (s, e) => CenterCard();

            this.card = new CardPanel
            {
                Size = new Size(400, 520),
                CornerRadius = 18,
                Padding = new Padding(0)
            };

            this.btnTheme = new ModernButton
            {
                Variant = ButtonVariant.Ghost,
                IconGlyph = AppTheme.IsDark ? ThemeIcons.Sun : ThemeIcons.Moon,
                Size = new Size(36, 36),
                Location = new Point(348, 14),
                CornerRadius = 9,
                TabStop = false
            };
            this.btnTheme.Click += (s, e) => AppTheme.Toggle();

            this.lblWelcome = new ThemedLabel
            {
                Text = "Welcome back",
                Role = LabelRole.Primary,
                Font = ThemeFonts.H1,
                AutoSize = true,
                Location = new Point(32, 44)
            };

            this.lblSub = new ThemedLabel
            {
                Text = "Sign in to your account to continue",
                Role = LabelRole.Muted,
                Font = ThemeFonts.Body,
                AutoSize = true,
                Location = new Point(32, 88)
            };

            this.lblUser = new ThemedLabel
            {
                Text = "User name",
                Role = LabelRole.Secondary,
                Font = ThemeFonts.BodyBold,
                AutoSize = true,
                Location = new Point(32, 140)
            };

            this.txtUserName = new InputBox
            {
                Location = new Point(32, 162),
                Size = new Size(336, 46),
                IconGlyph = ThemeIcons.User,
                Placeholder = "Enter your user name"
            };

            this.lblPass = new ThemedLabel
            {
                Text = "Password",
                Role = LabelRole.Secondary,
                Font = ThemeFonts.BodyBold,
                AutoSize = true,
                Location = new Point(32, 224)
            };

            this.txtPassword = new InputBox
            {
                Location = new Point(32, 246),
                Size = new Size(336, 46),
                IconGlyph = ThemeIcons.Lock,
                Placeholder = "Enter your password",
                ShowReveal = true,
                UseSystemPasswordChar = true
            };
            this.txtPassword.InnerTextBox.KeyUp += txtPassword_KeyUp;

            this.lblError = new ThemedLabel
            {
                Text = string.Empty,
                Role = LabelRole.Danger,
                Font = ThemeFonts.Small,
                AutoSize = false,
                Location = new Point(32, 302),
                Size = new Size(336, 20)
            };

            this.btnLogin = new ModernButton
            {
                Text = "Sign In",
                Variant = ButtonVariant.Primary,
                Location = new Point(32, 332),
                Size = new Size(336, 46),
                CornerRadius = 10,
                Font = ThemeFonts.Button
            };
            this.btnLogin.Click += btnLogin_Click;

            this.btnExit = new ModernButton
            {
                Text = "Exit",
                Variant = ButtonVariant.Ghost,
                Location = new Point(32, 388),
                Size = new Size(336, 40),
                CornerRadius = 10,
                DialogResult = DialogResult.Cancel
            };
            this.btnExit.Click += btnExit_Click;

            this.lblHelp = new ThemedLabel
            {
                Text = "Need help? Contact your administrator.",
                Role = LabelRole.Muted,
                Font = ThemeFonts.Tiny,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(32, 440),
                Size = new Size(336, 20)
            };

            this.lblVersion = new ThemedLabel
            {
                Text = "Supply Branch v" + AppVersion.CurrentVersion,
                Role = LabelRole.Muted,
                Font = ThemeFonts.Tiny,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(32, 468),
                Size = new Size(336, 20)
            };

            this.card.Controls.Add(this.btnTheme);
            this.card.Controls.Add(this.lblWelcome);
            this.card.Controls.Add(this.lblSub);
            this.card.Controls.Add(this.lblUser);
            this.card.Controls.Add(this.txtUserName);
            this.card.Controls.Add(this.lblPass);
            this.card.Controls.Add(this.txtPassword);
            this.card.Controls.Add(this.lblError);
            this.card.Controls.Add(this.btnLogin);
            this.card.Controls.Add(this.btnExit);
            this.card.Controls.Add(this.lblHelp);
            this.card.Controls.Add(this.lblVersion);

            this.pnlRight.Controls.Add(this.card);

            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);

            this.AcceptButton = this.btnLogin;
            this.CancelButton = this.btnExit;

            this.ResumeLayout(false);
        }

        private static Label CreateFeatureIcon(string glyph, int top)
        {
            return new Label
            {
                AutoSize = false,
                Size = new Size(24, 24),
                Location = new Point(48, top),
                Text = glyph,
                Font = ThemeFonts.Icon,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                ForeColor = ColorTranslator.FromHtml("#93C5FD")
            };
        }

        private static Label CreateFeatureText(string text, int top)
        {
            return new Label
            {
                AutoSize = true,
                Location = new Point(80, top + 3),
                Text = text,
                UseMnemonic = false,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.Transparent,
                ForeColor = ColorTranslator.FromHtml("#C6D2E3")
            };
        }

        private void CenterCard()
        {
            if (card == null || pnlRight == null)
                return;

            int x = Math.Max(0, (pnlRight.ClientSize.Width - card.Width) / 2);
            int y = Math.Max(0, (pnlRight.ClientSize.Height - card.Height) / 2);
            card.Location = new Point(x, y);
        }

        private void PaintBrandPanel(object sender, PaintEventArgs e)
        {
            Rectangle bounds = pnlLeft.ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                bounds,
                ColorTranslator.FromHtml("#0B1526"),
                ColorTranslator.FromHtml("#16295C"),
                135F))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush soft = new SolidBrush(Color.FromArgb(14, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(soft, bounds.Width - 210, -90, 320, 320);
                e.Graphics.FillEllipse(soft, -120, bounds.Height - 190, 280, 280);
            }

            using (SolidBrush accent = new SolidBrush(Color.FromArgb(28, 37, 99, 235)))
            {
                e.Graphics.FillEllipse(accent, bounds.Width - 120, bounds.Height - 140, 220, 220);
            }
        }

        // =====================================================================
        // Runtime styling
        // =====================================================================
        private void ApplyLoginStyle()
        {
            ThemePalette p = AppTheme.P;

            this.BackColor = p.Background;
            this.Font = ThemeFonts.Body;

            // ----- Left brand panel -----
            lblBrand.Font = new Font("Segoe UI Semibold", 27F);
            lblBrand.ForeColor = Color.White;

            lblTag.Font = new Font("Segoe UI", 10.5F);
            lblTag.ForeColor = ColorTranslator.FromHtml("#B9C6DA");

            lblCopyright.Font = ThemeFonts.Small;
            lblBrandVersion.Font = ThemeFonts.Small;

            // ----- Right panel -----
            pnlRight.BackColor = p.Background;

            // ----- Card labels -----
            lblWelcome.Font = ThemeFonts.H1;
            lblSub.Font = ThemeFonts.Body;
            lblHelp.Font = ThemeFonts.Tiny;
            lblVersion.Font = ThemeFonts.Tiny;

            // ----- Buttons -----
            btnLogin.Font = new Font("Segoe UI Semibold", 10.5F);
            btnExit.Font = new Font("Segoe UI Semibold", 9.5F);

            btnTheme.IconGlyph = AppTheme.IsDark ? ThemeIcons.Sun : ThemeIcons.Moon;

            // ----- Input events -----
            CenterCard();
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            ApplyLoginStyle();
            pnlLeft.Invalidate();
            Invalidate(true);
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

                txtUserName.InnerTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtUserName.InnerTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtUserName.InnerTextBox.AutoCompleteCustomSource = collection;
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

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock) && string.IsNullOrEmpty(lblError.Text))
            {
                lblError.ForeColor = AppTheme.P.TextMuted;
                lblError.Text = "Caps Lock is on";
            }
            else if (!Control.IsKeyLocked(Keys.CapsLock) && lblError.Text == "Caps Lock is on")
            {
                lblError.Text = "";
            }
        }

        private void ShowError(string message, Control focusTarget = null)
        {
            lblError.ForeColor = AppTheme.P.Danger;
            lblError.Text = message;

            if (focusTarget is InputBox)
                ((InputBox)focusTarget).FocusInput();
            else if (focusTarget != null)
                focusTarget.Focus();
        }

        private void SetBusy(bool busy)
        {
            btnLogin.Enabled = !busy;
            btnExit.Enabled = !busy;
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

            if (txtUserName.Value.Trim() == "")
            {
                ShowError("Please enter your user name.", txtUserName);
                return;
            }

            if (txtPassword.Value.Trim() == "")
            {
                ShowError("Please enter your password.", txtPassword);
                return;
            }

            User user = new User
            {
                UserName = txtUserName.Value.Trim(),
                Password = txtPassword.Value
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
                SaveUsernameToHistory(user.UserName);

                // Stop listening for theme changes while this form is hidden.
                AppTheme.ThemeChanged -= HandleThemeChanged;

                frmMain frm = new frmMain();
                frm.FormClosed += (s, a) => Application.Exit();
                frm.Show();
                this.Hide();
            }
            else
            {
                ShowError("Invalid user name or password.");
                txtPassword.Value = string.Empty;
                txtPassword.FocusInput();
            }
        }
    }
}
