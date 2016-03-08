using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.BranchType.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.BranchType()
                {
                    Name = name
                };
                tx.BranchTypes.InsertOnSubmit(newData);
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