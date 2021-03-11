using Model.Common;
using Nancy;
using Nancy.Security;
using Utility.ConfigHandler.Config;

namespace Web.Modules
{
    public class BaseModule : NancyModule
    {
        public UserAccount UserAccountInfo = null;

        public BaseModule()
        {
            this.RequiresAuthentication();
            Init();
        }

        public BaseModule(string modulePath)
            : base(modulePath)
        {
            this.RequiresAuthentication();
            Init();
        }

        private void Init()
        {
            Before += ctx =>
            {
                //静态资源版本
                ViewBag.Version = SystemConfig.StaticVersion;
                UserAccountInfo = Session["UserInfo"] as UserAccount;
                return null;
            };
        }
    }
}
