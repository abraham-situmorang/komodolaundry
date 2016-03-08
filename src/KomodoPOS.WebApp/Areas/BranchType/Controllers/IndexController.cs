using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.BranchType.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().BranchTypes
                .Select(x => new Models.BranchTypeModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
	}
}