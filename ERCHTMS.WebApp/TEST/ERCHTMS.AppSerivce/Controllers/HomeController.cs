using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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
                int reviewproblemnum = 0;//待复查验收的隐患数
                int assessproblemnum = 0;//待评估隐患数
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
                int reviewproblemnum = 0;//待复查验收的隐患数
                int assessproblemnum = 0;//待评估隐患数
                int overtimempnum = 0;//逾期未整改隐患
                int uploadHtNum = 0;   //我上传的隐患
                int planNum = 0;//进行中的风险辨识计划数
                int dailyexamineNum = 0;//日常考核待审核数


                RiskPlanBLL planBll = new RiskPlanBLL();
                planNum = planBll.GetPlanCount(user, 10);
                SaftyCheckDataBLL saftbll = new SaftyCheckDataBLL();
                int[] countcheck = saftbll.GetCheckCount(user, 0);
                safechecknum = countcheck.Sum();
                DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
                dailyexamineNum = dailyexaminebll.CountIndex(user);

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

                list = desktop.GetScaffoldNum(user);
                int waitscaffoldchecknum = list[0];//脚手架待验收
                int waitscaffoldauditnum = list[1];//脚手架待审核


                int specialEquNum = desktop.GetEquimentNum(user);//特种设备数量
                int dangerProjectNum = desktop.GetProjectNum(user);//施工中的危大工程数量

                list = desktop.GetlllegalNum(user);
                int illegalNum = list[0];//违章数量
                int approveillegalnum = list[1];//待核准的违章数量
                int verifyillegalnum = list[2];//待整改的违章数量
                int reviewillegalnum = list[3];//待验收的违章数量
                int overdueIllegalNum = list[4];//逾期未整改违章数量
                decimal illegalCompleteRatio = desktop.GetlllegalRatio(user);//违章整改率



                int meetnum = desktop.GetMeetNum(user.UserId);//安全会议数量

                int safetynum = desktop.GetSafetyChangeNum(user);//安全设施变动审待核数量
                int risktrainnum = 0;//风险预知训练

                list = new OutprojectblacklistBLL().ToAuditOutPeoject(user);//外包工程
                int wbscla = list[2];//待审（核）批三措两案
                int wbrcsq = list[5];//待审（核）批入厂许可
                int wbkgsq = list[6];//待审（核）批开工申请
                //data.Add("WBDWZZ", wb[0]);//单位资质
                //data.Add("WBRYZZ", wb[1]);//人员资质
                //data.Add("WBSCLA", wb[2]);//三措两案
                //data.Add("WBDDSB", wb[3]);//特种设备
                //data.Add("WBTZSB", wb[4]);//电动设备
                //data.Add("WBRCSQ", wb[5]);//入场许可
                //data.Add("WBKGSQ", wb[6]);//开工申请


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
                        reviewProblemNum = reviewproblemnum,//待复查验收的隐患数
                        delayProblemNum = modifypostphoneproblemnum,//待审(核)批整改延期隐患数
                        assessProblemNum = assessproblemnum,//待整改效果评估隐患
                        assessPlanNum = planNum,//待辨识的计划

                        dangerWorkNum = dangerworknum,//高风险作业数量
                        monitorDangerworkNum = monitordangerworknum,//高风险作业监督数量
                        waitConfirmationNum = waitconfirmationnum,//高风险通用待确认作业数量
                        waitApproveWorkNum = waitapproveworknum,//高风险通用待审核(批)作业数量
                        sidetaskallocation = sidetaskallocation,//待分配
                        sidesupervisionnum = sidesupervisionnum,//待监管

                        waitscaffoldcheckNum = waitscaffoldchecknum, //脚手架待验收
                        waitscaffoldauditNum = waitscaffoldauditnum, //脚手架待审核

                        dangerTrainNum = risktrainnum,//风险预知训练
                        approveIllegalNum = approveillegalnum,//待核准的违章数量
                        verifyIllegalNum = verifyillegalnum,//待整改的违章数量
                        reviewIllegalNum = reviewillegalnum, //待验收的违章数量


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
                        dailyexamineNum = dailyexamineNum
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
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
                int modifyproblemnum = 0;//待复查验证隐患
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
                    modifyproblemnum = int.Parse(data.Rows[6]["pnum"].ToString());//待复查验证隐患
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
                        modifyProblemNum = modifyproblemnum,//待复查验证隐患
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

            WeatherbService.WeatherService.WeatherWebService w = new WeatherbService.WeatherService.WeatherWebService();
            string[] res = new string[23];
            string cityname = dy.data.cityname;
            res = w.getWeatherbyCityName(cityname);
            string wearherStr = res[10];

            return new { Code = 0, Count = 1, Info = "获取数据成功", data = wearherStr };
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
                    var dtcheck = saftycheckdatabll.GetCheckStat(curUser);
                    decimal checknum = 0;//安全检查次数
                    if (dtcheck.Rows.Count > 0) { checknum = decimal.Parse(dtcheck.Rows[0][0].ToString()); }

                    //隐患排查
                    HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
                    decimal registernum = 0; //登记的隐患
                    decimal changenum = 0; //整改的隐患
                    decimal lllegalnum = 0;  //违章的次数
                    decimal drillplannum = 0; //应急演练数量
                    var dtht = htbaseinfobll.GetAppHidStatistics(curUser.DeptCode, 4);//登记的隐患数量
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
    }
}