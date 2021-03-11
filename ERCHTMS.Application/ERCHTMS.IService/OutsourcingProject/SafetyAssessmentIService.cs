using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核主表
    /// </summary>
    public interface SafetyAssessmentIService
    {
        #region 获取数据
        /// <summary>
        /// 导出安全考核汇总
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable ExportDataTotal(string time, string deptid);

        /// <summary>
        /// 获取内部部门
        /// </summary>
        /// <returns></returns>
        DataTable GetInDeptData();

        /// <summary>
        /// 内部部门考核导出
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable ExportDataInDept(string time, string deptid);

        /// <summary>
        /// 外部部门考核
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable ExportDataOutDept(string time, string deptid);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafetyAssessmentEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafetyAssessmentEntity GetEntity(string keyValue);

        int GetFormJsontotal(string keyValue);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        string GetMaxCode();

        /// <summary>
        ///  获取当前角色待审批的数量
        /// </summary>
        /// <returns></returns>
        string GetApplyNum();
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
        void SaveForm(string keyValue, SafetyAssessmentEntity entity);
        #endregion

        #region  当前登录人是否有权限审核并获取下一次审核权限实体
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string startnum);

        Flow GetAuditFlowData(string keyValue, string urltype);
        #endregion
    }
}
