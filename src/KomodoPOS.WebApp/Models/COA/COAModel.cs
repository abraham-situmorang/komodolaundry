using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.COA
{
    public class COAModel
    {
        public Guid Id { get; set; }

        public string GlobalData { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Saldo { get; set; }
    }
}