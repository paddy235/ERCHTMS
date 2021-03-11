using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：开收工会
    /// </summary>
    public interface WorkMeetingIService
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
        IEnumerable<WorkMeetingEntity> GetList(string queryJson);
        DataTable GetTable(string sql);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        WorkMeetingEntity GetEntity(string keyValue);
         /// <summary>
        /// 根据当前登录人获取未提交的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetNotCommitData(string userid);
        /// <summary>
        /// 获取今日开工会的临时工程数
        /// </summary>
        /// <returns></returns>
        int GetTodayTempProject(Operator curUser);
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
        void SaveForm(string keyValue, WorkMeetingEntity entity);

        void SaveWorkMeetingForm(string keyValue, WorkMeetingEntity entity, List<WorkmeetingmeasuresEntity> list,string ids);
        #endregion
    }
}
