using StampStoreApp;
using SupplyBranch.Controls;
using SupplyBranch.DataAccess;
using SupplyBranch.Forms.Reports;
using SupplyBranch.Forms.Stock;
using SupplyBranch.Forms.Transactions;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using static SupplyBranch.DataAccess.SupplyDAL;

namespace SupplyBranch.Forms
{
    public partial class frmDashBoard : Form
    {
        private readonly DBHelper db = new DBHelper();
        private readonly SupplyDAL supplyDal = new SupplyDAL();
        private readonly Label[] statValues = new Label[4];

        private Label taskEmptyState;
        private ThemedLabel lblTaskComboHint;
        private CategorySupplyChart supplyChart;
        private Button btnMetricSheets;
        private Button btnMetricValue;
        private bool showValueMetric;
        private DataTable suppliesData;
        private DataTable chartComparisonData;
        private string currentUserName = "User";
        private ThemedLabel welcomeLabel;

        public event Action<Form> NavigationRequested;

        public frmDashBoard()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.Disposed += (s, e) => AppTheme.ThemeChanged -= HandleThemeChanged;

            BuildDashboardLayout();
            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            BackColor = AppTheme.P.Background;
            UpdateMetricButtonsStyle();
            if (supplyChart != null)
                supplyChart.Invalidate();
            Invalidate(true);
        }

        // =============================================================
        //  Layout
        // =============================================================

        private void BuildDashboardLayout()
        {
            panel1.Controls.Remove(cmb_Task);
            panel1.Controls.Remove(btnAdd);
            panel1.Controls.Remove(button1);

            splitContainerMain.Panel1.Controls.Remove(dgvTasks);
            splitContainerMain.Panel2.Controls.Remove(dgvSuppliesBottom);

            Controls.Clear();

            panel1.Dispose();
            splitContainerMain.Dispose();
            dgvSuppliesBottom.Dispose();
            btnAdd.Dispose();
            button1.Dispose();

            BackColor = AppTheme.P.Background;
            ForeColor = AppTheme.P.TextPrimary;
            Font = ThemeFonts.Body;
            AutoScroll = true;

            ThemedTable layout = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Background,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            layout.Controls.Add(CreateDashboardHeader(), 0, 0);
            layout.Controls.Add(CreateQuickActions(), 0, 1);
            layout.Controls.Add(CreateStatisticsCards(), 0, 2);
            layout.Controls.Add(CreateWorkArea(), 0, 3);

            Controls.Add(layout);
        }

        private Control CreateDashboardHeader()
        {
            ThemedTable header = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Background,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            header.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel titlePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0, 2, 0, 0)
            };

            ThemedLabel title = new ThemedLabel
            {
                Text = "Welcome back, " + currentUserName,
                Role = LabelRole.Primary,
                Font = ThemeFonts.H1,
                AutoSize = true,
                Margin = new Padding(0)
            };

            welcomeLabel = title;

            ThemedLabel date = new ThemedLabel
            {
                Text = DateTime.Today.ToString("dddd, dd MMMM yyyy"),
                Role = LabelRole.Muted,
                Font = ThemeFonts.Small,
                AutoSize = true,
                Margin = new Padding(2, 4, 0, 0)
            };

            titlePanel.Controls.Add(title);
            titlePanel.Controls.Add(date);

            ModernButton refresh = new ModernButton
            {
                Text = "Refresh",
                IconGlyph = ThemeIcons.Refresh,
                Variant = ButtonVariant.Secondary,
                Size = new Size(112, 38),
                Margin = new Padding(8, 8, 0, 0),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                CornerRadius = 9
            };
            refresh.Click += (s, e) => RefreshData();

            header.Controls.Add(titlePanel, 0, 0);
            header.Controls.Add(refresh, 1, 0);
            return header;
        }

        private Control CreateQuickActions()
        {
            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 6, 0, 0),
                Margin = new Padding(0)
            };

            actions.Controls.Add(CreateNavigationButton("New Indent", ThemeIcons.Add, () => new frmIndent(), true));
            actions.Controls.Add(CreateNavigationButton("Generate Draft", ThemeIcons.Page, () => new frmSupplySearch(), false));
            actions.Controls.Add(CreateNavigationButton("Draft Supplies", ThemeIcons.Document, () => new frmDraftSupply(), false));
            actions.Controls.Add(CreateNavigationButton("Reports", ThemeIcons.Chart, () => new frmReport(), false));
            actions.Controls.Add(CreateNavigationButton("Stock Balance", ThemeIcons.Package, () => new frmStockBalance(), false));
            actions.Controls.Add(CreateNavigationButton("Blank Performa", ThemeIcons.Print, () => new frmReportPreview(), false));

            return actions;
        }

        private Control CreateStatisticsCards()
        {
            ThemedTable cards = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Background,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(0, 2, 0, 10),
                Margin = new Padding(0)
            };

            for (int i = 0; i < 4; i++)
                cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            cards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            StatCard[] stats =
            {
                new StatCard("Pending tasks", "Open items", ThemeIcons.Clock, AppTheme.P.Primary),
                new StatCard("Critical tasks", "Needs attention", ThemeIcons.Warning, AppTheme.P.Danger),
                new StatCard("Draft supplies", "In progress", ThemeIcons.Page, AppTheme.P.Warning),
                new StatCard("Approved supplies", "Ready to dispatch", ThemeIcons.Check, AppTheme.P.Success)
            };

            for (int i = 0; i < stats.Length; i++)
            {
                stats[i].Dock = DockStyle.Fill;
                stats[i].Margin = new Padding(0, 0, i == stats.Length - 1 ? 0 : 12, 0);
                statValues[i] = stats[i].ValueLabel;
                cards.Controls.Add(stats[i], i, 0);
            }

            return cards;
        }

        private Control CreateWorkArea()
        {
            ThemedTable workArea = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Background,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            workArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54F));
            workArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            workArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Control taskSection = CreateTasksSection();
            Control chartSection = CreateSupplyChartSection();

            taskSection.Margin = new Padding(0, 0, 12, 0);
            chartSection.Margin = new Padding(0);

            workArea.Controls.Add(taskSection, 0, 0);
            workArea.Controls.Add(chartSection, 1, 0);
            return workArea;
        }

        private Control CreateTasksSection()
        {
            CardPanel card = new CardPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 14, 18, 18),
                Margin = new Padding(0)
            };

            ThemedTable layout = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Surface,
                ColumnCount = 1,
                RowCount = 3,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ThemedLabel title = new ThemedLabel
            {
                Text = "My Tasks",
                Role = LabelRole.Primary,
                Font = ThemeFonts.H3,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };

            layout.Controls.Add(title, 0, 0);
            layout.Controls.Add(CreateTaskToolbar(), 0, 1);

            ThemedPanel gridHost = new ThemedPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Surface,
                Padding = new Padding(0, 4, 0, 0),
                Margin = new Padding(0)
            };

            dgvTasks.Dock = DockStyle.Fill;
            dgvTasks.Margin = new Padding(0);
            GridStyler.Apply(dgvTasks);

            taskEmptyState = new ThemedLabel
            {
                Dock = DockStyle.Fill,
                Role = LabelRole.Muted,
                Font = ThemeFonts.Body,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
                BackColor = Color.Transparent
            };

            gridHost.Controls.Add(dgvTasks);
            gridHost.Controls.Add(taskEmptyState);
            taskEmptyState.BringToFront();

            layout.Controls.Add(gridHost, 0, 2);
            card.Controls.Add(layout);
            return card;
        }

        private Control CreateSupplyChartSection()
        {
            CardPanel card = new CardPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 14, 18, 18),
                Margin = new Padding(0)
            };

            ThemedTable layout = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Surface,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ThemedTable topBar = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Surface,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            topBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            topBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));
            topBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ThemedLabel title = new ThemedLabel
            {
                Text = "Category Supply: FY Comparison",
                Role = LabelRole.Primary,
                Font = ThemeFonts.H3,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };

            FlowLayoutPanel toggleHost = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0, 3, 0, 3)
            };

            btnMetricValue = new Button
            {
                Text = "Value",
                Font = ThemeFonts.Small,
                Size = new Size(68, 28),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 0, 0, 0)
            };
            btnMetricValue.Click += (s, e) =>
            {
                showValueMetric = true;
                UpdateMetricButtonsStyle();
                ApplyChartMetric();
            };

            btnMetricSheets = new Button
            {
                Text = "Sheets",
                Font = ThemeFonts.Small,
                Size = new Size(70, 28),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            btnMetricSheets.Click += (s, e) =>
            {
                showValueMetric = false;
                UpdateMetricButtonsStyle();
                ApplyChartMetric();
            };

            toggleHost.Controls.Add(btnMetricValue);
            toggleHost.Controls.Add(btnMetricSheets);
            UpdateMetricButtonsStyle();

            topBar.Controls.Add(title, 0, 0);
            topBar.Controls.Add(toggleHost, 1, 0);

            supplyChart = new CategorySupplyChart
            {
                Dock = DockStyle.Fill,
                CornerRadius = 10,
                BorderWidth = 1,
                UseAltSurface = true,
                Padding = new Padding(12),
                Margin = new Padding(0, 8, 0, 0)
            };

            layout.Controls.Add(topBar, 0, 0);
            layout.Controls.Add(supplyChart, 0, 1);
            card.Controls.Add(layout);
            return card;
        }

        private void UpdateMetricButtonsStyle()
        {
            if (btnMetricSheets == null || btnMetricValue == null)
                return;

            ThemePalette p = AppTheme.P;

            btnMetricSheets.BackColor = !showValueMetric ? p.Primary : p.SurfaceAlt;
            btnMetricSheets.ForeColor = !showValueMetric ? p.OnPrimary : p.TextSecondary;
            btnMetricSheets.FlatAppearance.BorderColor = !showValueMetric ? p.Primary : p.Border;

            btnMetricValue.BackColor = showValueMetric ? p.Primary : p.SurfaceAlt;
            btnMetricValue.ForeColor = showValueMetric ? p.OnPrimary : p.TextSecondary;
            btnMetricValue.FlatAppearance.BorderColor = showValueMetric ? p.Primary : p.Border;
        }

        private Control CreateTaskToolbar()
        {
            ThemedTable toolbar = new ThemedTable
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Surface,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            toolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            CardPanel comboHost = new CardPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 8,
                BorderWidth = 1,
                UseAltSurface = true,
                Padding = new Padding(8, 0, 2, 0),
                Margin = new Padding(0, 4, 8, 4)
            };

            cmb_Task.Dock = DockStyle.Fill;
            cmb_Task.Margin = new Padding(0);
            ControlStyler.StyleComboBox(cmb_Task);

            lblTaskComboHint = new ThemedLabel
            {
                Text = "No completed tasks",
                Role = LabelRole.Muted,
                Font = ThemeFonts.Small,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Visible = false
            };

            comboHost.Controls.Add(cmb_Task);
            comboHost.Controls.Add(lblTaskComboHint);
            lblTaskComboHint.BringToFront();

            ModernButton addButton = new ModernButton
            {
                Text = "Add task",
                IconGlyph = ThemeIcons.Add,
                Variant = ButtonVariant.Primary,
                Dock = DockStyle.Fill,
                Height = 34,
                Margin = new Padding(0, 2, 8, 2),
                CornerRadius = 8
            };
            addButton.Click += btnAdd_Click_1;

            ModernButton deleteButton = new ModernButton
            {
                Text = "Delete",
                IconGlyph = ThemeIcons.Delete,
                Variant = ButtonVariant.Secondary,
                Dock = DockStyle.Fill,
                Height = 34,
                Margin = new Padding(0, 2, 0, 2),
                CornerRadius = 8
            };
            deleteButton.Click += button1_Click_1;

            toolbar.Controls.Add(comboHost, 0, 0);
            toolbar.Controls.Add(addButton, 1, 0);
            toolbar.Controls.Add(deleteButton, 2, 0);
            return toolbar;
        }

        private Control CreateNavigationButton(
            string text,
            string icon,
            Func<Form> formFactory,
            bool isPrimary)
        {
            ModernButton button = new ModernButton
            {
                Text = text,
                IconGlyph = icon,
                Variant = isPrimary ? ButtonVariant.Primary : ButtonVariant.Secondary,
                Size = new Size(148, 38),
                Margin = new Padding(0, 0, 8, 4),
                CornerRadius = 9
            };

            button.Click += (s, e) =>
            {
                if (formFactory == null)
                    return;

                Form target = formFactory();
                Action<Form> navigate = NavigationRequested;

                if (navigate == null)
                {
                    if (target != null)
                        target.Dispose();

                    MessageBox.Show(
                        "Navigation is unavailable.",
                        "Dashboard",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                navigate(target);
            };

            return button;
        }

        // =============================================================
        //  Supply counts + FY comparison chart
        // =============================================================

        private void LoadSupplyCounts()
        {
            try
            {
                string[] statuses = new string[] { "Draft", "Approve" };
                suppliesData = supplyDal.GetSuppliesByStatusNames(statuses);
                UpdateSupplyStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load supply summary:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadCategorySupplyChart()
        {
            try
            {
                FinancialYearRange fy = SupplyDAL.GetFinancialYearsRange(DateTime.Today);
                chartComparisonData = supplyDal.GetCategorySupplyComparison(fy.CurrentFY, fy.LastFY);

                if (supplyChart != null)
                {
                    supplyChart.CurrentYearLabel = "Current FY (" + fy.CurrentFY + ")";
                    supplyChart.LastYearLabel = "Last FY (" + fy.LastFY + ")";
                    ApplyChartMetric();
                }
            }
            catch (Exception ex)
            {
                if (supplyChart != null)
                    supplyChart.SetItems(new List<CategoryComparisonItem>(), "Sheets");

                MessageBox.Show(
                    "Unable to load financial year chart data:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ApplyChartMetric()
        {
            if (supplyChart == null)
                return;

            List<CategoryComparisonItem> items = new List<CategoryComparisonItem>();

            if (chartComparisonData != null)
            {
                string currentCol = showValueMetric ? "CurrentFyValue" : "CurrentFySheets";
                string lastCol = showValueMetric ? "LastFyValue" : "LastFySheets";

                foreach (DataRow row in chartComparisonData.Rows)
                {
                    string category = Convert.ToString(row["CategoryName"]) ?? "Category";
                    double currentVal = row[currentCol] == DBNull.Value ? 0 : Convert.ToDouble(row[currentCol]);
                    double lastVal = row[lastCol] == DBNull.Value ? 0 : Convert.ToDouble(row[lastCol]);

                    items.Add(new CategoryComparisonItem
                    {
                        CategoryName = category,
                        CurrentValue = currentVal,
                        LastValue = lastVal
                    });
                }
            }

            supplyChart.SetItems(items, showValueMetric ? "Rs." : "Sheets");
        }

        // =============================================================
        //  Form load
        // =============================================================

        /// <summary>
        /// Shows the full name of the signed-in user (falls back to the login
        /// name when the full name is not available).
        /// </summary>
        private void UpdateWelcomeText()
        {
            if (welcomeLabel == null)
                return;

            string displayName = string.IsNullOrWhiteSpace(CurrentUser.FullName)
                ? currentUserName
                : CurrentUser.FullName.Trim();

            welcomeLabel.Text = "Welcome back, " + displayName;
        }

        private void frmDashBoard_Load(object sender, EventArgs e)
        {            try
            {
                if (!string.IsNullOrEmpty(CurrentUser.UserName))
                    currentUserName = CurrentUser.UserName;

                UpdateWelcomeText();

                LoadTasks();
                FillCompletedTasksCombo();
                ConfigureDataGridView();
                UpdateStatistics();
                LoadSupplyCounts();
                LoadCategorySupplyChart();
                UpdateEmptyStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Form load error:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =============================================================
        //  DataGridView configuration
        // =============================================================

        private void ConfigureDataGridView()
        {
            try
            {
                GridStyler.Apply(dgvTasks);

                if (dgvTasks.Columns.Contains("TaskID"))
                    dgvTasks.Columns["TaskID"].Visible = false;

                if (dgvTasks.Columns.Contains("TaskDescription"))
                {
                    dgvTasks.Columns["TaskDescription"].HeaderText = "Task";
                    dgvTasks.Columns["TaskDescription"].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
                    dgvTasks.Columns["TaskDescription"].MinimumWidth = 200;
                }

                if (dgvTasks.Columns.Contains("IsCompleted"))
                    dgvTasks.Columns["IsCompleted"].Visible = false;

                if (dgvTasks.Columns.Contains("Priority"))
                    dgvTasks.Columns["Priority"].Visible = false;

                if (dgvTasks.Columns.Contains("PriorityText"))
                {
                    dgvTasks.Columns["PriorityText"].HeaderText = "Priority";
                    dgvTasks.Columns["PriorityText"].Width = 100;
                    dgvTasks.Columns["PriorityText"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvTasks.Columns.Contains("CreatedDate"))
                {
                    dgvTasks.Columns["CreatedDate"].HeaderText = "Created";
                    dgvTasks.Columns["CreatedDate"].Width = 100;
                    dgvTasks.Columns["CreatedDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                }

                if (dgvTasks.Columns.Contains("DueDate"))
                {
                    dgvTasks.Columns["DueDate"].HeaderText = "Due Date";
                    dgvTasks.Columns["DueDate"].Width = 100;
                    dgvTasks.Columns["DueDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                }

                dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvTasks.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Grid configuration error: " + ex.Message);
            }
        }

        // =============================================================
        //  Tasks
        // =============================================================

        private void LoadTasks(bool showCompleted = true)
        {
            try
            {
                TaskHelper.LoadTasksToGrid(dgvTasks, currentUserName, showCompleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading tasks:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RefreshData()
        {
            LoadTasks(showCompleted: true);
            FillCompletedTasksCombo();
            UpdateStatistics();
            LoadSupplyCounts();
            LoadCategorySupplyChart();
            UpdateEmptyStates();
        }

        private void FillCompletedTasksCombo()
        {
            try
            {
                string query = @"
            SELECT 
                TaskID,
                TaskDescription,
                Priority
            FROM ToDoList
            WHERE IsCompleted = 1
              AND UserName = @UserName
            ORDER BY Priority DESC, TaskID DESC";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@UserName", currentUserName)
                };

                DataTable dt = db.ExecuteQuery(query, parameters);

                cmb_Task.DataSource = null;
                cmb_Task.DisplayMember = "TaskDescription";
                cmb_Task.ValueMember = "TaskID";
                cmb_Task.DataSource = dt;
                cmb_Task.SelectedIndex = -1;

                if (lblTaskComboHint != null)
                    lblTaskComboHint.Visible = dt == null || dt.Rows.Count == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load completed tasks.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (AddTaskForm form = new AddTaskForm(currentUserName))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show(
                            "Task added successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error opening add task form:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cmb_Task.SelectedIndex == -1 || cmb_Task.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Please select a task to delete.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cmb_Task.Focus();
                    return;
                }

                int taskId = Convert.ToInt32(cmb_Task.SelectedValue);

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this completed task?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                if (TaskHelper.DeleteTask(taskId))
                    RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error deleting task:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvTasks_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                int taskId = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells["TaskID"].Value);

                bool currentStatus =
                    dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value != DBNull.Value &&
                    Convert.ToBoolean(dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value);

                if (TaskHelper.UpdateTaskStatus(taskId, !currentStatus))
                    RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating task status:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        // =============================================================
        //  Statistics + empty states
        // =============================================================

        private void UpdateStatistics()
        {
            TaskStats stats = TaskHelper.GetTaskStatistics(currentUserName);

            if (statValues[0] != null)
                statValues[0].Text = stats.PendingTasks.ToString();

            if (statValues[1] != null)
                statValues[1].Text = stats.CriticalPending.ToString();
        }

        private void UpdateSupplyStatistics()
        {
            int draftCount = 0;
            int approvedCount = 0;

            if (suppliesData != null && suppliesData.Columns.Contains("StatusName"))
            {
                foreach (DataRow row in suppliesData.Rows)
                {
                    string status = Convert.ToString(row["StatusName"]);

                    if (string.Equals(status, "Draft", StringComparison.OrdinalIgnoreCase))
                        draftCount++;
                    else if (string.Equals(status, "Approve", StringComparison.OrdinalIgnoreCase))
                        approvedCount++;
                }
            }

            if (statValues[2] != null)
                statValues[2].Text = draftCount.ToString();

            if (statValues[3] != null)
                statValues[3].Text = approvedCount.ToString();
        }

        private void UpdateEmptyStates()
        {
            if (taskEmptyState != null)
            {
                taskEmptyState.Text = "No tasks to show. Use Add task to create one.";
                taskEmptyState.Visible = dgvTasks.Rows.Count == 0;
            }
        }
    }

    internal sealed class CategoryComparisonItem
    {
        public string CategoryName { get; set; }
        public double CurrentValue { get; set; }
        public double LastValue { get; set; }
    }

    internal sealed class CategorySupplyChart : CardPanel
    {
        private List<CategoryComparisonItem> _items = new List<CategoryComparisonItem>();
        private string _unitLabel = "Sheets";

        public string CurrentYearLabel { get; set; }
        public string LastYearLabel { get; set; }

        public CategorySupplyChart()
        {
            CurrentYearLabel = "Current FY";
            LastYearLabel = "Last FY";
        }

        public void SetItems(List<CategoryComparisonItem> items, string unitLabel)
        {
            _items = items ?? new List<CategoryComparisonItem>();
            _unitLabel = string.IsNullOrWhiteSpace(unitLabel) ? "Sheets" : unitLabel;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ThemePalette p = AppTheme.P;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int padL = 14;
            int padR = 14;
            int padT = 12;
            int padB = 12;

            Rectangle inner = new Rectangle(
                padL,
                padT,
                Math.Max(10, Width - padL - padR),
                Math.Max(10, Height - padT - padB));

            // Legend + Unit header
            int legendHeight = 24;
            Rectangle legendRect = new Rectangle(inner.X, inner.Y, inner.Width, legendHeight);

            Color lastColor = p.TextMuted;
            Color currentColor = p.Primary;

            using (SolidBrush bLast = new SolidBrush(lastColor))
                g.FillRectangle(bLast, legendRect.X, legendRect.Y + 6, 12, 12);

            TextRenderer.DrawText(
                g, LastYearLabel, ThemeFonts.Small,
                new Point(legendRect.X + 17, legendRect.Y + 3), p.TextSecondary);

            Size lastSize = TextRenderer.MeasureText(LastYearLabel, ThemeFonts.Small);
            int curX = legendRect.X + 28 + lastSize.Width;

            using (SolidBrush bCur = new SolidBrush(currentColor))
                g.FillRectangle(bCur, curX, legendRect.Y + 6, 12, 12);

            TextRenderer.DrawText(
                g, CurrentYearLabel, ThemeFonts.Small,
                new Point(curX + 17, legendRect.Y + 3), p.TextPrimary);

            TextRenderer.DrawText(
                g, "Unit: " + _unitLabel, ThemeFonts.Tiny,
                new Rectangle(legendRect.Right - 110, legendRect.Y + 4, 110, 18),
                p.TextMuted,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter);

            Rectangle plotArea = new Rectangle(
                inner.X,
                legendRect.Bottom + 8,
                inner.Width,
                Math.Max(20, inner.Height - legendHeight - 8));

            if (_items.Count == 0)
            {
                TextRenderer.DrawText(
                    g,
                    "No approved/issued supply data found for Current or Last FY.",
                    ThemeFonts.Body,
                    plotArea,
                    p.TextMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            double maxVal = 0;
            foreach (CategoryComparisonItem item in _items)
            {
                if (item.CurrentValue > maxVal) maxVal = item.CurrentValue;
                if (item.LastValue > maxVal) maxVal = item.LastValue;
            }
            if (maxVal <= 0) maxVal = 1;

            // Horizontal bar layout for readable category names
            int rowHeight = Math.Max(34, Math.Min(54, plotArea.Height / Math.Max(1, _items.Count)));
            int labelColWidth = Math.Min(135, Math.Max(85, plotArea.Width / 3));
            int valueColWidth = 78;
            int barAreaLeft = plotArea.X + labelColWidth + 8;
            int barAreaWidth = Math.Max(40, plotArea.Width - labelColWidth - valueColWidth - 16);

            using (SolidBrush bLast = new SolidBrush(lastColor))
            using (SolidBrush bCur = new SolidBrush(currentColor))
            using (Pen gridPen = new Pen(ColorUtil.WithAlpha(p.Border, 140), 1F))
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    int y = plotArea.Y + i * rowHeight;
                    if (y + rowHeight > plotArea.Bottom + 4)
                        break;

                    CategoryComparisonItem item = _items[i];

                    if (i > 0)
                        g.DrawLine(gridPen, plotArea.X, y - 2, plotArea.Right, y - 2);

                    Rectangle catRect = new Rectangle(plotArea.X, y, labelColWidth, rowHeight - 4);
                    TextRenderer.DrawText(
                        g, item.CategoryName, ThemeFonts.Small, catRect, p.TextPrimary,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                    int singleBarH = Math.Max(7, (rowHeight - 14) / 2);
                    int gap = 3;
                    int topBarY = y + Math.Max(3, (rowHeight - (singleBarH * 2 + gap)) / 2);

                    int lastW = (int)Math.Round((item.LastValue / maxVal) * barAreaWidth);
                    int curW = (int)Math.Round((item.CurrentValue / maxVal) * barAreaWidth);

                    if (item.LastValue > 0 && lastW < 3) lastW = 3;
                    if (item.CurrentValue > 0 && curW < 3) curW = 3;

                    if (lastW > 0)
                        g.FillRectangle(bLast, barAreaLeft, topBarY, lastW, singleBarH);

                    if (curW > 0)
                        g.FillRectangle(bCur, barAreaLeft, topBarY + singleBarH + gap, curW, singleBarH);

                    Rectangle valRect = new Rectangle(
                        barAreaLeft + barAreaWidth + 6,
                        y,
                        valueColWidth,
                        rowHeight - 4);

                    string curText = FormatCompact(item.CurrentValue);
                    string lastText = FormatCompact(item.LastValue);

                    TextRenderer.DrawText(
                        g, curText, ThemeFonts.Tiny,
                        new Rectangle(valRect.X, topBarY - 1, valRect.Width, singleBarH + 2),
                        p.TextPrimary,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                    TextRenderer.DrawText(
                        g, lastText, ThemeFonts.Tiny,
                        new Rectangle(valRect.X, topBarY + singleBarH + gap - 1, valRect.Width, singleBarH + 2),
                        p.TextMuted,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                }
            }
        }

        private static string FormatCompact(double value)
        {
            if (Math.Abs(value) >= 10000000D)
                return (value / 10000000D).ToString("0.##", CultureInfo.InvariantCulture) + "Cr";
            if (Math.Abs(value) >= 100000D)
                return (value / 100000D).ToString("0.##", CultureInfo.InvariantCulture) + "L";
            if (Math.Abs(value) >= 1000D)
                return (value / 1000D).ToString("0.#", CultureInfo.InvariantCulture) + "K";
            return value.ToString("0.##", CultureInfo.InvariantCulture);
        }
    }
}