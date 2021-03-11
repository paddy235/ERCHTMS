using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：安全措施变动申请表
    /// </summary>
    public class SafetychangeBLL
    {
        private SafetychangeIService service = new SafetychangeService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetychangeEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public Flow GetFlow(string keyValue, List<string> modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue, List<string> modulename, string flowid, bool isendflow, string workdeptid, string projectid, string specialtytype = "")
        {
            return service.GetAppFlowList(keyValue, modulename, flowid, isendflow, workdeptid,projectid, specialtytype);
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取安全设施变动台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson)
        {
            return service.GetLedgerList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetychangeEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public DataTable FindTable(string sql)
        {
            return service.FindTable(sql);
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
        public void SaveForm(string keyValue, SafetychangeEntity entity)
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
    }
}
