using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using System.Web;
using System.Data;
using BSFramework.Util.Offices;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.OutsourcingProject;
using System.Net;
using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;
using System.IO;
using ERCHTMS.Entity.CarManage;
using System.Linq.Expressions;
using BSFramework.Util.Extension;
using System.Dynamic;
using BSFramework.Util.Attributes;
using System.Linq;
using ServiceStack.Common.Extensions;
using System.Data.Common;
using BSFramework.Data;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：部门管理
    /// </summary>
    public class DepartmentController : MvcControllerBase
    {
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        private DistrictBLL bis_districtbll = new DistrictBLL();
        private UserBLL userBLL = new UserBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private OutsourcingprojectBLL outProject = new OutsourcingprojectBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();

        #region 视图功能
        /// <summary>
        /// 部门管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 部门表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 部门选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 部门关系配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Config()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SelectTrainDepts()
        {
            return View();
        }
        /// <summary>
        /// 导入部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Import()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SelectToolsDept()
        {
            return View();
        }
        /// <summary>
        /// 新增工程页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddProjectForm()
        {
            return View();
        }
        #endregion

        #region 获取数据

        public string GetDeptIds(string deptId)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = new DataTable();
            if (user.RoleName.Contains("厂级部门") || user.RoleName.Contains("公司级"))
            {
                dt = departmentBLL.GetDataTable(string.Format("select  distinct OUTPROJECTID from EPG_OUTSOURINGENGINEER "));
            }
            else
            {
                dt = departmentBLL.GetDataTable(string.Format("select  distinct OUTPROJECTID from EPG_OUTSOURINGENGINEER where ENGINEERLETDEPTID in ('{0}') ", deptId.Replace(",", "','")));
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("{0},", dr[0].ToString());
            }
            return sb.ToString().Trim(',');
        }
        [HttpGet]
        public ActionResult GetWBDeptId(string orgId)
        {
            try
            {
                DataTable dtDept = departmentBLL.GetDataTable(string.Format("select DepartmentId from BASE_DEPARTMENT where organizeId='{0}' and description='外包工程承包商'", orgId));
                if (dtDept.Rows.Count > 0)
                {
                    return Success("获取数据成功", dtDept.Rows[0][0].ToString());
                }
                return Error("没有查询到数据");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #region 获取部门信息

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetDeptListByTargetCondition(string deptname)
        {
            DataTable dtDept = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(deptname))
                {
                    var parameter = new List<DbParameter>() { DbParameters.CreateDbParameter("@deptname", deptname) }.ToArray();

                    dtDept = departmentBLL.GetDataTableByParams(@"select a.departmentid ,a.encode  deptcode  ,a.fullname deptname ,b.fullname parentname from BASE_DEPARTMENT a left join base_department b on a.parentid = b.departmentid where a.fullname like  '%'|| @deptname ||'%' ", parameter);
                }
                return Content(dtDept.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 部门列表 
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetTreeJson(string organizeId, string keyword)
        {
            organizeId = string.IsNullOrEmpty(organizeId) ? "0" : organizeId;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            IEnumerable<DepartmentEntity> data = new List<DepartmentEntity>();
            IEnumerable<DepartmentEntity> newdata = new List<DepartmentEntity>();
            if (!user.IsSystem)
            {
                data = departmentBLL.GetList(organizeId).Where(t => t.OrganizeId == organizeId && t.Description != "各电厂" && t.Description != "区域子公司");
                if (!(user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户")))
                {
                    string deptIds = GetDeptIds(user.DeptId);
                    var depts = departmentBLL.GetList(organizeId).Where(t => deptIds.Contains(t.DepartmentId) && t.DepartmentId != "0");
                    if (depts.Count() > 0)
                    {
                        data = data.Where(t => t.EnCode.StartsWith(user.DeptCode) || deptIds.Contains(t.DepartmentId) || (t.Description == "外包工程承包商" && t.EnCode.StartsWith(user.OrganizeCode)));
                    }
                    else
                    {
                        data = data.Where(t => t.DeptCode.StartsWith(user.NewDeptCode));
                    }
                }
            }
            else
            {
                data = departmentBLL.GetList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                newdata = data.Where(t => t.FullName.Contains(keyword));
                data = GetParentId(newdata, data);
            }
            data = data.OrderBy(t => t.DeptCode).OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();


            List<DepartmentEntity> newList = data.ToList();
            List<DepartmentEntity> lstDepts = newList.Where(t => t.Description == "外包工程承包商").ToList();
            foreach (DepartmentEntity dept1 in lstDepts)
            {
                newList.Remove(dept1);
                //newList = newList.Where(t => t.Nature != "承包商").ToList();

                string newId = "cx100";
                newList.Add(new DepartmentEntity
                {
                    DepartmentId = newId,
                    FullName = "长协外包单位",
                    OrganizeId = dept1.OrganizeId,
                    EnCode = "",
                    ParentId = dept1.ParentId,
                    Nature = dept1.Nature,
                    DeptCode = dept1.DeptCode,
                    IsOrg = dept1.IsOrg
                });
                //string sql = string.Format("select DepartmentId,FullName,EnCode,ParentId,Description,Nature,DeptCode,IsOrg from base_department d where  d.nature='承包商' and d.depttype='长协' and d.ParentId='{0}'", dept1.DepartmentId);
                List<DepartmentEntity> lstNodes = newList.Where(t => t.Nature == "承包商" && t.DeptType == "长协" && t.ParentId == dept1.DepartmentId).ToList();
                foreach (DepartmentEntity dept in lstNodes)
                {
                    dept.ParentId = newId;

                }
                newId = "ls100";
                newList.Add(new DepartmentEntity
                {
                    DepartmentId = newId,
                    FullName = "临时外包单位",
                    EnCode = "",
                    ParentId = dept1.ParentId,
                    OrganizeId = dept1.OrganizeId,
                    Nature = dept1.Nature
                });
                lstNodes = newList.Where(t => t.Nature == "承包商" && t.DeptType == "临时" && t.ParentId == dept1.DepartmentId).ToList();
                foreach (DepartmentEntity dept in lstNodes)
                {
                    dept.ParentId = newId;

                }
            }
            var treeList = new List<TreeEntity>();
            foreach (DepartmentEntity item in newList)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = newList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.isexpand = item.DepartmentId == organizeId ? true : false;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                tree.AttributeA = "IsOrg";
                tree.AttributeValueA = item.IsOrg.ToString();
                tree.AttributeC = "NewCode";
                tree.AttributeValueC = item.DeptCode;
                tree.AttributeD = "IsDept";
                tree.AttributeValueD = !string.IsNullOrWhiteSpace(item.Description) ? "0" : "1";
                treeList.Add(tree);
            }
            if (treeList.Count > 0)
            {
                treeList[0].isexpand = true;
            }
            return Content(treeList.TreeToJson(organizeId));
        }

        [HttpGet]
        public ActionResult GetTrainDeptTreeJson(string deptId, string keyword)
        {
            try
            {
                string url = new DataItemDetailBLL().GetItemValue("WebApiUrl", "Train");
                if (string.IsNullOrWhiteSpace(url))
                {
                    return Error("未配置java培训平台获取部门组织机构的API地址！");
                }
                else
                {
                    string orgId = "";
                    DepartmentEntity dept = departmentBLL.GetEntity(deptId);
                    if (dept != null)
                    {
                        DepartmentEntity org = departmentBLL.GetEntity(dept.OrganizeId);
                        if (org != null)
                        {
                            if (!string.IsNullOrWhiteSpace(org.InnerPhone))
                            {


                                orgId = org.InnerPhone;
                                WebClient wc = new WebClient();
                                wc.Credentials = CredentialCache.DefaultCredentials;
                                wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
                                wc.Encoding = Encoding.UTF8;
                                string result = wc.DownloadString(url + "?companyId=" + orgId);
                                //string result = "{\"meta\":{ \"success\":  true,\"message\":  \"ok\"},\"data\":  {\"companyId\":  \"1\",\"companyName\": \"单位名称\",	";
                                //result += "\"departments\":[{\"department_id\":\"2222222\",\"department_name\":\"开发部\",\"department_code\":\"001\",\"department_pid\":\"1\"},{\"department_id\":\"12\",\"department_name\":\"测试部\",\"department_code\":\"002\",\"department_pid\":\"1\"}]}}";
                                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                                List<DepartmentEntity> data = new List<DepartmentEntity>();
                                if (dy.meta.success)
                                {
                                    data.Add(new DepartmentEntity
                                    {
                                        DepartmentId = dy.data.companyId,
                                        FullName = dy.data.companyName,
                                        EnCode = "00",
                                        ParentId = "0"
                                    });

                                    List<object> listDepts = dy.data.departments;

                                    foreach (object obj in listDepts)
                                    {
                                        dy = obj;
                                        data.Add(new DepartmentEntity
                                        {
                                            DepartmentId = dy.department_id,
                                            FullName = dy.department_name,
                                            EnCode = dy.department_code,
                                            ParentId = dy.department_pid
                                        });
                                    }
                                }
                                if (!string.IsNullOrEmpty(keyword))
                                {
                                    data = data.TreeWhere(t => t.FullName.Contains(keyword), "DepartmentId").ToList();
                                }
                                var treeList = new List<TreeEntity>();
                                foreach (DepartmentEntity item in data)
                                {
                                    TreeEntity tree = new TreeEntity();
                                    bool hasChildren = data.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                                    tree.id = item.DepartmentId;
                                    tree.text = item.FullName;
                                    tree.value = item.EnCode;
                                    tree.isexpand = false;
                                    tree.complete = true;
                                    tree.hasChildren = hasChildren;
                                    tree.parentId = item.ParentId;
                                    tree.Attribute = "Code";
                                    tree.AttributeValue = item.EnCode;
                                    treeList.Add(tree);
                                }
                                return Success("操作成功", treeList.TreeToJson());
                            }
                            else
                            {
                                return Error("请先配置培训平台单位ID！");
                            }
                        }
                        else
                        {
                            return Error("部门信息不存在！");
                        }
                    }
                    else
                    {
                        return Error("部门信息不存在！");
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 根据部门ID获取该部门下的部门列表 
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetTreeJsonByDeptId(string deptId, string keyword)
        {
            deptId = string.IsNullOrEmpty(deptId) ? "0" : deptId;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            IEnumerable<DepartmentEntity> data = new List<DepartmentEntity>();
            IEnumerable<DepartmentEntity> newdata = new List<DepartmentEntity>();
            if (!user.IsSystem)
            {
                data = departmentBLL.GetList().Where(t => t.DepartmentId == deptId || t.ParentId == deptId);
            }
            else
            {
                data = departmentBLL.GetList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                newdata = data.Where(t => t.FullName.Contains(keyword));
                data = GetParentId(newdata, data);
            }
            data = data.OrderBy(t => t.DeptCode).OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();

            var treeList = new List<TreeEntity>();



            foreach (DepartmentEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.isexpand = item.DepartmentId == deptId ? true : false;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                tree.AttributeA = "IsOrg";
                tree.AttributeValueA = item.IsOrg.ToString();
                tree.AttributeC = "NewCode";
                tree.AttributeValueC = item.DeptCode;
                treeList.Add(tree);
            }
            if (treeList.Count > 0)
            {
                treeList[0].isexpand = true;
            }
            return Content(treeList.TreeToJson(OperatorProvider.Provider.Current().OrganizeId));
        }

        //找到集合中最上级父节点的id
        public List<DepartmentEntity> GetParentId(IEnumerable<DepartmentEntity> data, IEnumerable<DepartmentEntity> alldata)
        {
            string id = "";
            List<DepartmentEntity> newdata = new List<DepartmentEntity>();
            if (data.Count() > 0)
            {
                newdata = data.ToList();

                for (int i = 0; i < newdata.Count; i++)
                {
                    id = newdata[i].ParentId;
                    //如果自己表里面没有父级 而查询前的表里面有则加入到表中
                    if (newdata.Where(it => it.DepartmentId == id).Count() == 0 && alldata.Where(it => it.DepartmentId == id).Count() > 0)
                    {

                        newdata.Add(alldata.Where(it => it.DepartmentId == id).FirstOrDefault());
                    }

                }


            }

            return newdata;

        }

        /// <summary>
        /// 部门列表 
        /// </summary>
        /// <param name="organizeCode">公司Code</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetTreeCodeJson(string organizeCode, string keyword)
        {
            var data = departmentCache.GetDeptListByCode(organizeCode).OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.FullName.Contains(keyword), "DepartmentId").OrderBy(t => t.SortCode).ToList();
            }
            var treeList = new List<TreeEntity>();
            foreach (DepartmentEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                tree.AttributeA = "IsOrg";
                tree.AttributeValueA = item.IsOrg.ToString();
                tree.AttributeC = "NewCode";
                tree.AttributeValueC = item.DeptCode;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }


        [HttpGet]
        public ActionResult GetDeptTreeJson1(string id = "0")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            List<OrganizeEntity> orgList = new List<OrganizeEntity>();
            List<DepartmentEntity> deptList = new List<DepartmentEntity>();
            if (user.IsSystem)
            {
                orgList = organizeCache.GetList().ToList();
            }
            else
            {
                orgList = organizeCache.GetList().Where(t => t.OrganizeId == user.OrganizeId).OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();
            }
            var treeList = new List<TreeEntity>();
            foreach (DepartmentEntity item in deptList)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = deptList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                tree.AttributeA = "IsOrg";
                tree.AttributeValueA = item.IsOrg.ToString();
                tree.AttributeC = "NewCode";
                tree.AttributeValueC = item.DeptCode;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(id));
        }

        [HttpGet]
        public ActionResult GetToolsDepts(string key)
        {

            SstmsService.CheckHeader header = new SstmsService.CheckHeader();
            header.Key = key;
            // 实例化服务对象
            SstmsService.DataServiceSoapClient service = new SstmsService.DataServiceSoapClient();
            List<SstmsService.Depart> data = service.GetDeparts(header).OrderBy(t => t.DepartCode).ToList();
            if (data.Count > 0)
            {
                var treeList = new List<TreeEntity>();
                foreach (SstmsService.Depart dept in data)
                {
                    TreeEntity tree = new TreeEntity();
                    bool hasChildren = data.Where(t => t.ParentID == dept.ID).Count() > 0 ? true : false;
                    tree.id = dept.ID;
                    tree.text = dept.DepartName;
                    tree.value = dept.DepartCode;
                    tree.isexpand = false;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = dept.ParentID;
                    treeList.Add(tree);
                }
                return Content(treeList.TreeToJson(data[0].ParentID));
            }
            else
            {
                return Error("没有查询到数据");
            }

        }
        /// <summary>
        /// 获取数据字典列表（绑定控件）
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataItemListJson()
        {
            var data = bis_districtbll.GetList().OrderBy(a => a.SortCode);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 部门列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>返回机构+部门树形Json</returns>
        public ActionResult GetOrganizeTreeJson(string keyword = "", int mode = 0)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                IEnumerable<DepartmentEntity> departmentdata = departmentBLL.GetList();
                if (mode == 1)
                {
                    departmentdata = departmentdata.Where(t => t.Nature != "班组");
                }
                var treeList = new List<TreeEntity>();
                string parentId = "0";
                if (!user.IsSystem)
                {
                    departmentdata = departmentdata.Where(t => t.DepartmentId != "0" && t.DeptCode.StartsWith(user.NewDeptCode));
                    if (departmentdata.Count() > 0)
                    {
                        parentId = departmentBLL.GetEntity(user.OrganizeId).ParentId;

                    }
                }
                else
                {
                    if (mode == 1)
                    {
                        departmentdata = departmentBLL.GetList().Where(t => t.DepartmentId != "0" && t.Nature != "班组");
                    }
                    else
                    {
                        departmentdata = departmentBLL.GetList().Where(t => t.DepartmentId != "0");
                    }

                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    departmentdata = departmentdata.Where(t => t.FullName.Contains(keyword.Trim()));
                    if (departmentdata.Count() > 0)
                    {
                        parentId = departmentdata.OrderBy(t => t.FullName).OrderBy(t => t.SortCode).FirstOrDefault().ParentId;
                    }
                }
                List<DepartmentEntity> newList = departmentdata.ToList();
                if (mode == 0)
                {
                    DepartmentEntity dept1 = newList.Where(t => t.Description == "外包工程承包商").FirstOrDefault();
                    if (dept1 != null)
                    {
                        newList.Remove(dept1);
                        newList = newList.Where(t => t.Nature != "承包商").ToList();

                        string newId = "cx100";
                        newList.Add(new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "长协外包单位",
                            EnCode = "",
                            ParentId = dept1.ParentId,
                            Nature = dept1.Nature,
                            Description = "外包工程承包商"
                        });
                        string sql = string.Format("select DepartmentId,FullName,EnCode,ParentId,Description,Nature,DeptCode,Manager,ManagerId,IsOrg from base_department d where d.organizeid='{0}' and d.nature='承包商' and d.depttype='长协' and d.ParentId='{1}'", user.OrganizeId, dept1.DepartmentId);
                        DataTable dtDepts = departmentBLL.GetDataTable(sql);
                        foreach (DataRow dr in dtDepts.Rows)
                        {
                            newList.Add(new DepartmentEntity
                            {
                                DepartmentId = dr[0].ToString(),
                                FullName = dr[1].ToString(),
                                EnCode = dr[2].ToString(),
                                ParentId = newId,
                                Nature = dr[5].ToString(),
                                Description = dr["Description"].ToString()

                            });
                            sql = string.Format("select DepartmentId,FullName,EnCode,ParentId,Description,Nature,DeptCode,Manager,ManagerId,IsOrg from base_department d where d.organizeid='{0}' and encode like '{1}%' and encode<>'{1}'", user.OrganizeId, dr[2].ToString());
                            DataTable dtChildren = departmentBLL.GetDataTable(sql);
                            foreach (DataRow dr1 in dtChildren.Rows)
                            {
                                newList.Add(new DepartmentEntity
                                {
                                    DepartmentId = dr1[0].ToString(),
                                    FullName = dr1[1].ToString(),
                                    EnCode = dr1[2].ToString(),
                                    ParentId = dr1[3].ToString(),
                                    Nature = dr1[5].ToString(),
                                    Description = dr["Description"].ToString()
                                });
                            }
                        }
                        newId = "ls100";
                        newList.Add(new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "临时外包单位",
                            EnCode = "",
                            ParentId = dept1.ParentId,
                            Nature = dept1.Nature,
                            Description = dept1.Description
                        });
                        sql = string.Format("select DepartmentId,FullName,EnCode,ParentId,Description,Nature,DeptCode,Manager,ManagerId,IsOrg from base_department d where d.organizeid='{0}' and d.nature='承包商' and d.depttype='临时' and d.ParentId='{1}'", user.OrganizeId, dept1.DepartmentId);
                        dtDepts = departmentBLL.GetDataTable(sql);
                        foreach (DataRow dr in dtDepts.Rows)
                        {
                            newList.Add(new DepartmentEntity
                            {
                                DepartmentId = dr[0].ToString(),
                                FullName = dr[1].ToString(),
                                EnCode = dr[2].ToString(),
                                ParentId = newId,
                                Nature = dr[5].ToString()
                            });
                            sql = string.Format("select DepartmentId,FullName,EnCode,ParentId,Description,Nature,DeptCode,Manager,ManagerId,IsOrg from base_department d where d.organizeid='{0}' and encode like '{1}%' and encode<>'{1}'", user.OrganizeId, dr[2].ToString());
                            DataTable dtChildren = departmentBLL.GetDataTable(sql);
                            foreach (DataRow dr1 in dtChildren.Rows)
                            {
                                newList.Add(new DepartmentEntity
                                {
                                    DepartmentId = dr1[0].ToString(),
                                    FullName = dr1[1].ToString(),
                                    EnCode = dr1[2].ToString(),
                                    ParentId = dr1[3].ToString(),
                                    Nature = dr1[5].ToString(),
                                    Description = dr1["Description"].ToString()

                                });
                            }
                        }
                    }
                }
                foreach (DepartmentEntity item in newList)
                {
                    #region 部门
                    TreeEntity tree = new TreeEntity();
                    bool hasChildren = newList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                    tree.id = item.DepartmentId;
                    tree.text = item.FullName;
                    tree.value = item.DepartmentId;
                    tree.parentId = item.ParentId;
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.Attribute = "Sort";
                    if (item.Nature == "集团" || item.Nature == "省级" || item.Nature == "厂级")
                    {
                        tree.AttributeValue = "Organize";
                    }
                    else
                    {
                        tree.AttributeValue = "Department";
                    }
                    var dept = departmentBLL.GetDeptOrgInfo(item.DepartmentId);
                    if (dept != null)
                    {
                        tree.AttributeA = "OrganizeId";
                        tree.AttributeValueA = dept.DepartmentId;
                    }
                    tree.AttributeB = "isOut";
                    tree.AttributeValueB = string.IsNullOrEmpty(item.Description) ? "0" : "1";
                    tree.AttributeC = "Nature";
                    tree.AttributeValueC = item.Nature;
                    tree.AttributeD = "Description";
                    tree.AttributeValueD = string.IsNullOrEmpty(item.Description) ? "" : item.Description;
                    treeList.Add(tree);
                    #endregion
                }
                return Content(treeList.TreeToJson(parentId));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 部门列表 
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回树形列表Json</returns>
        [HttpGet]
        public ActionResult GetTreeListJson(string condition, string keyword, string nodeId = "")
        {
            try
            {
                string parentId = "0";
                Operator user = OperatorProvider.Provider.Current();
                List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
                IEnumerable<DepartmentEntity> lstDepts = departmentBLL.GetList();
                if (user.IsSystem)
                {

                    departmentdata = lstDepts.OrderBy(t => t.FullName).OrderBy(a => a.SortCode).ToList();
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        if (authType == "1" || authType == "2")
                        {
                            departmentdata = lstDepts.Where(t => t.DepartmentId == user.DeptId).ToList();
                        }
                        if (authType == "3")
                        {
                            departmentdata = lstDepts.Where(t => t.EnCode.StartsWith(user.DeptCode)).ToList();
                        }
                        if (authType == "4")
                        {
                            if (user.RoleName.Contains("省级") || user.RoleName.Contains("集团"))
                            {
                                departmentdata = lstDepts.Where(t => t.DeptCode.StartsWith(user.NewDeptCode)).ToList();
                            }
                            else
                            {
                                departmentdata = lstDepts.Where(t => t.OrganizeId == user.OrganizeId || t.DepartmentId == user.OrganizeId).ToList();
                            }

                        }
                        if (authType == "5")
                        {
                            if (user.RoleName.Contains("省级") || user.RoleName.Contains("集团"))
                            {
                                departmentdata = lstDepts.Where(t => t.DeptCode.StartsWith(user.NewDeptCode)).ToList();
                            }
                            else
                            {
                                departmentdata = lstDepts.Where(t => t.OrganizeId == user.OrganizeId).ToList();
                            }
                            //departmentdata = departmentBLL.GetList().ToList();
                        }
                        departmentdata = departmentdata.OrderBy(t => t.FullName).OrderBy(a => a.SortCode).ToList();
                        if (departmentdata.Count > 0)
                        {
                            var dept = departmentBLL.GetEntity(user.OrganizeId);
                            if (dept != null)
                            {
                                parentId = dept.ParentId;
                            }
                            else
                            {
                                parentId = user.OrganizeId;
                            }

                        }
                    }
                    else
                    {
                        return Error("无数据");
                    }
                }

                if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                {
                    #region 多条件查询
                    switch (condition)
                    {
                        case "FullName":    //部门名称
                            departmentdata = departmentdata.TreeWhere(t => t.FullName.Contains(keyword), "DepartmentId");
                            break;
                        case "EnCode":      //部门编号
                            departmentdata = departmentdata.TreeWhere(t => t.EnCode.Contains(keyword), "DepartmentId");
                            break;
                        case "ShortName":   //部门简称
                            departmentdata = departmentdata.TreeWhere(t => t.ShortName.Contains(keyword), "DepartmentId");
                            break;
                        case "Manager":     //负责人
                            departmentdata = departmentdata.TreeWhere(t => t.Manager.Contains(keyword), "DepartmentId");
                            break;
                        case "OuterPhone":  //电话号
                            departmentdata = departmentdata.TreeWhere(t => t.OuterPhone.Contains(keyword), "DepartmentId");
                            break;
                        case "InnerPhone":  //分机号
                            departmentdata = departmentdata.TreeWhere(t => t.Manager.Contains(keyword), "DepartmentId");
                            break;
                        default:
                            break;
                    }
                    #endregion
                }

                if (!string.IsNullOrWhiteSpace(nodeId))
                {
                    DepartmentEntity node = departmentBLL.GetEntity(nodeId);
                    var arr = departmentBLL.GetDataTable(string.Format("select DepartmentId from BASE_DEPARTMENT where instr('{0}',encode)=1 ", node.EnCode)).AsEnumerable().Select(d => d.Field<string>("DepartmentId")).ToArray();
                    if (arr.Length > 0)
                    {
                        nodeId = string.Join<string>(",", arr);
                    }
                }

                List<DepartmentEntity> newList = departmentdata;
                DepartmentEntity dept1 = newList.Where(t => t.Description == "外包工程承包商").FirstOrDefault();
                if (dept1 != null)
                {
                    newList.Remove(dept1);
                    string newId = "cx100";
                    newList.Add(new DepartmentEntity
                    {
                        DepartmentId = newId,
                        FullName = "长协外包单位",
                        OrganizeId = dept1.OrganizeId,
                        EnCode = "",
                        ParentId = dept1.ParentId,
                        Nature = dept1.Nature
                    });
                    List<DepartmentEntity> lstChildren = newList.Where(t => t.Nature == "承包商" && t.DeptType == "长协" && t.ParentId == dept1.DepartmentId).ToList();
                    foreach (DepartmentEntity dept in lstChildren)
                    {
                        dept.ParentId = newId;

                    }
                    newId = "ls100";
                    newList.Add(new DepartmentEntity
                    {
                        DepartmentId = newId,
                        FullName = "临时外包单位",
                        EnCode = "",
                        ParentId = dept1.ParentId,
                        OrganizeId = dept1.OrganizeId,
                        Nature = dept1.Nature
                    });
                    lstChildren = newList.Where(t => t.Nature == "承包商" && t.DeptType == "临时" && t.ParentId == dept1.DepartmentId).ToList();
                    foreach (DepartmentEntity dept in lstChildren)
                    {
                        dept.ParentId = newId;

                    }
                }
                var treeList = new List<TreeGridEntity>();
                foreach (DepartmentEntity item in newList)
                {
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = newList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                    tree.id = item.DepartmentId;
                    tree.parentId = item.ParentId;
                    item.HasChild = hasChildren.ToString();
                    tree.expanded = item.ParentId == parentId || nodeId.Contains(item.DepartmentId);
                    tree.hasChildren = hasChildren;
                    string itemJson = item.ToJson();
                    if (item.Nature == "集团" || item.Nature == "省级" || item.Nature == "厂级")
                    {
                        itemJson = itemJson.Insert(1, "\"Sort\":\"Organize\",");
                    }
                    else
                    {
                        itemJson = itemJson.Insert(1, "\"Sort\":\"Department\",");
                    }
                    var dept = departmentBLL.GetDeptOrgInfo(item.DepartmentId);
                    if (dept != null)
                    {
                        itemJson = itemJson.Insert(1, "\"OrganizeId\":\"" + dept.DepartmentId + "\",");
                    }
                    tree.entityJson = itemJson;
                    treeList.Add(tree);
                }
                return Content(treeList.TreeJson(parentId));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 部门实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue, int mode = 1)
        {
            try
            {
                var data = departmentBLL.GetEntity(keyValue);
                if (data != null)
                {
                    if (!string.IsNullOrWhiteSpace(data.ManagerId))
                    {
                        var user = userBLL.GetUserInfoByAccount(data.ManagerId);
                        if (user != null)
                        {
                            data.InnerPhone = user.Mobile;
                        }
                    }
                    if (data.Description == "外包工程承包商")
                    {
                        data.Fax = "1";
                    }
                    else
                    {
                        data.Fax = "0";
                    }
                }
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 根据发包部门获取承包商信息
        /// </summary>
        /// <param name="sendDeptId"></param>
        /// <param name="orzid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOutProjectBySendDeptId(string orgid)
        {
            var dept = departmentBLL.GetList().Where(x => x.Description == "外包工程承包商" && x.OrganizeId == orgid).ToList().FirstOrDefault();
            if (dept != null)
            {
                var data = departmentBLL.GetNotBlackList().Where(x => x.OrganizeId == orgid && x.ParentId == dept.DepartmentId);
                return Content(data.ToJson());
            }
            else
            {
                return Content(new List<DepartmentEntity>().ToJson());
            }

        }


        /// <summary>
        /// 根据当前登录人获取班组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTeamByDeptId()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var data = departmentBLL.GetList().Where(x => x.Nature == "班组" && x.OrganizeId == user.OrganizeId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 根据当前登录人获取承包商
        /// </summary>
        /// <param name="sendDeptId"></param>
        /// <param name="orzid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOutProjectByAll()
        {
            var data = departmentBLL.GetNotBlackList();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                data = data.Where(a => a.OrganizeId == user.OrganizeId);
            }
            else if (role.Contains("承包商级用户"))
            {
                data = data.Where(a => a.DepartmentId == user.DeptId);
            }
            else
            {
                string deptIds = GetDeptIds(user.DeptId);
                data = data.Where(a => deptIds.Contains(a.DepartmentId) && a.DepartmentId != "0");
            }
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetContractDept()
        {

            Operator user = OperatorProvider.Provider.Current();
            var data = departmentBLL.GetList();
            if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
            {
                data = departmentBLL.GetList().Where(x => x.Nature == "承包商" && x.OrganizeId == user.OrganizeId);
            }
            else if (user.RoleName.Contains("承包商级用户"))
            {
                data = departmentBLL.GetList().Where(x => x.Nature == "承包商" && x.DepartmentId == user.DeptId && x.OrganizeId == user.OrganizeId);
            }
            else
            {
                string deptIds = GetDeptIds(user.DeptId);
                data = departmentBLL.GetList().Where(x => x.Nature == "承包商" && deptIds.Contains(x.DepartmentId) && x.DepartmentId != "0" && x.OrganizeId == user.OrganizeId);
            }
            return Content(data.ToJson());
        }
        //GetContractDepts
        /// <summary>
        /// 部门
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public ActionResult GetType(string keyValue)
        {
            var data = departmentBLL.GetEntity(keyValue);
            var flag = false;
            if (data != null)
            {
                if (data.Description != "外包工程承包商")
                {
                    flag = true;
                }
            }
            return Content(flag.ToJson());
        }

        /// <summary>
        /// 获取发包单位
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public ActionResult GetDeptListJson(string orgid)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            List<DepartmentEntity> list = null;
            if (currUser.IsSystem)
            {
                list = departmentBLL.GetList().Where(t => t.OrganizeId == orgid && t.Nature == "部门" && t.Description != "外包工程承包商").OrderBy(t => t.SortCode).ToList();
            }
            else
            {

                if (currUser.RoleName.Contains("省级") || currUser.RoleName.Contains("集团"))
                {
                    list = departmentBLL.GetList().Where(t => t.OrganizeId == orgid && t.Nature == "部门" && t.Description != "外包工程承包商").OrderBy(t => t.SortCode).ToList();
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                {
                    list = departmentBLL.GetList().Where(t => t.OrganizeId == orgid && t.Nature == "部门" && t.Description != "外包工程承包商").OrderBy(t => t.SortCode).ToList();
                }
                else if (currUser.RoleName.Contains("承包商"))
                {
                    DepartmentEntity currDept = new DepartmentBLL().GetEntity(currUser.DeptId);
                    string deptIds = GetDeptIds(currDept.DepartmentId);
                    list = departmentBLL.GetList().Where(t => t.OrganizeId == orgid && t.Nature == "部门" && t.Description != "外包工程承包商" && deptIds.Contains(t.DepartmentId) && t.DepartmentId != "0").OrderBy(t => t.SortCode).ToList();
                }
                else
                {
                    list = departmentBLL.GetList().Where(t => t.OrganizeId == orgid && t.Nature == "部门" && t.Description != "外包工程承包商" && t.DepartmentId == currUser.DeptId).OrderBy(t => t.SortCode).ToList();
                }
            }

            return Content(list.ToJson());
        }

        /// <summary>
        /// 获取新增部门的类型
        /// </summary>
        /// <param name="EnCode"></param>
        /// <param name="DeptId"></param>
        /// <param name="typeData"></param>
        /// <returns></returns>
        public ActionResult GetDeptName(string EnCode, string DeptId)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var data = new DataItemDetailBLL().GetDataItemListByItemCode(EnCode);
            //if (!user.IsSystem)
            //{
            //    if (user.RoleName.Contains("公司级用户"))
            //    {
            //        data = data.Where(x => x.ItemName == "专业" || x.ItemName == "部门" || x.ItemName == "班组");
            //    }
            //    else
            //    {
            //        var dept = departmentBLL.GetEntity(DeptId);
            //        if (dept != null)
            //        {
            //            var nature = dept.Nature;

            //            if (nature == "集团")
            //            {
            //                data = data.Where(x => x.ItemName == "省级" || x.ItemName == "部门");
            //            }
            //            else if (nature == "省级")
            //            {
            //                data = data.Where(x => x.ItemName == "厂级" || x.ItemName == "部门" || x.ItemName == "省级");
            //            }
            //            else if (nature == "厂级")
            //            {
            //                data = data.Where(x => x.ItemName == "部门");
            //            }
            //            else if (nature == "部门")
            //            {
            //                if (dept.Description == "外包工程承包商")
            //                    data = data.Where(x => x.ItemName == "承包商");
            //                else
            //                {
            //                    data = data.Where(x => x.ItemName == "专业" || x.ItemName == "班组");
            //                }
            //            }
            //            else if (nature == "专业")
            //                data = data.Where(x => x.ItemName == "班组");
            //            else if (nature == "承包商")
            //                data = data.Where(x => x.ItemName == "分包商");
            //            else
            //            {
            //                data = data.Where(x => x.ItemName == "-1");
            //            }
            //            if (data.Count() == 0)
            //            {
            //                data = new DataItemDetailBLL().GetDataItemListByItemCode(EnCode).Where(x => x.ItemName == nature);
            //            }
            //        }
            //        else
            //        {
            //            data = data.Where(x => x.ItemName == "集团" || x.ItemName == "省级" || x.ItemName == "厂级");
            //        }
            //    }
            //}
            return Content(data.ToJson());
        }



        #endregion

        #region 验证数据
        /// <summary>
        /// 部门编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistEnCode(string EnCode, string keyValue)
        {
            bool IsOk = departmentBLL.ExistEnCode(EnCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 部门名称不能重复
        /// </summary>
        /// <param name="FullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistFullName(string FullName, string keyValue)
        {
            bool IsOk = departmentBLL.ExistFullName(FullName, keyValue);
            return Content(IsOk.ToString());
        }
        #endregion
        /// <summary>
        /// 判断是否创建外包工程
        /// </summary>
        /// <param name="keyValue">部门Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExistEngineer(string keyValue)
        {
            List<OutsouringengineerEntity> engList = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == keyValue).ToList();
            if (engList.Count > 0)
            {
                return Error("已创建外包工程,不允许删除!");
            }
            else
                return Success("未创建外包工程");
        }
        #region 提交数据
        /// <summary>
        /// 删除部门
        /// 承包商关联工程信息,如果创建了工程不允许删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HandlerMonitor(6, "删除部门信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                DepartmentEntity entity = departmentBLL.GetEntity(keyValue);
                if (entity != null)
                {
                    //List<OutsouringengineerEntity> engList = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == entity.DepartmentId).ToList();
                    //if (engList.Count > 0)
                    //{
                    //    return Error("已创建外包工程,不允许删除!");
                    //}
                    DataItemDetailBLL di = new DataItemDetailBLL();
                    Operator user = OperatorProvider.Provider.Current();
                    var expression = LinqExtensions.True<UserEntity>();
                    expression = expression.And(t => t.DepartmentCode.StartsWith(entity.EnCode));

                    List<UserEntity> lstUsers = userBLL.GetListForCon(expression).Where(a => a.DepartmentCode.StartsWith(entity.EnCode)).ToList();
                    bool result = departmentBLL.RemoveForm(keyValue, lstUsers);

                    if (result)
                    {
                        string ModuleName = SystemInfo.CurrentModuleName;
                        string ModuleId = SystemInfo.CurrentModuleId;

                        LogEntity logEntity = new LogEntity();
                        logEntity.Browser = this.Request.Browser.Browser;
                        logEntity.CategoryId = 6;
                        logEntity.OperateTypeId = "6";
                        logEntity.OperateType = "删除";
                        logEntity.OperateAccount = user.UserName;
                        logEntity.OperateUserId = user.UserId;
                        logEntity.ExecuteResult = 1;
                        logEntity.Module = ModuleName;
                        logEntity.ModuleId = ModuleId;
                        logEntity.ExecuteResultJson = "操作信息:删除单位名称为" + entity.FullName + "的部门信息, 请求引用: 无, 其他信息:无";
                        LogBLL.WriteLog(logEntity);
                        if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                        {
                            var task = Task.Factory.StartNew(() =>
                            {
                                DeleteDept(entity, user.Account);
                            });

                        }
                        string way = di.GetItemValue("WhatWay");
                        //对接.net培训平台
                        if (way == "0")
                        {

                        }
                        //对接java培训平台
                        if (way == "1")
                        {
                            DepartmentEntity org = departmentBLL.GetEntity(entity.OrganizeId);
                            string enCode = entity.EnCode;
                            if (org.IsTrain == 1)
                            {
                                if (!string.IsNullOrWhiteSpace(entity.DeptKey))
                                {
                                    string[] arr = entity.DeptKey.Split('|');
                                    if (arr.Length == 2)
                                    {
                                        keyValue = arr[0];
                                        enCode = arr[1];
                                    }
                                }
                                Task.Factory.StartNew(() =>
                                {
                                    object obj = new
                                    {
                                        action = "delete",
                                        time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        deptId = keyValue,
                                        deptCode = enCode,
                                        companyId = org.InnerPhone
                                    };
                                    List<object> list = new List<object>();
                                    list.Add(obj);
                                    Busines.JPush.JPushApi.PushMessage(list, 0);

                                    logEntity = new LogEntity();
                                    logEntity.Browser = this.Request.Browser.Browser;
                                    logEntity.CategoryId = 5;
                                    logEntity.OperateTypeId = "6";
                                    logEntity.OperateType = "删除";
                                    logEntity.OperateAccount = user.UserName;
                                    logEntity.OperateUserId = user.UserId;
                                    logEntity.ExecuteResult = 1;
                                    logEntity.Module = ModuleName;
                                    logEntity.ModuleId = ModuleId;
                                    logEntity.ExecuteResultJson = string.Format("同步部门(删除)信息到java培训平台,同步信息：{0}", list.ToJson());
                                    LogBLL.WriteLog(logEntity);


                                    List<object> lstObjs = lstUsers.Select(t => new
                                    {
                                        action = "delete",
                                        time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        userId = t.UserId,
                                        account = t.Account,
                                        companyId = org.InnerPhone
                                    }).ToList<object>();
                                    if (lstObjs.Count > 50)
                                    {
                                        int page = 0;
                                        int total = lstObjs.Count;
                                        if (total % 50 == 0)
                                        {
                                            page = total / 50;
                                        }
                                        else
                                        {
                                            page = total / 50 + 1;
                                        }
                                        for (int j = 0; j < page; j++)
                                        {
                                            Busines.JPush.JPushApi.PushMessage(lstObjs.Skip(j * 50).Take(50), 1);
                                        }
                                    }
                                    else
                                    {
                                        Busines.JPush.JPushApi.PushMessage(lstObjs, 1);

                                    }
                                    logEntity = new LogEntity();
                                    logEntity.CategoryId = 5;
                                    logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                                    logEntity.OperateType = "删除";
                                    logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                                    logEntity.OperateUserId = user.UserId;

                                    logEntity.ExecuteResult = 1;
                                    logEntity.ExecuteResultJson = string.Format("同步用户(删除)到java培训平台,同步信息:\r\n{0}", lstUsers.ToJson());
                                    logEntity.Module = "人员档案";
                                    logEntity.ModuleId = "";
                                    logEntity.WriteLog();
                                });
                            }

                        }
                        //同步删除安防平台中部门与该部门下用户
                        DelHikDept(keyValue, lstUsers);
                    }
                }
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        private void DeleteDept(DepartmentEntity dept, string account)
        {

            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //当前操作用户账号
                nc.Add("account", account);
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(dept));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "DeleteDept?keyValue=" + dept.DepartmentId), nc);

            }
            catch (Exception ex)
            {
                if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
                }
                //将同步结果写入日志文件
                string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：删除部门失败，部门信息" + Newtonsoft.Json.JsonConvert.SerializeObject(dept) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
            }
            //将同步结果写入日志文件
            string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                var error = e.Error;
                if (error == null)
                {
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：返回结果>" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步部门发生异常>" + ex.Message + "\r\n");
            }


        }
        /// <summary>
        /// 保存部门表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="departmentEntity">部门实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)部门信息")]
        public ActionResult SaveForm(string keyValue, DepartmentEntity departmentEntity)
        {
            try
            {
                departmentEntity.DepartmentId = string.IsNullOrEmpty(keyValue) ? Guid.NewGuid().ToString() : keyValue;
                int count = departmentBLL.GetDataTable(string.Format("select count(1) from BASE_DEPARTMENT where parentid='{0}' and fullname='{2}' and DepartmentId!='{1}' ", departmentEntity.ParentId, departmentEntity.DepartmentId, departmentEntity.FullName.Trim())).Rows[0][0].ToInt();
                if (count > 0)
                {
                    return Error("该部门名称已经存在！");
                }
                string isnew = "";//判断是否是新增 如果是新增部门此字段为空
                DepartmentEntity deptentity = departmentBLL.GetEntity(keyValue);
                if (deptentity != null)
                {
                    isnew = deptentity.DepartmentId;
                }
                DepartmentEntity dept = departmentBLL.GetEntity(departmentEntity.DepartmentId);
                string action = dept == null ? "add" : "edit";
                DataItemDetailBLL di = new DataItemDetailBLL();
                bool result = departmentBLL.SaveForm(keyValue, departmentEntity);
                if (result)
                {
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                    {
                        if (string.IsNullOrWhiteSpace(departmentEntity.ParentId))
                        {
                            departmentEntity.ParentId = "-1";
                        }
                        var task = Task.Factory.StartNew(() =>
                        {
                            List<DepartmentEntity> lstDepts = new List<DepartmentEntity>();
                            lstDepts.Add(departmentEntity);
                            SaveDept(lstDepts, user.Account);
                        });

                    }
                    if (departmentEntity.IsTrain == 1)
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            departmentBLL.SyncDept(departmentEntity, keyValue);
                        });

                    }
                    if (departmentEntity.IsTools == 1 && !string.IsNullOrWhiteSpace(departmentEntity.ToolsKey))
                    {
                        string[] arr = departmentEntity.ToolsKey.Split('|');
                        if (arr.Length == 3)
                        {
                            var task = Task.Factory.StartNew(() =>
                            {
                                SyncToolsUsers(departmentEntity, keyValue, arr);
                            });
                        }

                    }
                    DepartmentEntity org = departmentBLL.GetEntity(departmentEntity.OrganizeId);
                    if (org != null)
                    {
                        if (org.IsTrain == 1)
                        {
                            string way = di.GetItemValue("WhatWay");
                            //对接.net培训平台
                            if (way == "0")
                            {

                            }
                            //对接java培训平台
                            if (way == "1")
                            {
                                string deptId = departmentEntity.DepartmentId;
                                dept = departmentBLL.GetEntity(departmentEntity.DepartmentId);
                                if (dept != null)
                                {
                                    string enCode = dept.EnCode;
                                    if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                    {
                                        string[] arr = dept.DeptKey.Split('|');
                                        if (arr.Length == 2)
                                        {
                                            deptId = arr[0];
                                            enCode = arr[1];
                                        }
                                    }
                                    string parentId = dept.ParentId;
                                    DepartmentEntity parentDept = departmentBLL.GetEntity(parentId);

                                    if (!string.IsNullOrWhiteSpace(parentDept.DeptKey))
                                    {
                                        string[] arr = parentDept.DeptKey.Split('|');
                                        if (arr.Length == 2)
                                        {
                                            parentId = arr[0];
                                        }
                                    }

                                    string ModuleName = SystemInfo.CurrentModuleName;
                                    string ModuleId = SystemInfo.CurrentModuleId;
                                    Task.Factory.StartNew(() =>
                                    {
                                        if (dept != null)
                                        {
                                            object obj = new
                                            {
                                                action = action,
                                                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                deptId = deptId,
                                                deptCode = enCode,
                                                deptName = dept.FullName,
                                                parentId = parentId,
                                                deptType = dept.Nature,
                                                unitKind = dept.DeptType,
                                                sort = dept.SortCode,
                                                companyId = org.InnerPhone
                                            };
                                            List<object> list = new List<object>();
                                            list.Add(obj);
                                            Busines.JPush.JPushApi.PushMessage(list, 0);


                                            LogEntity logEntity = new LogEntity();
                                            logEntity.CategoryId = 5;
                                            logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                                            logEntity.OperateType = "新增或保存";
                                            logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                                            logEntity.OperateUserId = user.UserId;

                                            logEntity.ExecuteResult = 1;
                                            logEntity.ExecuteResultJson = string.Format("同步部门到java培训平台,同步信息：{0}", list.ToJson());
                                            logEntity.Module = ModuleName;
                                            logEntity.ModuleId = ModuleId;
                                            logEntity.WriteLog();
                                        }
                                    });
                                }

                            }
                        }
                    }
                    //如果是可门双控 则把新增/修改操作同步到海康平台
                    SyncHik(isnew, departmentEntity.DepartmentId);
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作失败");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 推送指定部门下所有用户到班组
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "批量同步用户到班组")]
        public ActionResult SyncUsersToBZ(string deptId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deptId))
                {
                    return Error("参数deptId不能为空!");
                }
                DepartmentEntity dept = departmentBLL.GetEntity(deptId);
                if (dept == null)
                {
                    return Error("部门不存在!");
                }
                var expression = LinqExtensions.True<UserEntity>();
                expression = expression.And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0).And(t => t.DepartmentCode.StartsWith(dept.EnCode));
                var users = userBLL.GetListForCon(expression);
                string result = userBLL.SyncUsersToBZ(users.ToList(), 1);
                return string.IsNullOrWhiteSpace(result) ? Success("操作成功") : Error(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "同步部门信息到培训平台")]
        public ActionResult InitDepts(string keyValue)
        {
            DataItemDetailBLL di = new DataItemDetailBLL();
            DepartmentEntity dept = departmentBLL.GetEntity(keyValue);
            DepartmentEntity org = departmentBLL.GetEntity(dept.OrganizeId);
            if (org.IsTrain == 1)
            {
                string way = di.GetItemValue("WhatWay");
                //对接。net培训平台
                if (way == "0")
                {
                    string url = di.GetItemValue("TrainServiceUrl");
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        return Error("请先配置培训平台服务地址！");
                    }
                    else
                    {

                        if (dept.IsTrain == 1)
                        {
                            return SyncDept(dept.DepartmentId, dept.EnCode, dept.DeptKey);

                        }
                        else
                        {
                            return Error("操作失败，该部门未配置对接培训平台！");
                        }

                    }
                }
                //对接java培训平台
                else if (way == "1")
                {
                    string ModuleName = SystemInfo.CurrentModuleName;
                    string ModuleId = SystemInfo.CurrentModuleId;
                    try
                    {
                        var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        List<object> list = departmentBLL.GetDepts(keyValue).Select(t => new
                        {
                            action = "add",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            deptId = string.IsNullOrWhiteSpace(t.DeptKey) ? t.DepartmentId : t.DeptKey.Split('|')[0],
                            deptCode = string.IsNullOrWhiteSpace(t.DeptKey) ? t.EnCode : t.DeptKey.Split('|')[1],
                            deptName = t.FullName,
                            parentId = string.IsNullOrWhiteSpace(departmentBLL.GetEntity(t.ParentId).DeptKey) ? t.ParentId : departmentBLL.GetEntity(t.ParentId).DeptKey.Split('|')[0],
                            deptType = t.Nature,
                            unitKind = dept.DeptType,
                            sort = t.SortCode,
                            companyId = org.InnerPhone
                        }).ToList<object>();
                        Busines.JPush.JPushApi.PushMessage(list, 0);

                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 3;
                        logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                        logEntity.OperateType = "同步部门";
                        logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                        logEntity.OperateUserId = user.UserId;

                        logEntity.ExecuteResult = 1;
                        logEntity.ExecuteResultJson = string.Format("同步部门到java培训平台,同步信息:\r\n{0}", list.ToJson());
                        logEntity.Module = ModuleName;
                        logEntity.ModuleId = ModuleId;
                        logEntity.WriteLog();


                        var expression = LinqExtensions.True<UserEntity>();
                        expression = expression.And(t => t.DepartmentCode.StartsWith(dept.EnCode)).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
                        list = userBLL.GetListForCon(expression).Select(t => new
                        {
                            action = "add",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            userId = t.UserId,
                            userName = t.RealName,
                            account = t.Account,
                            deptId = string.IsNullOrWhiteSpace(departmentBLL.GetEntity(t.DepartmentId).DeptKey) ? t.DepartmentId : departmentBLL.GetEntity(t.DepartmentId).DeptKey.Split('|')[0],
                            deptCode = string.IsNullOrWhiteSpace(departmentBLL.GetEntity(t.DepartmentId).DeptKey) ? t.DepartmentCode : departmentBLL.GetEntity(t.DepartmentId).DeptKey.Split('|')[1],
                            //password = string.IsNullOrWhiteSpace(t.NewPassword)?"123456":DESEncrypt.Decrypt(t.NewPassword, t.Secretkey),
                            password = "123456",
                            sex = t.Gender,
                            idCard = t.IdentifyID,
                            email = t.Email,
                            mobile = t.Mobile,
                            birth = t.Birthday == null ? "" : t.Birthday.Value.ToString("yyyy-MM-dd"),//生日
                            postId = t.DutyId,
                            postName = t.DutyName,
                            age = t.Age.ToIntOrNull(),
                            native = t.Native,
                            nation = t.Nation,
                            jobTitle = t.JobTitle,
                            techLevel = t.TechnicalGrade,
                            workType = t.Craft,
                            companyId = org.InnerPhone,
                            //signContent=userBLL.GetSignContent(t.SignImg),//人员签名照
                            trainRoles = t.TrainRoleId,
                            role = t.IsTrainAdmin == null ? 0 : t.IsTrainAdmin //角色（0:学员，1:培训管理员）
                        }).ToList<object>();
                        if (list.Count > 50)
                        {
                            int page = 0;
                            int total = list.Count;
                            if (total % 50 == 0)
                            {
                                page = total / 50;
                            }
                            else
                            {
                                page = total / 50 + 1;
                            }
                            for (int j = 0; j < page; j++)
                            {
                                Busines.JPush.JPushApi.PushMessage(list.Skip(j * 50).Take(50), 1);
                                logEntity = new LogEntity();
                                logEntity.CategoryId = 5;
                                logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                                logEntity.OperateType = "同步用户";
                                logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                                logEntity.OperateUserId = user.UserId;

                                logEntity.ExecuteResult = 1;
                                logEntity.ExecuteResultJson = string.Format("同步用户到java培训平台,同步信息:\r\n{0}", list.ToJson());
                                logEntity.Module = ModuleName;
                                logEntity.ModuleId = ModuleId;
                                logEntity.WriteLog();
                            }
                        }
                        else
                        {
                            Busines.JPush.JPushApi.PushMessage(list, 1);
                            logEntity = new LogEntity();
                            logEntity.CategoryId = 5;
                            logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                            logEntity.OperateType = "同步用户";
                            logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                            logEntity.OperateUserId = user.UserId;

                            logEntity.ExecuteResult = 1;
                            logEntity.ExecuteResultJson = string.Format("同步用户到java培训平台,同步信息:\r\n{0}", list.ToJson());
                            logEntity.Module = ModuleName;
                            logEntity.ModuleId = ModuleId;
                            logEntity.WriteLog();
                        }

                        return Success("操作成功！");
                    }
                    catch (Exception ex)
                    {
                        return Error(ex.Message);
                    }

                }
                else
                {
                    return Error("编码WhatWay配置不正确！");
                }
            }
            else
            {
                return Error("该单位没有配置为同步培训平台！");
            }
        }

        #region 同步到海康平台
        /// <summary>
        /// 同步部门/新增/修改到海康平台
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="id"></param>
        public void SyncHik(string keyValue, string id)
        {
            //先判断是不是可门电厂的双控
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            string HikHttps = itemBll.GetItemValue("HikHttps");
            if (!string.IsNullOrEmpty(KMIndex) || !string.IsNullOrEmpty(HikHttps))//如果是可门双控则进行同步
            {
                List<DepartmentEntity> deptlist = new List<DepartmentEntity>();
                DepartmentEntity dept = departmentBLL.GetEntity(id);
                deptlist.Add(dept);
                if (string.IsNullOrEmpty(keyValue))
                {
                    AddHikDept(deptlist);
                }
                else
                {
                    UpdateHikDept(deptlist);
                }
            }

        }

        /// <summary>
        /// 根据部门ID同步删除海康平台中的部门节点
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public void DelHikDept(string id, List<UserEntity> userlist)
        {
            //先判断是不是可门电厂的双控
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            string HikHttps = itemBll.GetItemValue("HikHttps");
            if (!string.IsNullOrEmpty(KMIndex) || !string.IsNullOrEmpty(HikHttps)) //如果是可门双控则进行同步
            {
                string rtnMsg = "";
                DataItemDetailBLL data = new DataItemDetailBLL();
                var pitem = data.GetItemValue("Hikappkey"); //海康服务器密钥
                var baseurl = data.GetItemValue("HikBaseUrl"); //海康服务器地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                //先删除部门下的用户
                List<string> personIds = new List<string>();
                string PersonUrl = "/artemis/api/resource/v1/person/batch/delete"; //接口地址
                if (userlist.Count > 0)
                {
                    foreach (var person in userlist)
                    {
                        personIds.Add(person.UserId);
                    }
                    var personmodel = new
                    {
                        personIds
                    };
                    if (!string.IsNullOrEmpty(HikHttps))
                    {
                        HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                        byte[] result = HttpUtillibKbs.HttpPost(PersonUrl, JsonConvert.SerializeObject(personmodel), 20);
                        if (result != null)
                        {
                            rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                        }
                    }
                    else
                    {
                        rtnMsg = SocketHelper.LoadCameraList(personmodel, baseurl, PersonUrl, Key, Signature);
                    }
                }

                //然后在删除部门
                string Url = "/artemis/api/resource/v1/org/batch/delete"; //接口地址
                List<string> delstring = new List<string>();
                delstring.Add(id);
                var model = new
                {
                    indexCodes = delstring
                };
                if (!string.IsNullOrEmpty(HikHttps))
                {
                    HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                    byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(model), 20);
                    if (result != null)
                    {
                        rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                    }
                }
                else
                {
                    rtnMsg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                }
            }
        }

        public string UpdateHikDept(List<DepartmentEntity> dept)
        {
            string rtnMsg = "";
            DataItemDetailBLL data = new DataItemDetailBLL();
            var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
            string HikHttps = data.GetItemValue("HikHttps");//海康1.4及以上版本https
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/org/single/update";//接口地址

            foreach (var item in dept)
            {
                var model = new
                {
                    orgIndexCode = item.DepartmentId,
                    orgName = item.FullName
                };

                if (!string.IsNullOrEmpty(HikHttps))
                {
                    HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                    byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(model), 20);
                    if (result != null)
                    {
                        rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                    }
                }
                else
                {
                    rtnMsg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                }
            }
            return rtnMsg;
        }

        /// <summary>
        /// 部分组织结构同步到海康平台
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public string AddHikDept(List<DepartmentEntity> dept)
        {
            DataItemDetailBLL data = new DataItemDetailBLL();
            var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
            string HikHttps = data.GetItemValue("HikHttps");//海康1.4及以上版本https
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/org/batch/add";//接口地址

            List<object> modelList = new List<object>();
            int i = 1;
            foreach (var item in dept)
            {
                string parentid = "root000000";
                if (item.ParentId != "0" && item.ParentId != "-1")
                {
                    parentid = item.ParentId;
                }
                else if (item.ParentId == "-1")//如果是-1是双控里面的组织架构 不进行录入
                {
                    continue;
                }
                var model = new
                {
                    clientId = i,
                    orgIndexCode = item.DepartmentId,
                    orgName = item.FullName,
                    parentIndexCode = parentid
                };
                modelList.Add(model);
                i++;
            }
            string rtnMsg = string.Empty;
            if (!string.IsNullOrEmpty(HikHttps))
            {
                HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(modelList), 20);
                if (result != null)
                {
                    rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                }
            }
            else
            {
                rtnMsg = SocketHelper.LoadCameraList(modelList, baseurl, Url, Key, Signature);
            }
            return rtnMsg;
        }

        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "同步部门信息到海康安防平台")]
        public ActionResult InitHikDepts()
        {
            List<DepartmentEntity> dept = departmentBLL.GetList().OrderBy(a => a.EnCode).ToList();

            string rtnMsg = AddHikDept(dept);

            DeptAddRtnMsg rtnList = JsonConvert.DeserializeObject<DeptAddRtnMsg>(rtnMsg);

            if (rtnList.data.successes.Count > 0 && rtnList.data.failures.Count == 0)
            {
                return Success("操作成功。");
            }
            else
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步安防平台数据失败，异常信息：" + rtnMsg + "\r\n");
                return Success("部分同步失败,具体情况请查询日志。");

            }



        }

        #endregion

        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "获取培训平台基础数据")]
        public ActionResult SyncDept(string deptId, string deptCode, string deptKey)
        {
            string url = new DataItemDetailBLL().GetItemValue("TrainServiceUrl");
            if (string.IsNullOrWhiteSpace(url))
            {
                return Error("请先配置培训平台服务地址！");
            }
            string depts = null;
            string users = null;
            try
            {
                string[] arr = deptKey.Split('|');
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                wc.Encoding = Encoding.UTF8;
                //发送请求到web api并获取返回值，默认为post方式


                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "GetDeptInfo", DeptId = arr[0] });
                depts = wc.DownloadString(new Uri(url + "?json=" + json));
                json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "GetUserInfo", DeptId = arr[0] });
                users = wc.DownloadString(new Uri(url + "?json=" + json));
                string result = departmentBLL.SyncDeptForTrain(deptId, deptCode, depts, users, deptKey);
                if (string.IsNullOrEmpty(result))
                {

                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs")))
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs"));
                    }
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步信息：" + depts + "\r\n," + users);
                    return Success("操作成功。");
                }
                else
                {
                    return Error(result);
                }

            }
            catch (Exception ex)
            {
                if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs"));
                }
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息：" + depts + "\r\n" + users + "\r\n异常信息：" + ex.Message + "\r\n");
                return Error(ex.Message);
            }

        }
        private void SaveDept(List<DepartmentEntity> list, string account)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/json");
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                wc.UploadStringCompleted += wc_UploadStringCompleted;
                wc.UploadStringAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "PostDepartments "), "Post", list.ToJson());

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息" + list.ToJson() + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        public void SyncToolsUsers(DepartmentEntity dept, string keyValue, string[] arr)
        {

            SstmsService.CheckHeader header = new SstmsService.CheckHeader();
            header.Key = arr[2];
            List<SstmsService.DictionaryEntry> dic = new List<SstmsService.DictionaryEntry>();
            SstmsService.DictionaryEntry de = new SstmsService.DictionaryEntry();
            de.Key = "LaterID";
            de.Value = arr[0];
            dic.Add(de);
            de = new SstmsService.DictionaryEntry();
            de.Key = "Identify";
            de.Value = "";
            dic.Add(de);
            // 实例化服务对象
            SstmsService.DataServiceSoapClient service = new SstmsService.DataServiceSoapClient();
            SstmsService.PersonInfo[] data = service.GetPersons(header);
            //SstmsService.PersonInfo[] data = service.GetPersonsPage(header, dic.ToArray<SstmsService.DictionaryEntry>(), 1, 1000).List;
            DepartmentEntity org = departmentBLL.GetEntity(dept.OrganizeId);
            IList<UserEntity> listUsers = new List<UserEntity>();
            string path = new DataItemDetailBLL().GetItemValue("imgPath");
            foreach (SstmsService.PersonInfo per in data)
            {
                string unitId = per.OwnerDeptID;
                string userId = "";
                string roleName = "承包商级用户";
                string roleId = "c5530ccf-e84e-4df8-8b27-fd8954a9bbe9";
                DataTable dt = departmentBLL.GetDataTable(string.Format("select nature,deptid,a.deptcode,unitid from BIS_TOOLSDEPT a left join BASE_DEPARTMENT b on a.deptid=b.departmentid where unitid='{1}' and  a.deptcode like '{0}%'", dept.EnCode, per.UnitID));
                foreach (DataRow dr in dt.Rows)
                {
                    string deptId = dr["deptid"].ToString();
                    string deptCode = dr["deptcode"].ToString();
                    string nature = dr["nature"].ToString();
                    unitId = dr["unitid"].ToString();
                    if (unitId == per.UnitID)
                    {
                        switch (nature)
                        {
                            case "部门":
                                roleName = "部门级用户";
                                roleId = "6c094cef-cca3-4b41-a71b-6ee5e6b89008";
                                break;
                            case "专业":
                                roleName = "专业级用户";
                                roleId = "e3062d59-2484-4046-a420-478886d58656";
                                break;
                            case "班组":
                                roleName = "班组级用户";
                                roleId = "d9432a6e-5659-4f04-9c10-251654199714";
                                break;
                            case "厂级":
                                roleName = "公司级用户";
                                roleId = "aece6d68-ef8a-4eac-a746-e97f0067fab5";
                                break;
                            case "省级":
                                roleName = "省级用户";
                                roleId = "9a834c93-ff60-440e-845d-79b311eeacae";
                                break;
                        }
                        roleName += ",普通用户";
                        roleId += ",2a878044-06e9-4fe4-89f0-ba7bd5a1bde6";
                        UserEntity user = userBLL.GetUserByIdCard(per.IdentifyID);
                        if (user == null)
                        {
                            //    userId = user.UserId;
                            //}
                            //else
                            //{
                            user = new UserEntity();
                            user.UserId = Guid.NewGuid().ToString();
                            user.Account = per.IdentifyID;
                            userId = user.UserId;
                            //}
                            user.MSN = "1";
                            user.UserType = "一般工作人员";
                            user.EnCode = per.TraID;
                            user.Degrees = user.DegreesID = per.Degrees;
                            user.Birthday = per.BirthDay;
                            user.RoleId = roleId;
                            user.RoleName = roleName;
                            user.IdentifyID = per.IdentifyID;
                            user.Gender = per.Sex;
                            user.Nation = per.Nation;
                            user.Email = per.Email;
                            user.EnterTime = per.EntranceDate;
                            user.RealName = per.PersonName;
                            user.Degrees = per.Degrees;
                            user.Native = per.Native;
                            user.DepartureTime = per.LeaveDate;
                            user.Telephone = per.TelPhone;
                            user.IsPresence = per.IsOut == "是" ? "0" : "1";
                            user.Password = "123456";
                            user.DepartmentId = deptId;
                            user.DepartmentCode = deptCode;
                            user.OrganizeId = org.DepartmentId;
                            user.OrganizeCode = org.EnCode;
                            user.Craft = per.Category;
                            user.IsEpiboly = roleName.Contains("承包商") || roleName.Contains("分包商") ? "1" : "0";
                            if (string.IsNullOrWhiteSpace(user.HeadIcon))
                            {
                                byte[] bytes = service.GetPersonPicture(header, per.ID);
                                if (bytes.Length > 0)
                                {
                                    string headIcon = Guid.NewGuid().ToString() + ".png";
                                    FileStream fs = new FileStream(path + "\\Resource\\PhotoFile\\" + headIcon, FileMode.Create, FileAccess.ReadWrite);
                                    fs.Write(bytes, 0, bytes.Length);
                                    fs.Close();
                                    user.HeadIcon = "/Resource/PhotoFile/" + headIcon;
                                }
                            }
                            if (user.ModifyDate != null)
                            {
                                if (per.OperDate > user.ModifyDate)
                                {
                                    userBLL.SaveForm(userId, user);
                                }
                            }
                            else
                            {
                                userBLL.SaveForm(userId, user);
                            }
                            user.Password = "123456";
                            listUsers.Add(user);
                        }
                    }

                }
            }
            if (listUsers.Count > 0)
            {
                if (!string.IsNullOrEmpty(new DataItemDetailBLL().GetItemValue("bzAppUrl")))
                {
                    Task.Factory.StartNew(() =>
                    {
                        ImportUser(listUsers);
                    });
                }
            }
        }
        private void ImportUser(IList<UserEntity> userList)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //当前操作用户账号
                nc.Add("account", "System");
                foreach (UserEntity item in userList)
                {
                    //用户信息
                    item.Gender = item.Gender == "男" ? "1" : "0";
                    if (item.RoleName.Contains("班组级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "a1b68f78-ec97-47e0-b433-2ec4a5368f72";
                            item.RoleName = "班组长";
                        }
                        else
                        {
                            item.RoleId = "e503d929-daa6-472d-bb03-42533a11f9c6";
                            item.RoleName = "班组成员";
                        }
                    }
                    if (item.RoleName.Contains("部门级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "1266af38-9c0a-4eca-a04a-9829bc2ee92d";
                            item.RoleName = "部门管理员";
                        }
                        else
                        {
                            item.RoleId = "3a4b56ac-6207-429d-ac07-28ab49dca4a6";
                            item.RoleName = "部门级用户";
                        }
                    }
                    if (item.RoleName.Contains("公司级用户"))
                    {
                        //if (user.RoleName.Contains("负责人"))
                        //{
                        item.RoleId = "97869267-e5eb-4f20-89bd-61e7202c4ecd";
                        item.RoleName = "厂级管理员";
                        // }

                    }
                    if (item.EnterTime == null)
                    {
                        item.EnterTime = DateTime.Now;
                    }
                }

                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(userList));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "SaveUsers"), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(userList) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            var error = e.Error;
            //将同步结果写入日志文件
            string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                if (error == null)
                {
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：返回结果>" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
                }

            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步发生错误,错误信息>" + msg + "\r\n");
            }

        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var error = e.Error;
            //将同步结果写入日志文件
            string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
            }
            try
            {
                if (error == null)
                {
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：返回结果>" + e.Result + "\r\n");
                }
            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步发生错误,错误信息>" + msg + "\r\n");
            }

        }

        /// <summary>
        /// 导入岗位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDept(string OrganizeId)
        {
            int error = 0;
            var user = OperatorProvider.Provider.Current();
            string orgId = user.OrganizeId;
            if (!string.IsNullOrEmpty(OrganizeId))
            {
                orgId = OrganizeId;
            }
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
                DataItemDetailBLL di = new DataItemDetailBLL();
                int order = 1;

                string way = di.GetItemValue("WhatWay");
                DepartmentEntity org = departmentBLL.GetEntity(orgId);
                List<object> lstDepts = new List<object>();
                List<DepartmentEntity> lstBzDepts = new List<DepartmentEntity>();
                string bzApiUrl = di.GetItemValue("bzAppUrl");
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //部门名称
                    string fullName = dt.Rows[i][0].ToString();
                    //验证部门名称不能为重复
                    //if (!departmentBLL.ExistDeptJugement(fullName, OrganizeId))
                    //{
                    //    falseMessage += "</br>" + "第" + (i + 2) + "行部门已存在,未能导入.";
                    //    error++;
                    //    continue;
                    //}
                    //部门编码

                    string parentName = dt.Rows[i][2].ToString().Trim();//上级部门名称

                    List<string> list = departmentBLL.GetImportDeptCode(parentName, orgId);
                    if (list[0] == "-1")
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行上级部门名称存在错误,未能导入.";
                        error++;
                        continue;
                    }

                    //上级id
                    string parentid = list[0];
                    //部门类型
                    string nature = dt.Rows[i][1].ToString();
                    //部门类型
                    string deptType = dt.Rows[i][3].ToString();
                    if (string.IsNullOrEmpty(nature) || string.IsNullOrEmpty(fullName))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    else
                    {
                        if (nature.Contains("包商") && !deptType.Contains("外包单位"))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "部门类型选择错误.";
                            error++;
                            continue;
                        }
                        if (!nature.Contains("包商") && deptType.Contains("外包单位"))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "部门类型选择错误.";
                            error++;
                            continue;
                        }
                    }

                    //部门职责
                    string description = dt.Rows[i][4].ToString();

                    string deptId = Guid.NewGuid().ToString();
                    DepartmentEntity dept = new DepartmentEntity();
                    dept.DepartmentId = deptId;
                    dept.FullName = fullName;
                    dept.DepartDuty = description;
                    dept.SortCode = order;
                    dept.OrganizeId = orgId;
                    dept.DeptType = deptType.Replace("承包商", "");
                    dept.Nature = nature;
                    dept.EnabledMark = 1;
                    dept.ParentId = parentid;
                    string action = "add";

                    //判断同层级是否存在相同的部门名称
                    int number = departmentBLL.GetDataTable(string.Format("select count(1) from BASE_DEPARTMENT where parentid='{0}' and fullname='{2}' and DepartmentId!='{1}' ", dept.ParentId, dept.DepartmentId, fullName.Trim())).Rows[0][0].ToInt();
                    if (number > 0)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在重复,未能导入.";
                        error++;
                        continue;
                    }

                    try
                    {
                        DataTable dtDept = departmentBLL.GetDataTable(string.Format("select DepartmentId from BASE_DEPARTMENT where parentid='{0}' and fullname='{1}'", parentid, fullName));
                        if (dtDept.Rows.Count > 0)
                        {
                            deptId = dtDept.Rows[0][0].ToString();
                            action = "edit";
                        }
                        bool result = departmentBLL.SaveForm(deptId, dept);
                        if (result)
                        {
                            lstBzDepts.Add(dept);
                        }
                        //对接.net培训平台
                        if (way == "0")
                        {

                        }
                        //对接java培训平台
                        if (way == "1")
                        {
                            if (org.IsTrain == 1)
                            {
                                dept = departmentBLL.GetEntity(deptId);
                                if (dept != null)
                                {
                                    string enCode = dept.EnCode;
                                    string parentId = dept.ParentId;
                                    if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                    {
                                        string[] arr = dept.DeptKey.Split('|');
                                        if (arr.Length > 1)
                                        {
                                            deptId = arr[0];
                                            enCode = arr[1];
                                        }
                                    }

                                    DepartmentEntity parentDept = departmentBLL.GetEntity(parentId);
                                    if (parentDept != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(parentDept.DeptKey))
                                        {
                                            parentId = parentDept.DeptKey.Split('|')[0];
                                        }
                                    }

                                    object obj = new
                                    {
                                        action = action,
                                        time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        deptId = deptId,
                                        deptCode = enCode,
                                        deptName = dept.FullName,
                                        parentId = parentId,
                                        deptType = dept.Nature,
                                        unitKind = dept.DeptType,
                                        sort = dept.SortCode,
                                        companyId = org.InnerPhone
                                    };
                                    lstDepts.Add(obj);

                                }
                            }
                        }
                    }
                    catch
                    {
                        error++;
                    }

                }
                if (way == "1" && lstDepts.Count > 0)
                {
                    Busines.JPush.JPushApi.PushMessage(lstDepts, 0);

                    string moduleId = SystemInfo.CurrentModuleId;
                    string moduleName = SystemInfo.CurrentModuleName;
                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 5;
                    logEntity.OperateTypeId = ((int)OperationType.Delete).ToString();
                    logEntity.OperateType = "同步部门";
                    logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                    logEntity.OperateUserId = user.UserId;

                    logEntity.ExecuteResult = -1;
                    logEntity.ExecuteResultJson = string.Format("同步部门(导入)到java培训平台,同步信息:\r\n{0}", lstDepts.ToJson());
                    logEntity.Module = moduleName;
                    logEntity.ModuleId = moduleId;
                    logEntity.WriteLog();
                }
                if (lstBzDepts.Count > 0 && !string.IsNullOrWhiteSpace(bzApiUrl))
                {
                    SaveDept(lstBzDepts, user.Account);
                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }

        #endregion

        #region 根据查询条件获取部门树

        /// <summary>
        /// 根据查询条件获取部门树 
        /// </summary>
        /// <param name="Ids">上级部门(机构)Id</param>
        /// <param name="deptIds">页面带过来的部门ids</param>
        /// <param name="keyword">关键字</param>
        /// <param name="checkMode">单选或多选，0:单选，1:多选</param>
        /// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即OrganizeId=Ids)，1:获取部门ID为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即ParentId in(Ids))，3:获取部门Id在Ids中的部门(即DepartmentId in(Ids)),8：获取单位及子部门、关联的承包商单位，10:当前用户所属部门及下属部门</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetDeptTreeJson(string Ids, int checkMode = 0, int mode = 0, string deptIds = "0")
        {
            string parentId = "0";
            OrganizeBLL orgBLL = new OrganizeBLL();
            var treeList = new List<TreeEntity>();
            List<DepartmentEntity> data = new List<DepartmentEntity>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //本部门及下属部门
            if (mode == 10)
            {
                parentId = user.OrganizeId;
                data = departmentCache.GetList().Where(t => t.EnCode.Contains(user.DeptCode)).OrderBy(t => t.SortCode).ToList();
            }
            else if (mode == 11)
            {
                parentId = user.OrganizeId;
                //List<OrganizeEntity> list = orgBLL.GetList().Where(t => t.OrganizeId == user.OrganizeId).ToList();
                //foreach (OrganizeEntity oe in list)
                //{
                //    treeList.Add(new TreeEntity
                //    {
                //        id = oe.OrganizeId,
                //        text = oe.FullName,
                //        value = oe.EnCode,
                //        parentId = oe.ParentId,
                //        isexpand = true,
                //        complete = true,
                //        showcheck = false,
                //        hasChildren = true,
                //        Attribute = "Sort",
                //        AttributeValue = "Organize",
                //        AttributeA = "manager",
                //        AttributeValueA = oe.Manager + "," + oe.ManagerId + ",1"
                //    });
                //}
                data = departmentCache.GetList().Where(t => t.EnCode.Contains(user.OrganizeCode) && t.Description != "外包工程承包商").OrderBy(t => t.SortCode).ToList();
            }
            else
            {
                //如果没有传递参数parentId,则给出默认值
                if (string.IsNullOrEmpty(Ids))
                {
                    parentId = OperatorProvider.Provider.Current().OrganizeId;
                    //如果是根据机构ID获取部门
                    if (mode == 0)
                    {
                        if (user.IsSystem)
                        {
                            data = departmentCache.GetList().OrderBy(t => t.SortCode).ToList();
                        }
                        else
                        {
                            //parentId默认为当前用户所属的机构ID
                            data = departmentCache.GetList(parentId).OrderBy(t => t.SortCode).ToList();
                        }
                    }
                    else if (mode == 1)
                    {
                        //parentId默认为当前用户所属的部门ID
                        Ids = OperatorProvider.Provider.Current().DeptId;
                        data = departmentCache.GetDeptList(Ids).OrderBy(t => t.SortCode).ToList();
                    }
                    else
                    {
                        if (mode == 4)
                        {
                            DepartmentEntity deptC = departmentBLL.GetEntity(OperatorProvider.Provider.Current().DeptId);
                            var dept = OperatorProvider.Provider.Current();
                            if (dept.IsSystem)//系统管理员
                            {
                                data = departmentCache.GetDeptList(Ids, mode).Where(x => x.Nature == "承包商" || x.Nature == "分包商" || (x.Nature == "部门" && x.Description == "外包工程承包商")).OrderBy(x => x.SortCode).ToList();
                            }
                            else if (dept.DeptCode == user.OrganizeCode)//机构
                            {
                                data = departmentCache.GetDeptList(Ids, mode).Where(x => x.Nature == "承包商" || x.Nature == "分包商" || (x.Nature == "部门" && x.Description == "外包工程承包商") && x.OrganizeId == dept.OrganizeId).OrderBy(x => x.SortCode).ToList();

                            }
                            else if (dept.RoleName.Contains("承包商"))//承包商用户
                            {
                                data = departmentCache.GetDeptList(Ids, mode).Where(x => x.DepartmentId == deptC.DepartmentId || x.Nature == "分包商" || (x.Nature == "部门" && x.Description == "外包工程承包商")).OrderBy(x => x.SortCode).ToList();
                            }
                            else if (dept.RoleName.Contains("分包商"))//分包商用户
                            {
                                data = departmentCache.GetDeptList(Ids, mode).Where(x => x.DepartmentId == deptC.DepartmentId).OrderBy(t => t.SortCode).ToList();
                                data = data.Where(t => t.Nature == "部门" || t.Nature == "专业" || t.Nature == "班组" || t.Nature == "承包商" || t.Nature == "分包商").ToList();
                                foreach (DepartmentEntity item in data)
                                {
                                    item.ParentId = item.Nature == "部门" ? "0" : item.ParentId;
                                    TreeEntity tree = new TreeEntity();
                                    bool hasChildren = data.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                                    tree.id = item.DepartmentId;
                                    tree.text = item.FullName;
                                    tree.value = item.EnCode;
                                    tree.isexpand = true;
                                    tree.complete = true;
                                    tree.showcheck = checkMode == 0 ? false : true;
                                    tree.hasChildren = hasChildren;
                                    tree.parentId = item.ParentId == "0" ? item.OrganizeId : item.ParentId;
                                    tree.Attribute = "Sort";
                                    tree.AttributeValue = "Department";
                                    tree.AttributeA = "manager";
                                    tree.AttributeValueA = item.Manager + "," + item.ManagerId + "," + item.IsOrg;
                                    treeList.Add(tree);
                                }
                                return Content(treeList.ToJson());
                            }
                            else
                            {
                                if (deptC != null && deptC.IsOrg != 1)//厂级部门
                                {
                                    string departIds = GetDeptIds(deptC.DepartmentId);
                                    data = departmentCache.GetDeptList(Ids, mode).Where(x => (departIds.Contains(x.DepartmentId) && x.DepartmentId != "0") || x.Nature == "分包商" || (x.Nature == "部门" && x.Description == "外包工程承包商")).OrderBy(x => x.SortCode).ToList();
                                }
                                else//普通部门
                                    data = departmentCache.GetDeptList(Ids, mode).Where(x => x.Nature == "承包商" || x.Nature == "分包商" || (x.Nature == "部门" && x.Description == "外包工程承包商") && x.OrganizeId == dept.OrganizeId).OrderBy(x => x.SortCode).ToList();

                            }

                        }
                        else
                        {
                            data = departmentCache.GetDeptList(Ids, mode).OrderBy(x => x.SortCode).ToList();
                        }

                    }

                }
                else
                {
                    parentId = Ids;
                    if (mode == 0)
                    {
                        data = departmentCache.GetList(Ids).OrderBy(x => x.SortCode).ToList();
                    }
                    else if (mode == 1)
                    {

                        data = departmentCache.GetDeptList(Ids).OrderBy(x => x.SortCode).ToList();
                    }
                    else
                    {
                        data = departmentCache.GetDeptList(Ids, mode).OrderBy(x => x.SortCode).ToList();
                    }
                }
            }
            data = data.Where(t => t.Nature == "部门" || t.Nature == "专业" || t.Nature == "班组" || t.Nature == "承包商" || t.Nature == "分包商").ToList();
            foreach (DepartmentEntity item in data)
            {
                item.ParentId = item.Nature == "部门" ? "0" : item.ParentId;
                int chkState = 0;
                string[] arrids = deptIds.Split(',');
                if (arrids.Contains(item.EnCode))
                {
                    chkState = 1;
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                tree.img = item.Nature == "承包商" ? item.SendDeptID : null;//借用该字段，查发包的承包商。
                tree.isexpand = false;
                tree.complete = true;
                tree.checkstate = chkState;
                tree.showcheck = checkMode == 0 ? false : true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId == "0" ? item.OrganizeId : item.ParentId;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Department";
                tree.AttributeA = "manager";
                tree.AttributeValueA = item.Manager + "," + item.ManagerId + "," + item.IsOrg;
                treeList.Add(tree);
            }
            //除厂级单位外，取本单位及子单位和发包的承包商数据 yuguolei-20180803     
            string untakerJson = "";
            if (parentId != "-1")
            {
                var untaker = treeList.FirstOrDefault(x => x.img == parentId);
                if (untaker != null)
                {//发包部门的承包商
                    untakerJson = treeList.TreeToJson(untaker.parentId);
                }

                var tre = treeList.FirstOrDefault(x => x.id == parentId);
                if (tre != null)
                    parentId = tre.parentId;

            }
            if (treeList.Count > 0)
            {
                treeList[0].isexpand = true;
            }
            var json = treeList.TreeToJson(parentId);

            if (!string.IsNullOrEmpty(untakerJson))//拼接发包部门的承包商
                json = json.Substring(0, json.Length - 1) + "," + untakerJson.Substring(1);

            return Content(json);
        }
        /// <summary>
        /// 根据查询条件获取部门树 
        /// </summary>
        ///<param name="json">json查询条件,字段说明:
        ///Ids:单位Id，多个逗号分隔
        ///DeptIds:页面带过来的部门id,多个用逗号分隔(以设置默认选中状态)
        ///KeyWord:部门名称查询关键字
        ///SelectMode:单选或多选，0:单选，1:多选
        ///Mode:查询模式（1:获取部门Id为ids下的所有子部门,2:获取部门Id包含在Ids中的部门（不含本单位）,3:获取当前用户所在单位下的所有子部门（含本单位）,4:获取当前用户所在单位下的所有子部门（含本单位但不包含承包商和分包商）,5:获取省级下所有厂级(前提是当前用户属于省级用户),
        ///6:获取省级本部部门和下属所有厂级(前提是当前用户属于省级用户),7:获取当前用户下的管辖的承包商(前提是当前用户部门属于厂级下的部门),8:获取当前用户所在厂级下的所有承包商(前提是当前用户部门属于厂级下的部门)
        ///</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetDepartTreeJson(string json, string selectCode = "", string deptType = "")
        {
            ConditionJson con = JsonConvert.DeserializeObject<ConditionJson>(json);
            string Ids = con.Ids;
            int checkMode = con.SelectMode;
            int mode = con.Mode;
            string deptIds = con.DeptIds;
            string keyword = con.KeyWord;
            string departmentCode = json.Contains("DepartmentCode") ? con.DepartmentCode : "";
            string parentId = "0";
            string str = "";
            OrganizeBLL orgBLL = new OrganizeBLL();
            var treeList = new List<TreeEntity>();
            IEnumerable<DepartmentEntity> data = new List<DepartmentEntity>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem && string.IsNullOrEmpty(Ids) && mode != 13 && mode != 24 && mode != 25)
            {
                data = departmentBLL.GetList().Where(t => t.DepartmentId != "0").OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
            }
            else
            {
                switch (mode)
                {
                    case 0://获取当前用户所在单位下的所有子部门
                        data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode)).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
                        parentId = user.OrganizeId;
                        break;
                    case 400://获取当前用户所在单位下的所有子部门
                        if (user.RoleName.Contains("承包商"))
                        {
                            //含本部门及下属部门及管辖承包商
                            data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode)).OrderBy(x => x.SortCode).ToList();
                            //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                            //parentId = user.OrganizeId;
                            parentId = user.ParentId;
                        }
                        else
                        {
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode)).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
                            parentId = user.OrganizeId;
                        }
                        break;
                    case 1://获取部门Id为ids下的所有子部门(含本单位)
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            DepartmentEntity dept = departmentBLL.GetEntity(Ids);
                            if (dept != null)
                            {
                                data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(dept.DeptCode)).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
                                parentId = dept.DepartmentId;
                            }
                        }
                        break;
                    case 957:
                        if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
                        {
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode) || (t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级部门") || user.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) || (t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        else if (user.RoleName.Contains("班组") || user.RoleName.Contains("部门"))
                        {
                            //user.DeptCode  00014001001001001005
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.DeptCode.Substring(0, 20)) || t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商");
                            parentId = user.OrganizeId;
                        }
                        else {
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.DeptCode));
                            parentId = user.ParentId;
                        }
                        break;
                    case 10://获取部门Id为ids下的所有子部门(不含本单位)
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            DepartmentEntity dept = departmentBLL.GetEntity(Ids);
                            if (dept != null)
                            {
                                data = departmentBLL.GetList().Where(t => (t.Nature == "部门" || t.Nature == "承包商" || t.Nature == "分包商") && t.DeptCode.StartsWith(dept.DeptCode) && t.DepartmentId != Ids).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode); ;
                                if (data.Count() > 0)
                                {
                                    parentId = data.FirstOrDefault().ParentId;
                                }
                                else
                                {
                                    parentId = "-100";
                                }
                            }
                        }
                        break;
                    case 200://获取部门Id为ids下的所有专业
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            DepartmentEntity dept = departmentBLL.GetEntity(Ids);
                            if (dept != null)
                            {
                                data = departmentBLL.GetList().Where(t => (t.Nature == "专业") && t.DeptCode.StartsWith(dept.DeptCode) && t.DepartmentId != Ids).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
                                if (data.Count() > 0)
                                {
                                    parentId = data.FirstOrDefault().ParentId;
                                }
                                else
                                {
                                    parentId = "-100";
                                }
                            }
                        }
                        break;
                    case 2://获取部门Id包含在Ids中的部门（不含本单位）
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            IEnumerable<DepartmentEntity> allData = departmentBLL.GetList();
                            data = allData.Where(t => (Ids.Contains(t.DepartmentId) || Ids == t.EnCode) && t.DepartmentId != "0").OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
                            if (data.Count() > 0)
                            {
                                parentId = data.FirstOrDefault().OrganizeId;
                                data = GetParentId(data, allData);
                                data = data.OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode).ToList();
                            }
                        }
                        else
                        {
                            if (user.RoleName.Contains("集团公司") || user.RoleName.Contains("省级"))
                            {
                                data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode));
                                parentId = user.OrganizeId;
                            }
                            else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级部门"))
                            {
                                data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode));
                                parentId = user.OrganizeId;
                            }
                            else
                            {
                                //含本部门及下属部门及管辖承包商
                                string departIds = GetDeptIds(user.DeptId);
                                data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || (departIds.Contains(t.DepartmentId) && t.DepartmentId != "0")).OrderBy(x => x.SortCode).ToList();
                                //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                                data.Where(t => t.EnCode == user.DeptCode && t.Description != "外包工程承包商" && !departIds.Contains(t.DepartmentId)).ForEach(t => t.ParentId = user.OrganizeId);
                                parentId = user.OrganizeId;
                            }
                        }
                        if (data.Count() == 0)
                        {
                            data = departmentBLL.GetList().Where(t => t.DepartmentId == user.DeptId);
                            parentId = data.FirstOrDefault().ParentId;
                        }
                        break;
                    case 3://获取当前用户所在单位下的所有子部门（含本单位）
                        //高风险中消防水部门特殊处理
                        string specialDeptId = new DataItemDetailBLL().GetItemValue(user.OrganizeId, "FireDept");
                        if (!string.IsNullOrEmpty(specialDeptId) && specialDeptId.Contains(user.DeptId))
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode));
                            parentId = user.OrganizeId;
                        }
                        else
                        {
                            if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
                            {
                                data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode));
                                parentId = user.OrganizeId;
                            }
                            else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级部门"))
                            {
                                data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode));
                                parentId = user.OrganizeId;
                            }
                            else if (user.RoleName.Contains("部门"))
                            {
                                string departIds = GetDeptIds(user.DeptId);
                                int count = departmentBLL.GetList(user.OrganizeId).Where(t => departIds.Contains(t.DepartmentId) && t.DepartmentId != "0").Count();
                                //含本部门及下属部门及管辖承包商
                                if (count > 0)
                                {
                                    data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || departIds.Contains(t.DepartmentId)).OrderBy(x => x.SortCode).ToList();
                                }
                                else
                                {
                                    data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || departIds.Contains(t.DepartmentId)).OrderBy(x => x.SortCode).ToList();
                                }
                                //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                                parentId = user.OrganizeId;
                            }
                            else
                            {
                                //含本部门及下属部门及管辖承包商
                                data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode)).OrderBy(x => x.SortCode).ToList();
                                //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                                //parentId = user.OrganizeId;
                                parentId = user.ParentId;
                            }
                        }
                        break;
                    case 4://获取当前用户所在单位下的所有子部门（含本单位但不包含承包商和分包商）
                        if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
                        {
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级部门"))
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        else
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.ParentId;
                        }
                        break;
                    case 44://获取当前用户所在单位下的所有子部门（含本单位但不包含承包商和分包商）
                        if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
                        {
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        else
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        break;
                    case 5://获取省级下所有厂级(前提是当前用户属于省级用户)
                        data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode) && (t.Nature == "厂级" || t.Description == "各电厂"));
                        if (data.Count() > 0)
                        {
                            parentId = data.Where(t => t.Description == "各电厂").FirstOrDefault().ParentId;
                        }
                        else
                        {
                            parentId = "-100";
                        }
                        break;
                    case 6://获取省级本部部门和下属所有厂级(含本单位。前提是当前用户属于省级用户)
                        data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode) && (t.Nature == "厂级" || t.ParentId == user.OrganizeId || t.Description == "各电厂" || t.DeptCode == user.NewDeptCode));
                        parentId = user.OrganizeId;
                        break;
                    case 7://获取当前用户下的管辖的承包商(前提是当前用户部门属于厂级下的部门)
                        if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && (t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商")).OrderBy(x => x.SortCode).ToList();

                        }
                        else if (user.RoleName.Contains("承包商") || user.RoleName.Contains("分包商"))
                        {
                            data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.StartsWith(user.DeptCode)).OrderBy(x => x.SortCode).ToList();
                        }
                        else
                        {
                            string departIds = GetDeptIds(user.DeptId);
                            data = departmentBLL.GetList(user.OrganizeId).Where(t => t.Description == "外包工程承包商" || (departIds.Contains(t.DepartmentId) && t.DepartmentId != "0")).OrderBy(x => x.SortCode).ToList();
                        }
                        if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("部门"))
                        {
                            parentId = departmentBLL.GetList().Where(t => t.Description == "外包工程承包商" && t.EnCode.StartsWith(user.OrganizeCode)).FirstOrDefault().ParentId;
                        }
                        else
                        {
                            if (data.Count() > 0)
                            {
                                parentId = data.ToList().FirstOrDefault().ParentId;
                            }
                        }

                        break;
                    case 8://获取当前用户所在厂级下的部门及发包的承包商(前提是当前用户部门属于厂级下的部门)
                        if (!string.IsNullOrWhiteSpace(Ids))
                        {
                            string departIds = GetDeptIds(Ids);
                            var list = Ids.Split(',');
                            data = departmentBLL.GetList().Where(t => (list.Contains(t.DepartmentId) && t.DepartmentId != "0") || list.Contains(t.ParentId) || list.Contains(t.OrganizeId) || (departIds.Contains(t.DepartmentId) && t.DepartmentId != "0"));
                            parentId = Ids;
                        }
                        else
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && (t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            if (data.Count() > 0)
                            {
                                parentId = data.OrderBy(t => t.EnCode).First().ParentId;
                            }
                        }
                        break;
                    case 9://获取当前用户的集团、省级、厂级。
                        //data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode) && (t.Nature == "集团" || t.Nature == "省级" || t.Nature == "厂级"));
                        var query = departmentBLL.GetList().ToList();
                        var chld = GetAllChildrens(user.OrganizeId, query);
                        chld.Insert(0, query.Where(x => x.EnCode == user.OrganizeCode).FirstOrDefault());
                        data = chld.Where(t => t.Nature == "集团" || t.Nature == "省级" || t.Nature == "厂级");
                        if (data.Count() > 0)
                        {
                            parentId = data.First().ParentId;
                        }
                        break;
                    case 11://获取当前用户的子部门，不含厂级。
                        var q = departmentBLL.GetList().ToList();
                        var chld1 = GetAllChildrens(user.OrganizeId, q);
                        chld1.Insert(0, q.Where(x => x.EnCode == user.OrganizeCode).FirstOrDefault());
                        data = chld1.Where(t => t.Nature == "集团" || (t.Nature == "省级" && t.Description != "各电厂") || t.Nature == "部门");
                        if (data.Count() > 0)
                        {
                            parentId = data.First().ParentId;
                        }
                        break;
                    case 12:
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            DepartmentEntity dept = departmentBLL.GetEntity(Ids);
                            if (dept != null)
                            {
                                data = departmentBLL.GetList().Where(t => t.Nature == "班组" && t.DeptCode.StartsWith(dept.DeptCode));
                                if (data.Count() > 0)
                                {
                                    parentId = data.FirstOrDefault().ParentId;
                                }
                            }
                        }
                        str = "1";
                        break;
                    case 13://获取所有省公司和电厂
                        data = departmentBLL.GetList().Where(t => (t.Nature == "厂级" || t.Nature == "省级") && string.IsNullOrWhiteSpace(t.Description));
                        foreach (DepartmentEntity dept in data)
                        {
                            dept.ParentId = "200";
                        }
                        parentId = "200";
                        break;
                    case 14://获取当前用户所在单位下的所有子部门
                        data = departmentBLL.GetList().Where(t => (t.DeptCode.StartsWith(user.NewDeptCode) && t.Nature == "部门") || t.EnCode == user.OrganizeCode);
                        parentId = user.OrganizeId;
                        break;
                    case 15://获取特殊部门(编码配置中配置)
                        var strdept = new DataItemDetailBLL().GetItemValue(user.OrganizeId, "DeptSet");
                        var entity = departmentBLL.GetEntity(strdept);
                        if (entity != null)
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(entity.DeptCode));
                        parentId = strdept;
                        break;
                    case 16:
                        //获取本电厂所有部门,不包含班组,专业和承包商
                        if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && t.EnCode != user.OrganizeCode && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商" || t.Nature == "班组" || t.Nature == "专业"));
                        }
                        //获取本部门
                        else
                        {
                            data = departmentBLL.GetList().Where(t => t.DeptCode == user.NewDeptCode);
                        }
                        if (data.Count() > 0)
                        {
                            parentId = data.ToList().OrderBy(x => x.SortCode).FirstOrDefault().ParentId;
                        }
                        break;
                    case 17://获取部门Id为ids下的承包商
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            DepartmentEntity dept = departmentBLL.GetEntity(Ids);
                            if (dept != null)
                            {
                                string departIds = GetDeptIds(dept.DepartmentId);
                                data = departmentBLL.GetList().Where(t => departIds.Contains(t.DepartmentId) && t.DepartmentId != "0");
                                if (data.Count() > 0)
                                {
                                    parentId = data.ToList().OrderBy(x => x.SortCode).FirstOrDefault().ParentId;
                                }
                            }
                        }
                        break;
                    case 18://根据当前登录人获取班组
                        if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                        {
                            data = departmentBLL.GetList().Where(x => x.Nature == "班组" && x.OrganizeId == user.OrganizeId);
                        }
                        else
                        {
                            data = departmentBLL.GetList().Where(x => x.EnCode.StartsWith(user.DeptCode) && x.Nature == "班组");
                        }
                        str = "1";
                        break;
                    case 19://获取当前id下的部门或机构单位
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            data = departmentBLL.GetList().Where(t => t.DepartmentId == Ids);
                        }
                        if (!string.IsNullOrEmpty(departmentCode))
                        {
                            data = departmentBLL.GetList().Where(t => t.DeptCode == departmentCode);
                        }
                        if (data.Count() > 0)
                        {
                            parentId = data.ToList().OrderBy(x => x.SortCode).FirstOrDefault().ParentId;
                        }
                        break;
                    case 20://获取当前用户下的管辖的承包商(前提是当前用户部门属于厂级下的部门) 班组特殊处理
                        if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && (t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                        }
                        else if (user.RoleName.Contains("承包商") || user.RoleName.Contains("分包商"))
                        {
                            data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.StartsWith(user.DeptCode));
                        }
                        else if (user.RoleName.Contains("班组"))
                        {
                            string departIds = GetDeptIds(user.ParentId);
                            data = departmentBLL.GetList().Where(t => departIds.Contains(t.DepartmentId) && t.DepartmentId != "0");
                        }
                        else
                        {
                            string departIds = GetDeptIds(user.DeptId);
                            data = departmentBLL.GetList().Where(t => departIds.Contains(t.DepartmentId) && t.DepartmentId != "0");
                        }
                        if (data.Count() > 0)
                        {
                            parentId = data.ToList().OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode).FirstOrDefault().ParentId;
                        }
                        break;
                    case 21://获取当前登录人所在电厂下所有的单位(不包含承包商)
                        data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                        parentId = user.OrganizeId;
                        break;
                    case 22://获取当前登录人所在电厂下所有的承包商
                        data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && (t.Description == "外包工程承包商" || t.Nature == "承包商" || t.Nature == "分包商"));
                        if (data.Count() > 0)
                        {
                            parentId = user.OrganizeId;
                        }
                        break;
                    case 23://获取当前用户
                        var provdata = departmentBLL.GetList().Where(t => t.Nature == "省级" && !t.FullName.Contains("各电厂") && t.FullName != "区域子公司" && t.DeptCode.StartsWith(user.NewDeptCode.Substring(0, 3)));
                        DepartmentEntity provEntity = null;
                        //省级根节点
                        if (provdata.Count() > 0)
                        {
                            provEntity = provdata.FirstOrDefault();
                        }
                        //厂级单位
                        var sjdata = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode)).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode).ToList();
                        if (sjdata.Count() > 0)
                        {
                            var factdata = sjdata.Where(p => p.Nature == "厂级").FirstOrDefault();
                            if (null != provEntity)
                            {
                                factdata.ParentId = provEntity.DepartmentId;
                                sjdata.Add(provEntity);
                            }
                        }
                        if (null != provEntity)
                        {
                            //省级下面的部门
                            var provchilddata = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(provEntity.EnCode) && t.Nature != "省级" && t.EnCode != provEntity.EnCode).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode).ToList();
                            foreach (DepartmentEntity dentity in provchilddata)
                            {
                                if (sjdata.Where(p => p.DepartmentId == dentity.DepartmentId).Count() == 0)
                                {
                                    sjdata.Add(dentity);
                                }
                            }
                        }
                        data = sjdata.OrderBy(p => p.DeptCode).ToList();
                        if (data.Count() > 0)
                        {
                            parentId = data.FirstOrDefault().OrganizeId;
                        }
                        break;
                    case 100://获取当前用户所在单位下的所有子部门  
                        if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级"))
                        {
                            data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        else
                        {
                            data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode) && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商"));
                            parentId = user.OrganizeId;
                        }
                        break;
                    case 24:
                        #region 获取所有的厂级，不包括下级
                        data = departmentBLL.GetList().Where(x => x.Nature == "厂级");
                        //List<object> dataList = new List<object>();
                        foreach (var item in data)
                        {
                            TreeEntity tree = new TreeEntity();
                            bool hasChildren = data.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                            tree.id = item.DepartmentId;
                            tree.text = item.FullName;
                            tree.value = item.EnCode;
                            tree.img = item.Nature == "承包商" ? item.SendDeptID : null;//借用该字段，查发包的承包商。
                            tree.isexpand = item.DepartmentId == parentId ? true : false;
                            tree.complete = true;
                            tree.checkstate = 0;
                            tree.showcheck = checkMode == 0 ? false : true;
                            tree.hasChildren = hasChildren;
                            tree.parentId = item.ParentId;
                            tree.Attribute = "Code";
                            tree.AttributeValue = item.EnCode;
                            tree.AttributeB = "Sort";
                            tree.AttributeValueB = item.Nature == "厂级" || item.Nature == "集团" || item.Nature == "省级" ? "Organize" : "Department";
                            tree.AttributeA = "manager";
                            tree.AttributeValueA = item.Manager + "," + item.ManagerId + "," + item.IsOrg;
                            tree.AttributeC = "NewCode";
                            tree.AttributeValueC = item.DeptCode;
                            tree.AttributeD = "Nature";
                            tree.AttributeValueD = item.Nature;
                            treeList.Add(tree);
                        }

                        return Json(treeList, JsonRequestBehavior.AllowGet);
                    #endregion

                    case 25://获取所有的集团、省级、厂级的单位
                        data = departmentBLL.GetList().Where(x => x.Nature.Contains("集团") || x.Nature.Contains("省级") || x.Nature.Contains("厂级"));
                        break;
                    case 26:
                        if (!string.IsNullOrEmpty(Ids))
                        {
                            DepartmentEntity dept = departmentBLL.GetEntity(Ids);
                            if (dept != null)
                            {
                                data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(dept.DeptCode) && (t.Nature == "部门" || t.Nature == "厂级")).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
                                parentId = dept.DepartmentId;
                            }
                        }
                        break;
                        //获取配置的部门
                    case 27:
                        var deptitems = dataItemCache.GetDataItemList("LllegalVerifyDeptRoleSetting").Select(p => p.ItemName.Trim()).ToList();

                        data = departmentBLL.GetList().Where(t => deptitems.Contains(t.DepartmentId)).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);

                        if (data.Count() > 0)
                        {
                            parentId = data.FirstOrDefault().ParentId;
                        }
                        break;
                    case 88://获取当前登录人所在电厂下所有的单位(不包含承包商)
                        var cbs = departmentBLL.GetList().Where(t => t.Description == "外包工程承包商" && t.OrganizeId == user.OrganizeId).FirstOrDefault();
                        data = departmentBLL.GetList().Where(t => t.Description == "外包工程承包商" || (t.Nature == "承包商" && t.ParentId == cbs.DepartmentId));
                        parentId = user.OrganizeId;
                        break;
                    case 89://获取当前用户的集团、省级、厂级。
                        //data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.OrganizeCode) && (t.Nature == "集团" || t.Nature == "省级" || t.Nature == "厂级"));
                        var query1 = departmentBLL.GetList().ToList();
                        var chld2 = GetAllChildrens(user.OrganizeId, query1);
                        chld2.Insert(0, query1.Where(x => x.EnCode == user.OrganizeCode).FirstOrDefault());
                        data = chld2.Where(t => t.Nature == "集团" || t.Nature == "省级" || t.Nature == "厂级" || t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商");
                        if (data.Count() > 0)
                        {
                            parentId = user.OrganizeId;
                        }
                        break;
                    default://获取当前用户所在单位下的所有子部门  
                        data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode));
                        parentId = user.OrganizeId;
                        break;

                }
            }
            //按部门名称进行搜索
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                data = data.ToList().TreeWhere(t => t.FullName.Contains(keyword), "DepartmentId");
            }
            data = data.OrderBy(t => t.DeptCode).OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();
            List<DepartmentEntity> newList = data.ToList();
            List<DepartmentEntity> lstDepts = newList.Where(t => t.Description == "外包工程承包商").ToList();
            foreach (DepartmentEntity dept1 in lstDepts)
            {
                newList.Remove(dept1);
                string newId = "cx100_" + Guid.NewGuid().ToString();
                if (string.IsNullOrWhiteSpace(deptType) || deptType == "长协")
                {

                    List<DepartmentEntity> lstDept2 = newList.Where(t => t.DeptType == "长协" && t.ParentId == dept1.DepartmentId).ToList();
                    if (lstDept2.Count > 0)
                    {
                        DepartmentEntity cxDept = new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "长协外包单位",
                            EnCode = "cx100_" + dept1.EnCode,
                            ParentId = dept1.ParentId,
                            Description = "外包工程承包商",
                            Nature = dept1.Nature,
                            DeptCode = dept1.DeptCode,
                            Manager = dept1.Manager,
                            ManagerId = dept1.ManagerId,
                            IsOrg = dept1.IsOrg,
                            DeptType = dept1.DeptType
                        };
                        newList.Add(cxDept);
                    }
                    foreach (DepartmentEntity dept2 in lstDept2)
                    {
                        dept2.ParentId = newId;
                    }
                }
                if (string.IsNullOrWhiteSpace(deptType) || deptType == "临时")
                {
                    newId = "ls100_" + Guid.NewGuid().ToString();

                    List<DepartmentEntity> lstDept2 = newList.Where(t => t.DeptType == "临时" && t.ParentId == dept1.DepartmentId).ToList();
                    if (lstDept2.Count > 0)
                    {
                        newList.Add(new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "临时外包单位",
                            EnCode = "ls100_" + dept1.EnCode,
                            ParentId = dept1.ParentId,
                            Description = "外包工程承包商",
                            Nature = dept1.Nature,
                            DeptCode = dept1.DeptCode,
                            Manager = dept1.Manager,
                            ManagerId = dept1.ManagerId,
                            IsOrg = dept1.IsOrg,
                            DeptType = dept1.DeptType
                        });
                    }
                    foreach (DepartmentEntity dept2 in lstDept2)
                    {
                        dept2.ParentId = newId;
                    }
                }
            }
            foreach (DepartmentEntity item in newList)
            {
                int chkState = 0;
                //设置部门默认选中状态
                if (!string.IsNullOrEmpty(deptIds))
                {
                    string[] arrids = deptIds.Split(',');
                    if (arrids.Contains(item.DepartmentId) || arrids.Contains(item.EnCode))
                    {
                        chkState = 1;
                    }
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = newList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                
                
                //是否是外包工程承包商节点,部分界面涉及不选此节点 update 2019.05.14
                if (item.Description == "外包工程承包商")
                {
                    tree.img = "1";
                    tree.showcheck = false;
                }
                else if (item.Description == "各电厂" || item.Description == "区域子公司")
                {
                    tree.img = "2";
                    tree.showcheck = false;
                }
                else
                {
                    tree.img = "0";
                    tree.showcheck = checkMode == 0 ? false : true;
                }
                if (hasChildren && !string.IsNullOrEmpty(deptIds) && newList.Count(t => t.ParentId == item.DepartmentId && t.DepartmentId == deptIds) > 0)
                {
                    tree.isexpand = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(selectCode))
                    {   //展开指定树节点,本级不展开
                        if ((selectCode.StartsWith(item.DeptCode) && selectCode != item.DeptCode) || item.DepartmentId == parentId)
                        {
                            tree.isexpand = true;
                        }
                        else
                        {
                            tree.isexpand = false;
                        }
                    }
                    else
                    {
                        tree.isexpand = item.DepartmentId == parentId ? true : false;
                    }
                }
                tree.complete = true;
                tree.checkstate = chkState;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.Attribute = "Code";
                tree.AttributeValue = item.EnCode;
                tree.AttributeB = "Sort";
                tree.AttributeValueB = item.Nature == "厂级" || item.Nature == "集团" || item.Nature == "省级" ? "Organize" : "Department";
                tree.AttributeA = "manager";
                tree.AttributeValueA = item.Manager + "," + item.ManagerId + "," + item.IsOrg;
                tree.AttributeC = "NewCode";
                tree.AttributeValueC = item.DeptCode;
                tree.AttributeD = "Nature";
                tree.AttributeValueD = item.Nature;
                tree.AttributeE = "DeptType";
                tree.AttributeValueE = item.DeptType;
                tree.AttributeF = "ShortName";
                tree.AttributeValueF = item.ShortName;
                treeList.Add(tree);
            }

            if (str == "1")
            {
                return Content(treeList.TreeToJson(parentId));
            }
            else
            {
                //除厂级单位外，取本单位及子单位和发包的承包商数据 yuguolei-20180803     
                //string untakerJson = "";
                if (parentId != "-1")
                {
                    //var untaker = treeList.FirstOrDefault(x => x.img == parentId);
                    //if (untaker != null)
                    //{//发包部门的承包商
                    //    untakerJson = treeList.TreeToJson(untaker.parentId);
                    //}

                    var tre = treeList.FirstOrDefault(x => x.id == parentId);
                    if (tre != null)
                        parentId = tre.parentId;

                }
                json = treeList.TreeToJson(parentId);
                if (json.Length < 5)
                {
                    json = treeList.TreeToJson("0");
                }

                //if (!string.IsNullOrEmpty(untakerJson))//拼接发包部门的承包商
                //    json = json.Substring(0, json.Length - 1) + "," + untakerJson.Substring(1);

                return Content(json);
            }
        }
        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<DepartmentEntity> GetAllChildrens(string pId, List<DepartmentEntity> list)
        {
            var query = list.Where(x => x.ParentId == pId).ToList();
            query = query.Concat(query.SelectMany(x => GetAllChildrens(x.DepartmentId, list))).ToList();

            return query;
        }
        #endregion
    }
}
