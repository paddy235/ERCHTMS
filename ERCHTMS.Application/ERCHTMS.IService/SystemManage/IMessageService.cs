using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.IService.SystemManage
{
    /// <summary>
    /// 描 述：数据设置
    /// </summary>
    public interface IMessageService
    {
        #region 获取数据
        DataTable FindTable(string sql);
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<MessageEntity> GetList(string deptCode);
         /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
         /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        MessageEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据用户账号获取未读消息数量
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        int GetMessCountByUserId(ERCHTMS.Code.Operator currUser);
        #endregion

        #region 验证数据
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        bool SaveForm(string keyValue, MessageEntity ds);

        void FlagReadMessage(string userAccount);
        #endregion
    }
}
