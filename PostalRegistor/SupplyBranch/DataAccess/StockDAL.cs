using SupplyBranch.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SupplyBranch.DAL
{
    public class StockDAL
    {
        DBHelper db = new DBHelper();

        public bool StockIn(
            int categoryID,
            int denominationID,
            int boxQty,
            int packetQty,
            int sheetQty,
            int stampQty,
            string remarks,
            int? createdBy = null)
        {
            return db.ExecuteTransaction((con, transaction) =>
            {
                // ==========================================
                // 1. Check existing StockMaster record
                // ==========================================

                string checkSql = @"
                    SELECT StockID
                    FROM StockMaster
                    WHERE CategoryID = @CategoryID
                      AND DenominationID = @DenominationID";

                int stockID = 0;

                using (SqlCommand cmd = new SqlCommand(
                    checkSql, con, transaction))
                {
                    cmd.Parameters.AddWithValue(
                        "@CategoryID", categoryID);

                    cmd.Parameters.AddWithValue(
                        "@DenominationID", denominationID);

                    object result = cmd.ExecuteScalar();

                    if (result != null &&
                        result != DBNull.Value)
                    {
                        stockID = Convert.ToInt32(result);
                    }
                }

                // ==========================================
                // 2. Create StockMaster if not exists
                // ==========================================

                if (stockID == 0)
                {
                    string insertStockSql = @"
                        INSERT INTO StockMaster
                        (
                            CategoryID,
                            DenominationID,
                            BoxQty,
                            PacketQty,
                            SheetQty,
                            StampQty,
                            ModifiedDate
                        )
                        VALUES
                        (
                            @CategoryID,
                            @DenominationID,
                            @BoxQty,
                            @PacketQty,
                            @SheetQty,
                            @StampQty,
                            GETDATE()
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using (SqlCommand cmd = new SqlCommand(
                        insertStockSql, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@CategoryID", categoryID);

                        cmd.Parameters.AddWithValue(
                            "@DenominationID", denominationID);

                        cmd.Parameters.AddWithValue(
                            "@BoxQty", boxQty);

                        cmd.Parameters.AddWithValue(
                            "@PacketQty", packetQty);

                        cmd.Parameters.AddWithValue(
                            "@SheetQty", sheetQty);

                        cmd.Parameters.AddWithValue(
                            "@StampQty", stampQty);

                        stockID = Convert.ToInt32(
                            cmd.ExecuteScalar());
                    }
                }
                else
                {
                    // ==========================================
                    // 3. Add IN quantities to existing balance
                    // ==========================================

                    string updateStockSql = @"
                        UPDATE StockMaster
                        SET
                            BoxQty = BoxQty + @BoxQty,
                            PacketQty = PacketQty + @PacketQty,
                            SheetQty = SheetQty + @SheetQty,
                            StampQty = StampQty + @StampQty,
                            ModifiedDate = GETDATE()
                        WHERE StockID = @StockID";

                    using (SqlCommand cmd = new SqlCommand(
                        updateStockSql, con, transaction))
                    {
                        cmd.Parameters.AddWithValue(
                            "@BoxQty", boxQty);

                        cmd.Parameters.AddWithValue(
                            "@PacketQty", packetQty);

                        cmd.Parameters.AddWithValue(
                            "@SheetQty", sheetQty);

                        cmd.Parameters.AddWithValue(
                            "@StampQty", stampQty);

                        cmd.Parameters.AddWithValue(
                            "@StockID", stockID);

                        cmd.ExecuteNonQuery();
                    }
                }

                // ==========================================
                // 4. Save StockTransaction history
                // ==========================================

                string transactionSql = @"
                    INSERT INTO StockTransaction
                    (
                        StockID,
                        TransactionType,
                        BoxQty,
                        PacketQty,
                        SheetQty,
                        StampQty,
                        TransactionDate,
                        ReferenceType,
                        ReferenceID,
                        Remarks,
                        CreatedBy
                    )
                    VALUES
                    (
                        @StockID,
                        'IN',
                        @BoxQty,
                        @PacketQty,
                        @SheetQty,
                        @StampQty,
                        GETDATE(),
                        'Stock IN',
                        NULL,
                        @Remarks,
                        @CreatedBy
                    )";

                using (SqlCommand cmd = new SqlCommand(
                    transactionSql, con, transaction))
                {
                    cmd.Parameters.AddWithValue(
                        "@StockID", stockID);

                    cmd.Parameters.AddWithValue(
                        "@BoxQty", boxQty);

                    cmd.Parameters.AddWithValue(
                        "@PacketQty", packetQty);

                    cmd.Parameters.AddWithValue(
                        "@SheetQty", sheetQty);

                    cmd.Parameters.AddWithValue(
                        "@StampQty", stampQty);

                    cmd.Parameters.AddWithValue(
                        "@Remarks",
                        string.IsNullOrWhiteSpace(remarks)
                            ? (object)DBNull.Value
                            : remarks.Trim());

                    cmd.Parameters.AddWithValue(
                        "@CreatedBy",
                        createdBy.HasValue
                            ? (object)createdBy.Value
                            : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            });
        }

        public DataTable GetStockInHistory()
        {
            string query = @"
        SELECT
            T.TransactionID,
            C.Name AS Category,
            'Rs.' +
            CAST(
                CAST(D.Denomination AS DECIMAL(18,2))
                AS VARCHAR(20)
            ) + '/-' AS Denomination,

            T.BoxQty,
            T.PacketQty,
            T.SheetQty,
            T.StampQty,
            T.TransactionDate,
            T.Remarks

        FROM StockTransaction T

        INNER JOIN StockMaster S
            ON T.StockID = S.StockID

        INNER JOIN StampCategory C
            ON S.CategoryID = C.CategoryID

        INNER JOIN Denomination D
            ON S.DenominationID = D.DenominationID

        WHERE T.TransactionType = 'IN'

        ORDER BY
            T.TransactionDate DESC,
            T.TransactionID DESC";

            return db.GetDataTable(query);
        }

        public DataTable GetStockBalance()
        {
            string query = @"
        SELECT
            S.StockID,
            C.Name AS Category,

            'Rs.' +
            CAST(
                CAST(D.Denomination AS DECIMAL(18,2))
                AS VARCHAR(20)
            ) + '/-' AS Denomination,

            S.BoxQty,
            S.PacketQty,
            S.SheetQty,
            S.StampQty,

            S.ModifiedDate

        FROM StockMaster S

        INNER JOIN StampCategory C
            ON S.CategoryID = C.CategoryID

        INNER JOIN Denomination D
            ON S.DenominationID = D.DenominationID

        ORDER BY
            C.Name,
            D.Denomination";

            return db.GetDataTable(query);
        }
    }
}