using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace SupplyBranch.DataAccess
{
    /// <summary>One saved row of SupplyDetail.</summary>
    public class SupplyLine
    {
        public int IndentDetailID { get; set; }
        public int CategoryID { get; set; }
        public int DenominationID { get; set; }
        public int SupplySheets { get; set; }
        public int PiecesPerSheet { get; set; }
        public int LoosePieces { get; set; }
        public int TotalPieces { get; set; }
        public string LedgerFolio { get; set; }
        public string CaseNoFrom { get; set; }
        public string CaseNoTo { get; set; }
        public string CaseCode { get; set; }
    }

    /// <summary>One StockTransaction OUT row of a supply.</summary>
    public class StockLine
    {
        public int CategoryID { get; set; }
        public int DenominationID { get; set; }
        public int BoxQty { get; set; }
        public int PacketQty { get; set; }
        public int SheetQty { get; set; }
        public int StampQty { get; set; }
    }

    /// <summary>Pure stock arithmetic (same break-down order as StockDAL.AutoConvertAndDeductStock).</summary>
    internal static class StockMath
    {
        /// <summary>
        /// Deducts the requested quantities, breaking Box -> Packet -> Sheet -> Stamp only when needed.
        /// Returns false (and leaves negative values) when the stock is not enough.
        /// </summary>
        public static bool TryDeduct(
            ref int box, ref int packet, ref int sheet, ref int stamp,
            int reqBox, int reqPacket, int reqSheet, int reqStamp,
            int packetsPerBox, int sheetsPerPacket, int piecesPerSheet)
        {
            while (stamp < reqStamp)
            {
                if (sheet > 0) { sheet--; stamp += piecesPerSheet; }
                else if (packet > 0) { packet--; sheet += sheetsPerPacket; }
                else if (box > 0) { box--; packet += packetsPerBox; }
                else break;
            }
            stamp -= reqStamp;

            while (sheet < reqSheet)
            {
                if (packet > 0) { packet--; sheet += sheetsPerPacket; }
                else if (box > 0) { box--; packet += packetsPerBox; }
                else break;
            }
            sheet -= reqSheet;

            while (packet < reqPacket)
            {
                if (box > 0) { box--; packet += packetsPerBox; }
                else break;
            }
            packet -= reqPacket;

            box -= reqBox;

            return box >= 0 && packet >= 0 && sheet >= 0 && stamp >= 0;
        }
    }

    internal class SupplyDAL
    {
        private readonly DBHelper db = new DBHelper();

        public int GetSheetsPerCase(int categoryID, int denominationID)
        {
            // PacketsPerBox aur SheetsPerPacket ko multiply karke Sheets Per Case calculate kar rahe hain
            string query = @"SELECT ISNULL(PacketsPerBox * SheetsPerPacket, 0) 
                    FROM UnitConversionMaster 
                    WHERE CategoryID = @CategoryID AND DenominationID = @DenominationID";

            SqlParameter[] param = {
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID)
    };

            object result = db.ExecuteScalar(query, param);

            if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int sheetsPerCase))
            {
                return sheetsPerCase;
            }

            return 0;
        }


        public void UpdateIndentStatusAfterSupply(int supplyID, int indentID)
        {
            string sql = @"
DECLARE @IndentTotalPieces INT;
DECLARE @TotalSupplyPieces INT;
DECLARE @NewIndentStatusID INT;
DECLARE @NewStatusID INT;

--------------------------------------------------
-- Total Indent Quantity
--------------------------------------------------

SELECT
    @IndentTotalPieces = ISNULL(SUM(ID.TotalPieces), 0)
FROM IndentDetail ID
WHERE ID.IndentID = @IndentID;


--------------------------------------------------
-- Total Approved / Issued Supply
--------------------------------------------------

SELECT
    @TotalSupplyPieces = ISNULL(SUM(SD.TotalPieces), 0)
FROM SupplyDetail SD
INNER JOIN SupplyMaster SM
    ON SD.SupplyID = SM.SupplyID
WHERE SM.IndentID = @IndentID
  AND SM.StatusID IN (2,3);


--------------------------------------------------
-- Decide Indent Status
--------------------------------------------------

IF @TotalSupplyPieces >= @IndentTotalPieces
BEGIN
    SET @NewIndentStatusID = 7; -- Closed
    SET @NewStatusID = 7; -- Closed
END
ELSE
BEGIN
    SET @NewIndentStatusID = 6; -- Partial
    SET @NewStatusID = 5; -- Open
END;


--------------------------------------------------
-- Update IndentMaster
--------------------------------------------------

UPDATE IndentMaster
SET IndentStatus = @NewIndentStatusID,
    StatusID = @NewStatusID
WHERE IndentID = @IndentID;


--------------------------------------------------
-- Update Current Supply
--------------------------------------------------

UPDATE SupplyMaster
SET IndentStatusID = @NewIndentStatusID
WHERE SupplyID = @SupplyID;
";

            SqlParameter[] parameters =
            {
        new SqlParameter("@SupplyID", supplyID),
        new SqlParameter("@IndentID", indentID)
    };

            db.ExecuteNonQuery(sql, parameters);
        }

        public int GetTotalSupplyPieces(int indentID)
        {
            string sql = @"
SELECT
    ISNULL(SUM(SD.TotalPieces), 0)
FROM SupplyMaster SM
INNER JOIN SupplyDetail SD
    ON SM.SupplyID = SD.SupplyID
WHERE SM.IndentID = @IndentID;
";

            SqlParameter[] parameters =
            {
        new SqlParameter("@IndentID", indentID)
    };

            object result = db.ExecuteScalar(sql, parameters);

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Deletes a DRAFT supply (details, stock entry, header) in one transaction
        /// and re-evaluates the indent status. Non-draft supplies are never touched.
        /// </summary>
        public bool DeleteDraftSupply(int supplyID)
        {
            string query = @"
SET XACT_ABORT ON;
BEGIN TRAN;

DECLARE @IndentID INT = (SELECT IndentID FROM SupplyMaster WHERE SupplyID = @SupplyID AND StatusID = 1);

IF @IndentID IS NOT NULL
BEGIN
    DELETE FROM StockTransaction WHERE SupplyID = @SupplyID AND TransactionType = 'OUT';
    DELETE FROM SupplyDetail     WHERE SupplyID = @SupplyID;
    DELETE FROM SupplyMaster     WHERE SupplyID = @SupplyID AND StatusID = 1;

    DECLARE @Total    INT = (SELECT ISNULL(SUM(TotalPieces), 0) FROM IndentDetail WHERE IndentID = @IndentID);
    DECLARE @Supplied INT = (SELECT ISNULL(SUM(SD.TotalPieces), 0)
                             FROM SupplyDetail SD
                             INNER JOIN SupplyMaster SM ON SD.SupplyID = SM.SupplyID
                             WHERE SM.IndentID = @IndentID AND SM.StatusID IN (1, 2, 3));

    UPDATE IndentMaster
    SET IndentStatus = CASE WHEN @Supplied = 0 THEN 5
                            WHEN @Supplied >= @Total THEN 7
                            ELSE 6 END
    WHERE IndentID = @IndentID;
END

COMMIT TRAN;
SELECT CASE WHEN @IndentID IS NULL THEN 0 ELSE 1 END;";

            SqlParameter[] p =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            object result = db.ExecuteScalar(query, p);

            return result != null && result != DBNull.Value && Convert.ToInt32(result) == 1;
        }

        public DataTable GetIndentItems(int indentID)
        {
            string sql = @"
    SELECT
        DetailID,
        CategoryID,
        DenominationID,
        CategoryName,
        Denomination,

        IndentSheets,
        IndentLoosePieces,
        IndentTotalPieces,

        RemainingTotalPieces,

        CASE
            WHEN RemainingTotalPieces < IndentTotalPieces
                THEN RemainingTotalPieces
            ELSE 0
        END AS OriginalPendingPieces,

        PiecesPerSheet,

        SupplySheets,
        SupplyLoosePieces,
        SupplyTotalPieces

    FROM vwIndentBalance

    WHERE IndentID=@IndentID
      AND RemainingTotalPieces > 0

    ORDER BY CategoryName, Denomination";

            SqlParameter[] parameters =
            {
        new SqlParameter("@IndentID", indentID)
    };

            return db.ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// Returns supplies filtered by StatusIDs (eg. Draft=1, Approved=2)
        /// </summary>
        public DataTable GetSuppliesByStatusIDs(int[] statusIDs)
        {
            if (statusIDs == null || statusIDs.Length == 0)
                return new DataTable();

            StringBuilder sql = new StringBuilder();

            sql.Append(@"
SELECT
    SM.SupplyID,
    SM.SupplyNo,
    SM.SupplyDate,
    SM.IndentID,
    ISNULL(IM.IndentNo, '') AS IndentNo,
    ISNULL(O.OfficeName, '') AS OfficeName,
    SM.StatusID,
    ISNULL((SELECT SUM(SD.TotalPieces) FROM SupplyDetail SD WHERE SD.SupplyID = SM.SupplyID), 0) AS TotalPieces,
    ISNULL((SELECT SUM(SD.SupplyQty) FROM SupplyDetail SD WHERE SD.SupplyID = SM.SupplyID), 0) AS TotalSheet
FROM SupplyMaster SM
LEFT JOIN IndentMaster IM ON SM.IndentID = IM.IndentID
LEFT JOIN Office O ON IM.OfficeID = O.OfficeID
WHERE SM.StatusID IN (");

            List<SqlParameter> parameters = new List<SqlParameter>();

            for (int i = 0; i < statusIDs.Length; i++)
            {
                string paramName = "@s" + i;
                sql.Append(paramName);
                if (i < statusIDs.Length - 1)
                    sql.Append(",");

                parameters.Add(new SqlParameter(paramName, statusIDs[i]));
            }

            sql.Append(@")
ORDER BY SM.SupplyDate DESC, SM.SupplyNo DESC");

            return db.ExecuteQuery(sql.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// Returns supplies filtered by Status names (eg. 'Draft', 'Approve')
        /// </summary>
        public DataTable GetSuppliesByStatusNames(string[] statusNames)
        {
            if (statusNames == null || statusNames.Length == 0)
                return new DataTable();

            StringBuilder sql = new StringBuilder();

            sql.Append(@"
SELECT
    SM.SupplyID,
    SM.SupplyNo,
    SM.SupplyDate,
    SM.IndentID,
    ISNULL(IM.IndentNo, '') AS IndentNo,
    ISNULL(O.OfficeName, '') AS OfficeName,
    SM.StatusID,
    ISNULL((SELECT SUM(SD.TotalPieces) FROM SupplyDetail SD WHERE SD.SupplyID = SM.SupplyID), 0) AS TotalPieces,
    ISNULL((SELECT SUM(SD.SupplyQty) FROM SupplyDetail SD WHERE SD.SupplyID = SM.SupplyID), 0) AS TotalSheet,
    ISNULL(SS.StatusName, '') AS StatusName
FROM SupplyMaster SM
LEFT JOIN IndentMaster IM ON SM.IndentID = IM.IndentID
LEFT JOIN Office O ON IM.OfficeID = O.OfficeID
LEFT JOIN Status SS ON SM.StatusID = SS.StatusID
WHERE SS.StatusName IN (");

            List<SqlParameter> parameters = new List<SqlParameter>();

            for (int i = 0; i < statusNames.Length; i++)
            {
                string paramName = "@n" + i;
                sql.Append(paramName);
                if (i < statusNames.Length - 1)
                    sql.Append(",");

                parameters.Add(new SqlParameter(paramName, statusNames[i]));
            }

            sql.Append(@")
ORDER BY SM.SupplyDate DESC, SM.SupplyNo DESC");

            return db.ExecuteQuery(sql.ToString(), parameters.ToArray());
        }

        public DataTable GetPendingIndent(
                            int officeId,
                            DateTime? fromDate,
                            DateTime? toDate,
                            int filterType,
                            string indentNo)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                        SELECT
                            VB.IndentID,
                            VB.IndentNo,
                            VB.OfficeName,
                            VB.IndentDate,

                            COUNT(VB.DetailID) AS TotalItems,

                                SUM(CASE
                                WHEN VB.RemainingTotalPieces > 0
                                THEN 1
                                ELSE 0
                                END) AS RemainingItems,

                                MAX(VB.StatusName) AS StatusName

                        FROM vwIndentBalance VB

                        WHERE 1 = 1
                        ");

            List<SqlParameter> parameters = new List<SqlParameter>();

            // Office Filter
            if (officeId > 0)
            {
                sql.Append(" AND VB.OfficeID=@OfficeID ");
                parameters.Add(new SqlParameter("@OfficeID", officeId));
            }

            // Date From
            if (fromDate.HasValue)
            {
                sql.Append(" AND VB.IndentDate>=@FromDate ");
                parameters.Add(new SqlParameter("@FromDate", fromDate.Value.Date));
            }

            // Date To
            if (toDate.HasValue)
            {
                sql.Append(" AND VB.IndentDate < @ToDate ");
                parameters.Add(
                    new SqlParameter(
                        "@ToDate",
                        toDate.Value.Date.AddDays(1)));
            }

            // Indent No Filter
            if (!string.IsNullOrWhiteSpace(indentNo))
            {
                sql.Append(" AND VB.IndentNo LIKE @IndentNo ");
                parameters.Add(new SqlParameter("@IndentNo", "%" + indentNo.Trim() + "%"));
            }


            sql.Append(@"

                  GROUP BY

                    VB.IndentID,
                    VB.IndentNo,
                    VB.OfficeName,
                    VB.IndentDate
                    ");

            switch (filterType)
            {
                // Pending
                case 0:

                    sql.Append(@"
                    HAVING
                    SUM(CASE
                            WHEN VB.RemainingTotalPieces > 0
                            THEN 1
                            ELSE 0
                        END) > 0
                    ");
                    break;

                // Completed
                case 1:

                    sql.Append(@"
                    HAVING
                    SUM(CASE
                            WHEN VB.RemainingTotalPieces > 0
                            THEN 1
                            ELSE 0
                        END) = 0
                    ");
                    break;

                // All
                case 2:
                    break;
            }

            sql.Append(@"
                    ORDER BY
                    VB.IndentDate DESC,
                    VB.IndentNo DESC
                    ");

            return db.ExecuteQuery(sql.ToString(), parameters.ToArray());
        }


        public DataTable GetIndentHeader(int indentID)
        {
            string sql = @"
SELECT
    IM.IndentID,
    IM.IndentNo,
    IM.IndentDate,
    IM.IndentStatus,
    O.OfficeName,
    S.StatusName
FROM IndentMaster IM

INNER JOIN Office O
    ON IM.OfficeID = O.OfficeID

INNER JOIN Status S
    ON IM.StatusID = S.StatusID

WHERE IM.IndentID = @IndentID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@IndentID", indentID)
    };

            return db.ExecuteQuery(sql, parameters);
        }

        public void InsertSupplyDetail(
            int supplyID,
            DataGridViewRow row)
        {

            string query = @"

INSERT INTO SupplyDetail
(
    SupplyID,
    IndentDetailID,
    CategoryID,
    DenominationID,
    SupplyQty,
    PiecesPerSheet,
    LoosePieces,
    TotalPieces,
    LedgerFolio,
    CaseNoFrom,
    CaseNoTo,
    CaseCode
)

VALUES
(
    @SupplyID,
    @IndentDetailID,
    @CategoryID,
    @DenominationID,
    @SupplyQty,
    @PiecesPerSheet,
    @LoosePieces,
    @TotalPieces,
    @LedgerFolio,
    @CaseNoFrom,
    @CaseNoTo,
    @CaseCode
)";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID),

        new SqlParameter("@IndentDetailID",
            Convert.ToInt32(row.Cells["DetailID"].Value)),

        new SqlParameter("@CategoryID",
            Convert.ToInt32(row.Cells["CategoryID"].Value)),

        new SqlParameter("@DenominationID",
            Convert.ToInt32(row.Cells["DenominationID"].Value)),

        new SqlParameter("@SupplyQty",
            Convert.ToInt32(row.Cells["SupplySheets"].Value)),

        new SqlParameter("@PiecesPerSheet",
            Convert.ToInt32(row.Cells["PiecesPerSheet"].Value)),

        new SqlParameter("@LoosePieces",
            Convert.ToInt32(row.Cells["SupplyPieces"].Value)),

        new SqlParameter("@TotalPieces",
            Convert.ToInt32(row.Cells["SupplyTotalPieces"].Value)),

        new SqlParameter("@LedgerFolio",
            Convert.ToString(row.Cells["LedgerFolio"].Value ?? "")),

        new SqlParameter("@CaseNoFrom",
            Convert.ToString(row.Cells["CaseNoFrom"].Value ?? "")),

        new SqlParameter("@CaseNoTo",
            Convert.ToString(row.Cells["CaseNoTo"].Value ?? "")),

        new SqlParameter("@CaseCode",
            Convert.ToString(row.Cells["CaseCode"].Value ?? ""))
    };

            db.ExecuteNonQuery(query, param);
        }

        public DataTable GetSupplyDetails(int supplyID)
        {
            string query = @"

SELECT

    SD.IndentDetailID AS DetailID,

    SD.CategoryID,
    SD.DenominationID,

    SC.Name AS CategoryName,

    D.Denomination,

    ID.SheetQty AS IndentSheets,
    ID.PieceQty AS IndentLoosePieces,
    ID.TotalPieces AS IndentTotalPieces,

    -- Original Pending Quantity
    ID.TotalPieces AS OriginalPendingPieces,

    -- Current Balance
    (ID.TotalPieces - ISNULL(SD.TotalPieces,0))
        AS RemainingTotalPieces,

    SD.SupplyQty AS SupplySheets,
    SD.LoosePieces AS SupplyPieces,
    SD.PiecesPerSheet,
    SD.TotalPieces AS SupplyTotalPieces,

    SD.LedgerFolio,
    SD.CaseCode,
    SD.CaseNoFrom,
    SD.CaseNoTo

FROM SupplyDetail SD

INNER JOIN IndentDetail ID
    ON SD.IndentDetailID = ID.DetailID

INNER JOIN StampCategory SC
    ON SD.CategoryID = SC.CategoryID

INNER JOIN Denomination D
    ON SD.DenominationID = D.DenominationID

WHERE SD.SupplyID=@SupplyID

ORDER BY SD.IndentDetailID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            return db.ExecuteQuery(query, param);
        }

        public string GetOfficeCode(int indentID)
        {
            string query = @"
        SELECT O.OfficeCode
        FROM IndentMaster IM
        INNER JOIN Office O
            ON IM.OfficeID = O.OfficeID
        WHERE IM.IndentID = @IndentID";

            SqlParameter[] param =
            {
        new SqlParameter("@IndentID", indentID)
    };

            object result = db.ExecuteScalar(query, param);

            if (result == null || result == DBNull.Value)
                return "";

            return result.ToString();
        }


        public int GetLastGlobalNumber(string financialYear)
        {
            string query = @"
        SELECT ISNULL(MAX(GlobalSequence),0)
        FROM SupplyMaster
        WHERE FinancialYear = @FinancialYear";

            SqlParameter[] param =
            {
        new SqlParameter("@FinancialYear", financialYear)
    };

            object result = db.ExecuteScalar(query, param);

            return Convert.ToInt32(result);
        }

        public int GetLastOfficeNumber(int officeID, string financialYear)
        {
            string query = @"
        SELECT ISNULL(MAX(SM.OfficeSequence),0)
        FROM SupplyMaster SM
        INNER JOIN IndentMaster IM
            ON SM.IndentID = IM.IndentID
        WHERE IM.OfficeID = @OfficeID
          AND SM.FinancialYear = @FinancialYear";

            SqlParameter[] param =
            {
        new SqlParameter("@OfficeID", officeID),
        new SqlParameter("@FinancialYear", financialYear)
    };

            object result = db.ExecuteScalar(query, param);

            return Convert.ToInt32(result);
        }

        public int GetOfficeID(int indentID)
        {
            string query = @"
        SELECT OfficeID
        FROM IndentMaster
        WHERE IndentID = @IndentID";

            SqlParameter[] param =
            {
        new SqlParameter("@IndentID", indentID)
    };

            object result = db.ExecuteScalar(query, param);

            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }


        public (int OfficeID, string OfficeCode) GetOfficeInfo(int indentID)
        {
            string query = @"
    SELECT O.OfficeID, O.OfficeCode
    FROM IndentMaster IM
    INNER JOIN Office O
        ON IM.OfficeID = O.OfficeID
    WHERE IM.IndentID=@IndentID";

            SqlParameter[] param =
            {
        new SqlParameter("@IndentID", indentID)
    };

            DataTable dt = db.ExecuteQuery(query, param);

            if (dt.Rows.Count == 0)
                return (0, "");

            return (
                Convert.ToInt32(dt.Rows[0]["OfficeID"]),
                dt.Rows[0]["OfficeCode"].ToString()
            );
        }

        public int SaveDraftSupply(
    SupplyNumberInfo info,
    int indentID,
    int supplyTypeID,
    string dispatchMode,
    string packingType,
    string otherPackingType,
    int packingQty,
    string remarks,
    int indentStatusID,
    DateTime supplyDate)
        {
            string query = @"
INSERT INTO SupplyMaster
(
    SupplyNo,
    SupplyType,
    FinancialYear,
    SupplyDate,
    IndentID,
    StatusID,
    IndentStatusID,
    Remarks,
    CreatedDate,
    DispatchMode,
    PackingType,
    OtherPackingType,
    PackingQty,
    GlobalSequence,
    OfficeSequence
)
VALUES
(
    @SupplyNo,
    @SupplyType,
    @FinancialYear,
    @SupplyDate,
    @IndentID,
    1,                  -- Supply = Draft
    @IndentStatusID,    -- 5 Open / 6 Partial / 7 Closed
    @Remarks,
    GETDATE(),
    @DispatchMode,
    @PackingType,
    @OtherPackingType,
    @PackingQty,
    @GlobalSequence,
    @OfficeSequence
);

DECLARE @SupplyID INT;
SET @SupplyID = CAST(SCOPE_IDENTITY() AS INT);

UPDATE IndentMaster
SET IndentStatus = @IndentStatusID
WHERE IndentID = @IndentID;

SELECT @SupplyID;
";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyNo", info.SupplyNo),
        new SqlParameter("@SupplyType", supplyTypeID),
        new SqlParameter("@FinancialYear", info.FinancialYear),

        new SqlParameter("@SupplyDate",supplyDate),

        new SqlParameter("@IndentID", indentID),
        new SqlParameter("@IndentStatusID", indentStatusID),

        new SqlParameter("@Remarks", remarks ?? ""),
        new SqlParameter("@DispatchMode", dispatchMode ?? ""),
        new SqlParameter("@PackingType", packingType ?? ""),
        new SqlParameter("@OtherPackingType", (object)otherPackingType ?? DBNull.Value),
        new SqlParameter("@PackingQty", packingQty),

        new SqlParameter("@GlobalSequence", info.GlobalSequence),
        new SqlParameter("@OfficeSequence", info.OfficeSequence)
    };

            object result = db.ExecuteScalar(query, param);

            return Convert.ToInt32(result);
        }



        public DataTable GetDraftStatus()
        {
            string query = @"

    SELECT 0 AS StatusID, 'All' AS StatusName

    UNION ALL

    SELECT
        StatusID,
        StatusName
    FROM Status
    WHERE StatusID IN (1,2,3,4)

    ORDER BY StatusID";

            return db.ExecuteQuery(query, null);
        }

        public DataTable GetSupplyTypes()
        {
            string query = @"
        SELECT
            SupplyTypeID,
            SupplyTypeName
        FROM SupplyType
        ORDER BY SupplyTypeName";

            return db.ExecuteQuery(query, null);
        }

        public int GetExistingDraft(int indentID)
        {
            string query = @"
        SELECT TOP 1 SupplyID
        FROM SupplyMaster
        WHERE IndentID = @IndentID
          AND StatusID = 1
        ORDER BY SupplyID DESC";

            SqlParameter[] param =
            {
        new SqlParameter("@IndentID", indentID)
    };

            object result = db.ExecuteScalar(query, param);

            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }

        public DataTable GetDraftHeader(int supplyID)
        {
            string query = @"

SELECT
    SM.SupplyID,
    SM.IndentID,
    SM.SupplyNo,
    SM.FinancialYear,
    SM.SupplyDate,
    SM.SupplyType,
    SM.DispatchMode,
    SM.PackingType,
    SM.OtherPackingType,
    SM.PackingQty,
    SM.InvoiceNo,
    SM.Remarks,
    SM.StatusID,
    ST.StatusName AS SupplyStatus


FROM SupplyMaster SM

LEFT JOIN Status ST
    ON SM.StatusID = ST.StatusID

WHERE SM.SupplyID = @SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            return db.ExecuteQuery(query, param);
        }


        public void DeleteSupplyDetails(int supplyID)
        {
            string query = @"
        DELETE FROM SupplyDetail
        WHERE SupplyID = @SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            db.ExecuteNonQuery(query, param);
        }

        // NOTE: the old InsertSupplyDetail(...) overload that wrote the non-existing columns
        // SupplySheets / SupplyPieces / TotalSupplyQty has been removed (it was never called).

        public void UpdateSupplyMaster(
      int supplyID,
      int supplyTypeID,
      DateTime supplyDate,
      string dispatchMode,
      string packingType,
      string otherPackingType,
      int packingQty,
      string invoiceNo,
      string remarks)
        {
            string query = @"
UPDATE SupplyMaster
SET
    SupplyType   = @SupplyType,
    SupplyDate   = @SupplyDate,
    DispatchMode = @DispatchMode,
    PackingType  = @PackingType,
    OtherPackingType = @OtherPackingType,
    PackingQty   = @PackingQty,
    InvoiceNo    = @InvoiceNo,
    Remarks      = @Remarks
WHERE SupplyID = @SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID),

        new SqlParameter("@SupplyType", supplyTypeID),

        new SqlParameter("@SupplyDate", supplyDate),

        new SqlParameter("@DispatchMode", dispatchMode ?? ""),
        new SqlParameter("@PackingType", packingType ?? ""),
        new SqlParameter("@OtherPackingType", (object)otherPackingType ?? DBNull.Value),
        new SqlParameter("@PackingQty", packingQty),

        new SqlParameter("@InvoiceNo", invoiceNo ?? ""),
        new SqlParameter("@Remarks", remarks ?? "")
    };

            db.ExecuteNonQuery(query, param);
        }


        public SupplyNumberInfo GetSupplyInfo(int supplyID)
        {
            string query = @"
SELECT
    SupplyNo,
    FinancialYear,
    GlobalSequence,
    OfficeSequence
FROM SupplyMaster
WHERE SupplyID=@SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            DataTable dt = db.ExecuteQuery(query, param);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new SupplyNumberInfo
            {
                SupplyNo = row["SupplyNo"].ToString(),
                FinancialYear = row["FinancialYear"].ToString(),
                GlobalSequence = Convert.ToInt32(row["GlobalSequence"]),
                OfficeSequence = Convert.ToInt32(row["OfficeSequence"])
            };
        }


        public DataTable GetSupplyHeader(int supplyID)
        {
            string query = @"

SELECT

SM.SupplyID,
SM.SupplyNo,
SM.FinancialYear,
SM.SupplyType,
SM.DispatchMode,
SM.PackingType,
SM.OtherPackingType,
SM.PackingQty,
SM.Remarks,
SM.NeedReprint,
SM.IndentID,

IM.IndentNo,
IM.IndentDate,

O.OfficeName,

SS.StatusName

FROM SupplyMaster SM

INNER JOIN IndentMaster IM
ON SM.IndentID=IM.IndentID

INNER JOIN Office O
ON IM.OfficeID=O.OfficeID

INNER JOIN Status SS
ON SM.StatusID=SS.StatusID

WHERE SM.SupplyID=@SupplyID
";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            return db.ExecuteQuery(query, param);
        }


        /// <summary>
        /// Old status-only issue. Only an Approved supply can be issued.
        /// Prefer IssueSupplyWithStock (status + stock deduction in one transaction).
        /// </summary>
        public void IssueSupply(int supplyID)
        {
            string query = @"

UPDATE SupplyMaster

SET
    StatusID = 3

WHERE SupplyID = @SupplyID
  AND StatusID = 2";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            db.ExecuteNonQuery(query, param);
        }

        public DataTable GetDraftSupply(
      int officeID,
      int statusID,
      DateTime? fromDate,
      DateTime? toDate,
      string supplyNo)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"

SELECT

SM.SupplyID,
SM.SupplyNo,
SM.InvoiceNo,
SM.SupplyDate,
O.OfficeName,
SS.StatusName

FROM SupplyMaster SM

INNER JOIN IndentMaster IM
ON SM.IndentID = IM.IndentID

INNER JOIN Office O
ON IM.OfficeID = O.OfficeID

INNER JOIN Status SS
ON SM.StatusID = SS.StatusID

WHERE 1 = 1");

            List<SqlParameter> param =
                new List<SqlParameter>();

            // =======================
            // Status Filter
            // =======================
            if (statusID > 0)
            {
                sql.Append(" AND SM.StatusID=@StatusID");

                param.Add(
                    new SqlParameter("@StatusID", statusID));
            }

            // =======================
            // Office Filter
            // =======================
            if (officeID > 0)
            {
                sql.Append(" AND O.OfficeID=@OfficeID");

                param.Add(
                    new SqlParameter("@OfficeID", officeID));
            }

            if (fromDate.HasValue)
            {
                sql.Append(" AND SM.SupplyDate>=@FromDate");

                param.Add(
                    new SqlParameter("@FromDate",
                    fromDate.Value.Date));
            }

            if (toDate.HasValue)
            {
                // "To date" must include the whole last day
                sql.Append(" AND SM.SupplyDate < @ToDate");

                param.Add(
                    new SqlParameter("@ToDate",
                    toDate.Value.Date.AddDays(1)));
            }

            if (!string.IsNullOrWhiteSpace(supplyNo))
            {
                sql.Append(" AND SM.SupplyNo LIKE @SupplyNo");

                param.Add(
                    new SqlParameter("@SupplyNo",
                    "%" + supplyNo.Trim() + "%"));
            }

            sql.Append(@"

ORDER BY
SM.SupplyDate DESC,
SM.SupplyNo DESC");

            return db.ExecuteQuery(
                sql.ToString(),
                param.ToArray());
        }


        public void ApproveSupply(
    int supplyID,
    SupplyNumberInfo info)
        {
            string query = @"

UPDATE SupplyMaster

SET

SupplyNo=@SupplyNo,
FinancialYear=@FinancialYear,
GlobalSequence=@GlobalSequence,
OfficeSequence=@OfficeSequence,
StatusID=2

WHERE SupplyID=@SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID),
        new SqlParameter("@SupplyNo", info.SupplyNo),
        new SqlParameter("@FinancialYear", info.FinancialYear),
        new SqlParameter("@GlobalSequence", info.GlobalSequence),
        new SqlParameter("@OfficeSequence", info.OfficeSequence)
    };

            db.ExecuteNonQuery(query, param);
        }

        public int GetLastApprovedGlobalNumber(string financialYear)
        {
            string query = @"

SELECT ISNULL(MAX(GlobalSequence),0)

FROM SupplyMaster

WHERE FinancialYear=@FinancialYear
AND StatusID IN (2,3)";

            SqlParameter[] param =
            {
        new SqlParameter("@FinancialYear", financialYear)
    };

            return Convert.ToInt32(db.ExecuteScalar(query, param));
        }

        public int GetLastSupplyGlobalNumber(string financialYear)
        {
            string sql = @"
        SELECT ISNULL(MAX(GlobalSequence), 0)
        FROM SupplyMaster
        WHERE FinancialYear = @FinancialYear
          AND StatusID IN (1, 2, 3)";

            SqlParameter[] parameters =
            {
        new SqlParameter("@FinancialYear", financialYear)
    };

            object result = db.ExecuteScalar(sql, parameters);

            return result == null || result == DBNull.Value
                ? 0
                : Convert.ToInt32(result);
        }

        public int GetLastSupplyOfficeNumber(
     int officeID,
     string financialYear)
        {
            string sql = @"
        SELECT ISNULL(MAX(SM.OfficeSequence), 0)
        FROM SupplyMaster SM
        INNER JOIN IndentMaster IM
            ON SM.IndentID = IM.IndentID
        WHERE IM.OfficeID = @OfficeID
          AND SM.FinancialYear = @FinancialYear
          AND SM.StatusID IN (1, 2, 3)";

            SqlParameter[] parameters =
            {
        new SqlParameter("@OfficeID", officeID),
        new SqlParameter("@FinancialYear", financialYear)
    };

            object result = db.ExecuteScalar(sql, parameters);

            return result == null || result == DBNull.Value
                ? 0
                : Convert.ToInt32(result);
        }

        public bool CanAssignInvoiceSequentially(int supplyID)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM SupplyMaster
        WHERE SupplyID < @SupplyID
          AND StatusID = 1
          AND GlobalInvoiceSequence IS NULL";

            SqlParameter[] parameters =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            object result = db.ExecuteScalar(sql, parameters);

            int previousPendingDrafts =
                result == null || result == DBNull.Value
                    ? 0
                    : Convert.ToInt32(result);

            // A previous draft still has no GlobalInvoiceSequence
            if (previousPendingDrafts > 0)
                return false;

            return true;
        }

        public bool CanApproveDraftSequentially(int supplyID)
        {
            string sql = @"
        SELECT
            SM.SupplyID,
            SM.GlobalSequence,
            SM.OfficeSequence,
            SM.FinancialYear,
            IM.OfficeID
        FROM SupplyMaster SM
        INNER JOIN IndentMaster IM
            ON SM.IndentID = IM.IndentID
        WHERE SM.SupplyID = @SupplyID
          AND SM.StatusID = 1";

            SqlParameter[] parameters =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            DataTable dt = db.ExecuteQuery(sql, parameters);

            // Current supply does not exist or is not a draft
            if (dt.Rows.Count == 0)
                return false;

            DataRow row = dt.Rows[0];

            int currentGlobalSequence =
                Convert.ToInt32(row["GlobalSequence"]);

            int currentOfficeSequence =
                Convert.ToInt32(row["OfficeSequence"]);

            int officeID =
                Convert.ToInt32(row["OfficeID"]);

            string financialYear =
                row["FinancialYear"].ToString();

            // ==========================================
            // Check Lowest Global Draft Sequence
            // ==========================================

            string globalSql = @"
        SELECT ISNULL(MIN(GlobalSequence), 0)
        FROM SupplyMaster
        WHERE StatusID = 1
          AND FinancialYear = @FinancialYear";

            SqlParameter[] globalParameters =
            {
        new SqlParameter("@FinancialYear", financialYear)
    };

            object globalResult =
                db.ExecuteScalar(globalSql, globalParameters);

            int lowestGlobalSequence =
                globalResult == null || globalResult == DBNull.Value
                    ? 0
                    : Convert.ToInt32(globalResult);

            // ==========================================
            // Check Lowest Office Draft Sequence
            // ==========================================

            string officeSql = @"
        SELECT ISNULL(MIN(SM.OfficeSequence), 0)
        FROM SupplyMaster SM
        INNER JOIN IndentMaster IM
            ON SM.IndentID = IM.IndentID
        WHERE SM.StatusID = 1
          AND SM.FinancialYear = @FinancialYear
          AND IM.OfficeID = @OfficeID";

            SqlParameter[] officeParameters =
            {
        new SqlParameter("@FinancialYear", financialYear),
        new SqlParameter("@OfficeID", officeID)
    };

            object officeResult =
                db.ExecuteScalar(officeSql, officeParameters);

            int lowestOfficeSequence =
                officeResult == null || officeResult == DBNull.Value
                    ? 0
                    : Convert.ToInt32(officeResult);

            // ==========================================
            // Both sequences must be the lowest
            // ==========================================

            if (currentGlobalSequence != lowestGlobalSequence)
                return false;

            if (currentOfficeSequence != lowestOfficeSequence)
                return false;

            return true;
        }

        public int GetLastApprovedOfficeNumber(
    int officeID,
    string financialYear)
        {
            string query = @"

SELECT ISNULL(MAX(OfficeSequence),0)

FROM SupplyMaster SM

INNER JOIN IndentMaster IM
ON SM.IndentID=IM.IndentID

WHERE IM.OfficeID=@OfficeID
AND SM.FinancialYear=@FinancialYear
AND SM.StatusID IN (2,3)";

            SqlParameter[] param =
            {
        new SqlParameter("@OfficeID", officeID),
        new SqlParameter("@FinancialYear", financialYear)
    };

            return Convert.ToInt32(db.ExecuteScalar(query, param));
        }

        public void ApproveSupply(
    int supplyID,
    SupplyNumberInfo info,
    int supplyTypeID,
    string dispatchMode,
    string packingType,
    string otherPackingType,
    int packingQty,
    string remarks)
        {
            string query = @"

UPDATE SupplyMaster

SET

SupplyNo        = @SupplyNo,
FinancialYear   = @FinancialYear,
SupplyType      = @SupplyType,
DispatchMode    = @DispatchMode,
PackingType     = @PackingType,
OtherPackingType = @OtherPackingType,
PackingQty      = @PackingQty,
Remarks         = @Remarks,
GlobalSequence  = @GlobalSequence,
OfficeSequence  = @OfficeSequence,
StatusID        = 2

WHERE SupplyID = @SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID),
        new SqlParameter("@SupplyNo", info.SupplyNo),
        new SqlParameter("@FinancialYear", info.FinancialYear),
        new SqlParameter("@SupplyType", supplyTypeID),
        new SqlParameter("@DispatchMode", dispatchMode),
        new SqlParameter("@PackingType", packingType),
        new SqlParameter("@OtherPackingType", (object)otherPackingType ?? DBNull.Value),
        new SqlParameter("@PackingQty", packingQty),
        new SqlParameter("@Remarks", remarks),
        new SqlParameter("@GlobalSequence", info.GlobalSequence),
        new SqlParameter("@OfficeSequence", info.OfficeSequence)
    };

            db.ExecuteNonQuery(query, param);
        }

        /// <summary>
        /// Cancels a DRAFT supply: removes its stock entry and re-evaluates the indent status.
        /// </summary>
        public void CancelSupply(int supplyID)
        {
            string query = @"
SET XACT_ABORT ON;
BEGIN TRAN;

DECLARE @IndentID INT = (SELECT IndentID FROM SupplyMaster WHERE SupplyID = @SupplyID AND StatusID = 1);

IF @IndentID IS NOT NULL
BEGIN
    UPDATE SupplyMaster SET StatusID = 4 WHERE SupplyID = @SupplyID AND StatusID = 1;
    DELETE FROM StockTransaction WHERE SupplyID = @SupplyID AND TransactionType = 'OUT';

    DECLARE @Total    INT = (SELECT ISNULL(SUM(TotalPieces), 0) FROM IndentDetail WHERE IndentID = @IndentID);
    DECLARE @Supplied INT = (SELECT ISNULL(SUM(SD.TotalPieces), 0)
                             FROM SupplyDetail SD
                             INNER JOIN SupplyMaster SM ON SD.SupplyID = SM.SupplyID
                             WHERE SM.IndentID = @IndentID AND SM.StatusID IN (1, 2, 3));

    UPDATE IndentMaster
    SET IndentStatus = CASE WHEN @Supplied = 0 THEN 5
                            WHEN @Supplied >= @Total THEN 7
                            ELSE 6 END
    WHERE IndentID = @IndentID;
END

COMMIT TRAN;";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            db.ExecuteNonQuery(query, param);
        }

        public DataTable GetSupplyPerformaHeader(int supplyID, bool? adddate)
        {
            string query = @"
SELECT
    SM.SupplyID,
    SM.SupplyNo,
    SM.SupplyDate,
    SM.FinancialYear,

    IM.IndentNo,
    IM.IndentDate,

    O.OfficeName,
    O.OfficeFileNo AS FileNo,
    O.OfficeCode,

    SM.PackingType,
    SM.OtherPackingType,
    SM.PackingQty,
    SM.DispatchMode,
    SM.InvoiceNo,

    @AddDate AS AddDate

FROM SupplyMaster SM

INNER JOIN IndentMaster IM
    ON SM.IndentID = IM.IndentID

INNER JOIN Office O
    ON IM.OfficeID = O.OfficeID

WHERE SM.SupplyID = @SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID),
        new SqlParameter("@AddDate", (object)adddate ?? DBNull.Value)
    };

            return db.ExecuteQuery(query, param);
        }

        public DataTable GetSupplyPerformaHeaderForMultiple(string supplyIDs, bool? adddate)
        {
            // Only whole numbers are kept, so the list can never inject SQL
            supplyIDs = SanitizeIdList(supplyIDs);

            // Yahan IN clause ke sath query banayi hai taake multiple SupplyIDs ek sath aa sakein
            if (string.IsNullOrWhiteSpace(supplyIDs))
                return new DataTable();

            string query = @"SELECT    
                SM.SupplyID,    
                SM.SupplyNo,    
                SM.SupplyDate,    
                SM.FinancialYear,    
                IM.IndentNo,    
                IM.IndentDate,    
                O.OfficeName,    
                O.OfficeFileNo AS FileNo,    
                O.OfficeCode,    
                SM.PackingType,    
                SM.OtherPackingType,
                SM.PackingQty,    
                SM.DispatchMode,    
                SM.InvoiceNo,    
                @AddDate AS AddDate
            FROM SupplyMaster SM
            INNER JOIN IndentMaster IM ON SM.IndentID = IM.IndentID
            INNER JOIN Office O ON IM.OfficeID = O.OfficeID
            WHERE SM.SupplyID IN (" + supplyIDs + @")";

            SqlParameter[] param =
            {
        new SqlParameter("@AddDate", (object)adddate ?? DBNull.Value)
    };

            return db.ExecuteQuery(query, param);
        }

        public DataTable GetSupplyDemandDetailForMultiple(string supplyIDs)
        {
            // Only whole numbers are kept, so the list can never inject SQL
            supplyIDs = SanitizeIdList(supplyIDs);

            if (string.IsNullOrWhiteSpace(supplyIDs))
                return new DataTable();
            // supplyIDs misal ke taur par "1, 2, 5" ki form mein honge
            string query = @"SELECT    
                ROW_NUMBER() OVER(ORDER BY SD.SupplyDetailID) AS SrNo,    
                ((ROW_NUMBER() OVER(PARTITION BY SD.SupplyID ORDER BY SD.SupplyDetailID) - 1) / 20) + 1 AS PageNo,
                SD.SupplyID,    
                SM.SupplyNo,    
                SM.SupplyDate,
                IM.IndentNo,
                IM.IndentDate,
                SM.DispatchMode,
                SM.InvoiceNo,
                SM.PackingQty,
                SM.PackingType,
                SM.OtherPackingType,
                O.OfficeName,
                O.OfficeFileNo AS FileNo,
                SC.Name AS CategoryName,    
                D.Denomination,    
                SD.PiecesPerSheet,    
                SD.SupplyQty AS SupplySheets,    
                SD.LoosePieces AS SupplyPieces,    
                SD.LedgerFolio,    
                SD.CaseNoFrom,    
                SD.CaseNoTo,    
                SD.CaseCode
            FROM SupplyDetail SD
            INNER JOIN SupplyMaster SM ON SD.SupplyID = SM.SupplyID
            INNER JOIN IndentMaster IM ON SM.IndentID = IM.IndentID
            INNER JOIN Office O ON IM.OfficeID = O.OfficeID
            INNER JOIN StampCategory SC ON SD.CategoryID = SC.CategoryID
            INNER JOIN Denomination D ON SD.DenominationID = D.DenominationID
            WHERE SD.SupplyID IN (" + supplyIDs + @") AND D.Denomination > 0 
            ORDER BY SD.SupplyID, SD.SupplyDetailID";

            DataTable dt = db.ExecuteQuery(query, null);
            return dt;
        }

        public DataTable GetSupplyPerformaDetail(int supplyID)
        {
            string query = @"

SELECT

    ROW_NUMBER() OVER(ORDER BY SD.SupplyDetailID) AS SrNo,

    SC.Name AS CategoryName,

    D.Denomination,

    SD.PiecesPerSheet,

    SD.SupplyQty AS SupplySheets,

    SD.LoosePieces AS SupplyPieces,

    SD.LedgerFolio,

    SD.CaseNoFrom,
    SD.CaseNoTo,
    SD.CaseCode

FROM SupplyDetail SD

INNER JOIN StampCategory SC
    ON SD.CategoryID = SC.CategoryID

INNER JOIN Denomination D
    ON SD.DenominationID = D.DenominationID

WHERE SD.SupplyID = @SupplyID

ORDER BY SD.SupplyDetailID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            DataTable dt = db.ExecuteQuery(query, param);
            const int MinRows = 9;

            while (dt.Rows.Count < MinRows)
            {
                DataRow dr = dt.NewRow();

                dr["SrNo"] = DBNull.Value;
                dr["CategoryName"] = "";
                dr["Denomination"] = DBNull.Value;
                dr["PiecesPerSheet"] = DBNull.Value;
                dr["SupplySheets"] = DBNull.Value;
                dr["SupplyPieces"] = DBNull.Value;

                dt.Rows.Add(dr);
            }
            return dt;
        }


        private bool ValidateInvoiceFormat(
    int supplyID,
    out string invoiceNo)
        {
            invoiceNo = "";

            string query = @"
SELECT
    SM.InvoiceNo,
    SM.SupplyType,
    SM.FinancialYear,
    O.OfficeCode
FROM SupplyMaster SM
INNER JOIN IndentMaster IM
    ON SM.IndentID = IM.IndentID
INNER JOIN Office O
    ON IM.OfficeID = O.OfficeID
WHERE SM.SupplyID = @SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            DataTable dt = db.ExecuteQuery(query, param);

            if (dt.Rows.Count == 0)
                return false;

            invoiceNo = dt.Rows[0]["InvoiceNo"].ToString();

            if (string.IsNullOrWhiteSpace(invoiceNo))
                return false;

            int supplyType =
                Convert.ToInt32(dt.Rows[0]["SupplyType"]);

            string financialYear =
                dt.Rows[0]["FinancialYear"].ToString();

            string officeCode =
                dt.Rows[0]["OfficeCode"].ToString();

            // Public Postage & Stationery
            if (supplyType == 1003)
            {
                if (!invoiceNo.StartsWith("P-PP-"))
                    return false;

                if (!invoiceNo.Contains("/" + officeCode + "-"))
                    return false;

                if (!invoiceNo.EndsWith("/" + financialYear))
                    return false;
            }
            else
            {
                // Service
                if (!invoiceNo.StartsWith("P-SP-"))
                    return false;

                if (!invoiceNo.Contains("/" + financialYear))
                    return false;

                if (!invoiceNo.EndsWith("(" + officeCode + ")"))
                    return false;
            }

            return true;
        }
        private bool ValidateInvoiceSequence(
     int supplyID,
     string invoiceNo,
     bool isDraft)
        {
            string query = @"
SELECT
    SM.SupplyType,
    SM.FinancialYear,
    O.OfficeID,
    O.OfficeCode
FROM SupplyMaster SM
INNER JOIN IndentMaster IM
    ON SM.IndentID = IM.IndentID
INNER JOIN Office O
    ON IM.OfficeID = O.OfficeID
WHERE SM.SupplyID=@SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            DataTable dt = db.ExecuteQuery(query, param);

            if (dt.Rows.Count == 0)
                return false;

            int supplyType =
                Convert.ToInt32(dt.Rows[0]["SupplyType"]);

            string financialYear =
                dt.Rows[0]["FinancialYear"].ToString();

            int officeID =
                Convert.ToInt32(dt.Rows[0]["OfficeID"]);

            //===============================
            // Extract Global Sequence
            //===============================

            int globalSequence = 0;

            if (supplyType == 1003)
            {
                // P-PP-0001/LHR-0001/26-27

                string[] part = invoiceNo.Split('/');

                string first =
                    part[0].Replace("P-PP-", "");

                int.TryParse(first, out globalSequence);
            }
            else
            {
                // P-SP-0001/26-27(KHI)

                string[] part = invoiceNo.Split('/');

                string first =
                    part[0].Replace("P-SP-", "");

                int.TryParse(first, out globalSequence);
            }

            //===============================
            // Global Sequence Check
            //===============================

            // qualify StatusID with table alias to avoid ambiguous column errors
            string statusCondition =
    isDraft
    ? "SM.StatusID IN (1,2,3)"
    : "SM.StatusID IN (2,3)";

            string globalQuery = $@"
SELECT ISNULL(MAX(GlobalInvoiceSequence),0)
FROM SupplyMaster SM
WHERE
{statusCondition}
AND SM.FinancialYear=@FinancialYear
AND SM.SupplyType=@SupplyType
AND SM.SupplyID<>@SupplyID";

            SqlParameter[] globalParam =
{
    new SqlParameter("@FinancialYear", financialYear),
    new SqlParameter("@SupplyType", supplyType),
    new SqlParameter("@SupplyID", supplyID)
        };

            int maxGlobal =
                Convert.ToInt32(
                    db.ExecuteScalar(globalQuery, globalParam));
            if (isDraft)
            {
                return true;
            }

            int expectedGlobal = maxGlobal + 1;

            if (globalSequence != expectedGlobal)
            {
                return false;
            }

            //===============================
            // Office Sequence
            //===============================

            if (supplyType == 1003)
            {
                int officeSequence = 0;

                string[] part = invoiceNo.Split('/');

                string officePart = part[1];

                string[] officeSplit =
                    officePart.Split('-');

                int.TryParse(
                    officeSplit[1],
                    out officeSequence);

                string officeQuery = $@"
SELECT ISNULL(MAX(SM.OfficeInvoiceSequence),0)

FROM SupplyMaster SM

INNER JOIN IndentMaster IM
ON SM.IndentID = IM.IndentID

WHERE

{statusCondition}

AND IM.OfficeID=@OfficeID

AND SM.FinancialYear=@FinancialYear

AND SM.SupplyType=@SupplyType

AND SM.SupplyID<>@SupplyID";

                SqlParameter[] officeParam =
 {
    new SqlParameter("@OfficeID", officeID),
    new SqlParameter("@FinancialYear", financialYear),
    new SqlParameter("@SupplyType", supplyType),
    new SqlParameter("@SupplyID", supplyID)
            };

                int maxOffice =
                    Convert.ToInt32(
                        db.ExecuteScalar(officeQuery, officeParam));

                if (!isDraft)
                {
                    int expectedOffice = maxOffice + 1;

                    if (officeSequence != expectedOffice)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private bool ValidateDuplicateInvoice(
    int supplyID,
    string invoiceNo,
    bool isDraft)
        {
            string statusCondition =
    isDraft
    ? "StatusID IN (1,2,3)"
    : "StatusID IN (2,3)";

            if (string.IsNullOrWhiteSpace(invoiceNo))
                return false;

            string duplicateQuery = $@"
SELECT COUNT(*)
FROM SupplyMaster
WHERE
{statusCondition}
AND InvoiceNo=@InvoiceNo
AND SupplyID<>@SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@InvoiceNo", invoiceNo),
        new SqlParameter("@SupplyID", supplyID)
    };

            int count = Convert.ToInt32(
                db.ExecuteScalar(duplicateQuery, param));

            if (count > 0)
                return false;

            return true;
        }
        private bool ValidateInvoiceNo(
     int supplyID,
     bool isDraft,
     out string invoiceNo)
        {
            invoiceNo = "";

            if (!ValidateInvoiceFormat(supplyID, out invoiceNo))
                return false;

            if (!ValidateInvoiceSequence(supplyID, invoiceNo, isDraft))
                return false;

            if (!ValidateDuplicateInvoice(supplyID, invoiceNo, isDraft))
                return false;

            return true;
        }
        public string GenerateInvoiceNo(int supplyID, bool isDraft)
        {
            // qualify StatusID with alias SM so injected condition is unambiguous
            string statusCondition = isDraft
                ? "SM.StatusID IN (1,2,3)"
                : "SM.StatusID IN (2,3)";
            //====================================================
            // 1. Check if Invoice already exists
            //====================================================

            string validInvoice;

            if (ValidateInvoiceFormat(supplyID, out validInvoice))
            {
                if (ValidateInvoiceSequence(supplyID, validInvoice, isDraft))
                {
                    if (ValidateDuplicateInvoice(supplyID, validInvoice, isDraft))
                    {
                        return validInvoice;
                    }
                }
            }





            //====================================================
            // 2. Get Supply information
            //====================================================

            string infoQuery = @"
SELECT
    SM.SupplyType,
    SM.FinancialYear,
    O.OfficeID,
    O.OfficeCode
FROM SupplyMaster SM
INNER JOIN IndentMaster IM
    ON SM.IndentID = IM.IndentID
INNER JOIN Office O
    ON IM.OfficeID = O.OfficeID
WHERE SM.SupplyID = @SupplyID";

            SqlParameter[] infoParam =
 {
    new SqlParameter("@SupplyID", supplyID)
};

            DataTable dtInfo =
                db.ExecuteQuery(infoQuery, infoParam);


            if (dtInfo.Rows.Count == 0)
                throw new Exception("Supply record not found.");

            int supplyType =
    Convert.ToInt32(dtInfo.Rows[0]["SupplyType"]);



            string financialYear =
                dtInfo.Rows[0]["FinancialYear"].ToString();

            int officeID =
                Convert.ToInt32(dtInfo.Rows[0]["OfficeID"]);

            string officeCode =
                dtInfo.Rows[0]["OfficeCode"].ToString();



            //====================================================
            // 3. Get Global Sequence (the current supply is excluded)
            //====================================================

            string globalQuery = $@"
SELECT ISNULL(MAX(GlobalInvoiceSequence),0)
FROM SupplyMaster SM
WHERE
{statusCondition}
AND SM.FinancialYear = @FinancialYear
AND SM.SupplyType = @SupplyType
AND SM.SupplyID <> @SupplyID";

            SqlParameter[] globalParam =
            {
        new SqlParameter("@FinancialYear", financialYear),
        new SqlParameter("@SupplyType", supplyType),
        new SqlParameter("@SupplyID", supplyID)
    };

            object globalResult =
                db.ExecuteScalar(globalQuery, globalParam);

            int maxGlobal = 0;

            if (globalResult != null && globalResult != DBNull.Value)
            {
                maxGlobal = Convert.ToInt32(globalResult);
            }

            int globalSequence = maxGlobal + 1;
            //====================================================
            // 4. Get Office Sequence (Only Public)
            //====================================================

            int officeSequence = 0;

            if (supplyType == 1003)
            {
                string officeQuery = $@"

SELECT
ISNULL(MAX(SM.OfficeInvoiceSequence),0)

FROM SupplyMaster SM

INNER JOIN IndentMaster IM
ON SM.IndentID = IM.IndentID

WHERE

{statusCondition}

AND IM.OfficeID = @OfficeID

AND SM.SupplyType = @SupplyType

AND SM.FinancialYear = @FinancialYear

AND SM.SupplyID <> @SupplyID";
                SqlParameter[] officeParam =
                {
            new SqlParameter("@OfficeID", officeID),
          new SqlParameter("@SupplyType", supplyType),
            new SqlParameter("@FinancialYear", financialYear),
            new SqlParameter("@SupplyID", supplyID)
        };

                object officeResult =
                    db.ExecuteScalar(officeQuery, officeParam);

                officeSequence =
                    Convert.ToInt32(officeResult) + 1;
            }

            //====================================================
            // 5. Generate Invoice Number
            //====================================================

            string invoiceNo = "";

            if (supplyType == 1003)
            {
                invoiceNo =
                    $"P-PP-{officeCode}-{officeSequence:0000}/{financialYear}";
            }
            else
            {
                invoiceNo =
                    $"P-SP-{globalSequence:0000}/{financialYear}({officeCode})";
            }

            //====================================================
            // 6. Save Invoice Number
            //====================================================

            string updateQuery = @"
UPDATE SupplyMaster
SET
    GlobalInvoiceSequence = @GlobalSequence,
    OfficeInvoiceSequence = @OfficeSequence,
    InvoiceNo = @InvoiceNo
WHERE SupplyID = @SupplyID";

            SqlParameter[] updateParam =
            {
        new SqlParameter("@GlobalSequence", globalSequence),

       new SqlParameter("@OfficeSequence",
    supplyType == 1003
        ? (object)officeSequence
        : DBNull.Value),

        new SqlParameter("@InvoiceNo", invoiceNo),

        new SqlParameter("@SupplyID", supplyID)
    };

            db.ExecuteNonQuery(updateQuery, updateParam);

            return invoiceNo;
        }

        public bool InvoiceRegenerated { get; private set; }

        public string EnsureValidInvoice(int supplyID, bool isDraft)
        {
            string invoiceNo;

            if (!ValidateInvoiceFormat(supplyID, out invoiceNo) ||
                !ValidateInvoiceSequence(supplyID, invoiceNo, isDraft) ||
                !ValidateDuplicateInvoice(supplyID, invoiceNo, isDraft))
            {
                InvoiceRegenerated = true;
                return GenerateInvoiceNo(supplyID, isDraft);
            }

            InvoiceRegenerated = false;
            return invoiceNo;
        }

        public void ApproveSupply(int supplyID)
        {
            string query = @"
UPDATE SupplyMaster
SET StatusID = 2
WHERE SupplyID = @SupplyID";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            db.ExecuteNonQuery(query, param);
        }


        public int GetNextAvailableGlobalSequence(
    int supplyID,
    string financialYear)
        {
            // ==================================================
            // Find the FIRST available Global Sequence number.
            //
            // StatusID: 1 = Draft, 2 = Approved, 3 = Issued
            // All three statuses are treated as occupied.
            //
            // The currently opened SupplyID is excluded because
            // we are checking what number is available for THIS draft.
            // ==================================================

            string sql = @"
        SELECT MIN(N.NumberValue)
        FROM
        (
            SELECT TOP (10000)
                ROW_NUMBER() OVER
                (
                    ORDER BY (SELECT NULL)
                ) AS NumberValue
            FROM sys.all_objects A
            CROSS JOIN sys.all_objects B
        ) N
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM SupplyMaster SM
            WHERE SM.FinancialYear = @FinancialYear
              AND SM.StatusID IN (1, 2, 3)
              AND SM.SupplyID <> @SupplyID
              AND SM.GlobalSequence = N.NumberValue
        );";

            SqlParameter[] parameters =
            {
        new SqlParameter(
            "@FinancialYear",
            financialYear),

        new SqlParameter(
            "@SupplyID",
            supplyID)
    };

            object result =
                db.ExecuteScalar(sql, parameters);

            // If no number is found, start from 1.
            if (result == null || result == DBNull.Value)
                return 1;

            return Convert.ToInt32(result);
        }

        public int GetNextAvailableOfficeSequence(
    int supplyID,
    int officeID,
    string financialYear)
        {
            // ==================================================
            // Find the FIRST available Office Sequence number
            // for the selected Office.
            //
            // Draft + Approved + Issued are considered occupied.
            // Current SupplyID is excluded.
            // ==================================================

            string sql = @"
        SELECT MIN(N.NumberValue)
        FROM
        (
            SELECT TOP (10000)
                ROW_NUMBER() OVER
                (
                    ORDER BY (SELECT NULL)
                ) AS NumberValue
            FROM sys.all_objects A
            CROSS JOIN sys.all_objects B
        ) N
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM SupplyMaster SM
            INNER JOIN IndentMaster IM
                ON SM.IndentID = IM.IndentID

            WHERE SM.FinancialYear = @FinancialYear
              AND IM.OfficeID = @OfficeID
              AND SM.StatusID IN (1, 2, 3)
              AND SM.SupplyID <> @SupplyID
              AND SM.OfficeSequence = N.NumberValue
        );";

            SqlParameter[] parameters =
            {
        new SqlParameter(
            "@FinancialYear",
            financialYear),

        new SqlParameter(
            "@OfficeID",
            officeID),

        new SqlParameter(
            "@SupplyID",
            supplyID)
    };

            object result =
                db.ExecuteScalar(sql, parameters);

            if (result == null || result == DBNull.Value)
                return 1;

            return Convert.ToInt32(result);
        }

        public bool UpdateDraftSupplyNumber(
    int supplyID,
    int globalSequence,
    int officeSequence,
    string officeCode,
    string financialYear)
        {
            // ==================================================
            // Build the new Supply Number
            // Example: P-0002/KHIT-0001/26-27
            // ==================================================

            string newSupplyNo =
                $"P-{globalSequence:0000}/" +
                $"{officeCode}-{officeSequence:0000}/" +
                $"{financialYear}";


            // ==================================================
            // Update ONLY the currently opened Draft.
            // StatusID = 1 means Draft.
            // ==================================================

            string sql = @"
        UPDATE SupplyMaster
        SET
            SupplyNo = @SupplyNo,
            GlobalSequence = @GlobalSequence,
            OfficeSequence = @OfficeSequence,
            ModifiedDate = GETDATE()
        WHERE SupplyID = @SupplyID
          AND StatusID = 1";


            SqlParameter[] parameters =
            {
        new SqlParameter(
            "@SupplyNo",
            newSupplyNo),

        new SqlParameter(
            "@GlobalSequence",
            globalSequence),

        new SqlParameter(
            "@OfficeSequence",
            officeSequence),

        new SqlParameter(
            "@SupplyID",
            supplyID)
    };


            int rowsAffected =
                db.ExecuteNonQuery(
                    sql,
                    parameters);


            return rowsAffected > 0;
        }

        public SupplyNumberInfo GetSupplyNumberInfo(int supplyID)
        {
            string sql = @"
        SELECT
            SupplyID,
            GlobalSequence,
            OfficeSequence,
            SupplyNo,
            FinancialYear
        FROM SupplyMaster
        WHERE SupplyID = @SupplyID
          AND StatusID = 1";

            SqlParameter[] parameters =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            DataTable dt =
                db.ExecuteQuery(sql, parameters);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            SupplyNumberInfo info = new SupplyNumberInfo();

            info.GlobalSequence =
                Convert.ToInt32(row["GlobalSequence"]);

            info.OfficeSequence =
                Convert.ToInt32(row["OfficeSequence"]);

            info.FinancialYear =
                Convert.ToString(row["FinancialYear"]);

            info.SupplyNo =
                Convert.ToString(row["SupplyNo"]);

            return info;
        }


        // =====================================================================
        // NEW: transactional methods used by frmSupply
        // =====================================================================

        // -----------------------------------------------------------------
        // Small transaction helpers
        // -----------------------------------------------------------------

        private static int TxExec(SqlConnection con, SqlTransaction tx, string sql, params SqlParameter[] p)
        {
            using (SqlCommand cmd = new SqlCommand(sql, con, tx))
            {
                if (p != null && p.Length > 0) cmd.Parameters.AddRange(p);
                return cmd.ExecuteNonQuery();
            }
        }

        private static DataTable TxFill(SqlConnection con, SqlTransaction tx, string sql, params SqlParameter[] p)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, con, tx))
            {
                if (p != null && p.Length > 0) cmd.Parameters.AddRange(p);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd)) da.Fill(dt);
            }
            return dt;
        }

        private static int TxInt(object v)
        {
            return (v == null || v == DBNull.Value) ? 0 : Convert.ToInt32(v);
        }

        /// <summary>Keeps only digits and commas so an id list can never inject SQL.</summary>
        public static string SanitizeIdList(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return "";

            List<string> clean = new List<string>();
            foreach (string part in ids.Split(','))
            {
                int id;
                if (int.TryParse(part.Trim(), out id)) clean.Add(id.ToString());
            }
            return string.Join(",", clean.ToArray());
        }

        // -----------------------------------------------------------------
        // Writes SupplyDetail + StockTransaction(OUT) inside the caller's transaction
        // -----------------------------------------------------------------

        private static void WriteDetailsAndStock(
            SqlConnection con, SqlTransaction tx, int supplyID,
            List<SupplyLine> lines, List<StockLine> stock, string remarks)
        {
            TxExec(con, tx, "DELETE FROM SupplyDetail WHERE SupplyID = @SupplyID",
                new SqlParameter("@SupplyID", supplyID));

            string insertDetail = @"
INSERT INTO SupplyDetail
(
    SupplyID, IndentDetailID, CategoryID, DenominationID, SupplyQty, PiecesPerSheet,
    LoosePieces, TotalPieces, LedgerFolio, CaseNoFrom, CaseNoTo, CaseCode
)
VALUES
(
    @SupplyID, @IndentDetailID, @CategoryID, @DenominationID, @SupplyQty, @PiecesPerSheet,
    @LoosePieces, @TotalPieces, @LedgerFolio, @CaseNoFrom, @CaseNoTo, @CaseCode
)";

            foreach (SupplyLine l in lines)
            {
                TxExec(con, tx, insertDetail,
                    new SqlParameter("@SupplyID", supplyID),
                    new SqlParameter("@IndentDetailID", l.IndentDetailID),
                    new SqlParameter("@CategoryID", l.CategoryID),
                    new SqlParameter("@DenominationID", l.DenominationID),
                    new SqlParameter("@SupplyQty", l.SupplySheets),
                    new SqlParameter("@PiecesPerSheet", l.PiecesPerSheet),
                    new SqlParameter("@LoosePieces", l.LoosePieces),
                    new SqlParameter("@TotalPieces", l.TotalPieces),
                    new SqlParameter("@LedgerFolio", l.LedgerFolio ?? ""),
                    new SqlParameter("@CaseNoFrom", l.CaseNoFrom ?? ""),
                    new SqlParameter("@CaseNoTo", l.CaseNoTo ?? ""),
                    new SqlParameter("@CaseCode", l.CaseCode ?? ""));
            }

            TxExec(con, tx,
                "DELETE FROM StockTransaction WHERE SupplyID = @SupplyID AND TransactionType = 'OUT'",
                new SqlParameter("@SupplyID", supplyID));

            string insertStock = @"
INSERT INTO StockTransaction
(
    StockID, TransactionType, BoxQty, PacketQty, SheetQty, StampQty,
    TransactionDate, ReferenceType, ReferenceID, Remarks, SupplyID
)
SELECT
    S.StockID, 'OUT', @BoxQty, @PacketQty, @SheetQty, @StampQty,
    GETDATE(), 'Supply', @SupplyID, @Remarks, @SupplyID
FROM StockMaster S
WHERE S.CategoryID = @CategoryID
  AND S.DenominationID = @DenominationID";

            foreach (StockLine s in stock)
            {
                int rows = TxExec(con, tx, insertStock,
                    new SqlParameter("@BoxQty", s.BoxQty),
                    new SqlParameter("@PacketQty", s.PacketQty),
                    new SqlParameter("@SheetQty", s.SheetQty),
                    new SqlParameter("@StampQty", s.StampQty),
                    new SqlParameter("@SupplyID", supplyID),
                    new SqlParameter("@Remarks", remarks ?? ""),
                    new SqlParameter("@CategoryID", s.CategoryID),
                    new SqlParameter("@DenominationID", s.DenominationID));

                if (rows == 0)
                    throw new InvalidOperationException(
                        "No StockMaster record was found for category " + s.CategoryID +
                        " / denomination " + s.DenominationID + ".");
            }
        }

        // -----------------------------------------------------------------
        // NEW draft: header + details + stock entry in ONE transaction
        // -----------------------------------------------------------------

        public int CreateSupplyDraft(
            SupplyNumberInfo info,
            int indentID,
            int supplyTypeID,
            string dispatchMode,
            string packingType,
            string otherPackingType,
            int packingQty,
            string remarks,
            int indentStatusID,
            DateTime supplyDate,
            List<SupplyLine> lines,
            List<StockLine> stock)
        {
            string insertHeader = @"
INSERT INTO SupplyMaster
(
    SupplyNo, SupplyType, FinancialYear, SupplyDate, IndentID, StatusID, IndentStatusID,
    Remarks, CreatedDate, DispatchMode, PackingType, OtherPackingType, PackingQty,
    GlobalSequence, OfficeSequence
)
VALUES
(
    @SupplyNo, @SupplyType, @FinancialYear, @SupplyDate, @IndentID, 1, @IndentStatusID,
    @Remarks, GETDATE(), @DispatchMode, @PackingType, @OtherPackingType, @PackingQty,
    @GlobalSequence, @OfficeSequence
);

DECLARE @NewID INT = CAST(SCOPE_IDENTITY() AS INT);

UPDATE IndentMaster
SET IndentStatus = @IndentStatusID
WHERE IndentID = @IndentID;

SELECT @NewID;";

            int newID = 0;
            string error = null;

            bool ok = db.ExecuteTransaction((con, tx) =>
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(insertHeader, con, tx))
                    {
                        cmd.Parameters.AddWithValue("@SupplyNo", info.SupplyNo);
                        cmd.Parameters.AddWithValue("@SupplyType", supplyTypeID);
                        cmd.Parameters.AddWithValue("@FinancialYear", info.FinancialYear);
                        cmd.Parameters.AddWithValue("@SupplyDate", supplyDate);
                        cmd.Parameters.AddWithValue("@IndentID", indentID);
                        cmd.Parameters.AddWithValue("@IndentStatusID", indentStatusID);
                        cmd.Parameters.AddWithValue("@Remarks", remarks ?? "");
                        cmd.Parameters.AddWithValue("@DispatchMode", dispatchMode ?? "");
                        cmd.Parameters.AddWithValue("@PackingType", packingType ?? "");
                        cmd.Parameters.AddWithValue("@OtherPackingType", (object)otherPackingType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PackingQty", packingQty);
                        cmd.Parameters.AddWithValue("@GlobalSequence", info.GlobalSequence);
                        cmd.Parameters.AddWithValue("@OfficeSequence", info.OfficeSequence);

                        newID = TxInt(cmd.ExecuteScalar());
                    }

                    if (newID <= 0)
                        throw new InvalidOperationException("The draft header could not be created.");

                    WriteDetailsAndStock(con, tx, newID, lines, stock, remarks);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    throw;
                }
            });

            if (error != null) throw new InvalidOperationException(error);
            if (!ok || newID <= 0)
                throw new InvalidOperationException("The draft could not be saved. No changes were made.");

            return newID;
        }

        // -----------------------------------------------------------------
        // EXISTING draft / approved supply: header + details + stock in ONE transaction
        // -----------------------------------------------------------------

        public void UpdateSupplyDraft(
            int supplyID,
            int supplyTypeID,
            DateTime supplyDate,
            string dispatchMode,
            string packingType,
            string otherPackingType,
            int packingQty,
            string invoiceNo,
            string remarks,
            int indentStatusID,
            List<SupplyLine> lines,
            List<StockLine> stock)
        {
            string updateHeader = @"
UPDATE SupplyMaster
SET
    SupplyType       = @SupplyType,
    SupplyDate       = @SupplyDate,
    DispatchMode     = @DispatchMode,
    PackingType      = @PackingType,
    OtherPackingType = @OtherPackingType,
    PackingQty       = @PackingQty,
    InvoiceNo        = @InvoiceNo,
    Remarks          = @Remarks,
    IndentStatusID   = @IndentStatusID
WHERE SupplyID = @SupplyID
  AND StatusID IN (1, 2);";

            // Only a DRAFT moves the indent status at save time.
            // (An approved supply is re-evaluated by UpdateIndentStatusAfterSupply.)
            string updateIndent = @"
UPDATE IM
SET IM.IndentStatus = @IndentStatusID
FROM IndentMaster IM
INNER JOIN SupplyMaster SM ON SM.IndentID = IM.IndentID
WHERE SM.SupplyID = @SupplyID
  AND SM.StatusID = 1;";

            string error = null;

            bool ok = db.ExecuteTransaction((con, tx) =>
            {
                try
                {
                    int rows = TxExec(con, tx, updateHeader,
                        new SqlParameter("@SupplyType", supplyTypeID),
                        new SqlParameter("@SupplyDate", supplyDate),
                        new SqlParameter("@DispatchMode", dispatchMode ?? ""),
                        new SqlParameter("@PackingType", packingType ?? ""),
                        new SqlParameter("@OtherPackingType", (object)otherPackingType ?? DBNull.Value),
                        new SqlParameter("@PackingQty", packingQty),
                        new SqlParameter("@InvoiceNo", invoiceNo ?? ""),
                        new SqlParameter("@Remarks", remarks ?? ""),
                        new SqlParameter("@IndentStatusID", indentStatusID),
                        new SqlParameter("@SupplyID", supplyID));

                    if (rows != 1)
                        throw new InvalidOperationException(
                            "This supply can no longer be edited (it may have been issued by another user).");

                    TxExec(con, tx, updateIndent,
                        new SqlParameter("@IndentStatusID", indentStatusID),
                        new SqlParameter("@SupplyID", supplyID));

                    WriteDetailsAndStock(con, tx, supplyID, lines, stock, remarks);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    throw;
                }
            });

            if (error != null) throw new InvalidOperationException(error);
            if (!ok)
                throw new InvalidOperationException("The changes could not be saved. Nothing was changed.");
        }

        // -----------------------------------------------------------------
        // Detail rows for editing a saved supply.
        // Pending = indent quantity minus what OTHER approved/issued supplies already took.
        // -----------------------------------------------------------------

        public DataTable GetSupplyDetailsForEdit(int supplyID)
        {
            string query = @"
SELECT
    SD.IndentDetailID AS DetailID,
    SD.CategoryID,
    SD.DenominationID,
    SC.Name AS CategoryName,
    D.Denomination,

    ID.SheetQty   AS IndentSheets,
    ID.PieceQty   AS IndentLoosePieces,
    ID.TotalPieces AS IndentTotalPieces,

    CASE WHEN ID.TotalPieces - ISNULL(O.OtherSupplied, 0) < 0
         THEN 0
         ELSE ID.TotalPieces - ISNULL(O.OtherSupplied, 0) END AS OriginalPendingPieces,

    CASE WHEN ID.TotalPieces - ISNULL(O.OtherSupplied, 0) - SD.TotalPieces < 0
         THEN 0
         ELSE ID.TotalPieces - ISNULL(O.OtherSupplied, 0) - SD.TotalPieces END AS RemainingPieces,

    CASE WHEN ID.TotalPieces - ISNULL(O.OtherSupplied, 0) - SD.TotalPieces < 0
         THEN 0
         ELSE ID.TotalPieces - ISNULL(O.OtherSupplied, 0) - SD.TotalPieces END AS RemainingTotalPieces,

    SD.SupplyQty    AS SupplySheets,
    SD.LoosePieces  AS SupplyPieces,
    SD.PiecesPerSheet,
    SD.TotalPieces  AS SupplyTotalPieces,

    SD.LedgerFolio,
    SD.CaseCode,
    SD.CaseNoFrom,
    SD.CaseNoTo

FROM SupplyDetail SD
INNER JOIN IndentDetail ID ON SD.IndentDetailID = ID.DetailID
INNER JOIN StampCategory SC ON SD.CategoryID = SC.CategoryID
INNER JOIN Denomination D ON SD.DenominationID = D.DenominationID
OUTER APPLY
(
    SELECT SUM(SD2.TotalPieces) AS OtherSupplied
    FROM SupplyDetail SD2
    INNER JOIN SupplyMaster SM2 ON SD2.SupplyID = SM2.SupplyID
    WHERE SD2.IndentDetailID = SD.IndentDetailID
      AND SM2.StatusID IN (2, 3)
      AND SM2.SupplyID <> SD.SupplyID
) O
WHERE SD.SupplyID = @SupplyID
ORDER BY SD.IndentDetailID";

            return db.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@SupplyID", supplyID) });
        }

        // -----------------------------------------------------------------
        // Items where this supply + other approved/issued supplies exceed the indent
        // -----------------------------------------------------------------

        public DataTable GetOverSuppliedItems(int supplyID)
        {
            string query = @"
SELECT
    SC.Name AS CategoryName,
    D.Denomination,
    ID.TotalPieces AS IndentTotal,
    ISNULL(O.OtherSupplied, 0) AS AlreadySupplied,
    SD.TotalPieces AS ThisSupply
FROM SupplyDetail SD
INNER JOIN IndentDetail ID ON SD.IndentDetailID = ID.DetailID
INNER JOIN StampCategory SC ON SD.CategoryID = SC.CategoryID
INNER JOIN Denomination D ON SD.DenominationID = D.DenominationID
OUTER APPLY
(
    SELECT SUM(SD2.TotalPieces) AS OtherSupplied
    FROM SupplyDetail SD2
    INNER JOIN SupplyMaster SM2 ON SD2.SupplyID = SM2.SupplyID
    WHERE SD2.IndentDetailID = SD.IndentDetailID
      AND SM2.StatusID IN (2, 3)
      AND SM2.SupplyID <> SD.SupplyID
) O
WHERE SD.SupplyID = @SupplyID
  AND SD.TotalPieces + ISNULL(O.OtherSupplied, 0) > ID.TotalPieces";

            return db.ExecuteQuery(query, new SqlParameter[] { new SqlParameter("@SupplyID", supplyID) });
        }

        // -----------------------------------------------------------------
        // ISSUE: status change + stock deduction in ONE transaction.
        // - only an Approved supply can be issued (a double click / second user fails safely)
        // - stock can never go negative: if it is not enough, everything is rolled back
        // -----------------------------------------------------------------

        public void IssueSupplyWithStock(int supplyID)
        {
            string error = null;

            bool ok = db.ExecuteTransaction((con, tx) =>
            {
                try
                {
                    int rows = TxExec(con, tx,
                        "UPDATE SupplyMaster SET StatusID = 3 WHERE SupplyID = @SupplyID AND StatusID = 2",
                        new SqlParameter("@SupplyID", supplyID));

                    if (rows != 1)
                        throw new InvalidOperationException(
                            "Only an Approved supply can be issued. It may already have been issued.");

                    DataTable lines = TxFill(con, tx, @"
SELECT ST.StockID, ST.BoxQty, ST.PacketQty, ST.SheetQty, ST.StampQty
FROM StockTransaction ST
WHERE ST.SupplyID = @SupplyID
  AND ST.TransactionType = 'OUT'",
                        new SqlParameter("@SupplyID", supplyID));

                    if (lines.Rows.Count == 0)
                        throw new InvalidOperationException(
                            "This supply has no Stock Entry. Open it, review the Stock Entry and save the changes before issuing.");

                    foreach (DataRow line in lines.Rows)
                    {
                        int stockID = TxInt(line["StockID"]);
                        int reqBox = TxInt(line["BoxQty"]);
                        int reqPacket = TxInt(line["PacketQty"]);
                        int reqSheet = TxInt(line["SheetQty"]);
                        int reqStamp = TxInt(line["StampQty"]);

                        // Lock the stock row for the duration of the transaction
                        DataTable bal = TxFill(con, tx, @"
SELECT S.CategoryID, S.DenominationID, S.BoxQty, S.PacketQty, S.SheetQty, S.StampQty,
       C.Name AS CategoryName, D.Denomination
FROM StockMaster S WITH (UPDLOCK, ROWLOCK)
INNER JOIN StampCategory C ON S.CategoryID = C.CategoryID
INNER JOIN Denomination D ON S.DenominationID = D.DenominationID
WHERE S.StockID = @StockID",
                            new SqlParameter("@StockID", stockID));

                        if (bal.Rows.Count == 0)
                            throw new InvalidOperationException("Stock record " + stockID + " was not found.");

                        DataRow b = bal.Rows[0];
                        int categoryID = TxInt(b["CategoryID"]);
                        int denominationID = TxInt(b["DenominationID"]);
                        int box = TxInt(b["BoxQty"]);
                        int packet = TxInt(b["PacketQty"]);
                        int sheet = TxInt(b["SheetQty"]);
                        int stamp = TxInt(b["StampQty"]);

                        int availBox = box, availPacket = packet, availSheet = sheet, availStamp = stamp;

                        int packetsPerBox = 0, sheetsPerPacket = 0, piecesPerSheet = 0;

                        DataTable conv = TxFill(con, tx, @"
SELECT PacketsPerBox, SheetsPerPacket, PiecesPerSheet
FROM UnitConversionMaster
WHERE CategoryID = @CategoryID AND DenominationID = @DenominationID",
                            new SqlParameter("@CategoryID", categoryID),
                            new SqlParameter("@DenominationID", denominationID));

                        if (conv.Rows.Count > 0)
                        {
                            packetsPerBox = TxInt(conv.Rows[0]["PacketsPerBox"]);
                            sheetsPerPacket = TxInt(conv.Rows[0]["SheetsPerPacket"]);
                            piecesPerSheet = TxInt(conv.Rows[0]["PiecesPerSheet"]);
                        }

                        if (!StockMath.TryDeduct(
                                ref box, ref packet, ref sheet, ref stamp,
                                reqBox, reqPacket, reqSheet, reqStamp,
                                packetsPerBox, sheetsPerPacket, piecesPerSheet))
                        {
                            throw new InvalidOperationException(string.Format(
                                "Insufficient stock for {0} Rs.{1}.\n\nAvailable: Box {2}, Packet {3}, Sheet {4}, Stamp {5}\nRequired: Box {6}, Packet {7}, Sheet {8}, Stamp {9}",
                                Convert.ToString(b["CategoryName"]),
                                Convert.ToDecimal(b["Denomination"]).ToString("0.##"),
                                availBox, availPacket, availSheet, availStamp,
                                reqBox, reqPacket, reqSheet, reqStamp));
                        }

                        TxExec(con, tx, @"
UPDATE StockMaster
SET BoxQty = @Box, PacketQty = @Packet, SheetQty = @Sheet, StampQty = @Stamp, ModifiedDate = GETDATE()
WHERE StockID = @StockID",
                            new SqlParameter("@Box", box),
                            new SqlParameter("@Packet", packet),
                            new SqlParameter("@Sheet", sheet),
                            new SqlParameter("@Stamp", stamp),
                            new SqlParameter("@StockID", stockID));
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    throw;
                }
            });

            if (error != null) throw new InvalidOperationException(error);
            if (!ok)
                throw new InvalidOperationException("The supply could not be issued. No changes were made.");
        }
    }
}