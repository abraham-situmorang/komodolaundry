using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Customer.Controllers
{
    public class DepositController : Controller
    {
        public ActionResult Index(string id)
        {
            ViewBag.DocNumber = new Service.DocId.GenDocIdServices(true).DEP();
            ViewBag.Id = id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string customerId, string docNumber, string amount,
            string cashBankCoaId,
            string payMethodId, string methodCardName, string methodCardNumber,
            string isRegular, string serviceId,
            string packageId,
            string note, string branchId)
        {
            try
            {
                bool methodCard = false;
                if (payMethodId == "1")
                    methodCard = true;

                bool valIsRegular = false;
                if (bool.Parse(isRegular))
                    valIsRegular = true;

                if (valIsRegular && string.IsNullOrEmpty(amount))
                    throw new ApplicationException("Jumlah deposit harus dimasukkan.");

                //price get from package price
                if (valIsRegular == false)
                    amount = "0";

                //MANDATORY
                if (valIsRegular == true && string.IsNullOrEmpty(serviceId))
                    throw new ApplicationException("Layanan harus dipilih.");
                if (valIsRegular == false && string.IsNullOrEmpty(packageId))
                    throw new ApplicationException("Paket harus dipilih");

                if (methodCard && string.IsNullOrEmpty(methodCardName))
                    throw new ApplicationException("Kolom Nama Kartu harus diisi.");
                if (methodCard && string.IsNullOrEmpty(methodCardNumber))
                    throw new ApplicationException("Kolom Nomor Kartu harus diisi.");
                //END MANDATORY

                var tx = new DataLayer.DADataContext();

                var newDeposit = new DataLayer.CustomerDeposit()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = int.Parse(customerId),
                    DocNumber = docNumber,
                    Amount = decimal.Parse(amount),
                    CashBankCoaId = Guid.Parse(cashBankCoaId),
                    
                    PayMethodId = int.Parse(payMethodId),
                    
                    MethodCard = methodCard,
                    MethodCardName = methodCard ? methodCardName : null,
                    MethodCardNumber = methodCard ? methodCardNumber : null,
                    
                    IsRegular = valIsRegular,
                    ServiceId = valIsRegular ? int.Parse(serviceId) : (int?)null,

                    PackageId = valIsRegular ? (int?)null : int.Parse(packageId),
                    
                    CreatedDate = DateTime.Now,
                    IsVoid = false,
                    Note = note,
                    BranchId = int.Parse(branchId),
                    StaffId = new Service.GetStaffIdByCookieServices().Get(Request.Cookies["Username"].Value)
                };
                tx.CustomerDeposits.InsertOnSubmit(newDeposit);

                //add to customer data
                var customer = tx.Customers.FirstOrDefault(x => x.Id == newDeposit.CustomerId);

                double addWeight = 0;
                if (newDeposit.IsRegular)
                {
                    //get service price by select serviceId
                    decimal servicePrice = tx.ServicesDatas.FirstOrDefault(x => x.Id == newDeposit.ServiceId.Value).Price;
                    decimal resultWeight = newDeposit.Amount / servicePrice;
                    addWeight = double.Parse(resultWeight.ToString());
                }
                else
                {
                    //get package price by select packageId
                    var packageData = tx.PackageDatas.FirstOrDefault(x => x.Id == newDeposit.PackageId.Value);
                    addWeight = packageData.Weight;
                    newDeposit.Amount += packageData.Price;
                }

                customer.DepositWeight += addWeight;

                //coa transaction/bank transaction
                new Service.Account.CoaTransactionServices()
                    .Debit(newDeposit.CashBankCoaId, Models.AutoDocumentName.DEP, newDeposit.DocNumber, newDeposit.Amount);

                //add income
                //TODO: add income here

                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success", id = newDeposit.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        public JsonResult GetCashBankData()
        {
            var tx = new DataLayer.DADataContext();
            var datas = (from coa in tx.COAs
                         join globalData in tx.GlobalDatas on coa.GlobalDataId equals globalData.Id
                         where globalData.Name == Models.Global.GlobalValueModel.CashBank
                         select new Models.GeneralModel()
                         {
                             Key = coa.Id.ToString(),
                             Display = coa.Name + " (" + coa.Code + ")"
                         })
                    .ToList();

            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackageData()
        {
            var datas = new DataLayer.DADataContext()
                .PackageDatas
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name + " (" + y.Price.ToString() + ")"
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetServiceData()
        {
            var datas = new DataLayer.DADataContext()
                .ServicesDatas
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name + " (" + y.Price.ToString() + ")"
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchData()
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
        
        public JsonResult GetPayMethodData()
        {
            var datas = new DataLayer.DADataContext()
                .PayMethods
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