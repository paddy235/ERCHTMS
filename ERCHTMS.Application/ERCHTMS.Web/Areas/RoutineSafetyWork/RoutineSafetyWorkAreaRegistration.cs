using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork
{
    public class RoutineSafetyWorkAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RoutineSafetyWork";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              this.AreaName + "_Default",
              this.AreaName + "/{controller}/{action}/{id}",
              new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
              new string[] { "ERCHTMS.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
