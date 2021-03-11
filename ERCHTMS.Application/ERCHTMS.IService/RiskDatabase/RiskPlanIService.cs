using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：辨识评估计划表
    /// </summary>
    public interface RiskPlanIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<RiskPlanEntity> GetList(string queryJson);

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
        RiskPlanEntity GetEntity(string keyValue);

         /// <summary>
        ///根据辨识计划ID获取相关风险点的各状态数量信息
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="startdate">计划开始时间</param>
        /// <param name="enddate">计划结束时间</param>
        /// <returns></returns>
        List<int> GetNumbers(string planId, string startDate, string endDate,int status,string areaId);
         /// <summary>
        ///根据辨识计划ID获取辨识和评估人员（多个人员账号用逗号分隔）
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="dataType">数据类型，0：获取辨识人员，1：获取评估人员</param>
        /// <returns>人员账号，多个用英文逗号分隔</returns>
        string GetUsers(string planId, int dataType=-1);
         /// <summary>
        /// 根据用户辨识评估的区域ID（多个用逗号分割）
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="userAccount">账号</param>
        /// <returns></returns>
        string GetCurrUserAreaId(string planId, string userAccount);
            /// <summary>
        /// 获取所有未完成计划相关联的区域ID（多个用逗号分割）
        /// </summary>
        /// <param name="status">状态，0：未完成，1：已完成</param>
        /// <returns></returns>
        string GetPlanAreaIds(int status = 0, string planId = "");

        /// <summary>
        /// 首页待办事项获取辨识评估计划
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        int GetPlanCount(ERCHTMS.Code.Operator user, int mode);
          /// <summary>
        /// 获取计划原有风险数量
        /// </summary>
        /// <param name="areaIds">计划关联区域</param>
        /// <param name="startDate">计划开始时间</param>
        /// <returns></returns>
        int GetRiskNumbers( string areaIds, string startDate, string planId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        int RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        int SaveForm(string keyValue, RiskPlanEntity entity);
          /// <summary>
        /// 设置计划完成状态并同步相关联风险记录到历史记录表
        /// </summary>
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="areaIds">区域ID(多个用逗号分隔)</param>
        /// <returns></returns>
        bool SetComplate(string planId, string areaIds);
        #endregion
    }
}