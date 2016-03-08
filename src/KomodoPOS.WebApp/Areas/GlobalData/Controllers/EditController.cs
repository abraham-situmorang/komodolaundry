using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GlobalData.Controllers
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
                            .GlobalDatas
                            .Where(x => x.Id == Guid.Parse(id) && x.IsDeleted == false)
                            .Select(s => new Models.Global.GlobalDataModel()
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Category = s.CategoryId.ToString(),
                                OrderBy = s.OrderBy
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
        public ActionResult Edit(string id, string catId, string name, string orderBy)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.GlobalDatas.FirstOrDefault(x => x.Id == Guid.Parse(id));

                data.CategoryId = Guid.Parse(catId);
                data.Name = name;
                data.OrderBy = int.Parse(orderBy);
                
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