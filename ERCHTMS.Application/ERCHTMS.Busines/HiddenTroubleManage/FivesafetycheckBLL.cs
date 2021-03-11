using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：五定安全检查
    /// </summary>
    public class FivesafetycheckBLL
    {
        private FivesafetycheckIService service = new FivesafetycheckService();

        #region 获取数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDeptByName(string name)
        {
            return service.GetDeptByName(name);
        }

        /// <summary>
        /// 根据检查类型编号查询首页
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public DataTable DeskTotalByCheckType(string itemcode)
        {
            return service.DeskTotalByCheckType(itemcode);
        }

        /// <summary>
        /// 返回安全考核不同类型待审批的数量的数量
        /// </summary>
        /// <param name="fivetype">检查类型</param>
        /// <param name="istopcheck"> 0:上级公司检查 1：公司安全检查</param>
        /// <param name="type"> 0:审核流程，1：整改  2：验收</param>
        /// <returns></returns>
        public string GetApplyNum(string fivetype, string istopcheck, string type)
        {
            return service.GetApplyNum(fivetype, istopcheck, type);
        }

        /// <summary>
        /// 根据sql查询返回
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return service.GetInfoBySql(sql);
        }
        public DataTable ExportAuditTotal(string keyvalue)
        {
            return service.ExportAuditTotal(keyvalue);
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

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FivesafetycheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        public IEnumerable<UserEntity> GetStepDept(ManyPowerCheckEntity powerinfo, string id)
        {
            return service.GetStepDept(powerinfo, id);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination, queryJson);
        }

        /// <summary>
        /// 获取整改情况列表分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAuditListJson(Pagination pagination, string queryJson)
        {
            return service.GetAuditListJson(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FivesafetycheckEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, FivesafetycheckEntity entity)
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
