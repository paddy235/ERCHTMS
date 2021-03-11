using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EquipmentManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using BSFramework.Data.Dapper;
using System.Configuration;
using BSFramework.Data;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// �� ������ͨ�豸������Ϣ��
    /// </summary>
    public class EquipmentController : MvcControllerBase
    {
        private EquipmentBLL equipmentbll = new EquipmentBLL();
        private ERCHTMS.Busines.PublicInfoManage.FileInfoBLL fileInfoBLL = new ERCHTMS.Busines.PublicInfoManage.FileInfoBLL();

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
        /// <summary>
        /// ѡ���豸
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// ���ɶ�ά��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImage()
        {
            return View();
        }

        /// <summary>
        /// �볡��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Leave()
        {
            return View();
        }

        /// <summary>
        /// �볡�б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LeaveList()
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
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,district,districtid,districtcode,case when t.state=1 then 'δ����'
when t.state=2 then '����' when t.state=3 then 'ͣ��' when t.state=4 then '����' when t.state=5 then '�볧' end as state,ControlDeptCode,CreateUserId,affiliation,(select count(1) from v_basehiddeninfo where workstream != '���Ľ���' and  deviceid=t.ID) hidnum,remark";
            pagination.p_tablename = "BIS_EQUIPMENT t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "  1=1";
            }
            else {
                //pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
                pagination.conditionJson = string.Format("  CREATEUSERORGCODE in(select  encode from BASE_DEPARTMENT start with encode='{0}' connect by  prior  departmentid = parentid)", user.OrganizeCode);
            }
            
            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson = where;
            //}
           
            var watch = CommonHelper.TimerStart();
            var data = equipmentbll.GetPageList(pagination, queryJson);
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
            var data = equipmentbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�����Ų��׼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHidStdJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = @"require stdname,CONTENT hiddesc,require hidmeasure";
            pagination.p_tablename = "BIS_HTSTANDARDITEM";
            pagination.conditionJson = "1=1";

            var watch = CommonHelper.TimerStart();
            var data = new HtStandardItemBLL().GetList(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = equipmentbll.GetEntity(keyValue);
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
            return ToJsonResult(equipmentbll.GetEquipmentNo(EquipmentNo, orgcode));
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
        /// ͨ���豸id��ȡ�豸�б�
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEquipmentTable(string ids)
        {
            DataTable dt = equipmentbll.GetEquipmentTable(ids.Split(','));
            return Content(dt.ToJson());
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
            equipmentbll.RemoveForm(keyValue);
            DeleteFiles(keyValue);
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
        public ActionResult SaveForm(string keyValue, EquipmentEntity entity)
        {
            entity.EquipmentName = entity.EquipmentName.Trim();
            entity.Specifications = entity.Specifications.Trim();
            entity.RelWord = string.IsNullOrWhiteSpace(entity.RelWord) ? "" : entity.RelWord.Trim();
            entity.UseAddress = string.IsNullOrWhiteSpace(entity.UseAddress) ? "": entity.UseAddress.Trim();
            entity.FactoryNo = string.IsNullOrWhiteSpace(entity.FactoryNo) ? "" : entity.FactoryNo.Trim();
            equipmentbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ��ͨ�豸�볡
        /// </summary>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <param name="equipmentId">�豸Id,����ö��ŷָ�</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "��ͨ�豸�볡")]
        public ActionResult Leave(string leaveTime, [System.Web.Http.FromBody]string equipmentId, [System.Web.Http.FromBody]string DepartureReason)
        {
            try
            {
                if (equipmentbll.SetLeave(equipmentId, leaveTime, DepartureReason) > 0)
                {
                    return Success("�����ɹ���");
                }
                else
                {
                    return Error("�������ɹ���");
                }
                
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

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
        public ActionResult BuilderImg(string equipIds, string equipNames, string equipNos,string equiptype)
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
        public void BuilderImg10(string keyValue, string filePath,string equiptype)
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

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ͨ�豸����")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,district,case when t.affiliation=1 then '����λ����' when t.affiliation=2 then '�����λ����' end as affiliation,controldept,case when t.state=1 then 'δ����' when t.state=2 then '����' when t.state=3 then 'ͣ��' when t.state=4 then '����' when t.state=5 then '�볧' end as useSta";
            pagination.p_tablename = "BIS_EQUIPMENT t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                //string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                //pagination.conditionJson = where;
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = equipmentbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "��ͨ�豸����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = "��ͨ�豸����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "�豸����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "�豸���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "����ͺ�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "affiliation", ExcelColumn = "������ϵ", Alignment = "center" });            
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "controldept", ExcelColumn = "�ܿز���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "usesta", ExcelColumn = "ʹ��״��", Alignment = "center" });

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }
        #endregion

        #region ������ͨ�豸
        /// <summary>
        /// ������ͨ�豸
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEquipment()
        {
            int error = 0;
            int sussceed = 0;
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
                string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(filePath);
                DataTable dt = ExcelHelper.ExcelImport(filePath);
                var districtList = new DistrictBLL().GetList().Where(x => x.OrganizeId == ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeId).ToList();
                var deptList = new DepartmentBLL().GetList().Where(x => x.OrganizeId == ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeId).ToList();
                var eno = int.Parse(equipmentbll.GetEquipmentNo("P1-", ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode)) + 1;                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals,deptList,districtList, out msg) == true)
                    {
                        var entity = GenEntity(vals, deptList, districtList, "P1-" + eno.ToString().PadLeft(4, '0'));
                        equipmentbll.SaveForm("", entity);
                        eno++;
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "��" + (i + 1) + "��" + msg + "</br>";
                        error++;
                    }
                }
                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + sussceed + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
                //ɾ����ʱ�ļ�
                System.IO.File.Delete(filePath);
            }
            return message;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool Validate(int index, object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList, out string msg)
        {
            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 6)
            {
                msg += "����ʽ����ȷ";
                r = false;
            }
            var obj = vals[0];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "���豸���Ʋ���Ϊ��";
                r = false;
            }
            obj = vals[1];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "������ͺŲ���Ϊ��";
                r = false;
            }           
            obj = vals[2];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "������������Ϊ��";
                r = false;
            }
            else
            {
                int ncount = districtList.Count(x => x.DistrictName == obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "������������ȷ";
                    r = false;
                }
            }
            obj = vals[3];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "���ܿز��Ų���Ϊ��";
                r = false;
            }
            else
            {
                int ncount = deptList.Count(x=>x.FullName==obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "���ܿز��Ų���ȷ";
                    r = false;
                }
            }
            //obj = vals[4];
            //if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            //{                
            //    msg += "�������ʲ���Ϊ��";
            //    r = false;
            //}
            obj = vals[5];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "��ʹ��״������Ϊ��";
                r = false;
            }
            else
            {
                var v = obj.ToString();
                if (v != "δ����" && v != "����" && v != "ͣ��" && v != "����" && v!="�볧")
                {
                    msg += "��ʹ��״��ֵ����ȷ";
                    r = false;
                }
            }

            return r;
        }
        private EquipmentEntity GenEntity(object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList,string eno)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            EquipmentEntity entity = new EquipmentEntity();
            entity.EquipmentName = vals[0].ToString().Trim();
            entity.Specifications = vals[1].ToString().Trim();            
            var obj = vals[2];
            if (obj != null && obj != DBNull.Value)
            {
                var district = districtList.FirstOrDefault(x => x.DistrictName == obj.ToString().Trim());
                if (district != null)
                {
                    entity.District = district.DistrictName;
                    entity.DistrictId = district.DistrictID;
                    entity.DistrictCode = district.DistrictCode;
                }
            }
            obj = vals[3];
            if (obj != null && obj != DBNull.Value)
            {
                var dept = deptList.FirstOrDefault(x => x.FullName == obj.ToString().Trim());
                if (dept != null)
                {
                    entity.ControlDept = dept.FullName;
                    entity.ControlDeptCode = dept.EnCode;
                    entity.ControlDeptID = dept.DepartmentId;
                }
            }
            entity.RelWord = vals[4].ToString().Trim();
            entity.State = "1";
            var state = vals[5].ToString();
            if (state == "����")
                entity.State = "2";
            else if (state == "ͣ��")
                entity.State = "3";
            else if (state == "����")
                entity.State = "4";
            else if (state == "�볧")
                entity.State = "5";

            entity.Affiliation = "1";
            entity.EquipmentNo = eno;

            return entity;
        }
        #endregion


        /// <summary>
        /// ����Զ�̵��豸��Ϣ
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetRemoteEquipmentList(string parentId)
        {
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = System.Web.HttpContext.Current.Server.MapPath(@"~/XmlConfig/remote_database.config");
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                AppSettingsSection appsection = (AppSettingsSection)config.GetSection("appSettings");
                string connectstring = appsection.Settings["RemoteBaseDb"].Value;
                IDatabase dbBase = new OracleDatabase(connectstring);
                string strWhere = " 1=1";
               
                if (!string.IsNullOrEmpty(parentId))
                {
                    strWhere += string.Format(" and parentid ='{0}' ");
                }
                else 
                {
                    strWhere += string.Format(" and id ='-1' "); //��
                }
                string sql = string.Format("select facname,faccode,parentid, id from FACTBFACILITY t where {0} order by faccode",strWhere);

                DataTable dt = dbBase.FindTable(sql);

                dbBase.Close();
           
                return new { code = 0, info = "��ȡ���ݳɹ�", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }


        /// <summary>
        /// ����Զ�̵��豸��Ϣ
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetRemoteEquipmentJson(int checkmode = 0, string facname = "",string nodeid="" ,int level =0)   
        {
            try
            {

                var treeList = new List<TreeEntity>();

                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

                fileMap.ExeConfigFilename = System.Web.HttpContext.Current.Server.MapPath(@"~/XmlConfig/remote_database.config");

                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                AppSettingsSection appsection = (AppSettingsSection)config.GetSection("appSettings");

                string connectstring = appsection.Settings["RemoteBaseDb"].Value;

                IDatabase dbBase = new OracleDatabase(connectstring);

                string strWhere = " 1=1";

                if (!string.IsNullOrEmpty(nodeid))
                {
                    level += 1;
                    strWhere += string.Format(@" and  parentid = '{0}' ", nodeid);
                }
                else 
                {
                    strWhere += @" and  parentid = '-1' ";
                }
                if (!string.IsNullOrEmpty(facname)) 
                {
                    strWhere += string.Format(@" and facname like '%{0}%'", facname);
                }

                string sql = string.Format("select createdate, facname,faccode,parentid, id from factbfacility t where {0} order by faccode", strWhere);

                DataTable dt = dbBase.FindTable(sql);

                dt.Columns.Add("level", typeof(Int32));
                dt.Columns.Add("isLeaf", typeof(bool));
                dt.Columns.Add("expanded", typeof(bool));

               
                DataTable clonedt = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        item["level"] = level;
                        DataRow[] itemDt = dt.Select(string.Format(" parentid ='{0}'", item["id"].ToString()));
                        item["isLeaf"] = itemDt.Length > 0;
                        item["expanded"] = false;
                        DataRow crow = clonedt.NewRow();
                        crow["facname"] = item["facname"];
                        crow["faccode"] = item["faccode"];
                        crow["createdate"] = item["createdate"];
                        crow["level"] = item["level"];
                        crow["parentid"] = item["parentid"];
                        crow["isLeaf"] = item["isLeaf"];
                        crow["id"] = item["id"];
                        clonedt.Rows.Add(crow);
                    }
                }
                var jsonData = new
                {
                    rows = clonedt,
                    total = clonedt.Rows.Count,
                    page =1,
                    records = clonedt.Rows.Count,

                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }


       /// <summary>
        /// ����Զ�̵��豸��Ϣ
       /// </summary>
       /// <param name="lv"></param>
       /// <param name="id"></param>
       /// <param name="pId"></param>
       /// <returns></returns>
        [HttpGet]
        public object GetRemoteEquipmentTreeJson(string lv,string id ="-1",string pId ="-1") 
        {
            try
            {

                var treeList = new List<TreeEntity>();

                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();

                fileMap.ExeConfigFilename = System.Web.HttpContext.Current.Server.MapPath(@"~/XmlConfig/remote_database.config");

                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                AppSettingsSection appsection = (AppSettingsSection)config.GetSection("appSettings");

                string connectstring = appsection.Settings["RemoteBaseDb"].Value;

                IDatabase dbBase = new OracleDatabase(connectstring);

                string strWhere = " 1=1";

                if (!string.IsNullOrEmpty(id))
                {
                    strWhere += string.Format(@" and  parentid = '{0}' ", id);
                }
                string sql = string.Format("select  facname t ,  facname name ,parentid  pId, id,faccode code from factbfacility t where {0} order by faccode", strWhere);

                DataTable dt = dbBase.FindTable(sql);
                dt.Columns.Add("click", typeof(bool));
                dt.Columns.Add("isParent", typeof(bool));
                DataTable clonedt = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        var childDt = dbBase.FindTable(string.Format("select  facname t ,  facname name ,parentid  pId, id from factbfacility t where parentid ='{0}' ", item["id"].ToString()));
                        item["isParent"] = childDt.Rows.Count > 0;
                        item["click"] = true;
                        DataRow crow = clonedt.NewRow();
                        crow["code"] = item["code"];
                        crow["name"] = item["name"];
                        crow["pId"] = item["pId"];
                        crow["id"] = item["id"];
                        crow["t"] = item["t"];
                        crow["click"] = item["click"];
                        crow["isParent"] = item["isParent"];
                        clonedt.Rows.Add(crow);
                    }
                }
                return ToJsonResult(clonedt); 
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
