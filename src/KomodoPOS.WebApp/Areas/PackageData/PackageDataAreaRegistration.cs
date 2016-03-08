using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.PackageData
{
    public class PackageDataAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PackageData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PackageData_default",
                "PackageData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}