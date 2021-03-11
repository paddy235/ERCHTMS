using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using BSFramework.Util;
using System.IO;
using Newtonsoft.Json;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    public class HTWorkFlowBLL
    {
        private HTWorkFlowIService service = new HTWorkFlowService();
        private LllegalRegisterIService lllegalservice = new LllegalRegisterService();
        private UserBLL userbll = new UserBLL();
        private IUserInfoService userinfo = new UserInfoService();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务


        #region 判断当前流程是否存在
        /// <summary>
        /// 判断当前流程是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsHaveCurWorkFlow(string mark)
        {
            try
            {
                return service.IsHaveCurWorkFlow(mark);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public string GetCurUserWfAuth(string rankname, string workflow, string endflow, string mark, string submittype, string arg1 = "", string arg2 = "", string arg3 = "", string businessid = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = businessid; //
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankname = rankname;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;
            wfentity.argument1 = arg1;
            wfentity.argument2 = arg2;
            wfentity.argument3 = arg3;

            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            if (result.ishave)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 删除工作流实例
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public bool DeleteWorkFlowObj(string objectID)
        {
            try
            {
                return service.DeleteWorkFlowObj(objectID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="wfObj"></param>
        /// <param name="objectID"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public bool CreateWorkFlowObj(string wfObj, string objectID, string curUser, string submittype = "提交")
        {
            try
            {
                return service.CreateWorkFlowObj(wfObj, objectID, curUser, submittype);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 根据实例id获取对应的退回流程实例相关对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetBackFlowObjectByKey(string keyValue)
        {
            return service.GetBackFlowObjectByKey(keyValue);
        }

        #region 提交流程
        /// <summary>
        /// 提交流程
        /// </summary>
        /// <param name="result"></param>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="wfFlag"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public int SubmitWorkFlow(string objectId, string participant, string wfFlag, string curUser, string submittype = "提交")
        {
            try
            {
                //返回流程提交的结果
                PushData pushdata = service.SubmitWorkFlow(objectId, participant, wfFlag, curUser, submittype);

                return pushdata.IsSucess;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 提交流程
        /// <summary>
        /// 提交流程
        /// </summary>
        /// <param name="result"></param>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="wfFlag"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public int SubmitWorkFlow(string startflow, string endflow, string objectId, string participant, string wfFlag, string curUser, string submittype = "提交")
        {
            try
            {
                //返回流程提交的结果
                PushData pushdata = service.SubmitWorkFlow(objectId, participant, wfFlag, curUser, submittype);

                PushMessageForLllegal(objectId, startflow, endflow, participant);

                return pushdata.IsSucess;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 提交流程
        /// <summary>
        /// 提交流程
        /// </summary>
        /// <param name="result"></param>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="wfFlag"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public int SubmitWorkFlow(WfControlObj control, WfControlResult result, string objectId, string participant, string wfFlag, string curUser)
        {
            try
            {
                //返回流程提交的结果
                PushData pushdata = service.SubmitWorkFlow(objectId, participant, wfFlag, curUser, control.submittype);

                //极光推送
                PushMessageForWorkFlow(pushdata.ProcessName, pushdata.NextActivityName, control, result);

                return pushdata.IsSucess;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region 流程提交，只提交实例不更改当前流程状态,即当前是隐患评估，提交后可能还是隐患评估，并且评估人员发生变化
        /// <summary>
        /// 流程提交，只提交实例不更改当前流程状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public bool SubmitWorkFlowNoChangeStatus(string objectId, string participant, string curUser, string submittype = "提交")
        {
            try
            {
                PushData pushdata = service.SubmitWorkFlowNoChangeStatus(objectId, participant, curUser, submittype);

                return pushdata.IsSucess > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 流程提交，只提交实例不更改当前流程状态,即当前是隐患评估，提交后可能还是隐患评估，并且评估人员发生变化
        /// <summary>
        /// 流程提交，只提交实例不更改当前流程状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public bool SubmitWorkFlowNoChangeStatus(string startflow, string endflow, string objectId, string participant, string curUser, string submittype = "提交")
        {
            try
            {
                PushData pushdata = service.SubmitWorkFlowNoChangeStatus(objectId, participant, curUser, submittype);

                PushMessageForLllegal(objectId, startflow, endflow, participant);

                return pushdata.IsSucess > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region 其他更新流程
        /// <summary>
        /// 其他更新流程
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool UpdateFlowStateByObjectId(string tableName, string fieldName, string objectId)
        {
            Operator user = OperatorProvider.Provider.Current();

            string tagName = QueryTagNameByCurrentWF(objectId);

            bool issucessful = false;

            try
            {
                issucessful = service.UpdateWorkStreamByObjectId(tableName, fieldName, objectId);

                //违章部分
                if (tableName == "bis_lllegalregister")
                {
     
                    LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL();
                    #region 违章评分对象
                    if (tagName == "流程结束")
                    {
                        LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(objectId);
                        try
                        {
                            //添加分数到
                            lllegalregisterbll.AddLllegalScore(entity);
                        }
                        catch (Exception ex)
                        {
                            int _actionType = 4;
                            LogEntity logEntity = new LogEntity();
                            logEntity.CategoryId = _actionType;
                            logEntity.OperateTypeId = _actionType.ToString();
                            logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                            logEntity.OperateAccount = user.UserName;
                            logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                            logEntity.ExecuteResult = 1;
                            logEntity.Module = SystemInfo.CurrentModuleName;
                            logEntity.ModuleId = SystemInfo.CurrentModuleId;
                            logEntity.ExecuteResultJson = "操作信息:推送违章对接培训平台接口失败, 错误信息:" + ex.ToJson();
                            logEntity.WriteLog();
                        }
                    }
                    #endregion



                    DataItemDetailBLL itemBll = new DataItemDetailBLL();
                    string bzApiUrl = itemBll.GetItemValue("bzApiUrl");
                    bool ispost = false;
                    #region 对接班组
                    if (!string.IsNullOrEmpty(bzApiUrl))
                    {
                        var entity = lllegalregisterbll.GetEntity(objectId);

                        if (null != entity)
                        {
                            string createuserid = entity.CREATEUSERID;
                            //登记违章
                            if (tagName == "违章整改" || (tagName == "流程结束" && entity.ADDTYPE == "1"))
                            {
                                ispost = true;
                            }

                            if (ispost)
                            {
                                var result = new
                                {
                                    userId = createuserid,
                                    data = 4 //登记违章
                                };

                                try
                                {
                                    string requestUrl = bzApiUrl + "/SafetyScore/AddScore";
                                    WebClient wc = new WebClient();
                                    wc.Credentials = CredentialCache.DefaultCredentials;
                                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                                    nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(result));
                                    wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                                    wc.UploadValuesAsync(new Uri(requestUrl), nc);

                                    //string res = HttpMethods.HttpPost(Path.Combine(bzApiUrl, "SafetySocre", "AddScore"), "json=" + JsonConvert.SerializeObject(result));
                                }
                                catch (Exception ex)
                                {
                                
                                    int _actionType = 5;
                                    LogEntity logEntity = new LogEntity();
                                    logEntity.CategoryId = _actionType;
                                    logEntity.OperateTypeId = _actionType.ToString();
                                    logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                                    logEntity.OperateAccount = user.UserName;
                                    logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                                    logEntity.ExecuteResult = 1;
                                    logEntity.Module = SystemInfo.CurrentModuleName;
                                    logEntity.ModuleId = SystemInfo.CurrentModuleId;
                                    logEntity.ExecuteResultJson = "操作信息:推送班组端积分接口失败, 错误信息:" + ex.Message + " , 异常信息:" + ex.InnerException + " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson();
                                    logEntity.WriteLog();

                                    string fileName = "推送班组端积分接口" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyyMMddHHmmss-") + "推送班组端积分接口错误信息:" + ex.Message + " , 异常信息:" + ex.InnerException +
                                        " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson() + ";\r\n");
                                }

                            }
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                int _actionType = 4;
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = _actionType;
                logEntity.OperateTypeId = _actionType.ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                logEntity.OperateAccount = user.UserName;
                logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                logEntity.ExecuteResult = 1;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.ExecuteResultJson = "操作错误信息:" + ex.ToJson();
                logEntity.WriteLog();
            }

            return issucessful;
        }
        #endregion

        #region 流程提交，只提交实例不更改当前流程状态,即当前是隐患评估，提交后可能还是隐患评估，并且评估人员发生变化
        /// <summary>
        /// 流程提交，只提交实例不更改当前流程状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public bool SubmitWorkFlowNoChangeStatus(WfControlObj control, WfControlResult result, string objectId, string participant, string curUser)
        {
            try
            {
                PushData pushdata = service.SubmitWorkFlowNoChangeStatus(objectId, participant, curUser, control.submittype);
                //因不改变流程状态
                pushdata.NextActivityName = control.startflow;
                //极光推送
                PushMessageForWorkFlow(pushdata.ProcessName, pushdata.NextActivityName, control, result);

                return pushdata.IsSucess > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 隐患下的极光推送
        /// <summary>
        /// 隐患下的极光推送
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="nextActivityName"></param>
        /// <param name="control"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool PushMessageForWorkFlow(string processName, string nextActivityName, WfControlObj control, WfControlResult result)
        {
            bool isSucessful = true;

            string pushcode = string.Empty; //推送代码

            string pushaccount = string.Empty; //推送到下一流程的人员账户

            string pushusernames = string.Empty; //推送到下一流程的人员

            string keyvalue = string.Empty; //关联业务id

            bool isPdImportant = false; //是否判定重大隐患


            //隐患处理流程下的极光消息推送
            if (null != result)
            {
                if (processName == "隐患处理审批")
                {
                    #region 获取极光推送代码
                    switch (control.startflow)
                    {
                        //当前流程结点
                        case "隐患登记":
                            //推送到下一流程的结点
                            if (nextActivityName == "隐患整改")
                            {
                                isPdImportant = true;
                                pushcode = "YH003";//待整改
                            }
                            else if (nextActivityName == "隐患评估")
                            {
                                pushcode = "YH005"; //待评估
                            }
                            else if (nextActivityName == "隐患完善")
                            {
                                pushcode = "YH001"; //待完善
                            }
                            else if (nextActivityName == "制定整改计划")
                            {
                                pushcode = "YH016";  //待制定整改计划
                            }
                            break;
                        case "隐患完善":
                            //推送到下一流程的结点
                            if (nextActivityName == "隐患整改")
                            {
                                isPdImportant = true;
                                pushcode = "YH003";//待整改
                            }
                            else if (nextActivityName == "隐患评估")
                            {
                                pushcode = "YH005"; //待评估
                            }
                            break;
                        //当前流程结点
                        case "隐患评估":
                            if (nextActivityName == "隐患整改")
                            {
                                isPdImportant = true;
                                pushcode = "YH003";//待整改
                            }
                            else if (nextActivityName == "隐患登记")
                            {
                                pushcode = "YH006";//评估未通过
                            }
                            else if (nextActivityName == "制定整改计划")
                            {
                                //退回的情况下
                                if (control.submittype.Contains("退回"))
                                {
                                    pushcode = "YH006";//评估未通过
                                }
                                else
                                {
                                    pushcode = "YH016";  //待制定整改计划
                                }
                            }
                            else if (nextActivityName == "隐患评估")
                            {
                                //退回的情况下
                                if (control.submittype.Contains("退回"))
                                {
                                    pushcode = "YH006";//评估未通过
                                }
                                else
                                {
                                    pushcode = "YH005";  //待评估
                                }
                            }
                            break;
                        case "制定整改计划":
                            if (nextActivityName == "隐患评估")
                            {
                                pushcode = "YH005";//待评估
                            }
                            if (nextActivityName == "隐患整改")
                            {
                                isPdImportant = true;
                                pushcode = "YH003";//待整改
                            }
                            break;
                        case "隐患整改":
                            if (nextActivityName == "隐患验收")
                            {
                                pushcode = "YH008";//待验收
                            }
                            else if (nextActivityName == "隐患登记" || nextActivityName == "隐患评估" || nextActivityName == "隐患完善")
                            {
                                pushcode = "YH015";//未整改退回
                            }
                            break;
                        case "隐患验收":
                            if (nextActivityName == "复查验证")
                            {
                                pushcode = "YH014";//待复查验证
                            }
                            else if (nextActivityName == "整改效果评估")
                            {
                                pushcode = "YH009";//待整改效果评估
                            }
                            else if (nextActivityName == "隐患整改")
                            {
                                pushcode = "YH007";//验收未通过
                            }
                            break;
                        case "复查验证":
                            if (nextActivityName == "隐患整改")
                            {
                                pushcode = "YH013";//复查验证未通过
                            }
                            break;
                        case "整改效果评估":
                            if (nextActivityName == "隐患整改")
                            {
                                pushcode = "YH010";//整改效果评估未通过	
                            }
                            break;
                        case "整改延期申请":
                            if (nextActivityName == "整改延期审批")
                            {
                                pushcode = "YH011";//整改延期待审批	
                            }
                            break;
                        case "整改延期审批":
                            if (nextActivityName == "整改延期审批")
                            {
                                pushcode = "YH011";//整改延期待审批	
                            }
                            else if (nextActivityName == "整改延期退回")
                            {
                                pushcode = "YH012";//整改延期审批未通过
                            }
                            break;

                    }
                    #endregion

                    #region 获取对应用户信息
                    if (!string.IsNullOrEmpty(result.username))
                    {
                        pushusernames = result.username;
                    }
                    if (!string.IsNullOrEmpty(result.actionperson))
                    {
                        pushaccount = result.actionperson;

                        //如果当前用户姓名为空,则代入账号查询
                        if (string.IsNullOrEmpty(pushusernames))
                        {
                            string[] accounts = pushaccount.Split(',');

                            DataTable dt = userbll.GetUserTable(accounts);

                            foreach (DataRow row in dt.Rows)
                            {
                                pushusernames += row["REALNAME"].ToString() + ",";
                            }
                            if (!string.IsNullOrEmpty(pushusernames))
                            {
                                pushusernames = pushusernames.Substring(0, pushusernames.Length - 1);  //接收人姓名
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(control.businessid))
                    {
                        keyvalue = control.businessid;
                    }
                    #endregion

                    #region 判定重大隐患提醒
                    if (isPdImportant)
                    {
                        string temppushcode = "YH004";//重大隐患提醒

                        string temppushaccount = ",";//推送账户

                        string temppushusername = string.Empty; //推送人

                        if (control.rankname.Contains("重大"))
                        {
                            if (!string.IsNullOrEmpty(pushaccount))
                            {
                                /*
                                  提醒 违章整改责任单位负责人、专工、安全管理人员/安全主管部门专工、安全管理人员、负责人
                                */
                                //整改人相关信息
                                UserInfoEntity uentity = userinfo.GetUserInfoByAccount(pushaccount);

                                string rolestr = dataitemdetailbll.GetItemValue("重大隐患提醒角色");

                                if (!string.IsNullOrEmpty(rolestr))
                                {
                                    string[] rolearray = rolestr.Split(';');

                                    foreach (string str in rolearray)
                                    {
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            string[] currolestr = str.Split('|');
                                            string curdeptid = string.Empty;
                                            if (currolestr[0] == "ChangeDept")
                                            {
                                                curdeptid = uentity.DepartmentCode;
                                            }
                                            else
                                            {
                                                curdeptid = currolestr[0].Trim().ToString();
                                            }

                                            var tempuser = userinfo.GetUserListByAnyCondition(uentity.OrganizeId, curdeptid, currolestr[1].Trim().ToString());
                                            foreach (UserInfoEntity userinfoEntity in tempuser)
                                            {
                                                if (!temppushaccount.Contains("," + userinfoEntity.Account + ","))
                                                {
                                                    temppushaccount += userinfoEntity.Account + ",";
                                                    temppushusername += userinfoEntity.RealName + ",";
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(temppushaccount) && temppushaccount != ",")
                            {
                                temppushaccount = temppushaccount.Substring(1, temppushaccount.Length - 2).ToString();
                                temppushusername = temppushusername.Substring(0, temppushusername.Length - 1).ToString();
                                //极光消息推送
                                JPushApi.PushMessage(temppushaccount, temppushusername, temppushcode, keyvalue);
                            }
                        }
                    }
                    #endregion
                }
                if (processName == "违章管理流程")
                {
                    #region 获取极光推送代码
                    switch (control.startflow)
                    {
                        //当前流程结点
                        case "违章登记":
                            //推送到下一流程的结点
                            if (nextActivityName == "违章核准")
                            {
                                pushcode = "WZ003";//待核准
                            }
                            else if (nextActivityName == "违章整改")
                            {
                                isPdImportant = true;

                                pushcode = "WZ002";//待整改
                            }
                            else if (nextActivityName == "违章完善")
                            {
                                pushcode = "WZ001";//待完善
                            }
                            else if (nextActivityName == "制定整改计划")
                            {
                                pushcode = "WZ011";//待制定整改计划
                            }
                            break;
                        //当前流程结点
                        case "违章举报":
                            //推送到下一流程的结点
                            if (nextActivityName == "违章审核")
                            {
                                pushcode = "WZ003";//待核准
                            }
                            else if (nextActivityName == "违章整改")
                            {
                                isPdImportant = true;

                                pushcode = "WZ002";//待整改
                            }
                            else if (nextActivityName == "违章完善")
                            {
                                pushcode = "WZ001";//待完善
                            }
                            else if (nextActivityName == "制定整改计划")
                            {
                                pushcode = "WZ011";//待制定整改计划
                            }
                            break;
                        //当前流程结点
                        case "违章完善":
                            if (nextActivityName == "违章整改")
                            {
                                isPdImportant = true;

                                pushcode = "WZ002";//待整改
                            }
                            else if (nextActivityName == "违章核准")
                            {
                                pushcode = "WZ003";//待核准
                            }
                            break;
                        //当前流程结点
                        case "违章核准":
                            //推送到下一流程的结点
                            if (nextActivityName == "违章核准")
                            {
                                pushcode = "WZ003";//待核准
                            }
                            else if (nextActivityName == "违章整改")
                            {
                                isPdImportant = true;

                                pushcode = "WZ002";//待整改
                            }
                            else if (nextActivityName == "违章登记" || nextActivityName == "违章完善")
                            {
                                pushcode = "WZ004";//核准未通过
                            }
                            else if (nextActivityName == "制定整改计划")
                            {
                                pushcode = "WZ011";//待制定整改计划
                            }
                            break;
                        //当前流程结点
                        case "违章审核":
                            //推送到下一流程的结点
                            if (nextActivityName == "违章审核")
                            {
                                pushcode = "WZ003";//待核准
                            }
                            else if (nextActivityName == "违章整改")
                            {
                                isPdImportant = true;

                                pushcode = "WZ002";//待整改
                            }
                            else if (nextActivityName == "违章登记" || nextActivityName == "违章举报" || nextActivityName == "违章完善")
                            {
                                pushcode = "WZ004";//核准未通过
                            }
                            else if (nextActivityName == "制定整改计划")
                            {
                                pushcode = "WZ011";//待制定整改计划
                            }
                            break;
                        //当前流程结点
                        case "违章整改":
                            //推送到下一流程的结点
                            if (nextActivityName == "违章验收")
                            {
                                pushcode = "WZ006";//待验收
                            }
                            else if (nextActivityName == "违章登记" || nextActivityName == "违章举报" || nextActivityName == "违章核准" || nextActivityName == "违章审核" || nextActivityName == "违章完善" || nextActivityName == "制定整改计划")
                            {
                                pushcode = "WZ009";//未整改退回
                            }
                            break;
                        //当前流程结点
                        case "违章验收":
                            //推送到下一流程的结点
                            if (nextActivityName == "验收确认")
                            {
                                pushcode = "WZ008";//待验收确认
                            }
                            else if (nextActivityName == "违章整改")
                            {
                                pushcode = "WZ005";//验收未通过
                            }
                            break;
                        case "验收确认":
                            //推送到下一流程的结点
                            if (nextActivityName == "违章整改")
                            {
                                pushcode = "WZ007";//验收确认未通过
                            }
                            break;
                        case "制定整改计划":
                            //推送到下一流程的结点
                            if (nextActivityName == "违章整改")
                            {
                                isPdImportant = true;

                                pushcode = "WZ002";//待整改
                            }
                            break;
                        case "整改延期申请":
                            if (nextActivityName == "整改延期审批")
                            {
                                pushcode = "WZ014";//整改延期待审批	
                            }
                            break;
                        case "整改延期审批":
                            if (nextActivityName == "整改延期审批")
                            {
                                pushcode = "WZ014";//整改延期待审批	
                            }
                            else if (nextActivityName == "整改延期退回")
                            {
                                pushcode = "WZ015";//整改延期审批未通过
                            }
                            break;
                    }
                    #endregion

                    #region 获取接收人
                    if (!string.IsNullOrEmpty(result.username))
                    {
                        pushusernames = result.username;
                    }
                    if (!string.IsNullOrEmpty(result.actionperson))
                    {
                        pushaccount = result.actionperson;

                        //如果当前用户姓名为空,则代入账号查询
                        if (string.IsNullOrEmpty(pushusernames))
                        {
                            string[] accounts = pushaccount.Split(',');

                            DataTable dt = userbll.GetUserTable(accounts);

                            foreach (DataRow row in dt.Rows)
                            {
                                pushusernames += row["REALNAME"].ToString() + ",";
                            }
                            if (!string.IsNullOrEmpty(pushusernames))
                            {
                                pushusernames = pushusernames.Substring(0, pushusernames.Length - 1);  //接收人姓名
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(control.businessid))
                    {
                        keyvalue = control.businessid;
                    }
                    #endregion

                    #region 判定重大违章提醒
                    if (isPdImportant)
                    {
                        string temppushcode = "WZ010";//重大违章提醒

                        string temppushaccount = ",";//推送账户

                        string temppushusername = string.Empty; //推送人

                        var llegalDt = lllegalservice.GetLllegalModel(keyvalue);

                        if (llegalDt.Rows.Count == 1)
                        {
                            string lllegallevelname = llegalDt.Rows[0]["lllegallevelname"].ToString();

                            if (lllegallevelname.Contains("严重"))
                            {
                                if (!string.IsNullOrEmpty(pushaccount))
                                {
                                    /*
                                      提醒 违章整改责任单位负责人、专工、安全管理人员/安全主管部门专工、安全管理人员、负责人
                                    */
                                    //整改人相关信息
                                    UserInfoEntity uentity = userinfo.GetUserInfoByAccount(pushaccount);

                                    string rolestr = dataitemdetailbll.GetItemValue("重大违章提醒角色");

                                    if (!string.IsNullOrEmpty(rolestr))
                                    {
                                        string[] rolearray = rolestr.Split(';');

                                        foreach (string str in rolearray)
                                        {
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                string[] currolestr = str.Split('|');
                                                string curdeptid = string.Empty;
                                                if (currolestr[0] == "ChangeDept")
                                                {
                                                    curdeptid = uentity.DepartmentCode;
                                                }
                                                else
                                                {
                                                    curdeptid = currolestr[0].Trim().ToString();
                                                }

                                                var tempuser = userinfo.GetUserListByAnyCondition(uentity.OrganizeId, curdeptid, currolestr[1].Trim().ToString());
                                                foreach (UserInfoEntity userinfoEntity in tempuser)
                                                {
                                                    if (!temppushaccount.Contains("," + userinfoEntity.Account + ","))
                                                    {
                                                        temppushaccount += userinfoEntity.Account + ",";
                                                        temppushusername += userinfoEntity.RealName + ",";
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(temppushaccount) && temppushaccount != ",")
                                {
                                    temppushaccount = temppushaccount.Substring(1, temppushaccount.Length - 2).ToString();
                                    temppushusername = temppushusername.Substring(0, temppushusername.Length - 1).ToString();
                                    //极光消息推送
                                    JPushApi.PushMessage(temppushaccount, temppushusername, temppushcode, keyvalue);
                                }
                            }
                        }
                    }
                    #endregion
                }

                //极光消息推送
                JPushApi.PushMessage(pushaccount, pushusernames, pushcode, keyvalue);
            }

            return isSucessful;
        }
        #endregion

        #region 推送违章消息
        /// <summary>
        /// 推送违章消息
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <param name="curflow"></param>
        /// <param name="nextflow"></param>
        /// <param name="pushaccount"></param>
        /// <returns></returns>
        public bool PushMessageForLllegal(string keyvalue, string curflow, string nextflow, string pushaccount)
        {
            bool isSucessful = true;

            string pushcode = string.Empty; //推送代码

            string pushusernames = string.Empty; //推送到下一流程的人员

            bool isPdImportant = false; //是否判定重大违章

            try
            {
                #region 获取极光推送代码
                switch (curflow)
                {
                    //当前流程结点
                    case "违章登记":
                        //推送到下一流程的结点
                        if (nextflow == "违章核准")
                        {
                            pushcode = "WZ003";//待核准
                        }
                        else if (nextflow == "违章整改")
                        {
                            isPdImportant = true;

                            pushcode = "WZ002";//待整改
                        }
                        else if (nextflow == "违章完善")
                        {
                            pushcode = "WZ001";//待完善
                        }
                        break;
                    case "违章举报":
                        //推送到下一流程的结点
                        if (nextflow == "违章审核")
                        {
                            pushcode = "WZ003";//待核准
                        }
                        else if (nextflow == "违章整改")
                        {
                            isPdImportant = true;

                            pushcode = "WZ002";//待整改
                        }
                        else if (nextflow == "违章完善")
                        {
                            pushcode = "WZ001";//待完善
                        }
                        break;
                    //当前流程结点
                    case "违章完善":
                        if (nextflow == "违章整改")
                        {
                            isPdImportant = true;

                            pushcode = "WZ002";//待整改
                        }
                        else if (nextflow == "违章核准")
                        {
                            pushcode = "WZ003";//待核准
                        }
                        break;
                    //当前流程结点
                    case "违章核准":
                        //推送到下一流程的结点
                        if (nextflow == "违章核准")
                        {
                            pushcode = "WZ003";//待核准
                        }
                        else if (nextflow == "违章整改")
                        {
                            isPdImportant = true;
                            pushcode = "WZ002";//待整改
                        }
                        else if (nextflow == "违章登记" || nextflow == "违章完善")
                        {
                            pushcode = "WZ004";//核准未通过
                        }
                        break;
                    //当前流程结点
                    case "违章审核":
                        //推送到下一流程的结点
                        if (nextflow == "违章审核")
                        {
                            pushcode = "WZ003";//待核准
                        }
                        else if (nextflow == "违章整改")
                        {
                            isPdImportant = true;
                            pushcode = "WZ002";//待整改
                        }
                        else if (nextflow == "违章举报" || nextflow == "违章完善")
                        {
                            pushcode = "WZ004";//核准未通过
                        }
                        break;
                    //当前流程结点
                    case "违章整改":
                        //推送到下一流程的结点
                        if (nextflow == "违章验收")
                        {
                            pushcode = "WZ006";//待验收
                        }
                        else if (nextflow == "违章登记" || nextflow == "违章举报" || nextflow == "违章核准" || nextflow == "违章审核" || nextflow == "违章完善")
                        {
                            pushcode = "WZ009";//未整改退回
                        }
                        break;
                    //当前流程结点
                    case "违章验收":
                        //推送到下一流程的结点
                        if (nextflow == "验收确认")
                        {
                            pushcode = "WZ008";//待验收确认
                        }
                        else if (nextflow == "违章整改")
                        {
                            pushcode = "WZ005";//验收未通过
                        }
                        break;
                    case "验收确认":
                        //推送到下一流程的结点
                        if (nextflow == "违章整改")
                        {
                            pushcode = "WZ007";//验收确认未通过
                        }
                        break;
                }
                #endregion

                #region 获取接收人
                if (!string.IsNullOrEmpty(pushaccount))
                {
                    string[] accounts = pushaccount.Split(',');

                    DataTable dt = userbll.GetUserTable(accounts);

                    foreach (DataRow row in dt.Rows)
                    {
                        pushusernames += row["REALNAME"].ToString() + ",";
                    }
                    if (!string.IsNullOrEmpty(pushusernames))
                    {
                        pushusernames = pushusernames.Substring(0, pushusernames.Length - 1);  //接收人姓名
                    }
                }
                #endregion

                //判定重大违章提醒
                if (isPdImportant)
                {
                    string temppushcode = "WZ010";//重大违章提醒

                    string temppushaccount = ",";//推送账户

                    string temppushusername = string.Empty; //推送人

                    var llegalDt = lllegalservice.GetLllegalModel(keyvalue);

                    if (llegalDt.Rows.Count == 1)
                    {
                        string lllegallevelname = llegalDt.Rows[0]["lllegallevelname"].ToString();

                        if (lllegallevelname.Contains("严重"))
                        {
                            if (!string.IsNullOrEmpty(pushaccount))
                            {
                                /*
                                  提醒 违章整改责任单位负责人、专工、安全管理人员/安全主管部门专工、安全管理人员、负责人
                                */
                                //整改人相关信息
                                UserInfoEntity uentity = userinfo.GetUserInfoByAccount(pushaccount);

                                string rolestr = dataitemdetailbll.GetItemValue("重大违章提醒角色");

                                if (!string.IsNullOrEmpty(rolestr))
                                {
                                    string[] rolearray = rolestr.Split(';');

                                    foreach (string str in rolearray)
                                    {
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            string[] currolestr = str.Split('|');
                                            string curdeptid = string.Empty;
                                            if (currolestr[0] == "ChangeDept")
                                            {
                                                curdeptid = uentity.DepartmentCode;
                                            }
                                            else
                                            {
                                                curdeptid = currolestr[0].Trim().ToString();
                                            }

                                            var tempuser = userinfo.GetUserListByAnyCondition(uentity.OrganizeId, curdeptid, currolestr[1].Trim().ToString());
                                            foreach (UserInfoEntity userinfoEntity in tempuser)
                                            {
                                                if (!temppushaccount.Contains("," + userinfoEntity.Account + ","))
                                                {
                                                    temppushaccount += userinfoEntity.Account + ",";
                                                    temppushusername += userinfoEntity.RealName + ",";
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(temppushaccount) && temppushaccount != ",")
                            {
                                temppushaccount = temppushaccount.Substring(1, temppushaccount.Length - 2).ToString();
                                temppushusername = temppushusername.Substring(0, temppushusername.Length - 1).ToString();
                                //极光消息推送
                                JPushApi.PushMessage(temppushaccount, temppushusername, temppushcode, keyvalue);
                            }
                        }
                    }
                }

                //极光消息推送
                JPushApi.PushMessage(pushaccount, pushusernames, pushcode, keyvalue);

            }
            catch (Exception)
            {
                isSucessful = false;
            }

            return isSucessful;
        }
        #endregion

        /// <summary>
        /// 获取当前的流程节点
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public string QueryTagNameByCurrentWF(string objectID)
        {
            try
            {
                return service.QueryTagByCurrentWF(objectID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取当前的流程节点标记
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public string QueryTagByCurrentWF(string objectID)
        {
            try
            {
                return service.QueryTagByCurrentWF(objectID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 通过当前实例ID，查询数据库，判断当前流程是否存在
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public bool IsHavaWFCurrentObject(string objectID)
        {
            try
            {
                return service.IsHavaWFCurrentObject(objectID);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 更新业务状态(隐患专用)
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool UpdateWorkStreamByObjectId(string objectId)
        {
            Operator user = OperatorProvider.Provider.Current();

            string tagName = QueryTagNameByCurrentWF(objectId);

            try
            {
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                string bzApiUrl = string.Empty;
                if (Debugger.IsAttached)
                {
                    bzApiUrl = "http://10.36.1.200/bzapp/api";
                }
                else
                {
                    bzApiUrl = itemBll.GetItemValue("bzApiUrl");
                }


                bool ispost = false;

                int reqtype = 0;

                if (!string.IsNullOrEmpty(bzApiUrl))
                {
                    var dt = new HTBaseInfoBLL().GetHiddenByKeyValue(objectId);

                    if (dt.Rows.Count == 1)
                    {
                        string createuserid = dt.Rows[0]["createuserid"].ToString();
                        //登记一般隐患（已整改）
                        if (tagName == "整改结束" && dt.Rows[0]["reformtype"].ToString() == "1" && dt.Rows[0]["rankname"].ToString().Contains("一般隐患"))
                        {
                            ispost = true;
                            reqtype = 2;
                        }
                        //登记一般隐患（未整改）
                        else if (tagName == "隐患整改" && dt.Rows[0]["rankname"].ToString().Contains("一般隐患"))
                        {
                            ispost = true;
                            reqtype = 1;
                        }
                        //登记重大隐患（未整改）
                        else if (tagName == "隐患整改" && dt.Rows[0]["rankname"].ToString().Contains("重大隐患"))
                        {
                            ispost = true;
                            reqtype = 3;
                        }

                        if (ispost)
                        {
                            var result = new
                            {
                                userId = createuserid,
                                data = reqtype
                            };
                            try
                            {
                                string requestUrl = bzApiUrl + "/SafetyScore/AddScore";
                                WebClient wc = new WebClient();
                                wc.Credentials = CredentialCache.DefaultCredentials;
                                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(result));
                                wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                                wc.UploadValuesAsync(new Uri(requestUrl), nc);

                                // string res = HttpMethods.HttpPost(Path.Combine(bzApiUrl, "SafetySocre", "AddScore"), "json=" + JsonConvert.SerializeObject(result));
                            }
                            catch (Exception ex)
                            {
                                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                                int _actionType = 5;
                                LogEntity logEntity = new LogEntity();
                                logEntity.CategoryId = _actionType;
                                logEntity.OperateTypeId = _actionType.ToString();
                                logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                                logEntity.OperateAccount = user.UserName;
                                logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                                logEntity.ExecuteResult = 1;
                                logEntity.Module = SystemInfo.CurrentModuleName;
                                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                                logEntity.ExecuteResultJson = "操作信息:推送班组端积分接口失败, 错误信息:" + ex.Message + " , 异常信息:" + ex.InnerException + " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson();
                                logEntity.WriteLog();

                                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyyMMddHHmmss-") + "推送班组端积分接口错误信息:" + ex.Message + " , 异常信息:" + ex.InnerException +
                                    " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson() + ";\r\n");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return service.UpdateWorkStreamByObjectId("bis_htbaseinfo", "workstream", objectId);

        }

        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            Operator user = OperatorProvider.Provider.Current();

            string fileName = "推送班组积分_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            //将同步结果写入日志文件
            try
            {
                int _actionType = 4;
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = _actionType;
                logEntity.OperateTypeId = _actionType.ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                logEntity.OperateAccount = user.UserName;
                logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                logEntity.ExecuteResult = 1;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.ExecuteResultJson = "操作信息:推送班组端积分接口, 返回结果:" + System.Text.Encoding.UTF8.GetString(e.Result) + ",Json信息:" + e.ToJson();
                logEntity.WriteLog();

                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "推送班组端积分接口结果信息:" + e.ToJson() + "\r\n");
            }
            catch (Exception ex)
            {
                int _actionType = 5;
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = _actionType;
                logEntity.OperateTypeId = _actionType.ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                logEntity.OperateAccount = user.UserName;
                logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                logEntity.ExecuteResult = 1;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.ExecuteResultJson = "操作信息:推送班组端积分接口失败, 错误信息:" + ex.Message + " , 异常信息:" + ex.InnerException +
                    " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson();
                logEntity.WriteLog();

                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyyMMddHHmmss-") + "推送班组端积分接口错误信息:" + ex.Message + " , 异常信息:" + ex.InnerException +
                    " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson() + ";\r\n");
            }
        }

        /// <summary>
        /// 常用操作枚举
        /// </summary>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public OperationType GetOperationType(string operationType)
        {
            OperationType opera = new OperationType();
            switch (operationType)
            {
                case "0":
                    opera = OperationType.Other;
                    break;
                case "Other":
                    opera = OperationType.Other;
                    break;

                case "1":
                    opera = OperationType.Login;
                    break;
                case "Login":
                    opera = OperationType.Login;
                    break;

                case "2":
                    opera = OperationType.Exit;
                    break;
                case "Exit":
                    opera = OperationType.Exit;
                    break;

                case "3":
                    opera = OperationType.Visit;
                    break;
                case "Visit":
                    opera = OperationType.Visit;
                    break;

                case "4":
                    opera = OperationType.Leave;
                    break;
                case "Leave":
                    opera = OperationType.Leave;
                    break;

                case "5":
                    opera = OperationType.Create;
                    break;
                case "Create":
                    opera = OperationType.Create;
                    break;

                case "6":
                    opera = OperationType.Delete;
                    break;
                case "Delete":
                    opera = OperationType.Delete;
                    break;

                case "7":
                    opera = OperationType.Update;
                    break;
                case "Update":
                    opera = OperationType.Update;
                    break;

                case "8":
                    opera = OperationType.Submit;
                    break;
                case "Submit":
                    opera = OperationType.Submit;
                    break;

                case "9":
                    opera = OperationType.Exception;
                    break;
                case "Exception":
                    opera = OperationType.Exception;
                    break;

                case "10":
                    opera = OperationType.AppLogin;
                    break;
                case "AppLogin":
                    opera = OperationType.AppLogin;
                    break;
            }

            return opera;

        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetActionList(string keyValue)
        {
            return service.GetActionList(keyValue);
        }

        #region 获取违章的流程图对象
        /// <summary>
        /// 获取违章的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetLllegalActionList(string keyValue)
        {
            return service.GetLllegalActionList(keyValue);
        }
        #endregion

        #region 获取问题的流程图对象
        /// <summary>
        /// 获取问题的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetQuestionActionList(string keyValue)
        {
            return service.GetQuestionActionList(keyValue);
        }
        #endregion

        #region 获取标准修编的流程图对象
        /// <summary>
        /// 获取标准修编的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetStandardApplyActionList(string keyValue)
        {
            return service.GetStandardApplyActionList(keyValue);
        }
        #endregion
        #region 获取工作计划的流程图对象
        /// <summary>
        /// 获取部门工作计划的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetDepartPlanApplyActionList(string keyValue)
        {
            return service.GetDepartPlanApplyActionList(keyValue);
        }
        /// <summary>
        /// 获取个人工作计划的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetPersonPlanApplyActionList(string keyValue)
        {
            return service.GetPersonPlanApplyActionList(keyValue);
        }
        #endregion

        #region 获取流程导向图
        /// <summary>
        /// 获取流程导向图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable QueryWorkFlowMapForApp(string keyValue, string mode)
        {
            try
            {
                return service.QueryWorkFlowMapForApp(keyValue, mode);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 获取公共的流程图对象
        /// <summary>
        /// 获取隐患的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetCommonFlow(Flow flow, string keyValue)
        {
            try
            {
                return service.GetCommonFlow(flow, keyValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
