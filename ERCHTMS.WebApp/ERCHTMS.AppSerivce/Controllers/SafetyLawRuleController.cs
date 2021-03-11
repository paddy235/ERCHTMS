using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.EngineeringManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SafetyLawManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EngineeringManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SafetyLawManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BSFramework.Util.WebControl;
using System.Text.RegularExpressions;
using ERCHTMS.Busines.HiddenTroubleManage;
using System.Text;
using ERCHTMS.Busines.EquipmentManage;
using System.Net;
using ERCHTMS.Entity.SystemManage;
using System.Net.Http;
using BSFramework.Util;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class SafetyLawRuleController : BaseApiController
    {
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DataItemBLL dataItemBLL = new DataItemBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private PerilEngineeringBLL pbll = new PerilEngineeringBLL();
        private SafetyLawBLL safetylawbll = new SafetyLawBLL();
        private AccidentCaseLawBLL accidentcaselawbll = new AccidentCaseLawBLL();
        private SafeInstitutionBLL safeinstitutionbll = new SafeInstitutionBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private SafeStandardsBLL safestandardsbll = new SafeStandardsBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
        SpecialEquipmentBLL seb = new SpecialEquipmentBLL();
        private AreaBLL areaBLL = new AreaBLL();
        private SafeInstitutionTreeBLL safeinstitutiontreebll = new SafeInstitutionTreeBLL();
        public HttpContext hcontent { get { return HttpContext.Current; } }

        #region 获取列表
        /// <summary>
        /// 根据查询条件获取数据列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getSafetyLawList([FromBody]JObject json)
        {

            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var reqParam = JsonConvert.DeserializeAnonymousType(res, new
                {
                    data = new
                    {
                        ORGANIZECODE = string.Empty
                    }
                });
                string filename = dy.data.filename;//文件名称
                string type = dy.data.type;//类型（1:生产法律法规 2.安全管理制度 3.安全操作规程 4.安全法律法规最新五条数据）
                string organizecode = string.IsNullOrEmpty(reqParam.data.ORGANIZECODE) ? user.OrganizeCode : reqParam.data.ORGANIZECODE;
                string orgcode = reqParam.data.ORGANIZECODE;
                string strwhere = "";
                string sql = "";
                if (!string.IsNullOrEmpty(filename))
                {
                    strwhere += " and filename like '%" + filename + "%'";
                }
                string codewhere = "";
                if (user.DeptCode == user.OrganizeCode)
                {
                    codewhere = "";
                }
                else
                {
                    var provdata = departmentBLL.GetList().Where(t => t.Nature == "省级" && !t.FullName.Contains("各电厂") && t.FullName != "区域子公司" && t.DeptCode.StartsWith(user.NewDeptCode.Substring(0, 3)));
                    DepartmentEntity provEntity = null;
                    //省级根节点
                    if (provdata.Count() > 0)
                    {
                        provEntity = provdata.FirstOrDefault();
                        codewhere = string.Format(" or createuserorgcode='{0}'", provEntity.EnCode);
                    }
                }

                if (type == "1") //安全生产法律法规
                {
                    if (!string.IsNullOrEmpty(orgcode))
                    {
                        if (orgcode == "省级")
                        {
                            sql = string.Format(@"select id as fileid,filename,issuedept,to_char(carrydate,'yyyy-mm-dd') as carrydate  from bis_safetylaw  where (createuserorgcode='00' {1}) {0} order by createdate desc", strwhere, codewhere);
                        }
                        else
                        {
                            sql = string.Format(@"select id as fileid,filename,issuedept,to_char(carrydate,'yyyy-mm-dd') as carrydate  from bis_safetylaw  where (createuserorgcode='{1}') {0} order by createdate desc", strwhere, orgcode);
                        }
                    }
                    else
                    {
                        if (user.RoleName.Contains("省级用户"))
                        {
                            sql = string.Format(@"select id as fileid,filename,issuedept,to_char(carrydate,'yyyy-mm-dd') as carrydate  from bis_safetylaw  where (createuserorgcode='00' or createuserorgcode='{1}') {0} order by createdate desc", strwhere, user.OrganizeCode);
                        }
                        else
                        {
                            sql = string.Format(@"select id as fileid,filename,issuedept,to_char(carrydate,'yyyy-mm-dd') as carrydate  from bis_safetylaw  where (createuserorgcode='{1}' {2} or createuserorgcode='00') {0} order by createdate desc", strwhere, user.OrganizeCode, codewhere);
                        }
                    }
                }
                if (type == "2")
                {
                    if (!string.IsNullOrEmpty(orgcode))
                    {
                        if (orgcode == "省级")
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safeinstitution a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='InstitutionLaw') and  (a.createuserorgcode='00' {1}) {0} order by a.createdate desc", strwhere, codewhere);
                        }
                        else
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safeinstitution a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='InstitutionLaw') and  (a.createuserorgcode='{1}')  {0} order by a.createdate desc", strwhere, orgcode);
                        }
                    }
                    else
                    {
                        //安全管理制度
                        if (user.RoleName.Contains("省级用户"))
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safeinstitution a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='InstitutionLaw') and  (a.createuserorgcode in (select encode from base_department where deptcode like '{1}%') or a.createuserorgcode='00') {0} order by a.createdate desc", strwhere, organizecode);
                        }
                        else
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safeinstitution a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='InstitutionLaw') and  (a.createuserorgcode='{1}' {2} or a.createuserorgcode='00') {0} order by a.createdate desc", strwhere, user.OrganizeCode, codewhere);
                        }
                    }
                }

                if (type == "3")
                {
                    if (!string.IsNullOrEmpty(orgcode))
                    {
                        if (orgcode == "省级")
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safestandards a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='StandardsLaw') and (a.createuserorgcode='00' {1}) {0} order by a.createdate desc", strwhere, codewhere);
                        }
                        else
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safestandards a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='StandardsLaw') and (a.createuserorgcode='{1}') {0} order by a.createdate desc", strwhere, orgcode);
                        }
                    }
                    else
                    {
                        if (user.RoleName.Contains("省级用户"))
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safestandards a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='StandardsLaw') and (a.createuserorgcode in (select encode from base_department where deptcode  like '{1}%') or a.createuserorgcode='00') {0} order by a.createdate desc", strwhere, organizecode);
                        }
                        else
                        {
                            sql = string.Format(@"select id as fileid,filename,b.itemname as lawtypecode,to_char(carrydate,'yyyy-mm-dd') as carrydate  from  bis_safestandards a left join base_dataitemdetail b on a.lawtypecode=b.itemvalue where itemid =(select  itemid from base_dataitem where itemcode='StandardsLaw') and (a.createuserorgcode='{1}' {2} or a.createuserorgcode='00') {0} order by a.createdate desc", strwhere, user.OrganizeCode, codewhere);
                        }
                    }
                }

                if (type == "4")
                {
                    sql = string.Format(@"select a.* from (select  id as fileid,filename,issuedept,to_char(carrydate,'yyyy-mm-dd') as carrydate  from bis_safetylaw where createuserorgcode='{1}' {0}  order by createdate desc)a where rownum <=5 ", strwhere, user.OrganizeCode);
                }
                DataTable dt = pbll.GetPerilEngineeringList(sql);
                return new
                {
                    Code = 0,
                    Count = dt.Rows.Count,
                    Info = "获取数据成功",
                    data = dt
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取所属来源
        /// <summary>
        /// 获取查询列表所属来源
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getDeptList([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string userId = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator user = OperatorProvider.Provider.Current();

            IList<DeptData> result = new List<DeptData>();
            var provdata = departmentBLL.GetList().Where(t => t.Nature == "省级" && !t.FullName.Contains("各电厂") && t.FullName != "区域子公司" && t.DeptCode.StartsWith(user.NewDeptCode.Substring(0, 3)));
            DepartmentEntity provEntity = null;
            //省级根节点
            if (provdata.Count() > 0)
            {
                provEntity = provdata.FirstOrDefault();
                DeptData deptdata1 = new DeptData();
                deptdata1.deptid = provEntity.DepartmentId;
                deptdata1.code = provEntity.EnCode;
                deptdata1.name = provEntity.FullName;
                deptdata1.newcode = provEntity.DeptCode;
                result.Add(deptdata1);
            }
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

        #region 获取安全生产法律法规详情
        /// <summary>
        /// 获取安全生产法律法规详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getLawDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string fileid = dy.data.fileid;
                SafetyLawEntity entity = safetylawbll.GetEntity(fileid);
                DataTable cdt = fileInfoBLL.GetFiles(entity.FilesId);
                IList<Photo> cfiles = new List<Photo>(); //资料文件
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new { fileid = entity.Id, filename = entity.FileName, lawarea = entity.LawArea, filecode = entity.FileCode, issuedept = entity.IssueDept, carrydate = string.IsNullOrEmpty(entity.CarryDate.ToString()) ? "" : Convert.ToDateTime(entity.CarryDate).ToString("yyyy-MM-dd"), validversions = entity.ValidVersions, cfiles = cfiles }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取安全管理制度详情
        /// <summary>
        /// 获取安全管理制度详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getInstitutionDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string fileid = dy.data.fileid;
                SafeInstitutionEntity entity = safeinstitutionbll.GetEntity(fileid);
                DataTable cdt = fileInfoBLL.GetFiles(entity.FilesId);
                IList<Photo> cfiles = new List<Photo>(); //资料文件
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new { fileid = entity.Id, filename = entity.FileName, issuedept = entity.IssueDept, filecode = entity.FileCode, lawtypecode = getName(entity.LawTypeCode, "InstitutionLaw"), carrydate = string.IsNullOrEmpty(entity.CarryDate.ToString()) ? "" : Convert.ToDateTime(entity.CarryDate).ToString("yyyy-MM-dd"), validversions = entity.ValidVersions, cfiles = cfiles }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取操作规程详情
        /// <summary>
        /// 获取操作规程详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getStandardsDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string fileid = dy.data.fileid;
                SafeStandardsEntity entity = safestandardsbll.GetEntity(fileid);
                DataTable cdt = fileInfoBLL.GetFiles(entity.FilesId);
                IList<Photo> cfiles = new List<Photo>(); //资料文件
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new { fileid = entity.Id, filename = entity.FileName, issuedept = entity.IssueDept, filecode = entity.FileCode, lawtypecode = getName(entity.LawTypeCode, "StandardsLaw"), carrydate = string.IsNullOrEmpty(entity.CarryDate.ToString()) ? "" : Convert.ToDateTime(entity.CarryDate).ToString("yyyy-MM-dd"), validversions = entity.ValidVersions, cfiles = cfiles }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion


        public string getName(string type, string itemcode)
        {
            var cName = dataItemCache.GetDataItemList(itemcode).Where(a => a.ItemValue == type).FirstOrDefault().ItemName;
            return cName;
        }

        #region 标准制度管理
        /// <summary>
        /// 获取标准制度所有类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getStdSysType([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dyy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dyy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }         
            var where = string.Format(" and CreateUserOrgCode='{0}'", curUser.OrganizeCode);            
            var data =  new StdsysTypeBLL().GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
            };
            var result = new { code = 0, info = "获取数据成功", count = data.Count, data = data };

            return JObject.Parse(JsonConvert.SerializeObject(result, Formatting.None, settings));
        }
        /// <summary>
        /// 获取标准制度一级类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getStdSysTypeByOne([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dyy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dyy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            var where = string.Format(" and CreateUserOrgCode='{0}' and parentid='-1'", curUser.OrganizeCode);
            var data = new StdsysTypeBLL().GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
            };
            var result = new { code = 0, info = "获取数据成功", count = data.Count, data = data };

            return JObject.Parse(JsonConvert.SerializeObject(result, Formatting.None, settings));
        }
        /// <summary>
        /// 根据角色获取标准制度类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getStdSysTypeByRole([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dyy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dyy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            var where = string.Format(" and CreateUserOrgCode='{0}'", curUser.OrganizeCode);
            if (!(curUser.IsSystem || curUser.RoleName.Contains("公司管理员") || curUser.RoleName.Contains("厂级部门用户")))
            {
                where += string.Format(" and Scope like '{0}%'", curUser.DeptCode);
            }
            var data = new StdsysTypeBLL().GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
            };
            var result = new { code = 0, info = "获取数据成功", count = data.Count, data = data };
            var resultData = JsonConvert.SerializeObject(result, Formatting.None, settings);
            //resultData = resultData.ToUpperProperties();//属性名改为大写字母，值不变。  

            return JObject.Parse(resultData);
        }
        /// <summary>
        /// 获取标准制度
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getStdSysList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dyy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dyy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            //分页获取数据
            var dy = dyy.data;
            Pagination pagination = new Pagination();
            pagination.page = dy.page!=null ? (int)dy.page: 1;
            pagination.rows = dy.rows != null ? (int)dy.rows : 10;           
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            var deptcode = "";
            if (res.Contains("owner")&& !string.IsNullOrWhiteSpace(dy.owner.ToString()))
                deptcode = curUser.DeptCode;
            var query = new { filename = dy.filename ?? "", type = dy.type ?? "", deptcode = deptcode };
            var queryJson = JsonConvert.SerializeObject(query);
            var data = new StdsysFilesBLL().GetList(pagination, queryJson);
            var JsonData = new
            {
                code = 0,
                info = "获取数据成功",
                count = pagination.records,
                data = data
            };
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
            };
            var resultData = JsonConvert.SerializeObject(JsonData, Formatting.None, settings);
            //resultData = resultData.ToMatchEntity<StdsysFilesEntity>();//小写转换为指定类型的大小写形式。

            return JObject.Parse(resultData);
        }
        /// <summary>
        /// 获取标准制度详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getStdSysDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string id = dy.data ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }

                var entity = new StdsysFilesBLL().GetEntity(id);
                if (entity != null)
                {
                    //附件
                    DataTable file = fileInfoBLL.GetFiles(entity.ID);
                    var pC1 = new List<Photo>();
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                        pC1.Add(p);
                    }
                    entity.Files = pC1;
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
                    };
                    var data = JsonConvert.SerializeObject(entity, Formatting.None, settings);                                      
                    //data = data.ToLowerProperties();//属性名改为小写字母，值不变。  

                    return new
                    {
                        code = 0,
                        info = "获取数据成功",
                        count = 1,
                        data = JObject.Parse(data)
                    };
                }
                else
                {
                    return new { code = -1, count = 0, info = "获取失败，记录不存在。", data = new object() };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "获取失败，错误：" + ex.Message, data = new object() };
            }
        }
        /// <summary>
        /// 新增标准制度
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddStdSys()
        {
            try
            {
                string res = HttpContext.Current.Request["json"]; //json.Value<string>("json");
                Submit<StdsysFilesEntity> dy = JsonConvert.DeserializeObject<Submit<StdsysFilesEntity>>(res, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss" });
                string userid = dy.userId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                var entity = dy.data;
                entity.ID = Guid.NewGuid().ToString();
                var stdsysfilesbll = new StdsysFilesBLL();
                if (!stdsysfilesbll.ExistSame(curUser.OrganizeCode, entity.FileName, entity.ID))
                {                    
                    HttpFileCollection files = hcontent.Request.Files;//附件 
                    UploadifyFile(entity.ID, "", files);//上传附件
                    stdsysfilesbll.SaveForm("", entity);
                }
                else
                {
                    return new { code = -1, count = 0, info = "保存失败，存在同名文件，请校正。", data = new object() };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败，错误：" + ex.Message, data = new object() };
            }
            return new { code = 0, count = 0, info = "保存成功", data = new object() };
        }
        /// <summary>
        /// 编辑标准制度
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object EditStdSys()
        {
            try
            {
                string res = HttpContext.Current.Request["json"]; //json.Value<string>("json");
                Submit<StdsysFilesEntity> dy = JsonConvert.DeserializeObject<Submit<StdsysFilesEntity>>(res, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss" });
                string userid = dy.userId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                var entity = dy.data;
                if (entity != null && !string.IsNullOrWhiteSpace(entity.ID))
                {
                    var stdsysfilesbll = new StdsysFilesBLL();
                    if (!stdsysfilesbll.ExistSame(curUser.OrganizeCode, entity.FileName, entity.ID))
                    {
                        //获取删除附件ID
                        string deleteFileId = entity.DeleteFileId;
                        if (!string.IsNullOrEmpty(deleteFileId))
                        {
                            DeleteFile(deleteFileId);
                        }
                        HttpFileCollection files = hcontent.Request.Files;//附件
                        UploadifyFile(entity.ID, "", files);//上传附件
                        stdsysfilesbll.SaveForm(entity.ID, entity);
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "保存失败，存在同名文件，请校正。", data = new object() };
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = "保存失败，记录不存在。", data = new object() };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败，错误：" + ex.Message, data = new object() };
            }
            return new { code = 0, count = 0, info = "保存成功", data = new object() };
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object RemoveStdSys([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string id = dy.data ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                new StdsysFilesBLL().RemoveForm(id);
                DeleteFileByRec(id);//删除附件
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "删除失败，错误：" + ex.Message, data = new object() };
            }
            return new { code = 0, count = 0, info = "删除成功", data = new object() };
        }
        #endregion

        #region 上传附件、删除附件
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string folderId, string foldername, HttpFileCollection fileList)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    for(var i=0;i<fileList.Count;i++)
                    {
                        HttpPostedFile file = fileList[i];
                        long filesize = file.ContentLength;
                        if (filesize > 0)
                        {
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();

                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string uploadDate1 = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                            string virtualPath = string.Format("~/Resource/StdSysFiles/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string virtualPath1 = string.Format("/Resource/StdSysFiles/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
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
                                fileInfoEntity.FolderId = "StdSysFiles";
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
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(fileInfoIds))
            {
                string ids = "";

                string[] strArray = fileInfoIds.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                    var entity = fileInfoBLL.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = dataitemdetailbll.GetItemValue("imgPath") + entity.FilePath.Replace("~", "");
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
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
        public void DeleteFileByRec(string recId)
        {
            if (!string.IsNullOrWhiteSpace(recId))
            {
                var list = fileInfoBLL.GetFileList(recId);
                foreach (var file in list)
                {
                    fileInfoBLL.RemoveForm(file.FileId);
                    var filePath = dataitemdetailbll.GetItemValue("imgPath") + file.FilePath.Replace("~", "");
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }
        #endregion

        #region 获取各法规分类的数量
        /// <summary>
        /// 获取各法规分类的数量
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLawTypeList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;

                //获取标题
                string filename = dy.data.filename;
                //获取省份
                string province = dy.data.province;
                //获取开始时间
                string stime = dy.data.stime;
                //获取结束时间
                string etime = dy.data.etime;
                //数据来源,电厂和省公司
                string orgcode = dy.data.organizecode;
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string strwhere = "1=1";
                if (!string.IsNullOrWhiteSpace(filename)) {
                    strwhere += string.Format(" and FileName  like '%{0}%'", filename);
                }
                if (!string.IsNullOrWhiteSpace(province))
                {
                    strwhere += string.Format(" and province = '{0}'", province);
                }
                //时间范围
                if (!string.IsNullOrWhiteSpace(stime) || !string.IsNullOrWhiteSpace(etime))
                {
                    if (string.IsNullOrEmpty(stime))
                    {
                        stime = "1899-01-01";
                    }
                    if (string.IsNullOrEmpty(etime))
                    {
                        etime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    strwhere += string.Format(" and ReleaseDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", stime, etime);
                }

                if (curUser.RoleName.Contains("省级用户"))
                {
                    strwhere += string.Format(" and ( createuserorgcode = '{0}' or createuserorgcode='00')", curUser.OrganizeCode);
                }
                else
                {
                    if (string.IsNullOrEmpty(orgcode))
                    {
                        var provdata = departmentBLL.GetList().Where(t => curUser.NewDeptCode.Contains(t.DeptCode) && t.Nature == "省级");
                        //DepartmentEntity provEntity = null;
                        strwhere += " and (";
                        //省级根节点
                        if (provdata.Count() > 0)
                        {
                            foreach (DepartmentEntity item in provdata)
                            {
                                strwhere += string.Format(" createuserorgcode='{0}' or ", item.EnCode);
                            }
                            //provEntity = provdata.FirstOrDefault();
                            //strwhere += string.Format(" createuserorgcode='{0}' or ", provEntity.EnCode);
                        }
                        strwhere += string.Format(" createuserorgcode = '{0}' or createuserorgcode='00')", curUser.OrganizeCode);
                    }
                    else
                    {
                        strwhere += string.Format(" and ( createuserorgcode = '{0}')", orgcode);
                    }

                }

                string sql = string.Format(@"select law.itemcode,law.itemname,case when num is null then 0 else num end as num  from( select  d.itemcode ,d.itemname from  base_dataitemdetail d  left join base_dataitem i on i.itemid = d.itemid
   where  d.enabledmark = 1 and d.deletemark = 0  and i.itemcode in ('LawType') and d.itemcode!='0'
order by d.sortcode asc) law left join (select lawtypecode,count(id) as num from bis_safetylaw t where {0} group by t.lawtypecode) t 
on law.itemcode=t.lawtypecode ", strwhere);
                DataTable dt = pbll.GetPerilEngineeringList(sql);
                return new
                {
                    Code = 0,
                    Count = dt.Rows.Count,
                    Info = "获取数据成功",
                    data = dt
                };
               
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 根据查询条件获取数据列表By法规类别
        /// <summary>
        /// 根据查询条件获取数据列表By法规类别
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLawListByLawType([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;

                //获取标题
                string filename = dy.data.filename;
                //第二层标题，联合查询
                string filenametwo = dy.data.filenametwo;
                //获取效力级别
                string lawtypecode = dy.data.lawtypecode;
                //获取省份
                string province = dy.data.province;
                //获取开始时间
                string stime = dy.data.stime;
                //获取结束时间
                string etime = dy.data.etime;
                //数据来源,电厂和省公司
                string orgcode = dy.data.organizecode;
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string strwhere = "1=1";
                
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = @"ReleaseDate as Cdate,FileName,FileCode,to_char(ReleaseDate,'yyyy-MM-dd') as ReleaseDate,to_char(CarryDate,'yyyy-MM-dd') as CarryDate,IssueDept,case when Effetstate='1' then '现行有效'
                                            when Effetstate ='2' then '即将实施'
                                            when Effetstate='3'  then '已修订'
                                            when Effetstate='4'  then '废止' end Effetstate,LawType,Province,MainContent,filesid,lawsource";
                pagination.p_tablename = " bis_safetylaw";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "Cdate desc,FileName";//排序字段 
                pagination.sord = "desc";//排序方式
                //pagination.conditionJson = string.Format(" CreateUserOrgCode='{0}'", curUser.OrganizeCode);
                pagination.conditionJson = "1=1 ";
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", filename);
                }
                if (!string.IsNullOrWhiteSpace(filenametwo))
                {
                    pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", filenametwo);
                }
                if (!string.IsNullOrWhiteSpace(province))
                {
                    pagination.conditionJson += string.Format(" and province = '{0}'", province);
                }
                if (!string.IsNullOrWhiteSpace(lawtypecode))
                {
                    pagination.conditionJson += string.Format(" and lawtypecode = '{0}'", lawtypecode);
                }
                if (curUser.RoleName.Contains("省级用户"))
                {
                    pagination.conditionJson += string.Format(" and ( createuserorgcode = '{0}' or createuserorgcode='00')", curUser.OrganizeCode);
                }
                else {
                    if (string.IsNullOrEmpty(orgcode))
                    {
                        var provdata = departmentBLL.GetList().Where(t => curUser.NewDeptCode.Contains(t.DeptCode) && t.Nature == "省级");
                        //DepartmentEntity provEntity = null;
                        pagination.conditionJson += " and (";
                        //省级根节点
                        if (provdata.Count() > 0)
                        {
                            foreach (DepartmentEntity item in provdata)
                            {
                                pagination.conditionJson += string.Format(" createuserorgcode='{0}' or ", item.EnCode);
                            }
                            //provEntity = provdata.FirstOrDefault();
                            //pagination.conditionJson += string.Format(" createuserorgcode='{0}' or ", provEntity.EnCode);
                        }
                        pagination.conditionJson += string.Format(" createuserorgcode = '{0}' or createuserorgcode='00')", curUser.OrganizeCode);
                    }
                    else {
                        pagination.conditionJson += string.Format(" and ( createuserorgcode = '{0}')", orgcode);
                    }
                    
                }
                //时间范围
                if (!string.IsNullOrWhiteSpace(stime) || !string.IsNullOrWhiteSpace(etime))
                {
                    if (string.IsNullOrEmpty(stime))
                    {
                        stime = "1899-01-01";
                    }
                    if (string.IsNullOrEmpty(etime))
                    {
                        etime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and ReleaseDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", stime, etime);
                }
                DataTable dt = htbaseinfobll.GetBaseInfoForApp(pagination);
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic entity = new ExpandoObject();
                    entity.filename = dr["FileName"].ToString();//标题
                    entity.filecode = dr["FileCode"].ToString();//文号
                    entity.createdate = dr["ReleaseDate"].ToString();//发布日期
                    entity.carrydate = dr["CarryDate"].ToString();//实施日期
                    entity.issuedept = dr["IssueDept"].ToString();//发布机关
                    entity.lawtype = dr["LawType"].ToString();//效力级别
                    entity.province = dr["Province"].ToString();//发布区域
                    entity.effetstate = dr["Effetstate"].ToString();//时效性
                    entity.maincontent = dr["MainContent"].ToString();//内容大纲
                    //在线预览文档
                    if (string.IsNullOrWhiteSpace(dr["filesid"].ToString()))
                    {
                        entity.previewurl = "";
                    }
                    else {
                        string state = dr["lawsource"].ToString();//数据来源,0本地,1同步
                        if (state == "1") {
                            string sql = string.Format("select count(1) from BASE_FILEINFO t where t.recid='{0}'", dr["filesid"].ToString());
                            DataTable dtRec = seb.SelectData(sql);
                            if (dtRec.Rows[0][0].ToString() != "0") {
                                state = "0";//虽然是同步的数据，但是内网环境，所有读取本地文件
                            }
                        }
                        string path = dataitemdetailbll.GetItemValue("imgUrl");
                        entity.previewurl = string.Format(@"{0}/Content/SecurityDynamics/Preview.html?keyValue={1}&type={2}", path, dr["filesid"].ToString(), state);
                        //entity.previewurl = string.Format(dataitemdetailbll.GetItemValue("PreviewUrl", "Resource"), hcontent.Request.Url.Host, Config.GetValue("SoftName"), dr["filesid"].ToString(), state);
                    }
                    IList<Photo> rList = new List<Photo>();
                    //附件
                    DataTable file = fileInfoBLL.GetFiles(dr["ID"].ToString());
                    foreach (DataRow drs in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = drs["fileid"].ToString();
                        p.filename = drs["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                        rList.Add(p);
                    }
                    entity.file = rList;
                    data.Add(entity);
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取事故案例列表
        /// <summary>
        /// 获取事故案例列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCaseList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;

                //获取标题
                string filename = dy.data.filename;
                //获取事故等级
                string accidentgrade = dy.data.accidentgrade;
                //获取开始时间
                string stime = dy.data.stime;
                //获取结束时间
                string etime = dy.data.etime;
                //数据来源,电厂和省公司
                string orgcode = dy.data.organizecode;
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string strwhere = "1=1";

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = @"FileName,AccRange,AccTime,AccidentCompany,RelatedCompany,intDeaths,AccType,relatedjob,province,case when AccidentGrade='1' then '一般事故'
                                            when AccidentGrade ='2' then '较大事故'
                                            when AccidentGrade='3'  then '重大事故'
                                            when AccidentGrade='4'  then '特别重大事故' end AccidentGrade,casefid,casesource,filesid";
                pagination.p_tablename = " bis_accidentCaseLaw";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate desc,FileName";//排序字段 
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", filename);
                }
                if (!string.IsNullOrWhiteSpace(accidentgrade))
                {
                    pagination.conditionJson += string.Format(" and AccidentGrade = '{0}'", accidentgrade);
                }
                //时间范围
                if (!string.IsNullOrWhiteSpace(stime) || !string.IsNullOrWhiteSpace(etime))
                {
                    if (string.IsNullOrEmpty(stime))
                    {
                        stime = "1899-01-01";
                    }
                    if (string.IsNullOrEmpty(etime))
                    {
                        etime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and acctime between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", stime, etime);
                }

                if (curUser.RoleName.Contains("省级用户"))
                {
                    pagination.conditionJson += string.Format(" and ( createuserorgcode = '{0}' or createuserorgcode='00')", curUser.OrganizeCode);
                }
                else
                {
                    if (string.IsNullOrEmpty(orgcode))
                    {
                        var provdata = departmentBLL.GetList().Where(t => curUser.NewDeptCode.Contains(t.DeptCode) && t.Nature == "省级");
                        //DepartmentEntity provEntity = null;
                        pagination.conditionJson += " and (";
                        //省级根节点
                        if (provdata.Count() > 0)
                        {
                            foreach (DepartmentEntity item in provdata)
                            {
                                pagination.conditionJson += string.Format(" createuserorgcode='{0}' or ", item.EnCode);
                            }
                            //provEntity = provdata.FirstOrDefault();
                            //pagination.conditionJson += string.Format(" createuserorgcode='{0}' or ", provEntity.EnCode);
                        }
                        pagination.conditionJson += string.Format(" createuserorgcode = '{0}' or createuserorgcode='00')", curUser.OrganizeCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ( createuserorgcode = '{0}')", orgcode);
                    }

                }
                DataTable dt = htbaseinfobll.GetBaseInfoForApp(pagination);
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic entity = new ExpandoObject();
                    entity.filename = dr["FileName"].ToString();//事故名称
                    entity.accidentcompany = dr["accidentcompany"].ToString();//事故单位
                    entity.acctime = dr["acctime"].ToString();//事故时间
                    entity.accidentgrade = dr["accidentgrade"].ToString();//事故等级
                    entity.acctype = dr["acctype"].ToString();//事故类别
                    entity.relatedcompany = dr["relatedcompany"].ToString();//涉事单位
                    entity.relatedjob = dr["relatedjob"].ToString();//涉事工种
                    entity.intdeaths = dr["intdeaths"].ToString();//死亡人数
                    entity.province = dr["province"].ToString();//所属辖区
                    //在线预览文档
                    string fid = dr["casefid"].ToString();
                    if (string.IsNullOrWhiteSpace(fid)) {
                        fid = dr["filesid"].ToString();
                    }
                    if (string.IsNullOrWhiteSpace(fid))
                    {
                        entity.previewurl = "";
                    }
                    else {
                        string state = dr["casesource"].ToString();//数据来源,0本地,1同步
                        if (state == "1")
                        {
                            string sql = string.Format("select count(1) from BASE_FILEINFO t where t.recid='{0}'", fid);
                            DataTable dtRec = seb.SelectData(sql);
                            if (dtRec.Rows[0][0].ToString() != "0")
                            {
                                state = "0";//虽然是同步的数据，但是内网环境，所有读取本地文件
                            }
                        }
                        string path = dataitemdetailbll.GetItemValue("imgUrl");
                        entity.previewurl = string.Format(@"{0}/Content/SecurityDynamics/Preview.html?keyValue={1}&type={2}", path, fid, state);
                        //entity.previewurl = string.Format(dataitemdetailbll.GetItemValue("PreviewUrl", "Resource"), hcontent.Request.Url.Host, Config.GetValue("SoftName"), fid, state);
                    }
                    
                    IList<Photo> rList = new List<Photo>();
                    //附件
                    DataTable file = fileInfoBLL.GetFiles(dr["ID"].ToString());
                    foreach (DataRow drs in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = drs["fileid"].ToString();
                        p.filename = drs["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                        rList.Add(p);
                    }
                    entity.file = rList;
                    data.Add(entity);
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        [HttpGet]
        public object GetProvinceData()
        {
            try
            {
                var data = areaBLL.GetAreaList("0");
                List<object> dataList = new List<object>();
                dynamic entity = new ExpandoObject();
                entity.areaid = "-1";
                entity.areacode = "00";
                entity.areaname = "全国";
                dataList.Add(entity);
                foreach (var item in data)
                {
                    dynamic dy = new ExpandoObject();
                    dy.areaid = item.AreaId;
                    dy.areacode = item.AreaCode;
                    dy.areaname = item.AreaName;
                    dataList.Add(dy);
                }
                return new { code = 0, info = "获取数据成功", count = dataList.Count, data = dataList };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }


        /// <summary>
        /// 获取分类节点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetTypeTreeJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //0代表规章制度,1代表操作规程
                string datatype = dy.data.datatype;
                //机构CODE
                string orgcode = dy.data.organizecode;
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var treeList = new List<object>();
                string code = curUser.OrganizeCode;
                if (!string.IsNullOrWhiteSpace(orgcode))
                {
                    code = orgcode;
                }
                var where = string.Format(" and CreateUserOrgCode='{0}' and datatype='{1}'", code, datatype);
                var data = safeinstitutiontreebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
                foreach (var item in data)
                {
                    bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                    dynamic tree = new ExpandoObject();
                    tree.id = item.ID;
                    tree.text = item.TreeName;
                    tree.value = item.TreeCode;
                    tree.parentid = item.ParentId;
                    tree.haschildren = hasChild;

                    treeList.Add(tree);
                }
                return new { code = 0, info = "获取数据成功", count = treeList.Count, data = treeList };
            }
            catch (Exception ex)
            {
               return new { Code = -1, Count = 0, Info = ex.Message };
            }
            
        }

        #region 获取规章制度列表
        /// <summary>
        /// 获取规章制度列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInstitutionList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;

                //获取文件名称
                string filename = dy.data.filename;
                //获取文件类型
                string filetype = dy.data.filetype;
                //获取开始时间
                string stime = dy.data.stime;
                //获取结束时间
                string etime = dy.data.etime;
                //数据来源,电厂和省公司
                string orgcode = dy.data.organizecode;
                //是否包含下级节点,0否1是
                string iscontain = dy.data.iscontain;
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string strwhere = "1=1";

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = @"CreateDate,FileName,IssueDept,FileCode,to_char(CarryDate,'yyyy-MM-dd') as CarryDate,FilesId,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate ,lawtypename,Remark";
                pagination.p_tablename = " bis_safeinstitution";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "releasedate desc,FileName";//排序字段 
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", filename);
                }
                if (!string.IsNullOrWhiteSpace(filetype))
                {
                    if (!string.IsNullOrWhiteSpace(iscontain) && iscontain == "1")
                    {
                        pagination.conditionJson += string.Format(" and LawTypeCode like '{0}%'", filetype);
                    }
                    else {
                        pagination.conditionJson += string.Format(" and LawTypeCode = '{0}'", filetype);
                    }
                }
                //时间范围
                if (!string.IsNullOrWhiteSpace(stime) || !string.IsNullOrWhiteSpace(etime))
                {
                    if (string.IsNullOrEmpty(stime))
                    {
                        stime = "1899-01-01";
                    }
                    if (string.IsNullOrEmpty(etime))
                    {
                        etime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and releasedate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", stime, etime);
                }
                if(string.IsNullOrWhiteSpace(orgcode)){
                    orgcode = curUser.OrganizeCode;
                }
                pagination.conditionJson += string.Format(" and createuserorgcode = '{0}' ", orgcode);

                
                DataTable dt = htbaseinfobll.GetBaseInfoForApp(pagination);
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic entity = new ExpandoObject();
                    entity.filename = dr["filename"].ToString();//文件名称
                    entity.filecode = dr["filecode"].ToString();//文件编号
                    entity.releasedate = dr["releasedate"].ToString();//发布日期
                    entity.revisedate = dr["revisedate"].ToString();//修订日期
                    entity.carrydate = dr["carrydate"].ToString();//实施日期
                    entity.issuedept = dr["issuedept"].ToString();//发布单位/部门
                    entity.remark = dr["Remark"].ToString();//备注
                    //在线预览文档
                    string fid = dr["FilesId"].ToString();
                    if (string.IsNullOrWhiteSpace(fid))
                    {
                        entity.previewurl = "";
                    }
                    else {
                        string path = dataitemdetailbll.GetItemValue("imgUrl");
                        entity.previewurl = string.Format(@"{0}/Content/SecurityDynamics/Preview.html?keyValue={1}&type={2}", path, fid, "0");
                        //entity.previewurl = string.Format(dataitemdetailbll.GetItemValue("PreviewUrl", "Resource"), hcontent.Request.Url.Host, Config.GetValue("SoftName"), fid, "0");
                    }
                    IList<Photo> rList = new List<Photo>();
                    //附件
                    DataTable file = fileInfoBLL.GetFiles(dr["ID"].ToString());
                    foreach (DataRow drs in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = drs["fileid"].ToString();
                        p.filename = drs["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                        rList.Add(p);
                    }
                    entity.file = rList;
                    data.Add(entity);
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion


        #region 获取操作规程列表
        /// <summary>
        /// 获取操作规程列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetStandardsList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;

                //获取文件名称
                string filename = dy.data.filename;
                //获取文件类型
                string filetype = dy.data.filetype;
                //获取开始时间
                string stime = dy.data.stime;
                //获取结束时间
                string etime = dy.data.etime;
                //数据来源,电厂和省公司
                string orgcode = dy.data.organizecode;
                //是否包含下级节点,0否1是
                string iscontain = dy.data.iscontain;
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string strwhere = "1=1";

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = @"CreateDate,FileName,IssueDept,FileCode,to_char(CarryDate,'yyyy-MM-dd') as CarryDate,FilesId,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate ,lawtypename,Remark";
                pagination.p_tablename = " bis_safeStandards";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "releasedate desc,FileName";//排序字段 
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", filename);
                }
                if (!string.IsNullOrWhiteSpace(filetype))
                {
                    if (!string.IsNullOrWhiteSpace(iscontain) && iscontain == "1")
                    {
                        pagination.conditionJson += string.Format(" and LawTypeCode like '{0}%'", filetype);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and LawTypeCode = '{0}'", filetype);
                    }
                }
                //时间范围
                if (!string.IsNullOrWhiteSpace(stime) || !string.IsNullOrWhiteSpace(etime))
                {
                    if (string.IsNullOrEmpty(stime))
                    {
                        stime = "1899-01-01";
                    }
                    if (string.IsNullOrEmpty(etime))
                    {
                        etime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and releasedate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", stime, etime);
                }
                if (string.IsNullOrWhiteSpace(orgcode))
                {
                    orgcode = curUser.OrganizeCode;
                }
                pagination.conditionJson += string.Format(" and createuserorgcode = '{0}' ", orgcode);


                DataTable dt = htbaseinfobll.GetBaseInfoForApp(pagination);
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic entity = new ExpandoObject();
                    entity.filename = dr["filename"].ToString();//文件名称
                    entity.filecode = dr["filecode"].ToString();//文件编号
                    entity.releasedate = dr["releasedate"].ToString();//发布日期
                    entity.revisedate = dr["revisedate"].ToString();//修订日期
                    entity.carrydate = dr["carrydate"].ToString();//实施日期
                    entity.issuedept = dr["issuedept"].ToString();//发布单位/部门
                    entity.remark = dr["Remark"].ToString();//备注
                    //在线预览文档
                    string fid = dr["FilesId"].ToString();
                    if (string.IsNullOrWhiteSpace(fid))
                    {
                        entity.previewurl = "";
                    }
                    else
                    {
                        string path = dataitemdetailbll.GetItemValue("imgUrl");
                        entity.previewurl = string.Format(@"{0}/Content/SecurityDynamics/Preview.html?keyValue={1}&type={2}", path, fid, "0");
                        //entity.previewurl = string.Format(dataitemdetailbll.GetItemValue("PreviewUrl", "Resource"), hcontent.Request.Url.Host, Config.GetValue("SoftName"), fid, "0");
                    }
                    IList<Photo> rList = new List<Photo>();
                    //附件
                    DataTable file = fileInfoBLL.GetFiles(dr["ID"].ToString());
                    foreach (DataRow drs in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = drs["fileid"].ToString();
                        p.filename = drs["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                        rList.Add(p);
                    }
                    entity.file = rList;
                    data.Add(entity);
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion


        #region 同步资源平台(法规和事故案例)

        /// <summary>
        /// 每星期同步法规数据
        /// </summary>
        [HttpGet]
        public object GetLawData()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                //获取要同步的数据类别
                string LawType = di.GetItemValue("LawType", "Resource");
                string Circulate = di.GetItemValue("Circulate", "Resource");
                string PageSize = di.GetItemValue("PageSize", "Resource");
                if (!string.IsNullOrWhiteSpace(LawType))
                {
                    string[] LawTypeStr = LawType.Split(',');
                    for (int j = 0; j < LawTypeStr.Length; j++)
                    {
                        for (int i = 0; i < int.Parse(Circulate); i++)
                        {
                            string str = string.Format(di.GetItemValue("LawServiceUrl", "Resource"), LawTypeStr[j], i, int.Parse(PageSize));
                            var readData = webClient.DownloadString(new Uri(str));
                            //string result = Encoding.GetEncoding("UTF-8").GetString(readData);
                            string result = readData;
                            dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                            if (dy.code == "000")
                            {
                                int count = 0;
                                string ResultJson = JsonConvert.SerializeObject(dy.result);
                                JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                                if (jarray.Count > 0)
                                {
                                    //删除历史数据,LawSource为1的
                                    if (j == 0)
                                    {
                                        string sql = "delete bis_safetylaw t where t.LawSource='1'";
                                        seb.UpdateData(sql);
                                    }
                                    foreach (JObject rhInfo in jarray)
                                    {
                                        try
                                        {
                                            SafetyLawEntity entity = new SafetyLawEntity();
                                            entity.CreateUserOrgCode = "00";
                                            entity.Id = rhInfo["Id"].ToString();//法规数据ID
                                            entity.LawSource = "1";//1标识从法规库同步过来
                                            entity.EffetState = rhInfo["effectState"].ToString();//时效性
                                            entity.CarryDate = Convert.ToDateTime(rhInfo["effectiveTime"].ToString());//实施日期
                                            string IssueDeptCode = rhInfo["lawDept"].ToString();
                                            entity.IssueDeptCode = IssueDeptCode;//发布机构CODE

                                            if (IssueDeptCode.Contains(','))
                                            {
                                                string IssueDept = string.Empty;
                                                foreach (string item in IssueDeptCode.Split(','))
                                                {
                                                    string value = GetDataItemByValue("LawDept", item);
                                                    if (!string.IsNullOrEmpty(value))
                                                    {
                                                        IssueDept += value + ",";
                                                    }
                                                }
                                                entity.IssueDept = IssueDept.Substring(0, IssueDept.Length - 1);
                                            }
                                            else
                                            {

                                                entity.IssueDept = GetDataItemByValue("LawDept", IssueDeptCode);//发布机构
                                            }

                                            entity.FilesId = rhInfo["Id"].ToString();//文件库的文件ID
                                            entity.FileName = rhInfo["lawName"].ToString();//标题
                                            entity.FileCode = rhInfo["lawNo"].ToString();//文号
                                            entity.Province = rhInfo["province"].ToString();//发布地区
                                            entity.ReleaseDate = Convert.ToDateTime(rhInfo["releaseTime"].ToString());//发布日期
                                            entity.LawType = rhInfo["var_label_name"].ToString();//效力级别(法规分类)
                                            entity.LawTypeCode = di.GetItemValue(rhInfo["var_label_name"].ToString(), "LawType");
                                            string mainCntent = rhInfo["mainCntent"].ToString();
                                            if (!string.IsNullOrEmpty(mainCntent)&&mainCntent.Length > 500)
                                            {
                                                mainCntent = mainCntent.Substring(0, 500);
                                            }
                                            entity.MainContent = mainCntent;//内容大纲
                                            string copyContent = rhInfo["copyContent"].ToString();
                                            if (!string.IsNullOrEmpty(copyContent) && copyContent.Length > 500)
                                            {
                                                copyContent = copyContent.Substring(0, 500);
                                            }
                                            entity.CopyContent = copyContent;//正文快照
                                            entity.UpdateDate = DateTime.Now;
                                            safetylawbll.SaveForm(rhInfo["Id"].ToString(), entity);
                                        }
                                        catch (Exception e)
                                        {
                                            count++;
                                            exStr += e.Message + "\r\n";
                                            //将同步结果写入日志文件
                                            string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                            WLogger("zyptLaw", conten);
                                        }

                                    }
                                    //写日志
                                    //将同步结果写入日志文件
                                    exStr += string.Format("第{0}次,同步成功{1}条数据,失败{2}条!", j, jarray.Count - count, count) +"\r\n";
                                    string conten1 = string.Format("第{0}次,同步成功{1}条数据,失败{2}条!", j, jarray.Count - count, count);
                                    WLogger("zyptLaw", conten1);
                                    if (jarray.Count < int.Parse(PageSize))
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    exStr += "无同步数据!" + "\r\n";
                                    //将同步结果写入日志文件
                                    WLogger("zyptLaw", "无同步数据!");
                                    break;
                                }
                            }
                            else
                            {
                                exStr += result + "\r\n";
                                //将同步结果写入日志文件
                                WLogger("zyptLaw", result);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                WLogger("zyptLaw", e.Message);
            }
            return exStr;
        }

        /// <summary>
        /// 每星期同步发布机构数据,在同步法规之前
        /// </summary>
        [HttpGet]
        public object GetLawDeptData()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                string str = di.GetItemValue("GetLawDeptUrl", "Resource");
                DataItemEntity die = dataItemBLL.GetEntityByCode("LawDept");
                //var readData = webClient.UploadData(str, "POST", Encoding.GetEncoding("UTF-8").GetBytes(""));
                string result = webClient.DownloadString(new Uri(str));
                dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                if (dy.code == "000")
                {
                    int count = 0;
                    string ResultJson = JsonConvert.SerializeObject(dy.result);
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                    if (jarray.Count > 0)
                    { //删除历史数据,LawSource为1的
                        string sql = string.Format("delete base_dataitemdetail t where t.itemid='{0}'", die.ItemId);
                        seb.UpdateData(sql);
                        foreach (JObject rhInfo in jarray)
                        {
                            try
                            {
                                DataItemDetailEntity entity = new DataItemDetailEntity();
                                entity.CreateUserId = "System";
                                entity.CreateUserName = "超级管理员";
                                entity.ItemValue = rhInfo["intId"].ToString();//机构ID
                                entity.ItemName = rhInfo["varLabelName"].ToString();//机构名称
                                entity.ItemId = die.ItemId;
                                entity.ParentId = "0";
                                entity.SortCode = 1;
                                entity.EnabledMark = 1;
                                di.SaveForm("", entity);
                            }
                            catch (Exception e)
                            {
                                count++;
                                exStr += string.Format("保存该条数据失败!{0};", e.Message) + "\r\n";
                                //将同步结果写入日志文件
                                string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                WLogger("fbjg", conten);
                            }

                        }
                    }
                    exStr += string.Format("同步{0}条数据成功!", jarray.Count) + "\r\n";
                    //将同步结果写入日志文件
                    string conten1 = string.Format("同步{0}条数据成功!", jarray.Count);
                    WLogger("fbjg", conten1);
                }
                else
                {
                    exStr += result + "\r\n";
                    //将同步结果写入日志文件
                    WLogger("fbjg", result);
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                //将同步结果写入日志文件
                WLogger("fbjg", e.Message);
            }
            return exStr;
        }

        /// <summary>
        /// 获取法规库附件表信息
        /// </summary>
        [HttpGet]
        public object GetAllExAttachment()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                //最多循环次数
                string Circulate = di.GetItemValue("Circulate", "Resource");
                string PageSize = di.GetItemValue("PageSize", "Resource");
                for (int i = 0; i < int.Parse(Circulate); i++)
                {
                    //get请求,获取地址
                    string str = string.Format(di.GetItemValue("GetAllExAttachment", "Resource"), i, int.Parse(PageSize));
                    var readData = webClient.DownloadString(new Uri(str));
                    string result = readData;
                    dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    if (dy.code == "000")
                    {
                        int count = 0;
                        string ResultJson = JsonConvert.SerializeObject(dy.result);
                        JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                        if (jarray.Count > 0)
                        {
                            string sql = "delete ex_attachment t";
                            if (i == 0) {
                                //删除历史数据
                                seb.UpdateData(sql);
                            }
                            foreach (JObject rhInfo in jarray)
                            {
                                try
                                {
                                    string id = rhInfo["int_id"].ToString();
                                    //string var_code = rhInfo["Id"].ToString();//附件编号
                                    string var_name = rhInfo["var_name"].ToString();//附件名称
                                    var_name = Regex.Replace(var_name,@"\s"," ");
                                    string var_fid = rhInfo["var_fid"].ToString();//文件ID
                                    string var_local_name = rhInfo["var_local_name"].ToString();//附件原始名称
                                    var_local_name = Regex.Replace(var_local_name, @"\s", " ");
                                    string var_ext = rhInfo["var_ext"].ToString();//文件后缀
                                    sql = string.Format(@"insert into ex_attachment (ID, CREATEDATE, var_name, var_fid, var_local_name, var_ext)
values ('{0}', to_date('{1}','yyyy-mm-dd hh24:mi:ss'), '{2}','{3}', '{4}', '{5}')", id, DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm"), var_name, var_fid, var_local_name, var_ext);
                                    seb.UpdateData(sql);
                                }
                                catch (Exception e)
                                {
                                    count++;
                                    exStr += string.Format("保存该条数据失败!{0};", e.Message) + "\r\n";
                                    //将同步结果写入日志文件
                                    string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                    WLogger("ExAttachment", conten);
                                }

                            }
                            exStr += string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count, count) + "\r\n";
                            //写日志
                            //将同步结果写入日志文件
                            string conten1 = string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count, count);
                            WLogger("ExAttachment", conten1);
                            if (jarray.Count < int.Parse(PageSize))
                            {
                                break;
                            }
                        }
                        else
                        {
                            exStr += "无同步数据!" + "\r\n";
                            //将同步结果写入日志文件
                            WLogger("ExAttachment", "无同步数据!");
                            break;
                        }
                    }
                    else
                    {
                        exStr += result + "\r\n";
                        //将同步结果写入日志文件
                        WLogger("ExAttachment", result);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                WLogger("ExAttachment", e.Message);
            }
            return exStr;
        }

        /// <summary>
        /// 法规库所有附件关联表
        /// </summary>
        [HttpGet]
        public object GetAllExLawAttachment()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                //最多循环次数
                string Circulate = di.GetItemValue("Circulate", "Resource");
                string PageSize = di.GetItemValue("PageSize", "Resource");
                for (int i = 0; i < int.Parse(Circulate); i++)
                {
                    //get请求,获取地址
                    string str = string.Format(di.GetItemValue("GetAllExLawAttachment", "Resource"), i, int.Parse(PageSize));
                    var readData = webClient.DownloadString(new Uri(str));
                    string result = readData;
                    dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    if (dy.code == "000")
                    {
                        int count = 0;
                        string ResultJson = JsonConvert.SerializeObject(dy.result);
                        JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                        if (jarray.Count > 0)
                        {
                            string sql = "delete ex_law_attachment t";
                            if (i == 0) {
                                //删除历史数据
                                seb.UpdateData(sql);
                            }

                            foreach (JObject rhInfo in jarray)
                            {
                                try
                                {
                                    string id = rhInfo["id"].ToString();
                                    string law_id = rhInfo["law_id"].ToString();//案例id
                                    string attachment_id = rhInfo["attachment_id"].ToString();//附件id
                                    string law_type = rhInfo["law_type"].ToString();//法规下的附件类型:"00其他"、"01 法规正文"、"02 法规条款"、"03 法规附件"、"04 法规释义"、“05法规解读”
                                    string ext = rhInfo["ext"].ToString();//文件后缀
                                    string valid = rhInfo["valid"].ToString();//是否有效：1：有效；0：无效
                                    sql = string.Format(@"insert into ex_law_attachment (ID, CREATEDATE, law_id, attachment_id, law_type, ext,valid)
values ('{0}', to_date('{1}','yyyy-mm-dd hh24:mi:ss'), '{2}','{3}', '{4}', '{5}','{6}')", id, DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm"), law_id, attachment_id, law_type, ext, valid);
                                    seb.UpdateData(sql);
                                }
                                catch (Exception e)
                                {
                                    count++;
                                    exStr += string.Format("保存该条数据失败!{0};", e.Message) + "\r\n";
                                    //将同步结果写入日志文件
                                    string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                    WLogger("LawAttachment", conten);
                                }

                            }
                            exStr += string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count, count) + "\r\n";
                            //写日志
                            //将同步结果写入日志文件
                            string conten1 = string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count, count);
                            WLogger("LawAttachment", conten1);
                            if (jarray.Count < int.Parse(PageSize))
                            {
                                break;
                            }
                        }
                        else
                        {
                            exStr += "无同步数据!" + "\r\n";
                            //将同步结果写入日志文件
                            WLogger("LawAttachment", "无同步数据!");
                            break;
                        }
                    }
                    else
                    {
                        exStr += result + "\r\n";
                        //将同步结果写入日志文件
                        WLogger("LawAttachment", result);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                WLogger("LawAttachment", e.Message);
            }
            return exStr;
        }

        /// <summary>
        /// 法规库详情表，按条导入
        /// </summary>
        [HttpGet]
        public object GetAllLawExcelDetail()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                //最多循环次数
                string Circulate = di.GetItemValue("Circulate", "Resource");
                string PageSize = di.GetItemValue("PageSize", "Resource");
                for (int i = 0; i < int.Parse(Circulate); i++)
                {
                    //get请求,获取地址
                    string str = string.Format(di.GetItemValue("GetAllLawExcelDetail", "Resource"), i, int.Parse(PageSize));
                    var readData = webClient.DownloadString(new Uri(str));
                    string result = readData;
                    dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    if (dy.code == "000")
                    {
                        int count = 0;
                        string ResultJson = JsonConvert.SerializeObject(dy.result);
                        JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                        if (jarray.Count > 0)
                        {
                            string sql = "delete law_excel_detail t";
                            if (i == 0) {
                                //删除历史数据
                                seb.UpdateData(sql);
                            }
                            foreach (JObject rhInfo in jarray)
                            {
                                try
                                {
                                    string id = rhInfo["id"].ToString();
                                    string law_id = rhInfo["law_id"].ToString();//案例id
                                    string pno = rhInfo["pno"].ToString();//上级条款编号
                                    string detail_no = rhInfo["detail_no"].ToString();//条款编号
                                    string detail_content = "";
                                    if (rhInfo["detail_content"] != null)
                                    {
                                        detail_content = rhInfo["detail_content"].ToString();//条款内容
                                        detail_content = Regex.Replace(detail_content, @"\s", " ");
                                    }
                                    string level_type = "";
                                    if (rhInfo["level_type"] != null)
                                    {
                                        level_type = rhInfo["level_type"].ToString();//节点类型(0、标题，1、章，2、节、3条)
                                    }
                                    string level_order = "";
                                    if (rhInfo["level_order"] != null)
                                    {
                                        level_order = rhInfo["level_order"].ToString();//条款排序
                                    }
                                    string detail_name = "";
                                    if (rhInfo["detail_name"] != null)
                                    {
                                        detail_name = rhInfo["detail_name"].ToString();//条款名称
                                    }

                                    sql = string.Format(@"insert into law_excel_detail (ID, CREATEDATE, law_id, pno, detail_no, detail_content,level_type,level_order,detail_name)
values ('{0}', to_date('{1}','yyyy-mm-dd hh24:mi:ss'), '{2}','{3}', '{4}', '{5}','{6}','{7}','{8}')", id, DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm"), law_id, pno, detail_no, detail_content, level_type, level_order, detail_name);
                                    seb.UpdateData(sql);
                                }
                                catch (Exception e)
                                {
                                    count++;
                                    exStr += string.Format("保存该条数据失败!{0};", e.Message) + "\r\n";
                                    //将同步结果写入日志文件
                                    string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                    WLogger("ExcelDetail", conten);
                                }

                            }
                            exStr += string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count, count) + "\r\n";
                            //写日志
                            //将同步结果写入日志文件
                            string conten1 = string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count, count);
                            WLogger("ExcelDetail", conten1);
                            if (jarray.Count < int.Parse(PageSize))
                            {
                                break;
                            }
                        }
                        else
                        {
                            exStr += "无同步数据!" + "\r\n";
                            //将同步结果写入日志文件
                            WLogger("ExcelDetail", "无同步数据!");
                            break;
                        }
                    }
                    else
                    {
                        exStr += result + "\r\n";
                        //将同步结果写入日志文件
                        WLogger("ExcelDetail", result);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                WLogger("ExcelDetail", e.Message);
            }
            return exStr;
        }

        private void WLogger(string type, string content)
        {
            //将同步结果写入日志文件
            string fileName = type + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
            }
            object obj = new object();
            lock (obj)
            {
                using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/Logs/" + fileName), FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    byte[] bytData = Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + content + "\r\n");
                    fs.Write(bytData, 0, bytData.Length);
                    fs.Flush();
                    fs.Dispose();
                }
            }
        }

        private string GetDataItemByValue(string itemcode, string itemvalue)
        {
            try
            {
                string sql = string.Format("select itemname from  base_dataitemdetail  where itemid =(select  itemid from base_dataitem where itemcode='{0}') and itemvalue='{1}'", itemcode, itemvalue);
                DataTable dt = seb.SelectData(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }


        /// <summary>
        /// 每星期同步涉事单位数据,在同步事故案例之前
        /// </summary>
        [HttpGet]
        public object GetCaseRelatedCompany()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                string str = di.GetItemValue("GetCaseRelatedCompany", "Resource");
                DataItemEntity die = dataItemBLL.GetEntityByCode("RelatedCompany");
                //var readData = webClient.UploadData(str, "POST", Encoding.GetEncoding("UTF-8").GetBytes(""));
                string result = webClient.DownloadString(new Uri(str));
                dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                if (dy.code == "000")
                {
                    int count = 0;
                    string ResultJson = JsonConvert.SerializeObject(dy.result);
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                    if (jarray.Count > 0)
                    { //删除历史数据,LawSource为1的
                        string sql = string.Format("delete base_dataitemdetail t where t.itemid='{0}'", die.ItemId);
                        seb.UpdateData(sql);
                        foreach (JObject rhInfo in jarray)
                        {
                            try
                            {
                                DataItemDetailEntity entity = new DataItemDetailEntity();
                                entity.CreateUserId = "System";
                                entity.CreateUserName = "超级管理员";
                                entity.ItemValue = rhInfo["intid"].ToString();//机构ID
                                entity.ItemName = rhInfo["varname"].ToString();//机构名称
                                entity.ItemId = die.ItemId;
                                entity.ParentId = "0";
                                entity.SortCode = 1;
                                entity.EnabledMark = 1;
                                di.SaveForm("", entity);
                            }
                            catch (Exception e)
                            {
                                count++;
                                exStr += string.Format("保存该条数据失败!{0};", e.Message) + "\r\n";
                                //将同步结果写入日志文件
                                string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                WLogger("ssdw", conten);
                            }

                        }
                    }
                    exStr += string.Format("同步{0}条数据成功!", jarray.Count) + "\r\n";
                    //将同步结果写入日志文件
                    string conten1 = string.Format("同步{0}条数据成功!", jarray.Count);
                    WLogger("ssdw", conten1);
                }
                else
                {
                    exStr += result + "\r\n";
                    //将同步结果写入日志文件
                    WLogger("ssdw", result);
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                //将同步结果写入日志文件
                WLogger("ssdw", e.Message);
            }
            return exStr;
        }


        /// <summary>
        /// 每星期同步事故类别,在同步事故案例之前
        /// </summary>
        [HttpGet]
        public object GetAccType()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                string str = di.GetItemValue("GetAccType", "Resource");
                DataItemEntity die = dataItemBLL.GetEntityByCode("AccType");
                //var readData = webClient.UploadData(str, "POST", Encoding.GetEncoding("UTF-8").GetBytes(""));
                string result = webClient.DownloadString(new Uri(str));
                dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                if (dy.code == "000")
                {
                    int count = 0;
                    string ResultJson = JsonConvert.SerializeObject(dy.result);
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                    if (jarray.Count > 0)
                    { //删除历史数据,LawSource为1的
                        string sql = string.Format("delete base_dataitemdetail t where t.itemid='{0}'", die.ItemId);
                        seb.UpdateData(sql);
                        foreach (JObject rhInfo in jarray)
                        {
                            try
                            {
                                if (rhInfo["intid"].ToString() == "1") continue;
                                DataItemDetailEntity entity = new DataItemDetailEntity();
                                entity.CreateUserId = "System";
                                entity.CreateUserName = "超级管理员";
                                entity.ItemValue = rhInfo["intid"].ToString();//机构ID
                                entity.ItemName = rhInfo["varname"].ToString();//机构名称
                                entity.ItemId = die.ItemId;
                                entity.ParentId = "0";
                                entity.SortCode = int.Parse(rhInfo["varvalue"].ToString());
                                entity.EnabledMark = 1;
                                di.SaveForm("", entity);
                            }
                            catch (Exception e)
                            {
                                count++;
                                exStr += string.Format("保存该条数据失败!{0};", e.Message) + "\r\n";
                                //将同步结果写入日志文件
                                string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                WLogger("sglb", conten);
                            }

                        }
                    }
                    exStr += string.Format("同步{0}条数据成功!", jarray.Count) + "\r\n";
                    //将同步结果写入日志文件
                    string conten1 = string.Format("同步{0}条数据成功!", jarray.Count);
                    WLogger("sglb", conten1);
                }
                else
                {
                    exStr += result + "\r\n";
                    //将同步结果写入日志文件
                    WLogger("sglb", result);
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                //将同步结果写入日志文件
                WLogger("sglb", e.Message);
            }
            return exStr;
        }

        /// <summary>
        /// 每星期同步事故案例数据
        /// </summary>
        [HttpGet]
        public object GetCaseData()
        {
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.Headers.Add("Content-Type", "text/json; charset=utf-8");
            webClient.Encoding = Encoding.UTF8;
            string exStr = string.Empty;
            try
            {
                //获取最多同步次数
                string Circulate = di.GetItemValue("Circulate", "Resource");
                string PageSize = di.GetItemValue("PageSize", "Resource");
                for (int i = 0; i < int.Parse(Circulate); i++)
                {
                    string str = string.Format(di.GetItemValue("GetCaseData", "Resource"), i, int.Parse(PageSize));
                    var readData = webClient.DownloadString(new Uri(str));
                    //string result = Encoding.GetEncoding("UTF-8").GetString(readData);
                    string result = readData;
                    dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    if (dy.code == "000")
                    {
                        int count = 0;
                        int continueNum = 0;
                        string ResultJson = JsonConvert.SerializeObject(dy.result);
                        JArray jarray = (JArray)JsonConvert.DeserializeObject(ResultJson);
                        if (jarray.Count > 0)
                        {
                            if (i == 0) {
                                string sql = "delete bis_accidentCaseLaw t where t.CaseSource='1'";
                                seb.UpdateData(sql);
                            }

                            foreach (JObject rhInfo in jarray)
                            {
                                try
                                {
                                    AccidentCaseLawEntity entity = new AccidentCaseLawEntity();
                                    if (accidentcaselawbll.GetEntity(rhInfo["intId"].ToString()) != null) {
                                        continueNum++;
                                        continue;
                                    } 
                                    entity.CreateUserOrgCode = "00";
                                    entity.Id = rhInfo["intId"].ToString();//法规数据ID
                                    entity.CaseSource = "1";//1标识从法规库同步过来
                                    entity.AccRange = "3";//事故范围
                                    entity.AccTime = Convert.ToDateTime(rhInfo["datProduceDate"].ToString());//事故时间
                                    string RelatedCompany = rhInfo["varMatterCompanyIds"].ToString();//涉事单位ID
                                    if (RelatedCompany.Contains(','))
                                    {
                                        string IssueDept = string.Empty;
                                        foreach (string item in RelatedCompany.Split(','))
                                        {
                                            string value = GetDataItemByValue("RelatedCompany", item);
                                            if (!string.IsNullOrEmpty(value))
                                            {
                                                IssueDept += value + ",";
                                            }
                                        }
                                        entity.RelatedCompany = IssueDept.Substring(0, IssueDept.Length - 1);
                                    }
                                    else
                                    {

                                        entity.RelatedCompany = GetDataItemByValue("RelatedCompany", RelatedCompany);//涉事单位名称
                                    }
                                    entity.CaseFid = rhInfo["varFid"].ToString();//fid,带后缀,文件库的文件ID
                                    entity.FileName = rhInfo["varName"].ToString();//标题
                                    entity.AccidentCompany = GetDataItemByValue("RelatedCompany", rhInfo["varAccidentCompanyId"].ToString());//事故单位
                                    entity.Province = rhInfo["provinceName"].ToString();//所属辖区
                                    entity.RelatedEquipment = rhInfo["varRelatedEquipment"].ToString();//涉事设备
                                    entity.RelatedJob = rhInfo["varRelatedJob"].ToString();//涉事工种
                                    string AccidentGrade = rhInfo["chrAccidentGrade"].ToString();
                                    switch (AccidentGrade)
                                    {
                                        case "一般事故":
                                            entity.AccidentGrade = "1";//事故等级
                                            break;
                                        case "较大事故":
                                            entity.AccidentGrade = "2";//事故等级
                                            break;
                                        case "重大事故":
                                            entity.AccidentGrade = "3";//事故等级
                                            break;
                                        case "特别重大事故":
                                            entity.AccidentGrade = "4";//事故等级
                                            break;
                                        default:
                                            entity.AccidentGrade = "1";//事故等级
                                            break;
                                    }

                                    entity.intDeaths = rhInfo["intDeaths"].ToString(); ;//死亡人数
                                    string AccTypeCode = rhInfo["chrType"].ToString();//事故类别ID
                                    entity.AccTypeCode = AccTypeCode;

                                    if (AccTypeCode.Contains(','))
                                    {
                                        string AccType = string.Empty;
                                        foreach (string item in AccTypeCode.Split(','))
                                        {
                                            string value = GetDataItemByValue("AccType", item);
                                            if (!string.IsNullOrEmpty(value))
                                            {
                                                AccType += value + ",";
                                            }
                                        }
                                        entity.AccType = AccType.Substring(0, AccType.Length - 1);
                                    }
                                    else
                                    {

                                        entity.AccType = GetDataItemByValue("AccType", AccTypeCode);//事故类别
                                    }
                                    accidentcaselawbll.SaveForm(rhInfo["intId"].ToString(), entity);
                                }
                                catch (Exception e)
                                {
                                    count++;
                                    exStr += e.Message + "\r\n";
                                    //将同步结果写入日志文件
                                    string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, JsonConvert.SerializeObject(rhInfo));
                                    WLogger("case", conten);
                                }

                            }
                            //写日志
                            //将同步结果写入日志文件
                            exStr += string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count - continueNum, count) + "\r\n";
                            string conten1 = string.Format("同步成功{0}条数据,失败{1}条!", jarray.Count - count - continueNum, count);
                            WLogger("case", conten1);
                            if (jarray.Count < int.Parse(PageSize))
                            {
                                break;
                            }
                        }
                        else
                        {
                            exStr += "无同步数据!" + "\r\n";
                            //将同步结果写入日志文件
                            WLogger("case", "无同步数据!");
                            break;
                        }
                    }
                    else
                    {
                        exStr += result + "\r\n";
                        //将同步结果写入日志文件
                        WLogger("case", result);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                exStr += e.Message + "\r\n";
                //写日志
                WLogger("case", e.Message);
            }
            return exStr;
        }

        #endregion

        /// <summary>
        /// 远程下载法规文件，最后执行该方法
        /// </summary>
        [HttpGet]
        public object DownloadFileByLaw()
        {
            string exStr = string.Empty;
            int xcount = 0;//失败条数
            int scount = 0;
            try
            {
                if (dataitemdetailbll.GetItemValue("flag")=="0")
                {
                    exStr = "外网环境,无需下载文件";
                    return exStr;
                }
                //查询法规所有的附件信息
                string sql = string.Format(@"select t.filesid,t.var_fid,t.var_local_name from (select t.filesid,f.var_fid,f.var_local_name from bis_safetylaw t left join (select e.law_id,t.var_fid,t.var_local_name from ex_attachment t left join ex_law_attachment e on t.id=e.attachment_id) f
on t.id=f.law_id where t.lawsource='1') t left join BASE_FILEINFO o on t.filesid=o.recid where o.recid is not null");
                DataTable dt = seb.SelectData(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            string filesid = dr["filesid"].ToString();
                            string filename = dr["var_local_name"].ToString();
                            string filepath = string.Format(@"{0}", dataitemdetailbll.GetItemValue("imgPath"));
                            FileInfoBLL fileBll = new FileInfoBLL();
                            FileInfoEntity entity = fileBll.GetEntity(filesid, filename);
                            int res = 0;
                            if (entity != null)
                            {
                                res = fileBll.DeleteFile(filesid, filename, filepath + entity.FilePath.Replace("~", ""));
                            }
                        }
                        catch (Exception ex)
                        {
                            scount++;
                            exStr += "删除:" + ex.Message + "\r\n";
                            //写日志
                            WLogger("LawFileDown", ex.Message);
                        }
                        
                    }
                    exStr += string.Format("删除成功{0}条数据,失败{1}条!", dt.Rows.Count - scount, scount) + "\r\n";
                    //写日志
                    //将同步结果写入日志文件
                    string conten1 = string.Format("删除成功{0}条数据,失败{1}条!", dt.Rows.Count - scount, scount);
                    WLogger("LawFileDown", conten1);
                    
                }

                sql = string.Format(@"select t.filesid,f.var_fid,f.var_local_name from bis_safetylaw t left join (select e.law_id,t.var_fid,t.var_local_name from ex_attachment t left join ex_law_attachment e on t.id=e.attachment_id) f
on t.id=f.law_id where t.lawsource='1' ");
                dt = seb.SelectData(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string fid = dr["var_fid"].ToString();
                        //if (fid != "a49b6a74121f4e37a815d5a01db7646d.doc") continue;
                        string filename = dr["var_local_name"].ToString();
                        var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
                        long expiresLong = DateTime.Now.Ticks / 1000 + 180;
                        string expires = expiresLong.ToString();
                        string token = BSFramework.Util.Md5Helper.MD5(di.GetItemValue("userKey", "Resource") + expires, 32);
                        string url = string.Format(di.GetItemValue("FileDowUrl", "Resource"), fid, di.GetItemValue("user", "Resource"), expires, token);
                        //调用接口下载文件
                        WebClient webClient = new WebClient();
                        webClient.Headers.Add(HttpRequestHeader.UserAgent, GetUserAgent());
                        string filepath = string.Format(@"{0}", dataitemdetailbll.GetItemValue("imgPath"));
                        string fileGuid = Guid.NewGuid().ToString();
                        string FileEextension = Path.GetExtension(filename);
                        string virtualPath = string.Format("~/Resource/LawSystem/{0}{1}", fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/LawSystem/{0}{1}", fileGuid, FileEextension);
                        string fullFileName = filepath + virtualPath1;
                        //创建文件夹
                        string path = Path.GetDirectoryName(fullFileName);
                        Directory.CreateDirectory(path);
                        
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(fullFileName))
                        {
                            try
                            {
                                //保存文件
                                webClient.DownloadFile(url, fullFileName);
                                //文件信息写入数据库
                                fileInfoEntity.CreateUserId = "System";
                                fileInfoEntity.CreateUserName = "超级管理员";
                                fileInfoEntity.ModifyUserId = "System";
                                fileInfoEntity.ModifyUserName = "超级管理员";
                                fileInfoEntity.Create();
                                fileInfoEntity.FileId = fileGuid;
                                fileInfoEntity.RecId = dr["filesid"].ToString(); //关联ID
                                fileInfoEntity.FolderId = "LawSystem";
                                fileInfoEntity.FileName = filename;
                                fileInfoEntity.FilePath = virtualPath;
                                fileInfoEntity.FileSize = "";
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileInfoBLL.SaveForm("", fileInfoEntity);
                            }
                            catch (Exception e)
                            {
                                xcount++;
                                //将同步结果写入日志文件
                                string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, dr["filesid"].ToString());
                                WLogger("LawFileDown", conten);
                            }
                            
                        }
                    }
                    exStr += string.Format("同步成功{0}条数据,失败{1}条!", dt.Rows.Count - xcount, xcount) + "\r\n";
                    //写日志
                    //将同步结果写入日志文件
                    string conten1 = string.Format("同步成功{0}条数据,失败{1}条!", dt.Rows.Count - xcount, xcount);
                    WLogger("LawFileDown", conten1);

                }

            }
            catch (Exception ex)
            {
                exStr += ex.Message + "\r\n";
                //写日志
                WLogger("LawFileDown", ex.Message);
            }
            
            return exStr;
        }

        /// <summary>
        /// 远程下载案例文件，最后执行该方法
        /// </summary>
        [HttpGet]
        public object DownloadFileByCase()
        {
            string exStr = string.Empty;
            int xcount = 0;//失败条数
            int scount = 0;
            try
            {
                if (dataitemdetailbll.GetItemValue("flag") == "0")
                {
                    exStr = "外网环境,无需下载文件";
                    return exStr;
                }
                //查询法规所有的附件信息
                string sql = string.Format(@"select t.casefid,f.filename from bis_accidentCaseLaw t left join BASE_FILEINFO f on t.casefid=f.recid where f.recid is not null");
                DataTable dt = seb.SelectData(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            string filesid = dr["casefid"].ToString();
                            string FileEextension = Path.GetExtension(filesid);
                            string filename = dr["filename"].ToString();
                            string filepath = string.Format(@"{0}", dataitemdetailbll.GetItemValue("imgPath"));
                            FileInfoBLL fileBll = new FileInfoBLL();
                            FileInfoEntity entity = fileBll.GetEntity(filesid, filename);
                            int res = 0;
                            if (entity != null)
                            {
                                res = fileBll.DeleteFile(filesid, filename, filepath + entity.FilePath.Replace("~", ""));
                            }
                        }
                        catch (Exception ex)
                        {
                            scount++;
                            exStr += "删除:" + ex.Message + "\r\n";
                            //写日志
                            WLogger("CaseFileDown", ex.Message);
                        }

                    }
                    exStr += string.Format("删除成功{0}条数据,失败{1}条!", dt.Rows.Count - scount, scount) + "\r\n";
                    //写日志
                    //将同步结果写入日志文件
                    string conten1 = string.Format("删除成功{0}条数据,失败{1}条!", dt.Rows.Count - scount, scount);
                    WLogger("CaseFileDown", conten1);

                }

                sql = string.Format(@"select t.casefid,t.filename from bis_accidentCaseLaw t where t.casesource='1'");
                dt = seb.SelectData(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string fid = dr["casefid"].ToString();
                        string FileEextension = Path.GetExtension(fid);
                        //if (fid != "a49b6a74121f4e37a815d5a01db7646d.doc") continue;
                        string filename = dr["filename"].ToString() + FileEextension;
                        var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
                        long expiresLong = DateTime.Now.Ticks / 1000 + 180;
                        string expires = expiresLong.ToString();
                        string token = BSFramework.Util.Md5Helper.MD5(di.GetItemValue("userKey", "Resource") + expires, 32);
                        string url = string.Format(di.GetItemValue("FileDowUrl", "Resource"), fid, di.GetItemValue("user", "Resource"), expires, token);
                        //调用接口下载文件
                        WebClient webClient = new WebClient();
                        webClient.Headers.Add(HttpRequestHeader.UserAgent, GetUserAgent());
                        string filepath = string.Format(@"{0}", dataitemdetailbll.GetItemValue("imgPath"));
                        string fileGuid = Guid.NewGuid().ToString();
                        string virtualPath = string.Format("~/Resource/CaseSystem/{0}{1}", fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/CaseSystem/{0}{1}", fileGuid, FileEextension);
                        string fullFileName = filepath + virtualPath1;
                        //创建文件夹
                        string path = Path.GetDirectoryName(fullFileName);
                        Directory.CreateDirectory(path);

                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(fullFileName))
                        {
                            try
                            {
                                //保存文件
                                webClient.DownloadFile(url, fullFileName);
                                //文件信息写入数据库
                                fileInfoEntity.CreateUserId = "System";
                                fileInfoEntity.CreateUserName = "超级管理员";
                                fileInfoEntity.ModifyUserId = "System";
                                fileInfoEntity.ModifyUserName = "超级管理员";
                                fileInfoEntity.Create();
                                fileInfoEntity.FileId = fileGuid;
                                fileInfoEntity.RecId = dr["casefid"].ToString(); //关联ID
                                fileInfoEntity.FolderId = "CaseSystem";
                                fileInfoEntity.FileName = filename;
                                fileInfoEntity.FilePath = virtualPath;
                                fileInfoEntity.FileSize = "";
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileInfoBLL.SaveForm("", fileInfoEntity);
                            }
                            catch (Exception e)
                            {
                                xcount++;
                                //将同步结果写入日志文件
                                string conten = string.Format("保存该条数据失败!{0}:{1}", e.Message, dr["casefid"].ToString());
                                WLogger("CaseFileDown", conten);
                            }

                        }
                    }
                    exStr += string.Format("同步成功{0}条数据,失败{1}条!", dt.Rows.Count - xcount, xcount) + "\r\n";
                    //写日志
                    //将同步结果写入日志文件
                    string conten1 = string.Format("同步成功{0}条数据,失败{1}条!", dt.Rows.Count - xcount, xcount);
                    WLogger("CaseFileDown", conten1);

                }

            }
            catch (Exception ex)
            {
                exStr += ex.Message + "\r\n";
                //写日志
                WLogger("CaseFileDown", ex.Message);
            }

            return exStr;
        }

        public static string GetUserAgent()
        {
            return string.Format("Mozilla/5.0 (Windows NT {0}; {1}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",
                    Environment.OSVersion.Version.ToString(2),
                    Environment.Is64BitProcess ? "WOW64" : "Win32");
        }
    }
}