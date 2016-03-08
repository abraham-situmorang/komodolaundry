using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryData.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string inventoryCategoryId, string name, string unit, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.Inventory()
                {
                    InventoryCategoryId = int.Parse(inventoryCategoryId),
                    Name = name,
                    CreatedDate = DateTime.Now,
                    Unit = unit,
                    IsDeleted = false,
                    Note = note
                };
                tx.Inventories.InsertOnSubmit(newData);
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetInventoryCategory()
        {
            var datas = new DataLayer.DADataContext()
                .InventoryCategories
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }
    }
}