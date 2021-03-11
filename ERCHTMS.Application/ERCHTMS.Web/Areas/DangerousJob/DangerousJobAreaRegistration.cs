using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.DangerousJob
{
    public class DangerousJobAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DangerousJob";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DangerousJob_default",
                "DangerousJob/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}