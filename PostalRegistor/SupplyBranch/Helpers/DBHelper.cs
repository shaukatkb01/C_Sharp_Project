using System;
using System.Data;
using System.Data.SqlClient;

namespace SupplyBranch.Helpers
{
    internal class DBHelper
    {

        public SqlConnection GetConnection()
        {
            return db.GetConnection();
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteNonQuery();
                }
            }
        }


        public object ExecuteScalar(string query, SqlParameter[] parameters)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteScalar();
                }
            }
        }
       

        public int ExecuteNonQuery(string query)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);

                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection con = db.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }
        DBConnection db = new DBConnection();

        public DataTable GetDataTable(string query)
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;
            }
        }

        

        public object ExecuteScalar(string query)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);

                return cmd.ExecuteScalar();
            }
        }


    }


  
    }