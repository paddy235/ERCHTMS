using ERCHTMS.Entity.RiskDataBaseConfig;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：工作任务清单说明表
    /// </summary>
    public interface WorkfileIService
    {
        #region 获取数据
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<WorkfileEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        WorkfileEntity GetEntity(string keyValue);
          /// <summary>
        /// 根据机构Code查询本机构是否已经添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool GetIsExist(string orgCode);
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
        void SaveForm(string keyValue, WorkfileEntity entity);
        #endregion
    }
}
