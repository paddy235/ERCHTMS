using System.Data;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// 描 述：危害因素人员表
    /// </summary>
    public class HazardfactoruserController : MvcControllerBase
    {
        private HazardfactoruserBLL hazardfactoruserbll = new HazardfactoruserBLL();

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
            var data = hazardfactoruserbll.GetList(queryJson);
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
            var data = hazardfactoruserbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            DepartmentBLL departmentBLL = new DepartmentBLL();


            string sqlwhere = "Account!='System' and us is not null";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                sqlwhere = "1=1 and us is not null";
            }
            else
            {
                var queryParam = queryJson.ToJObject();
                if (queryParam["datatype"].IsEmpty())
                {
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                    if (!string.IsNullOrEmpty(where) && (queryParam["code"].IsEmpty() || !queryJson.Contains("code")))
                    {
                        sqlwhere += " and " + where;
                    }
                }

            }



            var data = new HazardfactoruserBLL().GetPageList(sqlwhere, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                {
                    DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["DEPARTMENTCODE"], "部门", dr["organizeid"]));
                    if (dt.Rows.Count > 0)
                    {
                        string name = "";
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            name += dr1["fullname"].ToString() + "/";
                        }
                        dr["deptname"] = name.TrimEnd('/');
                    }
                }
            }

            for (int i = 0; i < data.Rows.Count; i++)
            {
                data.Rows[i][0] = i + 1;
            }
            string FileUrl = @"\Resource\ExcelTemplate\接触职业危害因素人员_导出模板.xlsx";
            AsposeExcelHelper.ExecuteResult(data, FileUrl, "接触职业危害因素人员清单", "接触职业危害因素人员列表");

            return Success("导出成功。");
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
            hazardfactoruserbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HazardfactoruserEntity entity)
        {
            hazardfactoruserbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
