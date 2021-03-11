using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签上下线日志
    /// </summary>
    public class LableonlinelogBLL
    {
        private LableonlinelogIService service = new LableonlinelogService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LableonlinelogEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LableonlinelogEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据标签ID获取在线标签
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public LableonlinelogEntity GetOnlineEntity(string LableId)
        {
            return service.GetOnlineEntity(LableId);
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
        public void SaveForm(string keyValue, LableonlinelogEntity entity)
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
        /// 储存上下线数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveStatus(LableonlinelogEntity entity)
        {
           return service.SaveStatus(entity);
        }

        #endregion
    }
}
