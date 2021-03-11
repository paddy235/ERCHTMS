using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.BaseManage;
using System.IO;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class SuppliesController : MvcControllerBase
    {
        private SuppliesBLL suppliesbll = new SuppliesBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private DistrictBLL districtbll = new DistrictBLL();

        #region ��ͼ����

        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TestIndex()
        {
            return View();
        }

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

            //ViewBag.Code = suppliesbll.GetMaxCode();
            UserBLL userbll = new UserBLL();
            ViewBag.User = userbll.GetEntity(OperatorProvider.Provider.Current().UserId);


            return View();
        }

        /// <summary>
        /// ѡ��Ӧ������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        #endregion

        #region ��ȡ����


        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {

            try
            {
                queryJson = queryJson ?? "";
                pagination.p_kid = "ID";
                pagination.p_fields = " SUPPLIESCODE,SUPPLIESTYPENAME,SUPPLIESNAME,NUM,SUPPLIESUNTILNAME,STORAGEPLACE,DEPARTNAME,USERNAME,createuserid,createuserdeptcode,createuserorgcode,WORKAREANAME,models,userid,departid,(select count(1) from mae_suppliescheckdetail where suppliesid=t.id) as checknum";
                pagination.p_tablename = "mae_supplies t";
                pagination.conditionJson = "1=1";
                pagination.sidx = "SUPPLIESCODE";
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

                var data = suppliesbll.GetPageList(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = suppliesbll.GetList(queryJson);
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
            var data = suppliesbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ����ids��ȡ��������
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMutipleDataJson(string Ids)
        {
            try
            {
                var data = suppliesbll.GetMutipleDataJson(Ids);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// ��ȡ���������
        /// </summary>
        /// <param name="DutyPerson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDutySupplies(string DutyPerson)
        {
            try
            {
                var data = suppliesbll.GetDutySuppliesDataJson(DutyPerson);
                return Success("��ȡ�ɹ�", data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
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
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                foreach (var item in keyValue.Split(','))
                {
                    if (suppliesbll.CheckRemove(item).Rows.Count > 0)
                    {
                        return Error("���������ڱ��������ã��޷�ɾ����");
                    }
                    suppliesbll.RemoveForm(item);
                }
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
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
        public ActionResult SaveForm(string keyValue, SuppliesEntity entity)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (string.IsNullOrWhiteSpace(entity.SUPPLIESCODE))
                {
                    entity.SUPPLIESCODE = suppliesbll.GetMaxCode();
                }
                suppliesbll.SaveForm(keyValue, entity);
                var entityInorOut = new InoroutrecordEntity
                {
                    USERID = user.UserId,
                    USERNAME = user.UserName,
                    DEPARTID = user.DeptId,
                    DEPARTNAME = user.DeptName,
                    INOROUTTIME = DateTime.Now,
                    SUPPLIESCODE = entity.SUPPLIESCODE,
                    SUPPLIESTYPE = entity.SUPPLIESTYPE,
                    SUPPLIESTYPENAME = entity.SUPPLIESTYPENAME,
                    SUPPLIESNAME = entity.SUPPLIESNAME,
                    SUPPLIESUNTIL = entity.SUPPLIESUNTIL,
                    SUPPLIESUNTILNAME = entity.SUPPLIESUNTILNAME,
                    NUM = entity.NUM,
                    STORAGEPLACE = entity.STORAGEPLACE,
                    MOBILE = entity.MOBILE,
                    SUPPLIESID = entity.ID,
                    STATUS = 0
                };
                var inoroutrecordbll = new InoroutrecordBLL();
                if (keyValue == "")
                    inoroutrecordbll.SaveForm("", entityInorOut);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="recid"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CopyForm(string keyValue, string recid)
        {
            try
            {
                IList<FileInfoEntity> filelist = fileinfobll.GetFileList(recid);

                IList<FileInfoEntity> filelist1 = fileinfobll.GetFileList(keyValue);
                foreach (var item in filelist1)
                {
                    fileinfobll.RemoveForm(item.FileId);
                }

                string dir = string.Format("~/Resource/{0}/{1}", "ht/images", DateTime.Now.ToString("yyyyMMdd"));
                foreach (var item in filelist)
                {
                    if (!Directory.Exists(Server.MapPath(dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath(dir));
                    }
                    if (System.IO.File.Exists(Server.MapPath(item.FilePath)))
                    {
                        string newFileName = Guid.NewGuid().ToString() + item.FileExtensions;
                        string newFilePath = dir + "/" + newFileName;
                        System.IO.File.Copy(Server.MapPath(item.FilePath), Server.MapPath(newFilePath));
                        item.FilePath = newFilePath;
                    }
                    item.RecId = keyValue;
                    item.FileId = Guid.NewGuid().ToString();
                    fileinfobll.SaveForm("", item);
                }
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "����Ӧ������")]
        public string ImportSuppliesData() {
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";

            string falseMessage = "";
            int count = HttpContext.Request.Files.Count; 
            if (count > 0)
            {
                count = 0;
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
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName), file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx") ? Aspose.Cells.FileFormatType.Excel2007Xlsx : Aspose.Cells.FileFormatType.Excel2003);
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

                //�жϱ�ͷ�Ƿ���ȷ,����ʹ�ô���ģ��
                var sheet = wb.Worksheets[0];
                if (sheet.Cells[2, 0].StringValue != "��������" || sheet.Cells[2, 1].StringValue != "��������" || sheet.Cells[2, 2].StringValue != "����ͺ�" || sheet.Cells[2, 3].StringValue != "����"
                   || sheet.Cells[2, 4].StringValue != "��λ" || sheet.Cells[2, 5].StringValue != "�������" || sheet.Cells[2, 6].StringValue != "��ŵص�" || sheet.Cells[2, 7].StringValue != "��Ҫ����")
                {
                    return message;
                }
                //List<SuppliesEntity> slist = new List<SuppliesEntity>();
                for (int i = 3; i <= sheet.Cells.MaxDataRow; i++)
                {
                    count++;
                    SuppliesEntity entity = new SuppliesEntity();
                    if (string.IsNullOrWhiteSpace(sheet.Cells[i, 1].StringValue))
                    {
                        falseMessage += "</br>" + "��" + (i + 1) + "���������Ʋ���Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(sheet.Cells[i, 3].StringValue))
                    {
                        falseMessage += "</br>" + "��" + (i + 1) + "����������Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    entity.SUPPLIESTYPENAME = sheet.Cells[i, 0].StringValue;
                    var item = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_SUPPLIESTYPE'").Where(x => x.ItemName == entity.SUPPLIESTYPENAME).ToList().FirstOrDefault();
                    if (item != null) {
                        entity.SUPPLIESTYPE = item.ItemValue;
                    }
                    entity.SUPPLIESNAME = sheet.Cells[i, 1].StringValue;
                    entity.Models = sheet.Cells[i, 2].StringValue;
                    entity.NUM = Convert.ToInt32(sheet.Cells[i, 3].StringValue);
                    entity.SUPPLIESUNTILNAME = sheet.Cells[i, 4].StringValue;
                    var itemUnit = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_SUPPLIESUNTIL'").Where(x => x.ItemName == entity.SUPPLIESUNTILNAME).ToList().FirstOrDefault();
                    if (itemUnit != null)
                    {
                        entity.SUPPLIESUNTIL = itemUnit.ItemValue;
                    }
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 5].StringValue))
                    {
                        DistrictEntity district = districtbll.GetDistrict(user.OrganizeId, sheet.Cells[i, 5].StringValue);
                        if (district == null)
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "��������ϵͳ������,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        else
                        {
                            entity.WorkAreaCode = district.DistrictCode;
                            entity.WorkAreaName = district.DistrictName;
                        }
                    }
                    entity.STORAGEPLACE = sheet.Cells[i, 6].StringValue;
                    entity.MAINFUN = sheet.Cells[i, 7].StringValue;
                    entity.CREATEDATE = DateTime.Now;
                    entity.DEPARTID = user.DeptId;
                    entity.DEPARTNAME = user.DeptName;
                    entity.USERID = user.UserId;
                    entity.USERNAME = user.UserName;
                    entity.SUPPLIESCODE = suppliesbll.GetMaxCode();
                    //entity.Create();
                    suppliesbll.SaveForm(entity.ID,entity);
                    var entityInorOut = new InoroutrecordEntity
                    {
                        USERID = entity.USERID,
                        USERNAME = entity.USERNAME,
                        DEPARTID = entity.DEPARTID,
                        DEPARTNAME = entity.DEPARTNAME,
                        INOROUTTIME = DateTime.Now,
                        SUPPLIESCODE = entity.SUPPLIESCODE,
                        SUPPLIESTYPE = entity.SUPPLIESTYPE,
                        SUPPLIESTYPENAME = entity.SUPPLIESTYPENAME,
                        SUPPLIESNAME = entity.SUPPLIESNAME,
                        SUPPLIESUNTIL = entity.SUPPLIESUNTIL,
                        SUPPLIESUNTILNAME = entity.SUPPLIESUNTILNAME,
                        NUM = entity.NUM,
                        STORAGEPLACE = entity.STORAGEPLACE,
                        MOBILE = entity.MOBILE,
                        SUPPLIESID = entity.ID,
                        STATUS = 0
                    };
                    var inoroutrecordbll = new InoroutrecordBLL();
                    inoroutrecordbll.SaveForm("", entityInorOut);

                    //slist.Add(entity);
                }
               
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "����";
                if (error > 0)
                {
                    message += "</br>" + falseMessage;
                }
            }
            return message;
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����Ӧ������")]
        public ActionResult ExportSuppliesList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = " SUPPLIESCODE,SUPPLIESTYPENAME,SUPPLIESNAME,NUM,SUPPLIESUNTILNAME,WORKAREANAME,STORAGEPLACE,DEPARTNAME,USERNAME";
            pagination.p_tablename = "V_mae_supplies t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";
            #region Ȩ��У��
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
            var data = suppliesbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "Ӧ������";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "Ӧ������.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESCODE".ToLower(), ExcelColumn = "���ʱ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESTYPENAME".ToLower(), ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESNAME".ToLower(), ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "NUM".ToLower(), ExcelColumn = "����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESUNTILNAME".ToLower(), ExcelColumn = "��λ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "WORKAREANAME".ToLower(), ExcelColumn = "�������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "STORAGEPLACE".ToLower(), ExcelColumn = "��ŵص�" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DEPARTNAME".ToLower(), ExcelColumn = "���β���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "USERNAME".ToLower(), ExcelColumn = "������" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����Ӧ������")]
        public ActionResult Export(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = " SUPPLIESCODE,SUPPLIESTYPENAME,SUPPLIESNAME,NUM,SUPPLIESUNTILNAME,STORAGEPLACE,DEPARTNAME,USERNAME";
            pagination.p_tablename = "V_mae_supplies t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";
            #region Ȩ��У��
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
            var data = suppliesbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "Ӧ������";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "Ӧ������.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESCODE".ToLower(), ExcelColumn = "���ʱ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESTYPENAME".ToLower(), ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESNAME".ToLower(), ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "NUM".ToLower(), ExcelColumn = "����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESUNTILNAME".ToLower(), ExcelColumn = "��λ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "STORAGEPLACE".ToLower(), ExcelColumn = "��ŵص�" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DEPARTNAME".ToLower(), ExcelColumn = "���β���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "USERNAME".ToLower(), ExcelColumn = "������" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
