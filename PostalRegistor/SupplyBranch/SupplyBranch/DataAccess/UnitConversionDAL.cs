using SupplyBranch.Helpers;
using SupplyBranch.Models;
using System;
using System.Data;
using System.Data.SqlClient;

////
//////
///



namespace SupplyBranch.DAL
{
    public class UnitConversionDAL
    {
        DBHelper db = new DBHelper();

        public DataTable GetDenominationsForEdit(int categoryID, int denominationID)
        {
            string query = @"
        SELECT
            D.DenominationID,
            'Rs.' +
            CAST(
                CAST(D.Denomination AS DECIMAL(18,2))
                AS VARCHAR(20)
            ) + '/-' AS Denomination
        FROM Denomination D
        WHERE D.CategoryID = @CategoryID
          AND
          (
              D.DenominationID = @DenominationID
              OR NOT EXISTS
              (
                  SELECT 1
                  FROM UnitConversionMaster U
                  WHERE U.CategoryID = D.CategoryID
                    AND U.DenominationID = D.DenominationID
              )
          )
        ORDER BY D.Denomination";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@DenominationID", denominationID)
    };

            return db.ExecuteQuery(query, parameters);
        }
        public DataRow GetConversion(int denominationID)
        {
            string query = @"
                SELECT
                    PacketsPerBox,
                    SheetsPerPacket,
                    PiecesPerSheet
                FROM UnitConversionMaster
                WHERE DenominationID = @DenominationID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@DenominationID", denominationID)
            };

            DataTable dt = db.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return dt.Rows[0];

            return null;
        }

        public DataTable GetCategories()
        {
            string query = @"
                SELECT
                    CategoryID,
                    Name
                FROM StampCategory
                ORDER BY Name";

            return db.GetDataTable(query);
        }

        public DataTable GetDenominations(int categoryID)
        {
            string query = @"
                SELECT
                    D.DenominationID,
                    'Rs. ' + FORMAT(D.Denomination, '0.##') + '/-' AS Denomination
                FROM Denomination D
                WHERE D.CategoryID = @CategoryID
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM UnitConversionMaster U
                      WHERE U.CategoryID = D.CategoryID
                        AND U.DenominationID = D.DenominationID
                  )
                ORDER BY D.Denomination";

            SqlParameter[] parameters =
            {
                new SqlParameter("@CategoryID", categoryID)
            };

            return db.ExecuteQuery(query, parameters);
        }

        public bool Save(UnitConversionModel model)
        {
            string query = @"
                INSERT INTO UnitConversionMaster
                (
                    CategoryID,
                    DenominationID,
                    PacketsPerBox,
                    SheetsPerPacket,
                    PiecesPerSheet,
                    Remarks
                )
                VALUES
                (
                    @CategoryID,
                    @DenominationID,
                    @PacketsPerBox,
                    @SheetsPerPacket,
                    @PiecesPerSheet,
                    @Remarks
                )";

            SqlParameter[] parameters =
            {
                new SqlParameter("@CategoryID", model.CategoryID),

                new SqlParameter("@DenominationID", model.DenominationID),

                new SqlParameter("@PacketsPerBox", model.PacketsPerBox),

                new SqlParameter("@SheetsPerPacket", model.SheetsPerPacket),

                new SqlParameter("@PiecesPerSheet", model.PiecesPerSheet),

                new SqlParameter(
                    "@Remarks",
                    string.IsNullOrWhiteSpace(model.Remarks)
                        ? (object)DBNull.Value
                        : model.Remarks)
            };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable GetAll()
        {
            string query = @"
                SELECT
                    U.ConversionID,
                    U.CategoryID,
                    C.Name AS Category,
                    U.DenominationID,
                    'Rs. ' + FORMAT(D.Denomination, '0.##') + '/-' AS Denomination,
                    U.PacketsPerBox,
                    U.SheetsPerPacket,
                    U.PiecesPerSheet,
                    U.Remarks
                FROM UnitConversionMaster U
                INNER JOIN StampCategory C
                    ON U.CategoryID = C.CategoryID
                INNER JOIN Denomination D
                    ON U.DenominationID = D.DenominationID
                ORDER BY
                    C.Name,
                    D.Denomination";

            return db.GetDataTable(query);
        }

        public DataRow GetByID(int conversionID)
        {
            string query = @"
                SELECT *
                FROM UnitConversionMaster
                WHERE ConversionID = @ConversionID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ConversionID", conversionID)
            };

            DataTable dt = db.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return dt.Rows[0];

            return null;
        }

        public bool Update(UnitConversionModel model)
        {
            string query = @"
                UPDATE UnitConversionMaster
                SET
                    CategoryID = @CategoryID,
                    DenominationID = @DenominationID,
                    PacketsPerBox = @PacketsPerBox,
                    SheetsPerPacket = @SheetsPerPacket,
                    PiecesPerSheet = @PiecesPerSheet,
                    Remarks = @Remarks
                WHERE ConversionID = @ConversionID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ConversionID", model.ConversionID),

                new SqlParameter("@CategoryID", model.CategoryID),

                new SqlParameter("@DenominationID", model.DenominationID),

                new SqlParameter("@PacketsPerBox", model.PacketsPerBox),

                new SqlParameter("@SheetsPerPacket", model.SheetsPerPacket),

                new SqlParameter("@PiecesPerSheet", model.PiecesPerSheet),

                new SqlParameter(
                    "@Remarks",
                    string.IsNullOrWhiteSpace(model.Remarks)
                        ? (object)DBNull.Value
                        : model.Remarks)
            };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool IsDuplicate(
            int conversionID,
            int categoryID,
            int denominationID)
        {
            string query = @"
                SELECT COUNT(*)
                FROM UnitConversionMaster
                WHERE CategoryID = @CategoryID
                  AND DenominationID = @DenominationID
                  AND ConversionID <> @ConversionID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ConversionID", conversionID),
                new SqlParameter("@CategoryID", categoryID),
                new SqlParameter("@DenominationID", denominationID)
            };

            return Convert.ToInt32(
                db.ExecuteScalar(query, parameters)) > 0;
        }

        public bool Delete(int conversionID)
        {
            string query = @"
                DELETE FROM UnitConversionMaster
                WHERE ConversionID = @ConversionID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ConversionID", conversionID)
            };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable GetStockDenominations(int categoryID)
        {
            string query = @"
        SELECT
            U.DenominationID,
           'Rs. ' + FORMAT(D.Denomination, '0.##') + '/-' AS Denomination
        FROM UnitConversionMaster U
        INNER JOIN Denomination D
            ON U.DenominationID = D.DenominationID
        WHERE U.CategoryID = @CategoryID
        ORDER BY D.Denomination";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", categoryID)
    };

            return db.ExecuteQuery(query, parameters);
        }
    }
}






///
////




    //    public DataRow GetConversion(int denominationID)
    //    {
    //        string query = @"
    //    SELECT PiecesPerSheet
    //           /*SheetsPerSet*/
    //    FROM UnitConversionMaster
    //    WHERE DenominationID=@DenominationID";

    //        SqlParameter[] parameters =
    //        {
    //    new SqlParameter("@DenominationID", denominationID)
    //};

    //        DataTable dt = db.ExecuteQuery(query, parameters);

    //        if (dt.Rows.Count > 0)
    //            return dt.Rows[0];

    //        return null;
    //    }

        //public DataTable GetCategories()
        //{
        //    string query = @"SELECT CategoryID,
        //                    Name
        //             FROM StampCategory
        //             ORDER BY Name";

        //    return db.GetDataTable(query);
        //}
    //    public DataTable GetDenominations(int categoryID)
    //    {
    //        string query = @"
    //                    SELECT
    //                        D.DenominationID,
    //                        'Rs.' + CAST(CAST(D.Denomination AS DECIMAL(18,2)) AS VARCHAR(20)) + '/-' AS Denomination
    //                    FROM Denomination D
    //                    WHERE D.CategoryID = @CategoryID
    //                      AND NOT EXISTS
    //                      (
    //                          SELECT 1
    //                          FROM UnitConversionMaster U
    //                          WHERE U.CategoryID = D.CategoryID
    //                            AND U.DenominationID = D.DenominationID
    //                      )
    //                    ORDER BY D.Denomination";

    //        SqlParameter[] parameters =
    //        {
    //    new SqlParameter("@CategoryID", categoryID)
    //};

    //        return db.ExecuteQuery(query, parameters);
    //    }

    //    public bool Save(UnitConversionModel model)
    //    {
    //        string query = @"
    //INSERT INTO UnitConversionMaster
    //(
    //    CategoryID,
    //    DenominationID,
    //    PiecesPerSheet,
    //    Remarks
    //)
    //VALUES
    //(
    //    @CategoryID,
    //    @DenominationID,
    //    @PiecesPerSheet,
    //    @Remarks
    //)";

    //        SqlParameter[] parameters =
    //        {
    //    new SqlParameter("@CategoryID", model.CategoryID),
    //    new SqlParameter("@DenominationID", model.DenominationID),
    //    new SqlParameter("@PiecesPerSheet", model.PiecesPerSheet),
    //    new SqlParameter("@Remarks",
    //        string.IsNullOrWhiteSpace(model.Remarks)
    //        ? (object)DBNull.Value
    //        : model.Remarks)
    //};

    //        return db.ExecuteNonQuery(query, parameters) > 0;
    //    }

    //    public DataTable GetAll()
    //    {
    //        string query = @"
    //SELECT
    //    U.ConversionID,
    //    U.CategoryID,
    //    C.Name AS Category,
    //    U.DenominationID,
    //    D.Denomination,
    //    U.PiecesPerSheet,
    //    U.Remarks
    //FROM UnitConversionMaster U
    //INNER JOIN StampCategory C
    //    ON U.CategoryID = C.CategoryID
    //INNER JOIN Denomination D
    //    ON U.DenominationID = D.DenominationID
    //ORDER BY
    //    C.Name,
    //    D.Denomination";

    //        return db.GetDataTable(query);
    //    }

    //    public DataRow GetByID(int conversionID)
    //    {
    //        string query = @"
    //SELECT *
    //FROM UnitConversionMaster
    //WHERE ConversionID=@ConversionID";

    //        SqlParameter[] parameters =
    //        {
    //    new SqlParameter("@ConversionID", conversionID)
    //};

    //        DataTable dt = db.ExecuteQuery(query, parameters);

    //        if (dt.Rows.Count > 0)
    //            return dt.Rows[0];

    //        return null;
    //    }

    //    public bool Update(UnitConversionModel model)
    //    {
    //        string query = @"
    //UPDATE UnitConversionMaster
    //SET
    //    CategoryID = @CategoryID,
    //    DenominationID = @DenominationID,
    //    PiecesPerSheet = @PiecesPerSheet,
    //    Remarks = @Remarks
    //WHERE ConversionID = @ConversionID";

    //        SqlParameter[] parameters =
    //        {
    //    new SqlParameter("@ConversionID", model.ConversionID),
    //    new SqlParameter("@CategoryID", model.CategoryID),
    //    new SqlParameter("@DenominationID", model.DenominationID),
    //    new SqlParameter("@PiecesPerSheet", model.PiecesPerSheet),
    //    new SqlParameter("@Remarks",
    //        string.IsNullOrWhiteSpace(model.Remarks)
    //        ? (object)DBNull.Value
    //        : model.Remarks)
    //};

    //        return db.ExecuteNonQuery(query, parameters) > 0;
    //    }

    //    public bool IsDuplicate(int conversionID, int categoryID, int denominationID)
    //    {
    //        string query = @"
    //SELECT COUNT(*)
    //FROM UnitConversionMaster
    //WHERE CategoryID=@CategoryID
    //  AND DenominationID=@DenominationID
    //  AND ConversionID<>@ConversionID";

    //        SqlParameter[] parameters =
    //        {
    //    new SqlParameter("@ConversionID", conversionID),
    //    new SqlParameter("@CategoryID", categoryID),
    //    new SqlParameter("@DenominationID", denominationID)
    //};

    //        return Convert.ToInt32(db.ExecuteScalar(query, parameters)) > 0;
    //    }

    //    public bool Delete(int conversionID)
    //    {
    //        string query = @"
    //DELETE FROM UnitConversionMaster
    //WHERE ConversionID=@ConversionID";

    //        SqlParameter[] parameters =
    //        {
    //    new SqlParameter("@ConversionID", conversionID)
    //};

    //        return db.ExecuteNonQuery(query, parameters) > 0;
    //    }




   