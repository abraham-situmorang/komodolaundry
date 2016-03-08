using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Supplier.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name, string address, string contactPerson, string contactNumber, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.SupplierData()
                {
                    Name = name,
                    Address = address,
                    ContactPerson = contactPerson,
                    ContactNumber = contactNumber,
                    Note = note
                };
                tx.SupplierDatas.InsertOnSubmit(newData);
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