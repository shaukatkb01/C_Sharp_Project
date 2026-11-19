using SupplyBranch.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace SupplyBranch.DataAccess
{
    public class SupplyDAL
    {
        private readonly DBHelper _db;

        public SupplyDAL(DBHelper db)
        {
            _db = db;
        }

        public void UpdateIndentStatusAfterSupply(int supplyID, int indentID)
        {
            string sql = @"
DECLARE @IndentTotalPieces INT;
DECLARE @TotalSupplyPieces INT;
DECLARE @NewIndentStatusID INT;
DECLARE @NewStatusID INT;

SELECT
    @IndentTotalPieces = ISNULL(SUM(ID.TotalPieces), 0)
FROM IndentDetail ID
WHERE ID.IndentID = @IndentID;

SELECT
    @TotalSupplyPieces = ISNULL(SUM(SD.TotalPieces), 0)
FROM SupplyDetail SD
INNER JOIN SupplyMaster SM
    ON SD.SupplyID = SM.SupplyID
WHERE SM.IndentID = @IndentID
  AND SM.StatusID IN (2,3);

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

UPDATE IndentMaster
SET IndentStatus = @NewIndentStatusID,
    StatusID = @NewStatusID
WHERE IndentID = @IndentID;

UPDATE SupplyMaster
SET IndentStatusID = @NewIndentStatusID
WHERE SupplyID = @SupplyID;
";

            SqlParameter[] parameters =
            {
                new SqlParameter("@SupplyID", supplyID),
                new SqlParameter("@IndentID", indentID)
            };

            _db.ExecuteNonQuery(sql, parameters);
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

            object? result = _db.ExecuteScalar(sql, parameters);

            return Convert.ToInt32(result ?? 0);
        }

        public bool DeleteDraftSupply(int supplyID)
        {
            string query = @"
DELETE FROM StockTransaction 
WHERE SupplyID = @SupplyID;

DELETE FROM SupplyDetail 
WHERE SupplyID = @SupplyID 
  AND EXISTS (
      SELECT 1 
      FROM SupplyMaster 
      WHERE SupplyID = @SupplyID 
        AND StatusID = 1
  );

DELETE FROM SupplyMaster 
WHERE SupplyID = @SupplyID 
  AND StatusID = 1;
";

            SqlParameter[] p =
            {
                new SqlParameter("@SupplyID", supplyID)
            };

            return _db.ExecuteNonQuery(query, p) > 0;
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

            return _db.ExecuteQuery(sql, parameters);
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
    ID.TotalPieces AS OriginalPendingPieces,
    (ID.TotalPieces - ISNULL(SD.TotalPieces,0)) AS RemainingTotalPieces,
    SD.SupplyQty AS SupplySheets,
    SD.LoosePieces AS SupplyPieces,
    SD.PiecesPerSheet,
    SD.TotalPieces AS SupplyTotalPieces,
    SD.LedgerFolio,
    SD.CaseCode,
    SD.CaseNoFrom,
    SD.CaseNoTo
FROM SupplyDetail SD
INNER JOIN IndentDetail ID ON SD.IndentDetailID = ID.DetailID
INNER JOIN StampCategory SC ON SD.CategoryID = SC.CategoryID
INNER JOIN Denomination D ON SD.DenominationID = D.DenominationID
WHERE SD.SupplyID=@SupplyID
ORDER BY SD.IndentDetailID";

            SqlParameter[] param = { new SqlParameter("@SupplyID", supplyID) };

            return _db.ExecuteQuery(query, param);
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

            object result = _db.ExecuteScalar(query, param);

            return Convert.ToInt32(result == null || result == DBNull.Value ? 0 : result);
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

            object result = _db.ExecuteScalar(query, param);

            return Convert.ToInt32(result == null || result == DBNull.Value ? 0 : result);
        }

        public (int OfficeID, string OfficeCode) GetOfficeInfo(int indentID)
        {
            string query = @"\n    SELECT O.OfficeID, O.OfficeCode\n    FROM IndentMaster IM\n    INNER JOIN Office O\n        ON IM.OfficeID = O.OfficeID\n    WHERE IM.IndentID=@IndentID";

            SqlParameter[] param =
            {
                new SqlParameter("@IndentID", indentID)
            };

            DataTable dt = _db.ExecuteQuery(query, param);

            if (dt.Rows.Count == 0)
                return (0, string.Empty);

            return (
                Convert.ToInt32(dt.Rows[0]["OfficeID"]),
                dt.Rows[0]["OfficeCode"].ToString()
            );
        }

        public DataTable GetSupplyTypes()
        {
            string query = @"
        SELECT
            SupplyTypeID,
            SupplyTypeName
        FROM SupplyType
        ORDER BY SupplyTypeName";

            return _db.ExecuteQuery(query, null);
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

            _db.ExecuteNonQuery(query, param);
        }

        public int GetLastApprovedGlobalNumber(string financialYear)
        {
            string query = @"\n\nSELECT ISNULL(MAX(GlobalSequence),0)\n\nFROM SupplyMaster\n\nWHERE FinancialYear=@FinancialYear\nAND StatusID IN (2,3)";

            SqlParameter[] param =
            {
                new SqlParameter("@FinancialYear", financialYear)
            };

            object result = _db.ExecuteScalar(query, param);

            return Convert.ToInt32(result == null || result == DBNull.Value ? 0 : result);
        }

        public int GetLastApprovedOfficeNumber(int officeID, string financialYear)
        {
            string query = @"\nSELECT ISNULL(MAX(SM.OfficeSequence),0)\nFROM SupplyMaster SM\nINNER JOIN IndentMaster IM\n    ON SM.IndentID = IM.IndentID\nWHERE IM.OfficeID = @OfficeID\n  AND SM.FinancialYear = @FinancialYear\n  AND SM.StatusID IN (2,3)";

            SqlParameter[] param =
            {
                new SqlParameter("@OfficeID", officeID),
                new SqlParameter("@FinancialYear", financialYear)
            };

            object result = _db.ExecuteScalar(query, param);

            return Convert.ToInt32(result == null || result == DBNull.Value ? 0 : result);
        }

        public void InsertSupplyDetail(int supplyID, int categoryID, int denominationID, int supplySheets, int supplyPieces, int totalSupplyQty, string ledgerFolio, string caseNoFrom, string caseNoTo, string caseCode)
        {
            string query = @"
INSERT INTO SupplyDetail
(
    SupplyID,
    CategoryID,
    DenominationID,
    SupplySheets,
    SupplyPieces,
    TotalSupplyQty,
    LedgerFolio,
    CaseNoFrom,
    CaseNoTo,
    CaseCode
)
VALUES
(
    @SupplyID,
    @CategoryID,
    @DenominationID,
    @SupplySheets,
    @SupplyPieces,
    @TotalSupplyQty,
    @LedgerFolio,
    @CaseNoFrom,
    @CaseNoTo,
    @CaseCode
)
";

            SqlParameter[] param =
            {
                new SqlParameter("@SupplyID", supplyID),
                new SqlParameter("@CategoryID", categoryID),
                new SqlParameter("@DenominationID", denominationID),
                new SqlParameter("@SupplySheets", supplySheets),
                new SqlParameter("@SupplyPieces", supplyPieces),
                new SqlParameter("@TotalSupplyQty", totalSupplyQty),
                new SqlParameter("@LedgerFolio", ledgerFolio ?? string.Empty),
                new SqlParameter("@CaseNoFrom", caseNoFrom ?? string.Empty),
                new SqlParameter("@CaseNoTo", caseNoTo ?? string.Empty),
                new SqlParameter("@CaseCode", caseCode ?? string.Empty)
            };

            _db.ExecuteNonQuery(query, param);
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
SM.PackingQty,
SM.Remarks,
SM.NeedReprint,
SM.IndentID,
IM.IndentNo,
IM.IndentDate,
O.OfficeName,
SS.StatusName
FROM SupplyMaster SM
INNER JOIN IndentMaster IM ON SM.IndentID=IM.IndentID
INNER JOIN Office O ON IM.OfficeID=O.OfficeID
INNER JOIN Status SS ON SM.StatusID=SS.StatusID
WHERE SM.SupplyID=@SupplyID
";

            SqlParameter[] param = { new SqlParameter("@SupplyID", supplyID) };

            return _db.ExecuteQuery(query, param);
        }

        public void ApproveSupply(int supplyID)
        {
            string query = @"UPDATE SupplyMaster SET StatusID = 2 WHERE SupplyID = @SupplyID";

            SqlParameter[] param = { new SqlParameter("@SupplyID", supplyID) };

            _db.ExecuteNonQuery(query, param);
        }

        public int SaveDraftSupply(
            SupplyNumberInfo info,
            int indentID,
            int supplyTypeID,
            string dispatchMode,
            string packingType,
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
    1,
    @IndentStatusID,
    @Remarks,
    GETDATE(),
    @DispatchMode,
    @PackingType,
    @PackingQty,
    @GlobalSequence,
    @OfficeSequence
);

SELECT CAST(SCOPE_IDENTITY() AS INT);
";

            SqlParameter[] param =
            {
                new SqlParameter("@SupplyNo", info.SupplyNo),
                new SqlParameter("@SupplyType", supplyTypeID),
                new SqlParameter("@FinancialYear", info.FinancialYear),
                new SqlParameter("@SupplyDate", supplyDate),
                new SqlParameter("@IndentID", indentID),
                new SqlParameter("@IndentStatusID", indentStatusID),
                new SqlParameter("@Remarks", remarks ?? string.Empty),
                new SqlParameter("@DispatchMode", dispatchMode ?? string.Empty),
                new SqlParameter("@PackingType", packingType ?? string.Empty),
                new SqlParameter("@PackingQty", packingQty),
                new SqlParameter("@GlobalSequence", info.GlobalSequence),
                new SqlParameter("@OfficeSequence", info.OfficeSequence)
            };

            object? result = _db.ExecuteScalar(query, param);

            return Convert.ToInt32(result ?? 0);
        }
    }
}
