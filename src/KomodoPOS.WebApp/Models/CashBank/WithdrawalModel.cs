using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.CashBank
{
    public class WithdrawalModel
    {
        public string Id { get; set; }

        public string DocNumber { get; set; }

        public string Note { get; set; }

        public string CashBank { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal TotalAmount { get; set; }

        public List<Models.CashBank.CashBankWithdrawalDataModel> ListCashBankWithdrawalDataModel = new List<Models.CashBank.CashBankWithdrawalDataModel>();
    }
}