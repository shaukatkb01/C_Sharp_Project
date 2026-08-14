using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupplyBranch.DataAccess;
namespace SupplyBranch.Helpers
{
    public class SupplyNumberGenerator
    {
        private readonly SupplyDAL supplyDAL = new SupplyDAL();

        public SupplyNumberInfo GenerateApprovedSupplyNumber(int indentID)
        {
            string financialYear = GetFinancialYear();

            var office = supplyDAL.GetOfficeInfo(indentID);

            // صرف Approved + Issued Count ہوں گے
            int lastGlobal =
                supplyDAL.GetLastApprovedGlobalNumber(financialYear);

            int lastOffice =
                supplyDAL.GetLastApprovedOfficeNumber(
                    office.OfficeID,
                    financialYear);

            int nextGlobal = lastGlobal + 1;
            int nextOffice = lastOffice + 1;

            SupplyNumberInfo info = new SupplyNumberInfo();

            info.GlobalSequence = nextGlobal;
            info.OfficeSequence = nextOffice;
            info.OfficeID = office.OfficeID;
            info.OfficeCode = office.OfficeCode;
            info.FinancialYear = financialYear;

            info.SupplyNo =
                $"P-{nextGlobal:0000}/{office.OfficeCode}-{nextOffice:0000}/{financialYear}";

            return info;
        }


        public SupplyNumberInfo GenerateSupplyNumber(int indentID)
        {
            string financialYear = GetFinancialYear();

            var office = supplyDAL.GetOfficeInfo(indentID);

            int lastGlobal =
                supplyDAL.GetLastGlobalNumber(financialYear);

            int lastOffice =
                supplyDAL.GetLastOfficeNumber(
                    office.OfficeID,
                    financialYear);

            int nextGlobal = lastGlobal + 1;

            int nextOffice = lastOffice + 1;

            SupplyNumberInfo info = new SupplyNumberInfo();

            info.GlobalSequence = nextGlobal;

            info.OfficeSequence = nextOffice;

            info.OfficeID = office.OfficeID;

            info.OfficeCode = office.OfficeCode;

            info.FinancialYear = financialYear;

            info.SupplyNo =
                $"P-{nextGlobal:0000}/{office.OfficeCode}-{nextOffice:0000}/{financialYear}";

            return info;
        }
        public string GetFinancialYear()
        {
            DateTime today = DateTime.Today;

            int startYear;
            int endYear;

            // پاکستان پوسٹ کا Financial Year
            // 1 جولائی سے شروع ہوگا

            if (today.Month >= 7)
            {
                startYear = today.Year % 100;
                endYear = (today.Year + 1) % 100;
            }
            else
            {
                startYear = (today.Year - 1) % 100;
                endYear = today.Year % 100;
            }

            return $"{startYear:00}-{endYear:00}";
        }

    }
}
