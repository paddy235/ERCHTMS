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

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
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
        public ActionResult SaveSettingForm(string keyValue, HTChangeInfoEntity entity)
        {
            Operator curUser = new OperatorProvider().Current();
            var cEntity = htchangeinfobll.GetEntityByCode(keyValue); //根据HidCode获取整改对象
            string  postponereason = Request.Form["POSTPONEREASON"] != null ? Request.Form["POSTPONEREASON"].ToString() : "";
            string hidid = Request.Form["HIDID"] != null ? Request.Form["HIDID"].ToString() : "";
            cEntity.POSTPONEDAYS = entity.POSTPONEDAYS;
            if (!string.IsNullOrEmpty(entity.POSTPONEDEPT))
            {
                entity.POSTPONEDEPT = "," + entity.POSTPONEDEPT + ",";
            }
            cEntity.POSTPONEDEPT = entity.POSTPONEDEPT;  //批复部门Code
            cEntity.POSTPONEDEPTNAME = entity.POSTPONEDEPTNAME;  //批复部门名称
            cEntity.APPLICATIONSTATUS = entity.APPLICATIONSTATUS;
            htchangeinfobll.SaveForm(cEntity.ID, cEntity); //更新延期设置

            //保存申请记录
            HTExtensionEntity exentity = new HTExtensionEntity();
            exentity.HIDCODE = cEntity.HIDCODE;
            exentity.HIDID = hidid;
            exentity.HANDLEDATE = DateTime.Now;
            exentity.POSTPONEDAYS = entity.POSTPONEDAYS.ToString();
            exentity.HANDLEUSERID = curUser.UserId;
            exentity.HANDLEUSERNAME = curUser.UserName;
            exentity.HANDLEDEPTCODE = curUser.DeptCode;
            exentity.HANDLEDEPTNAME = curUser.DeptName;
            exentity.HANDLETYPE = "0";  //申请类型
            exentity.HANDLEID = DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
            exentity.POSTPONEREASON = postponereason;
            htextensionbll.SaveForm("", exentity);

            return Success("操作成功。");
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

            if(!string.IsNullOrEmpty(CHANGEID))
            {
                var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                entity.AUTOID = tempEntity.AUTOID;
                entity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                entity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                entity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                entity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
            }

            htchangeinfobll.SaveForm(CHANGEID, entity);

            //退回
            if (ISBACK == "1")
            {

                IList<UserEntity> ulist = new List<UserEntity>();

                string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

                string[] pstr = HidApproval.Split('#');  //分隔机构组


                var createuserid = new HTBaseInfoBLL().GetEntity(keyValue).CREATEUSERID;

                var createUser = userbll.GetEntity(createuserid);

                foreach (string strArgs in pstr)
                {
                    string[] str = strArgs.Split('|');

                    //当前机构相同，且为本部门安全管理员验证
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                    {
                        //登记人为
                        DataTable rDt = userbll.HaveRoleListByKey(createuserid, dataitemdetailbll.GetItemValue("HidApprovalSetting"));
                        //安全管理员登记的
                        if (rDt.Rows.Count > 0)
                        {
                            //如果登记人是安全管理员 3
                            participant = rDt.Rows[0]["account"].ToString();

                            wfFlag = "3";
                        }
                        else
                        {
                            //登记人非安全管理员 2 
                            DataTable dt = htapprovalbll.GetDataTableByHidCode(entity.HIDCODE);
                            if (dt.Rows.Count > 0)
                            {
                                //获取核查人 
                                participant = dt.Rows[0]["account"].ToString(); //整改人 Or 登记人

                                wfFlag = "2";
                            }
                        }

                        break;
                    }
                    //指定部门情况
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                    {
                        //获取指定部门的所有人员   //取登记人 当前人是指定部门,则退回登记,反之则到核查

                        string curDeptCode = "'" + createUser.DepartmentCode + "'";

                        //当前人是否包含在指定部门当中
                        if (str[2].ToString().Contains(curDeptCode))
                        {
                            //取登记
                            participant = userbll.GetEntity(bEntity.CREATEUSERID).Account;

                            wfFlag = "3";
                        }
                        else 
                        {
                            DataTable dt = htapprovalbll.GetDataTableByHidCode(entity.HIDCODE);
                            if (dt.Rows.Count > 0) 
                            {
                                //获取核查人 
                                participant = dt.Rows[0]["account"].ToString();

                                wfFlag = "2";
                            }
                        }
                        break;
                    }
                }

            }
            else //正常提交到验收流程
            {
                //验收人员
                UserEntity auser = userbll.GetEntity(aEntity.ACCEPTPERSON);

                participant = auser.Account;

                wfFlag = "1";
            }

            if (!string.IsNullOrEmpty(participant) && !string.IsNullOrEmpty(wfFlag))
            {
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                }
            }
            else 
            {
                return Error("操作失败!");
            }
            return Success("操作成功!");
        }
        #endregion
    }
}
