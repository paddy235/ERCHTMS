using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.IService.WorkPlan;
using ERCHTMS.Service.WorkPlan;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.WorkPlan
{
    /// <summary>
    /// 描 述：工作计划详情
    /// </summary>
    public class PlanDetailsBLL
    {
        private PlanDetailsIService service = new PlanDetailsService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PlanDetailsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PlanDetailsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 更新变更状态
        /// </summary>
        /// <param name="applyId"></param>
        public void UpdateChangedData(string applyId)
        {
            service.UpdateChangedData(applyId);
        }
        #endregion

        #region 统计
        /// <summary>
        /// 统计工作计划
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable Statistics(string deptId, string starttime, string endtime, string applytype = "")
        {
            return service.Statistics(deptId, starttime, endtime,applytype);
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
        public void RemoveFormByApplyId(string keyValue)
        {
            try
            {
                service.RemoveFormByApplyId(keyValue);
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
        public void SaveForm(string keyValue, PlanDetailsEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 按月保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveMonth(string keyValue, PlanDetailsEntity entity)
        {
            try
            {
                for (int i = 0; i < 12; i++)
                {
                    var pdt = DateTime.Parse(string.Format("{0}-{1}-01", entity.PlanFinDate.Value.Year, (i + 1))).AddMonths(1).AddSeconds(-1);
                    entity.ID = Guid.NewGuid().ToString();
                    entity.PlanFinDate = pdt;
                    service.SaveForm("", entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
