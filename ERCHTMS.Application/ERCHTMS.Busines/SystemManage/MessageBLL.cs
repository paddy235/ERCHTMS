using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Cache.Factory;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Busines.SystemManage
{
    /// <summary>
    /// 描 述：数据设置
    /// </summary>
    public class MessageBLL
    {
        private IMessageService service = new MessageService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = BSFramework.Util.Config.GetValue("SoftName") + "_BlackSetCache";

        #region 获取数据
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MessageEntity> GetList(string deptCode)
        {
            return service.GetList(deptCode);
        }
      /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable  GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public DataTable FindTable(string sql) {
            return service.FindTable(sql);
        }
         /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MessageEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据用户账号获取未读消息数量
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public int GetMessCountByUserId(ERCHTMS.Code.Operator currUser)
        {
            return service.GetMessCountByUserId(currUser);
        }

        #endregion

        #region 验证数据
        
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据用户账号获取未读消息并标记已读
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public void FlagReadMessage(string userAccount)
        {
             service.FlagReadMessage(userAccount);
        }
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, MessageEntity ds)
        {
            try
            {
                
                return service.SaveForm(keyValue, ds);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
