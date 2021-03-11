using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.KbsDeviceManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack.Common;
using ThoughtWorks.QRCode.Codec;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class CarManageController : BaseApiController
    {
        CarUserBLL CarUserbll = new CarUserBLL();
        CarinfoBLL carbll = new CarinfoBLL();
        CarreservationBLL crbll = new CarreservationBLL();
        CarrideBLL cridebll = new CarrideBLL();
        CarinlogBLL carinbll = new CarinlogBLL();
        private VisitcarBLL visitbll = new VisitcarBLL();
        OperticketmanagerBLL opbll = new OperticketmanagerBLL();
        HazardouscarBLL hacarbll = new HazardouscarBLL();
        RouteconfigBLL routebll = new RouteconfigBLL();
        WarehousegpsBLL warebll = new WarehousegpsBLL();
        CarviolationBLL viobll = new CarviolationBLL();
        TemporaryGroupsBLL Tempbll = new TemporaryGroupsBLL();
        HikdeviceBLL devicebll = new HikdeviceBLL();
        HikinoutlogBLL inoutbll = new HikinoutlogBLL();
        HikaccessBLL accessBll = new HikaccessBLL();
        CameramanageBLL cameramanagebll = new CameramanageBLL();
        DataItemDetailBLL pdata = new DataItemDetailBLL();
        DepartmentBLL departmentBLL = new DepartmentBLL();
        private ArealocationBLL arealocationbll = new ArealocationBLL();
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();

        public HttpContext ctx { get { return HttpContext.Current; } }

        #region 车辆管理

        /// <summary>
        /// 查看本用户私家车
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPrivateCar([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                CarinfoEntity car = carbll.GetUserCar(userId);
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                if (car != null)
                {

                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = new { car.ID, car.CarNo, car.Model, car.IsAuthorized, car.AuthUserId, car.AuthUserName, driver = path + car.DriverLicenseUrl, driving = path + car.DrivingLicenseUrl, car.State, car.Remark } };
                }
                else
                {
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = new { ID = "", CarNo = "", Model = "", IsAuthorized = "", AuthUserId = "", AuthUserName = "", driver = "", driving = "" } };
                }



            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取车牌号省字
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCarNo([FromBody] JObject json)
        {
            try
            {
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'CarNo'");
                return new { Code = 0, Count = data.Count(), Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }


        /// <summary>
        /// 添加/修改本用户私家车
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddPrivateCar()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                string CarNo = dy.data.CarNo;
                string Model = dy.data.Model;
                int IsAuthorized = Convert.ToInt32(dy.data.IsAuthorized);
                string AuthUserId = dy.data.AuthUserId;
                string AuthUserName = dy.data.AuthUserName;

                CarNo = CarNo.ToUpper();
                if (carbll.GetCarNoIsRepeat(CarNo, id))
                {
                    return new { Code = -1, Count = 0, Info = "该车牌号已录入，请勿重复录入车辆" };
                }

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                CarinfoEntity car;
                if (id != "")
                {
                    car = carbll.GetEntity(id);
                    if (car != null && car.State.ToString() == "1" && car.CarNo != CarNo)
                    {//审批通过后再修改车牌删除之前的出入权限重新审批车辆
                        //string oldcar = car.CarNo;
                        //car.CarNo = CarNo;
                        car.State = "";
                        car.Remark = "";
                        carbll.UpdateHiaKangCar(car, "");
                    }
                    else if (car != null && car.State.ToString() == "1")
                    {

                    }
                    else
                    {//审核未通过再次提交
                        car.State = "";
                        car.Remark = "";
                    }
                }
                else
                {
                    UserBLL ubll = new UserBLL();
                    UserEntity use = ubll.GetEntity(userId);
                    car = new CarinfoEntity();
                    car.CreateUserId = userId;
                    car.Type = 1;
                    if (use != null)
                    {
                        car.Dirver = use.RealName;
                        car.DirverId = use.UserId;
                        car.Phone = use.Mobile;
                        if (user != null) { car.Deptname = user.DeptName; }
                    }
                }

                string driver;
                string driving;
                UpImg(out driver, out driving);
                if (driver != "")
                {
                    car.DriverLicenseUrl = driver;
                }
                if (driving != "")
                {
                    car.DrivingLicenseUrl = driving;
                }

                car.IsAuthorized = IsAuthorized;
                if (IsAuthorized == 1)
                {
                    car.AuthUserId = AuthUserId;
                    car.AuthUserName = AuthUserName;
                }
                else
                {
                    car.AuthUserId = "";
                    car.AuthUserName = "";
                }

                car.CarNo = CarNo;
                car.Model = Model;

                //DataItemDetailBLL pdata = new DataItemDetailBLL();
                //var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                //var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
                carbll.SaveForm(id, car);
                return new { Code = 0, Count = 0, Info = "提交成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 上传方法
        /// </summary>
        /// <returns></returns>
        public void UpImg(out string driver, out string driving)
        {
            var dd = new DataItemDetailBLL();
            driver = "";
            driving = "";
            foreach (string key in ctx.Request.Files.AllKeys)
            {
                string fileurl = "";
                string url = "";
                HttpPostedFile file = ctx.Request.Files[key];
                //string type = string.Empty;
                if (file.ContentType.Contains("image"))
                {
                    if (key == "driver")
                    {
                        driver = "/Resource/Driver/" + Guid.NewGuid().ToString() + "." + file.FileName.Split('.')[1];
                        fileurl = dd.GetItemValue("imgPath") + driver;
                        url = dd.GetItemValue("imgPath") + "/Resource/Driver/";
                    }
                    else
                    {
                        driving = "/Resource/Driving/" + Guid.NewGuid().ToString() + "." + file.FileName.Split('.')[1];
                        fileurl = dd.GetItemValue("imgPath") + driving;
                        url = dd.GetItemValue("imgPath") + "/Resource/Driving/";
                    }


                    //type = "照片";
                    //EmerImgUrl += file.FileName + "|";

                }

                //上传附件到服务器
                if (!System.IO.Directory.Exists(url))
                {
                    System.IO.Directory.CreateDirectory(url);
                }
                file.SaveAs(fileurl);
            }
        }

        /// <summary>
        /// 查看班车预约列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRe([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                if (dy.data != null) { string CarNo = dy.data.CarNo; }

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                int pageSize = int.Parse(dy.pagesize.ToString()); //每页的记录数
                if (pageSize == 0) { pageSize = 20; }
                int pageIndex = int.Parse(dy.pageindex.ToString());  //当前页索引
                Pagination pagination = new Pagination();
                pagination.p_tablename = @" bis_carreservation d 
              join bis_carinfo t on d.cid = t.id and t.type = 0 
              left join (select count(1) as sumnum ,baseid as vid from bis_carreservation e where e.datatype =1 group by e.baseid) td1 on td1.vid=d.id";
                pagination.p_fields = @" d.saddress,  d.eaddress, d.resdate,
                      t.carno,
                      t.numberlimit,
                      d.cid,
                      d.createuserid,
                      t.model,
                      d.baseid,
                      td1.sumnum,
                      '' as isreser ";
                pagination.p_kid = "d.id";
                pagination.page = pageIndex;
                pagination.rows = pageSize;
                pagination.sidx = "d.createdate desc";
                pagination.sord = "";
                pagination.conditionJson = " 1=1 and  d.datatype=0 ";

                if (!string.IsNullOrEmpty(userId))
                {//筛选当前或大于当前时间的记录
                    pagination.conditionJson += string.Format(" and to_char(d.resdate,'yyyy-MM-dd')>='{0}' ", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                DataTable dt = crbll.GetPageList(pagination, "");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sql = string.Format("select id from  bis_carreservation s where s.baseid='{0}' and s.createuserid='{1}' and s.datatype=1", dt.Rows[i]["id"].ToString(), userId);
                        var obtdt = opbll.GetDataTable(sql);
                        if (obtdt.Rows.Count > 0)
                        {
                            dt.Rows[i]["isreser"] = "1";
                        }
                        else { dt.Rows[i]["isreser"] = "0"; }
                    }
                }
                return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 预约班车
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddRe([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string cid = dy.data.CID;
                string CarNo = dy.data.CarNo;
                //int Time = Convert.ToInt32(dy.data.Time);
                int IsReser = Convert.ToInt32(dy.data.IsReser);
                string baseid = dy.data.BaseId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                crbll.AddReser(userId, cid, 0, CarNo, IsReser, baseid);
                return new { Code = 0, Count = 0, Info = "提交成功" };
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }
        }

        /// <summary>
        /// 扫码上车
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Coarding([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string CarNo = dy.data.CarNo;
                CarinfoEntity car = carbll.GetBusCar(CarNo);
                if (car != null)
                {
                    CarrideEntity ride = new CarrideEntity();
                    ride.CID = car.ID;
                    ride.CarNo = car.CarNo;
                    ride.Status = 0;
                    ride.Type = car.Type;
                    ride.ScancodeTime = DateTime.Now;
                    cridebll.SaveForm("", ride);
                    return new { Code = 0, Count = 0, Info = "扫码成功" };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "二维码错误,无对应车辆" };
                }
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }
        }

        /// <summary>
        /// 司机添加电厂班车班次
        /// </summary>
        /// <returns></returns>
        public object AddDriverCarInfo([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string Id = dy.data.Id;
                string cid = dy.data.CID;
                string CarNo = dy.data.CarNo;
                string time = dy.data.time;
                string sadder = dy.data.Saddress;
                string eadder = dy.data.Eaddress;
                string type = dy.data.type;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                CarreservationEntity entity = crbll.GetEntity(Id);
                if (type == "0")
                {//添加

                    var list = crbll.GetList("");
                    var list1 = list.Where(a => a.DataType == 0 && a.CarNo == CarNo && a.RESDate == DateTime.Parse(time)).ToList();
                    if (list1 != null && list1.Count > 0)
                    {
                        return new { Code = -1, Count = 0, Info = "该车在同一时间段已有预约记录！" };
                    }
                    var list2 = list.Where(a => a.DataType == 0 && a.CreateUserId == userId && a.Saddress == sadder && a.RESDate == DateTime.Parse(time)).ToList();
                    if (list2 != null && list2.Count > 0)
                    {
                        return new { Code = -1, Count = 0, Info = "您在同一时间相同起点已有一条预约记录！" };
                    }

                    entity = new CarreservationEntity();
                    entity.RESDate = DateTime.Parse(time);
                    entity.CarNo = CarNo;
                    entity.Saddress = sadder;
                    entity.Eaddress = eadder;
                    entity.CID = cid;
                    entity.DataType = 0;
                    entity.CreateUserId = userId;
                    crbll.AddDriverCarInfo(Id, entity);
                }
                else if (type == "1" && entity != null)
                {//修改
                    var list = crbll.GetList("").Where(a => a.BaseId == Id && a.DataType == 1).ToList();
                    if (list != null && list.Count > 0)
                    {
                        return new { Code = -1, Count = 0, Info = "该班次已有人员预约不能进行修改！" };
                    }
                    else
                    {
                        entity.RESDate = DateTime.Parse(time);
                        entity.CarNo = CarNo;
                        entity.Saddress = sadder;
                        entity.Eaddress = eadder;
                        entity.CID = cid;
                        entity.CreateUserId = userId;
                        crbll.AddDriverCarInfo(entity.ID, entity);
                    }
                }
                else if (type == "2" && entity != null)
                {//删除
                    crbll.RemoveForm(entity.ID);
                }
                return new { Code = 0, Count = 0, Info = "提交成功" };
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }
        }


        /// <summary>
        /// 司机获取电厂班车预约列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDriverCarListJson([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                if (dy.data != null) { string CarNo = dy.data.CarNo; }

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                int pageSize = int.Parse(dy.pagesize.ToString()); //每页的记录数
                if (pageSize == 0) pageSize = 20;
                int pageIndex = int.Parse(dy.pageindex.ToString());  //当前页索引
                Pagination pagination = new Pagination();
                pagination.p_tablename = @" bis_carreservation d 
              join bis_carinfo t on d.cid = t.id and t.type = 0 
              left join (select count(1) as sumnum ,baseid as vid from bis_carreservation e where e.datatype =1 group by e.baseid) td1 on td1.vid=d.id";
                pagination.p_fields = @" d.saddress,  d.eaddress, d.resdate,
                      t.carno,
                      t.numberlimit,
                      d.cid,
                      d.createuserid,
                      t.model,
                      d.baseid,
                      td1.sumnum,
                      '' as isreser ";
                pagination.p_kid = "d.id";
                pagination.page = pageIndex;
                pagination.rows = pageSize;
                pagination.sidx = "d.createdate desc";
                pagination.sord = "";
                pagination.conditionJson = " 1=1 and  d.datatype=0 ";

                if (!string.IsNullOrEmpty(userId))
                {//筛选当前或大于当前时间的记录
                    pagination.conditionJson += string.Format(" and to_char(d.resdate,'yyyy-MM-dd')>='{0}' ", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                CarinfoEntity car = carbll.GetList("").Where(a => a.DirverId == userId && a.Type == 0).ToList().FirstOrDefault();
                if (car != null)
                {//司机一但绑定了车辆，只筛选跟自己相关的车辆
                    pagination.conditionJson += string.Format(" and  d.carno='{0}'   ", car.CarNo);
                }

                var dt = crbll.GetPageList(pagination, "");
                var objmode = new
                {
                    list = dt,
                    entity = GetDriverCarItem(userId)
                };

                return new { Code = 0, Info = "获取数据成功", Count = dt.Rows.Count, data = objmode };
                //return new { code = 0, info = GetDriverCarItem(userId), count = pagination.records, data = dt.ToJson() };
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }

        }

        /// <summary>
        /// 司机已绑定班车信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ReserVation GetDriverCarItem(string userId)
        {
            ReserVation entity = new ReserVation();
            try
            {
                CarinfoEntity car = carbll.GetList("").Where(a => a.DirverId == userId && a.Type == 0).ToList().FirstOrDefault();
                if (car != null)
                {
                    entity.CarNo = car.CarNo;
                    entity.CID = car.ID;
                    var list = cridebll.GetList("").Where(a => a.CID == car.ID).ToList(); ;
                    if (list != null)
                    {
                        entity.Model = list.Count + "/" + car.NumberLimit;
                    }
                    else
                    {
                        entity.Model = 0 + "/" + car.NumberLimit;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;

        }


        /// <summary>
        /// 司机扫码启动车辆并绑定与车辆关联信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object DriverScanningEnableCar([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string CarNo = dy.data.CarNo;
                string IsEnable = dy.data.IsEnable;//1取消预约 0预约
                CarinfoEntity car = carbll.GetBusCar(CarNo);
                if (car != null)
                {
                    if (car.IsEnable == 1 && !car.DirverId.Contains(userId))
                    {
                        return new { Code = -1, Count = 0, Info = "该车已被【" + car.Dirver + "】占用，请更换车辆！" };
                    }
                    else
                    {
                        var user = new UserBLL().GetEntity(userId);
                        if (user != null)
                        {
                            if (IsEnable == "1")
                            {//禁用
                                car.IsEnable = 0;
                                car.Dirver = "";
                                car.DirverId = "";
                                car.Phone = "";
                            }
                            else
                            {//启动
                                var objlist = carbll.GetList("").Where(a => a.DirverId == userId && a.Type == 0).ToList();
                                if (objlist != null && objlist.Count > 0)
                                {
                                    return new { Code = -1, Count = 0, Info = "你已经绑定了【" + objlist[0].CarNo + "】车辆，请解绑后再绑定新车！" };
                                }
                                else
                                {
                                    car.IsEnable = 1;
                                    car.Dirver = user.RealName;
                                    car.DirverId = user.UserId;
                                    car.Phone = user.Mobile;
                                }
                            }
                            carbll.SaveForm(car.ID, car);
                        }
                        return new { Code = 0, Count = 0, Info = "扫码成功" };
                    }
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "二维码错误,无对应车辆" };
                }
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }
        }

        /// <summary>
        /// 司机查看本人或所有电厂班车信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDrivereCarInfo([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string type = dy.data.type;//0本人绑定班车 1所以电厂班车

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (type == "0")
                {
                    CarinfoEntity car = carbll.GetList("").Where(a => a.DirverId == userId && a.Type == 0).ToList().FirstOrDefault();
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = car };
                }
                else
                {
                    List<CarinfoEntity> list = carbll.GetList("").Where(a => a.Type == 0).ToList();
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = list };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取电厂班车地址路线
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetDriverCarAddress([FromBody] JObject json)
        {
            try
            {
                DataItemDetailBLL itembll = new DataItemDetailBLL();
                IEnumerable<DataItemModel> list = itembll.GetDataItemListByItemCode("ShuttleBusRoute");
                List<ComboxEntity> addlist = new List<ComboxEntity>();
                foreach (var item in list)
                {
                    ComboxEntity y1 = new ComboxEntity();
                    y1.itemName = item.ItemName;
                    y1.itemValue = item.ItemName;
                    addlist.Add(y1);
                }
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = addlist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }



        /// <summary>
        /// 车辆过杆
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object CarPass([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string CarNo = dy.data.CarNo;
                int Status = Convert.ToInt32(dy.data.Status);
                string Address = dy.data.Address;
                CarinfoEntity car = carbll.GetCar(CarNo);
                CarinlogEntity carlog = new CarinlogEntity();
                carlog.Address = Address;
                carlog.ID = Guid.NewGuid().ToString();
                carlog.CarNo = CarNo;
                carlog.CreateDate = DateTime.Now;
                carlog.CreateUserDeptCode = "00";
                carlog.CreateUserId = "System";
                carlog.CreateUserOrgCode = "00";
                carlog.IsOut = 0;
                carlog.Status = Status;
                carlog.IsLeave = 0;
                if (car != null)
                {
                    carlog.CID = car.ID;
                    carlog.Type = car.Type;

                    if (car.Type == 1)
                    {
                        if (car.IsAuthorized == 1)
                        {
                            //如果授权 则记录授权人ID
                            carlog.DriverName = car.AuthUserName;
                            carlog.DriverID = car.AuthUserId;
                            UserBLL ubll = new UserBLL();
                            UserEntity use = ubll.GetEntity(car.AuthUserId);
                            if (use != null)
                            {
                                carlog.Phone = use.Mobile;
                            }
                        }
                        else
                        {
                            carlog.DriverName = car.Dirver;
                            carlog.Phone = car.Phone;
                            carlog.DriverID = car.CreateUserId;
                        }


                    }
                    else
                    {
                        carlog.DriverName = car.Dirver;
                        carlog.Phone = car.Phone;
                        carlog.DriverID = "";
                    }

                    carinbll.AddPassLog(carlog);
                }
                else
                {
                    VisitcarEntity visit = visitbll.GetCar(CarNo);
                    if (visit != null)
                    {
                        carlog.DriverName = visit.Dirver;
                        carlog.Phone = visit.Phone;
                        carlog.DriverID = "";
                        carlog.CID = visit.ID;
                        carlog.Type = 3;
                        carinbll.AddPassLog(carlog);
                    }
                    else
                    {
                        OperticketmanagerEntity op = opbll.GetCar(CarNo);
                        if (op != null)
                        {
                            carlog.DriverName = op.DriverName;
                            carlog.Phone = op.DriverTel;
                            carlog.DriverID = "";
                            carlog.CID = op.ID;
                            carlog.Type = 4;
                            carinbll.AddPassLog(carlog);
                        }
                        else
                        {
                            HazardouscarEntity ha = hacarbll.GetCar(CarNo);
                            if (ha != null)
                            {
                                carlog.DriverName = ha.Dirver;
                                carlog.Phone = ha.Phone;
                                carlog.DriverID = "";
                                carlog.CID = ha.ID;
                                carlog.Type = 5;
                                carinbll.AddPassLog(carlog);
                            }
                            else
                            {
                                return new { Code = -1, Count = 0, Info = "场内无此车辆" };
                            }
                        }
                    }
                }



                return new { Code = 0, Count = 0, Info = "添加成功" };
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }
        }

        #endregion

        #region 危化品车辆接口
        /// <summary>
        /// 查看当天危化品车辆数量
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHazardousListJson([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string day = dy.data.Day;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var data = hacarbll.GetHazardousList(day);
                return new { Code = 0, Count = data.Count, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 查看当天危化品车辆列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHazardousItem([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string Hid = dy.data.Hid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                CarcheckitemdetailBLL detail = new CarcheckitemdetailBLL();
                var data = detail.GetList(Hid);
                return new { Code = 0, Count = data.Count(), Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 处理人提交检查信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitHazardousItem([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string Item = JsonConvert.SerializeObject(dy.data);
                List<CarcheckitemdetailEntity> detailList = JsonConvert.DeserializeObject<List<CarcheckitemdetailEntity>>(Item);
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                CarcheckitemdetailBLL detail = new CarcheckitemdetailBLL();
                detail.Update(detailList);
                return new { Code = 0, Count = 0, Info = "获取数据成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 查看该危害因素车辆检查表的二维码
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetItemAndImg([FromBody] JObject json)
        {
            var dd = new DataItemDetailBLL();
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.Id;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                string qrcode = "/Resource/qrcode/";
                string fileurl = dd.GetItemValue("imgPath") + qrcode;
                string pType = "危化品车辆";
                if (!System.IO.File.Exists(fileurl))
                {
                    System.IO.Directory.CreateDirectory(fileurl);
                }

                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeVersion = 10;
                qrCodeEncoder.QRCodeScale = 2;
                qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
                Bitmap bmp = qrCodeEncoder.Encode(id + "|" + pType, Encoding.UTF8);
                bmp.Save(fileurl + id + ".jpg", ImageFormat.Jpeg);
                bmp.Dispose();
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = path + qrcode + id + ".jpg" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }



        }

        /// <summary>
        /// 扫二维码交接接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object QRCodeSubmit([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.Id;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                hacarbll.ChangeProcess(id);
                return new { Code = 0, Count = 1, Info = "提交数据成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 根据ID获取危化品车辆数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHazardousCar([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.Id;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var data = hacarbll.GetEntity(id);
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                if (!data.HandoverSign.IsNullOrEmpty())
                {
                    data.HandoverSign = path + data.HandoverSign;
                }

                if (!data.ProcessingSign.IsNullOrEmpty())
                {
                    data.ProcessingSign = path + data.ProcessingSign;
                }

                if (!data.DrivingLicenseUrl.IsNullOrEmpty())
                {
                    data.DrivingLicenseUrl = path + data.DrivingLicenseUrl;
                }

                if (!data.DriverLicenseUrl.IsNullOrEmpty())
                {
                    data.DriverLicenseUrl = path + data.DriverLicenseUrl;
                }

                return new { Code = 0, Count = 1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 三维调用存储规划路线点
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveRoutePoint()
        {
            try
            {
                //string res = json.Value<string>("json");
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //string Item = JsonConvert.SerializeObject(res);
                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string json = reader.ReadToEnd();
                GpsList detailList = JsonConvert.DeserializeObject<GpsList>(json);

                if (detailList.data == null || detailList.data.Count == 0)
                {
                    return new { Code = 1, Count = 0, Info = "路径点不能为空" };
                }

                var data = routebll.GetEntity(detailList.ID);
                data.PointList = json;
                routebll.SaveForm(detailList.ID, data);
                return new { Code = 0, Count = 1, Info = "提交数据成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 三维调用存储规划路线点
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRoutePoint([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                List<GpsPoint> detailList = new List<GpsPoint>();
                Random r = new Random();
                for (int i = 0; i < 10; i++)
                {
                    GpsPoint g = new GpsPoint();
                    g.X = r.NextDouble();
                    g.Y = r.NextDouble();
                    detailList.Add(g);
                }

                GpsList gps = new GpsList();
                gps.data = detailList;
                gps.ID = "1234123";
                //string userId = dy.userid;
                //string id = dy.data.Id;
                ////获取用户基本信息
                //OperatorProvider.AppUserId = userId;  //设置当前用户
                //Operator user = OperatorProvider.Provider.Current();
                //var data = hacarbll.GetEntity(id);
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = gps };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取所有车辆路线(底层算法模块初始化调用，值获取三级节点)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRouteList([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                RouteconfigBLL configbll = new RouteconfigBLL();
                var data = configbll.GetRoute();
                return new { Code = 0, Count = 1, Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }

        /// <summary>
        /// 获取所有区域节点
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAreaList([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                AreagpsBLL Areabll = new AreagpsBLL();

                var data = Areabll.GetTable();
                return new { Code = 0, Count = 1, Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }

        /// <summary>
        /// 获取仓库位置列表
        /// </summary>
        /// <param name="queryJson">查询参数</param> 
        /// <returns>返回列表Json</returns>
        [HttpPost]
        public object GetWareHouseList([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var data = warebll.GetList("");
                return new { Code = 0, Count = data.Count(), Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }

        /// <summary>
        /// 获取在场车辆信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPresentCar(string json)
        {
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
            string CarNo = dy.data.CarNo;
            string TakegoodsName = dy.data.Takegoodsname;
            string where = "";
            //获取页数和条数
            int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
            Pagination pagination = new Pagination();

            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            pagination.p_kid = "ID";
            pagination.p_fields = "Createdate,Takegoodsname,carno,purpose,dirver,phone,note,state,type,gpsid";
            pagination.p_tablename = @"(
            select null  Takegoodsname,ID,Createdate,carno,'拜访' as purpose,dirver
            ,phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'0' type,NVL(vnum,0) vnum,gpsid from bis_visitcar  vi
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on vi.id=cv.cid
            where state=3 

            union 

            select Takegoodsname,ID,Createdate,platenumber carno,'物料' as purpose,
            DriverName dirver,DriverTel phone,PassRemark note,JsImgpath anumber,JsImgpath driverlicenseurl,XsImgpath drivinglicenseurl,examinestatus state,'1' type,NVL(vnum,0) vnum,gpsid 
            from WL_OPERTICKETMANAGER vi
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on vi.id=cv.cid
             where isdelete=1 and examinestatus=3 
            
            union 

            select null Takegoodsname,ID,Createdate,carno,'危化品' as purpose,dirver
            ,phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'2' type,NVL(vnum,0) vnum,gpsid from bis_hazardouscar  hazardous
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on hazardous.id=cv.cid
            where state=3 
            ) a1
            ";
            if (CarNo != "")
            {
                where += " and CarNo like '%" + CarNo + "%' ";
            }

            if (TakegoodsName != "")
            {
                where += " and TakegoodsName like '%" + TakegoodsName + "%'  ";
            }
            pagination.conditionJson = " 1=1" + where;
            pagination.sord = "desc";
            pagination.records = 0;
            pagination.page = page;//页数
            pagination.rows = rows;//行数
            pagination.sidx = "Createdate";//排序字段


            var dt = visitbll.GetPageList(pagination, null);
            return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
        }

        /// <summary>
        /// 获取车辆违章信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetViolationList(string json)
        {

            //string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);


            //获取页数和条数
            int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
            Pagination pagination = new Pagination();

            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            pagination.p_kid = "ID";
            pagination.p_fields = "Createdate,Takegoodsname,carno,purpose,dirver,phone,note,state,type,VIOLATIONTYPE,gpsid";
            pagination.p_tablename = @"(
            select null  Takegoodsname,vi.ID,vi.Createdate,carno,'拜访' as purpose,vi.dirver
            ,vi.phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'0' type,VIOLATIONTYPE,gpsid from bis_visitcar  vi
            left join BIS_CARVIOLATION  cv on vi.id=cv.cid
            where state=3 

            union 

            select Takegoodsname,vi.ID,vi.Createdate,platenumber carno,'物料' as purpose,
            DriverName dirver,DriverTel phone,PassRemark note,JsImgpath anumber,JsImgpath driverlicenseurl,XsImgpath drivinglicenseurl,examinestatus state,'1' type,VIOLATIONTYPE,gpsid 
            from WL_OPERTICKETMANAGER vi
            left join BIS_CARVIOLATION  cv on vi.id=cv.cid
             where isdelete=1 and examinestatus=3  
            
            union 

            select null Takegoodsname,hazardous.ID,hazardous.Createdate,carno,'危化品' as purpose,hazardous.dirver
            ,hazardous.phone,note,ACCOMPANYINGPERSON anumber,driverlicenseurl,drivinglicenseurl,state,'2' type,VIOLATIONTYPE,gpsid from bis_hazardouscar  hazardous
            left join  BIS_CARVIOLATION    cv on hazardous.id=cv.cid
            where state=3 
            ) a1
            ";
            pagination.conditionJson = " 1=1 and VIOLATIONTYPE is not null";


            pagination.sord = "desc";
            pagination.records = 0;
            pagination.page = page;//页数
            pagination.rows = rows;//行数
            pagination.sidx = "Createdate";//排序字段


            var dt = visitbll.GetPageList(pagination, null);
            return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
        }

        /// <summary>
        /// 新增违章数据
        /// </summary>
        /// <param name="queryJson">查询参数</param> 
        /// <returns>返回列表Json</returns>
        [HttpPost]
        public object AddViolation([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.data.id;
                int type = Convert.ToInt32(dy.data.type);
                int ViolationType = Convert.ToInt32(dy.data.ViolationType);
                string ViolationMsg = dy.data.ViolationMsg;
                viobll.AddViolation(id, type, ViolationType, ViolationMsg);
                return new { Code = 0, Count = 1, Info = "提交数据成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }

        /// <summary>
        /// 获取所有设置了gps的场内车辆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetGpsCar()
        {
            try
            {
                var data = carbll.GetGspCar();
                return new { Code = 0, Count = data.Count, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }
        /// <summary>
        /// 三维获取车辆详细信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object ThreeDGetCar()
        {
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string json = reader.ReadToEnd();
                var car = JsonConvert.DeserializeObject<GetCarEntity>(json);
                int type = car.Type;
                string id = car.ID;
                RtnCarEntity rtnCar = new RtnCarEntity();
                switch (type)
                {
                    case 0:
                    case 1:
                    case 2:
                        if (type == 0)
                        {
                            rtnCar.Type = "电厂班车";
                        }
                        else if (type == 1)
                        {
                            rtnCar.Type = "商务公车";
                        }
                        else
                        {
                            rtnCar.Type = "私家车";
                        }

                        var carinfo = carbll.GetEntity(id);
                        rtnCar.CarNo = carinfo.CarNo;
                        rtnCar.Driver = carinfo.Dirver;
                        rtnCar.Phone = carinfo.Phone;
                        rtnCar.GPSID = carinfo.GpsId;
                        var inlog = carinbll.GetNewCarinLog(carinfo.CarNo);
                        if (inlog != null)
                        {
                            rtnCar.StartTime = Convert.ToDateTime(inlog.CreateDate);
                        }
                        else
                        {
                            rtnCar.StartTime = DateTime.Now;
                        }

                        break;
                    case 3:
                        var visit = visitbll.GetEntity(id);
                        rtnCar.Type = "拜访车辆";
                        rtnCar.CarNo = visit.CarNo;
                        rtnCar.Phone = visit.Phone;
                        rtnCar.Driver = visit.Dirver;
                        rtnCar.GPSID = visit.GPSID;
                        rtnCar.StartTime = Convert.ToDateTime(visit.InTime);
                        break;
                    case 4:
                        var op = opbll.GetEntity(id);
                        rtnCar.Type = "物料车辆";
                        rtnCar.CarNo = op.Platenumber;
                        rtnCar.Phone = op.DriverTel;
                        rtnCar.Driver = op.DriverName;
                        rtnCar.StartTime = Convert.ToDateTime(op.Getdata);
                        break;
                    case 5:
                        var ha = hacarbll.GetEntity(id);
                        rtnCar.Type = "危化品车辆";
                        rtnCar.CarNo = ha.CarNo;
                        rtnCar.Phone = ha.Phone;
                        rtnCar.Driver = ha.Dirver;
                        rtnCar.StartTime = Convert.ToDateTime(ha.InTime);
                        break;

                }
                return new { Code = 0, Count = 1, Info = "获取成功", data = rtnCar };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }


        }

        /// <summary>
        /// 三维获取车辆路线轨迹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCarPoint()
        {
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string input = reader.ReadToEnd();
                //var car = JsonConvert.DeserializeObject<GetSafeHatGpsDataInput>(json);
                //GetSafeHatGpsDataInput gd = new GetSafeHatGpsDataInput();
                //gd.SN = "014145730468";
                //gd.StartTime = Convert.ToDateTime("2020-01-12 07:00:00");
                //gd.EndTime = Convert.ToDateTime("2020-01-12 18:00:00");
                //var inputtest = new { input = gd };
                //string input = JsonConvert.SerializeObject(gd);
                string Url = new DataItemDetailBLL().GetItemValue("IOTUrl");
                string rtnstr = HttpCommon.HttpPostJson(Url + "GpsPoint/GetCarPointData?data=" + input, "");
                List<CarLocationData> cardata = JsonConvert.DeserializeObject<List<CarLocationData>>(rtnstr);
                GpsList gpsList = new GpsList();
                List<GpsPoint> pointlist = new List<GpsPoint>();
                double lon = 0;
                double lat = 0;
                foreach (var item in cardata)
                {
                    if (lon == Convert.ToDouble(item.Longitude) && lat == Convert.ToDouble(item.Latitude))
                    {
                        continue;
                    }
                    GpsPoint point = new GpsPoint();
                    point.X = Convert.ToDouble(item.Longitude);
                    point.Y = Convert.ToDouble(item.Latitude);
                    point.Z = 500;
                    pointlist.Add(point);
                    lon = point.X;
                    lat = point.Y;
                }

                gpsList.data = pointlist;
                gpsList.ID = "123";
                return new { Code = 0, Count = 1, Info = "获取成功", data = gpsList };
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = "获取失败", data = e.Message };
            }


        }
        /// <summary>
        /// 初始化在场拜访\危化品车辆
        /// </summary>
        [HttpPost]
        public object IniVHOCar()
        {
            try
            {
                var data = visitbll.IniVHOCar();
                return new { Code = 0, Count = 1, Info = "获取成功", data = data };
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }


        }

        #endregion

        #region 给海康事件回调接口


        /// <summary>
        /// 门禁回调接口人脸通过事件(安防平台1.4及以上版本)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AccessNewBack()
        {
            string json = "";
            try
            {

                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                #region 进入方法记录日志
                string fileName = "UserInLog" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录方法调用 Json：" + json);
                #endregion
                HikBack hi = JsonConvert.DeserializeObject<HikBack>(json);
                HikinoutlogEntity inuser = null;
                if (hi.Params.Events.Count > 0)
                {
                    //获取到所有部门集合
                    var deptlist = departmentBLL.GetList();

                    DataItemDetailBLL pdata = new DataItemDetailBLL();
                    var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                    var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
                    var HikHttpsIP = pdata.GetItemValue("HikHttpsIP");//海康平台访问IP
                    var imgPath = pdata.GetItemValue("imgPath");//图片存放路径
                    string key = string.Empty;// "21049470";
                    string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                    if (!string.IsNullOrEmpty(pitem))
                    {
                        key = pitem.Split('|')[0];
                        sign = pitem.Split('|')[1];
                    }

                    #region 根据回调事件获取的人员卡号信息查询人员信息

                    string PicUrl = "/artemis/api/acs/v1/event/pictures";//获取门禁事件图片接口路径
                    string UserUrl = "/artemis/api/resource/v2/person/advance/personList";
                    // 中途还可以设置平台信息，后续的请求都以此信息为准
                    SocketHelper.SetPlatformInfo(key, sign, HikHttpsIP, 443, true);
                    for (int i = 0; i < hi.Params.Events.Count; i++)
                    {
                        var UserModel = new
                        {
                            pageNo = 1,
                            pageSize = 1000,
                            cardNo = hi.Params.Events[i].data.ExtEventCardNo
                        };
                        //string UserMsg = SocketHelper.LoadCameraList(UserModel, url, UserUrl, key, sign);
                        HttpUtillibKbs.SetPlatformInfo(key, sign, HikHttpsIP, 443, true);
                        byte[] result1 = HttpUtillibKbs.HttpPost(UserUrl, JsonConvert.SerializeObject(UserModel), 20);
                        string UserMsg = System.Text.Encoding.UTF8.GetString(result1);
                        UserSearch user = JsonConvert.DeserializeObject<UserSearch>(UserMsg);
                        if (user.code == "0" && user.data.list.Count > 0)
                        {
                            string personid = user.data.list[0].personId;
                            TemporaryUserEntity TemporaryUser = Tempbll.HikGetUserEntity(personid);
                            int USERTYPE = 0;
                            if (TemporaryUser == null)
                            {
                                USERTYPE = 2;
                            }
                            else
                            {
                                if (TemporaryUser.Istemporary == 0)
                                {
                                    USERTYPE = TemporaryUser.Istemporary;
                                }
                                else
                                {
                                    USERTYPE = 2;
                                }
                            }
                            #region 根据设备ID查询平台中配置的设备信息
                            HikdeviceEntity device = devicebll.GetDeviceEntity(hi.Params.Events[i].srcParentIndex);
                            if (device != null)
                            {
                                var picmodel = new
                                {
                                    svrIndexCode = hi.Params.Events[i].data.svrIndexCode,
                                    picUri = hi.Params.Events[i].data.ExtEventPictureURL
                                };
                                string body = JsonConvert.SerializeObject(picmodel);
                                string Img = "";
                                //string Img = SocketHelper.GetHeader(url, body, PicUrl, key, sign);
                                string picPath = "/Resource/HikInImg/" + Guid.NewGuid().ToString() + ".jpeg";
                                byte[] result = SocketHelper.HikHttpPost(PicUrl, body, 15);
                                if (null == result)
                                {
                                    SocketHelper.SetLog("UserImg", "人员图片异常", "无返回值");
                                }
                                else
                                {
                                    string tmp = System.Text.Encoding.UTF8.GetString(result);
                                    JObject obj = null;
                                    try
                                    {
                                        obj = (JObject)JsonConvert.DeserializeObject(tmp);
                                        SocketHelper.SetLog("UserImg", "人员图片异常", tmp);
                                    }
                                    catch (Exception)
                                    {
                                        Img = SocketHelper.WriteImg(imgPath, result);
                                    }
                                }
                                HikinoutlogEntity Hik = new HikinoutlogEntity();
                                Hik.AreaName = device.AreaName;
                                if (USERTYPE == 0)
                                {
                                    Hik.DeptId = user.data.list[0].orgIndexCode;
                                    string[] deptname = user.data.list[0].orgPathName.Split('/');
                                    Hik.DeptName = deptname[deptname.Length - 1];
                                    var dept = deptlist.FirstOrDefault(it => it.DepartmentId == Hik.DeptId);
                                    if (dept.Nature == "承包商")
                                    {
                                        USERTYPE = 1;
                                    }
                                }
                                else
                                {
                                    Hik.DeptId = user.data.list[0].orgIndexCode;
                                    Hik.DeptName = "临时人员";
                                }

                                Hik.CreateDate = DateTime.Now;
                                Hik.CreateUserId = "System";
                                Hik.CreateUserDeptCode = "00";
                                Hik.CreateUserOrgCode = "00";
                                Hik.DeviceName = device.DeviceName;
                                Hik.DeviceType = 0;
                                Hik.EventType = 1;
                                Hik.ID = Guid.NewGuid().ToString();
                                Hik.InOut = device.OutType;
                                Hik.UserId = user.data.list[0].personId;
                                Hik.UserName = user.data.list[0].personName;
                                Hik.UserType = USERTYPE;
                                Hik.ScreenShot = Img;
                                Hik.DeviceHikID = device.HikID;
                                if (device.OutType == 0)
                                {
                                    Hik.IsOut = 0;
                                }
                                else
                                {
                                    Hik.IsOut = 1;
                                    //如果是出门则要先找到该人员对应的入场记录进行更改
                                    inuser = inoutbll.GetInUser(personid);
                                    if (inuser != null)
                                    {
                                        inuser.IsOut = 1;
                                        inuser.InId = Hik.ID;
                                        inuser.OutTime = DateTime.Now;
                                        inuser.ModifyDate = DateTime.Now;
                                        inuser.ModifyUserId = "System";
                                        List<PersononlineEntity> plist = new List<PersononlineEntity>();
                                        int hour = (Convert.ToDateTime(inuser.OutTime) - Convert.ToDateTime(inuser.CreateDate)).Hours;
                                        for (int b = 0; b <= hour; b++)
                                        {
                                            var dp = deptlist.Where(it => it.DepartmentId == inuser.DeptId).FirstOrDefault();
                                            if (dp != null)
                                            {
                                                PersononlineEntity person = new PersononlineEntity();
                                                person.ID = Guid.NewGuid().ToString();
                                                person.TimeNum = Convert.ToDateTime(inuser.CreateDate).AddHours(b);
                                                person.UserId = inuser.UserId;
                                                person.UserName = inuser.UserName;
                                                person.DeptId = inuser.DeptId;
                                                person.DeptName = inuser.DeptName;
                                                person.DeptCode = dp.EnCode;
                                                person.OnlineHour = person.TimeNum.Value.Hour.ToString();
                                                person.OnlineDate = person.TimeNum.Value.ToString("yyyy-MM-dd");
                                                person.LogId = inuser.ID;
                                                person.LogType = 1;
                                                person.CreateDate = DateTime.Now;
                                                person.CreateUserId = "system";
                                                person.CreateUserDeptCode = "00";
                                                person.CreateUserOrgCode = "00";
                                                plist.Add(person);
                                            }
                                        }

                                        if (plist.Count > 0)
                                        {
                                            PersononlineBLL plbll = new PersononlineBLL();
                                            plbll.Insert(plist);
                                        }
                                    }
                                }
                                inoutbll.UserAisleSave(Hik, inuser);
                            }

                            #endregion
                        }
                    }
                    #endregion
                }
                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                string fileName = "UserIn" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }


        /// <summary>
        /// 门禁回调接口 人脸通过事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AccessBack()
        {
            string json = "";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                #region 进入方法记录日志
                string fileName = "UserFace" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录方法调用 Json：" + json);
                #endregion
                HikBack hi = JsonConvert.DeserializeObject<HikBack>(json);
                HikinoutlogEntity inuser = null;
                if (hi.Params.Events.Count > 0)
                {
                    //获取到所有部门集合
                    var deptlist = departmentBLL.GetList();
                    DataItemDetailBLL pdata = new DataItemDetailBLL();
                    var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                    var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
                    var HikHttpsIP = pdata.GetItemValue("HikHttpsIP");//海康平台访问IP
                    var imgPath = pdata.GetItemValue("imgPath");//图片存放路径
                    string HikHttps = pdata.GetItemValue("HikHttps");//海康1.4及以上版本https

                    string key = string.Empty;// "21049470";
                    string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                    if (!string.IsNullOrEmpty(pitem))
                    {
                        key = pitem.Split('|')[0];
                        sign = pitem.Split('|')[1];
                    }

                    #region 根据回调事件获取的人员卡号信息查询人员信息
                    string UserUrl = "/artemis/api/resource/v2/person/advance/personList";
                    // 中途还可以设置平台信息，后续的请求都以此信息为准
                    SocketHelper.SetPlatformInfo(key, sign, HikHttpsIP, 443, true);
                    for (int i = 0; i < hi.Params.Events.Count; i++)
                    {
                        var UserModel = new
                        {
                            pageNo = 1,
                            pageSize = 1000,
                            cardNo = hi.Params.Events[i].data.ExtEventCardNo
                        };


                        string UserMsg = string.Empty;
                        if (!string.IsNullOrEmpty(HikHttps))
                        {
                            HttpUtillibKbs.SetPlatformInfo(key, sign, HikHttpsIP, 443, true);
                            byte[] result1 = HttpUtillibKbs.HttpPost(UserUrl, JsonConvert.SerializeObject(UserModel), 20);
                            UserMsg = System.Text.Encoding.UTF8.GetString(result1);
                        }
                        else
                        {
                            UserMsg = SocketHelper.LoadCameraList(UserModel, url, UserUrl, key, sign);
                        }
                        UserSearch user = JsonConvert.DeserializeObject<UserSearch>(UserMsg);
                        if (user.code == "0" && user.data.list.Count > 0)
                        {
                            string personid = user.data.list[0].personId;
                            TemporaryUserEntity TemporaryUser = Tempbll.HikGetUserEntity(personid);
                            int USERTYPE = 0;
                            if (TemporaryUser == null)
                            {
                                USERTYPE = 2;
                            }
                            else
                            {
                                if (TemporaryUser.Istemporary == 0)
                                {
                                    USERTYPE = TemporaryUser.Istemporary;
                                }
                                else
                                {
                                    USERTYPE = 2;
                                }
                            }
                            #region 根据设备ID查询平台中配置的设备信息

                            HikdeviceEntity device = devicebll.GetDeviceEntity(hi.Params.Events[i].srcParentIndex);
                            if (device != null)
                            {
                                #region 门禁图片不需要下载 直接记录海康服务器图片地址
                                //var picmodel = new
                                //{
                                //    svrIndexCode = hi.Params.Events[i].data.svrIndexCode,
                                //    picUri = hi.Params.Events[i].data.ExtEventPictureURL
                                //};
                                //string body = JsonConvert.SerializeObject(picmodel);
                                //string picPath = "/Resource/HikInImg/" + Guid.NewGuid().ToString() + ".jpeg";
                                ////string Img = SocketHelper.GetHeader(url, body, PicUrl, key, sign);
                                //byte[] result = SocketHelper.HikHttpPost(PicUrl, body, 15);
                                //if (null == result)
                                //{
                                //    SocketHelper.SetLog("UserImg", "人员图片异常", "无返回值");
                                //}
                                //else
                                //{
                                //    string tmp = System.Text.Encoding.UTF8.GetString(result);
                                //    JObject obj = null;
                                //    try
                                //    {
                                //        obj = (JObject)JsonConvert.DeserializeObject(tmp);
                                //        SocketHelper.SetLog("UserImg", "人员图片异常", tmp);

                                //    }
                                //    catch (Exception e)
                                //    {
                                //        SocketHelper.WriteImg(imgPath, result, picPath);
                                //    }
                                //} 
                                #endregion

                                HikinoutlogEntity Hik = new HikinoutlogEntity();
                                Hik.AreaName = device.AreaName;
                                if (USERTYPE == 0)
                                {
                                    Hik.DeptId = user.data.list[0].orgIndexCode;
                                    string[] deptname = user.data.list[0].orgPathName.Split('/');
                                    Hik.DeptName = deptname[deptname.Length - 1];
                                    var dept = deptlist.FirstOrDefault(it => it.DepartmentId == Hik.DeptId);
                                    if (dept.Nature == "承包商")
                                        USERTYPE = 1;
                                }
                                else
                                {
                                    Hik.DeptId = user.data.list[0].orgIndexCode;
                                    Hik.DeptName = "临时人员";
                                }

                                Hik.CreateDate = Convert.ToDateTime(hi.Params.Events[i].happenTime);
                                Hik.CreateUserId = "System";
                                Hik.CreateUserDeptCode = "00";
                                Hik.CreateUserOrgCode = "00";
                                Hik.DeviceName = device.DeviceName;
                                Hik.DeviceType = 0;
                                Hik.EventType = 1;
                                Hik.ID = Guid.NewGuid().ToString();
                                Hik.InOut = device.OutType;
                                Hik.UserId = user.data.list[0].personId;
                                Hik.UserName = user.data.list[0].personName;
                                Hik.UserType = USERTYPE;
                                Hik.ScreenShot = hi.Params.Events[i].data.ExtEventPictureURL;
                                Hik.HikPicSvr = hi.Params.Events[i].data.svrIndexCode;
                                Hik.DeviceHikID = device.HikID;
                                if (device.OutType == 0)
                                    Hik.IsOut = 0;
                                else
                                {
                                    Hik.IsOut = 1;
                                    //如果是出门则要先找到该人员对应的入场记录进行更改
                                    inuser = inoutbll.GetInUser(personid);
                                    if (inuser != null)
                                    {
                                        inuser.IsOut = 1;
                                        inuser.InId = Hik.ID;
                                        inuser.OutTime = Convert.ToDateTime(hi.Params.Events[i].happenTime);
                                        inuser.ModifyDate = Convert.ToDateTime(hi.Params.Events[i].happenTime);
                                        inuser.ModifyUserId = "System";
                                        List<PersononlineEntity> plist = new List<PersononlineEntity>();
                                        int hour = (Convert.ToDateTime(inuser.OutTime) - Convert.ToDateTime(inuser.CreateDate)).Hours;
                                        for (int b = 0; b <= hour; b++)
                                        {
                                            var dp = deptlist.Where(it => it.DepartmentId == inuser.DeptId).FirstOrDefault();
                                            if (dp != null)
                                            {
                                                PersononlineEntity person = new PersononlineEntity();
                                                person.ID = Guid.NewGuid().ToString();
                                                person.TimeNum = Convert.ToDateTime(inuser.CreateDate).AddHours(b);
                                                person.UserId = inuser.UserId;
                                                person.UserName = inuser.UserName;
                                                person.DeptId = inuser.DeptId;
                                                person.DeptName = inuser.DeptName;
                                                person.DeptCode = dp.EnCode;
                                                person.OnlineHour = person.TimeNum.Value.Hour.ToString();
                                                person.OnlineDate = person.TimeNum.Value.ToString("yyyy-MM-dd");
                                                person.LogId = inuser.ID;
                                                person.LogType = 1;
                                                person.CreateDate = Convert.ToDateTime(hi.Params.Events[i].happenTime);
                                                person.CreateUserId = "system";
                                                person.CreateUserDeptCode = "00";
                                                person.CreateUserOrgCode = "00";
                                                plist.Add(person);
                                            }
                                        }
                                        if (plist.Count > 0)
                                        {
                                            PersononlineBLL plbll = new PersononlineBLL();
                                            plbll.Insert(plist);
                                        }
                                    }
                                }
                                inoutbll.UserAisleSave(Hik, inuser);
                            }
                            #endregion
                        }
                    }
                    #endregion 
                }
                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                string fileName = "UserFaceError" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }


        /// <summary>
        /// 设备间门禁回调接口 合法卡通过事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object DeviceAccessBack()
        {
            string json = "";
            try
            {

                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                #region 进入方法记录日志
                string fileName = "UserCardInLog" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录方法调用 Json：" + json);
                #endregion
                HikBack hi = JsonConvert.DeserializeObject<HikBack>(json);
                HikinoutlogEntity inuser = null;
                if (hi.Params.Events.Count > 0)
                {
                    //获取到所有部门集合
                    var deptlist = departmentBLL.GetList();
                    DataItemDetailBLL pdata = new DataItemDetailBLL();
                    var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                    var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址

                    string key = string.Empty;// "21049470";
                    string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                    if (!string.IsNullOrEmpty(pitem))
                    {
                        key = pitem.Split('|')[0];
                        sign = pitem.Split('|')[1];
                    }
                    #region 根据回调事件获取的人员卡号信息查询人员信息
                    string UserUrl = "/artemis/api/resource/v2/person/advance/personList";
                    for (int i = 0; i < hi.Params.Events.Count; i++)
                    {
                        var UserModel = new
                        {
                            pageNo = 1,
                            pageSize = 1000,
                            cardNo = hi.Params.Events[i].data.ExtEventCardNo
                        };

                        string UserMsg = SocketHelper.LoadCameraList(UserModel, url, UserUrl, key, sign);
                        UserSearch user = JsonConvert.DeserializeObject<UserSearch>(UserMsg);
                        if (user.code == "0" && user.data.list.Count > 0)
                        {
                            string personid = user.data.list[0].personId;
                            TemporaryUserEntity TemporaryUser = Tempbll.HikGetUserEntity(personid);
                            int USERTYPE = 0;
                            if (TemporaryUser == null)
                            {
                                USERTYPE = 2;
                            }
                            else
                            {
                                if (TemporaryUser.Istemporary == 0)
                                {
                                    USERTYPE = TemporaryUser.Istemporary;
                                }
                                else
                                {
                                    USERTYPE = 2;
                                }
                            }
                            #region 根据设备ID查询平台中配置的设备信息         
                            HikaccessEntity device = accessBll.HikGetEntity(hi.Params.Events[i].srcIndex);
                            if (device != null)
                            {
                                string Img = "";
                                HikinoutlogEntity Hik = new HikinoutlogEntity();
                                Hik.AreaName = device.AreaName;
                                if (USERTYPE == 0)
                                {
                                    Hik.DeptId = user.data.list[0].orgIndexCode;
                                    string[] deptname = user.data.list[0].orgPathName.Split('/');
                                    Hik.DeptName = deptname[deptname.Length - 1];
                                    var dept = deptlist.FirstOrDefault(it => it.DepartmentId == Hik.DeptId);
                                    if (dept.Nature == "承包商")
                                    {
                                        USERTYPE = 1;
                                    }
                                }
                                else
                                {
                                    Hik.DeptId = user.data.list[0].orgIndexCode;
                                    Hik.DeptName = "临时人员";
                                }   
                                Hik.CreateDate = Convert.ToDateTime(hi.Params.Events[i].happenTime);
                                Hik.CreateUserId = "System";
                                Hik.CreateUserDeptCode = "00";
                                Hik.CreateUserOrgCode = "00";
                                Hik.DeviceName = device.DeviceName;
                                Hik.DeviceType = 2;
                                Hik.EventType = 3;
                                Hik.ID = Guid.NewGuid().ToString();
                                Hik.InOut = device.OutType;
                                Hik.UserId = user.data.list[0].personId;
                                Hik.UserName = user.data.list[0].personName;
                                Hik.UserType = USERTYPE;
                                Hik.ScreenShot = Img;
                                Hik.DeviceHikID = device.HikId;
                                if (device.OutType == 0)
                                {
                                    Hik.IsOut = 0;
                                }
                                else
                                {
                                    Hik.IsOut = 1;
                                    //如果是出门则要先找到该人员对应的入场记录进行更改
                                    inuser = inoutbll.GetInUser(personid);
                                    if (inuser != null)
                                    {
                                        inuser.IsOut = 1;
                                        inuser.InId = Hik.ID;
                                        inuser.OutTime = Convert.ToDateTime(hi.Params.Events[i].happenTime);
                                        inuser.ModifyDate = DateTime.Now;
                                        inuser.ModifyUserId = "System";
                                    }
                                }
                                inoutbll.UserAisleSave(Hik, inuser);
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                string fileName = "CardInLog" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }

        }

        /// <summary>
        /// 设备间门禁回调接口 按钮开门（出门事件）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AccessOutBack()
        {
            string json = "";
            try
            {

                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                #region 进入方法记录日志
                string fileName = "UserCardOutLog" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录方法调用 Json：" + json);
                #endregion
                HikBack hi = JsonConvert.DeserializeObject<HikBack>(json);
                HikinoutlogEntity inuser = null;
                if (hi.Params.Events.Count > 0)
                {

                    DataItemDetailBLL pdata = new DataItemDetailBLL();
                    var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                    var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址

                    string key = string.Empty;// "21049470";
                    string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                    if (!string.IsNullOrEmpty(pitem))
                    {
                        key = pitem.Split('|')[0];
                        sign = pitem.Split('|')[1];
                    }
                    #region 根据回调事件获取的人员卡号信息查询人员信息
                    for (int i = 0; i < hi.Params.Events.Count; i++)
                    {
                        //根据此设备id找到进门记录
                        inuser = inoutbll.DeviceGetLog(hi.Params.Events[i].srcIndex);
                        //将最近一条进门记录状态改为出门
                        inuser.IsOut = 1;
                        inuser.OutTime = Convert.ToDateTime(hi.Params.Events[i].happenTime);

                        HikinoutlogEntity Hik = new HikinoutlogEntity();
                        Hik.DeviceHikID = hi.Params.Events[i].srcIndex;
                        Hik.IsOut = 1;
                        Hik.AreaName = inuser.AreaName;
                        Hik.OutTime = Convert.ToDateTime(hi.Params.Events[i].happenTime);
                        Hik.DeptId = inuser.DeptId;
                        Hik.DeptName = inuser.DeptName;
                        Hik.DeviceName = inuser.DeviceName;
                        Hik.DeviceType = 2;
                        Hik.EventType = 3;
                        Hik.InId = inuser.ID;
                        Hik.InOut = 1;
                        Hik.UserId = inuser.UserId;
                        Hik.UserName = inuser.UserName;
                        Hik.UserType = inuser.UserType;
                        inoutbll.UserAisleSave(Hik, inuser);
                        #endregion
                    }
                }

                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                string fileName = "CardOutLog" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：人员记录存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }

        }

        /// <summary>
        /// 入场压线事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object InPressureLineBack()
        {
            string json = "";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();

                string fileName = "CarInPressureLine" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))                 
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));                  
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：入场压线方法Json:" + json + "\r\n");
                
                HikCarBack hi = JsonConvert.DeserializeObject<HikCarBack>(json);
                foreach (var events in hi.Params.Events)
                {
                    if (events.status == 5 && (events.data.eventCmd == 2 || events.data.eventCmd == 6))
                    {

                        if (events.data.parkName.Contains("一号地磅")) //一号地磅停车场      
                        {
                            CacheFactory.Cache().WriteCache(events.data.roadwayIndex, "PoundB:Roadway");
                        }
                        else if (events.data.parkName.Contains("二号地磅"))//二号地磅停车场      
                        {
                            SendTicket(events.data.plateNo);
                            CacheFactory.Cache().WriteCache(events.data.roadwayIndex, "PoundA:Roadway");
                        }
                        else if (events.data.parkName.Contains("除灰")) //除灰停车场       
                        {
                            CacheFactory.Cache().WriteCache(events.data.roadwayIndex, "PoundC:Roadway");
                        }
                        else
                        {
                            //压线照片存入缓存
                            CacheFactory.Cache().WriteCache(events.data.picUrl.vehiclePicUrl, events.eventId, DateTime.Now.AddMinutes(5));
                        }
                    }
                }
                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                string fileName = "CarInPressureLineError" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：车辆压线存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.InnerException.Message, data = "" };
            }
        }

        /// <summary>
        /// 车牌号
        /// </summary>
        /// <param name="plateNo"></param>
        public void SendTicket(string plateNo)
        {
            object returnData = plateNo;
            int dataType = -1;
            var result = new CalculateBLL().GetEntranceTicket(plateNo);
            if (result != null)
            {
                dataType = 0;
                returnData = result;
            }
            var sendData = new { DataType = dataType, Data = returnData };
            SendMsgToClient("192.168.9.234", sendData.ToJson());
        }

        public void SendMsgToClient(string userid, string msg)
        {
            string url = CacheFactory.Cache().GetCache<string>("SignalRUrl");
            if (url.IsNullOrWhiteSpace())
            {
                url = pdata.GetItemValue("SignalRUrl");
                url = url.Replace("signalr", "").Replace("\"", "");
                CacheFactory.Cache().WriteCache<string>(url, "SignalRUrl");
            }
            HubConnection hubConnection = null;
            IHubProxy ChatsHub = null;
            try
            {
                hubConnection = new HubConnection(url);
                ChatsHub = hubConnection.CreateHubProxy("ChatsHub");
                hubConnection.Start().ContinueWith(task =>
                {
                    if (!task.IsFaulted)
                    //连接成功调用服务端方法
                    {
                        ChatsHub.Invoke("sendMsgKm", userid, msg);
                    }
                });
            }
            catch (Exception)
            {
                //  eventLog1.WriteEntry("创建SingnalR代理出错,异常信息：" + ex.Message);
            }

        }

        /// <summary>
        /// 出厂压线事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object OutPressureLineBack()
        {
            string json = "";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                SocketHelper.SetLog("CarOutPressureLine", "出场压线方法", json);
                json = reader.ReadToEnd();
                HikCarBack hi = JsonConvert.DeserializeObject<HikCarBack>(json);
                foreach (var events in hi.Params.Events)
                {
                    if (events.status == 5 && (events.data.eventCmd == 2 || events.data.eventCmd == 6))
                    {
                        CacheFactory.Cache().WriteCache(events.data.picUrl.vehiclePicUrl, events.eventId, DateTime.Now.AddMinutes(5));
                    }
                }
                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                string fileName = "CarOutPressureLineError" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：车辆压线存储异常：" + e.Message + "Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }


        /// <summary>
        /// 入场放行事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object InReleaseCarBack()
        {
            string json = "";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                SocketHelper.SetLog("CarRelease", "入场放行方法", json);
                SaveCarRelease(json, 0);

                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };

            }
            catch (Exception e)
            {
                string fileName = "CarRelease" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：车辆放行存储异常：" + e.Message + " Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }


        /// <summary>
        /// 出场放行事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object OutReleaseCarBack()
        {
            string json = "";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                SocketHelper.SetLog("CarRelease", "出厂放行方法", json);
                SaveCarRelease(json, 1);
                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                string fileName = "CarReleaseError" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：车辆放行存储异常：" + e.Message + " Json:" + json + "\r\n");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }


        public void SaveCarRelease(string json, int status)
        {
            HikCarBack hi = JsonConvert.DeserializeObject<HikCarBack>(json);

            foreach (var events in hi.Params.Events)
            {
                string deviceCode = events.srcIndex;
                HikdeviceEntity device = devicebll.GetDeviceEntity(deviceCode);
                string CarNo = events.data.plateNo;
                string CarImg = CacheFactory.Cache().GetCache<string>(events.eventId);
                CarinfoEntity car = carbll.GetCar(CarNo);
                CarinlogEntity carlog = new CarinlogEntity();
                carlog.Address = device.AreaName;
                carlog.ID = Guid.NewGuid().ToString();
                carlog.CarNo = CarNo;
                carlog.CreateDate = Convert.ToDateTime(events.happenTime);
                carlog.CreateUserDeptCode = "00";
                carlog.CreateUserId = "System";
                carlog.CreateUserOrgCode = "00";
                carlog.IsOut = 0;
                carlog.Status = status;
                //是否离场默认状态跟进出场一致  如果是出厂数据是否离场默认为是
                carlog.IsLeave = status;
                if (car != null)
                {
                    carlog.CID = car.ID;
                    carlog.Type = car.Type;

                    if (car.Type == 1)
                    {
                        if (car.IsAuthorized == 1)
                        {
                            //如果授权 则记录授权人ID
                            carlog.DriverName = car.AuthUserName;
                            carlog.DriverID = car.AuthUserId;
                            UserBLL ubll = new UserBLL();
                            UserEntity use = ubll.GetEntity(car.AuthUserId);
                            if (use != null)
                            {
                                carlog.Phone = use.Mobile;
                            }
                        }
                        else
                        {
                            carlog.DriverName = car.Dirver;
                            carlog.Phone = car.Phone;
                            carlog.DriverID = car.CreateUserId;
                        }


                    }
                    else
                    {
                        carlog.DriverName = car.Dirver;
                        carlog.Phone = car.Phone;
                        carlog.DriverID = "";
                    }

                    carinbll.BackAddPassLog(carlog, device.DeviceName, CarImg);
                }
                else
                {
                    VisitcarEntity visit = visitbll.NewGetCar(CarNo);
                    if (visit != null)
                    {
                        carlog.DriverName = visit.Dirver;
                        carlog.Phone = visit.Phone;
                        carlog.DriverID = "";
                        carlog.CID = visit.ID;
                        carlog.Type = 3;
                        carinbll.BackAddPassLog(carlog, device.DeviceName, CarImg);
                    }
                    else
                    {
                        OperticketmanagerEntity op = opbll.GetCar(CarNo);
                        if (op != null)
                        {
                            carlog.DriverName = op.DriverName;
                            carlog.Phone = op.DriverTel;
                            carlog.DriverID = "";
                            carlog.CID = op.ID;
                            carlog.Type = 4;
                            carinbll.BackAddPassLog(carlog, device.DeviceName, CarImg);
                        }
                        else
                        {
                            HazardouscarEntity ha = hacarbll.GetCar(CarNo);
                            if (ha != null)
                            {
                                carlog.DriverName = ha.Dirver;
                                carlog.Phone = ha.Phone;
                                carlog.DriverID = "";
                                carlog.CID = ha.ID;
                                carlog.Type = 5;
                                carinbll.BackAddPassLog(carlog, device.DeviceName, CarImg);
                            }
                            else
                            {
                                string fileName = "NotCar" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs")))
                                {
                                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs"));
                                }
                                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": Json:" + json + "\r\n");
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 海康摄像头接口三维调用
        /// <summary>
        /// 获取摄像头集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCameraList()
        {
            try
            {

                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string json = reader.ReadToEnd();

                Pagination pagination = new Pagination();

                string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
                pagination.p_kid = "ID";
                pagination.p_fields = "CREATEUSERID,CREATEDATE,CAMERAID,CAMERANAME,SORT,AREAID,AREANAME";
                pagination.p_tablename = @"BIS_CAMERAMANAGE";
                pagination.conditionJson = " 1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string queryJson = "";

                var data = cameramanagebll.GetPageList(pagination, queryJson);
                return new { Code = 0, Count = 1, Info = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 根据传输进来的摄像头唯一标识返回其播放地址
        /// </summary>
        /// <returns></returns>
        public object GetCameraUrl()
        {
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string json = reader.ReadToEnd();

                string key = string.Empty;// "21049470";
                string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址

                var ckmodel = new
                {
                    cameraIndexCode = json,
                    streamType = 0,
                    protocol = "rtsp",
                    transmode = 1
                };
                string parkMsg = SocketHelper.LoadCameraList(ckmodel, url, "/artemis/api/video/v2/cameras/previewURLs", key, sign);
                VideoReturn pl = JsonConvert.DeserializeObject<VideoReturn>(parkMsg);
                return new { Code = 1, Count = 0, Info = pl.data.url };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        #endregion

        #region 三维导航接口
        /// <summary>
        /// 根据发送过来的车牌号返回导航路线
        /// </summary>
        /// <returns></returns>
        public object GetCarPath()
        {
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string json = reader.ReadToEnd();
                CarNoModel carnomodel = JsonConvert.DeserializeObject<CarNoModel>(json);
                string LineName = "";//路线名称
                //先根据获取到的车牌号查询到其分配的路线ID
                if (carnomodel.CarType == 0)
                {
                    VisitcarEntity car = visitbll.GetCar(carnomodel.CarNo);
                    if (car == null)
                    {
                        return new { Code = -1, Count = 0, Info = "未找到对应拜访车辆信息" };

                    }
                    LineName = car.LineName;
                }
                else if (carnomodel.CarType == 1)
                {
                    OperticketmanagerBLL obll = new OperticketmanagerBLL();
                    OperticketmanagerEntity op = obll.GetCar(carnomodel.CarNo);
                    if (op == null)
                    {
                        return new { Code = -1, Count = 0, Info = "未找到对应物料车辆信息" };

                    }
                    int ISwharf = op.ISwharf;
                    string Transporttype = op.Transporttype;
                    if (Transporttype == "提货")
                    {
                        LineName = op.Dress + Transporttype;
                        if (ISwharf == 1)
                        {
                            LineName += "(码头)";
                        }
                    }
                    else
                    {
                        if (ISwharf == 1)
                        {
                            LineName = "物料转运(码头)";
                        }
                        else
                        {
                            LineName = "转运(纯称重)";
                        }
                    }
                }
                else if (carnomodel.CarType == 2)
                {
                    HazardouscarBLL hbll = new HazardouscarBLL();
                    HazardouscarEntity ha = hbll.GetCar(carnomodel.CarNo);
                    if (ha == null)
                    {
                        return new { Code = -1, Count = 0, Info = "未找到对应危化品车辆信息" };

                    }
                    LineName = ha.HazardousName;
                }
                //获取所有的路线 
                RouteconfigBLL configbll = new RouteconfigBLL();
                var data = configbll.GetRoute();
                Route ro = data.Where(it => it.TypeName == LineName).FirstOrDefault();
                if (ro != null)
                {
                    return new { Code = 1, Count = 1, Info = ro.PointList };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "未找到该车辆对应路线" };
                }

            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion
        #region 三维区域人数列表
        /// <summary>
        /// 获取到一级区域用户人数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetAreaUserList()
        {
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string json = reader.ReadToEnd();
                var data = inoutbll.GetAccPersonNum();
                return new { Code = 1, Count = data.Count, Info = data };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 根据父节点id获取其下所有子节点树
        /// </summary>
        /// <returns></returns>
        public object GetAreaTree()
        {
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                string json = reader.ReadToEnd();
                var data = inoutbll.GetAreaSon(json);
                var cameradata = cameramanagebll.GetCameraList();
                var treeList = new List<TreeEntity>();



                foreach (DistrictEntity item in data)
                {
                    TreeEntity tree = new TreeEntity();
                    bool hasChildren = data.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                    tree.id = item.DistrictID;
                    tree.text = item.DistrictName;
                    tree.value = item.DistrictID;
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item.ParentID;
                    tree.Attribute = "Code";
                    tree.AttributeValue = item.DistrictCode;
                    tree.AttributeA = "IsArea";
                    tree.AttributeValueA = "Area";
                    treeList.Add(tree);
                    var camera = cameradata.Where(it => it.AreaId == item.DistrictID).ToList();
                    if (camera != null && camera.Count > 0)
                    {
                        foreach (var cameraitem in camera)
                        {
                            TreeEntity cameratree = new TreeEntity();
                            cameratree.id = cameraitem.CameraId;
                            cameratree.text = cameraitem.CameraName;
                            cameratree.value = cameraitem.CameraId;
                            cameratree.isexpand = true;
                            cameratree.complete = true;
                            cameratree.hasChildren = true;
                            cameratree.parentId = item.DistrictID;
                            cameratree.Attribute = "Code";
                            cameratree.AttributeValue = "";
                            cameratree.AttributeA = "IsArea";
                            cameratree.AttributeValueA = "Camera";
                            treeList.Add(cameratree);
                        }
                    }
                }
                //if (treeList.Count > 0)
                //{
                //    treeList[0].isexpand = true;
                //}
                return new { Code = 1, Count = treeList.Count, Info = treeList };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion


        #region 华电可门手机app调用（双控宝）

        /// <summary>
        /// 获取权限范围内待审批记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetStayApprovalRecord([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.userid;
                string tel = dy.data.tel;
                string sql = string.Format(@" select *
            from (select ID,'拜访(无车)' as purpose, dirver, phone,d.carno as comname,d.venuestate as carno, d.accompanyingperson,d.intime, createdate,2 type,d.visituserphone as tel,applydate
            from BIS_USERCAR d
            where state > 0
            and state < 4 and appstatue=0
            union
            select ID,'拜访(有车)' as purpose, dirver, phone,t.comname,t.carno,t.accompanyingperson,t.intime, createdate,1 type,t.visituserphone as tel,applydate
            from bis_visitcar t
            where state > 0
             and state < 4 and appstatue=0
            ) a1 where tel='{0}'
            order by Createdate desc  ", tel);
                var data = opbll.GetDataTable(sql);
                List<CarEntity> list = new List<CarEntity>();
                foreach (DataRow Rows in data.Rows)
                {//待审批
                    CarEntity entity = new CarEntity();
                    entity.Id = Rows[0].ToString();
                    entity.purpose = Rows[1].ToString();
                    entity.dirver = Rows[2].ToString();
                    entity.phone = Rows[3].ToString();
                    entity.comname = Rows[4].ToString();
                    entity.carno = Rows[5].ToString();
                    entity.accompanyingperson = Rows[6].ToString();
                    entity.sumbmittime = Rows[8].ToString();
                    entity.type = Rows[9].ToString();
                    entity.intime = DateTime.Parse(Rows[11].ToString()).ToString("yyyy-MM-dd HH:mm");
                    list.Add(entity);
                }
                var dt = GetApprovalAdoptNoOutRrcord(tel);
                foreach (DataRow Rows in dt.Rows)
                {//待出厂
                    CarEntity entity = new CarEntity();
                    entity.Id = Rows[0].ToString();
                    entity.Name = Rows[1].ToString();
                    entity.DriverName = Rows[2].ToString();
                    entity.dirver = Rows[2].ToString();
                    entity.phone = Rows[3].ToString();
                    entity.InspectUser = Rows[4].ToString();
                    entity.ReceiveUser = Rows[5].ToString();
                    //entity.position = Rows[7].ToString();
                    entity.type = Rows[9].ToString();
                    entity.intime = Rows[10].ToString();
                    list.Add(entity);
                }
                return new { Code = 0, Count = data.Rows.Count, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }


        /// <summary>
        /// 获取权限范围内审批通过未出厂审批记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable GetApprovalAdoptNoOutRrcord(string tel)
        {
            try
            {
                string sql = string.Format(@" select *
           from (select ID,d.gpsname as name, dirver, phone,d.visituser as pname,d.carno as hname, visitdept,gpsid, createdate,3 type,intime
           from BIS_USERCAR d
           where state > 2
           and state < 4 and appstatue=1 and visituserphone='{0}'
           union
           select ID,t.gpsname as name, dirver, phone,t.comname as pname,t.carno as hname, visitdept,gpsid, createdate,3 type,intime
           from bis_visitcar t
           where state > 2
           and state < 4 and appstatue=1 and visituserphone='{0}'
           union 
           select id, s.hazardousname as name,s.dirver,s.phone,s.processingname as pname,s.handovername as hname,note as visitdept,gpsid,createdate,4 type,intime from bis_hazardouscar s
           where state > 2
           and state < 4 
          ) a1 
          order by Createdate desc ", tel);
                return opbll.GetDataTable(sql);
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }



        /// <summary>
        /// 拜访人员审批状态提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UpdateApprovalItemState([FromBody] JObject json)
        {
            string msg = "操作成功";
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string pid = dy.data.pId;//主键
                string State = dy.data.State;//状态

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var ventity = new VisitcarBLL().GetEntity(pid);
                var uentity = new CarUserBLL().GetEntity(pid);
                if (ventity != null)
                {//拜访车辆
                    if (ventity.State == 88)
                        return new { Code = -1, Count = 0, Info = "审核失败,该记录处于已撤销状态无法审核！" };
                    ventity.AppStatue = int.Parse(State);
                    if (int.Parse(State) == 2) { ventity.State = 99; }
                    new VisitcarBLL().SaveForm(pid, ventity);
                    SendMsgToClient("menwei", ventity.ToJson());
                }
                else if (uentity != null)
                {//拜访人员
                    if (uentity.State == 88)
                        return new { Code = -1, Count = 0, Info = "审核失败,该记录处于已撤销状态无法审核！" };
                    uentity.AppStatue = int.Parse(State);
                    if (int.Parse(State) == 2) { uentity.State = 99; }
                    new CarUserBLL().SaveForm(pid, uentity, null);
                    SendMsgToClient("menwei", uentity.ToJson());
                }
                else
                {
                    msg = "找不到对应的记录！";
                }
                return new { Code = 0, Count = 0, Info = msg };
            }
            catch (Exception e)
            {
                return new { Code = -1, Count = 0, Info = e.Message };
            }
        }

        /// <summary>
        /// 人员拜访序列化实体
        /// </summary>
        public class CarEntity
        {
            /// <summary>
            ///主键
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 目的
            /// </summary>
            public string purpose { get; set; }
            /// <summary>
            /// 申请人
            /// </summary>
            public string dirver { get; set; }
            /// <summary>
            /// 手机号码
            /// </summary>
            public string phone { get; set; }
            /// <summary>
            /// 单位
            /// </summary>
            public string comname { get; set; }
            /// <summary>
            /// 车牌号码
            /// </summary>
            public string carno { get; set; }
            /// <summary>
            /// 随行人员
            /// </summary>
            public string accompanyingperson { get; set; }
            /// <summary>
            /// 入场时间
            /// </summary>
            public string intime { get; set; }

            /// <summary>
            /// 提交时间
            /// </summary>
            public string sumbmittime { get; set; }

            /// <summary>
            /// 类型 1.2待审批 3审批通过未出厂 4 危化品
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 危化品名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 司机姓名
            /// </summary>
            public string DriverName { get; set; }

            /// <summary>
            /// 检查人
            /// </summary>
            public string InspectUser { get; set; }
            /// <summary>
            /// 接收人
            /// </summary>
            public string ReceiveUser { get; set; }
            /// <summary>
            /// 当前位置
            /// </summary>
            public string position { get; set; }

        }

        #endregion

        #region 华电可门园区卡口事件回调
        /// <summary>
        /// 园区卡口事件回调 （超速、违停）
        /// </summary>
        /// <returns></returns>
        public object OverSpeedCallBack()
        {
            string json = "";
            try
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                json = reader.ReadToEnd();
                WriteLog.AddLog("园区卡口超速事件：" + json, "OverSpeed");
                //将数据添加到车辆预警信息表
                var hikCallBack = JsonConvert.DeserializeObject<HikCallBackModel<OverSpeedModel>>(json);
                var eventList = hikCallBack.Params.Events;
                if (eventList != null && eventList.Count > 0)
                {
                    eventList.ForEach(x =>
                    {
                        try
                        {
                            CarviolationEntity entity = new CarviolationEntity()
                            {
                                ID = Guid.NewGuid().ToString(),
                                CardNo = x.data.plateNo,
                                CID = x.data.eventIndex,
                                CreateDate = DateTime.Now,
                                CreateUserDeptCode = "0",
                                CreateUserId = "0",
                                CreateUserOrgCode = "0",
                                Dirver = x.data.person == null ? string.Empty : x.data.person.personName,
                                IsProcess = 0,
                                ModifyDate = DateTime.Now,
                                ModifyUserId = "0",
                                Phone = x.data.person == null ? "" : x.data.person.phoneNo,
                                ViolationMsg = x.data.plateNo + " " + x.data.monitorName + "  车速" + x.data.speed + "km/h",
                                ViolationType = 0,
                                Address = x.data.monitorName,
                                Speed = x.data.speed,
                                platePicUrl = x.data.picUrl.platePicUrl,
                                vehiclePicUrl = x.data.picUrl.vehiclePicUrl,
                                HikPicSvr = x.data.imageIndexCode,
                                vehicleTypeName = x.data.vehicleTypeName
                            };               
                            viobll.Insert(entity);
                        }
                        catch (Exception ex)
                        {
                            WriteLog.AddLog("园区卡口事件存储异常：" + JsonConvert.SerializeObject(ex) + "\r\n" + json, "OverSpeed");
                        }

                    });
                }
                return new { Code = 0, Count = 1, Info = "接受数据成功", data = "" };
            }
            catch (Exception e)
            {
                WriteLog.AddLog("园区卡口事件存储异常：" + JsonConvert.SerializeObject(e) + "\r\n" + json, "OverSpeed");
                return new { Code = -1, Count = 0, Info = e.Message, data = "" };
            }
        }



        #region 海康事件回调实体
        public class HikCallBackModel<T> where T : class, new()
        {
            /// <summary>
            /// 通知方法名称 默认事件类通知名称为”OnEventNotify”
            /// </summary>
            public string method { get; set; }
            [JsonProperty("params")]
            public Params<T> Params { get; set; }
        }
        /// <summary>
        /// 事件参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Params<T> where T : class, new()
        {
            /// <summary>
            /// 事件分类 门禁事件默认为”event_acs”
            /// </summary>
            public string ability { get; set; }

            public List<EventModel<T>> Events { get; set; }
            /// <summary>
            /// 事件发送时间
            /// </summary>
            public string sendTime { get; set; }
        }
        /// <summary>
        /// 事件主体
        /// </summary>
        public class EventModel<T> where T : class, new()
        {
            /// <summary>
            /// 	事件唯一标识
            /// </summary>
            public string eventId { get; set; }

            /// <summary>
            /// 事件源编号，物理设备是资源编号
            /// </summary>
            public string srcIndex { get; set; }

            /// <summary>
            /// 事件源资源类型 
            /// monitorPoint：园区-卡口点
            /// </summary>
            public string srcType { get; set; }

            /// <summary>
            /// 事件状态
            /// 0-瞬时
            /// 1-开始
            ///2-停止
            ///4-事件联动结果更新
            ///5-事件图片异步上传
            /// </summary>
            public int status { get; set; }


            /// <summary>
            /// 脉冲超时时间
            ///  单位：秒
            /// </summary>
            public int timeout { get; set; }

            /// <summary>
            /// 事件发生时间（设备时间）
            /// ISO8601，示例：2018-08-15T 15:53:47.000+08:00
            /// </summary>
            public DateTime happenTime { get; set; }

            /// <summary>
            /// 事件发生的事件源父设备
            /// </summary>
            public string srcParentIdex { get; set; }

            /// <summary>
            /// 事件发生的事件源父设备
            /// </summary>
            public T data { get; set; }
        }

        /// <summary>
        /// 超速事件详情
        /// </summary>
        public class OverSpeedModel
        {
            /// <summary>
            /// 事件唯一标识
            /// </summary>
            public string eventIndex { get; set; }

            /// <summary>
            /// 车牌号
            /// </summary>
            public string plateNo { get; set; }

            /// <summary>
            /// 车牌类型key
            /// </summary>
            public string plateType { get; set; }

            /// <summary>
            /// 车牌类型名称
            /// </summary>
            public string plateTypeName { get; set; }

            /// <summary>
            /// 	车辆类型key
            /// </summary>
            public string vehicleType { get; set; }

            /// <summary>
            /// 	车辆类型名称
            /// </summary>
            public string vehicleTypeName { get; set; }


            /// <summary>
            /// 	过车时间
            /// </summary>
            public string crossTime { get; set; }

            /// <summary>
            /// 整型，速度值  
            /// 单位km/h
            /// </summary>
            public int speed { get; set; }

            /// <summary>
            /// 布控类型
            /// 1-被盗车
            ///2-被抢车
            ///3-嫌疑车
            ///4-交通违法车
            ///5-紧急查控车
            ///6-违章车
            /// </summary>
            public string alarmType { get; set; }

            /// <summary>
            ///布控类型名称
            /// 单位km/h
            /// </summary>
            public string alarmTypeName { get; set; }

            /// <summary>
            ///卡口点主键
            /// </summary>
            public string monitorId { get; set; }

            /// <summary>
            ///	卡口点名称
            /// </summary>
            public string monitorName { get; set; }

            /// <summary>
            ///	整型，违法类型
            /// </summary>
            public int illegalType { get; set; }

            /// <summary>
            ///	点位或是区间名称
            /// </summary>
            public string mixedName { get; set; }

            /// <summary>
            ///		点位测速或是区间测速类型
            ///		1–点位测速
            ///2–区间测速
            /// </summary>
            public int mixedType { get; set; }

            /// <summary>
            ///点位或是区间id
            /// </summary>
            public string mixedId { get; set; }

            /// <summary>
            /// 卡口点编号
            /// </summary>
            public string monitorIndexCode { get; set; }

            /// <summary>
            /// 包含车牌和车辆url
            /// </summary>
            public picUrl picUrl { get; set; }

            /// <summary>
            /// 图片服务器编号
            /// </summary>
            public string imageIndexCode { get; set; }

            /// <summary>
            /// 超速阈值 单位 km/h
            /// </summary>
            public string speedLimit { get; set; }

            /// <summary>
            /// 车主信息
            /// </summary>
            public person person { get; set; }
        }

        /// <summary>
        /// 包含车牌和车辆url
        /// </summary>
        public class picUrl
        {
            /// <summary>
            /// 车牌url
            /// 此参数和imageIndexCode参数的值作为入参，从获取卡口事件图片接口获取图片
            /// </summary>
            public string platePicUrl { get; set; }

            /// <summary>
            ///车辆url
            ///此参数和imageIndexCode参数的值作为入参，从获取卡口事件图片接口获取图片
            /// </summary>
            public string vehiclePicUrl { get; set; }
        }

        /// <summary>
        /// 车主信息
        /// </summary>
        public class person
        {

            /// <summary>
            /// 车主姓名
            /// </summary>
            public string personName { get; set; }

            /// <summary>
            /// 车主电话	
            /// </summary>
            public string phoneNo { get; set; }
        }

        #endregion

        #endregion



        #region 黄金埠手机接口门禁



        /// <summary>
        /// 
        /// </summary>
        public class FactoryEntity
        {
            public string fullname { get; set; }
            public string person { get; set; }
            public string usertype { get; set; }
            public List<RealUserEntity> Ulist { get; set; }
        }

        public class RealUserEntity
        {
            public string id { get; set; }
            public string userid { get; set; }
            public string username { get; set; }
            public string gender { get; set; }
            public string dutyname { get; set; }
            public string devicename { get; set; }
            public string outtime { get; set; }

        }



        /// <summary>
        /// 获取实时在厂人员
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetRealTimeInTheFactoryList([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var userId = dy.userid;
                string type = dy.data.Type == "0" ? "内部" : "长协临时";
                string Name = dy.data.UserName;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                //string queryJson = new
                //{
                //    Name = Name
                //}.ToJson();
                var data = new HikinoutlogBLL().GetAllDepartment(null);
                List<FactoryEntity> list = new List<FactoryEntity>();
                foreach (DataRow Rows in data.Rows)
                {
                    string utype = Rows["depttype"].ToString();
                    if (!type.Contains(utype)) continue;
                    FactoryEntity entity = new FactoryEntity();
                    entity.fullname = Rows["fullname"].ToString();
                    entity.person = Rows["person"].ToString();
                    entity.usertype = utype;
                    entity.Ulist = GetRealTimeUserList(entity.fullname, "");
                    list.Add(entity);
                }
                return new { Code = 0, Count = data.Rows.Count, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }

        /// <summary>
        /// 获取实时在厂人员详细列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public List<RealUserEntity> GetRealTimeUserList(string deptName, string personName)
        {
            List<RealUserEntity> list = new List<RealUserEntity>();
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                //int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.userid";
                pagination.p_fields = "b.REALNAME,b.gender,b.dutyname,a.devicename,a.CREATEDATE";
                pagination.p_tablename = @"(select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.userid = b.userid left join (select * from HJB_PERSONSET where MODULETYPE = 0) t on a.userid = t.userid";
                pagination.conditionJson = " a.inout = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)";
                //pagination.records = 0;
                pagination.page = 1;//页数
                pagination.rows = 10000000;//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";
                var data = new HikinoutlogBLL().GetTableByDeptname(pagination, deptName, personName);
                foreach (DataRow Rows in data.Rows)
                {
                    RealUserEntity entity = new RealUserEntity();
                    //entity.id = Rows["id"].ToString();
                    entity.userid = Rows["userid"].ToString();
                    entity.username = Rows["realname"].ToString();
                    entity.gender = Rows["gender"].ToString();
                    entity.dutyname = Rows["dutyname"].ToString();
                    entity.devicename = Rows["devicename"].ToString();
                    entity.outtime = Rows["createdate"].ToString();
                    list.Add(entity);
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        /// <summary>
        /// 获取实时人员在厂统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetRealTimeUserCount([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                string sql = string.Format(@"select count(distinct(a.deptname)) as num from bis_hikinoutlog a 
                                        left join V_USERINFO b on a.deptid = b.departmentid 
                                        where nature = '承包商' and length(encode) <= 23 and b.ISEPIBOLY = '是' and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
                DataTable dt = operticketmanagerbll.GetDataTable(sql);
                object outsourcing = dt.Rows[0]["num"].ToString();
                string sql2 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
                DataTable dt2 = operticketmanagerbll.GetDataTable(sql2);
                object allPerson = dt2.Rows[0]["num"].ToString();
                string sql3 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0) and b.ISEPIBOLY = '否' and b.DEPTTYPE is null");
                DataTable dt3 = operticketmanagerbll.GetDataTable(sql3);
                object inPerson = dt3.Rows[0]["num"].ToString();
                string sql4 = string.Format(@"select count(distinct(a.userid)) as num from bis_hikinoutlog a
left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0) and  b.DEPTTYPE is not null");
                DataTable dt4 = operticketmanagerbll.GetDataTable(sql4);
                object outPerson = dt4.Rows[0]["num"].ToString();

                List<dynamic> PersonData = new List<dynamic>();
                PersonData.Add(new
                {
                    outsourcing = outsourcing,
                    allPerson = allPerson,
                    inPerson = inPerson,
                    outPerson = outPerson
                });

                return new { Code = 0, Count = 4, Info = "获取数据成功", data = PersonData };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }

        /// <summary>
        ///  获取进出门禁记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetInAndOurUserRecord([FromBody] JObject json)
        {
            var result = new List<HikinoutlogEntity>();
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var loginUserId = dy.userid;
                var userId = dy.userid;
                if (dy.data != null && !string.IsNullOrEmpty(dy.data.userid))
                    userId = dy.data.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = loginUserId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var queryString = new
                {
                    UserId = userId,
                    StartDate = DateTime.Now.AddMonths(-1).Date,
                    EndDate = DateTime.Now.Date
                }.ToJson();
                bool isAppintAccount = false;
                string accounts = new DataItemDetailBLL().GetItemValue("SpecialAccount", "HjbBasice")?.ToLower();
                if (!string.IsNullOrEmpty(accounts))
                {
                    List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (accountArray.Contains(user.Account.ToLower()))
                        isAppintAccount = true;
                }
                string sqlTemp = string.Format("select count(userid) from hjb_personset where moduletype=0 and userid='{0}'", userId);
                DataTable dtItems = new OperticketmanagerBLL().GetDataTable(sqlTemp);
                string isSpecailUser = dtItems.Rows[0][0].ToString();
                if (!isAppintAccount)
                {
                    if ((isSpecailUser == "1" && userId == loginUserId) || isSpecailUser == "0")
                        result = new HikinoutlogBLL().GetList(queryString).ToList();
                }
                else
                    result = new HikinoutlogBLL().GetList(queryString).ToList();

                return new { Code = 0, Count = 0, Info = "获取数据成功", data = result };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message, data = "" };
            }
        }




        #endregion

        /// <summary>
        /// 获取区域相关的隐患风险或作业信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetAreaStatus([FromBody] JObject json)
        {
            try
            {
                var arealist = arealocationbll.GetRiskTable();
                var risklist = new DesktopBLL().GetKMAreaStatus();

                var hiddenlist = arealocationbll.GetHiddenCount();
                for (int i = 0; i < arealist.Count; i++)
                {
                    var risk = risklist.Where(it => arealist[i].DistrictCode.Contains(it.areacode)).OrderBy(it => it.gradeval).ToList();
                    if (risk.Count > 0)
                    {
                        arealist[i].Level = risk[0].gradeval;
                    }
                    var hidden = hiddenlist.Where(it => arealist[i].DistrictCode.Contains(it.areacode)).ToList();
                    if (hidden.Count > 0)
                    {
                        arealist[i].HtNum = hidden.Sum(it => it.htcount);
                    }
                }
                return new { Code = 1, Count = arealist.Count, Info = "获取数据成功", data = arealist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
    }
}


