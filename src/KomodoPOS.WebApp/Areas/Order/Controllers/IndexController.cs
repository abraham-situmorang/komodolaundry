using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Order.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var tx = new DataLayer.DADataContext();

            var data = (from ord in tx.ORDs
                        join staf in tx.Staffs on ord.StaffId equals staf.Id
                        join branch in tx.BranchDatas on ord.BranchId equals branch.Id
                        join service in tx.ServicesDatas on ord.ServicesDataId equals service.Id
                        join package in tx.PackageDatas on ord.PackageDataId equals package.Id
                        select new Models.Order.OrderModel()
                        {
                            Id = ord.Id,
                            DocNumber = ord.DocNumber,
                            Note = ord.Note,
                            IsVoid = ord.IsVoid,
                            Staff = staf.Name,
                            Branch = branch.Name,
                            SourceBranch = branch.Name,
                            CreatedDate = ord.CreatedDate,
                            TotalItem = ord.TotalItem,
                            TotalWeight = ord.TotalWeight,
                            Status = ord.Status,
                            IsCompletePay = ord.IsCompletePay,
                            TotalPayWeight = ord.TotalPayWeight,
                            Services = service.Name,
                            Package = package.Name
                        })
                        .ToList().ToDataSourceResult(request);
            return Json(data);
        }

        public ActionResult ReadData([DataSourceRequest] DataSourceRequest request, string docId)
        {
            var tx = new DataLayer.DADataContext();

            var data = (from ordData in tx.ORDDatas
                        join brand in tx.BrandDatas on ordData.BrandDataId equals brand.Id
                        join colour in tx.Colours on ordData.ColourDataId equals colour.Id
                        join garment in tx.GarmentDatas on ordData.GarmentId equals garment.Id
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

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}