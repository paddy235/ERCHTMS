using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Entity.CarManage;
using System.Web;
using System.IO;
using ERCHTMS.Busines.SystemManage;
using System;
using ERCHTMS.Code;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 描 述：用户门禁数据类
    /// </summary>
    public class AcesscontrolinfoController : MvcControllerBase
    {
        private AcesscontrolinfoBLL acesscontrolinfobll = new AcesscontrolinfoBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
        private TemporaryGroupsBLL Tempbll = new TemporaryGroupsBLL();
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 人员拜访录入人脸视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CarUserForm()
        {
            return View();
        }

        /// <summary>
        /// 人脸批量导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportFace()
        {
            return View();
        }


        #endregion

        #region 获取数据

        /// <summary>
        /// 获取选中用户关联人脸指纹数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetUserList(string userids)
        {
            var data = acesscontrolinfobll.GetUserList(userids);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取拜访人员人脸信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetCarUserList(string userids, string type = "0")
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
                #region 临时人员判断
                if (type == "1")
                {
                    var tempentity = new TemporaryGroupsBLL().GetEmptyUserEntity(users[i]);
                    if (tempentity == null)
                    {
                        var Us = userBLL.GetEntity(users[i]);
                        if (Us != null)
                        {
                            List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
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
                            var dept1 = departmentBLL.GetEntity(Us.DepartmentId);
                            if (dept1 != null)
                            {
                                inserttuser.GroupsName = dept1.FullName;
                            }
                            list.Add(inserttuser);
                            new TemporaryGroupsBLL().SaveTemporaryList("", list);
                        }
                    }
                }
                #endregion
            }
            string sql = string.Empty;
            if (type == "1")
            {
                sql = string.Format(@"  select d.userid as id, realname as username from v_userinfo d   where d.userid in ({0})", userid);
            }
            else
            {
                sql = string.Format(@" select d.id,d.userimg,d.username,d.baseid,d.imgdata from bis_usercarfileimg d   where d.id in ({0})", userid);
            }

            var data = Opertickebll.GetDataTable(sql);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = acesscontrolinfobll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = acesscontrolinfobll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
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
            acesscontrolinfobll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string userid, string imgData, int type)
        {
            AcesscontrolinfoEntity ac = new AcesscontrolinfoEntity();
            ac.TID = keyValue;
            ac.UserId = userid;
            ac.Face = imgData;
            if (type == 1)
            {
                ac.IsFace = 1;
            }
            else
            {
                ac.IsFinger = 1;
            }
            acesscontrolinfobll.SaveForm(keyValue, ac);
            return Success("操作成功。");
        }

        /// <summary>
        /// 调用摄像头录入人脸照片
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFileImgForm(string keyValue, string imgData, string type, string NewType)
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
                if (type == "1")
                {//门岗管理
                    List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                    var tempdate = Tempbll.GetEmptyUserEntity(keyValue);
                    if (tempdate != null)
                    {
                        var facedata = ImgToBase64(HttpUtility.UrlDecode(imgData));
                        //调用海康平台接口验证人脸照片是否合格
                        int NewTypes = NewType == "" ? 0 : 1;
                        string msg = SocketHelper.FaceImgIsQualified(facedata.ImgData, baseurl, Key, Signature, NewTypes);
                        if (msg != null && !string.IsNullOrEmpty(msg))
                        {
                            FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                            if (!ress.data.checkResult)
                            {
                                return Content("false", "1");
                            }
                        }
                        if (string.IsNullOrEmpty(tempdate.UserImg))
                        {//未授权
                            FacedataEntity face = new FacedataEntity();
                            List<FacedataEntity> FaceList = new List<FacedataEntity>();
                            face.UserId = tempdate.USERID;
                            face.ImgData = facedata.ImgData;
                            FaceList.Add(face);
                            SocketHelper.UploadFace(FaceList, baseurl, Key, Signature, NewTypes);
                            tempdate.UserImg = facedata.UserImg;
                            tempdate.ImgData = facedata.ImgData;
                            list.Add(tempdate);
                            Tempbll.SaveTemporaryList(tempdate.USERID, list);
                        }
                        else if (!string.IsNullOrEmpty(tempdate.PassPost))
                        {//已授权
                            tempdate.UserImg = facedata.UserImg;
                            tempdate.ImgData = facedata.ImgData;
                            tempdate.Remark = "1";
                            if (NewTypes == 1)
                            {//新版本https调用
                                new PersonNewBLL().SaveUserFace(tempdate, keyValue, true);
                            }
                            else
                            {
                                Tempbll.SaveUserFace(tempdate, keyValue, true);
                            }

                        }
                        return Success("操作成功。");
                    }
                    else
                    {
                        return Success("1", "1");
                    }
                }
                else
                {//拜访车辆(门卫)
                    CarUserFileImgEntity entity = new CarUserFileImgEntity();
                    var facedata = ImgToBase64(HttpUtility.UrlDecode(imgData));
                    //调用海康平台接口验证人脸照片是否合格
                    string msg = SocketHelper.FaceImgIsQualified(facedata.ImgData, baseurl, Key, Signature);
                    if (msg != null && !string.IsNullOrEmpty(msg))
                    {
                        FaceTestingEntity ress = JsonConvert.DeserializeObject<FaceTestingEntity>(msg);
                        if (!ress.data.checkResult)
                        {
                            return Content("false", "1");
                        }
                    }
                    entity.ID = keyValue;
                    entity.Userimg = facedata.UserImg;
                    entity.Imgdata = facedata.ImgData;
                    new CarUserBLL().SaveFileImgForm(entity);
                    return Success("操作成功。");
                }
            }
            catch (System.Exception er)
            {
                return Success(er.Message);
            }
        }


        public TemporaryUserEntity ImgToBase64(string imgData)
        {
            TemporaryUserEntity UserEntity = new TemporaryUserEntity();
            string fileurl = string.Empty;
            if (!string.IsNullOrEmpty(imgData))
            {//拍照的照片
                UserEntity.ImgData = imgData;
                DataItemDetailBLL dd = new DataItemDetailBLL();
                string path = dd.GetItemValue("imgPath") + "\\Resource\\" + "TemporaryUser";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
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
                UserEntity.UserImg = fileurl;
            }
            return UserEntity;
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


        #endregion


    }

}
