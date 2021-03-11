using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    public class HTReCheckController : MvcControllerBase
    {
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL();
        private HtReCheckBLL htrecheckbll = new HtReCheckBLL();
        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务
        //
        // GET: /HiddenTroubleManage/HTReCheck/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
         [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
         [HttpGet]
         public ActionResult Detail() 
         {
             return View();
         } 
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string hideCode)
        {
            var data = htrecheckbll.GetList(hideCode);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htrecheckbll.GetHistoryList(keyCode);
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
            var data = htrecheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 保存表单（新增、修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HtReCheckEntity entity, HTChangeInfoEntity centity, HTBaseInfoEntity bentity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string RECHECKID = Request.Form["RECHECKID"] != null ? Request.Form["RECHECKID"].ToString() : ""; //复查验证ID
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //整改ID
            string participant = string.Empty;  //获取流程下一节点的参与人员
            string wfFlag = string.Empty; //流程标识

            var baseEntity = htbaseinfobll.GetEntity(keyValue);
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            wfentity.argument1 = bentity.MAJORCLASSIFY;
            wfentity.argument3 = bentity.HIDTYPE; //隐患类别
            wfentity.argument4 = bentity.HIDBMID; //所属部门
            wfentity.startflow = baseEntity.WORKSTREAM;
            wfentity.rankid = baseEntity.HIDRANK;
            wfentity.user = curUser;
            wfentity.organizeid = baseEntity.HIDDEPART; //对应电厂id
            //省级登记的
            if (baseEntity.ADDTYPE == "2")
            {
                wfentity.mark = "省级隐患排查";
            }
            else //厂级
            {
                wfentity.mark = "厂级隐患排查";
            }
            //复查通过
            if (entity.RECHECKSTATUS == "1")
            {
                wfentity.submittype = "提交";
            }
            else //复查不通过
            {
                wfentity.submittype = "退回";
            }

             //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            
            //返回操作结果成功
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;

                wfFlag = result.wfflag;

                   //如果是更改状态
                if (result.ischangestatus)
                {
                    
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //提交流程
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //保存复查验证
                            if (!string.IsNullOrEmpty(RECHECKID))
                            {
                                var tempEntity = htrecheckbll.GetEntity(RECHECKID);
                                entity.AUTOID = tempEntity.AUTOID;
                            }
                            htrecheckbll.SaveForm(RECHECKID, entity); //保存信息

                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态

                            #region 退回后重新新增整改记录及整改效果评估记录
                            if (wfentity.submittype == "退回")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                                if (tagName == "隐患整改")
                                {
                                    //整改记录
                                    HTChangeInfoEntity cEntity = htchangeinfobll.GetEntity(CHANGEID);
                                    if (null != cEntity)
                                    {
                                        HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                                        newEntity = cEntity;
                                        newEntity.CREATEDATE = DateTime.Now;
                                        newEntity.MODIFYDATE = DateTime.Now;
                                        newEntity.ID = null;
                                        newEntity.AUTOID = cEntity.AUTOID + 1;
                                        newEntity.CHANGERESUME = null;
                                        newEntity.CHANGEFINISHDATE = null;
                                        newEntity.REALITYMANAGECAPITAL = 0;
                                        newEntity.ATTACHMENT = Guid.NewGuid().ToString(); //整改附件
                                        newEntity.HIDCHANGEPHOTO = Guid.NewGuid().ToString(); //整改图片
                                        newEntity.APPSIGN = "Web";
                                        htchangeinfobll.SaveForm("", newEntity);
                                    }
                                }
                                //验收记录
                                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(baseEntity.HIDCODE);
                                if (null != htacceptinfoentity)
                                {
                                    HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                                    accptEntity = htacceptinfoentity;
                                    accptEntity.ID = null;
                                    accptEntity.AUTOID = htacceptinfoentity.AUTOID + 1;
                                    accptEntity.CREATEDATE = DateTime.Now;
                                    accptEntity.MODIFYDATE = DateTime.Now;
                                    accptEntity.ACCEPTSTATUS = null;
                                    accptEntity.ACCEPTIDEA = null;
                                    accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                                    accptEntity.APPSIGN = "Web";
                                    htacceptinfobll.SaveForm("", accptEntity);
                                }
                            }
                            #endregion
                        }
                        return Success(result.message);
                    }
                    else
                    {
                        return Error("目标流程参与者未定义");
                    }
                }
                else 
                {
                    //保存复查验证
                    if (!string.IsNullOrEmpty(RECHECKID))
                    {
                        var tempEntity = htrecheckbll.GetEntity(RECHECKID);
                        entity.AUTOID = tempEntity.AUTOID;
                    }
                    htrecheckbll.SaveForm(RECHECKID, entity); //保存信息
                    //获取
                    htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                    return Success(result.message);
                }
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

    }
}
