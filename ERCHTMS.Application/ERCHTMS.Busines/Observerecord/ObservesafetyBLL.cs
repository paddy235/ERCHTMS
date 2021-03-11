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
    /// 描 述：观察记录安全行为
    /// </summary>
    public class ObservesafetyBLL
    {
        private ObservesafetyIService service = new ObservesafetyService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ObservesafetyEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        public DataTable GetSafetyPageList(Pagination pagination, string queryJson)
        {
            return service.GetSafetyPageList(pagination, queryJson);
        }

        /// <summary>
        /// 根据类型和行为获取列表
        /// </summary>
        /// <param name="issafety">是否安全0-安全行为 1 -不安全行为</param>
        /// <param name="type">观察类别-7大类</param>
        /// <param name="recordid">观察记录Id</param>
        /// <returns></returns>
        public DataTable GetDataByType(string issafety, string type, string recordid)
        {
            return service.GetDataByType(issafety, type, recordid);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObservesafetyEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ObservesafetyEntity entity)
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

        public void SaveForm(string recordId, List<ObservesafetyEntity> entity)
        {
            try
            {
                service.SaveForm(recordId, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}