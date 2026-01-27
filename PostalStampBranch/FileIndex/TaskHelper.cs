using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FileIndex
{
    public static class TaskHelper
    {
        public static void LoadTasksToGrid(DataGridView dgv, string userName)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // Humne IsCompleted ko bhi mangwaya hai
                string query = "SELECT TaskID, TaskDescription, IsCompleted FROM ToDoList WHERE UserName = @user";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@user", userName);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;

                // Faltu columns hide kardein
                if (dgv.Columns["TaskID"] != null) dgv.Columns["TaskID"].Visible = false;
                if (dgv.Columns["IsCompleted"] != null) dgv.Columns["IsCompleted"].Visible = false;

                // Width set karein
                dgv.Columns["TaskDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // 2. Status Update Karna (Complete/Uncomplete)
        public static void UpdateTaskStatus(int taskId, bool status)
        {
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = "UPDATE ToDoList SET IsCompleted = @s WHERE TaskID = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@s", status ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", taskId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // 2. Naya Task Save karne ka function
        public static void AddNewTask(string task, string userName)
        {
            if (string.IsNullOrWhiteSpace(task)) return;

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = "INSERT INTO ToDoList (UserName, TaskDescription) VALUES (@user, @task)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user", userName);
                cmd.Parameters.AddWithValue("@task", task);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void MarkAsDone(int taskId, bool status)
        {
          
            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // Yahan 'IsCompleted' wahi naam hai jo aapne table mein rakha tha
                string query = "UPDATE ToDoList SET IsCompleted = @status WHERE TaskID = @id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@status", status ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", taskId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        
    }

        public static void DeleteTask(int taskId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    // TaskID ke zariye specific record delete karna
                    string query = "DELETE FROM ToDoList WHERE TaskID = @id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", taskId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Task delete karne mein masla hua: " + ex.Message);
            }
        }
    }
}