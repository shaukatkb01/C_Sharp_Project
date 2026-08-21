using SupplyBranch.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SupplyBranch.DAL
{
    public class StockDAL
    {
        DBHelper db = new DBHelper();

        public bool DeleteStockTransactionsBySupplyID(int supplyID)
        {
            string query = "DELETE FROM StockTransaction WHERE SupplyID = @SupplyID";

            SqlParameter[] p =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            return db.ExecuteNonQuery(query, p) >= 0;
        }

        public DataRow GetStockBalance(int categoryID, int denominationID)
        {
            string query = @"
        SELECT 
            sm.StockID,
            sm.CategoryID,
            sm.DenominationID,
            ISNULL(sm.BoxQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.BoxQty ELSE 0 END), 0) AS BoxQty,
            ISNULL(sm.PacketQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.PacketQty ELSE 0 END), 0) AS PacketQty,
            ISNULL(sm.SheetQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.SheetQty ELSE 0 END), 0) AS SheetQty,
            ISNULL(sm.StampQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.StampQty ELSE 0 END), 0) AS StampQty
        FROM [dbo].[StockMaster] sm
        LEFT JOIN [dbo].[StockTransaction] st ON sm.StockID = st.StockID
        WHERE sm.CategoryID = @CategoryID 
          AND sm.DenominationID = @DenominationID
        GROUP BY sm.StockID, sm.CategoryID, sm.DenominationID, sm.BoxQty, sm.PacketQty, sm.SheetQty, sm.StampQty";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID)
    };

            DataTable dt = db.ExecuteQuery(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];

            return null;
        }

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
            'Rs.' + CAST(CAST(D.Denomination AS INT) AS VARCHAR(20)) + '/-' AS Denomination, 
            ISNULL(S.BoxQty, 0) - ISNULL(SUM(CASE WHEN ST.TransactionType = 'OUT' THEN ST.BoxQty ELSE 0 END), 0) AS BoxQty,
            ISNULL(S.PacketQty, 0) - ISNULL(SUM(CASE WHEN ST.TransactionType = 'OUT' THEN ST.PacketQty ELSE 0 END), 0) AS PacketQty,
            ISNULL(S.SheetQty, 0) - ISNULL(SUM(CASE WHEN ST.TransactionType = 'OUT' THEN ST.SheetQty ELSE 0 END), 0) AS SheetQty,
            ISNULL(S.StampQty, 0) - ISNULL(SUM(CASE WHEN ST.TransactionType = 'OUT' THEN ST.StampQty ELSE 0 END), 0) AS StampQty,
            S.ModifiedDate 
        FROM StockMaster S 
        INNER JOIN StampCategory C ON S.CategoryID = C.CategoryID 
        INNER JOIN Denomination D ON S.DenominationID = D.DenominationID 
        LEFT JOIN StockTransaction ST ON S.StockID = ST.StockID
        GROUP BY 
            S.StockID, 
            C.Name, 
            D.Denomination, 
            S.BoxQty, 
            S.PacketQty, 
            S.SheetQty, 
            S.StampQty, 
            S.ModifiedDate 
        ORDER BY 
            C.Name, 
            D.Denomination";

            return db.GetDataTable(query);
        }

        public int GetStockID(int categoryID, int denominationID)
        {
            string query = @"
SELECT StockID
FROM StockMaster
WHERE CategoryID = @CategoryID
  AND DenominationID = @DenominationID";

            SqlParameter[] param =
            {
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID)
    };

            object result = db.ExecuteScalar(query, param);

            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }

        public void InsertStockTransactionOut(
    int stockID,
    int boxQty,
    int packetQty,
    int sheetQty,
    int stampQty,
    int supplyID,
    string remarks)
        {
            string query = @"
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
    SupplyID
)
VALUES
(
    @StockID,
    'OUT',
    @BoxQty,
    @PacketQty,
    @SheetQty,
    @StampQty,
    GETDATE(),
    'Supply',
    @SupplyID,
    @Remarks,
    @SupplyID
);";

            SqlParameter[] param =
            {
        new SqlParameter("@StockID", stockID),
        new SqlParameter("@BoxQty", boxQty),
        new SqlParameter("@PacketQty", packetQty),
        new SqlParameter("@SheetQty", sheetQty),
        new SqlParameter("@StampQty", stampQty),
        new SqlParameter("@SupplyID", supplyID),
        new SqlParameter("@Remarks", remarks ?? "")
    };

            db.ExecuteNonQuery(query, param);
        }

        public void UpdateStockTransactionOut(
    int stockID,
    int boxQty,
    int packetQty,
    int sheetQty,
    int stampQty,
    int supplyID,
    string remarks)
        {
            string query = @"
        UPDATE StockTransaction
        SET 
            BoxQty = @BoxQty,
            PacketQty = @PacketQty,
            SheetQty = @SheetQty,
            StampQty = @StampQty,
            Remarks = @Remarks,
            TransactionDate = GETDATE()
        WHERE SupplyID = @SupplyID 
          AND StockID = @StockID 
          AND TransactionType = 'OUT';";

            SqlParameter[] param =
            {
        new SqlParameter("@StockID", stockID),
        new SqlParameter("@BoxQty", boxQty),
        new SqlParameter("@PacketQty", packetQty),
        new SqlParameter("@SheetQty", sheetQty),
        new SqlParameter("@StampQty", stampQty),
        new SqlParameter("@SupplyID", supplyID),
        new SqlParameter("@Remarks", (object)remarks ?? DBNull.Value)
    };

            db.ExecuteNonQuery(query, param);
        }


        public DataTable GetStockTransactionsBySupplyID(int supplyID)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT 
                      sm.CategoryID AS CategoryID,
                        sm.DenominationID AS DenominationID,
                        c.Name AS Category,
                        d.Denomination AS Denomination,
                        st.BoxQty AS BoxQty,
                        st.PacketQty AS PacketQty,
                        st.SheetQty AS SheetQty,
                        st.StampQty AS StampQty
                     FROM [dbo].[StockTransaction] st
                     INNER JOIN [dbo].[StockMaster] sm ON st.StockID = sm.StockID
                     LEFT JOIN [dbo].[StampCategory] c ON sm.CategoryID = c.CategoryID
                     LEFT JOIN [dbo].[Denomination] d ON sm.DenominationID = d.DenominationID
                     WHERE st.SupplyID = @SupplyID";

            using (SqlConnection conn = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplyID", supplyID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

    }
}