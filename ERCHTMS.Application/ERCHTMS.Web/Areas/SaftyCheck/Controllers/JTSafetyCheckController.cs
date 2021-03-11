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
    /// �� ������ȫ����
    /// </summary>
    public class JTSafetyCheckController : MvcControllerBase
    {
        private JTSafetyCheckBLL saftycheckdatabll = new JTSafetyCheckBLL();
        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details()
        {
            return View();
        }

        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = saftycheckdatabll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
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
        /// ��ȫ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   

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
                //    if (user.RoleName.Contains("��˾��") || user.RoleName.Contains("����"))
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
                    string sql1 = sql + string.Format(" and result='�����'");
                    int count = deptBll.GetDataTable(sql1).Rows[0][0].ToInt();
                    dr["count1"] = count;
                    count = 0;
                    sql1 = string.Format("select realitydate,plandate from JT_CHECKITEMS where checkid='{0}' and result='δ���' and plandate is not null", id);
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
                if (status=="����δ���")
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "ɾ������")]
        public ActionResult RemoveForm(string keyValue)
        {
            saftycheckdatabll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "ɾ������")]
        public ActionResult RemoveItemForm(string keyValue)
        {
            saftycheckdatabll.RemoveItemForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// �������ݵ�cache��
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
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/DocumentFile/��ȫ��鵼��ģ��.doc"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
                db.MoveToBookmark("title");
                db.InsertHtml("<font style='font-size:21px;font-weight:bold;text-decoration:underline;'>" + title + "</font>");
                doc.MailMerge.ExecuteWithRegions(data);
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                doc.Save(resp, title+"��ȫ���������ܱ�.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
                return Success("�����ɹ�");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// �������ݵ�cache��
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
                    string sql1 = sql + string.Format(" and result='�����'");
                    int count = deptBll.GetDataTable(sql1).Rows[0][0].ToInt();
                    dr["count1"] = count;
                    count = 0;
                    sql1 = string.Format("select realitydate,plandate from JT_CHECKITEMS where checkid='{0}' and result='δ���' and plandate is not null", id);
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
                        typeName= "�ճ���ȫ���";
                    }
                    if (type == "2")
                    {
                        typeName = "ר�ȫ���";
                    }
                    if (type == "3")
                    {
                        typeName = "�ڼ���ǰ��ȫ���";
                    }
                    if (type == "4")
                    {
                        typeName = "�����԰�ȫ���";
                    }
                    if (type == "5")
                    {
                        typeName = "�ۺϰ�ȫ���";
                    }
                    if (type == "6")
                    {
                        typeName = "������ȫ���";
                    }
                    dr["checktype"] = typeName;

                }
                data.Columns.Remove("id");
                List<ColumnEntity> list = new List<ColumnEntity>();
                list.Add(new ColumnEntity {
                    Column = "checktitle",
                    ExcelColumn = "�������", 
                });
                list.Add(new ColumnEntity
                {
                    Column = "checktype",
                    ExcelColumn = "�������",
                });
                list.Add(new ColumnEntity
                {
                    Column = "startdate",
                    ExcelColumn = "��鿪ʼʱ��",
                });
                list.Add(new ColumnEntity
                {
                    Column = "enddate",
                    ExcelColumn = "������ʱ��",
                });
                list.Add(new ColumnEntity
                {
                    Column = "total",
                    ExcelColumn = "�������",
                });
                list.Add(new ColumnEntity
                {
                    Column = "count1",
                    ExcelColumn = "��������",
                });
                list.Add(new ColumnEntity
                {
                    Column = "count2",
                    ExcelColumn = "����δ��������",
                });
                ExcelHelper.ExportByAspose(data, "��ȫ����¼", list);
                return Success("�����ɹ�");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// �������ݵ�cache��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData(string checkId)
        {
            try
            {
                int error = 0;
                string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
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

                        string itemName = dt.Rows[i][1].ToString().Trim();//�����ʩ
                        string measures = dt.Rows[i][2].ToString().Trim();//�����ʩ
                        string dutyDept = dt.Rows[i][3].ToString().Trim();//���ε�λ
                        string dutyUser = dt.Rows[i][4].ToString().Trim();//������
                        string planDate = dt.Rows[i][5].ToString().Trim();//�ƻ����ʱ��
                        string realityDate = dt.Rows[i][6].ToString().Trim();//ʵ�����ʱ��
                        string checkDept = dt.Rows[i][7].ToString().Trim();//���յ�λ
                        string checkUser = dt.Rows[i][8].ToString().Trim();//������
                        string result = dt.Rows[i][9].ToString().Trim();//���Ľ��
                        string remark = dt.Rows[i][10].ToString().Trim();//��ע
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
                            falseMessage.AppendFormat("</br>��{0}��������Ŀ����Ϊ�գ�", i + 1);
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
                                falseMessage.AppendFormat("</br>��{0}�мƻ����ʱ���ʽ����ȷ��", i + 1);
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
                                falseMessage.AppendFormat("</br>��{0}��ʵ�����ʱ���ʽ����ȷ��", i + 1);
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
                            falseMessage.AppendFormat("</br>��{0}�м�¼�Ѵ���,�޷����룡", i + 1);
                            error++;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(dutyDept))
                        {
                            DataTable dtDept = deptBll.GetDataTable(string.Format("select departmentid,encode  from base_department where fullname='{0}'", dutyDept));
                            if (dtDept.Rows.Count == 0)
                            {
                                //falseMessage.AppendFormat("</br>��{0}�����ε�λ��ϵͳ��ƥ�䣡", i + 2);
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
                                    //falseMessage.AppendFormat("</br>��{0}����������ϵͳ��ƥ�䣡", i + 2);
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
                                    //falseMessage.AppendFormat("</br>��{0}����������ϵͳ��ƥ�䣡", i + 2);
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
                                //falseMessage.AppendFormat("</br>��{0}�����յ�λ��ϵͳ��ƥ�䣡", i + 2);
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
                                    //falseMessage.AppendFormat("</br>��{0}����������ϵͳ��ƥ�䣡", i + 2);
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
                                    //falseMessage.AppendFormat("</br>��{0}����������ϵͳ��ƥ�䣡", i + 2);
                                    //error++;
                                    //continue;
                                }
                                else
                                {
                                    checkUserId = dtUser.Rows[0][0].ToString();
                                }
                            }
                        }
                        result = string.IsNullOrWhiteSpace(result) ? "δ���" : result;
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
                            falseMessage.AppendFormat("</br>��{0}�е���ʧ�ܣ�", i + 1);
                            error++;
                            continue;
                            //falseMessage.AppendFormat("</br>����ʧ�ܣ���");
                            //error = dt.Rows.Count;
                        }
                    }
                    count = dt.Rows.Count;
                    message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
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
                        message += "<br /><a href='../../Resource/temp/" + fileName+"' target='_blank'>���ظ���,�鿴����</a>";
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="projectItem">�����Ŀ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ���")]
        public ActionResult SaveForm(string keyValue, SafetyCheckEntity entity)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                bool result=saftycheckdatabll.SaveForm(keyValue, entity);
                if(result)
                {
                    //�����������˷��Ͷ���Ϣ����
                    Task.Run(()=> {
                        saftycheckdatabll.SendMessage(keyValue);
                    });

                }
                return Success("�����ɹ���");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ�鷢�����⼰�������")]
        public ActionResult SaveItemForm(string keyValue, CheckItemsEntity entity)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                saftycheckdatabll.SaveItemForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion
    }
}
