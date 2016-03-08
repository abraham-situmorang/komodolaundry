using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GarmentData.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string garmentCatId, string name, string description)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.GarmentData()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Description = description,
                    GarmentCategoryId = int.Parse(garmentCatId),
                };
                tx.GarmentDatas.InsertOnSubmit(newData);
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult GetGarmentCategory()
        {
            var datas = new DataLayer.DADataContext()
                .GarmentCategories
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