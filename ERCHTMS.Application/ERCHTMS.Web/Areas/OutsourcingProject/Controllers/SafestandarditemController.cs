using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System;
using System.Data;
using BSFramework.Util.Offices;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：安全考核内容表
    /// </summary>
    public class SafestandarditemController : MvcControllerBase
    {
        private SafestandarditemBLL safestandarditembll = new SafestandarditemBLL();
        private SafestandardBLL safestandardbll = new SafestandardBLL();

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
        /// 新增表单 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ItemForm()
        {
            return View();
        }

        /// <summary>
        /// 选择标准
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectItems()
        {
            return View();
        }


        /// <summary>
        /// 导入页面
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
            pagination.p_fields = "content,require,num,norm,stid,case when d.name is not null then d.name || '>' || c.name || '>' || b.name when e.name is not  null then e.name || '>' || d.name || '>' || c.name || '>' || b.name when f.name is not  null then f.name || '>' || e.name || '>' || d.name || '>' || c.name || '>' || b.name else c.name || '>' || b.name end name, b.name as dname," +
                "(case when d.PARENTID = '0' then d.name  when e.PARENTID = '0' then e.name when f.PARENTID = '0'   then f.name  when c.PARENTID = '0'   then c.name else b.name end ) as typename";
            pagination.p_tablename = "EPG_SAFESTANDARDITEM a left join EPG_SAFESTANDARD    b on a.stid=b.id left join EPG_SAFESTANDARD    c on b.parentid=c.id left join EPG_SAFESTANDARD    d on c.parentid=d.id left join EPG_SAFESTANDARD    e on d.parentid=e.id left join EPG_SAFESTANDARD    f on e.parentid=f.id";
            //if (queryJson.ToJObject()["enCode"].IsEmpty())
            pagination.conditionJson = "a.createuserorgcode='" + user.OrganizeCode + "'";
            //else
            //    pagination.conditionJson = " 1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = safestandarditembll.GetPageList(pagination, queryJson);
            //foreach (DataRow dr in data.Rows)
            //{
            //    dr["num"] = safestandarditembll.GetNumber(dr["id"].ToString());
            //}
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safestandarditembll.GetList(queryJson);
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
            var data = safestandarditembll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
       //[HttpGet]
        public ActionResult GetStandardTreeJson(string isCheck, string ids = "", string selIds = "", string tid = "")
        {
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();
            selIds = "," + selIds.Trim(',') + ",";
            var treeList = new List<TreeEntity>();
            var where = string.Format(" and (CreateUserOrgCode='{0}' or Id='0')", user.OrganizeCode);
            var data = safestandardbll.GetList(where).OrderBy(t => t.ENCODE).ToList();
            //var data = safestandardbll.GetList(where).ToList();
            try
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(tid))
                {
                    SafestandardEntity hs = safestandardbll.GetEntity(tid);
                    if (hs != null)
                    {
                        DataTable dt = new ERCHTMS.Busines.BaseManage.DepartmentBLL().GetDataTable(string.Format("select id from EPG_SAFESTANDARD t where instr('{0}',encode)=1", hs.ENCODE));
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.AppendFormat("{0},", dr[0].ToString());
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    ids = ids.Trim(',');
                    ids = "," + ids + ",";
                }
                foreach (SafestandardEntity item in data)
                {
                    if (!selIds.Contains("," + item.ID + ","))
                    {
                        string hasChild = safestandardbll.IsHasChild(item.ID) ? "1" : "0";
                        TreeEntity tree = new TreeEntity();
                        tree.id = item.ID;
                        tree.text = item.NAME.Replace("\n", "").Replace("\\", "╲");
                        tree.value = item.ID;
                        tree.parentId = item.PARENTID;
                        tree.isexpand = (item.PARENTID == "-1") ? true : false;
                        tree.complete = true;
                        if (!string.IsNullOrWhiteSpace(ids))
                        {
                            tree.checkstate = ids.Contains("," + item.ID + ",") ? 1 : 0;
                        }
                        tree.hasChildren = hasChild == "1" ? true : false;
                        tree.Attribute = "Code";
                        tree.showcheck = string.IsNullOrEmpty(isCheck) ? false : true;
                        if (!string.IsNullOrEmpty(tid))
                        {
                            if (sb.ToString().Contains(item.ID))
                            {
                                tree.isexpand = true;
                            }
                        }
                        tree.AttributeValue = item.ENCODE + "|" + hasChild;
                        treeList.Add(tree);
                    }


                }
                return Content(treeList.TreeToJson("-1"));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 获取安全考核类型实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetSafeStandardFormJson(string keyValue)
        {
            var data = safestandardbll.GetEntity(keyValue);
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
            safestandarditembll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafestandarditemEntity entity)
        {
            var st = safestandardbll.GetEntity(entity.STID);
            entity.STCODE = st.ENCODE;
            safestandarditembll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 保存考核分类树表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveStandardForm(string keyValue, SafestandardEntity entity)
        {
            safestandardbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 删除考核分类树数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveStandardForm(string keyValue)
        {
            safestandardbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 导入隐患标准库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入安全考核标准库")]
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
                count = dt.Rows.Count - 1;
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    string one = dt.Rows[j][1].ToString();
                    string content = dt.Rows[j][6].ToString();
                    string require = dt.Rows[j][7].ToString();
                    if (string.IsNullOrEmpty(one) || string.IsNullOrEmpty(content))
                    {
                        sb.AppendFormat("第{0}行：一级标准分类或考核内容要求不为空！<br />", j);
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
                        safestandardbll.Save(one, two, three, four, five, content, require, norm);
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("第{0}行：{1}<br />", j, ex.Message);
                        error++;
                    }

                }
            }
            if (error > 0)
            {
                message = string.Format("共有{0}条数据，导入失败{1}条数据，{2}", count, error, sb.ToString());
            }
            return message;
        }
        #endregion
    }
}
