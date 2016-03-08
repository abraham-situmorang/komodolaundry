using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.CashBank.Controllers
{
    public class TransferController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.DocNumber = new Service.DocId.GenDocIdServices(true).BTR();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string docNumber, string note, string fromCoa, string toCoa, string amount)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.CashBankTransfer()
                {
                    Id = Guid.NewGuid(),
                    DocNumber = docNumber,
                    Note = note,
                    FromCoaId = Guid.Parse(fromCoa),
                    ToCoaId = Guid.Parse(toCoa),
                    Amount = decimal.Parse(amount),
                    CreatedDate = DateTime.Now
                };
                tx.CashBankTransfers.InsertOnSubmit(newData);

                //coa transaction/bank transaction
                new Service.Account.CoaTransactionServices()
                    .Credit(newData.FromCoaId, Models.AutoDocumentName.BTR, newData.DocNumber, newData.Amount);
                
                new Service.Account.CoaTransactionServices()
                    .Debit(newData.ToCoaId, Models.AutoDocumentName.BTR, newData.DocNumber, newData.Amount);

                new Service.DocId.GenDocIdServices().SaveLastIdFunction(Models.Global.DocumentAbbreviation.BTR);
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetCashBankData()
        {
            var tx = new DataLayer.DADataContext();
            var datas = (from coa in tx.COAs
                         join globalData in tx.GlobalDatas on coa.GlobalDataId equals globalData.Id
                         where globalData.Name == Models.Global.GlobalValueModel.CashBank
                         select new Models.GeneralModel()
                         {
                             Key = coa.Id.ToString(),
                             Display = "(" + coa.Code + ") " + coa.Name
                         })
                    .ToList();

            return Json(datas, JsonRequestBehavior.AllowGet);
        }
	}
}