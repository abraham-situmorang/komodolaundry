using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service
{
    public class GetGarmentCatByIdServices
    {
        public string Get(int id)
        {
            return new DataLayer.DADataContext()
            .GarmentCategories
            .FirstOrDefault(x => x.Id == id).Name;
        }
    }
}