using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupplyBranch.Helpers;
using System.Windows.Forms;

namespace SupplyBranch.Helpers
{
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

        //DBHelper db = new DBHelper();
        
        #region Load Tasks to DataGridView

        /// <summary>
        /// Tasks ko DataGridView mein load karta hai with priority colors
        /// </summary>
        public static void LoadTasksToGrid(
    DataGridView dgv,
    string userName,
    bool showCompleted = true)
        {
            try
            {
                string query = @"
            SELECT
                TaskID,
                TaskDescription,
                Priority,
                IsCompleted,
                CreatedDate,
                DueDate
            FROM ToDoList
            WHERE UserName = @UserName";

                if (!showCompleted)
                {
                    query += " AND IsCompleted = 0";
                }

                query += @"
            ORDER BY
                IsCompleted ASC,
                Priority DESC,
                DueDate ASC";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserName", userName)
        };

                // Use current project's DBHelper
                DBHelper db = new DBHelper();

                DataTable dt =
                    db.ExecuteQuery(query, parameters);

                // ==========================================
                // Priority Display Text
                // ==========================================

                dt.Columns.Add("PriorityText", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    int priority =
                        row["Priority"] != DBNull.Value
                            ? Convert.ToInt32(row["Priority"])
                            : 2;

                    row["PriorityText"] =
                        GetPriorityText(priority);
                }

                // ==========================================
                // Bind Grid
                // ==========================================

                dgv.DataSource = dt;

                // ==========================================
                // Hide Technical Columns
                // ==========================================

                if (dgv.Columns["TaskID"] != null)
                    dgv.Columns["TaskID"].Visible = false;

                if (dgv.Columns["IsCompleted"] != null)
                    dgv.Columns["IsCompleted"].Visible = false;

                if (dgv.Columns["Priority"] != null)
                    dgv.Columns["Priority"].Visible = false;

                // ==========================================
                // Task Description
                // ==========================================

                if (dgv.Columns["TaskDescription"] != null)
                {
                    dgv.Columns["TaskDescription"].HeaderText = "Task";
                    dgv.Columns["TaskDescription"].AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;

                    dgv.Columns["TaskDescription"].DisplayIndex = 0;
                }

                // ==========================================
                // Priority
                // ==========================================

                if (dgv.Columns["PriorityText"] != null)
                {
                    dgv.Columns["PriorityText"].HeaderText = "Priority";
                    dgv.Columns["PriorityText"].Width = 100;

                    dgv.Columns["PriorityText"]
                        .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                    dgv.Columns["PriorityText"].DisplayIndex = 1;
                }

                // ==========================================
                // Created Date
                // ==========================================

                if (dgv.Columns["CreatedDate"] != null)
                {
                    dgv.Columns["CreatedDate"].HeaderText = "Created";
                    dgv.Columns["CreatedDate"].Width = 100;

                    dgv.Columns["CreatedDate"]
                        .DefaultCellStyle.Format = "dd-MMM-yyyy";

                    dgv.Columns["CreatedDate"].DisplayIndex = 2;
                }

                // ==========================================
                // Due Date
                // ==========================================

                if (dgv.Columns["DueDate"] != null)
                {
                    dgv.Columns["DueDate"].HeaderText = "Due Date";
                    dgv.Columns["DueDate"].Width = 100;

                    dgv.Columns["DueDate"]
                        .DefaultCellStyle.Format = "dd-MMM-yyyy";

                    dgv.Columns["DueDate"].DisplayIndex = 3;
                }

                // ==========================================
                // Priority Colors
                // ==========================================

                ApplyPriorityColors(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading tasks:\n\n" + ex.Message,
                    "Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
        public static bool AddNewTask(
     string task,
     string userName,
     int priority = 2,
     DateTime? dueDate = null)
        {
            // ==========================================
            // Validate Task
            // ==========================================

            if (string.IsNullOrWhiteSpace(task))
            {
                MessageBox.Show(
                    "Task description cannot be empty!",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            // ==========================================
            // Validate Priority
            // ==========================================

            if (priority < 1 || priority > 4)
            {
                priority = 2; // Medium
            }

            try
            {
                string query = @"
            INSERT INTO ToDoList
            (
                UserName,
                TaskDescription,
                Priority,
                CreatedDate,
                DueDate,
                IsCompleted
            )
            VALUES
            (
                @UserName,
                @TaskDescription,
                @Priority,
                @CreatedDate,
                @DueDate,
                0
            )";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserName", userName),

            new SqlParameter(
                "@TaskDescription",
                task.Trim()),

            new SqlParameter(
                "@Priority",
                priority),

            new SqlParameter(
                "@CreatedDate",
                DateTime.Now),

            new SqlParameter(
                "@DueDate",
                dueDate.HasValue
                    ? (object)dueDate.Value
                    : DBNull.Value)
        };

                // Current SupplyBranch DBHelper
                DBHelper db = new DBHelper();

                db.ExecuteNonQuery(query, parameters);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error adding task:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
                string query = @"
            UPDATE ToDoList
            SET IsCompleted = @Status
            WHERE TaskID = @TaskID";

                SqlParameter[] parameters =
                {
            new SqlParameter("@Status", isCompleted),
            new SqlParameter("@TaskID", taskId)
        };

                DBHelper db = new DBHelper();

                db.ExecuteNonQuery(query, parameters);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating task status:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
                MessageBox.Show(
                    "Priority must be between 1 and 4!",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            try
            {
                string query = @"
            UPDATE ToDoList
            SET Priority = @Priority
            WHERE TaskID = @TaskID";

                SqlParameter[] parameters =
                {
            new SqlParameter("@Priority", newPriority),
            new SqlParameter("@TaskID", taskId)
        };

                DBHelper db = new DBHelper();

                db.ExecuteNonQuery(query, parameters);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating priority:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
                // ==========================================
                // Confirmation
                // ==========================================

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this task?\n\n" +
                    "This action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return false;

                // ==========================================
                // Delete Task
                // ==========================================

                string query = @"
            DELETE FROM ToDoList
            WHERE TaskID = @TaskID";

                SqlParameter[] parameters =
                {
            new SqlParameter("@TaskID", taskId)
        };

                DBHelper db = new DBHelper();

                int rowsAffected =
                    db.ExecuteNonQuery(query, parameters);

                // ==========================================
                // Result
                // ==========================================

                if (rowsAffected > 0)
                {
                    MessageBox.Show(
                        "Task deleted successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;
                }
                else
                {
                    MessageBox.Show(
                        "Task not found!",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error deleting task:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
                string query = @"
            SELECT 
                COUNT(*) AS Total,

                SUM(CASE 
                    WHEN IsCompleted = 1 
                    THEN 1 ELSE 0 
                END) AS Completed,

                SUM(CASE 
                    WHEN IsCompleted = 0 
                    THEN 1 ELSE 0 
                END) AS Pending,

                SUM(CASE 
                    WHEN IsCompleted = 0 
                         AND Priority = 4 
                    THEN 1 ELSE 0 
                END) AS CriticalPending,

                SUM(CASE 
                    WHEN IsCompleted = 0 
                         AND Priority = 3 
                    THEN 1 ELSE 0 
                END) AS HighPending,

                SUM(CASE 
                    WHEN IsCompleted = 0 
                         AND DueDate < GETDATE() 
                    THEN 1 ELSE 0 
                END) AS Overdue

            FROM ToDoList
            WHERE UserName = @UserName";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserName", userName)
        };

                DBHelper db = new DBHelper();

                DataTable dt =
                    db.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    stats.TotalTasks =
                        row["Total"] != DBNull.Value
                            ? Convert.ToInt32(row["Total"])
                            : 0;

                    stats.CompletedTasks =
                        row["Completed"] != DBNull.Value
                            ? Convert.ToInt32(row["Completed"])
                            : 0;

                    stats.PendingTasks =
                        row["Pending"] != DBNull.Value
                            ? Convert.ToInt32(row["Pending"])
                            : 0;

                    stats.CriticalPending =
                        row["CriticalPending"] != DBNull.Value
                            ? Convert.ToInt32(row["CriticalPending"])
                            : 0;

                    stats.HighPending =
                        row["HighPending"] != DBNull.Value
                            ? Convert.ToInt32(row["HighPending"])
                            : 0;

                    stats.OverdueTasks =
                        row["Overdue"] != DBNull.Value
                            ? Convert.ToInt32(row["Overdue"])
                            : 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error getting task statistics:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
