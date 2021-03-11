using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.IService.RiskDataBaseConfig;
using ERCHTMS.Service.RiskDataBaseConfig;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：安全风险管控配置表
    /// </summary>
    public class RiskdatabaseconfigBLL
    {
        private RiskdatabaseconfigIService service = new RiskdatabaseconfigService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 根据SQL语句获取数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql)
        {
            return service.GetTable(sql);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskdatabaseconfigEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskdatabaseconfigEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, RiskdatabaseconfigEntity entity)
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
