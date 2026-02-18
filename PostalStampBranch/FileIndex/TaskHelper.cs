using System;
using System.Data;
using System.Drawing;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace FileIndex
{
    /// <summary>
    /// Task Management Helper with Priority Support
    /// Priority Levels: 1=Low (Green), 2=Medium (Orange), 3=High (Red), 4=Critical (Dark Red)
    /// </summary>
    public static class TaskHelper
    {
        #region Priority Colors & Constants

        /// <summary>
        /// Priority levels enum
        /// </summary>
        public enum TaskPriority
        {
            Low = 1,      // 🟢 Green
            Medium = 2,   // 🟠 Orange
            High = 3,     // 🔴 Red
            Critical = 4  // 🔥 Dark Red
        }

        // Color scheme for priorities
        public static readonly Color LowPriorityColor = Color.FromArgb(39, 174, 96);      // Green
        public static readonly Color MediumPriorityColor = Color.FromArgb(243, 156, 18);  // Orange
        public static readonly Color HighPriorityColor = Color.FromArgb(231, 76, 60);     // Red
        public static readonly Color CriticalPriorityColor = Color.FromArgb(192, 57, 43); // Dark Red
        public static readonly Color CompletedColor = Color.FromArgb(149, 165, 166);      // Gray
        public static readonly Color TextWhite = Color.White;
        public static readonly Color TextDark = Color.FromArgb(44, 62, 80);

        #endregion

        #region Load Tasks to DataGridView

        /// <summary>
        /// Tasks ko DataGridView mein load karta hai with priority colors
        /// </summary>
        public static void LoadTasksToGrid(DataGridView dgv, string userName, bool showCompleted = true)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    // Query with priority
                    string query = @"SELECT 
                        TaskID, 
                        TaskDescription, 
                        Priority,
                        IsCompleted,
                        CreatedDate,
                        DueDate
                    FROM ToDoList 
                    WHERE UserName = @user";

                    // Agar completed tasks nahi dikhane to filter lagao
                    if (!showCompleted)
                    {
                        query += " AND IsCompleted = 0";
                    }

                    // Sort by: Incomplete first, then by Priority (High to Low), then by DueDate
                    query += " ORDER BY IsCompleted ASC, Priority DESC, DueDate ASC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@user", userName);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // ✅ Add Priority Display Column (Text format)
                    dt.Columns.Add("PriorityText", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        int priority = row["Priority"] != DBNull.Value ? Convert.ToInt32(row["Priority"]) : 2;
                        row["PriorityText"] = GetPriorityText(priority);
                    }

                    dgv.DataSource = dt;

                    // ═══════════════════════════════════════════════════════
                    // Column Configuration
                    // ═══════════════════════════════════════════════════════

                    // Hide TaskID
                    if (dgv.Columns["TaskID"] != null)
                        dgv.Columns["TaskID"].Visible = false;

                    // Hide IsCompleted (we'll show via checkbox or strikethrough)
                    if (dgv.Columns["IsCompleted"] != null)
                        dgv.Columns["IsCompleted"].Visible = false;

                    // Hide numeric Priority column (original)
                    if (dgv.Columns["Priority"] != null)
                    {
                        dgv.Columns["Priority"].Visible = false;
                    }

                    // Show PriorityText column (new display column)
                    if (dgv.Columns["PriorityText"] != null)
                    {
                        dgv.Columns["PriorityText"].HeaderText = "Priority";
                        dgv.Columns["PriorityText"].Width = 100;
                        dgv.Columns["PriorityText"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgv.Columns["PriorityText"].DisplayIndex = 1; // Second column after Task
                    }

                    // Task Description
                    if (dgv.Columns["TaskDescription"] != null)
                    {
                        dgv.Columns["TaskDescription"].HeaderText = "Task";
                        dgv.Columns["TaskDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgv.Columns["TaskDescription"].DisplayIndex = 0; // First column
                    }

                    // Created Date
                    if (dgv.Columns["CreatedDate"] != null)
                    {
                        dgv.Columns["CreatedDate"].HeaderText = "Created";
                        dgv.Columns["CreatedDate"].Width = 100;
                        dgv.Columns["CreatedDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                        dgv.Columns["CreatedDate"].DisplayIndex = 2;
                    }

                    // Due Date
                    if (dgv.Columns["DueDate"] != null)
                    {
                        dgv.Columns["DueDate"].HeaderText = "Due Date";
                        dgv.Columns["DueDate"].Width = 100;
                        dgv.Columns["DueDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                        dgv.Columns["DueDate"].DisplayIndex = 3;
                    }

                    // Apply colors after data is loaded
                    ApplyPriorityColors(dgv);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error loading tasks:\n{ex.Message}",
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Apply Priority Colors

        /// <summary>
        /// DataGridView rows ko priority ke mutabiq color karta hai
        /// </summary>
        public static void ApplyPriorityColors(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                // Get priority and completed status
                int priority = row.Cells["Priority"].Value != DBNull.Value
                    ? Convert.ToInt32(row.Cells["Priority"].Value)
                    : 2; // Default Medium

                bool isCompleted = row.Cells["IsCompleted"].Value != DBNull.Value
                    && Convert.ToBoolean(row.Cells["IsCompleted"].Value);

                Color backColor;
                Color foreColor = TextWhite;

                // Agar completed hai to gray color
                if (isCompleted)
                {
                    backColor = CompletedColor;
                    foreColor = TextWhite;

                    // Strikethrough effect for completed tasks
                    row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Strikeout);
                }
                else
                {
                    // Priority ke mutabiq color
                    switch (priority)
                    {
                        case 1: // Low
                            backColor = LowPriorityColor;
                            break;
                        case 2: // Medium
                            backColor = MediumPriorityColor;
                            break;
                        case 3: // High
                            backColor = HighPriorityColor;
                            break;
                        case 4: // Critical
                            backColor = CriticalPriorityColor;
                            break;
                        default:
                            backColor = MediumPriorityColor;
                            break;
                    }

                    row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Regular);
                }

                // Apply colors
                row.DefaultCellStyle.BackColor = backColor;
                row.DefaultCellStyle.ForeColor = foreColor;
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(
                    Math.Max(0, backColor.R - 30),
                    Math.Max(0, backColor.G - 30),
                    Math.Max(0, backColor.B - 30)
                );
                row.DefaultCellStyle.SelectionForeColor = foreColor;
            }
        }

        /// <summary>
        /// Priority number ko text mein convert karta hai
        /// </summary>
        private static string GetPriorityText(int priority)
        {
            switch (priority)
            {
                case 1: return "🟢 Low";
                case 2: return "🟠 Medium";
                case 3: return "🔴 High";
                case 4: return "🔥 Critical";
                default: return "🟠 Medium";
            }
        }

        #endregion

        #region Add New Task with Priority

        /// <summary>
        /// Naya task add karta hai with priority
        /// </summary>
        public static bool AddNewTask(string task, string userName, int priority = 2, DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(task))
            {
                MessageBox.Show("⚠️ Task description cannot be empty!",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Priority validation (1-4)
            if (priority < 1 || priority > 4)
            {
                priority = 2; // Default to Medium
            }

            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = @"INSERT INTO ToDoList 
                        (UserName, TaskDescription, Priority, CreatedDate, DueDate, IsCompleted) 
                        VALUES (@user, @task, @priority, @created, @due, 0)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", userName);
                    cmd.Parameters.AddWithValue("@task", task.Trim());
                    cmd.Parameters.AddWithValue("@priority", priority);
                    cmd.Parameters.AddWithValue("@created", DateTime.Now);
                    cmd.Parameters.AddWithValue("@due", dueDate.HasValue ? (object)dueDate.Value : DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error adding task:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Update Task Status

        /// <summary>
        /// Task status ko update karta hai (Complete/Incomplete)
        /// </summary>
        public static bool UpdateTaskStatus(int taskId, bool isCompleted)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "UPDATE ToDoList SET IsCompleted = @status WHERE TaskID = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@status", isCompleted ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", taskId);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error updating task status:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Alias for UpdateTaskStatus (backward compatibility)
        /// </summary>
        public static void MarkAsDone(int taskId, bool status)
        {
            UpdateTaskStatus(taskId, status);
        }

        #endregion

        #region Update Task Priority

        /// <summary>
        /// Task ki priority update karta hai
        /// </summary>
        public static bool UpdateTaskPriority(int taskId, int newPriority)
        {
            // Priority validation
            if (newPriority < 1 || newPriority > 4)
            {
                MessageBox.Show("⚠️ Priority must be between 1 and 4!",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "UPDATE ToDoList SET Priority = @priority WHERE TaskID = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@priority", newPriority);
                    cmd.Parameters.AddWithValue("@id", taskId);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error updating priority:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Delete Task

        /// <summary>
        /// Task ko delete karta hai
        /// </summary>
        public static bool DeleteTask(int taskId)
        {
            try
            {
                // Confirmation dialog
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this task?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return false;

                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = "DELETE FROM ToDoList WHERE TaskID = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", taskId);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("✅ Task deleted successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("⚠️ Task not found!",
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error deleting task:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Get Task Statistics

        /// <summary>
        /// User ke tasks ki statistics return karta hai
        /// </summary>
        public static TaskStats GetTaskStatistics(string userName)
        {
            TaskStats stats = new TaskStats();

            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    string query = @"
                        SELECT 
                            COUNT(*) as Total,
                            SUM(CASE WHEN IsCompleted = 1 THEN 1 ELSE 0 END) as Completed,
                            SUM(CASE WHEN IsCompleted = 0 THEN 1 ELSE 0 END) as Pending,
                            SUM(CASE WHEN IsCompleted = 0 AND Priority = 4 THEN 1 ELSE 0 END) as CriticalPending,
                            SUM(CASE WHEN IsCompleted = 0 AND Priority = 3 THEN 1 ELSE 0 END) as HighPending,
                            SUM(CASE WHEN IsCompleted = 0 AND DueDate < GETDATE() THEN 1 ELSE 0 END) as Overdue
                        FROM ToDoList 
                        WHERE UserName = @user";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", userName);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        stats.TotalTasks = reader["Total"] != DBNull.Value ? Convert.ToInt32(reader["Total"]) : 0;
                        stats.CompletedTasks = reader["Completed"] != DBNull.Value ? Convert.ToInt32(reader["Completed"]) : 0;
                        stats.PendingTasks = reader["Pending"] != DBNull.Value ? Convert.ToInt32(reader["Pending"]) : 0;
                        stats.CriticalPending = reader["CriticalPending"] != DBNull.Value ? Convert.ToInt32(reader["CriticalPending"]) : 0;
                        stats.HighPending = reader["HighPending"] != DBNull.Value ? Convert.ToInt32(reader["HighPending"]) : 0;
                        stats.OverdueTasks = reader["Overdue"] != DBNull.Value ? Convert.ToInt32(reader["Overdue"]) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error getting statistics:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return stats;
        }

        #endregion
    }

    #region Task Statistics Class

    /// <summary>
    /// Task statistics container
    /// </summary>
    public class TaskStats
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CriticalPending { get; set; }
        public int HighPending { get; set; }
        public int OverdueTasks { get; set; }

        public double CompletionPercentage
        {
            get
            {
                if (TotalTasks == 0) return 0;
                return (double)CompletedTasks / TotalTasks * 100;
            }
        }
    }

    #endregion
}