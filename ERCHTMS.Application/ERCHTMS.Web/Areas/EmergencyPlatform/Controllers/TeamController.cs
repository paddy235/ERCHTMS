using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using System.Drawing;
using System.Web;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Text.RegularExpressions;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Entity.EmergencyPlatform;
using System.Linq;
using System.Linq.Expressions;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：应急队伍
    /// </summary>
    public class TeamController : MvcControllerBase
    {
        private TeamBLL teambll = new TeamBLL();
        private UserBLL userBLL = new UserBLL();
        private PostBLL postBLL = new PostBLL();
        private DepartmentBLL departBLL = new DepartmentBLL();
        private OrganizeBLL orgBLL = new OrganizeBLL();


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
        public ActionResult Import()
        {
            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "TEAMID";
            pagination.p_fields = "TEAMID as Id,CREATEUSERID, Mobile, POSTID,USERID,PostName,UserFullName,DepartName,DepartId,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode,orgname,orgcode,remark";
            pagination.p_tablename = "v_mae_team t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }


            var watch = CommonHelper.TimerStart();
            var data = teambll.GetPageList(pagination, queryJson);
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
            var data = teambll.GetList(queryJson);
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
            var data = teambll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportTeam(string PostId)
        {
            //if (OperatorProvider.Provider.Current().IsSystem)
            //{
            //    return "超级管理员无此操作权限";
            //}
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司
            string deptId = PostId;//所属部门
            int error = 0;
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
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()) || string.IsNullOrEmpty(dt.Rows[i][1].ToString()) || string.IsNullOrEmpty(dt.Rows[i][2].ToString()) || string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    //应急组织机构
                    string orgname = dt.Rows[i][0].ToString();
                    DataItemCache dataItemCache = new DataItemCache();
                    var orgItem = dataItemCache.GetDataItemList().Where(e => e.EnCode == "MAE_ORG" && e.ItemName == orgname).FirstOrDefault();
                    string OrgCode = orgItem == null ? "" : orgItem.ItemCode;
                    //if (orgItem == null)
                    //{
                    //    falseMessage += "第" + i + "行导入失败,应急组织机构不存在！</br>";
                    //    error++;
                    //    continue;
                    //}
                    //应急职务
                    string postname = dt.Rows[i][1].ToString();
                    var post = dataItemCache.GetDataItemList().Where(e => e.EnCode == "MAE_TEAM_ZW" && e.ItemName == postname).FirstOrDefault();
                    //if (post == null)
                    //{
                    //    falseMessage += "第" + i + "行导入失败,应急职务不存在！</br>";
                    //    error++;
                    //    continue;
                    //}
                    string postid = post == null ? "" : post.ItemValue;
                    //姓名
                    string usernmae = dt.Rows[i][2].ToString();
                    
                    //所属部门
                    string departname = dt.Rows[i][3].ToString();
                    var depart = departBLL.GetList().Where(e => e.FullName == departname && e.OrganizeId == orgId).FirstOrDefault();
                    var org = orgBLL.GetList().Where(e => e.FullName == departname && e.OrganizeId == orgId).FirstOrDefault();
                    if (depart == null && org == null)
                    {
                        falseMessage += "第" + i + "行导入失败,所属部门不存在！</br>";
                        error++;
                        continue;
                    }
                    //联系方式
                    string mobile = dt.Rows[i][4].ToString();
                    //备注
                    string remark = dt.Rows[i][5].ToString();

                    Expression<Func<UserEntity, bool>> condition = e => e.OrganizeId == orgId && e.RealName == usernmae && e.DepartmentId == depart.DepartmentId;
                    var user = userBLL.GetListForCon(condition).FirstOrDefault();
                    if (user == null)
                    {
                        falseMessage += "第" + i + "行导入失败,该用户关联信息不存在！</br>";
                        error++;
                        continue;
                    }


                    try
                    {
                        var item = new TeamEntity { OrgName = orgname, OrgCode = OrgCode, MOBILE = mobile, USERFULLNAME = usernmae, POSTNAME = postname, POSTID = postid, DEPARTID = depart == null ? org.OrganizeId : depart.DepartmentId, DEPARTNAME = departname, USERID = user.UserId, Remark = remark };
                        var listcheck = teambll.GetList(string.Format(" and USERFULLNAME='{0}' and Mobile='{1}' and DepartId='{2}' and POSTID='{3}'", item.USERFULLNAME, item.MOBILE, item.DEPARTID, item.POSTID));
                        if (listcheck != null && listcheck.Count() > 0)
                        {
                            falseMessage += "应急职务:" + item.POSTNAME + ",所属部门" + item.DEPARTNAME + ",姓名:" + item.USERFULLNAME + "的信息已经存在！";
                            //return Error("该条数据已存在。");
                        }
                        else
                            teambll.SaveForm("", item);

                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }

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
            teambll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TeamEntity entity)
        {
            teambll.SaveForm(keyValue, entity);

            return Success("操作成功。");
        }

        [HttpPost]
        public ActionResult SaveListForm()
        {
            string data = Request["param"];
            var list = data.ToObject<List<TeamEntity>>();
            string errormessage = "";
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.TEAMID))
                {
                    var listcheck = teambll.GetList(string.Format(" and USERFULLNAME='{0}' and Mobile='{1}' and DepartId='{2}' and POSTNAME='{3}'", item.USERFULLNAME, item.MOBILE, item.DEPARTID, item.POSTNAME));
                    if (listcheck != null && listcheck.Count() > 0)
                    {
                        errormessage += "应急职务:" + item.POSTNAME + ",姓名:" + item.USERFULLNAME + "的信息已经存在,保存失败！<BR>";
                        //return Error("该条数据已存在。");
                    }
                }
                else
                {
                    var listcheck = teambll.GetList(string.Format(" and  USERFULLNAME='{0}' and Mobile='{1}' and DepartId='{2}' and POSTNAME='{3}' and TEAMID!='{4}'", item.USERFULLNAME, item.MOBILE, item.DEPARTID, item.POSTNAME, item.TEAMID));
                    if (listcheck != null && listcheck.Count() > 0)
                    {
                        errormessage += "应急职务:" + item.POSTNAME + ",姓名:" + item.USERFULLNAME + "的信息已经存在,保存失败！<BR>";
                        //return Error("该条数据已存在。");
                    }

                   
                }
            }
            if (errormessage.Length > 0)
                return Error(errormessage);
            foreach (var item in list)
            {
                 teambll.SaveForm(item.TEAMID, item);
            }
  
            return Success("操作成功。");
        }

        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出用户数据")]
        public ActionResult ExportTeamList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "TEAMID";
            pagination.p_fields = " OrgName,PostName,UserFullName,Mobile,DepartName,remark";
            pagination.p_tablename = "V_MAE_TEAM t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "createdate";//排序字段
            pagination.sord = "desc";//排序方式  
            #region 权限校验
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = teambll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "应急队伍";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "应急队伍.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "orgname", ExcelColumn = "应急组织机构" });
            listColumnEntity.Add(new ColumnEntity() { Column = "postname", ExcelColumn = "应急职务" });
            listColumnEntity.Add(new ColumnEntity() { Column = "userfullname", ExcelColumn = "姓名" });
            listColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "联系方式" });
            listColumnEntity.Add(new ColumnEntity() { Column = "departname", ExcelColumn = "所属部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "备注" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion


    }
}
