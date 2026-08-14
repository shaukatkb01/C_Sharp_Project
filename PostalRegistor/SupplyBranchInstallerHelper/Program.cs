using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SupplyBranchInstallerHelper
{
    internal class Program
    {
        static string LogFile =
            @"C:\SupplyBranchInstall.log";

        static void Main(string[] args)
        {
            try
            {
                Log("========================================");
                Log("Helper started");
                Log("Arguments: " + string.Join(" | ", args));

                // -----------------------------------------
                // 1. Get SQL installation script path
                // -----------------------------------------

                string sqlFile;

                //if (args.Length > 0 &&
                //    !string.IsNullOrWhiteSpace(args[0]))
                //{
                //    sqlFile = args[0];
                //}
                //else
                //{
                    sqlFile = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "SupplyDB_Install.sql");
                //}

                Log("SQL File: " + sqlFile);

                if (!File.Exists(sqlFile))
                {
                    throw new FileNotFoundException(
                        "SupplyDB_Install.sql was not found.",
                        sqlFile);
                }

                // -----------------------------------------
                // 2. Start LocalDB
                // -----------------------------------------

                StartLocalDB();

                // -----------------------------------------
                // 3. Connect to LocalDB
                // -----------------------------------------

                string masterConnection =
                    @"Data Source=(localdb)\MSSQLLocalDB;" +
                    "Initial Catalog=master;" +
                    "Integrated Security=True;" +
                    "Connect Timeout=30;" +
                    "TrustServerCertificate=True;";

                Log("Connecting to LocalDB...");

                using (SqlConnection con =
                    new SqlConnection(masterConnection))
                {
                    con.Open();

                    Log("Connected to LocalDB successfully.");

                    // -----------------------------------------
                    // 4. Create SupplyDB if it does not exist
                    // -----------------------------------------

                    string createDatabase = @"
IF DB_ID(N'SupplyDB') IS NULL
BEGIN
    CREATE DATABASE [SupplyDB];
END";

                    using (SqlCommand cmd =
                        new SqlCommand(createDatabase, con))
                    {
                        cmd.CommandTimeout = 120;
                        cmd.ExecuteNonQuery();
                    }

                    Log("SupplyDB checked/created successfully.");
                }

                // -----------------------------------------
                // 5. Run SQL installation script
                // -----------------------------------------

                string databaseConnection =
                    @"Data Source=(localdb)\MSSQLLocalDB;" +
                    "Initial Catalog=SupplyDB;" +
                    "Integrated Security=True;" +
                    "Connect Timeout=30;" +
                    "TrustServerCertificate=True;";

                string sqlScript =
                    File.ReadAllText(sqlFile);

                // Split script at GO statements
                string[] batches =
                    System.Text.RegularExpressions.Regex.Split(
                        sqlScript,
                        @"^\s*GO\s*($|\-\-.*$)",
                        System.Text.RegularExpressions.RegexOptions.Multiline |
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                using (SqlConnection con =
                    new SqlConnection(databaseConnection))
                {
                    con.Open();

                    Log("Connected to SupplyDB.");

                    foreach (string batch in batches)
                    {
                        string sql = batch.Trim();

                        if (string.IsNullOrWhiteSpace(sql))
                            continue;

                        using (SqlCommand cmd =
                            new SqlCommand(sql, con))
                        {
                            cmd.CommandTimeout = 120;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                Log("SupplyDB installation completed successfully.");

                Console.WriteLine(
                    "SupplyDB installation completed successfully.");
            }
            catch (Exception ex)
            {
                Log("ERROR:");
                Log(ex.ToString());

                Console.WriteLine(
                    "Database installation failed:");

                Console.WriteLine(ex.Message);
            }
        }

        // =====================================================
        // Start LocalDB
        // =====================================================

        private static void StartLocalDB()
        {
            Log("Checking MSSQLLocalDB...");

            string result = RunSqlLocalDb(
                "info MSSQLLocalDB");

            Log("sqllocaldb info result:");
            Log(result);

            // Try to start the existing instance
            Log("Starting MSSQLLocalDB...");

            string startResult = RunSqlLocalDb(
                "start MSSQLLocalDB");

            Log("sqllocaldb start result:");
            Log(startResult);

            // Give LocalDB some time to start
            Thread.Sleep(3000);

            // Verify again
            string verifyResult = RunSqlLocalDb(
                "info MSSQLLocalDB");

            Log("MSSQLLocalDB after start:");
            Log(verifyResult);

            if (verifyResult.IndexOf(
                    "State: Running",
                    StringComparison.OrdinalIgnoreCase) < 0)
            {
                Log("LocalDB did not report Running state.");
            }
            else
            {
                Log("MSSQLLocalDB is running.");
            }
        }

        // =====================================================
        // Run sqllocaldb.exe
        // =====================================================

        private static string RunSqlLocalDb(string arguments)
        {
            string[] possiblePaths =
            {
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe"),

                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFilesX86),
                    @"Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe"),

                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles),
                    @"Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe"),

                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFilesX86),
                    @"Microsoft SQL Server\150\Tools\Binn\SqlLocalDB.exe")
            };

            string exePath = null;

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    exePath = path;
                    break;
                }
            }

            // If not found in known locations,
            // try PATH environment variable.
            if (exePath == null)
                exePath = "sqllocaldb.exe";

            Log("Executing: " + exePath + " " + arguments);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = psi;

                process.Start();

                string output =
                    process.StandardOutput.ReadToEnd();

                string error =
                    process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(error))
                {
                    output += Environment.NewLine +
                              "ERROR: " + error;
                }

                return output;
            }
        }

        // =====================================================
        // Log
        // =====================================================

        private static void Log(string message)
        {
            try
            {
                File.AppendAllText(
                    LogFile,
                    DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt") +
                    " - " +
                    message +
                    Environment.NewLine);
            }
            catch
            {
                // Do not stop installation because logging failed.
            }
        }
    }
}