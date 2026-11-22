using SupplyBranch.Forms.Administration;
using SupplyBranch.Forms.Masters;
using SupplyBranch.Forms.Reports;
using SupplyBranch.Forms.Stock;
using SupplyBranch.Forms.Transactions;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.Forms
{
    public partial class frmDashBoard : Form
    {
        private readonly DBHelper db = new DBHelper();
        private readonly Label[] statValues = new Label[4];
        private Label taskEmptyState;
        private Label supplyEmptyState;
        private DataTable suppliesData;

        public event Action<Form> NavigationRequested;

        #region Properties & Variables

        private string currentUserName = "Ali"; // Ya GlobalData.CurrentUser se lo

        #endregion

        #region Supplies Bottom Grid

        private void LoadSuppliesBottom()
        {
            try
            {
                // Use status names instead of numeric IDs
                string[] statuses = new string[] { "Draft", "Approve" };

                SupplyBranch.DataAccess.SupplyDAL dal = new SupplyBranch.DataAccess.SupplyDAL();

                DataTable dt = dal.GetSuppliesByStatusNames(statuses);
                suppliesData = dt;
                dgvSuppliesBottom.DataSource = dt;
                UpdateSupplyStatistics();
                UpdateEmptyStates();

                // Basic visibility and header text
                if (dgvSuppliesBottom.Columns.Contains("SupplyID"))
                    dgvSuppliesBottom.Columns["SupplyID"].Visible = false;

                if (dgvSuppliesBottom.Columns.Contains("IndentID"))
                    dgvSuppliesBottom.Columns["IndentID"].Visible = false; // hide completely

                if (dgvSuppliesBottom.Columns.Contains("StatusID"))
                    dgvSuppliesBottom.Columns["StatusID"].Visible = false; // hide completely

                if (dgvSuppliesBottom.Columns.Contains("SupplyNo"))
                    dgvSuppliesBottom.Columns["SupplyNo"].HeaderText = "Supply No";

                if (dgvSuppliesBottom.Columns.Contains("SupplyDate"))
                    dgvSuppliesBottom.Columns["SupplyDate"].HeaderText = "Supply Date";

                if (dgvSuppliesBottom.Columns.Contains("OfficeName"))
                    dgvSuppliesBottom.Columns["OfficeName"].HeaderText = "Office";

                // Replace TotalPieces with TotalSheet for display. Prefer TotalSheet if present.
                if (dgvSuppliesBottom.Columns.Contains("TotalSheet"))
                {
                    dgvSuppliesBottom.Columns["TotalSheet"].HeaderText = "Total Sheet";
                    dgvSuppliesBottom.Columns["TotalSheet"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvSuppliesBottom.Columns["TotalSheet"].Width = 100;
                }
                else if (dgvSuppliesBottom.Columns.Contains("TotalPieces"))
                {
                    // Fallback: keep existing data but show as Total Sheet
                    dgvSuppliesBottom.Columns["TotalPieces"].HeaderText = "Total Sheet";
                    dgvSuppliesBottom.Columns["TotalPieces"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvSuppliesBottom.Columns["TotalPieces"].Width = 100;
                }

                // Hide the original TotalPieces column if both exist
                if (dgvSuppliesBottom.Columns.Contains("TotalPieces") && dgvSuppliesBottom.Columns.Contains("TotalSheet"))
                    dgvSuppliesBottom.Columns["TotalPieces"].Visible = false;

                // Visual improvements
                dgvSuppliesBottom.EnableHeadersVisualStyles = false;
                dgvSuppliesBottom.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 62, 80);
                dgvSuppliesBottom.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvSuppliesBottom.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dgvSuppliesBottom.RowTemplate.Height = 30;
                dgvSuppliesBottom.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 144, 255);
                dgvSuppliesBottom.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvSuppliesBottom.BackgroundColor = Color.WhiteSmoke;
                dgvSuppliesBottom.BorderStyle = BorderStyle.None;
                dgvSuppliesBottom.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvSuppliesBottom.RowHeadersVisible = false;

                // Column sizing and alignment tweaks
                foreach (DataGridViewColumn col in dgvSuppliesBottom.Columns)
                {
                    if (col.Name == "OfficeName" || col.Name == "IndentNo")
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.MinimumWidth = 150;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    else if (col.ValueType == typeof(DateTime) || col.Name.ToLower().Contains("date"))
                    {
                        col.Width = 110;
                        col.DefaultCellStyle.Format = "dd-MMM-yyyy";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else if (col.ValueType == typeof(int) || col.ValueType == typeof(decimal) || col.Name.ToLower().Contains("total") || col.Name.ToLower().Contains("sheet"))
                    {
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        if (col.Width < 80) col.Width = 90;
                    }
                    else
                    {
                        // default small padding for readability
                        col.MinimumWidth = 80;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load supplies:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Constructor & Form Load

        public frmDashBoard()
        {
            InitializeComponent();
            BuildDashboardLayout();
        }

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

            BackColor = Color.FromArgb(244, 247, 251);
            AutoScroll = true;
            Font = new Font("Segoe UI", 9F);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18),
                ColumnCount = 1,
                RowCount = 4,
                BackColor = BackColor
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 102F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            layout.Controls.Add(CreateDashboardHeader(), 0, 0);
            layout.Controls.Add(CreateQuickActions(), 0, 1);
            layout.Controls.Add(CreateStatisticsCards(), 0, 2);
            layout.Controls.Add(CreateWorkArea(), 0, 3);
            Controls.Add(layout);
        }

        private Control CreateDashboardHeader()
        {
            TableLayoutPanel header = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));

            Panel titlePanel = new Panel { Dock = DockStyle.Fill };
            Label title = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 43, 60),
                Location = new Point(0, 0),
                Text = "Welcome back, " + currentUserName
            };
            Label date = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(2, 32),
                Text = DateTime.Today.ToString("dddd, dd MMMM yyyy")
            };
            titlePanel.Controls.Add(title);
            titlePanel.Controls.Add(date);

            Button refresh = CreateActionButton("Refresh", Color.FromArgb(37, 99, 235));
            refresh.Text = "Refresh";
            refresh.ForeColor = Color.White;
            refresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            refresh.TextAlign = ContentAlignment.MiddleCenter;
            refresh.AutoEllipsis = false;
            refresh.Dock = DockStyle.None;
            refresh.Size = new Size(105, 50);
            refresh.Anchor = AnchorStyles.Right;
            refresh.Margin = new Padding(4, 6, 10, 6);
            refresh.Click += (sender, args) => RefreshData();
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
                BackColor = Color.Transparent,
                Padding = new Padding(0, 6, 0, 0)
            };

            actions.Controls.Add(CreateNavigationButton("New Indent", () => new frmIndent(), true, "INDENT"));
            actions.Controls.Add(CreateNavigationButton("Generate Draft", () => new frmSupplySearch(), false, "SUPPLY"));
            actions.Controls.Add(CreateNavigationButton("Draft Supplies", () => new frmDraftSupply(), false, "SUPPLY"));
            actions.Controls.Add(CreateNavigationButton("Reports", () => new frmReport(), false, "REPORTS"));
            actions.Controls.Add(CreateNavigationButton("Stock Balance", () => new frmStockBalance(), false, "STOCK"));
            actions.Controls.Add(CreateNavigationButton("BlankPerforma", () => new frmReportPreview(), false, "REPORTS"));

            return actions;
        }
        private Control CreateStatisticsCards()
        {
            TableLayoutPanel cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 8)
            };
            for (int i = 0; i < 4; i++)
                cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            string[] titles = { "Pending tasks", "Critical tasks", "Draft supplies", "Approved supplies" };
            Color[] accents =
            {
                Color.FromArgb(37, 99, 235),
                Color.FromArgb(220, 38, 38),
                Color.FromArgb(217, 119, 6),
                Color.FromArgb(5, 150, 105)
            };

            for (int i = 0; i < titles.Length; i++)
            {
                Panel card = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(0, 0, i == titles.Length - 1 ? 0 : 10, 0)
                };
                Panel accent = new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accents[i] };
                Label value = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 21F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(32, 43, 60),
                    Location = new Point(18, 8),
                    Text = "0"
                };
                Label title = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Location = new Point(20, 60),
                    Text = titles[i]
                };
                card.Controls.Add(title);
                card.Controls.Add(value);
                card.Controls.Add(accent);
                statValues[i] = value;
                cards.Controls.Add(card, i, 0);
            }
            return cards;
        }

        private Control CreateWorkArea()
        {
            TableLayoutPanel workArea = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 0)
            };
            workArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            workArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));

            Panel taskSection = CreateSection("My Tasks", dgvTasks, out taskEmptyState);
            Panel supplySection = CreateSection("Recent Draft & Approved Supplies", dgvSuppliesBottom, out supplyEmptyState);
            taskSection.Margin = new Padding(0, 0, 10, 0);
            supplySection.Margin = new Padding(0);
            workArea.Controls.Add(taskSection, 0, 0);
            workArea.Controls.Add(supplySection, 1, 0);
            return workArea;
        }

        private Panel CreateSection(string heading, DataGridView grid, out Label emptyState)
        {
            Panel section = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12)
            };
            Label title = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 43, 60),
                Text = heading,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Panel content = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            grid.Dock = DockStyle.Fill;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 244, 250);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(226, 232, 240);
            grid.RowTemplate.Height = 30;

            content.Controls.Add(grid);
            emptyState = new Label
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(100, 116, 139),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            content.Controls.Add(emptyState);
            emptyState.BringToFront();

            if (grid == dgvTasks)
            {
                TableLayoutPanel taskHeader = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    Height = 40,
                    ColumnCount = 3,
                    RowCount = 1,
                    BackColor = Color.White
                };
                taskHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                taskHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
                taskHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82F));
                cmb_Task.Dock = DockStyle.Fill;
                cmb_Task.Margin = new Padding(0, 3, 5, 5);
                btnAdd.Text = "Add task";
                button1.Text = "Delete";
                StyleSmallButton(btnAdd, Color.FromArgb(37, 99, 235));
                StyleSmallButton(button1, Color.FromArgb(220, 38, 38));
                btnAdd.Dock = DockStyle.Fill;
                button1.Dock = DockStyle.Fill;
                btnAdd.Margin = new Padding(0, 2, 5, 5);
                button1.Margin = new Padding(0, 2, 0, 5);
                taskHeader.Controls.Add(cmb_Task, 0, 0);
                taskHeader.Controls.Add(btnAdd, 1, 0);
                taskHeader.Controls.Add(button1, 2, 0);
                section.Controls.Add(content);
                section.Controls.Add(taskHeader);
            }
            else
            {
                section.Controls.Add(content);
            }
            section.Controls.Add(title);
            return section;
        }

        private Control CreateNavigationButton(
     string text,
     Func<Form> formFactory,
     bool isPrimary,
     string moduleCode = null)
        {
            Button button = CreateActionButton(
                text,
                isPrimary ? Color.FromArgb(37, 99, 235) : Color.White);

            button.Tag = moduleCode;

            if (!isPrimary)
            {
                button.ForeColor = Color.FromArgb(51, 65, 85);
                button.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            }

            button.Margin = new Padding(0, 0, 8, 4);

            button.Click += (sender, args) =>
            {
                if (formFactory == null)
                    return;

                Form target = formFactory();
                Action<Form> navigate = NavigationRequested;

                if (navigate == null)
                {
                    target?.Dispose();
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

        private Button CreateActionButton(string text, Color backColor)
        {
            Button button = new Button
            {
                Text = text,
                Height = 36,
                Width = 124,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void StyleSmallButton(Button button, Color backColor)
        {
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.UseVisualStyleBackColor = false;
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
            statValues[2].Text = draftCount.ToString();
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

        private void frmDashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Theme apply karo

                // 2. Current user set karo (agar GlobalData use kar rahe ho)
                if (!string.IsNullOrEmpty(CurrentUser.UserName))
                {
                    currentUserName = CurrentUser.UserName;
                }

                // 3. Tasks load karo
                LoadTasks();

                // 4. Completed tasks combo fill karo
                FillCompletedTasksCombo();

                // 5. DataGridView columns setup
                ConfigureDataGridView();

                // 6. Statistics update karo (optional)
                UpdateStatistics();

                // 7. Supplies bottom grid (Draft / Approved)
                LoadSuppliesBottom();
                UpdateEmptyStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Form load error:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region DataGridView Configuration

        /// <summary>
        /// DataGridView columns ko properly configure karta hai
        /// </summary>
        private void ConfigureDataGridView()
        {
            try
            {
                ApplyDashboardGridStyle(dgvTasks);

                // TaskID column (agar visible hai to hide karo)
                if (dgvTasks.Columns.Contains("TaskID"))
                {
                    dgvTasks.Columns["TaskID"].Visible = false;
                }

                // TaskDescription column
                if (dgvTasks.Columns.Contains("TaskDescription"))
                {
                    dgvTasks.Columns["TaskDescription"].HeaderText = "Task";
                    dgvTasks.Columns["TaskDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvTasks.Columns["TaskDescription"].MinimumWidth = 200;
                }

                // IsCompleted column (hide karo - color se dikhega)
                if (dgvTasks.Columns.Contains("IsCompleted"))
                {
                    dgvTasks.Columns["IsCompleted"].Visible = false;
                }

                // Priority column (hide karo - numeric value)
                if (dgvTasks.Columns.Contains("Priority"))
                {
                    dgvTasks.Columns["Priority"].Visible = false;
                }

                // PriorityText column (text display)
                if (dgvTasks.Columns.Contains("PriorityText"))
                {
                    dgvTasks.Columns["PriorityText"].HeaderText = "Priority";
                    dgvTasks.Columns["PriorityText"].Width = 100;
                    dgvTasks.Columns["PriorityText"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // CreatedDate column
                if (dgvTasks.Columns.Contains("CreatedDate"))
                {
                    dgvTasks.Columns["CreatedDate"].HeaderText = "Created";
                    dgvTasks.Columns["CreatedDate"].Width = 100;
                    dgvTasks.Columns["CreatedDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                }

                // DueDate column
                if (dgvTasks.Columns.Contains("DueDate"))
                {
                    dgvTasks.Columns["DueDate"].HeaderText = "Due Date";
                    dgvTasks.Columns["DueDate"].Width = 100;
                    dgvTasks.Columns["DueDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                }

                // Row height
                dgvTasks.RowTemplate.Height = 35;

                // Selection mode
                dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvTasks.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Grid configuration error: {ex.Message}");
            }
        }

        #endregion

        private void ApplyDashboardGridStyle(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 244, 250);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(226, 232, 240);
            grid.RowTemplate.Height = 30;
            grid.RowHeadersVisible = false;
        }

        #region Load Tasks

        /// <summary>
        /// Tasks ko grid mein load karta hai
        /// </summary>
        private void LoadTasks(bool showCompleted = true)
        {
            try
            {
                TaskHelper.LoadTasksToGrid(dgvTasks, currentUserName, showCompleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error loading tasks:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Data refresh (alias for LoadTasks)
        /// </summary>
        private void RefreshData()
        {
            LoadTasks(showCompleted: true);
            FillCompletedTasksCombo();
            UpdateStatistics();
            LoadSuppliesBottom();
            UpdateEmptyStates();
        }

        #endregion

        #region Fill Completed Tasks ComboBox

        /// <summary>
        /// Completed tasks ko combobox mein fill karta hai
        /// </summary>
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
            new SqlParameter("@UserName", CurrentUser.UserName)
        };

                DataTable dt = db.ExecuteQuery(query, parameters);

                

                cmb_Task.DataSource = null;
                cmb_Task.DisplayMember = "TaskDescription";
                cmb_Task.ValueMember = "TaskID";
                cmb_Task.DataSource = dt;

                // No task selected initially
                cmb_Task.SelectedIndex = -1;
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

        #endregion

        #region Add Task Button

        /// <summary>
        /// Add task button click event
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (AddTaskForm form = new AddTaskForm(CurrentUser.UserName))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show(
                            "Task added successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Refresh To-Do List
                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error opening Add Task form.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region DataGridView Cell Double Click (Toggle Complete/Incomplete)

        /// <summary>
        /// Double-click par task ko complete/incomplete toggle karta hai
        /// </summary>
        private void dgvTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Header click ignore karo
            if (e.RowIndex < 0) return;

            try
            {
                // TaskID get karo
                int taskId = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells["TaskID"].Value);

                // Current status get karo
                bool currentStatus = dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value != DBNull.Value
                    && Convert.ToBoolean(dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value);

                // Status toggle karo
                bool newStatus = !currentStatus;

                // Update in database
                bool success = TaskHelper.UpdateTaskStatus(taskId, newStatus);

                if (success)
                {
                    // Refresh grid
                    RefreshData();

                    // User feedback (optional - comment out if annoying)
                    string statusText = newStatus ? "completed" : "reopened";
                    // MessageBox.Show($"✅ Task {statusText}!", "Success", 
                    //     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error updating task status:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Delete Completed Task Button

        /// <summary>
        /// Delete button click - selected completed task ko delete karta hai
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check agar koi task selected hai
                if (cmb_Task.SelectedIndex == -1 || cmb_Task.SelectedValue == null)
                {
                    MessageBox.Show("⚠️ Please select a task to delete!",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    cmb_Task.Focus();
                    return;
                }

                // TaskID get karo
                int taskId = Convert.ToInt32(cmb_Task.SelectedValue);

                // Confirmation
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this completed task?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Delete task
                bool success = TaskHelper.DeleteTask(taskId);

                if (success)
                {
                    // Success message handled in TaskHelper.DeleteTask
                    // Refresh everything
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error deleting task:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Update Statistics (Optional)

        /// <summary>
        /// Task statistics ko update karta hai (agar form par labels hain)
        /// </summary>
        private void UpdateStatistics()
        {
            var stats = TaskHelper.GetTaskStatistics(currentUserName);
            statValues[0].Text = stats.PendingTasks.ToString();
            statValues[1].Text = stats.CriticalPending.ToString();
        }

        #endregion

        #region Context Menu for DataGridView (Optional - Add if needed)

        /// <summary>
        /// Right-click context menu setup (optional)
        /// </summary>
        private void SetupContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            // Mark Complete
            ToolStripMenuItem markComplete = new ToolStripMenuItem("✓ Mark as Complete");
            markComplete.Click += (s, e) =>
            {
                if (dgvTasks.SelectedRows.Count > 0)
                {
                    int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
                    TaskHelper.UpdateTaskStatus(taskId, true);
                    RefreshData();
                }
            };

            // Mark Incomplete
            ToolStripMenuItem markIncomplete = new ToolStripMenuItem("↻ Mark as Incomplete");
            markIncomplete.Click += (s, e) =>
            {
                if (dgvTasks.SelectedRows.Count > 0)
                {
                    int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
                    TaskHelper.UpdateTaskStatus(taskId, false);
                    RefreshData();
                }
            };

            // Change Priority
            ToolStripMenuItem changePriority = new ToolStripMenuItem("🎨 Change Priority");
            changePriority.Click += ChangePriority_Click;

            // Delete
            ToolStripMenuItem deleteTask = new ToolStripMenuItem("🗑️ Delete Task");
            deleteTask.Click += (s, e) =>
            {
                if (dgvTasks.SelectedRows.Count > 0)
                {
                    int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
                    TaskHelper.DeleteTask(taskId);
                    RefreshData();
                }
            };

            cms.Items.Add(markComplete);
            cms.Items.Add(markIncomplete);
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add(changePriority);
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add(deleteTask);

            dgvTasks.ContextMenuStrip = cms;
        }

        /// <summary>
        /// Change priority dialog
        /// </summary>
        private void ChangePriority_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0) return;

            try
            {
                int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);

                using (Form priorityForm = new Form())
                {
                    priorityForm.Text = "Change Priority";
                    priorityForm.Size = new Size(320, 180);
                    priorityForm.StartPosition = FormStartPosition.CenterParent;
                    priorityForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    priorityForm.MaximizeBox = false;
                    priorityForm.MinimizeBox = false;

                    Label lbl = new Label
                    {
                        Text = "Select new priority:",
                        Location = new Point(20, 20),
                        Size = new Size(260, 20)
                    };

                    ComboBox cmb = new ComboBox
                    {
                        Location = new Point(20, 50),
                        Size = new Size(260, 25),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    cmb.Items.AddRange(new object[] {
                        "🟢 Low Priority",
                        "🟠 Medium Priority",
                        "🔴 High Priority",
                        "🔥 Critical Priority"
                    });
                    cmb.SelectedIndex = 1;

                    Button btnOk = new Button
                    {
                        Text = "✓ OK",
                        Location = new Point(120, 100),
                        Size = new Size(80, 30),
                        DialogResult = DialogResult.OK
                    };

                    Button btnCancel = new Button
                    {
                        Text = "✗ Cancel",
                        Location = new Point(210, 100),
                        Size = new Size(80, 30),
                        DialogResult = DialogResult.Cancel
                    };

                    priorityForm.Controls.Add(lbl);
                    priorityForm.Controls.Add(cmb);
                    priorityForm.Controls.Add(btnOk);
                    priorityForm.Controls.Add(btnCancel);
                    priorityForm.AcceptButton = btnOk;
                    priorityForm.CancelButton = btnCancel;

                    if (priorityForm.ShowDialog() == DialogResult.OK && cmb.SelectedIndex != -1)
                    {
                        int newPriority = cmb.SelectedIndex + 1;

                        if (TaskHelper.UpdateTaskPriority(taskId, newPriority))
                        {
                            MessageBox.Show("✅ Priority updated!",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Removed/Deprecated Methods

        /// <summary>
        /// ApplyFormatting() - NO LONGER NEEDED
        /// TaskHelper.ApplyPriorityColors() already handles this
        /// </summary>
        // private void ApplyFormatting() { } // REMOVED

        #endregion

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (AddTaskForm form = new AddTaskForm(currentUserName))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Success message
                        MessageBox.Show("✅ Task added successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Refresh everything
                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error opening add task form:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Check agar koi task selected hai
                if (cmb_Task.SelectedIndex == -1 || cmb_Task.SelectedValue == null)
                {
                    MessageBox.Show("⚠️ Please select a task to delete!",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    cmb_Task.Focus();
                    return;
                }

                // TaskID get karo
                int taskId = Convert.ToInt32(cmb_Task.SelectedValue);

                // Confirmation
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this completed task?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Delete task
                bool success = TaskHelper.DeleteTask(taskId);

                if (success)
                {
                    // Success message handled in TaskHelper.DeleteTask
                    // Refresh everything
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error deleting task:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvTasks_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Header click ignore karo
            if (e.RowIndex < 0) return;

            try
            {
                // TaskID get karo
                int taskId = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells["TaskID"].Value);

                // Current status get karo
                bool currentStatus = dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value != DBNull.Value
                    && Convert.ToBoolean(dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value);

                // Status toggle karo
                bool newStatus = !currentStatus;

                // Update in database
                bool success = TaskHelper.UpdateTaskStatus(taskId, newStatus);

                if (success)
                {
                    // Refresh grid
                    RefreshData();

                    // User feedback (optional - comment out if annoying)
                    string statusText = newStatus ? "completed" : "reopened";
                    // MessageBox.Show($"✅ Task {statusText}!", "Success", 
                    //     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error updating task status:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
