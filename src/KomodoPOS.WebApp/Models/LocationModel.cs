using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Models
{
    public class LocationModel
    {
        public string Id { get; set; }

        public string NamaGerai { get; set; }

        public string Alamat1 { get; set; }

        public string Alamat2 { get; set; }

        public string Kota { get; set; }

        public string Note { get; set; }

        public bool IsActive { get; set; }
    }
}