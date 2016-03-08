using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GlobalCategory
{
    public class GlobalCategoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GlobalCategory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GlobalCategory_default",
                "GlobalCategory/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}