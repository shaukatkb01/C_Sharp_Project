using System;

namespace SupplyBranch.Models
{
    public class ReportFilter
    {
        public int? IndentID { get; set; }

        public int? SupplyID { get; set; }

        public int? OfficeID { get; set; }

        public int? CategoryID { get; set; }

        public int? SupplyType { get; set; }

        public int? StatusID { get; set; }

        public string FinancialYear { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool SupplyRegisterOnly { get; set; } = false;

        public bool CurrentBalanceOnly { get; set; }
    }
}