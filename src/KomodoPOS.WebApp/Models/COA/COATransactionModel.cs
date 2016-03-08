using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.COA
{
    public class COATransactionModel
    {
        public Guid Id { get; set; }

        public string COA { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public decimal Balance { get; set; }
    }
}