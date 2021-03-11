using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System;
using System.Data;
using BSFramework.Util.Offices;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：隐患标准库
    /// </summary>
    public class HtStandardController : MvcControllerBase
    {
        private HtStandardBLL htstandardbll = new HtStandardBLL();

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
        [HttpGet]
        public ActionResult ItemForm()
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
            return View();
        }
        [HttpGet]
        public ActionResult SelectItems()
        {
            return View();
        }
        /// <summary>
        ///导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = htstandardbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            pagination.p_kid = "a.id";
            pagination.p_fields = "content,require,num,norm,stid,case when d.name is not null then d.name || '>' || c.name || '>' || b.name when e.name is not  null then e.name || '>' || d.name || '>' || c.name || '>' || b.name when f.name is not  null then f.name || '>' || e.name || '>' || d.name || '>' || c.name || '>' || b.name else c.name || '>' || b.name end name";
            pagination.p_tablename = "BIS_HTSTANDARDITEM a left join BIS_HTSTANDARD b on a.stid=b.id left join BIS_HTSTANDARD c on b.parentid=c.id left join BIS_HTSTANDARD d on c.parentid=d.id left join BIS_HTSTANDARD e on d.parentid=e.id left join BIS_HTSTANDARD f on e.parentid=f.id";
            //if (queryJson.ToJObject()["enCode"].IsEmpty())
                pagination.conditionJson = "a.createuserorgcode='"+user.OrganizeCode+"'";
            //else
            //    pagination.conditionJson = " 1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = htstandardbll.GetPageList(pagination, queryJson);
            foreach(DataRow dr in data.Rows)
            {
                dr["num"] = htstandardbll.GetNumber(dr["id"].ToString());
            }
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
        [HttpPost]
        public ActionResult GetItemsJson(Pagination pagination, string queryJson,string users="",string userIds="",string codes="")
        {
            Operator user = OperatorProvider.Provider.Current();
            pagination.p_kid = string.Format("a.id rid,('{0}' || rownum) pkid ", Guid.NewGuid().ToString());
            pagination.p_fields = "'' checkobjecttype,content,require,norm,stid,case when d.name is not null then d.name || '>' || c.name || '>' || b.name when e.name is not  null then e.name || '>' || d.name || '>' || c.name || '>' || b.name when f.name is not  null then f.name || '>' || e.name || '>' || d.name || '>' || c.name || '>' || b.name else c.name || '>' || b.name end name";
            pagination.conditionJson = " 1=1 ";
            //if (!string.IsNullOrEmpty(users))
            //{
                pagination.p_fields += string.Format(",'{0}' CheckMan,'{1}' CheckManID,'{2}' BelongDept ", users, userIds, codes);
            //}
            pagination.p_tablename = "BIS_HTSTANDARDITEM a left join BIS_HTSTANDARD b on a.stid=b.id left join BIS_HTSTANDARD c on b.parentid=c.id left join BIS_HTSTANDARD d on c.parentid=d.id left join BIS_HTSTANDARD e on d.parentid=e.id left join BIS_HTSTANDARD f on e.parentid=f.id";
            //if (queryJson.ToJObject()["enCode"].IsEmpty())
            //pagination.conditionJson = " and a.createuserorgcode='" + user.OrganizeCode + "'";
            //else
            //    pagination.conditionJson = " 1=1 ";
            //pagination.sidx = "stid";
            //pagination.sord = "asc";
            var watch = CommonHelper.TimerStart();
            var data = htstandardbll.GetPageList(pagination, queryJson);
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
     
        public ActionResult GetGrpPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            pagination.p_kid = "a.id";
            pagination.p_fields = "content,require,num,norm,name,stid";
            pagination.p_tablename = "BIS_HTSTANDARDITEM a left join BIS_HTSTANDARD b on a.stid=b.id";
            pagination.conditionJson = " 1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = htstandardbll.GetPageList(pagination, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                dr["num"] = htstandardbll.GetNumber(dr["id"].ToString());
            }
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
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
       //[HttpGet]
        public ActionResult GetStandardTreeJson(string isCheck,string ids="",string selIds="",string tid="")
        {
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();
            selIds = "," + selIds.Trim(',') + "," ;
            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode='{0}' or Id='0'", user.OrganizeCode);
            var data = htstandardbll.GetList(where).OrderBy(t=>t.EnCode).ToList();
            try
            {
                 StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(tid))
            {
                HtStandardEntity hs=htstandardbll.GetEntity(tid);
                if(hs!=null)
                {
                    DataTable dt = new ERCHTMS.Busines.BaseManage.DepartmentBLL().GetDataTable(string.Format("select id from BIS_HTSTANDARD t where instr('{0}',encode)=1", hs.EnCode));
                    foreach(DataRow dr in dt.Rows)
                    {
                     sb.AppendFormat("{0},",dr[0].ToString());
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(ids))
            {
                ids = ids.Trim(',');
                ids = "," + ids + ",";
            }
            foreach (HtStandardEntity item in data)
            {
                if (!selIds.Contains(","+item.Id+","))
                {
                    string hasChild = htstandardbll.IsHasChild(item.Id) ? "1" : "0";
                    TreeEntity tree = new TreeEntity();
                    tree.id = item.Id;
                    tree.text = item.Name.Replace("\n", "").Replace("\\", "╲");
                    tree.value = item.Id;
                    tree.parentId = item.Parentid;
                    tree.isexpand = (item.Parentid == "-1") ? true : false;
                    tree.complete = true;
                    if (!string.IsNullOrWhiteSpace(ids))
                    {
                        tree.checkstate = ids.Contains("," + item.Id + ",") ? 1 : 0;
                    }
                    tree.hasChildren = hasChild == "1" ? true : false;
                    tree.Attribute = "Code";
                    tree.showcheck = string.IsNullOrEmpty(isCheck) ? false : true;
                    if(!string.IsNullOrEmpty(tid))
                    {
                        if (sb.ToString().Contains(item.Id))
                        {
                            tree.isexpand = true;
                        }
                    }
                    tree.AttributeValue = item.EnCode + "|" + hasChild;
                    treeList.Add(tree);
                }
               
                
            }
            return Content(treeList.TreeToJson("-1"));
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// 获取省公司标准树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetGrpStandardTreeJson(string isCheck, string ids = "")
        {
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            var where = string.Format(" and CreateUserOrgCode!='{0}' and CreateUserOrgCode in(select encode from base_department start with encode='{0}' connect by  prior parentid = departmentid)", user.OrganizeCode);
            var data = htstandardbll.GetList(where).OrderBy(t => t.EnCode).ToList();
            foreach (HtStandardEntity item in data)
            {
                string hasChild = htstandardbll.IsHasChild(item.Id) ? "1" : "0";
                TreeEntity tree = new TreeEntity();
                tree.id = item.Id;
                tree.text = item.Name.Replace("\n", "").Replace("\\", "╲");
                tree.value = item.Id;
                tree.parentId = item.Parentid;
                tree.checkstate = ids.Contains(item.Id) ? 1 : 0;
                tree.isexpand = (item.Parentid == "-1") ? true : false;
                tree.complete = true;
                tree.hasChildren = hasChild == "1" ? true : false;
                tree.Attribute = "Code";
                tree.showcheck = string.IsNullOrEmpty(isCheck) ? false : true;
                tree.AttributeValue = item.EnCode + "|" + hasChild;
                treeList.Add(tree);

            }
            return Content(treeList.TreeToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = htstandardbll.GetEntity(keyValue);
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
            htstandardbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HtStandardEntity entity)
        {
            htstandardbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 导入隐患标准库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入隐患标准库")]
        public string ImportData()
        {
            int error = 0;
            string message = "数据导入成功";
            StringBuilder sb = new StringBuilder("具体错误位置：<br />");
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                count=dt.Rows.Count-1;
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    string one = dt.Rows[j][1].ToString();
                    string content = dt.Rows[j][6].ToString();
                    string require = dt.Rows[j][7].ToString();
                    if (string.IsNullOrEmpty(one) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(require))
                    {
                        sb.AppendFormat("第{0}行：一级标准分类或隐患描述或者整改要求为空！<br />",j);
                        error++;
                        continue;
                    }
                    string two = dt.Rows[j][2].ToString();
                    string three = dt.Rows[j][3].ToString();
                    string four = dt.Rows[j][4].ToString();
                    string five = dt.Rows[j][5].ToString();
                  
                    string norm = dt.Rows[j][8].ToString();
                    try
                    {
                        htstandardbll.Save(one, two, three, four, five, content, require, norm);
                    }
                    catch(Exception ex)
                    {
                        sb.AppendFormat("第{0}行：{1}<br />",j,ex.Message);
                        error++;
                    }
                    
                }
            }
            if (error>0)
            {
                message = string.Format("共有{0}条数据，导入失败{1}条数据，{2}",count,error,sb.ToString());
            }
            return message;
        }
        #endregion
    }
}