using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryUse.Controllers
{
    public class CreateController : Controller
    {
        private const string _SessionModel_InventoryUse = "_SessionModel_InventoryUse";
        public Models.InventoryUseModel Model
        {
            get
            {
                if (Session[_SessionModel_InventoryUse] == null)
                {
                    return (Models.InventoryUseModel)(Session[_SessionModel_InventoryUse] = new Models.InventoryUseModel());
                }

                return Session[_SessionModel_InventoryUse] as Models.InventoryUseModel;
            }
            set { }
        }

        public ActionResult Index()
        {
            Model = new Models.InventoryUseModel();
            Model.ListInventoryUseDataModel = new List<Models.InventoryUseDataModel>();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string branchId, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var useInv = new DataLayer.UseInventory()
                {
                    Id = Guid.NewGuid(),
                    DocId = new Service.DocId.GenDocIdServices(branchId).USE(),
                    CreatedDate = DateTime.Now,
                    IsVoid = false,
                    StaffId = new Service.GetStaffIdByCookieServices().Get(Request.Cookies["Username"].Value),
                    BranchId = int.Parse(branchId),
                    Note = note
                };
                tx.UseInventories.InsertOnSubmit(useInv);

                foreach (var item in Model.ListInventoryUseDataModel)
                {
                    var useData = new DataLayer.UseInventoryData()
                    {
                        Id = item.Id,
                        DocId = useInv.DocId,
                        InventoryId = int.Parse(item.Inventory),
                        Qty = item.Qty
                    };
                    tx.UseInventoryDatas.InsertOnSubmit(useData);

                    var inv = tx.InventoryStocks
                                .FirstOrDefault
                                (x => x.InventoryId == useData.InventoryId
                                && x.BranchId == useInv.BranchId);
                    try
                    {
                        if (inv == null)
                            throw new ApplicationException("Tidak ada stok untuk inventory: " + new Service.GetInventoryNameByIdServices().Get(int.Parse(item.Inventory)));
                        
                        if (useData.Qty > inv.Qty)
                            throw new ApplicationException("Stok untuk Inventory: '" + new Service.GetInventoryNameByIdServices().Get(int.Parse(item.Inventory)) + "' tidak cukup. Masukkan jumlah yang lebih kecil atau sama dengan " + inv.Qty.ToString());
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = ex.Message });
                    }

                    inv.Qty -= useData.Qty;                    
                }
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
            var data = Model.ListInventoryUseDataModel
                .Select(x => new Models.InventoryUseDataModel()
                {
                    Id = x.Id,
                    Inventory = new Service.GetInventoryNameByIdServices().Get(int.Parse(x.Inventory)),
                    Qty = x.Qty,
                    Unit = new Service.GetInventoryUnitByIdServices().Get(int.Parse(x.Inventory)),
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddData(string invId, string qty)
        {
            try
            {
                Model.ListInventoryUseDataModel.Add(
                    new Models.InventoryUseDataModel()
                    {
                        Id = Guid.NewGuid(),
                        Inventory = invId,
                        Qty = int.Parse(qty)
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
                var data = Model.ListInventoryUseDataModel.FirstOrDefault(x => x.Id == Guid.Parse(id));

                Model.ListInventoryUseDataModel.Remove(data);

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
    }
}