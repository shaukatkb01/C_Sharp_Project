using System;

namespace SupplyBranch.Models
{
    public class UnitConversionModel
    {
        public int ConversionID { get; set; }

        public int CategoryID { get; set; }

        public int DenominationID { get; set; }

        public int PacketsPerBox { get; set; }

        public int SheetsPerPacket { get; set; }

        public int PiecesPerSheet { get; set; }

        public string Remarks { get; set; }
    }
}