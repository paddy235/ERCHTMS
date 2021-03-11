using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System;
using ERCHTMS.Busines.SystemManage;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：隐患验收信息表
    /// </summary>
    public class HTAcceptInfoController : MvcControllerBase
    {
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL();
        private HtReCheckBLL htrecheckbll = new HtReCheckBLL();
        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL(); //部门操作对象
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
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeliverForm()
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
            var data = htacceptinfobll.GetList(hideCode);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htacceptinfobll.GetHistoryList(keyCode);
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
            var data = htacceptinfobll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除隐患验收信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htacceptinfobll.RemoveForm(keyValue);
            return Success("删除成功。");
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
        public ActionResult SaveForm(string keyValue, HTBaseInfoEntity bentity, HTAcceptInfoEntity entity, HTChangeInfoEntity centity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; //验收ID
            string ACCEPTSTATUS = Request.Form["ACCEPTSTATUS"] != null ? Request.Form["ACCEPTSTATUS"].ToString() : ""; //验收情况
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //整改ID
            string participant = string.Empty;  //获取流程下一节点的参与人员
            string wfFlag = string.Empty; //流程标识

            string IIMajorRisks = dataitemdetailbll.GetItemValue("IIMajorRisks"); //II级重大隐患

            string IMajorRisks = dataitemdetailbll.GetItemValue("IMajorRisks"); //I级重大隐患


            var baseEntity = htbaseinfobll.GetEntity(keyValue);
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            wfentity.argument1 = bentity.MAJORCLASSIFY; //专业分类
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
            //验收通过
            if (ACCEPTSTATUS == "1")
            {
                wfentity.submittype = "提交";
            }
            else //验收不通过
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
                            #region 操作其他项目
                            if (!string.IsNullOrEmpty(ACCEPTID))
                            {
                                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                                entity.AUTOID = tempEntity.AUTOID;
                            }
                            if (null == entity.ACCEPTDATE)
                            {
                                entity.ACCEPTDATE = DateTime.Now;
                            }
                            htacceptinfobll.SaveForm(ACCEPTID, entity);

                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态

                            //退回则重新添加验收记录
                            if (wfentity.submittype == "退回")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                                if (tagName == "隐患整改")
                                {
                                    //整改记录
                                    HTChangeInfoEntity chEntity = htchangeinfobll.GetEntity(CHANGEID);
                                    HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                                    newEntity = chEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.ID = null;
                                    newEntity.AUTOID = chEntity.AUTOID + 1;
                                    newEntity.CHANGERESUME = null;
                                    newEntity.CHANGEFINISHDATE = null;
                                    newEntity.REALITYMANAGECAPITAL = 0;
                                    newEntity.ATTACHMENT = Guid.NewGuid().ToString(); //整改附件
                                    newEntity.HIDCHANGEPHOTO = Guid.NewGuid().ToString(); //整改图片
                                    newEntity.APPSIGN = "Web";
                                    htchangeinfobll.SaveForm("", newEntity);
                                }
                        
                                //验收记录
                                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(bentity.HIDCODE);
                                HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                                accptEntity = htacceptinfoentity;
                                accptEntity.ID = null;
                                accptEntity.AUTOID = htacceptinfoentity.AUTOID + 1;
                                accptEntity.CREATEDATE = DateTime.Now;
                                accptEntity.MODIFYDATE = DateTime.Now;
                                accptEntity.ACCEPTSTATUS = null;
                                accptEntity.ACCEPTIDEA = null;
                                accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString(); //验收图片
                                accptEntity.APPSIGN = "Web";
                                htacceptinfobll.SaveForm("", accptEntity);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        return Error("请联系系统管理员，确认是否配置流程所需参与人!");
                    }
                }
                else
                {
                    //验收信息
                    if (!string.IsNullOrEmpty(ACCEPTID))
                    {
                        var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                        entity.AUTOID = tempEntity.AUTOID;
                    }
                    if (null == entity.ACCEPTDATE)
                    {
                        entity.ACCEPTDATE = DateTime.Now;
                    }
                    htacceptinfobll.SaveForm(ACCEPTID, entity);

                    //添加下一个验收对象
                    HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                    accptEntity = entity;
                    accptEntity.ID = string.Empty;
                    accptEntity.AUTOID = entity.AUTOID + 1;
                    accptEntity.CREATEDATE = DateTime.Now;
                    accptEntity.MODIFYDATE = DateTime.Now;
                    accptEntity.ACCEPTSTATUS = string.Empty;
                    accptEntity.ACCEPTIDEA = string.Empty;
                    accptEntity.ACCEPTDATE = null;
                    accptEntity.DAMAGEDATE = null;
                    accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString(); //验收图片
                    accptEntity.APPSIGN = "Web";
                    htacceptinfobll.SaveForm("", accptEntity);
                    //获取
                    htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

    }
}
