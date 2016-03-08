using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Customer.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().Customers
                .Where(a => a.IsDelete == false)
                .Select(x => new Models.CustomerModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Mobile = x.Mobile,
                    Email = x.Email,                    
                    CreatedDate = x.CreatedDate,
                    DepositWeight = x.DepositWeight,
                    IsActive = x.IsActive,
                    Note = x.Note,
                    Branch = new Service.GetBranchNameByIdServices().Get(x.BranchId),
                    RegisterBy = new Service.GetStaffNameByIdServices().Get(x.StaffId)
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.Customers.FirstOrDefault(x => x.Id == int.Parse(id));

                data.IsDelete = true;
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Disabled(string id)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.Customers.FirstOrDefault(x => x.Id == int.Parse(id));

                data.IsActive = false;
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}