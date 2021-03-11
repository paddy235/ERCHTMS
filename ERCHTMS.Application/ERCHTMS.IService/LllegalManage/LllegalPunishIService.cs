using ERCHTMS.Entity.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.LllegalManage
{
    /// <summary>
    /// 描 述：违章责任人处罚信息
    /// </summary>
    public interface LllegalPunishIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LllegalPunishEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LllegalPunishEntity GetEntity(string keyValue);


        #region 获取考核记录集合对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        List<LllegalPunishEntity> GetListByLllegalId(string LllegalId, string type);
        #endregion

        #region 获取考核记录实体对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        LllegalPunishEntity GetEntityByBid(string LllegalId);
        #endregion

        #region 获取考核记录实体对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        LllegalPunishEntity GetEntityByApproveId(string approveId);
        #endregion

        #region 获取考核记录集合对象(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// <summary>
        /// 获取集合(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        int DeleteLllegalPunishList(string LllegalId, string type);
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
        void SaveForm(string keyValue, LllegalPunishEntity entity);
        #endregion
    }
}