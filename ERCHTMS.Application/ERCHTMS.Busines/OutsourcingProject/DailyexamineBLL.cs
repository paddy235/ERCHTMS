using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：日常考核表
    /// </summary>
    public class DailyexamineBLL
    {
        private DailyexamineIService service = new DailyexamineService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DailyexamineEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DailyexamineEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            return service.GetMaxCode();
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
        /// 日常考核汇总
        /// </summary>
        /// <param name="pagination">查询语句</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        public DataTable GetExamineCollent(Pagination pagination, string queryJson) {
            return service.GetExamineCollent(pagination, queryJson);
        }
        /// <summary>
        /// 导出使用
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetExportExamineCollent(Pagination pagination, string queryJson)
        {
            return service.GetExportExamineCollent(pagination, queryJson);
        }
        /// <summary>
        /// 首页提醒
        /// </summary>
        /// <returns></returns>
        public int CountIndex(ERCHTMS.Code.Operator user)
        {
            return service.CountIndex(user);
        }

        #region  当前登录人是否有权限审核并获取下一次审核权限实体
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid)
        {
            return service.CheckAuditPower(currUser, out state, moduleName, createdeptid);
        }
        #endregion
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
        public void SaveForm(string keyValue, DailyexamineEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
