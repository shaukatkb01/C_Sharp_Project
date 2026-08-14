using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBranch
{
    internal class DBConnection
    {
        private string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=SupplyDB;
              Integrated Security=True;
              TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
