using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class InventoryPurchaseDataModel
    {
        public Guid Id { get; set; }

        public string Inventory { get; set; }

        public int Qty { get; set; }

        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Amount { get; set; }
    }
}