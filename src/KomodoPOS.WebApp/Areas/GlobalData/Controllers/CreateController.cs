﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GlobalData.Controllers
{
    public class CreateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string catId, string name, string orderBy)
        {
            try
            {
                var tx = new DataLayer.DADataContext();

                var newData = new DataLayer.GlobalData()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = Guid.Parse(catId),
                    Name = name,
                    OrderBy = int.Parse(orderBy),
                    IsDeleted = false
                };
                tx.GlobalDatas.InsertOnSubmit(newData);
                tx.SubmitChanges();
                tx.Dispose();

                return Json(new { success = true, message = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetGlobalCategory()
        {
            var datas = new DataLayer.DADataContext()
                .GlobalCategories
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