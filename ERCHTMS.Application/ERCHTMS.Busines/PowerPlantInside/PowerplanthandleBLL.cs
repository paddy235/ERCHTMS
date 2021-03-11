using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using ERCHTMS.Service.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Busines.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理
    /// </summary>
    public class PowerplanthandleBLL
    {
        private PowerplanthandleIService service = new PowerplanthandleService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PowerplanthandleEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PowerplanthandleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

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

        /// <summary>
        /// 事故事件处理待办
        /// </summary>
        /// <returns></returns>
        public List<string> ToAuditPowerHandle()
        {
            return service.ToAuditPowerHandle();
        }

        public DataTable GetAuditInfo(string keyValue, string modulename)
        {
            return service.GetAuditInfo(keyValue, modulename);
        }

        /// <summary>
        /// 获取流程图整改信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public DataTable GetReformInfo(string keyValue)
        {
            return service.GetReformInfo(keyValue);
        }

        /// <summary>
        /// 获取流程图验收信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public DataTable GetCheckInfo(string keyValue, string modulename)
        {
            return service.GetCheckInfo(keyValue, modulename);
        }

        public DataTable GetTableBySql(string sql)
        {
            return service.GetTableBySql(sql);
        }

        public string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "")
        {
            return service.GetUserName(flowdeptid, flowrolename, type, specialtytype);
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue)
        {
            return service.GetAppFlowList(keyValue);
        }

        public List<CheckFlowData> GetAppFullFlowList(string keyValue)
        {
            return service.GetAppFullFlowList(keyValue);
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
        public void SaveForm(string keyValue, PowerplanthandleEntity entity)
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

        /// <summary>
        /// 更新事故事件记录状态
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateApplyStatus(string keyValue)
        {
            try
            {
                service.UpdateApplyStatus(keyValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
