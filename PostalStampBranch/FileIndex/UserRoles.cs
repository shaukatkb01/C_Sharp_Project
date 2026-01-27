using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class UserRoles : Form
    {
        public void DeleteUserRole( int userId)
        {
            // 1. Check karein ke kya user select kiya gaya hai?
            if (cmb_User.SelectedValue == null)
            {
                MessageBox.Show("Please Select User first");
                return;
            }
                
             try
            {
                using (SqlConnection con = new SqlConnection(Db.ConString))
                {
                    // Query ko theek kiya: NOT IN (1) ya simply != 1


                    // Sirf Role delete hoga, UserInfo table ko kuch nahi hoga
                    string query = @"DELETE FROM UserRoles WHERE UserID = @id AND RoleID != 1";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", userId);
                    con.Open();
                    
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if ( rowsAffected > 0)
                        MessageBox.Show("Role removed successfully!");
                    else
                        MessageBox.Show("No role was removed (User might be Admin).");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public UserRoles()
        {
            InitializeComponent();
        }

        private void UserRoles_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                UIHelper.ApplyTheme(this);

                UIHelper.SetFormTheme(this);
                LoadUserRolesGrid();
            }

            using var con = new SqlConnection(Db.ConString);
            {
                try
                {
                    string query = @"SELECT UserID,FullName 
                                    FROM UsersInfo
                                    ORDER BY UserID DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cmb_User.DataSource = dt;
                    cmb_User.DisplayMember = "FullName";
                    cmb_User.ValueMember = "UserID";
                    cmb_User.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                try
                {
                    string query1 = @"SELECT RoleID, RoleName FROM Roles";
                    SqlDataAdapter adapter1 = new SqlDataAdapter(query1, con);
                    DataTable dt1 = new DataTable();
                    adapter1.Fill(dt1);
                    cmb_Roles.DataSource = dt1;
                    cmb_Roles.DisplayMember = "RoleName";
                    cmb_Roles.ValueMember = "RoleID";
                    cmb_Roles.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void LoadUserRolesGrid()
        {
            var con = new SqlConnection(Db.ConString);
            {
                try
                {
                    string query = @"SELECT U.FullName, R.RoleName, UR.AssignedAt
                     FROM UserRoles UR
                     JOIN UsersInfo U ON UR.UserID = U.UserID
                     JOIN Roles R ON UR.RoleID = R.RoleID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    DataGridView.DataSource = dt;
                    DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btn_Assign_Click(object sender, EventArgs e)
        {
            int user_ID = Convert.ToInt32(cmb_User.SelectedValue);
            int role_ID = Convert.ToInt32(cmb_Roles.SelectedValue);
            if (user_ID > 0 && role_ID > 0)
            {
                using var con = new SqlConnection(Db.ConString);
                try
                {

                    string query = @"INSERT INTO UserRoles(UserID, RoleID)
                          VALUES(@uID,@rID)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("uID", cmb_User.SelectedValue);
                    cmd.Parameters.AddWithValue("rID", cmb_Roles.SelectedValue);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Role Assigned Successfully!");
                    LoadUserRolesGrid();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MessageBox.Show("Perhaps this role has already been assigned!", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Proble in Database!" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Selecte User and Assigned a role first");
                cmb_User.Focus();
            }
        }

       

        private void btn_delet_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(cmb_User.SelectedValue);
           
            DeleteUserRole(userId);
            LoadUserRolesGrid();
        }
    }
}
