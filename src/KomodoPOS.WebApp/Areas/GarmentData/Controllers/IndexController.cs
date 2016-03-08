using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GarmentData.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().GarmentDatas
                .Select(x => new Models.GarmentDataModel()
                {
                    Id = x.Id,
                    GarmentCategory = new Service.GetGarmentCatByIdServices().Get(x.GarmentCategoryId),
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}