using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SupplyBranch
{
    internal class ModuleDAL
    {
        private readonly DBHelper db = new DBHelper();


        public DataTable GetActiveModules()
        {
            string query = @"
        SELECT
            ModuleID,
            ModuleCode,
            ModuleName,
            SortOrder
        FROM dbo.SystemModules
        WHERE IsActive = 1
        ORDER BY SortOrder ASC, ModuleName ASC";

            return db.ExecuteQuery(query, null);
        }

        public HashSet<int> GetUserModuleIDs(int userID)
        {
            HashSet<int> allowedIDs = new HashSet<int>();

            string query = @"
        SELECT ModuleID
        FROM dbo.UserModuleAccess
        WHERE UserID = @UserID
          AND IsAllowed = 1";

            SqlParameter[] parameters =
            {
        new SqlParameter("@UserID", userID)
    };

            DataTable dt = db.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                if (row["ModuleID"] != DBNull.Value)
                {
                    allowedIDs.Add(Convert.ToInt32(row["ModuleID"]));
                }
            }

            return allowedIDs;
        }

        public void SaveUserModulePermissions(int userID, IEnumerable<int> allowedModuleIDs)
        {
            HashSet<int> allowedSet = new HashSet<int>(allowedModuleIDs ?? Enumerable.Empty<int>());
            DataTable activeModules = GetActiveModules();

            foreach (DataRow row in activeModules.Rows)
            {
                int moduleID = Convert.ToInt32(row["ModuleID"]);
                int isAllowedBit = allowedSet.Contains(moduleID) ? 1 : 0;

                string upsertQuery = @"
            IF EXISTS (SELECT 1 FROM dbo.UserModuleAccess WHERE UserID = @UserID AND ModuleID = @ModuleID)
            BEGIN
                UPDATE dbo.UserModuleAccess
                SET IsAllowed = @IsAllowed
                WHERE UserID = @UserID
                  AND ModuleID = @ModuleID;
            END
            ELSE
            BEGIN
                INSERT INTO dbo.UserModuleAccess (UserID, ModuleID, IsAllowed)
                VALUES (@UserID, @ModuleID, @IsAllowed);
            END";

                SqlParameter[] parameters =
                {
            new SqlParameter("@UserID", SqlDbType.Int) { Value = userID },
            new SqlParameter("@ModuleID", SqlDbType.Int) { Value = moduleID },
            new SqlParameter("@IsAllowed", SqlDbType.Bit) { Value = isAllowedBit }
        };

                db.ExecuteNonQuery(upsertQuery, parameters);
            }
        }
        public HashSet<string> GetAllowedModuleCodes(int userID)
        {
            HashSet<string> allowedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string query = @"
        SELECT m.ModuleCode
        FROM dbo.UserModuleAccess u
        INNER JOIN dbo.SystemModules m
            ON u.ModuleID = m.ModuleID
        WHERE u.UserID = @UserID
          AND u.IsAllowed = 1
          AND m.IsActive = 1";

            SqlParameter[] parameters =
            {
        new SqlParameter("@UserID", userID)
    };

            DataTable dt = db.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                string code = Convert.ToString(row["ModuleCode"])?.Trim();
                if (!string.IsNullOrWhiteSpace(code))
                {
                    allowedCodes.Add(code);
                }
            }

            return allowedCodes;
        }


        public DataTable GetAllModules()
        {
            string query = @"
        SELECT
            ModuleID,
            ModuleCode,
            ModuleName,
            SortOrder,
            IsActive
        FROM dbo.SystemModules
        ORDER BY SortOrder ASC, ModuleName ASC";

            return db.ExecuteQuery(query, null);
        }

        public bool ModuleCodeExists(string moduleCode, int excludeModuleID = 0)
        {
            string query = @"
                SELECT COUNT(*)
                FROM dbo.SystemModules
                WHERE ModuleCode = @ModuleCode
                  AND ModuleID <> @ExcludeModuleID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ModuleCode", moduleCode.Trim().ToUpper()),
                new SqlParameter("@ExcludeModuleID", excludeModuleID)
            };

            object result = db.ExecuteScalar(query, parameters);
            int count = (result == null || result == DBNull.Value)
                ? 0
                : Convert.ToInt32(result);

            return count > 0;
        }

        public bool InsertModule(string moduleCode, string moduleName, int sortOrder, bool isActive)
        {
            string query = @"
                INSERT INTO dbo.SystemModules
                (
                    ModuleCode,
                    ModuleName,
                    SortOrder,
                    IsActive
                )
                VALUES
                (
                    @ModuleCode,
                    @ModuleName,
                    @SortOrder,
                    @IsActive
                )";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ModuleCode", moduleCode.Trim().ToUpper()),
                new SqlParameter("@ModuleName", moduleName.Trim()),
                new SqlParameter("@SortOrder", sortOrder),
                new SqlParameter("@IsActive", isActive)
            };

            int rows = db.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public bool UpdateModule(int moduleID, string moduleCode, string moduleName, int sortOrder, bool isActive)
        {
            string query = @"
                UPDATE dbo.SystemModules
                SET
                    ModuleCode = @ModuleCode,
                    ModuleName = @ModuleName,
                    SortOrder  = @SortOrder,
                    IsActive   = @IsActive
                WHERE ModuleID = @ModuleID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ModuleID", moduleID),
                new SqlParameter("@ModuleCode", moduleCode.Trim().ToUpper()),
                new SqlParameter("@ModuleName", moduleName.Trim()),
                new SqlParameter("@SortOrder", sortOrder),
                new SqlParameter("@IsActive", isActive)
            };

            int rows = db.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }

        public bool ToggleModuleStatus(int moduleID, bool isActive)
        {
            string query = @"
                UPDATE dbo.SystemModules
                SET IsActive = @IsActive
                WHERE ModuleID = @ModuleID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ModuleID", moduleID),
                new SqlParameter("@IsActive", isActive)
            };

            int rows = db.ExecuteNonQuery(query, parameters);
            return rows > 0;
        }
    }
}