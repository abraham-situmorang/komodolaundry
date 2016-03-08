using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.StockIn.Controllers
{
    public class CreateController : Controller
    {
        private const string _SessionModel_StockIn = "_SessionModel_StockIn";
        public Models.StockInModel Model
        {
            get
            {
                if (Session[_SessionModel_StockIn] == null)
                {
                    return (Models.StockInModel)(Session[_SessionModel_StockIn] = new Models.StockInModel());
                }

                return Session[_SessionModel_StockIn] as Models.StockInModel;
            }
            set { }
        }

        public ActionResult Index()
        {
            Model = new Models.StockInModel();
            Model.ListStockInDataModel = new List<Models.StockInDataModel>();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string branchId, string note)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var stockIn = new DataLayer.StockIn()
                {
                    Id = Guid.NewGuid(),
                    BranchId = int.Parse(branchId),
                    DocId = new Service.DocId.GenDocIdServices(branchId).STO(),
                    StaffId = new Service.GetStaffIdByCookieServices().Get(Request.Cookies["Username"].Value),
                    CreatedDate = DateTime.Now,
                    Note = note
                };
                tx.StockIns.InsertOnSubmit(stockIn);
                tx.SubmitChanges();

                foreach (var item in Model.ListStockInDataModel)
                {
                    var stockData = new DataLayer.StockInData()
                    {
                        Id = item.Id,
                        DocId = stockIn.DocId,
                        InventoryId = int.Parse(item.Inventory),
                        Qty = item.Qty
                    };
                    tx.StockInDatas.InsertOnSubmit(stockData);

                    var inv = tx.InventoryStocks
                                .FirstOrDefault(x => x.BranchId == stockIn.BranchId
                                && x.InventoryId == stockData.InventoryId);

                    if (inv == null)
                    {
                        new DataLayer.InventoryStock()
                            {
                                BranchId = stockIn.BranchId,
                                InventoryId = stockData.InventoryId,
                                Qty = stockData.Qty
                            };
                    }
                    else
                    {
                        inv.Qty += stockData.Qty;
                    }                    
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
            var data = Model.ListStockInDataModel
                .Select(x => new Models.StockInDataModel()
                {
                    Id = x.Id,
                    Inventory = new Service.GetInventoryNameByIdServices().Get(int.Parse(x.Inventory)),
                    Qty = x.Qty
                })
                .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddData(string invId, string qty)
        {
            try
            {
                Model.ListStockInDataModel.Add(
                    new Models.StockInDataModel()
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
                var data = Model.ListStockInDataModel.FirstOrDefault(x => x.Id == Guid.Parse(id));

                Model.ListStockInDataModel.Remove(data);

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