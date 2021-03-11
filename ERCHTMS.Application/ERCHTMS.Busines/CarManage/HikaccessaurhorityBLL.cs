using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：门禁点权限表
    /// </summary>
    public class HikaccessaurhorityBLL
    {
        private HikaccessaurhorityIService service = new HikaccessaurhorityService();

        #region 获取数据

        /// <summary>
        /// 权限列表
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
        public IEnumerable<HikaccessaurhorityEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HikaccessaurhorityEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue, string pitem, string url)
        {
            try
            {
                service.RemoveForm(keyValue, pitem, url);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 通过用户删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveUserForm(string keyValue, string pitem, string url)
        {
            try
            {
                service.RemoveUserForm(keyValue, pitem, url);
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
        public void SaveForm(string StartTime, string EndTime, List<Access> DeptList, List<Access> AccessList, int Type, string pitem, string url)
        {
            try
            {
                service.SaveForm(StartTime, EndTime, DeptList, AccessList, Type, pitem, url);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
