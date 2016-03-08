using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class IncomeModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Money { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsVoid { get; set; }

        public string Branch { get; set; }

        public string Staff { get; set; }
    }
}