using SupplyBranch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupplyBranch.Helpers;

namespace SupplyBranch.DAL
{
    internal class DenominationDAL
    {
        DBHelper db = new DBHelper();

        public bool Delete(int denominationID)
        {
            string query = @"DELETE FROM Denomination
                     WHERE DenominationID = @DenominationID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@DenominationID", denominationID)
    };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Update(DenominationModel model)
        {
            string query = @"UPDATE Denomination
                     SET CategoryID = @CategoryID,
                         Denomination = @Denomination
                     WHERE DenominationID = @DenominationID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", model.CategoryID),
        new SqlParameter("@Denomination", model.Denomination),
        new SqlParameter("@DenominationID", model.DenominationID)
    };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable Search(string keyword)
        {
            string query = @"SELECT
                        d.DenominationID,
                        d.CategoryID,
                        c.Name AS Category,
                        d.Denomination
                     FROM Denomination d
                     INNER JOIN StampCategory c
                        ON d.CategoryID = c.CategoryID
                     WHERE c.Name LIKE @Keyword
                        OR CAST(d.Denomination AS NVARCHAR(50)) LIKE @Keyword
                     ORDER BY c.Name, d.Denomination";

            SqlParameter[] parameters =
            {
        new SqlParameter("@Keyword", "%" + keyword + "%")
    };

            return db.ExecuteQuery(query, parameters);
        }

        public DataTable GetAll()
        {
            string query = @"SELECT
                        d.DenominationID,
                        d.CategoryID,
                        c.Name AS Category,
                        d.Denomination
                     FROM Denomination d
                     INNER JOIN StampCategory c
                        ON d.CategoryID = c.CategoryID
                     ORDER BY c.Name, d.Denomination";

            return db.GetDataTable(query);
        }
       
        public bool Insert(DenominationModel model)
        {
            string query = @"INSERT INTO Denomination
                    (CategoryID, Denomination)
                     VALUES
                    (@CategoryID, @Denomination)";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", model.CategoryID),
        new SqlParameter("@Denomination", model.Denomination)
    };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

       
        

        public bool Exists(int categoryID, decimal denomination, int denominationID = 0)
        {
            string query = @"SELECT COUNT(*)
                     FROM Denomination
                     WHERE CategoryID = @CategoryID
                     AND Denomination = @Denomination
                     AND DenominationID <> @DenominationID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", categoryID),
        new SqlParameter("@Denomination", denomination),
        new SqlParameter("@DenominationID", denominationID)
    };

            int count = Convert.ToInt32(db.ExecuteScalar(query, parameters));

            return count > 0;
        }
    }
}
