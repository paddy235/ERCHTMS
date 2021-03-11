using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置实例表
    /// </summary>
    public interface WfInstanceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<WfInstanceEntity> GetList(string queryJson);



        #region 获取特定的流程配置实例
        /// <summary>
        /// 获取特定的流程配置实例
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="rankname"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        DataTable GetProcessData();
        #endregion
        /// <summary>
        /// 获取流程对象
        /// </summary>
        /// <param name="instanceid"></param>
        /// <returns></returns>
        DataTable GetActivityData(string instanceid);
        List<WfInstanceEntity> GetListByArgs(string orgid, string rankname, string mark);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        WfInstanceEntity GetEntity(string keyValue);
        #endregion

        #region  流程配置实例信息
        /// <summary>
        /// 流程配置实例信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetWfInstanceInfoPageList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, WfInstanceEntity entity);

        #region 批量更新流程实例
        /// <summary>
        /// 批量更新流程实例Id
        /// </summary>
        /// <param name="typename"></param>
        void BatchUpdateInstance(string typename);
        #endregion

        #endregion
    }
}