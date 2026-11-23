using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBranch.Models
{
    internal class Supply
    {
        public int SupplyID { get; set; }

        public string SupplyNo { get; set; }

        public string SupplyType { get; set; }

        public string FinancialYear { get; set; }

        public DateTime SupplyDate { get; set; }

        public int IndentID { get; set; }

        public int StatusID { get; set; }

        public string Remarks { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool NeedReprint { get; set; }

        public string DispatchMode { get; set; }

        public string PackingType { get; set; }

        public int PackingQty { get; set; }

        public string LedgerFolio { get; set; }
    }
}
