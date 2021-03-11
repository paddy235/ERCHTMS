using ERCHTMS.Entity.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.LllegalManage
{
    /// <summary>
    /// 描 述：反违章奖励表
    /// </summary>
    public interface LllegalAwardDetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LllegalAwardDetailEntity> GetList(string queryJson);

        /// <summary>
                /// 获取实体
                /// </summary>
                /// <param name="keyValue">主键值</param>
                /// <returns></returns>
        LllegalAwardDetailEntity GetEntity(string keyValue);
        #endregion

        #region 获取违章奖励信息

        /// <summary>
        /// 获取违章奖励信息
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        List<LllegalAwardDetailEntity> GetListByLllegalId(string LllegalId);
        #endregion

        /// <summary>
        /// 删除违章奖励
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        int DeleteLllegalAwardList(string LllegalId);

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
        void SaveForm(string keyValue, LllegalAwardDetailEntity entity);
        #endregion
    }
}