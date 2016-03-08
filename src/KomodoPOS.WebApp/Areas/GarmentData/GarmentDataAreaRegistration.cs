using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GarmentData
{
    public class GarmentDataAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GarmentData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GarmentData_default",
                "GarmentData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}