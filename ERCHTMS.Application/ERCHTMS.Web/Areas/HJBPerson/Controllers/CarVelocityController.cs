using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.KbsDeviceManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace ERCHTMS.Web.Areas.HJBPerson.Controllers
{
    /// <summary>
    /// 摄像头智能管控
    /// </summary>
    public class CarVelocityController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        private HikinoutlogBLL hikinoutlogbll = new HikinoutlogBLL();
        private DataItemDetailBLL pdata = new DataItemDetailBLL();
        private KbscameramanageBLL kbscameramanagebll = new KbscameramanageBLL();


        // GET: HJBPerson/CarVelocity
        #region 视图
        /// <summary>
        /// 车辆违章
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 安全帽预警
        /// </summary>
        /// <returns></returns>
        public ActionResult WaringList()
        {
            return View();
        }


        #endregion



        #region 获取数据
        /// <summary>
        /// 加载车辆超速记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "CARDNO,vehicleTypeName,DeptName,DIRVER,PHONE,SPEED,ADDRESS,CREATEDATE,VEHICLEPICURL,HIKPICSVR";
            pagination.p_tablename = "BIS_CARVIOLATION";
            pagination.conditionJson = "1=1";
            var data = hikinoutlogbll.Get_BIS_CARVIOLATION(pagination, queryJson);
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
       /// 请求海康服务器图片资源
       /// </summary>
       /// <param name="picSvr"></param>
       /// <param name="picPath"></param>
       /// <returns></returns>
        public ActionResult GetHikImagePath(string picSvr, string picPath)
        {
            string imagePath = string.Empty;
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            var HikHttpsIP = pdata.GetItemValue("HikHttpsIP");//海康平台访问IP
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/video/v1/events/picture";//接口地址

            var model = new
            {
                svrIndexCode = picSvr,
                picUri = picPath,
                netProtocol = "http"
            };
            HttpUtillibKbs.SetPlatformInfo(Key, Signature, HikHttpsIP, 443, true);
            byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(model), 20);
            string UserMsg = System.Text.Encoding.UTF8.GetString(result);
            dynamic imageObj = JsonConvert.DeserializeObject<dynamic>(UserMsg);
            if (imageObj != null && imageObj.code.ToString()=="0")             
                imagePath = imageObj.data.picUrl;
            return Content(imagePath);
        }

        /// <summary>
        /// 安全帽预警
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetWaringListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = " type,warningcontent,deptname,liablename,baseid,createdate,liableid,'' as camid ";
            pagination.p_tablename = "bis_earlywarning ";
            pagination.conditionJson = "1=1 ";
            var data = hikinoutlogbll.Get_BIS_CARVIOLATION(pagination, queryJson);
            if (data.Rows.Count > 0)
            {
                var list = kbscameramanagebll.GetList("").ToList();
                foreach (DataRow item in data.Rows)
                {
                    var CamIp = item["liableid"].ToString();
                    var entity = list.Where(a => a.CameraIP == CamIp).FirstOrDefault();
                    if (entity != null)
                    {//摄像头唯一Id（调用实时画面）
                        item["camid"] = entity.CameraId;
                    }
                }
            }

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



        #region 导出数据
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        /// 
        [HandlerMonitor(0, "车辆测速数据")]
        public ActionResult ExportData(Pagination pagination, string queryJson)
        {
            try {
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "ID";
                pagination.p_fields = "CARDNO,vehicleTypeName,DeptName,DIRVER,PHONE,SPEED,ADDRESS,CREATEDATE,VEHICLEPICURL,TO_CHAR(CREATEDATE,'yyyy-mm-dd hh24:mi:ss') as datetime";
                pagination.p_tablename = "BIS_CARVIOLATION";
                pagination.conditionJson = "1=1";
                
                pagination.sidx = "CreateDate";
                pagination.sord = "desc";
                var data = hikinoutlogbll.Get_BIS_CARVIOLATION(pagination, queryJson);

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "车辆测速数据信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "车辆测速数据导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "cardno".ToLower(), ExcelColumn = "车牌号码" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "vehicleTypeName".ToLower(), ExcelColumn = "车辆类型" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "所属单位（部门）" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dirver".ToLower(), ExcelColumn = "驾驶员" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "phone".ToLower(), ExcelColumn = "驾驶员电话" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "speed".ToLower(), ExcelColumn = "抓拍车速（km/h）" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "address".ToLower(), ExcelColumn = "抓拍地点" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "datetime".ToLower(), ExcelColumn = "时间" });
                //调用导出方法
                ExcelHelper.ExcelDownload(data, excelconfig);
            }
            catch (Exception ex) {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "安全帽预警数据")]
        public ActionResult ExportUserData(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "ID";
                pagination.p_fields = "warningcontent,deptname,liablename,baseid,TO_CHAR(createdate,'yyyy-mm-dd hh24:mi:ss') as datetime";
                pagination.p_tablename = "bis_earlywarning";
                pagination.conditionJson = "1=1";
                var data = hikinoutlogbll.Get_BIS_CARVIOLATION(pagination, queryJson);

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全帽预警数据信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "安全帽预警数据信息.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "warningcontent".ToLower(), ExcelColumn = "预警内容" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "所属单位（部门）" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "liablename".ToLower(), ExcelColumn = "责任人" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "datetime".ToLower(), ExcelColumn = "预警时间" });
                //调用导出方法
                ExcelHelper.ExcelDownload(data, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }


        #endregion

    }
}