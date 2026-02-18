//// ═══════════════════════════════════════════════════════════════════════════════
//// SIMPLE INLINE APPROACH - Main Form Mein Hi Task Add Karo
//// (Bina separate dialog ke - directly form par controls rakhkar)
//// ═══════════════════════════════════════════════════════════════════════════════

//using System;
//using System.Drawing;
//using System.Windows.Forms;

//namespace FileIndex
//{
//    public partial class ToDoListForm : Form
//    {
//        // ─────────────────────────────────────────────────────────────────
//        // DESIGNER CONTROLS (Form Designer mein add karein)
//        // ─────────────────────────────────────────────────────────────────
        
//        /*
//        private GroupBox grpAddTask;
//        private TextBox txtTask;
//        private ComboBox cmbPriority;
//        private CheckBox chkDueDate;
//        private DateTimePicker dtpDueDate;
//        private Button btnAddTask;
//        private Button btnClear;
//        private DataGridView dgvTasks;
//        private Label lblTaskCount;
//        private Label lblPendingCount;
//        private Label lblCriticalCount;
//        */

//        private string currentUserName = "Ali"; // Ya login user se lo

//        public ToDoListForm()
//        {
//            InitializeComponent();
//        }

//        private void ToDoListForm_Load(object sender, EventArgs e)
//        {
//            // Theme apply
//            // ThemeManager.ApplyTheme(this);
            
//            // Setup Priority ComboBox
//            SetupPriorityComboBox();
            
//            // Load tasks
//            LoadTasks();
            
//            // Show statistics
//            UpdateStatistics();
            
//            // Set focus
//            txtTask.Focus();
//        }

//        // ═══════════════════════════════════════════════════════════════
//        // SETUP METHODS
//        // ═══════════════════════════════════════════════════════════════

//        private void SetupPriorityComboBox()
//        {
//            cmbPriority.Items.Clear();
//            cmbPriority.Items.Add("🟢 Low Priority");
//            cmbPriority.Items.Add("🟠 Medium Priority");
//            cmbPriority.Items.Add("🔴 High Priority");
//            cmbPriority.Items.Add("🔥 Critical Priority");
//            cmbPriority.SelectedIndex = 1; // Default: Medium
//            cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
//        }

//        private void LoadTasks()
//        {
//            TaskHelper.LoadTasksToGrid(dgvTasks, currentUserName, showCompleted: true);
//        }

//        private void UpdateStatistics()
//        {
//            var stats = TaskHelper.GetTaskStatistics(currentUserName);
            
//            lblTaskCount.Text = $"Total Tasks: {stats.TotalTasks}";
//            lblPendingCount.Text = $"⏳ Pending: {stats.PendingTasks}";
//            lblCriticalCount.Text = $"🔥 Critical: {stats.CriticalPending}";
//        }

//        // ═══════════════════════════════════════════════════════════════
//        // ADD TASK BUTTON CLICK
//        // ═══════════════════════════════════════════════════════════════

//        private void btnAddTask_Click(object sender, EventArgs e)
//        {
//            // ─────────────────────────────────────────────────────────
//            // STEP 1: Validation
//            // ─────────────────────────────────────────────────────────
            
//            if (string.IsNullOrWhiteSpace(txtTask.Text))
//            {
//                MessageBox.Show("⚠️ Please enter task description!", 
//                    "Validation Error", 
//                    MessageBoxButtons.OK, 
//                    MessageBoxIcon.Warning);
//                txtTask.Focus();
//                return;
//            }
            
//            if (cmbPriority.SelectedIndex == -1)
//            {
//                MessageBox.Show("⚠️ Please select priority!", 
//                    "Validation Error", 
//                    MessageBoxButtons.OK, 
//                    MessageBoxIcon.Warning);
//                cmbPriority.Focus();
//                cmbPriority.DroppedDown = true;
//                return;
//            }
            
//            // ─────────────────────────────────────────────────────────
//            // STEP 2: Get Values
//            // ─────────────────────────────────────────────────────────
            
//            string taskDescription = txtTask.Text.Trim();
//            int priority = cmbPriority.SelectedIndex + 1; // 1=Low, 2=Medium, 3=High, 4=Critical
//            DateTime? dueDate = chkDueDate.Checked ? dtpDueDate.Value.Date : (DateTime?)null;
            
//            // ─────────────────────────────────────────────────────────
//            // STEP 3: Add Task
//            // ─────────────────────────────────────────────────────────
            
//            bool success = TaskHelper.AddNewTask(
//                task: taskDescription,
//                userName: currentUserName,
//                priority: priority,
//                dueDate: dueDate
//            );
            
//            if (success)
//            {
//                // Success message
//                MessageBox.Show("✅ Task added successfully!", 
//                    "Success", 
//                    MessageBoxButtons.OK, 
//                    MessageBoxIcon.Information);
                
//                // Clear fields
//                ClearFields();
                
//                // Reload grid
//                LoadTasks();
                
//                // Update statistics
//                UpdateStatistics();
                
//                // Focus back to task textbox
//                txtTask.Focus();
//            }
//        }

//        // ═══════════════════════════════════════════════════════════════
//        // CLEAR FIELDS
//        // ═══════════════════════════════════════════════════════════════

//        private void btnClear_Click(object sender, EventArgs e)
//        {
//            ClearFields();
//        }

//        private void ClearFields()
//        {
//            txtTask.Clear();
//            cmbPriority.SelectedIndex = 1; // Reset to Medium
//            chkDueDate.Checked = false;
//            dtpDueDate.Value = DateTime.Now.AddDays(7);
//            txtTask.Focus();
//        }

//        // ═══════════════════════════════════════════════════════════════
//        // DUE DATE CHECKBOX CHANGE
//        // ═══════════════════════════════════════════════════════════════

//        private void chkDueDate_CheckedChanged(object sender, EventArgs e)
//        {
//            dtpDueDate.Enabled = chkDueDate.Checked;
            
//            if (chkDueDate.Checked)
//            {
//                dtpDueDate.Value = DateTime.Now.AddDays(7); // Default 7 days
//            }
//        }

//        // ═══════════════════════════════════════════════════════════════
//        // DATAGRIDVIEW EVENTS
//        // ═══════════════════════════════════════════════════════════════

//        // Double-click to mark complete/incomplete
//        private void dgvTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex < 0) return; // Header clicked
            
//            try
//            {
//                // Get task ID
//                int taskId = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells["TaskID"].Value);
                
//                // Get current status
//                bool currentStatus = Convert.ToBoolean(dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value);
                
//                // Toggle status
//                bool newStatus = !currentStatus;
                
//                // Update
//                TaskHelper.UpdateTaskStatus(taskId, newStatus);
                
//                // Reload
//                LoadTasks();
//                UpdateStatistics();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error: {ex.Message}", "Error", 
//                    MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Right-click context menu
//        private void dgvTasks_MouseUp(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Right)
//            {
//                var hitTest = dgvTasks.HitTest(e.X, e.Y);
                
//                if (hitTest.RowIndex >= 0)
//                {
//                    dgvTasks.ClearSelection();
//                    dgvTasks.Rows[hitTest.RowIndex].Selected = true;
                    
//                    ShowTaskContextMenu(e.Location);
//                }
//            }
//        }

//        private void ShowTaskContextMenu(Point location)
//        {
//            ContextMenuStrip cms = new ContextMenuStrip();
            
//            // Mark Complete
//            ToolStripMenuItem markComplete = new ToolStripMenuItem("✓ Mark as Complete");
//            markComplete.Click += (s, e) =>
//            {
//                if (dgvTasks.SelectedRows.Count > 0)
//                {
//                    int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
//                    TaskHelper.UpdateTaskStatus(taskId, true);
//                    LoadTasks();
//                    UpdateStatistics();
//                }
//            };
            
//            // Mark Incomplete
//            ToolStripMenuItem markIncomplete = new ToolStripMenuItem("↻ Mark as Incomplete");
//            markIncomplete.Click += (s, e) =>
//            {
//                if (dgvTasks.SelectedRows.Count > 0)
//                {
//                    int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
//                    TaskHelper.UpdateTaskStatus(taskId, false);
//                    LoadTasks();
//                    UpdateStatistics();
//                }
//            };
            
//            // Change Priority
//            ToolStripMenuItem changePriority = new ToolStripMenuItem("🎨 Change Priority");
//            changePriority.Click += ChangePriority_Click;
            
//            // Delete
//            ToolStripMenuItem deleteTask = new ToolStripMenuItem("🗑️ Delete Task");
//            deleteTask.Click += (s, e) =>
//            {
//                if (dgvTasks.SelectedRows.Count > 0)
//                {
//                    int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
                    
//                    if (TaskHelper.DeleteTask(taskId))
//                    {
//                        LoadTasks();
//                        UpdateStatistics();
//                    }
//                }
//            };
            
//            cms.Items.Add(markComplete);
//            cms.Items.Add(markIncomplete);
//            cms.Items.Add(new ToolStripSeparator());
//            cms.Items.Add(changePriority);
//            cms.Items.Add(new ToolStripSeparator());
//            cms.Items.Add(deleteTask);
            
//            cms.Show(dgvTasks, location);
//        }

//        // ═══════════════════════════════════════════════════════════════
//        // CHANGE PRIORITY
//        // ═══════════════════════════════════════════════════════════════

//        private void ChangePriority_Click(object sender, EventArgs e)
//        {
//            if (dgvTasks.SelectedRows.Count == 0) return;
            
//            int taskId = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
            
//            // Simple input dialog
//            using (Form priorityForm = new Form())
//            {
//                priorityForm.Text = "Change Priority";
//                priorityForm.Size = new Size(320, 180);
//                priorityForm.StartPosition = FormStartPosition.CenterParent;
//                priorityForm.FormBorderStyle = FormBorderStyle.FixedDialog;
//                priorityForm.MaximizeBox = false;
//                priorityForm.MinimizeBox = false;
                
//                Label lbl = new Label
//                {
//                    Text = "Select new priority:",
//                    Location = new Point(20, 20),
//                    Size = new Size(260, 20)
//                };
                
//                ComboBox cmb = new ComboBox
//                {
//                    Location = new Point(20, 50),
//                    Size = new Size(260, 25),
//                    DropDownStyle = ComboBoxStyle.DropDownList
//                };
//                cmb.Items.AddRange(new object[] { 
//                    "🟢 Low Priority", 
//                    "🟠 Medium Priority", 
//                    "🔴 High Priority", 
//                    "🔥 Critical Priority" 
//                });
//                cmb.SelectedIndex = 1;
                
//                Button btnOk = new Button
//                {
//                    Text = "✓ OK",
//                    Location = new Point(120, 100),
//                    Size = new Size(80, 30),
//                    DialogResult = DialogResult.OK
//                };
                
//                Button btnCancel = new Button
//                {
//                    Text = "✗ Cancel",
//                    Location = new Point(210, 100),
//                    Size = new Size(80, 30),
//                    DialogResult = DialogResult.Cancel
//                };
                
//                priorityForm.Controls.Add(lbl);
//                priorityForm.Controls.Add(cmb);
//                priorityForm.Controls.Add(btnOk);
//                priorityForm.Controls.Add(btnCancel);
//                priorityForm.AcceptButton = btnOk;
//                priorityForm.CancelButton = btnCancel;
                
//                if (priorityForm.ShowDialog() == DialogResult.OK && cmb.SelectedIndex != -1)
//                {
//                    int newPriority = cmb.SelectedIndex + 1;
                    
//                    if (TaskHelper.UpdateTaskPriority(taskId, newPriority))
//                    {
//                        MessageBox.Show("✅ Priority updated!", "Success", 
//                            MessageBoxButtons.OK, MessageBoxIcon.Information);
//                        LoadTasks();
//                    }
//                }
//            }
//        }
//    }
//}


//// ═══════════════════════════════════════════════════════════════════════════════
//// DESIGNER CODE EXAMPLE - InitializeComponent() mein yeh controls add karein
//// ═══════════════════════════════════════════════════════════════════════════════

///*
//private void InitializeComponent()
//{
//    this.grpAddTask = new GroupBox();
//    this.txtTask = new TextBox();
//    this.cmbPriority = new ComboBox();
//    this.chkDueDate = new CheckBox();
//    this.dtpDueDate = new DateTimePicker();
//    this.btnAddTask = new Button();
//    this.btnClear = new Button();
//    this.dgvTasks = new DataGridView();
//    this.lblTaskCount = new Label();
//    this.lblPendingCount = new Label();
//    this.lblCriticalCount = new Label();
    
//    // Form
//    this.Text = "📝 To-Do List Manager";
//    this.Size = new Size(900, 700);
//    this.StartPosition = FormStartPosition.CenterScreen;
    
//    // GroupBox
//    this.grpAddTask.Text = "➕ Add New Task";
//    this.grpAddTask.Location = new Point(20, 20);
//    this.grpAddTask.Size = new Size(850, 150);
    
//    // Task TextBox
//    Label lblTask = new Label { Text = "Task:", Location = new Point(20, 30), AutoSize = true };
//    this.txtTask.Location = new Point(20, 50);
//    this.txtTask.Size = new Size(400, 25);
    
//    // Priority ComboBox
//    Label lblPriority = new Label { Text = "Priority:", Location = new Point(440, 30), AutoSize = true };
//    this.cmbPriority.Location = new Point(440, 50);
//    this.cmbPriority.Size = new Size(180, 25);
    
//    // Due Date
//    this.chkDueDate.Text = "Due Date:";
//    this.chkDueDate.Location = new Point(640, 50);
//    this.chkDueDate.Size = new Size(90, 25);
    
//    this.dtpDueDate.Location = new Point(640, 80);
//    this.dtpDueDate.Size = new Size(180, 25);
//    this.dtpDueDate.Enabled = false;
    
//    // Buttons
//    this.btnAddTask.Text = "💾 Add Task";
//    this.btnAddTask.Location = new Point(20, 100);
//    this.btnAddTask.Size = new Size(120, 35);
    
//    this.btnClear.Text = "🔄 Clear";
//    this.btnClear.Location = new Point(150, 100);
//    this.btnClear.Size = new Size(100, 35);
    
//    // DataGridView
//    this.dgvTasks.Location = new Point(20, 230);
//    this.dgvTasks.Size = new Size(850, 380);
    
//    // Statistics
//    this.lblTaskCount.Location = new Point(20, 190);
//    this.lblTaskCount.AutoSize = true;
//    this.lblPendingCount.Location = new Point(200, 190);
//    this.lblPendingCount.AutoSize = true;
//    this.lblCriticalCount.Location = new Point(400, 190);
//    this.lblCriticalCount.AutoSize = true;
    
//    // Add to GroupBox
//    this.grpAddTask.Controls.Add(lblTask);
//    this.grpAddTask.Controls.Add(this.txtTask);
//    this.grpAddTask.Controls.Add(lblPriority);
//    this.grpAddTask.Controls.Add(this.cmbPriority);
//    this.grpAddTask.Controls.Add(this.chkDueDate);
//    this.grpAddTask.Controls.Add(this.dtpDueDate);
//    this.grpAddTask.Controls.Add(this.btnAddTask);
//    this.grpAddTask.Controls.Add(this.btnClear);
    
//    // Add to Form
//    this.Controls.Add(this.grpAddTask);
//    this.Controls.Add(this.dgvTasks);
//    this.Controls.Add(this.lblTaskCount);
//    this.Controls.Add(this.lblPendingCount);
//    this.Controls.Add(this.lblCriticalCount);
//}
//*/
