using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models.Global
{
    public class GlobalDataModel
    {
        public Guid Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public int OrderBy { get; set; }
    }
}