using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Staff.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name, string address, string mobile,
            string password, string email, string branchId, string isLogin, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.Staff()
                {
                    Name = name,
                    Address = address,
                    Mobile = mobile,
                    Password = password,
                    Email = email,
                    BranchId = int.Parse(branchId),
                    CreatedDate = DateTime.Now,
                    IsLogin = bool.Parse(isLogin),
                    IsActive = true,
                    IsDeleted = false,
                    Note = note
                };
                tx.Staffs.InsertOnSubmit(newData);
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetBranch()
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
    }
}