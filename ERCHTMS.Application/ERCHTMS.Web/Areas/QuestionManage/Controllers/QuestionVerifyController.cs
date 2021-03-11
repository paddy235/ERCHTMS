using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.QuestionManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.QuestionManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.QuestionManage.Controllers
{
    public class QuestionVerifyController : MvcControllerBase
    {

        private QuestionInfoBLL questioninfobll = new QuestionInfoBLL();
        private QuestionReformBLL questionreformbll = new QuestionReformBLL();
        private QuestionVerifyBLL questionverifybll = new QuestionVerifyBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务

        /// <summary>
        /// 验证页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        //验证详情
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 整改历史列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        // <summary>
        /// 整改历史详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }

        // <summary>
        /// 重新指定验证人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PointForm()
        {
            return View();
        }

        #region 提交问题验证内容
        /// <summary>
        /// 提交问题验证内容
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, QuestionVerifyEntity qEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlResult result = new WfControlResult();
            try
            {
                string wfFlag = string.Empty;  //流程标识

                string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)

                QuestionInfoEntity baseEntity = questioninfobll.GetEntity(keyValue);
                QuestionReformEntity reformEntity = questionreformbll.GetEntityByBid(keyValue);

                QuestionVerifyEntity aptEntity = questionverifybll.GetEntityByBid(keyValue);
                if (null == aptEntity)
                {
                    aptEntity = new QuestionVerifyEntity();
                }
                aptEntity.QUESTIONID = keyValue;
                aptEntity.VERIFYRESULT = qEntity.VERIFYRESULT;
                aptEntity.VERIFYOPINION = qEntity.VERIFYOPINION;
                aptEntity.MODIFYDATE = DateTime.Now;
                aptEntity.MODIFYUSERID = curUser.UserId;
                aptEntity.MODIFYUSERNAME = curUser.UserName;
                aptEntity.VERIFYSIGN = !string.IsNullOrEmpty(qEntity.VERIFYSIGN) ? HttpUtility.UrlDecode(qEntity.VERIFYSIGN) : "";
                aptEntity.VERIFYDATE = qEntity.VERIFYDATE;


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = baseEntity.FLOWSTATE;
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.argument1 = curUser.UserId;
                wfentity.organizeid = baseEntity.BELONGDEPTID; //对应电厂id
                //厂级
                wfentity.mark = "厂级问题流程";
                //验证通过
                if (qEntity.VERIFYRESULT == "1")
                {
                    wfentity.submittype = "提交";
                }
                else //验证不通过
                {
                    wfentity.submittype = "退回";
                }

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                //返回操作结果成功(配置流程的情况下)
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    //如果是更改状态
                    #region 如果是更改状态
                    if (result.ischangestatus)
                    {
                        //提交流程到下一节点
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);
                            if (count > 0)
                            {
                                //添加问题验证记录
                                aptEntity.ID = null;
                                aptEntity.VERIFYDEPTID = curUser.DeptId;
                                aptEntity.VERIFYDEPTCODE = curUser.DeptCode;
                                aptEntity.VERIFYDEPTNAME = curUser.DeptName;
                                aptEntity.VERIFYPEOPLE = curUser.Account;
                                aptEntity.VERIFYPEOPLENAME = curUser.UserName;
                                questionverifybll.SaveForm("", aptEntity);

                                //退回则重新添加验证记录
                                if (wfFlag == "2")
                                {
                                    QuestionReformEntity newEntity = new QuestionReformEntity();
                                    newEntity = reformEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.MODIFYUSERID = curUser.UserId;
                                    newEntity.MODIFYUSERNAME = curUser.UserName;
                                    newEntity.REFORMPIC = null; //重新生成图片GUID
                                    newEntity.REFORMSTATUS = null; //整改完成情况
                                    newEntity.REFORMDESCRIBE = null; //整改情况描述
                                    newEntity.REFORMFINISHDATE = null; //整改完成时间
                                    newEntity.ID = "";
                                    questionreformbll.SaveForm("", newEntity);
                                    //验证记录记录
                                    QuestionVerifyEntity cptEntity = new QuestionVerifyEntity();
                                    cptEntity = aptEntity;
                                    cptEntity.ID = null;
                                    cptEntity.CREATEDATE = DateTime.Now;
                                    cptEntity.MODIFYDATE = DateTime.Now;
                                    cptEntity.VERIFYRESULT = null;
                                    cptEntity.VERIFYOPINION = null;
                                    cptEntity.VERIFYSIGN = null;
                                    questionverifybll.SaveForm("", cptEntity);
                                }
                                htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", keyValue);  //更新业务流程状态
                            }
                            else
                            {
                                return Error("当前用户无评估权限!");
                            }
                        }
                        else
                        {
                            return Error(result.message);
                        }
                    }
                    #endregion
                    #region 不更改状态的情况下
                    else  //不更改状态的情况下
                    {
                        //保存隐患评估信息
                        #region 提交流程到下一节点
                        if (!string.IsNullOrEmpty(participant))
                        {
                            //添加问题验证记录
                            aptEntity.ID = null;
                            aptEntity.VERIFYDEPTID = curUser.DeptId;
                            aptEntity.VERIFYDEPTCODE = curUser.DeptCode;
                            aptEntity.VERIFYDEPTNAME = curUser.DeptName;
                            aptEntity.VERIFYPEOPLE = curUser.Account;
                            aptEntity.VERIFYPEOPLENAME = curUser.UserName;
                            questionverifybll.SaveForm("", aptEntity);

                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                        }
                        else
                        {
                            return Error(result.message);
                        }
                        #endregion
                    }
                    #endregion
                }
                //不按照配置
                else if (result.code == WfCode.NoInstance || result.code == WfCode.NoEnable)
                {
                    //验证通过
                    if (qEntity.VERIFYRESULT == "1")
                    {
                        wfFlag = "1";

                        participant = curUser.Account;
                    }
                    else //验证不通过
                    {
                        wfFlag = "2";

                        participant = reformEntity.REFORMPEOPLE; //整改人
                    }

                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //添加问题验证记录
                            aptEntity.VERIFYDEPTID = curUser.DeptId;
                            aptEntity.VERIFYDEPTCODE = curUser.DeptCode;
                            aptEntity.VERIFYDEPTNAME = curUser.DeptName;
                            aptEntity.VERIFYPEOPLE = curUser.Account;
                            aptEntity.VERIFYPEOPLENAME = curUser.UserName;
                            questionverifybll.SaveForm(aptEntity.ID, aptEntity);

                            //退回则重新添加验证记录
                            if (wfFlag == "2")
                            {
                                QuestionReformEntity newEntity = new QuestionReformEntity();
                                newEntity = reformEntity;
                                newEntity.CREATEDATE = DateTime.Now;
                                newEntity.MODIFYDATE = DateTime.Now;
                                newEntity.MODIFYUSERID = curUser.UserId;
                                newEntity.MODIFYUSERNAME = curUser.UserName;
                                newEntity.REFORMPIC = null; //重新生成图片GUID
                                newEntity.REFORMSTATUS = null; //整改完成情况
                                newEntity.REFORMDESCRIBE = null; //整改情况描述
                                newEntity.REFORMFINISHDATE = null; //整改完成时间
                                newEntity.ID = "";
                                questionreformbll.SaveForm("", newEntity);
                                //验证记录记录
                                QuestionVerifyEntity cptEntity = new QuestionVerifyEntity();
                                cptEntity = aptEntity;
                                cptEntity.ID = null;
                                cptEntity.CREATEDATE = DateTime.Now;
                                cptEntity.MODIFYDATE = DateTime.Now;
                                cptEntity.VERIFYRESULT = null;
                                cptEntity.VERIFYOPINION = null;
                                cptEntity.VERIFYSIGN = null;
                                questionverifybll.SaveForm("", cptEntity);
                            }

                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", keyValue);  //更新业务流程状态
                        }
                        result.message = "操作成功!";
                    }
                    else
                    {
                        return Error("操作失败,请联系管理员!");
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success(result.message);
        }
        #endregion



        #region 重新指定验证人内容
        /// <summary>
        /// 重新指定验证人内容
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult PointSubmitForm(string keyValue, QuestionVerifyEntity qEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            try
            {
                string wfFlag = string.Empty;  //流程标识

                string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)

                QuestionInfoEntity baseEntity = questioninfobll.GetEntity(keyValue);
                QuestionReformEntity reformEntity = questionreformbll.GetEntityByBid(keyValue);

                QuestionVerifyEntity aptEntity = questionverifybll.GetEntityByBid(keyValue);
                aptEntity.VERIFYRESULT = qEntity.VERIFYRESULT;
                aptEntity.VERIFYOPINION = qEntity.VERIFYOPINION;
                aptEntity.MODIFYDATE = DateTime.Now;
                aptEntity.MODIFYUSERID = curUser.UserId;
                aptEntity.MODIFYUSERNAME = curUser.UserName;


                //验证信息
                qEntity.QUESTIONID = keyValue;
                questionverifybll.SaveForm("", qEntity);


                //返回操作结果成功
                if (baseEntity.FLOWSTATE == "流程结束")
                {
                    wfFlag = "1"; //指定到验证阶段

                    participant = qEntity.VERIFYPEOPLE; //验证人

                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        return Error("请选择验证人!");
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("提交成功!");
        }
        #endregion

        #region 获取验收历史记录
        /// <summary>
        /// 获取验收历史记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = questionverifybll.GetHistoryList(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 获取验收详情记录
        /// <summary>
        /// 获取验收详情记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntityJson(string keyValue)
        {
            var data = questionverifybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
}