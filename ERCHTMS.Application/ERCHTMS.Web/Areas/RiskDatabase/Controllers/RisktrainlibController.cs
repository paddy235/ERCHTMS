using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Web;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ����
    /// </summary>
    public class RisktrainlibController : MvcControllerBase
    {
        private RisktrainlibBLL risktrainlibbll = new RisktrainlibBLL();
        private RisktrainlibdetailBLL risktrainlibdetailbll = new RisktrainlibdetailBLL();
        private DistrictBLL districtBLL = new DistrictBLL();
        private DangerSourceBLL dangerBLL = new DangerSourceBLL();

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
        public ActionResult Import()
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
        [HttpGet]
        public ActionResult SelectTrianLib()
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
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "bis_risktrainlib";
            pagination.p_kid = "id";
            pagination.p_fields = @"worktask,worktype,workpost,workarea,workareaid,workdes,usernum,modifynum,
resources,createusername,createdate,createuserdeptcode,createuserorgcode,createuserid,worktypecode,risklevel,risklevelval";
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = risktrainlibbll.GetPageListJson(pagination, queryJson);
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
        [HttpGet]
        public ActionResult GetPageJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_tablename = "bis_risktrainlib";
            pagination.p_kid = "id";
            pagination.p_fields = @"worktask,worktype,workpost,workarea,workareaid,workdes,usernum,modifynum,resources,createusername,createdate,createuserdeptcode,createuserorgcode,createuserid,risklevelval";
            pagination.conditionJson += "  createuserorgcode=" + user.OrganizeCode;
            var data = risktrainlibbll.GetPageListJson(pagination, queryJson);
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
            var data = risktrainlibbll.GetList(queryJson);
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
            var data = risktrainlibbll.GetEntity(keyValue);
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
            risktrainlibbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ɾ����Դ���տ�����
        /// </summary>
        /// <param name="keyValue">����</param>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DelRiskData()
        {
            if (risktrainlibbll.DelRiskData())
            {
                return Success("ɾ���ɹ���");
            }
            else
            {
                return Error("ɾ��ʧ�ܡ�");
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
        public ActionResult SaveForm(string keyValue, RisktrainlibEntity entity, string measuresJson)
        {
            List<RisktrainlibdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RisktrainlibdetailEntity>>(measuresJson);
            entity.DataSources = "3";//�����޸Ļ���������������Դͳһ�޸�Ϊ3
            risktrainlibbll.SaveForm(keyValue, entity, list);
            return Success("�����ɹ���");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "�������ѵ����")]
        public string ImportTrainLib()
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "��������Ա�޴˲���Ȩ��";
                }
                string orgId = OperatorProvider.Provider.Current().OrganizeId;
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
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName),file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")?Aspose.Cells.FileFormatType.Excel2007Xlsx:Aspose.Cells.FileFormatType.Excel2003);
                    var sheet = wb.Worksheets[0];
                    if (sheet.Cells[1, 1].StringValue != "��������" || sheet.Cells[1, 2].StringValue != "���յȼ�" || sheet.Cells[1, 3].StringValue != "��ҵ����"
                        || sheet.Cells[1, 4].StringValue != "��ҵ��λ" || sheet.Cells[1, 5].StringValue != "��������" || sheet.Cells[1, 6].StringValue != "��Դ׼��"
                        || sheet.Cells[1, 7].StringValue != "��ҵ����" || sheet.Cells[1, 8].StringValue != "����" || sheet.Cells[1, 9].StringValue != "Ǳ��Σ��" || sheet.Cells[1, 10].StringValue != "������ʩ")
                    {
                        return message;
                    }
                    var RiskLibList = new List<RisktrainlibEntity>();
                    var RiskDetailList = new List<RisktrainlibdetailEntity>();
                    var date = DateTime.Now;
                    for (int i = 2; i <= sheet.Cells.MaxDataRow; i++)
                    {
                        var entity = new RisktrainlibEntity();
                        entity.Create();
                        entity.WorkTask = sheet.Cells[i, 1].StringValue;
                        entity.RiskLevel = sheet.Cells[i, 2].StringValue;
                        switch (entity.RiskLevel)
                        {
                            case "�ش����":
                                entity.RiskLevelVal = "1";
                                break;
                            case "�ϴ����":
                                entity.RiskLevelVal = "2";
                                break;
                            case "һ�����":
                                entity.RiskLevelVal = "3";
                                break;
                            case "�ͷ���":
                                entity.RiskLevelVal = "4";
                                break;
                            default:
                                break;
                        }
                        entity.WorkType = sheet.Cells[i, 3].StringValue;
                        entity.WorkPost = sheet.Cells[i, 4].StringValue;
                        entity.WorkDes = sheet.Cells[i, 5].StringValue;
                        entity.Resources = sheet.Cells[i, 6].StringValue;
                        entity.WorkArea = sheet.Cells[i, 7].StringValue;
                        entity.DataSources = "2";
                        if (string.IsNullOrEmpty(sheet.Cells[i, 1].StringValue))
                        {
                            entity.ID = RiskLibList[i - 1 - 2].ID;
                        }
                        RiskLibList.Add(entity);
                    }

                    for (int i = 2; i <= sheet.Cells.MaxDataRow; i++)
                    {
                        var dentity = new RisktrainlibdetailEntity();
                        if (sheet.Cells[i, 8].StringValue.Length > 1000)
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "�й����ַ����ȳ���,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        if (sheet.Cells[i, 9].StringValue.Length > 1000)
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "�д��ڷ����ַ����ȳ���,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        if (sheet.Cells[i, 10].StringValue.Length > 1000)
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "�й����ʩ�ַ����ȳ���,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        dentity.Process = sheet.Cells[i, 8].StringValue;
                        dentity.AtRisk = sheet.Cells[i, 9].StringValue;
                        dentity.Controls = sheet.Cells[i, 10].StringValue;
                        dentity.Create();

                        dentity.WorkId = RiskLibList[i - 2].ID;

                        RiskDetailList.Add(dentity);
                    }
                    RiskLibList = RiskLibList.Where(x => x.WorkTask != "").ToList();
                    for (int i = 0; i < RiskLibList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(RiskLibList[i].WorkType))
                        {
                            var data = new DataItemDetailBLL().GetDataItemListByItemCode("'StatisticsType'");
                            string worktype = string.Empty;
                            var list = RiskLibList[i].WorkType.Split(',');
                            for (int k = 0; k < list.Length; k++)
                            {
                                var entity = data.Where(x => x.ItemName == list[k].Trim()).FirstOrDefault();
                                if (entity != null)
                                {
                                    if (string.IsNullOrWhiteSpace(worktype))
                                    {

                                        RiskLibList[i].WorkTypeCode += entity.ItemValue + ",";
                                        worktype += entity.ItemName + ",";

                                    }
                                    else
                                    {
                                        if (!worktype.Contains(entity.ItemName))
                                        {
                                            RiskLibList[i].WorkTypeCode += entity.ItemValue + ",";
                                            worktype += entity.ItemName + ",";
                                        }
                                    }
                                }
                                //if (entity != null)
                                //{
                                //    RiskLibList[i].WorkTypeCode += entity.ItemValue + ",";
                                //}
                                //else
                                //{
                                //    RiskLibList[i].WorkType.Replace(list[k], "");
                                //}
                            }
                            if (!string.IsNullOrWhiteSpace(RiskLibList[i].WorkTypeCode))
                            {
                                RiskLibList[i].WorkTypeCode = RiskLibList[i].WorkTypeCode.Substring(0, RiskLibList[i].WorkTypeCode.Length - 1);
                                RiskLibList[i].WorkType = worktype.Substring(0, worktype.Length - 1);
                            }
                        }
                        if (string.IsNullOrWhiteSpace(RiskLibList[i].WorkArea))
                        {
                            //falseMessage += "</br>" + "��" + (i+1) + "������Ϊ��,δ�ܵ���.";
                            //error++;
                            //continue;
                        }
                        else
                        {
                            DistrictEntity disEntity = districtBLL.GetDistrict(orgId, RiskLibList[i].WorkArea);
                            if (disEntity == null)
                            {
                                //�糧û�и������򲻸�ֵ
                                RiskLibList[i].WorkArea = "";
                            }
                            else
                            {
                                RiskLibList[i].WorkAreaId = disEntity.DistrictID;
                            }
                        }
                    }
                    risktrainlibbll.InsertImportData(RiskLibList, RiskDetailList);
                    //risktrainlibbll.InsertRiskTrainLib(RiskLibList);
                    //risktrainlibdetailbll.InsertRiskTrainDetailLib(RiskDetailList);
                    count = RiskDetailList.Count;
                    message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                    message += "</br>" + falseMessage;
                    //DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                }
                return message;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            //return null;
        }
        /// <summary>
        /// Ԥ֪����ѵ���⵼������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson, string fileName)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            //string path = "~/Resource/Temp";
            string fName = "��ҵ��ȫ������" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/Σ��Ԥ֪ѵ�����ݿ⵼��ģ��.xlsx"));
            var queryParam = queryJson.ToJObject();
            //var riskType = queryParam["riskType"].ToString();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 10000000;
            pagination.sidx = "t.createdate,t.id";
            pagination.sord = "desc";
            pagination.p_tablename = "bis_risktrainlib t left join bis_risktrainlibdetail d on d.workid = t.id";
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.worktask,t.risklevel,t.worktype,t.workpost,t.workdes,
t.resources,t.workarea,d.process,d.atrisk,d.controls";
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = risktrainlibbll.GetPageListJson(pagination, queryJson);
            var cells = wb.Worksheets[0].Cells;
            int Colnum = data.Columns.Count;
            int Rownum = data.Rows.Count;
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum - 1; k++)
                {
                    if (k == 0)
                    {
                        cells[2 + i, k].PutValue(i + 1);
                    }
                    else
                    {
                        cells[2 + i, k].PutValue(data.Rows[i][k].ToString());
                    }
                }
            }
            int q = 0;
            int RowOrder = 0;
            int m = 1;
            for (int i = 0; i < data.Rows.Count; i = q)
            {
                RowOrder = data.Select(string.Format("id='{0}'", data.Rows[i]["id"].ToString())).ToList().Count;
                cells.Merge(2 + q, 0, RowOrder, 1);
                cells.Merge(2 + q, 1, RowOrder, 1);
                cells.Merge(2 + q, 2, RowOrder, 1);
                cells.Merge(2 + q, 3, RowOrder, 1);
                cells.Merge(2 + q, 4, RowOrder, 1);
                cells.Merge(2 + q, 5, RowOrder, 1);
                cells.Merge(2 + q, 6, RowOrder, 1);
                cells.Merge(2 + q, 7, RowOrder, 1);
                cells[2 + q, 0].PutValue(m);
                m++;
                q += RowOrder;
            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            System.Threading.Thread.Sleep(400);
            wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            return Success("�����ɹ���", fName);
        }

        #endregion
    }
}
