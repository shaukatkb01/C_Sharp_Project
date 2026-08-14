using SupplyBranch.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using SupplyBranch.Helpers;
namespace SupplyBranch.DAL
{
    internal class StampCategoryDAL
    {
        DBHelper db = new DBHelper();

        public DataTable GetCategoryList()
        {
            string query = @"SELECT
                        CategoryID,
                        Name
                     FROM StampCategory
                     ORDER BY Name";

            return db.GetDataTable(query);
        }

        public DataTable Search(string keyword)
        {
            string query = @"SELECT
                        CategoryID,
                        Name,
                        Description
                     FROM StampCategory
                     WHERE Name LIKE @Keyword
                        OR Description LIKE @Keyword
                     ORDER BY Name";

            SqlParameter[] parameters =
            {
        new SqlParameter("@Keyword", "%" + keyword + "%")
    };

            return db.ExecuteQuery(query, parameters);
        }
        public bool Delete(int categoryID)
        {
            string query = @"DELETE FROM StampCategory
                     WHERE CategoryID=@CategoryID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryID", categoryID)
    };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Update(StampCategoryModel model)
        {
            string query = @"UPDATE StampCategory
                     SET Name=@Name,
                         Description=@Description
                     WHERE CategoryID=@CategoryID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@Name", model.Name),
        new SqlParameter("@Description", model.Description),
        new SqlParameter("@CategoryID", model.CategoryID)
    };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Insert(StampCategoryModel model)
        {
            string query = @"INSERT INTO StampCategory
                    (Name, Description)
                    VALUES
                    (@Name,@Description)";

            SqlParameter[] parameters =
            {
        new SqlParameter("@Name", model.Name),
        new SqlParameter("@Description", model.Description)
    };

            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable GetAll()
        {
            string query = @"SELECT
                                CategoryID,
                                Name,
                                Description
                             FROM StampCategory
                             ORDER BY Name";

            return db.GetDataTable(query);
        }

        public bool Exists(string categoryName, int categoryID = 0)
        {
            string query = @"SELECT COUNT(*)
                     FROM StampCategory
                     WHERE Name=@Name
                     AND CategoryID<>@CategoryID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@Name", categoryName),
        new SqlParameter("@CategoryID", categoryID)
    };

            int count = Convert.ToInt32(db.ExecuteScalar(query, parameters));

            return count > 0;
        }
    }
}