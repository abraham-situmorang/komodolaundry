using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Customer.Controllers
{
    public class EditController : Controller
    {
        public ActionResult Index(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetData(string id)
        {
            try
            {
                var data = new DataLayer.DADataContext()
                            .Customers
                            .Where(x => x.Id == int.Parse(id))
                            .Select(s => new Models.CustomerModel()
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Address = s.Address,
                                Mobile = s.Mobile,
                                Email = s.Email,
                                Note = s.Note,
                                Branch = s.BranchId.ToString()
                            })
                            .FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, string name, string address, string mobile,
            string email, string note, string branchId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.Customers.FirstOrDefault(x => x.Id == int.Parse(id));

                data.Name = name;
                data.Address = address;
                data.Mobile = mobile;
                data.Email = email;
                data.Note = note;
                data.BranchId = int.Parse(branchId);

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