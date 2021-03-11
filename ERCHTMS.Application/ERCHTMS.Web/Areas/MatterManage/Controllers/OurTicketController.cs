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
    /// 描 述：开票管理入厂开票
    /// </summary>
    public class OurTicketController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        private CalculateBLL calculatebll = new CalculateBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

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
        /// 详情页面视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowForm(string keyValue)
        {
            return View();
        }

        /// <summary>
        /// 过程管理记录视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NotesForm()
        {
            return View();
        }

        /// <summary>
        /// 过程管理车辆厂内行驶路线
        /// </summary>
        /// <returns></returns>
        public ActionResult NotesItinerary()
        {
            return View();
        }


        /// <summary>
        /// 打印视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp()
        {
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
        /// 获取查看过程管理实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetProcessFormJson(string keyValue)
        {
            var data = operticketmanagerbll.GetProcessEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取最新一条记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetNewFormJson(string keyValue)
        {
            var data = operticketmanagerbll.GetNewEntity("");
            return ToJsonResult(data);
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
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                data.Remark = entity.Remark;
                operticketmanagerbll.SaveForm(keyValue, data);
                SaveDailyRecord(data, "修改");
            }
            return Success("操作成功。");
        }


        /// <summary>
        /// 打印时同步修改出厂时间
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
                    //SaveDailyRecord(data, "打印");
                }
                data.OutDatabasetime = DateTime.Now;
                //else SaveDailyRecord(data, "再次打印");
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
            return Success("操作成功。", data);
        }

        /// <summary>
        /// 车辆出厂时对应称重记录不能再修改
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
        /// 车辆异常放行备注
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
                    data.Status = "正常";
                    data.PassRemark = data.PassRemark + "(正常放行)";
                }
                operticketmanagerbll.SaveForm(keyValue, data);
                if (!data.OutDate.HasValue)
                    UpateOutTime(keyValue, data);//异常放行相当于车辆出厂
                RemoveCarpermission(data.Platenumber);
                ReleaseGPSEquipment(keyValue);
            }

            return Success("操作成功。", data);
        }

        /// <summary>
        /// 移除车辆识别后自动抬杆放行权限
        /// </summary>
        /// <param name="CarNo"></param>
        public void RemoveCarpermission(string CarNo)
        {
            #region 删除车辆进出权限

            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            var pitem = dataItemDetailBLL.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = dataItemDetailBLL.GetItemValue("HikBaseUrl");//海康服务器地址
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
            CarAlgorithmEntity car = new CarAlgorithmEntity();
            car.ID = keyValue;
            car.State = 1;
            SocketHelper.SendMsg(car.ToJson(), IP, Port);

            #endregion
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
            entity.WorkType = 2;
            entity.Theme = type;
            operticketmanagerbll.InsetDailyRecord(entity);
        }


        #endregion

        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
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

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "车辆出厂";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "车辆出厂.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "numbers", ExcelColumn = "提货/转运单号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ticketno", ExcelColumn = "开票号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "takegoodsname", ExcelColumn = "提货方", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "platenumber", ExcelColumn = "车牌号", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "transporttype", ExcelColumn = "运输类型", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "producttype", ExcelColumn = "副产品类型", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dress", ExcelColumn = "装灰点", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "zcount", ExcelColumn = "车数", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "znetwneight", ExcelColumn = "重量（吨）", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "getdata", ExcelColumn = "进厂时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "outdate", ExcelColumn = "出厂时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "staytime", ExcelColumn = "厂内逗留时间(分钟)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "status", ExcelColumn = "厂内状态", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "passremark", ExcelColumn = "备注", Width = 30 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception)
            {

            }
            return Success("导出成功。");
        }
    }
}
