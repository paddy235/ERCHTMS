using System;
using System.Collections.Generic;
using System.Data;
using System.Web.ModelBinding;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：车辆路线配置树
    /// </summary>
    public class RouteconfigController : MvcControllerBase
    {
        private RouteconfigBLL routeconfigbll = new RouteconfigBLL();
        private VisitcarBLL visitbll = new VisitcarBLL();
        private HazardouscarBLL hazarbll = new HazardouscarBLL();
        private OperticketmanagerBLL operbll = new OperticketmanagerBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == "RouteConfig")
                {
                    ViewBag.Url = dt.Rows[i][1].ToString();
                }
            }
            dt.Dispose();
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
        /// 拜访路线页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VisitRoute()
        {
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == "RouteConfig")
                {
                    ViewBag.Url = dt.Rows[i][1].ToString();
                }
            }
            dt.Dispose();
            return View();
        }

        /// <summary>
        /// 选择路线界面
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectLine()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取路线集合
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLineList()
        {
            var routelist=routeconfigbll.RouteDropdown();
            return Content(routelist.ToJson());
        }


        [HttpGet]
        public string GetCarPoint(string id, string type)
        {
            string sn = "";
            string startTime = "";
            string endTime = "";
            string Url = new DataItemDetailBLL().GetItemValue("IOTUrl");
            switch (type)
            {
                case "0":
                    break;
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    var visitcar = visitbll.GetEntity(id);
                    sn = visitcar.GPSID;
                    if (visitcar.InTime != null)
                    {
                        startTime = Convert.ToDateTime(visitcar.InTime).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        startTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    if (visitcar.OutTime != null)
                    {
                        endTime = Convert.ToDateTime(visitcar.InTime).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        endTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    break;
                case "4":
                    var opercar = operbll.GetEntity(id);
                    sn = opercar.GpsId;
                    if (opercar.Getdata != null)
                    {
                        startTime = Convert.ToDateTime(opercar.Getdata).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        startTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    if (opercar.OutDate != null)
                    {
                        endTime = Convert.ToDateTime(opercar.OutDate).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        endTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    break;
                case "5":
                    var hazardouscar = hazarbll.GetEntity(id);
                    sn = hazardouscar.GPSID;
                    if (hazardouscar.InTime != null)
                    {
                        startTime = Convert.ToDateTime(hazardouscar.InTime).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        startTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    if (hazardouscar.OutTime != null)
                    {
                        endTime = Convert.ToDateTime(hazardouscar.InTime).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        endTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    break;

            }
            string json = "{\"sn\": \"" + sn + "\",\"startTime\": \"" + startTime + "\",\"endTime\": \"" + endTime + "\"}";
            string msg = HttpCommon.HttpPostJson(Url + "/services/app/GpsCar/GetDeviceGpsData", json);
            IotCar car = JsonConvert.DeserializeObject<IotCar>(msg);
            GpsList gps = new GpsList();
            List<GpsPoint> PointList = new List<GpsPoint>();
            foreach (var carGpsData in car.result)
            {
                GpsPoint point = new GpsPoint();
                point.X = Convert.ToDouble(carGpsData.Latitude);
                point.Y = Convert.ToDouble(carGpsData.Longitude);
                point.Z = 200;
                PointList.Add(point);
            }

            gps.ID = id;
            gps.data = PointList;
            return gps.ToJson();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = routeconfigbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = routeconfigbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据ID 获取其中路线Json
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPointJson(string ID)
        {
            string json = routeconfigbll.GetEntity(ID).PointList;
            return json;
        }



        [HttpGet]
        public ActionResult GetTreeJson(string json)
        {
            var data = routeconfigbll.GetTree(0);
            //如果是0则初始化数据
            if (data.Count == 0)
            {
                data = IniRoute();
            }
            string parentId = "0";

            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (RouteconfigEntity item in data)
            {
                bool frist = true;
                TreeEntity tree = new TreeEntity();
                bool hasChildren = false;
                if (item.Level < 3)
                {
                    hasChildren = true;
                }
                else
                {
                    hasChildren = false;
                }
                tree.id = item.ID;
                if (item.IsEnable == 1)
                {
                    tree.text = item.ItemName + "(启用)";
                }
                else
                {
                    tree.text = item.ItemName;
                }

                tree.value = item.GID;
                ////是否是外包工程承包商节点,部分界面涉及不选此节点 update 2019.05.14
                //tree.img = item.Description == "外包工程承包商" ? "1" : "0";
                if (item.Level != 3)
                {
                    tree.isexpand = true;
                }
                else
                {
                    tree.isexpand = false;
                }
                tree.complete = true;
                if (item.Level == 3 && frist)
                {
                    tree.checkstate = 1;
                    frist = false;
                }
                else
                {
                    tree.checkstate = 0;
                }

                tree.showcheck = false;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.Attribute = "Level";
                tree.AttributeValue = item.Level.ToString();
                tree.AttributeB = "IsEnable";
                tree.AttributeValueB = item.IsEnable.ToString();
                tree.AttributeA = "IsPier";
                tree.AttributeValueA = item.IsPier.ToString();
                //tree.AttributeC = "PointJson";
                //tree.AttributeValueC = item.PointList;


                treeList.Add(tree);
            }


            return Content(treeList.TreeToJson(parentId));

        }

        /// <summary>
        /// 获取拜访路线树节点
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetVisitTreeJson(string json)
        {
            var data = routeconfigbll.GetTree(1);
            //如果是0则初始化数据
            if (data.Count == 0)
            {
                data = IniVisitRoute();
            }
            string parentId = "0";

            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (RouteconfigEntity item in data)
            {
                bool frist = true;
                TreeEntity tree = new TreeEntity();
                bool hasChildren = false;
                if (item.Level < 3)
                {
                    hasChildren = true;
                }
                else
                {
                    hasChildren = false;
                }
                tree.id = item.ID;
                //if (item.IsEnable == 1)
                //{
                //    tree.text = item.ItemName + "(启用)";
                //}
                //else
                //{
                    tree.text = item.ItemName;
                //}

                tree.value = item.GID;
                ////是否是外包工程承包商节点,部分界面涉及不选此节点 update 2019.05.14
                //tree.img = item.Description == "外包工程承包商" ? "1" : "0";
                if (item.Level != 3)
                {
                    tree.isexpand = true;
                }
                else
                {
                    tree.isexpand = false;
                }
                tree.complete = true;
                if (item.Level == 3 && frist)
                {
                    tree.checkstate = 1;
                    frist = false;
                }
                else
                {
                    tree.checkstate = 0;
                }

                tree.showcheck = false;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.Attribute = "Level";
                tree.AttributeValue = item.Level.ToString();
                tree.AttributeB = "IsEnable";
                tree.AttributeValueB = item.IsEnable.ToString();
                tree.AttributeA = "IsPier";
                tree.AttributeValueA = item.IsPier.ToString();
                //tree.AttributeC = "PointJson";
                //tree.AttributeValueC = item.PointList;


                treeList.Add(tree);
            }


            return Content(treeList.TreeToJson(parentId));

        }

        public List<RouteconfigEntity> IniVisitRoute()
        {
            List<RouteconfigEntity> rlist = new List<RouteconfigEntity>();

            //拜访车辆
            RouteconfigEntity Visit = new RouteconfigEntity();
            Visit.Level = 1;
            Visit.GID = "";
            Visit.IsEnable = 0;
            Visit.IsPier = 0;
            Visit.ParentId = "0";
            Visit.ItemName = "拜访车辆";
            Visit.PointList = "";
            Visit.Sort = 3;
            Visit.LineType = 1;
            Visit.Create();
            rlist.Add(Visit);

            RouteconfigEntity Visitdetail = new RouteconfigEntity();
            Visitdetail.Level = 2;
            Visitdetail.GID = "Visit";
            Visitdetail.IsEnable = 0;
            Visitdetail.IsPier = 0;
            Visitdetail.ParentId = Visit.ID;
            Visitdetail.ItemName = "拜访路线";
            Visitdetail.PointList = "";
            Visitdetail.Sort = 1;
            Visitdetail.LineType = 1;
            Visitdetail.Create();
            rlist.Add(Visitdetail);

            //RouteconfigEntity VisitLine1 = new RouteconfigEntity();
            //VisitLine1.Level = 3;
            //VisitLine1.GID = "Visit";
            //VisitLine1.IsEnable = 0;
            //VisitLine1.IsPier = 0;
            //VisitLine1.ParentId = Visitdetail.ID;
            //VisitLine1.ItemName = "1号路线";
            //VisitLine1.PointList = "";
            //VisitLine1.Sort = 1;
            //VisitLine1.Create();

            //RouteconfigEntity VisitLine2 = new RouteconfigEntity();
            //VisitLine2.Level = 3;
            //VisitLine2.GID = "Visit";
            //VisitLine2.IsEnable = 0;
            //VisitLine2.IsPier = 0;
            //VisitLine2.ParentId = Visitdetail.ID;
            //VisitLine2.ItemName = "2号路线";
            //VisitLine2.PointList = "";
            //VisitLine2.Sort = 2;
            //VisitLine2.Create();

            //rlist.Add(VisitLine1);
            //rlist.Add(VisitLine2);


            

            routeconfigbll.SaveList(rlist);

            return rlist;
        }

        public List<RouteconfigEntity> IniRoute()
        {
            List<RouteconfigEntity> rlist = new List<RouteconfigEntity>();
            //危化品
            RouteconfigEntity Hazadous = new RouteconfigEntity();
            Hazadous.Level = 1;
            Hazadous.GID = "";
            Hazadous.IsEnable = 0;
            Hazadous.IsPier = 0;
            Hazadous.ParentId = "0";
            Hazadous.ItemName = "危化品车辆路线";
            Hazadous.PointList = "";
            Hazadous.Sort = 1;
            Hazadous.LineType = 0;
            Hazadous.Create();
            rlist.Add(Hazadous);
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            //根据危化品类型添加二级节点
            var data = dataItemDetailBLL.GetDataItemListByItemCode("'HazardousCar'");
            int i = 1;
            foreach (var item in data)
            {
                RouteconfigEntity Hazadousdetail = new RouteconfigEntity();
                Hazadousdetail.Level = 2;
                Hazadousdetail.GID = item.ItemDetailId;
                Hazadousdetail.IsEnable = 0;
                Hazadousdetail.IsPier = 0;
                Hazadousdetail.ParentId = Hazadous.ID;
                Hazadousdetail.ItemName = item.ItemName;
                Hazadousdetail.PointList = "";
                Hazadousdetail.Sort = i;
                Hazadousdetail.LineType = 0;
                Hazadousdetail.Create();
                i++;
                rlist.Add(Hazadousdetail);
                RouteconfigEntity HazadousLine1 = new RouteconfigEntity();
                HazadousLine1.Level = 3;
                HazadousLine1.GID = item.ItemDetailId;
                HazadousLine1.IsEnable = 0;
                HazadousLine1.IsPier = 0;
                HazadousLine1.ParentId = Hazadousdetail.ID;
                HazadousLine1.ItemName = "1号路线";
                HazadousLine1.PointList = "";
                HazadousLine1.Sort = 1;
                HazadousLine1.LineType = 0;
                HazadousLine1.Create();

                RouteconfigEntity HazadousLine2 = new RouteconfigEntity();
                HazadousLine2.Level = 3;
                HazadousLine2.GID = item.ItemDetailId;
                HazadousLine2.IsEnable = 0;
                HazadousLine2.IsPier = 0;
                HazadousLine2.ParentId = Hazadousdetail.ID;
                HazadousLine2.ItemName = "2号路线";
                HazadousLine2.PointList = "";
                HazadousLine2.Sort = 2;
                HazadousLine2.LineType = 0;
                HazadousLine2.Create();

                rlist.Add(HazadousLine1);
                rlist.Add(HazadousLine2);
            }


            RouteconfigEntity WlCar = new RouteconfigEntity();
            WlCar.Level = 1;
            WlCar.GID = "";
            WlCar.IsEnable = 0;
            WlCar.IsPier = 0;
            WlCar.ParentId = "0";
            WlCar.ItemName = "物料销售车辆";
            WlCar.PointList = "";
            WlCar.Sort = 2;
            WlCar.LineType = 0;
            WlCar.Create();
            rlist.Add(WlCar);

            var wllist = routeconfigbll.GetWlList();
            i = 1;
            foreach (var item in wllist)
            {
                RouteconfigEntity WlCardetail = new RouteconfigEntity();
                WlCardetail.Level = 2;
                WlCardetail.GID = item.ItemDetailId;
                WlCardetail.IsEnable = 0;
                WlCardetail.IsPier = 0;
                WlCardetail.ParentId = WlCar.ID;
                WlCardetail.ItemName = item.ItemName + "提货";
                WlCardetail.PointList = "";
                WlCardetail.Sort = i;
                WlCardetail.LineType = 0;
                WlCardetail.Create();
                rlist.Add(WlCardetail);
                i++;
                RouteconfigEntity WlLine1 = new RouteconfigEntity();
                WlLine1.Level = 3;
                WlLine1.GID = item.ItemDetailId;
                WlLine1.IsEnable = 0;
                WlLine1.IsPier = 0;
                WlLine1.ParentId = WlCardetail.ID;
                WlLine1.ItemName = "1号路线";
                WlLine1.PointList = "";
                WlLine1.Sort = 1;
                WlLine1.LineType = 0;
                WlLine1.Create();

                RouteconfigEntity WlLine2 = new RouteconfigEntity();
                WlLine2.Level = 3;
                WlLine2.GID = item.ItemDetailId;
                WlLine2.IsEnable = 0;
                WlLine2.IsPier = 0;
                WlLine2.ParentId = WlCardetail.ID;
                WlLine2.ItemName = "2号路线";
                WlLine2.PointList = "";
                WlLine2.Sort = 2;
                WlLine2.LineType = 0;
                WlLine2.Create();

                rlist.Add(WlLine1);
                rlist.Add(WlLine2);
            }

            foreach (var item in wllist)
            {
                RouteconfigEntity WlCarMtdetail = new RouteconfigEntity();
                WlCarMtdetail.Level = 2;
                WlCarMtdetail.GID = item.ItemDetailId + "MT";
                WlCarMtdetail.IsEnable = 0;
                WlCarMtdetail.IsPier = 0;
                WlCarMtdetail.ParentId = WlCar.ID;
                WlCarMtdetail.ItemName = item.ItemName + "提货(码头)";
                WlCarMtdetail.PointList = "";
                WlCarMtdetail.Sort = i;
                WlCarMtdetail.LineType = 0;
                WlCarMtdetail.Create();
                rlist.Add(WlCarMtdetail);
                i++;
                RouteconfigEntity WlLine1 = new RouteconfigEntity();
                WlLine1.Level = 3;
                WlLine1.GID = item.ItemDetailId + "MT";
                WlLine1.IsEnable = 0;
                WlLine1.IsPier = 0;
                WlLine1.ParentId = WlCarMtdetail.ID;
                WlLine1.ItemName = "1号路线";
                WlLine1.PointList = "";
                WlLine1.Sort = 1;
                WlLine1.LineType = 0;
                WlLine1.Create();

                RouteconfigEntity WlLine2 = new RouteconfigEntity();
                WlLine2.Level = 3;
                WlLine2.GID = item.ItemDetailId + "MT";
                WlLine2.IsEnable = 0;
                WlLine2.IsPier = 0;
                WlLine2.ParentId = WlCarMtdetail.ID;
                WlLine2.ItemName = "2号路线";
                WlLine2.PointList = "";
                WlLine2.Sort = 2;
                WlLine2.LineType = 0;
                WlLine2.Create();

                rlist.Add(WlLine1);
                rlist.Add(WlLine2);
            }

            RouteconfigEntity WlZydetail = new RouteconfigEntity();
            WlZydetail.Level = 2;
            WlZydetail.GID = "Zy";
            WlZydetail.IsEnable = 0;
            WlZydetail.IsPier = 0;
            WlZydetail.ParentId = WlCar.ID;
            WlZydetail.ItemName = "转运(纯称重)";
            WlZydetail.PointList = "";
            WlZydetail.Sort = i;
            WlZydetail.LineType = 0;
            WlZydetail.Create();
            rlist.Add(WlZydetail);
            i++;
            RouteconfigEntity WlZyLine1 = new RouteconfigEntity();
            WlZyLine1.Level = 3;
            WlZyLine1.GID = "Zy";
            WlZyLine1.IsEnable = 0;
            WlZyLine1.IsPier = 0;
            WlZyLine1.ParentId = WlZydetail.ID;
            WlZyLine1.ItemName = "1号路线";
            WlZyLine1.PointList = "";
            WlZyLine1.Sort = 1;
            WlZyLine1.LineType = 0;
            WlZyLine1.Create();

            RouteconfigEntity WlZyLine2 = new RouteconfigEntity();
            WlZyLine2.Level = 3;
            WlZyLine2.GID = "Zy";
            WlZyLine2.IsEnable = 0;
            WlZyLine2.IsPier = 0;
            WlZyLine2.ParentId = WlZydetail.ID;
            WlZyLine2.ItemName = "2号路线";
            WlZyLine2.PointList = "";
            WlZyLine2.Sort = 2;
            WlZyLine2.LineType = 0;
            WlZyLine2.Create();

            rlist.Add(WlZyLine1);
            rlist.Add(WlZyLine2);


            RouteconfigEntity WlZyMtdetail = new RouteconfigEntity();
            WlZyMtdetail.Level = 2;
            WlZyMtdetail.GID = "ZyMt";
            WlZyMtdetail.IsEnable = 0;
            WlZyMtdetail.IsPier = 0;
            WlZyMtdetail.ParentId = WlCar.ID;
            WlZyMtdetail.ItemName = "物料转运(码头)";
            WlZyMtdetail.PointList = "";
            WlZyMtdetail.Sort = i;
            WlZyMtdetail.LineType = 0;
            WlZyMtdetail.Create();
            rlist.Add(WlZyMtdetail);
            i++;
            RouteconfigEntity WlZyMtLine1 = new RouteconfigEntity();
            WlZyMtLine1.Level = 3;
            WlZyMtLine1.GID = "ZyMt";
            WlZyMtLine1.IsEnable = 0;
            WlZyMtLine1.IsPier = 0;
            WlZyMtLine1.ParentId = WlZyMtdetail.ID;
            WlZyMtLine1.ItemName = "1号路线";
            WlZyMtLine1.PointList = "";
            WlZyMtLine1.Sort = 1;
            WlZyMtLine1.LineType = 0;
            WlZyMtLine1.Create();

            RouteconfigEntity WlZyMtLine2 = new RouteconfigEntity();
            WlZyMtLine2.Level = 3;
            WlZyMtLine2.GID = "ZyMt";
            WlZyMtLine2.IsEnable = 0;
            WlZyMtLine2.IsPier = 0;
            WlZyMtLine2.ParentId = WlZyMtdetail.ID;
            WlZyMtLine2.ItemName = "2号路线";
            WlZyMtLine2.PointList = "";
            WlZyMtLine2.Sort = 2;
            WlZyMtLine2.LineType = 0;
            WlZyMtLine2.Create();

            rlist.Add(WlZyMtLine1);
            rlist.Add(WlZyMtLine2);


            ////拜访车辆
            //RouteconfigEntity Visit = new RouteconfigEntity();
            //Visit.Level = 1;
            //Visit.GID = "";
            //Visit.IsEnable = 0;
            //Visit.IsPier = 0;
            //Visit.ParentId = "0";
            //Visit.ItemName = "拜访车辆";
            //Visit.PointList = "";
            //Visit.Sort = 3;
            //Visit.Create();
            //rlist.Add(Visit);

            //RouteconfigEntity Visitdetail = new RouteconfigEntity();
            //Visitdetail.Level = 2;
            //Visitdetail.GID = "Visit";
            //Visitdetail.IsEnable = 0;
            //Visitdetail.IsPier = 0;
            //Visitdetail.ParentId = Visit.ID;
            //Visitdetail.ItemName = "拜访路线";
            //Visitdetail.PointList = "";
            //Visitdetail.Sort = 1;
            //Visitdetail.Create();
            //rlist.Add(Visitdetail);

            //RouteconfigEntity VisitLine1 = new RouteconfigEntity();
            //VisitLine1.Level = 3;
            //VisitLine1.GID = "Visit";
            //VisitLine1.IsEnable = 0;
            //VisitLine1.IsPier = 0;
            //VisitLine1.ParentId = Visitdetail.ID;
            //VisitLine1.ItemName = "1号路线";
            //VisitLine1.PointList = "";
            //VisitLine1.Sort = 1;
            //VisitLine1.Create();

            //RouteconfigEntity VisitLine2 = new RouteconfigEntity();
            //VisitLine2.Level = 3;
            //VisitLine2.GID = "Visit";
            //VisitLine2.IsEnable = 0;
            //VisitLine2.IsPier = 0;
            //VisitLine2.ParentId = Visitdetail.ID;
            //VisitLine2.ItemName = "2号路线";
            //VisitLine2.PointList = "";
            //VisitLine2.Sort = 2;
            //VisitLine2.Create();

            //rlist.Add(VisitLine1);
            //rlist.Add(VisitLine2);


            //其他车辆
            RouteconfigEntity Other = new RouteconfigEntity();
            Other.Level = 1;
            Other.GID = "";
            Other.IsEnable = 0;
            Other.IsPier = 0;
            Other.ParentId = "0";
            Other.ItemName = "其他车辆";
            Other.PointList = "";
            Other.Sort = 4;
            Other.LineType = 0;
            Other.Create();
            rlist.Add(Other);

            RouteconfigEntity Otherdetail = new RouteconfigEntity();
            Otherdetail.Level = 2;
            Otherdetail.GID = "Other";
            Otherdetail.IsEnable = 0;
            Otherdetail.IsPier = 0;
            Otherdetail.ParentId = Other.ID;
            Otherdetail.ItemName = "其他路线";
            Otherdetail.PointList = "";
            Otherdetail.Sort = 1;
            Otherdetail.LineType = 0;
            Otherdetail.Create();
            rlist.Add(Otherdetail);

            RouteconfigEntity OtherLine1 = new RouteconfigEntity();
            OtherLine1.Level = 3;
            OtherLine1.GID = "Other";
            OtherLine1.IsEnable = 0;
            OtherLine1.IsPier = 0;
            OtherLine1.ParentId = Otherdetail.ID;
            OtherLine1.ItemName = "1号路线";
            OtherLine1.PointList = "";
            OtherLine1.Sort = 1;
            OtherLine1.LineType = 0;
            OtherLine1.Create();

            RouteconfigEntity OtherLine2 = new RouteconfigEntity();
            OtherLine2.Level = 3;
            OtherLine2.GID = "Other";
            OtherLine2.IsEnable = 0;
            OtherLine2.IsPier = 0;
            OtherLine2.ParentId = Otherdetail.ID;
            OtherLine2.ItemName = "2号路线";
            OtherLine2.PointList = "";
            OtherLine2.Sort = 2;
            OtherLine2.LineType = 0;
            OtherLine2.Create();

            rlist.Add(OtherLine1);
            rlist.Add(OtherLine2);

            routeconfigbll.SaveList(rlist);

            return rlist;
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 应用路线
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectLine(string ID)
        {
            routeconfigbll.SelectLine(ID);
            return Success("应用成功。");
        }

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
            routeconfigbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, RouteconfigEntity entity)
        {
            string parentid=routeconfigbll.GetVisitParentid();
            entity.LineType = 1;
            entity.GID = "Visit";
            entity.IsEnable = 0;
            entity.IsPier = 0;
            entity.Level = 3;
            entity.ParentId = parentid;
            entity.PointList = "";
            routeconfigbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
