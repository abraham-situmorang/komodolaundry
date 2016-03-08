using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Customer.Controllers
{
    public class DepositDetailController : Controller
    {
        public ActionResult Index(string msg, string id)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.Msg = "true";
            }
            var model = new DataLayer.DADataContext().CustomerDeposits
                            .FirstOrDefault(x => x.Id == Guid.Parse(id));
            return View(model);
        }
	}
}