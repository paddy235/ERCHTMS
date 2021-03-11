using ERCHTMS.Entity.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.Observerecord
{
    /// <summary>
    /// 描 述：观察记录表
    /// </summary>
    public interface ObserverecordIService
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
        IEnumerable<ObserverecordEntity> GetList();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ObserverecordEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据观察记录Id获取观察类别
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetObsTypeData(string keyValue);
        /// <summary>
        /// 获取安全行为与不安全行为占比统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetSafetyStat(string deptCode, string year = "", string quarter = "",string month="");
        /// <summary>
        /// 获取不安全比例趋势图
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetQsStat(string deptCode, string year = "");
        /// <summary>
        /// 获取观察分析对比图
        /// </summary>
        /// <param name="deptCode">单位Code</param>
        /// <param name="year">年</param>
        /// <param name="quarter">季度</param>
        /// <param name="month">月度</param>
        /// <param name="issafety">issafety 0 不安全行为 1 安全行为</param>
        /// <returns></returns>
        string GetUntiDbStat(string deptCode, string issafety, string year = "", string quarter = "", string month = "");
        DataTable GetTable(string sql);
        /// <summary>
        /// 根据观察计划Id与任务分解Id查询是否进行了观察记录
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="planfjid"></param>
        /// <returns></returns>
        bool GetObsRecordByPlanIdAndFjId(string planid, string planfjid);
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
        void SaveForm(string keyValue, ObserverecordEntity entity, List<ObservecategoryEntity> listCategory, List<ObservesafetyEntity> listSafety);
        #endregion
    }
}
