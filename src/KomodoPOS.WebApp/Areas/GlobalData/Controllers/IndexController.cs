using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GlobalData.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var tx = new DataLayer.DADataContext();
            var result = (from data in tx.GlobalDatas
                    join cat in tx.GlobalCategories on data.CategoryId equals cat.Id
                    where data.IsDeleted == false
                    select new Models.Global.GlobalDataModel()
                    {
                        Id = data.Id,
                        Category = cat.Name,
                        Name = data.Name,
                        OrderBy = data.OrderBy
                    })
                    .OrderBy(o => o.OrderBy)
                .ToList().ToDataSourceResult(request);
            
            return Json(result);
        }
    }
}