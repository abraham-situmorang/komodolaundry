using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GarmentData.Controllers
{
    public class EditController : Controller
    {
        public ActionResult Index(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetData(string id)
        {
            try
            {
                var data = new DataLayer.DADataContext()
                            .GarmentDatas
                            .Where(x => x.Id == Guid.Parse(id))
                            .Select(s => new Models.GarmentDataModel()
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Description = s.Description,
                                GarmentCategory = s.GarmentCategoryId.ToString()
                            })
                            .FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, string garmentCatId, string name, string description)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.GarmentDatas.FirstOrDefault(x => x.Id == Guid.Parse(id));

                data.Name = name;
                data.Description = description;
                data.GarmentCategoryId = int.Parse(garmentCatId);

                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}