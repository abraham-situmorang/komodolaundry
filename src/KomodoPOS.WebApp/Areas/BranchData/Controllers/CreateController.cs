using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.BranchData.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name, string note, string branchId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.BranchData()
                {
                    Name = name,
                    Note = note,
                    BranchTypeId = int.Parse(branchId)
                };
                tx.BranchDatas.InsertOnSubmit(newData);
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetBranchType()
        {
            var datas = new DataLayer.DADataContext()
                .BranchTypes
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