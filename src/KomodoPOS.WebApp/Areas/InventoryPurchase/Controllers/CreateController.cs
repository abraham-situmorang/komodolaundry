using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryPurchase.Controllers
{
    public class CreateController : Controller
    {
        private const string _SessionModel_InventoryPurchase = "_SessionModel_InventoryPurchase";
        public Models.InventoryPurchaseModel Model
        {
            get
            {
                if (Session[_SessionModel_InventoryPurchase] == null)
                {
                    return (Models.InventoryPurchaseModel)(Session[_SessionModel_InventoryPurchase] = new Models.InventoryPurchaseModel());
                }

                return Session[_SessionModel_InventoryPurchase] as Models.InventoryPurchaseModel;
            }
            set { }
        }

        public ActionResult Index()
        {
            Model = new Models.InventoryPurchaseModel();
            Model.ListInventoryPurchaseDataModel = new List<Models.InventoryPurchaseDataModel>();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string branchId, string note, string cashId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var useInv = new DataLayer.PurchaseInventory()
                {
                    Id = Guid.NewGuid(),
                    DocId = new Service.DocId.GenDocIdServices(branchId).PUR(),
                    CreatedDate = DateTime.Now,
                    IsVoid = false,
                    StaffId = new Service.GetStaffIdByCookieServices().Get(Request.Cookies["Username"].Value),
                    BranchId = int.Parse(branchId),
                    Note = note,
                    CashBankId = Guid.Parse(cashId)
                };
                tx.PurchaseInventories.InsertOnSubmit(useInv);

                foreach (var item in Model.ListInventoryPurchaseDataModel)
                {
                    var useData = new DataLayer.PurchaseInventoryData()
                    {
                        Id = item.Id,
                        DocId = useInv.DocId,
                        InventoryId = int.Parse(item.Inventory),
                        Qty = item.Qty,
                        UnitPrice = item.UnitPrice
                    };
                    tx.PurchaseInventoryDatas.InsertOnSubmit(useData);
                }

                //coa transaction/bank transaction
                new Service.Account.CoaTransactionServices()
                    .Credit(useInv.CashBankId, Models.AutoDocumentName.PUR, useInv.DocId, Model.ListInventoryPurchaseDataModel.Sum(x => x.Amount));
                
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var data = Model.ListInventoryPurchaseDataModel
                .Select(x => new Models.InventoryPurchaseDataModel()
                {
                    Id = x.Id,
                    Inventory = new Service.GetInventoryNameByIdServices().Get(int.Parse(x.Inventory)),
                    Qty = x.Qty,
                    Unit = new Service.GetInventoryUnitByIdServices().Get(int.Parse(x.Inventory)),
                    UnitPrice = x.UnitPrice,
                    Amount = x.UnitPrice * x.Qty,
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddData(string invId, string qty, string unitPrice)
        {
            try
            {
                Model.ListInventoryPurchaseDataModel.Add(
                    new Models.InventoryPurchaseDataModel()
                    {
                        Id = Guid.NewGuid(),
                        Inventory = invId,
                        Qty = int.Parse(qty),
                        UnitPrice = decimal.Parse(unitPrice),
                        Amount = decimal.Parse(unitPrice) * int.Parse(qty),
                    }
                );

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DelData(string id)
        {
            try
            {
                var data = Model.ListInventoryPurchaseDataModel.FirstOrDefault(x => x.Id == Guid.Parse(id));

                Model.ListInventoryPurchaseDataModel.Remove(data);

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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

        public JsonResult GetInventoryData()
        {
            var datas = new DataLayer.DADataContext()
                .Inventories
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
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
                             Display = "(" + coa.Code + ") " + coa.Name
                         })
                    .ToList();

            return Json(datas, JsonRequestBehavior.AllowGet);
        }
    }
}