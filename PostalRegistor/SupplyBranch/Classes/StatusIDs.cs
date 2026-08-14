using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyBranch.Classes
{

    
        public static class StatusIDs
        {
        // Supply Status
        public const int SupplyDraft = 1;
        public const int SupplyApproved = 2;
        public const int SupplyDispatch = 3;
        public const int SupplyCancelled = 4;

        // Indent Status
        public const int IndentOpen = 5;
        public const int IndentPartial = 6;
        public const int IndentClosed = 7;
    }
    
    
}
