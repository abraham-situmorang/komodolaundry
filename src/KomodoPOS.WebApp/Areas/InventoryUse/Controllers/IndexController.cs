using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryUse.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().UseInventories
                .Select(x => new Models.InventoryUseModel()
                {
                    Id = x.Id,
                    IsVoid = x.IsVoid,
                    DocId = x.DocId,
                    Staff = new Service.GetStaffNameByIdServices().Get(x.StaffId),
                    Branch = new Service.GetBranchNameByIdServices().Get(x.BranchId),
                    CreatedDate = x.CreatedDate,
                    Note = x.Note
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        public ActionResult ReadData([DataSourceRequest] DataSourceRequest request, string docId)
        {
            if (string.IsNullOrEmpty(docId)) return Json(null);

            var data = new DataLayer.DADataContext().UseInventoryDatas
                .Where(x => x.DocId == docId)
                .Select(x => new Models.InventoryUseDataModel()
                {
                    Id = x.Id,
                    Inventory = new Service.GetInventoryNameByIdServices().Get(x.InventoryId),
                    Qty = x.Qty,
                    Unit = new Service.GetInventoryUnitByIdServices().Get(x.InventoryId),
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}