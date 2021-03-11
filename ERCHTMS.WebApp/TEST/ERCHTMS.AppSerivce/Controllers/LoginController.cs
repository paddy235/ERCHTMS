
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using ERCHTMS.AppSerivce;
using ERCHTMS.Busines;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    [HandlerLogin(LoginMode.Ignore)]
    public class LoginController : ApiController
    {
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentCache departmentCache = new DepartmentCache();
        private RoleCache roleCache = new RoleCache();
        private AccountBLL accountBLL = new AccountBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL();
        // GET api/login
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/login/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/login
        public void Post([FromBody]string value)
        {
        }

        // PUT api/login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/login/5
        public void Delete(int id)
        {
        }

        #region 移动端登录
        /// <summary>
        /// 移动端登录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Object checkLogin([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string username = dy.data.useraccount;
            string password = dy.data.password;
            LogEntity logEntity = new LogEntity();
            logEntity.CategoryId = 1;
            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
            logEntity.OperateAccount = username;
            logEntity.OperateUserId = username;
            logEntity.Module = "APP";

            try
            {
                #region 内部账户验证
                UserBLL userBLL = new UserBLL();
                UserInfoEntity userEntity = userBLL.CheckLogin(username, password);
                if (userEntity != null)
                {
                    if (userEntity.AllowStartTime != null && userEntity.AllowEndTime != null)
                    {
                        if (DateTime.Now > userEntity.AllowEndTime)
                        {
                            return new { code = -1, count = 0, info = "您的账号使用期限已过期，请联系管理员或客服，谢谢" };
                        }
                    }
                    AuthorizeBLL authorizeBLL = new AuthorizeBLL();
                    Operator operators = new Operator();
                    operators.UserId = userEntity.UserId;
                    operators.Code = userEntity.EnCode;
                    operators.Account = userEntity.Account;
                    operators.UserName = userEntity.RealName;
                    operators.Password = userEntity.Password;
                    operators.Secretkey = userEntity.Secretkey;
                    operators.DeptId = userEntity.DepartmentId;
                    operators.ParentId = userEntity.ParentId;
                    operators.DeptCode = userEntity.DepartmentCode;
                    operators.OrganizeCode = userEntity.OrganizeCode;
                    operators.DeptName = userEntity.DeptName;
                    operators.IsTrain = userEntity.IsTrain;
                    operators.SignImg = userEntity.SignImg;
                    DepartmentEntity dept = userBLL.GetUserOrgInfo(userEntity.UserId);//获取当前用户所属的机构
                    operators.OrganizeId = dept.DepartmentId; //所属机构ID
                    operators.OrganizeCode = dept.EnCode;//所属机构编码
                    operators.NewDeptCode = dept.DeptCode;//所属机构新的编码（对应部门表中新加的编码字段deptcode）
                    operators.OrganizeName = dept.FullName;//所属机构名称
                    operators.SpecialtyType = userEntity.SpecialtyType;

                    ////公司级用户
                    if (new UserBLL().HaveRoleListByKey(userEntity.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
                    {
                        operators.DeptId = userEntity.OrganizeId;
                        operators.DeptCode = userEntity.OrganizeCode;
                        operators.DeptName = userEntity.OrganizeName;
                    }
                    operators.PostName = userBLL.GetObjectName(userEntity.UserId, 3);
                    operators.RoleName = userBLL.GetObjectName(userEntity.UserId, 2);
                    operators.RoleId = userEntity.RoleId;
                    operators.PostId = userEntity.PostId;
                    operators.DutyName = userBLL.GetObjectName(userEntity.UserId, 4);
                    operators.IPAddress = Net.Ip;
                    operators.Photo = dataitemdetailbll.GetItemValue("imgUrl") + userEntity.HeadIcon;  //头像
                    operators.IdentifyID = userEntity.IdentifyID; //身份证号码
                    //operators.SendDeptID = userEntity.SendDeptID;
                    //operators.IPAddressName = IPLocation.GetLocation(Net.Ip);
                    operators.ObjectId = new PermissionBLL().GetObjectStr(userEntity.UserId);
                    operators.LogTime = DateTime.Now;
                    operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
                    //写入当前用户数据权限
                    AuthorizeDataModel dataAuthorize = new AuthorizeDataModel();
                    dataAuthorize.ReadAutorize = authorizeBLL.GetDataAuthor(operators);
                    dataAuthorize.ReadAutorizeUserId = authorizeBLL.GetDataAuthorUserId(operators);
                    dataAuthorize.WriteAutorize = authorizeBLL.GetDataAuthor(operators, true);
                    dataAuthorize.WriteAutorizeUserId = authorizeBLL.GetDataAuthorUserId(operators, true);
                    operators.DataAuthorize = dataAuthorize;
                    //判断是否系统管理员
                    if (userEntity.Account == "System")
                    {
                        operators.IsSystem = true;
                    }
                    else
                    {
                        operators.IsSystem = false;
                    }

                    string userMode = "";

                    string roleCode = dataitemdetailbll.GetItemValue("HidApprovalSetting");

                    string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

                    string[] pstr = HidApproval.Split('#');  //分隔机构组

                    foreach (string strArgs in pstr)
                    {
                        string[] str = strArgs.Split('|');

                        //当前机构相同，且为本部门安全管理员验证  第一种 层层上报
                        if (str[0].ToString() == userEntity.OrganizeId && str[1].ToString() == "0")
                        {
                            /*************临时使用，后续需要进行调整，原有隐患基于角色，较为固定，后期则废弃*************/
                            //WfControlObj wfentity = new WfControlObj();
                            //wfentity.businessid = ""; //
                            //wfentity.startflow = "隐患评估";
                            //wfentity.submittype = "上报";
                            //wfentity.rankname = "一般隐患";
                            //wfentity.user = operators;
                            //wfentity.mark = "厂级隐患排查"; //厂级隐患排查
                            //wfentity.isvaliauth = true;

                            ////获取下一流程的操作人
                            //WfControlResult result = new  WfControlBLL().GetWfControl(wfentity);
                            //bool ishaveapproval = result.ishave;  //具有评估权限的人

                            int count = new UserBLL().GetUserListByRole(userEntity.DepartmentCode, roleCode, userEntity.OrganizeId).ToList().Where(p => p.UserId == userEntity.UserId).Count();
                            if (count > 0)//包含安全管理员、负责人
                            {
                                userMode = "0";
                            }
                            else
                            {
                                userMode = "1";
                            }

                            break;
                        }
                        if (str[0].ToString() == userEntity.OrganizeId && str[1].ToString() == "1")
                        {
                            //获取指定部门的所有人员
                            int count = new UserBLL().GetUserListByDeptCode(str[2].ToString(), null, false, userEntity.OrganizeId).ToList().Where(p => p.UserId == userEntity.UserId).Count();
                            if (count > 0)
                            {
                                userMode = "2";
                            }
                            else
                            {
                                userMode = "3";
                            }
                            break;
                        }
                    }
                    if (userEntity.RoleName.Contains("省级用户"))
                    {
                        userMode = "4";
                    }
                    string rankArgs = dataitemdetailbll.GetItemValue("GeneralHid"); //一般隐患
                    operators.rankArgs = rankArgs;
                    operators.wfMode = userMode;

                    string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

                    string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

                    string CompanyRole = hidPlantLevel + "," + hidOrganize;

                    var userList = userBLL.GetUserListByDeptCode(userEntity.DepartmentCode, CompanyRole, false, userEntity.OrganizeId).Where(p => p.UserId == userEntity.UserId).ToList();

                    string isPlanLevel = "";
                    //当前用户是公司级及厂级用户
                    if (userList.Count() > 0)
                    {
                        isPlanLevel = "1"; //厂级用户
                    }
                    else
                    {
                        isPlanLevel = "0";  //非公司及厂级
                    }
                    operators.isPlanLevel = isPlanLevel;

                    string pricipalCode = dataitemdetailbll.GetItemValue("HidPrincipalSetting");
                    IList<UserEntity> ulist = new UserBLL().GetUserListByRole(userEntity.DepartmentCode, pricipalCode, userEntity.OrganizeId).ToList();
                    //返回的记录数,大于0，标识当前用户拥有部门负责人身份，反之则无
                    int uModel = ulist.Where(p => p.UserId == userEntity.UserId).Count();
                    operators.isPrincipal = uModel > 0 ? "1" : "0";
                    var deptEntity = new DepartmentBLL().GetEntity(userEntity.DepartmentId);
                    if (null != deptEntity)
                    {
                        operators.SendDeptID = deptEntity.SendDeptID;
                    }
                    else
                    {
                        operators.SendDeptID = "";
                    }
                    //用于违章的用户标记
                    string mark = string.Empty;

                    mark = userbll.GetSafetyAndDeviceDept(operators); //1 安全管理部门， 2 装置部门   5.发包部门

                    string isPrincipal = userbll.HaveRoleListByKey(operators.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0 ? "3" : ""; //第一级核准人
                    if (!string.IsNullOrEmpty(isPrincipal))
                    {
                        if (!string.IsNullOrEmpty(mark))
                        {
                            mark = mark + "," + isPrincipal;
                        }
                        else
                        {
                            mark = isPrincipal;
                        }
                    }
                    string isEpiboly = userbll.HaveRoleListByKey(operators.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0 ? "4" : "";  //承包商

                    if (!string.IsNullOrEmpty(isEpiboly))
                    {
                        if (!string.IsNullOrEmpty(mark))
                        {
                            mark = mark + "," + isEpiboly;
                        }
                        else
                        {
                            mark = isEpiboly;
                        }
                    }
                    operators.uMark = mark;
                    //国电新疆红雁池专用
                    string GDXJ_HYC_ORGCODE = dataitemdetailbll.GetItemValue("GDXJ_HYC_ORGCODE");
                    //国电新疆红雁池专用
                    operators.IsGdxjUser = userEntity.OrganizeCode == GDXJ_HYC_ORGCODE ? 1 : 0;
                    OperatorProvider.Provider.AddCurrent(operators);
                    //登录限制
                    //LoginLimit(username, operators.IPAddress, operators.IPAddressName);
                    //写入日志
                    logEntity.ExecuteResult = 1;
                    logEntity.ExecuteResultJson = "登录成功";
                    logEntity.WriteLog();

                    //异步处理与培训平台对接功能
                    //UserEntity ue = userbll.GetEntity(userEntity.UserId);
                    //UserInfoExtension uinfoextesion = GoToTrainee(ue);

                    var di = new DataItemDetailBLL();
                    string webPath = di.GetItemValue("imgPath");
                    string webUrl = di.GetItemValue("imgUrl");
                    string signUrl = "";
                    string qrCodeImgUrl = webUrl + "/Resource/AppFile/download.jpg";
                    if (!string.IsNullOrEmpty(userEntity.SignImg))
                    {
                        if (userEntity.SignImg.ToLower().Trim().StartsWith("http://"))
                        {
                            signUrl = userEntity.SignImg;
                        }
                        else
                        {
                            string fname = "";
                            string sImg = "";
                            if (userEntity.SignImg.ToLower().Contains("/resource/sign/"))
                            {
                                fname = userEntity.SignImg.Replace("/", "\\");
                                string name = userEntity.SignImg.Substring(userEntity.SignImg.LastIndexOf("/") + 1);
                                sImg = "s" + name.Replace("/", "\\");
                            }
                            else
                            {
                                fname = "\\Resource\\sign\\" + userEntity.SignImg.Replace("/", "\\");
                                sImg = "\\Resource\\sign\\s" + userEntity.SignImg.Replace("/", "\\");
                            }

                            if (File.Exists(webPath + sImg))
                            {
                                signUrl = webUrl + sImg.Replace("\\", "/");
                            }
                            else
                            {
                                if (File.Exists(webPath + fname))
                                {
                                    signUrl = webUrl + fname.Replace("\\", "/");
                                }
                            }
                        }
                    }
                    return new
                    {
                        code = 0,
                        count = -1,
                        info = "登陆成功",
                        data = new
                        {
                            userid = operators.UserId,
                            tokenid = operators.Token,
                            useraccount = operators.Account,
                            telephone = userEntity.Telephone,
                            phone = userEntity.Mobile,
                            username = operators.UserName,
                            password = operators.Password,
                            logtime = operators.LogTime,
                            secretkey = operators.Secretkey,
                            gender = operators.Gender,
                            organizeid = operators.OrganizeId,
                            deptid = operators.DeptId,
                            deptcode = operators.DeptCode,
                            deptname = operators.DeptName,
                            organizecode = operators.OrganizeCode,
                            organizename = operators.OrganizeName,
                            objectid = operators.ObjectId,
                            ipaddress = operators.IPAddress,
                            ipaddressname = operators.IPAddressName,
                            issystem = operators.IsSystem,
                            roleid = operators.RoleId,
                            rolename = operators.RoleName,
                            postid = operators.PostId,
                            postname = operators.PostName,
                            dutyname = operators.DutyName,
                            photo = operators.Photo,
                            wfmode = operators.wfMode,
                            senddeptid = operators.SendDeptID,
                            rankargs = operators.rankArgs,
                            isprincipal = operators.isPrincipal,
                            identifyid = operators.IdentifyID,
                            mark = operators.uMark,
                            signurl = signUrl,
                            isgdxjuser = operators.IsGdxjUser,
                            qrimgurl = qrCodeImgUrl,
                            //ticket = uinfoextesion.ticket,
                            //openid = uinfoextesion.openId,
                            //traineeaccount = uinfoextesion.traineeAccount,
                            //traineepwd = uinfoextesion.traineePwd,
                            dataauthorize = new
                            {
                                moduleid = operators.DataAuthorize.ModuleId,
                                readautorize = operators.DataAuthorize.ReadAutorize,
                                readautorizeuserid = operators.DataAuthorize.ReadAutorizeUserId,
                                writeautorize = operators.DataAuthorize.WriteAutorize,
                                writeautorizeuserid = operators.DataAuthorize.WriteAutorizeUserId
                            }
                        }
                    };
                }
                else
                {
                    return new { code = -1, count = 0, info = "密码输入错误" };
                }
                #endregion
            }
            catch (Exception ex)
            {
                WebHelper.RemoveCookie("autologin");                  //清除自动登录
                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = ex.Message;
                logEntity.WriteLog();
                return new { code = -1, count = 0, info = "账号或密码错误!" };
            }
        }

        #region 扫描二维码保存用户ID
        /// <summary>
        /// 扫描二维码保存用户ID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object QRCodeLogin([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string qrcode = dy.data.qrcode;
            CacheFactory.Cache().WriteCache(userid, qrcode, DateTime.Now.AddMinutes(30));
            return new { code = 0, count = 1, info = "成功", data = userid };
        }
        #endregion

        #endregion

        #region 获取培训平台的相关信息
        /// <summary>
        /// 获取培训平台的相关信息
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        //public UserInfoExtension GoToTrainee(UserEntity users)
        //{
        //    UserInfoExtension model = new UserInfoExtension();
        //    DataTable dt = dataitemdetailbll.GetListByCode("SafetyTrainee"); //获取对应的安全培训配置项
        //    SingleUser entity = new SingleUser();
        //    entity.account = users.Telephone;
        //    entity.password = users.Password;
        //    entity.name = users.RealName;
        //    entity.mobileNo = users.Telephone;
        //    entity.sex = users.Gender == "男" ? "1" : "0";
        //    entity.idType = "0";
        //    entity.idNumber = users.IdentifyID;
        //    entity.email = users.Email;
        //    entity.isValid = "1";
        //    string openId = users.OpenId; // openid(单点登陆唯一标识) 
        //    DataRow[] rows = dt.Select(string.Format(" itemname = '{0}'", users.OrganizeId)); //当前培训信息
        //    string traineerInfo = string.Empty;
        //    if (rows.Count() > 0)
        //    {
        //        traineerInfo = rows[0]["itemvalue"].ToString();
        //    }
        //    if (!string.IsNullOrEmpty(traineerInfo))
        //    {
        //        string[] arrTrainee = traineerInfo.Split('|');
        //        string version = arrTrainee[0].ToString();//版本
        //        string appCode = arrTrainee[1].ToString(); //应用代码       skc
        //        string secretKey = arrTrainee[2].ToString(); //应用密钥//4DAA67B83F932CBD  CB93FE6E34B90127
        //        string account = users.Telephone;
        //        string singleValue = string.Empty;
        //        string yqStr = string.Empty;
        //        string rqUrl = string.Empty;
        //        string result = string.Empty;

        //        string reValue = users.Password;
        //        try
        //        {
        //            ////第一步，进行验签
        //            GetAccountObj aobj = new GetAccountObj();
        //            aobj.account = users.Telephone;
        //            aobj.appCode = appCode;
        //            aobj.secretKey = secretKey;
        //            aobj.version = version;
        //            yqStr = JsonConvert.SerializeObject(aobj);
        //            singleValue = HttpCommon.MD5EncryptFor32(yqStr);
        //            //验证是否存在当前用户
        //            string validateStr = dt.Select(" itemname = 'TraineeValidate'")[0]["itemvalue"].ToString();
        //            rqUrl = string.Format(validateStr, version, appCode, users.Telephone, singleValue);
        //            //HttpCommon.HttpPostAsync(rqUrl, "", HttpAsyncValidate, System.Text.Encoding.UTF8); //此处无法应用异步操作
        //            result = HttpCommon.HttpPost(rqUrl);

        //            if (!result.Contains("未能解析此远程名称"))
        //            {
        //                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
        //                string code = dy.code;
        //                //账号不存在
        //                if (code == "B000009")
        //                {
        //                    string userJson = JsonConvert.SerializeObject(entity);
        //                    string aesData = HttpCommon.AesEncrypt(userJson, secretKey);
        //                    RegisterObj yqObj = new RegisterObj();
        //                    yqObj.appCode = appCode;
        //                    yqObj.data = aesData;
        //                    yqObj.secretKey = secretKey;
        //                    yqObj.version = version;
        //                    yqStr = JsonConvert.SerializeObject(yqObj);
        //                    singleValue = HttpCommon.MD5EncryptFor32(yqStr);
        //                    //注册信息
        //                    string registerStr = dt.Select(" itemname = 'TraineeRegister'")[0]["itemvalue"].ToString();
        //                    rqUrl = string.Format(registerStr, version, appCode, aesData, singleValue);
        //                    if (rqUrl.Contains("+"))
        //                    {
        //                        rqUrl = rqUrl.Replace("+", "%2B");
        //                    }
        //                    result = HttpCommon.HttpPost(rqUrl);
        //                }
        //                else
        //                {
        //                    if (code != "C000007")
        //                    {
        //                        //获取账号后
        //                        string data = dy.data.ToString();
        //                        if (!string.IsNullOrEmpty(data))
        //                        {
        //                            data = data.Replace("\r\n", "");
        //                        }
        //                        string userStr = HttpCommon.AesDecrypt(data, secretKey);
        //                        dynamic userdy = JsonConvert.DeserializeObject<ExpandoObject>(userStr);
        //                        openId = userdy.openId.ToString();
        //                        reValue = userdy.password.ToString();
        //                        //如果当前账号下本地数据库中openid为空
        //                        if (string.IsNullOrEmpty(users.OpenId))
        //                        {
        //                            //更新人员信息表,将openId 存入person信息表当中
        //                            string usql = string.Format("update base_user set openid = '{0}'  where userid = '{1}'",openId,users.UserId);
        //                            //DbParameter[] param = new OracleParameter[] 
        //                            //{
        //                            //    new OracleParameter("@openids",openId),
        //                            //    new OracleParameter("@userids",users.UserId)
        //                            //};
        //                            //待更新
        //                           int excuteResult = userbll.ExcuteBySql(usql);
        //                        }
        //                    }
        //                }
        //                //登录用户中心获取票据
        //                LoginObj loginobj = new LoginObj();
        //                loginobj.account = users.Telephone;
        //                loginobj.appCode = appCode;
        //                loginobj.password = users.Password;
        //                loginobj.secretKey = secretKey;
        //                loginobj.version = version;
        //                yqStr = JsonConvert.SerializeObject(loginobj);
        //                singleValue = HttpCommon.MD5EncryptFor32(yqStr);
        //                //跳转到注册页面
        //                string loginStr = dt.Select(" itemname = 'TraineeLogin'")[0]["itemvalue"].ToString();
        //                rqUrl = string.Format(loginStr, version, appCode, loginobj.account, loginobj.password, singleValue);
        //                if (rqUrl.Contains("+"))
        //                {
        //                    rqUrl = rqUrl.Replace("+", "%2B");
        //                }
        //                result = HttpCommon.HttpPost(rqUrl);
        //                dynamic logindy = JsonConvert.DeserializeObject<ExpandoObject>(result);
        //                if (logindy.code.ToString() == "000000")
        //                {
        //                    string logindata = logindy.data.ToString();
        //                    if (!string.IsNullOrEmpty(logindata))
        //                    {
        //                        logindata = logindata.Replace("\r\n", "");
        //                    }
        //                    string loginstr = HttpCommon.AesDecrypt(logindata, secretKey);
        //                    dynamic lastdy = JsonConvert.DeserializeObject<ExpandoObject>(loginstr);
        //                    string ticket = lastdy.ticket;  //票据
        //                    model.ticket = ticket;
        //                }
        //                model.openId = openId;
        //                model.traineeAccount = users.Telephone;
        //                model.traineePwd = reValue;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLog(ex); //写日志
        //            throw;
        //        }
        //    }
        //    return model;
        //}
        #endregion

        #region 验证回调
        /// <summary>
        /// 验证回调
        /// </summary>
        /// <param name="result"></param>
        //public void HttpAsyncValidate(object sender, UploadDataCompletedEventArgs e)
        //{
        //    string result = System.Text.Encoding.Default.GetString(e.Result);
        //} 
        #endregion

        #region 获取票据
        /// <summary>
        /// 获取票据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        //[HttpPost]
        //public Object GetTicket([FromBody]JObject json)
        //{
        //    string res = json.Value<string>("json");

        //    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

        //    //获取用户Id

        //    string userid = dy.userid;
        //    //获取用户基本信息
        //    OperatorProvider.AppUserId = userid;  //设置当前用户

        //    Operator curUser = OperatorProvider.Provider.Current();

        //    string ticket = string.Empty;
        //    //获取用户基本信息
        //    if (null == curUser)
        //    {
        //        return new { code = -1, count = 0, info = "请求失败,请登录!" };
        //    }
        //    string account = res.Contains("traineeaccount")? dy.data.traineeaccount: "";//账户
        //    string password = res.Contains("traineepwd") ? dy.data.traineepwd : "";//密码
        //    try
        //    {
        //        DataTable dt = dataitemdetailbll.GetListByCode("SafetyTrainee"); //获取对应的安全培训配置项
        //        UserEntity users = userbll.GetEntity(curUser.UserId);
        //        string openId = users.OpenId; // openid 单点登陆唯一标识
        //        DataRow[] rows = dt.Select(string.Format(" itemname = '{0}'", users.OrganizeId)); //当前培训信息
        //        string traineerInfo = string.Empty;
        //        if (rows.Count() > 0)
        //        {
        //            traineerInfo = rows[0]["itemvalue"].ToString();
        //        }
        //        if (!string.IsNullOrEmpty(traineerInfo))
        //        {
        //            string[] arrTrainee = traineerInfo.Split('|');
        //            string version = arrTrainee[0].ToString();//版本
        //            string appCode = arrTrainee[1].ToString(); //应用代码       skc
        //            string secretKey = arrTrainee[2].ToString(); //应用密钥//4DAA67B83F932CBD  CB93FE6E34B90127 

        //            //登录用户中心获取票据
        //            LoginObj loginobj = new LoginObj();
        //            loginobj.account = account;
        //            loginobj.appCode = appCode;
        //            loginobj.password = password;
        //            loginobj.secretKey = secretKey;
        //            loginobj.version = version;
        //            string yqStr = JsonConvert.SerializeObject(loginobj);
        //            string singleValue = HttpCommon.MD5EncryptFor32(yqStr);
        //            //跳转到注册页面
        //            string loginStr = dt.Select(" itemname = 'TraineeLogin'")[0]["itemvalue"].ToString();
        //            string rqUrl = string.Format(loginStr, version, appCode, loginobj.account, loginobj.password, singleValue);
        //            if (rqUrl.Contains("+"))
        //            {
        //                rqUrl = rqUrl.Replace("+", "%2B");
        //            }
        //            string result = HttpCommon.HttpPost(rqUrl);
        //            if (!result.Contains("未能解析此远程名称"))
        //            {
        //                dynamic logindy = JsonConvert.DeserializeObject<ExpandoObject>(result);
        //                if (logindy.code.ToString() == "000000")
        //                {
        //                    string logindata = logindy.data.ToString();
        //                    if (!string.IsNullOrEmpty(logindata))
        //                    {
        //                        logindata = logindata.Replace("\r\n", "");
        //                    }
        //                    string loginstr = HttpCommon.AesDecrypt(logindata, secretKey);
        //                    dynamic lastdy = JsonConvert.DeserializeObject<ExpandoObject>(loginstr);
        //                    ticket = lastdy.ticket;  //票据 
        //                }
        //            }
        //        }
        //        return new { code = 0, info = "获取成功", data = ticket };
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog(ex);
        //        return new { code = 1, info = "登录失败" };
        //    }
        //}
        #endregion

        #region 异常信息记录
        /// <summary>
        /// 异常信息记录
        /// </summary>
        /// <param name="ex"></param>
        //public void WriteLog(Exception ex)
        //{
        //    string filePath = AppDomain.CurrentDomain.BaseDirectory + "/erchtmsapp" + DateTime.Now.ToString("yyyyMMddhhmiss") + "log.txt";
        //    if (!File.Exists(filePath))
        //    {
        //        FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(fs);
        //        sw.WriteLine("InnerException异常信息：" + ex.InnerException);
        //        sw.WriteLine("Message异常信息：" + ex.Message);
        //        sw.WriteLine("Source异常信息：" + ex.Source);
        //        sw.WriteLine("TargetSite异常信息：" + ex.TargetSite);
        //        sw.WriteLine("StackTrace异常信息：" + ex.StackTrace);
        //        sw.Close();
        //        fs.Close();
        //    }
        //    else
        //    {
        //        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(fs);
        //        sw.WriteLine("InnerException异常信息：" + ex.InnerException);
        //        sw.WriteLine("Message异常信息：" + ex.Message);
        //        sw.WriteLine("Source异常信息：" + ex.Source);
        //        sw.WriteLine("TargetSite异常信息：" + ex.TargetSite);
        //        sw.WriteLine("StackTrace异常信息：" + ex.StackTrace);
        //        sw.Close();
        //        fs.Close();
        //    }
        //}
        #endregion
    }

    //登录信息
    public class loginData
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }

    //登录账号信息
    public class loginUserInfo
    { /// <summary>
        /// 用户主键
        /// </summary>		
        public string userid { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>		
        public string account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>		
        public string realname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>		
        public string headicon { get; set; }
        /// <summary>
        /// 性别
        /// </summary>		
        public string gender { get; set; }
        /// <summary>
        /// 手机
        /// </summary>		
        public string mobile { get; set; }
        /// <summary>
        /// 电话
        /// </summary>		
        public string telephone { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>		
        public string email { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>		
        public string oicq { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>		
        public string wechat { get; set; }
        /// <summary>
        /// MSN
        /// </summary>		
        public string msn { get; set; }
        /// <summary>
        /// 主管主键
        /// </summary>		
        public string managerid { get; set; }
        /// <summary>
        /// 主管
        /// </summary>		
        public string manager { get; set; }
        /// <summary>
        /// 机构主键
        /// </summary>		
        public string organizeid { get; set; }
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string organizename { get; set; }
        /// <summary>
        /// 部门主键
        /// </summary>		
        public string departmentid { get; set; }
        /// <summary>
        /// 部门名字
        /// </summary>		
        public string departmentname { get; set; }
        /// <summary>
        /// 角色主键
        /// </summary>		
        public string roleid { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string rolename { get; set; }
        /// <summary>
        /// 岗位主键
        /// </summary>		
        public string dutyid { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>		
        public string dutyname { get; set; }
        /// <summary>
        /// 职位主键
        /// </summary>		
        public string postid { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>		
        public string postname { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string description { get; set; }
    }

    public class UserInfoExtension
    {
        public string userId { get; set; }
        public string openId { get; set; }
        public string ticket { get; set; }
        public string traineeAccount { get; set; }
        public string traineePwd { get; set; }

        public string specialcode { get; set; }   //特殊人员code

        public string isprincipal { get; set; } //是否有权限(0:无权限 1:有权限)  
    }

}
