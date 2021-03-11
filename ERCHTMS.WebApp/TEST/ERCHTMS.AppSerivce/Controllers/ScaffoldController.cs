using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Cache;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class ScaffoldController : BaseApiController
    {

        ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        ScaffoldspecBLL scaffoldspecbll = new ScaffoldspecBLL();
        ScaffoldprojectBLL scaffoldprojectbll = new ScaffoldprojectBLL();
        ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();
        HighProjectSetBLL highprojectsetbll = new HighProjectSetBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        /// <summary>
        /// 得到脚手架申请列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    pageindex = 1,
                    pagesize = 20,
                    data = new
                    {
                        ScaffoldType = string.Empty,
                        SetupType = string.Empty,
                        AuditState = string.Empty,
                        SetupCompanyId = string.Empty,
                        SetupCompanyCode = string.Empty,
                        SetupStartDate = string.Empty,
                        SetupEndDate = string.Empty,
                        ApplyCode = string.Empty,
                        ViewRange = string.Empty,
                        IsNoDismentle = string.Empty,
                        SearchType = string.Empty      //1或非1   1就是过滤
                    }
                });
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }

                var watch = CommonHelper.TimerStart();

                Pagination pagination = new Pagination();
                pagination.conditionJson = " 1=1 ";
                pagination.page = dy.pageindex;
                pagination.rows = dy.pagesize;
                pagination.sidx = "a.createdate";     //排序字段
                pagination.sord = "desc";             //排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ////查看范围数据权限
                ///**
                // * 1.作业单位及子部门（下级）
                // * 2.本人创建的高风险作业
                // * 3.发包部门管辖的外包单位
                // * 4.外包单位只能看本单位的
                // * */
                if (!user.IsSystem)
                {
                    if (curUser.RoleName.Contains("公司") || curUser.RoleName.Contains("厂级"))
                    {
                        pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and a.setupcompanyid in(select departmentid from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
                    }
                }
                //如果为待验收或待审核，才加此过滤条件
                if (!string.IsNullOrEmpty(dy.data.SearchType) && dy.data.SearchType == "1")
                {
                    string[] roles = user.RoleName.Split(',');
                    string roleWhere = "";
                    foreach (var r in roles)
                    {
                        roleWhere += string.Format("or flowrolename like '%{0}%'", r);
                    }
                    roleWhere = roleWhere.Substring(2);
                    if (user.RoleName.Contains("专工"))
                    {
                        string specialtytype = "''";
                        if (user.SpecialtyType != null)
                        {
                            specialtytype = "'" + user.SpecialtyType.Replace(",", "','") + "'";
                        }
                        pagination.conditionJson += string.Format("  and a.id in(select s.id from bis_scaffold s where s.flowremark='发包部门' and s.specialtytype in({2}) and s.AuditState in(1,4,6) and s.flowdeptid like '%{0}%' and ({1}) union all select s.id from bis_scaffold s where (s.flowremark!='发包部门' or s.flowremark is null) and s.AuditState in(1,4,6) and s.flowdeptid like '%{0}%' and ({1}))", user.DeptId, roleWhere, specialtytype);
                    }
                    else
                    {
                        //当前有审核权限的部门及角色，才可查看
                        pagination.conditionJson += string.Format(" and a.flowdeptid like '%{0}%' and ({1})", user.DeptId, roleWhere);
                    }
                }

                var list = scaffoldbll.GetList(pagination, JsonConvert.SerializeObject(dy.data));
                //list.Columns[0].ColumnName = "keyvalue";

                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                //Id 转换前的列名  keyvalue 转换后的列名
                //dict_props.Add("Id", "keyvalue");
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }

        /// <summary>
        /// 获取使用状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLedgerType()
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'LedgerType'");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { statevalue = x.ItemValue, statename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 得到脚手架台账列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLedgerList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    pageindex = 1,
                    pagesize = 20,
                    data = new
                    {
                        SetupCompanyId = string.Empty,
                        SetupCompanyCode = string.Empty,
                        DismentleCompanyId = string.Empty,
                        DismentleCompanyCode = string.Empty,
                        ActSetupStartDate = string.Empty,
                        ActSetupEndDate = string.Empty,
                        DismentleStartDate = string.Empty,
                        DismentleEndDate = string.Empty,
                        CheckStartDate = string.Empty,
                        CheckEndDate = string.Empty,
                        LedgerType = string.Empty,      //0-在用 1-已拆除
                        OutProjectName = string.Empty
                    }
                });
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }

                Pagination pagination = new Pagination();
                pagination.conditionJson = " 1=1 ";
                pagination.page = dy.pageindex;
                pagination.rows = dy.pagesize;
                pagination.sidx = "a.createdate desc";
                var list = scaffoldbll.GetLedgerList(pagination, JsonConvert.SerializeObject(dy.data), "app");
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                settings.Converters.Add(new DecimalToStringConverter());
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }

        }

        /// <summary>
        /// 跟据申请信息ID得到架体规格及形式列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetScaffSpecs([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                var data = scaffoldspecbll.GetList(dy.data.id);
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                //Id 转换前的列名  keyvalue 转换后的列名
                dict_props.Add("Id", "keyvalue");
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", data = JArray.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        /// <summary>
        /// 跟据申请信息ID得到审核记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetScaffAudits([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                var data = scaffoldauditrecordbll.GetList(dy.data.id);
                foreach (var item in data)
                {
                    item.AuditSignImg = string.IsNullOrWhiteSpace(item.AuditSignImg) ? "" : webUrl + item.AuditSignImg.ToString().Replace("../../", "/");
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", data = JArray.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        /// <summary>
        /// 删除申请信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object RemoveForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }

                ScaffoldEntity scaffoldEntity = scaffoldbll.GetEntity(dy.data.id);
                if (scaffoldEntity != null)
                {
                    if (scaffoldEntity.AuditState != 0)
                    {
                        throw new ArgumentException("审核状态为“申请中”才可以删除");
                    }
                }
                scaffoldbll.RemoveForm(dy.data.id);
                List<FileInfoEntity> list = fileInfoBLL.GetFileList(dy.data.id);
                var fileids = from a in list select a.FileId;
                if (fileids.Count() > 0)
                {
                    DeleteFile(string.Join(",", fileids.ToArray()));
                }
                return new { code = 0, info = "操作成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        /// <summary>
        /// 删除规格及形式
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object RemoveScaffoldSpec([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                scaffoldspecbll.RemoveForm(dy.data.id);

                return new { code = 0, info = "操作成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        /// <summary>
        /// 脚手架详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                var scaffoleEntity = scaffoldbll.GetEntity(dy.data.id);
                if (scaffoleEntity == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                if (scaffoleEntity.SpecialtyType != null)
                    scaffoleEntity.SpecialtyTypeName = getName(scaffoleEntity.SpecialtyType, "SpecialtyType");
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                string jsondata = JsonConvert.SerializeObject(scaffoleEntity);
                ScaffoldModel model = JsonConvert.DeserializeObject<ScaffoldModel>(jsondata);
                model.ScaffoldSpecs = scaffoldspecbll.GetList(scaffoleEntity.Id);
                model.ScaffoldProjects = scaffoldprojectbll.GetList(scaffoleEntity.Id);
                foreach (var item in model.ScaffoldProjects)
                {
                    item.SignPic = string.IsNullOrWhiteSpace(item.SignPic) ? "" : webUrl + item.SignPic.ToString().Replace("../../", "/");
                }
                model.ScaffoldAudits = scaffoldauditrecordbll.GetList(scaffoleEntity.Id);
                foreach (var item in model.ScaffoldAudits)
                {
                    item.AuditSignImg = string.IsNullOrWhiteSpace(item.AuditSignImg) ? "" : webUrl + item.AuditSignImg.ToString().Replace("../../", "/");
                }
                DataTable cdt = fileInfoBLL.GetFiles(model.Id);
                IList<Photo> cfiles = new List<Photo>();
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.fileid = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }
                model.cfiles = cfiles;

                DataTable cdt1 = fileInfoBLL.GetFiles(model.AcceptFileId);
                IList<Photo> acceptfiles = new List<Photo>();
                foreach (DataRow item in cdt1.Rows)
                {
                    Photo p = new Photo();
                    p.fileid = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    acceptfiles.Add(p);
                }
                model.acceptfiles = acceptfiles;
                model.RiskRecord = highriskrecordbll.GetList(model.Id).ToList();

                #region 流程
                string modulename = string.Empty;
                if (scaffoleEntity.ScaffoldType == 0)
                {
                    if (scaffoleEntity.SetupType == 1)
                    {
                        if (scaffoleEntity.SetupCompanyType == 0)
                        {
                            modulename = "(搭设申请-内部-6米以上)审核";
                        }
                        else
                        {
                            modulename = "(搭设申请-外包-6米以上)审核";
                        }
                    }
                    else
                    {
                        if (scaffoleEntity.SetupCompanyType == 0)
                        {
                            modulename = "(搭设申请-内部-6米以下)审核";
                        }
                        else
                        {
                            modulename = "(搭设申请-外包-6米以下)审核";
                        }
                    }
                }
                if (scaffoleEntity.ScaffoldType == 1)
                {
                    if (scaffoleEntity.SetupType == 0)
                    {
                        if (scaffoleEntity.SetupCompanyType == 0)
                        {
                            modulename = "(搭设验收-内部-6米以下)审核";
                        }
                        else
                        {
                            modulename = "(搭设验收-外包-6米以下)审核";
                        }
                    }
                    else
                    {
                        if (scaffoleEntity.SetupCompanyType == 0)
                        {
                            modulename = "(搭设验收-内部-6米以上)审核";
                        }
                        else
                        {
                            modulename = "(搭设验收-外包-6米以上)审核";
                        }
                    }
                }
                if (scaffoleEntity.ScaffoldType == 2)
                {
                    if (scaffoleEntity.SetupType == 1)
                    {
                        if (scaffoleEntity.SetupCompanyType == 0)
                        {
                            modulename = "(搭设拆除-内部-6米以上)审核";
                        }
                        else
                        {
                            modulename = "(搭设拆除-外包-6米以上)审核";
                        }
                    }
                    else
                    {
                        if (scaffoleEntity.SetupCompanyType == 0)
                        {
                            modulename = "(搭设拆除-内部-6米以下)审核";
                        }
                        else
                        {
                            modulename = "(搭设拆除-外包-6米以下)审核";
                        }
                    }
                }
                bool isendflow = false;
                if (scaffoleEntity.AuditState == 2 || scaffoleEntity.AuditState == 3 || scaffoleEntity.AuditState == 5)
                {
                    isendflow = true;
                }
                var nodelist = scaffoldbll.GetAppFlowList(scaffoleEntity.Id, modulename, scaffoleEntity.FlowId, isendflow, scaffoleEntity.SetupCompanyId, string.IsNullOrEmpty(scaffoleEntity.SpecialtyType) ? "" : scaffoleEntity.SpecialtyType);
                model.CheckFlow = nodelist;
                #endregion

                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                //Id 转换前的列名  keyvalue 转换后的列名
                //dict_props.Add("Id", "keyvalue");

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(model, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        /// <summary>
        /// 脚手架申请审核
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Audit()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty,
                        deletefileids = string.Empty,
                        checktype = 0,          //sx 20181130 modify 为区别是否是项目验收确认，1为项目验收确认 其他则为正常审批流程
                        auditentity = new ScaffoldauditrecordEntity(),
                        projects = new List<ScaffoldprojectEntity>()
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("审核出错，错误信息：data为null");
                }
                if (dy.data.auditentity == null)
                {
                    throw new ArgumentException("审核出错，错误信息：参数为null");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                ScaffoldEntity scaffoldEntity = scaffoldbll.GetEntity(dy.data.id);

                //审核通过的、验收不通过的、审核不通过的无法操作
                if (scaffoldEntity == null)
                {
                    throw new ArgumentException("审核出错，申请信息不存在");
                }
                //判断当前角色权限 角色一致且部门一样
                string[] curUserRoles = curUser.RoleName.ToString().Split(',');

                if (!scaffoldEntity.FlowDeptId.Contains(curUser.DeptId))
                {
                    throw new ArgumentException("无当前部门权限");
                }
                var isApprove = false;
                foreach (var r in curUserRoles)
                {
                    if (scaffoldEntity.FlowRoleName.Contains(r))
                    {
                        isApprove = true;
                        break;
                    }
                }
                if (!isApprove)
                {
                    throw new ArgumentException("无当前角色权限");
                }
                if (scaffoldEntity.AuditState == 2 || scaffoldEntity.AuditState == 3)
                {
                    throw new ArgumentException("此申请已处理");
                }
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                if (scaffoldEntity.AuditState == 4)
                {
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    var project = dy.data.projects;
                    if (project.Count > 0)
                    {
                        foreach (var item in project)
                        {
                            item.SignPic = string.IsNullOrWhiteSpace(item.SignPic) ? "" : item.SignPic.Replace(webUrl, "").ToString();
                            if (files.Count > 0)
                            {
                                for (int i = 0; i < files.AllKeys.Length; i++)
                                {
                                    HttpPostedFile file = files[i];
                                    string fileOverName = System.IO.Path.GetFileName(file.FileName);
                                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                                    string FileEextension = Path.GetExtension(file.FileName);
                                    if (fileName.Split('_')[0] == "sign")
                                    {
                                        if (fileName.Split('_')[1] == item.Id)
                                        {
                                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                            string newFileName = fileName.Split('_')[1] + FileEextension;
                                            string newFilePath = dir + "\\" + newFileName;
                                            file.SaveAs(newFilePath);
                                            item.SignPic = "/Resource/sign/" + fileOverName.Split('_')[1];
                                        }
                                        break;
                                    }
                                }
                            }
                            scaffoldprojectbll.SaveForm(item.Id, item);
                        }
                    }
                    //验收照片
                    string acceptFileId = scaffoldEntity.Id + "_acceptfileid";
                    scaffoldEntity.AcceptFileId = acceptFileId;
                    scaffoldbll.SaveForm(scaffoldEntity.Id, scaffoldEntity);
                    //如果有删除的文件，则进行删除
                    if (!string.IsNullOrEmpty(dy.data.deletefileids))
                    {
                        DeleteFile(dy.data.deletefileids);
                    }
                    //再重新上传
                    if (files.Count > 0)
                    {
                        UploadifyFile(acceptFileId, "scaffoldfile", files);
                    }
                }
                else
                {
                    dy.data.auditentity.AuditSignImg = string.IsNullOrWhiteSpace(dy.data.auditentity.AuditSignImg) ? "" : dy.data.auditentity.AuditSignImg.Replace(webUrl, "").ToString();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            if (fileName == dy.data.auditentity.Id)
                            {
                                string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                string newFileName = fileName + FileEextension;
                                string newFilePath = dir + "\\" + newFileName;
                                file.SaveAs(newFilePath);
                                dy.data.auditentity.AuditSignImg = "/Resource/sign/" + fileOverName;
                                break;
                            }
                        }
                    }
                }
                dy.data.auditentity.Id = null;
                scaffoldbll.ApplyCheck(dy.data.id, dy.data.auditentity, dy.data.projects, dy.data.checktype);

                return new { code = 0, data = "", info = "操作成功" };

            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }


        /// <summary>
        /// 验收项目保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveProjects()
        {

            try
            {
                string res = HttpContext.Current.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty,
                        deletefileids = string.Empty,
                        projects = new List<ScaffoldprojectEntity>()
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                ScaffoldEntity scaffoldEntity = scaffoldbll.GetEntity(dy.data.id);
                if (scaffoldEntity == null)
                {
                    throw new ArgumentException("缺少参数：脚手架信息不存在");
                }
                if (dy.data.projects == null)
                {
                    throw new ArgumentException("缺少参数：验收项目为空");
                }
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                HttpFileCollection files = HttpContext.Current.Request.Files;
                var project = dy.data.projects;
                if (project.Count > 0)
                {
                    foreach (var item in project)
                    {
                        item.SignPic = string.IsNullOrWhiteSpace(item.SignPic) ? "" : item.SignPic.Replace(webUrl, "").ToString();
                        if (files.Count > 0)
                        {
                            for (int i = 0; i < files.AllKeys.Length; i++)
                            {
                                HttpPostedFile file = files[i];
                                string fileOverName = System.IO.Path.GetFileName(file.FileName);
                                string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                                string FileEextension = Path.GetExtension(file.FileName);
                                if (fileName.Split('_')[0] == "sign")
                                {
                                    if (fileName.Split('_')[1] == item.Id)
                                    {
                                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                        string newFileName = fileName.Split('_')[1] + FileEextension;
                                        string newFilePath = dir + "\\" + newFileName;
                                        file.SaveAs(newFilePath);
                                        item.SignPic = "/Resource/sign/" + fileOverName.Split('_')[1];
                                    }
                                    break;
                                }
                            }
                        }
                        scaffoldprojectbll.SaveForm(item.Id, item);
                    }
                }
                //验收照片
                string acceptFileId = scaffoldEntity.Id + "_acceptfileid";
                scaffoldEntity.AcceptFileId = acceptFileId;
                scaffoldbll.SaveForm(scaffoldEntity.Id, scaffoldEntity);
                //如果有删除的文件，则进行删除
                if (!string.IsNullOrEmpty(dy.data.deletefileids))
                {
                    DeleteFile(dy.data.deletefileids);
                }
                //再重新上传
                if (files.Count > 0)
                {
                    UploadifyFile(acceptFileId, "scaffoldfile", files);
                }
                return new { code = 0, data = "", info = "处理成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }

        }
        /// <summary>
        /// 脚手架申请信息添加或修改
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new ScaffoldModel()
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }

                var model = dy.data; //JsonConvert.DeserializeObject<ScaffoldModel>(dy.data);
                //申请时间服务端生成
                model.ApplyDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                }
                //如果有删除的文件，则进行删除
                if (!string.IsNullOrEmpty(model.DeleteFileIds))
                {
                    DeleteFile(model.DeleteFileIds);
                }
                //再重新上传
                HttpFileCollection files = HttpContext.Current.Request.Files;
                UploadifyFile(model.Id, "scaffoldfile", files);

                highriskrecordbll.RemoveFormByWorkId(model.Id);
                if (model.RiskRecord != null)
                {
                    var num=0;
                    foreach (var item in model.RiskRecord)
                    {
                        item.CreateDate = DateTime.Now.AddSeconds(-num);
                        item.WorkId = model.Id;
                        highriskrecordbll.SaveForm("", item);
                        num++;
                    }
                }
                scaffoldbll.SaveForm(model.Id, model);

                return new { code = 0, data = "", info = "操作成功" };

            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        [HttpPost]
        public object LedgerOp()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        keyvalue = string.Empty,//作业id
                        ledgertype = string.Empty,//0:即将作业;即将搭设;即将变动 1:作业中;搭设中 4:即将拆除 5:拆除中
                        worktime = string.Empty,//时间
                        issendmessage = string.Empty,//是否发送短消息(0:否 1:是)
                        type = string.Empty//1:高风险作业 2:脚手架作业 3.安全设施变动
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                scaffoldbll.LedgerOp(dy.data.keyvalue, dy.data.ledgertype, dy.data.type, dy.data.worktime, dy.data.issendmessage);
                return new { code = 0, data = "", info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        #region 图片上传
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
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                        //创建文件夹
                        if (!fileName.Contains("sign"))
                        {
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

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
            FileInfoBLL fileInfoBLL = new FileInfoBLL();
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
                        var filePath = HttpContext.Current.Server.MapPath(entity.FilePath);
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
        #endregion
        /// <summary>
        /// 得到验收项目
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProjects([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                if (dy.data == null || string.IsNullOrEmpty(dy.data.id))
                {
                    IEnumerable<HighProjectSetEntity> projects = highprojectsetbll.GetList(" and typenum = -1 and createuserorgcode='" + curUser.OrganizeCode + "'");
                    //Id 转换前的列名  keyvalue 转换后的列名
                    dict_props.Add("MeasureName", "projectname");
                    dict_props.Add("MeasureResultOne", "resultyes");
                    dict_props.Add("MeasureResultTwo", "resultno");
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                        DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                        //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                    };
                    return new { code = 0, info = "获取数据成功", data = JArray.Parse(JsonConvert.SerializeObject(projects, Formatting.None, settings)) };
                }
                else
                {
                    //Id 转换前的列名  keyvalue 转换后的列名
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                        DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                        //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                    };
                    var data = scaffoldprojectbll.GetListByCondition(string.Format(" and ScaffoldId='{0}'", dy.data.id)).ToList();
                    foreach (var item in data)
                    {
                        item.SignPic = string.IsNullOrWhiteSpace(item.SignPic) ? "" : webUrl + item.SignPic.ToString().Replace("../../", "/");
                    }
                    return new { code = 0, info = "获取数据成功", data = JArray.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }

        #region 值转换
        public string getName(string type, string encode)
        {
            var cName = dataItemCache.GetDataItemList(encode).Where(a => a.ItemValue == type).FirstOrDefault().ItemName;
            return cName;
        }
        #endregion
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
    }

    public class DecimalToStringConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType.FullName == "System.Decimal";

        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null && value.GetType().FullName == "System.Decimal")
            {
                writer.WriteValue(Convert.ToInt64(value));
            }
        }
    }
}