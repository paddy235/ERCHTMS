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

namespace ERCHTMS.AppSerivce.Controllers
{
    public class SafetyLawRuleController : BaseApiController
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private PerilEngineeringBLL pbll = new PerilEngineeringBLL();
        private SafetyLawBLL safetylawbll = new SafetyLawBLL();
        private SafeInstitutionBLL safeinstitutionbll = new SafeInstitutionBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private SafeStandardsBLL safestandardsbll = new SafeStandardsBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
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
    }
}