using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：合同
    /// </summary>
    public class CompactBLL
    {
        private CompactIService service = new CompactService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public object GetCompactProtocol(string keyValue)
        {
            return service.GetCompactProtocol(keyValue);
        }

        public object GetLastCompactProtocol(string keyValue)
        {
            return service.GetLastCompactProtocol(keyValue);
        }

        #region 获取工程下的合同信息
        /// <summary>
        /// 获取工程下的合同信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<CompactEntity> GetListByProjectId(string projectId)
        {
            return service.GetListByProjectId(projectId);
        }
        /// <summary>
        /// 根基工程Id获取合同期限
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetComoactTimeByProjectId(string projectId) {
            return service.GetComoactTimeByProjectId(projectId);
        }
        #endregion
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
        /// <returns></returns>
        public void SaveForm(string keyValue, CompactEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
