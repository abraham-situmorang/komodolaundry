using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.BranchData.Controllers
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
                            .BranchDatas
                            .Where(x => x.Id == int.Parse(id))
                            .Select(s => new Models.BranchDataModel()
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Note = s.Note,
                                BranchType = s.BranchTypeId.ToString()
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
        public ActionResult Edit(string id, string name, string note, string branchId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.BranchDatas.FirstOrDefault(x => x.Id == int.Parse(id));

                data.Name = name;
                data.Note = note;
                data.BranchTypeId = int.Parse(branchId);

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