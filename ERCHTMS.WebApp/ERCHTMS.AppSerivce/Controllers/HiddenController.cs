using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HazardsourceManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.LllegalStandard;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    //隐患排查
    [HandlerLogin(LoginMode.Enforce)]
    public class HiddenController : BaseApiController
    {
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentCache departmentCache = new DepartmentCache();
        private RoleCache roleCache = new RoleCache();
        private AccountBLL accountBLL = new AccountBLL();

        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //隐患整改信息
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL(); //隐患评估信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收信息
        private HtReCheckBLL htrecheckbll = new HtReCheckBLL(); //隐患复查验证
        private HTEstimateBLL htestimatebll = new HTEstimateBLL(); //整改效果评估信息
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //隐患流程
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private HTExtensionBLL htextensionbll = new HTExtensionBLL(); //隐患延期整改
        private SaftyCheckDataRecordBLL saftycheckdatarecordbll = new SaftyCheckDataRecordBLL();
        private DistrictBLL districtbll = new DistrictBLL();//区域
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private ProjectBLL projectbll = new ProjectBLL();
        private OrganizeBLL orgBLL = new OrganizeBLL();
        private FileFolderBLL fileFolderBLL = new FileFolderBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private OpinionBLL opinionbll = new OpinionBLL();

        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); //违章基本业务对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //违章核准业务对象
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //违章整改业务对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //违章验收业务对象
        private LllegalConfirmBLL lllegalconfirmbll = new LllegalConfirmBLL(); //验收确认信息对象
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); //考核业务对象
        private LllegalAwardDetailBLL lllegalawarddetailbll = new LllegalAwardDetailBLL(); //违章奖励信息
        private LllegalExtensionBLL lllegalextensionbll = new LllegalExtensionBLL(); //违章整改延期对象

        private LllegalStatisticsBLL legbll = new LllegalStatisticsBLL();
        private LllegalstandardBLL lllegalstandardbll = new LllegalstandardBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务

        public HttpContext ctx { get { return HttpContext.Current; } }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        /*隐患流程*/

        #region 基础数据访问

        #region 获取所属单位
        /// <summary>
        /// 获取所属单位
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHidDepart([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            IList<DeptData> result = new List<DeptData>();
            try
            {

                if (curUser.RoleName.Contains("省级用户"))
                {
                    var dtDept = departmentBLL.GetAllFactory(curUser);
                    foreach (DataRow row in dtDept.Rows)
                    {
                        DeptData entity = new DeptData();
                        entity.deptid = row["departmentid"].ToString();
                        entity.code = row["encode"].ToString();
                        entity.name = row["fullname"].ToString();
                        entity.newcode = row["deptcode"].ToString();
                        result.Add(entity);
                    }
                }
                else
                {
                    //当前用户的所属机构
                    DepartmentEntity dept = userbll.GetUserOrgInfo(curUser.UserId);
                    DeptData deptdata = new DeptData();
                    deptdata.deptid = dept.DepartmentId;
                    deptdata.code = dept.EnCode;
                    deptdata.name = dept.FullName;
                    deptdata.newcode = dept.DeptCode;
                    result.Add(deptdata);
                }
            }
            catch (Exception)
            {
                return new
                {
                    code = -1,
                    info = "获取数据失败",
                    count = 0
                };
            }

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 0,
                data = result
            };

        }
        #endregion

        private List<DepartmentEntity> lstDeptData = new List<DepartmentEntity>();

        #region 获取公司机构(整改部门)
        /// <summary>
        /// 获取公司机构
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInst([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            IList<DeptData> result = new List<DeptData>();
            IList<DeptData> list = new List<DeptData>();

            try
            {
                //选中的所属单位
                string orgid = string.Empty;
                //请求标记
                string reqmark = res.Contains("reqmark") ? dy.data.reqmark : "0";   //1 排查 ，2 整改 ，3 厂级验收，4 省级验收 ，5 复查 

                string mode = res.Contains("mode") ?  dy.data.mode : "";

                List<DepartmentEntity> dlist = departmentBLL.GetList().OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();

                List<DepartmentEntity> plist = new List<DepartmentEntity>();

                //获取当前部门
                string organizeId = curUser.OrganizeId;

                string parentId = string.Empty;

                if (!string.IsNullOrEmpty(organizeId))
                {
                    var temporg = departmentBLL.GetEntity(organizeId);
                    parentId = temporg.ParentId;
                    orgid = organizeId.ToString();
                    plist = dlist.Where(t => t.DeptCode.StartsWith(temporg.DeptCode)).OrderBy(t => t.DeptCode).OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();
                }

                if (curUser.RoleName.Contains("省级用户"))
                {
                    orgid = res.Contains("orgid") ? dy.data.orgid : "";
                    //只有厂级
                    if (reqmark == "2" || reqmark == "3")
                    {
                        parentId = departmentBLL.GetEntity(orgid).ParentId;
                        organizeId = orgid;
                    }
                    //只有省级
                    else if (reqmark == "1" || reqmark == "4" || reqmark == "5")
                    {
                        parentId = departmentBLL.GetEntity(curUser.OrganizeId).ParentId;
                    }
                    else //其他
                    {
                        parentId = "0";
                    }
                }


                if (!string.IsNullOrEmpty(mode))
                {
                    switch (mode)
                    {
                        case "27":
                            var deptitems = new DataItemDetailBLL().GetDataItemListByItemCode("LllegalVerifyDeptRoleSetting").Select(p => p.ItemName.Trim()).ToList();

                            plist = dlist.Where(t => deptitems.Contains(t.DepartmentId)).OrderBy(t => t.DeptCode).OrderBy(t => t.FullName).OrderBy(t => t.SortCode).ToList();

                            organizeId = string.Empty;

                            if (plist.Count() > 0)
                            {
                                parentId = plist.FirstOrDefault().ParentId;
                            }
                            break;
                    }
                }

                lstDeptData = plist;

                try
                {
                    if (string.IsNullOrEmpty(organizeId))
                    {
                        var templist = plist.Where(p => p.ParentId == parentId).ToList();
                        foreach (var temporg in templist)
                        {
                            DeptData dept = new DeptData();
                            dept.deptid = temporg.DepartmentId;
                            dept.code = temporg.EnCode;
                            dept.newcode = temporg.DeptCode;
                            //dept.isorg = 1;
                            dept.oranizeid = temporg.OrganizeId;
                            dept.parentcode = "";
                            dept.parentid = temporg.ParentId;
                            dept.name = temporg.FullName;
                            dept.Manager = temporg.Manager;
                            dept.ManagerId = temporg.ManagerId;
                            list = GetChangeDept(templist, dept, orgid, reqmark);
                            dept.isparent = list.Count() > 0;
                            dept.children = list;
                            result.Add(dept);
                        }
                    }
                    else
                    {
                        //获取当前机构下的所有部门
                        DepartmentEntity org = departmentBLL.GetEntity(organizeId);
                        DeptData dept = new DeptData();
                        dept.deptid = org.DepartmentId;
                        dept.code = org.EnCode;
                        dept.newcode = org.DeptCode;
                        dept.isorg = 1;
                        dept.oranizeid = org.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = org.FullName;
                        dept.isparent = true;
                        dept.Manager = org.Manager;
                        dept.ManagerId = org.ManagerId;
                        list = GetChangeDept(dept, orgid, reqmark);
                        dept.children = list;
                        result.Add(dept);

                        IList<DeptData> lstDepts = dept.children.Where(t => t.name == "临时外包单位").ToList();
                        if (lstDepts.Count > 0)
                        {
                            dept = lstDepts[0];
                            dept.deptid = dept.deptid;
                            list = GetChangeDept(dept, orgid, reqmark);
                            dept.children = list;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new
                    {
                        code = -1,
                        info = "获取数据失败",
                        count = 0
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    code = -1,
                    info = ex.Message,
                    count = 0,
                    data = result
                };
            }

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 0,
                data = result
            };
        }
        #endregion

        #region 获取部门
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="parentDept"></param>
        /// <returns></returns>
        public IList<DeptData> GetChangeDept(DeptData parentDept, string orgid, string reqmark = "")
        {
            IList<DeptData> list = new List<DeptData>();

            bool istrue = true;

            try
            {
                string parentId = parentDept.deptid;
                #region 通用
                List<DepartmentEntity> plist = new List<DepartmentEntity>();
                if (parentDept.name == "长协外包单位")
                {
                    parentId = parentId.Replace("cx100_", "");
                    plist = departmentBLL.GetList().Where(t => t.ParentId == parentId && t.DeptType == "长协").OrderBy(t => t.SortCode).ToList();
                }
                if (parentDept.name == "临时外包单位")
                {
                    parentId = parentId.Replace("ls100_", "");
                    plist = departmentBLL.GetList().Where(t => t.DeptType == "临时" && t.ParentId == parentId).OrderBy(t => t.SortCode).ToList();
                }
                else
                {
                    plist = departmentBLL.GetList().Where(t => t.ParentId == parentId).OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode).ToList();
                }
                List<DepartmentEntity> lstDepts = plist.Where(t => t.Description == "外包工程承包商").ToList();
                if (lstDepts.Count > 0)
                {
                    DepartmentEntity dept1 = lstDepts.FirstOrDefault();
                    string newId = "ls100_" + dept1.DepartmentId;
                    plist.Add(new DepartmentEntity
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
                        OrganizeId = dept1.OrganizeId,
                        IsOrg = dept1.IsOrg,
                        DeptType = dept1.DeptType
                    });
                }
                //  1 ,4, 5
                if (plist.Count() > 0)
                {
                    var dlist = departmentBLL.GetList().OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode).ToList();

                    foreach (DepartmentEntity entity in plist)
                    {
                        DeptData depts = new DeptData();
                        if (entity.Description == "外包工程承包商" && entity.FullName != "临时外包单位")
                        {
                            entity.DepartmentId = "cx100_" + entity.DepartmentId;
                            entity.EnCode = "cx100_" + entity.EnCode;
                            entity.FullName = "长协外包单位";

                        }
                        if (entity.Nature == "承包商" || entity.Nature == "分包商")
                        {
                            if (entity.DeptType == "长协")
                            {
                                entity.ParentId = "cx100_" + entity.ParentId;
                            }
                            if (entity.DeptType == "临时")
                            {
                                entity.ParentId = "ls100_" + entity.ParentId;
                            }
                            if (entity.DeptType != "长协" && parentDept.name == "长协外包单位")
                            {
                                continue;
                            }
                            else
                            {
                                entity.ParentId = "ls100_" + entity.ParentId;
                            }
                        }
                        depts.deptid = entity.DepartmentId;
                        depts.code = entity.EnCode;
                        depts.oranizeid = entity.OrganizeId;
                        depts.name = entity.FullName;
                        depts.isorg = 0;
                        depts.parentid = entity.ParentId;
                        if (depts.parentid == "0")
                        {
                            depts.parentid = depts.oranizeid;
                        }
                        depts.parentcode = parentDept.parentcode;
                        if (!string.IsNullOrEmpty(entity.Description))
                        {
                            depts.isoptional = "1";
                        }

                        if (reqmark == "1" || reqmark == "4" || reqmark == "5")
                        {
                            if (entity.FullName == "各电厂")
                            {
                                istrue = false;
                            }
                        }


                        if (istrue)
                        {
                            parentId = depts.deptid;
                            if (depts.name == "长协外包单位")
                            {
                                parentId = depts.deptid.Replace("cx100_", "");
                            }
                            var pdepts = dlist.Where(p => p.ParentId == parentId).ToList();
                            if (pdepts.Count() > 0)
                            {
                                depts.isparent = true;
                                if (!string.IsNullOrEmpty(orgid))
                                {
                                    if (entity.Nature == "厂级")
                                    {
                                        if (entity.DepartmentId == orgid)
                                        {
                                            var glist = GetChangeDept(depts, orgid);
                                            depts.children = glist;
                                        }
                                    }
                                    else
                                    {
                                        var glist = GetChangeDept(depts, orgid);
                                        depts.children = glist;
                                    }
                                }
                                else
                                {
                                    var glist = GetChangeDept(depts, orgid);
                                    depts.children = glist;
                                }

                            }
                            else
                            {
                                depts.isparent = false;
                                depts.children = new List<DeptData>();
                            }

                            if (!string.IsNullOrEmpty(orgid))
                            {
                                if (entity.Nature == "厂级")
                                {
                                    if (entity.DepartmentId == orgid)
                                    {
                                        list.Add(depts);

                                    }
                                }
                                else
                                {
                                    list.Add(depts);
                                }
                            }
                            else
                            {
                                list.Add(depts);

                            }
                        }

                    }
                }
                else
                {
                    if (parentDept.parentid != "0" && parentDept.name != "临时外包单位")
                    {
                        list.Add(parentDept);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }
        #endregion

        #region 获取部门
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="parentDept"></param>
        /// <returns></returns>
        public IList<DeptData> GetChangeDept(List<DepartmentEntity> plist, DeptData parentDept, string orgid, string reqmark = "")
        {
            IList<DeptData> list = new List<DeptData>();

            bool istrue = true;

            try
            {
                string parentId = parentDept.deptid;

                #region 历史
                if (plist.Count() > 0)
                {
                    var dlist = plist.Where(p => p.ParentId == parentId).OrderBy(t=>t.SortCode).OrderBy(t=>t.FullName);
                    foreach (DepartmentEntity entity in dlist)
                    {
                        DeptData depts = new DeptData();
                        depts.deptid = entity.DepartmentId;
                        depts.code = entity.EnCode;
                        depts.oranizeid = entity.OrganizeId;
                        depts.name = entity.FullName;
                        depts.isorg = 0;
                        depts.parentid = entity.ParentId;
                        if (depts.parentid == "0")
                        {
                            depts.parentid = depts.oranizeid;
                        }
                        depts.parentcode = parentDept.parentcode;
                        if (istrue)
                        {

                            var pdepts = plist.Where(p => p.ParentId == depts.deptid).ToList();

                            if (entity.EnCode.StartsWith("cx100"))
                            {
                                pdepts = lstDeptData.Where(p => p.ParentId == depts.deptid && p.DeptType == "长协").ToList();
                            }
                            if (entity.EnCode.StartsWith("ls100"))
                            {
                                pdepts = lstDeptData.Where(p => p.ParentId == depts.deptid && p.DeptType == "临时").ToList();
                            }
                            if (pdepts.Count() > 0)
                            {
                                depts.isparent = true;
                                var glist = GetChangeDept(pdepts, depts, orgid);
                                depts.children = glist;
                            }
                            else
                            {
                                depts.isparent = false;
                                depts.children = new List<DeptData>();
                            }
                            list.Add(depts);
                        }
                    }
                }
                else
                {
                    if (parentDept.parentid != "0")
                    {
                        list.Add(parentDept);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }
        #endregion

        #region 获取区域
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAllArea([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            int pagesize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 0;

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //选中的所属单位
            string orgid = string.Empty;
            //获取当前部门
            string organizeId = curUser.OrganizeId;

            string parentid = res.Contains("parentid") ? dy.data.parentid.ToString() : "0";

            string areaname = res.Contains("areaname") ? dy.data.areaname.ToString() : "";

            DefaultDataSettingBLL defaultdatasettingbll = new DefaultDataSettingBLL();

            List<DefaultDataSettingEntity> defaultlist = defaultdatasettingbll.GetList(curUser.UserId).Where(p => p.DEFAULTMARK == "HIDPOINTNAME").ToList(); //排序 

            if (curUser.RoleName.Contains("省级用户"))
            {
                orgid = res.Contains("orgid") ? dy.data.orgid : "";
                //风险查询条件时没有传OrgId--省级用户传入厂级Code,此时默认OrgId为厂级Id
                if (string.IsNullOrWhiteSpace(orgid))
                {
                    var deptcode = res.Contains("deptcode") ? dy.data.orgid : "";
                    if (!string.IsNullOrWhiteSpace(deptcode))
                    {
                        var dept = new DepartmentBLL().GetList().Where(x => x.EnCode == deptcode && x.Nature == "厂级").FirstOrDefault();
                        if (dept != null)
                        {
                            orgid = dept.DepartmentId;
                        }
                    }
                }
            }
            else
            {
                orgid = curUser.OrganizeId; //厂级用户
            }

            IList<AreaData> result = new List<AreaData>();

            try
            {
                //获取当前机构下所有的区域
                var dlist = districtbll.GetListByOrgIdAndParentId(orgid, "");

                var ditlist = dlist.Where(p => p.ParentID == parentid).ToList();

                if (!string.IsNullOrWhiteSpace(areaname))
                {
                    ditlist = dlist.Where(t => t.DistrictName.Contains(areaname.Trim())).ToList();
                }
                if (!string.IsNullOrWhiteSpace(areaname) && ditlist.Count > 0)
                {

                    ditlist = GetParentId(ditlist, dlist);
                }

                foreach (DistrictEntity item in ditlist)
                {
                    AreaData entity = new AreaData();
                    entity.areaid = item.DistrictID;
                    entity.areacode = item.DistrictCode;
                    entity.areaname = item.DistrictName;
                    entity.parentareaid = item.ParentID;
                    entity.isdefault = defaultlist.Where(p => p.DEFAULTKEY == item.DistrictCode).ToList().Count() > 0;
                    var parentarea = dlist.Where(p => p.ParentID == item.DistrictID);
                    if (parentarea.Count() > 0)
                    {
                        entity.isparent = true;
                        if (!string.IsNullOrWhiteSpace(areaname))
                        {
                            var glist = GetAreaData(dlist, defaultlist, entity, orgid, item.DistrictID).Where(x => x.areaname.Contains(areaname)).ToList();
                            entity.children = glist;
                        }
                        else
                        {
                            var glist = GetAreaData(dlist, defaultlist, entity, orgid, item.DistrictID);
                            entity.children = glist;
                        }
                    }
                    else
                    {
                        entity.isparent = false;
                        entity.children = new List<AreaData>();
                    }
                    result.Add(entity);
                }
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < result[i].children.Count; j++)
                    {
                        for (int k = 0; k < result.Count; k++)
                        {
                            if (result[i].children[j].parentareaid == result[k].parentareaid)
                            {
                                result.Remove(result[k]);
                                if (i > 0) i--;
                                break;
                            }
                        }

                    }

                }
            }
            catch (Exception)
            {
                return new { code = -1, info = "获取数据失败", count = 0 };
            }
            //获取当前部门
            return new { code = 0, info = "获取数据成功", count = result.Count(), data = result };
        }
        //找到集合中最上级父节点的id
        public List<DistrictEntity> GetParentId(IEnumerable<DistrictEntity> data, IEnumerable<DistrictEntity> alldata)
        {
            string id = "";
            List<DistrictEntity> newdata = new List<DistrictEntity>();
            if (data.Count() > 0)
            {
                newdata = data.ToList();

                for (int i = 0; i < newdata.Count; i++)
                {
                    id = newdata[i].ParentID;
                    //如果自己表里面没有父级 而查询前的表里面有则加入到表中
                    if (newdata.Where(it => it.DistrictID == id).Count() == 0 && alldata.Where(it => it.DistrictID == id).Count() > 0)
                    {
                        //if (newdata.Where(it => it.ParentID == id).Count() == 0) {
                        newdata.Add(alldata.Where(it => it.DistrictID == id).FirstOrDefault());
                        //}
                    }
                }
            }
            //if (newdata.Count == 0)
            //{
            //    newdata = data.ToList();
            //}
            return newdata;

        }
        #endregion

        #region 获取区域
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="areadata"></param>
        /// <param name="organizeid"></param>
        /// <returns></returns>
        public IList<AreaData> GetAreaData(List<DistrictEntity> list, List<DefaultDataSettingEntity> defaultlist, AreaData areadata, string organizeid, String itemid)
        {
            IList<AreaData> result = new List<AreaData>();

            try
            {
                var plist = list.Where(t => t.ParentID == itemid).OrderBy(t => t.DistrictCode).ToList();

                if (plist.Count() > 0)
                {
                    foreach (DistrictEntity item in plist)
                    {
                        AreaData entity = new AreaData();
                        entity.areaid = item.DistrictID;
                        entity.areacode = item.DistrictCode;
                        entity.areaname = item.DistrictName;
                        entity.parentareaid = item.ParentID;
                        entity.isdefault = defaultlist.Where(p => p.DEFAULTKEY == item.DistrictCode).ToList().Count() > 0;
                        var parentarea = list.Where(p => p.ParentID == item.DistrictID);
                        if (parentarea.Count() > 0)
                        {
                            entity.isparent = true;
                            var glist = GetAreaData(list, defaultlist, entity, organizeid, item.DistrictID);
                            entity.children = glist;
                        }
                        else
                        {
                            entity.isparent = false;
                            entity.children = new List<AreaData>();
                        }
                        result.Add(entity);
                    }
                }
                else
                {
                    result.Add(areadata);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        #endregion


        #region 隐患级别
        /// <summary>
        /// 隐患级别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetHidRanks([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string hiddepart = res.Contains("hiddepart") ? dy.data.hiddepart : "";  //所属单位
            string rankid = res.Contains("rankid") ? dy.data.rankid.ToString() : ""; //隐患级别id
            string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid.ToString() : "";//隐患id
            string marjorclassify = res.Contains("marjorclassify") ? dy.data.marjorclassify : ""; //隐患专业分类
            string workstream = res.Contains("workstream") ? dy.data.workstream : "";  //流程状态
            string hidtype = res.Contains("hidtype") ? dy.data.hidtype : "";  //隐患类别
            string hidbmid = res.Contains("hidbmid") ? dy.data.hidbmid : "";  //所属部门id
            List<HidRankNewData> data = new List<HidRankNewData>();
            string mark = string.Empty;

            try
            {
                #region 配置项目
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidRank','HidMajorClassify','HidBmEnableOrganize','AcceptPersonControl','IsEnableMajorClassify','AppHidRankControl','ControlPicMustUpload'").ToList();//编码集合
                var rankitemlist = itemlist.Where(p => p.EnCode == "HidRank").ToList(); //隐患级别
                var majoritemlist = itemlist.Where(p => p.EnCode == "HidMajorClassify").ToList();//隐患专业分类集合 
                var bmIitemlist = itemlist.Where(p => p.EnCode == "HidBmEnableOrganize").ToList();//'HidBmEnableOrganize'
                var appHidRankControl = itemlist.Where(p => p.EnCode == "AppHidRankControl").ToList();// 移动端隐患级别获取控制
                string ControlPicMustUpload = string.Empty;
                var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == curUser.OrganizeId);
                if (cpmu.Count() > 0)
                {
                    ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
                }
                int getIndex = -1;
                if (appHidRankControl.Count() > 0)
                {
                    var tempAppHidRank = appHidRankControl.Where(p => p.ItemName == "HidRankControlDispatch");
                    if (tempAppHidRank.Count() > 0)
                    {
                        getIndex = int.Parse(tempAppHidRank.FirstOrDefault().ItemValue.Trim());
                    }

                }
                if (curUser.RoleName.Contains("省级用户"))
                {
                    mark = "省级隐患排查";
                }
                else
                {
                    mark = "厂级隐患排查";
                }


                //获取是否设置验收人权限
                string authdeptid = !string.IsNullOrEmpty(hiddepart) ? hiddepart : curUser.OrganizeId;
                string argsCode = curUser.OrganizeCode;
                if (string.IsNullOrEmpty(hiddepart))
                {
                    var deptEntity = departmentBLL.GetEntity(authdeptid);
                    if (null != deptEntity)
                    {
                        argsCode = deptEntity.DeptCode;
                    }
                }
                bool IsEnableAccept = itemlist.Where(p => p.EnCode == "AcceptPersonControl").Where(p => p.ItemValue == argsCode).Count() > 0; //是否启用验收人控制 
                bool IsEnableMajorClassify = itemlist.Where(p => p.EnCode == "IsEnableMajorClassify").Where(p => p.ItemValue == argsCode).Count() > 0;//是否启用专业分类 

                #endregion

                if (getIndex == 0) //仅仅只获取隐患级别，不调用任何权限控制服务； 
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = false; //判定是否存在上报功能权限
                        model.isshowappoint = false; //是否具有指定
                        model.ismustwrite = false;
                        model.ishavetjsubmit = false;
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                            mcentity.isupsubmit = false; //判定是否存在上报功能权限
                            mcentity.ismustwrite = false;
                            mcentity.ishavetjsubmit = false;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = false; //判定是否存在上报功能权限
                            model.isshowappoint = false; //是否具有指定整改责任人权限
                            model.ismustwrite = false;
                            model.ishavetjsubmit = false; //是否具有同级提交权限 //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                                mcentity.isupsubmit = false; //判定是否存在上报功能权限
                                mcentity.ismustwrite = false;
                                mcentity.ishavetjsubmit = false; //是否具有同级提交权限 //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }
                else if (getIndex == 1) //隐患级别调用权限控制服务，专业分类不调取； 
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", string.Empty, hidtype, hidbmid,string.Empty, hiddenid); //判定是否存在上报功能权限
                        model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", string.Empty, string.Empty, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定
                        model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", string.Empty, string.Empty, hidtype, hidbmid, string.Empty, hiddenid);
                        bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", string.Empty, string.Empty, hidtype, hidbmid, string.Empty, hiddenid); //是否具有不必填权限
                        bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", string.Empty, string.Empty, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                        model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                            mcentity.isupsubmit = false; //判定是否存在上报功能权限
                            mcentity.ismustwrite = false;
                            mcentity.ishavetjsubmit = false;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                            model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                            model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限
                            bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                                mcentity.isupsubmit = false; //判定是否存在上报功能权限
                                mcentity.ismustwrite = false;
                                mcentity.ishavetjsubmit = false;//是否具有同级提交权限 //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }
                else if (getIndex == 2) //隐患级别不调用权限控制服务，专业分类则调取权限控制服务； 
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = false; //判定是否存在上报功能权限
                        model.isshowappoint = false; //是否具有指定
                        model.ismustwrite = false;
                        model.ishavetjsubmit = false; //是否具有不必填权限  //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                            mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                            mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = false; //判定是否存在上报功能权限
                            model.isshowappoint = false; //是否具有指定整改责任人权限
                            model.ismustwrite = false;
                            model.ishavetjsubmit = false; //是否具有同级提交权限  //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                                mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                                mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, string.Empty, hidbmid, hiddenid);
                                bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, string.Empty, hidbmid, hiddenid); //是否具有同级提交权限
                                bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                        model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定
                        model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid);
                        bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有不必填权限
                        bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                        model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                            mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                            mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                            model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                            model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限
                            bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", null, null, hidtype, hidbmid,string.Empty,hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                                mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                                mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                                bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限
                                bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = data };
        }
        #endregion

        #region 隐患级别
        /// <summary>
        /// 隐患级别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHidRank([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string hiddepart = res.Contains("hiddepart") ? dy.data.hiddepart : "";  //所属单位
            string rankid = res.Contains("rankid") ? dy.data.rankid.ToString() : ""; //隐患级别id
            string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid.ToString() : "";//隐患id
            string marjorclassify = res.Contains("marjorclassify") ? dy.data.marjorclassify : ""; //隐患专业分类
            string workstream = res.Contains("workstream") ? dy.data.workstream : "";  //流程状态
            string hidtype = res.Contains("hidtype") ? dy.data.hidtype : "";  //隐患类别
            string hidbmid = res.Contains("hidbmid") ? dy.data.hidbmid : "";  //所属部门id
            List<HidRankNewData> data = new List<HidRankNewData>();
            string mark = string.Empty;

            try
            {
                #region 配置项目
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidRank','HidMajorClassify','HidBmEnableOrganize','AcceptPersonControl','IsEnableMajorClassify','AppHidRankControl','ControlPicMustUpload'").ToList();//编码集合
                var rankitemlist = itemlist.Where(p => p.EnCode == "HidRank").ToList(); //隐患级别
                var majoritemlist = itemlist.Where(p => p.EnCode == "HidMajorClassify").ToList();//隐患专业分类集合 
                var bmIitemlist = itemlist.Where(p => p.EnCode == "HidBmEnableOrganize").ToList();//'HidBmEnableOrganize'
                var appHidRankControl = itemlist.Where(p => p.EnCode == "AppHidRankControl").ToList();// 移动端隐患级别获取控制                                                                                  //相关图片必传控制配置
                string ControlPicMustUpload = string.Empty;
                var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == curUser.OrganizeId);
                if (cpmu.Count() > 0)
                {
                    ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
                }

                int getIndex = -1;
                if (appHidRankControl.Count() > 0)
                {
                    var tempAppHidRank = appHidRankControl.Where(p => p.ItemName == "HidRankControlDispatch");
                    if (tempAppHidRank.Count() > 0)
                    {
                        getIndex = int.Parse(tempAppHidRank.FirstOrDefault().ItemValue.Trim());
                    }

                }
                if (curUser.RoleName.Contains("省级用户"))
                {
                    mark = "省级隐患排查";
                }
                else
                {
                    mark = "厂级隐患排查";
                }


                //获取是否设置验收人权限
                string authdeptid = !string.IsNullOrEmpty(hiddepart) ? hiddepart : curUser.OrganizeId;
                string argsCode = curUser.OrganizeCode;
                if (string.IsNullOrEmpty(hiddepart))
                {
                    var deptEntity = departmentBLL.GetEntity(authdeptid);
                    if (null != deptEntity)
                    {
                        argsCode = deptEntity.DeptCode;
                    }
                }
                bool IsEnableAccept = itemlist.Where(p => p.EnCode == "AcceptPersonControl").Where(p => p.ItemValue == argsCode).Count() > 0; //是否启用验收人控制 
                bool IsEnableMajorClassify = itemlist.Where(p => p.EnCode == "IsEnableMajorClassify").Where(p => p.ItemValue == argsCode).Count() > 0;//是否启用专业分类 

                #endregion

                if (getIndex == 0) //仅仅只获取隐患级别，不调用任何权限控制服务； 
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = false; //判定是否存在上报功能权限
                        model.isshowappoint = false; //是否具有指定
                        model.ismustwrite = false;
                        model.ishavetjsubmit = false;
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                        model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                        model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                            mcentity.isupsubmit = false; //判定是否存在上报功能权限
                            mcentity.ismustwrite = false;
                            mcentity.ishavetjsubmit = false;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = false; //判定是否存在上报功能权限
                            model.isshowappoint = false; //是否具有指定整改责任人权限
                            model.ismustwrite = false;
                            model.ishavetjsubmit = false; //是否具有同级提交权限 //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                                mcentity.isupsubmit = false; //判定是否存在上报功能权限
                                mcentity.ismustwrite = false;
                                mcentity.ishavetjsubmit = false; //是否具有同级提交权限 //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                                mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                                mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }
                else if (getIndex == 1) //隐患级别调用权限控制服务，专业分类不调取； 
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                        model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定
                        model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid);
                        bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有不必填权限
                        bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                        model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                        model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                        model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                            mcentity.isupsubmit = false; //判定是否存在上报功能权限
                            mcentity.ismustwrite = false;
                            mcentity.ishavetjsubmit = false;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                            model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                            model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限
                            bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = false; //是否具有指定整改责任人权限
                                mcentity.isupsubmit = false; //判定是否存在上报功能权限
                                mcentity.ismustwrite = false;
                                mcentity.ishavetjsubmit = false;//是否具有同级提交权限 //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                                mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                                mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }
                else if (getIndex == 2) //隐患级别不调用权限控制服务，专业分类则调取权限控制服务； 
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = false; //判定是否存在上报功能权限
                        model.isshowappoint = false; //是否具有指定
                        model.ismustwrite = false;
                        model.ishavetjsubmit = false; //是否具有不必填权限  //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                        model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                        model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty,hiddenid); //是否具有指定整改责任人权限
                            mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty,hiddenid); //判定是否存在上报功能权限
                            mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty,hiddenid);
                            bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty,hiddenid);
                            bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty,hiddenid);
                            mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = false; //判定是否存在上报功能权限
                            model.isshowappoint = false; //是否具有指定整改责任人权限
                            model.ismustwrite = false;
                            model.ishavetjsubmit = false; //是否具有同级提交权限  //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                                mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                                mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                                bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限
                                bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                                mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                                mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 通用调用
                    if (!string.IsNullOrEmpty(rankid))
                    {
                        List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                        HidRankNewData model = new HidRankNewData();
                        model.isenableaccept = IsEnableAccept;
                        model.hidrank = rankid;
                        model.hidrankname = rankitemlist.Where(p => p.ItemDetailId == rankid).Count() > 0 ? rankitemlist.Where(p => p.ItemDetailId == rankid).FirstOrDefault().ItemName : string.Empty;
                        model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                        model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定
                        model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid);
                        bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有不必填权限
                        bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                        model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                        model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                        model.isenablemajorclassify = IsEnableMajorClassify;
                        model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                        model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                        model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                        foreach (DataItemModel majorentity in majoritemlist)
                        {
                            HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                            mcentity.isenableaccept = IsEnableAccept;
                            mcentity.majorclassify = majorentity.ItemDetailId;
                            mcentity.majorclassifyname = majorentity.ItemName;
                            mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                            mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                            mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                            mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                            mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            mcentity.isenablemajorclassify = IsEnableMajorClassify;
                            mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            mclist.Add(mcentity);
                        }
                        model.majordata = mclist;
                        data.Add(model);
                    }
                    else
                    {
                        foreach (DataItemModel entity in rankitemlist)
                        {
                            List<HidMajorClassifyNewData> mclist = new List<HidMajorClassifyNewData>();
                            HidRankNewData model = new HidRankNewData();
                            model.isenableaccept = IsEnableAccept;
                            model.hidrank = entity.ItemDetailId;
                            model.hidrankname = entity.ItemName;
                            model.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", "", null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                            model.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                            model.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid);
                            bool ishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限
                            bool ishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", null, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                            model.ishavetjsubmit = ishavesubmit || ishavetjsubmit;
                            model.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                            model.isenablemajorclassify = IsEnableMajorClassify;
                            model.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                            model.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                            model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                            foreach (DataItemModel majorentity in majoritemlist)
                            {
                                HidMajorClassifyNewData mcentity = new HidMajorClassifyNewData();
                                mcentity.isenableaccept = IsEnableAccept;
                                mcentity.majorclassify = majorentity.ItemDetailId;
                                mcentity.majorclassifyname = majorentity.ItemName;
                                mcentity.isshowappoint = GetCurUserWfAuth(model.hidrank, workstream, "制定整改计划", mark, "制定提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                                mcentity.isupsubmit = GetCurUserWfAuth(model.hidrank, workstream, "", mark, "上报", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                                mcentity.ismustwrite = GetCurUserWfAuth(model.hidrank, workstream, "隐患整改", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid);
                                bool mcishavesubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限
                                bool mcishavetjsubmit = GetCurUserWfAuth(model.hidrank, "隐患评估", "隐患评估", mark, "同级提交", mcentity.majorclassify, null, hidtype, hidbmid, string.Empty, hiddenid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                                mcentity.ishavetjsubmit = mcishavesubmit || mcishavetjsubmit;
                                mcentity.isenablebm = bmIitemlist.Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0;
                                mcentity.isenablemajorclassify = IsEnableMajorClassify;
                                mcentity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");
                                mcentity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");
                                mcentity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");
                                mclist.Add(mcentity);
                            }
                            model.majordata = mclist;
                            data.Add(model);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = data };
        }
        #endregion

        #region 专业分类
        /// <summary>
        /// 隐患专业分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetMajorClassify()
        {
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidMajorClassify'");

            DefaultDataSettingBLL defaultdatasettingbll = new DefaultDataSettingBLL();
            List<DefaultDataSettingEntity> list = defaultdatasettingbll.GetList(curUser.UserId).ToList(); //排序
            List<object> result = new List<object>();
            foreach (DataItemModel model in itemlist)
            {
                result.Add(new { majorclassify = model.ItemDetailId, majorclassifyname = model.ItemName, isdefault = list.Where(p => p.DEFAULTKEY == model.ItemDetailId).ToList().Count() > 0 });
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = result };
        }

        /// <summary>
        /// 隐患专业分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetGenericMajorClassify()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'GIHiddenClassify'");

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { majorclassify = x.ItemDetailId, majorclassifyname = x.ItemName }) };
        }
        #endregion

        #region 台账类型
        /// <summary>
        /// 台账类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetStandingType()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidStandingType'");

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { standingvalue = x.ItemValue, standingname = x.ItemName }) };
        }
        #endregion

        #region 隐患检查类型
        /// <summary>
        /// 隐患检查类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCheckType([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string type = res.Contains("ckmark") ? dy.data.ckmark : "";
            List<DataItemModel> itemlist = new List<DataItemModel>();
            itemlist = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckType'").ToList();
            if (!string.IsNullOrEmpty(type))
            {
                //去掉日常
                if (curUser.RoleName.Contains("省级用户"))
                {
                    var model = itemlist.Where(p => p.ItemName.Contains("日常安全检查")).FirstOrDefault();
                    if (null != model)
                    {
                        itemlist.Remove(model);
                    }
                }
                else
                {
                    var model = itemlist.Where(p => p.ItemName.Contains("其他安全检查")).FirstOrDefault();
                    if (null != model)
                    {
                        itemlist.Remove(model);
                    }
                }
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { checktype = x.ItemDetailId, checktypename = x.ItemName, checktypevalue = x.ItemValue }) };
        }
        #endregion

        #region 隐患类别
        /// <summary>
        ///隐患类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHidType([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string checktypeid = res.Contains("checktypeid") ? dy.data.checktypeid : ""; //安全检查类型id

            DefaultDataSettingBLL defaultdatasettingbll = new DefaultDataSettingBLL();
            List<DefaultDataSettingEntity> defaultlist = defaultdatasettingbll.GetList(curUser.UserId).ToList(); //排序 
            var data = dataitemdetailbll.GetDataItemListByItemCode("'HidType'").ToList();
            var basedata = data.Where(p => p.ItemCode == "0");
            //根据安全检查类型id获取隐患类别
            if (!string.IsNullOrEmpty(checktypeid))
            {
                var checktypeitem = dataitemdetailbll.GetEntity(checktypeid);
                if (null != checktypeitem)
                {
                    basedata = data.Where(p => p.Description.Trim().Contains(checktypeitem.ItemValue.Trim())).ToList();
                }
            }
            List<HidTypeData> list = new List<HidTypeData>();

            foreach (DataItemModel item in basedata)
            {
                HidTypeData entity = new HidTypeData();
                entity.hidtype = item.ItemDetailId;
                entity.hidtypename = item.ItemName;
                entity.parentid = item.ItemCode;
                entity.isparent = data.Where(p => p.ItemCode == item.ItemDetailId).Count() > 0;
                entity.isdefault = defaultlist.Where(p => p.DEFAULTKEY == item.ItemDetailId).Count() > 0;
                entity.children = GetHidTypeData(data, defaultlist, item.ItemDetailId);
                list.Add(entity);
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = list };
        }

        public List<HidTypeData> GetHidTypeData(List<DataItemModel> dlist, List<DefaultDataSettingEntity> defaultlist, string parentid)
        {
            List<HidTypeData> list = new List<HidTypeData>();
            var dataitemlist = dlist.Where(p => p.ItemCode == parentid).ToList();
            foreach (DataItemModel data in dataitemlist)
            {
                HidTypeData entity = new HidTypeData();
                entity.hidtype = data.ItemDetailId;
                entity.hidtypename = data.ItemName;
                entity.parentid = data.ItemCode;
                entity.isdefault = defaultlist.Where(p => p.DEFAULTKEY == data.ItemDetailId).Count() > 0;
                entity.children = GetHidTypeData(dlist, defaultlist, entity.hidtype);
                list.Add(entity);
            }
            return list;
        }
        #endregion

        #region 隐患状态
        /// <summary>
        /// 隐患状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHiddenStatus()
        {
            /*
            List<object> list = new List<object>();
            list.Add(new { name = "制定整改计划", id = "制定整改计划" });
            list.Add(new { name = "逾期未整改", id = "逾期未整改" });
            list.Add(new { name = "延期整改", id = "延期整改" });
            list.Add(new { name = "未整改", id = "未整改" });
            list.Add(new { name = "已整改", id = "已整改" });
            list.Add(new { name = "未闭环", id = "未闭环" });
            list.Add(new { name = "已闭环", id = "已闭环" });
            list.Add(new { name = "挂牌督办", id = "挂牌督办" });
            return new { code = 0, info = "获取数据成功", count = 0, data = list };
             */
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'ChangeStatus'");

            return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { id = x.ItemName, name = x.ItemName }) };

        }
        #endregion

        #region   行业版本隐患类别
        /// <summary>
        ///  行业版本隐患类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetGenericHidType([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string majorclassifyId = dy.data.majorclassifyId;

            DataItemDetailEntity detailitem = dataitemdetailbll.GetEntity(majorclassifyId);

            List<HidTypeData> list = new List<HidTypeData>();

            string detailcode = string.Empty;
            if (null != detailitem)
            {
                detailcode = detailitem.ItemCode;
            }
            var itemlist = dataitemdetailbll.GetDataItemByDetailCode("GIHiddenType", detailcode).ToList();

            foreach (DataItemModel item in itemlist)
            {
                HidTypeData entity = new HidTypeData();
                entity.hidtype = item.ItemDetailId;
                entity.hidtypename = item.ItemName;
                entity.parentid = item.ItemCode;
                entity.isparent = itemlist.Where(p => p.ItemCode == item.ItemDetailId).Count() > 0;
                entity.children = GetHidTypeData(itemlist, new List<DefaultDataSettingEntity>(), item.ItemDetailId);
                list.Add(entity);
            }

            return new { code = 0, info = "获取数据成功", count = list.Count(), data = list };
        }
        #endregion

        #region 隐患排查标准
        /// <summary>
        /// 隐患排查标准
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHidStandard([FromBody]JObject json)
        {
            //获取自己的新增隐患
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            var list = new List<hidstandard>();
            string orgid = res.Contains("orgid") ? dy.data.orgid : "";
            string parentid = res.Contains("parentid") ? dy.data.parentid : "0";
            try
            {
                string code = curUser.OrganizeCode;
                //省级用户通过所属单位来选择控制
                if (curUser.RoleName.Contains("省级用户"))
                {
                    if (!string.IsNullOrEmpty(orgid))
                    {
                        code = departmentBLL.GetEntity(orgid).EnCode;
                    }
                }
                if (!string.IsNullOrEmpty(code))
                {
                    list = GetHidStandardById(code, parentid);
                }
                else { return new { code = -1, info = "获取数据失败", count = 0 }; }
            }
            catch (Exception)
            {
                return new { code = -1, info = "获取数据失败", count = 0 };
            }
            return new { code = 0, info = "获取数据成功", count = list.Count(), data = list };

        }
        #endregion

        #region 隐患级别标准
        [HttpPost]
        public object GetHidRankStandard([FromBody]JObject json)
        {
            //获取自己的新增隐患
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            DataItemModel itemmode = new DataItemModel();
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HiddenRankStandardLib'");
            if (itemlist.Count() > 0)
            {
                var curItem = itemlist.Where(p => p.ItemValue == curUser.OrganizeId);
                if (curItem.Count() > 0)
                {
                    itemmode = curItem.FirstOrDefault();
                }
                else
                {
                    itemmode = itemlist.Where(p => p.ItemValue == "HidRankBaseStandard").FirstOrDefault();
                }
            }
            //itemmode.Description
            return new { code = 0, info = "获取数据成功", count = 0, data = itemmode.Description };
        }
        #endregion

        #region 隐患标准
        public List<hidstandard> GetHidStandardById(string organizecode, string id)
        {
            var bll = new HtStandardBLL();
            List<hidstandard> list = new List<hidstandard>();
            var allList = bll.GetList(string.Format(" and CreateUserOrgCode='{0}'", organizecode)).OrderBy(t => t.EnCode).ToList();
            if (allList.Count > 0)
            {
                var slist = allList.Where(x => x.Parentid == id).ToList();
                if (slist.Count() > 0)
                {
                    foreach (ERCHTMS.Entity.RiskDatabase.HtStandardEntity item in slist)
                    {
                        #region 部门
                        hidstandard model = new hidstandard();
                        model.id = item.Id;
                        model.parentid = item.Parentid;
                        model.standardname = item.Name;
                        model.isparent = allList.Count(x => x.Parentid == item.Id) > 0;
                        model.children = model.isparent == false ? new List<hidstandard>() : GetHidStandardChildren(item.Id, allList);
                        var ilist = new List<hidstandarditem>();
                        if (model.isparent == false)
                        {
                            var clist = new HtStandardItemBLL().GetItemList(item.Id);
                            foreach (HtStandardItemEntity citem in clist)
                            {
                                hidstandarditem smodel = new hidstandarditem();
                                smodel.content = citem.Content;
                                smodel.require = citem.Require;
                                smodel.norm = citem.Norm;
                                ilist.Add(smodel);
                            }

                        }
                        model.standarddata = ilist;
                        list.Add(model);
                        #endregion
                    }
                }
            }

            return list;
        }
        public List<hidstandard> GetHidStandardChildren(string parentId, List<HtStandardEntity> listAll)
        {
            var bll = new HtStandardBLL();
            var children = new List<hidstandard>();
            var slist = listAll.Where(x => x.Parentid == parentId).ToList();
            if (slist.Count() > 0)
            {
                foreach (ERCHTMS.Entity.RiskDatabase.HtStandardEntity item in slist)
                {
                    #region 部门
                    hidstandard model = new hidstandard();
                    model.id = item.Id;
                    model.parentid = item.Parentid;
                    model.standardname = item.Name;
                    model.isparent = listAll.Count(x => x.Parentid == item.Id) > 0;
                    model.children = model.isparent == false ? new List<hidstandard>() : GetHidStandardChildren(item.Id, listAll);
                    var ilist = new List<hidstandarditem>();
                    if (model.isparent == false)
                    {
                        var clist = new HtStandardItemBLL().GetItemList(item.Id);
                        foreach (HtStandardItemEntity citem in clist)
                        {
                            hidstandarditem smodel = new hidstandarditem();
                            smodel.content = citem.Content;
                            smodel.require = citem.Require;
                            smodel.norm = citem.Norm;
                            ilist.Add(smodel);
                        }
                    }
                    model.standarddata = ilist;
                    children.Add(model);
                    #endregion
                }
            }
            return children;
        }
        #endregion

        #region 已有隐患描述
        /// <summary>
        /// 已有隐患描述
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHidDescribe([FromBody]JObject json)
        {
            //获取自己的新增隐患
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hiddescribe = dy.data.hiddescribe;

            var dt = htbaseinfobll.GetDescribeListByUserId(curUser.UserId, hiddescribe);

            return new { code = 0, info = "获取数据成功", count = 0, data = dt.Select().Select(x => new { hiddescribe = x.Field<string>("hiddescribe"), changemeasure = x.Field<string>("changemeasure") }) };
        }
        #endregion

        #region 判定当前用户是否为安全管理员身份
        /// <summary>
        /// 获取当前用户是否为安全管理员身份
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object QuerySafetyRole([FromBody]JObject json)
        {
            //获取自己的新增隐患
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string uModel = string.Empty;

            try
            {

                //安全管理员角色编码
                string roleCode = dataitemdetailbll.GetItemValue("HidApprovalSetting");

                string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

                string[] pstr = HidApproval.Split('#');  //分隔机构组

                IList<UserEntity> ulist = new List<UserEntity>();

                foreach (string strArgs in pstr)
                {
                    string[] str = strArgs.Split('|');
                    //当前机构相同，且为本部门安全管理员验证
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                    {
                        ulist = userbll.GetUserListByRole(curUser.DeptCode, roleCode, curUser.OrganizeId).ToList();

                        break;
                    }
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                    {
                        //获取指定部门的所有人员
                        ulist = new UserBLL().GetUserListByDeptCode(str[2].ToString(), null, false, curUser.OrganizeId).ToList();

                        break;
                    }
                }
                if (ulist.Count() > 0)
                {
                    //返回的记录数,大于0，标识当前用户拥有安全管理员身份，反之则无
                    uModel = ulist.Where(p => p.UserId == curUser.UserId).Count().ToString();
                }
            }
            catch (Exception)
            {
                return new { code = -1, info = "获取数据失败", count = 0 };
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = uModel };
        }
        #endregion

        #region 判定是否是指定部门
        /// <summary>
        /// 是否是指定部门
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object IsAssignDepartment([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

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
                return new { code = 0, info = "获取数据成功", count = 0, data = "1" };
            }
            else
            {
                return new { code = -1, info = "获取数据成功", count = 0, data = "0" };
            }

        }
        #endregion

        #region  判定当前用户是否为回复人身份
        /// <summary>
        ///  获取当前用户是否为回复人身份
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object QueryReplayerRole([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //部门负责人角色编码
            string roleCode = dataitemdetailbll.GetItemValue("HidPrincipalSetting");

            IList<UserEntity> ulist = userbll.GetUserListByRole(curUser.DeptCode, roleCode, curUser.OrganizeId).ToList();

            //返回的记录数,大于0，标识当前用户拥有安全管理员身份，反之则无
            string uModel = ulist.Where(p => p.UserId == curUser.UserId).Count().ToString();

            return new { code = 0, info = "获取数据成功", count = 0, data = uModel };
        }
        #endregion

        #region 获取检查人员选项(选择人员)
        /// <summary>
        /// 获取检查人员选项
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCheckPerson([FromBody]JObject json)
        {
            //获取自己的新增隐患
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string userArgs = dy.data.realname; //传入的用户名称

            string dutydeptid = dy.data.dutydeptid; //传入的部门编码

            string orgid = res.Contains("orgid") ? dy.data.orgid : ""; //对应所属单位id

            string reqmark = res.Contains("reqmark") ? dy.data.reqmark : ""; //请求标识

            string threeperson = res.Contains("threeperson") ? dy.data.threeperson : ""; //三种人

            string relevancedeptid = res.Contains("relevancedeptid") ? dy.data.relevancedeptid : ""; //关联部门id 

            string majorclassify = res.Contains("majorclassify") ? dy.data.majorclassify : ""; //专业分类id 

            string sjorgid = string.Empty;

            if (curUser.RoleName.Contains("省级用户"))
            {
                sjorgid = curUser.OrganizeId; //当前用户id
            }
            if (string.IsNullOrEmpty(orgid))
            {
                orgid = curUser.OrganizeId;
            }

            //整改责任
            if (!string.IsNullOrEmpty(relevancedeptid))
            {
                string rolestr = string.Empty;
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'ChangeDeptRelevancePerson'");
                if (itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Count() > 0)
                {
                    rolestr = itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Where(p => p.ItemName == curUser.OrganizeCode).FirstOrDefault().ItemValue;
                }
                if (string.IsNullOrEmpty(rolestr))
                {
                    rolestr = "100104,100105"; //负责人 安全管理员
                }
                var list = userbll.GetUserListByAnyCondition(orgid, relevancedeptid, rolestr, majorclassify);
                return new
                {
                    code = 0,
                    info = "获取数据成功",
                    count = 0,
                    data = list.Select(x =>
                        new
                        {
                            checkperson = x.RealName,
                            checkpersonid = x.UserId,
                            checkdept = x.DeptName,
                            checkdeptcode = x.DepartmentCode,
                            checkdeptid = x.DepartmentId,
                            telphone = x.Telephone,
                            account = x.Account,
                            dutyname = x.DutyName,
                            identifyID = x.IdentifyID,
                        })
                };
            }
            else
            {        //获取所有人员
                var list = userbll.GetAllTableByArgs(userArgs, dutydeptid, orgid, sjorgid, reqmark, threeperson);
                return new
                {
                    code = 0,
                    info = "获取数据成功",
                    count = 0,
                    data = list.Select().Select(x =>
                        new
                        {
                            checkperson = x.Field<string>("realname"),
                            checkpersonid = x.Field<string>("userid"),
                            checkdept = x.Field<string>("departmentname"),
                            checkdeptcode = x.Field<string>("departmentcode"),
                            checkdeptid = x.Field<string>("departmentid"),
                            telphone = x.Field<string>("mobile"),
                            account = x.Field<string>("account"),
                            dutyname = x.Field<string>("dutyname"),
                            identifyID = x.Field<string>("identifyid")
                        })
                };
            }
        }
        #endregion

        #region 权限控制获取(国电新疆红雁池)
        [HttpPost]
        public object GetSelfChangeAuth([FromBody]JObject json)
        {
            //获取自己的新增隐患
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            bool issucess = false;

            HidLimits entity = new HidLimits();  //待返回的结果集

            List<HidAuthData> list = new List<HidAuthData>(); //具体权限项目

            //提交  隐患登记-隐患整改
            //登记阶段
            string GDXJ_HYC_ORGCODE = dataitemdetailbll.GetItemValue("GDXJ_HYC_ORGCODE");
            //国电新疆红雁池专用
            entity.isgdxj = curUser.OrganizeCode == GDXJ_HYC_ORGCODE;
            if (entity.isgdxj)
            {
                entity.reformtype = "3";
                entity.mustremark = "1:表示所有整改内容不必填;2:表示所有整改内容必填;3:表示只有整改部门必填，其他不必填";


                //本部门整改情况下
                #region 本部门整改、一般隐患登记
                issucess = GetCurUserWfAuthOfGDXJ("一般隐患", "提交", "隐患登记", "隐患整改", "厂级隐患排查");
                HidAuthData bbmybentity = new HidAuthData();
                bbmybentity.currentflow = "隐患登记";
                bbmybentity.rankname = "一般隐患";
                bbmybentity.isselfchange = 1;  //是本部门整改
                if (issucess)
                {
                    bbmybentity.mustwrite = 2;
                }
                else
                {
                    if (curUser.RoleName.Contains("部门级用户") && curUser.RoleName.Contains("安全管理员"))
                    {
                        bbmybentity.mustwrite = 2;
                    }
                    else
                    {
                        bbmybentity.mustwrite = 1;
                    }
                }
                list.Add(bbmybentity);
                #endregion

                issucess = false;

                #region 本部门整改、重大隐患登记
                issucess = GetCurUserWfAuthOfGDXJ("重大隐患", "提交", "隐患登记", "隐患整改", "厂级隐患排查");
                HidAuthData bbmzdentity = new HidAuthData();
                bbmzdentity.currentflow = "隐患登记";
                bbmzdentity.rankname = "重大隐患";
                bbmzdentity.isselfchange = 1;  //是本部门整改
                if (issucess)
                {
                    bbmzdentity.mustwrite = 2;
                }
                else
                {
                    if (curUser.RoleName.Contains("部门级用户") && curUser.RoleName.Contains("安全管理员"))
                    {
                        bbmzdentity.mustwrite = 2;
                    }
                    else
                    {
                        bbmzdentity.mustwrite = 1;
                    }
                }
                list.Add(bbmzdentity);
                #endregion

                issucess = false;

                #region 非本部门整改、一般隐患登记
                issucess = GetCurUserWfAuthOfGDXJ("一般隐患", "制定提交", "隐患登记", "制定整改计划", "厂级隐患排查");
                HidAuthData fbbmybentity = new HidAuthData();
                fbbmybentity.currentflow = "隐患登记";
                fbbmybentity.rankname = "一般隐患";
                fbbmybentity.isselfchange = 0;  //非本部门整改
                if (issucess)
                {
                    fbbmybentity.mustwrite = 3;
                }
                else
                {
                    fbbmybentity.mustwrite = 1;
                }
                list.Add(fbbmybentity);
                #endregion

                issucess = false;

                #region 非本部门整改、重大隐患登记
                issucess = GetCurUserWfAuthOfGDXJ("重大隐患", "制定提交", "隐患登记", "制定整改计划", "厂级隐患排查");
                HidAuthData fbbmzdentity = new HidAuthData();
                fbbmzdentity.currentflow = "隐患登记";
                fbbmzdentity.rankname = "重大隐患";
                fbbmzdentity.isselfchange = 0;  //非本部门整改
                if (issucess)
                {
                    fbbmzdentity.mustwrite = 3;
                }
                else
                {
                    fbbmzdentity.mustwrite = 1;
                }
                list.Add(fbbmzdentity);
                #endregion


                issucess = false;

                #region 一般隐患评估
                issucess = GetCurUserWfAuthOfGDXJ("一般隐患", "提交", "隐患评估", "隐患整改", "厂级隐患排查");
                HidAuthData ybpgEntity = new HidAuthData();
                ybpgEntity.currentflow = "隐患评估";
                ybpgEntity.rankname = "一般隐患";
                ybpgEntity.isselfchange = 1;  //本部门整改
                if (issucess)
                {
                    ybpgEntity.mustwrite = 2;//整改内容所有必填
                }
                else
                {
                    ybpgEntity.mustwrite = 1; //整改内容所有不必填
                }
                issucess = false;
                //一般安监部专用
                issucess = GetCurUserWfAuthOfGDXJ("一般隐患", "制定提交", "隐患评估", "制定整改计划", "厂级隐患排查");
                if (issucess)
                {
                    //如果当前用户没有提交  评估直接到整改，则必填整改部门
                    if (ybpgEntity.mustwrite == 1)
                    {
                        ybpgEntity.mustwrite = 3;
                    }
                }

                //部门级专用
                if (curUser.RoleName.Contains("部门级用户") && curUser.RoleName.Contains("安全管理员") && !curUser.RoleName.Contains("厂级部门用户"))
                {
                    ybpgEntity.mustwrite = 2;  //当为部门级安全管理员的时候，则一般隐患评估且本部门整改为必填。
                }
                list.Add(ybpgEntity);
                #endregion

                issucess = false;

                #region 一般隐患评估
                HidAuthData ybpgfbbmEntity = new HidAuthData();
                ybpgfbbmEntity.currentflow = "隐患评估";
                ybpgfbbmEntity.rankname = "一般隐患";
                ybpgfbbmEntity.isselfchange = 0;  //非本部门整改
                //部门级专用
                if (curUser.RoleName.Contains("部门级用户") && curUser.RoleName.Contains("安全管理员") && !curUser.RoleName.Contains("厂级部门用户"))
                {
                    ybpgfbbmEntity.mustwrite = 1;  //当为部门级安全管理员的时候，则一般隐患评估且本部门整改为必填。
                }
                else
                {
                    ybpgfbbmEntity.mustwrite = ybpgEntity.mustwrite;
                }
                list.Add(ybpgfbbmEntity);
                #endregion

                issucess = false;

                #region 重大隐患评估
                issucess = GetCurUserWfAuthOfGDXJ("重大隐患", "提交", "隐患评估", "隐患整改", "厂级隐患排查");
                HidAuthData zdpgEntity = new HidAuthData();
                zdpgEntity.currentflow = "隐患评估";
                zdpgEntity.rankname = "重大隐患";
                zdpgEntity.isselfchange = 1;  //本部门整改
                if (issucess)
                {
                    zdpgEntity.mustwrite = 2;
                }
                else
                {
                    zdpgEntity.mustwrite = 1;
                }
                issucess = false;

                issucess = GetCurUserWfAuthOfGDXJ("重大隐患", "制定提交", "隐患评估", "制定整改计划", "厂级隐患排查");
                if (issucess)
                {
                    if (zdpgEntity.mustwrite == 1)
                    {
                        zdpgEntity.mustwrite = 3;
                    }
                }

                //部门级专用
                if (curUser.RoleName.Contains("部门级用户") && curUser.RoleName.Contains("安全管理员") && !curUser.RoleName.Contains("厂级部门用户"))
                {
                    zdpgEntity.mustwrite = 2;  //当为部门级安全管理员的时候，则一般隐患评估且本部门整改为必填。
                }
                list.Add(zdpgEntity);
                #endregion

                issucess = false;

                #region 重大隐患评估
                HidAuthData zdpgfbbmEntity = new HidAuthData();
                zdpgfbbmEntity.currentflow = "隐患评估";
                zdpgfbbmEntity.rankname = "重大隐患";
                zdpgfbbmEntity.isselfchange = 0;  //非本部门整改
                //部门级专用
                if (curUser.RoleName.Contains("部门级用户") && curUser.RoleName.Contains("安全管理员") && !curUser.RoleName.Contains("厂级部门用户"))
                {
                    zdpgfbbmEntity.mustwrite = 1;  //当为部门级安全管理员的时候，则一般隐患评估且本部门整改为必填。
                }
                else
                {
                    zdpgfbbmEntity.mustwrite = zdpgEntity.mustwrite;
                }
                list.Add(zdpgfbbmEntity);
                #endregion

                entity.list = list;
            }
            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 0,
                data = entity
            };
        }


        public bool GetCurUserWfAuthOfGDXJ(string rankname, string submittype, string workflow, string endflow, string mark)
        {
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = ""; //
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankname = rankname;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;

            string resultVal = string.Empty;
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

            return result.ishave;
        }
        #endregion

        #endregion

        #region 获取所有隐患列表接口
        /// <summary>
        /// 获取所有隐患列表接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAllProblems([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string tokenId = dy.tokenid; //设备唯一标识

            int pageSize = int.Parse(dy.pagesize.ToString()); //每页的记录数

            int pageIndex = int.Parse(dy.pageindex.ToString());  //当前页索引

            string action = res.Contains("action") ? dy.data.action : ""; //请求类型

            string deptid = res.Contains("deptid") ? dy.data.deptid : "";  //所属单位

            string qdeptcode = res.Contains("qdeptcode") ? dy.data.qdeptcode : "";  //所属单位编码

            string registerdeptcode = res.Contains("registerdeptcode") ? dy.data.registerdeptcode : "";  //登记编码

            string checkdepart = res.Contains("checkdepart") ? dy.data.checkdepart : ""; //排查单位

            string problemareaid = res.Contains("problemareaid") ? dy.data.problemareaid : ""; //隐患区域

            string safetydetailid = res.Contains("safetydetailid") ? dy.data.safetydetailid : ""; //检查项ID

            string DeviceId = res.Contains("deviceid") ? dy.data.deviceid : ""; //检查项ID

            string districtid = res.Contains("districtid") ? dy.data.districtid : "";  //区域code

            string relevanceid = res.Contains("relevanceid") ? dy.data.relevanceid : "";  //关联应用

            string relevancetype = res.Contains("relevancetype") ? dy.data.relevancetype : "";  //关联应用类型

            string workstream = res.Contains("workstream") ? dy.data.workstream : "";  //流程状态

            string hiddenstatus = res.Contains("hiddenstatus") ? dy.data.hiddenstatus : "";  //隐患状态

            string standingtype = res.Contains("standingtype") ? dy.data.standingtype : "";  //台账类型

            string standingmark = res.Contains("standingmark") ? dy.data.standingmark : "";  //台账标记

            string tablename = string.Format(@" ( select a.DeviceId,a.account,a.modifydate,a.createuserdeptcode,a.createuserorgcode,a.createuserid,a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,a.id as hiddenid ,to_char(a.createdate,'yyyy-MM-dd') createdate,a.hidcode as problemid ,to_char(a.checkdate,'yyyy-MM-dd')checkdate,a.checkdepart,a.checkdepartid,a.checktype,a.checknumber,a.relevanceid,a.relevancetype,a.isbreakrule,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype,b.participant,c.applicationstatus ,b.actionperson,b.actionpersonname,a.hidtype,c.changedutydepartcode,c.changeperson,a.exposurestate,c.postponedept,c.postponedeptname ,c.postponeperson,c.postponepersonname, a.hiddescribe, c.changemeasure,c.changedeadine,a.safetycheckobjectid,d.acceptdepartcode,d.acceptperson, f.filepath, m.recheckperson,m.recheckpersonname,m.recheckdepartcode,m.recheckdepartname,a.hiddepart,a.hiddepartname,a.deptcode,(case when a.workstream ='整改结束' then 1 else 0 end)  ordernumber,(case when  a.workstream ='隐患登记' then '隐患登记' when a.workstream ='隐患评估' then  '评估中' when  a.workstream ='隐患完善' then '完善中' when a.workstream ='隐患整改' then '整改中' when  a.workstream ='制定整改计划' then '制定整改计划中'  when  a.workstream ='隐患验收' then '验收中' when  a.workstream ='复查验证' then '复查中' when  a.workstream ='整改效果评估' then '效果评估中' when a.workstream ='整改结束' then '整改结束' end )actionstatus,a.safetycheckname,c.chargeperson,c.chargepersonname,c.chargedeptid,c.chargedeptname,c.isappoint ,a.rolename,b.participantname,a.hidphoto,a.hidbmid,a.hidbmname,j.id changeplanid,k.createdate curapprovedate,l.createdate curacceptdate,(k.createdate +  m.beforeapprove / 24) beforeapprovedate ,(l.createdate + m.beforeaccept/24) beforeacceptdate,(k.createdate +  m.afterapprove/24) afterapprovedate ,(l.createdate + m.afteraccept/24) afteracceptdate from v_htbaseinfo a left join ( select a.id,a.participant,a.actionperson,a.participantname,a.participantname actionpersonname from v_workflow a) b on a.id = b.id left join v_htchangeinfo c on a.hidcode = c.hidcode left join v_htacceptinfo d on a.hidcode = d.hidcode left join v_htrecheck m  on a.hidcode = m.hidcode left join  (select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_htbaseinfo a left join base_fileinfo b on a.hidphoto = b.recid  group by a.id  ) f on  a.id = f.id  left join bis_changeplandetail j on a.id = j.hiddenid left join v_currenthtapprove k on a.id = k.objectid left join v_currenthtaccept l on a.id = l.objectid left join (select beforeapprove ,beforeaccept,afterapprove,afteraccept,organizeid from bis_expirationtimesetting where modulename ='Hidden' ) m on a.hiddepart = m.organizeid ) a ", dataitemdetailbll.GetItemValue("imgUrl"));
            Pagination pagination = new Pagination();
            pagination.p_tablename = tablename;
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "ordernumber asc , createdate desc ,modifydate desc";
            pagination.sord = "";
            pagination.conditionJson = " 1=1 ";
            pagination.p_fields = @"account,modifydate,createuserdeptcode,createuserorgcode,createuserid,checktypename,hidtypename,hidrankname,checkdepartname,
                                    createdate,problemid,checkdate,checktype,checknumber,isgetafter,relevanceid,relevancetype,
                                    isbreakrule ,hidtype, hidrank,hidplace,hidpoint,workstream,addtype,participant,applicationstatus,postponedept,
                                    postponedeptname,postponeperson,postponepersonname,hiddescribe,changemeasure,filepath,changedutydepartcode,hiddepartname,
                                    recheckdepartname,recheckpersonname,deptcode,checkdepart,actionpersonname,actionstatus,curapprovedate,curacceptdate,beforeapprovedate,beforeacceptdate,afterapprovedate,afteracceptdate";

            pagination.p_kid = "hiddenid";



            //台账标记
            if (!string.IsNullOrEmpty(standingmark))
            {
                pagination.conditionJson += @" and workstream != '隐患登记'";
            }
            //台账类型
            if (!string.IsNullOrEmpty(standingtype))
            {
                //pagination.conditionJson += @" and workstream != '隐患评估'";

                if (standingtype.Contains("公司级"))
                {
                    pagination.conditionJson += @" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  hidrankname  like  '%重大隐患%') ";
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and rolename  like  '%{0}%' and hidrankname  like  '%一般隐患%' and  rolename not like '%厂级%' ", standingtype);
                }
            }

            //组织机构
            if (!string.IsNullOrEmpty(curUser.OrganizeCode))
            {
                //省级单位
                if (curUser.RoleName.Contains("省级用户"))
                {
                    pagination.conditionJson += string.Format(@" and  deptcode  like '{0}%' ", curUser.NewDeptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and  hiddepart = '{0}' ", curUser.OrganizeId);
                }
            }
            //整改单位
            if (!string.IsNullOrEmpty(qdeptcode))
            {
                pagination.conditionJson += string.Format(@" and  changedutydepartcode like '{0}%' ", qdeptcode);
            }
            //登记单位
            if (!string.IsNullOrEmpty(registerdeptcode))
            {
                pagination.conditionJson += string.Format(@" and  createuserdeptcode like '{0}%' ", registerdeptcode);
            }
            //所属单位
            if (!string.IsNullOrEmpty(deptid))
            {
                pagination.conditionJson += string.Format(@" and  hiddepart = '{0}' ", deptid);
            }
            //排查单位
            if (!string.IsNullOrEmpty(checkdepart))
            {
                pagination.conditionJson += string.Format(@" and  checkdepartid = '{0}' ", checkdepart);
            }
            //区域
            if (!string.IsNullOrEmpty(districtid))
            {
                pagination.conditionJson += string.Format(@" and  hidpoint = '{0}' ", districtid.ToString());
            }
            //安全检查项目ID
            if (!string.IsNullOrEmpty(safetydetailid))
            {
                pagination.conditionJson += string.Format(@" and safetycheckobjectid ='{0}'", safetydetailid.ToString());
            }
            //设备ID
            if (!string.IsNullOrEmpty(DeviceId))
            {
                pagination.conditionJson += string.Format(@" and deviceid like '%{0}%'", DeviceId.ToString());
            }
            //应用ID
            if (!string.IsNullOrEmpty(relevanceid))
            {
                pagination.conditionJson += string.Format(@" and relevanceid like '%{0}%'", relevanceid.ToString());
            }
            //应用relevancetype
            if (!string.IsNullOrEmpty(relevancetype))
            {
                pagination.conditionJson += string.Format(@" and relevancetype = '{0}'", relevancetype.ToString());
            }

            //流程状态
            if (!string.IsNullOrEmpty(workstream))
            {
                pagination.conditionJson += string.Format(@" and workstream = '{0}'", workstream.ToString());
            }
            #region 隐患状态
            if (!string.IsNullOrEmpty(hiddenstatus))
            {
                switch (hiddenstatus)
                {
                    case "制定整改计划":
                        pagination.conditionJson += @" and workstream = '制定整改计划' ";
                        break;
                    case "逾期未评估":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患评估'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);
                        break;
                    case "即将到期未评估":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患评估'  and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate  ", DateTime.Now);
                        break;
                    case "未整改":
                        pagination.conditionJson += @" and workstream = '隐患整改' ";
                        break;
                    case "逾期未整改":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now);
                        break;
                    case "延期整改":
                        pagination.conditionJson += @" and  problemid in (select distinct hidcode from bis_htextension where handlesign ='1')";
                        break;
                    case "即将到期未整改":
                        pagination.conditionJson += @"and workstream = '隐患整改' and ((hidrankname like  '%一般隐患%'  and changedeadine - 3 <= 
                                                         sysdate  and sysdate <= changedeadine + 1 )  or (hidrankname like '%重大隐患%' and changedeadine - 5 <= sysdate and  sysdate <= changedeadine + 1 ) )";
                        break;
                    case "逾期未验收":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afteracceptdate", DateTime.Now);
                        break;
                    case "即将到期未验收":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeacceptdate   and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afteracceptdate ", DateTime.Now);
                        break;
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", curUser.UserId);
                        break;
                    case "已整改":
                        pagination.conditionJson += @" and   workstream in ('隐患验收','复查验证','整改效果评估','整改结束')"; //
                        break;
                    case "挂牌督办":
                        pagination.conditionJson += @" and  isgetafter ='1'";
                        break;
                    case "未整改结束":
                        pagination.conditionJson += @" and  workstream !='整改结束'";
                        break;
                    case "未闭环":
                        pagination.conditionJson += @" and  workstream !='整改结束' and  workstream !='隐患评估' ";
                        break;
                    case "已闭环":
                        pagination.conditionJson += @" and  workstream ='整改结束'";
                        break;
                }
            }
            #endregion

            string hidrank = string.Empty;
            string dutydept = string.Empty;
            string hiddescribe = string.Empty;

            switch (action)
            {
                //获取所有已发现的一般隐患接口
                case "1":
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and  workstream != '隐患完善' and  workstream != '制定整改计划' ";

                    pagination.conditionJson += @" and  hidrankname  like  '%一般隐患%'";
                    break;
                //获取所有已发现的重大隐患接口(安全生产指标)
                case "2":
                    if (curUser.RoleName.Contains("省级用户"))
                    {
                        pagination.conditionJson += string.Format("  and  deptcode like '{0}%'  and  to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy')='{1}' ", curUser.NewDeptCode, DateTime.Now.ToString("yyyy"));
                    }
                    else if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司管理员"))
                    {
                        pagination.conditionJson += "  and  changedutydepartcode like '" + curUser.OrganizeCode + "%'";
                    }
                    else
                    {
                        pagination.conditionJson += "  and changedutydepartcode like '" + curUser.DeptCode + "%'";
                    }

                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and  workstream != '隐患完善' and  workstream != '制定整改计划'";

                    pagination.conditionJson += string.Format(@" and  hidrankname  like '%重大%' and  to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy')='{0}'", DateTime.Now.ToString("yyyy"));
                    break;
                //获取所有整改中的一般隐患接口
                case "3":
                    pagination.conditionJson += @" and  hidrankname  like  '%一般隐患%' and workstream  = '隐患整改'";
                    break;
                //获取所有整改中的重大隐患接口
                case "4":
                    pagination.conditionJson += @" and  hidrankname  like '%重大%' and workstream  = '隐患整改'";
                    break;
                //获取所有逾期未整改的一般隐患接口
                case "5":
                    pagination.conditionJson += string.Format(@" and workstream  = '隐患整改'  and 
                         to_date('{0}','yyyy-mm-dd hh24:mi:ss')> ( changedeadine + 1 ) and hidrankname like  '%一般隐患%'", DateTime.Now);
                    break;
                //获取所有逾期未整改的重大隐患接口
                case "6":
                    pagination.conditionJson += string.Format(@" and workstream  = '隐患整改'  and 
                         to_date('{0}','yyyy-mm-dd hh24:mi:ss')> ( changedeadine + 1 ) and  hidrankname like '%重大%'", DateTime.Now);
                    break;
                //获取某一个区域下所有隐患的接口
                case "7":
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and  workstream != '隐患完善' and  workstream != '制定整改计划'  ";

                    pagination.conditionJson += string.Format(@" and  hidpoint like '{0}%'", problemareaid);
                    break;
                //获取所有待制定整改列表接口
                case "41":
                    pagination.conditionJson += @" and workstream   =  '制定整改计划'";
                    break;
                //获取所有待隐患完善列表接口
                case "33":
                    pagination.conditionJson += @" and workstream   =  '隐患完善' ";
                    break;
                //获取所有待评估与预评估隐患列表接口
                case "8":
                    pagination.conditionJson += @" and workstream   =  '隐患评估' ";
                    break;
                //获取所有整改中隐患列表接口
                case "9":
                    pagination.conditionJson += @" and workstream  = '隐患整改'";
                    break;
                //获取所有待复查验收的隐患列表接口
                case "10":
                    pagination.conditionJson += @" and workstream   = '隐患验收'";
                    break;
                //获取所有待复查验证列表接口
                case "34":
                    pagination.conditionJson += @" and workstream   =  '复查验证' ";
                    break;
                //获取所有隐患整改效果评估列表接口
                case "11":
                    pagination.conditionJson += @" and workstream  = '整改效果评估'";
                    break;
                //获取所有逾期未整改隐患列表接口(安全生产指标)
                case "12":
                    if (curUser.RoleName.Contains("省级用户"))
                    {
                        pagination.conditionJson += string.Format("  and  deptcode like '{0}%'  ", curUser.OrganizeCode);
                    }
                    else if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司管理员"))
                    {
                        pagination.conditionJson += "  and  changedutydepartcode like '" + curUser.OrganizeCode + "%'";
                    }
                    else
                    {
                        pagination.conditionJson += "  and changedutydepartcode like '" + curUser.DeptCode + "%'";
                    }

                    pagination.conditionJson += string.Format(@" and workstream  = '隐患整改'  and 
                         to_date('{0}','yyyy-mm-dd hh24:mi:ss')> ( changedeadine + 1 ) and   to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy')='{1}'", DateTime.Now, DateTime.Now.ToString("yyyy"));
                    break;
                //获取所有整改完成的隐患列表接口
                case "13":
                    pagination.conditionJson += @" and  changeresult ='1' and  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')";
                    break;
                //获取即将逾期的未整改隐患列表接口
                case "14":
                    pagination.conditionJson += @"and workstream = '隐患整改' and ((hidrankname like  '%一般隐患%' and changedeadine - 3 <= sysdate  and sysdate < changedeadine + 1 )
                          or (hidrankname like '%重大%' and changedeadine - 5 <= sysdate  and sysdate < changedeadine + 1 ) )";
                    break;
                //个人登记待完善隐患接口
                case "35":
                    pagination.conditionJson += string.Format(@" and  createuserid ='{0}' and workstream   = '隐患完善' ", curUser.UserId);
                    break;
                //个人登记待评估隐患接口
                case "15":
                    pagination.conditionJson += string.Format(@" and  createuserid ='{0}' and workstream   = '隐患评估' ", curUser.UserId);
                    break;
                //个人登记待整改的隐患接口
                case "16":
                    pagination.conditionJson += string.Format(@" and  createuserid ='{0}' and workstream   = '隐患整改' ", curUser.UserId);
                    break;
                //个人登记待验收的隐患接口
                case "17":
                    pagination.conditionJson += string.Format(@" and  createuserid ='{0}' and workstream   = '隐患验收' ", curUser.UserId);
                    break;
                //个人登记待复查验证隐患接口
                case "36":
                    pagination.conditionJson += string.Format(@" and  createuserid ='{0}' and workstream   = '复查验证' ", curUser.UserId);
                    break;
                //个人登记逾期未整改的隐患接口
                case "18":
                    pagination.conditionJson += string.Format(@" and  createuserid ='{0}' and workstream  = '隐患整改'  and 
                         to_date('{1}','yyyy-mm-dd hh24:mi:ss') > ( changedeadine + 1 )", curUser.UserId, DateTime.Now);
                    break;
                //个人待完善的隐患
                case "30":
                    pagination.conditionJson += string.Format(@" and  actionperson like '%,{0},%' and workstream  = '隐患完善'", curUser.Account);
                    break;
                //个人处理待制定整改计划隐患列表
                case "40":
                    pagination.conditionJson += string.Format(@" and actionperson  like   '%,{0},%' and workstream  = '制定整改计划'", curUser.Account);
                    break;
                //个人处理待用户评估隐患列表
                case "19":
                    pagination.conditionJson += string.Format(@" and actionperson  like   '%,{0},%' and workstream  = '隐患评估'", curUser.Account);
                    break;
                //个人处理待用户整改隐患列表
                case "20":
                    pagination.conditionJson += string.Format(@" and changeperson  =  '{0}' and workstream  = '隐患整改'  ", curUser.UserId);
                    break;
                //个人处理待用户验收隐患列表
                case "21":
                    pagination.conditionJson += string.Format(@" and actionperson  like   '%,{0},%' and workstream  = '隐患验收'", curUser.Account);
                    break;
                //个人处理待用户复查验证隐患列表
                case "31":
                    pagination.conditionJson += string.Format(@" and actionperson  like '%,{0},%' and workstream  = '复查验证'", curUser.Account);
                    break;
                //个人处理待用户效果评估隐患列表
                case "22":
                    pagination.conditionJson += string.Format(@" and  actionperson  like   '%,{0},%'  and  workstream  = '整改效果评估' ", curUser.Account);
                    break;
                //个人处理逾期未整改隐患列表
                case "23":
                    pagination.conditionJson += string.Format(@" and  changeperson ='{0}' and workstream  = '隐患整改'  and 
                         to_date('{1}','yyyy-mm-dd hh24:mi:ss') > ( changedeadine + 1 )", curUser.UserId, DateTime.Now);
                    break;
                //个人登记的隐患列表
                case "24":
                    pagination.conditionJson += string.Format(@" and createuserid = '{0}'", curUser.UserId);
                    break;
                //隐患曝光
                case "25":
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划' ";

                    pagination.conditionJson += @" and exposurestate = '1'";
                    break;
                //隐患台账
                case "26":
                    hidrank = res.Contains("hidrank") ? dy.data.hidrank : ""; //隐患级别
                    dutydept = res.Contains("dutydeptid") ? dy.data.dutydeptid : ""; //整改部门
                    hiddescribe = res.Contains("hiddescribe") ? dy.data.hiddescribe : ""; //隐患描述

                    pagination.conditionJson += @"  and workstream != '隐患登记'  "; //and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'

                    if (!string.IsNullOrEmpty(hidrank))
                    {
                        pagination.conditionJson += string.Format(@" and  hidrank='{0}' ", hidrank.ToString());
                    }
                    if (!string.IsNullOrEmpty(dutydept))
                    {
                        pagination.conditionJson += string.Format(@" and  changedutydepartcode  like '{0}%' ", dutydept.ToString());
                    }
                    if (!string.IsNullOrEmpty(workstream))
                    {
                        pagination.conditionJson += string.Format(@" and  workstream = '{0}' ", workstream.ToString());
                    }
                    if (!string.IsNullOrEmpty(hiddescribe))
                    {
                        pagination.conditionJson += string.Format(@" and  hiddescribe  like '%{0}%' ", hiddescribe.ToString());
                    }
                    //隐患区域
                    if (!string.IsNullOrEmpty(problemareaid))
                    {
                        pagination.conditionJson += string.Format(@" and  hidpoint like '{0}%'", problemareaid);
                    }
                    break;
                //隐患整改延期申请
                case "27":
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";

                    pagination.conditionJson += string.Format(@" and  (applicationstatus ='1' and postponeperson  like  '%,{0},%')", curUser.Account);

                    break;
                //隐患整改延期  (安全生产指标)
                case "28":
                    string dcode = string.Empty; //部门编码

                    if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司管理员"))
                    {
                        dcode = curUser.OrganizeCode;
                    }
                    else
                    {
                        dcode = curUser.DeptCode;
                    }

                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";

                    pagination.conditionJson += string.Format(@" and hiddenid in  (select distinct hidid from BIS_HTEXTENSION t where t.handledeptcode like '{0}%')", dcode);
                    break;
                //安全生产指标 隐患(安全生产指标)
                case "29":
                    if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司管理员"))
                    {
                        pagination.conditionJson += "  and  changedutydepartcode like '" + curUser.OrganizeCode + "%'";
                    }
                    else
                    {
                        pagination.conditionJson += "  and changedutydepartcode like '" + curUser.DeptCode + "%'";
                    }

                    hidrank = res.Contains("hidrank") ? dy.data.hidrank : ""; //隐患级别
                    dutydept = res.Contains("dutydeptid") ? dy.data.dutydeptid : ""; //整改部门
                    workstream = res.Contains("workstream") ? dy.data.workstream : ""; //流程状态
                    hiddescribe = res.Contains("hiddescribe") ? dy.data.hiddescribe : ""; //隐患描述

                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划' ";

                    if (!string.IsNullOrEmpty(hidrank))
                    {
                        pagination.conditionJson += string.Format(@" and  hidrank='{0}' ", hidrank.ToString());
                    }
                    if (!string.IsNullOrEmpty(dutydept))
                    {
                        pagination.conditionJson += string.Format(@" and  changedutydepartcode  like '{0}%' ", dutydept.ToString());
                    }
                    if (!string.IsNullOrEmpty(workstream))
                    {
                        pagination.conditionJson += string.Format(@" and  workstream = '{0}' ", workstream.ToString());
                    }
                    if (!string.IsNullOrEmpty(hiddescribe))
                    {
                        pagination.conditionJson += string.Format(@" and  hiddescribe  like '%{0}%' ", hiddescribe.ToString());
                    }
                    //隐患区域
                    if (!string.IsNullOrEmpty(problemareaid))
                    {
                        pagination.conditionJson += string.Format(@" and  hidpoint like '{0}%'", problemareaid);
                    }
                    break;
                //省公司发现的隐患
                case "37":
                    pagination.conditionJson += string.Format(@" and  checkdepartid like '{0}%'  and  to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy')='{1}' ", curUser.OrganizeCode, DateTime.Now.Year);

                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";
                    break;
                //省公司下整改率低于80%
                case "38":
                    pagination.conditionJson += string.Format(@" and  workstream  = '隐患整改' and  to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy')='{0}' ", DateTime.Now.Year);
                    break;
                //省公司下的重大隐患
                case "39":
                    pagination.conditionJson += string.Format(@"   and  hidrankname  like '%重大%'  and  to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy')='{0}' ", DateTime.Now.Year);
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";
                    break;
                //省公司下的重大隐患指标(本年度重大隐患+往年未整改完的重大隐患)
                case "42":
                    pagination.conditionJson += string.Format(@"   and  ((hidrankname  like '%重大%'  and  to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy') !='{0}' and workstream ='隐患整改'） or (hidrankname  like '%重大%'  and  to_char(to_date(createdate,'yyyy-mm-dd hh24:mi:ss'),'yyyy')='{0}')) ", DateTime.Now.Year);
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";
                    break;
                case "43":  //个人登记，已提交的
                    pagination.conditionJson += string.Format(@" and createuserid = '{0}' and workstream != '隐患登记'  ", curUser.UserId);
                    break;
            }
            var dt = htbaseinfobll.GetBaseInfoForApp(pagination);

            return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };

        }
        #endregion

        #region 新增保存隐患 / 提交隐患  / 一次性提交隐患 / 隐患完善  /
        /// <summary>
        ///  新增保存隐患 / 提交隐患  / 一次性提交隐患 / 隐患完善  / 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddHidTroublePush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {

                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                string wfFlag = "";

                string participant = "";

                string keyValue = "";

                string reformtype = dy.data.reformtype; //新增类型

                string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid : "";  //主键

                //隐患信息
                #region 隐患登记信息
                HTBaseInfoEntity entity = new HTBaseInfoEntity();
                string hidcode = res.Contains("problemid") ? dy.data.problemid : ""; //隐患编号
                entity.HIDCODE = hidcode;
                //如果hiddenid 不为空,则是编辑状态
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    entity = htbaseinfobll.GetEntity(hiddenid);
                }
                else
                {
                    //防止编码重复，如果多次提交同一个编码的，则进行覆盖处理(因此时hiddenid传过来为null)
                    IList<HTBaseInfoEntity> tlist = htbaseinfobll.GetListByCode(entity.HIDCODE);
                    if (tlist.Count() > 0)
                    {
                        hiddenid = tlist.FirstOrDefault().ID;
                    }
                }
                entity.APPSIGN = AppSign; //移动端标记
                entity.ISSELFCHANGE = res.Contains("isselfchange") ? dy.data.isselfchange : ""; //是否本部门整改
                entity.HIDRANK = res.Contains("hidrank") ? dy.data.hidrank : ""; //隐患级别  recdata.data.rankname
                entity.HIDPOINT = res.Contains("hidpointid") ? dy.data.hidpointid : "";// 所属区域code
                entity.HIDPOINTNAME = res.Contains("hidpoint") ? dy.data.hidpoint : ""; //所属区域名称
                entity.HIDTYPE = res.Contains("categoryid") ? dy.data.categoryid : "";  //隐患类别 recdata.data.category 
                entity.HIDDESCRIBE = res.Contains("hiddescribe") ? dy.data.hiddescribe : ""; //隐患描述

                entity.HIDBMID = res.Contains("hidbmid") ? dy.data.hidbmid : ""; //所属部门id
                entity.HIDBMNAME = res.Contains("hidbmname") ? dy.data.hidbmname : ""; //所属部门name

                entity.ISBREAKRULE = "0";  //违章行为（0:不是,1：是）
                //所属工程
                entity.HIDPROJECT = res.Contains("engineerid") ? dy.data.engineerid : "";  //所属工程
                entity.HIDPROJECTNAME = res.Contains("engineername") ? dy.data.engineername : "";  //所属工程名称

                //新增的部分字段
                entity.DEVICENAME = res.Contains("devicename") ? dy.data.devicename : "";  //设备名称
                entity.DEVICECODE = res.Contains("devicecode") ? dy.data.devicecode : "";  //设备编号
                entity.DEVICEID = res.Contains("deviceid") ? dy.data.deviceid : "";  //设备id
                entity.MONITORPERSONNAME = res.Contains("monitorpersonname") ? dy.data.monitorpersonname : "";  //厂级监控人员名称
                entity.MONITORPERSONID = res.Contains("monitorpersonid") ? dy.data.monitorpersonid : "";  //厂级监控人员Id
                entity.RELEVANCEID = res.Contains("relevanceid") ? dy.data.relevanceid : "";  //关联应用Id
                entity.RELEVANCETYPE = res.Contains("relevancetype") ? dy.data.relevancetype : "";  //关联其他应用标记
                entity.HIDNAME = res.Contains("hidname") ? dy.data.hidname : "";  //隐患名称
                entity.HIDSTATUS = res.Contains("hidstatus") ? dy.data.hidstatus : "";  //隐患现状
                entity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : "";  //专业分类
                entity.HIDCONSEQUENCE = res.Contains("hidconsequence") ? dy.data.hidconsequence : "";  //可能导致的后果

                entity.CHECKMAN = res.Contains("checkman") ? dy.data.checkman : ""; //排查人
                entity.CHECKMANNAME = res.Contains("checkmanname") ? dy.data.checkmanname : ""; //排查人员名称
                entity.CHECKDEPARTID = res.Contains("checkdept") ? dy.data.checkdept : "";//排查单位
                entity.CHECKDEPARTCODE = res.Contains("checkdeptcode") ? dy.data.checkdeptcode : ""; //排查单位code
                entity.CHECKDEPARTNAME = res.Contains("checkdeptname") ? dy.data.checkdeptname : ""; //排查单位名称
                entity.CHECKTYPE = res.Contains("checktypeid") ? dy.data.checktypeid : ""; //检查类型     recdata.data.checkType
                entity.SAFETYCHECKOBJECTID = res.Contains("safetydetailid") ? dy.data.safetydetailid : ""; //检查项目ID
                entity.SAFETYCHECKNAME = res.Contains("safetycheckname") ? dy.data.safetycheckname : ""; //检查名称
                string chosedcheckname = res.Contains("chosedcheckname") ? dy.data.chosedcheckname : ""; //检查名称选择标识 
                //安全检查  chosedcheckname 不为空时，表示当前选择的安全检查名称，非安全检查登记的
                if (!string.IsNullOrEmpty(entity.SAFETYCHECKOBJECTID) && !string.IsNullOrEmpty(chosedcheckname))
                {
                    entity.RELEVANCEID = new SaftyCheckDataRecordBLL().GetRecordFromHT(entity.SAFETYCHECKOBJECTID, curUser);
                }
                entity.CHECKDATE = res.Contains("checkdate") ? Convert.ToDateTime(dy.data.checkdate) : null;  //排查日期
                entity.HIDDEPART = res.Contains("deptid") ? dy.data.deptid : ""; //所属单位id
                entity.HIDDEPARTNAME = res.Contains("deptname") ? dy.data.deptname : ""; //所属单位名称

                //如果省公司登记则
                entity.ADDTYPE = reformtype;  //添加类型

                /*****重大隐患情况下****/

                entity.HIDPLACE = res.Contains("hidplace") ? dy.data.hidplace : ""; //隐患地点
                entity.REPORTDIGEST = res.Contains("reportdigest") ? dy.data.reportdigest : ""; //隐患报告摘要
                entity.HIDREASON = res.Contains("hidreason") ? dy.data.hidreason : ""; //隐患产生原因
                entity.HIDDANGERLEVEL = res.Contains("hiddangerlevel") ? dy.data.hiddangerlevel : ""; //隐患危害程度
                entity.PREVENTMEASURE = res.Contains("preventmeasure") ? dy.data.preventmeasure : ""; //主要预控及治理措施
                entity.HIDCHAGEPLAN = res.Contains("hidchageplan") ? dy.data.hidchageplan : ""; //隐患整改计划
                entity.EXIGENCERESUME = res.Contains("exigenceresume") ? dy.data.exigenceresume : ""; //应急措施简述
                entity.ISGETAFTER = res.Contains("isgetafter") ? dy.data.isgetafter : ""; //是否挂牌督办
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件

                string issubmit = res.Contains("isupsubmit") ? dy.data.isupsubmit : ""; //是否上报
                entity.UPSUBMIT = issubmit; //是否上报

                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(hiddenid))
                {
                    entity.HIDPHOTO = Guid.NewGuid().ToString();
                }

                //先删除图片
                DeleteFile(fileids);

                //上传隐患图片
                entity.HIDPHOTO = !string.IsNullOrEmpty(entity.HIDPHOTO) ? entity.HIDPHOTO : Guid.NewGuid().ToString();
                UploadifyFile(entity.HIDPHOTO, "problemimg", files);
                /********************************/
                //新增
                htbaseinfobll.SaveForm(hiddenid, entity);

                #endregion

                //主键为空
                #region  创建流程对象
                if (string.IsNullOrEmpty(hiddenid))
                {
                    string workFlow = "01";//隐患处理
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, curUser.UserId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //更新业务流程状态
                    }
                }
                #endregion

                //整改信息
                #region 隐患整改信息
                HTChangeInfoEntity centity = null;
                string changeID = string.Empty;
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    centity = htchangeinfobll.GetEntityByCode(hidcode);
                }
                if (null == centity)
                {
                    centity = new HTChangeInfoEntity();
                    changeID = "";
                }
                else
                {
                    changeID = centity.ID;
                }
                centity.APPSIGN = AppSign; //移动端标记
                centity.HIDCODE = entity.HIDCODE; //隐患编码
                centity.CHANGEPERSON = res.Contains("dutypersonid") ? dy.data.dutypersonid : "";//整改人
                centity.CHANGEPERSONNAME = res.Contains("dutyperson") ? dy.data.dutyperson : "";//整改人
                centity.CHANGEDUTYDEPARTCODE = res.Contains("dutydeptid") ? dy.data.dutydeptid : "";//整改部门code
                if (!string.IsNullOrEmpty(centity.CHANGEDUTYDEPARTCODE))
                {
                    var changeDept = departmentBLL.GetEntityByCode(centity.CHANGEDUTYDEPARTCODE);
                    centity.CHANGEDUTYDEPARTID = changeDept.DepartmentId;
                }
                centity.CHANGEDUTYDEPARTNAME = res.Contains("dutydept") ? dy.data.dutydept : ""; //整改部门
                centity.CHANGEDUTYTEL = res.Contains("dutytel") ? dy.data.dutytel : ""; //整改人电话
                string deadinetime = res.Contains("deadinetime") ? dy.data.deadinetime : null; //整改截止时间
                if (!string.IsNullOrEmpty(deadinetime))
                {
                    centity.CHANGEDEADINE = Convert.ToDateTime(deadinetime); //整改截至时间
                }
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null; //整改截至时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.CHANGEFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                //立即整改
                if (reformtype == "1")
                {
                    if (null != centity.CHANGEFINISHDATE)
                    {
                        centity.CHANGEDEADINE = centity.CHANGEFINISHDATE; //整改结束时间赋值给整改截止时间
                    }
                    else
                    {
                        centity.CHANGEFINISHDATE = DateTime.Now; //整改结束时间
                        centity.CHANGEDEADINE = DateTime.Now; //整改截止时间
                    }
                }
                centity.CHANGERESUME = res.Contains("reformdescribe") ? dy.data.reformdescribe : ""; // 整改情况描述
                //计划治理资金
                string planmanagecapital = res.Contains("planmanagecapital") ? (null != dy.data.planmanagecapital ? dy.data.planmanagecapital.ToString() : "0") : "0";
                centity.PLANMANAGECAPITAL = !string.IsNullOrEmpty(planmanagecapital) ? Convert.ToDecimal(planmanagecapital) : 0;//计划治理资金
                //实际治理资金
                string realitymanagecapital = res.Contains("realitymanagecapital") ? (null != dy.data.realitymanagecapital ? dy.data.realitymanagecapital.ToString() : "0") : "0";
                centity.REALITYMANAGECAPITAL = !string.IsNullOrEmpty(realitymanagecapital) ? Convert.ToDecimal(realitymanagecapital) : 0; //实际治理资金realitymanagecapital

                centity.CHANGEMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";  //整改措施
                centity.ISAPPOINT = res.Contains("isappoint") ? dy.data.isappoint : null; //是否指定整改责任人
                centity.CHARGEPERSON = res.Contains("chargeperson") ? dy.data.chargeperson : ""; //整改责任负责人
                centity.CHARGEPERSONNAME = res.Contains("chargepersonname") ? dy.data.chargepersonname : ""; //整改责任负责人
                centity.CHARGEDEPTID = res.Contains("chargedeptid") ? dy.data.chargedeptid : ""; //指定整改责任部门
                centity.CHARGEDEPTNAME = res.Contains("chargedeptname") ? dy.data.chargedeptname : ""; //指定整改责任部门
                centity.BACKREASON = ""; //退回原因
                if (string.IsNullOrEmpty(hiddenid))
                {
                    centity.HIDCHANGEPHOTO = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                //上传隐患图片
                centity.HIDCHANGEPHOTO = !string.IsNullOrEmpty(centity.HIDCHANGEPHOTO) ? centity.HIDCHANGEPHOTO : Guid.NewGuid().ToString();
                UploadifyFile(centity.HIDCHANGEPHOTO, "reformimg", files);

                centity.ATTACHMENT = !string.IsNullOrEmpty(centity.ATTACHMENT) ? centity.ATTACHMENT : Guid.NewGuid().ToString();
                //上传整改附件
                UploadifyFile(centity.ATTACHMENT, "attachment", files);
                /********************************/
                //新增
                htchangeinfobll.SaveForm(changeID, centity);

                #endregion

                //验收信息
                #region 隐患验收信息
                HTAcceptInfoEntity aentity = null;
                string acceptID = string.Empty;
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    aentity = htacceptinfobll.GetEntityByHidCode(hidcode);
                }
                if (null == aentity)
                {
                    aentity = new HTAcceptInfoEntity();
                    acceptID = "";
                }
                else
                {
                    acceptID = aentity.ID;
                }
                aentity.APPSIGN = AppSign; //移动端标记
                aentity.HIDCODE = entity.HIDCODE;
                aentity.ACCEPTPERSON = res.Contains("checkpersonid") ? dy.data.checkpersonid : ""; //
                aentity.ACCEPTPERSONNAME = res.Contains("checkperson") ? dy.data.checkperson : "";
                aentity.ACCEPTDEPARTCODE = res.Contains("acceptdepartcode") ? dy.data.acceptdepartcode : "";
                aentity.ACCEPTDEPARTNAME = res.Contains("acceptdepartname") ? dy.data.acceptdepartname : "";
                aentity.ISUPACCEPT = res.Contains("isupaccept") ? dy.data.isupaccept : ""; //是否省级单位验收
                string checktime = res.Contains("checktime") ? dy.data.checktime : null;
                if (!string.IsNullOrEmpty(checktime))
                {
                    aentity.ACCEPTDATE = Convert.ToDateTime(checktime);
                }
                if (string.IsNullOrEmpty(hiddenid))
                {
                    aentity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                aentity.ACCEPTPHOTO = !string.IsNullOrEmpty(aentity.ACCEPTPHOTO) ? aentity.ACCEPTPHOTO : Guid.NewGuid().ToString();
                UploadifyFile(aentity.ACCEPTPHOTO, "checkimg", files);
                /********************************/
                //保存
                htacceptinfobll.SaveForm(acceptID, aentity);
                #endregion

                //一次性提交隐患流程
                #region  一次性提交
                if (reformtype == "1")
                {
                    //隐患整改
                    var changeEntity = htchangeinfobll.GetEntityByCode(hidcode);
                    if (null != changeEntity)
                    {
                        changeEntity.CHANGERESULT = "1";
                        changeEntity.CHANGEDEADINE = changeEntity.CHANGEFINISHDATE;
                        //新增
                        htchangeinfobll.SaveForm(changeEntity.ID, changeEntity);
                    }
                    //隐患验收
                    var acceptEntity = htacceptinfobll.GetEntityByHidCode(hidcode);
                    if (null != acceptEntity)
                    {
                        acceptEntity.ACCEPTSTATUS = "1";
                        //新增
                        htacceptinfobll.SaveForm(acceptEntity.ID, acceptEntity);
                    }
                    //隐患评估信息
                    #region 一次性提交
                    HTApprovalEntity approavlentity = new HTApprovalEntity();
                    approavlentity.APPSIGN = AppSign; //移动端标记
                    approavlentity.HIDCODE = entity.HIDCODE;
                    approavlentity.APPROVALPERSON = curUser.UserId;
                    approavlentity.APPROVALPERSONNAME = curUser.UserName;
                    approavlentity.APPROVALDEPARTCODE = curUser.DeptCode;
                    approavlentity.APPROVALDEPARTNAME = curUser.DeptName;
                    approavlentity.APPROVALRESULT = "1";
                    approavlentity.APPROVALDATE = DateTime.Now;
                    htapprovalbll.SaveForm(acceptID, approavlentity);

                    wfFlag = "2";//整改结束

                    participant = curUser.Account;

                    int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //更新业务流程状态
                    }
                    #endregion
                }
                #endregion

                //限期整改隐患
                #region 限期整改隐患
                else   //限期整改隐患
                {
                    keyValue = entity.ID;  //隐患主键

                    //隐患类型
                    var detailItem = dataitemdetailbll.GetDataItemListByItemCode("'HidRank'").ToList().Where(p => p.ItemDetailId == entity.HIDRANK).FirstOrDefault();

                    WfControlObj wfentity = new WfControlObj();
                    var tbentity = htbaseinfobll.GetEntity(keyValue);
                    wfentity.businessid = keyValue; //主键
                    wfentity.startflow = tbentity.WORKSTREAM;//流程状态
                    wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
                    wfentity.argument2 = curUser.DeptId; //当前部门
                    wfentity.argument3 = entity.HIDTYPE;//隐患类别
                    wfentity.argument4 = entity.HIDBMID; //所属部门
                   
                    //是否上报
                    if (issubmit == "1")
                    {
                        wfentity.submittype = "上报";
                    }
                    else
                    {
                        wfentity.submittype = "提交";

                        //不指定整改责任人
                        if (centity.ISAPPOINT == "0")
                        {
                            wfentity.submittype = "制定提交";
                        }

                    }
                    //国电新疆版本
                    if (entity.ADDTYPE == "3")
                    {    //非本部门提交
                        if (entity.ISSELFCHANGE == "0")
                        {
                            wfentity.submittype = "制定提交";
                        }
                    }
                    wfentity.rankid = tbentity.HIDRANK;
                    wfentity.user = curUser;
                    wfentity.organizeid = tbentity.HIDDEPART; //对应电厂id
                    wfentity.istoend = "否"; //非立即整改
                    //省级登记的隐患消息推送
                    if (tbentity.ADDTYPE == "2") //省级登记的
                    {
                        //推送到隐患完善
                        wfentity.mark = "省级隐患排查";

                    }
                    else  //电厂推送的隐患 
                    {
                        wfentity.mark = "厂级隐患排查";
                    }
                    //获取下一流程的操作人
                    WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                    if (result.code == WfCode.Sucess)
                    {
                        participant = result.actionperson;
                        wfFlag = result.wfflag;
                        //推进流程
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                            }
                        }
                        else
                        {
                            return new { code = -1, count = 0, info = "请联系系统管理员，添加本单位及相关单位评估人员!" };
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = result.message };
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 隐患制定整改计划
        /// <summary>
        ///  制定整改计划
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object DrawUpChangePlanPush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {

                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                string wfFlag = "";

                string participant = "";

                string keyValue = "";

                string reformtype = dy.data.reformtype; //新增类型

                string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid : "";  //主键

                //隐患信息
                #region 隐患登记信息
                HTBaseInfoEntity entity = new HTBaseInfoEntity();
                string hidcode = res.Contains("problemid") ? dy.data.problemid : ""; //隐患编号
                entity.HIDCODE = hidcode;
                //如果hiddenid 不为空,则是编辑状态
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    entity = htbaseinfobll.GetEntity(hiddenid);
                }
                else
                {
                    //防止编码重复，如果多次提交同一个编码的，则进行覆盖处理(因此时hiddenid传过来为null)
                    IList<HTBaseInfoEntity> tlist = htbaseinfobll.GetListByCode(entity.HIDCODE);
                    if (tlist.Count() > 0)
                    {
                        hiddenid = tlist.FirstOrDefault().ID;
                    }
                }
                entity.APPSIGN = AppSign; //移动端标记
                entity.ISFORMULATE = "1"; //是否已经制定了整改计划
                entity.ISSELFCHANGE = res.Contains("isselfchange") ? dy.data.isselfchange : ""; //是否本部门整改
                entity.HIDRANK = res.Contains("hidrank") ? dy.data.hidrank : ""; //隐患级别  recdata.data.rankname
                entity.HIDPOINT = res.Contains("hidpointid") ? dy.data.hidpointid : "";// 所属区域code
                entity.HIDPOINTNAME = res.Contains("hidpoint") ? dy.data.hidpoint : ""; //所属区域名称
                entity.HIDTYPE = res.Contains("categoryid") ? dy.data.categoryid : "";  //隐患类别 recdata.data.category 
                entity.HIDDESCRIBE = res.Contains("hiddescribe") ? dy.data.hiddescribe : ""; //隐患描述
                entity.HIDBMID = res.Contains("hidbmid") ? dy.data.hidbmid : ""; //所属部门id
                entity.HIDBMNAME = res.Contains("hidbmname") ? dy.data.hidbmname : ""; //所属部门name
                entity.ISBREAKRULE = "0";  //违章行为（0:不是,1：是）
                //所属工程
                entity.HIDPROJECT = res.Contains("engineerid") ? dy.data.engineerid : "";  //所属工程
                entity.HIDPROJECTNAME = res.Contains("engineername") ? dy.data.engineername : "";  //所属工程名称

                //新增的部分字段
                entity.DEVICENAME = res.Contains("devicename") ? dy.data.devicename : "";  //设备名称
                entity.DEVICECODE = res.Contains("devicecode") ? dy.data.devicecode : "";  //设备编号
                entity.DEVICEID = res.Contains("deviceid") ? dy.data.deviceid : "";  //设备id
                entity.MONITORPERSONNAME = res.Contains("monitorpersonname") ? dy.data.monitorpersonname : "";  //厂级监控人员名称
                entity.MONITORPERSONID = res.Contains("monitorpersonid") ? dy.data.monitorpersonid : "";  //厂级监控人员Id
                entity.RELEVANCEID = res.Contains("relevanceid") ? dy.data.relevanceid : "";  //关联应用Id
                entity.RELEVANCETYPE = res.Contains("relevancetype") ? dy.data.relevancetype : "";  //关联其他应用标记
                entity.HIDNAME = res.Contains("hidname") ? dy.data.hidname : "";  //隐患名称
                entity.HIDSTATUS = res.Contains("hidstatus") ? dy.data.hidstatus : "";  //隐患现状
                entity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : "";  //专业分类
                entity.HIDCONSEQUENCE = res.Contains("hidconsequence") ? dy.data.hidconsequence : "";  //可能导致的后果

                entity.CHECKMAN = res.Contains("checkman") ? dy.data.checkman : ""; //排查人
                entity.CHECKMANNAME = res.Contains("checkmanname") ? dy.data.checkmanname : ""; //排查人员名称
                entity.CHECKDEPARTID = res.Contains("checkdept") ? dy.data.checkdept : "";//排查单位
                entity.CHECKDEPARTCODE = res.Contains("checkdeptcode") ? dy.data.checkdeptcode : ""; //排查单位code
                entity.CHECKDEPARTNAME = res.Contains("checkdeptname") ? dy.data.checkdeptname : ""; //排查单位名称
                entity.CHECKTYPE = res.Contains("checktypeid") ? dy.data.checktypeid : ""; //检查类型     recdata.data.checkType
                entity.SAFETYCHECKOBJECTID = res.Contains("safetydetailid") ? dy.data.safetydetailid : ""; //检查项目ID
                entity.CHECKDATE = res.Contains("checkdate") ? Convert.ToDateTime(dy.data.checkdate) : null;  //排查日期
                entity.HIDDEPART = res.Contains("deptid") ? dy.data.deptid : ""; //所属单位id
                entity.HIDDEPARTNAME = res.Contains("deptname") ? dy.data.deptname : ""; //所属单位名称
                //如果省公司登记则
                entity.ADDTYPE = reformtype;  //添加类型

                /*****重大隐患情况下****/

                entity.HIDPLACE = res.Contains("hidplace") ? dy.data.hidplace : ""; //隐患地点
                entity.REPORTDIGEST = res.Contains("reportdigest") ? dy.data.reportdigest : ""; //隐患报告摘要
                entity.HIDREASON = res.Contains("hidreason") ? dy.data.hidreason : ""; //隐患产生原因
                entity.HIDDANGERLEVEL = res.Contains("hiddangerlevel") ? dy.data.hiddangerlevel : ""; //隐患危害程度
                entity.PREVENTMEASURE = res.Contains("preventmeasure") ? dy.data.preventmeasure : ""; //主要预控及治理措施
                entity.HIDCHAGEPLAN = res.Contains("hidchageplan") ? dy.data.hidchageplan : ""; //隐患整改计划
                entity.EXIGENCERESUME = res.Contains("exigenceresume") ? dy.data.exigenceresume : ""; //应急措施简述
                entity.ISGETAFTER = res.Contains("isgetafter") ? dy.data.isgetafter : ""; //是否挂牌督办
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件

                string issubmit = res.Contains("isupsubmit") ? dy.data.isupsubmit : ""; //是否上报

                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(hiddenid))
                {
                    entity.HIDPHOTO = Guid.NewGuid().ToString();
                }

                //先删除图片
                DeleteFile(fileids);

                //上传隐患图片
                entity.HIDPHOTO = !string.IsNullOrEmpty(entity.HIDPHOTO) ? entity.HIDPHOTO : Guid.NewGuid().ToString();
                UploadifyFile(entity.HIDPHOTO, "problemimg", files);
                /********************************/
                //新增
                htbaseinfobll.SaveForm(hiddenid, entity);

                #endregion

                //整改信息
                #region 隐患整改信息
                HTChangeInfoEntity centity = null;
                string changeID = string.Empty;
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    centity = htchangeinfobll.GetEntityByCode(hidcode);
                }
                if (null == centity)
                {
                    centity = new HTChangeInfoEntity();
                    changeID = "";
                }
                else
                {
                    changeID = centity.ID;
                }
                centity.APPSIGN = AppSign; //移动端标记
                centity.HIDCODE = entity.HIDCODE; //隐患编码
                centity.CHANGEPERSON = res.Contains("dutypersonid") ? dy.data.dutypersonid : "";//整改人
                centity.CHANGEPERSONNAME = res.Contains("dutyperson") ? dy.data.dutyperson : "";//整改人
                centity.CHANGEDUTYDEPARTCODE = res.Contains("dutydeptid") ? dy.data.dutydeptid : "";//整改部门code
                if (!string.IsNullOrEmpty(centity.CHANGEDUTYDEPARTCODE))
                {
                    var changeDept = departmentBLL.GetEntityByCode(centity.CHANGEDUTYDEPARTCODE);
                    centity.CHANGEDUTYDEPARTID = changeDept.DepartmentId;
                }
                centity.CHANGEDUTYDEPARTNAME = res.Contains("dutydept") ? dy.data.dutydept : ""; //整改部门
                centity.CHANGEDUTYTEL = res.Contains("dutytel") ? dy.data.dutytel : ""; //整改人电话
                string deadinetime = res.Contains("deadinetime") ? dy.data.deadinetime : null; //整改截止时间
                if (!string.IsNullOrEmpty(deadinetime))
                {
                    centity.CHANGEDEADINE = Convert.ToDateTime(deadinetime); //整改截至时间
                }
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null; //整改截至时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.CHANGEFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                centity.CHANGERESUME = res.Contains("reformdescribe") ? dy.data.reformdescribe : ""; // 整改情况描述
                //计划治理资金
                string planmanagecapital = res.Contains("planmanagecapital") ? (null != dy.data.planmanagecapital ? dy.data.planmanagecapital.ToString() : "0") : "0";
                centity.PLANMANAGECAPITAL = !string.IsNullOrEmpty(planmanagecapital) ? Convert.ToDecimal(planmanagecapital) : 0;//计划治理资金
                //实际治理资金
                string realitymanagecapital = res.Contains("realitymanagecapital") ? (null != dy.data.realitymanagecapital ? dy.data.realitymanagecapital.ToString() : "0") : "0";
                centity.REALITYMANAGECAPITAL = !string.IsNullOrEmpty(realitymanagecapital) ? Convert.ToDecimal(realitymanagecapital) : 0; //实际治理资金realitymanagecapital

                centity.CHANGEMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";  //整改措施
                centity.BACKREASON = ""; //退回原因
                if (string.IsNullOrEmpty(hiddenid))
                {
                    centity.HIDCHANGEPHOTO = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                //上传隐患图片
                centity.HIDCHANGEPHOTO = !string.IsNullOrEmpty(centity.HIDCHANGEPHOTO) ? centity.HIDCHANGEPHOTO : Guid.NewGuid().ToString();
                UploadifyFile(centity.HIDCHANGEPHOTO, "reformimg", files);
                /********************************/
                //新增
                htchangeinfobll.SaveForm(changeID, centity);

                #endregion

                //验收信息
                #region 隐患验收信息
                HTAcceptInfoEntity aentity = null;
                string acceptID = string.Empty;
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    aentity = htacceptinfobll.GetEntityByHidCode(hidcode);
                }
                if (null == aentity)
                {
                    aentity = new HTAcceptInfoEntity();
                    acceptID = "";
                }
                else
                {
                    acceptID = aentity.ID;
                }
                aentity.APPSIGN = AppSign; //移动端标记
                aentity.HIDCODE = entity.HIDCODE;
                aentity.ACCEPTPERSON = res.Contains("checkpersonid") ? dy.data.checkpersonid : ""; //
                aentity.ACCEPTPERSONNAME = res.Contains("checkperson") ? dy.data.checkperson : "";
                aentity.ACCEPTDEPARTCODE = res.Contains("acceptdepartcode") ? dy.data.acceptdepartcode : "";
                aentity.ACCEPTDEPARTNAME = res.Contains("acceptdepartname") ? dy.data.acceptdepartname : "";
                aentity.ISUPACCEPT = res.Contains("isupaccept") ? dy.data.isupaccept : ""; //是否省级单位验收
                string checktime = res.Contains("checktime") ? dy.data.checktime : null;
                if (!string.IsNullOrEmpty(checktime))
                {
                    aentity.ACCEPTDATE = Convert.ToDateTime(checktime);
                }
                if (string.IsNullOrEmpty(hiddenid))
                {
                    aentity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                UploadifyFile(aentity.ACCEPTPHOTO, "checkimg", files);
                /********************************/
                //保存
                htacceptinfobll.SaveForm(acceptID, aentity);
                #endregion

                //制定整改计划
                #region 制定整改计划
                keyValue = entity.ID;  //隐患主键

                //bool isspecial = false;

                #region 检查当前是否为生技部(具有直接提交到隐患整改的权限，也可设置其他角色部门),

                //WfControlObj wfValentity = new WfControlObj();
                //wfValentity.businessid = ""; //
                //wfValentity.startflow = "制定整改计划";
                //wfValentity.endflow = "隐患整改";
                //wfValentity.submittype = "提交";
                //wfValentity.rankid = entity.HIDRANK;
                //wfValentity.user = curUser;
                //wfValentity.mark = "厂级隐患排查";
                //wfValentity.isvaliauth = true;

                //string resultVal = string.Empty;
                ////获取下一流程的操作人
                //WfControlResult valresult = wfcontrolbll.GetWfControl(wfValentity);
                //isspecial = valresult.ishave; //验证结果
                #endregion


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.startflow = "制定整改计划";
                //具有生技部的权限，且整改部门就是生技部，则直接提交到整改
                //if (isspecial && centity.CHANGEDUTYDEPARTID == curUser.DeptId)
                //{
                wfentity.submittype = "提交";
                //}
                //else
                //{
                //    wfentity.submittype = "制定提交";
                //}
                wfentity.rankid = entity.HIDRANK;
                wfentity.user = curUser;
                wfentity.mark = "厂级隐患排查";
                wfentity.organizeid = entity.HIDDEPART; //对应电厂id
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    //推进流程
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "请联系系统管理员，添加本单位及相关单位评估人员!" };
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 隐患转交制定整改计划/隐患整改流程
        /// <summary>
        /// 隐患转交制定整改计划/隐患整改流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public object HiddenDeliverPlan()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string participant = string.Empty;

                string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid : "";  //主键 

                string reqtype = res.Contains("reqtype") ? dy.data.reqtype : ""; //1 验收转交

                HTBaseInfoEntity entity = htbaseinfobll.GetEntity(hiddenid);

                if (!string.IsNullOrEmpty(reqtype))
                {
                    #region 验收转交,更新验收信息
                    if (reqtype == "1")
                    {
                        var acceptEntity = htacceptinfobll.GetEntityByHidCode(entity.HIDCODE);
                        acceptEntity.ACCEPTPERSON = res.Contains("checkpersonid") ? dy.data.checkpersonid : ""; //
                        acceptEntity.ACCEPTPERSONNAME = res.Contains("checkperson") ? dy.data.checkperson : "";
                        acceptEntity.ACCEPTDEPARTCODE = res.Contains("acceptdepartcode") ? dy.data.acceptdepartcode : "";
                        acceptEntity.ACCEPTDEPARTNAME = res.Contains("acceptdepartname") ? dy.data.acceptdepartname : "";
                        htacceptinfobll.SaveForm(acceptEntity.ID, acceptEntity);
                    } 
                    #endregion
                }
                else
                {
                    //更新整改信息
                    if (!string.IsNullOrEmpty(entity.HIDCODE))
                    {
                        var changeEntity = htchangeinfobll.GetEntityByCode(entity.HIDCODE);
                        changeEntity.CHARGEDEPTID = res.Contains("chargedeptid") ? dy.data.chargedeptid : ""; //指定整改责任部门
                        changeEntity.CHARGEDEPTNAME = res.Contains("chargedeptname") ? dy.data.chargedeptname : ""; //指定整改责任部门
                        changeEntity.CHARGEPERSON = res.Contains("chargeperson") ? dy.data.chargeperson : ""; //整改责任负责人
                        changeEntity.CHARGEPERSONNAME = res.Contains("chargepersonname") ? dy.data.chargepersonname : ""; //整改责任负责人
                        changeEntity.CHANGEPERSONNAME = res.Contains("dutyperson") ? dy.data.dutyperson : "";//整改人
                        changeEntity.CHANGEPERSON = res.Contains("dutypersonid") ? dy.data.dutypersonid : "";//整改人
                        changeEntity.CHANGEDUTYDEPARTNAME = res.Contains("dutydept") ? dy.data.dutydept : ""; //整改部门
                        changeEntity.CHANGEDUTYDEPARTCODE = res.Contains("dutydeptid") ? dy.data.dutydeptid : "";//整改部门code
                        if (!string.IsNullOrEmpty(changeEntity.CHANGEDUTYDEPARTCODE))
                        {
                            var changedept = departmentBLL.GetEntityByCode(changeEntity.CHANGEDUTYDEPARTCODE);
                            changeEntity.CHANGEDUTYDEPARTID = changedept.DepartmentId;  //整改部门id
                        }
                        changeEntity.CHANGEDUTYTEL = res.Contains("dutytel") ? dy.data.dutytel : "";
                        htchangeinfobll.SaveForm(changeEntity.ID, changeEntity);
                    }
                }

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;

                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = hiddenid; //业务id
                wfentity.startflow = entity.WORKSTREAM;
                wfentity.endflow = entity.WORKSTREAM;
                wfentity.submittype = "转交";
                wfentity.rankid = entity.HIDRANK;
                wfentity.user = curUser;
                wfentity.mark = "厂级隐患排查";
                wfentity.organizeid = entity.HIDDEPART; //对应电厂id
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //不更改状态
                        if (!result.ischangestatus)
                        {
                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, hiddenid, participant, curUser.UserId);
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "请联系系统管理员，添加当前流程下的参与者!" };
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 一次性提交多个关联检查人的登记隐患信息
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public object SaveHiddenForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                string reformtype = dy.data.reformtype; //新增类型

                string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid : "";  //主键

                //隐患信息
                #region 隐患登记信息
                HTBaseInfoEntity entity = new HTBaseInfoEntity();
                string hidcode = res.Contains("problemid") ? dy.data.problemid : ""; //隐患编号
                entity.HIDCODE = hidcode;
                //如果hiddenid 不为空,则是编辑状态
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    entity = htbaseinfobll.GetEntity(hiddenid);
                }
                else
                {
                    //防止编码重复，如果多次提交同一个编码的，则进行覆盖处理(因此时hiddenid传过来为null)
                    IList<HTBaseInfoEntity> tlist = htbaseinfobll.GetListByCode(entity.HIDCODE);
                    if (tlist.Count() > 0)
                    {
                        hiddenid = tlist.FirstOrDefault().ID;
                    }
                }

                entity.HIDRANK = res.Contains("hidrank") ? dy.data.hidrank : ""; //隐患级别  recdata.data.rankname
                entity.HIDPOINT = res.Contains("hidpointid") ? dy.data.hidpointid : "";// 所属区域code
                entity.HIDPOINTNAME = res.Contains("hidpoint") ? dy.data.hidpoint : ""; //所属区域名称
                entity.HIDTYPE = res.Contains("categoryid") ? dy.data.categoryid : "";  //隐患类别 recdata.data.category 
                entity.HIDDESCRIBE = res.Contains("hiddescribe") ? dy.data.hiddescribe : ""; //隐患描述
                entity.HIDBMID = res.Contains("hidbmid") ? dy.data.hidbmid : ""; //所属部门id
                entity.HIDBMNAME = res.Contains("hidbmname") ? dy.data.hidbmname : ""; //所属部门name
                entity.ISBREAKRULE = "0";  //违章行为（0:不是,1：是）

                entity.HIDPROJECT = res.Contains("engineerid") ? dy.data.engineerid : "";  //所属工程
                entity.HIDPROJECTNAME = res.Contains("engineername") ? dy.data.engineername : "";  //所属工程名称

                entity.CHECKMAN = res.Contains("checkman") ? dy.data.checkman : ""; //排查人
                entity.CHECKMANNAME = res.Contains("checkmanname") ? dy.data.checkmanname : ""; //排查人员名称
                entity.CHECKDEPARTID = res.Contains("checkdept") ? dy.data.checkdept : "";
                entity.CHECKDEPARTCODE = res.Contains("checkdept") ? dy.data.checkdept : ""; //排查单位
                entity.CHECKDEPARTNAME = res.Contains("checkdeptname") ? dy.data.checkdeptname : ""; //排查单位名称
                entity.CHECKTYPE = res.Contains("checktypeid") ? dy.data.checktypeid : ""; //检查类型     recdata.data.checkType
                entity.SAFETYCHECKOBJECTID = res.Contains("safetydetailid") ? dy.data.safetydetailid : ""; //检查项目ID
                entity.SAFETYCHECKNAME = res.Contains("safetycheckname") ? dy.data.safetycheckname : ""; //检查名称
                entity.CHECKDATE = res.Contains("checkdate") ? Convert.ToDateTime(dy.data.checkdate) : null;  //排查日期
                entity.HIDDEPART = res.Contains("deptid") ? dy.data.deptid : ""; //所属单位id
                entity.HIDDEPARTNAME = res.Contains("deptname") ? dy.data.deptname : ""; //所属单位名称
                string issubmit = res.Contains("isupsubmit") ? dy.data.isupsubmit : ""; //是否上报
                entity.UPSUBMIT = issubmit; //是否上报
                //如果省公司登记则
                entity.ADDTYPE = reformtype;  //添加类型

                //新增的部分字段
                entity.DEVICENAME = res.Contains("devicename") ? dy.data.devicename : "";  //设备名称
                entity.DEVICECODE = res.Contains("devicecode") ? dy.data.devicecode : "";  //设备编号
                entity.DEVICEID = res.Contains("deviceid") ? dy.data.deviceid : "";  //设备id
                entity.MONITORPERSONNAME = res.Contains("monitorpersonname") ? dy.data.monitorpersonname : "";  //厂级监控人员名称
                entity.MONITORPERSONID = res.Contains("monitorpersonid") ? dy.data.monitorpersonid : "";  //厂级监控人员Id
                entity.RELEVANCEID = res.Contains("relevanceid") ? dy.data.relevanceid : "";  //关联应用Id
                entity.RELEVANCETYPE = res.Contains("relevancetype") ? dy.data.relevancetype : "";  //关联其他应用标记
                entity.HIDNAME = res.Contains("hidname") ? dy.data.hidname : "";  //隐患名称
                entity.HIDSTATUS = res.Contains("hidstatus") ? dy.data.hidstatus : "";  //隐患现状
                entity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : "";  //专业分类
                entity.HIDCONSEQUENCE = res.Contains("hidconsequence") ? dy.data.hidconsequence : "";  //可能导致的后果
                string chosedcheckname = res.Contains("chosedcheckname") ? dy.data.chosedcheckname : ""; //检查名称选择标识 
                //安全检查
                if (!string.IsNullOrEmpty(entity.SAFETYCHECKOBJECTID) && !string.IsNullOrEmpty(chosedcheckname))
                {
                    entity.RELEVANCEID = new SaftyCheckDataRecordBLL().GetRecordFromHT(entity.SAFETYCHECKOBJECTID, curUser);
                }
                entity.APPSIGN = AppSign; //移动端标记

                /*****重大隐患情况下****/

                entity.HIDPLACE = res.Contains("hidplace") ? dy.data.hidplace : ""; //隐患地点
                entity.REPORTDIGEST = res.Contains("reportdigest") ? dy.data.reportdigest : ""; //隐患报告摘要
                entity.HIDREASON = res.Contains("hidreason") ? dy.data.hidreason : ""; //隐患产生原因
                entity.HIDDANGERLEVEL = res.Contains("hiddangerlevel") ? dy.data.hiddangerlevel : ""; //隐患危害程度
                entity.PREVENTMEASURE = res.Contains("preventmeasure") ? dy.data.preventmeasure : ""; //防控措施
                entity.HIDCHAGEPLAN = res.Contains("hidchageplan") ? dy.data.hidchageplan : ""; //隐患整改计划
                entity.EXIGENCERESUME = res.Contains("exigenceresume") ? dy.data.exigenceresume : ""; //应急预案简述
                entity.ISGETAFTER = res.Contains("isgetafter") ? dy.data.isgetafter : ""; //是否挂牌督办
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件

                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(hiddenid))
                {
                    entity.HIDPHOTO = Guid.NewGuid().ToString();
                }

                //先删除图片
                DeleteFile(fileids);

                //上传隐患图片
                UploadifyFile(entity.HIDPHOTO, "problemimg", files);
                /********************************/
                //新增
                htbaseinfobll.SaveForm(hiddenid, entity);

                #endregion

                //主键为空
                #region 创建隐患流程
                if (string.IsNullOrEmpty(hiddenid))
                {
                    string workFlow = "01";//隐患处理
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, curUser.UserId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(entity.ID);  //更新业务流程状态
                    }
                }
                #endregion

                //整改信息
                #region 隐患整改信息
                HTChangeInfoEntity centity = null;
                string changeID = string.Empty;
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    centity = htchangeinfobll.GetEntityByCode(hidcode);
                }
                if (null == centity)
                {
                    centity = new HTChangeInfoEntity();
                    changeID = "";
                }
                else
                {
                    changeID = centity.ID;
                }
                centity.APPSIGN = AppSign; //移动端标记
                centity.HIDCODE = entity.HIDCODE; //隐患编码
                centity.CHANGEPERSON = res.Contains("dutypersonid") ? dy.data.dutypersonid : "";//整改人
                centity.CHANGEPERSONNAME = res.Contains("dutyperson") ? dy.data.dutyperson : "";//整改人
                centity.CHANGEDUTYDEPARTCODE = res.Contains("dutydeptid") ? dy.data.dutydeptid : "";//整改部门code
                if (!string.IsNullOrEmpty(centity.CHANGEDUTYDEPARTCODE))
                {
                    var changeDept = departmentBLL.GetEntityByCode(centity.CHANGEDUTYDEPARTCODE);
                    centity.CHANGEDUTYDEPARTID = changeDept.DepartmentId;
                }
                centity.CHANGEDUTYDEPARTNAME = res.Contains("dutydept") ? dy.data.dutydept : ""; //整改部门
                centity.CHANGEDUTYTEL = res.Contains("dutytel") ? dy.data.dutytel : ""; //整改人电话

                string deadinetime = res.Contains("deadinetime") ? dy.data.deadinetime : null;
                if (!string.IsNullOrEmpty(deadinetime))
                {
                    centity.CHANGEDEADINE = Convert.ToDateTime(deadinetime); //整改截至时间
                }
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null; //整改截至时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.CHANGEFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                centity.CHANGERESUME = res.Contains("reformdescribe") ? dy.data.reformdescribe : ""; // 整改情况描述
                //计划治理资金
                string planmanagecapital = res.Contains("planmanagecapital") ? (null != dy.data.planmanagecapital ? dy.data.planmanagecapital.ToString() : "0") : "0";
                centity.PLANMANAGECAPITAL = !string.IsNullOrEmpty(planmanagecapital) ? Convert.ToDecimal(planmanagecapital) : 0;//计划治理资金
                //实际治理资金
                string realitymanagecapital = res.Contains("realitymanagecapital") ? (null != dy.data.realitymanagecapital ? dy.data.realitymanagecapital.ToString() : "0") : "0";
                centity.REALITYMANAGECAPITAL = !string.IsNullOrEmpty(realitymanagecapital) ? Convert.ToDecimal(realitymanagecapital) : 0; //实际治理资金realitymanagecapital

                centity.CHANGEMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";  //整改措施
                centity.BACKREASON = null; //退回原因
                if (string.IsNullOrEmpty(hiddenid))
                {
                    centity.HIDCHANGEPHOTO = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                //上传隐患图片
                UploadifyFile(centity.HIDCHANGEPHOTO, "reformimg", files);
                /********************************/
                //新增
                htchangeinfobll.SaveForm(changeID, centity);

                #endregion

                //验收信息
                #region 隐患验收信息
                HTAcceptInfoEntity aentity = null;
                string acceptID = string.Empty;
                if (!string.IsNullOrEmpty(hiddenid))
                {
                    aentity = htacceptinfobll.GetEntityByHidCode(hidcode);
                }
                if (null == aentity)
                {
                    aentity = new HTAcceptInfoEntity();
                    acceptID = "";
                }
                else
                {
                    acceptID = aentity.ID;
                }
                aentity.APPSIGN = AppSign; //移动端标记
                aentity.HIDCODE = entity.HIDCODE;
                aentity.ACCEPTPERSON = res.Contains("checkpersonid") ? dy.data.checkpersonid : "";
                aentity.ACCEPTPERSONNAME = res.Contains("checkperson") ? dy.data.checkperson : "";
                aentity.ACCEPTDEPARTCODE = res.Contains("acceptdepartcode") ? dy.data.acceptdepartcode : "";
                aentity.ACCEPTDEPARTNAME = res.Contains("acceptdepartname") ? dy.data.acceptdepartname : "";
                aentity.ISUPACCEPT = res.Contains("isupaccept") ? dy.data.isupaccept : "";
                string checktime = res.Contains("checktime") ? dy.data.checktime : null;
                if (!string.IsNullOrEmpty(checktime))
                {
                    aentity.ACCEPTDATE = Convert.ToDateTime(checktime);
                }
                if (string.IsNullOrEmpty(hiddenid))
                {
                    aentity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                UploadifyFile(aentity.ACCEPTPHOTO, "checkimg", files);
                /********************************/
                //保存
                htacceptinfobll.SaveForm(acceptID, aentity);
                #endregion

                //复查验证
                #region 复查验证
                IList<UserEntity> ulist = new List<UserEntity>();
                //隐患类型
                var detailItem = dataitemdetailbll.GetDataItemListByItemCode("'HidRank'").ToList().Where(p => p.ItemDetailId == entity.HIDRANK).FirstOrDefault();

                //省级登记的隐患
                if (curUser.RoleName.Contains("省级用户"))
                {

                    //添加复查验证信息   非省级验收 并且为一般隐患 
                    if (aentity.ISUPACCEPT != "1" && detailItem.ItemName.Contains("一般"))
                    {
                        #region 复查验证
                        HtReCheckEntity rcEntity = null;
                        string recheckID = string.Empty;
                        if (!string.IsNullOrEmpty(hiddenid))
                        {
                            rcEntity = htrecheckbll.GetEntityByHidCode(hidcode);
                        }
                        if (null == rcEntity)
                        {
                            rcEntity = new HtReCheckEntity();
                            recheckID = "";
                        }
                        else
                        {
                            recheckID = rcEntity.ID;
                        }
                        rcEntity.APPSIGN = AppSign; //移动端标记
                        rcEntity.HIDCODE = entity.HIDCODE;
                        rcEntity.RECHECKPERSON = res.Contains("recheckspersonid") ? dy.data.recheckspersonid : ""; //复查人id
                        rcEntity.RECHECKPERSONNAME = res.Contains("rechecksperson") ? dy.data.rechecksperson : ""; //
                        rcEntity.RECHECKDEPARTCODE = res.Contains("rechecksdepartcode") ? dy.data.rechecksdepartcode : "";
                        rcEntity.RECHECKDEPARTNAME = res.Contains("rechecksdepartname") ? dy.data.rechecksdepartname : "";
                        string rechecktime = res.Contains("rechecksdate") ? dy.data.rechecksdate : null;
                        if (!string.IsNullOrEmpty(rechecktime))
                        {
                            rcEntity.RECHECKDATE = Convert.ToDateTime(rechecktime);
                        }
                        htrecheckbll.SaveForm(recheckID, rcEntity);
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 隐患评估
        /// <summary>
        /// 隐患评估
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ApprovalProblemPush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string keyValue = dy.data.hiddenid;//隐患编号 

            string hidCode = dy.data.problemid;  //隐患编码

            string isUpSubmit = dy.data.isupsubmit; //是否上报

            string dutypersonid = res.Contains("dutypersonid") ? dy.data.dutypersonid : "";  //整改人

            string submitUser = "";

            WfControlResult result = new WfControlResult();

            HttpFileCollection files = ctx.Request.Files;//上传的文件 

            HTBaseInfoEntity baseentity = new HTBaseInfoBLL().GetEntity(keyValue);

            HTChangeInfoEntity chEntity = htchangeinfobll.GetEntityByHidCode(hidCode); //获取当前隐患整改信息

            HTAcceptInfoEntity aEntity = htacceptinfobll.GetEntityByHidCode(hidCode); //获取当前隐患验收信息

            HTApprovalEntity entity = new HTApprovalEntity(); //获取当前隐患信息

            entity.HIDCODE = hidCode; //隐患编码

            entity.APPROVALPERSON = curUser.UserId; //评估人id

            entity.APPROVALPERSONNAME = curUser.UserName; //评估人姓名
            entity.APPROVALDEPARTCODE = curUser.DeptCode; //部门Code
            entity.APPROVALDEPARTNAME = curUser.DeptName;//部门名称
            if (curUser.RoleName.Contains("公司级") || curUser.RoleName.Contains("厂级"))
            {
                entity.APPROVALDEPARTNAME = curUser.OrganizeName;//部门名称
                entity.APPROVALDEPARTCODE = curUser.OrganizeCode; //部门Code
            }


            try
            {
                entity.APPROVALDATE = res.Contains("approvaldate") ? Convert.ToDateTime(dy.data.approvaldate) : DBNull.Value; //评估时间

                entity.APPROVALRESULT = res.Contains("approvalresult") ? dy.data.approvalresult : ""; //评估结果

                string isexpose = res.Contains("isexpose") ? dy.data.isexpose : ""; //是否曝光

                entity.APPROVALREASON = res.Contains("approvalreason") ? dy.data.approvalreason : "";  //评估原因

                string wfFlag = string.Empty;  //流程标识

                bool isgoback = false;

                string participant = string.Empty;  //获取流程下一节点的参与人员

                #region 保存基本信息
                //评估ID
                string APPROVALID = "";
                string CHANGEID = chEntity.ID;
                string ACCEPTID = aEntity.ID;
                APPROVALID = APPROVALID == "&nbsp;" ? "" : APPROVALID;

                //保存隐患曝光状态
                baseentity.EXPOSURESTATE = isexpose;
                baseentity.EXPOSUREDATETIME = DateTime.Now;
                baseentity.PREVENTMEASURE = res.Contains("preventmeasure") ? dy.data.preventmeasure : "";
                baseentity.HIDSTATUS = res.Contains("hidstatus") ? dy.data.hidstatus : "";
                baseentity.MONITORPERSONNAME = res.Contains("monitorpersonname") ? dy.data.monitorpersonname : "";
                baseentity.MONITORPERSONID = res.Contains("monitorpersonid") ? dy.data.monitorpersonid : "";
                baseentity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : "";  //专业分类 

                baseentity.HIDRANK = res.Contains("hidrank") ? dy.data.hidrank : ""; //隐患级别  recdata.data.rankname
                baseentity.HIDPOINT = res.Contains("hidpointid") ? dy.data.hidpointid : "";// 所属区域code
                baseentity.HIDPOINTNAME = res.Contains("hidpoint") ? dy.data.hidpoint : ""; //所属区域名称
                baseentity.HIDTYPE = res.Contains("categoryid") ? dy.data.categoryid : "";  //隐患类别 recdata.data.category 
                baseentity.HIDDESCRIBE = res.Contains("hiddescribe") ? dy.data.hiddescribe : ""; //隐患描述

                baseentity.HIDBMID = res.Contains("hidbmid") ? dy.data.hidbmid : ""; //所属部门id
                baseentity.HIDBMNAME = res.Contains("hidbmname") ? dy.data.hidbmname : ""; //所属部门name

                baseentity.HIDDEPART = res.Contains("deptid") ? dy.data.deptid : ""; //所属单位id
                baseentity.HIDDEPARTNAME = res.Contains("deptname") ? dy.data.deptname : ""; //所属单位名称

                baseentity.APPSIGN = AppSign; //移动端标记
                //所属工程
                baseentity.HIDPROJECT = res.Contains("engineerid") ? dy.data.engineerid : "";  //所属工程
                baseentity.HIDPROJECTNAME = res.Contains("engineername") ? dy.data.engineername : "";  //所属工程名称

                //新增的部分字段
                baseentity.DEVICENAME = res.Contains("devicename") ? dy.data.devicename : "";  //设备名称
                baseentity.DEVICECODE = res.Contains("devicecode") ? dy.data.devicecode : "";  //设备编号
                baseentity.DEVICEID = res.Contains("deviceid") ? dy.data.deviceid : "";  //设备id
                baseentity.MONITORPERSONNAME = res.Contains("monitorpersonname") ? dy.data.monitorpersonname : "";  //厂级监控人员名称
                baseentity.MONITORPERSONID = res.Contains("monitorpersonid") ? dy.data.monitorpersonid : "";  //厂级监控人员Id
                baseentity.RELEVANCEID = res.Contains("relevanceid") ? dy.data.relevanceid : "";  //关联应用Id
                baseentity.RELEVANCETYPE = res.Contains("relevancetype") ? dy.data.relevancetype : "";  //关联其他应用标记
                baseentity.HIDNAME = res.Contains("hidname") ? dy.data.hidname : "";  //隐患名称
                baseentity.HIDSTATUS = res.Contains("hidstatus") ? dy.data.hidstatus : "";  //隐患现状
                baseentity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : "";  //专业分类
                baseentity.HIDCONSEQUENCE = res.Contains("hidconsequence") ? dy.data.hidconsequence : "";  //可能导致的后果

                baseentity.CHECKMAN = res.Contains("checkman") ? dy.data.checkman : ""; //排查人
                baseentity.CHECKMANNAME = res.Contains("checkmanname") ? dy.data.checkmanname : ""; //排查人员名称
                baseentity.CHECKDEPARTID = res.Contains("checkdept") ? dy.data.checkdept : "";//排查单位
                baseentity.CHECKDEPARTCODE = res.Contains("checkdeptcode") ? dy.data.checkdeptcode : ""; //排查单位code
                baseentity.CHECKDEPARTNAME = res.Contains("checkdeptname") ? dy.data.checkdeptname : ""; //排查单位名称
                baseentity.CHECKTYPE = res.Contains("checktypeid") ? dy.data.checktypeid : ""; //检查类型     recdata.data.checkType
                baseentity.SAFETYCHECKOBJECTID = res.Contains("safetydetailid") ? dy.data.safetydetailid : ""; //检查项目ID
                baseentity.SAFETYCHECKNAME = res.Contains("safetycheckname") ? dy.data.safetycheckname : ""; //检查名称



                /*****重大隐患情况下****/

                baseentity.HIDPLACE = res.Contains("hidplace") ? dy.data.hidplace : ""; //隐患地点
                baseentity.REPORTDIGEST = res.Contains("reportdigest") ? dy.data.reportdigest : ""; //隐患报告摘要
                baseentity.HIDREASON = res.Contains("hidreason") ? dy.data.hidreason : ""; //隐患产生原因
                baseentity.HIDDANGERLEVEL = res.Contains("hiddangerlevel") ? dy.data.hiddangerlevel : ""; //隐患危害程度
                baseentity.PREVENTMEASURE = res.Contains("preventmeasure") ? dy.data.preventmeasure : ""; //主要预控及治理措施
                baseentity.HIDCHAGEPLAN = res.Contains("hidchageplan") ? dy.data.hidchageplan : ""; //隐患整改计划
                baseentity.EXIGENCERESUME = res.Contains("exigenceresume") ? dy.data.exigenceresume : ""; //应急措施简述
                baseentity.ISGETAFTER = res.Contains("isgetafter") ? dy.data.isgetafter : ""; //是否挂牌督办

                baseentity.ISSELFCHANGE = res.Contains("isselfchange") ? dy.data.isselfchange : ""; //是否本部门整改 

                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件

                //先删除图片
                DeleteFile(fileids);

                //上传隐患图片
                baseentity.HIDPHOTO = !string.IsNullOrEmpty(baseentity.HIDPHOTO) ? baseentity.HIDPHOTO : Guid.NewGuid().ToString();
                UploadifyFile(baseentity.HIDPHOTO, "problemimg", files);

                //保存隐患基本信息
                htbaseinfobll.SaveForm(keyValue, baseentity);

                //存在回退原因后，需要清空原因提交
                chEntity.BACKREASON = "";
                chEntity.CHANGEPERSONNAME = res.Contains("dutyperson") ? dy.data.dutyperson : "";//整改人
                chEntity.CHANGEPERSON = res.Contains("dutypersonid") ? dy.data.dutypersonid : "";//整改人
                chEntity.CHANGEDUTYDEPARTCODE = res.Contains("dutydeptid") ? dy.data.dutydeptid : "";//整改部门code
                if (!string.IsNullOrEmpty(chEntity.CHANGEDUTYDEPARTCODE))
                {
                    var changeDept = departmentBLL.GetEntityByCode(chEntity.CHANGEDUTYDEPARTCODE);
                    chEntity.CHANGEDUTYDEPARTID = changeDept.DepartmentId;
                }
                chEntity.CHANGEDUTYDEPARTNAME = res.Contains("dutydept") ? dy.data.dutydept : ""; //整改部门
                chEntity.CHANGEDUTYTEL = res.Contains("dutytel") ? dy.data.dutytel : ""; //整改人电话
                string deadinetime = res.Contains("deadinetime") ? dy.data.deadinetime : null; //整改截至时间
                if (!string.IsNullOrEmpty(deadinetime))
                {
                    chEntity.CHANGEDEADINE = Convert.ToDateTime(deadinetime);
                }
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null; //整改截至时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    chEntity.CHANGEFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                chEntity.CHANGERESUME = res.Contains("reformdescribe") ? dy.data.reformdescribe : ""; // 整改情况描述
                //计划治理资金
                string planmanagecapital = res.Contains("planmanagecapital") ? (null != dy.data.planmanagecapital ? dy.data.planmanagecapital.ToString() : "0") : "0";
                chEntity.PLANMANAGECAPITAL = !string.IsNullOrEmpty(planmanagecapital) ? Convert.ToDecimal(planmanagecapital) : 0;//计划治理资金
                //实际治理资金
                string realitymanagecapital = res.Contains("realitymanagecapital") ? (null != dy.data.realitymanagecapital ? dy.data.realitymanagecapital.ToString() : "0") : "0";
                chEntity.REALITYMANAGECAPITAL = !string.IsNullOrEmpty(realitymanagecapital) ? Convert.ToDecimal(realitymanagecapital) : 0; //实际治理资金realitymanagecapital

                chEntity.CHANGEMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";  //整改措施

                //是否指定整改责任人
                chEntity.ISAPPOINT = res.Contains("isappoint") ? dy.data.isappoint : null; //是否指定整改责任人
                chEntity.CHARGEPERSON = res.Contains("chargeperson") ? dy.data.chargeperson : ""; //整改责任负责人
                chEntity.CHARGEPERSONNAME = res.Contains("chargepersonname") ? dy.data.chargepersonname : ""; //整改责任负责人
                chEntity.CHARGEDEPTID = res.Contains("chargedeptid") ? dy.data.chargedeptid : ""; //指定整改责任部门
                chEntity.CHARGEDEPTNAME = res.Contains("chargedeptname") ? dy.data.chargedeptname : ""; //指定整改责任部门

                chEntity.APPSIGN = AppSign; //移动端标记
                //隐患整改
                htchangeinfobll.SaveForm(CHANGEID, chEntity);

                aEntity.ACCEPTPERSONNAME = res.Contains("checkperson") ? dy.data.checkperson : "";
                aEntity.ACCEPTPERSON = res.Contains("checkpersonid") ? dy.data.checkpersonid : "";
                aEntity.ACCEPTDEPARTCODE = res.Contains("acceptdepartcode") ? dy.data.acceptdepartcode : "";
                aEntity.ACCEPTDEPARTNAME = res.Contains("acceptdepartname") ? dy.data.acceptdepartname : "";
                string checktime = res.Contains("checktime") ? dy.data.checktime : null;
                if (!string.IsNullOrEmpty(checktime))
                {
                    aEntity.ACCEPTDATE = Convert.ToDateTime(checktime);
                }
                aEntity.APPSIGN = AppSign; //移动端标记
                //隐患验收
                htacceptinfobll.SaveForm(ACCEPTID, aEntity);
                #endregion
                //隐患评估附件id
                entity.APPROVALFILE = Guid.NewGuid().ToString();
                entity.APPSIGN = AppSign; //移动端标记

                //上传隐患评估图片
                entity.APPROVALFILE = !string.IsNullOrEmpty(entity.APPROVALFILE) ? entity.APPROVALFILE : Guid.NewGuid().ToString();
                UploadifyFile(entity.APPROVALFILE, "approvalimg", files);

                //请求对象
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = baseentity.WORKSTREAM;
                wfentity.rankid = baseentity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = baseentity.HIDDEPART; //对应电厂id
                wfentity.argument1 = baseentity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = curUser.DeptId; //当前部门
                wfentity.argument3 = baseentity.HIDTYPE;//隐患类别
                wfentity.argument4 = baseentity.HIDBMID; //所属部门

                //省公司登记的隐患
                if (baseentity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else
                {
                    wfentity.mark = "厂级隐患排查";
                }

                #region 省提交形式
                if (isUpSubmit == "1")  //上报，且存在上级部门
                {
                    wfentity.submittype = "上报";
                }
                else  //不上报，评估通过需要提交整改，评估不通过退回到登记
                {
                    /****判断当前人是否评估通过*****/
                    #region 判断当前人是否评估通过
                    //    //评估通过，则直接进行整改
                    if (entity.APPROVALRESULT == "1")
                    {
                        wfentity.submittype = "提交";

                        //不指定整改责任人
                        if (chEntity.ISAPPOINT == "0")
                        {
                            wfentity.submittype = "制定提交";
                        }
                        //判断是否是同级提交
                        bool ismajorpush = GetCurUserWfAuth(baseentity.HIDRANK, "隐患评估", "隐患评估", "厂级隐患排查", "同级提交", baseentity.MAJORCLASSIFY, null, null, null, string.Empty, keyValue);
                        if (ismajorpush)
                        {
                            wfentity.submittype = "同级提交";
                        }

                        //国电新疆版本
                        if (baseentity.ADDTYPE == "3")
                        {
                            //非本部门整改
                            if (baseentity.ISSELFCHANGE == "0")
                            {
                                wfentity.submittype = "制定提交";

                                //如果已经制定了整改计划,则按照提交来进行推送
                                if (baseentity.ISFORMULATE == "1")
                                {
                                    wfentity.submittype = "提交";
                                }
                                //如果当前评估部门是整改部门，则直接提交
                                if (curUser.DeptId == chEntity.CHANGEDUTYDEPARTID)
                                {
                                    wfentity.submittype = "提交";
                                }
                                //如果当前评估部门是创建部门，则直接提交至非本部门整改的安监部
                                if (curUser.DeptCode == baseentity.CREATEUSERDEPTCODE)
                                {
                                    wfentity.submittype = "制定提交";
                                }
                            }
                            else  //公司级用户不管如何，都不会直接到生技部  本部门提交
                            {
                                UserEntity userEntity = userbll.GetEntity(baseentity.CREATEUSERID);
                                if (userEntity.RoleName.Contains("公司级用户") && curUser.RoleName.Contains("公司级用户"))
                                {
                                    wfentity.submittype = "制定提交";
                                }
                            }
                        }

                    }
                    else  //评估不通过，退回到登记 
                    {
                        wfentity.submittype = "退回";

                        isgoback = true; //确定退回操作

                        //国电新疆版本
                        if (baseentity.ADDTYPE == "3")
                        {
                            //已经制定了整改计划，则按照制定计划退回
                            if (baseentity.ISFORMULATE == "1")
                            {
                                wfentity.submittype = "制定退回";
                            }
                        }
                    }
                    #endregion
                }
                #endregion


                //退回操作
                if (isgoback)
                {
                    //验收信息清空(所有评估退回)
                    if (baseentity.ADDTYPE == "0" || baseentity.ADDTYPE == "1")
                    {
                        aEntity.ACCEPTPERSONNAME = " ";
                        aEntity.ACCEPTPERSON = " ";
                        aEntity.ACCEPTDEPARTCODE = " ";
                        aEntity.ACCEPTDEPARTNAME = " ";
                        htacceptinfobll.SaveForm(ACCEPTID, aEntity);
                    }
                }
                //返回结果
                result = wfcontrolbll.GetWfControl(wfentity);

                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson; //目标流程参与者

                    wfFlag = result.wfflag;

                    if (!string.IsNullOrEmpty(participant))
                    {
                        //如果是更改状态
                        if (result.ischangestatus)
                        {

                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);
                            if (count > 0)
                            {
                                //保存隐患评估信息
                                htapprovalbll.SaveForm(APPROVALID, entity);
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                            }
                            else
                            {
                                return new { code = -1, count = 0, info = "当前用户无评估权限" };
                            }
                        }
                        else  //不更改状态的情况下
                        {
                            //保存隐患评估信息
                            htapprovalbll.SaveForm(APPROVALID, entity);
                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = result.message };
                    }
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "处理失败!" };
            }
            return new { code = 0, count = 0, info = result.message };
        }



        public bool GetCurUserWfAuth(string rankid, string workflow, string endflow, string mark, string submittype, string arg1 = "", string arg2 = "", string arg3 = "", string arg4 = "", string arg5 = "", string businessid = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = businessid;
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankid = rankid;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;
            wfentity.argument1 = arg1;
            wfentity.argument2 = arg2;
            wfentity.argument3 = arg3;
            wfentity.argument4 = arg4;
            wfentity.argument5 = arg5;
            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

            return result.ishave;
        }

        public object GetCurUserWfAuthByCondition(string queryJson)
        {
            List<object> list = new List<object>();
            if (!string.IsNullOrEmpty(queryJson))
            {

                Operator curUser = OperatorProvider.Provider.Current();
                JArray jarray = (JArray)JsonConvert.DeserializeObject(queryJson);
                foreach (JObject obj in jarray)
                {
                    string rankname = obj["rankname"].ToString();
                    string workflow = obj["workflow"].ToString();
                    string endflow = obj["endflow"].ToString();
                    string mark = obj["mark"].ToString();
                    string submittype = obj["submittype"].ToString();
                    string arg1 = queryJson.Contains("arg1") ? obj["arg1"].ToString() : string.Empty;
                    string arg2 = queryJson.Contains("arg2") ? obj["arg2"].ToString() : string.Empty;
                    string arg3 = queryJson.Contains("arg3") ? obj["arg3"].ToString() : string.Empty;
                    string arg4 = queryJson.Contains("arg4") ? obj["arg4"].ToString() : string.Empty;
                    string arg5 = queryJson.Contains("arg5") ? obj["arg4"].ToString() : string.Empty;
                    string businessid = obj["businessid"].ToString();
                    string action = obj["action"].ToString();

                    WfControlObj wfentity = new WfControlObj();
                    wfentity.businessid = businessid;
                    wfentity.startflow = workflow;
                    wfentity.endflow = endflow;
                    wfentity.submittype = submittype;
                    wfentity.rankname = rankname;
                    wfentity.user = curUser;
                    wfentity.mark = mark;
                    wfentity.isvaliauth = true;
                    wfentity.argument1 = arg1;
                    wfentity.argument2 = arg2;
                    wfentity.argument3 = curUser.DeptId;
                    wfentity.argument4 = arg4;
                    wfentity.argument5 = arg5;
                    //获取下一流程的操作人
                    WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                    list.Add(new { key = action, value = result.ishave });
                }
            }
            return list;
        }
        #endregion

        #region 隐患整改
        /// <summary>
        /// 隐患整改
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ReferFixResultPush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }


            WfControlResult result = new WfControlResult();
            try
            {
                string keyValue = dy.data.hiddenid; //隐患主键

                string hidcode = dy.data.problemid; // 隐患编码

                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : string.Empty; //整改完成时间 

                string checkperson = res.Contains("checkperson") ? dy.data.checkperson : string.Empty; //验收人

                string checkpersonid = res.Contains("checkpersonid") ? dy.data.checkpersonid : string.Empty;//验收人id

                string checktime = res.Contains("checktime") ? dy.data.checktime : string.Empty; //验收日期

                string realitymanagecapital = res.Contains("realitymanagecapital") ? dy.data.realitymanagecapital : "0";  //实际治理资金

                string reformdescribe = res.Contains("reformdescribe") ? dy.data.reformdescribe : string.Empty; //整改情况描述

                string changeresult = res.Contains("reformresult") ? dy.data.reformresult : string.Empty; //整改结果

                string isback = res.Contains("isback") ? dy.data.isback : string.Empty;//是否回退 

                string participant = string.Empty;  //获取流程下一节点的参与人员

                string wfFlag = string.Empty; //流程标识

                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                HTBaseInfoEntity baseentity = htbaseinfobll.GetEntity(keyValue);  //获取隐患信息

                HTAcceptInfoEntity acceptentity = htacceptinfobll.GetEntityByHidCode(hidcode); //获取对应的验收信息对象
                //更新隐患整改信息
                HTChangeInfoEntity entity = htchangeinfobll.GetEntityByHidCode(hidcode);//获取对应的整改信息对象
                entity.APPSIGN = AppSign; //移动端标记
                entity.CHANGEFINISHDATE = Convert.ToDateTime(reformfinishdate); //整改完成时间
                entity.CHANGERESUME = reformdescribe; //整改情况描述
                entity.REALITYMANAGECAPITAL = !string.IsNullOrEmpty(realitymanagecapital) ? Convert.ToDecimal(realitymanagecapital) : 0; //实际治理资金
                entity.CHANGERESULT = changeresult; //整改结果
                entity.BACKREASON = res.Contains("reformbackreason") ? dy.data.reformbackreason : string.Empty; //退回原因

                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                //先删除图片
                DeleteFile(fileids);

                entity.HIDCHANGEPHOTO = !string.IsNullOrEmpty(entity.HIDCHANGEPHOTO) ? entity.HIDCHANGEPHOTO : Guid.NewGuid().ToString();
                //上传整改图片
                UploadifyFile(entity.HIDCHANGEPHOTO, "reformimg", files);

                entity.ATTACHMENT = !string.IsNullOrEmpty(entity.ATTACHMENT) ? entity.ATTACHMENT : Guid.NewGuid().ToString();
                //上传整改附件
                UploadifyFile(entity.ATTACHMENT, "attachment", files);

                htchangeinfobll.SaveForm(entity.ID, entity);

                var createUser = userbll.GetEntity(baseentity.CREATEUSERID);


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = baseentity.WORKSTREAM;
                wfentity.rankid = baseentity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = baseentity.HIDDEPART; //对应电厂id
                if (baseentity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else
                {
                    wfentity.mark = "厂级隐患排查";
                }
                //退回 
                if (isback == "1")
                {
                    //整改历史记录
                    var changeitem = htchangeinfobll.GetHistoryList(hidcode).ToList();
                    //如果未整改可以退回
                    if (changeitem.Count() == 0)
                    {
                        wfentity.submittype = "退回";
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "整改过后的隐患无法再次退回!" };
                    }
                }
                else //正常提交到验收流程
                {
                    wfentity.submittype = "提交";
                }

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                #region 获取下一流程的操作人
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
                                entity.CHARGEPERSONNAME = userentity.RealName;
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
                #endregion
                #region 非自动处理的流程
                else if (result.code == WfCode.NoAutoHandle)
                {
                    bool isupdate = false;//是否更改流程状态
                    //退回操作  单独处理
                    if (isback == "1")
                    {
                        DataTable dt = htworkflowbll.GetBackFlowObjectByKey(keyValue);

                        if (dt.Rows.Count > 0)
                        {
                            wfFlag = dt.Rows[0]["wfflag"].ToString(); //流程走向

                            participant = dt.Rows[0]["participant"].ToString();  //指向人

                            isupdate = dt.Rows[0]["isupdate"].ToString() == "1"; //是否更改流程状态
                        }
                    }

                    #region 更改流程状态的情况下
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
                    #endregion
                }
                #endregion
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "处理失败" };
            }
            return new { code = 0, count = 0, info = result.message };
        }
        #endregion

        #region 隐患延期申请、审核、审批
        /// <summary>
        /// 隐患延期申请入口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object DelayApplyDanger([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {
                string actiondo = res.Contains("actiondo") ? dy.data.actiondo : string.Empty; //动作

                string hidcode = res.Contains("problemid") ? dy.data.problemid : string.Empty; //隐患编码

                string keyValue = res.Contains("hiddenid") ? dy.data.hiddenid : string.Empty;  //隐患主键  

                string postponereason = res.Contains("applyreason") ? dy.data.applyreason : string.Empty; //申请原因

                string controlmeasure = res.Contains("controlmeasure") ? dy.data.controlmeasure : string.Empty; //临时管控措施

                string postponedays = res.Contains("postponedays") ? dy.data.postponedays : string.Empty; //申请天数

                string postponeresult = res.Contains("postponeresult") ? dy.data.postponeresult : string.Empty; //审批结果

                string handleid = res.Contains("handleid") ? dy.data.handleid : string.Empty; //关联ID

                HTBaseInfoEntity bentity = htbaseinfobll.GetEntity(keyValue);  //获取隐患信息

                var centity = htchangeinfobll.GetEntityByCode(hidcode); //根据HidCode获取整改对象

                string nextName = string.Empty;

                bool isupdate = false;

                string wfFlag = string.Empty;

                string participant = string.Empty;

                //保存申请记录
                HTExtensionEntity exentity = new HTExtensionEntity();
                WfControlResult result = new WfControlResult();

                //条件
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.submittype = "提交";
                wfentity.rankid = bentity.HIDRANK;
                wfentity.user = curUser;
                wfentity.mark = "整改延期流程";
                wfentity.organizeid = bentity.HIDDEPART; //对应电厂id

                //通过延期申请来区分
                if (actiondo == "apply")
                {
                    wfentity.startflow = "整改延期申请";

                    centity.POSTPONEDAYS = int.Parse(postponedays.ToString());
                }
                else //审批过程
                {
                    wfentity.startflow = "整改延期审批";
                }
                //不通过
                if (postponeresult == "0")
                {
                    centity.APPLICATIONSTATUS = "-1"; //延期申请失败

                    //延期失败保存整改人相关信息到result,用于极光推送
                    UserEntity changeUser = userbll.GetEntity(centity.CHANGEPERSON);
                    if (null != changeUser)
                    {
                        result.actionperson = changeUser.Account;
                        result.username = centity.CHANGEPERSONNAME;
                        result.deptname = centity.CHANGEDUTYDEPARTNAME;
                        result.deptid = changeUser.DepartmentId;
                        result.deptcode = centity.CHANGEDUTYDEPARTCODE;
                    }
                }
                else   //通过，包括申请、审批
                {
                    //获取下一流程的操作人
                    result = wfcontrolbll.GetWfControl(wfentity);
                    //处理成功
                    if (result.code == WfCode.Sucess)
                    {
                        participant = result.actionperson;
                        wfFlag = result.wfflag;

                        centity.POSTPONEPERSON = "," + participant + ",";  // 用于当前人账户判断是否具有操作其权限
                        centity.POSTPONEDAYS = int.Parse(postponedays); //申请天数
                        centity.POSTPONEDEPT = result.deptcode;  //审批部门Code
                        centity.POSTPONEDEPTNAME = result.deptname;  //审批部门名称
                        centity.POSTPONEPERSONNAME = result.username;
                        centity.APPLICATIONSTATUS = wfFlag;
                        //是否更新时间，累加天数
                        if (wfFlag == "2") { isupdate = true; }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = result.message };
                    }
                    //如果安环部、生技部审批通过，则更改整改截至时间、验收时间，增加相应的整改天数
                    if (isupdate)
                    {
                        //重新赋值整改截至时间
                        centity.CHANGEDEADINE = centity.CHANGEDEADINE.Value.AddDays(centity.POSTPONEDAYS);
                        //更新验收时间
                        HTAcceptInfoEntity aEntity = htacceptinfobll.GetEntityByHidCode(hidcode);
                        if (null != aEntity.ACCEPTDATE)
                        {
                            aEntity.ACCEPTDATE = aEntity.ACCEPTDATE.Value.AddDays(centity.POSTPONEDAYS);
                        }
                        htacceptinfobll.SaveForm(aEntity.ID, aEntity);
                        exentity.HANDLESIGN = "1"; //成功标记
                    }
                }
                centity.APPSIGN = AppSign; //移动端标记
                //更新整改信息
                htchangeinfobll.SaveForm(centity.ID, centity); //更新延期设置

                //保存申请及申请记录
                exentity.HIDCODE = hidcode;
                exentity.HIDID = keyValue;
                exentity.HANDLEDATE = DateTime.Now;
                exentity.POSTPONEDAYS = centity.POSTPONEDAYS.ToString();
                exentity.HANDLEUSERID = curUser.UserId;
                exentity.HANDLEUSERNAME = curUser.UserName;
                exentity.HANDLEDEPTCODE = curUser.DeptCode;
                exentity.HANDLEDEPTNAME = curUser.DeptName;
                exentity.CONTROLMEASURE = controlmeasure; //临时管控措施
                if (actiondo == "apply")
                {
                    exentity.HANDLETYPE = "0";  //申请

                    nextName = "整改延期审批";
                }
                else
                {
                    //成功
                    if (wfFlag == "2" && postponeresult == "1")
                    {
                        exentity.HANDLETYPE = wfFlag;  //处理类型 0 申请 1 审批 2 整改结束    wfFlag状态返回 2 时表示整改延期完成
                    }
                    //审批中
                    else if (wfFlag != "2" && postponeresult == "1")
                    {
                        exentity.HANDLETYPE = "1";  //审批中

                        nextName = "整改延期审批";
                    }
                    else //失败
                    {
                        if (postponeresult == "0")
                        {
                            exentity.HANDLETYPE = "-1";  //失败

                            nextName = "整改延期退回";
                        }
                    }
                }
                exentity.HANDLEID = !string.IsNullOrEmpty(handleid) ? handleid : DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
                if (exentity.HANDLETYPE == "0")
                {
                    exentity.POSTPONEREASON = postponereason; //申请下 为申请理由
                }
                else
                {
                    exentity.POSTPONEOPINION = postponereason; //审核下 为审核意见
                }
                exentity.POSTPONERESULT = postponeresult;  //申请结果
                exentity.APPSIGN = AppSign; //移动端标记
                htextensionbll.SaveForm("", exentity);

                //极光推送
                htworkflowbll.PushMessageForWorkFlow("隐患处理审批", nextName, wfentity, result);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "处理失败" };
            }
            return new { code = 0, count = 0, info = "处理成功" };
        }
        #endregion

        #region  隐患验收接口
        /// <summary>
        /// 隐患验收接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ProblemCheckPush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            WfControlResult result = new WfControlResult();
            try
            {
                string keyValue = dy.data.hiddenid; //主键

                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                HTBaseInfoEntity bentity = htbaseinfobll.GetEntity(keyValue);

                string hidcode = bentity.HIDCODE;

                HTAcceptInfoEntity entity = htacceptinfobll.GetEntityByHidCode(hidcode); //获取隐患验收信息

                HTChangeInfoEntity centity = htchangeinfobll.GetEntityByHidCode(hidcode); //获取隐患整改信息

                string ACCEPTID = entity.ID; //验收ID

                string ACCEPTSTATUS = res.Contains("checkresult") ? dy.data.checkresult : null; //验收结果

                string ACCEPTDATE = res.Contains("checktime") ? dy.data.checktime : null;//验收日期

                string ACCEPTIDEA = res.Contains("checkopinion") ? dy.data.checkopinion : null; //验收意见

                string DAMAGEDATE = res.Contains("damagedate") ? dy.data.damagedate : null; //销号日期 

                string CHANGEID = centity.ID; //整改ID

                string participant = string.Empty;  //获取流程下一节点的参与人员

                string wfFlag = string.Empty; //流程标识

                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = bentity.WORKSTREAM;
                wfentity.rankid = bentity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = bentity.HIDDEPART; //对应电厂id
                wfentity.argument1 = bentity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = curUser.DeptId; //当前部门
                wfentity.argument3 = bentity.HIDTYPE;//隐患类别
                wfentity.argument4 = bentity.HIDBMID; //所属部门
                //省级登记的
                if (bentity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else //厂级
                {
                    wfentity.mark = "厂级隐患排查";
                }
                //验收通过
                if (ACCEPTSTATUS == "1")
                {
                    wfentity.submittype = "提交";
                }
                else //验收不通过
                {
                    wfentity.submittype = "退回";
                }

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                //返回操作结果成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    if (result.ischangestatus)
                    {

                        if (!string.IsNullOrEmpty(participant))
                        {
                            //提交流程
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                #region 处理其他
                                //更新处理
                                entity.ACCEPTSTATUS = ACCEPTSTATUS;
                                entity.ACCEPTDATE = Convert.ToDateTime(ACCEPTDATE);
                                entity.ACCEPTIDEA = ACCEPTIDEA;

                                entity.ACCEPTDEPARTCODE = curUser.DeptCode;
                                entity.ACCEPTDEPARTNAME = curUser.DeptName;
                                entity.ACCEPTPERSON = curUser.UserId;
                                entity.ACCEPTPERSONNAME = curUser.UserName;
                                //}
                                if (!string.IsNullOrEmpty(DAMAGEDATE))
                                {
                                    entity.DAMAGEDATE = Convert.ToDateTime(DAMAGEDATE.ToString());
                                }

                                //上传隐患验收图片
                                entity.ACCEPTPHOTO = !string.IsNullOrEmpty(entity.ACCEPTPHOTO) ? entity.ACCEPTPHOTO : Guid.NewGuid().ToString();
                                UploadifyFile(entity.ACCEPTPHOTO, "checkimg", files);
                                entity.APPSIGN = AppSign; //移动端标记
                                htacceptinfobll.SaveForm(ACCEPTID, entity);

                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态


                                //退回则重新添加验收记录
                                #region 退回则重新添加验收记录
                                if (wfentity.submittype == "退回")
                                {
                                    string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                                    if (tagName == "隐患整改")
                                    {
                                        //整改记录
                                        HTChangeInfoEntity chEntity = htchangeinfobll.GetEntity(CHANGEID);
                                        HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                                        newEntity = chEntity;
                                        newEntity.CREATEDATE = DateTime.Now;
                                        newEntity.MODIFYDATE = DateTime.Now;
                                        newEntity.ID = null;
                                        newEntity.CHANGERESUME = null;
                                        newEntity.CHANGEFINISHDATE = null;
                                        newEntity.REALITYMANAGECAPITAL = 0;
                                        newEntity.HIDCHANGEPHOTO = Guid.NewGuid().ToString();
                                        newEntity.ATTACHMENT = Guid.NewGuid().ToString();
                                        newEntity.APPSIGN = AppSign; //移动端标记
                                        htchangeinfobll.SaveForm("", newEntity);
                                    }

                                    //验收记录
                                    HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(bentity.HIDCODE);
                                    HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                                    accptEntity = htacceptinfoentity;
                                    accptEntity.ID = null;
                                    accptEntity.CREATEDATE = DateTime.Now;
                                    accptEntity.MODIFYDATE = DateTime.Now;
                                    accptEntity.ACCEPTSTATUS = null;
                                    accptEntity.ACCEPTIDEA = null;
                                    accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                                    accptEntity.APPSIGN = AppSign; //移动端标记
                                    htacceptinfobll.SaveForm("", accptEntity);
                                }
                                #endregion

                                #endregion


                            }
                        }
                        else
                        {
                            return new { code = -1, count = 0, info = "请联系系统管理员，确认是否配置流程所需参与人!" };
                        }
                    }
                    else
                    {
                        //更新处理
                        entity.ACCEPTSTATUS = ACCEPTSTATUS;
                        entity.ACCEPTDATE = Convert.ToDateTime(ACCEPTDATE);
                        entity.ACCEPTIDEA = ACCEPTIDEA;

                        entity.ACCEPTDEPARTCODE = curUser.DeptCode;
                        entity.ACCEPTDEPARTNAME = curUser.DeptName;
                        entity.ACCEPTPERSON = curUser.UserId;
                        entity.ACCEPTPERSONNAME = curUser.UserName;
                        //}
                        if (!string.IsNullOrEmpty(DAMAGEDATE))
                        {
                            entity.DAMAGEDATE = Convert.ToDateTime(DAMAGEDATE.ToString());
                        }

                        //上传隐患验收图片
                        entity.ACCEPTPHOTO = !string.IsNullOrEmpty(entity.ACCEPTPHOTO) ? entity.ACCEPTPHOTO : Guid.NewGuid().ToString();
                        UploadifyFile(entity.ACCEPTPHOTO, "checkimg", files);
                        entity.APPSIGN = AppSign; //移动端标记
                        htacceptinfobll.SaveForm(ACCEPTID, entity);

                        //添加下一个验收对象
                        HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                        accptEntity = entity;
                        accptEntity.ID = string.Empty;
                        accptEntity.AUTOID = entity.AUTOID + 1;
                        accptEntity.CREATEDATE = DateTime.Now;
                        accptEntity.MODIFYDATE = DateTime.Now;
                        accptEntity.ACCEPTSTATUS = string.Empty;
                        accptEntity.ACCEPTIDEA = string.Empty;
                        accptEntity.ACCEPTDATE = null;
                        accptEntity.DAMAGEDATE = null;
                        accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString(); //验收图片
                        accptEntity.APPSIGN = "Web";
                        htacceptinfobll.SaveForm("", accptEntity);
                        //获取
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "处理失败" };
            }

            return new { code = 0, count = 0, info = result.message };
        }
        #endregion

        #region 隐患整改效果评估
        /// <summary>
        /// 隐患整改效果评估
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ProblemAssessPush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            WfControlResult result = new WfControlResult();
            try
            {
                string keyValue = dy.data.hiddenid; //隐患主键  

                string hidcode = dy.data.problemid; //隐患编码

                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                HTChangeInfoEntity chEntity = htchangeinfobll.GetEntityByHidCode(hidcode); //获取隐患整改信息

                HTEstimateEntity entity = htestimatebll.GetEntityByHidCode(hidcode); //获取隐患整改效果评估信息

                HTAcceptInfoEntity atEntity = htacceptinfobll.GetEntityByHidCode(hidcode); //获取隐患验收信息 

                string estimateresult = res.Contains("evaluateresult") ? dy.data.evaluateresult : null; //评估结果

                string estimatedate = res.Contains("estimatedate") ? dy.data.estimatedate : null; //评估日期

                string ESTIMATEID = string.Empty;
                if (null == entity)
                {
                    entity = new HTEstimateEntity();
                    entity.ESTIMATEPHOTO = Guid.NewGuid().ToString();
                }
                else
                {
                    ESTIMATEID = entity.ID;
                }
                entity.HIDCODE = hidcode;
                entity.ESTIMATERESULT = estimateresult; //评估结果
                entity.ESTIMATEDATE = Convert.ToDateTime(estimatedate);
                entity.ESTIMATEDEPARTCODE = curUser.DeptCode;
                entity.ESTIMATEDEPARTNAME = curUser.DeptName;
                entity.ESTIMATEPERSON = curUser.UserId;
                entity.ESTIMATEPERSONNAME = curUser.UserName;

                //上传隐患整改效果评估图片
                entity.ESTIMATEPHOTO = !string.IsNullOrEmpty(entity.ESTIMATEPHOTO) ? entity.ESTIMATEPHOTO : Guid.NewGuid().ToString();
                UploadifyFile(entity.ESTIMATEPHOTO, "evaluateimg", files);

                string CHANGEID = chEntity.ID; //整改ID

                string participant = string.Empty;  //获取流程下一节点的参与人员

                string wfFlag = string.Empty; //流程标识

                var baseEntity = htbaseinfobll.GetEntity(keyValue);
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = baseEntity.WORKSTREAM;
                wfentity.rankid = baseEntity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.HIDDEPART; //对应电厂id
                //省级登记的
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else //厂级
                {
                    wfentity.mark = "厂级隐患排查";
                }
                //评估合格
                if (estimateresult == "1")
                {
                    wfentity.submittype = "提交";
                }
                else //评估不通过
                {
                    wfentity.submittype = "退回";
                }

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                //返回操作结果成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    #region 提交流程
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //保存整改效果评估
                        htestimatebll.SaveForm(ESTIMATEID, entity);

                        //提交流程
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态

                            //退回后重新新增整改记录及整改效果评估记录
                            #region 退回后重新新增整改记录及整改效果评估记录
                            if (wfentity.submittype == "退回")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                                if (tagName == "隐患整改")
                                {
                                    //整改记录
                                    HTChangeInfoEntity cEntity = htchangeinfobll.GetEntity(CHANGEID);
                                    HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                                    newEntity = cEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.ID = null;
                                    newEntity.HIDCHANGEPHOTO = Guid.NewGuid().ToString();
                                    newEntity.ATTACHMENT = Guid.NewGuid().ToString();
                                    newEntity.CHANGERESUME = null;
                                    newEntity.CHANGEFINISHDATE = null;
                                    newEntity.REALITYMANAGECAPITAL = 0;
                                    newEntity.AUTOID = cEntity.AUTOID + 1;
                                    htchangeinfobll.SaveForm("", newEntity);
                                }

                                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(hidcode);
                                //验收记录
                                HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                                accptEntity = htacceptinfoentity;
                                accptEntity.ID = null;
                                accptEntity.CREATEDATE = DateTime.Now;
                                accptEntity.MODIFYDATE = DateTime.Now;
                                accptEntity.ACCEPTSTATUS = null;
                                accptEntity.ACCEPTIDEA = null;
                                accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                                htacceptinfobll.SaveForm("", accptEntity);

                            }
                            #endregion
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "目标流程参与者未定义" };
                    }

                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "处理失败" };
            }

            return new { code = 0, count = 0, info = result.message };
        }
        #endregion

        #region 隐患复查验证
        /// <summary>
        /// 隐患复查验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object ProblemReCheck()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            WfControlResult result = new WfControlResult();
            try
            {
                string keyValue = dy.data.hiddenid; //隐患主键  

                string hidcode = dy.data.problemid; //隐患编码

                HTChangeInfoEntity chEntity = htchangeinfobll.GetEntityByHidCode(hidcode); //获取隐患整改信息

                HtReCheckEntity entity = new HtReCheckEntity();// htrecheckbll.GetEntityByHidCode(hidcode); //获取隐患复查验证信息

                string ESTIMATEID = string.Empty; //复查ID

                string recheckstatus = res.Contains("rechecksstatus") ? dy.data.rechecksstatus : "0"; //复查验证结果

                string recheckdate = res.Contains("rechecksdate") ? dy.data.rechecksdate : null; //复查验证日期

                string rechecksidea = res.Contains("rechecksidea") ? dy.data.rechecksidea : null; //复查验证意见
                entity.RECHECKSTATUS = recheckstatus; //评估结果
                entity.RECHECKDATE = Convert.ToDateTime(recheckdate);
                entity.RECHECKDEPARTCODE = curUser.DeptCode;
                entity.RECHECKDEPARTNAME = curUser.DeptName;
                entity.RECHECKPERSON = curUser.UserId;
                entity.RECHECKIDEA = rechecksidea;
                entity.HIDCODE = hidcode;
                entity.RECHECKPERSONNAME = curUser.UserName;


                string CHANGEID = chEntity.ID; //整改ID

                string participant = string.Empty;  //获取流程下一节点的参与人员

                string wfFlag = string.Empty; //流程标识


                var baseEntity = htbaseinfobll.GetEntity(keyValue);
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = baseEntity.WORKSTREAM;
                wfentity.rankid = baseEntity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.HIDDEPART; //对应电厂id
                wfentity.argument1 = baseEntity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = curUser.DeptId; //当前部门
                wfentity.argument3 = baseEntity.HIDTYPE;//隐患类别
                wfentity.argument4 = baseEntity.HIDBMID; //所属部门
                //省级登记的
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else //厂级
                {
                    wfentity.mark = "厂级隐患排查";
                }
                //复查通过
                if (entity.RECHECKSTATUS == "1")
                {
                    wfentity.submittype = "提交";
                }
                else //复查不通过
                {
                    wfentity.submittype = "退回";
                }

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                //返回操作结果成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    //如果是更改状态
                    if (result.ischangestatus)
                    {
                        if (!string.IsNullOrEmpty(participant))
                        {
                            //提交流程
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                //保存复查验证
                                htrecheckbll.SaveForm(ESTIMATEID, entity); //保存信息

                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态

                                //退回后重新新增整改记录及整改效果评估记录
                                #region 退回后重新新增整改记录及整改效果评估记录
                                if (wfentity.submittype == "退回")
                                {
                                    string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                                    //退回到整改后才新增
                                    if (tagName == "隐患整改")
                                    {
                                        //整改记录
                                        HTChangeInfoEntity cEntity = htchangeinfobll.GetEntity(CHANGEID);
                                        if (null != cEntity)
                                        {
                                            HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                                            newEntity = cEntity;
                                            newEntity.CREATEDATE = DateTime.Now;
                                            newEntity.MODIFYDATE = DateTime.Now;
                                            newEntity.ID = null;
                                            newEntity.AUTOID = cEntity.AUTOID + 1;
                                            newEntity.HIDCHANGEPHOTO = Guid.NewGuid().ToString();
                                            newEntity.ATTACHMENT = Guid.NewGuid().ToString();
                                            newEntity.CHANGERESUME = null;
                                            newEntity.CHANGEFINISHDATE = null;
                                            newEntity.REALITYMANAGECAPITAL = 0;
                                            htchangeinfobll.SaveForm("", newEntity);
                                        }
                                    }

                                    //验收记录
                                    HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(hidcode);
                                    if (null != htacceptinfoentity)
                                    {
                                        HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                                        accptEntity = htacceptinfoentity;
                                        accptEntity.ID = null;
                                        accptEntity.CREATEDATE = DateTime.Now;
                                        accptEntity.MODIFYDATE = DateTime.Now;
                                        accptEntity.ACCEPTSTATUS = null;
                                        accptEntity.ACCEPTIDEA = null;
                                        accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString();
                                        accptEntity.AUTOID = htacceptinfoentity.AUTOID + 1;
                                        htacceptinfobll.SaveForm("", accptEntity);
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            return new { code = -1, count = 0, info = "目标流程参与者未定义" };
                        }
                    }
                    else
                    {
                        //保存复查验证
                        htrecheckbll.SaveForm(ESTIMATEID, entity); //保存信息
                        //获取
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "处理失败" };
            }

            return new { code = 0, count = 0, info = result.message };
        }
        #endregion

        #region 获取隐患详情信息
        /// <summary>
        /// 获取隐患详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProblemInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hiddenid = dy.data.hiddenid; //隐患主键

            var baseInfo = htbaseinfobll.GetHiddenByKeyValue(hiddenid);

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidBmEnableOrganize','IsEnableMajorClassify','AcceptPersonControl','ControlPicMustUpload'");

            HiddenData entity = new HiddenData();

            if (baseInfo.Rows.Count == 1)
            {
                entity.hiddenid = baseInfo.Rows[0]["hiddenid"].ToString(); //key 
                entity.problemid = baseInfo.Rows[0]["problemid"].ToString();  //隐患编码
                entity.hidbmid = baseInfo.Rows[0]["hidbmid"].ToString();  //所属部门id
                entity.hidbmname = baseInfo.Rows[0]["hidbmname"].ToString();  //所属部门name
                entity.safetydetailid = baseInfo.Rows[0]["safetycheckobjectid"].ToString();  //安全检查id
                entity.isselfchange = baseInfo.Rows[0]["isselfchange"].ToString(); //是否本部门整改
                entity.isformulate = baseInfo.Rows[0]["isformulate"].ToString();  //是否已经制定了整改计划
                entity.deptid = baseInfo.Rows[0]["hiddepart"].ToString();
                entity.deptname = baseInfo.Rows[0]["hiddepartname"].ToString();
                entity.hidrank = baseInfo.Rows[0]["hidrank"].ToString(); //隐患级别
                entity.rankname = baseInfo.Rows[0]["rankname"].ToString();
                entity.hidtypeid = baseInfo.Rows[0]["categoryid"].ToString(); //隐患类别
                entity.hidtype = baseInfo.Rows[0]["category"].ToString();//隐患类别
                entity.categoryid = baseInfo.Rows[0]["categoryid"].ToString();//隐患类别
                entity.category = baseInfo.Rows[0]["category"].ToString();//隐患类别
                entity.hidpointid = baseInfo.Rows[0]["hidpointid"].ToString();
                entity.hidpoint = baseInfo.Rows[0]["hidpoint"].ToString();
                entity.hidplace = baseInfo.Rows[0]["hidplace"].ToString();
                entity.hiddescribe = baseInfo.Rows[0]["hiddescribe"].ToString();
                entity.dutypersonid = baseInfo.Rows[0]["dutypersonid"].ToString();
                entity.dutyperson = baseInfo.Rows[0]["dutyperson"].ToString();
                entity.dutydeptcode = baseInfo.Rows[0]["dutydeptcode"].ToString();
                entity.dutydeptid = baseInfo.Rows[0]["dutydeptid"].ToString();
                entity.dutydept = baseInfo.Rows[0]["dutydept"].ToString();
                entity.dutytel = baseInfo.Rows[0]["dutytel"].ToString();
                entity.reportdigest = baseInfo.Rows[0]["reportdigest"].ToString();
                entity.engineerid = baseInfo.Rows[0]["hidproject"].ToString();
                entity.engineername = baseInfo.Rows[0]["hidprojectname"].ToString();

                entity.deviceid = baseInfo.Rows[0]["deviceid"].ToString();
                entity.devicecode = baseInfo.Rows[0]["devicecode"].ToString();
                entity.devicename = baseInfo.Rows[0]["devicename"].ToString();
                entity.monitorpersonid = baseInfo.Rows[0]["monitorpersonid"].ToString(); //厂级监控人员
                entity.monitorpersonname = baseInfo.Rows[0]["monitorpersonname"].ToString();
                entity.relevanceid = baseInfo.Rows[0]["relevanceid"].ToString();
                entity.relevancetype = baseInfo.Rows[0]["relevancetype"].ToString();
                entity.majorclassify = baseInfo.Rows[0]["majorclassify"].ToString();
                entity.majorclassifyname = baseInfo.Rows[0]["majorclassifyname"].ToString();
                entity.hidname = baseInfo.Rows[0]["hidname"].ToString();
                entity.hidstatus = baseInfo.Rows[0]["hidstatus"].ToString();
                entity.hidconsequence = baseInfo.Rows[0]["hidconsequence"].ToString(); //可能导致的后果

                entity.actionperson = baseInfo.Rows[0]["actionperson"].ToString();
                entity.applicationstatus = baseInfo.Rows[0]["applicationstatus"].ToString();
                entity.reformbackreason = baseInfo.Rows[0]["backreason"].ToString();
                entity.isenableback = true; //启用
                var historyacceptList = htacceptinfobll.GetHistoryList(entity.problemid).ToList();
                if (historyacceptList.Count() > 0)
                {
                    entity.isenableback = false; //不启用是否回退
                }
                entity.chargeperson = baseInfo.Rows[0]["chargeperson"].ToString(); //整改责任负责人
                entity.chargepersonname = baseInfo.Rows[0]["chargepersonname"].ToString();   //整改责任负责人
                entity.chargedeptid = baseInfo.Rows[0]["chargedeptid"].ToString(); //整改责任单位
                entity.chargedeptname = baseInfo.Rows[0]["chargedeptname"].ToString(); //整改责任单位
                entity.isappoint = baseInfo.Rows[0]["isappoint"].ToString(); //是否指定整改责任人
                entity.deadinetime = baseInfo.Rows[0]["deadinetime"].ToString().Replace("00:00:00", "").Trim();//整改截至日期
                entity.reformfinishdate = baseInfo.Rows[0]["reformfinishdate"].ToString().Replace("00:00:00", "").Trim(); //整改结束时间
                entity.reformresult = baseInfo.Rows[0]["reformresult"].ToString();
                entity.reformmeasure = baseInfo.Rows[0]["reformmeasure"].ToString();
                entity.reformdescribe = baseInfo.Rows[0]["reformdescribe"].ToString();
                entity.reformtype = baseInfo.Rows[0]["reformtype"].ToString();
                entity.checkpersonid = baseInfo.Rows[0]["checkpersonid"].ToString();
                entity.checkperson = baseInfo.Rows[0]["checkperson"].ToString();
                entity.acceptdepartcode = baseInfo.Rows[0]["acceptdepartcode"].ToString();
                entity.acceptdepartname = baseInfo.Rows[0]["acceptdepartname"].ToString();
                entity.isupaccept = baseInfo.Rows[0]["isupaccept"].ToString();
                entity.checkopinion = baseInfo.Rows[0]["checkopinion"].ToString();  //验收意见
                entity.checktime = baseInfo.Rows[0]["checktime"].ToString().Replace("00:00:00", "").Trim(); //验收日期
                entity.checkman = baseInfo.Rows[0]["checkman"].ToString();
                entity.checkmanname = baseInfo.Rows[0]["checkmanname"].ToString();
                entity.checkdept = baseInfo.Rows[0]["checkdept"].ToString();
                entity.checkdeptname = baseInfo.Rows[0]["checkdeptname"].ToString();
                entity.checkresult = baseInfo.Rows[0]["checkresult"].ToString();
                entity.workstream = baseInfo.Rows[0]["workstream"].ToString();
                entity.isexpose = baseInfo.Rows[0]["isexpose"].ToString();
                entity.planmanagecapital = baseInfo.Rows[0]["planmanagecapital"].ToString();
                entity.realitymanagecapital = baseInfo.Rows[0]["realitymanagecapital"].ToString();
                entity.checktypeid = baseInfo.Rows[0]["checktypeid"].ToString();
                entity.checktype = baseInfo.Rows[0]["checktype"].ToString();
                entity.dangerlocation = baseInfo.Rows[0]["dangerlocation"].ToString();
                entity.reportsummary = baseInfo.Rows[0]["reportsummary"].ToString();
                entity.causereason = baseInfo.Rows[0]["causereason"].ToString();
                entity.damagelevel = baseInfo.Rows[0]["damagelevel"].ToString();
                entity.preventmeasure = baseInfo.Rows[0]["preventmeasure"].ToString();
                entity.reformplan = baseInfo.Rows[0]["reformplan"].ToString();
                entity.replan = baseInfo.Rows[0]["replan"].ToString();
                entity.isgetafter = baseInfo.Rows[0]["tosupervise"].ToString();
                entity.checkdate = baseInfo.Rows[0]["checkdate"].ToString().Replace("00:00:00", "").Trim(); //排查日期
                entity.safetycheckname = baseInfo.Rows[0]["safetycheckname"].ToString(); //安全检查名称

                entity.approvalperson = baseInfo.Rows[0]["approvalpersonname"].ToString();
                entity.approvaldate = baseInfo.Rows[0]["approvaldate"].ToString().Replace("00:00:00", "").Trim();
                entity.approvalresult = baseInfo.Rows[0]["approvalresult"].ToString();
                entity.approvalreason = baseInfo.Rows[0]["approvalreason"].ToString();
                entity.approvaldepartname = baseInfo.Rows[0]["approvaldepartname"].ToString(); //评估部门
                /*******复查验证*****/
                entity.recheckspersonid = baseInfo.Rows[0]["recheckperson"].ToString(); //复查人
                entity.rechecksperson = baseInfo.Rows[0]["recheckpersonname"].ToString(); //复查人
                entity.rechecksdepartcode = baseInfo.Rows[0]["recheckdepartcode"].ToString(); //复查部门编码
                entity.rechecksdepartname = baseInfo.Rows[0]["recheckdepartname"].ToString(); //复查部门名称
                entity.rechecksdate = baseInfo.Rows[0]["recheckdate"].ToString().Replace("00:00:00", "").Trim(); //复查日期
                entity.rechecksstatus = baseInfo.Rows[0]["recheckstatus"].ToString(); //复查情况
                entity.rechecksidea = baseInfo.Rows[0]["recheckidea"].ToString(); //复查意见

                entity.evaluateperson = baseInfo.Rows[0]["estimatepersonname"].ToString();
                entity.evaluateresult = baseInfo.Rows[0]["estimateresult"].ToString();
                entity.estimatedate = baseInfo.Rows[0]["estimatedate"].ToString().Replace("00:00:00", "").Trim();

                //判定是否存在上报功能
                #region 判定是否存在上报功能
                string mark = string.Empty;
                if (entity.reformtype == "2")
                {
                    mark = "省级隐患排查";
                }
                else
                {
                    mark = "厂级隐患排查";
                }
                entity.isupsubmit = baseInfo.Rows[0]["upsubmit"].ToString(); //是否上报
                entity.isshowappoint = GetCurUserWfAuth(entity.hidrank, entity.workstream, "制定整改计划", mark, "制定提交", entity.majorclassify, null, entity.hidtypeid, entity.hidbmid, string.Empty, hiddenid); //是否具有指定整改责任人权限
                entity.ishaveupsubmit = GetCurUserWfAuth(entity.hidrank, entity.workstream, "", mark, "上报", entity.majorclassify, null, entity.hidtypeid, entity.hidbmid, string.Empty, hiddenid); //判定是否存在上报功能权限
                #endregion

                /******判定是否安全领导*****/
                //安全分管领导
                if (curUser.RoleName.Contains("公司领导") && curUser.RoleName.Contains("公司级用户") && curUser.RoleName.Contains("安全管理员"))
                {
                    entity.currole = 1;
                }

                string hidphoto = baseInfo.Rows[0]["hidphoto"].ToString();  //隐患图片
                string hidchangephoto = baseInfo.Rows[0]["hidchangephoto"].ToString();   //隐患整改图片
                string attachment = baseInfo.Rows[0]["attachment"].ToString();   //隐患整改附件
                string acceptphoto = baseInfo.Rows[0]["acceptphoto"].ToString();   //隐患验收图片
                string estimatephoto = baseInfo.Rows[0]["estimatephoto"].ToString();   //隐患整改效果评估图片
                string approvalfj = baseInfo.Rows[0]["approvalfile"].ToString();  //隐患评估附件 
                IList<Photo> problempics = new List<Photo>(); //隐患图片
                IList<Photo> reformpics = new List<Photo>(); //整改图片
                IList<Photo> attachmentpics = new List<Photo>(); //整改附件 
                IList<Photo> checkpics = new List<Photo>();  //验收图片 
                IList<Photo> evaluatepics = new List<Photo>();  //整改效果评估图片 
                IList<Photo> approvalfile = new List<Photo>();  //整改效果评估图片  
                //隐患图片
                IEnumerable<FileInfoEntity> hidfile = fileInfoBLL.GetImageListByTop5Object(hidphoto);
                foreach (FileInfoEntity fentity in hidfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    problempics.Add(p);
                }
                entity.problempics = problempics;
                //整改图片
                IEnumerable<FileInfoEntity> changefile = fileInfoBLL.GetImageListByTop5Object(hidchangephoto);
                foreach (FileInfoEntity fentity in changefile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    reformpics.Add(p);
                }
                entity.reformpics = reformpics;

                //整改附件
                IEnumerable<FileInfoEntity> attachmentfile = fileInfoBLL.GetImageListByObject(attachment);
                foreach (FileInfoEntity fentity in attachmentfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    attachmentpics.Add(p);
                }
                entity.attachment = attachmentpics;

                //验收图片
                IEnumerable<FileInfoEntity> acceptfile = fileInfoBLL.GetImageListByTop5Object(acceptphoto);
                foreach (FileInfoEntity fentity in acceptfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    checkpics.Add(p);
                }
                entity.checkpics = checkpics;
                //隐患评估附件
                IEnumerable<FileInfoEntity> appfile = fileInfoBLL.GetImageListByObject(approvalfj);
                foreach (FileInfoEntity fentity in appfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    approvalfile.Add(p);
                }
                entity.approvalpics = approvalfile;

                //整改效果评估图片
                IEnumerable<FileInfoEntity> evaluatefile = fileInfoBLL.GetImageListByTop5Object(estimatephoto);
                foreach (FileInfoEntity fentity in evaluatefile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    evaluatepics.Add(p);
                }
                entity.evaluatepics = evaluatepics;
                //判断评估人是否具有操作是否本部门按钮(国电新疆专用)
                if (curUser.RoleName.Contains("部门级用户") && curUser.RoleName.Contains("安全管理员") && !curUser.RoleName.Contains("厂级部门用户"))
                {
                    entity.isusechangeck = true;
                }
                else
                {
                    entity.isusechangeck = false;
                }
                entity.checkflow = GetCheckFlowData(entity.hiddenid, "Hidden");
                entity.isdeliver = htworkflowbll.GetCurUserWfAuth("一般隐患", "隐患整改", "隐患整改", "厂级隐患排查", "转交") == "1"; //是否具有转交功能
                entity.isacceptdeliver = htworkflowbll.GetCurUserWfAuth("一般隐患", "隐患验收", "隐患验收", "厂级隐患排查", "转交") == "1";//是否具有转交功能
                entity.isenablebm = itemlist.Where(p => p.EnCode == "HidBmEnableOrganize").Where(p => p.ItemValue == curUser.OrganizeCode).Count() > 0; //是否启用所属部门
                string argsCode = curUser.OrganizeCode;
                if (!string.IsNullOrEmpty(entity.deptid))
                {
                    argsCode = departmentBLL.GetEntity(entity.deptid).DeptCode;
                }
                bool IsEnableMajorClassify = itemlist.Where(p => p.EnCode == "IsEnableMajorClassify").Where(p => p.ItemValue == argsCode).Count() > 0;//是否启用专业分类 
                entity.isenablemajorclassify = IsEnableMajorClassify;
                //获取整改对象
                var changeplanentity = htbaseinfobll.GetChangePlanEntity(entity.hiddenid);
                if (null != changeplanentity)
                {
                    entity.changeplanid = changeplanentity.ID;
                    entity.changeplaninfo = changeplanentity.REMARK;
                    //整改计划附件
                    IList<Photo> changplanfiles = new List<Photo>();  //整改计划附件  
                    IEnumerable<FileInfoEntity> changeplanlist = fileInfoBLL.GetImageListByTop5Object(changeplanentity.ATTACHMENT);
                    foreach (FileInfoEntity fentity in changeplanlist)
                    {
                        Photo p = new Photo();
                        p.id = fentity.FileId;
                        p.filename = fentity.FileName;
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                        p.folderid = fentity.FolderId;
                        changplanfiles.Add(p);
                    }
                    entity.changeplanfiles = changplanfiles;
                }

                //获取是否设置验收人权限
                var curdept = departmentBLL.GetEntity(entity.deptid);
                if (null != curdept)
                {
                    bool IsEnableAccept = itemlist.Where(p => p.EnCode == "AcceptPersonControl").Where(p => p.ItemValue == curdept.DeptCode).Count() > 0;
                    entity.isenableaccept = IsEnableAccept;

                    if (entity.workstream != "隐患验收" && entity.workstream != "复查验证" && entity.workstream != "整改效果评估" && entity.workstream != "整改结束")
                    {
                        if (!string.IsNullOrEmpty(entity.isupaccept))
                        {
                            if (entity.isupaccept == "0" && IsEnableAccept)
                            {
                                entity.checkpersonid = string.Empty;
                                entity.checkperson = string.Empty;
                                entity.acceptdepartcode = string.Empty;
                                entity.acceptdepartname = string.Empty;
                                entity.checktime = string.Empty; //验收日期
                            }
                        }
                        else
                        {
                            if (IsEnableAccept)
                            {
                                entity.checkpersonid = string.Empty;
                                entity.checkperson = string.Empty;
                                entity.acceptdepartcode = string.Empty;
                                entity.acceptdepartname = string.Empty;
                                entity.checktime = string.Empty; //验收日期
                            }
                        }
                    }
                }
                string ControlPicMustUpload = string.Empty;
                var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == curUser.OrganizeId);
                if (cpmu.Count() > 0)
                {
                    ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
                }
                entity.yhpicmustupload = ControlPicMustUpload.Contains("HIDPHOTO");//隐患图片是否必传
                entity.zgpicmustupload = ControlPicMustUpload.Contains("HIDCHANGEPHOTO");//隐患整改图片是否必传
                entity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPHOTO");//隐患验收图片是否必传 
            }
            return new { code = 0, count = 0, info = "获取成功", data = entity };
        }
        #endregion

        #region 隐患流程图
        /// <summary>
        /// 流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData> GetCheckFlowData(string keyValue, string mode)
        {
            var flowdt = htworkflowbll.QueryWorkFlowMapForApp(keyValue, mode);
            List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData> checkflow = new List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData>();
            if (flowdt.Rows.Count > 1)
            {
                //已经处理的部分
                foreach (DataRow row in flowdt.Rows)
                {
                    ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData checkentity = new Entity.HighRiskWork.ViewModel.CheckFlowData();
                    //如果当前起始为空，则是登记阶段
                    if (!string.IsNullOrEmpty(row["fromname"].ToString()))
                    {
                        checkentity.isoperate = "0"; //是否正在处理  0 否 1 是
                        checkentity.isapprove = "1"; //是否已经处理过 0 否 1 是
                        checkentity.auditdate = row["createdate"].ToString();
                        checkentity.auditdeptname = row["deptname"].ToString();
                        checkentity.auditstate = row["fromname"].ToString() + "已处理";
                        checkentity.auditusername = row["username"].ToString();
                        checkentity.auditremark = row["contents"].ToString();
                        checkflow.Add(checkentity);
                    }
                }
                //最后结点
                string lastflow = flowdt.Rows[flowdt.Rows.Count - 1]["toname"].ToString();
                ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData checkflowdata = new Entity.HighRiskWork.ViewModel.CheckFlowData();
                if (lastflow == "整改结束" || lastflow == "流程结束")
                {
                    checkflowdata.isoperate = "0"; //是否正在处理  0 否 1 是
                    checkflowdata.isapprove = "1"; //是否已经处理过 0 否 1 是
                    checkflowdata.auditdate = flowdt.Rows[flowdt.Rows.Count - 1]["createdate"].ToString();
                    checkflowdata.auditdeptname = flowdt.Rows[flowdt.Rows.Count - 1]["deptname"].ToString();
                    checkflowdata.auditstate = lastflow;
                    checkflowdata.auditusername = flowdt.Rows[flowdt.Rows.Count - 1]["username"].ToString();
                    checkflowdata.auditremark = flowdt.Rows[flowdt.Rows.Count - 1]["contents"].ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastflow))
                    {
                        checkflowdata.isoperate = "1"; //是否正在处理  0 否 1 是
                        checkflowdata.isapprove = "0"; //是否已经处理过 0 否 1 是
                        checkflowdata.auditdate = null;
                        checkflowdata.auditstate = lastflow + "处理中";
                        checkflowdata.auditusername = flowdt.Rows[flowdt.Rows.Count - 1]["participantname"].ToString();
                        checkflowdata.auditremark = string.Empty;//flowdt.Rows[flowdt.Rows.Count - 1]["contents"].ToString();
                        string[] lastuser = flowdt.Rows[flowdt.Rows.Count - 1]["participant"].ToString().Replace("$", "").ToString().Split(',');
                        string newuserStr = "";
                        foreach (string s in lastuser)
                        {
                            newuserStr += "'" + s + "',";
                        }
                        if (!string.IsNullOrEmpty(newuserStr))
                        {
                            newuserStr = newuserStr.Substring(0, newuserStr.Length - 1).ToString();
                        }
                        string newSql = string.Format(@"select a.userid,a.account,a.realname, b.fullname from base_user a 
                                        left join (
                                           select departmentid,encode, fullname from base_department 
                                        ) b on  a.departmentcode = b.encode where  a.account in ({0})", newuserStr);
                        var newDt = htbaseinfobll.GetGeneralQueryBySql(newSql);
                        string lastNodeDept = ",";
                        foreach (DataRow lastNodeRow in newDt.Rows)
                        {
                            string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                            if (!lastNodeDept.Contains(tempDept))
                            {
                                lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept != ",")
                        {
                            lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                        }
                        else
                        {
                            lastNodeDept = "";
                        }
                        checkflowdata.auditdeptname = lastNodeDept;
                    }
                }
                checkflow.Add(checkflowdata);
            }
            else //仅有登记记录 
            {
                if (!string.IsNullOrEmpty(flowdt.Rows[0]["toname"].ToString()))
                {
                    ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData checkflowdata = new Entity.HighRiskWork.ViewModel.CheckFlowData();
                    checkflowdata.isoperate = "0"; //是否正在处理  0 否 1 是
                    checkflowdata.isapprove = "1"; //是否已经处理过 0 否 1 是
                    checkflowdata.auditdate = flowdt.Rows[0]["createdate"].ToString();
                    checkflowdata.auditdeptname = flowdt.Rows[0]["deptname"].ToString();
                    checkflowdata.auditstate = flowdt.Rows[0]["toname"].ToString() + "已处理";
                    checkflowdata.auditusername = flowdt.Rows[0]["username"].ToString();
                    checkflowdata.auditremark = flowdt.Rows[0]["contents"].ToString();
                    checkflow.Add(checkflowdata);
                }
            }
            return checkflow;
        }
        #endregion

        #region 获取评估历史记录列表
        /// <summary>
        /// 获取评估历史记录列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetApprovalInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hidcode = dy.data.problemid;   //隐患编码

            var list = htapprovalbll.GetHistoryList(hidcode);

            IList<ApprovalData> result = new List<ApprovalData>();

            int i = list.Count();
            foreach (HTApprovalEntity entity in list)
            {
                ApprovalData data = new ApprovalData();
                data.approvaltimes = i.ToString();
                data.problemid = entity.HIDCODE;
                data.approvalpersonid = entity.APPROVALPERSON;
                data.approvalperson = entity.APPROVALPERSONNAME;
                data.approvaldepartcode = entity.APPROVALDEPARTCODE;
                data.approvaldepartname = entity.APPROVALDEPARTNAME;
                data.approvaldate = entity.APPROVALDATE != null ? entity.APPROVALDATE.Value.ToString("yyyy-MM-dd") : "";
                data.approvalresult = entity.APPROVALRESULT;
                data.approvalreason = entity.APPROVALREASON;
                IList<Photo> approvalpics = new List<Photo>(); //整改图片
                IEnumerable<FileInfoEntity> approvalfile = fileInfoBLL.GetImageListByObject(entity.APPROVALFILE);
                foreach (FileInfoEntity fentity in approvalfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    approvalpics.Add(p);
                }
                data.approvalpics = approvalpics;
                i--;
                result.Add(data);
            }

            return new { code = 0, info = "获取数据成功", count = list.Count(), data = result };
        }
        #endregion

        #region 获取整改历史记录列表
        /// <summary>
        /// 获取整改历史记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetReformInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hidcode = dy.data.problemid;   //隐患编码

            var list = htchangeinfobll.GetHistoryList(hidcode);

            var result = new List<ReformData>();
            int i = list.Count();
            foreach (HTChangeInfoEntity entity in list)
            {
                ReformData data = new ReformData();
                data.reformtimes = i.ToString();
                data.problemid = entity.HIDCODE;
                data.dutyperson = entity.CHANGEPERSONNAME; //整改人姓名
                data.dutydept = entity.CHANGEDUTYDEPARTNAME; //整改单位
                data.dutytel = entity.CHANGEDUTYTEL; //整改人联系方式
                data.deadinetime = entity.CHANGEDEADINE != null ? entity.CHANGEDEADINE.Value.ToString("yyyy-MM-dd") : ""; //整改截至时间
                data.reformfinishdate = entity.CHANGEFINISHDATE != null ? entity.CHANGEFINISHDATE.Value.ToString("yyyy-MM-dd") : ""; //整改完成时间
                data.reformdescribe = entity.CHANGERESUME;//整改情况描述
                data.realitymanagecapital = null != entity.REALITYMANAGECAPITAL ? entity.REALITYMANAGECAPITAL.ToString() : "0"; //实际治理资金
                data.reformmeasure = entity.CHANGEMEASURE; //整改措施
                data.reformresult = entity.CHANGERESULT; //整改结果
                data.reformbackreason = entity.BACKREASON; //回退原因
                data.planmanagecapital = null != entity.PLANMANAGECAPITAL ? entity.PLANMANAGECAPITAL.ToString() : "0"; //计划治理资金
                IList<Photo> reformpics = new List<Photo>(); //整改图片
                IEnumerable<FileInfoEntity> changefile = fileInfoBLL.GetImageListByObject(entity.HIDCHANGEPHOTO);
                foreach (FileInfoEntity fentity in changefile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    reformpics.Add(p);
                }
                data.reformpics = reformpics;
                result.Add(data);
                i--;
            }

            return new { code = 0, count = 0, info = "获取成功", data = result };
        }
        #endregion

        #region 获取验收历史记录列表
        /// <summary>
        /// 获取验收历史记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCheckInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hidcode = dy.data.problemid;   //隐患编码

            var list = htacceptinfobll.GetHistoryList(hidcode);


            var result = new List<AcceptData>();
            int i = list.Count();
            foreach (HTAcceptInfoEntity entity in list)
            {
                AcceptData data = new AcceptData();
                data.checktimes = i.ToString();
                data.problemid = entity.HIDCODE;
                data.checkperson = entity.ACCEPTPERSONNAME;
                data.checktime = entity.ACCEPTDATE != null ? entity.ACCEPTDATE.Value.ToString("yyyy-MM-dd") : "";
                data.checkopinion = entity.ACCEPTIDEA;
                data.checkresult = entity.ACCEPTSTATUS;
                IList<Photo> checkpics = new List<Photo>(); //整改图片
                IEnumerable<FileInfoEntity> acceptfile = fileInfoBLL.GetImageListByObject(entity.ACCEPTPHOTO);
                foreach (FileInfoEntity fentity in acceptfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    checkpics.Add(p);
                }
                data.checkpics = checkpics;
                result.Add(data);
                i--;
            }

            return new { code = 0, count = 0, info = "获取成功", data = result };
        }
        #endregion

        #region 获取复查验证历史记录列表
        /// <summary>
        /// 获取复查验证历史记录列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRecheckInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hidcode = dy.data.problemid;   //隐患编码

            var list = htrecheckbll.GetHistoryList(hidcode);

            IList<ReCheckData> result = new List<ReCheckData>();

            int i = list.Count();
            foreach (HtReCheckEntity entity in list)
            {
                ReCheckData data = new ReCheckData();
                data.recheckstimes = i.ToString();
                data.problemid = entity.HIDCODE;
                data.recheckspersonid = entity.RECHECKPERSON;
                data.rechecksperson = entity.RECHECKPERSONNAME;
                data.rechecksdepartcode = entity.RECHECKDEPARTCODE;
                data.rechecksdepartname = entity.RECHECKDEPARTNAME;
                data.rechecksdate = entity.RECHECKDATE != null ? entity.RECHECKDATE.Value.ToString("yyyy-MM-dd") : "";
                data.rechecksstatus = entity.RECHECKSTATUS;
                data.rechecksidea = entity.RECHECKIDEA;
                i--;
                result.Add(data);
            }

            return new { code = 0, info = "获取数据成功", count = list.Count(), data = result };
        }
        #endregion

        #region 获取整改效果评估列表
        /// <summary>
        /// 获取整改效果评估
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEstimateInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hidcode = dy.data.problemid;   //隐患编码

            var list = htestimatebll.GetHistoryList(hidcode);

            var result = new List<EstimateData>();
            int i = list.Count();
            foreach (HTEstimateEntity entity in list)
            {
                EstimateData data = new EstimateData();
                data.evaluatetimes = i.ToString();
                data.problemid = entity.HIDCODE;
                data.evaluateperson = entity.ESTIMATEPERSONNAME;
                data.estimatedate = entity.ESTIMATEDATE != null ? entity.ESTIMATEDATE.Value.ToString("yyyy-MM-dd") : "";
                data.evaluateresult = entity.ESTIMATERESULT;
                IList<Photo> checkpics = new List<Photo>(); //整改图片
                IEnumerable<FileInfoEntity> acceptfile = fileInfoBLL.GetImageListByObject(entity.ESTIMATEPHOTO);
                foreach (FileInfoEntity fentity in acceptfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    checkpics.Add(p);
                }
                data.evaluatepics = checkpics;
                result.Add(data);
                i--;
            }

            return new { code = 0, info = "获取数据成功", count = list.Count(), data = result };
        }
        #endregion

        #region 获取整改延期信息列表
        /// <summary>
        /// 获取延期信息列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDelayApplyInfoList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string hidcode = dy.data.problemid;   //隐患编码

            var list = htextensionbll.GetList(hidcode);
            var changeEntity = htchangeinfobll.GetEntityByHidCode(hidcode);

            var handleList = list.Select(p => p.HANDLEID).ToList().Distinct().ToList();
            IList<PostPoneData> result = new List<PostPoneData>();

            foreach (string handleid in handleList)
            {
                var templist = list.Where(p => p.HANDLEID == handleid).ToList().OrderByDescending(p => p.CREATEDATE).ToList();
                PostPoneData data = new PostPoneData();
                if (null != templist.FirstOrDefault())
                {
                    var entity = templist.FirstOrDefault();
                    data.handleid = entity.HANDLEID;
                    data.hiddenid = entity.HIDID;
                    data.problemid = entity.HIDCODE;
                    data.postponedays = entity.POSTPONEDAYS; //申请天数
                    data.postponeresult = entity.POSTPONERESULT;//审批结果
                    data.applyreason = entity.POSTPONEOPINION;//审批意见
                    data.controlmeasure = entity.CONTROLMEASURE; //临时管控措施
                    data.applydate = null != entity.HANDLEDATE ? entity.HANDLEDATE.Value.ToString("yyyy-MM-dd") : ""; //申请时间/审批时间
                    data.applyperson = entity.HANDLEUSERNAME;//申请人/审批人
                    data.applypersonid = entity.HANDLEUSERID;//申请人/审批人
                    data.applydept = entity.HANDLEDEPTCODE;//申请部门/审批部门
                    data.applydeptname = entity.HANDLEDEPTNAME;//申请部门/审批部门
                    if (entity.HANDLETYPE == "0")
                    {
                        data.handlestate = "延期申请"; //状态
                        data.applyreason = entity.POSTPONEREASON; //申请理由
                    }
                    if (entity.HANDLETYPE == "1")
                    {
                        data.handlestate = "延期审批";
                    }
                    if (entity.HANDLETYPE == "2")
                    {
                        data.handlestate = "延期完成";
                    }
                    if (entity.HANDLETYPE == "-1")
                    {
                        data.handlestate = "延期失败";
                    }
                    result.Add(data);
                }
            }


            return new
            {
                code = 0,
                info = "获取数据成功",
                count = handleList.Count(),
                data = result
            };
        }
        #endregion

        #region 获取整改延期详情信息
        /// <summary>
        /// 获取延期详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object DelayApplyInfo([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string hidcode = dy.data.problemid;
            string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid.ToString() : string.Empty;
            string handleid = res.Contains("handleid") ? dy.data.handleid.ToString() : string.Empty;
            List<HTExtensionEntity> list = new List<HTExtensionEntity>();
            if (!string.IsNullOrEmpty(handleid))
            {
                list = htextensionbll.GetList(hidcode).Where(p => p.HANDLEID == handleid).OrderBy(p=>p.CREATEDATE).ToList();
            }
            else
            {
                var templist = htextensionbll.GetListByCondition(hidcode).OrderByDescending(p => p.CREATEDATE).ToList();
                if (templist.Count() > 0)
                {
                    list = htextensionbll.GetList(hidcode).Where(p => p.HANDLEID == templist[0].HANDLEID).OrderBy(p => p.CREATEDATE).ToList();
                }
            }

            var baseentity = htbaseinfobll.GetEntity(hiddenid);
            IList<PostPoneData> result = new List<PostPoneData>();
            string IIMajorRisks = dataitemdetailbll.GetItemValue("IIMajorRisks"); //II级重大隐患
            string IMajorRisks = dataitemdetailbll.GetItemValue("IMajorRisks"); //I级重大隐患
            foreach (HTExtensionEntity entity in list)
            {
                PostPoneData data = new PostPoneData();
                if (null != baseentity)
                {
                    data.workstream = baseentity.WORKSTREAM;
                    if (baseentity.HIDRANK == IMajorRisks || baseentity.HIDRANK == IIMajorRisks)
                    {
                        data.rankname = "重大隐患";
                    }
                    else
                    {
                        data.rankname = "一般隐患";
                    }
                }
                data.handleid = entity.HANDLEID;
                data.hiddenid = entity.HIDID;
                data.problemid = entity.HIDCODE;
                data.postponedays = entity.POSTPONEDAYS;
                data.controlmeasure = entity.CONTROLMEASURE; //临时管控措施
                data.applyreason = entity.POSTPONEOPINION;
                data.postponeresult = entity.POSTPONERESULT;
                data.applydate = null != entity.HANDLEDATE ? entity.HANDLEDATE.Value.ToString("yyyy-MM-dd") : "";
                data.applyperson = entity.HANDLEUSERNAME;
                data.applypersonid = entity.HANDLEUSERID;
                data.applydept = entity.HANDLEDEPTCODE;
                data.applydeptname = entity.HANDLEDEPTNAME;
                if (entity.HANDLETYPE == "0")
                {
                    data.handlestate = "延期申请"; //状态
                    data.applyreason = entity.POSTPONEREASON;
                }
                if (entity.HANDLETYPE == "1")
                {
                    data.handlestate = "延期审批";
                }
                if (entity.HANDLETYPE == "2")
                {
                    data.handlestate = "延期完成";
                }
                if (entity.HANDLETYPE == "-1")
                {
                    data.handlestate = "延期失败";
                }
                result.Add(data);
            }
            if (!string.IsNullOrEmpty(handleid))
            {
                result = result.Where(p => p.handleid == handleid).ToList();
            }
            return new { code = 0, info = "获取数据成功", count = result.Count(), data = result };
        }
        #endregion

        #region 获取隐患曝光列表
        /// <summary>
        /// 获取违章列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ExposeBrokenRulesList([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            //获取前几个隐患曝光
            string num = res.Contains("num") ? dy.data.num.ToString() : "3";

            string type = res.Contains("type") ? dy.data.type.ToString() : "0";

            DataTable dt = new DataTable();

            //隐患曝光
            if (type == "0")
            {
                dt = htbaseinfobll.QueryExposureHid(num);
            }
            else  //违章曝光
            {
                dt = legbll.QueryExposureLllegal(num);
            }

            return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
        }
        #endregion

        #region  隐患图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public void UploadifyFile(string folderId, string foldername, HttpFileCollection fileList)
        {

            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = fileList[i];

                        if (fileList.AllKeys[i].Contains(foldername))
                        {
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                            //创建文件夹
                            string path = Path.GetDirectoryName(fullFileName);
                            Directory.CreateDirectory(path);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            if (!System.IO.File.Exists(fullFileName))
                            {
                                //保存文件
                                file.SaveAs(fullFileName);
                            }
                            //文件信息写入数据库
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            fileInfoEntity.RecId = folderId; //关联ID
                            fileInfoEntity.FolderId = "ht/images";
                            fileInfoEntity.FileName = file.FileName;
                            fileInfoEntity.FilePath = virtualPath;
                            fileInfoEntity.FileSize = filesize.ToString();
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.Replace(".", "");
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 4;
                logEntity.OperateTypeId = ((int)OperationType.Exception).ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
                logEntity.OperateAccount = curUser.UserName;
                logEntity.OperateUserId = curUser.UserId;
                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = ex.Message;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.WriteLog();
            }
        }
        #endregion

        #region 删除图片
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="recId">各图片Id  xxxxx,xxxxxx,xxxxxxxx</param>
        /// <param name="folderId">关联ID</param>
        /// <returns></returns>
        public bool DeleteFile(string recId)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(recId))
            {
                string ids = "";

                string[] strArray = recId.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }

                int count = fileInfoBLL.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        #endregion

        #region 隐患统计

        #region 隐患统计(省级下)
        /// <summary>
        /// 隐患统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getProblemStatisticsList([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            HidStatisticsData entity = new HidStatisticsData();

            IList<ProblemModify> problemmodifylist = new List<ProblemModify>();

            string deptcode = string.Empty;

            //省级用户
            if (curUser.RoleName.Contains("省级用户"))
            {
                deptcode = res.Contains("deptcode") ? dy.data.deptcode.ToString() : "";  //按电厂来
            }
            else if (curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户"))  //当前用户是公司级及厂级用户
            {
                deptcode = curUser.OrganizeCode; //厂级用户
            }
            else
            {
                deptcode = curUser.DeptCode; //普通用户
            }
            try
            {
                //整改隐患数量
                var dt1 = htbaseinfobll.GetAppHidStatistics(deptcode, 0);

                if (dt1.Rows.Count == 1)
                {
                    entity.problemtotalnums = dt1.Rows[0]["allhid"].ToString(); //全部
                    entity.normalproblemnums = dt1.Rows[0]["ordinaryhid"].ToString(); //一般
                    entity.seriousproblemnums = dt1.Rows[0]["importanhid"].ToString(); //重大
                    entity.normalproblemrate = int.Parse(entity.problemtotalnums) == 0 ? "0" : Math.Round((Convert.ToDecimal(entity.normalproblemnums) * 100 / Convert.ToDecimal(entity.problemtotalnums)), 2).ToString();
                    entity.seriousproblemrate = int.Parse(entity.problemtotalnums) == 0 ? "0" : Math.Round((Convert.ToDecimal(entity.seriousproblemnums) * 100 / Convert.ToDecimal(entity.problemtotalnums)), 2).ToString();
                }

                //隐患整改情况
                var dt2 = htbaseinfobll.GetAppHidStatistics(deptcode, 1);
                if (dt2.Rows.Count == 1)
                {
                    ProblemModify data1 = new ProblemModify();
                    data1.seriousproblemrate = "全部隐患";//隐患级别
                    data1.modifynums = dt2.Rows[0]["yzgnum"].ToString();//已整改数
                    data1.unmodifynums = dt2.Rows[0]["wzgnum"].ToString();//未整改数
                    data1.modifyrate = dt2.Rows[0]["allzgl"].ToString();//整改率
                    problemmodifylist.Add(data1);

                    ProblemModify data2 = new ProblemModify();
                    data2.seriousproblemrate = "一般隐患";//隐患级别
                    data2.modifynums = dt2.Rows[0]["ybyzgnum"].ToString(); //已整改数
                    data2.unmodifynums = dt2.Rows[0]["ybwzgnum"].ToString();//未整改数
                    data2.modifyrate = dt2.Rows[0]["ybzgl"].ToString();//整改率
                    problemmodifylist.Add(data2);

                    ProblemModify data3 = new ProblemModify();
                    data3.seriousproblemrate = "重大隐患";//隐患级别
                    data3.modifynums = dt2.Rows[0]["zdyzgnum"].ToString();//已整改数
                    data3.unmodifynums = dt2.Rows[0]["zdwzgnum"].ToString();//未整改数
                    data3.modifyrate = dt2.Rows[0]["zdzgl"].ToString();//整改率
                    problemmodifylist.Add(data3);
                }
                entity.problemmodifylist = problemmodifylist;
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = "获取数据失败", Count = 0 };
            }

            return new { Code = 0, Info = "获取数据成功", Count = 0, data = entity };
        }
        #endregion

        #region 班组终端隐患统计(按创建单位来统计)
        /// <summary>
        /// 班组终端统计(按创建单位来统计)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHtChartForTeam([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string year = res.Contains("year") ? dy.data.year : DateTime.Now.Year.ToString();
                string deptCode = curUser.DeptCode;  //部门
                //隐患等级统计
                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = deptCode;
                hentity.sYear = year;
                hentity.sAction = "15";
                hentity.sOrganize = curUser.OrganizeId;
                //隐患等级统计图
                var rankdt = htbaseinfobll.QueryStatisticsByAction(hentity);
                List<oseries> oslist = new List<oseries>();
                if (rankdt.Rows.Count == 1)
                {
                    //一般隐患
                    oseries ordinary = new oseries();
                    ordinary.name = "一般隐患";
                    ordinary.precent = Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) != 0 ? Math.Round(Convert.ToDecimal(rankdt.Rows[0]["ordinaryhid"].ToString()) / Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) * 100, 2) : 0;
                    ordinary.num = int.Parse(rankdt.Rows[0]["ordinaryhid"].ToString());
                    oslist.Add(ordinary);

                    //重大隐患
                    oseries important = new oseries();
                    important.name = "重大隐患";
                    important.precent = Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) != 0 ? Math.Round(Convert.ToDecimal(rankdt.Rows[0]["importanhid"].ToString()) / Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) * 100, 2) : 0;
                    important.num = int.Parse(rankdt.Rows[0]["importanhid"].ToString());
                    oslist.Add(important);
                }
                //隐患月份趋势统计图
                hentity.sAction = "14";
                var monthdt = htbaseinfobll.QueryStatisticsByAction(hentity);
                List<mseries> list = new List<mseries>();
                foreach (DataRow row in monthdt.Rows)
                {
                    mseries entity = new mseries();
                    entity.name = row["month"].ToString();
                    entity.ybnum = int.Parse(row["ordinaryhid"].ToString());
                    entity.zdnum = int.Parse(row["importanhid"].ToString());
                    list.Add(entity);
                }

                var data = new
                {
                    rankdata = oslist,
                    monthdata = list
                };

                return new { Code = 0, Info = "获取数据成功", Count = 0, data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }
        }
        #endregion

        #region 隐患等级统计图
        /// <summary>
        /// 隐患等级统计图
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHtLevelChart([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string year = dy.data.year;
                string deptCode = curUser.DeptCode;  //部门
                if (string.IsNullOrEmpty(year))
                {
                    year = DateTime.Now.Year.ToString();
                }
                //判断是否是厂级用户或者是公司用户
                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = deptCode;
                hentity.sYear = year;
                hentity.sAction = "2";
                hentity.sOrganize = curUser.OrganizeId;

                //当前用户是厂级
                if (curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                //隐患等级统计图
                var hidrank = htbaseinfobll.QueryStatisticsByAction(hentity);
                return new { Code = 0, Info = "获取数据成功", Count = 0, data = hidrank };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }

        }
        #endregion

        #region 隐患数量变化趋势图
        /// <summary>
        /// 隐患数量变化趋势图
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHtNumChart([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string deptCode = curUser.DeptCode;  //部门
                string year = dy.data.year;
                string level = dy.data.level;
                if (string.IsNullOrEmpty(year))
                {
                    year = DateTime.Now.Year.ToString();
                }
                string hidPoint = "";  //区域 
                string hidRank = string.Empty;
                if (!string.IsNullOrEmpty(level))
                {
                    hidRank = level.Replace("请选择", "");  //隐患级别
                }
                string ischeck = "";
                string checkType = "";
                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = deptCode;
                hentity.sYear = year;
                hentity.sArea = hidPoint;
                hentity.sHidRank = hidRank;
                hentity.sAction = "4";
                hentity.isCheck = ischeck;
                hentity.sCheckType = checkType;

                var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

                IList<series> slist = new List<series>();

                if (!string.IsNullOrEmpty(hidRank))
                {
                    string[] arr = hidRank.Split(',');

                    //单个或多个隐患级别
                    foreach (string aStr in arr)
                    {
                        series s = new series();
                        s.name = aStr;
                        List<int> dlist = new List<int>();
                        foreach (DataRow row in dt.Rows)
                        {
                            int tempValue = 0;
                            if (aStr == "一般隐患")
                            {
                                tempValue = Convert.ToInt32(row["OrdinaryHid"].ToString());
                            }
                            else  //重大隐患
                            {
                                tempValue = Convert.ToInt32(row["ImportanHid"].ToString());
                            }
                            dlist.Add(tempValue);
                        }
                        s.data = dlist;
                        slist.Add(s);
                    }
                }
                else   //无隐患级别条件
                {
                    series s = new series();
                    s.name = "所有隐患";
                    List<int> dlist = new List<int>();
                    foreach (DataRow row in dt.Rows)
                    {
                        int tempValue = Convert.ToInt32(row["allhid"].ToString());

                        dlist.Add(tempValue);
                    }
                    s.data = dlist;
                    slist.Add(s);
                }
                return new { Code = 0, Info = "获取数据成功", Count = 0, data = slist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }
        }
        #endregion

        #region 隐患整改情况数量变化趋势图统计
        /// <summary>
        ///隐患整改情况数量变化趋势图统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHtNumChangeChart([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string deptCode = curUser.DeptCode;  //部门
                string year = dy.data.year;  //年度
                if (string.IsNullOrEmpty(year))
                {
                    year = DateTime.Now.Year.ToString();
                }
                string hidPoint = "";  //区域 
                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = deptCode;
                hentity.sYear = year;
                hentity.sArea = hidPoint;
                hentity.sAction = "7";

                var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

                IList<fseries> slist = new List<fseries>();

                fseries s1 = new fseries();
                s1.name = "所有隐患";
                fseries s2 = new fseries();
                s2.name = "一般隐患";
                fseries s3 = new fseries();
                s3.name = "重大隐患";
                List<decimal> list1 = new List<decimal>();
                List<decimal> list2 = new List<decimal>();
                List<decimal> list3 = new List<decimal>();
                foreach (DataRow row in dt.Rows)
                {
                    decimal total = Convert.ToDecimal(row["aChangeVal"].ToString()); //总的整改率
                    decimal ordinary = Convert.ToDecimal(row["oChangeVal"].ToString()); //一般隐患整改率
                    decimal great = Convert.ToDecimal(row["iChangeVal"].ToString()); //重大隐患整改率

                    list1.Add(total);
                    list2.Add(ordinary);
                    list3.Add(great);
                }
                s1.data = list1;
                s2.data = list2;
                s3.data = list3;
                slist.Add(s1);
                slist.Add(s2);
                slist.Add(s3);
                return new { Code = 0, Info = "获取数据成功", Count = 0, data = slist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }

        }
        #endregion

        #region 隐患整改情况统计
        [HttpPost]
        /// <summary>
        ///隐患整改情况统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public object GetHtNumReadjustChart([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string deptCode = curUser.DeptCode;  //部门
                string level = dy.data.level;  //年度
                string year = dy.data.year;
                string hidRank = "";  //隐患级别
                if (!string.IsNullOrEmpty(level))
                {
                    hidRank = level.Replace("请选择", "");
                }
                if (string.IsNullOrEmpty(year))
                {
                    year = DateTime.Now.Year.ToString();
                }
                string hidPoint = "";  //区域
                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = deptCode; //整改部门
                hentity.sArea = hidPoint; //区域
                hentity.sYear = year; //年度
                hentity.sHidRank = hidRank; //隐患级别

                hentity.sAction = "6";   //对比图分析
                //列表
                var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

                IList<series> slist = new List<series>();
                List<int> oVal = new List<int>();
                List<int> iVal = new List<int>();
                series s1 = new series();
                s1.name = "已整改";
                series s2 = new series();
                s2.name = "未整改";
                //图表分析
                foreach (DataRow row in dt.Rows)
                {
                    //y 轴Value 
                    oVal.Add(Convert.ToInt32(row["yValue"].ToString()));
                    iVal.Add(Convert.ToInt32(row["wValue"].ToString()));
                }
                s1.data = oVal; //已整改
                s2.data = iVal; //未整改
                slist.Add(s1);
                slist.Add(s2);

                var jsonData = new { tdata = dt, sdata = slist };
                return new { Code = 0, Info = "获取数据成功", Count = 0, data = jsonData };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }

        }
        #endregion

        #region 安全检查统计
        /// <summary>
        ///安全检查统计
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        [HttpPost]
        public object GetCheckNumChart([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string year = dy.data.year;//年份
                string ctype = dy.data.checkType;//检查类型
                if (string.IsNullOrEmpty(year))
                {
                    year = DateTime.Now.Year.ToString();
                }
                var data = new SaftyCheckDataRecordBLL().GetSaftyList(curUser.DeptCode, year, ctype);
                return new { Code = 0, Info = "获取数据成功", Count = 0, data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }


        }
        #endregion

        #region 隐患台账
        /// <summary>
        /// 隐患台账
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpPost]
        public object GetHtList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string userId = curUser.UserId;

                string startTime = dy.data.startTime;//开始时间
                string endTime = dy.data.endTime;//结束时间
                string htlevel = dy.data.level;//级别
                string status = dy.data.status;//隐患状态
                string flowState = dy.data.flowState;//隐患流程状态
                string checkType = dy.data.checkType;//检查类型
                string htType = dy.data.htType;//隐患类型
                string desc = dy.data.describe;//隐患描述
                string isSee = dy.data.isSee;//是否曝光
                string mode = res.Contains("mode") ? dy.data.mode : ""; //标记 
                string qdeptcode = res.Contains("qdeptcode") ? dy.data.qdeptcode : ""; //部门编码
                string queryJson = new
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    HidRank = htlevel,
                    ChangeStatus = status,
                    WorkStream = flowState,
                    SaftyCheckType = checkType,
                    HidType = htType,
                    HidDescribe = desc,
                    IsExposureState = isSee,
                    mode = mode,
                    qdeptcode = qdeptcode
                }.ToJson();
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());

                queryJson = queryJson.Insert(1, "\"userId\":\"" + userId + "\",");
                queryJson = queryJson.Insert(1, "\"isPlanLevel\":\"" + curUser.isPlanLevel + "\","); //添加当前是否公司及厂级
                var data = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson);
                return new { Code = 0, Info = "获取数据成功", Count = pagination.records, data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }
        }
        #endregion

        #region 检查列表
        [HttpPost]
        /// <summary>
        /// 检查列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   

        public object getCheckList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (null == user)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CreateDate,CheckBeginTime,CheckMan,createuserid,createuserdeptcode,createuserorgcode,CheckDataRecordName,t.SolvePerson";

                pagination.conditionJson = "datatype in(0,2)";
                string where1 = "";
                string arg = "";
                string queryJson = "";
                string ctype = dy.data.ctype;
                string title = dy.data.title;
                string startTime = dy.data.startTime;
                string endTime = dy.data.endTime;
                if (ctype == "1")
                {
                    queryJson = new { ctype = ctype, st = startTime, et = endTime }.ToJson();
                    if (!user.IsSystem)
                    {
                        if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                        {
                            arg = user.OrganizeCode;
                            pagination.conditionJson += string.Format(" and (t1.id is not null or createuserdeptcode like '{0}%')", arg);
                        }
                        else
                        {
                            arg = user.OrganizeCode;
                            where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                            pagination.conditionJson += string.Format("  and (t1.id is not null or createuserdeptcode='{0}')", arg);
                        }
                        pagination.p_tablename = string.Format(@"bis_saftycheckdatarecord t left join(select id from (select id,(','||checkmanid||',') as checkmanid  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode like '{0}%' {1} )
) b on a.checkmanid  like '%'||b.account||'%' where account is not null group by id) t1
on t.ID=t1.id", arg, where1);
                    }
                    else
                    {
                        pagination.p_tablename = "bis_saftycheckdatarecord t";
                    }
                }
                else
                {
                    pagination.p_fields = "CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,SolveCount,createuserid,createuserdeptcode,createuserorgcode,t.SolvePerson";
                    queryJson = new { ctype = ctype, stm = startTime, etm = endTime }.ToJson();
                    if (!user.IsSystem)
                    {
                        if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                        {
                            arg = user.OrganizeCode;
                            pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode like '{0}%')", arg);
                        }
                        else
                        {
                            arg = user.DeptCode;
                            where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                            pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode='{0}')", arg);
                        }
                        pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManAccount||',') as CheckManAccount,recid  from  BIS_SAFTYCONTENT) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode like '{0}%' {1} )
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid ", arg, where1);
                    }
                    else
                    {
                        pagination.p_tablename = "bis_saftycheckdatarecord t";
                    }

                }
                SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
                var data = srbll.GetPageList(pagination, queryJson).ToList();
                return new { Code = 0, Info = "获取数据成功", Count = data.Count, data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }

        }
        #endregion



        #endregion

        #region 省级公司首页-安全预警
        /// <summary>
        /// 省级公司首页-安全预警
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getSafetyWarningList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            int type = res.Contains("type") ? int.Parse(dy.data.type.ToString()) : 0; //统计类型

            DataTable dt = htbaseinfobll.GetHidSafetyWarning(type, curUser.OrganizeCode); //统计

            List<hidsafetywarning> list = new List<hidsafetywarning>();

            foreach (DataRow row in dt.Rows)
            {
                hidsafetywarning entity = new hidsafetywarning();
                entity.deptcode = row["deptcode"].ToString();
                entity.deptname = row["deptname"].ToString();
                entity.organizeid = row["organizeid"].ToString();
                entity.total = row["total"].ToString();
                list.Add(entity);
            }
            return new { Code = 0, Info = "获取数据成功", Count = list.Count(), data = list };
        }
        #endregion

        #region 意见反馈
        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddOpinion()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                OpinionEntity entity = new OpinionEntity();
                entity.OpinionPersonID = curUser.UserId; //评估人id
                entity.OpinionPersonName = curUser.UserName; //评估人姓名
                entity.OpinionDeptCode = curUser.DeptCode; //部门Code
                entity.OpinionDeptName = curUser.DeptName;//部门名称
                entity.OpinionDate = DateTime.Now; //当前时间
                string opinionContent = res.Contains("opinioncontent") ? dy.data.opinioncontent : "";
                string opinionPhoto = Guid.NewGuid().ToString();
                entity.OpinionContent = opinionContent;
                entity.OpinionPhoto = opinionPhoto;
                //上传隐患图片
                UploadifyFile(opinionPhoto, "opinionimg", files);
                opinionbll.SaveForm("", entity);

                return new { code = 0, count = 0, info = "提交成功!" };
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "提交失败!" };
            }
        }
        #endregion

        #region  保存整改计划
        /// <summary>
        /// 保存整改计划
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveChangePlanDetail()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {
                string keyValue = res.Contains("changeplanid") ? dy.data.changeplanid : string.Empty; //主键  
                string hiddenid = res.Contains("hiddenid") ? dy.data.hiddenid : string.Empty; //隐患  
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string remark = res.Contains("changeplaninfo") ? dy.data.changeplaninfo : string.Empty; //整改计划信息
                string fileids = res.Contains("fileids") ? dy.data.fileids : string.Empty;
                ChangePlanDetailEntity entity = new ChangePlanDetailEntity();
                entity.HIDDENID = hiddenid;
                //如果hiddenid 不为空,则是编辑状态
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity = htbaseinfobll.GetChangePlanEntity(keyValue);
                }
                entity.REMARK = remark;
                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(keyValue))
                {
                    entity.ATTACHMENT = Guid.NewGuid().ToString();
                }
                //先删除图片
                DeleteFile(fileids);
                //上传附件
                entity.ATTACHMENT = !string.IsNullOrEmpty(entity.ATTACHMENT) ? entity.ATTACHMENT : Guid.NewGuid().ToString();
                UploadifyFile(entity.ATTACHMENT, "changeplan", files);
                /********************************/
                //新增
                htbaseinfobll.SaveChangePlan(keyValue, entity);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        /*********************违章流程*************************/

        #region 基础信息

        #region 违章类型
        /// <summary>
        /// 违章类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalType([FromBody]JObject json)
        {

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'LllegalType'"); //违章类型

            string applianceclass = dataitemdetailbll.GetItemValue("ApplianceClass"); //装置类对象 

            return new { code = 0, info = "获取数据成功", count = 0, data = new { itemdata = itemlist.Select(x => new { lllegaltypeid = x.ItemDetailId, lllegaltypename = x.ItemName }), applianceclass = applianceclass } };
        }
        #endregion

        #region 获取违章控制权限
        /// <summary>
        /// 获取违章控制权限
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalAuth([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid.ToString() : "";//违章id

            string lllegaltypeid = res.Contains("lllegaltypeid") ? dy.data.lllegaltypeid.ToString() : "";//违章类型id

            string majorclassify = res.Contains("majorclassify") ? dy.data.majorclassify.ToString() : ""; //隐患专业分类

            string lllegalteamcode = res.Contains("lllegalteamcode") ? dy.data.lllegalteamcode.ToString() : ""; //违章单位编码

            string lllegallevel = res.Contains("lllegallevel") ? dy.data.lllegallevel.ToString() : ""; //违章级别

            string flowstate = res.Contains("flowstate") ? dy.data.flowstate.ToString() : ""; //违章状态

            LllegalAuthData model = new LllegalAuthData();

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'FlowState','ControlPicMustUpload','LllegalAwardDetailAuth'");
            try
            {
                string mark = string.Empty;
                if (curUser.RoleName.Contains("省级用户"))
                {
                    mark = "省级违章流程";
                }
                else
                {
                    mark = "厂级违章流程";
                }
                bool ispromptlychange = false;
                if (curUser.RoleName.Contains("安全管理员") || curUser.RoleName.Contains("负责人") || curUser.RoleName.Contains("专工")) { ispromptlychange = true; }
                string approveStatus = itemlist.Where(p=>p.EnCode== "FlowState").Where(p => p.ItemName == "违章核准").Count() > 0 ? "违章核准" : "违章审核";
                model.ispromptlychange = ispromptlychange;//GetCurUserWfAuth(null, "违章核准", "", mark, "提交", majorclassify, lllegaltypeid, curUser.DeptId, lllegalid);  //是否能立即整改
                model.ishaveupsubmit = GetCurUserWfAuth(null, flowstate, approveStatus, mark, "上报", majorclassify, lllegaltypeid, curUser.DeptId, lllegalteamcode, lllegallevel, lllegalid); //判定是否存在上报功能权限
                model.isshowappoint = GetCurUserWfAuth(null, flowstate, "制定整改计划", mark, "制定提交", majorclassify, lllegaltypeid, curUser.DeptId, lllegalteamcode, lllegallevel, lllegalid); //是否具有指定
                model.iscanchange = GetCurUserWfAuth(null, flowstate, "违章整改", mark, "提交", majorclassify, lllegaltypeid, curUser.DeptId, lllegalteamcode, lllegallevel, lllegalid); //判定是否能直接到整改结束
                model.ismustsubmit = GetCurUserWfAuth(null, flowstate, approveStatus, mark, "限制上报", majorclassify, lllegaltypeid, curUser.DeptId, lllegalteamcode, lllegallevel, lllegalid); //判定是否具有限制上报，即装置性违章下，当前人必须上报的权限
                model.ishavetjsubmit = GetCurUserWfAuth(null, flowstate, approveStatus, mark, "同级提交", majorclassify, lllegaltypeid, curUser.DeptId, lllegalteamcode, lllegallevel, lllegalid); //是否具有同级提交权限，一般针对本部门专工提交非本专业数据到本部门对应专业的专工
                model.isendflow = GetCurUserWfAuth(null, flowstate, "流程结束", mark, "提交", majorclassify, lllegaltypeid, curUser.DeptId, lllegalteamcode, lllegallevel, lllegalid); //是否具有结束流程的权限
                model.ishrdl = dataitemdetailbll.GetItemValue("IsOpenPassword") == "true"; //是否华润电力
                string ControlPicMustUpload = string.Empty;
                var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == curUser.OrganizeId);
                if (cpmu.Count() > 0)
                {
                    ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
                }
                model.wzpicmustupload = ControlPicMustUpload.Contains("LLLEGALPIC");
                model.zgpicmustupload = ControlPicMustUpload.Contains("REFORMPIC");
                model.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPIC");
                //是否能操作违章奖励
                var awardauth = itemlist.Where(p => p.EnCode == "LllegalAwardDetailAuth").Where(p => p.ItemName == curUser.OrganizeId);
                model.isgetaward = awardauth.Count() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = model };
        }
        #endregion

        #region 获取专业主管
        /// <summary>
        /// 获取专业主管
        /// </summary>
        /// <param name="reformdeptcode"></param>
        /// <param name="majorclassify"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetKmSpecialPerson([FromBody]JObject json)
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string reformdeptcode = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";
            string majorclassify = res.Contains("majorclassify") ? dy.data.majorclassify : "";
            UserInfoEntity userinfo = null;
            try
            {
                //专业不为空，获取专业主管
                if (!string.IsNullOrEmpty(reformdeptcode) && !string.IsNullOrEmpty(majorclassify))
                {
                    List<UserInfoEntity> ulist = userbll.GetUserListByCodeAndRole(reformdeptcode, "").ToList();
                    List<UserInfoEntity> lastlist = new List<UserInfoEntity>();
                    string km_major_role = dataitemdetailbll.GetItemValue("KM_MAJOR_ROLE"); //可门配置 
                    var mcItem = dataitemdetailbll.GetDataItemListByItemCode("'HidMajorClassify'").Where(p => p.ItemDetailId == majorclassify).FirstOrDefault();
                    var templist = ulist.Where(p => p.RoleName.Contains(km_major_role)).ToList();
                    string SpecialtyType = "," + mcItem.ItemValue + ",";
                    foreach (UserInfoEntity entity in templist)
                    {
                        string sptype = !string.IsNullOrEmpty(entity.SpecialtyType) ? "," + entity.SpecialtyType + "," : "";
                        if (sptype.Contains(SpecialtyType))
                        {
                            lastlist.Add(entity);
                        }
                    }
                    if (lastlist.Count() > 0)
                    {
                        userinfo = lastlist.FirstOrDefault();
                    }
                    if (null == userinfo)
                    {
                        return new { code = 0, info = "获取数据成功", count = 0, data = new { } };
                    }
                    return new { code = 0, info = "获取数据成功", count = 0, data = userinfo };
                }
                else
                {
                    return new { code = 0, info = "获取数据成功", count = 0, data = new { } };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
        }
        #endregion

        #region 违章级别
        /// <summary>
        /// 违章级别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalLevel()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'LllegalLevel'"); //违章级别

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { lllegallevelid = x.ItemDetailId, lllegallevelname = x.ItemName }) };
        }
        #endregion

        #region 违章状态
        /// <summary>
        /// 违章状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalStatus()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'LllegalStatus'");

            return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { id = x.ItemName, name = x.ItemName }) };
        }
        #endregion

        #region 违章流程状态
        /// <summary>
        /// 违章流程状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalFlowState()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'FlowState'"); //违章流程状态

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { flowstateid = x.ItemName, flowstatename = x.ItemName }) };
        }
        #endregion

        #region 违章考核对象
        /// <summary>
        /// 违章考核对象
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalExamObject()
        {

            var itemlist = new Dictionary<string, string>(); //违章流程状态
            itemlist.Add("考核人员", "考核人员");
            itemlist.Add("考核部门", "考核部门");
            itemlist.Add("第一联责人员", "第一联责人员");
            itemlist.Add("第一联责部门", "第一联责部门");
            itemlist.Add("第二联责人员", "第二联责人员");
            itemlist.Add("第二联责部门", "第二联责部门");
            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { assessobject = x.Key, assessobjectname = x.Value }) };
        }
        #endregion

        #region 获取违章标准
        /// <summary>
        /// 获取违章标准
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalStandard([FromBody]JObject json)
        {
            //获取违章标准
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {
                int pageSize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 10; //每页的记录数
                int pageIndex = res.Contains("pageindex") ? int.Parse(dy.pageindex.ToString()) : 1; //当前页索引

                string lllegaltype = res.Contains("lllegaltype") ? dy.data.lllegaltype.ToString() : string.Empty;
                string lllegallevel = res.Contains("lllegallevel") ? dy.data.lllegallevel.ToString() : string.Empty;
                string lllegaldescribe = res.Contains("lllegaldescribe") ? dy.data.lllegaldescribe.ToString() : string.Empty;

                Pagination pagination = new Pagination();

                pagination.page = pageIndex;
                pagination.rows = pageSize;
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,des,leglevel,legLevalName,legtype,legTypeName,bustype,busTypeName,descore,demoney,firstdescore,firstdemoney,seconddescore,seconddemoney,remark";
                pagination.p_kid = "id";
                pagination.p_tablename = @"v_lllegalstdinfo";
                pagination.conditionJson = " 1=1";
                pagination.conditionJson += string.Format(@" and createuserorgcode like '{0}%'", curUser.OrganizeCode);

                //违章类型
                if (!string.IsNullOrEmpty(lllegaltype))
                {
                    pagination.conditionJson += string.Format(@" and  legtype='{0}' ", lllegaltype.ToString());
                }
                //违章级别
                if (!string.IsNullOrEmpty(lllegallevel))
                {
                    pagination.conditionJson += string.Format(@" and leglevel ='{0}'", lllegallevel.ToString());
                }
                //违章描述 
                if (!string.IsNullOrEmpty(lllegaldescribe))
                {
                    pagination.conditionJson += string.Format(@" and des like '%{0}%'", lllegaldescribe.ToString());
                }
                var data = lllegalstandardbll.GetLllegalStdInfo(pagination, string.Empty);

                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 已有违章描述
        /// <summary>
        /// 已有违章描述
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalDescribe([FromBody]JObject json)
        {
            //获取自己的新增违章
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string lllegaldescribe = res.Contains("lllegaldescribe") ? dy.data.lllegaldescribe : "";// 违章描述

            var dt = lllegalregisterbll.GetLllegalDescribeList(curUser.UserId, lllegaldescribe); //违章描述内容

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 0,
                data = dt.Select().Select(x => new
                {
                    lllegaldescribe = x.Field<string>("lllegaldescribe"),
                    lllegaltype = x.Field<string>("lllegaltype"),
                    lllegaltypename = x.Field<string>("lllegaltypename"),
                    lllegallevel = x.Field<string>("lllegallevel"),
                    lllegallevelname = x.Field<string>("lllegallevelname")
                })
            };
        }
        #endregion

        #region 获取所有违章列表接口
        /// <summary>
        /// 获取所有违章列表接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string tokenId = res.Contains("tokenid") ? dy.tokenid.ToString() : ""; //设备唯一标识
            int pageSize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 10; //每页的记录数
            int pageIndex = res.Contains("pageindex") ? int.Parse(dy.pageindex.ToString()) : 1; //当前页索引

            string action = res.Contains("action") ? dy.data.action.ToString() : ""; //请求类型
            string lllegallevel = res.Contains("lllegallevel") ? dy.data.lllegallevel : ""; //违章级别
            string lllegaltype = res.Contains("lllegaltype") ? dy.data.lllegaltype : ""; //违章类型
            string reseverone = res.Contains("reseverone") ? dy.data.reseverone : ""; //应用id
            string resevertwo = res.Contains("resevertwo") ? dy.data.resevertwo : ""; //应用id
            string reseverthree = res.Contains("reseverthree") ? dy.data.reseverthree : ""; //应用id 
            string flowstate = res.Contains("flowstate") ? dy.data.flowstate : ""; //流程状态
            string year = res.Contains("bzspecialyear") ? dy.data.bzspecialyear : "";
            string month = res.Contains("bzspecialmonth") ? dy.data.bzspecialmonth : "";
            string day = res.Contains("bzspecialday") ? dy.data.bzspecialday : "";
            string qdeptcode = res.Contains("qdeptcode") ? dy.data.qdeptcode : "";//电厂code
            string currdeptcode = res.Contains("currdeptcode") ? dy.data.currdeptcode : "";//违章部门
            string currdate = string.Empty;
            if (!string.IsNullOrWhiteSpace(year) && !string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(day))
            {
                currdate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day)).ToString("yyyy-MM-dd");
            }
            string lllegalstatus = res.Contains("lllegalstatus") ? dy.data.lllegalstatus : ""; //违章状态
            string createdeptcode = res.Contains("createdeptcode") ? dy.data.createdeptcode : ""; //登记单位
            string lllegalstartdate = res.Contains("lllegalstartdate") ? dy.data.lllegalstartdate : ""; //违章时间
            string lllegalenddate = res.Contains("lllegalenddate") ? dy.data.lllegalenddate : ""; //违章时间
            string reformdept = res.Contains("reformdept") ? dy.data.reformdept : ""; //整改单位 --部门编码
            string belongcode = res.Contains("belongcode") ? dy.data.belongcode : "";//省公司查看各电厂的数据
            string regcode = res.Contains("regcode") ? dy.data.regcode : "";//省公司查看登记单位（1：本单位登记，2：电厂登记）
            string standingtype = res.Contains("standingtype") ? dy.data.standingtype : "";  //台账类型
            string standingmark = res.Contains("standingmark") ? dy.data.standingmark : "";  //台账标记
            string specialmark = res.Contains("specialmark") ? dy.data.specialmark : "";  //西塞山台账标记

            string tablename = string.Empty;

            tablename = string.Format(@"( 
                                            select a.belongdepart,a.belongdepartid,a.createuserdeptcode,a.createuserorgcode,a.modifydate, a.createuserid, a.id,a.createdate, a.lllegalnumber,
                                            a.lllegaltype,a.lllegaltypename ,a.lllegaltime,a.lllegallevel,a.lllegallevelname, a.lllegalperson,a.lllegalpersonid,a.lllegalteam,a.lllegalteamcode,
                                            a.lllegaldepart,a.lllegaldepartcode,a.lllegaldescribe,a.lllegaladdress ,a.lllegalpic,a.reformrequire,a.flowstate,a.createusername ,a.addtype,a.isexposure,
                                            a.reformpeople,a.reformpeopleid,a.reformtel,a.reformdeptcode,a.reformdeptname,a.reformdeadline,a.reformfinishdate,a.reformstatus,a.reformmeasure,a.reformchargeperson,
                                            a.reformchargepersonname,a.reformchargedeptid,a.reformchargedeptname,a.isappoint,a.applicationstatus,a.postponedays,a.postponedept,a.postponedeptname,a.postponeperson,
                                            a.postponepersonname,a.isgrpaccept,a.acceptpeopleid,a.acceptpeople,a.acceptdeptname,a.acceptdeptcode,a.acceptresult,a.acceptmind,a.accepttime ,a.reseverid,a.resevertype,
                                            a.reseverone,a.resevertwo,a.reseverthree ,a.participant ,f.filepath,c.actionpersonname, (case when a.flowstate ='流程结束' then 1 else 0 end)  ordernumber,
                                            ( case when  a.flowstate ='违章审核' then  '审核中' when  a.flowstate ='违章核准' then  '核准中' when  a.flowstate ='违章完善' then '完善中'   when  a.flowstate ='制定整改计划' then '制定整改计划中'  when
                                            a.flowstate ='违章整改' then '整改中' when  a.flowstate ='违章验收' then '验收中'  when  a.flowstate ='验收确认' then '验收确认中' when a.flowstate ='流程结束' then '流程结束' else a.flowstate end ) actionstatus ,a.rolename,g.handleid, 
                                            a.curapprovedate,a.curacceptdate,a.beforeapprovedate,a.beforeacceptdate,a.afterapprovedate,a.afteracceptdate ,a.verifydeptid,a.verifydeptname  from v_lllegalallbaseinfo a
                                                left join v_imageview b on a.lllegalpic = b.recid  
                                                left join (  select a.id,a.participant, (select listagg(b.realname,',') within group(order by b.account) from base_user b
                                                 where instr(','|| substr(a.participant,2,length(a.participant)-1) ||',',','||b.account||',')>0) actionpersonname from v_lllegalworkflow a  ) c on a.id = c.id
                                            left join  (
                                                    select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_lllegalregister a
                                                    left join base_fileinfo b on a.lllegalpic = b.recid  group by a. id
                                                  ) f on  a.id = f.id   
                                                left join v_lllegalextension  g on a.id = g.lllegalid
                                        ) a ", dataitemdetailbll.GetItemValue("imgUrl"));

            Pagination pagination = new Pagination();
            pagination.p_tablename = tablename;
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "ordernumber asc ,createdate desc,modifydate desc";
            pagination.sord = "";
            pagination.conditionJson = " 1=1 ";
            pagination.p_fields = @"belongdepartid,belongdepart,createuserdeptcode,createuserorgcode,createuserid, createdate,lllegalnumber,lllegaltype, lllegaltypename ,lllegaltime,lllegallevel,
                                    lllegallevelname, lllegalperson,lllegalpersonid,lllegalteam,lllegalteamcode,lllegaldepart,lllegaldepartcode,lllegaldescribe,lllegaladdress ,lllegalpic,
                                    reformrequire,flowstate,createusername ,addtype,isexposure,reformpeople,reformpeopleid,reformtel,reformdeptcode,reformdeptname,reformdeadline,reformfinishdate,
                                    reformstatus,reformmeasure,isgrpaccept,acceptpeopleid,acceptpeople,acceptdeptname,acceptdeptcode,acceptresult,acceptmind,accepttime,reseverid,resevertype,reseverone,
                                    resevertwo,reseverthree ,participant,filepath,actionpersonname,actionstatus,handleid, curapprovedate,curacceptdate,beforeapprovedate,beforeacceptdate,afterapprovedate,
                                    afteracceptdate,verifydeptid,verifydeptname";

            pagination.p_kid = "id";

            //台账标记
            if (!string.IsNullOrEmpty(standingmark))
            {
                pagination.conditionJson += @" and flowstate != '违章登记' and flowstate != '违章举报' ";
            }
            //台账类型
            if (!string.IsNullOrEmpty(standingtype))
            {
                //pagination.conditionJson += @" and flowstate != '违章核准'";

                if (standingtype.Contains("公司级"))
                {
                    pagination.conditionJson += @" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%') ";
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and rolename  like  '%{0}%'  and  rolename not like '%厂级%' ", standingtype);
                }
            }

            #region 隐患状态
            if (!string.IsNullOrEmpty(lllegalstatus))
            {
                switch (lllegalstatus)
                {
                    case "制定整改计划":
                        pagination.conditionJson += @" and flowstate = '制定整改计划' ";
                        break;
                    case "未整改":
                        pagination.conditionJson += @" and flowstate = '违章整改' ";
                        break;
                    case "逾期未审核":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章审核'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);
                        break;
                    case "即将到期未审核":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章审核'  and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate ", DateTime.Now);
                        break;
                    case "逾期未核准":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章核准'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);
                        break;
                    case "即将到期未核准":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章核准'  and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate ", DateTime.Now);
                        break;
                    case "逾期未整改":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > reformdeadline + 1", DateTime.Now);
                        break;
                    case "即将到期未整改":
                        pagination.conditionJson += @"and flowstate = '违章整改' and (reformdeadline - 3 <= sysdate  and sysdate <= reformdeadline + 1)";
                        break;
                    case "逾期未验收":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afteracceptdate", DateTime.Now);
                        break;
                    case "即将到期未验收":
                        pagination.conditionJson += string.Format(@" and flowstate = '违章验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeacceptdate   and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afteracceptdate  ", DateTime.Now);
                        break;
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", curUser.UserId);
                        break;
                    case "已整改":
                        pagination.conditionJson += @" and  flowstate in ('违章验收','验收确认','流程结束')";
                        break;
                    case "未闭环":
                        pagination.conditionJson += @" and  flowstate in ('违章整改','违章验收','验收确认')";
                        break;
                    case "已闭环":
                        pagination.conditionJson += @" and  flowstate  = '流程结束'";
                        break;
                }
            }
            #endregion
            //安全管理部门查看（通过配置选项）  所有   //各部门查看自己的     
            string deptcode = curUser.DeptCode;
            if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("集团") || curUser.RoleName.Contains("省级用户"))
            {
                deptcode = curUser.OrganizeCode;
            }

            if (curUser.RoleName.Contains("集团") || curUser.RoleName.Contains("省级用户"))
            {//省公司查看电厂数据
                pagination.conditionJson += string.Format(@" and createuserorgcode  in (select encode from base_department start with encode='{0}' connect by  prior departmentid=parentid)", curUser.OrganizeCode);
            }
            else
            {//电厂可查看省公司登记的违章。
                pagination.conditionJson += string.Format(@" and belongdepartid='{0}'", curUser.OrganizeId);
            }
            //违章时间
            if (!string.IsNullOrEmpty(lllegalstartdate))
            {
                var starttime = Convert.ToDateTime(lllegalstartdate).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(@" and  lllegaltime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  ", starttime);
            }
            //违章时间
            if (!string.IsNullOrEmpty(lllegalenddate))
            {
                var endtime = Convert.ToDateTime(lllegalenddate).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(@" and   lllegaltime  <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  ", endtime);
            }
            //登记时间
            if (!string.IsNullOrEmpty(currdate))
            {
                pagination.conditionJson += string.Format(@" and  to_char(createdate,'yyyy-MM-dd') = '{0}' ", currdate);
            }
            //登记单位
            if (!string.IsNullOrEmpty(createdeptcode))
            {
                pagination.conditionJson += string.Format(@" and  createuserdeptcode ='{0}' ", createdeptcode);
            }
            //所属部门
            if (!string.IsNullOrWhiteSpace(currdeptcode))
            {
                pagination.conditionJson += string.Format(@" and  lllegalteamcode like'{0}%'", currdeptcode);
            }
            //违章级别
            if (!string.IsNullOrEmpty(lllegallevel))
            {
                pagination.conditionJson += string.Format(@" and  lllegallevel ='{0}' ", lllegallevel);
            }
            //违章类型
            if (!string.IsNullOrEmpty(lllegaltype))
            {
                pagination.conditionJson += string.Format(@" and  lllegaltype ='{0}' ", lllegaltype);
            }
            //应用id
            if (!string.IsNullOrEmpty(reseverone))
            {
                pagination.conditionJson += string.Format(@" and  reseverone ='{0}' ", reseverone);
            }
            //应用id
            if (!string.IsNullOrEmpty(resevertwo))
            {
                pagination.conditionJson += string.Format(@" and  resevertwo ='{0}' ", resevertwo);
            }
            //应用id
            if (!string.IsNullOrEmpty(reseverthree))
            {
                pagination.conditionJson += string.Format(@" and  reseverthree ='{0}' ", reseverthree);
            }
            //流程状态
            if (!string.IsNullOrEmpty(flowstate))
            {
                pagination.conditionJson += string.Format(@" and  flowstate ='{0}' ", flowstate);
            }
            //整改单位编码
            if (!string.IsNullOrEmpty(reformdept))
            {
                pagination.conditionJson += string.Format(@" and  reformdeptcode ='{0}' ", reformdept);
            }
            switch (action)
            {
                //未上传违章
                case "1":
                    pagination.conditionJson += string.Format(@" and  flowstate  in  ('违章登记','违章举报')  and  createuserid ='{0}'", curUser.UserId);
                    break;
                case "1x"://本人完善
                    pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '违章完善'", curUser.Account + ',');
                    break;
                //已上传违章
                case "2":
                    pagination.conditionJson += string.Format(@" and  flowstate  in  (select itemname from v_sywzstatus where  itemname != '违章登记' and itemname != '违章举报')  and  createuserid ='{0}' ", curUser.UserId);
                    break;
                //个人处理-待核准列表
                case "3":
                    pagination.conditionJson += string.Format(@" and (','|| substr(participant,2) ||',')  like   '%,{0},%' and flowstate in ('违章核准','违章审核')", curUser.Account);
                    break;
                //个人处理-待制定整改计划列表
                case "12":
                    pagination.conditionJson += string.Format(@" and (','|| substr(participant,2) ||',')  like   '%,{0},%' and flowstate  = '制定整改计划'  ", curUser.Account);
                    break;
                //个人处理-待整改列表
                case "4":
                    pagination.conditionJson += string.Format(@" and reformpeopleid like '%{0}%' and flowstate  = '违章整改'  ", curUser.UserId);
                    break;
                //个人处理-待延期审批列表
                case "13":
                    pagination.conditionJson += string.Format(@" and  (applicationstatus ='1' and postponeperson  like  '%,{0},%')  ", curUser.Account);
                    break;
                //个人处理-待验收列表
                case "5":
                    pagination.conditionJson += string.Format(@" and acceptpeopleid  like  '%{0}%' and flowstate  = '违章验收'", curUser.UserId);
                    break;
                case "5x"://本人确认
                    pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '验收确认'", curUser.Account + ',');
                    break;
                //个人处理-逾期未整改列表
                case "6":
                    pagination.conditionJson += string.Format(@" and  reformpeopleid like '%{0}%' and flowstate  = '违章整改'  and  to_date('{1}','yyyy-mm-dd hh24:mi:ss') > (reformdeadline + 1)", curUser.UserId, DateTime.Now);
                    break;
                //违章曝光
                case "7":
                    //当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  lllegalteamcode like '{0}%' ", deptcode);

                    pagination.conditionJson += @" and isexposure = '是'";
                    break;
                //所有确定的违章
                case "8":
                    //当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  lllegalteamcode like '{0}%' ", deptcode);

                    pagination.conditionJson += @"  and flowstate in (select itemname from v_yesqrwzstatus)";
                    break;
                //逾期未整改列表
                case "9":
                    //当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  lllegalteamcode like '{0}%' ", deptcode);

                    pagination.conditionJson += string.Format(@"  and flowstate  = '违章整改'  and 
                         to_date('{0}','yyyy-mm-dd hh24:mi:ss') > (reformdeadline + 1)", DateTime.Now);
                    break;
                //已闭环的违章
                case "10":
                    //违章单位为当前部门及下属单位或创建单位为当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  (lllegalteamcode like '{0}%' or createuserdeptcode like '{0}%' or reformdeptcode='{0}') ", deptcode);
                    pagination.conditionJson += @"  and flowstate ='流程结束' ";
                    break;
                //对应电厂进来(省级首页指标)
                case "11":
                    pagination.conditionJson += string.Format(@"  and belongdepartid = (select departmentid from base_department where encode ='{0}')  and to_char(createdate,'yyyy')='{1}'", qdeptcode, DateTime.Now.Year);

                    pagination.conditionJson += @"  and flowstate in (select itemname from v_yesqrwzstatus)";
                    break;
                //按部门整改列表,违章整改确认
                case "14":
                    if (curUser.RoleName.Contains("安全管理员"))
                    {
                        pagination.conditionJson += string.Format(@" and reformpeopleid not like '%{0}%' and   reformdeptcode  =  '{1}' and flowstate  = '违章整改'  ", curUser.UserId, curUser.DeptCode);
                    }
                    else
                    {
                        pagination.conditionJson += " and 1!=1";
                    }
                    break;
            }
            if (!string.IsNullOrEmpty(belongcode))
            {//各电厂的反违章
                pagination.conditionJson += string.Format(@" and  lllegalteamcode like '{0}%' ", belongcode);
            }
            if (!string.IsNullOrWhiteSpace(regcode))
            {//省公司或电厂登记的反违章0
                if (regcode == "1")//省公司登记
                {
                    pagination.conditionJson += " and isgrpaccept is not null";
                }

                else if (regcode == "2")//电厂登记
                {
                    pagination.conditionJson += " and isgrpaccept is null";
                }
            }

            //西塞山台账标记
            if (!string.IsNullOrEmpty(specialmark))
            {
                string idsql = string.Format(@"  or  id  in  (select distinct  objectid from v_xsslllegalstandingbook where  encode ='{0}' )", curUser.DeptCode);

                //厂级、公司层级
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
                {
                    pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  {0}) ", idsql);
                }
                //部门层级
                else if (curUser.RoleName.Contains("部门级") && !curUser.RoleName.Contains("厂级"))
                {
                    pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode  ='{0}' {1})  ", curUser.DeptCode, idsql);
                }
                //班组层级
                else if (curUser.RoleName.Contains("班组级"))
                {
                    pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode in 
                        (select encode from base_department   where nature = '部门' start with encode='{0}' connect by  prior parentid  = departmentid)  or  createuserdeptcode  ='{0}' {1})  ", curUser.DeptCode, idsql);
                }
                //承包商层级
                else if (curUser.RoleName.Contains("承包商"))
                {
                    //当前用户外包工程不为空时
                    if (!string.IsNullOrEmpty(curUser.ProjectID))
                    {
                        pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode in 
                             (select a.encode from base_department a, epg_outsouringengineer  b   where a.nature = '部门'  and a.departmentid = b.engineerusedeptid and b.id ='{0}')  or  createuserdeptcode  ='{1}'  {2})", curUser.ProjectID, curUser.DeptCode, idsql);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and  (rolename  like  '%公司级%' or   rolename  like  '%厂级部门用户%'  or  createuserdeptcode in 
                             (select a.encode from base_department a, epg_outsouringengineer  b   where a.nature = '部门'  and a.departmentid = b.engineerusedeptid and b.outprojectid ='{0}'  and b.createuserorgcode='{1}')  or  createuserdeptcode  ='{2}'  {3})", curUser.ProjectID, curUser.OrganizeCode, curUser.DeptCode, idsql);
                    }
                }
            }
            var dt = htbaseinfobll.GetBaseInfoForApp(pagination);

            return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
        }
        #endregion

        #region 获取核准历史列表
        /// <summary>
        /// 获取违章历史列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetApproveList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : ""; //违章id

            List<ApproveHistoryModel> data = new List<ApproveHistoryModel>(); //返回的结果集合

            if (!string.IsNullOrEmpty(lllegalid))
            {
                List<LllegalApproveEntity> list = lllegalapprovebll.GetHistoryList(lllegalid).OrderBy(p => p.CREATEDATE).ToList(); //排序

                foreach (LllegalApproveEntity entity in list)
                {
                    ApproveHistoryModel model = new ApproveHistoryModel();
                    model.approveid = entity.ID;
                    model.lllegalid = entity.LLLEGALID;
                    model.approveresult = entity.APPROVERESULT;
                    model.approvedeptname = entity.APPROVEDEPTNAME;
                    model.approvedate = null != entity.APPROVEDATE ? entity.APPROVEDATE.Value.ToString("yyyy-MM-dd") : "";
                    model.approveperson = entity.APPROVEPERSON;
                    model.approvereason = entity.APPROVEREASON;
                    var punish = lllegalpunishbll.GetEntityByApproveId(entity.ID);
                    if (null != punish)
                    {
                        model.assessobject = punish.ASSESSOBJECT;
                        model.chargepersonone = punish.PERSONINCHARGENAME;
                        model.economicspunishone = punish.ECONOMICSPUNISH;
                        model.performancepoint = punish.PERFORMANCEPOINT;
                        model.educationone = punish.EDUCATION;
                        model.lllegalpointone = punish.LLLEGALPOINT;
                        model.awaitjobone = punish.AWAITJOB;
                        model.mark = punish.MARK;
                    }
                    data.Add(model);
                }
            }

            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
        }
        #endregion

        #region 获取整改历史列表
        /// <summary>
        /// 获取整改历史列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetReformList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : ""; //违章id
            List<ReformHistoryModel> data = new List<ReformHistoryModel>(); //返回的结果集合
            if (!string.IsNullOrEmpty(lllegalid))
            {
                List<LllegalReformEntity> list = lllegalreformbll.GetHistoryList(lllegalid).OrderBy(p => p.CREATEDATE).ToList(); //排序

                foreach (LllegalReformEntity entity in list)
                {
                    ReformHistoryModel model = new ReformHistoryModel();
                    model.reformid = entity.ID;
                    model.lllegalid = entity.LLLEGALID;
                    model.reformstatus = entity.REFORMSTATUS;
                    model.reformdeadline = null != entity.REFORMDEADLINE ? entity.REFORMDEADLINE.Value.ToString("yyyy-MM-dd") : "";
                    model.reformfinishdate = null != entity.REFORMFINISHDATE ? entity.REFORMFINISHDATE.Value.ToString("yyyy-MM-dd") : "";
                    model.reformpeople = entity.REFORMPEOPLE;
                    model.reformtel = entity.REFORMTEL;
                    model.reformdeptname = entity.REFORMDEPTNAME;
                    model.reformmeasure = entity.REFORMMEASURE;
                    model.reformtel = entity.REFORMTEL;
                    List<Photo> picdata = new List<Photo>(); //整改图片  
                    IEnumerable<FileInfoEntity> reformfile = fileInfoBLL.GetImageListByObject(entity.REFORMPIC);
                    foreach (FileInfoEntity fentity in reformfile)
                    {
                        Photo p = new Photo();
                        p.id = fentity.FileId;
                        p.filename = fentity.FileName;
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                        p.folderid = fentity.FolderId;
                        picdata.Add(p);
                    }
                    model.reformpic = picdata;

                    List<Photo> attdata = new List<Photo>(); //整改附件  
                    IEnumerable<FileInfoEntity> attachmentfile = fileInfoBLL.GetImageListByObject(entity.REFORMATTACHMENT);
                    foreach (FileInfoEntity fentity in attachmentfile)
                    {
                        Photo p = new Photo();
                        p.id = fentity.FileId;
                        p.filename = fentity.FileName;
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                        p.folderid = fentity.FolderId;
                        attdata.Add(p);
                    }
                    model.attachment = attdata;
                    data.Add(model);
                }
            }

            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
        }
        #endregion

        #region 获取违章整改延期详情信息
        /// <summary>
        /// 获取延期详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object LllegalDelayApplyInfo([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : ""; //违章id
            string handleid = res.Contains("handleid") ? dy.data.handleid.ToString() : string.Empty;
            List<LllegalExtensionEntity> list = new List<LllegalExtensionEntity>();
            if (!string.IsNullOrEmpty(handleid))
            {
                list = lllegalextensionbll.GetList(lllegalid).Where(p => p.HANDLEID == handleid).OrderBy(p => p.CREATEDATE).ToList();
            }
            else
            {
                list = lllegalextensionbll.GetListByCondition(lllegalid).OrderBy(p => p.CREATEDATE).ToList();
            }

            var baseentity = lllegalregisterbll.GetEntity(lllegalid);
            IList<PostPoneData> result = new List<PostPoneData>();

            foreach (LllegalExtensionEntity entity in list)
            {
                LllegalPostPoneData data = new LllegalPostPoneData();
                if (null != baseentity)
                {
                    data.flowstate = baseentity.FLOWSTATE;
                }
                data.handleid = entity.HANDLEID;
                data.lllegalid = entity.LLLEGALID;
                data.postponedays = entity.POSTPONEDAYS;
                data.applyreason = entity.POSTPONEOPINION; //审核意见
                data.postponeresult = entity.POSTPONERESULT;
                data.applydate = null != entity.HANDLEDATE ? entity.HANDLEDATE.Value.ToString("yyyy-MM-dd") : "";
                data.applyperson = entity.HANDLEUSERNAME;
                data.applypersonid = entity.HANDLEUSERID;
                data.applydept = entity.HANDLEDEPTCODE;
                data.applydeptname = entity.HANDLEDEPTNAME;
                if (entity.HANDLETYPE == "0")
                {
                    data.handlestate = "延期申请"; //状态
                    data.applyreason = entity.POSTPONEREASON; //申请原因
                }
                if (entity.HANDLETYPE == "1")
                {
                    data.handlestate = "延期审批";
                }
                if (entity.HANDLETYPE == "2")
                {
                    data.handlestate = "延期完成";
                }
                if (entity.HANDLETYPE == "-1")
                {
                    data.handlestate = "延期失败";
                }
                data.handletype = entity.HANDLETYPE;
                result.Add(data);
            }
            if (!string.IsNullOrEmpty(handleid))
            {
                result = result.Where(p => p.handleid == handleid).ToList();
            }
            return new { code = 0, info = "获取数据成功", count = result.Count(), data = result };
        }
        #endregion

        #region 获取验收历史列表
        /// <summary>
        /// 获取验收历史列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAcceptList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : ""; //违章id

            List<AcceptHistoryModel> data = new List<AcceptHistoryModel>(); //返回的结果集合

            if (!string.IsNullOrEmpty(lllegalid))
            {
                List<LllegalAcceptEntity> list = lllegalacceptbll.GetHistoryList(lllegalid).OrderBy(p => p.CREATEDATE).ToList(); //排序

                foreach (LllegalAcceptEntity entity in list)
                {
                    AcceptHistoryModel model = new AcceptHistoryModel();
                    model.acceptid = entity.ID;
                    model.lllegalid = entity.LLLEGALID;
                    model.acceptresult = entity.ACCEPTRESULT;
                    model.acceptpeople = entity.ACCEPTPEOPLE;
                    model.acceptdeptname = entity.ACCEPTDEPTNAME;
                    model.acceptmind = entity.ACCEPTMIND;
                    model.accepttime = null != entity.ACCEPTTIME ? entity.ACCEPTTIME.Value.ToString("yyyy-MM-dd") : "";
                    model.isgrpaccept = entity.ISGRPACCEPT;
                    List<Photo> picdata = new List<Photo>(); //验收图片  
                    IEnumerable<FileInfoEntity> reformfile = fileInfoBLL.GetImageListByObject(entity.ACCEPTPIC);
                    foreach (FileInfoEntity fentity in reformfile)
                    {
                        Photo p = new Photo();
                        p.id = fentity.FileId;
                        p.filename = fentity.FileName;
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                        p.folderid = fentity.FolderId;
                        picdata.Add(p);
                    }
                    model.acceptpic = picdata;
                    data.Add(model);
                }
            }

            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
        }
        #endregion

        #region 获取验收确认历史列表
        /// <summary>
        /// 获取验收确认历史列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetConfirmList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : ""; //违章id

            List<ConfirmHistoryModel> data = new List<ConfirmHistoryModel>(); //返回的结果集合

            if (!string.IsNullOrEmpty(lllegalid))
            {
                List<LllegalConfirmEntity> list = lllegalconfirmbll.GetHistoryList(lllegalid).OrderBy(p => p.CREATEDATE).ToList(); //排序

                foreach (LllegalConfirmEntity entity in list)
                {
                    ConfirmHistoryModel model = new ConfirmHistoryModel();
                    model.confirmid = entity.ID;
                    model.lllegalid = entity.LLLEGALID;
                    model.confirmresult = entity.CONFIRMRESULT;
                    model.confirmpeople = entity.CONFIRMPEOPLE;
                    model.confirmdeptname = entity.CONFIRMDEPTNAME;
                    model.confirmmind = entity.CONFIRMMIND;
                    model.confirmtime = null != entity.CONFIRMTIME ? entity.CONFIRMTIME.Value.ToString("yyyy-MM-dd") : "";
                    data.Add(model);
                }
            }

            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
        }
        #endregion

        #region 获取违章详情信息
        /// <summary>
        /// 获取违章详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalDetail([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : ""; //隐患主键

            var baseInfo = lllegalregisterbll.GetLllegalModel(lllegalid);

            LllegalModel entity = new LllegalModel();
            if (baseInfo.Rows.Count == 1)
            {
                //违章基本信息
                entity.lllegalid = baseInfo.Rows[0]["id"].ToString();
                entity.createuserid = baseInfo.Rows[0]["createuserid"].ToString();//创建人id
                entity.createusername = baseInfo.Rows[0]["createusername"].ToString();//创建用户姓名
                entity.createdate = !string.IsNullOrEmpty(baseInfo.Rows[0]["createdate"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["createdate"].ToString()).ToString("yyyy-MM-dd") : ""; // baseInfo.Rows[0]["createdate"].ToString(); //创建时间
                entity.createdeptid = baseInfo.Rows[0]["createdeptid"].ToString();//创建单位id
                entity.createdeptname = baseInfo.Rows[0]["createdeptname"].ToString();//创建单位名称
                entity.lllegalnumber = baseInfo.Rows[0]["lllegalnumber"].ToString();  //违章编号

                entity.verifydeptname = baseInfo.Rows[0]["verifydeptname"].ToString(); //审核部门
                entity.verifydeptid = baseInfo.Rows[0]["verifydeptid"].ToString(); //审核部门
                entity.lllegaltype = baseInfo.Rows[0]["lllegaltype"].ToString();//违章类型id
                entity.lllegaltypename = baseInfo.Rows[0]["lllegaltypename"].ToString();//违章类型名称
                entity.lllegaltime = !string.IsNullOrEmpty(baseInfo.Rows[0]["lllegaltime"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["lllegaltime"].ToString()).ToString("yyyy-MM-dd") : ""; //违章时间
                entity.lllegallevel = baseInfo.Rows[0]["lllegallevel"].ToString();//违章级别id
                entity.belongdepart = baseInfo.Rows[0]["belongdepart"].ToString();
                entity.belongdepartid = baseInfo.Rows[0]["belongdepartid"].ToString();
                entity.lllegallevelname = baseInfo.Rows[0]["lllegallevelname"].ToString();//违章级别名称
                entity.majorclassify = baseInfo.Rows[0]["majorclassify"].ToString();//专业分类id
                entity.majorclassifyname = baseInfo.Rows[0]["majorclassifyname"].ToString();//专业分类名称
                entity.lllegalperson = baseInfo.Rows[0]["lllegalperson"].ToString(); //违章人员姓名
                entity.lllegalpersonid = baseInfo.Rows[0]["lllegalpersonid"].ToString();//违章人员id
                entity.lllegalteam = baseInfo.Rows[0]["lllegalteam"].ToString(); //违章单位名称
                entity.lllegalteamcode = baseInfo.Rows[0]["lllegalteamcode"].ToString();//违章单位编码
                entity.lllegaldepart = baseInfo.Rows[0]["lllegaldepart"].ToString();//违章责任单位名称
                entity.lllegaldepartcode = baseInfo.Rows[0]["lllegaldepartcode"].ToString();//违章责任单位编码
                entity.lllegaldescribe = baseInfo.Rows[0]["lllegaldescribe"].ToString();//违章责任描述
                entity.lllegaladdress = baseInfo.Rows[0]["lllegaladdress"].ToString(); //违章地点
                entity.reformrequire = baseInfo.Rows[0]["reformrequire"].ToString(); //整改要求
                entity.flowstate = baseInfo.Rows[0]["flowstate"].ToString(); //流程状态
                entity.addtype = baseInfo.Rows[0]["addtype"].ToString(); //登记类型  已整改的违章还是 一般登记
                entity.isexposure = baseInfo.Rows[0]["isexposure"].ToString() == "是" ? "1" : "0"; //是否曝光
                entity.reformpeople = baseInfo.Rows[0]["reformpeople"].ToString();//整改人姓名
                entity.reformpeopleid = baseInfo.Rows[0]["reformpeopleid"].ToString();//整改人id
                entity.reformtel = baseInfo.Rows[0]["reformtel"].ToString(); //整改人联系方式
                entity.reformdeptcode = baseInfo.Rows[0]["reformdeptcode"].ToString(); //整改部门编码
                entity.reformdeptname = baseInfo.Rows[0]["reformdeptname"].ToString(); //整改部门名称
                entity.reformdeadline = !string.IsNullOrEmpty(baseInfo.Rows[0]["reformdeadline"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["reformdeadline"].ToString()).ToString("yyyy-MM-dd") : ""; //整改截止时间
                entity.reformfinishdate = !string.IsNullOrEmpty(baseInfo.Rows[0]["reformfinishdate"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["reformfinishdate"].ToString()).ToString("yyyy-MM-dd") : "";//整改结束时间
                entity.reformstatus = baseInfo.Rows[0]["reformstatus"].ToString() == "已完成" ? "1" : "0"; //整改完成情况
                entity.reformmeasure = baseInfo.Rows[0]["reformmeasure"].ToString();//整改措施
                entity.applicationstatus = baseInfo.Rows[0]["applicationstatus"].ToString();//整改延期状态值

                entity.isappoint = baseInfo.Rows[0]["isappoint"].ToString();//是否指定整改责任人
                entity.reformchargeperson = baseInfo.Rows[0]["reformchargeperson"].ToString();//整改责任负责人
                entity.reformchargepersonname = baseInfo.Rows[0]["reformchargepersonname"].ToString();//整改责任负责人
                entity.reformchargedeptid = baseInfo.Rows[0]["reformchargedeptid"].ToString();//指定整改责任部门
                entity.reformchargedeptname = baseInfo.Rows[0]["reformchargedeptname"].ToString();//指定整改责任部门

                entity.acceptpeopleid = baseInfo.Rows[0]["acceptpeopleid"].ToString(); //验收人id
                entity.acceptpeople = baseInfo.Rows[0]["acceptpeople"].ToString();//验收人姓名
                entity.acceptdeptname = baseInfo.Rows[0]["acceptdeptname"].ToString(); //验收部门名称
                entity.acceptdeptcode = baseInfo.Rows[0]["acceptdeptcode"].ToString(); //验收部门编码
                entity.acceptresult = baseInfo.Rows[0]["acceptresult"].ToString(); //验收结果
                entity.acceptmind = baseInfo.Rows[0]["acceptmind"].ToString();//验收意见
                entity.accepttime = !string.IsNullOrEmpty(baseInfo.Rows[0]["accepttime"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["accepttime"].ToString()).ToString("yyyy-MM-dd") : "";//验收时间
                entity.isgrpaccept = baseInfo.Rows[0]["isgrpaccept"].ToString();//是否省公司验收
                entity.confirmpeople = baseInfo.Rows[0]["confirmusername"].ToString();
                entity.confirmpeopleid = baseInfo.Rows[0]["confirmuserid"].ToString();
                //验收确认信息
                LllegalConfirmEntity cfrEntity = lllegalconfirmbll.GetHistoryList(lllegalid).FirstOrDefault();
                if (null != cfrEntity)
                {
                    entity.confirmdeptcode = cfrEntity.CONFIRMDEPTCODE;
                    entity.confirmdeptname = cfrEntity.CONFIRMDEPTNAME;
                    entity.confirmmind = cfrEntity.CONFIRMMIND;
                    entity.confirmpeople = cfrEntity.CONFIRMPEOPLE;
                    entity.confirmpeopleid = cfrEntity.CONFIRMPEOPLEID;
                    entity.confirmresult = cfrEntity.CONFIRMRESULT;
                    entity.confirmtime = cfrEntity.CONFIRMTIME.HasValue ? cfrEntity.CONFIRMTIME.Value.ToString("yyyy-MM-dd") : "";
                }
                else
                {
                    entity.confirmdeptcode = "";
                    entity.confirmdeptname = "";
                    entity.confirmmind = "";
                    entity.confirmresult = "";
                    entity.confirmtime = "";
                }
                entity.reseverid = baseInfo.Rows[0]["reseverid"].ToString();//关联应用id
                entity.resevertype = baseInfo.Rows[0]["resevertype"].ToString(); //关联应用类型
                entity.participant = baseInfo.Rows[0]["participant"].ToString(); //流程参与账户
                entity.isupsafety = baseInfo.Rows[0]["isupsafety"].ToString() == "是" ? "1" : "0";//是否上报安全主管部门
                entity.isback = baseInfo.Rows[0]["reseverfour"].ToString();  //是否回退
                entity.backreason = baseInfo.Rows[0]["reseverfive"].ToString();  //回退原因
                entity.engineerid = baseInfo.Rows[0]["engineerid"].ToString();  //外包工程id
                entity.engineername = baseInfo.Rows[0]["engineername"].ToString(); //外包工程名称

                entity.isenableback = true; //启用回退
                var historyacceptList = lllegalacceptbll.GetHistoryList(lllegalid).ToList();
                if (historyacceptList.Count() > 0)
                {
                    entity.isenableback = false;
                }

                //关联责任人
                #region 违章考核信息
                List<PunishData> punishdata = new List<PunishData>();
                List<LllegalPunishEntity> lllegalpunishList = lllegalpunishbll.GetListByLllegalId(lllegalid, "");
                foreach (LllegalPunishEntity punishEntity in lllegalpunishList)
                {
                    PunishData pshdata = new PunishData();
                    pshdata.assessobject = punishEntity.ASSESSOBJECT;
                    pshdata.chargeperson = punishEntity.PERSONINCHARGENAME;
                    pshdata.chargepersonid = punishEntity.PERSONINCHARGEID;
                    pshdata.economicspunish = punishEntity.ECONOMICSPUNISH;
                    pshdata.performancepoint = punishEntity.PERFORMANCEPOINT;
                    pshdata.education = punishEntity.EDUCATION;
                    pshdata.lllegalpoint = punishEntity.LLLEGALPOINT;
                    pshdata.awaitjob = punishEntity.AWAITJOB;
                    pshdata.mark = punishEntity.MARK;
                    punishdata.Add(pshdata);
                }
                entity.punishdata = punishdata; 
                #endregion

                #region 违章奖励信息
                var awardInfo = lllegalawarddetailbll.GetListByLllegalId(lllegalid);//违章奖励信息
                List<AwardData> awarddata = new List<AwardData>();
                foreach (LllegalAwardDetailEntity awardDetail in awardInfo)
                {
                    awarddata.Add(new AwardData
                    {
                        awarduserid = awardDetail.USERID,
                        awardusername = awardDetail.USERNAME,
                        awarddeptid = awardDetail.DEPTID,
                        awarddeptname = awardDetail.DEPTNAME,
                        awardmoney = awardDetail.MONEY,
                        awardpoints = awardDetail.POINTS
                    });
                }
                entity.awarddata = awarddata; 
                #endregion

                string lllegalphoto = baseInfo.Rows[0]["lllegalpic"].ToString();  //违章图片 
                string reformphoto = baseInfo.Rows[0]["reformpic"].ToString();   //违章整改图片
                string acceptphoto = baseInfo.Rows[0]["acceptpic"].ToString();   //违章验收图片
                string attachment = baseInfo.Rows[0]["reformattachment"].ToString();   //违章附件
                List<Photo> lllegalpic = new List<Photo>(); //违章图片
                List<Photo> reformpic = new List<Photo>(); //整改图片
                List<Photo> acceptpic = new List<Photo>();  //验收图片 
                List<Photo> reformattachment = new List<Photo>();  //违章附件 
                IEnumerable<FileInfoEntity> lllegalfile = fileInfoBLL.GetImageListByTop5Object(lllegalphoto);
                foreach (FileInfoEntity fentity in lllegalfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    lllegalpic.Add(p);
                }
                entity.lllegalpic = lllegalpic;
                IEnumerable<FileInfoEntity> reformfile = fileInfoBLL.GetImageListByTop5Object(reformphoto);
                foreach (FileInfoEntity fentity in reformfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    reformpic.Add(p);
                }
                entity.reformpic = reformpic;
                //整改附件
                IEnumerable<FileInfoEntity> attachmentfile = fileInfoBLL.GetImageListByTop5Object(attachment);
                foreach (FileInfoEntity fentity in attachmentfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    reformattachment.Add(p);
                }
                entity.attachment = reformattachment;
                //验收图片
                IEnumerable<FileInfoEntity> acceptfile = fileInfoBLL.GetImageListByTop5Object(acceptphoto);
                foreach (FileInfoEntity fentity in acceptfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    acceptpic.Add(p);
                }
                entity.acceptpic = acceptpic;
                entity.appentity = new LllegalApproveBLL().GetEntityByBid(entity.lllegalid);
                entity.checkflow = GetCheckFlowData(entity.lllegalid, "Lllegal");
                entity.isdeliver = htworkflowbll.GetCurUserWfAuth("", "违章整改", "违章整改", "厂级违章流程", "转交") == "1";
                entity.isacceptdeliver = htworkflowbll.GetCurUserWfAuth("", "违章验收", "违章验收", "厂级违章流程", "转交") == "1";
                string ControlPicMustUpload = string.Empty;
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'ControlPicMustUpload','LllegalAwardDetailAuth'");
                var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == curUser.OrganizeId);
                if (cpmu.Count() > 0)
                {
                    ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
                }
                entity.wzpicmustupload = ControlPicMustUpload.Contains("LLLEGALPIC");
                entity.zgpicmustupload = ControlPicMustUpload.Contains("REFORMPIC");
                entity.yspicmustupload = ControlPicMustUpload.Contains("ACCEPTPIC");
                //违章分析模板
                entity.analyzetemplate = dataitemdetailbll.GetItemValue("imgUrl") + "/Resource/ExcelTemplate/内蒙古京能康巴什热电有限公司---典型违章分析(模板).docx";
                //是否能操作违章奖励
                var awardauth = itemlist.Where(p => p.EnCode == "LllegalAwardDetailAuth").Where(p => p.ItemName == curUser.OrganizeId);
                entity.isgetaward = awardauth.Count() > 0;
            }

            return new { code = 0, count = 0, info = "获取成功", data = entity };
        }
        #endregion

        #region 按月统计当天登记的违章并且当天未验收的违章数量和当天登记的违章的总数量
        [HttpPost]
        public object GetLllegalRegisterNumByMonth([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string year = dy.data.year;
                string month = dy.data.month;
                string currdate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1).ToString("yyyy-MM-dd");
                var dt = htbaseinfobll.GetLllegalRegisterNumByMonth(currdate, curUser.DeptCode);

                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {

                return new { code = 0, info = "获取数据失败", count = 0, data = new DataTable() };
            }
        }
        #endregion

        #region 违章统计--1按类型统计 2违章数据变换趋势
        [HttpPost]
        public object GetStatis([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                int year = Convert.ToInt32(dy.data.year);
                var starttime = new DateTime(year, 1, 1);
                var endtime = new DateTime(year, 12, 31);
                //按整改单位统计
                var queryJson = new
                {
                    startTime = starttime,
                    endTime = endtime,
                    deptCode = curUser.DeptCode,
                    typeGroups = "作业类,管理类,指挥类,装置类,文明卫生类",
                    deptMark = "reformdeptcode"
                };

                var dtType = legbll.GetLllegalTypeTotal(queryJson.ToJson());//违章类型数量统计
                var queryJson1 = new
                {
                    year = year,
                    deptCode = curUser.DeptCode,
                    levelGroups = "一般违章,较严重违章,严重违章",
                    deptMark = "reformdeptcode"
                };
                //按整改单位统计
                var dtqs = legbll.GetLllegalTrendData(queryJson1.ToJson());
                var dttotal = GetLllegalTrendTotal(dtqs, queryJson1.ToJson());//趋势图
                var jsonData = new
                {
                    pieType = dtType,
                    lineQs = dttotal
                };
                return new { code = 0, info = "获取数据成功", count = 1, data = jsonData };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0, data = new DataTable() };
            }
        }

        [HttpPost]
        public object GetWzPieStatis([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string seltype = res.Contains("seltype") ? dy.data.seltype : string.Empty; //按类型统计  0:月度;1:季度;2:年度
                string stattype = res.Contains("stattype") ? dy.data.stattype : "0"; //统计方式  0 按创建单位 创建时间  1 违章单位  违章时间 
                string deptMark = string.Empty;
                string timeMark = string.Empty;
                string deptCode = string.Empty;
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("省级") || curUser.RoleName.Contains("集团"))
                {
                    deptCode = curUser.OrganizeCode;
                }
                if (stattype == "0") //创建单位 创建时间
                {
                    deptMark = "createuserdeptcode";
                    timeMark = "createdate";
                }
                else if (stattype == "1") //违章单位 违章时间
                {
                    deptMark = "lllegalteamcode";
                    timeMark = "lllegaltime";
                }
                //按登记单位来统计
                var queryJson = new
                {
                    deptCode = deptCode,
                    seltype = seltype,
                    typeGroups = "作业类,管理类,指挥类,装置类,文明卫生类",
                    deptMark = deptMark,
                    timeMark = timeMark
                };

                var dtType = legbll.GetLllegalTypeTotal(queryJson.ToJson());//违章类型数量统计
            
                var jsonData = new
                {
                    piedata = dtType
                };
                return new { code = 0, info = "获取数据成功", count = 1, data = jsonData };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0, data = new DataTable() };
            }
        }

        #region 协助方法
        private List<object> GetLllegalTrendTotal(DataTable dt, string queryJson)
        {
            var list = new List<dynamic>();

            if (dt != null && dt.Rows.Count > 0)
            {
                var colors = new Dictionary<string, string>()
                {
                    {"一般违章","#558ED5" },
                    {"较严重违章","#FFC000" },
                    {"严重违章","#FF0000" },
                    {"合计","#29E3E8" }
                };
                var select = dt.Select();
                var queryParam = queryJson.ToJObject();
                var groups = queryParam["levelGroups"].ToString();
                var grpList = groups.Split(new char[] { ',' });
                int[] total = new int[dt.Rows.Count];
                for (var i = 0; i < grpList.Length; i++)
                {
                    var grpName = grpList[i];
                    List<int> data = new List<int>();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow dr = dt.Rows[j];
                        int num = int.Parse(dr[grpName].ToString());
                        total[j] += num;
                        data.Add(num);
                    }
                    list.Add(new { name = grpName, color = GetColor(colors, grpName), data = data });
                }
                var totalName = "合计";
                list.Add(new { name = totalName, color = GetColor(colors, totalName), data = total });
            }

            return list;
        }
        private string GetColor(Dictionary<string, string> dic, string key)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            string r = string.Format("RGB({0}, {1}, {2})", rnd.Next(255), rnd.Next(255), rnd.Next(255));//默认随机颜色

            if (dic.ContainsKey(key))
                r = dic[key];

            return r;
        }
        #endregion

        [HttpPost]
        public object GetJFStatis([FromBody]JObject json)
        {

            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                int year = Convert.ToInt32(dy.data.year);
                var queryJson = new
                {
                    year = year,
                    deptCode = curUser.DeptCode

                };
                // string queryJson = "{\"year\":" + year + ",\"deptCode\":" + curUser.DeptCode + "}";
                var dt = legbll.GetJFStatis(queryJson.ToJson());

                return new { code = 0, info = "获取数据成功", count = 1, data = dt };
            }
            catch (Exception)
            {

                return new { code = 0, info = "获取数据失败", count = 0, data = new DataTable() };
            }
        }
        #endregion

        #region  违章统计(按级别进行统计)
        /// <summary>
        /// 违章统计(按级别进行统计)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getLllegalStatisticsList([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string deptcode = string.Empty;

            //省级用户
            if (curUser.RoleName.Contains("省级用户"))
            {
                deptcode = res.Contains("deptcode") ? dy.data.deptcode.ToString() : "";  //按电厂来
            }
            else if (curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户"))  //当前用户是公司级及厂级用户
            {
                deptcode = curUser.OrganizeCode; //厂级用户
            }
            else
            {
                deptcode = curUser.DeptCode; //普通用户
            }
            try
            {
                //违章数量
                var dt = legbll.GetAppLllegalStatistics(deptcode, "", 0);

                string wztotal = string.Empty;//违章总数 
                string ybwznum = string.Empty;  //一般违章
                string jyznum = string.Empty; //较严重违章
                string yznum = string.Empty; //严重违章

                string ybwz_ratio = string.Empty; //一般违章占比
                string jyzwz_ratio = string.Empty; //较严重违章占比
                string yzwz_ratio = string.Empty; //严重违章占比
                if (dt.Rows.Count == 1)
                {
                    wztotal = dt.Rows[0]["lllegal"].ToString();
                    ybwznum = dt.Rows[0]["yblllegal"].ToString();
                    jyznum = dt.Rows[0]["jyzlllegal"].ToString();
                    yznum = dt.Rows[0]["yzlllegal"].ToString();

                    ybwz_ratio = int.Parse(wztotal) == 0 ? "0" : Math.Round((Convert.ToDecimal(ybwznum) * 100 / Convert.ToDecimal(wztotal)), 2).ToString(); //一般违章占比
                    jyzwz_ratio = int.Parse(wztotal) == 0 ? "0" : Math.Round((Convert.ToDecimal(jyznum) * 100 / Convert.ToDecimal(wztotal)), 2).ToString();//较严重违章占比
                    yzwz_ratio = int.Parse(wztotal) == 0 ? "0" : Math.Round((Convert.ToDecimal(yznum) * 100 / Convert.ToDecimal(wztotal)), 2).ToString();//严重违章占比
                }

                //返回结果
                var jsondata = new
                {
                    wztotal = wztotal,
                    ybwznum = ybwznum,
                    jyznum = jyznum,
                    yznum = yznum,
                    ybwzratio = ybwz_ratio,
                    jyzwzratio = jyzwz_ratio,
                    yzwzratio = yzwz_ratio,
                    ybname = "一般违章",
                    jyzname = "较严重违章",
                    yzname = "严重违章"
                };

                return new { Code = 0, Info = "获取数据成功", Count = 0, data = jsondata };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = "获取数据失败", Count = 0 };
            }
        }
        #endregion

        #region 违章数量统计
        /// <summary>
        /// 违章整改
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalLevelZGV()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            var deptCode = curUser.DeptCode;
            if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.DeptName.Contains("安环部"))
                deptCode = curUser.OrganizeCode;
            var dt = legbll.GetLllegalZgv(deptCode);
            //定义已整改跟未整改
            var zgYes1 = 0M;
            var zgNo1 = 0M;
            var zgYes2 = 0M;
            var zgNo2 = 0M;
            var zgYes3 = 0M;
            var zgNo3 = 0M;
            var Total = 0M;
            foreach (DataRow dr in dt.Rows)
            {
                Total += decimal.Parse(dr["Num"].ToString());
                if (dr["lllegallevelname"].ToString() == "一般违章")
                {
                    //已整改
                    if (dr["Flowstate"].ToString() == "违章验收" || dr["Flowstate"].ToString() == "流程结束")
                        zgYes1 += decimal.Parse(dr["Num"].ToString());
                    else
                        zgNo1 += decimal.Parse(dr["Num"].ToString());
                }
                if (dr["lllegallevelname"].ToString() == "较严重违章")
                {
                    //已整改
                    if (dr["Flowstate"].ToString() == "违章验收" || dr["Flowstate"].ToString() == "流程结束")
                        zgYes2 += decimal.Parse(dr["Num"].ToString());
                    else
                        zgNo2 += decimal.Parse(dr["Num"].ToString());
                }
                if (dr["lllegallevelname"].ToString() == "严重违章")
                {
                    //已整改
                    if (dr["Flowstate"].ToString() == "违章验收" || dr["Flowstate"].ToString() == "流程结束")
                        zgYes3 += decimal.Parse(dr["Num"].ToString());
                    else
                        zgNo3 += decimal.Parse(dr["Num"].ToString());
                }
            }
            //统计返回数据
            var list = new List<LllegalZGInfo>();
            var total1 = zgYes1 + zgNo1;
            list.Add(new LllegalZGInfo { LllegalTotal = total1, LllegalLevelName = "一般违章", LllegalReform = zgYes1, LllegalReformNo = zgNo1, LllegalZGV = (total1 == 0 ? 0 : zgYes1 / total1).ToString("f2") + "%" });
            var total2 = zgYes2 + zgNo2;
            list.Add(new LllegalZGInfo { LllegalTotal = total2, LllegalLevelName = "较严重违章", LllegalReform = zgYes2, LllegalReformNo = zgNo2, LllegalZGV = (total2 == 0 ? 0 : zgYes2 / total2).ToString("f2") + "%" });
            var total3 = zgYes3 + zgNo3;
            list.Add(new LllegalZGInfo { LllegalTotal = total3, LllegalLevelName = "严重违章", LllegalReform = zgYes3, LllegalReformNo = zgNo3, LllegalZGV = (total3 == 0 ? 0 : zgYes3 / total3).ToString("f2") + "%" });
            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 0,
                data = new { Total = Total, ZGVInfo = list }
            };
        }

        public class LllegalZGInfo
        {
            public decimal LllegalTotal { get; set; }
            public string LllegalLevelName { get; set; }
            public decimal LllegalReform { get; set; }
            public decimal LllegalReformNo { get; set; }
            public string LllegalZGV { get; set; }

        }
        #endregion

        #region 班组终端违章统计(按创建单位来统计)
        /// <summary>
        /// 班组终端统计(按创建单位来统计)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalChartForTeam([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string year = res.Contains("year") ? dy.data.year : DateTime.Now.Year.ToString();
                string deptCode = curUser.DeptCode;  //部门

                //违章类别统计图
                var rankdt = legbll.GetAppLllegalStatistics(deptCode, year, 2);
                List<oseries> oslist = new List<oseries>();
                if (rankdt.Rows.Count == 1)
                {
                    //文明卫生类
                    oseries wmtype = new oseries();
                    wmtype.name = "文明卫生类";
                    wmtype.precent = Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) != 0 ? Math.Round(Convert.ToDecimal(rankdt.Rows[0]["wmtype"].ToString()) / Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) * 100, 2) : 0;
                    wmtype.num = int.Parse(rankdt.Rows[0]["wmtype"].ToString());
                    wmtype.color = "#F6EC08";
                    oslist.Add(wmtype);

                    //装置类
                    oseries zztype = new oseries();
                    zztype.name = "装置类";
                    zztype.precent = Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) != 0 ? Math.Round(Convert.ToDecimal(rankdt.Rows[0]["zztype"].ToString()) / Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) * 100, 2) : 0;
                    zztype.num = int.Parse(rankdt.Rows[0]["zztype"].ToString());
                    zztype.color = "#4F81BD";
                    oslist.Add(zztype);


                    //装置类
                    oseries zytype = new oseries();
                    zytype.name = "作业类";
                    zytype.precent = Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) != 0 ? Math.Round(Convert.ToDecimal(rankdt.Rows[0]["zytype"].ToString()) / Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) * 100, 2) : 0;
                    zytype.num = int.Parse(rankdt.Rows[0]["zytype"].ToString());
                    zytype.color = "#C0504D";
                    oslist.Add(zytype);


                    //装置类
                    oseries gltype = new oseries();
                    gltype.name = "管理类";
                    gltype.precent = Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) != 0 ? Math.Round(Convert.ToDecimal(rankdt.Rows[0]["gltype"].ToString()) / Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) * 100, 2) : 0;
                    gltype.num = int.Parse(rankdt.Rows[0]["gltype"].ToString());
                    gltype.color = "#98B65A";
                    oslist.Add(gltype);


                    //装置类
                    oseries zhtype = new oseries();
                    zhtype.name = "指挥类";
                    zhtype.precent = Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) != 0 ? Math.Round(Convert.ToDecimal(rankdt.Rows[0]["zhtype"].ToString()) / Convert.ToDecimal(rankdt.Rows[0]["total"].ToString()) * 100, 2) : 0;
                    zhtype.num = int.Parse(rankdt.Rows[0]["zhtype"].ToString());
                    zhtype.color = "#8064A2";
                    oslist.Add(zhtype);
                }
                //违章月份趋势统计图
                var monthdt = legbll.GetAppLllegalStatistics(deptCode, year, 1);
                List<mseries> list = new List<mseries>();
                foreach (DataRow row in monthdt.Rows)
                {
                    mseries entity = new mseries();
                    entity.name = row["month"].ToString();
                    entity.ybnum = int.Parse(row["s1"].ToString());
                    entity.jyznum = int.Parse(row["s2"].ToString());
                    entity.zdnum = int.Parse(row["s3"].ToString());
                    list.Add(entity);
                }

                var data = new
                {
                    rankdata = oslist,
                    monthdata = list
                };

                return new { Code = 0, Info = "获取数据成功", Count = 0, data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }
        }
        #endregion

        #region  获取违章档案(班组终端)
        /// <summary>
        /// 获取违章档案(班组终端)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalRecordForTeam([FromBody]JObject json)
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string year = res.Contains("year") ? dy.data.year : DateTime.Now.Year.ToString();

                var data = lllegalregisterbll.GetLllegalRecord(curUser.UserId, year);

                return new { Code = 0, Info = "获取数据成功", Count = 0, data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message, Count = 0 };
            }
        }
        #endregion

        #endregion

        #region 违章流程部分

        #region 违章整改
        /// <summary>
        /// 违章整改率
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalLevelTotal()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            var queryJson = new { deptCode = dy.DeptCode, Year = dy.Year, levelGroups = "一般违章,较严重违章,严重违章" };
            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 0,
                data = legbll.GetLllegalLevelTotal(queryJson.ToJson())
            };

        }
        #endregion

        #region 违章标准
        /// <summary>
        /// 违章标准列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLllEgalStdInfoList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string roleNames = curUser.RoleName;
            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = res.Contains("page") ? int.Parse(dy.page) : 1;
            pagination.rows = res.Contains("rows") ? int.Parse(dy.rows) : 10;

            pagination.p_fields = @"des,leglevel,legLevalName,legtype,legTypeName,bustype,busTypeName,descore,demoney,firstdescore,firstdemoney,seconddescore,seconddemoney,remark";

            pagination.p_kid = "id";
            pagination.p_tablename = @"v_lllegalstdinfo";
            pagination.conditionJson = " 1=1";
            if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.DeptName.Contains("安环部"))
            {
                pagination.conditionJson += " and CREATEUSERDEPTCODE like '" + curUser.OrganizeCode + "%'";
            }
            else
            {
                pagination.conditionJson += " and CREATEUSERDEPTCODE like '" + curUser.DeptCode + "%'";
            }
            string lllegaltype = res.Contains("lllegaltype") ? dy.data.lllegaltype : string.Empty;
            string lllegallevel = res.Contains("lllegallevel") ? dy.data.lllegallevel : string.Empty;
            string lllegaldescribe = res.Contains("lllegaldescribe") ? dy.data.lllegaldescribe : string.Empty;

            //违章类型
            if (!string.IsNullOrEmpty(lllegaltype))
            {
                pagination.conditionJson += string.Format(@" and  legtype='{0}' ", lllegaltype.ToString());
            }
            //违章级别
            if (!string.IsNullOrEmpty(lllegallevel))
            {
                pagination.conditionJson += string.Format(@" and leglevel ='{0}'", lllegallevel.ToString());
            }
            //违章描述 
            if (!string.IsNullOrEmpty(lllegaldescribe))
            {
                pagination.conditionJson += string.Format(@" and des like '%{0}%'", lllegaldescribe.ToString());
            }
            var queryJson = new { };
            var data = lllegalstandardbll.GetLllegalStdInfo(pagination, queryJson.ToJson());
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
            };
            return new { code = 0, info = "获取数据成功", data = JsonData };

        }


        /// <summary>
        /// 选择违章标准
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SelectLllEgalStdInfo([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;

            string roleNames = curUser.RoleName;
            //分页获取数据
            string sqlwhere = "";
            if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.DeptName.Contains("安环部"))
                sqlwhere += " and CREATEUSERDEPTCODE like '" + curUser.OrganizeCode + "%'";
            else
                sqlwhere += " and CREATEUSERDEPTCODE like '" + curUser.DeptCode + "%'";
            var dt = hazardsourcebll.FindTableBySql(string.Format("select id,des,leglevel,legLevalname,legtype,legtypeName,descore,demoney,firstdescore,firstdemoney,seconddescore,seconddemoney from v_lllegalstdinfo where Id='{0}'" + sqlwhere, dy.Id));

            return new { code = 0, info = "获取数据成功", data = dt };

        }

        #endregion

        #region 新增保存违章 / 提交违章  / 一次性提交违章(已整改违章登记)
        /// <summary>
        /// 新增保存隐患 / 提交隐患  / 一次性提交隐患  / 提交违章
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddLllegalPush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string wfFlag = "";
                string participant = "";
                string startflow = string.Empty;//起始
                string endflow = string.Empty; //截止
                string addtype = dy.data.addtype; //新增类型  一般整改违章 0    立即整改违章 1  
                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                #region 违章登记信息
                LllegalRegisterEntity entity = null;
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    entity = lllegalregisterbll.GetEntity(lllegalid);
                }
                if (null == entity)
                {
                    entity = new LllegalRegisterEntity();
                    entity.CREATEDEPTID = curUser.DeptId;
                    entity.CREATEDEPTNAME = curUser.DeptName;
                    entity.ID = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                }
                //新增时
                if (string.IsNullOrEmpty(lllegalid))
                {
                    if (!curUser.RoleName.Contains("省级"))
                    {
                        entity.BELONGDEPARTID = curUser.OrganizeId;
                        entity.BELONGDEPART = curUser.OrganizeName;
                    }
                    string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";//违章编号
                    entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));
                }
                //违章 基本信息                
                entity.LLLEGALTYPE = res.Contains("lllegaltype") ? dy.data.lllegaltype : "";// 违章类型id
                string lllegaltime = res.Contains("lllegaltime") ? dy.data.lllegaltime : "";
                if (!string.IsNullOrEmpty(lllegaltime))
                {
                    entity.LLLEGALTIME = Convert.ToDateTime(lllegaltime);//违章时间
                }
                entity.VERIFYDEPTNAME = res.Contains("verifydeptname") ? dy.data.verifydeptname : "";//审核部门
                entity.VERIFYDEPTID = res.Contains("verifydeptid") ? dy.data.verifydeptid : "";//审核部门
                entity.LLLEGALLEVEL = res.Contains("lllegallevel") ? dy.data.lllegallevel : "";  //违章级别
                entity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : ""; //专业分类
                entity.LLLEGALPERSON = res.Contains("lllegalperson") ? dy.data.lllegalperson : ""; //违章人员姓名
                entity.LLLEGALPERSONID = res.Contains("lllegalpersonid") ? dy.data.lllegalpersonid : "";  //违章人员id
                entity.LLLEGALTEAM = res.Contains("lllegalteam") ? dy.data.lllegalteam : ""; //违章单位名称
                entity.LLLEGALTEAMCODE = res.Contains("lllegalteamcode") ? dy.data.lllegalteamcode : "";  //违章单位编码
                entity.LLLEGALDEPART = res.Contains("lllegaldepart") ? dy.data.lllegaldepart : "";  //违章责任单位名称
                entity.LLLEGALDEPARTCODE = res.Contains("lllegaldepartcode") ? dy.data.lllegaldepartcode : "";  //违章责任单位编码
                entity.LLLEGALDESCRIBE = res.Contains("lllegaldescribe") ? dy.data.lllegaldescribe : "";  //违章责任描述
                entity.LLLEGALADDRESS = res.Contains("lllegaladdress") ? dy.data.lllegaladdress : "";  //违章地点
                entity.REFORMREQUIRE = res.Contains("reformrequire") ? dy.data.reformrequire : "";  //整改措施
                entity.FLOWSTATE = res.Contains("flowstate") ? dy.data.flowstate : "";  //流程状态
                entity.ISEXPOSURE = res.Contains("isexposure") ? dy.data.isexposure : "";  //是否曝光
                entity.RESEVERID = res.Contains("reseverid") ? dy.data.reseverid : "";  //关联应用id
                entity.RESEVERTYPE = res.Contains("resevertype") ? dy.data.resevertype : "";  //关联应用类型
                entity.RESEVERONE = res.Contains("reseverone") ? dy.data.reseverone : "";  //关联应用类型
                entity.RESEVERTWO = res.Contains("resevertwo") ? dy.data.resevertwo : "";  //关联应用类型
                entity.RESEVERTHREE = res.Contains("reseverthree") ? dy.data.reseverthree : "";  //关联应用类型
                entity.ISUPSAFETY = res.Contains("isupsafety") ? dy.data.isupsafety : "";  //是否上报安全主管部门
                entity.ENGINEERID = res.Contains("engineerid") ? dy.data.engineerid : "";   //外包工程id
                entity.ENGINEERNAME = res.Contains("engineername") ? dy.data.engineername : "";   //外包工程
                //省级用户传2
                if (curUser.RoleName.Contains("省级"))
                {
                    addtype = "2";
                }
                entity.ADDTYPE = addtype; //新增方式
                entity.RESEVERFIVE = "";
                entity.RESEVERFOUR = "";
                entity.BELONGDEPARTID = res.Contains("belongdepartid") ? dy.data.belongdepartid : entity.BELONGDEPARTID;   //所属单位ID
                entity.BELONGDEPART = res.Contains("belongdepart") ? dy.data.belongdepart : entity.BELONGDEPART;   //所属单位
                //违章图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(lllegalid))
                {
                    entity.LLLEGALPIC = Guid.NewGuid().ToString(); //违章图片
                }
                DeleteFile(fileids);  //先删除图片
                entity.LLLEGALPIC = !string.IsNullOrEmpty(entity.LLLEGALPIC) ? entity.LLLEGALPIC : Guid.NewGuid().ToString();
                UploadifyFile(entity.LLLEGALPIC, "lllegalpic", files); //上传违章图片
                /********************************/
                entity.APPSIGN = AppSign; //移动端标记
                //新增
                lllegalregisterbll.SaveForm(lllegalid, entity);
                #endregion
                #region 创建主体流程
                //主键为空  创建主体流程
                if (string.IsNullOrEmpty(lllegalid))
                {
                    string workFlow = string.Empty;
                    //一般整改 0 表示厂级限期， 2 表示省级限期
                    if (addtype == "0" || addtype == "2")
                    {
                        workFlow = "03";//违章处理
                    }
                    else  //立即整改  addtype :1 
                    {
                        workFlow = "04";//违章处理
                    }
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, curUser.UserId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                #endregion
                if (!string.IsNullOrEmpty(entity.ID))
                {
                    entity = lllegalregisterbll.GetEntity(entity.ID);
                }
                #region 违章整改信息
                LllegalReformEntity centity = new LllegalReformEntity();
                string reformid = res.Contains("reformid") ? dy.data.reformid : "";  //整改id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    centity = lllegalreformbll.GetEntityByBid(lllegalid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.LLLEGALID = entity.ID; //隐患编码 
                centity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人姓名
                centity.REFORMPEOPLEID = res.Contains("reformpeopleid") ? dy.data.reformpeopleid : "";  //整改人id
                centity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人联系方式
                centity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";  //整改部门编码
                centity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";  //验收部门编码
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : ""; //整改完成情况
                centity.REFORMMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";//整改措施 
                centity.ISAPPOINT = res.Contains("isappoint") ? dy.data.isappoint : null; //是否指定整改责任人
                centity.REFORMCHARGEPERSON = res.Contains("reformchargeperson") ? dy.data.reformchargeperson : ""; //整改责任负责人
                centity.REFORMCHARGEPERSONNAME = res.Contains("reformchargepersonname") ? dy.data.reformchargepersonname : ""; //整改责任负责人
                centity.REFORMCHARGEDEPTID = res.Contains("reformchargedeptid") ? dy.data.reformchargedeptid : ""; //指定整改责任部门
                centity.REFORMCHARGEDEPTNAME = res.Contains("reformchargedeptname") ? dy.data.reformchargedeptname : ""; //指定整改责任部门
                //整改截至时间
                string reformdeadline = res.Contains("reformdeadline") ? dy.data.reformdeadline : null;
                if (!string.IsNullOrEmpty(reformdeadline))
                {
                    centity.REFORMDEADLINE = Convert.ToDateTime(reformdeadline); //整改截至时间
                }
                //整改结束时间
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                //图片
                if (string.IsNullOrEmpty(centity.REFORMPIC))
                {
                    centity.REFORMPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                //上传违章整改图片
                centity.REFORMPIC = !string.IsNullOrEmpty(centity.REFORMPIC) ? centity.REFORMPIC : Guid.NewGuid().ToString();
                UploadifyFile(centity.REFORMPIC, "reformpic", files);

                centity.REFORMATTACHMENT = !string.IsNullOrEmpty(centity.REFORMATTACHMENT) ? centity.REFORMATTACHMENT : Guid.NewGuid().ToString();
                //上传整改附件
                UploadifyFile(centity.REFORMATTACHMENT, "attachment", files);
                /********************************/
                //新增
                centity.APPSIGN = AppSign; //移动端标记
                lllegalreformbll.SaveForm(reformid, centity);

                #endregion
                #region 违章考核信息
                string punishdata = res.Contains("punishdata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.punishdata) : "";
                if (!string.IsNullOrEmpty(punishdata))
                {
                    //先删除关联责任人集合
                    lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");
                    //反序列化考核对象
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(punishdata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string assessobject = rhInfo["assessobject"].ToString();
                        string personinchargename = rhInfo["chargeperson"].ToString(); //关联责任人姓名
                        string personinchargeid = rhInfo["chargepersonid"].ToString();//关联责任人id
                        string performancepoint = rhInfo["performancepoint"].ToString(); // EHS绩效考核 
                        string economicspunish = rhInfo["economicspunish"].ToString(); // 经济处罚
                        string education = rhInfo["education"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString();//违章扣分
                        string awaitjob = rhInfo["awaitjob"].ToString();//待岗

                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.ASSESSOBJECT = assessobject; //考核对象
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = assessobject.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }
                #endregion
                #region 违章奖励信息
                string awarddata = res.Contains("awarddata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.awarddata) : ""; 
                if (!string.IsNullOrEmpty(awarddata))
                {  //先删除关联集合
                    lllegalawarddetailbll.DeleteLllegalAwardList(entity.ID);
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(awarddata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string userid = rhInfo["awarduserid"].ToString(); //奖励用户
                        string username = rhInfo["awardusername"].ToString();
                        string deptid = rhInfo["awarddeptid"].ToString();//奖励用户部门
                        string deptname = rhInfo["awarddeptname"].ToString();
                        string points = rhInfo["awardpoints"].ToString();  //奖励积分
                        string money = rhInfo["awardmoney"].ToString(); //奖励金额
                        LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                        awardEntity.LLLEGALID = entity.ID;
                        awardEntity.USERID = userid; //奖励对象
                        awardEntity.USERNAME = username;
                        awardEntity.DEPTID = deptid;
                        awardEntity.DEPTNAME = deptname;
                        awardEntity.POINTS = !string.IsNullOrEmpty(points) ? int.Parse(points) : 0;
                        awardEntity.MONEY = !string.IsNullOrEmpty(money) ? Convert.ToDecimal(money) : 0;
                        lllegalawarddetailbll.SaveForm("", awardEntity);
                    }
                }
                #endregion

                #region 违章验收信息
                LllegalAcceptEntity aentity = new LllegalAcceptEntity();
                string acceptid = res.Contains("acceptid") ? dy.data.acceptid : "";  //验收id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    aentity = lllegalacceptbll.GetEntityByBid(lllegalid);
                }
                if (null != aentity)
                {
                    acceptid = aentity.ID;
                }
                aentity.LLLEGALID = entity.ID;
                aentity.ACCEPTPEOPLEID = res.Contains("acceptpeopleid") ? dy.data.acceptpeopleid : ""; //验收人id
                aentity.ACCEPTPEOPLE = res.Contains("acceptpeople") ? dy.data.acceptpeople : ""; //验收人姓名
                aentity.ACCEPTDEPTNAME = res.Contains("acceptdeptname") ? dy.data.acceptdeptname : ""; //验收部门名称
                aentity.ACCEPTDEPTCODE = res.Contains("acceptdeptcode") ? dy.data.acceptdeptcode : ""; //验收部门编码
                aentity.ISGRPACCEPT = res.Contains("isgrpaccept") ? dy.data.isgrpaccept : aentity.ISGRPACCEPT; //是否省公司验收
                aentity.CONFIRMUSERID = res.Contains("confirmuserid") ? dy.data.confirmuserid : (aentity.ISGRPACCEPT == "否" ? (!string.IsNullOrEmpty(aentity.CONFIRMUSERID) ? aentity.CONFIRMUSERID : curUser.UserId) : "");//验收确认人编号，默认登记人。
                aentity.CONFIRMUSERNAME = res.Contains("confirmusername") ? dy.data.confirmusername : (aentity.ISGRPACCEPT == "否" ? (!string.IsNullOrWhiteSpace(aentity.CONFIRMUSERNAME) ? aentity.CONFIRMUSERNAME : curUser.UserName) : "");//验收确认人姓名，默认登记人。
                if (addtype == "1" || addtype == "2")
                {
                    aentity.ACCEPTRESULT = res.Contains("acceptresult") ? dy.data.acceptresult : "";  //验收结果
                }
                aentity.ACCEPTMIND = res.Contains("acceptmind") ? dy.data.acceptmind : ""; //验收意见
                string accepttime = res.Contains("accepttime") ? dy.data.accepttime : "";
                if (!string.IsNullOrEmpty(accepttime))
                {
                    aentity.ACCEPTTIME = Convert.ToDateTime(accepttime);
                }
                if (string.IsNullOrEmpty(lllegalid))
                {
                    aentity.ACCEPTPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                aentity.ACCEPTPIC = !string.IsNullOrEmpty(aentity.ACCEPTPIC) ? aentity.ACCEPTPIC : Guid.NewGuid().ToString();
                UploadifyFile(aentity.ACCEPTPIC, "acceptpic", files);
                /********************************/
                //保存
                aentity.APPSIGN = AppSign; //移动端标记
                lllegalacceptbll.SaveForm(acceptid, aentity);
                #endregion
                #region  一次性提交 添加核准 及 考核信息
                if (addtype == "1")
                {
                    //违章整改
                    var changeEntity = lllegalreformbll.GetEntityByBid(lllegalid);
                    if (null != changeEntity)
                    {
                        changeEntity.REFORMSTATUS = "1";
                        //新增
                        lllegalreformbll.SaveForm(changeEntity.ID, changeEntity);
                    }
                    //违章验收
                    var acceptEntity = lllegalacceptbll.GetEntityByBid(lllegalid);
                    if (null != acceptEntity)
                    {
                        acceptEntity.ACCEPTRESULT = "1";
                        //新增
                        lllegalacceptbll.SaveForm(acceptEntity.ID, acceptEntity);
                    }

                    //违章核准信息
                    #region 违章核准信息
                    LllegalApproveEntity apentity = new LllegalApproveEntity();
                    string approvedate = res.Contains("approvedate") ? dy.data.approvedate : null;
                    if (!string.IsNullOrEmpty(approvedate))
                    {
                        apentity.APPROVEDATE = Convert.ToDateTime(approvedate);
                    }
                    apentity.LLLEGALID = entity.ID;
                    apentity.APPROVEDEPTCODE = res.Contains("approvedeptcode") ? dy.data.approvedeptcode : "";  //核准部门编码
                    apentity.APPROVEDEPTNAME = res.Contains("approvedeptname") ? dy.data.approvedeptname : "";  //核准部门名称
                    apentity.APPROVEPERSON = res.Contains("approveperson") ? dy.data.approveperson : "";  //核准人姓名
                    apentity.APPROVEPERSONID = res.Contains("approvepersonid") ? dy.data.approvepersonid : "";  //核准人id
                    apentity.APPROVEREASON = res.Contains("approvereason") ? dy.data.approvereason : "";  //核准意见
                    apentity.APPROVERESULT = res.Contains("approveresult") ? dy.data.approveresult : "1";  //核准情况
                    apentity.APPSIGN = AppSign; //移动端标记
                    lllegalapprovebll.SaveForm("", apentity);
                    #endregion
                    //提交流程

                    wfFlag = "1";//整改结束

                    participant = curUser.Account;


                    //提交流程
                    int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态

                        //添加分数到
                        lllegalregisterbll.AddLllegalScore(entity);
                    }
                }
                #endregion
                #region 非已整改违章提交
                else
                {
                    WfControlObj wfentity = new WfControlObj();
                    wfentity.businessid = entity.ID; //违章主键
                    wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
                    wfentity.argument2 = entity.LLLEGALTYPE;//违章类型
                    wfentity.argument3 = curUser.DeptId;//当前人所属部门
                    wfentity.argument4 = entity.LLLEGALTEAMCODE;//违章部门
                    wfentity.startflow = entity.FLOWSTATE;
                    //是否上报
                    if (entity.ISUPSAFETY == "1")
                    {
                        wfentity.submittype = "上报";
                    }
                    else
                    {
                        wfentity.submittype = "提交";
                        //不指定整改责任人
                        if (centity.ISAPPOINT == "0")
                        {
                            wfentity.submittype = "制定提交";
                        }
                    }
                    wfentity.rankid = null;
                    wfentity.user = curUser;
                    if (addtype == "2")
                    {
                        wfentity.mark = "省级违章流程";
                    }
                    else
                    {
                        wfentity.mark = "厂级违章流程";
                    }
                    wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                    //获取下一流程的操作人
                    WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                    //处理成功
                    if (result.code == WfCode.Sucess)
                    {
                        participant = result.actionperson;
                        wfFlag = result.wfflag;

                        //提交流程到下一节点
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, entity.ID, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态

                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(entity.ID);

                                #region 违章评分对象
                                if (tagName == "违章整改" || tagName == "流程结束")
                                {
                                    //添加分数到
                                    lllegalregisterbll.AddLllegalScore(entity);

                                }
                                #endregion
                            }
                        }
                        return new { code = 0, count = 0, info = result.message };
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = result.message };
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 保存违章信息  安全检查专用
        /// <summary>
        /// 保存违章信息  安全检查专用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveLllegal()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string addtype = dy.data.addtype; //新增类型  一般整改违章 0    立即整改违章 1  
                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                #region 违章登记信息
                LllegalRegisterEntity entity = new LllegalRegisterEntity();
                //违章 基本信息
                entity.ID = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                //新增时
                if (string.IsNullOrEmpty(lllegalid))
                {
                    entity.CREATEDEPTID = curUser.DeptId;
                    entity.CREATEDEPTNAME = curUser.DeptName;
                    if (!curUser.RoleName.Contains("省级"))
                    {
                        entity.BELONGDEPARTID = curUser.OrganizeId;
                        entity.BELONGDEPART = curUser.OrganizeName;
                    }
                    string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";
                    entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum)); //违章编号
                }
                entity.LLLEGALTYPE = res.Contains("lllegaltype") ? dy.data.lllegaltype : "";// 违章类型id
                string lllegaltime = res.Contains("lllegaltime") ? dy.data.lllegaltime : "";
                if (!string.IsNullOrEmpty(lllegaltime))
                {
                    entity.LLLEGALTIME = Convert.ToDateTime(lllegaltime);//违章时间
                }
                entity.VERIFYDEPTNAME = res.Contains("verifydeptname") ? dy.data.verifydeptname : "";//审核部门
                entity.VERIFYDEPTID = res.Contains("verifydeptid") ? dy.data.verifydeptid : "";//审核部门
                entity.LLLEGALLEVEL = res.Contains("lllegallevel") ? dy.data.lllegallevel : "";  //违章级别
                entity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : ""; //专业分类
                entity.LLLEGALPERSON = res.Contains("lllegalperson") ? dy.data.lllegalperson : ""; //违章人员姓名
                entity.LLLEGALPERSONID = res.Contains("lllegalpersonid") ? dy.data.lllegalpersonid : "";  //违章人员id
                entity.LLLEGALTEAM = res.Contains("lllegalteam") ? dy.data.lllegalteam : ""; //违章单位名称
                entity.LLLEGALTEAMCODE = res.Contains("lllegalteamcode") ? dy.data.lllegalteamcode : "";  //违章单位编码
                entity.LLLEGALDEPART = res.Contains("lllegaldepart") ? dy.data.lllegaldepart : "";  //违章责任单位名称
                entity.LLLEGALDEPARTCODE = res.Contains("lllegaldepartcode") ? dy.data.lllegaldepartcode : "";  //违章责任单位编码
                entity.LLLEGALDESCRIBE = res.Contains("lllegaldescribe") ? dy.data.lllegaldescribe : "";  //违章责任描述
                entity.LLLEGALADDRESS = res.Contains("lllegaladdress") ? dy.data.lllegaladdress : "";  //违章地点
                entity.REFORMREQUIRE = res.Contains("reformrequire") ? dy.data.reformrequire : "";  //整改要求
                entity.FLOWSTATE = res.Contains("flowstate") ? dy.data.flowstate : "";  //流程状态
                entity.ISEXPOSURE = res.Contains("isexposure") ? dy.data.isexposure : "";  //是否曝光
                entity.RESEVERID = res.Contains("reseverid") ? dy.data.reseverid : "";  //关联应用id
                entity.RESEVERTYPE = res.Contains("resevertype") ? dy.data.resevertype : "";  //关联应用类型
                entity.RESEVERONE = res.Contains("reseverone") ? dy.data.reseverone : "";  //关联应用类型
                entity.RESEVERTWO = res.Contains("resevertwo") ? dy.data.resevertwo : "";  //关联应用类型
                entity.RESEVERTHREE = res.Contains("reseverthree") ? dy.data.reseverthree : "";  //关联应用类型
                entity.ISUPSAFETY = res.Contains("isupsafety") ? dy.data.isupsafety : "";  //是否上报安全主管部门
                entity.ENGINEERID = res.Contains("engineerid") ? dy.data.engineerid : "";   //外包工程id
                entity.ENGINEERNAME = res.Contains("engineername") ? dy.data.engineername : "";   //外包工程

                entity.BELONGDEPARTID = res.Contains("belongdepartid") ? dy.data.belongdepartid : entity.BELONGDEPARTID;   //所属单位ID
                entity.BELONGDEPART = res.Contains("belongdepart") ? dy.data.belongdepart : entity.BELONGDEPART;   //所属单位
                //省级用户传2
                if (curUser.RoleName.Contains("省级"))
                {
                    addtype = "2";
                }
                entity.ADDTYPE = addtype; //新增方式
                entity.RESEVERFIVE = "";
                entity.RESEVERFOUR = "";
                //违章图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(lllegalid))
                {
                    entity.LLLEGALPIC = Guid.NewGuid().ToString(); //违章图片
                }
                //先删除图片
                DeleteFile(fileids);
                //上传违章图片
                entity.LLLEGALPIC = !string.IsNullOrEmpty(entity.LLLEGALPIC) ? entity.LLLEGALPIC : Guid.NewGuid().ToString();
                UploadifyFile(entity.LLLEGALPIC, "lllegalpic", files);
                /********************************/
                //新增
                lllegalregisterbll.SaveForm(lllegalid, entity);
                #endregion
                #region 创建主体流程
                //主键为空  创建主体流程
                if (string.IsNullOrEmpty(lllegalid))
                {
                    string workFlow = string.Empty;
                    if (addtype == "0" || addtype == "2") //一般整改
                    {
                        workFlow = "03";//违章处理
                    }
                    else  //立即整改  addtype :1 
                    {
                        workFlow = "04";//违章处理
                    }
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, curUser.UserId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                #endregion
                #region 违章整改信息
                LllegalReformEntity centity = new LllegalReformEntity();
                string reformid = res.Contains("reformid") ? dy.data.reformid : "";  //整改id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    centity = lllegalreformbll.GetEntityByBid(lllegalid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.LLLEGALID = entity.ID; //隐患编码 
                centity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人姓名
                centity.REFORMPEOPLEID = res.Contains("reformpeopleid") ? dy.data.reformpeopleid : "";  //整改人id
                centity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人联系方式
                centity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";  //整改部门编码
                centity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";  //验收部门编码
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : ""; //整改完成情况
                centity.REFORMMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";//整改措施 
                centity.ISAPPOINT = res.Contains("isappoint") ? dy.data.isappoint : null; //是否指定整改责任人
                centity.REFORMCHARGEPERSON = res.Contains("reformchargeperson") ? dy.data.reformchargeperson : ""; //整改责任负责人
                centity.REFORMCHARGEPERSONNAME = res.Contains("reformchargepersonname") ? dy.data.reformchargepersonname : ""; //整改责任负责人
                centity.REFORMCHARGEDEPTID = res.Contains("reformchargedeptid") ? dy.data.reformchargedeptid : ""; //指定整改责任部门
                centity.REFORMCHARGEDEPTNAME = res.Contains("reformchargedeptname") ? dy.data.reformchargedeptname : ""; //指定整改责任部门
                string reformdeadline = res.Contains("reformdeadline") ? dy.data.reformdeadline : null;//整改截至时间
                if (!string.IsNullOrEmpty(reformdeadline))
                {
                    centity.REFORMDEADLINE = Convert.ToDateTime(reformdeadline); //整改截至时间
                }
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;//整改结束时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                if (string.IsNullOrEmpty(lllegalid))//图片
                {
                    centity.REFORMPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                //上传违章整改图片
                centity.REFORMPIC = !string.IsNullOrEmpty(centity.REFORMPIC) ? centity.REFORMPIC : Guid.NewGuid().ToString();
                UploadifyFile(centity.REFORMPIC, "reformpic", files);

                centity.REFORMATTACHMENT = !string.IsNullOrEmpty(centity.REFORMATTACHMENT) ? centity.REFORMATTACHMENT : Guid.NewGuid().ToString();
                //上传整改附件
                UploadifyFile(centity.REFORMATTACHMENT, "attachment", files);
                /********************************/
                //新增
                lllegalreformbll.SaveForm(reformid, centity);
                #endregion
                #region 违章考核信息
                //新增关联责任人
                string punishdata = res.Contains("punishdata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.punishdata) : "";
                if (!string.IsNullOrEmpty(punishdata))
                {
                    //先删除关联责任人集合
                    lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");
                    //反序列化考核对象
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(punishdata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string assessobject = rhInfo["assessobject"].ToString();
                        string personinchargename = rhInfo["chargeperson"].ToString(); //关联责任人姓名
                        string personinchargeid = rhInfo["chargepersonid"].ToString();//关联责任人id
                        string performancepoint = rhInfo["performancepoint"].ToString(); // EHS绩效考核 
                        string economicspunish = rhInfo["economicspunish"].ToString(); // 经济处罚
                        string education = rhInfo["education"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString();//违章扣分
                        string awaitjob = rhInfo["awaitjob"].ToString();//待岗

                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.ASSESSOBJECT = assessobject; //考核对象
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = assessobject.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }
                #endregion
                #region 违章验收信息
                LllegalAcceptEntity aentity = new LllegalAcceptEntity();
                string acceptid = res.Contains("acceptid") ? dy.data.acceptid : "";  //验收id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    aentity = lllegalacceptbll.GetEntityByBid(lllegalid);
                }
                if (null != aentity)
                {
                    acceptid = aentity.ID;
                }
                aentity.LLLEGALID = entity.ID;
                aentity.ACCEPTPEOPLEID = res.Contains("acceptpeopleid") ? dy.data.acceptpeopleid : ""; //验收人id
                aentity.ACCEPTPEOPLE = res.Contains("acceptpeople") ? dy.data.acceptpeople : ""; //验收人姓名
                aentity.ACCEPTDEPTNAME = res.Contains("acceptdeptname") ? dy.data.acceptdeptname : ""; //验收部门名称
                aentity.ACCEPTDEPTCODE = res.Contains("acceptdeptcode") ? dy.data.acceptdeptcode : ""; //验收部门编码
                if (addtype == "1")
                {
                    aentity.ACCEPTRESULT = res.Contains("acceptresult") ? dy.data.acceptresult : "";  //验收结果
                }
                aentity.ACCEPTMIND = res.Contains("acceptmind") ? dy.data.acceptmind : ""; //验收意见
                string accepttime = res.Contains("accepttime") ? dy.data.accepttime : "";
                if (!string.IsNullOrEmpty(accepttime))
                {
                    aentity.ACCEPTTIME = Convert.ToDateTime(accepttime);
                }
                if (string.IsNullOrEmpty(lllegalid))
                {
                    aentity.ACCEPTPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                aentity.ACCEPTPIC = !string.IsNullOrEmpty(aentity.ACCEPTPIC) ? aentity.ACCEPTPIC : Guid.NewGuid().ToString();
                UploadifyFile(aentity.ACCEPTPIC, "acceptpic", files);
                /********************************/
                //保存
                lllegalacceptbll.SaveForm(acceptid, aentity);
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 违章核准
        /// <summary>
        /// 违章核准 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddApprovePush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            HttpFileCollection files = ctx.Request.Files;//上传的文件 
            try
            {
                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                string approveresult = res.Contains("approveresult") ? dy.data.approveresult : "";  //是否核准通过
                string wfFlag = string.Empty;  //流程标识
                string participant = string.Empty;  //获取流程下一节点的参与人员
                #region  保存违章信息
                LllegalRegisterEntity entity = new LllegalRegisterEntity();
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    entity = lllegalregisterbll.GetEntity(lllegalid);
                }
                if (null == entity)
                {
                    entity = new LllegalRegisterEntity();
                    entity.ID = lllegalid;  //主键
                }
                //违章 基本信息                
                entity.LLLEGALTYPE = res.Contains("lllegaltype") ? dy.data.lllegaltype : "";// 违章类型id
                entity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : ""; //专业分类
                string lllegaltime = res.Contains("lllegaltime") ? dy.data.lllegaltime : "";
                if (!string.IsNullOrEmpty(lllegaltime))
                {
                    entity.LLLEGALTIME = Convert.ToDateTime(lllegaltime);//违章时间
                }
                entity.LLLEGALLEVEL = res.Contains("lllegallevel") ? dy.data.lllegallevel : "";  //违章级别
                entity.LLLEGALPERSON = res.Contains("lllegalperson") ? dy.data.lllegalperson : ""; //违章人员姓名
                entity.LLLEGALPERSONID = res.Contains("lllegalpersonid") ? dy.data.lllegalpersonid : "";  //违章人员id
                entity.LLLEGALTEAM = res.Contains("lllegalteam") ? dy.data.lllegalteam : ""; //违章单位名称
                entity.LLLEGALTEAMCODE = res.Contains("lllegalteamcode") ? dy.data.lllegalteamcode : "";  //违章单位编码
                entity.LLLEGALDEPART = res.Contains("lllegaldepart") ? dy.data.lllegaldepart : "";  //违章责任单位名称
                entity.LLLEGALDEPARTCODE = res.Contains("lllegaldepartcode") ? dy.data.lllegaldepartcode : "";  //违章责任单位编码
                entity.LLLEGALDESCRIBE = res.Contains("lllegaldescribe") ? dy.data.lllegaldescribe : "";  //违章责任描述
                entity.LLLEGALADDRESS = res.Contains("lllegaladdress") ? dy.data.lllegaladdress : "";  //违章地点
                entity.REFORMREQUIRE = res.Contains("reformrequire") ? dy.data.reformrequire : "";  //整改要求
                entity.ISUPSAFETY = res.Contains("isupsafety") ? dy.data.isupsafety : "";  //是否上报安全主管部门
                entity.ISEXPOSURE = res.Contains("isexposure") ? dy.data.isexposure : "";  //是否曝光
                entity.ENGINEERID = res.Contains("engineerid") ? dy.data.engineerid : "";   //外包工程id
                entity.ENGINEERNAME = res.Contains("engineername") ? dy.data.engineername : "";   //外包工程
                //违章图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                //先删除图片
                DeleteFile(fileids);
                //上传违章图片
                entity.LLLEGALPIC = !string.IsNullOrEmpty(entity.LLLEGALPIC) ? entity.LLLEGALPIC : Guid.NewGuid().ToString();
                UploadifyFile(entity.LLLEGALPIC, "lllegalpic", files);
                /********************************/
                entity.RESEVERFIVE = "";
                entity.RESEVERFOUR = "";
                //新增
                lllegalregisterbll.SaveForm(lllegalid, entity);
                #endregion
                #region 违章整改信息
                LllegalReformEntity centity = null;
                string reformid = string.Empty;  //整改id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    centity = lllegalreformbll.GetEntityByBid(lllegalid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.LLLEGALID = entity.ID; //隐患编码 
                centity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人姓名
                centity.REFORMPEOPLEID = res.Contains("reformpeopleid") ? dy.data.reformpeopleid : "";  //整改人id
                centity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人联系方式
                centity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";  //整改部门编码
                centity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";  //验收部门编码
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : ""; //整改完成情况
                centity.REFORMMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";//整改措施 
                centity.ISAPPOINT = res.Contains("isappoint") ? dy.data.isappoint : null; //是否指定整改责任人
                centity.REFORMCHARGEPERSON = res.Contains("reformchargeperson") ? dy.data.reformchargeperson : ""; //整改责任负责人
                centity.REFORMCHARGEPERSONNAME = res.Contains("reformchargepersonname") ? dy.data.reformchargepersonname : ""; //整改责任负责人
                centity.REFORMCHARGEDEPTID = res.Contains("reformchargedeptid") ? dy.data.reformchargedeptid : ""; //指定整改责任部门
                centity.REFORMCHARGEDEPTNAME = res.Contains("reformchargedeptname") ? dy.data.reformchargedeptname : ""; //指定整改责任部门
                //整改截至时间
                string reformdeadline = res.Contains("reformdeadline") ? dy.data.reformdeadline : null;
                if (!string.IsNullOrEmpty(reformdeadline))
                {
                    centity.REFORMDEADLINE = Convert.ToDateTime(reformdeadline); //整改截至时间
                }
                //整改结束时间
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                //新增
                lllegalreformbll.SaveForm(reformid, centity);
                #endregion
                #region 保存核准基本信息
                LllegalPunishEntity psentity = new LllegalPunishEntity();
                #region 违章核准信息
                LllegalApproveEntity apentity = new LllegalApproveEntity();
                string approvedate = res.Contains("approvedate") ? dy.data.approvedate : null;
                if (!string.IsNullOrEmpty(approvedate))
                {
                    apentity.APPROVEDATE = Convert.ToDateTime(approvedate);
                }
                apentity.LLLEGALID = lllegalid;
                apentity.APPROVEDEPTCODE = res.Contains("approvedeptcode") ? dy.data.approvedeptcode : "";  //核准部门编码
                apentity.APPROVEDEPTNAME = res.Contains("approvedeptname") ? dy.data.approvedeptname : "";  //核准部门名称
                apentity.APPROVEPERSON = res.Contains("approveperson") ? dy.data.approveperson : "";  //核准人姓名
                apentity.APPROVEPERSONID = res.Contains("approvepersonid") ? dy.data.approvepersonid : "";  //核准人id
                apentity.APPROVEREASON = res.Contains("approvereason") ? dy.data.approvereason : "";  //核准意见
                apentity.APPROVERESULT = approveresult;   //核准情况
                apentity.APPSIGN = AppSign; //移动端标记
                #endregion

                //违章考核信息
                #region 违章考核信息
                string punishdata = res.Contains("punishdata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.punishdata) : "";
                if (!string.IsNullOrEmpty(punishdata))
                {
                    //先删除关联责任人集合
                    lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");
                    //反序列化考核对象
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(punishdata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string assessobject = rhInfo["assessobject"].ToString();
                        string personinchargename = rhInfo["chargeperson"].ToString(); //关联责任人姓名
                        string personinchargeid = rhInfo["chargepersonid"].ToString();//关联责任人id
                        string performancepoint = rhInfo["performancepoint"].ToString(); // EHS绩效考核 
                        string economicspunish = rhInfo["economicspunish"].ToString(); // 经济处罚
                        string education = rhInfo["education"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString();//违章扣分
                        string awaitjob = rhInfo["awaitjob"].ToString();//待岗

                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.ASSESSOBJECT = assessobject; //考核对象
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = assessobject.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }
                #endregion

                #region 违章奖励信息
                string awarddata = res.Contains("awarddata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.awarddata) : "";
                if (!string.IsNullOrEmpty(awarddata))
                {  //先删除关联集合
                    lllegalawarddetailbll.DeleteLllegalAwardList(entity.ID);
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(awarddata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string userid = rhInfo["awarduserid"].ToString(); //奖励用户
                        string username = rhInfo["awardusername"].ToString();
                        string deptid = rhInfo["awarddeptid"].ToString();//奖励用户部门
                        string deptname = rhInfo["awarddeptname"].ToString();
                        string points = rhInfo["awardpoints"].ToString();  //奖励积分
                        string money = rhInfo["awardmoney"].ToString(); //奖励金额
                        LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                        awardEntity.LLLEGALID = entity.ID;
                        awardEntity.USERID = userid; //奖励对象
                        awardEntity.USERNAME = username;
                        awardEntity.DEPTID = deptid;
                        awardEntity.DEPTNAME = deptname;
                        awardEntity.POINTS = !string.IsNullOrEmpty(points) ? int.Parse(points) : 0;
                        awardEntity.MONEY = !string.IsNullOrEmpty(money) ? Convert.ToDecimal(money) : 0;
                        lllegalawarddetailbll.SaveForm("", awardEntity);
                    }
                }
                #endregion

                #endregion
                #region 违章验收信息
                LllegalAcceptEntity aentity = null;
                string acceptid = string.Empty;
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    aentity = lllegalacceptbll.GetEntityByBid(lllegalid);
                }
                if (null != aentity)
                {
                    acceptid = aentity.ID;
                }
                aentity.ACCEPTPEOPLEID = res.Contains("acceptpeopleid") ? dy.data.acceptpeopleid : ""; //验收人id
                aentity.ACCEPTPEOPLE = res.Contains("acceptpeople") ? dy.data.acceptpeople : ""; //验收人姓名
                aentity.ACCEPTDEPTNAME = res.Contains("acceptdeptname") ? dy.data.acceptdeptname : ""; //验收部门名称
                aentity.ACCEPTDEPTCODE = res.Contains("acceptdeptcode") ? dy.data.acceptdeptcode : ""; //验收部门编码
                // aentity.ACCEPTRESULT = res.Contains("acceptresult") ? dy.data.acceptresult : "";  //验收结果
                aentity.ACCEPTMIND = res.Contains("acceptmind") ? dy.data.acceptmind : ""; //验收意见
                string accepttime = res.Contains("accepttime") ? dy.data.accepttime : "";
                if (!string.IsNullOrEmpty(accepttime))
                {
                    aentity.ACCEPTTIME = Convert.ToDateTime(accepttime);
                }
                //保存
                lllegalacceptbll.SaveForm(acceptid, aentity);
                #endregion
                #region 流程推进
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = lllegalid; //
                wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = entity.LLLEGALTYPE;//违章类型
                wfentity.argument3 = curUser.DeptId;//当前人所属部门
                wfentity.argument4 = entity.LLLEGALTEAMCODE;//违章部门
                wfentity.argument5 = entity.LLLEGALLEVEL;//违章级别
                wfentity.startflow = entity.FLOWSTATE;
                //上报，且存在上级部门
                if (entity.ISUPSAFETY == "1")
                {
                    wfentity.submittype = "上报";
                }
                else  //不上报，评估通过需要提交整改，评估不通过退回到登记
                {
                    /****判断当前人是否评估通过*****/
                    #region 判断当前人是否评估通过
                    //评估通过，则直接进行整改
                    if (apentity.APPROVERESULT == "1")
                    {
                        wfentity.submittype = "提交";
                        //不指定整改责任人
                        if (centity.ISAPPOINT == "0")
                        {
                            wfentity.submittype = "制定提交";
                        }
                        //判断是否是同级提交
                        bool ismajorpush = GetCurUserWfAuth(null, entity.FLOWSTATE, entity.FLOWSTATE, "厂级违章流程", "同级提交", entity.MAJORCLASSIFY, null, null, entity.LLLEGALLEVEL,lllegalid);
                        if (ismajorpush)
                        {
                            wfentity.submittype = "同级提交";
                        }
                    }
                    else  //评估不通过，退回到登记 
                    {
                        wfentity.submittype = "退回";
                    }
                    #endregion
                }
                wfentity.rankid = null;
                wfentity.user = curUser;
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //如果是更改状态
                        if (result.ischangestatus)
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, lllegalid, participant, wfFlag, curUser.UserId);
                            if (count > 0)
                            {
                                lllegalapprovebll.SaveForm("", apentity); //保存违章核准
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(lllegalid);
                                #region 违章评分对象
                                if (tagName == "违章整改" || tagName == "流程结束")
                                {
                                    lllegalregisterbll.AddLllegalScore(entity);//添加分数到
                                }
                                #endregion
                                return new { code = 0, count = 0, info = result.message };
                            }
                            else
                            {
                                return new { code = -1, count = 0, info = result.message };
                            }
                        }
                        else  //不更改状态的情况下
                        {
                            //保存违章核准
                            lllegalapprovebll.SaveForm("", apentity);//保存违章核准
                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, lllegalid, participant, curUser.UserId);
                            return new { code = 0, count = 0, info = result.message };
                        }
                    }
                    else
                    {
                        return new { code = 0, count = 0, info = result.message };
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message.ToString() };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 违章制定整改计划
        /// <summary>
        ///  制定整改计划
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object PlanReformPush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                #region 违章登记信息
                LllegalRegisterEntity entity = new LllegalRegisterEntity();
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    entity = lllegalregisterbll.GetEntity(lllegalid);
                }
                //违章 基本信息
                entity.ID = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                entity.LLLEGALTYPE = res.Contains("lllegaltype") ? dy.data.lllegaltype : "";// 违章类型id
                string lllegaltime = res.Contains("lllegaltime") ? dy.data.lllegaltime : "";
                if (!string.IsNullOrEmpty(lllegaltime))
                {
                    entity.LLLEGALTIME = Convert.ToDateTime(lllegaltime);//违章时间
                }
                entity.LLLEGALLEVEL = res.Contains("lllegallevel") ? dy.data.lllegallevel : "";  //违章级别
                entity.MAJORCLASSIFY = res.Contains("majorclassify") ? dy.data.majorclassify : ""; //专业分类
                entity.LLLEGALPERSON = res.Contains("lllegalperson") ? dy.data.lllegalperson : ""; //违章人员姓名
                entity.LLLEGALPERSONID = res.Contains("lllegalpersonid") ? dy.data.lllegalpersonid : "";  //违章人员id
                entity.LLLEGALTEAM = res.Contains("lllegalteam") ? dy.data.lllegalteam : ""; //违章单位名称
                entity.LLLEGALTEAMCODE = res.Contains("lllegalteamcode") ? dy.data.lllegalteamcode : "";  //违章单位编码
                entity.LLLEGALDEPART = res.Contains("lllegaldepart") ? dy.data.lllegaldepart : "";  //违章责任单位名称
                entity.LLLEGALDEPARTCODE = res.Contains("lllegaldepartcode") ? dy.data.lllegaldepartcode : "";  //违章责任单位编码
                entity.LLLEGALDESCRIBE = res.Contains("lllegaldescribe") ? dy.data.lllegaldescribe : "";  //违章责任描述
                entity.LLLEGALADDRESS = res.Contains("lllegaladdress") ? dy.data.lllegaladdress : "";  //违章地点
                entity.REFORMREQUIRE = res.Contains("reformrequire") ? dy.data.reformrequire : "";  //整改要求
                entity.FLOWSTATE = res.Contains("flowstate") ? dy.data.flowstate : "";  //流程状态
                entity.ISEXPOSURE = res.Contains("isexposure") ? dy.data.isexposure : "";  //是否曝光
                entity.RESEVERID = res.Contains("reseverid") ? dy.data.reseverid : "";  //关联应用id
                entity.RESEVERTYPE = res.Contains("resevertype") ? dy.data.resevertype : "";  //关联应用类型
                entity.RESEVERONE = res.Contains("reseverone") ? dy.data.reseverone : "";  //关联应用类型
                entity.RESEVERTWO = res.Contains("resevertwo") ? dy.data.resevertwo : "";  //关联应用类型
                entity.RESEVERTHREE = res.Contains("reseverthree") ? dy.data.reseverthree : "";  //关联应用类型
                entity.ISUPSAFETY = res.Contains("isupsafety") ? dy.data.isupsafety : "";  //是否上报安全主管部门
                entity.ENGINEERID = res.Contains("engineerid") ? dy.data.engineerid : "";   //外包工程id
                entity.ENGINEERNAME = res.Contains("engineername") ? dy.data.engineername : "";   //外包工程
                entity.RESEVERFIVE = "";
                entity.RESEVERFOUR = "";
                //违章图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(lllegalid))
                {
                    entity.LLLEGALPIC = Guid.NewGuid().ToString(); //违章图片
                }
                //先删除图片
                DeleteFile(fileids);
                //上传违章图片
                entity.LLLEGALPIC = !string.IsNullOrEmpty(entity.LLLEGALPIC) ? entity.LLLEGALPIC : Guid.NewGuid().ToString();
                UploadifyFile(entity.LLLEGALPIC, "lllegalpic", files);
                /********************************/
                //新增
                entity.APPSIGN = AppSign; //移动端标记
                lllegalregisterbll.SaveForm(lllegalid, entity);
                #endregion
                #region 违章整改信息
                LllegalReformEntity centity = new LllegalReformEntity();
                string reformid = res.Contains("reformid") ? dy.data.reformid : "";  //整改id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    centity = lllegalreformbll.GetEntityByBid(lllegalid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.LLLEGALID = entity.ID; //隐患编码 
                centity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人姓名
                centity.REFORMPEOPLEID = res.Contains("reformpeopleid") ? dy.data.reformpeopleid : "";  //整改人id
                centity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人联系方式
                centity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";  //整改部门编码
                centity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";  //验收部门编码
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : ""; //整改完成情况
                centity.REFORMMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";//整改措施 
                centity.ISAPPOINT = res.Contains("isappoint") ? dy.data.isappoint : null; //是否指定整改责任人
                centity.REFORMCHARGEPERSON = res.Contains("reformchargeperson") ? dy.data.reformchargeperson : ""; //整改责任负责人
                centity.REFORMCHARGEPERSONNAME = res.Contains("reformchargepersonname") ? dy.data.reformchargepersonname : ""; //整改责任负责人
                centity.REFORMCHARGEDEPTID = res.Contains("reformchargedeptid") ? dy.data.reformchargedeptid : ""; //指定整改责任部门
                centity.REFORMCHARGEDEPTNAME = res.Contains("reformchargedeptname") ? dy.data.reformchargedeptname : ""; //指定整改责任部门
                //整改截至时间
                string reformdeadline = res.Contains("reformdeadline") ? dy.data.reformdeadline : null;
                if (!string.IsNullOrEmpty(reformdeadline))
                {
                    centity.REFORMDEADLINE = Convert.ToDateTime(reformdeadline); //整改截至时间
                }
                //整改结束时间
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                //图片
                if (string.IsNullOrEmpty(lllegalid))
                {
                    centity.REFORMPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                //上传违章整改图片
                centity.REFORMPIC = !string.IsNullOrEmpty(centity.REFORMPIC) ? centity.REFORMPIC : Guid.NewGuid().ToString();
                UploadifyFile(centity.REFORMPIC, "reformpic", files);

                centity.REFORMATTACHMENT = !string.IsNullOrEmpty(centity.REFORMATTACHMENT) ? centity.REFORMATTACHMENT : Guid.NewGuid().ToString();
                //上传整改附件
                UploadifyFile(centity.REFORMATTACHMENT, "attachment", files);
                /********************************/
                //新增
                centity.APPSIGN = AppSign; //移动端标记
                lllegalreformbll.SaveForm(reformid, centity);

                #endregion
                #region 违章考核信息
                //新增关联责任人
                string punishdata = res.Contains("punishdata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.punishdata) : "";
                if (!string.IsNullOrEmpty(punishdata))
                {
                    //先删除关联责任人集合
                    lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");
                    //反序列化考核对象
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(punishdata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string assessobject = rhInfo["assessobject"].ToString();//考核类型
                        string personinchargename = rhInfo["chargeperson"].ToString(); //关联责任人姓名/单位
                        string personinchargeid = rhInfo["chargepersonid"].ToString();//关联责任人id/单位id
                        string performancepoint = rhInfo["performancepoint"].ToString(); // EHS绩效考核 
                        string economicspunish = rhInfo["economicspunish"].ToString(); // 经济处罚
                        string education = rhInfo["education"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString();//违章扣分
                        string awaitjob = rhInfo["awaitjob"].ToString();//待岗

                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.ASSESSOBJECT = assessobject; //考核对象
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = assessobject.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }
                #endregion
                #region 违章验收信息
                LllegalAcceptEntity aentity = new LllegalAcceptEntity();
                string acceptid = res.Contains("acceptid") ? dy.data.acceptid : "";  //验收id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    aentity = lllegalacceptbll.GetEntityByBid(lllegalid);
                }
                if (null != aentity)
                {
                    acceptid = aentity.ID;
                }
                aentity.LLLEGALID = entity.ID;
                aentity.ACCEPTPEOPLEID = res.Contains("acceptpeopleid") ? dy.data.acceptpeopleid : ""; //验收人id
                aentity.ACCEPTPEOPLE = res.Contains("acceptpeople") ? dy.data.acceptpeople : ""; //验收人姓名
                aentity.ACCEPTDEPTNAME = res.Contains("acceptdeptname") ? dy.data.acceptdeptname : ""; //验收部门名称
                aentity.ACCEPTDEPTCODE = res.Contains("acceptdeptcode") ? dy.data.acceptdeptcode : ""; //验收部门编码
                string accepttime = res.Contains("accepttime") ? dy.data.accepttime : "";
                if (!string.IsNullOrEmpty(accepttime))
                {
                    aentity.ACCEPTTIME = Convert.ToDateTime(accepttime);
                }
                if (string.IsNullOrEmpty(lllegalid))
                {
                    aentity.ACCEPTPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                aentity.ACCEPTPIC = !string.IsNullOrEmpty(aentity.ACCEPTPIC) ? aentity.ACCEPTPIC : Guid.NewGuid().ToString();
                UploadifyFile(aentity.ACCEPTPIC, "acceptpic", files);
                /********************************/
                //保存
                aentity.APPSIGN = AppSign; //移动端标记
                lllegalacceptbll.SaveForm(acceptid, aentity);
                #endregion
                #region 违章流程推进
                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                string participant = string.Empty;
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = lllegalid; //
                wfentity.startflow = "制定整改计划";
                wfentity.submittype = "提交";
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = entity.LLLEGALTYPE;//违章类型
                wfentity.argument3 = curUser.DeptId;//当前人所属部门
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, lllegalid, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态

                            string tagName = htworkflowbll.QueryTagNameByCurrentWF(lllegalid);

                            #region 违章评分对象
                            if (tagName == "违章整改" || tagName == "流程结束")
                            {
                                //添加分数到
                                lllegalregisterbll.AddLllegalScore(entity);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = result.message };
                    }
                    return new { code = 0, count = 0, info = result.message };
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
        }
        #endregion

        #region 违章转交制定整改计划/违章整改流程
        /// <summary>
        /// 违章转交制定整改计划/违章整改流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public object LllegalDeliverPlan()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string participant = string.Empty;
                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                string reqtype = res.Contains("reqtype") ? dy.data.reqtype : ""; //1 验收转交
                //违章基本信息
                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(lllegalid);

                if (!string.IsNullOrEmpty(reqtype))
                {
                    /********验收信息************/
                    if (reqtype == "1")
                    {
                        var aentity = lllegalacceptbll.GetEntityByBid(lllegalid);
                        aentity.ACCEPTPEOPLEID = res.Contains("acceptpeopleid") ? dy.data.acceptpeopleid : ""; //验收人id
                        aentity.ACCEPTPEOPLE = res.Contains("acceptpeople") ? dy.data.acceptpeople : ""; //验收人姓名
                        aentity.ACCEPTDEPTNAME = res.Contains("acceptdeptname") ? dy.data.acceptdeptname : ""; //验收部门名称
                        aentity.ACCEPTDEPTCODE = res.Contains("acceptdeptcode") ? dy.data.acceptdeptcode : ""; //验收部门编码
                        lllegalacceptbll.SaveForm(aentity.ID, aentity);
                    }
                }
                else
                {
                    /********整改信息************/
                    //更新整改信息
                    if (!string.IsNullOrEmpty(lllegalid))
                    {
                        var reformEntity = lllegalreformbll.GetEntityByBid(lllegalid);
                        reformEntity.REFORMCHARGEDEPTID = res.Contains("reformchargedeptid") ? dy.data.reformchargedeptid : ""; //指定整改责任部门
                        reformEntity.REFORMCHARGEDEPTNAME = res.Contains("reformchargedeptname") ? dy.data.reformchargedeptname : ""; //指定整改责任部门
                        reformEntity.REFORMCHARGEPERSON = res.Contains("reformchargeperson") ? dy.data.reformchargeperson : ""; //整改责任负责人
                        reformEntity.REFORMCHARGEPERSONNAME = res.Contains("reformchargepersonname") ? dy.data.reformchargepersonname : ""; //整改责任负责人
                        reformEntity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人姓名
                        reformEntity.REFORMPEOPLEID = res.Contains("reformpeopleid") ? dy.data.reformpeopleid : "";  //整改人id
                        reformEntity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";//整改部门名称
                        reformEntity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : ""; //整改部门编码
                        reformEntity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人联系方式
                        lllegalreformbll.SaveForm(reformEntity.ID, reformEntity);
                    }
                }

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = lllegalid; //
                wfentity.startflow = entity.FLOWSTATE;
                wfentity.endflow = entity.FLOWSTATE;
                wfentity.submittype = "转交";
                wfentity.rankid = null;
                wfentity.user = curUser;
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //不更改状态
                        if (!result.ischangestatus)
                        {
                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, lllegalid, participant, curUser.UserId);
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "请联系系统管理员，添加当前流程下的参与者!" };
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 违章延期申请、审核、审批
        /// <summary>
        /// 违章延期申请入口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object LllegalDelayApply([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string actiondo = res.Contains("actiondo") ? dy.data.actiondo : string.Empty; //动作
                string keyValue = res.Contains("lllegalid") ? dy.data.lllegalid : string.Empty;  //违章主键  
                string handleid = res.Contains("handleid") ? dy.data.handleid : string.Empty;  //处理id  
                string postponereason = res.Contains("applyreason") ? dy.data.applyreason : string.Empty; //申请原因
                string postponedays = res.Contains("postponedays") ? dy.data.postponedays : string.Empty; //申请天数
                string postponeresult = res.Contains("postponeresult") ? dy.data.postponeresult : string.Empty; //审批结果
                LllegalRegisterEntity bentity = lllegalregisterbll.GetEntity(keyValue);  //获取违章信息
                var centity = lllegalreformbll.GetEntityByBid(keyValue); //根据keyValue获取整改对象
                string nextName = string.Empty;
                bool isupdate = false;
                string wfFlag = string.Empty;
                string participant = string.Empty;
                //保存申请记录
                LllegalExtensionEntity exentity = new LllegalExtensionEntity();
                WfControlResult result = new WfControlResult();
                WfControlObj wfentity = new WfControlObj(); //条件
                wfentity.businessid = keyValue; //
                wfentity.argument1 = bentity.MAJORCLASSIFY;
                wfentity.argument2 = bentity.LLLEGALTYPE;
                wfentity.submittype = "提交";
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.mark = "违章整改延期";
                wfentity.organizeid = bentity.BELONGDEPARTID; //对应电厂id
                if (actiondo == "apply")//通过延期申请来区分
                {
                    wfentity.startflow = "整改延期申请";
                    if (!string.IsNullOrEmpty(postponedays))
                    {
                        centity.POSTPONEDAYS = int.Parse(postponedays.ToString());
                    }
                }
                else //审批过程
                {
                    wfentity.startflow = "整改延期审批";
                }
                if (postponeresult == "0") //不通过
                {
                    centity.APPLICATIONSTATUS = "-1"; //延期申请失败
                    //UserEntity changeUser = userbll.GetEntity(centity.REFORMPEOPLEID); //延期失败保存整改人相关信息到result,用于极光推送
                    //if (null != changeUser)
                    //{
                    //    result.actionperson = changeUser.Account;
                    //    result.username = centity.REFORMPEOPLE;
                    //    result.deptname = centity.REFORMDEPTNAME;
                    //    result.deptid = changeUser.DepartmentId;
                    //    result.deptcode = centity.REFORMDEPTCODE;
                    //}
                    string[] userids = centity.REFORMPEOPLEID.Split(',');
                    DataTable userdt = userbll.GetUserTable(userids);
                    foreach (DataRow row in userdt.Rows)
                    {
                        result.actionperson += row["account"].ToString()+",";
                        result.username += row["realname"].ToString() + ",";
                        if (!result.deptname.Contains(row["deptname"].ToString()))
                        {
                            result.deptname += row["deptname"].ToString() + ",";
                        }
                        if (!result.deptid.Contains(row["departmentid"].ToString()))
                        {
                            result.deptid += row["departmentid"].ToString() + ",";
                        }
                        if (!result.deptcode.Contains(row["departmentcode"].ToString()))
                        {
                            result.deptcode += row["departmentcode"].ToString() + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(result.actionperson))
                    {
                        result.actionperson = result.actionperson.Substring(0, result.actionperson.Length - 1);
                    }
                    if (!string.IsNullOrEmpty(result.username))
                    {
                        result.username = result.username.Substring(0, result.username.Length - 1);
                    }
                    if (!string.IsNullOrEmpty(result.deptname))
                    {
                     
                        result.deptname = result.deptname.Substring(0, result.deptname.Length - 1);
                    }
                    if (!string.IsNullOrEmpty(result.deptid))
                    {
                        result.deptid = result.deptid.Substring(0, result.deptid.Length - 1);
                    }
                    if (!string.IsNullOrEmpty(result.deptcode))
                    {
                        result.deptcode = result.deptcode.Substring(0, result.deptcode.Length - 1);
                    }
                }
                else   //通过，包括申请、审批
                {
                    result = wfcontrolbll.GetWfControl(wfentity); //获取下一流程的操作人
                    //处理成功
                    if (result.code == WfCode.Sucess)
                    {
                        participant = result.actionperson;
                        wfFlag = result.wfflag;
                        centity.POSTPONEPERSON = "," + participant + ",";  // 用于当前人账户判断是否具有操作其权限
                        if (!string.IsNullOrEmpty(postponedays))
                        {
                            centity.POSTPONEDAYS = int.Parse(postponedays); //申请天数
                        }
                        centity.POSTPONEDEPT = result.deptcode;  //审批部门Code
                        centity.POSTPONEDEPTNAME = result.deptname;  //审批部门名称
                        centity.POSTPONEPERSONNAME = result.username;
                        centity.APPLICATIONSTATUS = wfFlag;
                        if (wfFlag == "2") { isupdate = true; }  //是否更新时间，累加天数
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = result.message };
                    }
                    //如果安环部、生技部审批通过，则更改整改截至时间、验收时间，增加相应的整改天数
                    if (isupdate)
                    {
                        //重新赋值整改截至时间
                        centity.REFORMDEADLINE = centity.REFORMDEADLINE.Value.AddDays(centity.POSTPONEDAYS.Value);
                        //更新验收时间
                        LllegalAcceptEntity aEntity = lllegalacceptbll.GetEntityByBid(keyValue);
                        if (null != aEntity.ACCEPTTIME)
                        {
                            aEntity.ACCEPTTIME = aEntity.ACCEPTTIME.Value.AddDays(centity.POSTPONEDAYS.Value);
                        }
                        lllegalacceptbll.SaveForm(aEntity.ID, aEntity);
                        exentity.HANDLESIGN = "1"; //成功标记
                    }
                }
                centity.APPSIGN = AppSign; //移动端标记
                //更新整改信息
                lllegalreformbll.SaveForm(centity.ID, centity); //更新延期设置

                //保存申请及申请记录
                exentity.LLLEGALID = keyValue;
                exentity.HANDLEDATE = DateTime.Now;
                if (null != centity.POSTPONEDAYS)
                {
                    exentity.POSTPONEDAYS = centity.POSTPONEDAYS.ToString();
                }
                exentity.HANDLEUSERID = curUser.UserId;
                exentity.HANDLEUSERNAME = curUser.UserName;
                exentity.HANDLEDEPTCODE = curUser.DeptCode;
                exentity.HANDLEDEPTNAME = curUser.DeptName;
                if (actiondo == "apply")
                {
                    exentity.HANDLETYPE = "0";  //申请
                    nextName = "整改延期审批";
                }
                else
                {
                    //成功
                    if (wfFlag == "2" && postponeresult == "1")
                    {
                        exentity.HANDLETYPE = wfFlag;  //处理类型 0 申请 1 审批 2 流程结束    wfFlag状态返回 2 时表示整改延期完成
                    }
                    //审批中
                    else if (wfFlag != "2" && postponeresult == "1")
                    {
                        exentity.HANDLETYPE = "1";  //审批中

                        nextName = "整改延期审批";
                    }
                    else //失败
                    {
                        if (postponeresult == "0")
                        {
                            exentity.HANDLETYPE = "-1";  //失败

                            nextName = "整改延期退回";
                        }
                    }
                }
                exentity.HANDLEID = !string.IsNullOrEmpty(handleid) ? handleid : DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
                if (exentity.HANDLETYPE == "0")
                {
                    exentity.POSTPONEREASON = postponereason; //申请下 为申请理由
                }
                else
                {
                    exentity.POSTPONEOPINION = postponereason; //审核下 为审核意见
                }
                exentity.POSTPONERESULT = postponeresult;  //申请结果
                exentity.APPSIGN = AppSign; //移动端标记
                lllegalextensionbll.SaveForm("", exentity);

                //极光推送
                htworkflowbll.PushMessageForWorkFlow("违章管理流程", nextName, wfentity, result);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "处理失败" };
            }
            return new { code = 0, count = 0, info = "处理成功" };
        }
        #endregion

        #region 违章整改
        /// <summary>
        /// 违章整改
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddReformPush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string participant = string.Empty;
                string wfFlag = string.Empty;  //流程标识
                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                string isback = res.Contains("isback") ? dy.data.isback : ""; //是否回退  // 是/ 否
                string backreason = res.Contains("backreason") ? dy.data.backreason : "";   //回退原因
                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(lllegalid);
                if (null != entity)
                {
                    entity.RESEVERFOUR = isback;
                    entity.RESEVERFIVE = backreason;
                    lllegalregisterbll.SaveForm(lllegalid, entity); //更新
                }
                string acceptpeopleid = res.Contains("acceptpeopleid") ? dy.data.acceptpeopleid : "";  //验收人id
                #region 违章整改信息
                LllegalReformEntity centity = new LllegalReformEntity();
                string reformid = "";  //整改id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    centity = lllegalreformbll.GetEntityByBid(lllegalid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.LLLEGALID = lllegalid; //违章id 
                centity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人姓名
                centity.REFORMPEOPLEID = res.Contains("reformpeopleid") ? dy.data.reformpeopleid : "";  //整改人id
                centity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人联系方式
                centity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";  //整改部门编码
                centity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";  //验收部门编码
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : ""; //整改完成情况
                centity.REFORMMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";//整改措施 
                string reformdeadline = res.Contains("reformdeadline") ? dy.data.reformdeadline : null; //整改截至时间
                if (!string.IsNullOrEmpty(reformdeadline))
                {
                    centity.REFORMDEADLINE = Convert.ToDateTime(reformdeadline); //整改截至时间
                }
                //整改结束时间
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                //违章整改图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                //先删除图片
                DeleteFile(fileids);

                /**********图片部分缺省**********/
                //上传违章整改图片
                //图片
                if (string.IsNullOrEmpty(centity.REFORMPIC))
                {
                    centity.REFORMPIC = Guid.NewGuid().ToString();
                }
                UploadifyFile(centity.REFORMPIC, "reformpic", files);

                centity.REFORMATTACHMENT = !string.IsNullOrEmpty(centity.REFORMATTACHMENT) ? centity.REFORMATTACHMENT : Guid.NewGuid().ToString();
                //上传整改附件
                UploadifyFile(centity.REFORMATTACHMENT, "attachment", files);
                /********************************/
                //更改
                centity.APPSIGN = AppSign; //移动端标记
                lllegalreformbll.SaveForm(reformid, centity);
                #endregion
                #region 流程推进
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = lllegalid;
                wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = entity.LLLEGALTYPE;//违章类型
                wfentity.argument3 = curUser.DeptId;//当前人所属部门
                wfentity.startflow = entity.FLOWSTATE;
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                //退回 
                if (isback == "是")
                {
                    //历史记录
                    var reformitem = lllegalreformbll.GetHistoryList(lllegalid).ToList();
                    //如果未整改可以退回
                    if (reformitem.Count() == 0)
                    {
                        wfentity.submittype = "退回";
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "整改过后的违章无法再次退回" };
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
                        //提交流程到下一节点
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, lllegalid, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                            }
                        }
                        else
                        {
                            return new { code = -1, count = 0, info = "请联系系统管理员，确认提交问题!" };
                        }
                    }
                    else
                    {
                        //获取
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, lllegalid, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(lllegalid);

                        #region 当前还处于违章整改阶段
                        if (tagName == "违章整改")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            centity.REFORMPEOPLE = userentity.RealName;
                            centity.REFORMPEOPLEID = userentity.UserId;
                            centity.REFORMDEPTNAME = userentity.DeptName;
                            centity.REFORMDEPTCODE = userentity.DepartmentCode;
                            centity.REFORMTEL = userentity.Telephone;
                            lllegalreformbll.SaveForm(centity.ID, centity);
                        }
                        #endregion
                    }
                }
                //非自动处理的流程
                else if (result.code == WfCode.NoAutoHandle)
                {
                    bool isupdate = false;//是否更改流程状态
                    //退回操作  单独处理
                    if (isback == "是")
                    {
                        DataTable dt = htworkflowbll.GetBackFlowObjectByKey(lllegalid);

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
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, lllegalid, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                            }
                            result.message = "处理成功";
                            result.code = WfCode.Sucess;
                        }
                    }
                    else
                    {
                        //不更改流程状态下
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, lllegalid, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(lllegalid);
                        #region 当前还处于违章整改阶段
                        if (tagName == "违章整改")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            centity.REFORMPEOPLE = userentity.RealName;
                            centity.REFORMPEOPLEID = userentity.UserId;
                            centity.REFORMDEPTNAME = userentity.DeptName;
                            centity.REFORMDEPTCODE = userentity.DepartmentCode;
                            centity.REFORMTEL = userentity.Telephone;
                            lllegalreformbll.SaveForm(centity.ID, centity);
                        }
                        #endregion

                        result.message = "处理成功";
                        result.code = WfCode.Sucess;
                    }
                }
                if (result.code == WfCode.Sucess)
                {
                    return new { code = 0, count = 0, info = result.message };
                }
                else //其他返回状态
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }
        }
        #endregion

        #region  违章验收
        /// <summary>
        /// 违章验收
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddAcceptPush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            WfControlResult result = new WfControlResult();
            HttpFileCollection files = ctx.Request.Files;//上传的文件 
            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)
            try
            {
                #region 违章验收信息
                LllegalRegisterEntity baseEntity = lllegalregisterbll.GetEntity(lllegalid); //违章基本信息
                LllegalAcceptEntity aentity = new LllegalAcceptEntity();
                string acceptid = res.Contains("acceptid") ? dy.data.acceptid : "";  //验收id
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    aentity = lllegalacceptbll.GetEntityByBid(lllegalid);
                }
                if (null != aentity)
                {
                    acceptid = aentity.ID;
                }
                aentity.ISGRPACCEPT = res.Contains("isgrpaccept") ? dy.data.isgrpaccept : aentity.ISGRPACCEPT; //是否省公司验收
                aentity.CONFIRMUSERID = res.Contains("confirmuserid") ? dy.data.confirmuserid : aentity.CONFIRMUSERID; //验收确认人id
                aentity.CONFIRMUSERNAME = res.Contains("confirmusername") ? dy.data.confirmusername : aentity.CONFIRMUSERNAME; //验收确认人姓名
                aentity.ACCEPTPEOPLEID = res.Contains("acceptpeopleid") ? dy.data.acceptpeopleid : ""; //验收人id
                aentity.ACCEPTPEOPLE = res.Contains("acceptpeople") ? dy.data.acceptpeople : ""; //验收人姓名
                aentity.ACCEPTDEPTNAME = res.Contains("acceptdeptname") ? dy.data.acceptdeptname : ""; //验收部门名称
                aentity.ACCEPTDEPTCODE = res.Contains("acceptdeptcode") ? dy.data.acceptdeptcode : ""; //验收部门编码
                aentity.ACCEPTRESULT = res.Contains("acceptresult") ? dy.data.acceptresult : "";  //验收结果
                aentity.ACCEPTMIND = res.Contains("acceptmind") ? dy.data.acceptmind : ""; //验收意见
                string accepttime = res.Contains("accepttime") ? dy.data.accepttime : "";
                if (!string.IsNullOrEmpty(accepttime))
                {
                    aentity.ACCEPTTIME = Convert.ToDateTime(accepttime);
                }

                //违章验收图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件

                //先删除图片
                DeleteFile(fileids);
                if (string.IsNullOrEmpty(aentity.ACCEPTPIC))
                {
                    aentity.ACCEPTPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                UploadifyFile(aentity.ACCEPTPIC, "acceptpic", files);
                /********************************/
                #endregion
                #region 流程推进
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = lllegalid;
                wfentity.argument1 = baseEntity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = baseEntity.LLLEGALTYPE;//违章类型
                wfentity.argument3 = curUser.DeptId;//当前人所属部门
                wfentity.startflow = baseEntity.FLOWSTATE;
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.BELONGDEPARTID; //对应电厂id
                //省级
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                //验收通过
                if (aentity.ACCEPTRESULT == "1")
                {
                    wfentity.submittype = "提交";
                }
                else //验收不通过
                {
                    wfentity.submittype = "退回";
                }

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                //返回操作结果成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, lllegalid, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //添加违章验收记录
                            aentity.APPSIGN = AppSign; //移动端标记
                            lllegalacceptbll.SaveForm(acceptid, aentity);

                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态

                            //退回则重新添加验收记录
                            if (wfentity.submittype == "退回")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(lllegalid);
                                //验收记录
                                LllegalAcceptEntity cptEntity = new LllegalAcceptEntity();
                                cptEntity = aentity;
                                cptEntity.ID = null;
                                cptEntity.CREATEDATE = DateTime.Now;
                                cptEntity.MODIFYDATE = DateTime.Now;
                                cptEntity.ACCEPTRESULT = null;
                                cptEntity.ACCEPTMIND = null;
                                cptEntity.ACCEPTPIC = null;
                                lllegalacceptbll.SaveForm("", cptEntity);

                                if (tagName == "违章整改")
                                {
                                    LllegalReformEntity reformEntity = lllegalreformbll.GetEntityByBid(lllegalid);
                                    LllegalReformEntity newEntity = new LllegalReformEntity();
                                    newEntity = reformEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.MODIFYUSERID = curUser.UserId;
                                    newEntity.MODIFYUSERNAME = curUser.UserName;
                                    newEntity.REFORMPIC = string.Empty; //重新生成图片GUID
                                    newEntity.REFORMATTACHMENT = string.Empty; //整改附件
                                    newEntity.REFORMSTATUS = string.Empty; //整改完成情况
                                    newEntity.REFORMMEASURE = string.Empty; //整改具体措施
                                    newEntity.REFORMFINISHDATE = null; //整改完成时间
                                    newEntity.ID = "";
                                    lllegalreformbll.SaveForm("", newEntity);
                                }
                            }

                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = result.message };
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
            return new { code = 0, count = 0, info = result.message };
        }
        #endregion

        #region  违章验收确认
        /// <summary>
        /// 违章验收
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddConfirmPush()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)
            WfControlResult result = new WfControlResult();
            try
            {
                //违章基本信息
                LllegalRegisterEntity baseEntity = lllegalregisterbll.GetEntity(lllegalid);
                #region 验收确认信息
                LllegalConfirmEntity confirmEntity = new LllegalConfirmEntity();
                if (!string.IsNullOrWhiteSpace(lllegalid))
                {
                    confirmEntity = lllegalconfirmbll.GetEntityByBid(lllegalid);
                }
                if (confirmEntity == null)
                {
                    confirmEntity = new LllegalConfirmEntity();
                    confirmEntity.LLLEGALID = lllegalid;
                }
                confirmEntity.CONFIRMRESULT = res.Contains("confirmresult") ? dy.data.confirmresult : "";
                confirmEntity.CONFIRMMIND = res.Contains("confirmmind") ? dy.data.confirmmind : "";
                confirmEntity.CONFIRMPEOPLEID = res.Contains("confirmpeopleid") ? dy.data.confirmpeopleid : ""; //确认人id
                confirmEntity.CONFIRMPEOPLE = res.Contains("confirmpeople") ? dy.data.confirmpeople : ""; //确认人姓名
                confirmEntity.CONFIRMDEPTNAME = res.Contains("confirmdeptname") ? dy.data.confirmdeptname : ""; //确认部门名称
                confirmEntity.CONFIRMDEPTCODE = res.Contains("confirmdeptcode") ? dy.data.confirmdeptcode : ""; //确认部门编码
                string confirmtime = res.Contains("confirmtime") ? dy.data.confirmtime : ""; //确认时间
                if (!string.IsNullOrEmpty(confirmtime))
                {
                    confirmEntity.CONFIRMTIME = Convert.ToDateTime(confirmtime);
                }
                confirmEntity.MODIFYDATE = DateTime.Now;
                confirmEntity.MODIFYUSERID = curUser.UserId;
                confirmEntity.MODIFYUSERNAME = curUser.UserName;
                #endregion
                #region 流程推进
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = lllegalid;
                wfentity.argument1 = baseEntity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = baseEntity.LLLEGALTYPE;//违章类型
                wfentity.argument3 = curUser.DeptId;//当前人所属部门
                wfentity.startflow = baseEntity.FLOWSTATE;
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.BELONGDEPARTID; //对应电厂id
                //厂级
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                //验收通过
                if (confirmEntity.CONFIRMRESULT == "1")
                {
                    wfentity.submittype = "提交";
                }
                else //验收不通过
                {
                    wfentity.submittype = "退回";
                }
                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);
                //返回操作结果成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, lllegalid, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                            //添加验收确认信息
                            confirmEntity.APPSIGN = AppSign; //移动端标记
                            lllegalconfirmbll.SaveForm(confirmEntity.ID, confirmEntity);

                            //退回则重新添加验收确认记录
                            if (wfentity.submittype == "退回")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(lllegalid);

                                if (tagName == "违章整改")
                                {
                                    //整改记录
                                    LllegalReformEntity reformEntity = lllegalreformbll.GetEntityByBid(lllegalid);
                                    LllegalReformEntity newEntity = new LllegalReformEntity();
                                    newEntity = reformEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.MODIFYUSERID = curUser.UserId;
                                    newEntity.MODIFYUSERNAME = curUser.UserName;
                                    newEntity.REFORMPIC = string.Empty; //重新生成图片GUID
                                    newEntity.REFORMATTACHMENT = string.Empty; //整改附件
                                    newEntity.REFORMSTATUS = string.Empty; //整改完成情况
                                    newEntity.REFORMMEASURE = string.Empty; //整改具体措施
                                    newEntity.REFORMFINISHDATE = null; //整改完成时间
                                    newEntity.ID = "";
                                    lllegalreformbll.SaveForm("", newEntity);
                                }
                                //验收记录
                                LllegalAcceptEntity acceptEntity = lllegalacceptbll.GetEntityByBid(lllegalid);
                                LllegalAcceptEntity newacceptEntity = new LllegalAcceptEntity();
                                newacceptEntity = acceptEntity;
                                newacceptEntity.ID = null;
                                newacceptEntity.CREATEDATE = DateTime.Now;
                                newacceptEntity.MODIFYDATE = DateTime.Now;
                                newacceptEntity.ACCEPTRESULT = null;
                                newacceptEntity.ACCEPTMIND = null;
                                newacceptEntity.ACCEPTPIC = null;
                                lllegalacceptbll.SaveForm("", newacceptEntity);

                                //验收确认记录
                                LllegalConfirmEntity cptEntity = new LllegalConfirmEntity();
                                cptEntity = confirmEntity;
                                cptEntity.ID = null;
                                cptEntity.CREATEDATE = DateTime.Now;
                                cptEntity.MODIFYDATE = DateTime.Now;
                                cptEntity.CONFIRMRESULT = null;
                                cptEntity.CONFIRMMIND = null;
                                lllegalconfirmbll.SaveForm("", cptEntity);
                            }
                        }
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
            return new { code = 0, count = 0, info = result.message };
        }
        #endregion
        #endregion

        #region 设置默认项
        /// <summary>
        ///  //设置默认项
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveDefaultOptions()
        {
            string res = HttpContext.Current.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                DefaultDataSettingBLL defaultdatasettingbll = new DefaultDataSettingBLL();
                string defaultkey = res.Contains("defaultkey") ? dy.data.defaultkey : "";  //键
                string defaultvalue = res.Contains("defaultvalue") ? dy.data.defaultvalue : "";  //值
                string defaultname = res.Contains("defaultname") ? dy.data.defaultname : "";  //属性名称
                string defaultmark = res.Contains("defaultmark") ? dy.data.defaultmark : "";  //属性标记
                if (!string.IsNullOrEmpty(defaultmark))
                {
                    var list = defaultdatasettingbll.GetList(curUser.UserId).Where(p => p.DEFAULTMARK == defaultmark).ToList();
                    if (list.Count() > 0)
                    {
                        foreach (DefaultDataSettingEntity mode in list)
                        {
                            defaultdatasettingbll.RemoveForm(mode.ID);
                        }
                    }
                }
                DefaultDataSettingEntity entity = new DefaultDataSettingEntity();
                entity.DEFAULTKEY = defaultkey;
                entity.DEFAULTVALUE = defaultvalue;
                entity.DEFAULTNAME = defaultname;
                entity.DEFAULTMARK = defaultmark;
                entity.USERID = curUser.UserId;
                defaultdatasettingbll.SaveForm("", entity);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 获取默认项列表
        /// <summary>
        /// 获取默认项列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDefaultOptionsList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            DefaultDataSettingBLL defaultdatasettingbll = new DefaultDataSettingBLL();
            List<object> data = new List<object>(); //返回的结果集合
            List<DefaultDataSettingEntity> list = defaultdatasettingbll.GetList(curUser.UserId).ToList(); //排序
            foreach (DefaultDataSettingEntity entity in list)
            {
                data.Add(new { defaultkey = entity.DEFAULTKEY, defaultvalue = entity.DEFAULTVALUE, defaultname = entity.DEFAULTNAME, defaultmark = entity.DEFAULTMARK });
            }
            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
        }
        #endregion


        #region 班组端

        #region 终端首页任务看板-今日工作
        /// <summary>
        /// 终端首页任务看板-今日工作
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpPost]
        public object GetTodayMissionInTerminalHomeTask([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                List<TerminalHomeTask> data = new List<TerminalHomeTask>();

                string sql = string.Format(@"select  a.id  taskid,a.flowstate tasktype, a.lllegaldescribe  taskdescribe,to_char(a.reformdeadline,'yyyy-MM-dd') taskdate,a.participantname  taskperson,'wz' taskmark 
                                    from v_lllegalallbaseinfo a  where  (','|| substr(participant,2) ||',')  like   '%,{0},%' and flowstate in ('违章核准','违章审核')
                                    union
                                    select  a.id  taskid,a.flowstate tasktype, a.lllegaldescribe  taskdescribe,to_char(a.reformdeadline,'yyyy-MM-dd') taskdate,a.participantname  taskperson,'wz' taskmark  
                                    from v_lllegalallbaseinfo a  where  (','|| substr(participant,2) ||',')  like   '%,{0},%' and flowstate  = '违章整改'
                                    union
                                    select  a.id  taskid,a.flowstate tasktype, a.lllegaldescribe  taskdescribe,to_char(a.reformdeadline,'yyyy-MM-dd') taskdate,a.participantname  taskperson,'wz' taskmark  
                                    from v_lllegalallbaseinfo a  where  (','|| substr(participant,2) ||',')  like   '%,{0},%' and flowstate  = '违章验收'
                                    union
                                    select  a.id  taskid,a.flowstate tasktype, a.lllegaldescribe  taskdescribe,to_char(a.reformdeadline,'yyyy-MM-dd') taskdate,a.participantname  taskperson,'wz' taskmark  
                                    from v_lllegalallbaseinfo a  where  (','|| substr(participant,2) ||',')  like   '%,{0},%' and flowstate  = '验收确认'
                                    union
                                    select a.id taskid,a.workstream tasktype, to_char(a.hiddescribe) taskdescribe,to_char(b.changedeadine,'yyyy-MM-dd') taskdate,to_char(c.participantname) taskperson,'yh' taskmark  from v_htbaseinfo a
                                    left join v_htchangeinfo b on a.hidcode = b.hidcode 
                                    left join v_workflow c on a.id = c.id where c.actionperson like '%,{0},%'  and a.workstream ='隐患评估'
                                    union
                                    select a.id taskid,a.workstream tasktype, to_char(a.hiddescribe) taskdescribe,to_char(b.changedeadine,'yyyy-MM-dd') taskdate,to_char(c.participantname) taskperson,'yh' taskmark  from v_htbaseinfo a
                                    left join v_htchangeinfo b on a.hidcode = b.hidcode 
                                    left join v_workflow c on a.id = c.id where c.actionperson like '%,{0},%'  and a.workstream ='隐患整改'
                                    union
                                    select a.id taskid,a.workstream tasktype, to_char(a.hiddescribe) taskdescribe,to_char(b.changedeadine,'yyyy-MM-dd') taskdate,to_char(c.participantname) taskperson,'yh' taskmark  from v_htbaseinfo a
                                    left join v_htchangeinfo b on a.hidcode = b.hidcode 
                                    left join v_workflow c on a.id = c.id where c.actionperson like '%,{0},%'  and a.workstream ='隐患验收' ", curUser.Account);

                var dt = htbaseinfobll.GetGeneralQueryBySql(sql);

                foreach (DataRow row in dt.Rows)
                {
                    data.Add(new TerminalHomeTask
                    {
                        taskid = row["taskid"].ToString(),
                        taskdescribe = row["taskdescribe"].ToString(),
                        taskdate = row["taskdate"].ToString(),
                        taskperson = row["taskperson"].ToString(),
                        tasktype = row["tasktype"].ToString(),
                        taskmark = row["taskmark"].ToString()
                    });
                }

                return new { code = 0, info = "获取数据成功", count = 0, data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, Count = 0 };
            }
        }
        #endregion

        #region 终端首页安全指标(涉及违章)

        [HttpPost]
        public object GetSafetyQuotaInTerminalHome([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
            }

            string sql = string.Empty;
            string stdate = string.Empty;
            string etdate = string.Empty;

            try
            {
                string qtype = res.Contains("qtype") ? dy.data.qtype : string.Empty; //统计方式
                //时间类型
                if (!string.IsNullOrEmpty(qtype))
                {
                    //统计类型
                    switch (qtype)
                    {
                        case "0"://本月
                            stdate = DateTime.Now.ToString("yyyy-MM-01");
                            etdate = DateTime.Now.AddMonths(1).AddDays(-1).ToString("yyyy-MM-01");
                            break;
                        case "1"://本季度
                            stdate = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                            etdate = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day).AddMonths(3).AddDays(-1).ToString("yyyy-MM-dd");
                            break;
                        case "2"://本年
                            stdate = DateTime.Now.ToString("yyyy-01-01");
                            etdate = DateTime.Now.AddYears(1).AddDays(-1).ToString("yyyy-01-01");
                            break;
                    }
                }
                //违章
                sql = string.Format(@" select '违章次数' name, count(1)  num  from v_lllegalbaseinfo where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= createdate  
                                and  createdate < to_date('{1}','yyyy-mm-dd hh24:mi:ss') and createuserdeptcode like '{2}%'", stdate, etdate, curUser.DeptCode);
                //安全检查
                sql += string.Format(@" union all select '安全检查' name, count(1) num from BIS_SAFTYCHECKDATARECORD t where to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= checkbegintime  
                                and  checkbegintime< to_date('{1}','yyyy-mm-dd hh24:mi:ss') and datatype in(0,2) and (',' || checkdeptcode like '%,{2}%' or  ',' || CHECKDEPTID like '%,{2}%' or belongdept like '{2}%')", stdate, etdate, curUser.DeptCode);
                //应急演练记录
                sql += string.Format(@" union all select '应急演练' name,count(1) num from mae_drillplanrecord a left join base_department b on a.DEPARTID=b.departmentid where to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= DRILLTIME and DRILLTIME< to_date('{1}','yyyy-mm-dd hh24:mi:ss') and ISCOMMIT=1 and (',' || b.encode like '%,{2}%' or  ',' || ORGDEPTCODE like '%,{2}%' ) ", stdate, etdate, curUser.DeptCode);

                var dt = departmentBLL.GetDataTable(sql);

                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };

            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, Count = 0 };
            }
        }
        #endregion

        #endregion



    }

    #region 核准历史记录
    /// <summary>
    /// 核准历史记录
    /// </summary>
    public class ApproveHistoryModel
    {
        public string lllegalid { get; set; }   //违章id
        public string approveid { get; set; }   //核准id
        public string approveresult { get; set; }  //核准结果
        public string approvedeptname { get; set; }  //核准单位 
        public string approvedate { get; set; }  //核准时间
        public string approveperson { get; set; }  //核准人 
        public string approvereason { get; set; }  //核准意见/不通过原因
        public string assessobject { get; set; } //考核对象
        public string chargepersonone { get; set; }  //违章责任人
        public decimal? economicspunishone { get; set; } //经济处罚
        public decimal? performancepoint { get; set; } // EHS绩效考核 
        public decimal? educationone { get; set; } //教育培训
        public decimal? lllegalpointone { get; set; }  //违章扣分
        public decimal? awaitjobone { get; set; }  //待岗
        public string mark { get; set; }//区分考核、联责对象

        //public string chargepersontwo { get; set; }  //违章责任人(第一联)
        //public decimal? economicspunishtwo { get; set; } //经济处罚
        //public decimal? educationtwo { get; set; } //教育培训
        //public decimal? lllegalpointtwo { get; set; }  //违章扣分
        //public decimal? awaitjobtwo { get; set; }  //待岗

        //public string chargepersonthree { get; set; }  //违章责任人(第二联) 
        //public decimal? economicspunishthree { get; set; } //经济处罚
        //public decimal? educationthree { get; set; } //教育培训
        //public decimal? lllegalpointthree { get; set; }  //违章扣分
        //public decimal? awaitjobthree { get; set; }  //待岗 
    }
    #endregion

    #region 整改历史记录
    /// <summary>
    /// 整改历史记录
    /// </summary>
    public class ReformHistoryModel
    {
        public string lllegalid { get; set; }   //违章id
        public string reformid { get; set; }   //整改id 
        public string reformstatus { get; set; }  //整改结果
        public string reformdeadline { get; set; }  //整改截止时间
        public string reformfinishdate { get; set; }  //整改完成时间
        public string reformpeople { get; set; }  //整改人 
        public string reformtel { get; set; }  //整改电话
        public string reformdeptname { get; set; } //整改部门 
        public string reformmeasure { get; set; }  //整改措施
        public List<Photo> reformpic { get; set; }  //整改图片 
        public List<Photo> attachment { get; set; }  //整改附件 
    }
    #endregion

    #region 验收历史记录
    /// <summary>
    /// 验收历史记录
    /// </summary>
    public class AcceptHistoryModel
    {
        public string lllegalid { get; set; }   //违章id
        public string acceptid { get; set; }   //验收id 
        public string acceptresult { get; set; }  //验收结果
        public string acceptpeople { get; set; }  //验收人 
        public string acceptdeptname { get; set; } //验收单位
        public string acceptmind { get; set; }  //验收意见
        public string accepttime { get; set; }
        public List<Photo> acceptpic { get; set; } //验收图片
        public string isgrpaccept { get; set; }//是否省公司验收
    }
    #endregion

    #region 验收确认历史记录
    /// <summary>
    /// 验收确认历史记录
    /// </summary>
    public class ConfirmHistoryModel
    {
        public string lllegalid { get; set; }   //违章id
        public string confirmid { get; set; }   //确认id 
        public string confirmresult { get; set; }  //确认结果
        public string confirmpeople { get; set; }  //确认人 
        public string confirmdeptname { get; set; } //确认单位
        public string confirmmind { get; set; }  //确认意见
        public string confirmtime { get; set; }
    }
    #endregion

    #region 违章对象
    /// <summary>
    /// 违章对象
    /// </summary>
    public class LllegalModel
    {
        public LllegalApproveEntity appentity { get; set; }
        public string lllegalid { get; set; } //id
        public string createuserid { get; set; } //创建人id
        public string createdate { get; set; } //创建时间
        public string createdeptid { get; set; } //创建部门id
        public string createdeptname { get; set; }  //创建部门名称
        public string lllegalnumber { get; set; }  //违章编号
        public string belongdepart { get; set; }//所属单位
        public string belongdepartid { get; set; }//所属单位编号
        public string lllegaltype { get; set; } //违章类型id
        public string lllegaltypename { get; set; } //违章类型名称
        public string lllegaltime { get; set; } //违章时间
        public string lllegallevel { get; set; }  //违章级别id
        public string lllegallevelname { get; set; } //违章级别名称
        public string verifydeptname { get; set; } //审核部门
        public string verifydeptid { get; set; } //审核部门
        public string majorclassify { get; set; }  //专业分类id
        public string majorclassifyname { get; set; } //专业分类名称 
        public string lllegalperson { get; set; } //违章人员姓名
        public string lllegalpersonid { get; set; } //违章人员id
        public string lllegalteam { get; set; }  //违章单位名称
        public string lllegalteamcode { get; set; } //违章单位编码
        public string lllegaldepart { get; set; } //违章责任单位名称
        public string lllegaldepartcode { get; set; } //违章责任单位编码
        public string lllegaldescribe { get; set; } //违章责任描述
        public string lllegaladdress { get; set; } //违章地点
        public string engineerid { get; set; }  //外包工程id
        public string engineername { get; set; }  //外包工程名称
        public List<Photo> lllegalpic { get; set; }  // 违章图片
        public string reformrequire { get; set; }  //整改要求
        public string flowstate { get; set; }  //流程状态
        public string createusername { get; set; } //创建用户姓名
        public string addtype { get; set; } //登记类型  已整改的违章还是 一般登记
        public string isexposure { get; set; }  //是否曝光
        public string reformpeople { get; set; } //整改人姓名
        public string reformpeopleid { get; set; } //整改人id
        public string reformtel { get; set; } //整改人联系方式
        public string reformdeptcode { get; set; } //整改部门编码
        public string reformdeptname { get; set; } //整改部门名称
        public string reformdeadline { get; set; } //整改截止时间
        public string reformfinishdate { get; set; } //整改结束时间
        public string reformstatus { get; set; } //整改完成情况
        public string reformmeasure { get; set; } //整改措施
        public string applicationstatus { get; set; } //整改延期状态值

        public string isappoint { get; set; } //是否指定整改责任人
        public string reformchargeperson { get; set; }  //整改责任负责人
        public string reformchargepersonname { get; set; } //整改责任负责人
        public string reformchargedeptid { get; set; }  //指定整改责任部门
        public string reformchargedeptname { get; set; }  //指定整改责任部门

        public List<Photo> reformpic { get; set; }  //整改图片
        public List<Photo> attachment { get; set; }  //整改附件 
        public string acceptpeopleid { get; set; }  //验收人id
        public string acceptpeople { get; set; } //验收人姓名
        public string acceptdeptname { get; set; } //验收部门名称
        public string acceptdeptcode { get; set; } //验收部门编码
        public string acceptresult { get; set; }  //验收结果
        public string acceptmind { get; set; } //验收意见
        public string accepttime { get; set; } //验收时间
        public string isgrpaccept { get; set; }//是否省公司验收
        public string confirmpeopleid { get; set; }  //确认人id
        public string confirmpeople { get; set; } //确认人姓名
        public string confirmdeptname { get; set; } //确认部门名称
        public string confirmdeptcode { get; set; } //确认部门编码
        public string confirmresult { get; set; }  //确认结果
        public string confirmmind { get; set; } //确认意见
        public string confirmtime { get; set; } //确认时间
        public string reseverid { get; set; } //关联应用id
        public string resevertype { get; set; } //关联应用类型
        public string isback { get; set; } //是否回退
        public string backreason { get; set; } //回退原因
        public bool isenableback { get; set; } //是否启用回退
        public string participant { get; set; }  //流程参与账户
        public string isupsafety { get; set; }  //是否上报安全主管部门
        public List<Photo> acceptpic { get; set; } //验收图片
        public List<PunishData> punishdata { get; set; } //考核集合
        public List<AwardData> awarddata { get; set; } //奖励对象
        public List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData> checkflow { get; set; }  //流程图
        public bool isdeliver { get; set; } //是否存在转交
        public bool isacceptdeliver { get; set; }//是否存在验收转交
        public bool wzpicmustupload { get; set; } //违章图片是否必传
        public bool zgpicmustupload { get; set; } //违章整改图片是否必传
        public bool yspicmustupload { get; set; } //违章验收图片是否必传
        public string analyzetemplate { get; set; } //违章分析模板

        public bool isgetaward { get; set; } //是否具有奖励
    }
    #endregion

    public class PunishData
    {
        public string assessobject { get; set; } //考核对象
        public string chargeperson { get; set; }  //违章责任人
        public string chargepersonid { get; set; }  //违章责任人id
        public decimal? performancepoint { get; set; } // EHS绩效考核 
        public decimal? economicspunish { get; set; } //经济处罚
        public decimal? education { get; set; } //教育培训
        public decimal? lllegalpoint { get; set; }  //违章扣分
        public decimal? awaitjob { get; set; }  //待岗
        public string mark { get; set; }//区分考核、联责对象
    }

    public class AwardData
    {
        public string awarduserid { get; set; } //奖励人员id
        public string awardusername { get; set; }  //奖励人员姓名
        public string awarddeptid { get; set; }  //奖励人员部门
        public string awarddeptname { get; set; }  //奖励人员部门
        public decimal? awardmoney { get; set; } // 奖励金额 
        public int? awardpoints { get; set; }  //奖励积分

    }
    public class oseries
    {
        public string name { get; set; }
        public int num { get; set; }
        public decimal precent { get; set; }
        public string color { get; set; }
    }
    public class mseries
    {
        public string name { get; set; }
        public int ybnum { get; set; }
        public int jyznum { get; set; }
        public int zdnum { get; set; }
    }
    public class series
    {
        public string name { get; set; }
        public List<int> data { get; set; }
    }
    public class fseries
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }
    }

    #region 隐患标准
    /// <summary>
    /// 隐患标准
    /// </summary>
    public class hidstandard
    {
        public string id { get; set; }
        public string standardname { get; set; }
        public bool isparent { get; set; }
        public string parentid { get; set; }
        public List<hidstandarditem> standarddata { get; set; }
        public List<hidstandard> children { get; set; }
    }
    #endregion

    #region 隐患标准明细
    /// <summary>
    /// 隐患标准明细
    /// </summary>
    public class hidstandarditem
    {
        public string norm { get; set; }  //检查依据
        public string content { get; set; }   //隐患描述
        public string require { get; set; }   //隐患整改要求
    }
    #endregion

    #region 安全预警对象
    /// <summary>
    /// 安全预警对象
    /// </summary>
    public class hidsafetywarning
    {
        public string total { get; set; }  //总数 、整改率
        public string deptname { get; set; }  //电厂名称
        public string deptcode { get; set; }  //编码 
        public string organizeid { get; set; } //机构id
    }
    #endregion

    public class HidRankNewData
    {
        public string hidrank { get; set; }
        public string hidrankname { get; set; }
        public bool isupsubmit { get; set; }
        public bool isshowappoint { get; set; } //是否显示指定整改责任人
        public bool ismustwrite { get; set; } //是否整改必填
        public bool ishavetjsubmit { get; set; } //是否具有同级提交
        public bool isenablebm { get; set; } //是否启用所属部门
        public bool isenableaccept { get; set; } //是否具有验收配置
        public bool isenablemajorclassify { get; set; } //是否启用专业分类 
        public bool yhpicmustupload { get; set; } //控制隐患图片必传
        public bool zgpicmustupload { get; set; } //控制整改图片必传
        public bool yspicmustupload { get; set; } //控制验收图片必传
        public List<HidMajorClassifyNewData> majordata { get; set; } //专业对象
    }
    public class HidMajorClassifyNewData
    {
        public string majorclassify { get; set; }
        public string majorclassifyname { get; set; }
        public bool isupsubmit { get; set; }
        public bool isshowappoint { get; set; } //是否显示指定整改责任人
        public bool ismustwrite { get; set; } //是否整改必填
        public bool ishavetjsubmit { get; set; } //是否具有同级提交
        public bool isenablebm { get; set; } //是否启用所属部门
        public bool isenablemajorclassify { get; set; } //是否启用专业分类 
        public bool isenableaccept { get; set; } //是否具有验收配置
        public bool yhpicmustupload { get; set; } //控制隐患图片必传
        public bool zgpicmustupload { get; set; } //控制整改图片必传
        public bool yspicmustupload { get; set; } //控制验收图片必传
    }
    public class HidLimits
    {
        public bool isgdxj { get; set; } //是否国电新疆

        public string reformtype { get; set; }  //新增类型  0 通用限期 ， 1 立即整改，2 省级限期

        //必填说明
        public string mustremark { get; set; }  //必填说明

        public List<HidAuthData> list { get; set; }
    }
    public class HidAuthData
    {
        //是否本部门整改
        public int isselfchange { get; set; }
        //当前结点
        public string currentflow { get; set; }
        //隐患级别
        public string rankname { get; set; }
        //控制必填项
        public int mustwrite { get; set; }
    }
    public class LllegalAuthData
    {
        public bool ispromptlychange { get; set; } //是否能立即整改
        public bool iscanchange { get; set; } //是否能直接到整改
        public bool ishaveupsubmit { get; set; }  //是否具有上报功能 
        public bool ismustsubmit { get; set; }    //是否具有限制上报功能
        public bool isshowappoint { get; set; } //是否显示指定整改责任人
        public bool ishavetjsubmit { get; set; } //是否具有同级提交权限
        public bool isendflow { get; set; } //是否能够直接到流程结束
        public bool ishrdl { get; set; } //是否华润电力
        public bool wzpicmustupload { get; set; } //控制隐患图片必传
        public bool zgpicmustupload { get; set; } //控制整改图片必传
        public bool yspicmustupload { get; set; } //控制验收图片必传
        public bool isgetaward { get; set; } //是否具有奖励
    }
    public class HidTypeData
    {
        public string hidtype { get; set; }
        public string hidtypename { get; set; }
        public string parentid { get; set; }
        public bool isparent { get; set; }
        public bool isdefault { get; set; }
        public List<HidTypeData> children { get; set; }
    }

    /// <summary>
    /// 班组端首页任务看板-个人任务对象
    /// </summary>
    public class TerminalHomeTask
    {
        public string taskid { get; set; } //任务id
        public string tasktype { get; set; } //任务类别
        public string taskdescribe { get; set; } //描述(违章、隐患)
        public string taskdate { get; set; } //任务时间
        public string taskperson { get; set; } //任务人员
        public string taskmark { get; set; } //任务标记 用于区分是隐患还是违章
    }
}