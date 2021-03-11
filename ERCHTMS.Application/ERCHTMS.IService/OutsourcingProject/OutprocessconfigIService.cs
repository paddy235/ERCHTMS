using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包流程配置表
    /// </summary>
    public interface OutprocessconfigIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OutprocessconfigEntity> GetList();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OutprocessconfigEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);
        /// <summary>
        /// 判断该电厂是否存在该模块的配置
        /// </summary>
        /// <param name="deptid">电厂ID</param>
        /// <param name="moduleCode">模块Code</param>
        /// <returns>0:不存在 >0 存在</returns>
        int IsExistByModuleCode(string deptid, string moduleCode);

        OutprocessconfigEntity GetEntityByModuleCode(string deptid, string moduleCode);
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
        void SaveForm(string keyValue, OutprocessconfigEntity entity);

        /// <summary>
        /// 删除关联数据
        /// </summary>
        /// <param name="recid"></param>
        void DeleteLinkData(string recid);
        #endregion
    }
}
