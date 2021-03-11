using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：起吊证
    /// </summary>
    public interface LifthoistcertIService
    {
        #region 获取数据
         /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(Pagination page, LifthoistSearchModel search);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LifthoistcertEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, LifthoistcertEntity entity);

        /// <summary>
        /// 审核更新
        /// </summary>
        /// <param name="jobEntity">凭吊证实体</param>
        /// <param name="auditEntity">审核实体</param>
        void ApplyCheck(LifthoistcertEntity certEntity, LifthoistauditrecordEntity auditEntity);
        #endregion
    }
}
