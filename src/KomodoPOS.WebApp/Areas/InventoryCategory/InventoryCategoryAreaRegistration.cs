using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryCategory
{
    public class InventoryCategoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InventoryCategory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InventoryCategory_default",
                "InventoryCategory/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}