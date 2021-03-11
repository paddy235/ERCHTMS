using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using ERCHTMS.Service.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.StandardSystem
{
    /// <summary>
    /// 描 述：标准修编申请
    /// </summary>
    public class StandardApplyBLL
    {
        private StandardApplyIService service = new StandardApplyService();

        #region 获取数据
        /// <summary>
        /// 统计首页待办事项数量
        /// </summary>
        /// <param name="indextype">1：重新申请，2：待审核（批），3：待分配</param>
        /// <returns></returns>
        public int CountIndex(string indextype)
        {
            return service.CountIndex(indextype);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StandardApplyEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取工作流节点
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DataTable GetWorkDetailList(string objectId)
        {
            return service.GetWorkDetailList(objectId);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StandardApplyEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, StandardApplyEntity entity)
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
