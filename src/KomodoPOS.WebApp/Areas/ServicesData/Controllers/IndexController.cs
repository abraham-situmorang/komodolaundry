using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.ServicesData.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().ServicesDatas
                .Select(x => new Models.ServicesDataModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Duration = x.Duration,
                    Note = x.Note
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}