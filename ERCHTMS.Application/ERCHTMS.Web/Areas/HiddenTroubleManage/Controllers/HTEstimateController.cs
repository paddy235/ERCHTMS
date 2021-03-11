using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：隐患整改效果评估表
    /// </summary>
    public class HTEstimateController : MvcControllerBase
    {
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private UserBLL userbll = new UserBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
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
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = htestimatebll.GetList(queryJson);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htestimatebll.GetHistoryList(keyCode);
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
            var data = htestimatebll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除隐患整改效果评估信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htestimatebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HTAcceptInfoEntity atEntity, HTChangeInfoEntity chEntity, HTEstimateEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; //验收ID
            string ESTIMATEID = Request.Form["ESTIMATEID"] != null ? Request.Form["ESTIMATEID"].ToString() : ""; //整改效果评估ID
            string ESTIMATERESULT = Request.Form["ESTIMATERESULT"] != null ? Request.Form["ESTIMATERESULT"].ToString() : ""; //评估情况
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //整改ID
            string participant = string.Empty;  //获取流程下一节点的参与人员
            string wfFlag = string.Empty; //流程标识


            var baseEntity = htbaseinfobll.GetEntity(keyValue);
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
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
            //评估合格
            if (ESTIMATERESULT == "1")
            {
                wfentity.submittype = "提交";
            }
            else //评估不通过
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

                if (!string.IsNullOrEmpty(participant))
                {
                    //提交流程
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态

                        //保存整改效果评估
                        if (!string.IsNullOrEmpty(ESTIMATEID))
                        {
                            var tempEntity = htestimatebll.GetEntity(ESTIMATEID);
                            entity.AUTOID = tempEntity.AUTOID;
                        }
                        htestimatebll.SaveForm(ESTIMATEID, entity);

                        //退回后重新新增整改记录及整改效果评估记录
                        if (wfentity.submittype == "退回")
                        {
                            string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                            if (tagName == "隐患整改")
                            {
                                //整改记录
                                HTChangeInfoEntity cEntity = htchangeinfobll.GetEntity(CHANGEID);
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

                            HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(baseEntity.HIDCODE);
                            //验收记录
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
                    }
                }
                else
                {
                    return Error("目标流程参与者未定义");
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
