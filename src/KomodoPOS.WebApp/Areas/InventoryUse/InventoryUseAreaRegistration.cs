using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryUse
{
    public class InventoryUseAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InventoryUse";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InventoryUse_default",
                "InventoryUse/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}