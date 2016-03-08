using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.PackageData.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name, string price, string weight, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.PackageData()
                {
                    Name = name,
                    Price = decimal.Parse(price),
                    Weight = double.Parse(weight),
                    Note = note
                };
                tx.PackageDatas.InsertOnSubmit(newData);
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