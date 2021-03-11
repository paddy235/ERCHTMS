using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督作业信息
    /// </summary>
    public interface SuperviseWorkInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 根据监督任务获取作业信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SuperviseWorkInfoEntity> GetList(string strwhere);
         /// <summary>
        /// 根据分配任务id和班组id获取作业信息
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        IEnumerable<SuperviseWorkInfoEntity> GetList(string taskshareid, string teamid);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SuperviseWorkInfoEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, SuperviseWorkInfoEntity entity);

        /// <summary>
        ///根据分配id删除作业信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveWorkByTaskShareId(string keyValue);
        #endregion
    }
}
