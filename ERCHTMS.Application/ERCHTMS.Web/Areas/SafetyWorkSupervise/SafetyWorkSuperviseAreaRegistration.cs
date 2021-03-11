using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.SafetyWorkSupervise
{
    public class SafetyWorkSuperviseAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SafetyWorkSupervise";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SafetyWorkSupervise_default",
                "SafetyWorkSupervise/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}