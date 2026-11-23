using System;
using System.Drawing;
using System.Windows.Forms;
using SupplyBranch.Helpers;

namespace SupplyBranch.Forms
{
    /// <summary>
    /// Add/Edit Task Form with Priority Selection
    /// </summary>
    public partial class AddTaskForm : Form
    {
        #region Form Controls

        private TextBox txtTask;
        private ComboBox cmbPriority;
        private DateTimePicker dtpDueDate;
        private CheckBox chkDueDate;
        private TextBox txtRemarks;
        private Button btnSave;
        private Button btnCancel;
        private Label lblTask;
        private Label lblPriority;
        private Label lblDueDate;
        private Label lblRemarks;
        private Panel pnlPriorityPreview;
        private Label lblPriorityPreview;

        #endregion

        #region Properties

        public string UserName { get; set; }
        public string TaskDescription => txtTask.Text.Trim();
        public int SelectedPriority => cmbPriority.SelectedIndex + 1; // 1-4
        public DateTime? DueDate => chkDueDate.Checked ? dtpDueDate.Value.Date : (DateTime?)null;
        public string Remarks => txtRemarks.Text.Trim();

        #endregion

        #region Constructor

        public AddTaskForm(string userName)
        {
            UserName = userName;
            InitializeComponent();
            InitializeCustomComponents();
        }

        #endregion

        #region Initialize Components

        private void InitializeComponent()
        {
            this.txtTask = new TextBox();
            this.cmbPriority = new ComboBox();
            this.dtpDueDate = new DateTimePicker();
            this.chkDueDate = new CheckBox();
            this.txtRemarks = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.lblTask = new Label();
            this.lblPriority = new Label();
            this.lblDueDate = new Label();
            this.lblRemarks = new Label();
            this.pnlPriorityPreview = new Panel();
            this.lblPriorityPreview = new Label();
            
            this.SuspendLayout();
            
            // ═══════════════════════════════════════════════════════
            // Form Settings
            // ═══════════════════════════════════════════════════════
            this.ClientSize = new Size(500, 450);
            this.Text = "📝 Add New Task";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 9.75F);
            
            // ═══════════════════════════════════════════════════════
            // Task Description Label
            // ═══════════════════════════════════════════════════════
            this.lblTask.AutoSize = true;
            this.lblTask.Location = new Point(20, 20);
            this.lblTask.Text = "Task Description: *";
            this.lblTask.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            
            // ═══════════════════════════════════════════════════════
            // Task TextBox
            // ═══════════════════════════════════════════════════════
            this.txtTask.Location = new Point(20, 45);
            this.txtTask.Size = new Size(450, 25);
            this.txtTask.Multiline = true;
            this.txtTask.Height = 80;
            this.txtTask.ScrollBars = ScrollBars.Vertical;
            this.txtTask.MaxLength = 500;
            
            // ═══════════════════════════════════════════════════════
            // Priority Label
            // ═══════════════════════════════════════════════════════
            this.lblPriority.AutoSize = true;
            this.lblPriority.Location = new Point(20, 140);
            this.lblPriority.Text = "Priority: *";
            this.lblPriority.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            
            // ═══════════════════════════════════════════════════════
            // Priority ComboBox
            // ═══════════════════════════════════════════════════════
            this.cmbPriority.Location = new Point(20, 165);
            this.cmbPriority.Size = new Size(200, 25);
            this.cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPriority.Items.AddRange(new object[] {
                "🟢 Low Priority",
                "🟠 Medium Priority",
                "🔴 High Priority",
                "🔥 Critical Priority"
            });
            this.cmbPriority.SelectedIndex = 1; // Default: Medium
            this.cmbPriority.SelectedIndexChanged += cmbPriority_SelectedIndexChanged;
            
            // ═══════════════════════════════════════════════════════
            // Priority Preview Panel
            // ═══════════════════════════════════════════════════════
            this.pnlPriorityPreview.Location = new Point(240, 165);
            this.pnlPriorityPreview.Size = new Size(230, 50);
            this.pnlPriorityPreview.BorderStyle = BorderStyle.FixedSingle;
            this.pnlPriorityPreview.BackColor = TaskHelper.MediumPriorityColor;
            
            this.lblPriorityPreview.Dock = DockStyle.Fill;
            this.lblPriorityPreview.Text = "MEDIUM PRIORITY";
            this.lblPriorityPreview.TextAlign = ContentAlignment.MiddleCenter;
            this.lblPriorityPreview.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblPriorityPreview.ForeColor = Color.White;
            this.pnlPriorityPreview.Controls.Add(this.lblPriorityPreview);
            
            // ═══════════════════════════════════════════════════════
            // Due Date CheckBox
            // ═══════════════════════════════════════════════════════
            this.chkDueDate.Location = new Point(20, 230);
            this.chkDueDate.Size = new Size(150, 25);
            this.chkDueDate.Text = "📅 Set Due Date";
            this.chkDueDate.Font = new Font("Segoe UI", 10F);
            this.chkDueDate.CheckedChanged += chkDueDate_CheckedChanged;
            
            // ═══════════════════════════════════════════════════════
            // Due Date Picker
            // ═══════════════════════════════════════════════════════
            this.dtpDueDate.Location = new Point(180, 230);
            this.dtpDueDate.Size = new Size(290, 25);
            this.dtpDueDate.Format = DateTimePickerFormat.Long;
            this.dtpDueDate.Value = DateTime.Now.AddDays(7);
            this.dtpDueDate.Enabled = false;
            
            // ═══════════════════════════════════════════════════════
            // Remarks Label
            // ═══════════════════════════════════════════════════════
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new Point(20, 270);
            this.lblRemarks.Text = "Remarks (Optional):";
            this.lblRemarks.Font = new Font("Segoe UI", 10F);
            
            // ═══════════════════════════════════════════════════════
            // Remarks TextBox
            // ═══════════════════════════════════════════════════════
            this.txtRemarks.Location = new Point(20, 295);
            this.txtRemarks.Size = new Size(450, 25);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Height = 60;
            this.txtRemarks.ScrollBars = ScrollBars.Vertical;
            this.txtRemarks.MaxLength = 500;
            
            // ═══════════════════════════════════════════════════════
            // Save Button
            // ═══════════════════════════════════════════════════════
            this.btnSave.Location = new Point(280, 380);
            this.btnSave.Size = new Size(90, 35);
            this.btnSave.Text = "💾 Save";
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.BackColor = Color.FromArgb(39, 174, 96);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.Click += btnSave_Click;
            
            // ═══════════════════════════════════════════════════════
            // Cancel Button
            // ═══════════════════════════════════════════════════════
            this.btnCancel.Location = new Point(380, 380);
            this.btnCancel.Size = new Size(90, 35);
            this.btnCancel.Text = "❌ Cancel";
            this.btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancel.BackColor = Color.FromArgb(189, 195, 199);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Cursor = Cursors.Hand;
            this.btnCancel.Click += btnCancel_Click;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            
            // ═══════════════════════════════════════════════════════
            // Add Controls to Form
            // ═══════════════════════════════════════════════════════
            this.Controls.Add(this.lblTask);
            this.Controls.Add(this.txtTask);
            this.Controls.Add(this.lblPriority);
            this.Controls.Add(this.cmbPriority);
            this.Controls.Add(this.pnlPriorityPreview);
            this.Controls.Add(this.chkDueDate);
            this.Controls.Add(this.dtpDueDate);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void InitializeCustomComponents()
        {
            // Apply theme (if you're using ThemeManager)
            // ThemeManager.ApplyTheme(this);
            
            // Set focus
            this.Load += (s, e) => txtTask.Focus();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Priority selection change par preview update karta hai
        /// </summary>
        private void cmbPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbPriority.SelectedIndex)
            {
                case 0: // Low
                    pnlPriorityPreview.BackColor = TaskHelper.LowPriorityColor;
                    lblPriorityPreview.Text = "LOW PRIORITY";
                    break;
                case 1: // Medium
                    pnlPriorityPreview.BackColor = TaskHelper.MediumPriorityColor;
                    lblPriorityPreview.Text = "MEDIUM PRIORITY";
                    break;
                case 2: // High
                    pnlPriorityPreview.BackColor = TaskHelper.HighPriorityColor;
                    lblPriorityPreview.Text = "HIGH PRIORITY";
                    break;
                case 3: // Critical
                    pnlPriorityPreview.BackColor = TaskHelper.CriticalPriorityColor;
                    lblPriorityPreview.Text = "CRITICAL PRIORITY";
                    break;
            }
        }

        /// <summary>
        /// Due Date checkbox toggle
        /// </summary>
        private void chkDueDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpDueDate.Enabled = chkDueDate.Checked;
        }

        /// <summary>
        /// Save button click - Validation aur task add karta hai
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // ═══════════════════════════════════════════════════════
            // Validation
            // ═══════════════════════════════════════════════════════
            
            if (string.IsNullOrWhiteSpace(txtTask.Text))
            {
                MessageBox.Show("⚠️ Please enter task description!", 
                    "Validation Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtTask.Focus();
                return;
            }
            
            if (cmbPriority.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Please select priority!", 
                    "Validation Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                cmbPriority.Focus();
                cmbPriority.DroppedDown = true;
                return;
            }
            
            // Due date validation (optional)
            if (chkDueDate.Checked && dtpDueDate.Value.Date < DateTime.Now.Date)
            {
                DialogResult result = MessageBox.Show(
                    "⚠️ Due date is in the past!\n\nDo you want to continue?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                
                if (result != DialogResult.Yes)
                {
                    dtpDueDate.Focus();
                    return;
                }
            }
            
            // ═══════════════════════════════════════════════════════
            // Save Task
            // ═══════════════════════════════════════════════════════
            
            bool success = TaskHelper.AddNewTask(
                task: TaskDescription,
                userName: UserName,
                priority: SelectedPriority,
                dueDate: DueDate
            );
            
            if (success)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Cancel button click
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}


// ═══════════════════════════════════════════════════════════════════════════════
// USAGE - Main Form Mein Kaise Use Karein
// ═══════════════════════════════════════════════════════════════════════════════

/*
// Main form mein "Add Task" button ka click event:

private void btnAddTask_Click(object sender, EventArgs e)
{
    using (AddTaskForm form = new AddTaskForm("Ali"))  // Current user name
    {
        if (form.ShowDialog() == DialogResult.OK)
        {
            // Task successfully added
            MessageBox.Show("✅ Task added successfully!", 
                "Success", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
            
            // Refresh tasks grid
            TaskHelper.LoadTasksToGrid(dgvTasks, "Ali", showCompleted: true);
            
            // Update statistics (optional)
            ShowTaskStatistics();
        }
    }
}


// Ya phir simple inline approach (bina separate form ke):

private void btnQuickAddTask_Click(object sender, EventArgs e)
{
    // Validation
    if (string.IsNullOrWhiteSpace(txtTaskQuick.Text))
    {
        MessageBox.Show("⚠️ Please enter task description!", 
            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtTaskQuick.Focus();
        return;
    }
    
    if (cmbPriorityQuick.SelectedIndex == -1)
    {
        MessageBox.Show("⚠️ Please select priority!", 
            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cmbPriorityQuick.Focus();
        return;
    }
    
    // Add task
    int priority = cmbPriorityQuick.SelectedIndex + 1;
    DateTime? dueDate = chkDueDateQuick.Checked ? dtpDueDateQuick.Value : (DateTime?)null;
    
    bool success = TaskHelper.AddNewTask(
        txtTaskQuick.Text,
        "Ali",
        priority,
        dueDate
    );
    
    if (success)
    {
        MessageBox.Show("✅ Task added!", "Success", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        // Clear fields
        txtTaskQuick.Clear();
        cmbPriorityQuick.SelectedIndex = 1; // Reset to Medium
        chkDueDateQuick.Checked = false;
        
        // Refresh grid
        TaskHelper.LoadTasksToGrid(dgvTasks, "Ali", true);
    }
}

*/
