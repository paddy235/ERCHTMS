using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患评估信息表
    /// </summary>
    public interface HTApprovalIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HTApprovalEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HTApprovalEntity GetEntity(string keyValue);

        /// <summary>
        /// 通过隐患编码获取
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        HTApprovalEntity GetEntityByHidCode(string hidCode);
        /// <summary>
        /// 根据隐患编码获取Table
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        DataTable GetDataTableByHidCode(string hidCode);

        IEnumerable<HTApprovalEntity> GetHistoryList(string hidCode);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);

        void RemoveFormByCode(string hidcode);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, HTApprovalEntity entity);
        #endregion
    }
}
