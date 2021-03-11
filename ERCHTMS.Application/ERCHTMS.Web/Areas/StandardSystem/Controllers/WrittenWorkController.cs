using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.StandardSystem;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using BSFramework.Data;
using System.Collections.Generic;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.IO;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.SafetyLawManage;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// �� �������湤������swp
    /// </summary>
    public class WrittenWorkController : MvcControllerBase
    {
        private WrittenWorkBLL writtenworkbll = new WrittenWorkBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private StoreLawBLL storelawbll = new StoreLawBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var strdept = new DataItemDetailBLL().GetItemValue(user.OrganizeId, "DeptSet");
            var entity = new DepartmentBLL().GetEntity(strdept);
            ViewBag.SpecialDept = entity != null ? entity.DeptCode : "";
            ViewBag.SpecialDeptId = entity != null ? entity.DepartmentId : "";
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
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// �ҵ��ղ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult myStoreIndex()
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

            var watch = CommonHelper.TimerStart();
            pagination.conditionJson = "1=1";

            #region ����Ȩ��
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //�鿴��Χ����Ȩ��
            /**
             * */
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            #endregion
            var data = writtenworkbll.GetPageDataTable(pagination, queryJson, authType);
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
            var data = writtenworkbll.GetList(queryJson);
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
            var data = writtenworkbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                var data = writtenworkbll.GetPageDataTable(pagination, queryJson, authType);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("filename"));
                excelTable.Columns.Add(new DataColumn("issuedept"));
                excelTable.Columns.Add(new DataColumn("filecode"));
                excelTable.Columns.Add(new DataColumn("publishdate"));
                excelTable.Columns.Add(new DataColumn("carrydate"));
                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    newDr["filename"] = item["filename"];
                    newDr["issuedept"] = item["issuedept"];
                    newDr["filecode"] = item["filecode"];
                    DateTime publishdate, carrydate;
                    DateTime.TryParse(item["publishdate"].ToString(), out publishdate);
                    DateTime.TryParse(item["carrydate"].ToString(), out carrydate);
                    newDr["publishdate"] = publishdate.ToString("yyyy-MM-dd");
                    newDr["carrydate"] = carrydate.ToString("yyyy-MM-dd");
                    excelTable.Rows.Add(newDr);
                }

                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "���湤�̳���SWP��Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                excelconfig.FileName = "���湤�̳���SWP��Ϣ����.xls";
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����������", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "�䷢����", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "�ļ����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publishdate", ExcelColumn = "��������", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʵʩ����", Width = 10 });
                //���õ�������
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
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
            writtenworkbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WrittenWorkEntity entity)
        {
            writtenworkbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportSWP(string belongtypecode)
        {
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
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                if (Directory.Exists(Server.MapPath("~/Resource/ht/images/swp")) == false)//��������ھʹ���file�ļ���
                {
                    Directory.CreateDirectory(Server.MapPath("~/Resource/ht/images/swp"));
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //�ļ�����
                    string filename = dt.Rows[i][0].ToString();
                    //�ļ����
                    string filecode = dt.Rows[i][1].ToString();
                    //�䷢����
                    string iuusedept = dt.Rows[i][2].ToString();
                    //��������
                    string publishdate = dt.Rows[i][3].ToString();
                    //ʵʩ����
                    string carrydate = dt.Rows[i][4].ToString();
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filecode) || string.IsNullOrEmpty(iuusedept) || string.IsNullOrEmpty(carrydate) || string.IsNullOrEmpty(publishdate))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    WrittenWorkEntity sl = new WrittenWorkEntity();
                    sl.FileName = filename;
                    sl.FileCode = filecode;
                    sl.IssueDept = iuusedept;
                    sl.BelongTypeCode = belongtypecode;//������λ
                    sl.FilesId = Guid.NewGuid().ToString();

                    FileInfoEntity fileEntity = new FileInfoEntity();
                    fileEntity.RecId = sl.FilesId;
                    fileEntity.EnabledMark = 1;
                    fileEntity.DeleteMark = 0;
                    fileEntity.FilePath = "~/Resource/ht/images/swp/" + filename;
                    fileEntity.FileName = sl.FileName;
                    fileEntity.FolderId = "ht/images";
                    try
                    {
                        sl.CarryDate = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                        sl.PublishDate = DateTime.Parse(DateTime.Parse(publishdate).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    try
                    {
                        writtenworkbll.SaveForm("", sl);
                        fileInfoBLL.SaveForm("", fileEntity);
                    }
                    catch
                    {
                        error++;
                    }
                }
                count = dt.Rows.Count - 1;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }
            return message;
        }
        #endregion

        #region �ҵ��ղ�
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetStoreListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid";
            pagination.p_fields = "a.lawid,b.CreateDate,FileName,IssueDept,FileCode,publishdate,CarryDate,FilesId";
            pagination.p_tablename = " bis_storelaw a left join hrs_writtenwork b on a.lawid=b.id";
            pagination.conditionJson = "userid='" + user.UserId + "' and ctype='5'";
            var data = storelawbll.GetPageDataTable(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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
        #endregion

        #region �����ҵ��ղ�
        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportMyStoreData(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = "FileName,IssueDept,FileCode,publishdate,CarryDate";
                pagination.p_tablename = " bis_storelaw a left join hrs_writtenwork b on a.lawid=b.id";
                pagination.conditionJson = "userid='" + user.UserId + "' and ctype='5'";
                pagination.sidx = "a.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                DataTable exportTable = storelawbll.GetPageDataTable(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "���湤������SWP�ҵ��ղ�";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "���湤������SWP�ҵ��ղص���.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����������", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "�䷢����", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "�ļ����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publishdate", ExcelColumn = "��������", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʵʩ����", Width = 10 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion
    }
}
