using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.ServicesData
{
    public class ServicesDataAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ServicesData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ServicesData_default",
                "ServicesData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}