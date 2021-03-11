using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}