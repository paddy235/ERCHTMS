using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.MessageManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.SignalR;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Linq.Expressions;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public class ManyPowerCheckBLL
    {
        private IManyPowerCheckService service = new ManyPowerCheckService();

        #region 获取数据
        /// <summary>
        /// 获取列表(按autoid asc,createdate asc排序)
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <param name="moduleName">模块名称</param>
        /// <remarks></remarks>
        /// <returns></returns>
        public List<ManyPowerCheckEntity> GetList(string orgCode, string moduleName)
        {
            return service.GetList(orgCode, moduleName);
        }
        /// <summary>
        /// 根据机构编码和模块编码查询权限配置
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <param name="moduleNo">模块编码</param>
        /// <returns></returns>
        public List<ManyPowerCheckEntity> GetListByModuleNo(string orgCode, string moduleNo)
        {
            return service.GetListByModuleNo(orgCode, moduleNo);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="moduleName"></param>
        /// <param name="flowType"></param>
        /// <returns></returns>
        public List<ManyPowerCheckEntity> GetList(string orgCode, string moduleName, int serialnum)
        {
            return service.GetList(orgCode, moduleName, serialnum);
        }

        public List<ManyPowerCheckEntity> GetListBySerialNum(string orgCode, string moduleName) {
            return service.GetListBySerialNum(orgCode, moduleName);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <param name="pagination">分页参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetManyPowerCheckEntityPage(Pagination pagination, string queryJson)
        {
            return service.GetManyPowerCheckEntityPage(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ManyPowerCheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public ManyPowerCheckEntity CheckAuditForNext(Operator currUser, string moduleName, string curFlowId)
        {
            return service.CheckAuditForNext(currUser, moduleName, curFlowId);
        }

        /// <summary>
        /// 获取审核人账号
        /// </summary>
        /// <param name="currflowid">当前流程节点</param>
        /// <param name="businessid">业务逻辑节点ID</param>
        /// <param name="NextStepApproveUserAccount">指定下一步审核人账号</param>
        /// <param name="specialtytype">专业类别</param>
        /// <param name="executedept">执行部门</param>
        /// <param name="outsourcingdept">外包单位</param>
        /// <param name="createdept">创建部门</param>
        /// <param name="professionaldept">专业部门</param>
        /// <param name="ondutydept">值班部门</param>
        /// <param name="supervisordept">监理单位</param>
        /// <param name="outengineerid">外包工程id</param>
        /// <returns></returns>
        public string GetApproveUserAccount(string currflowid, string businessid, string NextStepApproveUserAccount = "", string specialtytype = "", string executedept = "", string outsourcingdept = "", string createdept = "", string professionaldept = "", string ondutydept = "", string supervisordept = "",string outengineerid="")
        {
            return service.GetApproveUserAccount(currflowid, businessid, NextStepApproveUserAccount, specialtytype, executedept, outsourcingdept, createdept, professionaldept, ondutydept,supervisordept, outengineerid);
        }

        /// <summary>
        /// 获取审核人账号
        /// </summary>
        /// <param name="currflowid">当前流程节点</param>
        /// <param name="businessid">业务逻辑节点ID</param>
        /// <param name="NextStepApproveUserAccount">指定下一步审核人账号</param>
        /// <param name="specialtytype">专业类别</param>
        /// <param name="executedept">执行部门</param>
        /// <param name="outsourcingdept">外包单位</param>
        /// <param name="createdept">创建部门</param>
        /// <param name="professionaldept">专业部门</param>
        /// <param name="ondutydept">值班部门</param>
        /// <param name="supervisordept">监理单位</param>
        /// <param name="outengineerid">外包工程ID</param>
        /// <returns></returns>
        public string GetApproveUserId(string currflowid, string businessid, string NextStepApproveUserAccount = "", string specialtytype = "", string executedept = "", string outsourcingdept = "", string createdept = "", string professionaldept = "", string ondutydept = "", string supervisordept = "",string outengineerid="")
        {
            return service.GetApproveUserId(currflowid, businessid, NextStepApproveUserAccount, specialtytype, executedept, outsourcingdept, createdept, professionaldept, ondutydept, supervisordept, outengineerid);
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
        public void SaveForm(string keyValue, ManyPowerCheckEntity entity)
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
