using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Expense.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().Expenses
                .Select(x => new Models.ExpenseModel()
                {
                    Id = x.Id.ToString(),
                    DocNumber = x.DocNumber,
                    Note = x.Note,
                    TotalAmount = x.TotalAmount,
                    CreatedDate = x.CreatedDate,
                    IsVoid = x.IsVoid,                    
                    Staff = new Service.GetStaffNameByIdServices().Get(x.StaffId),
                    Branch = new Service.GetBranchNameByIdServices().Get(x.BranchId),
                    CashBank = new Service.Account.GetCoaNameByIdServices().Get(x.CashBankId)
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        public ActionResult ReadData([DataSourceRequest] DataSourceRequest request, string docId)
        {
            if (string.IsNullOrEmpty(docId)) return Json(null);

            var data = new DataLayer.DADataContext().ExpenseDatas
                .Where(x => x.DocNumber == docId)
                .Select(x => new Models.ExpenseDataModel()
                {
                    Id = x.Id,
                    CoaExpense = x.DocNumber,
                    Description = x.Description,
                    Amount = x.Amount,
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}