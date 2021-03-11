using ERCHTMS.Entity.FireManage;
using ERCHTMS.Busines.FireManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using Newtonsoft.Json;
using System;
using ERCHTMS.Entity.SystemManage;
using System.Data;
using System.Web;
using BSFramework.Util.Extension;
using Aspose.Words;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// �� �����ճ�Ѳ��
    /// </summary>
    public class EverydayPatrolController : MvcControllerBase
    {
        private EverydayPatrolBLL everydaypatrolbll = new EverydayPatrolBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private EverydayPatrolDetailBLL everydaypatroldetailbll = new EverydayPatrolDetailBLL();
        private AffirmRecordBLL affirmrecordbll = new AffirmRecordBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            //ViewBag.ItemDetailValue = "";
            ////��ȡ�ճ�Ѳ��ID
            //DataItemDetailEntity entity = dataitemdetailbll.GetListByItemCodeEntity("EverydayPatrol");
            //if (entity != null)
            //    ViewBag.ItemDetailValue = entity.ItemValue;
            ViewBag.ItemDetailValue1 = "";//�ռ�
            ViewBag.ItemDetailValue2 = "";//�ܼ�
            ViewBag.ItemDetailValue3 = "";//�¼�
            ViewBag.ItemDetailValue4 = "";//����
            //��ȡ�ճ�Ѳ��ID
            DataItemDetailEntity entity1 = dataitemdetailbll.GetListByItemCodeEntity("RJ");
            if (entity1 != null)
                ViewBag.ItemDetailValue1 = entity1.ItemValue;
            DataItemDetailEntity entity2 = dataitemdetailbll.GetListByItemCodeEntity("ZJ");
            if (entity2 != null)
                ViewBag.ItemDetailValue2 = entity2.ItemValue;
            DataItemDetailEntity entity3 = dataitemdetailbll.GetListByItemCodeEntity("YJ");
            if (entity3 != null)
                ViewBag.ItemDetailValue3 = entity3.ItemValue;
            DataItemDetailEntity entity4 = dataitemdetailbll.GetListByItemCodeEntity("QT");
            if (entity4 != null)
                ViewBag.ItemDetailValue4 = entity4.ItemValue;
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
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "PatrolType,PatrolTypeCode,AffirmState,AffirmUserId,District,PATROLDEPT,PATROLDATE,PATROLPERSON,PATROLPLACE,PROBLEMNUM,createuserid,createuserdeptcode,createuserorgcode,DutyUser,ByDept,Signature";
            pagination.p_tablename = "HRS_EVERYDAYPATROL";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = everydaypatrolbll.GetPageList(pagination, queryJson);
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
            var data = everydaypatrolbll.GetList(queryJson);
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
            var data = everydaypatrolbll.GetEntity(keyValue);
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
            everydaypatrolbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="postData">ʵ�����json</param>
        /// <param name="jsonData">��ϸjson</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string postData, string jsonData)
        {
            //everydaypatrolbll.SaveForm(keyValue, entity);
            //return Success("�����ɹ���");
            try
            {
                List<EverydayPatrolDetailEntity> projects = JsonConvert.DeserializeObject<List<EverydayPatrolDetailEntity>>(jsonData);
                EverydayPatrolEntity model = JsonConvert.DeserializeObject<EverydayPatrolEntity>(postData);
                if (projects == null)
                {
                    return Error("�������������Ϣ������Ϊnull");
                }
                var num = 0;
                if (projects.Count > 0)
                {
                    foreach (var item in projects)
                    {
                        if (item.Result == 1)
                        {
                            num = num + 1;
                        }
                        everydaypatroldetailbll.SaveForm(item.Id, item);//������ϸ
                    }
                    model.ProblemNum = num;
                    everydaypatrolbll.SaveForm(keyValue, model);//��������
                }
            }
            catch (Exception ex)
            {
                return Error("�������������Ϣ��" + ex.Message);
            }

            return Success("�ύ�ɹ���");
        }
        #endregion

       /// <summary>
       /// ��ѯ���Ÿ�����
       /// </summary>
       /// <param name="departmentid"></param>
       /// <returns></returns>
        [HttpGet]
        public ActionResult GetMajorUserId(string departmentid)
        {
            string majorUserId = everydaypatrolbll.GetMajorUserId(departmentid);
            return Content(majorUserId);
        }

        /// <summary>
        /// ���Ƹ���
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostFile(string postData, string keyValue)
        {
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            FileInfoBLL fileInfoBLL = new FileInfoBLL();

            var dt = JsonConvert.DeserializeObject<List<FileInfoEntity>>(postData);
            List<FileInfoEntity> projects = dt;
            string dir = string.Format("~/Resource/{0}/{1}", "ht/images", DateTime.Now.ToString("yyyyMMdd"));
            foreach (FileInfoEntity item in dt)
            {
                var filepath = Server.MapPath(item.FilePath);
                if (System.IO.File.Exists(filepath))
                {
                    string newFileName = Guid.NewGuid().ToString() + item.FileExtensions;
                    string newFilePath = dir + "/" + newFileName;
                    System.IO.File.Copy(filepath, Server.MapPath(newFilePath));
                }
                item.RecId = keyValue;
                item.FileId = Guid.NewGuid().ToString();
                fileInfoBLL.SaveForm("", item);
            }
            //fileInfoBLL.SaveForm("", fileInfoEntity);
            return Success("�����ɹ���");
        }

        #region ���ݵ���
        /// <summary>
        /// �����ճ�Ѳ��
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�����ճ�Ѳ���嵥")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "PatrolType,PatrolDept,PatrolPerson,District,to_char(PatrolDate,'yyyy-MM-dd hh24:mi') as PatrolDate,PROBLEMNUM,createuserid";
            pagination.p_tablename = "HRS_EVERYDAYPATROL";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = everydaypatrolbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�����ճ�Ѳ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�����ճ�Ѳ��.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "patroltype", ExcelColumn = "Ѳ������", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "patroldept", ExcelColumn = "Ѳ�鲿��", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "patrolperson", ExcelColumn = "Ѳ����", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "Ѳ������", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "patroldate", ExcelColumn = "Ѳ��ʱ��", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "problemnum", ExcelColumn = "������������", Alignment = "center" });

            excelconfig.ColumnEntity = listColumnEntity;
            //���õ�������
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //���õ�������
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, listColumnEntity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �����ճ�Ѳ��(��ϸ��Ϣ)
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�����ճ�Ѳ���¼")]
        public ActionResult Export2(string queryJson)
        {
            var data = everydaypatroldetailbll.GetList(queryJson).ToList();
            var affirmData = affirmrecordbll.GetList(queryJson).ToList();//ȷ�ϼ�¼

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
            string fileName = "����Ѳ���¼_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/�ճ�Ѳ���¼ģ��.doc");
            
            DataSet ds = new DataSet();
            DataTable dtPro = new DataTable("project");
            dtPro.Columns.Add("PatrolDate");
            dtPro.Columns.Add("PatrolDept");
            dtPro.Columns.Add("PatrolPlace");
            dtPro.Columns.Add("PatrolPerson");
            dtPro.Columns.Add("Date1");
            dtPro.Columns.Add("Date2");
            dtPro.Columns.Add("Date3");
            dtPro.Columns.Add("PatrolType");
            dtPro.Columns.Add("ByDept");
            dtPro.Columns.Add("DutyUser");
            dtPro.Columns.Add("Signature1");
            dtPro.Columns.Add("Signature2");
            dtPro.Columns.Add("Signature3");

            DataTable dt = new DataTable("list");
            dt.Columns.Add("no");
            dt.Columns.Add("PatrolContent");
            dt.Columns.Add("Result");
            dt.Columns.Add("Problem");
            dt.Columns.Add("Dispose");

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            DataRow row = dtPro.NewRow();

            string pic = Server.MapPath("~/content/Images/no_1.png");//Ĭ��ͼƬ
            string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["PatrolDate"].IsEmpty())
                {
                    row["PatrolDate"] = Convert.ToDateTime(queryParam["PatrolDate"]).ToString("yyyy-MM-dd");
                    row["Date1"] = Convert.ToDateTime(queryParam["PatrolDate"]).ToString("yyyy-MM-dd");
                }
                if (!queryParam["PatrolPerson"].IsEmpty())
                {
                    row["PatrolPerson"] = queryParam["PatrolPerson"].ToString();
                }
                if (!queryParam["PatrolPlace"].IsEmpty())
                {
                    row["PatrolPlace"] = queryParam["PatrolPlace"].ToString();
                }
                if (!queryParam["PatrolDept"].IsEmpty())
                {
                    row["PatrolDept"] = queryParam["PatrolDept"].ToString();
                }
                if (!queryParam["PatrolType"].IsEmpty())
                {
                    if (queryParam["PatrolType"].ToString() == "�¼�") {
                        strDocPath = Server.MapPath("~/Resource/ExcelTemplate/����Ѳ���¼ģ��.doc");
                    }
                    row["PatrolType"] = queryParam["PatrolType"].ToString();
                }
                if (!queryParam["DutyUser"].IsEmpty())
                {
                    row["DutyUser"] = queryParam["DutyUser"].ToString();
                }
                if (!queryParam["ByDept"].IsEmpty())
                {
                    row["ByDept"] = queryParam["ByDept"].ToString();
                }
                if (!queryParam["Signature"].IsEmpty())
                {
                    var filepath = "";
                    if ((queryParam["Signature"].ToString()).IndexOf("http") > -1)
                    {
                        filepath = (Server.MapPath("~/") + queryParam["Signature"].ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath = queryParam["Signature"] == null ? "" : (Server.MapPath("~/") + queryParam["Signature"].ToString().Replace("../../", "").ToString()).Replace("/", @"\").ToString();

                    }//string filepath = queryParam["Signature"].ToString();
                    if (System.IO.File.Exists(filepath))
                        row["Signature1"] = filepath;
                    else
                        row["Signature1"] = pic;
                }
            }
            if (affirmData != null && affirmData.Count > 0)
            {
                if (affirmData.Count == 1)
                {
                    var filepath = "";
                    if ((affirmData[0].Signature.ToString()).IndexOf("http") > -1)
                    {
                        filepath = (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath = affirmData[0].Signature == null ? "" : (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();

                    }
                    if (System.IO.File.Exists(filepath))
                        row["Signature2"] = filepath;
                    else
                        row["Signature2"] = pic;
                    
                    row["Date2"] = Convert.ToDateTime(affirmData[0].AffirmDate).ToString("yyyy-MM-dd");
                }
                if (affirmData.Count == 2)
                {
                    var filepath = "";
                    if ((affirmData[0].Signature.ToString()).IndexOf("http") > -1)
                    {
                        filepath = (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath = affirmData[0].Signature == null ? "" : (Server.MapPath("~/") + affirmData[0].Signature.ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();

                    }
                    if (System.IO.File.Exists(filepath))
                        row["Signature2"] = filepath;
                    else
                        row["Signature2"] = pic;

                    row["Date2"] = Convert.ToDateTime(affirmData[0].AffirmDate).ToString("yyyy-MM-dd");

                    var filepath1 = "";
                    if ((affirmData[1].Signature.ToString()).IndexOf("http") > -1)
                    {
                        filepath1 = (Server.MapPath("~/") + affirmData[1].Signature.ToString().Replace(webUrl, "").ToString()).Replace("/", @"\").ToString();
                    }
                    else
                    {
                        filepath1 = affirmData[1].Signature == null ? "" : (Server.MapPath("~/") + affirmData[1].Signature.ToString().Replace("../../", "").ToString()).Replace(@"\/", "\\").ToString();

                    }
                    if (System.IO.File.Exists(filepath1))
                        row["Signature3"] = filepath1;
                    else
                        row["Signature3"] = pic;
                    row["Date3"] = Convert.ToDateTime(affirmData[1].AffirmDate).ToString("yyyy-MM-dd");
                }
            }
            dtPro.Rows.Add(row);

            if (data.Count() > 0)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["no"] = (i + 1);
                    dtrow["PatrolContent"] = data[i].PatrolContent;
                    if (data[i].Result == 0)
                    {
                        dtrow["Result"] = data[i].ResultTrue;
                    }
                    else if (data[i].Result == 1)
                    {
                        dtrow["Result"] = data[i].ResultFalse;
                    }
                    else {
                        dtrow["Result"] = "";
                    }
                    dtrow["Problem"] = data[i].Problem;
                    dtrow["Dispose"] = data[i].Dispose;
                    dt.Rows.Add(dtrow);
                }
            }
            ds.Tables.Add(dt);

            ds.Tables.Add(dtPro);

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            doc.MailMerge.Execute(dtPro);
            doc.MailMerge.ExecuteWithRegions(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("�����ɹ�!");
        }
        #endregion
    }
}
