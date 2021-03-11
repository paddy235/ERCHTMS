using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using System.Collections.Generic;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Aspose.Cells;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.ToolEquipmentManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.ToolEquipmentManage;
using Newtonsoft.Json;
using ServiceStack.Common.Web;
using ThoughtWorks.QRCode.Codec;

namespace ERCHTMS.Web.Areas.ToolEquipmentManage.Controllers
{
    /// <summary>
    /// �� ���������߻�����Ϣ
    /// </summary>
    public class ToolequipmentController : MvcControllerBase
    {
        private ToolequipmentBLL toolequipmentBll = new ToolequipmentBLL();
        private DataItemDetailBLL dataitemdetailBll = new DataItemDetailBLL();
        private ToolrecordBLL toolrecordbll = new ToolrecordBLL();
        private ERCHTMS.Busines.PublicInfoManage.FileInfoBLL fileInfoBLL = new ERCHTMS.Busines.PublicInfoManage.FileInfoBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
        public ActionResult AddRecordForm()
        {
            return View();
        }

        /// <summary>
        /// ��ȫ�����߱�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SafeTool()
        {
            return View();
        }

        /// <summary>
        /// �ֹ����߱�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SgTool()
        {
            return View();
        }

        /// <summary>
        /// ��ȫ������ͳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ToolsStatistics()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BuilderImage()
        {
            return View();
        }

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
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,b.Appraise,b.operuser,Equipmenttype,EquipmentName,EquipmentValue,EquipmentNo,Specifications,outputdeptname,checkdate,NextCheckDate,factorydate,district,districtid,districtcode,depositary,controluserid,controlusername,acceptance,ControlDept,ControlDeptId,ControlDeptCode,belongdept,belongdeptid,belongdeptCode,unit,quantity";
            pagination.p_tablename = @"BIS_TOOLEQUIPMENT a left join (select appraise,tb1.toolequipmentid,operuser from BIS_TOOLRECORD tb1 ,(select  max(createdate) createdate,toolequipmentid from BIS_TOOLRECORD group by toolequipmentid) tb2 
             where tb1.createdate= tb2.createdate  and tb1.toolequipmentid= tb2.toolequipmentid) b on a.id = b.toolequipmentid";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }


            var watch = CommonHelper.TimerStart();
            var data = toolequipmentBll.GetPageList(pagination, queryJson);
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
            var data = toolequipmentBll.GetList(queryJson);
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
            var data = toolequipmentBll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�豸���
        /// </summary>
        /// <param name="EquipmentNo">�豸���</param>
        /// <param name="orgcode">��������</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            return ToJsonResult(toolequipmentBll.GetEquipmentNo(EquipmentNo, orgcode));
        }
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFiles(string keyValue)
        {
            var data = fileInfoBLL.GetFiles(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�豸���ͳ��ͼ
        /// </summary>
        /// <returns></returns>
        public string GetEquipmentTypeStat(string queryJson)
        {
            return toolequipmentBll.GetEquipmentTypeStat(queryJson);
        }

        /// <summary>
        /// ��ȡ�豸����б�
        /// </summary>
        /// <returns></returns>
        public ActionResult GetToolStatisticsList(string queryJson)
        {
            var data = toolequipmentBll.GetToolStatisticsList(queryJson);
            return ToJsonResult(data);
        }



        /// <summary>
        /// ��ȡ�綯�����߼���¼
        /// </summary>
        /// <returns></returns>
        public ActionResult GetToolRecordList(Pagination pagination, string queryJson)
        {
            //var data = toolequipmentBll.GetToolRecordList(keyValue);
            //return ToJsonResult(data);
            pagination.p_kid = "id";
            pagination.p_fields = @"toolequipmentid,equipmentname,equipmentno,voltagelevel,trialvoltage,checkdate,nextcheckdate,appraise,operuser,specification,checkproject";
            pagination.p_tablename = "BIS_TOOLRECORD";
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }

            var watch = CommonHelper.TimerStart();
            var data = toolrecordbll.GetPageList(pagination, queryJson);
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
            toolequipmentBll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ToolequipmentEntity entity)
        {
            toolequipmentBll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveToolrecord(string keyValue, ToolrecordEntity entity)
        {
            toolequipmentBll.SaveToolrecord(keyValue, entity);
            return Success("�����ɹ���");
        }

        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "��������������")]
        public ActionResult Export(string queryJson, string tooltype)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            //pagination.p_kid = "ID";
            pagination.p_kid = "rownum idx";
            if (tooltype == "1")
            {
                pagination.p_fields =
                    @"equipmentvalue,specifications,outputdeptname,factorydate,equipmentno,checkdate,nextcheckdate,case  when b.Appraise = '1' then '�ϸ�' when b.Appraise = '2' then '���ϸ�' end as Appraise,
            b.operuser,district,depositary,controlusername";
            }
            else if (tooltype == "2")
            {
                //pagination.p_fields =
                //    @"equipmentvalue,case  when equipmenttype = '1' then '������ȫ������' when equipmenttype = '2' then '��е����ѧ������' end equipmenttype,specifications,equipmentno,case  when b.Appraise = '1' then '�ϸ�' when b.Appraise = '2' then '���ϸ�' end as Appraise,checkdate,nextcheckdate,b.operuser,district,depositary,controlusername";
                pagination.p_fields =
                    @"case  when equipmenttype = '1' then '������ȫ������' when equipmenttype = '2' then '��е����ѧ������' end equipmenttype,equipmentvalue,specifications,outputdeptname,factorydate,equipmentno,checkdate,nextcheckdate,b.operuser";
            }
            else if (tooltype == "3")
            {
            //    pagination.p_fields =
            //        @"equipmentvalue,equipmentno,specifications,belongdept,district,depositary,case  when b.Appraise = '1' then '�ϸ�' when b.Appraise = '2' then '���ϸ�' end as Appraise,b.operuser,controlusername,checkdate";
                pagination.p_fields =
                    @"equipmentvalue,equipmentno,specifications,quantity,unit,depositary,a.createusername,a.createdate";
            }
            pagination.p_tablename =
                @"BIS_TOOLEQUIPMENT a left join (select appraise,tb1.toolequipmentid,operuser from BIS_TOOLRECORD tb1 ,(select  max(createdate) createdate,toolequipmentid from BIS_TOOLRECORD group by toolequipmentid) tb2 
                where tb1.createdate= tb2.createdate  and tb1.toolequipmentid= tb2.toolequipmentid) b on a.id = b.toolequipmentid";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1";


            var data = toolequipmentBll.GetPageList(pagination, queryJson);
         
            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            if (tooltype == "1")
            {
                excelconfig.Title = "�綯������";
                excelconfig.FileName = "�綯������.xls";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 25;
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                excelconfig.ColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentvalue", ExcelColumn = "�綯����������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "����ͺ�", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "outputdeptname", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "factorydate", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "���", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nextcheckdate", ExcelColumn = "�´���������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "appraise", ExcelColumn = "����", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "operuser", ExcelColumn = "������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "depositary", ExcelColumn = "���λ��", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "controlusername", ExcelColumn = "������Ա", Alignment = "center" });
            }
            else if (tooltype == "2")
            {
                excelconfig.Title = "��ȫ������";
                excelconfig.FileName = "��ȫ������.xls";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 25;
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                excelconfig.ColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmenttype", ExcelColumn = "��ȫ����������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentvalue", ExcelColumn = "��ȫ����������", Alignment = "center" });     
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "����ͺ�", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "outputdeptname", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "factorydate", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "���", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "appraise", ExcelColumn = "�����", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nextcheckdate", ExcelColumn = "�´μ�������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "operuser", ExcelColumn = "������", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "��������", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "depositary", ExcelColumn = "���λ��", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "controlusername", ExcelColumn = "������Ա", Alignment = "center" });
            }
            else if (tooltype == "3")
            {
                excelconfig.Title = "�ֹ�����";
                excelconfig.FileName = "�ֹ�����.xls";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 25;
                excelconfig.IsAllSizeColumn = true;
                
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                excelconfig.ColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentvalue", ExcelColumn = "�ֹ���������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "���", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "����ͺ�", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "quantity", ExcelColumn = "����", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "unit", ExcelColumn = "��λ", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "belongdept", ExcelColumn = "��������", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "��������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "depositary", ExcelColumn = "���λ��", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "appraise", ExcelColumn = "�����", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "�Ǽ���", Alignment = "center" });
                //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "controlusername", ExcelColumn = "������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "�Ǽ�����", Alignment = "center" });
            }

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }


        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "��������������")]
        public ActionResult ExportStatisticsExcel(string queryJson)
        {
            var data = toolequipmentBll.GetToolStatisticsList(queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "������ͳ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "������ͳ��.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            excelconfig.ColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "EquipmentName", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num1", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num2", ExcelColumn = "��ȫ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num3", ExcelColumn = "��ȫ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num4", ExcelColumn = "�Ǹ߰�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num5", ExcelColumn = "�����۾�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num6", ExcelColumn = "�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num7", ExcelColumn = "�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num8", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num9", ExcelColumn = "�����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num10", ExcelColumn = "�ſ�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num11", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Total", ExcelColumn = "�ܼ�", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }

        #endregion

        #region �������ɶ�ά��
        /// <summary>
        /// �������ɶ�ά�벢������word
        /// </summary>
        /// <param name="userId">�û�Id,����ö��ŷָ�</param>
        /// <param name="userName">�û�����,����ö��ŷָ�</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImg(string equipIds, string equipNames, string equipNos, string equiptype)
        {
            if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/qrcode")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/qrcode"));
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/�豸��ά���ӡ.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            DataTable dt = new DataTable("U");
            dt.Columns.Add("BigEWM2");
            dt.Columns.Add("PersonName");
            dt.Columns.Add("PersonNo");
            int i = 0;
            string fileName = "";
            string[] equipNameArr = equipNames.Split(',');
            string[] equipNoArr = equipNos.Split(',');
            foreach (string code in equipIds.Split(','))
            {
                DataRow dr = dt.NewRow();
                dr[1] = equipNameArr[i];
                dr[2] = equipNoArr[i];

                fileName = code + ".jpg";
                string filePath = Server.MapPath("~/Resource/qrcode/" + fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    BuilderImg10(code, filePath, equiptype);
                }
                dr[0] = Server.MapPath("~/Resource/qrcode/" + fileName);
                dt.Rows.Add(dr);
                i++;
            }

            doc.MailMerge.Execute(dt);
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return Success("���ɳɹ�", new { fileName = fileName });
        }
        public void BuilderImg10(string keyValue, string filePath, string equiptype)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 21;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            float size = 301, margin = 1f;
            System.Drawing.Image image = qrCodeEncoder.Encode(keyValue + "|" + equiptype, Encoding.UTF8);
            int resWidth = (int)(size + 2 * margin);
            int resHeight = (int)(size + 2 * margin);
            // ���ľ��������½�һ��bitmap����Ȼ��image��������Ⱦ
            Bitmap newBit = new Bitmap(resWidth, resHeight, PixelFormat.Format32bppRgb);
            Graphics gg = Graphics.FromImage(newBit);

            // ���ñ�����ɫ
            for (int x = 0; x < resWidth; x++)
            {
                for (int y = 0; y < resHeight; y++)
                {
                    newBit.SetPixel(x, y, System.Drawing.Color.White);
                }
            }

            // ���ú�ɫ�߿�
            for (int i = 0; i < resWidth; i++)
            {
                newBit.SetPixel(i, 0, System.Drawing.Color.Black);
                newBit.SetPixel(i, resHeight - 1, System.Drawing.Color.Black);

            }

            for (int j = 0; j < resHeight; j++)
            {
                newBit.SetPixel(0, j, System.Drawing.Color.Black);
                newBit.SetPixel(resWidth - 1, j, System.Drawing.Color.Black);

            }
            RectangleF desRect = new RectangleF() { X = margin, Y = margin, Width = size, Height = size };
            RectangleF srcRect = new RectangleF() { X = 0, Y = 0, Width = image.Width, Height = image.Height };
            gg.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);
            newBit.Save(filePath, ImageFormat.Jpeg);
            newBit.Dispose();
            image.Dispose();
        }
        #endregion


        #region ���빤����
        /// <summary>
        /// ���빤����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HandlerMonitor(5, "���빤������Ϣ")]
        public string ImportTools(string tooltype, string DepartmentId)
        {
            try
            {

                int error = 0;
                string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
                string falseMessage = "";
                int count = 0;
                if (HttpContext.Request.Files.Count > 0)
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
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    List<ToolequipmentEntity> toolequipmentList = new List<ToolequipmentEntity>();
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    DataTable userTable = new UserBLL().GetTable(user.OrganizeId);

                    #region  ��ȫ�����ߵ���
                    if (tooltype == "2")
                    {
                        foreach (DataRow row in dt.Rows)
                        {

                            if (row[0].ToString() == "���")
                            {
                                continue;
                            }
                            else
                            {
                                ToolequipmentEntity toolequipment = new ToolequipmentEntity();
                                toolequipment.ToolType = tooltype;
                                if (tooltype == "2")
                                {
                                    switch (row[1].ToString())
                                    {
                                        case "������ȫ������":
                                            toolequipment.EquipmentType = "1";
                                            break;
                                        case "��е����ѧ������":
                                            toolequipment.EquipmentType = "2";
                                            break;
                                    }
                                }
                                else
                                {
                                    toolequipment.EquipmentType = "0";
                                }

                                toolequipment.EquipmentValue = row[2].ToString();
                                toolequipment.EquipmentName = dataitemdetailBll.GetItemValue(toolequipment.EquipmentValue);
                                toolequipment.Specifications = row[3].ToString();
                                toolequipment.OutputDeptName = row[4].ToString();
                                string factorydate = row[5].ToString();
                                try
                                {
                                    if (!string.IsNullOrEmpty(factorydate))
                                    {
                                        toolequipment.FactoryDate = DateTime.Parse(factorydate);
                                    }
                                    else
                                    {
                                        toolequipment.FactoryDate = null;
                                    }
                                }
                                catch (Exception)
                                {
                                    toolequipment.FactoryDate = null;
                                }
                                toolequipment.EquipmentNo = row[6].ToString();

                                string strcheckdate = row[7].ToString();
                                DateTime? checkdate;
                                try
                                {
                                    if (!string.IsNullOrEmpty(strcheckdate))
                                    {
                                        checkdate = DateTime.Parse(strcheckdate);
                                    }
                                    else
                                    {
                                        checkdate = null;
                                    }
                                }
                                catch (Exception)
                                {
                                    checkdate = null;
                                }

                                string strnextcheckdate = row[8].ToString();
                                DateTime? nextcheckdate;
                                try
                                {
                                    if (!string.IsNullOrEmpty(strnextcheckdate))
                                    {
                                        nextcheckdate = DateTime.Parse(strnextcheckdate);
                                    }
                                    else
                                    {
                                        nextcheckdate = null; ;
                                    }
                                }
                                catch (Exception)
                                {
                                    nextcheckdate = null; ;
                                }
                                toolequipment.CheckDate = checkdate;
                                toolequipment.NextCheckDate = nextcheckdate;
                                if (nextcheckdate != null && checkdate != null && nextcheckdate >= checkdate)
                                { toolequipment.CheckDateCycle = nextcheckdate.ToDate().Subtract(checkdate.ToDate()).Days.ToString(); }
                                string checkUserName = row[9].ToString().Trim();
                                string strwhere = "realname ='" + checkUserName + "' ";
                                DataRow[] checkUser = userTable.Select(strwhere);
                                if (checkUser.Length > 0)
                                {
                                    toolequipment.OperUser = checkUserName;
                                    toolequipment.OperUserId = checkUser[0]["userid"].ToString();
                                }

                                DepartmentEntity department;
                                if (!string.IsNullOrEmpty(DepartmentId))
                                {
                                    department = departmentBLL.GetEntity(DepartmentId);
                                }
                                else
                                {
                                    department = departmentBLL.GetEntity(user.DeptId);
                                }
                                toolequipment.ControlDept = department.FullName;
                                toolequipment.ControlDeptCode = department.EnCode;
                                toolequipment.ControlDeptId = department.DepartmentId;

                                toolequipmentList.Add(toolequipment);

                            }




                        }
                    }
                    #endregion

                    #region �ֹ����ߵ���

                    else if (tooltype == "3")
                    {
                        foreach (DataRow row in dt.Rows)
                        {

                            if (row[0].ToString() == "���")
                            {
                                continue;
                            }
                            else
                            {
                                ToolequipmentEntity toolequipment = new ToolequipmentEntity();
                                toolequipment.ToolType = tooltype;
                                toolequipment.EquipmentType = "0";
                                toolequipment.EquipmentValue = row[1].ToString();
                                toolequipment.EquipmentName = dataitemdetailBll.GetItemValue(toolequipment.EquipmentValue);
                                toolequipment.EquipmentNo = row[2].ToString();
                                toolequipment.Specifications = row[3].ToString();
                                toolequipment.Quantity = row[4].ToString();
                                toolequipment.Unit = row[5].ToString();
                                toolequipment.Depositary = row[6].ToString();
                                string createUserName = row[7].ToString().Trim();
                                string strwhere = "realname ='" + createUserName + "' ";
                                DataRow[] createUser = userTable.Select(strwhere);
                                if (createUser.Length > 0)
                                {
                                    toolequipment.CreateUserName = createUserName;
                                    toolequipment.CreateUserId = createUser[0]["userid"].ToString();
                                }

                                string createDate = row[8].ToString();
                                try
                                {
                                    if (!string.IsNullOrEmpty(createDate))
                                    {
                                        toolequipment.CreateDate = DateTime.Parse(createDate);
                                    }
                                    else
                                    {
                                        toolequipment.CreateDate = DateTime.Now;
                                    }
                                }
                                catch (Exception)
                                {
                                    toolequipment.CreateDate = DateTime.Now; ;
                                }

                                //���Ű���
                                DepartmentEntity department;
                                if (!string.IsNullOrEmpty(DepartmentId))
                                {
                                    department = departmentBLL.GetEntity(DepartmentId);
                                }
                                else
                                {
                                    department = departmentBLL.GetEntity(user.DeptId);
                                }
                                toolequipment.BelongDept = department.FullName;
                                toolequipment.BelongDeptCode = department.EnCode;
                                toolequipment.BelongDeptId = department.DepartmentId;

                                toolequipmentList.Add(toolequipment);

                            }
   



                        }
                    }
                    

                    #endregion
                


                    foreach (var toolequipment in toolequipmentList)
                    {
                        toolequipmentBll.SaveForm(Guid.NewGuid().ToString(), toolequipment);
                        count++;
                    }
                    message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                    message += "</br>" + falseMessage;
                }

                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        #endregion
    }
}
