using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业
    /// </summary>
    public interface LifthoistjobIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(Pagination page, LifthoistSearchModel search);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable getTempEquipentList(Pagination page, LifthoistSearchModel search);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LifthoistjobEntity GetEntity(string keyValue);

        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);

        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);
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
        void SaveForm(string keyValue, LifthoistjobEntity entity);

        /// <summary>
        /// 审核更新
        /// </summary>
        /// <param name="jobEntity">起重吊装作业实体</param>
        /// <param name="auditEntity">审核实体</param>
        void ApplyCheck(LifthoistjobEntity jobEntity, LifthoistauditrecordEntity auditEntity);
        #endregion
    }
}
