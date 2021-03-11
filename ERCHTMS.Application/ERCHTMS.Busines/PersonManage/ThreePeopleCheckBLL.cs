using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// 描 述：三种人审批业务表
    /// </summary>
    public class ThreePeopleCheckBLL
    {
        private ThreePeopleCheckIService service = new ThreePeopleCheckService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ThreePeopleCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
         /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="applyId">申请表Id</param>
        /// <returns></returns>
        public IEnumerable<ThreePeopleInfoEntity> GetUserList(string applyId)
        {
            return service.GetUserList(applyId);
        }
        public DataTable GetItemList(string id)
        {
            return service.GetItemList(id);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ThreePeopleCheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ThreePeopleCheckEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SaveForm(string keyValue, ThreePeopleCheckEntity entity, List<ThreePeopleInfoEntity> list, ERCHTMS.Entity.OutsourcingProject.AptitudeinvestigateauditEntity auditInfo = null)
        {
            return service.SaveForm(keyValue, entity, list, auditInfo);
        }
        #endregion
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string applyId="")
        {
            return service.CheckAuditPower(currUser, out state, moduleName, createdeptid, applyId);
        }
    }
}
