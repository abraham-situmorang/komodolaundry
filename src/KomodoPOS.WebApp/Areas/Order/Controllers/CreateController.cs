using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Order.Controllers
{
    public class CreateController : Controller
    {
        private const string _SessionModel_NewOrder = "_SessionModel_NewOrder";
        public Models.Order.OrderModel Model
        {
            get
            {
                if (Session[_SessionModel_NewOrder] == null)
                {
                    return (Models.Order.OrderModel)(Session[_SessionModel_NewOrder] = new Models.Order.OrderModel());
                }

                return Session[_SessionModel_NewOrder] as Models.Order.OrderModel;
            }
            set { }
        }

        public ActionResult Index()
        {
            Model = new Models.Order.OrderModel();
            Model.ListOrderDataModel = new List<Models.Order.OrderDataModel>();
            
            ViewBag.DocNumber = new Service.DocId.GenDocIdServices(true).ORD();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string customerId, string branchId, string docNumber, string note,
            string totalItem, string totalWeight, string serviceId, string packageId)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var ord = new DataLayer.ORD()
                {
                    Id = Guid.NewGuid(),
                    DocNumber = docNumber,
                    Note = note,
                    IsVoid = false,
                    StaffId = new Service.GetStaffIdByCookieServices().Get(Request.Cookies["Username"].Value),
                    BranchId = int.Parse(branchId),
                    SourceBranchId = int.Parse(branchId),
                    CreatedDate = DateTime.Now,
                    TotalItem = int.Parse(totalItem),
                    TotalWeight = double.Parse(totalWeight),
                    //TODO: new order status add global data
                    Status = "NEW ORDER",
                    IsCompletePay = false,
                    TotalPayWeight = 0,
                    ServicesDataId = int.Parse(serviceId),
                    PackageDataId = int.Parse(packageId)
                };
                tx.ORDs.InsertOnSubmit(ord);

                foreach (var item in Model.ListOrderDataModel)
                {
                    var useData = new DataLayer.ORDData()
                    {
                        Id = item.Id,
                        DocNumber = ord.DocNumber,
                        Note = item.Note,
                        Qty = item.Qty,
                        Weight = item.Weight,
                        BrandDataId = string.IsNullOrEmpty(item.Brand) ? (int?)null : int.Parse(item.Brand),
                        ColourDataId = string.IsNullOrEmpty(item.Colour) ? (int?)null : int.Parse(item.Colour),
                        GarmentId = string.IsNullOrEmpty(item.Garment) ? (Guid?)null : Guid.Parse(item.Garment)
                    };
                    tx.ORDDatas.InsertOnSubmit(useData);
                }

                new Service.DocId.GenDocIdServices().SaveLastIdFunction(Models.Global.DocumentAbbreviation.ORD);

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
            var tx = new DataLayer.DADataContext();
            var data = (from ordData in Model.ListOrderDataModel
                        join brand in tx.BrandDatas on int.Parse(ordData.Brand) equals brand.Id
                        join colour in tx.Colours on int.Parse(ordData.Colour) equals colour.Id
                        join garment in tx.GarmentDatas on Guid.Parse(ordData.Garment) equals garment.Id
                        select new Models.Order.OrderDataModel()
                        {
                            Id = ordData.Id,
                            Note = ordData.Note,
                            Qty = ordData.Qty,
                            Weight = ordData.Weight,
                            Brand = brand.Name,
                            Colour = colour.Name,
                            Garment = garment.Name
                        })
                        .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddData(string note, string qty, string weight,
            string brandId, string colourId, string garmentId)
        {
            try
            {
                Model.ListOrderDataModel.Add(
                    new Models.Order.OrderDataModel()
                    {
                        Id = Guid.NewGuid(),
                        Note = note,
                        Qty = string.IsNullOrEmpty(qty) ? (int?)null : int.Parse(qty),
                        Weight = string.IsNullOrEmpty(weight) ? (double?)null : double.Parse(weight),
                        Brand = string.IsNullOrEmpty(brandId) ? null : brandId,
                        Colour = string.IsNullOrEmpty(colourId) ? null : colourId,
                        Garment = string.IsNullOrEmpty(garmentId) ? null : garmentId
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
                var data = Model.ListOrderDataModel.FirstOrDefault(x => x.Id == Guid.Parse(id));

                Model.ListOrderDataModel.Remove(data);

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #region combobox Master
        public JsonResult GetCustomerData()
        {
            var datas = new DataLayer.DADataContext()
                .Customers
                .Where(w => w.IsActive)
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name + " (" + y.Mobile + ")"
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
        public JsonResult GetServiceData()
        {
            var datas = new DataLayer.DADataContext()
                .ServicesDatas
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPackageData()
        {
            var datas = new DataLayer.DADataContext()
                .PackageDatas
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region combobox Detail
        public JsonResult GetBrandData()
        {
            var datas = new DataLayer.DADataContext()
                .BrandDatas
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetColourData()
        {
            var datas = new DataLayer.DADataContext()
                .Colours
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGarmentData()
        {
            var datas = new DataLayer.DADataContext()
                .GarmentDatas
                .Select(y => new Models.GeneralModel()
                {
                    Key = y.Id.ToString(),
                    Display = y.Name
                }).ToList();

            return Json(datas,
                JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}