using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System.IO;
using ERCHTMS.Code;
using System;
using System.Data;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Text.RegularExpressions;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using System.Linq;
using System.Collections.Generic;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using ERCHTMS.Busines.SystemManage;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� �������������Ա��
    /// </summary>
    public class AptitudeinvestigatepeopleController : MvcControllerBase
    {
        private PostCache postCache = new PostCache();
        private AptitudeinvestigatepeopleBLL aptitudeinvestigatepeoplebll = new AptitudeinvestigatepeopleBLL();
        private PostBLL postBLL = new PostBLL();
        private DataItemCache dic = new DataItemCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL userBLL = new UserBLL();
        private HistoryPeopleBLL hispeoplebll = new HistoryPeopleBLL();
        private BlackSetBLL scoresetbll = new BlackSetBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private CertificateinspectorsBLL certificateinspectorsbll = new CertificateinspectorsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

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
            var data = aptitudeinvestigatepeoplebll.GetList(queryJson);
            return ToJsonResult(data);
        }
         [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson) {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.ID tid";
                pagination.p_fields = @"p.outsourcingname,
                                       e.engineername,
                                       t.realname,t.workyear,
                                       t.identifyid,t.stateofhealth,
                                       t.gender,t.createuserid,
                                       t.mobile,t.degreesid,
                                       t.dutyname,t.isoverage,t.age";
                pagination.p_tablename = @"epg_aptitudeinvestigatepeople t
                                      left Join epg_outsouringengineer e on e.id = t.outengineerid
                                      left join epg_outsourcingproject p on p.outprojectid = t.outprojectid";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                pagination.conditionJson = "1=1";
                var data = aptitudeinvestigatepeoplebll.GetPageList(pagination, queryJson);

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
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

        /// <summary>
        /// ��ȡ������Ա����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOverAgePeopleCount(string keyValue)
        {
            try
            {
                int count = aptitudeinvestigatepeoplebll.GetList("").Where(t => t.PEOPLEREVIEWID == keyValue && t.IsOverAge == "1").Count();
                return Success("��ȡ���ݳɹ�", count);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// ��ȡ��Ա��ʷ��¼��ҳ��ʾ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
         [HttpGet]
         public ActionResult GetHistoryPageListJson(Pagination pagination, string queryJson) {
             try
             {
                 var watch = CommonHelper.TimerStart();
                 pagination.p_kid = "t.ID tid";
                 pagination.p_fields = @"p.outsourcingname,
                                       e.engineername,t.workyear,
                                       t.realname,t.stateofhealth,
                                       t.identifyid,t.degreesid,
                                       t.gender,
                                       t.mobile,
                                       t.dutyname";
                 pagination.p_tablename = @"epg_historypeople t
                                      left Join epg_outsouringengineer e on e.id = t.outengineerid
                                      left join epg_outsourcingproject p on p.outprojectid = t.outprojectid";
                 pagination.sidx = "t.createdate";//�����ֶ�
                 pagination.sord = "desc";//����ʽ
                 var data = hispeoplebll.GetHistoryPageList(pagination, queryJson);

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
             catch (System.Exception ex)
             {

                 throw new Exception(ex.Message);
             }
         }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = aptitudeinvestigatepeoplebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��ʷ��¼��Աʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            try
            {
                var data = hispeoplebll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }



        /// <summary>
        /// ���֤�����ظ�
        /// </summary>
        /// <param name="IdentifyID">���֤��</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistIdentifyID(string IdentifyID, string keyValue)
        {
            bool IsOk = userBLL.ExistIdentifyID(IdentifyID, keyValue);
            bool IsOk1 = aptitudeinvestigatepeoplebll.ExistIdentifyID(IdentifyID, keyValue);
            if (IsOk == true && IsOk1 == true)
                return Content(true.ToString());
            else
                return Content(false.ToString());
        }


        /// <summary>
        /// �˻������ظ�
        /// </summary>
        /// <param name="Accounts">�˻�ֵ</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistAccount(string Accounts, string keyValue)
        {
            bool IsOk = userBLL.ExistAccount(Accounts, keyValue);
            bool IsOk1 = aptitudeinvestigatepeoplebll.ExistAccount(Accounts, keyValue);
            if (IsOk == true && IsOk1 == true)
                return Content(true.ToString());
            else
                return Content(false.ToString());
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// �ϴ�ͷ��
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //û���ļ��ϴ���ֱ�ӷ���
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string FileEextension = Path.GetExtension(files[0].FileName);
            
            string UserId = OperatorProvider.Provider.Current().UserId;
            string virtualPath = string.Format("/Resource/PhotoFile/{0}{1}", Guid.NewGuid().ToString(), FileEextension);
            string fullFileName = Server.MapPath("~" + virtualPath);
            //�����ļ��У������ļ�
            string path = Path.GetDirectoryName(fullFileName);
            Directory.CreateDirectory(path);
            files[0].SaveAs(fullFileName);

            return Success("�ϴ��ɹ���", virtualPath);
        }
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
            aptitudeinvestigatepeoplebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AptitudeinvestigatepeopleEntity entity)
        {
            try
            {
                var currUser = OperatorProvider.Provider.Current();
                CertificateinspectorsBLL certificateinspectorsbll = new CertificateinspectorsBLL();
                var queryJson = new { UserId = keyValue };
                if (!string.IsNullOrWhiteSpace(entity.WORKOFTYPE))
                {
                    switch (entity.WORKOFTYPE)
                    {
                        case "���ӹ�":
                            if (certificateinspectorsbll.GetList(JsonConvert.SerializeObject(queryJson)).Where(t => t.USERID == keyValue && t.CertType == "������ҵ����֤" && t.WorkType == "�ߴ���ҵ").Count() == 0)
                            {
                                return Error("���ӹ���Ա��Ҫ�ύ֤������Ϊ������ҵ����֤����ҵ���Ϊ�ߴ���ҵ��֤����Ϣ��");
                            }
                            break;
                        case "�纸��":
                            if (certificateinspectorsbll.GetList(JsonConvert.SerializeObject(queryJson)).Where(t => t.USERID == keyValue && t.CertType == "������ҵ����֤" && t.WorkType == "���������и���ҵ").Count() == 0)
                            {
                                return Error("�纸����Ա��Ҫ�ύ֤������Ϊ������ҵ����֤����ҵ���Ϊ���������и���ҵ��֤����Ϣ��");
                            }
                            break;
                        case "���ع�":
                            if (certificateinspectorsbll.GetList(JsonConvert.SerializeObject(queryJson)).Where(t => t.USERID == keyValue && t.CertType == "�����豸��ҵ��Ա֤" && t.WorkType == "���ػ���ҵ").Count() == 0)
                            {
                                return Error("���ع���Ա��Ҫ�ύ֤������Ϊ�����豸��ҵ��Ա֤����ҵ���Ϊ���ػ���ҵ��֤����Ϣ��");
                            }
                            break;
                        case "�繤":
                            if (certificateinspectorsbll.GetList(JsonConvert.SerializeObject(queryJson)).Where(t => t.USERID == keyValue && t.CertType == "������ҵ����֤" && t.WorkType == "�繤��ҵ").Count() == 0)
                            {
                                return Error("�繤��Ա��Ҫ�ύ֤������Ϊ������ҵ����֤����ҵ���Ϊ�繤��ҵ��֤����Ϣ��");
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(entity.SpecialtyType))
                {

                    string dqzyValue = new DataItemDetailBLL().GetItemValue("����רҵ", "SpecialtyType");
                    if ((entity.SpecialtyType + ",").Contains(dqzyValue + ","))
                    {
                        if (certificateinspectorsbll.GetList(JsonConvert.SerializeObject(queryJson)).Where(t => t.USERID == keyValue && t.CertType == "������ҵ����֤" && t.WorkType == "�繤��ҵ").Count() == 0)
                        {
                            return Error("����רҵ��Ա��Ҫ�ύ֤������Ϊ������ҵ����֤����ҵ���Ϊ�繤��ҵ��֤����Ϣ��");
                        }
                    }
                }
                BlackSetEntity blacksetentity = scoresetbll.GetList(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode).Where(t => t.ItemCode == "01" && t.Status == 1).FirstOrDefault();
                entity.IsOverAge = "0";
                if (blacksetentity != null && blacksetentity.ItemValue.Split('|').Length == 4)
                {
                    string brithday = entity.IDENTIFYID.Length == 18 ? entity.IDENTIFYID.Substring(6, 4) + "-" + entity.IDENTIFYID.Substring(10, 2) + "-" + entity.IDENTIFYID.Substring(12, 2) : "19" + entity.IDENTIFYID.Substring(6, 2) + "-" + entity.IDENTIFYID.Substring(8, 2) + "-" + entity.IDENTIFYID.Substring(10, 2);
                    int age = CalculateAge(brithday);
                    entity.BIRTHDAY = DateTime.Parse(brithday);
                    entity.Age = age.ToString();
                    int len = 0;
                    string[] arr1 = new string[] { };//��ͨ��Ա��������
                    string[] arr2 = new string[] { };//������ҵ��Ա��������
                    string[] arr3 = new string[] { };//������Ա��������
                    string[] arr4 = new string[] { };//�����豸��ҵ��Ա��������
                    int isCL = departmentBLL.GetDataTable(string.Format("select count(1) from BIS_BLACKSET where itemcode='10' and deptcode='{0}' and status=1", currUser.OrganizeCode)).Rows[0][0].ToInt();
                    if (isCL > 0)
                    {
                        DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemvalue from BIS_BLACKSET t where status=1 and deptcode='{0}' and (t.itemcode='01' or t.itemcode='06' or t.itemcode='07' or t.itemcode='08') order by itemcode", currUser.OrganizeCode));
                        len = dtItems.Rows.Count;

                        StringBuilder sb = new StringBuilder();
                        if (len > 0)
                        {
                            arr1 = dtItems.Rows[0][0].ToString().Split('|');
                        }
                        if (len > 1)
                        {
                            arr2 = dtItems.Rows[1][0].ToString().Split('|');
                        }
                        if (len > 2)
                        {
                            arr3 = dtItems.Rows[2][0].ToString().Split('|');
                        }
                        if (len > 3)
                        {
                            arr4 = dtItems.Rows[3][0].ToString().Split('|');
                        }
                    }
                    //userEntity.Birthday = DateTime.Parse(birthday);
                    //userEntity.Age = (DateTime.Now.Year - userEntity.Birthday.Value.Year).ToString();
                    if (isCL > 0 && len > 0)
                    {
                        //�ж��Ƿ�����Ա
                        if (entity.USERTYPE == "������Ա" && arr3.Length > 0)
                        {
                            if (entity.GENDER == "��" && (entity.Age.ToInt() < arr3[0].ToInt() || entity.Age.ToInt() > arr3[1].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                            if (entity.GENDER == "Ů" && (entity.Age.ToInt() < arr3[2].ToInt() || entity.Age.ToInt() > arr3[3].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                        }
                        if (entity.ISSPECIAL == "��" && arr2.Length > 0)
                        {
                            if (entity.GENDER == "��" && (entity.Age.ToInt() < arr2[0].ToInt() || entity.Age.ToInt() > arr2[1].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                            if (entity.GENDER == "Ů" && (entity.Age.ToInt() < arr2[2].ToInt() || entity.Age.ToInt() > arr2[3].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                        }
                        if (entity.ISSPECIALEQU == "��" && arr4.Length > 0)
                        {
                            if (entity.GENDER == "��" && (entity.Age.ToInt() < arr4[0].ToInt() || entity.Age.ToInt() > arr4[1].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                            if (entity.GENDER == "Ů" && (entity.Age.ToInt() < arr4[2].ToInt() || entity.Age.ToInt() > arr4[3].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                        }
                        if (entity.ISSPECIALEQU == "��" && entity.ISSPECIAL == "��" && entity.USERTYPE != "������Ա" && arr1.Length > 0)
                        {
                            if (entity.GENDER == "��" && (entity.Age.ToInt() < arr1[0].ToInt() || entity.Age.ToInt() > arr1[1].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                            if (entity.GENDER == "Ů" && (entity.Age.ToInt() < arr1[2].ToInt() || entity.Age.ToInt() > arr1[3].ToInt()))
                            {
                                return Error("����Ա���䳬�䣬�޷�¼��ϵͳ��");
                            }
                        }
                    }
                    else
                    {
                        if (entity.GENDER == "��")
                        {
                            if (age < Convert.ToInt32(blacksetentity.ItemValue.Split('|')[0]) || age > Convert.ToInt32(blacksetentity.ItemValue.Split('|')[1]))
                            {
                                entity.IsOverAge = "1";
                            }
                        }
                        else if (entity.GENDER == "Ů")
                        {
                            if (age < Convert.ToInt32(blacksetentity.ItemValue.Split('|')[2]) || age > Convert.ToInt32(blacksetentity.ItemValue.Split('|')[3]))
                            {
                                entity.IsOverAge = "1";
                            }
                        }
                    }

                }
                bool IsOk = userBLL.ExistIdentifyID(entity.IDENTIFYID, "");
                if (IsOk)
                {
                    var list = (from c in aptitudeinvestigatepeoplebll.GetList("")
                                join
                               d in new PeopleReviewBLL().GetList("") on c.PEOPLEREVIEWID equals d.ID into join1
                                from tt in join1.DefaultIfEmpty()
                                where (tt == null  || tt.ISAUDITOVER == "0" || tt.ISAUDITOVER == "" || tt.ISAUDITOVER == null) && c.IDENTIFYID == entity.IDENTIFYID
                                select c).ToList();
                    if (list.Count() > 0) //�ж�����Ա������û�б����ͨ���������Ƿ���ڸ����֤������
                    {
                        if (list.Where(t => t.ID == keyValue).Count() != list.Count)
                        {
                            return Error("�����֤�Ѿ�����Ա�����д��ڣ�����ʧ��");
                        }
                    }
                    bool IsOk2 = userBLL.ExistAccount(entity.ACCOUNTS, "");
                    bool IsOk3 = aptitudeinvestigatepeoplebll.ExistAccount(entity.ACCOUNTS, keyValue);
                    if (IsOk2 == false || IsOk3 == false)
                        return Error("���˺��Ѿ�����Ա����������Ա�����д��ڣ�����ʧ��");
                }
                aptitudeinvestigatepeoplebll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// ������鵼����Ա
        /// </summary>
        /// <param name="outProId">�����λId</param>
        /// <param name="outEngId">�������Id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportUser(string outProId, string outEngId, string peoplereviewid)
        {
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "��������Ա�޴˲���Ȩ��";
                }
                string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾
                Operator currUser = OperatorProvider.Provider.Current();
                string deptId = outProId;//�����λ
                int error = 0;
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("zip")))
                    {
                        return message;
                    }
                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string decompressionDirectory = Server.MapPath("~/Resource/decompression/") + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "\\";
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);

                    wb.Open(decompressionDirectory + file.FileName.Replace("zip", "xls"));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    int len = 0;
                    string[] arr1 = new string[] { };//��ͨ��Ա��������
                    string[] arr2 = new string[] { };//������ҵ��Ա��������
                    string[] arr3 = new string[] { };//������Ա��������
                    string[] arr4 = new string[] { };//�����豸��ҵ��Ա��������
                    int isCL = departmentBLL.GetDataTable(string.Format("select count(1) from BIS_BLACKSET where itemcode='10' and deptcode='{0}' and status=1", currUser.OrganizeCode)).Rows[0][0].ToInt();
                    if (isCL > 0)
                    {
                        DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemvalue from BIS_BLACKSET t where status=1 and deptcode='{0}' and (t.itemcode='01' or t.itemcode='06' or t.itemcode='07' or t.itemcode='08') order by itemcode", currUser.OrganizeCode));
                        len = dtItems.Rows.Count;

                        StringBuilder sb = new StringBuilder();
                        if (len > 0)
                        {
                            arr1 = dtItems.Rows[0][0].ToString().Split('|');
                        }
                        if (len > 1)
                        {
                            arr2 = dtItems.Rows[1][0].ToString().Split('|');
                        }
                        if (len > 2)
                        {
                            arr3 = dtItems.Rows[2][0].ToString().Split('|');
                        }
                        if (len > 3)
                        {
                            arr4 = dtItems.Rows[3][0].ToString().Split('|');
                        }
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //����
                        string realname = dt.Rows[i]["����"].ToString();
                        //�Ա�
                        string gender = dt.Rows[i]["�Ա�"].ToString();
                        //if (string.IsNullOrEmpty(realname) || string.IsNullOrEmpty(gender))
                        //{
                        //    continue;
                        //}
                        //����
                        string native = dt.Rows[i]["ʡ�����ᣩ"].ToString();
                        //ѧ��
                        string degrees = dt.Rows[i]["ѧ��"].ToString();
                        string degreesid = "";
                        //����
                        string nation = dt.Rows[i]["����"].ToString();
                        //��λ
                        string dutyName = dt.Rows[i]["��λ"].ToString();
                        string dutyid = "";
                        //��Ա����
                        string userType = dt.Rows[i]["��Ա����"].ToString();
                        //���֤��
                        string identity = dt.Rows[i]["���֤��"].ToString();
                        //�˺�
                        string account = dt.Rows[i]["�˺�"].ToString();
                        //�ֻ���
                        string mobile = dt.Rows[i]["�ֻ�����"].ToString();

                        //�Ƿ�������ҵ��Ա
                        string isspecial = dt.Rows[i]["�Ƿ�Ϊ������ҵ��Ա"].ToString();
                        //�Ƿ������豸��ҵ��Ա
                        string isspecialequ = dt.Rows[i]["�Ƿ�Ϊ�����豸��ҵ��Ա"].ToString();
                        ////�Ƿ�Ϊ������
                        //string isfourperson = dt.Rows[i][13].ToString();
                        ////���������
                        //string fourpersontype = dt.Rows[i][14].ToString();
                        //�Ƿ����
                        string isepiboly = "1";

                        string worktype = dt.Rows[i]["����"].ToString();
                        string workyear = dt.Rows[i]["����������"].ToString();
                        string healthstate = dt.Rows[i]["����״��"].ToString();
                        string isapplicationldap = "";
                        string isoverage = "0";
                        string birthday = "";
                        //string accounttype = dt.Rows[i][17].ToString();
                        string specialtype = dt.Rows[i]["רҵ����"].ToString();
                        string workertype = dt.Rows[i]["�ù���ʽ"].ToString();
                        string address = dt.Rows[i]["��ͥ��ַ"].ToString();
                        string quickquery = dt.Rows[i]["ʡ�����ᣩ"].ToString();
                        string manager = dt.Rows[i]["��"].ToString();
                        string district = dt.Rows[i]["��"].ToString();
                        string street = dt.Rows[i]["����"].ToString();
                        string ldfj = dt.Rows[i]["�Ͷ���ͬ���������ƣ�"].ToString();
                        string gsfj = dt.Rows[i]["���˱��գ��������ƣ�"].ToString();
                        string syfj = dt.Rows[i]["���������˺����գ��������ƣ�"].ToString();
                        string tjfj = dt.Rows[i]["�����ϣ��������ƣ�"].ToString();
                        string qtfj = dt.Rows[i]["�����������������ƣ�"].ToString();

                        DataItemModel di = dic.GetDataItemList("Degrees").Where(a => a.ItemName == degrees).FirstOrDefault();
                        if (di != null)
                        {
                            degreesid = di.ItemValue;
                        }
                        //---****ֵ���ڿ���֤*****--
                        var isldap = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("IsOpenPassword");
                        if (isldap == "true")
                        {
                            isapplicationldap = dt.Rows[i]["�Ƿ���Ҫ����LDAP�˺�"].ToString();
                            if (string.IsNullOrEmpty(realname) || string.IsNullOrEmpty(identity) || string.IsNullOrEmpty(dutyName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(userType) || string.IsNullOrEmpty(isspecial) || string.IsNullOrEmpty(isspecialequ))
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(realname) || string.IsNullOrEmpty(identity) || string.IsNullOrEmpty(dutyName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(userType) || string.IsNullOrEmpty(isspecial) || string.IsNullOrEmpty(isspecialequ) || string.IsNullOrEmpty(account))
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        //--�ֻ�����֤
                        if (!string.IsNullOrEmpty(mobile))
                        {
                            if (!Regex.IsMatch(mobile, @"^(\+\d{2,3}\-)?\d{11}$", RegexOptions.IgnoreCase))
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " �ֻ��Ÿ�ʽ����,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        //���������λ�Ƿ������乫˾���߲���
                        if (string.IsNullOrEmpty(deptId) || deptId == "undefined")
                        {
                            //������˾
                            RoleEntity data = postCache.GetList(orgId, "true").OrderBy(x => x.SortCode).Where(a => a.FullName == dutyName).FirstOrDefault();
                            if (data == null)
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��λ�����ڸù�˾,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            //��������
                            //������˾
                            RoleEntity data = postCache.GetList(orgId, deptId).OrderBy(x => x.SortCode).Where(a => a.FullName == dutyName).FirstOrDefault();
                            if (data == null)
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��λ�����ڸò���,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        //--**��֤��λ�Ƿ����**--


                        RoleEntity re = postBLL.GetList().Where(a => (a.FullName == dutyName && a.OrganizeId == orgId)).FirstOrDefault();
                        if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                        {
                            re = postBLL.GetList().Where(a => (a.FullName == dutyName && a.OrganizeId == orgId && a.DeptId == deptId)).FirstOrDefault();
                            if (re == null)
                            {
                                re = postBLL.GetList().Where(a => (a.FullName == dutyName && a.OrganizeId == orgId && a.Nature == departmentBLL.GetEntity(deptId).Nature)).FirstOrDefault();
                            }
                        }
                        if (re == null)
                        {
                            falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��λ����,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        else
                        {
                            dutyid = re.RoleId;
                        }
                        //---****���֤��ȷ��֤*****--
                        if (!Regex.IsMatch(identity, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ���֤�Ÿ�ʽ����,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        ////---****���֤�ظ���֤*****--��֤˫����Ա��ͬ��ʱ�����ظ���
                        //if (!userBLL.ExistIdentifyID(identity, ""))
                        //{
                        //    falseMessage += "</br>" + "��" + (i + 2) + "�����֤���ظ�,δ�ܵ���.";
                        //    error++;
                        //    continue;
                        //}
                        ////---****���֤�ظ���֤*****--��֤������Ա��
                        //if (!aptitudeinvestigatepeoplebll.ExistIdentifyID(identity, ""))
                        //{
                        //    falseMessage += "</br>" + "��" + (i + 2) + "�����֤���ظ�,δ�ܵ���.";
                        //    error++;
                        //    continue;
                        //}

                        //---****��Ա���䳬����֤*****--
                        BlackSetEntity blacksetentity = scoresetbll.GetList(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode).Where(t => t.ItemCode == "01" && t.Status == 1).FirstOrDefault();
                        int age = 0;
                        string brithday = identity.Length == 18 ? identity.Substring(6, 4) + "-" + identity.Substring(10, 2) + "-" + identity.Substring(12, 2) : "19" + identity.Substring(6, 2) + "-" + identity.Substring(8, 2) + "-" + identity.Substring(10, 2);
                        birthday = brithday;
                        age = CalculateAge(brithday);
                        if (blacksetentity != null && blacksetentity.ItemValue.Split('|').Length == 4)
                        {
                            if (isCL > 0 && len > 0)
                            {
                                //�ж��Ƿ�����Ա
                                if (userType == "������Ա" && arr3.Length > 0)
                                {
                                    if (gender == "��" && (age < arr3[0].ToInt() || age > arr3[1].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                    if (gender == "Ů" && (age < arr3[2].ToInt() || age > arr3[3].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                }
                                if (isspecial == "��" && arr2.Length > 0)
                                {
                                    if (gender == "��" && (age < arr2[0].ToInt() || age > arr2[1].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                    if (gender == "Ů" && (age < arr2[2].ToInt() || age > arr2[3].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                }
                                if (isspecialequ == "��" && arr4.Length > 0)
                                {
                                    if (gender == "��" && (age < arr4[0].ToInt() || age > arr4[1].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                    if (gender == "Ů" && (age < arr4[2].ToInt() || age > arr4[3].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                }
                                if (isspecialequ == "��" && isspecial == "��" && userType != "������Ա" && arr1.Length > 0)
                                {
                                    if (gender == "��" && (age < arr1[0].ToInt() || age > arr1[1].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                    if (gender == "Ů" && (age < arr1[2].ToInt() || age > arr1[3].ToInt()))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��,δ�ܵ���.";
                                        error++;
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                if (gender == "��")
                                {
                                    if (age < Convert.ToInt32(blacksetentity.ItemValue.Split('|')[0]) || age > Convert.ToInt32(blacksetentity.ItemValue.Split('|')[1]))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��������Χ.";
                                        isoverage = "1";
                                    }
                                }
                                else if (gender == "Ů")
                                {
                                    if (age < Convert.ToInt32(blacksetentity.ItemValue.Split('|')[2]) || age > Convert.ToInt32(blacksetentity.ItemValue.Split('|')[3]))
                                    {
                                        falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ��Ա���䳬��������Χ.";
                                        isoverage = "1";
                                    }
                                }
                            }

                        }


                        //---****�˺��ظ���֤*****--��֤˫����Ա��ͬ��ʱ�����ظ���
                        //if (!userBLL.ExistAccount(account, ""))
                        //{
                        //    falseMessage += "</br>" + "��" + (i + 2) + "���˺��ظ�,δ�ܵ���.";
                        //    error++;
                        //    continue;
                        //}
                        //---****�˺��ظ���֤ * ****--��֤������Ա��
                        //if (!aptitudeinvestigatepeoplebll.ExistAccount(account, ""))
                        //{
                        //    falseMessage += "</br>" + "��" + (i + 2) + "���˺��ظ�,δ�ܵ���.";
                        //    error++;
                        //    continue;
                        //}
                        AptitudeinvestigatepeopleEntity people = new AptitudeinvestigatepeopleEntity();
                        people.ID = Guid.NewGuid().ToString();
                        people.REALNAME = realname;
                        people.GENDER = gender;
                        people.Age = age.ToString();
                        try
                        {
                            if (!string.IsNullOrEmpty(birthday))
                            {
                                people.BIRTHDAY = DateTime.Parse(DateTime.Parse(birthday).ToString("yyyy-MM-dd"));
                            }

                        }
                        catch
                        {
                            falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ʱ������,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(specialtype))
                        {
                            people.SpecialtyType = dataitemdetailbll.GetItemValue(specialtype, "SpecialtyType");
                        }
                        if (!string.IsNullOrWhiteSpace(workertype))
                        {
                            people.WorkerType= dataitemdetailbll.GetItemValue(workertype, "WorkerType");
                        }
                        people.DEGREES = degrees;
                        people.DEGREESID = degreesid;
                        people.DUTYID = dutyid;
                        people.DUTYNAME = dutyName;
                        people.IDENTIFYID = identity;
                        people.ORGANIZEID = orgId;
                        people.ORGANIZECODE = OperatorProvider.Provider.Current().OrganizeCode;
                        people.OUTPROJECTID = outProId;
                        people.IsOverAge = isoverage;
                        List<DepartmentEntity> listdept = new DepartmentBLL().GetList().Where(x => x.DepartmentId == outProId).ToList();
                        if (listdept.Count > 0)
                        {
                            var dept = listdept.FirstOrDefault();
                            if (dept != null)
                                people.OUTPROJECTCODE = dept.EnCode;
                            else
                                people.OUTPROJECTCODE = OperatorProvider.Provider.Current().DeptCode;
                        }
                        else
                        {
                            people.OUTPROJECTCODE = OperatorProvider.Provider.Current().DeptCode;
                        }
                        people.OUTENGINEERID = outEngId;
                        people.ISEPIBOLY = isepiboly;
                        people.ISSPECIAL = isspecial;
                        people.ISSPECIALEQU = isspecialequ;
                        people.MOBILE = mobile;
                        people.NATION = nation;
                        people.NATIVE = native;
                        people.USERTYPE = userType;
                        people.WORKOFTYPE = worktype;
                        people.WORKYEAR = workyear;
                        people.STATEOFHEALTH = healthstate;
                        people.PEOPLEREVIEWID = peoplereviewid;
                        people.ISFOURPERSON = "��";
                        people.ACCOUNTS = account;
                        people.QuickQuery = quickquery.Replace("ʡ", "");
                        people.Manager = manager;
                        people.District = district;
                        people.Street = street;
                        //people.FOURPERSONTYPE = fourpersontype;
                        if (isldap == "true")
                        {
                            people.IsApplicationLdap = isapplicationldap == "��" ? "1" : "0";
                            people.AccountType = "0";
                            try
                            {
                                string ldapaccount = "CRP_";
                                int n = 0;
                                foreach (char c in realname)
                                {
                                    n++;
                                    if (n == 1)
                                    {
                                        ldapaccount += (ConvertToPinYin(c.ToString())).ToUpper();
                                    }
                                    else
                                    {
                                        ldapaccount += (Str.PinYin(c.ToString())).ToUpper();
                                    }
                                }
                                ldapaccount += identity.Substring(identity.Length - 4);
                                people.ACCOUNTS = ldapaccount;
                                account = ldapaccount;
                            }
                            catch { }
                        }
                        //---****���֤��֤������� ���þ��˺ţ������ж��˺��Ƿ��ظ�
                        if (!userBLL.ExistIdentifyID(identity, ""))
                        {
                            UserEntity user = userBLL.GetUserByIdCard(identity);
                            people.ACCOUNTS = user.Account;
                        }
                        else
                        {
                            //---****���֤���ظ���֤*****--��֤��Ա���ʱ�ͬ��ʱ�����ظ���
                            var list = (from c in aptitudeinvestigatepeoplebll.GetList("")
                                        join
                                       d in new PeopleReviewBLL().GetList("") on c.PEOPLEREVIEWID equals d.ID into join1
                                        from tt in join1.DefaultIfEmpty()
                                        where (tt == null || tt.ISAUDITOVER == "0" || tt.ISAUDITOVER=="" || tt.ISAUDITOVER==null) && c.IDENTIFYID == identity
                                        select c).ToList();
                            if (list.Count() > 0)
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " ���֤�ظ�,���֤�Ŵ�������Ա��������л��ߵ�ǰExcel����������˺��ظ�������,δ�ܵ���.";
                                error++;
                                continue;
                            }
                            //---****�˺��ظ���֤*****--��֤˫����Ա��ͬ��ʱ�����ظ���
                            if (!userBLL.ExistAccount(account, ""))
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "�� " + realname + " �˺��ظ�,δ�ܵ���.";
                                error++;
                                continue;
                            }
                            //---****�˺��ظ���֤*****--��֤������Ա��
                            if (!aptitudeinvestigatepeoplebll.ExistAccount(account, ""))
                            {
                                falseMessage += "</br>" + "��" + (i + 3) + "�� "+ realname + " �˺��ظ�,�˺Ŵ�������Ա��������л��ߵ�ǰExcel����������˺��ظ�������,δ�ܵ���.";
                                error++;
                                continue;
                            }
                        }
                        #region �Ͷ���ͬ
                        if (!string.IsNullOrWhiteSpace(ldfj))
                        {
                            foreach (var item in ldfj.Replace("��", ",").Split(','))
                            {
                                var fileinfo = new FileInfo(decompressionDirectory + item);
                                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                string fileguid = Guid.NewGuid().ToString();
                                fileInfoEntity.Create();
                                fileInfoEntity.RecId = people.ID + "02"; //����ID
                                fileInfoEntity.FileName = item;
                                fileInfoEntity.FilePath = "~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fileguid + fileinfo.Extension;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                fileInfoEntity.FileExtensions = fileinfo.Extension;
                                fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        #endregion

                        #region ���˱���
                        if (!string.IsNullOrWhiteSpace(gsfj))
                        {
                            foreach (var item in gsfj.Replace("��", ",").Split(','))
                            {
                                var fileinfo = new FileInfo(decompressionDirectory + item);
                                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                string fileguid = Guid.NewGuid().ToString();
                                fileInfoEntity.Create();
                                fileInfoEntity.RecId = people.ID + "03"; //����ID
                                fileInfoEntity.FileName = item;
                                fileInfoEntity.FilePath = "~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fileguid + fileinfo.Extension;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                fileInfoEntity.FileExtensions = fileinfo.Extension;
                                fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        #endregion

                        #region ���������˺�����
                        if (!string.IsNullOrWhiteSpace(syfj))
                        {
                            foreach (var item in syfj.Replace("��", ",").Split(','))
                            {
                                var fileinfo = new FileInfo(decompressionDirectory + item);
                                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                string fileguid = Guid.NewGuid().ToString();
                                fileInfoEntity.Create();
                                fileInfoEntity.RecId = people.ID + "05"; //����ID
                                fileInfoEntity.FileName = item;
                                fileInfoEntity.FilePath = "~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fileguid + fileinfo.Extension;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                fileInfoEntity.FileExtensions = fileinfo.Extension;
                                fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        #endregion

                        #region ������
                        if (!string.IsNullOrWhiteSpace(tjfj))
                        {
                            foreach (var item in tjfj.Replace("��", ",").Split(','))
                            {
                                var fileinfo = new FileInfo(decompressionDirectory + item);
                                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                string fileguid = Guid.NewGuid().ToString();
                                fileInfoEntity.Create();
                                fileInfoEntity.RecId = people.ID + "01"; //����ID
                                fileInfoEntity.FileName = item;
                                fileInfoEntity.FilePath = "~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fileguid + fileinfo.Extension;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                fileInfoEntity.FileExtensions = fileinfo.Extension;
                                fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        #endregion

                        #region ��������
                        if (!string.IsNullOrWhiteSpace(qtfj))
                        {
                            foreach (var item in qtfj.Replace("��", ",").Split(','))
                            {
                                var fileinfo = new FileInfo(decompressionDirectory + item);
                                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                string fileguid = Guid.NewGuid().ToString();
                                fileInfoEntity.Create();
                                fileInfoEntity.RecId = people.ID + "04"; //����ID
                                fileInfoEntity.FileName = item;
                                fileInfoEntity.FilePath = "~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fileguid + fileinfo.Extension;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//�ļ���С��kb��
                                fileInfoEntity.FileExtensions = fileinfo.Extension;
                                fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                TransportRemoteToServer(Server.MapPath("~/Resource//PeopleAudit/" + DateTime.Now.ToString("yyyyMMdd") + "/"), decompressionDirectory + item, fileguid + fileinfo.Extension);
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        #endregion
                        try
                        {
                            aptitudeinvestigatepeoplebll.SaveForm(people.ID, people);
                        }
                        catch
                        {
                            error++;
                        }
                    }
                    message = "����" + dt.Rows.Count + "����¼,�ɹ�����" + (dt.Rows.Count - error) + "����ʧ��" + error + "��";
                    message += "</br>" + falseMessage;
                }
                return message;
            }
            catch (Exception ex)
            {
                return message;
            }
            
        }

        public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();

            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);
                    if (fileName != "")
                    {
                        if ((System.IO.File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!System.IO.File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = System.IO.File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
        }

        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="src">Զ�̷�����·���������ļ���·����</param>  
        /// <param name="dst">�����ļ���·��</param>  
        /// <param name="filename"></param> 
        public static void TransportRemoteToServer(string src, string dst, string filename)
        {
            if (!Directory.Exists(src))
            {
                Directory.CreateDirectory(src);
            }
            FileStream inFileStream = new FileStream(src + filename, FileMode.OpenOrCreate);

            FileStream outFileStream = new FileStream(dst, FileMode.Open);

            byte[] buf = new byte[outFileStream.Length];

            int byteCount;

            while ((byteCount = outFileStream.Read(buf, 0, buf.Length)) > 0)
            {
                inFileStream.Write(buf, 0, byteCount);

            }

            inFileStream.Flush();

            inFileStream.Close();

            outFileStream.Flush();

            outFileStream.Close();

        }

        /// <summary>
        /// �������������Ա�������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SummitPhyInfo(string keyValue, PhyInfoEntity entity)
        {
            aptitudeinvestigatepeoplebll.SummitPhyInfo(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �ٴ��ύʱ������Ա��Ϣ
        /// </summary>
        /// <param name="oldKeyValue"></param>
        /// <param name="newKeyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CopyPeople(string oldKeyValue, string newKeyValue)
        {
            try
            {
                List<AptitudeinvestigatepeopleEntity> list = new List<AptitudeinvestigatepeopleEntity>();
                list = aptitudeinvestigatepeoplebll.GetList("").Where(t => t.PEOPLEREVIEWID == oldKeyValue).ToList();
                foreach (var item in list)
                {

                    List<FileInfoEntity> file1 = fileInfoBLL.GetFileList(item.ID + "01");
                    List<FileInfoEntity> file2 = fileInfoBLL.GetFileList(item.ID + "02");
                    List<FileInfoEntity> file3 = fileInfoBLL.GetFileList(item.ID + "03");
                    List<FileInfoEntity> file4 = fileInfoBLL.GetFileList(item.ID + "04");
                    List<CertificateinspectorsEntity> certList = certificateinspectorsbll.GetList(JsonConvert.SerializeObject(new { UserId = item.ID })).ToList();
                    item.ID = Guid.NewGuid().ToString();
                    foreach (var cert in certList)
                    {
                        List<FileInfoEntity> file5 = fileInfoBLL.GetFileList(cert.ID);
                        cert.ID = Guid.NewGuid().ToString();
                        foreach (var itemfile5 in file5)
                        {
                            itemfile5.RecId = cert.ID;
                            fileInfoBLL.SaveForm("", itemfile5);
                        }
                        cert.USERID = item.ID;
                        certificateinspectorsbll.SaveForm(cert.ID, cert);
                    }
                   
                    foreach (var itemfile1 in file1)
                    {
                        itemfile1.RecId = item.ID + "01";
                        fileInfoBLL.SaveForm("", itemfile1);
                    }
                    foreach (var itemfile2 in file2)
                    {
                        itemfile2.RecId = item.ID + "02";
                        fileInfoBLL.SaveForm("", itemfile2);
                    }
                    foreach (var itemfile3 in file3)
                    {
                        itemfile3.RecId = item.ID + "03";
                        fileInfoBLL.SaveForm("", itemfile3);
                    }
                    foreach (var itemfile4 in file4)
                    {
                        itemfile4.RecId = item.ID + "04";
                        fileInfoBLL.SaveForm("", itemfile4);
                    }
                    item.PEOPLEREVIEWID = newKeyValue;
                    aptitudeinvestigatepeoplebll.SaveForm(item.ID, item);
                }
                return Success("���Ƴɹ�");
            }
            catch (Exception ex)
            {
                return Error("����ʧ��");
            }
           
        }


        /// <summary>
        /// ���֤�����ظ�
        /// </summary>
        /// <param name="IdentifyID">���֤��</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExistIdentifyID(string IdentifyID, string keyValue)
        {
            bool IsOk = userBLL.ExistIdentifyID(IdentifyID, keyValue);
            string account = "";
            if (!IsOk)//�ж���Ա�������Ƿ���ڸ����֤�����ݣ����������Ա�������˺ŵ����������ݵ��˺�
            {
                UserEntity user = userBLL.GetUserByIdCard(IdentifyID);
                account = user.Account;
                var data = new
                {
                    IsOk = IsOk.ToString(),
                    Account = account
                };
                return ToJsonResult(data);
            }
            else
            {
                var list = (from c in aptitudeinvestigatepeoplebll.GetList("")
                            join
                           d in new PeopleReviewBLL().GetList("") on c.PEOPLEREVIEWID equals d.ID into join1
                            from tt in join1.DefaultIfEmpty()
                            where (tt == null || tt.ISAUDITOVER == "0" || tt.ISAUDITOVER=="" || tt.ISAUDITOVER == null) && c.IDENTIFYID == IdentifyID
                            select c).ToList(); 
                if (list.Count > 0) //�ж�����Ա������û�б����ͨ���������Ƿ���ڸ����֤������
                {
                    if (list.Where(t => t.ID == keyValue).Count() == list.Count)
                    {
                        var data = new
                        {
                            IsOk = IsOk.ToString(),
                            Account = account
                        };
                        return ToJsonResult(data);
                    }
                    else
                    {
                        return Error("�����֤�Ѿ�����Ա�����д��ڣ�����������");
                    }
                    
                }
                else
                {
                    var data = new
                    {
                        IsOk = IsOk.ToString(),
                        Account = account
                    };
                    return ToJsonResult(data);
                }
            }
        }
        #endregion

        #region ����ת����ȫƴ��ƴ��

        #region ����ƴ������������
        //����ƴ������������
        private static int[] getValue = new int[]
                    {
                        -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036,
                        -20032, -20026, -20002, -19990, -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763,
                        -19756, -19751, -19746, -19741, -19739, -19728, -19725, -19715, -19540, -19531, -19525, -19515,
                        -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, -19263, -19261, -19249,
                        -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023, -19018, -19006,
                        -19003, -18996, -18977, -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735,
                        -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501, -18490, -18478, -18463, -18448,
                        -18447, -18446, -18239, -18237, -18231, -18220, -18211, -18201, -18184, -18183, -18181, -18012,
                        -17997, -17988, -17970, -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752,
                        -17733, -17730, -17721, -17703, -17701, -17697, -17692, -17683, -17676, -17496, -17487, -17482,
                        -17468, -17454, -17433, -17427, -17417, -17202, -17185, -16983, -16970, -16942, -16915, -16733,
                        -16708, -16706, -16689, -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448,
                        -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, -16393, -16220, -16216,
                        -16212, -16205, -16202, -16187, -16180, -16171, -16169, -16158, -16155, -15959, -15958, -15944,
                        -15933, -15920, -15915, -15903, -15889, -15878, -15707, -15701, -15681, -15667, -15661, -15659,
                        -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419, -15416, -15408, -15394,
                        -15385, -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153, -15150,
                        -15149, -15144, -15143, -15141, -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109,
                        -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922, -14921, -14914, -14908, -14902,
                        -14894, -14889, -14882, -14873, -14871, -14857, -14678, -14674, -14670, -14668, -14663, -14654,
                        -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345,
                        -14170, -14159, -14151, -14149, -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112,
                        -14109, -14099, -14097, -14094, -14092, -14090, -14087, -14083, -13917, -13914, -13910, -13907,
                        -13906, -13905, -13896, -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601,
                        -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359, -13356, -13343,
                        -13340, -13329, -13326, -13318, -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076,
                        -13068, -13063, -13060, -12888, -12875, -12871, -12860, -12858, -12852, -12849, -12838, -12831,
                        -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359, -12346, -12320, -12300,
                        -12120, -12099, -12089, -12074, -12067, -12058, -12039, -11867, -11861, -11847, -11831, -11798,
                        -11781, -11604, -11589, -11536, -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067,
                        -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, -11018, -11014, -10838, -10832,
                        -10815, -10800, -10790, -10780, -10764, -10587, -10544, -10533, -10519, -10331, -10329, -10328,
                        -10322, -10315, -10309, -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254
                    };

        //����ƴ������
        private static string[] getName = new string[]
                    {
                        "A", "Ai", "An", "Ang", "Ao", "Ba", "Bai", "Ban", "Bang", "Bao", "Bei", "Ben",
                        "Beng", "Bi", "Bian", "Biao", "Bie", "Bin", "Bing", "Bo", "Bu", "Ba", "Cai", "Can",
                        "Cang", "Cao", "Ce", "Ceng", "Cha", "Chai", "Chan", "Chang", "Chao", "Che", "Chen", "Cheng",
                        "Chi", "Chong", "Chou", "Chu", "Chuai", "Chuan", "Chuang", "Chui", "Chun", "Chuo", "Ci", "Cong",
                        "Cou", "Cu", "Cuan", "Cui", "Cun", "Cuo", "Da", "Dai", "Dan", "Dang", "Dao", "De",
                        "Deng", "Di", "Dian", "Diao", "Die", "Ding", "Diu", "Dong", "Dou", "Du", "Duan", "Dui",
                        "Dun", "Duo", "E", "En", "Er", "Fa", "Fan", "Fang", "Fei", "Fen", "Feng", "Fo",
                        "Fou", "Fu", "Ga", "Gai", "Gan", "Gang", "Gao", "Ge", "Gei", "Gen", "Geng", "Gong",
                        "Gou", "Gu", "Gua", "Guai", "Guan", "Guang", "Gui", "Gun", "Guo", "Ha", "Hai", "Han",
                        "Hang", "Hao", "He", "Hei", "Hen", "Heng", "Hong", "Hou", "Hu", "Hua", "Huai", "Huan",
                        "Huang", "Hui", "Hun", "Huo", "Ji", "Jia", "Jian", "Jiang", "Jiao", "Jie", "Jin", "Jing",
                        "Jiong", "Jiu", "Ju", "Juan", "Jue", "Jun", "Ka", "Kai", "Kan", "Kang", "Kao", "Ke",
                        "Ken", "Keng", "Kong", "Kou", "Ku", "Kua", "Kuai", "Kuan", "Kuang", "Kui", "Kun", "Kuo",
                        "La", "Lai", "Lan", "Lang", "Lao", "Le", "Lei", "Leng", "Li", "Lia", "Lian", "Liang",
                        "Liao", "Lie", "Lin", "Ling", "Liu", "Long", "Lou", "Lu", "Lv", "Luan", "Lue", "Lun",
                        "Luo", "Ma", "Mai", "Man", "Mang", "Mao", "Me", "Mei", "Men", "Meng", "Mi", "Mian",
                        "Miao", "Mie", "Min", "Ming", "Miu", "Mo", "Mou", "Mu", "Na", "Nai", "Nan", "Nang",
                        "Nao", "Ne", "Nei", "Nen", "Neng", "Ni", "Nian", "Niang", "Niao", "Nie", "Nin", "Ning",
                        "Niu", "Nong", "Nu", "Nv", "Nuan", "Nue", "Nuo", "O", "Ou", "Pa", "Pai", "Pan",
                        "Pang", "Pao", "Pei", "Pen", "Peng", "Pi", "Pian", "Piao", "Pie", "Pin", "Ping", "Po",
                        "Pu", "Qi", "Qia", "Qian", "Qiang", "Qiao", "Qie", "Qin", "Qing", "Qiong", "Qiu", "Qu",
                        "Quan", "Que", "Qun", "Ran", "Rang", "Rao", "Re", "Ren", "Reng", "Ri", "Rong", "Rou",
                        "Ru", "Ruan", "Rui", "Run", "Ruo", "Sa", "Sai", "San", "Sang", "Sao", "Se", "Sen",
                        "Seng", "Sha", "Shai", "Shan", "Shang", "Shao", "She", "Shen", "Sheng", "Shi", "Shou", "Shu",
                        "Shua", "Shuai", "Shuan", "Shuang", "Shui", "Shun", "Shuo", "Si", "Song", "Sou", "Su", "Suan",
                        "Sui", "Sun", "Suo", "Ta", "Tai", "Tan", "Tang", "Tao", "Te", "Teng", "Ti", "Tian",
                        "Tiao", "Tie", "Ting", "Tong", "Tou", "Tu", "Tuan", "Tui", "Tun", "Tuo", "Wa", "Wai",
                        "Wan", "Wang", "Wei", "Wen", "Weng", "Wo", "Wu", "Xi", "Xia", "Xian", "Xiang", "Xiao",
                        "Xie", "Xin", "Xing", "Xiong", "Xiu", "Xu", "Xuan", "Xue", "Xun", "Ya", "Yan", "Yang",
                        "Yao", "Ye", "Yi", "Yin", "Ying", "Yo", "Yong", "You", "Yu", "Yuan", "Yue", "Yun",
                        "Za", "Zai", "Zan", "Zang", "Zao", "Ze", "Zei", "Zen", "Zeng", "Zha", "Zhai", "Zhan",
                        "Zhang", "Zhao", "Zhe", "Zhen", "Zheng", "Zhi", "Zhong", "Zhou", "Zhu", "Zhua", "Zhuai", "Zhuan",
                        "Zhuang", "Zhui", "Zhun", "Zhuo", "Zi", "Zong", "Zou", "Zu", "Zuan", "Zui", "Zun", "Zuo"
                    };
        #endregion

        /// <summary>
        /// ����ת����ȫƴ��ƴ��
        /// </summary>
        /// <param name="chstr">�����ַ���</param>
        /// <returns>ת�����ƴ���ַ���</returns>
        public static string ConvertToPinYin(string chstr)
        {
            Regex reg = new Regex("^[\u4e00-\u9fa5]$");//��֤�Ƿ����뺺��
            byte[] arr = new byte[2];
            string pystr = "";
            int asc = 0, M1 = 0, M2 = 0;
            char[] mChar = string.IsNullOrEmpty(chstr) ? new char[0] : chstr.ToCharArray();//��ȡ���ֶ�Ӧ���ַ�����
            for (int j = 0; j < mChar.Length; j++)
            {
                //���������Ǻ���
                if (reg.IsMatch(mChar[j].ToString()))
                {
                    arr = System.Text.Encoding.Default.GetBytes(mChar[j].ToString());
                    M1 = (short)(arr[0]);
                    M2 = (short)(arr[1]);
                    asc = M1 * 256 + M2 - 65536;
                    if (asc > 0 && asc < 160)
                    {
                        pystr += mChar[j];
                    }
                    else
                    {
                        switch (asc)
                        {
                            case -9254:
                                pystr += "Zhen";
                                break;
                            case -8985:
                                pystr += "Qian";
                                break;
                            case -5463:
                                pystr += "Jia";
                                break;
                            case -8274:
                                pystr += "Ge";
                                break;
                            case -5448:
                                pystr += "Ga";
                                break;
                            case -5447:
                                pystr += "La";
                                break;
                            case -4649:
                                pystr += "Chen";
                                break;
                            case -5436:
                                pystr += "Mao";
                                break;
                            case -5213:
                                pystr += "Mao";
                                break;
                            case -3597:
                                pystr += "Die";
                                break;
                            case -5659:
                                pystr += "Tian";
                                break;
                            default:
                                for (int i = (getValue.Length - 1); i >= 0; i--)
                                {
                                    if (getValue[i] <= asc) //�жϺ��ֵ�ƴ���������Ƿ���ָ����Χ��
                                    {
                                        pystr += getName[i];//�����������Χ���ȡ��Ӧ��ƴ��
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
                else//������Ǻ���
                {
                    pystr += mChar[j].ToString();//������Ǻ����򷵻�
                }
            }
            return pystr;//���ػ�ȡ���ĺ���ƴ��
        }

        #endregion


        #region �������ڻ�ȡ����
        public static int CalculateAge(string Brithday)
        {
            DateTime BrithDate = Convert.ToDateTime(Brithday);
            DateTime NowDate = DateTime.Now;
            int age = NowDate.Year - BrithDate.Year;
            if (NowDate.Month < BrithDate.Month || (NowDate.Month == BrithDate.Month && NowDate.Day < BrithDate.Day))
            {
                age--;
            }
            return age;
        }
        #endregion

        /// <summary>
        /// ��ȡ�ϴ�ģ��
        /// </summary>
        /// <param name="filename"></param>
        public ActionResult DownloadTemplate(string filename)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/" + filename));
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            wb.Save(Server.MapPath("~/Resource/Temp/" + filename));
            return Success("�����ɹ���", filename);
        }
    }
}
