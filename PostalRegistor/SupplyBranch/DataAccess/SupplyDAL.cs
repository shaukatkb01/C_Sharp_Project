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
    internal class SupplyDAL
    {
        private readonly DBHelper db = new DBHelper();

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

        public bool DeleteDraftSupply(int supplyID)
        {
            string query = @"

DELETE FROM SupplyDetail
WHERE SupplyID=@SupplyID
AND EXISTS
(
    SELECT 1
    FROM SupplyMaster
    WHERE SupplyID=@SupplyID
    AND StatusID=1
);

DELETE FROM SupplyMaster
WHERE SupplyID=@SupplyID
AND StatusID=1";

            SqlParameter[] p =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            return db.ExecuteNonQuery(query, p) > 0;
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
    1,                  -- Supply = Draft
    @IndentStatusID,    -- 5 Open / 6 Partial / 7 Closed
    @Remarks,
    GETDATE(),
    @DispatchMode,
    @PackingType,
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

        public void InsertSupplyDetail(
    int supplyID,
    int categoryID,
    int denominationID,
    int supplySheets,
    int supplyPieces,
    int totalSupplyQty,
    string ledgerFolio,
    string caseNoFrom,
    string caseNoTo,
    string caseCode)
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
)";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID),
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID),
        new SqlParameter("@SupplySheets", supplySheets),
        new SqlParameter("@SupplyPieces", supplyPieces),
        new SqlParameter("@TotalSupplyQty", totalSupplyQty),
        new SqlParameter("@LedgerFolio", ledgerFolio),
        new SqlParameter("@CaseNoFrom", caseNoFrom),
        new SqlParameter("@CaseNoTo", caseNoTo),
        new SqlParameter("@CaseCode", caseCode)
    };

            db.ExecuteNonQuery(query, param);
        }


        public void UpdateSupplyMaster(
      int supplyID,
      int supplyTypeID,
      DateTime supplyDate,
      string dispatchMode,
      string packingType,
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


        public void IssueSupply(int supplyID)
        {
            string query = @"

UPDATE SupplyMaster

SET
    StatusID = 3

WHERE SupplyID = @SupplyID";

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
                sql.Append(" AND SM.SupplyDate<=@ToDate");

                param.Add(
                    new SqlParameter("@ToDate",
                    toDate.Value.Date));
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

            // پہلے کوئی Draft موجود ہے جس کا
            // GlobalInvoiceSequence ابھی NULL ہے
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

    // Current Supply موجود نہیں یا Draft نہیں
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
        new SqlParameter("@PackingQty", packingQty),
        new SqlParameter("@Remarks", remarks),
        new SqlParameter("@GlobalSequence", info.GlobalSequence),
        new SqlParameter("@OfficeSequence", info.OfficeSequence)
    };

            db.ExecuteNonQuery(query, param);
        }

        public void CancelSupply(int supplyID)
        {
            string query = @"

UPDATE SupplyMaster

SET
    StatusID = 4

WHERE
    SupplyID = @SupplyID
    AND StatusID = 1";

            SqlParameter[] param =
            {
        new SqlParameter("@SupplyID", supplyID)
    };

            db.ExecuteNonQuery(query, param);
        }

        public DataTable GetSupplyPerformaHeader(int supplyID)
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
    SM.PackingQty,
    SM.DispatchMode,
    SM.InvoiceNo

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

            return db.ExecuteQuery(query, param);
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
            // 3. Get Global Sequence
            //====================================================

            string globalQuery = $@"
SELECT ISNULL(MAX(GlobalInvoiceSequence),0)
FROM SupplyMaster SM
WHERE
{statusCondition}
AND SM.FinancialYear = @FinancialYear
AND SM.SupplyType = @SupplyType";

            SqlParameter[] globalParam =
            {
        new SqlParameter("@FinancialYear", financialYear),
        new SqlParameter("@SupplyType", supplyType)
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

            if(supplyType == 1003)
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

AND SM.FinancialYear = @FinancialYear";
                SqlParameter[] officeParam =
                {
            new SqlParameter("@OfficeID", officeID),
          new SqlParameter("@SupplyType", supplyType),
            new SqlParameter("@FinancialYear", financialYear)
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
            string invoiceNo = "";

            if (!ValidateInvoiceSequence(supplyID, invoiceNo, isDraft))
            {
                MessageBox.Show("Sequence Invalid");

                InvoiceRegenerated = true;

                return GenerateInvoiceNo(supplyID, isDraft);
            }

            if (!ValidateInvoiceFormat(supplyID, out invoiceNo))
            {
                InvoiceRegenerated = true;
                return GenerateInvoiceNo(supplyID, isDraft);
            }

            if (!ValidateInvoiceSequence(supplyID, invoiceNo, isDraft))
            {
                InvoiceRegenerated = true;
                return GenerateInvoiceNo(supplyID, isDraft);
            }

            if (!ValidateDuplicateInvoice(supplyID, invoiceNo, isDraft))
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
            // StatusID:
            // 1 = Draft
            // 2 = Approved
            // 3 = Issued
            //
            // All three statuses are treated as occupied.
            //
            // IMPORTANT:
            // The currently opened SupplyID is excluded because
            // we are checking what number is available for THIS
            // Draft.
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

            // ==================================================
            // If no number is found, start from 1.
            // ==================================================

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
            //
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
            // Example:
            // P-0002/KHIT-0001/26-27
            // ==================================================

            string newSupplyNo =
                $"P-{globalSequence:0000}/" +
                $"{officeCode}-{officeSequence:0000}/" +
                $"{financialYear}";


            // ==================================================
            // Update ONLY the currently opened Draft.
            //
            // StatusID = 1 means Draft.
            // This prevents an Approved/Issued record from
            // being changed accidentally.
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

    }


}

