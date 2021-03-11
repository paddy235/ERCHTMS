using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Content
{
    /// <summary>
    /// trainee 的摘要说明
    /// </summary>
    public class trainee : IHttpHandler
    {
        //编码管理明细内容
        DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        UserBLL userbll = new UserBLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            GoToTrainee(context);
        }


        #region 跳转到培训平台
        /// <summary>
        /// 跳转到培训
        /// </summary>
        public void GoToTrainee(HttpContext context)
        {

            DataTable dt = dataitemdetailbll.GetListByCode("SafetyTrainee"); //获取对应的安全培训配置项

            Operator users = OperatorProvider.Provider.Current(); //当前用户
            UserEntity person = userbll.GetEntity(users.UserId);

            SingleUser entity = new SingleUser();
            entity.account = users.Account;
            entity.password = HttpCommon.MD5Encrypt(users.Password);
            entity.name = users.UserName;
            entity.mobileNo = person.Telephone;
            entity.sex = person.Gender == "男" ? "1" : "0";
            entity.idType = "0";
            entity.idNumber = person.IdentifyID;
            entity.email = person.Email;
            entity.isValid = "1";

            string openId = person.OpenId; // openid(单点登陆唯一标识) 
            DataRow[] rows = dt.Select(string.Format(" itemname = '{0}'", users.OrganizeId)); //当前培训信息
            string traineerInfo = string.Empty;
            if (rows.Count() > 0)
            {
                traineerInfo = rows[0]["itemvalue"].ToString();
            }
            if (!string.IsNullOrEmpty(traineerInfo))
            {
                #region  具体操作内容
                string[] arrTrainee = traineerInfo.Split('|');
                string version = arrTrainee[0].ToString();//版本
                string appCode = arrTrainee[1].ToString(); //应用代码       skc
                string secretKey = arrTrainee[2].ToString(); //应用密钥//4DAA67B83F932CBD  CB93FE6E34B90127
                string account = person.Telephone;
                string singleValue = string.Empty;
                string yqStr = string.Empty;
                string rqUrl = string.Empty;
                string result = string.Empty;
                ////用户中心密码
                string returnPwd = HttpCommon.MD5Encrypt(users.Password);
                ////当前用户手机号不能为空
                if (string.IsNullOrEmpty(account))
                {
                    //跳转到错误页面
                    context.Response.Redirect("~/Error/ErrorMessage", false);
                }
                try
                {
                    ////第一步，进行验签
                    GetAccountObj aobj = new GetAccountObj();
                    aobj.account = person.Telephone;
                    aobj.appCode = appCode;
                    aobj.secretKey = secretKey;
                    aobj.version = version;
                    yqStr = JsonConvert.SerializeObject(aobj);
                    singleValue = HttpCommon.MD5EncryptFor32(yqStr);
                    //验证是否存在当前用户
                    string validateStr = dt.Select(" itemname = 'TraineeValidate'")[0]["itemvalue"].ToString();
                    rqUrl = string.Format(validateStr, version, appCode, person.Telephone, singleValue);
                    result = HttpCommon.HttpPost(rqUrl);
                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    string code = dy.code;
                    //账号不存在
                    if (code == "B000009")
                    {
                        string userJson = JsonConvert.SerializeObject(entity);
                        string aesData = HttpCommon.AesEncrypt(userJson, secretKey);
                        RegisterObj yqObj = new RegisterObj();
                        yqObj.appCode = appCode;
                        yqObj.data = aesData;
                        yqObj.secretKey = secretKey;
                        yqObj.version = version;
                        yqStr = JsonConvert.SerializeObject(yqObj);
                        singleValue = HttpCommon.MD5EncryptFor32(yqStr);
                        //注册信息
                        string registerStr = dt.Select(" itemname = 'TraineeRegister'")[0]["itemvalue"].ToString();
                        rqUrl = string.Format(registerStr, version, appCode, aesData, singleValue);
                        if (rqUrl.Contains("+"))
                        {
                            rqUrl = rqUrl.Replace("+", "%2B");
                        }
                        result = HttpCommon.HttpPost(rqUrl);
                    }
                    else
                    {
                        //成功状态
                        if (code == "000000")
                        {
                            //获取账号后
                            string data = dy.data.ToString();
                            if (!string.IsNullOrEmpty(data))
                            {
                                data = data.Replace("\r\n", "");
                            }
                            string userStr = HttpCommon.AesDecrypt(data, secretKey);
                            dynamic userdy = JsonConvert.DeserializeObject<ExpandoObject>(userStr);
                            openId = userdy.openId;
                            returnPwd = userdy.password.ToString();
                            //如果当前账号下本地数据库中openid为空
                            if (string.IsNullOrEmpty(person.OpenId))
                            {
                                //更新人员信息表,将openId 存入person信息表当中
                                //person.OpenId = openId;
                                //userbll.SaveForm(person.UserId, person);
                                //更新人员信息表,将openId 存入person信息表当中
                                string usql = string.Format("update base_user set openid = '{0}'  where userid = '{1}'", openId, person.UserId);
                                //待更新
                                int excuteResult = userbll.ExcuteBySql(usql);
                            }
                        }
                    }
                    //登录用户中心获取票据
                    LoginObj loginobj = new LoginObj();
                    loginobj.account = person.Telephone;
                    loginobj.appCode = appCode;
                    loginobj.password = returnPwd;
                    loginobj.secretKey = secretKey;
                    loginobj.version = version;
                    yqStr = JsonConvert.SerializeObject(loginobj);
                    singleValue = HttpCommon.MD5EncryptFor32(yqStr);
                    //跳转到注册页面
                    string loginStr = dt.Select(" itemname = 'TraineeLogin'")[0]["itemvalue"].ToString();
                    rqUrl = string.Format(loginStr, version, appCode, loginobj.account, loginobj.password, singleValue);
                    if (rqUrl.Contains("+"))
                    {
                        rqUrl = rqUrl.Replace("+", "%2B");
                    }
                    result = HttpCommon.HttpPost(rqUrl);
                    dynamic logindy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    if (logindy.code.ToString() == "000000")
                    {
                        string logindata = logindy.data.ToString();
                        if (!string.IsNullOrEmpty(logindata))
                        {
                            logindata = logindata.Replace("\r\n", "");
                        }
                        string loginstr = HttpCommon.AesDecrypt(logindata, secretKey);
                        dynamic lastdy = JsonConvert.DeserializeObject<ExpandoObject>(loginstr);
                        string ticket = lastdy.ticket;  //票据
                        string traineeStr = dt.Select(" itemname = 'TraineeUrl'")[0]["itemvalue"].ToString();
                        string sendurl = string.Format(traineeStr, ticket);

                        context.Response.Redirect(sendurl, false);
                    }
                    else
                    {
                        context.Response.Redirect("~/Error/ErrorMessage", false);
                    }
                }
                catch (Exception ex)
                {

                    context.Response.Redirect("~/Error/ErrorMessage", false);
                }
                #endregion
            }

        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }



}