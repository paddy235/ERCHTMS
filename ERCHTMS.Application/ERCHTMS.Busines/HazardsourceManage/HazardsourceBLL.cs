using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.IService.HazardsourceManage;
using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Service.HazardsourceManage;

namespace ERCHTMS.Busines.HazardsourceManage
{
    /// <summary>
    /// 描 述：危险源辨识评估
    /// </summary>
    public class HazardsourceBLL
    {
        private IHazardsourceService service = new HazardsourceService();

        #region 获取数据
        public DataTable FindTableBySql(string sql)
        {
            return service.FindTableBySql(sql);

        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteBySql(string sql)
        {
            return service.ExecuteBySql(sql);

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
        public IEnumerable<HazardsourceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazardsourceEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, HazardsourceEntity entity)
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
