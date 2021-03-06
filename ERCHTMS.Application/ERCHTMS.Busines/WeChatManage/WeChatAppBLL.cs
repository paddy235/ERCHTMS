using ERCHTMS.Entity.WeChatManage;
using ERCHTMS.IService.WeChatManage;
using ERCHTMS.Service.WeChatManage;
using System;
using System.Collections.Generic;

namespace ERCHTMS.Busines.WeChatManage
{
    /// <summary>
    /// 描 述：企业号应用
    /// </summary>
    public class WeChatAppBLL
    {
        private IWeChatAppService service = new WeChatAppService();

        #region 获取数据
        /// <summary>
        /// 应用列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WeChatAppEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 应用实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WeChatAppEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除应用
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
        /// 应用（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="weChatAppEntity">应用实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, WeChatAppEntity weChatAppEntity)
        {
            try
            {
                service.SaveForm(keyValue, weChatAppEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
