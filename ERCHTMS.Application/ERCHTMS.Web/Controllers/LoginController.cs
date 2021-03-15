using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.KbsDeviceManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using ServiceStack.Common;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml;
using ERCHTMS.Entity.PersonManage;

namespace ERCHTMS.Web.Controllers
{
    /// <summary>
    /// 描 述：系统登录
    /// </summary>
    //[HandlerLogin(LoginMode.Ignore)]
    //[HandlerAuthorize(PermissionMode.Ignore)]
    public class LoginController : Controller
    {

        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private HikinoutlogBLL hikinoutlogbll = new HikinoutlogBLL();
        #region 视图功能
        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }

        /// <summary>
        /// 西塞山大屏首页（主要展示外包和高风险信息）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult XSSIndex()
        {
            return View();
        }
        /// <summary>
        /// 京泰1号岗大屏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult JTOneScreen()
        {
            return View();
        }
        /// <summary>
        /// 测试用于定时任务激活w3wp进程时调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ActiveProcess(string keyValue="0123456")
        {
            var fileList = new Busines.PublicInfoManage.FileInfoBLL().GetEntity(keyValue);
            return Success(fileList.ToJson());
        }

        /// <summary>
        /// 可门一号大屏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Screen()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BJIndex(string code = "00005001001001001")
        {
            DepartmentBLL deptBll = new DepartmentBLL();
            DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='较大风险'", code));
            ViewBag.Jdcount = dtCount.Rows[0][0].ToString();
            dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='重大风险'", code));
            ViewBag.Zdcount = dtCount.Rows[0][0].ToString();
            return View();
        }
        [HttpGet]
        public ActionResult JTIndex(string code = "00005001001001001")
        {
            DepartmentBLL deptBll = new DepartmentBLL();
            DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='较大风险'", code));
            ViewBag.Jdcount = dtCount.Rows[0][0].ToString();
            dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='重大风险'", code));
            ViewBag.Zdcount = dtCount.Rows[0][0].ToString();
            return View();
        }
        /// <summary>
        /// 广西华昇
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HSIndex(string code = "00019")
        {
            DepartmentBLL deptBll = new DepartmentBLL();
            DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='较大风险'", code));
            ViewBag.Jdcount = dtCount.Rows[0][0].ToString();
            dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='重大风险'", code));
            ViewBag.Zdcount = dtCount.Rows[0][0].ToString();
            return View();
        }
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            string bzLoginUrl = new DataItemDetailBLL().GetItemValue("bzLoginUrl");
            if (!string.IsNullOrEmpty(bzLoginUrl))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index1");
            }
        }
        [HttpGet]
        public ActionResult Index1()
        {

            var di = new DataItemDetailBLL();
            string ldaplandurl = di.GetItemValue("LDAPLandUrl");
            string IsOpenPassword = di.GetItemValue("IsOpenPassword");
            if (IsOpenPassword != "true")
            {
                return View();
            }
            try
            {
                string username = Request.Headers["OAM_REMOTE_USER"];
                if (!string.IsNullOrWhiteSpace(username))
                {
                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 1;
                    logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                    logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                    logEntity.OperateAccount = username;
                    logEntity.OperateUserId = username;
                    logEntity.Module = Config.GetValue("SoftName");
                    logEntity.OperateAccount = username;
                    try
                    {
                        #region 内部账户验证
                        UserBLL userBLL = new UserBLL();
                        UserInfoEntity userEntity = userBLL.GetUserInfoByAccount(username);
                        bool result = SetUserInfo(userEntity);
                        if (result)
                        {
                            //if(userEntity.)
                            Response.Redirect(ldaplandurl);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        WebHelper.RemoveCookie("autologin");                  //清除自动登录
                        logEntity.ExecuteResult = -1;
                        logEntity.ExecuteResultJson = ex.Message;
                        logEntity.WriteLog();
                        return Error(ex.Message);
                    }
                }
                else
                {

                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 华电可门首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult KmIndex()
        {
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {//可门配置信息
                TimeSpan t = DateTime.Now - DateTime.Parse(dt.Rows[0][2].ToString());
                ViewBag.SafeDay = t.Days + 1;//安全天数
                ViewBag.Account = dt.Rows[1][1].ToString();//模拟登录账号
                ViewBag.weather = dt.Rows[2][1].ToString();//天气位置
                if (dt.Rows.Count > 3)
                {
                    ViewBag.SDmanager = dt.Rows[3][1].ToString();//三维图数据包路径
                }
                dt.Dispose();
                //UserInfoEntity userinfo = new UserBLL().GetUserInfoByAccount(ViewBag.Account);
                //if (userinfo != null)
                //{//三维请求地址拼接
                //    sql = "select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='AppSettings' and t.itemname='WebApiUrl'  order by t.sortcode asc";
                //    dt = new OperticketmanagerBLL().GetDataTable(sql);
                //    if (dt.Rows.Count > 0)
                //    {
                //        string[] str = dt.Rows[0][1].ToString().Split('/');
                //        ViewBag.StrUrl = str[2];
                //        ViewBag.StrUr2 = userinfo.UserId;
                //        ViewBag.StrUr3 = str[3];
                //    }
                //}
            }
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user == null)
            {//单点登录
                var args = BSFramework.Util.DESEncrypt.Encrypt("" + ViewBag.Account + "^../Login/KmIndex?mode=1^" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "^KMMIS");
                SignIn(args);
            }
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            ViewBag.KMHikImgIp = pdata.GetItemValue("KMHikImgIp");//海康图片访问ip地址
            return View();
        }

        /// <summary>
        /// 华电可门展厅四屏展示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult KmztIndex()
        {
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {//可门配置信息
                TimeSpan t = DateTime.Now - DateTime.Parse(dt.Rows[0][2].ToString());
                ViewBag.SafeDay = t.Days + 1;//安全天数
                ViewBag.Account = dt.Rows[1][1].ToString();//模拟登录账号
                ViewBag.weather = dt.Rows[2][1].ToString();//天气位置
                dt.Dispose();
            }
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user == null)
            {//单点登录
                var args = BSFramework.Util.DESEncrypt.Encrypt("" + ViewBag.Account + "^../Login/KmIndex?mode=1^" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "^KMMIS");
                SignIn(args);
            }
            return View();
        }
        [HttpGet]
        public ActionResult KbsIndex(string code = "0017001001001001")
        {
            DepartmentBLL deptBll = new DepartmentBLL();
            DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='较大风险'", code));
            ViewBag.Jdcount = dtCount.Rows[0][0].ToString();
            dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and deptcode like '{0}%' and grade='重大风险'", code));
            ViewBag.Zdcount = dtCount.Rows[0][0].ToString();
            return View();
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        /// <summary>
        /// 安全退出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult OutLogin()
        {
            Operator user = OperatorProvider.Provider.Current();
            if (user != null)
            {
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 1;
                logEntity.OperateTypeId = ((int)OperationType.Exit).ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exit);
                logEntity.OperateAccount = user.Account + "(" + user.UserName + ")";
                logEntity.OperateUserId = user.UserId;
                logEntity.ExecuteResult = 1;
                logEntity.ExecuteResultJson = "退出系统";
                logEntity.Module = Config.GetValue("SoftName");
                logEntity.WriteLog();
            }
            Session.Abandon();                                          //清除当前会话
            Session.Clear();
            //清除当前浏览器所有Session
            OperatorProvider.Provider.EmptyCurrent();                //清除登录者信息
            //WebHelper.RemoveCookie("autologin");                  //清除自动登录
            return Content(new AjaxResult { type = ResultType.success, message = "退出系统" }.ToJson());
        }
        /// <summary>
        /// 切换用户身份，以指定电厂编码和角色进入系统（用于省公司首页地图展示使用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeUser(string deptCode, string roleCode, string args = "")
        {
            ERCHTMS.Entity.BaseManage.UserInfoEntity user = null;
            if (string.IsNullOrEmpty(args))
            {
                user = new UserBLL().GetUserListByCodeAndRole(deptCode, roleCode).FirstOrDefault();
                user.RealName = user.DeptName;
            }
            else
            {
                user = new UserBLL().GetUserInfoByAccount(BSFramework.Util.DESEncrypt.Decrypt(args));
            }
            if (user != null)
            {
                Operator currUser = OperatorProvider.Provider.Current();
                bool result = SetUserInfo(user);
                if (result)
                {
                    return Success("操作成功", BSFramework.Util.DESEncrypt.Encrypt(currUser.Account));
                }
                else
                {
                    return Error("操作失败");
                }
            }
            return Error("操作失败");
        }
        [HttpGet]
        public ActionResult ImitateLogin(string args)
        {
            try
            {
                args = BSFramework.Util.DESEncrypt.Decrypt(args);
                string[] arr = args.Split('|');
                string userId = arr[0];
                string account = arr[1];
                string LoginUserKey = Config.GetValue("SoftName");
                userId = "UID_" + userId + "_" + LoginUserKey;
                Operator curUser = CacheFactory.Cache().GetCache<Operator>(userId);

                if (curUser == null)
                {
                    UserBLL userBLL = new UserBLL();
                    UserInfoEntity user = userBLL.GetUserInfoByAccount(account);
                    bool result = SetUserInfo(user);
                    if (result)
                    {
                        return Success("操作成功");
                    }
                    else
                    {
                        return Error("用户不存在！");
                    }
                }
                else
                {
                    FormsAuth.SignIn(curUser.UserId, curUser, 2 * 60);
                    return Success("操作成功");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 单点登录入口
        /// </summary>
        /// <param name="args">验证解密的参数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignIn(string args)
        {
            try
            {
                //string url = "../Home/AdminWindos";
                //args = BSFramework.Util.DESEncrypt.Encrypt("hrhbgly^"+url+"^" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "^KJMIS");
                string encryptStr = "";
                string val = dataitemdetailbll.GetItemValue("IsGdxy");
                string isGdhc = dataitemdetailbll.GetItemValue("GdhcUrl");
                //args = "062A15F7F2F123E4A600FB5A8AE1621E0F6FAFFDCFBC9F14C4956B81A1924336794B288096B7FE09";
                //isGdhc = "asdfasdf";
                //string key = "123^^2020-12-21 15:21:50^GDHCMIS";
                if (!string.IsNullOrEmpty(val) || !string.IsNullOrWhiteSpace(isGdhc))
                {
                    //string test = BSFramework.Util.DESEncrypt.EncryptString(key);
                    encryptStr = BSFramework.Util.DESEncrypt.DecryptString(args);
                    
                }
                else
                {
                    encryptStr = BSFramework.Util.DESEncrypt.Decrypt(args);
                }
                string[] arr = encryptStr.Split('^');
                string account = arr[0]; //账号
                string moduleNo = arr[1];//模块编号或者地址
                string time = arr[2]; //时间戳
                string appKey = arr[3];//应用app名称，用于授权
                if (string.IsNullOrWhiteSpace(account))
                {
                    string msg = "用户账号不能为空！";
                    return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
                }
                //TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - DateTime.Parse(time).Ticks);
                //if (ts.TotalMinutes > 10)
                //{
                //    string msg = "验证数据失败：链接已经失效！";
                //    return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
                //}

                UserBLL userBLL = new UserBLL();
                UserInfoEntity user = userBLL.GetUserInfoByAccount(account);
                bool result = SetUserInfo(user);
                if (result)
                {
                    if (user.EnabledMark == 0)
                    {
                        string msg = "账号已被禁用,请联系系统管理员！";
                        return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
                    }
                    ERCHTMS.Code.OperatorProvider.AppUserId = user.UserId;
                    if (!string.IsNullOrWhiteSpace(moduleNo))
                    {
                        if (moduleNo.Contains("/"))
                        {
                            //if (moduleNo.Contains("?"))
                            //{
                            //    moduleNo += "&json=" + Newtonsoft.Json.JsonConvert.SerializeObject(new { userid=user.UserId});
                            //}
                            //else
                            //{
                            //    moduleNo += "?json=" + Newtonsoft.Json.JsonConvert.SerializeObject(new { userid = user.UserId });
                            //}
                            return Redirect(moduleNo);
                        }
                        else
                        {
                            ModuleBLL moduleBll = new ModuleBLL();
                            ModuleEntity module = moduleBll.GetEntityByCode(moduleNo);
                            if (module == null)
                            {
                                string msg = "验证数据失败：错误的地址！";
                                return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
                            }
                            else
                            {
                                return Redirect(Request.ApplicationPath + module.UrlAddress);
                            }
                        }
                    }
                    else
                    {
                        return Redirect("../Home/AdminWindos");
                    }
                   

                }
                else
                {
                    string msg = "身份验证失败！";
                    return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
                }

            }
            catch (Exception ex)
            {
                string msg = "验证数据失败：" + ex.Message;
                return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
            }

        }

        /// <summary>
        /// 国电汉川CAS平台跳转
        /// </summary>
        /// <param name="ticket">登录票据</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CasSigiIn(string ticket)
        {
            try
            {
                string CASHOST = new DataItemDetailBLL().GetItemValue("CasUrl");
                if (string.IsNullOrWhiteSpace(CASHOST))
                {
                    return RedirectToAction("ErrorMsg", "Error", new { Message = "获取Cas服务平台地址错误，请联系管理员或客服，谢谢！" });
                }
                System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
                string service = Request.Url.GetLeftPart(UriPartial.Path);
                if (ticket == null || ticket.Length == 0) //票据为空 跳转CAS服务平台登录页面重新登录
                {
                    string redir = CASHOST + "login?service=" + service;
                    return Redirect(redir);
                }
                string validateurl = CASHOST + "serviceValidate?ticket=" + ticket + "&service=" + service;
                StreamReader Reader = new StreamReader(new WebClient().OpenRead(validateurl));
                string resp = Reader.ReadToEnd();
                NameTable nt = new NameTable();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
                XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
                XmlTextReader reader = new XmlTextReader(resp, XmlNodeType.Element, context);
                string netid = null; //账号
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        string tag = reader.LocalName;
                        if (tag == "user")
                        {
                            netid = reader.ReadString();
                        }
                    }
                }

                reader.Close();
                if (netid == null)
                {
                    //取得用户账号为空，按集成系统业务逻辑处理
                    return RedirectToAction("ErrorMsg", "Error", new { Message = "电力双控系统中不存在账号为空的账号，请联系管理员或客服，谢谢！" });
                }
                else
                {
                    UserBLL userBLL = new UserBLL();
                    UserInfoEntity userInfo = userBLL.GetUserInfoByAccount(netid);
                    if (userInfo == null)
                    {
                        return RedirectToAction("ErrorMsg", "Error", new { Message = "账号再电力双控系统中不存在，请联系管理员或客服，谢谢！" });
                    }

                    bool result = SetUserInfo(userInfo);
                    if (result)
                    {
                        var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        if (userInfo.AllowStartTime != null && userInfo.AllowEndTime != null)
                        {
                            if (DateTime.Now > userInfo.AllowEndTime)
                            {
                                return RedirectToAction("ErrorMsg", "Error", new { Message = "您的账号使用期限已过期，请联系管理员或客服，谢谢！" });
                            }
                            else
                            {
                                return RedirectToAction("AdminWindos", "Home");
                            }
                        }
                        else
                        {
                            LogEntity logEntity = new LogEntity();
                            logEntity.CategoryId = 1;
                            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                            logEntity.OperateAccount = netid;
                            logEntity.OperateUserId = netid;
                            logEntity.Module = Config.GetValue("SoftName");
                            logEntity.OperateAccount = netid;
                            Task.Run(() =>
                            {
                                //写入日志

                                logEntity.CategoryId = 1;
                                logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                                logEntity.OperateAccount = userInfo.RealName;
                                logEntity.OperateUserId = userInfo.RealName;
                                logEntity.Module = Config.GetValue("SoftName");
                                logEntity.OperateAccount = userInfo.Account + "(" + userInfo.RealName + ")";
                                logEntity.ExecuteResult = 1;
                                logEntity.ExecuteResultJson = "登录成功";
                                logEntity.WriteLog();
                            });
                            Task.Run(()=> {
                                SaveDailyRecord(user);
                            });
                           
                            return RedirectToAction("AdminWindos", "Home");
                        }
                    }
                    else
                    {
                        return RedirectToAction("ErrorMsg", "Error", new { Message = "身份验证失败，请联系管理员或客服，谢谢！" });
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "验证数据失败：" + ex.Message;
                return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
            }
        }

        /// <summary>
        /// 国电汉川转岗待办跳转
        /// </summary>
        /// <param name="account"></param>
        /// <param name="itemcode"></param>
        /// <param name="flowid"></param>
        /// <param name="datetime"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TransferSignIn(string account, string itemcode, string flowid, string datetime, string appid)
        {
            try
            {
                UserBLL userBLL = new UserBLL();
                if (string.IsNullOrWhiteSpace(account))
                {
                    return RedirectToAction("ErrorMsg", "Error", new { Message = "跳转地址错误，请联系管理员或客服，谢谢！" });
                }
                else
                {
                    account = BSFramework.Util.DESEncrypt.Decrypt(account);
                }
                UserInfoEntity userInfo = userBLL.GetUserInfoByAccount(account);
                if (userInfo == null)
                {
                    return RedirectToAction("ErrorMsg", "Error", new { Message = "账号再电力双控系统中不存在，请联系管理员或客服，谢谢！" });
                }
                if (!string.IsNullOrWhiteSpace(datetime))
                {
                    try
                    {
                        if (!(Convert.ToDateTime(datetime) >= DateTime.Now && Convert.ToDateTime(datetime) <= DateTime.Now.AddMinutes(5)))
                        {
                            return RedirectToAction("ErrorMsg", "Error", new { Message = "链接失效，请重新登录！" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ErrorMsg", "Error", new { Message = "链接失效，请重新登录！" });
                    }
                }
                bool result = SetUserInfo(userInfo);
                if (result)
                {
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    if (userInfo.AllowStartTime != null && userInfo.AllowEndTime != null)
                    {
                        if (DateTime.Now > userInfo.AllowEndTime)
                        {
                            return RedirectToAction("ErrorMsg", "Error", new { Message = "您的账号使用期限已过期，请联系管理员或客服，谢谢！" });
                        }
                        else
                        {
                            return Redirect("../Home/AdminWindos?itemCode=" + itemcode);
                        }
                    }
                    else
                    {
                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 1;
                        logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                        logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                        logEntity.OperateAccount = account;
                        logEntity.OperateUserId = account;
                        logEntity.Module = Config.GetValue("SoftName");
                        logEntity.OperateAccount = account;
                        Task.Run(() =>
                        {
                            //写入日志

                            logEntity.CategoryId = 1;
                            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                            logEntity.OperateAccount = userInfo.RealName;
                            logEntity.OperateUserId = userInfo.RealName;
                            logEntity.Module = Config.GetValue("SoftName");
                            logEntity.OperateAccount = userInfo.Account + "(" + userInfo.RealName + ")";
                            logEntity.ExecuteResult = 1;
                            logEntity.ExecuteResultJson = "登录成功";
                            logEntity.WriteLog();
                        });
                        Task.Run(() =>
                        {
                            SaveDailyRecord(user);
                        });
                        return Redirect("../Home/AdminWindos?itemCode=" + itemcode);
                    }
                }
                else
                {
                    return RedirectToAction("ErrorMsg", "Error", new { Message = "身份验证失败，请联系管理员或客服，谢谢！" });
                }
            }
            catch (Exception ex)
            {
                string msg = "验证数据失败：" + ex.Message;
                return RedirectToAction("ErrorMsg", "Error", new { Message = msg });
            }
        }
        

        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
            }
            catch (Exception ex)
            {

            }


        }
        public bool SetUserInfo(UserInfoEntity userEntity)
        {
            if (userEntity != null)
            {
                UserBLL userBLL = new UserBLL();
                Operator operators = new Operator();
                operators.UserId = userEntity.UserId;
                operators.Code = userEntity.EnCode;
                operators.Account = userEntity.Account;
                operators.UserName = userEntity.RealName;
                operators.Password = userEntity.Password;
                operators.Secretkey = userEntity.Secretkey;

                DepartmentEntity dept = userBLL.GetUserOrganizeInfo(userEntity);//获取当前用户所属的机构
                operators.OrganizeId = dept.DepartmentId; //所属机构ID
                operators.OrganizeCode = dept.EnCode;//所属机构编码
                operators.NewDeptCode = dept.DeptCode;//所属机构新的编码（对应部门表中新加的编码字段deptcode）
                operators.OrganizeName = dept.FullName;//所属机构名称
                operators.Industry = dept.Industry;//所属行业

                operators.DeptId = userEntity.DepartmentId;
                operators.DeptCode = userEntity.DepartmentCode;
                operators.DeptName = userEntity.DeptName;
                //operators.OrganizeName = userEntity.OrganizeName;
                operators.RoleId = userEntity.RoleId;
                operators.PostId = userEntity.PostId;
                operators.ProjectID = userEntity.ProjectId;
                operators.isEpiboly = userEntity.isEpiboly == "是" ? true : false;
                operators.PostName = userEntity.DutyName;
                operators.RoleName = userEntity.RoleName;
                operators.DutyName = userEntity.PostName;
                operators.DutyId = userEntity.DutyId; //岗位id
                operators.IPAddress = Net.Ip;
                operators.IsTrainAdmin = userEntity.IsTrainAdmin == null ? 0 : userEntity.IsTrainAdmin;
                operators.SpecialtyType = userEntity.SpecialtyType;
                operators.AccountType = userEntity.AccountType;
                operators.IsTrain = userEntity.IsTrain == null ? 0 : userEntity.IsTrain;

                if (!string.IsNullOrEmpty(userEntity.HeadIcon))
                {
                    if (userEntity.HeadIcon.ToLower().StartsWith("http://"))
                    {
                        operators.Photo = userEntity.HeadIcon;
                    }
                    else
                    {
                        if (System.IO.File.Exists(Server.MapPath("~" + userEntity.HeadIcon)))
                        {
                            operators.Photo = userEntity.HeadIcon;
                        }
                        else
                        {
                            operators.Photo = "/Content/images/on-line.png";
                        }
                    }
                }
                operators.ParentId = userEntity.ParentId;
                operators.SignImg = userEntity.SignImg;
                //operators.SendDeptID = userEntity.SendDeptID;
                //operators.IPAddressName = IPLocation.GetLocation(Net.Ip);
                operators.IdentifyID = userEntity.IdentifyID; //身份证号码
                operators.ObjectId = new PermissionBLL().GetObjectStr(userEntity.UserId);
                operators.LogTime = DateTime.Now;
                operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
                //判断是否系统管理员
                if (userEntity.Account == "System")
                {
                    operators.IsSystem = true;
                }
                else
                {
                    operators.IsSystem = false;
                }

                //string userMode = "";

                //string roleCode = dataitemdetailbll.GetItemValue("HidApprovalSetting");

                //string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

                //string[] pstr = HidApproval.Split('#');  //分隔机构组

                //foreach (string strArgs in pstr)
                //{
                //    string[] str = strArgs.Split('|');

                //    //当前机构相同，且为本部门安全管理员验证  第一种
                //    if (str[0].ToString() == userEntity.OrganizeId && str[1].ToString() == "0")
                //    {
                //        int count = userBLL.GetUserListByRole(userEntity.DepartmentCode, roleCode, userEntity.OrganizeId).ToList().Where(p => p.UserId == userEntity.UserId).Count();
                //        if (count > 0)
                //        {
                //            userMode = "0";
                //        }
                //        else
                //        {
                //            userMode = "1";
                //        }

                //        break;
                //    }
                //    if (str[0].ToString() == userEntity.OrganizeId && str[1].ToString() == "1")
                //    {
                //        //获取指定部门的所有人员
                //        int count = userBLL.GetUserListByDeptCode(str[2].ToString(), null, false, userEntity.OrganizeId).ToList().Where(p => p.UserId == userEntity.UserId).Count();
                //        if (count > 0)
                //        {
                //            userMode = "2";
                //        }
                //        else
                //        {
                //            userMode = "3";
                //        }
                //        break;
                //    }
                //}
                //operators.wfMode = userMode;

                //string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

                //string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

                //string CompanyRole = hidPlantLevel + "," + hidOrganize;

                //var userList = userBLL.GetUserListByDeptCode(userEntity.DepartmentCode, CompanyRole, false, userEntity.OrganizeId).Where(p => p.UserId == userEntity.UserId).ToList();

                //string isPlanLevel = "";
                ////当前用户是公司级及厂级用户
                //if (userList.Count() > 0)
                //{
                //    isPlanLevel = "1"; //厂级用户
                //}
                //else
                //{
                //    isPlanLevel = "0";  //非公司及厂级
                //}
                //operators.isPlanLevel = isPlanLevel;
                //var deptEntity = new DepartmentBLL().GetEntity(userEntity.DepartmentId);
                //if (null != deptEntity)
                //{
                //    operators.SendDeptID = deptEntity.SendDeptID;
                //}
                //else
                //{
                //    operators.SendDeptID = "";
                //}

                OperatorProvider.Provider.AddCurrent(operators);
                //登录限制
                //LoginLimit(username, operators.IPAddress, operators.IPAddressName);

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="verifycode">验证码</param>
        /// <param name="autologin">下次自动登录</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckLogin(string username, string password, string verifycode, int autologin, string shapassword = "", string password1 = "")
        {
            //try
            //{
            //    string pwd = BSFramework.Util.DESEncrypt.DecryptString(password1.ToUpper());
            //    password1 = pwd;
            //}
            //catch
            //{

            //}
            //if (password1.Length>16)
            //{
            //    return Error("密码长度不能大于16位！");
            //}
            ////如果是广西华昇版本则进行其他处理
            //string isGxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
            //if(!string.IsNullOrWhiteSpace(isGxhs))
            //{
            //    return CheckUser(username, password, verifycode, autologin, shapassword, password1);
            //}
          
            try
            {
                #region 验证码验证
                string val = Config.GetValue("IsHaveCode");
                if (val == "true")
                {
                    if (autologin == 0)
                    {
                        verifycode = Md5Helper.MD5(verifycode.ToLower(), 16);
                        if (Session["session_verifycode"].IsEmpty() || verifycode != Session["session_verifycode"].ToString())
                        {
                            throw new Exception("验证码错误，请重新输入");
                        }
                    }
                }

                #endregion

                #region 内部账户验证
                UserBLL userBLL = new UserBLL();
                UserInfoEntity userInfo = userBLL.GetUserInfoByAccount(username);
                if (userInfo == null)
                {
                    return Error("账号或密码错误");
                }
                UserInfoEntity userEntity = userBLL.CheckLogin(username, password, shapassword);

                bool result = SetUserInfo(userEntity);
                if (result)
                {
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    int isOk = 1;
                    ERCHTMS.Busines.SystemManage.PasswordSetBLL psBll = new PasswordSetBLL();
                    List<string> lst = psBll.IsPasswordRuleStatus(user);
                    if (lst[0] == "true")
                    {
                        if (userInfo.LastVisit == null)
                        {
                            isOk = 0;
                            return Success("登录成功", isOk);
                        }
                        else
                        {
                            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(lst[4]);
                            if (!reg1.IsMatch(password1))
                            {
                                isOk = 0;
                                return Success("登录成功", isOk);
                            }
                        }

                    }
                    if (userEntity.AllowStartTime != null && userEntity.AllowEndTime != null)
                    {
                        if (DateTime.Now > userEntity.AllowEndTime)
                        {
                            return Error("您的账号使用期限已过期，请联系管理员或客服，谢谢！");
                        }
                        else
                        {
                            return Success("登录成功", isOk);
                        }
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            //写入日志
                            LogEntity logEntity = new LogEntity();
                            logEntity.CategoryId = 1;
                            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                            logEntity.OperateAccount = username;
                            logEntity.OperateUserId = username;
                            logEntity.Module = Config.GetValue("SoftName");
                            logEntity.OperateAccount = username;
                            logEntity.CategoryId = 1;
                            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                            logEntity.OperateAccount = userEntity.RealName;
                            logEntity.OperateUserId = userEntity.RealName;
                            logEntity.Module = Config.GetValue("SoftName");
                            logEntity.OperateAccount = userEntity.Account + "(" + userEntity.RealName + ")";
                            logEntity.ExecuteResult = 1;
                            logEntity.ExecuteResultJson = "登录成功";
                            logEntity.WriteLog();
                        });
                        Task.Run(() =>
                        {
                            SaveDailyRecord(user);
                        });
                        return Success("登录成功", isOk);
                    }
                }
                else
                {
                    return Error("账号或密码错误");
                }
                #endregion
            }
            catch (Exception ex)
            {
                WebHelper.RemoveCookie("autologin");  //清除自动登录
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 验证域账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidateUser(string account, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
                {
                    return false;
                }
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                nc.Add("account", account); nc.Add("password", password);
                string url = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("WebApiUrl", "AppSettings") + "Directory/checkUser";
                byte[] bytes = wc.UploadValues(url, nc);
                string result = Encoding.UTF8.GetString(bytes);
                dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(result);
                if (dy.code == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 用户登录验证,先验证本地账户再验证域账号(广西华昇)
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="verifycode">验证码</param>
        /// <param name="autologin">下次自动登录</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckUser(string username, string password, string verifycode, int autologin, string shapassword = "", string password1 = "")
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("账号和密码不能为空！");
            }

            LogEntity logEntity = new LogEntity();
            logEntity.CategoryId = 1;
            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
            logEntity.OperateAccount = username;
            logEntity.OperateUserId = username;
            logEntity.Module = Config.GetValue("SoftName");
            logEntity.OperateAccount = username;
            UserBLL userBLL = new UserBLL();
            try
            {
                #region 验证码验证
                string val = Config.GetValue("IsHaveCode");
                if (val == "true")
                {
                    if (autologin == 0)
                    {
                        verifycode = Md5Helper.MD5(verifycode.ToLower(), 16);
                        if (Session["session_verifycode"].IsEmpty() || verifycode != Session["session_verifycode"].ToString())
                        {
                            throw new Exception("验证码错误，请重新输入");
                        }
                    }
                }
                #endregion
                #region 内部账户验证

                UserInfoEntity userInfo = userBLL.GetUserInfoByAccount(username);
                if (userInfo == null)
                {
                    return Error("账号或密码错误");
                }
                else
                {
                    if (userInfo.isEpiboly == "否" && username.ToLower() != "system")
                    {
                        bool res = ValidateUser(username, password1);
                        if (!res)
                        {
                            return Error("账号或密码错误");
                        }
                        else
                        {
                            SetUserInfo(userInfo);
                            return Success("登录成功", 1);
                        }
                    }
                }
                UserInfoEntity userEntity = userBLL.CheckLogin(username, password, shapassword);
                bool result = SetUserInfo(userEntity);
                if (result)
                {
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    int isOk = 1;
                    ERCHTMS.Busines.SystemManage.PasswordSetBLL psBll = new PasswordSetBLL();
                    List<string> lst = psBll.IsPasswordRuleStatus(user);
                    if (lst[0] == "true")
                    {
                        if (userInfo.LastVisit == null)
                        {
                            isOk = 0;
                            return Success("登录成功", isOk);
                        }
                        else
                        {
                            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(lst[4]);
                            if (!reg1.IsMatch(password1))
                            {
                                isOk = 0;
                                return Success("登录成功", isOk);
                            }
                        }

                    }
                    if (userEntity.AllowStartTime != null && userEntity.AllowEndTime != null)
                    {
                        if (DateTime.Now > userEntity.AllowEndTime)
                        {
                            return Error("您的账号使用期限已过期，请联系管理员或客服，谢谢！");
                        }
                        else
                        {
                            return Success("登录成功", isOk);
                        }
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            //写入日志

                            logEntity.CategoryId = 1;
                            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                            logEntity.OperateAccount = userEntity.RealName;
                            logEntity.OperateUserId = userEntity.RealName;
                            logEntity.Module = Config.GetValue("SoftName");
                            logEntity.OperateAccount = userEntity.Account + "(" + userEntity.RealName + ")";
                            logEntity.ExecuteResult = 1;
                            logEntity.ExecuteResultJson = "登录成功";
                            logEntity.WriteLog();
                        });
                        Task.Run(() =>
                        {
                            SaveDailyRecord(user);
                        });
                        return Success("登录成功", isOk);
                    }
                }
                else
                {

                    return Error("账号或密码错误");

                }
                #endregion
            }
            catch (Exception ex)
            {
                WebHelper.RemoveCookie("autologin");                  //清除自动登录
                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = ex.Message;
                logEntity.WriteLog();
                return Error(ex.Message);
            }
        }
        #endregion

        #region 注册账户、登录限制
        private AccountBLL accountBLL = new AccountBLL();
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobileCode">手机号码</param>
        /// <returns>返回6位数验证码</returns>
        [HttpGet]
        public ActionResult GetSecurityCode(string mobileCode)
        {
            if (!ValidateUtil.IsValidMobile(mobileCode))
            {
                throw new Exception("手机格式不正确,请输入正确格式的手机号码。");
            }
            var data = accountBLL.GetSecurityCode(mobileCode);
            if (!string.IsNullOrEmpty(data))
            {
                SmsModel smsModel = new SmsModel();
                smsModel.account = Config.GetValue("SMSAccount");
                smsModel.pswd = Config.GetValue("SMSPswd");
                smsModel.url = Config.GetValue("SMSUrl");
                smsModel.mobile = mobileCode;
                smsModel.msg = "验证码 " + data + "，(请确保是本人操作且为本人手机，否则请忽略此短信)";
                SmsHelper.SendSmsByJM(smsModel);
            }
            return Success("获取成功。");
        }
        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="mobileCode">手机号</param>
        /// <param name="securityCode">短信验证码</param>
        /// <param name="fullName">姓名</param>
        /// <param name="password">密码（md5）</param>
        /// <param name="verifycode">图片验证码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(string mobileCode, string securityCode, string fullName, string password, string verifycode)
        {
            AccountEntity accountEntity = new AccountEntity();
            accountEntity.MobileCode = mobileCode;
            accountEntity.SecurityCode = securityCode;
            accountEntity.FullName = fullName;
            accountEntity.Password = password;
            accountEntity.IPAddress = Net.Ip;
            accountEntity.IPAddressName = IPLocation.GetLocation(accountEntity.IPAddress);
            accountEntity.AmountCount = 30;
            accountBLL.Register(accountEntity);
            return Success("注册成功。");
        }
        /// <summary>
        /// 登录限制
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="iPAddress">IP</param>
        /// <param name="iPAddressName">IP所在城市</param>
        public void LoginLimit(string account, string iPAddress, string iPAddressName)
        {
            //if (account == "System")
            //{
            //    return;
            //}
            string platform = Net.Browser;
            accountBLL.LoginLimit(platform, account, iPAddress, iPAddressName);
        }
        #endregion


        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        private ActionResult Success(string message)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message }.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        private ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message, resultdata = data }.ToJson());
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public ActionResult Error(string message)
        {
            return Content(new AjaxResult { type = ResultType.error, message = message }.ToJson());
        }
        /// <summary>
        /// 扫描二维码登陆
        /// </summary>
        /// <param name="QRCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckQRCodeLogin(string QRCode)
        {
            var userid = CacheFactory.Cache().GetCache<string>(QRCode);
            if (string.IsNullOrEmpty(userid))
            {
                return Error("票据失效,请重新扫码");
            }
            else
            {
                var userentity = new UserBLL().GetEntity(userid);
                var userinfo = new UserBLL().GetUserInfoByAccount(userentity.Account);
                string username = userentity.Account;
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 1;
                logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
                logEntity.OperateAccount = username;
                logEntity.OperateUserId = username;
                logEntity.Module = Config.GetValue("SoftName");
                logEntity.OperateAccount = username;
                try
                {
                    #region 内部账户验证
                    //UserBLL userBLL = new UserBLL();
                    //UserInfoEntity userEntity = userBLL.CheckLogin(username, password);
                    bool result = SetUserInfo(userinfo);
                    if (result)
                    {
                        return Success("登录成功", new { userid = userid, username = userentity.RealName });
                    }
                    else
                    {
                        return Error("账号或密码错误");
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    WebHelper.RemoveCookie("autologin");                  //清除自动登录
                    logEntity.ExecuteResult = -1;
                    logEntity.ExecuteResultJson = ex.Message;
                    logEntity.WriteLog();
                    return Error(ex.Message);
                }
            }

        }
        /// <summary>
        /// 工作日志
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SaveDailyRecord(Operator user)
        {
            if (user != null)
            {
                string res = string.Empty;
                var entity1 = new DataItemBLL().GetEntityByCode("BalanceManage");
                if (entity1 != null)
                {
                    IEnumerable<DataItemDetailEntity> list = new DataItemDetailBLL().GetList(entity1.ItemId);
                    foreach (var item in list)
                    {//地磅员角色
                        if (string.IsNullOrEmpty(item.ItemValue)) continue;
                        res += string.Format("'{0}',", item.ItemValue);
                    }
                    if (user.RoleName.Contains(res))
                    {
                        DailyrRecordEntity entity = new DailyrRecordEntity();
                        entity.WorkType = 3;
                        entity.Theme = "登录";
                        new OperticketmanagerBLL().InsetDailyRecord(entity);
                    }
                }
            }
        }

        /// <summary>
        /// 获取可门电厂首页配置信息
        /// </summary>
        [HttpPost]
        public void GetKmIndexConfigure()
        {
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {//可门配置信息
                TimeSpan t = DateTime.Now - DateTime.Parse(dt.Rows[0][2].ToString());
                ViewBag.SafeDay = t.Days + 1;//安全天数
                ViewBag.Account = dt.Rows[1][1].ToString();//模拟登录账号
                ViewBag.weather = dt.Rows[2][1].ToString();//天气位置
            }
        }


        /// <summary>
        /// 获取今日高风险作业
        /// </summary>
        /// <returns></returns>    
        [HttpGet]
        [HandlerLogin(LoginMode.Ignore)]
        public ActionResult GetDangerWorkToday()
        {
           string level= dataitemdetailbll.GetEnableItemValue("ShowLevel");
            var result = new SafeworkcontrolBLL().GetDangerWorkToday(level);
            return Content(result.ToJson());
        }

        /// <summary>
        /// 获取当前安全生产天数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetScreenSafetyDay()
        {
            //获取当前用户           
            string safeDay = "0";
            string notice = string.Empty;
            try
            {
                var list = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetDataItemListByItemCode("'KmConfigure'").OrderBy(x => x.SortCode);
                var entity = list.FirstOrDefault(x => x.ItemName == "安全生产天数");
                if (entity != null)
                {
                    DateTime t1 = DateTime.Parse(entity.ItemCode);
                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - t1.Ticks);
                    safeDay = (ts.Days + 1).ToString();
                }
                var resultDt = new DesktopBLL().GetScreenTitle();
                if (resultDt.Rows.Count > 0)
                    notice = resultDt.Rows[0]["title"].ToString();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

            return Content(new { safeDay, notice }.ToJson());
        }



        /// <summary>
        /// 获取全厂人数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPersonNums()
        {
            var hikinoutlogbll = new HikinoutlogBLL();
            var dt = hikinoutlogbll.GetNums();
            List<string> eareaNames = new List<string>();
            eareaNames.Add("一号岗");
            eareaNames.Add("三号岗");
            eareaNames.Add("码头岗");
            List<int> userType = new List<int>();
            userType.Add(0);
            userType.Add(1);
            userType.Add(2);
            List<dynamic> PersonData = new List<dynamic>();
            foreach (int type in userType)
            {
                DataRow[] typeRows = dt.Select(string.Format(" type='{0}'", type));
                int station1 = 0, station2 = 0, station3 = 0;
                foreach (DataRow row in typeRows)
                {
                    if (row[2].ToString() == "一号岗")
                        station1 = Convert.ToInt32(row[1]);
                    if (row[2].ToString() == "三号岗")
                        station2 = Convert.ToInt32(row[1]);
                    if (row[2].ToString() == "码头岗")
                        station3 = Convert.ToInt32(row[1]);
                }
                PersonData.Add(new
                {
                    userType = type,
                    stationCount1 = station1,
                    stationCount2 = station2,
                    stationCount3 = station3,
                    total = station1 + station2 + station3
                });
            }
            var CarData = hikinoutlogbll.GetCarStatistic();

            var LastData = hikinoutlogbll.GetLastInoutLog();

            var returnData = new { PersonData, CarData, LastData };

            return Content(returnData.ToJson());
        }



        [HttpGet]
        public JsonResult GetPersonData()
        {
            HikinoutlogBLL hikinoutlogBLL = new HikinoutlogBLL();
            var data = hikinoutlogBLL.GetPersonData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAreaData()
        {
            HikinoutlogBLL hikinoutlogBLL = new HikinoutlogBLL();
            var data = hikinoutlogBLL.GetAreaData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCarData()
        {
            CarinlogBLL carinlogBLL = new CarinlogBLL();
            var data = carinlogBLL.GetCarData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据编码的类别获取编码列表
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public ActionResult GetItemDetailListByCategory(string category)
        {
            var data = new DataItemDetailBLL().GetListItems(category);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #region 京泰电厂
        #region  加载三维地图数据
        /// <summary>
        /// 加载三维地图上展示的  隐患分布   消防设施  风险作业 分布数据
        /// </summary>
        /// <param name="loadType"></param>
        /// <param name="areaCodes"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadData(string loadType, List<string> areaCodes)
        {
            SocketHelper.SetLog("3D_Info", "加载三维地图数据", loadType + "|" + JsonConvert.SerializeObject(areaCodes));
            try
            {
                switch (loadType)
                {
                    case "HiddenDanger"://隐患
                        System.Collections.IList hiddenDangerdata = new HTBaseInfoBLL().GetCountByArea(areaCodes);
                        return Json(hiddenDangerdata, JsonRequestBehavior.AllowGet);
                        break;
                    case "FireDevice"://消防设施
                        System.Collections.IList firedata = new FirefightingBLL().GetCountByArea(areaCodes);
                        return Json(firedata, JsonRequestBehavior.AllowGet);
                        break;
                    case "HighRisk"://高风险作业
                        DataTable hightRiskData = new HighRiskCommonApplyBLL().GetCountByArea(areaCodes);
                        if (hightRiskData != null && hightRiskData.Rows != null && hightRiskData.Rows.Count > 0)
                        {
                            List<object> obj = new List<object>();
                            var tor = hightRiskData.Rows.GetEnumerator();
                            while (tor.MoveNext())
                            {
                                DataRow dr = tor.Current as DataRow;
                                obj.Add(new
                                {
                                    DistrictID = dr["WORKAREACODE"] is DBNull ? "" : dr["WORKAREACODE"].ToString(),
                                    DistrictName = dr["WORKAREANAME"] is DBNull ? "" : dr["WORKAREANAME"].ToString(),
                                    Count = dr["COUNT"] is DBNull ? 0 : Convert.ToInt32(dr["COUNT"])
                                });
                            }
                            return Json(obj, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new List<object>(), JsonRequestBehavior.AllowGet);
                        break;
                    default:
                        break;
                }

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SocketHelper.SetLog("3D_Error", "三维交互报错", loadType + "|" + JsonConvert.SerializeObject(areaCodes) + "\r\n" + JsonConvert.SerializeObject(ex));
                return Json("");
            }
        }
        /// <summary>
        /// 获取当天的人员进出数据
        /// </summary>
        /// <returns></returns>

        public ActionResult GetTodayCarPeopleCount()
        {
            try
            {
                Dictionary<string, int> dataList = new HikinoutlogBLL().GetTodayCarPeopleCount();
                return Json(dataList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new Dictionary<string, object>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取最新的车辆人员进出数据，取前五条
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCarPeopleTopData()
        {
            try
            {
                Dictionary<string, object> dic = new HikinoutlogBLL().GetCarPeopleTopData();
                return Json(dic.ToJson(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 
        /// 获取设备间监控编号用于展示平台播放
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetcameraIndexCode()
        {
            //string[] strArrycameraIndexCode = new string[] { "15007d053cc34b70958c3df5ce5773de", "56434d21e3db444d85983746921d3a32", "5d6c65caa5644f2f8f8ad2a427ece7a4", "3aac382390fd400ca10f952e83cf9286" };

            //Random rd = new Random();
            //int index = rd.Next(0, 4);
            //var data = strArrycameraIndexCode[index];

            var cacheKey = "DeviceWatch";//缓存键的值
            var cacheService = CacheFactory.Cache();
            HikinoutlogEntity cacheValue = cacheService.GetCache<HikinoutlogEntity>(cacheKey);
            if (cacheValue == null)
            {
                cacheValue = hikinoutlogbll.GetFirsetData();
                //写入缓存
                Task.Run(() =>
                {
                    cacheService.WriteCache(cacheValue, cacheKey, DateTime.Now.AddSeconds(6));
                });
            }
            string DeviceHikID = cacheValue.DeviceHikID;
            string cameraIndexCode = hikinoutlogbll.GetCameraIndexCodeByDoorIndexCode(DeviceHikID);
            if (cameraIndexCode.IsNullOrEmpty())
            {
                //设定一个默认展示的监控
                cameraIndexCode = "9bf1dc0d123e4b0b920009846e36289f";
            }
            string UserName = cacheValue.UserName;
            string DeptName = cacheValue.DeptName;
            int? InOut = cacheValue.InOut;
            string CreateDate = cacheValue.CreateDate?.ToString("yyyy-MM-dd HH:mm:ss");
            string AreaName = cacheValue.AreaName;
            object data = new { cameraIndexCode, UserName, DeptName, InOut, CreateDate, AreaName };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
    public class MyPolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            return true;
        }
    }
}
