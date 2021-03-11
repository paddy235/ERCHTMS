using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资
    /// </summary>
    public class SuppliesBLL
    {
        private ISuppliesService service = new SuppliesService();

        #region 获取数据

        /// <summary>
        public string GetMaxCode()
        {
            return service.GetMaxCode();
        }

        public IEnumerable<SuppliesEntity> GetListForCon(Expression<Func<SuppliesEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SuppliesEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuppliesEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public IEnumerable<SuppliesEntity> GetMutipleDataJson(string Ids)
        {
            return service.GetMutipleDataJson(Ids);
        }

        /// <summary>
        /// 根据责任人获取负责的物资
        /// </summary>
        /// <param name="DutyPerson"></param>
        /// <returns></returns>
        public IEnumerable<SuppliesEntity> GetDutySuppliesDataJson(string DutyPerson)
        {
            return service.GetDutySuppliesDataJson(DutyPerson);
        }

        public DataTable CheckRemove(string keyvalue)
        {
            return service.CheckRemove(keyvalue);
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
        public void SaveForm(string keyValue, SuppliesEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveForm(List<SuppliesEntity> slist)
        {
            try
            {
                service.SaveForm(slist);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
