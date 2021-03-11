using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资领用申请
    /// </summary>
    public interface SuppliesacceptIService
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
        IEnumerable<SuppliesacceptEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SuppliesacceptEntity GetEntity(string keyValue);

        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);
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
        string SaveForm(string keyValue, SuppliesacceptEntity entity);

        /// <summary>
        /// 审核表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        /// <param name="DetailData"></param>
        string AuditForm(string keyValue, AptitudeinvestigateauditEntity aentity,string DetailData);
        #endregion
    }
}
