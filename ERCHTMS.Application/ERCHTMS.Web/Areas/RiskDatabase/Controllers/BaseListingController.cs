using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using ERCHTMS.Busines.BaseManage;
using System.Linq.Expressions;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：作业活动及设备设施清单
    /// </summary>
    public class BaseListingController : MvcControllerBase
    {
        private BaseListingBLL baselistingbll = new BaseListingBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private PostBLL postBLL = new PostBLL();

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
        /// 选择清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "id";
                pagination.p_fields = "name,name as equname,activitystep,isconventional,isspecialequ,areaname,areacode,areaid,others,case when b.num>0 then 0 else 1 end as status,b.num as evaluatenum,createuserdeptcode,c.fullname as createuserdeptname,a.createdate,post";
                pagination.p_tablename = "bis_baselisting a left join (select count(1) as num,listingid from bis_riskassess where status=1 and deletemark=0 and enabledmark=0 group by listingid) b on a.id=b.listingid left join base_department c on a.createuserdeptcode=c.encode";
                pagination.conditionJson = "1=1";
                var data = baselistingbll.GetPageListJson(pagination, queryJson);
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = baselistingbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        public ActionResult GetNameData(string query,int type)
        {
            try
            {
                Expression<Func<BaseListingEntity, bool>> condition = t => t.Name.Contains(query) && t.Type == type;
                var data = baselistingbll.GetList(condition).Select(t => t.Name).Distinct().ToArray();
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
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
            try
            {
                baselistingbll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
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
        public ActionResult SaveForm(string keyValue, BaseListingEntity entity)
        {
            try
            {
                //---****判断系统是否已经存在该作业活动、活动步骤的数据*****--
                Expression<Func<BaseListingEntity, bool>> condition = t => t.Name == entity.Name && t.ActivityStep == entity.ActivityStep && t.Type == 0 && t.PostId == entity.PostId;
                if (baselistingbll.GetList(condition).Count() > 0)
                {
                    return Error("存在该作业活动、活动步骤、岗位的数据，无法重复添加。");
                }
                baselistingbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        [HttpPost]
        public string ImportData()
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "超级管理员无此操作权限";
                }
                var currUser = OperatorProvider.Provider.Current();
                string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司

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
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = new DataTable();
                    if (cells.MaxDataRow > 1)
                    {
                        dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                        #region 作业活动类
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //用于数据验证填报部门
                            string deptlist = dt.Rows[i]["填报单位"].ToString().Trim();

                            string controlDept = currUser.DeptName;//管控部门
                            string controlDeptId = currUser.DeptId;//管控部门
                            string controlDeptCode = currUser.DeptCode;//管控部门

                            //岗位（工种）
                            string Post = dt.Rows[i]["岗位（工种）"].ToString().Trim();
                            string PostId = string.Empty;
                            //作业活动
                            string Name = dt.Rows[i]["作业活动"].ToString().Trim();
                            //活动步骤
                            string ActivityStep = dt.Rows[i]["活动步骤"].ToString().Trim();
                            //常规/非常规
                            string IsConventional = dt.Rows[i]["常规/非常规"].ToString().Trim();
                            //其他
                            string Others = dt.Rows[i]["其他"].ToString().Trim();



                            //---****值存在空验证*****--
                            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ActivityStep) || string.IsNullOrEmpty(IsConventional) || string.IsNullOrWhiteSpace(Post))
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                                error++;
                                continue;
                            }
                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            bool isSkip = false;
                            //验证所填部门是否存在
                            if (!string.IsNullOrWhiteSpace(deptlist))
                            {
                                var array = deptlist.Split('/');
                                for (int j = 0; j < array.Length; j++)
                                {
                                    if (j == 0)
                                    {
                                        var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "部门" && x.FullName == array[j].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                                if (entity == null)
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                                    error++;
                                                    isSkip = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    controlDept = entity.FullName;
                                                    controlDeptId = entity.DepartmentId;
                                                    controlDeptCode = entity.EnCode;
                                                    p1 = entity.DepartmentId;
                                                }
                                            }
                                            else
                                            {
                                                controlDept = entity.FullName;
                                                controlDeptId = entity.DepartmentId;
                                                controlDeptCode = entity.EnCode;
                                                p1 = entity.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity.FullName;
                                            controlDeptId = entity.DepartmentId;
                                            controlDeptCode = entity.EnCode;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else if (j == 1)
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                            if (entity1 == null)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                                error++;
                                                isSkip = true;
                                                break;
                                            }
                                            else
                                            {
                                                controlDept = entity1.FullName;
                                                controlDeptId = entity1.DepartmentId;
                                                controlDeptCode = entity1.EnCode;
                                                p2 = entity1.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                            p2 = entity1.DepartmentId;
                                        }

                                    }
                                    else
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                            error++;
                                            isSkip = true;
                                            break;
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                        }
                                    }
                                }
                            }

                            if (isSkip)
                            {
                                continue;
                            }

                            //验证岗位是不是在部门里面
                            var controldept = departmentBLL.GetEntity(controlDeptId);
                            RoleEntity re = new RoleEntity();
                            if (controldept.Nature == "厂级")
                            {
                                re = postBLL.GetList().Where(a => (a.FullName == Post && a.OrganizeId == orgId)).FirstOrDefault();
                            }
                            else
                            {
                                re = postBLL.GetList().Where(a => (a.FullName == Post && a.OrganizeId == orgId && a.DeptId == controlDeptId)).FirstOrDefault();
                            }
                            if (re == null)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行岗位有误,未能导入.";
                                error++;
                                continue;
                            }
                            else
                            {
                                PostId = re.RoleId;
                            }
                            //---****判断系统是否已经存在该作业活动、活动步骤的数据*****--
                            Expression<Func<BaseListingEntity, bool>> condition = t => t.Name == Name && t.ActivityStep == ActivityStep && t.Type == 0 && t.PostId == PostId;
                            if (baselistingbll.GetList(condition).Count() > 0)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行数据已经存在于系统中，无需添加.";
                                error++;
                                continue;
                            }
                            BaseListingEntity Listingentity = new BaseListingEntity();
                            Listingentity.Name = Name;
                            Listingentity.ActivityStep = ActivityStep;
                            Listingentity.IsConventional = IsConventional == "常规" ? 0 : 1;
                            Listingentity.Others = Others;
                            Listingentity.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                            Listingentity.ControlsDept = controlDept;
                            Listingentity.ControlsDeptId = controlDeptId;
                            Listingentity.ControlsDeptCode = controlDeptCode;
                            Listingentity.Type = 0;
                            Listingentity.Post = Post;
                            Listingentity.PostId = PostId;
                            condition = t => t.Name == Name && !(t.AreaName == null || t.AreaName.Trim() == "");
                            var defualt = baselistingbll.GetList(condition).ToList().FirstOrDefault();
                            Listingentity.AreaName = defualt == null ? "" : defualt.AreaName;
                            Listingentity.AreaId = defualt == null ? "" : defualt.AreaId;
                            Listingentity.AreaCode = defualt == null ? "" : defualt.AreaCode;
                            Listingentity.CreateDate = DateTime.Now.AddSeconds(i);
                            baselistingbll.SaveForm("", Listingentity);
                        }
                        count = dt.Rows.Count;
                        message = "作业活动类共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                        message += "</br>" + falseMessage + "</br>";
                        #endregion
                    }
                    else
                    {
                        message = "作业活动类没有数据。</br>";
                    }


                   
                    error = 0;
                    falseMessage = "";
                    cells = wb.Worksheets[1].Cells;
                    if (cells.MaxDataRow > 1)
                    {
                        #region 设备设施类
                        dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //用于数据验证填报部门
                            string deptlist = dt.Rows[i]["填报单位"].ToString().Trim();

                            string controlDept = currUser.DeptName;//管控部门
                            string controlDeptId = currUser.DeptId;//管控部门
                            string controlDeptCode = currUser.DeptCode;//管控部门
                            
                            //设备名称
                            string Name = dt.Rows[i]["设备名称"].ToString().Trim();
                            //所在地点
                            string arealist = dt.Rows[i]["所在地点"].ToString().Trim();
                            //是否特种设备
                            string IsSpecialEqu = dt.Rows[i]["是否特种设备"].ToString().Trim();
                            //其他
                            string Others = dt.Rows[i]["其他"].ToString().Trim();
                            string AreaName = string.Empty; //所在地点名称
                            string AreaId = string.Empty; //所在地点Id
                            string AreaCode = string.Empty; //所在地点Code



                            //---****值存在空验证*****--
                            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(arealist) || string.IsNullOrEmpty(IsSpecialEqu))
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                                error++;
                                continue;
                            }
                            var p1 = string.Empty;
                            var p2 = string.Empty;
                            bool isSkip = false;
                            //验证所填部门是否存在
                            if (!string.IsNullOrWhiteSpace(deptlist))
                            {
                                var array = deptlist.Split('/');
                                for (int j = 0; j < array.Length; j++)
                                {
                                    if (j == 0)
                                    {
                                        var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "部门" && x.FullName == array[j].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                                if (entity == null)
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                                    error++;
                                                    isSkip = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    controlDept = entity.FullName;
                                                    controlDeptId = entity.DepartmentId;
                                                    controlDeptCode = entity.EnCode;
                                                    p1 = entity.DepartmentId;
                                                }
                                            }
                                            else
                                            {
                                                controlDept = entity.FullName;
                                                controlDeptId = entity.DepartmentId;
                                                controlDeptCode = entity.EnCode;
                                                p1 = entity.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity.FullName;
                                            controlDeptId = entity.DepartmentId;
                                            controlDeptCode = entity.EnCode;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else if (j == 1)
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                            if (entity1 == null)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                                error++;
                                                isSkip = true;
                                                break;
                                            }
                                            else
                                            {
                                                controlDept = entity1.FullName;
                                                controlDeptId = entity1.DepartmentId;
                                                controlDeptCode = entity1.EnCode;
                                                p2 = entity1.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                            p2 = entity1.DepartmentId;
                                        }

                                    }
                                    else
                                    {
                                        var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                                        if (entity1 == null)
                                        {
                                            falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                            error++;
                                            isSkip = true;
                                            break;
                                        }
                                        else
                                        {
                                            controlDept = entity1.FullName;
                                            controlDeptId = entity1.DepartmentId;
                                            controlDeptCode = entity1.EnCode;
                                        }
                                    }
                                }
                            }
                            if (isSkip)
                            {
                                continue;
                            }
                            //验证所在地点（区域）
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == arealist && x.OrganizeId == currUser.OrganizeId).FirstOrDefault();
                            if (disItem != null)
                            {
                                AreaId = disItem.DistrictID;
                                AreaCode = disItem.DistrictCode;
                                AreaName = disItem.DistrictName;
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行所在地点信息与系统内置的区域不一致,未能导入.";
                                error++;
                                continue;
                            }
                            //---****判断系统是否已经存在该设备名称、所在地点的数据*****--
                            Expression<Func<BaseListingEntity, bool>> condition = t => t.Name == Name && t.AreaId == AreaId && t.Type == 1;
                            if (baselistingbll.GetList(condition).Count() > 0)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行数据已经存在于系统中，无需添加.";
                                error++;
                                continue;
                            }
                            BaseListingEntity Listingentity = new BaseListingEntity();
                            Listingentity.Name = Name;
                            Listingentity.AreaCode = AreaCode;
                            Listingentity.AreaId = AreaId;
                            Listingentity.AreaName = AreaName;
                            Listingentity.IsSpecialEqu = IsSpecialEqu == "是" ? 0 : 1;
                            Listingentity.Others = Others;
                            Listingentity.CreateUserDeptCode = string.IsNullOrWhiteSpace(deptlist) ? currUser.DeptCode : controlDeptCode;
                            Listingentity.ControlsDept = controlDept;
                            Listingentity.ControlsDeptId = controlDeptId;
                            Listingentity.ControlsDeptCode = controlDeptCode;
                            Listingentity.Type = 1;
                            Listingentity.CreateDate = DateTime.Now.AddSeconds(i);
                            baselistingbll.SaveForm("", Listingentity);
                        }
                        count = dt.Rows.Count;
                        message += "设备设施类共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                        message += "</br>" + falseMessage;
                        #endregion
                    }
                    else
                    {
                        message += "设备设施类没有数据。</br>";
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }
        /// <summary>
        /// 导出风险管控列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.p_kid = "";
                pagination.p_fields = "'' num1,post,name,activitystep,case when isconventional=0 then '常规' else  '非常规' end as isconventional,others as others1,b.fullname as dept1,to_char(a.createdate,'yyyy-MM-dd') as date1,'' num2,name as equname,areaname,case when isspecialequ=0 then '是' else  '否' end as isspecialequ,others as others2，b.fullname as dept2,to_char(a.createdate,'yyyy-MM-dd') as date2,type";
                pagination.p_tablename = "bis_baselisting a left join base_department b on a.createuserdeptcode=b.encode";
                pagination.conditionJson = "1=1";
                pagination.page = 1;
                pagination.rows = 100000;
                if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
                {
                    pagination.conditionJson += " and a.CREATEUSERORGCODE ='" + user.OrganizeCode + "'";
                }
                else
                {
                    pagination.conditionJson += " and a.createuserdeptcode ='" + user.DeptCode + "'";
                }
                var data = baselistingbll.GetPageListJson(pagination, queryJson);
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/作业活动及设备设施清单导出模板.xls"));
                if (data.Rows.Count > 0)
                {
                    DataTable dt1 = data.Select("type=0").Count() > 0 ? data.Select("type=0").CopyToDataTable() : new DataTable();
                    
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dt1.Rows[i]["num1"] = i + 1;
                        }
                        wb.Worksheets[0].Cells.ImportDataTable(dt1, false, 1, 0, data.Rows.Count, 7);
                    }

                    DataTable dt2 = data.Select("type=1").Count() > 0 ? data.Select("type=1").CopyToDataTable() : new DataTable();

                    if (dt2.Rows.Count > 0)
                    {
                        dt2.Columns.Remove("post"); dt2.Columns.Remove("name"); dt2.Columns.Remove("activitystep"); dt2.Columns.Remove("isconventional"); dt2.Columns.Remove("others1");
                        dt2.Columns.Remove("dept1"); dt2.Columns.Remove("date1"); dt2.Columns.Remove("num1");
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            dt2.Rows[i]["num2"] = i + 1;
                        }
                        wb.Worksheets[1].Cells.ImportDataTable(dt2, false, 1, 0, data.Rows.Count, 7);
                    }
                }
                wb.Save(Server.UrlEncode("作业活动及设备设施清单.xls"), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                return Success("导出成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        #endregion
    }
}
