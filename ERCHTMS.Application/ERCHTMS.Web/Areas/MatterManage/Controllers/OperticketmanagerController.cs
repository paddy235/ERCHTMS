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
    /// 描 述：开票管理入厂开票
    /// </summary>
    public class OperticketmanagerController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        private CalculateBLL calculatebll = new CalculateBLL();
        private DataItemBLL dataItemBLL = new DataItemBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

        private object NumberLock = new object();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 司机完善信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditDriverInfo()
        {
            return View();
        }

        /// <summary>
        /// 选择开票模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TemplateSelect()
        {
            return View();
        }

        /// <summary>
        /// 打印视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp(string keyValue)
        {
            string sql = string.Format("select sum(netwneight) from wl_calculate  where isdelete='1' and  baseid='{0}'", keyValue);
            DataTable dt = operticketmanagerbll.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {//净重
                ViewBag.weight = dt.Rows[0][0].ToString();
            }
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取入场开票工作记录
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
        /// 获取司机上传开票信息
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
        ///判断是否有相同车辆未出场纪录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetisNotOutvehicle(string keyValue, string Number)
        {
            string res = string.Empty;
            string sql = string.Format("select count(1) from wl_operticketmanager d where d.platenumber='{0}' and Isdelete='1' and d.outdate is null ", Number);
            if (!string.IsNullOrEmpty(keyValue))
            {//修改时排除自身
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
        /// 生成当前开票单号
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
        /// 获取物料副产品类型
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
        /// 根据车牌号获取最后一次开票信息
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
        /// 获取开票模板配置
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

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            operticketmanagerbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, OperticketmanagerEntity entity)
        {
            operticketmanagerbll.SaveForm(keyValue, entity);

            #region 修改时更新称重记录信息
            if (!string.IsNullOrEmpty(keyValue))
            {//该开票记录是否已有称重记录信息
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

            //日志
            if (string.IsNullOrEmpty(keyValue))
            {
                SaveDailyRecord(entity, "新增");
                BindGPSEquipment(entity);
            }
            else
                SaveDailyRecord(entity, "修改");
            return Success("操作成功。", entity.ID);
        }

        /// <summary>
        /// 完善司机信息
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


                return Success("操作成功。");
            }
            catch (Exception)
            {
                return Success("操作失败！");
            }
        }


        private void BindGPSEquipment(OperticketmanagerEntity entity)
        {
            #region 定位数据发送
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
                var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//海康服务器密钥                    
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
                baseUrl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//海康服务器地址
                CacheFactory.Cache().WriteCache<string>(baseUrl, "Hik:baseUrl");
            }

            string parkNames = "1号岗,二号地磅";
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
                if (entity.Transporttype == "提货")
                {
                    Car.LineName = entity.Dress + entity.Transporttype;
                    if (entity.ShipLoading == 1)
                    {
                        Car.LineName += "(码头)";
                        parkNames += ",码头岗";
                    }
                }
                else
                {
                    if (entity.ShipLoading == 1)
                    {
                        Car.LineName = "物料转运(码头)";
                        parkNames += ",码头岗";
                    }
                    else
                        Car.LineName = "转运(纯称重)";  
                }
                SocketHelper.SendMsg(Car.ToJson(), IP, Port);
            }
            //车辆放行
            AddCarpermission(baseUrl, key, sign, entity.Platenumber, entity.DriverTel, entity.DriverName, parkNames);
        }

        #region 海康平台数据提交

        /// <summary>
        /// 给车辆添加进入停车场的权限
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <param name="CarNo"></param>
        /// <param name="Phone"></param>
        /// <param name="UserName"></param>
        /// <param name="parkName"></param>
        public void AddCarpermission(string Url, string key, string sign, string CarNo, string Phone, string UserName, string parkName = "1号岗")
        {


            #region 检查车辆在海康平台是否存在
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
                #region 车辆权限编辑
                string[] parkNames = parkName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //车辆需要经过的停车场
                List<string> pakindex = new List<string>();
                foreach (string pname in parkNames)
                {
                    pakindex.Add(pl.data.FirstOrDefault(x => x.parkName.Contains(pname))?.parkIndexCode);
                }
                if (existsVehicle.code == "0" && existsVehicle.data.total == 0)//车辆不存在就新增车辆
                {
                    var addModel = new
                    {
                        plateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "物料车",
                        parkIndexCode = string.Join(",", pakindex),
                        startTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")
                    };
                    SocketHelper.LoadCameraList(new List<dynamic>() { addModel }, Url, "/artemis/api/v1/vehicle/addVehicle", key, sign);
                }
                else if (existsVehicle.code == "0" && existsVehicle.data.total > 0)//车辆存在就修改车辆
                {
                    var updateModel = new
                    {
                        plateNo = CarNo,
                        oldPlateNo = CarNo,
                        plateType = 0,
                        plateColor = 1,
                        carType = 2,
                        carColor = 0,
                        mark = "物料车",
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
        /// 移除车辆识别后自动抬杆放行权限
        /// </summary>
        /// <param name="CarNo"></param>
        public void RemoveCarpermission(string CarNo)
        {
            #region 删除车辆进出权限

            string key = CacheFactory.Cache().GetCache<string>("Hik:key");// "21049470";
            string sign = CacheFactory.Cache().GetCache<string>("Hik:sign");// "4gZkNoh3W92X6C66Rb6X";
            string baseUrl = CacheFactory.Cache().GetCache<string>("Hik:baseUrl");
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key))
            {
                var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//海康服务器密钥                    
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
                baseUrl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//海康服务器地址
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
        /// 删除（修改记录可用状态）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
                SaveDailyRecord(data, "删除");
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 打印记录
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
                SaveDailyRecord(data, "打印");
            }
        }

        /// <summary>
        /// 保存工作日志
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
        /// 打印时将该记录生成二维码图片
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveImg(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                operticketmanagerbll.SaveForm(keyValue, data);
            }
            return Success("操作成功。");
        }


        #endregion
    }
}
