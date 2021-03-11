using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web.Http;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using ERCHTMS.AppSerivce.Models;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.KbsDeviceManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class KbsBaseManageController : BaseApiController
    {
        private LablemanageBLL lablebll = new LablemanageBLL();
        private UserBLL userbll = new UserBLL();
        private LocationBLL lobll = new LocationBLL();
        private LableonlinelogBLL lablelogbll = new LableonlinelogBLL();
        private OfflinedeviceBLL odbll = new OfflinedeviceBLL();
        private KbscameramanageBLL camerabll = new KbscameramanageBLL();
        private KbsdeviceBLL devicebll = new KbsdeviceBLL();
        private BaseStationBLL stationBll = new BaseStationBLL();
        private ArealocationBLL arealocationbll = new ArealocationBLL();
        private DistrictBLL disbll = new DistrictBLL();
        private WorkcameracaptureBLL wccaputuerbll = new WorkcameracaptureBLL();
        private RiskAssessBLL riskAssessBLL = new RiskAssessBLL();
        /// <summary>
        /// 查看本人是否绑定标签
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetUserLable([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var lableentity = lablebll.GetUserLable(userId);
                if (lableentity != null)
                {
                    string bt = Convert.ToDateTime(lableentity.BindTime).ToString("yyyy-MM-dd HH:mm:ss");
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = new { ID = lableentity.ID, LableId = lableentity.LableId, Power = "100%", BindTime = bt } };
                }
                else
                {
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = new { ID = "", LableId = "", Power = "", BindTime = "" } };
                }



            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 头不足位数补0
        /// </summary>
        /// <param name="cardno"></param>
        /// <returns></returns>
        private string GetLableZero(string cardno, int MaxLength)
        {
            if (cardno.Length < MaxLength)
            {
                int len = MaxLength - cardno.Length;
                string top = "";
                for (int i = 0; i < len; i++)
                {
                    top += "0";
                }

                cardno = top + cardno;
            }
            return cardno;
        }

        /// <summary>
        /// 绑定用户本人标签
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object BindMeLable([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string LableId = dy.data.LableId;
                LableId = GetLableZero(LableId, 6);
                string LableTypeName = dy.data.LableTypeName;
                string LableTypeId = dy.data.LableTypeId;
                if (lablebll.GetIsBind(LableId))
                {
                    return new { Code = -1, Count = 0, Info = "此标签已绑定，请更换别的标签进行绑定" };
                }

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var userdq = userbll.GetEntity(userId);
                LablemanageEntity lable = new LablemanageEntity();
                lable.Type = 0;
                lable.BindTime = DateTime.Now;
                lable.IsBind = 1;
                lable.UserId = user.UserId;
                lable.DeptCode = user.DeptCode;
                lable.DeptId = user.DeptId;
                lable.DeptName = user.DeptName;
                lable.LableId = LableId;
                lable.IdCardOrDriver = user.IdentifyID;
                lable.LableTypeId = LableTypeId;
                lable.LableTypeName = LableTypeName;
                lable.Name = user.UserName;
                lable.Phone = userdq.Mobile;
                lable.OperUserId = user.UserName;
                lable.State = "离线";
                lable.Power = "100%";
                lable.Create();
                lablebll.SaveForm("", lable);
                //将新绑定的标签信息同步到后台计算服务中
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "LableEntity";
                sd.EntityString = JsonConvert.SerializeObject(lable);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
                return new { Code = 0, Count = 0, Info = "绑定成功" };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 绑定用户标签
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object BindUserLable([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string username = dy.data.BindUserName;
                string BindUserId = dy.data.BindUserId;
                string IdCard = dy.data.IdCard;
                string LableId = dy.data.LableId;
                LableId = GetLableZero(LableId, 6);
                string Phone = dy.data.Phone;
                string LableTypeName = dy.data.LableTypeName;
                string LableTypeId = dy.data.LableTypeId;
                string DeptId = dy.data.DeptId;
                string DeptCode = dy.data.DeptCode;
                string DeptName = dy.data.DeptName;

                if (lablebll.GetIsBind(LableId))
                {
                    return new { Code = -1, Count = 0, Info = "此标签已绑定，请更换别的标签进行绑定" };
                }

                if (BindUserId != "")
                {
                    var lableentity = lablebll.GetUserLable(BindUserId);
                    if (lableentity != null)
                    {
                        return new { Code = -1, Count = 0, Info = "此用户已经绑定标签,请勿重复绑定" };
                    }
                }

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                LablemanageEntity lable = new LablemanageEntity();
                lable.Type = 0;
                lable.BindTime = DateTime.Now;
                lable.IsBind = 1;
                lable.UserId = BindUserId;
                lable.DeptCode = DeptCode;
                lable.DeptId = DeptId;
                lable.DeptName = DeptName;
                lable.LableId = LableId;
                lable.IdCardOrDriver = IdCard;
                lable.LableTypeId = LableTypeId;
                lable.LableTypeName = LableTypeName;
                lable.Name = username;
                lable.Phone = Phone;
                lable.OperUserId = user.UserName;
                lable.State = "离线";
                lable.Power = "100%";
                lable.Create();
                lablebll.SaveForm("", lable);
                //将新绑定的标签信息同步到后台计算服务中
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "LableEntity";
                sd.EntityString = JsonConvert.SerializeObject(lable);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
                return new { Code = 0, Count = 0, Info = "绑定成功" };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 绑定车辆标签
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object BindCarLable([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string CarNo = dy.data.CarNo;//车牌
                string Driver = dy.data.Driver;//驾驶员
                string userId = dy.userid;
                string LableId = dy.data.LableId;
                LableId = GetLableZero(LableId, 6);
                string Phone = dy.data.Phone;
                string LableTypeName = dy.data.LableTypeName;
                string LableTypeId = dy.data.LableTypeId;
                string DeptId = dy.data.DeptId;
                string DeptCode = dy.data.DeptCode;
                string DeptName = dy.data.DeptName;

                if (lablebll.GetIsBind(LableId))
                {
                    return new { Code = -1, Count = 0, Info = "此标签已绑定，请更换别的标签进行绑定" };
                }



                var lableentity = lablebll.GetCarLable(CarNo);
                if (lableentity != null)
                {
                    return new { Code = -1, Count = 0, Info = "此车辆已经绑定标签,请勿重复绑定" };
                }

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                LablemanageEntity lable = new LablemanageEntity();
                lable.Type = 1;
                lable.BindTime = DateTime.Now;
                lable.IsBind = 1;
                lable.UserId = "";
                lable.DeptCode = DeptCode;
                lable.DeptId = DeptId;
                lable.DeptName = DeptName;
                lable.LableId = LableId;
                lable.IdCardOrDriver = Driver;
                lable.LableTypeId = LableTypeId;
                lable.LableTypeName = LableTypeName;
                lable.Name = CarNo;
                lable.Phone = Phone;
                lable.OperUserId = user.UserName;
                lable.State = "离线";
                lable.Power = "100%";
                lable.Create();
                lablebll.SaveForm("", lable);
                //将新绑定的标签信息同步到后台计算服务中
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "LableEntity";
                sd.EntityString = JsonConvert.SerializeObject(lable);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
                return new { Code = 0, Count = 0, Info = "绑定成功" };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取绑定标签列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLableList([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string LableTypeId = dy.data.LableTypeId;
                string BindTime = dy.data.BindTime;
                string DeptCode = dy.data.DeptCode;
                int type = Convert.ToInt32(dy.data.Type);
                var data = lablebll.GetList(LableTypeId, DeptCode, BindTime, type);
                return new { Code = 0, Count = data.Count, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }


        /// <summary>
        /// 获取绑定标签详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLableDetail([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string ID = dy.data.ID;
                var data = lablebll.GetEntity(ID);
                return new { Code = 0, Count = 1, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 标签解绑
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object LableUntie([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string ID = dy.data.ID;
                lablebll.Untie(ID);
                return new { Code = 0, Count = 0, Info = "解绑成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }


        /// <summary>
        /// 获取标签类别列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object LableType([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                int type = Convert.ToInt32(dy.data.Type);
                DataItemBLL di = new DataItemBLL();
                //先获取到字典项
                DataItemEntity DataItems = di.GetEntityByCode("LabelType");

                DataItemDetailBLL did = new DataItemDetailBLL();
                //根据字典项获取值
                IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

                var data = didList.Where(it => it.Description.Contains(type.ToString())).OrderBy(it => it.SortCode).ToList();
                return new { Code = 0, Count = data.Count, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 根据lableid获取是否有绑定数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLable([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string LableId = dy.data.LableId;
                LableId = GetLableZero(LableId, 6);
                LablemanageEntity lb = lablebll.GetLable(LableId);
                string id = "";
                int type = 0;
                if (lb != null)
                {
                    id = lb.ID;
                    type = Convert.ToInt32(lb.Type);
                }

                var data = new
                {
                    ID = id,
                    Type = type
                };
                return new { Code = 0, Count = 1, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 根据时间搜索历史轨迹
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLocation([FromBody] JObject json)
        {
            try
            {
                string LableId = json.Value<string>("LableId");
                string StartTime = json.Value<string>("StartTime");
                string EndTime = json.Value<string>("EndTime");

                DateTime st = Convert.ToDateTime(StartTime);
                DateTime et = Convert.ToDateTime(EndTime);
                DateTime newet = et.AddHours(3);
                var lolist = lobll.GetLocation(LableId, st, newet);
                List<KbsGPSPoint> rtnpoint = new List<KbsGPSPoint>();
                foreach (var item in lolist)
                {
                    List<KbsGPSPoint> polist = JsonConvert.DeserializeObject<List<KbsGPSPoint>>(item.PointList);
                    var plist = polist.Where(it => it.CreateTime >= st && it.CreateTime <= et).ToList();
                    rtnpoint = rtnpoint.Union<KbsGPSPoint>(plist).ToList();
                }
                rtnpoint = rtnpoint.OrderBy(it => it.CreateTime).ToList();
                return new { Code = 0, Count = 1, Info = "获取成功", data = rtnpoint };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 根据摄像头ID获取画面
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetVideo([FromBody] JObject json)
        {
            try
            {
                string CameraId = json.Value<string>("CameraId");
                string returnUrl = "";
                string Cameraurl = "/artemis/api/video/v2/cameras/previewURLs";
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
                string key = string.Empty;
                string sign = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    key = pitem.Split('|')[0];
                    sign = pitem.Split('|')[1];
                }
                var Model = new
                {
                    cameraIndexCode = CameraId,
                    protocol = "rtmp"
                };

                HttpUtillibKbs.SetPlatformInfo(key, sign, url, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(Cameraurl, Model.ToJson(), 20);
                string CameraMsg = System.Text.Encoding.UTF8.GetString(result);
                CameraRtn cr = JsonConvert.DeserializeObject<CameraRtn>(CameraMsg);
                returnUrl = cr.data.url;
                return new { Code = 0, Count = 1, Info = "获取成功", data = returnUrl };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取摄像头历史视频
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetVedioReplay([FromBody] JObject json)
        {
            try
            {
                string CameraId = json.Value<string>("CameraId");
                string StartTime = json.Value<string>("StartTime");
                string EndTime = json.Value<string>("EndTime");
                string returnUrl = "";

                string Cameraurl = "/artemis/api/video/v2/cameras/playbackURLs";
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
                string key = string.Empty;// 
                string sign = string.Empty;// 
                if (!string.IsNullOrEmpty(pitem))
                {
                    key = pitem.Split('|')[0];
                    sign = pitem.Split('|')[1];
                }
                var Model = new
                {
                    cameraIndexCode = CameraId,
                    protocol = "rtmp",
                    beginTime = StartTime,
                    endTime = EndTime
                };
                //string CameraMsg = SocketHelper.LoadCameraList(Model, url, Cameraurl, key, sign);
                HttpUtillibKbs.SetPlatformInfo(key, sign, url, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(Cameraurl, Model.ToJson(), 20);
                string CameraMsg = System.Text.Encoding.UTF8.GetString(result);
                CameraRtn cr = JsonConvert.DeserializeObject<CameraRtn>(CameraMsg);
                returnUrl = cr.data.url;
                return new { Code = 0, Count = 1, Info = "获取成功", data = returnUrl };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SendMeassge([FromBody] JObject json)
        {
            try
            {
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                string title = json.Value<string>("Title");
                string Content = json.Value<string>("Content");
                //获取需要发送的管理账号
                string account = itemBll.GetItemValue("WarningUser");
                string[] accounts = account.Split(',');
                JPushApi.KbsPushMeassge(accounts[0], accounts[1], title, "隐患排查", Content);
                return new { Code = 0, Count = 1, Info = "发送成功" };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #region 给三维调用接口
        /// <summary>
        /// 获取区域范围详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAreaLocation([FromBody] JObject json)
        {
            try
            {
                var data = arealocationbll.GetTable();
                return new { Code = 0, Count = 1, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 保存区域范围
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SetAreaLocation([FromBody] JObject json)
        {
            try
            {
                string ID = json.Value<string>("ID");
                string DistrictId = json.Value<string>("DistrictId");
                string PostList = json.Value<string>("PostList");
                string ModelIds = json.Value<string>("ModelIds");
                var dis = disbll.GetEntity(DistrictId);
                ArealocationEntity Area = new ArealocationEntity();
                Area.AreaCode = dis.DistrictCode;
                Area.AreaId = DistrictId;
                Area.AreaName = dis.DistrictName;
                Area.AreaParentId = dis.ParentID;
                if (ID == "")
                {
                    Area.ID = Guid.NewGuid().ToString();
                    Area.CreateDate = DateTime.Now;
                    Area.CreateUserId = "System";
                    Area.CreateUserDeptCode = "00";
                    Area.CreateUserOrgCode = "00";
                }
                else
                {
                    Area.ID = ID;
                    Area.ModifyDate = DateTime.Now;
                    Area.ModifyUserId = "System";
                }
                Area.AreaParentId = dis.ParentID;
                Area.PointList = PostList;
                Area.ModelIds = ModelIds;
                arealocationbll.SaveForm(ID, Area);

                KbsAreaLocation ka = new KbsAreaLocation();
                ka.DistrictCode = Area.AreaCode;
                ka.DistrictID = Area.AreaId;
                ka.DistrictName = Area.AreaName;
                ka.ID = Area.ID;
                ka.ModelIds = Area.ModelIds;
                ka.OrganizeId = dis.OrganizeId;
                ka.ParentID = Area.AreaParentId;
                ka.PointList = Area.PointList;
                ka.SortCode = dis.SortCode;
                SendData sd = new SendData();
                if (ID == "")
                {
                    sd.DataName = "AddArea";
                }
                else
                {
                    sd.DataName = "UpdateArea";
                }
                sd.EntityString = JsonConvert.SerializeObject(ka);

                //将新绑定的标签信息同步到后台计算服务中
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                rh.SendMessage(JsonConvert.SerializeObject(sd));

                return new { Code = 0, Count = 1, Info = "新增成功" };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 调用摄像头进行抓图
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Capture([FromBody] JObject json)
        {
            try
            {
                string CameraID = json.Value<string>("CameraID");
                string UserID = json.Value<string>("UserID");
                string WorkID = json.Value<string>("WorkID");
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
                //抓拍方法
                string CaptureUrl = "/artemis/api/video/v1/manualCapture";
                var Model = new
                {
                    cameraIndexCode = CameraID
                };
                //string CaptureMsg = SocketHelper.LoadCameraList(Model, url, CaptureUrl, key, sign);
                HttpUtillibKbs.SetPlatformInfo(key, sign, url, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(CaptureUrl, Model.ToJson(), 20);
                string CameraMsg = System.Text.Encoding.UTF8.GetString(result);
                CaptureRtn cr = JsonConvert.DeserializeObject<CaptureRtn>(CameraMsg);
                //将抓拍的数据保存到数据库中
                WorkcameracaptureEntity wc = new WorkcameracaptureEntity();
                wc.ID = Guid.NewGuid().ToString();
                wc.UserId = UserID;
                wc.WorkId = WorkID;
                wc.CameraId = CameraID;
                wc.CaptureURL = cr.data.picUrl;
                wc.CreateDate = DateTime.Now;
                wc.CreateUserId = "System";
                wc.CreateUserDeptCode = "00";
                wc.CreateUserOrgCode = "00";
                wccaputuerbll.SaveForm("", wc);
                return new { Code = 0, Count = 1, Info = "抓图成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取摄像头抓图列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCaptureList([FromBody] JObject json)
        {
            try
            {
                string WorkID = json.Value<string>("WorkID");
                string UserId = json.Value<string>("UserId");
                string CameraID = json.Value<string>("CameraID");
                var calist = wccaputuerbll.GetCaptureList(WorkID, UserId, CameraID);
                return new { Code = 0, Count = calist.Count, Info = "获取成功", data = calist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }

        /// <summary>
        /// 三维获取标签数据接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ThreeDGetLable([FromBody] JObject json)
        {
            try
            {
                string LableID = json.Value<string>("LableID");
                string Search = json.Value<string>("Search");
                var calist = lablebll.GetPageList(LableID, Search);
                return new { Code = 0, Count = calist.Count, Info = "获取成功", data = calist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }

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
                var risklist = new DesktopBLL().GetKbsAreaStatus();
                var hiddenlist = arealocationbll.GetHiddenCount();
                var safeworkcontrolBll = new SafeworkcontrolBLL();
                var tasklist = safeworkcontrolBll.GetDistrictLevel();
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
                    var tasks = tasklist.Where(x => x.DistrictId.StartsWith(arealist[i].DistrictCode));
                    if (tasks != null && tasks.Count() > 0)
                        arealist[i].Level2 = tasks.Min(x => x.GradeVal.Value);
                }

                return new { Code = 1, Count = arealist.Count, Info = "获取数据成功", data = arealist };


            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 与点位计算交互接口
        /// <summary>
        /// 保存统计数据到Redis中
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public object SetAreaStatistics([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                List<Areastatistics> stlist = JsonConvert.DeserializeObject<List<Areastatistics>>(res);
                CacheFactory.Cache().WriteCache(stlist, "AreaStatistics", DateTime.Now.AddHours(24));
                return new { Code = 1, Count = 0, Info = "保存redis成功" };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 将历史定位点保存到数据库中
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SavePoint([FromBody] JObject json)
        {
            try
            {
                string url = json.Value<string>("json");
                DirectoryInfo root = new DirectoryInfo(url);
                FileInfo[] files = root.GetFiles();
                List<LocationEntity> locationlist = new List<LocationEntity>();
                foreach (var item in files)
                {
                    string fileurl = item.FullName;
                    string polist = System.IO.File.ReadAllText(fileurl);
                    LocationEntity location = new LocationEntity();
                    location.LableID = item.Name.Substring(0, item.Name.IndexOf('.'));
                    location.PointList = polist;
                    string[] name = url.Split('\\');
                    location.TimeNum = Convert.ToInt32(name[name.Length - 1]);
                    location.TimeDate = name[name.Length - 2];
                    location.ID = Guid.NewGuid().ToString();
                    location.CreateDate = DateTime.Now;
                    location.CreateUserId = "system";
                    location.CreateUserDeptCode = "00";
                    location.CreateUserOrgCode = "00";
                    locationlist.Add(location);
                }

                if (lobll.Insert(locationlist))
                {

                    //添加成功则删除原文件
                    DeleteDir(url);
                    return new { Code = 1, Count = 0, Info = "同步成功" };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "同步失败" };
                }


            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 保存标签在线状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveLableStatus([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                LableStatus ls = JsonConvert.DeserializeObject<LableStatus>(jsonstr);
                if (ls.Status == 0)//如果是上线状态增加一条上线记录
                {
                    LableonlinelogEntity lablelog = new LableonlinelogEntity();
                    lablelog.ID = Guid.NewGuid().ToString();
                    lablelog.DeptId = ls.DeptId;
                    lablelog.DeptName = ls.DeptName;
                    lablelog.DeptCode = ls.DeptCode;
                    lablelog.IsOut = 0;
                    lablelog.LableId = ls.LableId;
                    lablelog.UserId = ls.UserId;
                    lablelog.UserName = ls.Name;
                    lablelog.CreateDate = DateTime.Now;
                    lablelog.CreateUserId = "system";
                    lablelog.CreateUserDeptCode = "00";
                    lablelog.CreateUserOrgCode = "00";
                    lablelogbll.SaveStatus(lablelog);
                }
                else
                {
                    //如果是离线状态则将原来的上线记录修改成完毕，然后对应增加每小时记录
                    LableonlinelogEntity lablelog = lablelogbll.GetOnlineEntity(ls.LableId);
                    if (lablelog != null)
                    {
                        lablelog.IsOut = 1;
                        lablelog.OutTime = DateTime.Now;
                        lablelogbll.SaveStatus(lablelog);
                    }
                }

                return new { Code = 1, Count = 0, Info = "保存成功" };



            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 更新标签状态电量
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateLableState([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                LableState ls = JsonConvert.DeserializeObject<LableState>(jsonstr);
                LablemanageEntity lable = lablebll.GetEntity(ls.ID);
                lable.State = ls.State;
                lable.Power = ls.Power;
                lablebll.SaveForm(lable.ID, lable);
                return new { Code = 1, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 添加离线设备记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public object AddOffineDevice([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                OffineEntity offine = JsonConvert.DeserializeObject<OffineEntity>(jsonstr);
                OfflinedeviceEntity od = new OfflinedeviceEntity();
                od.ID = Guid.NewGuid().ToString();
                od.DeviceType = offine.Type;
                od.DeviceId = offine.ID;
                od.OfflineDevice = DateTime.Now;
                od.CreateDate = DateTime.Now;
                od.CreateUserId = "system";
                od.CreateUserDeptCode = "00";
                od.CreateUserOrgCode = "00";
                odbll.SaveForm("", od);
                return new { Code = 1, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }


        }
        /// <summary>
        /// 更新门禁离线状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateDeviceState([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                LableState ls = JsonConvert.DeserializeObject<LableState>(jsonstr);
                KbsdeviceEntity device = devicebll.GetEntity(ls.ID);
                device.State = ls.State;
                devicebll.UpdateState(device);
                return new { Code = 1, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 更改基站在线状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public object UpdateStationState([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                LableState ls = JsonConvert.DeserializeObject<LableState>(jsonstr);
                BaseStationEntity lable = stationBll.GetEntity(ls.ID);
                lable.State = ls.State;
                stationBll.UpdateState(lable);
                return new { Code = 1, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 更新摄像头离线状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateCameraState([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                LableState ls = JsonConvert.DeserializeObject<LableState>(jsonstr);
                KbscameramanageEntity camera = camerabll.GetEntity(ls.ID);
                camera.State = ls.State;
                camerabll.UpdateState(camera);
                return new { Code = 1, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 初始化获取所有摄像头数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object IniCamera()
        {
            try
            {
                var cameralist = camerabll.GetList("");
                return new { Code = 0, Count = cameralist.Count(), Info = "获取成功", data = cameralist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        [HttpPost]
        public object IniDevice()
        {
            try
            {
                var devicelist = devicebll.GetList("");
                return new { Code = 0, Count = devicelist.Count(), Info = "获取成功", data = devicelist };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        [HttpPost]
        public object GetDeviceOnline([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                string[] ids = jsonstr.Split(',');
                var model = new
                {
                    indexCodes = ids,
                    pageNo = 1,
                    pageSize = 1000
                };

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

                string DeviceUrl = "/artemis/api/nms/v1/online/acs_device/get";
                HttpUtillibKbs.SetPlatformInfo(key, sign, url, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(DeviceUrl, model.ToJson(), 20);
                string DeviceMsg = System.Text.Encoding.UTF8.GetString(result);
                return new { Code = 0, Count = 0, Info = "获取成功", data = DeviceMsg };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }




        }


        [HttpPost]
        public object GetCameraOnline([FromBody] JObject json)
        {
            try
            {
                string jsonstr = json.Value<string>("json");
                string[] ids = jsonstr.Split(',');
                var model = new
                {
                    indexCodes = ids,
                    pageNo = 1,
                    pageSize = 1000
                };

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

                string CameraUrl = "/artemis/api/nms/v1/online/camera/get";

                HttpUtillibKbs.SetPlatformInfo(key, sign, url, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(CameraUrl, model.ToJson(), 20);
                string CameraMsg = System.Text.Encoding.UTF8.GetString(result);
                return new { Code = 0, Count = 0, Info = "获取成功", data = CameraMsg };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }




        }
        /// <summary>
        /// 获取标签权限
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLableAuthorize([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                AuthorizeBLL abll = new AuthorizeBLL();
                var flag = abll.HasOperAuthority(user, "34cb0304-23a1-42c9-a821-500bd303f9c0", "Untie");
                int data = 0;
                if (flag)
                {
                    data = 0;
                }
                else
                {
                    data = 1;
                }

                return new { Code = 0, Count = 1, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取预警权限
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWarningAuthorize([FromBody] JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                AuthorizeBLL abll = new AuthorizeBLL();
                var flag = abll.HasOperAuthority(user, "71b1ffd2-091d-478c-bd66-7993e0ffec11", "search");
                int data = 0;
                if (flag)
                {
                    data = 0;
                }
                else
                {
                    data = 1;
                }

                return new { Code = 0, Count = 1, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取所有预警类型信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEarlyWarningAllList([FromBody] JObject json)
        {
            try
            {
                var basebll = new EarlywarningmanageBLL();
                var data = basebll.GetList("").ToList();
                return new { Code = 0, Count = data.Count, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        private void DeleteDir(string url)
        {
            try
            {
                System.IO.DirectoryInfo fileinfo = new DirectoryInfo(url);
                //去除只读属性
                fileinfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //成功则删除源文件
                if (Directory.Exists(url))
                {
                    foreach (var f in Directory.GetFileSystemEntries(url))
                    {
                        if (File.Exists(f))
                        {
                            File.Delete(f);
                        }
                        else
                        {
                            DeleteDir(url);
                        }
                    }
                }

                //删除空文件夹
                Directory.Delete(url);
            }
            catch (Exception)
            {

            }


        }
        #endregion

        #region 基站管理
        /// <summary>
        /// 获取所有基站信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetBaseAllList([FromBody] JObject json)
        {
            try
            {
                var basebll = new BaseStationBLL();
                var data = basebll.GetList("").ToList();
                return new { Code = 0, Count = data.Count, Info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        #endregion

        /// <summary>
        /// 三维地图上人员头像
        /// </summary>
        /// <param name="parameterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel GetUserPhoto(ParameterModel parameterModel)
        {
            var user = userbll.GetEntity(parameterModel.UserId);
            if (user == null) return new ResultModel<PersonPhotoModel> { Success = true, data = null };

            if (string.IsNullOrEmpty(user.HeadIcon)) return new ResultModel<PersonPhotoModel> { Success = true, data = new PersonPhotoModel { UserId = user.UserId } };

            var basepath = new DataItemDetailBLL().GetItemValue("imgPath");
            var filepath = Path.Combine(basepath, user.HeadIcon.TrimStart('/').Replace('/', '\\'));
            if (!File.Exists(filepath)) return new ResultModel<PersonPhotoModel> { Success = true, data = new PersonPhotoModel { UserId = user.UserId } };

            var bytes = File.ReadAllBytes(filepath);
            var base64 = Convert.ToBase64String(bytes);
            return new ListModel<PersonPhotoModel> { Success = true, data = new List<PersonPhotoModel> { new PersonPhotoModel { UserId = user.UserId, Base64Image = base64 } } };
        }




        /// <summary>
        /// 调用多个摄像头联动抓图
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public object CaptureNew([FromBody] JObject json)
        {
            try
            {
                string CameraID = json.Value<string>("CameraID");
                string UserID = json.Value<string>("UserID");
                string WorkID = json.Value<string>("WorkID");

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
                var entity = new SafeworkcontrolBLL().GetEntity(WorkID);
                if (entity != null && entity.comerid != null)
                {
                    foreach (var item in entity.comerid.Split(','))
                    {
                        if (item.IsNullOrWhiteSpace()) continue;
                        //抓拍方法
                        string CaptureUrl = "/artemis/api/video/v1/manualCapture";
                        var Model = new
                        {
                            cameraIndexCode = item
                        };
                        HttpUtillibKbs.SetPlatformInfo(key, sign, url, 80, false);
                        byte[] result = HttpUtillibKbs.HttpPost(CaptureUrl, Model.ToJson(), 20);
                        string CameraMsg = System.Text.Encoding.UTF8.GetString(result);
                        CaptureRtn cr = JsonConvert.DeserializeObject<CaptureRtn>(CameraMsg);
                        if (cr != null && cr.code == "0")
                        {
                            //将抓拍的数据保存到数据库中
                            WorkcameracaptureEntity wc = new WorkcameracaptureEntity();
                            wc.ID = Guid.NewGuid().ToString();
                            wc.UserId = UserID;
                            wc.WorkId = WorkID;
                            wc.CameraId = item;
                            wc.CaptureURL = cr.data.picUrl.Replace("6113", "80").Replace("https", "http");
                            wc.CreateDate = DateTime.Now;
                            wc.CreateUserId = "System";
                            wc.CreateUserDeptCode = "00";
                            wc.CreateUserOrgCode = "00";
                            wccaputuerbll.SaveForm("", wc);
                        }
                    }
                    return new { Code = 0, Count = 1, Info = "抓图成功" };
                }
                else
                {
                    return new { Code = -1, Count = 0, Info = "没找到对应的作业记录信息！" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 三围地图一级风险统计
        /// </summary>
        /// <param name="parameterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public object RiskCount(ParameterModel<bool> parameterModel)
        {
            var dict = new Dictionary<int, string> { { 1, "重大风险" }, { 2, "较大风险" }, { 3, "一般风险" }, { 4, "低风险" } };
            var data = riskAssessBLL.RiskCount(parameterModel.Data);
            var result = dict.GroupJoin(data, x => x.Key, y => y.GradeVal, (x, y) => new { Level = x.Value, Count = y.Count() }).ToList();
            return new { Success = true, Code = 1, data = result };
        }

        /// <summary>
        /// 三围地图点击区域统计
        /// </summary>
        /// <param name="parameterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public object RiskCount2(ParameterModel<string> parameterModel)
        {
            var level1 = new string[] { "重大风险", "较大风险", "一般风险", "低风险" };
            var data1 = riskAssessBLL.RiskCount2(parameterModel.Data);
            var part1 = level1.GroupJoin(data1, x => x, y => y.Grade, (x, y) => new { Level = x, Count = y.Count() }).ToList();
            var level2 = new string[] { "重大隐患", "一般隐患" };
            var data2 = riskAssessBLL.RiskCount3(parameterModel.Data);
            var part2 = level2.GroupJoin(data2, x => x, y => y.Grade, (x, y) => new { Level = x, Count = y.Count() }).ToList();
            var level3 = new string[] { "一级风险", "二级风险", "三级风险", "四级风险" };
            var data3 = riskAssessBLL.RiskCount4(parameterModel.Data);
            var part3 = level3.GroupJoin(data3, x => x, y => y.Grade, (x, y) => new { Level = x, Count = y.Count() }).ToList();

            return new { Success = true, Code = 1, data = new[] { part1, part2, part3 } };
        }
    }
}
