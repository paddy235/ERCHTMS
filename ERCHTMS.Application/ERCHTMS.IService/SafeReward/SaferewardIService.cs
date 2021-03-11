using ERCHTMS.Entity.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.IService.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励
    /// </summary>
    public interface SaferewardIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SaferewardEntity> GetList(string queryJson);


        object GetStandardJson();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SaferewardEntity GetEntity(string keyValue);

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
        void SaveForm(string keyValue, SaferewardEntity entity);
        #endregion

        void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity, string leaderShipId);


        DataTable GetRewardStatisticsList(string year);

        DataTable GetRewardStatisticsTimeList(string year);

        string GetRewardStatisticsCount(string year);

        string GetRewardStatisticsTime(string year);

        Flow GetFlow(string keyValue);
        List<object> GetLeaderList();

        string GetRewardCode();

        string GetRewardNum();

        string GetDeptPId(string deptId);

        List<object> GetSpecialtyPrincipal(string applyDeptId);

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetAptitudeInfo(string keyValue);

        string GetRewardStatisticsExcel(string year = "");

        string GetRewardStatisticsTimeExcel(string year = "");
    }
}
