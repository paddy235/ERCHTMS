using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// �� ��������ʲ�Ž�����
    /// </summary>
    public class KbsdeviceController : MvcControllerBase
    {
        private KbsdeviceBLL kbsdevicebll = new KbsdeviceBLL();
        DataItemDetailBLL itemBll = new DataItemDetailBLL();
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
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult See()
        {
            return View();
        }

        /// <summary>
        /// ͳ��ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult Static()
        {
            return View();
        }
        /// <summary>
        /// ����������ҳ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseHome()
        {
            return View();
        }
        public ActionResult GetPos()
        {
            return View();
        }

        /// <summary>
        /// ��άȫ����ʾ
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowThreePage()
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
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var data = kbsdevicebll.GetPageList(queryJson);

            int total = data.Count / pagination.rows;
            if (data.Count % pagination.rows != 0)
            {
                total += 1;
            }

            var jsonData = new
            {
                rows = data,
                total = total,
                page = 1,
                records = data.Count,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// WEB��ȡ��ά��ͼ·��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string WebGetThreeUrl()
        {
            return itemBll.GetItemValue("WebKbsThreeDURL");
        }
        /// <summary>
        /// APP��ȡ��ά��ͼ·��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetThreeUrl()
        {
            return itemBll.GetItemValue("KbsThreeDURL");
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = kbsdevicebll.GetList(queryJson);
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
            var data = kbsdevicebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ǩͳ��ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDeviceChart()
        {
            List<object[]> list = new List<object[]>();
            var data = kbsdevicebll.GetPageList("");
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
            foreach (var item in AreaList)
            {
                int num = 0;
                num = data.Where(it => it.AreaCode.Contains(item.DistrictCode)).Count();
                object[] arr = { item.DistrictName, num };
                if (num > 0)
                {
                    list.Add(arr);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// ��ǩͳ�Ʊ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDeviceStatistics()
        {
            var data = kbsdevicebll.GetPageList("");
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<KbssEntity> klist = new List<KbssEntity>();
            int Znum = 0;
            foreach (var item in AreaList)
            {
                KbssEntity kbs = new KbssEntity();
                kbs.Name = item.DistrictName;
                kbs.Num = data.Where(it => it.AreaCode.Contains(item.DistrictCode)).Count();
                kbs.DistrictCode = item.DistrictCode;
                Znum += kbs.Num;
                kbs.OnNum = data.Where(it => it.AreaCode.Contains(item.DistrictCode) && it.State == "����").Count();
                kbs.OffNum = data.Where(it => it.AreaCode.Contains(item.DistrictCode) && it.State == "����").Count();
                klist.Add(kbs);
            }


            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                double offProportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].OnNum / Znum;
                    offProportion = (double)klist[j].OffNum / Znum;
                    Proportion = Proportion * 100;
                    offProportion = offProportion * 100;
                }
                klist[j].Count = Znum;
                klist[j].OnProportion = Proportion.ToString("0.00") + "%";
                klist[j].OfflineProportion = offProportion.ToString("0.00") + "%";
            }

            return Content(klist.ToJson());
        }

        /// <summary>
        /// ��ȡ��ǩ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetCount()
        {
            return kbsdevicebll.GetPageList("").Count.ToString();
        }

        /// <summary>
        /// ����״̬��ȡ��վ����
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetDeviceNum(string status)
        {
            return kbsdevicebll.GetDeviceNum(status).ToString();
        }
        #endregion

        #region �ύ����
        //<summary>
        //�����Ž�
        //</summary>
        //<returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDevice()
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


                DistrictBLL districtbll = new DistrictBLL();
                List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "");


                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                List<DataItemModel> data = dataItemDetailBLL.GetDataItemListByItemCode("'KbsOutType'").ToList();



                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string DeviceID = dt.Rows[i][0].ToString();
                    string ControllerId = dt.Rows[i][1].ToString();
                    string OutType = dt.Rows[i][2].ToString();
                    string DeviceName = dt.Rows[i][3].ToString();
                    string DeviceModel = dt.Rows[i][4].ToString();
                    //����
                    string AreaName = dt.Rows[i][5].ToString();
                    string AreaValue = "";
                    string AreaCode = "";
                    //¥����
                    string FloorNo = dt.Rows[i][6].ToString();
                    string DevicePoint = dt.Rows[i][7].ToString();
                    string DeviceIp = dt.Rows[i][8].ToString();

                    if (string.IsNullOrEmpty(DeviceID))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�Ž�IDΪ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(ControllerId))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "������IDΪ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(OutType))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��������Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(DeviceName))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�Ž�����Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(DeviceModel))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�Ž��ͺ�Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(AreaName))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��������Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(FloorNo))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "¥����Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(DevicePoint))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�Ž�����Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(DeviceIp))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "IP��ַΪ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    var IP = @"(^(\d+)\.(\d+)\.(\d+)\.(\d+)$)";//@"/^(\d+)\.(\d+)\.(\d+)\.(\d+)$/g";
                    var point = @"(^\d{1,9}(.\d{1,2});\d{1,9}(.\d{1,2})$)";

                    ////��֤�Ƿ���IP
                    if (!Regex.IsMatch(DeviceIp, IP))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��IP��ַ��ʽ��д����,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    ////��֤�Ƿ�������
                    if (!Regex.IsMatch(DevicePoint, point))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�������ʽ��д����,��ʽӦΪxx.xx;xx.xx,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    var area = AreaList.Where(it => it.DistrictName == AreaName).FirstOrDefault();
                    if (area == null)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "������������д����,δ�ҵ���Ӧ������,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    var ot = data.Where(it => it.ItemName == OutType).FirstOrDefault();
                    if (ot == null)
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "�н���������д����,δ�ҵ���Ӧ������,δ�ܵ���.";
                        error++;
                        continue;
                    }

                    AreaValue = area.DistrictID;
                    AreaCode = area.DistrictCode;

                    KbsdeviceEntity kbs = new KbsdeviceEntity();
                    kbs.AreaCode = AreaCode;
                    kbs.AreaName = AreaName;
                    kbs.DeviceId = DeviceID;
                    kbs.DeviceName = DeviceName;
                    kbs.DeviceModel = DeviceModel;
                    kbs.OutType = Convert.ToInt32(ot.ItemValue);
                    kbs.FloorNo = FloorNo;
                    kbs.OperUserName = OperatorProvider.Provider.Current().UserName;
                    kbs.AreaId = AreaValue;
                    kbs.DeviceIP = DeviceIp;
                    kbs.DevicePoint = DevicePoint;
                    kbs.ControllerId = ControllerId;
                    kbs.State = "����";

                    try
                    {
                        kbsdevicebll.SaveForm("", kbs);
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
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            kbsdevicebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, KbsdeviceEntity entity)
        {
            if (keyValue == "")
            {
                entity.State = "����";
            }
            kbsdevicebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        #endregion
    }
}
