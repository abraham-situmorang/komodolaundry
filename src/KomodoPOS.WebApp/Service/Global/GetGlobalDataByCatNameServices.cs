using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service.Global
{
    public class GetGlobalDataByCatNameServices
    {
        public List<Models.Global.GlobalDataModel> Get(string catName)
        {
            var tx =  new DataLayer.DADataContext();
            
            return (from data in tx.GlobalDatas
                    join cat in tx.GlobalCategories on data.CategoryId equals cat.Id
                    where cat.Name == catName && data.IsDeleted == false
                    select new Models.Global.GlobalDataModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        OrderBy = data.OrderBy,
                        Category = cat.Name
                    })
                    .OrderBy(o => o.OrderBy).ToList();
        }
    }
}