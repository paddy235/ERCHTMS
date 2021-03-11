using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.Busines.EvaluateManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Web;
using System;
using System.Data;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.BaseManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.EvaluateManage.Controllers
{
    /// <summary>
    /// �� �����Ϲ���������ϸ
    /// </summary>
    public class EvaluateDetailsController : MvcControllerBase
    {
        private EvaluateDetailsBLL evaluatedetailsbll = new EvaluateDetailsBLL();
        private StcategoryBLL StcategoryBLL = new StcategoryBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();
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
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = evaluatedetailsbll.GetPageList(pagination, queryJson);
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
            var data = evaluatedetailsbll.GetList(queryJson);
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
            var data = evaluatedetailsbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetCategoryListJson()
        {
            var data = StcategoryBLL.GetCategoryList();
            return Content(data.ToJson());
        }
        /// <summary>
        /// ��ȡС��
        /// </summary>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetRankListJson(string Category)
        {
            var data = StcategoryBLL.GetRankList(Category);
            return Content(data.ToJson());
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
            evaluatedetailsbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, EvaluateDetailsEntity entity)
        {
            evaluatedetailsbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ��������
        /// <summary>
        /// ����Ϲ�������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImportEvaluateDetails(string keyValue,string EvaluatePlanId)
        {
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾          
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            string orgid = OperatorProvider.Provider.Current().OrganizeId;
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
                if (dt.Rows.Count < 1)
                {
                    falseMessage += string.Format(@"����ʧ��,����д���ݣ�</br>", order);
                }
                else
                {
                    for (int i = 2; i < dt.Rows.Count; i++)
                    {
                        EvaluateDetailsEntity item = new EvaluateDetailsEntity();
                        order = i + 1;
                        item.MainId = keyValue;
                        item.EvaluatePlanId = EvaluatePlanId;
                        #region ����
                        string categoryname = dt.Rows[i][1].ToString();
                        if (!string.IsNullOrEmpty(categoryname))
                        {
                            item.CategoryName = categoryname;
                            JObject queryJson = new JObject();
                            queryJson.Add(new JProperty("name", categoryname));
                            var data = StcategoryBLL.GetQueryEntity(queryJson.ToString());
                            if (data != null)
                                item.Category = data.ID;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,������ϵͳ�в����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,�����Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region С��
                        string rankname = dt.Rows[i][2].ToString();
                        if (!string.IsNullOrEmpty(rankname))
                        {
                            item.RankName = rankname;
                            JObject queryJson = new JObject();
                            queryJson.Add(new JProperty("name", rankname));
                            queryJson.Add(new JProperty("parentid", item.Category));
                            var data = StcategoryBLL.GetQueryEntity(queryJson.ToString());
                            if (data != null)
                                item.Rank = data.ID;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,С����ϵͳ�в����ڻ��߲����ڴ��࣡</br>", order);
                                error++;
                                continue;
                            }
                        }
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,�����Ϊ�գ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region �ļ�����
                        string filename = dt.Rows[i][3].ToString();
                        if (!string.IsNullOrEmpty(filename))
                        {
                            item.FileName = filename;
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�ļ���ż����Ʋ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region �䲼����
                        string dutydept = dt.Rows[i][4].ToString();
                        if (!string.IsNullOrEmpty(dutydept))
                        {
                            item.DutyDept = dutydept;
                        }
                        #endregion

                        #region ʵʩ����
                        string putdate = dt.Rows[i][5].ToString();
                        if (!string.IsNullOrEmpty(putdate))
                        {
                            try
                            {
                                item.PutDate = DateTime.Parse(DateTime.Parse(putdate).ToString("yyyy-MM-dd"));
                            }
                            catch
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,ʵʩ���ڲ��ԣ�(��ȷʾ����2019-01-01)</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        #region ������ҵ��׼������
                        string normname = dt.Rows[i][6].ToString();
                        if (!string.IsNullOrEmpty(normname))
                        {
                            item.NormName = normname;
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,������ҵ��׼�����Ʋ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��������
                        string clause = dt.Rows[i][7].ToString();
                        if (!string.IsNullOrEmpty(clause))
                        {
                            item.Clause = clause;
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���������Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ���÷�Χ
                        string applyscope = dt.Rows[i][8].ToString();
                        if (!string.IsNullOrEmpty(applyscope))
                        {
                            item.ApplyScope = applyscope;
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���÷�Χ����Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ������
                        string isconform = dt.Rows[i][9].ToString();
                        if (!string.IsNullOrEmpty(isconform))
                        {
                            if (isconform == "����") item.IsConform = 0;
                            else if (isconform == "������") item.IsConform = 1;
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�����Բ����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����Բ���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region ��״������
                        string describe = dt.Rows[i][10].ToString();
                        if (!string.IsNullOrEmpty(describe))
                        {
                            item.Describe = describe;
                        }
                        #endregion

                        

                        #region �������
                        string opinion = dt.Rows[i][11].ToString();
                        if (!string.IsNullOrEmpty(opinion))
                        {
                            if (item.IsConform == 1)
                            {
                                item.Opinion = opinion;
                            }
                            else
                            {
                                item.Opinion = null;
                            }
                        }
                        else
                        {
                            if (item.IsConform == 1)
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�����������Ϊ�գ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        #region ���ļƻ����ʱ��
                        string FinishTime = dt.Rows[i][12].ToString();
                        DateTime tempFinishTime;
                        if (!string.IsNullOrEmpty(FinishTime))
                        {
                            if (item.IsConform == 1)
                            {
                                try
                                {
                                    item.FinishTime = DateTime.Parse(DateTime.Parse(FinishTime).ToString("yyyy-MM-dd"));
                                }
                                catch
                                {
                                    falseMessage += string.Format(@"��{0}�е���ʧ��,���Ľ�ֹʱ�䲻�ԣ�(��ȷʾ����2019-01-01)</br>", order);
                                    error++;
                                    continue;
                                }
                                //if (DateTime.TryParse(FinishTime, out tempFinishTime))
                                //    item.FinishTime = tempFinishTime;
                                //else
                                //{
                                //    falseMessage += string.Format(@"��{0}�е���ʧ��,���ļƻ����ʱ�䲻�ԣ�</br>", order);
                                //    error++;
                                //    continue;
                                //}
                            }
                            else
                            {
                                item.FinishTime = null;
                            }
                        }
                        else
                        {
                            if (item.IsConform == 1)
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,���ļƻ����ʱ�䲻��Ϊ�գ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        //������
                        //string evaluateperson = dt.Rows[i][13].ToString();
                        //if (!string.IsNullOrEmpty(evaluateperson))
                        //{
                        //    item.EvaluatePerson = evaluateperson;
                        //}
                        #region ������
                        string evaluateperson = dt.Rows[i][13].ToString();
                        if (!string.IsNullOrEmpty(evaluateperson))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == evaluateperson && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.EvaluatePersonId = userEntity.UserId;
                                item.EvaluatePerson = evaluateperson;
                            }
                            else
                            {
                                falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲����ڣ�</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����˲���Ϊ�գ�</br>", order);
                            error++;
                            continue;
                        }
                        #endregion
                        try
                        {
                            evaluatedetailsbll.SaveForm("", item);
                        }
                        catch
                        {
                            error++;
                        }

                    }
                }
                count = dt.Rows.Count;
                message = "����" + (count-2) + "����¼,�ɹ�����" + ((count - 2) - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
        /// <summary>
        /// ���Ƹ���
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostFile(string postData, string keyValue) {
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            FileInfoBLL fileInfoBLL = new FileInfoBLL();

            fileInfoBLL.DeleteFileByRecId(keyValue);//ɾ��ԭ�и���

            var dt = JsonConvert.DeserializeObject<List<FileInfoEntity>>(postData);
            List<FileInfoEntity> projects = dt;
            string dir = string.Format("~/Resource/{0}/{1}", "ht/images", DateTime.Now.ToString("yyyyMMdd"));
            if (Directory.Exists(Server.MapPath(dir)) == false)//��������ھʹ����ļ���
            {
                Directory.CreateDirectory(Server.MapPath(dir));
            }
            foreach (FileInfoEntity item in dt) {
                var filepath = Server.MapPath(item.FilePath);
                if (System.IO.File.Exists(filepath))
                {
                    string sufx = System.IO.Path.GetExtension(filepath);
                    string newFileName = Guid.NewGuid().ToString() + sufx;
                    string newFilePath = dir + "/" + newFileName;
                    System.IO.File.Copy(filepath, Server.MapPath(newFilePath));
                    item.FilePath = newFilePath;
                }
                item.RecId = keyValue;
                item.FileId = Guid.NewGuid().ToString();
                fileInfoBLL.SaveForm("", item);
            }
            fileInfoBLL.SaveForm("", fileInfoEntity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        //[HandlerMonitor(0, "����������ʩexcel")]
        public ActionResult ExportExcel(string condition, string queryJson)
        {
            Pagination pagination = new Pagination
            {
                page = 1,
                rows = 100000000,
                p_kid = "Id",
                p_fields = @"CategoryName,RankName,FileName,DutyDept,
to_char(PutDate,'yyyy-MM-dd') as PutDate,NormName,Clause,ApplyScope,
case when IsConform = '0' then '����' when IsConform = '1' then '������' else '' end as IsConform,
Describe,Opinion,to_char(FinishTime,'yyyy-MM-dd hh24:mi') as FinishTime,EvaluatePerson",
                p_tablename = "HRS_EVALUATEDETAILS",
                conditionJson = "1=1",
                sidx = "IsConform desc,CreateDate",
                sord = "desc"
            };
            DataTable data = evaluatedetailsbll.GetPageList(pagination, queryJson);


            //���õ�����ʽ
            //ExcelConfig excelconfig = new ExcelConfig
            //{
            //    Title = "�Ϲ�������",
            //    TitleFont = "����",
            //    TitleHeight = 30,
            //    TitlePoint = 25,
            //    FileName = "�Ϲ�������" + ".xls",
            //    IsAllSizeColumn = true,
            //    //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            //    ColumnEntity = new List<ColumnEntity>(){
            //    new ColumnEntity() {Column = "categoryname", ExcelColumn = "����", Alignment = "center"},
            //    new ColumnEntity() {Column = "rankname", ExcelColumn = "С��", Alignment = "center"},
            //    new ColumnEntity() {Column = "filename", ExcelColumn = "�ļ���ż�����", Alignment = "center"},
            //    new ColumnEntity() {Column = "dutydept", ExcelColumn = "�䲼����", Alignment = "center"},
            //    new ColumnEntity() {Column = "putdate", ExcelColumn = "ʵʩ����", Alignment = "center"},
            //    new ColumnEntity() {Column = "normname", ExcelColumn = "������ҵ��׼������", Alignment = "center"},
            //    new ColumnEntity() {Column = "clause", ExcelColumn = "��������", Alignment = "center"},
            //    new ColumnEntity() {Column = "applyscope", ExcelColumn = "���÷�Χ", Alignment = "center"},
            //    new ColumnEntity() {Column = "isconform", ExcelColumn = "������",  Alignment = "center"},
            //    new ColumnEntity() {Column = "describe", ExcelColumn = "��״����������", Alignment = "center"},
            //    new ColumnEntity() {Column = "opinion", ExcelColumn = "�������", Alignment = "center"},
            //    new ColumnEntity() {Column = "finishtime", ExcelColumn = "���Ľ�ֹʱ��", Alignment = "center"},
            //    new ColumnEntity() {Column = "evaluateperson", ExcelColumn = "������", Alignment = "center"}
            //    }
            //};

            ////���õ�������
            ////ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //return Success("�����ɹ���");

            for (int i = 0; i < data.Rows.Count; i++)
            {
                data.Rows[i][0] = i + 1;
                //data.Rows[i]["TimeType"] = data.Rows[i]["TimeNum"].ToString() + data.Rows[i]["TimeType"].ToString();
                
            }

            string FileUrl = @"\Resource\ExcelTemplate\�Ϲ������۵���.xls";
            AsposeExcelHelper.ExecuteResult(data, FileUrl, "�Ϲ�������", "�Ϲ�������");

            return Success("�����ɹ���");
        }
    }
}
