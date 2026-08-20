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
            string appDir =
                AppDomain.CurrentDomain.BaseDirectory
                .TrimEnd(Path.DirectorySeparatorChar);

            string currentExe =
                Application.ExecutablePath;

            string currentProcessName =
                Process.GetCurrentProcess().ProcessName;

            string tempRoot =
                Path.Combine(
                    Path.GetTempPath(),
                    "SupplyBranch_Update_" +
                    Guid.NewGuid().ToString("N"));

            string extractDir =
                Path.Combine(
                    tempRoot,
                    "Extracted");

            string batchFile =
                Path.Combine(
                    tempRoot,
                    "UpdateRunner.bat");

            try
            {
                // =====================================================
                // 1. Check ZIP
                // =====================================================

                if (string.IsNullOrWhiteSpace(zipPath) ||
                    !File.Exists(zipPath))
                {
                    MessageBox.Show(
                        "Update ZIP file nahi mili.",
                        "Update",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }


                // =====================================================
                // 2. Create temporary folders
                // =====================================================

                Directory.CreateDirectory(extractDir);


                // =====================================================
                // 3. Extract ZIP
                // =====================================================

                ZipFile.ExtractToDirectory(
                    zipPath,
                    extractDir);


                // =====================================================
                // 4. Find NEW SupplyBranch.exe
                // =====================================================

                string[] exeFiles =
                    Directory.GetFiles(
                        extractDir,
                        "SupplyBranch.exe",
                        SearchOption.AllDirectories);

                if (exeFiles.Length == 0)
                {
                    MessageBox.Show(
                        "Update ZIP mein SupplyBranch.exe nahi mila.\r\n\r\n" +
                        "Extracted folder:\r\n" +
                        extractDir,
                        "Update",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }


                // Agar multiple EXE hon to sab se deep/latest file
                string sourceExe =
                    exeFiles
                        .OrderByDescending(x => x.Length)
                        .First();


                string sourceDir =
                    Path.GetDirectoryName(sourceExe);


                // =====================================================
                // 5. Check New Version
                // =====================================================

                FileVersionInfo newVersion =
                    FileVersionInfo.GetVersionInfo(sourceExe);

                FileVersionInfo oldVersion =
                    FileVersionInfo.GetVersionInfo(currentExe);


                // =====================================================
                // 6. Debug information
                // =====================================================

                string debugMessage =
                    "Current EXE:\r\n" +
                    currentExe +
                    "\r\n\r\n" +

                    "Current Version:\r\n" +
                    oldVersion.FileVersion +
                    "\r\n\r\n" +

                    "New EXE:\r\n" +
                    sourceExe +
                    "\r\n\r\n" +

                    "New Version:\r\n" +
                    newVersion.FileVersion +
                    "\r\n\r\n" +

                    "Application Folder:\r\n" +
                    appDir;


                // =====================================================
                // OPTIONAL DEBUG
                // Agar zaroorat ho to uncomment karo
                // =====================================================

                // MessageBox.Show(
                //     debugMessage,
                //     "Update Debug",
                //     MessageBoxButtons.OK,
                //     MessageBoxIcon.Information);


                // =====================================================
                // 7. Create updater BAT
                // =====================================================

                string batContent = $@"@echo off
setlocal EnableExtensions EnableDelayedExpansion

title SupplyBranch Updater

echo.
echo ================================================
echo             SupplyBranch Updater
echo ================================================
echo.

echo Waiting for application to close...
echo.

:WAIT

tasklist /FI ""IMAGENAME eq {currentProcessName}.exe"" 2>NUL | find /I ""{currentProcessName}.exe"" >NUL

if not errorlevel 1 (
    timeout /t 1 /nobreak >nul
    goto WAIT
)

echo Application closed.
echo.

timeout /t 2 /nobreak >nul


REM ========================================================
REM Backup old EXE
REM ========================================================

if exist ""{appDir}\SupplyBranch.exe"" (
    echo Creating backup...

    copy /Y ""{appDir}\SupplyBranch.exe"" ""{appDir}\SupplyBranch.exe.old"" >nul
)


REM ========================================================
REM Copy ALL new application files
REM ========================================================

echo.
echo Installing new version...
echo.

xcopy ""{sourceDir}\*"" ""{appDir}\"" /E /I /Y /H /R /C

if errorlevel 1 (
    echo.
    echo ================================================
    echo ERROR: Files could not be copied.
    echo ================================================
    echo.
    pause
    exit /b 1
)

echo.
echo Files copied successfully.
echo.


REM ========================================================
REM Verify EXE exists
REM ========================================================

if not exist ""{appDir}\SupplyBranch.exe"" (
    echo.
    echo ERROR: SupplyBranch.exe not found after update.
    echo.
    pause
    exit /b 1
)


REM ========================================================
REM Delete ZIP
REM ========================================================

if exist ""{zipPath}"" (
    del /F /Q ""{zipPath}""
)


REM ========================================================
REM Cleanup extracted update
REM ========================================================

if exist ""{tempRoot}"" (
    rmdir /S /Q ""{tempRoot}""
)


REM ========================================================
REM Start NEW application
REM ========================================================

echo.
echo Starting updated SupplyBranch...
echo.

timeout /t 2 /nobreak >nul

start """" ""{appDir}\SupplyBranch.exe""


REM ========================================================
REM Exit updater
REM ========================================================

exit /b 0
";


                File.WriteAllText(
                    batchFile,
                    batContent);


                // =====================================================
                // 8. Start BAT as Administrator
                // =====================================================

                ProcessStartInfo psi =
                    new ProcessStartInfo
                    {
                        FileName = batchFile,
                        WorkingDirectory = tempRoot,
                        UseShellExecute = true,
                        Verb = "runas",
                        CreateNoWindow = false
                    };


                Process.Start(psi);


                // =====================================================
                // 9. Close current application
                // =====================================================

                Application.Exit();
            }
            catch (Exception ex)
            {
                try
                {
                    if (Directory.Exists(tempRoot))
                    {
                        Directory.Delete(
                            tempRoot,
                            true);
                    }
                }
                catch
                {
                }


                MessageBox.Show(
                    "Update apply karne mein error aaya:\r\n\r\n" +
                    ex.Message +
                    "\r\n\r\n" +
                    "Current EXE:\r\n" +
                    currentExe,
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
