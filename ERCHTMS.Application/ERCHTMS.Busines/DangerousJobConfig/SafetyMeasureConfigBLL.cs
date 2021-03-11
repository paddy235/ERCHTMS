using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.IService.DangerousJobConfig;
using ERCHTMS.Service.DangerousJobConfig;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.DangerousJobConfig
{
    /// <summary>
    /// 描 述：危险作业安全措施配置
    /// </summary>
    public class SafetyMeasureConfigBLL
    {
        private SafetyMeasureConfigIService service = new SafetyMeasureConfigService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyMeasureConfigEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyMeasureConfigEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafetyMeasureConfigEntity entity,List<SafetyMeasureDetailEntity> list)
        {
            try
            {
                service.SaveForm(keyValue, entity, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
