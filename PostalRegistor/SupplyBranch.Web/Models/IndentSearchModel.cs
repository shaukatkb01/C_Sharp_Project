using System;

namespace SupplyBranch.Models
{
    public class IndentSearchModel
    {
        public int ZoneID { get; set; }

        public int OfficeID { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string IndentNo { get; set; }
    }
}
