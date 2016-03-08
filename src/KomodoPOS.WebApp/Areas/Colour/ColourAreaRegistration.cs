using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.Colour
{
    public class ColourAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Colour";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Colour_default",
                "Colour/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}