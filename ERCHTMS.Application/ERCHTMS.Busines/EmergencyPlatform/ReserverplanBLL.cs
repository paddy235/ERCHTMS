using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Text;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急预案
    /// </summary>
    public class ReserverplanBLL
    {
        private IReserverplanService service = new ReserverplanService();

        #region 获取数据
        public string GetOptionsString()
        {
            var list = GetList("");

            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", item.ID, item.NAME);
            }
            return sb.ToString();
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
        public IEnumerable<ReserverplanEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ReserverplanEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ReserverplanEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                throw;
            }
        }
        #endregion
    }
}
