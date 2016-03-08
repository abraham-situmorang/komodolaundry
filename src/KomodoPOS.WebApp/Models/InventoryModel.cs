using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class InventoryModel
    {
        public int Id { get; set; }

        public string InventoryCategory { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Unit { get; set; }

        public string Note { get; set; }
    }
}