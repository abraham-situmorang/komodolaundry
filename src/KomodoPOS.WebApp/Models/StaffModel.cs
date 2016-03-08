using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class StaffModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Branch { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsLogin { get; set; }

        public bool IsActive { get; set; }

        public string Note { get; set; }
    }
}