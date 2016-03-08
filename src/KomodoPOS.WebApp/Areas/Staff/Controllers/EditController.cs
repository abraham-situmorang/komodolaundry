using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Staff.Controllers
{
    public class EditController : Controller
    {
        public ActionResult Index(string id)
        {
            ViewBag.Id = id;
            return View();
        }
        
        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult GetData(string id)
        //{
        //    try
        //    {
        //        var data = new DataLayer.KomodoPOSDataDataContext()
        //                    .Staffs
        //                    .Where(x => x.Id == Guid.Parse(id))
        //                    .Select(s => new Models.StaffModel()
        //                    {
        //                        Id = s.Id.ToString(),
        //                        FullName = s.Full_Name,
        //                        Username = s.Username,
        //                        Email = s.Email,
        //                        Location = s.Location_Id.ToString(),
                                
        //                        Cassier = s.Cassier,
        //                        ManageCash = s.Manage_Cash,
        //                        ManageCustomer = s.Manage_Customer,
        //                        ManageProductPrice = s.Manage_Product_Price,
        //                        CancelingTransaction = s.Canceling_Transaction
        //                    })
        //                    .FirstOrDefault();

        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Edit(string id, string fullName, string username, string email,
        //    string password, string locationId,
        //    string cassier, string manageCash, string manageCustomer,
        //    string manageProductPrice, string cancelingTransaction
        //    )
        //{
        //    try
        //    {
        //        var tx = new DataLayer.KomodoPOSDataDataContext();

        //        var data = tx.Staffs.FirstOrDefault(x => x.Id == Guid.Parse(id));

        //        data.Full_Name = fullName;
        //        data.Username = username;
        //        data.Email = email;
        //        data.Location_Id = Guid.Parse(locationId);
                                
        //        data.Cassier = bool.Parse(cassier);
        //        data.Manage_Cash = bool.Parse(manageCash);
        //        data.Manage_Customer = bool.Parse(manageCustomer);
        //        data.Manage_Product_Price = bool.Parse(manageProductPrice);
        //        data.Canceling_Transaction = bool.Parse(cancelingTransaction);

        //        tx.SubmitChanges();
        //        tx.Dispose();

        //        return Json(new { success = true, message = "success" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}
	}
}