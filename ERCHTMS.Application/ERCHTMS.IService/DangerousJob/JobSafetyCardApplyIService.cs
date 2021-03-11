using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.IService.DangerousJob
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public interface JobSafetyCardApplyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<JobSafetyCardApplyEntity> GetList(string queryJson);

        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取今日高危作业列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTodayWorkList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        JobSafetyCardApplyEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable ConfigurationByWorkList(string keyValue, string moduleno);


        DataTable GetPageView(Pagination pagination, string queryJson);

        DataTable FindTable(string sql);
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
        void SaveForm(string keyValue, JobSafetyCardApplyEntity entity);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, JobSafetyCardApplyEntity entity, List<ManyPowerCheckEntity> data, string arr, string arrData);

        /// <summary>
        /// 变更操作人
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="TransferUserName"></param>
        /// <param name="TransferUserAccount"></param>
        /// <param name="TransferUserId"></param>
        void ExchangeForm(string keyValue, string TransferUserName, string TransferUserAccount, string TransferUserId);

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        Flow GetFlow(string KeyValue);

        List<CheckFlowData> GetAppFlowList(string keyValue);
        #endregion

        string GetDangerousJobCount(string starttime, string endtime, string deptid, string deptcode);

        string GetDangerousJobList(string starttime, string endtime, string deptid, string deptcode);

    }
}
