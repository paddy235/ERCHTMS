using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Common;
using System.Text;
using WcfService.Service;
using System.Net;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.Home;
using ERCHTMS.Busines.LllegalManage;
using BSFramework.Util;

namespace ERCHTMS.Web.Controllers
{
    public class DesktopController : MvcControllerBase
    {

        DesktopBLL desktop = new DesktopBLL();
        private OutcommitfileBLL outcommitfilebll = new OutcommitfileBLL();
        private OutcommitfilesettingBLL outcommitfilesettingbll = new OutcommitfilesettingBLL();
        #region  通用版本的领导驾驶舱(电厂层级)

        #region 安全风险管控中心首页--领导驾驶舱（核心屏）

        #region 安全风险管控中心首页
        /// <summary>
        /// 安全风险管控中心首页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PowerPlantSafetyHomePage(string itemType, int mode = 0)
        {
            string result = string.Empty;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable data = desktop.GetDataSetForCommon(user, itemType, mode);
            return Success("获取数据成功", data);
        }
        #endregion
        /// <summary>
        /// 获取风险和隐患信息
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult GetRiskAndHt()
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                List<int> listRisk = desktop.GetRiskNumForGDXY(user);
                List<int> listHt = desktop.GetHtNum(user);
                FuelServiceClient client = new FuelServiceClient();
                List<SafeMeasureTicketTemp> result = new List<SafeMeasureTicketTemp>();
                bool isError = false;
                try
                {
                    result = client.GetSafeMeasureTicketData().Where(t => !string.IsNullOrWhiteSpace(t.BeLongAreaCode)).ToList();
                }
                catch
                {
                    isError = true;
                    result = new List<SafeMeasureTicketTemp>();
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "重大风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "重大风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "较大风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "一般风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "低风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "低风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "低风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                    //result.Add(new SafeMeasureTicketTemp
                    //{
                    //    HazardLevel = "低风险",
                    //    BeLongAreaCode = "00001001001001001003",
                    //    StartDate = DateTime.Now.AddDays(-1),
                    //    EndDate = DateTime.Now.AddDays(1)
                    //});
                } 
                List<string> listAssess = new List<string>();
                List<object> list = new List<object>();
                listAssess.Add("重大风险");
                listAssess.Add("较大风险");
                listAssess.Add("一般风险");
                listAssess.Add("低风险");
                if (result.Count > 0)
                {
                     foreach (string grade in listAssess)
                     {
                         list.Add(new {
                             grade=grade,
                             data=result.Where(t => t.HazardLevel == grade).ToList()
                         });
                     }
                }
                return Success(isError?"获取MIS工作票数据失败!":"获取数据成功", new { risk = listRisk, ht = listHt,work=list});
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }

        #region 获取整改率低于多少的电厂数据
        /// <summary>
        /// 获取整改率低于多少的电厂数据
        /// </summary>
        /// <param name="rankname"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RectificationRateUnderHowMany(string rankname, decimal num)
        {
            string result = string.Empty;
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DataTable dt = desktop.GetRectificationRateUnderHowMany(user, rankname, num);

            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(dt));
        }
        #endregion

        #endregion

        #region 安全风险管控中心首页--领导驾驶舱（左侧屏）

        #region 未闭环隐患统计
        /// <summary>
        /// 未闭环隐患统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetNoCloseLoopHidStatistics()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            //按责任单位统计
            DataTable unitdata = desktop.GetNoCloseLoopHidStatistics(user, 1);
            //按区域统计
            DataTable areadata = desktop.GetNoCloseLoopHidStatistics(user, 2);
            //按专业统计
            DataTable majordata = desktop.GetNoCloseLoopHidStatistics(user, 3);

            var jsondata = new
            {
                unitdata = unitdata,
                areadata = areadata,
                majordata = majordata
            };

            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(jsondata));
        }
        #endregion

        #region 隐患整改率统计
        /// <summary>
        /// 隐患整改率统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHiddenChangeForLeaderCockpit()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DataTable data = desktop.GetHiddenChangeForLeaderCockpit(user);

            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }
        #endregion

        #region 今日作业风险/高风险作业统计
        /// <summary>
        /// 今日作业风险/高风险作业统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHighRiskWorkingForLeaderCockpit()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dtdata = new DataTable();
            DataTable tjdata = new DataTable();
            string gxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                dtdata = desktop.GetHighRiskWorkingForLeaderCockpit(user, 3);

                tjdata = desktop.GetHighRiskWorkingForLeaderCockpit(user, 4);
            }
            else
            {
                dtdata = desktop.GetHighRiskWorkingForLeaderCockpit(user, 0);

                tjdata = desktop.GetHighRiskWorkingForLeaderCockpit(user, 1);
            }
            var jsondata = new
            {
                dtdata = dtdata,
                tjdata = tjdata
            };
            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(jsondata));
        }
        #endregion

        #region 高风险作业数量
        /// <summary>
        /// 高风险作业数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHighRiskWorkingNum()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable resultData = desktop.GetHighRiskWorkingForLeaderCockpit(user, 2);
            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(resultData));
        }
        #endregion

        #endregion

        #region 安全风险管控中心首页--领导驾驶舱（右侧屏）

        #region 各部门未闭环违章统计
        /// <summary>
        /// 各部门未闭环违章统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetNoCloseLoopLllegalStatistics()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DataTable data = desktop.GetNoCloseLoopLllegalStatistics(user);

            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }
        #endregion


        #region 各部门违章整改率统计
        /// <summary>
        /// 各部门违章整改率统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLllegalChangeForLeaderCockpit()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DataTable data = desktop.GetLllegalChangeForLeaderCockpit(user);

            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }
        #endregion

        #endregion

        #endregion

        /// <summary>
        /// 是否是通用版
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string IsGeneric()
        {
            string industry = OperatorProvider.Provider.Current().Industry;
            if (industry.IsNullOrEmpty() || industry == "电力")//如果为空 或者为电力 则为电力双控 其余的则为通用双控
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }

        /// <summary>
        /// 领导驾驶舱首页实时监控数据统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetStatData()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string orgCode = user.OrganizeCode;

            List<decimal> data = new List<decimal>();
            HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
            var dt = htbaseinfobll.QueryHidWorkList(user);
            DesktopBLL desktop = new DesktopBLL();
            data.Add(decimal.Parse(dt.Rows[0][2].ToString()));//隐患总数
            data.Add(decimal.Parse(dt.Rows[1][2].ToString()) + int.Parse(dt.Rows[2][2].ToString()));//重大隐患数
            data.Add(decimal.Parse(dt.Rows[0][3].ToString()));//已整改隐患数
            data.Add(decimal.Parse(dt.Rows[0][4].ToString()));//未整改隐患数

            List<int> list = desktop.GetDangerSourceNum(user);
            data.Add(list[0]);//危险源总数
            data.Add(list[1]);//重大危险源数

            list = desktop.GetAccidentNum(user);
            data.Add(list[0]);//事故起数
            data.Add(list[1]);//事故死亡人数
            data.Add(list[2]);//事故重伤人数

            SaftyCheckDataBLL saftycheckdatabll = new SaftyCheckDataBLL();
            var dtCheck = saftycheckdatabll.GetCheckStat(user);
            data.Add(decimal.Parse(dtCheck.Rows[0][0].ToString()));//安全检查次数
            data.Add(decimal.Parse(desktop.GetCheckHtNum(user).ToString()));//检查发现隐患数

            list = desktop.GetWBProjectNum(user);
            data.Add(list[0]);//外包工程数
            data.Add(list[1]);//外包工程在场人数

            list = desktop.GetRiskNum(user);
            data.Add(list[0]);//风险总数

            list = desktop.GetWorkNum(user);
            data.Add(list[3]);//高风险作业总数

            data.Add(decimal.Parse(dt.Rows[0][5].ToString()));//逾期未整改隐患数
            data.Add(decimal.Parse(dt.Rows[0][6].ToString()));//隐患整改率

            data.Add(list[4]);//正在进行的高风险作业数

            //日常安全检查次数
            string rcNum = desktop.GetSafetyCheckOfEveryDay(user);
            data.Add(int.Parse(rcNum));

            return Success("获取数据成功", Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }
        /// <summary>
        /// 根据配置动态获取实时监控数据指标或待办事项
        /// </summary>
        /// <param name="itemType">获取类型（实时监控,待办事项）</param>
        /// <param name="mode">待办事项查询方式（0:本人，1:电厂全部）</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeptStatData(string itemType, int mode = 0,int pfrom=0)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                  DataTable data = new DesktopBLL().GetDeptDataStat(user, itemType, mode);
                  if (pfrom==1)
                  {
                      data.Columns.Remove("icon"); data.Columns.Remove("address"); data.Columns.Remove("callback"); data.Columns.Remove("itemstyle"); data.Columns.Remove("itemtype");
                  }
                  return Success("获取数据成功", data);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
          
        }
        /// <summary>
        /// 隐患统计图表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHTChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("公司领导") || user.IsSystem || user.RoleName.Contains("厂级部门用户"))
            {
                string orgCode = user.OrganizeCode;
                return Success("获取数据成功", new DesktopBLL().GetHTChart(user));
            }
            else
            {
                return Error("没有数据");
            }
        }
        /// <summary>
        /// 按工程类型统计外包工程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProjectChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("公司领导") || user.RoleName.Contains("厂级部门用户"))
            {
                string orgCode = user.OrganizeCode;
                return Success("获取数据成功", new DesktopBLL().GetProjectChart(user));
            }
            else
            {
                return Error("没有数据");
            }
        }

        /// <summary>
        /// 按工程类型统计外包工程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProjectChart2()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("公司领导") || user.IsSystem || user.RoleName.Contains("厂级部门用户"))
            {
                string orgCode = user.OrganizeCode;
                var dt = new DesktopBLL().GetProjectChart(user);
                List<string> xValues = new List<string>();
                List<int> yValues = new List<int>();
                foreach (DataRow item in dt.Rows)
                {
                    yValues.Add(int.Parse(item["Num"].ToString()));
                    xValues.Add(item["Name"].ToString());
                }
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, yValues = yValues });
                return Success("获取数据成功", result);
            }
            else
            {
                return Error("没有数据");
            }
        }
        /// <summary>
        /// 按工程风险等级统计外包工程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProjectChart3()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("公司领导") || user.IsSystem || user.RoleName.Contains("厂级部门用户"))
            {
                string orgCode = user.OrganizeCode;
                var dt = new DesktopBLL().GetProjectChartByLevel(user);
                List<string> xValues = new List<string>();
                List<int> yValues = new List<int>();
                foreach (DataRow item in dt.Rows)
                {
                    yValues.Add(int.Parse(item["Num"].ToString()));
                    xValues.Add(item["Name"].ToString());
                }
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { xValues = xValues, yValues = yValues });
                return Success("获取数据成功", result);
            }
            else
            {
                return Error("没有数据");
            }
        }



        #region 外包流程管理
        /// <summary>
        /// 外包流程管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProjectData()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var data = new OutprojectblacklistBLL().ToAuditOutPeoject(user);
            var data1 = new OutprojectblacklistBLL().ToIndexData(user);
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(new { data = data, data1 = data1 });
            return Success("获取数据成功", result);
        }
        /// <summary>
        /// 根据机构Code查询是否有资料说明并查询当前登陆用户是否设置了提醒
        /// </summary>
        /// <param name="orgCode">机构Code</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEntityByOrgCode(string orgCode) {
       

            var data = outcommitfilebll.GetEntityByOrgCode(orgCode);

            Operator currUser = OperatorProvider.Provider.Current();
            var s = outcommitfilesettingbll.GetList().Where(x => x.FileCommitId == data.ID && x.UserId == currUser.UserId).FirstOrDefault();
            var jsonData = new
            {
                data = data,
                userSet = s
            };
            return ToJsonResult(jsonData);
        }
        #endregion




        /// <summary>
        /// 外包人员数量变化趋势图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProjectPersonChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("公司领导") || user.IsSystem || user.RoleName.Contains("厂级部门用户"))
            {
                string orgCode = user.OrganizeCode;
                return Success("获取数据成功", new DesktopBLL().GetProjectPersonChart(user));
            }
            else
            {
                return Error("没有数据");
            }
        }
        /// <summary>
        /// 获取通知公告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetNotices()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetNotices(user));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取安全会议
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMeets()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetMeets(user));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取安全动态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTrends()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetTrends(user));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取红黑榜
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRedBlack(int mode = 0)
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetRedBlack(user, mode));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取风险等级数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRiskCounChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetRiskCounChart(user));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 隐患类别统计图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHtTypeChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                var dt = new DesktopBLL().GetHTTypeChart(user);
                return Success("获取数据成功", dt);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 隐患变化趋势图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHTChangeChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetHTChangeChart(user));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 隐患及安全检查变化趋势图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTendencyChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetTendencyChart(user));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        ///外包工程安全管控统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProjectStat()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                return Success("获取数据成功", new DesktopBLL().GetProjectStat(user));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 高风险作业分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWorkTypeChart()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("公司领导") || user.IsSystem || user.RoleName.Contains("厂级部门用户"))
            {
                string orgCode = user.OrganizeCode;
                return Success("获取数据成功", new DesktopBLL().GetWorkTypeChart(user));
            }
            else
            {
                return Error("没有数据");
            }
        }
        /// <summary>
        /// 获取安全事例
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWorksRecord()
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                var data = new DesktopBLL().GetWorks(user);
                return Success("获取数据成功", data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        public ActionResult GetTestData(string code = "")
        {
            try
            {
                //获取当前用户
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                FuelServiceClient client = new FuelServiceClient();
                var result = client.GetSafeMeasureTicketData().ToList();
                if (!string.IsNullOrWhiteSpace(code))
                {
                    DataTable dtDis = new DepartmentBLL().GetDataTable(string.Format("select d.districtcode from bis_district d where d.description='{0}' and d.districtcode like '{1}%'", code, user.OrganizeCode));
                    if (dtDis.Rows.Count > 0)
                    {
                        string areaCode = dtDis.Rows[0][0].ToString();
                        result = result.Where(t => t.BeLongAreaCode.StartsWith(areaCode) && t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now).ToList();
                    }
                }
                return Success("获取数据成功", result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        /// <summary>
        /// 获取工作票据详情（国电新疆鸿雁池）
        /// </summary>
        /// <param name="areaCodes"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTicketInfo(string ticketId = "DB05ECBB574F4B4383482374C34EE37B")
        {
            try
            {
                var deptBll = new DepartmentBLL();
                DataTable dt = deptBll.GetDataTable(string.Format("select wt_code,status_name,unit_name,wt_type_name,content,area_name,risk_grade_name,org_name,maint_org_name,work_leader_name,work_class_person,wt_signer_name,permit_by_name,permit_start_time,act_end_time,now_work_leader_name,delay_time from wo_wt_view where id='{0}' and rownum=1", ticketId),"BaseDb1",1);
                int count1 = deptBll.GetDataTable(string.Format("select count(1) num from wo_wt_view where main_ticket_id='{0}' ", ticketId), "BaseDb1", 1).Rows[0][0].ToInt();
                int count2 = deptBll.GetDataTable(string.Format("select count(1) num from WO_WT_FIRE_VIEW where WT_ID='{0}'", ticketId), "BaseDb1", 1).Rows[0][0].ToInt();
                DataTable dtMeasures = deptBll.GetDataTable(string.Format("select process_name,danger_source_name,consequence_name,risk_grade_name,measure_name from wo_wt_view where main_ticket_id='{0}'", ticketId), "BaseDb1", 1);
                return Success("获取数据成功", new { ticket = dt, measures = dtMeasures, stat = new List<int> { count1, count2 } });
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
          
        }
        /// <summary>
        /// 根据区域和风险等级获取工作票信息（国电新疆鸿雁池）
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTicketList(string areaCode,string grade)
        {
            try
            {
                var deptBll = new DepartmentBLL();
                DataTable dt = deptBll.GetDataTable(string.Format("select distinct id,wt_code,unit_name,wt_type_name,content,risk_grade_name,work_leader_name,permit_start_time,act_end_time from wo_wt_view where status_name='开工' and risk_grade_name='{0}' and area_unm like '{1}%'", grade, areaCode), "BaseDb1", 1);
                return Success("获取数据成功", dt);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpPost]
        public ActionResult GetAreaStatusTest(string areaCodes)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var di = new DataItemDetailBLL();
            var deptBll = new DepartmentBLL();
            DataTable data = new DataTable();
            //对接国电新疆鸿雁池两票
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("code");
                dt.Columns.Add("status");
                dt.Columns.Add("htnum");
                dt.Columns.Add("fxnum");
                dt.Columns.Add("areacode");
                dt.Columns.Add("wxnum");
                dt.Columns.Add("content");
                string htNum = "";
                StringBuilder sb = new StringBuilder();
                foreach (string code in areaCodes.Split(','))
                {
                    string areaCode = "";
                    htNum = "";
                    DataTable dtDis = deptBll.GetDataTable(string.Format("select d.districtcode from bis_district d where d.description='{0}' and d.districtcode like '{1}%'", code, user.OrganizeCode));
                    if (dtDis.Rows.Count > 0)
                    {
                        int grade = 0;
                        areaCode = dtDis.Rows[0][0].ToString();
                        string sql = " and risktype in('管理','设备','区域')";
                        DataTable dtCount = deptBll.GetDataTable(string.Format("select min(gradeval) from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1}", areaCode, sql));
                        if (dtCount.Rows.Count > 0)
                        {
                            if (dtCount.Rows[0][0] != DBNull.Value)
                            {
                                grade = dtCount.Rows[0][0].ToString().ToInt();
                            }

                        }
                        //隐患数量
                        DataTable dtHt = deptBll.GetDataTable(string.Format("select rankname,count(1) num from v_basehiddeninfo t where t.workstream!='整改结束' and t.hidpoint like '{0}%' group by rankname", areaCode));
                        if (dtHt.Rows.Count > 0)
                        {
                            var rows = dtHt.Select("rankname='一般隐患'");
                            if (rows.Length > 0)
                            {
                                htNum = rows[0][1].ToString();
                            }
                            else
                            {
                                htNum = "0";
                            }
                            rows = dtHt.Select("rankname='重大隐患'");
                            if (rows.Length > 0)
                            {
                                htNum += "," + rows[0][1].ToString();
                            }
                            else
                            {
                                htNum += ",0";
                            }
                        }

                        DataTable dtItems = deptBll.GetDataTable(string.Format("select id,wt_code,unit_code,unit_name,wt_type_name,content,min(risk_grade_name) risk_grade_name,work_leader_name,permit_start_time,act_end_time from (select id,wt_code,unit_code,unit_name,wt_type_name,content,case when risk_grade_name='重大风险' then 1 when risk_grade_name='较大风险' then 2  when risk_grade_name='一般风险' then 3 when risk_grade_name='低风险' then 4 else 5 end risk_grade_name,work_leader_name,permit_start_time,act_end_time from wo_wt_view t where status_name='开工' and area_unm like '{0}%') t group by id,wt_code,unit_code,unit_name,wt_type_name,content,work_leader_name,permit_start_time,act_end_time", areaCode), "BaseDb1", 1);
                        List<Assess> listAssess = new List<Assess>();
                        listAssess.Add(new Assess { GradeCode = "4", Grade = "低风险", AreaCode = areaCode, Count = 0 });
                        listAssess.Add(new Assess { GradeCode = "3", Grade = "一般风险", AreaCode = areaCode, Count = 0 });
                        listAssess.Add(new Assess { GradeCode = "2", Grade = "较大风险", AreaCode = areaCode, Count = 0 });
                        listAssess.Add(new Assess { GradeCode = "1", Grade = "重大风险", AreaCode = areaCode, Count = 0 });
                        int level = 0;
                        if (dtItems.Rows.Count > 0)
                        {
                          
                            foreach (Assess ass in listAssess)
                            {
                                foreach (DataRow sm in dtItems.Rows)
                                {
                                    if (ass.GradeCode == sm["risk_grade_name"].ToString())
                                    {
                                        ass.Count++;
                                    }
                                }
                            }

                            if (listAssess.Where(t => t.GradeCode == "1" && t.Count > 0).Count() > 0)
                            {
                                level = 1;
                            }
                            else if (listAssess.Where(t => t.GradeCode == "2" && t.Count > 0).Count() > 0)
                            {
                                level = 2;
                            }
                            else if (listAssess.Where(t => t.GradeCode == "3" && t.Count > 0).Count() > 0)
                            {
                                level = 3;
                            }
                            else if (listAssess.Where(t => t.GradeCode == "4" && t.Count > 0).Count() > 0)
                            {
                                level = 4;
                            }
                            else
                            {
                                level = 0;
                            }


                        }
                        DataRow row = dt.NewRow();
                        row[0] = code;
                        row[1] = level;
                        row[4] = areaCode;
                        row["fxnum"] = Newtonsoft.Json.JsonConvert.SerializeObject(listAssess);
                        row["htnum"] = htNum;
                        if (dtItems.Rows.Count > 0)
                        {
                            row["content"] = Newtonsoft.Json.JsonConvert.SerializeObject(dtItems);
                        }
                        dt.Rows.Add(row);
                    }
                }
                return Success("", dt);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
               
        }
        /// <summary>
        /// 获取区域相关的隐患风险或作业信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAreaStatus(string areaCodes,int mode=1)
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                string apiUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("WebApiUrl", "AppSettings");
                var di = new DataItemDetailBLL();
                var deptBll = new DepartmentBLL();
                #region 判断是否是国电荥阳版本
                string val = di.GetItemValue("IsGdxy");
                if (!string.IsNullOrEmpty(val) && mode==2)
                {
                    FuelServiceClient client = new FuelServiceClient();
                    List<SafeMeasureTicketTemp> result = new List<SafeMeasureTicketTemp>();
                    bool isError = false;
                    try
                    {
                        result = client.GetSafeMeasureTicketData().Where(t => !string.IsNullOrWhiteSpace(t.BeLongAreaCode)).ToList();
                    }
                    catch
                    {
                        isError = true;
                        result = new List<SafeMeasureTicketTemp>();
                    } 

                    DataTable dt = new DataTable();
                    dt.Columns.Add("code");
                    dt.Columns.Add("status");
                    dt.Columns.Add("htnum");
                    dt.Columns.Add("fxnum");
                    dt.Columns.Add("areacode");
                    dt.Columns.Add("wxnum");
                    dt.Columns.Add("content");
                    string htNum = "";
                    StringBuilder sb = new StringBuilder();
                    foreach (string code in areaCodes.Split(','))
                    {
                        string areaCode = "";
                        htNum = "";
                        DataTable dtDis = deptBll.GetDataTable(string.Format("select d.districtcode from bis_district d where d.description='{0}' and d.districtcode like '{1}%'", code, user.OrganizeCode));
                        if (dtDis.Rows.Count > 0)
                        {
                            int grade = 0;
                            areaCode = dtDis.Rows[0][0].ToString();
                            string sql = " and risktype in('管理','设备','区域')";
                            DataTable dtCount = deptBll.GetDataTable(string.Format("select min(gradeval) from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1}", areaCode, sql));
                            if (dtCount.Rows.Count>0)
                            {
                                if(dtCount.Rows[0][0]!=DBNull.Value)
                                {
                                    grade = dtCount.Rows[0][0].ToString().ToInt();
                                }
                               
                            }
                            //隐患数量
                            DataTable dtHt = deptBll.GetDataTable(string.Format("select rankname,count(1) num from v_basehiddeninfo t where t.workstream!='整改结束' and t.hidpoint like '{0}%' group by rankname", areaCode));
                            if (dtHt.Rows.Count > 0)
                            {
                                var rows = dtHt.Select("rankname='一般隐患'");
                                if (rows.Length > 0)
                                {
                                    htNum = rows[0][1].ToString();
                                }
                                else
                                {
                                    htNum = "0";
                                }
                                rows = dtHt.Select("rankname='重大隐患'");
                                if (rows.Length > 0)
                                {
                                    htNum += "," + rows[0][1].ToString();
                                }
                                else
                                {
                                    htNum += ",0";
                                }
                            }

                            var dtItems = result.Where(t => t.BeLongAreaCode.StartsWith(areaCode) && t.StartDate < DateTime.Now && t.EndDate > DateTime.Now).ToList();
                            List<Assess> listAssess = new List<Assess>();
                            listAssess.Add(new Assess { Grade = "低风险", AreaCode = areaCode, Count = 0 });
                            listAssess.Add(new Assess { Grade = "一般风险", AreaCode = areaCode, Count = 0 });
                            listAssess.Add(new Assess { Grade = "较大风险", AreaCode = areaCode, Count = 0 });
                            listAssess.Add(new Assess { Grade = "重大风险", AreaCode = areaCode, Count = 0 });
                            if (dtItems.Count > 0)
                            {
                                int level = 0;
                                foreach (Assess ass in listAssess)
                                {
                                    foreach (SafeMeasureTicketTemp sm in dtItems)
                                    {
                                        if (ass.Grade == sm.HazardLevel)
                                        {
                                            ass.Count++;
                                        }
                                    }
                                }

                                if (listAssess.Where(t => t.Grade == "重大风险" && t.Count > 0).Count() > 0)
                                {
                                    level = 1;
                                }
                                else if (listAssess.Where(t => t.Grade == "较大风险" && t.Count > 0).Count() > 0)
                                {
                                    level = 2;
                                }
                                else if (listAssess.Where(t => t.Grade == "一般风险" && t.Count > 0).Count() > 0)
                                {
                                    level = 3;
                                }
                                else if (listAssess.Where(t => t.Grade == "低风险" && t.Count > 0).Count() > 0)
                                {
                                    level = 4;
                                }
                                else
                                {
                                    level = 0;
                                }
                                if (level<grade)
                                {
                                    grade = level;
                                }
                               
                            }
                            DataRow row = dt.NewRow();
                            row[0] = code;
                            row[1] = grade;
                            row[4] = areaCode;
                            row["fxnum"] = Newtonsoft.Json.JsonConvert.SerializeObject(listAssess);
                            row["htnum"] = htNum;
                            if (dtItems.Count > 0)
                            {
                                row["content"] = Newtonsoft.Json.JsonConvert.SerializeObject(dtItems);
                            }
                            dt.Rows.Add(row);
                        }
                    }
                    return Success(apiUrl, dt);
                }
                #endregion
                else
                {
                    DataTable data=new DataTable();
                    //对接国电新疆鸿雁池两票
                    string orgCode =di.GetItemValue("xjhyc", "FactoryEncode");
                    if(user.OrganizeCode==orgCode && mode==2)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("code");
                        dt.Columns.Add("status");
                        dt.Columns.Add("htnum");
                        dt.Columns.Add("fxnum");
                        dt.Columns.Add("areacode");
                        dt.Columns.Add("wxnum");
                        dt.Columns.Add("content");
                        string htNum = "";
                        StringBuilder sb = new StringBuilder();
                        foreach (string code in areaCodes.Split(','))
                        {
                            string areaCode = "";
                            htNum = "";
                            DataTable dtDis = deptBll.GetDataTable(string.Format("select d.districtcode from bis_district d where d.description='{0}' and d.districtcode like '{1}%'", code, user.OrganizeCode));
                            if (dtDis.Rows.Count > 0)
                            {
                                int grade = 0;
                                areaCode = dtDis.Rows[0][0].ToString();
                                string sql = " and risktype in('管理','设备','区域')";
                                DataTable dtCount = deptBll.GetDataTable(string.Format("select min(gradeval) from BIS_RISKASSESS t where status=1 and deletemark=0 and t.areacode like '{0}%' {1}", areaCode, sql));
                                if (dtCount.Rows.Count > 0)
                                {
                                    if (dtCount.Rows[0][0] != DBNull.Value)
                                    {
                                        grade = dtCount.Rows[0][0].ToString().ToInt();
                                    }

                                }
                                //隐患数量
                                DataTable dtHt = deptBll.GetDataTable(string.Format("select rankname,count(1) num from v_basehiddeninfo t where t.workstream!='整改结束' and t.hidpoint like '{0}%' group by rankname", areaCode));
                                if (dtHt.Rows.Count > 0)
                                {
                                    var rows = dtHt.Select("rankname='一般隐患'");
                                    if (rows.Length > 0)
                                    {
                                        htNum = rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        htNum = "0";
                                    }
                                    rows = dtHt.Select("rankname='重大隐患'");
                                    if (rows.Length > 0)
                                    {
                                        htNum += "," + rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        htNum += ",0";
                                    }
                                }

                               // DataTable dtItems = deptBll.GetDataTable(string.Format("select id,wt_code,unit_name,wt_type_name,content,min(risk_grade_name) risk_grade_name,work_leader_name,permit_start_time,act_end_time from (select id,wt_code,unit_name,wt_type_name,content,case when risk_grade_name='重大风险' then 1 when risk_grade_name='较大风险' then 2  when risk_grade_name='一般风险' then 3 when risk_grade_name='低风险' then 4 else 5 end risk_grade_name,work_leader_name,permit_start_time,act_end_time from wo_wt_view t where status_name='开工' and area_unm like '{0}%' and to_date(t.permit_start_time,'yyyy-mm-dd HH24:mi:ss')<sysdate and To_date(t.act_end_time,'yyyy-mm-dd HH24:mi:ss') > sysdate) t group by id,wt_code,unit_name,wt_type_name,content,work_leader_name,permit_start_time,act_end_time", areaCode), "BaseDb1", 1);
                                DataTable dtItems = deptBll.GetDataTable(string.Format("select id,wt_code,unit_name,wt_type_name,content,min(risk_grade_name) risk_grade_name,work_leader_name,permit_start_time,act_end_time from (select id,wt_code,unit_name,wt_type_name,content,case when risk_grade_name='重大风险' then 1 when risk_grade_name='较大风险' then 2  when risk_grade_name='一般风险' then 3 when risk_grade_name='低风险' then 4 else 5 end risk_grade_name,work_leader_name,permit_start_time,act_end_time from wo_wt_view t where status_name='开工' and area_unm like '{0}%') t group by id,wt_code,unit_name,wt_type_name,content,work_leader_name,permit_start_time,act_end_time", areaCode), "BaseDb1", 1);
                                List<Assess> listAssess = new List<Assess>();
                                listAssess.Add(new Assess {GradeCode="4", Grade = "低风险", AreaCode = areaCode, Count = 0 });
                                listAssess.Add(new Assess { GradeCode = "3", Grade = "一般风险", AreaCode = areaCode, Count = 0 });
                                listAssess.Add(new Assess { GradeCode = "2", Grade = "较大风险", AreaCode = areaCode, Count = 0 });
                                listAssess.Add(new Assess { GradeCode = "1", Grade = "重大风险", AreaCode = areaCode, Count = 0 });
                                int level = 0;
                                if (dtItems.Rows.Count>0)
                                {
                                  
                                    foreach (Assess ass in listAssess)
                                    {
                                        foreach (DataRow sm in dtItems.Rows)
                                        {
                                            if (ass.GradeCode == sm["risk_grade_name"].ToString())
                                            {
                                                ass.Count++;
                                            }
                                        }
                                    }

                                    if (listAssess.Where(t => t.GradeCode == "1" && t.Count > 0).Count() > 0)
                                    {
                                        level = 1;
                                    }
                                    else if (listAssess.Where(t => t.GradeCode == "2" && t.Count > 0).Count() > 0)
                                    {
                                        level = 2;
                                    }
                                    else if (listAssess.Where(t => t.GradeCode == "3" && t.Count > 0).Count() > 0)
                                    {
                                        level = 3;
                                    }
                                    else if (listAssess.Where(t => t.GradeCode == "4" && t.Count > 0).Count() > 0)
                                    {
                                        level = 4;
                                    }
                                    else
                                    {
                                        level = 0;
                                    }
                                }
                                DataRow row = dt.NewRow();
                                row[0] = code;
                                row[1] = level;
                                row[4] = areaCode;
                                row["fxnum"] = Newtonsoft.Json.JsonConvert.SerializeObject(listAssess);
                                row["htnum"] = htNum;
                                if (dtItems.Rows.Count > 0)
                                {
                                    row["content"] = Newtonsoft.Json.JsonConvert.SerializeObject(dtItems);
                                }
                                dt.Rows.Add(row);
                            }
                        }
                        return Success(apiUrl, dt);
                    }
                    else
                    {
                         data = new DesktopBLL().GetAreaStatus(user, areaCodes, mode);
                         return Success(apiUrl, data);
                    }
                  
                }
               
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 电厂隐患排名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRatioDataOfFactory(int top = 0, int mode = 0)
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                var data = new DesktopBLL().GetRatioDataOfFactory(user, mode).ToTable();

                DataTable newDt = null;
                if (top > 0)
                {
                    newDt = data.Clone();
                    for (int j = 0; j < top; j++)
                    {
                        DataRow newRow = newDt.NewRow();
                        newRow[0] = data.Rows[j][0];
                        newRow[1] = data.Rows[j][1];
                        newRow[2] = data.Rows[j][2];
                        newRow[3] = data.Rows[j][3];
                        newDt.Rows.Add(newRow);
                    }
                }
                else
                {
                    newDt = data;
                }
                return Success("获取数据成功", newDt);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 电厂月度安全指标对比分析
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRatioDataOfMonth(string time)
        {
            //获取当前用户
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                time = DateTime.Parse(time + "-01").ToString();
                string month = DateTime.Parse(time).AddDays(-1).ToString();
                List<object> data = new List<object>();
                var desktop = new DesktopBLL();
                DataTable dtDepts = new DepartmentBLL().GetAllFactory(user);
                foreach (DataRow dr in dtDepts.Rows)
                {
                    List<object> temp = new List<object>();
                    temp.Add(dr[1].ToString());
                    List<decimal> listCurrMonth = desktop.GetHt2CheckOfFactory(dr[4].ToString(), time, dr[0].ToString());//当月数据
                    List<decimal> listprevMonth = desktop.GetHt2CheckOfFactory(dr[4].ToString(), month);//上月数据
                    for (int j = 0; j < listCurrMonth.Count; j++)
                    {
                        decimal val = listCurrMonth[j] - listprevMonth[j];
                        temp.Add(listCurrMonth[j]);
                        temp.Add(val);
                    }
                    temp.Add(dr[0].ToString());
                    temp.Add(dr[4].ToString());
                    data.Add(temp);

                }
                return Success("获取数据成功", data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取电厂相关信息（用于省公司首页地图展示使用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFactoryInfo()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = new DepartmentBLL().GetAllFactory(user);
            var desktop = new DesktopBLL();
            List<object> data = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                user = new Operator
                {
                    OrganizeId = dr[4].ToString(),
                    OrganizeCode = dr[0].ToString(),
                    RoleName = "公司级用户,公司领导"
                };
                List<string> list = desktop.GetScoreInfo(user);
                data.Add(new
                {
                    level = list[1],
                    score = list[0],
                    man = dr[5].ToString(),
                    name = dr[1].ToString(),
                    address = dr[6].ToString(),
                    encode = dr[0].ToString(),
                    id = dr[4].ToString()
                });
            }
            return Success("获取数据成功", data);
        }
        /// <summary>
        /// 实时监控数据统计（华电新疆省公司）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStatForXJ()
        {
            List<decimal> data = new List<decimal>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DesktopBLL desktop = new DesktopBLL();
            List<int> list = desktop.GetSafetyCheckForGroup(user);
            data.Add(list[0]);//省公司安全检查次数
            list = desktop.GetHtForGroup(user);
            data.Add(list[3]);//省公司发现隐患数
            data.Add(list[4]);//未治理完成重大隐患数
            List<decimal> list1 = desktop.GetWarnItems(user);
            data.Add(list1[2]);//隐患整改率
            data.Add(list[2]); //逾期未整改隐患数
            data.Add(list1[1]); //隐患整改率低于80%的电厂

            return Success("获取数据成功", data);
        }

        #region 获取省公司下属各电厂隐患整改率
        /// <summary>
        /// 获取省公司下属各电厂隐患整改率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHtInfo(int mode = 0)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DesktopBLL desktop = new DesktopBLL();
            List<object> data = new List<object>();

            //隐患整改情况
            if (mode < 2)
            {
                DataTable dt = new DepartmentBLL().GetAllFactory(user);
                #region 隐患整改情况
                foreach (DataRow dr in dt.Rows)
                {
                    List<decimal> list = desktop.GetHtZgl(dr["departmentid"].ToString());
                    decimal zgl = list[1];
                    decimal count = list[0];
                    if (mode == 0)
                    {
                        if (zgl < 80 && count > 0)
                        {
                            data.Add(new
                            {
                                id = dr["departmentid"].ToString(),
                                code = dr[0].ToString(),
                                name = dr[1].ToString(),
                                num = zgl
                            });
                        }
                    }
                    if (mode == 1)
                    {
                        if (count > 0)
                        {
                            data.Add(new
                            {
                                id = dr["departmentid"].ToString(),
                                code = dr[0].ToString(),
                                name = dr[1].ToString(),
                                num = zgl
                            });
                        }
                    }
                }
                #endregion
            }
            else
            {
                //存在重大隐患的电厂  一级风险超过3个的电厂
                DataTable dt = desktop.GetHtOrRiskItems(user, mode);
                foreach (DataRow dr in dt.Rows)
                {
                    data.Add(new
                    {
                        id = dr["departmentid"].ToString(),
                        code = dr["deptcode"].ToString(),
                        name = dr["departmentname"].ToString(),
                        num = dr["nums"].ToString()
                    });
                }
            }

            return Success("获取数据成功", data);
        }
        #endregion

        #region 获取当前用户下所有的电厂(省级)
        [HttpGet]
        public ActionResult GetFactoryData()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var data = new DepartmentBLL().GetAllFactory(user);
            return Success("获取数据成功", data);
        }
        #endregion

          /// <summary>
        /// 实时监控数据统计（华电新疆省公司）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetItemsForTeam()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId=user.UserId});
                string url = new DataItemDetailBLL().GetItemValue("bzAppUrl") + "/GetAdminTodos";
                string content = wc.UploadString(new Uri(url), "POST", data);
                return content;
            }
            catch (Exception ex)
            {
                Logger.Error(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：获取班组待办事项异常：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                //将同步结果写入日志文件
                //string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                //System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：获取班组待办事项异常：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex) + "\r\n");
                return "{Success:false}";
            }
        }
        /// <summary>
        /// 隐患月度趋势（广西华昇）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHTStat()
        {

            return Success("删除成功。");
        }
        /// <summary>
        /// 隐患月度趋势（广西华昇）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMonthHTStat()
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DepartmentBLL deptBll = new DepartmentBLL();
                List<decimal> lstCheck = new List<decimal>();
                List<decimal> lstHt = new List<decimal>();
                List<decimal> lstWz = new List<decimal>();
                List<decimal> lsthtzgl = new List<decimal>();
                List<decimal> lstwzzgl = new List<decimal>();
                List<string> lstMonths = new List<string>();
                string orgCode = user.OrganizeCode;
                for (int j = 1; j <= 12; j++)
                {
                    string month = j < 10 ? "0" + j.ToString() : j.ToString();
                    string time = DateTime.Now.Year.ToString() + "-" + month;
                    //安全检查
                    string sql = string.Format("select count(1) from BIS_SAFTYCHECKDATARECORD t where datatype in(0,2) and t.belongdept like '{0}%' and to_char(checkbegintime,'yyyy-mm')='{1}'", orgCode, time);
                    decimal count = deptBll.GetDataTable(sql).Rows[0][0].ToDecimal();
                    lstCheck.Add(count);
                    //发现隐患
                    sql = string.Format(@"select  count(a.id) from bis_htbaseinfo a where a.workstream !='隐患登记' and a.workstream !='隐患评估'  
                                                                    and a.workstream !='隐患申诉' and a.workstream !='隐患完善' and a.workstream !='制定整改计划'  and a.createuserdeptcode like '{0}%' and to_char(checkdate,'yyyy-mm')='{1}'", orgCode, time);
                    count = deptBll.GetDataTable(sql).Rows[0][0].ToDecimal();
                    lstHt.Add(count);
                    //发现违章
                    sql = string.Format(@"select count(1) from v_lllegalbaseinfo  a where  to_char(lllegaltime,'yyyy-mm')='{1}' and a.createuserorgcode='{0}'", orgCode, time);
                    count = deptBll.GetDataTable(sql).Rows[0][0].ToDecimal();
                    lstWz.Add(count);
                    sql = string.Format(@"select count(1) from v_lllegalbaseinfo  a where  to_char(lllegaltime,'yyyy-mm')='{1}' and a.createuserorgcode='{0}'", orgCode, time);
                    count = deptBll.GetDataTable(sql).Rows[0][0].ToDecimal();
                    //隐患整改率
                  
                    sql = string.Format(@"select count(1) from  bis_htbaseinfo a left join v_htchangeinfo b on a.hidcode = b.hidcode where  a.workstream='整改结束' and  b.changedutydepartcode like '{0}%' and to_char(changefinishdate,'yyyy-mm')='{1}' and to_char(checkdate,'yyyy-mm')='{1}'
 union all select count(1) from  bis_htbaseinfo a left join v_htchangeinfo b on a.hidcode = b.hidcode where a.createuserdeptcode like '{0}%' and a.workstream !='隐患登记' and a.workstream !='隐患评估'  and a.workstream !='隐患申诉' and a.workstream !='隐患完善' and a.workstream !='制定整改计划' and to_char(checkdate,'yyyy-mm')='{1}'", orgCode, time);
                    DataTable dt=deptBll.GetDataTable(sql);
                    decimal sum=dt.Rows[1][0].ToDecimal();
                    count= dt.Rows[0][0].ToDecimal();
                    count = sum == 0 ? 0 : Math.Round(count / sum, 2) * 100;
                    lsthtzgl.Add(count);
                    //违章整改率
                    sql = string.Format(@"select count(1) from bis_lllegalregister a left join v_lllegalreforminfo b on a.id = b.lllegalid  where a.flowstate='流程结束' and  b.reformdeptcode like '{0}%' and to_char(reformfinishdate,'yyyy-mm')='{1}' and  to_char(lllegaltime,'yyyy-mm')='{1}'
                                          union all
                                         select count(1) from bis_lllegalregister a left join v_lllegalreforminfo b on a.id = b.lllegalid where a.createuserdeptcode like '{0}%' and a.flowstate in (select itemname from v_yesqrwzstatus) and to_char(lllegaltime,'yyyy-mm')='{1}'", orgCode, time);
                    dt = deptBll.GetDataTable(sql);
                    sum = dt.Rows[0][0].ToDecimal();
                    count = dt.Rows[1][0].ToDecimal();
                    count = sum == 0 ? 0 : Math.Round(count / sum, 2) * 100;
                    lstwzzgl.Add(count);
                    lstMonths.Add(j+"月");
                }
                return Success("删除成功。", new { jc = lstCheck, yhfx = lstHt, wzfx = lstWz, yhzgl = lsthtzgl, wzzgl = lstwzzgl, months = lstMonths });
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 实时工作（广西华昇）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRealTimeWork()
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                List<RealTimeWorkModel> list=desktop.GetRealTimeWork(user);
                list = list.OrderByDescending(t=>t.Time).ToList();
                List<RealTimeWorkModel> newList = list.Take(6).ToList();
                return Success("获取数据成功。", newList);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 预警中心（广西华昇）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getWarn()
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                List<RealTimeWorkModel> list = desktop.GetWarningCenterWork(user);
                list = list.OrderByDescending(t => t.Time).ToList();
                List<RealTimeWorkModel> newList = list.Take(6).ToList();
                return Success("获取数据成功。", new { data = newList, count = list.Count });
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取监督检查-未整改隐患
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult GetNoChangeHidList(string code = "")
        {
            var curUser = new OperatorProvider().Current(); //当前用户

            if (string.IsNullOrEmpty(code))
            {
                code = curUser.OrganizeCode;
            }
            HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
            var dt = htbaseinfobll.GetNoChangeHidList(code);

            return Success("获取数据成功", dt);
        }

        #region 违章曝光治理
        /// <summary>
        /// 违章曝光
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public ActionResult QueryExposureLllegal(string num)
        {
            Operator curUser = new OperatorProvider().Current();

            var data = new LllegalRegisterBLL().QueryExposureLllegal(num); 

            return Content(data.ToJson());
        }
        #endregion


        /// <summary>
        /// 获取首页内容
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeskTotalByCheckType(string itemcode)
        {
            try
            {
                var data = new FivesafetycheckBLL().DeskTotalByCheckType(itemcode);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
    }
    public class Assess
    {
        public string Grade;
        public string GradeCode;
        public string AreaCode;
        public int Count;
    }
}
