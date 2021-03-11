using BSFramework.Util;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Controllers
{
    public class WebIMController : Controller
    {
        public object GetMembers(string id, string ownerId)
        {
            UserBLL userBLL = new UserBLL();
            UserEntity user = userBLL.GetEntity(ownerId);
            DataTable dt = userBLL.GetMembers(id);
            object obj = new { code = "0", msg = "", data = new { owner = new { id = ownerId, username = user.RealName, avatar = "", sing = "" }, members = dt.Rows.Count, list = dt } };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
        [HttpPost]
        public object UploadFile()
        {
            HttpPostedFileBase file =HttpContext.Request.Files[0];
            try
            {
                string fileName = System.IO.Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/WeChatFile/") + fileName);
                string json =Newtonsoft.Json.JsonConvert.SerializeObject( new { code = 0, msg = "", data = new { src = "../Resource/WeChatFile/" + fileName, name = fileName } });
                return Content(json);
            }
            catch (Exception ex)
            {
                return Content("{ code = 1, msg = \"" + ex.Message + "\" }");
            }
        }
        [HttpPost]
        public object UploadCapture()
        {
            try
            {
                string data = Request["picdata"];
                byte[] byteData = null;

                byteData = Convert.FromBase64String(data);
                string strExtendName = "jpg";
                try
                {
                    strExtendName = Request["extendName"];
                }
                catch (Exception ee)
                {

                }
                string imageName =DateTime.Now.ToString("yyyyMMddHHmmss")+"."+ strExtendName;
                System.IO.File.WriteAllBytes(Server.MapPath("~/Resource/WeChatFile/")+ imageName, byteData);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 0, msg = "", data = new { src = "../Resource/WeChatFile/" + imageName, name = imageName } });
                return Content(json);
            }
            catch (Exception ex)
            {
                return Content("{ code = 1, msg = \""+ex.Message+"\" }");
            }
        }
         

    }
}
