using SupplyBranch.Helpers;
using SupplyBranch.Models;
using SupplyBranch.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace SupplyBranch.DataAccess
{
    public class ReportDAL
    {
        private DBHelper db = new DBHelper();


        public DataTable GetCategories()
        {
            string query = @"
        SELECT
            0 AS CategoryID,
            'All Categories' AS Name,
            0 AS SortOrder

        UNION ALL

        SELECT
            CategoryID,
            Name,
            1 AS SortOrder
        FROM StampCategory

        ORDER BY SortOrder, Name";

            return db.GetDataTable(query);
        }

        //        public DataTable GetSupplyRegister(ReportFilter filter)
        //        {
        //            StringBuilder query = new StringBuilder();

        //            query.Append(@"
        //SELECT *
        //FROM vw_ReportSupply
        //WHERE 1=1");

        //            List<SqlParameter> parameters = new List<SqlParameter>();

        //            //--------------------------------------------------
        //            // Indent
        //            //--------------------------------------------------

        //            if (filter.IndentID.HasValue)
        //            {
        //                query.Append(" AND IndentID=@IndentID");

        //                parameters.Add(
        //                    new SqlParameter("@IndentID", filter.IndentID.Value));
        //            }

        //            //--------------------------------------------------
        //            // Supply
        //            //--------------------------------------------------

        //            if (filter.SupplyID.HasValue)
        //            {
        //                query.Append(" AND SupplyID=@SupplyID");

        //                parameters.Add(
        //                    new SqlParameter("@SupplyID", filter.SupplyID.Value));
        //            }

        //            //--------------------------------------------------
        //            // Office
        //            //--------------------------------------------------

        //            if (filter.OfficeID.HasValue)
        //            {
        //                query.Append(" AND OfficeID=@OfficeID");

        //                parameters.Add(
        //                    new SqlParameter("@OfficeID", filter.OfficeID.Value));
        //            }

        //            //--------------------------------------------------
        //            // Category
        //            //--------------------------------------------------

        //            if (filter.CategoryID.HasValue)
        //            {
        //                query.Append(" AND CategoryID=@CategoryID");

        //                parameters.Add(
        //                    new SqlParameter("@CategoryID", filter.CategoryID.Value));
        //            }

        //            //--------------------------------------------------
        //            // Supply Type
        //            //--------------------------------------------------

        //            if (filter.SupplyType.HasValue)
        //            {
        //                query.Append(" AND SupplyType=@SupplyType");

        //                parameters.Add(
        //                    new SqlParameter("@SupplyType", filter.SupplyType.Value));
        //            }

        //            //--------------------------------------------------
        //            // Status
        //            //--------------------------------------------------
        //            //============================
        //            // Supply Register
        //            //============================

        //            if (filter.SupplyRegisterOnly)
        //            {
        //                query.Append(" AND StatusID IN (2,3)");
        //            }


        //            else if (filter.StatusID.HasValue)
        //            {
        //                query.Append(" AND StatusID=@StatusID");

        //                parameters.Add(
        //                    new SqlParameter("@StatusID", filter.StatusID.Value));
        //            }

        //            //--------------------------------------------------
        //            // Financial Year
        //            //--------------------------------------------------

        //            if (!string.IsNullOrWhiteSpace(filter.FinancialYear))
        //            {
        //                query.Append(" AND FinancialYear=@FinancialYear");

        //                parameters.Add(
        //                    new SqlParameter("@FinancialYear", filter.FinancialYear));
        //            }

        //            //--------------------------------------------------
        //            // Date From
        //            //--------------------------------------------------

        //            if (filter.FromDate.HasValue)
        //            {
        //                query.Append(" AND SupplyDate>=@FromDate");

        //                parameters.Add(
        //                    new SqlParameter("@FromDate", filter.FromDate.Value.Date));
        //            }

        //            //--------------------------------------------------
        //            // Date To
        //            //--------------------------------------------------

        //            if (filter.ToDate.HasValue)
        //            {
        //                query.Append(" AND SupplyDate<=@ToDate");

        //                parameters.Add(
        //                    new SqlParameter("@ToDate", filter.ToDate.Value.Date));
        //            }

        //            //--------------------------------------------------
        //            // Sorting
        //            //--------------------------------------------------

        //            query.Append(@"
        //ORDER BY
        //    SupplyDate,
        //    SupplyNo,
        //    CategoryID,
        //    Denomination");

        //            return db.ExecuteQuery(
        //                query.ToString(),
        //                parameters.ToArray());
        //        }


        public DataTable GetSupplyRegister(ReportFilter filter)
        {
            StringBuilder query = new StringBuilder();

            //--------------------------------------------------
            // Select View
            //--------------------------------------------------

            if (filter.CurrentBalanceOnly)
            {
                query.Append(@"
SELECT *
FROM vw_IndentCurrentBalance
WHERE 1=1");
            }
            else
            {
                query.Append(@"
SELECT *
FROM vw_ReportSupply
WHERE 1=1");
            }

            List<SqlParameter> parameters = new List<SqlParameter>();

            //--------------------------------------------------
            // Indent
            //--------------------------------------------------

            if (filter.IndentID.HasValue)
            {
                query.Append(" AND IndentID=@IndentID");

                parameters.Add(
                    new SqlParameter(
                        "@IndentID",
                        filter.IndentID.Value));
            }

            //--------------------------------------------------
            // Office
            //--------------------------------------------------

            if (filter.OfficeID.HasValue)
            {
                query.Append(" AND OfficeID=@OfficeID");

                parameters.Add(
                    new SqlParameter(
                        "@OfficeID",
                        filter.OfficeID.Value));
            }

            //--------------------------------------------------
            // Category
            //--------------------------------------------------

            if (filter.CategoryID.HasValue)
            {
                query.Append(" AND CategoryID=@CategoryID");

                parameters.Add(
                    new SqlParameter(
                        "@CategoryID",
                        filter.CategoryID.Value));
            }

            //--------------------------------------------------
            // CURRENT BALANCE REPORT
            //--------------------------------------------------

            if (filter.CurrentBalanceOnly)
            {
                // Current Balance report mein sirf
                // indent-level filtering use hogi.

                if (filter.FromDate.HasValue)
                {
                    query.Append(" AND IndentDate>=@FromDate");

                    parameters.Add(
                        new SqlParameter(
                            "@FromDate",
                            filter.FromDate.Value.Date));
                }

                if (filter.ToDate.HasValue)
                {
                    // Date ka complete din include karne ke liye
                    DateTime nextDay = filter.ToDate.Value.Date.AddDays(1);

                    query.Append(" AND IndentDate<@ToDate");

                    parameters.Add(
                        new SqlParameter(
                            "@ToDate",
                            nextDay));
                }

                query.Append(@"
ORDER BY
    OfficeName,
    IndentDate,
    IndentNo,
    CategoryID,
    Denomination");

                return db.ExecuteQuery(
                    query.ToString(),
                    parameters.ToArray());
            }

            //--------------------------------------------------
            // NORMAL SUPPLY REGISTER
            //--------------------------------------------------

            //--------------------------------------------------
            // Supply
            //--------------------------------------------------

            if (filter.SupplyID.HasValue)
            {
                query.Append(" AND SupplyID=@SupplyID");

                parameters.Add(
                    new SqlParameter(
                        "@SupplyID",
                        filter.SupplyID.Value));
            }

            //--------------------------------------------------
            // Supply Type
            //--------------------------------------------------

            if (filter.SupplyType.HasValue)
            {
                query.Append(" AND SupplyType=@SupplyType");

                parameters.Add(
                    new SqlParameter(
                        "@SupplyType",
                        filter.SupplyType.Value));
            }

            //--------------------------------------------------
            // Status
            //--------------------------------------------------

            if (filter.SupplyRegisterOnly)
            {
                query.Append(" AND StatusID IN (2,3)");
            }
            else if (filter.StatusID.HasValue)
            {
                query.Append(" AND StatusID=@StatusID");

                parameters.Add(
                    new SqlParameter(
                        "@StatusID",
                        filter.StatusID.Value));
            }

            //--------------------------------------------------
            // Financial Year
            //--------------------------------------------------

            if (!string.IsNullOrWhiteSpace(filter.FinancialYear))
            {
                query.Append(" AND FinancialYear=@FinancialYear");

                parameters.Add(
                    new SqlParameter(
                        "@FinancialYear",
                        filter.FinancialYear));
            }

            //--------------------------------------------------
            // Supply Date From
            //--------------------------------------------------

            if (filter.FromDate.HasValue)
            {
                query.Append(" AND SupplyDate>=@FromDate");

                parameters.Add(
                    new SqlParameter(
                        "@FromDate",
                        filter.FromDate.Value.Date));
            }

            //--------------------------------------------------
            // Supply Date To
            //--------------------------------------------------

            if (filter.ToDate.HasValue)
            {
                DateTime nextDay = filter.ToDate.Value.Date.AddDays(1);

                query.Append(" AND SupplyDate<@ToDate");

                parameters.Add(
                    new SqlParameter(
                        "@ToDate",
                        nextDay));
            }

            //--------------------------------------------------
            // Sorting
            //--------------------------------------------------

            query.Append(@"
ORDER BY
    SupplyDate,
    SupplyNo,
    CategoryID,
    Denomination");

            return db.ExecuteQuery(
                query.ToString(),
                parameters.ToArray());
        }

        public DataTable GetIndentRegister(ReportFilter filter)
        {
            StringBuilder query = new StringBuilder();

            query.Append(@"
SELECT *
FROM vw_ReportIndent
WHERE 1=1");

            List<SqlParameter> parameters = new List<SqlParameter>();

            //--------------------------------------------------
            // Indent
            //--------------------------------------------------

            if (filter.IndentID.HasValue)
            {
                query.Append(" AND IndentID=@IndentID");

                parameters.Add(
                    new SqlParameter("@IndentID", filter.IndentID.Value));
            }

            //--------------------------------------------------
            // Office
            //--------------------------------------------------

            if (filter.OfficeID.HasValue)
            {
                query.Append(" AND OfficeID=@OfficeID");

                parameters.Add(
                    new SqlParameter("@OfficeID", filter.OfficeID.Value));
            }

            //--------------------------------------------------
            // Category
            //--------------------------------------------------

            if (filter.CategoryID.HasValue)
            {
                query.Append(" AND CategoryID=@CategoryID");

                parameters.Add(
                    new SqlParameter("@CategoryID", filter.CategoryID.Value));
            }

            //--------------------------------------------------
            // Status
            //--------------------------------------------------

            if (filter.StatusID.HasValue)
            {
                query.Append(" AND StatusID=@StatusID");

                parameters.Add(
                    new SqlParameter("@StatusID", filter.StatusID.Value));
            }

            //--------------------------------------------------
            // Financial Year
            //--------------------------------------------------

            if (!string.IsNullOrWhiteSpace(filter.FinancialYear))
            {
                query.Append(" AND FinancialYear=@FinancialYear");

                parameters.Add(
                    new SqlParameter("@FinancialYear", filter.FinancialYear));
            }

            //--------------------------------------------------
            // Date From
            //--------------------------------------------------

            if (filter.FromDate.HasValue)
            {
                query.Append(" AND IndentDate>=@FromDate");

                parameters.Add(
                    new SqlParameter("@FromDate", filter.FromDate.Value.Date));
            }

            //--------------------------------------------------
            // Date To
            //--------------------------------------------------

            if (filter.ToDate.HasValue)
            {
                query.Append(" AND IndentDate<=@ToDate");

                parameters.Add(
                    new SqlParameter("@ToDate", filter.ToDate.Value.Date));
            }

            //--------------------------------------------------
            // Sorting
            //--------------------------------------------------

            query.Append(@"
ORDER BY
    IndentDate,
    IndentNo,
    CategoryID,
    Denomination");

            return db.ExecuteQuery(
                query.ToString(),
                parameters.ToArray());
        }

        // 2. GetOfficeList Method
        public DataTable GetOfficeList()
        {
            // 1. SQL Query (OfficeTable ki jagah apne database ke table ka asli naam likhein)
            string query = "SELECT OfficeID, OfficeName FROM Office";

            // 2. DBHelper ke zariye query chala kar DataTable hasil karein
            DataTable dt = db.GetDataTable(query);

            // 3. Result return kar dein
            return dt;
        }
    }
}