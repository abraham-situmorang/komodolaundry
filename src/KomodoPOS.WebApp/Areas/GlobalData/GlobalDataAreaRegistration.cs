using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GlobalData
{
    public class GlobalDataAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GlobalData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GlobalData_default",
                "GlobalData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}