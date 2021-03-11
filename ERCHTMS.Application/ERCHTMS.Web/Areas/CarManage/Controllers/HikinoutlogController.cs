using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Cache.Factory;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util.Offices;
using System.Text;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：设备记录人员进出日志
    /// </summary>
    public class HikinoutlogController : MvcControllerBase
    {
        private HikinoutlogBLL hikinoutlogbll = new HikinoutlogBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            ViewBag.KMHikImgIp = pdata.GetItemValue("KMHikImgIp");//海康图片访问ip地址
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
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "username,deptname,devicename,areaname,inout,devicetype,eventtype,screenshot,createdate";
            pagination.p_tablename = @"BIS_HIKINOUTLOG";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            //}
            //else
            //{




            //    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson += " and " + where;




            //}

            var data = hikinoutlogbll.GetPageList(pagination, queryJson);

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
        /// 获取全厂人数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPersonNums()
        {
            var dt = hikinoutlogbll.GetNums();
            List<string> eareaNames = new List<string>();
            eareaNames.Add("一号岗");
            eareaNames.Add("三号岗");
            eareaNames.Add("码头岗");
            List<int> userType = new List<int>();
            userType.Add(0);
            userType.Add(1);
            userType.Add(2);
            List<dynamic> PersonData = new List<dynamic>();
            foreach (int type in userType)
            {
                DataRow[] typeRows = dt.Select(string.Format(" type='{0}'", type));
                int station1 = 0, station2 = 0, station3 = 0;
                foreach (DataRow row in typeRows)
                {
                    if (row[2].ToString() == "一号岗")
                        station1 = Convert.ToInt32(row[1]);
                    if (row[2].ToString() == "三号岗")
                        station2 = Convert.ToInt32(row[1]);
                    if (row[2].ToString() == "码头岗")
                        station3 = Convert.ToInt32(row[1]);
                }
                PersonData.Add(new
                {
                    userType = type,
                    stationCount1 = station1,
                    stationCount2 = station2,
                    stationCount3 = station3,
                    total = station1 + station2 + station3
                });
            }
            var CarData = hikinoutlogbll.GetCarStatistic();

            var LastData = hikinoutlogbll.GetLastInoutLog();

            var returnData = new { PersonData, CarData, LastData };

            return Content(returnData.ToJson());
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikinoutlogbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        #region 设备间实时监控
        [HttpGet]
        public ActionResult DeviceWatch()
        {
            
            var cacheKey = "DeviceWatch";//缓存键的值
            var cacheService = CacheFactory.Cache();
            HikinoutlogEntity cacheValue = cacheService.GetCache<HikinoutlogEntity>(cacheKey);
            if (cacheValue == null )
            {
                cacheValue =    hikinoutlogbll.GetFirsetData();
                //写入缓存
                Task.Run(() =>
                {
                    cacheService.WriteCache(cacheValue, cacheKey, DateTime.Now.AddSeconds(6));
                });
            }
            return ToJsonResult(cacheValue);
        }
        #endregion
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
            hikinoutlogbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HikinoutlogEntity entity)
        {
            hikinoutlogbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
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
                pagination.p_kid = "ID";
                pagination.p_fields = @"username,deptname,devicename,areaname,inout,devicetype,eventtype,screenshot,createdate, 
                                                        case
                                                        when inout = 0 then '进门'
                                                        when inout = 1 then '出门'
                                                        else ''
                                                        end inoutname,
                                                        case
                                                        when EVENTTYPE = 1 then '人脸通过事件'
                                                        when EVENTTYPE = 2 then '车辆放行事件'
                                                        when EVENTTYPE = 3 then '门禁刷卡事件'
                                                        when EVENTTYPE = 4 then '门禁指纹通过事件'
                                                        else ''
                                                        end eventtypename ";
                pagination.p_tablename = @"BIS_HIKINOUTLOG";
                pagination.conditionJson = " 1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataTable exportTable = hikinoutlogbll.GetPageList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "人员进出记录";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "人员进出记录导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "username", ExcelColumn = "姓名", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "部门", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "devicename", ExcelColumn = "门禁点", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "areaname", ExcelColumn = "门禁点区域", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "inoutname", ExcelColumn = "出/入", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "eventtypename", ExcelColumn = "事件类型", Width = 20 });

                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
    }
}
