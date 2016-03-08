using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class InventoryStockModel
    {
        public int Id { get; set; }

        public string Branch { get; set; }

        public int Qty { get; set; }
    }
}