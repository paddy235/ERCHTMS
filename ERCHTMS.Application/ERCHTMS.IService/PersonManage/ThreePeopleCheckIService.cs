using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// 描 述：三种人审批业务表
    /// </summary>
    public interface ThreePeopleCheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ThreePeopleCheckEntity> GetList(string queryJson);
        DataTable GetItemList(string id);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ThreePeopleCheckEntity GetEntity(string keyValue);

        DataTable GetPageList(Pagination pagination, string queryJson);

         /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="applyId">申请表Id</param>
        /// <returns></returns>
        IEnumerable<ThreePeopleInfoEntity> GetUserList(string applyId);
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
        void SaveForm(string keyValue, ThreePeopleCheckEntity entity);
        bool SaveForm(string keyValue, ThreePeopleCheckEntity entity, List<ThreePeopleInfoEntity> list, AptitudeinvestigateauditEntity auditInfo = null);

        #endregion

         /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
       ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string applyId="");


    }
}
