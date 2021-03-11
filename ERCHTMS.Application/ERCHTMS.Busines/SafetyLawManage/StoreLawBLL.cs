using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.IService.SafetyLawManage;
using ERCHTMS.Service.SafetyLawManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.SafetyLawManage
{
    /// <summary>
    /// 描 述：收藏法律法规
    /// </summary>
    public class StoreLawBLL
    {
        private StoreLawIService service = new StoreLawService();

        #region 获取数据
        /// <summary>
        /// 获取安全生产法律法规我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            return service.GetPageDataTable(pagination, queryJson);
        }

        /// <summary>
        /// 获取安全管理制度我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageJsonInstitution(Pagination pagination, string queryJson)
        {
            return service.GetPageJsonInstitution(pagination, queryJson);
        }

        /// <summary>
        /// 获取安全操作规程我的收藏列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageJsonStandards(Pagination pagination, string queryJson)
        {
            return service.GetPageJsonStandards(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StoreLawEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StoreLawEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// 根据法律id确定是否已收藏
        /// </summary>
        /// <returns></returns>
        public int GetStoreBylawId(string lawid)
        {
            return service.GetStoreBylawId(lawid);
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
        public void SaveForm(string keyValue, StoreLawEntity entity)
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
