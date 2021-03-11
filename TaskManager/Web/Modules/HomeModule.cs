using Utility.ConfigHandler.Config;
using Utility.Encrypt;
using Utility.Filter;
using Utility.Mef;
using Nancy;
using Utility.Admin;
using Utility.Auth;

namespace Web.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            //主页
            Get["/"] = r =>
            {
                return Response.AsRedirect("/Home/Index");
            };

            //主页
            Get["/Home/Index"] = r =>
            {
                return View["index", new { UserName = UserAccountInfo.UserName, Title = SystemConfig.SystemTitle, ProgramName = SystemConfig.ProgramName }];
            };

            ///桌面
            Get["/DestTop"] = r =>
            {
                return View["DestTop", MachineNumber.GetMachineInfo()];
            };

            //修改密码
            Post["/Home/ChgPwd"] = r =>
            {
                ApiResult<string> result = new ApiResult<string>();
                string PasswordOne = this.Request.Form.PasswordOne;
                string PasswordTwo = this.Request.Form.PasswordTwo;
                IUserService UserService = MefConfig.TryResolve<IUserService>();
                if (string.IsNullOrEmpty(PasswordOne) || string.IsNullOrEmpty(PasswordTwo) || !PasswordOne.Equals(PasswordTwo))
                {
                    result.HasError = true;
                    result.Message = "两次密码不一致";
                }
                else
                {
                    UserService.ChgPwd(UserAccountInfo.UserGUID, DESEncrypt.Encrypt(PasswordOne));
                }
                return result;
            };
        }
    }
}
