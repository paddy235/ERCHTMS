using ERCHTMS.Entity.SafetyMeshManage;
using ERCHTMS.IService.SafetyMeshManage;
using ERCHTMS.Service.SafetyMeshManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.SafetyMeshManage
{
    /// <summary>
    /// 描 述：安全网络
    /// </summary>
    public class SafetyMeshBLL
    {
        private SafetyMeshIService service = new SafetyMeshService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyMeshEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyMeshEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetTableList(string queryJson)
        {
            return service.GetTableList(queryJson);
        }
        public DataTable GetTable(string queryJson)
        {
            return service.GetTable(queryJson);
        }
        public IEnumerable<SafetyMeshEntity> GetListForCon(Expression<Func<SafetyMeshEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
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
        public void SaveForm(string keyValue, SafetyMeshEntity entity)
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
