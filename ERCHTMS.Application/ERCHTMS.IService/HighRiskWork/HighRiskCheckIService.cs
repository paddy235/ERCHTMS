using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：高危险作业审核/审批表
    /// </summary>
    public interface HighRiskCheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HighRiskCheckEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HighRiskCheckEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据申请表id删除数据
        /// </summary>
        int Remove(string workid);

        /// <summary>
        /// 根据申请id获取审核信息[不包括没审的]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        IEnumerable<HighRiskCheckEntity> GetCheckListInfo(string approveid);

        /// <summary>
        /// 根据申请id获取没审核的条数
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        int GetNoCheckNum(string approveid);

          /// <summary>
        /// 根据申请id和当前登录人获取审核(批)记录
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        HighRiskCheckEntity GetNeedCheck(string approveid);

         /// <summary>
        /// 根据申请id获取审批信息[不包括没审的]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        HighRiskCheckEntity GetApproveInfo(string approveid);
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
        void SaveForm(string keyValue, HighRiskCheckEntity entity);
        #endregion
    }
}
