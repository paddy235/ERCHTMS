using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using System.Collections.Generic;
using ERCHTMS.Busines.MatterManage;
using System;
using System.Data;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Web.Areas.KbsDeviceManage.Models;
using Newtonsoft.Json;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 描 述：作业现场安全管控 
    /// </summary>
    public class SafeworkcontrolController : MvcControllerBase
    {
        private SafeworkcontrolBLL safeworkcontrolbll = new SafeworkcontrolBLL();
        private OperticketmanagerBLL Opertickebll = new OperticketmanagerBLL();
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
        /// 现场工作首页
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkHome()
        {
            return View();
        }

        /// <summary>
        /// 预警信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WarningInfo()
        {
            return View();
        }
        /// <summary>
        /// 预警信息详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WarningInfoForm()
        {
            return View();
        }
        /// <summary>
        /// 预警信息详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowArea()
        {
            return View();
        }
        /// <summary>
        /// 查看抓拍记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowRecordImg()
        {
            return View();
        }


        #endregion

        #region 获取数据


        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSafeworkItemJson(string itemcode)
        {
            List<ComboxsEntity> Rlist = new List<ComboxsEntity>();
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var list = pdata.GetDataItemListByItemCode(itemcode);
            if (list != null)
            {
                foreach (var item in list)
                {
                    ComboxsEntity y1 = new ComboxsEntity();
                    y1.itemName = item.ItemName;
                    y1.itemValue = item.ItemValue;
                    y1.Key = item.SimpleSpelling;
                    Rlist.Add(y1);
                }
            }
            return ToJsonResult(Rlist);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safeworkcontrolbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();    
            pagination.p_kid = "ID";
            pagination.p_fields = "workNo,taskName,taskType,taskManageName,taskRegionName,deptName,ActualStartTime,ActualEndTime,planenstarttime,planendtime,dangerlevel ";
            pagination.p_tablename = @" bis_SafeWorkControl ";
            pagination.conditionJson = " 1=1";

            var data = safeworkcontrolbll.GetPageList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safeworkcontrolbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取预警实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetWarningInfoEntity(string keyValue)
        {
            var data = safeworkcontrolbll.GetWarningInfoEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 首页获取各个班组异常数量统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWorkWarningGroupJson(string type)
        {
            DateTime stime = LinqHelper.Getmondydate(DateTime.Now);
            DateTime etime = LinqHelper.GetSundayDate(DateTime.Now); 
            string res = string.Empty;
            if (type == "本周") res = " and TO_CHAR(createdate,'yyyy-MM-dd')>='" + stime.ToString("yyyy-MM-dd") + "'  and TO_CHAR(createdate,'yyyy-MM-dd')<='" + etime.ToString("yyyy-MM-dd") + "'";
            else res = " and TO_CHAR(createdate,'yyyy-MM')='" + DateTime.Now.ToString("yyyy-MM") + "' ";

            string sql = string.Format("select d.deptname,count(1) as num from bis_earlywarning d where d.type=0 {0}  group by d.deptname,d.deptcode", res);
            var dt = Opertickebll.GetDataTable(sql);

            return Content(dt.ToJson());
        }

        /// <summary>
        /// 首页获取作业总数月统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWorkMonthGroupJson(int type)
        {
            //现场工作
            string sql = string.Format("select to_char(Actualstarttime,'yyyy-MM') as month, count(1) as num from bis_safeworkcontrol d where  to_char(Actualstarttime,'yyyy')={0} group  by  to_char(Actualstarttime,'yyyy-MM')", DateTime.Now.Year);
            if (type == 1)
            {//预警月统计
                sql = string.Format("select to_char(createdate,'yyyy-MM') as month, count(1) as num from bis_earlywarning d where type=0 and to_char(createdate,'yyyy')={0} group  by  to_char(createdate,'yyyy-MM')", DateTime.Now.Year);
            }
            DataTable dt = Opertickebll.GetDataTable(sql);
            List<int> list = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                bool boor = true;
                foreach (DataRow Rows in dt.Rows)
                {
                    var num = Rows[0].ToString().Split('-')[1];
                    int number = int.Parse(num);
                    if (number == i)
                    {
                        boor = false;
                        list.Add(int.Parse(Rows[1].ToString()));
                    }
                }
                if (boor)
                {
                    list.Add(0);
                }
            }
            return Content(list.ToJson());
        }

        /// <summary>
        /// 作业实时分布统计图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkRealTimeDistribution()
        {
            string sql = string.Format(@"select tasktype,count(1) as num from bis_safeworkcontrol d where (
(Actualstarttime<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  
ActualEndTime >=to_date('{0}','yyyy-mm-dd hh24:mi:ss')
)or( Actualstarttime<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  ActualEndTime is null)) and state=1  group by d.tasktype", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DataTable dt = Opertickebll.GetDataTable(sql);
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var list = pdata.GetDataItemListByItemCode("SafeWorkType");
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
            foreach (var item in list)
            {
                KbsEntity kbs = new KbsEntity();
                kbs.Name = item.ItemName;
                int num = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (item.ItemValue == dt.Rows[i][0].ToString())
                    {
                        num = Convert.ToInt32(dt.Rows[i]["num"]);
                        break;
                    }
                }
                kbs.Num = num;
                Znum += num;
                klist.Add(kbs);
            }
            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].Num / Znum;
                    Proportion = Proportion * 100;
                }
                klist[j].Proportion = Proportion.ToString("0.00") + "%";
            }
            dt.Dispose();
            return Content(klist.ToJson());
        }

        /// <summary>
        /// 统计图形显示信息
        /// </summary>
        /// <returns></returns>
        public string GetLableChart()
        {
            List<object[]> list = new List<object[]>();
            string sql = string.Format("select tasktype,count(1) as num from bis_safeworkcontrol d where Actualstarttime<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and ActualEndTime>=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and state=1  group by d.tasktype", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DataTable dt = Opertickebll.GetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object[] arr = { dt.Rows[i][0].ToString(), Convert.ToInt32(dt.Rows[i][1]) };
                list.Add(arr);
            }
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 作业实时分布统计表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkRealTimeTableJson()
        {
            DistrictBLL districtbll = new DistrictBLL();
            List<DistrictEntity> AreaList = districtbll.GetListByOrgIdAndParentId("", "0");
            List<SafeworkcontrolEntity> WorkList = safeworkcontrolbll.GetNowWork();
            List<KbssEntity> klist = new List<KbssEntity>();
            int Znum = 0;
            foreach (var item in AreaList)
            {
                KbssEntity kbs = new KbssEntity();
                kbs.Name = item.DistrictName;
                kbs.Num = WorkList.Where(a => a.Taskregioncode.Contains(item.DistrictCode)).Count();
                kbs.DistrictCode = item.DistrictCode;
                Znum += kbs.Num;
                kbs.OnNum = WorkList.Where(a => a.Taskregioncode.Contains(item.DistrictCode)).Count();
                klist.Add(kbs);
            }
            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].Num / Znum;
                    Proportion = Proportion * 100;
                }
                klist[j].OnNum = Znum;
                klist[j].OnProportion = Proportion.ToString("0") + "%";
            }
            return Content(klist.ToJson());
        }

        /// <summary>
        /// 获取作业抓拍列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRecordImg(string keyValue)
        {
            List<WorkcameracaptureEntity> list = new WorkcameracaptureBLL().GetCaptureList(keyValue, "", "");
            return ToJsonResult(list);
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
            safeworkcontrolbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafeworkcontrolEntity entity)
        {
            try
            {
                if (entity.State == 0)
                {//延期
                    var data = safeworkcontrolbll.GetEntity(keyValue);
                    if (data != null)
                    {
                        data.Planendtime = entity.Planendtime;
                        safeworkcontrolbll.AppSaveForm(keyValue, data);
                    }
                }
                 else if (entity.State == 1)
                {//提交作业
                    entity.comerid = GetElectricFenceCameraList(entity);
                    entity.Takeeffecttime = entity.Actualstarttime;
                    entity.Create();
                    safeworkcontrolbll.AppSaveForm(keyValue, entity);
                    //将现场工作信息同步到后台计算服务中
                    RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                    SendData sd = new SendData();
                    sd.DataName = "AddSafeworkcontrolEntity";
                    sd.EntityString = JsonConvert.SerializeObject(entity);
                    rh.SendMessage(JsonConvert.SerializeObject(sd));
                   
                }

                else if (entity.State == 2)
                {//结束作业
                    var data = safeworkcontrolbll.GetEntity(keyValue);
                    if (data != null)
                    {
                        data.ActualEndTime = entity.ActualEndTime;
                        data.State = entity.State;
                        data.Invalidtime = entity.ActualEndTime;
                        safeworkcontrolbll.SaveForm(data.ID, data);
                        //通知三维删除对应预警信息
                        RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                        var list = safeworkcontrolbll.GetBatchWarningInfoList(data.ID);
                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                if (item.type == 0 && item.NoticeType != 2)
                                {//人员未到岗
                                    SendTDWarning(3, item.WarningContent, item.LiableId, item.BaseId, rh);
                                }
                                else if (item.type == 0 && item.NoticeType == 2)
                                {//非作业人员误闯
                                    SendTDWarning(4, item.WarningContent, item.LiableId, item.Remark, rh);
                                }
                            }
                            //将现场工作信息同步到后台计算服务中
                            SendData sd = new SendData();
                            sd.DataName = "DelSafeworkcontrolEntity";
                            sd.EntityString = data.ID;
                            rh.SendMessage(JsonConvert.SerializeObject(sd));
                        }
                        else
                        {
                            //将现场工作信息同步到后台计算服务中
                            SendData sd = new SendData();
                            sd.DataName = "DelSafeworkcontrolEntity";
                            sd.EntityString = data.ID;
                            rh.SendMessage(JsonConvert.SerializeObject(sd));
                        }
                    }
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Success("操作失败！");
            }
        }




        /// <summary>
        /// 获取电子围栏内所有摄像头信息
        /// </summary>
        public string GetElectricFenceCameraList(SafeworkcontrolEntity data)
        {
            string ComerId = string.Empty;
            try
            {
                //三维监控点位服务接口地址
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var ThreeDApi = pdata.GetItemValue("kbsThreeMonitoringPoints");
                space sp = new space();
                spacedata sds = new spacedata();
                List<spacegeo> geolist = new List<spacegeo>();
                List<spacepnt> pntlist = new List<spacepnt>();
                var comList = new KbscameramanageBLL().GetPageList("").Where(a => a.State == "在线" && a.CameraType == "枪机" && a.MonitoringArea != null).ToList();
                spacegeo geo = new spacegeo();
                geo.id = data.ID;
                if (data.Areacode != null)
                {//电子围栏坐标点
                    PositionsEntity alist = JsonConvert.DeserializeObject<PositionsEntity>(data.Areacode);
                    List<double> dlist = new List<double>();
                    if (data.Areastate == 0)
                    {//矩形
                        geo.type = 1;
                        geo.distance = 0;
                        //第一个点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        //第二个点
                        dlist.Add(alist.positions[1].x);
                        dlist.Add(alist.positions[0].z);
                        //第三个点
                        dlist.Add(alist.positions[1].x);
                        dlist.Add(alist.positions[1].z);
                        //第四个点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[1].z);
                        //回到原点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.coor = dlist;
                        geolist.Add(geo);
                    }
                    else if (data.Areastate == 1)
                    {//圆形
                        geo.type = 0;
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.distance = Convert.ToInt32(data.Radius);
                        geo.coor = dlist;
                        geolist.Add(geo);
                    }
                    else if (data.Areastate == 2)
                    {//手绘多边形
                        foreach (var ac in alist.positions)
                        {
                            dlist.Add(ac.x);
                            dlist.Add(ac.z);
                        }
                        //闭合回到原点
                        dlist.Add(alist.positions[0].x);
                        dlist.Add(alist.positions[0].z);
                        geo.coor = dlist;
                        geo.type = 1;
                        geo.distance = 0;
                        geolist.Add(geo);
                    }

                    foreach (var com in comList)
                    {//摄像头坐标点集
                        PositionsEntity alist1 = JsonConvert.DeserializeObject<PositionsEntity>(com.MonitoringArea);
                        if (alist1.floorID == alist.floorID)
                        {
                            List<double> dlist1 = new List<double>();
                            foreach (var ac in alist1.positions)
                            {
                                dlist1.Add(ac.x);
                                dlist1.Add(ac.z);
                            }
                            //闭合回到原点
                            dlist1.Add(alist1.positions[0].x);
                            dlist1.Add(alist1.positions[0].z);
                            spacepnt pnt = new spacepnt()
                            {
                                id = com.CameraId,
                                x = 0,
                                y = 0,
                                coor = dlist1
                            };
                            pntlist.Add(pnt);
                        }
                    }
                }
                sds.geo = geolist;
                sds.pnt = pntlist;
                sp.type = "2";
                sp.data = sds;
                string rtn = HttpUtillibKbs.HttpThreeDPost(ThreeDApi, JsonConvert.SerializeObject(sp));
                List<RtnSpace> rss = JsonConvert.DeserializeObject<List<RtnSpace>>(rtn);
                foreach (var item in rss)
                {
                    if (string.IsNullOrEmpty(item.pntId)) continue;
                    ComerId += item.pntId + ',';
                }
                return ComerId;
            }
            catch (Exception er)
            {
                return ComerId;
            }
        }


        /// <summary>
        /// 发送给三维结束预警
        /// </summary>
        /// <param name="we"></param>
        public void SendTDWarning(int type, string warningContent, string liableId, string baseId, RabbitMQHelper rh)
        {
            try
            {
                WarningEntity we = new WarningEntity
                {
                    type = type,
                    WarningContent = warningContent,
                    LiableName = "",
                    LiableId = liableId,
                    BaseId = baseId,
                    deptCode = "",
                    deptName = "",
                    TaskName = "",
                    typeIds = ""
                };
                //发送给三维的时候改为标签ID
                SendData sd = new SendData();
                sd.DataName = "WarningEnd";
                sd.EntityString = JsonConvert.SerializeObject(we);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public ViewResult Edit(string id)
        {

            var items = dataItemDetailBLL.GetListItems("安全管控作业类型");
            ViewData["Tasktype"] = items.Select(x => new SelectListItem { Value = x.ItemName, Text = x.ItemName });

            items = dataItemDetailBLL.GetListItems("作业风险等级");
            ViewData["DangerLevel"] = items.Select(x => new SelectListItem { Value = x.ItemName, Text = x.ItemName });

            var model = new TaskModel
            {
                Actualstarttime = DateTime.Now
            };
            return View(model);
        }


        public ViewResult OpenMap()
        {
            return View();
        }



    }



    public static class LinqHelper
    {


        public static DateTime Getmondydate(this DateTime somedate)
        {
            int i = somedate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return somedate.Subtract(ts);
        }
        public static DateTime GetSundayDate(this DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }


    }

}
