using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using System.Web;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using BSFramework.Cache.Factory;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util.Offices;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.Desktop;
namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 描 述：三种人审批业务表
    /// </summary>
    public class ThreePeopleCheckController : MvcControllerBase
    {
        private ThreePeopleCheckBLL threepeoplecheckbll = new ThreePeopleCheckBLL();

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
        /// 三种人清单列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// 三种人导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 新增人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson,int mode=0)
        {
            Operator user = OperatorProvider.Provider.Current();

            try
            {
                if (mode == 1)
                {
                    
                    DesktopBLL deskBll = new DesktopBLL();
                    List<string> list = deskBll.GetThreeCount(user);
                    string ids = string.Join(",", list);
                    pagination.p_kid = "t.ID";
                    pagination.p_fields = string.Format("status,isover,issumbit,t.CreateUserId,t.useraccount,belongdept,applysno,t.applytype,t.createusername,t.createtime,flowname,t.createuserdeptid,checkroleid,'{0}' checkdeptid", user.DeptId);
                    pagination.p_tablename = string.Format("BIS_THREEPEOPLECHECK t left join bis_manypowercheck c on t.nodeid=c.id");
                    pagination.conditionJson = string.Format("t.id in('{0}')", ids.Replace(",", "','"));
                }
                else
                {
                    pagination.p_kid = "t.ID";
                    pagination.p_fields = "t.status,t.isover,issumbit,t.CreateUserId,t.useraccount,t.belongdept,t.applysno,t.applytype,t.createusername,t.createtime,c.flowname,t.createuserdeptid,c.checkdeptid,c.checkroleid";
                    pagination.p_tablename = "BIS_THREEPEOPLECHECK t left join bis_manypowercheck c on t.nodeid=c.id";
                    pagination.conditionJson = "1=1";
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }

                }
                var watch = CommonHelper.TimerStart();
                DataTable data = threepeoplecheckbll.GetPageList(pagination, queryJson);
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
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        [HttpGet]
        public ActionResult GetItemListJson(string applyId)
        {
            var  data = threepeoplecheckbll.GetUserList(applyId).ToList();
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetUserCacheJson(string applyId)
        {
            var data = CacheFactory.Cache().GetCache<List<ThreePeopleInfoEntity>>(applyId);
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
            var data = threepeoplecheckbll.GetEntity(keyValue);
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
            threepeoplecheckbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="ApplyUsers">人员信息</param>
        ///<param name="AuditInfo">审核信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, ThreePeopleCheckEntity entity, string ApplyUsers)
        {
            List<ThreePeopleInfoEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ThreePeopleInfoEntity>>(ApplyUsers);
            if(threepeoplecheckbll.SaveForm(keyValue, entity, list))
            {
                string status = "";
                Operator curUser = OperatorProvider.Provider.Current();
                ThreePeopleCheckEntity tp = threepeoplecheckbll.GetEntity(keyValue);
                string deptId = "";
                if (tp!=null)
                {
                    if (tp.ApplyType=="内部")
                    {
                        deptId = tp.BelongDeptId;
                    }
                    else
                    {
                        string sql = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'", entity.ProjectId);
                        DataTable dt =new DepartmentBLL().GetDataTable(sql);
                        if (dt.Rows.Count > 0)
                        {
                            deptId = dt.Rows[0][0].ToString();
                        }
                    }
                }
                else
                {
                    deptId = curUser.DeptId;
                }
                entity.IsSumbit = 1;
                ManyPowerCheckEntity mp = threepeoplecheckbll.CheckAuditPower(curUser, out status, "三种人审核", deptId, entity.Id);
                if (mp != null)
                {
                        entity.NodeId = mp.ID;
                        entity.IsOver = 0;
                        entity.Status = mp.FLOWNAME;
                        threepeoplecheckbll.SaveForm(keyValue, entity);
                }
                else
                {
                    entity.IsOver = 1;
                    entity.Status = "流程结束";
                    threepeoplecheckbll.SaveForm(keyValue, entity);
                }
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="ApplyUsers">人员信息</param>
        ///<param name="AuditInfo">审核信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Audit(string keyValue, AptitudeinvestigateauditEntity AuditInfo)
        {
            ThreePeopleCheckEntity entity=threepeoplecheckbll.GetEntity(keyValue);
            Operator curUser = OperatorProvider.Provider.Current();
            string status="0";
            ManyPowerCheckEntity mp = threepeoplecheckbll.CheckAuditPower(curUser, out status, "三种人审核", entity.CreateUserDeptId, keyValue);
            AuditInfo.AUDITPEOPLEID = curUser.UserId;
            AuditInfo.AUDITDEPTID = curUser.DeptId;
            AuditInfo.APTITUDEID = keyValue;
            AuditInfo.FlowId = entity.NodeId;
            if (mp!=null)
            {
                if (status=="0")
                {
                    return Error("对不起，您没有审核的权限");
                }
                else
                {
                 
                    new AptitudeinvestigateauditBLL().SaveForm("", AuditInfo);
                    entity.NodeId =AuditInfo.AUDITRESULT=="1"?"-100":mp.ID;
                    entity.IsOver =0;
                    entity.IsSumbit = AuditInfo.AUDITRESULT == "1" ? 0 : 1;
                    entity.Status = AuditInfo.AUDITRESULT=="1"?"被退回,请重新提交":mp.FLOWNAME;
                    threepeoplecheckbll.SaveForm(keyValue, entity, new List<ThreePeopleInfoEntity>(), AuditInfo);
                    if (AuditInfo.AUDITRESULT == "1")
                    {
                        new DepartmentBLL().ExecuteSql(string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set disable='1' where aptitudeid='{0}'", keyValue));
                        ERCHTMS.Busines.JPush.JPushApi.PushMessage(entity.UserAccount, entity.CreateUserName, "ThreePeople", keyValue);
                    }
                    return Success("操作成功。");
                }
            }
            else
            {
                if (status=="0")
                {
                    return Error("对不起，您没有审核的权限");
                }
                else
                {
                    new AptitudeinvestigateauditBLL().SaveForm("", AuditInfo);
                    entity.IsOver = AuditInfo.AUDITRESULT == "1" ? 0:1;
                    entity.IsSumbit = AuditInfo.AUDITRESULT == "1" ? 0 : 1;
                    entity.Status = AuditInfo.AUDITRESULT == "1" ? "被退回,请重新提交" :"流程结束";
                    entity.NodeId = "-100";
                    threepeoplecheckbll.SaveForm(keyValue, entity, new List<ThreePeopleInfoEntity>(), AuditInfo);
                    if (AuditInfo.AUDITRESULT == "1")
                    {
                        ERCHTMS.Busines.JPush.JPushApi.PushMessage(entity.UserAccount, entity.CreateUserName, "ThreePeople", keyValue);
                        new DepartmentBLL().ExecuteSql(string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set disable='1' where aptitudeid='{0}'", keyValue));
                    }
                    return Success("操作成功");
                }
            }
          
        }
        #endregion

        /// <summary>
        /// 导入三种人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportUsers(string applyId)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无法操作此功能";
            }
            var currUser = OperatorProvider.Provider.Current();
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
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
              
                StringBuilder sb = new StringBuilder("begin \r\n");
                IList<ThreePeopleInfoEntity> list = new List<ThreePeopleInfoEntity>();
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    DataRow dr=dt.Rows[i];
                    string userName = dr[0].ToString().Trim();
                    string idCard = dr[1].ToString().Trim();
                    string userType = dr[2].ToString().Trim();
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(idCard) || string.IsNullOrEmpty(userType))
                    {
                        falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    //---****身份证正确验证*****--
                    if (!Regex.IsMatch(idCard, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                    {
                        falseMessage += "</br>" + "第" + (i + 3) + "行身份证号格式有误,未能导入.";
                        error++;
                        continue;
                    }
                    if (list.Count(t => t.IdCard == idCard) > 0)
                    {
                        falseMessage += "</br>" + "第" + (i + 3) + "行人员身份证信息已存在,未能导入.";
                        error++;
                        continue;
                    }
                    else
                    {
                        list.Add(new ThreePeopleInfoEntity
                        {
                            UserName = userName,
                            IdCard = idCard,
                            ApplyId = applyId,
                            OrgCode = currUser.OrganizeCode,
                            TicketType = userType
                        });
                    }

                }
                //sb.Append("end \r\n commit;");
                if (dt.Rows.Count>0)
                {
                    CacheFactory.Cache().WriteCache(list, applyId, DateTime.Now.AddMinutes(30));
                   // new DepartmentBLL().ExecuteSql(sb.ToString());
                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
        }

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出三种人清单")]
        public ActionResult ExportData(string queryJson)
        {
            UserBLL userBLL = new UserBLL();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "u.createdate";
            pagination.sord = "desc";
            pagination.rows = 100000000;
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "REALNAME,GENDER,identifyid,MOBILE,DEPTNAME,fourpersontype";
            pagination.p_tablename = "v_userinfo u";
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
            pagination.conditionJson = string.IsNullOrEmpty(where) ? "Account!='System'" : where;
            pagination.conditionJson += " and isfourperson='是'";
            var data = userBLL.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "三种人清单";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "三种人清单.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "姓名", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "性别", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identifyid", ExcelColumn = "身份证号", Alignment = "center",Width=200 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "电话", Alignment = "center", Width = 150 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "单位/部门", Alignment = "center", Width = 200 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fourpersontype", ExcelColumn = "四种人类别", Alignment = "center" });




            //调用导出方法
            // ExcelHelper.ExcelDownload(data, excelconfig);

            ExcelHelper.ExportByAspose(data, "三种人清单_"+DateTime.Now.ToString("yyyyMMdd"), excelconfig.ColumnEntity);
            return Success("导出成功。");
        }
        #endregion

    }
}
