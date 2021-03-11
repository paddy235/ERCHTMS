using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using System.Web;
using System;
using System.Data;
using BSFramework.Util.Offices;
using System.IO;
using System.Collections;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：岗位管理
    /// </summary>
    public class PostController : MvcControllerBase
    {
        private PostCache postCache = new PostCache();
        private PostBLL postBLL = new PostBLL();
        private RoleBLL rollBLL = new RoleBLL();

        #region 视图功能
        /// <summary>
        /// 岗位管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 岗位表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 导入岗位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// 选择岗位
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取单个角色信息
        /// </summary>
        /// <param name="code">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetListByCode(string code)
        {
            RoleEntity re = rollBLL.GetList().Where(a => a.EnCode == code).FirstOrDefault();
            return Content(re.ToJson());
        }
        /// <summary>
        /// 根据id获取多个角色信息
        /// </summary>
        /// <param name="id">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetRoleById(string id)
        {
            RoleEntity re = postBLL.GetList().Where(a => a.RoleId == id).FirstOrDefault();
            return Content(re.ToJson());
        }
        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.RoleId";
            pagination.p_fields = "t.EnCode,t.FullName,t.OrganizeId,t.CreateDate,t.EnabledMark,t.Description,t.Nature,t.RoleNames,case when o.FullName is null then d.fullname else o.fullname end  orgname,t.deptname,t.deptid";
            pagination.p_tablename = "BASE_ROLE t left join base_organize o on t.organizeid=o.organizeid left join BASE_DEPARTMENT d on t.OrganizeId=d.departmentid";
            pagination.conditionJson = "t.Category=2";
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
            var data = postBLL.GetList(pagination, queryJson);
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
        /// 选择岗位页面使用
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPostListJsonForSelect(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.RoleId";
            pagination.p_fields = "t.EnCode,t.FullName,t.OrganizeId,t.CreateDate,t.EnabledMark,t.Description,t.Nature,t.RoleNames,case when o.FullName is null then d.fullname else o.fullname end  orgname";
            pagination.p_tablename = "BASE_ROLE t left join base_organize o on t.organizeid=o.organizeid left join BASE_DEPARTMENT d on t.OrganizeId=d.departmentid and t.deptid =d.departmentid";
            pagination.conditionJson = "t.Category=2";
            Operator user = OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and t.OrganizeId='" + user.OrganizeId + "'";
            }
            var data = postBLL.GetList(pagination, queryJson);
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
        /// 岗位列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <param name="isOrg">仅仅选择了公司</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string organizeId, string isOrg)
        {
            IList<RoleEntity> list = new List<RoleEntity>();
            list = postCache.GetList(organizeId, isOrg).OrderBy(x => x.SortCode).ToList();
            return Content(list.ToJson());
        }

        /// <summary>
        /// 岗位列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <param name="isOrg">仅仅选择了公司</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetRealListJson(string departmentid)
        {
            IList<RoleEntity> list = new List<RoleEntity>();
            list = postCache.GetRealList(departmentid).OrderBy(x => x.SortCode).ToList();
            return Content(list.ToJson());
        }

        /// <summary>
        /// 根据部门id获取岗位列表
        /// </summary>
        /// <param name="deptid">部门id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPostJson(string deptid)
        {
            var dept = new DepartmentBLL().GetEntity(deptid);
            string organizeId = "";
            if (dept != null)
            {
                organizeId = dept.OrganizeId;
            }

            var data = postCache.GetList(organizeId, deptid).OrderBy(x => x.SortCode);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 根据机构编码和层级获取岗位信息
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <param name="isOrg">true：查询厂级下岗位，其他获取部门ID为isOrg的层级下的岗位</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPostListJson(string orgCode, string isOrg)
        {
            var dept = new DepartmentBLL().GetEntityByCode(orgCode);
            if (dept != null)
            {
                isOrg = dept.EnCode;
                var entity = new DepartmentBLL().GetEntity(dept.OrganizeId);
                if (entity != null)
                {
                    orgCode = entity.EnCode;
                }
            }
            else
            {
                isOrg = "true";
            }
            DepartmentCache departBll = new DepartmentCache();
            DepartmentEntity departList = departBll.GetList().Where(t => t.EnCode == orgCode).FirstOrDefault();
            var data = postCache.GetList(departList.OrganizeId, isOrg).OrderBy(x => x.SortCode);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 岗位实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = postBLL.GetEntity(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 根据岗位名称获取岗位实体
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="PostName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPostEntity(string orgid, string PostName)
        {
            var data = postBLL.GetList().Where(t => t.OrganizeId == orgid && t.FullName == PostName);
            return Content(data.ToJson());
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 岗位编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistEnCode(string EnCode, string keyValue)
        {
            bool IsOk = postBLL.ExistEnCode(EnCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 岗位名称不能重复
        /// </summary>
        /// <param name="FullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistFullName(string FullName, string keyValue)
        {
            bool IsOk = postBLL.ExistFullName(FullName, keyValue);
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(6, "删除岗位信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            postBLL.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存岗位表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="postEntity">岗位实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)岗位信息")]
        public ActionResult SaveForm(string keyValue, RoleEntity postEntity)
        {
            postBLL.SaveForm(keyValue, postEntity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 导入岗位
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
                var deptBll = new DepartmentBLL();
                int k = 1;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //岗位名称
                    string fullName = dt.Rows[i][0].ToString();

                    ////验证岗位重复
                    //if (!postBLL.ExistPostJugement(fullName))
                    //{
                    //    falseMessage += "</br>" + "第" + (i + 2) + "行值存在重复,未能导入.";
                    //    error++;
                    //    continue;
                    //}
                    //岗位编码
                    string encode = dt.Rows[i][1].ToString();
                    //层级
                    string nature = dt.Rows[i][2].ToString();
                    if (string.IsNullOrEmpty(nature) || string.IsNullOrEmpty(fullName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    //if (string.IsNullOrEmpty(encode))
                    //{
                    //    falseMessage += "</br>" + "第" + (i + 2) + "行岗位编码为空,未能导入.";
                    //    error++;
                    //    continue;
                    //}
                    //if (!postBLL.ExistEnCode(encode, ""))
                    //{
                    //    falseMessage += "</br>" + "第" + (i + 2) + "行岗位编码已经存在,未能导入.";
                    //    error++;
                    //    continue;
                    //}
                    //岗位描述
                    string description = dt.Rows[i][7].ToString();
                    //是否负责人所在岗位
                    string isFZR = dt.Rows[i][3].ToString();
                    //是否公司领导所在岗位
                    string isGSLD = dt.Rows[i][4].ToString();
                    //是否安全管理员所在岗位
                    string isGLY = dt.Rows[i][5].ToString();
                    //是否专工
                    string isZG = dt.Rows[i][6].ToString();

                    string deptname = dt.Rows[i][8].ToString();

                    RoleEntity role = new RoleEntity();
                    role.FullName = fullName;
                    role.Description = description;
                    role.SortCode = order;
                    role.RoleId = Guid.NewGuid().ToString();
                    role.OrganizeId = orgId;
                    role.Nature = nature;
                    role.EnabledMark = 1;
                    role.EnCode = encode;
                    role.Category = 2;
                    if (!string.IsNullOrWhiteSpace(deptname))
                    {
                        var arrDepts = deptname.Split(';');
                        if (arrDepts.Length == 1)
                        {
                            string name=arrDepts[0];
                            if (name.Contains("/"))
                            {
                                var arr = name.Split('/');
                                if (arr.Length > 1)
                                {
                                    DataTable dtDept = deptBll.GetDataTable(string.Format("select DepartmentId from BASE_DEPARTMENT where fullname='{0}' and organizeid='{1}'", arr[arr.Length - 2], OrganizeId));
                                    if (dtDept.Rows.Count > 0)
                                    {
                                        var listDepts = list.Where(x => x.FullName == arr[arr.Length - 1] && x.ParentId == dtDept.Rows[0][0].ToString()).ToList();
                                        if (listDepts.Count > 0)
                                        {
                                            var entity = listDepts.FirstOrDefault();
                                            role.DeptId = entity.DepartmentId;
                                            role.DeptName = entity.FullName;
                                        }
                                        else
                                        {
                                            falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在于系统,未能导入.";
                                            error++;
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        falseMessage += "</br>" + "第" + (i + 2) + "行上级部门不存在于系统,未能导入.";
                                        error++;
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                var dept = list.Where(x => x.FullName == name).ToList();
                                if (dept.Count > 0)
                                {
                                    var entity = dept.FirstOrDefault();
                                    role.DeptId = entity.DepartmentId;
                                    role.DeptName = entity.FullName;
                                }
                                else
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在于系统,未能导入.";
                                    error++;
                                    continue;
                                }

                            }
                            role.EnCode = DateTime.Now.ToString("yyyyMMddHHmmss") + k;
                            //var dept = list.Where(x => x.FullName == deptname).ToList();
                            //if (dept.Count > 0)
                            //{
                            //    var entity = dept.FirstOrDefault();
                            //    role.DeptId = entity.DepartmentId;
                            //    role.DeptName = entity.FullName;
                            //}
                            //层级角色
                            RoleEntity re = null;
                            DepartmentEntity org = new DepartmentBLL().GetEntity(orgId);
                            if (!nature.Contains("厂级"))
                            {

                                if (org.Nature == "省级")
                                {
                                    re = rollBLL.GetList().Where(a => a.FullName == "省级用户").FirstOrDefault();
                                }
                                else
                                {
                                    re = rollBLL.GetList().Where(a => a.FullName.Contains(nature+"级")).FirstOrDefault();
                                }

                            }
                            else
                            {
                                re = rollBLL.GetList().Where(a => a.FullName.Contains("公司级")).FirstOrDefault();
                            }
                            //普通用户
                            RoleEntity reuser = rollBLL.GetList().Where(a => a.FullName == "普通用户").FirstOrDefault();
                            //根据层级设置角色关系
                            if (!(re == null || reuser == null))
                            {

                                if (org.Nature == "省级")
                                {
                                    role.RoleIds = re.RoleId + ",";
                                    role.RoleNames = re.FullName + ",";
                                }
                                else
                                {
                                    role.RoleIds = re.RoleId + "," + reuser.RoleId + ",";
                                    role.RoleNames = re.FullName + "," + reuser.FullName + ",";
                                }
                            }
                            //赋值角色
                            if (isFZR == "是")
                            {
                                RoleEntity reisFZR = null;
                                if (org.Nature == "省级")
                                {
                                    reisFZR = rollBLL.GetList().Where(a => a.EnCode == "300013").FirstOrDefault();
                                }
                                else
                                {
                                    reisFZR = rollBLL.GetList().Where(a => a.FullName == "负责人").FirstOrDefault();

                                }
                                if (reisFZR != null)
                                {
                                    role.RoleIds += reisFZR.RoleId + ",";
                                    role.RoleNames += reisFZR.FullName + ",";
                                }

                            }
                            if (isGSLD == "是")
                            {
                                RoleEntity reisGSLD = null;
                                if (org.Nature == "省级")
                                {
                                    reisGSLD = rollBLL.GetList().Where(a => a.EnCode == "300015").FirstOrDefault();
                                }
                                else
                                {
                                    reisGSLD = rollBLL.GetList().Where(a => a.FullName == "公司领导").FirstOrDefault();

                                }
                                if (reisGSLD != null)
                                {
                                    role.RoleIds += reisGSLD.RoleId + ",";
                                    role.RoleNames += reisGSLD.FullName + ",";
                                }

                            }
                            if (isGLY == "是")
                            {
                                RoleEntity reisGLY = null;
                                if (org.Nature == "省级")
                                {
                                    reisGLY = rollBLL.GetList().Where(a => a.EnCode == "300012").FirstOrDefault();
                                }
                                else
                                {
                                    reisGLY = rollBLL.GetList().Where(a => a.FullName == "安全管理员").FirstOrDefault();

                                }
                                if (reisGLY != null)
                                {
                                    role.RoleIds += reisGLY.RoleId + ",";
                                    role.RoleNames += reisGLY.FullName + ",";
                                }

                            }
                            if (isZG == "是")
                            {
                                RoleEntity reisGLY = null;
                                if (org.Nature == "省级")
                                {
                                    reisGLY = rollBLL.GetList().Where(a => a.EnCode == "300014").FirstOrDefault();
                                }
                                else
                                {
                                    reisGLY = rollBLL.GetList().Where(a => a.FullName == "专工").FirstOrDefault();

                                }
                                if (reisGLY != null)
                                {
                                    role.RoleIds += reisGLY.RoleId + ",";
                                    role.RoleNames += reisGLY.FullName + ",";
                                }

                            }
                            role.RoleIds = role.RoleIds.TrimEnd(',');
                            role.RoleNames = role.RoleNames.TrimEnd(',');
                            try
                            {
                                postBLL.SaveForm("", role);
                            }
                            catch
                            {
                                error++;
                            }
                        }
                        else
                        {
                            foreach (string name in arrDepts)
                            {
                               
                                role = new RoleEntity();
                                role.FullName = fullName;
                                role.Description = description;
                                role.SortCode = order;
                                role.RoleId = Guid.NewGuid().ToString();
                                role.OrganizeId = orgId;
                                role.Nature = nature;
                                role.EnabledMark = 1;
                                role.EnCode = DateTime.Now.ToString("yyyyMMddHHmmss")+k;
                                role.Category = 2;
                                if (name.Contains("/"))
                                {
                                    var arr = name.Split('/');
                                    if (arr.Length > 1)
                                    {
                                        DataTable dtDept = deptBll.GetDataTable(string.Format("select DepartmentId from BASE_DEPARTMENT where fullname='{0}' and organizeid='{1}'", arr[arr.Length - 2], OrganizeId));
                                        if (dtDept.Rows.Count > 0)
                                        {
                                            var listDepts = list.Where(x => x.FullName == arr[arr.Length - 1] && x.ParentId == dtDept.Rows[0][0].ToString()).ToList();
                                            if (listDepts.Count > 0)
                                            {
                                                var entity = listDepts.FirstOrDefault();
                                                role.DeptId = entity.DepartmentId;
                                                role.DeptName = entity.FullName;
                                            }
                                            else
                                            {
                                                falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在于系统,未能导入.";
                                                error++;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            falseMessage += "</br>" + "第" + (i + 2) + "行上级部门不存在于系统,未能导入.";
                                            error++;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    var dept = list.Where(x => x.FullName == name).ToList();
                                    if (dept.Count > 0)
                                    {
                                        var entity = dept.FirstOrDefault();
                                        role.DeptId = entity.DepartmentId;
                                        role.DeptName = entity.FullName;
                                    }
                                    else
                                    {
                                        falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在于系统,未能导入.";
                                        error++;
                                        break;
                                    }

                                }
                                //层级角色
                                RoleEntity re = null;
                                DepartmentEntity org = new DepartmentBLL().GetEntity(orgId);
                                if (!nature.Contains("厂级"))
                                {

                                    if (org.Nature == "省级")
                                    {
                                        re = rollBLL.GetList().Where(a => a.FullName == "省级用户").FirstOrDefault();
                                    }
                                    else
                                    {
                                        re = rollBLL.GetList().Where(a => a.FullName.Contains(nature + "级")).FirstOrDefault();
                                    }

                                }
                                else
                                {
                                    re = rollBLL.GetList().Where(a => a.FullName.Contains("公司级")).FirstOrDefault();
                                }
                                //普通用户
                                RoleEntity reuser = rollBLL.GetList().Where(a => a.FullName == "普通用户").FirstOrDefault();
                                //根据层级设置角色关系
                                if (!(re == null || reuser == null))
                                {

                                    if (org.Nature == "省级")
                                    {
                                        role.RoleIds = re.RoleId + ",";
                                        role.RoleNames = re.FullName + ",";
                                    }
                                    else
                                    {
                                        role.RoleIds = re.RoleId + "," + reuser.RoleId + ",";
                                        role.RoleNames = re.FullName + "," + reuser.FullName + ",";
                                    }
                                }
                                //赋值角色
                                if (isFZR == "是")
                                {
                                    RoleEntity reisFZR = null;
                                    if (org.Nature == "省级")
                                    {
                                        reisFZR = rollBLL.GetList().Where(a => a.EnCode == "300013").FirstOrDefault();
                                    }
                                    else
                                    {
                                        reisFZR = rollBLL.GetList().Where(a => a.FullName == "负责人").FirstOrDefault();

                                    }
                                    if (reisFZR != null)
                                    {
                                        role.RoleIds += reisFZR.RoleId + ",";
                                        role.RoleNames += reisFZR.FullName + ",";
                                    }

                                }
                                if (isGSLD == "是")
                                {
                                    RoleEntity reisGSLD = null;
                                    if (org.Nature == "省级")
                                    {
                                        reisGSLD = rollBLL.GetList().Where(a => a.EnCode == "300015").FirstOrDefault();
                                    }
                                    else
                                    {
                                        reisGSLD = rollBLL.GetList().Where(a => a.FullName == "公司领导").FirstOrDefault();

                                    }
                                    if (reisGSLD != null)
                                    {
                                        role.RoleIds += reisGSLD.RoleId + ",";
                                        role.RoleNames += reisGSLD.FullName + ",";
                                    }

                                }
                                if (isGLY == "是")
                                {
                                    RoleEntity reisGLY = null;
                                    if (org.Nature == "省级")
                                    {
                                        reisGLY = rollBLL.GetList().Where(a => a.EnCode == "300012").FirstOrDefault();
                                    }
                                    else
                                    {
                                        reisGLY = rollBLL.GetList().Where(a => a.FullName == "安全管理员").FirstOrDefault();

                                    }
                                    if (reisGLY != null)
                                    {
                                        role.RoleIds += reisGLY.RoleId + ",";
                                        role.RoleNames += reisGLY.FullName + ",";
                                    }

                                }
                                if (isZG == "是")
                                {
                                    RoleEntity reisGLY = null;
                                    if (org.Nature == "省级")
                                    {
                                        reisGLY = rollBLL.GetList().Where(a => a.EnCode == "300014").FirstOrDefault();
                                    }
                                    else
                                    {
                                        reisGLY = rollBLL.GetList().Where(a => a.FullName == "专工").FirstOrDefault();

                                    }
                                    if (reisGLY != null)
                                    {
                                        role.RoleIds += reisGLY.RoleId + ",";
                                        role.RoleNames += reisGLY.FullName + ",";
                                    }

                                }
                                role.RoleIds = role.RoleIds.TrimEnd(',');
                                role.RoleNames = role.RoleNames.TrimEnd(',');
                                try
                                {
                                    postBLL.SaveForm("", role);
                                    k++;
                                }
                                catch
                                {
                                    error++;
                                }
                            }
                        }
                    }
                    else
                    {
                        role.EnCode = DateTime.Now.ToString("yyyyMMddHHmmss") + k;
                        //var dept = list.Where(x => x.FullName == deptname).ToList();
                        //if (dept.Count > 0)
                        //{
                        //    var entity = dept.FirstOrDefault();
                        //    role.DeptId = entity.DepartmentId;
                        //    role.DeptName = entity.FullName;
                        //}
                        //层级角色
                        RoleEntity re = null;
                        DepartmentEntity org = new DepartmentBLL().GetEntity(orgId);
                        if (!nature.Contains("厂级"))
                        {

                            if (org.Nature == "省级")
                            {
                                re = rollBLL.GetList().Where(a => a.FullName == "省级用户").FirstOrDefault();
                            }
                            else
                            {
                                re = rollBLL.GetList().Where(a => a.FullName.Contains(nature + "级")).FirstOrDefault();
                            }

                        }
                        else
                        {
                            re = rollBLL.GetList().Where(a => a.FullName.Contains("公司级")).FirstOrDefault();
                        }
                        //普通用户
                        RoleEntity reuser = rollBLL.GetList().Where(a => a.FullName == "普通用户").FirstOrDefault();
                        //根据层级设置角色关系
                        if (!(re == null || reuser == null))
                        {

                            if (org.Nature == "省级")
                            {
                                role.RoleIds = re.RoleId + ",";
                                role.RoleNames = re.FullName + ",";
                            }
                            else
                            {
                                role.RoleIds = re.RoleId + "," + reuser.RoleId + ",";
                                role.RoleNames = re.FullName + "," + reuser.FullName + ",";
                            }
                        }
                        //赋值角色
                        if (isFZR == "是")
                        {
                            RoleEntity reisFZR = null;
                            if (org.Nature == "省级")
                            {
                                reisFZR = rollBLL.GetList().Where(a => a.EnCode == "300013").FirstOrDefault();
                            }
                            else
                            {
                                reisFZR = rollBLL.GetList().Where(a => a.FullName == "负责人").FirstOrDefault();

                            }
                            if (reisFZR != null)
                            {
                                role.RoleIds += reisFZR.RoleId + ",";
                                role.RoleNames += reisFZR.FullName + ",";
                            }

                        }
                        if (isGSLD == "是")
                        {
                            RoleEntity reisGSLD = null;
                            if (org.Nature == "省级")
                            {
                                reisGSLD = rollBLL.GetList().Where(a => a.EnCode == "300015").FirstOrDefault();
                            }
                            else
                            {
                                reisGSLD = rollBLL.GetList().Where(a => a.FullName == "公司领导").FirstOrDefault();

                            }
                            if (reisGSLD != null)
                            {
                                role.RoleIds += reisGSLD.RoleId + ",";
                                role.RoleNames += reisGSLD.FullName + ",";
                            }

                        }
                        if (isGLY == "是")
                        {
                            RoleEntity reisGLY = null;
                            if (org.Nature == "省级")
                            {
                                reisGLY = rollBLL.GetList().Where(a => a.EnCode == "300012").FirstOrDefault();
                            }
                            else
                            {
                                reisGLY = rollBLL.GetList().Where(a => a.FullName == "安全管理员").FirstOrDefault();

                            }
                            if (reisGLY != null)
                            {
                                role.RoleIds += reisGLY.RoleId + ",";
                                role.RoleNames += reisGLY.FullName + ",";
                            }

                        }
                        if (isZG == "是")
                        {
                            RoleEntity reisGLY = null;
                            if (org.Nature == "省级")
                            {
                                reisGLY = rollBLL.GetList().Where(a => a.EnCode == "300014").FirstOrDefault();
                            }
                            else
                            {
                                reisGLY = rollBLL.GetList().Where(a => a.FullName == "专工").FirstOrDefault();

                            }
                            if (reisGLY != null)
                            {
                                role.RoleIds += reisGLY.RoleId + ",";
                                role.RoleNames += reisGLY.FullName + ",";
                            }

                        }
                        role.RoleIds = role.RoleIds.TrimEnd(',');
                        role.RoleNames = role.RoleNames.TrimEnd(',');
                        try
                        {
                            postBLL.SaveForm("", role);
                        }
                        catch
                        {
                            error++;
                        }
                    }
                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
    }
}
