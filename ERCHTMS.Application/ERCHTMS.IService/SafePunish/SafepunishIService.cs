using ERCHTMS.Entity.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.IService.SafePunish
{
    /// <summary>
    /// 描 述：安全惩罚
    /// </summary>
    public interface SafepunishIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafepunishEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafepunishEntity GetEntity(string keyValue);

        DataTable GetPageList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, SafepunishEntity entity, SafekpidataEntity kpiEntity);
        #endregion

        void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity);

        string GetPunishStatisticsCount(string year, string statMode);

        string GetPunishStatisticsList(string year, string statMode);
        Flow GetFlow(string keyValue);
        string GetPunishCode();

        string GetPunishNum();

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetAptitudeInfo(string keyValue);
    }
}
