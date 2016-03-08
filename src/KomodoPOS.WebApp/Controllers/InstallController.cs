using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Controllers
{
    public class InstallController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Execute()
        {
            var db = new DataLayer.DADataContext();
            db.CreateDatabase();

            var setting = new DataLayer.Setting()
            {
                Name = "License",
                Value = "Full",
                BranchId = 1,
                Description = "Full License"
            };
            db.Settings.InsertOnSubmit(setting);
            db.SubmitChanges();
            db.Dispose();

            return View();
        }
    }
}