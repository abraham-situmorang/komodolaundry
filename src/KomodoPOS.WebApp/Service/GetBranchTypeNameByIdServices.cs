using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service
{
    public class GetBranchTypeNameByIdServices
    {
        public string Get(int id)
        {
            return new DataLayer.DADataContext()
            .BranchTypes
            .FirstOrDefault(x => x.Id == id).Name;
        }
    }
}