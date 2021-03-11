using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Web;
using ERCHTMS.Busines.EquipmentManage;
using System.IO;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class ConferenceController : MvcControllerBase
    {
        private ConferenceBLL conferencebll = new ConferenceBLL();
        private ConferenceUserBLL conferenceuserbll = new ConferenceUserBLL();
        private SpecialEquipmentBLL specialequipmentbll = new SpecialEquipmentBLL();

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
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,ConferenceName,Compere,COMPEREID,CompereDept,ConferenceTime,Locale,ConferencePerson,IsSend,UserId";
            pagination.p_tablename = "BIS_Conference t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string sqlWhere = "";
                string deptCode = "";
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    deptCode= queryParam["code"].ToString();
                }

                if (role.Contains("��˾���û�") || role.Contains("���������û�"))
                {
                    if (!string.IsNullOrEmpty(deptCode)&&deptCode!=user.DeptCode)
                    {
                        pagination.conditionJson = string.Format(" ((CREATEUSERORGCODE = '{0}' and CREATEUSERDEPTCODE like '{1}%' and IsSend='0'))", user.OrganizeCode, deptCode, user.UserId);
                    }
                    else {
                        pagination.conditionJson = string.Format(" ((CREATEUSERORGCODE = '{0}' and IsSend='0') or  createuserid='{1}')", user.OrganizeCode,user.UserId);
                    }
                    
                }
                else {
                    if (!queryParam["isOrg"].IsEmpty()&&queryParam["isOrg"].ToString() == "Organize")
                    {
                        pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}' or (CREATEUSERDEPTCODE like '{1}%' and IsSend='0')) ", user.UserId, user.DeptCode);
                    }
                    else {
                        if (!string.IsNullOrEmpty(deptCode))
                        {
                            if (deptCode == user.DeptCode)
                            {
                                pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}' or (CREATEUSERDEPTCODE like '{1}%' and IsSend='0')) ", user.UserId, deptCode);
                            }
                            else
                            {
                                pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}') and CREATEUSERDEPTCODE like '{1}%'", user.UserId, deptCode);
                            }
                        }
                        else
                        {
                            pagination.conditionJson = string.Format("((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}' or (CREATEUSERDEPTCODE like '{1}%' and IsSend='0')) ", user.UserId, user.DeptCode);
                        }
                    }
                }
            }

            var watch = CommonHelper.TimerStart();
            var data = conferencebll.GetPageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = conferencebll.GetList(queryJson);
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
            var data = conferencebll.GetEntity(keyValue);
            return ToJsonResult(data);
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
        public ActionResult RemoveForm(string keyValue)
        {
            conferencebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, ConferenceEntity entity)
        {
            conferencebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ȫ��������")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "ConferenceName,Locale,conferencetime,comperedept,compere,conferenceperson";
            pagination.p_tablename = "BIS_Conference t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson = where + string.Format(" and ((instr(userid,'{0}')>0 and IsSend='0') or createuserid='{0}') ", user.UserId);
            }

            var watch = CommonHelper.TimerStart();
            var data = conferencebll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "��ȫ����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "��ȫ����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "conferencename", ExcelColumn = "����ص�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "locale", ExcelColumn = "�ص�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "conferencetime", ExcelColumn = "����ʱ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "comperedept", ExcelColumn = "�ٿ�����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "compere", ExcelColumn = "������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "conferenceperson", ExcelColumn = "����Ӧ������", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }

        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ȫ����ǩ��������")]
        public ActionResult ExportQD(string keyValue)
        {
            //��ȡ���������Ϣ
            ConferenceEntity cr = conferencebll.GetEntity(keyValue);
            ////��ȡ����ǩ����Ա��Ϣ
            //List<Object> data = conferenceuserbll.GetSignTable(keyValue);
            ////ǩ��
            //List<Object> dataQD = data[0] as List<Object>;
            ////δǩ��
            //List<Object> dataWQD = data[1] as List<Object>;
            ////���
            //List<Object> dataQ = data[2] as List<Object>;

            string path = Request.PhysicalApplicationPath;
            int index = 0;
            string fileName = HttpUtility.UrlDecode("��ȫ����ǩ��") + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";  //���嵼���ļ�����һ���ǵñ���
            try
            {
                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook();
                wk.Open(Server.MapPath("~/Resource/ExcelTemplate/��ȫ����ǩ����ģ��.xlsx")); //����ģ��
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Style style = wk.Styles[wk.Styles.Add()];
                style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;//���־���
                style.IsTextWrapped = true;//�Զ�����
                style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                string sql = string.Format(" select t.USERNAME,o.DEPTNAME,t.PHOTOURL,t.issign,t.REVIEWSTATE from BIS_ConferenceUser t left join v_userinfo o on t.userid=o.userid where t.ConferenceID='{0}'", keyValue);
                DataTable dt = specialequipmentbll.SelectData(sql);
                //�λ���Ա����
                var listC = dt;
                //ǩ����Ա����
                var SdUser = listC.Select(" issign='0'");
                //δǩ����Ա����
                var YdUser = listC.Select(" issign<>'0' and REVIEWSTATE<>'2'");
                //�������
                var QUser = listC.Select(" REVIEWSTATE='2'");
                sheet.Cells[1, 2].PutValue(cr.ConferenceName);//��������
                sheet.Cells[1, 4].PutValue(cr.Locale);//����ص�
                sheet.Cells[2, 2].PutValue(cr.CompereDept);//�ٿ�����   
                sheet.Cells[2, 4].PutValue(cr.Compere);//������
                sheet.Cells[3, 2].PutValue(cr.Content);//��Ҫ����
                if (SdUser.Length == 0)
                {
                    sheet.Cells[5, 1].PutValue("��");
                }
                else
                {
                    sheet.Cells[5 + SdUser.Length, 0].PutValue("��ǩ������");
                    sheet.Cells.Merge(5, 0, SdUser.Length, 1);
                    foreach (DataRow item in SdUser)
                    {
                        index++;
                        sheet.Cells[4 + index, 1].PutValue(index);
                        sheet.Cells[4 + index, 2].PutValue(item[0].ToString());//�λ���Ա
                        sheet.Cells[4 + index, 4].PutValue(item[1].ToString());//��������
                        if (!string.IsNullOrEmpty(item[2].ToString())) {
                            string[] u = item[2].ToString().Split('/');
                            string url = path + "/" + u[1] + "/" + u[2] + "/" + u[3] + "/" + u[4] + "/" + u[5];//ǩ��
                            FileStream fs = new FileStream(url, FileMode.Open);
                            int imgIndex = sheet.Pictures.Add(4 + index, 3, fs, 120, 30);
                            fs.Close();
                            sheet.Cells.SetColumnWidth(5, 20);
                            sheet.Cells.SetRowHeight(4 + index, 44);
                            sheet.Pictures[imgIndex].Width = 139;
                            sheet.Pictures[imgIndex].Height = 50;
                            sheet.Pictures[imgIndex].Top = 3;
                            sheet.Pictures[imgIndex].Left = 6;
                        }
                    }

                }
                int num = SdUser.Length == 0 ? 1 : SdUser.Length;
                int s = SdUser.Length == 0 ? 1 : SdUser.Length;
                if (index > 0)
                {
                    s = 0;
                }

                if (YdUser.Length == 0)
                {
                    sheet.Cells[5 + num, 0].PutValue("δǩ������");
                    sheet.Cells[5 + num, 1].PutValue("��");
                }
                else
                {
                    sheet.Cells[5 + num, 0].PutValue("δǩ������");
                    sheet.Cells.Merge(5 + num, 0, YdUser.Length, 1);
                    foreach (DataRow item in YdUser)
                    {
                        index++;
                        sheet.Cells[4 + s + index, 1].PutValue(index);
                        sheet.Cells[4 + s + index, 2].PutValue(item[0].ToString());
                        sheet.Cells[4 + s + index, 4].PutValue(item[1].ToString());
                        sheet.Cells[4 + s + index, 3].PutValue("");

                    }

                }
                s = YdUser.Length == 0 ? 1 : YdUser.Length;
                int qj = s + SdUser.Length == 0 ? 1 : s + SdUser.Length;
                qj = SdUser.Length == 0 ? qj + 1 : qj;
                int qjNum = 0;
                if (QUser.Length > 0)
                {
                    sheet.Cells[5 + qj, 0].PutValue("�������");
                    sheet.Cells.Merge(5 + qj, 0, QUser.Length, 1);
                    foreach (DataRow dr in QUser)
                    {
                        sheet.Cells[5 + qj + qjNum, 1].PutValue(index++ + 1);
                        sheet.Cells[5 + qj + qjNum, 2].PutValue(dr[0].ToString());
                        sheet.Cells[5 + qj + qjNum, 4].PutValue(dr[1].ToString());
                        sheet.Cells[5 + qj + qjNum, 3].PutValue("");
                        qjNum++;
                    }
                }
                else
                {
                    qjNum += 1;
                    sheet.Cells[5 + qj, 0].PutValue("�������");
                    sheet.Cells[5 + qj, 1].PutValue("��");
                }
                Aspose.Cells.Range r = sheet.Cells.CreateRange(5, 0, qj + qjNum, 5);
                r.Style = style;
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                wk.Save(HttpUtility.UrlEncode(fileName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInExcel, resp);

                return ToJsonResult("0");
            }
            catch (Exception ex)
            {
                string env = Config.GetValue("Environment");
                string msg = env == "dev" ? ex.Message : "�Բ���,ϵͳ������";
                return ToJsonResult("" + msg + "");
            }
            
        }
        #endregion
    }
}
