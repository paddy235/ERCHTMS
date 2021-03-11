using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using ERCHTMS.Service.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.Observerecord
{
    /// <summary>
    /// 描 述：观察记录表
    /// </summary>
    public class ObserverecordBLL
    {
        private ObserverecordIService service = new ObserverecordService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        public DataTable GetTable(string sql) {
            return service.GetTable(sql);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ObserverecordEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 根据观察记录Id获取观察类别
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetObsTypeData(string keyValue) {
            return service.GetObsTypeData(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObserverecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取安全行为与不安全行为占比统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetSafetyStat(string deptCode, string year = "", string quarter = "",string month="")
        {
            return service.GetSafetyStat(deptCode, year, quarter, month);
        }
        /// <summary>
        /// 获取观察分析对比图
        /// </summary>
        /// <param name="deptCode">单位Code</param>
        /// <param name="year">年</param>
        /// <param name="quarter">季度</param>
        /// <param name="month">月度</param>
        /// <param name="issafety">issafety 0 不安全行为 1 安全行为</param>
        /// <returns></returns>
        public string GetUntiDbStat(string deptCode, string issafety, string year = "", string quarter = "", string month = "")
        {
            return service.GetUntiDbStat(deptCode,issafety, year, quarter, month);
        }
        /// <summary>
        /// 获取不安全比例趋势图
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetQsStat(string deptCode, string year = "")
        {
            return service.GetQsStat(deptCode, year);
        }
        /// <summary>
        /// 根据观察计划Id与任务分解Id查询是否进行了观察记录
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="planfjid"></param>
        /// <returns></returns>
        public bool GetObsRecordByPlanIdAndFjId(string planid, string planfjid) {
            return service.GetObsRecordByPlanIdAndFjId(planid, planfjid);
        }

        public DataTable GetMenuTable(string sql) {
            return service.GetTable(sql);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ObserverecordEntity entity, List<ObservecategoryEntity> listCategory, List<ObservesafetyEntity> safetyList)
        {
            try
            {
                service.SaveForm(keyValue, entity, listCategory, safetyList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
