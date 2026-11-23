using SupplyBranch.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using SupplyBranch.Helpers;

namespace SupplyBranch.DataAccess
{
    internal class OfficeDAL
    {
        DBHelper db = new DBHelper();

        public DataTable GetFinancialYear()
        {
            string query = @"
SELECT
    '0' AS FinancialYear,
    'All Financial Years' AS FinancialYearName

UNION

SELECT DISTINCT
    FinancialYear,
    FinancialYear AS FinancialYearName
FROM SupplyMaster

ORDER BY FinancialYear";

            return db.GetDataTable(query);
        }

        public DataTable GetAllOffices()
        {
            string query = @"
SELECT
    0 AS OfficeID,
    'All Offices' AS OfficeName

UNION ALL

SELECT
    OfficeID,
    OfficeName
FROM Office

ORDER BY OfficeID";

            return db.GetDataTable(query);
        }

        public DataTable GetOfficeList()
        {
            string query = @"

SELECT

OfficeID,
OfficeName

FROM Office

ORDER BY OfficeName";

            return db.ExecuteQuery(query, null);
        }
        public DataTable GetByZone(int zoneID)
        {
            string sql;

            SqlParameter[] parameters = null;

            if (zoneID == 0)
            {
                sql = @"SELECT
                    OfficeID,
                    OfficeName
                FROM Office
                ORDER BY OfficeName";
            }
            else
            {
                sql = @"SELECT
                    OfficeID,
                    OfficeName
                FROM Office
                WHERE ZoneID=@ZoneID
                ORDER BY OfficeName";

                parameters = new SqlParameter[]
                {
            new SqlParameter("@ZoneID", zoneID)
                };
            }

            return db.ExecuteQuery(sql, parameters);
        }

        public DataTable Search(string text)
        {
            string sql = @"SELECT
                        O.OfficeID,
                        Z.ZoneName,
                        O.OfficeName,
                        O.OfficeFileNo,
                        O.OfficeCode,
                        O.ZoneID
                   FROM Office O
                   INNER JOIN OfficeZone Z
                        ON O.ZoneID = Z.ZoneID
                   WHERE O.OfficeName LIKE @Search
                      OR O.OfficeFileNo LIKE @Search
                      OR O.OfficeCode LIKE @Search
                      OR Z.ZoneName LIKE @Search
                   ORDER BY Z.ZoneName,O.OfficeName";

            SqlParameter[] parameters =
            {
        new SqlParameter("@Search", "%" + text + "%")
    };

            return db.ExecuteQuery(sql, parameters);
        }

        public bool Delete(int officeID)
        {
            string sql = @"DELETE FROM Office
                   WHERE OfficeID=@OfficeID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@OfficeID", officeID)
    };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public DataTable GetAll()
        {
            string sql = @"SELECT
                        O.OfficeID,
                        Z.ZoneName,
                        O.OfficeName,
                        O.OfficeFileNo,
                        O.OfficeCode,
                        O.ZoneID
                   FROM Office O
                   INNER JOIN OfficeZone Z
                        ON O.ZoneID = Z.ZoneID
                   ORDER BY Z.ZoneName,O.OfficeName";

            return db.GetDataTable(sql);
        }

        public bool Update(Office office)
        {
            string sql = @"UPDATE Office
                   SET OfficeName=@OfficeName,
                       OfficeFileNo=@OfficeFileNo,
                       OfficeCode=@OfficeCode,
                       ZoneID=@ZoneID
                   WHERE OfficeID=@OfficeID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@OfficeName", office.OfficeName),
        new SqlParameter("@OfficeFileNo", office.OfficeFileNo),
        new SqlParameter("@OfficeCode", office.OfficeCode),
        new SqlParameter("@ZoneID", office.ZoneID),
        new SqlParameter("@OfficeID", office.OfficeID)
    };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }
        public bool IsFileNoExists(string officeFileNo, int officeID = 0)
        {
            string sql = @"SELECT COUNT(*)
                   FROM Office
                   WHERE OfficeFileNo = @OfficeFileNo
                     AND OfficeID <> @OfficeID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@OfficeFileNo", officeFileNo),
        new SqlParameter("@OfficeID", officeID)
    };

            int count = Convert.ToInt32(db.ExecuteScalar(sql, parameters));

            return count > 0;
        }

        public bool IsOfficeExists(string officeName, int zoneID, int officeID = 0)
        {
            string sql = @"SELECT COUNT(*)
                   FROM Office
                   WHERE OfficeName = @OfficeName
                     AND ZoneID = @ZoneID
                     AND OfficeID <> @OfficeID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@OfficeName", officeName),
        new SqlParameter("@ZoneID", zoneID),
        new SqlParameter("@OfficeID", officeID)
    };

            int count = Convert.ToInt32(db.ExecuteScalar(sql, parameters));

            return count > 0;
        }

        public bool Save(Office office)
        {
            string sql = @"INSERT INTO Office
                   (OfficeName, OfficeFileNo, OfficeCode, ZoneID)
                   VALUES
                   (@OfficeName, @OfficeFileNo, @OfficeCode, @ZoneID)";

            SqlParameter[] parameters =
            {
        new SqlParameter("@OfficeName", office.OfficeName),
        new SqlParameter("@OfficeFileNo", office.OfficeFileNo),
        new SqlParameter("@OfficeCode", office.OfficeCode),
        new SqlParameter("@ZoneID", office.ZoneID)
    };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

    }
}