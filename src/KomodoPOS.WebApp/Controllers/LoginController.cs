using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(string _user, string _password)
        {
            try
            {
                return Json(new { success = true, message = "success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
            

        }
	}
}