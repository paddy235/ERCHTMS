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
    /// 描 述：外包流程配置表
    /// </summary>
    public class OutprocessconfigBLL
    {
        private OutprocessconfigIService service = new OutprocessconfigService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutprocessconfigEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutprocessconfigEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson) {
            return service.GetPageListJson(pagination, queryJson);
        }
        /// <summary>
        /// 判断该电厂是否存在该模块的配置
        /// </summary>
        /// <param name="deptid">电厂ID</param>
        /// <param name="moduleCode">模块Code</param>
        /// <returns>0:不存在 >0 存在</returns>
        public int IsExistByModuleCode(string deptid, string moduleCode)
        {
            return service.IsExistByModuleCode(deptid, moduleCode);
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
        public void SaveForm(string keyValue, OutprocessconfigEntity entity)
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

        public void DeleteLinkData(string recid)
        {
            try
            {
                service.DeleteLinkData(recid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
