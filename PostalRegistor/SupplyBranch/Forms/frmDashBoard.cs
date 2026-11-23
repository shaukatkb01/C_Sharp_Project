using SupplyBranch.Controls;
using SupplyBranch.Forms.Reports;
using SupplyBranch.Forms.Stock;
using SupplyBranch.Forms.Transactions;
using SupplyBranch.Helpers;
using StampStoreApp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmDashBoard : Form
    {
        private readonly DBHelper db = new DBHelper();
        private readonly Label[] statValues = new Label[4];

        private Label taskEmptyState;
        private Label supplyEmptyState;
        private ThemedLabel lblTaskComboHint;
        private DataTable suppliesData;
        private string currentUserName = "User";

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
            Invalidate(true);
        }

        // =============================================================
        //  Layout
        // =============================================================

        private void BuildDashboardLayout()
        {
            // Detach and dispose the legacy designer layout but keep the
            // DataGridViews and the ComboBox so existing data logic still works.
            panel1.Controls.Remove(cmb_Task);
            panel1.Controls.Remove(btnAdd);
            panel1.Controls.Remove(button1);

            splitContainerMain.Panel1.Controls.Remove(dgvTasks);
            splitContainerMain.Panel2.Controls.Remove(dgvSuppliesBottom);

            Controls.Clear();

            panel1.Dispose();
            splitContainerMain.Dispose();
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

            Panel titlePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            ThemedLabel title = new ThemedLabel
            {
                Text = "Welcome back, " + currentUserName,
                Role = LabelRole.Primary,
                Font = ThemeFonts.H1,
                AutoSize = true,
                Location = new Point(0, 4)
            };

            ThemedLabel date = new ThemedLabel
            {
                Text = DateTime.Today.ToString("dddd, dd MMMM yyyy"),
                Role = LabelRole.Muted,
                Font = ThemeFonts.Small,
                AutoSize = true,
                Location = new Point(2, 40)
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
                WrapContents = false,
                AutoScroll = true,
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
            workArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            workArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            workArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Control taskSection = CreateSection("My Tasks", dgvTasks, out taskEmptyState, true);
            Control supplySection = CreateSection(
                "Recent Draft & Approved Supplies", dgvSuppliesBottom, out supplyEmptyState, false);

            taskSection.Margin = new Padding(0, 0, 12, 0);
            supplySection.Margin = new Padding(0);

            workArea.Controls.Add(taskSection, 0, 0);
            workArea.Controls.Add(supplySection, 1, 0);
            return workArea;
        }

        private Control CreateSection(
            string heading,
            DataGridView grid,
            out Label emptyState,
            bool withTaskToolbar)
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
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, withTaskToolbar ? 44F : 8F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ThemedLabel title = new ThemedLabel
            {
                Text = heading,
                Role = LabelRole.Primary,
                Font = ThemeFonts.H3,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };

            layout.Controls.Add(title, 0, 0);

            if (withTaskToolbar)
                layout.Controls.Add(CreateTaskToolbar(), 0, 1);
            else
                layout.Controls.Add(new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Transparent,
                    Margin = new Padding(0)
                }, 0, 1);

            ThemedPanel gridHost = new ThemedPanel
            {
                Dock = DockStyle.Fill,
                Role = PanelRole.Surface,
                Padding = new Padding(0, 4, 0, 0),
                Margin = new Padding(0)
            };

            grid.Dock = DockStyle.Fill;
            grid.Margin = new Padding(0);
            GridStyler.Apply(grid);

            emptyState = new ThemedLabel
            {
                Dock = DockStyle.Fill,
                Role = LabelRole.Muted,
                Font = ThemeFonts.Body,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
                BackColor = Color.Transparent
            };

            gridHost.Controls.Add(grid);
            gridHost.Controls.Add(emptyState);
            emptyState.BringToFront();

            layout.Controls.Add(gridHost, 0, 2);
            card.Controls.Add(layout);
            return card;
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
        //  Supplies bottom grid
        // =============================================================

        private void LoadSuppliesBottom()
        {
            try
            {
                string[] statuses = new string[] { "Draft", "Approve" };

                SupplyBranch.DataAccess.SupplyDAL dal = new SupplyBranch.DataAccess.SupplyDAL();
                DataTable dt = dal.GetSuppliesByStatusNames(statuses);

                suppliesData = dt;
                dgvSuppliesBottom.DataSource = dt;

                UpdateSupplyStatistics();
                UpdateEmptyStates();

                if (dgvSuppliesBottom.Columns.Contains("SupplyID"))
                    dgvSuppliesBottom.Columns["SupplyID"].Visible = false;

                if (dgvSuppliesBottom.Columns.Contains("IndentID"))
                    dgvSuppliesBottom.Columns["IndentID"].Visible = false;

                if (dgvSuppliesBottom.Columns.Contains("StatusID"))
                    dgvSuppliesBottom.Columns["StatusID"].Visible = false;

                if (dgvSuppliesBottom.Columns.Contains("SupplyNo"))
                    dgvSuppliesBottom.Columns["SupplyNo"].HeaderText = "Supply No";

                if (dgvSuppliesBottom.Columns.Contains("SupplyDate"))
                    dgvSuppliesBottom.Columns["SupplyDate"].HeaderText = "Supply Date";

                if (dgvSuppliesBottom.Columns.Contains("OfficeName"))
                    dgvSuppliesBottom.Columns["OfficeName"].HeaderText = "Office";

                if (dgvSuppliesBottom.Columns.Contains("TotalSheet"))
                {
                    dgvSuppliesBottom.Columns["TotalSheet"].HeaderText = "Total Sheet";
                    dgvSuppliesBottom.Columns["TotalSheet"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dgvSuppliesBottom.Columns["TotalSheet"].Width = 100;
                }
                else if (dgvSuppliesBottom.Columns.Contains("TotalPieces"))
                {
                    dgvSuppliesBottom.Columns["TotalPieces"].HeaderText = "Total Sheet";
                    dgvSuppliesBottom.Columns["TotalPieces"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dgvSuppliesBottom.Columns["TotalPieces"].Width = 100;
                }

                if (dgvSuppliesBottom.Columns.Contains("TotalPieces") &&
                    dgvSuppliesBottom.Columns.Contains("TotalSheet"))
                {
                    dgvSuppliesBottom.Columns["TotalPieces"].Visible = false;
                }

                foreach (DataGridViewColumn col in dgvSuppliesBottom.Columns)
                {
                    if (col.Name == "OfficeName" || col.Name == "IndentNo")
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.MinimumWidth = 150;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    else if (col.ValueType == typeof(DateTime) ||
                             col.Name.ToLower().Contains("date"))
                    {
                        col.Width = 110;
                        col.DefaultCellStyle.Format = "dd-MMM-yyyy";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else if (col.ValueType == typeof(int) ||
                             col.ValueType == typeof(decimal) ||
                             col.Name.ToLower().Contains("total") ||
                             col.Name.ToLower().Contains("sheet"))
                    {
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        if (col.Width < 80)
                            col.Width = 90;
                    }
                    else
                    {
                        col.MinimumWidth = 80;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load supplies:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =============================================================
        //  Form load
        // =============================================================

        private void frmDashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CurrentUser.UserName))
                    currentUserName = CurrentUser.UserName;

                LoadTasks();
                FillCompletedTasksCombo();
                ConfigureDataGridView();
                UpdateStatistics();
                LoadSuppliesBottom();
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
            LoadSuppliesBottom();
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

            if (supplyEmptyState != null)
            {
                supplyEmptyState.Text = "No draft or approved supplies found.";
                supplyEmptyState.Visible = dgvSuppliesBottom.Rows.Count == 0;
            }
        }
    }
}
