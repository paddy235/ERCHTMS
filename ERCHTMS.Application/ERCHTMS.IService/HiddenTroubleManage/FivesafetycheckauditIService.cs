using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：五定安全检查 整改验收表
    /// </summary>
    public interface FivesafetycheckauditIService
    {
        #region 获取数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GeDataTableByIds(string ids);
        DataTable GetDataTable(string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<FivesafetycheckauditEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        FivesafetycheckauditEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, FivesafetycheckauditEntity entity);
        #endregion
    }
}
