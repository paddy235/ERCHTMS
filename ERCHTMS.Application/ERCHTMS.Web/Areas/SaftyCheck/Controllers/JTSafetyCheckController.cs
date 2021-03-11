using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.Busines.SaftyCheck;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Web;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.IO;
using ERCHTMS.Entity.PublicInfoManage;
using System.Threading;
using System.Data;
using BSFramework.Util.Offices;
using System.Linq;
using ERCHTMS.Busines.JTSafetyCheck;
using ERCHTMS.Entity.JTSafetyCheck;
using BSFramework.Util.Extension;
using System.Text;
using Aspose.Cells;
using System.Threading.Tasks;

namespace ERCHTMS.Web.Areas.SaftyCheck.Controllers
{
    /// <summary>
    /// 描 述：安全检查表
    /// </summary>
    public class JTSafetyCheckController : MvcControllerBase
    {
        private JTSafetyCheckBLL saftycheckdatabll = new JTSafetyCheckBLL();
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
        [HttpGet]
        public ActionResult List()
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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details()
        {
            return View();
        }

        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
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
            var data = saftycheckdatabll.GetList(queryJson);
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
            try
            {
                SafetyCheckEntity data = saftycheckdatabll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        [HttpGet]
        public ActionResult GetItemFormJson(string keyValue)
        {
            CheckItemsEntity data = saftycheckdatabll.GetItemEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 安全检查表列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   

        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "id";
                pagination.p_fields = "checktitle,checktype,startdate,enddate,0 total,0 count1,0 count2,createuserid";
                pagination.p_tablename = @"jt_safetycheck t";
                pagination.conditionJson = "1=1";
                var user = OperatorProvider.Provider.Current();
                //if (!user.IsSystem)
                //{
                //    if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
                //    {
                //        pagination.conditionJson = string.Format("deptcode like '{0}%'", user.OrganizeCode);
                //    }
                //    else
                //    {
                //        pagination.conditionJson = string.Format("deptcode like '{0}%' or (CheckDeptCode || ',') like '%{0},%'", user.DeptCode);
                //    }
                //}
                var watch = CommonHelper.TimerStart();
                var data = saftycheckdatabll.GetPageList(pagination, queryJson);
                DepartmentBLL deptBll = new DepartmentBLL();
                foreach (DataRow dr in data.Rows)
                {
                    string id = dr["id"].ToString();
                    string sql = string.Format("select count(1) from JT_CHECKITEMS where checkid='{0}'", id);
                    int total = deptBll.GetDataTable(sql).Rows[0][0].ToInt();
                    dr["total"] = total;
                    string sql1 = sql + string.Format(" and result='已完成'");
                    int count = deptBll.GetDataTable(sql1).Rows[0][0].ToInt();
                    dr["count1"] = count;
                    count = 0;
                    sql1 = string.Format("select realitydate,plandate from JT_CHECKITEMS where checkid='{0}' and result='未完成' and plandate is not null", id);
                    DataTable dtCount = deptBll.GetDataTable(sql1);
                    if (dtCount.Rows.Count>0)
                    {
                        count = dtCount.AsEnumerable().Where(t => t.Field<DateTime>("plandate")<DateTime.Now.ToString("yyyy-MM-dd 00:00:00").ToDate()).Count();
                        dr["count2"] = count;

                    }
                    dr["count2"] = count;

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
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        public ActionResult GetItemsListJson(string checkId,string status="")
        {
            try
            {
                var data = saftycheckdatabll.GetItemsList(checkId,status);
                if (status=="逾期未完成")
                {
                    if (data.Rows.Count > 0)
                    {
                        var items = data.AsEnumerable().Where(t => t.Field<DateTime>("plandate") < DateTime.Now.ToString("yyyy-MM-dd 00:00:00").ToDate()).Select(t => new
                        {
                            id=t.Field<string>("id"),
                            createuserid = t.Field<string>("createuserid"),
                            itemname = t.Field<string>("itemname"),
                            measures = t.Field<string>("measures"),
                            deptname = t.Field<string>("deptname"),
                            dutyuser = t.Field<string>("dutyuser"),
                            plandate = t.Field<DateTime>("plandate"),
                            realitydate = t.Field<string>("realitydate"),
                            checkuser = t.Field<string>("checkuser"),
                            result = t.Field<string>("result"),
                            remark = t.Field<string>("remark")
                        });
                        return ToJsonResult(items);
                       
                    }
                    return ToJsonResult(data);
                }
                else
                {
                    return ToJsonResult(data);
                }
              
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
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
        [HandlerMonitor(6, "删除检查表")]
        public ActionResult RemoveForm(string keyValue)
        {
            saftycheckdatabll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除检查表")]
        public ActionResult RemoveItemForm(string keyValue)
        {
            saftycheckdatabll.RemoveItemForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 导入数据到cache中
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Export(string keyValue,string title)
        {
            try
            {
                string sql = "select itemname,measures,deptname,dutyuser,to_char(plandate,'yyyy-mm-dd') plandate,to_char(realitydate,'yyyy-mm-dd') realitydate,checkuser,result,remark  from jt_checkitems where checkid='" + keyValue + "' order by sortcode asc";
                DataTable data = new DepartmentBLL().GetDataTable(sql);
                data.TableName = "T";
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/DocumentFile/安全检查导出模板.doc"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
                db.MoveToBookmark("title");
                db.InsertHtml("<font style='font-size:21px;font-weight:bold;text-decoration:underline;'>" + title + "</font>");
                doc.MailMerge.ExecuteWithRegions(data);
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                doc.Save(resp, title+"安全检查问题汇总表.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 导入数据到cache中
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000;
                pagination.p_kid = "id";
                pagination.p_fields = "checktitle,checktype,to_char(startdate,'yyyy-mm-dd') startdate,to_char(enddate,'yyyy-mm-dd') enddate,0 total,0 count1,0 count2";
                pagination.p_tablename = @"jt_safetycheck t";
                pagination.conditionJson = "1=1";
                var user = OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    pagination.conditionJson = string.Format("deptcode like '{0}%'", user.OrganizeCode);
                }
                var watch = CommonHelper.TimerStart();
                var data = saftycheckdatabll.GetPageList(pagination, queryJson);
                DepartmentBLL deptBll = new DepartmentBLL();
                string typeName = "";
                foreach (DataRow dr in data.Rows)
                {
                    string id = dr["id"].ToString();
                    string sql = string.Format("select count(1) from JT_CHECKITEMS where checkid='{0}'", id);
                    int total = deptBll.GetDataTable(sql).Rows[0][0].ToInt();
                    dr["total"] = total;
                    string sql1 = sql + string.Format(" and result='已完成'");
                    int count = deptBll.GetDataTable(sql1).Rows[0][0].ToInt();
                    dr["count1"] = count;
                    count = 0;
                    sql1 = string.Format("select realitydate,plandate from JT_CHECKITEMS where checkid='{0}' and result='未完成' and plandate is not null", id);
                    DataTable dtCount = deptBll.GetDataTable(sql1);
                    if (dtCount.Rows.Count > 0)
                    {
                        count = dtCount.AsEnumerable().Where(t => t.Field<DateTime>("plandate") < DateTime.Now.ToString("yyyy-MM-dd 00:00:00").ToDate()).Count();
                        dr["count2"] = count;

                    }
                    dr["count2"] = count;
                    string type = dr["checktype"].ToString();
                    if (type == "1")
                    {
                        typeName= "日常安全检查";
                    }
                    if (type == "2")
                    {
                        typeName = "专项安全检查";
                    }
                    if (type == "3")
                    {
                        typeName = "节假日前后安全检查";
                    }
                    if (type == "4")
                    {
                        typeName = "季节性安全检查";
                    }
                    if (type == "5")
                    {
                        typeName = "综合安全检查";
                    }
                    if (type == "6")
                    {
                        typeName = "其他安全检查";
                    }
                    dr["checktype"] = typeName;

                }
                data.Columns.Remove("id");
                List<ColumnEntity> list = new List<ColumnEntity>();
                list.Add(new ColumnEntity {
                    Column = "checktitle",
                    ExcelColumn = "检查名称", 
                });
                list.Add(new ColumnEntity
                {
                    Column = "checktype",
                    ExcelColumn = "检查类型",
                });
                list.Add(new ColumnEntity
                {
                    Column = "startdate",
                    ExcelColumn = "检查开始时间",
                });
                list.Add(new ColumnEntity
                {
                    Column = "enddate",
                    ExcelColumn = "检查结束时间",
                });
                list.Add(new ColumnEntity
                {
                    Column = "total",
                    ExcelColumn = "检查项数",
                });
                list.Add(new ColumnEntity
                {
                    Column = "count1",
                    ExcelColumn = "整改项数",
                });
                list.Add(new ColumnEntity
                {
                    Column = "count2",
                    ExcelColumn = "超期未整改项数",
                });
                ExcelHelper.ExportByAspose(data, "安全检查记录", list);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 导入数据到cache中
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData(string checkId)
        {
            try
            {
                int error = 0;
                string message = "请选择格式正确的文件再导入!";
                StringBuilder falseMessage = new StringBuilder();
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
                    Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    int success = dt.Rows.Count - 1;
                    var user = OperatorProvider.Provider.Current();
                    DepartmentBLL deptBll = new DepartmentBLL();
                    List<CheckItemsEntity> list = new List<CheckItemsEntity>();
                    List<int> lstRowIndex = new List<int>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string itemName = dt.Rows[i][1].ToString().Trim();//治理措施
                        string measures = dt.Rows[i][2].ToString().Trim();//治理措施
                        string dutyDept = dt.Rows[i][3].ToString().Trim();//责任单位
                        string dutyUser = dt.Rows[i][4].ToString().Trim();//责任人
                        string planDate = dt.Rows[i][5].ToString().Trim();//计划完成时间
                        string realityDate = dt.Rows[i][6].ToString().Trim();//实际完成时间
                        string checkDept = dt.Rows[i][7].ToString().Trim();//验收单位
                        string checkUser = dt.Rows[i][8].ToString().Trim();//验收人
                        string result = dt.Rows[i][9].ToString().Trim();//整改结果
                        string remark = dt.Rows[i][10].ToString().Trim();//备注
                        string dutyDeptCode = "";
                        string dutyDeptId = "";
                        string dutyUserId = "";
                        string checkDeptId = "";
                        string checkUserId = "";
                        if (string.IsNullOrWhiteSpace(itemName))
                        {
                            if(!lstRowIndex.Contains(i + 2))
                            {
                                lstRowIndex.Add(i + 2);
                            }
                            falseMessage.AppendFormat("</br>第{0}行问题项目不能为空！", i + 1);
                            error++;
                            continue;
                        }
                        if(!string.IsNullOrWhiteSpace(planDate))
                        {
                            DateTime time = new DateTime();
                            if(!DateTime.TryParse(planDate,out time))
                            {
                                if (!lstRowIndex.Contains(i + 2))
                                {
                                    lstRowIndex.Add(i + 2);
                                }
                                falseMessage.AppendFormat("</br>第{0}行计划完成时间格式不正确！", i + 1);
                                error++;
                                continue;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(realityDate))
                        {
                            DateTime time = new DateTime();
                            if (!DateTime.TryParse(realityDate, out time))
                            {
                                if (!lstRowIndex.Contains(i + 2))
                                {
                                    lstRowIndex.Add(i + 2);
                                }
                                falseMessage.AppendFormat("</br>第{0}行实际完成时间格式不正确！", i + 1);
                                error++;
                                continue;
                            }
                        }
                        DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1) from jt_checkitems where CheckId='{4}' and ItemName='{0}' and Measures='{1}' and DutyUser='{2}' and DeptName='{3}'", itemName,measures,dutyUser,dutyDept,checkId));
                        if (dtCount.Rows[0][0].ToInt() >0)
                        {
                            if (!lstRowIndex.Contains(i + 2))
                            {
                                lstRowIndex.Add(i + 2);
                            }
                            falseMessage.AppendFormat("</br>第{0}行记录已存在,无法导入！", i + 1);
                            error++;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(dutyDept))
                        {
                            DataTable dtDept = deptBll.GetDataTable(string.Format("select departmentid,encode  from base_department where fullname='{0}'", dutyDept));
                            if (dtDept.Rows.Count == 0)
                            {
                                //falseMessage.AppendFormat("</br>第{0}行责任单位与系统不匹配！", i + 2);
                                //error++;
                                //continue;
                            }
                            else
                            {
                                dutyDeptCode = dtDept.Rows[0][1].ToString();
                                dutyDeptId = dtDept.Rows[0][0].ToString();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(dutyUser))
                        {
                            if (!string.IsNullOrWhiteSpace(dutyDeptId))
                            {
                                DataTable dtUser = deptBll.GetDataTable(string.Format("select userid from base_user where  organizeid='{2}' and realname='{0}' and departmentid='{1}' ", dutyUser, dutyDeptId, user.OrganizeId));
                                if (dtUser.Rows.Count == 0)
                                {
                                    //falseMessage.AppendFormat("</br>第{0}行责任人与系统不匹配！", i + 2);
                                    //error++;
                                    //continue;
                                }
                                else
                                {
                                    dutyUserId = dtUser.Rows[0][0].ToString();
                                }
                            }
                            else
                            {
                                DataTable dtUser = deptBll.GetDataTable(string.Format("select userid from base_user where  organizeid='{1}' and realname='{0}' ", dutyUser, user.OrganizeId));
                                if (dtUser.Rows.Count == 0)
                                {
                                    //falseMessage.AppendFormat("</br>第{0}行责任人与系统不匹配！", i + 2);
                                    //error++;
                                    //continue;
                                }
                                else
                                {
                                    dutyUserId = dtUser.Rows[0][0].ToString();
                                }
                            }
                        }


                        if (!string.IsNullOrWhiteSpace(checkDept))
                        {
                            DataTable dtDept = deptBll.GetDataTable(string.Format("select departmentid,encode  from base_department where fullname='{0}'", checkDept));
                            if (dtDept.Rows.Count == 0)
                            {
                                //falseMessage.AppendFormat("</br>第{0}行验收单位与系统不匹配！", i + 2);
                                //error++;
                                //continue;
                            }
                            else
                            {
                                checkDeptId = dtDept.Rows[0][0].ToString();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(checkUser))
                        {
                            if (!string.IsNullOrWhiteSpace(checkDeptId))
                            {
                                DataTable dtUser = deptBll.GetDataTable(string.Format("select userid from v_userinfo where  organizeid='{2}' and realname='{0}' and departmentid='{1}' ", checkUser, checkDeptId, user.OrganizeId));
                                if (dtUser.Rows.Count == 0)
                                {
                                    //falseMessage.AppendFormat("</br>第{0}行验收人与系统不匹配！", i + 2);
                                    //error++;
                                    //continue;
                                }
                                else
                                {
                                    checkDeptId = dtUser.Rows[0][0].ToString();
                                }
                            }
                            else
                            {
                                DataTable dtUser = deptBll.GetDataTable(string.Format("select userid from v_userinfo where  organizeid='{1}' and realname='{0}' ", checkUser, user.OrganizeId));
                                if (dtUser.Rows.Count == 0)
                                {
                                    //falseMessage.AppendFormat("</br>第{0}行验收人与系统不匹配！", i + 2);
                                    //error++;
                                    //continue;
                                }
                                else
                                {
                                    checkUserId = dtUser.Rows[0][0].ToString();
                                }
                            }
                        }
                        result = string.IsNullOrWhiteSpace(result) ? "未完成" : result;
                        CheckItemsEntity entity=new CheckItemsEntity
                        {
                            IsSend=0,
                            Id = Guid.NewGuid().ToString(),
                            ItemName = itemName,
                            Measures = measures,
                            CheckUser = checkUser,
                            CheckUserId = checkUserId,
                            DutyUser = dutyUser,
                            DutyUserId = dutyUserId,
                            DeptCode = dutyDeptCode,
                            DeptName = dutyDept,
                            PlanDate = string.IsNullOrWhiteSpace(planDate) ?null:planDate.ToDateOrNull(),
                            RealityDate = string.IsNullOrWhiteSpace(realityDate)?null:realityDate.ToDateOrNull(),
                            CreateTime = DateTime.Now,
                            CreateUserId = user.UserId,
                            Remark = remark,
                            Result = result,
                            CheckId = checkId,
                            SortCode=i
                        };
                        bool res = saftycheckdatabll.SaveItemForm("", entity);
                        if (res)
                        {

                        }
                        else
                        {
                            falseMessage.AppendFormat("</br>第{0}行导入失败！", i + 1);
                            error++;
                            continue;
                            //falseMessage.AppendFormat("</br>导入失败！！");
                            //error = dt.Rows.Count;
                        }
                    }
                    count = dt.Rows.Count;
                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                    if(lstRowIndex.Count>0)
                    {
                        foreach (int rowIndex in lstRowIndex)
                        {
                            Style style = wb.Styles[wb.Styles.Add()];
                            style.ForegroundColor= System.Drawing.Color.Red;
                            style.BackgroundColor = System.Drawing.Color.Red;
                            style.Pattern = BackgroundType.Solid;
                            cells.Rows[rowIndex].ApplyStyle(style,new StyleFlag() { All=true});
                        }
                        wb.Save(Server.MapPath("~/Resource/temp/" + fileName));
                        message += "<br /><a href='../../Resource/temp/" + fileName+"' target='_blank'>下载附件,查看详情</a>";
                    }
                }
                return message;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="projectItem">检查项目</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或者修改检查表")]
        public ActionResult SaveForm(string keyValue, SafetyCheckEntity entity)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                bool result=saftycheckdatabll.SaveForm(keyValue, entity);
                if(result)
                {
                    //给问题责任人发送短消息提醒
                    Task.Run(()=> {
                        saftycheckdatabll.SendMessage(keyValue);
                    });

                }
                return Success("操作成功。");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或者修改检查发现问题及整改情况")]
        public ActionResult SaveItemForm(string keyValue, CheckItemsEntity entity)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                saftycheckdatabll.SaveItemForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion
    }
}
