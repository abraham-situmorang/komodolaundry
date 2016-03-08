using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.PackageData.Controllers
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
                            .PackageDatas
                            .Where(x => x.Id == int.Parse(id))
                            .Select(s => new Models.PackageDataModel()
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Price = s.Price,
                                Weight = s.Weight,
                                Note = s.Note
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
        public ActionResult Edit(string id, string name, string price, string weight, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.PackageDatas.FirstOrDefault(x => x.Id == int.Parse(id));

                data.Name = name;
                data.Price = decimal.Parse(price);
                data.Weight = double.Parse(weight);
                data.Note = note;

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