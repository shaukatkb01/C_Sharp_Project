using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplyBranch.DataAccess;
using System.Data;

namespace SupplyBranch.Web.Controllers
{
    [Authorize]
    public class IndentController : Controller
    {
        private readonly IndentDAL _indentDal;
        private readonly StockDAL _stockDal;
        private readonly OfficeDAL _officeDal;
        private readonly Microsoft.Extensions.Logging.ILogger<IndentController> _logger;

        public IndentController(IndentDAL indentDal, StockDAL stockDal, OfficeDAL officeDal, Microsoft.Extensions.Logging.ILogger<IndentController> logger)
        {
            _indentDal = indentDal;
            _stockDal = stockDal;
            _officeDal = officeDal;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // List all indents
            DataTable dt = _indentDal.Search(null, null, null, null, null);

            return View(dt);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var header = _indentDal.GetIndentHeader(id);
            var details = _indentDal.GetIndentDetails(id);

            ViewData["Header"] = header;
            ViewData["Details"] = details;

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Offices = _officeDal.GetOfficeList();
            ViewBag.Categories = _stockDal.GetCategories();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SupplyBranch.Web.Models.IndentCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Offices = _officeDal.GetOfficeList();
                    ViewBag.Categories = _stockDal.GetCategories();

                    return View(model);
                }

                var master = new SupplyBranch.Models.IndentMasterModel
                {
                    IndentNo = model.IndentNo,
                    IndentDate = model.IndentDate == default ? DateTime.Today : model.IndentDate,
                    OfficeID = model.OfficeID,
                    Remarks = model.Remarks ?? string.Empty
                };

                DataTable dtItems = new DataTable();
                dtItems.Columns.Add("CategoryID", typeof(int));
                dtItems.Columns.Add("DenominationID", typeof(int));
                dtItems.Columns.Add("SheetQty", typeof(int));
                dtItems.Columns.Add("PieceQty", typeof(int));
                dtItems.Columns.Add("PiecesPerSheet", typeof(int));
                dtItems.Columns.Add("TotalPieces", typeof(int));

                if (!string.IsNullOrWhiteSpace(model.ItemsJson))
                {
                    var items = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>>(model.ItemsJson);

                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            var row = dtItems.NewRow();
                            row["CategoryID"] = Convert.ToInt32(item["CategoryID"]);
                            row["DenominationID"] = Convert.ToInt32(item["DenominationID"]);
                            row["SheetQty"] = Convert.ToInt32(item["SheetQty"]);
                            row["PieceQty"] = Convert.ToInt32(item["PieceQty"]);
                            row["PiecesPerSheet"] = Convert.ToInt32(item["PiecesPerSheet"]);
                            row["TotalPieces"] = Convert.ToInt32(item["TotalPieces"]);

                            dtItems.Rows.Add(row);
                        }
                    }
                }

                int newIndentID = _indentDal.SaveIndent(master, dtItems);

                TempData["SuccessMessage"] = "Indent saved successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving indent {IndentNo}", model?.IndentNo);

                TempData["ErrorMessage"] = "Error saving indent: " + ex.Message;

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
}
