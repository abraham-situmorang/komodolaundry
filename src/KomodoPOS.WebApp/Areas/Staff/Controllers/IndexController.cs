using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Staff.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new DataLayer.DADataContext().Staffs
                .Where(a => a.IsDeleted == false)
                .Select(x => new Models.StaffModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Mobile = x.Mobile,
                    Email = x.Email,
                    Branch = new Service.GetBranchNameByIdServices().Get(x.BranchId),
                    CreatedDate = x.CreatedDate,
                    IsLogin = x.IsLogin,
                    IsActive = x.IsActive,
                    Note = x.Note
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id)
        {
            try
            {
                var tx = new KomodoLaundry.WebApp.DataLayer.DADataContext();

                var data = tx.Staffs.FirstOrDefault(x => x.Id == int.Parse(id));

                data.IsDeleted = true;
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