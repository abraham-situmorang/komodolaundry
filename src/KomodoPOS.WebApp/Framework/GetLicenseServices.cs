using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Framework
{
    public static class GetLicenseServices
    {
        public static bool Get()
        {
            string val = new DataLayer.DADataContext()
            .Settings
            .FirstOrDefault(x => x.Name == "License").Value;

            if (val == "FREE") return true;
            return false;
        }
    }
}