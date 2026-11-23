using SupplyBranch.Classes;
using SupplyBranch.Controls;
using SupplyBranch.DataAccess;
using SupplyBranch.Forms.Administration;
using SupplyBranch.Forms.Help;
using SupplyBranch.Forms.Masters;
using SupplyBranch.Forms.Reports;
using SupplyBranch.Forms.Stock;
using SupplyBranch.Forms.Transactions;
using SupplyBranch.Helpers;
using StampStoreApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmMain : Form
    {
        private readonly List<SidebarButton> navButtons = new List<SidebarButton>();
        private readonly ModuleDAL moduleDAL = new ModuleDAL();

        private TableLayoutPanel rootLayout;
        private TableLayoutPanel rightLayout;
        private ThemedPanel pnlSidebar;
        private ScrollHostPanel pnlNavHost;
        private ThemedPanel pnlHeader;
        private ThemedPanel pnlContentHost;
        private ThemedLabel lblPageTitle;
        private ThemedLabel lblPageSubtitle;
        private ThemedLabel lblHeaderUser;
        private ThemedLabel lblHeaderMeta;
        private ThemedLabel lblSidebarUser;
        private Panel pnlHeaderUser;
        private ThemedLabel lblHeaderRole;
        private Panel pnlSidebarDivider;
        private ModernButton btnThemeToggle;
        private Timer clockTimer;

        public frmMain()
        {
            InitializeComponent();
            ClearMenuShortcuts(menuStrip1.Items);

            BuildShell();

            this.KeyPreview = true;
            this.KeyDown += frmMain_KeyDown;

            AppTheme.ThemeChanged += HandleThemeChanged;
            this.Disposed += FrmMain_Disposed;
        }

        private void FrmMain_Disposed(object sender, EventArgs e)
        {
            if (clockTimer != null)
            {
                clockTimer.Stop();
                clockTimer.Dispose();
            }

            AppTheme.ThemeChanged -= HandleThemeChanged;
        }

        // =============================================================
        //  SHELL (sidebar + header + content host)
        // =============================================================

        private void BuildShell()
        {
            SuspendLayout();

            // Detach the legacy chrome but keep menuStrip1 alive because the
            // existing permission logic still walks its menu items.
            Controls.Remove(menuStrip1);
            Controls.Remove(statusStrip1);
            Controls.Remove(lblMain);
            Controls.Remove(pnlMain);

            menuStrip1.Visible = false;
            statusStrip1.Visible = false;
            lblMain.Visible = false;

            BackColor = AppTheme.P.Background;
            ForeColor = AppTheme.P.TextPrimary;
            Font = ThemeFonts.Body;

            rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = AppTheme.P.Background,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 252F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            rootLayout.Controls.Add(BuildSidebar(), 0, 0);

            rightLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = AppTheme.P.Background,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            rightLayout.Controls.Add(BuildHeader(), 0, 0);

            pnlContentHost = new ThemedPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Background,
                Padding = new Padding(20, 18, 20, 18),
                Margin = Padding.Empty
            };

            pnlMain.Dock = DockStyle.Fill;
            pnlMain.AutoSize = false;
            pnlMain.AutoScroll = true;
            pnlMain.BorderStyle = BorderStyle.None;
            pnlMain.BackColor = AppTheme.P.Background;
            pnlMain.Margin = Padding.Empty;

            pnlContentHost.Controls.Add(pnlMain);
            rightLayout.Controls.Add(pnlContentHost, 0, 1);

            rootLayout.Controls.Add(rightLayout, 1, 0);
            Controls.Add(rootLayout);

            ResumeLayout(true);
        }

        private Control BuildSidebar()
        {
            pnlSidebar = new ThemedPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Sidebar,
                Margin = Padding.Empty
            };

            ThemedTable layout = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Sidebar,
                ColumnCount = 1,
                RowCount = 3,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 138F));

            layout.Controls.Add(BuildBrand(), 0, 0);

            // Scrollable navigation area
            pnlNavHost = new ScrollHostPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Sidebar,
                Margin = Padding.Empty
            };

            ThemedTable navStack = new ThemedTable
            {
                Dock = DockStyle.None,
                AutoSize = false,
                ColumnCount = 1,
                RowCount = 0,
                Role = PanelRole.Sidebar,
                Padding = new Padding(10, 4, 6, 10),
                Margin = Padding.Empty
            };
            navStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            pnlNavHost.SetContent(navStack);
            navStack.Tag = "NAV_STACK";

            layout.Controls.Add(pnlNavHost, 0, 1);
            layout.Controls.Add(BuildSidebarFooter(), 0, 2);

            pnlSidebar.Controls.Add(layout);

            // Populate navigation
            AddNavItem(navStack, "Dashboard", ThemeIcons.Dashboard, null, typeof(frmDashBoard),
                () => OpenForm(new frmDashBoard()));

            AddNavSection(navStack, "Masters");
            AddNavItem(navStack, "Office Zone", ThemeIcons.Map, "Master", typeof(frmOfficeZone),
                () => OpenForm(new frmOfficeZone()));
            AddNavItem(navStack, "Office", ThemeIcons.Office, "Master", typeof(frmOffice),
                () => OpenForm(new frmOffice()));
            AddNavItem(navStack, "Stamp Category", ThemeIcons.Document, "Master", typeof(frmStampCategory),
                () => OpenForm(new frmStampCategory()));
            AddNavItem(navStack, "Denomination", ThemeIcons.Money, "Master", typeof(frmDenomination),
                () => OpenForm(new frmDenomination()));
            AddNavItem(navStack, "Unit Conversion", ThemeIcons.Switch, "Master", typeof(frmUnitConversionMaster),
                () => OpenForm(new frmUnitConversionMaster()));
            AddNavItem(navStack, "User Management", ThemeIcons.People, "Master", typeof(frmUserManagement),
                () => OpenForm(new frmUserManagement()), true);

            AddNavSection(navStack, "Stock");
            AddNavItem(navStack, "Stock In", ThemeIcons.Package, "StockIn", typeof(frmStockIn),
                () => OpenForm(new frmStockIn()));
            AddNavItem(navStack, "Stock Balance", ThemeIcons.Document, "StockBalance", typeof(frmStockBalance),
                () => OpenForm(new frmStockBalance()));
            AddNavItem(navStack, "Stock Adjustment", ThemeIcons.Edit, "StockAdjustment", typeof(frmStockAdjustment),
                () => OpenForm(new frmStockAdjustment()));

            AddNavSection(navStack, "Supply Operations");
            AddNavItem(navStack, "Indent Entry", ThemeIcons.Add, "IndentEntryer", typeof(frmIndent),
                () => OpenForm(new frmIndent()));
            AddNavItem(navStack, "Indent Correction", ThemeIcons.Edit, "IndentCorrection", typeof(frmIndentSearch),
                () => OpenForm(new frmIndentSearch()));
            AddNavItem(navStack, "Supply Search", ThemeIcons.Search, "SupplySearch", typeof(frmSupplySearch),
                () => OpenForm(new frmSupplySearch()));
            AddNavItem(navStack, "Draft Supply", ThemeIcons.Page, "DraftSearch", typeof(frmDraftSupply),
                () => OpenForm(new frmDraftSupply()));
            AddNavItem(navStack, "Postal Stationery", ThemeIcons.Library, "SUPPLY", null,
                () => btnPostal_Click(null, EventArgs.Empty));

            AddNavSection(navStack, "Reports");
            AddNavItem(navStack, "Indent & Supply Report", ThemeIcons.Chart, "GeneralReport", typeof(frmReport),
                () => OpenForm(new frmReport()));
            AddNavItem(navStack, "Demand Print", ThemeIcons.Print, "DemandPrint", typeof(frmDemandPrint),
                () => OpenForm(new frmDemandPrint()));

            AddNavSection(navStack, "Administration");
            AddNavItem(navStack, "Change Password", ThemeIcons.Lock, "ChangePassword", typeof(frmChangePassword),
                () => OpenForm(new frmChangePassword()));
            AddNavItem(navStack, "Backup Database", ThemeIcons.Save, "Backup", null,
                () => DatabaseBackup.ChangeBackupLocation());
            AddNavItem(navStack, "Restore Database", ThemeIcons.Download, "Restore", null,
                () => RestoreDatabase(), true);
            AddNavItem(navStack, "Restore Safety Backup", ThemeIcons.Refresh, "RestoreSafety", null,
                () => RestoreSafetyBackup(), true);

            AddNavSection(navStack, "Help");
            AddNavItem(navStack, "User Guide", ThemeIcons.Help, "Help", null,
                () => ShowDialogForm(new frmUserGuide()));
            AddNavItem(navStack, "Keyboard Shortcuts", ThemeIcons.Keyboard, "Help", null,
                () => ShowDialogForm(new frmKeyboardShortcuts()));
            AddNavItem(navStack, "About", ThemeIcons.Info, "Help", null,
                () => ShowDialogForm(new frmAbout()));

            pnlNavHost.Relayout();

            return pnlSidebar;
        }

        private Control BuildBrand()
        {
            ThemedPanel brand = new ThemedPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Sidebar,
                Margin = Padding.Empty
            };

            IconBadge logo = new IconBadge
            {
                Glyph = ThemeIcons.Package,
                Accent = AppTheme.P.Primary,
                Size = new Size(42, 42),
                Radius = 12,
                Location = new Point(20, 26)
            };

            ThemedLabel name = new ThemedLabel
            {
                Text = "Supply Branch",
                Role = LabelRole.SidebarText,
                Font = ThemeFonts.H3,
                AutoSize = true,
                Location = new Point(72, 24)
            };

            ThemedLabel sub = new ThemedLabel
            {
                Text = "Management System",
                Role = LabelRole.SidebarMuted,
                Font = ThemeFonts.Tiny,
                AutoSize = true,
                Location = new Point(73, 46)
            };

            brand.Controls.Add(logo);
            brand.Controls.Add(name);
            brand.Controls.Add(sub);
            return brand;
        }

        private Control BuildSidebarFooter()
        {
            ThemedTable footer = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Sidebar,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(14, 10, 14, 14),
                Margin = Padding.Empty
            };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 9F));
            footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            pnlSidebarDivider = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 1,
                BackColor = ColorUtil.Blend(AppTheme.P.Sidebar, AppTheme.P.SidebarText, 0.18F),
                Margin = new Padding(0, 0, 0, 8)
            };

            Panel userRow = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };

            IconBadge avatar = new IconBadge
            {
                Glyph = ThemeIcons.User,
                Accent = AppTheme.P.Primary,
                Size = new Size(36, 36),
                Radius = 18,
                Location = new Point(0, 6)
            };

            ThemedLabel caption = new ThemedLabel
            {
                Text = "Signed in as",
                Role = LabelRole.SidebarMuted,
                Font = ThemeFonts.Tiny,
                AutoSize = true,
                Location = new Point(46, 6)
            };

            lblSidebarUser = new ThemedLabel
            {
                Text = "User",
                Role = LabelRole.SidebarText,
                Font = ThemeFonts.BodyBold,
                AutoSize = false,
                Size = new Size(160, 20),
                Location = new Point(46, 23),
                AutoEllipsis = true
            };

            userRow.Controls.Add(avatar);
            userRow.Controls.Add(caption);
            userRow.Controls.Add(lblSidebarUser);

            ModernButton logout = new ModernButton
            {
                Text = "Logout",
                IconGlyph = ThemeIcons.Power,
                Variant = ButtonVariant.Sidebar,
                Dock = DockStyle.Fill,
                Height = 38,
                Margin = new Padding(0, 6, 0, 0),
                CornerRadius = 8
            };
            logout.Click += (s, e) => Logout();

            footer.Controls.Add(pnlSidebarDivider, 0, 0);
            footer.Controls.Add(userRow, 0, 1);
            footer.Controls.Add(logout, 0, 2);

            return footer;
        }

        private Control BuildHeader()
        {
            pnlHeader = new ThemedPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Header,
                Padding = new Padding(22, 10, 18, 10),
                Margin = Padding.Empty
            };
            pnlHeader.Resize += (s, e) => UpdateHeaderResponsive();

            ThemedTable header = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Header,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            header.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ----- Title block (two stacked rows) -----
            ThemedTable titleBlock = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Header,
                ColumnCount = 1,
                RowCount = 2,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            titleBlock.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            titleBlock.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            titleBlock.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));

            lblPageTitle = new ThemedLabel
            {
                Text = "Dashboard",
                Role = LabelRole.Primary,
                Font = ThemeFonts.H2,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Margin = Padding.Empty
            };

            lblPageSubtitle = new ThemedLabel
            {
                Text = "Supply Branch Management System",
                Role = LabelRole.Muted,
                Font = ThemeFonts.Small,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                AutoEllipsis = true,
                Margin = Padding.Empty
            };

            titleBlock.Controls.Add(lblPageTitle, 0, 0);
            titleBlock.Controls.Add(lblPageSubtitle, 0, 1);

            // ----- Right cluster (auto-sizes, never overflows) -----
            FlowLayoutPanel right = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.None,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(12, 0, 0, 0)
            };

            btnThemeToggle = new ModernButton
            {
                Variant = ButtonVariant.Ghost,
                IconGlyph = AppTheme.IsDark ? ThemeIcons.Sun : ThemeIcons.Moon,
                Size = new Size(38, 38),
                Margin = new Padding(8, 9, 0, 0),
                CornerRadius = 9,
                TabStop = false
            };
            btnThemeToggle.Click += (s, e) => AppTheme.Toggle();

            pnlHeaderUser = new Panel
            {
                Size = new Size(180, 42),
                BackColor = Color.Transparent,
                Margin = new Padding(10, 7, 2, 0)
            };

            IconBadge avatar = new IconBadge
            {
                Glyph = ThemeIcons.User,
                Accent = AppTheme.P.Primary,
                Size = new Size(34, 34),
                Radius = 17,
                Location = new Point(0, 4)
            };

            lblHeaderUser = new ThemedLabel
            {
                Text = "User",
                Role = LabelRole.Primary,
                Font = ThemeFonts.BodyBold,
                AutoSize = false,
                Size = new Size(140, 20),
                Location = new Point(42, 4),
                AutoEllipsis = true
            };

            lblHeaderRole = new ThemedLabel
            {
                Text = "Signed in",
                Role = LabelRole.Muted,
                Font = ThemeFonts.Tiny,
                AutoSize = true,
                Location = new Point(43, 22)
            };

            pnlHeaderUser.Controls.Add(avatar);
            pnlHeaderUser.Controls.Add(lblHeaderUser);
            pnlHeaderUser.Controls.Add(lblHeaderRole);

            lblHeaderMeta = new ThemedLabel
            {
                Text = string.Empty,
                Role = LabelRole.Muted,
                Font = ThemeFonts.Small,
                AutoSize = true,
                Margin = new Padding(0, 21, 12, 0)
            };

            right.Controls.Add(btnThemeToggle);
            right.Controls.Add(pnlHeaderUser);
            right.Controls.Add(lblHeaderMeta);

            header.Controls.Add(titleBlock, 0, 0);
            header.Controls.Add(right, 1, 0);

            pnlHeader.Controls.Add(header);
            return pnlHeader;
        }

        /// <summary>
        /// Hides lower-priority header items on narrow windows so the page
        /// title always stays readable and nothing spills over the sidebar.
        /// </summary>
        private void UpdateHeaderResponsive()
        {
            if (pnlHeader == null)
                return;

            int width = pnlHeader.ClientSize.Width;

            if (lblHeaderMeta != null)
                lblHeaderMeta.Visible = width >= 900;

            if (pnlHeaderUser != null)
                pnlHeaderUser.Visible = width >= 700;

            if (lblPageSubtitle != null)
                lblPageSubtitle.Visible = width >= 620;
        }

        private void AddNavSection(ThemedTable stack, string title)
        {
            ThemedLabel label = new ThemedLabel
            {
                Text = title.ToUpperInvariant(),
                Role = LabelRole.SidebarMuted,
                Font = ThemeFonts.SectionLabel,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(8, 0, 0, 4),
                Margin = new Padding(0, 8, 0, 0)
            };

            AddNavControl(stack, label, 40F);
        }

        private SidebarButton AddNavItem(
            ThemedTable stack,
            string text,
            string glyph,
            string moduleCode,
            Type formType,
            Action action,
            bool adminOnly = false)
        {
            SidebarButton button = new SidebarButton
            {
                Text = text,
                IconGlyph = glyph,
                Tag = moduleCode,
                NavFormType = formType,
                AdminOnly = adminOnly,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 1, 0, 1)
            };

            button.Click += (s, e) =>
            {
                if (formType != null)
                    SetActiveNav(button);

                if (action != null)
                    action();
            };

            navButtons.Add(button);
            AddNavControl(stack, button, 40F);
            return button;
        }

        private static void AddNavControl(ThemedTable stack, Control control, float rowHeight)
        {
            stack.RowCount++;
            stack.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            stack.Controls.Add(control, 0, stack.RowCount - 1);
        }

        private void SetActiveNav(SidebarButton button)
        {
            foreach (SidebarButton nav in navButtons)
                nav.IsActive = ReferenceEquals(nav, button);
        }

        private void SetActiveNavByType(Type formType)
        {
            bool found = false;

            foreach (SidebarButton nav in navButtons)
            {
                bool active = nav.NavFormType != null && nav.NavFormType == formType;
                nav.IsActive = active;

                if (active)
                    found = true;
            }

            if (!found)
            {
                foreach (SidebarButton nav in navButtons)
                    nav.IsActive = false;
            }
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            if (rootLayout != null)
                rootLayout.BackColor = AppTheme.P.Background;

            if (rightLayout != null)
                rightLayout.BackColor = AppTheme.P.Background;

            if (pnlMain != null)
                pnlMain.BackColor = AppTheme.P.Background;

            AppTheme.ApplyScrollbarTheme(pnlNavHost, true);
            AppTheme.ApplyScrollbarTheme(pnlMain);

            if (pnlSidebarDivider != null)
                pnlSidebarDivider.BackColor =
                    ColorUtil.Blend(AppTheme.P.Sidebar, AppTheme.P.SidebarText, 0.18F);

            if (btnThemeToggle != null)
            {
                btnThemeToggle.IconGlyph = AppTheme.IsDark ? ThemeIcons.Sun : ThemeIcons.Moon;
                btnThemeToggle.Invalidate();
            }

            Invalidate(true);
        }

        // =============================================================
        //  Navigation helpers
        // =============================================================

        private void OpenForm(Form childForm)
        {
            if (childForm == null)
                return;

            // Close any currently open child forms in pnlMain
            foreach (Form openForm in pnlMain.Controls.OfType<Form>().ToList())
            {
                openForm.Close();
                openForm.Dispose();
                pnlMain.Controls.Remove(openForm);
            }

            if (childForm is frmDashBoard dashboard)
                dashboard.NavigationRequested += Dashboard_NavigationRequested;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.Margin = new Padding(0);
            childForm.BackColor = AppTheme.P.Background;

            pnlMain.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();

            string title = childForm.Text;

            Control[] foundControls = childForm.Controls.Find("lblSubTitle", true);
            if (foundControls.Length > 0 && foundControls[0] is Label lblSub &&
                !string.IsNullOrWhiteSpace(lblSub.Text))
            {
                title = lblSub.Text;
            }

            if (childForm is frmDashBoard)
                title = "Dashboard";

            lblPageTitle.Text = string.IsNullOrWhiteSpace(title)
                ? "Supply Branch"
                : title;

            lblPageSubtitle.Text = "Supply Branch Management System";

            SetActiveNavByType(childForm.GetType());
        }

        private void ShowDialogForm(Form form)
        {
            if (form == null)
                return;

            using (form)
            {
                form.ShowDialog(this);
            }
        }

        private void RestoreDatabase()
        {
            if (!DatabaseBackup.RestoreDatabase())
                return;

            Application.Restart();
        }

        private void RestoreSafetyBackup()
        {
            if (!DatabaseBackup.RestoreSafetyBackup())
                return;

            Application.Restart();
        }

        private void Dashboard_NavigationRequested(Form childForm)
        {
            OpenForm(childForm);
        }

        // =============================================================
        //  Login / logout
        // =============================================================

        private void Logout()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            // Take Database Backup
            if (!DatabaseBackup.BackupDatabase())
            {
                MessageBox.Show(
                    "Logout cancelled because database backup could not be completed.",
                    "Logout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Clear Current User
            CurrentUser.UserID = 0;
            CurrentUser.UserName = null;
            CurrentUser.FullName = null;

            // Close Forms inside pnlMain
            foreach (Control control in pnlMain.Controls)
            {
                if (control is Form childForm)
                    childForm.Close();
            }

            pnlMain.Controls.Clear();

            // Show Login
            frmLogin login = new frmLogin();
            login.Show();

            if (clockTimer != null)
                clockTimer.Stop();

            AppTheme.ThemeChanged -= HandleThemeChanged;
            this.Hide();
        }

        // =============================================================
        //  Keyboard shortcuts
        // =============================================================

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            // F5 = Refresh active form
            if (e.KeyCode == Keys.F5)
            {
                RefreshActiveForm();
                e.Handled = true;
            }
            // Esc = Close active child form
            else if (e.KeyCode == Keys.Escape)
            {
                CloseActiveForm();
                e.Handled = true;
            }
        }

        private void CloseActiveForm()
        {
            Form active = pnlMain.Controls
                .OfType<Form>()
                .FirstOrDefault(f => f.Visible);

            if (active != null)
                active.Close();
        }

        private void RefreshActiveForm()
        {
            Form active = pnlMain.Controls
                .OfType<Form>()
                .FirstOrDefault(f => f.Visible);

            IRefreshable refreshable = active as IRefreshable;
            if (refreshable != null)
                refreshable.RefreshData();
        }

        public interface IRefreshable
        {
            void RefreshData();
        }

        // =============================================================
        //  Menu permissions
        // =============================================================

        private void ApplyMenuPermissions(int userID, string userName)
        {
            bool isSuperAdmin = string.Equals(
                userName != null ? userName.Trim() : null,
                "admin",
                StringComparison.OrdinalIgnoreCase);

            HashSet<string> allowedCodes = isSuperAdmin
                ? null
                : moduleDAL.GetAllowedModuleCodes(userID);

            foreach (ToolStripItem item in menuStrip1.Items)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                    ApplyMenuItemPermission(menuItem, isSuperAdmin, allowedCodes);
            }
        }

        private void ApplyMenuItemPermission(
            ToolStripMenuItem menuItem,
            bool isSuperAdmin,
            HashSet<string> allowedCodes)
        {
            bool hasAnyEnabledChild = false;
            bool hasAnyChildMenuItem = false;

            foreach (ToolStripItem dropItem in menuItem.DropDownItems)
            {
                ToolStripMenuItem childMenuItem = dropItem as ToolStripMenuItem;
                if (childMenuItem != null)
                {
                    hasAnyChildMenuItem = true;
                    ApplyMenuItemPermission(childMenuItem, isSuperAdmin, allowedCodes);

                    if (childMenuItem.Enabled)
                        hasAnyEnabledChild = true;
                }
            }

            string moduleCode = Convert.ToString(menuItem.Tag);
            if (moduleCode != null)
                moduleCode = moduleCode.Trim();

            if (isSuperAdmin)
            {
                menuItem.Enabled = true;
                return;
            }

            if (!string.IsNullOrWhiteSpace(moduleCode))
            {
                menuItem.Enabled = allowedCodes != null && allowedCodes.Contains(moduleCode);
            }
            else if (hasAnyChildMenuItem)
            {
                menuItem.Enabled = hasAnyEnabledChild;
            }
        }

        private void ApplyNavPermissions()
        {
            bool isAdmin = string.Equals(
                CurrentUser.UserName,
                "admin",
                StringComparison.OrdinalIgnoreCase);

            Dictionary<string, bool> states = new Dictionary<string, bool>(
                StringComparer.OrdinalIgnoreCase);

            CollectMenuStates(menuStrip1.Items, states);

            foreach (SidebarButton button in navButtons)
            {
                if (button.AdminOnly)
                    button.Visible = isAdmin;

                string code = button.Tag as string;
                if (!string.IsNullOrWhiteSpace(code) && states.ContainsKey(code))
                    button.Enabled = states[code];
            }
        }

        private static void CollectMenuStates(
            ToolStripItemCollection items,
            Dictionary<string, bool> states)
        {
            foreach (ToolStripItem item in items)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem == null)
                    continue;

                string code = menuItem.Tag as string;
                if (!string.IsNullOrWhiteSpace(code))
                    states[code] = menuItem.Enabled;

                if (menuItem.DropDownItems.Count > 0)
                    CollectMenuStates(menuItem.DropDownItems, states);
            }
        }

        private static void ClearMenuShortcuts(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem == null)
                    continue;

                menuItem.ShortcutKeys = Keys.None;
                ClearMenuShortcuts(menuItem.DropDownItems);
            }
        }

        // =============================================================
        //  Form load
        // =============================================================

        private void frmMain_Load(object sender, EventArgs e)
        {
            ApplyMenuPermissions(CurrentUser.UserID, CurrentUser.UserName);
            ApplyNavPermissions();

            this.Text = "SupplyBranch Version " + AppVersion.CurrentVersion;

            string displayName = string.IsNullOrWhiteSpace(CurrentUser.FullName)
                ? (CurrentUser.UserName ?? "User")
                : CurrentUser.FullName;

            lblHeaderUser.Text = displayName;
            lblSidebarUser.Text = displayName;

            UpdateHeaderResponsive();
            AppTheme.ApplyScrollbarTheme(pnlNavHost, true);
            AppTheme.ApplyScrollbarTheme(pnlMain);
            UpdateClock();

            clockTimer = new Timer { Interval = 1000 };
            clockTimer.Tick += (s, a) => UpdateClock();
            clockTimer.Start();

            bool isAdmin = string.Equals(
                CurrentUser.UserName,
                "admin",
                StringComparison.OrdinalIgnoreCase);

            mnuRestoreDatabase.Visible = isAdmin;
            mnuRestoreSafetyBackup.Visible = isAdmin;
            mnuUserManagement.Visible = isAdmin;

            OpenForm(new frmDashBoard());
        }

        private void UpdateClock()
        {
            if (lblHeaderMeta == null)
                return;

            lblHeaderMeta.Text = DateTime.Now.ToString("ddd, dd MMM yyyy   \u2022   hh:mm:ss tt");
        }

        private async void frmMain_Shown(object sender, EventArgs e)
        {
            await CheckForUpdatesAsync();
        }

        // =============================================================
        //  Update check
        // =============================================================

        private async Task<string> GetLatestVersionAsync()
        {
            string url = "https://raw.githubusercontent.com/shaukatkb01/C_Sharp_Project/SupplyBranch-Only/PostalRegistor/SupplyBranch/UpdateInfo.txt";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    string content = await client.GetStringAsync(url);

                    if (string.IsNullOrWhiteSpace(content))
                        return string.Empty;

                    // Line 1: Version number, Line 2: ZIP download link
                    string[] lines = content.Split(
                        new[] { "\r\n", "\n" },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length > 0)
                    {
                        string latestVersion = lines[0].Trim();
                        AppVersionInfo.AvailableVersion = latestVersion;

                        if (lines.Length > 1)
                            AppVersionInfo.DownloadUrl = lines[1].Trim();

                        return latestVersion;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UPDATE CHECK ERROR: " + ex.Message);
            }

            return string.Empty;
        }

        private async Task CheckForUpdatesAsync()
        {
            this.Text = "SupplyBranch - Checking Update...";

            try
            {
                string currentVersion = AppVersion.CurrentVersion;
                AppVersionInfo.CurrentVersion = currentVersion;

                string latestVersion = await GetLatestVersionAsync();
                AppVersionInfo.AvailableVersion = latestVersion;

                if (!string.IsNullOrWhiteSpace(latestVersion))
                {
                    Version current;
                    Version latest;

                    if (Version.TryParse(AppVersionInfo.CurrentVersion, out current) &&
                        Version.TryParse(latestVersion, out latest) &&
                        latest > current)
                    {
                        DialogResult result = MessageBox.Show(
                            "A new version of SupplyBranch is available!\n\n" +
                            "Current Version: " + AppVersionInfo.CurrentVersion + "\n" +
                            "Latest Version: " + latestVersion + "\n\n" +
                            "Would you like to download and install the update now?",
                            "SupplyBranch Update Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            if (string.IsNullOrEmpty(AppVersionInfo.DownloadUrl))
                            {
                                MessageBox.Show(
                                    "Download URL missing. Please check UpdateInfo.txt on server.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                                return;
                            }

                            string tempZipPath = Path.Combine(
                                Path.GetTempPath(),
                                "SupplyBranch_Update.zip");

                            using (frmUpdateDownload downloadForm =
                                new frmUpdateDownload(AppVersionInfo.DownloadUrl, tempZipPath))
                            {
                                if (downloadForm.ShowDialog() == DialogResult.OK)
                                    UpdateDAL.ApplyUpdateAndRestart(tempZipPath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("VERSION CHECK ERROR: " + ex.ToString());
            }
            finally
            {
                this.Text = "SupplyBranch Version " + AppVersionInfo.CurrentVersion;
            }
        }

        // =============================================================
        //  Legacy menu handlers (still used by sidebar / shortcuts)
        // =============================================================

        private void btnPostal_Click(object sender, EventArgs e)
        {
            frmPostal_Stationery frmPostal = new frmPostal_Stationery();
            frmPostal.ShowDialog();
        }

        private void officeZoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmOfficeZone());
        }

        private void officeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmOffice());
        }

        private void stampCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStampCategory());
        }

        private void denominationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmDenomination());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseBackup.BackupDatabase();
            Application.Exit();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmUnitConversionMaster());
        }

        private void indentCorrectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmIndentSearch());
        }

        private void indentEntryerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmIndent());
        }

        private void searchIndentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmSupplySearch());
        }

        private void draftSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmDraftSupply());
        }

        private void officeWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmReport());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmChangePassword());
        }

        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseBackup.ChangeBackupLocation();
        }

        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmDashBoard());
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAbout about = new frmAbout())
            {
                about.ShowDialog(this);
            }
        }

        private void userGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmUserGuide guide = new frmUserGuide())
            {
                guide.ShowDialog(this);
            }
        }

        private void keyboardShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmKeyboardShortcuts shortcuts = new frmKeyboardShortcuts())
            {
                shortcuts.ShowDialog(this);
            }
        }

        private void mnuRestoreDatabase_Click(object sender, EventArgs e)
        {
            RestoreDatabase();
        }

        private void mnuRestoreSafetyBackup_Click(object sender, EventArgs e)
        {
            RestoreSafetyBackup();
        }

        private void mnuUserManagement_Click(object sender, EventArgs e)
        {
            if (!string.Equals(
                CurrentUser.UserName,
                "admin",
                StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Only Admin user can access User Management.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            OpenForm(new frmUserManagement());
        }

        private void enterStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStockIn());
        }

        private void stockBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStockBalance());
        }

        private void stockAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmStockAdjustment());
        }

        private void fileIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // FileIndex is part of the separate PostalStampBranch application.
        }

        private void demandPrintOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmDemandPrint());
        }

        private void userRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new frmModuleManagement());
        }
    }
}
