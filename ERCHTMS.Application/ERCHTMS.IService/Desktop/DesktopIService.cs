using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.Home;
using ERCHTMS.Entity.Common;

namespace ERCHTMS.IService.Desktop
{
    /// <summary>
    /// 描 述：
    /// </summary>
    public interface DesktopIService
    {
        #region  通用版本的领导驾驶舱(电厂层级)

        #region  预警指标、安全指标
        /// <summary>
        /// 预警指标、安全指标
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<DesktopPageIndex> GetPowerPlantWarningIndex(Operator user);
        #endregion

        #region 获取整改率低于多少的电厂数据
        /// <summary>
        /// 获取整改率低于多少的电厂数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rankname"></param>
        /// <returns></returns>
        DataTable GetRectificationRateUnderHowMany(Operator user, string rankname, decimal num);
        #endregion

        #region  未闭环隐患统计
        /// <summary>
        /// 未闭环隐患统计
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetNoCloseLoopHidStatistics(Operator user, int mode);

        #region 隐患整改率
        /// <summary>
        /// 隐患整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetHiddenChangeForLeaderCockpit(Operator user);
        #endregion

        #endregion

        #region 各部门未闭环违章统计
        /// <summary>
        ///各部门未闭环违章统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        DataTable GetNoCloseLoopLllegalStatistics(Operator user);
        #endregion

        #region 各部门违章整改率统计
        /// <summary>
        /// 各部门违章整改率统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetLllegalChangeForLeaderCockpit(Operator user);
        #endregion

        #region 今日作业风险/高风险作业统计
        /// <summary>
        /// 今日作业风险/高风险作业统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetHighRiskWorkingForLeaderCockpit(Operator user, int mode);

        #endregion

        #endregion

        #region 获取数据
        /// <summary>
        /// 高风险作业分类统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetWorkTypeChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取日常安全检查统计数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetSafetyCheckOfEveryDay(ERCHTMS.Code.Operator user);

        #region 获取本年度趋势图(隐患总数、安全检查总数)
        /// <summary>
        /// 获取本年度趋势图(隐患总数、安全检查总数)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetTendencyChart(ERCHTMS.Code.Operator user);
        #endregion


        /// <summary>
        /// 隐患统计图表
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        string GetHTChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 按工程类型统计外包工程
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        DataTable GetProjectChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 按工程风险等级统计外包工程
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        DataTable GetProjectChartByLevel(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 外包人员数量变化趋势图
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        string GetProjectPersonChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 按隐患分类统计隐患
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        DataTable GetHTTypeChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取检查发现的隐患
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetCheckHtNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 隐患数量变化趋势图 
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        string GetHTChangeChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取通知公告
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetNotices(ERCHTMS.Code.Operator user);


        /// <summary>
        /// 获取一号岗大屏标题
        /// </summary>        
        /// <returns></returns>
        DataTable GetScreenTitle();

        /// <summary>
        /// 待分配的检查任务
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetAllotCheckCount(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取安全会议
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetMeets(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取安全动态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetTrends(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取红黑榜
        /// </summary>
        /// <param name="user"></param>
        ///  <param name="mode">0:红榜，1:黑榜</param>
        /// <returns></returns>
        DataTable GetRedBlack(ERCHTMS.Code.Operator user, int mode);
        /// <summary>
        /// 外包工程概况统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetProjectStat(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取危险源数量（依次为总数量，重大危险源数量）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetDangerSourceNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取人员违章信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetWZInfo(Pagination pagination, string queryJson); 

        DataTable GetWZInfo(string userid, int mode = 0);
        DataTable GetWZInfoByUserId(string userId,int mode=0);
        /// <summary>
        /// 获取未签到的会议数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetMeetNum(string userId);
        /// <summary>
        /// 获取施工中危大工程数
        /// <param name="user"></param>
        /// </summary>
        /// <returns></returns>
        int GetProjectNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取隐患数量(依次为总数量，重大隐患数量，一般隐患数量)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetHtNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取重大风险数量(依次为总数量，重大风险数量，较大风险数量，一般风险数量，低风险数量)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetRiskNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取外包工程数量信息（工程数量，外包人员在厂人数，外包工程总数，在场外包单位数，本月新进外包人员，外包单位违章次数）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetWBProjectNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取高风险作业(依次为总数量，待监督的数量）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetWorkNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取高危作业安全许可证审批待办（依次为高处作业、起重吊装作业、动土作业、断路作业、动火作业、盲板抽堵作业、受限空间作业、设备检修清理作业、待措施确认、待停电、待备案、待验收、待送电）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetJobSafetyCardNum(ERCHTMS.Code.Operator user);

        /// <summary>
        /// 获取脚手架统计（待验收、待审核）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetScaffoldNum(ERCHTMS.Code.Operator user);


        /// <summary>
        /// 获取消防水待审核(批)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetFireWaterNum(ERCHTMS.Code.Operator user);

        /// <summary>
        /// 获取特种设备数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetEquimentNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取违章数量信息（依次为：违章总数量、待核准、待整改、待验收、逾期未整改数量、逾期整改数量，待完善的违章）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetlllegalNum(ERCHTMS.Code.Operator user);

        /// <summary>
        /// 应急内容数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetDrillRecordNum(ERCHTMS.Code.Operator user);

                /// <summary>
        /// 获取问题数量信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
         List<int> GetQuestionNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取事故数量信息（依次为事故起数，死亡人数，重伤人员）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetAccidentNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取违章整改率
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        decimal GetlllegalRatio(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 按风险等级绘图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetRiskCounChart(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取安全事例
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetWorks(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 根据日期获取个人安全事例记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        DataTable GetWorkInfoByTime(ERCHTMS.Code.Operator user, string time);
        /// <summary>
        /// 获取各区域的最大风险等级信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        DataTable GetAreaStatus(ERCHTMS.Code.Operator user, string areaCode, int mode = 1);

        /// <summary>
        /// 获取各区域的最大风险等级信息(康巴什版本)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        List<AreaRiskLevel> GetKbsAreaStatus();


        /// <summary>
        /// 获取各区域的最大风险等级信息(可门管控中心【风险四色图】版本)
        /// </summary>
        /// <returns></returns>
        List<AreaRiskLevel> GetKMAreaStatus();

        int GetSafetyChangeNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 安全预警项目（省公司级），依次为存在重大隐患的电厂数，隐患整改率小于80%的电厂数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<decimal> GetWarnItems(ERCHTMS.Code.Operator user);

        /// <summary>
        /// 风险项目相关(省公司级)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<decimal> GetRiskAnalyzeItems(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取安全检查数，依次为安全检查次数，待执行的安全检查数（省公司级）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetSafetyCheckForGroup(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取安全检查数，依次为安全检查次数，待执行的安全检查数（省公司级）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetHtForGroup(ERCHTMS.Code.Operator user);

        /// <summary>
        /// 电厂隐患排名
        /// </summary>
        /// <param name="deptCode">省公司deptCode</param>
        /// <param name="mode">排名方式，0：按隐患数量排名，1：按隐患整改率排名，2：按未闭环的数量排名</param>
        /// <returns></returns>
        DataView GetRatioDataOfFactory(ERCHTMS.Code.Operator user, int mode = 0);
        /// <summary>
        /// 电厂安全检查和隐患信息统计
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        List<decimal> GetHt2CheckOfFactory(string orgId, string time, string orgCode = "");

        /// <summary>
        /// 获取当前用户所属机构的数据指标项目
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetDeptDataSet(ERCHTMS.Code.Operator user, string itemType);

        /// <summary>
        /// 获取电厂隐患整改率
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<decimal> GetHtZgl(string orgId);

        /// <summary>
        /// 获取隐患或者风险的统计项目
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetHtOrRiskItems(ERCHTMS.Code.Operator user, int mode);
        /// <summary>
        /// 获取省公司下发的安全检查任务
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<int> GetSafetyCheckTask(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 华升大屏实时工作
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<RealTimeWorkModel> GetRealTimeWork(Operator user);

        /// <summary>
        /// 华昇大屏预警中心
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<RealTimeWorkModel> GetWarningCenterWork(Operator user);
        int GetJobApprovalFormNum(Operator user);
        #endregion

        #region MyRegion
        /// <summary>
        /// 获取指标值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        decimal GetSafetyAssessedValue(SafetyAssessedArguments entity);
        #endregion

        #region MyRegion
        /// <summary>
        /// 获取指标大项
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        List<SafetyAssessedModel> GetSafetyAssessedData(SafetyAssessedArguments argument);
        #endregion

        #region 获取模块下对应的指标
        /// <summary>
        /// 获取模块下对应的指标
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        List<SafetyAssessedChildModel> GetSafetyAssessedChildData(SafetyAssessedArguments argument, List<ClassificationIndexEntity> list);
        #endregion

        
        #region 获取曝光信息
        /// <summary>
        /// 获取曝光信息
        /// </summary>
        /// <returns></returns>
        DataTable GetExposureInfo(ERCHTMS.Code.Operator user);
        #endregion

        /// <summary>
        /// 安措计划代办事项
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetSafeMeasureNum(Operator user);

        /// <summary>
        /// 国电汉川对接待办接收
        /// </summary>
        /// <param name="entity"></param>
        void GdhcDbsxSyncJS(GdhcDbsxEntity entity);
        /// <summary>
        /// 国电汉川对接待办已办
        /// </summary>
        /// <param name="entity"></param>
        void GdhcDbsxSyncYB(GdhcDbsxEntity entity);
        /// <summary>
        /// 国电汉川对接待办办结
        /// </summary>
        /// <param name="entity"></param>
        void GdhcDbsxSyncBJ(GdhcDbsxEntity entity);
    }
}
