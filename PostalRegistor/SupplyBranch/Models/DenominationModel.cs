using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBranch.Models
{
    public class DenominationModel
    {
        public int DenominationID { get; set; }

        public decimal Denomination { get; set; }

        public int CategoryID { get; set; }
    }
}