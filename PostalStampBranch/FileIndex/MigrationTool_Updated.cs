using System;
using System.Data;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class YourForm : Form
    {
        // ═══════════════════════════════════════════════════════════════════════════════
        // MIGRATION BUTTON CLICK - Main Entry Point
        // ═══════════════════════════════════════════════════════════════════════════════
        
        private void button1_Click(object sender, EventArgs e)
        {
            string accessPath = @"F:\New folder\ProjectNew\PostalStampBranchData\backend\PostalStampsBranch_be.accdb";
            string accessConStr = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={accessPath};";
            string sqlConStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PSDB;Integrated Security=True;";
            
            this.Cursor = Cursors.WaitCursor;
            
            using (OleDbConnection accCon = new OleDbConnection(accessConStr))
            using (SqlConnection sqlCon = new SqlConnection(sqlConStr))
            {
                try
                {
                    accCon.Open();
                    sqlCon.Open();
                    
                    // ═══════════════════════════════════════════════════════
                    // Migration Process
                    // ═══════════════════════════════════════════════════════
                    
                    int totalInserted = 0;
                    int totalUpdated = 0;
                    int totalSkipped = 0;
                    
                    // PhilitelicBuearu table migrate karo
                    var result = MigrateTable(accCon, sqlCon, "PhilitelicBuearu", "Id");
                    totalInserted += result.Inserted;
                    totalUpdated += result.Updated;
                    totalSkipped += result.Skipped;
                    
                    // Agar aur tables migrate karni hain to uncomment karo:
                    
                    // StockPrice table (Primary Key column ka naam specify karo)
                    // var result2 = MigrateTable(accCon, sqlCon, "StockPrice", "PriceID");
                    // totalInserted += result2.Inserted;
                    // totalUpdated += result2.Updated;
                    // totalSkipped += result2.Skipped;
                    
                    // StockPhilQuantity table
                    // var result3 = MigrateTable(accCon, sqlCon, "StockPhilQuantity", "QuantityID");
                    // totalInserted += result3.Inserted;
                    // totalUpdated += result3.Updated;
                    // totalSkipped += result3.Skipped;
                    
                    // ═══════════════════════════════════════════════════════
                    // Success Message
                    // ═══════════════════════════════════════════════════════
                    
                    string message = $@"✅ Migration Completed Successfully!

📊 Summary:
━━━━━━━━━━━━━━━━━━━━━━━━━━
➕ New Records Inserted: {totalInserted}
🔄 Existing Records Updated: {totalUpdated}
⏭️ Skipped (No Changes): {totalSkipped}
━━━━━━━━━━━━━━━━━━━━━━━━━━
📝 Total Processed: {totalInserted + totalUpdated + totalSkipped}";
                    
                    MessageBox.Show(message, "Migration Status", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Migration Error:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // MIGRATE TABLE METHOD - Main Logic
        // ═══════════════════════════════════════════════════════════════════════════════
        
        /// <summary>
        /// Access se SQL Server mein data migrate karta hai
        /// Agar record exist karta hai to UPDATE, warna INSERT
        /// </summary>
        /// <param name="accCon">Access database connection</param>
        /// <param name="sqlCon">SQL Server connection</param>
        /// <param name="tableName">Table name (same in both databases)</param>
        /// <param name="primaryKeyColumn">Primary key column name (Identity column)</param>
        /// <returns>Migration statistics</returns>
        private MigrationResult MigrateTable(
            OleDbConnection accCon, 
            SqlConnection sqlCon, 
            string tableName, 
            string primaryKeyColumn)
        {
            MigrationResult result = new MigrationResult();
            
            try
            {
                // ─────────────────────────────────────────────────────────
                // STEP 1: Access se data fetch karo
                // ─────────────────────────────────────────────────────────
                
                string selectQuery = $"SELECT * FROM {tableName}";
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectQuery, accCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show($"⚠️ No data found in Access table: {tableName}", 
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return result;
                }
                
                // ─────────────────────────────────────────────────────────
                // STEP 2: SQL Server mein column names fetch karo
                // (Identity column ko skip karne ke liye)
                // ─────────────────────────────────────────────────────────
                
                var columns = GetNonIdentityColumns(sqlCon, tableName, primaryKeyColumn);
                
                if (columns.Count == 0)
                {
                    MessageBox.Show($"❌ No columns found in SQL table: {tableName}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
                
                // ─────────────────────────────────────────────────────────
                // STEP 3: SET IDENTITY_INSERT ON (Primary Key preserve karne ke liye)
                // ─────────────────────────────────────────────────────────
                
                SqlCommand setIdentityOn = new SqlCommand(
                    $"SET IDENTITY_INSERT {tableName} ON", sqlCon);
                setIdentityOn.ExecuteNonQuery();
                
                // ─────────────────────────────────────────────────────────
                // STEP 4: Har row ko process karo (INSERT ya UPDATE)
                // ─────────────────────────────────────────────────────────
                
                foreach (DataRow row in dt.Rows)
                {
                    object primaryKeyValue = row[primaryKeyColumn];
                    
                    // Check: Record already exists?
                    bool exists = RecordExists(sqlCon, tableName, primaryKeyColumn, primaryKeyValue);
                    
                    if (exists)
                    {
                        // UPDATE existing record
                        bool hasChanges = UpdateRecord(sqlCon, tableName, primaryKeyColumn, 
                            primaryKeyValue, row, columns);
                        
                        if (hasChanges)
                            result.Updated++;
                        else
                            result.Skipped++;
                    }
                    else
                    {
                        // INSERT new record
                        InsertRecord(sqlCon, tableName, primaryKeyColumn, row, columns);
                        result.Inserted++;
                    }
                }
                
                // ─────────────────────────────────────────────────────────
                // STEP 5: SET IDENTITY_INSERT OFF
                // ─────────────────────────────────────────────────────────
                
                SqlCommand setIdentityOff = new SqlCommand(
                    $"SET IDENTITY_INSERT {tableName} OFF", sqlCon);
                setIdentityOff.ExecuteNonQuery();
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error migrating table '{tableName}':\n{ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // HELPER METHODS
        // ═══════════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Table ke non-identity columns fetch karta hai
        /// </summary>
        private List<string> GetNonIdentityColumns(SqlConnection sqlCon, string tableName, string primaryKeyColumn)
        {
            List<string> columns = new List<string>();
            
            string query = $@"
                SELECT COLUMN_NAME 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = @tableName
                ORDER BY ORDINAL_POSITION";
            
            using (SqlCommand cmd = new SqlCommand(query, sqlCon))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string colName = reader.GetString(0);
                        columns.Add(colName);
                    }
                }
            }
            
            return columns;
        }

        /// <summary>
        /// Check karta hai ke record already exist karta hai ya nahi
        /// </summary>
        private bool RecordExists(SqlConnection sqlCon, string tableName, string primaryKeyColumn, object primaryKeyValue)
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {primaryKeyColumn} = @pkValue";
            
            using (SqlCommand cmd = new SqlCommand(query, sqlCon))
            {
                cmd.Parameters.AddWithValue("@pkValue", primaryKeyValue);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// Naya record insert karta hai
        /// </summary>
        private void InsertRecord(SqlConnection sqlCon, string tableName, string primaryKeyColumn, 
            DataRow row, List<string> columns)
        {
            // Column names aur values prepare karo
            var columnList = string.Join(", ", columns);
            var paramList = string.Join(", ", columns.Select(c => "@" + c));
            
            string insertQuery = $"INSERT INTO {tableName} ({columnList}) VALUES ({paramList})";
            
            using (SqlCommand cmd = new SqlCommand(insertQuery, sqlCon))
            {
                foreach (string col in columns)
                {
                    object value = row[col];
                    cmd.Parameters.AddWithValue("@" + col, value ?? DBNull.Value);
                }
                
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Existing record ko update karta hai
        /// Returns: true agar changes the, false agar same data tha
        /// </summary>
        private bool UpdateRecord(SqlConnection sqlCon, string tableName, string primaryKeyColumn, 
            object primaryKeyValue, DataRow row, List<string> columns)
        {
            // Primary key column ko exclude karo (update nahi karna)
            var updateColumns = columns.Where(c => c != primaryKeyColumn).ToList();
            
            if (updateColumns.Count == 0)
                return false;
            
            // Check: Koi changes hain ya nahi?
            if (!HasChanges(sqlCon, tableName, primaryKeyColumn, primaryKeyValue, row, updateColumns))
                return false;
            
            // SET clause prepare karo (Col1 = @Col1, Col2 = @Col2, ...)
            var setClause = string.Join(", ", updateColumns.Select(c => $"{c} = @{c}"));
            
            string updateQuery = $"UPDATE {tableName} SET {setClause} WHERE {primaryKeyColumn} = @pkValue";
            
            using (SqlCommand cmd = new SqlCommand(updateQuery, sqlCon))
            {
                foreach (string col in updateColumns)
                {
                    object value = row[col];
                    cmd.Parameters.AddWithValue("@" + col, value ?? DBNull.Value);
                }
                
                cmd.Parameters.AddWithValue("@pkValue", primaryKeyValue);
                cmd.ExecuteNonQuery();
            }
            
            return true;
        }

        /// <summary>
        /// Check karta hai ke new data aur existing data mein koi difference hai ya nahi
        /// </summary>
        private bool HasChanges(SqlConnection sqlCon, string tableName, string primaryKeyColumn, 
            object primaryKeyValue, DataRow newRow, List<string> columns)
        {
            // Existing data fetch karo
            string selectQuery = $"SELECT * FROM {tableName} WHERE {primaryKeyColumn} = @pkValue";
            
            using (SqlCommand cmd = new SqlCommand(selectQuery, sqlCon))
            {
                cmd.Parameters.AddWithValue("@pkValue", primaryKeyValue);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        foreach (string col in columns)
                        {
                            if (col == primaryKeyColumn) continue;
                            
                            object existingValue = reader[col];
                            object newValue = newRow[col];
                            
                            // NULL comparison
                            if (existingValue == DBNull.Value && newValue == DBNull.Value)
                                continue;
                            
                            if (existingValue == DBNull.Value || newValue == DBNull.Value)
                                return true; // Different
                            
                            // Value comparison
                            if (!existingValue.Equals(newValue))
                                return true; // Different
                        }
                    }
                }
            }
            
            return false; // No changes
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // MIGRATION RESULT CLASS
    // ═══════════════════════════════════════════════════════════════════════════════
    
    public class MigrationResult
    {
        public int Inserted { get; set; } = 0;
        public int Updated { get; set; } = 0;
        public int Skipped { get; set; } = 0;
        
        public int Total => Inserted + Updated + Skipped;
    }
}
