using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }

        public double DepositWeight { get; set; }

        public bool IsActive { get; set; }

        public string Note { get; set; }

        public string RegisterBy { get; set; }

        public string Branch { get; set; }
    }
}