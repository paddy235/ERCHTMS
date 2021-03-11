using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.IService.SaftProductTargetManage;
using ERCHTMS.Service.SaftProductTargetManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产责任书
    /// </summary>
    public class SafeProductDutyBookBLL
    {
        private SafeProductDutyBookIService service = new SafeProductDutyBookService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafeProductDutyBookEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafeProductDutyBookEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取安全目标责任书列表
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetListByProductId(string productId)
        {
            return service.GetListByProductId(productId);
        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
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
        /// 根据安全生产目标id删除数据
        /// </summary>
        /// <param name="planId">目标ID</param>
        public int Remove(string productId)
        {
            try
            {
                service.Remove(productId);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafeProductDutyBookEntity entity)
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
