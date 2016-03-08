using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Expense.Controllers
{
    public class CreateController : Controller
    {
        private const string _SessionModel_Expense = "_SessionModel_Expense";
        public Models.ExpenseModel Model
        {
            get
            {
                if (Session[_SessionModel_Expense] == null)
                {
                    return (Models.ExpenseModel)(Session[_SessionModel_Expense] = new Models.ExpenseModel());
                }

                return Session[_SessionModel_Expense] as Models.ExpenseModel;
            }
            set { }
        }

        public ActionResult Index()
        {
            Model = new Models.ExpenseModel();
            Model.ListExpenseDataModel = new List<Models.ExpenseDataModel>();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string note, string branchId, string cashBankId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var useInv = new DataLayer.Expense()
                {
                    Id = Guid.NewGuid(),
                    DocNumber = new Service.DocId.GenDocIdServices(branchId).EXP(),
                    Note = note,
                    TotalAmount = Model.ListExpenseDataModel.Sum(sum => sum.Amount),
                    CreatedDate = DateTime.Now,
                    IsVoid = false,
                    StaffId = new Service.GetStaffIdByCookieServices().Get(Request.Cookies["Username"].Value),
                    BranchId = int.Parse(branchId),
                    CashBankId = Guid.Parse(cashBankId)
                };
                tx.Expenses.InsertOnSubmit(useInv);

                foreach (var item in Model.ListExpenseDataModel)
                {
                    var useData = new DataLayer.ExpenseData()
                    {
                        Id = item.Id,
                        CoaId = Guid.Parse(item.CoaExpense),
                        DocNumber = useInv.DocNumber,
                        Description = item.Description,
                        Amount = item.Amount
                    };
                    tx.ExpenseDatas.InsertOnSubmit(useData);
                }

                //coa transaction/bank transaction
                new Service.Account.CoaTransactionServices()
                    .Credit(Guid.Parse(cashBankId), Models.AutoDocumentName.EXP, useInv.DocNumber, Model.ListExpenseDataModel.Sum(sum => sum.Amount));

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
            var data = Model.ListExpenseDataModel
                .Select(s => new Models.ExpenseDataModel()
                {
                    Id = s.Id,
                    CoaExpense = new Service.Account.GetCoaNameByIdServices().Get(Guid.Parse(s.CoaExpense)),
                    Description = s.Description,
                    Amount = s.Amount
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddData(string coaId, string description, string amount)
        {
            try
            {
                Model.ListExpenseDataModel.Add(
                    new Models.ExpenseDataModel()
                    {
                        Id = Guid.NewGuid(),
                        CoaExpense = coaId,
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
                var data = Model.ListExpenseDataModel.FirstOrDefault(x => x.Id == Guid.Parse(id));

                Model.ListExpenseDataModel.Remove(data);

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #region ComboBox
        public JsonResult GetBranchData()
        {
            var datas = new DataLayer.DADataContext()
                .BranchDatas
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
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
        #endregion
    }
}