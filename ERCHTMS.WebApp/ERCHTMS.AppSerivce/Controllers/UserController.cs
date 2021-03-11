using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.AppSerivce.Models;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class UserController : ApiController
    {
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

        /// <summary>
        /// 博安云后台注册用户调用接口
        /// </summary>
        /// <param name="json">用户信息（如：json:{userName:"",mobile:"",deptName:""}）</param>
        ///参数说明：userName：姓名，mobile：手机号(作为系统账号)，deptName：单位名称
        /// <returns></returns>
        [HttpPost]
        public object RegisterUser()
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string json = HttpContext.Current.Request.Params["json"];
            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    ERCHTMS.Busines.BaseManage.UserBLL userBll = new ERCHTMS.Busines.BaseManage.UserBLL();
                    ERCHTMS.Busines.BaseManage.DepartmentBLL deptBll = new ERCHTMS.Busines.BaseManage.DepartmentBLL();
                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                    if (string.IsNullOrEmpty(dy.userName))
                    {
                        return new { code = 1, data = "", info = "参数：userName为空" };
                    }
                    if (string.IsNullOrEmpty(dy.mobile))
                    {
                        return new { code = 1, data = "", info = "参数：mobile为空" };
                    }
                    if (string.IsNullOrEmpty(dy.deptName))
                    {
                        return new { code = 1, data = "", info = "参数：deptName为空" };
                    }
                    //判断账号是否在系统中存在
                    bool result = userBll.ExistAccount(dy.mobile, "");
                    if (result)
                    {
                        string deptid = new DataItemDetailBLL().GetItemValue("dept", "TryDept");//所属单位ID，在后台配置（目前处理方案是在后台初始化好一个演示的单位，注册的用户直接挂在此单位即可）
                        string pathurl = new DataItemDetailBLL().GetItemValue("imgUrl"); //web平台对应的地址，放在编码配置也可后台直接后去，请根据实际情况处理
                        if (!string.IsNullOrEmpty(deptid))
                        {
                            var dept = deptBll.GetEntity(deptid);
                            UserEntity user = new UserEntity();
                            user.UserId = Guid.NewGuid().ToString();
                            user.Account = dy.mobile;
                            Random rnd = new Random();
                            string str = rnd.Next(10000, 99999).ToString();
                            user.RealName = dy.userName;
                            user.Password = str;
                            user.DepartmentId = dept.DepartmentId;
                            user.DepartmentCode = dept.EnCode;
                            user.OrganizeId = dept.OrganizeId;
                            user.OrganizeCode = dept.EnCode;
                            user.Mobile = dy.mobile;
                            user.IsEpiboly = "0";
                            user.RoleName = "公司级用户,公司管理员";
                            user.RoleId = "aece6d68-ef8a-4eac-a746-e97f0067fab5,5af22786-e2f2-4a3d-8da3-ecfb16b96f36";
                            user.AllowStartTime = DateTime.Now;
                            user.AllowEndTime = Convert.ToDateTime(user.AllowStartTime).AddDays(15);
                            user.Description = dy.deptName;
                            user.IdentifyID = "420115" + new Random().Next(1970, 2000) + "0722" + new Random().Next(1000, 9999);
                            user.IsPresence = "1";
                            var tempdata = new
                            {
                                account = dy.mobile,//账号
                                password = str,
                                allowEndTime = Convert.ToDateTime(user.AllowEndTime).ToString("yyyy-MM-dd HH:mm:ss"),
                                path = pathurl
                            };
                            userBll.SaveForm(user.UserId, user);
                            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,注册成功,参数为:" + json + "\r\n");
                            return new { code = 0, data = tempdata, info = "操作成功" };
                        }
                        else
                        {
                            return new { code = 1, data = "", info = "试用单位未配置" };
                        }
                    }
                    else
                    {
                        return new { code = 1, data = "", info = "手机号已存在!" };
                    }
                }
                else
                {
                    return new { code = 1, data = "", info = "json参数为空" };
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,注册失败,异常信息:" + ex.Message + "参数为:" + json + "\r\n");
                return new { code = 1, data = "", info = ex.Message };
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetUserData([FromBody]JObject json)
        {
            try
            {
                ERCHTMS.Busines.BaseManage.UserBLL userBll = new ERCHTMS.Busines.BaseManage.UserBLL();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "U.USERID";
                pagination.p_fields = "U.REALNAME,U.DEPARTMENTID,D.FULLNAME as DeptName,U.WORKGROUPID,U.MOBILE,U.TELEPHONE,U.ISPRESENCE";
                pagination.p_tablename = @"BASE_USER U 
                                           LEFT JOIN BASE_DEPARTMENT D ON U.DEPARTMENTID=D.DEPARTMENTID";
                pagination.conditionJson = string.Format(" ORGANIZECODE ='{0}'", curUser.OrganizeCode);
                //pagination.conditionJson = string.Format(" 1=1");
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "u.createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                //查询条件 名称
                string RealName = dy.data.RealName;
                if (!string.IsNullOrEmpty(RealName))
                {
                    pagination.conditionJson += string.Format(" and U.RealName='{0}'", RealName);
                }
                //查询条件 登记时间（创建日期）
                string CreateDate = dy.data.CreateDate;
                if (!string.IsNullOrEmpty(CreateDate))
                {
                    pagination.conditionJson += string.Format(" and to_char(U.CreateDate,'yyyy-mm-dd')='{0}'", CreateDate);
                }
                DataTable dt = userBll.GetPageList(pagination, null);
                var JsonData = new
                {
                    rows = dt,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                };
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JsonData };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        public ListResult<UserEntity> GetUsers(ModelParam<string[]> p)
        {
            var total = 0;
            ERCHTMS.Busines.BaseManage.UserBLL userBll = new ERCHTMS.Busines.BaseManage.UserBLL();
            var data = userBll.GetList(p.Data, p.PageSize, p.PageIndex, out total);
            return new ListResult<UserEntity>() { Success = true, Data = data, Total = total };
        }

        [HttpPost]
        public object InsertTemporaryPeople([FromBody]JObject jObject)
        {
            var userService = new UserBLL();
            try
            {
                string res = jObject.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new List<TemporaryPeopleParameter>()
                });
                UserEntity createUser = userService.GetEntity(dy.userid); ;
                if (createUser == null) throw new Exception("你没有权限添加权限");

                List<UserEntity> successList = new List<UserEntity>();//要新增到数据库里的数据
                List<object> badList = new List<object>();//有问题的数据
                foreach (TemporaryPeopleParameter item in dy.data)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(item.Mobile) && !userService.ExistMoblie(item.Mobile))
                        {
                            //校验手机号是否重复，重复该条数据不让添加
                            badList.Add(new { data = item, msg = "手机号重复" });
                            continue;
                        }
                        //如果账号存在则再生成一个，直到没有重复的为止
                        string pyStr = Str.PinYin(item.RealName);
                        string account = pyStr;
                        int count = 1;
                        while (!userService.ExistAccount(account))
                        {
                            account = pyStr + GetNumStr(count);
                            count++;
                        }

                        UserEntity user = new UserEntity();
                        user.UserId = Guid.NewGuid().ToString();
                        user.RealName = item.RealName;
                        user.IdentifyID = item.IdentifyID;
                        user.Mobile = item.Mobile;
                        user.Account = account;
                        user.IdentifyID = item.IdentifyID;
                        user.IsEpiboly = "1";
                        user.Gender = "男";
                        user.OrganizeId = createUser.OrganizeId;
                        user.OrganizeCode = createUser.OrganizeCode;
                        user.CreateUserOrgCode = createUser.OrganizeCode;
                        user.CreateDate = DateTime.Now;
                        user.CreateUserDeptCode = createUser.DepartmentCode;
                        user.CreateUserId = createUser.UserId;
                        user.CreateUserName = createUser.RealName;
                        user.DepartmentId = createUser.DepartmentId;
                        user.DepartmentCode = createUser.DepartmentCode;
                        user.Password = "Abc123456";//默认密码123456
                        user.IsPresence = "1";

                        //岗位随机分配一个本班组下没有负责人角色的岗位
                        IEnumerable<RoleEntity> rlist = new JobBLL().GetList().Where(p => p.DeptId == createUser.DepartmentId && !p.RoleIds.Contains("27eb996b-1294-41d6-b8e6-837645a66819"));
                        if (rlist != null && rlist.Count() > 0)
                        {
                            var defaultRole = rlist.FirstOrDefault();
                            user.DutyId = defaultRole.RoleId;
                            user.DutyName = defaultRole.FullName;
                        }
                        //	职务:默认为编码管理中排序为最后一个的职务
                        var defaultJob = new JobBLL().GetList().Where(p => p.OrganizeId == createUser.OrganizeId).OrderByDescending(x => x.SortCode).FirstOrDefault();
                        if (defaultJob != null)
                        {
                            user.PostName = defaultJob.FullName;
                            user.PostId = defaultJob.RoleId;
                            user.PostCode = defaultJob.EnCode;
                        }
                        //角色，默认班组级用户
                        RoleEntity roleEntity = new RoleCache().GetList().Where(a => a.OrganizeId == createUser.OrganizeId || string.IsNullOrWhiteSpace(a.OrganizeId)).Where(p => p.FullName.Contains("班组级用户")).FirstOrDefault();
                        if (roleEntity != null) user.RoleId = roleEntity.RoleId; user.RoleName = roleEntity.FullName;
                        roleEntity = new RoleCache().GetList().Where(a => a.OrganizeId == createUser.OrganizeId || string.IsNullOrWhiteSpace(a.OrganizeId)).Where(p => p.FullName.Contains("普通用户")).FirstOrDefault();
                        if (roleEntity != null)
                        {
                            if (!string.IsNullOrEmpty(roleEntity.RoleId))
                            {
                                user.RoleId += "," + roleEntity.RoleId;
                                user.RoleName += "," + roleEntity.FullName;
                            }
                            else
                            {
                                user.RoleId += roleEntity.RoleId;
                                user.RoleName = roleEntity.FullName;
                            }
                        }
                        string objId = userService.SaveForm(user.UserId, user, 0);
                        if (!string.IsNullOrWhiteSpace(objId))
                        {
                            //不为空则添加成功 
                            successList.Add(user);
                            if (!string.IsNullOrWhiteSpace(new DataItemDetailBLL().GetItemValue("bzAppUrl")))
                            {
                                user.Password = "Abc123456";
                                var task = Task.Factory.StartNew(() =>
                                {
                                    SaveUser(user);
                                });
                            }
                        }
                        else
                        {
                            badList.Add(new { data = item, msg = "添加失败" });
                        }

                    }
                    catch (Exception itemEx)
                    {
                        badList.Add(new { data = item, msg = itemEx.Message });
                    }

                }

                return new { Code = 0, Info = "操作成功", data = successList.Select(p => new { p.RealName, p.Account, Password = "Abc123456" }) };

            }
            catch (Exception ex)
            {
                return new { Code = -1, info = "操作失败", data = ex.Message };
            }
        }

        #region 私有方法
        /// <summary>
        /// 生成给账号用的后缀 ，四位长度
        /// </summary>
        /// <param name="count">查询的</param>
        /// <returns></returns>
        private string GetNumStr(int count)
        {
            string countStr = count.ToString();
            //int length = 4 - countStr.Length;
            countStr = countStr.PadLeft(4, '0');
            //while (length > 0)
            //{
            //    countStr.PadLeft()
            //}
            return countStr;
        }

        private void SaveUser(UserEntity user)
        {
            WebClient wc = new WebClient();
            DataItemDetailBLL dd = new DataItemDetailBLL();
            string imgUrl = dd.GetItemValue("imgUrl");
            string bzAppUrl = dd.GetItemValue("bzAppUrl");
            wc.Credentials = CredentialCache.DefaultCredentials;
            string logPath = new DataItemDetailBLL().GetItemValue("imgPath") + "\\logs\\";
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //当前操作用户账号
                nc.Add("account", "System");
                //用户信息

                if (user.EnterTime == null)
                {
                    user.EnterTime = DateTime.Now;
                }
                if (!string.IsNullOrEmpty(user.SignImg))
                {
                    user.SignImg = imgUrl + user.SignImg;
                }


                if (!string.IsNullOrEmpty(user.HeadIcon))
                {
                    user.HeadIcon = imgUrl + user.HeadIcon;
                }
                if (user.Password.Contains("**"))
                {
                    user.Password = null;
                }
                user.Gender = user.Gender == "男" ? "1" : "0";
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                if (Debugger.IsAttached)
                {
                    bzAppUrl = "http://localhost:10037/api/SyncData/";

                }
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
            string logPath = new DataItemDetailBLL().GetItemValue("imgPath") + "\\logs\\";
            //将同步结果写入日志文件
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
        }
        #endregion
    }
}