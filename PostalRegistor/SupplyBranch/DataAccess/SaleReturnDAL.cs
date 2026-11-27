using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SupplyBranch.DAL;

namespace SupplyBranch.DAL
{
    public class SaleReturnLine
    {
        public int SupplyDetailID { get; set; }
        public int CategoryID { get; set; }
        public int DenominationID { get; set; }
        public int PiecesPerSheet { get; set; }
        public int ReturnSheets { get; set; }
        public int ReturnPieces { get; set; }
        public int TotalPieces { get; set; }
        public string Remarks { get; set; }
    }

    public class SaleReturnStockLine
    {
        public int CategoryID { get; set; }
        public int DenominationID { get; set; }
        public int BoxQty { get; set; }
        public int PacketQty { get; set; }
        public int SheetQty { get; set; }
        public int StampQty { get; set; }
    }

    public class SaleReturnDAL
    {
        private readonly DBHelper db = new DBHelper();

        private static int TxExec(SqlConnection con, SqlTransaction tx, string sql, params SqlParameter[] p)
        {
            using (SqlCommand cmd = new SqlCommand(sql, con, tx))
            {
                if (p != null && p.Length > 0) cmd.Parameters.AddRange(p);
                return cmd.ExecuteNonQuery();
            }
        }

        private static int TxInt(object v)
        {
            return (v == null || v == DBNull.Value) ? 0 : Convert.ToInt32(v);
        }

        public (int SequenceNo, string ReturnNo) GetNextReturnNumber(string financialYear)
        {
            string sql = @"
                SELECT ISNULL(MAX(SequenceNo), 0)
                FROM SaleReturnMaster
                WHERE FinancialYear = @FinancialYear;";

            SqlParameter[] p = { new SqlParameter("@FinancialYear", financialYear) };
            int nextSeq = Convert.ToInt32(db.ExecuteScalar(sql, p)) + 1;
            string returnNo = $"SR{nextSeq:0000}/{financialYear}";
            return (nextSeq, returnNo);
        }

        public int GetAlreadyReturnedPieces(int supplyDetailID, int excludeReturnID = 0)
        {
            string sql = @"
                SELECT ISNULL(SUM(D.TotalPieces), 0)
                FROM SaleReturnDetail D
                INNER JOIN SaleReturnMaster M ON D.ReturnID = M.ReturnID
                WHERE D.SupplyDetailID = @SupplyDetailID
                  AND M.StatusID = 3
                  AND M.ReturnID <> @ExcludeReturnID;";

            SqlParameter[] p =
            {
                new SqlParameter("@SupplyDetailID", supplyDetailID),
                new SqlParameter("@ExcludeReturnID", excludeReturnID)
            };

            return Convert.ToInt32(db.ExecuteScalar(sql, p));
        }

        public int SaveDraft(
            int returnID,
            string financialYear,
            DateTime returnDate,
            int supplyID,
            string remarks,
            List<SaleReturnLine> lines,
            List<SaleReturnStockLine> stockLines)
        {
            int savedID = returnID;
            string error = null;

            bool ok = db.ExecuteTransaction((con, tx) =>
            {
                try
                {
                    if (savedID <= 0)
                    {
                        int nextSeq;
                        using (SqlCommand seqCmd = new SqlCommand(
                            "SELECT ISNULL(MAX(SequenceNo), 0) + 1 FROM SaleReturnMaster WITH (UPDLOCK, HOLDLOCK) WHERE FinancialYear = @FY",
                            con, tx))
                        {
                            seqCmd.Parameters.AddWithValue("@FY", financialYear);
                            nextSeq = TxInt(seqCmd.ExecuteScalar());
                        }

                        string returnNo = $"SR{nextSeq:0000}/{financialYear}";

                        string insertMaster = @"
                            INSERT INTO SaleReturnMaster
                                (ReturnNo, FinancialYear, SequenceNo, ReturnDate, SupplyID, StatusID, Remarks, CreatedDate)
                            VALUES
                                (@ReturnNo, @FinancialYear, @SequenceNo, @ReturnDate, @SupplyID, 1, @Remarks, GETDATE());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using (SqlCommand cmd = new SqlCommand(insertMaster, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@ReturnNo", returnNo);
                            cmd.Parameters.AddWithValue("@FinancialYear", financialYear);
                            cmd.Parameters.AddWithValue("@SequenceNo", nextSeq);
                            cmd.Parameters.AddWithValue("@ReturnDate", returnDate.Date);
                            cmd.Parameters.AddWithValue("@SupplyID", supplyID);
                            cmd.Parameters.AddWithValue("@Remarks", remarks ?? "");
                            savedID = TxInt(cmd.ExecuteScalar());
                        }
                    }
                    else
                    {
                        int rows = TxExec(con, tx, @"
                            UPDATE SaleReturnMaster
                            SET ReturnDate = @ReturnDate,
                                SupplyID   = @SupplyID,
                                Remarks    = @Remarks
                            WHERE ReturnID = @ReturnID
                              AND StatusID = 1;",
                            new SqlParameter("@ReturnDate", returnDate.Date),
                            new SqlParameter("@SupplyID", supplyID),
                            new SqlParameter("@Remarks", remarks ?? ""),
                            new SqlParameter("@ReturnID", savedID));

                        if (rows != 1)
                            throw new InvalidOperationException("Only Draft returns can be updated.");
                    }

                    WriteReturnChildren(con, tx, savedID, lines, stockLines);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    throw;
                }
            });

            if (error != null) throw new InvalidOperationException(error);
            if (!ok || savedID <= 0) throw new InvalidOperationException("Sale return draft could not be saved.");

            return savedID;
        }

        private static void WriteReturnChildren(
            SqlConnection con,
            SqlTransaction tx,
            int returnID,
            List<SaleReturnLine> lines,
            List<SaleReturnStockLine> stockLines)
        {
            TxExec(con, tx, "DELETE FROM SaleReturnDetail WHERE ReturnID = @ReturnID",
                new SqlParameter("@ReturnID", returnID));

            string insertDetail = @"
                INSERT INTO SaleReturnDetail
                    (ReturnID, SupplyDetailID, CategoryID, DenominationID, PiecesPerSheet, ReturnSheets, ReturnPieces, TotalPieces, Remarks)
                VALUES
                    (@ReturnID, @SupplyDetailID, @CategoryID, @DenominationID, @PiecesPerSheet, @ReturnSheets, @ReturnPieces, @TotalPieces, @Remarks);";

            foreach (SaleReturnLine l in lines)
            {
                if (l.TotalPieces <= 0) continue;

                TxExec(con, tx, insertDetail,
                    new SqlParameter("@ReturnID", returnID),
                    new SqlParameter("@SupplyDetailID", l.SupplyDetailID),
                    new SqlParameter("@CategoryID", l.CategoryID),
                    new SqlParameter("@DenominationID", l.DenominationID),
                    new SqlParameter("@PiecesPerSheet", l.PiecesPerSheet),
                    new SqlParameter("@ReturnSheets", l.ReturnSheets),
                    new SqlParameter("@ReturnPieces", l.ReturnPieces),
                    new SqlParameter("@TotalPieces", l.TotalPieces),
                    new SqlParameter("@Remarks", l.Remarks ?? ""));
            }

            TxExec(con, tx, "DELETE FROM SaleReturnStock WHERE ReturnID = @ReturnID",
                new SqlParameter("@ReturnID", returnID));

            string insertStock = @"
                INSERT INTO SaleReturnStock
                    (ReturnID, CategoryID, DenominationID, BoxQty, PacketQty, SheetQty, StampQty)
                VALUES
                    (@ReturnID, @CategoryID, @DenominationID, @BoxQty, @PacketQty, @SheetQty, @StampQty);";

            foreach (SaleReturnStockLine s in stockLines)
            {
                if (s.BoxQty == 0 && s.PacketQty == 0 && s.SheetQty == 0 && s.StampQty == 0)
                    continue;

                TxExec(con, tx, insertStock,
                    new SqlParameter("@ReturnID", returnID),
                    new SqlParameter("@CategoryID", s.CategoryID),
                    new SqlParameter("@DenominationID", s.DenominationID),
                    new SqlParameter("@BoxQty", s.BoxQty),
                    new SqlParameter("@PacketQty", s.PacketQty),
                    new SqlParameter("@SheetQty", s.SheetQty),
                    new SqlParameter("@StampQty", s.StampQty));
            }
        }

        public int PostReturnWithStockIn(
            int returnID,
            string financialYear,
            DateTime returnDate,
            int supplyID,
            string remarks,
            List<SaleReturnLine> lines,
            List<SaleReturnStockLine> stockLines)
        {
            bool hasStock = false;
            if (stockLines != null)
            {
                foreach (SaleReturnStockLine s in stockLines)
                {
                    if (s.BoxQty != 0 || s.PacketQty != 0 || s.SheetQty != 0 || s.StampQty != 0)
                    {
                        hasStock = true;
                        break;
                    }
                }
            }

            if (!hasStock)
                throw new InvalidOperationException(
                    "This sale return has no stock quantities to add back. Nothing was posted.");

            // First ensure latest draft values exist
            int targetID = SaveDraft(returnID, financialYear, returnDate, supplyID, remarks, lines, stockLines);
            string error = null;

            bool ok = db.ExecuteTransaction((con, tx) =>
            {
                try
                {
                    // Re-verify returnable limits inside transaction
                    foreach (SaleReturnLine l in lines)
                    {
                        if (l.TotalPieces <= 0) continue;

                        int issuedPieces = 0;
                        int alreadyReturned = 0;

                        using (SqlCommand chk = new SqlCommand(@"
                            SELECT
                                ISNULL((SELECT TotalPieces FROM SupplyDetail WHERE SupplyDetailID = @SDID), 0) AS IssuedPieces,
                                ISNULL((SELECT SUM(D.TotalPieces)
                                        FROM SaleReturnDetail D
                                        INNER JOIN SaleReturnMaster M ON D.ReturnID = M.ReturnID
                                        WHERE D.SupplyDetailID = @SDID AND M.StatusID = 3), 0) AS AlreadyReturned;",
                            con, tx))
                        {
                            chk.Parameters.AddWithValue("@SDID", l.SupplyDetailID);
                            using (SqlDataReader r = chk.ExecuteReader())
                            {
                                if (r.Read())
                                {
                                    issuedPieces = TxInt(r["IssuedPieces"]);
                                    alreadyReturned = TxInt(r["AlreadyReturned"]);
                                }
                            }
                        }

                        if (alreadyReturned + l.TotalPieces > issuedPieces)
                            throw new InvalidOperationException("Return quantity exceeds remaining issued balance.");
                    }

                    int rows = TxExec(con, tx, @"
                        UPDATE SaleReturnMaster
                        SET StatusID   = 3,
                            PostedDate = GETDATE()
                        WHERE ReturnID = @ReturnID
                          AND StatusID = 1;",
                        new SqlParameter("@ReturnID", targetID));

                    if (rows != 1)
                        throw new InvalidOperationException("Only a Draft sale return can be posted.");

                    string returnNo = "";
                    using (SqlCommand noCmd = new SqlCommand(
                        "SELECT ReturnNo FROM SaleReturnMaster WHERE ReturnID = @ReturnID", con, tx))
                    {
                        noCmd.Parameters.AddWithValue("@ReturnID", targetID);
                        returnNo = Convert.ToString(noCmd.ExecuteScalar());
                    }

                    foreach (SaleReturnStockLine s in stockLines)
                    {
                        if (s.BoxQty == 0 && s.PacketQty == 0 && s.SheetQty == 0 && s.StampQty == 0)
                            continue;

                        int stockID = 0;
                        using (SqlCommand findStock = new SqlCommand(@"
                            SELECT StockID
                            FROM StockMaster WITH (UPDLOCK, ROWLOCK)
                            WHERE CategoryID = @CategoryID AND DenominationID = @DenominationID;",
                            con, tx))
                        {
                            findStock.Parameters.AddWithValue("@CategoryID", s.CategoryID);
                            findStock.Parameters.AddWithValue("@DenominationID", s.DenominationID);
                            stockID = TxInt(findStock.ExecuteScalar());
                        }

                        if (stockID <= 0)
                            throw new InvalidOperationException(
                                $"StockMaster record not found for Category {s.CategoryID} / Denomination {s.DenominationID}.");

                        TxExec(con, tx, @"
                            UPDATE StockMaster
                            SET BoxQty       = BoxQty + @BoxQty,
                                PacketQty    = PacketQty + @PacketQty,
                                SheetQty     = SheetQty + @SheetQty,
                                StampQty     = StampQty + @StampQty,
                                ModifiedDate = GETDATE()
                            WHERE StockID = @StockID;",
                            new SqlParameter("@BoxQty", s.BoxQty),
                            new SqlParameter("@PacketQty", s.PacketQty),
                            new SqlParameter("@SheetQty", s.SheetQty),
                            new SqlParameter("@StampQty", s.StampQty),
                            new SqlParameter("@StockID", stockID));

                        string txRemarks = string.IsNullOrWhiteSpace(remarks)
                            ? $"Sale Return {returnNo}"
                            : $"Sale Return {returnNo} - {remarks.Trim()}";

                        TxExec(con, tx, @"
                            INSERT INTO StockTransaction
                            (
                                StockID, TransactionType, BoxQty, PacketQty, SheetQty, StampQty,
                                TransactionDate, ReferenceType, ReferenceID, Remarks, SupplyID
                            )
                            VALUES
                            (
                                @StockID, 'IN', @BoxQty, @PacketQty, @SheetQty, @StampQty,
                                GETDATE(), 'SaleReturn', @ReturnID, @Remarks, @SupplyID
                            );",
                            new SqlParameter("@StockID", stockID),
                            new SqlParameter("@BoxQty", s.BoxQty),
                            new SqlParameter("@PacketQty", s.PacketQty),
                            new SqlParameter("@SheetQty", s.SheetQty),
                            new SqlParameter("@StampQty", s.StampQty),
                            new SqlParameter("@ReturnID", targetID),
                            new SqlParameter("@Remarks", txRemarks),
                            new SqlParameter("@SupplyID", supplyID));
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    throw;
                }
            });

            if (error != null) throw new InvalidOperationException(error);
            if (!ok) throw new InvalidOperationException("Sale return posting failed. No stock changes were made.");

            return targetID;
        }

        public DataTable GetSaleReturns(string returnNo, string statusFilter, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                    M.ReturnID,
                    M.ReturnNo,
                    M.ReturnDate,
                    M.FinancialYear,
                    M.SupplyID,
                    SM.SupplyNo,
                    ISNULL(SM.InvoiceNo, '') AS InvoiceNo,
                    ISNULL(O.OfficeName, '') AS OfficeName,
                    ISNULL((SELECT SUM(D.ReturnSheets) FROM SaleReturnDetail D WHERE D.ReturnID = M.ReturnID), 0) AS TotalSheets,
                    ISNULL((SELECT SUM(D.TotalPieces)  FROM SaleReturnDetail D WHERE D.ReturnID = M.ReturnID), 0) AS TotalPieces,
                    CASE WHEN M.StatusID = 3 THEN 'Posted' ELSE 'Draft' END AS StatusName,
                    M.StatusID,
                    ISNULL(M.Remarks, '') AS Remarks
                FROM SaleReturnMaster M
                INNER JOIN SupplyMaster SM ON M.SupplyID = SM.SupplyID
                INNER JOIN IndentMaster IM ON SM.IndentID = IM.IndentID
                INNER JOIN Office O        ON IM.OfficeID = O.OfficeID
                WHERE M.ReturnDate >= @FromDate
                  AND M.ReturnDate <  @ToDate");

            List<SqlParameter> p = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1))
            };

            if (!string.IsNullOrWhiteSpace(returnNo))
            {
                sql.Append(" AND M.ReturnNo LIKE @ReturnNo");
                p.Add(new SqlParameter("@ReturnNo", "%" + returnNo.Trim() + "%"));
            }

            if (string.Equals(statusFilter, "Draft", StringComparison.OrdinalIgnoreCase))
                sql.Append(" AND M.StatusID = 1");
            else if (string.Equals(statusFilter, "Posted", StringComparison.OrdinalIgnoreCase))
                sql.Append(" AND M.StatusID = 3");

            sql.Append(" ORDER BY M.ReturnDate DESC, M.ReturnID DESC;");

            return db.ExecuteQuery(sql.ToString(), p.ToArray());
        }

        public DataTable GetSaleReturnHeader(int returnID)
        {
            string sql = @"
                SELECT
                    M.ReturnID,
                    M.ReturnNo,
                    M.ReturnDate,
                    M.FinancialYear,
                    M.SupplyID,
                    M.StatusID,
                    CASE WHEN M.StatusID = 3 THEN 'Posted' ELSE 'Draft' END AS StatusName,
                    ISNULL(M.Remarks, '') AS Remarks,
                    SM.SupplyNo,
                    SM.SupplyDate,
                    ISNULL(SM.InvoiceNo, '') AS InvoiceNo,
                    ISNULL(O.OfficeName, '') AS OfficeName
                FROM SaleReturnMaster M
                INNER JOIN SupplyMaster SM ON M.SupplyID = SM.SupplyID
                INNER JOIN IndentMaster IM ON SM.IndentID = IM.IndentID
                INNER JOIN Office O        ON IM.OfficeID = O.OfficeID
                WHERE M.ReturnID = @ReturnID;";

            return db.ExecuteQuery(sql, new[] { new SqlParameter("@ReturnID", returnID) });
        }

        public DataTable GetSaleReturnDetails(int returnID)
        {
            string sql = @"
                SELECT
                    D.ReturnDetailID,
                    D.SupplyDetailID,
                    D.CategoryID,
                    D.DenominationID,
                    SC.Name AS CategoryName,
                    DN.Denomination,
                    D.PiecesPerSheet,
                    SD.SupplyQty AS IssuedSheets,
                    SD.TotalPieces AS IssuedTotalPieces,
                    D.ReturnSheets,
                    D.ReturnPieces AS ReturnLoosePieces,
                    D.TotalPieces AS ReturnTotalPieces,
                    ISNULL(D.Remarks, '') AS ReturnRemarks
                FROM SaleReturnDetail D
                INNER JOIN SupplyDetail SD  ON D.SupplyDetailID = SD.SupplyDetailID
                INNER JOIN StampCategory SC ON D.CategoryID     = SC.CategoryID
                INNER JOIN Denomination DN  ON D.DenominationID = DN.DenominationID
                WHERE D.ReturnID = @ReturnID
                ORDER BY D.ReturnDetailID;";

            return db.ExecuteQuery(sql, new[] { new SqlParameter("@ReturnID", returnID) });
        }

        public DataTable GetSaleReturnStock(int returnID)
        {
            string sql = @"
                SELECT
                    S.CategoryID,
                    S.DenominationID,
                    SC.Name AS Category,
                    DN.Denomination,
                    S.BoxQty,
                    S.PacketQty,
                    S.SheetQty,
                    S.StampQty
                FROM SaleReturnStock S
                INNER JOIN StampCategory SC ON S.CategoryID     = SC.CategoryID
                INNER JOIN Denomination DN  ON S.DenominationID = DN.DenominationID
                WHERE S.ReturnID = @ReturnID
                ORDER BY S.ReturnStockID;";

            return db.ExecuteQuery(sql, new[] { new SqlParameter("@ReturnID", returnID) });
        }
    }
}