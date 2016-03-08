using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace KomodoLaundry.WebApp.Areas.Customer.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string name, string address, string mobile,
            string email, string note, string branchId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newCustomer = new DataLayer.Customer()
                {
                    Name = name,
                    Address = address,
                    Mobile = mobile,
                    Email = email,
                    CreatedDate = DateTime.Now,
                    DepositWeight = 0,
                    IsActive = true,
                    IsDelete = false,
                    Note = note,
                    BranchId = int.Parse(branchId),
                    StaffId = new Service.GetStaffIdByCookieServices().Get(Request.Cookies["Username"].Value)
                };
                tx.Customers.InsertOnSubmit(newCustomer);
                tx.SubmitChanges();
                tx.Dispose();

                MailMessage mail = new MailMessage("john@gmail.com", newCustomer.Email);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.google.com";
                mail.Subject = "Welcome to my laundry shop";
                mail.Body = "success registration to our system.";
                mail.Attachments.Add(new Attachment(@"C:\data\document1.pdf"));

                client.Send(mail);


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