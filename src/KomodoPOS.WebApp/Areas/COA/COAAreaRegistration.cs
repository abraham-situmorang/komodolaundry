using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.COA
{
    public class COAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "COA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "COA_default",
                "COA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}