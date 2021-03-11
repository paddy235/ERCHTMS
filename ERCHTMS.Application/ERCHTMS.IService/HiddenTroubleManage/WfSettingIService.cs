using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程转向配置实例表
    /// </summary>
    public interface WfSettingIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<WfSettingEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        WfSettingEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取通用的查询内容
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetGeneralQuery(string sql, DbParameter[] param);
        #endregion

        #region  流程配置实例信息
        /// <summary>
        /// 流程配置实例信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetWfSettingInfoPageList(Pagination pagination, string queryJson);
        #endregion

        #region 根据对象获取所有相关的适配条件内容
        /// <summary>
        /// 根据对象获取所有相关的适配条件内容
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        DataTable GetWfSettingForInstance(WfControlObj entity,  string settingtype, string setting);
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
        void SaveForm(string keyValue, WfSettingEntity entity);
        #endregion
    }
}