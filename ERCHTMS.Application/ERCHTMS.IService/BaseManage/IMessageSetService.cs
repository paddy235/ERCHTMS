using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// 描 述：数据设置
    /// </summary>
    public interface IMessageSetService
    {
        #region 获取数据
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<MessageSetEntity> GetList(string deptCode);
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
        MessageSetEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据编号获取消息对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MessageSetEntity GetEntityByCode(string code);
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
        void SaveForm(string keyValue, MessageSetEntity ds);
        #endregion
    }
}
