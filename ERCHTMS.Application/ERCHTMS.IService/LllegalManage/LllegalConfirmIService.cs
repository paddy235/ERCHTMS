using ERCHTMS.Entity.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.LllegalManage
{
    /// <summary>
    /// 描 述：违章验收确认信息
    /// </summary>
    public interface LllegalConfirmIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LllegalConfirmEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LllegalConfirmEntity GetEntity(string keyValue);

        #region 获取最近一条验收实体对象
        /// <summary>
        /// 获取最近一条验收实体对象
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        LllegalConfirmEntity GetEntityByBid(string LllegalId);
        #endregion

        #region 获取历史的所有验收信息
        /// <summary>
        /// 获取历史的所有验收信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<LllegalConfirmEntity> GetHistoryList(string LllegalId);
        #endregion


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
        void SaveForm(string keyValue, LllegalConfirmEntity entity);
        #endregion
    }
}