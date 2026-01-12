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
        // Humne isay 'static' banaya hai taake direct call ho sake
        public static void CalculateAndDisplayStock(int fileId,
            TextBox txtStamp, TextBox txtFdc, TextBox txtLeaf, TextBox txtFdcc, TextBox txtPost)
        {
            int sQty = 0, fdcQty = 0, leafQty = 0, fdccQty = 0, postQty = 0;

            using (SqlConnection con = new SqlConnection(Db.ConString))
            {
                string query = @"SELECT StampsQty, FDCQty, LeafletQty, FDCCQty, PostmarkQty, SupplyType 
                                 FROM PhilatelicSupply 
                                 WHERE FileNo = @fid AND SupplyType IN (1, 2, 3, 4)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@fid", fileId);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int type = Convert.ToInt32(reader["SupplyType"]);

                        // Values uthatay waqt NULL check
                        int curS = reader["StampsQty"] != DBNull.Value ? Convert.ToInt32(reader["StampsQty"]) : 0;
                        int curF = reader["FDCQty"] != DBNull.Value ? Convert.ToInt32(reader["FDCQty"]) : 0;
                        int curL = reader["LeafletQty"] != DBNull.Value ? Convert.ToInt32(reader["LeafletQty"]) : 0;
                        int curFC = reader["FDCCQty"] != DBNull.Value ? Convert.ToInt32(reader["FDCCQty"]) : 0;
                        int curP = reader["PostmarkQty"] != DBNull.Value ? Convert.ToInt32(reader["PostmarkQty"]) : 0;

                        // Logic: 1,3 is PLUS and 2,4 is MINUS
                        if (type == 1 || type == 3)
                        {
                            sQty += curS; fdcQty += curF; leafQty += curL; fdccQty += curFC; postQty += curP;
                        }
                        else
                        {
                            sQty -= curS; fdcQty -= curF; leafQty -= curL; fdccQty -= curFC; postQty -= curP;
                        }
                    }

                    // TextBoxes mein data dikhana
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
