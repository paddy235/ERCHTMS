using BSFramework.Util;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class WebIMController : ApiController
    {
        
        // GET api/<controller>/5
        /// <summary>
        /// 获取联系人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public object GetMembers(string id, string ownerId)
        {
            UserBLL userBLL = new UserBLL();
            UserEntity user = userBLL.GetEntity(ownerId);
            DataTable dt = userBLL.GetMembers(id);
            object obj = new { code = "0", msg = "", data = new { owner = new { id = ownerId, username = user.RealName, avatar = "", sing = "" }, members = dt.Rows.Count, list = dt } };
            return obj;
        }
        /// <summary>
        /// 上传图片和文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UploadFile()
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            try
            {
                string dir = HttpContext.Current.Server.MapPath("~/Resource/WeChatFile");
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                string fileName = System.IO.Path.GetFileName(file.FileName); 
                file.SaveAs(dir +"\\" +fileName);
                string appPath = "http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath;
                return new { code = 0, msg = "", data = new { src =appPath + "/Resource/WeChatFile/" + fileName, name = fileName } };
            }
            catch (Exception ex)
            {
                return new { code = 1, msg = ex.Message };
            }

        }
        /// <summary>
        /// 屏幕截图文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UploadCapture()
        {
            try
            {
                //接收以base64编码后的文件内容
                string data = HttpContext.Current.Request["picdata"];
                byte[] byteData = null;

                byteData = Convert.FromBase64String(data);
                string strExtendName = "jpg";
                try
                {
                    string dir = HttpContext.Current.Server.MapPath("~/Resource/WeChatFile");
                    if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                    string appPath = "http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath;
                    strExtendName = HttpContext.Current.Request["extendName"];
                    string imageName = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + strExtendName;
                    System.IO.File.WriteAllBytes(dir+"\\" + imageName, byteData);
                    object obj = new { code = 0, msg = "", data = new { src = appPath + "/Resource/WeChatFile/" + imageName, name = imageName } };
                    return obj;
                }
                catch (Exception ex)
                {
                    return new { code = 1, msg = ex.Message };
                }
               
            }
            catch (Exception ex)
            {
                return new { code = 1, msg =ex.Message  };
            }
        }
    }
}