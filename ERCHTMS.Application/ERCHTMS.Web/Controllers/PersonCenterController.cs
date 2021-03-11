using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ERCHTMS.Busines.SystemManage;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage;
using System.Data;

namespace ERCHTMS.Web.Controllers
{
    /// <summary>
    /// 描 述：个人中心
    /// </summary>
    public class PersonCenterController : MvcControllerBase
    {
        private UserBLL userBLL = new UserBLL();

        #region 视图功能
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.userId = OperatorProvider.Provider.Current().UserId;
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdatePwd()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile(int mode=0)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string virtualPath = "";
            string UserId = OperatorProvider.Provider.Current().UserId;
            if (mode==0)
            {
                string FileEextension = Path.GetExtension(files[0].FileName);
                
                virtualPath = string.Format("/Resource/PhotoFile/{0}{1}", Guid.NewGuid().ToString(), FileEextension);
                string fullFileName = Server.MapPath("~" + virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                files[0].SaveAs(fullFileName);

                UserEntity userEntity = userBLL.GetEntity(OperatorProvider.Provider.Current().UserId);
                userEntity.HeadIcon = virtualPath;
                userBLL.SaveForm(userEntity.UserId, userEntity);
                return Success("上传成功。", virtualPath);
            }
            else
            {
                DataItemDetailBLL dd = new DataItemDetailBLL(); 

                //string path = @"D:\workproject\ERCHTMS\ERCHTMS.Application\ERCHTMS.Web\Resource\sign\";
                string imgurl = dd.GetItemValue("imgUrl");

                string path = dd.GetItemValue("imgPath") + "\\Resource\\sign";

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                var file = files[0];
                string ext = System.IO.Path.GetExtension(file.FileName);
                string fileName =  Guid.NewGuid().ToString() + ".png";
                file.SaveAs(path + "\\" + fileName);
                if(GetPicThumbnail(path + "\\" + fileName,path + "\\s" + fileName,40,100,20))
                {
                     virtualPath = "/Resource/sign/s" + fileName;
                }
                else
                {
                     virtualPath = "/Resource/sign/" + fileName;
                }
                new UserBLL().UploadSignImg(UserId, virtualPath);
                string bzAppUrl = new DataItemDetailBLL().GetItemValue("bzAppUrl");
                if (!string.IsNullOrEmpty(bzAppUrl))
                {
                    UpdateSign(UserId, fileName, path, imgurl, bzAppUrl);
                }
                return Success("上传成功。", virtualPath);
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
        /// <summary>
        /// 验证旧密码
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidationOldPassword(string OldPassword)
        {
            OldPassword = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(OldPassword, 32).ToLower(), OperatorProvider.Provider.Current().Secretkey).ToLower(), 32).ToLower();
            if (OldPassword != OperatorProvider.Provider.Current().Password)
            {

                return Error("原密码错误，请重新输入");
            }
            else
            {
                return Success("通过信息验证");
            }
        }
        /// <summary>
        /// 提交修改密码
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="password">新密码</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitResetPassword(string password, string oldPassword, string verifyCode)
        {
            if (password.Trim().Length > 16)
            {
                return Error("密码长度不能大于16位!");
            }
            verifyCode = Md5Helper.MD5(verifyCode.ToLower(), 16);
            if (Session["session_verifycode"].IsEmpty() || verifyCode != Session["session_verifycode"].ToString())
            {
                return Error("验证码错误，请重新输入");
            }
            oldPassword = Md5Helper.MD5(DESEncrypt.Encrypt(oldPassword, OperatorProvider.Provider.Current().Secretkey).ToLower(), 32).ToLower();
            if (oldPassword != OperatorProvider.Provider.Current().Password)
            {
                return Error("原密码错误，请重新输入");
            }
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ERCHTMS.Busines.SystemManage.PasswordSetBLL psBll = new PasswordSetBLL();
            List<string> lst = psBll.IsPasswordRuleStatus(user);
            if (lst[0] == "true")
            {
                //string[] arr = lst[1].Split(';');
                //System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(lst[4]);
                //if (arr[0].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[A-Z]{1,}.*$/");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[1].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[a-z]{1,}.*$/");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[2].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[0-9]{1,}.*$/");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[3].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[~_!=@#\$%^&\*\?\(\)]{1,}.*$/");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(lst[4]);
                if (!reg.IsMatch(password))
                {
                    return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                }
            }
           

            string pwd = Md5Helper.MD5(password, 32).ToLower();
            string userId = user.UserId;
            userBLL.RevisePassword(userId, pwd);
            //userBLL.RecordPassword(userId, password);
            DataItemDetailBLL di = new DataItemDetailBLL();
            if (!string.IsNullOrEmpty(new DataItemDetailBLL().GetItemValue("bzAppUrl")))
            {
                UpdatePwd(userId, pwd);
            }
            string way = di.GetItemValue("WhatWay");
            DepartmentEntity org = new DepartmentBLL().GetEntity(user.OrganizeId);
            if (org.IsTrain == 1)
            {
                //对接.net培训平台
                if (way == "0")
                {

                }

                //对接java培训平台
                if (way == "1")
                {
                    Task.Factory.StartNew(() =>
                    {
                        UserEntity userEntity = userBLL.GetEntity(userId);
                        object obj = new
                        {
                            action = "updatePwd",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            userId = userId,
                            account = user.Account,
                            password = password,
                            companyId=org.InnerPhone
                        };
                        List<object> list = new List<object>();
                        list.Add(obj);
                        Busines.JPush.JPushApi.PushMessage(list, 1);

                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 5;
                        logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                        logEntity.OperateType = "修改密码";
                        logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                        logEntity.OperateUserId = user.UserId;

                        logEntity.ExecuteResult = 1;
                        logEntity.ExecuteResultJson = string.Format("同步用户(修改密码)到java培训平台,同步信息:\r\n{0}", list.ToJson());
                        logEntity.Module = "个人中心";
                        logEntity.ModuleId = "";
                        logEntity.WriteLog();

                    });
                }
            }
            Session.Abandon(); Session.Clear();
            return Success("密码修改成功，请牢记新密码。\r 将会自动安全退出。");
        }
        /// <summary>
        /// 密码强度校验
        /// </summary>
        /// <param name="password"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(string password, string verifyCode)
        {
            if(password.Trim().Length>16)
            {
                return Error("密码长度不能大于16位");
            }
            verifyCode = Md5Helper.MD5(verifyCode.ToLower(), 16);
            if (Session["session_verifycode"].IsEmpty() || verifyCode != Session["session_verifycode"].ToString())
            {
                return Error("验证码错误，请重新输入");
            }
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ERCHTMS.Busines.SystemManage.PasswordSetBLL psBll = new PasswordSetBLL();
            List<string> lst = psBll.IsPasswordRuleStatus(user);
            if (lst[0] == "true")
            {
                //string[] arr = lst[1].Split(';');
                //System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(lst[4]);
                //if (arr[0].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(".*[A-Z]{1,}.*");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[1].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@".*[a-z]{1,}.*");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[2].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@".*[0-9]{1,}.*");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[3].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@".*[~_!=@#\$%^&\*\?\(\)]{1,}.*");
                //    if (!reg1.IsMatch(password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(lst[4]);
                if (!reg.IsMatch(password))
                {
                    return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                }
            }
               
           
                string userId = user.UserId;
                string pwd = Md5Helper.MD5(password, 32).ToLower();
                userBLL.RevisePassword(userId, pwd);
                Session.Abandon(); Session.Clear();
                if (!string.IsNullOrEmpty(new DataItemDetailBLL().GetItemValue("bzAppUrl")))
                {
                    UpdatePwd(userId, pwd);
                }
                DepartmentEntity org = new DepartmentBLL().GetEntity(user.OrganizeId);
                if (org.IsTrain == 1)
                {
                    Task.Factory.StartNew(() =>
                    {
                        UserEntity userEntity = userBLL.GetEntity(userId);
                        object obj = new
                        {
                            action = "updatePwd",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            userId = userId,
                            account = user.Account,
                            password = password,
                            companyId=org.InnerPhone
                        };
                        List<object> list = new List<object>();
                        list.Add(obj);
                        Busines.JPush.JPushApi.PushMessage(list, 1);

                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 5;
                        logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                        logEntity.OperateType = "修改密码";
                        logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                        logEntity.OperateUserId = user.UserId;

                        logEntity.ExecuteResult = 1;
                        logEntity.ExecuteResultJson = string.Format("同步用户(修改密码)到java培训平台,同步信息:\r\n{0}", list.ToJson());
                        logEntity.Module = "个人中心";
                        logEntity.ModuleId = "";
                        logEntity.WriteLog();

                    });
                }
                return Success("密码修改成功，请牢记新密码。\r 将会自动安全退出。");
        }
        #endregion
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        private void UpdatePwd(string userId,string pwd)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "UpdatePwd?userId=" + userId + "&pwd=" + pwd), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：修改密码失败，用户信息" + userId + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            //将同步结果写入日志文件
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            if(e.Error!=null)
            {

                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + Newtonsoft.Json.JsonConvert.SerializeObject(e.Error) + "\r\n");
            }
            else
            {
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + Newtonsoft.Json.JsonConvert.SerializeObject(e.Result) + "\r\n");
            }
           
        }

        /// <summary>
        /// 个人待办提醒设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveWaitWork(int status=1)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DepartmentBLL deptBll = new DepartmentBLL();
            int count=deptBll.ExecuteSql(string.Format("begin\r\ndelete from BIS_USERWAIWORK where userid='{1}';\r\ninsert into BIS_USERWAIWORK(id,userid,status) values('{0}','{1}',1);\r\n end\r\ncommit;", Guid.NewGuid().ToString(),user.UserId));
            return count>0?Success("操作成功"):Error("操作失败");
        }
        [HttpGet]
        public ActionResult GetUserWorkStatus()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemDetailBLL di = new DataItemDetailBLL();
            string val=di.GetItemValue("个人是否可以关闭待办提醒", "dbtx");
            if(val=="1")
            {
                return Content("1");
            }
            else
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtStatus = deptBll.GetDataTable(string.Format("select status from BIS_USERWAIWORK where userid='{0}'", user.UserId));
                string status = dtStatus.Rows.Count == 0 ? "0" : dtStatus.Rows[0][0].ToString();
                return Content(status);
            }
        }

        [HttpGet]
        public ActionResult GetWorkStatus()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataItemDetailBLL di = new DataItemDetailBLL();
            string val = di.GetItemValue("是否提醒", "dbtx");
            string status = "1";
            if (val == "1")
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtStatus = deptBll.GetDataTable(string.Format("select status from BIS_USERWAIWORK where userid='{0}' and status=1", user.UserId));
                if(dtStatus.Rows.Count>0)
                {
                    status = "0";
                }
                return Content(status);
            }
            else
            {
                return Content("0");
            }
             
        }

    }
}
