using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplyBranch.DataAccess;
using System.Data;
using SupplyBranch.Models;
using SupplyBranch.Helpers;

namespace SupplyBranch.Web.Controllers
{
    [Authorize]
    public class SupplyController : Controller
    {
        private readonly DBHelper _db;
        private readonly SupplyDAL _supplyDal;
        private readonly StockDAL _stockDal;
        private readonly OfficeDAL _officeDal;
        private readonly Microsoft.Extensions.Logging.ILogger<SupplyController> _logger;

        public SupplyController(DBHelper db, SupplyDAL supplyDal, StockDAL stockDal, OfficeDAL officeDal, Microsoft.Extensions.Logging.ILogger<SupplyController> logger)
        {
            _db = db;
            _supplyDal = supplyDal;
            _stockDal = stockDal;
            _officeDal = officeDal;
            _logger = logger;
        }

        public IActionResult Index()
        {
            string sql = @"SELECT SM.SupplyID, SM.SupplyNo, SM.SupplyDate, IM.IndentNo, O.OfficeName, SS.StatusName
FROM SupplyMaster SM
LEFT JOIN IndentMaster IM ON SM.IndentID = IM.IndentID
LEFT JOIN Office O ON IM.OfficeID = O.OfficeID
LEFT JOIN Status SS ON SM.StatusID = SS.StatusID
ORDER BY SM.SupplyDate DESC";

            DataTable dt = _db.ExecuteQuery(sql);

            return View(dt);
        }

        public IActionResult Details(int id)
        {
            DataTable header = _supplyDal.GetSupplyHeader(id);
            DataTable details = _supplyDal.GetSupplyDetails(id);

            var model = new SupplyDetailsViewModel
            {
                Header = header,
                Details = details
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Load dropdowns
            ViewBag.SupplyTypes = _supplyDal.GetSupplyTypes();
            ViewBag.Offices = _officeDal.GetOfficeList();
            ViewBag.Categories = _stockDal.GetCategories();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SupplyBranch.Web.Models.SupplyCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // repopulate dropdowns
                    ViewBag.SupplyTypes = _supplyDal.GetSupplyTypes();
                    ViewBag.Offices = _officeDal.GetOfficeList();
                    ViewBag.Categories = _stockDal.GetCategories();

                    return View(model);
                }

                var gen = HttpContext.RequestServices.GetService(typeof(SupplyBranch.Helpers.SupplyNumberGenerator)) as SupplyBranch.Helpers.SupplyNumberGenerator;

                SupplyNumberInfo supplyInfo;

                if (gen != null)
                {
                    supplyInfo = gen.GenerateSupplyNumber(model.IndentID);
                }
                else
                {
                    supplyInfo = new SupplyNumberInfo { SupplyNo = "", FinancialYear = DateTime.Today.Year.ToString(), GlobalSequence = 0, OfficeSequence = 0 };
                }

                int indentStatusID = 5; // Default Open

                int supplyID = _supplyDal.SaveDraftSupply(
                    supplyInfo,
                    model.IndentID,
                    model.SupplyTypeID,
                    model.DispatchMode ?? string.Empty,
                    model.PackingType ?? string.Empty,
                    model.PackingQty,
                    model.Remarks ?? string.Empty,
                    indentStatusID,
                    model.SupplyDate == default ? DateTime.Today : model.SupplyDate);

                _supplyDal.DeleteSupplyDetails(supplyID);

                if (!string.IsNullOrWhiteSpace(model.ItemsJson))
                {
                    var items = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>>(model.ItemsJson);

                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            int categoryID = Convert.ToInt32(item["categoryID"]);
                            int denominationID = Convert.ToInt32(item["denominationID"]);
                            int supplySheets = Convert.ToInt32(item["supplySheets"]);
                            int supplyPieces = Convert.ToInt32(item["supplyPieces"]);
                            int totalSupplyQty = Convert.ToInt32(item["totalSupplyQty"]);
                            string ledgerFolio = item.ContainsKey("ledgerFolio") ? item["ledgerFolio"]?.ToString() ?? string.Empty : string.Empty;
                            string caseNoFrom = item.ContainsKey("caseNoFrom") ? item["caseNoFrom"]?.ToString() ?? string.Empty : string.Empty;
                            string caseNoTo = item.ContainsKey("caseNoTo") ? item["caseNoTo"]?.ToString() ?? string.Empty : string.Empty;
                            string caseCode = item.ContainsKey("caseCode") ? item["caseCode"]?.ToString() ?? string.Empty : string.Empty;

                            _supplyDal.InsertSupplyDetail(supplyID, categoryID, denominationID, supplySheets, supplyPieces, totalSupplyQty, ledgerFolio, caseNoFrom, caseNoTo, caseCode);
                        }
                    }
                }

                TempData["SuccessMessage"] = "Supply saved as draft successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving supply for IndentID {IndentID}", model?.IndentID);

                TempData["ErrorMessage"] = "Error saving supply: " + ex.Message;

                // repopulate dropdowns
                ViewBag.SupplyTypes = _supplyDal.GetSupplyTypes();
                ViewBag.Offices = _officeDal.GetOfficeList();
                ViewBag.Categories = _stockDal.GetCategories();

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Denominations(int categoryId)
        {
            var dt = _stockDal.GetDenominationsByCategory(categoryId);

            var list = new System.Collections.Generic.List<object>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                list.Add(new { id = row["DenominationID"], text = row["Denomination"] ?? row["DisplayDenomination"] });
            }

            return Json(list);
        }
    }

    public class SupplyDetailsViewModel
    {
        public DataTable Header { get; set; }

        public DataTable Details { get; set; }
    }
}
