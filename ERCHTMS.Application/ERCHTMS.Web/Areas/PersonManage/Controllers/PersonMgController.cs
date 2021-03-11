using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.PersonManage;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 门岗管理
    /// </summary>
    public class PersonMgController : MvcControllerBase
    {
        private UserBLL userBLL = new UserBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
        private TemporaryGroupsBLL Tempbll = new TemporaryGroupsBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 页面视图
        /// <summary>
        /// 门岗管理列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增临时人员
        /// </summary>
        /// <returns></returns>
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 权限管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Jurisdiction()
        {
            return View();
        }
        /// <summary>
        /// 查看详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowDetails()
        {
            return View();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EditDetails()
        {
            return View();
        }

        /// <summary>
        /// 加入禁用名单
        /// </summary>
        /// <returns></returns>
        public ActionResult Forbidden()
        {
            return View();
        }
        /// <summary>
        /// 禁入名单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DebarUserList()
        {
            return View();
        }
        /// <summary>
        /// 临时分组管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Grouping()
        {
            return View();
        }
        /// <summary>
        /// 考勤周期
        /// </summary>
        /// <returns></returns>
        public ActionResult Cycle()
        {
            return View();
        }

        /// <summary>
        ///临时人员考勤周期
        /// </summary>
        /// <returns></returns>
        public ActionResult TempCycleFrom()
        {
            return View();
        }
        /// <summary>
        ///选择临时人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TempCycleList()
        {
            return View();
        }
        /// <summary>
        /// 临时人员批量导入
        /// </summary>
        /// <returns></returns>
        public ActionResult TempImport()
        {
            return View();
        }


        #endregion

        #region 数据获取

        /// <summary>
        /// 获取选中禁入名单用户信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpPost]
        public ActionResult GetCheckUserList(string userids, string type)
        {
            string[] users = userids.Split(',');
            string userid = "";
            for (int i = 0; i < users.Length; i++)
            {
                if (i == 0)
                {
                    userid = "'" + users[i] + "'";
                }
                else
                {
                    userid += ",'" + users[i] + "'";
                }
            }
            string sql = string.Empty;
            if (type == "1")
            {//临时人员
                sql = string.Format(@" select userid, USERNAME as realname,Gender,Identifyid,COMNAME as organizename,GROUPSNAME as deptname,'临时人员' as usertype,GROUPSID,TEL as mobile,1 as datatype，createdate  from BIS_temporaryUser u  where userid in ({0})", userid);

            }
            else
            {//电厂人员
                sql = string.Format(@" select userid, RealName,Gender,Mobile,OrganizeName,DeptName,DutyName,createdate from v_userinfo   where userid in ({0})", userid);

            }
            var data = Opertickebll.GetDataTable(sql);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 组成员
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult GroupUserCount(string groupId, string type)
        {
            string num = string.Empty;
            string sql = string.Format("select count(1) from BIS_temporaryUser d where d.groupsid='{0}'", groupId);
            var data = Opertickebll.GetDataTable(sql);
            if (data.Rows.Count > 0)
            {
                num = data.Rows[0][0].ToString();
            }
            return Content(num);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string userId)
        {
            var data = Tempbll.GetList(userId).ToList();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取人员禁入记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageForbiddenRecordJson(Pagination pagination, string userid)
        {
            pagination.p_kid = "id";
            pagination.p_fields = "d.starttime,d.endtime,d.remark";
            pagination.p_tablename = " BIS_ForbiddenRecord d";
            pagination.conditionJson = "d.UserId='" + userid + "'";

            var watch = CommonHelper.TimerStart();
            var data = new UserBLL().GetPageList(pagination, null);
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

        /// <summary>
        /// 禁入名单列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageDebarUserlistJson(Pagination pagination, string queryJson)
        {

            pagination.p_kid = "userid";
            pagination.p_fields = "username as realname,starttime,postname,endtime,d.remark,gender,identifyid,comname as organizename,t.groupsname as deptname,d.groupsname,groupsid,tel as mobile,istemporary";
            pagination.p_tablename = " bis_temporaryuser  d left join bis_temporarygroups t on d.groupsid=t.id  ";
            pagination.conditionJson = "d.isdebar=1 ";
            //查询条件
            var queryParam = queryJson.ToJObject();
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString().Trim();
                switch (condition)
                {
                    case "RealName":          //姓名
                        pagination.conditionJson += string.Format(" and d.USERNAME  like '%{0}%'", keyord);
                        break;
                    case "DeptName":          //部门名称
                        pagination.conditionJson += string.Format(" and d.GROUPSNAME like '%{0}%'", keyord);
                        break;
                    case "identifyid":          //身份证号
                        pagination.conditionJson += string.Format(" and d.IDENTIFYID like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = new UserBLL().GetPageList(pagination, null);
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

        /// <summary>
        /// 选择临时人员列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageTempUserlistJson(Pagination pagination, string GroupId)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "USERNAME as realname,Gender,Identifyid,COMNAME as organizename,t.GROUPSNAME as deptname,'临时人员' as usertype,GROUPSID,TEL as mobile,1 as datatype,u.starttime,u.endtime,u.passpost";
            pagination.p_tablename = " BIS_temporaryUser u join bis_temporarygroups t on u.groupsid=t.id ";
            pagination.conditionJson = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1 and Istemporary=1 and ISDebar=0 and GROUPSID='" + GroupId + "' ";
            var data = Tempbll.GetPageList(pagination, null);
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

        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "id";
            pagination.p_fields = "d.groupsname,d.groupscode,a.ucount,0 as state";
            pagination.p_tablename = @" BIS_TEMPORARYGROUPS d
              left join (select count(1) as ucount, groupsid
              from BIS_temporaryUser t
              group by t.groupsid) a
              on d.id = a.groupsid";
            pagination.conditionJson = "d.isenable='1'";

            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var watch = CommonHelper.TimerStart();
            var data = new UserBLL().GetPageList(pagination, queryJson);
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


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson1(Pagination pagination, string queryJson, int mode = 0)
        {
            var queryParam = queryJson.ToJObject();
            if (!queryParam["CarType"].IsEmpty())
            {//临时人员
                string userKind = queryParam["CarType"].ToString();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "u.USERID";
                pagination.p_fields = "USERNAME as realname,Gender,Identifyid,COMNAME as organizename,t.GROUPSNAME as deptname,'临时人员' as usertype,groupsid,TEL as mobile,1 as datatype,u.starttime as createdate ,u.endtime,u.passpost";
                pagination.p_tablename = " BIS_temporaryUser u join bis_temporarygroups t on u.groupsid=t.id ";
                pagination.conditionJson = "";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = "1=1 and Istemporary=1 and ISDebar=0 and GROUPSID='" + userKind + "' ";
                }
                if (queryParam["ImgType"].ToString() == "未上传人脸")
                {
                    pagination.conditionJson += " and  u.userimg is null ";
                }
                if (queryParam["ImgType"].ToString() == "未授权")
                {
                    pagination.conditionJson += " and  u.passpost is null ";
                }
                //--------此判断用于筛选考勤时间过期的人员
                if (queryParam["ImgType"].ToString() == "考勤过期")//判断考勤时间是否小于当前时间是则为考勤过期
                {
                    pagination.p_tablename += " LEFT JOIN (select endtime,USERID  from BIS_TEMPORARYUSER WHERE USERID is not NULL ) bt  on bt.userid=u.userid  ";
                    pagination.conditionJson += "AND bt.ENDTIME is not null AND bt.ENDTIME < SYSDATE  ";

                }
                var data = Tempbll.GetPageList(pagination, queryJson);
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
            else
            {//电厂用户
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "u.USERID";
                pagination.p_fields = " '' as endtime ,'' as passpost,Account,senddeptid,REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,CreateDate,isblack,identifyid,score,IsPresence,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,u.IsTransfer,u.DepartureReason,u.fourpersontype,1 isleave,IsEpiboly";
                pagination.p_tablename = "v_userinfo u left join (select a.userid,sum(score) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='" + DateTime.Now.Year + "' group by a.userid) t on u.userid=t.userid";
                pagination.conditionJson = "Account!='System' and u.EnabledMark=1 and u.userid not in (select userid from BIS_temporaryUser rr where rr.isdebar=1) ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {

                    if (queryParam["datatype"].IsEmpty())
                    {
                        string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                        if (!string.IsNullOrEmpty(where) && (queryParam["code"].IsEmpty() || !queryJson.Contains("code")))
                        {
                            pagination.conditionJson += " and " + where;
                        }
                    }
                }
                if (queryParam["ImgType"].ToString() == "未上传人脸")
                {
                    pagination.conditionJson += " and u.userid in (select userid from BIS_temporaryUser uu where uu.userimg is null)  ";

                }
                if (queryParam["ImgType"].ToString() == "未授权")
                {
                    pagination.conditionJson += " and u.userid in (select userid from BIS_temporaryUser uu where uu.passpost is null)  ";
                }
                // pagination.conditionJson += string.Format(" or u.userid in(select userid from base_user where departmentid in( select t.departmentid from BASE_DEPARTMENT t where t.senddeptid='{0}'))", user.DeptId);
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(user.OrganizeId);

                Dictionary<string, string> dic = new CertificateBLL().GetOverdueCertList(pagination.conditionJson);
                //--------此判断用于筛选考勤时间过期的人员
                if (queryParam["ImgType"].ToString() == "考勤过期")//判断考勤时间是否小于当前时间是则为考勤过期
                {
                    pagination.p_tablename += " LEFT JOIN (select endtime,USERID  from BIS_TEMPORARYUSER WHERE USERID is not NULL ) bt  on bt.userid=u.userid  ";
                    pagination.conditionJson += "AND bt.ENDTIME is not null AND bt.ENDTIME < SYSDATE  ";

                }
                var data = userBLL.GetPageList(pagination, queryJson);
                string softName = BSFramework.Util.Config.GetValue("SoftName").ToLower();
                List<TemporaryUserEntity> tempuserList = new TemporaryGroupsBLL().GetUserList();//所有临时人员
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
                    if (dr["IsEpiboly"].ToString() == "是" && softName == "xss")
                    {
                        DataTable dtItems = departmentBLL.GetDataTable(string.Format("select remark from XSS_ENTERRECORD where idcard='{0}' order by time desc", dr["identifyid"].ToString()));
                        if (dtItems.Rows.Count > 0)
                        {
                            dr["isleave"] = dtItems.Rows[0][0].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(dr["USERID"].ToString()))
                    {
                        var tempuser = tempuserList.Where(t => t.USERID == dr["USERID"].ToString()).FirstOrDefault();
                        if (tempuser != null)
                        {
                            dr["endtime"] = tempuser.EndTime;
                            dr["passpost"] = tempuser.PassPost;
                            if (!string.IsNullOrEmpty(tempuser.startTime.ToString()))
                            {
                                dr["createdate"] = tempuser.startTime;
                            }
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
        }




        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Tempbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取电厂所有的设备区域
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeviceArea()
        {
            var areaList = new HikdeviceBLL().GetDeviceArea();
            return Json(areaList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 表单提交
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            Tempbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 提交表单（保存、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="ArrList">新增</param>
        /// <param name="Updatelist">修改</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, List<TemporaryGroupsEntity> ArrList, List<TemporaryGroupsEntity> Updatelist)
        {
            Tempbll.SaveForm(keyValue, ArrList, Updatelist);
            return Success("操作成功。");
        }

        #endregion


        #region 临时人员
        /// <summary>
        /// 调用摄像头拍照人脸是否合格
        /// </summary>
        /// <param name="UserEntity"></param>
        /// <param name="imgData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TempUserFaceImgIsQualified(TemporaryUserEntity UserEntity, string imgData)
        {
            try
            {
                #region 获取海康地址和秘钥
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
                #endregion
                imgData = HttpUtility.UrlDecode(imgData);
                DataItemDetailBLL dd = new DataItemDetailBLL();
                string path = dd.GetItemValue("imgPath") + "\\Resource\\" + "TemporaryUser";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileurl = string.Empty;
                string sdd = imgData.Substring(0, imgData.IndexOf(',') + 1);
                string base64 = imgData.Replace(sdd, "");
                byte[] arr2 = Convert.FromBase64String(base64);
                using (MemoryStream ms2 = new MemoryStream(arr2))
                {//将png格式图片存储为jpg格式文件
                    System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                    bmp2 = KiResizelmage(bmp2, 600, 450);
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    bmp2.Save(path + "\\" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fileurl = "\\Resource\\" + "TemporaryUser" + "\\" + fileName;
                }
                string srcPath = Server.MapPath("~" + fileurl);
                if (System.IO.File.Exists(srcPath))
                {
                    //读jpg图片转为Base64String
                    var nbase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(srcPath));
                    UserEntity.ImgData = nbase64;
                }
                //调用海康平台接口验证人脸照片是否合格
                string msg = SocketHelper.FaceImgIsQualified(UserEntity.ImgData, baseurl, Key, Signature);
                if (msg != null && !string.IsNullOrEmpty(msg))
                {
                    FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                    if (!ress.data.checkResult)
                    {
                        return Success("false", fileurl);
                    }
                }
                return Success("操作成功", fileurl);
            }
            catch (Exception ex)
            {
                return Success(ex.Message);
            }
        }

        /// <summary>
        /// 设置人脸图片大小
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="newW"></param>
        /// <param name="newH"></param>
        /// <returns></returns>
        public Bitmap KiResizelmage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 临时人员保存
        /// </summary>
        /// <param name="UserEntity"></param>
        /// <param name="imgData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUForm(TemporaryUserEntity UserEntity, string imgData)
        {
            try
            {
                string fileurl = string.Empty;
                if (!string.IsNullOrEmpty(UserEntity.UserImg))
                {
                    string srcPath = Server.MapPath("~" + UserEntity.UserImg);
                    if (System.IO.File.Exists(srcPath))
                    {
                        //读jpg图片转为Base64String
                        var nbase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(srcPath));
                        UserEntity.ImgData = nbase64;
                    }
                }
                UserEntity.Istemporary = 1;
                var baselist = Tempbll.SaveUForm("", UserEntity);
                return Content(baselist.ToJson());
            }
            catch (Exception er)
            {
                return Success("操作失败。");
            }
        }



        /// <summary>
        /// 用户是否有人脸信息
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IsHaveFace(string userids)
        {
            int count = 0;//总记录
            int yes = 0;//成功
            string res = string.Empty;
            string str = string.Empty;
            string ids = string.Empty;
            TemporaryUserEntity entity = new TemporaryUserEntity();
            if (!string.IsNullOrEmpty(userids))
            {
                foreach (var item in userids.Split(','))
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    count++;
                    var data = Tempbll.GetEmptyUserEntity(item);
                    if (data != null)
                    {//临时表
                        if (string.IsNullOrEmpty(data.UserImg))
                        {
                            res += data.UserName + ",";
                        }
                        else
                        {
                            ids += item + ",";
                            yes++;
                        }
                    }
                    else
                    {//用户表
                        var users = new UserBLL().GetEntity(item);
                        if (users != null)
                        {
                            str += users.RealName + ",";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(res)) { res = res.TrimEnd(','); }
            if (!string.IsNullOrEmpty(str)) { str = str.TrimEnd(','); }
            if (!string.IsNullOrEmpty(ids)) { ids = ids.TrimEnd(','); }
            entity.UserImg = res;
            entity.UserName = str;
            entity.USERID = ids;
            entity.Remark = yes + "/" + count;
            return ToJsonResult(entity);
        }



        /// <summary>
        /// 电厂用户详情中修改单个权限信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="Imgpath"></param>
        /// <returns></returns>
        public ActionResult SaveuplodFace(string userids, string enttime, string starttime, string passpost, string IsNewImg, string UserImg)
        {
            try
            {
                #region 获取海康地址和秘钥
                DataItemDetailBLL data1 = new DataItemDetailBLL();
                var pitem = data1.GetItemValue("Hikappkey");//海康服务器密钥
                var baseurl = data1.GetItemValue("HikBaseUrl");//海康服务器地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                #endregion
                bool Power = false;//是否过授权
                var data = Tempbll.GetEmptyUserEntity(userids);
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(UserImg))
                    {
                        string srcPath = Server.MapPath("~" + UserImg);
                        if (System.IO.File.Exists(srcPath))
                        { //读图片转为Base64String
                            var base64 = Tobase64(srcPath);
                            data.UserImg = UserImg;
                            data.ImgData = base64;
                        }
                    }
                    if (!string.IsNullOrEmpty(data.PassPost)) { Power = true; }
                    data.PassPost = passpost;
                    data.startTime = DateTime.Parse(starttime);
                    data.EndTime = DateTime.Parse(enttime);
                    data.Remark = IsNewImg;//1人脸更换0默认
                    if (!Power && IsNewImg == "1")
                    {
                        List<FacedataEntity> FaceList = new List<FacedataEntity>();
                        FacedataEntity faces = new FacedataEntity();
                        faces.UserId = data.USERID;
                        faces.ImgData = data.ImgData;
                        FaceList.Add(faces);
                        SocketHelper.UploadFace(FaceList, baseurl, Key, Signature);
                    }
                    var baselist = Tempbll.SaveUserFace(data, data.USERID, Power);
                    return Content(baselist.ToJson());
                }
                else
                {
                    return Success("没找到对应记录信息！");
                }
            }
            catch (Exception)
            {
                return Success("操作失败。");
            }
        }


        /// <summary>
        /// 批量权限设置
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCycle(string userids, string enttime, string starttime, string passpost)
        {
            try
            {
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                if (!string.IsNullOrEmpty(userids))
                {
                    foreach (var item in userids.Split(','))
                    {
                        if (string.IsNullOrEmpty(item)) continue;
                        TemporaryUserEntity entity = new TemporaryUserEntity();
                        entity.USERID = item;
                        entity.startTime = DateTime.Parse(starttime);
                        entity.PassPost = passpost;
                        entity.EndTime = DateTime.Parse(enttime);
                        list.Add(entity);
                    }
                }
                Tempbll.SaveCycle(list);
                return Success("操作成功。");
            }
            catch (Exception er)
            {
                return Success("操作失败。");
            }
        }

        /// <summary>
        /// 新批量权限下发
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveNewCycle(string userids, string enttime, string starttime, string passpost)
        {
            try
            {
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                if (!string.IsNullOrEmpty(userids))
                {
                    foreach (var item in userids.Split(','))
                    {
                        if (string.IsNullOrEmpty(item)) continue;
                        TemporaryUserEntity entity = new TemporaryUserEntity();
                        entity.USERID = item;
                        entity.startTime = DateTime.Parse(starttime);
                        entity.PassPost = passpost;
                        entity.EndTime = DateTime.Parse(enttime);
                        list.Add(entity);
                    }
                }
                var baselist = Tempbll.SaveCycle(list);
                return Content(baselist.ToJson());
            }
            catch (Exception er)
            {
                return Success("操作失败。");
            }
        }

        /// <summary>
        /// 查询任务执行进度
        /// </summary>
        /// <param name="taskIds">接口调用返回唯一标识</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuerySpeedofprogress(string taskIds)
        {
            string b = "1";
            try
            {
                if (!string.IsNullOrEmpty(taskIds))
                {//同一批中有更改有新添加查询要分别查询执行进度
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
                    var arr = taskIds.Split(',');
                    for (int i = 0; i < taskIds.Split(',').Length; i++)
                    {
                        if (arr[i] == "null") continue;
                        string taskid = arr[i].ToString();
                        //查询进度
                        string softName = BSFramework.Util.Config.GetValue("SoftName").ToLower();
                        string msg1 = string.Empty;
                        if (softName == "jnjt")
                        {
                            msg1 = SocketHelper.QuerySpeedofprogress(taskid, baseurl, Key, Signature, 0, true);
                        }
                        else
                        {
                            msg1 = SocketHelper.QuerySpeedofprogress(taskid, baseurl, Key, Signature);
                        }
                        progress p1 = JsonConvert.DeserializeObject<progress>(msg1);
                        if (p1 != null)
                        {
                            if (!p1.data.isFinished || p1.data.percent != 100)
                                b = "0";
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Content(b);
        }

        /// <summary>
        /// 根据出入权限配置快捷下载(IC卡号、人脸)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ActionResult DownloadToEquipment(List<AddJurisdictionEntity> list)
        {
            try
            {
                if (list.Count > 0)
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
                    List<resourceInfos1> downloadRes = new List<resourceInfos1>();
                    foreach (var item in list)
                    {
                        foreach (var subItem in item.resourceInfos)
                        {
                            if (!downloadRes.Exists(x => x.resourceIndexCode == subItem.resourceIndexCode))
                                downloadRes.Add(subItem);
                        }
                    }
                    Tempbll.downloadUserlimits(downloadRes, baseurl, Key, Signature);
                }
                return Success("权限下发完成。");
            }
            catch (Exception)
            {
                return Success("操作失败。");
            }

        }

        /// <summary>
        /// 批量处理根据出入权限配置快捷下载(IC卡号、人脸)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewDownloadToEquipment(List<AddJurisdictionEntity> list)
        {
            try
            {
                if (list.Count > 0)
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
                    int i = 0;
                    if (list.Count > 1) i = 1;
                    Tempbll.downloadUserlimits(list[i].resourceInfos, baseurl, Key, Signature);
                }
                return Success("权限下发完成。");
            }
            catch (Exception)
            {
                return Success("操作失败。");
            }

        }



        /// <summary>
        /// 加入禁入名单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ActionResult SaveForbidden(List<TemporaryUserEntity> list)
        {
            try
            {
                var baselist = Tempbll.SaveForbidden(list);
                return Content(baselist.ToJson());
            }
            catch (Exception er)
            {
                return Success("操作失败。");
            }
        }

        /// <summary>
        /// 移除禁入名单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ActionResult RemoveForbidden(string list)
        {
            try
            {
                var baselist = Tempbll.RemoveForbidden(list);
                return Content(baselist.ToJson());
            }
            catch (Exception er)
            {
                return Success("操作失败。");
            }
        }

        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUserJurisdiction(TemporaryUserEntity UserEntity, string userids, string type)
        {
            Tempbll.SaveUserJurisdiction(UserEntity, userids, type);
            return Success("操作成功。");
        }

        /// <summary>
        /// 获取临时人员实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetUserFormJson(string keyValue)
        {
            var data = Tempbll.GetUserEntity(keyValue);
            return ToJsonResult(data);
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
            string filename = files[0].FileName;
            string str = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.'));
            str = str.ToLower();
            if (str != null && str != ".jpg")
            {
                return Success("请上传jpg格式人脸图片！", "1");
            }
            if (files[0].ContentLength > 200 * 1024 || files[0].ContentLength < 10 * 1024)
            {
                return Success("请把文件大小控制在10kb到200kb之间！", "1");
            }
            //文件流转base64
            Stream strenm = files[0].InputStream;
            byte[] bty = new byte[strenm.Length];
            strenm.Read(bty, 0, bty.Length);
            strenm.Seek(0, SeekOrigin.Begin);
            string facePicData = Convert.ToBase64String(bty);
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
            //调用海康平台接口验证人脸照片是否合格
            string msg = SocketHelper.FaceImgIsQualified(facePicData, baseurl, Key, Signature);
            if (msg != null && !string.IsNullOrEmpty(msg))
            {
                FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                if (!ress.data.checkResult)
                {
                    return Success("人脸照片不合格，请重新上传！", "1");
                }
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

        #endregion

        #region 根据Zip导入人脸

        public ActionResult ImportFace()
        {
            return View();
        }

        public ActionResult ConversionFaces()
        {
            return View();
        }

        /// <summary>
        /// 转换人脸图片名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ConversionFace()
        {
            string PathMsg = "";
            try
            {
                string message = "";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count == 1)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    bool isZip1 = file.FileName.Substring(file.FileName.IndexOf('.')).Contains("zip");//第一个文件是否为Zip格式
                    if (!isZip1)
                    {
                        return message;
                    }
                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string time = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    string decompressionDirectory = Server.MapPath("~/Resource/ConversionFaces/") + time + "\\";
                    UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);
                    DirectoryInfo root = new DirectoryInfo(decompressionDirectory);
                    FileInfo[] files = root.GetFiles();
                    UserBLL ubll = new UserBLL();
                    List<TemporaryUserEntity> tempuserList = Tempbll.GetUserList();//所有临时人员
                    List<UserEntity> userlist = ubll.GetList().ToList();

                    List<string> deletestr = new List<string>();
                    for (int i = 0; i < files.Length; i++)
                    {
                        string[] filenames = files[i].Name.Split('.');
                        if (filenames.Length > 1)
                        {
                            if (filenames[filenames.Length - 1] != "jpg")
                            {
                                deletestr.Add(files[i].FullName);
                                message += "</br>压缩包中文件:" + files[i].Name + "不是jpg格式,请更改后再导入。";
                                continue;
                            }

                            if (files[i].Length > 200 * 1024 || files[i].Length < 10 * 1024)
                            {
                                deletestr.Add(files[i].FullName);
                                message += "</br>压缩包中文件:" + files[i].Name + "大小请控制在10kb到200kb之间,请更改后再导入。";
                                continue;
                            }


                            string name = filenames[filenames.Length - 2];
                            if (userlist.Where(it => it.RealName == name).Count() > 1)
                            {
                                deletestr.Add(files[i].FullName);
                                message += "</br>压缩包中文件:" + files[i].Name + "文件名有重名用户。";
                                continue;
                            }

                            UserEntity Us = userlist.Where(it => it.RealName == name).FirstOrDefault();
                            if (Us == null)
                            {
                                if (tempuserList.Where(it => it.UserName == name).Count() > 1)
                                {
                                    deletestr.Add(files[i].FullName);
                                    message += "</br>压缩包中文件:" + files[i].Name + "文件名有重名用户。";
                                    continue;
                                }

                                TemporaryUserEntity tempuser = tempuserList.Where(it => it.UserName == name).FirstOrDefault();
                                if (tempuser == null)
                                {
                                    deletestr.Add(files[i].FullName);
                                    message += "</br>压缩包中文件:" + files[i].Name + "文件名不是手机号或系统中无此用户，请先添加相应用户。";
                                    continue;
                                }
                                else
                                {
                                    //判断是否导入过人脸
                                    if (tempuser.ImgData != "")
                                    {
                                        PathMsg = files[i].FullName + "---------" + decompressionDirectory + tempuser.Tel + ".jpg";
                                        FileMove(files[i].FullName, decompressionDirectory + tempuser.Tel + ".jpg");
                                    }
                                    else
                                    {
                                        deletestr.Add(files[i].FullName);
                                        message += "</br>压缩包中文件:" + files[i].Name + "文件关联用户已录入人脸，请勿重复导入。";
                                        continue;
                                    }

                                }

                            }
                            else
                            {
                                TemporaryUserEntity tempuser = tempuserList.Where(it => it.USERID == Us.UserId).FirstOrDefault();
                                if (tempuser == null)
                                {
                                    PathMsg = files[i].FullName + "---------" + decompressionDirectory + Us.Account + ".jpg";
                                    FileMove(files[i].FullName, decompressionDirectory + Us.Account + ".jpg");
                                }
                                else
                                {
                                    //判断是否导入过人脸
                                    if (tempuser.ImgData != "")
                                    {
                                        PathMsg = files[i].FullName + "---------" + decompressionDirectory + tempuser.Tel + ".jpg";
                                        FileMove(files[i].FullName, decompressionDirectory + tempuser.Tel + ".jpg");
                                    }
                                    else
                                    {
                                        deletestr.Add(files[i].FullName);
                                        message += "</br>压缩包中文件:" + files[i].Name + "文件关联用户已录入人脸，请勿重复导入。";
                                        continue;
                                    }

                                }
                            }


                        }
                        else
                        {
                            deletestr.Add(files[i].FullName);
                            message += "</br>压缩包中文件:" + files[i].Name + "有错误,请更改后再导入。";
                            continue;
                        }


                    }
                    foreach (var str in deletestr)
                    {
                        System.IO.File.Delete(str);
                    }

                    //message = "共有" + dt.Rows.Count + "条记录,成功导入" + success + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }
                else
                {
                    message = "请选择文件格式正确的文件再导入!";
                }

                return message;
            }
            catch (Exception e)
            {
                return e.Message + "。导入的压缩包不正确,异常转换路径:" + PathMsg;
            }

        }

        public void FileMove(string fileurl, string newfileurl)
        {
            try
            {
                System.IO.File.Move(fileurl, newfileurl);
            }
            catch (Exception ex)
            {


            }
        }

        /// <summary>
        /// Zip批量人脸导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportFaces()
        {
            try
            {
                int error = 0;
                string message = "";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count == 1)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        message = "压缩包名称不能为空";
                        return message;
                    }

                    bool isZip1 = file.FileName.Substring(file.FileName.IndexOf('.'))
                        .Contains("zip"); //第一个文件是否为Zip格式
                    if (!isZip1)
                    {
                        message = "文件格式不正确，请上传zip格式文件";
                        return message;
                    }

                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") +
                                       System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string time = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    string decompressionDirectory = Server.MapPath("~/Resource/UserFaces/") + time + "\\";
                    UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);
                    DirectoryInfo root = new DirectoryInfo(decompressionDirectory);
                    FileInfo[] files = root.GetFiles();
                    UserBLL ubll = new UserBLL();
                    List<TemporaryUserEntity> tempuserList = Tempbll.GetUserList(); //所有临时人员
                    List<UserEntity> userlist = ubll.GetList().ToList();
                    List<DepartmentEntity> deptList = departmentBLL.GetList().ToList();
                    bool falg = true; //默认为正确，如果有一个出错则改为错误

                    List<TemporaryUserEntity> insertTemp = new List<TemporaryUserEntity>(); //需要新增的数据
                    List<TemporaryUserEntity> UpdateTemp = new List<TemporaryUserEntity>(); //需要修改的数据
                    if (files.Length > 0)
                    {

                        foreach (var fi in files)
                        {
                            string[] filenames = fi.Name.Split('.');
                            if (filenames.Length > 1)
                            {
                                if (filenames[filenames.Length - 1].ToLower() != "jpg")
                                {
                                    falg = false;
                                    message += "</br>压缩包中文件:" + fi.Name + "不是jpg格式,请更改后再导入。";
                                    continue;
                                }

                                if (fi.Length > 200 * 1024 || fi.Length < 10 * 1024)
                                {
                                    falg = false;
                                    message += "</br>压缩包中文件:" + fi.Name + "大小请控制在10kb到200kb之间,请更改后再导入。";
                                    continue;
                                }
                                var facePicData = Tobase64(fi.FullName);
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
                                //调用海康平台接口验证人脸照片是否合格
                                string msg = SocketHelper.FaceImgIsQualified(facePicData, baseurl, Key, Signature);
                                if (msg != null && !string.IsNullOrEmpty(msg))
                                {
                                    FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                                    if (!ress.data.checkResult)
                                    {
                                        falg = false;
                                        message += "</br>压缩包中文件:" + fi.Name + "人脸照片不合格,请更换后再导入。";
                                        continue;
                                    }
                                }

                                string name = filenames[filenames.Length - 2];
                                UserEntity Us = userlist.Where(it => it.Mobile == name).FirstOrDefault();
                                if (Us == null)
                                {
                                    TemporaryUserEntity tempuser =
                                        tempuserList.Where(it => it.Tel == name).FirstOrDefault();
                                    if (tempuser == null)
                                    {
                                        falg = false;
                                        message += "</br>压缩包中文件:" + fi.Name + "文件名不是手机号或系统中无此用户，请先添加相应用户。";
                                        continue;
                                    }
                                    else
                                    {
                                        //判断是否导入过人脸 如果人脸路径为空则给他录入
                                        if (string.IsNullOrEmpty(tempuser.UserImg))
                                        {
                                            //如果以在临时人员中存在则添加到修改列表中
                                            tempuser.ImgData = Tobase64(fi.FullName);
                                            tempuser.UserImg = "/Resource/UserFaces/" + time + "/" + fi.Name;
                                            UpdateTemp.Add(tempuser);
                                        }
                                        else
                                        {
                                            error++;
                                            //falg = false;
                                            //message += "</br>压缩包中文件:" + fi.Name + "文件关联用户已录入人脸，自动跳过本次操作。";
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    TemporaryUserEntity tempuser = tempuserList.Where(it => it.USERID == Us.UserId)
                                        .FirstOrDefault();
                                    if (tempuser != null)
                                    {
                                        //判断是否导入过人脸
                                        if (string.IsNullOrEmpty(tempuser.UserImg))
                                        {
                                            //如果以在临时人员中存在则添加到修改列表中
                                            tempuser.ImgData = Tobase64(fi.FullName);
                                            tempuser.UserImg = "/Resource/UserFaces/" + time + "/" + fi.Name;
                                            UpdateTemp.Add(tempuser);
                                        }
                                        else
                                        {
                                            //falg = false;
                                            error++;
                                            //message += "</br>压缩包中文件:" + fi.Name + "文件关联用户已录入人脸，请勿重复导入。";
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        falg = false;
                                        message += "</br>压缩包中文件:" + fi.Name + "文件关联用户未初始化到安防平台，请联系管理员。";
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                falg = false;
                                message += "</br>压缩包中文件:" + fi.Name + "有错误,请更改后再导入。";
                                continue;
                            }
                        }
                        //只有全部条件通过才执行保存方法
                        if (falg)
                        {
                            DataItemDetailBLL pdata = new DataItemDetailBLL();
                            string key = string.Empty; // "21049470";
                            string sign = string.Empty; // "4gZkNoh3W92X6C66Rb6X";
                            var pitem = pdata.GetItemValue("Hikappkey"); //海康服务器密钥
                            var url = pdata.GetItemValue("HikBaseUrl"); //海康服务器地址
                            if (!string.IsNullOrEmpty(pitem))
                            {
                                key = pitem.Split('|')[0];
                                sign = pitem.Split('|')[1];
                            }

                            Tempbll.SaveFace(insertTemp, UpdateTemp, url, key, sign);
                            message = "共有" + files.Length + "条记录,成功导入" + (UpdateTemp.Count - 0) + "条，人脸信息已录入" + error + "条";
                        }
                    }
                    else
                    {
                        message = "压缩包中无文件,请确认图片是直属压缩包中!";
                        return message;
                    }
                    message += "</br>" + falseMessage;
                }
                else
                {
                    message = "请选择文件格式正确的文件再导入!";
                }

                return message;
            }
            catch (Exception e)
            {
                return "导入的压缩包不正确";
            }

        }

        /// <summary>
        /// 临时人员批量导入（包含人脸）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportTempUserList()
        {
            try
            {
                string message = string.Empty;
                string falseMessage = string.Empty;

                int count = HttpContext.Request.Files.Count;
                if (count == 1)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        message = "压缩包名称不能为空";
                        return message;
                    }

                    bool isZip1 = file.FileName.Substring(file.FileName.IndexOf('.'))
                        .Contains("zip"); //第一个文件是否为Zip格式
                    if (!isZip1)
                    {
                        message = "文件格式不正确，请上传zip格式文件";
                        return message;
                    }

                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") +
                                       System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string time = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    string decompressionDirectory = Server.MapPath("~/Resource/TempUserImport/") + time + "\\";
                    string Imgpath = UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);
                    DirectoryInfo root = new DirectoryInfo(decompressionDirectory);
                    var newpath = root.GetDirectories().FirstOrDefault();//子目录
                    FileInfo[] files = root.GetFiles();
                    List<TemporaryUserEntity> Templist = new List<TemporaryUserEntity>();
                    List<TemporaryUserEntity> insertTemp = new List<TemporaryUserEntity>(); //需要新增的数据
                    List<string> countlist = new List<string>();
                    bool falg = true; //默认为正确，如果有一个出错则改为错误

                    #region 临时人员信息
                    List<TemporaryUserEntity> tempuserList = Tempbll.GetUserList(); //所有临时人员

                    DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/TempUserImport/" + time + "/" + files[0].Name));
                    int order = 1;
                    int error = 0;
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        order = i;
                        TemporaryUserEntity entity = new TemporaryUserEntity();
                        entity.UserName = dt.Rows[i][0].ToString();
                        entity.Gender = dt.Rows[i][1].ToString();
                        entity.Identifyid = dt.Rows[i][2].ToString();
                        entity.ComName = dt.Rows[i][3].ToString();
                        entity.GroupsName = dt.Rows[i][4].ToString();
                        entity.Tel = dt.Rows[i][5].ToString();
                        string starttime = dt.Rows[i][6].ToString();
                        string enttime = dt.Rows[i][7].ToString();
                        entity.PassPost = dt.Rows[i][8].ToString();

                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.Gender) || string.IsNullOrEmpty(entity.ComName) || string.IsNullOrEmpty(entity.GroupsName) || string.IsNullOrEmpty(entity.Tel) || string.IsNullOrEmpty(starttime) || string.IsNullOrEmpty(enttime) || string.IsNullOrEmpty(entity.PassPost))
                        {
                            falg = false;
                            message += "</br>" + "人员导入模板中第" + (i + 2) + "行值存在空,请完善后在导入。";
                            error++;
                            continue;
                        }
                        countlist.Add(entity.Tel);
                        var tempuser = tempuserList.Where(t => t.Tel == entity.Tel).FirstOrDefault();
                        if (tempuser != null)
                        {
                            falg = false;
                            message += "</br>" + "人员导入模板中第" + (i + 2) + "行系统中该用户已存在,请核对后在导入。";
                            error++;
                            continue;
                        }
                        entity.Groupsid = GetIsGroupNameHave(entity.GroupsName);
                        if (string.IsNullOrEmpty(entity.Groupsid))
                        {
                            falg = false;
                            message += "</br>" + "人员导入模板中第" + (i + 2) + "分组名称不存在,请核对后在导入。";
                            error++;
                            continue;
                        }
                        try
                        {
                            entity.startTime = DateTime.Parse(DateTime.Parse(starttime).ToString("yyyy-MM-dd"));
                            entity.EndTime = DateTime.Parse(DateTime.Parse(enttime).ToString("yyyy-MM-dd"));
                            if (entity.startTime >= entity.EndTime)
                            {
                                falg = false;
                                message += "</br>" + "人员导入模板中第" + (i + 2) + "考勤开始时间不能大于考勤结束时间,请修正后在导入。";
                                error++;
                                continue;
                            }
                        }
                        catch
                        {
                            falg = false;
                            message += "</br>" + "人员导入模板中第" + (i + 2) + "行时间有误,请完善后在导入。";
                            error++;
                            continue;
                        }
                        Templist.Add(entity);
                    }
                    #endregion

                    #region 临时人员人脸
                    DirectoryInfo roots = new DirectoryInfo(newpath.FullName);
                    FileInfo[] files1 = roots.GetFiles();

                    if (files1.Length > 0)
                    {
                        foreach (var fi in files1)
                        {
                            string[] filenames = fi.Name.Split('.');
                            if (filenames.Length > 1)
                            {
                                if (filenames[filenames.Length - 1].ToLower() != "jpg")
                                {
                                    falg = false;
                                    message += "</br>人脸图片压缩包中文件:" + fi.Name + "不是jpg格式,请更改后再导入。";
                                    continue;
                                }

                                if (fi.Length > 200 * 1024 || fi.Length < 10 * 1024)
                                {
                                    falg = false;
                                    message += "</br>人脸图片压缩包中文件:" + fi.Name + "大小请控制在10kb到200kb之间,请更改后再导入。";
                                    continue;
                                }
                                #region 人脸是否合格
                                var facePicData = Tobase64(fi.FullName);
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
                                //调用海康平台接口验证人脸照片是否合格
                                string msg = SocketHelper.FaceImgIsQualified(facePicData, baseurl, Key, Signature);
                                if (msg != null && !string.IsNullOrEmpty(msg))
                                {
                                    FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                                    if (!ress.data.checkResult)
                                    {
                                        falg = false;
                                        message += "</br>压缩包中文件:" + fi.Name + "人脸照片不合格,请更换后再导入。";
                                        continue;
                                    }
                                }
                                #endregion
                                string name = filenames[filenames.Length - 2];
                                TemporaryUserEntity tempuser = Templist.Where(it => it.Tel == name).FirstOrDefault();
                                if (tempuser == null && !countlist.Contains(name))
                                {
                                    falg = false;
                                    message += "</br>人脸图片压缩包中文件:" + fi.Name + "文件名不是手机号或临时人员导入模板中无此用户，请更改后再导入。";
                                    continue;
                                }
                                else
                                {
                                    if (tempuser != null)
                                    {
                                        //判断是否导入过人脸
                                        if (string.IsNullOrEmpty(tempuser.UserImg))
                                        {
                                            //如果以在临时人员中存在则添加到修改列表中
                                            tempuser.ImgData = Tobase64(fi.FullName);
                                            tempuser.UserImg = "/Resource/TempUserImport/" + time + "/" + fi.Directory.Name + "/" + fi.Name;
                                            insertTemp.Add(tempuser);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                falg = false;
                                message += "</br>压缩包中文件:" + fi.Name + "有错误,请更改后再导入。";
                                continue;
                            }
                        }
                        //只有全部条件通过才执行保存方法
                        if (falg)
                        {
                            Tempbll.SaveTempImport("", insertTemp);
                            int pcount = dt.Rows.Count - 1;
                            message = "共有" + pcount + "条记录,成功导入" + insertTemp.Count + "条，失败" + (pcount - insertTemp.Count) + "条";
                        }
                    }
                    #endregion
                }
                return message;
            }
            catch (Exception er)
            {
                return "导入的压缩包不正确";
            }
        }

        /// <summary>
        /// 临时组是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetIsGroupNameHave(string name)
        {
            string res = string.Empty;
            TemporaryGroupsEntity itme = Tempbll.GetList("").ToList().Where(t => t.GroupsName == name).FirstOrDefault();
            if (itme != null)
            {
                res = itme.ID;
            }
            return res;
        }


        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Tobase64(string url)
        {
            string base64 = "";
            Bitmap bmp1 = new Bitmap(url);
            using (MemoryStream ms = new MemoryStream())
            {
                bmp1.Save(ms, ImageFormat.Jpeg);
                byte[] arr1 = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr1, 0, (int)ms.Length);
                ms.Close();
                base64 = Convert.ToBase64String(arr1);
            }

            return base64;
        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="zipedFile"></param>
        /// <param name="strDirectory"></param>
        /// <param name="password"></param>
        /// <param name="overWrite"></param>
        public string UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            string Imgpath = string.Empty;
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();

            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);
                    if (string.IsNullOrEmpty(Imgpath)) Imgpath = strDirectory + directoryName;
                    if (fileName != "")
                    {
                        if ((System.IO.File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!System.IO.File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = System.IO.File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
            return Imgpath;
        }

        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="src">远程服务器路径（共享文件夹路径）</param>  
        /// <param name="dst">本地文件夹路径</param>  
        /// <param name="filename"></param> 
        public static void TransportRemoteToServer(string src, string dst, string filename)
        {
            if (!Directory.Exists(src))
            {
                Directory.CreateDirectory(src);
            }
            FileStream inFileStream = new FileStream(src + filename, FileMode.OpenOrCreate);

            FileStream outFileStream = new FileStream(dst, FileMode.Open);

            byte[] buf = new byte[outFileStream.Length];

            int byteCount;

            while ((byteCount = outFileStream.Read(buf, 0, buf.Length)) > 0)
            {
                inFileStream.Write(buf, 0, byteCount);

            }

            inFileStream.Flush();

            inFileStream.Close();

            outFileStream.Flush();

            outFileStream.Close();

        }

        #endregion
    }
}