using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.StockIn
{
    public class StockInAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "StockIn";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "StockIn_default",
                "StockIn/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}