using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：整改延期对象
    /// </summary>
    public class HTExtensionController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTExtensionBLL htextensionbll = new HTExtensionBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
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

        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GroupList() 
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
        public ActionResult GetListJson(string keyValue)
        {
            var data = htextensionbll.GetList(keyValue); 
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListByCondition(string keyValue)  
        {
            var data = htextensionbll.GetListByCondition(keyValue);  
            return ToJsonResult(data); 
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetGroupListJson(string handleid, string hidcode)  
        {
            var data = htextensionbll.GetList(hidcode).Where(p=>p.HANDLEID==handleid).ToList();
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
            var data = htextensionbll.GetEntity(keyValue);
            var cEntity = htchangeinfobll.GetEntityByHidCode(data.HIDCODE);
            var JsonData = new
            {
                data = data ,
                cdata = cEntity
            };
            return Content(JsonData.ToJson());
        }
        #endregion

        [HttpGet]
        public ActionResult GetFirstObjectJson(string keyValue)
        {
            var data = htextensionbll.GetFirstEntity(keyValue);
            return ToJsonResult(data);
        }

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除隐患整改延期信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htextensionbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HTExtensionEntity entity)
        {
            Operator curUser = new OperatorProvider().Current();

            string postPoneResult = Request.Form["POSTPONERESULT"].ToString();  //审批结果

            var bsentity = htbaseinfobll.GetEntity(keyValue); //根据id获取隐患对象

            //string postponereason = Request.Form["POSTPONEREASON"].ToString(); //审批意见

            string rankname = string.Empty;

             bool isUpdateDate = false;

             string participant = string.Empty;
             string wfFlag =string.Empty;

             string hidcode = bsentity.HIDCODE;

             try
             {
                 WfControlResult result = new WfControlResult();
                 WfControlObj wfentity = new WfControlObj();
                 wfentity.businessid = keyValue; //
                 wfentity.startflow = "整改延期审批";
                 wfentity.submittype = "提交";
                 wfentity.rankid = bsentity.HIDRANK;
                 wfentity.user = curUser;
                 wfentity.mark = "整改延期流程";
                 wfentity.organizeid = bsentity.HIDDEPART; //对应电厂id

                 HTChangeInfoEntity cEntity = htchangeinfobll.GetEntityByHidCode(hidcode);
                 //是否通过
                 if (postPoneResult == "1")
                 {
                     //获取下一流程的操作人
                     result = wfcontrolbll.GetWfControl(wfentity);
                     //处理成功
                     if (result.code == WfCode.Sucess)
                     {
                         participant = result.actionperson;
                         wfFlag = result.wfflag;

                         cEntity.POSTPONEPERSON = "," + participant + ",";  // 用于当前人账户判断是否具有操作其权限
                         cEntity.POSTPONEDAYS = cEntity.POSTPONEDAYS; //申请天数
                         cEntity.POSTPONEDEPT = result.deptcode;  //审批部门Code
                         cEntity.POSTPONEDEPTNAME = result.deptname;  //审批部门名称
                         cEntity.POSTPONEPERSONNAME = result.username; //审批人
                         cEntity.APPLICATIONSTATUS = wfFlag; //延期通过
                     }
                     else 
                     {
                         return Error(result.message);
                     }
                 }
                 else
                 {
                     cEntity.APPLICATIONSTATUS = "3"; //延期申请失败
                     //延期失败保存整改人相关信息到result,用于极光推送
                     UserEntity  changeUser = userbll.GetEntity(cEntity.CHANGEPERSON);
                     if(null!=changeUser)
                     {
                         result.actionperson = changeUser.Account;
                         result.username = cEntity.CHANGEPERSONNAME;
                         result.deptname = cEntity.CHANGEDUTYDEPARTNAME;
                         result.deptid = changeUser.DepartmentId;
                         result.deptcode = cEntity.CHANGEDUTYDEPARTCODE;
                     }
                 }

                 //延期成功 
                 if (wfFlag == "2" && postPoneResult == "1")
                 {
                     isUpdateDate = true;
                 }
                 //如果安环部、生技部审批通过，则更改整改截至时间、验收时间，增加相应的整改天数
                 if (isUpdateDate)
                 {
                     //重新赋值整改截至时间
                     cEntity.CHANGEDEADINE = cEntity.CHANGEDEADINE.Value.AddDays(cEntity.POSTPONEDAYS);

                     //更新验收时间
                     HTAcceptInfoEntity aEntity = htacceptinfobll.GetEntityByHidCode(hidcode);
                     if (null != aEntity.ACCEPTDATE)
                     {
                         aEntity.ACCEPTDATE = aEntity.ACCEPTDATE.Value.AddDays(cEntity.POSTPONEDAYS);
                     }
                     htacceptinfobll.SaveForm(aEntity.ID, aEntity);

                     entity.HANDLESIGN = "1"; //成功标记
                 }
                 cEntity.APPSIGN = "Web";
                 //更新整改信息
                 htchangeinfobll.SaveForm(cEntity.ID, cEntity);

                 //添加审批记录
                 entity.APPSIGN = "Web";

                 string nextName = string.Empty;

                 //成功
                 if (wfFlag == "2" && postPoneResult == "1")
                 {
                     entity.HANDLETYPE = wfFlag;  //处理类型 0 申请 1 审批 2 整改结束    wfFlag状态返回 2 时表示整改延期完成
                 }
                 //审批中
                 else if (wfFlag != "2" && postPoneResult == "1")
                 {
                     entity.HANDLETYPE = "1";  //审批中

                     nextName = "整改延期审批";
                 }
                 else //失败
                 {
                     if (postPoneResult == "0")
                     {
                         entity.HANDLETYPE = "-1";  //失败

                         nextName = "整改延期退回";
                     }
                 }
               
                 entity.POSTPONERESULT = postPoneResult;  //申请结果
                 htextensionbll.SaveForm("", entity);

                 //极光推送
                 htworkflowbll.PushMessageForWorkFlow("隐患处理审批", nextName, wfentity, result);
             }
             catch (Exception ex)
             {
                 return Error(ex.Message.ToString());
             }

            return Success("操作成功。");
        }
        #endregion


        /// <summary>
        /// 提交延期整改天数
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, string value)
        {
            HTChangeInfoEntity cEntity = htchangeinfobll.GetEntityByHidCode(keyValue);
            cEntity.POSTPONEDAYS = !string.IsNullOrEmpty(value) ? int.Parse(value) : 0;
            cEntity.APPLICATIONSTATUS = "1"; //标记正在延期申请之中
            htchangeinfobll.SaveForm(cEntity.ID, cEntity);
            return Success("操作成功。");
        }
    }
}
