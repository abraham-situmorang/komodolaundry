using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.CashBank
{
    public class CashBankDepositDataModel
    {
        public Guid Id { get; set; }

        public string DocNumber { get; set; }

        public string Coa { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}