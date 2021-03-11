using ERCHTMS.Entity.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BSFramework.Util.WebControl;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：工器具审核表
    /// </summary>
    public interface ToolsAuditIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ToolsAuditEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ToolsAuditEntity GetEntity(string keyValue);
        ToolsAuditEntity GetAuditEntity(string TOOLSID);

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 通过工器具信息ID获取审核信息
        /// </summary>
        /// <param name="toolid"></param>
        /// <returns></returns>
        List<ToolsAuditEntity> GetAuditList(string toolid);
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
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ToolsAuditEntity entity, string moduleName);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveFormToolAudit(string keyValue, ToolsAuditEntity entity);
        #endregion
    }
}
