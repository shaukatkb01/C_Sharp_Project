using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace SupplyBranch.DataAccess
{
    public class UserDAL
    {
        private readonly DBHelper _db;
        private readonly ILogger<UserDAL>? _logger;

        public UserDAL(DBHelper db, ILogger<UserDAL>? logger)
        {
            _db = db;
            _logger = logger;
        }

        public bool AddUser(string fullName, string userName, string password, string email, string authorityID)
        {
            try
            {
                string checkQuery = @"SELECT COUNT(*) FROM dbo.UsersInfo WHERE UserName = @UserName";

                SqlParameter[] checkParameters = { new SqlParameter("@UserName", userName?.Trim() ?? string.Empty) };

                DataTable dt = _db.ExecuteQuery(checkQuery, checkParameters);

                if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
                    return false; // user exists

                string insertQuery = @"
            INSERT INTO dbo.UsersInfo
            (
                FullName,
                UserName,
                Password,
                UserEmail,
                Athu_ID,
                IsActive
            )
            VALUES
            (
                @FullName,
                @UserName,
                @Password,
                @UserEmail,
                @Athu_ID,
                1
            )";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@FullName", fullName?.Trim() ?? string.Empty),
                    new SqlParameter("@UserName", userName?.Trim() ?? string.Empty),
                    new SqlParameter("@Password", password ?? string.Empty),
                    new SqlParameter("@UserEmail", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email.Trim()),
                    new SqlParameter("@Athu_ID", string.IsNullOrWhiteSpace(authorityID) ? (object)DBNull.Value : authorityID.Trim())
                };

                _db.ExecuteNonQuery(insertQuery, parameters);

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unable to add user {UserName}", userName);
                throw;
            }
        }

        public bool UserNameExists(string userName)
        {
            try
            {
                string query = @"SELECT UserID FROM dbo.UsersInfo WHERE UserName = @UserName";

                SqlParameter[] parameters = { new SqlParameter("@UserName", userName?.Trim() ?? string.Empty) };

                DataTable dt = _db.ExecuteQuery(query, parameters);

                return dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error checking username exists for {UserName}", userName);
                throw;
            }
        }

        public DataTable GetUserByID(int userID)
        {
            try
            {
                string query = @"
        SELECT
            UserID,
            FullName,
            UserName,
            UserEmail,
            Athu_ID,
            IsActive
        FROM dbo.UsersInfo
        WHERE UserID = @UserID";

                SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };

                return _db.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching user by id {UserID}", userID);
                throw;
            }
        }

        public bool UpdateUser(int userID, string fullName, string userName, string email, string authorityID)
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
                    new SqlParameter("@UserEmail", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email.Trim()),
                    new SqlParameter("@Athu_ID", string.IsNullOrWhiteSpace(authorityID) ? (object)DBNull.Value : authorityID.Trim())
                };

                _db.ExecuteNonQuery(query, parameters);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to update user: " + ex.Message, ex);
            }
        }

        public bool UserNameExistsForOtherUser(string userName, int userID)
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

            DataTable dt = _db.ExecuteQuery(query, parameters);

            return dt.Rows.Count > 0;
        }

        public DataTable GetAllUsers(int currentUserId)
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

            SqlParameter[] parameters = { new SqlParameter("@CurrentUserID", currentUserId) };

            return _db.ExecuteQuery(query, parameters);
        }

        public User? Login(string userName, string password)
        {
            try
            {
                // Fetch user row by username to allow detailed checks (password match, active flag)
                // Use case-insensitive comparison to avoid issues with DB collation.
                string sql = @"
        SELECT TOP 1
            UserID,
            UserName,
            Password,
            FullName,
            IsActive
        FROM dbo.UsersInfo
        WHERE LOWER(UserName) = LOWER(@UserName)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@UserName", userName?.Trim() ?? string.Empty)
                };

                // Log the SQL and parameter for diagnostics
                _logger?.LogInformation("Executing SQL for login: {Sql} with UserName={UserName}", sql, userName);
                Console.WriteLine($"[UserDAL.Login] Executing SQL: {sql.Replace("\n", " ")}");
                Console.WriteLine($"[UserDAL.Login] Parameter @UserName = '{userName}'");

                DataTable dt = _db.ExecuteQuery(sql, parameters);

                Console.WriteLine($"[UserDAL.Login] Query returned {dt.Rows.Count} rows");

                if (dt.Rows.Count == 0)
                {
                    // no such user
                    _logger?.LogInformation("Login failed - user not found: {UserName}", userName);
                    return null;
                }

                var row = dt.Rows[0];
                bool isActive = row["IsActive"] != DBNull.Value && Convert.ToInt32(row["IsActive"]) == 1;

                // If the account is inactive, throw an explicit exception so the controller can show a clear message.
                if (!isActive)
                {
                    _logger?.LogWarning("Login attempt for inactive user {UserName}", userName);
                    Console.WriteLine($"[UserDAL.Login] User '{userName}' is inactive (IsActive={row["IsActive"]}). Throwing InvalidOperationException.");
                    throw new InvalidOperationException("User account is inactive. Please contact your administrator.");
                }

                string storedPassword = Convert.ToString(row["Password"]) ?? string.Empty;

                // Plain-text comparison (matches original WinForms behavior)
                if (!string.Equals(storedPassword, password ?? string.Empty, StringComparison.Ordinal))
                {
                    _logger?.LogInformation("Login failed - invalid password for user {UserName}", userName);
                    Console.WriteLine($"[UserDAL.Login] Password mismatch for user '{userName}'");
                    return null;
                }

                Console.WriteLine($"[UserDAL.Login] Login successful for user '{userName}', UserID={row["UserID"]}");

                return new User
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    UserName = Convert.ToString(row["UserName"]),
                    FullName = Convert.ToString(row["FullName"])
                };
            }
            catch (InvalidOperationException)
            {
                // rethrow so controller can display specific inactive message
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in UserDAL.Login for user {UserName}", userName);
                throw;
            }
        }

        public bool ChangePassword(int userID, string currentPassword, string newPassword)
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

            int rows = _db.ExecuteNonQuery(sql, parameters);

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

                int rows = _db.ExecuteNonQuery(sql, parameters);

                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to change user status: " + ex.Message, ex);
            }
        }

        public bool IsUserActive(int userID)
        {
            string sql = @"
        SELECT IsActive
        FROM UsersInfo
        WHERE UserID = @UserID";

            SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };

            object result = _db.ExecuteScalar(sql, parameters);

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

                SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };

                int rows = _db.ExecuteNonQuery(query, parameters);

                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to disable user: " + ex.Message, ex);
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

                int rows = _db.ExecuteNonQuery(sql, parameters);

                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to change user status: " + ex.Message, ex);
            }
        }
    }
}
