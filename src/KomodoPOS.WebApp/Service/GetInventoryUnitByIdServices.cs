﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service
{
    public class GetInventoryUnitByIdServices
    {
        public string Get(int id)
        {
            return new DataLayer.DADataContext()
            .Inventories
            .FirstOrDefault(x => x.Id == id).Unit;
        }
    }
}