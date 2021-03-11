using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections;
using System;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
{
    /// <summary>
    /// 描 述：隐患评估信息表
    /// </summary>
    public class HTApprovalController : MvcControllerBase
    {
        private HTApprovalBLL htapprovebll = new HTApprovalBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //隐患流程
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //隐患整改
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收
        private DepartmentBLL departmentbll = new DepartmentBLL(); //部门操作对象
        private UserBLL userbll = new UserBLL(); //人员操作对象
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
        public ActionResult GetListJson(string hideCode)
        {
            var data = htapprovebll.GetList(hideCode);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htapprovebll.GetHistoryList(keyCode);
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
            var data = htapprovebll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "删除隐患评估信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            htapprovebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }


        /// <summary>
        /// 是否指定部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsAssignDepartment()
        {
            Operator curUser = OperatorProvider.Provider.Current();

            bool isSuccessful = false;

            string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

            string[] pstr = HidApproval.Split('#');  //分隔机构组

            foreach (string strArgs in pstr)
            {
                string[] str = strArgs.Split('|');
                //指定部门
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                {
                    //获取指定部门的所有人员
                    isSuccessful = true;

                    break;
                }
            }

            if (isSuccessful)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 保存提交
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string submitUser, string isUpSubmit, HTApprovalEntity entity, HTChangeInfoEntity chEntity, HTAcceptInfoEntity aEntity)
        {

            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //流程标识

            bool isParentDept = false;

            int upMark = 0;

            //部门级用户
            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidDepartment")).Rows.Count > 0)
            {
                upMark = 1;

                isParentDept = true; //上报到机构单位
            }
            //承包商用户，上报到发包部门
            else if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidContractor")).Rows.Count > 0)
            {
                upMark = 2;

                isParentDept = true;  //上级部门为发包部门
            }
            else
            {
                isParentDept = departmentbll.IsExistSuperior(curUser.DeptId);  //判断是否存在上级部门
            }
           
            string participant = string.Empty;  //获取流程下一节点的参与人员

            //上报，且不存在上级部门，返回提示
            if (isUpSubmit == "1" && !isParentDept)
            {
                return Error("当前不存在上级部门，无法进行上报操作!");
            }
            else if (isUpSubmit == "1" && isParentDept)  //上报，且存在上级部门
            {
                //上报分几种情况  分包商，承包商，公司级人员，各部门人员
                IList<UserEntity> ulist = new List<UserEntity>(); 

                //部门级用户，上报到公司机构
                if (upMark==1) 
                {
                    string args = dataitemdetailbll.GetItemValue("HidOrganize") + "," + dataitemdetailbll.GetItemValue("HidApprovalSetting");
                    //获取机构
                    ulist = userbll.GetUserListByDeptCode(null, args, true, curUser.OrganizeId);
                }
                //承包商用户，上报到发包部门
                else if (upMark==2)
                {
                    string sendDeptID = departmentbll.GetEntity(curUser.DeptId).SendDeptID;

                    var SendDeptCode = "'" + departmentbll.GetEntity(sendDeptID).EnCode + "'";

                    ulist = userbll.GetUserListByDeptCode(SendDeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), false, curUser.OrganizeId);
                }
                else 
                {
                    //获取上级部门的安全管理员 ,并赋值 participant
                    ulist = userbll.GetParentUserByCurrent(curUser.UserId, dataitemdetailbll.GetItemValue("HidApprovalSetting"));
                }

                foreach (UserEntity u in ulist)
                {
                    participant += u.Account + ",";
                }

                if (!string.IsNullOrEmpty(participant))
                {
                    participant = participant.Substring(0, participant.Length - 1);  //上级部门安全管理员

                    htworkflowbll.SubmitWorkFlowNoChangeStatus(keyValue, participant, curUser.UserId);

                    return Success("操作成功!");
                }
                else
                {
                    return Error("当前上级部门无安全管理员,如需上报,请联系系统管理员进行配置!");
                }

            }
            else  //不上报，评估通过需要提交整改，评估不通过退回到登记
            {
                //评估ID
                string APPROVALID = Request.Form["APPROVALID"] != null ? Request.Form["APPROVALID"].ToString() : "";

                string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : "";

                string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : "";

                APPROVALID = APPROVALID == "&nbsp;" ? "" : APPROVALID;

                //隐患曝光
                string EXPOSURESTATE = Request.Form["EXPOSURESTATE"] != null ? Request.Form["EXPOSURESTATE"].ToString() : "";

                /****判断当前人是否评估通过*****/

                //评估通过，则直接进行整改
                if (entity.APPROVALRESULT == "1")
                {
                    wfFlag = "2";  //整改标识
                }
                else  //评估不通过，退回到登记 
                {
                    wfFlag = "3";   //登记标识
                }
                participant = submitUser; //整改人 Or 登记人

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    //保存隐患曝光状态
                    HTBaseInfoEntity baseentity = new HTBaseInfoBLL().GetEntity(keyValue);
                    baseentity.EXPOSURESTATE = EXPOSURESTATE;
                    baseentity.EXPOSUREDATETIME = DateTime.Now;
                    htbaseinfobll.SaveForm(keyValue, baseentity);

                    //存在回退原因后，需要清空原因提交
                    HTChangeInfoEntity cEntity = htchangeinfobll.GetEntityByCode(entity.HIDCODE);
                    if (!string.IsNullOrEmpty(cEntity.BACKREASON))
                    {
                        cEntity.BACKREASON = null;
                        htchangeinfobll.SaveForm(cEntity.ID, cEntity);
                    }

                    //隐患整改
                    if (!string.IsNullOrEmpty(CHANGEID))
                    {
                        var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                        chEntity.AUTOID = tempEntity.AUTOID;
                        chEntity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                        chEntity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                        chEntity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                        chEntity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
                    }
                    htchangeinfobll.SaveForm(CHANGEID, chEntity);

                    //隐患验收
                    if (!string.IsNullOrEmpty(ACCEPTID))
                    {
                        var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                        aEntity.AUTOID = tempEntity.AUTOID;
                    }
                    htacceptinfobll.SaveForm(ACCEPTID, aEntity);

                    //保存隐患评估信息
                    htapprovebll.SaveForm(APPROVALID, entity);

                    htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态

                    return Success("操作成功!");
                }
                else
                {
                    return Error("当前用户无评估权限!");
                }

            }
        }
        #endregion


    }
}
