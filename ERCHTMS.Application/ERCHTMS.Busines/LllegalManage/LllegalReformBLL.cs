using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：违章整改信息
    /// </summary>
    public class LllegalReformBLL
    {
        private LllegalReformIService service = new LllegalReformService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalReformEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalReformEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        #region 获取最近一条整改实体对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public LllegalReformEntity GetEntityByBid(string LllegalId) 
        {
            return service.GetEntityByBid(LllegalId);
        }
        #endregion

        #region 获取历史的所有整改信息
        /// <summary>
        /// 获取历史的所有核准信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LllegalReformEntity> GetHistoryList(string LllegalId) 
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
        public void SaveForm(string keyValue, LllegalReformEntity entity)
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