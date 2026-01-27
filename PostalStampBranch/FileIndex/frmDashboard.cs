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

namespace FileIndex
{
    public partial class frmDashboard : Form
    {
        private void FillCompletedTasksCombo()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    // Query: Sirf wahi tasks jo mukammal (IsCompleted = 1) hain
                    string query = @"SELECT TaskID, TaskDescription 
                             FROM ToDoList 
                             WHERE IsCompleted = 1 
                             ORDER BY TaskID ASC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // ComboBox ko data dena
                    cmb_Task.DataSource = dt;
                    cmb_Task.DisplayMember = "TaskDescription";
                    cmb_Task.ValueMember = "TaskID";

                    // Shuru mein ComboBox khali dikhane ke liye
                    cmb_Task.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Combo box bharne mein masla hua: " + ex.Message);
            }
        }
        public frmDashboard()
        {
            InitializeComponent();
        }
        private void frmDashboard_Load(object sender, EventArgs e)
        {
            UIHelper.ApplyTheme(this);
            UIHelper.SetFormTheme(this);

            RefreshData();
            FillCompletedTasksCombo();

            dgvTasks.Columns["TaskDescription"].Width = 300;

            // 2. Column ko screen ki bachi hui saari jagah de dena (Fill mode)
            // Is se column automatic bada ho jaye ga aur grid khali nazar nahi aaye gi
            dgvTasks.Columns["TaskDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // 3. Column ki width utni rakhen jitna us mein likha hua text hai
            dgvTasks.Columns["TaskID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void ApplyFormatting()
        {
            foreach (DataGridViewRow row in dgvTasks.Rows)
            {
                if (row.Cells["IsCompleted"].Value != DBNull.Value)
                {
                    bool done = Convert.ToBoolean(row.Cells["IsCompleted"].Value);
                    if (done)
                    {
                        row.DefaultCellStyle.Font = new Font(dgvTasks.Font, FontStyle.Strikeout);
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        row.DefaultCellStyle.Font = new Font(dgvTasks.Font, FontStyle.Regular);
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }








        // Data refresh karne ka chota sa function
        private void RefreshData()
        {
            // TaskHelper class ka function call kiya
            TaskHelper.LoadTasksToGrid(dgvTasks, GlobalData.CurrentUser);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTaskDescription.Text))
            {
                // TaskHelper class ka function call kiya
                TaskHelper.AddNewTask(txtTaskDescription.Text.Trim(), GlobalData.CurrentUser);

                // Textbox saaf karein aur grid update karein
                txtTaskDescription.Clear();
                RefreshData();

                MessageBox.Show("Task save ho gaya!");
            }
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells["TaskID"].Value);
                bool currentStatus = Convert.ToBoolean(dgvTasks.Rows[e.RowIndex].Cells["IsCompleted"].Value);

                // Status ulat dein (agar 0 hai to 1 kar dein)
                TaskHelper.UpdateTaskStatus(id, !currentStatus);

                // Refresh karein
                TaskHelper.LoadTasksToGrid(dgvTasks, GlobalData.CurrentUser);
                ApplyFormatting();
                FillCompletedTasksCombo();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmb_Task.SelectedIndex != -1)
            {
                int taskId = Convert.ToInt32(cmb_Task.SelectedValue);
                // Task ko delete kar dein
                TaskHelper.DeleteTask(taskId);
                // Refresh karein
                RefreshData();
                FillCompletedTasksCombo();
                MessageBox.Show("Task delete ho gaya!");
            }
            else
            {
                MessageBox.Show("Koi task select nahi hua.");
            }
        }
    }
}
