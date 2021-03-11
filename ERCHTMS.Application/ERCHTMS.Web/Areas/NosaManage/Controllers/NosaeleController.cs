using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.NosaManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 描 述：元素表
    /// </summary>
    public class NosaeleController : MvcControllerBase
    {
        private NosaeleBLL nosaelebll = new NosaeleBLL();
        private UserBLL userbll = new UserBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private NosaareaBLL nosaareabll = new NosaareaBLL();

        #region 视图功能
        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 选择页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)//EHS部门Code
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)//EHS部门Code
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        #endregion

        #region 获取数据      
        /// <summary>
        /// 是否存在相同编号的元素
        /// </summary>
        /// <param name="keyValue">id</param>
        /// <param name="No">编号</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult ExistEleNo(string keyValue,string No)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}' and id<>'{2}'", user.OrganizeCode, No, keyValue)).ToList();
            var r = oldList.Count > 0;

            return Success("已存在该元素", r);
        }  
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();           
            var data = nosaelebll.GetList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nosaelebll.GetEntity(keyValue);
            //返回值
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
        }
        /// <summary>
        /// 获取元素树节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEleTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            var data = nosaelebll.GetList(where).OrderBy(t => t.NO).ToList();
            foreach (var item in data)
            {
                var hasChild = data.Where(x=>x.ParentId==item.ID).Count()>0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "No";
                tree.AttributeA = "DutyDepartId";
                tree.AttributeB = "DutyDepartName";
                tree.AttributeC = "DutyUserId";
                tree.AttributeD = "DutyUserName";
                tree.AttributeValue = item.NO;
                tree.AttributeValueA = item.DutyDepartId;
                tree.AttributeValueB = item.DutyDepartName;
                tree.AttributeValueC = item.DutyUserId;
                tree.AttributeValueD = item.DutyUserName;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("-1"));
        }

        /// <summary>
        /// 获取区域树节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAreaTreeJson()
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}'", user.OrganizeCode);
            var data = nosaareabll.GetList(where).OrderBy(t => t.NO).ToList();
            foreach (var item in data)
            {
                var hasChild =  false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.Name;
                tree.value = item.ID;
                tree.parentId = "";
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "No";
                tree.AttributeA = "DutyDepartId";
                tree.AttributeB = "DutyDepartName";
                tree.AttributeC = "DutyUserId";
                tree.AttributeD = "DutyUserName";
                tree.AttributeValue = item.NO;
                tree.AttributeValueA = item.DutyDepartId;
                tree.AttributeValueB = item.DutyDepartName;
                tree.AttributeValueC = item.DutyUserId;
                tree.AttributeValueD = item.DutyUserName;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(""));
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
            nosaelebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosaeleEntity entity)
        {
            nosaelebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 导入元素
        /// <summary>
        /// 导入自评标准
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEle()
        {
            int error = 0;
            int sussceed = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }               
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);
                DataTable dt = ExcelHelper.ExcelImport(filePath);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, userbll, nosaelebll, out msg) == true)
                    {
                        var entity = GenEntity(vals, userbll, nosaelebll);
                        nosaelebll.SaveForm(entity.ID, entity);
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "第" + (i + 1) + "行" + msg + "<br/>";
                        error++;
                    }
                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + sussceed + "条，失败" + error + "条";
                message += "<br/>" + falseMessage;
                //删除临时文件
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool IsNull(object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == "";
        }
        private bool Validate(int index, object[] vals,UserBLL userbll, NosaeleBLL nosaelebll, out string msg)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 6)
            {
                msg += "，格式不正确";
                r = false;
            }
            var obj = vals[1];
            if (IsNull(obj))
            {
                msg += "，元素编号不能为空";
                r = false;
            }
            else
            {                
                var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, obj.ToString().Trim())).ToList();
                if (oldList.Count > 0)
                    msg += "，元素编号已存在";
            }
            obj = vals[2];
            if (IsNull(obj))
            {
                msg += "，元素名称不能为空";
                r = false;
            }           
            obj = vals[3];
            if (IsNull(obj))
            {
                msg += "，元素责任部门不能为空";
                r = false;
            }
          
            obj = vals[4];
            if (IsNull(obj))
            {
                msg += "，元素责任人不能为空";
                r = false;
            }
            else if (!IsNull(vals[3]))
            {
                var entity = userbll.GetUserInfoByName(vals[3].ToString().Trim(), obj.ToString().Trim());
                if (entity == null)
                {
                    msg += "，责任部门中不存在相应的负责人用户";
                    r = false;
                }
            }

            obj = vals[5];//上级元素编号
            if (!IsNull(obj))
            {
                var list = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'",user.OrganizeCode, obj.ToString().Trim()));
                if (list.Count()==0)
                {
                    msg += "，上级元素编号不存在";
                    r = false;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg += "。";
                r = false;
            }

            return r;
        }
        private NosaeleEntity GenEntity(object[] vals,UserBLL userbll, NosaeleBLL nosaelebll)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            NosaeleEntity entity = new NosaeleEntity() { ID = Guid.NewGuid().ToString() };
            var oldList = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'", user.OrganizeCode, vals[1].ToString().Trim())).ToList();
            if (oldList.Count > 0)
                entity = oldList[0];

            entity.NO = vals[1].ToString().Trim();        
            entity.Name = vals[2].ToString().Trim();        
            entity.DutyDepartName = vals[3].ToString().Trim();
            entity.DutyUserName = vals[4].ToString().Trim();
            var uEntity = userbll.GetUserInfoByName(entity.DutyDepartName, entity.DutyUserName);
            entity.DutyUserId = uEntity.UserId;
            entity.DutyDepartId = uEntity.DepartmentId;
            var obj = vals[5];//上级元素编号
            if (!IsNull(obj))
            {
                var list = nosaelebll.GetList(String.Format(" and createuserorgcode='{0}' and no='{1}'",user.OrganizeCode, obj.ToString().Trim())).ToList();
                entity.ParentId = list[0].ID;
                entity.ParentName = list[0].Name;
            }

            return entity;
        }
        #endregion
    }
}
