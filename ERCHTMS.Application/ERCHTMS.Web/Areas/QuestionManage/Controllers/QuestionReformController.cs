using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.Busines.QuestionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using System;
using System.Web;

namespace ERCHTMS.Web.Areas.QuestionManage.Controllers
{
    /// <summary>
    /// 描 述：问题整改信息
    /// </summary>
    public class QuestionReformController : MvcControllerBase
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
            var data = questionreformbll.GetList(queryJson);
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
            var data = questionreformbll.GetEntity(keyValue);
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
        public ActionResult RemoveForm(string keyValue)
        {
            questionreformbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, QuestionReformEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            QuestionReformEntity rEntity = questionreformbll.GetEntityByBid(keyValue);
            rEntity.REFORMFINISHDATE = entity.REFORMFINISHDATE;
            rEntity.REFORMMEASURE = entity.REFORMMEASURE;
            rEntity.REFORMSTATUS = entity.REFORMSTATUS;
            rEntity.MODIFYDATE = DateTime.Now;
            rEntity.MODIFYUSERID = curUser.UserId;
            rEntity.MODIFYUSERNAME = curUser.UserName;
            rEntity.REFORMPIC = entity.REFORMPIC;
            rEntity.REFORMSIGN = !string.IsNullOrEmpty(entity.REFORMSIGN) ? HttpUtility.UrlDecode(entity.REFORMSIGN) :"";
            rEntity.REFORMDESCRIBE = entity.REFORMDESCRIBE;
            rEntity.REFORMFINISHDATE = entity.REFORMFINISHDATE;
            rEntity.REFORMPIC = entity.REFORMPIC;
            rEntity.REFORMREASON = entity.REFORMREASON;
            questionreformbll.SaveForm(rEntity.ID, rEntity);
            return Success("操作成功。");
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
        public ActionResult SubmitForm(string keyValue, QuestionInfoEntity entity, QuestionReformEntity rEntity, QuestionVerifyEntity aEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string participant = string.Empty;

            string wfFlag = string.Empty;

            WfControlResult result = new WfControlResult();
            try
            {
                //保存退回操作信息
                QuestionInfoEntity baseentity = questioninfobll.GetEntity(keyValue);
                questioninfobll.SaveForm(keyValue, baseentity);

                //整改信息
                QuestionReformEntity rfEntity = questionreformbll.GetEntityByBid(keyValue);
                rfEntity.REFORMDESCRIBE = rEntity.REFORMDESCRIBE; //整改情况描述
                rfEntity.REFORMFINISHDATE = rEntity.REFORMFINISHDATE; //整改结束时间
                rfEntity.REFORMMEASURE = rEntity.REFORMMEASURE; //整改措施
                rfEntity.REFORMSTATUS = rEntity.REFORMSTATUS; //整改完成情况
                rfEntity.REFORMSIGN = !string.IsNullOrEmpty(rEntity.REFORMSIGN) ? HttpUtility.UrlDecode(rEntity.REFORMSIGN) : "";
                rfEntity.REFORMREASON = rEntity.REFORMREASON; //未完成原因
                rfEntity.MODIFYDATE = DateTime.Now;
                rfEntity.MODIFYUSERID = curUser.UserId;
                rfEntity.MODIFYUSERNAME = curUser.UserName;
                rfEntity.REFORMPIC = rEntity.REFORMPIC; //整改图片

                #region MyRegion
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = "问题整改";
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.argument1 = curUser.UserId;
                wfentity.organizeid = baseentity.BELONGDEPTID; //对应电厂id
                wfentity.mark = "厂级问题流程";
                wfentity.submittype = "提交";


                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    //参与者
                    participant = result.actionperson;
                    //状态
                    wfFlag = result.wfflag;

                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //更新整改内容
                            //rfEntity.REFORMDEPTCODE = curUser.DeptCode; //当前人
                            //rfEntity.REFORMDEPTID = curUser.DeptId;
                            //rfEntity.REFORMDEPTNAME = curUser.DeptName;
                            rfEntity.REFORMPEOPLE = curUser.Account;
                            rfEntity.REFORMPEOPLENAME = curUser.UserName;
                            questionreformbll.SaveForm(rfEntity.ID, rfEntity);


                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        return Error("请联系系统管理员，确认提交问题!");
                    }
                }
                else if (result.code == WfCode.NoInstance || result.code == WfCode.NoEnable)
                {

                    wfFlag = "1"; //到验证阶段

                    participant = aEntity.VERIFYPEOPLE;  //验证人

                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //更新整改内容
                            //rfEntity.REFORMDEPTCODE = curUser.DeptCode; //当前人
                            //rfEntity.REFORMDEPTID = curUser.DeptId;
                            //rfEntity.REFORMDEPTNAME = curUser.DeptName;
                            rfEntity.REFORMPEOPLE = curUser.Account;
                            rfEntity.REFORMPEOPLENAME = curUser.UserName;
                            questionreformbll.SaveForm(rfEntity.ID, rfEntity);

                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", keyValue);  //更新业务流程状态
                        }
                        return Success("操作成功!");
                    }
                    else
                    {
                        return Success("操作失败,请联系管理员!");
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
                #endregion
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
            var data = questionreformbll.GetHistoryList(keyValue);
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
            var data = questionreformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
}