using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HseManage
{
    public class HseManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HseManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HseManage_default",
                "HseManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}