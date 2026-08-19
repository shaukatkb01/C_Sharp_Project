using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO;
using System.IO.Compression; // Nuget / Assembly reference: System.IO.Compression.FileSystem
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 // Nuget / Assembly reference: System.IO.Compression.FileSystem
using System.Data.SqlClient;
namespace SupplyBranch.DataAccess
{
    internal class UpdateDAL
    {


        private static void ExecuteSqlScript(string sqlScript)
        {
            // 'GO' keywords par query ko alag alag command me split karein
            string[] commands = System.Text.RegularExpressions.Regex.Split(
                sqlScript,
                @"^\s*GO\s*$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline
            );

            DBConnection db = new DBConnection();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (string cmdText in commands)
                        {
                            if (!string.IsNullOrWhiteSpace(cmdText))
                            {
                                using (SqlCommand cmd = new SqlCommand(cmdText, conn, tran))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        tran.Commit(); // Script kamyabi se execute hone par save karein
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback(); // Error aane par purani halat me wapas layein
                        throw new Exception("Database script update fail ho gayi: " + ex.Message);
                    }
                }
            }
        }
        public static void ApplyUpdateAndRestart(string zipPath)
    {
        string appDir = AppDomain.CurrentDomain.BaseDirectory;
        string tempExtractPath = Path.Combine(Path.GetTempPath(), "ExtractedUpdate");
        string batPath = Path.Combine(Path.GetTempPath(), "update_runner.bat");

        try
        {
            // 1. Purana Temp Folder Clean Karein
            if (Directory.Exists(tempExtractPath))
            {
                Directory.Delete(tempExtractPath, true);
            }

            // 2. Zip Ko C# Me Temp Folder Par Extract Karein
            ZipFile.ExtractToDirectory(zipPath, tempExtractPath);

            // 3. Check Karein Agar Database Script (update.sql) Majood Hai To Run Karein
            string sqlFilePath = Path.Combine(tempExtractPath, "update.sql");
            if (File.Exists(sqlFilePath))
            {
                string sqlContent = File.ReadAllText(sqlFilePath);
                ExecuteSqlScript(sqlContent); // Database Update Function Call
            }

            // 4. Batch Script Create Karein (Jo Temp Files Ko App Folder Me Copy Karegi)
            string exePath = Path.Combine(appDir, "SupplyBranch.exe");
            string batContent = $@"@echo off
timeout /t 3 /nobreak > nul
powershell -Command ""Copy-Item -Path '{tempExtractPath}\*' -Destination '{appDir}' -Recurse -Force""
del ""{zipPath}""
rmdir /s /q ""{tempExtractPath}""
start """" ""{exePath}""
del ""%~f0""";

            File.WriteAllText(batPath, batContent);

            // 5. Batch Script Ko Admin Rights Ke Sath Launch Karein
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = batPath,
                Verb = "runas",
                CreateNoWindow = true,
                UseShellExecute = true
            };

            Process.Start(psi);
            Application.Exit(); // Application close karein taakay file replace ho sakein
        }
        catch (Exception ex)
        {
            MessageBox.Show("Update apply karne mein masla aaya: " + ex.Message,
                            "Update Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OnDownloadCompleted()
        {
            string zipPath = Path.Combine(Path.GetTempPath(), "SupplyBranch_Update.zip");
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string batPath = Path.Combine(Path.GetTempPath(), "update_runner.bat");

            // 1. Batch script create karein
            string batContent = $@"
@echo off
timeout /t 3 /nobreak > nul
powershell -Command ""Expand-Archive -Path '{zipPath}' -DestinationPath '{appDir}' -Force""
del ""{zipPath}""
start """" ""{Path.Combine(appDir, "SupplyBranch.exe")}""
del ""%~f0""
";
            File.WriteAllText(batPath, batContent);

            // 2. Batch script run karein (hidden window mein)
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = batPath,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);

            // 3. Main Application ko band karein taakay files overwrite ho sakein
            Application.Exit();
        }
    }
}
