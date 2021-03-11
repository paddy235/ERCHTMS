using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ThoughtWorks.QRCode.Codec;
using System.Text;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class CarinfoController : MvcControllerBase
    {
        private CarinfoBLL carinfobll = new CarinfoBLL();
        private CargpsBLL gpsbll = new CargpsBLL();
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
        /// �鿴ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult See()
        {
            return View();
        }
        /// <summary>
        /// �鿴Υ����Ϣҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViolationRecord()
        {
            return View();
        }
        
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ���ɳ���Base64��ά��ͼƬ
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetRCode(string CarNo) {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 5;
            qrCodeEncoder.QRCodeScale = 10;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            Bitmap bit=qrCodeEncoder.Encode(CarNo+"|��������",Encoding.UTF8);
            string Rcode=ImgToBase64(bit);
            bit.Dispose();
            return Rcode;
        }

        public string ImgToBase64(Bitmap bit)
        {
            try
            {
                MemoryStream ms=new MemoryStream();
                bit.Save(ms,ImageFormat.Jpeg);
                byte[] arr=new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int) ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception e)
            {
                return "";
            }
        }


        /// <summary>
        /// Υ��״̬
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWzStatus()
        {
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            ComboxEntity y1 = new ComboxEntity();
            y1.itemName = "��";
            y1.itemValue = "0";
            ComboxEntity y2 = new ComboxEntity();
            y2.itemName = "��";
            y2.itemValue = "1";
            Rlist.Add(y1);
            Rlist.Add(y2);
            return ToJsonResult(Rlist);
        }
        /// <summary>
        /// ��ȡ������ͨ���Ÿ�
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrent()
        {
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var dt = pdata.GetDataItemListByItemName("ͨ���Ÿ�");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ComboxEntity y1 = new ComboxEntity();
                    y1.itemName = item[0].ToString();
                    y1.itemValue = item[0].ToString();
                    Rlist.Add(y1);
                }
            }
            return ToJsonResult(Rlist);
        }
        /// <summary>
        /// �����������������Ӳ�ѯ
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCarInfo(string name)
        {
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var dt = pdata.GetDataItemListByItemName(name);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ComboxEntity y1 = new ComboxEntity();
                    y1.itemName = item[0].ToString();
                    y1.itemValue = item[1].ToString();
                    Rlist.Add(y1);
                }
            }
            return ToJsonResult(Rlist);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCarCailthepolice()
        {
            List<ComboxEntity> Rlist = new List<ComboxEntity>();
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            ComboxEntity y1 = new ComboxEntity();
            y1.itemName = "Խ�籨��";
            y1.itemValue = "1";
            ComboxEntity y2 = new ComboxEntity();
            y2.itemName = "���ٱ���";
            y2.itemValue = "0";
            Rlist.Add(y1);
            Rlist.Add(y2);
            return ToJsonResult(Rlist);
        }



        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "info.carno,info.dirver,info.phone,info.numberlimit,info.model,info.insperctiondate,info.nextinsperctiondate,NVL(zvi.num,0) as znum,NVL(vi.num,0) as num";
            pagination.p_tablename = @"bis_carinfo info
           left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   group by CARDNO)
						zvi on info.CARNO=zvi.CARDNO
					 left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   where ISPROCESS=0 group by CARDNO)
						vi on info.CARNO=vi.CARDNO
            ";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    pagination.conditionJson += " and createuserorgcode like '" + user.OrganizeCode + "%'";
            //}

            var data = carinfobll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// ��ѯ��GPS�豸�Ƿ�ռ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetGps(string id, string gpsid)
        {
            if (id.Trim() == "")
            {
                id = "1";
            }

            return gpsbll.GetGps(id, gpsid).ToString();
        }

        /// <summary>
        /// ��ȡ�������ʱ��
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetTimeString(string time)
        {
            if (time.Trim() != "")
            {
                return Convert.ToDateTime(time).AddDays(-1).AddYears(1).ToString("yyyy-MM-dd");
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// ���ƺ��Ƿ����ظ�
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetCarNoIsRepeat(string CarNo, string id)
        {
            return carinfobll.GetCarNoIsRepeat(CarNo, id).ToString();
        }

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = carinfobll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// �ϴ���ʻ֤/��ʻ֤
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile(string type)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //û���ļ��ϴ���ֱ�ӷ���
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string virtualPath = "";
            string UserId = OperatorProvider.Provider.Current().UserId;

            DataItemDetailBLL dd = new DataItemDetailBLL();
            string path = dd.GetItemValue("imgPath") + "\\Resource\\" + type;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var file = files[0];
            string ext = System.IO.Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + ".png";
            file.SaveAs(path + "\\" + fileName);
            //if (GetPicThumbnail(path + "\\" + fileName, path + "\\s" + fileName, 40, 100, 20))
            //{
            //    virtualPath = "/Resource/" + type + "/s" + fileName;
            //}
            //else
            //{
            virtualPath = "/Resource/" + type + "/" + fileName;
            //}
            return Success("�ϴ��ɹ���", virtualPath);



        }

        /// ����ѹ��ͼƬ  
        /// <param name="sFile">ԭͼƬ</param>  
        /// <param name="dFile">ѹ���󱣴�λ��</param>  
        /// <param name="dHeight">�߶�</param>  
        /// <param name="dWidth"></param>  
        /// <param name="flag">ѹ������(����ԽСѹ����Խ��) 1-100</param>  
        /// <returns></returns>  
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //����������
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //���´���Ϊ����ͼƬʱ������ѹ������  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//����ѹ���ı���1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile��ѹ�������·��  
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
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
            carinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, CarinfoEntity entity)
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//������������Կ
            var url = pdata.GetItemValue("HikBaseUrl");//������������ַ
            carinfobll.SaveForm(keyValue, entity, pitem, url);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �����嵥
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportData()
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
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                List<DataItemModel> data = dataItemDetailBLL.GetDataItemListByItemCode("'CarNo'").ToList();


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    //���ƺ�
                    string CarNo = dt.Rows[i][0].ToString();
                    //����Ʒ��ϵ��
                    string Model = dt.Rows[i][1].ToString();
                    ////��ʻ��
                    //string Dirver = dt.Rows[i][2].ToString();
                    ////��ʻ�˵绰
                    //string Phone = dt.Rows[i][3].ToString();
                    //����������
                    string Time = dt.Rows[i][2].ToString();
                    //��ʼʱ��
                    string StartTime = dt.Rows[i][3].ToString();
                    ////����ʱ��
                    string EndTime = dt.Rows[i][4].ToString();
                    //��������
                    string Num = dt.Rows[i][5].ToString();

                    if (CarNo.Trim() == "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "���ƺ�Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    CarNo = CarNo.Trim().ToUpper();//Ӣ��ת��Ϊ��д


                    string s = CarNo.Substring(0, 1);
                    bool flag = false;
                    foreach (var d in data)
                    {
                        if (d.ItemName == s)
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag == false)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "���ƺ������ʽ����,��һλ��������ȷ��ʡ��д,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    //�жϳ��ƺ�λ���Ƿ�Ϸ�
                    if (!Regex.IsMatch(CarNo.Trim(), "(^[�����弽ԥ���ɺ�����³������Ӷ���ʽ����¼���������ش�����ʹ��A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9��ѧ���۰�]{1}$)"))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "���ƺ���д����,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (Model.Trim() == "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�ڲ����Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    //if (Dirver.Trim() == "")
                    //{
                    //    falseMessage += "</br>" + "��" + (i + 2) + "��ʻ��Ϊ��,δ�ܵ���.";
                    //    error++;
                    //    continue;
                    //}
                    //if (Phone.Trim() == "")
                    //{
                    //    falseMessage += "</br>" + "��" + (i + 2) + "��ʻ�˵绰Ϊ��,δ�ܵ���.";
                    //    error++;
                    //    continue;
                    //}
                    if (Time.Trim() == "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "����������Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (StartTime.Trim() == "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʼʱ��Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (EndTime.Trim() == "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "����ʱ��Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (Num.Trim() == "")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��������Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    DateTime dtime;
                    try
                    {

                        DateTime.TryParse(Time, out dtime);

                        if (dtime.ToString("yyyy-MM-dd") == "0001-01-01")
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "���������ڸ�ʽ����,������yyyy-MM-dd��ʽ,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "���������ڸ�ʽ����,������yyyy-MM-dd��ʽ,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    DateTime Stime;


                    DateTime.TryParse(StartTime, out Stime);

                    if (Stime.ToString("yyyy-MM-dd") == "0001-01-01")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ʼʱ���ʽ����,������yyyy-MM-dd��ʽ,δ�ܵ���.";
                        error++;
                        continue;
                    }



                    DateTime Etime;


                    DateTime.TryParse(StartTime, out Etime);

                    if (Etime.ToString("yyyy-MM-dd") == "0001-01-01")
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "����ʱ���ʽ����,������yyyy-MM-dd��ʽ,δ�ܵ���.";
                        error++;
                        continue;
                    }


                    if (carinfobll.GetCarNoIsRepeat(CarNo, ""))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "���ƺ������Ѵ���,�����ظ�¼��.";
                        error++;
                        continue;
                    }

                    Etime = Convert.ToDateTime(Etime.ToString("yyyy-MM-dd 23:59:59"));
                    Stime = Convert.ToDateTime(Stime.ToString("yyyy-MM-dd 00:00:00"));

                    CarinfoEntity hf = new CarinfoEntity();
                    hf.CarNo = CarNo.Trim();
                    hf.GpsId = "";
                    hf.GpsName = "";
                    //hf.Dirver = Dirver;
                    hf.InsperctionDate = dtime;
                    hf.Model = Model;
                    hf.NumberLimit = Convert.ToInt32(Num);
                    hf.Endtime = Etime;
                    hf.Starttime = Stime;
                    //hf.Phone = Phone;
                    hf.NextInsperctionDate = dtime.AddDays(-1).AddYears(1);
                    hf.Type = 0;


                    try
                    {
                        DataItemDetailBLL pdata = new DataItemDetailBLL();
                        var pitem = pdata.GetItemValue("Hikappkey");//������������Կ
                        var url = pdata.GetItemValue("HikBaseUrl");//������������ַ
                        carinfobll.SaveForm("", hf, pitem, url);
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
