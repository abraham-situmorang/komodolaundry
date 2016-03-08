using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service
{
    public class GetBranchNameByIdServices
    {
        public string Get(int id)
        {
            return new DataLayer.DADataContext()
            .BranchDatas
            .FirstOrDefault(x => x.Id == id).Name;
        }

        public string GetIsNull(int? id)
        {
            if (!id.HasValue) return "(ALL)";

            return new DataLayer.DADataContext()
            .BranchDatas
            .FirstOrDefault(x => x.Id == id).Name;
        }
    }
}