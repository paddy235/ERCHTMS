using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using System.Net;
using System;
using System.Text;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：角色管理
    /// </summary>
    public class RoleController : MvcControllerBase
    {
        private RoleBLL roleBLL = new RoleBLL();
        private RoleCache roleCache = new RoleCache();

        #region 视图功能
        /// <summary>
        /// 角色管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 角色表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 角色选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="organizeId">公司Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string organizeId)
        {
            var data = roleBLL.GetList(organizeId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = roleBLL.GetPageList(pagination, queryJson);
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
        /// 角色实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = roleBLL.GetEntity(keyValue);
            return Content(data.ToJson("yyyy-MM-dd HH:mm"));
        }
        #endregion

        #region 根据查询条件获取角色树

        /// <summary>
        /// 根据查询条件获取部门树 
        /// </summary>
        /// <param name="Ids">上级部门(机构)Id</param>
        /// <param name="checkMode">单选或多选，0:单选，1:多选</param>
        /// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即OrganizeId=Ids)，1:获取部门ID为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即ParentId in(Ids))，3:获取部门Id在Ids中的部门(即DepartmentId in(Ids))</param>
        /// <param name="roleIDs">角色IDs</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetRoleTreeJson(string Ids, string roleIDs, int checkMode = 0, int mode = 0)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //不传Ids那么显示所有通用角色，选择Ids显示当前公司下面的加上所有通用角色
            string parentId = "0";
            var treeList = new List<TreeEntity>();
            IEnumerable<RoleEntity> data = new List<RoleEntity>();
            //如果没有传递参数parentId,则给出默认值
            if (string.IsNullOrEmpty(Ids))
            {
                data = roleCache.GetList().Where(a=>a.OrganizeId=="" || a.OrganizeId == null);
            }
            else
            {
                parentId = Ids.Contains(",") ? "0" : Ids;

                data = roleCache.GetList().Where(a => parentId.Contains(string.IsNullOrEmpty(a.OrganizeId) ? "" : a.OrganizeId) || string.IsNullOrWhiteSpace(a.OrganizeId)).ToList();

            }
            if (!user.IsSystem)
            {
                string roleName = "省级领导,省级专责工,省级主管用户,省级安管员用户,省级管理员,省级用户,集团用户,超级管理员";
                data = data.Where(t => !roleName.Contains(t.FullName));
            }
            foreach (RoleEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = false;
                tree.id = item.RoleId;
                tree.text = item.FullName;
                tree.value = item.RoleId;
                tree.isexpand = true;
                tree.complete = true;
                if (!string.IsNullOrEmpty(roleIDs))
                {
                    if (roleIDs.Contains(item.RoleId)) tree.checkstate = 1;
                }
                tree.showcheck = checkMode == 0 ? false : true;
                tree.hasChildren = hasChildren;
                tree.parentId = parentId.Contains(",") ? "0" : parentId;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(parentId));
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 角色编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistEnCode(string EnCode, string keyValue)
        {
            bool IsOk = roleBLL.ExistEnCode(EnCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 角色名称不能重复
        /// </summary>
        /// <param name="FullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistFullName(string FullName, string keyValue)
        {
            bool IsOk = roleBLL.ExistFullName(FullName, keyValue);
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(6, "删除角色信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            RoleEntity roleEntity=roleBLL.GetEntity(keyValue);
            if(roleEntity!=null)
            {
                bool result=roleBLL.RemoveForm(keyValue);
                if (result)
                {
                    DataItemDetailBLL di = new DataItemDetailBLL();
                    string apiUrl = di.GetItemValue("bzAppUrl");
                    if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                    {
                        WebClient wc = new WebClient();
                        wc.Encoding = Encoding.UTF8;
                        wc.Headers.Add("Content-Type", "application/json");
                        wc.Credentials = CredentialCache.DefaultCredentials;
                        //发送请求到web api并获取返回值，默认为post方式
                        try
                        {
                            System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                            wc.UploadStringCompleted += wc_UploadStringCompleted;
                            wc.UploadStringAsync(new Uri(apiUrl + "DeleteRole/" + keyValue), "Delete", roleEntity.ToJson());

                        }
                        catch (Exception ex)
                        {
                            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
                            {
                                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
                            }
                            //将同步结果写入日志文件
                            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                            System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步删除角色失败，同步信息" + roleEntity.ToJson() + ",异常信息：" + ex.Message + "\r\n");
                        }
                    }
                    return Success("删除成功。");
                }
                else
                {
                    return Error("操作失败！");
                }
               
            }
            else
            {
                return Error("记录已不存在！");
            }

        }
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)角色信息")]
        public ActionResult SaveForm(string keyValue, RoleEntity roleEntity)
        {
            bool result= roleBLL.SaveForm(keyValue, roleEntity);
            if (result)
            {
                DataItemDetailBLL di = new DataItemDetailBLL();
                string apiUrl = di.GetItemValue("bzAppUrl");
                if (!string.IsNullOrEmpty(apiUrl))
                {
                    WebClient wc = new WebClient();
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("Content-Type", "application/json");
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    //发送请求到web api并获取返回值，默认为post方式
                    try
                    {
                        List<RoleEntity> list = new List<RoleEntity>();
                        list.Add(roleEntity);
                        System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                        wc.UploadStringCompleted += wc_UploadStringCompleted;
                        wc.UploadStringAsync(new Uri(apiUrl + "postRoles"), "post", list.ToJson());

                    }
                    catch (Exception ex)
                    {
                        if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
                        {
                            System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
                        }
                        //将同步结果写入日志文件
                        string fileName = "role_"+DateTime.Now.ToString("yyyyMMdd") + ".log";
                        System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步角色失败，同步信息" + roleEntity.ToJson() + ",异常信息：" + ex.Message + "\r\n");
                    }
                }
                return Success("操作成功。");
            }
            else
            {
                return Error("操作失败");
            }


           
        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var error = e.Error;
            //将同步结果写入日志文件
            string fileName = "role_"+DateTime.Now.ToString("yyyyMMdd") + ".log";
            if(!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
            }
            try
            {

                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步角色结果>" + e.Result + "\r\n");
            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步角色结果>" + msg + "\r\n");
            }

        }
        #endregion
    }
}
