using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.BrandData.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name, string description)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.BrandData()
                {
                    Name = name,
                    Description = description
                };
                tx.BrandDatas.InsertOnSubmit(newData);
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