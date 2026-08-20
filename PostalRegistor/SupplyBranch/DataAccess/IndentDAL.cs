using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace SupplyBranch.DAL
{
    public class IndentDAL
    {
        DBHelper db = new DBHelper();

        public bool Exists(string indentNo)
        {
            string query = @"SELECT COUNT(*)
                     FROM IndentMaster
                     WHERE IndentNo=@IndentNo";

            SqlParameter[] parameters =
            {
        new SqlParameter("@IndentNo", indentNo)
    };

            return Convert.ToInt32(db.ExecuteScalar(query, parameters)) > 0;
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

            int count = Convert.ToInt32(
                db.ExecuteScalar(query, parameters));

            return count > 0;
        }

        public string GetIndentStatus(int indentID)
        {
            // IM se StatusID le kar S (Status Table) se Status Name laya gaya hai
            string query = @"
        SELECT S.StatusName 
        FROM IndentMaster IM
        INNER JOIN Status S ON IM.IndentStatus = S.StatusID
        WHERE IM.IndentID = @IndentID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@IndentID", indentID)
    };

            object result = db.ExecuteScalar(query, parameters);

            // Agar Status Name mil jaye to return karein, warna empty string
            return (result != null && result != DBNull.Value) ? result.ToString() : string.Empty;
        }

        public DataTable GetZones()
        {
            string query = @"SELECT ZoneID, ZoneName
                             FROM OfficeZone
                             ORDER BY ZoneName";

            return db.GetDataTable(query);
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

            return db.ExecuteQuery(query, parameters);
        }

        public DataTable GetCategories()
        {
            string query = @"SELECT CategoryID, Name
                             FROM StampCategory
                             ORDER BY Name";

            return db.GetDataTable(query);
        }

        public DataTable GetDenominations(int categoryID)
        {
            string query = @"
                        SELECT
                            UCM.DenominationID,
                            'Rs.' + CAST(CAST(D.Denomination AS DECIMAL(18,2)) AS VARCHAR(20)) + '/-' AS DisplayDenomination,
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

            return db.ExecuteQuery(query, parameters);
        }

        public bool SaveIndent(IndentMasterModel master, DataTable dtItems)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    //=========================
                    // Save Master
                    //=========================

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


                    //=========================
                    // Save Detail
                    //=========================

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
                                            new SqlParameter("@CategoryID", row["CategoryID"]),
                                            new SqlParameter("@DenominationID", row["DenominationID"]),
                                            new SqlParameter("@SheetQty", row["SheetQty"]),
                                            new SqlParameter("@PieceQty", row["PieceQty"]),
                                            new SqlParameter("@PiecesPerSheet", row["PiecesPerSheet"]),
                                            new SqlParameter("@TotalPieces", row["TotalPieces"])
                                        };

                        SqlCommand detailCmd = new SqlCommand(detailQuery, con, tran);

                        detailCmd.Parameters.AddRange(detailParameters);

                        detailCmd.ExecuteNonQuery();
                    }


                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    MessageBox.Show(ex.ToString());

                    return false;
                }
            }
        }

        public bool HasSupplyRecords(int indentID)
        {
            string query = @"SELECT COUNT(1) 
                    FROM SupplyMaster SD
                    INNER JOIN IndentMaster IM ON SD.IndentID = IM.IndentID
                    WHERE IM.IndentID = @IndentID";

            // DBConnection class ka object banayein
            DBConnection db = new DBConnection();

            // GetConnection() method se SqlConnection hasil karein
            using (SqlConnection conn = db.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IndentID", indentID);
                conn.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
        public bool UpdateIndent(IndentMasterModel master, DataTable dtItems)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    // =========================
                    // Update Master
                    // =========================

                    string masterQuery = @"
                UPDATE IndentMaster
                SET
                    IndentNo = @IndentNo,
                    IndentDate = @IndentDate,
                    OfficeID = @OfficeID,
                    Remarks = @Remarks
                WHERE IndentID = @IndentID";

                    SqlCommand masterCmd = new SqlCommand(
                        masterQuery, con, tran);

                    masterCmd.Parameters.AddWithValue(
                        "@IndentID", master.IndentID);

                    masterCmd.Parameters.AddWithValue(
                        "@IndentNo", master.IndentNo);

                    masterCmd.Parameters.AddWithValue(
                        "@IndentDate", master.IndentDate);

                    masterCmd.Parameters.AddWithValue(
                        "@OfficeID", master.OfficeID);

                    masterCmd.Parameters.AddWithValue(
                        "@Remarks",
                        string.IsNullOrWhiteSpace(master.Remarks)
                            ? (object)DBNull.Value
                            : master.Remarks);

                    masterCmd.ExecuteNonQuery();


                    // =========================
                    // Delete Old Details
                    // =========================

                    string deleteQuery = @"
                DELETE FROM IndentDetail
                WHERE IndentID = @IndentID";

                    SqlCommand deleteCmd = new SqlCommand(
                        deleteQuery, con, tran);

                    deleteCmd.Parameters.AddWithValue(
                        "@IndentID", master.IndentID);

                    deleteCmd.ExecuteNonQuery();


                    // =========================
                    // Insert Corrected Details
                    // =========================

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

                        SqlCommand detailCmd = new SqlCommand(
                            detailQuery, con, tran);

                        detailCmd.Parameters.AddWithValue(
                            "@IndentID", master.IndentID);

                        detailCmd.Parameters.AddWithValue(
                            "@CategoryID", row["CategoryID"]);

                        detailCmd.Parameters.AddWithValue(
                            "@DenominationID", row["DenominationID"]);

                        detailCmd.Parameters.AddWithValue(
                            "@SheetQty", row["SheetQty"]);

                        detailCmd.Parameters.AddWithValue(
                            "@PieceQty", row["PieceQty"]);

                        detailCmd.Parameters.AddWithValue(
                            "@PiecesPerSheet", row["PiecesPerSheet"]);

                        detailCmd.Parameters.AddWithValue(
                            "@TotalPieces", row["TotalPieces"]);

                        detailCmd.ExecuteNonQuery();
                    }


                    // =========================
                    // Commit
                    // =========================

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    MessageBox.Show(
                        ex.ToString(),
                        "Update Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }
        }
        public DataTable SearchIndent(
    int? zoneID,
    int? officeID,
    DateTime? fromDate,
    DateTime? toDate,
    string indentNo)
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

            return db.ExecuteQuery(query, parameters.ToArray());
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

            SqlParameter[] parameters =
            {
        new SqlParameter("@IndentID", indentID)
    };

            DataTable dt = db.ExecuteQuery(query, parameters);

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

            SqlParameter[] parameters =
            {
        new SqlParameter("@IndentID", indentID)
    };

            return db.ExecuteQuery(query, parameters);
        }

        public bool DeleteIndent(int indentID)
        {
            using (SqlConnection con = db.GetConnection())
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
                catch (Exception ex)
                {
                    tran.Rollback();

                    MessageBox.Show(ex.Message);

                    return false;
                }


            }
        }






    }
}