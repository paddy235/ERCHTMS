using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.DangerousJobConfig
{
    public class DangerousJobConfigAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DangerousJobConfig";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DangerousJobConfig_default",
                "DangerousJobConfig/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}