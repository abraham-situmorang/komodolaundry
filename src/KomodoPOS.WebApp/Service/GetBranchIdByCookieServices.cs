using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service
{
    public class GetBranchIdByCookieServices
    {
        public int Get(string username)
        {
            return new DataLayer.DADataContext()
            .Staffs
            .FirstOrDefault(x => x.Name == username).BranchId;
        }
    }
}