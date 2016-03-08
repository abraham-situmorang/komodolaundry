using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service.Account
{
    public class GetCOACategoryAllServices
    {
        public List<Models.Global.GlobalDataModel> Get()
        {
            return new Service.Global.GetGlobalDataByCatNameServices()
                .Get(Models.Global.GlobalValueModel.CoaCategory);
        }
    }
}