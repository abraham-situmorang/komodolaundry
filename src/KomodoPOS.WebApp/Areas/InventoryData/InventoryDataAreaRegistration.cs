using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.InventoryData
{
    public class InventoryDataAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InventoryData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InventoryData_default",
                "InventoryData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}