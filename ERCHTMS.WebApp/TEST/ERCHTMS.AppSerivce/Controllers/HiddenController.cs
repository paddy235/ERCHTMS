using BSFramework.Data;
using BSFramework.Util;
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
using ERCHTMS.Entity.SystemManage.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
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

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //选中的所属单位
            string orgid = string.Empty;

            //获取当前部门
            string organizeId = curUser.OrganizeId;


            //请求标记
            string reqmark = res.Contains("reqmark") ? dy.data.reqmark.ToString() : "0";   //1 排查 ，2 整改 ，3 厂级验收，4 省级验收 ，5 复查 

            string parentId = string.Empty;

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
            else
            {
                parentId = departmentBLL.GetEntity(curUser.OrganizeId).ParentId;
                orgid = organizeId.ToString(); //厂级用户
            }


            IList<DeptData> result = new List<DeptData>();
            IList<DeptData> list = new List<DeptData>();
            try
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
                list = GetChangeDept(dept, orgid, reqmark);
                dept.children = list;
                result.Add(dept);
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

                List<DepartmentEntity> plist = departmentBLL.GetList().Where(t => t.ParentId == parentId).OrderBy(t => t.SortCode).ToList();

                //  1 ,4, 5

                if (plist.Count() > 0)
                {
                    var dlist = departmentBLL.GetList().OrderBy(t => t.SortCode).ToList();

                    foreach (DepartmentEntity entity in plist)
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

                        if (reqmark == "1" || reqmark == "4" || reqmark == "5")
                        {
                            if (entity.FullName == "各电厂")
                            {
                                istrue = false;
                            }
                        }


                        if (istrue)
                        {
                            var pdepts = dlist.Where(p => p.ParentId == depts.deptid).ToList();
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
                    if (parentDept.parentid != "0")
                    {
                        list.Add(parentDept);
                    }
                }
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

            if (curUser.RoleName.Contains("省级用户"))
            {
                orgid = res.Contains("orgid") ? dy.data.orgid : "";
                //风险查询条件时没有传OrgId--省级用户传入厂级Code,此时默认OrgId为厂级Id
                if (string.IsNullOrWhiteSpace(orgid)) {
                    var deptcode = res.Contains("deptcode") ? dy.data.orgid : "";
                    if (!string.IsNullOrWhiteSpace(deptcode)) {
                        var dept = new DepartmentBLL().GetList().Where(x => x.EnCode == deptcode && x.Nature == "厂级").FirstOrDefault();
                        if (dept != null) {
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

                foreach (DistrictEntity item in ditlist)
                {
                    AreaData entity = new AreaData();
                    entity.areaid = item.DistrictID;
                    entity.areacode = item.DistrictCode;
                    entity.areaname = item.DistrictName;
                    entity.parentareaid = item.ParentID;
                    var parentarea = dlist.Where(p => p.ParentID == item.DistrictID);
                    if (parentarea.Count() > 0)
                    {
                        entity.isparent = true;
                        var glist = GetAreaData(dlist, entity, orgid, item.DistrictID);
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
            catch (Exception)
            {
                return new { code = -1, info = "获取数据失败", count = 0 };
            }
            //获取当前部门
            return new { code = 0, info = "获取数据成功", count = result.Count(), data = result };
        }
        #endregion

        #region 获取区域
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="areadata"></param>
        /// <param name="organizeid"></param>
        /// <returns></returns>
        public IList<AreaData> GetAreaData(List<DistrictEntity> list, AreaData areadata, string organizeid, String itemid)
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
                        var parentarea = list.Where(p => p.ParentID == item.DistrictID);
                        if (parentarea.Count() > 0)
                        {
                            entity.isparent = true;
                            var glist = GetAreaData(list, entity, organizeid, item.DistrictID);
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
        [HttpPost]
        public object GetHidRank()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidRank'").ToList();
            List<HidRankNewData> data = new List<HidRankNewData>();
            foreach (DataItemModel entity in itemlist)
            {
                HidRankNewData model = new HidRankNewData();
                model.hidrank = entity.ItemDetailId;
                model.hidrankname = entity.ItemName;
                //判定是否存在上报功能
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = ""; //
                wfentity.startflow = "隐患评估";
                wfentity.submittype = "上报";
                wfentity.user = curUser;
                if (curUser.RoleName.Contains("省级用户"))
                {
                    wfentity.mark = "省级隐患排查";
                }
                else
                {
                    wfentity.mark = "厂级隐患排查";
                }
                wfentity.isvaliauth = true;
                if (entity.ItemName.Contains("一般隐患"))
                {
                    wfentity.rankname = "一般隐患";
                }
                else
                {
                    wfentity.rankname = "重大隐患";
                }
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                model.isupsubmit = result.ishave;
                data.Add(model);
            }
            return new { code = 0, info = "获取数据成功", count = 0, data = data };
        }
        #endregion

        #region 隐患专业分类
        /// <summary>
        /// 隐患专业分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetMajorClassify()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidMajorClassify'");

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { majorclassify = x.ItemDetailId, majorclassifyname = x.ItemName }) };
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
            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { checktype = x.ItemDetailId, checktypename = x.ItemName }) };
        }
        #endregion

        #region 隐患类别
        /// <summary>
        /// 隐患类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHidType()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'HidType'");

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { hidtype = x.ItemDetailId, hidtypename = x.ItemName }) };
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

            string sjorgid = string.Empty;

            if (curUser.RoleName.Contains("省级用户"))
            {
                sjorgid = curUser.OrganizeId; //当前用户id
            }
            if (string.IsNullOrEmpty(orgid))
            {
                orgid = curUser.OrganizeId;
            }
            //获取所有人员
            var list = userbll.GetAllTableByArgs(userArgs, dutydeptid, orgid, sjorgid, reqmark);

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
                        account = x.Field<string>("account")
                    })
            };
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

            string tablename = string.Format(@" (
                                                  select a.DeviceId,a.account,a.modifydate,a.createuserdeptcode,a.createuserorgcode,a.createuserid,
                                                  a.checktypename,a.hidtypename,a.hidrankname,a.checkdepartname,a.isgetafter,a.id as hiddenid ,to_char(a.createdate,'yyyy-MM-dd') createdate,
                                                  a.hidcode as problemid ,to_char(a.checkdate,'yyyy-MM-dd') checkdate,a.checkdepart,a.checkdepartid,a.checktype,a.checknumber,a.relevanceid,a.relevancetype,
                                                  a.isbreakrule,a.hidrank,a.hidplace,a.hidpoint,a.workstream ,a.addtype,b.participant,c.applicationstatus ,b.actionperson,b.actionpersonname,a.hidtype,c.changedutydepartcode,
                                                  c.changeperson,a.exposurestate,c.postponedept,c.postponedeptname ,c.postponeperson,c.postponepersonname, a.hiddescribe,
                                                  c.changemeasure,c.changedeadine,a.safetycheckobjectid,d.acceptdepartcode,d.acceptperson,(case when f.filepath is not 
                                                  null then ('{0}'||substr(f.filepath,2)) else '' end) as filepath ,
                                                  m.recheckperson,m.recheckpersonname,m.recheckdepartcode,m.recheckdepartname,a.hiddepart,a.hiddepartname,a.deptcode,
                                                 ( case when  a.workstream ='隐患登记' then '隐患登记' when a.workstream ='隐患评估' then  '评估中' when  a.workstream ='隐患完善' then '完善中' when
                                                     a.workstream ='隐患整改' then '整改中' when  a.workstream ='隐患验收' then '验收中' when  a.workstream ='复查验证' then '复查中' when  a.workstream ='整改效果评估' then '效果评估中' when
                                                      a.workstream ='整改结束' then '整改结束' end ) actionstatus
                                                  from v_htbaseinfo a
                                                  left join (
                                                   select a.id,a.participant,a.actionperson, (select listagg(b.realname,',') within group(order by b.account) from base_user b where instr(','|| a.actionperson ||',',','||b.account||',')>0) actionpersonname from v_workflow a
                                                  ) b on a.id = b.id 
                                                  left join v_htchangeinfo c on a.hidcode = c.hidcode
                                                  left join v_htacceptinfo d on a.hidcode = d.hidcode
                                                  left join v_htrecheck m  on a.hidcode = m.hidcode 
                                                  left join v_imageview f on a.hidphoto = f.recid 
                                         ) a ", dataitemdetailbll.GetItemValue("imgUrl"));
            Pagination pagination = new Pagination();
            pagination.p_tablename = tablename;
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "createdate desc ,modifydate desc";
            pagination.sord = "";
            pagination.conditionJson = " 1=1 ";
            pagination.p_fields = @"account,modifydate,createuserdeptcode,createuserorgcode,createuserid,checktypename,hidtypename,hidrankname,checkdepartname,
                                    createdate,problemid,checkdate,checktype,checknumber,isgetafter,relevanceid,relevancetype,
                                    isbreakrule ,hidtype, hidrank,hidplace,hidpoint,workstream,addtype,participant,applicationstatus,postponedept,
                                    postponedeptname,postponeperson,postponepersonname,hiddescribe,changemeasure,filepath,changedutydepartcode,hiddepartname,
                                    recheckdepartname,recheckpersonname,deptcode,checkdepart,actionpersonname,actionstatus";

            pagination.p_kid = "hiddenid";


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

            string hidrank = string.Empty;
            string dutydept = string.Empty;
            string hiddescribe = string.Empty;

            switch (action)
            {
                //获取所有已发现的一般隐患接口
                case "1":
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and  workstream != '隐患完善' and  workstream != '制定整改计划' ";

                    pagination.conditionJson += @" and  hidrankname  = '一般隐患'";
                    break;
                //获取所有已发现的重大隐患接口(安全生产指标)
                case "2":
                    if (curUser.RoleName.Contains("省级用户"))
                    {
                        pagination.conditionJson += string.Format("  and  deptcode like '{0}%'  and  to_char(createdate,'yyyy') ='{1}' ", curUser.NewDeptCode, DateTime.Now.Year.ToString());
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

                    pagination.conditionJson += @" and  hidrankname  like '%重大%'";
                    break;
                //获取所有整改中的一般隐患接口
                case "3":
                    pagination.conditionJson += @" and  hidrankname  = '一般隐患' and workstream  = '隐患整改'";
                    break;
                //获取所有整改中的重大隐患接口
                case "4":
                    pagination.conditionJson += @" and  hidrankname  like '%重大%' and workstream  = '隐患整改'";
                    break;
                //获取所有逾期未整改的一般隐患接口
                case "5":
                    pagination.conditionJson += string.Format(@" and workstream  = '隐患整改'  and 
                         to_date('{0}','yyyy-mm-dd hh24:mi:ss')> changedeadine and hidrankname =  '一般隐患'", DateTime.Now);
                    break;
                //获取所有逾期未整改的重大隐患接口
                case "6":
                    pagination.conditionJson += string.Format(@" and workstream  = '隐患整改'  and 
                         to_date('{0}','yyyy-mm-dd hh24:mi:ss')> changedeadine and  hidrankname like '%重大%'", DateTime.Now);
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
                         to_date('{0}','yyyy-mm-dd hh24:mi:ss')> (changedeadine+1)", DateTime.Now);
                    break;
                //获取所有整改完成的隐患列表接口
                case "13":
                    pagination.conditionJson += @" and  changeresult ='1' and  workstream in ('隐患验收','复查验证','整改效果评估','整改结束')";
                    break;
                //获取即将逾期的未整改隐患列表接口
                case "14":
                    pagination.conditionJson += @"and workstream = '隐患整改' and ((hidrankname = '一般隐患' and changedeadine - 3 <= sysdate  and sysdate < changedeadine + 1 )
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
                         to_date('{1}','yyyy-mm-dd hh24:mi:ss') > (changedeadine+1)", curUser.UserId, DateTime.Now);
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
                         to_date('{1}','yyyy-mm-dd hh24:mi:ss') > (changedeadine+1)", curUser.UserId, DateTime.Now);
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

                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";

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
                    pagination.conditionJson += string.Format(@" and  checkdepartid like '{0}%'  and  to_char(createdate,'yyyy') ='{1}' ", curUser.OrganizeCode, DateTime.Now.Year);

                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";
                    break;
                //省公司下整改率低于80%
                case "38":
                    pagination.conditionJson += string.Format(@" and  workstream  = '隐患整改' and  to_char(createdate,'yyyy') ='{0}' ", DateTime.Now.Year);
                    break;
                //省公司下的重大隐患
                case "39":
                    pagination.conditionJson += string.Format(@"   and  hidrankname  like '%重大%'  and  to_char(createdate,'yyyy') ='{0}' ", DateTime.Now.Year);
                    pagination.conditionJson += @"  and workstream != '隐患登记'  and  workstream != '隐患评估' and workstream != '隐患完善' and workstream !='制定整改计划'";
                    break;
                //省公司下的重大隐患指标(本年度重大隐患+往年未整改完的重大隐患)
                case "42":
                    pagination.conditionJson += string.Format(@"   and  ((hidrankname  like '%重大%'  and  to_char(createdate,'yyyy') !='{0}' and workstream ='隐患整改'） or (hidrankname  like '%重大%'  and  to_char(createdate,'yyyy') ='{0}')) ", DateTime.Now.Year);
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

                //一次性提交隐患流程
                #region  一次性提交
                if (reformtype == "1")
                {
                    //隐患整改
                    var changeEntity = htchangeinfobll.GetEntityByCode(hidcode);
                    if (null != changeEntity)
                    {
                        changeEntity.CHANGERESULT = "1";
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
                    //是否上报
                    if (issubmit == "1")
                    {
                        wfentity.submittype = "上报";
                    }
                    else
                    {
                        wfentity.submittype = "提交";
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

                bool isspecial = false;

                #region 检查当前是否为生技部(具有直接提交到隐患整改的权限，也可设置其他角色部门),

                WfControlObj wfValentity = new WfControlObj();
                wfValentity.businessid = ""; //
                wfValentity.startflow = "制定整改计划";
                wfValentity.endflow = "隐患整改";
                wfValentity.submittype = "提交";
                wfValentity.rankid = entity.HIDRANK;
                wfValentity.user = curUser;
                wfValentity.mark = "厂级隐患排查";
                wfValentity.isvaliauth = true;

                string resultVal = string.Empty;
                //获取下一流程的操作人
                WfControlResult valresult = wfcontrolbll.GetWfControl(wfValentity);
                isspecial = valresult.ishave; //验证结果
                #endregion


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.startflow = "制定整改计划";
                //具有生技部的权限，且整改部门就是生技部，则直接提交到整改
                if (isspecial && centity.CHANGEDUTYDEPARTID == curUser.DeptId)
                {
                    wfentity.submittype = "提交";
                }
                else
                {
                    wfentity.submittype = "制定提交";
                }
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
                entity.CHECKDATE = res.Contains("checkdate") ? Convert.ToDateTime(dy.data.checkdate) : null;  //排查日期
                entity.HIDDEPART = res.Contains("deptid") ? dy.data.deptid : ""; //所属单位id
                entity.HIDDEPARTNAME = res.Contains("deptname") ? dy.data.deptname : ""; //所属单位名称
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
            if (curUser.isPlanLevel == "1")
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

                baseentity.APPSIGN = AppSign; //移动端标记
                baseentity.HIDPROJECT = res.Contains("engineerid") ? dy.data.engineerid : "";  //所属工程
                baseentity.HIDPROJECTNAME = res.Contains("engineername") ? dy.data.engineername : "";  //所属工程名称
                baseentity.ISSELFCHANGE = res.Contains("isselfchange") ? dy.data.isselfchange : ""; //是否本部门整改 
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
                UploadifyFile(entity.APPROVALFILE, "approvalimg", files);

                //请求对象
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.startflow = baseentity.WORKSTREAM;
                wfentity.rankid = baseentity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = baseentity.HIDDEPART; //对应电厂id

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
                    var approvalHistory = htapprovalbll.GetHistoryList(hidCode).Where(p => p.APPROVALPERSON == curUser.UserId && p.APPROVALRESULT == "1").ToList();
                    if (approvalHistory.Count() == 0)
                    {
                        //获取下一流程的操作人
                        result = wfcontrolbll.GetWfControl(wfentity);
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "评估过后的隐患,无法再次退回!" };
                    }
                }
                else
                {
                    //返回结果
                    result = wfcontrolbll.GetWfControl(wfentity);
                }

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
        #endregion

        #region    获取上一个部门(非包含特定角色下的部门)
        /// <summary>
        /// 获取上一个部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public DepartmentEntity GetDepartEntityBySpecial(string deptId)
        {
            DepartmentEntity pentity = new DepartmentEntity();

            try
            {
                var deptEntity = departmentBLL.GetEntity(deptId);

                string professinalName = dataitemdetailbll.GetItemValue("ProfessinalQuality"); //部门性质
                if (null != deptEntity)
                {
                    //如果当前部门是专业,则取再上一个部门
                    if (deptEntity.Nature == professinalName)
                    {
                        pentity = GetDepartEntityBySpecial(deptEntity.ParentId);
                    }
                    else
                    {
                        pentity = deptEntity;
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return pentity;
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

                //上传隐患图片
                UploadifyFile(entity.HIDCHANGEPHOTO, "reformimg", files);

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
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    //参与者
                    participant = result.actionperson;
                    //状态
                    wfFlag = result.wfflag;

                    if (!string.IsNullOrEmpty(participant) && !string.IsNullOrEmpty(wfFlag))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                        }
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
                exentity.POSTPONEREASON = postponereason;
                exentity.POSTPONERESULT = postponeresult;  //申请结果
                exentity.APPSIGN = AppSign; //移动端标记
                htextensionbll.SaveForm("", exentity);

                //极光推送
                htworkflowbll.PushMessageForHidden("隐患处理审批", nextName, wfentity, result);
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

                    #region 处理其他
                    //更新处理
                    entity.ACCEPTSTATUS = ACCEPTSTATUS;
                    entity.ACCEPTDATE = Convert.ToDateTime(ACCEPTDATE);
                    entity.ACCEPTIDEA = ACCEPTIDEA;
                    //验收人为空的情况下
                    if (string.IsNullOrEmpty(entity.ACCEPTPERSON))
                    {
                        entity.ACCEPTDEPARTCODE = curUser.DeptCode;
                        entity.ACCEPTDEPARTNAME = curUser.DeptName;
                        entity.ACCEPTPERSON = curUser.UserId;
                        entity.ACCEPTPERSONNAME = curUser.UserName;
                    }
                    if (!string.IsNullOrEmpty(DAMAGEDATE))
                    {
                        entity.DAMAGEDATE = Convert.ToDateTime(DAMAGEDATE.ToString());
                    }

                    //上传隐患验收图片
                    UploadifyFile(entity.ACCEPTPHOTO, "checkimg", files);
                    entity.APPSIGN = AppSign; //移动端标记
                    htacceptinfobll.SaveForm(ACCEPTID, entity);

                    //退回则重新添加验收记录
                    #region 退回则重新添加验收记录
                    if (wfFlag == "3")
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

                    if (!string.IsNullOrEmpty(participant))
                    {
                        //提交流程
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "请联系系统管理员，确认是否配置流程所需参与人!" };
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

                    //退回后重新新增整改记录及整改效果评估记录
                    #region 退回后重新新增整改记录及整改效果评估记录
                    if (wfFlag == "2")
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

                        HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(cEntity.HIDCODE);
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
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "目标流程参与者未定义" };
                    }
                    #endregion
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

                HtReCheckEntity entity = htrecheckbll.GetEntityByHidCode(hidcode); //获取隐患复查验证信息

                string ESTIMATEID = string.Empty; //复查ID

                if (null != entity)
                {
                    ESTIMATEID = entity.ID;
                }
                else
                {
                    entity = new HtReCheckEntity();
                }

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
                htrecheckbll.SaveForm(ESTIMATEID, entity); //保存信息

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
                    //退回后重新新增整改记录及整改效果评估记录
                    #region 退回后重新新增整改记录及整改效果评估记录
                    if (wfFlag == "2")
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
                        //验收记录
                        HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(cEntity.HIDCODE);
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
                        //复查验证信息
                        HtReCheckEntity htrecheckentity = htrecheckbll.GetEntityByHidCode(cEntity.HIDCODE);
                        if (null != htrecheckentity)
                        {
                            HtReCheckEntity recheckEntity = new HtReCheckEntity();
                            recheckEntity = htrecheckentity;
                            recheckEntity.ID = null;
                            recheckEntity.CREATEDATE = DateTime.Now;
                            recheckEntity.MODIFYDATE = DateTime.Now;
                            recheckEntity.RECHECKSTATUS = null;
                            recheckEntity.RECHECKIDEA = null;
                            recheckEntity.AUTOID = htrecheckentity.AUTOID + 1;
                            htrecheckbll.SaveForm("", recheckEntity);
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(participant))
                    {
                        //提交流程
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
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

            HiddenData entity = new HiddenData();

            if (baseInfo.Rows.Count == 1)
            {
                entity.hiddenid = baseInfo.Rows[0]["hiddenid"].ToString(); //key 
                entity.problemid = baseInfo.Rows[0]["problemid"].ToString();  //隐患编码
                entity.safetydetailid = baseInfo.Rows[0]["safetycheckobjectid"].ToString();  //安全检查id
                entity.isselfchange = baseInfo.Rows[0]["isselfchange"].ToString(); //是否本部门整改
                entity.isformulate = baseInfo.Rows[0]["isformulate"].ToString();  //是否已经制定了整改计划
                entity.deptid = baseInfo.Rows[0]["hiddepart"].ToString();
                entity.deptname = baseInfo.Rows[0]["hiddepartname"].ToString();
                entity.hidrank = baseInfo.Rows[0]["hidrank"].ToString(); //隐患级别
                entity.rankname = baseInfo.Rows[0]["rankname"].ToString();
                entity.categoryid = baseInfo.Rows[0]["categoryid"].ToString();
                entity.category = baseInfo.Rows[0]["category"].ToString();
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
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = ""; //
                wfentity.startflow = "隐患评估";
                wfentity.submittype = "上报";
                wfentity.user = curUser;
                if (entity.reformtype == "2")
                {
                    wfentity.mark = "省级隐患排查";
                }
                else
                {
                    wfentity.mark = "厂级隐患排查";
                }
                wfentity.isvaliauth = true;
                if (entity.rankname.Contains("一般隐患"))
                {
                    wfentity.rankname = "一般隐患";
                }
                else
                {
                    wfentity.rankname = "重大隐患";
                }
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                entity.ishaveupsubmit = result.ishave;
                entity.isupsubmit = baseInfo.Rows[0]["upsubmit"].ToString(); //是否上报
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
                // entity.isusechangeck = baseInfo.Rows[0]["createuserdeptcode"].ToString() == curUser.DeptCode;  
            }


            return new { code = 0, count = 0, info = "获取成功", data = entity };
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
                data.approvaldate = entity.APPROVALDATE != null ? entity.APPROVALDATE.ToString().Replace("0:00:00", "") : "";
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
                data.deadinetime = entity.CHANGEDEADINE != null ? entity.CHANGEDEADINE.ToString().Replace("0:00:00", "") : ""; //整改截至时间
                data.reformfinishdate = entity.CHANGEFINISHDATE != null ? entity.CHANGEFINISHDATE.ToString().Replace("0:00:00", "") : ""; //整改完成时间
                data.reformdescribe = entity.CHANGERESUME;//整改情况描述
                data.realitymanagecapital = entity.REALITYMANAGECAPITAL.ToString(); //实际治理资金
                data.reformmeasure = entity.CHANGEMEASURE; //整改措施
                data.reformresult = entity.CHANGERESULT; //整改结果
                data.reformbackreason = entity.BACKREASON; //回退原因
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
                data.checktime = entity.ACCEPTDATE != null ? entity.ACCEPTDATE.ToString().Replace("0:00:00", "") : "";
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
                data.rechecksdate = entity.RECHECKDATE != null ? entity.RECHECKDATE.ToString().Replace("0:00:00", "") : "";
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
                data.estimatedate = entity.ESTIMATEDATE != null ? entity.ESTIMATEDATE.ToString().Replace("0:00:00", "") : "";
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
                    data.handleid = entity.HANDLETYPE;
                    data.hiddenid = entity.HIDID;
                    data.problemid = entity.HIDCODE;
                    data.postponedays = entity.POSTPONEDAYS; //申请天数
                    data.postponeresult = entity.POSTPONERESULT;//审批结果
                    data.applyreason = entity.POSTPONEREASON; //申请理由/审批意见
                    data.applydate = entity.HANDLEDATE.ToString().Replace("0:00:00", "").Trim(); //申请时间/审批时间
                    data.applyperson = entity.HANDLEUSERNAME;//申请人/审批人
                    data.applypersonid = entity.HANDLEUSERID;//申请人/审批人
                    data.applydept = entity.HANDLEDEPTCODE;//申请部门/审批部门
                    data.applydeptname = entity.HANDLEDEPTNAME;//申请部门/审批部门
                    if (entity.HANDLETYPE == "0")
                    {
                        data.handlestate = "延期申请"; //状态
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

            var list = htextensionbll.GetListByCondition(hidcode);

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
                data.applyreason = entity.POSTPONEREASON;
                data.postponeresult = entity.POSTPONERESULT;
                data.applydate = entity.HANDLEDATE.ToString().Replace("0:00:00", "").Trim();
                data.applyperson = entity.HANDLEUSERNAME;
                data.applypersonid = entity.HANDLEUSERID;
                data.applydept = entity.HANDLEDEPTCODE;
                data.applydeptname = entity.HANDLEDEPTNAME;
                if (entity.HANDLETYPE == "0")
                {
                    data.handlestate = "延期申请"; //状态
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
            }
            catch (Exception ex)
            {
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
                    IsExposureState = isSee
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



        /*********************违章流程*************************/

        #region 基础信息

        #region 违章类型
        /// <summary>
        /// 违章类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetLllegalType()
        {

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'LllegalType'"); //违章类型

            string applianceclass = dataitemdetailbll.GetItemValue("ApplianceClass"); //装置类对象 

            return new { code = 0, info = "获取数据成功", count = 0, data = new { itemdata = itemlist.Select(x => new { lllegaltypeid = x.ItemDetailId, lllegaltypename = x.ItemName }), applianceclass = applianceclass } };
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

            string userid = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

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

            string year = res.Contains("year") ? dy.data.year : "";
            string month = res.Contains("month") ? dy.data.month : "";
            string day = res.Contains("day") ? dy.data.day : "";
            string qdeptcode = res.Contains("qdeptcode") ? dy.data.qdeptcode : "";//电厂code
            string currdeptcode = res.Contains("currdeptcode") ? dy.data.currdeptcode : "";
            string currdate = string.Empty;
            if (!string.IsNullOrWhiteSpace(year) && !string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(day))
            {
                currdate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day)).ToString("yyyy-MM-dd");
            }
            string createdeptcode = res.Contains("createdeptcode") ? dy.data.createdeptcode : ""; //登记单位
            string lllegalstartdate = res.Contains("lllegalstartdate") ? dy.data.lllegalstartdate : ""; //违章时间
            string lllegalenddate = res.Contains("lllegalenddate") ? dy.data.lllegalenddate : ""; //违章时间

            string reformdept = res.Contains("reformdept") ? dy.data.reformdept : ""; //整改单位 --部门编码
            string belongcode = res.Contains("belongcode") ? dy.data.belongcode : "";//省公司查看各电厂的数据
            string regcode = res.Contains("regcode") ? dy.data.regcode : "";//省公司查看登记单位（1：本单位登记，2：电厂登记）

            string tablename = string.Empty;

            tablename = string.Format(@" ( 
                                       select a.belongdepart,a.belongdepartid,a.createuserdeptcode,a.createuserorgcode,a.modifydate, a.createuserid, a.id,a.createdate, a.lllegalnumber,
                                        a.lllegaltype,a.lllegaltypename ,a.lllegaltime,a.lllegallevel,a.lllegallevelname, a.lllegalperson,a.lllegalpersonid,a.lllegalteam,a.lllegalteamcode,a.lllegaldepart,a.lllegaldepartcode,
                                        a.lllegaldescribe,a.lllegaladdress ,a.lllegalpic,a.reformrequire,a.flowstate,a.createusername ,a.addtype,a.isexposure,a.reformpeople,a.reformpeopleid,
                                        a.reformtel,a.reformdeptcode,a.reformdeptname,a.reformdeadline,a.reformfinishdate,a.reformstatus,a.reformmeasure,a.isgrpaccept,a.acceptpeopleid,
                                        a.acceptpeople,a.acceptdeptname,a.acceptdeptcode,a.acceptresult,a.acceptmind,a.accepttime ,a.reseverid,a.resevertype,a.reseverone,a.resevertwo,
                                        a.reseverthree ,a.participant ,(case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end) as filepath,c.actionpersonname,
                                        ( case when  a.flowstate ='违章登记' then '违章登记' when a.flowstate ='违章核准' then  '核准中' when  a.flowstate ='违章完善' then '完善中' when
                                             a.flowstate ='违章整改' then '整改中' when  a.flowstate ='违章验收' then '验收中'  when  a.flowstate ='验收确认' then '验收确认中' when
                                              a.flowstate ='整改结束' then '整改结束' end ) actionstatus
                                          from v_lllegalallbaseinfo a
                                        left join v_imageview b on a.lllegalpic = b.recid  
                                        left join (
                                           select a.id,a.participant, (select listagg(b.realname,',') within group(order by b.account) from base_user b where instr(','|| substr(a.participant,2,length(a.participant)-1) ||',',','||b.account||',')>0) actionpersonname from v_lllegalworkflow a
                                          ) c on a.id = c.id  
                                        ) a ", dataitemdetailbll.GetItemValue("imgUrl"));

            Pagination pagination = new Pagination();

            pagination.p_tablename = tablename;
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "createdate desc,modifydate desc";
            pagination.sord = "";
            pagination.conditionJson = " 1=1 ";
            pagination.p_fields = @"belongdepartid,belongdepart,createuserdeptcode,createuserorgcode,createuserid, createdate,lllegalnumber,lllegaltype, lllegaltypename ,lllegaltime,lllegallevel,
                lllegallevelname, lllegalperson,lllegalpersonid,lllegalteam,lllegalteamcode,lllegaldepart,lllegaldepartcode,lllegaldescribe,lllegaladdress ,lllegalpic,
                reformrequire,flowstate,createusername ,addtype,isexposure,reformpeople,reformpeopleid,reformtel,reformdeptcode,reformdeptname,reformdeadline,reformfinishdate,reformstatus,
reformmeasure,isgrpaccept,acceptpeopleid,acceptpeople,acceptdeptname,acceptdeptcode,acceptresult,acceptmind,accepttime,reseverid,resevertype,reseverone,resevertwo,reseverthree ,participant,filepath,actionpersonname,actionstatus";

            pagination.p_kid = "id";
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
                pagination.conditionJson += string.Format(@" and createuserorgcode  in (select encode from base_department start with encode='{0}' connect by  prior parentid=departmentid)", curUser.OrganizeCode);
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
                pagination.conditionJson += string.Format(@" and  to_char(createdate,'yyyy-MM-dd') = to_char('{0}','yyyy-MM-dd') ", currdate);
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
                    pagination.conditionJson += string.Format(@" and  flowstate  in ('违章登记')  and  createuserid ='{0}'", curUser.UserId);
                    break;
                case "1x"://本人完善
                    pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '违章完善'", curUser.Account + ',');
                    break;
                //已上传违章
                case "2":
                    pagination.conditionJson += string.Format(@" and  flowstate  in ('违章核准','违章整改','违章验收','验收确认','整改结束')  and  createuserid ='{0}' ", curUser.UserId);
                    break;
                //个人处理-待核准列表
                case "3":
                    pagination.conditionJson += string.Format(@" and (','|| substr(participant,2) ||',')  like   '%,{0},%' and flowstate  = '违章核准'", curUser.Account);
                    break;
                //个人处理-待整改列表
                case "4":
                    pagination.conditionJson += string.Format(@" and reformpeopleid  =  '{0}' and flowstate  = '违章整改'  ", curUser.UserId);
                    break;
                //个人处理-待验收列表
                case "5":
                    pagination.conditionJson += string.Format(@" and acceptpeopleid  =  '{0}' and flowstate  = '违章验收'", curUser.UserId);
                    break;
                case "5x"://本人确认
                    pagination.conditionJson += string.Format(@" and  participant||','  like  '%{0}%' and flowstate = '验收确认'", curUser.Account + ',');
                    break;
                //个人处理-逾期未整改列表
                case "6":
                    pagination.conditionJson += string.Format(@" and  reformpeopleid ='{0}' and flowstate  = '违章整改'  and 
                         to_date('{1}','yyyy-mm-dd hh24:mi:ss') > (reformdeadline + 1)", curUser.UserId, DateTime.Now);
                    break;
                //违章曝光
                case "7":
                    //当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  lllegalteamcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid=parentid) ", deptcode);

                    pagination.conditionJson += @" and isexposure = '是'";
                    break;
                //所有确定的违章
                case "8":
                    //当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  lllegalteamcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid=parentid) ", deptcode);

                    pagination.conditionJson += @"  and flowstate != '违章登记'  and  flowstate != '违章完善'  and  flowstate != '违章核准' ";
                    break;
                //逾期未整改列表
                case "9":
                    //当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  lllegalteamcode like '{0}%' ", deptcode);

                    pagination.conditionJson += string.Format(@"  and flowstate  = '违章整改'  and 
                         to_date('{1}','yyyy-mm-dd hh24:mi:ss') > (reformdeadline + 1)", DateTime.Now);
                    break;
                case "10":
                    //违章单位为当前部门及下属单位或创建单位为当前部门及下属单位
                    pagination.conditionJson += string.Format(@" and  (lllegalteamcode like '{0}%' or createuserdeptcode like '{0}%' or reformdeptcode='{0}') ", deptcode);
                    pagination.conditionJson += @"  and flowstate ='整改结束' ";
                    break;
                //对应电厂进来(省级首页指标)
                case "11":
                    pagination.conditionJson += string.Format(@"  and belongdepartid = (select departmentid from base_department where encode ='{0}')  and to_char(createdate,'yyyy')='{1}'", qdeptcode, DateTime.Now.Year);

                    pagination.conditionJson += @"  and flowstate != '违章登记'  and  flowstate != '违章完善'  and  flowstate != '违章核准' ";
                    break;
            }
            if (!string.IsNullOrEmpty(belongcode))
            {//各电厂的反违章
                pagination.conditionJson += string.Format(@" and  lllegalteamcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid=parentid) ", belongcode);
            }
            if (!string.IsNullOrWhiteSpace(regcode))
            {//省公司或电厂登记的反违章
                if (regcode == "1")//省公司登记
                    pagination.conditionJson += " and isgrpaccept is not null";
                else if (regcode == "2")//电厂登记
                    pagination.conditionJson += " and isgrpaccept is null";
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
            //获取用户Id
            string userId = dy.userid;

            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户

            Operator user = OperatorProvider.Provider.Current();

            if (null == user)
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
                        model.chargepersonone = punish.PERSONINCHARGENAME;
                        model.economicspunishone = punish.ECONOMICSPUNISH;
                        model.educationone = punish.EDUCATION;
                        model.lllegalpointone = punish.LLLEGALPOINT;
                        model.awaitjobone = punish.AWAITJOB;
                        model.chargepersontwo = punish.FIRSTINCHARGENAME;
                        model.economicspunishtwo = punish.FIRSTECONOMICSPUNISH;
                        model.educationtwo = punish.FIRSTEDUCATION;
                        model.lllegalpointtwo = punish.FIRSTLLLEGALPOINT;
                        model.awaitjobtwo = punish.FIRSTAWAITJOB;
                        model.chargepersonthree = punish.SECONDINCHARGENAME;
                        model.economicspunishthree = punish.SECONDECONOMICSPUNISH;
                        model.educationthree = punish.SECONDEDUCATION;
                        model.lllegalpointthree = punish.SECONDLLLEGALPOINT;
                        model.awaitjobthree = punish.SECONDAWAITJOB;
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
            //获取用户Id
            string userId = dy.userid;

            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户

            Operator user = OperatorProvider.Provider.Current();

            if (null == user)
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
                    data.Add(model);
                }
            }

            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
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
            //获取用户Id
            string userId = dy.userid;

            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户

            Operator user = OperatorProvider.Provider.Current();

            if (null == user)
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
            //获取用户Id
            string userId = dy.userid;

            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户

            Operator user = OperatorProvider.Provider.Current();

            if (null == user)
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

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

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

                entity.lllegaltype = baseInfo.Rows[0]["lllegaltype"].ToString();//违章类型id
                entity.lllegaltypename = baseInfo.Rows[0]["lllegaltypename"].ToString();//违章类型名称
                entity.lllegaltime = !string.IsNullOrEmpty(baseInfo.Rows[0]["lllegaltime"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["lllegaltime"].ToString()).ToString("yyyy-MM-dd") : ""; //违章时间
                entity.lllegallevel = baseInfo.Rows[0]["lllegallevel"].ToString();//违章级别id
                entity.belongdepart = baseInfo.Rows[0]["belongdepart"].ToString();
                entity.belongdepartid = baseInfo.Rows[0]["belongdepartid"].ToString();
                entity.lllegallevelname = baseInfo.Rows[0]["lllegallevelname"].ToString();//违章级别名称
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
                LllegalConfirmEntity cfrEntity = lllegalconfirmbll.GetEntityByBid(lllegalid);
                if (cfrEntity != null)
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
                entity.chargepersonone = baseInfo.Rows[0]["personinchargename"].ToString();//违章责任人
                entity.chargepersonidone = baseInfo.Rows[0]["personinchargeid"].ToString(); //违章责任人id
                entity.economicspunishone = !string.IsNullOrEmpty(baseInfo.Rows[0]["economicspunish"].ToString()) ? Convert.ToDecimal(baseInfo.Rows[0]["economicspunish"].ToString()) : 0;//经济处罚
                entity.educationone = !string.IsNullOrEmpty(baseInfo.Rows[0]["education"].ToString()) ? Convert.ToDecimal(baseInfo.Rows[0]["education"].ToString()) : 0;//教育培训
                entity.lllegalpointone = !string.IsNullOrEmpty(baseInfo.Rows[0]["lllegalpoint"].ToString()) ? decimal.Parse(baseInfo.Rows[0]["lllegalpoint"].ToString()) : 0;//违章扣分
                entity.awaitjobone = !string.IsNullOrEmpty(baseInfo.Rows[0]["awaitjob"].ToString()) ? decimal.Parse(baseInfo.Rows[0]["awaitjob"].ToString()) : 0; //待岗

                //关联责任人
                List<PunishData> punishdata = new List<PunishData>();
                List<LllegalPunishEntity> lllegalpunishList = lllegalpunishbll.GetListByLllegalId(lllegalid, "1");
                foreach (LllegalPunishEntity punishEntity in lllegalpunishList)
                {
                    PunishData pshdata = new PunishData();
                    pshdata.chargeperson = punishEntity.PERSONINCHARGENAME;
                    pshdata.chargepersonid = punishEntity.PERSONINCHARGEID;
                    pshdata.economicspunish = punishEntity.ECONOMICSPUNISH;
                    pshdata.education = punishEntity.EDUCATION;
                    pshdata.lllegalpoint = punishEntity.LLLEGALPOINT;
                    pshdata.awaitjob = punishEntity.AWAITJOB;
                    punishdata.Add(pshdata);
                }
                entity.punishdata = punishdata;

                string lllegalphoto = baseInfo.Rows[0]["lllegalpic"].ToString();  //违章图片 
                string reformphoto = baseInfo.Rows[0]["reformpic"].ToString();   //违章整改图片
                string acceptphoto = baseInfo.Rows[0]["acceptpic"].ToString();   //违章验收图片
                List<Photo> lllegalpic = new List<Photo>(); //违章图片
                List<Photo> reformpic = new List<Photo>(); //整改图片
                List<Photo> acceptpic = new List<Photo>();  //验收图片 
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

                string userid = dy.userid; //用户ID 

                OperatorProvider.AppUserId = userid;  //设置当前用户

                Operator curUser = OperatorProvider.Provider.Current();

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

                string userid = dy.userid; //用户ID 

                OperatorProvider.AppUserId = userid;  //设置当前用户

                Operator curUser = OperatorProvider.Provider.Current();

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                int year = Convert.ToInt32(dy.data.year);
                var starttime = new DateTime(year, 1, 1);
                var endtime = new DateTime(year, 12, 31);

                var queryJson = new
                {
                    startTime = starttime,
                    endTime = endtime,
                    deptCode = curUser.DeptCode,
                    typeGroups = "作业类,管理类,指挥类,装置类,文明卫生类"
                };
                var dtType = legbll.GetLllegalTypeTotal(queryJson.ToJson());//违章类型数量统计
                var queryJson1 = new
                {
                    year = year,
                    deptCode = curUser.DeptCode,
                    levelGroups = "一般违章,较严重违章,严重违章"
                };
                var dtqs = legbll.GetLllegalTrendData(queryJson1.ToJson());
                var dttotal = GetLllegalTrendTotal(dtqs, queryJson1.ToJson());//趋势图
                var jsonData = new
                {
                    pieType = dtType,
                    lineQs = dttotal
                };
                return new { code = 0, info = "获取数据成功", count = 1, data = jsonData };
            }
            catch (Exception)
            {

                return new { code = 0, info = "获取数据失败", count = 0, data = new DataTable() };
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

                string userid = dy.userid; //用户ID 

                OperatorProvider.AppUserId = userid;  //设置当前用户

                Operator curUser = OperatorProvider.Provider.Current();

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
                var dt = legbll.GetAppLllegalStatistics(deptcode,"", 0);

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
                var rankdt = legbll.GetAppLllegalStatistics(deptCode, year,2);
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

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

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
                    if (dr["Flowstate"].ToString() == "违章验收" || dr["Flowstate"].ToString() == "整改结束")
                        zgYes1 += decimal.Parse(dr["Num"].ToString());
                    else
                        zgNo1 += decimal.Parse(dr["Num"].ToString());
                }
                if (dr["lllegallevelname"].ToString() == "较严重违章")
                {
                    //已整改
                    if (dr["Flowstate"].ToString() == "违章验收" || dr["Flowstate"].ToString() == "整改结束")
                        zgYes2 += decimal.Parse(dr["Num"].ToString());
                    else
                        zgNo2 += decimal.Parse(dr["Num"].ToString());
                }
                if (dr["lllegallevelname"].ToString() == "严重违章")
                {
                    //已整改
                    if (dr["Flowstate"].ToString() == "违章验收" || dr["Flowstate"].ToString() == "整改结束")
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

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

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
            string userid = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string roleNames = curUser.RoleName;
            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = int.Parse(dy.page) <= 0 ? 1 : int.Parse(dy.page);
            pagination.rows = int.Parse(dy.rows) <= 0 ? 1 : int.Parse(dy.rows);

            pagination.p_fields = @"des,leglevel,legLevalName,legtype,legTypeName,bustype,busTypeName,descore,demoney,firstdescore,firstdemoney,seconddescore,seconddemoney,remark";

            pagination.p_kid = "id";
            pagination.p_tablename = @"v_lllegalstdinfo";
            pagination.conditionJson = " 1=1";
            if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户") || curUser.DeptName.Contains("安环部"))
                pagination.conditionJson += " and CREATEUSERDEPTCODE like '" + curUser.OrganizeCode + "%'";
            else
                pagination.conditionJson += " and CREATEUSERDEPTCODE like '" + curUser.DeptCode + "%'";

            //违章类型
            if (dy.lllegaltype.Length > 0)
            {
                pagination.conditionJson += string.Format(@" and  legtype='{0}' ", dy.lllegaltype.ToString());
            }
            //违章级别
            if (dy.lllegallevel.Length > 0)
            {
                pagination.conditionJson += string.Format(@" and leglevel ='{0}'", dy.lllegallevel.ToString());
            }
            //违章描述 
            if (dy.lllegaldescribe.Length > 0)
            {
                pagination.conditionJson += string.Format(@" and des like '%{0}%'", dy.lllegaldescribe.ToString());
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
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
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

            string userId = dy.userid; //当前用户ID 

            OperatorProvider.AppUserId = userId;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

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

                bool isAddScore = false;

                //违章信息
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
                    entity.BELONGDEPARTID = curUser.OrganizeId;
                    entity.BELONGDEPART = curUser.OrganizeName;

                    string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";
                    //违章编号
                    entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));
                }
                //违章 基本信息                
                entity.LLLEGALTYPE = res.Contains("lllegaltype") ? dy.data.lllegaltype : "";// 违章类型id
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

                //先删除图片
                DeleteFile(fileids);

                //上传违章图片
                UploadifyFile(entity.LLLEGALPIC, "lllegalpic", files);
                /********************************/
                //新增
                lllegalregisterbll.SaveForm(lllegalid, entity);

                #endregion

                //创建主体流程
                #region 创建主体流程
                //主键为空  创建主体流程
                if (string.IsNullOrEmpty(lllegalid))
                {
                    string workFlow = string.Empty;
                    //一般整改
                    if (addtype == "0")
                    {
                        workFlow = "03";//违章处理

                    }
                    else  //立即整改  addtype :1 
                    {
                        workFlow = "04";//违章处理
                    }

                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                #endregion

                //整改信息
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
                UploadifyFile(centity.REFORMPIC, "reformpic", files);
                /********************************/
                //新增
                lllegalreformbll.SaveForm(reformid, centity);

                #endregion

                //违章考核信息
                #region 违章考核信息
                LllegalPunishEntity psentity = new LllegalPunishEntity();
                string punishid = res.Contains("punishid") ? dy.data.punishid : "";  //考核项id
                /************考核信息**********/
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    psentity = lllegalpunishbll.GetEntityByBid(lllegalid); //取0的数
                }
                if (null != psentity)
                {
                    punishid = psentity.ID;
                }
                psentity.PERSONINCHARGEID = res.Contains("chargepersonidone") ? dy.data.chargepersonidone : "";  //违章责任人id
                psentity.PERSONINCHARGENAME = res.Contains("chargepersonone") ? dy.data.chargepersonone : "";  //违章责任人
                psentity.ECONOMICSPUNISH = res.Contains("economicspunishone") ? (!string.IsNullOrEmpty(dy.data.economicspunishone) ? Convert.ToDecimal(dy.data.economicspunishone.ToString()) : 0) : 0;  //经济处罚
                psentity.EDUCATION = res.Contains("educationone") ? (!string.IsNullOrEmpty(dy.data.educationone) ? Convert.ToDecimal(dy.data.educationone.ToString()) : 0) : 0;  //教育培训
                psentity.LLLEGALPOINT = res.Contains("lllegalpointone") ? (!string.IsNullOrEmpty(dy.data.lllegalpointone) ? decimal.Parse(dy.data.lllegalpointone.ToString()) : 0) : 0;   //违章扣分
                psentity.AWAITJOB = res.Contains("awaitjobone") ? (!string.IsNullOrEmpty(dy.data.awaitjobone) ? decimal.Parse(dy.data.awaitjobone.ToString()) : 0) : 0;  //待岗
                //违章责任人
                if (string.IsNullOrEmpty(lllegalid))
                {
                    psentity.LLLEGALID = entity.ID;
                    psentity.APPROVEID = string.Empty;
                    psentity.MARK = "0"; //这里的"0"表示考核记录信息，"1"表示违章核准的核准考核记录
                }
                lllegalpunishbll.SaveForm(punishid, psentity); //新增 or 更新

                //先删除关联责任人集合
                lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "1");
                //新增关联责任人
                string punishdata = res.Contains("punishdata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.punishdata) : "";
                if (!string.IsNullOrEmpty(punishdata))
                {
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(punishdata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string personinchargename = rhInfo["chargeperson"].ToString(); //关联责任人姓名
                        string personinchargeid = rhInfo["chargepersonid"].ToString();//关联责任人id
                        string economicspunish = rhInfo["economicspunish"].ToString(); // 经济处罚
                        string education = rhInfo["education"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString();//违章扣分
                        string awaitjob = rhInfo["awaitjob"].ToString();//待岗

                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = "1"; //标记为关联责任人
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }

                #endregion

                //验收信息
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
                UploadifyFile(aentity.ACCEPTPIC, "acceptpic", files);
                /********************************/
                //保存
                lllegalacceptbll.SaveForm(acceptid, aentity);
                #endregion

                //一次性提交违章流程
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
                    lllegalapprovebll.SaveForm("", apentity);
                    #endregion
                    //提交流程

                    wfFlag = "1";//整改结束

                    participant = curUser.Account;

                    //用户考核积分管理
                    lllegalpunishbll.SaveUserScore(psentity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                    //关联责任人
                    var relevanceList = lllegalpunishbll.GetListByLllegalId(entity.ID, "1");
                    foreach (LllegalPunishEntity lpEntity in relevanceList)
                    {
                        //违章责任人
                        lllegalpunishbll.SaveUserScore(lpEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                    }

                    //提交流程
                    int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                #endregion

                #region 非已整改违章提交
                else
                {
                    //取当前违章信息
                    var regEntity = lllegalregisterbll.GetEntity(entity.ID);

                    startflow = regEntity.FLOWSTATE; //起始流程状态

                    IList<UserEntity> ulist = new List<UserEntity>();

                    //省公司、分子公司用户
                    if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("GrpUser")).Rows.Count > 0)
                    {
                        endflow = "违章完善";

                        wfFlag = "4";  // 登记=>完善     
                        //取安全主管部门用户 完善
                        participant = userbll.GetSafetyDeviceDeptUser("0", entity.BELONGDEPARTID);
                    }
                    //外包单位人员提交到发包单位
                    else if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0)
                    {
                        endflow = "违章核准";

                        wfFlag = "3";  // 登记=>核准
                        //取发包单位 核准
                        participant = userbll.GetSafetyDeviceDeptUser("3", curUser);
                    }
                    else  //其他层级的用户 
                    {
                        //安全管理部门用户提交
                        if (userbll.GetSafetyAndDeviceDept(curUser).Contains("1"))
                        {
                            if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                            {
                                endflow = "违章整改";

                                wfFlag = "2";  // 登记=>整改

                                //如果非装置类 则提交到整改
                                UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象
                                //取整改人
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                            else
                            {

                                //装置类  则提交到装置部门核准
                                var lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;
                                //如果当前选择的是装置类 取装置单位 下账户
                                if (lllegatypename == dataitemdetailbll.GetItemValue("ApplianceClass"))
                                {
                                    endflow = "违章核准";

                                    wfFlag = "3";  // 登记=>核准

                                    //取装置用户
                                    participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                                }
                                else  //非装置类
                                {
                                    endflow = "违章整改";

                                    wfFlag = "2";  // 登记=>整改

                                    //如果非装置类 则提交到整改
                                    UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象
                                    //取整改人
                                    participant = reformUser.Account;

                                    isAddScore = true;
                                }
                            }

                        }
                        //装置部门人员
                        else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                        {
                            endflow = "违章整改";

                            wfFlag = "2";  // 登记=>整改

                            //如果非装置类 则提交到整改
                            UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象

                            //取整改人
                            participant = reformUser.Account;

                            isAddScore = true;
                        }
                        else  //非安全管理部门提交到安全管理部门核准
                        {

                            //负责人提交，如果没有上报则直接整改，反之直接提交到安全管理部门核准(二次核准)
                            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0)
                            {
                                //上报
                                if (entity.ISUPSAFETY == "1")
                                {
                                    endflow = "违章核准";

                                    wfFlag = "3";  // 登记=>核准

                                    if (curUser.RoleName.Contains("班组级用户") || curUser.RoleName.Contains("专业级用户"))
                                    {
                                        // errorMsg = "部门级安全管理员用户";//上级部门安全员
                                        //取安全管理部门用户 
                                        participant = userbll.GetSafetyDeviceDeptUser("4", curUser);  //取上级(部门级)安全管理员的用户
                                    }
                                    else
                                    {
                                        // errorMsg = "安全部门管理员用户";//上级部门安全员
                                        //取安全管理部门用户 
                                        participant = userbll.GetSafetyDeviceDeptUser("0", curUser);  //取安全部门管理员
                                    }
                                }
                                else  //不上报
                                {
                                    endflow = "违章整改";

                                    wfFlag = "2";  // 登记=>整改

                                    UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象
                                    //取整改人
                                    participant = reformUser.Account;

                                    isAddScore = true;
                                }
                            }
                            else  //提交到班组负责人处核准
                            {
                                endflow = "违章核准";

                                wfFlag = "3";  // 登记=>核准
                                //取班组负责人
                                participant = userbll.GetSafetyDeviceDeptUser("2", curUser);
                            }
                        }
                    }



                    //添加用户积分关联
                    if (isAddScore)
                    {
                        //用户考核积分管理
                        lllegalpunishbll.SaveUserScore(psentity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                        //关联责任人
                        var relevanceList = lllegalpunishbll.GetListByLllegalId(entity.ID, "1");
                        foreach (LllegalPunishEntity lpEntity in relevanceList)
                        {
                            //违章责任人
                            lllegalpunishbll.SaveUserScore(lpEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                        }
                    }

                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(startflow, endflow, entity.ID, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "未找到下一节操作人员，流程推送失败。" };
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

            string userId = dy.userid; //当前用户ID 

            OperatorProvider.AppUserId = userId;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                string addtype = dy.data.addtype; //新增类型  一般整改违章 0    立即整改违章 1  

                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键

                //违章信息
                #region 违章登记信息
                LllegalRegisterEntity entity = new LllegalRegisterEntity();
                //违章 基本信息
                entity.ID = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键
                //新增时
                if (string.IsNullOrEmpty(lllegalid))
                {
                    string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";
                    //违章编号
                    entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));
                }
                entity.LLLEGALTYPE = res.Contains("lllegaltype") ? dy.data.lllegaltype : "";// 违章类型id
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
                UploadifyFile(entity.LLLEGALPIC, "lllegalpic", files);
                /********************************/
                //新增
                lllegalregisterbll.SaveForm(lllegalid, entity);

                #endregion

                //创建主体流程
                #region 创建主体流程
                //主键为空  创建主体流程
                if (string.IsNullOrEmpty(lllegalid))
                {
                    string workFlow = string.Empty;
                    //一般整改
                    if (addtype == "0")
                    {
                        workFlow = "03";//违章处理

                    }
                    else  //立即整改  addtype :1 
                    {
                        workFlow = "04";//违章处理
                    }

                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                #endregion

                //整改信息
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
                UploadifyFile(centity.REFORMPIC, "reformpic", files);
                /********************************/
                //新增
                lllegalreformbll.SaveForm(reformid, centity);

                #endregion

                //违章考核信息
                #region 违章考核信息
                LllegalPunishEntity psentity = new LllegalPunishEntity();
                string punishid = res.Contains("punishid") ? dy.data.punishid : "";  //考核项id
                /************考核信息**********/
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    psentity = lllegalpunishbll.GetEntityByBid(lllegalid); //取0的数
                }
                if (null != psentity)
                {
                    punishid = psentity.ID;
                }
                psentity.PERSONINCHARGEID = res.Contains("chargepersonidone") ? dy.data.chargepersonidone : "";  //违章责任人id
                psentity.PERSONINCHARGENAME = res.Contains("chargepersonone") ? dy.data.chargepersonone : "";  //违章责任人
                psentity.ECONOMICSPUNISH = res.Contains("economicspunishone") ? (!string.IsNullOrEmpty(dy.data.economicspunishone) ? Convert.ToDecimal(dy.data.economicspunishone.ToString()) : 0) : 0;  //经济处罚
                psentity.EDUCATION = res.Contains("educationone") ? (!string.IsNullOrEmpty(dy.data.educationone) ? Convert.ToDecimal(dy.data.educationone.ToString()) : 0) : 0;  //教育培训
                psentity.LLLEGALPOINT = res.Contains("lllegalpointone") ? (!string.IsNullOrEmpty(dy.data.lllegalpointone) ? decimal.Parse(dy.data.lllegalpointone.ToString()) : 0) : 0;   //违章扣分
                psentity.AWAITJOB = res.Contains("awaitjobone") ? (!string.IsNullOrEmpty(dy.data.awaitjobone) ? decimal.Parse(dy.data.awaitjobone.ToString()) : 0) : 0;  //待岗
                //新增的时候，增加考核
                if (string.IsNullOrEmpty(lllegalid))
                {
                    psentity.LLLEGALID = entity.ID;
                    psentity.APPROVEID = string.Empty;
                    psentity.MARK = "0"; //这里的"0"表示考核记录信息，"1"表示违章核准的核准考核记录
                }
                lllegalpunishbll.SaveForm(punishid, psentity); //新增 or 更新

                //先删除关联责任人集合
                lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "1");
                //新增关联责任人
                string punishdata = res.Contains("punishdata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.punishdata) : "";
                if (!string.IsNullOrEmpty(punishdata))
                {
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(punishdata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string personinchargename = rhInfo["chargeperson"].ToString(); //关联责任人姓名
                        string personinchargeid = rhInfo["chargepersonid"].ToString();//关联责任人id
                        string economicspunish = rhInfo["economicspunish"].ToString(); // 经济处罚
                        string education = rhInfo["education"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString();//违章扣分
                        string awaitjob = rhInfo["awaitjob"].ToString();//待岗

                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = "1"; //标记为关联责任人
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }
                #endregion

                //验收信息
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
                //{"ORA-01400: 无法将 NULL 插入 (\"ERCHTMS\".\"BIS_LLLEGALACCEPT\".\"LLLEGALID\")"}
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

            string userId = dy.userid; // 用户ID

            OperatorProvider.AppUserId = userId;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

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

                bool isSubmit = true; //是否要执行提交步骤,安全管理部门用于控制装置类违章转发至装置部门

                bool isAddScore = false;

                string startflow = "违章核准";

                string endflow = string.Empty;

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

                //不通过,退回到违章登记，不管是I级核准还是II级核准
                #region 不通过,退回到违章登记，不管是I级核准还是II级核准
                if (approveresult == "0")
                {
                    var regEntity = lllegalregisterbll.GetEntity(lllegalid);
                    var regUser = userbll.GetEntity(regEntity.CREATEUSERID);
                    if (regUser.RoleName.Contains("省级用户"))
                    {
                        endflow = "违章完善";
                        //省公司违章回退到完善
                        wfFlag = "3";  // 核准=>完善                      
                        //取安全主管部门用户 完善
                        participant = userbll.GetSafetyDeviceDeptUser("0", regEntity.BELONGDEPARTID);
                    }
                    else
                    {
                        endflow = "违章登记";
                        wfFlag = "2"; //核准=>登记
                        string createuserid = regEntity.CREATEUSERID;
                        UserEntity userEntity = userbll.GetEntity(createuserid);
                        participant = userEntity.Account;  //登记用户
                    }
                }
                else  //核准通过
                {
                    // 安全管理部门人员
                    if (userbll.GetSafetyAndDeviceDept(curUser).Contains("1"))
                    {
                        var lllegatype = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE); //违章类型

                        string lllegatypename = string.Empty; //违章类型

                        if (null != lllegatype)
                        {
                            lllegatypename = lllegatype.ItemName; //违章类型名称
                        }

                        //当前人有且是装置部门，直接到整改
                        if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                        {
                            endflow = "违章整改";
                            //取整改人
                            wfFlag = "1";  // 核准=>整改
                            //如果非装置类 则提交到整改
                            UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象
                            //取整改人
                            participant = reformUser.Account;

                            isAddScore = true;
                        }
                        else
                        {
                            //判断是否装置类违章
                            if (lllegatypename == dataitemdetailbll.GetItemValue("ApplianceClass"))
                            {
                                endflow = "违章核准";
                                //更改核准人账号，变更为装置部门用户  此步步需要更改状态
                                isSubmit = false;
                                //取装置部门用户
                                participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                            }
                            else
                            {
                                endflow = "违章整改";
                                //如果是非装置类违章，通过则进行整改
                                //取整改人
                                wfFlag = "1";  // 核准=>整改
                                //如果非装置类 则提交到整改
                                UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象
                                //取整改人
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                        }
                    }
                    //装置用户
                    else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                    {
                        endflow = "违章整改";
                        wfFlag = "1";  // 核准=>整改
                        //如果非装置类 则提交到整改
                        UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象
                        //取整改人
                        participant = reformUser.Account;

                        isAddScore = true;
                    }
                    else  //其他部门人员 
                    {

                        //上报情况下
                        if (entity.ISUPSAFETY == "1")
                        {
                            endflow = "违章核准";
                            //核准=>核准
                            isSubmit = false;  //不改变流程状态
                            if (curUser.RoleName.Contains("班组级用户") || curUser.RoleName.Contains("专业级用户"))
                            {
                                // errorMsg = "部门级安全管理员用户";//上级部门安全员
                                //取安全管理部门用户 
                                participant = userbll.GetSafetyDeviceDeptUser("4", curUser);  //取上级(部门级)安全管理员的用户
                            }
                            else
                            {
                                // errorMsg = "安全部门管理员用户";//上级部门安全员
                                //取安全管理部门用户 
                                participant = userbll.GetSafetyDeviceDeptUser("0", curUser);  //取安全部门管理员
                            }
                        }
                        else //不上报情况下，提交到整改 
                        {
                            endflow = "违章整改";
                            wfFlag = "1";  //核准=>整改
                            //如果非装置类 则提交到整改
                            UserEntity reformUser = userbll.GetEntity(centity.REFORMPEOPLEID); //整改用户对象
                            //取整改人
                            participant = reformUser.Account;

                            isAddScore = true;

                        }
                    }
                }
                #endregion

                //保存核准基本信息 (不执行真正意义上的提交,则无法进行核准)
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
                lllegalapprovebll.SaveForm("", apentity);

                #endregion

                //新增考核内容信息(特别针对核准过程)
                #region 违章考核信息
                /************考核信息**********/
                //新增的时候，增加考核
                string punishid = res.Contains("punishid") ? dy.data.punishid : "";  //考核项id
                /************考核信息**********/
                if (!string.IsNullOrEmpty(lllegalid))
                {
                    psentity = lllegalpunishbll.GetEntityByBid(lllegalid); //取0的数
                }
                if (null != psentity)
                {
                    punishid = psentity.ID;
                }
                psentity.LLLEGALID = entity.ID;
                psentity.APPROVEID = string.Empty; //保存对应的核准记录id
                psentity.PERSONINCHARGEID = res.Contains("chargepersonidone") ? dy.data.chargepersonidone : "";  //违章责任人id
                psentity.PERSONINCHARGENAME = res.Contains("chargepersonone") ? dy.data.chargepersonone : "";  //违章责任人
                psentity.ECONOMICSPUNISH = res.Contains("economicspunishone") ? (!string.IsNullOrEmpty(dy.data.economicspunishone) ? Convert.ToDecimal(dy.data.economicspunishone.ToString()) : 0) : 0;  //经济处罚
                psentity.EDUCATION = res.Contains("educationone") ? (!string.IsNullOrEmpty(dy.data.educationone) ? Convert.ToDecimal(dy.data.educationone.ToString()) : 0) : 0;  //教育培训
                psentity.LLLEGALPOINT = res.Contains("lllegalpointone") ? (!string.IsNullOrEmpty(dy.data.lllegalpointone) ? decimal.Parse(dy.data.lllegalpointone.ToString()) : 0) : 0; //违章扣分
                psentity.AWAITJOB = res.Contains("awaitjobone") ? (!string.IsNullOrEmpty(dy.data.awaitjobone) ? decimal.Parse(dy.data.awaitjobone.ToString()) : 0) : 0; //待岗
                psentity.MARK = "0"; //这里的"0"表示考核记录信息，"1"表示违章核准的核准考核记录
                lllegalpunishbll.SaveForm(psentity.ID, psentity);

                //先删除关联责任人集合
                lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "1");
                //新增关联责任人
                string punishdata = res.Contains("punishdata") ? Newtonsoft.Json.JsonConvert.SerializeObject(dy.data.punishdata) : "";
                if (!string.IsNullOrEmpty(punishdata))
                {
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(punishdata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string personinchargename = rhInfo["chargeperson"].ToString(); //关联责任人姓名
                        string personinchargeid = rhInfo["chargepersonid"].ToString();//关联责任人id
                        string economicspunish = rhInfo["economicspunish"].ToString(); // 经济处罚
                        string education = rhInfo["education"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString();//违章扣分
                        string awaitjob = rhInfo["awaitjob"].ToString();//待岗

                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = "1"; //标记为关联责任人
                        lllegalpunishbll.SaveForm("", newpunishEntity);
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

                //添加用户积分关联
                if (isAddScore)
                {
                    //用户考核积分管理
                    lllegalpunishbll.SaveUserScore(psentity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                    //关联责任人
                    var relevanceList = lllegalpunishbll.GetListByLllegalId(entity.ID, "1");
                    foreach (LllegalPunishEntity lpEntity in relevanceList)
                    {
                        //违章责任人
                        lllegalpunishbll.SaveUserScore(lpEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                    }
                }

                //确定要提交
                if (isSubmit)
                {
                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(startflow, endflow, lllegalid, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                        }
                    }
                }
                else  //安全管理部门对装置类违章进行转发，转发至装置部门单位下，无需更改流程状态
                {
                    htworkflowbll.SubmitWorkFlowNoChangeStatus(startflow, endflow, lllegalid, participant, curUser.UserId);
                }
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message.ToString() };
            }
            return new { code = 0, count = 0, info = "保存成功" };
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

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {

                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                string participant = "";

                string startflow = "违章整改";

                string endflow = string.Empty;

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

                //违章整改图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件

                //先删除图片
                DeleteFile(fileids);

                /**********图片部分缺省**********/
                //上传违章整改图片
                UploadifyFile(centity.REFORMPIC, "reformpic", files);
                /********************************/
                //更改
                lllegalreformbll.SaveForm(reformid, centity);


                //回退操作
                if (isback == "是")
                {
                    DataTable dt = htworkflowbll.GetBackFlowObjectByKey(lllegalid);

                    if (dt.Rows.Count > 0)
                    {
                        wfFlag = dt.Rows[0]["wfflag"].ToString(); //流程走向

                        participant = dt.Rows[0]["participant"].ToString();  //指向人

                        endflow = dt.Rows[0]["fromname"].ToString();
                    }
                }
                else
                {
                    endflow = "违章验收";

                    wfFlag = "1";
                    //获取验收人
                    UserEntity userEntity = userbll.GetEntity(acceptpeopleid); //验收人

                    if (null != userEntity)
                    {
                        participant = userEntity.Account;  //获取流程下一节点的参与人员 (取验收人)
                    }
                }

                //提交流程到下一节点
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(startflow, endflow, lllegalid, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                    }
                }
                #endregion
            }
            catch (Exception)
            {

                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
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

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {

                HttpFileCollection files = ctx.Request.Files;//上传的文件 

                string startflow = "违章验收";

                string endflow = string.Empty;

                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键

                string wfFlag = string.Empty;  //流程标识

                string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)

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

                /**********图片部分缺省**********/
                UploadifyFile(aentity.ACCEPTPIC, "acceptpic", files);
                /********************************/
                //保存
                lllegalacceptbll.SaveForm(acceptid, aentity);
                #endregion

                //不通过
                if (aentity.ACCEPTRESULT == "0")
                {
                    //整改记录
                    LllegalReformEntity reformEntity = lllegalreformbll.GetEntityByBid(lllegalid);
                    LllegalReformEntity newEntity = new LllegalReformEntity();
                    newEntity = reformEntity;
                    newEntity.CREATEDATE = DateTime.Now;
                    newEntity.MODIFYDATE = DateTime.Now;
                    newEntity.MODIFYUSERID = curUser.UserId;
                    newEntity.MODIFYUSERNAME = curUser.UserName;
                    newEntity.REFORMPIC = Guid.NewGuid().ToString(); //重新生成图片GUID
                    newEntity.REFORMSTATUS = null; //整改完成情况
                    newEntity.REFORMMEASURE = null; //整改具体措施
                    newEntity.REFORMFINISHDATE = null; //整改完成时间
                    newEntity.ID = "";
                    lllegalreformbll.SaveForm("", newEntity);

                    //验收记录
                    LllegalAcceptEntity cptEntity = new LllegalAcceptEntity();
                    cptEntity = aentity;
                    cptEntity.ID = null;
                    cptEntity.CREATEDATE = DateTime.Now;
                    cptEntity.MODIFYDATE = DateTime.Now;
                    cptEntity.ACCEPTRESULT = null;
                    cptEntity.ACCEPTMIND = null;
                    cptEntity.ACCEPTPIC = Guid.NewGuid().ToString();
                    lllegalacceptbll.SaveForm("", cptEntity);

                    endflow = "违章整改";

                    wfFlag = "2";  // 违章整改

                    UserEntity reformUser = userbll.GetEntity(reformEntity.REFORMPEOPLEID); //整改用户对象
                    //取整改人
                    participant = reformUser.Account;
                }
                else  //通过的情况下
                {
                    if (aentity.ISGRPACCEPT == "否")
                    {
                        endflow = "验收确认";
                        //省公司登记的违章，且要求电厂验收，需要省公司验收确认
                        wfFlag = "3";//验收确认
                        var userId = lllegalregisterbll.GetEntity(lllegalid).CREATEUSERID;
                        var confirmUser = userbll.GetEntity(userId);//登记人
                        participant = confirmUser.Account;
                    }
                    else
                    {
                        endflow = "整改结束";

                        wfFlag = "1";  // 整改结束

                        participant = curUser.Account;
                    }
                }

                //提交流程到下一节点
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(startflow, endflow, lllegalid, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                    }
                }


            }
            catch (Exception)
            {
                return new { code = 0, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
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

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {

                string lllegalid = res.Contains("lllegalid") ? dy.data.lllegalid : "";  //主键

                string wfFlag = string.Empty;  //流程标识

                string startflow = "验收确认";

                string endflow = string.Empty;

                string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)

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
                string confirmtime = res.Contains("confirmtime") ? dy.data.confirmtime : "";
                if (!string.IsNullOrEmpty(confirmtime))
                {
                    confirmEntity.CONFIRMTIME = Convert.ToDateTime(confirmtime);
                }
                confirmEntity.MODIFYDATE = DateTime.Now;
                confirmEntity.MODIFYUSERID = curUser.UserId;
                confirmEntity.MODIFYUSERNAME = curUser.UserName;
                lllegalconfirmbll.SaveForm(confirmEntity.ID, confirmEntity);

                //不通过
                if (confirmEntity.CONFIRMRESULT == "0")
                {
                    //整改记录
                    LllegalReformEntity reformEntity = lllegalreformbll.GetEntityByBid(lllegalid);
                    LllegalReformEntity newEntity = new LllegalReformEntity();
                    newEntity = reformEntity;
                    newEntity.CREATEDATE = DateTime.Now;
                    newEntity.MODIFYDATE = DateTime.Now;
                    newEntity.MODIFYUSERID = curUser.UserId;
                    newEntity.MODIFYUSERNAME = curUser.UserName;
                    newEntity.REFORMPIC = null; //重新生成图片GUID
                    newEntity.REFORMSTATUS = null; //整改完成情况
                    newEntity.REFORMMEASURE = null; //整改具体措施
                    newEntity.REFORMFINISHDATE = null; //整改完成时间
                    newEntity.ID = "";
                    lllegalreformbll.SaveForm("", newEntity);

                    //验收确认记录
                    LllegalConfirmEntity cptEntity = new LllegalConfirmEntity();
                    cptEntity = confirmEntity;
                    cptEntity.ID = null;
                    cptEntity.CREATEDATE = DateTime.Now;
                    cptEntity.MODIFYDATE = DateTime.Now;
                    cptEntity.CONFIRMRESULT = null;
                    cptEntity.CONFIRMMIND = null;
                    lllegalconfirmbll.SaveForm("", cptEntity);

                    endflow = "违章整改";

                    wfFlag = "2";  // 违章整改

                    UserEntity reformUser = userbll.GetEntity(reformEntity.REFORMPEOPLEID); //整改用户对象
                    //取整改人
                    participant = reformUser.Account;
                }
                else  //通过的情况下
                {
                    endflow = "整改结束";

                    wfFlag = "1";  // 验收确认结束

                    participant = curUser.Account;
                }

                //提交流程到下一节点
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(startflow, endflow, lllegalid, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", lllegalid);  //更新业务流程状态
                    }
                }
            }
            catch (Exception)
            {
                return new { code = 0, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
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

        public string chargepersonone { get; set; }  //违章责任人
        public decimal? economicspunishone { get; set; } //经济处罚
        public decimal? educationone { get; set; } //教育培训
        public decimal? lllegalpointone { get; set; }  //违章扣分
        public decimal? awaitjobone { get; set; }  //待岗

        public string chargepersontwo { get; set; }  //违章责任人(第一联)
        public decimal? economicspunishtwo { get; set; } //经济处罚
        public decimal? educationtwo { get; set; } //教育培训
        public decimal? lllegalpointtwo { get; set; }  //违章扣分
        public decimal? awaitjobtwo { get; set; }  //待岗

        public string chargepersonthree { get; set; }  //违章责任人(第二联) 
        public decimal? economicspunishthree { get; set; } //经济处罚
        public decimal? educationthree { get; set; } //教育培训
        public decimal? lllegalpointthree { get; set; }  //违章扣分
        public decimal? awaitjobthree { get; set; }  //待岗 
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
        public List<Photo> reformpic { get; set; }  //整改图片
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

        /*  考核内容 */
        public string chargepersonone { get; set; }  //违章责任人
        public string chargepersonidone { get; set; }  //违章责任人id
        public decimal? economicspunishone { get; set; } //经济处罚
        public decimal? educationone { get; set; } //教育培训
        public decimal? lllegalpointone { get; set; }  //违章扣分
        public decimal? awaitjobone { get; set; }  //待岗
        public List<PunishData> punishdata { get; set; }
    }
    #endregion

    public class PunishData
    {
        public string chargeperson { get; set; }  //违章责任人
        public string chargepersonid { get; set; }  //违章责任人id
        public decimal? economicspunish { get; set; } //经济处罚
        public decimal? education { get; set; } //教育培训
        public decimal? lllegalpoint { get; set; }  //违章扣分
        public decimal? awaitjob { get; set; }  //待岗
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
}