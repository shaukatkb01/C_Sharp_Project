using System;

namespace SupplyBranch.Models
{
    public class IndentMasterModel
    {
        public int IndentID { get; set; }

        public string IndentNo { get; set; }

        public DateTime IndentDate { get; set; }

        public int OfficeID { get; set; }

        public string Remarks { get; set; }
    }
}