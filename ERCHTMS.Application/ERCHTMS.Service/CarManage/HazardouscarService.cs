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
using ERCHTMS.Service.SystemManage;
using Newtonsoft.Json;
using System.Globalization;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：危害因素车辆表
    /// </summary>
    public class HazardouscarService : RepositoryFactory<HazardouscarEntity>, HazardouscarIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazardouscarEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.Phone == queryJson).ToList();
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

            pagination.conditionJson += " and state > 0";

            //来访类型
            if (!queryParam["StartTime"].IsEmpty())
            {
                string sttime = queryParam["StartTime"].ToString() + " 00:00:00";

                pagination.conditionJson += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH:mm:ss') >= '{0}'  ", sttime);

            }

            if (!queryParam["EndTime"].IsEmpty())
            {
                string endtime = queryParam["EndTime"].ToString() + " 23:59:59";
                pagination.conditionJson += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH:mm:ss') <= '{0}' ", endtime);
            }

            //状态
            if (!queryParam["Status"].IsEmpty())
            {
                string Status = queryParam["Status"].ToString();

                if (Status == "1")
                {
                    pagination.conditionJson += string.Format(" and (state = 1 or state=2)");
                }
                else
                {

                    pagination.conditionJson += string.Format(" and state = {0}", Status);
                }

            }

            //状态
            if (!queryParam["Hazardous"].IsEmpty())
            {
                string Hazardous = queryParam["Hazardous"].ToString();

                pagination.conditionJson += string.Format(" and Hazardousid = '{0}'", Hazardous);

            }


            //状态
            if (!queryParam["Vnum"].IsEmpty())
            {
                string Vnum = queryParam["Vnum"].ToString();
                if (Vnum == "0")
                {
                    pagination.conditionJson += string.Format(" and (vnum = 0 or vnum is null)");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and vnum > 0");
                }



            }





            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazardouscarEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取此危害因素是否配置了检查表
        /// </summary>
        /// <param name="HazardousId"></param>
        /// <returns></returns>
        public bool GetHazardous(string HazardousId)
        {
            Repository<CarcheckitemhazardousEntity> inlogdb = new Repository<CarcheckitemhazardousEntity>(DbFactory.Base());
            CarcheckitemhazardousEntity haza = inlogdb.FindEntity(it => it.HazardousId == HazardousId);
            if (haza != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public HazardouscarEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.CarNo == CarNo).OrderByDescending(it => it.CreateDate).FirstOrDefault();
        }


        /// <summary>
        /// 获取当日危化品车辆
        /// </summary>
        /// <returns></returns>
        public List<HazardouscarEntity> GetHazardousList(string day)
        {
            string sql =
                string.Format(
                    @"select * from BIS_HAZARDOUSCAR where TO_CHAR(CREATEDATE,'yyyy-MM-dd')='{0}'",
                    day);
            return BaseRepository().FindList(sql).ToList();
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
        public void SaveForm(string keyValue, HazardouscarEntity entity)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string HazardousId = entity.HazardousId;
                Repository<CarcheckitemhazardousEntity> inlogdb = new Repository<CarcheckitemhazardousEntity>(DbFactory.Base());
                CarcheckitemhazardousEntity haza = inlogdb.FindEntity(it => it.HazardousId == HazardousId);

                Repository<CarcheckitemmodelEntity> modeldb = new Repository<CarcheckitemmodelEntity>(DbFactory.Base());
                IEnumerable<CarcheckitemmodelEntity> modelList = modeldb.IQueryable(it => it.CID == haza.CID).OrderBy(it => it.Sort);

                List<CarcheckitemdetailEntity> DetailList = new List<CarcheckitemdetailEntity>();
                foreach (CarcheckitemmodelEntity item in modelList)
                {
                    CarcheckitemdetailEntity detail = new CarcheckitemdetailEntity();
                    detail.CheckStatus = 0;
                    detail.CheckItem = item.CheckItem;
                    detail.Sort = item.Sort;
                    detail.CreateUserId = "System";
                    detail.CreateDate = DateTime.Now;
                    detail.CreateUserDeptCode = "00";
                    detail.CreateUserOrgCode = "00";
                    detail.ID = Guid.NewGuid().ToString();
                    detail.PID = entity.ID;
                    DetailList.Add(detail);
                }
                res.Insert<CarcheckitemdetailEntity>(DetailList);
                //res.Insert<HazardouscarEntity>(entity);
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
            }
        }


        /// <summary>
        /// 保存跟随人员及人脸图片
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="userjson"></param>
        public void SaveFaceUserForm(string keyValue, HazardouscarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string HazardousId = entity.HazardousId;
                Repository<CarcheckitemhazardousEntity> inlogdb = new Repository<CarcheckitemhazardousEntity>(DbFactory.Base());
                CarcheckitemhazardousEntity haza = inlogdb.FindEntity(it => it.HazardousId == HazardousId);

                Repository<CarcheckitemmodelEntity> modeldb = new Repository<CarcheckitemmodelEntity>(DbFactory.Base());
                IEnumerable<CarcheckitemmodelEntity> modelList = modeldb.IQueryable(it => it.CID == haza.CID).OrderBy(it => it.Sort);

                List<CarcheckitemdetailEntity> DetailList = new List<CarcheckitemdetailEntity>();
                foreach (CarcheckitemmodelEntity item in modelList)
                {
                    CarcheckitemdetailEntity detail = new CarcheckitemdetailEntity();
                    detail.CheckStatus = 0;
                    detail.CheckItem = item.CheckItem;
                    detail.Sort = item.Sort;
                    detail.CreateUserId = "System";
                    detail.CreateDate = DateTime.Now;
                    detail.CreateUserDeptCode = "00";
                    detail.CreateUserOrgCode = "00";
                    detail.ID = Guid.NewGuid().ToString();
                    detail.PID = entity.ID;
                    DetailList.Add(detail);
                }
                res.Insert<CarcheckitemdetailEntity>(DetailList);
                //res.Insert<HazardouscarEntity>(entity);
                res.Commit();
                SaveUserFileImgForm(entity, userjson);
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }

        /// <summary>
        /// 保存随行人员信息
        /// </summary>
        /// <param name="entity">主记录</param>
        /// <param name="userjson">随行人员集合</param>
        public void SaveUserFileImgForm(HazardouscarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<CarUserFileImgEntity> list = new List<CarUserFileImgEntity>();
                for (int i = 0; i < userjson.Count; i++)
                {
                    CarUserFileImgEntity uentity = new CarUserFileImgEntity();
                    if (!string.IsNullOrEmpty(entity.Dirver))
                    {//跟随人员
                        uentity.Create();
                        uentity.Username = userjson[i].Username;
                        uentity.Userimg = userjson[i].Userimg;
                        uentity.Imgdata = userjson[i].Imgdata;
                        uentity.Baseid = entity.ID;
                        uentity.ID = Guid.NewGuid().ToString();
                        uentity.CreateDate = DateTime.Now;
                        uentity.OrderNum = i;
                        entity.AccompanyingPerson = entity.AccompanyingPerson + userjson[i].Username + ",";
                        list.Add(uentity);
                    }
                }
                entity.AccompanyingNumber = userjson.Count;
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

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void Update(string keyValue, HazardouscarEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    Repository<CarUserFileImgEntity> caruserdb = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                    List<CarUserFileImgEntity> list = caruserdb.IQueryable(it => it.Baseid == keyValue).ToList();
                    entity.Modify(keyValue);
                    BaseRepository().Update(entity);
                    //if (entity.State == 3)
                    //{//审批通过 (下发入厂权限)
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
                    {//拒绝入场
                        DeleteUserHiK(list);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 改变GPS绑定信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, HazardouscarEntity entity, List<PersongpsEntity> pgpslist)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //将该车1小时内的打卡记录都变为已进场
                Repository<HazardouscarEntity> inlogdb = new Repository<HazardouscarEntity>(DbFactory.Base());
                HazardouscarEntity old = inlogdb.FindEntity(keyValue);
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
                            pgpslist[i].Type = 0;
                            pgpslist[i].VID = old.ID;
                            pgpslist[i].UserName = pgpslist[i].UserName.Substring(0, pgpslist[i].UserName.IndexOf(':'));
                            pgpslist[i].Create();

                        }
                        res.Insert<PersongpsEntity>(pgpslist);
                    }

                    CargpsEntity cgps = new CargpsEntity();
                    cgps.AID = old.ID;
                    cgps.CarNo = old.CarNo;
                    cgps.GpsId = old.GPSID;
                    cgps.GpsName = old.GPSNAME;
                    cgps.Status = 0;
                    cgps.Type = 1;
                    cgps.Create();

                    res.Insert<CargpsEntity>(cgps);
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
                    if (isupdate)
                    {
                        Repository<CargpsEntity> Carinlogdb = new Repository<CargpsEntity>(DbFactory.Base());
                        CargpsEntity Car = Carinlogdb.IQueryable(it => it.AID == old.ID && it.Status == 0).FirstOrDefault();
                        Car.GpsId = old.GPSID;
                        Car.GpsName = old.GPSNAME;
                        Car.Modify(Car.ID);
                        res.Update<CargpsEntity>(Car);
                    }
                }
                res.Update<HazardouscarEntity>(old);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 改变危化品车辆数据状态位交接完成
        /// </summary>
        /// <param name="id"></param>
        public void ChangeProcess(string id)
        {
            HazardouscarEntity hazard = BaseRepository().FindEntity(id);
            Operator user = OperatorProvider.Provider.Current();
            hazard.HandoverName = user.UserName;
            hazard.HandoverId = user.UserId;
            hazard.HandoverSign = user.SignImg;
            hazard.HazardousProcess = 2;
            hazard.Modify(id);
            BaseRepository().Update(hazard);
        }

        #region 数据同步及权限下发到海康平台

        /// <summary>
        /// 将拜访人员信息上传到海康平台
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        public void UploadUserHiK(HazardouscarEntity entity, List<CarUserFileImgEntity> list)
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
                var no = Str.PinYin(item.Username).ToUpper() + time;//卡号唯一
                //人脸信息（base64必须为jpg格式）
                List<FaceEntity> faces = new List<FaceEntity>();
                FaceEntity face = new FaceEntity();
                face.faceData = item.Imgdata;
                faces.Add(face);
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

                string msg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
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
            var no = time + Str.PinYin(Item.Username).ToUpper();//卡号唯一
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
        private void UploadUserlimits(List<CarUserFileImgEntity> list, string baseUrl, string Key, string Signature)
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
                    downloadUserlimits(resourceInfos, baseUrl, Key, Signature);
                }
            }
        }

        /// <summary>
        /// 根据出入权限配置快捷下载(IC卡号、人脸)
        /// </summary>
        private void downloadUserlimits(List<resourceInfos1> resourceInfos, string baseUrl, string Key, string Signature)
        {
            var url = "/artemis/api/acps/v1/authDownload/configuration/shortcut";
            var model = new
            {
                taskType = 5,
                resourceInfos
            };
            string msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
        }


        /// <summary>
        /// 离场或拒绝入场删除海康平台人员信息
        /// </summary>
        /// <param name="list">离厂人员</param>
        /// <param name="type">0拒绝入场 1人员离厂</param>
        public void DeleteUserHiK(List<CarUserFileImgEntity> list, int type = 0)
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

        /// <summary>
        /// 删除对应设备中出入权限配置
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
                    msg = SocketHelper.LoadCameraList(model, baseUrl, url, Key, Signature);
                }
            }
        }


        #endregion

        #endregion
    }
}
