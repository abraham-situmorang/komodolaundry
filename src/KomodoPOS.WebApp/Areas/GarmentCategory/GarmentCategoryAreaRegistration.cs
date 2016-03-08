using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.GarmentCategory
{
    public class GarmentCategoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GarmentCategory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GarmentCategory_default",
                "GarmentCategory/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}