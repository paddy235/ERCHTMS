using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Collections;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险通用作业申请
    /// </summary>
    public interface HighRiskCommonApplyIService
    {
        #region 获取数据
        /// <summary>
        /// 得到当前最大编号
        /// </summary>
        /// <returns></returns>
        object GetMaxCode();
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<HighRiskCommonApplyEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HighRiskCommonApplyEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HighRiskCommonApplyEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取高风险通用台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetLedgerList(Pagination pagination, string queryJson, Boolean GetOperate = true);
        /// <summary>
        /// 获取高风险通用作业申请列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageDataTable(Pagination pagination, string queryJson);

        DataTable GetTable(string sql);

        /// <summary>
        /// 获取执行部门
        /// </summary>
        /// <param name="workdepttype">作业单位类型</param>
        /// <param name="workdept">作业单位</param>
        /// <param name="projectid">外包工程ID</param>
        /// <param name="Executedept">执行部门</param>
        void GetExecutedept(string workdepttype, string workdept, string projectid, out string Executedept);

        /// <summary>
        /// 获取外包单位
        /// </summary>
        /// <param name="workdept">作业单位</param>
        /// <param name="outsouringengineerdept"></param>
        void GetOutsouringengineerDept(string workdept, out string outsouringengineerdept);


        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);

        Flow GetFlow(string keyValue, string modulename);

        /// <summary>
        /// 获取审核流程名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string GetModuleName(HighRiskCommonApplyEntity entity);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        PushMessageData SaveForm(string keyValue, string type, HighRiskCommonApplyEntity entity, List<HighRiskRecordEntity> list, List<HighRiskApplyMBXXEntity> mbList);

        void SaveApplyForm(string keyValue, HighRiskCommonApplyEntity entity);

        /// <summary>
        /// 确认，审核
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="state"></param>
        /// <param name="recordData"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        PushMessageData SubmitCheckForm(string keyValue, string state, string recordData, HighRiskCommonApplyEntity entity, ScaffoldauditrecordEntity aentity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">主键</param>
        void UpdateForm(HighRiskCommonApplyEntity entity);

        /// <summary>
        /// 修改sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int UpdateData(string sql);
        #endregion

        #region 统计
        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        /// 月度趋势(统计图)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearCount(string year, string deptid, string deptcode);

        /// <summary>
        /// 月度趋势(表格)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearList(string year, string deptid, string deptcode);


        /// <summary>
        /// 单位对比(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        string GetHighWorkDepartCount(string starttime, string endtime);

        /// <summary>
        /// 单位对比(表格)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        string GetHighWorkDepartList(string starttime, string endtime);
        #endregion


        #region 获取今日高风险作业
        /// <summary>
        /// 获取今日高风险作业(作业台账中作业中的数据)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTodayWorkList(Pagination pagination, string queryJson);
        #endregion

        #region 手机端作业统计
        DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode);
        #endregion

        #region  外包需要
        bool GetProjectNum(string outProjectId);
        DataTable GetCountByArea(List<string> areaCodes);
        #endregion
    }
}
