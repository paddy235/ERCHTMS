using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.Desktop;
using ERCHTMS.IService.Desktop;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using System.Linq;
using System.Collections;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Busines.SafeReward;
using ERCHTMS.Busines.SafePunish;
using ERCHTMS.Busines.ComprehensiveManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PowerPlantInside;
using FrameWork.Extension;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Busines.WorkPlan;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Entity.Home;
using ERCHTMS.Busines.SafetyWorkSupervise;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Entity.CarManage;
using Newtonsoft.Json;
using ERCHTMS.Entity.Common;

namespace ERCHTMS.Busines.Desktop
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public class DesktopBLL
    {
        private DesktopIService service = new DesktopService();


        #region  通用版本的领导驾驶舱(电厂层级)

        #region  预警指标、安全指标
        /// <summary>
        /// 预警指标、安全指标
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DesktopPageIndex> GetPowerPlantWarningIndex(Operator user)
        {
            //通用厂级领导驾驶舱指标
            List<DesktopPageIndex> list = service.GetPowerPlantWarningIndex(user);

            #region 外包流程管理
            var data = new OutprojectblacklistBLL().ToAuditOutPeoject(user);
            var data1 = new OutprojectblacklistBLL().ToIndexData(user);
            //data:  0 单位资质审查待审核(查) , 1 人员审查待审核 , 2 三措两案待审核 ,3 特种设备,4 普通设备, 5 入场许可,6 开工申请待审核(查);
            //data1: 0 保证金 ,1 合同 ,2 协议 ,3 安全技术交底, 4 开(收)工会,5 安全评价
            #region 外包流程管理

            DesktopPageIndex WBWF001 = new DesktopPageIndex();
            WBWF001.code = "WBWF001";
            WBWF001.name = "合同协议";
            WBWF001.modulecode = "WBWFMG-01";
            WBWF001.modulename = "外包工程流程";
            WBWF001.count = data1[1].ToString(); // 合同
            WBWF001.isdecimal = false;
            list.Add(WBWF001);

            DesktopPageIndex WBWF002 = new DesktopPageIndex();
            WBWF002.code = "WBWF002";
            WBWF002.name = "合同协议";
            WBWF002.modulecode = "WBWFMG-01";
            WBWF002.modulename = "外包工程流程";
            WBWF002.count = data1[2].ToString(); // 协议
            WBWF002.isdecimal = false;
            list.Add(WBWF002);

            DesktopPageIndex WBWF003 = new DesktopPageIndex();
            WBWF003.code = "WBWF003";
            WBWF003.name = "单位资质审查";
            WBWF003.modulecode = "WBWFMG-01";
            WBWF003.modulename = "外包工程流程";
            WBWF003.count = data[0].ToString(); // 单位资质审查待审核
            WBWF003.isdecimal = false;
            list.Add(WBWF003);

            DesktopPageIndex WBWF004 = new DesktopPageIndex();
            WBWF004.code = "WBWF004";
            WBWF004.name = "人员资质审查";
            WBWF004.modulecode = "WBWFMG-01";
            WBWF004.modulename = "外包工程流程";
            WBWF004.count = data[1].ToString(); // 人员审查待审核
            WBWF004.isdecimal = false;
            list.Add(WBWF004);

            DesktopPageIndex WBWF005 = new DesktopPageIndex();
            WBWF005.code = "WBWF005";
            WBWF005.name = "保证金";
            WBWF005.modulecode = "WBWFMG-01";
            WBWF005.modulename = "外包工程流程";
            WBWF005.count = data1[0].ToString(); // 保证金
            WBWF005.isdecimal = false;
            list.Add(WBWF005);


            DesktopPageIndex WBWF006 = new DesktopPageIndex();
            WBWF006.code = "WBWF006";
            WBWF006.name = "安全技术交底";
            WBWF006.modulecode = "WBWFMG-01";
            WBWF006.modulename = "外包工程流程";
            WBWF006.count = data1[3].ToString(); // 安全技术交底
            WBWF006.isdecimal = false;
            list.Add(WBWF006);


            DesktopPageIndex WBWF007 = new DesktopPageIndex();
            WBWF007.code = "WBWF007";
            WBWF007.name = "三措两案";
            WBWF007.modulecode = "WBWFMG-01";
            WBWF007.modulename = "外包工程流程";
            WBWF007.count = data[2].ToString(); // 三措两案
            WBWF007.isdecimal = false;
            list.Add(WBWF007);

            DesktopPageIndex WBWF008 = new DesktopPageIndex();
            WBWF008.code = "WBWF008";
            WBWF008.name = "工器具验收";
            WBWF008.modulecode = "WBWFMG-01";
            WBWF008.modulename = "外包工程流程";
            WBWF008.count = data[3].ToString(); // 工器具验收-特种设备验收
            WBWF008.isdecimal = false;
            list.Add(WBWF008);

            DesktopPageIndex WBWF009 = new DesktopPageIndex();
            WBWF009.code = "WBWF009";
            WBWF009.name = "工器具验收";
            WBWF009.modulecode = "WBWFMG-01";
            WBWF009.modulename = "外包工程流程";
            WBWF009.count = data[4].ToString(); // 工器具验收-普通设备验收
            WBWF009.isdecimal = false;
            list.Add(WBWF009);


            DesktopPageIndex WBWF010 = new DesktopPageIndex();
            WBWF010.code = "WBWF010";
            WBWF010.name = "入场许可";
            WBWF010.modulecode = "WBWFMG-01";
            WBWF010.modulename = "外包工程流程";
            WBWF010.count = data[5].ToString(); // 入场许可
            WBWF010.isdecimal = false;
            list.Add(WBWF010);

            DesktopPageIndex WBWF011 = new DesktopPageIndex();
            WBWF011.code = "WBWF011";
            WBWF011.name = "开工申请";
            WBWF011.modulecode = "WBWFMG-01";
            WBWF011.modulename = "外包工程流程";
            WBWF011.count = data[6].ToString(); // 开工申请
            WBWF011.isdecimal = false;
            list.Add(WBWF011);

            DesktopPageIndex WBWF012 = new DesktopPageIndex();
            WBWF012.code = "WBWF012";
            WBWF012.name = "开(收)工会";
            WBWF012.modulecode = "WBWFMG-01";
            WBWF012.modulename = "外包工程流程";
            WBWF012.count = data1[4].ToString(); // 开(收)工会
            WBWF012.isdecimal = false;
            list.Add(WBWF012);


            DesktopPageIndex WBWF013 = new DesktopPageIndex();
            WBWF013.code = "WBWF013";
            WBWF013.name = "安全评价";
            WBWF013.modulecode = "WBWFMG-01";
            WBWF013.modulename = "外包工程流程";
            WBWF013.count = data1[5].ToString(); //安全评价
            WBWF013.isdecimal = false;
            list.Add(WBWF013);

            #endregion
            #endregion

            return list;
        }
        #endregion

        #region 获取整改率低于多少的电厂数据
        /// <summary>
        /// 获取整改率低于多少的电厂数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rankname"></param>
        /// <returns></returns>
        public DataTable GetRectificationRateUnderHowMany(Operator user, string rankname, decimal num)
        {
            try
            {
                return service.GetRectificationRateUnderHowMany(user, rankname, num);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region  未闭环隐患统计
        /// <summary>
        /// 未闭环隐患统计
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetNoCloseLoopHidStatistics(Operator user, int mode)
        {
            return service.GetNoCloseLoopHidStatistics(user, mode);
        }

        #region 隐患整改率
        /// <summary>
        /// 隐患整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetHiddenChangeForLeaderCockpit(Operator user)
        {
            return service.GetHiddenChangeForLeaderCockpit(user);
        }
        #endregion

        #region 各部门未闭环违章统计
        /// <summary>
        ///各部门未闭环违章统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public DataTable GetNoCloseLoopLllegalStatistics(Operator user)
        {
            return service.GetNoCloseLoopLllegalStatistics(user);
        }
        #endregion

        #region 各部门违章整改率统计
        /// <summary>
        /// 各部门违章整改率统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetLllegalChangeForLeaderCockpit(Operator user)
        {
            return service.GetLllegalChangeForLeaderCockpit(user);
        }
        #endregion

        #endregion

        #region 今日作业风险/高风险作业统计
        /// <summary>
        /// 今日作业风险/高风险作业统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetHighRiskWorkingForLeaderCockpit(Operator user, int mode)
        {
            return service.GetHighRiskWorkingForLeaderCockpit(user, mode);
        }
        #endregion

        #region  厂级领导驾驶舱数据设置(通用版本)
        /// <summary>
        /// 厂级领导驾驶舱数据设置(通用版本)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemType"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetDataSetForCommon(ERCHTMS.Code.Operator user, string itemType, int mode = 0)
        {
            DataTable dt = GetDeptDataSet(user, itemType);

            if (dt.Rows.Count > 0)
            {
                List<DesktopPageIndex> list = itemType == "SSJK" ? GetPowerPlantWarningIndex(user) : null;
                dt.Columns.Add("Num"); //返回数据
                dt.Columns.Add("Code");//项目代码
                dt.Columns.Add("TypeCode");//大类代码
                foreach (DataRow dr in dt.Rows)
                {
                    var entity = list.Where(p => p.code == dr["itemcode"].ToString()).FirstOrDefault();
                    if (null != entity)
                    {
                        dr["Num"] = entity.count;
                        dr["Code"] = entity.code;
                        dr["TypeCode"] = entity.modulecode;
                        if (!string.IsNullOrWhiteSpace(dr["callback"].ToString()))
                        {
                            dr["callback"] = dr["callback"].ToString().Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Url}", dr["address"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(dr["itemstyle"].ToString()))
                        {
                            dr["itemstyle"] = dr["itemstyle"].ToString().Replace("{Callback}", dr["Callback"].ToString()).Replace("{Icon}", dr["Icon"].ToString()).Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Num}", dr["Num"].ToString()).Replace("{Url}", dr["address"].ToString()).Replace("{Code}", entity.code);
                        }
                    }
                }
            }
            return dt;
        }
        #endregion


        #endregion

        public string GetWorkTypeChart(ERCHTMS.Code.Operator user)
        {
            return service.GetWorkTypeChart(user);
        }
        public DataTable GetFactoryCheckListForGroup(Operator user)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select encode deptcode ,fullname  deptname,count(1) num from base_department d
 left join BIS_SAFTYCHECKDATARECORD r on d.departmentid=r.CheckedDepartID where 
 datatype=0 and  nature='厂级' and deptcode like '{0}%'   group by encode,fullname", user.OrganizeCode));
            return dt;
        }

        /// <summary>
        /// 是否是行业通用版
        /// </summary>
        /// <returns></returns>
        public bool IsGeneric()
        {
            string industry = OperatorProvider.Provider.Current().Industry;
            if (industry.IsNullOrEmpty() || industry == "电力") //如果为空 或者为电力 则为电力双控 其余的则为通用双控
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region 获取隐患或者风险的统计项目
        /// <summary>
        /// 获取隐患或者风险的统计项目
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetHtOrRiskItems(ERCHTMS.Code.Operator user, int mode)
        {
            return service.GetHtOrRiskItems(user, mode);
        }
        #endregion

        /// <summary>
        /// 获取日常安全检查统计数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetSafetyCheckOfEveryDay(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyCheckOfEveryDay(user);
        }
        /// <summary>
        /// 省级查看安全检查
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetCheckListForGroup(ERCHTMS.Code.Operator user)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select encode deptcode ,fullname  deptname,count(1) from base_department d
 left join BIS_SAFTYCHECKDATARECORD r on d.departmentid=r.CheckedDepartID where 
 datatype=0 and  nature='厂级' and deptcode like '{0}%'   group by encode,fullname", user.OrganizeCode));
            return dt;
        }



        /// <summary>
        /// 首页实时监控数据指标
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Hashtable GetMonitorData(ERCHTMS.Code.Operator user, string itemType = "SSJK")
        {
            List<string> lstItems = GetDeptDataSet(user, itemType).AsEnumerable().Select(t => t.Field<string>("itemcode")).ToList();
            Hashtable data = new Hashtable();
            HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
            var dt = htbaseinfobll.QueryHidWorkList(user);
            decimal zdnum = 0;
            foreach (DataRow rows in dt.Rows) 
            {
                string itemvalue = rows["itemvalue"].ToString();
                if (itemvalue.Contains("重大隐患")) 
                {
                    zdnum += Convert.ToDecimal(rows["total"].ToString());
                }
            }
            data.Add("HTZDYH", zdnum);//重大隐患数
            data.Add("HTYZG", decimal.Parse(dt.Rows[0][3].ToString()));//已整改隐患数
            data.Add("HTWZG", decimal.Parse(dt.Rows[0][4].ToString()));//未整改隐患数
            data.Add("HTYQWZG", decimal.Parse(dt.Rows[0][5].ToString()));//逾期未整改隐患数
            data.Add("HTZGL", decimal.Parse(dt.Rows[0][6].ToString()));//隐患整改率
            List<int> list = GetHtNum(user);
            if (itemType == "AQZB")
            {
                data.Add("HTSUM", list[0]);//隐患数
            }
            else
            {
                data.Add("HTSUM", decimal.Parse(dt.Rows[0][2].ToString()));//隐患数
            }
            data.Add("HTYQZGL", list[3]);//整改延期隐患数

            if (lstItems.Contains("DSSUM"))
            {
                list = GetDangerSourceNum(user);
                data.Add("DSSUM", list[0]);//危险源总数
                data.Add("DSZD", list[1]);//重大危险源数
            }
           
            if(lstItems.Contains("SGSUM"))
            {
                list = GetAccidentNum(user);
                data.Add("SGSUM", list[0]);//事故起数
                data.Add("SGSW", list[1]);//事故死亡人数
                data.Add("SGZS", list[2]);//事故重伤人数
            }
            SaftyCheckDataBLL saftycheckdatabll = new SaftyCheckDataBLL();
            var dtCheck = saftycheckdatabll.GetCheckStat(user);
            data.Add("CKSUM", decimal.Parse(dtCheck.Rows[0][0].ToString()));//安全检查次数
            //日常安全检查次数
            string rcNum = GetSafetyCheckOfEveryDay(user);
            data.Add("CKRC", int.Parse(rcNum));//日常检查次数
            data.Add("CHFXHT", decimal.Parse(GetCheckHtNum(user).ToString()));//检查发现隐患数

            list = GetWBProjectNum(user);
            data.Add("WBZJSUM", list[0]);//在建外包工程数
            data.Add("WBZCRS", list[1]);//外包工程在场人数
            data.Add("WBGCSUM", list[2]);//外包工程总数
            data.Add("WBDW", list[3]);//在场外包单位数
            data.Add("WBBYXJ", list[4]);//本月新进外包人员
            data.Add("WBWZNUM", list[5]);//外包单位违章次数



            list = GetRiskNum(user);
            data.Add("FXSUM", list[0]);//风险总数
            data.Add("FXZD", list[1]);//重大风险数

                list = GetWorkNum(user);
                data.Add("ZYSUM", list[3]);//高风险作业总数
                data.Add("ZYJX", list[4]);//正在进行的高风险作业数
                data.Add("ZYJR", list[7]);//今日高风险作业
            if(lstItems.Contains("JRGWZYZS"))
            {
                data.Add("JRGWZYZS", GetTodayWorkForDangerJob()); //今日高危作业
            }

            if (lstItems.Contains("SCHSUM"))
            {
                list = GetSafetyCheckForGroup(user);
                data.Add("SCHSUM", list[0]);//省公司安全检查次数
                data.Add("SCHRW", list[2]);//省公司下发给下属电厂的检查任务数
            }

            if (lstItems.Contains("SCHFXHT"))
            {
                list = GetHtForGroup(user);
                data.Add("SCHFXHT", list[3]);//省公司发现隐患数
                data.Add("SHTWWCZDYH", list[4]);//未治理完成重大隐患数
                data.Add("SHTYQWZG", list[2]); //逾期未整改隐患数
            }

            //if (lstItems.Contains("SHTZGL"))
            //{
                List<decimal> list1 = GetWarnItems(user);
                data.Add("SHTZGL", list1[2]);//隐患整改率
                data.Add("SHT80", list1[1]); //隐患整改率低于80%的电厂
                data.Add("CZZDYHDC", list1[3]); //存在重大隐患的电厂
            //}

            var datas = new HTBaseInfoBLL().QueryHidBacklogRecord("10", user.UserId);
            if (datas.Rows.Count == 2)
            {
                if (datas.Rows[0]["serialnumber"].ToString() == "1")
                {
                    int uploadHtNum = int.Parse(datas.Rows[0]["pnum"].ToString());
                    data.Add("HTWSC", uploadHtNum); //我上传的隐患
                }

            }
            if (lstItems.Contains("YJFXCGSGDC"))
            {
                var fxlist = GetRiskAnalyzeItems(user);
                data.Add("YJFXCGSGDC", fxlist[0]); //一级风险超过3个的电厂
            }
            list = GetlllegalNum(user);//违章信息
            data.Add("WZSUM", list[0]);//违章数量
            data.Add("WZWZG", list[5]);//未整改的违章
            data.Add("WZYQ", list[4]);//逾期未整改违章数量
            if (lstItems.Contains("WZZGL"))
            {
                data.Add("WZZGL", GetlllegalRatio(user));//违章整改率
            }

            int count=0;
            if (lstItems.Contains("SBTZ"))
            {
                count = GetEquimentNum(user);//特种设备数量
                data.Add("SBTZ", count);
            }
            if (lstItems.Contains("WDGC"))
            {
                count = GetProjectNum(user);//施工中的危大工程数量
                data.Add("WDGC", count);
            }

            if (lstItems.Contains("JRLSGC"))
            {
                count = new WorkMeetingBLL().GetTodayTempProject(user);//今日临时外包工程
                data.Add("JRLSGC", count);
            }
            if(lstItems.Contains("ZCWBNUM"))
            {
                count = GetWBUsersCount(user);//在厂外包人数数量(西塞山)
                data.Add("ZCWBNUM", count);
            }
            return data;
        }
        /// <summary>
        ///获取在厂外包人数数量(西塞山)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetWBUsersCount(ERCHTMS.Code.Operator user)
        {
            string softName = BSFramework.Util.Config.GetValue("SoftName").ToLower() ;
            int count = 0;
            if (softName.StartsWith("xss"))
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtCards = deptBll.GetDataTable(string.Format("select distinct idcard from XSS_ENTERRECORD where remark='0' and idcard in(select identifyid from base_user where isepiboly='1')"));
                List<string> list = new List<string>();
                foreach (DataRow dr in dtCards.Rows)
                {
                    DataTable dtRows = deptBll.GetDataTable(string.Format("select *from (select remark from XSS_ENTERRECORD where idcard='{0}' order by time desc) where rownum=1", dr[0].ToString()));
                    if (dtRows.Rows.Count>0)
                    {
                        if (dtRows.Rows[0][0].ToString() == "0")
                        {
                            if (!list.Contains(dr[0].ToString()))
                            {
                                count++;
                                list.Add(dr[0].ToString());
                            }

                        }
                    }
                   
                }
            }
            return count;
        }
        /// <summary>
        /// 待分配的检查任务
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetAllotCheckCount(ERCHTMS.Code.Operator user)
        {
            return service.GetAllotCheckCount(user);
        }
        /// <summary>
        /// 首页待办事项统计指标
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Hashtable GetWorkData(ERCHTMS.Code.Operator user, int mode = 0)
        {
            List<string> arrItems = GetDeptDataSet(user, "DBSX").AsEnumerable().Select(t=>t.Field<string>("itemcode")).ToList();

            Hashtable data = new Hashtable();
            string execCheckNum = "0"; //待执行的安全检查数
            string changeplanHiddenNum = "0"; //待制定整改计划
            string perfectionHiddenNum = "0";//待完善隐患数
            string approvalHiddenNum = "0";//待评估隐患数
            string reformHiddenNum = "0";//待整改隐患数
            string delayHiddenNum = "0";//待审(核)批整改延期隐患数
            string reviewHiddenNum = "0";//待验收的隐患数
            string recheckHiddenNum = "0"; //待复查验证的隐患数
            string assessHiddenNum = "0";//待整改效果评估隐患数
            string dailyexamineNum = "0";//待审核日常考核
            int changJobNum = 0;//人员转岗待审核
            string planNum = "0";//进行中的风险评估计划数
            string accidenteventNum = "0";//待审核事故事件
            int planApplyBMNum = 0;//待审核部门工作计划数量
            int planApplyGRNum = 0;//待审核个人工作计划数量
            string lifthoistNum = "0";//待审核起重吊装作业
            int FeedbackNum = 0;//待反馈数据
            int ConfirmationNum = 0;//待督办确认数据
            RiskPlanBLL planBll = new RiskPlanBLL();
         
            SaftyCheckDataBLL saftbll = new SaftyCheckDataBLL();
           

            //隐患待办事项
            var dtHt = new HTBaseInfoBLL().QueryHidBacklogRecord(mode.ToString(), user.UserId);
            if (dtHt.Rows.Count == 8)
            {
                approvalHiddenNum = dtHt.Rows[0]["pnum"].ToString();//待评估隐患数
                perfectionHiddenNum = dtHt.Rows[5]["pnum"].ToString();//待完善隐患数
                reformHiddenNum = dtHt.Rows[1]["pnum"].ToString();//待整改隐患数
                delayHiddenNum = dtHt.Rows[2]["pnum"].ToString();//待审(核)批整改延期隐患数
                reviewHiddenNum = dtHt.Rows[3]["pnum"].ToString();//待验收的隐患数
                recheckHiddenNum = dtHt.Rows[6]["pnum"].ToString();//待复查验证隐患数
                assessHiddenNum = dtHt.Rows[4]["pnum"].ToString();//待整改效果评估隐患数
                changeplanHiddenNum = dtHt.Rows[7]["pnum"].ToString(); //待制定整改计划

                data.Add("HTPG", approvalHiddenNum);
                data.Add("HTDZG", reformHiddenNum);
                data.Add("HTYQZG", delayHiddenNum);
                data.Add("HTYS", reviewHiddenNum);
                data.Add("HTFCYZ", recheckHiddenNum);
                data.Add("HTXGPG", assessHiddenNum);
                data.Add("HTWS", perfectionHiddenNum);
                data.Add("HTZDZGJH", changeplanHiddenNum);
            }
            //风险辨识评估
            if (arrItems.Contains("FXBSJH"))
            {
                planNum = planBll.GetPlanCount(user, mode).ToString();
                data.Add("FXBSJH", planNum);
            }
            //安全检查
            if (arrItems.Contains("CKPLAN"))
            {
                int[] countcheck = saftbll.GetCheckCount(user, mode);
                execCheckNum = countcheck.Sum() + "," + countcheck[0] + "," + countcheck[1] + "," + countcheck[2] + "," + countcheck[3] + "," + countcheck[4];
                data.Add("CKPLAN", execCheckNum);
            }
          
          

            //日常考核
            if (arrItems.Contains("RCKHDSH"))
            {
                DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
                dailyexamineNum = dailyexaminebll.CountIndex(user).ToString();
                data.Add("RCKHDSH", dailyexamineNum);
            }
            //外包工程管控
            if (arrItems.Contains("WBDWZZ"))
            {
                var wb = new OutprojectblacklistBLL().ToAuditOutPeoject(user);
                data.Add("WBDWZZ", wb[0]);//单位资质
                data.Add("WBRYZZ", wb[1]);//人员资质
                data.Add("WBSCLA", wb[2]);//三措两案
                data.Add("WBDDSB", wb[4]);//电动设备
                data.Add("WBTZSB", wb[3]);//特种设备
                data.Add("WBRCSQ", wb[5]);//入场许可
                data.Add("WBKGSQ", wb[6]);//开工申请
                data.Add("DSHAQJSJD", wb[7]);//安全技术交底
            }
            var work =GetWorkNum(user);
            data.Add("ZYTY", work[1] + work[0]);//高风险通用待审核(批)作业
            data.Add("ZYJD", work[2]);//高风险待监督
            List<int> list = new List<int>();
            if (arrItems.Contains("ZYJSJ"))
            {
                list = GetScaffoldNum(user);
                data.Add("ZYJSJ", list[1]);//脚手架待审核
            }
            if (arrItems.Contains("ZYSSBD"))
            {
                data.Add("ZYSSBD", GetSafetyChangeNum(user));//安全设施变动审待核数量
            }

            if (arrItems.Contains("ZYXFS"))
            {
                list =GetFireWaterNum(user);
                data.Add("ZYXFS", list[0]);//消防水待审核
            }

            list = GetlllegalNum(user);//违章
            data.Add("WZHZ", list[1]);//待核准违章
            data.Add("WZDZG", list[2]);//待整改违章
            data.Add("WZDYS", list[3]);//待验收违章
            data.Add("WZDWS", list[6]);//待完善违章
            data.Add("WZDZDZGJH", list[7]);//待验收确认违章
            data.Add("WZDYSQR", list[8]);//待验收确认违章
            data.Add("WZDZGYQSP", list[9]);//待整改延期申请审批
            data.Add("WZZGQR", list[10]);//违章整改确认
            data.Add("WZDAKFDWS", list[11]);//违章档案扣分待完善

            list = GetJobSafetyCardNum(user);
            data.Add("DSPGCZY", list[0]);//待审批高处作业
            data.Add("DSPQZDZZY", list[1]);//待审批起重吊装
            data.Add("DSPDTZY", list[2]);//待审批动土作业
            data.Add("DSPDLZY", list[3]);//待审批断路作业
            data.Add("DSPDHZY", list[4]);//待审批动火作业
            data.Add("DSPMBCDZY", list[5]);//待审批盲板抽堵作业
            data.Add("DSPSXKJZY", list[6]);//待审批受限空间
            data.Add("DSPSBJXQLZY", list[7]);//待审批设备检修清理作业
            data.Add("DCSQRZYAQZ", list[8]);//待措施确认作业安全证
            data.Add("DTDZYAQZ", list[9]);//待停电作业安全证
            data.Add("DBAZYAQZ", list[10]);//待备案作业安全证
            data.Add("DYSZYAQZ", list[11]);//待验收作业安全证
            data.Add("DSDZYAQZ", list[12]);//待送电作业安全证

            if (arrItems.Contains("YJYLJLWS"))
            {
                list = GetDrillRecordNum(user);
                data.Add("YJYLJLWS", list[0]);//待完善应急演练记录
            }
            //问题流程
            if (arrItems.Contains("WTDZG"))
            {
              
                list = GetQuestionNum(user);//问题
                data.Add("WTDZG", list[0]);//待整改问题
                data.Add("WTDYZ", list[1]);//待验证问题
                data.Add("FXWTDPG", list[2]);//待评估的发现问题
            }
            //华润标准
            if (arrItems.Contains("BZSQ"))
            {
                StandardApplyBLL sa = new StandardApplyBLL();//标准修（订）审核发布
                data.Add("BZSQ", sa.CountIndex("1"));//需重新申请的标准修（订）
                data.Add("BZDSH", sa.CountIndex("2"));//待审核（批）标准修（订）
                data.Add("BZDSFP", sa.CountIndex("3"));//待分配的标准修（订）
            }
            //EHS计划
            if (arrItems.Contains("EHSPLAN"))
            {
                int countInfo = new ERCHTMS.Busines.ComprehensiveManage.InfoSubmitBLL().CountIndex("1");
                data.Add("EHSPLAN", countInfo);//待上报EHS信息
            }
            //安全奖励
            if (arrItems.Contains("AQJL"))
            {
                string count = new SaferewardBLL().GetRewardNum();//待审核的安全奖励
                data.Add("AQJL", count);
            }
            //安全惩罚
            if (arrItems.Contains("AQCF"))
            {
                string count = new SafepunishBLL().GetPunishNum();//待审核的安全惩罚
                data.Add("AQCF", count);
            }
            //华润NOSA
            if (arrItems.Contains("NOSAUPLOAD"))
            {
                var nosaworkbll = new NosaManage.NosaworksBLL();
                int countNOSA = nosaworkbll.CountIndex("1");//待上传的NOSA工作清单
                data.Add("NOSAUPLOAD", countNOSA);
            }
            if (arrItems.Contains("NOSACHECK"))
            {
                var nosaworkbll = new NosaManage.NosaworksBLL();
                int countNOSA = nosaworkbll.CountIndex("3"); //待审核的NOSA工作清单
                data.Add("NOSACHECK", countNOSA);
            }
            //人员转岗
            if (arrItems.Contains("TransferNum"))
            {
                changJobNum = new TransferBLL().GetTransferNum();//人员转岗待审核
                data.Add("TransferNum", changJobNum);
            }
            //事故事件
            if (arrItems.Contains("AccidentEventNum"))
            {
                accidenteventNum = new PowerplantinsideBLL().GetAccidentEventNum();//待审核事故事件
                data.Add("AccidentEventNum", accidenteventNum);
            }

            if (arrItems.Contains("DSHSGSJCL"))
            {
                var sgsj = new PowerplanthandleBLL().ToAuditPowerHandle();//事故事件处理待办
                data.Add("DSHSGSJCL", sgsj[0]);
                data.Add("DZGSGSJ", sgsj[1]);
                data.Add("DQSSGSJCL", sgsj[2]);
                data.Add("DYSSGSJ", sgsj[3]);
            }

            if (arrItems.Contains("QZDZZY"))
            {
                lifthoistNum = new LifthoistjobBLL().GetLifthoistjobNum();//待审核起重吊装
                data.Add("QZDZZY", lifthoistNum);
            }
            if (arrItems.Contains("AllotCheckNum"))
            {
                data.Add("AllotCheckNum", GetAllotCheckCount(user));//待分配或完善的检查数量
            }

            if (arrItems.Contains("YJYLJL"))
            {
                var evaluatenum = new DrillplanrecordBLL().GetDrillPlanRecordEvaluateNum(user);
                data.Add("YJYLJL", evaluatenum);
            }
            if (arrItems.Contains("YJYLJLPG"))
            {
                var assessnum = new DrillplanrecordBLL().GetDrillPlanRecordAssessNum(user);
                data.Add("YJYLJLPG", assessnum);
            }
            if (arrItems.Contains("WHPLY"))
            {
                //获取待审批的危化品领用
                data.Add("WHPLY", GetWhplyCount(user, "XLD_DANGEROUSCHEMICALRECEIVE"));
            }
            if (arrItems.Contains("WHPBF"))
            {
                //获取待审批的危化品报废
                data.Add("WHPBF", GetCheckCount(user, "XLD_DANGEROUSCHEMICALSCRAP"));
            }

            if (arrItems.Contains("BMGZJH"))
            {
                //综合信息管理
                planApplyBMNum = new PlanApplyBLL().GetPlanApplyBMNum(user);//待审核部门工作计划
                data.Add("BMGZJH", planApplyBMNum);
            }
            if (arrItems.Contains("GRGZJH"))
            {
                planApplyGRNum = new PlanApplyBLL().GetPlanApplyGRNum(user);//待审核个人工作计划
                data.Add("GRGZJH", planApplyGRNum);
            }
            if (arrItems.Contains("SZRSH"))
            {
                //获取待审核的三种人数据
                data.Add("SZRSH", GetThreeCount(user).Count);
            }
            if (arrItems.Contains("AqdtCount"))
            {
                //获取待审批的安全动态
                data.Add("AqdtCount", GetAqdtCount(user));
            }
            if (arrItems.Contains("DFKSL"))
            {
                FeedbackNum = new SafetyworksuperviseBLL().GetSuperviseNum(user.UserId, "1");//待反馈数据
                data.Add("DFKSL", FeedbackNum);
            }
            if (arrItems.Contains("DDBSJ"))
            {
                ConfirmationNum = new SafetyworksuperviseBLL().GetSuperviseNum(user.UserId, "2");//待督办确认数据
                data.Add("DDBSJ", ConfirmationNum);
            }
            if (arrItems.Contains("JOBAPPROVAL"))
            {
                var num =GetJobApprovalFormNum(user);
                data.Add("JOBAPPROVAL", num);
            }

            if (arrItems.Contains("DSPYJWZLY"))
            {
                data.Add("DSPYJWZLY", GetSuppliesAccept());//待审批应急物资领取
            }
            if (arrItems.Contains("DSHJGAQYS"))
            {
                //获取待审批的竣工安全验收
                data.Add("DSHJGAQYS", GetCheckCount(user, "EPG_SAFETYCOLLECT"));
            }
            // 安全考核待审批
            if (arrItems.Contains("APPLYSAFE"))
            {
                string safeApplyNum = "0";
                safeApplyNum = new SafetyAssessmentBLL().GetApplyNum();
                data.Add("APPLYSAFE", safeApplyNum);
            }

            #region 五定安全检查
            // 安全检查康巴什
            if (arrItems.Contains("FIVESAFECHECKNUM")) //安全检查
            {
                data.Add("FIVESAFECHECKNUM", new FivesafetycheckBLL().GetApplyNum("1","0","0"));

            }
            if (arrItems.Contains("FIVESAFELPJC")) //两票检查待审批
            {
                data.Add("FIVESAFELPJC", new FivesafetycheckBLL().GetApplyNum("2", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFEWWDWJC")) //外委单位检查待审批
            {
                data.Add("FIVESAFEWWDWJC", new FivesafetycheckBLL().GetApplyNum("3", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFEFWZJC")) //反违章检查待审批
            {
                data.Add("FIVESAFEFWZJC", new FivesafetycheckBLL().GetApplyNum("4", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFESMXTJC")) // 输煤系统检查待审批
            {
                data.Add("FIVESAFESMXTJC", new FivesafetycheckBLL().GetApplyNum("5", "0", "0"));

            }
            if (arrItems.Contains("FIVESAFEMCAQJC")) // 煤场安全检查待审批
            {
                data.Add("FIVESAFEMCAQJC", new FivesafetycheckBLL().GetApplyNum("6", "0", "0"));

            }

            if (arrItems.Contains("FIVESAFERCAQJC")) // 日常安全检查待审批
            {
                data.Add("FIVESAFERCAQJC", new FivesafetycheckBLL().GetApplyNum("7", "1", "0"));

            }
            if(arrItems.Contains("FIVESAFEZXAQJC")) // 专项安全检查待审批
            {
                data.Add("FIVESAFEZXAQJC", new FivesafetycheckBLL().GetApplyNum("8", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEJRJC")) // 	假日前后安全检查待审批
            {
                data.Add("FIVESAFEJRJC", new FivesafetycheckBLL().GetApplyNum("9", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEJJXJC")) // 	季节性安全检查
            {
                data.Add("FIVESAFEJJXJC", new FivesafetycheckBLL().GetApplyNum("10", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEZHJC")) // 	综合安全检查
            {
                data.Add("FIVESAFEZHJC", new FivesafetycheckBLL().GetApplyNum("11", "1", "0"));

            }
            if (arrItems.Contains("FIVESAFEQTJC")) // 	其他安全检查待审批
            {
                data.Add("FIVESAFEQTJC", new FivesafetycheckBLL().GetApplyNum("12", "1", "0"));

            }


            if (arrItems.Contains("FIVEQUACTION")) //检查问题待整改
            {
                data.Add("FIVEQUACTION", new FivesafetycheckBLL().GetApplyNum("", "", "1"));

            }
            if (arrItems.Contains("FIVEQUACCEPT")) //检查问题待验收
            {
                data.Add("FIVEQUACCEPT", new FivesafetycheckBLL().GetApplyNum("", "", "2"));

            }
            #endregion

            // 安全考核待审批
            if (arrItems.Contains("ACJHNum"))
            {
                data.Add("ACJHNum", service.GetSafeMeasureNum(user));//华电江陵安措计划
            }

            if (arrItems.Contains("RYLCDSP"))
            {
                data.Add("RYLCDSP", new LeaveApproveBLL().GetDBSXNum(user));//外包人员离厂审批
            }

            // 矩阵检查待
            if (arrItems.Contains("MatrixAction"))
            {
                data.Add("MatrixAction", new MatrixsafecheckBLL().GetActionNum());
            }

            return data;
        }
        public int GetCheckCount(ERCHTMS.Code.Operator user, string tableName)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select * from " + tableName + " t where flowdept like'%{0}%'", user.DeptId));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                {

                    var roleArr = user.RoleName.Split(','); //当前人员角色
                    var roleName = dr["flowrolename"].ToString(); //审核橘色
                    for (var i = 0; i < roleArr.Length; i++)
                    {
                        //满足审核部门同当前人部门id一致，切当前人角色存在与审核角色中
                        if (roleName.IndexOf(roleArr[i]) >= 0)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }
        public int GetWhplyCount(ERCHTMS.Code.Operator user, string tableName)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select * from " + tableName + " t"));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowdept"].ToString()))
                {
                    if (dr["flowdept"].ToString() == user.DeptId)
                    {
                        if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                        {

                            var roleArr = user.RoleName.Split(','); //当前人员角色
                            var roleName = dr["flowrolename"].ToString(); //审核橘色
                            for (var i = 0; i < roleArr.Length; i++)
                            {
                                //满足审核部门同当前人部门id一致，切当前人角色存在与审核角色中
                                if (roleName.IndexOf(roleArr[i]) >= 0)
                                {
                                    count++;
                                    break;
                                }
                            }
                        }
                    }
                }
                DataTable whpList = new DepartmentBLL().GetDataTable(string.Format(@"select * from XLD_DANGEROUSCHEMICAL t where id='{0}'", dr["mainid"].ToString()));
                if (whpList != null && whpList.Rows.Count > 0)
                {
                    if (whpList.Rows[0]["grantpersonid"].ToString() == user.UserId)
                    {
                        if (dr["grantstate"].ToString() == "2")//
                        {
                            count++;
                            continue;
                        }
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 获取待审批的安全动态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetAqdtCount(ERCHTMS.Code.Operator user)
        {
            DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select * from BIS_SECURITYDYNAMICS t where flowdept='{0}'", user.DeptId));
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr["flowrolename"].ToString()))
                {

                    var roleArr = user.RoleName.Split(','); //当前人员角色
                    var roleName = dr["flowrolename"].ToString(); //审核橘色
                    for (var i = 0; i < roleArr.Length; i++)
                    {
                        //满足审核部门同当前人部门id一致，切当前人角色存在与审核角色中
                        if (roleName.IndexOf(roleArr[i]) >= 0)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count;
        }
        public DataTable GetDeptDataStat(ERCHTMS.Code.Operator user, string itemType, int mode = 0)
        {
            DataTable dt = GetDeptDataSet(user, itemType);

            if (dt.Rows.Count > 0)
            {
                Hashtable ht = new Hashtable();
                if (itemType == "SSJK" || itemType == "AQZB")
                {
                    ht = GetMonitorData(user, itemType);
                }
                if (itemType == "DBSX")
                {
                    ht = GetWorkData(user, mode);
                }
                dt.Columns.Add("Num");
                foreach (DataRow dr in dt.Rows)
                {
                    if (ht.ContainsKey(dr["itemcode"].ToString()))
                    {
                        dr["Num"] = ht[dr["itemcode"].ToString()];

                        if (!string.IsNullOrWhiteSpace(dr["callback"].ToString()))
                        {
                            dr["callback"] = dr["callback"].ToString().Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Url}", dr["address"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(dr["itemstyle"].ToString()))
                        {
                            dr["itemstyle"] = dr["itemstyle"].ToString().Replace("{Callback}", dr["Callback"].ToString()).Replace("{Icon}", dr["Icon"].ToString()).Replace("{ItemName}", dr["ItemName"].ToString()).Replace("{Num}", dr["Num"].ToString()).Replace("{Url}", dr["address"].ToString());
                        }

                    }

                }
            }
            return dt;
        }

        private Hashtable GetIndexData(Operator user)
        {
            throw new NotImplementedException();
        }

        #region 获取本年度趋势图(隐患总数、安全检查总数)
        /// <summary>
        /// 获取本年度趋势图(隐患总数、安全检查总数)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetTendencyChart(ERCHTMS.Code.Operator user)
        {
            return service.GetTendencyChart(user);
        }
        #endregion
        /// <summary>
        /// 获取外包工程数量信息（在建工程数量，外包人员在厂人数，外包工程总数，在场外包单位数，本月新进外包人员，外包单位违章次数）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetWBProjectNum(ERCHTMS.Code.Operator user)
        {
            return service.GetWBProjectNum(user);
        }
        /// <summary>
        /// 隐患统计图表
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GetHTChart(ERCHTMS.Code.Operator user)
        {
            return service.GetHTChart(user);
        }
        /// <summary>
        /// 获取当前用户需要审批的记录ID（多个用逗号分隔）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<string> GetThreeCount(Operator user)
        {
            List<string> list = new List<string>();
           
            DepartmentBLL deptBll = new DepartmentBLL();
            DataTable dt = deptBll.GetDataTable(string.Format("select t.id,m.ApplyType,t.applytype type1,t.createuserdeptid,ProjectId,belongdeptid,nodeid,m.checkdeptid,m.checkroleid,m.scriptcurcontent,CheckUserId from BIS_THREEPEOPLECHECK t left join bis_manypowercheck m on t.nodeid=m.id where isover=0 and issumbit=1 and nodeid is not null"));
            UserBLL userBll = new UserBLL();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["applytype"].ToString() == "0")
                {
                    string deptId = "";
                    if (dr["checkdeptid"].ToString() == "-1")
                    {
                      
                        if (dr["type1"].ToString() == "内部")
                        {
                            deptId = dr["belongdeptid"].ToString();
                        }
                        else
                        {
                            string sql = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'", dr["projectid"].ToString());
                            DataTable dtPeoject = deptBll.GetDataTable(sql);
                            if (dtPeoject.Rows.Count > 0)
                            {
                                deptId = dtPeoject.Rows[0][0].ToString();
                            }
                        }
                       
                    }
                    if (dr["checkdeptid"].ToString() == "-3")
                    {
                        deptId = dr["createuserdeptid"].ToString();
                    }
                    string userIds = userBll.GetWFUserListByDeptRoleOrg(user.OrganizeId, deptId, "", dr["checkroleid"].ToString(), "").Cast<UserInfoEntity>().Aggregate("", (current, u) => current + (u.UserId + ",")).Trim(',');
                    if (userIds.Contains(user.UserId))
                    {
                        list.Add(dr["id"].ToString());
                    }
                }
                if (dr["applytype"].ToString() == "1")
                {
                    string []arr=deptBll.GetDataTable(dr["scriptcurcontent"].ToString()).AsEnumerable().Select(t => t.Field<string>("userid")).ToArray();
                    string userIds = string.Join(",", arr);
                    if (userIds.Contains(user.UserId))
                    {
                        list.Add(dr["id"].ToString());
                    }
                }
                if (dr["applytype"].ToString() == "2")
                {

                    if (dr["CheckUserId"].ToString().Contains(user.UserId))
                    {
                        list.Add(dr["id"].ToString());
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取待审批三种人数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetThreeCount1(ERCHTMS.Code.Operator user)
        {
            string count = new DepartmentBLL().GetDataTable(string.Format(@"select count(1) from (SELECT c.checkroleid,case when c.checkdeptid='-3' then t.createuserdeptid else to_char(c.checkdeptid) end as checkdeptid from BIS_THREEPEOPLECHECK t left join bis_manypowercheck c on t.nodeid=c.id) b where checkdeptid='{0}' and iscontaions(checkroleid,'{1}')=1", user.DeptId, user.RoleId)).Rows[0][0].ToString();
            return int.Parse(count);
        }
        /// <summary>
        /// 按工程类型统计外包工程
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        public DataTable GetProjectChart(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectChart(user);
        }
        /// <summary>
        /// 按工程风险等级统计外包工程
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        public DataTable GetProjectChartByLevel(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectChartByLevel(user);
        }

        /// <summary>
        /// 获取检查发现的隐患
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetCheckHtNum(ERCHTMS.Code.Operator user)
        {
            return service.GetCheckHtNum(user);
        }
        /// <summary>
        /// 外包人员数量变化趋势图
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GetProjectPersonChart(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectPersonChart(user);
        }
        /// <summary>
        /// 按隐患分类统计隐患
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        public DataTable GetHTTypeChart(ERCHTMS.Code.Operator user)
        {
            return service.GetHTTypeChart(user);
        }
        /// <summary>
        /// 隐患数量变化趋势图 
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GetHTChangeChart(ERCHTMS.Code.Operator user)
        {
            return service.GetHTChangeChart(user);
        }
        /// <summary>
        /// 获取通知公告
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetNotices(ERCHTMS.Code.Operator user)
        {
            return service.GetNotices(user);
        }

        /// <summary>
        /// 获取一号岗大屏标题
        /// </summary>        
        /// <returns></returns>
        public DataTable GetScreenTitle()
        {
            return service.GetScreenTitle();
        }

        /// <summary>
        /// 获取安全会议
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetMeets(ERCHTMS.Code.Operator user)
        {
            return service.GetMeets(user);
        }
        /// <summary>
        /// 获取安全动态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetTrends(ERCHTMS.Code.Operator user)
        {
            return service.GetTrends(user);
        }
        /// <summary>
        /// 获取红黑榜
        /// </summary>
        /// <param name="user"></param>
        ///  <param name="mode">0:红榜，1:黑榜</param>
        /// <returns></returns>
        public DataTable GetRedBlack(ERCHTMS.Code.Operator user, int mode)
        {
            return service.GetRedBlack(user, mode);
        }
        /// <summary>
        /// 外包工程概况统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetProjectStat(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectStat(user);
        }
        /// <summary>
        /// 获取人员违章信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetWZInfo(Pagination pagination, string queryJson)
        {
            return service.GetWZInfo(pagination, queryJson);
        }
        public DataTable GetWZInfo(string userid, int mode=0)
        {
            return service.GetWZInfo(userid, mode);
        }
        public DataTable GetWZInfoByUserId(string userId,int mode=0)
        {
            return service.GetWZInfoByUserId(userId, mode);
        }
        /// <summary>
        /// 获取未签到的会议数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetMeetNum(string userId)
        {
            return service.GetMeetNum(userId);
        }
        /// <summary>
        /// 获取施工中危大工程数
        /// <param name="user"></param>
        /// </summary>
        /// <returns></returns>
        public int GetProjectNum(ERCHTMS.Code.Operator user)
        {
            return service.GetProjectNum(user);
        }
        /// <summary>
        ///获取隐患数量(依次为总数量，重大隐患数量，一般隐患数量,整改延期的隐患数,逾期未整改隐患数,已整改隐患，未整改隐患)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetHtNum(ERCHTMS.Code.Operator user)
        {
            return service.GetHtNum(user);
        }
        /// <summary>
        /// 获取危险源数量（依次为总数量，重大危险源数量）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetDangerSourceNum(ERCHTMS.Code.Operator user)
        {
            return service.GetDangerSourceNum(user);
        }
        /// <summary>
        /// 获取事故数量信息（依次为事故起数，死亡人数，重伤人员）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetAccidentNum(ERCHTMS.Code.Operator user)
        {
            return service.GetAccidentNum(user);
        }
        /// <summary>
        /// 获取重大风险数量(依次为总数量，重大风险数量，较大风险数量，一般风险数量，低风险数量)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetRiskNum(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskNum(user);
        }
        /// <summary>
        /// 获取高风险作业(依次为高风险通用待确认作业数量，高风险通用待审核(批)作业数量,待监督的数量,高风险作业总数）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetWorkNum(ERCHTMS.Code.Operator user)
        {
            return service.GetWorkNum(user);
        }

        /// <summary>
        /// 获取高危作业安全许可证审批待办（依次为高处作业、起重吊装作业、动土作业、断路作业、动火作业、盲板抽堵作业、受限空间作业、设备检修清理作业、待措施确认、待停电、待备案、待验收、待送电）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetJobSafetyCardNum(ERCHTMS.Code.Operator user)
        {
            return service.GetJobSafetyCardNum(user);
        }

        /// <summary>
        /// 获取脚手架统计（待验收、待审核）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetScaffoldNum(ERCHTMS.Code.Operator user)
        {
            return service.GetScaffoldNum(user);
        }

        /// <summary>
        /// 获取消防水待审核(批)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetFireWaterNum(ERCHTMS.Code.Operator user)
        {
            return service.GetFireWaterNum(user);
        }
        /// <summary>
        /// 获取特种设备数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetEquimentNum(ERCHTMS.Code.Operator user)
        {
            return service.GetEquimentNum(user);
        }
        /// <summary>
        /// 获取违章数量信息（依次为：违章总数量、待核准、待整改、待验收、逾期未整改数量、逾期整改数量，待完善的违章数量）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetlllegalNum(ERCHTMS.Code.Operator user)
        {
            return service.GetlllegalNum(user);
        }


        /// <summary>
        /// 应急内容数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetDrillRecordNum(ERCHTMS.Code.Operator user) 
        {
            return service.GetDrillRecordNum(user);
        }
        /// <summary>
        /// 获取问题数量信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetQuestionNum(ERCHTMS.Code.Operator user)
        {
            return service.GetQuestionNum(user);
        }
        /// <summary>
        /// 获取违章整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public decimal GetlllegalRatio(ERCHTMS.Code.Operator user)
        {
            return service.GetlllegalRatio(user);
        }
        /// <summary>
        /// 按风险等级绘图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetRiskCounChart(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskCounChart(user);
        }
        /// <summary>
        /// 获取安全事例
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetWorks(ERCHTMS.Code.Operator user)
        {
            return service.GetWorks(user);
        }
        /// <summary>
        /// 根据日期获取个人安全事例记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public DataTable GetWorkInfoByTime(ERCHTMS.Code.Operator user, string time)
        {
            return service.GetWorkInfoByTime(user, time);
        }
        /// <summary>
        /// 获取各区域的最大风险等级信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode">区域唯一标识码，不一定是区域编码，多个用英文逗号分隔</param>
        /// <returns></returns>
        public DataTable GetAreaStatus(ERCHTMS.Code.Operator user, string areaCode, int mode = 0)
        {
            return service.GetAreaStatus(user,areaCode, mode);
        }

        /// <summary>
        /// 获取各区域的最大风险等级信息(康巴什版本)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public List<AreaRiskLevel> GetKbsAreaStatus()
        {
            return service.GetKbsAreaStatus();
        }


        /// <summary>
        /// 获取各区域的最大风险等级信息(可门管控中心【风险四色图】版本)
        /// </summary>
        /// <returns></returns>
        public List<AreaRiskLevel> GetKMAreaStatus()
        {
            return service.GetKMAreaStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetSafetyChangeNum(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyChangeNum(user);
        }
        /// <summary>
        /// 安全预警项目（省公司级），依次为存在重大隐患的电厂数，隐患整改率小于80%的电厂数，隐患整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<decimal> GetWarnItems(ERCHTMS.Code.Operator user)
        {
            return service.GetWarnItems(user);
        }

        public List<decimal> GetRiskAnalyzeItems(ERCHTMS.Code.Operator user)
        {
            return service.GetRiskAnalyzeItems(user);
        }

        /// <summary>
        /// 获取安全检查数，依次为安全检查次数，待执行的安全检查数（省公司级）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetSafetyCheckForGroup(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyCheckForGroup(user);
        }
        /// <summary>
        /// 获取隐患信息，依次为重大隐患数，整改延期隐患数，逾期未整改隐患数，发现的隐患数量（省公司级）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetHtForGroup(ERCHTMS.Code.Operator user)
        {
            return service.GetHtForGroup(user);
        }
        /// <summary>
        /// 获取电厂预警分数值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public decimal GetScore(ERCHTMS.Code.Operator user, string time = "")
        {
            RiskBLL riskBLL = new RiskBLL();//安全风险
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//安全检查
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//事故隐患
            ClassificationBLL classBLL = new ClassificationBLL();
            List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();
            if (list.Count == 0)
            {
                list = classBLL.GetList("0").ToList();
            }
            decimal totalScore = 0;
            //计算事故隐患总得分
            decimal score = htBLL.GetHiddenWarning(user, time);
            totalScore = score * decimal.Parse(list[0].WeightCoeffcient);
            //计算安全检查总得分
            score = saBLL.GetSafeCheckSumCount(user);
            totalScore += score * decimal.Parse(list[1].WeightCoeffcient);
            //计算安全风险总得分
            score = riskBLL.GetRiskValueByTime(user, time);
            totalScore += score * decimal.Parse(list[2].WeightCoeffcient);
            return totalScore;
        }
        /// <summary>
        /// 获取电厂预警分数信息（以此为得分，安全级别）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<string> GetScoreInfo(ERCHTMS.Code.Operator user, string time = "")
        {
            RiskBLL riskBLL = new RiskBLL();//安全风险
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//安全检查
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//事故隐患
            ClassificationBLL classBLL = new ClassificationBLL();
            List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();
            if (list.Count == 0)
            {
                list = classBLL.GetList("0").ToList();
            }
            decimal totalScore = 0;
            //计算事故隐患总得分
            decimal score = htBLL.GetHiddenWarning(user, time);
            totalScore = score * decimal.Parse(list[0].WeightCoeffcient);
            //计算安全检查总得分
            score = saBLL.GetSafeCheckSumCount(user);
            totalScore += score * decimal.Parse(list[1].WeightCoeffcient);
            ////计算安全风险总得分
            //score = riskBLL.GetRiskValueByTime(user, time);
            //totalScore += score * decimal.Parse(list[2].WeightCoeffcient);

            List<string> data = new List<string>();
            data.Add(totalScore.ToString());

            DataItemDetailBLL itemBLL = new DataItemDetailBLL();
            string val = itemBLL.GetItemValue("基础预警区间分值设置");
            int idx = 0;
            string[] arr = val.Split('|');
            if (!string.IsNullOrEmpty(val))
            {
                int j = 0;
                foreach (string str in arr)
                {
                    string[] arrVal = str.Split(',');
                    if (totalScore > decimal.Parse(arrVal[0]) && totalScore <= decimal.Parse(arrVal[1]))
                    {
                        idx = j;
                        break;
                    }
                    j++;
                }
            }
            arr = new string[] { "安全", "注意", "警告", "危险" };
            data.Add(arr[idx]);
            return data;
        }
        /// <summary>
        /// 电厂隐患排名
        /// </summary>
        /// <param name="deptCode">省公司deptCode</param>
        /// <param name="mode">排名方式，0：按隐患数量排名，1：按隐患整改率排名，2：按未闭环的数量排名</param>
        /// <returns></returns>
        public DataView GetRatioDataOfFactory(ERCHTMS.Code.Operator user, int mode = 0)
        {
            return service.GetRatioDataOfFactory(user, mode);
        }
        /// <summary>
        /// 电厂安全检查和隐患信息统计
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<decimal> GetHt2CheckOfFactory(string orgId, string time, string orgCode = "")
        {
            return service.GetHt2CheckOfFactory(orgId, time, orgCode);
        }
        /// <summary>
        /// 获取当前用户所属机构的数据指标项目
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetDeptDataSet(ERCHTMS.Code.Operator user, string itemType)
        {
            return service.GetDeptDataSet(user, itemType);
        }
        /// <summary>
        /// 获取电厂隐患整改率
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<decimal> GetHtZgl(string orgId)
        {
            return service.GetHtZgl(orgId);
        }
        /// <summary>
        /// 获取省公司下发的安全检查任务
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetSafetyCheckTask(ERCHTMS.Code.Operator user)
        {
            return service.GetSafetyCheckTask(user);
        }

        /// <summary>
        /// 获取重大风险数量(依次为总数量，重大风险数量，较大风险数量，一般风险数量，低风险数量)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> GetRiskNumForGDXY(ERCHTMS.Code.Operator user)
        {
            DepartmentBLL deptBll = new DepartmentBLL();
            List<int> list = new List<int>();
            string sql = "";
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0  and t.deptcode like '{0}%' and risktype in('管理','设备','区域')", user.OrganizeCode);
            }
            else if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0 and risktype in('管理','设备','区域') and t.createuserorgcode in (select encode from BASE_DEPARTMENT d where d.deptcode like '{0}%' and d.nature='{1}')", user.NewDeptCode, "厂级");
            }
            else
            {
                sql = string.Format("select count(1) from BIS_RISKASSESS t where status=1 and deletemark=0 and enabledmark=0  and t.deptcode like '{0}%' and risktype in('管理','设备','区域')", user.DeptCode);
            }
            int count = int.Parse(deptBll.GetDataTable(sql).Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='重大风险'").Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='较大风险'").Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='一般风险'").Rows[0][0].ToString());
            list.Add(count);
            count = int.Parse(deptBll.GetDataTable(sql + " and grade='低风险'").Rows[0][0].ToString());
            list.Add(count);
            return list;
        }

        #region 安全状态分数值
        #region MyRegion
        /// <summary>
        /// 获取指标值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public decimal GetSafetyAssessedValue(SafetyAssessedArguments entity)
        {
            return service.GetSafetyAssessedValue(entity);
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 获取指标大项
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public List<SafetyAssessedModel> GetSafetyAssessedData(SafetyAssessedArguments argument)
        {
            return service.GetSafetyAssessedData(argument);
        }
        #endregion

        #region 获取模块下对应的指标
        /// <summary>
        /// 获取模块下对应的指标
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<SafetyAssessedChildModel> GetSafetyAssessedChildData(SafetyAssessedArguments argument, List<ClassificationIndexEntity> list)
        {
            return service.GetSafetyAssessedChildData(argument, list);
        }
        #endregion
        #endregion


        #region 获取曝光信息
        /// <summary>
        /// 获取曝光信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetExposureInfo(ERCHTMS.Code.Operator user)
        {
            try
            {
                return service.GetExposureInfo(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 华升大屏实时工作
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<RealTimeWorkModel> GetRealTimeWork(Operator user)
        {
            return service.GetRealTimeWork(user);
        }
        /// <summary>
        /// 华昇大屏预警中心
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<RealTimeWorkModel> GetWarningCenterWork(Operator user)
        {
            return service.GetWarningCenterWork(user);
        }
        public int GetJobApprovalFormNum(ERCHTMS.Code.Operator user)
        {
            return service.GetJobApprovalFormNum(user);
        }

        /// <summary>
        /// 获取今日高危作业
        /// </summary>
        /// <returns></returns>
        public int GetTodayWorkForDangerJob()
        {
            JobSafetyCardApplyBLL BLL = new JobSafetyCardApplyBLL();
            Pagination pagenation = new Pagination();
            pagenation.rows = 100000;
            pagenation.page = 1;
            DataTable dt = BLL.GetTodayWorkList(pagenation, JsonConvert.SerializeObject(new { }));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取应急物资领用待办
        /// </summary>
        /// <returns></returns>
        public int GetSuppliesAccept()
        {
            try
            {
                SuppliesacceptBLL BLL = new SuppliesacceptBLL();
                Pagination pagenation = new Pagination();
                pagenation.rows = 100000;
                pagenation.page = 1;
                pagenation.conditionJson = "1=1";
                DataTable dt = BLL.GetPageList(pagenation, JsonConvert.SerializeObject(new { dbsx = "0" }));
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }


        #region 国电汉川对接待办推送
        /// <summary>
        /// 国电汉川对接待办推送(接收)
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncJS(GdhcDbsxEntity entity)
        {

            service.GdhcDbsxSyncJS(entity);
        }

        /// <summary>
        /// 国电汉川对接待办推送(已办)
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncYB(GdhcDbsxEntity entity)
        {

            service.GdhcDbsxSyncYB(entity);
        }

        /// <summary>
        /// 国电汉川对接待办推送(办结)
        /// </summary>
        /// <param name="entity"></param>
        public void GdhcDbsxSyncBJ(GdhcDbsxEntity entity)
        {

            service.GdhcDbsxSyncBJ(entity);
        }
        #endregion
    }

}
