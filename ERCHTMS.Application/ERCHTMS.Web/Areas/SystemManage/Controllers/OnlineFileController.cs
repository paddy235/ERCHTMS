using BSFramework.Util;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    public class OnlineFileController : Controller
    {
        //
        // GET: /SystemManage/OnlineFile/
        #region 视图功能
        /// <summary>
        /// 文档编辑
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 获取机构部门组织树菜单
        /// <summary>
        /// 获取文档结构树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDepartTreeJson()
        {
            string fid = "";
            List<TreeEntity> treeList = new List<TreeEntity>();
            string path = HttpContext.Server.MapPath("~");
            //设置不显示的最顶级节点
            TreeEntity toponeTree = new TreeEntity();
            toponeTree.id = "0";
            toponeTree.parentId = "-1";
            toponeTree.text = "";
            toponeTree.value = "";
            toponeTree.isexpand = true;
            toponeTree.complete = true;
            toponeTree.hasChildren = true;
            treeList.Add(toponeTree);
            //设置根级树节点
            TreeEntity topTree = new TreeEntity();
            topTree.id= fid = Guid.NewGuid().ToString();
            topTree.parentId = "0";
            topTree.text = path.Substring(path.LastIndexOf("\\")+1,path.Length-path.LastIndexOf("\\")-1);
            topTree.value = path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1); ;
            topTree.isexpand = true;
            topTree.complete = true;
            topTree.hasChildren = true;
            topTree.Attribute = "MapPath";
            topTree.AttributeValue = HttpUtility.UrlEncode(path);
            treeList.Add(topTree);
            string[] dirs = Directory.GetDirectories(path);//获取目录下的所有文件夹
            string[] dirs2 = Directory.GetFiles(path);//获取目录下的所有文件
            BindTrees(dirs, dirs2, treeList, fid);

            return Content(treeList.TreeToJson());
        }

        private void BindTrees(string[] dirs, string[] dirs2, List<TreeEntity> treeList,string fid)
        {
            //加载文件夹
            foreach (string dir in dirs)
            {
                #region 子文件夹
                string qid = "";
                string[] md = Directory.GetDirectories(dir);//获取目录下的所有文件夹
                string[] mdf = Directory.GetFiles(dir);//获取目录下的所有文件

                TreeEntity tree = new TreeEntity();
                tree.id = qid = Guid.NewGuid().ToString();
                tree.text = string.Format("{0}", Path.GetFileName(dir));
                tree.value = string.Format("{0}", Path.GetFileName(dir));
                tree.parentId = fid;
                tree.isexpand = false;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "MapPath";
                tree.AttributeValue = HttpUtility.UrlEncode(dir);
                treeList.Add(tree);
                if (mdf.Length > 0|| md.Length>0)
                {
                    BindTrees(md,mdf, treeList, qid);//递归去除所有文件
                }
                #endregion
            }
            //加载文件
            foreach (string dir in dirs2)
            {
                #region 文件
                //if (Path.GetFileName(dir).Contains("cshtml") || Path.GetFileName(dir).Contains("config") || Path.GetFileName(dir).Contains("css") || Path.GetFileName(dir).Contains("js"))
                //{
                    TreeEntity tree = new TreeEntity();
                    tree.id = Guid.NewGuid().ToString();
                    tree.text = string.Format("{0}", Path.GetFileName(dir));
                    tree.value = string.Format("{0}", Path.GetFileName(dir));
                    tree.parentId = fid;
                    tree.isexpand = false;
                    tree.complete = true;
                    tree.hasChildren = false;
                    tree.Attribute = "MapPath";
                    tree.AttributeValue = HttpUtility.UrlEncode(dir);
                    treeList.Add(tree);
                //}
                #endregion
            }
        }
       
        /// <summary>
        /// 显示文档内容
        /// </summary>
        [HttpGet]
        public ActionResult BindFiles(string path)
        {
            var readText = "";
            path = HttpUtility.UrlDecode(path);
            if (System.IO.File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                readText = sr.ReadToEnd();
                sr.Close();
            }
            return Content(readText);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="projectItem">页面内容</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string path,string projectItem)
        {
            projectItem = HttpUtility.UrlDecode(projectItem);
            //修改之前备份文件
            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            
            string oldFile = sr.ReadToEnd();
            sr.Close();
            StreamWriter sw = new StreamWriter(path + "_bak" + DateTime.Now.ToString("yyyyMMddHHmmss"), false, System.Text.Encoding.UTF8);
            sw.Write(oldFile);
            sw.Close();

            sw = new StreamWriter(path, false, System.Text.Encoding.UTF8);
            sw.Write(projectItem.Replace("\n","\r\n"));
            sw.Close();
            return Content(new AjaxResult { type = ResultType.success, message = "保存成功" }.ToJson());
        }
        #endregion

    }
}
