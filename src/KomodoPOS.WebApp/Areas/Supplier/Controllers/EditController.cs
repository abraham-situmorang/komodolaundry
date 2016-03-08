using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Supplier.Controllers
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
                            .SupplierDatas
                            .FirstOrDefault(x => x.Id == int.Parse(id));

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, string name, string address, string contactPerson, string contactNumber, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var data = tx.SupplierDatas.FirstOrDefault(x => x.Id == int.Parse(id));

                data.Name = name;
                data.Address = address;
                data.ContactPerson = contactPerson;
                data.ContactNumber = contactNumber;
                data.Note = note;

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