using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SupplyBranch.DataAccess
{
    public class IndentDAL
    {
        private readonly DBHelper _db;

        public IndentDAL(DBHelper db)
        {
            _db = db;
        }

        public bool Exists(string indentNo)
        {
            string query = @"SELECT COUNT(*)
                     FROM IndentMaster
                     WHERE IndentNo=@IndentNo";

            SqlParameter[] parameters =
            {
                new SqlParameter("@IndentNo", indentNo)
            };

            return Convert.ToInt32(_db.ExecuteScalar(query, parameters)) > 0;
        }

        public bool Exists(string indentNo, int indentID)
        {
            string query = @"
        SELECT COUNT(*)
        FROM IndentMaster
        WHERE IndentNo = @IndentNo
          AND IndentID <> @IndentID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@IndentNo", indentNo),
                new SqlParameter("@IndentID", indentID)
            };

            int count = Convert.ToInt32(_db.ExecuteScalar(query, parameters));

            return count > 0;
        }

        public string GetIndentStatus(int indentID)
        {
            string query = @"
        SELECT S.StatusName 
        FROM IndentMaster IM
        INNER JOIN Status S ON IM.IndentStatus = S.StatusID
        WHERE IM.IndentID = @IndentID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@IndentID", indentID)
            };

            object result = _db.ExecuteScalar(query, parameters);

            return (result != null && result != DBNull.Value) ? result.ToString() : string.Empty;
        }

        public DataTable GetZones()
        {
            string query = @"SELECT ZoneID, ZoneName
                             FROM OfficeZone
                             ORDER BY ZoneName";

            return _db.ExecuteQuery(query);
        }

        public DataTable GetOffices(int zoneID)
        {
            string query = @"SELECT OfficeID, OfficeName
                             FROM Office
                             WHERE ZoneID=@ZoneID
                             ORDER BY OfficeName";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ZoneID", zoneID)
            };

            return _db.ExecuteQuery(query, parameters);
        }

        public DataTable GetCategories()
        {
            string query = @"SELECT CategoryID, Name
                             FROM StampCategory
                             ORDER BY Name";

            return _db.ExecuteQuery(query);
        }

        public DataTable GetDenominations(int categoryID)
        {
            string query = @"
                        SELECT
                            UCM.DenominationID,
        'Rs. ' + FORMAT(D.Denomination, '0.##') + '/-' AS DisplayDenomination,                           
                            UCM.PiecesPerSheet
                        FROM UnitConversionMaster UCM
                        INNER JOIN Denomination D
                            ON UCM.DenominationID = D.DenominationID
                        WHERE UCM.CategoryID = @CategoryID
                        ORDER BY D.Denomination";

            SqlParameter[] parameters =
            {
                new SqlParameter("@CategoryID", categoryID)
            };

            return _db.ExecuteQuery(query, parameters);
        }

        public int SaveIndent(IndentMasterModel master, DataTable dtItems)
        {
            using (SqlConnection con = _db.GetConnection())
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    string masterQuery = @"
                INSERT INTO IndentMaster
                (
                    IndentNo,
                    IndentDate,
                    OfficeID,
                    Remarks
                )
                VALUES
                (
                    @IndentNo,
                    @IndentDate,
                    @OfficeID,
                    @Remarks
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    SqlParameter[] masterParameters =
                    {
                        new SqlParameter("@IndentNo", master.IndentNo),
                        new SqlParameter("@IndentDate", master.IndentDate),
                        new SqlParameter("@OfficeID", master.OfficeID),
                        new SqlParameter("@Remarks",
                            string.IsNullOrWhiteSpace(master.Remarks)
                            ? (object)DBNull.Value
                            : master.Remarks)
                    };

                    SqlCommand masterCmd = new SqlCommand(masterQuery, con, tran);

                    masterCmd.Parameters.AddRange(masterParameters);

                    int indentID = Convert.ToInt32(masterCmd.ExecuteScalar());

                    foreach (DataRow row in dtItems.Rows)
                    {
                        string detailQuery = @"
                            INSERT INTO IndentDetail
                                            (
                                                IndentID,
                                                CategoryID,
                                                DenominationID,
                                                SheetQty,
                                                PieceQty,
                                                PiecesPerSheet,
                                                TotalPieces
                                            )
                                            VALUES
                                            (
                                                @IndentID,
                                                @CategoryID,
                                                @DenominationID,
                                                @SheetQty,
                                                @PieceQty,
                                                @PiecesPerSheet,
                                                @TotalPieces
                                            )";

                        SqlParameter[] detailParameters =
                        {
                            new SqlParameter("@IndentID", indentID),
                            new SqlParameter("@CategoryID", Convert.ToInt32(row["CategoryID"])),
                            new SqlParameter("@DenominationID", Convert.ToInt32(row["DenominationID"])),
                            new SqlParameter("@SheetQty", Convert.ToInt32(row["SheetQty"])),
                            new SqlParameter("@PieceQty", Convert.ToInt32(row["PieceQty"])),
                            new SqlParameter("@PiecesPerSheet", Convert.ToInt32(row["PiecesPerSheet"])),
                            new SqlParameter("@TotalPieces", Convert.ToInt32(row["TotalPieces"]))
                        };

                        SqlCommand detailCmd = new SqlCommand(detailQuery, con, tran);

                        detailCmd.Parameters.AddRange(detailParameters);

                        detailCmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                    return indentID;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public DataTable Search(int? zoneID, int? officeID, DateTime? fromDate, DateTime? toDate, string indentNo)
        {
            string query = @"
SELECT
    IM.IndentID,
    IM.IndentNo,
    IM.IndentDate,
    O.OfficeName,
    COUNT(ID.DetailID) AS TotalItems,
    IM.Remarks
FROM IndentMaster IM
INNER JOIN Office O
    ON IM.OfficeID = O.OfficeID
INNER JOIN OfficeZone OZ
    ON O.ZoneID = OZ.ZoneID
INNER JOIN IndentDetail ID
    ON IM.IndentID = ID.IndentID
WHERE 1=1 ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (zoneID.HasValue)
            {
                query += " AND OZ.ZoneID=@ZoneID";
                parameters.Add(new SqlParameter("@ZoneID", zoneID.Value));
            }

            if (officeID.HasValue)
            {
                query += " AND O.OfficeID=@OfficeID";
                parameters.Add(new SqlParameter("@OfficeID", officeID.Value));
            }

            if (!string.IsNullOrWhiteSpace(indentNo))
            {
                query += " AND IM.IndentNo LIKE @IndentNo";
                parameters.Add(new SqlParameter("@IndentNo", "%" + indentNo + "%"));
            }

            if (fromDate.HasValue)
            {
                query += " AND IM.IndentDate>=@FromDate";
                parameters.Add(new SqlParameter("@FromDate", fromDate.Value.Date));
            }

            if (toDate.HasValue)
            {
                query += " AND IM.IndentDate<DATEADD(day,1,@ToDate)";
                parameters.Add(new SqlParameter("@ToDate", toDate.Value.Date));
            }

            query += @"
                        GROUP BY
                            IM.IndentID,
                            IM.IndentNo,
                            IM.IndentDate,
                            O.OfficeName,
                            IM.Remarks

                        ORDER BY IM.IndentDate DESC";

            return _db.ExecuteQuery(query, parameters.ToArray());
        }

        public DataRow GetIndentHeader(int indentID)
        {
            string query = @"
    SELECT
        IM.IndentID,
        IM.IndentNo,
        IM.IndentDate,
        O.ZoneID,
        IM.OfficeID,
        IM.Remarks
    FROM IndentMaster IM
    INNER JOIN Office O
        ON IM.OfficeID = O.OfficeID
    WHERE IM.IndentID=@IndentID";

            SqlParameter[] parameters = { new SqlParameter("@IndentID", indentID) };

            DataTable dt = _db.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return dt.Rows[0];

            return null;
        }

        public DataTable GetIndentDetails(int indentID)
        {
            string query = @"
SELECT
    ID.DetailID,
    ID.CategoryID,
    SC.Name AS Category,
    ID.DenominationID,
    D.Denomination,
    ID.SheetQty,
    ID.PieceQty,
    ID.PiecesPerSheet,
    ID.TotalPieces
FROM IndentDetail ID
INNER JOIN StampCategory SC
    ON ID.CategoryID = SC.CategoryID
INNER JOIN Denomination D
    ON ID.DenominationID = D.DenominationID
WHERE ID.IndentID=@IndentID
ORDER BY ID.DetailID";

            SqlParameter[] parameters = { new SqlParameter("@IndentID", indentID) };

            return _db.ExecuteQuery(query, parameters);
        }

        public bool DeleteIndent(int indentID)
        {
            using (SqlConnection con = _db.GetConnection())
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    string query = @"
                DELETE FROM IndentDetail
                WHERE IndentID=@IndentID";

                    SqlCommand cmd = new SqlCommand(query, con, tran);

                    cmd.Parameters.AddWithValue("@IndentID", indentID);

                    cmd.ExecuteNonQuery();

                    string query2 = @"
                        DELETE FROM IndentMaster
                        WHERE IndentID=@IndentID";

                    SqlCommand cmd2 = new SqlCommand(query2, con, tran);

                    cmd2.Parameters.AddWithValue("@IndentID", indentID);

                    cmd2.ExecuteNonQuery();

                    tran.Commit();

                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
