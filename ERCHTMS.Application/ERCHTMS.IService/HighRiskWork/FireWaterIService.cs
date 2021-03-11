using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：使用消防水
    /// </summary>
    public interface FireWaterIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(Pagination page, string queryJson, string authType, Operator user);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        FireWaterEntity GetEntity(string keyValue);
        FireWaterCondition GetConditionEntity(string fireWaterId);

        /// <summary>
        /// 获取执行情况集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        IList<FireWaterCondition> GetConditionList(string keyValue);
        #endregion

        #region 获取消防水使用台账
        /// <summary>
        /// 获取消防水使用台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetLedgerList(Pagination pagination, string queryJson, Operator user);
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
        void SaveForm(string keyValue, FireWaterModel model);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, FireWaterEntity entity);


        /// <summary>
        /// 更新业务表、审核表
        /// </summary>
        /// <param name="fireWaterEntity">业务主表实体</param>
        /// <param name="auditEntity">审核表实体</param>
        void UpdateForm(FireWaterEntity fireWaterEntity, ScaffoldauditrecordEntity auditEntity);
        /// <summary>
        /// 提交执行情况
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        void SubmitCondition(string keyValue, FireWaterCondition entity);

        /// <summary>
        /// 获取APP流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);

        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);
        #endregion
    }
}
