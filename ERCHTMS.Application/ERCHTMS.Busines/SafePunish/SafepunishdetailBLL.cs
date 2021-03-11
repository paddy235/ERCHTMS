using ERCHTMS.Entity.SafePunish;
using ERCHTMS.IService.SafePunish;
using ERCHTMS.Service.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.SafePunish
{
    /// <summary>
    /// 描 述：安全考核详细
    /// </summary>
    public class SafepunishdetailBLL
    {
        private SafepunishdetailIService service = new SafepunishdetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafepunishdetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafepunishdetailEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafepunishdetailEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取奖励详细列表
        /// </summary>
        /// <param name="punishId">安全考核id</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public IEnumerable<SafepunishdetailEntity> GetListByPunishId(string punishId, string type)
        {
            return service.GetListByPunishId(punishId, type);
        }

        /// <summary>
        /// 根据安全考核ID删除数据
        /// </summary>
        /// <param name="punishId">安全考核ID</param>
        /// <param name="type">类型</param>
        public int Remove(string punishId, string type)
        {
            try
            {
                service.Remove(punishId, type);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion
    }
}
