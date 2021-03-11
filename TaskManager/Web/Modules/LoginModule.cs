using Model.Common;
using Nancy;
using Nancy.Extensions;
using Nancy.Authentication.Forms;
using System;
using Utility.Auth;
using Utility.ConfigHandler.Config;
using Utility.Encrypt;
using Utility.Mef;

namespace Web.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
            : base("Login")
        {
            Get["/"] = r =>
            {
                var ErrorMsg = this.Request.Query.ErrorMsg;
                var UserCode = this.Request.Query.UserCode;
                return View["index", new
                {
                    ErrorMsg = ErrorMsg,
                    UserCode = UserCode,
                    Title = SystemConfig.SystemTitle,
                    ProgramName = SystemConfig.ProgramName,
                    ShowOriginalAccountInfo = SystemConfig.ShowOriginalAccountInfo ? "1" : "0"
                }];
            };

            Post["/"] = r =>
            {
                string UserCode ="";
                string Password = "";
                if (this.Request.Url.ToString().Contains("args"))
                {
                     string args = this.Request.Query.args;
                     args = Utility.Encrypt.DESEncrypt.Decrypt(args, "!2#3@1YV");
                     UserCode = args.Split('|')[0];
                     Password = args.Split('|')[1];
                }
                else
                {
                    UserCode = this.Request.Form.UserCode;
                    Password = this.Request.Form.Password;
                }
                IUserService UserService = MefConfig.TryResolve<IUserService>();
                if (string.IsNullOrEmpty(UserCode) || string.IsNullOrEmpty(Password))
                {

                    return this.Context.GetRedirect("~/Login?ErrorMsg=" + Uri.EscapeDataString("用户名或密码不能为空") + "&UserCode=" + UserCode);
                }
                UserAccount account = UserService.GetUserInfo(UserCode, DESEncrypt.Encrypt(Password));
                if (account == null)
                {
                    return this.Context.GetRedirect("~/Login?ErrorMsg=" + Uri.EscapeDataString("用户名或密码错误") + "&UserCode=" + UserCode);
                }
                else
                {
                    Guid guid = Guid.ParseExact(account.UserGUID, "N");
                    Session["UserInfo"] = account;
                    return this.Login(guid, DateTime.Now.AddDays(1));
                }
            };
            Get["/SignIn"] = r =>
            {
                string UserCode = "";
                string Password = "";
                if (this.Request.Url.ToString().Contains("args"))
                {
                    string args = this.Request.Query.args;
                    args = Utility.Encrypt.DESEncrypt.Decrypt(args, "!2#3@1YV");
                    UserCode = args.Split('|')[0];
                    Password = args.Split('|')[1];
                }
                else
                {
                    UserCode = this.Request.Form.UserCode;
                    Password = this.Request.Form.Password;
                }
                IUserService UserService = MefConfig.TryResolve<IUserService>();
                if (string.IsNullOrEmpty(UserCode) || string.IsNullOrEmpty(Password))
                {

                    return this.Context.GetRedirect("~/Login?ErrorMsg=" + Uri.EscapeDataString("用户名或密码不能为空") + "&UserCode=" + UserCode);
                }
                UserAccount account = UserService.GetUserInfo(UserCode, DESEncrypt.Encrypt(Password));
                if (account == null)
                {
                    return this.Context.GetRedirect("~/Login?ErrorMsg=" + Uri.EscapeDataString("用户名或密码错误") + "&UserCode=" + UserCode);
                }
                else
                {
                    Guid guid = Guid.ParseExact(account.UserGUID, "N");
                    Session["UserInfo"] = account;
                    return this.Login(guid, DateTime.Now.AddDays(1), "/Task/Grid");
                }
            };

            //退出登录
            Get["/Exit"] = r =>
            {
                Session.DeleteAll();
                return this.Context.GetRedirect("~/Login");
            };
        }
    }
}
