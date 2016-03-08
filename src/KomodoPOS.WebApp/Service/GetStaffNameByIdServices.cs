using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service
{
    public class GetStaffNameByIdServices
    {
        public string Get(int id)
        {
            return new DataLayer.DADataContext()
            .Staffs
            .FirstOrDefault(x => x.Id == id).Name;
        }
    }
}