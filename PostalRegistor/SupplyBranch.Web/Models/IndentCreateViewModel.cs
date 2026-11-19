using System;
using System.ComponentModel.DataAnnotations;

namespace SupplyBranch.Web.Models
{
    public class IndentCreateViewModel
    {
        [Required]
        public string IndentNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime IndentDate { get; set; }

        [Required]
        public int OfficeID { get; set; }

        public string Remarks { get; set; }

        // JSON array of items: { CategoryID, DenominationID, SheetQty, PieceQty, PiecesPerSheet, TotalPieces }
        public string ItemsJson { get; set; }
    }
}
