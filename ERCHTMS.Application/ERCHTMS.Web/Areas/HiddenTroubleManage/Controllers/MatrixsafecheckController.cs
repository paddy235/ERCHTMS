using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using System.Web;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using Aspose.Words;
using System.IO;
using System.Drawing;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// �� ��������ȫ���ƻ�
    /// </summary>
    public class MatrixsafecheckController : MvcControllerBase
    {
        private MatrixsafecheckBLL matrixsafecheckbll = new MatrixsafecheckBLL();
        private MatrixcontentBLL matrixcontentbll = new MatrixcontentBLL();
        private MatrixdeptBLL matrixdeptbll = new MatrixdeptBLL();

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

        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetForm()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeptForm()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContentForm()
        {
            return View();
        }

        /// <summary>
        /// ����ҳ�� 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ActionForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CheckForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Matiximport()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡȨ��
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public string GetDataAuthority()
        {
            AuthorizeBLL authBLL = new AuthorizeBLL();
            string result = authBLL.GetDataAuthority(OperatorProvider.Provider.Current(), Request.Cookies["currentmoduleId"].Value, "");
            return result;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCanlendarListJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetCanlendarListJson(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = matrixsafecheckbll.GetPageListJson(pagination, queryJson);
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
        /// ��ȡ���е���ݼ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetYearList()
        {
            var data = matrixsafecheckbll.GetInfoBySql("select to_char(CHECKTIME,'yyyy') yealname from bis_matrixsafecheck group by to_char(CHECKTIME,'yyyy') order by to_char(CHECKTIME,'yyyy')  ");
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetList(queryJson);
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
            var data = matrixsafecheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ���¾������id 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult SetFormJson(string keyValue,string recid)
        {
            var data = matrixsafecheckbll.SetFormJson(keyValue, recid);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetContentPageJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetContentPageJson(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��鲿��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeptPageJson(string queryJson)
        {
            var data = matrixsafecheckbll.GetDeptPageJson(queryJson);
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
            matrixsafecheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, MatrixsafecheckEntity entity)
        {
            entity.ISOVER = 0;
            
            DataTable dt = new DataTable();
            if (keyValue == "" || keyValue == null)
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  = to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  ");
            }
            else
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  =  to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  and ID <> '" + keyValue + "' ");
            }
            if (dt.Rows.Count == 0)
            {
                matrixsafecheckbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            else
            {
                return Error("�Ѵ�����ͬʱ������ݡ�");

            }
            

            
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
        public ActionResult SubmitForm(string keyValue, MatrixsafecheckEntity entity)
        {
            entity.ISOVER = 1;
            
            DataTable dt = new DataTable();
            if (keyValue == "" || keyValue == null)
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  = to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  ");
            }
            else
            {
                dt = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  =  to_date('" + entity.CHECKTIME + "', 'yyyy-MM-dd HH24:mi:ss')  and ID <> '" + keyValue + "' ");
            }
            if (dt.Rows.Count == 0)
            {
                matrixsafecheckbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            else
            {
                return Error("�Ѵ�����ͬʱ������ݡ�");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSetForm(MatrixEntity entity)
        {
            var user = OperatorProvider.Provider.Current();
            string isrole = "1";
            if (user.RoleName.Contains("����"))
            {
                isrole = "0";
            }

            if (!string.IsNullOrEmpty(entity.arrcontent))
            {
                var arrcontentlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MatrixcontentEntity>>(entity.arrcontent);
                foreach (MatrixcontentEntity arrcontentinfo in arrcontentlist)
                {
                    MatrixcontentEntity en = matrixcontentbll.GetEntity(arrcontentinfo.ID);
                    if (en != null) //ִ��update
                    {
                        en.CONTENT = arrcontentinfo.CONTENT;
                        en.CODE = arrcontentinfo.CODE;
                        en.ISROLE = isrole;
                        matrixcontentbll.SaveForm(en.ID, en);
                    }
                    else // ����
                    {
                        arrcontentinfo.ISROLE = isrole;
                        matrixcontentbll.SaveForm("", arrcontentinfo);
                    }
                }
               
            }

            if (!string.IsNullOrEmpty(entity.arrdept))
            {
                var arrdeptlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MatrixdeptEntity>>(entity.arrdept);
                foreach (MatrixdeptEntity arrcondeptinfo in arrdeptlist)
                {
                    MatrixdeptEntity en = matrixdeptbll.GetEntity(arrcondeptinfo.ID);
                    if (en != null) //ִ��update
                    {
                        en.DEPTNAME = arrcondeptinfo.DEPTNAME;
                        en.DEPT = arrcondeptinfo.DEPT;
                        en.DEPTCODE = arrcondeptinfo.DEPTCODE;
                        en.CODE = arrcondeptinfo.CODE;
                        en.ISROLE = isrole;
                        matrixdeptbll.SaveForm(en.ID, en);
                    }
                    else // ����
                    {
                        arrcondeptinfo.ISROLE = isrole;
                        matrixdeptbll.SaveForm("", arrcondeptinfo);
                    }
                }
            }

            if (!string.IsNullOrEmpty(entity.delcontent))
            {
                var arr = entity.delcontent.Split(',');

                foreach (string ain in arr)
                {
                    matrixcontentbll.RemoveForm(ain);
                }
            }

            if (!string.IsNullOrEmpty(entity.deldept))
            {
                var arr = entity.deldept.Split(',');

                foreach (string ain in arr)
                {
                    matrixdeptbll.RemoveForm(ain);
                }
            }

            return Success("�����ɹ���");
        }


        /// <summary>
        /// ����j������
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportQuestion()
        {
            var user = OperatorProvider.Provider.Current();
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޴˲���Ȩ��";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
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

                string where = "";
                if (user.RoleName.Contains("����"))
                {
                    where += "  a.ISROLE = '0' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
                else
                {
                    where += "  a.ISROLE = '1' and a.CREATEUSERDEPTCODE ='" + user.DeptCode + "'";
                }
                DataTable dtcontent = matrixsafecheckbll.GetInfoBySql("select ID, CODE,CONTENT from BIS_MATRIXCONTENT a where   " + where);
                if (dtcontent.Rows.Count == 0)
                {
                    return "�������ü������";
                }
                DataTable dtdept = matrixsafecheckbll.GetInfoBySql("select ID, code,dept,deptname,deptcode from BIS_MATRIXDEPT a where   " + where);
                if (dtdept.Rows.Count == 0)
                {
                    return "�������ü�鲿��";
                }
                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, false);
                dt.Rows.RemoveAt(0);
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    //�������
                    string checktime = dt.Rows[i][0].ToString();
                    //�������
                    string checkcontent = dt.Rows[i][1].ToString();
                    //��鲿��
                    string checkdept = dt.Rows[i][2].ToString();
                    //�����Ա
                    string checkuser = dt.Rows[i][3].ToString();

                    //---****ֵ���ڿ���֤*****--

                    if (string.IsNullOrEmpty(checktime) && string.IsNullOrEmpty(checkcontent) && string.IsNullOrEmpty(checkdept) && string.IsNullOrEmpty(checkuser))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(checktime) || string.IsNullOrEmpty(checkcontent) || string.IsNullOrEmpty(checkdept))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    

                    MatrixsafecheckEntity entir = new MatrixsafecheckEntity();
                    entir.ISOVER = 1;


                    // ���ʱ��
                    try
                    {
                        if (!string.IsNullOrEmpty(checktime))
                        {
                            // ���ʱ��
                            DataTable dtcount = matrixsafecheckbll.GetInfoBySql("select id from bis_matrixsafecheck where checktime  = to_date('" + DateTime.Parse(checktime).ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd HH24:mi:ss')  ");

                            if (dtcount.Rows.Count > 0)
                            {
                                falseMessage += "</br>" + "��" + (i + 2) + "�м��ʱ���Ѵ���,δ�ܵ���.";
                                error++;
                                continue;
                            }
                            else
                            {
                                entir.CHECKTIME = DateTime.Parse(DateTime.Parse(checktime).ToString("yyyy-MM-dd"));
                            }

                            
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�м��ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    // �������
                    entir.CONTENT = checkcontent;
                    if (!string.IsNullOrEmpty(checkcontent))
                    {
                        var conarr = checkcontent.Split(',');
                        int contentresult = 0;
                        
                        foreach (string continfo in conarr)
                        {
                            foreach (DataRow condr in dtcontent.Rows)
                            {
                                if (continfo == condr["CONTENT"].ToString())
                                {
                                    if (entir.CONTENTID == "" || entir.CONTENTID == null)
                                    {
                                        
                                        entir.CONTENTID = condr["ID"].ToString();
                                        entir.CONTENTNUM = condr["CODE"].ToString();
                                    }
                                    else
                                    {
                                        entir.CONTENTID += "," + condr["ID"].ToString();
                                        entir.CONTENTNUM += "," + condr["CODE"].ToString();
                                    }
                                    contentresult++;
                                    break;
                                    
                                }
                            }
                        }
                        if (contentresult != conarr.Length)
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "�м����������,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }

                    // ��鲿��
                    entir.CHECKDEPTNAME = checkdept;
                    if (!string.IsNullOrEmpty(checkdept))
                    {
                        var deptarr = checkdept.Split(',');
                        int checkdeptresult = 0;
                        
                        foreach (string deptinfo in deptarr)
                        {
                            foreach (DataRow deptdr in dtdept.Rows)
                            {
                                if (deptinfo == deptdr["DEPTNAME"].ToString())
                                {
                                    if (entir.CHECKDEPTSEL == "" || entir.CHECKDEPTSEL == null)
                                    {
                                        
                                        entir.CHECKDEPTSEL = deptdr["ID"].ToString();
                                        entir.CHECKDEPT = deptdr["DEPT"].ToString();
                                        entir.CHECKDEPTCODE = deptdr["DEPTCODE"].ToString();
                                        entir.CHECKDEPTNUM = deptdr["CODE"].ToString();

                                    }
                                    else
                                    {
                                        entir.CHECKDEPTSEL += "," + deptdr["ID"].ToString();
                                        entir.CHECKDEPT += "," + deptdr["DEPT"].ToString();
                                        entir.CHECKDEPTCODE += "," + deptdr["DEPTCODE"].ToString();
                                        entir.CHECKDEPTNUM += "," + deptdr["CODE"].ToString();

                                    }
                                    checkdeptresult++;
                                    break;
                                    
                                }
                            }
                        }
                        if (checkdeptresult != deptarr.Length)
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "�м�鲿������,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }

                    // �����Ա
                    entir.CHECKUSERNAME = checkuser;
                    if (!string.IsNullOrEmpty(checkuser))
                    {
                        var userarr = checkuser.Split(',');
                        int checkuserresult = 0;
                        foreach (string userinfo in userarr)
                        {
                            DataTable userdt = matrixsafecheckbll.GetInfoBySql(" SELECT USERID,ACCOUNT,DEPARTMENTCODE,REALNAME FROM BASE_USER WHERE REALNAME = '"+ userinfo + "' ");

                            if (userdt.Rows.Count == 0)
                            {
                                break;
                            }
                            else
                            {
                                if (entir.CHECKUSER == "" || entir.CHECKUSER == null)
                                {

                                    entir.CHECKUSER = userdt.Rows[0]["USERID"].ToString();
                                    entir.CHECKUSERCODE = userdt.Rows[0]["ACCOUNT"].ToString();
                                    entir.CHECKUSERDEPT = userdt.Rows[0]["DEPARTMENTCODE"].ToString();

                                }
                                else
                                {
                                    entir.CHECKUSER += "," + userdt.Rows[0]["USERID"].ToString();
                                    entir.CHECKUSERCODE += "," + userdt.Rows[0]["ACCOUNT"].ToString();
                                    entir.CHECKUSERDEPT = userdt.Rows[0]["DEPARTMENTCODE"].ToString();

                                }
                                checkuserresult++;
                            }

                        }


                        if (checkuserresult != userarr.Length)
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "�м����Ա����,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }

                    try
                    {
                        entir.ID = Guid.NewGuid().ToString();
                        matrixsafecheckbll.SaveForm(entir.ID, entir);
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�б���ʧ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                }


                count = dt.Rows.Count - 1;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }
            return message;
        }

        public ActionResult SavePic(string qystr)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string guid = Guid.NewGuid().ToString();
            try
            {
                //
                var fileurl = string.Empty;
                DataItemDetailBLL dd = new DataItemDetailBLL();
                string path = Server.MapPath("~") + "\\Resource\\" + "Temp";
                byte[] arr2 = Convert.FromBase64String(qystr);
                using (MemoryStream ms2 = new MemoryStream(arr2))
                {//��png��ʽͼƬ�洢Ϊjpg��ʽ�ļ�
                    System.Drawing.Bitmap bmp2 = new System.Drawing.Bitmap(ms2);
                    //bmp2 = KiResizelmage(bmp2, 600, 450);
                    string fileName = Guid.NewGuid().ToString() + ".png";
                    bmp2.Save(path + "\\" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fileurl = "\\Resource\\" + "Temp" + "\\" + fileName;
                }
                //img = Server.MapPath("~" + fileurl);
                matrixsafecheckbll.ExecuteBySql("insert into bis_matrixsafecheck_file(id, contentbase) values ('" + guid + "', '" + fileurl + "')");

            }
            catch (Exception ex)
            {

               
            }


            

            return ToJsonResult(guid);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="keyvalue"></param>
        public void ExportAuditTotal(string qystr)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/matrixcheck/����ʽ��ȫ����.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToBookmark("matrixhtml");

            DataTable dt = matrixsafecheckbll.GetInfoBySql("select contentbase from bis_matrixsafecheck_file where id = '" + qystr + "'");
            string basere = "";
            if (dt.Rows.Count > 0)
            {
                basere = dt.Rows[0]["CONTENTBASE"].ToString();


            }

            string html = "<img width='983' height='552' src='"+ Server.MapPath("~" + basere) + "' />";
            builder.InsertHtml(html);
            //Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            //db.MoveToMergeField("gzpic");
            
            //byte[] bytes = Convert.FromBase64String(cs);

            //db.InsertImage(@"D:\cs.png",700,500);

            doc.Save(resp, Server.UrlEncode("����ʽ��ȫ����" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        #endregion
    }
}


