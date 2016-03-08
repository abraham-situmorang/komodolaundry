using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class CashDataModel
    {
        public int Id { get; set; }

        public string CashType { get; set; }

        public string Name { get; set; }

        public decimal TotalMoney { get; set; }

        public string Branch { get; set; }
    }
}