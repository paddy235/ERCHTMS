using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：职位管理
    /// </summary>
    public class JobController : MvcControllerBase
    {
        private JobBLL jobBLL = new JobBLL();
        private JobCache jobCache = new JobCache();

        #region 视图功能
        /// <summary>
        /// 职位管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 职位表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 职务导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Import()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 职位列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            //var watch = CommonHelper.TimerStart();
            //var data = jobBLL.GetPageList(pagination, queryJson);
            //var JsonData = new
            //{
            //    rows = data,
            //    total = pagination.total,
            //    page = pagination.page,
            //    records = pagination.records,
            //    costtime = CommonHelper.TimerEnd(watch)
            //};
            //return Content(JsonData.ToJson());

            //pagination.p_kid = "roleid";
            //pagination.p_fields = "t.EnCode,t.FullName,t.OrganizeId,t.CreateDate,t.EnabledMark,t.Description,t.Nature,t.RoleNames,case when o.FullName is null then d.fullname else o.fullname end  orgname,t.deptname,t.deptid";
            //pagination.p_tablename = "BASE_ROLE t left join base_organize o on t.organizeid=o.organizeid left join BASE_DEPARTMENT d on t.OrganizeId=d.departmentid";
            //pagination.conditionJson = "t.category=3";
            //var watch = CommonHelper.TimerStart();
            //var data = jobBLL.GetList(pagination, queryJson);
            //var JsonData = new
            //{
            //    rows = data,
            //    total = pagination.total,
            //    page = pagination.page,
            //    records = pagination.records,
            //    costtime = CommonHelper.TimerEnd(watch)
            //};
            //return Content(JsonData.ToJson());


            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.RoleId";
            pagination.p_fields = "t.EnCode,t.FullName,t.OrganizeId,t.CreateDate,t.EnabledMark,t.Description,t.Nature,t.RoleNames,case when o.FullName is null then d.fullname else o.fullname end  orgname,t.deptname,t.deptid";
            pagination.p_tablename = "BASE_ROLE t left join base_organize o on t.organizeid=o.organizeid left join BASE_DEPARTMENT d on t.OrganizeId=d.departmentid";
            pagination.conditionJson = "t.Category=3";
            Operator user = OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                AuthorizeBLL authBll = new AuthorizeBLL();
                string authType = authBll.GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (string.IsNullOrEmpty(authType))
                {
                    pagination.conditionJson = "0=1";
                }
                else
                {
                    if (int.Parse(authType) < 4)
                    {
                        pagination.conditionJson = "0=1";
                    }
                    if (authType == "4")
                    {
                        pagination.conditionJson += " and t.OrganizeId='" + user.OrganizeId + "'";
                    }
                }
            }
            var data = jobBLL.GetList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 职位列表
        /// </summary>
        /// <param name="organizeId">机构Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string organizeId)
        {
            var data = jobCache.GetList(organizeId);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 职位列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <param name="isOrg">仅仅选择了公司</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public string NewGetListJson(string organizeId)
        {
            DepartmentBLL debll = new DepartmentBLL();
            DepartmentEntity dept = debll.GetEntity(organizeId);
            IEnumerable<RoleEntity> data;
            if (dept != null)
            {
                data = jobCache.GetList(dept.OrganizeId, dept.Nature).OrderBy(x => x.SortCode);
            }
            else
            {
                data = jobCache.GetList(organizeId, "").OrderBy(x => x.SortCode);
            }

            StringBuilder sb = new StringBuilder();
            foreach (RoleEntity dr in data)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dr.RoleId, dr.FullName);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 职位实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = jobBLL.GetEntity(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 职位编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistEnCode(string EnCode, string keyValue)
        {
            bool IsOk = jobBLL.ExistEnCode(EnCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 职位名称不能重复
        /// </summary>
        /// <param name="FullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistFullName(string FullName, string keyValue)
        {
            bool IsOk = jobBLL.ExistFullName(FullName, keyValue);
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(6, "删除职位信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            jobBLL.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存职位表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="jobEntity">职位实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)职位信息")]
        public ActionResult SaveForm(string keyValue, RoleEntity jobEntity)
        {
            jobBLL.SaveForm(keyValue, jobEntity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 导入职务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportPost(string OrganizeId)
        {
            string orgId = OrganizeId;//上级公司
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                var list = new DepartmentBLL().GetList(OrganizeId);
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //职务名称
                    string fullName = dt.Rows[i][0].ToString();

                    ////验证职务重复
                    //if (!postBLL.ExistPostJugement(fullName))
                    //{
                    //    falseMessage += "</br>" + "第" + (i + 2) + "行值存在重复,未能导入.";
                    //    error++;
                    //    continue;
                    //}
                    //职务编码
                    string encode = dt.Rows[i][1].ToString();
                    //层级
                    string nature = dt.Rows[i][2].ToString();
                    if (string.IsNullOrEmpty(nature) || string.IsNullOrEmpty(fullName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(encode))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行职务编码为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (!jobBLL.ExistEnCode(encode, ""))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行职务编码已经存在,未能导入.";
                        error++;
                        continue;
                    }
                    //职务描述
                    string description = dt.Rows[i][3].ToString();

                    string deptname = "";


                    RoleEntity role = new RoleEntity();
                    role.FullName = fullName;
                    role.Description = description;
                    role.SortCode = order;
                    role.RoleId = Guid.NewGuid().ToString();
                    role.OrganizeId = orgId;
                    role.Nature = nature;
                    role.EnabledMark = 1;
                    role.EnCode = encode;
                    role.Category = 3;
                    //if (!string.IsNullOrWhiteSpace(deptname))
                    //{
                    //    var dept = list.Where(x => x.FullName == deptname).ToList();
                    //    if (dept.Count > 0)
                    //    {
                    //        var entity = dept.FirstOrDefault();
                    //        role.DeptId = entity.DepartmentId;
                    //        role.DeptName = entity.FullName;
                    //    }
                    //}
                    try
                    {
                        jobBLL.SaveForm("", role);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
    }
}
