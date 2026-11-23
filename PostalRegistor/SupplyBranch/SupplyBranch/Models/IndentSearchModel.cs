using System;

namespace SupplyBranch.Models
{
    public class IndentSearchModel
    {
        public int IndentID { get; set; }

        public string IndentNo { get; set; }

        public DateTime IndentDate { get; set; }

        public string OfficeName { get; set; }

        public int TotalItems { get; set; }

        public string Remarks { get; set; }
    }
}