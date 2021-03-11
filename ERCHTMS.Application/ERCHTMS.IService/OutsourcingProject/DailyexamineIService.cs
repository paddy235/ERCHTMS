using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：日常考核表
    /// </summary>
    public interface DailyexamineIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DailyexamineEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DailyexamineEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        string GetMaxCode();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 日常考核汇总
        /// </summary>
        /// <param name="pagination">查询语句</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        DataTable GetExamineCollent(Pagination pagination, string queryJson);
        /// <summary>
        /// 导出使用
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetExportExamineCollent(Pagination pagination, string queryJson);

        /// <summary>
        /// 首页提醒
        /// </summary>
        /// <returns></returns>
        int CountIndex(ERCHTMS.Code.Operator user);

        #region  当前登录人是否有权限审核并获取下一次审核权限实体
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid);
        #endregion
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
        void SaveForm(string keyValue, DailyexamineEntity entity);
        #endregion
    }
}
