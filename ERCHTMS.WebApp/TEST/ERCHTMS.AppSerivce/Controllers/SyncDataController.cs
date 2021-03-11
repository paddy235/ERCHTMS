using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BSFramework.Util;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.AppSerivce.Controllers
{

    public class SyncDataController : BaseApiController
    {

        /// <summary>
        /// 设置当前操作用户信息并缓存
        /// </summary>
        /// <param name="userEntity">用户实体</param>
        private void SetUserInfo(UserInfoEntity userEntity)
        {
            if (ERCHTMS.Code.OperatorProvider.Provider.IsOnLine() != 1)
            {
                Operator operators = new Operator();
                operators.UserId = userEntity.UserId;
                operators.Code = userEntity.EnCode;
                operators.Account = userEntity.Account;
                operators.UserName = userEntity.RealName;
                operators.Secretkey = userEntity.Secretkey;
                operators.OrganizeId = userEntity.OrganizeId;
                operators.DeptId = userEntity.DepartmentId;
                operators.DeptCode = userEntity.DepartmentCode;
                operators.OrganizeCode = userEntity.OrganizeCode;
                operators.LogTime = DateTime.Now;
                ERCHTMS.Code.OperatorProvider.Provider.AddCurrent(operators);
                OperatorProvider.AppUserId = userEntity.UserId;
            }

        }
        [HttpGet]
        public string Get(string id)
        {
            return id;
        }
        /// <summary>
        /// 新增或保存用户
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">用户信息（对象被序列化之后的）</param>
        /// <returns></returns>
        [HttpPost]
        public string SaveUser(string keyValue)
        {

            string json = HttpContext.Current.Request.Params["json"];
            string account = HttpContext.Current.Request.Params["account"];
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string LabourNo = dy.LabourNo;//工号
                string WorkKind = dy.WorkKind;//工种
                string Folk = dy.Folk;//民族
                string CurrentWorkAge = dy.CurrentWorkAge;//工种工龄
                string OldDegree = dy.OldDegree;//原始学历
                string NewDegree = dy.NewDegree;//后期学历
                string Quarters = dy.Quarters;//职务名称
                string Planer = dy.Planer;//职务编号
                string Visage = dy.Visage;//政治面貌
                string Age = dy.Age;
                string Native = dy.Native;
                string TecLevel = dy.TecLevel;
                string JobName = dy.JobName;
                string HealthStatus = dy.HealthStatus;

                JobBLL jobbll = new JobBLL();
                List<RoleEntity> rolist = jobbll.GetList().ToList();
                UserEntity user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserEntity>(json);
                UserBLL userBll = new UserBLL();
                UserInfoEntity currUser = userBll.GetUserInfoByAccount(account);
                SetUserInfo(currUser);
                user.Gender = user.Gender == "1" ? "男" : "女";
                user.IsEpiboly = "0";
                user.IsPresence = "1";
                user.IsSpecial = user.IsSpecial == "" ? "否" : user.IsSpecial;
                user.IsSpecialEqu = user.IsSpecialEqu == "" ? "否" : user.IsSpecialEqu;
                user.EnCode = LabourNo;
                user.Craft = WorkKind;
                user.Nation = Folk;
                user.CraftAge = CurrentWorkAge;
                user.Degrees = OldDegree;
                user.DegreesID = OldDegree;
                user.LateDegrees = NewDegree;
                user.LateDegreesID = NewDegree;
                user.Political = Visage;
                user.Age = Age;
                user.Native = Native;
                user.TechnicalGrade = TecLevel;
                user.JobTitle = JobName;
                user.HealthStatus = HealthStatus;
                string jobid = "";
                if (Planer.Trim() != "")
                {
                    string[] planers = Planer.Split(',');
                    foreach (string jobno in planers)
                    {
                        RoleEntity ro = rolist.Where(it => it.EnCode == jobno).FirstOrDefault();
                        if (ro != null)
                        {
                            if (jobid == "")
                            {
                                jobid = ro.RoleId;
                            }
                            else
                            {
                                jobid += "," + ro.RoleId;
                            }
                        }
                    }
                }

                user.PostId = jobid;
                user.PostName = Quarters;
                user.PostCode = Planer;

                var deptBll = new DepartmentBLL();
                var dept=deptBll.GetEntity(user.DepartmentId);
                if(dept!=null)
                {
                    dept = deptBll.GetEntity(dept.OrganizeId);
                    if (dept!=null)
                    {
                        user.OrganizeCode = dept.EnCode;
                        user.OrganizeId = dept.OrganizeId;
                    }
                }
                if (user.RoleName.Contains("班组长"))
                {
                    user.RoleId = "2a878044-06e9-4fe4-89f0-ba7bd5a1bde6,d9432a6e-5659-4f04-9c10-251654199714,27eb996b-1294-41d6-b8e6-837645a66819";
                    user.RoleName = "普通用户,班组级用户,负责人";
                }
                if (user.RoleName.Contains("班组成员"))
                {
                    user.RoleId = "2a878044-06e9-4fe4-89f0-ba7bd5a1bde6,d9432a6e-5659-4f04-9c10-251654199714";
                    user.RoleName = "普通用户,班组级用户";

                }
                if (user.RoleName.Contains("部门管理员"))
                {
                    user.RoleId = "2a878044-06e9-4fe4-89f0-ba7bd5a1bde6,6c094cef-cca3-4b41-a71b-6ee5e6b89008,27eb996b-1294-41d6-b8e6-837645a66819";
                    user.RoleName = "普通用户,部门级用户,负责人";

                }
                if (user.RoleName.Contains("厂级管理员"))
                {

                    user.RoleId = "2a878044-06e9-4fe4-89f0-ba7bd5a1bde6,aece6d68-ef8a-4eac-a746-e97f0067fab5,27eb996b-1294-41d6-b8e6-837645a66819,5af22786-e2f2-4a3d-8da3-ecfb16b96f36";
                    user.RoleName = "普通用户,公司级用户,负责人,公司管理员";

                }
                try
                {
                    //处理接收以base64编码后的文件内容
                    if (!string.IsNullOrWhiteSpace(dy.SignImg))
                    {
                        byte[] byteData = byteData = Convert.FromBase64String(dy.SignImg);
                        // string dir = HttpContext.Current.Server.MapPath("~/Resource/sign");
                        DataItemDetailBLL dd = new DataItemDetailBLL();
                        string path = dd.GetItemValue("imgPath") + "\\Resource\\sign";
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        string imageName = Guid.NewGuid().ToString() + ".png";
                        System.IO.File.WriteAllBytes(path + "\\" + imageName, byteData);
                        user.SignImg = "/Resource/sign/" + imageName;
                    }

                }
                catch (Exception)
                {

                }
                userBll.SaveForm(keyValue, user);
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,保存用户成功,同步信息：" + json + "\r\n");
                return "success";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,保存用户失败,同步信息：" + json + ",异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteUser(string keyValue)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {

                UserBLL userBll = new UserBLL();
                UserEntity user = userBll.GetEntity(keyValue);
                if (user != null)
                {
                    userBll.RemoveForm(keyValue);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除用户成功,同步信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "\r\n");
                    return "操作成功";
                }
                return "用户不存在";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除用户失败,用户Id：" + keyValue + ",异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }
        /// <summary>
        /// 新增或保存部门
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">部门信息（对象被序列化之后的）</param>
        /// <returns></returns>
        [HttpPost]
        public string SaveDept(string keyValue)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string json = HttpContext.Current.Request.Params["json"];
            try
            {
                string account = HttpContext.Current.Request.Params["account"];
                UserBLL userBll = new UserBLL();
                UserInfoEntity currUser = userBll.GetUserInfoByAccount(account);
                SetUserInfo(currUser);
                DepartmentEntity dept = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(json);
                DepartmentBLL deptBll = new DepartmentBLL();
                deptBll.SaveForm(keyValue, dept);
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",保存部门成功,同步信息：" + json + "\r\n");
                return "success";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,保存部门失败,同步信息：" + json + ",异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public string UpdatePwd(string userId, string pwd)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";

            try
            {
                UserBLL userBll = new UserBLL();
                UserEntity user = userBll.GetEntity(userId);
                if (user != null)
                {
                    userBll.RevisePassword(userId,  Md5Helper.MD5(pwd,32).ToLower());
                }
                else
                {
                    return "用户不存在！";
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",修改密码成功,同步信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "\r\n");
                return "success";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,修改密码失败,同步信息：" + userId + ">" + pwd + ",异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }

        /// <summary>
        ///删除部门
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">部门信息（对象被序列化之后的）</param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteDept(string keyValue)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DepartmentEntity dept = deptBll.GetEntity(keyValue);
                if (dept != null)
                {
                    deptBll.RemoveForm(keyValue, null);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除部门成功,同步信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(dept) + "\r\n");
                    return "success";
                }
                return "error";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除部门失败,部门Id：" + keyValue + ",异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }
        /// <summary>
        /// 新增或保存角色
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">用户信息（对象被序列化之后的）</param>
        /// <returns></returns>
        [HttpPost]
        public string SaveRole(string keyValue)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string json = HttpContext.Current.Request.Params["json"];
            try
            {
                string account = HttpContext.Current.Request.Params["account"];
                UserBLL userBll = new UserBLL();
                UserInfoEntity currUser = userBll.GetUserInfoByAccount(account);
                SetUserInfo(currUser);

                RoleEntity role = Newtonsoft.Json.JsonConvert.DeserializeObject<RoleEntity>(json);
                RoleBLL roleBll = new RoleBLL();
                roleBll.SaveForm(keyValue, role);
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,保存角色成功,同步信息：" + json + "\r\n");
                return "success";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ，删除角色失败,同步信息：" + json + ",异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }
        /// <summary>
        /// 新增或保存角色
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">用户信息（对象被序列化之后的）</param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteRole(string keyValue)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                RoleBLL roleBll = new RoleBLL();
                RoleEntity role = roleBll.GetEntity(keyValue);
                if (role != null)
                {
                    roleBll.RemoveForm(keyValue);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除角色成功,操作信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(role) + "\r\n");
                    return "success";
                }
                return "error";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除角色失败,角色Id：" + keyValue + ",异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }

        #region 旁站监督任务对接班组
        /// <summary>
        /// 修改监督任务
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">用户信息（对象被序列化之后的）</param>
        /// <returns></returns>
        [HttpPost]
        public string SupervisionTask()
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            List<PushMessageData> listmessage = new List<PushMessageData>();
            try
            {
                string json = HttpContext.Current.Request.Params["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);

                StaffInfoEntity entity = new StaffInfoBLL().GetEntity(dy.JobId);
                if (entity != null)
                {
                    string userid = "";
                    //班组中班前会选择人员和时间
                    if (dy.state == "0")
                    {
                        entity.PStartTime = dy.StartTime;
                        entity.PEndTime = dy.EndTime;
                        entity.TaskUserId = dy.TaskUserId;
                        entity.TaskUserName = dy.TaskUserName;
                        entity.DataIsSubmit = "1";
                        new StaffInfoBLL().SaveForm(entity.Id, entity);//修改旁站监督人员

                        PushMessageData messagedata = new PushMessageData();
                        //推送消息到待监管旁站监督
                        messagedata.SendCode = "ZY015";
                        messagedata.EntityId = entity.Id;
                        listmessage.Add(messagedata);

                        string[] arrid = dy.TaskUserId.Split(',');
                        string[] arrname = dy.TaskUserName.Split(',');
                        for (int i = 0; i < arrid.Length; i++)
                        {
                            if (!userid.Contains(arrid[i]))
                            {
                                userid += userid + ",";
                            }
                            StaffInfoEntity staff = new StaffInfoEntity();
                            staff.PTeamName = entity.PTeamName;
                            staff.PTeamCode = entity.PTeamCode;
                            staff.PTeamId = entity.PTeamId;
                            staff.TaskUserId = arrid[i];
                            staff.TaskUserName = arrname[i];
                            staff.PStartTime = entity.PStartTime;
                            staff.PEndTime = entity.PEndTime;
                            staff.WorkInfoId = entity.WorkInfoId;
                            staff.WorkInfoName = entity.WorkInfoName;
                            staff.SumTimeStr = 0;
                            staff.DataIsSubmit = "0";
                            staff.SuperviseState = "0";//监督状态
                            staff.TaskShareId = entity.TaskShareId;
                            staff.TaskLevel = "2";
                            staff.StaffId = entity.Id;
                            staff.CreateUserId = entity.CreateUserId;
                            staff.CreateUserName = entity.CreateUserName;
                            staff.CreateUserDeptCode = entity.CreateUserDeptCode;
                            staff.CreateUserOrgCode = entity.CreateUserOrgCode;
                            staff.Create();
                            new StaffInfoBLL().SaveForm("", staff);
                        }

                        PushMessageData message = new PushMessageData();
                        //推送消息到待执行旁站监督
                        message.UserId = string.IsNullOrEmpty(userid) ? "" : userid.TrimEnd(',');
                        message.SendCode = "ZY016";
                        message.EntityId = entity.Id;
                        listmessage.Add(message);
                        //推送
                        new TaskShareBLL().PushSideMessage(listmessage);
                    }
                    //班后会
                    else if (dy.state == "1")
                    {
                        var newdata = new
                        {
                            taskshareid = entity.TaskShareId,
                            staffid = entity.Id
                        };
                        //SuperviseState 2:未完成 3:已完成
                        var strstate = "0";
                        if (dy.SuperviseState == "3")
                        {
                            strstate = "1";
                        }
                        new StaffInfoBLL().GetList(Newtonsoft.Json.JsonConvert.SerializeObject(newdata)).ToList().ForEach(t =>
                        {
                            t.DataIsSubmit = "1";
                            t.SuperviseState = strstate;
                            new StaffInfoBLL().SaveForm(t.Id, t);//修改下级任务
                        });
                        entity.SuperviseState = strstate;
                        new StaffInfoBLL().SaveForm(entity.Id, entity);
                    }
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,修改成功,参数为:" + json + "\r\n");
                }
                else
                {
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,实体为空,参数为:" + json + "\r\n");
                }
                return "success";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,修改失败,异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }



        /// <summary>
        /// 删除旁站监督任务
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">用户信息（对象被序列化之后的）</param>
        /// <returns></returns>
        [HttpPost]
        public string DelTask()
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string json = HttpContext.Current.Request.Params["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);

            try
            {
                new StaffInfoBLL().RemoveForm(dy.JobId);
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除成功,参数为:" + json + "\r\n");
                return "success";
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除失败,异常信息:" + ex.Message + "\r\n");
                return ex.Message;
            }
        }


        /// <summary>
        /// 生成任务
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpGet]
        public string BulidTask()
        {
            try
            {
                new StaffInfoBLL().SendTaskInfo();
                return "success";
            }
            catch (Exception ex)
            {
                return "error" + ex.Message;
            }
        }
        #endregion
    }
}