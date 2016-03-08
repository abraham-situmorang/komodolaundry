using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryPurchase
{
    public class InventoryPurchaseAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InventoryPurchase";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InventoryPurchase_default",
                "InventoryPurchase/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}