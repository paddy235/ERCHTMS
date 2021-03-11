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
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章验收信息表
    /// </summary>
    public class LllegalAcceptController : MvcControllerBase
    {
        private UserBLL userbll = new UserBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //核准信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();

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

        #region 提交验收流程内容
        /// <summary>
        /// 提交验收流程内容
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalAcceptEntity acceptEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            try
            {

                /*
                 考虑两种情况  
                 * 当验收不通过，则验收退回到违章整改状态，并进行重新整改 ，这里需要注意，一旦不通过，则重新生成一条整改信息，一条验收信息.
                 * 当验收通过,则整改结束
                 */

                /*
                   验收  验收到整改结束
                */
                string wfFlag = string.Empty;  //流程标识

                string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)


                LllegalAcceptEntity aptEntity = lllegalacceptbll.GetEntityByBid(keyValue);
                aptEntity.ACCEPTRESULT = acceptEntity.ACCEPTRESULT;
                aptEntity.ACCEPTMIND = acceptEntity.ACCEPTMIND;
                aptEntity.MODIFYDATE = DateTime.Now;
                aptEntity.MODIFYUSERID = curUser.UserId;
                aptEntity.MODIFYUSERNAME = curUser.UserName;
                aptEntity.ACCEPTPIC = acceptEntity.ACCEPTPIC;
                lllegalacceptbll.SaveForm(aptEntity.ID, aptEntity);

                //不通过
                if (acceptEntity.ACCEPTRESULT == "0")
                {
                    //整改记录
                    LllegalReformEntity reformEntity = lllegalreformbll.GetEntityByBid(keyValue);
                    LllegalReformEntity newEntity = new LllegalReformEntity();
                    newEntity = reformEntity;
                    newEntity.CREATEDATE = DateTime.Now;
                    newEntity.MODIFYDATE = DateTime.Now;
                    newEntity.MODIFYUSERID = curUser.UserId;
                    newEntity.MODIFYUSERNAME = curUser.UserName;
                    newEntity.REFORMPIC = null; //重新生成图片GUID
                    newEntity.REFORMSTATUS = null; //整改完成情况
                    newEntity.REFORMMEASURE = null; //整改具体措施
                    newEntity.REFORMFINISHDATE = null; //整改完成时间
                    newEntity.ID = "";
                    lllegalreformbll.SaveForm("", newEntity);

                    //验收记录
                    LllegalAcceptEntity cptEntity = new LllegalAcceptEntity();
                    cptEntity = aptEntity;
                    cptEntity.ID = null;
                    cptEntity.CREATEDATE = DateTime.Now;
                    cptEntity.MODIFYDATE = DateTime.Now;
                    cptEntity.ACCEPTRESULT = null;
                    cptEntity.ACCEPTMIND = null;
                    cptEntity.ACCEPTPIC = null;
                    lllegalacceptbll.SaveForm("", cptEntity);


                    wfFlag = "2";  // 违章整改

                    UserEntity reformUser = userbll.GetEntity(reformEntity.REFORMPEOPLEID); //整改用户对象
                    //取整改人
                    participant = reformUser.Account;
                }
                else  //通过的情况下
                {
                    wfFlag = "1";  // 整改结束

                    participant = curUser.Account;
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

        #region 获取验收历史记录
        /// <summary>
        /// 获取验收历史记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = lllegalacceptbll.GetHistoryList(keyValue);
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
            var data = lllegalacceptbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
}
