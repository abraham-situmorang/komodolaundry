using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.CashBank.Controllers
{
    public class WithdrawalController : Controller
    {
        private const string _SessionModel_CashBankWithdrawal = "_SessionModel_CashBankWithdrawal";
        public Models.CashBank.WithdrawalModel Model
        {
            get
            {
                if (Session[_SessionModel_CashBankWithdrawal] == null)
                {
                    return (Models.CashBank.WithdrawalModel)(Session[_SessionModel_CashBankWithdrawal] = new Models.CashBank.WithdrawalModel());
                }

                return Session[_SessionModel_CashBankWithdrawal] as Models.CashBank.WithdrawalModel;
            }
            set { }
        }

        public ActionResult Index()
        {
            Model = new Models.CashBank.WithdrawalModel();
            Model.ListCashBankWithdrawalDataModel = new List<Models.CashBank.CashBankWithdrawalDataModel>();

            ViewBag.DocNumber = new Service.DocId.GenDocIdServices(true).BWD();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string docNumber, string note, string cashBankId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();
                
                var cashBank = new DataLayer.CashBankWithdrawal()
                {
                    Id = Guid.NewGuid(),
                    DocNumber = docNumber,
                    Note = note,
                    CashBankId = Guid.Parse(cashBankId),
                    CreatedDate = DateTime.Now,
                    TotalAmount = Model.ListCashBankWithdrawalDataModel.Sum(sum => sum.Amount),
                };
                tx.CashBankWithdrawals.InsertOnSubmit(cashBank);

                foreach (var item in Model.ListCashBankWithdrawalDataModel)
                {
                    var useData = new DataLayer.CashBankWithdrawalData()
                    {
                        Id = item.Id,
                        DocNumber = cashBank.DocNumber,
                        CoaId = Guid.Parse(item.Coa),
                        Description = item.Description,
                        Amount = item.Amount
                    };
                    tx.CashBankWithdrawalDatas.InsertOnSubmit(useData);
                }

                //coa transaction/bank transaction
                new Service.Account.CoaTransactionServices()
                    .Credit(cashBank.CashBankId, Models.AutoDocumentName.BWD, cashBank.DocNumber, Model.ListCashBankWithdrawalDataModel.Sum(x => x.Amount));

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

            var data = Model.ListCashBankWithdrawalDataModel
                .Join(tx.COAs,
                deposit => Guid.Parse(deposit.Coa),
                coa => coa.Id,
                (deposit, coa) => new { deposit, coa })
                .Select(x => new Models.CashBank.CashBankWithdrawalDataModel()
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
                Model.ListCashBankWithdrawalDataModel.Add(
                    new Models.CashBank.CashBankWithdrawalDataModel()
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
                var data = Model.ListCashBankWithdrawalDataModel.FirstOrDefault(x => x.Id == Guid.Parse(id));

                Model.ListCashBankWithdrawalDataModel.Remove(data);

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