using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.MatterManage.Controllers
{
    /// <summary>
    /// �� ������Ʊ�����볧��Ʊ
    /// </summary>
    public class OurTicketController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        private CalculateBLL calculatebll = new CalculateBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

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
        /// ����ҳ����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowForm(string keyValue)
        {
            return View();
        }

        /// <summary>
        /// ���̹����¼��ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NotesForm()
        {
            return View();
        }

        /// <summary>
        /// ���̹�����������ʻ·��
        /// </summary>
        /// <returns></returns>
        public ActionResult NotesItinerary()
        {
            return View();
        }


        /// <summary>
        /// ��ӡ��ͼ
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp()
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
        public ActionResult GetPageList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "znetwneight/1000 znetwneight,zcount, ca.Numbers,ca.createdate,ca.GetData,ca.ProductType,ca.SupplyName,ca.PlateNumber,ca.Dress,ca.Remark,ca.transporttype,ca.takegoodsname,ca.PassRemark,ca.IsFirst,ca.IsTrajectory,ca.WeighingNum,ca.DataBaseNum,ca.OutDate,ca.StayTime,ca.Status,ca.OrderNum,ca.OutDatabaseTime";
            pagination.p_tablename = "WL_OPERTICKETMANAGER ca left join (SELECT SUM(netwneight) znetwneight,COUNT(BASEID) zcount, BASEID FROM WL_CALCULATE WHERE ISDELETE='1' GROUP BY BASEID)  calculate on calculate.BASEID=ca.id   ";
            pagination.conditionJson = "1=1 and Isdelete='1' ";
            var watch = CommonHelper.TimerStart();
            var data = operticketmanagerbll.GetPageList(pagination, queryJson);
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
            var data = operticketmanagerbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�鿴���̹���ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetProcessFormJson(string keyValue)
        {
            var data = operticketmanagerbll.GetProcessEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ����һ����¼ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetNewFormJson(string keyValue)
        {
            var data = operticketmanagerbll.GetNewEntity("");
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
            operticketmanagerbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                data.Remark = entity.Remark;
                operticketmanagerbll.SaveForm(keyValue, data);
                SaveDailyRecord(data, "�޸�");
            }
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ��ӡʱͬ���޸ĳ���ʱ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpateOutTime(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                if (data.OutDate == null)
                {
                    UpdateCalculate(keyValue);
                    if (data.ShipLoading == 0)
                    {
                        data.OutDate = DateTime.Now;
                        data.LetMan = OperatorProvider.Provider.Current().UserName;
                    }
                    //SaveDailyRecord(data, "��ӡ");
                }
                data.OutDatabasetime = DateTime.Now;
                //else SaveDailyRecord(data, "�ٴδ�ӡ");
                data.ExamineStatus = 4;
                data.OrderNum = 1;
                if (data.Getdata != null)
                {
                    System.TimeSpan t1 = DateTime.Parse(data.OutDate.ToString()) - DateTime.Parse(data.Getdata.ToString());
                    data.StayTime = t1.TotalMinutes;
                    data.DbOutTime = data.RCdbTime = null;
                }
                operticketmanagerbll.SaveForm(keyValue, data);
            }
            return Success("�����ɹ���", data);
        }

        /// <summary>
        /// ��������ʱ��Ӧ���ؼ�¼�������޸�
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateCalculate(string keyValue)
        {
            string sql = string.Format("select id from wl_calculate d where d.baseid='{0}' and d.isdelete='1'", keyValue);
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                var data = calculatebll.GetEntity(dt.Rows[0][0].ToString());
                if (data != null)
                {
                    data.IsOut = 1;
                    calculatebll.SaveForm(data.ID, data);
                }
            }
        }


        /// <summary>
        /// �����쳣���б�ע
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult UpateContent(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                data.PassRemark = entity.PassRemark;
                data.LetMan = entity.LetMan;
                if (string.IsNullOrEmpty(entity.Status))
                {
                    data.Status = "����";
                    data.PassRemark = data.PassRemark + "(��������)";
                }
                operticketmanagerbll.SaveForm(keyValue, data);
                if (!data.OutDate.HasValue)
                    UpateOutTime(keyValue, data);//�쳣�����൱�ڳ�������
                RemoveCarpermission(data.Platenumber);
                ReleaseGPSEquipment(keyValue);
            }

            return Success("�����ɹ���", data);
        }

        /// <summary>
        /// �Ƴ�����ʶ����Զ�̧�˷���Ȩ��
        /// </summary>
        /// <param name="CarNo"></param>
        public void RemoveCarpermission(string CarNo)
        {
            #region ɾ����������Ȩ��

            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//������������Կ
            var baseurl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//������������ַ
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }

            if (!string.IsNullOrEmpty(CarNo))
            {
                var selectmodel = new
                {
                    pageNo = 1,
                    pageSize = 1000,
                    plateNo = CarNo
                };
                var existsVehicleStr = SocketHelper.LoadCameraList(selectmodel, baseurl, "/artemis/api/resource/v1/vehicle/advance/vehicleList", key, sign);
                dynamic existsVehicle = JsonConvert.DeserializeObject<dynamic>(existsVehicleStr);
                List<dynamic> vechileList = new List<dynamic>();

                if (existsVehicle.code == "0" && existsVehicle.data.total > 0)
                {
                    foreach (dynamic obj in existsVehicle.data.list)
                    {
                        vechileList.Add(obj.vehicleId);
                        break;
                    }
                    var delModel = new
                    {
                        vehicleIds = vechileList
                    };
                    SocketHelper.LoadCameraList(delModel, baseurl, "/artemis/api/resource/v1/vehicle/batch/delete", key, sign);
                }
            }
            #endregion
        }

        private void ReleaseGPSEquipment(string keyValue)
        {
            #region ��λ���ݷ���
            int Port = 0;
            string IP = CacheFactory.Cache().GetCache<string>("SocketUrl:IP");
            string PostStr = CacheFactory.Cache().GetCache<string>("SocketUrl:Port");
            if (!string.IsNullOrEmpty(PostStr))
                Port = Convert.ToInt32(PostStr);
            if (string.IsNullOrEmpty(IP) || Port == 0)
            {
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'SocketUrl'");
                foreach (var item in data)
                {
                    if (item.ItemName == "IP")
                    {
                        IP = item.ItemValue;
                        CacheFactory.Cache().WriteCache<string>(item.ItemValue, "SocketUrl:IP");
                    }
                    else if (item.ItemName == "Port")
                    {
                        Port = Convert.ToInt32(item.ItemValue);
                        CacheFactory.Cache().WriteCache<string>(item.ItemValue, "SocketUrl:Port");
                    }
                }
            }
            CarAlgorithmEntity car = new CarAlgorithmEntity();
            car.ID = keyValue;
            car.State = 1;
            SocketHelper.SendMsg(car.ToJson(), IP, Port);

            #endregion
        }

        /// <summary>
        /// ���湤����־
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SaveDailyRecord(OperticketmanagerEntity data, string type)
        {
            DailyrRecordEntity entity = new DailyrRecordEntity();
            entity.Content = data.Transporttype;
            entity.WorkType = 2;
            entity.Theme = type;
            operticketmanagerbll.InsetDailyRecord(entity);
        }


        #endregion

        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {


                Pagination pagination = new Pagination();

                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "Id";
                pagination.p_fields = " ca.Numbers,'' as ticketno,ca.takegoodsname,ca.PlateNumber,ca.transporttype,ca.ProductType,ca.Dress,zcount,znetwneight/1000 znetwneight,ca.GetData,ca.OutDate,ca.StayTime,ca.Status,ca.PassRemark,ca.createdate,ca.SupplyName,ca.Remark,ca.IsFirst,ca.IsTrajectory,ca.WeighingNum,ca.DataBaseNum,ca.OrderNum";
                pagination.p_tablename = "WL_OPERTICKETMANAGER ca left join (SELECT SUM(netwneight) znetwneight,COUNT(BASEID) zcount, BASEID FROM WL_CALCULATE WHERE ISDELETE='1' GROUP BY BASEID)  calculate on calculate.BASEID=ca.id   ";
                pagination.conditionJson = "1=1 and Isdelete='1' ";
                var watch = CommonHelper.TimerStart();
                DataTable exportTable = operticketmanagerbll.GetPageList(pagination, queryJson);

                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��������";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��������.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "numbers", ExcelColumn = "���/ת�˵���", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ticketno", ExcelColumn = "��Ʊ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "takegoodsname", ExcelColumn = "�����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "platenumber", ExcelColumn = "���ƺ�", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "transporttype", ExcelColumn = "��������", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "producttype", ExcelColumn = "����Ʒ����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dress", ExcelColumn = "װ�ҵ�", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "zcount", ExcelColumn = "����", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "znetwneight", ExcelColumn = "�������֣�", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "getdata", ExcelColumn = "����ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "outdate", ExcelColumn = "����ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "staytime", ExcelColumn = "���ڶ���ʱ��(����)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "status", ExcelColumn = "����״̬", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "passremark", ExcelColumn = "��ע", Width = 30 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception)
            {

            }
            return Success("�����ɹ���");
        }
    }
}
