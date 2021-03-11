using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Service.StandardSystem;
using Newtonsoft.Json;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：车辆基础信息表
    /// </summary>
    public class CarinfoService : RepositoryFactory<CarinfoEntity>, CarinfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarinfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取录入车辆的
        /// </summary>
        /// <returns></returns>
        public List<CarinfoEntity> GetGspCar()
        {
            return BaseRepository().IQueryable(it =>  it.GpsName != null && it.GpsId != null && it.Type < 3).ToList();
        }

        /// <summary>
        /// 车牌号是否有重复
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetCarNoIsRepeat(string CarNo, string id)
        {

            string sql = string.Format("select count(id) from bis_carinfo where carno='{0}' ", CarNo);
            if (!id.IsEmpty())
            {
                sql += string.Format(" and id!='{0}'", id);
            }

            int count = Convert.ToInt32(BaseRepository().FindObject(sql));
            return count > 0;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarinfoEntity GetUserCar(string userid)
        {
            return this.BaseRepository().IQueryable(it => it.CreateUserId == userid && it.Type == 1).FirstOrDefault();
        }
        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarinfoEntity GetBusCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo && it.Type == 0).FirstOrDefault();
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarinfoEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo).FirstOrDefault();
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //车辆类型
            if (!queryParam["Type"].IsEmpty())
            {
                string Type = queryParam["Type"].ToString();

                pagination.conditionJson += string.Format(" and info.Type = {0}", Type);

            }

            if (!queryParam["WzStatus"].IsEmpty())
            {
                int WzStatus = Convert.ToInt32(queryParam["WzStatus"]);
                if (WzStatus == 0)
                {
                    pagination.conditionJson += string.Format(" and vi.num >0");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and vi.num is null");
                }
            }
            //通行门岗
            if (!queryParam["CurrentName"].IsEmpty())
            {
                string name = queryParam["CurrentName"].ToString();
                pagination.conditionJson += string.Format(" and info.currentgname like '%{0}%'", name);
            }


            if (!queryParam["condition"].IsEmpty())
            {
                //车牌号
                if (queryParam["condition"].ToString() == "CarNo" && !queryParam["keyword"].IsEmpty())
                {
                    string CarNo = queryParam["keyword"].ToString();

                    pagination.conditionJson += string.Format(" and info.CarNo like '%{0}%'", CarNo);

                }
                //驾驶人
                if (queryParam["condition"].ToString() == "Dirver" && !queryParam["keyword"].IsEmpty())
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and info.Dirver like '%{0}%'", Dirver);
                }
                //电话号码
                if (queryParam["condition"].ToString() == "Phone" && !queryParam["keyword"].IsEmpty())
                {

                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and info.Phone  like '{0}%'", Phone);
                }

                //单位
                if (queryParam["condition"].ToString() == "Remark" && !queryParam["keyword"].IsEmpty())
                {
                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and info.Remark  like '{0}%'", Phone);
                }

            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }



        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarinfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue, string IP, int Port)
        {
            try
            {
                var data = this.BaseRepository().FindEntity(keyValue);
                if (data != null)
                {//同步删除海康平台车辆
                    OperHaiKangRecord(data, 2, "");
                }
            this.BaseRepository().Delete(keyValue);
            //给计算模块发送出厂删除指令
            CarAlgorithmEntity Car = new CarAlgorithmEntity();
            Car.ID = keyValue;
            Car.State = 1;
            SocketHelper.SendMsg(Car.ToJson(), IP, Port);
        }
            catch (Exception)
            {
                throw;
            }
        }
     

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url,string IP,int Port)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //string key = string.Empty;// "21049470";
                //string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                //if (!string.IsNullOrEmpty(pitem))
                //{
                //    key = pitem.Split('|')[0];
                //    sign = pitem.Split('|')[1];
                //}

                if (!string.IsNullOrEmpty(keyValue))
                {

                    //将该车1小时内的打卡记录都变为已进场
                    Repository<CarinfoEntity> inlogdb = new Repository<CarinfoEntity>(DbFactory.Base());
                    CarinfoEntity old = inlogdb.FindEntity(keyValue);
                    //判断是否修改GPS信息，如果修改则相应的修改GPS信息数据
                    if (entity.GpsId != null && old.GpsId != entity.GpsId && entity.GpsId.Trim() != "")
                    {
                        string sql = string.Format(@"update bis_cargps set gpsid='{0}',gpsname='{1}' where aid='{2}'", entity.GpsId, entity.GpsName, entity.ID);
                        res.ExecuteBySql(sql);
                    }
                    else if (entity.GpsId == null && old.GpsId != entity.GpsId)
                    {
                        string sql = string.Format(@"update bis_cargps set gpsid='{0}',gpsname='{1}' where aid='{2}'", "", "", entity.ID);
                        res.ExecuteBySql(sql);
                    }

                    entity.Modify(keyValue);
                    if (string.IsNullOrEmpty(entity.Dirver))
                    {
                        entity.Dirver = "";
                    }
                    if (string.IsNullOrEmpty(entity.Remark))
                    {
                        entity.Remark = "";
                    }
                    if (string.IsNullOrEmpty(entity.Phone))
                    {
                        entity.Phone = "";
                    }

                    res.Update<CarinfoEntity>(entity);

                    #region 根据车辆车牌号码同步修改海康那边的车辆数据
                    var data = this.BaseRepository().FindEntity(keyValue);
                    if (data != null)
                    {
                        OperHaiKangRecord(entity, 1, data.CarNo);
                    }
                    #endregion
                    res.Commit();
                    if (!string.IsNullOrEmpty(entity.GpsId) && !string.IsNullOrEmpty(entity.GpsName))
                    {
                        CarAlgorithmEntity Car = new CarAlgorithmEntity();
                        Car.CarNo = entity.CarNo;
                        Car.GPSID = entity.GpsId;
                        Car.GPSName = entity.GpsName;
                        Car.ID = entity.ID;
                        Car.Type = Convert.ToInt32(entity.Type);
                        Car.State = 0;
                        Car.LineName = "";
                        Car.GoodsName = "";
                        SocketHelper.SendMsg(Car.ToJson(), IP, Port);
                    }
                }
                else
                {
                    entity.Create();
                    #region 先添加海康车辆
                    //var model = new
                    //{
                    //    clientId = 1,
                    //    plateNo = entity.CarNo

                    //};
                    //List<object> modellist = new List<object>();
                    //modellist.Add(model);
                    //string addMsg = SocketHelper.LoadCameraList(model, url, "/artemis/api/resource/v1/vehicle/batch/add", key, sign);
                    //AddCarMsg addcar = JsonConvert.DeserializeObject<AddCarMsg>(addMsg);
                    //if (addcar != null && addcar.code == "0" && addcar.data.successes.Count > 0)
                    //{
                    //    entity.ID = addcar.data.successes[0].vehicleId;
                    //}
                    OperHaiKangRecord(entity, 0, "");
                    #endregion
                    res.Insert<CarinfoEntity>(entity);
                    CargpsEntity gps = new CargpsEntity();
                    gps.AID = entity.ID;
                    gps.CarNo = entity.CarNo;
                    gps.GpsId = entity.GpsId;
                    gps.GpsName = entity.GpsName;
                    gps.Status = 0;
                    gps.Type = 0;
                    gps.Create();
                    res.Insert<CargpsEntity>(gps);
                    res.Commit();

                    if (!string.IsNullOrEmpty(entity.GpsId) && !string.IsNullOrEmpty(entity.GpsName))
                    {
                        CarAlgorithmEntity Car = new CarAlgorithmEntity();
                        Car.CarNo = entity.CarNo;
                        Car.GPSID = entity.GpsId;
                        Car.GPSName = entity.GpsName;
                        Car.ID = entity.ID;
                        Car.Type = Convert.ToInt32(entity.Type);
                        Car.State = 0;
                        Car.LineName = "";
                        Car.GoodsName = "";
                        SocketHelper.SendMsg(Car.ToJson(), IP, Port);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// /保存表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, CarinfoEntity entity)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<CarinfoEntity>(entity);
                    res.Commit();
                }
                else
                {
                    entity.Create();
                    res.Insert<CarinfoEntity>(entity);
                    res.Commit();
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 私家车辆审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void CartoExamine(string keyValue, CarinfoEntity entity)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<CarinfoEntity>(entity);
                    res.Commit();
                    if (entity.State == "1")
                    {
                        OperHaiKangRecord(entity, 0, "");
                    }
                }
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 手机app修改海康车辆信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="NewCar"></param>
        public void UpdateHiaKangCar(CarinfoEntity entity, string OldCar)
        {
            try
            {
                OperHaiKangRecord(entity, 2, OldCar);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region 海康平台

      /// <summary>
        /// 海康平台数据同步信息
      /// </summary>
      /// <param name="entity">实体</param>
      /// <param name="type">0新增 1修改 2删除</param>
      /// <param name="NewCar">旧车牌</param>
        public void OperHaiKangRecord(CarinfoEntity entity, int type, string OldCar)
        {
            try
            {
                #region 获取海康地址和秘钥
                DataItemDetailService data = new DataItemDetailService();
                var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }
                #endregion
                #region 添加车辆
                if (type == 0)
                {//添加
                    string addurl = "/artemis/api/v1/vehicle/addVehicle";
                    List<object> mllist = new List<object>();
                    var user = new UserService().GetEntity(entity.DirverId);
                    if (user != null)
                    {
                        var model = new
                        {//有车主
                            personId = entity.DirverId,
                            plateNo = entity.CarNo,
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//停车场唯一标识
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            vehicleGroup = GetGroupInfo(entity.Type)//群组唯一标识
                        };
                        mllist.Add(model);
                    }
                    else
                    {
                        var model = new
                        {//无车主
                            //personId = entity.DirverId,
                            plateNo = entity.CarNo,
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//停车场唯一标识
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            vehicleGroup = GetGroupInfo(entity.Type)//群组唯一标识
                        };
                        mllist.Add(model);
                    }
                    SocketHelper.LoadCameraList(mllist, baseurl, addurl, Key, Signature);
                }
                #endregion
                #region 修改车辆
                else if (type == 1)
                {//修改
                    string updateurl = "/artemis/api/v1/vehicle/updateVehicle";
                    List<object> mllist = new List<object>();
                    var user = new UserService().GetEntity(entity.DirverId);
                    if (user != null)
                    {
                        var model = new
                        {//有车主
                            plateNo = entity.CarNo,//新车牌
                            oldPlateNo = OldCar,//旧车牌
                            ownerId = entity.DirverId,
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//停车场唯一标识
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            isUpdateFunction = 1,
                            vehicleGroup = GetGroupInfo(entity.Type)//群组唯一标识
                        };
                        mllist.Add(model);
                    }
                    else
                    {
                        var model = new
                        {//无车主
                            plateNo = entity.CarNo,//新车牌
                            oldPlateNo = OldCar,//旧车牌
                            parkIndexCode = GetTrafficPost(entity.Currentgid, entity.Type),//停车场唯一标识
                            plateType = 0,
                            plateColor = 0,
                            carType = 0,
                            carColor = 0,
                            startTime = DateTime.Parse(entity.Starttime.ToString()).ToString("yyyy-MM-dd"),
                            endTime = DateTime.Parse(entity.Endtime.ToString()).ToString("yyyy-MM-dd"),
                            isUpdateFunction = 1,
                            vehicleGroup = GetGroupInfo(entity.Type)//群组唯一标识
                        };
                        mllist.Add(model);
                    }
                    SocketHelper.LoadCameraList(mllist, baseurl, updateurl, Key, Signature);
                }
                #endregion
                #region 删除车辆
                else if (type == 2)
                {//删除
                    string selurl = "/artemis/api/v1/vehicle/fetchVehicle";
                    var model = new
                    {
                        plateNo = entity.CarNo,
                        pageNo = 1,
                        pageSize = 10
                    };
                    //查询车辆记录唯一标识
                    string msg = SocketHelper.LoadCameraList(model, baseurl, selurl, Key, Signature);
                    CarSelectEntity pl = JsonConvert.DeserializeObject<CarSelectEntity>(msg);
                    if (pl != null && pl.code == "0")
                    {
                        string carid = pl.data.rows[0].vehicleId;
                        //删除车辆
                        string delurl = "/artemis/api/resource/v1/vehicle/batch/delete";
                        List<string> list = new List<string>();
                        list.Add(carid);
                        var delmodel = new
                        {
                            vehicleIds = list
                        };
                        SocketHelper.LoadCameraList(delmodel, baseurl, delurl, Key, Signature);
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取通行岗唯一标示（即车库）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTrafficPost(string currentid, int type)
        {
            string res = string.Empty;
            try
            {
                switch (type)
                {
                    case 1://私家车
                        res = currentid;
                        break;
                    case 6://临时通行车辆
                        res = currentid;
                        break;
                    default:
                        DataItemDetailService itembll = new DataItemDetailService();
                        IEnumerable<DataItemModel> list = itembll.GetDataItemListByItemCode("KmCarType");
                        var entity = list.Where(a => a.ItemValue == type.ToString()).FirstOrDefault();
                        if (entity != null && entity.Description != null)
                        {//编码管理中设置
                            res = entity.Description.Replace("<p>", "").Replace("</p>", "").Trim(); ;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        /// <summary>
        /// 获取车辆群组信息
        /// </summary>
        /// <returns></returns>
        public string GetGroupInfo(int type)
        {
            string res = string.Empty;
            try
            {
                DataItemDetailService itembll = new DataItemDetailService();
                IEnumerable<DataItemModel> list = itembll.GetDataItemListByItemCode("KmCarType");
                var entity = list.Where(a => a.ItemValue == type.ToString()).FirstOrDefault();
                if (entity != null)
                {//编码管理中设置
                    res = entity.ItemCode;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }


        #endregion

    }
}
