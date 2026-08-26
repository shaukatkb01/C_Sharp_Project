using SupplyBranch.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SupplyBranch.DAL
{
    public class StockDAL
    {
        DBHelper db = new DBHelper();

        public class UnitConversion
        {
            public int PacketsPerBox { get; set; }
            public int SheetsPerPacket { get; set; }
            public int PiecesPerSheet { get; set; }
        }
        public DataTable GetCategories()
        {
            string query = @"
        SELECT DISTINCT 
            C.CategoryID, 
            C.Name 
        FROM [dbo].[StampCategory] C
        INNER JOIN [dbo].[StockMaster] S ON C.CategoryID = S.CategoryID
        ORDER BY C.Name ASC;";

            return db.GetDataTable(query);
        }
        public DataTable GetDenominationsByCategory(int categoryID)
        {
            string query = @"
        SELECT DISTINCT 
            D.DenominationID, 
            'Rs. ' + CAST(CAST(D.Denomination AS INT) AS VARCHAR(20)) + '/-' AS Denomination 
        FROM [dbo].[Denomination] D
        INNER JOIN [dbo].[StockMaster] S ON D.DenominationID = S.DenominationID
        WHERE S.CategoryID = @CategoryID 
        ORDER BY Denomination ASC;";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", categoryID)
    };

            return db.ExecuteQuery(query, parameters);
        }

        public DataTable GetAdjustmentHistory()
        {
            string query = @"
        SELECT TOP 20
            A.AdjustmentID,
            C.Name AS Category,
            'Rs. ' + CAST(CAST(D.Denomination AS INT) AS VARCHAR(20)) + '/-' AS Denomination,
            A.AdjustType,
            A.BoxQty,
            A.PacketQty,
            A.SheetQty,
            A.StampQty,
            A.Reason,
            A.AdjustedDate
        FROM StockAdjustment A
        INNER JOIN StampCategory C ON A.CategoryID = C.CategoryID
        INNER JOIN Denomination D ON A.DenominationID = D.DenominationID
        ORDER BY A.AdjustmentID DESC;";

            return db.GetDataTable(query);
        }

        public bool AdjustStock(int categoryID, int denominationID, string adjustType, int boxQty, int packetQty, int sheetQty, int stampQty, string reason)
        {
            // 1. Current Stock Fetch Karein
            DataRow stock = GetStockMasterRow(categoryID, denominationID);
            if (stock == null) return false;

            int stockID = Convert.ToInt32(stock["StockID"]);
            int currentBox = Convert.ToInt32(stock["BoxQty"]);
            int currentPacket = Convert.ToInt32(stock["PacketQty"]);
            int currentSheet = Convert.ToInt32(stock["SheetQty"]);
            int currentStamp = Convert.ToInt32(stock["StampQty"]);

            // 2. Adjust Type ke mutabiq Plus ya Minus calculate karein
            if (adjustType.ToUpper() == "ADD")
            {
                currentBox += boxQty;
                currentPacket += packetQty;
                currentSheet += sheetQty;
                currentStamp += stampQty;
            }
            else if (adjustType.ToUpper() == "LESS")
            {
                currentBox = Math.Max(0, currentBox - boxQty);
                currentPacket = Math.Max(0, currentPacket - packetQty);
                currentSheet = Math.Max(0, currentSheet - sheetQty);
                currentStamp = Math.Max(0, currentStamp - stampQty);
            }

            // 3. StockMaster ko direct update karein
            string updateStockQuery = @"
        UPDATE StockMaster 
        SET BoxQty = @BoxQty, 
            PacketQty = @PacketQty, 
            SheetQty = @SheetQty, 
            StampQty = @StampQty, 
            ModifiedDate = GETDATE()
        WHERE StockID = @StockID;";

            SqlParameter[] stockParams =
            {
        new SqlParameter("@BoxQty", currentBox),
        new SqlParameter("@PacketQty", currentPacket),
        new SqlParameter("@SheetQty", currentSheet),
        new SqlParameter("@StampQty", currentStamp),
        new SqlParameter("@StockID", stockID)
    };

            db.ExecuteNonQuery(updateStockQuery, stockParams);

            // 4. StockAdjustment History/Audit Table mein insert karein
            string insertAdjustmentQuery = @"
        INSERT INTO StockAdjustment 
            (StockID, CategoryID, DenominationID, AdjustType, BoxQty, PacketQty, SheetQty, StampQty, Reason, AdjustedDate)
        VALUES 
            (@StockID, @CategoryID, @DenominationID, @AdjustType, @BoxQty, @PacketQty, @SheetQty, @StampQty, @Reason, GETDATE());";

            SqlParameter[] adjustParams =
            {
        new SqlParameter("@StockID", stockID),
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID),
        new SqlParameter("@AdjustType", adjustType),
        new SqlParameter("@BoxQty", boxQty),
        new SqlParameter("@PacketQty", packetQty),
        new SqlParameter("@SheetQty", sheetQty),
        new SqlParameter("@StampQty", stampQty),
        new SqlParameter("@Reason", string.IsNullOrWhiteSpace(reason) ? (object)DBNull.Value : reason)
    };

            return db.ExecuteNonQuery(insertAdjustmentQuery, adjustParams) > 0;
        }

        public void AutoConvertAndDeductStock(int categoryID, int denominationID, int reqBox, int reqPacket, int reqSheet, int reqStamp)
        {
            // 1. StockMaster se current stock hasil karein
            DataRow stock = GetStockMasterRow(categoryID, denominationID);
            if (stock == null) return;

            int stockID = Convert.ToInt32(stock["StockID"]);
            int box = Convert.ToInt32(stock["BoxQty"]);
            int packet = Convert.ToInt32(stock["PacketQty"]);
            int sheet = Convert.ToInt32(stock["SheetQty"]);
            int stamp = Convert.ToInt32(stock["StampQty"]);

            // 2. UnitConversionMaster se conversion rates hasil karein
            UnitConversion conv = GetUnitConversion(categoryID, denominationID);
            if (conv == null) return;

            int packetsPerBox = conv.PacketsPerBox;
            int sheetsPerPacket = conv.SheetsPerPacket;
            int piecesPerSheet = conv.PiecesPerSheet;

            // --- STEP A: STAMPS CONVERSION ---
            // Agar required stamps mojood stamps se zyada hain
            while (stamp < reqStamp)
            {
                if (sheet > 0)
                {
                    sheet -= 1;
                    stamp += piecesPerSheet;
                }
                else if (packet > 0)
                {
                    packet -= 1;
                    sheet += sheetsPerPacket;
                }
                else if (box > 0)
                {
                    box -= 1;
                    packet += packetsPerBox;
                }
                else
                {
                    break; // Mazeed stock nahi bacha torne ke liye
                }
            }
            stamp -= reqStamp; // Required stamps minus kar dein

            // --- STEP B: SHEETS CONVERSION ---
            while (sheet < reqSheet)
            {
                if (packet > 0)
                {
                    packet -= 1;
                    sheet += sheetsPerPacket;
                }
                else if (box > 0)
                {
                    box -= 1;
                    packet += packetsPerBox;
                }
                else
                {
                    break;
                }
            }
            sheet -= reqSheet; // Required sheets minus kar dein

            // --- STEP C: PACKETS CONVERSION ---
            while (packet < reqPacket)
            {
                if (box > 0)
                {
                    box -= 1;
                    packet += packetsPerBox;
                }
                else
                {
                    break;
                }
            }
            packet -= reqPacket; // Required packets minus kar dein

            // --- STEP D: BOXES DEDUCTION ---
            box -= reqBox; // Required boxes minus kar dein

            // 3. Updated stock ko wapis `StockMaster` mein save/update kar dein
            string updateQuery = @"
        UPDATE StockMaster 
        SET BoxQty = @BoxQty, 
            PacketQty = @PacketQty, 
            SheetQty = @SheetQty, 
            StampQty = @StampQty, 
            ModifiedDate = GETDATE()
        WHERE StockID = @StockID;";

            SqlParameter[] param =
            {
        new SqlParameter("@BoxQty", box),
        new SqlParameter("@PacketQty", packet),
        new SqlParameter("@SheetQty", sheet),
        new SqlParameter("@StampQty", stamp),
        new SqlParameter("@StockID", stockID)
    };

            db.ExecuteNonQuery(updateQuery, param);
        }
        public DataRow GetStockMasterRow(int categoryID, int denominationID)
        {
            string query = @"SELECT 
                        StockID, 
                        CategoryID, 
                        DenominationID, 
                        BoxQty, 
                        PacketQty, 
                        SheetQty, 
                        StampQty 
                     FROM StockMaster 
                     WHERE CategoryID = @CategoryID 
                       AND DenominationID = @DenominationID;";

            SqlParameter[] param =
            {
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID)
    };

            DataTable dt = db.GetDataTable(query, param);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }

            return null;
        }
        public void AutoConvertStockMaster(int categoryID, int denominationID, int reqBox, int reqPacket, int reqSheet, int reqStamp)
        {
            // 1. StockMaster se current stock hasil karein
            DataRow stock = GetStockMasterRow(categoryID, denominationID);
            if (stock == null) return;

            int stockID = Convert.ToInt32(stock["StockID"]);
            int currentBox = Convert.ToInt32(stock["BoxQty"]);
            int currentPacket = Convert.ToInt32(stock["PacketQty"]);
            int currentSheet = Convert.ToInt32(stock["SheetQty"]);
            int currentStamp = Convert.ToInt32(stock["StampQty"]);

            // 2. UnitConversionMaster se rates hasil karein
            UnitConversion conv = GetUnitConversion(categoryID, denominationID);
            if (conv == null) return;

            bool isModified = false;

            // A. Sheet to Stamp Conversion Check
            if (currentStamp < reqStamp && currentSheet > 0)
            {
                currentSheet -= 1;
                currentStamp += conv.PiecesPerSheet;
                isModified = true;
            }

            // B. Packet to Sheet Conversion Check
            if (currentSheet < reqSheet && currentPacket > 0)
            {
                currentPacket -= 1;
                currentSheet += conv.SheetsPerPacket;
                isModified = true;
            }

            // C. Box to Packet Conversion Check
            if (currentPacket < reqPacket && currentBox > 0)
            {
                currentBox -= 1;
                currentPacket += conv.PacketsPerBox;
                isModified = true;
            }

            // 3. Agar koi conversion hui hai to StockMaster update karein
            if (isModified)
            {
                string updateQuery = @"
            UPDATE StockMaster 
            SET BoxQty = @BoxQty, 
                PacketQty = @PacketQty, 
                SheetQty = @SheetQty, 
                StampQty = @StampQty, 
                ModifiedDate = GETDATE()
            WHERE StockID = @StockID;";

                SqlParameter[] param =
                {
            new SqlParameter("@BoxQty", currentBox),
            new SqlParameter("@PacketQty", currentPacket),
            new SqlParameter("@SheetQty", currentSheet),
            new SqlParameter("@StampQty", currentStamp),
            new SqlParameter("@StockID", stockID)
        };

                db.ExecuteNonQuery(updateQuery, param);
            }
        }
        public UnitConversion GetUnitConversion(int categoryID, int denominationID)
        {
            string query = @"SELECT PacketsPerBox, SheetsPerPacket, PiecesPerSheet 
                    FROM UnitConversionMaster 
                    WHERE CategoryID = @CategoryID AND DenominationID = @DenominationID;";

            SqlParameter[] param =
            {
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID)
    };

            DataTable dt = db.GetDataTable(query, param);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new UnitConversion
                {
                    PacketsPerBox = Convert.ToInt32(row["PacketsPerBox"]),
                    SheetsPerPacket = Convert.ToInt32(row["SheetsPerPacket"]),
                    PiecesPerSheet = Convert.ToInt32(row["PiecesPerSheet"])
                };
            }

            return null;
        }

        private bool AdjustAndValidateStock(
    ref int reqBox, ref int reqPacket, ref int reqSheet, ref int reqStamp,
    int availBox, int availPacket, int availSheet, int availStamp,
    UnitConversion conv)
        {
            // Total stamps per unit calculate karein
            int stampsPerSheet = conv.PiecesPerSheet;
            int stampsPerPacket = conv.SheetsPerPacket * stampsPerSheet;
            int stampsPerBox = conv.PacketsPerBox * stampsPerPacket;

            // Direct Total Stamps calculate karein
            int totalAvailableStamps = (availBox * stampsPerBox) + (availPacket * stampsPerPacket) +
                                       (availSheet * stampsPerSheet) + availStamp;

            int totalRequiredStamps = (reqBox * stampsPerBox) + (reqPacket * stampsPerPacket) +
                                      (reqSheet * stampsPerSheet) + reqStamp;

            // Validation: Agar required total stock se zyada ho
            if (totalRequiredStamps > totalAvailableStamps)
            {
                return false; // Stock insufficient hai
            }

            return true;
        }
        public DataTable GetCurrentStockPosition()
        {
            DataTable dt = new DataTable();

            // Direct StockMaster se real-time balance fetch karne ke liye SQL Query
            string query = @"
        SELECT 
            C.Name AS CategoryName,
            'Rs. ' + FORMAT(CAST(D.Denomination AS INT), '#,##0') + '/-' AS DenominationValue,
            ISNULL(S.BoxQty, 0) AS BalanceBoxQty,
            ISNULL(S.PacketQty, 0) AS BalancePacketQty,
            ISNULL(S.SheetQty, 0) AS BalanceSheetQty,
            ISNULL(S.StampQty, 0) AS BalanceStampQty
        FROM [dbo].[StockMaster] S
        INNER JOIN [dbo].[StampCategory] C ON S.CategoryID = C.CategoryID
        INNER JOIN [dbo].[Denomination] D ON S.DenominationID = D.DenominationID
        ORDER BY C.Name, D.Denomination;";

            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Stock balance data fetch karne mein masla aaya: " + ex.Message);
            }

            return dt;
        }
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
    StockID,
    CategoryID,
    DenominationID,
    ISNULL(BoxQty, 0) AS BoxQty,
    ISNULL(PacketQty, 0) AS PacketQty,
    ISNULL(SheetQty, 0) AS SheetQty,
    ISNULL(StampQty, 0) AS StampQty
FROM [dbo].[StockMaster]
WHERE CategoryID = @CategoryID 
  AND DenominationID = @DenominationID;

     --   SELECT 
       --     sm.StockID,
         --   sm.CategoryID,
           -- sm.DenominationID,
            --ISNULL(sm.BoxQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.BoxQty ELSE 0 END), 0) AS BoxQty,
            --ISNULL(sm.PacketQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.PacketQty ELSE 0 END), 0) AS PacketQty,
            --ISNULL(sm.SheetQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.SheetQty ELSE 0 END), 0) AS SheetQty,
            --ISNULL(sm.StampQty, 0) - ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.StampQty ELSE 0 END), 0) AS StampQty
        --FROM [dbo].[StockMaster] sm
        --LEFT JOIN [dbo].[StockTransaction] st ON sm.StockID = st.StockID
        --WHERE sm.CategoryID = @CategoryID 
          --AND sm.DenominationID = @DenominationID
        --GROUP BY sm.StockID, sm.CategoryID, sm.DenominationID, sm.BoxQty, sm.PacketQty, sm.SheetQty, sm.StampQty";

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
            ISNULL(S.BoxQty, 0) AS BoxQty,
            ISNULL(S.PacketQty, 0) AS PacketQty,
            ISNULL(S.SheetQty, 0) AS SheetQty,
            ISNULL(S.StampQty, 0) AS StampQty,
            S.ModifiedDate 
        FROM StockMaster S 
        INNER JOIN StampCategory C ON S.CategoryID = C.CategoryID 
        INNER JOIN Denomination D ON S.DenominationID = D.DenominationID 
        ORDER BY 
            C.Name, 
            D.Denomination;";

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