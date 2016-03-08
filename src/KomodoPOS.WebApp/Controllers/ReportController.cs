using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult Index(string name, string id, string file)
        {
            LocalReport report = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), name + ".rdlc");
            if (System.IO.File.Exists(path))
                report.ReportPath = path;
            else
                return View("Index");

            var customers = new DataLayer.DADataContext().Customers
                //.Where(x => x.Id == Guid.Parse(id))
                                            .ToList();

            //customers = new List<DataLayer.Customer>();
            ReportDataSource data1 = new ReportDataSource("DataSet1", customers);
            report.DataSources.Add(data1);

            string reportType = file;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + file + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = report.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }
	}
}