using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资
    /// </summary>
    public interface ISuppliesService
    {
        #region 获取数据

        /// <summary>
        /// 获取最大的SuppliesCode
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetMaxCode();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SuppliesEntity> GetListForCon(Expression<Func<SuppliesEntity, bool>> condition);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SuppliesEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SuppliesEntity GetEntity(string keyValue);

        IEnumerable<SuppliesEntity> GetMutipleDataJson(string Ids);

        /// <summary>
        /// 根据责任人获取负责的物资
        /// </summary>
        /// <param name="DutyPerson"></param>
        /// <returns></returns>
        IEnumerable<SuppliesEntity> GetDutySuppliesDataJson(string DutyPerson);

        DataTable CheckRemove(string keyvalue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, SuppliesEntity entity);
        void SaveForm(List<SuppliesEntity> slist);
        #endregion
    }
}
