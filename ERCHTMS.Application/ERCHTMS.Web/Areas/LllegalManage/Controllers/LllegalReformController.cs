using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using System.Collections.Generic;
using System;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    #region 违章整改信息表
    /// <summary>
    /// 描 述：违章整改信息表
    /// </summary>
    public class LllegalReformController : MvcControllerBase
    {

        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //核准信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象
        private LllegalExtensionBLL lllegalextensionbll = new LllegalExtensionBLL(); //违章整改延期

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
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult KmIndex() 
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
        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Approval() 
        {
            return View();
        }
        
        /// <summary>
        /// 整改转交
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeliverForm() 
        {
            return View();
        }
        #endregion

        #region 保存表单信息
        /// <summary>
        /// 保存表单信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, LllegalReformEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            LllegalReformEntity rEntity = lllegalreformbll.GetEntityByBid(keyValue);
            rEntity.REFORMFINISHDATE = entity.REFORMFINISHDATE;
            rEntity.REFORMMEASURE = entity.REFORMMEASURE;
            rEntity.REFORMSTATUS = entity.REFORMSTATUS;
            rEntity.MODIFYDATE = DateTime.Now;
            rEntity.MODIFYUSERID = curUser.UserId;
            rEntity.MODIFYUSERNAME = curUser.UserName;
            rEntity.REFORMPIC = entity.REFORMPIC;
            rEntity.REFORMATTACHMENT = entity.REFORMATTACHMENT;
            lllegalreformbll.SaveForm(rEntity.ID, rEntity);
            return Success("操作成功。");
        }
        #endregion

        #region 设置延期申请天数
        /// <summary>
        /// 设置延期申请天数
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSettingForm(string keyValue, LllegalReformEntity entity)
        {
            Operator curUser = new OperatorProvider().Current();
            LllegalRegisterEntity bsentity = lllegalregisterbll.GetEntity(keyValue);//主键
            string rankname = string.Empty;

            bool isUpdateDate = false; //是否更新时间

            var cEntity = lllegalreformbll.GetEntityByBid(keyValue); //获取整改对象
            string postponereason = Request.Form["POSTPONEREASON"] != null ? Request.Form["POSTPONEREASON"].ToString() : "";


            try
            {
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = bsentity.MAJORCLASSIFY;
                wfentity.argument2 = bsentity.LLLEGALTYPE;
                wfentity.startflow = "整改延期申请";
                wfentity.submittype = "提交";
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.mark = "违章整改延期";
                wfentity.organizeid = bsentity.BELONGDEPARTID; //对应电厂id

                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //保存申请记录
                LllegalExtensionEntity exentity = new LllegalExtensionEntity();
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    string participant = result.actionperson;
                    string wfFlag = result.wfflag;

                    cEntity.POSTPONEPERSON = "," + participant + ",";  // 用于当前人账户判断是否具有操作其权限
                    cEntity.POSTPONEDAYS = entity.POSTPONEDAYS; //申请天数
                    cEntity.POSTPONEDEPT = result.deptcode;  //审批部门Code
                    cEntity.POSTPONEDEPTNAME = result.deptname;  //审批部门名称
                    cEntity.POSTPONEPERSONNAME = result.username;
                    cEntity.APPLICATIONSTATUS = wfFlag;
                    //是否更新时间，累加天数
                    if (wfFlag == "2") { isUpdateDate = true; }
                    //如果安环部、生技部审批通过，则更改整改截至时间、验收时间，增加相应的整改天数
                    if (isUpdateDate)
                    {
                        //重新赋值整改截至时间
                        cEntity.REFORMDEADLINE = cEntity.REFORMDEADLINE.Value.AddDays(cEntity.POSTPONEDAYS.Value);

                        //更新验收时间
                        LllegalAcceptEntity aEntity = lllegalacceptbll.GetEntityByBid(keyValue);
                        if (null != aEntity.ACCEPTTIME)
                        {
                            aEntity.ACCEPTTIME = aEntity.ACCEPTTIME.Value.AddDays(cEntity.POSTPONEDAYS.Value);
                        }
                        lllegalacceptbll.SaveForm(aEntity.ID, aEntity);

                        exentity.HANDLESIGN = "1"; //成功标记
                    }
                    cEntity.APPSIGN = "Web";
                    //更新整改信息
                    lllegalreformbll.SaveForm(cEntity.ID, cEntity); //更新延期设置

                    exentity.LLLEGALID = keyValue;
                    exentity.HANDLEDATE = DateTime.Now;
                    exentity.POSTPONEDAYS = entity.POSTPONEDAYS.ToString();
                    exentity.HANDLEUSERID = curUser.UserId;
                    exentity.HANDLEUSERNAME = curUser.UserName;
                    exentity.HANDLEDEPTCODE = curUser.DeptCode;
                    exentity.HANDLEDEPTNAME = curUser.DeptName;
                    exentity.HANDLETYPE = "0";  //申请类型  状态返回 2 时表示整改延期完成 (申请)
                    exentity.HANDLEID = DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
                    exentity.POSTPONEREASON = postponereason;
                    exentity.APPSIGN = "Web";
                    lllegalextensionbll.SaveForm("", exentity);

                    //极光推送
                    if (wfFlag != "2") 
                    {
                          htworkflowbll.PushMessageForWorkFlow("违章管理流程", "整改延期审批", wfentity, result); 
                    }
      

                    return Success(result.message);
                }
                else
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message.ToString());
            }
        } 
        #endregion

        #region 提交整改内容
        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity acceptEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string participant = string.Empty;

            string wfFlag = string.Empty;

            WfControlResult result  = new WfControlResult();
            try
            {
                //保存退回操作信息
                LllegalRegisterEntity baseentity = lllegalregisterbll.GetEntity(keyValue);
                baseentity.RESEVERFOUR = entity.RESEVERFOUR;
                baseentity.RESEVERFIVE = entity.RESEVERFIVE;
                lllegalregisterbll.SaveForm(keyValue, baseentity);

                //整改信息
                LllegalReformEntity rfEntity = lllegalreformbll.GetEntityByBid(keyValue);
                rfEntity.REFORMFINISHDATE = rEntity.REFORMFINISHDATE;
                rfEntity.REFORMMEASURE = rEntity.REFORMMEASURE;
                rfEntity.REFORMSTATUS = rEntity.REFORMSTATUS;
                rfEntity.MODIFYDATE = DateTime.Now;
                rfEntity.MODIFYUSERID = curUser.UserId;
                rfEntity.MODIFYUSERNAME = curUser.UserName;
                rfEntity.REFORMPIC = rEntity.REFORMPIC;
                rfEntity.REFORMATTACHMENT = rEntity.REFORMATTACHMENT;
                lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.argument1 = baseentity.MAJORCLASSIFY; //专业分类
                wfentity.startflow = baseentity.FLOWSTATE;
                wfentity.rankid =null;
                wfentity.user = curUser;
                wfentity.organizeid = baseentity.BELONGDEPARTID; //对应电厂id
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                //退回 
                if (entity.RESEVERFOUR == "是")
                {
                    //历史记录
                    var reformitem = lllegalreformbll.GetHistoryList(entity.ID).ToList();
                    //如果未整改可以退回
                    if (reformitem.Count() == 0)
                    {
                        wfentity.submittype = "退回";
                    }
                    else
                    {
                        return Error("整改过后的违章无法再次退回!");
                    }
                }
                else //正常提交到验收流程
                {
                    wfentity.submittype = "提交";
                }


                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    //参与者
                    participant = result.actionperson;
                    //状态
                    wfFlag = result.wfflag;

                      //如果是更改状态
                    if (result.ischangestatus)
                    {
                        //提交流程到下一节点
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                            }
                        }
                        else
                        {
                            return Error("请联系系统管理员，确认提交问题!");
                        }
                    }
                    else 
                    {
                        //获取
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                        #region 当前还处于违章整改阶段
                        if (tagName == "违章整改" )
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            rfEntity.REFORMPEOPLE = userentity.RealName;
                            rfEntity.REFORMPEOPLEID = userentity.UserId;
                            rfEntity.REFORMDEPTNAME = userentity.DeptName;
                            rfEntity.REFORMDEPTCODE = userentity.DepartmentCode;
                            rfEntity.REFORMTEL = userentity.Telephone;
                            lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);
                        }
                        #endregion
                    }
                }
                //非自动处理的流程
                else if (result.code == WfCode.NoAutoHandle)
                {
                    bool isupdate = false;//是否更改流程状态
                    //退回操作  单独处理
                    if (entity.RESEVERFOUR == "是")
                    {
                        DataTable dt = htworkflowbll.GetBackFlowObjectByKey(keyValue);

                        if (dt.Rows.Count > 0)
                        {
                            wfFlag = dt.Rows[0]["wfflag"].ToString(); //流程走向

                            participant = dt.Rows[0]["participant"].ToString();  //指向人

                            isupdate = dt.Rows[0]["isupdate"].ToString()=="1"; //是否更改流程状态
                        }
                    }
                    //更改流程状态的情况下
                    if (isupdate)
                    {
                        if (!string.IsNullOrEmpty(participant) && !string.IsNullOrEmpty(wfFlag))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                            }
                            result.message = "处理成功";
                            result.code = WfCode.Sucess;
                        }
                    }
                    else 
                    {
                        //不更改流程状态下
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                        #region 当前还处于违章整改阶段
                        if (tagName == "违章整改")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            rfEntity.REFORMPEOPLE = userentity.RealName;
                            rfEntity.REFORMPEOPLEID = userentity.UserId;
                            rfEntity.REFORMDEPTNAME = userentity.DeptName;
                            rfEntity.REFORMDEPTCODE = userentity.DepartmentCode;
                            rfEntity.REFORMTEL = userentity.Telephone;
                            lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);
                        }
                        #endregion

                        result.message = "处理成功";
                        result.code = WfCode.Sucess;
                    }

                }

                if (result.code == WfCode.Sucess)
                {
                    return Success(result.message);
                }
                else //其他返回状态
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        

        #region 获取整改历史记录
        /// <summary>
        /// 获取整改历史记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = lllegalreformbll.GetHistoryList(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 获取整改详情记录
        /// <summary>
        /// 获取整改详情记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntityJson(string keyValue)
        {
            var data = lllegalreformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
    #endregion
}
