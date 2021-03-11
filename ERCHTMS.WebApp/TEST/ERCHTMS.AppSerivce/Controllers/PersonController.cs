using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ERCHTMS.Busines.OccupationalHealthManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class PersonController : BaseApiController
    {
        ERCHTMS.Busines.BaseManage.UserBLL userBll = new ERCHTMS.Busines.BaseManage.UserBLL();
        ERCHTMS.Busines.BaseManage.DepartmentBLL deptBll = new ERCHTMS.Busines.BaseManage.DepartmentBLL();
        private TransferBLL transferbll = new TransferBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        /// <summary>
        /// 14.人员列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            try
            {
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;

                string username = dy.data.username;//姓名
                string deptid = dy.data.deptid;//部门
                string card = dy.data.idcard;//身份证
                long pageIndex = dy.data.pageIndex;
                long pageSize = dy.data.pageSize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                pagination.p_kid = "u.USERID";
                pagination.p_fields = "REALNAME,MOBILE,DEPTNAME,usertype,GENDER,identifyid,case when headicon is null then '' else ('" + path + "' || headicon) end faceurl";
                pagination.p_tablename = "v_userinfo u";
                pagination.conditionJson = "Account!='System'";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), "ea93dc6b-83fc-4ac2-a1b7-56ef6909445c", "departmentcode", "organizecode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }

                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    userName = username,
                    idCard = card,
                    departmentId = deptid
                });
                var data = new UserBLL().GetPageList(pagination, queryJson);
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 扫码获取人员相关信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.uid;
                bool IsPermission = false;//是否有权限
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = userBll.GetUserInfoEntity(id);   //获取用户基本信息

                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                string path = itemBll.GetItemValue("imgUrl");
                var cert = new ERCHTMS.Busines.PersonManage.CertificateBLL().GetList(id).Select(t => new { t.CertName, t.CertNum, SendDate = t.SendDate.Value.ToString("yyyy-MM-dd"), t.Years, t.SendOrgan, FilePath = path + t.FilePath }).ToList();//获取人员证书信息
                var wzInfo = new DesktopBLL().GetWZInfoByUserId(id);//违章信息
                var health = new ERCHTMS.Busines.OccupationalHealthManage.OccupationalstaffdetailBLL().GetUserTable(id);//职业病信息
                //20190326 fwz 新增查询个人接触职业危害因素
                string Hazardfactor = "";
                //判断是否是本人\厂领导\EHS部与人力资源部的人
                if (userId == id)
                {
                    IsPermission = true;
                }
                //获取当前操作用户
                var Appuser = userBll.GetUserInfoEntity(userId);   //获取用户基本信息
                //EHS部与人力资源部配置在字典中 通过字典查找
                var Perdeptname = Appuser.DepartmentCode;
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'SelectDept'").ToList();
                if (data != null)
                {
                    foreach (var Peritem in data)
                    {
                        string value = Peritem.ItemValue;
                        string[] values = value.Split('|');
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] == Perdeptname) //如果部门编码对应则是有权限的人
                            {
                                IsPermission = true;
                                break;
                            }
                        }
                    }
                }

                //如果是厂领导也有权限
                if (Appuser.RoleName.Contains("厂级部门用户") && Appuser.RoleName.Contains("公司领导"))
                {
                    IsPermission = true;
                }

                if (IsPermission)
                {
                    Hazardfactor = new HazardfactoruserBLL().GetUserHazardfactor(user.Account);
                }

                var work = new WorkRecordBLL().GetList(id).Select(t => new { EnterDate = t.EnterDate.ToString().Contains("0001") ? "" : t.EnterDate.ToString("yyyy-MM-dd"), LeaveTime = t.LeaveTime.ToString().Contains("0001") ? "" : t.LeaveTime.ToString("yyyy-MM-dd"), t.DeptName, t.PostName, t.OrganizeName, t.JobName }).ToList();//工作记录
                string deptname = new TransferBLL().GetDeptName(user.DepartmentId);
                string projectName = "";
                if (!string.IsNullOrEmpty(user.ProjectId))
                {
                    OutsouringengineerEntity entity = new OutsouringengineerBLL().GetEntity(user.ProjectId);
                    if (entity != null)
                    {
                        projectName = entity.ENGINEERNAME;
                    }

                }

                string photo = "";
                if (!string.IsNullOrEmpty(user.HeadIcon))
                {

                    if (!string.IsNullOrEmpty(path))
                    {
                        photo = path + user.HeadIcon;
                    }

                }
                int score = 100;
                var item = itemBll.GetEntity("csjf");
                if (item != null)
                {
                    score = int.Parse(item.ItemValue);
                }

                int point = 0;
                string argValue = new DataItemDetailBLL().GetItemValue("Point");
                if (!string.IsNullOrEmpty(argValue) && argValue == "point")
                {
                    dynamic pointdata = UpdatePoint(user.Account);
                    if (pointdata != null && pointdata.Qualitydata.Count > 0)
                    {
                        point = pointdata.Qualitydata[0].point;
                    }
                }


                //2019-03-08 Fwz修改 加入职务字段 部门显示改为层级显示
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = new { realname = user.RealName, point = point, sex = user.Gender, identifyid = user.IdentifyID, deptname = deptname, postname = user.DutyName, dutyname = user.PostName, organizename = user.OrganizeName, native = user.Native, nation = user.Nation, isSpecial = user.IsSpecial, isSpecialEqu = user.IsSpecialEqu, isBlack = user.IsBlack, mobile = user.Mobile, usertype = user.UserType, isEpiboly = user.isEpiboly, enterDate = user.EnterTime == null ? "" : user.EnterTime.Value.ToString("yyyy-MM-dd"), leaveTime = user.DepartureTime == null ? "" : user.DepartureTime.Value.ToString("yyyy-MM-dd"), isPresence = user.isPresence, score = new UserScoreBLL().GetUserScore(user.UserId, DateTime.Now.Year.ToString()) + score, projectName = projectName, faceUrl = photo, CertInfo = cert, IllInfo = health, Hazardfactor = Hazardfactor, IsPermission = IsPermission, WorkInfo = work, IllegalInfo = wzInfo, isfourperson = user.IsFourPerson, fourpersontype = user.FourPersonType } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 同步培训平台人员素质意识值
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="fileName"></param>
        public dynamic UpdatePoint(string userAccount)
        {
            try
            {
                var trainserviceurl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TrainServiceUrl");
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                //发送请求到web api并获取返回值，默认为post方式
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Business = "GetQualityObj",
                    userAccount = userAccount
                });
                nc.Add("json", queryJson);
                byte[] arr = wc.UploadValues(new Uri(trainserviceurl), nc);
                return JsonConvert.DeserializeObject<ExpandoObject>(System.Text.Encoding.Default.GetString(arr));
            }
            catch (Exception e)
            {
                return null;
            }
           
        }


        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SetBlacklist([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string uId = dy.data.uid; //人员Id
                string reason = dy.data.reason;//加入黑名单原因
                OperatorProvider.AppUserId = userId;  //设置当前用户
                new BlacklistBLL().SaveForm("", new BlacklistEntity
                {
                    UserId = uId,
                    Reason = reason,
                    JoinTime = DateTime.Now
                });
                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 积分明细
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object IntegralInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string uId = dy.data.uid; //人员Id
                OperatorProvider.AppUserId = userId;  //设置当前用户
                UserEntity user = new UserBLL().GetEntity(uId);
                DataTable dt = new UserScoreBLL().GetList(uId);

                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity("csjf");
                decimal score = new UserScoreBLL().GetUserScore(uId, DateTime.Now.Year.ToString());
                score = entity == null ? score : int.Parse(entity.ItemValue) + score;
                return new { Code = 0, Count = 0, Info = "操作成功", data = new { score = score, Details = dt } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取考核项目
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getItems([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100;
                pagination.p_kid = "id as itemid";
                pagination.p_fields = "ItemName,itemtype,score";
                pagination.p_tablename = "BIS_SCORESET";
                pagination.conditionJson = "isauto=0";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.IsSystem)
                {
                    pagination.conditionJson += " and deptcode='00'";
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (deptcode='00' or deptcode='{0}')", user.OrganizeCode);
                }

                var data = new UserScoreBLL().GetPageJsonList(pagination, "");

                return new { Code = 0, Count = 0, Info = "操作成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取考核项目详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getItemInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.itemid;
                OperatorProvider.AppUserId = userId;  //设置当前用户

                var entity = new ScoreSetBLL().GetEntity(id);

                return new { Code = 0, Count = 0, Info = "操作成功", data = new { itemid = id, itemname = entity.ItemName, score = entity.Score, itemtype = entity.ItemType } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///登记积分
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object addIntegral([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string uId = dy.data.uid; //人员Id
                string itemid = dy.data.itemid;
                long score = dy.data.score;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ScoreSetEntity ss = new ScoreSetBLL().GetEntity(itemid);
                UserScoreEntity us = new UserScoreEntity
                {
                    Id = System.Guid.NewGuid().ToString(),
                    UserId = uId,
                    ItemId = itemid,
                    Score = ss.ItemType == "加分" ? decimal.Parse(score.ToString()) : decimal.Parse("-" + score.ToString()),
                    Year = System.DateTime.Now.Year.ToString(),
                    CreateDate = System.DateTime.Now,
                    CreateUserId = userId,
                    CreateUserDeptCode = user.DeptCode,
                    CreateUserOrgCode = user.OrganizeCode
                };
                new UserScoreBLL().SaveForm("", us);

                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 上传用户签名图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object uploadSign()
        {
            try
            {
                var dd = new DataItemDetailBLL();
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                long mode = 0;
                if (res.Contains("mode"))
                {
                    mode = dy.data.mode;
                }
                HttpFileCollection files = ctx.Request.Files;//签名图片 
                string userId = dy.userid; //新增类型

                string path = dd.GetItemValue("imgPath") + "\\Resource\\sign";
                string imgurl = dd.GetItemValue("imgUrl");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string ext = System.IO.Path.GetExtension(files[0].FileName);
                string fileName = Guid.NewGuid().ToString() + ".png";
                files[0].SaveAs(path + "\\" + fileName);
                if (mode == 0)
                {
                    userBll.UploadSignImg(userId, "/Resource/sign/" + fileName);
                }

                string bzAppUrl = new DataItemDetailBLL().GetItemValue("bzAppUrl");
                if (!string.IsNullOrEmpty(bzAppUrl))
                {
                    UpdateSign(userId, fileName, path, imgurl, bzAppUrl);
                }

                return new { Code = 0, Count = 0, Info = "操作成功", data = new { signUrl = imgurl + "/Resource/sign/" + fileName } };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };

            }
        }

        /// <summary>
        /// 班组同步签名信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="fileName"></param>
        public void UpdateSign(string userid, string fileName, string path, string imgurl, string bzAppUrl)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                System.IO.File.AppendAllText(path + "\\" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步成功" + "\r\n");
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    userid = userid,
                    filepath = imgurl + "/Resource/sign/" + fileName
                });
                nc.Add("json", json);
                wc.UploadValuesAsync(new Uri(bzAppUrl + "UpdateUrl"), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "~/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：数据失败" + ",异常信息：" + ex.Message + "\r\n");
            }

        }

        /// <summary>
        /// 人员离职
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object leaveJob()
        {
            try
            {
                var dd = new DataItemDetailBLL();
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //用户Id
                string time = dy.data.time;//离职时间
                string Reson = dy.data.reson;//离职原因
                int count = userBll.SetLeave(userId, time, Reson);
                //离职后生成工作记录
                new WorkRecordBLL().EditRecord(userId, time);
                string msg = count > 0 ? "操作成功" : "操作失败,请确认该人员是否在系统中存在！";
                return new { Code = 0, Info = msg };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message };

            }
        }
        /// <summary>
        /// 人员调岗
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object changeJob()
        {
            try
            {
                var dd = new DataItemDetailBLL();
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //用户Id
                string postId = dy.data.postId;//新的岗位Id
                string postName = dy.data.postName;//新的岗位名称
                string dutyId = dy.data.dutyId;//新的职务Id
                string dutyName = dy.data.dutyName;//新的职务名称
                string time = dy.data.time;//转岗时间
                string deptId = dy.data.deptId;//新的部门
                int count = userBll.LeavePost(userId, deptId, postId, postName, dutyId, dutyName, time);
                string msg = count > 0 ? "操作成功" : "操作失败,请确认该人员是否在系统中存在！";
                return new { Code = 0, Info = msg };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message };

            }
        }

        /// <summary>
        /// 人员调岗
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Transfer()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string data = JsonConvert.SerializeObject(dy.data);
                TransferEntity entity = JsonConvert.DeserializeObject<TransferEntity>(data);
                string uid = entity.UserId;//获取到需要转岗的用户id
                //是否是新增 0是新增 1是修改
                string isNew = dy.isNew;
                UserEntity ue = userBll.GetEntity(uid);
                entity.InDeptId = ue.DepartmentId;
                entity.InDeptName = deptBll.GetEntity(ue.DepartmentId).FullName;
                entity.InDeptCode = ue.DepartmentCode;
                entity.InJobId = ue.PostId;
                entity.InJobName = ue.PostName;
                entity.InPostId = ue.DutyId;
                entity.InPostName = ue.DutyName;
                transferbll.AppSaveForm(isNew, entity, dy.userId);
                return new { Code = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = ex.Message };

            }
        }

        [HttpGet]
        public object GetTrainResult(string IdCard)
        {
            try
            {
                SstmsService.CheckHeader header = new SstmsService.CheckHeader();
                header.Key = "C8E52648921869CE5BF5C80569046C11";
                // 实例化服务对象
                SstmsService.DataServiceSoapClient service = new SstmsService.DataServiceSoapClient();
                SstmsService.Depart[] data = service.GetDeparts(header);

                //string json = "{\"Examdata\":[{\"PARTYID\":\"1785106885\",\"USERNAME\":\"冯亮\",\"DEPARTID\":\"-1014595119\",\"USERACCOUNT\":\"20018576\",\"DEPARTNAME\":\"人事\",\"EXAMNAME\":\"2016年非生产人员安全培训_考试\",\"EXAMSTARTTIME\":\"2016-05-31T00:00:00\",\"EXAMENDTIME\":\"2016-07-30T23:59:00\",\"TRAINPLANCODE\":\"1766144250\",\"POINT\":67.0,\"STATE\":\"合格\",\"CREATEDATE\":\"2016-06-16T10:30:09\",\"PARENTID\":\"1781654785\",\"TIMES\":null,\"PASSLINE\":\"60\",\"EXAMID\":\"-1065522118\",\"CREATENAME\":\"沙角B管理员\"},{\"PARTYID\":\"1785106885\",\"USERNAME\":\"冯亮\",\"DEPARTID\":\"-1014595119\",\"USERACCOUNT\":\"20018576\",\"DEPARTNAME\":\"人事\",\"EXAMNAME\":\"2017年非生产人员安全培训考试补考2\",\"EXAMSTARTTIME\":\"2017-08-10T15:45:00\",\"EXAMENDTIME\":\"2017-09-01T15:43:00\",\"TRAINPLANCODE\":\"1766144250\",\"POINT\":null,\"STATE\":null,\"CREATEDATE\":null,\"PARENTID\":\"1781654785\",\"TIMES\":null,\"PASSLINE\":\"60\",\"EXAMID\":\"24380e16-bd6e-4a0e-8c1c-7cc97d8fe721\",\"CREATENAME\":\"沙角B管理员\"}]}";
                //var dtExams = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Examdata\":", "").TrimEnd('}'));
                //if (string.IsNullOrEmpty(IdCard))
                //{
                //    return new { code = 0, result = 3, message = "身份证号不允许为空" };
                //}
                //if(!BSFramework.Util.ValidateUtil.IsIdCard(IdCard))
                //{
                //    return new { code = 0, result = 3, message = "身份证格式不正确" };
                //}
                //var user = userBll.GetUserByIdCard(IdCard);
                //if(user==null)
                //{
                //    return new { code = 0,result=4, message = "该人员信息不存在" };
                //}
                //bool flag=new AptitudeinvestigatepeopleBLL().IsAuditByUserId(user.UserId);
                //if (1!=1)
                //{
                //    return new { code = 0, result = 2, message = "资质未审核通过" };
                //}
                //else
                //{
                //    //json = userBll.GetExamRecord(user.Account, user.DepartmentId);
                //    if (json.Length > 30)
                //    {
                //        //dtExams = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Examdata\":", "").TrimEnd('}'));
                //        var rows = dtExams.Select("point is not null");     
                //        if (rows.Count() > 0)
                //        {
                //            if (int.Parse(rows[0]["point"].ToString()) < int.Parse(rows[0]["passline"].ToString()))
                //            {
                //                return new { code = 0, result =2, message = "培训考试成绩不合格" };
                //            }
                //            else
                //            {
                //                return new { code = 0, result =1, message = "培训考试及资质通过" };
                //            }
                //        }
                //        else
                //        {
                //            return new { code = 0, result = 0, message = "未参加培训考试，没有查询到记录" };
                //        }
                //     }
                //    else
                //    {
                //        return new { code = 0, result = 0, message = "未参加培训考试，没有查询到记录" };
                //    }
                //}
                return new { Code = 0, Info = data };
            }
            catch (Exception ex)
            {
                return new { code = 0, result = 5, message = ex.Message };

            }
        }
        [HttpGet]
        public object SyncToolsDepts()
        {
            try
            {
                DataTable dt = deptBll.GetDataTable(string.Format("select deptid,unitid,keys from BIS_TOOLSDEPT"));
                foreach (DataRow dr in dt.Rows)
                {
                    DepartmentEntity dept = deptBll.GetEntity(dr[0].ToString());
                    if (dept != null)
                    {
                        SstmsService.CheckHeader header = new SstmsService.CheckHeader();
                        header.Key = dr[2].ToString();
                        List<SstmsService.DictionaryEntry> dic = new List<SstmsService.DictionaryEntry>();
                        SstmsService.DictionaryEntry de = new SstmsService.DictionaryEntry();
                        de.Key = "LaterID";
                        de.Value = dr[1].ToString();
                        dic.Add(de);
                        de = new SstmsService.DictionaryEntry();
                        de.Key = "Identify";
                        de.Value = "";
                        dic.Add(de);
                        // 实例化服务对象
                        SstmsService.DataServiceSoapClient service = new SstmsService.DataServiceSoapClient();
                        SstmsService.PersonInfo[] data = service.GetPersonsPage(header, dic.ToArray<SstmsService.DictionaryEntry>(), 1, 1000).List;
                        DepartmentEntity org = deptBll.GetEntity(dept.OrganizeId);
                        foreach (SstmsService.PersonInfo per in data)
                        {
                            string unitId = per.OwnerDeptID;
                            string userId = "";
                            string roleName = "承包商级用户";
                            string roleId = "c5530ccf-e84e-4df8-8b27-fd8954a9bbe9";

                            if (dt.Rows.Count > 0)
                            {
                                string nature = dept.Nature;
                                switch (nature)
                                {
                                    case "部门":
                                        roleName = "部门级用户";
                                        roleId = "6c094cef-cca3-4b41-a71b-6ee5e6b89008";
                                        break;
                                    case "专业":
                                        roleName = "专业级用户";
                                        roleId = "e3062d59-2484-4046-a420-478886d58656";
                                        break;
                                    case "班组":
                                        roleName = "班组级用户";
                                        roleId = "d9432a6e-5659-4f04-9c10-251654199714";
                                        break;
                                    case "厂级":
                                        roleName = "公司级用户";
                                        roleId = "aece6d68-ef8a-4eac-a746-e97f0067fab5";
                                        break;
                                    case "省级":
                                        roleName = "省级用户";
                                        roleId = "9a834c93-ff60-440e-845d-79b311eeacae";
                                        break;
                                }
                                roleName += ",普通用户";
                                roleId += ",2a878044-06e9-4fe4-89f0-ba7bd5a1bde6";
                                UserEntity user = userBll.GetUserByIdCard(per.IdentifyID);
                                if (user == null)
                                {
                                    //    userId = user.UserId;
                                    //}
                                    //else
                                    //{
                                    user = new UserEntity();
                                    user.UserId = Guid.NewGuid().ToString();
                                    user.Account = per.IdentifyID;
                                    userId = user.UserId;
                                    // }

                                    user.MSN = "1";
                                    user.UserType = "一般工作人员";
                                    user.EnCode = per.TraID;
                                    user.Degrees = user.DegreesID = per.Degrees;
                                    user.Birthday = per.BirthDay;
                                    user.RoleId = roleId;
                                    user.RoleName = roleName;
                                    user.IdentifyID = per.IdentifyID;
                                    user.Gender = per.Sex;
                                    user.Nation = per.Nation;
                                    user.Email = per.Email;
                                    user.EnterTime = per.EntranceDate;
                                    user.RealName = per.PersonName;
                                    user.Degrees = per.Degrees;
                                    user.Native = per.Native;
                                    user.DepartureTime = per.LeaveDate;
                                    user.Telephone = per.TelPhone;
                                    user.IsPresence = per.IsOut == "是" ? "0" : "1";
                                    user.Password = "123456";
                                    user.DepartmentId = dept.DepartmentId;
                                    user.DepartmentCode = dept.EnCode;
                                    user.OrganizeId = dept.OrganizeId;
                                    user.OrganizeCode = org.EnCode;
                                    user.Craft = per.Category;
                                    user.IsEpiboly = roleName.Contains("承包商") || roleName.Contains("分包商") ? "1" : "0";
                                    if (user.ModifyDate != null)
                                    {
                                        if (per.OperDate > user.ModifyDate)
                                        {
                                            userBll.SaveForm(userId, user);
                                        }
                                    }
                                    else
                                    {
                                        userBll.SaveForm(userId, user);
                                    }
                                }
                            }

                        }
                    }

                }
                Task.Factory.StartNew(() =>
                {
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/") + fileName, string.Format("{0}:同步工具箱人员信息成功\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                });
                return new { Code = 1, message = "同步工具箱人员信息成功" };
            }
            catch (Exception ex)
            {
                Task.Run(() =>
                {
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/") + fileName, string.Format("{0}:{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message));

                });
                return new { code = 0, message = ex.Message };
            }
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
                            var tempdata = new
                            {
                                account = dy.mobile,//账号
                                password = str,
                                allowEndTime = Convert.ToDateTime(user.AllowEndTime).ToString("yyyy-MM-dd HH:mm:ss"),
                                path = pathurl
                            };
                            userBll.SaveOnlyForm(user.UserId, user);
                            //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,注册成功,参数为:" + json + "\r\n");
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
                //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,注册失败,异常信息:" + ex.Message + "参数为:" + json + "\r\n");
                return new { code = 1, data = "", info = ex.Message };
            }
        }

    }
}