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

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：隐患整改信息表
    /// </summary>
    public class HTChangeInfoController : MvcControllerBase
    {
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL();
        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTExtensionBLL htextensionbll = new HTExtensionBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息

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
        public ActionResult Approval()
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = htchangeinfobll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据部门编码获取实体
        /// </summary>
        /// <param name="HidCode"></param>
        /// <returns></returns>
        public ActionResult GetEntityByCode(string keyValue)
        {
            var data = htchangeinfobll.GetEntityByCode(keyValue);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htchangeinfobll.GetHistoryList(keyCode);
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
            var data = htchangeinfobll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除隐患整改信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htchangeinfobll.RemoveForm(keyValue);
            return Success("删除成功。");
        }


        /// <summary>
        /// 保存表单信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HTChangeInfoEntity entity)
        {
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //整改ID
            htchangeinfobll.SaveForm(CHANGEID, entity);
            return Success("操作成功。");
        }



        /// <summary>
        /// 设置延期申请天数
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSettingForm(string keyCode, HTChangeInfoEntity entity)
        {
            Operator curUser = new OperatorProvider().Current();
            string hidid = Request.Form["HIDID"] != null ? Request.Form["HIDID"].ToString() : ""; //隐患主键
            HTBaseInfoEntity bsentity = new HTBaseInfoBLL().GetEntity(hidid);//主键
            string rankname = string.Empty;

            bool isUpdateDate = false; //是否更新时间

            var cEntity = htchangeinfobll.GetEntityByCode(keyCode); //根据HidCode获取整改对象
            string postponereason = Request.Form["POSTPONEREASON"] != null ? Request.Form["POSTPONEREASON"].ToString() : "";
            string controlmeasure = Request.Form["CONTROLMEASURE"] != null ? Request.Form["CONTROLMEASURE"].ToString() : ""; //管控措施

            try
            {
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = hidid; //
                wfentity.argument1 = bsentity.MAJORCLASSIFY;
                wfentity.argument3 = bsentity.HIDTYPE; //隐患类别
                wfentity.startflow = "整改延期申请";
                wfentity.submittype = "提交";
                wfentity.rankid = bsentity.HIDRANK;
                wfentity.user = curUser;
                wfentity.mark = "整改延期流程";
                wfentity.organizeid = bsentity.HIDDEPART; //对应电厂id

                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //保存申请记录
                HTExtensionEntity exentity = new HTExtensionEntity();
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
                        cEntity.CHANGEDEADINE = cEntity.CHANGEDEADINE.Value.AddDays(cEntity.POSTPONEDAYS);

                        //更新验收时间
                        HTAcceptInfoEntity aEntity = htacceptinfobll.GetEntityByHidCode(keyCode);
                        if (null != aEntity.ACCEPTDATE) 
                        {
                            aEntity.ACCEPTDATE = aEntity.ACCEPTDATE.Value.AddDays(cEntity.POSTPONEDAYS);
                        }
                        htacceptinfobll.SaveForm(aEntity.ID, aEntity);

                        exentity.HANDLESIGN = "1"; //成功标记
                    }
                    cEntity.APPSIGN = "Web";
                    //更新整改信息
                    htchangeinfobll.SaveForm(cEntity.ID, cEntity); //更新延期设置

                    exentity.HIDCODE = cEntity.HIDCODE;
                    exentity.HIDID = hidid;
                    exentity.HANDLEDATE = DateTime.Now;
                    exentity.POSTPONEDAYS = entity.POSTPONEDAYS.ToString();
                    exentity.HANDLEUSERID = curUser.UserId;
                    exentity.HANDLEUSERNAME = curUser.UserName;
                    exentity.HANDLEDEPTCODE = curUser.DeptCode;
                    exentity.HANDLEDEPTNAME = curUser.DeptName;
                    exentity.HANDLETYPE = "0";  //申请类型  状态返回 2 时表示整改延期完成 (申请)
                    exentity.HANDLEID = DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
                    exentity.POSTPONEREASON = postponereason;//延期理由
                    exentity.CONTROLMEASURE = controlmeasure;//临时管控措施
                    exentity.APPSIGN = "Web";
                    htextensionbll.SaveForm("", exentity);

                    //极光推送
                    htworkflowbll.PushMessageForWorkFlow("隐患处理审批", "整改延期审批", wfentity, result); 

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

        /// <summary>
        /// 提交表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, HTBaseInfoEntity bEntity, HTChangeInfoEntity entity, HTAcceptInfoEntity aEntity)
        {

            Operator curUser = OperatorProvider.Provider.Current();

            string CHANGERESULT = Request.Form["CHANGERESULT"] != null ? Request.Form["CHANGERESULT"].ToString() : ""; //整改结果
            string ISBACK = Request.Form["ISBACK"] != null ? Request.Form["ISBACK"].ToString() : ""; //是否回退
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //整改ID

            string participant = string.Empty;  //获取流程下一节点的参与人员
            string wfFlag = string.Empty; //流程标识
            try
            {

                if (!string.IsNullOrEmpty(CHANGEID))
                {
                    var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                    entity.AUTOID = tempEntity.AUTOID;
                    entity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                    entity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                    entity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                    entity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
                }
                entity.APPSIGN = "Web";
                htchangeinfobll.SaveForm(CHANGEID, entity);

                //隐患基本信息
                var baseEntity = htbaseinfobll.GetEntity(keyValue);


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.argument1 = bEntity.MAJORCLASSIFY; //专业分类
                wfentity.argument3 = bEntity.HIDTYPE; //隐患类别
                wfentity.argument4 = bEntity.HIDBMID; //所属部门
                wfentity.startflow = baseEntity.WORKSTREAM;
                wfentity.rankid = baseEntity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.HIDDEPART; //对应电厂id
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else
                {
                    wfentity.mark = "厂级隐患排查";
                }
                //退回 
                if (ISBACK == "1")
                {
                    //历史记录
                    var changeitem = htchangeinfobll.GetHistoryList(entity.HIDCODE).ToList();
                    //如果未整改可以退回
                    if (changeitem.Count() == 0)
                    {
                        wfentity.submittype = "退回";
                    }
                    else
                    {
                        return Error("整改过后的隐患无法再次退回!");
                    }
                }
                else //正常提交到验收流程
                {
                    wfentity.submittype = "提交";
                }


                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
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
                        if (!string.IsNullOrEmpty(participant) && !string.IsNullOrEmpty(wfFlag))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                            }

                            string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                            if (tagName == "制定整改计划" && wfentity.submittype == "退回")
                            {
                                UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                                entity.CHARGEPERSONNAME  = userentity.RealName;
                                entity.CHARGEPERSON = userentity.Account;
                                entity.CHARGEDEPTID = userentity.DepartmentId;
                                entity.CHARGEDEPTNAME = userentity.DeptName;
                                entity.ISAPPOINT = "0";
                                htchangeinfobll.SaveForm(entity.ID, entity);
                            }
                        }
                    }
                    else
                    {
                        //获取
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                        #region 当前还处于隐患整改阶段
                        if (tagName == "隐患整改")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            entity.CHANGEPERSONNAME = userentity.RealName;
                            entity.CHANGEPERSON = userentity.UserId;
                            entity.CHANGEDUTYDEPARTNAME = userentity.DeptName;
                            entity.CHANGEDUTYDEPARTID = userentity.DepartmentId;
                            entity.CHANGEDUTYDEPARTCODE = userentity.DepartmentCode;
                            entity.CHANGEDUTYTEL = userentity.Telephone;
                            htchangeinfobll.SaveForm(entity.ID, entity);
                        }
                        #endregion
                    }
                }
                //非自动处理的流程
                else if (result.code == WfCode.NoAutoHandle)
                {
                    bool isupdate = false;//是否更改流程状态
                    //退回操作  单独处理
                    if (ISBACK == "1")
                    {
                        DataTable dt = htworkflowbll.GetBackFlowObjectByKey(keyValue);

                        if (dt.Rows.Count > 0)
                        {
                            wfFlag = dt.Rows[0]["wfflag"].ToString(); //流程走向

                            participant = dt.Rows[0]["participant"].ToString();  //指向人

                            isupdate = dt.Rows[0]["isupdate"].ToString() == "1"; //是否更改流程状态
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
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                            }

                            string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                            if (tagName == "制定整改计划" && wfentity.submittype == "退回")
                            {
                                UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                                entity.CHARGEPERSONNAME = userentity.RealName;
                                entity.CHARGEPERSON = userentity.Account;
                                entity.CHARGEDEPTID = userentity.DepartmentId;
                                entity.CHARGEDEPTNAME = userentity.DeptName;
                                entity.ISAPPOINT = "0";
                                htchangeinfobll.SaveForm(entity.ID, entity);
                            }

                            result.message = "处理成功";
                            result.code = WfCode.Sucess;
                        }
                    }
                    else
                    {
                        //获取
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                        #region 当前还处于隐患整改阶段
                        if (tagName == "隐患整改")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            entity.CHANGEPERSONNAME = userentity.RealName;
                            entity.CHANGEPERSON = userentity.UserId;
                            entity.CHANGEDUTYDEPARTNAME = userentity.DeptName;
                            entity.CHANGEDUTYDEPARTID = userentity.DepartmentId;
                            entity.CHANGEDUTYDEPARTCODE = userentity.DepartmentCode;
                            entity.CHANGEDUTYTEL = userentity.Telephone;
                            htchangeinfobll.SaveForm(entity.ID, entity);
                        }
                        #endregion
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
    }
}
