using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using ERCHTMS.Service.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.Observerecord
{
    /// <summary>
    /// 描 述：观察计划任务分解
    /// </summary>
    public class ObsplanworkBLL
    {
        private ObsplanworkIService service = new ObsplanworkService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination,queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObsplanworkEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public IEnumerable<ObsplanworkEntity> GetList()
        {
            return service.GetList();
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
        public void SaveForm(string keyValue, ObsplanworkEntity entity)
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