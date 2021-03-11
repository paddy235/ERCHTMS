using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核主表
    /// </summary>
    public class SafetyAssessmentBLL
    {
        private SafetyAssessmentIService service = new SafetyAssessmentService();

        #region 获取数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tiem"></param>
        /// <returns></returns>
        public DataTable ExportDataTotal(string time, string deptid)
        {
            return service.ExportDataTotal(time, deptid);
        }

        /// <summary>
        /// 获取内部部门单位
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetInDeptData()
        {
            return service.GetInDeptData();
        }

        /// <summary>
        /// 内部部门导出
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable ExportDataInDept(string time, string deptid)
        {
            return service.ExportDataInDept(time, deptid);
        }

        /// <summary>
        /// 外部导出
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable ExportDataOutDept(string time, string deptid)
        {
            return service.ExportDataOutDept(time, deptid);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyAssessmentEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyAssessmentEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public int GetFormJsontotal(string keyValue)
        {
            return service.GetFormJsontotal(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取编号，每月初始化后三位流水号
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            return service.GetMaxCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetApplyNum()
        {
            return service.GetApplyNum();
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
        public void SaveForm(string keyValue, SafetyAssessmentEntity entity)
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
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string startnum)
        {
            return service.CheckAuditPower(currUser, out state, moduleName, createdeptid, startnum);
        }

        /// <summary>
        /// 查询审核流程图
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：0：安全考核</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype)
        {
            return service.GetAuditFlowData(keyValue, urltype);
        }
        #endregion
    }
}
