using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SupplyBranch.DataAccess
{
    public class DatabaseMigrator
    {
        private static string connectionString = "Data Source=.;Initial Catalog=YourDbName;Integrated Security=True;"; // Aap ki Connection String

        public static void ApplyMigrations()
        {
            try
            {
                int currentDbVersion = GetCurrentDbVersion();

                // -------------------------------------------------------------
                // Migration 1 -> Version 2 (e.g. Naya Column Add Karna)
                // -------------------------------------------------------------
                if (currentDbVersion < 2)
                {
                    string scriptV2 = @"
                        -- Columns add karein
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SupplyMaster' AND COLUMN_NAME = 'Remarks')
                        BEGIN
                            ALTER TABLE SupplyMaster ADD Remarks NVARCHAR(250) NULL;
                        END

                        -- Version update karein
                        UPDATE DbVersion SET VersionNo = 2, UpdatedOn = GETDATE();
                    ";

                    ExecuteSqlScript(scriptV2);
                    Debug.WriteLine("Database upgraded to Version 2 successfully.");
                }

                // -------------------------------------------------------------
                // Migration 2 -> Version 3 (e.g. Naya Table Banana)
                // -------------------------------------------------------------
                if (currentDbVersion < 3)
                {
                    string scriptV3 = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLogs')
                        BEGIN
                            CREATE TABLE AuditLogs (
                                LogID INT IDENTITY(1,1) PRIMARY KEY,
                                UserAction NVARCHAR(200),
                                CreatedOn DATETIME DEFAULT GETDATE()
                            );
                        END

                        UPDATE DbVersion SET VersionNo = 3, UpdatedOn = GETDATE();
                    ";

                    ExecuteSqlScript(scriptV3);
                    Debug.WriteLine("Database upgraded to Version 3 successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DATABASE MIGRATION ERROR: " + ex.Message);
            }
        }

        private static int GetCurrentDbVersion()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Agar DbVersion table maujood na ho toh pehle usey banayein
                string checkTableSql = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DbVersion')
                    BEGIN
                        CREATE TABLE DbVersion (VersionNo INT NOT NULL, UpdatedOn DATETIME DEFAULT GETDATE());
                        INSERT INTO DbVersion (VersionNo) VALUES (1);
                    END";

                using (SqlCommand cmdTable = new SqlCommand(checkTableSql, conn))
                {
                    cmdTable.ExecuteNonQuery();
                }

                // Current version No select karein
                string selectSql = "SELECT TOP 1 VersionNo FROM DbVersion;";
                using (SqlCommand cmd = new SqlCommand(selectSql, conn))
                {
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 1;
                }
            }
        }

        private static void ExecuteSqlScript(string sqlScript)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlScript, conn, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit(); // SQL transaction successfully save karein
                    }
                    catch
                    {
                        transaction.Rollback(); // Error par rollback karein taakay DB corrupt na ho
                        throw;
                    }
                }
            }
        }
    }
}