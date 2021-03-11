using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险作业许可申请
    /// </summary>
    public class HighRiskApplyBLL
    {
        private HighRiskApplyIService service = new HighRiskApplyService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<HighRiskApplyEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HighRiskApplyEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HighRiskApplyEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            return service.GetPageDataTable(pagination, queryJson);
        }

        /// <summary>
        /// 获取审批完成作业列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetSelectDataTable(Pagination pagination, string queryJson)
        {
            return service.GetSelectDataTable(pagination, queryJson);
        }
        /// <summary>
        /// 获取审核(批)列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetVerifyPageTableJson(Pagination pagination, string queryJson)
        {
            return service.GetVerifyPageTableJson(pagination, queryJson);
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
        public int SaveForm(string keyValue, HighRiskApplyEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region 统计

        /// <summary>
        /// 获取审批完成作业列表(统计跳转)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetStatisticsWorkTable(Pagination pagination, string queryJson)
        {
            return service.GetStatisticsWorkTable(pagination, queryJson);
        }

        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkCount(starttime, endtime, deptid, deptcode);
        }


        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// 月度趋势(统计图)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearCount(year, deptid, deptcode);
        }

        /// <summary>
        /// 月度趋势(表格)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearList(year, deptid, deptcode);
        }
        #endregion

        #region 移动端
        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetWorkTypeInfo(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetWorkTypeInfo(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// 月度趋势
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetWorkYearCount(string year, string deptid, string deptcode)
        {
            return service.GetWorkYearCount(year, deptid, deptcode);
        }
        #endregion
    }
}
