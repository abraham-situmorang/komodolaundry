using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.CashBank.Controllers
{
    public class DetailController : Controller
    {
        public ActionResult Index(string id)
        {
            var coa = new DataLayer.DADataContext().COAs.FirstOrDefault(x => x.Id == Guid.Parse(id));
            ViewBag.Id = id;
            ViewBag.Message = coa.Code + " " + coa.Name;
            return View();
        }
        
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string id)
        {
            var tx = new DataLayer.DADataContext();

            var data = (from tran in tx.COATransactions
                        join coa in tx.COAs on tran.COAId equals coa.Id
                        where tran.COAId == Guid.Parse(id)
                        select new Models.COA.COATransactionModel()
                        {
                            Id = coa.Id,
                            CreatedDate = tran.CreatedDate,
                            Name = tran.Name,
                            Description = tran.Description,
                            Debit = tran.Debit.HasValue ? tran.Debit.Value : 0,
                            Credit = tran.Credit.HasValue ? tran.Credit.Value : 0,
                            Balance = tran.Balance
                        })
                    .OrderByDescending(o => o.CreatedDate).ToList().ToDataSourceResult(request);
            return Json(data);
        }
	}
}