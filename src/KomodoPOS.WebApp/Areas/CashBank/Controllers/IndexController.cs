using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.CashBank.Controllers
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

            var data = (from coa in tx.COAs
                    join globalData in tx.GlobalDatas on coa.GlobalDataId equals globalData.Id
                    where globalData.Name == Models.Global.GlobalValueModel.CashBank
                    select new Models.COA.COAModel()
                    {
                        Id = coa.Id,
                        GlobalData = globalData.Name,
                        Code = coa.Code,
                        Name = coa.Name,
                        Saldo = coa.Saldo
                    })
                    .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}