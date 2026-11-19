using System;
using System.ComponentModel.DataAnnotations;

namespace SupplyBranch.Web.Models
{
    public class SupplyCreateViewModel
    {
        [Required]
        public int IndentID { get; set; }

        [Required]
        public int SupplyTypeID { get; set; }

        public string DispatchMode { get; set; }

        public string PackingType { get; set; }

        public int PackingQty { get; set; }

        public string Remarks { get; set; }

        [DataType(DataType.Date)]
        public DateTime SupplyDate { get; set; }

        // JSON array of items. Each item: { categoryID, denominationID, supplySheets, supplyPieces, totalSupplyQty, ledgerFolio, caseNoFrom, caseNoTo, caseCode }
        public string ItemsJson { get; set; }
    }
}
