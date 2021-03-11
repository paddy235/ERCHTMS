using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：隐患评估信息表
    /// </summary>
    public class HTApprovalController : MvcControllerBase
    {
        private HTApprovalBLL htapprovebll = new HTApprovalBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //隐患流程
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //隐患整改
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收
        private DepartmentBLL departmentbll = new DepartmentBLL(); //部门操作对象
        private UserBLL userbll = new UserBLL(); //人员操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string actionName = string.Empty;

            string GDXJ_HYC_ORGCODE = dataitemdetailbll.GetItemValue("GDXJ_HYC_ORGCODE");

            //国电新疆红雁池专用
            if (curUser.OrganizeCode == GDXJ_HYC_ORGCODE)
            {
                string[] allKeys = Request.QueryString.AllKeys;
                if (allKeys.Count() > 0)
                {
                    actionName = "HYCForm?";
                    int num = 0;
                    foreach (string str in allKeys)
                    {
                        string strValue = Request.QueryString[str];
                        if (num == 0)
                        {
                            actionName += str + "=" + strValue;
                        }
                        else
                        {
                            actionName += "&" + str + "=" + strValue;
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "HYCForm";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HYCForm()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string hideCode)
        {
            var data = htapprovebll.GetList(hideCode);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htapprovebll.GetHistoryList(keyCode);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = htapprovebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除隐患评估信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htapprovebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        #endregion


        #region 保存提交流程(厂级)
        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string isUpSubmit, HTBaseInfoEntity bentity, HTApprovalEntity entity, HTChangeInfoEntity chEntity, HTAcceptInfoEntity aEntity)
        {

            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //流程标识

            #region 保存基本信息

            //评估ID
            string APPROVALID = Request.Form["APPROVALID"] != null ? Request.Form["APPROVALID"].ToString() : "";

            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : "";

            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : "";

            APPROVALID = APPROVALID == "&nbsp;" ? "" : APPROVALID;

            //隐患曝光
            string EXPOSURESTATE = Request.Form["EXPOSURESTATE"] != null ? Request.Form["EXPOSURESTATE"].ToString() : "";
            //设备
            if (string.IsNullOrEmpty(bentity.DEVICEID))
            {
                bentity.DEVICEID = string.Empty;
            }
            if (string.IsNullOrEmpty(bentity.DEVICENAME))
            {
                bentity.DEVICENAME = string.Empty;
            }
            if (string.IsNullOrEmpty(bentity.DEVICECODE))
            {
                bentity.DEVICECODE = string.Empty;
            }
            //保存隐患基本信息
            htbaseinfobll.SaveForm(keyValue, bentity);

            //隐患整改
            if (!string.IsNullOrEmpty(CHANGEID))
            {
                var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                chEntity.AUTOID = tempEntity.AUTOID;
                chEntity.BACKREASON = "";  //回退原因
                chEntity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                chEntity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                chEntity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                chEntity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
            }
            htchangeinfobll.SaveForm(CHANGEID, chEntity);

            //隐患验收
            if (!string.IsNullOrEmpty(ACCEPTID))
            {
                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                aEntity.AUTOID = tempEntity.AUTOID;
            }
            htacceptinfobll.SaveForm(ACCEPTID, aEntity);
            #endregion

            string participant = string.Empty;  //获取流程下一节点的参与人员

            HTBaseInfoEntity baseEntity = htbaseinfobll.GetEntity(keyValue);

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            wfentity.argument1 = bentity.MAJORCLASSIFY; //专业分类
            wfentity.argument2 = curUser.DeptId; //当前部门
            wfentity.argument3 = bentity.HIDTYPE; //隐患类别
            wfentity.argument4 = bentity.HIDBMID; //所属部门
            string startflow = baseEntity.WORKSTREAM;
            wfentity.startflow = startflow;
            wfentity.rankid = baseEntity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "厂级隐患排查";
            wfentity.organizeid = baseEntity.HIDDEPART; //对应电厂id
            //返回结果
            WfControlResult result = new WfControlResult();

            if (isUpSubmit == "1")  //上报，且存在上级部门
            {
                #region 上报

                wfentity.submittype = "上报";
                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson; //目标流程参与者

                    if (!string.IsNullOrEmpty(participant))
                    {
                        //保存隐患评估信息
                        htapprovebll.SaveForm(APPROVALID, entity);

                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        return Success(result.message);
                    }
                    else
                    {
                        return Error("当前上级部门无评估人员,如需上报,请联系系统管理员进行配置!");
                    }

                }
                else
                {
                    return Error(result.message);
                }
                #endregion
            }
            else  //不上报，评估通过需要提交整改，评估不通过退回到登记
            {
                /****判断当前人是否评估通过*****/
                #region 判断当前人是否评估通过
                //评估通过，则直接进行整改
                if (entity.APPROVALRESULT == "1")
                {
                    wfentity.submittype = "提交";
                    //不指定整改责任人
                    if (chEntity.ISAPPOINT == "0")
                    {
                        wfentity.submittype = "制定提交";
                    }
                    //判断是否是同级提交
                    bool ismajorpush = GetCurUserWfAuth(baseEntity.HIDRANK, "隐患评估", "隐患评估", "厂级隐患排查", "同级提交", baseEntity.MAJORCLASSIFY, null, null, keyValue);
                    if (ismajorpush) 
                    {
                        wfentity.submittype = "同级提交";
                    }

                    #region 国电新疆版本
                    if (baseEntity.ADDTYPE == "3")
                    {
                        //非本部门整改
                        if (baseEntity.ISSELFCHANGE == "0")
                        {
                            wfentity.submittype = "制定提交";

                            //如果已经制定了整改计划,则按照提交来进行推送
                            if (baseEntity.ISFORMULATE == "1")
                            {
                                wfentity.submittype = "提交";
                            }
                            //如果当前评估部门是整改部门，则直接提交
                            if (curUser.DeptId == chEntity.CHANGEDUTYDEPARTID)
                            {
                                wfentity.submittype = "提交";
                            }
                            //如果当前评估部门是创建部门，则直接提交至非本部门整改的安监部
                            if (curUser.DeptCode == baseEntity.CREATEUSERDEPTCODE)
                            {
                                wfentity.submittype = "制定提交";
                            }
                        }
                        else  //本部门整改情况下， 公司级用户不管如何，都不会直接到生技部
                        {
                            UserEntity userEntity = userbll.GetEntity(baseEntity.CREATEUSERID);
                            if (userEntity.RoleName.Contains("公司级用户") && curUser.RoleName.Contains("公司级用户"))
                            {
                                wfentity.submittype = "制定提交";
                            }
                        }
                    } 
                    #endregion
                }
                else  //评估不通过，退回到登记 
                {
                    wfentity.submittype = "退回";

                    #region 国电新疆版本
                    if (baseEntity.ADDTYPE == "3")
                    {
                        //已经制定了整改计划，则按照制定计划退回
                        if (baseEntity.ISFORMULATE == "1")
                        {
                            wfentity.submittype = "制定退回";
                        }
                    } 
                    #endregion
                }
                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    if (!string.IsNullOrEmpty(participant))
                    {
                        //如果是更改状态
                        if (result.ischangestatus)
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);
                            if (count > 0)
                            {
                                //保存隐患评估信息
                                htapprovebll.SaveForm(APPROVALID, entity);
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                                return Success(result.message);
                            }
                            else
                            {
                                return Error("当前用户无评估权限!");
                            }
                        }
                        else  //不更改状态的情况下
                        {
                            //保存隐患评估信息
                            htapprovebll.SaveForm(APPROVALID, entity);

                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                            return Success(result.message);
                        }
                    }
                    else
                    {
                        return Error("目标流程参与者未定义!");
                    }

                }
                else
                {
                    return Error(result.message);
                }
                #endregion
            }

        }
        #endregion

        #region 省级登记的隐患提交及上报(评估)
        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCForm(string keyValue, string isUpSubmit, HTBaseInfoEntity bentity, HTApprovalEntity entity, HTChangeInfoEntity chEntity, HTAcceptInfoEntity aEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //流程标识

            IList<UserEntity> ulist = new List<UserEntity>();

            #region 保存基本信息

            //评估ID
            string APPROVALID = Request.Form["APPROVALID"] != null ? Request.Form["APPROVALID"].ToString() : "";

            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : "";

            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : "";

            APPROVALID = APPROVALID == "&nbsp;" ? "" : APPROVALID;

            //隐患曝光
            string EXPOSURESTATE = Request.Form["EXPOSURESTATE"] != null ? Request.Form["EXPOSURESTATE"].ToString() : "";
            //保存隐患基本信息
            htbaseinfobll.SaveForm(keyValue, bentity);

            //隐患整改
            if (!string.IsNullOrEmpty(CHANGEID))
            {
                var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                chEntity.AUTOID = tempEntity.AUTOID;
                chEntity.BACKREASON = "";  //回退原因
                chEntity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                chEntity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                chEntity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                chEntity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
            }
            htchangeinfobll.SaveForm(CHANGEID, chEntity);

            //隐患验收
            if (!string.IsNullOrEmpty(ACCEPTID))
            {
                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                aEntity.AUTOID = tempEntity.AUTOID;
            }
            htacceptinfobll.SaveForm(ACCEPTID, aEntity);
            #endregion

            string participant = string.Empty;  //获取流程下一节点的参与人员

            bool isgoback = false;

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            string startflow = htbaseinfobll.GetEntity(keyValue).WORKSTREAM;
            wfentity.startflow = startflow;
            wfentity.rankid = bentity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "省级隐患排查";
            wfentity.organizeid = bentity.HIDDEPART; //对应电厂id
            //返回结果
            WfControlResult result = new WfControlResult();

            if (isUpSubmit == "1")  //上报，且存在上级部门
            {
                wfentity.submittype = "上报";
            }
            else  //不上报，评估通过需要提交整改，评估不通过退回到登记
            {
                /****判断当前人是否评估通过*****/
                #region 判断当前人是否评估通过
                //评估通过，则直接进行整改
                if (entity.APPROVALRESULT == "1")
                {
                    wfentity.submittype = "提交";
                }
                else  //评估不通过，退回到登记 
                {
                    wfentity.submittype = "退回";
                    isgoback = true;
                }
                #endregion
            }

   
            //获取下一流程的操作人
            result = wfcontrolbll.GetWfControl(wfentity);

            //返回操作结果成功
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                //更改状态
                if (result.ischangestatus)
                {
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //保存隐患评估信息
                            htapprovebll.SaveForm(APPROVALID, entity);
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                        }
                        else
                        {
                            return Error("当前用户无评估权限!");
                        }
                    }
                }
                else //不更改状态
                {
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //保存隐患评估信息
                        htapprovebll.SaveForm(APPROVALID, entity);

                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                    }
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }

        }
        #endregion

        public bool GetCurUserWfAuth(string rankid, string workflow, string endflow, string mark, string submittype, string arg1 = "", string arg2 = "", string arg3 = "", string businessid = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = businessid; 
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankid = rankid;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;
            wfentity.argument1 = arg1;
            wfentity.argument2 = arg2;
            wfentity.argument3 = arg3;

            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

            return result.ishave;
        }
    }
}
