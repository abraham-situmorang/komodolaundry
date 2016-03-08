using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.CashBank
{
    public class TransferModel
    {
        public string Id { get; set; }

        public string DocNumber { get; set; }

        public string Note { get; set; }

        public string FromCoa { get; set; }

        public string ToCoa { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}