using SupplyBranch.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using SupplyBranch.Helpers;
namespace SupplyBranch.DataAccess
{
    internal class OfficeZoneDAL
    {
        DBHelper db = new DBHelper();

        public DataTable GetZones()
        {
            string sql = @"SELECT ZoneID, ZoneName
                   FROM OfficeZone
                   ORDER BY ZoneName";

            return db.GetDataTable(sql);
        }
        public bool Delete(int zoneID)
        {
            string sql = @"DELETE FROM OfficeZone
                   WHERE ZoneID=@ZoneID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@ZoneID", zoneID)
    };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(OfficeZone zone)
        {
            string sql = @"UPDATE OfficeZone
                   SET ZoneName=@ZoneName
                   WHERE ZoneID=@ZoneID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@ZoneName", zone.ZoneName),
        new SqlParameter("@ZoneID", zone.ZoneID)
    };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool IsZoneExists(string zoneName, int zoneID = 0)
        {
            string sql = @"SELECT COUNT(*)
                   FROM OfficeZone
                   WHERE ZoneName = @ZoneName
                   AND ZoneID <> @ZoneID";

            SqlParameter[] parameters =
            {
        new SqlParameter("@ZoneName", zoneName),
        new SqlParameter("@ZoneID", zoneID)
    };

            int count = Convert.ToInt32(db.ExecuteScalar(sql, parameters));

            return count > 0;
        }

        public bool Save(OfficeZone zone)
        {
            string sql = @"INSERT INTO OfficeZone(ZoneName)
                           VALUES(@ZoneName)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ZoneName", zone.ZoneName)
            };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public DataTable GetAll()
        {
            string sql = "SELECT ZoneID, ZoneName FROM OfficeZone ORDER BY ZoneName";

            return db.GetDataTable(sql);
        }


    }
}