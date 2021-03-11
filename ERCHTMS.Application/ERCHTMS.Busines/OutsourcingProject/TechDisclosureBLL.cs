using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全技术交底
    /// </summary>
    public class TechDisclosureBLL
    {
        private TechDisclosureIService service = new TechDisclosureService();

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
        public IEnumerable<TechDisclosureEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TechDisclosureEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public DataTable GetNameByPorjectId(string projectId, string type) {
            return service.GetNameByPorjectId(projectId, type);
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
        /// <returns></returns>
        public void SaveForm(string keyValue, TechDisclosureEntity entity)
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
        /// <summary>
        /// 审核表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        public void ApporveForm(string keyValue, TechDisclosureEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            try
            {
                service.ApporveForm(keyValue, entity, aentity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion
    }
}
