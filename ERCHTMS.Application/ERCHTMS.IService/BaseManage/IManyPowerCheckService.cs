using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;
using BSFramework.Data.Repository;
using ERCHTMS.Code;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public interface IManyPowerCheckService
    {
        #region 获取数据
        List<ManyPowerCheckEntity> GetList(string orgCode, string moduleName);

        List<ManyPowerCheckEntity> GetList(string orgCode, string moduleName, int serialnum);
        /// <summary>
        /// 根据机构编码和模块编码查询权限配置
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <param name="moduleNo">模块编码</param>
        /// <returns></returns>
        List<ManyPowerCheckEntity> GetListByModuleNo(string orgCode, string moduleNo);
        List<ManyPowerCheckEntity> GetListBySerialNum(string orgCode, string moduleName);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetManyPowerCheckEntityPage(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ManyPowerCheckEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取下一步审核节点
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="curFlowId"></param>
        /// <returns></returns>
        ManyPowerCheckEntity CheckAuditForNext(Operator currUser, string moduleName, string curFlowId);

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
        string GetApproveUserAccount(string currflowid, string businessid, string NextStepApproveUserAccount = "", string specialtytype = "", string executedept = "", string outsourcingdept = "", string createdept = "", string professionaldept = "", string ondutydept = "", string supervisordept = "",string outengineerid="");


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
        string GetApproveUserId(string currflowid, string businessid, string NextStepApproveUserAccount = "", string specialtytype = "", string executedept = "", string outsourcingdept = "", string createdept = "", string professionaldept = "", string ondutydept = "", string supervisordept = "",string outengineerid="");
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
        void SaveForm(string keyValue, ManyPowerCheckEntity entity);
        #endregion
    }
}
