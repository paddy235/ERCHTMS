using ERCHTMS.Entity.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.StandardSystem
{
    /// <summary>
    /// 描 述：标准修编申请表
    /// </summary>
    public interface StandardCheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<StandardCheckEntity> GetList(string queryJson);
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        StandardCheckEntity GetEntity(string keyValue);
        /// <summary>
        /// 会签是否完成
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        bool FinishSign(string keyValue, string checkUserId);
        /// <summary>
        /// 分委会审核是否完成
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        bool FinishCommittee(string keyValue, string checkUserId);
        /// <summary>
        /// 是否全部完成审核
        /// </summary>
        /// <returns></returns>
        bool FinishComplete(string checkUserId, string checkUserName, string checkType);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        StandardCheckEntity GetLastEntityByRecId(string keyValue,string checkType);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, StandardCheckEntity entity);
        #endregion
    }
}
