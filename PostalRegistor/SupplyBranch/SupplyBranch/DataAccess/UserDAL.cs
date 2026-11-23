using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace SupplyBranch
{
    internal class UserDAL
    {
        DBHelper db = new DBHelper();

        public bool AddUser(
    string fullName,
    string userName,
    string password,
    string email,
    string authorityID)
        {
            try
            {
                // Check duplicate UserName
                string checkQuery = @"
            SELECT COUNT(*)
            FROM UsersInfo
            WHERE UserName = @UserName";

                SqlParameter[] checkParameters =
                {
            new SqlParameter("@UserName", userName.Trim())
        };

                DataTable dt =
                    db.ExecuteQuery(checkQuery, checkParameters);

                // ExecuteQuery returns DataTable, so get count separately
                // Use scalar query instead if your DBHelper supports ExecuteScalar.

                string insertQuery = @"
            INSERT INTO UsersInfo
            (
                FullName,
                UserName,
                Password,
                UserEmail,
                Athu_ID
            )
            VALUES
            (
                @FullName,
                @UserName,
                @Password,
                @UserEmail,
                @Athu_ID
            )";

                SqlParameter[] parameters =
                {
            new SqlParameter("@FullName", fullName.Trim()),
            new SqlParameter("@UserName", userName.Trim()),
            new SqlParameter("@Password", password),
            new SqlParameter(
                "@UserEmail",
                string.IsNullOrWhiteSpace(email)
                    ? (object)DBNull.Value
                    : email.Trim()),
            new SqlParameter(
                "@Athu_ID",
                string.IsNullOrWhiteSpace(authorityID)
                    ? (object)DBNull.Value
                    : authorityID.Trim())
        };

                db.ExecuteNonQuery(insertQuery, parameters);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to add user.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        public bool UserNameExists(string userName)
        {
            try
            {
                string query = @"
            SELECT UserID
            FROM UsersInfo
            WHERE UserName = @UserName";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserName", userName.Trim())
        };

                DataTable dt = db.ExecuteQuery(query, parameters);

                return dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to check User Name.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return true;
            }
        }

        public DataTable GetUserByID(int userID)
        {
            string query = @"
        SELECT
            UserID,
            FullName,
            UserName,
            UserEmail,
            Athu_ID
        FROM UsersInfo
        WHERE UserID = @UserID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@UserID", userID)
    };

            return db.ExecuteQuery(query, parameters);
        }

        public bool UpdateUser(
    int userID,
    string fullName,
    string userName,
    string email,
    string authorityID)
        {
            try
            {
                string query = @"
            UPDATE UsersInfo
            SET
                FullName = @FullName,
                UserName = @UserName,
                UserEmail = @UserEmail,
                Athu_ID = @Athu_ID
            WHERE UserID = @UserID";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserID", userID),
            new SqlParameter("@FullName", fullName.Trim()),
            new SqlParameter("@UserName", userName.Trim()),
            new SqlParameter(
                "@UserEmail",
                string.IsNullOrWhiteSpace(email)
                    ? (object)DBNull.Value
                    : email.Trim()),
            new SqlParameter(
                "@Athu_ID",
                string.IsNullOrWhiteSpace(authorityID)
                    ? (object)DBNull.Value
                    : authorityID.Trim())
        };

                db.ExecuteNonQuery(query, parameters);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update user.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        public bool UserNameExistsForOtherUser(string userName, int userID)
        {
            try
            {
                string query = @"
            SELECT UserID
            FROM UsersInfo
            WHERE UserName = @UserName
              AND UserID <> @UserID";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserName", userName.Trim()),
            new SqlParameter("@UserID", userID)
        };

                DataTable dt = db.ExecuteQuery(query, parameters);

                return dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to check User Name.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return true;
            }
        }

        public DataTable GetAllUsers()
        {
            string query = @"
        SELECT
            UserID,
            FullName,
            UserName,
            UserEmail,
            Athu_ID,
            IsActive
        FROM UsersInfo
        WHERE UserID <> @CurrentUserID
        ORDER BY UserName";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CurrentUserID", CurrentUser.UserID)
    };

            return db.ExecuteQuery(query, parameters);
        }

        public bool Login(User user)
        {
            string sql = @"
        SELECT
            UserID,
            UserName,
            FullName
        FROM UsersInfo
        WHERE UserName = @UserName
          AND Password = @Password
          AND IsActive = 1";

            SqlParameter[] parameters =
            {
        new SqlParameter("@UserName", user.UserName),
        new SqlParameter("@Password", user.Password)
    };

            DataTable dt = db.ExecuteQuery(sql, parameters);

            // User does not exist, password incorrect,
            // or user is disabled.
            if (dt.Rows.Count == 0)
                return false;

            // Save logged-in user information
            CurrentUser.UserID =
                Convert.ToInt32(dt.Rows[0]["UserID"]);

            CurrentUser.UserName =
                Convert.ToString(dt.Rows[0]["UserName"]);

            CurrentUser.FullName =
                Convert.ToString(dt.Rows[0]["FullName"]);

            return true;
        }


        public bool ChangePassword(
      int userID,
      string currentPassword,
      string newPassword)
        {
            string sql = @"
        UPDATE UsersInfo
        SET Password = @NewPassword
        WHERE UserID = @UserID
          AND Password = @CurrentPassword";

            SqlParameter[] parameters =
            {
        new SqlParameter("@UserID", userID),
        new SqlParameter("@CurrentPassword", currentPassword),
        new SqlParameter("@NewPassword", newPassword)
    };

            int rows = db.ExecuteNonQuery(sql, parameters);

            return rows > 0;
        }

        public bool SetUserActiveStatus(int userID, bool isActive)
        {
            try
            {
                string sql = @"
            UPDATE UsersInfo
            SET IsActive = @IsActive
            WHERE UserID = @UserID";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserID", userID),
            new SqlParameter("@IsActive", isActive)
        };

                int rows = db.ExecuteNonQuery(sql, parameters);

                return rows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to change user status.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        public bool IsUserActive(int userID)
        {
            string sql = @"
        SELECT IsActive
        FROM UsersInfo
        WHERE UserID = @UserID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@UserID", userID)
    };

            object result = db.ExecuteScalar(sql, parameters);

            if (result == null || result == DBNull.Value)
                return false;

            return Convert.ToBoolean(result);
        }

        public bool DisableUser(int userID)
        {
            try
            {
                string query = @"
            UPDATE UsersInfo
            SET IsActive = 0
            WHERE UserID = @UserID
              AND UserName <> 'admin'";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserID", userID)
        };

                int rows = db.ExecuteNonQuery(query, parameters);

                return rows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to disable user.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        public bool ToggleUserStatus(int userID, bool isActive)
        {
            try
            {
                string sql = @"
            UPDATE UsersInfo
            SET IsActive = @IsActive
            WHERE UserID = @UserID";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserID", userID),
            new SqlParameter("@IsActive", isActive)
        };

                int rows = db.ExecuteNonQuery(sql, parameters);

                return rows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to change user status.\n\n" + ex.Message,
                    "User Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

    }
}