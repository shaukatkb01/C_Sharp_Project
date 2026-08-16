using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplyBranch.DataAccess
{
    internal class UpdateDAL
    {


        public static void ApplyUpdateAndRestart(string zipPath)
        {
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

            // 2. Batch script ko Admin Rights ke sath launch karein
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = batPath,
                Verb = "runas",             // <-- Yeh line batch script ko Admin rights degi
                CreateNoWindow = true,
                UseShellExecute = true
            };

            try
            {
                Process.Start(psi);
                Application.Exit();         // Application close karein taakay file unlock ho jaye
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update apply karne ke liye Administrator rights zaroori hain: " + ex.Message,
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
