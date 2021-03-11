using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Cache.Factory;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：消息设置
    /// </summary>
    public class MessageSetBLL
    {
        private IMessageSetService service = new MessageSetService();

        #region 获取数据
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MessageSetEntity> GetList(string deptCode)
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
         /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MessageSetEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据编号获取消息对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MessageSetEntity GetEntityByCode(string code)
        {
            return service.GetEntityByCode(code);
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
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, MessageSetEntity ds)
        {
            try
            {
                service.SaveForm(keyValue, ds);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
