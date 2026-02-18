using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndex
{
    public class Db
    {
        public static string ConString
        {
            get
            {
                // Debugging: Check karein ke system ko kitni strings nazar aa rahi hain
                int count = ConfigurationManager.ConnectionStrings.Count;

                var settings = ConfigurationManager.ConnectionStrings["DBCon"];

                if (settings == null)
                {
                    // Ye message aapko bataye ga ke asal mein masla kya hai
                    string errorMsg = $"XML nahi mil rahi! System ko total {count} strings mili hain. " +
                                      "Yaqeen karein ke App.config file 'Copy Always' par set hai.";

                    MessageBox.Show(errorMsg); // Error ki wajah jan'ne ke liye
                    return "";
                }

                return settings.ConnectionString;
            }
        }
    }
}
