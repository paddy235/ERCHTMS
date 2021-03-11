using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配班组
    /// </summary>
    public class TeamsInfoBLL
    {
        private TeamsInfoIService service = new TeamsInfoService();

        #region 获取数据
        /// <summary>
        /// 根据条件获取班组任务分配信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TeamsInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TeamsInfoEntity> GetAllList(string queryJson)
        {
            return service.GetAllList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TeamsInfoEntity GetEntity(string keyValue)
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
        public string SaveForm(string keyValue, TeamsInfoEntity entity)
        {
            try
            {
                keyValue = service.SaveForm(keyValue, entity);
                return keyValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
