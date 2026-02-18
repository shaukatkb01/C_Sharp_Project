using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndex
{
    internal class StockManager
    {
        public static void CalculateAndDisplayStock(int fileId,
            TextBox txtStamp, TextBox txtFdc, TextBox txtLeaf, TextBox txtFdcc, TextBox txtPost)
        {
            int sQty = 0, fdcQty = 0, leafQty = 0, fdccQty = 0, postQty = 0;

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                // Query 1: PhilatelicSupply se data uthana
                string query = @"SELECT StampsQty, FDCQty, LeafletQty, FDCCQty, PostmarkQty, SupplyType 
                             FROM PhilatelicSupply 
                             WHERE FileNo = @fid AND SupplyType IN (1, 2, 3, 4)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@fid", fileId);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool hasData = false;
                        while (reader.Read())
                        {
                            hasData = true;
                            int type = Convert.ToInt32(reader["SupplyType"]);

                            // Short way to handle NULL and Convert
                            int curS = reader["StampsQty"] != DBNull.Value ? Convert.ToInt32(reader["StampsQty"]) : 0;
                            int curF = reader["FDCQty"] != DBNull.Value ? Convert.ToInt32(reader["FDCQty"]) : 0;
                            int curL = reader["LeafletQty"] != DBNull.Value ? Convert.ToInt32(reader["LeafletQty"]) : 0;
                            int curFC = reader["FDCCQty"] != DBNull.Value ? Convert.ToInt32(reader["FDCCQty"]) : 0;
                            int curP = reader["PostmarkQty"] != DBNull.Value ? Convert.ToInt32(reader["PostmarkQty"]) : 0;

                            if (type == 1 || type == 3) // Stock IN
                            {
                                sQty += curS; fdcQty += curF; leafQty += curL; fdccQty += curFC; postQty += curP;
                            }
                            else // Stock OUT (Type 2, 4)
                            {
                                sQty -= curS; fdcQty -= curF; leafQty -= curL; fdccQty -= curFC; postQty -= curP;
                            }
                        }

                        // Agar pehle table mein data NAHI mila, toh doosre table (StockPhilQuantity) ko check karein
                        if (!hasData)
                        {
                            if (reader != null) reader.Close();

                            // query2 mein SupplyType nikal diya kyunke wo table mein nahi hai
                            string query2 = @"SELECT StampFCQty, FDCQty, LeafletQty, PostmarkQty 
                      FROM StockPhilQuantity 
                      WHERE FileNo = @fid";

                            SqlCommand cmd2 = new SqlCommand(query2, con);
                            cmd2.Parameters.AddWithValue("@fid", fileId);

                            using (SqlDataReader reader2 = cmd2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    // Yahan hum 'type' check nahi karenge, direct PLUS karenge
                                    int curS = reader2["StampFCQty"] != DBNull.Value ? Convert.ToInt32(reader2["StampFCQty"]) : 0;
                                    int curF = reader2["FDCQty"] != DBNull.Value ? Convert.ToInt32(reader2["FDCQty"]) : 0;
                                    int curL = reader2["LeafletQty"] != DBNull.Value ? Convert.ToInt32(reader2["LeafletQty"]) : 0;
                                    int curP = reader2["PostmarkQty"] != DBNull.Value ? Convert.ToInt32(reader2["PostmarkQty"]) : 0;

                                    // Direct PLUS kyunke supplytype nahi hai (Opening Balance treat karenge)
                                    sQty += curS;
                                    fdcQty += curF;
                                    leafQty += curL;
                                    postQty += curP;
                                }
                            }
                        }
                    }

                    // Final Display
                    txtStamp.Text = sQty.ToString();
                    txtFdc.Text = fdcQty.ToString();
                    txtLeaf.Text = leafQty.ToString();
                    txtFdcc.Text = fdccQty.ToString();
                    txtPost.Text = postQty.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Stock Error: " + ex.Message);
                }
            }
        }
    }
}
