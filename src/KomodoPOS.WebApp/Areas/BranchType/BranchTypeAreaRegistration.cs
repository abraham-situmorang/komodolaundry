using System.Web.Mvc;

namespace KomodoLaundry.WebApp.Areas.BranchType
{
    public class BranchTypeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BranchType";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BranchType_default",
                "BranchType/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}