using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class StockInDataModel
    {
        public Guid Id { get; set; }

        public string Inventory { get; set; }

        public int Qty { get; set; }
    }
}