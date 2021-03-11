using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using ERCHTMS.Service.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人员表
    /// </summary>
    public class OccupatioalstaffBLL
    {
        private OccupatioalstaffIService service = new OccupatioalstaffService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OccupatioalstaffEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson,string where) 
        {
            return service.GetTable(queryJson, where);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OccupatioalstaffEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, OccupatioalstaffEntity entity)
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

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            return service.GetPageListByProc(pagination, queryJson);
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="IsNew">是否新增</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(bool IsNew, OccupatioalstaffEntity entity)
        {
            try
            {
                service.SaveForm(IsNew, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
