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
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //核准信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); //考核信息对象

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
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalApproveEntity pEntity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
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

            bool isSubmit = true; //是否要执行提交步骤,安全管理部门用于控制装置类违章转发至装置部门

            bool isAddScore = false; //是否添加到用户积分

            var lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;

            //不通过,退回到违章登记，不管是I级核准还是II级核准
            if (pEntity.APPROVERESULT == "0")
            {
                wfFlag = "2";
                string createuserid = lllegalregisterbll.GetEntity(keyValue).CREATEUSERID;
                UserEntity userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //登记用户
                errorMsg = "登记用户";
            }
            else  //核准通过
            {
                // 安全管理部门人员
                if (userbll.GetSafetyAndDeviceDept(curUser).Contains("1"))
                {
                    //当前人有且是装置部门，直接到整改
                    if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                    {
                        //取整改人
                        wfFlag = "1";  // II级核准=>整改
                        //如果非装置类 则提交到整改
                        UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象
                        //取整改人
                        participant = reformUser.Account;

                        errorMsg = "整改责任人";

                        isAddScore = true;
                    }
                    else 
                    {
                        //判断是否装置类违章
                        if (lllegatypename == dataitemdetailbll.GetItemValue("ApplianceClass"))
                        {
                            //更改核准人账号，变更为装置部门用户  此步步需要更改状态
                            isSubmit = false;
                            //取装置部门用户
                            participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                            errorMsg = "装置部门用户";
                        }
                        else
                        {
                            //如果是非装置类违章，通过则进行整改
                            //取整改人
                            wfFlag = "1";  // II级核准=>整改
                            //如果非装置类 则提交到整改
                            UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象
                            //取整改人
                            participant = reformUser.Account;

                            errorMsg = "整改责任人";

                            isAddScore = true;
                        }
                    }
      
                }
                //装置用户
                else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                {
                       wfFlag = "1";  // II级核准=>整改
                       //如果非装置类 则提交到整改
                       UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象
                       //取整改人
                       participant = reformUser.Account;

                       errorMsg = "整改责任人";

                       isAddScore = true;
                }
                else  //其他部门人员 
                {

                   //上报情况下
                    if (entity.ISUPSAFETY == "1")
                    {
                        wfFlag = "3";
                        //取安全管理部门 ，推送至II级核准
                        //取安全管理部门用户
                        participant = userbll.GetSafetyDeviceDeptUser("0", curUser);
                        errorMsg = "安全管理部门用户";
                    }
                    else //不上报情况下，提交到整改 
                    {
                        wfFlag = "1";  //I级核准=>整改
                        //如果非装置类 则提交到整改
                        UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象
                        //取整改人
                        participant = reformUser.Account;

                        errorMsg = "整改责任人";

                        isAddScore = true;

                    }
                }
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

            //保存核准基本信息 (不执行真正意义上的提交,则无法进行核准)
            if (isSubmit) 
            {
                pEntity.LLLEGALID = keyValue;
                lllegalapprovebll.SaveForm("", pEntity);

                //新增考核内容信息(特别针对核准过程)
                pbEntity.MARK = "1"; //表示考核记录下的
                pbEntity.LLLEGALID = keyValue;
                pbEntity.APPROVEID = pEntity.ID;
                pbEntity.CREATEDATE = DateTime.Now;
                pbEntity.CREATEUSERDEPTCODE = curUser.DeptCode;
                pbEntity.CREATEUSERID = curUser.UserId;
                pbEntity.CREATEUSERNAME = curUser.UserName;
                pbEntity.CREATEUSERORGCODE = curUser.OrganizeCode;
                pbEntity.MODIFYDATE = DateTime.Now;
                pbEntity.MODIFYUSERID = curUser.UserId;
                pbEntity.MODIFYUSERNAME = curUser.UserName;
                lllegalpunishbll.SaveForm("", pbEntity);
            }

            //同时对基础的考核内容进行更改相应的惩罚值
            LllegalPunishEntity punishEntity = lllegalpunishbll.GetEntityByBid(keyValue);
            if (null != punishEntity)
            {
                //punishEntity.APPROVEID = pEntity.ID;
                punishEntity.PERSONINCHARGEID = pbEntity.PERSONINCHARGEID;
                punishEntity.PERSONINCHARGENAME = pbEntity.PERSONINCHARGENAME;
                punishEntity.ECONOMICSPUNISH = pbEntity.ECONOMICSPUNISH;
                punishEntity.LLLEGALPOINT = pbEntity.LLLEGALPOINT;
                punishEntity.AWAITJOB = pbEntity.AWAITJOB;
                punishEntity.LLLEGAOTHER = pbEntity.LLLEGAOTHER;
                punishEntity.FIRSTINCHARGEID = pbEntity.FIRSTINCHARGEID;
                punishEntity.FIRSTINCHARGENAME = pbEntity.FIRSTINCHARGENAME;
                punishEntity.FIRSTECONOMICSPUNISH = pbEntity.FIRSTECONOMICSPUNISH;
                punishEntity.FIRSTLLLEGALPOINT = pbEntity.FIRSTLLLEGALPOINT;
                punishEntity.FIRSTAWAITJOB = pbEntity.FIRSTAWAITJOB;
                punishEntity.FIRSTOTHER = pbEntity.FIRSTOTHER;
                punishEntity.SECONDINCHARGEID = pbEntity.SECONDINCHARGEID;
                punishEntity.SECONDINCHARGENAME = pbEntity.SECONDINCHARGENAME;
                punishEntity.SECONDECONOMICSPUNISH = pbEntity.SECONDECONOMICSPUNISH;
                punishEntity.SECONDLLLEGALPOINT = pbEntity.SECONDLLLEGALPOINT;
                punishEntity.SECONDAWAITJOB = pbEntity.SECONDAWAITJOB;
                punishEntity.SECONDOTHER = pbEntity.SECONDOTHER;
                lllegalpunishbll.SaveForm(punishEntity.ID, punishEntity);
            }


            //  string ReformID = Request.Form["REFORMID"] != null ? Request.Form["REFORMID"].ToString() : ""; //整改ID
            //违章整改记录
            LllegalReformEntity cEntity = lllegalreformbll.GetEntityByBid(keyValue);
            cEntity.REFORMDEADLINE = rEntity.REFORMDEADLINE;
            cEntity.REFORMPEOPLE = rEntity.REFORMPEOPLE;
            cEntity.REFORMPEOPLEID = rEntity.REFORMPEOPLEID;
            cEntity.REFORMDEPTCODE = rEntity.REFORMDEPTCODE;
            cEntity.REFORMDEPTNAME = rEntity.REFORMDEPTNAME;
            cEntity.REFORMTEL = rEntity.REFORMTEL;
            lllegalreformbll.SaveForm(cEntity.ID, cEntity);


            // string AcceptID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; // 验收ID 
            //隐患验收
            LllegalAcceptEntity aptEntity = lllegalacceptbll.GetEntityByBid(keyValue);
            aptEntity.ACCEPTPEOPLE = aEntity.ACCEPTPEOPLE;
            aptEntity.ACCEPTPEOPLEID = aEntity.ACCEPTPEOPLEID;
            aptEntity.ACCEPTDEPTCODE = aEntity.ACCEPTDEPTCODE;
            aptEntity.ACCEPTDEPTNAME = aEntity.ACCEPTDEPTNAME;
            aptEntity.ACCEPTTIME = aEntity.ACCEPTTIME;
            lllegalacceptbll.SaveForm(aptEntity.ID, aptEntity);


            //添加用户积分关联
            if (isAddScore)
            {
                lllegalpunishbll.SaveUserScore(pbEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                lllegalpunishbll.SaveUserScore(pbEntity.FIRSTINCHARGEID, entity.LLLEGALLEVEL);
                lllegalpunishbll.SaveUserScore(pbEntity.SECONDINCHARGEID, entity.LLLEGALLEVEL);
            }

            //确定要提交
            if (isSubmit)
            {
                //提交流程到下一节点
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，确认" + errorMsg + "!");
                }
            }
            else  //安全管理部门对装置类违章进行转发，转发至装置部门单位下，无需更改流程状态
            {
                bool isSuccess = htworkflowbll.SubmitWorkFlowNoChangeStatus(keyValue, participant, curUser.UserId);

                if (isSuccess)
                {
                    return Success("操作成功!");
                }
                else
                {
                    return Error("请联系系统管理员，确认" + errorMsg + "!");
                }
            }
            return Success("操作成功!");
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
    }
}
