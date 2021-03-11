using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：安全措施变动申请表
    /// </summary>
    public interface SafetychangeIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafetychangeEntity> GetList(string queryJson);
        /// <summary>
        /// 获取安全设施变动台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetLedgerList(Pagination pagination, string queryJson);

        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafetychangeEntity GetEntity(string keyValue);
        Flow GetFlow(string keyValue, List<string> modulename);

        List<CheckFlowData> GetAppFlowList(string keyValue, List<string> modulename, string flowid, bool isendflow, string workdeptid, string projectid, string specialtytype = "");
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
        void SaveForm(string keyValue, SafetychangeEntity entity);

        DataTable FindTable(string sql);
        #endregion
    }
}
