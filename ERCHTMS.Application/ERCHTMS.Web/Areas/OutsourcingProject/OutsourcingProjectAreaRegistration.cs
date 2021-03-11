using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.OutsourcingProject
{
    public class OutsourcingProjectAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OutsourcingProject";
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
