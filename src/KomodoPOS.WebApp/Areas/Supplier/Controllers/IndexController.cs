using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Supplier.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().SupplierDatas
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}