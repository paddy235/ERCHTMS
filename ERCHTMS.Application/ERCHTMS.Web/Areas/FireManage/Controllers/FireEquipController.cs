using ERCHTMS.Entity.FireManage;
using ERCHTMS.Busines.FireManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using Aspose.Cells;
using System.Drawing;
using System.Web;
using System.Data;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// �� �������������䱸
    /// </summary>
    public class FireEquipController : MvcControllerBase
    {
        private FireEquipBLL FireEquipbll = new FireEquipBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
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
            pagination.p_fields = "EquipmentName,Purpose,EquipOne,EquipUnitOne,EquipRatioOne,PracticalEquipOne,PracticalEquipUnitOne,RemarkOne,EquipTwo,EquipUnitTwo,EquipRatioTwo,PracticalEquipTwo,PracticalEquipUnitTwo,RemarkTwo,createusername,createdate,createuserorgcode,createuserdeptcode,createuserid";
            pagination.p_tablename = "HRS_FireEquip";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }
            var watch = CommonHelper.TimerStart();
            var data = FireEquipbll.GetPageList(pagination, queryJson);
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
            var data = FireEquipbll.GetList(queryJson);
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
            var data = FireEquipbll.GetEntity(keyValue);
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
            FireEquipbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FireEquipEntity entity)
        {
            FireEquipbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
        #region ���ݵ���
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������������װ���䱸��׼")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"equipmentname,purpose,
(Concat(equipone,equipunitone)) as equipone,
equipratioone,
 (Concat(practicalequipone,practicalequipunitone)) as practicalequipone,
(Concat(equiptwo,equipunittwo)) as equiptwo,
equipratiotwo,
(Concat(practicalequiptwo,practicalequipunittwo)) as practicalequiptwo";
            pagination.p_tablename = "HRS_FIREEQUIP t";
            pagination.conditionJson = string.Format(" 1=1 ");
            pagination.sidx = "createdate";//�����ֶ�
            pagination.sord = "desc";//����ʽ  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var exportTable = FireEquipbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            //ExcelConfig excelconfig = new ExcelConfig();
            //excelconfig.Title = "��������װ���䱸��׼";
            //excelconfig.TitleFont = "΢���ź�";
            //excelconfig.TitlePoint = 16;
            //excelconfig.FileName = "��������װ���䱸��׼.xls";
            //excelconfig.IsAllSizeColumn = true;
            ////ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            //List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            //excelconfig.ColumnEntity = listColumnEntity;
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "����", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "purpose", ExcelColumn = "��Ҫ��;", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipone", ExcelColumn = "һ��վ�䱸", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipratioone", ExcelColumn = "һ��վ���ݱ�", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "practicalequipone", ExcelColumn = "һ��վʵ���䱸����", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equiptwo", ExcelColumn = "����վ�䱸", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipratiotwo", ExcelColumn = "����վ���ݱ�", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "practicalequiptwo", ExcelColumn = "����վʵ���䱸����", Alignment = "center" });

            ////���õ�������
            //ExcelHelper.ExcelDownload(data, excelconfig);
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            string fName = "��������װ���䱸��׼_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
            var num = wb.Worksheets[0].Cells.Columns.Count;

            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
            Aspose.Cells.Cell cell = sheet.Cells[0, 0];
            cell.PutValue("��������װ���䱸��׼"); //����
            cell.Style.Pattern = BackgroundType.Solid;
            cell.Style.Font.Size = 16;
            cell.Style.Font.Color = Color.Black;
            List<string> colList = new List<string>() {  "����", "��Ҫ��;", "һ��վ�䱸", "һ��վ���ݱ�", "һ��վʵ���䱸����", "����վ�䱸", "����վ���ݱ�", "����վʵ���䱸����" };
            List<string> colList1 = new List<string>() { "equipmentname", "purpose", "equipone", "equipratioone", "practicalequipone", "equiptwo", "equipratiotwo", "practicalequiptwo" };
            for (int i = 0; i < colList.Count; i++)
            {
                //�����
                Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                serialcell.PutValue(" ");

                for (int j = 0; j < colList.Count; j++)
                {
                    Aspose.Cells.Cell curcell = sheet.Cells[1, j];
                    //sheet.Cells.SetColumnWidth(j, 40);
                    curcell.Style.Pattern = BackgroundType.Solid;
                    curcell.Style.Font.Size = 12;
                    curcell.Style.Font.Color = Color.Black;
                    curcell.PutValue(colList[j].ToString()); //��ͷ
                }
                Aspose.Cells.Cells cells = sheet.Cells;
                cells.Merge(0, 0, 1, colList.Count);
            }
            for (int i = 0; i < exportTable.Rows.Count; i++)
            {
                //�������
                for (int j = 0; j < colList1.Count; j++)
                {
                    Aspose.Cells.Cell curcell = sheet.Cells[i + 2, j];
                    curcell.PutValue(exportTable.Rows[i][colList1[j]].ToString());
                }

            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

            return Success("�����ɹ���");
        }

        #endregion
    }
}
