using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.Order
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public string DocNumber { get; set; }

        public string Note { get; set; }

        public bool IsVoid { get; set; }

        public string Staff { get; set; }
        public string Branch { get; set; }
        public string SourceBranch { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? TotalItem { get; set; }
        public double? TotalWeight { get; set; }
        public string Status { get; set; }
        public bool IsCompletePay { get; set; }
        public double? TotalPayWeight { get; set; }
        public string Services { get; set; }
        public string Package { get; set; }

        public List<Models.Order.OrderDataModel> ListOrderDataModel = new List<Models.Order.OrderDataModel>();
    }
}