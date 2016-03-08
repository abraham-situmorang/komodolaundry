using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class ExpenseModel
    {
        public string Id { get; set; }

        public string DocNumber { get; set; }

        public string Note { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsVoid { get; set; }

        public string Branch { get; set; }

        public string Staff { get; set; }

        public string CashBank { get; set; }

        public List<Models.ExpenseDataModel> ListExpenseDataModel = new List<ExpenseDataModel>();
    }
}