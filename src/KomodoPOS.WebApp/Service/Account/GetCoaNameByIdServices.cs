using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service.Account
{
    public class GetCoaNameByIdServices
    {
        public string Get(Guid id)
        {
            return new DataLayer.DADataContext()
            .COAs
            .FirstOrDefault(x => x.Id == id).Name;
        }
    }
}