using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SafeNoteManage;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SafeNote;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Web.Controllers
{
    /// <summary>
    /// 描 述：系统首页
    /// </summary>
    [HandlerLogin(LoginMode.Enforce)]
    public class HomeController : Controller
    {
        private DataItemDetailBLL detailBLL = new DataItemDetailBLL();
        UserBLL user = new UserBLL();
        DepartmentBLL department = new DepartmentBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private DesktopBLL desktopbll = new DesktopBLL();
        private SafeNoteBLL safenotebll = new SafeNoteBLL();

        #region 视图功能
        /// <summary>
        /// 后台框架页
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminDefault()
        {
            if (@ERCHTMS.Code.OperatorProvider.Provider.Current() == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        #region  新加的两个方法
        public ActionResult KmJsc()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            string url = new DataItemDetailBLL().GetItemValue(user.OrganizeCode, "HomeMap");
            ViewBag.MapUrl = url;
            return View();
        }


        public ActionResult Map()
        {
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new DepartmentBLL().GetDataTable(sql);
            if (dt.Rows.Count > 1)
            {//可门配置信息
                TimeSpan t = DateTime.Now - DateTime.Parse(dt.Rows[0][2].ToString());
                ViewBag.SafeDay = t.Days + 1;//安全天数
                ViewBag.Account = dt.Rows[1][1].ToString();//模拟登录账号
                ViewBag.weather = dt.Rows[2][1].ToString();//天气位置
                ViewBag.SDmanager = dt.Rows[3][1].ToString();//三维图数据包路径
                dt.Dispose();
            }
            return View();
        }
        /// <summary>
        /// 门卫首页过程监控
        /// </summary>
        /// <returns></returns>
        public ActionResult MgMap()
        {
            var item = detailBLL.GetItemValue("KmConfigure", "Manager");
            if (string.IsNullOrEmpty(item))
            {//可门配置信息
                ViewBag.SDmanager = item;//三维图数据包路径  
            }
            return View();
        }
        /// <summary>
        /// 三维地图的展示
        /// </summary>
        /// <returns></returns>
        public ActionResult ThreeDMap()
        {
            return View();
        }
        /// <summary>
        /// 康巴什首页
        /// </summary>
        /// <returns></returns>
        public ActionResult KBSIndex()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            string url = new DataItemDetailBLL().GetItemValue(user.OrganizeCode, "HomeMap");
            ViewBag.MapUrl = url;
            return View();
        }
        #endregion
        /// <summary>
        /// 首页省公司查看下属电厂隐患整改率
        /// </summary>
        /// <returns></returns>
        public ActionResult FactoryHtInfo()
        {
            return View();
        }
        public ActionResult EHSIndex()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string url = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TrainWebUrl");

            ViewData["Url"] = url + "?tokenId=" + BSFramework.Util.DESEncrypt.EncryptString(user.Account);
            return View();
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            string url = new DataItemDetailBLL().GetItemValue(user.OrganizeCode, "HomeMap");
            ViewBag.MapUrl = url;
            return View();
        }

        /// <summary>
        /// 可门电厂首页
        /// </summary>
        /// <returns></returns>
        public ActionResult KmIndex()
        {
            return View();
        }


        public ActionResult NewIndex()
        {
            return View();
        }
        public ActionResult XYIndex()
        {
            string url = new DataItemDetailBLL().GetItemValue(@ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode, "HomeMap");
            ViewBag.MapUrl = url;
            return View();
        }
        public ActionResult XJIndex()
        {
            return View();
        }
        public ActionResult BJIndex()
        {
            return View();
        }
        public ActionResult AdminLTE()
        {
            if (@ERCHTMS.Code.OperatorProvider.Provider.Current() == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult AdminWindos()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string bzUrl = detailBLL.GetItemValue("bzWebUrl");
            string bzIndex = detailBLL.GetItemValue("bzIndex");

            //var doshboard = detailBLL.GetItemValue("班组首页");
            //string args = BSFramework.Util.DESEncrypt.Encrypt(string.Concat(user.Account, "^" + doshboard + "^", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "^DLBZ"));

            //ViewBag.doshboard = bzUrl + "login/signin?args=" + args;
            ViewBag.bzUrl = bzUrl;
            ViewBag.bzIndex = bzIndex;
            if (user == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            //是否配置违章管理模块
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='PECCANCY_ADD')", user.OrganizeId));
            ViewBag.IsWz = dt.Rows[0][0].ToString();
            //是否配置问题管理模块
            dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='QUESTION_ADD')", user.OrganizeId));
            ViewBag.IsWt = dt.Rows[0][0].ToString();
            return View();
        }
        public ActionResult AdminPretty()
        {
            if (@ERCHTMS.Code.OperatorProvider.Provider.Current() == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        /// <summary>
        /// 后台框架页
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminBeyond()
        {
            if (@ERCHTMS.Code.OperatorProvider.Provider.Current() == null)
            {
                WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public string IsKmMark()
        {
            string KMIndex = new DataItemDetailBLL().GetItemValue("KMIndexUrl");
            return KMIndex;
        }


        /// <summary>
        /// 获取当前电厂
        /// </summary>
        /// <returns></returns>
        public string GetCurrentFactory()
        {
            string result = string.Empty;
            string values = new DataItemDetailBLL().GetItemValue("JLIndex");
            if (!string.IsNullOrEmpty(values))
            {
                return "6";
            }
            values = new DataItemDetailBLL().GetItemValue("KMIndexUrl");
            if (!string.IsNullOrEmpty(values))
            {
                return "5";
            }
            values = new DataItemDetailBLL().GetItemValue("XSSIndexUrl"); //西塞山
            if (!string.IsNullOrEmpty(values))
            {
                return "8";
            }
            return "";
        }

        /// <summary>
        /// 我的桌面
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            //return RedirectToAction("XYIndex");

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string bzIndex = itemBll.GetItemValue("bzIndex");
            //班组领导首页(特定页，如果不配置，则默认Index)
            string bzLeader = itemBll.GetItemValue("bzLeader");
            string hdxyUrl = itemBll.GetItemValue("hdxyUrl");
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            string JLIndex = itemBll.GetItemValue("JLIndex");
            string isKbs = itemBll.GetItemValue("IsKbs");
            if (!string.IsNullOrEmpty(JLIndex))
            {
                return RedirectToAction("JLIndex");
            }
            if (!string.IsNullOrEmpty(hdxyUrl))
            {
                if (user.RoleName.Contains("公司领导") || user.RoleName.Contains("厂级部门用户"))
                {
                    return RedirectToAction(hdxyUrl);
                }
                else
                {
                    return RedirectToAction("NewDesktop");//NewDesktop
                }

            }
            //康巴什首页
            if (!string.IsNullOrWhiteSpace(isKbs))
            {
                if (user.RoleName.Contains("公司领导") || user.RoleName.Contains("厂级部门用户"))
                {
                    return RedirectToAction("../Home/KBSIndex");
                }
            }
            //可门电厂首页
            //可门电厂首页
            if (!string.IsNullOrEmpty(KMIndex))
            {
                if (user.RoleName.Contains("门卫"))
                {
                    return RedirectToAction("DoorPostIndex");//NewDesktop
                }
                else if (user.RoleName.Contains("公司领导") || user.RoleName.Contains("厂级部门用户"))
                {
                    return RedirectToAction("KmJsc");
                    //return RedirectToAction("Index");
                }
            }


            if (!string.IsNullOrEmpty(bzIndex))
            {
                //华电首页（包含公司领导及普通用户）
                if (!string.IsNullOrEmpty(bzLeader))
                {
                    if (user.RoleName.Contains("公司领导") || user.RoleName.Contains("厂级部门用户"))
                    {
                        return RedirectToAction(bzLeader);
                    }
                    else if (user.RoleName.Contains("省级用户"))
                    {
                        return RedirectToAction("XJIndex");
                    }
                    else //普通用户层级
                    {
                        var url = itemBll.GetItemValue("bzWebUrl") + "login/signin";
                        var account = user.Account;
                        var time = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
                        var appKey = "DLBZ";
                        var ary = string.Join("^", new string[] { account, bzIndex, time, appKey });
                        var args = BSFramework.Util.DESEncrypt.Encrypt(ary);

                        return Redirect(url + "?args=" + args);
                    }
                }
                else
                {
                    var url = itemBll.GetItemValue("bzWebUrl") + "login/signin";
                    var account = user.Account;
                    var time = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
                    var appKey = "DLBZ";
                    var ary = string.Join("^", new string[] { account, bzIndex, time, appKey });
                    var args = BSFramework.Util.DESEncrypt.Encrypt(ary);

                    return Redirect(url + "?args=" + args);
                }
            }
            else
            {

                if (user == null)
                {
                    WebHelper.WriteCookie("login_error", "Overdue");//登录已超时,请重新登录
                    return RedirectToAction("Index", "Login");
                }
                if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
                {
                    //return Redirect("Index");
                    return RedirectToAction("Index");
                    //return RedirectToAction("XJDesktop");
                }
                if (user.RoleName.Contains("公司领导") || user.RoleName.Contains("厂级部门用户"))
                {
                    //return Redirect("../home/Index");
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("NewDesktop");
                    //return RedirectToAction("DoorPostIndex");//NewDesktop

                }
            }
        }

        /// <summary>
        /// 华电新疆
        /// </summary>
        /// <returns></returns>
        public ActionResult XJDesktop()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string url = new DataItemDetailBLL().GetItemValue(user.OrganizeCode, "HomeMap");
            ViewBag.MapUrl = url;
            return View();
        }

        /// <summary>
        /// 华电可门门岗首页
        /// </summary>
        /// <returns></returns>
        public ActionResult DoorPostIndex()
        {
            return View();
        }

        /// <summary>
        /// 华电江陵首页
        /// </summary>
        /// <returns></returns>
        public ActionResult JLIndex()
        {
            return View();
        }

        public ActionResult NewDesktop()
        {
            return View();
        }
        public ActionResult AdminDefaultDesktop()
        {
            return View();
        }
        public ActionResult AdminLTEDesktop()
        {
            return View();
        }
        public ActionResult AdminWindosDesktop()
        {
            return View();
        }
        public ActionResult AdminPrettyDesktop()
        {
            return View();
        }

        public ActionResult SkinIndex()
        {
            return View();
        }
        /// <summary>
        /// 安全指标预警
        /// </summary>
        /// <returns></returns>
        public ActionResult SafetyWarn()
        {
            return View();
        }
        /// <summary>
        /// 安全风险
        /// </summary>
        /// <returns></returns>
        public ActionResult SafetyRisk()
        {
            return View();
        }
        /// <summary>
        /// 安全风险趋势
        /// </summary>
        /// <returns></returns>
        public ActionResult SafetyTide()
        {
            return View();
        }
        /// <summary>
        /// 安全检查
        /// </summary>
        /// <returns></returns>
        public ActionResult SafetyCheck()
        {
            return View();
        }
        /// <summary>
        /// 事故隐患
        /// </summary>
        /// <returns></returns>
        public ActionResult SafetyHT()
        {
            return View();
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 访问功能
        /// </summary>
        /// <param name="moduleId">功能Id</param>
        /// <param name="moduleName">功能模块</param>
        /// <param name="moduleUrl">访问路径</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VisitModule(string moduleId, string moduleName, string moduleUrl)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.CategoryId = 2;
            logEntity.OperateTypeId = ((int)OperationType.Visit).ToString();
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Visit);
            logEntity.OperateAccount = OperatorProvider.Provider.Current().Account;
            logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
            logEntity.ModuleId = moduleId;
            logEntity.Module = moduleName;
            logEntity.ExecuteResult = 1;
            logEntity.ExecuteResultJson = "访问地址：" + moduleUrl;
            logEntity.WriteLog();
            return Content(moduleId);
        }
        /// <summary>
        /// 离开功能
        /// </summary>
        /// <param name="moduleId">功能模块Id</param>
        /// <returns></returns>
        public ActionResult LeaveModule(string moduleId)
        {
            return null;
        }
        #endregion



        #region 获取ip归属地
        /// <summary>
        /// 获取ip归属地
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAddress(string ip)
        {
            string address = IPLocation.GetLocation(ip);

            return address;
        }
        #endregion

        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <param name="mode">查询方式(0:我的,1:全部)</param>
        /// <returns></returns>
        [HttpPost]
        public string GetWorkList(int mode = 0)
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string execCheckNum = "0"; //待执行的安全检查数
            string perfectionHiddenNum = "0";//待完善隐患数
            string approvalHiddenNum = "0";//待评估隐患数
            string reformHiddenNum = "0";//待整改隐患数
            string delayHiddenNum = "0";//待审(核)批整改延期隐患数
            string reviewHiddenNum = "0";//待验收的隐患数
            string recheckHiddenNum = "0"; //待复查验证的隐患数
            string assessHiddenNum = "0";//待整改效果评估隐患数

            string planNum = "0";//进行中的风险评估计划数
            RiskPlanBLL planBll = new RiskPlanBLL();
            planNum = planBll.GetPlanCount(user, mode).ToString();
            SaftyCheckDataBLL saftbll = new SaftyCheckDataBLL();
            int[] countcheck = saftbll.GetCheckCount(user, mode);
            execCheckNum = countcheck.Sum() + "," + countcheck[0] + "," + countcheck[1] + "," + countcheck[2] + "," + countcheck[3] + "," + countcheck[4];

            //隐患待办事项
            var data = htbaseinfobll.QueryHidBacklogRecord(mode.ToString(), user.UserId);
            if (data.Rows.Count == 8)
            {
                approvalHiddenNum = data.Rows[0]["pnum"].ToString();//待评估隐患数
                perfectionHiddenNum = data.Rows[5]["pnum"].ToString();//待完善隐患数
                reformHiddenNum = data.Rows[1]["pnum"].ToString();//待整改隐患数
                delayHiddenNum = data.Rows[2]["pnum"].ToString();//待审(核)批整改延期隐患数
                reviewHiddenNum = data.Rows[3]["pnum"].ToString();//待验收的隐患数
                recheckHiddenNum = data.Rows[6]["pnum"].ToString();//待复查验证隐患数
                assessHiddenNum = data.Rows[4]["pnum"].ToString();//待整改效果评估隐患数
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new string[] { execCheckNum, perfectionHiddenNum, approvalHiddenNum, reformHiddenNum, delayHiddenNum, reviewHiddenNum, recheckHiddenNum, assessHiddenNum, planNum });
        }
        /// <summary>
        /// 根据预警区间设置绘制图表背景
        /// </summary>
        /// <param name="colors">各区间背景色设置</param>
        /// <returns></returns>
        [HttpPost]
        public string GetPlotBands(string colors = "#fff039,#ffcccc,#ffebd6,#ccebff")
        {

            DataItemDetailBLL itemBLL = new DataItemDetailBLL();
            string val = itemBLL.GetItemValue("基础预警区间分值设置");
            List<object> list = new List<object>();
            if (!string.IsNullOrEmpty(val))
            {
                string[] arr = val.Split('|');
                string[] arrColors = colors.Split(',');
                int j = 0;
                foreach (string str in arr)
                {
                    string[] arrVal = str.Split(',');
                    list.Add(new { color = arrColors[j], from = int.Parse(arrVal[0]), to = int.Parse(arrVal[1]) });
                    j++;
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 获取安全风险最近半年风险值以绘制图表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetRiskValues(string orgCode = "", string orgId = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(orgCode))
            {
                user = new Operator
                {
                    OrganizeId = orgId,
                    OrganizeCode = orgCode,
                    RoleName = "公司级用户,公司领导"
                };
            }
            RiskBLL riskBLL = new RiskBLL();
            return riskBLL.GetRiskValues(user);
        }


        #region 近半年隐患得分趋势
        /// <summary>
        /// 近半年隐患得分趋势
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHiddenValues(string orgCode = "", string orgId = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(orgCode))
            {
                curUser = new Operator
                {
                    OrganizeId = orgId,
                    OrganizeCode = orgCode,
                    RoleName = "公司级用户,公司领导"
                };
            }
            var list = new ClassificationBLL().GetList(curUser.OrganizeId);
            if (list.Count() == 0)
            {
                list = new ClassificationBLL().GetList("0");
            }
            List<string> xlist = new List<string>();
            List<double> ylist = new List<double>();
            for (int i = 6; i >= 1; i--)
            {
                string startDate = string.Empty;

                startDate = DateTime.Now.AddMonths(-i).ToString("yyyy-MM") + "-01";

                string endDate = Convert.ToDateTime(startDate).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

                double score = 0;
                string monthname = string.Empty;

                DataTable dt = htbaseinfobll.GetHiddenInfoOfWarning(curUser, startDate, endDate);
                foreach (DataRow row in dt.Rows)
                {
                    score += Convert.ToDouble(row["score"].ToString());
                }
                xlist.Add(DateTime.Now.AddMonths(-i).ToString("yyyy.MM"));
                ylist.Add(score);
            }
            var jsondata = new { x = xlist, y = ylist };

            return Content(jsondata.ToJson());
        }
        #endregion

        #region 近半年安全检查
        /// <summary>
        /// 近半年近半年安全检查得分趋势
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSafeCheckWarningM()
        {
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();
            return saBLL.GetSafeCheckWarningS();
        }
        #endregion
        /// <summary>
        /// 获取近一年预警指标数据以绘制图表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetRiskTrend(string orgCode = "", string orgId = "")
        {
            List<decimal> yValues = new List<decimal>();
            List<string> xValues = new List<string>();
            if (CacheFactory.Cache().GetCache<List<string>>("RiskTrend_X_" + orgCode) != null)
            {
                xValues = CacheFactory.Cache().GetCache<List<string>>("RiskTrend_X_" + orgCode);
                yValues = CacheFactory.Cache().GetCache<List<decimal>>("RiskTrend_Y_" + orgCode);
            }
            else
            {
                RiskBLL riskBLL = new RiskBLL();//安全风险
                SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//安全检查
                HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//事故隐患
                string time = DateTime.Now.AddMonths(-13).ToString("yyyy-MM-01");

                Operator user = OperatorProvider.Provider.Current();
                if (!string.IsNullOrEmpty(orgCode))
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = orgCode,
                        RoleName = "公司级用户,公司领导"
                    };
                }

                //ClassificationBLL classBLL = new ClassificationBLL();
                //List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();

                //if (list.Count == 0)
                //{
                //    list = classBLL.GetList("0").ToList();
                //}
                for (int j = 1; j <= 12; j++)
                {
                    //decimal totalScore = 0;
                    ////计算事故隐患总得分
                    string startDate = DateTime.Parse(time).AddMonths(j).ToString("yyyy-MM-dd");
                    //decimal score = htBLL.GetHiddenWarning(user, startDate);
                    //totalScore = score * decimal.Parse(list[0].WeightCoeffcient);
                    ////计算安全检查总得分
                    //score = saBLL.GetSafeCheckWarningM(user, startDate, 1);
                    //totalScore += score * decimal.Parse(list[1].WeightCoeffcient);
                    ////计算安全风险总得分
                    //score = riskBLL.GetRiskValueByTime(user, startDate);
                    //totalScore += score * decimal.Parse(list[2].WeightCoeffcient);

                    //yValues.Add(Math.Round(totalScore, 1));
                    //xValues.Add(DateTime.Parse(startDate).ToString("yyyy.MM"));

                    yValues.Add(0);
                    xValues.Add(DateTime.Parse(startDate).ToString("yyyy.MM"));
                }
                CacheFactory.Cache().WriteCache<List<string>>(xValues, "RiskTrend_X_" + orgCode, DateTime.Now.AddDays(2));
                CacheFactory.Cache().WriteCache<List<decimal>>(yValues, "RiskTrend_Y_" + orgCode, DateTime.Now.AddDays(2));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = xValues, y = yValues });
        }
        /// <summary>
        /// 集团用户首页指标汇总表格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetAllRiskSate()
        {
            RiskBLL riskBLL = new RiskBLL();//安全风险
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//安全检查
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//事故隐患
            List<object> data = new List<object>();
            OrganizeBLL orgBll = new OrganizeBLL();
            DataTable dt = orgBll.GetDTList();

            ClassificationBLL classBLL = new ClassificationBLL();
            foreach (DataRow dr in dt.Rows)
            {
                Operator user = new Operator
                {
                    OrganizeId = dr["orgid"].ToString(),
                    OrganizeCode = dr["encode"].ToString(),
                    RoleName = "公司级用户,公司领导"
                };
                //计算事故隐患总得分
                decimal htScore = htBLL.GetHiddenWarning(user, "");
                decimal jcScore = saBLL.GetSafeCheckSumCount(user);
                decimal fxScore = riskBLL.GetRiskValueByTime(user, "");

                List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();
                if (list.Count == 0)
                {
                    list = classBLL.GetList("0").ToList();
                }
                data.Add(new { name = dr[1].ToString(), code = dr[0].ToString(), fxScore = fxScore, jcScore = jcScore, htScore = htScore, score = htScore * decimal.Parse(list[0].WeightCoeffcient) + jcScore * decimal.Parse(list[1].WeightCoeffcient) + fxScore * decimal.Parse(list[2].WeightCoeffcient) });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 集团用户首页双控工作汇总表格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetAllWorkState()
        {
            RiskBLL riskBLL = new RiskBLL();//安全风险
            SaftyCheckDataBLL saBLL = new SaftyCheckDataBLL();//安全检查
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//事故隐患
            List<object> data = new List<object>();
            OrganizeBLL orgBll = new OrganizeBLL();
            DataTable dt = orgBll.GetDTList();

            ClassificationBLL classBLL = new ClassificationBLL();
            foreach (DataRow dr in dt.Rows)
            {
                Operator user = new Operator
                {
                    OrganizeId = dr["orgid"].ToString(),
                    OrganizeCode = dr["encode"].ToString(),
                    DeptCode = dr["encode"].ToString(),
                    RoleName = "公司级用户,公司领导"
                };
                decimal[] fxArr = riskBLL.GetHomeStat(user);
                DataTable dtJC = saBLL.GetCheckStat(user);
                DataTable dtHT = htBLL.QueryHidWorkList(user); //首页双控工作
                data.Add(new { name = dr[1].ToString(), code = dr[0].ToString(), yjfx = fxArr[1], ejfx = fxArr[2], jccs = dtJC.Rows[0][0].ToString(), ybyh = dtHT.Rows[3][2].ToString(), zdyh1 = dtHT.Rows[1][2].ToString(), zdyh2 = dtHT.Rows[2][2].ToString(), yzgs = dtHT.Rows[0][3].ToString(), wzgs = dtHT.Rows[0][4].ToString(), zgl = dtHT.Rows[0][6].ToString() });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 计算预警指标值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetWarnValue(string time = "", string orgCode = "", string orgId = "")
        {
            RiskBLL riskBLL = new RiskBLL();//安全风险
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//安全检查
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//事故隐患
            Operator user = OperatorProvider.Provider.Current();
            ClassificationBLL classBLL = new ClassificationBLL();
            List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();
            if (list.Count == 0)
            {
                list = classBLL.GetList("0").ToList();
            }
            decimal totalScore = 0; int count = 0;

            if (user.RoleName.Contains("省级用户") || user.RoleName.Contains("集团用户"))
            {
                if (!string.IsNullOrEmpty(orgCode))
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = orgCode,
                        RoleName = "公司级用户,公司领导"
                    };
                    totalScore = desktopbll.GetScore(user, time);
                }
                else
                {
                    OrganizeBLL orgBll = new OrganizeBLL();
                    DataTable dt = orgBll.GetDTList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        user = new Operator
                        {
                            OrganizeId = dr[2].ToString(),
                            OrganizeCode = dr[0].ToString(),
                            RoleName = "公司级用户,公司领导"
                        };
                        totalScore += desktopbll.GetScore(user, time);
                    }
                    totalScore = totalScore / dt.Rows.Count;
                }
            }
            else
            {
                totalScore = desktopbll.GetScore(user, time);
            }
            DataItemDetailBLL itemBLL = new DataItemDetailBLL();
            string val = itemBLL.GetItemValue("基础预警区间分值设置");
            count = 0;
            if (!string.IsNullOrEmpty(val))
            {
                string[] arr = val.Split('|');
                int j = 0;
                foreach (string str in arr)
                {
                    string[] arrVal = str.Split(',');
                    if (totalScore > decimal.Parse(arrVal[0]) && totalScore <= decimal.Parse(arrVal[1]))
                    {
                        count = j;
                        break;
                    }
                    j++;
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { score = Math.Round(totalScore, 1), index = count });
        }


        /// <summary>
        /// 计算预警指标值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetIndexWarnValue(string startDate = "", string endDate = "", string orgCode = "", string orgId = "")
        {
            Operator user = OperatorProvider.Provider.Current();
            decimal totalScore = 0; int count = 0;

            try
            {
                string key = "WarnScore_" + user.OrganizeCode;
                string score = CacheFactory.Cache().GetCache<string>(key);

                if (string.IsNullOrEmpty(score))
                {
                    if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
                    {
                        startDate = DateTime.Now.Year.ToString() + "-01" + "-01";
                        endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    }

                    SafetyAssessedArguments entity = new SafetyAssessedArguments();
                    entity.startDate = startDate;
                    entity.endDate = endDate;
                    if (user.RoleName.Contains("省级用户") || user.RoleName.Contains("集团用户"))
                    {
                        if (!string.IsNullOrEmpty(orgCode))
                        {
                            entity.orgId = orgId;
                            entity.orgCode = orgCode;
                            totalScore = desktopbll.GetSafetyAssessedValue(entity);
                        }
                        else
                        {
                            OrganizeBLL orgBll = new OrganizeBLL();
                            DataTable dt = orgBll.GetDTList();
                            foreach (DataRow dr in dt.Rows)
                            {
                                entity.orgId = dr[2].ToString();
                                entity.orgCode = dr[0].ToString();
                                totalScore += desktopbll.GetSafetyAssessedValue(entity);
                            }
                            totalScore = totalScore / dt.Rows.Count;
                        }
                    }
                    else
                    {
                        entity.orgId = user.OrganizeId;
                        entity.orgCode = user.OrganizeCode;
                        totalScore = desktopbll.GetSafetyAssessedValue(entity);
                    }
                    CacheFactory.Cache().WriteCache<string>(totalScore.ToString(), key, DateTime.Now.AddDays(1));
                }
                else
                {
                    totalScore = decimal.Parse(score);
                }

                string[] scorearr = new string[4];

                DataItemDetailBLL itemBLL = new DataItemDetailBLL();
                string val = itemBLL.GetItemValue("基础预警区间分值设置");
                count = 0;
                if (!string.IsNullOrEmpty(val))
                {
                    string[] arr = val.Split('|');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] arrVal = arr[i].Split(',');
                        scorearr[i] = arrVal[1];  //取后一个数字
                    }
                    int j = 0;
                    foreach (string str in arr)
                    {
                        string[] arrVal = str.Split(',');

                        if (totalScore > decimal.Parse(arrVal[0]) && totalScore <= decimal.Parse(arrVal[1]))
                        {
                            count = j;
                            break;
                        }
                        j++;
                    }
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(new { score = Math.Round(totalScore, 1), index = count, scorearry = scorearr });

            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }

        }
        #region 获取预警集合对象
        /// <summary>
        /// 获取预警集合对象
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="orgCode"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWarnData(string startDate = "", string endDate = "", string orgCode = "", string orgId = "")
        {
            Operator user = OperatorProvider.Provider.Current();
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                startDate = DateTime.Now.Year.ToString() + "-01" + "-01";
                endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }

            SafetyAssessedArguments entity = new SafetyAssessedArguments();
            entity.startDate = startDate;
            entity.endDate = endDate;
            if (user.RoleName.Contains("省级用户") || user.RoleName.Contains("集团用户"))
            {
                if (!string.IsNullOrEmpty(orgCode))
                {
                    entity.orgId = orgId;
                    entity.orgCode = orgCode;
                }
            }
            else
            {
                entity.orgId = user.OrganizeId;
                entity.orgCode = user.OrganizeCode;
            }
            var data = desktopbll.GetSafetyAssessedData(entity);

            return Content(new AjaxResult { type = ResultType.success, message = "获取数据成功", resultdata = data }.ToJson());
        }
        #endregion

        /// <summary>
        /// 获取风险考核指标结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetRiskList(string time)
        {
            Operator user = OperatorProvider.Provider.Current();
            RiskBLL riskBLL = new RiskBLL();//安全风险
            object data = riskBLL.GetRiskWarn(user, time);
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }


        [HttpPost]
        public ActionResult RemoveNote(string keyValue)
        {
            safenotebll.RemoveForm(keyValue);
            return Content("删除成功。");
        }
        [HttpPost]
        public ActionResult SaveDateNote(string keyValue, DateTime time, string value)
        {
            SafeNoteEntity note = new SafeNoteEntity() { Time = time, Value = value, Type = "2" };
            safenotebll.SaveForm(keyValue, note);
            return Content("操作成功。");
        }
        /// <summary>
        /// 获取安全事例的详情
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        [HttpGet]
        public string GetDateNote(DateTime time, string state)
        {
            Operator user = OperatorProvider.Provider.Current();
            if (state == "1")
            {
                List<SafeNoteEntity> noteList = safenotebll.GetList().Where(x => (x.Time.Value.ToString("yyyy-MM-dd") == time.ToString("yyyy-MM-dd") && x.CreateUserId == user.UserId)).ToList();
                DataTable dt = new DesktopBLL().GetWorkInfoByTime(user, time.ToString("yyyy-MM-dd"));
                foreach (DataRow dr in dt.Rows)
                {
                    SafeNoteEntity enity = new SafeNoteEntity
                    {
                        Id = dr[0].ToString(),
                        Time = time,
                        Value = dr[1].ToString(),
                        Type = dr[2].ToString()
                    };
                    noteList.Add(enity);
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(noteList);
            }
            else
            {
                var date = safenotebll.GetList().Where(x => x.CreateUserId == user.UserId).ToList().Select(x => new
                {
                    Type = x.Type,
                    Time = x.Time.Value.Month >= 10 ? (x.Time.Value.Day >= 10 ? x.Time.Value.ToString("yyyy-MM-dd") : x.Time.Value.ToString("yyyy-M-d")) : (x.Time.Value.Day < 10 ? x.Time.Value.ToString("yyyy-M-d") : x.Time.Value.ToString("yyyy-M-dd")),
                    Value = x.Value
                });
                return Newtonsoft.Json.JsonConvert.SerializeObject(date);
            }
        }
        [HttpGet]
        public string GetNoteEntity(string keyValue)
        {
            SafeNoteEntity noteEntity = safenotebll.GetEntity(keyValue);
            var data = new
            {
                Time = noteEntity.Time.Value.ToString("yyyy-MM-dd"),
                Value = noteEntity.Value,
                Id = noteEntity.Id
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }
        /// <summary>
        /// 获取班组安全卫士之星（广西华昇）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetStars(int type)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Encoding = Encoding.GetEncoding("utf-8");
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                byte[] bytes = null;
                //发送请求到web api并获取返回值，默认为post方式
                string apiUrl = detailBLL.GetItemValue("yjbzUrl");
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    data = new { Type = type }
                });
                nc.Add("json", json);
                bytes = wc.UploadValues(new Uri(apiUrl + "/SafetyScore/GetUserScorefirstthree"), "POST", nc);
                string result = Encoding.UTF8.GetString(bytes);
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 0, message = "操作成功", data = result });

            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 1, message = ex.Message });
            }

        }

    }
}
