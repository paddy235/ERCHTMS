using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：外包电厂提交资料说明表
    /// </summary>
    public class OutcommitfileController : MvcControllerBase
    {
        private OutcommitfileBLL outcommitfilebll = new OutcommitfileBLL();

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
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.createuserid,t.createuserdeptcode,t.createuserorgcode,
                                        t.createdate,t.createusername,t.fileexplain,t.remark,t.title,
                                        t.issend,t.sendtime,t.orgname";
                pagination.p_tablename = "epg_outcommitfile t";
                pagination.conditionJson = "  1=1 ";
                if (currUser.IsSystem)
                {
                  
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(currUser, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and  t.createuserid='" + currUser.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and t.createuserdeptcode='" + currUser.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and t.createuserdeptcode like '" + currUser.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and t.createuserdeptcode like '" + currUser.OrganizeCode + "%'";
                                break;
                            case "5":
                                pagination.conditionJson += string.Format(" and t.createuserdeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", currUser.NewDeptCode);
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                var data = outcommitfilebll.GetPageList(pagination, queryJson);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取实体和附件信息 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormAndFile(string keyValue)
        {
            var data = outcommitfilebll.GetEntity(keyValue);
            var fileList = new FileInfoBLL().GetFileList(data.ID);
            var jsonData = new
            {
                data = data,
                fileList = fileList,
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = outcommitfilebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 根据机构Code查询本机构是否已经添加
        /// </summary>
        /// <param name="orgCode">机构Code</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIsExist(string orgCode) {
            var data = outcommitfilebll.GetIsExist(orgCode);
            return ToJsonResult(data);
        }
       /// <summary>
       /// 根据机构Code查询是否有资料说明
       /// </summary>
       /// <param name="orgCode">机构Code</param>
       /// <returns></returns>
      [HttpPost]
        public ActionResult GetEntityByOrgCode(string orgCode)
        {
            var data = outcommitfilebll.GetEntityByOrgCode(orgCode);
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
            outcommitfilebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, OutcommitfileEntity entity)
        {
            outcommitfilebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
