using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.Order
{
    public class OrderDataModel
    {
        public Guid Id { get; set; }

        public string DocNumber { get; set; }

        public string Note { get; set; }

        public int? Qty { get; set; }

        public double? Weight { get; set; }

        public string Brand { get; set; }
        public string Colour { get; set; }
        public string Garment { get; set; }
    }
}