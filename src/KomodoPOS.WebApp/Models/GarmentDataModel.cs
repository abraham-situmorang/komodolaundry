using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class GarmentDataModel
    {
        public Guid Id { get; set; }

        public string GarmentCategory { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}