using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class BranchDataModel
    {
        public int Id { get; set; }

        public string BranchType { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }
    }
}