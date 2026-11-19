using System;

namespace SupplyBranch.Helpers
{
    public class SupplyNumberInfo
    {
        public string SupplyNo { get; set; } = string.Empty;

        public int GlobalSequence { get; set; }

        public int OfficeSequence { get; set; }

        public string FinancialYear { get; set; } = string.Empty;

        public int OfficeID { get; set; }

        public string OfficeCode { get; set; } = string.Empty;
    }
}
