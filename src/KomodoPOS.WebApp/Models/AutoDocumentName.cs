using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public static class AutoDocumentName
    {
        /// <summary>
        /// ORDER (ORD)
        /// </summary>
        public const string ORD = "ORDER (ORD)";

        /// <summary>
        /// PAYMENT (PAY)
        /// </summary>
        public const string PAY = "PAYMENT (PAY)";
        
        /// <summary>
        /// PURCHASE ORDER (PUR)
        /// </summary>
        public const string PUR = "PURCHASE ORDER (PUR)";

        /// <summary>
        /// EXPENSE (EXP)
        /// </summary>
        public const string EXP = "EXPENSE (EXP)";

        /// <summary>
        /// INCOME (INC)
        /// </summary>
        public const string INC = "INCOME (INC)";

        /// <summary>
        /// Bank Transfer (BTR)
        /// </summary>
        public const string BTR = "Bank Transfer (BTR)";

        /// <summary>
        /// Bank Deposit (BDE)
        /// </summary>
        public const string BDE = "Bank Deposit (BDE)";

        /// <summary>
        /// Bank Withdrawal (BWD)
        /// </summary>
        public const string BWD = "Bank Withdrawal (BWD)";

        /// <summary>
        /// Customer Deposit (DEP)
        /// </summary>
        public const string DEP = "Customer Deposit (DEP)";
    }
}