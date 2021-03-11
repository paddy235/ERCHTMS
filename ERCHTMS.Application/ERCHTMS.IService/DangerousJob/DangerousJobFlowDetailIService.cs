using BSFramework.Util.WebControl;
using ERCHTMS.Entity.DangerousJob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业流程流转表
    /// </summary>
    public interface DangerousJobFlowDetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DangerousJobFlowDetailEntity> GetList();

        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DangerousJobFlowDetailEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable GetEntityByT(string keyValue, string fid);
        /// <summary>
        /// 获取当前审核人id
        /// </summary>
        /// <param name="BusinessId"></param>
        /// <param name="flowdetailid">流转表id</param>
        /// <returns></returns>
        string GetCurrentStepUser(string BusinessId, string flowdetailid);
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
        void SaveForm(string keyValue, DangerousJobFlowDetailEntity entity);
        /// <summary>
        /// 审核保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void CheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity);
        /// <summary>
        ///安全风险审批单 审核保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void ApprovalFormCheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity);

        #endregion
    }
}
