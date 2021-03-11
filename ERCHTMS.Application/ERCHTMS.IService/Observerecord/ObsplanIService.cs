using ERCHTMS.Entity.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.Observerecord
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    public interface ObsplanIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ObsplanEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ObsplanEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据观察计划Id与任务分解Id获取相应信息--此处查询的是发布后的数据--BIS_OBSPLAN_FB
        /// </summary>
        /// <param name="PlanId">计划id </param>
        /// <param name="PlanFjId">任务分解Id</param>
        /// <returns></returns>
        DataTable GetPlanById(string PlanId, string PlanFjId,string month);
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
        void SaveForm(string keyValue, ObsplanEntity entity);
        bool CommitEhsData(Operator currUser);
        /// <summary>
        /// 查询本年度该观察计划那些月份进行了观察记录
        /// </summary>
        /// <param name="planId">计划Id</param>
        /// <param name="planfjid">计划任务分解Id</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        DataTable GetObsRecordIsExist(string planId, string planfjid, string year);
        /// <summary>
        /// 导入模板数据
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        void InsertImportData(List<ObsplanEntity> obsplan, List<ObsplanworkEntity> obsplanwork);
        void InsertImportData(List<ObsplanEHSEntity> obsplan, List<ObsplanworkEntity> obsplanwork);
        /// <summary>
        /// 复制年度计划
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="oldYear">复制的年度</param>
        /// <param name="newYear">复制到的年度</param>
        /// <returns></returns>
        bool CopyHistoryData(Operator currUser, string oldYear, string newYear);

            /// <summary>
        /// 只修改计划月份直接同步到EHS与发布的数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SynchData(string planid, string planfjid);
        #endregion
    }
}
