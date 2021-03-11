using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：违章验收确认确认信息
    /// </summary>
    public class LllegalConfirmBLL
    {
        private LllegalConfirmIService service = new LllegalConfirmService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalConfirmEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalConfirmEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        #region 获取最近一条验收确认实体对象
        /// <summary>
        /// 获取最近一条验收确认实体对象
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public LllegalConfirmEntity GetEntityByBid(string LllegalId)
        {
            return service.GetEntityByBid(LllegalId);
        }
        #endregion

        #region 获取历史的所有验收确认信息
        /// <summary>
        /// 获取历史的所有验收确认信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LllegalConfirmEntity> GetHistoryList(string LllegalId)
        {
            return service.GetHistoryList(LllegalId);
        }
        #endregion

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
        public void SaveForm(string keyValue, LllegalConfirmEntity entity)
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