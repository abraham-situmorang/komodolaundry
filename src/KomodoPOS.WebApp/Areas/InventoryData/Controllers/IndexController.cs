using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryData.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().Inventories
                .Where(a => a.IsDeleted == false)
                .Select(x => new Models.InventoryModel()
                {
                    Id = x.Id,
                    InventoryCategory = new Service.GetInventoryCategoryNameByIdServices().Get(x.InventoryCategoryId),
                    Name = x.Name,
                    CreatedDate = x.CreatedDate,
                    Unit = x.Unit,
                    Note = x.Note
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.Inventories.FirstOrDefault(x => x.Id == int.Parse(id));

                data.IsDeleted = true;
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult ReadStock([DataSourceRequest] DataSourceRequest request, string invId)
        {
            if (string.IsNullOrEmpty(invId)) return Json(null);

            var data = new DataLayer.DADataContext().InventoryStocks
                .Where(w => w.InventoryId == int.Parse(invId))
                .Select(x => new Models.InventoryStockModel()
                {
                    Id = x.Id,
                    Branch = new Service.GetBranchNameByIdServices().Get(x.BranchId),
                    Qty = x.Qty
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}