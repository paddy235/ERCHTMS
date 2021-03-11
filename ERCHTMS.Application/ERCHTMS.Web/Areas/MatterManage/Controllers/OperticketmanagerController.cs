using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.MatterManage.Controllers
{
    /// <summary>
    /// �� ������Ʊ�����볧��Ʊ
    /// </summary>
    public class OperticketmanagerController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        private CalculateBLL calculatebll = new CalculateBLL();
        private DataItemBLL dataItemBLL = new DataItemBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

        private object NumberLock = new object();

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
        /// ˾��������Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditDriverInfo()
        {
            return View();
        }

        /// <summary>
        /// ѡ��Ʊģ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TemplateSelect()
        {
            return View();
        }

        /// <summary>
        /// ��ӡ��ͼ
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp(string keyValue)
        {
            string sql = string.Format("select sum(netwneight) from wl_calculate  where isdelete='1' and  baseid='{0}'", keyValue);
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {//����
                ViewBag.weight = dt.Rows[0][0].ToString();
            }
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
            pagination.p_fields = "Numbers,createdate,GetData,ProductType,SupplyName,PlateNumber,Dress,Remark,transporttype,takegoodsname,outdate,outcu,getstamptime,ORDERNUMR,drivername,drivertel";
            pagination.p_tablename = "WL_OPERTICKETMANAGER";
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
        /// ��ȡ�볡��Ʊ������¼
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetdailyrRecord(string type)
        {
            string sql = string.Format("select CREATEDATE,theme,content,WorkType,UserName from wl_dailyrRecord d where WorkType={2} and CREATEDATE > to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') and CREATEDATE < to_date('{1}', 'yyyy-MM-dd HH24:mi:ss') order by createdate desc ", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1), type);
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            return dt.Rows.Count > 0 ? dt.ToJson() : "";
        }

        /// <summary>
        /// ��ȡ˾���ϴ���Ʊ��Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetDriverUplodUrl(string keyValue)
        {
            string url = dataItemDetailBLL.GetItemValue("imgUrl") + "/Content/SecurityDynamics/index.html?keyValue=" + keyValue;
            return url;
        }

        /// <summary>
        ///�ж��Ƿ�����ͬ����δ������¼
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetisNotOutvehicle(string keyValue, string Number)
        {
            string res = string.Empty;
            string sql = string.Format("select count(1) from wl_operticketmanager d where d.platenumber='{0}' and Isdelete='1' and d.outdate is null ", Number);
            if (!string.IsNullOrEmpty(keyValue))
            {//�޸�ʱ�ų�����
                sql = string.Format("select count(1) from wl_operticketmanager d where d.platenumber='{0}' and Isdelete='1' and d.outdate is null and d.id!='{1}'", Number, keyValue);
            }
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            if (dt.Rows[0][0].ToString() != "0")
            {
                res = "1";
            }
            return res;
        }

        /// <summary>
        /// ���ɵ�ǰ��Ʊ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetTicketNumber(string product, string transportType, string takeGoodsName)
        {
            string number;
            lock (NumberLock)
            {
                number = operticketmanagerbll.GetTicketNumber(product, takeGoodsName, transportType);
            }
            return number;
        }

        /// <summary>
        /// ��ȡ���ϸ���Ʒ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProductType(string type, string key)
        {
            if (type == "1")
            {
                DataItemEntity entity = new DataItemBLL().GetEntityByCode("KmdcProductType");
                if (entity != null)
                {
                    string sql = string.Format("select d.itemname,d.itemcode,d.itemid from BASE_DATAITEM d where d.parentid='{0}' order by sortcode", entity.ItemId);
                    DataTable dt = operticketmanagerbll.GetDataTable(sql);
                    return dt.ToJson();
                }
            }
            else
            {
                string sql = string.Format("select d.itemname,d.itemvalue from BASE_DATAITEMDETAIL d  where d.itemid='{0}' order by sortcode", key);
                DataTable dt = operticketmanagerbll.GetDataTable(sql);
                return dt.ToJson();
            }
            return "";
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ���һ�ο�Ʊ��Ϣ
        /// </summary>
        /// <param name="plateNo"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetLastTicket(string plateNo)
        {
            OperticketmanagerEntity lastTicket = operticketmanagerbll.GetCar(plateNo);
            return lastTicket.ToJson();
        }

        /// <summary>
        /// ��ȡ��Ʊģ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetTemplate()
        {
            string sql = string.Format("SELECT * FROM WL_OPERTICKETTEMPLATE ");
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            return dt.Rows.Count > 0 ? dt.ToJson() : "";
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
            operticketmanagerbll.SaveForm(keyValue, entity);

            #region �޸�ʱ���³��ؼ�¼��Ϣ
            if (!string.IsNullOrEmpty(keyValue))
            {//�ÿ�Ʊ��¼�Ƿ����г��ؼ�¼��Ϣ
                string sql = string.Format("select id from wl_calculate d where d.baseid='{0}' and d.isdelete='1'", keyValue);
                DataTable dt = operticketmanagerbll.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    var data = calculatebll.GetEntity(dt.Rows[0][0].ToString());
                    if (data != null)
                    {
                        data.Platenumber = entity.Platenumber;
                        data.Takegoodsid = entity.Takegoodsid;
                        data.Takegoodsname = entity.Takegoodsname;
                        data.Transporttype = entity.Transporttype;
                        data.Goodsname = entity.Producttype;
                        calculatebll.SaveForm(data.ID, data);
                    }
                }
            }
            #endregion

            //��־
            if (string.IsNullOrEmpty(keyValue))
            {
                SaveDailyRecord(entity, "����");
                BindGPSEquipment(entity);
            }
            else
                SaveDailyRecord(entity, "�޸�");
            return Success("�����ɹ���", entity.ID);
        }

        /// <summary>
        /// ����˾����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity1"></param>
        /// <returns></returns>
        public ActionResult SaveDriverInfo(string keyValue, OperticketmanagerEntity entity1)
        {
            try
            {
                OperticketmanagerEntity entity = new OperticketmanagerBLL().GetEntity(keyValue);
                if (entity != null)
                {
                    entity.DriverName = entity1.DriverName;
                    entity.DriverTel = entity1.DriverTel;
                    entity.ExamineStatus = 1;
                    entity.JsImgpath = entity1.JsImgpath;
                    entity.ISwharf = entity1.ISwharf;
                    entity.XsImgpath = entity1.XsImgpath;
                    entity.IdentitetiImg = entity1.IdentitetiImg;
                    entity.HzWeight = entity1.HzWeight;
                    //new OperticketmanagerBLL().SaveForm(keyValue, entity);

                }


                return Success("�����ɹ���");
            }
            catch (Exception)
            {
                return Success("����ʧ�ܣ�");
            }
        }


        private void BindGPSEquipment(OperticketmanagerEntity entity)
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
            #endregion

            string key = CacheFactory.Cache().GetCache<string>("Hik:key");// "21049470";
            string sign = CacheFactory.Cache().GetCache<string>("Hik:sign");// "4gZkNoh3W92X6C66Rb6X";
            string baseUrl = CacheFactory.Cache().GetCache<string>("Hik:baseUrl");
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key))
            {
                var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//������������Կ                    
                if (!string.IsNullOrEmpty(pitem))
                {
                    key = pitem.Split('|')[0];
                    sign = pitem.Split('|')[1];
                    CacheFactory.Cache().WriteCache<string>(key, "Hik:key");
                    CacheFactory.Cache().WriteCache<string>(sign, "Hik:sign");
                }
            }
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//������������ַ
                CacheFactory.Cache().WriteCache<string>(baseUrl, "Hik:baseUrl");
            }

            string parkNames = "1�Ÿ�,���ŵذ�";
            entity.ExamineStatus = 3;
            if (!entity.Getdata.HasValue)
                entity.Getdata = DateTime.Now;

            operticketmanagerbll.SaveForm(entity.ID, entity);
            if (!string.IsNullOrEmpty(entity.GpsId))
            {

                CarAlgorithmEntity Car = new CarAlgorithmEntity();
                Car.CarNo = entity.Platenumber;
                Car.GPSID = entity.GpsId;
                Car.GPSName = entity.GpsName;
                Car.ID = entity.ID;
                Car.State = 0;
                Car.Type = 4;
                if (entity.Transporttype == "���")
                {
                    Car.LineName = entity.Dress + entity.Transporttype;
                    if (entity.ShipLoading == 1)
                    {
                        Car.LineName += "(��ͷ)";
                        parkNames += ",��ͷ��";
                    }
                }
                else
                {
                    if (entity.ShipLoading == 1)
                    {
                        Car.LineName = "����ת��(��ͷ)";
                        parkNames += ",��ͷ��";
                    }
                    else
                        Car.LineName = "ת��(������)";  
                }
                SocketHelper.SendMsg(Car.ToJson(), IP, Port);
            }
            //��������
            AddCarpermission(baseUrl, key, sign, entity.Platenumber, entity.DriverTel, entity.DriverName, parkNames);
        }

        #region ����ƽ̨�����ύ

        /// <summary>
        /// ��������ӽ���ͣ������Ȩ��
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <param name="CarNo"></param>
        /// <param name="Phone"></param>
        /// <param name="UserName"></param>
        /// <param name="parkName"></param>
        public void AddCarpermission(string Url, string key, string sign, string CarNo, string Phone, string UserName, string parkName = "1�Ÿ�")
        {


            #region ��鳵���ں���ƽ̨�Ƿ����
            var selectmodel = new
            {
                pageNo = 1,
                pageSize = 100,
                plateNo = CarNo
            };
            var existsVehicleStr = SocketHelper.LoadCameraList(selectmodel, Url, "/artemis/api/resource/v1/vehicle/advance/vehicleList", key, sign);
            dynamic existsVehicle = JsonConvert.DeserializeObject<ExpandoObject>(existsVehicleStr);
            #endregion
            var parkmodel = new
            {
                parkIndexCodes = ""
            };

            string parkMsg = SocketHelper.LoadCameraList(parkmodel, Url, "/artemis/api/resource/v1/park/parkList", key, sign);
            parkList pl = JsonConvert.DeserializeObject<parkList>(parkMsg);
            if (pl != null && pl.data != null && pl.data.Count > 0)
            {
                #region ����Ȩ�ޱ༭
                string[] parkNames = parkName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //������Ҫ������ͣ����
                List<string> pakindex = new List<string>();
                foreach (string pname in parkNames)
                {
                    pakindex.Add(pl.data.FirstOrDefault(x => x.parkName.Contains(pname))?.parkIndexCode);
                }
                if (existsVehicle.code == "0" && existsVehicle.data.total == 0)//���������ھ���������
                {
                    var addModel = new
                    {
                        plateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "���ϳ�",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")
                    };
                    SocketHelper.LoadCameraList(new List<dynamic>() { addModel }, Url, "/artemis/api/v1/vehicle/addVehicle", key, sign);
                }
                else if (existsVehicle.code == "0" && existsVehicle.data.total > 0)//�������ھ��޸ĳ���
                {
                    var updateModel = new
                    {
                        plateNo = CarNo,
                        oldPlateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "���ϳ�",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                        isUpdateFunction = 1
                    };
                    string updateMsg = SocketHelper.LoadCameraList(new List<dynamic>() { updateModel }, Url, "/artemis/api/v1/vehicle/updateVehicle", key, sign);
                }
                #endregion
            }
        }

        /// <summary>
        /// �Ƴ�����ʶ����Զ�̧�˷���Ȩ��
        /// </summary>
        /// <param name="CarNo"></param>
        public void RemoveCarpermission(string CarNo)
        {
            #region ɾ����������Ȩ��

            string key = CacheFactory.Cache().GetCache<string>("Hik:key");// "21049470";
            string sign = CacheFactory.Cache().GetCache<string>("Hik:sign");// "4gZkNoh3W92X6C66Rb6X";
            string baseUrl = CacheFactory.Cache().GetCache<string>("Hik:baseUrl");
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key))
            {
                var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//������������Կ                    
                if (!string.IsNullOrEmpty(pitem))
                {
                    key = pitem.Split('|')[0];
                    sign = pitem.Split('|')[1];
                    CacheFactory.Cache().WriteCache<string>(key, "Hik:key");
                    CacheFactory.Cache().WriteCache<string>(sign, "Hik:sign");
                }
            }
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//������������ַ
                CacheFactory.Cache().WriteCache<string>(baseUrl, "Hik:baseUrl");
            }

            if (!string.IsNullOrEmpty(CarNo))
            {
                var selectmodel = new
                {
                    pageNo = 1,
                    pageSize = 1,
                    plateNo = CarNo
                };
                var existsVehicleStr = SocketHelper.LoadCameraList(selectmodel, baseUrl, "/artemis/api/resource/v1/vehicle/advance/vehicleList", key, sign);
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
                    SocketHelper.LoadCameraList(delModel, baseUrl, "/artemis/api/resource/v1/vehicle/batch/delete", key, sign);
                }
            }
            #endregion

        }

        #endregion

        /// <summary>
        /// ɾ�����޸ļ�¼����״̬��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpateStatus(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                data.Isdelete = 0;
                data.PassRemark = entity.PassRemark;
                operticketmanagerbll.SaveForm(keyValue, data);
                RemoveCarpermission(data.Platenumber);
                SaveDailyRecord(data, "ɾ��");
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ӡ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        [HttpPost]
        public void SaveStampRecord(string keyValue)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                data.GetStampTime = DateTime.Now;
                data.OrderNumR = 1;
                operticketmanagerbll.SaveForm(keyValue, data);
                SaveDailyRecord(data, "��ӡ");
            }
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
            entity.WorkType = 1;
            entity.Theme = type;
            operticketmanagerbll.InsetDailyRecord(entity);
        }


        /// <summary>
        /// ��ӡʱ���ü�¼���ɶ�ά��ͼƬ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveImg(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                operticketmanagerbll.SaveForm(keyValue, data);
            }
            return Success("�����ɹ���");
        }


        #endregion
    }
}
