using BSFramework.Util.WebControl;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.DangerousJob
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public interface JobApprovalFormIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<JobApprovalFormEntity> GetList(string queryJson);

        DataTable GetPageList(Pagination pagination, string queryJson);
        DataTable GetAppPageList(Pagination pagination, string queryJson);
        DataTable GetPageView(Pagination pagination, string queryJson);
        DataTable GetCardPageList(Pagination pagination, string queryJson);
        string IsLedgerSetting(string keyvalue);
        
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        JobApprovalFormEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable ConfigurationByWorkList(string keyValue, string moduleno);
        /// <summary>
        /// 获取作业安全证申请列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IList<JobApprovalFormEntity> GetJobSafetyCardApplyPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取作业安全证申请列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetJobSafetyCardApplyList(Pagination pagination, string queryJson);
        DataTable GetCheckInfo(string keyValue);
        /// <summary>
        /// 获取安全证
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        DataTable GetSafetyCardTable(string[] userids);

        List<CheckFlowData> GetAppFlowList(string keyValue);
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
        void SaveForm(string keyValue, JobApprovalFormEntity entity);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, JobApprovalFormEntity entity, List<ManyPowerCheckEntity> data, string arr);
        /// <summary>
        /// 修改sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int UpdateData(string sql);
        #endregion
        Flow GetFlow(string KeyValue);

        /// <summary>
        /// 变更操作人
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="TransferUserName"></param>
        /// <param name="TransferUserAccount"></param>
        /// <param name="TransferUserId"></param>
        void ExchangeForm(string keyValue, string TransferUserName, string TransferUserAccount, string TransferUserId);


        string DangerousJobLevelCount(string starttime, string endtime, string deptid, string deptcode);

        string DangerousJobLevelList(string starttime, string endtime, string deptid, string deptcode);
    }
}
