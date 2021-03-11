using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using System.Web;
using System;
using BSFramework.Util.Offices;
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
using System.Text;
using System.Data.Common;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// �� ������ȫ�������ɷ���
    /// </summary>
    public class SafetyLawController : MvcControllerBase
    {
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //����������Ϣ
        private SafetyLawBLL safetylawbll = new SafetyLawBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
        private DataItemBLL dataItemBLL = new DataItemBLL();
        SpecialEquipmentBLL seb = new SpecialEquipmentBLL();
        private AreaBLL areaBLL = new AreaBLL();

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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LawForm()
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
        /// <summary>
        /// ѡ��ҳ��
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id as safeid";
            pagination.p_fields = "CreateDate,FileName,LawArea,IssueDept,FileCode,ValidVersions,CarryDate,releasedate,FilesId,effetstate,LawSource,createuserid,createuserdeptcode,createuserorgcode,updatedate,channeltype";
            pagination.p_tablename = " bis_safetylaw";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                //if (!string.IsNullOrEmpty(authType))
                //{
                //    switch (authType)
                //    {
                //        case "1":
                //            pagination.conditionJson += " and (createuserid='" + user.UserId + "' or createuserorgcode='00')";
                //            break;
                //        case "2":
                //            pagination.conditionJson += " and (createuserdeptcode='" + user.DeptCode + "' or createuserorgcode='00')";
                //            break;
                //        case "3":
                //            pagination.conditionJson += " and (createuserdeptcode like'" + user.DeptCode + "%' or createuserorgcode='00')";
                //            break;
                //        case "4":
                //            pagination.conditionJson += " and (createuserorgcode='" + user.OrganizeCode + "' or createuserorgcode='00')";
                //            break;

                //    }
                //}
                //else
                //{
                //    pagination.conditionJson += " and 0=1";
                //}
                IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                //�糧��ȡʡ��˾�Ļ���ID
                if (user.RoleName.Contains("ʡ���û�"))
                {
                    pagination.conditionJson += " and ( createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                }
                else
                {
                    orgcodelist = departmentBLL.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "ʡ��");
                    pagination.conditionJson += " and (";
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        pagination.conditionJson += "createuserorgcode ='" + item.EnCode + "' or ";
                    }
                    pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                }

            }
            var data = safetylawbll.GetPageDataTable(pagination, queryJson);
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
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safetylawbll.GetList(queryJson);
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
            var data = safetylawbll.GetEntity(keyValue);
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
                pagination.p_kid = "Id";
                pagination.p_fields = @"FileName,FileCode,to_char(ReleaseDate,'yyyy-MM-dd') as ReleaseDate,to_char(CarryDate,'yyyy-MM-dd') as CarryDate,IssueDept,case when Effetstate='1' then '������Ч'
                                            when Effetstate ='2' then '����ʵʩ'
                                            when Effetstate='3'  then '���޶�'
                                            when Effetstate='4'  then '��ֹ' end Effetstate";
                pagination.p_tablename = " bis_safetylaw";
                pagination.conditionJson = "1=1";
                pagination.sidx = "ReleaseDate";//�����ֶ� 
                pagination.sord = "desc";//����ʽ
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    //if (!string.IsNullOrEmpty(authType))
                    //{
                    //    switch (authType)
                    //    {
                    //        case "1":
                    //            pagination.conditionJson += " and (createuserid='" + user.UserId + "' or createuserorgcode='00')";
                    //            break;
                    //        case "2":
                    //            pagination.conditionJson += " and (createuserdeptcode='" + user.DeptCode + "' or createuserorgcode='00')";
                    //            break;
                    //        case "3":
                    //            pagination.conditionJson += " and (createuserdeptcode like'" + user.DeptCode + "%' or createuserorgcode='00')";
                    //            break;
                    //        case "4":
                    //            pagination.conditionJson += " and (createuserorgcode='" + user.OrganizeCode + "' or createuserorgcode='00')";
                    //            break;

                    //    }
                    //}
                    //else
                    //{
                    //    pagination.conditionJson += " and 0=1";
                    //}
                    IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                    //�糧��ȡʡ��˾�Ļ���ID
                    if (user.RoleName.Contains("ʡ���û�"))
                    {
                        pagination.conditionJson += " and ( createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                    }
                    else
                    {
                        orgcodelist = departmentBLL.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "ʡ��");
                        pagination.conditionJson += " and (";
                        foreach (DepartmentEntity item in orgcodelist)
                        {
                            pagination.conditionJson += "createuserorgcode ='" + item.EnCode + "' or ";
                        }
                        pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                    }
                }
                DataTable exportTable = safetylawbll.GetPageDataTable(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ȫ�������ɷ�����Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��ȫ�������ɷ�����Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "��������", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "�ĺ�/��׼��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "����ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʵʩ����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "��������", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "effetstate", ExcelColumn = "ʱЧ��", Width = 15 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
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
            safetylawbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyLawEntity entity)
        {
            entity.LawSource = "0";//�ڲ�����
            safetylawbll.SaveForm(keyValue, entity);
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
        public string ImportLaw(string lawtypecode)
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
                if (Directory.Exists(Server.MapPath("~/Resource/ht/images/channel")) == false)//��������ھʹ���file�ļ���
                {
                    Directory.CreateDirectory(Server.MapPath("~/Resource/ht/images/channel"));
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //�ļ�����
                    string filename = dt.Rows[i][0].ToString();
                    //����
                    string lawarea = dt.Rows[i][1].ToString();
                    //�ļ����
                    string filecode = dt.Rows[i][2].ToString();
                    //�䷢����
                    string iuusedept = dt.Rows[i][3].ToString();
                    //ʩ������
                    string carrydate = dt.Rows[i][4].ToString();
                    //��Ч�汾��
                    string validversions = dt.Rows[i][5].ToString();
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filecode) || string.IsNullOrEmpty(iuusedept) || string.IsNullOrEmpty(carrydate) || string.IsNullOrEmpty(validversions))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    SafetyLawEntity sl = new SafetyLawEntity();
                    sl.FileName = filename;
                    sl.LawArea = lawarea;
                    sl.FileCode = filecode;
                    sl.IssueDept = iuusedept;
                    sl.ValidVersions = validversions;
                    sl.LawTypeCode = lawtypecode;//����
                    sl.FilesId = Guid.NewGuid().ToString();
                    FileInfoEntity fileEntity = new FileInfoEntity();
                    fileEntity.RecId = sl.FilesId;
                    fileEntity.EnabledMark = 1;
                    fileEntity.DeleteMark = 0;
                    fileEntity.FilePath = "~/Resource/ht/images/channel/" + filename;
                    fileEntity.FileName = sl.FileName;
                    fileEntity.FolderId = "ht/images";
                    try
                    {
                        sl.CarryDate = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʱ������,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    try
                    {
                        safetylawbll.SaveForm("", sl);
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

        #region wordתPDF
        /// <summary>
        /// wordתPDF
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WordToPdf(string fileid)
        {
            try
            {
                DataTable fie = fileInfoBLL.GetFiles(fileid);
                if (fie != null && fie.Rows.Count > 0) {
                    string path = fie.Rows[0]["FilePath"].ToString();
                    string[] str = path.Split('/');
                    if (str[str.Length - 1].EndsWith(".pdf"))
                    {
                        return ToJsonResult(path);
                    }
                    else {
                        path = Server.MapPath(path);
                        string str1 = "~/Resource/Temp/" + str[str.Length - 1].Replace("docx", "pdf").Replace("doc", "pdf");
                        string savePath = Server.MapPath(str1);
                        if (!System.IO.File.Exists(savePath))
                        {
                            System.IO.File.Copy(path, savePath);
                            Aspose.Words.Document doc = new Aspose.Words.Document(savePath);
                            doc.Save(savePath, Aspose.Words.SaveFormat.Pdf);
                        }
                        return ToJsonResult(str1);
                    }
                    
                }
                return ToJsonResult(0);
                
            }
            catch (Exception ex)
            {
                return ToJsonResult(0);
            }
        }
        #endregion


        #region wordתPDF
        /// <summary>
        /// wordתPDF
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FileToPdf(string url)  
        {
            try
            {
                string path = Server.MapPath(url);
                if (System.IO.File.Exists(path))
                {
                    string str1 = url.Replace("docx", "pdf").Replace("doc", "pdf");
                    string savePath = Server.MapPath(str1);
                    if (!System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Copy(path, savePath);
                        Aspose.Words.Document doc = new Aspose.Words.Document(savePath);
                        doc.Save(savePath, Aspose.Words.SaveFormat.Pdf);
                    }
                    return ToJsonResult(str1);
                }
                else { return ToJsonResult(0); }
            }
            catch (Exception)
            {
                return ToJsonResult(0);
            }
        }
        #endregion

        #region ���ջ���ƽ̨������������Ԥ����ַ
        /// <summary>
        /// wordתPDF
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFileUrl(string keyValue)
        {
            try
            {
                if (dataItemDetailBLL.GetItemValue("flag") == "1")
                {
                    DataTable fie = fileInfoBLL.GetFiles(keyValue);
                    if (fie != null && fie.Rows.Count > 0)
                    {
                        string path = fie.Rows[0]["FilePath"].ToString();
                        string[] str = path.Split('/');
                        if (str[str.Length - 1].EndsWith(".pdf"))
                        {
                            return ToJsonResult(path);
                        }
                        else
                        {
                            path = Server.MapPath(path);
                            string str1 = "~/Resource/Temp/" + str[str.Length - 1].Replace("docx", "pdf").Replace("doc", "pdf");
                            string savePath = Server.MapPath(str1);
                            if (!System.IO.File.Exists(savePath))
                            {
                                System.IO.File.Copy(path, savePath);
                                Aspose.Words.Document doc = new Aspose.Words.Document(savePath);
                                doc.Save(savePath, Aspose.Words.SaveFormat.Pdf);
                            }
                            return ToJsonResult(str1);
                        }

                    }
                    return ToJsonResult(0);
                }
                else {
                    string fid = string.Empty;
                    if (keyValue.Contains('.'))
                    {
                        fid = keyValue;
                    }
                    else
                    {
                        //���ݷ���ID��ȡFID(����׺)
                        string sql = string.Format(@"select t.var_fid from ex_attachment t where t.id 
in(select attachment_id from ex_law_attachment t where t.law_id='{0}' and t.law_type='01')", keyValue);
                        DataTable dt = seb.SelectData(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            fid = dt.Rows[0][0].ToString();
                        }
                        else
                        {
                            return ToJsonResult(0);
                        }
                    }
                    var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
                    long expiresLong = DateTime.Now.Ticks / 1000 + 180;
                    string expires = expiresLong.ToString();
                    string token = Md5Helper.MD5(di.GetItemValue("antiStealLink.key", "Resource") + fid + expires, 32);
                    string url = string.Format(di.GetItemValue("LawWebUrl", "Resource"), di.GetItemValue("appId", "Resource"), fid, expires, token);
                    return ToJsonResult(url);
                }
                

            }
            catch (Exception)
            {
                return ToJsonResult(0);
            }
        }
        #endregion
        

        /// <summary>
        /// ��ȡ�����ֵ��б�������������򣨰󶨿ؼ���
        /// </summary>
        /// <param name="EnCode">����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetDataItemListSortJson(string EnCode)
        {
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'" + EnCode + "'").Where(a=>a.ItemValue!="0").OrderBy(it => it.SortCode);
            return Content(data.ToJson());
        }


        private string GetDataItemByValue(string itemcode,string itemvalue) {
            try
            {
                string sql = string.Format("select itemname from  base_dataitemdetail  where itemid =(select  itemid from base_dataitem where itemcode='{0}') and itemvalue='{1}'", itemcode, itemvalue);
                DataTable dt = seb.SelectData(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
            
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetiDeptListJson(Pagination pagination,string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            pagination.p_kid = "ITEMDETAILID";
            pagination.p_fields = "ItemName,ItemValue";
            pagination.p_tablename = " BASE_DATAITEMDETAIL t";
            pagination.conditionJson = string.Format(" t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}') ", queryParam["itemCode"]);

            //��ѯ����
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.ItemName like '%{0}%'", queryParam["keyword"]);
            }
            
            var data = htbaseinfobll.GetBaseInfoForApp(pagination);
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
        /// <summary>
        /// ���ݷ�������code��ȡ�б�
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetiDeptJson(string UserIDs)
        {
            var strSql = new StringBuilder();
            string sql = string.Join(",", UserIDs.Split(',')).Replace(",", "','");
            sql = string.Format(@"SELECT ItemName,ItemValue from  BASE_DATAITEMDETAIL t where ItemValue in('{0}') and t.itemid=(select itemid from base_dataitem a where a.itemcode='LawDept')", sql);
            DataTable dt = seb.SelectData(sql);
            return Content(dt.ToJson());
        }

        #region ��ȡ�ļ����ص�ַ
        /// <summary>
        /// wordתPDF
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFileDowUrl(string keyValue,string type)
        {
            try
            {
                if (dataItemDetailBLL.GetItemValue("flag") == "1")
                {
                    type = "0";
                }
                if (type == "0")
                {
                    DataTable fie = fileInfoBLL.GetFiles(keyValue);
                    if (fie != null && fie.Rows.Count > 0)
                    {
                        string path = fie.Rows[0]["FileName"].ToString();
                        return ToJsonResult(path);
                    }
                    return ToJsonResult(0);
                }
                else {
                    string fid = string.Empty;
                    if (keyValue.Contains('.'))
                    {
                        fid = keyValue;
                    }
                    else {
                        
                        //fast�ļ�
                        //���ݷ���ID��ȡFID(����׺)
                        string sql = string.Format(@"select t.var_fid from ex_attachment t where t.id 
in(select attachment_id from ex_law_attachment t where t.law_id='{0}' and t.law_type='01')", keyValue);
                        DataTable dt = seb.SelectData(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            fid = dt.Rows[0][0].ToString();
                        }
                        else
                        {
                            return ToJsonResult(0);
                        }
                    }
                    var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
                    long expiresLong = DateTime.Now.Ticks / 1000 + 180;
                    string expires = expiresLong.ToString();
                    string token = Md5Helper.MD5(di.GetItemValue("userKey", "Resource") + expires, 32);
                    string url = string.Format(di.GetItemValue("FileDowUrl", "Resource"), fid, di.GetItemValue("user", "Resource"), expires, token);
                    return ToJsonResult(url);
                    
                }

            }
            catch (Exception)
            {
                return ToJsonResult(0);
            }
        }
        #endregion
        
    }
}
