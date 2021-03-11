using ERCHTMS.Entity.HiddenTroubleManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERCHTMS.IService.HiddenTroubleManage
{
    public interface HTWorkFlowIService
    {

        #region 判断当前流程是否存在
        /// <summary>
        /// 判断当前流程是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool IsHaveCurWorkFlow(string mark);
        #endregion

        /// <summary>
        /// 删除工作流实例
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        bool DeleteWorkFlowObj(string objectID);

        #region  创建流程实例
        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="wfObj">隐患流程编码</param>
        /// <param name="objectID">实例ID</param>
        /// <param name="curUser">当前用户ID</param>
        /// <returns></returns>
        bool CreateWorkFlowObj(string wfObj, string objectID, string curUser, string submittype = "提交");
        #endregion

        #region 流程提交 / 退回
        /// <summary>
        /// 流程提交  
        /// </summary>
        /// <param name="objectId">实例ID</param>
        /// <param name="participant">参与者</param>
        /// <param name="wfFlag">流程转向</param>
        /// <param name="curUser">当前用户</param>
        /// <returns></returns>
        PushData SubmitWorkFlow(string objectId, string participant, string wfFlag, string curUser, string submittype = "提交");
        #endregion

        #region 获取当前的流程节点名称
        /// <summary>
        /// 获取当前的流程节点
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        string QueryTagNameByCurrentWF(string objectID);
        #endregion

        #region 获取当前的流程节点标记
        /// <summary>
        /// 获取当前的流程节点标记
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        string QueryTagByCurrentWF(string objectID);
        #endregion

        #region  通过当前实例ID，查询数据库，判断当前流程是否存在
        /// <summary>
        /// 通过当前实例ID，查询数据库，判断当前流程是否存在
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        bool IsHavaWFCurrentObject(string objectID);
        #endregion

        #region 更新业务状态
        /// <summary>
        /// 更新业务状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        bool UpdateWorkStreamByObjectId(string tableName, string fieldName, string objectId);
        #endregion

        #region 流程提交，只提交实例不更改当前流程状态,即当前是隐患评估，提交后可能还是隐患评估，并且评估人员发生变化
        /// <summary>
        /// 流程提交，只提交实例不更改当前流程状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        PushData SubmitWorkFlowNoChangeStatus(string objectId, string participant, string curUser, string submittype = "提交");
        #endregion

        Flow GetActionList(string keyValue);
        Flow GetStandardApplyActionList(string keyValue);
        Flow GetDepartPlanApplyActionList(string keyValue);
        Flow GetPersonPlanApplyActionList(string keyValue);
        #region 获取违章的流程图对象
        /// <summary>
        /// 获取违章的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        Flow GetLllegalActionList(string keyValue);
        #endregion

        /// <summary>
        /// 问题流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        Flow GetQuestionActionList(string keyValue);
        DataTable GetBackFlowObjectByKey(string keyValue);

        #region 获取流程导向图
        /// <summary>
        /// 获取流程导向图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable QueryWorkFlowMapForApp(string keyValue, string mode);
        #endregion

        #region 获取公共的流程图对象
        /// <summary>
        /// 获取隐患的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        Flow GetCommonFlow(Flow flow, string keyValue);
        #endregion
    }
}
