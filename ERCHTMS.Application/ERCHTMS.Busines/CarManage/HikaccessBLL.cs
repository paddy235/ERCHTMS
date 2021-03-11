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
    /// 描 述：海康门禁间设备管理
    /// </summary>
    public class HikaccessBLL
    {
        private HikaccessIService service = new HikaccessService();

        #region 获取数据

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
        public IEnumerable<HikaccessEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HikaccessEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据hikID获取设备信息
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikaccessEntity HikGetEntity(string HikId)
        {
            return service.HikGetEntity(HikId);
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 门禁状态反控
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="type"></param>
        /// <param name="pitem"></param>
        /// <param name="url"></param>
        public void ChangeControl(string keyValue, int type, string pitem, string url)
        {
            try
            {
                service.ChangeControl(keyValue, type, pitem, url);
            }
            catch (Exception e)
            {
                throw;
            }
        }

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
        public void SaveForm(string keyValue, HikaccessEntity entity)
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
