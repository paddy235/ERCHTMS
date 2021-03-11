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
using System.Web;
using System;
using System.Data;
using ERCHTMS.Cache;
using ERCHTMS.Busines.BaseManage;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// �� �����ص����λ
    /// </summary>
    public class KeyPartController : MvcControllerBase
    {
        private KeyPartBLL keypartbll = new KeyPartBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private readonly DepartmentBLL departBLL = new DepartmentBLL();
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
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }

        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericIndex()
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
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericForm()
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
        #endregion

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
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericImport()
        {
            return View();
        }
        

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
            pagination.p_fields = "PartName,District,DutyUser,Dutydept,Rank,NextPatrolDate,EmployState,createuserid,createuserdeptcode,createuserorgcode,DutyUserId";
            pagination.p_tablename = "HRS_KEYPART";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = keypartbll.GetPageList(pagination, queryJson);
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
            var data = keypartbll.GetList(queryJson);
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
            var data = keypartbll.GetEntity(keyValue);
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
            keypartbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, KeyPartEntity entity)
        {
            keypartbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �������ɶ�ά�벢������word
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="partNames"></param>
        /// <param name="dutyDepts"></param>
        /// <param name="dutyUsers"></param>
        /// <param name="equiptype"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImg(string Ids, string partNames, string dutyDepts, string dutyUsers, string equiptype)
        {
            if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/qrcode")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/qrcode"));
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/�ص����λ��ά���ӡ.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            DataTable dt = new DataTable("U");
            dt.Columns.Add("BigEWM2");
            dt.Columns.Add("PartName");
            dt.Columns.Add("DutyDept");
            dt.Columns.Add("DutyUser");
            int i = 0;
            string fileName = "";
            string[] partNameArr = partNames.Split(',');
            string[] dutyDeptArr = dutyDepts.Split(',');
            string[] dutyUserArr = dutyUsers.Split(',');
            foreach (string code in Ids.Split(','))
            {
                DataRow dr = dt.NewRow();
                dr[1] = partNameArr[i];
                dr[2] = dutyDeptArr[i];
                dr[3] = dutyUserArr[i];

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="filePath"></param>
        /// <param name="equiptype"></param>
        public void BuilderImg10(string keyValue, string filePath, string equiptype)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
                QRCodeVersion = 21,
                QRCodeScale = 3,
                QRCodeForegroundColor = System.Drawing.Color.Black
            };
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
            pagination.p_fields = @"PartName,District,Dutydept,DutyUser,to_char(NextPatrolDate,'yyyy-MM-dd') as NextPatrolDate,
 case when EmployState = '0' then '����' when EmployState = '1' then 'ͣ��' when EmployState = '2' then '����' else '' end as EmployState,
 case when Rank = '1' then 'һ����������' when Rank = '2' then '������������' else '' end as Rank";
            pagination.p_tablename = "HRS_KEYPART";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = keypartbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�ص����λ";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�ص����λ.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "partname", ExcelColumn = "�ص����λ����", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "����", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutydept", ExcelColumn = "���β���", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutyuser", ExcelColumn = "������", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "nextpatroldate", ExcelColumn = "�´�Ѳ������", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "employstate", ExcelColumn = "ʹ��״̬", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "rank", ExcelColumn = "���𼶱�", Alignment = "center" });

            excelconfig.ColumnEntity = listColumnEntity;
            //���õ�������
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //���õ�������
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, listColumnEntity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����ص����λ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ImportFirefighting()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޴˲���Ȩ��";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾          
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
                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    KeyPartEntity item = new KeyPartEntity();
                    order = i + 1;
                    #region �ص����λ����
                    string partname = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(partname))
                    {
                        item.PartName = partname;
                        var data = new DataItemCache().ToItemValue("PartName", partname);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.PartNo = data;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�ص����λ���Ʋ����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�ص����λ���Ʋ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ����λ��
                    string district = dt.Rows[i][1].ToString();
                    if (!string.IsNullOrEmpty(district))
                    {
                        var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                        if (disItem != null)
                        {
                            item.DistrictId = disItem.DistrictID;
                            item.DistrictCode = disItem.DistrictCode;
                            item.District = district;
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����λ�ò����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,����λ�ò���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���β���
                    string dutydept = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(dutydept))
                    {
                        var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                        if (data != null)
                        {
                            item.DutyDept = dutydept;
                            item.DutyDeptCode = data.EnCode;
                        }

                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ������
                    string dutyUser = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(dutyUser))
                    {
                        var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                        if (userEntity != null)
                        {
                            item.DutyUserId = userEntity.UserId;
                            item.DutyUser = dutyUser;
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

                    #region �����˵绰
                    string dutyTel = dt.Rows[i][4].ToString();
                    if (!string.IsNullOrEmpty(dutyTel))
                    {
                        item.DutyTel = dutyTel;
                    }
                    #endregion

                    #region �����ṹ
                    string structure = dt.Rows[i][5].ToString();
                    if (!string.IsNullOrEmpty(structure))
                    {
                        item.Structure = structure;
                        var data = new DataItemCache().ToItemValue("Structure", structure);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.Structure = data;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����ṹ�����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"��{0}�е���ʧ��,�����ṹ����Ϊ�գ�</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region ���������m2��
                    string acreage = dt.Rows[i][6].ToString();
                    int tempAcreage;
                    if (!string.IsNullOrEmpty(acreage))
                    {
                        if (int.TryParse(acreage, out tempAcreage))
                            item.Acreage = tempAcreage;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���������m2������Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"��{0}�е���ʧ��,���������m2������Ϊ�գ�</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region ��Ҫ�洢��Ʒ
                    string storegoods = dt.Rows[i][7].ToString();
                    if (!string.IsNullOrEmpty(storegoods))
                    {
                        item.StoreGoods = storegoods;
                    }
                    #endregion

                    #region ��Ҫ�����װ��
                    string outfireequip = dt.Rows[i][8].ToString();
                    if (!string.IsNullOrEmpty(outfireequip))
                    {
                        item.OutfireEquip = outfireequip;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��Ҫ�����װ������Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region �ص����λ����
                    string peoplenum = dt.Rows[i][9].ToString();
                    int tempPeopleNum;
                    if (!string.IsNullOrEmpty(peoplenum))
                    {
                        if (int.TryParse(peoplenum, out tempPeopleNum))
                            item.PeopleNum = tempPeopleNum;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�ص����λ��������Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region ���𼶱�
                    string rank = dt.Rows[i][10].ToString();
                    if (!string.IsNullOrEmpty(rank))
                    {
                        if (rank == "һ����������") item.Rank = 1;
                        else if (rank == "������������") item.Rank = 2;
                        else {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���𼶱𲻴��ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,���𼶱���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���Ѳ�����ڣ������գ�
                    string latelypatroldate = dt.Rows[i][11].ToString();
                    DateTime tempLatelyPatrolDate;
                    if (!string.IsNullOrEmpty(latelypatroldate))
                        if (DateTime.TryParse(latelypatroldate, out tempLatelyPatrolDate))
                            item.LatelyPatrolDate = tempLatelyPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���Ѳ�����ڲ��ԣ�</br>", order);
                            error++;
                            continue;
                        }
                    #endregion

                    #region Ѳ�����ڣ��죩
                    string patrolperiod = dt.Rows[i][12].ToString();
                    int tempPatrolPeriod;
                    if (!string.IsNullOrEmpty(patrolperiod))
                    {
                        if (int.TryParse(patrolperiod, out tempPatrolPeriod))
                            item.PatrolPeriod = tempPatrolPeriod;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,Ѳ�����ڣ��죩����Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region �´�Ѳ������
                    string nextpatroldate = dt.Rows[i][13].ToString();
                    DateTime tempNextPatrolDate;
                    if (!string.IsNullOrEmpty(nextpatroldate))
                        if (DateTime.TryParse(nextpatroldate, out tempNextPatrolDate))
                            item.NextPatrolDate = tempNextPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�´�Ѳ�����ڲ��ԣ�</br>", order);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�´�Ѳ�����ڲ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ʹ��״̬
                    string employstate = dt.Rows[i][14].ToString();
                    if (!string.IsNullOrEmpty(employstate))
                    {
                        if (employstate == "����") item.EmployState = 0;
                        else if (employstate == "ͣ��") item.EmployState = 1;
                        else if (employstate == "����") item.EmployState = 2;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ʹ��״̬�����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,ʹ��״̬����Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ����Σ���Է���
                    string analyze = dt.Rows[i][15].ToString();
                    if (!string.IsNullOrEmpty(analyze))
                    {
                        item.Analyze = analyze;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,����Σ���Է�������Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region �����ʩ
                    string measure = dt.Rows[i][16].ToString();
                    if (!string.IsNullOrEmpty(measure))
                    {
                        item.Measure = measure;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�����ʩ����Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion
                    //����Ҫ��
                    string require = dt.Rows[i][17].ToString();
                    if (!string.IsNullOrEmpty(require))
                    {
                        item.Require = require;
                    }
                    //��ע
                    string remark = dt.Rows[i][18].ToString();
                    if (!string.IsNullOrEmpty(remark))
                    {
                        item.Remark = remark;
                    }

                    try
                    {
                        keypartbll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }


        /// <summary>
        /// �����ص����λ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ImportGenericFirefighting()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޴˲���Ȩ��";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾          
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
                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    KeyPartEntity item = new KeyPartEntity();
                    order = i + 1;
                    #region �ص����λ����
                    string partname = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(partname))
                    {
                        item.PartName = partname;
                        item.PartNo = partname;
                        //var data = new DataItemCache().ToItemValue("PartName", partname);
                        //if (data != null && !string.IsNullOrEmpty(data))
                        //    item.PartNo = data;
                        //else
                        //{
                        //    falseMessage += string.Format(@"��{0}�е���ʧ��,�ص����λ���Ʋ����ڣ�</br>", order);
                        //    error++;
                        //    continue;
                        //}
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�ص����λ���Ʋ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ����λ��
                    string district = dt.Rows[i][1].ToString();
                    if (!string.IsNullOrEmpty(district))
                    {
                        var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                        if (disItem != null)
                        {
                            item.DistrictId = disItem.DistrictID;
                            item.DistrictCode = disItem.DistrictCode;
                            item.District = district;
                        }
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,����λ�ò����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,����λ�ò���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���β���
                    string dutydept = dt.Rows[i][2].ToString();
                    if (!string.IsNullOrEmpty(dutydept))
                    {
                        var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                        if (data != null)
                        {
                            item.DutyDept = dutydept;
                            item.DutyDeptCode = data.EnCode;
                        }

                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,���β��Ų���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ������
                    string dutyUser = dt.Rows[i][3].ToString();
                    if (!string.IsNullOrEmpty(dutyUser))
                    {
                        var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                        if (userEntity != null)
                        {
                            item.DutyUserId = userEntity.UserId;
                            item.DutyUser = dutyUser;
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

                    #region �����˵绰
                    string dutyTel = dt.Rows[i][4].ToString();
                    if (!string.IsNullOrEmpty(dutyTel))
                    {
                        item.DutyTel = dutyTel;
                    }
                    #endregion

                    #region �����ṹ
                    string structure = dt.Rows[i][5].ToString();
                    if (!string.IsNullOrEmpty(structure))
                    {
                        item.Structure = structure;
                        var data = new DataItemCache().ToItemValue("Structure", structure);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.Structure = data;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�����ṹ�����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"��{0}�е���ʧ��,�����ṹ����Ϊ�գ�</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region ���������m2��
                    string acreage = dt.Rows[i][6].ToString();
                    int tempAcreage;
                    if (!string.IsNullOrEmpty(acreage))
                    {
                        if (int.TryParse(acreage, out tempAcreage))
                            item.Acreage = tempAcreage;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���������m2������Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    //else
                    //{
                    //    falseMessage += string.Format(@"��{0}�е���ʧ��,���������m2������Ϊ�գ�</br>", order);
                    //    error++;
                    //    continue;
                    //}
                    #endregion

                    #region ��Ҫ�洢��Ʒ
                    string storegoods = dt.Rows[i][7].ToString();
                    if (!string.IsNullOrEmpty(storegoods))
                    {
                        item.StoreGoods = storegoods;
                    }
                    #endregion

                    #region ��Ҫ�����װ��
                    string outfireequip = dt.Rows[i][8].ToString();
                    if (!string.IsNullOrEmpty(outfireequip))
                    {
                        item.OutfireEquip = outfireequip;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,��Ҫ�����װ������Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region �ص����λ����
                    string peoplenum = dt.Rows[i][9].ToString();
                    int tempPeopleNum;
                    if (!string.IsNullOrEmpty(peoplenum))
                    {
                        if (int.TryParse(peoplenum, out tempPeopleNum))
                            item.PeopleNum = tempPeopleNum;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�ص����λ��������Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region ���𼶱�
                    string rank = dt.Rows[i][10].ToString();
                    if (!string.IsNullOrEmpty(rank))
                    {
                        if (rank == "һ����������") item.Rank = 1;
                        else if (rank == "������������") item.Rank = 2;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���𼶱𲻴��ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,���𼶱���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ���Ѳ�����ڣ������գ�
                    string latelypatroldate = dt.Rows[i][11].ToString();
                    DateTime tempLatelyPatrolDate;
                    if (!string.IsNullOrEmpty(latelypatroldate))
                        if (DateTime.TryParse(latelypatroldate, out tempLatelyPatrolDate))
                            item.LatelyPatrolDate = tempLatelyPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,���Ѳ�����ڲ��ԣ�</br>", order);
                            error++;
                            continue;
                        }
                    #endregion

                    #region Ѳ�����ڣ��죩
                    string patrolperiod = dt.Rows[i][12].ToString();
                    int tempPatrolPeriod;
                    if (!string.IsNullOrEmpty(patrolperiod))
                    {
                        if (int.TryParse(patrolperiod, out tempPatrolPeriod))
                            item.PatrolPeriod = tempPatrolPeriod;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,Ѳ�����ڣ��죩����Ϊ���֣�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    #endregion

                    #region �´�Ѳ������
                    string nextpatroldate = dt.Rows[i][13].ToString();
                    DateTime tempNextPatrolDate;
                    if (!string.IsNullOrEmpty(nextpatroldate))
                        if (DateTime.TryParse(nextpatroldate, out tempNextPatrolDate))
                            item.NextPatrolDate = tempNextPatrolDate;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,�´�Ѳ�����ڲ��ԣ�</br>", order);
                            error++;
                            continue;
                        }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�´�Ѳ�����ڲ���Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ʹ��״̬
                    string employstate = dt.Rows[i][14].ToString();
                    if (!string.IsNullOrEmpty(employstate))
                    {
                        if (employstate == "����") item.EmployState = 0;
                        else if (employstate == "ͣ��") item.EmployState = 1;
                        else if (employstate == "����") item.EmployState = 2;
                        else
                        {
                            falseMessage += string.Format(@"��{0}�е���ʧ��,ʹ��״̬�����ڣ�</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,ʹ��״̬����Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region ����Σ���Է���
                    string analyze = dt.Rows[i][15].ToString();
                    if (!string.IsNullOrEmpty(analyze))
                    {
                        item.Analyze = analyze;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,����Σ���Է�������Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    #region �����ʩ
                    string measure = dt.Rows[i][16].ToString();
                    if (!string.IsNullOrEmpty(measure))
                    {
                        item.Measure = measure;
                    }
                    else
                    {
                        falseMessage += string.Format(@"��{0}�е���ʧ��,�����ʩ����Ϊ�գ�</br>", order);
                        error++;
                        continue;
                    }
                    #endregion
                    //����Ҫ��
                    string require = dt.Rows[i][17].ToString();
                    if (!string.IsNullOrEmpty(require))
                    {
                        item.Require = require;
                    }
                    //��ע
                    string remark = dt.Rows[i][18].ToString();
                    if (!string.IsNullOrEmpty(remark))
                    {
                        item.Remark = remark;
                    }

                    try
                    {
                        keypartbll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }
        #endregion
    }
}
