using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.IService.AccidentEvent;
using ERCHTMS.Service.AccidentEvent;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.AccidentEvent
{
    /// <summary>
    /// 描 述：WSSJBG
    /// </summary>
    public class WSSJBGBLL
    {
        private IWSSJBGService service = new WSSJBGService();

        #region 获取数据

        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<WSSJBGEntity> GetListForCon(Expression<Func<WSSJBGEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WSSJBGEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WSSJBGEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WSSJBGEntity entity)
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
