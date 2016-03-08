using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Response.Cookies["Username"].Value = "Admin";
            Response.Cookies["Username"].Expires = DateTime.Now.AddDays(10);
            
            ViewBag.Message = "Welcome to KomodoLaundry.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Aplikasi Laundry Online.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "KomodoLaundry contact page.";

            return View();
        }
    }
}
