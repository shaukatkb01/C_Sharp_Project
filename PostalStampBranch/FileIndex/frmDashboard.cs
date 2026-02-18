using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostalStampSystem;

namespace FileIndex
{
    public partial class frmDashboard : Form
    {
        #region Properties & Variables

        private string currentUserName = "Ali"; // Ya GlobalData.CurrentUser se lo

        #endregion

        #region Constructor & Form Load

        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Theme apply karo
                ThemeManager.ApplyTheme(this);

                // 2. Current user set karo (agar GlobalData use kar rahe ho)
                if (!string.IsNullOrEmpty(GlobalData.CurrentUser))
                {
                    currentUserName = GlobalData.CurrentUser;
                }

                // 3. Tasks load karo
                LoadTasks();

                // 4. Completed tasks combo fill karo
                FillCompletedTasksCombo();

                // 5. DataGridView columns setup
                ConfigureDataGridView();

                // 6. Statistics update karo (optional)
                UpdateStatistics();
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
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    // ✅ FIXED: Priority bhi fetch karo
                    string query = @"SELECT TaskID, TaskDescription, Priority
                                   FROM ToDoList 
                                   WHERE IsCompleted = 1 
                                     AND UserName = @user
                                   ORDER BY Priority DESC, TaskID DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", currentUserName);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // ComboBox ko data assign
                    cmb_Task.DataSource = dt;
                    cmb_Task.DisplayMember = "TaskDescription";
                    cmb_Task.ValueMember = "TaskID";

                    // Default: No selection
                    cmb_Task.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Combo box error:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                var stats = TaskHelper.GetTaskStatistics(currentUserName);

                // Agar form par yeh labels hain to update karo
                // Warna comment out kar do

                /*
                if (lblTotalTasks != null)
                    lblTotalTasks.Text = $"Total: {stats.TotalTasks}";

                if (lblCompleted != null)
                    lblCompleted.Text = $"✓ Completed: {stats.CompletedTasks}";

                if (lblPending != null)
                    lblPending.Text = $"⏳ Pending: {stats.PendingTasks}";

                if (lblCritical != null)
                    lblCritical.Text = $"🔥 Critical: {stats.CriticalPending}";

                if (lblHigh != null)
                    lblHigh.Text = $"🔴 High: {stats.HighPending}";

                if (lblOverdue != null)
                    lblOverdue.Text = $"⚠️ Overdue: {stats.OverdueTasks}";

                if (progressBar1 != null)
                    progressBar1.Value = (int)stats.CompletionPercentage;
                */
            }
            catch
            {
                // Statistics optional hain, error ignore karo
            }
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
    }
}