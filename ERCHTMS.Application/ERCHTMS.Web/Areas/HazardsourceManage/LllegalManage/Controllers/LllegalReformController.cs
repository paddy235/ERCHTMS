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
            lllegalreformbll.SaveForm(rEntity.ID, rEntity);
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
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity acceptEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
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
                lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);

                /*
                 整改  整改到验收
                 */
                string errorMsg = string.Empty;

                string wfFlag = string.Empty;

                string participant = string.Empty;

                //退回操作
                if (entity.RESEVERFOUR == "是")
                {
                    DataTable dt = htworkflowbll.GetBackFlowObjectByKey(keyValue);

                    if (dt.Rows.Count > 0)
                    {
                        wfFlag = dt.Rows[0]["wfflag"].ToString(); //流程走向

                        participant = dt.Rows[0]["participant"].ToString();  //指向人
                    }
                }
                else
                {
                    wfFlag = "1";  //流程标识
                    //获取验收人
                    UserEntity userEntity = userbll.GetEntity(acceptEntity.ACCEPTPEOPLEID); //验收人

                    if (null != userEntity)
                    {
                        participant = userEntity.Account;  //获取流程下一节点的参与人员 (取验收人)
                    }
                }


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
                    return Error("请联系系统管理员，确认提交问题!");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Success("操作成功!");
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
