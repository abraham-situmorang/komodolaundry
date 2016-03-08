using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.CashBank.Controllers
{
    public class DepositController : Controller
    {
        private const string _SessionModel_CashBankDeposit = "_SessionModel_CashBankDeposit";
        public Models.CashBank.DepositModel Model
        {
            get
            {
                if (Session[_SessionModel_CashBankDeposit] == null)
                {
                    return (Models.CashBank.DepositModel)(Session[_SessionModel_CashBankDeposit] = new Models.CashBank.DepositModel());
                }

                return Session[_SessionModel_CashBankDeposit] as Models.CashBank.DepositModel;
            }
            set { }
        }

        public ActionResult Index()
        {
            Model = new Models.CashBank.DepositModel();
            Model.ListCashBankDepositDataModel = new List<Models.CashBank.CashBankDepositDataModel>();

            ViewBag.DocNumber = new Service.DocId.GenDocIdServices(true).BDE();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string docNumber, string note, string cashBankId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var cashBank = new DataLayer.CashBankDeposit()
                {
                    Id = Guid.NewGuid(),
                    DocNumber = docNumber,
                    Note = note,
                    CashBankId = Guid.Parse(cashBankId),
                    CreatedDate = DateTime.Now,
                    TotalAmount = Model.ListCashBankDepositDataModel.Sum(sum => sum.Amount),
                };
                tx.CashBankDeposits.InsertOnSubmit(cashBank);

                foreach (var item in Model.ListCashBankDepositDataModel)
                {
                    var useData = new DataLayer.CashBankDepositData()
                    {
                        Id = item.Id,
                        DocNumber = cashBank.DocNumber,
                        CoaId = Guid.Parse(item.Coa),
                        Description = item.Description,
                        Amount = item.Amount
                    };
                    tx.CashBankDepositDatas.InsertOnSubmit(useData);
                }
                
                //coa transaction/bank transaction
                new Service.Account.CoaTransactionServices()
                    .Debit(cashBank.CashBankId, Models.AutoDocumentName.BDE, cashBank.DocNumber, Model.ListCashBankDepositDataModel.Sum(x => x.Amount));
                
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var tx = new DataLayer.DADataContext();
            
            var data = Model.ListCashBankDepositDataModel
                .Join(tx.COAs,
                deposit => Guid.Parse(deposit.Coa),
                coa => coa.Id,
                (deposit, coa) => new { deposit, coa })
                .Select(x => new Models.CashBank.CashBankDepositDataModel()
                {
                    Id = x.deposit.Id,
                    Coa = x.coa.Name,
                    Description = x.deposit.Description,
                    Amount = x.deposit.Amount
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddData(string coaId, string description, string amount)
        {
            try
            {
                Model.ListCashBankDepositDataModel.Add(
                    new Models.CashBank.CashBankDepositDataModel()
                    {
                        Id = Guid.NewGuid(),
                        Coa = coaId,
                        Description = description,
                        Amount = decimal.Parse(amount)
                    }
                );

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DelData(string id)
        {
            try
            {
                var data = Model.ListCashBankDepositDataModel.FirstOrDefault(x => x.Id == Guid.Parse(id));

                Model.ListCashBankDepositDataModel.Remove(data);

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        public JsonResult GetCoaData()
        {
            var tx = new DataLayer.DADataContext();
            var datas = (from coa in tx.COAs
                         join globalData in tx.GlobalDatas on coa.GlobalDataId equals globalData.Id
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