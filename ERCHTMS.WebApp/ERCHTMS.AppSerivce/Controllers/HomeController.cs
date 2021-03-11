using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PowerPlantInside;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SafetyWorkSupervise;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    [WebAPIHandlerError]
    public class HomeController : BaseApiController
    {
        RiskAssessBLL riskassessbll = new RiskAssessBLL();
        HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        DesktopBLL desktopbll = new DesktopBLL();

        #region 获取首页安全指标
        /// <summary>
        /// 获取首页安全指标
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getIndexData([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = new DesktopBLL().GetDeptDataStat(user, "AQZB");
                dt.Columns.Remove("address"); dt.Columns.Remove("callback"); dt.Columns.Remove("itemstyle"); dt.Columns.Remove("itemtype");
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["icon"] = path + dr["icon"].ToString();
                }
                return new
                {
                    Code = 0,
                    Count = dt.Rows.Count,
                    Info = "获取数据成功",
                    data = dt
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 首页统计
        /// <summary>
        /// 首页统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getMainInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                int safechecknum = 0; //待执行的安全检查数
                int appoveproblemnum = 0;//待评估隐患数
                int changeplanproblemnum = 0;//待制定整改计划
                int modifyproblemnum = 0;//待整改隐患数
                int modifypostphoneproblemnum = 0;//待审(核)批整改延期隐患数
                int reviewproblemnum = 0;//待验收的隐患数
                int assessproblemnum = 0;//待评估隐患数
                int recheckproblemnum = 0; //待复查验证的隐患数
                int overtimempnum = 0;//逾期未整改隐患
                int meuploadp = 0;   //我上传的隐患
                int planNum = 0;//进行中的风险辨识计划数

                RiskPlanBLL planBll = new RiskPlanBLL();
                planNum = planBll.GetPlanCount(user, 10);
                SaftyCheckDataBLL saftbll = new SaftyCheckDataBLL();
                int[] countcheck = saftbll.GetCheckCount(user, 0);
                safechecknum = countcheck.Sum();
                //隐患待办事项
                var data = new HTBaseInfoBLL().QueryHidBacklogRecord("0", user.UserId);
                if (data.Rows.Count == 8)
                {
                    appoveproblemnum = int.Parse(data.Rows[0]["pnum"].ToString());
                    modifyproblemnum = int.Parse(data.Rows[1]["pnum"].ToString());
                    modifypostphoneproblemnum = int.Parse(data.Rows[2]["pnum"].ToString());
                    reviewproblemnum = int.Parse(data.Rows[3]["pnum"].ToString());
                    assessproblemnum = int.Parse(data.Rows[4]["pnum"].ToString());
                    changeplanproblemnum = int.Parse(data.Rows[7]["pnum"].ToString());
                    recheckproblemnum = int.Parse(data.Rows[6]["pnum"].ToString());
                }

                var datas = new HTBaseInfoBLL().QueryHidBacklogRecord("10", user.UserId);
                if (datas.Rows.Count == 2)
                {
                    if (datas.Rows[0]["serialnumber"].ToString() == "1")
                    {
                        meuploadp = int.Parse(datas.Rows[0]["pnum"].ToString());
                    }
                    if (datas.Rows[1]["serialnumber"].ToString() == "2")
                    {
                        overtimempnum = int.Parse(datas.Rows[1]["pnum"].ToString());
                    }

                }
                int meetnum = 0;//安全会议数量
                int verifydangerworknum = 0;//高风险作业审核数量
                int approvedangerworknum = 0;//高风险作业审批数量
                int monitordangerworknum = 0;//高风险作业监督数量
                int approveillegalnum = 0;//待核准的违章数量
                int verifyillegalnum = 0;//待整改的违章数量
                int reviewillegalnum = 0;//待验收的违章数量
                int risktrainnum = 0;//风险预知训练
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new
                    {
                        safechecknum = safechecknum,//安全检查数
                        appoveproblemnum = appoveproblemnum,//待评估隐患
                        changeplanproblemnum = changeplanproblemnum, //待制定整改计划
                        modifyproblemnum = modifyproblemnum,//待整改隐患
                        modifypostphoneproblemnum = modifypostphoneproblemnum,//待审(核)批整改延期隐患数
                        reviewproblemnum = reviewproblemnum,//待复查验收的隐患数
                        assessproblemnum = assessproblemnum,//待评估隐患数
                        recheckproblemnum = recheckproblemnum, //待复查验证隐患数
                        assessplannum = planNum,//待辨识的计划
                        overtimempnum = overtimempnum,//逾期未整改隐患
                        meuploadp = meuploadp, //我上传的隐患

                        meetnum = meetnum,///安全会议数量
                        verifydangerworknum = verifydangerworknum,//高风险作业审核数量
                        approvedangerworknum = approvedangerworknum,//高风险作业审批数量
                        monitordangerworknum = monitordangerworknum,//高风险作业监督数量
                        risktrainnum = risktrainnum,//风险预知训练
                        approveillegalnum = approveillegalnum,//待核准的违章数量
                        verifyillegalnum = verifyillegalnum,//待整改的违章数量
                        reviewillegalnum = reviewillegalnum //待验收的违章数量

                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 待办事项
        /// <summary>
        /// 待办事项
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getDelayItems([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                int safechecknum = 0; //待执行的安全检查数
                int appoveproblemnum = 0;//待评估隐患数
                int changeplanproblemnum = 0;//待制定整改计划
                int modifyproblemnum = 0;//待整改隐患数
                int modifypostphoneproblemnum = 0;//待审(核)批整改延期隐患数
                int reviewproblemnum = 0;//待验收的隐患数
                int assessproblemnum = 0;//待评估隐患数
                int recheckproblemnum = 0; //待复查验证的隐患数
                int overtimempnum = 0;//逾期未整改隐患
                int uploadHtNum = 0;   //我上传的隐患
                int planNum = 0;//进行中的风险辨识计划数
                int dailyexamineNum = 0;//日常考核待审核数

                int everydaypatrolNum = 0;//消防巡查代办

                int powerhandleapproveNum = 0;//待审核事故事件处理数
                int carVistorAuditNum = 0;//可门拜访人员待审批记录

                int FeedbackNum = 0;//待反馈数据
                int ConfirmationNum = 0;//待督办确认数据

                SafetyworksuperviseBLL supervise = new SafetyworksuperviseBLL();
                FeedbackNum = supervise.GetSuperviseNum(user.UserId, "1");//待反馈数据
                ConfirmationNum = supervise.GetSuperviseNum(user.UserId, "2");//待督办确认数据


                RiskPlanBLL planBll = new RiskPlanBLL();
                planNum = planBll.GetPlanCount(user, 10);
                SaftyCheckDataBLL saftbll = new SaftyCheckDataBLL();
                int[] countcheck = saftbll.GetCheckCount(user, 0);
                safechecknum = countcheck.Sum();
                DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
                dailyexamineNum = dailyexaminebll.CountIndex(user);
                everydaypatrolNum = GetEverydayPatrolListCount(userId);
                carVistorAuditNum = new CarUserBLL().GetStayApprovalRecordCount(userId);

                int awaitHtNum = 0;//待完善的隐患数量
                //隐患待办事项
                var data = new HTBaseInfoBLL().QueryHidBacklogRecord("0", user.UserId);
                if (data.Rows.Count == 8)
                {
                    appoveproblemnum = int.Parse(data.Rows[0]["pnum"].ToString());
                    changeplanproblemnum = int.Parse(data.Rows[7]["pnum"].ToString());
                    modifyproblemnum = int.Parse(data.Rows[1]["pnum"].ToString());
                    modifypostphoneproblemnum = int.Parse(data.Rows[2]["pnum"].ToString());
                    reviewproblemnum = int.Parse(data.Rows[3]["pnum"].ToString());
                    assessproblemnum = int.Parse(data.Rows[4]["pnum"].ToString());
                    recheckproblemnum = int.Parse(data.Rows[6]["pnum"].ToString());
                    awaitHtNum = int.Parse(data.Rows[5]["pnum"].ToString());
                }

                var datas = new HTBaseInfoBLL().QueryHidBacklogRecord("10", user.UserId);
                if (datas.Rows.Count == 2)
                {
                    if (datas.Rows[0]["serialnumber"].ToString() == "1")
                    {
                        uploadHtNum = int.Parse(datas.Rows[0]["pnum"].ToString());
                    }
                    if (datas.Rows[1]["serialnumber"].ToString() == "2")
                    {
                        overtimempnum = int.Parse(datas.Rows[1]["pnum"].ToString());
                    }

                }
                DesktopBLL desktop = new DesktopBLL();
                List<int> list = desktop.GetHtNum(user);
                int htNum = list[0];//隐患数量
                int bigHtNum = list[1];//重大隐患数量
                int overdueHtCompleteNum = list[3];//整改延期数量

                list = desktop.GetRiskNum(user);
                int bigRiskNum = list[1];//重大风险数量

                list = desktop.GetWorkNum(user);
                int waitconfirmationnum = list[0];//高风险通用待确认作业数量
                int waitapproveworknum = list[1];//高风险通用待审核(批)作业数量
                int monitordangerworknum = list[5];//高风险待监督的数量
                int sidetaskallocation = list[2];//高风险待分配任务
                int sidesupervisionnum = list[6];//高风险待监管任务
                int dangerworknum = list[3];//高风险作业数量
                int temptodaynum = list[7];//今日高风险作业

                list = desktop.GetScaffoldNum(user);
                int waitscaffoldchecknum = list[0];//脚手架待验收
                int waitscaffoldauditnum = list[1];//脚手架待审核

                int lifthoistNum = Convert.ToInt32(new LifthoistjobBLL().GetLifthoistjobNum());//待审核起重吊装


                list = desktop.GetFireWaterNum(user);
                int waitfirewaterauditnum = list[0];//消防水使用待审核


                int specialEquNum = desktop.GetEquimentNum(user);//特种设备数量
                int dangerProjectNum = desktop.GetProjectNum(user);//施工中的危大工程数量

                list = desktop.GetJobSafetyCardNum(user);//获取高危作业安全许可证待办
                int JobSafetyCardNum = list.Sum();

                list = desktop.GetlllegalNum(user);
                int illegalNum = list[0];//违章数量
                int approveillegalnum = list[1];//待核准的违章数量
                int verifyillegalnum = list[2];//待整改的违章数量
                int reviewillegalnum = list[3];//待验收的违章数量
                int overdueIllegalNum = list[4];//逾期未整改违章数量
                int perfectionlllegalNum = list[6]; //待完善的违章
                int planreformlllegalNum = list[7]; //待制定整改计划 
                int acceptlllegalNum = list[8]; //待验收确认
                int postponelllegalNum = list[9]; //待整改延期违章
                int lllegalreformaffirmNum = 0; //违章整改确认数量  
                if (curUser.RoleName.Contains("安全管理员"))
                {
                    lllegalreformaffirmNum = list[10];
                }
                decimal illegalCompleteRatio = desktop.GetlllegalRatio(user);//违章整改率


                var drilllist = desktop.GetDrillRecordNum(user);
                int drillplanrecordNum = drilllist[0];//待完善应急演练记录

                //问题流程
                list = desktop.GetQuestionNum(user);//问题
                int reformquestionnum = list[0];//待整改问题
                int verifyquestionnum = list[1];//待验证问题
                int approvequestionnum = list[2]; //待评估的发现问题

                int meetnum = desktop.GetMeetNum(user.UserId);//安全会议数量

                int safetynum = desktop.GetSafetyChangeNum(user);//安全设施变动审待核数量
                int risktrainnum = 0;//风险预知训练

                list = new OutprojectblacklistBLL().ToAuditOutPeoject(user);//外包工程
                int wbscla = list[2];//待审（核）批三措两案
                int wbrcsq = list[5];//待审（核）批入厂许可
                int wbkgsq = list[6];//待审（核）批开工申请
                int wbaqjsjd = list[7];//待审核安全技术交底
                //data.Add("WBDWZZ", wb[0]);//单位资质
                //data.Add("WBRYZZ", wb[1]);//人员资质
                //data.Add("WBSCLA", wb[2]);//三措两案
                //data.Add("WBDDSB", wb[3]);//特种设备
                //data.Add("WBTZSB", wb[4]);//电动设备
                //data.Add("WBRCSQ", wb[5]);//入场许可
                //data.Add("WBKGSQ", wb[6]);//开工申请

                int tempProject = 0;//今日临时外包工程

                tempProject = new WorkMeetingBLL().GetTodayTempProject(user);
                var sgsj = new PowerplanthandleBLL().ToAuditPowerHandle();//事故事件处理待办
                powerhandleapproveNum = int.Parse(sgsj[0]) + int.Parse(sgsj[1]) + int.Parse(sgsj[2]) + int.Parse(sgsj[3]);
                //危险作业审批单
                int JobApprovalNum = desktop.GetJobApprovalFormNum(user);

                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new
                    {
                        safeCheckNum = safechecknum,//安全检查数
                        meetNum = meetnum,///安全会议数量
                        modifyProblemNum = modifyproblemnum,//待整改隐患
                        appoveProblemNum = appoveproblemnum,//待评估隐患
                        changeplanProblemNum = changeplanproblemnum, //待制定整改计划
                        reviewProblemNum = reviewproblemnum,//待验收的隐患数
                        delayProblemNum = modifypostphoneproblemnum,//待审(核)批整改延期隐患数
                        assessProblemNum = assessproblemnum,//待整改效果评估隐患
                        recheckProblemNum = recheckproblemnum,//待复查验证的隐患
                        assessPlanNum = planNum,//待辨识的计划

                        dangerWorkNum = dangerworknum,//高风险作业数量
                        monitorDangerworkNum = monitordangerworknum,//高风险作业监督数量
                        waitConfirmationNum = waitconfirmationnum,//高风险通用待确认作业数量
                        waitApproveWorkNum = waitapproveworknum,//高风险通用待审核(批)作业数量
                        waitConfirmAndApproveWorkNum = waitapproveworknum + waitconfirmationnum,//高风险通用待确认作业数量加上待审核(批)作业数量
                        sidetaskallocation = sidetaskallocation,//待分配
                        sidesupervisionnum = sidesupervisionnum,//待监管
                        carVistorAuditNum = carVistorAuditNum,//待审批

                        waitlifthoistNum = lifthoistNum,//待审核审批起重吊装作业

                        waitscaffoldcheckNum = waitscaffoldchecknum, //脚手架待验收
                        waitscaffoldauditNum = waitscaffoldauditnum, //脚手架待审核
                        waitfirewaterauditnum = waitfirewaterauditnum,//消防水使用待审核

                        /*****验收信息****/
                        dangerTrainNum = risktrainnum,//风险预知训练
                        approveIllegalNum = approveillegalnum,//待核准的违章数量
                        verifyIllegalNum = verifyillegalnum,//待整改的违章数量
                        reviewIllegalNum = reviewillegalnum, //待验收的违章数量
                        perfectionlllegalNum = perfectionlllegalNum, //待完善的违章
                        planreformlllegalNum = planreformlllegalNum,//待制定整改计划 
                        acceptlllegalNum = acceptlllegalNum,//待验收确认
                        postponelllegalNum = postponelllegalNum, //待整改延期违章
                        lllegalreformaffirmNum = lllegalreformaffirmNum, //违章整改确认 

                        drillplanrecordNum = drillplanrecordNum,//待完善应急演练记录

                        reformQuestionNum = reformquestionnum, //待整改的问题 
                        verifyQuestionNum = verifyquestionnum, //待验证的问题  
                        approveQuestionNum = approvequestionnum, //待评估的发现问题

                        htNum = htNum,//隐患数量
                        bigHtNum = bigHtNum,//重大隐患数量
                        overdueHtNum = overtimempnum,//逾期未整改隐患数量
                        bigRiskNum = bigRiskNum,//重大风险数量
                        specialEquNum = specialEquNum,//特种设备数量
                        dangerProjectNum = dangerProjectNum,//施工中的危大工程数量
                        overdueIllegalNum = overdueIllegalNum,//逾期未整改违章数量
                        illegalCompleteRatio = illegalCompleteRatio,//违章整改率
                        illegalNum = illegalNum,//违章数量
                        overdueHtCompleteNum = overdueHtCompleteNum,//整改延期隐患数量
                        uploadHtNum = uploadHtNum, //我上传隐患数量
                        safetynum = safetynum,//安全设施变动审待核数量
                        awaitHtNum = awaitHtNum,//待完善的隐患数量
                        wbsclaNum = wbscla,
                        wbrcsqNum = wbrcsq,
                        wbkgsqNum = wbkgsq,
                        wbaqjsjdNum = wbaqjsjd,
                        dailyexamineNum = dailyexamineNum,
                        tempProject = tempProject, // 今日临时外包工程
                        temptodaynum = temptodaynum,//今日高风险作业
                        firepatrolnum = everydaypatrolNum,
                        powerhandleapprovenum = powerhandleapproveNum,//待审核事故事件处理
                        FeedbackNum = FeedbackNum,//待反馈数据
                        ConfirmationNum = ConfirmationNum,//待督办确认数据
                        JobSafetyCardNum = JobSafetyCardNum,//高危作业 作业安全证待办
                        JobApprovalNum = JobApprovalNum //危险作业审批单
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        private EverydayPatrolBLL everydaypatrolbll = new EverydayPatrolBLL();
        public int GetEverydayPatrolListCount(string userId)
        {
            try
            {
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                //获取页数和条数
                int page = 1, rows = 99999999;
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "PatrolType,PatrolTypeCode,AffirmState,AffirmUserId,District,PATROLDEPT,PATROLDATE,PATROLPERSON,PATROLPLACE,PROBLEMNUM,createuserid,createuserdeptcode,createuserorgcode";
                pagination.p_tablename = "HRS_EVERYDAYPATROL";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                pagination.conditionJson += string.Format(" and((createuserid='{0}' and AffirmState=0) or (AffirmUserId like '%{1}%' and AffirmState=1))", userId, curUser.Account);

                DataTable dt = everydaypatrolbll.GetPageList(pagination, null);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region 待办事项(省公司级)
        /// <summary>
        /// 待办事项(省公司级)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getDelayItemsForGroup([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                int safechecknum = 0; //待执行安全检查数
                int appoveproblemnum = 0;//待评估隐患数
                int modifypostphoneproblemnum = 0;//待延期审（核）批
                int reviewproblemnum = 0;//待验收隐患
                int recheckproblemnum = 0;//待复查验证隐患
                int assessproblemnum = 0;//待整改效果评估

                int illegalNum = 0; //违章次数
                int reviewIllegalNum = 0; //待验收的违章数

                //SaftyCheckDataBLL saftbll = new SaftyCheckDataBLL();
                //int[] countcheck = saftbll.GetCheckCount(user, 0);
                //safechecknum = countcheck.Sum();
                //隐患待办事项
                var data = new HTBaseInfoBLL().QueryHidBacklogRecord("0", user.UserId);
                if (data.Rows.Count == 8)
                {
                    appoveproblemnum = int.Parse(data.Rows[0]["pnum"].ToString());//待评估隐患数
                    modifypostphoneproblemnum = int.Parse(data.Rows[2]["pnum"].ToString());//待延期审（核）批
                    reviewproblemnum = int.Parse(data.Rows[3]["pnum"].ToString());//待验收隐患
                    assessproblemnum = int.Parse(data.Rows[4]["pnum"].ToString());//待整改效果评估
                    recheckproblemnum = int.Parse(data.Rows[6]["pnum"].ToString());//待复查验证隐患
                }
                List<int> list = new DesktopBLL().GetSafetyCheckForGroup(user);

                int deptNum = 0; //一级风险超过3个的电厂数量
                int oneRiskNum = 0;//重大风险总数
                int twoRiskNum = 0;//较大风险总数
                if (user.RoleName.Contains("省级用户"))
                {
                    DataTable dt = riskassessbll.GetIndexRiskTarget("重大风险", user, 1);
                    DataTable dt1 = riskassessbll.GetIndexRiskTarget("较大风险", user, 2);
                    DataTable dt2 = riskassessbll.GetIndexRiskTarget("重大风险", user, 3);
                    deptNum = dt2.Rows.Count;
                    if (dt.Rows.Count > 0)
                    {
                        oneRiskNum = Convert.ToInt32(dt.Compute("Sum(num)", ""));
                    }
                    else
                    {
                        oneRiskNum = 0;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        twoRiskNum = Convert.ToInt32(dt1.Compute("Sum(num)", ""));
                    }
                    else
                    {
                        twoRiskNum = 0;
                    }
                }
                int bigHtNum = 0;//重大隐患数量
                int overdueHtNum = 0;//逾期未整改隐患数量
                int htNum = 0;//省公司发现隐患
                int checkNum = 0;//检查次数
                checkNum = list[0];
                safechecknum = list[1];
                List<decimal> list1 = new DesktopBLL().GetWarnItems(user);
                decimal reformPercentNum = list1[1];//隐患整改率低于80%的电厂数
                decimal bigHtFactoryNum = list1[3];//存在重大隐患电厂数
                //重要指标之重大隐患、逾期未整改隐患、省公司发现的隐患
                list = new DesktopBLL().GetHtForGroup(user);
                bigHtNum = list[0]; //重大隐患数
                overdueHtNum = list[2];//逾期未整改隐患数
                htNum = list[3]; //省公司排查的隐患数量
                illegalNum = list[6]; //违章次数
                reviewIllegalNum = list[5]; //待验收违章
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new
                    {
                        safeCheckNum = safechecknum,//待执行安全检查数
                        appoveProblemNum = appoveproblemnum,//待评估隐患
                        delayProblemNum = modifypostphoneproblemnum,//待延期审（核）批
                        reviewProblemNum = reviewproblemnum,//待验收隐患
                        recheckProblemNum = recheckproblemnum,//待复查验证隐患 
                        assessProblemNum = assessproblemnum,//待整改效果评估

                        bigHtNum = bigHtNum,//重大隐患数量
                        overdueHtNum = overdueHtNum,//逾期未整改隐患数量
                        htNum = htNum,//省公司发现隐患
                        checkNum = checkNum,//检查次数

                        reformPercentNum = reformPercentNum,//隐患整改率低于80%的电厂数
                        bigHtFactoryNum = bigHtFactoryNum,//存在重大隐患电厂数

                        reviewIllegalNum = reviewIllegalNum, //待验收违章
                        illegalNum = illegalNum, //违章次数

                        deptNum = deptNum,
                        oneRiskNum = oneRiskNum,
                        twoRiskNum = twoRiskNum
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取预警详情
        /// <summary>
        /// 获取预警详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object getWarningInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userId = dy.userid;
            string startmonth = dy.data.startmonth;  //检索对应得月份  格式"2017-11"

            string starDate = startmonth + "-01"; //起始时间
            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator user = OperatorProvider.Provider.Current();

            decimal riskscore = 0; //安全风险得分
            decimal safetyscore = 0; //安全检查得分
            decimal hiddenscore = 0; //隐患排查得分

            /*计算对应的分项总分*/
            RiskBLL riskBLL = new RiskBLL();
            SaftyCheckDataRecordBLL saftybll = new SaftyCheckDataRecordBLL();
            if (user.RoleName.Contains("省级"))
            {

                DataTable dtDepts = new DepartmentBLL().GetAllFactory(user);
                foreach (DataRow dr in dtDepts.Rows)
                {
                    user = new Operator
                    {
                        OrganizeId = dr[2].ToString(),
                        OrganizeCode = dr[0].ToString(),
                        DeptCode = dr[0].ToString()
                    };
                    //安全风险得分 增加自己的总分(非权重计算的)

                    riskscore += riskBLL.GetRiskValueByTime(user, starDate);

                    //安全检查得分 增加自己的总分(非权重计算的)

                    safetyscore += saftybll.GetSafeCheckWarningM(user, starDate, 1);

                    //隐患排查得分 增加自己的总分(非权重计算的)
                    hiddenscore += new HTBaseInfoBLL().GetHiddenWarning(user, starDate);

                }
                riskscore = riskscore / dtDepts.Rows.Count;
                safetyscore = safetyscore / dtDepts.Rows.Count;
                hiddenscore = hiddenscore / dtDepts.Rows.Count;

                riskscore = Math.Round(riskscore, 2);
                safetyscore = Math.Round(safetyscore, 2);
                hiddenscore = Math.Round(hiddenscore, 2);
            }
            //获取结果集
            DataTable dt = new DataTable();
            dt.Columns.Add("indexname");  //分项指标
            dt.Columns.Add("score"); //总得分
            dt.Columns.Add("weight"); //权重
            dt.Columns.Add("lastscore"); //加权重所得
            decimal totalscore = 0; //合计总得分
            ClassificationBLL classificationbll = new ClassificationBLL();
            var list = classificationbll.GetList(user.OrganizeId);
            if (list.Count() == 0)
            {
                list = classificationbll.GetList("0");
            }
            foreach (ClassificationEntity entity in list)
            {
                DataRow row = dt.NewRow();
                row["indexname"] = entity.ClassificationIndex;
                row["weight"] = entity.WeightCoeffcient;
                switch (entity.ClassificationCode)
                {
                    case "01": //隐患排查
                        row["score"] = hiddenscore;

                        break;
                    case "02"://安全检查
                        row["score"] = safetyscore;
                        break;
                    case "03": //安全风险
                        row["score"] = riskscore;
                        break;
                }
                row["lastscore"] = Convert.ToDecimal(row["score"].ToString()) * Convert.ToDecimal(entity.WeightCoeffcient); //增加权重系数，计算得分
                totalscore += Convert.ToDecimal(row["lastscore"].ToString()); //累加权重系数后所得分项总分
                dt.Rows.Add(row);
            }

            return new
            {
                code = 0,
                count = 0,
                data = dt,
                score = totalscore
            };

        }
        #endregion

        #region 省级用户获取首页指标
        /// <summary>
        /// 省级用户获取首页指标
        /// actiontype 1:重大风险指标 2 较大风险指标 3：重大风险大于3的指标
        /// </summary>
        [HttpPost]
        public object GetIndexTarget([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                if (null == currUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //int pageSize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 10; //每页的记录数

                //int pageIndex = res.Contains("pageindex") ? int.Parse(dy.pageindex.ToString()) : 1;  //当前页索引

                int actiontype = res.Contains("actiontype") ? int.Parse(dy.data.actiontype.ToString()) : 0; //查询类型
                DataTable dt = new DataTable();
                switch (actiontype)
                {
                    case 1:
                        dt = riskassessbll.GetIndexRiskTarget("重大风险", currUser, 1);
                        break;
                    case 2:
                        dt = riskassessbll.GetIndexRiskTarget("较大风险", currUser, 2);
                        break;
                    case 3:
                        dt = riskassessbll.GetIndexRiskTarget("重大风险", currUser, 3);
                        break;
                    case 4:  //重大隐患-按电厂排列(省级)
                        dt = htbaseinfobll.GetImportantIndexForProvincial(1, currUser);
                        break;
                    case 5: //逾期未整改隐患-按电厂排列(省级)
                        dt = htbaseinfobll.GetImportantIndexForProvincial(2, currUser);
                        break;
                    case 6: //省公司发现的隐患-按电厂排列(省级)
                        dt = htbaseinfobll.GetImportantIndexForProvincial(3, currUser);
                        break;
                    case 7: //违章数量-按电厂排列(省级)
                        dt = htbaseinfobll.GetImportantIndexForProvincial(4, currUser);
                        break;
                    case 8: //安全检查次数(省级)
                        dt = new DesktopBLL().GetFactoryCheckListForGroup(currUser);
                        break;
                    default:
                        break;
                }
                return new { code = 0, count = dt.Rows.Count, info = "获取成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 获取首页分数
        /// <summary>
        /// 获取首页分数
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getHomeWarning([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userId = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator user = OperatorProvider.Provider.Current();

            DesktopBLL desktopbll = new DesktopBLL();
            RiskBLL riskBLL = new RiskBLL();//安全风险
            SaftyCheckDataRecordBLL saBLL = new SaftyCheckDataRecordBLL();//安全检查
            HTBaseInfoBLL htBLL = new HTBaseInfoBLL();//事故隐患
            ClassificationBLL classBLL = new ClassificationBLL();
            DataItemDetailBLL itemBLL = new DataItemDetailBLL();
            List<ClassificationEntity> list = classBLL.GetList(user.OrganizeId).ToList();
            if (list.Count == 0)
            {
                list = classBLL.GetList("0").ToList();
            }
            decimal totalScore = 0;
            decimal yhscore = 0;
            decimal aqjcscore = 0;
            decimal aqfxscore = 0;
            decimal yhzlstardard = 0;
            decimal aqjcstardard = 0;
            decimal aqfxstardard = 0;
            decimal qualifiedscore = 0;

            decimal yhscore1 = 0;
            decimal aqjcscore1 = 0;
            decimal aqfxscore1 = 0;

            string val = itemBLL.GetItemValue("基础预警区间分值设置");
            int count = 0;
            if (user.RoleName.Contains("省级"))
            {

                DataTable dtDepts = new DepartmentBLL().GetAllFactory(user);
                foreach (DataRow dr in dtDepts.Rows)
                {
                    user = new Operator
                    {
                        OrganizeId = dr[2].ToString(),
                        OrganizeCode = dr[0].ToString(),
                    };
                    totalScore += desktopbll.GetScore(user, "");


                    //计算事故隐患总得分
                    decimal score = htBLL.GetHiddenWarning(user, "");
                    totalScore += score * decimal.Parse(list[0].WeightCoeffcient);
                    yhscore1 += score * decimal.Parse(list[0].WeightCoeffcient);
                    yhzlstardard += score;
                    //计算安全检查总得分
                    score = saBLL.GetSafeCheckSumCount(user);
                    totalScore += score * decimal.Parse(list[1].WeightCoeffcient);
                    aqjcscore1 += score * decimal.Parse(list[1].WeightCoeffcient);
                    aqjcstardard += score;
                    //计算安全风险总得分
                    score = riskBLL.GetRiskValueByTime(user, "");
                    totalScore += score * decimal.Parse(list[2].WeightCoeffcient);
                    aqfxscore1 += score * decimal.Parse(list[2].WeightCoeffcient);
                    aqfxstardard += score;

                }

                totalScore = totalScore / dtDepts.Rows.Count;
                yhscore = yhscore1 / dtDepts.Rows.Count;
                aqjcscore = aqjcscore1 / dtDepts.Rows.Count;
                aqfxscore = aqfxscore1 / dtDepts.Rows.Count;

                yhzlstardard = yhzlstardard / dtDepts.Rows.Count;
                aqjcstardard = aqjcstardard / dtDepts.Rows.Count;
                aqfxstardard = aqfxstardard / dtDepts.Rows.Count;
            }
            else
            {
                //计算事故隐患总得分
                decimal score = htBLL.GetHiddenWarning(user, "");
                totalScore = score * decimal.Parse(list[0].WeightCoeffcient);
                yhscore = score * decimal.Parse(list[0].WeightCoeffcient);
                yhzlstardard = score;
                //计算安全检查总得分
                score = saBLL.GetSafeCheckSumCount(user);
                totalScore += score * decimal.Parse(list[1].WeightCoeffcient);
                aqjcscore = score * decimal.Parse(list[1].WeightCoeffcient);
                aqjcstardard = score;
                //计算安全风险总得分
                score = riskBLL.GetRiskValueByTime(user, "");
                totalScore += score * decimal.Parse(list[2].WeightCoeffcient);
                aqfxscore = score * decimal.Parse(list[2].WeightCoeffcient);
                aqfxstardard = score;
            }
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
                qualifiedscore = decimal.Parse(arr[2].Split(',')[0].ToString());
            }
            string warningtext = string.Empty;
            switch (count)
            {
                case 0:
                    warningtext = "危险";
                    break;
                case 1:
                    warningtext = "较危险";
                    break;
                case 2:
                    warningtext = "较安全";
                    break;
                case 3:
                    warningtext = "安全";
                    break;
            }
            bool isyhzlsucess = Math.Round(yhzlstardard, 1) >= qualifiedscore ? true : false;
            bool isaqjcsucess = Math.Round(aqjcstardard, 1) >= qualifiedscore ? true : false;
            bool isaqfxsucess = Math.Round(aqfxstardard, 1) >= qualifiedscore ? true : false;

            return new { code = 0, data = new { score = Math.Round(totalScore, 1), yhscore = Math.Round(yhscore, 1), aqjcscore = Math.Round(aqjcscore, 1), aqfxscore = Math.Round(aqfxscore, 1), index = count, warningtext = warningtext, isyhzlsucess = isyhzlsucess, isaqjcsucess = isaqjcsucess, isaqfxsucess = isaqfxsucess }, count = 0 };
        }
        #endregion

        #region 获取天气概况
        /// <summary>
        /// 获取天气概况
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getWeather([FromBody]JObject json)
        {
            string jsonstr = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(jsonstr);

            string userId = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator user = OperatorProvider.Provider.Current();

            DataItemBLL itemBll = new DataItemBLL();

            string wearherData = "";

            ERCHTMS.Entity.SystemManage.DataItemEntity entity = itemBll.GetEntity("0000111122223333");
            if (entity == null)
            {
                WeatherbService.WeatherService.WeatherWebService w1 = new WeatherbService.WeatherService.WeatherWebService();
                string[] res1 = new string[23];
                string cityname1 = dy.data.cityname;
                res1 = w1.getWeatherbyCityName(cityname1);
                string wearherStr1 = res1[10];

                wearherData = wearherStr1;

                entity = new DataItemEntity();
                entity.ItemCode = "Weather";
                entity.ItemName = wearherStr1;
                entity.ItemId = "0000111122223333";
                entity.Description = DateTime.Now.ToString("yyyy-MM-dd");
                itemBll.SaveForm("", entity);
            }
            else
            {
                if (entity.Description == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    wearherData = entity.ItemName;
                }
                else
                {
                    WeatherbService.WeatherService.WeatherWebService w = new WeatherbService.WeatherService.WeatherWebService();
                    string[] res = new string[23];
                    string cityname = dy.data.cityname;
                    res = w.getWeatherbyCityName(cityname);
                    string wearherStr = res[10];

                    wearherData = wearherStr;

                    entity.ItemName = wearherStr;
                    entity.Description = DateTime.Now.ToString("yyyy-MM-dd");
                    itemBll.SaveForm("0000111122223333", entity);
                }
            }
            return new { Code = 0, Count = 1, Info = "获取数据成功", data = wearherData };
        }
        #endregion

        #region 获取五型班组首页数据
        /// <summary>
        /// 获取五型班组数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getFiveTypeInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {
                if (null != curUser)
                {
                    //安全检查
                    SaftyCheckDataBLL saftycheckdatabll = new SaftyCheckDataBLL();
                    var dtcheck = saftycheckdatabll.GetCheckStat(curUser, 2);
                    decimal checknum = 0;//安全检查次数
                    if (dtcheck.Rows.Count > 0) { checknum = decimal.Parse(dtcheck.Rows[0][0].ToString()); }

                    //隐患排查
                    HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
                    decimal registernum = 0; //登记的隐患
                    decimal changenum = 0; //整改的隐患
                    decimal lllegalnum = 0;  //违章的次数
                    decimal drillplannum = 0; //应急演练数量
                    string code = string.Empty;
                    if (curUser.RoleName.Contains("公司级") || curUser.RoleName.Contains("厂级"))
                    {
                        code = curUser.OrganizeCode;
                    }
                    else
                    {
                        code = curUser.DeptCode;
                    }
                    var dtht = htbaseinfobll.GetAppHidStatistics(code, 4);//登记的隐患数量
                    if (dtht.Rows.Count > 0)
                    {
                        registernum = decimal.Parse(dtht.Rows[0][0].ToString()); //登记的隐患数量
                        changenum = decimal.Parse(dtht.Rows[1][0].ToString()); //整改的隐患数量
                        lllegalnum = decimal.Parse(dtht.Rows[2][0].ToString()); //违章的次数
                        drillplannum = decimal.Parse(dtht.Rows[3][0].ToString()); //应急演练数量
                    }
                    return new
                    {
                        Code = 0,
                        Count = 0,
                        Info = "获取数据成功",
                        data = new
                        {
                            checknum = checknum,//安全检查数
                            registernum = registernum,//登记的隐患
                            changenum = changenum, //整改的隐患
                            lllegalnum = lllegalnum,//违章的次数
                            drillplannum = drillplannum//应急演练数量
                        }
                    };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "用户不存在!" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }
        #endregion

        #region 获取终端首页数据
        /// <summary>
        /// 获取五型班组数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getTerminalDataInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                int category = (int)dy.data;
                List<IndexData> data = new List<IndexData>();

                if (null != curUser)
                {
                    //安全检查
                    SaftyCheckDataBLL saftycheckdatabll = new SaftyCheckDataBLL();
                    var dtcheck = saftycheckdatabll.GetCheckStat(curUser, category);
                    decimal checknum = 0;//安全检查次数
                    if (dtcheck.Rows.Count > 0) { checknum = decimal.Parse(dtcheck.Rows[0][0].ToString()); }
                    data.Add(new IndexData() { key = "ZD_AQJCSL", value = checknum.ToString() });
                    //隐患排查
                    HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
                    decimal lllegalnum = 0;  //违章的次数
                    decimal drillplannum = 0; //应急演练数量
                    string code = string.Empty;
                    if (curUser.RoleName.Contains("公司级") || curUser.RoleName.Contains("厂级"))
                    {
                        code = curUser.OrganizeCode;
                    }
                    else
                    {
                        code = curUser.DeptCode;
                    }
                    var dtht = htbaseinfobll.GetAppHidStatistics(code, 4, category);//登记的隐患数量
                    if (dtht.Rows.Count > 0)
                    {
                        lllegalnum = decimal.Parse(dtht.Rows[2][0].ToString()); //违章的次数
                        drillplannum = decimal.Parse(dtht.Rows[3][0].ToString()); //应急演练数量

                        data.Add(new IndexData() { key = "ZD_WZCS", value = lllegalnum.ToString() });
                        data.Add(new IndexData() { key = "ZD_YJYLSL", value = drillplannum.ToString() });
                    }
                    return new
                    {
                        Code = 0,
                        Count = data.Count,
                        Info = "获取数据成功",
                        data = data
                    };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "用户不存在!" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }
        #endregion

        #region 获取班组平台指标信息
        /// <summary>
        /// 获取班组平台指标信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getBZPlatformIndexInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            List<IndexData> data = new List<IndexData>();
            try
            {
                if (null != curUser)
                {
                    string code = string.Empty;
                    if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户"))
                    {
                        code = curUser.OrganizeCode;
                    }
                    else
                    {
                        code = curUser.DeptCode;
                    }
                    var dtht = htbaseinfobll.GetAppHidStatistics(code, 5);//登记的隐患数量
                    data.Add(new IndexData { key = "SK_WZGYH", value = dtht.Rows[0][0].ToString() });//未整改隐患 
                    data.Add(new IndexData { key = "SK_WZGWZ", value = dtht.Rows[1][0].ToString() }); //未整改违章
                    data.Add(new IndexData { key = "SK_YJYL", value = dtht.Rows[2][0].ToString() }); //应急演练 
                    data.Add(new IndexData { key = "SK_YHSL", value = dtht.Rows[3][0].ToString() }); //隐患数量 
                    data.Add(new IndexData { key = "SK_WZCS", value = dtht.Rows[4][0].ToString() }); //违章次数 
                    data.Add(new IndexData { key = "SK_AQJC ", value = dtht.Rows[5][0].ToString() });//安全检查 
                    data.Add(new IndexData { key = "SK_YBYH", value = dtht.Rows[6][0].ToString() });//一般隐患
                    data.Add(new IndexData { key = "SK_ZDYH", value = dtht.Rows[7][0].ToString() }); //重大隐患 
                    data.Add(new IndexData { key = "SK_YHZGL", value = dtht.Rows[8][0].ToString() });//按创建部门隐患整改率
                    data.Add(new IndexData { key = "SK_YBYHZGL", value = dtht.Rows[9][0].ToString() }); //一般隐患整改率 
                    data.Add(new IndexData { key = "SK_ZDYHZGL", value = dtht.Rows[10][0].ToString() }); //重大隐患整改率
                    data.Add(new IndexData { key = "SK_WZZGL", value = dtht.Rows[11][0].ToString() }); //按创建部门违章整改率
                    data.Add(new IndexData { key = "SK_PZJD", value = dtht.Rows[12][0].ToString() }); //旁站监督
                    data.Add(new IndexData { key = "SK_ZGYHZGL", value = dtht.Rows[13][0].ToString() }); //按整改部门隐患整改率
                    data.Add(new IndexData { key = "SK_ZGWZZGL", value = dtht.Rows[14][0].ToString() }); //按整改部门违章整改率

                    data.Add(new IndexData { key = "SK_BYWZGYH", value = dtht.Rows[15][0].ToString() }); //本月未整改隐患
                    data.Add(new IndexData { key = "SK_BYYZGYH", value = dtht.Rows[16][0].ToString() }); //本月已整改隐患
                    data.Add(new IndexData { key = "SK_BYWZGWZ", value = dtht.Rows[17][0].ToString() }); //本月未整改违章
                    data.Add(new IndexData { key = "SK_BYYZGWZ", value = dtht.Rows[18][0].ToString() }); //本月已整改违章
                    return new
                    {
                        Code = 0,
                        Count = 0,
                        Info = "获取数据成功",
                        data = data
                    };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "用户不存在!" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }
        #endregion

        #region 班组端统计
        /// <summary>
        /// 班组端统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getBZPlatformStatisticsInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {
                if (null != curUser)
                {
                    string mode = res.Contains("mode") ? dy.data.mode.ToString() : "";
                    DataTable data = new DataTable();
                    switch (mode)
                    {
                        case "1":  //未闭环隐患统计-按责任单位统计
                            data = desktopbll.GetNoCloseLoopHidStatistics(curUser, 1);
                            break;
                        case "2":  //未闭环隐患统计-按区域统计
                            data = desktopbll.GetNoCloseLoopHidStatistics(curUser, 2);
                            break;
                        case "3":  //未闭环隐患统计-按专业统计
                            data = desktopbll.GetNoCloseLoopHidStatistics(curUser, 3);
                            break;
                        case "4":  //隐患整改率统计
                            data = desktopbll.GetHiddenChangeForLeaderCockpit(curUser);
                            break;
                        case "5":  //各部门未闭环违章统计
                            data = desktopbll.GetNoCloseLoopLllegalStatistics(curUser);
                            break;
                        case "6":  //各部门违章整改率统计
                            data = desktopbll.GetLllegalChangeForLeaderCockpit(curUser);
                            break;
                    }
                    return new
                    {
                        Code = 0,
                        Count = 0,
                        Info = "获取数据成功",
                        data = data
                    };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "用户不存在!" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }
        #endregion

        #region 获取最新的曝光信息
        /// <summary>
        /// 获取最新的曝光信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getNewExposureInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {
                if (null != curUser)
                {
                    var dt = desktopbll.GetExposureInfo(curUser);
                    return new
                    {
                        Code = 0,
                        Count = 0,
                        Info = "获取数据成功",
                        data = dt
                    };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "用户不存在!" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }
        #endregion

        #region 获取培训平台待办接口
        [HttpPost]
        public object GetSafetyTrainSystem([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator user = OperatorProvider.Provider.Current();

            string DelayUrl = new DataItemDetailBLL().GetItemValue("TrainServiceDelayUrl");
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
            string url = new DataItemDetailBLL().GetItemValue("GetTrainServiceDelayUrl");
            int num = 0;
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    string result = wc.UploadString(new Uri(url + "?user_account=" + user.Account), "post");
                    dynamic dyObject = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Dynamic.ExpandoObject>(result);
                    if (dyObject.meta.success == true)
                    {
                        num = Int32.Parse(dyObject.data.msg_count);
                    }
                    else
                    {
                        return new
                        {
                            Code = 0,
                            Info = dyObject.meta.message,
                            data = new
                            {
                                delayurl = DelayUrl,
                                num = 0
                            }
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new
                    {
                        Code = 0,
                        Info = ex.ToString(),
                        data = new
                        {
                            delayurl = DelayUrl,
                            num = 0
                        }
                    };
                }
            }
            return new
            {
                Code = 0,
                Count = 0,
                Info = "获取数据成功",
                data = new
                {
                    delayurl = DelayUrl,
                    num = num
                }
            };
        }
        #endregion


        [HttpPost]
        public object getConfigInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userId = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator user = OperatorProvider.Provider.Current();
            DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
            string val = dataitemdetailbll.GetItemValue("是否提醒", "dbtx");
            DataTable dtStatus = new DataTable();
            string isRemind = "1";
            if (val == "1")
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                dtStatus = deptBll.GetDataTable(string.Format("select status from BIS_USERWAIWORK where userid='{0}' and status=1", userId));
                if(dtStatus.Rows.Count>0)
                {
                    isRemind = "0";
                }

            }
            else
            {
                isRemind = "0";
            }
            val = dataitemdetailbll.GetItemValue("个人是否可以关闭待办提醒", "dbtx");
            string isClose = "0";
            if (val == "1")
            {
                isClose = "1";
            }
            else
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                dtStatus = deptBll.GetDataTable(string.Format("select status from BIS_USERWAIWORK where userid='{0}'", userId));
                isClose = dtStatus.Rows.Count == 0 ? "0" :dtStatus.Rows[0][0].ToString();

            }
            //string userMode = "";
            //string roleCode = dataitemdetailbll.GetItemValue("HidApprovalSetting");
            //string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");
            //string[] pstr = HidApproval.Split('#');  //分隔机构组

            //foreach (string strArgs in pstr)
            //{
            //    string[] str = strArgs.Split('|');

            //    //当前机构相同，且为本部门安全管理员验证  第一种 层层上报
            //    if (str[0].ToString() == user.OrganizeId && str[1].ToString() == "0")
            //    {
            //        /*************临时使用，后续需要进行调整，原有隐患基于角色，较为固定，后期则废弃*************/
            //        //WfControlObj wfentity = new WfControlObj();
            //        //wfentity.businessid = ""; //
            //        //wfentity.startflow = "隐患评估";
            //        //wfentity.submittype = "上报";
            //        //wfentity.rankname = "一般隐患";
            //        //wfentity.user = operators;
            //        //wfentity.mark = "厂级隐患排查"; //厂级隐患排查
            //        //wfentity.isvaliauth = true;

            //        ////获取下一流程的操作人
            //        //WfControlResult result = new  WfControlBLL().GetWfControl(wfentity);
            //        //bool ishaveapproval = result.ishave;  //具有评估权限的人

            //        int count = new UserBLL().GetUserListByRole(user.DeptCode, roleCode, user.OrganizeId).ToList().Where(p => p.UserId == user.UserId).Count();
            //        if (count > 0)//包含安全管理员、负责人
            //        {
            //            userMode = "0";
            //        }
            //        else
            //        {
            //            userMode = "1";
            //        }

            //        break;
            //    }
            //    if (str[0].ToString() == user.OrganizeId && str[1].ToString() == "1")
            //    {
            //        //获取指定部门的所有人员
            //        int count = new UserBLL().GetUserListByDeptCode(str[2].ToString(), null, false, user.OrganizeId).ToList().Where(p => p.UserId == user.UserId).Count();
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
            //if (user.RoleName.Contains("省级用户"))
            //{
            //    userMode = "4";
            //}
            //string rankArgs = dataitemdetailbll.GetItemValue("GeneralHid"); //一般隐患
            //string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            //string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            //string CompanyRole = hidPlantLevel + "," + hidOrganize;
            //UserBLL userBLL = new UserBLL();
            //var userList = userBLL.GetUserListByDeptCode(user.DeptCode, CompanyRole, false, user.OrganizeId).Where(p => p.UserId == user.UserId).ToList();

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
            //string pricipalCode = dataitemdetailbll.GetItemValue("HidPrincipalSetting");
            //IList<UserEntity> ulist = userBLL.GetUserListByRole(user.DeptCode, pricipalCode, user.OrganizeId).ToList();
            ////返回的记录数,大于0，标识当前用户拥有部门负责人身份，反之则无
            //int uModel = ulist.Where(p => p.UserId == user.UserId).Count();
            ////用于违章的用户标记
            //string mark = string.Empty;
            //Operator operators = new Operator {
            //     UserId=user.UserId,
            //     DeptCode=user.DeptCode,
            //     DeptId=user.DeptId,
            //     OrganizeCode=user.OrganizeCode,
            //     OrganizeId=user.OrganizeId
            //};
            //mark = userBLL.GetSafetyAndDeviceDept(operators); //1 安全管理部门， 2 装置部门   5.发包部门

            //string isPrincipal = userBLL.HaveRoleListByKey(user.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0 ? "3" : ""; //第一级核准人
            //if (!string.IsNullOrEmpty(isPrincipal))
            //{
            //    if (!string.IsNullOrEmpty(mark))
            //    {
            //        mark = mark + "," + isPrincipal;
            //    }
            //    else
            //    {
            //        mark = isPrincipal;
            //    }
            //}
            //string isEpiboly = userBLL.HaveRoleListByKey(operators.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0 ? "4" : "";  //承包商

            //if (!string.IsNullOrEmpty(isEpiboly))
            //{
            //    if (!string.IsNullOrEmpty(mark))
            //    {
            //        mark = mark + "," + isEpiboly;
            //    }
            //    else
            //    {
            //        mark = isEpiboly;
            //    }
            //}
            ////国电新疆红雁池专用
            //string GDXJ_HYC_ORGCODE = dataitemdetailbll.GetItemValue("GDXJ_HYC_ORGCODE");
            ////国电新疆红雁池专用
            //int IsGdxjUser = user.OrganizeCode == GDXJ_HYC_ORGCODE ? 1 : 0;
            //string webPath = dataitemdetailbll.GetItemValue("imgPath");
            //string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
            //string signUrl = "";
            //string qrCodeImgUrl = webUrl + "/Resource/AppFile/download.jpg";
            //UserEntity userEntity = userBLL.GetEntity(user.UserId);
            //if (!string.IsNullOrEmpty(userEntity.SignImg))
            //{
            //    if (userEntity.SignImg.ToLower().Trim().StartsWith("http://"))
            //    {
            //        signUrl = userEntity.SignImg;
            //    }
            //    else
            //    {
            //        string fname = "";
            //        string sImg = "";
            //        if (userEntity.SignImg.ToLower().Contains("/resource/sign/"))
            //        {
            //            fname = userEntity.SignImg.Replace("/", "\\");
            //            string name = userEntity.SignImg.Substring(userEntity.SignImg.LastIndexOf("/") + 1);
            //            sImg = "s" + name.Replace("/", "\\");
            //        }
            //        else
            //        {
            //            fname = "\\Resource\\sign\\" + userEntity.SignImg.Replace("/", "\\");
            //            sImg = "\\Resource\\sign\\s" + userEntity.SignImg.Replace("/", "\\");
            //        }

            //        if (File.Exists(webPath + sImg))
            //        {
            //            signUrl = webUrl + sImg.Replace("\\", "/");
            //        }
            //        else
            //        {
            //            if (File.Exists(webPath + fname))
            //            {
            //                signUrl = webUrl + fname.Replace("\\", "/");
            //            }
            //        }
            //    }
            //}
            //string isCache = dataitemdetailbll.GetItemValue(userEntity.OrganizeCode, "CacheSafetyCheck");

            //string appTrainPic = dataitemdetailbll.GetItemValue("PicUrl", "Train");
            //string appTrainUrl = dataitemdetailbll.GetItemValue("ApiUrl", "Train");
            //string appTrainPwd = dataitemdetailbll.GetItemValue("PwdKey", "Train");
            //string appTrainFix = dataitemdetailbll.GetItemValue("AccountFix", "Train");
            //isCache = string.IsNullOrEmpty(isCache) ? "0" : isCache;
            return new
            {
                code = 0,
                info = "获取数据成功",
                data = new
                {
                    //app_train_pic = appTrainPic,
                    //app_train_url = appTrainUrl,
                    //app_train_pwdkey = appTrainPwd,
                    //app_train_account_postfix = appTrainFix,
                    //rankargs = rankArgs,
                    //isprincipal = isPrincipal,
                    //identifyid = userEntity.IdentifyID,
                    //mark = mark,
                    //wfmode=userMode,
                    //signurl = signUrl,
                    //isgdxjuser = IsGdxjUser,
                    //qrimgurl = qrCodeImgUrl,
                    //iscachesafetycheck = isCache,
                    isremind = isRemind,
                    isclose = isClose
                }
            };
        }
    }
        
}