using ERCHTMS.Busines.PublicInfoManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Controllers
{
    [HandlerLogin(Code.LoginMode.Valid)]
    public class AttachmentController : Controller
    {
        // GET: Attachment
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult Download(string filepath)
        {
            var filename = filepath.Substring(filepath.LastIndexOf("/") + 1);
            var path = filepath.Replace("/", "\\");
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource", path);
            return File(path, "application/octet-stream", filename);
        }
    }
}