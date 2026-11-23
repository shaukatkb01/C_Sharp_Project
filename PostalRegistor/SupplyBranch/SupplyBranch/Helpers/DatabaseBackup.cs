using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace SupplyBranch.Helpers
{
    public static class DatabaseBackup
    {
        public static bool BackupDatabase()
        {
            try
            {
                // ==========================================
                // Get Saved Backup Location
                // ==========================================

                string backupFolder =
                    Properties.Settings.Default.BackupFolder;

                // ==========================================
                // First Time / Invalid Location
                // ==========================================

                if (string.IsNullOrWhiteSpace(backupFolder) ||
                    !Directory.Exists(backupFolder))
                {
                    using (FolderBrowserDialog dialog =
                           new FolderBrowserDialog())
                    {
                        dialog.Description =
                            "Select folder where SupplyDB backup will be saved.";

                        dialog.ShowNewFolderButton = true;

                        if (dialog.ShowDialog() != DialogResult.OK)
                            return false;

                        backupFolder = dialog.SelectedPath;

                        // Save selected location permanently
                        Properties.Settings.Default.BackupFolder =
                            backupFolder;

                        Properties.Settings.Default.Save();
                    }
                }

                // ==========================================
                // Backup File
                // ==========================================

                string backupPath =
                    Path.Combine(
                        backupFolder,
                        "SupplyDB.bak");

                // ==========================================
                // SQL Server Connection
                // ==========================================

                string connectionString =
                    @"Data Source=(localdb)\MSSQLLocalDB;
                      Initial Catalog=SupplyDB;
                      Integrated Security=True;
                      TrustServerCertificate=True;";

                using (SqlConnection con =
                       new SqlConnection(connectionString))
                {
                    con.Open();

                    // ==========================================
                    // SQL Backup
                    // INIT = overwrite existing backup
                    // ==========================================

                    string sql = @"
BACKUP DATABASE [SupplyDB]
TO DISK = @BackupPath
WITH INIT,
     FORMAT,
     NAME = 'SupplyDB Full Backup',
     STATS = 10;";

                    using (SqlCommand cmd =
                           new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(
                            "@BackupPath",
                            SqlDbType.NVarChar,
                            500).Value = backupPath;

                        cmd.ExecuteNonQuery();
                    }
                }

                // ==========================================
                // Success
                // ==========================================

               

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database backup failed."
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    "Backup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }


        // ==========================================
        // Change Backup Location
        // ==========================================

        public static bool ChangeBackupLocation()
        {
            try
            {
                string currentFolder =
                    Properties.Settings.Default.BackupFolder;

                using (FolderBrowserDialog dialog =
                       new FolderBrowserDialog())
                {
                    dialog.Description =
                        "Select new folder for SupplyDB backup.";

                    dialog.ShowNewFolderButton = true;

                    if (!string.IsNullOrWhiteSpace(currentFolder) &&
                        Directory.Exists(currentFolder))
                    {
                        dialog.SelectedPath = currentFolder;
                    }

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return false;

                    Properties.Settings.Default.BackupFolder =
                        dialog.SelectedPath;

                    Properties.Settings.Default.Save();

                    MessageBox.Show(
                        "Backup location changed successfully."
                        + Environment.NewLine
                        + Environment.NewLine
                        + dialog.SelectedPath,
                        "Backup Location",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not change backup location."
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    "Backup Location Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }


        // ==========================================
        // Get Current Backup Location
        // ==========================================

        public static string GetBackupLocation()
        {
            return Properties.Settings.Default.BackupFolder;
        }

        public static bool RestoreDatabase()
        {
            // ==========================================
            // Admin Only
            // ==========================================

            if (!string.Equals(
                    CurrentUser.UserName,
                    "admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Only Admin user can restore the database.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            try
            {
                // ==========================================
                // Create Safety Backup
                // ==========================================

                if (!CreateSafetyBackup())
                {
                    MessageBox.Show(
                        "Restore cancelled because the current database backup could not be created.",
                        "Restore Cancelled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // ==========================================
                // Select Backup File
                // ==========================================

                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Title = "Select SupplyDB Backup File";
                    dialog.Filter = "SQL Server Backup (*.bak)|*.bak";
                    dialog.Multiselect = false;

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return false;

                    string backupPath = dialog.FileName;

                    // ==========================================
                    // Confirmation
                    // ==========================================

                    DialogResult result = MessageBox.Show(
                        "Restoring the database will replace the current database.\n\n" +
                        "All data currently in SupplyDB will be replaced by the selected backup.\n\n" +
                        "Do you want to continue?",
                        "Confirm Database Restore",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);

                    if (result != DialogResult.Yes)
                        return false;

                    // ==========================================
                    // Connection
                    // ==========================================

                    string masterConnectionString =
                        @"Data Source=(localdb)\MSSQLLocalDB;
                  Initial Catalog=master;
                  Integrated Security=True;
                  TrustServerCertificate=True;";

                    using (SqlConnection con =
                           new SqlConnection(masterConnectionString))
                    {
                        con.Open();

                        // ==========================================
                        // Force all users out of SupplyDB
                        // ==========================================

                        string sql = @"
ALTER DATABASE [SupplyDB]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

RESTORE DATABASE [SupplyDB]
FROM DISK = @BackupPath
WITH REPLACE;

ALTER DATABASE [SupplyDB]
SET MULTI_USER;";

                        using (SqlCommand cmd =
                               new SqlCommand(sql, con))
                        {
                            cmd.Parameters.Add(
                                new SqlParameter("@BackupPath", backupPath));

                            cmd.CommandTimeout = 300;

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show(
                        "Database restored successfully.\n\n" +
                        "The application will now restart.",
                        "Restore Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database restore failed.\n\n" +
                    ex.Message,
                    "Restore Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        public static bool CreateSafetyBackup()
        {
            try
            {
                string backupFolder =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.MyDocuments),
                        "SupplyBranch Backups");

                if (!Directory.Exists(backupFolder))
                    Directory.CreateDirectory(backupFolder);

                string backupPath =
                    Path.Combine(
                        backupFolder,
                        "SupplyDB_BeforeRestore.bak");

                string connectionString =
                    @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=master;
              Integrated Security=True;
              TrustServerCertificate=True;";

                using (SqlConnection con =
                       new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = @"
BACKUP DATABASE [SupplyDB]
TO DISK = @BackupPath
WITH INIT,
     NAME = 'SupplyDB Safety Backup Before Restore';";

                    using (SqlCommand cmd =
                           new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(
                            new SqlParameter("@BackupPath", backupPath));

                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Safety backup could not be created.\n\n" +
                    ex.Message,
                    "Backup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        public static bool RestoreSafetyBackup()
        {
            if (!string.Equals(
                    CurrentUser.UserName,
                    "admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Only Admin user can restore the safety backup.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            try
            {
                string backupPath = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments),
                    "SupplyBranch Backups",
                    "SupplyDB_BeforeRestore.bak");

                if (!File.Exists(backupPath))
                {
                    MessageBox.Show(
                        "Safety backup file was not found.\n\n" +
                        backupPath,
                        "Safety Backup Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                DialogResult result = MessageBox.Show(
                    "The database will be restored to the state it had BEFORE the last restore operation.\n\n" +
                    "Continue?",
                    "Confirm Safety Restore",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                    return false;

                string connectionString =
                    @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=master;
              Integrated Security=True;
              TrustServerCertificate=True;";

                using (SqlConnection con =
                       new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = @"
ALTER DATABASE [SupplyDB]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

RESTORE DATABASE [SupplyDB]
FROM DISK = @BackupPath
WITH REPLACE;

ALTER DATABASE [SupplyDB]
SET MULTI_USER;";

                    using (SqlCommand cmd =
                           new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(
                            new SqlParameter("@BackupPath", backupPath));

                        cmd.CommandTimeout = 300;

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Safety backup restored successfully.\n\n" +
                    "The application will now restart.",
                    "Restore Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Safety restore failed.\n\n" +
                    ex.Message,
                    "Restore Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }
    }


    }