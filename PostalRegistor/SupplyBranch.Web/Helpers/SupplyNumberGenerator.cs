using SupplyBranch.DataAccess;
using SupplyBranch.Helpers;

namespace SupplyBranch.Helpers
{
    public class SupplyNumberGenerator
    {
        private readonly SupplyDAL _supplyDAL;

        public SupplyNumberGenerator(SupplyDAL supplyDAL)
        {
            _supplyDAL = supplyDAL;
        }

        public SupplyNumberInfo GenerateApprovedSupplyNumber(int indentID)
        {
            string financialYear = GetFinancialYear();

            var office = _supplyDAL.GetOfficeInfo(indentID);

            int lastGlobal = _supplyDAL.GetLastApprovedGlobalNumber(financialYear);

            int lastOffice = _supplyDAL.GetLastApprovedOfficeNumber(office.OfficeID, financialYear);

            int nextGlobal = lastGlobal + 1;
            int nextOffice = lastOffice + 1;

            SupplyNumberInfo info = new SupplyNumberInfo
            {
                GlobalSequence = nextGlobal,
                OfficeSequence = nextOffice,
                OfficeID = office.OfficeID,
                OfficeCode = office.OfficeCode,
                FinancialYear = financialYear,
                SupplyNo = $"P-{nextGlobal:0000}/{office.OfficeCode}-{nextOffice:0000}/{financialYear}"
            };

            return info;
        }

        public SupplyNumberInfo GenerateSupplyNumber(int indentID)
        {
            string financialYear = GetFinancialYear();

            var office = _supplyDAL.GetOfficeInfo(indentID);

            int lastGlobal = _supplyDAL.GetLastGlobalNumber(financialYear);

            int lastOffice = _supplyDAL.GetLastOfficeNumber(office.OfficeID, financialYear);

            int nextGlobal = lastGlobal + 1;
            int nextOffice = lastOffice + 1;

            SupplyNumberInfo info = new SupplyNumberInfo
            {
                GlobalSequence = nextGlobal,
                OfficeSequence = nextOffice,
                OfficeID = office.OfficeID,
                OfficeCode = office.OfficeCode,
                FinancialYear = financialYear,
                SupplyNo = $"P-{nextGlobal:0000}/{office.OfficeCode}-{nextOffice:0000}/{financialYear}"
            };

            return info;
        }

        public string GetFinancialYear()
        {
            DateTime today = DateTime.Today;

            int startYear;
            int endYear;

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
