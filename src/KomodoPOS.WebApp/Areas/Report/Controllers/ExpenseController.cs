using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Report.Controllers
{
    public class ExpenseController : Controller
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
                    Name = x.Name,
                    Description = x.Description,
                    Money = x.Money,
                    Cash = new Service.GetCashNameByIdServices().Get(x.CashId),
                    CreatedDate = DateTime.Now,
                    IsVoid = false,
                    Branch = new Service.GetBranchNameByIdServices().Get(x.BranchId),
                    Staff = new Service.GetStaffNameByIdServices().Get(x.StaffId),
                    ExpenseType = new Service.GetExpenseTypeNameByIdServices().Get(x.ExpenseTypeId)                    
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }
    }
}