using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Aspose.Cells;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.OccupationalHealthManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ThoughtWorks.QRCode.Codec;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    public class PersonController : MvcControllerBase
    {
        private PostCache postCache = new PostCache();
        private PostBLL postBLL = new PostBLL();
        private RoleBLL roleBLL = new RoleBLL();
        private UserBLL userBLL = new UserBLL();
        private UserCache userCache = new UserCache();
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        private ModuleFormInstanceBLL moduleFormInstanceBll = new ModuleFormInstanceBLL();
        private PermissionBLL permissionBLL = new PermissionBLL();
        private DataItemCache dic = new DataItemCache();
        private DataItemModel dm = new DataItemModel();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private TemporaryGroupsBLL tempbll = new TemporaryGroupsBLL();
        private HikinoutlogBLL hikInOut = new HikinoutlogBLL();

        string logPath = AppDomain.CurrentDomain.BaseDirectory + "logs";
        private LeaveApproveBLL leaveApproveBLL = new LeaveApproveBLL();
        #region 视图功能
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            ViewBag.IsAppointAccount = 0;   
            string accounts = new DataItemDetailBLL().GetItemValue("SpecialAccount", "HjbBasice")?.ToLower();
            if (!string.IsNullOrEmpty(accounts))
            {
                List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Operator user = OperatorProvider.Provider.Current();
                if (accountArray.Contains(user.Account.ToLower()))
                    ViewBag.IsAppointAccount = 1;
            }
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult EditRole()
        {
            return View();
        }
        /// <summary>
        ///特种作业和特种设备人员清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult List()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult AdUsers()
        {
            return View();
        }
        /// <summary>
        /// 用户表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Form()
        {
            bool isShow = false;
            if (!string.IsNullOrEmpty(new DataItemDetailBLL().GetItemValue("bzAppUrl")))
            {
                isShow = true;
            }
            ViewBag.isShow = isShow;
            return View();
        }
        /// <summary>
        /// 人员入场(职)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult LeaveForm()
        {
            return View();
        }
        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult RevisePassword()
        {
            return View();
        }
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 人员离场
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Leave()
        {
            return View();
        }
        /// <summary>
        /// 在厂外包单位人员清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult EnterRecord()
        {
            return View();
        }
        /// <summary>
        /// 外包人员统计（国电）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult HJBStat()
        {
            return View();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult LeaveEdit()
        {
            return View();
        }
        /// <summary>
        /// 人员离场
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Leavelist()
        {
            return View();
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImage()
        {
            return View();
        }
        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Stat()
        {
            return View();
        }

        /// <summary>
        /// 转岗
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Transfer()
        {
            return View();
        }

        /// <summary>
        /// 接触职业危害因素人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult UserHazardFactor()
        {
            return View();
        }

        /// <summary>
        /// 导入集团编号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportEncode()
        {
            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取受训角色(来自培训平台)
        /// </summary>
        /// <returns>返回机构+部门+用户树形Json</returns>
        [HttpGet]
        public ActionResult GetTrainRole()
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                DepartmentEntity org = departmentBLL.GetEntity(user.OrganizeId);
                if (org.IsTrain == 1)
                {
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
                    string url = new DataItemDetailBLL().GetItemValue("GetTrainRole");
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { company_id = org.InnerPhone });
                    string result = wc.UploadString(new Uri(url), json);
                    dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(result);
                    if (dy.meta.success)
                    {
                        List<object> data = dy.data;
                        return Success("获取数据成功", data);
                    }
                    else
                    {
                        return Error(dy.meta.message);
                    }
                }
                else
                {
                    return Error("没有获取到数据");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>返回机构+部门+用户树形Json</returns>
        [HttpGet]
        //[HandlerMonitor(3, "查询机构+部门+用户树形Json数据!")]
        public ActionResult GetTreeJson(string keyword)
        {
            var organizedata = organizeCache.GetList();
            var departmentdata = departmentCache.GetList();
            var userdata = userCache.GetList();
            var treeList = new List<TreeEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                #region 机构
                TreeEntity tree = new TreeEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                    //if (hasChildren == false)
                    //{
                    //    continue;
                    //}
                }
                tree.id = item.OrganizeId;
                tree.text = item.FullName;
                tree.value = item.OrganizeId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Organize";
                treeList.Add(tree);
                #endregion
            }
            foreach (DepartmentEntity item in departmentdata)
            {
                #region 部门
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Department";
                treeList.Add(tree);
                #endregion
            }
            foreach (UserEntity item in userdata)
            {
                #region 用户
                TreeEntity tree = new TreeEntity();
                tree.id = item.UserId;
                tree.text = item.RealName;
                tree.value = item.Account;
                tree.parentId = item.DepartmentId;
                tree.title = item.RealName + "（" + item.Account + "）";
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = false;
                tree.Attribute = "Sort";
                tree.AttributeValue = "User";
                tree.img = "fa fa-user";
                treeList.Add(tree);
                #endregion
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.TreeToJson());
        }
        /// <summary>
        /// 获取机构编码
        /// </summary>
        /// <param name="orgid">关键字</param>
        /// <returns>返回机构+部门+用户树形Json</returns>
        [HttpGet]
        public ActionResult GetOrganizeCode(string orgid)
        {
            var organizedata = organizeCache.GetEntity(orgid);
            return Content(organizedata.ToJson());
        }
        #region 获取机构部门组织树菜单
        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDepartTreeJson()
        {
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();
            List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
            List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
            if (user.IsSystem)
            {
                organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentCache.GetList().OrderBy(x => x.SortCode).ToList();
            }
            else
            {
                organizedata = organizeCache.GetList().Where(t => t.OrganizeId == user.OrganizeId).OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentCache.GetList(user.OrganizeId).OrderBy(x => x.SortCode).ToList();
            }
            var treeList = new List<TreeEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                #region 机构
                //if (existAuthorizeData.Count(t => t.ResourceId == item.OrganizeId) == 0) continue;
                TreeEntity tree = new TreeEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                }
                tree.id = item.OrganizeId;
                tree.text = item.FullName;
                tree.value = item.OrganizeId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Organize";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
                #endregion
            }
            foreach (DepartmentEntity item in departmentdata)
            {
                #region 部门
                //if (existAuthorizeData.Count(t => t.ResourceId == item.DepartmentId) == 0) continue;
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Department";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
                #endregion
            }
            return Content(treeList.TreeToJson());
        }
        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeptTreeJson()
        {
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();
            List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
            List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
            string roleNames = user.RoleName;
            if (user.IsSystem)
            {
                organizedata = organizeBLL.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(x => x.SortCode).ToList();
            }
            else
            {
                organizedata = organizeBLL.GetList().Where(t => t.OrganizeId == user.OrganizeId).OrderByDescending(x => x.CreateDate).ToList();

                if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).OrderBy(x => x.SortCode).ToList();
                }
                else if (roleNames.Contains("班组级用户"))
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.DepartmentId == user.DeptId).ToList();
                    departmentdata[0].ParentId = organizedata[0].OrganizeId;
                }
                else
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || t.SendDeptID == user.DeptId).OrderBy(x => x.SortCode).ToList();
                }
            }
            var treeList = new List<TreeEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                #region 机构
                //if (existAuthorizeData.Count(t => t.ResourceId == item.OrganizeId) == 0) continue;
                TreeEntity tree = new TreeEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                }
                tree.id = item.OrganizeId;
                tree.text = item.FullName;
                tree.value = item.OrganizeId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Organize";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
                #endregion
            }
            foreach (DepartmentEntity item in departmentdata)
            {
                #region 部门
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {


                    if (item.Nature == "部门")
                    {
                        int count = departmentdata.Count(t => t.ParentId == item.DepartmentId);
                        tree.parentId = count == 0 ? item.OrganizeId : item.ParentId;
                    }
                    else
                    {
                        tree.parentId = item.ParentId;
                    }
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Sort";
                tree.AttributeValue = (item.Nature == "分包商" || item.Nature == "承包商" || item.Description == "外包工程承包商") && !(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")) ? "Contract" : "Department";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
                #endregion
            }
            return Content(treeList.TreeToJson());
        }
        #endregion


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <returns>返回用户列表Json</returns>
        [HttpGet]
        //[HandlerMonitor(3, "根据部门查询用户列表!")]
        public ActionResult GetListJson(string departmentId)
        {
            var data = userCache.GetList(departmentId);
            return Content(data.ToJson());
        }
        /// <summary>
        ///获取培训记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回用户列表Json</returns>
        [HttpGet]
        public ActionResult GetTrainRecord(string userId)
        {
            string error = "";
            string result = userBLL.GetTrainRecord(userId, out error);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return Content(result);
            }
            else
            {
                return Error(error);
            }
            //var user = userBLL.GetEntity(userId);
            //string val = new DataItemDetailBLL().GetItemValue("TrainSyncWay");//对接方式，0：账号，1：身份证,不配置默认为账号
            //string way = new DataItemDetailBLL().GetItemValue("WhatWay");//对接平台 0：.net培训平台 1:java培训平台
            //if (way == "1")
            //{
            //    string account = user.Account;
            //    string fileName = "Train_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            //    try
            //    {
            //        WebClient wc = new WebClient();
            //        wc.Credentials = CredentialCache.DefaultCredentials;
            //        wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //        //发送请求到web api并获取返回值，默认为post方式
            //        string url = new DataItemDetailBLL().GetItemValue("TrainServiceUrl");
            //        string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { user_account = account });
            //        string result = wc.UploadString(new Uri(url), json);
            //        System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员培训档案,远程服务器返回信息：" + result + "\r\n");
            //        dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Dynamic.ExpandoObject>(result);
            //        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(dy.data));
            //    }
            //    catch (Exception ex)
            //    {
            //        System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员培训档案异常,信息：" + ex.Message + "\r\n");
            //        return Content(ex.ToString());
            //    }


            //}
            //else
            //{
            //    string idCard = "";
            //    string account = user.Account;

            //    if (!string.IsNullOrWhiteSpace(val))
            //    {
            //        if (val == "0")
            //        {
            //            idCard = "";
            //        }
            //        else
            //        {
            //            account = "";
            //            idCard = user.IdentifyID;
            //        }
            //    }
            //    var json = userBLL.GetTrainRecord(userId, account, "", idCard);
            //    if (json.Length > 1)
            //    {
            //        var dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Traindata\":", "").TrimEnd('}'));
            //        dt.Columns.Add("ISEXAM");
            //        dt.Columns.Add("LINE");
            //        dt.Columns.Add("SCORE");
            //        json = userBLL.GetExamRecord(userId, account, "", idCard);
            //        if (json.Length > 30)
            //        {
            //            var dtExams = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Examdata\":", "").TrimEnd('}'));
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                var rows = dtExams.Select("TRAINPLANCODE='" + dr["TRAINPRJID"].ToString() + "'");
            //                if (rows.Count() > 0)
            //                {
            //                    if (string.IsNullOrEmpty(rows[0]["POINT"].ToString()))
            //                    {
            //                        dr["ISEXAM"] = "否";
            //                    }
            //                    else
            //                    {
            //                        dr["ISEXAM"] = "是";
            //                        dr["SCORE"] = rows[0]["POINT"];
            //                    }
            //                    dr["LINE"] = rows[0]["PASSLINE"];
            //                }
            //                else
            //                {
            //                    dr["ISEXAM"] = "否";
            //                }
            //            }
            //        }
            //        var JsonData = new
            //        {
            //            rows = dt,
            //            total = 1,
            //            page = 1,
            //            records = dt.Rows.Count
            //        };
            //        return Content(JsonData.ToJson());
            //    }
            //    else
            //    {
            //        return Error("没有找到培训记录");
            //    }
            //}
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetHaardPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,identifyid,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,u.IsTransfer,u.DepartureReason,us";
            pagination.p_tablename = "v_userinfo u left join (select userid,username,LISTAGG(riskvalue,',') WITHIN GROUP (ORDER BY riskvalue) AS us from (  select userid,username,riskvalue from BIS_HAZARDFACTORUSER hauser left join BIS_HAZARDFACTORs ha on ha.hid=hauser.hid where 1=1 group by userid,username,riskvalue) f  group by userid,username) t on u.account=t.userid";
            pagination.conditionJson = "Account!='System' and us is not null";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1 and us is not null";
            }
            else
            {
                OccupationalstaffdetailBLL occupationalstaffdetailbll = new OccupationalstaffdetailBLL();
                //没有权限则显示空数据
                if (occupationalstaffdetailbll.IsPer())
                {
                    pagination.conditionJson += " and u.deptcode like '" + user.OrganizeCode + "%'";
                }
                else
                {
                    pagination.conditionJson += " and 1!=1";
                }

                //var queryParam = queryJson.ToJObject();
                //if (queryParam["datatype"].IsEmpty())
                //{
                //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                //    if (!string.IsNullOrEmpty(where) && (queryParam["code"].IsEmpty() || !queryJson.Contains("code")))
                //    {
                //        pagination.conditionJson += " and " + where;
                //    }
                //}

            }



            var data = new HazardfactoruserBLL().GetPageList(pagination, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                {
                    DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["DEPARTMENTCODE"], "部门", dr["organizeid"]));
                    if (dt.Rows.Count > 0)
                    {
                        string name = "";
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            name += dr1["fullname"].ToString() + "/";
                        }
                        dr["deptname"] = name.TrimEnd('/');
                    }
                }
            }
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),


            };
            return Content(JsonData.ToJson());
        }


        /// <summary>
        /// 用户列表(来自广西华昇预用户)
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]

        public ActionResult GetGXhsPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "Account,REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,CreateDate,identifyid,IsPresence,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,IsEpiboly";
            pagination.p_tablename = "v_userinfo u";
            pagination.conditionJson = "openid=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");

                if (string.IsNullOrWhiteSpace(where))
                {
                    pagination.conditionJson += "  and organizecode='" + user.OrganizeCode + "'";
                }
                if (user.IsSystem)
                {
                    pagination.conditionJson = "openid=1";
                }

                var data = userBLL.GetPageList(pagination, queryJson);
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetPageListJson(Pagination pagination, string queryJson, int mode = 0)
        {
            string softName = BSFramework.Util.Config.GetValue("SoftName").ToLower();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "Account,senddeptid,REALNAME,MOBILE,OrganizeName,departmentid,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,CreateDate,isblack,identifyid,score,IsPresence,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,u.IsTransfer,u.DepartureReason,u.fourpersontype,1 isleave,IsEpiboly,0 iscl,u.rolename,isleaving,(select inout from (select rownum,inout from bis_hikinoutlog where userid=u.USERID and to_char(createdate,'yyyy-mm-dd')=to_char(sysdate, 'yyyy-mm-dd') order by createdate desc) temp where rownum=1) as inout";
            if (!string.IsNullOrEmpty(softName) && softName.ToLower() == "gdhjb")
            {
                pagination.p_fields = @"Account,senddeptid,REALNAME,MOBILE,OrganizeName,departmentid,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,CreateDate,isblack,identifyid,score,IsPresence,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,u.IsTransfer,u.DepartureReason,u.fourpersontype,1 isleave,IsEpiboly,0 iscl,u.rolename,isleaving,
                                        (select inout from (select rownum,inout from bis_hikinoutlog where userid=u.USERID order by createdate desc) temp where rownum=1) as inout,
                                        case (select count(userid) from hjb_personset where moduletype=0 and userid=u.USERID) when 0 then 0 else 1 end as SpecialUser";
            }
            pagination.p_tablename = "v_userinfo u left join (select a.userid,sum(score) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + DateTime.Now.Year + "' group by a.userid) t on u.userid=t.userid";
            pagination.conditionJson = "Account!='System'";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");

                DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemvalue,itemcode,status from BIS_BLACKSET t where  deptcode='{0}' and (t.itemcode='01' or t.itemcode='06' or t.itemcode='07' or t.itemcode='08' or t.itemcode='11') order by itemcode", user.OrganizeCode));
                int len = dtItems.Rows.Count;
                string[] arr1 = new string[] { };//普通人员年龄条件
                string[] arr2 = new string[] { };//特种作业人员年龄条件
                string[] arr3 = new string[] { };//监理人员年龄条件
                string[] arr4 = new string[] { };//特种设备作业人员年龄条件
                string[] arr5 = new string[] { };//外包人员年龄条件
                StringBuilder sb = new StringBuilder("");
                if (len > 0)
                {
                    if (dtItems.Rows[0][1].ToString() == "01" && dtItems.Rows[0][2].ToString() == "1")
                    {
                        arr1 = dtItems.Rows[0][0].ToString().Split('|');
                    }
                }
                if (len > 1)
                {
                    if (dtItems.Rows[1][1].ToString() == "06" && dtItems.Rows[1][2].ToString() == "1")
                    {
                        arr2 = dtItems.Rows[1][0].ToString().Split('|');
                    }
                }
                if (len > 2)
                {
                    if (dtItems.Rows[2][1].ToString() == "07" && dtItems.Rows[2][2].ToString() == "1")
                    {
                        arr3 = dtItems.Rows[2][0].ToString().Split('|');
                    }
                }
                if (len > 3)
                {
                    if (dtItems.Rows[3][1].ToString() == "08" && dtItems.Rows[3][2].ToString() == "1")
                    {
                        arr4 = dtItems.Rows[3][0].ToString().Split('|');
                    }
                }
                if (len > 4)
                {
                    if (dtItems.Rows[4][1].ToString() == "11" && dtItems.Rows[4][2].ToString() == "1")
                    {
                        arr5 = dtItems.Rows[4][0].ToString().Split('|');
                    }
                }
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {

                    var queryParam = queryJson.ToJObject();
                    if (queryParam["datatype"].IsEmpty())
                    {
                        if (!string.IsNullOrEmpty(where) && (queryParam["code"].IsEmpty() || !queryJson.Contains("code")))
                        {
                            pagination.conditionJson += " and " + where;
                        }
                    }
                    if (!queryParam["isThree"].IsEmpty())
                    {
                        pagination.conditionJson += " and organizecode='" + user.OrganizeCode + "'";
                    }
                    if (string.IsNullOrWhiteSpace(where))
                    {
                        where = " organizecode='" + user.OrganizeCode + "'";
                    }
                    if (!queryParam["userStatus"].IsEmpty())
                    {

                        if (arr1.Length > 1)
                        {
                            sb = new StringBuilder("select '' userid from dual where 0=1");
                            if (arr1[0].Length > 0 && arr1[1].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and (u.isspecial='否' and u.isspecialequ='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr1[0], arr1[1]);
                            }
                        }
                        if (arr1.Length > 3)
                        {
                            if (arr1[2].Length > 0 && arr1[3].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and (u.isspecial='否' and u.isspecialequ='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr1[2], arr1[3]);
                            }
                        }
                        if (arr2.Length > 1)
                        {
                            if (arr2[0].Length > 0 && arr2[1].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr2[0], arr2[1]);
                            }
                        }
                        if (arr2.Length > 3)
                        {
                            if (arr2[2].Length > 0 && arr2[3].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr2[2], arr2[3]);
                            }
                        }
                        if (arr3.Length > 1)
                        {
                            if (arr3[0].Length > 0 && arr3[1].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and u.usertype='监理人员') t where age<{1} or age>{2}", where, arr3[0], arr3[1]);
                            }
                        }
                        if (arr3.Length > 3)
                        {
                            if (arr3[2].Length > 0 && arr3[3].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and u.usertype='监理人员') t where age<{1} or age>{2}", where, arr3[2], arr3[3]);
                            }
                        }
                        if (arr4.Length > 1)
                        {
                            if (arr4[0].Length > 0 && arr4[1].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr4[0], arr4[1]);

                            }
                        }
                        if (arr4.Length > 3)
                        {
                            if (arr4[2].Length > 0 && arr4[3].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr4[2], arr4[3]);
                            }
                        }
                        if (arr5.Length > 1)
                        {
                            if (arr5[0].Length > 0 && arr5[1].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and ROLENAME like '%承包商%') t where age<{1} or age>{2}", where, arr5[0], arr5[1]);

                            }
                        }
                        if (arr5.Length > 3)
                        {
                            if (arr5[2].Length > 0 && arr5[3].Length > 0)
                            {
                                sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and ROLENAME like '%承包商%') t where age<{1} or age>{2}", where, arr5[2], arr5[3]);
                            }
                        }
                        if (sb.Length > 0)
                        {
                            pagination.conditionJson += string.Format(" and u.userid in({0})", sb.ToString());
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and 0=1");
                        }

                    }
                }
                // pagination.conditionJson += string.Format(" or u.userid in(select userid from base_user where departmentid in( select t.departmentid from BASE_DEPARTMENT t where t.senddeptid='{0}'))", user.DeptId);
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(user.OrganizeId);

                Dictionary<string, string> dic = new CertificateBLL().GetOverdueCertList(pagination.conditionJson);
                var data = userBLL.GetPageList(pagination, queryJson);
                if (sb.Length == 0)
                {
                    sb = new StringBuilder("select '' userid from dual where 0=1");
                    if (arr1.Length > 1)
                    {
                        if (arr1[0].Length > 0 && arr1[1].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and (u.isspecial='否' and u.isspecialequ='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr1[0], arr1[1]);
                        }
                    }
                    if (arr1.Length > 3)
                    {
                        if (arr1[2].Length > 0 && arr1[3].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and (u.isspecial='否' and u.isspecialequ='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr1[2], arr1[3]);
                        }
                    }
                    if (arr2.Length > 1)
                    {
                        if (arr2[0].Length > 0 && arr2[1].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr2[0], arr2[1]);
                        }
                    }
                    if (arr2.Length > 3)
                    {
                        if (arr2[2].Length > 0 && arr2[3].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr2[2], arr2[3]);
                        }
                    }
                    if (arr3.Length > 1)
                    {
                        if (arr3[0].Length > 0 && arr3[1].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and u.usertype='监理人员') t where age<{1} or age>{2}", where, arr3[0], arr3[1]);
                        }
                    }
                    if (arr3.Length > 3)
                    {
                        if (arr3[2].Length > 0 && arr3[3].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and u.usertype='监理人员') t where age<{1} or age>{2}", where, arr3[2], arr3[3]);
                        }
                    }
                    if (arr4.Length > 1)
                    {
                        if (arr4[0].Length > 0 && arr4[1].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr4[0], arr4[1]);

                        }
                    }
                    if (arr4.Length > 3)
                    {
                        if (arr4[2].Length > 0 && arr4[3].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", where, arr4[2], arr4[3]);
                        }
                    }

                    if (arr5.Length > 1)
                    {
                        if (arr5[0].Length > 0 && arr5[1].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and {0} and  ROLENAME like '%承包商%') t where age<{1} or age>{2}", where, arr5[0], arr5[1]);

                        }
                    }
                    if (arr5.Length > 3)
                    {
                        if (arr5[2].Length > 0 && arr5[3].Length > 0)
                        {
                            sb.AppendFormat(" union all select userid from (select userid,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and {0} and  ROLENAME like '%承包商%') t where age<{1} or age>{2}", where, arr5[2], arr5[3]);
                        }
                    }
                }
                DataTable dtCL = null;
                if (sb.Length > 0)
                {
                    dtCL = departmentBLL.GetDataTable(sb.ToString());
                }
                foreach (DataRow dr in data.Rows)
                {
                    if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                    {
                        DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from base_department where encode=(select encode from base_department t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["DEPARTMENTCODE"], "部门", dr["organizeid"]));
                        if (dt.Rows.Count > 0)
                        {
                            string name = "";
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                name += dr1["fullname"].ToString() + "/";
                            }
                            dr["deptname"] = name.TrimEnd('/');
                        }
                    }
                    if (dr["IsEpiboly"].ToString() == "是" && softName == "xss")
                    {
                        dtItems = departmentBLL.GetDataTable(string.Format("select remark from XSS_ENTERRECORD where idcard='{0}' order by time desc", dr["identifyid"].ToString()));
                        if (dtItems.Rows.Count > 0)
                        {
                            dr["isleave"] = dtItems.Rows[0][0].ToString();
                        }
                    }
                    if (dtCL != null)
                    {
                        DataRow[] rows = dtCL.Select("userid='" + dr["userid"].ToString() + "'");
                        if (rows.Length > 0)
                        {
                            dr["iscl"] = 1;
                        }
                    }
                }
                var JsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    userdata = new { score = entity == null ? "100" : entity.ItemValue, certInfo = dic }

                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 工作记录
        /// </summary>
        /// <returns>返回分页列表Json</returns>   
        public ActionResult GetPageWorkListJson(string userId)
        {
            var data = new WorkRecordBLL().GetList(userId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 违章记录
        /// </summary>
        /// <returns>返回分页列表Json</returns>   
        public ActionResult GetWZListJson(Pagination pagination, string queryJson)
        {
            var data = new DesktopBLL().GetWZInfo(pagination, queryJson);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <param name="mode">证书类别</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageCertListJson(Pagination pagination, string queryJson, int mode = 0)
        {
            pagination.p_kid = "a.userid";
            pagination.p_fields = "certname,a.Gender,certnum,senddate,sendorgan,years,a.realname,a.identifyid,a.deptname,a.mobile,startdate,enddate,applydate,worktype,workitem,filepath,cid,remark,result";
            pagination.p_tablename = "v_userinfo a left join (select t.id cid,t.userid,certname,certnum,senddate,sendorgan,years,startdate,enddate,applydate,worktype,workitem,filepath,remark,'0' result from bis_certificate t left join v_userinfo u on t.userid=u.userid";
            pagination.conditionJson = "1=1 ";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string pType = "特种设备作业人员证";
            if (mode == 1)
            {
                pType = "特种作业操作证";
                pagination.conditionJson += " and isspecial='是' ";
                pagination.p_tablename += " where  isspecial='是' and (certname='特种作业操作证' or certtype='特种作业操作证')";
            }
            if (mode == 2)
            {
                pagination.p_tablename += " where  isspecialequ='是' and (certname='特种设备作业人员证' or certtype='特种设备作业人员证')";
                pagination.conditionJson += " and isspecialequ='是'";
            }
            pagination.p_tablename += ") b on a.userid=b.userid";
            string where = " 1=1 ";

            var queryParam = queryJson.ToJObject();
            //是否在场参数
            if (!queryParam["ispresence"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ispresence  = '{0}'", queryParam["ispresence"].ToString());
            }
            //是否当前电厂数据
            if (!queryParam["isself"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and organizeid  = '{0}'", user.OrganizeId);
            }
            //是否当前电厂数据
            if (!queryParam["workType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and worktype= '{0}'", queryParam["workType"].ToString());
            }
            if (!user.RoleName.Contains("省级"))
            {
                where = "departmentcode like '" + user.DeptCode + "%'";
                if (!user.IsSystem)
                {
                    string con = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                    if (!string.IsNullOrEmpty(con))
                    {
                        where = con;
                        pagination.conditionJson += " and " + where;
                    }
                    else
                    {
                        where = "1=1";
                    }
                }
            }
            else
            {
                if (!queryParam["departmentCode"].IsEmpty())
                {
                    var dept = departmentBLL.GetEntityByCode(queryParam["departmentCode"].ToString());
                    if (dept != null)
                    {
                        if (dept.Nature == "省级")
                        {
                            pagination.conditionJson += string.Format(" and departmentCode in(select encode from base_department where deptcode like '{0}%')", dept.DeptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and departmentCode  like '{0}%'", dept.EnCode);
                        }
                    }
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetPageList(pagination, queryJson);
            var deptBll = new DepartmentBLL();
            foreach (DataRow dr in data.Rows)
            {
                int count = deptBll.GetDataTable(string.Format("select count(1) from base_fileinfo where recid='{0}'", dr["cid"].ToString())).Rows[0][0].ToInt();
                if (count > 0)
                {
                    dr["filepath"] = "1";
                }
                if (mode == 1 && dr["applydate"].ToString().Length > 0)
                {
                    //判断是否有复审记录
                    count = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTAUDIT where certid='{0}' and audittype='复审' and to_char(auditdate,'yyyy')='{1}'", dr["cid"], DateTime.Parse(dr["applydate"].ToString()).ToString("yyyy"))).Rows[0][0].ToInt();
                    if (count > 0)
                    {
                        dr["Remark"] = "1";

                    }
                    else
                    {
                        dr["Remark"] = "0";
                    }
                }
                DataTable dtItems = deptBll.GetDataTable(string.Format("select result from BIS_CERTAUDIT where certid='{0}' and audittype='复审' order by createdate desc", dr["cid"]));
                if (dtItems.Rows.Count > 0)
                {
                    if (dtItems.Rows[0][0].ToString() == "不合格")
                    {
                        dr["Result"] = "1";
                    }
                    else
                    {
                        dr["Result"] = "0";
                    }

                }
            }
            CertificateBLL certBll = new CertificateBLL();
            decimal percent = certBll.GetCertPercent(pagination.records, where, pType);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),
                userdata = new { count = percent }
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 选择用户页面使用（不判断权限）
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetUserListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "USERID";
            pagination.p_fields = "REALNAME,MOBILE,OrganizeName,ORGANIZEID,DEPTNAME,DEPARTMENTID,DEPARTMENTCODE,DUTYNAME,POSTNAME,ROLENAME,ROLEID,MANAGER,ENABLEDMARK,ENCODE,ACCOUNT,NICKNAME,HEADICON,GENDER,EMAIL,OrganizeCode";
            pagination.p_tablename = "V_USERINFO t";
            pagination.conditionJson = "Account!='System'";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (!user.IsSystem)
            //{
            //    pagination.conditionJson = "Account!='System'";
            //}
            //else
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            //if (!string.IsNullOrEmpty(authType))
            //{
            //    switch (authType)
            //    {
            //        case "1":
            //            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
            //            break;
            //        case "2":
            //            pagination.conditionJson += " and departmentcode='" + user.DeptCode + "'";
            //            break;
            //        case "3":
            //            pagination.conditionJson += string.Format(" and (departmentcode like '{0}%' or departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
            //            break;
            //        case "4":
            //            pagination.conditionJson += " and organizecode='" + user.OrganizeCode + "'";
            //            break;
            //    }
            //}
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetUserList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 用户实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        //[HandlerMonitor(3, "查询用户对象信息!")]
        public ActionResult GetFormJson(string keyValue)
        {
            var userData = userBLL.GetEntity(keyValue);
            var dept = departmentBLL.GetEntity(userData.OrganizeId);
            string nature = "厂级";
            if (dept != null)
            {
                nature = dept.Nature;

            }
            DataTable dt = departmentBLL.GetDataTable(string.Format("select px_account from XSS_USER  where useraccount='{0}'", userData.Account));
            if (dt.Rows.Count > 0)
            {
                userData.Description = dt.Rows[0][0].ToString();
            }
            dt.Dispose();
            return Content(new { data = userData, nature = nature }.ToJson());
        }

        [HttpGet]
        public ActionResult GetFormJsonByAccount(string account)
        {
            var data = userBLL.GetList().Where(p => p.Account == account).FirstOrDefault();
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetUserInfo(string keyValue)
        {
            var data = userBLL.GetUserInfoEntity(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetUserInfoByAccount(string account)
        {
            var data = userBLL.GetUserInfoByAccount(account);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 人员门禁进出记录按单位汇总
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="deptType"></param>
        ///  <param name="deptName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEnterRecordList(string userName, string deptType, string deptName)
        {
            List<string> sb = new List<string>();
            string where = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(deptType))
            {
                where += string.Format(" and depttype='{0}'", deptType.Trim());
            }
            if (!string.IsNullOrWhiteSpace(deptName))
            {
                where += string.Format(" and fullname like '%{0}%'", deptName.Trim());
            }
            if (!string.IsNullOrWhiteSpace(userName))
            {
                where += string.Format(" and username like '%{0}%'", userName.Trim());
            }
            string sql = string.Format(@"select * from (select idcard,remark,row_number() over(partition by idcard order by time desc) as num from XSS_ENTERRECORD where idcard in(select idcard from XSS_ENTERRECORD where remark='0' and idcard in(select identifyid from base_user u left join base_department d on u.departmentid=d.departmentid where isepiboly='1' and {0}))) c where c.num=1 and remark=0", where);

            DataTable dtIdCards = departmentBLL.GetDataTable(sql);
            sb = dtIdCards.AsEnumerable().Select(t => t.Field<string>("idcard")).ToList();
            List<object> list = new List<object>();
            int count = 0;
            if (sb.Count > 0)
            {
                string idCards = string.Join(",", sb);
                idCards = idCards.Replace(",", "','");
                where = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    where = "depttype='" + deptType + "'";
                }
                if (!string.IsNullOrWhiteSpace(deptName))
                {
                    where += string.Format(" and fullname like '%{0}%'", deptName.Trim());
                }
                sql = string.Format("select d.departmentid,fullname,depttype from base_department d where {1} and  d.departmentid in(select u.departmentid from base_user u where u.isepiboly='1' and  u.identifyid in('{0}'))", idCards, where);
                DataTable dtDepts = departmentBLL.GetDataTable(sql);
                foreach (DataRow dr in dtDepts.Rows)
                {
                    DataTable dtUsers = departmentBLL.GetDataTable(string.Format("select userid,a.realname name,a.gender sex,a.identifyid idcard,a.dutyname from base_user a  where departmentid='{0}' and  identifyid in('{1}')", dr[0].ToString(), idCards));
                    dtUsers.Columns.Add("way"); dtUsers.Columns.Add("time"); dtUsers.Columns.Add("hours");
                    foreach (DataRow dr1 in dtUsers.Rows)
                    {
                        string idCard = dr1["idcard"].ToString();
                        DataTable dtItem = departmentBLL.GetDataTable(string.Format("select *from (select enrtyway,time,remark from XSS_ENTERRECORD where idcard='{0}' order by time desc) where rownum<3", idCard));
                        if (dtItem.Rows.Count > 0)
                        {
                            if (dtItem.Rows[0]["remark"].ToString() == "0")
                            {
                                long ticks = DateTime.Now.Ticks - DateTime.Parse(dtItem.Rows[0]["time"].ToString()).Ticks;
                                TimeSpan ts = new TimeSpan(ticks);
                                dr1["way"] = dtItem.Rows[0]["enrtyway"].ToString() + "→";
                                dr1["time"] = dtItem.Rows[0]["time"].ToString() + "--";
                                dr1["hours"] = Math.Round(ts.TotalHours, 2);
                                continue;
                            }
                            if (dtItem.Rows.Count == 1)
                            {
                                if (dtItem.Rows[0]["remark"].ToString() == "0")
                                {
                                    long ticks = DateTime.Now.Ticks - DateTime.Parse(dtItem.Rows[0]["time"].ToString()).Ticks;
                                    TimeSpan ts = new TimeSpan(ticks);
                                    dr1["way"] = dtItem.Rows[0]["enrtyway"].ToString() + "→";
                                    dr1["time"] = dtItem.Rows[0]["time"].ToString() + "--";
                                    dr1["hours"] = Math.Round(ts.TotalHours, 2);
                                    continue;
                                }
                                else
                                {
                                    dr1["way"] = "→" + dtItem.Rows[0]["enrtyway"].ToString();
                                    dr1["time"] = "--" + dtItem.Rows[0]["time"].ToString();
                                    dr1["hours"] = "-";
                                    continue;
                                }
                            }

                            if (dtItem.Rows.Count > 1)
                            {
                                if (dtItem.Rows[0]["remark"].ToString() == "1" && dtItem.Rows[1]["remark"].ToString() == "0")
                                {
                                    long ticks = DateTime.Parse(dtItem.Rows[0]["time"].ToString()).Ticks - DateTime.Parse(dtItem.Rows[1]["time"].ToString()).Ticks;
                                    TimeSpan ts = new TimeSpan(ticks);
                                    dr1["way"] = dtItem.Rows[1]["enrtyway"].ToString() + "→" + dtItem.Rows[0]["enrtyway"].ToString();
                                    dr1["time"] = dtItem.Rows[1]["time"].ToString() + "--" + dtItem.Rows[0]["time"].ToString();
                                    dr1["hours"] = Math.Round(ts.TotalHours, 2);

                                }
                                if (dtItem.Rows[0]["remark"].ToString() == "1" && dtItem.Rows[1]["remark"].ToString() == "1")
                                {

                                    dr1["way"] = "→" + dtItem.Rows[0]["enrtyway"].ToString();
                                    dr1["time"] = "--" + dtItem.Rows[0]["time"].ToString();
                                    dr1["hours"] = "-";

                                }
                            }
                        }

                    }
                    count += dtUsers.Rows.Count;
                    list.Add(new
                    {
                        deptId = dr[0].ToString(),
                        deptName = dr[1].ToString(),
                        deptType = dr[2].ToString(),
                        count = dtUsers.Rows.Count,
                        users = dtUsers
                    });
                }

            }
            //按单位内在厂人数排序显示
            var newList = (from s in list orderby s.GetType().GetProperty("count").GetValue(s, null) descending select s).ToList();
            return Content(new { data = newList, count = count }.ToJson());
        }
        [HttpPost]
        public ActionResult InsertEnterRecord(string idCard, string deptName, string time, string way, string userName)
        {
            try
            {
                string sql = string.Format("insert into XSS_ENTERRECORD(id,createtime,username,unitname,time,idcard,enrtyway,remark) values('{0}',to_date('{1}','yyyy-mm-dd hh24:mi:ss'),'{2}','{3}',to_date('{4}','yyyy-mm-dd hh24:mi:ss'),'{5}','{6}','{7}')", Guid.NewGuid().ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userName, deptName, time, idCard, way, 1);
                int count = departmentBLL.ExecuteSql(sql);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 外包单位人员统计
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportWBStat(string deptName = "", string deptType = "")
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                string content = CacheFactory.Cache().GetCache<string>("WBStat_" + user.OrganizeCode);
                DataTable dt = new DataTable();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    content = content.Replace("&nbsp;&nbsp;", " ");
                    dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(content);
                }
                else
                {
                    dt = GetWBStatDataTable(deptName, deptType);
                }

                Aspose.Cells.Workbook wb = new Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/外包人员导出模板.xls"));
                List<object> list = dt.AsEnumerable().Select(t => new { name = t.Field<string>("unitname"), sum = t.Field<string>("sum"), total = t.Field<string>("total"), lack = t.Field<string>("lack"), avgage = t.Field<string>("avgage") }).Where(t => !string.IsNullOrWhiteSpace(t.name)).Distinct().Skip(1).ToList<object>();
                dt.Columns.Remove("avgage");
                wb.Worksheets[0].Cells.ImportDataTable(dt, false, 1, 0);
                for (int j = 0; j < list.Count; j++)
                {
                    dynamic dy = list[j] as dynamic;
                    string value = dy.name;
                    string sum = dy.sum;
                    string total = dy.total;
                    string lack = dy.lack;
                    string avgage = dy.avgage;
                    if (j == 0)
                    {
                        wb.Worksheets[0].Cells.Merge(2, 0, 8, 1);
                        wb.Worksheets[0].Cells[2, 0].PutValue(j);

                        wb.Worksheets[0].Cells.Merge(2, 1, 8, 1);
                        wb.Worksheets[0].Cells[2, 1].PutValue(value);

                        wb.Worksheets[0].Cells.Merge(2, 2, 8, 1);
                        wb.Worksheets[0].Cells[2, 2].PutValue(sum);

                        wb.Worksheets[0].Cells.Merge(2, 3, 8, 1);
                        wb.Worksheets[0].Cells[2, 3].PutValue(total);

                        wb.Worksheets[0].Cells.Merge(2, 4, 8, 1);
                        wb.Worksheets[0].Cells[2, 4].PutValue(lack);

                        //wb.Worksheets[0].Cells.Merge(2, 5, 8, 1);
                        //wb.Worksheets[0].Cells[2, 5].PutValue(avgage);

                        wb.Worksheets[0].Cells.Merge(5, 8, 1, 2);
                        wb.Worksheets[0].Cells[5, 8].PutValue("平均年龄");
                        wb.Worksheets[0].Cells[5, 10].PutValue(avgage);
                    }
                    else
                    {
                        wb.Worksheets[0].Cells.Merge(8 * j + 4, 0, 8, 1);
                        wb.Worksheets[0].Cells[8 * j + 4, 0].PutValue(j);

                        wb.Worksheets[0].Cells.Merge(8 * j + 4, 1, 8, 1);
                        wb.Worksheets[0].Cells[8 * j + 4, 1].PutValue(value);

                        wb.Worksheets[0].Cells.Merge(8 * j + 4, 2, 8, 1);
                        wb.Worksheets[0].Cells[8 * j + 4, 2].PutValue(sum);

                        wb.Worksheets[0].Cells.Merge(8 * j + 4, 3, 8, 1);
                        wb.Worksheets[0].Cells[8 * j + 4, 3].PutValue(total);

                        wb.Worksheets[0].Cells.Merge(8 * j + 4, 4, 8, 1);
                        wb.Worksheets[0].Cells[8 * j + 4, 4].PutValue(lack);

                        //wb.Worksheets[0].Cells.Merge(10 * j + 2, 5, 8, 1);
                        //wb.Worksheets[0].Cells[10 * j + 2, 5].PutValue(avgage);

                        wb.Worksheets[0].Cells.Merge(8 * j + 4, 8, 1, 2);
                        wb.Worksheets[0].Cells[8 * j + 4, 8].PutValue("平均年龄");
                        wb.Worksheets[0].Cells[8 * j + 4, 10].PutValue(avgage);

                        string sql = string.Format(@"select cast(rownum as varchar(4))sno,t.* from (select userid,u.realname,d.fullname,u.dutyname,u.Gender,u.Nation,u.identifyid,(u.quickquery || u.manager || u.district || u.street || u.address) addr,
u.quickquery,u.manager,u.district,u.street,to_char(u.entertime,'yyyy-mm-dd') time,cast(count(a.LLLEGALPERSONID) as varchar(4)) wzcount, u.degreesid,u.specialtytype bz,to_char(u.birthday,'yyyy-mm-dd') birth,u.age,'√' hs,'' way,'' gsbx,'' ywbx,'' score,'' certs from base_user u left join base_department d on u.departmentid=d.departmentid left join (select LLLEGALPERSONID from  V_LLLEGALBASEINFO where flowstate='流程结束') a on u.userid=a.LLLEGALPERSONID where u.organizeid='{0}' and d.nature='承包商' and d.fullname='{1}' 
group by userid,u.realname,d.fullname,u.dutyname,u.Gender,u.Nation,u.identifyid,u.address,
u.quickquery,u.manager,u.district,u.street,u.entertime, u.degreesid,u.specialtytype,u.birthday,u.age) t", user.OrganizeId, value);
                        DataTable dtUsers = departmentBLL.GetDataTable(sql);
                        foreach (DataRow dr in dtUsers.Rows)
                        {
                            string stype = dr["bz"].ToString();
                            if (!string.IsNullOrWhiteSpace(stype))
                            {
                                DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemname from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem where itemcode='SpecialtyType') and itemvalue in('{0}')", stype.Replace(",", "','")));
                                string[] arr = dtItems.AsEnumerable().Select(t => t.Field<string>("itemname")).ToArray();
                                string bz = string.Join(",", arr);
                                dr["bz"] = bz;
                            }
                            sql = string.Format("select a.itemname from EPG_APTITUDEINVESTIGATEPEOPLE t left join base_dataitemdetail a on t.workertype=a.itemvalue  where t.id='{0}' and a.itemid=(select itemid from base_dataitem where itemcode='WorkerType')", dr["userid"].ToString());
                            DataTable dtWay = departmentBLL.GetDataTable(sql);
                            if (dtWay.Rows.Count > 0)
                            {
                                dr["way"] = dtWay.Rows[0][0].ToString();
                            }
                            sql = string.Format("select count(1) from base_fileinfo f where f.recid='{0}03'  union all select count(1) from base_fileinfo f where f.recid='{0}05'", dr["userid"].ToString());
                            dtWay = departmentBLL.GetDataTable(sql);
                            if (dtWay.Rows[0][0].ToInt() > 0)
                            {
                                dr["gsbx"] = "√";
                            }
                            if (dtWay.Rows[1][0].ToInt() > 0)
                            {
                                dr["ywbx"] = "√";
                            }
                            sql = string.Format("select wm_concat(certnum) from BIS_CERTIFICATE t where userid='{0}' and certtype='特种作业操作证'", dr["userid"].ToString());
                            dtWay = departmentBLL.GetDataTable(sql);
                            string certs = "";
                            if (dtWay.Rows.Count > 0)
                            {
                                if (dtWay.Rows[0][0] != null)
                                {
                                    certs += "特种作业证号:" + dtWay.Rows[0][0].ToString();
                                }
                            }
                            sql = string.Format("select wm_concat(certnum) from BIS_CERTIFICATE t where userid='{0}' and certtype='特种设备作业人员证'", dr["userid"].ToString());
                            dtWay = departmentBLL.GetDataTable(sql);
                            if (dtWay.Rows.Count > 0)
                            {
                                if (dtWay.Rows[0][0] != null)
                                {
                                    certs += "\n特种设备作业证号:" + dtWay.Rows[0][0].ToString();
                                }
                            }
                            if (certs.Length > 0)
                            {
                                dr["certs"] = certs;
                            }
                            try
                            {
                                string error = "";
                                string result = userBLL.GetTrainRecord(dr["userid"].ToString(), out error);
                                if (!string.IsNullOrWhiteSpace(result))
                                {
                                    DataTable dtExam = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(result);
                                    if (dtExam.Rows.Count > 0)
                                    {
                                        dr["score"] = dtExam.Rows[0]["exam_score"].ToString();
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }

                        }
                        int index = wb.Worksheets.Add();
                        wb.Worksheets[index].Name = value;

                        sql = string.Format(@"select '序号' sno,'' userid,'姓名' realname,'部门' fullname,'岗位' dutyname,'性别' Gender,'民族' Nation,'身份证号' identifyid,'家庭住址' addr,
'省' quickquery,'市' manager,'区(县)' district,'镇(乡)' street,'进厂时间' time,'有无安全记录' wzcount,'文凭' degreesid,'家属是否在公司' js,'专业分类' bz,'出生' birth,'年龄' age,'核实' hs,'用工方式' way,'工伤保险' gsbx,'人身意外伤害保险' ywbx,'安全教育分数' score,'特种作业证号' certs from dual");
                        DataTable dtSum = departmentBLL.GetDataTable(sql);
                        dtSum.Merge(dtUsers);
                        dtSum.Columns.Remove("userid");
                        int len = dtSum.Columns.Count;
                        wb.Worksheets[index].Cells.ImportDataTable(dtSum, false, 1, 0);
                        wb.Worksheets[index].AutoFitColumns();
                        wb.Worksheets[index].Cells.Merge(0, 0, 1, len);
                        string title = string.Format("{0}{1}", value, DateTime.Now.ToString("yyyy.MM.dd"));
                        wb.Worksheets[index].Cells[0, 0].PutValue(title);
                        wb.Worksheets[index].Cells[0, 0].Style.Font.IsBold = true;
                        wb.Worksheets[index].Cells[0, 0].Style.ShrinkToFit = true;
                        wb.Worksheets[index].Cells.SetRowHeight(0, 25);
                        wb.Worksheets[index].Cells[0, 0].Style.Font.Size = 18;
                        wb.Worksheets[index].Cells.SetColumnWidth(6, 20);
                        wb.Worksheets[index].Cells.SetColumnWidth(7, 25);
                        wb.Worksheets[index].Cells.SetColumnWidth(17, 15);
                        wb.Worksheets[index].Cells.SetColumnWidth(18, 10);
                        wb.Worksheets[index].Cells[0, 0].Style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                        StyleFlag sf = new StyleFlag();
                        sf.HorizontalAlignment = true;
                        sf.VerticalAlignment = true;
                        //sf.WrapText = true;
                        Style style = wb.Styles[wb.Styles.Add()];
                        style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                        wb.Worksheets[index].Cells.ApplyStyle(style, sf);
                    }
                }

                string fileName = "外包人员统计_" + DateTime.Now.ToString("yyyyMMdHHmmss") + ".xls";
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                wb.Save(Server.UrlEncode(fileName), FileFormatType.Excel2003, SaveType.OpenInBrowser, resp);
                return Success("操作成功", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 缓存外包人员统计数据
        /// </summary>
        /// <param name="deptName"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public DataTable GetWBStatDataTable(string deptName = "", string deptType = "")
        {
            try
            {

                Operator user = OperatorProvider.Provider.Current();
                DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemvalue,itemcode,status from BIS_BLACKSET t where deptcode='{0}' and (t.itemcode='01' or t.itemcode='06' or t.itemcode='07' or t.itemcode='08') order by itemcode", user.OrganizeCode));
                int len = dtItems.Rows.Count;
                string[] arr1 = new string[] { };//普通人员年龄条件
                string[] arr2 = new string[] { };//特种作业人员年龄条件
                string[] arr3 = new string[] { };//监理人员年龄条件
                string[] arr4 = new string[] { };//特种设备作业人员年龄条件
                StringBuilder sb = new StringBuilder();
                if (len > 0)
                {
                    if (dtItems.Rows[0][1].ToString() == "01" && dtItems.Rows[0][2].ToString() == "1")
                    {
                        arr1 = dtItems.Rows[0][0].ToString().Split('|');
                    }
                }
                if (len > 1)
                {
                    if (dtItems.Rows[1][1].ToString() == "06" && dtItems.Rows[1][2].ToString() == "1")
                    {
                        arr2 = dtItems.Rows[1][0].ToString().Split('|');
                    }
                }
                if (len > 2)
                {
                    if (dtItems.Rows[2][1].ToString() == "07" && dtItems.Rows[2][2].ToString() == "1")
                    {
                        arr3 = dtItems.Rows[2][0].ToString().Split('|');
                    }
                }
                if (len > 3)
                {
                    if (dtItems.Rows[3][1].ToString() == "08" && dtItems.Rows[3][2].ToString() == "1")
                    {
                        arr4 = dtItems.Rows[3][0].ToString().Split('|');
                    }
                }
                int sum = 100;
                int CLCount = 0;
                string where = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(deptName))
                {
                    where += " and fullname like '%" + deptName.Trim() + "%'";
                }
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    where += " and depttype='" + deptType.Trim() + "'";
                }
                DataTable dtTitle = new DataTable();
                DataTable dtStat = departmentBLL.GetDataTable(string.Format("select  '序号' sno,'单位' unitname,'合同人数' sum,'实际人数' total,'缺员' lack,'平均年龄' avgage,'地域结构' area,'人数' pcount,'占比' ratio,'性别' sex,'人数' sexcount,'占比' sexratio,'年龄' agerange,'人数' agecount,'占比' ageratio,'文化程度' xl,'人数' xlcount,'占比' xlratio,'未核验' status from dual "));
                DataTable dtDepts = departmentBLL.GetDataTable(string.Format("select d.departmentid,fullname,encode from base_department d where d.nature='承包商' and d.encode like '{0}%' and {1} and and parentid in (select departmentid from base_department where Organizeid='{2}' and Description='外包工程承包商')", user.OrganizeCode, where, user.OrganizeId));
                int index = 1;
                foreach (DataRow dr in dtDepts.Rows)
                {
                    string deptId = dr["departmentid"].ToString();
                    string deptCode = dr["encode"].ToString();
                    string unitName = dr["fullname"].ToString();
                    string sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' union all select count(1) num from base_user u where u.ispresence='1' and u.simplespelling='21' and u.departmentcode like '{0}%' union all select case when num=0 then 0 else round(age/num,2) end age from (select count(1) num,sum(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.departmentcode like '{0}%') t
union all select count(1) num from base_user u where u.gender='男' and u.ispresence='1'  and u.departmentcode like '{0}%'
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 20 and 30
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='小学' and u.departmentcode like '{0}%'", deptCode);
                    DataTable dtData = departmentBLL.GetDataTable(sql);
                    int total = dtData.Rows[0][0].ToInt();

                    sql = string.Format("select nvl(contractpersonnum,0) num from epg_outsourcingproject t where t.outprojectid='{0}'", deptId);
                    sum = departmentBLL.GetDataTable(sql).Rows[0][0].ToInt();
                    DataTable dt = departmentBLL.GetDataTable(string.Format("select '{5}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{4}' lack,'0' avgage,'外省' area,'0' pcount,'0' ratio,'男' sex,'0' sexcount,'0' sexratio,'20~30' agerange,'0' agecount,'0' ageratio,'小学' xl,'0' xlcount,'0' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, sum - total, index));
                    dt.Rows[0]["avgage"] = dtData.Rows[2][0];
                    dt.Rows[0]["pcount"] = total - dtData.Rows[1][0].ToInt(); dt.Rows[0]["ratio"] = total == 0 ? "0%" : Math.Round((total.ToDecimal() - dtData.Rows[1][0].ToDecimal()) * 100 / total, 2).ToString() + "%";
                    dt.Rows[0]["sexcount"] = dtData.Rows[3][0]; dt.Rows[0]["sexratio"] = total == 0 ? "0%" : Math.Round(dtData.Rows[3][0].ToDecimal() * 100 / total, 2).ToString() + "%";
                    dt.Rows[0]["agecount"] = dtData.Rows[4][0]; dt.Rows[0]["ageratio"] = total == 0 ? "0%" : Math.Round(dtData.Rows[4][0].ToDecimal() * 100 / total, 2).ToString() + "%";
                    dt.Rows[0]["xlcount"] = dtData.Rows[5][0]; dt.Rows[0]["xlratio"] = total == 0 ? "0%" : Math.Round(dtData.Rows[5][0].ToDecimal() * 100 / total, 2).ToString() + "%";

                    sql = string.Format(@"select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 31 and 40
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='初中' and u.departmentcode like '{0}%'", deptCode);
                    DataTable dtData1 = departmentBLL.GetDataTable(sql);

                    DataTable dt1 = departmentBLL.GetDataTable(string.Format(@"select '{14}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{13}' lack,'{5}' avgage,'江西省' area,'{4}' pcount,'{6}%' ratio,
'女' sex,'{7}' sexcount,'{8}%' sexratio,'31~40' agerange,'{9}' agecount,'{10}%' ageratio,'初中' xl,'{11}' xlcount,'{12}%' xlratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData.Rows[1][0].ToInt().ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData.Rows[1][0].ToDecimal() * 100 / total, 2), total - dtData.Rows[3][0].ToInt(), total == 0 ? 0 : Math.Round((total.ToDecimal() - dtData.Rows[3][0].ToDecimal()) * 100 / total, 2), dtData1.Rows[0][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total.ToDecimal(), 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));

                    dt.Merge(dt1);

                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余干县' and u.departmentcode like '{0}%' union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 41 and 50
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='高中' and u.departmentcode like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;其中余干县' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'41~50' agerange,'{7}' agecount,'{8}%' ageratio,'高中' xl,'{9}' xlcount,'{10}%' xlratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total, 2), dtData1.Rows[2][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[2][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));
                    dt.Merge(dt1);
                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and quickquery='江西' and u.District='余干县' and street='黄金埠镇' and u.departmentcode like '{0}%' union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 51 and 60
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='中专' and u.departmentcode like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄金埠镇' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'51~60' agerange,'{7}' agecount,'{8}%' ageratio,'中专' xl,'{9}' xlcount,'{10}%' xlratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total, 2), dtData1.Rows[2][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[2][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));
                    dt.Merge(dt1);

                    CLCount = 0;
                    if (arr1.Length > 1)
                    {
                        if (arr1[0].Length > 0 && arr1[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr1[0], arr1[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr1.Length > 3)
                    {
                        if (arr1[2].Length > 0 && arr1[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr1[2], arr1[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr2.Length > 1)
                    {
                        if (arr2[0].Length > 0 && arr2[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr2[0], arr2[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr2.Length > 3)
                    {
                        if (arr2[2].Length > 0 && arr2[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr2[2], arr2[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr3.Length > 1)
                    {
                        if (arr3[0].Length > 0 && arr3[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%'  and u.usertype='监理人员') t where age<{1} or age>{2}", deptCode, arr3[0], arr3[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr3.Length > 3)
                    {
                        if (arr3[2].Length > 0 && arr3[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and u.usertype='监理人员') t where age<{1} or age>{2}", deptCode, arr3[2], arr3[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr4.Length > 1)
                    {
                        if (arr4[0].Length > 0 && arr4[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr4[0], arr4[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr4.Length > 3)
                    {
                        if (arr4[2].Length > 0 && arr4[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr4[2], arr4[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and DistrictCode='1858' and u.departmentcode like '{0}%' 
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='大专' and u.departmentcode like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;其中余江县' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'超龄' agerange,'{7}' agecount,'{8%}' ageratio,'大专' xl,'{9}' xlcount,'{10}%' xlratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), CLCount, total == 0 ? 0 : Math.Round(CLCount.ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));
                    dt.Merge(dt1);

                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='锦江镇' and u.departmentcode like '{0}%' 
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='本科' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and Description='长假' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and Description='短假' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='硕士' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='画桥镇' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='黄庄乡' and u.departmentcode like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;锦江镇' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'请长假' agerange,'{7}' agecount,'{8}%' ageratio,'本科' xl,'{9}' xlcount,'{10}%' xlratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), dtData1.Rows[2][0].ToString(), total == 0 ? 0 : Math.Round(dtData1.Rows[2][0].ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));
                    dt.Merge(dt1);

                    sql = string.Format(@"select '{12}' sno, '{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;画桥镇' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'请短假' agerange,'{7}' agecount,'{8}%' ageratio,'硕士' xl,'{9}' xlcount,'{10}%' xlratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[5][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[5][0].ToDecimal() * 100 / total, 2), dtData1.Rows[3][0].ToString(), total == 0 ? 0 : Math.Round(dtData1.Rows[3][0].ToDecimal() * 100 / total, 2), dtData1.Rows[4][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[4][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index);
                    dt1 = departmentBLL.GetDataTable(sql);
                    dt.Merge(dt1);

                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{8}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{7}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄庄乡' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' xlratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[6][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[6][0].ToDecimal() * 100 / total, 2), sum - total, index));
                    dt.Merge(dt1);
                    dtStat.Merge(dt);
                    //                    if (index < dtDepts.Rows.Count)
                    //                    {
                    //                        dtTitle = departmentBLL.GetDataTable(string.Format(@"select '' sno, '' unitname,'' sum,'' total,'' lack,'' avgage,'' area,'' pcount,'' ratio,'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' xlratio,'' status from dual  union all select '序号' sno, '单位' unitname,'合同人数' sum,'实际人数' total,'缺员' lack,'平均年龄' avgage,'地域结构' area,'人数' pcount,'占比' ratio,'性别' sex,'人数' sexcount,'占比' sexratio,'年龄' agerange,'人数' agecount,'占比' ageratio,'文化程度' xl,'人数' xlcount,'占比' xlratio,'未核验' status from dual 
                    // "));
                    //                        dtStat.Merge(dtTitle);
                    //                    }
                    index++;
                }
                string sql3 = string.Format(@" departmentid in(select d.departmentid from base_department d where d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId);
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    sql3 = string.Format(@" departmentid in(select d.departmentid from base_department d where depttype='{2}' and d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and  Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId, deptType);
                }
                string sql2 = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and u.departmentcode like '{0}%' and {1}
union all select case when num=0 then 0 else round(age/num,2) end age from (select count(1) num,sum(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.departmentcode in({0}) and {1}) t
union all select count(1) num from base_user u where u.gender='男' and u.ispresence='1' and u.departmentcode like '{0}%' and {1}
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' and {1}) t
where t.age between 20 and 30
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' and {1}) t
where t.age between 31 and 40
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' and {1}) t
where t.age between 41 and 50
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' and {1}) t
where t.age between 51 and 60
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='小学' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='初中' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='高中' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='中专' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='大专' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='本科' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='硕士' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余干县' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余干县' and street='黄金埠镇' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='锦江镇' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='画桥镇' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='黄庄乡' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and Description='长假' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and Description='短假' and u.departmentcode like '{0}%' and {1}", user.OrganizeCode, sql3);
                DataTable dtData2 = departmentBLL.GetDataTable(sql2);
                int totalCount = dtData2.Rows[0][0].ToInt();
                string age = dtData2.Rows[2][0].ToString();
                int bsCount = dtData2.Rows[1][0].ToInt();
                int wsCount = totalCount - bsCount;
                int manCount = dtData2.Rows[3][0].ToInt();
                int age23Count = dtData2.Rows[4][0].ToInt();

                sql2 = string.Format("select nvl(sum(nvl(SAFEMANAGERPEOPLE,0)+nvl(ENGINEERWORKPEOPLE,0)+nvl(ENGINEERTECHPERSON,0)),0) num from EPG_OUTSOURINGENGINEER t where t.createuserorgcode='{0}' and ENGINEERSTATE='002'", user.OrganizeCode);
                sum = departmentBLL.GetDataTable(sql2).Rows[0][0].ToInt();

                DataTable dtSum = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{12}' lack,'{3}' avgage,'外省' area,'{4}' pcount,'{5}%' ratio,'男' sex,'{6}' sexcount,'{7}%' sexratio,'20~30' agerange,'{8}' agecount,'{9}%' ageratio,'小学' xl,'{10}' xlcount,'{11}%' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1'  and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                    "全厂合计", sum, totalCount, age, wsCount, Math.Round(wsCount.ToDecimal() * 100 / totalCount, 2), manCount, Math.Round(manCount.ToDecimal() * 100 / totalCount, 2), age23Count, Math.Round(age23Count.ToDecimal() * 100 / totalCount, 2), dtData2.Rows[8][0].ToString(), dtData2.Rows[8][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[8][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));

                DataTable dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{12}' lack,'{3}' avgage,'江西省' area,'{4}' pcount,'{5}%' ratio,'女' sex,'{6}' sexcount,'{7}%' sexratio,'31~40' agerange,'{8}' agecount,'{9}%' ageratio,'初中' xl,'{10}' xlcount,'{11}%' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                    "全厂合计", sum, totalCount, age, bsCount, Math.Round(bsCount.ToDecimal() * 100 / totalCount, 2), totalCount - manCount, Math.Round((totalCount - manCount).ToDecimal() * 100 / totalCount, 2), dtData2.Rows[5][0].ToInt(), dtData2.Rows[5][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[5][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[9][0].ToString(), dtData2.Rows[9][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[9][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{10}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;其中余干县' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'41~50' agerange,'{6}' agecount,'{7}%' ageratio,'高中' xl,'{8}' xlcount,'{9}%' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                    "全厂合计", sum, totalCount, age, dtData2.Rows[15][0].ToInt(), dtData2.Rows[15][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[15][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[6][0].ToInt(), dtData2.Rows[6][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[6][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[10][0].ToString(), dtData2.Rows[10][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[10][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{10}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄金埠镇' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'51~60' agerange,'{6}' agecount,'{7}%' ageratio,'中专' xl,'{8}' xlcount,'{9}%' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                                 "全厂合计", sum, totalCount, age, dtData2.Rows[16][0].ToInt(), dtData2.Rows[16][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[16][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[7][0].ToInt(), dtData2.Rows[7][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[7][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[11][0].ToString(), dtData2.Rows[11][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[11][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);


                CLCount = 0;

                if (arr1.Length > 1)
                {
                    if (arr1[0].Length > 0 && arr1[1].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr1[0], arr1[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr1.Length > 3)
                {
                    if (arr1[2].Length > 0 && arr1[3].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr1[2], arr1[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr2.Length > 1)
                {
                    if (arr2[0].Length > 0 && arr2[1].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr2[0], arr2[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr2.Length > 3)
                {
                    if (arr2[2].Length > 0 && arr2[3].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr2[2], arr2[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr3.Length > 1)
                {
                    if (arr3[0].Length > 0 && arr3[1].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%'  and u.usertype='监理人员') t where age<{1} or age>{2}", user.OrganizeCode, arr3[0], arr3[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr3.Length > 3)
                {
                    if (arr3[2].Length > 0 && arr3[3].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and u.usertype='监理人员') t where age<{1} or age>{2}", user.OrganizeCode, arr3[2], arr3[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr4.Length > 1)
                {
                    if (arr4[0].Length > 0 && arr4[1].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr4[0], arr4[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr4.Length > 3)
                {
                    if (arr4[2].Length > 0 && arr4[3].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr4[2], arr4[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{11}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;其中余江区' area,'{4}' pcount,'{5}' ratio,'' sex,'' sexcount,'' sexratio,'超龄' agerange,'{6}' agecount,'{7}%' ageratio,'大专' xl,'{8}' xlcount,'{9}%' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                               "全厂合计", sum, totalCount, age, dtData2.Rows[17][0].ToInt(), dtData2.Rows[17][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[17][0].ToDecimal() * 100 / totalCount, 2), CLCount, CLCount == 0 ? 0 : Math.Round(CLCount.ToDecimal() * 100 / totalCount, 2), dtData2.Rows[12][0].ToString(), dtData2.Rows[12][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[12][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{11}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;锦江镇' area,'{4}' pcount,'{5}' ratio,'' sex,'' sexcount,'' sexratio,'请长假' agerange,'{6}' agecount,'{7}%' ageratio,'大专' xl,'{8}' xlcount,'{9}%' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                             "全厂合计", sum, totalCount, age, dtData2.Rows[18][0].ToInt(), dtData2.Rows[18][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[18][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[21][0].ToString(), dtData2.Rows[21][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[21][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[13][0].ToString(), dtData2.Rows[13][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[13][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{11}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;画桥镇' area,'{4}' pcount,'{5}' ratio,'' sex,'' sexcount,'' sexratio,'请短假' agerange,'{6}' agecount,'{7}%' ageratio,'本科' xl,'{8}' xlcount,'{9}%' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                          "全厂合计", sum, totalCount, age, dtData2.Rows[19][0].ToInt(), dtData2.Rows[19][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[19][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[22][0].ToString(), dtData2.Rows[22][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[22][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[14][0].ToString(), dtData2.Rows[14][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[14][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{7}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{6}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄庄乡' area,'{4}' pcount,'{5}' ratio,'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' xlratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                       "全厂合计", sum, totalCount, age, dtData2.Rows[20][0].ToInt(), dtData2.Rows[20][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[20][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                DataTable dtSum1 = departmentBLL.GetDataTable(string.Format(@"select '序号' sno, '单位' unitname,'合同人数' sum,'实际人数' total,'缺员' lack,'平均年龄' avgage,'地域结构' area,'人数' pcount,'占比' ratio,'性别' sex,'人数' sexcount,'占比' sexratio,'年龄' agerange,'人数' agecount,'占比' ageratio,'文化程度' xl,'人数' xlcount,'占比' xlratio,'未核验' status from dual 
 "));

                dtSum1.Merge(dtSum);

                dtTitle = departmentBLL.GetDataTable(string.Format(@"select '' sno, '' unitname,'' sum,'' total,'' lack,'' avgage,'' area,'' pcount,'' ratio,'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' xlratio,'' status from dual 
 "));
                dtSum1.Merge(dtTitle);
                dtSum1.Merge(dtStat);
                CacheFactory.Cache().WriteCache(dtSum1.ToJson(), "WBStat_" + user.OrganizeCode, DateTime.Now.AddDays(2));
                return dtSum1;
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// 外包单位人员统计
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWBUserStat(string deptName = "", string deptType = "")
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                List<object> list = new List<object>();
                int count = 0;
                DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemvalue,itemcode,status from BIS_BLACKSET t where deptcode='{0}' and (t.itemcode='01' or t.itemcode='06' or t.itemcode='07' or t.itemcode='08') order by itemcode", user.OrganizeCode));
                int len = dtItems.Rows.Count;
                string[] arr1 = new string[] { };//普通人员年龄条件
                string[] arr2 = new string[] { };//特种作业人员年龄条件
                string[] arr3 = new string[] { };//监理人员年龄条件
                string[] arr4 = new string[] { };//特种设备作业人员年龄条件
                StringBuilder sb = new StringBuilder();
                if (len > 0)
                {
                    if (dtItems.Rows[0][1].ToString() == "01" && dtItems.Rows[0][2].ToString() == "1")
                    {
                        arr1 = dtItems.Rows[0][0].ToString().Split('|');
                    }
                }
                if (len > 1)
                {
                    if (dtItems.Rows[1][1].ToString() == "06" && dtItems.Rows[1][2].ToString() == "1")
                    {
                        arr2 = dtItems.Rows[1][0].ToString().Split('|');
                    }
                }
                if (len > 2)
                {
                    if (dtItems.Rows[2][1].ToString() == "07" && dtItems.Rows[2][2].ToString() == "1")
                    {
                        arr3 = dtItems.Rows[2][0].ToString().Split('|');
                    }
                }
                if (len > 3)
                {
                    if (dtItems.Rows[3][1].ToString() == "08" && dtItems.Rows[3][2].ToString() == "1")
                    {
                        arr4 = dtItems.Rows[3][0].ToString().Split('|');
                    }
                }
                int sum = 100;
                int CLCount = 0;
                string where = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(deptName))
                {
                    where += " and fullname like '%" + deptName.Trim() + "%'";
                }
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    where += " and depttype='" + deptType.Trim() + "'";
                }
                DataTable dtTitle = new DataTable();
                DataTable dtStat = departmentBLL.GetDataTable(string.Format("select  '序号' sno,'单位' unitname,'合同人数' sum,'实际人数' total,'缺员' lack,'平均年龄' avgage,'地域结构' area,'人数' pcount,'占比' ratio,'性别' sex,'人数' sexcount,'占比' sexratio,'年龄' agerange,'人数' agecount,'占比' ageratio,'文化程度' xl,'人数' xlcount,'占比' xlratio,'用工方式' yg,'人数' ygcount,'占比' ygratio,'未核验' status from dual "));
                DataTable dtDepts = departmentBLL.GetDataTable(string.Format("select d.departmentid,fullname,encode from base_department d where d.nature='承包商' and d.encode like '{0}%' and {1} and parentid in (select departmentid from base_department where Organizeid='{2}' and Description='外包工程承包商')", user.OrganizeCode, where, user.OrganizeId));
                int index = 1;
                foreach (DataRow dr in dtDepts.Rows)
                {
                    string deptId = dr["departmentid"].ToString();
                    string deptCode = dr["encode"].ToString();
                    string unitName = dr["fullname"].ToString();
                    string sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and u.departmentcode like '{0}%' union all select case when num=0 then 0 else round(age/num,2) end age from (select count(1) num,sum(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.departmentcode like '{0}%') t
union all select count(1) num from base_user u where u.gender='男' and u.ispresence='1'  and u.departmentcode like '{0}%'
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 20 and 30
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='小学' and u.departmentcode like '{0}%' union all select count(distinct t.identifyid) as num  from epg_aptitudeinvestigatepeople t where workertype=3 and outprojectcode  like '{0}%'", deptCode);
                    DataTable dtData = departmentBLL.GetDataTable(sql);
                    int total = dtData.Rows[0][0].ToInt();

                    sql = string.Format("select nvl(contractpersonnum,0) num from epg_outsourcingproject t where t.outprojectid='{0}'", deptId);
                    sum = departmentBLL.GetDataTable(sql).Rows[0][0].ToInt();
                    DataTable dt = departmentBLL.GetDataTable(string.Format("select '{5}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{4}' lack,'0' avgage,'外省' area,'0' pcount,'0' ratio,'男' sex,'0' sexcount,'0' sexratio,'20~30' agerange,'0' agecount,'0' ageratio,'小学' xl,'0' xlcount,'0' xlratio,'正式工' yg,'0' ygcount,'0' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, sum - total, index));
                    dt.Rows[0]["avgage"] = dtData.Rows[2][0];
                    dt.Rows[0]["pcount"] = total - dtData.Rows[1][0].ToInt(); dt.Rows[0]["ratio"] = total == 0 ? "0%" : Math.Round((total.ToDecimal() - dtData.Rows[1][0].ToDecimal()) * 100 / total, 2).ToString() + "%";
                    dt.Rows[0]["sexcount"] = dtData.Rows[3][0]; dt.Rows[0]["sexratio"] = total == 0 ? "0%" : Math.Round(dtData.Rows[3][0].ToDecimal() * 100 / total, 2).ToString() + "%";
                    dt.Rows[0]["agecount"] = dtData.Rows[4][0]; dt.Rows[0]["ageratio"] = total == 0 ? "0%" : Math.Round(dtData.Rows[4][0].ToDecimal() * 100 / total, 2).ToString() + "%";
                    dt.Rows[0]["xlcount"] = dtData.Rows[5][0]; dt.Rows[0]["xlratio"] = total == 0 ? "0%" : Math.Round(dtData.Rows[5][0].ToDecimal() * 100 / total, 2).ToString() + "%";
                    dt.Rows[0]["ygcount"] = dtData.Rows[6][0]; dt.Rows[0]["ygratio"] = total == 0 ? "0%" : Math.Round(dtData.Rows[6][0].ToDecimal() * 100 / total, 2).ToString() + "%";

                    sql = string.Format(@"select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 31 and 40
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='初中' and u.departmentcode like '{0}%' union all select count(distinct t.identifyid) as num  from epg_aptitudeinvestigatepeople t where workertype=1 and outprojectcode  like '{0}%'", deptCode);
                    DataTable dtData1 = departmentBLL.GetDataTable(sql);

                    DataTable dt1 = departmentBLL.GetDataTable(string.Format(@"select '{14}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{13}' lack,'{5}' avgage,'江西省' area,'{4}' pcount,'{6}%' ratio,
'女' sex,'{7}' sexcount,'{8}%' sexratio,'31~40' agerange,'{9}' agecount,'{10}%' ageratio,'初中' xl,'{11}' xlcount,'{12}%' xlratio,'劳务派遣' yg,'{15}' ygcount,'{16}%' ygratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData.Rows[1][0].ToInt().ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData.Rows[1][0].ToDecimal() * 100 / total, 2), total - dtData.Rows[3][0].ToInt(), total == 0 ? 0 : Math.Round((total.ToDecimal() - dtData.Rows[3][0].ToDecimal()) * 100 / total, 2), dtData1.Rows[0][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total.ToDecimal(), 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index, dtData1.Rows[2][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[2][0].ToDecimal() * 100 / total.ToDecimal(), 2)));

                    dt.Merge(dt1);

                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余干县' and u.departmentcode like '{0}%' union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 41 and 50
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='高中' and u.departmentcode like '{0}%' union all select count(distinct t.identifyid) as num  from epg_aptitudeinvestigatepeople t where workertype=2 and outprojectcode  like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;其中余干县' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'41~50' agerange,'{7}' agecount,'{8}%' ageratio,'高中' xl,'{9}' xlcount,'{10}%' xlratio,'农民工' yg,'{13}' ygcount,'{14}%' ygratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total, 2), dtData1.Rows[2][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[2][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index, dtData1.Rows[3][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[3][0].ToDecimal() * 100 / total.ToDecimal(), 2)));
                    dt.Merge(dt1);
                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.District='余干县' and street='黄金埠镇' and u.departmentcode like '{0}%' union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.departmentcode like '{0}%') t
where t.age between 51 and 60
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='中专' and u.departmentcode like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄金埠镇' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'51~60' agerange,'{7}' agecount,'{8}%' ageratio,'中专' xl,'{9}' xlcount,'{10}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total, 2), dtData1.Rows[2][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[2][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));
                    dt.Merge(dt1);

                    CLCount = 0;
                    if (arr1.Length > 1)
                    {
                        if (arr1[0].Length > 0 && arr1[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1'  and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr1[0], arr1[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr1.Length > 3)
                    {
                        if (arr1[2].Length > 0 && arr1[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr1[2], arr1[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr2.Length > 1)
                    {
                        if (arr2[0].Length > 0 && arr2[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr2[0], arr2[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr2.Length > 3)
                    {
                        if (arr2[2].Length > 0 && arr2[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr2[2], arr2[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr3.Length > 1)
                    {
                        if (arr3[0].Length > 0 && arr3[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%'  and u.usertype='监理人员') t where age<{1} or age>{2}", deptCode, arr3[0], arr3[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr3.Length > 3)
                    {
                        if (arr3[2].Length > 0 && arr3[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and u.usertype='监理人员') t where age<{1} or age>{2}", deptCode, arr3[2], arr3[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr4.Length > 1)
                    {
                        if (arr4[0].Length > 0 && arr4[1].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr4[0], arr4[1]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    if (arr4.Length > 3)
                    {
                        if (arr4[2].Length > 0 && arr4[3].Length > 0)
                        {
                            string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", deptCode, arr4[2], arr4[3]);
                            DataTable dtAge = departmentBLL.GetDataTable(sql1);
                            CLCount += dtAge.Rows[0][0].ToInt();
                        }
                    }
                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and u.departmentcode like '{0}%' 
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='大专' and u.departmentcode like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;其中余江县' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'超龄' agerange,'{7}' agecount,'{8}%' ageratio,'大专' xl,'{9}' xlcount,'{10}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), CLCount, total == 0 ? 0 : Math.Round(CLCount.ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));
                    dt.Merge(dt1);

                    sql = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='锦江镇' and u.departmentcode like '{0}%' 
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='本科' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and Description='长假' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and Description='短假' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='硕士' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='画桥镇' and u.departmentcode like '{0}%'
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='黄庄乡' and u.departmentcode like '{0}%'", deptCode);
                    dtData1 = departmentBLL.GetDataTable(sql);
                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;锦江镇' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'请长假' agerange,'{7}' agecount,'{8}%' ageratio,'本科' xl,'{9}' xlcount,'{10}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[0][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[0][0].ToDecimal() * 100 / total, 2), dtData1.Rows[2][0].ToString(), total == 0 ? 0 : Math.Round(dtData1.Rows[2][0].ToDecimal() * 100 / total, 2), dtData1.Rows[1][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[1][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index));
                    dt.Merge(dt1);

                    sql = string.Format(@"select '{12}' sno, '{0}' unitname,'{3}' sum,'{2}' total,'{11}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;画桥镇' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'请短假' agerange,'{7}' agecount,'{8}%' ageratio,'硕士' xl,'{9}' xlcount,'{10}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[5][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[5][0].ToDecimal() * 100 / total, 2), dtData1.Rows[3][0].ToString(), total == 0 ? 0 : Math.Round(dtData1.Rows[3][0].ToDecimal() * 100 / total, 2), dtData1.Rows[4][0].ToInt(), total == 0 ? 0 : Math.Round(dtData1.Rows[4][0].ToDecimal() * 100 / total.ToDecimal(), 2), sum - total, index);
                    dt1 = departmentBLL.GetDataTable(sql);
                    dt.Merge(dt1);

                    dt1 = departmentBLL.GetDataTable(string.Format(@"select '{8}' sno,'{0}' unitname,'{3}' sum,'{2}' total,'{7}' lack,'{5}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄庄乡' area,'{4}' pcount,'{6}%' ratio,
'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' xlratio,'' yg,'' ygcount,'' ygratio,'' status from 
(select count(1) total from base_user u  where u.ispresence='1' and u.departmentcode like '{1}%') t", unitName, deptCode, total, sum, dtData1.Rows[6][0].ToInt(), dtData.Rows[2][0].ToDecimal(), total == 0 ? 0 : Math.Round(dtData1.Rows[6][0].ToDecimal() * 100 / total, 2), sum - total, index));
                    dt.Merge(dt1);
                    dtStat.Merge(dt);
                    //                    if (index<dtDepts.Rows.Count)
                    //                    {
                    //                        dtTitle = departmentBLL.GetDataTable(string.Format(@"select '' sno, '' unitname,'' sum,'' total,'' lack,'' avgage,'' area,'' pcount,'' ratio,'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' xlratio,'' status from dual  union all select '序号' sno, '单位' unitname,'合同人数' sum,'实际人数' total,'缺员' lack,'平均年龄' avgage,'地域结构' area,'人数' pcount,'占比' ratio,'性别' sex,'人数' sexcount,'占比' sexratio,'年龄' agerange,'人数' agecount,'占比' ageratio,'文化程度' xl,'人数' xlcount,'占比' xlratio,'未核验' status from dual 
                    // "));
                    //                        dtStat.Merge(dtTitle);
                    //                    }
                    list.Add(new
                    {
                        deptId = deptId,
                        deptCode = deptCode,
                        deptName = unitName,
                        count = total,
                        rows = dt
                    });
                    index++;
                }
                string sql3 = string.Format(@" departmentid in(select d.departmentid from base_department d where d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId);
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    sql3 = string.Format(@" departmentid in(select d.departmentid from base_department d where  depttype='{2}' and d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId, deptType);
                }
                string sql4 = string.Format(@" outprojectid in(select d.departmentid from base_department d where d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId);
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    sql4 = string.Format(@" outprojectid in(select d.departmentid from base_department d where  depttype='{2}' and d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId, deptType);
                }
                string sql2 = string.Format(@"select count(1) num from base_user u where u.ispresence='1' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and  u.departmentcode like '{0}%' and {1}
union all select case when num=0 then 0 else round(age/num,2) end age from (select count(1) num,sum(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.departmentcode like '{0}%' and {1}) t
union all select count(1) num from base_user u where u.gender='男' and u.ispresence='1' and u.departmentcode like '{0}%' and {1}
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and   u.departmentcode like '{0}%' and {1}) t
where t.age between 20 and 30
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and   u.departmentcode like '{0}%' and {1}) t
where t.age between 31 and 40
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and  u.departmentcode like '{0}%' and {1}) t
where t.age between 41 and 50
union all select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and  u.departmentcode like '{0}%' and {1}) t
where t.age between 51 and 60
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='小学' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='初中' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='高中' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='中专' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='大专' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='本科' and u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and DegreesID='硕士' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余干县' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余干县' and street='黄金埠镇' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and u.departmentcode in({0}) and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='锦江镇' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='画桥镇' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and u.quickquery='江西' and District='余江县' and street='黄庄乡' and  u.departmentcode like '{0}%' and {1}
union all select count(1) num from base_user u where u.ispresence='1' and Description='长假' and  u.departmentcode like '{0}%'  and {1}
union all select count(1) num from base_user u where u.ispresence='1' and Description='短假' and  u.departmentcode like '{0}%'  and {1}
union all  select count(distinct t.identifyid) as num  from epg_aptitudeinvestigatepeople t where workertype=3 and outprojectcode like '{0}%' and {2}
union all  select count(distinct t.identifyid) as num  from epg_aptitudeinvestigatepeople t where workertype=1 and outprojectcode like '{0}%' and {2}
union all  select count(distinct t.identifyid) as num  from epg_aptitudeinvestigatepeople t where workertype=2 and outprojectcode like '{0}%' and {2}", user.OrganizeCode, sql3, sql4);
                DataTable dtData2 = departmentBLL.GetDataTable(sql2);
                int totalCount = dtData2.Rows[0][0].ToInt();
                string age = dtData2.Rows[2][0].ToString();
                int bsCount = dtData2.Rows[1][0].ToInt();
                int wsCount = totalCount - bsCount;
                int manCount = dtData2.Rows[3][0].ToInt();
                int age23Count = dtData2.Rows[4][0].ToInt();

                sql2 = string.Format("select sum(nvl(ContractPersonNum,0)) num from epg_outsourcingproject a left join base_department b on a.outprojectid=b.departmentid where b.parentid in (select departmentid from base_department t where t.description ='外包工程承包商' and Organizeid='{0}') and {1}", user.OrganizeId, sql4);
                sum = departmentBLL.GetDataTable(sql2).Rows[0][0].ToInt();

                DataTable dtSum = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{12}' lack,'{3}' avgage,'外省' area,'{4}' pcount,'{5}%' ratio,'男' sex,'{6}' sexcount,'{7}%' sexratio,'20~30' agerange,'{8}' agecount,'{9}%' ageratio,'小学' xl,'{10}' xlcount,'{11}%' xlratio,'正式工' yg,'{13}' ygcount,'{14}%' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1'  and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                    "全厂合计", sum, totalCount, age, wsCount, totalCount == 0 ? 0 : Math.Round(wsCount.ToDecimal() * 100 / totalCount, 2), manCount, totalCount == 0 ? 0 : Math.Round(manCount.ToDecimal() * 100 / totalCount, 2), age23Count, totalCount == 0 ? 0 : Math.Round(age23Count.ToDecimal() * 100 / totalCount, 2), dtData2.Rows[8][0].ToString(), dtData2.Rows[8][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[8][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, dtData2.Rows[23][0].ToString(), dtData2.Rows[23][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[23][0].ToDecimal() * 100 / totalCount, 2)));

                DataTable dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{12}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{12}' lack,'{3}' avgage,'江西省' area,'{4}' pcount,'{5}%' ratio,'女' sex,'{6}' sexcount,'{7}%' sexratio,'31~40' agerange,'{8}' agecount,'{9}%' ageratio,'初中' xl,'{10}' xlcount,'{11}%' xlratio,'劳务派遣' yg,'{13}' ygcount,'{14}%' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                    "全厂合计", sum, totalCount, age, bsCount, totalCount == 0 ? 0 : Math.Round(bsCount.ToDecimal() * 100 / totalCount, 2), totalCount - manCount, totalCount == 0 ? 0 : Math.Round((totalCount - manCount).ToDecimal() * 100 / totalCount, 2), dtData2.Rows[5][0].ToInt(), dtData2.Rows[5][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[5][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[9][0].ToString(), dtData2.Rows[9][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[9][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, dtData2.Rows[24][0].ToString(), dtData2.Rows[24][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[24][0].ToDecimal() * 100 / totalCount, 2)));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{10}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;其中余干县' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'41~50' agerange,'{6}' agecount,'{7}%' ageratio,'高中' xl,'{8}' xlcount,'{9}%' xlratio,'农民工' yg,'{11}' ygcount,'{12}%' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                    "全厂合计", sum, totalCount, age, dtData2.Rows[15][0].ToInt(), dtData2.Rows[15][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[15][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[6][0].ToInt(), dtData2.Rows[6][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[6][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[10][0].ToString(), dtData2.Rows[10][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[10][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, dtData2.Rows[25][0].ToString(), dtData2.Rows[25][0].ToInt() == 0 ? 0 : Math.Round(dtData2.Rows[25][0].ToDecimal() * 100 / totalCount, 2)));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{10}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄金埠镇' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'51~60' agerange,'{6}' agecount,'{7}%' ageratio,'中专' xl,'{8}' xlcount,'{9}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                                 "全厂合计", sum, totalCount, age, dtData2.Rows[16][0].ToInt(), dtData2.Rows[16][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[16][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[7][0].ToInt(), dtData2.Rows[7][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[7][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[11][0].ToString(), dtData2.Rows[11][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[11][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);


                CLCount = 0;

                if (arr1.Length > 1)
                {
                    if (arr1[0].Length > 0 && arr1[1].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr1[0], arr1[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr1.Length > 3)
                {
                    if (arr1[2].Length > 0 && arr1[3].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='否' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr1[2], arr1[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr2.Length > 1)
                {
                    if (arr2[0].Length > 0 && arr2[1].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr2[0], arr2[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr2.Length > 3)
                {
                    if (arr2[2].Length > 0 && arr2[3].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.isspecial='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr2[2], arr2[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr3.Length > 1)
                {
                    if (arr3[0].Length > 0 && arr3[1].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and u.usertype='监理人员') t where age<{1} or age>{2}", user.OrganizeCode, arr3[0], arr3[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr3.Length > 3)
                {
                    if (arr3[2].Length > 0 && arr3[3].Length > 0)
                    {
                        sql2 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and u.usertype='监理人员') t where age<{1} or age>{2}", user.OrganizeCode, arr3[2], arr3[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql2);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr4.Length > 1)
                {
                    if (arr4[0].Length > 0 && arr4[1].Length > 0)
                    {
                        string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='男' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr4[0], arr4[1]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql1);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                if (arr4.Length > 3)
                {
                    if (arr4[2].Length > 0 && arr4[3].Length > 0)
                    {
                        string sql1 = string.Format("select count(1) from (select (cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from base_user u where u.ispresence='1' and u.gender='女' and u.departmentcode like '{0}%' and (u.ISSPECIALEQU='是' and u.usertype<>'监理人员')) t where age<{1} or age>{2}", user.OrganizeCode, arr4[2], arr4[3]);
                        DataTable dtAge = departmentBLL.GetDataTable(sql1);
                        CLCount += dtAge.Rows[0][0].ToInt();
                    }
                }
                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{11}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;其中余江县' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'超龄' agerange,'{6}' agecount,'{7}%' ageratio,'大专' xl,'{8}' xlcount,'{9}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                               "全厂合计", sum, totalCount, age, dtData2.Rows[17][0].ToInt(), dtData2.Rows[17][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[17][0].ToDecimal() * 100 / totalCount, 2), CLCount, CLCount == 0 || totalCount == 0 ? 0 : Math.Round(CLCount.ToDecimal() * 100 / totalCount, 2), dtData2.Rows[12][0].ToString(), dtData2.Rows[12][0].ToInt() == 0 || totalCount == 0 ? 0 : Math.Round(dtData2.Rows[12][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{11}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;锦江镇' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'请长假' agerange,'{6}' agecount,'{7}%' ageratio,'大专' xl,'{8}' xlcount,'{9}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                             "全厂合计", sum, totalCount, age, dtData2.Rows[18][0].ToInt(), dtData2.Rows[18][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[18][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[21][0].ToString(), dtData2.Rows[21][0].ToInt() == 0 || totalCount == 0 ? 0 : Math.Round(dtData2.Rows[21][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[13][0].ToString(), dtData2.Rows[13][0].ToInt() == 0 || totalCount == 0 ? 0 : Math.Round(dtData2.Rows[13][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{11}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{10}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;画桥镇' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'请短假' agerange,'{6}' agecount,'{7}%' ageratio,'本科' xl,'{8}' xlcount,'{9}%' xlratio,'' yg,'' ygcount,'' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                          "全厂合计", sum, totalCount, age, dtData2.Rows[19][0].ToInt(), dtData2.Rows[19][0].ToInt() == 0 || totalCount == 0 ? 0 : Math.Round(dtData2.Rows[19][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[22][0].ToString(), dtData2.Rows[22][0].ToInt() == 0 || totalCount == 0 ? 0 : Math.Round(dtData2.Rows[22][0].ToDecimal() * 100 / totalCount, 2), dtData2.Rows[14][0].ToString(), dtData2.Rows[14][0].ToInt() == 0 || totalCount == 0 ? 0 : Math.Round(dtData2.Rows[14][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                dtItems2 = departmentBLL.GetDataTable(string.Format(@"select '{7}' sno,'{0}' unitname,'{1}' sum,'{2}' total,'{6}' lack,'{3}' avgage,'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;黄庄乡' area,'{4}' pcount,'{5}%' ratio,'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' xlratio,'' yg,'' ygcount,'' ygratio,'' status from (select count(1) total from base_user u  where u.ispresence='1' and u.isepiboly='1' and u.departmentcode like '{1}%') t",
                       "全厂合计", sum, totalCount, age, dtData2.Rows[20][0].ToInt(), dtData2.Rows[20][0].ToInt() == 0 ? 0 : totalCount == 0 ? 0 : Math.Round(dtData2.Rows[20][0].ToDecimal() * 100 / totalCount, 2), sum - totalCount, 0));
                dtSum.Merge(dtItems2);

                DataTable dtSum1 = departmentBLL.GetDataTable(string.Format(@"select '序号' sno, '单位' unitname,'合同人数' sum,'实际人数' total,'缺员' lack,'平均年龄' avgage,'地域结构' area,'人数' pcount,'占比' ratio,'性别' sex,'人数' sexcount,'占比' sexratio,'年龄' agerange,'人数' agecount,'占比' ageratio,'文化程度' xl,'人数' xlcount,'占比' xlratio,'用工方式' yg,'人数' ygcount,'占比' ygratio, '未核验' status from dual 
 "));

                dtSum1.Merge(dtSum);

                dtTitle = departmentBLL.GetDataTable(string.Format(@"select '' sno, '' unitname,'' sum,'' total,'' lack,'' avgage,'' area,'' pcount,'' ratio,'' sex,'' sexcount,'' sexratio,'' agerange,'' agecount,'' ageratio,'' xl,'' xlcount,'' yg,'' ygcount,'' ygratio,'' xlratio,'' status from dual 
 "));
                dtSum1.Merge(dtTitle);
                dtSum1.Merge(dtStat);
                CacheFactory.Cache().WriteCache(dtSum1.ToJson(), "WBStat_" + user.OrganizeCode, DateTime.Now.AddDays(2));

                sql2 = string.Format(@"select count(1) from base_user where ispresence='1' and departmentid in(select d.departmentid from base_department d where d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId);
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    sql2 = string.Format(@"select count(1) from base_user where ispresence='1' and departmentid in(select d.departmentid from base_department d where depttype='{2}' and d.nature='承包商' and d.encode like '{0}%' and parentid in (select departmentid from base_department where Organizeid='{1}' and Description='外包工程承包商'))", user.OrganizeCode, user.OrganizeId, deptType);
                }
                totalCount = departmentBLL.GetDataTable(sql2).Rows[0][0].ToInt();
                dtSum.Rows[0]["total"] = totalCount;
                list.Insert(0, new
                {
                    deptId = user.OrganizeId,
                    deptCode = user.OrganizeCode,
                    deptName = "全厂合计",
                    count = totalCount,
                    rows = dtSum
                });
                //按单位内在厂人数排序显示
                var newList = (from s in list orderby s.GetType().GetProperty("count").GetValue(s, null) descending select s).ToList();
                return Content(new { data = newList, count = count }.ToJson());
                // return Content(new { data = list, count = count }.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 人员门禁进出记录(西塞山)
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEnterRecord(string idCard)
        {
            try
            {
                List<string> ids = new List<string>();
                List<object> list = new List<object>();
                DataTable dt = departmentBLL.GetDataTable(string.Format("select * from XSS_ENTERRECORD where idcard='{0}' and time>to_date('{1}','yyyy-mm-dd hh24:mi:ss') order by time desc", idCard, DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd 00:00:00")));
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["remark"].ToString() == "0")
                    {
                        if (j == 0)
                        {
                            ids.Add(dt.Rows[j]["id"].ToString());
                            long ticks = DateTime.Now.Ticks - DateTime.Parse(dt.Rows[j]["time"].ToString()).Ticks;
                            TimeSpan ts = new TimeSpan(ticks);
                            list.Add(new
                            {
                                entryWay = dt.Rows[j]["enrtyway"].ToString() + "→",
                                time = dt.Rows[j]["time"].ToString() + "--",
                                hours = Math.Round(ts.TotalHours, 2),
                                status = list.Count > 0 ? 0 : 1    //在厂
                            });

                            continue;
                        }
                        else
                        {
                            if (dt.Rows[j - 1]["remark"].ToString() == "0" && !ids.Contains(dt.Rows[j]["id"].ToString()))
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                list.Add(new
                                {
                                    entryWay = dt.Rows[j]["enrtyway"].ToString() + "→",
                                    time = dt.Rows[j]["time"].ToString() + "--",
                                    hours = "-",
                                    status = list.Count > 0 ? 0 : 1  //在厂
                                });

                            }
                            else
                            {
                                if (!ids.Contains(dt.Rows[j]["id"].ToString()))
                                {
                                    long ticks = DateTime.Parse(dt.Rows[j - 1]["time"].ToString()).Ticks - DateTime.Parse(dt.Rows[j]["time"].ToString()).Ticks;
                                    TimeSpan ts = new TimeSpan(ticks);
                                    ids.Add(dt.Rows[j]["id"].ToString());
                                    ids.Add(dt.Rows[j - 1]["id"].ToString());
                                    list.Add(new
                                    {
                                        entryWay = dt.Rows[j]["enrtyway"].ToString() + "→" + dt.Rows[j - 1]["enrtyway"].ToString(),
                                        time = dt.Rows[j]["time"].ToString() + "--" + dt.Rows[j - 1]["time"].ToString(),
                                        hours = Math.Round(ts.TotalHours, 2),
                                        status = list.Count > 0 ? 0 : 2   //离场
                                    });

                                }

                            }
                        }
                    }
                    else
                    {
                        if (j + 1 < dt.Rows.Count)
                        {
                            if (dt.Rows[j + 1]["remark"].ToString() == "0" && !ids.Contains(dt.Rows[j]["id"].ToString()))
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                ids.Add(dt.Rows[j + 1]["id"].ToString());
                                long ticks = DateTime.Parse(dt.Rows[j]["time"].ToString()).Ticks - DateTime.Parse(dt.Rows[j + 1]["time"].ToString()).Ticks;
                                TimeSpan ts = new TimeSpan(ticks);
                                list.Add(new
                                {
                                    entryWay = dt.Rows[j + 1]["enrtyway"].ToString() + "→" + dt.Rows[j]["enrtyway"].ToString(),
                                    time = dt.Rows[j + 1]["time"].ToString() + "--" + dt.Rows[j]["time"].ToString(),
                                    hours = Math.Round(ts.TotalHours, 2),
                                    status = list.Count > 0 ? 0 : 2   //离场
                                });

                            }
                            else
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                list.Add(new
                                {
                                    entryWay = "→" + dt.Rows[j]["enrtyway"].ToString(),
                                    time = "--" + dt.Rows[j]["time"].ToString(),
                                    hours = "-",
                                    status = list.Count > 0 ? 0 : 2   //离场
                                });

                            }
                        }
                        else
                        {
                            if (!ids.Contains(dt.Rows[j]["id"].ToString()))
                            {
                                ids.Add(dt.Rows[j]["id"].ToString());
                                list.Add(new
                                {
                                    entryWay = "→" + dt.Rows[j]["enrtyway"].ToString(),
                                    time = "--" + dt.Rows[j]["time"].ToString(),
                                    hours = "-",
                                    status = list.Count > 0 ? 0 : 2   //离场
                                });

                            }

                        }

                    }
                }

                return Content(list.ToJson());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }


        /// <summary>
        /// 人员门禁进出记录(黄金埠)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAttendance(string userId)
        {
            try
            {
                var queryString = new
                {
                    UserId = userId,
                    StartDate = DateTime.Now.AddMonths(-1).Date,
                    EndDate = DateTime.Now.Date
                }.ToJson();

                var result = hikInOut.GetList(queryString);
                return Content(result.ToJson());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }


        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="Account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistAccount(string Account, string keyValue)
        {
            bool IsOk = userBLL.ExistAccount(Account, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 身份证不能重复
        /// </summary>
        /// <param name="IdentifyID">身份证号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistIdentifyID(string IdentifyID, string keyValue)
        {
            bool IsOk = userBLL.ExistIdentifyID(IdentifyID, keyValue);
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HandlerMonitor(6, "删除用户信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            UserInfoEntity entity = userBLL.GetUserInfoEntity(keyValue);
            Operator user = OperatorProvider.Provider.Current();
            if (keyValue == "System")
            {
                throw new Exception("当前账户不能删除");
            }
            userBLL.RemoveForm(keyValue);
            Task.Run(() =>
            {
                DeleteHikUser(keyValue);
            });


            //毕节删除
            UserEntity uentity = userBLL.GetEntity(keyValue);
            DeleteHdgzUser(uentity);
            string moduleId = SystemInfo.CurrentModuleId;
            string moduleName = SystemInfo.CurrentModuleName;
            DataItemDetailBLL di = new DataItemDetailBLL();
            var task = Task.Factory.StartNew(() =>
            {
                LogEntity logEntity = new LogEntity();
                logEntity.Browser = this.Request.Browser.Browser;
                logEntity.CategoryId = 6;
                logEntity.OperateTypeId = "6";
                logEntity.OperateType = "删除";
                logEntity.OperateAccount = user.UserName;
                logEntity.OperateUserId = user.UserId;
                logEntity.ExecuteResult = 1;
                logEntity.Module = moduleName;
                logEntity.ModuleId = moduleId;
                logEntity.ExecuteResultJson = "操作信息:删除姓名为" + entity.RealName + ",账号为" + entity.Account + "的" + entity.OrganizeName + "-" + entity.DeptName + "用户信息, 请求引用: 无, 其他信息:无";
                LogBLL.WriteLog(logEntity);
                if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                {
                    DeleteUser(entity, user);
                }

            });
            string way = di.GetItemValue("WhatWay");
            DepartmentEntity org = departmentBLL.GetEntity(entity.OrganizeId);
            //对接.net培训平台
            if (way == "0")
            {

            }

            //对接java培训平台
            if (way == "1" && org.IsTrain == 1)
            {
                Task.Factory.StartNew(() =>
                {
                    if (entity != null)
                    {
                        object obj = new
                        {
                            action = "delete",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            userId = keyValue,
                            account = entity.Account,
                            companyId = org.InnerPhone
                        };
                        List<object> list = new List<object>();
                        list.Add(obj);
                        Busines.JPush.JPushApi.PushMessage(list, 1);

                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 5;
                        logEntity.OperateTypeId = ((int)OperationType.Delete).ToString();
                        logEntity.OperateType = "删除用户";
                        logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                        logEntity.OperateUserId = user.UserId;

                        logEntity.ExecuteResult = 1;
                        logEntity.ExecuteResultJson = string.Format("同步用户(删除用户)到java培训平台,同步信息:\r\n{0}", list.ToJson());
                        logEntity.Module = moduleName;
                        logEntity.ModuleId = moduleId;
                        logEntity.WriteLog();
                    }
                });
            }
            return Success("删除成功。");
        }
        private void DeleteUser(UserInfoEntity user, Operator currUser)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //当前操作用户账号
                nc.Add("account", currUser.Account);
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "DeleteUser?keyValue=" + user.UserId), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：删除数据失败，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            //将同步结果写入日志文件
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
        }
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)用户信息")]
        public ActionResult SaveForm(string keyValue, string strUserEntity, string FormInstanceId, string strModuleFormInstanceEntity)
        {
            try
            {
                UserEntity userEntity = strUserEntity.ToObject<UserEntity>();
                string res = userBLL.IsBalckUser(keyValue, userEntity.IdentifyID);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    return Error(string.Format("该用户已于时间{0}被加入黑名单！", res));
                }
                ModuleFormInstanceEntity moduleFormInstanceEntity = strModuleFormInstanceEntity.ToObject<ModuleFormInstanceEntity>();


                string objectId = userBLL.SaveForm(keyValue, userEntity);
                moduleFormInstanceEntity.ObjectId = objectId;
                moduleFormInstanceBll.SaveEntity(FormInstanceId, moduleFormInstanceEntity);

                //毕节更新
                List<UserEntity> userEntityList = new List<UserEntity>();
                userEntityList.Add(userEntity);
                UpdateHdgzUser(userEntityList);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);

            }

        }
        /// <summary>
        /// 人员离场
        /// </summary>
        /// <param name="leaveTime">离场时间</param>
        /// <param name="userId">用户Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Leave(string leaveTime, [System.Web.Http.FromBody]string userId, [System.Web.Http.FromBody]string DepartureReason)
        {
            try
            {
                if (userBLL.SetLeave(userId, leaveTime, DepartureReason) > 0)
                {
                    var currUser = OperatorProvider.Provider.Current();
                    string sql = string.Format("select a.* from v_userinfo a where a.userid in ('" + userId.Replace(",", "','") + "')");
                    List<UserInfoEntity> users = userBLL.GetUserListBySql(sql).ToList();
                    Task.Run(() =>
                    {
                        WorkRecord(users, currUser);
                    });
                    Task.Run(() =>
                    {
                        SyZg(leaveTime, DepartureReason, users, currUser);
                    });
                    Task.Run(() =>
                    {
                        SaveForbidden(leaveTime, DepartureReason, userId, -1);
                    });
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 根据修改类型设置受训角色ID
        /// </summary>
        /// <param name="oldId">原始Id(多个用英文逗号分隔)</param>
        /// <param name="newId">新的Id(多个用英文逗号分隔)</param>
        /// <param name="type">0：追加，1:覆盖</param>
        /// <returns></returns>
        private string GetRoleId(string oldId, string newId, string type)
        {
            newId = newId.Trim(',');
            string roleId = "";
            if (string.IsNullOrWhiteSpace(oldId))
            {
                roleId = newId;
            }
            else
            {
                oldId = oldId.Trim(',');
                if (type == "1")
                {
                    roleId = newId;
                }
                else
                {
                    List<string> lstOld = oldId.Split(',').ToList();
                    List<string> lstNew = newId.Split(',').ToList();
                    foreach (string id in lstOld)
                    {
                        if (!lstNew.Contains(id))
                        {
                            lstNew.Add(id);
                        }
                    }
                    roleId = string.Join(",", lstNew);
                }

            }
            return roleId;
        }
        /// <summary>
        /// 批量修改受训角色
        /// </summary>
        /// <param name="leaveTime">离场时间</param>
        /// <param name="userId">用户Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "修改受训角色")]
        public ActionResult EditTrainRole(string type, [System.Web.Http.FromBody]string userId, [System.Web.Http.FromBody]string roleId)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                Expression<Func<UserEntity, bool>> condition = e => userId.Contains(e.UserId);
                var lstUsers = userBLL.GetListForCon(condition);
                List<object> list = new List<object>();
                foreach (UserEntity t in lstUsers)
                {
                    string trainRoleId = GetRoleId(t.TrainRoleId, roleId, type);
                    t.TrainRoleId = trainRoleId;
                    string uId = userBLL.SaveForm(t.UserId, t);
                    if (!string.IsNullOrWhiteSpace(uId))
                    {
                        list.Add(new
                        {
                            action = "updateTrainRole",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            userId = t.UserId,
                            userName = t.RealName,
                            account = t.Account,
                            companyId = departmentBLL.GetEntity(t.OrganizeId).InnerPhone,
                            trainRoles = trainRoleId,
                            actionType = type
                        });
                    }
                }

                //List<object> list = lstUsers.Select(t => new
                //{
                //    action = "edit",
                //    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //    userId = t.UserId,
                //    userName = t.RealName,
                //    account = t.Account,
                //    deptId = string.IsNullOrWhiteSpace(departmentBLL.GetEntity(t.DepartmentId).DeptKey) ? t.DepartmentId : departmentBLL.GetEntity(t.DepartmentId).DeptKey.Split('|')[0],
                //    deptCode = string.IsNullOrWhiteSpace(departmentBLL.GetEntity(t.DepartmentId).DeptKey) ? t.DepartmentCode : departmentBLL.GetEntity(t.DepartmentId).DeptKey.Split('|')[1],
                //    password = pwd,
                //    sex = t.Gender,
                //    idCard = t.IdentifyID,
                //    email = t.Email,
                //    mobile = t.Mobile,
                //    birth = t.Birthday == null ? "" : t.Birthday.Value.ToString("yyyy-MM-dd"),//生日
                //    postId = t.DutyId,
                //    postName = t.DutyName,
                //    age = t.Age.ToIntOrNull(),
                //    native = t.Native,
                //    nation = t.Nation,
                //    jobTitle = t.JobTitle,
                //    techLevel = t.TechnicalGrade,
                //    workType = t.Craft,
                //    companyId = departmentBLL.GetEntity(t.OrganizeId).InnerPhone,
                //    trainRoles = GetRoleId(t.TrainRoleId,roleId,type),
                //    role = t.IsTrainAdmin == null ? 0 : t.IsTrainAdmin //角色（0:学员，1:培训管理员）
                //}).ToList<object>();
                string ModuleName = SystemInfo.CurrentModuleName;
                string ModuleId = SystemInfo.CurrentModuleId;
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
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 同步删除海康人员信息
        /// </summary>
        public void DeleteHikUser(string userid)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            if (!string.IsNullOrEmpty(KMIndex))
            {//只允许可门电厂人员执行该操作

                DataItemDetailBLL data = new DataItemDetailBLL();
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }

                List<TemporaryUserEntity> tempuserList = tempbll.GetUserList();//所有临时人员
                var uentity = tempuserList.Where(t => t.USERID == userid).FirstOrDefault();
                if (uentity != null)
                {
                    //临时表记录
                    tempbll.DeleteTemporaryList("", uentity);

                    if (!string.IsNullOrEmpty(uentity.Postname))
                    {//已授权删除设备中对应的出入权限
                        list.Add(uentity);
                        tempbll.DeleteUserlimits(list, baseurl, Key, Signature);
                    }

                    //海康平台记录
                    string Url = "/artemis/api/resource/v1/person/batch/delete";//接口地址
                    List<string> dellist = new List<string>();
                    dellist.Add(uentity.USERID);
                    var model = new
                    {
                        personIds = dellist
                    };
                    SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                }
            }
        }

        /// <summary>
        ///同步修改华电可门电厂海康平台出入权限（离厂）
        /// </summary>
        /// <param name="leaveTime"></param>
        /// <param name="DepartureReason"></param>
        /// <param name="userId"></param>
        public void SaveForbidden(string leaveTime, string DepartureReason, string userId, int type)
        {
            //说明：【双控离厂、加入黑名单】相当于可门电厂【加入禁入名单】
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            if (!string.IsNullOrEmpty(KMIndex))
            {//只允许可门电厂人员执行该操作
                List<TemporaryUserEntity> tempuserList = new TemporaryGroupsBLL().GetUserList();//所有临时人员
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                foreach (var uid in userId.Split(','))
                {//加入禁入名单
                    var uentity = tempuserList.Where(t => t.USERID == uid).FirstOrDefault();
                    if (uentity != null)
                    {
                        uentity.EndTime = Convert.ToDateTime(leaveTime);
                        uentity.Remark = DepartureReason;
                        list.Add(uentity);
                    }
                }
                if (type == 0)
                {

                    new TemporaryGroupsBLL().SaveForbidden(list);
                }
                else if (type == -1)
                {
                    new TemporaryGroupsBLL().DeleteRightFromDevice(list);
                }
                else
                {//移除禁入名单
                    new TemporaryGroupsBLL().RemoveForbidden(userId);
                }
            }
        }


        /// <summary>
        /// 离场同步
        /// </summary>
        private void SyZg(string leaveTime, string DepartureReason, List<UserInfoEntity> users, Operator curUser)
        {
            string url = new DataItemDetailBLL().GetItemValue("yjbzUrl");
            if (string.IsNullOrWhiteSpace(url))
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                try
                {
                    foreach (var user in users)
                    {
                        BzAppTransfer tr = new BzAppTransfer();
                        tr.Id = "";
                        tr.RoleDutyId = "";
                        tr.RoleDutyName = "";
                        tr.allocationtime = "";
                        tr.department = "";
                        tr.departmentid = "";
                        tr.iscomplete = true;
                        tr.leaveremark = DepartureReason;
                        tr.leavetime = leaveTime;
                        tr.oldRoleDutyName = "";
                        tr.olddepartment = "";
                        tr.olddepartmentid = "";
                        tr.oldquarters = "";
                        tr.quarters = "";
                        tr.quartersid = "";
                        tr.userId = user.UserId;
                        tr.username = user.RealName;


                        BzBase bs = new BzBase();
                        bs.data = tr;
                        bs.userId = curUser.UserId;
                        //如果是待审批状态 则同步到班组那边
                        System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                        nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(bs));
                        wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                        wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("yjbzUrl") + "/UserWorkAllocation/updatetoerchtms"), nc);
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// 工作记录日志
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="currUser"></param>
        private void WorkRecord(List<UserInfoEntity> users, Operator currUser)
        {

            foreach (var user in users)
            {
                new WorkRecordBLL().WriteWorkRecord(user, currUser);
            }
        }

        /// <summary>
        /// 修改人员离场信息
        /// </summary>
        /// <param name="leaveTime">离场时间</param>
        /// <param name="userId">用户Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "人员离场")]
        public ActionResult EditLeave(string leaveTime, [System.Web.Http.FromBody]string userId, [System.Web.Http.FromBody]string DepartureReason)
        {
            try
            {
                if (userBLL.SetLeave(userId, leaveTime, DepartureReason) > 0)
                {
                    new WorkRecordBLL().EditRecord(userId, leaveTime);
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadPhoto()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string FileEextension = Path.GetExtension(files[0].FileName);
            string UserId = OperatorProvider.Provider.Current().UserId;
            string virtualPath = string.Format("/Resource/PhotoFile/{0}{1}", Guid.NewGuid().ToString(), FileEextension);
            string fullFileName = Server.MapPath("~" + virtualPath);
            //创建文件夹，保存文件
            string path = Path.GetDirectoryName(fullFileName);
            Directory.CreateDirectory(path);
            files[0].SaveAs(fullFileName);
            return Success("上传成功。", virtualPath);
        }
        /// <summary>
        /// 人员入场
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="strUserEntity">用户实体json</param>
        /// <param name="deptName">部门名称</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "人员入场")]

        public ActionResult Enter(string keyValue, string strUserEntity, string deptName)
        {
            try
            {
                UserEntity userEntity = strUserEntity.ToObject<UserEntity>();
                userEntity.IsPresence = "1";
                //
                //获取职务Code
                if (userEntity.PostId != null && userEntity.PostId != "")
                {
                    string postcode = "";
                    IEnumerable<RoleEntity> rlist = new JobBLL().GetList();
                    string[] Postids = userEntity.PostId.Split(',');
                    for (int i = 0; i < Postids.Length; i++)
                    {
                        RoleEntity ro = rlist.Where(it => it.RoleId == Postids[i]).FirstOrDefault();
                        if (ro != null)
                        {
                            if (postcode == "")
                            {
                                postcode = ro.EnCode;
                            }
                            else
                            {
                                postcode += "," + ro.EnCode;
                            }
                        }
                    }
                    userEntity.PostCode = postcode;
                }
                string objectId = userBLL.SaveForm(keyValue, userEntity);
                Task.Run(() =>
                {
                    SaveForbidden("", "", keyValue, 1);
                });
                //入场后重置密码为123456
                string Password = Md5Helper.MD5("Abc123456", 32);
                userBLL.RevisePassword(keyValue, Password.ToLower());
                var userInfo = userBLL.GetUserInfoEntity(objectId);
                if (userInfo != null)
                {
                    new WorkRecordBLL().SaveForm("", new WorkRecordEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        OrganizeName = userInfo.OrganizeName,
                        DeptCode = userInfo.DepartmentCode,
                        DeptId = userInfo.DepartmentId,
                        EnterDate = userEntity.EnterTime.Value,
                        JobName = userInfo.PostName,
                        WorkType = 1,
                        UserId = keyValue,
                        UserName = userEntity.RealName,
                        DeptName = userInfo.DeptName,
                        PostName = userEntity.DutyName
                    });
                    DataItemDetailBLL di = new DataItemDetailBLL();
                    if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                    {
                        userEntity.Password = "Abc123456";
                        Task.Run(() =>
                        {
                            SaveUser(userEntity);
                        });
                    }

                    string way = di.GetItemValue("WhatWay");
                    DepartmentEntity org = departmentBLL.GetEntity(userInfo.OrganizeId);
                    if (org.IsTrain == 1)
                    {
                        //对接.net培训平台
                        if (way == "0")
                        {

                        }
                        //对接java培训平台
                        if (way == "1")
                        {
                            DepartmentEntity dept = departmentBLL.GetEntity(userInfo.DepartmentId);
                            if (dept != null)
                            {
                                string deptId = dept.DepartmentId;
                                string enCode = dept.EnCode;
                                if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                {
                                    string[] arr = dept.DeptKey.Split('|');
                                    deptId = arr[0];
                                    if (arr.Length > 1)
                                    {
                                        enCode = arr[1];
                                    }
                                }
                                Task.Run(() =>
                                {
                                    object obj = new
                                    {
                                        action = "edit",
                                        time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        userId = userInfo.UserId,
                                        userName = userInfo.RealName,
                                        account = userInfo.Account,
                                        deptId = deptId,
                                        deptCode = enCode,
                                        password = DBNull.Value, //为null时不要修改密码!
                                        sex = userInfo.Gender,
                                        idCard = userInfo.IdentifyID,
                                        email = userInfo.Email,
                                        mobile = userInfo.Mobile,
                                        birth = userInfo.Birthday == null ? "" : userInfo.Birthday.Value.ToString("yyyy-MM-dd"),//生日
                                        postId = userEntity.DutyId,
                                        postName = userInfo.DutyName,//岗位
                                        age = userEntity.Age,//年龄
                                        native = userInfo.Native, //籍贯
                                        nation = userInfo.Nation, //民族
                                        encode = userInfo.EnCode,//工号
                                        companyId = org.InnerPhone,
                                        jobTitle = userEntity.JobTitle,
                                        techLevel = userEntity.TechnicalGrade,
                                        workType = userEntity.Craft,
                                        trainRoles = userEntity.TrainRoleId,
                                        role = userInfo.IsTrainAdmin == null ? 0 : userInfo.IsTrainAdmin //角色（0:学员，1:培训管理员）
                                    };
                                    List<object> list = new List<object>();
                                    list.Add(obj);
                                    Busines.JPush.JPushApi.PushMessage(list, 1);
                                });
                            }

                        }
                    }
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        private void SaveUser(UserEntity user)
        {
            WebClient wc = new WebClient();
            DataItemDetailBLL dd = new DataItemDetailBLL();
            string imgUrl = dd.GetItemValue("imgUrl");
            string bzAppUrl = dd.GetItemValue("bzAppUrl");
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //当前操作用户账号
                nc.Add("account", ERCHTMS.Code.OperatorProvider.Provider.Current().Account);
                //用户信息
                user.Gender = user.Gender == "男" ? "1" : "0";
                if (user.RoleName.Contains("班组级用户"))
                {
                    if (user.RoleName.Contains("负责人"))
                    {
                        user.RoleId = "a1b68f78-ec97-47e0-b433-2ec4a5368f72";
                        user.RoleName = "班组长";
                    }
                    else
                    {
                        user.RoleId = "e503d929-daa6-472d-bb03-42533a11f9c6";
                        user.RoleName = "班组成员";
                    }
                }
                if (user.RoleName.Contains("部门级用户"))
                {
                    if (user.RoleName.Contains("负责人"))
                    {
                        user.RoleId = "1266af38-9c0a-4eca-a04a-9829bc2ee92d";
                        user.RoleName = "部门管理员";
                    }
                    else
                    {
                        user.RoleId = "3a4b56ac-6207-429d-ac07-28ab49dca4a6";
                        user.RoleName = "部门级用户";
                    }
                }
                if (user.RoleName.Contains("公司级用户"))
                {
                    //if (user.RoleName.Contains("负责人"))
                    //{
                    user.RoleId = "97869267-e5eb-4f20-89bd-61e7202c4ecd";
                    user.RoleName = "厂级管理员";
                    // }

                }
                if (user.EnterTime == null)
                {
                    user.EnterTime = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(user.SignImg))
                    user.SignImg = imgUrl + user.SignImg;

                if (!string.IsNullOrEmpty(user.HeadIcon))
                    user.HeadIcon = imgUrl + user.HeadIcon;

                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                wc.UploadValuesAsync(new Uri(bzAppUrl + "SaveUser?keyValue=" + user.UserId), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            //将同步结果写入日志文件
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
        }
        /// <summary>
        /// 人员加入黑名单
        /// </summary>
        /// <param name="content">原因</param>
        /// <param name="userIds">用户Id,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "人员加入黑名单")]
        public ActionResult Black(string userIds, string content)
        {
            try
            {
                userBLL.SetBlack(userIds, 1);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(7, "修改用户信息")]
        public ActionResult UpdateForm(string keyValue, string strUserEntity)
        {
            UserEntity userEntity = strUserEntity.ToObject<UserEntity>();

            string objectId = userBLL.UpdateUserInfo(keyValue, userEntity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存重置修改密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(7, "修改用户密码信息")]
        public ActionResult SaveRevisePassword(string keyValue, string Password)
        {
            if (keyValue == "System")
            {
                throw new Exception("当前账户不能重置密码");
            }
            userBLL.RevisePassword(keyValue, Password);
            return Success("密码修改成功，请牢记新密码。");
        }
        /// <summary>
        /// 禁用账户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(7, "设置当前用户账户不可用")]
        public ActionResult DisabledAccount(string keyValue)
        {
            if (keyValue == "System")
            {
                throw new Exception("当前账户不禁用");
            }
            userBLL.UpdateState(keyValue, 0);
            return Success("账户禁用成功。");
        }
        /// <summary>
        /// 启用账户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(7, "设置当前用户账户可用")]
        public ActionResult EnabledAccount(string keyValue)
        {
            userBLL.UpdateState(keyValue, 1);
            return Success("账户启用成功。");
        }
        /// <summary>
        /// 批量生成二维码并导出到word
        /// </summary>
        /// <param name="userId">用户Id,多个用逗号分隔</param>
        /// <param name="userName">用户姓名,多个用逗号分隔</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImg(string userId, string userName, string pType = "人员")
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/qrcode")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/qrcode"));
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/二维码打印.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            DataTable dt = new DataTable("U");
            dt.Columns.Add("BigEWM2");
            dt.Columns.Add("PersonName");
            int i = 0;
            string fileName = "";
            userId = userId.Trim(',');
            userName = userName.Trim(',');
            foreach (string code in userId.Split(','))
            {
                DataRow dr = dt.NewRow();
                dr[1] = userName.Split(',')[i];

                fileName = code + ".jpg";
                if (!System.IO.File.Exists(Server.MapPath("~/Resource/qrcode/" + fileName)))
                {
                    Bitmap bmp = qrCodeEncoder.Encode(code + "|" + pType, Encoding.UTF8);
                    bmp.Save(Server.MapPath("~/Resource/qrcode/" + fileName), ImageFormat.Jpeg);
                    bmp.Dispose();
                }
                dr[0] = Server.MapPath("~/Resource/qrcode/" + fileName);
                i++;
                dt.Rows.Add(dr);
            }

            doc.MailMerge.Execute(dt);
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return Success("生成成功", new { fileName = fileName });
        }
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportUser()
        {
            var currUser = OperatorProvider.Provider.Current();
            if (currUser.IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            try
            {
                string orgId = currUser.OrganizeId;//所属公司
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
                    Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    int order = 1;
                    IList<UserEntity> UserList = new List<UserEntity>();
                    //先获取到职务列表
                    IEnumerable<RoleEntity> rlist = new JobBLL().GetList();

                    List<object> lstObjs = new List<object>();
                    DataItemDetailBLL di = new DataItemDetailBLL();
                    string way = di.GetItemValue("WhatWay");

                    int len = 0;
                    string[] arr1 = new string[] { };//普通人员年龄条件
                    string[] arr2 = new string[] { };//特种作业人员年龄条件
                    string[] arr3 = new string[] { };//监理人员年龄条件
                    string[] arr4 = new string[] { };//特种设备作业人员年龄条件
                    int isCL = departmentBLL.GetDataTable(string.Format("select count(1) from BIS_BLACKSET where itemcode='10' and deptcode='{0}' and status=1", currUser.OrganizeCode)).Rows[0][0].ToInt();
                    if (isCL > 0)
                    {
                        DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemvalue,itemcode,status from BIS_BLACKSET t where  deptcode='{0}' and (t.itemcode='01' or t.itemcode='06' or t.itemcode='07' or t.itemcode='08') order by itemcode", currUser.OrganizeCode));
                        len = dtItems.Rows.Count;

                        StringBuilder sb = new StringBuilder();
                        if (len > 0)
                        {
                            if (dtItems.Rows[0][1].ToString() == "01" && dtItems.Rows[0][2].ToString() == "1")
                            {
                                arr1 = dtItems.Rows[0][0].ToString().Split('|');
                            }
                        }
                        if (len > 1)
                        {
                            if (dtItems.Rows[1][1].ToString() == "06" && dtItems.Rows[1][2].ToString() == "1")
                            {
                                arr2 = dtItems.Rows[1][0].ToString().Split('|');
                            }
                        }
                        if (len > 2)
                        {
                            if (dtItems.Rows[2][1].ToString() == "07" && dtItems.Rows[2][2].ToString() == "1")
                            {
                                arr3 = dtItems.Rows[2][0].ToString().Split('|');
                            }
                        }
                        if (len > 3)
                        {
                            if (dtItems.Rows[3][1].ToString() == "08" && dtItems.Rows[3][2].ToString() == "1")
                            {
                                arr4 = dtItems.Rows[3][0].ToString().Split('|');
                            }
                        }
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        order = i;
                        //部门
                        string deptlist = dt.Rows[i]["部门"].ToString();
                        //姓名
                        string fullName = dt.Rows[i]["姓名"].ToString();
                        //性别
                        string sex = dt.Rows[i]["性别"].ToString().Trim();
                        //账号=工号
                        string account = dt.Rows[i]["账号"].ToString().Trim();
                        string deptName = deptlist.Trim();//所属部门
                        string deptId = string.Empty;//所属部门
                        //职务
                        string duty = dt.Rows[i]["职务"].ToString().Trim();
                        string dutyid = "";
                        string dutyName = "";
                        string dutyCode = "";
                        //密码
                        string password = "Abc123456";
                        //政治面貌
                        string politics = dt.Rows[i]["政治面貌"].ToString().Trim();
                        //身份证号
                        string identity = dt.Rows[i]["身份证号"].ToString().Trim();
                        //原始学历
                        string degrees = dt.Rows[i]["原始学历"].ToString();
                        //后期学历
                        string degrees1 = dt.Rows[i]["后期学历"].ToString();
                        //工号
                        string sno = dt.Rows[i]["工号"].ToString().Trim();
                        //岗位
                        string postName = dt.Rows[i]["岗位"].ToString().Trim();
                        string postid = "";
                        //工种
                        string worktype = dt.Rows[i]["工种"].ToString();
                        //手机号
                        string mobile = dt.Rows[i]["手机号码"].ToString().Trim();
                        //籍贯
                        string native = dt.Rows[i]["籍贯"].ToString();
                        //民族
                        string nation = dt.Rows[i]["民族"].ToString();
                        //人员类型
                        string userType = dt.Rows[i]["人员类型"].ToString().Trim();
                        //是否特种作业人员
                        string isTZ = dt.Rows[i]["是否特种作业人员"].ToString().Trim();
                        //是否特种设备作业人员
                        string isTZSB = dt.Rows[i]["是否特种设备作业人员"].ToString().Trim();
                        //是否外包
                        string isOut = dt.Rows[i]["是否外包"].ToString().Trim();
                        //入厂（职）时间
                        string enterTime = dt.Rows[i]["入厂(职)时间"].ToString().Trim();
                        string projectName = dt.Rows[i]["外包工程名称"].ToString().Trim();
                        string projectId = string.Empty;
                        //是否为四种人
                        string isfourperson = dt.Rows[i]["是否为三种人"].ToString();
                        //四种人类别
                        string fourpersontype = dt.Rows[i]["三种人类别"].ToString();
                        //职称
                        string zhicheng = dt.Rows[i]["职称"].ToString();
                        //技术等级
                        string jsdj = dt.Rows[i]["技术等级"].ToString();
                        //邮箱
                        string email = "";
                        //备注
                        string description = "";

                        //DataItemModel di = dic.GetDataItemList("Degrees").Where(a => a.ItemName == degrees).FirstOrDefault();
                        //if (di != null)
                        //{
                        //    degreesid = di.ItemValue;
                        //}
                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(identity) || string.IsNullOrEmpty(postName) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(sex) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(userType) || string.IsNullOrEmpty(isTZ) || string.IsNullOrEmpty(isTZSB) || string.IsNullOrEmpty(isOut))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                            error++;
                            continue;
                        }
                        //验证所填部门是否存在
                        var p1 = string.Empty; var p2 = string.Empty;
                        var deptFlag = false;
                        var array = deptlist.Split('/');
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (j == 0)
                            {
                                if (currUser.RoleName.Contains("省级"))
                                {
                                    var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity1 == null)
                                    {
                                        falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在,未能导入.";
                                        error++;
                                        deptFlag = true;
                                        break;
                                    }
                                    else
                                    {
                                        deptId = entity1.DepartmentId;
                                        p1 = entity1.DepartmentId;
                                    }
                                }
                                else
                                {
                                    var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "部门" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString()).FirstOrDefault();
                                        if (entity == null)
                                        {
                                            entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                            if (entity == null)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 2) + "行部门不存在,未能导入.";
                                                error++;
                                                deptFlag = true;
                                                break;
                                            }
                                            else
                                            {
                                                deptId = entity.DepartmentId;
                                                p1 = entity.DepartmentId;
                                            }
                                        }
                                        else
                                        {
                                            deptId = entity.DepartmentId;
                                            p1 = entity.DepartmentId;
                                        }
                                    }
                                    else
                                    {
                                        deptId = entity.DepartmentId;
                                        p1 = entity.DepartmentId;
                                    }
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
                                        falseMessage += "</br>" + "第" + (i + 2) + "行专业/班组不存在,未能导入.";
                                        error++;
                                        deptFlag = true;
                                        continue;
                                    }
                                    else
                                    {
                                        deptId = entity1.DepartmentId;
                                        p2 = entity1.DepartmentId;
                                    }
                                }
                                else
                                {
                                    deptId = entity1.DepartmentId;
                                    p2 = entity1.DepartmentId;
                                }

                            }
                            else
                            {
                                var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行班组不存在,未能导入.";
                                    error++;
                                    deptFlag = true;
                                    continue;
                                }
                                else
                                {
                                    deptId = entity1.DepartmentId;
                                }
                            }
                        }
                        if (deptFlag)
                        {
                            continue;
                        }
                        //--手机号验证
                        if (!string.IsNullOrEmpty(mobile))
                        {
                            if (!Regex.IsMatch(mobile, @"^(\+\d{2,3}\-)?\d{11}$", RegexOptions.IgnoreCase))
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行手机号格式有误,未能导入.";
                                error++;
                                continue;
                            }
                        }
                        //--邮箱验证
                        if (!string.IsNullOrEmpty(email))
                        {
                            if (!Regex.IsMatch(email, @"^\w{3,}@\w+(\.\w+)+$", RegexOptions.IgnoreCase))
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行邮箱格式有误,未能导入.";
                                error++;
                                continue;
                            }
                        }
                        //验证账号的唯一性
                        if (!userBLL.ExistAccount(account))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行账号已存在,未能导入.";
                            error++;
                            continue;
                        }
                        //检验所填职务是否属于其公司或者部门
                        if (!string.IsNullOrWhiteSpace(duty))
                        {
                            var dutylist = duty.Split('/');
                            if (!string.IsNullOrEmpty(deptId) && deptId != "undefined")
                            {
                                for (int k = 0; k < dutylist.Length; k++)
                                {
                                    RoleEntity re1 = rlist.Where(a => a.FullName == dutylist[k].ToString() && a.OrganizeId == orgId && a.Category == 3).FirstOrDefault();
                                    if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                                    {
                                        re1 = rlist.Where(a => a.FullName == dutylist[k].ToString() && a.OrganizeId == orgId && a.Nature == departmentBLL.GetEntity(deptId).Nature && a.Category == 3).FirstOrDefault();
                                    }
                                    if (re1 == null)
                                    {
                                        falseMessage += "</br>" + "第" + (i + 2) + "行职务有误,未能导入.";
                                        error++;
                                        continue;
                                    }
                                    else
                                    {
                                        if (dutyid == "")
                                        {
                                            dutyid += re1.RoleId;
                                            dutyName += re1.FullName;
                                            dutyCode += re1.EnCode;
                                        }
                                        else
                                        {
                                            dutyid += ',' + re1.RoleId;
                                            dutyName += ',' + re1.FullName;
                                            dutyCode += ',' + re1.EnCode;
                                        }


                                    }
                                    ////所属公司
                                    //RoleEntity data = postCache.GetList(orgId, "true").OrderBy(x => x.SortCode).Where(a => a.FullName == dutylist[i].ToString()).FirstOrDefault();
                                    //if (data == null)
                                    //{
                                    //    falseMessage += "</br>" + "第" + (i + 2) + "行职务不属于该公司,未能导入.";
                                    //    error++;
                                    //    break;
                                    //}
                                }
                            }
                            //Code 根据字符串排序 然后根据排序后的Code位置进行整个排序
                            string[] olddutyids = dutyid.Split(',');
                            string[] dutyids = dutyCode.Split(',');
                            string[] dutyNames = dutyName.Split(',');
                            string[] dutyCodes = dutyCode.Split(',');
                            string newdutyid = "";
                            string newdutyName = "";
                            string newdutyCode = "";
                            Array.Sort(dutyids, new CustomComparer());
                            for (int j = 0; j < dutyids.Length; j++)
                            {
                                for (int k = 0; k < dutyCodes.Length; k++)
                                {
                                    if (dutyids[j] == dutyCodes[k])
                                    {
                                        if (newdutyid == "")
                                        {
                                            newdutyid = olddutyids[k];
                                            newdutyName = dutyNames[k];
                                            newdutyCode = dutyCodes[k];
                                        }
                                        else
                                        {
                                            newdutyid += ',' + olddutyids[k];
                                            newdutyName += ',' + dutyNames[k];
                                            newdutyCode += ',' + dutyCodes[k];
                                        }
                                    }
                                }
                            }

                            dutyid = newdutyid;
                            dutyName = newdutyName;
                            dutyCode = newdutyCode;
                        }

                        //检验所填岗位是否属于其公司或者部门
                        if (string.IsNullOrEmpty(deptId) || deptId == "undefined")
                        {
                            //所属公司
                            RoleEntity data = postCache.GetList(orgId, "true").Where(a => a.FullName == postName || a.IsPublic == 1).FirstOrDefault();
                            if (data == null)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行岗位不属于该公司,未能导入.";
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            //所属部门
                            //所属公司
                            RoleEntity data = postCache.GetList(orgId, deptId).Where(a => a.FullName == postName).FirstOrDefault();
                            if (data == null)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行岗位不属于该部门,未能导入.";
                                error++;
                                continue;
                            }
                        }
                        //--**验证岗位是否存在**--


                        RoleEntity re = postBLL.GetList().Where(a => (a.FullName == postName && a.OrganizeId == orgId)).FirstOrDefault();
                        if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                        {
                            re = postBLL.GetList().Where(a => (a.FullName == postName && a.OrganizeId == orgId && a.DeptId == deptId)).FirstOrDefault();
                            if (re == null)
                            {
                                re = postBLL.GetList().Where(a => (
                                    a.FullName == postName && a.OrganizeId == orgId &&
                                    a.Nature == departmentBLL.GetEntity(deptId).Nature)).FirstOrDefault();
                            }
                        }
                        if (re == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行岗位有误,未能导入.";
                            error++;
                            continue;
                        }
                        else
                        {
                            postid = re.RoleId;
                        }
                        //角色
                        //--**根据选择的岗位来默认角色信息

                        string roleName = "";
                        string roleId = "";
                        //如果选择的是厂级部门的话，角色会默认追加“厂级部门用户”
                        if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                        {
                            if (departmentBLL.GetEntity(deptId).IsOrg == 1)
                            {
                                roleName += "厂级部门用户,";
                                RoleEntity cj = roleBLL.GetList().Where(a => a.FullName == "厂级部门用户").FirstOrDefault();
                                if (cj != null)
                                    roleId += cj.RoleId + ",";
                            }
                        }
                        IEnumerable<RoleEntity> ro = postBLL.GetList().Where(a => a.RoleId == postid);
                        ////根据选择的岗位来追加角色（这里还要加上所选部门的层级）
                        //if (!(string.IsNullOrEmpty(DepartmentId) || DepartmentId == "undefined"))
                        //{
                        //    ro = ro.Where(a => a.Nature == departmentBLL.GetEntity(DepartmentId).Nature);
                        //}
                        RoleEntity roleentity = ro.FirstOrDefault();
                        if (roleentity != null)
                        {
                            roleName += roleentity.RoleNames;
                            roleId += roleentity.RoleIds;
                        }

                        //---****身份证正确验证*****--
                        if (!Regex.IsMatch(identity, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行身份证号格式有误,未能导入.";
                            error++;
                            continue;
                        }

                        //---****身份证重复验证*****--
                        if (!userBLL.ExistIdentifyID(identity, ""))
                        {
                            string res = userBLL.IsBalckUser("", identity);
                            if (!string.IsNullOrWhiteSpace(res))
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行,该用户于时间" + res + "被加入黑名单！";
                                error++;
                                continue;
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行身份证号已存在,未能导入.";
                                error++;
                                continue;
                            }
                        }
                        UserEntity ue = new UserEntity();
                        ue.UserId = Guid.NewGuid().ToString();
                        ue.Account = account.Trim();
                        ue.Password = password;
                        ue.RealName = fullName;
                        ue.Gender = sex;

                        string idcardError = "";
                        string birthday = "";
                        if (CheckIdCard(identity, out idcardError, out birthday))
                        {
                            ue.Birthday = DateTime.Parse(birthday);
                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行" + idcardError + ",未能导入.";
                            error++;
                            continue;
                        }
                        ue.Age = (DateTime.Now.Year - ue.Birthday.Value.Year).ToString();
                        ue.Political = politics;
                        ue.Mobile = mobile;
                        ue.OrganizeId = orgId;
                        if (isCL > 0 && len > 0)
                        {
                            //判断是否超龄人员
                            if (userType == "监理人员" && arr3.Length > 0)
                            {
                                if (sex == "男" && (ue.Age.ToInt() < arr3[0].ToInt() || ue.Age.ToInt() > arr3[1].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                                if (sex == "女" && (ue.Age.ToInt() < arr3[2].ToInt() || ue.Age.ToInt() > arr3[3].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                            }
                            if (isTZ == "是" && arr2.Length > 0)
                            {
                                if (sex == "男" && (ue.Age.ToInt() < arr2[0].ToInt() || ue.Age.ToInt() > arr2[1].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                                if (sex == "女" && (ue.Age.ToInt() < arr2[2].ToInt() || ue.Age.ToInt() > arr2[3].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                            }
                            if (isTZSB == "是" && arr4.Length > 0)
                            {
                                if (sex == "男" && (ue.Age.ToInt() < arr4[0].ToInt() || ue.Age.ToInt() > arr4[1].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                                if (sex == "女" && (ue.Age.ToInt() < arr4[2].ToInt() || ue.Age.ToInt() > arr4[3].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                            }
                            if (isTZSB == "否" && isTZ == "否" && userType != "监理人员" && arr1.Length > 0)
                            {
                                if (sex == "男" && (ue.Age.ToInt() < arr1[0].ToInt() || ue.Age.ToInt() > arr1[1].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                                if (sex == "女" && (ue.Age.ToInt() < arr1[2].ToInt() || ue.Age.ToInt() > arr1[3].ToInt()))
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行人员年龄超龄,未能导入.";
                                    error++;
                                    continue;
                                }
                            }
                        }

                        if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                        {
                            ue.DepartmentId = deptId;
                            ue.DepartmentCode = departmentBLL.GetEntity(deptId).EnCode;
                        }
                        else
                        {
                            ue.DepartmentCode = organizeCache.GetEntity(orgId).EnCode;
                        }
                        ue.EnCode = sno;
                        ue.LateDegrees = ue.LateDegreesID = degrees1;
                        ue.JobTitle = zhicheng;
                        ue.TechnicalGrade = jsdj;
                        ue.Craft = worktype;
                        ue.IsEpiboly = "0";
                        ue.IsPresence = "1";
                        ue.RoleId = roleId;
                        ue.RoleName = roleName;
                        ue.DutyId = postid;
                        ue.DutyName = postName;
                        ue.PostName = dutyName;
                        ue.PostId = dutyid;
                        ue.PostCode = dutyCode;
                        ue.DeleteMark = 0;
                        ue.EnabledMark = 1;
                        ue.IdentifyID = identity;
                        ue.Degrees = ue.DegreesID = degrees;
                        ue.Email = email;
                        ue.EnabledMark = 0;
                        ue.OrganizeCode = currUser.OrganizeCode;
                        ue.Description = description;
                        ue.IsSpecial = isTZ;
                        ue.IsSpecialEqu = isTZSB;
                        ue.Nation = nation;
                        ue.Native = native;
                        ue.IsEpiboly = isOut == "是" ? "1" : "0";
                        ue.UserType = userType;
                        ue.IsPresence = "1";
                        ue.ISFOURPERSON = isfourperson;
                        ue.FOURPERSONTYPE = fourpersontype;
                        if (isOut == "是")
                        {
                            var wblist = departmentBLL.GetList().Where(x => x.SendDeptID == deptId).ToList();
                            for (int k = 0; k < wblist.Count; k++)
                            {
                                var p = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == wblist[i].DepartmentId && x.OUTPROJECTNAME == projectName).FirstOrDefault();
                                if (p != null)
                                {
                                    projectId = p.ID;
                                }
                            }
                            if (!string.IsNullOrEmpty(projectId))
                            {
                                ue.ProjectId = projectId;
                            }
                        }
                        else
                        {
                            ue.ProjectId = projectId;
                        }

                        //if (!string.IsNullOrEmpty(leaveTime))
                        //{
                        //    ue.DepartureTime = DateTime.Parse(leaveTime);
                        //}
                        // if (isIn == "是")
                        //{
                        try
                        {
                            if (!string.IsNullOrEmpty(enterTime))
                            {
                                ue.EnterTime = DateTime.Parse(enterTime);
                            }
                            else
                            {
                                ue.EnterTime = DateTime.Now;
                            }
                        }
                        catch (Exception)
                        {

                            ue.EnterTime = DateTime.Now;
                        }
                        //}
                        //else
                        //{
                        //    try
                        //    {
                        //        if (!string.IsNullOrEmpty(leaveTime))
                        //        {
                        //            ue.DepartureTime = DateTime.Parse(leaveTime);
                        //        }
                        //        else
                        //        {
                        //            ue.DepartureTime = DateTime.Now;
                        //        }
                        //    }
                        //    catch (Exception)
                        //    {

                        //        ue.EnterTime = DateTime.Now;
                        //    }
                        //}
                        try
                        {
                            UserInfoEntity user = userBLL.GetUserInfoByAccount(ue.Account);
                            string action = user == null ? "add" : "edit";

                            ue.UserId = userBLL.SaveForm("", ue);
                            ue.Password = password;
                            if (!string.IsNullOrWhiteSpace(ue.UserId))
                            {
                                UserList.Add(ue);

                                DepartmentEntity org = departmentBLL.GetEntity(orgId);
                                if (org.IsTrain == 1)
                                {
                                    //对接.net培训平台
                                    if (way == "0")
                                    {

                                    }
                                    //对接java培训平台
                                    if (way == "1")
                                    {


                                        DepartmentEntity dept = departmentBLL.GetEntity(ue.DepartmentId);
                                        if (dept != null)
                                        {
                                            deptId = ue.DepartmentId;
                                            string enCode = ue.DepartmentCode;
                                            if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                            {
                                                string[] arr = dept.DeptKey.Split('|');
                                                deptId = arr[0];
                                                if (arr.Length > 1)
                                                {
                                                    enCode = arr[1];
                                                }
                                            }
                                            object obj = new
                                            {
                                                action = action,
                                                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                userId = ue.UserId,
                                                userName = ue.RealName,
                                                password = password,
                                                account = ue.Account,
                                                deptId = deptId,
                                                deptCode = enCode,
                                                sex = ue.Gender,
                                                idCard = ue.IdentifyID,
                                                email = ue.Email,
                                                mobile = ue.Mobile,
                                                birth = ue.Birthday,//生日
                                                postId = ue.DutyId,
                                                postName = ue.DutyName,//岗位
                                                age = ue.Age,//年龄
                                                native = ue.Native, //籍贯
                                                nation = ue.Nation, //民族
                                                encode = ue.EnCode,//工号
                                                jobTitle = ue.JobTitle,
                                                techLevel = ue.TechnicalGrade,
                                                workType = ue.Craft,
                                                companyId = org.InnerPhone,
                                                trainRoles = ue.TrainRoleId,
                                                role = 0//角色（0:学员，1:培训管理员）
                                            };
                                            lstObjs.Add(obj);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行账号或手机号或工号在系统中已存在,未能导入.";
                                error++;
                                continue;
                            }

                        }
                        catch
                        {
                            error++;
                            continue;
                        }
                    }
                    if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                    {
                        userBLL.SyncUsersToBZ(UserList);
                        //ImportUser(UserList);
                    }
                    if (way == "1" && lstObjs.Count > 0)
                    {
                        if (lstObjs.Count > 100)
                        {
                            int page = 0;
                            int total = lstObjs.Count;
                            if (total % 100 == 0)
                            {
                                page = total / 100;
                            }
                            else
                            {
                                page = total / 100 + 1;
                            }
                            for (int j = 0; j < page; j++)
                            {
                                Busines.JPush.JPushApi.PushMessage(lstObjs.Skip(j * 100).Take(100), 1);
                            }
                        }
                        else
                        {
                            Busines.JPush.JPushApi.PushMessage(lstObjs, 1);
                        }
                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 5;
                        logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                        logEntity.OperateType = "导入用户";
                        logEntity.OperateAccount = currUser.Account + "（" + currUser.UserName + "）";
                        logEntity.OperateUserId = currUser.UserId;

                        logEntity.ExecuteResult = 1;
                        logEntity.ExecuteResultJson = string.Format("同步用户(导入)到java培训平台,同步信息:\r\n{0}", lstObjs.ToJson());
                        logEntity.Module = "人员档案";
                        logEntity.WriteLog();
                    }
                    count = dt.Rows.Count;
                    Task.Run(() =>
                    {
                        ImportUserHik(UserList);
                    });
                    //毕节新增
                    UpdateHdgzUser(UserList.ToList());
                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }

                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public bool CheckIdCard(string IdCard, out string error, out string sbirthday)
        {
            //var aCity ={ 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" };
            List<int> aCity = new List<int>() { 11, 12, 13, 14, 15, 21, 22, 23, 31, 32, 33, 34, 35, 36, 37, 41, 42, 43, 44, 45, 46, 50, 51, 52, 53, 54, 61, 62, 63, 64, 65, 71, 81, 82, 91 };
            error = "";
            sbirthday = "";
            if (!Regex.IsMatch(IdCard, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
            {
                error = "你输入的身份证长度或格式错误";
                return false;
            }
            //IdCard = IdCard.replace(/x$/i, "a");
            if (!aCity.Contains(Convert.ToInt32(IdCard.Substring(0, 2))))
            {
                error = "你的身份证地区非法";
                return false;
            }
            var sBirthday = IdCard.Substring(6, 4) + "-" + IdCard.Substring(10, 2) + "-" + IdCard.Substring(12, 2);
            try
            {

                if (!string.IsNullOrEmpty(sBirthday))
                {
                    DateTime r = new DateTime();
                    if (DateTime.TryParse(sBirthday, out r))
                    {
                        sbirthday = r.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        error = "身份证上的出生日期非法";
                        return false;
                    }

                }
            }
            catch
            {
                error = "身份证号有误";
                return false;
            }
            //for (var i = 17; i >= 0; i--) {
            //    iSum += (Math.Pow(2, i) % 11) * Convert.ToInt32(IdCard.ToArray()[17 - i].ToInt().ToString(), 11);
            //}
            //if (iSum % 11 != 1) {
            //    error = "你输入的身份证号非法";
            //    return false;
            //}

            return true;
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
                nc.Add("account", ERCHTMS.Code.OperatorProvider.Provider.Current().Account);
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
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(userList) + ",异常信息：" + ex.Message + "\r\n");
            }
        }


        /// <summary>
        /// 同步更新毕节人员信息 (单条/批量)
        /// </summary>
        public void UpdateHdgzUser(List<UserEntity> uentityList)
        {
            try
            {
                string isGZBJ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                if (string.IsNullOrWhiteSpace(isGZBJ))
                {
                    return;
                }
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                DataItemDetailBLL data = new DataItemDetailBLL();
                var Key = data.GetItemValue("Hdgzappkey");//毕节URL密钥
                var baseurl = data.GetItemValue("HdgzBaseUrl");//毕节API服务器地址

                string Url = "/api/v2/employee/update/";//接口地址
                List<HdgzUserEntity> hdgzUserEntityList = new List<HdgzUserEntity>();
                foreach (var uentity in uentityList)
                {
                    //var model = new
                    //{
                    //    pin = uentity.IdentifyID,
                    //    name = uentity.RealName,
                    //    deptnumber = "1"
                    //};
                    HdgzUserEntity hdgzUserEntity = new HdgzUserEntity();
                    hdgzUserEntity.pin = uentity.IdentifyID;
                    hdgzUserEntity.name = uentity.RealName;
                    hdgzUserEntity.deptnumber = "1";
                    hdgzUserEntityList.Add(hdgzUserEntity);
                }
                SocketHelper.LoadHdgzCameraList(hdgzUserEntityList, baseurl, Url, Key);
            }
            catch { }
        }
        /// <summary>
        /// 删除毕节人员信息
        /// </summary>
        public void DeleteHdgzUser(UserEntity uentity)
        {
            try
            {
                string isGZBJ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                if (string.IsNullOrWhiteSpace(isGZBJ))
                {
                    return;
                }
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                DataItemDetailBLL data = new DataItemDetailBLL();
                var Key = data.GetItemValue("Hdgzappkey");//毕节URL密钥
                var baseurl = data.GetItemValue("HdgzBaseUrl");//毕节API服务器地址

                string Url = "/api/v2/employee/delete/";//接口地址
                var model = new
                {
                    pin = uentity.IdentifyID
                };
                SocketHelper.LoadHdgzCameraList(model, baseurl, Url, Key);
            }
            catch { }
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出用户数据")]
        public ActionResult ExportUserList(string condition, string queryJson)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(OperatorProvider.Provider.Current().OrganizeId);
            string score = entity == null ? "100" : entity.ItemValue;
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "u.createdate";
            pagination.sord = "desc";
            pagination.rows = 100000000;
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "REALNAME,GENDER,identifyid,to_char(birthday,'yyyy-mm-dd') birthday,encode,Craft,MOBILE,native,nation,political,degreesid,latedegrees,DEPTNAME,dutyNAME,usertype,technicalgrade,jobtitle,isspecial,isspecialequ,isfourperson,fourpersontype,isepiboly,to_char(entertime,'yyyy-mm-dd') entertime,(nvl(score,0)+" + score + ") as score";
            pagination.p_tablename = "v_userinfo u left join (select a.userid,nvl(sum(score),0) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + DateTime.Now.Year + "' group by a.userid) t on u.userid=t.userid";
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
            pagination.conditionJson = string.IsNullOrEmpty(where) ? "Account!='System'" : where;
            pagination.conditionJson += " and isPresence='是'";
            var data = userBLL.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "人员档案";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "人员档案.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "姓名", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "性别", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identifyid", ExcelColumn = "身份证号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "birthday", ExcelColumn = "出生日期", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "encode", ExcelColumn = "人员编号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "craft", ExcelColumn = "工种", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "手机", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "native", ExcelColumn = "籍贯", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nation", ExcelColumn = "民族", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "political", ExcelColumn = "政治面貌", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "degreesid", ExcelColumn = "原始学历", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "latedegrees", ExcelColumn = "后期学历", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "单位/部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dutyname", ExcelColumn = "岗位", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "usertype", ExcelColumn = "人员类型", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "technicalgrade", ExcelColumn = "技术等级", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "jobtitle", ExcelColumn = "职称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isspecial", ExcelColumn = "是否为特种作业人员", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isspecialequ", ExcelColumn = "是否为特种设备作业人员", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isfourperson", ExcelColumn = "是否为三种人", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fourpersontype", ExcelColumn = "三种人类别", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isepiboly", ExcelColumn = "是否外包", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ispresence", ExcelColumn = "是否在厂(职)", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "entertime", ExcelColumn = "入厂(职)时间", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "score", ExcelColumn = "积分" });




            //调用导出方法
            // ExcelHelper.ExcelDownload(data, excelconfig);

            ExcelHelper.ExportByAspose(data, "人员档案", excelconfig.ColumnEntity);
            return Success("导出成功。");
        }

        /// <summary>
        /// 导出人员证书列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出用户证书数据")]
        public ActionResult ExportUserCertList(string condition, string queryJson, int mode = 0)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "";
                pagination.p_fields = "a.realname,a.Gender,a.deptname,certname,certnum,senddate,cast(years as varchar(16)) as years,enddate,sendorgan";
                pagination.p_tablename = "v_userinfo a left join (select t.userid,certname,Gender,certnum,senddate,sendorgan,years,realname,deptname,startdate,enddate,applydate,worktype,workitem from BIS_CERTIFICATE t left join v_userinfo u on t.userid=u.userid";
                pagination.conditionJson = "1=1 ";
                pagination.sidx = "realname,deptname";
                pagination.sord = "desc";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string title = "特种设备作业人员清单";
                if (mode == 1)
                {
                    pagination.p_fields = "a.realname,a.Gender,a.deptname,certname,certnum,senddate,cast(years as varchar(16)) as years,startdate,enddate,applydate，worktype,workitem,sendorgan";
                    title = "特种作业操作人员清单";
                    pagination.conditionJson += " and ISSPECIAL='是'";
                    pagination.p_tablename += " where  ISSPECIAL='是'";
                }
                if (mode == 2)
                {
                    pagination.p_fields = "a.realname,a.Gender,a.mobile,a.deptname,certname,certnum,senddate,cast(years as varchar(16)) as years,enddate,worktype,workitem,sendorgan";
                    pagination.p_tablename += " where  ISSPECIALEQU='是'";
                    pagination.conditionJson += " and ISSPECIALEQU='是'";
                }
                pagination.p_tablename += ") b on a.userid=b.userid";
                var queryParam = queryJson.ToJObject();
                //是否在场参数
                if (!queryParam["ispresence"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and ispresence  = '{0}'", queryParam["ispresence"].ToString());
                }
                //是否当前电厂数据
                if (!queryParam["isself"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and organizeid  = '{0}'", user.OrganizeId);
                }

                if (!user.IsSystem)
                {
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                var data = userBLL.GetPageList(pagination, queryJson);

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = title;
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = title + ".xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "姓名", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "性别", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "单位/部门", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "certname", ExcelColumn = "证书名称", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "certnum", ExcelColumn = "证书编号", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "senddate", ExcelColumn = "初领日期", Alignment = "center" });
                if (mode == 1)
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "years", ExcelColumn = "有效期限(年)", Alignment = "center", Width = 10 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "startdate", ExcelColumn = "有效期开始日期", Alignment = "center" });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "enddate", ExcelColumn = "有效期结束日期", Alignment = "center" });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applydate", ExcelColumn = "应复审日期", Alignment = "center" });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktype", ExcelColumn = "作业类别", Alignment = "center" });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workitem", ExcelColumn = "操作项目", Alignment = "center" });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "years", ExcelColumn = "复审周期(年)", Alignment = "center", Width = 10 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "enddate", ExcelColumn = "有效期限", Alignment = "center" });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktype", ExcelColumn = "种类", Alignment = "center" });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workitem", ExcelColumn = "作业项目", Alignment = "center" });
                }
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sendorgan", ExcelColumn = "发证机关", Alignment = "center" });
                //调用导出方法
                ExcelHelper.ExcelDownload(data, excelconfig);
                return Success("导出成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 人员档案
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportUserInfo(string userId, string score = "")
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1;
                pagination.p_kid = "userid";
                pagination.p_fields = "realname,gender,encode,nation,deptname,native,dutyname postname,postname dutyname,degreesid,usertype,to_char(birthday,'yyyy-mm-dd') birthday,identifyid,isspecial,isspecialequ,isepiboly,ispresence,mobile,to_char(departuretime,'yyyy-mm-dd') as departuretime,headicon,'" + score + "' as score,isfourperson,fourpersontype,political,age,craft,technicalgrade,craftage,jobtitle,latedegrees,healthstatus,telephone,to_char(entertime,'yyyy-mm-dd') as entertime,projectid,organizename";
                pagination.p_tablename = "v_userinfo u";
                pagination.conditionJson = "userid='" + userId + "'";
                pagination.sidx = "realname";
                pagination.sord = "desc";
                DataTable dtUser = userBLL.GetPageList(pagination, "{}");//人员基本信息
                if (dtUser.Rows[0]["headicon"] != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~" + dtUser.Rows[0]["headicon"].ToString())))
                    {
                        dtUser.Rows[0]["headicon"] = Server.MapPath("~" + dtUser.Rows[0]["headicon"].ToString());

                        string fileName = Server.MapPath("~/Resource/Temp/" + System.IO.Path.GetFileName(dtUser.Rows[0]["headicon"].ToString()));
                        if (GetPicThumbnail(dtUser.Rows[0]["headicon"].ToString(), fileName, 150, 120, 20))
                        {
                            dtUser.Rows[0]["headicon"] = fileName;
                        }
                    }
                    else
                    {
                        dtUser.Rows[0]["headicon"] = Server.MapPath("~/content/Images/no.png");
                    }
                }
                if (dtUser.Rows[0]["projectid"] != null)
                {
                    var pro = new ERCHTMS.Busines.OutsourcingProject.OutsouringengineerBLL().GetEntity(dtUser.Rows[0]["projectid"].ToString());
                    if (pro != null)
                    {
                        dtUser.Rows[0]["projectid"] = pro.ENGINEERNAME;
                    }
                }
                if (dtUser.Rows[0]["userid"] != null)
                {

                    if (!Directory.Exists(Server.MapPath("~/Resource/Upfile")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Resource/Upfile"));
                    }

                    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                    qrCodeEncoder.QRCodeVersion = 10;
                    qrCodeEncoder.QRCodeScale = 2;
                    qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
                    Bitmap bmp = qrCodeEncoder.Encode(dtUser.Rows[0]["userid"] + "|人员", Encoding.UTF8);
                    bmp.Save(Server.MapPath("~/Resource/Upfile/Code.jpg"), ImageFormat.Jpeg);
                    bmp.Dispose();


                    dtUser.Rows[0]["userid"] = Server.MapPath("~/Resource/Upfile/Code.jpg");

                }

                dtUser.TableName = "U";
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/人员档案_导出模板.doc"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
                doc.MailMerge.Execute(dtUser);

                DataTable dtCert = departmentBLL.GetDataTable("select id,certname,certnum,to_char(senddate,'yyyy-mm-dd') senddate,years,to_char(enddate,'yyyy-mm-dd') enddate,SendOrgan from BIS_CERTIFICATE where userid='" + userId + "'");//人员证书信息
                FileInfoBLL fileBll = new FileInfoBLL();
                List<string> listImages = new List<string>();
                foreach (DataRow dr in dtCert.Rows)
                {
                    DataTable dtFiles = fileBll.GetFiles(dr["id"].ToString());
                    foreach (DataRow dr1 in dtFiles.Rows)
                    {
                        if (dr1["filepath"] != null)
                        {
                            string path = Server.MapPath(dr1["filepath"].ToString());
                            if (System.IO.File.Exists(path))
                            {
                                //string fileName = Server.MapPath("~/Resource/Temp/" + System.IO.Path.GetFileName(path));
                                //if (GetPicThumbnail(path, fileName, 150, 150, 20))
                                //{
                                listImages.Add(path);
                                //}
                            }
                        }
                    }
                }
                dtCert.TableName = "C";
                doc.MailMerge.ExecuteWithRegions(dtCert);


                if (doc.Range.Bookmarks["images"] != null)
                {
                    db.MoveToBookmark("images");
                    foreach (string imgPath in listImages)
                    {
                        db.InsertImage(imgPath, 290, 220);
                    }
                }
                dtCert = new DesktopBLL().GetWZInfo(userId, 2);//人员违章信息
                dtCert.TableName = "D";

                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                foreach (DataRow dr in dtCert.Rows)
                {
                    if (dr["filepath"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath(dr["filepath"].ToString())))
                        {
                            dr["filepath"] = Server.MapPath(dr["filepath"].ToString());

                            string fileName = Server.MapPath("~/Resource/Temp/" + System.IO.Path.GetFileName(dr["filepath"].ToString()));
                            if (GetPicThumbnail(dr["filepath"].ToString(), fileName, 150, 120, 20))
                            {
                                dr["filepath"] = fileName;
                            }
                        }
                        else
                        {
                            dr["filepath"] = Server.MapPath("~/content/Images/no.png");
                        }
                    }
                    dr["lllegaltype"] = itemBll.GetEntity(dr["lllegaltype"].ToString()).ItemName;
                }
                doc.MailMerge.ExecuteWithRegions(dtCert);
                DataTable dtHeal = new OccupationalstaffdetailBLL().GetUserTable(userId);
                dtHeal.TableName = "F";
                doc.MailMerge.ExecuteWithRegions(dtHeal);
                doc.MailMerge.DeleteFields();
                string filePath = Server.MapPath("~/Resource/temp/" + userId + ".doc");
                doc.Save(filePath);

                string url = "../../Utility/DownloadFile?filePath=~/Resource/temp/" + userId + ".doc&speed=102400&newFileName=" + dtUser.Rows[0]["realname"].ToString() + ".doc";
                return Redirect(url);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion

        #region 人员统计
        /// <summary>
        /// 人员统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStatInfo(string keyword)
        {
            List<List<string>> list = new List<List<string>>();
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                return ToJsonResult(list);
            }
            if (user.RoleName.Contains("省级"))
            {
                string newCode = new DepartmentBLL().GetUserCompany(user).DeptCode;
                DataTable dtDepts = new DepartmentBLL().GetAllFactory(user);
                DataRow drNew = dtDepts.NewRow();
                drNew[0] = user.OrganizeCode;
                drNew[1] = "全部";
                drNew[2] = "2";
                drNew[3] = newCode;
                dtDepts.Rows.InsertAt(drNew, 0);

                drNew = dtDepts.NewRow();
                drNew[0] = user.OrganizeCode;
                drNew[1] = user.OrganizeName;
                drNew[2] = "1";
                drNew[3] = "";
                dtDepts.Rows.InsertAt(drNew, 1);

                drNew = dtDepts.NewRow();
                drNew[0] = user.OrganizeCode;
                drNew[1] = "&nbsp;&nbsp;&nbsp;&nbsp;各电厂";
                drNew[2] = "3";
                drNew[3] = newCode;
                dtDepts.Rows.InsertAt(drNew, 2);

                foreach (DataRow dr in dtDepts.Rows)
                {
                    string prefix = "";

                    if (dr[2].ToString() == "1")
                    {
                        prefix = "&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                    if (dr[2].ToString() == "0")
                    {
                        prefix = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                    List<string> item = userBLL.GetStatByDeptCodeForGroup(dr[0].ToString(), dr[3].ToString(), dr[2].ToString());
                    item.Insert(0, prefix + dr[1].ToString());
                    item.Add(dr[0].ToString());
                    item.Add(dr[2].ToString());
                    list.Add(item);
                }
            }
            else
            {
                DataTable dtDepts = new DepartmentBLL().GetContractDepts(user.OrganizeCode);
                DataRow drNew = dtDepts.NewRow();
                drNew[0] = user.OrganizeCode;
                drNew[1] = "全部";
                drNew[2] = "2";
                dtDepts.Rows.InsertAt(drNew, 0);

                drNew = dtDepts.NewRow();
                drNew[0] = user.OrganizeCode;
                drNew[1] = user.OrganizeName;
                drNew[2] = "1";
                dtDepts.Rows.InsertAt(drNew, 1);

                foreach (DataRow dr in dtDepts.Rows)
                {
                    string prefix = "";
                    for (int j = 3; j <= dr[0].ToString().Length; j += 3)
                    {
                        prefix += "&nbsp;&nbsp;";
                    }

                    List<string> item = userBLL.GetStatByDeptCode(dr[0].ToString(), dr[2].ToString());
                    item.Insert(0, prefix + dr[1].ToString());
                    item.Add(dr[0].ToString());
                    item.Add(dr[2].ToString());
                    list.Add(item);
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region 数据导出
        /// <summary>
        /// 离场人员清单
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "离厂人员清单")]
        public ActionResult ExportLevelList(string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "REALNAME,GENDER,identifyid,mobile,organizename,deptname,dutyname,usertype,departuretime";
            pagination.p_tablename = "v_userinfo u left join (select a.userid,sum(score) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + DateTime.Now.Year + "' group by a.userid) t on u.userid=t.userid";
            pagination.conditionJson = "Account!='System'";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                var queryParam = queryJson.ToJObject();
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where) && (queryParam["code"].IsEmpty() || !queryJson.Contains("code")))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = userBLL.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "离厂人员清单";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "离厂人员清单.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "REALNAME".ToLower(), ExcelColumn = "姓名", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "GENDER".ToLower(), ExcelColumn = "性别", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "identifyid".ToLower(), ExcelColumn = "身份证号", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "mobile".ToLower(), ExcelColumn = "手机", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "organizename".ToLower(), ExcelColumn = "公司", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "部门", Alignment = "Center" });

            listColumnEntity.Add(new ColumnEntity() { Column = "dutyname".ToLower(), ExcelColumn = "岗位", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "usertype".ToLower(), ExcelColumn = "人员类型", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "departuretime".ToLower(), ExcelColumn = "离厂时间", Alignment = "Center" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion

        /// 无损压缩图片  
        /// <param name="sFile">原图片</param>  
        /// <param name="dFile">压缩后保存位置</param>  
        /// <param name="dHeight">高度</param>  
        /// <param name="dWidth"></param>  
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>  
        /// <returns></returns>  

        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }


        public ActionResult GetPerformanceList(string year, string userid)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
                string data = "?userid=" + userid + "&year=" + year;
                string url = new DataItemDetailBLL().GetItemValue("bzAppUrl") + "/GetPerformanceList" + data;
                if (Debugger.IsAttached)
                {
                    //调试代码
                    data = "?userid=755c4c22-32c8-44fe-a83e-b0ec236b7428&year=2019";
                    //data = Newtonsoft.Json.JsonConvert.SerializeObject(new { userid = "755c4c22-32c8-44fe-a83e-b0ec236b7428", year = 2019 });
                    url = "http://localhost:10037/api/SyncData/GetPerformanceList" + data;
                }
                string content = wc.UploadString(new Uri(url), "POST");
                return Content(content);
            }
            catch (Exception ex)
            {
                Logger.Error(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员档案获取绩效信息失败：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                //将同步结果写入日志文件
                //string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                //System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：获取班组待办事项异常：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex) + "\r\n");
                return Content(ex.Message);
            }
        }


        #region 海康平台数据同步
        /// <summary>
        /// 人员批量导入录入海康平台
        /// </summary>
        /// <param name="ulist"></param>
        public void ImportUserHik(IList<UserEntity> ulist)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            // List<UserEntity> list = new List<UserEntity>();
            if (!string.IsNullOrEmpty(KMIndex))
            {//只允许可门电厂人员执行该操作
                List<TemporaryUserEntity> tempuserList = new TemporaryGroupsBLL().GetUserList();//所有临时人员
                List<TemporaryUserEntity> insertTemp = new List<TemporaryUserEntity>();
                List<DepartmentEntity> deptList = departmentBLL.GetList().ToList();
                foreach (var Us in ulist)
                {
                    #region 电厂人员同步到临时表中
                    var uentity = tempuserList.Where(t => t.USERID == Us.UserId).FirstOrDefault();
                    if (uentity == null)
                    {
                        //如果不存在于临时列表则新增一条数据
                        TemporaryUserEntity inserttuser = new TemporaryUserEntity();
                        inserttuser.Tel = Us.Account;
                        inserttuser.ComName = "";
                        inserttuser.CreateDate = Us.CreateDate;
                        inserttuser.CreateUserId = Us.CreateUserId;
                        inserttuser.USERID = Us.UserId;
                        inserttuser.Gender = Us.Gender;
                        inserttuser.ISDebar = 0;
                        inserttuser.Istemporary = 0;
                        inserttuser.Identifyid = Us.IdentifyID;
                        inserttuser.Postname = Us.DutyName;
                        inserttuser.UserName = Us.RealName;
                        inserttuser.Groupsid = Us.DepartmentId;
                        inserttuser.startTime = Us.CreateDate;
                        var dept1 = deptList.Where(it => it.DepartmentId == Us.DepartmentId).FirstOrDefault();
                        if (dept1 != null)
                        {
                            inserttuser.GroupsName = dept1.FullName;
                        }
                        insertTemp.Add(inserttuser);
                        AddSingleHikUser(Us);
                    }
                    #endregion
                }
                new TemporaryGroupsBLL().SaveTemporaryList("", insertTemp);
            }
        }

        /// <summary>
        /// 单条数据同步
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string AddSingleHikUser(UserEntity item)
        {
            DataItemDetailBLL data = new DataItemDetailBLL();
            var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/single/add";//接口地址
            int Gender = 0;
            if (item.Gender == "男")
            {
                Gender = 1;
            }
            else
            {
                Gender = 2;
            }

            var model = new
            {
                personId = item.UserId,
                personName = item.RealName,
                orgIndexCode = item.DepartmentId,
                gender = Gender,
                phoneNo = item.Mobile,
                certificateType = "111",
                certificateNo = item.IdentifyID,
                jobNo = item.EnCode
            };
            string rtnMsg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
            return rtnMsg;
        }
        #endregion

        #region 导入人员集团编号
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImportUserEncode(string fieldName = "EnCode", string orgId = "")
        {
            var currUser = OperatorProvider.Provider.Current();
            try
            {
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
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName), Aspose.Cells.FileFormatType.Excel2007Xlsx);
                    //string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    //file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                    //Workbook wb = new Aspose.Cells.Workbook();
                    //wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
                    IList<string> mutipname = new List<string>(); //系统中名字重复的人员
                    IEnumerable<UserEntity> lstUsers = new List<UserEntity>();
                    if (string.IsNullOrWhiteSpace(orgId))
                    {
                        lstUsers = userBLL.GetList();
                    }
                    else
                    {
                        lstUsers = userBLL.GetListForCon(t => t.OrganizeId == orgId);
                    }
                    mutipname = lstUsers.GroupBy(t => t.RealName).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //姓名
                        string fullname = dt.Rows[i]["姓名"].ToString().Trim();
                        //集团编号
                        string encode = dt.Rows[i]["编号"].ToString().Trim();
                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(encode))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行姓名或者集团编号存在空,未能导入.";
                            error++;
                            continue;
                        }
                        if (mutipname.Contains(fullname))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行 " + fullname + " 在系统中存在重名情况未能导入.";
                            error++;
                            continue;
                        }
                        try
                        {
                            UserEntity user = lstUsers.Where(t => t.RealName == fullname).FirstOrDefault();
                            if (user == null)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行 " + fullname + " 在系统中不存在,未能导入.";
                                error++;
                                continue;
                            }
                            else
                            {
                                if (fieldName == "EnCode")
                                {
                                    user.EnCode = encode;
                                }
                                else
                                {
                                    user.Account = encode;
                                }
                                userBLL.SaveForm(user.UserId, user);
                            }

                        }
                        catch
                        {
                            error++;
                            continue;
                        }
                    }
                    count = dt.Rows.Count;
                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }

                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion
    }
    #region 毕节辅助类
    /// <summary>
    /// 人员信息实体
    /// </summary>
    public class HdgzUserEntity
    {
        public string pin { get; set; }
        public string name { get; set; }
        public string deptnumber { get; set; }
    }
    #endregion
}
