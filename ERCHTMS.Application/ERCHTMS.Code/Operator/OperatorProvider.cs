using BSFramework.Cache.Factory;
using BSFramework.Util;
using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Web;
using System.Web.Security;

namespace ERCHTMS.Code
{
    /// <summary>
    /// 描 述：当前操作者回话
    /// </summary>
    public class OperatorProvider : OperatorIProvider
    {
        #region 静态实例
        /// <summary>
        /// 当前提供者
        /// </summary>
        public static OperatorIProvider Provider
        {
            get { return new OperatorProvider(); }
        }
        /// <summary>
        /// 给app调用
        /// </summary>
        public static string AppUserId
        {
            set;
            get;
        }
        #endregion

        /// <summary>
        /// 秘钥
        /// </summary>
        private string LoginUserKey = Config.GetValue("SoftName");
        /// <summary>
        /// 登陆提供者模式:Session、Cookie 
        /// </summary>
        private string LoginProvider = Config.GetValue("LoginProvider");
        /// <summary>
        /// 写入登录信息
        /// </summary>
        /// <param name="user">成员信息</param>
        public virtual void AddCurrent(Operator userData)
        {
            try
            {
                //if (LoginProvider == "Cookie")
                //{
                //    CacheFactory.Cache().WriteCache(userData.DataAuthorize, userData.UserId + "_" + LoginUserKey, userData.LogTime.AddHours(12));
                //    //WebHelper.WriteCookie(LoginUserKey, DESEncrypt.Encrypt(new { UserId = userData.UserId, Account = userData.Account, UserName = userData.UserName, IsSystem = userData.IsSystem, Token = userData.Token, Code = userData.Code, DeptCode = userData.DeptCode, OrganizeCode = userData.OrganizeCode, OrganizeId = userData.OrganizeId, DeptId = userData.DeptId, RoleName = userData.RoleName, PostName = userData.PostName, DeptName = userData.DeptName, OrganizeName = userData.OrganizeName, Password = userData.Password, Secretkey = userData.Secretkey }.ToJson()));
                //}
                //else
                //{
                //    WebHelper.WriteSession(LoginUserKey, DESEncrypt.Encrypt(userData.ToJson()));
                //}
                if (LoginProvider == "Cookie")
                {
                    FormsAuth.SignIn(userData.UserId, userData, 2 * 60);
                }
                CacheFactory.Cache().WriteCache(userData, "UID_"+userData.UserId + "_" + LoginUserKey, userData.LogTime.AddHours(24));
                CacheFactory.Cache().WriteCache(userData.Token, userData.UserId + "_" + LoginUserKey, userData.LogTime.AddHours(24));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public virtual string GetUserId()
        {
            try
            {
                string userId = "";
                string json = HttpContext.Current.Request["json"];
                if (!string.IsNullOrEmpty(json))
                {
                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                    userId = dy.userid;
                }
                return userId;
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public virtual Operator Current()
        {
            try
            {
                Operator user = new Operator();
                string userId = GetUserId();
                userId = "UID_" + userId + "_" + LoginUserKey;
                user = CacheFactory.Cache().GetCache<Operator>(userId);
                if (user!=null)
                {
                    return user;
                }
                if (LoginProvider == "AppClient")
                {
                    if (string.IsNullOrEmpty(userId))
                    {
                        user = CacheFactory.Cache().GetCache<Operator>("UID_" + AppUserId + "_" + LoginUserKey);
                    }
                    else
                    {
                        user = CacheFactory.Cache().GetCache<Operator>(userId);
                    }
                }
                else
                {
                    string key = FormsAuth.GetUserKey();
                    user = string.IsNullOrEmpty(key) ? null : CacheFactory.Cache().GetCache<Operator>("UID_" + key + "_" + LoginUserKey);
                    if (user == null)
                    {
                        if (string.IsNullOrEmpty(userId))
                        {
                            if (!string.IsNullOrEmpty(AppUserId))
                            {
                                user = CacheFactory.Cache().GetCache<Operator>("UID_" + AppUserId + "_" + LoginUserKey);
                            }
                        }
                        else
                        {
                            user = CacheFactory.Cache().GetCache<Operator>(userId);
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 删除登录信息
        /// </summary>
        public virtual void EmptyCurrent()
        {
            FormsAuth.SingOut();
            if (LoginProvider == "Cookie")
            {
                string val = WebHelper.GetCookie(LoginUserKey);
                if (!string.IsNullOrEmpty(val))
                {
                    string userId = DESEncrypt.Decrypt(val);
                    WebHelper.RemoveCookie(LoginUserKey.Trim());
                    //CacheFactory.Cache().RemoveCache(LoginUserKey);
                    //CacheFactory.Cache().RemoveCache(userId + "_" + LoginUserKey);
                    //CacheFactory.Cache().RemoveCache("UID_" + userId + "_" + LoginUserKey);
                }

            }
            else
            {
                WebHelper.RemoveSession(LoginUserKey);
            }
        }
        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOverdue()
        {
            try
            {
                //object str = "";
                ////AuthorizeDataModel dataAuthorize = null;
                //if (LoginProvider == "Cookie")
                //{
                //    str = WebHelper.GetCookie(LoginUserKey);
                //    Operator user = DESEncrypt.Decrypt(str.ToString()).ToObject<Operator>();
                //    //dataAuthorize = CacheFactory.Cache().GetCache<AuthorizeDataModel>(user.UserId + "_" + LoginUserKey);
                //    //if (dataAuthorize==null)
                //    //{
                //    //    return true;
                //    //}
                //    //if (string.IsNullOrEmpty(WebHelper.GetCookie(FormsAuthentication.FormsCookieName)))
                //    //{
                //    //    return true;
                //    //}
                //    if (string.IsNullOrEmpty(WebHelper.GetCookie(LoginUserKey)))
                //    {
                //        return true;
                //    }
                //}
                //else
                //{
                //    str = WebHelper.GetSession(LoginUserKey);
                //}
                //if (str != null && str.ToString() != "")
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}

                if (string.IsNullOrEmpty(WebHelper.GetCookie(Config.GetValue("SoftName"))))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }
        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public virtual int IsOnLine()
        {
            Operator user = new Operator();
            string key = FormsAuth.GetUserKey();
            if (string.IsNullOrEmpty(key))
            {
                return -1;
            }
            key = "UID_" + key + "_" + LoginUserKey;
            user = CacheFactory.Cache().GetCache<Operator>(key);
            if (user == null)
            {
                return -1;
            }
            //if (LoginProvider == "Cookie")
            //{
            //    //user = DESEncrypt.Decrypt(WebHelper.GetCookie(LoginUserKey).ToString()).ToObject<Operator>();
            //     //AuthorizeDataModel dataAuthorize = CacheFactory.Cache().GetCache<AuthorizeDataModel>(user.UserId + "_" + LoginUserKey);
            //}
            //else
            //{
            //    user = DESEncrypt.Decrypt(WebHelper.GetSession(LoginUserKey).ToString()).ToObject<Operator>();
            //}
           
            object token = CacheFactory.Cache().GetCache<string>(user.UserId + "_" + LoginUserKey);
            if (token == null)
            {
                return -1;//过期
            }
            if (user.Token == token.ToString())
            {
                return 1;//正常
            }
            else
            {
                return 0;//已登录
            }
        }
    }
}
