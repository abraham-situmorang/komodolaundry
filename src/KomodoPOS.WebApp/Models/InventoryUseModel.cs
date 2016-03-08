using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class InventoryUseModel
    {
        public Guid Id { get; set; }

        public bool IsVoid { get; set; }

        public string DocId { get; set; }

        public string Staff { get; set; }

        public string Branch { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Note { get; set; }

        public List<Models.InventoryUseDataModel> ListInventoryUseDataModel = new List<InventoryUseDataModel>();
    }
}