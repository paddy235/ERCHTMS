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

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
{
    /// <summary>
    /// 描 述：整改延期对象
    /// </summary>
    public class HTExtensionController : MvcControllerBase
    {
        private HTExtensionBLL htextensionbll = new HTExtensionBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
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
        #endregion


        /// <summary>
        /// 获取当前用户是否为回复人身份
        /// </summary>
        /// <returns></returns>
       [HttpGet]
        public ActionResult QueryReplayerRole()
        {
            //部门负责人角色编码
            string roleCode = dataitemdetailbll.GetItemValue("HidPrincipalSetting");

            Operator curUser = OperatorProvider.Provider.Current();

            IList<UserEntity> ulist = userbll.GetUserListByRole(curUser.DeptCode, roleCode, curUser.OrganizeId).ToList();

            //返回的记录数,大于0，标识当前用户拥有安全管理员身份，反之则无
            string uModel = ulist.Where(p => p.UserId == curUser.UserId).Count().ToString();

            return Content(uModel);
        }

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
            string postPoneResult = Request.Form["POSTPONERESULT"].ToString();  //审批结果

            string actionDo = Request.Form["ACTIONDO"].ToString(); //审批动作

            string postponedeptname = Request.Form["POSTPONEDEPTNAME"]!=null ? Request.Form["POSTPONEDEPTNAME"].ToString():""; //指定部门名称

            string postponedept = Request.Form["POSTPONEDEPT"] != null ? Request.Form["POSTPONEDEPT"].ToString() : ""; //指定部门Code 

            string result = "";

            bool isUpdateDate = false;

            string handletype = "";

            //部门负责人 同意
            if (postPoneResult == "1" && actionDo == "approval")
            {
                handletype = "1";

                result = "2";
            }
            //指定部门人员 同意
            else if (postPoneResult == "1" && actionDo == "replay")
            {
                handletype = "2";

                result = "3";

                isUpdateDate = true;
            }
            else  //退回操作 
            {
                if (actionDo == "approval")
                {
                    handletype = "3";
                }
                if (actionDo == "replay")
                {
                    handletype = "4";
                }

                //延期申请失败
                result = "4";
            }

            HTChangeInfoEntity cEntity = htchangeinfobll.GetEntityByHidCode(entity.HIDCODE);
            cEntity.APPLICATIONSTATUS = result;
            if (!string.IsNullOrEmpty(postponedept)) 
            {
                postponedept = "," + postponedept + ",";
            }
            cEntity.POSTPONEDEPT = postponedept;
            cEntity.POSTPONEDEPTNAME = postponedeptname;
            //如果安环部、生技部审批通过，则更改整改截至时间、验收时间，增加相应的整改天数
            if (isUpdateDate)
            {
                //重新赋值整改截至时间
                cEntity.CHANGEDEADINE = cEntity.CHANGEDEADINE.Value.AddDays(cEntity.POSTPONEDAYS);

                //更新验收时间
                HTAcceptInfoEntity aEntity = htacceptinfobll.GetEntityByHidCode(entity.HIDCODE);
                aEntity.ACCEPTDATE = aEntity.ACCEPTDATE.Value.AddDays(cEntity.POSTPONEDAYS);
                htacceptinfobll.SaveForm(aEntity.ID, aEntity);

                //申请合格标识
                entity.HANDLESIGN = "1";
            }
            //更新整改信息
            htchangeinfobll.SaveForm(cEntity.ID, cEntity);

            //添加审批记录
            entity.HANDLETYPE = handletype;
            entity.POSTPONERESULT = postPoneResult;
            htextensionbll.SaveForm("", entity);

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
