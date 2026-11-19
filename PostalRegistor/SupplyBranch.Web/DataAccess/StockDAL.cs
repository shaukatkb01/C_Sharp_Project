using SupplyBranch.Helpers;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SupplyBranch.DataAccess
{
    public class StockDAL
    {
        private readonly DBHelper _db;

        public StockDAL(DBHelper db)
        {
            _db = db;
        }

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

            return _db.ExecuteQuery(query);
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

            SqlParameter[] parameters = { new SqlParameter("@CategoryID", categoryID) };

            return _db.ExecuteQuery(query, parameters);
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

            return _db.ExecuteQuery(query);
        }

        public bool AdjustStock(int categoryID, int denominationID, string adjustType, int boxQty, int packetQty, int sheetQty, int stampQty, string reason)
        {
            DataRow stock = GetStockMasterRow(categoryID, denominationID);
            if (stock == null) return false;

            int stockID = Convert.ToInt32(stock["StockID"]);
            int currentBox = Convert.ToInt32(stock["BoxQty"]);
            int currentPacket = Convert.ToInt32(stock["PacketQty"]);
            int currentSheet = Convert.ToInt32(stock["SheetQty"]);
            int currentStamp = Convert.ToInt32(stock["StampQty"]);

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

            _db.ExecuteNonQuery(updateStockQuery, stockParams);

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

            return _db.ExecuteNonQuery(insertAdjustmentQuery, adjustParams) > 0;
        }

        public void AutoConvertAndDeductStock(int categoryID, int denominationID, int reqBox, int reqPacket, int reqSheet, int reqStamp)
        {
            DataRow stock = GetStockMasterRow(categoryID, denominationID);
            if (stock == null) return;

            int stockID = Convert.ToInt32(stock["StockID"]);
            int box = Convert.ToInt32(stock["BoxQty"]);
            int packet = Convert.ToInt32(stock["PacketQty"]);
            int sheet = Convert.ToInt32(stock["SheetQty"]);
            int stamp = Convert.ToInt32(stock["StampQty"]);

            UnitConversion conv = GetUnitConversion(categoryID, denominationID);
            if (conv == null) return;

            int packetsPerBox = conv.PacketsPerBox;
            int sheetsPerPacket = conv.SheetsPerPacket;
            int piecesPerSheet = conv.PiecesPerSheet;

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
                    break;
                }
            }

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

            int remainingBox = Math.Max(0, box - reqBox);
            int remainingPacket = Math.Max(0, packet - reqPacket);
            int remainingSheet = Math.Max(0, sheet - reqSheet);
            int remainingStamp = Math.Max(0, stamp - reqStamp);

            string updateSql = @"
                UPDATE StockMaster
                SET BoxQty = @BoxQty, PacketQty = @PacketQty, SheetQty = @SheetQty, StampQty = @StampQty, ModifiedDate = GETDATE()
                WHERE StockID = @StockID";

            SqlParameter[] updateParams =
            {
                new SqlParameter("@BoxQty", remainingBox),
                new SqlParameter("@PacketQty", remainingPacket),
                new SqlParameter("@SheetQty", remainingSheet),
                new SqlParameter("@StampQty", remainingStamp),
                new SqlParameter("@StockID", stockID)
            };

            _db.ExecuteNonQuery(updateSql, updateParams);
        }

        public DataRow GetStockMasterRow(int categoryID, int denominationID)
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
  AND DenominationID = @DenominationID;";

            SqlParameter[] parameters =
            {
                new SqlParameter("@CategoryID", categoryID),
                new SqlParameter("@DenominationID", denominationID)
            };

            DataTable dt = _db.ExecuteQuery(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];

            return null;
        }

        public UnitConversion GetUnitConversion(int categoryID, int denominationID)
        {
            string query = @"
SELECT PacketsPerBox, SheetsPerPacket, PiecesPerSheet
FROM UnitConversionMaster
WHERE CategoryID = @CategoryID
  AND DenominationID = @DenominationID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@CategoryID", categoryID),
                new SqlParameter("@DenominationID", denominationID)
            };

            DataTable dt = _db.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0) return null;

            return new UnitConversion
            {
                PacketsPerBox = Convert.ToInt32(dt.Rows[0]["PacketsPerBox"]),
                SheetsPerPacket = Convert.ToInt32(dt.Rows[0]["SheetsPerPacket"]),
                PiecesPerSheet = Convert.ToInt32(dt.Rows[0]["PiecesPerSheet"])
            };
        }

        public bool DeleteStockTransactionsBySupplyID(int supplyID)
        {
            string query = "DELETE FROM StockTransaction WHERE SupplyID = @SupplyID";

            SqlParameter[] p = { new SqlParameter("@SupplyID", supplyID) };

            return _db.ExecuteNonQuery(query, p) >= 0;
        }

        public DataTable GetStockBalanceTable()
        {
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

            return _db.ExecuteQuery(query);
        }

        public bool StockIn(int categoryID, int denominationID, int boxQty, int packetQty, int sheetQty, int stampQty, string remarks, int? createdBy = null)
        {
            return _db.ExecuteTransaction((con, transaction) =>
            {
                string checkSql = @"SELECT StockID FROM StockMaster WHERE CategoryID = @CategoryID AND DenominationID = @DenominationID";

                int stockID = 0;

                using (SqlCommand cmd = new SqlCommand(checkSql, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@DenominationID", denominationID);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        stockID = Convert.ToInt32(result);
                }

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

                    using (SqlCommand cmd = new SqlCommand(insertStockSql, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                        cmd.Parameters.AddWithValue("@DenominationID", denominationID);
                        cmd.Parameters.AddWithValue("@BoxQty", boxQty);
                        cmd.Parameters.AddWithValue("@PacketQty", packetQty);
                        cmd.Parameters.AddWithValue("@SheetQty", sheetQty);
                        cmd.Parameters.AddWithValue("@StampQty", stampQty);

                        stockID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    string updateSql = @"UPDATE StockMaster SET BoxQty = ISNULL(BoxQty,0) + @BoxQty, PacketQty = ISNULL(PacketQty,0) + @PacketQty, SheetQty = ISNULL(SheetQty,0) + @SheetQty, StampQty = ISNULL(StampQty,0) + @StampQty, ModifiedDate = GETDATE() WHERE StockID = @StockID";

                    using (SqlCommand cmd = new SqlCommand(updateSql, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BoxQty", boxQty);
                        cmd.Parameters.AddWithValue("@PacketQty", packetQty);
                        cmd.Parameters.AddWithValue("@SheetQty", sheetQty);
                        cmd.Parameters.AddWithValue("@StampQty", stampQty);
                        cmd.Parameters.AddWithValue("@StockID", stockID);

                        cmd.ExecuteNonQuery();
                    }
                }

                string insertTrans = @"INSERT INTO StockTransaction (StockID, CategoryID, DenominationID, TransactionType, BoxQty, PacketQty, SheetQty, StampQty, Remarks, CreatedDate, CreatedBy) VALUES (@StockID, @CategoryID, @DenominationID, 'IN', @BoxQty, @PacketQty, @SheetQty, @StampQty, @Remarks, GETDATE(), @CreatedBy)";

                using (SqlCommand cmd = new SqlCommand(insertTrans, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@StockID", stockID);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@DenominationID", denominationID);
                    cmd.Parameters.AddWithValue("@BoxQty", boxQty);
                    cmd.Parameters.AddWithValue("@PacketQty", packetQty);
                    cmd.Parameters.AddWithValue("@SheetQty", sheetQty);
                    cmd.Parameters.AddWithValue("@StampQty", stampQty);
                    cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", createdBy ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                return true;
            });
        }
    }
}
