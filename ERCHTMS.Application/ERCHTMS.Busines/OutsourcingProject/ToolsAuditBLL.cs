using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BSFramework.Util.WebControl;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：工器具审核表
    /// </summary>
    public class ToolsAuditBLL
    {
        private ToolsAuditIService service = new ToolsAuditService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ToolsAuditEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ToolsAuditEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public ToolsAuditEntity GetAuditEntity(string TOOLSID)
        {
            return service.GetAuditEntity(TOOLSID);
        }

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

         /// <summary>
        /// 通过工器具信息ID获取审核信息
        /// </summary>
        /// <param name="toolid"></param>
        /// <returns></returns>
        public List<ToolsAuditEntity> GetAuditList(string toolid)
        {
            return service.GetAuditList(toolid);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="moduleName">模块名称（值：设备工器具、特种设备工器具）</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ToolsAuditEntity entity,string moduleName)
        {
            try
            {
                service.SaveForm(keyValue, entity,moduleName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveFormToolAudit(string keyValue, ToolsAuditEntity entity)
        {
            try
            {
                service.SaveFormToolAudit(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
