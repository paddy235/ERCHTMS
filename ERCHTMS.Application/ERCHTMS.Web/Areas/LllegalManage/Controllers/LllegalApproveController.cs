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
using System;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Busines.LllegalManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Dynamic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章评估信息表
    /// </summary>
    public class LllegalApproveController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private DepartmentBLL departmentbll = new DepartmentBLL(); //部门操作对象
        private UserBLL userbll = new UserBLL(); //人员操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalAwardDetailBLL lllegalawarddetailbll = new LllegalAwardDetailBLL(); //违章奖励信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //核准信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); //考核信息对象

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
            string km_major_role = dataitemdetailbll.GetItemValue("KM_MAJOR_ROLE"); //可门配置 
            //可门
            if (!string.IsNullOrEmpty(km_major_role))
            {
                string actionName = string.Empty;
                string[] allKeys = Request.QueryString.AllKeys;
                if (allKeys.Count() > 0)
                {
                    actionName = "KmForm?";
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
                    actionName = "KmForm";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult KmForm()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        #endregion

        #region 保存提交核准内容
        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalApproveEntity pEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {

            /*
             注：核准分两步 
             * 
             *  确定为装置类情况下，如果当前核准人是安全管理部门人员, 先判断是否为装置类，如果不是 则直接到整改或退回，如果是，则转发至装置部门 或退回(不是提交)
             *  如果是非安全管理人员,则提交到安全管理部门人员会退回到登记.
             */

            string errorMsg = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //流程标识

            string participant = string.Empty;  //获取流程下一节点的参与人员

            var lllegatypename = "";
            if (!string.IsNullOrWhiteSpace(entity.LLLEGALTYPE))
            {
                lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;
            }

            //保存违章基本信息
            LllegalRegisterEntity baseentity = lllegalregisterbll.GetEntity(keyValue);
            entity.AUTOID = baseentity.AUTOID;
            entity.CREATEDATE = baseentity.CREATEDATE;
            entity.CREATEUSERDEPTCODE = baseentity.CREATEUSERDEPTCODE;
            entity.CREATEUSERID = baseentity.CREATEUSERID;
            entity.CREATEUSERNAME = baseentity.CREATEUSERNAME;
            entity.CREATEUSERORGCODE = baseentity.CREATEUSERORGCODE;
            entity.MODIFYDATE = DateTime.Now;
            entity.MODIFYUSERID = curUser.UserId;
            entity.MODIFYUSERNAME = curUser.UserName;
            entity.RESEVERFOUR = "";
            entity.RESEVERFIVE = "";
            lllegalregisterbll.SaveForm(keyValue, entity);

            #region 考核信息
           
            string RELEVANCEDATA = Request.Form["RELEVANCEDATA"];
            if (!string.IsNullOrEmpty(RELEVANCEDATA))
            {
                //先删除考核信息集合
                lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");

                JArray jarray = (JArray)JsonConvert.DeserializeObject(RELEVANCEDATA);

                foreach (JObject rhInfo in jarray)
                {
                    //string relevanceId = rhInfo["ID"].ToString(); //主键id
                    string assessobject = rhInfo["ASSESSOBJECT"].ToString();
                    string personinchargename = rhInfo["PERSONINCHARGENAME"].ToString(); //关联责任人姓名
                    string personinchargeid = rhInfo["PERSONINCHARGEID"].ToString();//关联责任人id
                    string performancepoint = rhInfo["PERFORMANCEPOINT"].ToString();//EHS绩效考核 
                    string economicspunish = rhInfo["ECONOMICSPUNISH"].ToString(); // 经济处罚
                    string education = rhInfo["EDUCATION"].ToString(); //教育培训
                    string lllegalpoint = rhInfo["LLLEGALPOINT"].ToString();//违章扣分
                    string awaitjob = rhInfo["AWAITJOB"].ToString();//待岗
                    LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                    newpunishEntity.LLLEGALID = entity.ID;
                    newpunishEntity.ASSESSOBJECT = assessobject; //考核对象
                    newpunishEntity.PERSONINCHARGEID = personinchargeid;
                    newpunishEntity.PERSONINCHARGENAME = personinchargename;
                    newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                    newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                    newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                    newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                    newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                    newpunishEntity.MARK = assessobject.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                    lllegalpunishbll.SaveForm("", newpunishEntity);
                }
            }
            #endregion

            #region 违章奖励信息
            string AWARDDATA = Request.Form["AWARDDATA"];
            if (!string.IsNullOrEmpty(AWARDDATA))
            {  //先删除关联集合
                lllegalawarddetailbll.DeleteLllegalAwardList(entity.ID);

                JArray jarray = (JArray)JsonConvert.DeserializeObject(AWARDDATA);

                foreach (JObject rhInfo in jarray)
                {
                    string userid = rhInfo["USERID"].ToString(); //奖励用户
                    string username = rhInfo["USERNAME"].ToString();
                    string deptid = rhInfo["DEPTID"].ToString();//奖励用户部门
                    string deptname = rhInfo["DEPTNAME"].ToString();
                    string points = rhInfo["POINTS"].ToString();  //奖励积分
                    string money = rhInfo["MONEY"].ToString(); //奖励金额

                    LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                    awardEntity.LLLEGALID = entity.ID;
                    awardEntity.USERID = userid; //奖励对象
                    awardEntity.USERNAME = username;
                    awardEntity.DEPTID = deptid;
                    awardEntity.DEPTNAME = deptname;
                    awardEntity.POINTS = !string.IsNullOrEmpty(points) ? int.Parse(points) : 0;
                    awardEntity.MONEY = !string.IsNullOrEmpty(money) ? Convert.ToDecimal(money) : 0;
                    lllegalawarddetailbll.SaveForm("", awardEntity);
                }
            }
            #endregion

            //违章整改记录
            LllegalReformEntity cEntity = lllegalreformbll.GetEntityByBid(keyValue);
            cEntity.REFORMDEADLINE = rEntity.REFORMDEADLINE;
            cEntity.REFORMPEOPLE = rEntity.REFORMPEOPLE;
            cEntity.REFORMPEOPLEID = rEntity.REFORMPEOPLEID;
            cEntity.REFORMDEPTCODE = rEntity.REFORMDEPTCODE;
            cEntity.REFORMDEPTNAME = rEntity.REFORMDEPTNAME;
            cEntity.REFORMTEL = rEntity.REFORMTEL;
            cEntity.REFORMSTATUS = string.Empty;
            cEntity.REFORMCHARGEDEPTID = rEntity.REFORMCHARGEDEPTID;
            cEntity.REFORMCHARGEDEPTNAME = rEntity.REFORMCHARGEDEPTNAME;
            cEntity.REFORMCHARGEPERSON = rEntity.REFORMCHARGEPERSON;
            cEntity.REFORMCHARGEPERSONNAME = rEntity.REFORMCHARGEPERSONNAME;
            cEntity.ISAPPOINT = rEntity.ISAPPOINT;
            lllegalreformbll.SaveForm(cEntity.ID, cEntity);

            //违章验收
            LllegalAcceptEntity aptEntity = lllegalacceptbll.GetEntityByBid(keyValue);
            aptEntity.ACCEPTPEOPLE = aEntity.ACCEPTPEOPLE;
            aptEntity.ACCEPTPEOPLEID = aEntity.ACCEPTPEOPLEID;
            aptEntity.ACCEPTDEPTCODE = aEntity.ACCEPTDEPTCODE;
            aptEntity.ACCEPTDEPTNAME = aEntity.ACCEPTDEPTNAME;
            aptEntity.ACCEPTTIME = aEntity.ACCEPTTIME;
            lllegalacceptbll.SaveForm(aptEntity.ID, aptEntity);


            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
            wfentity.argument2 = entity.LLLEGALTYPE;//违章类型
            wfentity.argument3 = curUser.DeptId;//当前人所属部门
            wfentity.argument4 = entity.LLLEGALTEAMCODE;//违章部门
            wfentity.argument5 = entity.LLLEGALLEVEL; //违章级别
            wfentity.startflow = baseentity.FLOWSTATE;
            //上报，且存在上级部门
            if (entity.ISUPSAFETY == "1")
            {
                wfentity.submittype = "上报";
            }
            else  //不上报，评估通过需要提交整改，评估不通过退回到登记
            {
                /****判断当前人是否评估通过*****/
                #region 判断当前人是否评估通过
                //评估通过，则直接进行整改
                if (pEntity.APPROVERESULT == "1")
                {
                    wfentity.submittype = "提交";
                    //不指定整改责任人
                    if (rEntity.ISAPPOINT == "0")
                    {
                        wfentity.submittype = "制定提交";
                    }
                    //判断是否是同级提交
                    bool ismajorpush = GetCurUserWfAuth(null, baseentity.FLOWSTATE, baseentity.FLOWSTATE, "厂级违章流程", "同级提交", entity.MAJORCLASSIFY, null, null, keyValue);
                    if (ismajorpush)
                    {
                        wfentity.submittype = "同级提交";
                    }
                }
                else  //评估不通过，退回到登记 
                {
                    wfentity.submittype = "退回";
                }
                #endregion
            }
            wfentity.rankid = null;
            wfentity.user = curUser;
            wfentity.mark = "厂级违章流程";
            wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
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
                            //保存违章核准
                            pEntity.LLLEGALID = keyValue;
                            lllegalapprovebll.SaveForm("", pEntity);

                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态

                            return Success(result.message);
                        }
                        else
                        {
                            return Error("当前用户无核准权限!");
                        }
                    }
                    else  //不更改状态的情况下
                    {
                        //保存违章核准
                        pEntity.LLLEGALID = keyValue;
                        lllegalapprovebll.SaveForm("", pEntity);

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
       

        }
        #endregion

        #region 获取核准历史记录
        /// <summary>
        /// 获取核准历史记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = lllegalapprovebll.GetHistoryList(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 获取核准详细信息
        /// <summary>
        /// 获取核准详细信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntityJson(string keyValue)
        {
            //获取对应的核准信息
            var approveData = lllegalapprovebll.GetEntity(keyValue);
            var punishData = lllegalpunishbll.GetEntityByApproveId(keyValue);
            var josnData = new
            {
                approveData = approveData, //核准详情信息
                punishData = punishData  //考核内容信息 
            };
            return Content(josnData.ToJson());
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
