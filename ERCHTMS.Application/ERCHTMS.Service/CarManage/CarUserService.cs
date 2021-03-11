using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.CarManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
using Newtonsoft.Json;
using System.Security.Cryptography;
using ERCHTMS.Service.SystemManage;
using System.Globalization;
using ERCHTMS.Service.BaseManage;
//using ServiceStack.Messaging.Rcon;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 拜访车辆
    /// </summary>
    public class CarUserService : RepositoryFactory<CarUserEntity>, CarUserIService
    {

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarUserEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.Phone == queryJson).OrderByDescending(it => it.CreateDate).ToList();
        }

        /// <summary>
        /// 获取门岗查询的物料及拜访信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDoorList()
        {
            string sql = string.Format(@"select * from (
            select ID,Createdate,carno,CONCAT(CONCAT(CONCAT('拜访-',visitdept),'-'),visituser) as purpose,dirver
            ,phone,note,accompanyingnumber anumber,driverlicenseurl,drivinglicenseurl,state,'0' type from bis_visitcar where state>0 and state<4

            union 

            select ID,Createdate,platenumber carno,CONCAT(CONCAT(CONCAT(CONCAT(dress,'-'),transporttype),'-'),PRODUCTTYPE) as purpose,
            DriverName dirver,DriverTel phone,Supplyname note,0 anumber,JsImgpath driverlicenseurl,XsImgpath drivinglicenseurl,examinestatus state,'1' type 
            from WL_OPERTICKETMANAGER where isdelete=1 and examinestatus<4 and examinestatus>0

            union 
            
            select ID,Createdate,carno,CONCAT(CONCAT(CONCAT('危化品-',thecompany),'-'),hazardousname) as purpose,dirver
            ,phone,note,accompanyingnumber anumber,driverlicenseurl,drivinglicenseurl,state,'2' type from bis_hazardouscar where state>0 and state<4
            ) a1 order by Createdate desc");
            return BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 查询是否有重复车牌号拜访车辆/危化品车辆
        /// </summary>
        /// <param name="CarNo">车牌号</param>
        /// <param name="type">3位拜访 5为危化品</param>
        /// <returns></returns>
        public bool GetVisitCf(string CarNo, int type)
        {
            string dbname = "bis_visitcar";
            if (type == 3)
            {
                dbname = "bis_visitcar";
            }
            else if (type == 5)
            {
                dbname = "BIS_HAZARDOUSCAR";
            }

            string sql = string.Format("select count(id) as cou from {1} where carno ='{0}' and state<4", CarNo, dbname);

            int num1 = Convert.ToInt32(BaseRepository().FindObject(sql));

            if (num1 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 获得当日外来车辆数量
        /// </summary>
        /// <returns></returns>
        public int GetOutCarNum()
        {
            string sql =
                string.Format(
                    @"select count(id)  as carnum from bis_visitcar where state>2 and state!=99 and TO_CHAR(CREATEDATE,'yyyy-MM-dd') >= '{0}'",
                    DateTime.Now.ToString("yyyy-MM-dd"));
            string wlsql = string.Format(@"select count(id) as carnum from wl_operticketmanager where   examinestatus>2 and examinestatus!=99 and TO_CHAR(CREATEDATE,'yyyy-MM-dd') >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            int num1 = Convert.ToInt32(BaseRepository().FindObject(sql));
            int num2 = Convert.ToInt32(BaseRepository().FindObject(wlsql));
            return num1 + num2;
        }

        /// <summary>
        /// 手机端获取待审批拜访记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetStayApprovalRecordCount(string userid)
        {
            try
            {
                var user = new UserService().GetEntity(userid);
                string sql = string.Empty;
                if (user != null)
                {
                    sql = string.Format(@" select count(1)
            from (select ID,'拜访(无车)' as purpose,d.visituserphone as tel, dirver, phone,d.carno as comname,d.venuestate as carno, d.accompanyingperson,d.intime, createdate,1 type
            from BIS_USERCAR d
            where state > 0
            and state < 4 and appstatue=0
            union
            select ID,'拜访(有车)' as purpose,t.visituserphone as tel, dirver, phone,t.comname,t.carno,t.accompanyingperson,t.intime, createdate,1 type
            from bis_visitcar t
            where state > 0
             and state < 4 and appstatue=0
            ) a1 where tel='{0}'
            order by Createdate desc ", user.Mobile);
                }
                int num2 = Convert.ToInt32(BaseRepository().FindObject(sql));
                return num2;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarUserEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public CarUserEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo).OrderByDescending(it => it.CreateDate).FirstOrDefault();
        }


        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            pagination.conditionJson += " and d.state > 2";

            //状态
            if (!queryParam["type"].IsEmpty())
            {
                string Status = queryParam["type"].ToString();
                pagination.conditionJson += string.Format(" and d.state = {0}", Status);
            }

            //进厂时间起
            if (!queryParam["sttime"].IsEmpty())
            {
                string sttime = queryParam["sttime"].ToString() + " 00:00:00";
                pagination.conditionJson += string.Format(" and TO_CHAR(d.CREATEDATE,'yyyy-MM-dd HH:mm:ss') >= '{0}'  ", sttime);
            }
            //进厂时间至
            if (!queryParam["endtime"].IsEmpty())
            {
                string endtime = queryParam["endtime"].ToString() + " 23:59:59";
                pagination.conditionJson += string.Format(" and TO_CHAR(d.CREATEDATE,'yyyy-MM-dd HH:mm:ss') <= '{0}' ", endtime);
            }
            //关键字查询
            if (!queryParam["condition"].IsEmpty())
            {
                //申请人
                if (queryParam["condition"].ToString() == "Dirver")
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and d.Dirver like '%{0}%'", Dirver);
                }
                //手机号
                if (queryParam["condition"].ToString() == "Phone")
                {
                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and d.Phone like '%{0}%'", Phone);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, CarUserEntity entity, List<CarUserFileImgEntity> userjson)
        {
            try
            {
                Repository<CarUserFileImgEntity> caruserdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                if (!string.IsNullOrEmpty(keyValue))
                {
                    List<CarUserFileImgEntity> list = caruserdb.IQueryable(it => it.Baseid == keyValue).ToList();
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                    //if (entity.State == 3)
                    //{//审批通过准许入厂(下发入厂权限)
                    //    DataItemDetailService data = new DataItemDetailService();
                    //    var pitem = data.GetItemValue("Hikappkey");
                    //    var baseurl = data.GetItemValue("HikBaseUrl");
                    //    string Key = string.Empty;
                    //    string Signature = string.Empty;
                    //    if (!string.IsNullOrEmpty(pitem))
                    //    {
                    //        Key = pitem.Split('|')[0];
                    //        Signature = pitem.Split('|')[1];
                    //    }
                    //    UploadUserlimits(list, baseurl, Key, Signature);
                    //}
                    if (entity.State == 99)
                    {//拒绝入场（删除海康平台对应用户）
                        DeleteUserHiK(list);
                    }
                }
                else
                {//随行人员
                    SaveUserFileImgForm(entity, userjson);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 添加用户出入权限
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<AddJurisdictionEntity> addUserJurisdiction(string keyValue, int State,string baseurl,string Key,string Signature)
        {
            List<AddJurisdictionEntity> taskIdlist = new List<AddJurisdictionEntity>();
            try
            {
                Repository<CarUserFileImgEntity> caruserdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                if (!string.IsNullOrEmpty(keyValue))
                {
                    List<CarUserFileImgEntity> list = caruserdb.IQueryable(it => it.Baseid == keyValue).ToList();
                    if (State == 3)
                    {//审批通过准许入厂(下发入厂权限)
                        var addentity = UploadUserlimits(list, baseurl, Key, Signature);
                        taskIdlist.Add(addentity);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return taskIdlist;
        }


      /// <summary>
      /// 保存随行人员信息
      /// </summary>
      /// <param name="entity">主记录</param>
      /// <param name="list">子记录</param>
        public void SaveUserFileImgForm(CarUserEntity entity, List<CarUserFileImgEntity> list)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                // List<CarUserFileImgEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CarUserFileImgEntity>>(userjson);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ID == "-1")
                    {//跟随人员包含申请人
                        list[i].Create();
                        if (string.IsNullOrEmpty(list[i].Username)) continue;
                        list[i].Baseid = entity.ID;
                        list[i].ID = Guid.NewGuid().ToString();
                        list[i].CreateDate = DateTime.Now;
                        entity.AccompanyingPerson = entity.AccompanyingPerson + list[i].Username + ",";
                    }
                }
                entity.AccompanyingPerson = entity.AccompanyingPerson.TrimEnd(',');
                res.Insert<CarUserFileImgEntity>(list);
                res.Insert(entity);
                res.Commit();
                UploadUserHiK(entity, list);//人员同步到海康平台
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        #region 数据同步及权限下发到海康平台

        /// <summary>
        /// 将拜访人员信息上传到海康平台
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        public void UploadUserHiK(CarUserEntity entity, List<CarUserFileImgEntity> list)
        {
            DataItemDetailService data = new DataItemDetailService();
            var pitem = data.GetItemValue("Hikappkey");
            var baseurl = data.GetItemValue("HikBaseUrl");
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/single/add";
            foreach (var item in list)
            {
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                var no = time + Str.PinYin(item.Username).ToUpper();//证件号码
                //人脸信息（base64必须为jpg格式）
                List<FaceEntity> faces = new List<FaceEntity>();
                FaceEntity face = new FaceEntity();
                face.faceData = item.Imgdata;
                faces.Add(face);
                string msg = null;
                if (string.IsNullOrEmpty(item.Imgdata))
                {//无人脸
                    var model = new
                    {
                        personId = item.ID,
                        personName = item.Username,
                        gender = "1",
                        orgIndexCode = "root000000",
                        birthday = "1990-01-01",
                        phoneNo = entity.Phone,
                        email = "person1@person.com",
                        certificateType = "990",
                        certificateNo = no
                    };
                    msg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                }
                else
                {//有人脸
                    var model = new
                    {
                        personId = item.ID,
                        personName = item.Username,
                        gender = "1",
                        orgIndexCode = "root000000",
                        birthday = "1990-01-01",
                        phoneNo = entity.Phone,
                        email = "person1@person.com",
                        certificateType = "990",
                        certificateNo = no,
                        faces
                    };
                    msg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                }
                parkList1 pl = JsonConvert.DeserializeObject<parkList1>(msg);
                if (pl != null && pl.code == "0")
                {
                    SetLoadUserCarNo(item, baseurl, Key, Signature);
                }
            }
        }

        /// <summary>
        /// 给新添加的人员分配卡号
        /// </summary>
        private void SetLoadUserCarNo(CarUserFileImgEntity Item, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/cis/v1/card/bindings";
            List<cardList1> cardList = new List<cardList1>();
            cardList1 entity = new cardList1();
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            var no = Str.PinYin(Item.Username).ToUpper() + time;//卡号唯一
            entity.cardNo = no.Trim();
            entity.personId = Item.ID;
            entity.cardType = 1;
            cardList.Add(entity);
            var model = new
            {
                startDate = DateTime.Now.ToString("yyyy-MM-dd"),
                endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                cardList
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
            parkList2 pl = JsonConvert.DeserializeObject<parkList2>(msg);
        }

        /// <summary>
        /// 出入权限配置
        /// </summary>
        private AddJurisdictionEntity UploadUserlimits(List<CarUserFileImgEntity> list, string baseUrl, string Key, string Signature)
        {
            AddJurisdictionEntity Juentity = new AddJurisdictionEntity();
            try
            {
                if (list.Count > 0)
                {
                    var url = "/artemis/api/acps/v1/auth_config/add";
                    List<personDatas1> personDatas = new List<personDatas1>();
                    personDatas1 entity = new personDatas1();
                    List<string> codes = new List<string>();
                    foreach (var item in list)
                    {
                        codes.Add(item.ID);//人员Id
                    }
                    entity.indexCodes = codes;
                    entity.personDataType = "person";
                    personDatas.Add(entity);

                    string Qres = string.Empty;//临时人员默认一号岗
                    string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='equipment1' order by t.sortcode asc");
                    DataTable dt = this.BaseRepository().FindTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//设备信息集合
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            List<int> nos = new List<int>();
                            if (dt.Rows[i][2].ToString() == "1")
                            {//门禁通道
                                nos.Add(1);
                                nos.Add(2);
                            }
                            else
                            {//门禁
                                nos.Add(1);
                            }
                            resourceInfos1 entity1 = new resourceInfos1();
                            entity1.resourceIndexCode = dt.Rows[i][1].ToString();//设备唯一编号
                            entity1.resourceType = "acsDevice";
                            entity1.channelNos = nos;
                            resourceInfos.Add(entity1);
                        }
                        string stime = Convert.ToDateTime(list[0].CreateDate).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);//ISO8601时间格式
                        string etime = Convert.ToDateTime(list[0].CreateDate).AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss+08:00", DateTimeFormatInfo.InvariantInfo);
                        var model = new
                        {
                            personDatas,
                            resourceInfos,
                            startTime = stime,
                            endTime = etime
                        };
                        string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);

                        JurisdictionEntity p1 = JsonConvert.DeserializeObject<JurisdictionEntity>(msg);
                        //Juentity.taskId = "54654646465465464654";
                        //Juentity.type = "add";
                        //Juentity.resourceInfos = resourceInfos;
                        if (p1 != null && p1.code == "0")
                        {
                            Juentity.taskId = p1.data.taskId;
                            Juentity.resourceInfos = resourceInfos;
                            Juentity.type = "add";
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Juentity;
        }

        /// <summary>
        /// 根据出入权限配置快捷下载(IC卡号、人脸)
        /// </summary>
        private void downloadUserlimits(List<resourceInfos1> resourceInfos, string baseUrl, string Key, string Signature)
        {
            try
            {
                var url = "/artemis/api/acps/v1/authDownload/configuration/shortcut";
                var model = new
                {
                    taskType = 5,
                    resourceInfos
                };
                string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
            }
            catch (Exception)
            {
                throw;
            }
        }


       /// <summary>
        /// 离场或拒绝入场删除海康平台人员信息
       /// </summary>
       /// <param name="list">离厂人员</param>
       /// <param name="type">0拒绝入场 1人员离厂</param>
        public void DeleteUserHiK(List<CarUserFileImgEntity> list, int type = 0)
        {
            try
            {
                if (list.Count > 0)
                {
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
                    string Url = "/artemis/api/resource/v1/person/batch/delete";//接口地址
                    List<string> dellist = new List<string>();
                    foreach (var item in list)
                    {
                        dellist.Add(item.ID);
                    }
                    if (type == 1) { DeleteUserlimits(list, baseurl, Key, Signature); };
                    var model = new
                    {
                        personIds = dellist
                    };
                    SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 拜访人员删除对应设备中出入权限配置
        /// </summary>
        public void DeleteUserlimits(List<CarUserFileImgEntity> list, string baseUrl, string Key, string Signature)
        {
            string msg = string.Empty;
            if (list.Count > 0)
            {
                var url = "/artemis/api/acps/v1/auth_config/delete";
                List<personDatas1> personDatas = new List<personDatas1>();
                personDatas1 entity = new personDatas1();
                List<string> codes = new List<string>();
                foreach (var Item in list)
                {
                    codes.Add(Item.ID);//人员Id
                }
                entity.indexCodes = codes;
                entity.personDataType = "person";
                personDatas.Add(entity);
                string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='equipment1' order by t.sortcode asc");
                //出入权限
                DataTable dt = this.BaseRepository().FindTable(sql);
                if (dt.Rows.Count > 0)
                {
                    List<resourceInfos1> resourceInfos = new List<resourceInfos1>();//设备信息集合
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<int> nos = new List<int>();
                        if (dt.Rows[i][2].ToString() == "1")
                        {//门禁通道
                            nos.Add(1);
                            nos.Add(2);
                        }
                        else
                        {//门禁
                            nos.Add(1);
                        }
                        resourceInfos1 entity1 = new resourceInfos1();
                        entity1.resourceIndexCode = dt.Rows[i][1].ToString();//设备唯一编号
                        entity1.resourceType = "acsDevice";
                        entity1.channelNos = nos;
                        resourceInfos.Add(entity1);
                    }
                    var model = new
                    {
                        personDatas,
                        resourceInfos
                    };
                    //配置删除权限
                    msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                    JurisdictionEntity ps = JsonConvert.DeserializeObject<JurisdictionEntity>(msg);
                    if (ps.code == "0")
                    {
                        //下发删除配置到硬件设备上
                        downloadUserlimits(resourceInfos, baseUrl, Key, Signature);
                    }
                }
            }
        }
      

        #endregion


        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, CarUserEntity entity, List<PersongpsEntity> pgpslist)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //将该车1小时内的打卡记录都变为已进场
                Repository<CarUserEntity> inlogdb = new Repository<CarUserEntity>(DbFactory.Base());
                CarUserEntity old = inlogdb.FindEntity(keyValue);
                bool isupdate = false;//是否修改
                if (old.GPSID != entity.GPSID)
                {
                    isupdate = true;
                }
                old.GPSID = entity.GPSID;
                old.GPSNAME = entity.GPSNAME;
                old.Modify(keyValue);
                if (old.State == 1)
                {
                    old.State = 2;
                    if (pgpslist != null)
                    {
                        for (int i = 0; i < pgpslist.Count; i++)
                        {
                            pgpslist[i].InTime = DateTime.Now;
                            pgpslist[i].State = 0;
                            pgpslist[i].Type = 1;
                            pgpslist[i].VID = old.ID;
                            pgpslist[i].UserName = pgpslist[i].UserName.Substring(0, pgpslist[i].UserName.IndexOf(':'));
                            pgpslist[i].Create();

                        }
                        res.Insert<PersongpsEntity>(pgpslist);
                    }
                }
                else
                {
                    string sql = "";
                    if (pgpslist != null)
                    {
                        for (int i = 0; i < pgpslist.Count; i++)
                        {
                            sql += "update bis_persongps set gpsid='" + pgpslist[i].GPSID + "' , gpsname='" +
                                   pgpslist[i].GPSNAME + "' where id='" + pgpslist[i].ID + "'";
                            res.ExecuteBySql(sql);
                        }
                    }
                }
                res.Update<CarUserEntity>(old);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 获取人员与Gps关联表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<PersongpsEntity> GetPersongpslist(string keyValue)
        {
            Repository<PersongpsEntity> pgpsinlogdb = new Repository<PersongpsEntity>(DbFactory.Base());
            List<PersongpsEntity> list = pgpsinlogdb.IQueryable(it => it.VID == keyValue && it.Type == 1).ToList();
            return list;
        }

        /// <summary>
        /// 车辆出厂
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Note"></param>
        /// <param name="type"></param>
        public void CarOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<CarUserFileImgEntity> dellist = new List<CarUserFileImgEntity>();//离厂人员
                if (type == 3)
                {//拜访人员
                    Repository<CarUserEntity> inlogdb = new Repository<CarUserEntity>(DbFactory.Base());
                    CarUserEntity old = inlogdb.FindEntity(keyValue);
                    old.Note = Note;
                    old.Modify(old.ID);
                    Repository<PersongpsEntity> pgpsinlogdb = new Repository<PersongpsEntity>(DbFactory.Base());
                    Repository<CarUserFileImgEntity> caruserdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                    List<PersongpsEntity> pgps = new List<PersongpsEntity>();
                    foreach (PersongpsEntity item in pergps)
                    {
                        PersongpsEntity entity = pgpsinlogdb.FindEntity(item.ID);
                        if (entity != null)
                        {
                            entity.Issubmit = 1;
                            entity.State = item.State;
                            entity.Modify(item.ID);
                            if (item.State == 1)
                            {
                                entity.OutTime = DateTime.Now;
                                var carentity = caruserdb.IQueryable(it => it.Baseid == keyValue && it.Username == entity.UserName).ToList();
                                dellist.AddRange(carentity);
                            }
                            pgps.Add(entity);
                        }
                    }
                    if (pergps.Where(t => t.State == 1).ToList().Count == pergps.Count)
                    {//子表所有人员都出厂修改主表出厂状态
                        old.OutTime = DateTime.Now;
                        old.State = 4;
                    }
                    res.Update<CarUserEntity>(old);
                    res.Update<PersongpsEntity>(pgps);
                }
            
                Repository<CargpsEntity> cgpsinlogdb = new Repository<CargpsEntity>(DbFactory.Base());
                List<CargpsEntity> cgps = cgpsinlogdb.IQueryable(it => it.AID == keyValue && it.Status == 0).ToList();
                if (cgps.Count > 0)
                {
                    for (int i = 0; i < cgps.Count; i++)
                    {
                        cgps[i].Status = 1;
                        cgps[i].EndTime = DateTime.Now;
                        cgps[i].Modify(cgps[i].ID);
                    }
                    res.Update<CargpsEntity>(cgps);
                }

                //车辆违章记录处理
                Repository<CarviolationEntity> violinlogdb = new Repository<CarviolationEntity>(DbFactory.Base());
                List<CarviolationEntity> violation = violinlogdb.IQueryable(it => it.CID == keyValue && it.IsProcess == 0).ToList();
                if (violation.Count > 0)
                {
                    for (int i = 0; i < violation.Count; i++)
                    {
                        violation[i].IsProcess = 1;
                        violation[i].ProcessMeasure = Note;
                        violation[i].Modify(violation[i].ID);
                    }
                    res.Update<CarviolationEntity>(violation);
                }
                res.Commit();
                DeleteUserHiK(dellist, 1);
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void WlChangeGps(string keyValue, OperticketmanagerEntity entity)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //将该车1小时内的打卡记录都变为已进场
                Repository<OperticketmanagerEntity> inlogdb = new Repository<OperticketmanagerEntity>(DbFactory.Base());
                OperticketmanagerEntity old = inlogdb.FindEntity(keyValue);
                bool isupdate = false;//是否修改
                if (old.GpsId != entity.GpsId)
                {
                    isupdate = true;
                }
                old.GpsId = entity.GpsId;
                old.GpsName = entity.GpsName;
                old.Modify(keyValue);
                if (old.ExamineStatus == 1)
                {
                    old.ExamineStatus = 2;


                    CargpsEntity cgps = new CargpsEntity();
                    cgps.AID = old.ID;
                    cgps.CarNo = old.Platenumber;
                    cgps.GpsId = old.GpsId;
                    cgps.GpsName = old.GpsName;
                    cgps.Status = 0;
                    cgps.Type = 1;
                    cgps.Create();
                    res.Insert<CargpsEntity>(cgps);
                }
                else
                {


                    if (isupdate)
                    {
                        Repository<CargpsEntity> Carinlogdb = new Repository<CargpsEntity>(DbFactory.Base());
                        CargpsEntity Car = Carinlogdb.IQueryable(it => it.AID == old.ID && it.Status == 0).FirstOrDefault();
                        Car.GpsId = old.GpsId;
                        Car.GpsName = old.GpsName;
                        Car.Modify(Car.ID);
                        res.Update<CargpsEntity>(Car);
                    }



                }
                res.Update<OperticketmanagerEntity>(old);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 调用摄像头门卫录入拜访人脸照片
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public void SaveFileImgForm(CarUserFileImgEntity entity)
        {
            Repository<CarUserFileImgEntity> inlogdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
            CarUserFileImgEntity old = inlogdb.FindEntity(entity.ID);
            if (old != null)
            {
                if (string.IsNullOrEmpty(old.Userimg))
                {//人脸信息未录入
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
                    FacedataEntity face = new FacedataEntity();
                    List<FacedataEntity> FaceList = new List<FacedataEntity>();
                    face.UserId = old.ID;
                    face.ImgData = entity.Imgdata;
                    FaceList.Add(face);
                    SocketHelper.UploadFace(FaceList, baseurl, Key, Signature);
                }
                old.Userimg = entity.Userimg;
                old.Imgdata = entity.Imgdata;
                inlogdb.Update(old);
            }
        }

        #endregion
    }
}
