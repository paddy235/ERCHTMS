using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险作业许可申请
    /// </summary>
    public interface HighRiskApplyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<HighRiskApplyEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HighRiskApplyEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HighRiskApplyEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageDataTable(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取审批完成作业列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetSelectDataTable(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取审核(批)列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetVerifyPageTableJson(Pagination pagination, string queryJson);

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
        void SaveForm(string keyValue, HighRiskApplyEntity entity);
        #endregion

        #region 统计
        /// <summary>
        /// 获取审批完成作业列表(统计跳转)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetStatisticsWorkTable(Pagination pagination, string queryJson);

        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        /// 月度趋势(统计图)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearCount(string year, string deptid, string deptcode);

        /// <summary>
        /// 月度趋势(表格)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearList(string year, string deptid, string deptcode);
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
        DataTable GetWorkTypeInfo(string starttime, string endtime, string deptid, string deptcode);


        /// <summary>
        /// 月度趋势
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        DataTable GetWorkYearCount(string year, string deptid, string deptcode);
        #endregion
    }
}
