using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public interface RiskHistotyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="workId">作业步骤ID</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        ///  <param name="accType">事故类型</param>
        ///  <param name="deptCode">部门编码</param>
        ///  <param name="keyWord">查询关键字</param>
        /// <returns>返回列表</returns>
        IEnumerable<RiskHistoryEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord);
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RiskHistoryEntity GetEntity(string keyValue);

        #endregion

        #region 提交数据
       /// <summary>
        /// 根据计划Id删除风险历史记录
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <returns></returns>
        int Remove(string planId);
        #endregion
    }
}