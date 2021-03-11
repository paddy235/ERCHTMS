using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.SafePunish;
using ERCHTMS.Busines.SafePunish;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Code;
using Newtonsoft.Json;
using Aspose.Words;
using System.Web;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.SafePunish.Controllers
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    public class SafepunishController : MvcControllerBase
    {
        private SafepunishBLL safepunishbll = new SafepunishBLL();
        private SafekpidataBLL safekpidatabll = new SafekpidataBLL();
        private SafepunishdetailBLL safepunishdetailbll = new SafepunishdetailBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PunishStandard()
        {
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }


        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PunishStatistics()
        {
            return View();
        }

        /// <summary>
        /// ������ϸҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PunishDetail()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
           // Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "Id";
            pagination.p_fields = @"CreateUserId,CreateDate,CreateUserName,ModifyUserId,ModifyDate,ModifyUserName,CreateUserDeptCode,CreateUserOrgCode,FlowState,ApplyUserId,ApproverPeopleIds,
            case when ApplyState= 0 then '������'
               when (amercetype='1' or amercetype='2') and ApplyState= 1 then '�������' 
               when (amercetype='1' or amercetype='2') and ApplyState= 2 then '�ֹ��쵼���' 
                when (amercetype='3' or amercetype='4') and ApplyState= 1 then 'רҵ���' 
                when (amercetype='3' or amercetype='4') and ApplyState= 2 then '�������'              
                        when ApplyState= 3 then '�����' end as  ApplyState,SafePunishCode,ApplyTime,PunishObjectNames,PunishType,PunishRemark,amercetype,ApplyUserName";
            pagination.p_tablename = "BIS_SAFEPUNISH";
            pagination.sidx = "CreateDate";//�����ֶ�
            pagination.sord = "desc";//����ʽ
            //pagination.conditionJson = "instr(ApproverPeopleIds,'"+user.UserId+"' )> 0";
            pagination.conditionJson = "1=1";
            var data = safepunishbll.GetPageList(pagination, queryJson);
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
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            SafepunishEntity data = safepunishbll.GetEntity(keyValue);
            if (data.AmerceType == "2")
            {
                data.PunishType = "";
            }
            var kpidata = safekpidatabll.GetList("").Where(p => p.SafePunishId == keyValue).FirstOrDefault();
            object obj = new
            {
                punishdata = data,
                kpidata = kpidata
            };
            return ToJsonResult(obj);
        }



        /// <summary>
        ///��ȡͳ������
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpGet]
        public string GetPunishStatisticsCount(string year,  string statMode)
        {
            return safepunishbll.GetPunishStatisticsCount(year, statMode);
        }


        /// <summary>
        ///��ȡͳ������(�б�)
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpPost]
        public string GetPunishStatisticsList(string year,  string statMode)
        {
            return safepunishbll.GetPunishStatisticsList(year, statMode);
        }

        /// <summary>
        /// ������ȫ�ͷ�ͳ��
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ȫ�ͷ�ͳ��")]
        public ActionResult ExportStatisticsExcel(string queryJson)
        {
            string jsonList = "";
            string mode = "";
            if (!string.IsNullOrEmpty(queryJson) && queryJson != "\"\"")
            {
                var queryParam = queryJson.ToJObject();
                mode = queryParam["statMode"].ToString();
                jsonList = safepunishbll.GetPunishStatisticsList(queryParam["year"].ToString(), queryParam["statMode"].ToString());
            }


            if (jsonList != "")
            {
                dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
                DataTable tb = JsonToDataTable(dyObj.rows.ToString());

                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ȫ�ͷ�";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 25;
                if (mode == "0")
                {
                    excelconfig.FileName = "��ȫ�ͷ�����";
                }
                else if (mode == "1")
                {
                    excelconfig.FileName = "��ȫ�ͷ����";
                }
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                excelconfig.ColumnEntity = new List<ColumnEntity>()
                {
                    new ColumnEntity() {Column = "TypeName", ExcelColumn = "�¼�����", Alignment = "center"},
                    new ColumnEntity() {Column = "num1", ExcelColumn = "1��", Alignment = "center"},
                    new ColumnEntity() {Column = "num2", ExcelColumn = "2��", Alignment = "center"},
                    new ColumnEntity() {Column = "num3", ExcelColumn = "3��", Alignment = "center"},
                    new ColumnEntity() {Column = "num4", ExcelColumn = "4��", Alignment = "center"},
                    new ColumnEntity() {Column = "num5", ExcelColumn = "5��", Alignment = "center"},
                    new ColumnEntity() {Column = "num6", ExcelColumn = "6��", Alignment = "center"},
                    new ColumnEntity() {Column = "num7", ExcelColumn = "7��", Alignment = "center"},
                    new ColumnEntity() {Column = "num8", ExcelColumn = "8��", Alignment = "center"},
                    new ColumnEntity() {Column = "num9", ExcelColumn = "9��", Alignment = "center"},
                    new ColumnEntity() {Column = "num10", ExcelColumn = "10��", Alignment = "center"},
                    new ColumnEntity() {Column = "num11", ExcelColumn = "11��", Alignment = "center"},
                    new ColumnEntity() {Column = "num12", ExcelColumn = "12��", Alignment = "center"}
                };
                if (mode == "0")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Total", ExcelColumn = "�ܼ�(��)", Alignment = "center" });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Total", ExcelColumn = "�ܼ�(Ԫ)", Alignment = "center" });
                }

                //���õ�������
                ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
                return Success("�����ɹ���");
            }
            else
            {
                return Success("δ��ѯ�����ݡ�");
            }

        }
        

        /// <summary>
        /// ������ȫ�ͷ�excel
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ȫ�ͷ�excel")]
        public ActionResult ExportSafePunishExcel(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = @"case when ApplyState= 0 then '������'
            when (amercetype='1' or amercetype='2') and ApplyState= 1 then '�������' 
               when (amercetype='1' or amercetype='2') and ApplyState= 2 then '�ֹ��쵼���' 
                when (amercetype='3' or amercetype='4') and ApplyState= 1 then 'רҵ���' 
                when (amercetype='3' or amercetype='4') and ApplyState= 2 then '�������'                   
            when ApplyState= 3 then '�����' end as  applystate,case when amercetype=1 then '�¹��¼�' when amercetype=2 then '����' when  amercetype=3 then '�����Ų�����' when amercetype=4 then '�ճ�����' end amercetype,safepunishcode,punishobjectnames,to_char(applytime,'yyyy-MM-dd HH24:mi:ss') applytime,punishremark ";
            pagination.p_tablename = "bis_safepunish a";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CreateDate";
            pagination.sord = "desc";
            var data = safepunishbll.GetPageList(pagination, queryJson);


            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "��ȫ�ͷ�";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "��ȫ�ͷ�" + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            excelconfig.ColumnEntity = new List<ColumnEntity>()
            {
                new ColumnEntity() {Column = "applystate", ExcelColumn = "����״̬", Alignment = "center"},
                new ColumnEntity() {Column = "amercetype", ExcelColumn = "��������", Alignment = "center"},
                new ColumnEntity() {Column = "safepunishcode", ExcelColumn = "���˱��", Alignment = "center"},
                new ColumnEntity() {Column = "applytime", ExcelColumn = "����ʱ��", Alignment = "center"},
                new ColumnEntity() {Column = "punishremark", ExcelColumn = "���˾�������", Alignment = "center"}
            };

            //���õ�������
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ������ȫ�ͷ���ϸ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportSafePunishInfo(string keyValue)
        {

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������

            string fileName = "��ȫ�ͷ�������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\��ȫ���˵���ģ��.docx";
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("PunishCode"); //���
            dt.Columns.Add("CreateDept"); //����רҵ(����)
            dt.Columns.Add("ApplyTime"); //����ʱ��
            dt.Columns.Add("BelongDept"); //�����˲���
            dt.Columns.Add("PunishNum"); //���˽��
            dt.Columns.Add("PunishRemark"); //�¼�����
            dt.Columns.Add("filed1"); //��һ����������
            dt.Columns.Add("filed2"); //�ڶ�����������
            dt.Columns.Add("approve1");//��һ��������
            dt.Columns.Add("approvename1");//�ڶ���������
            dt.Columns.Add("approvetime1");//��һ��������
            dt.Columns.Add("approve2");//�ڶ���������
            dt.Columns.Add("approvename2");//��һ��������
            dt.Columns.Add("approvetime2");//�ڶ���������
            DataRow row = dt.NewRow();


            //��ȫ������Ϣ
            SafepunishEntity safepunishentity = safepunishbll.GetEntity(keyValue);
            row["PunishCode"] = safepunishentity.SafePunishCode;
            row["PunishRemark"] = safepunishentity.PunishRemark;
            row["CreateDept"] = safepunishentity.BelongDept;
            row["ApplyTime"] = safepunishentity.ApplyTime.IsEmpty() ? "" : Convert.ToDateTime(safepunishentity.ApplyTime).ToString("yyyy-MM-dd");
            if (safepunishentity.AmerceType == "1" || safepunishentity.AmerceType == "2")
            {
                row["filed1"] = "�������";
                row["filed2"] = "�ֹ��쵼���";
            }
            else if (safepunishentity.AmerceType == "3" || safepunishentity.AmerceType == "4")
            {
                row["filed1"] = "רҵ���";
                row["filed2"] = "�������";
            }

            var flist = fileinfobll.GetImageListByRecid(keyValue);
            builder.MoveToMergeField("PunishImage");
            foreach (FileInfoEntity fmode in flist)
            {
                string path = "";
                if (string.IsNullOrWhiteSpace(fmode.FilePath))
                {
                    path = Server.MapPath("~/content/Images/no_1.png");
                }
                else
                {
                    var filepath = Server.MapPath("~/") + fmode.FilePath.Replace("~/", "").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        path = filepath;
                    }
                    else
                    {
                        path = Server.MapPath("~/content/Images/no_1.png");
                    }
                }
                builder.MoveToMergeField("RewardImage");
                builder.InsertImage(path, 200, 160);
            }


            row["approve1"] = safepunishentity.SpecialtyOpinion;
            row["approvetime1"] = safepunishentity.CreateDate.IsEmpty() ? "" : Convert.ToDateTime(safepunishentity.CreateDate).ToString("yyyy-MM-dd");
            UserEntity createuser = new UserBLL().GetEntity(safepunishentity.CreateUserId);
            if (createuser.SignImg.IsEmpty())
            {
                row["approvename1"] = Server.MapPath("~/content/Images/no_1.png");
            }
            else
            {
                var filepath = Server.MapPath("~/") + createuser.SignImg.ToString().Replace("../../", "").ToString();
                if (System.IO.File.Exists(filepath))
                {
                    row["approvename1"] = filepath;
                }
                else
                {
                    row["approvename1"] = Server.MapPath("~/content/Images/no_1.png");
                }
            }
            builder.MoveToMergeField("approvename1");
            builder.InsertImage(row["approvename1"].ToString(), 80, 35);

            //��ȡ�����˶���
            SafepunishdetailEntity safepunishdetailentity = safepunishdetailbll.GetListByPunishId(keyValue, "0").OrderBy(t => t.CreateDate).FirstOrDefault();
            row["BelongDept"] = departmentbll.GetEntity(safepunishdetailentity.BelongDept) == null ? "" : departmentbll.GetEntity(safepunishdetailentity.BelongDept).FullName;
            row["PunishNum"] = safepunishdetailentity.PunishNum;
            DataTable dtAptitude = safepunishbll.GetAptitudeInfo(keyValue);
            for (int i = dtAptitude.Rows.Count -1; i > 0; i--)
            {
                if (i == (dtAptitude.Rows.Count - 2))
                {
                    row["approve2"] = dtAptitude.Rows[i]["auditremark"];
                    row["approvetime2"] = dtAptitude.Rows[i]["auditdate"].IsEmpty() ? "" : Convert.ToDateTime(dtAptitude.Rows[i]["auditdate"]).ToString("yyyy-MM-dd");
                    if (dtAptitude.Rows[i]["auditsignimg"].IsEmpty())
                    {
                        row["approvename2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + dtAptitude.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["approvename2"] = filepath;
                        }
                        else
                        {
                            row["approvename2"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    builder.MoveToMergeField("approvename2");
                    builder.InsertImage(row["approvename2"].ToString(), 80, 35);
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("�����ɹ�!");
        }

        #region ��ȡ��ȫ�ͷ�����ͼ����
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetWorkActionList(string keyValue)
        {
            var josnData = safepunishbll.GetFlow(keyValue);
            return Content(josnData.ToJson());
        }

        #endregion



        /// <summary>
        /// ����ҵ��id��ȡ��Ӧ����˼�¼�б� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetSpecialAuditList(string keyValue)
        {
            var data = new AptitudeinvestigateauditBLL().GetAuditList(keyValue).Where(p => p.REMARK != "0").ToList();
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetPunishCode()
        {
            var data = safepunishbll.GetPunishCode();
            return ToJsonResult(data);
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetPunishNum()
        {
            var data = safepunishbll.GetPunishNum();
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
            try
            {
                safepunishbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafepunishEntity entity, SafekpidataEntity kpiEntity, [System.Web.Http.FromBody]string punishJson, [System.Web.Http.FromBody]string relatedpunishJson)
        {
            var year = DateTime.Now.ToString("yyyy");
            var month = DateTime.Now.ToString("MM");
            var day = DateTime.Now.ToString("dd");
            var punishCode = "Q/CRPHZHB 2208.06.01-JL02-" + year + month + day + safepunishbll.GetPunishCode();
            entity.SafePunishCode = !string.IsNullOrEmpty(entity.SafePunishCode) ? entity.SafePunishCode : punishCode;
            safepunishbll.SaveForm(keyValue, entity, kpiEntity);
            if (entity.AmerceType =="2" || entity.AmerceType=="3" || entity.AmerceType=="4")
            {
                if (punishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue,"0") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(punishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }

                if (relatedpunishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue, "1") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(relatedpunishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }
            }
            return Success("�����ɹ���");
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
        public ActionResult CommitApply(string keyValue, AptitudeinvestigateauditEntity entity,SafepunishEntity punEntity, SafekpidataEntity kpiEntity, [System.Web.Http.FromBody]string punishJson, [System.Web.Http.FromBody]string relatedpunishJson)
        {
            if (punEntity != null && (string.IsNullOrEmpty(punEntity.ApplyState) || punEntity.ApplyState == "0"))
            {
                var year = DateTime.Now.ToString("yyyy");
                var month = DateTime.Now.ToString("MM");
                var day = DateTime.Now.ToString("dd");
                var punishCode = "Q/CRPHZHB 2208.06.01-JL02-" + year + month + day + safepunishbll.GetPunishCode();
                punEntity.SafePunishCode = !string.IsNullOrEmpty(punEntity.SafePunishCode) ? punEntity.SafePunishCode : punishCode;
                safepunishbll.SaveForm(keyValue, punEntity, kpiEntity);
                if (punishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue, "0") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(punishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }

                if (relatedpunishJson.Length > 0)
                {
                    if (safepunishdetailbll.Remove(keyValue, "1") > 0)
                    {
                        List<SafepunishdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SafepunishdetailEntity>>(relatedpunishJson);
                        foreach (SafepunishdetailEntity data in list)
                        {
                            safepunishdetailbll.SaveForm("", data);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(keyValue) && entity != null)
            {
                safepunishbll.CommitApply(keyValue, entity);
            }

            return Success("�����ɹ���");
        }
        #endregion



        #region Json �ַ��� ת��Ϊ DataTable���ݼ���
        /// <summary>
        /// Json �ַ��� ת��Ϊ DataTable���ݼ��� ��ʽ[{"xxx":"yyy","x1":"yy2"},{"x2":"y2","x3":"y4"}]
        /// </summary>  
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //ʵ����
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //ȡ�������ֵ
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //ѭ������е�DataTable��
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion
    }
}
