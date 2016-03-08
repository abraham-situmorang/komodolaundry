using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.COA.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string coaCategoryId, string name, string code)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.COA()
                {
                    Id = Guid.NewGuid(),
                    GlobalDataId =Guid.Parse(coaCategoryId),
                    Name = name,
                    Code = code,
                    Saldo = 0,
                };
                tx.COAs.InsertOnSubmit(newData);
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetCOACategory()
        {
            var datas = new Service.Account.GetCOACategoryAllServices().Get()
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