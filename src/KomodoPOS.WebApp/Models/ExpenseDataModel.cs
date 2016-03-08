using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class ExpenseDataModel
    {
        public Guid Id { get; set; }

        public string CoaExpense { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}