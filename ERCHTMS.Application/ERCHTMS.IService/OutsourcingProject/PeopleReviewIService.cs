using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.EmergencyPlatform;

namespace ERCHTMS.IService.OutsourcingProject
{
    public interface PeopleReviewIService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<PeopleReviewEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        PeopleReviewEntity GetEntity(string keyValue);

        DataTable GetPagePeopleReviewListJson(Pagination pagination, string queryJson);
        /// <summary>
        /// 演练记录评价流程
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="DrillRecordId">演练记录Id</param>
        /// <param name="curFlowId">当前节点</param>
        /// <param name="isUseSetting"></param>
        /// <returns></returns>
        ManyPowerCheckEntity CheckEvaluateForNextFlow(Operator currUser, string moduleName, DrillplanrecordEntity entity);
        #region  高风险作业
        /// <summary>
        /// 通过作业单位查询配置审核的下一个审核单位和角色
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="outengineerid"></param>
        /// <param name="flowtype"></param>
        /// <param name="curFlowId"></param>
        /// <param name="isBack"></param>
        /// <returns></returns>
        ManyPowerCheckEntity CheckAuditForNextByWorkUnit(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack);


        ManyPowerCheckEntity CheckAuditForNextByOutsourcing(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack, bool isUseSetting,string projectid="");
        
        /// <summary>
        /// 起重吊装作业专用(审核专业)
        /// </summary>
        /// <param name="currUser">当前人</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="workUnitId">审核专业ID</param>
        /// <param name="curFlowId">当前节点ID</param>
        /// <param name="isBack">是否回退</param>
        /// <returns></returns>
        ManyPowerCheckEntity CheckAuditForNextByLiftHoist(Operator currUser, string moduleName, string workUnitId, string curFlowId, bool isBack);
        #endregion

        #region 按顺序进行获取下一个流程节点
        /// <summary>
        /// 按顺序进行获取下一个流程节点
        /// </summary>
        /// <param name="currUser"></param>
        /// <param name="moduleName"></param>
        /// <param name="outengineerid"></param>
        /// <param name="flowtype"></param>
        /// <param name="curFlowId"></param>
        /// <param name="isBack"></param>
        /// <returns></returns>
        ManyPowerCheckEntity CheckAuditForNextFlow(Operator currUser, string moduleName, string outengineerid, string curFlowId, bool isBack, bool isUseSetting);
        #endregion
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="outengineerid">工程Id</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string outengineerid, Boolean isStep = true, string CurFlowId = "", string EngineerletDeptId = "");
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);

        //void RemovePeople(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        List<string> SaveForm(string keyValue, PeopleReviewEntity entity);

        /// <summary>
        /// 人员资质对接班组
        /// </summary>
        /// <param name="user"></param>
        void SaveUser(List<UserEntity> user);
    }
}
